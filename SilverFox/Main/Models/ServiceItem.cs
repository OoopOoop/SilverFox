using Main.Shared;
using Main.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Runtime.Serialization;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Main.Models
{
   public class ServiceItem
    {
        public string ServiceName { get; set; }
        [XmlIgnore]
        public string Status { get; set; }
        [XmlIgnore]
        public string DisplayName { get; set; }
        [XmlIgnore]
        public bool CanStop { get; set; }
        [XmlIgnore]
        public string Description { get; set;}
        [XmlIgnore]
        public string StartMode { get; set;}
    }
}
