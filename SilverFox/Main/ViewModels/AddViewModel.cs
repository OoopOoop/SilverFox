using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Main.Models;
using Main.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Main.ViewModels
{
    public class AddViewModel : NotifyService
    {
        private IFrameNavigationService _navigationService;
        private ObservableCollection<ServiceItem> _runningServicesCollection;
        private ObservableCollection<ServiceItem> _selectedService;
        private List<ServiceItem> _originalCollection;
        private RelayCommand<object> _sendSelectedServices;
        private RelayCommand _loadServicesCommand;
        private RelayCommand _cancelAndGoBackCommand;
        private RelayCommand<string> _searchCommand;
        private bool _isPRingActive;

        public ObservableCollection<ServiceItem> RunningServicesCollection
        {
            get { return _runningServicesCollection; }
            set { _runningServicesCollection = value; OnPropertyChanged(); }
        }

        public bool IsPRingActive
        {
            get { return _isPRingActive; }
            set { _isPRingActive = value; OnPropertyChanged(); }
        }

        public RelayCommand CancelAndGoBackCommand => _cancelAndGoBackCommand ?? (_cancelAndGoBackCommand = new RelayCommand(
           () =>
           {
               RunningServicesCollection = null;
               _originalCollection = null;
               _navigationService.NavigateTo("MainWindow");
           }));

        public RelayCommand LoadServicesCommand => _loadServicesCommand ?? (_loadServicesCommand = new RelayCommand(
           async () =>
           {
               IsPRingActive = true;
               await getRunningServices();
               _originalCollection = new List<ServiceItem>(RunningServicesCollection);
               IsPRingActive = false;
           }));

        //Save and serialize selected services, go to the main page and pass the services
        public RelayCommand<object> SendSelectedServices => _sendSelectedServices ?? (_sendSelectedServices = new RelayCommand<object>(
            obj =>
            {
                var services = obj as IEnumerable;
                if (services != null)
                {
                    foreach (ServiceItem item in services)
                    {
                        _selectedService.Add(item);
                    }
                }

                Messenger.Default.Send(_selectedService);
                _navigationService.NavigateTo("MainWindow");
                _selectedService.Clear();
                RunningServicesCollection = null;
                _originalCollection = null;
            }));

        public AddViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
            _selectedService = new ObservableCollection<ServiceItem>();
            RunningServicesCollection = new ObservableCollection<ServiceItem>();
        }

        private async Task<ObservableCollection<ServiceItem>> getRunningServices()
        {
            return RunningServicesCollection = await ServiceManager.GetAllServiceItems();
        }

        public RelayCommand<string> SearchCommand => _searchCommand ?? (_searchCommand = new RelayCommand<string>(findAndReplace));

        private void findAndReplace(string searchParameter)
        {
            if (_originalCollection?.Count > 0)
            {
                //Search for nothing, means clear the search.
                if (string.IsNullOrEmpty(searchParameter))
                {
                    RunningServicesCollection = new ObservableCollection<ServiceItem>(_originalCollection);
                }
                else
                {
                    RunningServicesCollection = new ObservableCollection<ServiceItem>(doSearch(searchParameter));
                }
            }
        }

        private IEnumerable<ServiceItem> doSearch(string searchParameter)
        {
            Regex searchMatch = new Regex(searchParameter, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            foreach (var service in _originalCollection)
            {
                if (searchMatch.IsMatch(service.DisplayName) ||
                    searchMatch.IsMatch(service.ServiceName) ||
                    searchMatch.IsMatch(service.Description ?? ""))
                {
                    yield return service;
                }
            }
        }
    }
}