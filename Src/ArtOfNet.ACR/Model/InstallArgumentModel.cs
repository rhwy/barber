using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Args;

namespace ArtOfNet.ACR
{
   
    [ArgsModel(SwitchDelimiter = "-")]
    public class InstallArgumentModel
    {
        [Description("Install options : if defined, register the app in path and/or the powershell snapin")]
        [ArgsMemberSwitch("p","path")]
        [DefaultValue(".\\")]
        public string RegisterPath { get; set; }
    }
}
