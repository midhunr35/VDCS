using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using VSDApp.com.rta.vsd.hh.utilities;

namespace VSDApp.com.rta.vsd.hh.db
{
    class DBOfflineDataManager : IDBManager
    {
        private static DBOfflineDataManager _dbOfflineManager;
        private DBOfflineDataManager() { }
        public static DBOfflineDataManager GetInstance()
        {
            if (_dbOfflineManager == null)
            {
                _dbOfflineManager = new DBOfflineDataManager();
            }
            return _dbOfflineManager;
        }

        int GetVehicleCatgeoryID(string vehicleCategory)
        {
           

                SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
                string sqlQuery;

                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCeCommand command;

                sqlQuery = "SELECT Vehicle_category_ID FROM VSD_VEHICLE_CATEGORY WHERE (Category_Name = @vehCat)";
                command = new SqlCeCommand(sqlQuery, con);

                command.Parameters.Add("@vehCat", SqlDbType.NChar, 100).Value = vehicleCategory;

                SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);

                int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;

                int id = -1;
                if (rs.Read())
                {
                    id = (int)rs.GetInt32(0);
                }

                rs.Close();
                con.Close();
                return id;
           
        }

        int GetVehicleSubCatgeoryID(string vehiclesubCategory, string vehicleCategory)
        {


            SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
            string sqlQuery;

            if (con.State == ConnectionState.Closed) { con.Open(); }
            SqlCeCommand command;

            sqlQuery = "SELECT Vehicle_category_ID FROM VSD_VEHICLE_CATEGORY WHERE (Category_Name_A = @vehCat)  AND PARENT_VEHICLE_CATEGORY_ID IN " + " (SELECT Vehicle_Category_ID FROM VSD_VEHICLE_CATEGORY WHERE ISDISABLED ='F' and Category_Name='" + vehicleCategory + "')";
            command = new SqlCeCommand(sqlQuery, con);

            command.Parameters.Add("@vehCat", SqlDbType.NChar, 100).Value = vehiclesubCategory;

            SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);

            int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;

            int id = -1;
            if (rs.Read())
            {
                id = (int)rs.GetInt32(0);
            }

            rs.Close();
            con.Close();
            return id;

        }

        int GetVehicleCatgeoryARID(string vehicleCategory)
        {


            SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
            string sqlQuery;

            if (con.State == ConnectionState.Closed) { con.Open(); }
            SqlCeCommand command;

            sqlQuery = "SELECT Vehicle_category_ID FROM VSD_VEHICLE_CATEGORY WHERE (Category_Name_A = @vehCat)";
            command = new SqlCeCommand(sqlQuery, con);

            command.Parameters.Add("@vehCat", SqlDbType.NChar, 100).Value = vehicleCategory;

            SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);

            int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;

            int id = -1;
            if (rs.Read())
            {
                id = (int)rs.GetInt32(0);
            }

            rs.Close();
            con.Close();
            return id;

        }

        int GetVehicleSubCatgeoryARID(string vehiclesubCategory, string vehicleCategory)
        {


            SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
            string sqlQuery;

            if (con.State == ConnectionState.Closed) { con.Open(); }
            SqlCeCommand command;

            sqlQuery = "SELECT Vehicle_category_ID FROM VSD_VEHICLE_CATEGORY WHERE (Category_Name_A = @vehCat) AND PARENT_VEHICLE_CATEGORY_ID IN " + " (SELECT Vehicle_Category_ID FROM VSD_VEHICLE_CATEGORY WHERE ISDISABLED ='F' and Category_Name_A='" + vehicleCategory + "')";
            command = new SqlCeCommand(sqlQuery, con);

            command.Parameters.Add("@vehCat", SqlDbType.NChar, 100).Value = vehiclesubCategory;

            SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);

            int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;

            int id = -1;
            if (rs.Read())
            {
                id = (int)rs.GetInt32(0);
            }

            rs.Close();
            con.Close();
            return id;

        }

        string GetVehicleCatgeoryCode(string vehicleCategory)
        {
            try
            {

           
            SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
            string sqlQuery;

            if (con.State == ConnectionState.Closed) { con.Open(); }
            SqlCeCommand command;


            sqlQuery = "SELECT category_Code FROM VSD_VEHICLE_CATEGORY WHERE (Category_Name = @vehCat)";
            command = new SqlCeCommand(sqlQuery, con);

            command.Parameters.Add("@vehCat", SqlDbType.NChar, 100).Value = vehicleCategory;


            SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);

            int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;

            string code = "";
            if (rs.Read())
            {
                code = rs.GetString(0);
            }

            rs.Close();
            con.Close();
            return code;
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                return null;
               
            }
        }

        int GetSeverityID(string severityName)
        {
            SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
            string sqlQuery;

            if (con.State == ConnectionState.Closed) { con.Open(); }
            SqlCeCommand command;

            sqlQuery = "SELECT Severity_Level_ID FROM VSD_Severity_Level WHERE SEVERITY_LEVEL_NAME = @sevName AND ISDISABLED = 'F'";
            command = new SqlCeCommand(sqlQuery, con);
            command.Parameters.Add("@sevName", SqlDbType.NChar, 100).Value = severityName;

            SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);

            int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;

            int id = -1;
            if (rs.Read())
            {
                id = (int)rs.GetInt32(0);
            }

            rs.Close();
            con.Close();
            return id;
        }
        string GetSeverityCode(string severityName)
        {
            SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
            string sqlQuery;

            if (con.State == ConnectionState.Closed) { con.Open(); }
            SqlCeCommand command;

            sqlQuery = "SELECT Severity_Level_Code FROM VSD_Severity_Level WHERE SEVERITY_LEVEL_NAME = @sevName AND ISDISABLED = 'F'";
            command = new SqlCeCommand(sqlQuery, con);
            command.Parameters.Add("@sevName", SqlDbType.NChar, 100).Value = severityName;


            SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);

            int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;

            string code = "";
            if (rs.Read())
            {
                code = rs.GetString(0);
            }

            rs.Close();
            con.Close();
            return code;
        }
        int GetLocationID(string emirate, string area, string location)
        {
            Regex regExp = new Regex("[a-zA-Z0-9 ]*");
            bool isEng = regExp.IsMatch(emirate);

            SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
            string sqlQuery;

            if (con.State == ConnectionState.Closed) { con.Open(); }
            SqlCeCommand command;

            sqlQuery = "SELECT DISTINCT VSD_Location.Location_ID FROM VSD_Location INNER JOIN" +
                  " VSD_Country ON VSD_Location.Country_ID = VSD_Country.Country_ID" +
                  " WHERE (VSD_Location.Parent_Location_ID IN (SELECT Location_ID" +
                  " FROM  VSD_Location AS VSD_Location_1 WHERE (Location_Area_Name = @area) AND (Country_ID IN " +
                  " (SELECT Country_ID FROM VSD_Country AS VSD_Country_1 WHERE (Country_Name = @emirate)))))";
            command = new SqlCeCommand(sqlQuery, con);


            command.Parameters.Add("@area", SqlDbType.NChar, 200).Value = area;
            command.Parameters.Add("@emirate", SqlDbType.NChar, 50).Value = emirate;

            SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);

            int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;

            int id = -1;
            if (rs.Read())
            {
                id = (int)rs.GetInt32(0);
            }

            rs.Close();
            con.Close();
            return id;

        }

        public string GetProvisionalCode(int vID)
        {
            string countryCode="";
            try
            {
                 countryCode = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetCountryTicketCode(AppProperties.location.city).Trim();
                 App.VSDLog.Info("countryCode: "+ countryCode+" \n---now loop start---\n");
                string violationID = vID.ToString();
                string severity = string.Empty;
                for (int i = (violationID.Length); i < 7; i++)
                {
                    violationID = "0" + violationID;
                }
                App.VSDLog.Info("--loop on violationID End successfully---now getting vehCat--\n");
                string vehCat = GetVehicleCatgeoryCode(AppProperties.vehicle.VehicleCategory).Trim();
                App.VSDLog.Info("----got vehCat= "+vehCat+"\n--now starting loop on vehCat--\n");
                for (int j = vehCat.Length; j < 2; j++)
                {
                    vehCat = "0" + vehCat;
                }
                App.VSDLog.Info("--loop on vehCat end---Now making time stamp--\n");
                // return "P" + countryCode + "." + AppProperties.authorityCode.Trim() + "." + vehCat + "." + GetSeverityCode((AppProperties.Selected_Resource == "English") ? AppProperties.recordedViolation.ViolationSeverity : AppProperties.recordedViolation.ViolationSeverityAr).Trim() + "." + AppProperties.deviceCode + "." + String.Format("{0:yy}", System.DateTime.Now) + violationID;
                
                String UniqueTimeStamp = String.Format("{0:yy:mm:dd:hh:mm}", System.DateTime.Now);
                UniqueTimeStamp = UniqueTimeStamp.Replace(":", "");

                App.VSDLog.Info("--Unique timestamp--"+UniqueTimeStamp+"\n");

                UniqueTimeStamp = UniqueTimeStamp.Remove(0, 1);
                string authorityCode="01";
                if (AppProperties.authorityCode != null)
                    authorityCode = AppProperties.authorityCode;

                string deviceCode="01";
                severity = GetSeverityCode((AppProperties.Selected_Resource == "English") ? AppProperties.recordedViolation.ViolationSeverity : AppProperties.recordedViolation.ViolationSeverityAr).Trim();
                if (AppProperties.deviceCode != null)
                    deviceCode = AppProperties.deviceCode.ToString().Trim();
                //App.VSDLog.Info("unique timestamp after removal of first digit: " + UniqueTimeStamp);
                //App.VSDLog.Info("\n--Now making retun string--\n authorityCode: " + authorityCode.Trim() + "\n");
                App.VSDLog.Info(" Saverity Code: " + GetSeverityCode((AppProperties.Selected_Resource == "English") ? AppProperties.recordedViolation.ViolationSeverity : AppProperties.recordedViolation.ViolationSeverityAr).Trim() + "\n");
                App.VSDLog.Info("Device Code:" + deviceCode);

                return "P" + countryCode + "." + authorityCode.Trim() + "." + vehCat + "." + severity + "." + deviceCode + "." + UniqueTimeStamp;
            }
            catch (Exception e) {
                App.VSDLog.Info("ViolationID: "+vID+" LocationID and City:"+countryCode+"\n--Log Msg--\n"+e.Message +"\n---Trace----\n" +e.StackTrace );
                return "";
            }
        }

        #region IDBManager Members

        bool IDBManager.SaveUserCredentials(handHeldService.SynchronizeConfigDataResponseItem response)
        {
            throw new NotImplementedException();
        }
        void IDBManager.PopulateOnlineLoggedInUserInfo(handHeldService.SynchronizeConfigDataResponseItem response)
        {
            throw new NotImplementedException();
        }
        System.Data.SqlServerCe.SqlCeConnection IDBManager.GetConnection()
        {
            throw new NotImplementedException();
        }

        int IDBManager.SynchronizeConfig(handHeldService.SynchronizeConfigDataResponseItem test)
        {
            throw new NotImplementedException();
        }

        bool IDBManager.StoreOfflineData()
        {
            try
            {
                if (AppProperties.Is_DeviceInspection)
                {
                    AppProperties.IsStoreOfflineData = false;
                    return false;
                }
                if (AppProperties.vehicle.Country != AppProperties.defaultCountry)// || AppProperties.vehicle.Country != AppProperties.defaultCountryAr)
                {
                    AppProperties.IsStoreOfflineData = false;
                    return false;
                }
                App.VSDLog.Info("Inside Store Offline");
                // Schweers.Localization.ArabicConverter arab = new Schweers.Localization.ArabicConverter();

                SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
                string sqlQuery;

                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCeCommand command = null;

                if (AppProperties.vehicle != null)
                {

                    sqlQuery = "INSERT INTO VSD_VEHICLE_INFO(Vehicle_Category_ID, Vehicle_Plate_Category, Vehicle_Plate_Code, Vehicle_Plate_Source, Vehicle_Plate_Number, Vehicle_Country, Vehicle_Model, " +
                            "Make_Year, Vehicle_Chassis_Number, IsDisabled, Timestamp, Total_Impounding_Days,Mileage,Is_Grace_Period, IsHazard)" +
                            "values(@vID,@vPlCat,@vPlCode,@vPlSource,@vPlNumber,@vCountry," +
                            "@vModel,@vMakeYear,@vChassis,'F',@time,@Total_Impounding_Days,@Mileage,@Is_Grace_Period,@vIsHazard)";

                    command = new SqlCeCommand(sqlQuery, con);
                    //if (AppProperties.Selected_Resource == "English")
                    //{
                    //    command.Parameters.Add("@vID", SqlDbType.Int, 4).Value = GetVehicleCatgeoryID(AppProperties.vehicle.VehicleCategory);
                    //}
                    //else
                    //{
                    //    command.Parameters.Add("@vID", SqlDbType.Int, 4).Value = GetVehicleCatgeoryID(AppProperties.vehicle.VehicleCategory);
                    //}
                    int vid=0;
                    vid = GetVehicleSubCatgeoryARID(AppProperties.vehicle.SubCategoryAr, AppProperties.vehicle.VehicleCategoryAr);
                    if(vid==0)
                    {
                        vid = GetVehicleSubCatgeoryID(AppProperties.vehicle.SubCategoryAr, AppProperties.vehicle.VehicleCategory);
                    }
                    if (vid==0)
                    {
                        if (AppProperties.Selected_Resource == "English")
                        {
                            vid = GetVehicleCatgeoryID(AppProperties.vehicle.VehicleCategory);
                        }
                        else
                        {
                            vid = GetVehicleCatgeoryID(AppProperties.vehicle.VehicleCategory);
                        }

                    }
                    command.Parameters.Add("@vID", SqlDbType.Int, 4).Value = vid;


                    //command.Parameters.Add("@vID", SqlDbType.Int, 4).Value = GetVehicleCatgeoryID(AppProperties.vehicle.VehicleCategory);

                    App.VSDLog.Info("Sql Query VSD_VEHICLE_INFO" + sqlQuery);
                    if (con.State == ConnectionState.Closed) { con.Open(); }

                    command.Parameters.Add("@vPlCat", SqlDbType.NChar, 100).Value = AppProperties.vehicle.PlateCategory;
                    command.Parameters.Add("@vPlCode", SqlDbType.NChar, 100).Value = AppProperties.vehicle.PlateCode;
                    command.Parameters.Add("@vPlSource", SqlDbType.NChar, 100).Value = AppProperties.vehicle.Emirate;
                    command.Parameters.Add("@vPlNumber", SqlDbType.NChar, 100).Value = AppProperties.vehicle.PlateNumber;
                    command.Parameters.Add("@vCountry", SqlDbType.NChar, 100).Value = AppProperties.vehicle.Country;
                    command.Parameters.Add("@vModel", SqlDbType.NChar, 100).Value = AppProperties.vehicle.Model;
                    command.Parameters.Add("@vMakeYear", SqlDbType.NChar, 100).Value = AppProperties.vehicle.Make + AppProperties.vehicle.Year;
                    command.Parameters.Add("@vChassis", SqlDbType.NChar, 100).Value = AppProperties.vehicle.ChassisNumber;
                    command.Parameters.Add("@time", SqlDbType.DateTime, 8).Value = System.DateTime.Now;
                    command.Parameters.Add("@Total_Impounding_Days", SqlDbType.NChar, 50).Value =(AppProperties.vehicle.TotalImpoundingDays != "") ? AppProperties.vehicle.TotalImpoundingDays.Trim() : "0";
                    command.Parameters.Add("@Mileage", SqlDbType.NChar, 50).Value = (AppProperties.vehicle.Mileage != "") ? AppProperties.vehicle.Mileage.Trim() : "0";
                    command.Parameters.Add("@Is_Grace_Period", SqlDbType.NChar, 50).Value = AppProperties.vehicle.IsImpoundingGracePeriod;
                   command.Parameters.Add("@vIsHazard", SqlDbType.NChar, 5).Value = AppProperties.vehicle.IsHazard.ToString();
                  
				    int rowsAffected = command.ExecuteNonQuery();

                    sqlQuery = "SELECT Vehicle_Info_ID FROM VSD_Vehicle_Info ORDER BY Vehicle_Info_ID DESC";
                    command = new SqlCeCommand(sqlQuery, con);

                    SqlCeResultSet res = command.ExecuteResultSet(ResultSetOptions.Scrollable);
                    int vInfoID = -1;
                    if (res.HasRows)
                    {
                        res.Read();
                        vInfoID = Convert.ToInt32(res["VEHICLE_INFO_ID"]);
                        res.Close();
                    }
                    App.VSDLog.Info("Sql Query Update" + sqlQuery);

                    if (AppProperties.vehicle.Operator != null)
                    {
                        sqlQuery = "INSERT INTO VSD_Owner_Info(Vehicle_Info_ID, Owner_Name, Owner_Name_A, Trade_License_Number, Traffic_File_Number, IsDisabled, Timestamp) " +
                            " values (@vInfoID,@OwnerName,@OwnerNameAr,@TLN,@TFN,'F',@time)";
                        command = new SqlCeCommand(sqlQuery, con);
                        command.Parameters.Add("@vInfoID", SqlDbType.Int, 4).Value = vInfoID;
                        command.Parameters.Add("@OwnerName", SqlDbType.NChar, 100).Value = (AppProperties.vehicle.Operator.OperatorName != null) ? AppProperties.vehicle.Operator.OperatorName : "NULL";
                        command.Parameters.Add("@OwnerNameAr", SqlDbType.NVarChar, 100).Value = (AppProperties.vehicle.Operator.OperatorNameAr != null) ? AppProperties.vehicle.Operator.OperatorNameAr : "NULL";
                        command.Parameters.Add("@TLN", SqlDbType.NChar, 100).Value = (AppProperties.vehicle.TradeLicenseNumber != null) ? AppProperties.vehicle.TradeLicenseNumber : "NULL";
                        command.Parameters.Add("@TFN", SqlDbType.NChar, 100).Value = (AppProperties.vehicle.Operator.TrafficFileNumber != null) ? AppProperties.vehicle.Operator.TrafficFileNumber : "NULL";
                        command.Parameters.Add("@time", SqlDbType.DateTime, 8).Value = System.DateTime.Now;


                        rowsAffected = command.ExecuteNonQuery();
                        App.VSDLog.Info("Sql Query VSD_Owner_Info" + sqlQuery);

                    }



                    if (AppProperties.vehicle.DriverLicense != null || "".Equals(AppProperties.vehicle.DriverLicense))
                    {
                        sqlQuery = "INSERT INTO VSD_Driver_Info (Vehicle_Info_ID, Driver_Name, Driver_Name_A, Driver_License_Number, IsDisabled, Timestamp)" +
                        "values (@vInfoID,@DriverName,@DriverNameAr,@DLN,'F',@time)";
                        command = new SqlCeCommand(sqlQuery, con);
                        command.Parameters.Add("@vInfoID", SqlDbType.Int, 4).Value = vInfoID;
                        command.Parameters.Add("@DriverName", SqlDbType.NChar, 100).Value = (AppProperties.vehicle.DriverName != null) ? AppProperties.vehicle.DriverName : "NULL";
                        command.Parameters.Add("@DriverNameAr", SqlDbType.NVarChar, 100).Value = (AppProperties.vehicle.DriverNameAr != null) ? AppProperties.vehicle.DriverNameAr : "NULL";
                        command.Parameters.Add("@DLN", SqlDbType.NChar, 100).Value = (AppProperties.vehicle.DriverLicense != null) ? AppProperties.vehicle.DriverLicense : "NULL";
                        command.Parameters.Add("@time", SqlDbType.DateTime, 8).Value = System.DateTime.Now;
                        

                        rowsAffected = command.ExecuteNonQuery();
                        App.VSDLog.Info("Sql Query VSD_Driver_Info" + sqlQuery);

                    }



                    int vioID = -1;

                    if (AppProperties.recordedViolation != null)
                    {
                        App.VSDLog.Info("AppProperties.recordedViolation.Defect" + AppProperties.recordedViolation.Defect);
                        if (null != AppProperties.recordedViolation.Defect && AppProperties.recordedViolation.Defect.Length > 0)
                        {



                            if (con.State == ConnectionState.Closed) { con.Open(); }
                            sqlQuery = "INSERT INTO VSD_Violation(Severity_Level_ID,Violation_Ticket_Code,  Due_Date, "
                                + "Comments, Comments_A, Timestamp, Violation_Status, Created_By)" +
                                " values (@sevID,@VTC,@dueDate,@comments,@commentsAr,@time,'Open',@empName)";
                            command = new SqlCeCommand(sqlQuery, con);

                            command.Parameters.Add("@sevID", SqlDbType.Int, 4).Value = GetSeverityID(AppProperties.recordedViolation.ViolationSeverity);
                            if (con.State == ConnectionState.Closed) { con.Open(); }
                            command.Parameters.Add("@VTC", SqlDbType.NChar, 50).Value = "01";
                            command.Parameters.Add("@dueDate", SqlDbType.DateTime, 8).Value = AppProperties.recordedViolation.ViolationDueDays;
                            command.Parameters.Add("@comments", SqlDbType.NChar, 100).Value = (AppProperties.recordedViolation.ViolationComments != null) ? AppProperties.recordedViolation.ViolationComments : "NULL";
                            command.Parameters.Add("@commentsAr", SqlDbType.NVarChar, 100).Value = (AppProperties.recordedViolation.ViolationCommentsAr != null) ? AppProperties.recordedViolation.ViolationCommentsAr : "NULL";
                            command.Parameters.Add("@time", SqlDbType.DateTime, 8).Value = System.DateTime.Now;
                            if (AppProperties.recordedViolation.Inspector == null)
                            {
                                AppProperties.recordedViolation.Inspector = AppProperties.empUserName;
                            }
                            command.Parameters.Add("@empName", SqlDbType.NVarChar, 100).Value = (AppProperties.recordedViolation.Inspector != null) ? AppProperties.recordedViolation.Inspector : "NULL";
                            if (con.State == ConnectionState.Closed) { con.Open(); }
                            rowsAffected = command.ExecuteNonQuery();

                            App.VSDLog.Info("SQL " + sqlQuery);
                            // get the current ID and the n update the provisional

                            if (con.State == ConnectionState.Closed) { con.Open(); }
                            sqlQuery = "SELECT Violation_ID FROM VSD_Violation ORDER BY Violation_ID DESC";
                            command = new SqlCeCommand(sqlQuery, con);

                            SqlCeResultSet resultSet = command.ExecuteResultSet(ResultSetOptions.Scrollable);


                            if (resultSet.HasRows)
                            {
                                resultSet.Read();
                                vioID = Convert.ToInt32(resultSet["Violation_ID"]);
                                resultSet.Close();
                            }

                            if (con.State == ConnectionState.Closed) { con.Open(); }
                            sqlQuery = "Update VSD_Violation set Violation_Ticket_Code = @tickID where Violation_ID = @vid";
                            command = new SqlCeCommand(sqlQuery, con);
                            command.Parameters.Add("@vid", SqlDbType.Int).Value = vioID;
                            command.Parameters.Add("@tickID", SqlDbType.NChar).Value = (AppProperties.recordedViolation.ViolationTicketCode = GetProvisionalCode(vioID));
                            //SqlCeResultSet resultSet = command.ExecuteResultSet(ResultSetOptions.Scrollable);
                            if (con.State == ConnectionState.Closed) { con.Open(); }
                            rowsAffected = command.ExecuteNonQuery();



                            if (con.State == ConnectionState.Closed) { con.Open(); }
                            for (int i = 0; i < AppProperties.recordedViolation.Defect.Length; i++)
                            {

                                sqlQuery = "INSERT INTO VSD_Channel_Defect(Defect_ID, Violation_ID, Defect_Value)" +
                                    " values (@dID,@vID,@dValue)";
                                command = new SqlCeCommand(sqlQuery, con);
                                command.Parameters.Add("@dID", SqlDbType.Int, 4).Value = AppProperties.recordedViolation.Defect[i].DefectID;
                                command.Parameters.Add("@vID", SqlDbType.Int, 4).Value = vioID;
                                command.Parameters.Add("@dValue", SqlDbType.NVarChar, 100).Value = (AppProperties.recordedViolation.Defect[i].DefectValue != null) ? AppProperties.recordedViolation.Defect[i].DefectValue : "NULL";

                                rowsAffected = command.ExecuteNonQuery();
                            }
                            //call all values to get the provisional ticket code.
                            App.VSDLog.Info("\nVSD_Channel_Defect: " + sqlQuery);

                        }
                    }


                    sqlQuery = "INSERT INTO VSD_Inspection(Vehicle_Info_ID, Violation_ID, Location_ID, Plate_Condition, Is_Plate_Confiscated, Plate_Confiscation_Reason, Plate_Confiscation_reason_A, " +
                            "Reported_Inspector_Name_A, Inspection_Timestamp)" +
                            " values (@vInfoID,@vID,@locationID,@plCondition,@isConfiscated,@plConfiscationreason,@plConfiscationReasonAr,@InspectorName,@time)";
                    command = new SqlCeCommand(sqlQuery, con);
                    command.Parameters.Add("@vInfoID", SqlDbType.Int, 4).Value = vInfoID;
                    command.Parameters.Add("@vID", SqlDbType.Int, 4).Value = vioID;
                    command.Parameters.Add("@locationID", SqlDbType.Int, 4).Value = GetLocationID(AppProperties.recordedViolation.InspectionArea.city, AppProperties.recordedViolation.InspectionArea.area, AppProperties.recordedViolation.InspectionArea.location);
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    command.Parameters.Add("@plCondition", SqlDbType.NVarChar, 100).Value = (AppProperties.recordedViolation.PlateCondition != null) ? AppProperties.recordedViolation.PlateCondition : "NULL";
                    command.Parameters.Add("@isConfiscated", SqlDbType.NChar, 1).Value = (AppProperties.recordedViolation.IsConfiscated) ? "T" : "F";
                    command.Parameters.Add("@plConfiscationreason", SqlDbType.NChar, 100).Value = (AppProperties.recordedViolation.ConfiscationReason != null) ? AppProperties.recordedViolation.ConfiscationReason : "NULL";
                    command.Parameters.Add("@plConfiscationReasonAr", SqlDbType.NVarChar, 100).Value = (AppProperties.recordedViolation.ConfiscationReasonAr != null) ? AppProperties.recordedViolation.ConfiscationReasonAr : "NULL";
                    if (AppProperties.recordedViolation.Inspector == null)
                    {
                        AppProperties.recordedViolation.Inspector = AppProperties.empUserName;
                    }
                    command.Parameters.Add("@InspectorName", SqlDbType.NVarChar, 100).Value = (AppProperties.recordedViolation.Inspector != null) ? AppProperties.recordedViolation.Inspector : "NULL";
                    command.Parameters.Add("@time", SqlDbType.DateTime, 8).Value = System.DateTime.Now;

                    rowsAffected = command.ExecuteNonQuery();
                    App.VSDLog.Info("\nVSD_Inspection: " + sqlQuery);
                }
                con.Close();
                //System.Windows.MessageBox.Show("Data stored in local database");
                // System.Windows.Forms.MessageBox.Show("Data stored in local database");
                AppProperties.IsStoreOfflineData = true;

            }
            catch (Exception e)
            {
              //  System.Windows.Forms.MessageBox.Show(e.Message);
                 App.VSDLog.Info(e.StackTrace);
                 App.VSDLog.Info("Error Message on Save offline violation"+e.Message);
               // System.Windows.MessageBox.Show("Unable to store data");
                AppProperties.IsStoreOfflineData = false;
                return false;
            }
            return true;
        }

        VSDApp.com.rta.vsd.hh.data.Violation[] IDBManager.GetOfflineData()
        {
            throw new NotImplementedException();
        }

        bool IDBManager.OfflineDataExist()
        {
            throw new NotImplementedException();
        }

        bool IDBManager.CreateInitialDataBase()
        {
            throw new NotImplementedException();
        }

        bool IDBManager.AuthenticateUser(string userName, string userPass)
        {
            throw new NotImplementedException();
        }

        string[] IDBManager.GetDefectSeverityBusinessRules(string defectName, string defectSubCategory, string defectMainCat, string value)
        {
            throw new NotImplementedException();
        }

        string[] IDBManager.GetConfigurationDataForDueDays(string severity, int numberOfDefects)
        {
            throw new NotImplementedException();
        }


        string IDBManager.CalculateSeverity(string currentSeverity, string newSeverity, string type)
        {
            throw new NotImplementedException();
        }

        string IDBManager.GetRecommendation(DateTime dueDate, string violationStatus, string emirate, string severity)
        {
            throw new NotImplementedException();
        }
        string IDBManager.GetDefectCodes()
        {
            throw new NotImplementedException();
        }
        public DataTable GetDefectSevAndPropertyBusinesRule(string defectID)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region IDBManager Members


        

        #endregion
    }
}
