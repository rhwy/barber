using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management.Automation;
using NLog;
using System.Reflection;
using System.IO;
using Newtonsoft.Json;
using ArtOfNet.RazorTemplateCommand.PSSnapIn;

namespace ArtOfNet.RazorTemplateCommand
{
    [Cmdlet(VerbsCommon.Get, "barber")]
    public class RazerCmdlet : Cmdlet
    {
        private RtcWrapperHelper _helper = new RtcWrapperHelper();
        
        protected override void ProcessRecord()
        {
            try
            {
                if (!string.IsNullOrEmpty(TemplateFile) && File.Exists(TemplateFile))
                {
                    Template = File.ReadAllText(TemplateFile);
                }
                if (!string.IsNullOrEmpty(ModelFile) && File.Exists(ModelFile))
                {
                    Model = JsonConvert.DeserializeObject(File.ReadAllText(ModelFile));
                }


                string result = _helper.RunCommand(Template, Model, Output,Type, Quotes.IsPresent, Debug.IsPresent);
                //_logger.Info("Result : " + result);
                File.WriteAllText("c:\\log.txt", result);
                WriteObject(result);

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "Internal RTC", ErrorCategory.InvalidResult, Model));
            }
        }

        [Parameter]
        [Alias("m","mc")]
        public object Model { get; set; }

        [Parameter]
        [Alias("tc")]
        public string Template { get; set; }

        [Parameter]
        [Alias("mf")]
        public string ModelFile { get; set; }

        [Parameter]
        [Alias("tf")]
        public string TemplateFile { get; set; }

        [Parameter]
        [Alias("o", "of","result")]
        public string Output { get; set; }

        [Parameter]
        [Alias("t")]
        public string Type { get; set; }

        [Parameter]
        [Alias("q")]
        public SwitchParameter Quotes { get; set; }

        [Parameter]
        [Alias("d")]
        public SwitchParameter Debug { get; set; }
    }
}
