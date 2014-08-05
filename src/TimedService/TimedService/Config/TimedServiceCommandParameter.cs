using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace TimedService.Config
{
    public class TimedServiceCommandParameter
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("value")]
        public string Value { get; set; }
        
        [XmlText]
        public string ValueBody { get; set; }
    }
}
