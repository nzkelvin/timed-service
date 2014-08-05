using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace TimedService.Config
{
    public class ConfigurationHelper : IConfigurationHelper
    {
        /// <summary>
        /// Retrieve a typed configuration section from the default configuration file.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        public T GetSection<T>(string sectionName) where T: class
        {
            var section = ConfigurationManager.GetSection(sectionName) as XmlNode;

            if (section != null)
            {
                Type type = typeof(T);
                var xmlSerializer = new XmlSerializer(type);
                var deserializedConfigObject = xmlSerializer.Deserialize(new StringReader(section.OuterXml));
                return deserializedConfigObject as T;
            }

            return null;
        }

        /// <summary>
        /// Retrieve a typed configuration section from the default configuration file.
        /// When the configuration type name is the same as configuration section name. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetSection<T>() where T : class
        {
            return GetSection<T>(typeof(T).Name);
        }
    }
}
