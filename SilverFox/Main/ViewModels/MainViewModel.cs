using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Main.Models;
using Main.Shared;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Main.ViewModels
{
    public class MainViewModel:NotifyClass
    {

    

        private string _status;
        private RelayCommand<object> _rerfreshselectedStatusCommand;
        private ObservableCollection<ServiceItem> _selectedServicesCollection;
        private RelayCommand _loadServicesCommand;
        private RelayCommand<object> _removeServiceCommand;
        private RelayCommand _refreshAllStatusCommand;
        private RelayCommand<object> _stopServiceCommand;
        private RelayCommand<object> _startServiceCommand;
        private RelayCommand<object> _changeStatusCommand;
        private RelayCommand<string> _changeStartupCommand;
        private RelayCommand _navigateAddWindowCommand;
        private RelayCommand _windowClosingCommand;
        private IFrameNavigationService _navigationService;

        public ObservableCollection<ServiceItem> SelectedServicesCollection
        {
            get { return _selectedServicesCollection; }
            set { _selectedServicesCollection = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Updates status of single or multiple selected services
        /// </summary>
        public RelayCommand<object> RefreshSelectedStatusCommand => _rerfreshselectedStatusCommand ?? (_rerfreshselectedStatusCommand = new RelayCommand<object>(
           obj =>
           {
               var servToUpdate = obj as IEnumerable;
               if (servToUpdate != null)
               {
                   servToUpdate.Cast<ServiceItem>().ToList().ForEach(service =>
                   {
                       SelectedServicesCollection[SelectedServicesCollection.IndexOf(service)].Status = ServiceManager.RefreshStatus(service).Status;
                       SelectedServicesCollection[SelectedServicesCollection.IndexOf(service)].StartMode = ServiceManager.RefreshStatus(service).StartMode;
                   });
               }
           }));

        /// <summary>
        /// Remove selected services from the datagrid
        /// </summary>
        public RelayCommand<object> RemoveServiceCommand => _removeServiceCommand ?? (_removeServiceCommand = new RelayCommand<object>(
            obj =>
            {
                var servToRemove = obj as IEnumerable;
                if (servToRemove != null)
                {
                    servToRemove.Cast<ServiceItem>().ToList().ForEach(service =>
                    {
                        SelectedServicesCollection.Remove(service);
                    });
                }
            }));

        public RelayCommand RefreshAllStatusCommand => _refreshAllStatusCommand ?? (_refreshAllStatusCommand = new RelayCommand(refreshAllServicesStatus));

        /// <summary>
        /// Stop selected service
        /// </summary>
        public RelayCommand<object> StopServiceCommand => _stopServiceCommand ?? (_stopServiceCommand = new RelayCommand<object>(
            obj =>
            {
                var servToStop = obj as IEnumerable;
                if (servToStop != null)
                {
                    servToStop.Cast<ServiceItem>().ToList().ForEach(service =>
                    {
                        ServiceManager.StopService(service);
                        service.Status = ServiceManager.RefreshStatus(service).Status;
                    });
                }
            }));


        /// <summary>
        /// Start selected service 
        /// </summary>
        public RelayCommand<object> StartServiceCommand => _startServiceCommand ?? (_startServiceCommand = new RelayCommand<object>(
            obj =>
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
            }));


        /// <summary>
        /// Change startup command to "manual", "disabled" or "auto"
        /// </summary>
        public RelayCommand<object> ChangeStatusCommand => _changeStatusCommand ?? (_changeStatusCommand = new RelayCommand<object>(changeStatus));


        /// <summary>
        /// Get "automatic" "manual" or "disabled" parameters from radiobuttons
        /// </summary>
        public RelayCommand<string> ChangeStartupCommand => _changeStartupCommand ?? (_changeStartupCommand = new RelayCommand<string>(
         status =>
         {
             _status = status;
         }));
        
        public RelayCommand WindowClosingCommand => _windowClosingCommand ?? (_windowClosingCommand = new RelayCommand(
             () =>
            {
                 ServiceManager.SetSavedServiceItems(SelectedServicesCollection.ToList());
            }));
        
        public RelayCommand NavigateAddWindowCommand => _navigateAddWindowCommand ?? (_navigateAddWindowCommand = new RelayCommand(
            () =>
            {
                _navigationService.NavigateTo("AddWindow");
            }));
        
        public RelayCommand LoadServicesCommand => _loadServicesCommand ?? (_loadServicesCommand = new RelayCommand(
            async () =>
            {
                await getServices();
            }));
      

        public MainViewModel(IFrameNavigationService navigationService)
        {
            SelectedServicesCollection = new ObservableCollection<ServiceItem>();
            _navigationService = navigationService;
            displaySelectedServices();
            refreshAllServicesStatus();
        }

        /// <summary>
        /// Get selected services from the addWindow
        /// </summary>
        private void displaySelectedServices()
        {
            Messenger.Default.Register<ObservableCollection<ServiceItem>>(
                this,
                services =>
                {
                    foreach (ServiceItem service in services)
                    {
                        SelectedServicesCollection.Add(service);
                    }
                });
        }


        private async Task getServices()
        {
            if (SelectedServicesCollection.Count == 0)
            {
                await getServicesDataAsync();
            }
            return;
        }


        private async Task getServicesDataAsync()
        {
            if (SelectedServicesCollection.Count != 0)
            {
                return;
            }

            try
            {
                var collection = await ServiceManager.GetSavedServiceItems();
                SelectedServicesCollection = new ObservableCollection<ServiceItem>(collection);
            }
            catch
            {
                SelectedServicesCollection = new ObservableCollection<ServiceItem>();
            }
        }

        /// <summary>
        /// Refresh all services in datagrid selected or not
        /// </summary>
        private void refreshAllServicesStatus()
        {
            if (SelectedServicesCollection.Count != 0)
            {
                foreach (ServiceItem service in SelectedServicesCollection)
                {
                    service.Status = ServiceManager.RefreshStatus(service).Status;
                }
            }
        }



        private void changeStatus(object obj)
        {
            var servChngStatus = obj as IEnumerable;
            if (servChngStatus != null && _status != String.Empty)
            {
                servChngStatus.Cast<ServiceItem>().ToList().ForEach(service =>
                {
                    try
                    {
                        ServiceManager.ChangeStartMode(service, _status);
                        service.StartMode = ServiceManager.RefreshStatus(service).StartMode;
                    }
                    catch (Exception ex)
                    {
                        string exceptionMessage = "Could not change service start type";
                        string win32Exception = ex.Message;
                    }
                });
            }
        }
    }
}