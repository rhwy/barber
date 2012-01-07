using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using NLog;

namespace ArtOfNet.ACR
{
    public static class ControllerFactory
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        private static Dictionary<string,Type> _controllers = new Dictionary<string,Type>();

        public static Dictionary<string,Type> Controllers
        {
            get
            {
                return _controllers;
            }
        }

        static ControllerFactory()
        {
            _logger.Info("Building Controller Factory");

            try
            {
                var currentAssembly = Assembly.GetAssembly(typeof(ControllerFactory));
                _logger.Info("Assembly Info: {0}", currentAssembly.FullName);
                var controllers = currentAssembly
                                    .GetTypes()
                                    .Where(t => t.BaseType != null
                                            && t.BaseType.Name == "RtcControllerBase`1"
                                            && t.Name
                                                .ToLowerInvariant()
                                                .EndsWith("controller"));
                _logger.Info("Number of controllers found : {0}", controllers.Count());
                
                foreach (Type item in controllers)
                {
                    _controllers.Add(item.Name.ToLowerInvariant().Replace("controller", string.Empty), item);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
            
        }

        public static void Initialize()
        {}

        public static IRtcController GetController(ApplicationContext context)
        {

            return null;
        }
    }
}
