using Main.Shared;
using System.Xml.Serialization;

namespace Main.Models
{
    public class ServiceItem : NotifyService
    {
        private string serviceName;   
        private string displayName;  
        private string status;   
        private string description;
        private string startMode;
        private string originalDescription;
        private string originalDisplayName;
        private string originalStartMode;



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

       
        public string OriginalDescription
        {
            get { return originalDescription; }
            set { originalDescription = value; }
        }

      
        public string OriginalDisplayName
        {
            get { return originalDisplayName; }
            set { originalDisplayName = value; }
        }

      
        public string  OriginalStartMode
        {
            get { return originalStartMode; }
            set { originalStartMode = value; }
        }



        [XmlIgnore]
        public string Status
        {
            get { return status; }
            set { status = value; OnPropertyChanged(); }
        }

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