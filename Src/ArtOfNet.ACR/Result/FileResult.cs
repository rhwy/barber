using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ArtOfNet.ACR
{
    public class FileResult : ControllerResult
    {
        public string Content {get; private set;}
        public string Path { get; private set; }

        public FileResult(string content, string path)
        {
            Content = content;
            Path = path;
        }

        public override void ExecuteResult()
        {
 	        File.WriteAllText(Path,Content);
        }
    }
}
