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
    class DBBusinessRuleManager : IDBManager
    {
        private static DBBusinessRuleManager _dbBusinessManager;
        private DBBusinessRuleManager() { }

        public static DBBusinessRuleManager GetInstance()
        {
            if (_dbBusinessManager == null)
            {
                _dbBusinessManager = new DBBusinessRuleManager();
            }
            return _dbBusinessManager;
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

        int IDBManager.SynchronizeConfig(handHeldService.SynchronizeConfigDataResponseItem test)
        {
            throw new NotImplementedException();
        }

        System.Data.SqlServerCe.SqlCeConnection IDBManager.GetConnection()
        {
            throw new NotImplementedException();
        }

        bool IDBManager.StoreOfflineData()
        {
            throw new NotImplementedException();
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

        int GetDefectID(string defectName, string defectSubCat, string defectMainCat)
        {
            SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
            string sqlQuery;

            if (con.State == ConnectionState.Closed) { con.Open(); }
            SqlCeCommand command;
            

            sqlQuery = "SELECT Defect_ID FROM VSD_Defect WHERE     (DEFECT_Category_ID IN " +
                      "(SELECT     DEFECT_Category_ID FROM VSD_DEFECT_CATEGORY " +
                      "WHERE      (Category_Name = @defectSubCat) AND (Parent_DEFECT_Category_ID IN" +
                      "(SELECT     Defect_Category_ID FROM          VSD_DEFECT_CATEGORY AS VSD_DEFECT_CATEGORY_1 " +
                      "WHERE   (Category_Name = @defectMainCat))))) AND (VSD_DEFECT.DEFECT_NAME = @_defectName)";
            command = new SqlCeCommand(sqlQuery, con);

            command.Parameters.Add("@_defectName", SqlDbType.NChar).Value = defectName;
            command.Parameters.Add("@defectSubCat", SqlDbType.NChar).Value = defectSubCat;
            command.Parameters.Add("@defectMainCat", SqlDbType.NChar).Value = defectMainCat;

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
        int GetVehicleCategoryID(string vehicleCategory)
        {
            SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
            string sqlQuery;

            if (con.State == ConnectionState.Closed) { con.Open(); }
            SqlCeCommand command;
           
            sqlQuery = "SELECT Vehicle_category_ID FROM VSD_VEHICLE_CATEGORY WHERE (Category_Name = @vehCat)";
            command = new SqlCeCommand(sqlQuery, con);

            command.Parameters.Add("@vehCat", SqlDbType.NChar).Value = vehicleCategory;

           

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
        int GetSeverityForDefectAndVehicleCategory(int defectID, int VehID)
        {

            SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
            string sqlQuery;

            if (con.State == ConnectionState.Closed) { con.Open(); }
            SqlCeCommand command;

            sqlQuery = "SELECT Severity_Level_ID FROM VSD_Veh_Cat_Defect WHERE Defect_ID = @_defectID and Vehicle_Category_ID = @vehID";
            command = new SqlCeCommand(sqlQuery, con);
            command.Parameters.Add("@_defectID", SqlDbType.Int, 4).Value = defectID;
            command.Parameters.Add("@vehID", SqlDbType.Int, 4).Value = VehID;


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
        int GetSeverityLevel(string severityName, string type)
        {
            SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
            string sqlQuery;

            if (con.State == ConnectionState.Closed) { con.Open(); }
            SqlCeCommand command;


            sqlQuery = "SELECT Severity_Level FROM VSD_Severity_Level WHERE SEVERITY_LEVEL_NAME = @sevName AND ISDISABLED = 'F'";
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

        DataTable IDBManager.GetDefectSevAndPropertyBusinesRule(string defectID)
        {
            try
            {
                SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
                string sqlQuery;
                DataTable dtSevProp = new DataTable();

                int defID = Convert.ToInt32(defectID);

                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCeCommand command;
                int vehicleID = GetVehicleCategoryID(AppProperties.vehicle.VehicleCategory);

                int severityID = GetSeverityForDefectAndVehicleCategory(defID, vehicleID);

                sqlQuery = "SELECT DISTINCT VSD_Defect.Defect_Type, VSD_Defect.defect_name, VSD_Severity_Level.Severity_Level_Name, VSD_Severity_Level.Severity_Level_Name_A, " +
                             " VSD_Defect.Defect_ID, VSD_Defect.defect_name_a, VSD_DEFECT_PROPERTY.ENFORCE_TESTING, VSD_DEFECT_PROPERTY.ENFORCE_FINE " +
                              " FROM VSD_Defect INNER JOIN VSD_Veh_Cat_Defect ON VSD_Defect.Defect_ID = VSD_Veh_Cat_Defect.Defect_ID INNER JOIN " +
                             " VSD_Severity_Level ON VSD_Veh_Cat_Defect.Severity_Level_ID = VSD_Severity_Level.Severity_Level_ID INNER JOIN " +
                             " VSD_Defect_Category ON VSD_Defect.Defect_Category_ID = VSD_Defect_Category.Defect_Category_ID INNER JOIN " +
                             " VSD_DEFECT_PROPERTY ON VSD_DEFECT_PROPERTY.DEFECT_ID = VSD_Defect.Defect_ID " +
                             " WHERE   (VSD_Defect.Defect_ID = '" + defectID + "') AND (VSD_Severity_Level.Severity_Level_ID = '" + severityID + "') ";
                command = new SqlCeCommand(sqlQuery, con);
                dtSevProp = new DataTable();
                if (con.State == ConnectionState.Closed) { con.Open(); }
                dtSevProp.Load(command.ExecuteReader());

                con.Close();
                return dtSevProp;
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                return null;
            }
        }

        string[] IDBManager.GetDefectSeverityBusinessRules(string defectName, string defectSubCategory, string defectMainCat, string value)
        {
           // Schweers.Localization.ArabicConverter arab = new Schweers.Localization.ArabicConverter();

            int defectID = GetDefectID(defectName, defectSubCategory, defectMainCat);
            int vehicleID = GetVehicleCategoryID(AppProperties.vehicle.VehicleCategory);
            int severityID = GetSeverityForDefectAndVehicleCategory(defectID, vehicleID);

            SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
            string sqlQuery;

            if (con.State == ConnectionState.Closed) { con.Open(); }
            SqlCeCommand command;
            sqlQuery = "SELECT DISTINCT VSD_Defect.Defect_Type, VSD_Defect.Defect_Name, VSD_Severity_Level.Severity_Level_Name " +
                            ",VSD_Severity_Level.Severity_Level_Name_A, VSD_Defect.Defect_ID,VSD_Defect.Defect_Name_A,VSD_Defect_Property.ENFORCE_TESTING, VSD_Defect_Property.ENFORCE_FINE" +
                            " FROM  VSD_Defect INNER JOIN " +
                            " VSD_Veh_Cat_Defect ON VSD_Defect.Defect_ID = VSD_Veh_Cat_Defect.Defect_ID INNER JOIN " +
                            " VSD_Severity_Level ON VSD_Veh_Cat_Defect.Severity_Level_ID = VSD_Severity_Level.Severity_Level_ID INNER JOIN " +
                            " VSD_Severity_Level_Prop ON VSD_Severity_Level.Severity_Level_ID = VSD_Severity_Level_Prop.Severity_Level_ID INNER JOIN " +
                            " VSD_Defect_Category ON VSD_Defect.Defect_Category_ID = VSD_Defect_Category.Defect_Category_ID INNER JOIN " +
                            " VSD_Defect_Property ON VSD_Defect_Property.DEFECT_ID = VSD_Defect.Defect_ID"+

                            " WHERE  (VSD_Defect.Defect_ID = @_defectID) AND (VSD_Severity_Level.Severity_Level_ID = @SevID) AND VSD_Severity_Level.ISDISABLED ='F'";


            command = new SqlCeCommand(sqlQuery, con);
            command.Parameters.Add("@_defectID", SqlDbType.Int, 4).Value = defectID;
            command.Parameters.Add("@sevID", SqlDbType.Int, 4).Value = severityID;

            SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);
            int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;
            List<string> defectDetails = new List<string>();

            if (rowCount > 0)
            {
                rs.ReadFirst();
                try
                {
                    defectDetails.Add(defectID.ToString());
                    defectDetails.Add(rs.GetString(0));
                    defectDetails.Add(defectSubCategory);
                    defectDetails.Add(defectMainCat);
                    defectDetails.Add(rs.GetString(1));
                    defectDetails.Add(rs.GetString(2));
                    defectDetails.Add(rs.GetString(3));
                    defectDetails.Add(value);
                    defectDetails.Add(rs.GetInt32(4).ToString());
                    if (!rs.IsDBNull(5))
                    {
                        defectDetails.Add(rs.GetString(5));
                    }
                    else
                    {
                        defectDetails.Add("");
                    }
                    defectDetails.Add(rs.GetString(6));
                    defectDetails.Add(rs.GetString(7));

                   


                    // defectDetails.Add(rs.GetString(4));
                }
                catch (System.Data.SqlTypes.SqlNullValueException ex)
                {
                    //App.VSDLog.Info(ex.StackTrace);
                    return null;
                }

            }
            rs.Close();
            con.Close();
            return (defectDetails.Count > 0) ? defectDetails.ToArray() : null;

        }
        

        string[] IDBManager.GetConfigurationDataForDueDays(string severity, int numberOfDefects)
        {

            SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
            string sqlQuery;

            int severityID = GetSeverityID(severity);
            List<string> data = new List<string>();


            if (con.State == ConnectionState.Closed) { con.Open(); }
            SqlCeCommand command;

            sqlQuery = "SELECT DISTINCT VSD_Severity_Level_Prop.Due_Days, VSD_Severity_Level_Prop.Vehicle_Service_Suspension_Days, " +
                      " VSD_Severity_Level_Prop.Comp_Service_Suspension_Days, VSD_Severity_Level_Prop.Req_Plate_Confiscation, " +
                      " VSD_Severity_Level_Prop.Receipt_Title,VSD_Severity_Level_Prop.Receipt_Title_A,VSD_Severity_Level_Prop.Plate_Confiscation_Days FROM  VSD_Severity_Level INNER JOIN " +
                      " VSD_Severity_Level_Prop ON VSD_Severity_Level.Severity_Level_ID = VSD_Severity_Level_Prop.Severity_Level_ID" +
                      " WHERE (VSD_Severity_Level.Severity_Level_ID = @sevID) AND (VSD_Severity_Level_Prop.IsDisabled = 'F') AND (@noOfDefects BETWEEN " +
                      " VSD_Severity_Level_Prop.Min_No_Of_Defects AND VSD_Severity_Level_Prop.Max_No_Of_Defects)";

            command = new SqlCeCommand(sqlQuery, con);
            command.Parameters.Add("@sevID", SqlDbType.Int, 4).Value = severityID;
            command.Parameters.Add("@noOfDefects", SqlDbType.Int, 4).Value = numberOfDefects;





            SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);
            int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;


            if (rowCount > 0)
            {
                rs.ReadFirst();
                try
                {

                    data.Add(rs.GetInt32(0).ToString());
                    data.Add(rs.GetInt32(1).ToString());
                    data.Add(rs.GetInt32(2).ToString());
                    data.Add(rs.GetString(3).Trim());
                    data.Add(rs.GetString(4).Trim());
                    data.Add(rs.GetString(5).Trim());
                    data.Add(rs.GetInt32(6).ToString());


                }
                catch (System.Data.SqlTypes.SqlNullValueException ex)
                {
                    //App.VSDLog.Info(ex.StackTrace);
                    //MessageBox.Show(ex.Message);
                    
                    return null;
                }

            }
            rs.Close();
            con.Close();
            return (data.Count > 0) ? data.ToArray() : null;


        }
        string IDBManager.CalculateSeverity(string currentSeverity, string newSeverity, string type)
        {
            if (currentSeverity == null)
            {
                return newSeverity;
            }
            else
            {
                int currentLevel = GetSeverityLevel(currentSeverity, type);
                int newLevel = GetSeverityLevel(newSeverity, type);

                if (newLevel >= currentLevel)
                {
                    return newSeverity;
                }
                else
                {
                    return currentSeverity;
                }
            }
            return null;
        }
        string IDBManager.GetRecommendation(DateTime dueDate, string violationStatus, string emirate, string severity)
        {
            //checked for Non dubai and other emirates and status of violation
            if (!emirate.Equals(AppProperties.defaultEmirate) || !violationStatus.Equals("Open"))
            {
                return "";
            }
            else if (emirate.Equals(AppProperties.defaultEmirate) && violationStatus.Equals("Open"))
            {

                Regex regExp = new Regex("[A-Za-z0-9]*");
                bool isEng = regExp.IsMatch(severity);

                if (isEng)
                {
                    if (severity.Equals("Severe", StringComparison.CurrentCultureIgnoreCase) || severity.Equals("Major", StringComparison.CurrentCultureIgnoreCase))
                    {
                        AppProperties.canRecordDefects = false;
                    }
                }
                else
                {
                    if (severity.Equals("Severe", StringComparison.CurrentCultureIgnoreCase) || severity.Equals("Major", StringComparison.CurrentCultureIgnoreCase))
                   // if(severity.Equals("Severe"),StringComparison.CurrentCultureIgnoreCase) || severity.Equals("Major"), StringComparison.CurrentCultureIgnoreCase))
                    {
                        AppProperties.canRecordDefects = false;
                    }
                }

                return "";
            }
            return "";
        }




        string IDBManager.GetDefectCodes()
        {
            string start = "(";
            string end = ")";
            string value = "'-2000'";
            bool firstEntry = true;
            if (null != AppProperties.vehicle.Violations && AppProperties.vehicle.Violations.Length > 0)
            {
                for (int i = 0; i < AppProperties.vehicle.Violations.Length; i++)
                {
                    if (AppProperties.vehicle.Violations[i].ViolationStatus.Equals("Open", StringComparison.CurrentCultureIgnoreCase))
                    {
                        for (int j = 0; j < AppProperties.vehicle.Violations[i].Defect.Length; j++)
                        {
                            if (AppProperties.vehicle.Violations[i].Defect[j].DefectType.Equals("Defect", StringComparison.CurrentCultureIgnoreCase))
                            {
                                if (firstEntry)
                                {
                                    value = "'" + AppProperties.vehicle.Violations[i].Defect[j].DefectID.ToString() + "'";
                                    firstEntry = false;
                                }
                                else
                                {
                                    value += "," + "'" + AppProperties.vehicle.Violations[i].Defect[j].DefectID.ToString() + "'";
                                }

                            }
                        }
                    }
                }

            }

            return start + value + end;

        }

        #endregion

        #region IDBManager Members




        #endregion
    }
}
