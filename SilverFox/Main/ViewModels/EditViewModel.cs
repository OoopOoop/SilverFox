using GalaSoft.MvvmLight.Command;
using Main.Models;
using Main.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.ViewModels
{
    public class EditViewModel : NotifyService
    {
        private IFrameNavigationService _navigationService;
        private ServiceItem _serviceItem;

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

        private string originalDisplayName;
        public string OriginalDisplayName
        {
            get { return originalDisplayName; }
            set { originalDisplayName = value; OnPropertyChanged(); }
        }

        private string originalDescription;
        public string OriginalDescription
        {
            get { return originalDescription; }
            set { originalDescription = value; OnPropertyChanged(); }
        }


        private string originalStartType;
        public string OriginalStartType
        {
            get { return originalStartType; }
            set { originalStartType = value; OnPropertyChanged(); }
        }


        private RelayCommand saveChangesCommand;

        public RelayCommand SaveChangesCommand => saveChangesCommand ?? (saveChangesCommand = new RelayCommand(
            () => 
            {

            }
            ));


        private RelayCommand cancelCommand;

        public RelayCommand CancelCommand => cancelCommand ?? (cancelCommand = new RelayCommand(
            () =>
            {
                _navigationService.NavigateTo("MainWindow");
            }
            ));




        public EditViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;

            if (_navigationService.Parameter is ServiceItem)
                ServiceItem = _navigationService.Parameter as ServiceItem;

            OriginalDisplayName = ServiceItem.OriginalDisplayName ?? "";
            OriginalDescription = ServiceItem.OriginalDescription ?? "";
            OriginalStartType = ServiceItem.OriginalStartMode ?? "";
        }
    }
}