using ConsoleDisplayService;
using System;
using System.Linq;
using System.Management;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleDisplayService
{
    public class Service
    {
        public ServiceController[] scServices;
        private string serviceName;
        private ServiceController scController;

        public enum StartMode
        {
            Automatic,
            Disabled,
            Manual
        }


        public Service(string ServiceName)
        {
            scServices = ServiceController.GetServices();
            serviceName = ServiceName;
            scController = new ServiceController(serviceName);
            getService();
        }
       

        public void printAllRunningServices()
        {
            scServices = ServiceController.GetServices();
            foreach (ServiceController curService in scServices)
            {
                if (curService.Status == ServiceControllerStatus.Running)
                {
                    Console.WriteLine("---------------------------------------------");
                    Console.WriteLine("Service: {0}", curService.ServiceName);
                    Console.WriteLine("Display name: {0}", curService.DisplayName);
                    ManagementObject wmiService;
                    wmiService = new ManagementObject("Win32_Service.Name='" + curService.ServiceName + "'");
                    wmiService.Get();
                    Console.WriteLine("Start name:  {0}", wmiService["StartName"]);
                    Console.WriteLine("Desctription: {0}", wmiService["Description"]);          
                }
            }

            Console.ReadLine();
        }


        private void getService()
        {
            scController=scServices.FirstOrDefault(s => s.ServiceName == serviceName);
        }



        public void displayServiceInfo()
        {
            if(scController != null)
            {
                Console.WriteLine("Service: {0}", scController.ServiceName);
                Console.WriteLine("Display name: {0}", scController.DisplayName);
                ManagementObject wmiService;
                wmiService = new ManagementObject("Win32_Service.Name='" + scController.ServiceName + "'");
                wmiService.Get();
                Console.WriteLine("Start name:  {0}", wmiService["StartName"]);
                Console.WriteLine("Desctription: {0}", wmiService["Description"]);
                Console.WriteLine("Start Mode: {0}", wmiService["StartMode"]);
                Console.ReadLine();
            }
        }


        public void StopService()
        {
            if (scController!= null&& scController.Status==ServiceControllerStatus.Running)
            {
                displayServiceInfo();
                scController.Stop();
                scController.WaitForStatus(ServiceControllerStatus.Stopped);
                Console.Write("{0}---{1}", scController.DisplayName, scController.Status.ToString());          
                Console.ReadLine();
            }
        }

        public void StartService()
        {
            if (scController != null && scController.Status == ServiceControllerStatus.Stopped)
            {
                displayServiceInfo();
                scController.Start();
                scController.WaitForStatus(ServiceControllerStatus.Running);
                Console.Write("{0}---{1}", scController.DisplayName, scController.Status.ToString());
                Console.ReadLine();
            }
        }

        public void ChangeStartMode(StartMode mode)
        {
            ServiceStartMode startMode=0;

            switch (mode)
            {
                case StartMode.Automatic:
                    startMode = ServiceStartMode.Automatic;
                    break;
                case StartMode.Disabled:
                    startMode = ServiceStartMode.Disabled;
                    break;
                case StartMode.Manual:
                    startMode = ServiceStartMode.Manual;
                    break;
                default:
                    startMode = ServiceStartMode.Automatic;
                    break;
            }
            ServiceHelper.ChangeStartMode(scController, startMode);
        }


    }

    class Program
    {
        static void Main(string[] args)
        {
            var ser = new Service("TeamViewer");

            // ser.printAllRunningServices();
           // ser.displayServiceInfo();
             ser.StopService();
            // ser.StartService();
           //  ser.ChangeStartMode(Service.StartMode.Manual);

        }
    }
}
