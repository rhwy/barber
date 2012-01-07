using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace ArtOfNet.RazorTemplateCommand.Configuration
{
    public class RazorCommandConfigurationCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// When overridden in a derived class, creates a new <see cref="T:System.Configuration.ConfigurationElement"/>.
        /// </summary>
        /// <returns>
        /// A new <see cref="T:System.Configuration.ConfigurationElement"/>.
        /// </returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new RazorCommandConfigurationItem();
        }

        /// <summary>
        /// Gets the element key for a specified configuration element when overridden in a derived class.
        /// </summary>
        /// <param name="element">The <see cref="T:System.Configuration.ConfigurationElement"/> to return the key for.</param>
        /// <returns>
        /// An <see cref="T:System.Object"/> that acts as the key for the specified <see cref="T:System.Configuration.ConfigurationElement"/>.
        /// </returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ( (RazorCommandConfigurationItem)element ).Name;
        }

        /// <summary>
        /// Gets the <see cref="RazorCommandConfigurationItem"/> with the specified key.
        /// </summary>
        /// <value></value>
        public new RazorCommandConfigurationItem this[string key]
        {
            get { return (RazorCommandConfigurationItem)BaseGet(key); }
        }
    }
}
