using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TimedService.Config
{
    [XmlRoot("TimedServices")]
    public class TimedServicesConfig
    {
        [XmlElement("LogFilePath")]
        public string LogFilePath { get; set; }

        [XmlElement("TimedServiceCommand")]
        public TimedServiceCommandConfig[] TimedServiceCommandConfigs { get; set; }
    }
}
