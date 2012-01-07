using System;
using System.Text;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Dynamic;

namespace ArtOfNet.RazorTemplateCommand.Framework
{
    public abstract class AbstractTemplateBase
    {
        #region template execution
        [Browsable(false)]
        public StringBuilder Buffer { get; set; }

        [Browsable(false)]
        public StringWriter Writer { get; set; }

        public FileHelper File { get; private set; }

        public AbstractTemplateBase()
        {
            Buffer = new StringBuilder();
            Writer = new StringWriter(Buffer);
            Model = new { };
            File = new FileHelper();
        }

        public abstract void Execute();

        public virtual void Write(object value)
        {
            WriteLiteral(value);
        }

        public virtual void WriteLiteral(object value)
        {
            Buffer.Append(value);
        }
        #endregion

        public dynamic Model { get; set; }


    }
}
