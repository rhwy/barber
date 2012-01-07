using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Args;
using System.ComponentModel;

namespace ArtOfNet.ACR
{
    [ArgsModel(SwitchDelimiter = "-")]
    public class HelpArgumentModel
    {
        [Description("help options : if defails defined, print more details for a specific command")]
        [ArgsMemberSwitch("d", "details")]
        [DefaultValue("all")]
        public string InstallOptions { get; set; }
    }
    
}
