using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSDApp.com.rta.vsd.hh.utilities;

namespace VSDApp.com.rta.vsd.hh.manager
{
    class UserAccountManager : IUserAccountManager
    {
        private static UserAccountManager _userAccountManager;

        public static UserAccountManager GetInstance()
        {
            if (_userAccountManager == null)
            {
                _userAccountManager = new UserAccountManager();
            }
            return _userAccountManager;
        }
        #region IUserAccountManager Members

        public bool UpdateUsreAccounDetials(string picFormat, string picString, string MobileNumber)
        {
            try
            {
                App.VSDLog.Info("\n UpdateUsreAccounDetials: ------------------ START---------------------------");
                App.VSDLog.Info("\n UpdateUsreAccounDetials: Pictue Format"+picFormat);
                App.VSDLog.Info("\n UpdateUsreAccounDetials: Mobile Num" + MobileNumber);

                handHeldService.HandHeldService service = new handHeldService.HandHeldService();
                handHeldService.AuthHeader authorize = new VSDApp.handHeldService.AuthHeader();
                handHeldService.userProfileCredentialResponseType respItem = new handHeldService.userProfileCredentialResponseType();



                authorize.userName = AppProperties.LoggedInUser.UserName;
                authorize.password = AppProperties.LoggedInUser.EmpPassword;
                service.authHeader = authorize;
                respItem =  service.updateUserProfileCredentials("H-PS-UUP-1",AppProperties.LoggedInUser.UserName,MobileNumber,picString,picFormat);

                AppProperties.NotFoundError = false;
                AppProperties.businessError = false;
                AppProperties.IsException = false;
                AppProperties.IsServiceResponseNull = false;
               
                if (respItem.response != null)
                {
                    if (respItem.response.code == "1000")
                    {
                        App.VSDLog.Info("\n UpdateUsreAccounDetials: Service Response : 1000");
                        return true;
                    }
                    else if (respItem.response.code.Equals("2000"))
                    {
                         AppProperties.businessError = true;                        
                        
                         App.VSDLog.Info("un successfull responce code: " + respItem.response.code);
                         AppProperties.businessError = true;
                         if ((respItem.response != null) && (respItem.response.message != null) && (respItem.response.message != ""))
                         {
                             AppProperties.errorMessageFromBusiness = "Service Responce Code: " + respItem.response.code + "\n Responce Message: " + respItem.response.message;
                         }
                         else
                         {
                             AppProperties.errorMessageFromBusiness = "un successfull responce code:2000";
                         }

                         return false;
                        
                    }
                    else
                    {
                        AppProperties.businessError = true;
                        App.VSDLog.Info("\n UpdateUsreAccounDetials: Service Response Unsuccessfull");
                        if ((respItem.response != null) && (respItem.response.message != null) && (respItem.response.message != ""))
                        {
                            AppProperties.errorMessageFromBusiness = "Service Responce Code: " + respItem.response.code + "\n Responce Message: " + respItem.response.message;
                        }
                        else
                        {
                            AppProperties.errorMessageFromBusiness = "WebSerive Response Message Description: NULL";
                        }
                        return false;
                    }
                }
                else
                {
                    AppProperties.IsServiceResponseNull = true;
                    AppProperties.errorMessageFromBusiness = "WebSerive Response: NULL";
                    return false;
                }
             }
            catch (SqlCeException sqlEx)
            {
                App.VSDLog.Info("\n DBAPPLoginManager: SaveUserCredentials--- SQL EX -Exception");
                AppProperties.IsException = true;
                 App.VSDLog.Info(sqlEx.StackTrace);
                 return false;
                // System.Windows.Forms.MessageBox.Show(sqlEx.Message);
            }
        }

        #endregion
    }
}
