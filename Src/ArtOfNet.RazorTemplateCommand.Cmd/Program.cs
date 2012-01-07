using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using ArtOfNet.RazorTemplateCommand.Framework;
using System.Diagnostics;
using Args.Help.Formatters;
using Args;
using System.IO.Pipes;

namespace ArtOfNet.RazorTemplateCommand.Cmd
{
    
    class Program
    {
        static void Main(string[] args)
        {
            var application = new RtcApplication(args);
            application.Run();
        }

    }
}
