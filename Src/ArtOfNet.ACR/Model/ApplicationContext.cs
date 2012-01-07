using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtOfNet.ACR
{
    public class ApplicationContext
    {
        private static ApplicationContext _current;
        public static ApplicationContext Current
        {
            get
            {
                return _current;
            }
            set
            {
                if (_current == null)
                {
                    _current = value;
                }
                else
                {
                    throw new InvalidOperationException("Application Context can be only set once");
                }
            }
        }
        public string[] Args {get; set;}

        public ServerModeArgumentModel StartMode { get; set; }

        public ApplicationContext(string[] args)
        {
            Args = args;
            if (ApplicationContext.Current == null)
            {
                ApplicationContext.Current = this;
            }
        }
    }
}
