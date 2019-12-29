using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSDApp.com.rta.vsd.hh.db;
using VSDApp.com.rta.vsd.hh.utilities;

namespace VSDApp.com.rta.vsd.hh.manager
{
    class DriverDetailManager : IDriverDetailManager
    {
        private static DriverDetailManager _DriverDetailManager;
        public static DriverDetailManager GetInstance()
        {
            if (_DriverDetailManager == null)
            {
                _DriverDetailManager = new DriverDetailManager();
            }
            return _DriverDetailManager;
        }
        private DriverDetailManager()
        {
        }

        #region IDriverDetailManager Members

        bool IDriverDetailManager.InquireDriverDetails(string driverCountry, string driverEmirates, string driverLicNumber)
        {
            try
            {
                App.VSDLog.Info("\n IDriverDetailManager.InquireDriverDetails: --------------------START---------------------- ");

                handHeldService.HandHeldService service = new handHeldService.HandHeldService();
                handHeldService.AuthHeader authorize = new VSDApp.handHeldService.AuthHeader();
                handHeldService.InquireDriverDetailsResponseItem respItem = new handHeldService.InquireDriverDetailsResponseItem();
                handHeldService.VehicleDriver vehDriver = new handHeldService.VehicleDriver();



                authorize.userName = AppProperties.empUserName;
                authorize.password = AppProperties.empPassword;

                service.authHeader = authorize;
                AppProperties.deviceCode = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDeviceCode().Trim();
                App.VSDLog.Info("\n IDriverDetailManager.InquireDriverDetails: Device Code:" + AppProperties.deviceCode);
                driverEmirates = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetPlateSourceCode((driverEmirates == "") ? driverCountry : driverEmirates).Trim();
                App.VSDLog.Info("\n IDriverDetailManager.InquireDriverDetails: Driver Emirates:" + driverEmirates);
                App.VSDLog.Info("\n IDriverDetailManager.InquireDriverDetails: Driver Lic No:" + driverLicNumber);
                respItem = service.inquireDriverDetails("H-PS-IDD-1", driverLicNumber, driverEmirates);

                AppProperties.NotFoundError = false;
                AppProperties.businessError = false;
                AppProperties.IsException = false;


                if (respItem.response != null && respItem.response.code != null)
                {
                    if (respItem.response.code == "1000")
                    {
                        App.VSDLog.Info("\n IDriverDetailManager.InquireDriverDetails: Service Respoinse Code: 1000");
                        AppProperties.vehicle.DriverLicense = (respItem.driver.licenseNumber != null) ? respItem.driver.licenseNumber : "";
                        if (respItem.driver != null)
                        {
                            AppProperties.vehicle.DriverName = (respItem.driver.name != null) ? respItem.driver.name : "";
                            AppProperties.vehicle.DriverNameAr = (respItem.driver.nameArabic != null) ? respItem.driver.nameArabic : "";
                            AppProperties.vehicle.DriverCountry = (respItem.driver.driverNationality != null) ? respItem.driver.driverNationality : "";
                        }
                        if (respItem.driver.driverRiskRating != null)
                        {
                            AppProperties.vehicle.DriverRiskRattingName = (respItem.driver.driverRiskRating.riskRatingName != null) ? respItem.driver.driverRiskRating.riskRatingName : "";
                            AppProperties.vehicle.DriverRiskRattingColor = (respItem.driver.driverRiskRating.riskRatingColor != null) ? respItem.driver.driverRiskRating.riskRatingColor : "";
                            AppProperties.vehicle.DriverRiskRattingScore = (respItem.driver.driverRiskRating.riskRatingScore != null) ? respItem.driver.driverRiskRating.riskRatingScore.ToString() : "";
                        }


                        return true;
                    }
                    else
                    {
                        App.VSDLog.Info("\n IDriverDetailManager.InquireDriverDetails: Service Respoinse Code is not 1000");
                        App.VSDLog.Info("\n IDriverDetailManager.InquireDriverDetails: Service Respoinse Code: " + respItem.response.code);
                        AppProperties.NotFoundError = true;
                        if (respItem.response != null)
                        {
                            AppProperties.errorMessageFromBusiness = respItem.response.message;
                        }
                        return false;
                    }

                }
                else
                {
                    App.VSDLog.Info("\n IDriverDetailManager.InquireDriverDetails: Service Respoinse is NULL");
                }
                return false;
                //  handHeldService.SynchronizeConfigDataResponseItem test = service.synchronizeConfigData("RC123", dateTime, AppProperties.deviceCode, AppProperties.empUserName);

                App.VSDLog.Info("\n IDriverDetailManager.InquireDriverDetails: --------------------END---------------------- ");
            }
            catch (Exception ex)
            {
                App.VSDLog.Info("\n IDriverDetailManager.InquireDriverDetails: Exception in service call");
                AppProperties.IsException = true;
                App.VSDLog.Info(ex.StackTrace);
                if (ex.InnerException != null && ex.InnerException.Message != null)
                {
                    App.VSDLog.Info(ex.InnerException.Message);
                }
                return false;
            }

        }

        #endregion


    }
}
