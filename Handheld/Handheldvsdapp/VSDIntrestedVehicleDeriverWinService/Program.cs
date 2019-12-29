using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using VSDApp.com.rta.vsd.hh.db;

namespace VSDIntrestedVehicleDeriverWinService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            SynchInrestedVehNDriver s = new SynchInrestedVehNDriver();
           s.SyncIntrestedListofVehicleAndDriver();
            
         
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new WinService() 
            };
            ServiceBase.Run(ServicesToRun);
          
        }
    }
}
