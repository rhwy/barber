using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.IO.Pipes;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using System.Xml.Linq;
using ArtOfNet.NamedPipesHelper;

namespace ArtOfNet.RazorTemplateCommand.Client35IntegrationTest
{
   
    class Program
    {
        
        static void Main(string[] args)
        {
            
            
            TestWithTempFileResult();
        }

        static void TestWithNamedPipes()
        {
            string serverApp = Path.Combine("../../../../Src/ArtOfNet.RazorTemplateCommand.Cmd/bin/Debug/", "rtc.exe");

            if (!File.Exists(serverApp))
            {
                Console.WriteLine("{0} was not found");
                return;
            }

            using (PipedChildProcess process = new PipedChildProcess())
            {
                process.FileName = serverApp;
                process.Arguments = "-m NamedPipes";
                process.PipeName = "InProc";
                process.Start();
                var ano = new { Name = "Rui", List = new[] { "one", "two", "three" } };
                string value = JsonConvert.SerializeObject(ano);
                string result = process.Run("Render", "-tc", "Hello @Model.Name, values : @Model.List.Count", "-mc", value);
                Console.WriteLine("Templated result : {0}", result);

                Console.Read();
            }

        }

        static void TestWithTempFileResult()
        {
            string template = "With File : Hello @Model.Name, values : @Model.List.Count";
            var ano = new { Name = "Rui", List = new[] { "one", "two", "three" } };
            string value = JsonConvert.SerializeObject(ano);
            value = value.Replace(@"""", "'");
            //string serverApp = "rtc.exe";
            string serverApp = Path.Combine("../../../../Src/ArtOfNet.RazorTemplateCommand.Cmd/bin/Debug/", "rtc.exe");

            string result = string.Empty;
            string id = Guid.NewGuid().ToString();
            string tempDir = Path.Combine(Environment.CurrentDirectory,"Temp");
            if(!Directory.Exists(tempDir))
            {
                Directory.CreateDirectory(tempDir);
            }
            string resultFilePath = Path.Combine(tempDir,string.Format("Template_{0}.rr",id));

            Process process = new Process();
            process.StartInfo.FileName = serverApp;
            process.StartInfo.Arguments = string.Format(@"render -tc ""{0}"" -mc ""{1}"" -r ""{2}""",template,value,resultFilePath);
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = false;
            
            if (!process.Start())
            {
                result = "Process could not be started";
            }
            else
            {
                process.WaitForExit(1000);
                process.Close();
                if(File.Exists(resultFilePath))
                {
                    result = File.ReadAllText(resultFilePath);
                    File.Delete(resultFilePath);
                }
            }

            Console.WriteLine(result);
            Console.Read();
        }
        
    }
}
