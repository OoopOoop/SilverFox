using GalaSoft.MvvmLight.Command;
using Main.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Main.ViewModels
{
    public class AddViewModel:ViewModelBase
    {
        private List<ServiceItem> _selectedService;
        public ICommand SaveServicesCommand { get; set; }

        private ObservableCollection<ServiceItem> _runningServicesCollection;
        public ObservableCollection<ServiceItem> RunningServicesCollection
        {
            get { return _runningServicesCollection; }
            set { _runningServicesCollection = value; OnPropertyChanged(); }
        }

        public AddViewModel()
        {
            SaveServicesCommand = new RelayCommand<object>(SaveServices);
            RunningServicesCollection = getRunningServices();
            _selectedService = new List<ServiceItem>();
        }

        //Save and serialize selected services, redirect to the main page and pass the services
        private async void  SaveServices(object obj)
        {
            var services = obj as IEnumerable;
            if(services!=null)
            {
                foreach (ServiceItem item in services)
                {
                    _selectedService.Add(item);
                }
            }
           
            
            
             
           await ServiceManager.SetSavedServiceItems(_selectedService);
        }

        //Get collection of all running services
        private ObservableCollection<ServiceItem> getRunningServices()
        {
            return ServiceManager.GetAllServiceItems();
        }
    }
}
