using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

using System.Linq;
using System.Text;

using System.IO.Pipes;
using System.IO;
using System.Threading;

namespace ArtOfNet.NamedPipesHelper
{
    public class ReceivedMessageEventArgs : EventArgs
    {

        public ReceivedMessageEventArgs(string message)
        {
            Message = message;
        }

        public string Message { get; set; }
    }
}
