using Main.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.ViewModels
{
    public class AddViewModel:ViewModelBase
    {

        private ObservableCollection<ServiceItem> _runningServicesCollection;
        public ObservableCollection<ServiceItem> RunningServicesCollection
        {
            get { return _runningServicesCollection; }
            set { _runningServicesCollection = value; OnPropertyChanged(); }
        }

        public AddViewModel()
        {
           
            RunningServicesCollection = getRunningServices();
        }

        private ObservableCollection<ServiceItem> getRunningServices()
        {
            return ServiceManager.GetAllServiceItems();
        }
    }
}
