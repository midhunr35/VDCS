using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace VSDIntrestedVehicleDeriverWinService
{
    class WinService : ServiceBase
    {
        public static readonly ILog log = LogManager.GetLogger("VSDWinServiceLog");

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ServiceName = "VSDWinService";

        }
      //  private string folderPath = @"c:\temp";
        /// <summary>
        /// Set things in motion so your service can do its work.
        /// </summary>
        protected override void OnStart(string[] args)
        {

            log.Info("Entered in WinService OnStart : " + "T");
            
            /*
            if (!System.IO.Directory.Exists(folderPath))
                System.IO.Directory.CreateDirectory(folderPath);

            FileStream fs = new FileStream(folderPath + "\\WindowsService.txt",
                                FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter m_streamWriter = new StreamWriter(fs);
            m_streamWriter.BaseStream.Seek(0, SeekOrigin.End);
            m_streamWriter.WriteLine("VSD WindowsService: Service Started at " +
               DateTime.Now.ToShortDateString() + " " +
               DateTime.Now.ToShortTimeString() + "\n");
            m_streamWriter.Flush();
            m_streamWriter.Close();*/

            log.Info("VSD WindowsService: Service Started at " +DateTime.Now.ToShortDateString() + " " +DateTime.Now.ToShortTimeString() + "\n");
            SynchInrestedVehNDriver syncVeh = new SynchInrestedVehNDriver();
            syncVeh.SyncIntrestedListofVehicleAndDriver();
        
        }
        /// <summary>
        /// Stop this service.
        /// </summary>
        protected override void OnStop()
        {
            /*
            FileStream fs = new FileStream(folderPath +
              "\\WindowsService.txt",
              FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter m_streamWriter = new StreamWriter(fs);
            m_streamWriter.BaseStream.Seek(0, SeekOrigin.End);
            m_streamWriter.WriteLine("VSD WindowsService: Service Stopped at " +
              DateTime.Now.ToShortDateString() + " " +
              DateTime.Now.ToShortTimeString() + "\n");
             *  m_streamWriter.Flush();
            m_streamWriter.Close();

            */
            log.Info("VSD WindowsService: Service Stopped at " +
              DateTime.Now.ToShortDateString() + " " +
              DateTime.Now.ToShortTimeString() + "\n");

        }
    }
}
