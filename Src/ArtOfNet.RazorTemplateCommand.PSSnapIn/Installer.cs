using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Management.Automation;

namespace ArtOfNet.RazorTemplateCommand
{

    [RunInstaller(true)]
    public class InstallerSnapIn : System.Management.Automation.PSSnapIn
        {
        // Name for the PowerShell snap-in.
        public override string Name
            {
            get
                {
                return "ArtOfNet.Powershell.RazorTemplateEngine";
                }
            }

        // Vendor information for the PowerShell snap-in.
        public override string Vendor
            {
            get
                {
                return "ArtOfNet";
                }
            }

        // Description of the PowerShell snap-in
        public override string Description
            {
            get
                {
                return "A simple powershell tool to render Razor templates";
                }
            }
        }
}
