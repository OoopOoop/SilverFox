using Main.Shared;
using Main.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Main.Models
{
   public class ServiceItem
    {
        private ServiceController sController;
        private ManagementObject wmiService;

        public string ServiceName { get { return sController.ServiceName; } }
        public ServiceControllerStatus Status { get { return sController.Status; }}
        public string DisplayName { get { return sController.DisplayName; } }
        public bool CanStop { get { return sController.CanStop; } }

        public object Description { get { return wmiService["Description"]; } }
        public object StartMode { get { return wmiService["StartMode"]; } }

        public void StartService()
        {

        }

        public void StopService()
        {

        }

        public void ChangeSerStartUp()
        {

        }
    }
}
