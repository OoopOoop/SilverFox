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

namespace Main.ViewModels
{
    public class AddViewModel : NotifyService
    {
        private IFrameNavigationService _navigationService;
        private ObservableCollection<ServiceItem> _runningServicesCollection;
        private ObservableCollection<ServiceItem> _selectedService;
        private RelayCommand<object> _sendSelectedServices;
        private RelayCommand _loadServicesCommand;

        private RelayCommand _cancelAndGoBackCommand;
        

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


      public  List <ServiceItem> Match;

        public AddViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
            _selectedService = new ObservableCollection<ServiceItem>();
            RunningServicesCollection = new ObservableCollection<ServiceItem>();



            Match = new List<ServiceItem>();
        }


        private async Task<ObservableCollection<ServiceItem>> getRunningServices()
        {
            return RunningServicesCollection = await ServiceManager.GetAllServiceItems();
        }

        

        private RelayCommand<string> _searchCommand;
        public RelayCommand<string> SearchCommand => _searchCommand ?? (_searchCommand = new RelayCommand<string>(FindAndReplace));


        private void FindAndReplace(string searchParameter)
        {
            if (!String.IsNullOrEmpty(searchParameter)&&RunningServicesCollection.Count!=0)
            {

                doSearch(searchParameter);

                object test = null;
            }
        }






        private List<ServiceItem> doSearch(string searchParameter)
        {
            string[] words = searchParameter.Split(' ');
            foreach (string word in words)
            {
                foreach (ServiceItem service in RunningServicesCollection)
                {

                    //if (services.DisplayName.Contains(word) || services.ServiceName.Contains(word) )
                    //{
                    //    Match.Add(services);
                    //}

                    //if (services.Description.Contains(word))
                    // {
                    //     Match.Add(services);
                    // }


                    if (!String.IsNullOrEmpty(service.Description))
                    {
                       // Regex.Match(service.Description, word);

                        if (Regex.IsMatch(service.Description, @"\b(COM)\b"))
                        {
                             Match.Add(service);
                            //RunningServicesCollection.Remove(service);
                        }
                    }
                }
            }

            return Match;
        }
    }
}