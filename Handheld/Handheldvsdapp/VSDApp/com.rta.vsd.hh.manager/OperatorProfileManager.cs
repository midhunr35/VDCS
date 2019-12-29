using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VSDApp.com.rta.vsd.hh.data;
using VSDApp.com.rta.vsd.hh.db;
using VSDApp.com.rta.vsd.hh.utilities;

namespace VSDApp.com.rta.vsd.hh.manager
{
    class OperatorProfileManager : IOperatorProfile
    {
        private static OperatorProfileManager _operatorProfileManager;
        private OperatorProfileManager() { }
        public static OperatorProfileManager GetInstance()
        {
            if (_operatorProfileManager == null)
            {
                _operatorProfileManager = new OperatorProfileManager();
            }
            return _operatorProfileManager;
        }





        #region IOperatorProfile Members

        Operator IOperatorProfile.GetOperator(string operatorName, string trafficFileNumber)
        {
            Operator operatorFormWeb = new Operator();
            try
            {
                App.VSDLog.Info("\n IOperatorProfile.GetOperator: --------------------START---------------------- ");
                handHeldService.HandHeldService service = new VSDApp.handHeldService.HandHeldService();
                handHeldService.AuthHeader authorize = new VSDApp.handHeldService.AuthHeader();
                authorize.password = AppProperties.empPassword;
                authorize.userName = AppProperties.empUserName;
               
                service.authHeader = authorize;
                handHeldService.InquireCompanyProfileResponseItem responseItem = new VSDApp.handHeldService.InquireCompanyProfileResponseItem();
                operatorFormWeb = new Operator();
                App.VSDLog.Info("\n IOperatorProfile.GetOperator:TrafficeFileNumber:" + trafficFileNumber);
                responseItem = service.inquireCompanyProfile("H-PS-ICP-1", trafficFileNumber, null);
                if (responseItem.response != null && responseItem.response.code != null)
                {
                    if (responseItem.response.code.Equals("1000", StringComparison.CurrentCultureIgnoreCase))
                    {
                        App.VSDLog.Info("\n IOperatorProfile.GetOperator: Service Response Code = 1000");
                        // operatorFormWeb.OperatorOVRRScore = responseItem.company.riskRating.riskRatingName;
                        //OVRR 
                        if (responseItem.company.riskRating != null)
                        {
                            operatorFormWeb.OperatorOVRRScore = (responseItem.company.riskRating.riskRatingName != null) ? responseItem.company.riskRating.riskRatingName.ToString() : "";
                            operatorFormWeb.OperatorOVRRColor = (responseItem.company.riskRating.riskRatingColor != null) ? responseItem.company.riskRating.riskRatingColor.ToString() : "";
                        }
                        else
                        {
                            operatorFormWeb.OperatorOVRRScore = null;
                            operatorFormWeb.OperatorOVRRColor = null;
                        }

                        //OVRR 
                        if (responseItem.company.riskRatingDriver != null)
                        {
                            operatorFormWeb.OperatorODRRScore = (responseItem.company.riskRatingDriver.riskRatingName != null) ? responseItem.company.riskRatingDriver.riskRatingName.ToString() : "";
                            operatorFormWeb.OperatorODRRColor = (responseItem.company.riskRatingDriver.riskRatingColor != null) ? responseItem.company.riskRatingDriver.riskRatingColor.ToString() : "";
                        }
                        else
                        {
                            operatorFormWeb.OperatorODRRScore = null;
                            operatorFormWeb.OperatorODRRColor = null;
                        }




                        operatorFormWeb.OperatorName = responseItem.company.ownerName;
                        operatorFormWeb.OperatorNameAr = responseItem.company.ownerNameArabic;
                        operatorFormWeb.TrafficFileNumber = responseItem.company.trafficFileNumber;

                        Vehicle[] copyResponseVehicles = new Vehicle[responseItem.company.vehicles.Length];
                        int count = 0;
                        foreach (handHeldService.Vehicle responseVehicles in responseItem.company.vehicles)
                        {
                            copyResponseVehicles[count] = new Vehicle();
                            copyResponseVehicles[count].RiskRating = responseVehicles.riskRating.riskRatingName;
                            copyResponseVehicles[count].PlateNumber = responseVehicles.plateDetails.number;
                            copyResponseVehicles[count].PlateCode = responseVehicles.plateDetails.code;
                            copyResponseVehicles[count].PlateCategory = responseVehicles.plateDetails.category;
                            copyResponseVehicles[count].Emirate = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetPlateEmirate(responseVehicles.plateDetails.source);
                            copyResponseVehicles[count].PlateSource = responseVehicles.plateDetails.source;
                            count++;
                        }
                        operatorFormWeb.TopViolatingVehicles = copyResponseVehicles;
                    }
                    else if (responseItem.response.code.Equals("2000"))
                    {
                        App.VSDLog.Info("\n IOperatorProfile.GetOperator: Service Response Code = 2000");
                        AppProperties.businessError = true;
                        AppProperties.errorMessageFromBusiness = responseItem.response.message;

                    }
                    else
                    {
                        App.VSDLog.Info("\n IOperatorProfile.GetOperator: Service Response Code != 1000 and 2000");
                        AppProperties.NotFoundError = true;
                        AppProperties.errorMessageFromBusiness = responseItem.response.message;
                        // System.Windows.Forms.MessageBox.Show(responseItem.response.message);
                        return null;
                    }
                }
                App.VSDLog.Info("\n IOperatorProfile.GetOperator: Service Response is NULL");
                App.VSDLog.Info("\n IOperatorProfile.GetOperator: --------------------END---------------------------");
            }
            catch (Exception ex)
            {
                AppProperties.IsException = true;
                AppProperties.errorMessageFromBusiness = ex.InnerException.Message;
                App.VSDLog.Info(ex.StackTrace);
                return null;
            }
            return operatorFormWeb;
        }

        #endregion
    }
}
