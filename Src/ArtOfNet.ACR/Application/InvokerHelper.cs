using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using NLog;

namespace ArtOfNet.ACR
{
    public static class InvokerHelper
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public static object GetGenericMethod(object source, Type genericType, string methodName, params object[] parameters)
        {
            Type sourceType = source.GetType();
            MethodInfo method = sourceType.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (method != null)
            {
                MethodInfo generic = method.MakeGenericMethod(genericType);
                if (generic != null)
                {
                    return generic.Invoke(source, parameters);
                }
            }
            return null;
        }

        public static object InvokeSpecialMethod(object source,string methodName)
        {
            return InvokeSpecialMethod(source,methodName, null);
        }

        public static object InvokeSpecialMethod(object source, string methodName, object[] parameters)
        {
            Type sourceType = source.GetType();
            MethodInfo method = sourceType.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (method != null)
            {
               return method.Invoke(source, parameters);
            }
            return null;
        }

    }
}
