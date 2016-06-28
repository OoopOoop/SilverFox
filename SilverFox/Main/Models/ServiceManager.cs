using Main.Shared;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Management;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Main.Models
{
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

        /// <summary>
        /// Start servcie
        /// </summary>
        /// <param name="item"></param>
        public static void StartService(ServiceItem item)
        {
            var controller = GetService(item);
            if (controller.Status != ServiceControllerStatus.Running)
            {
                controller.Start();
                controller.WaitForStatus(ServiceControllerStatus.Running);
            }
        }

        /// <summary>
        /// Stop service
        /// </summary>
        /// <param name="item"></param>
        public static void StopService(ServiceItem item)
        {
            var controller = GetService(item);
            if (controller.Status != ServiceControllerStatus.Stopped)
            {
                controller.Stop();
                controller.WaitForStatus(ServiceControllerStatus.Stopped);
            }
        }

        /// <summary>
        /// Update service status
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static ServiceItem RefreshStatus(ServiceItem item)
        {
            var wmiService = GetManagementObject(item.ServiceName);

            var controller = GetService(item);
            controller.Refresh();

            string newStatus = controller.Status.ToString();

            string newStartup = (string)wmiService["StartMode"];

            if (newStatus != item.Status)
            {
                item.Status = newStatus;
            }
            else if (newStartup != item.StartMode)
            {
                item.StartMode = newStartup;
            }

            return item;
        }

        /// <summary>
        /// Change service startup type
        /// </summary>
        /// <param name="item"></param>
        /// <param name="changeTo"></param>
        public static void ChangeStartMode(ServiceItem item, string changeTo)
        {
            var controller = GetService(item);
            ServiceStartMode mode;
            switch (changeTo)
            {
                case "Automatic":
                    mode = ServiceStartMode.Automatic;
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

        public static Task<List<ServiceItem>> GetSavedServiceItems()
        {
            return Task.Run(() =>
            {
                if (File.Exists(AppConfiguration.SavedServicesFileName))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<ServiceItem>));

                    using (TextReader textReader = new StreamReader(AppConfiguration.SavedServicesFileName))
                    {
                        return (List<ServiceItem>)xmlSerializer.Deserialize(textReader);
                    }
                }

                return new List<ServiceItem>();
            });
        }

        public static void SetSavedServiceItems(List<ServiceItem> items)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<ServiceItem>));
            using (TextWriter textWriter = new StreamWriter(AppConfiguration.SavedServicesFileName))
            {
                xmlSerializer.Serialize(textWriter, items);
            }
        }

        /// <summary>
        /// Get all running services
        /// </summary>
        /// <returns></returns>
        public static Task<ObservableCollection<ServiceItem>> GetAllServiceItems()
        {
            return Task.Run(() =>
            {
                ObservableCollection<ServiceItem> runningServices = new ObservableCollection<ServiceItem>();

                var services = ServiceController.GetServices();
                foreach (var service in services)
                {
                    var wmiService = GetManagementObject(service.ServiceName);

                    runningServices.Add(new ServiceItem
                    {
                        ServiceName = service.ServiceName,
                        DisplayName = service.DisplayName,
                        Status = service.Status.ToString(),
                        Description = (string)wmiService["Description"],
                        StartMode = (string)wmiService["StartMode"],

                        DisplayName_original = service.DisplayName,
                        Description_original = (string)wmiService["Description"] ?? "",
                        StartMode_original = (string)wmiService["StartMode"]

                    });
                }

                return runningServices;
            });
        }
    }
}