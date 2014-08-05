using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimedService.Config
{
    public class CustomSectionHandler : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            return section;
        }
    }
}
