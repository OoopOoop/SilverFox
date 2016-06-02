using Main.Shared;
using System.Xml.Serialization;

namespace Main.Models
{
    public class ServiceItem : NotifyService
    {
        private string serviceName;   
        private string displayName;  
        private string status;   
       // private bool canStop;   
        private string description;
        private string startMode;

        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; OnPropertyChanged(); }
        }

        public string ServiceName
        {
            get { return serviceName; }
            set { serviceName = value; OnPropertyChanged(); }
        }

        [XmlIgnore]
        public string Status
        {
            get { return status; }
            set { status = value; OnPropertyChanged(); }
        }

        //[XmlIgnore]
        //public bool CanStop
        //{
        //    get { return canStop; }
        //    set { canStop = value; OnPropertyChanged(); }
        //}

        public string Description
        {
            get { return description; }
            set { description = value; OnPropertyChanged(); }
        }


        [XmlIgnore]
        public string StartMode
        {
            get { return startMode; }
            set { startMode = value; OnPropertyChanged(); }
        }
    }
}