using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Args;
using System.ComponentModel;

namespace ArtOfNet.ACR
{
    
    [ArgsModel(SwitchDelimiter = "-")]
    public class ServerModeArgumentModel
    {
        [Description("Define the run mode")]
        [DefaultValue(ServerMode.StandAlone)]
        [ArgsMemberSwitch(0)]
        public ServerMode ServerMode { get; set; }

        [Description("Define the run mode start options")]
        [ArgsMemberSwitch("o", "options")]
        [DefaultValue("")]
        public string StartOptions { get; set; }
    }
}
