using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace ArtOfNet.RazorTemplateCommand.Configuration
{
    public class ReferencedAssembliesConfigurationItem : ConfigurationElement
    {
        /// <summary>
        /// Gets or sets the web base url of an external search service.
        /// </summary>
        /// <value>The path.</value>
        [ConfigurationProperty("Name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string)base["Name"]; }
            set { base["Name"] = value; }
        }
    }
}
