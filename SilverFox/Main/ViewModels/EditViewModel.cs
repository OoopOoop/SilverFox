using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Main.Models;
using Main.Shared;
using System;

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

        public string OriginalDisplayName => _serviceItem.OriginalDisplayName;
        public string OriginalDescription => _serviceItem.OriginalDescription;
        public string ServiceName => _serviceItem.ServiceName;

        public string StartupMessage
        {
            get
            {
                if (_serviceItem.OriginalStartMode == _serviceItem.StartMode)
                    return "";

                return $"Startup mode was changed from {_serviceItem.OriginalStartMode} to {_serviceItem.StartMode}";
            }
        }

        public string NewDisplayName
        {
            get { return _newDisplayName; }
            set { _newDisplayName = value; OnPropertyChanged(); }
        }

        public string NewDescription
        {
            get { return _newDescription; }
            set { _newDescription = value; OnPropertyChanged(); }
        }

        public RelayCommand SaveChangesCommand => _saveChangesCommand ?? (_saveChangesCommand = new RelayCommand(
            () =>
            {
                if (string.IsNullOrEmpty(NewDisplayName))
                {
                    base.ShowErrorMessage("Display name is mandatory", "Error");
                    return;
                }

                _serviceItem.DisplayName = NewDisplayName;
                _serviceItem.Description = NewDescription;

                Messenger.Default.Send(_serviceItem);
                goToMainwindow();
            }
            ));

        private void goToMainwindow()
        {
            _navigationService.NavigateTo("MainWindow");
        }

        public RelayCommand CancelCommand => _cancelCommand ?? (_cancelCommand = new RelayCommand(
            () =>
            {
                goToMainwindow();
            }
            ));

        public EditViewModel(IFrameNavigationService navigationService)
        {
            this._navigationService = navigationService;

            if (this._navigationService.Parameter is ServiceItem)
                _serviceItem = this._navigationService.Parameter as ServiceItem;

            NewDisplayName = _serviceItem.DisplayName ?? "";
            NewDescription = _serviceItem.Description ?? "";
        }
    }
}