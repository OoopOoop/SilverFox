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
    public class MainViewModel : NotifyService
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
        private RelayCommand<object> _editServiceCommand;
        private IFrameNavigationService _navigationService;

        public ObservableCollection<ServiceItem> SelectedServicesCollection
        {
            get { return _selectedServicesCollection; }
            set { _selectedServicesCollection = value; OnPropertyChanged(); }
        }

        public RelayCommand RefreshAllStatusCommand => _refreshAllStatusCommand ?? (_refreshAllStatusCommand = new RelayCommand(refreshAllServicesStatus));

        public RelayCommand<object> StopServiceCommand => _stopServiceCommand ?? (_stopServiceCommand = new RelayCommand<object>(stopService));

        public RelayCommand<object> StartServiceCommand => _startServiceCommand ?? (_startServiceCommand = new RelayCommand<object>(startService));

        public RelayCommand WindowClosingCommand => _windowClosingCommand ?? (_windowClosingCommand = new RelayCommand(saveServices));

        public RelayCommand<object> ChangeStatusCommand => _changeStatusCommand ?? (_changeStatusCommand = new RelayCommand<object>(changeStatus));

        /// <summary>
        /// Update status of single or multiple selected services
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



        

        public RelayCommand<object> EditServiceCommand => _editServiceCommand ?? (_editServiceCommand = new RelayCommand<object>(
            obj =>
            {

                _navigationService.NavigateTo("EditWindow", obj);


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

     
        /// <summary>
        /// Get "automatic" "manual" or "disabled" parameters from radiobuttons
        /// </summary>
        public RelayCommand<string> ChangeStartupCommand => _changeStartupCommand ?? (_changeStartupCommand = new RelayCommand<string>(
         status =>
         {
             _status = status;
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



     


        private void getServiceEdit()
        {
            Messenger.Default.Register<ServiceItem>(
                this,
                service =>
                {
                    if (SelectedServicesCollection.Any(x => x.OriginalDisplayName == service.OriginalDisplayName))
                    {
                        SelectedServicesCollection[SelectedServicesCollection.IndexOf(service)].DisplayName = service.DisplayName;
                        SelectedServicesCollection[SelectedServicesCollection.IndexOf(service)].Description = service.Description;
                    }
                });
        }




        public MainViewModel(IFrameNavigationService navigationService)
        {
            SelectedServicesCollection = new ObservableCollection<ServiceItem>();
            _navigationService = navigationService;




            getServiceEdit();
            displaySelectedServices();
            // refreshAllServicesStatus();
        }

        /// <summary>
        /// Get selected services from the collection of services on the AddWindow
        /// </summary>
        private void displaySelectedServices()
        {
            Messenger.Default.Register<ObservableCollection<ServiceItem>>(
                this,
                services =>
                {
                    foreach (ServiceItem service in services)
                    {
                        if(!SelectedServicesCollection.Any(x=>x.DisplayName==service.DisplayName))
                        {
                            SelectedServicesCollection.Add(service);
                        }
                    }
                });
        }

        private async Task getServices()
        {
            if (SelectedServicesCollection.Count == 0)
            {
                await getServicesDataAsync();
                refreshAllServicesStatus();
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
            catch(Exception ex)
            {
                SelectedServicesCollection = new ObservableCollection<ServiceItem>();

                string caption = "Load error";
                string exception = ex.Message;
                string exceptionMessage = String.Format("{0} {1}", exception, "\n" + ex.InnerException ?? ex.InnerException.Message);
                base.ShowErrorMessage(exceptionMessage, caption);
            }
        }

        /// <summary>
        /// Save services in SelectedServicesCollection when programm closes
        /// </summary>
        private void saveServices()
        {
            try
            {
                ServiceManager.SetSavedServiceItems(SelectedServicesCollection.ToList());
            }
            catch (Exception ex)
            {
                string caption = "Save error";
                string exception = ex.Message;
                string exceptionMessage = String.Format("{0} {1}", exception, "\n" + ex.InnerException ?? ex.InnerException.Message);
                base.ShowErrorMessage(exceptionMessage, caption);
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
                    service.StartMode = ServiceManager.RefreshStatus(service).StartMode;
                }
            }
        }

        /// <summary>
        /// Change startup command to "manual", "disabled" or "auto"
        /// </summary>
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
                        string caption = "Startup type error";
                        string exception = ex.Message;
                        string exceptionMessage = String.Format("{0} {1} {2} {3} {4}", "Could not change", service.DisplayName, "service startup type","\n" + exception, "\n"+ex.InnerException??ex.InnerException.Message);
                        base.ShowErrorMessage(exceptionMessage, caption);
                    }
                });
            }
        }

        /// <summary>
        /// Start selected service
        /// </summary>
        private void startService(object obj)
        {
            var servToStop = obj as IEnumerable;
            if (servToStop != null)
            {
                servToStop.Cast<ServiceItem>().ToList().ForEach(service =>
                {
                    try
                    {
                        ServiceManager.StartService(service);
                        service.Status = ServiceManager.RefreshStatus(service).Status;
                    }
                    catch (Exception ex)
                    {
                        string caption = "Start service error";
                        string exception = ex.Message;
                        string exceptionMessage = String.Format("{0} {1}", exception, "\n" + ex.InnerException ?? ex.InnerException.Message);
                        base.ShowErrorMessage(exceptionMessage, caption);
                    }
                });
            }
        }

        /// <summary>
        /// Stop selected service
        /// </summary>
        private void stopService(object obj)
        {
            var servToStop = obj as IEnumerable;
            if (servToStop != null)
            {
                servToStop.Cast<ServiceItem>().ToList().ForEach(service =>
                {
                    try
                    {
                        ServiceManager.StopService(service);
                        service.Status = ServiceManager.RefreshStatus(service).Status;
                    }
                    catch (Exception ex)
                    {
                        string caption = "Stop service error";
                        string exception = ex.Message;
                        string exceptionMessage = String.Format("{0} {1}", exception, "\n" + ex.InnerException ?? ex.InnerException.Message);
                        base.ShowErrorMessage(exceptionMessage, caption);
                    }
                });
            }
        }
    }
}