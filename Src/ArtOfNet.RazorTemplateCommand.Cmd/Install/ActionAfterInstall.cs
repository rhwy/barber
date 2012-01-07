using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.IO;

namespace ArtOfNet.RazorTemplateCommand.Cmd
	{
	[RunInstaller(true)]
	public class ActionAfterInstall : Installer
		{

		public override void Install(System.Collections.IDictionary stateSaver)
			{
			base.Install(stateSaver);
		   
			}
		protected override void OnAfterInstall(System.Collections.IDictionary savedState)
			{
				base.OnAfterInstall(savedState);
				string targetDirectory = Context.Parameters["targetdir"];
                CustomRegister(targetDirectory);
				
			}

        public void CustomRegister(string targetdir)
            {
            string targetDirectory = targetdir;
            RegistrationHelper reg = new RegistrationHelper(targetDirectory);

            reg.LogLine("=============================================================");
            reg.LogLine("Razor Template Command - registration ");
            reg.LogLine("=============================================================");

            reg.LogLine(string.Format("Started at : {0}", DateTime.Now));

            if (reg.RegisterSnapIn())
                {
                reg.LogLine("registration complete");
                }
            else
                {
                reg.LogLine("Registration uncomplete");
                }
            reg.LogLine(string.Format("Finished at : {0}", DateTime.Now));
            reg.Save("Install.log");
            }
		}

		
	}
