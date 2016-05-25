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
   public class ServiceItem:ViewModelBase
    {
       // public string ServiceName { get; set; }    
       // public string Status { get; set; }
        //[XmlIgnore]
       // public string DisplayName { get; set;}
        //[XmlIgnore]
        //public bool CanStop { get; set; }
        //[XmlIgnore]
        //public string Description { get; set;}
        //[XmlIgnore]
        //public string StartMode { get; set;}


        [XmlIgnore]
        private string displayName;

        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; OnPropertyChanged(); }
        }

        private string serviceName;

        public string ServiceName
        {
            get { return serviceName; }
            set { serviceName = value; OnPropertyChanged(); }
        }

        [XmlIgnore]
        private string status;

        public string Status
        {
            get { return status; }
            set { status = value; OnPropertyChanged(); }
        }

        [XmlIgnore]
        private bool canStop;

        public bool CanStop
        {
            get { return canStop; }
            set { canStop = value; OnPropertyChanged(); }
        }

        [XmlIgnore]
        private string description;

        public string Description
        {
            get { return description; }
            set { description = value; OnPropertyChanged(); }
        }

        [XmlIgnore]
        private string startMode;

        public string StartMode
        {
            get { return startMode; }
            set { startMode = value; OnPropertyChanged(); }
        }


    }
}
