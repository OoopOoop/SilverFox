using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Main.Models;
using Main.Shared;
using System.Collections;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;

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


        public ObservableCollection<ServiceItem> RunningServicesCollection
        {
            get { return _runningServicesCollection; }
            set { _runningServicesCollection = value; OnPropertyChanged(); }
        }
        
        public RelayCommand CancelAndGoBackCommand => _cancelAndGoBackCommand ?? (_cancelAndGoBackCommand = new RelayCommand(
           () =>
           {
               _navigationService.NavigateTo("MainWindow");
           }));

        public RelayCommand LoadServicesCommand => _loadServicesCommand ?? (_loadServicesCommand = new RelayCommand(
           async () =>
           {
               await getRunningServices();
               _originalCollection = new List<ServiceItem>(RunningServicesCollection);
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
            if (!String.IsNullOrEmpty(searchParameter)&&_originalCollection.Count!=0)
            {
                var returnColletion=doSearch(searchParameter);
                RunningServicesCollection = new ObservableCollection<ServiceItem>(returnColletion);
            }
        }

        
        private IEnumerable<ServiceItem> doSearch(string searchParameter)
        {
            //replace all non-numeric or non-word characters with a single space
            searchParameter = Regex.Replace(searchParameter, "[^0-9 a-z]", " ");
            //replace multiply spaces with a single space
            searchParameter = Regex.Replace(searchParameter, @"\s+", " ");
            //replace single space with "|" character
            searchParameter = Regex.Replace(searchParameter, @"\s", "|");

            string keywords = "(" + searchParameter + ")";

            StringBuilder builder = new StringBuilder();

            foreach (ServiceItem service in _originalCollection)
            {
                string input = builder.Append(service.DisplayName).Append(" ").Append(service.ServiceName).Append(" ").Append(service.Description).ToString();

                if (Regex.IsMatch(input, @"\b" + keywords + @"\b", RegexOptions.Singleline | RegexOptions.IgnoreCase))
                {
                    yield return service;
                }

                builder.Clear();
            }
        }

    }
}