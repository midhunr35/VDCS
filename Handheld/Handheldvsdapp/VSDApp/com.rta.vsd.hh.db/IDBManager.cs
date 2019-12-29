using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using VSDApp.com.rta.vsd.hh.data;


namespace VSDApp.com.rta.vsd.hh.db
{
   public interface IDBManager
    {
       bool SaveUserCredentials(handHeldService.SynchronizeConfigDataResponseItem response);
       void PopulateOnlineLoggedInUserInfo(handHeldService.SynchronizeConfigDataResponseItem response);
        SqlCeConnection GetConnection();
        
        bool StoreOfflineData();
        Violation[] GetOfflineData();
        bool OfflineDataExist();
        bool CreateInitialDataBase();
        bool AuthenticateUser(string userName, string userPass);
        int SynchronizeConfig(handHeldService.SynchronizeConfigDataResponseItem test);

        string[] GetDefectSeverityBusinessRules(string defectName, string defectSubCategory, string defectMainCat, string value);
        string[] GetConfigurationDataForDueDays(string severity, int numberOfDefects);
        string CalculateSeverity(string currentSeverity, string newSeverity, string type);
        string GetRecommendation(DateTime dueDate, string violationStatus, string emirate, string severity);
        string GetDefectCodes();
        DataTable GetDefectSevAndPropertyBusinesRule(string defectID);
    }
}
