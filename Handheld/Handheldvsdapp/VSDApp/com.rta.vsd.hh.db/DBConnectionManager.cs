using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using VSDApp.com.rta.vsd.hh.utilities;
using VSDApp.handHeldService;

namespace VSDApp.com.rta.vsd.hh.db
{
   public class DBConnectionManager : IDBManager
    {
        private static DBConnectionManager _dbConnectionManager;
        private SqlCeConnection _sqlConnection;
        #region IDBManager Members

        public bool SaveUserCredentials(handHeldService.SynchronizeConfigDataResponseItem response)
        {
            throw new NotImplementedException();
        }

        public System.Data.SqlServerCe.SqlCeConnection GetConnection()
        {
            if (_sqlConnection == null)
            {
                _sqlConnection = new SqlCeConnection(AppProperties.connectionString);

            }
            else if (ConnectionState.Closed == _sqlConnection.State)
            {
                _sqlConnection.Open();
            }
            
            return _sqlConnection;
        }
        public static DBConnectionManager GetInstance()
        {
            if (_dbConnectionManager == null)
            {
                _dbConnectionManager = new DBConnectionManager();
                IDBManager iDBManager = (IDBManager)_dbConnectionManager;
            }
            return _dbConnectionManager;
        }
        public bool StoreOfflineData()
        {
            throw new NotImplementedException();
        }

        public bool OfflineDataExist()
        {
            throw new NotImplementedException();
        }

        public bool CreateInitialDataBase()
        {
            throw new NotImplementedException();
        }

        public bool AuthenticateUser(string userName, string userPass)
        {
            throw new NotImplementedException();
        }

        public int SynchronizeConfig(handHeldService.SynchronizeConfigDataResponseItem test)
        {
            throw new NotImplementedException();
        }

        public string[] GetDefectSeverityBusinessRules(string defectName, string defectSubCategory, string defectMainCat, string value)
        {
            throw new NotImplementedException();
        }

        public string[] GetConfigurationDataForDueDays(string severity, int numberOfDefects)
        {
            throw new NotImplementedException();
        }

        public string CalculateSeverity(string currentSeverity, string newSeverity, string type)
        {
            throw new NotImplementedException();
        }

        public string GetRecommendation(DateTime dueDate, string violationStatus, string emirate, string severity)
        {
            throw new NotImplementedException();
        }

        public string GetDefectCodes()
        {
            throw new NotImplementedException();
        }
        VSDApp.com.rta.vsd.hh.data.Violation[] IDBManager.GetOfflineData()
        {
            throw new NotImplementedException();
        }


        #endregion

        #region IDBManager Members
        public DataTable GetDefectSevAndPropertyBusinesRule(string defectID)
        {
            throw new NotImplementedException();
        }

        public void PopulateOnlineLoggedInUserInfo(SynchronizeConfigDataResponseItem response)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

