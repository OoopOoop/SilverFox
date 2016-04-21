using ConsoleDisplayService;
using System;
using System.Management;
using System.ServiceProcess;
using System.Threading;

namespace ConsoleDisplayService
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceController[] scServices;
            scServices = ServiceController.GetServices();

            var teamViwerSer = new ServiceController("TeamViewer");
            ServiceHelper.ChangeStartMode(teamViwerSer, ServiceStartMode.Automatic);

           
            if(teamViwerSer.Status!=ServiceControllerStatus.Running)
            {
                teamViwerSer.Start();
            }

            teamViwerSer.Close();

            foreach (ServiceController curService in scServices)
            {
                if(curService.Status==ServiceControllerStatus.Running)
                {
                    Console.WriteLine("Service: {0}", curService.ServiceName);
                    Console.WriteLine("Display name: {0}", curService.DisplayName);
                    ManagementObject wmiService;
                    wmiService = new ManagementObject("Win32_Service.Name='" + curService.ServiceName + "'");
                    wmiService.Get();
                    Console.WriteLine("Start name:  {0}", wmiService["StartName"]);
                    Console.WriteLine("Desctription: {0}", wmiService["Description"]);
                }
            }

           
            if(teamViwerSer.Status==ServiceControllerStatus.Stopped)
            {
                Console.WriteLine("TeamViwer service status: {0}", teamViwerSer.Status);
            }
            Console.WriteLine("TeamViwer service status: {0}", teamViwerSer.Status);
            Console.ReadLine();
        }
    }
}
