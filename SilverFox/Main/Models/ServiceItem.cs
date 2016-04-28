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


         public ServiceItem (ServiceController _sController)
        {
            sController = _sController;
            wmiService = new ManagementObject("Win32_Service.Name='" + sController.ServiceName + "'");
            wmiService.Get();
        }


        public void StartService()
        {
            try
            {
                sController.Start();
                sController.WaitForStatus(ServiceControllerStatus.Running);
            }
            catch(Exception)
            {

            }
        }


        public void StopService()
        {
            try
            {
                sController.Stop();
                sController.WaitForStatus(ServiceControllerStatus.Stopped);
            }
            catch (Exception)
            {

            }
        }


        public void RestartService()
        {
            try
            {
                if (sController.CanStop && (sController.Status == ServiceControllerStatus.Running || sController.Status == ServiceControllerStatus.Paused))
                {
                    StopService();
                    sController.WaitForStatus(ServiceControllerStatus.Stopped);
                }

                if (sController.Status == ServiceControllerStatus.Stopped)
                {
                    StartService();
                    sController.WaitForStatus(ServiceControllerStatus.Running);
                }
            }
            catch (Exception)
            {
            }
        }


        public void ChangeSerStartUp(int mode)
        {
            ServiceStartMode startMode = 0;

            switch (mode)
            {
                case 1:
                    startMode = ServiceStartMode.Automatic;
                    break;
                case 2:
                    startMode = ServiceStartMode.Disabled;
                    break;
                case 3:
                    startMode = ServiceStartMode.Manual;
                    break;
                default:
                    startMode = ServiceStartMode.Automatic;
                    break;
            }
            ServiceHelper.ChangeStartMode(sController, startMode);
        }
    }
}
