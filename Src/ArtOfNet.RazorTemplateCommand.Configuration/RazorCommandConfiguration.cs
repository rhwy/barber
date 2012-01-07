using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtOfNet.RazorTemplateCommand.Configuration
{
    public static class RazorCommandConfiguration
    {
        #region Internal Default properties
        /// <summary>
        /// These properties are defined by default but can be override by 
        /// configuration in RazorCommand\Properties section
        /// </summary>
        /// 
        private const string DEFAULT_TEMP_FOLDER = @".\TempGenerated";
        private const string DEFAULT_TEMPLATE_BASE_CLASS = "AbstractTemplateBase";
        private const string DEFAULT_TEMPLATE_CLASS_NAME = "templateGenerated";
        private const string DEFAULT_TEMPLATE_NAMESPACE = "ArtOfNet.RazorTemplateCommand.Output";



        #endregion
       

        private static readonly object _syncroot = new object();

        private static List<string> _referencedAssemblies = null;
        private static Dictionary<string,string> _properties = null;

        public static List<string> ReferencedAssemblies
        {
            get
            {
                if (_referencedAssemblies == null)
                {
                    LoadReferencedAssembliesConfiguration();
                }
                return _referencedAssemblies;
            }
        }

        public static Dictionary<string, string> Properties
        {
            get
            {
                if (_properties == null)
                {
                    LoadPropertiesConfiguration();
                }
                return _properties;
            }
        }

        private static void LoadPropertiesConfiguration()
        {
            
            _properties = new Dictionary<string, string>();
            if (RazorCommandConfigurationSection.Current == null)
            {
                return;
            }

            lock (_syncroot)
            {
                foreach (RazorCommandConfigurationItem element in RazorCommandConfigurationSection.Current.RazorProperties)
                {
                    if (element != null)
                    {
                        _properties.Add(element.Name,element.Value);
                    }
                }
            }
        }

        private static void LoadReferencedAssembliesConfiguration()
        {
            _referencedAssemblies = new List<string>();
            if (RazorCommandConfigurationSection.Current == null)
            {
                return;
            }

            lock (_syncroot)
            {
                foreach (ReferencedAssembliesConfigurationItem element in RazorCommandConfigurationSection.Current.IncludedAssemblies)
                {
                    if (element != null)
                    {
                        _referencedAssemblies.Add(element.Name);
                    }
                }
            }
        }

        private static string _defaultTempFolder;

        public static string DefaultTempFolder
        {
            get
            {
                if(string.IsNullOrEmpty(_defaultTempFolder))
                {
                    LoadValueOrDefault("DefaultTempFolder",ref _defaultTempFolder,DEFAULT_TEMP_FOLDER);
                }
                return _defaultTempFolder;
            }
        }


        private static void LoadValueOrDefault(string key,ref string container, string defaultValue)
        {
            if(Properties.ContainsKey(key))
            {
                container = Properties[key];
            } 
            else
            {
                container = defaultValue;
            }
        }
    }
}
