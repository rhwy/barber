using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Timers;


namespace ArtOfNet.NamedPipesHelper
{
    public class PipedChildProcess : IDisposable
    {
        public PipedChildProcess()
        {
            Errors = new List<string>();
        }

        #region IDisposable Members

        public void Dispose()
        {
           _namedPipeClient = null;
           if(_childProcess != null) _childProcess.Dispose();
        }

        #endregion

        private Process _childProcess;
        private NamedPipeClient _namedPipeClient;
        
        private static readonly object _sync = new object();

        public string FileName { get; set; }
        public string Arguments { get; set; }
        public string PipeName { get; set; }

        public bool HasErrors
        {
            get
            {
                return (Errors.Count > 0);
            }
        }
        public List<string> Errors {get; private set;}
        public bool Start()
        {
            if (string.IsNullOrEmpty(FileName))
            {
                Errors.Add("Process Filename can not be null or empty");
                return false;
            }
            if (string.IsNullOrEmpty(PipeName))
            {
                Errors.Add("PipeName can not be null or empty");
                return false;
            }

            _childProcess = new Process();
            _childProcess.StartInfo.FileName = FileName;
            _childProcess.StartInfo.Arguments = Arguments;
            _childProcess.StartInfo.UseShellExecute = false;
            _childProcess.StartInfo.RedirectStandardOutput = false;
            if (!_childProcess.Start())
            {
                Errors.Add("Process could not be started");
                return false;
            }

            _namedPipeClient = new NamedPipeClient(PipeName);
            _namedPipeClient.OnReceivedMessage += new EventHandler<ReceivedMessageEventArgs>(client_OnReceivedMessage);
            _namedPipeClient.Start();
            return true;
        }

        private bool _waitForCommand = true;
        private string _runResult = string.Empty;
        void client_OnReceivedMessage(object sender, ReceivedMessageEventArgs e)
        {
            lock (_sync)
            {
                int CrlfPosition = e.Message.LastIndexOf("\r\n");
                if(CrlfPosition>0)
                    _runResult = e.Message.Remove(CrlfPosition);
                else
                    _runResult = e.Message;
                _waitForCommand = false;
            }
            
        }

        public string Run(params string[] args)
        {
            if (args != null && args.Length > 0)
            {
                foreach (string arg in args)
                {
                    _namedPipeClient.Write(arg);
                }
                _namedPipeClient.Write("EXEC");
            }
            else
            {
                Errors.Add("Pipe arguments can not be null");
                _waitForCommand = false;
            }

            Timer t = new Timer(1000);
            t.Elapsed += new ElapsedEventHandler(t_Elapsed);
            t.Start();
            while (_waitForCommand) { }
            return _runResult;
        }

        void t_Elapsed(object sender, ElapsedEventArgs e)
        {
            _waitForCommand = false;
        }

    }
}
