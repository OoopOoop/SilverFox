using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Management;
using Main.Shared;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Main.Models
{
    //Make it static? (singletom)
    public static class ServiceManager
    {

        const string fileName = "services.txt";

        public static ServiceController GetService(ServiceItem item)
        {
            //var ServiceControllerService = ServiceController.GetServices().First((x) => x.ServiceName == item.ServiceName);
            var ServiceControllerService = ServiceController.GetServices().First((x) => x.DisplayName == item.DisplayName);
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



        //public static void RefreshStatus(ServiceItem item)
        //{
        //    // call it on startUp
        //    // Update value of serviceItem status
        //    var controller = GetService(item);
        //    controller.Refresh();
        //}



        public static ServiceItem RefreshStatus(ServiceItem item)
        {
            // call it on startUp
            // Update value of serviceItem status
            var controller = GetService(item);
            controller.Refresh();
            string newStatus = controller.Status.ToString();

            if(newStatus!=item.Status)
            {
                item.Status = newStatus;
            }

            return item;
        }



        public static ObservableCollection<ServiceItem> GetAllServiceItems()
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


            ObservableCollection<ServiceItem> runningServices = new ObservableCollection<ServiceItem>();

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
                        Description = (string)wmiService["Description"],
                        StartMode = (string)wmiService["StartMode"]
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

        public static Task<List<ServiceItem>> GetSavedServiceItems()
        {
            //read a file with saved servies
            return Task.Run(() =>
            {
                List<ServiceItem> savedServices = new List<ServiceItem>();
                try
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(fileName);
                    string xmlString = xmlDocument.OuterXml;

                    using (StringReader read = new StringReader(xmlString))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(List<ServiceItem>));

                        using (XmlReader reader = new XmlTextReader(read))
                        {
                            savedServices = (List<ServiceItem>)serializer.Deserialize(reader);
                            reader.Close();
                        }
                        read.Close();
                    }
                }
                catch
                {
                    savedServices = new List<ServiceItem>();
                }

                return savedServices;
            });
        }


        //Get reference to windows.storage
        public static Task SetSavedServiceItems(List<ServiceItem> items)
        {
           return Task.Run(() =>
            {
                XmlDocument xmlDocument = new XmlDocument();
                XmlSerializer serializer = new XmlSerializer(typeof(List<ServiceItem>));
                using (MemoryStream stream = new MemoryStream())
                {
                    serializer.Serialize(stream, items);
                    stream.Position = 0;
                    xmlDocument.Load(stream);
                    xmlDocument.Save(fileName);
                    stream.Close();
                }
            });
        }
    }
}
