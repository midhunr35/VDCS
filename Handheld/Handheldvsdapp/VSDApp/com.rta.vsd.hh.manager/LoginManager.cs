using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using VSDApp.com.rta.vsd.hh.db;
using VSDApp.com.rta.vsd.hh.utilities;
using VSDApp.WPFMessageBoxControl;

namespace VSDApp.com.rta.vsd.hh.manager
{
    class LoginManager : ILoginManager
    {
        private static LoginManager _loginManager;

        #region ILoginManager Members

        void ILoginManager.SetupServiceCall()
        {
            throw new NotImplementedException();
        }

        bool ILoginManager.OfflineLogin(string userName, string userPass)
        {
            IDBManager iDBManager = (IDBManager)DBAppLoginManager.GetInstance();
            ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetAuthCode();
            return iDBManager.AuthenticateUser(userName, userPass);
        }

        bool ILoginManager.LoginOnline(string username, string password)
        {
            try
            {
                // handHeldService.HandHeldService service = new VSDApp.handHeldService.HandHeldService();
                App.VSDLog.Info("\n ILoginManager.LoginOnline: --------------------START---------------------- ");
                
                handHeldService.HandHeldService service = new handHeldService.HandHeldService();
                handHeldService.AuthHeader authorize = new VSDApp.handHeldService.AuthHeader();


               
                authorize.userName = username;
                authorize.password = password;
                service.authHeader = authorize;
                
               // DateTime dateTime = new DateTime(2011, 08, 01);
                DateTime dateTime = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetAppSynTime();
                                                                                                              
                AppProperties.deviceCode = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDeviceCode().Trim();
                App.VSDLog.Info("\n ILoginManager.LoginOnline: Device Code: " + AppProperties.deviceCode.ToString());
                App.VSDLog.Info("\n ILoginManager.LoginOnline: Date Time: " + dateTime.ToString());

                //dateTime = DateTime.Now;
                //App.VSDLog.Info("changed Date " + dateTime.ToString());



                App.VSDLog.Info("\n UserName: " + username);
                //App.VSDLog.Info("\n Password: " + password);
                App.VSDLog.Info("\n AppProperties.empUserName: " + AppProperties.empUserName);
              
                 handHeldService.SynchronizeConfigDataResponseItem test = service.synchronizeConfigData("RC123", dateTime, AppProperties.deviceCode, AppProperties.empUserName);
                 App.VSDLog.Info("\n ILoginManager.LoginOnline: service.synchronizeConfigData() Service Call");
                // handHeldService.SynchronizeConfigDataResponseItem test = service.synchronizeConfigData("RC123", dateTime,AppProperties.deviceCode, AppProperties.empUserName);
                AppProperties.empID = test.employeeId;
                 App.VSDLog.Info("\n ILoginManager.LoginOnline: Employee ID: " + AppProperties.empID);
                AppProperties.authorityCode = test.authorityCode;
                IDBManager iDBManager = (IDBManager)DBAppLoginManager.GetInstance();
                if (test.response != null)
                {
                   // App.VSDLog.Info("\n ILoginManager.LoginOnline: service response Message:  " + test.response.message);
                    if (test.response.code != null)
                    {
                        App.VSDLog.Info("\n ILoginManager.LoginOnline: service response code:  " + test.response.code);
                    }
                }
                if (test.response.code.Equals("1000"))
                {
                    App.VSDLog.Info("\n ILoginManager.LoginOnline: service response code:1000");
                    int responseCode = iDBManager.SynchronizeConfig(test);
                  //  int responseCode = 1000;

                    if (responseCode == 1000)
                    {
                        ((IDBDataLoad)DBDataLoadManager.GetInstance()).UpdateApplicationSynTime();
                        ((IDBDataLoad)DBDataLoadManager.GetInstance()).SetAuthCode(test.authorityCode);
                      //  iDBManager.SaveUserCredentials();
 						iDBManager.SaveUserCredentials(test);
                        iDBManager.PopulateOnlineLoggedInUserInfo(test);
                        return true;
                    }
                    return false;
                }
                else if (test.response.code.Equals("2000"))
                {
                   // App.VSDLog.Info("\n ILoginManager.LoginOnline: service response code:2000");
                    AppProperties.businessError = true;
                    AppProperties.errorMessageFromBusiness = test.response.message;
                    return true;
                }
                else
                {
                    // System.Windows.Forms.MessageBox.Show(test.response.message);
                    return false;
                }
            }
            catch (Exception ex)
            {
                App.VSDLog.Info("\n ILoginManager.LoginOnline: exception while calling loginservice");
                App.VSDLog.Info("\n ILoginManager.LoginOnline after exp: "+ex.Message);
                App.VSDLog.Info("\n ILoginManager.LoginOnline stack: "+ex.StackTrace);
                AppProperties.IsException = true;
               
                //WPFMessageBoxResult _Result = WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
                AppProperties.exceptionFromServiceCall = ex.Message;              
                return false;
            }
        }

        #endregion

        private LoginManager()
        {
        }
        public static LoginManager GetInstance()
        {
            if (_loginManager == null)
            {
                _loginManager = new LoginManager();
            }
            return _loginManager;
        }
    }
}
