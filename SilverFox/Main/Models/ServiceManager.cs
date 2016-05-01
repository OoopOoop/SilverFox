using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Management;
using Main.Shared;
using System.Runtime.Serialization.Json;



namespace Main.Models
{
    //Make it static? (singletom)
    public static class ServiceManager
    {
        public static ServiceController GetService(ServiceItem item)
        {
            var ServiceControllerService = ServiceController.GetServices().First((x) => x.ServiceName == item.ServiceName);
            return ServiceControllerService;
        }

        public static ManagementObject GetManagementObject(string serviceName)
        {
           var wmiService = new ManagementObject("Win32_Service.Name='" + serviceName + "'");
           wmiService.Get();
           return wmiService;
        }

        public static void StartService(ServiceItem item)
        {
            //Start
            var controller=GetService(item);
            controller.Start();
            controller.WaitForStatus(ServiceControllerStatus.Running);
           
        }

        public static void StopService(ServiceItem item)
        {
            //Stop
            var controller = GetService(item);
            controller.Stop();
            controller.WaitForStatus(ServiceControllerStatus.Stopped);
        }

        public static void RefreshStatus(ServiceItem item)
        {
            // call it on startUp
            // Update value of serviceItem status
            var controller = GetService(item);
            controller.Refresh();
        }

        public static List<ServiceItem> GetAllServiceItems()
        {
            //get all currently running services
            //var items = ServiceController.GetServices().Select(service => new ServiceItem
            //{
            //    ServiceName = service.ServiceName,
            //    DisplayName = service.DisplayName,
            //    Status = service.Status.ToString(),
            //    CanStop = service.CanStop,
            //    Description = GetManagementObject(service.ServiceName)["Description"].ToString(),
            //    StartMode = GetManagementObject(service.ServiceName)["StartMode"].ToString(),
            //}).ToList();
            //return items;



            List<ServiceItem> runningServices = new List<ServiceItem>();
            var services = ServiceController.GetServices();
            foreach (var service in services)
            {
                var wmiService = GetManagementObject(service.ServiceName);
                runningServices.Add(new ServiceItem
                {
                    ServiceName = service.ServiceName,
                    CanStop = service.CanStop,
                    DisplayName = service.DisplayName,
                    Status = service.Status.ToString(),
                    Description = wmiService["Description"].ToString(),
                    StartMode = wmiService["StartMode"].ToString()
                }); 
            }

            return runningServices;
        }


        public static void ChangeStartMode(ServiceItem item, string changeTo)
        {
            var controller = GetService(item);
            ServiceStartMode mode;
            switch (changeTo)
            {
                case "Automatic":
                     mode=ServiceStartMode.Automatic;
                        break;
                case "Manual":
                    mode = ServiceStartMode.Manual;
                    break;
                case "Disabled":
                    mode = ServiceStartMode.Disabled;
                    break;
                default:
                    mode = ServiceStartMode.Automatic;
                    break;
            }

            ServiceHelper.ChangeStartMode(controller, mode);
        }

        public static List<ServiceItem> GetSavedServiceItems()
        {
            //read a file with saved servies
            return null;
        }


        //Get reference to windows.storage
        public static void SetSavedServiceItems(List<ServiceItem> items)
        {
            //saves all selected items (name)

         // StorageFolder 
                
            var jsonSerializer = new DataContractJsonSerializer(typeof(List<ServiceItem>));
          //  using(var stream =)
        }
    }
}
