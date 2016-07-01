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

        public EditViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;

            if (_navigationService.Parameter is ServiceItem)
                ServiceItem = _navigationService.Parameter as ServiceItem;
        }
    }
}