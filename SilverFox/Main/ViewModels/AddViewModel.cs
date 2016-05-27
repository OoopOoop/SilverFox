﻿using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Main.Models;
using Main.Shared;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Main.ViewModels
{
    public class AddViewModel
    {
        private ObservableCollection<ServiceItem> _selectedService;
        public ICommand SaveServicesCommand { get; set; }

        private ObservableCollection<ServiceItem> _runningServicesCollection;
        public ObservableCollection<ServiceItem> RunningServicesCollection
        {
            get { return _runningServicesCollection; }
            set { _runningServicesCollection = value; }
        }

        private IFrameNavigationService _navigationService;

        public AddViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;

            SaveServicesCommand = new RelayCommand<object>(sendToMainServices);
            RunningServicesCollection = getRunningServices();
            _selectedService = new ObservableCollection<ServiceItem>();
        }

        //Save and serialize selected services, redirect to the main page and pass the services
        private  void  sendToMainServices(object obj)
        {
            var services = obj as IEnumerable;
            if(services!=null)
            {
                foreach (ServiceItem item in services)
                {
                    _selectedService.Add(item);
                }
            }
           
           
           //await ServiceManager.SetSavedServiceItems(_selectedService.ToList());

           Messenger.Default.Send(_selectedService);
            _navigationService.NavigateTo("MainWindow");
            _selectedService.Clear();

        }

        //Get collection of all running services
        private ObservableCollection<ServiceItem> getRunningServices()
        {
            return ServiceManager.GetAllServiceItems();
        }
    }
}
