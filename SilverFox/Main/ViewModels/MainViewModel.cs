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
            set { _selectedServicesCollection = value;}
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


        //TODO: data in columns not updating, need completly replace item in datagrid to see changes
        private void refreshStatus(object obj)
        {            
            var servToUpdate = obj as IEnumerable;

            if(servToUpdate!=null)
            {
                servToUpdate.Cast<ServiceItem>().ToList().ForEach(s =>
                {
                 var updatedService = ServiceManager.RefreshStatus(s);
                 SelectedServicesCollection[SelectedServicesCollection.IndexOf(s)]=new ServiceItem {Status=updatedService.Status,DisplayName=updatedService.DisplayName,Description=updatedService.Description };
                });
        }
    }



        private void toAddWindow()
        {
            _navigationService.NavigateTo("AddWindow");
        }
    }
}