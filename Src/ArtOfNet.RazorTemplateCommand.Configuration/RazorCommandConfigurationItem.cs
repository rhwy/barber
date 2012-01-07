using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace ArtOfNet.RazorTemplateCommand.Configuration
{
    
    public class RazorCommandConfigurationItem : ConfigurationElement
    {
        /// <summary>
        /// Gets or sets the name of the property
        /// </summary>
        /// <value>The path.</value>
        [ConfigurationProperty("Name", IsKey = false, IsRequired = true)]
        public string Name
        {
            get { return (string)base["Name"]; }
            set { base["Name"] = value; }
        }

        /// <summary>
        /// Gets or sets the Value of the property
        /// </summary>
        /// <value>The path.</value>
        [ConfigurationProperty("Value", IsKey = false, IsRequired = true)]
        public string Value
        {
            get { return (string)base["Value"]; }
            set { base["Value"] = value; }
        }
    }
        
}
