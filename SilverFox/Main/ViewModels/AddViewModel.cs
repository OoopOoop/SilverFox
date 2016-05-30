using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Main.Models;
using Main.Shared;
using System.Collections;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Main.ViewModels
{
    public class AddViewModel : NotifyClass
    {
        private IFrameNavigationService _navigationService;
        private ObservableCollection<ServiceItem> _runningServicesCollection;
        private ObservableCollection<ServiceItem> _selectedService;
        private RelayCommand<object> _sendSelectedServices;
        private RelayCommand _loadServicesCommand;

        public ObservableCollection<ServiceItem> RunningServicesCollection
        {
            get { return _runningServicesCollection; }
            set { _runningServicesCollection = value; OnPropertyChanged(); }
        }

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
    }
}