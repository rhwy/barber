using System.CodeDom.Compiler;

namespace ArtOfNet.RazorTemplateCommand.Framework
{
    public class TemplateParser
    {
        public string FileName { get; set; }
        public string Content { get; set; }
        public CompilerErrorCollection Errors { get; set; }
        public string GeneratedSource { get; set; }
        public string Result { get; set; }
    }
}
