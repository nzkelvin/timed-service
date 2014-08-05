using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace TimedService.Config
{
    public class TimedServiceCommandConfig
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public string Type { get; set; }

        [XmlElement]
        public DateTime? RunAt { get; set; }

        [XmlElement]
        public DateTime? RunEvery { get; set; }

        [XmlElement("Parameter")]
        public TimedServiceCommandParameter[] Parameters { get; set; }
    }
}
