using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace ArtOfNet.RazorTemplateCommand.Configuration
{
    
    public class RazorCommandConfigurationSection : ConfigurationSection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RazorCommandConfigurationSection"/> class.
        /// </summary>
        protected RazorCommandConfigurationSection()
        {
        }

        /// <summary>
        /// Section path in configuration file.
        /// </summary>
        private static string SectionName = "RazorCommand";

        /// <summary>
        /// This property represents the <code>Referenced Assemblies to include in generated Razor templates </code> subsection 
        /// in the configuration file.
        /// </summary>
        /// <value><see cref="ReferencedAssembliesConfigurationCollection"/></value>
        [ConfigurationProperty("IncludedAssemblies")]
        [ConfigurationCollection(typeof(ReferencedAssembliesConfigurationItem),
            AddItemName = "add",
            RemoveItemName = "remove",
            ClearItemsName = "clear")]
        public ReferencedAssembliesConfigurationCollection IncludedAssemblies
        {
            get { return (ReferencedAssembliesConfigurationCollection)base["IncludedAssemblies"]; }
            set { base["IncludedAssemblies"] = value; }
        }
        

        /// <summary>
        /// This property represents the <code>properties used by the embedded Razor engine </code> subsection 
        /// in the configuration file.
        /// </summary>
        /// <value><see cref="ReferencedAssembliesConfigurationCollection"/></value>
        [ConfigurationProperty("Properties")]
        [ConfigurationCollection(typeof(RazorCommandConfigurationItem),
            AddItemName = "add",
            RemoveItemName = "remove",
            ClearItemsName = "clear")]
        public RazorCommandConfigurationCollection RazorProperties
        {
            get { return (RazorCommandConfigurationCollection)base["Properties"]; }
            set { base["Properties"] = value; }
        }
        /// <summary>
        /// Gets the full type name with the specified mapping key.
        /// </summary>
        /// <value></value>
        public new RazorCommandConfigurationItem this[string key]
        {
            get
            {
                return RazorProperties[key];
            }
        }

        /// <summary>
        /// Gets the current service mapping configuration.
        /// </summary>
        /// <value>The current.</value>
        public static RazorCommandConfigurationSection Current
        {
            get { return (RazorCommandConfigurationSection)ConfigurationManager.GetSection(SectionName); }
        }
    }
}
