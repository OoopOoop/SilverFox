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

     
        public string OriginalDisplayName { get; set; }
        public string OriginalDescription { get; set; }
        public string OriginalStartType { get; set; }


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
                _serviceItem.DisplayName = NewDisplayName;

                if (!string.IsNullOrEmpty(NewDescription))
                {
                    _serviceItem.Description = NewDescription;
                }
                else
                {
                    if (base.ShowConfirmMessage("Leave Description field empty?", "Confirmation"))
                    {
                        _serviceItem.Description = NewDescription;
                    }
                    else
                    {
                        return;
                    }

                }

              
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

            
            OriginalDisplayName = _serviceItem.OriginalDisplayName ?? "";
            OriginalDescription = _serviceItem.OriginalDescription ?? "";
            OriginalStartType = _serviceItem.OriginalStartMode ?? "";



            //NewDisplayName = _serviceItem.DisplayName ?? "";
            NewDisplayName = _serviceItem.DisplayName ?? OriginalDisplayName;
            NewDescription = _serviceItem.Description??"";
        }
    }
}