using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using VSDApp.com.rta.vsd.hh.data;
using VSDApp.com.rta.vsd.hh.utilities;

namespace VSDApp.com.rta.vsd.hh.manager
{
    class GPSLocationManager : IGPSLocation
    {

        private static GPSLocationManager _GPSLocationManager;

        private GPSLocationManager() { }
        public static GPSLocationManager GetInstance()
        {
            if (_GPSLocationManager == null)
            {
                _GPSLocationManager = new GPSLocationManager();
            }
            return _GPSLocationManager;
        }

        #region IGPSLocation Members
        /// <summary>
        /// Submit Gps Lat Long to the service Async
        /// </summary>
        /// <returns></returns>
        public bool SubmitHandHeldGeoLocation(HandHeldGPSLocation hhLocation)
        {
            try
            {
                CommonUtils.WriteLocationLog("\nEntered into SubmitHHLocation Manager Function");
                handHeldService.SubmitHandheldLocationResponseItem respItem = new VSDApp.handHeldService.SubmitHandheldLocationResponseItem();
                
                handHeldService.HandheldLocationInformation _locationInfo = new handHeldService.HandheldLocationInformation();
                handHeldService.AuthHeader header = new VSDApp.handHeldService.AuthHeader();

                Regex regExp = new Regex("[A-Za-z0-9]*");
                bool isEng = regExp.IsMatch(AppProperties.empUserName);


                if (isEng)
                {
                    header.userName = AppProperties.empUserName;
                    header.password = AppProperties.empPassword;
                    _locationInfo.firstName = AppProperties.empUserName;
                    if (AppProperties.empID != null)
                        _locationInfo.rtaEmployeeNumber = AppProperties.empID;
                    else
                        _locationInfo.rtaEmployeeNumber = "123456";

                }
                else
                {
                    header.userName = AppProperties.empUserName;
                    header.password = AppProperties.empPassword;
                    _locationInfo.firstNameArabic = AppProperties.empUserName;
                    if (AppProperties.empID != null)
                        _locationInfo.rtaEmployeeNumber = AppProperties.empID;
                    else
                        _locationInfo.rtaEmployeeNumber = "123456";
                }


                if (hhLocation != null)
                {

                    _locationInfo.handheldDeviceLocationLatitude = hhLocation.Latitude;
                    _locationInfo.handheldDeviceLocationLongitude = hhLocation.Longitude;
                   
                }
                else
                {
                    _locationInfo.handheldDeviceLocationLatitude = "0";
                    _locationInfo.handheldDeviceLocationLongitude = "0";
                }

                CommonUtils.WriteLocationLog("\n Device Current Latitude : " + _locationInfo.handheldDeviceLocationLatitude + "\n Device Current Longitude :" + _locationInfo.handheldDeviceLocationLongitude);
         
               handHeldService.HandHeldService hh = new VSDApp.handHeldService.HandHeldService();
               hh.authHeader = header;
               AppProperties.Is_HHLocationSubmition = false;
               respItem= hh.submitHandheldLocation("RC123", _locationInfo);
               AppProperties.Is_HHLocationSubmition = true;
               if (respItem.response.code.Equals("1000"))
              // if(true)
                {
                   CommonUtils.WriteLocationLog("HandHled Location Synched Successfully with DB");
               }
               else
               {
                   if ((respItem.response != null) && (respItem.response.message != null) && (respItem.response.message != ""))
                   {
                       CommonUtils.WriteLocationLog("Problem While Calling HH.submitHandheldLocation(). \n Error:"+ respItem.response.message);
                   }
                   else
                   {
                      CommonUtils.WriteLocationLog("Problem While Calling HH.submitHandheldLocation");
                   }
               }
               CommonUtils.WriteLocationLog("SubmitHHLocation Manager Function----- END------ ");
                return true;
               
            }
            catch (Exception ex)
            {
                AppProperties.Is_HHLocationSubmition = true;
               CommonUtils.WriteLocationLog("Exception in SubmitHHLocation Manager Function..."+ ex.Message);
               return false;
            }
        }

        #endregion
    }
}
