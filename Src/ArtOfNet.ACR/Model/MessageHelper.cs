using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace ArtOfNet.ACR
{
    public static class MessageHelper
    {
        public static string GetHeader()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("=====================================================");
            sb.AppendLine("Razor Template Command ");
            sb.AppendLine("=====================================================");
            sb.AppendLine(" (c) 2011 - ArtOfNet.com - Rui Carvalho");
            sb.AppendLine("=====================================================");
            return sb.ToString();
        }
    }
}
