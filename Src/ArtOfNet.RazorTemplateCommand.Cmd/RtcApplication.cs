using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtOfNet.ACR;

namespace ArtOfNet.RazorTemplateCommand.Cmd
{
    public class RtcApplication : CommandLineApplication
    {
        public RtcApplication():base()
        {
        }

        public RtcApplication(string[] args):base(args)
        {
        }

        protected void ApplicationStart()
        {
            Console.WriteLine(MessageHelper.GetHeader());
        }
    }
}
