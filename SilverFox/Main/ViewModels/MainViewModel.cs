using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Main.Models;
using Main.Shared;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System;
using System.Collections;
using System.Linq;

namespace Main.ViewModels
{
    public class MainViewModel 
    {
        private ObservableCollection<ServiceItem> _selectedServicesCollection;
        public ObservableCollection<ServiceItem> SelectedServicesCollection
        {
            get { return _selectedServicesCollection; }
            set { _selectedServicesCollection = value;}
        }


        private string status;

      

        public ICommand RefreshStatusCommand { get; set;}
        public ICommand RemoveServiceCommand { get; set;}
        public ICommand RefreshStatusAllCommand { get; set;}
        public ICommand StopServiceCommand { get; set;}
        public ICommand StartServiceCommand { get; set;}
        public ICommand ChangeStatusCommand { get; set;}
        public ICommand RadioBtnStatusCommand { get; set;}

        // get services from addWindow
        private void displaySelectedServices()
        {
            Messenger.Default.Register <Collection<ServiceItem>>(
                this,
                services =>
                {
                    foreach (ServiceItem  service in services)
                    {
                        SelectedServicesCollection.Add(service);
                    }
                });
        }


        public ICommand NavigateAddWindowCommand { get; set; }

        private  IFrameNavigationService _navigationService;

        public MainViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
            NavigateAddWindowCommand = new RelayCommand(GotoAddWindow);
            RefreshStatusCommand = new RelayCommand<object>(refreshStatus);
            RemoveServiceCommand = new RelayCommand<object>(removeFromCollection);
            RefreshStatusAllCommand = new RelayCommand(refreshAll);
            StopServiceCommand = new RelayCommand<object>(stopService);
            StartServiceCommand = new RelayCommand<object>(startService);

            RadioBtnStatusCommand = new RelayCommand<string>(setStatus);
            ChangeStatusCommand = new RelayCommand<object>(changeStatus);

            SelectedServicesCollection = new ObservableCollection<ServiceItem>();


            displaySelectedServices();
            refreshAll();
        }


        // get "automatic" "manual" or "disabled" status  from radiobuttons
        private void setStatus(string _status) => status = _status;
     


        private void changeStatus(object obj)
        {
            var servChngStatus = obj as IEnumerable;
            if (servChngStatus != null&&status!=String.Empty)
            {
                servChngStatus.Cast<ServiceItem>().ToList().ForEach(service =>
                {
                    ServiceManager.ChangeStartMode(service,status);
                    service.StartMode = ServiceManager.RefreshStatus(service).StartMode;
                });
            }
        }



        private void startService(object obj)
        {
            var servToStop = obj as IEnumerable;
            if (servToStop != null)
            {
                servToStop.Cast<ServiceItem>().ToList().ForEach(service =>
                {
                    ServiceManager.StartService(service);
                    service.Status = ServiceManager.RefreshStatus(service).Status;
                });
            }
        }

        private void stopService(object obj)
        {
            var servToStop = obj as IEnumerable;
            if (servToStop != null)
            {
                servToStop.Cast<ServiceItem>().ToList().ForEach(service =>
                {
                    ServiceManager.StopService(service);
                    service.Status=ServiceManager.RefreshStatus(service).Status;
                });
            }
        }


        //remove selected services from the datagrid
        private void removeFromCollection(object obj)
        {
            var servToRemove = obj as IEnumerable;
            if(servToRemove!=null)
            {
                servToRemove.Cast<ServiceItem>().ToList().ForEach(service =>
                {
                    SelectedServicesCollection.Remove(service);
                });
            }
        }


        //refresh all services in datagrid selected or not
        private void refreshAll()
        {
            if(SelectedServicesCollection.Count!=0)
            {
                foreach (ServiceItem service in SelectedServicesCollection)
                {
                    service.Status = ServiceManager.RefreshStatus(service).Status;
                }
            }
        }


        //Updates status of single or multiple selected services
        private void refreshStatus(object obj)
        {            
            var servToUpdate = obj as IEnumerable;

            if(servToUpdate!=null)
            {
                servToUpdate.Cast<ServiceItem>().ToList().ForEach(service =>
                {
                    SelectedServicesCollection[SelectedServicesCollection.IndexOf(service)].Status = ServiceManager.RefreshStatus(service).Status;
                });
        }
    }



        private void GotoAddWindow()
        {
            _navigationService.NavigateTo("AddWindow");
        }
    }
}