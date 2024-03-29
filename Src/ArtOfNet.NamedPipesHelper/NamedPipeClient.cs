using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

using System.Linq;
using System.Text;
using System.IO.Pipes;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace ArtOfNet.NamedPipesHelper
{
    public class NamedPipeClient : PipeStreamWrapperBase<NamedPipeClientStream>
    {
        public NamedPipeClient(string pipeName) : base(pipeName)
        {

        }

        protected override NamedPipeClientStream CreateStream()
        {
            var stream = new NamedPipeClientStream(".",
                             PipeName,
                             PipeDirection.InOut,
                             PipeOptions.Asynchronous,System.Security.Principal.TokenImpersonationLevel.Delegation);
            stream.Connect();
            stream.ReadMode = PipeTransmissionMode.Message;
            return stream;
        }

        protected override bool AutoFlushPipeWriter
        {
            get { return true; }
        }

        protected override void ReadFromPipe(object state)
        {
            try
            {
                while (Pipe != null && m_stopRequested == false)
                {
                    if (Pipe.IsConnected == true)
                    {
                        byte[] msg = ReadMessage(Pipe);

                        ThrowOnReceivedMessage(msg);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());                
            }
            finally
            {
                Debug.WriteLine("Client.Run() is exiting.");
            }
        }
    }
}
