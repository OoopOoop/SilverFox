using GalaSoft.MvvmLight.Command;
using Main.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Main.ViewModels
{
    public class AddViewModel:ViewModelBase
    {
        private List<ServiceItem> _selectedServices;
        public ICommand SaveServicesCommand { get; set; }

        private ObservableCollection<ServiceItem> _runningServicesCollection;
        public ObservableCollection<ServiceItem> RunningServicesCollection
        {
            get { return _runningServicesCollection; }
            set { _runningServicesCollection = value; OnPropertyChanged(); }
        }

        public AddViewModel()
        {
            SaveServicesCommand = new RelayCommand(SaveServices);
            RunningServicesCollection = getRunningServices();
        }

        //Save and serialize selected services, redirect to the main page and pass the services
        private async void  SaveServices()
        {
            await ServiceManager.SetSavedServiceItems(_selectedServices);
        }

        //Get collection of all running services
        private ObservableCollection<ServiceItem> getRunningServices()
        {
            return ServiceManager.GetAllServiceItems();
        }
    }
}
