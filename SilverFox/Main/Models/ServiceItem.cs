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

namespace Main.Models
{
   public class ServiceItem
    {
        public string ServiceName { get; set; }
        [IgnoreDataMember]
        public string Status { get; set; }
        [IgnoreDataMember]
        public string DisplayName { get; set; }
        [IgnoreDataMember]
        public bool CanStop { get; set; }
        [IgnoreDataMember]
        public string Description { get; set;}
        [IgnoreDataMember]
        public string StartMode { get; set;}
    }
}
