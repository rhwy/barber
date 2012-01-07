using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Args;
using System.ComponentModel;

namespace ArtOfNet.ACR
{
    [ArgsModel(SwitchDelimiter="-")]
    public class ControllerSelectionArgumentModel
    {
        [DefaultValue("render")]
        [ArgsMemberSwitch(0)]
        public string ControllerName { get; set; }
    }
}
