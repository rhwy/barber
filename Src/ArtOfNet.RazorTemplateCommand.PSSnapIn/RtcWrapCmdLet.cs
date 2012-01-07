using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management.Automation;
using System.IO;
using System.Reflection;
using NLog;

namespace ArtOfNet.RazorTemplateCommand.PSSnapIn
{
    [Cmdlet(VerbsCommon.Get, "barberpipe")]
    public class RtcCmdCmdLet : Cmdlet
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        RtcWrapperHelper _helper = new RtcWrapperHelper();

        protected override void ProcessRecord()
        {
            if (Info)
            {
                WriteObject(Assembly.GetAssembly(typeof(RtcCmdCmdLet)).FullName);
                return;
            }
            try
            {
                _logger.Info("Model = {0}",Model);
                _logger.Info("Template = {0}", Template);

                string result = _helper.MakePipedCommand(Template, Model);
                _logger.Info("Result = {0}",result);
                WriteObject(result);
                
            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "rtc.exe not found", ErrorCategory.InvalidData, Model));
            }
        }

        [Parameter]
        [Alias("i")]
        public SwitchParameter Info { get; set; }

        [Parameter]
        [Alias("m", "mc")]
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
        [Alias("o", "of", "result")]
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
