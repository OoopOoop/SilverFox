using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Main.Models;
using Main.Shared;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System;
using System.Collections;

namespace Main.ViewModels
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private ObservableCollection<ServiceItem> _selectedServicesCollection;
        public ObservableCollection<ServiceItem> SelectedServicesCollection
        {
            get { return _selectedServicesCollection; }
            set { _selectedServicesCollection = value; OnPropertyChanged();}
        }

        public ICommand RefreshStatusCommand { get; set;}

        private void setSelectedServices()
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
            NavigateAddWindowCommand = new RelayCommand(toAddWindow);

            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}

            RefreshStatusCommand = new RelayCommand<object>(refreshStatus);

            SelectedServicesCollection = new ObservableCollection<ServiceItem>();
            setSelectedServices();
        }



        private void refreshStatus(object obj)
        {
            
            var refreshedServ = obj as IEnumerable;



            //TODO: change foreach for for loop
            if (refreshedServ!=null)
            {
                foreach (ServiceItem service in refreshedServ)
                {
                    int serviceIndex = SelectedServicesCollection.IndexOf(service);
                    ServiceManager.RefreshStatus(service);
                    SelectedServicesCollection[serviceIndex] = service;
                }
              
            }
        }



        private void toAddWindow()
        {
            _navigationService.NavigateTo("AddWindow");
        }
    }
}