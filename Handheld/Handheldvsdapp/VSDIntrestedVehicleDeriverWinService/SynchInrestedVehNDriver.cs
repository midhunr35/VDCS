using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using log4net;
using VSDApp.com.rta.vsd.hh.data;
using System.Data;
using System.Data.SqlServerCe;
using VSDApp.com.rta.vsd.hh.db;
using VSDApp.com.rta.vsd.hh.encryption;
using VSDApp.com.rta.vsd.hh.data;
using System.Web;
using VSDApp.com.rta.vsd.hh.utilities;
namespace VSDIntrestedVehicleDeriverWinService
{
    class SynchInrestedVehNDriver
    {
        // get

        string LoggedInUserName = string.Empty;
        string LoggedInPassword = string.Empty;
        public static byte[] iVector = new byte[] { 69, 110, 99, 34, 42, 14, 77, 69, 69, 110, 99, 34, 42, 14, 77, 69 };
        public static byte[] encryptionKey = new byte[] { 54, 13, 110, 42, 35, 17, 98, 110, 12, 34, 67, 43, 81, 54, 120, 56, 54, 13, 110, 42, 35, 17, 98, 110, 12, 34, 67, 43, 81, 54, 120, 56 };


        #region Get Vehicle And Driver List From Service

      

        public void SyncIntrestedListofVehicleAndDriver()
        {
            WinService.log.Info("GetIntrestedListofVehicleAndDriver");
            try
            {
                List<InterestedVehicle> ListofVehicleofIntrest = new List<InterestedVehicle>();

                WinService.log.Info("Before service obj");
                VSDApp.handHeldService.HandHeldService service = new VSDApp.handHeldService.HandHeldService();

                WinService.log.Info("After service obj");
                VSDApp.handHeldService.InquireListOfInterestResponseItem respItem = new VSDApp.handHeldService.InquireListOfInterestResponseItem();
                WinService.log.Info("After Item obj");
                GetLoggedInUserDetials();

                VSDApp.handHeldService.AuthHeader authorize = new VSDApp.handHeldService.AuthHeader();
                authorize.userName = LoggedInUserName;
                authorize.password = LoggedInPassword;
                service.authHeader = authorize;




                String device_Code = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDeviceCode().Trim();
                WinService.log.Info("Device Code = " + device_Code);

                respItem = service.inquireListOfInterest("H-PS-IDD-1", device_Code);

                WinService.log.Info("SynchInrestedVehNDriver.inquireListOfInterest() call successfully");
                if (respItem.response != null)
                {
                    WinService.log.Info("SynchInrestedVehNDriver.inquireListOfInterest() Response code: " + respItem.response.code);
                    WinService.log.Info("Vehicle of Intrest" + respItem.arrayVehicleOfInterest.Length);
                    WinService.log.Info("Driver of Intrest" + respItem.arrayDriversOfInterest.Length);
                    if (respItem.response.code.Equals("1000"))
                    {
                        if (respItem.arrayVehicleOfInterest.Length > 0)
                        {
                            VSDApp.handHeldService.VehicleOfInterest[] vehicle_of_Intrest = respItem.arrayVehicleOfInterest;

                            ClearListofVehicleofIntrestTable();
                            SaveIntrestedListofVehicleLocally(vehicle_of_Intrest);
                        }
                        if (respItem.arrayDriversOfInterest.Length > 0)
                        {
                            VSDApp.handHeldService.DriverOfInterest[] driver_of_Intrest = respItem.arrayDriversOfInterest;
                            ClearListofDriverofIntrestTable();
                            SaveIntrestedListofDriverLocally(driver_of_Intrest);
                        }
                        WinService.log.Info("Records Save successfully for Intrested List Detials");
                        return;

                    }
                    else
                    {
                        this.SyncIntrestedListofVehicleAndDriver();
                    }
                }
                else
                {
                    WinService.log.Info("SynchInrestedVehNDriver.inquireListOfInterest() response come null");
                    this.SyncIntrestedListofVehicleAndDriver();
                }

            }
            catch (Exception ex)
            {
                WinService.log.Info("SynchInrestedVehNDriver.inquireListOfInterest() Exception:");
                WinService.log.Error("Exception" + ex.Message + ".." + ex.InnerException + ex.StackTrace);
                WinService.log.Error("Exception TimeStamp before sleep " + DateTime.Now);
                Thread.Sleep(new TimeSpan(0,1,0));
                WinService.log.Error("Exception TimeStamp after sleep " +DateTime.Now );
                this.SyncIntrestedListofVehicleAndDriver();
                // return ;
            }
        }
        #endregion

        #region Vehicle Intrested List Operations

        public void ClearListofVehicleofIntrestTable()
        {
            try
            {
                WinService.log.Info("Entering in ClearListofVehicleofIntrestTable");
                SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
                string sqlQuery;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCeCommand command;
                SqlCeResultSet rs;
                sqlQuery = "DELETE  FROM VSD_Interest_List_Vehicle";
              //  WinService.log.Info(sqlQuery);
                command = new SqlCeCommand(sqlQuery, con);
                rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);
                rs.Close();
                con.Close();
                WinService.log.Info("VSD_Interest_List_Vehicle Record deleted successfully");

            }
            catch (Exception ex)
            {
                WinService.log.Info("Exception in deleting VSD_Interest_List_Vehicle records" + ex.Message);
            }
        }

        public void SaveIntrestedListofVehicleLocally(VSDApp.handHeldService.VehicleOfInterest[] InterestedListofVehicle)
        {
            try
            {
                WinService.log.Info("SaveIntrestedListofVehicleLocally");
                SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
                WinService.log.Info("Connection String=" + con.ConnectionString);
                string sqlQuery;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCeCommand command;
                SqlCeResultSet rs;
                foreach (VSDApp.handHeldService.VehicleOfInterest vehicl in InterestedListofVehicle)
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                   

                   sqlQuery = "insert into VSD_Interest_List_Vehicle (ChassisNo,TelematicsOperatorId,TelematicsVehicleDeviceId,VehiclePlateCategory,VehiclePlateCode,VehiclePlateSource,IconType,ReasonCriteriaFailed,TimeStamp) Values (" + "'" + ((vehicl.chassisNumber != null) ? vehicl.chassisNumber.Trim() : "") + "'" + "," + "'" + ((vehicl.telematicsOperatorId != null) ? vehicl.telematicsOperatorId.Trim() : "") + "'" + "," + "'" + ((vehicl.telematicsVehicleDeviceId != null) ? vehicl.telematicsVehicleDeviceId.Trim() : "") + "'" + "," + "'" + ((vehicl.vehiclePlateCategory != null) ? vehicl.vehiclePlateCategory.Trim() : "") + "'" + "," + "'" + ((vehicl.vehiclePlateCode != null) ? vehicl.vehiclePlateCode.Trim() : "") + "'" + "," + "'" + ((vehicl.vehiclePlateSource != null) ? vehicl.vehiclePlateSource.Trim() : "") + "'" + "," + "'" + ((vehicl.iconType != null) ? vehicl.iconType.Trim() : "") + "'" + "," + "'" + ((vehicl.reasonCriteriaFailed != null) ? vehicl.reasonCriteriaFailed.Trim() : "") + "'" + "," + "'" + DateTime.Now + "'" + ")";
                   // sqlQuery = "insert into VSD_Interest_List_Vehicle (ChassisNo,TelematicsOperatorId,TelematicsVehicleDeviceId,VehiclePlateCategory,VehiclePlateCode,VehiclePlateSource,IconType,ReasonCriteriaFailed,TimeStamp) Values (" + "'" + ((vehicl.chassisNumber != null) ? vehicl.chassisNumber.Trim() : "") + "'" + "," + "'" + ((vehicl.telematicsOperatorId != null) ? vehicl.telematicsOperatorId.Trim() : "") + "'" + "," + "'" + ((vehicl.telematicsVehicleDeviceId != null) ? vehicl.telematicsVehicleDeviceId.Trim() : "") + "'" + "," + "'" + ((vehicl.vehiclePlateCategory != null) ? vehicl.vehiclePlateCategory.Trim() : "") + "'" + "," + "'" + ((vehicl.vehiclePlateCode != null) ? vehicl.vehiclePlateCode.Trim() : "") + "'" + "," + "'" + ((vehicl.vehiclePlateSource != null) ? vehicl.vehiclePlateSource.Trim() : "") + "'" + "," + "'" + ((vehicl.iconType != null) ? vehicl.iconType.Trim() : "") + "'" + "," + "'" + ((vehicl.reasonCriteriaFailed != null) ? vehicl.reasonCriteriaFailed.Trim() : "") + "'" + "," + "'" + DateTime.Now.ToString()+"'"  + ")";
                   // WinService.log.Info(sqlQuery);
                    command = new SqlCeCommand(sqlQuery, con);
                    rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);
                    rs.Close();
                    con.Close();
                }
                WinService.log.Info("Vehicle Saved Successfully: Count=" + InterestedListofVehicle.Length);

            }
            catch (Exception ex)
            {
                WinService.log.Info("Exception SaveIntrestedListofVehicleLocally" + ex.Message);
            }
        }

        public DateTime GetUpdatedTimeStampofVehicleList()
        {
            DateTime dtime = Convert.ToDateTime("01/01/1900 00:00:00");
            try
            {

                //  WinService.log.Info("Entering in GetUpdatedTimeStampofVehicleList");
                SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
                string sqlQuery;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCeCommand command;
                SqlCeResultSet rs;
                sqlQuery = "SELECT   top(1)     CONVERT(DateTime, TimeStamp) AS TimeStamp FROM       VSD_Interest_List_Vehicle";
                command = new SqlCeCommand(sqlQuery, con);

                rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);

                int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;

                string dateTime = String.Empty;
                if (rs.Read())
                {
                    dtime = rs.GetDateTime(0);
                    // dtime = Convert.ToDateTime(dateTime,
                    //  dtime = DateTime.ParseExact(dateTime, "yyyy-MM-dd HH:mm:ss,fff", System.Globalization.CultureInfo.InvariantCulture);
                    return dtime;

                }

                rs.Close();
                con.Close();

                //   return dateTime;
                // WinService.log.Info("Updated TimeStamp of Vehicle= " + dateTime);
                return dtime;
            }
            catch (Exception ex)
            {


                return dtime;
            }
        }

        #endregion

        #region Driver Intrested List Operations

        public void SaveIntrestedListofDriverLocally(VSDApp.handHeldService.DriverOfInterest[] InterestedListofDriver)
        {
            try
            {
                WinService.log.Info("Entering in SaveIntrestedListofDriverLocally");
                SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
                WinService.log.Info("Connection String=" + con.ConnectionString);
                string sqlQuery;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCeCommand command;
                SqlCeResultSet rs;
                foreach (VSDApp.handHeldService.DriverOfInterest vehicl in InterestedListofDriver)
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }


                    sqlQuery = "insert into VSD_Interest_List_Driver (TelematicsOperatorId,TelematicsTagId,DriverLicenseNumber,DriverRiskRatingCode,IconType,ReasonCriteriaFailed,TimeStamp) Values (" + "'" + ((vehicl.telematicsOperatorId != null) ? vehicl.telematicsOperatorId.Trim() : "") + "'" + "," + "'" + ((vehicl.telematicsTagId != null) ? vehicl.telematicsTagId.Trim() : "") + "'" + "," + "'" + ((vehicl.driverLicenseNumber != null) ? vehicl.driverLicenseNumber.Trim() : "") + "'" + "," + "'" + ((vehicl.driverRiskRatingCode != null) ? vehicl.driverRiskRatingCode.Trim() : "") + "'" + "," + "'" + ((vehicl.iconType != null) ? vehicl.iconType.Trim() : "") + "'" + "," + "'" + ((vehicl.reasonCriteriaFailed != null) ? vehicl.reasonCriteriaFailed.Trim() : "") + "'" + "," + "'"  + DateTime.Now + "'"  + ")";
                   // sqlQuery = "insert into VSD_Interest_List_Driver (TelematicsOperatorId,TelematicsTagId,DriverLicenseNumber,DriverRiskRatingCode,IconType,ReasonCriteriaFailed,TimeStamp) Values (" + "'" + ((vehicl.telematicsOperatorId != null) ? vehicl.telematicsOperatorId.Trim() : "") + "'" + "," + "'" + ((vehicl.telematicsTagId != null) ? vehicl.telematicsTagId.Trim() : "") + "'" + "," + "'" + ((vehicl.driverLicenseNumber != null) ? vehicl.driverLicenseNumber.Trim() : "") + "'" + "," + "'" + ((vehicl.driverRiskRatingCode != null) ? vehicl.driverRiskRatingCode.Trim() : "") + "'" + "," + "'" + ((vehicl.iconType != null) ? vehicl.iconType.Trim() : "") + "'" + "," + "'" + ((vehicl.reasonCriteriaFailed != null) ? vehicl.reasonCriteriaFailed.Trim() : "") + "'" + ","  + DateTime.Now.ToString() + "'" + ")";
                  //  WinService.log.Info(sqlQuery);
                    command = new SqlCeCommand(sqlQuery, con);
                    rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);
                    rs.Close();
                    con.Close();
                   
                }
                WinService.log.Info("Drivers Records Save successfully in VSD_Interest_List_Driver Count =" + InterestedListofDriver.Length);
            }
            catch (Exception ex)
            {

                WinService.log.Info("Exception SaveIntrestedListofDriverLocally" + ex.Message);
            }
        }

        public void ClearListofDriverofIntrestTable()
        {
            try
            {
                try
                {
                    WinService.log.Info("Entering in ClearListofDriverofIntrestTable");
                    SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
                    string sqlQuery;
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    SqlCeCommand command;
                    SqlCeResultSet rs;
                    sqlQuery = "DELETE FROM VSD_Interest_List_Driver";
                  //  WinService.log.Info(sqlQuery);
                    command = new SqlCeCommand(sqlQuery, con);
                    rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);
                    rs.Close();
                    con.Close();
                    WinService.log.Info("VSD_Interest_List_Driver Record deleted successfully");

                }
                catch (Exception ex)
                {
                    WinService.log.Info("Exception in deleting VSD_Interest_List_Driver records" + ex.Message);
                }
            }
            catch (Exception ex)
            {

                WinService.log.Info("Exception SaveIntrestedListofDriverLocally" + ex.Message);
            }
        }

        public DateTime? GetUpdatedTimeStampofDriverList()
        {
            try
            {
                WinService.log.Info("Entering in GetUpdatedTimeStampofDriverList");
                SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
                string sqlQuery;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCeCommand command;
                SqlCeResultSet rs;
                sqlQuery = "SELECT Top(1) TimeStamp  FROM VSD_Interest_List_Driver";
                command = new SqlCeCommand(sqlQuery, con);

                rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);

                int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;

                DateTime dateTime = new DateTime();
                if (rs.Read())
                {
                    dateTime = rs.GetDateTime(0);
                    return dateTime;
                }

                rs.Close();
                con.Close();
                WinService.log.Info("Updated TimeStamp of Driver= " + dateTime);
                return null;
                //   return dateTime;
                
            }
            catch (Exception ex)
            {

                WinService.log.Info("Exception GetUpdatedTimeStampofVehicleList" + ex.Message);
                return null;
            }
        }
        #endregion

        #region Get LoggedINUserName

        public void GetLoggedInUserDetials()
        {
            try
            {
                IDBManager iDBManager = (IDBManager)DBConnectionManager.GetInstance();
                string sql = "select * from VSD_USER_ACCOUNT ";
                WinService.log.Info("\n GetLoggedInUserDetials: SQL" + sql);
                SqlCeConnection con = iDBManager.GetConnection();

                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCeCommand command = new SqlCeCommand(sql, iDBManager.GetConnection());
                 SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);
                int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;
                
                byte[] encryptedBytes;
                string decryptedPassword;
                if (rs.HasRows)
                {
                    rs.ReadFirst();
                    encryptedBytes = (byte[])rs.GetValue(2);
                    decryptedPassword = ((IEncryptionDecryptionManager)new DecryptionManager()).Decrypt(encryptedBytes, encryptionKey, iVector);
                    LoggedInPassword = decryptedPassword;
                    LoggedInUserName = (rs.GetString(1) == null) ? "" : rs.GetString(1).Trim();
                    WinService.log.Info("\n GetLoggedInUserDetials-- USerName:" + LoggedInUserName);
               
                }
            }
            catch (Exception ex)
            {

                WinService.log.Info("Exception GetLoggedInUserDetials" + ex.Message);
            }
        }


        #endregion
    }
}
