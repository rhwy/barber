using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Web.Razor;
using Microsoft.CSharp;
using System.IO;
using System.CodeDom.Compiler;
using System.Reflection;
using Newtonsoft.Json;
using ArtOfNet.RazorTemplateCommand.Configuration;
using NLog;
using FluentConfiguration.Core;
using ArtOfNet.FluentConfiguration.Core;
using System.Runtime.CompilerServices;

using System.CodeDom;

namespace ArtOfNet.RazorTemplateCommand.Framework
{
    public static class RazorEngineHelper
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public static RazorTemplateEngine SetupRazorEngine(
                string defaultBaseClass = "AbstractTemplateBase",
                string defaultNamespace = "ArtOfNet.RazorTemplateCommand.Framework",
                string defaultClassName = "templateGenerated")
        {
            return SetupRazorEngine(
                new List<string>(){
                    "System",
                    "System.Collections.Generic",
                    "System.Linq",
                    "System.Dynamic",
                    typeof(AbstractTemplateBase).Namespace},
                defaultBaseClass,
                defaultNamespace,
                defaultClassName);
        }

        /// <summary>
        /// Setup a new instance of a Razor template engine and return it
        /// </summary>
        /// <param name="defaultImports"></param>
        /// <param name="defaultBaseClass"></param>
        /// <param name="defaultNamespace"></param>
        /// <param name="defaultClassName"></param>
        /// <returns></returns>
        public static RazorTemplateEngine SetupRazorEngine(
                IEnumerable<string> defaultImports,
                string defaultBaseClass = "TemplateBase",
                string defaultNamespace = "ArtOfNet.RazorTemplateCommand.Framework",
                string defaultClassName = "templateGenerated"
            )
        {
            RazorEngineHost host = new RazorEngineHost(new CSharpRazorCodeLanguage());
            host.DefaultBaseClass = defaultBaseClass;
            host.DefaultNamespace = defaultNamespace;
            host.DefaultClassName = defaultClassName;
            foreach (var item in defaultImports)
            {
                host.NamespaceImports.Add(item);
            }
            return new RazorTemplateEngine(host);
        }


        /// <summary>
        /// Load a new engine instance and setup it up, parse template and generate code
        /// on a string form (only to get the source)
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        public static string GenerateSourceCode(string template)
        {
            GeneratorResults razorResult = GenerateSourceCodeResult(template);

            CSharpCodeProvider codeProvider = new CSharpCodeProvider();
            string result = string.Empty;

            using (StringWriter sw = new StringWriter())
            {
                codeProvider.GenerateCodeFromCompileUnit(razorResult.GeneratedCode, sw, new CodeGeneratorOptions());
                result = sw.GetStringBuilder().ToString();
            }
            return result;
        }

        /// <summary>
        /// Load a new engine instance and setup it up, parse template and generate code
        /// on a GeneratorResults form (this allows to execute it after)
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        private static GeneratorResults GenerateSourceCodeResult(string template)
        {
            RazorTemplateEngine engine = RazorEngineHelper.SetupRazorEngine();
            GeneratorResults razorResult = null;

            using (TextReader rdr = new StringReader(template))
            {
                razorResult = engine.GenerateCode(rdr);
            }
            return razorResult;
        }

        public static string ProcessTemplate(string template)
        {
            return ProcessTemplate(template,null);
        }

        public static string ProcessTemplate(string template, dynamic model)
        {
            GeneratorResults razorResult = GenerateSourceCodeResult(template);
            
            //try to make anonymous visible to dynamics
            var attrib = new CodeAttributeDeclaration(new CodeTypeReference(typeof(InternalsVisibleToAttribute)));
            attrib.Arguments.Add(new CodeAttributeArgument(new CodePrimitiveExpression("ArtOfNet.RazorTemplateCommand.Framework")));

            razorResult.GeneratedCode.AssemblyCustomAttributes.Add(attrib);

            string finalResult = null;

            CSharpCodeProvider codeProvider = new CSharpCodeProvider();


            // Compile the generated code into an assembly
            string outputFolder = Path.Combine(Environment.CurrentDirectory, RazorCommandConfiguration.DefaultTempFolder);
            DirectoryInfo tempFolder = new DirectoryInfo(outputFolder);
            if (!tempFolder.Exists)
            {
                tempFolder.Create();
            }

            string outputAssemblyName = Path.Combine(outputFolder, String.Format("Temp_{0}.dll", Guid.NewGuid().ToString("N")));
            CompilerParameters compilerParams = new CompilerParameters();

            RazorCommandConfiguration.ReferencedAssemblies.ForEach(
                asm => compilerParams.ReferencedAssemblies.Add(asm)
            );
            
            compilerParams.OutputAssembly = outputAssemblyName;
            compilerParams.ReferencedAssemblies.Add(typeof(AbstractTemplateBase).Assembly.CodeBase.Replace("file:///", "").Replace("/", "\\"));
            CompilerResults results = codeProvider.CompileAssemblyFromDom(
               compilerParams,
                razorResult.GeneratedCode);

            if (results.Errors.HasErrors)
            {
                CompilerError err = results.Errors
                                           .OfType<CompilerError>()
                                           .Where(ce => !ce.IsWarning)
                                           .First();
                finalResult = string.Format("Error Compiling Template: ({0}, {1}) {2}",
                                              err.Line, err.Column, err.ErrorText);
            }
            else
            {
                // Load the assembly
                Assembly asm = Assembly.LoadFrom(outputAssemblyName);
                AbstractTemplateBase currentTransformResult = null;
                if (asm == null)
                {
                    Console.WriteLine("Error loading template assembly");
                }
                else
                {
                    // Get the template type
                    Type typ = asm.GetType("ArtOfNet.RazorTemplateCommand.Framework.templateGenerated");
                    if (typ == null)
                    {
                        Console.WriteLine("APP-ERROR : Could not find type RazorOutput.Template in assembly {0}", asm.FullName);
                    }
                    else
                    {
                        AbstractTemplateBase newTemplate = Activator.CreateInstance(typ) as AbstractTemplateBase;
                        
                        if (newTemplate == null)
                        {
                            Console.WriteLine("Could not construct RazorOutput.Template or it does not inherit from TemplateBase");
                        }
                        else
                        {
                            if (model != null)
                            {
                                newTemplate.Model = (dynamic)model;
                            }
                        
                            currentTransformResult = newTemplate;
                        }
                    }
                }
                if (currentTransformResult != null)
                {
                    currentTransformResult.Execute();
                    finalResult = currentTransformResult.Buffer.ToString();
                }
            }
            return finalResult;
        }


        public static string ProcessGeneratedCode(string template, dynamic model)
        {
            return ProcessTemplate(template, model);
        }

        public static string ProcessTemplateWithJsonModel(string template, string json)
        {
            dynamic model = JsonConvert.DeserializeObject<ExpandoObject>(json);
            
            return ProcessTemplate(template, model);
        }

        public static string ProcessTemplateWithPropertiesModel(string template, string properties)
        {
            PropertiesReader reader = new PropertiesReader(properties);
            dynamic model = reader.GetProperties();
            return ProcessTemplate(template,model);
        }

        public static string ProcessTemplateWithXmlModel(string template, string xml)
        {
            dynamic model = new DynamicXmlElement(xml);
            return ProcessTemplate(template,model);
        }

        public static string ProcessTemplateWithAnonymous(string template, object model)
        {
            string json = JsonConvert.SerializeObject(model);
            return ProcessTemplateWithJsonModel(template, json);
        }
    }
}
