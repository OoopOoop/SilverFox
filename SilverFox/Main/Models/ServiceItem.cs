using Main.Shared;
using System.Xml.Serialization;

namespace Main.Models
{
    public class ServiceItem : NotifyClass
    {
        private string serviceName;

        [XmlIgnore]
        private string displayName;

        [XmlIgnore]
        private string status;

        [XmlIgnore]
        private bool canStop;

        [XmlIgnore]
        private string description;

        [XmlIgnore]
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

        public string Status
        {
            get { return status; }
            set { status = value; OnPropertyChanged(); }
        }

        public bool CanStop
        {
            get { return canStop; }
            set { canStop = value; OnPropertyChanged(); }
        }

        public string Description
        {
            get { return description; }
            set { description = value; OnPropertyChanged(); }
        }

        public string StartMode
        {
            get { return startMode; }
            set { startMode = value; OnPropertyChanged(); }
        }
    }
}