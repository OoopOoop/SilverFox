using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Main.Models;
using Main.Shared;

namespace Main.ViewModels
{
    public class EditViewModel : NotifyService
    {
        private IFrameNavigationService _navigationService;
        private ServiceItem _serviceItem;
        private RelayCommand _saveChangesCommand;
        private string _newDisplayName;
        private string _newDescription;
        private RelayCommand _cancelCommand;

        public ServiceItem ServiceItem
        {
            get
            {
                return _serviceItem;
            }
            set
            {
                _serviceItem = value;
                OnPropertyChanged();
               
            }
        }


        //public string OriginalDisplayName { get; set; }
        public string OriginalDescription { get; set; }
        public string OriginalStartType { get; set; }



        private string originalDisplayName;

        public string OriginalDisplayName
        {
            get { return originalDisplayName; }
            set { originalDisplayName = value; OnPropertyChanged(); }
        }




        public string NewDisplayName
        {
            get { return _newDisplayName; }
            set { _newDisplayName = value;OnPropertyChanged(); }
        }

        
        public string NewDescription
        {
            get { return _newDescription; }
            set { _newDescription = value; OnPropertyChanged(); }
        }


        public RelayCommand SaveChangesCommand => _saveChangesCommand ?? (_saveChangesCommand = new RelayCommand(
            () => 
            {
                ServiceItem.DisplayName = NewDisplayName;
                ServiceItem.Description = NewDescription;
                Messenger.Default.Send(ServiceItem);
              
               _navigationService.NavigateTo("MainWindow");


             

            }
            ));


       

        public RelayCommand CancelCommand => _cancelCommand ?? (_cancelCommand = new RelayCommand(
            () =>
            {
                _navigationService.NavigateTo("MainWindow");
            }
            ));




        public EditViewModel(IFrameNavigationService navigationService)
        {
            this._navigationService = navigationService;

            if (this._navigationService.Parameter is ServiceItem)
                ServiceItem = this._navigationService.Parameter as ServiceItem;





            //OriginalDisplayName = ServiceItem.OriginalDisplayName ?? "";
            //OriginalDescription = ServiceItem.OriginalDescription ?? "";
            //OriginalStartType = ServiceItem.OriginalStartMode ?? "";

        }



        //private void editSelectedService()
        //{
        //    Messenger.Default.Register<ServiceItem>(
        //     this,
        //      service =>
        //      {
        //          //OriginalDisplayName = service.OriginalDisplayName ?? "";
        //          //OriginalDescription = service.OriginalDescription ?? "";
        //          //OriginalStartType = service.OriginalStartMode ?? "";
        //          if(service!=null)
        //          ServiceItem = service;
        //      });
        //}
    }
}