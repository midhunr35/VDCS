using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace VSDApp.com.rta.vsd.hh.utilities
{
    class AlprTargetedVehicles
    {
        MainWindow m_MainWindow = null;
        public string AlprCam01_Name = "";
        public string AlprCam01_IP = @"";

        public AlprTargetedVehicles(MainWindow mainWind)
        {
            m_MainWindow = mainWind;
        }
        public string StartGettingAlprIntrestedVehicle()
        {
            try
            {
                FileInfo alprSelectedFile = GetAlprFile();
                XmlDocument doc = new XmlDocument();
                string text = string.Empty;
                if (alprSelectedFile != null && alprSelectedFile.FullName != null && alprSelectedFile.FullName != "" && alprSelectedFile.Exists == true)
                {
                    doc.Load(alprSelectedFile.FullName);
                }
                else
                    return string.Empty;
                // doc.Load(@"C:\\Khurram's Data\\Office Data\\RTA New Development\\VSDDS-SupportDev\\ALPR Stuff\\SampleData.xml");
                foreach (XmlNode node in doc.DocumentElement.ChildNodes)
                {
                    //or loop through its children as well
                    if (node.Name == "PlateNumber")
                    {
                        text += node.InnerText + "\n" + " ";
                    }
                    if (node.Name == "PlateColor")
                    {
                        text += node.InnerText + "\n" + " ";
                    }
                    if (node.Name == "Emirate")
                    {
                        text += node.InnerText + "\n" + " ";
                    }
                }
                return text;
              

            }
            catch (System.Exception ex)
            {
                CommonUtils.WriteLocationLog("Exception in ALPR Target  ()" + ex.Message);
                return string.Empty;
            }
        }
        /// <summary>
        /// read file from remote machine
        /// </summary>
        private FileInfo GetAlprFile()
        {
            try
            {
                string ip = GetIPAdressOfAlpr(Properties.Settings.Default.AlprCam01_Name);
                if (ip == null || ip == "")
                    return null;
                DirectoryInfo dir = new DirectoryInfo(ip);
                FileInfo[] file = dir.GetFiles();
                FileInfo selectAlprFile = new FileInfo("Test");



                foreach (FileInfo selectedfile in file)
                {
                    if (selectedfile.Extension.Equals(".xml"))
                    {
                        if (selectedfile.Name.Equals("SampleData.xml"))
                        {
                            selectAlprFile = selectedfile;
                            File.SetAttributes(@"C:\Users\IBM_ADMIN\Desktop\New folder (3)\SampleData.xml", FileAttributes.Normal);
                            FileSecurity security = new FileSecurity();
                            FileIOPermission permission = new FileIOPermission(FileIOPermissionAccess.AllAccess, selectedfile.FullName);


                          
                           File.SetAttributes(selectedfile.FullName, FileAttributes.Normal);
                           // File.Move(selectedfile.FullName, Path.Combine(selectedfile.DirectoryName, "1" + selectedfile.Name));
                           
                            
                            File.Move(selectedfile.FullName, Path.ChangeExtension(selectedfile.FullName, ".xml.processed"));

                           
                            
                           
                            //selectedfile.Replace(selectedfile.Extension, ".processed");
                           //string changed = Path.ChangeExtension(selectedfile.DirectoryName, ".processed");                         

                        }
                    }
                }
                return selectAlprFile;
            }
            catch (System.Exception ex)
            {
                CommonUtils.WriteLocationLog("Exception in ALPR Target Vehicle ()" + ex.Message);
                return null;
            }
        }
        private string GetIPAdressOfAlpr(string alprName)
        {
            try
            {
                // Get is from VSD_Property

                string Alpr_IP = Properties.Settings.Default.AlprCam01_IP;
                return Alpr_IP;

            }
            catch (System.Exception ex)
            {
                CommonUtils.WriteLocationLog("Exception in ALPR Target Vehicle.cs.GetIPAdressOfAlpr ()" + ex.Message);
                return "";
            }
        }

    }
}
