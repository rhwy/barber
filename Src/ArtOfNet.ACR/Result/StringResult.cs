using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtOfNet.ACR;

namespace ArtOfNet.ACR
{
    public class StringResult : ControllerResult
    {
        public string Model {get;private set;}
        public bool Paused {get;private set;}

        public StringResult(string model):this(model,false)
        {
        }
        public StringResult(string model, bool paused)
        {
            Model = model;
            Paused = paused;
        }
        public override void ExecuteResult()
        {
            Console.WriteLine(Model);
        }
    }
}
