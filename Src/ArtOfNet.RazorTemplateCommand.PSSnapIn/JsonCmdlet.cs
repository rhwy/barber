using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management.Automation;
using Newtonsoft.Json;
using System.IO;
using ArtOfNet.RazorTemplateCommand.PSSnapIn;

namespace ArtOfNet.RazorTemplateCommand
{
    
    [Cmdlet(VerbsCommon.Get, "RtcJson")]
    public class JsonCmdlet : Cmdlet
    {
        protected override void ProcessRecord()
        {
            try
            {
                string json = JsonConvert.SerializeObject(Value);
                if(!Quotes)
                    json = json.Replace(@"""", "'");
                WriteObject(json);
            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "json parse", ErrorCategory.InvalidData, Value));
            }
        }

        [Parameter]
        [Alias("v")]
        public object Value { get; set; }

        [Parameter]
        [Alias("q")]
        public SwitchParameter Quotes { get; set; }
    }

    

}
