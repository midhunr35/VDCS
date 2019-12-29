using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using VSDApp.com.rta.vsd.hh.db;
using VSDApp.com.rta.vsd.hh.utilities;

namespace VSDApp.com.rta.vsd.hh.manager
{
    class ViolationManager : IViolation
    {
        private static ViolationManager _violationManager;
        private ViolationManager() { }
        public static ViolationManager GetInstance()
        {
            if (_violationManager == null)
            {
                _violationManager = new ViolationManager();
            }
            return _violationManager;
        }

        #region IViolation Members

        string IViolation.CalculateSeverity(string previousSeverity, string newSeverity, string locale)
        {
            return ((IDBManager)DBBusinessRuleManager.GetInstance()).CalculateSeverity(previousSeverity, newSeverity, locale);
        }

        string[] IViolation.GetConfigurationDataForSeverity(string severity, int defectsLength)
        {
            return ((IDBManager)DBBusinessRuleManager.GetInstance()).GetConfigurationDataForDueDays(severity, defectsLength);
        }
        bool IViolation.StoreOfflineViolation()
        {
            return ((IDBManager)DBOfflineDataManager.GetInstance()).StoreOfflineData();
        }
        string[] IViolation.GetLocation(string name, string type)
        {
            IDBDataLoad iDBDataLoad = (IDBDataLoad)DBDataLoadManager.GetInstance();

            if (null == type)
            {
                return iDBDataLoad.GetCountries();
            }

            if (type.Equals("area"))
            {
                return iDBDataLoad.GetAreas(name);
            }
            else if (type.Equals("location"))
            {
                return iDBDataLoad.GetLocation(name);
            }
            else
            {
                return iDBDataLoad.GetCities(name);
            }


        }

        string[] IViolation.GetDefects(string defect, string category, string defecttype)
        {
            IDBDataLoad iDataLoad = (IDBDataLoad)DBDataLoadManager.GetInstance();
            return iDataLoad.GetDefects(defect, category, defecttype);
        }
        

        string[] IViolation.GetDefectSeverity(string defectName, string defectSubCategory, string defectMainCat, string value)
        {
            IDBManager iDBManager = (IDBManager)DBBusinessRuleManager.GetInstance();
            return iDBManager.GetDefectSeverityBusinessRules(defectName, defectSubCategory, defectMainCat, value);
        }
       

        public void ClearInspections()
        {
            try
            {


                SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
                string sqlQuery;
                SqlCeCommand command;

                if (con.State == ConnectionState.Closed) { con.Open(); }
                sqlQuery = "Delete from VSD_Owner_Info";
                command = new SqlCeCommand(sqlQuery, con);
                int rowCount = command.ExecuteNonQuery();


                if (con.State == ConnectionState.Closed) { con.Open(); }
                sqlQuery = "Delete from VSD_Driver_Info";
                command = new SqlCeCommand(sqlQuery, con);
                rowCount = command.ExecuteNonQuery();


                if (con.State == ConnectionState.Closed) { con.Open(); }
                sqlQuery = "Delete from VSD_Channel_Defect";
                command = new SqlCeCommand(sqlQuery, con);
                rowCount = command.ExecuteNonQuery();

                if (con.State == ConnectionState.Closed) { con.Open(); }
                sqlQuery = "Delete from VSD_Inspection";
                command = new SqlCeCommand(sqlQuery, con);
                rowCount = command.ExecuteNonQuery();


                if (con.State == ConnectionState.Closed) { con.Open(); }
                sqlQuery = "Delete from VSD_Violation";
                command = new SqlCeCommand(sqlQuery, con);
                rowCount = command.ExecuteNonQuery();


                if (con.State == ConnectionState.Closed) { con.Open(); }
                sqlQuery = "Delete from VSD_Vehicle_Info";
                command = new SqlCeCommand(sqlQuery, con);
                rowCount = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);

            }

        }

        bool IViolation.SubmitViolation()
        {
            IDBDataLoad iDataLoad = ((IDBDataLoad)DBDataLoadManager.GetInstance());

            try
            {
                App.VSDLog.Info("\n----- ViolationManager.SubmitViolation()------- ");
                handHeldService.SubmitViolationResponseItem respItem = new VSDApp.handHeldService.SubmitViolationResponseItem();
                handHeldService.Inspection inspection = new VSDApp.handHeldService.Inspection();

                handHeldService.AuthHeader header = new VSDApp.handHeldService.AuthHeader();


                inspection.time = System.DateTime.Now;
                inspection.timeSpecified = true;
                App.VSDLog.Info("\n  inspection.time =" + inspection.time.ToString());
                App.VSDLog.Info("\n  inspection.timeSpecified =" + inspection.timeSpecified.ToString());
                AppProperties.recordedViolation.ViolationIssueDate = inspection.time;
                string adr = iDataLoad.GetLocationCode(AppProperties.recordedViolation.InspectionArea.city, AppProperties.recordedViolation.InspectionArea.area, AppProperties.recordedViolation.InspectionArea.location);
                
                //Dubai,Deira,Dafza
                if (adr == "")
                {
                    //   string adr = iDataLoad.GetLocationCode(AppProperties.de, AppProperties.recordedViolation.InspectionArea.area, AppProperties.recordedViolation.InspectionArea.location);
                }
                inspection.address = (adr != null) ? adr : "247.00012";

                App.VSDLog.Info("\n address =" + inspection.address);

                // inspection.channelPartInspectionCode = "01";

                Regex regExp = new Regex("[A-Za-z0-9]*");
                bool isEng = regExp.IsMatch(AppProperties.empUserName);


                if (isEng)
                {
                    header.userName = AppProperties.empUserName;
                    header.password = AppProperties.empPassword;
                    inspection.inspectorName = AppProperties.empUserName;
                    
                }
                else
                {
                    header.userName = AppProperties.empUserName;
                    header.password = AppProperties.empPassword;
                    inspection.inspectorNameArabic = AppProperties.empUserName;
                   
                }


                string serialNumber = "39";// Schweers.Sys.Device.SerialNumber;
                //empUserName code
                inspection.inspectorEmployeeId = AppProperties.empID;
                inspection.channelPartInspectionCode = AppProperties.deviceCode;

                App.VSDLog.Info("\n inspection.inspectorEmployeeId=" + inspection.inspectorEmployeeId );
                App.VSDLog.Info("\n inspection.channelPartInspectionCode=" + inspection.channelPartInspectionCode);
                //  isEng = regExp.IsMatch((AppProperties.recordedViolation.PlateCondition != null) ? AppProperties.recordedViolation.PlateCondition : (Resources.GetInstance().GetLocale().Equals(Resources.locale_EN)) ? "ENG" : "استعمال");
                isEng = true;

                /*
                if (AppProperties.Selected_Resource == "English")
                {
                    inspection.frontPlateConditionName = (AppProperties.recordedViolation.PlateCondition != null) ? AppProperties.recordedViolation.PlateCondition : "Good Condition";
                    inspection.rearPlateConditionName = (AppProperties.recordedViolation.PlateCondition != null) ? AppProperties.recordedViolation.PlateCondition : "Good Condition";
                }
                else
                {
                    inspection.frontPlateConditionName = (AppProperties.recordedViolation.PlateCondition != null) ? AppProperties.recordedViolation.PlateCondition : new CommonUtils().GetStringValue("Good Condition");
                    inspection.rearPlateConditionName = (AppProperties.recordedViolation.PlateCondition != null) ? AppProperties.recordedViolation.PlateCondition : new CommonUtils().GetStringValue("Good Condition");
                }
                */

                handHeldService.Vehicle veh = new VSDApp.handHeldService.Vehicle();

                handHeldService.VehiclePlate plate = new VSDApp.handHeldService.VehiclePlate();

                plate.source = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetPlateSourceCode((AppProperties.vehicle.Emirate == "") ? AppProperties.vehicle.Country : AppProperties.vehicle.Emirate).Trim();
                if (plate.source == null || plate.source == "")
                {
                    plate.source = AppProperties.vehicle.PlateSource;
                }
                plate.number = AppProperties.vehicle.PlateNumber.ToString();
                plate.category = (AppProperties.vehicle.PlateCategory != "") ? AppProperties.vehicle.PlateCategory.Trim() : "NA";
                plate.code = (AppProperties.vehicle.PlateCode != "") ? AppProperties.vehicle.PlateCode : "NA";
                if (AppProperties.vehicle.Country == "")
                {
                    veh.country = AppProperties.defaultCountry;
                }
                else
                {
                    veh.country = AppProperties.vehicle.Country.Trim();
                }
                veh.plateDetails = plate;
                veh.chassisNumber = AppProperties.vehicle.ChassisNumber;
                App.VSDLog.Info(@"\n plate.source=" + plate.source 
                    + ", \n plate.number=" + plate.number
                    + ", \n plate.category=" + plate.category
                    + ", \n plate.code=" + plate.code
                    + ", \n veh.country=" + veh.country);
                handHeldService.VehicleOwner owner = new VSDApp.handHeldService.VehicleOwner();
                if (AppProperties.vehicle.Operator != null)
                    owner.ownerName = AppProperties.vehicle.Operator.OperatorName;
                else
                    owner.ownerName = "";
                owner.ownerNameArabic = AppProperties.vehicle.Operator.OperatorNameAr;
                owner.trafficFileNumber = AppProperties.vehicle.Operator.TrafficFileNumber;
                veh.owner = owner;
                handHeldService.VehicleCategory vehCat = new VSDApp.handHeldService.VehicleCategory();
                //vehCat.code = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetVehicleCategoryCode((AppProperties.vehicle.VehicleCategory != null) ? AppProperties.vehicle.VehicleCategory : AppProperties.vehicle.VehicleCategoryAr).Trim();
                App.VSDLog.Info("SubCategoryAr :" + AppProperties.vehicle.SubCategoryAr
                    + "SubCategoryAr :" + AppProperties.vehicle.SubCategoryAr
                    + "AppProperties.vehicle.VehicleCategoryAr : " + AppProperties.vehicle.VehicleCategoryAr
                    + "AppProperties.vehicle.VehicleCategory :" + AppProperties.vehicle.VehicleCategory);
                string subcat=(AppProperties.vehicle.SubCategoryAr != null) ? AppProperties.vehicle.SubCategoryAr : AppProperties.vehicle.SubCategory;
                App.VSDLog.Info("Sub -" + subcat);
                vehCat.code = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetVehicleSubCategoryCode(subcat, (AppProperties.vehicle.VehicleCategoryAr != null) ? AppProperties.vehicle.VehicleCategoryAr : AppProperties.vehicle.VehicleCategory).Trim();
                if (string.IsNullOrEmpty(vehCat.code))
                {
                    //vehCat.code = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetVehicleCategoryCode((AppProperties.vehicle.VehicleCategory != null) ? AppProperties.vehicle.VehicleCategory : AppProperties.vehicle.VehicleCategoryAr).Trim();
                    vehCat.code = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetVehicleCategoryCode((AppProperties.vehicle.SubCategoryAr != null) ? AppProperties.vehicle.SubCategoryAr : AppProperties.vehicle.SubCategory).Trim();
                }
                App.VSDLog.Info("vehCat.code :" + vehCat.code);
                veh.category = vehCat;
                VSDApp.handHeldService.VehicleManufacturer manu = new VSDApp.handHeldService.VehicleManufacturer();
                manu.name = AppProperties.vehicle.Make;
                manu.model = AppProperties.vehicle.Model;
                veh.manufacturedYear = AppProperties.vehicle.Year;
                veh.manufacturer = manu;


                App.VSDLog.Info(@"\n owner.ownerName=" + owner.ownerName
                    + ", \n owner.ownerNameArabic=" + owner.ownerNameArabic
                    + ", \n owner.trafficFileNumber=" + owner.trafficFileNumber
                   
                    + ", \n vehCat.code=" + vehCat.code
                    + ", \n  veh.category=" + veh.category
                    + ", \n manu.name=" + manu.name
                    + ", \n manu.model=" + manu.model
                    + ", \n veh.manufacturedYear=" + veh.manufacturedYear
                    
                    );
                //Mileage
                if (AppProperties.vehicle.Mileage != null)
                {
                    veh.mileage = (AppProperties.vehicle.Mileage != "") ? AppProperties.vehicle.Mileage.Trim() : "0";
                }
                else
                {
                    AppProperties.vehicle.Mileage = "0";
                }
                //   (AppProperties.vehicle.Mileage = "") ? AppProperties.vehicle.Mileage.Trim() : "0";


                //create violation here


                if (null != AppProperties.recordedViolation.Defect && AppProperties.recordedViolation.Defect.Length > 0)
                {
                    App.VSDLog.Info("\n ViolationManager.SubmitViolation(): Now adding violations in service object. Total No. of Defect in this Inspection are: " + AppProperties.recordedViolation.Defect.Length);
                    //create violation
                    handHeldService.Violation violation = new VSDApp.handHeldService.Violation();
                    violation.comments = AppProperties.recordedViolation.ViolationComments;
                    violation.commentsArabic = AppProperties.recordedViolation.ViolationCommentsAr;
                    violation.provisionalTicketNumber = "?";
                    App.VSDLog.Info(@"\n violation.comments=" + violation.comments
                   + ", \n violation.commentsArabic=" + violation.commentsArabic                   
                   );
                    handHeldService.Defect[] defect = new VSDApp.handHeldService.Defect[AppProperties.recordedViolation.Defect.Length];

                    for (int i = 0; i < defect.Length; i++)
                    {
                        App.VSDLog.Info("\n------------Defect# " + i.ToString() + "----------------");
                        defect[i] = new VSDApp.handHeldService.Defect();
                        defect[i].comment = AppProperties.recordedViolation.Defect[i].DefectValue.Trim();
                        defect[i].code = iDataLoad.GetDefectCode(AppProperties.recordedViolation.Defect[i].DefectID).Trim();
                        App.VSDLog.Info(@"\n defect["+i.ToString()+"].comments=" + defect[i].comment
                        + ", \n defect[" + i.ToString() + "].code=" + defect[i].code
                        +"-----------------------------------------------------------------------------------"    );
                    }
                    violation.defects = defect;

                    //////////////////////////////////////

                    // Violation Impounding + Fine Info


                    //  violation.fineAmount = float.Parse(AppProperties.vehicle.TotalFineAmmount);
                    // Int32.Parse(AppProperties.vehicle.TotalFineAmmount);
                    //  violation.groundingDays = long.Parse(AppProperties.vehicle.TotalImpoundingDays);


                    violation.fineAmount = AppProperties.vehicle.TotalFineAmmount;

                    violation.groundingDays = AppProperties.vehicle.TotalImpoundingDays;

                    

                    violation.groundingGracePeriod = AppProperties.vehicle.IsImpoundingGracePeriod;
                    // violation.fineAmountString = "69";


                    inspection.violation = violation;

                    //  inspection.violation.fineAmount = Convert.ToSingle("105");
                    //   Single.Parse("105");

                    // inspection.violation.groundingDays = Int64.Parse("33");



                    /////////////////////////////////////

                    if (AppProperties.Is_DeviceInspection)
                    {
                        App.VSDLog.Info(@"\Device Inspection Option");
                        inspection.inspectionType = "Device Inspection";
                        violation.fineAmount = "0";

                        violation.groundingDays = "0";
                        violation.groundingGracePeriod = "T";
                        veh.mileage = "0";
                       

                    }
                    else
                    {

                        inspection.inspectionType = "Roadside";
                      //  handHeldService.VehicleDriver driver = new VSDApp.handHeldService.VehicleDriver();
                        //driver.name = ("" != AppProperties.vehicle.DriverName) ? AppProperties.vehicle.DriverName : "NA";
                       // driver.nameArabic = ("" != AppProperties.vehicle.DriverNameAr) ? AppProperties.vehicle.DriverNameAr : "NA";
                       // driver.licenseNumber = ("" != AppProperties.vehicle.DriverLicense) ? AppProperties.vehicle.DriverLicense : "NA";
                       // veh.driver = driver;
                        //To Do
                        //inspection.isPlateConfiscated = (AppProperties.recordedViolation.IsConfiscated) ? "T" : "F";
                        inspection.isPlateConfiscated = "F";
                        inspection.isRegistrationExpired = "F";

                  //      App.VSDLog.Info(@"\n driver.name =" + driver.name
                    //       + ", \n driver.nameArabic=" + driver.nameArabic
                      //     + ", \n driver.licenseNumber=" + driver.licenseNumber
                        //   );
                    }
                }
                else
                {

                    App.VSDLog.Info("\n ViolationManager.SubmitViolation(): inspection doesn't contain any violation ");
                    // inspection.isPlateConfiscated = (AppProperties.recordedViolation.IsConfiscated) ? "T" : "F";
                    inspection.isPlateConfiscated = "F";
                    inspection.isRegistrationExpired = "F";

                }
				 inspection.isPlateConfiscated = "F";
                inspection.isRegistrationExpired = "F";
                handHeldService.VehicleDriver driver = new VSDApp.handHeldService.VehicleDriver();
                driver.name = ("" != AppProperties.vehicle.DriverName) ? AppProperties.vehicle.DriverName : "NA";
                driver.nameArabic = ("" != AppProperties.vehicle.DriverNameAr) ? AppProperties.vehicle.DriverNameAr : "NA";
                driver.licenseNumber = ("" != AppProperties.vehicle.DriverLicense) ? AppProperties.vehicle.DriverLicense : "NA";
                veh.driver = driver;
                App.VSDLog.Info(@"\n driver.name =" + driver.name
                     + ", \n driver.nameArabic=" + driver.nameArabic
                     + ", \n driver.licenseNumber=" + driver.licenseNumber
                     );
                /*Added by kashif abbasi on dated 30-Sep-2015 for online voilation submission*/
                 veh.isHazard = AppProperties.vehicle.IsHazard.ToString();
				App.VSDLog.Info(@"\n veh.isHazard=" + veh.isHazard);
                inspection.vehicle = veh;

                //inspection.time = AppProperties.recordedViolation.ViolationIssueDate;

                handHeldService.HandHeldService hh = new VSDApp.handHeldService.HandHeldService();
                hh.authHeader = header;
                hh.Timeout = 180000;
                AppProperties.IsServiceResponseNull = false;
                AppProperties.IsException = false;
                AppProperties.NotFoundError = false;
                
                respItem = hh.submitViolation("RC123", inspection);


                App.VSDLog.Info("\n ViolationManager.SubmitViolation(): Called hh.submitViolation service send response code=" + respItem.response.code);
                if (respItem.response != null && respItem.response.code != null)
                {
                    if (respItem.response.code.Equals("1000"))
                    {
                        //create violation here


                        if (null != AppProperties.recordedViolation.Defect && AppProperties.recordedViolation.Defect.Length > 0)
                        {
                            App.VSDLog.Info(" Defect length greater than 0 =: " + AppProperties.recordedViolation.Defect.Length);
                            //System.Windows.Forms.MessageBox.Show("Violation Uploaded");
                            handHeldService.Inspection savedInspection = new VSDApp.handHeldService.Inspection();
                            savedInspection = respItem.savedInspection;
                            //if (savedInspection.violation.reportedDate != null)
                           // {
                             //   AppProperties.recordedViolation.ViolationIssueDate = savedInspection.violation.reportedDate;
                           // }
                          // AppProperties.recordedViolation.ViolationIssueDate.ToString("dd/MM/yy hh:mm");
                            AppProperties.recordedViolation.ViolationDueDays = (savedInspection.violation.dueDateSpecified) ? savedInspection.violation.dueDate : AppProperties.recordedViolation.ViolationDueDays;
                            App.VSDLog.Info("\nAppProperties.recordedViolation.ViolationDueDays" + AppProperties.recordedViolation.ViolationDueDays);
                            AppProperties.recordedViolation.ViolationSeverity = savedInspection.violation.severityLevel.value;
                            App.VSDLog.Info("\nAppProperties.recordedViolation.ViolationSeverity" + AppProperties.recordedViolation.ViolationSeverity);
                            AppProperties.recordedViolation.ViolationTicketCode = savedInspection.violation.ticketNumber;
                            App.VSDLog.Info("\nAppProperties.recordedViolation.ViolationTicketCode" + AppProperties.recordedViolation.ViolationTicketCode);
                            AppProperties.recordedViolation.ViolationTestType = savedInspection.violation.testTypeName;
                            App.VSDLog.Info("\nAppProperties.recordedViolation.ViolationTestType" + AppProperties.recordedViolation.ViolationTestType);
                            AppProperties.vehicle.VehicleSuspensionDate = (savedInspection.violation.vehicleSuspensionDateSpecified) ? savedInspection.violation.vehicleSuspensionDate : AppProperties.vehicle.VehicleSuspensionDate;
                            App.VSDLog.Info("\nAppProperties.vehicle.VehicleSuspensionDate" + AppProperties.vehicle.VehicleSuspensionDate);
                            AppProperties.vehicle.Operator.CompanySuspensionDate = (savedInspection.violation.ownerSuspensionDateSpecified) ? savedInspection.violation.ownerSuspensionDate : AppProperties.vehicle.Operator.CompanySuspensionDate;
                            App.VSDLog.Info("\nAppProperties.vehicle.Operator.CompanySuspensionDate" + AppProperties.vehicle.Operator.CompanySuspensionDate);

                            return true;

                        }
                        else
                        {
                            AppProperties.isInspectionUploaded = true;
                            /*
                            if (AppProperties.Selected_Resource == "English")
                                System.Windows.MessageBox.Show("Inspection Uploaded")
                            else
                                System.Windows.MessageBox.Show("تم تسجيل عملية التفتيش بنجاح");*/
                            AppProperties.recordedViolation.ViolationTicketCode = respItem.response.message;
                            return true;
                        }

                    }
                    else if (respItem.response.code.Equals("2000"))
                    {
                        //------added by Kashif abbasi on dated 22-Nov-2015 ----------------
                        CommonUtils.deleteImgDirectory("");//delete the defect image diractory because data is not saved

                        App.VSDLog.Info("un successfull responce code: " + respItem.response.code);
                        AppProperties.businessError = true;
                        if ((respItem.response != null) && (respItem.response.message != null) && (respItem.response.message != ""))
                        {
                            //AppProperties.errorMessageFromBusiness = "Service Responce Code: " + respItem.response.code + "\n Responce Message: " + respItem.response.message;
                            AppProperties.errorMessageFromBusiness = respItem.response.message;
                        }
                        else
                        {
                            AppProperties.errorMessageFromBusiness = "un successfull responce code:2000";
                        }
                        return true;
                    }
                    else
                    {
                        
                        CommonUtils.deleteImgDirectory("");
                        //-------------------------------------------------------------------
                        App.VSDLog.Info("\n ViolationManager.SubmitViolation(): service send unknown response code. response message is:" + respItem.response.message);
                        AppProperties.NotFoundError = true;
                        if ((respItem.response != null) && (respItem.response.message != null) && (respItem.response.message != ""))
                        {
                            AppProperties.errorMessageFromBusiness = "Service Responce Code: " + respItem.response.code + "\n Responce Message: " + respItem.response.message;
                        }
                        else
                        {
                            AppProperties.errorMessageFromBusiness = "WebSerive Response Message Description: NULL";
                        }
                        AppProperties.vehicle = null;
                        return false;
                    }
                }
                else
                {
                    App.VSDLog.Info("\n ViolationManager.SubmitViolation(): Service Response is NULL");
                    AppProperties.IsServiceResponseNull = true;
                    AppProperties.errorMessageFromBusiness = "WebSerive Response: NULL";
                    return false;
                }


            }
            catch (Exception e)
            {

                App.VSDLog.Info("\n ViolationManager.SubmitViolation(): come in catch block now going to save offline data");
                AppProperties.IsException = true;
                if (e.InnerException != null && AppProperties.errorMessageFromBusiness != null)
                {
                    AppProperties.errorMessageFromBusiness = e.InnerException.Message;
                }
                else
                {      
                                       
                   AppProperties.errorMessageFromBusiness = "Exception.InnerException = NULL  "+ e.StackTrace;
                    
                }
                return ((IDBManager)DBOfflineDataManager.GetInstance()).StoreOfflineData();

            }


        }
        bool IViolation.SubmitOfflineViolation()
        {
            try
            {
                App.VSDLog.Info("\n entered in ViolationManager.SubmitOfflineViolation()");
                List<Object> inspections;
                IDBDataLoad iDataLoad = ((IDBDataLoad)DBDataLoadManager.GetInstance());
                inspections = iDataLoad.GetStoredInspection();

                if (inspections.Count > 0)
                {
                    handHeldService.HandHeldService service = new VSDApp.handHeldService.HandHeldService();
                    handHeldService.SubmitOfflineViolationResponseItem[] resItem;
                    handHeldService.Inspection[] storedInspection = new VSDApp.handHeldService.Inspection[inspections.Count];



                    int inspectionID = -1;
                    int vehInfoID = -1;
                    int vioID = -1;

                    for (int i = 0; i < inspections.Count; i++)
                    {
                        storedInspection[i] = new VSDApp.handHeldService.Inspection();
                        string[] inspection = (string[])inspections[i];
                        inspectionID = Int32.Parse(inspection[0]);
                        vehInfoID = Int32.Parse(inspection[1]);
                        vioID = Int32.Parse(inspection[2]);
                        int locID = Int32.Parse(inspection[3]);

                        string locationExternalCode = iDataLoad.GetLocationCode(locID);

                        string[] vehicleInfo = iDataLoad.GetVehicleInfo(vehInfoID);
                        int vehicleCategoryID = Int32.Parse(vehicleInfo[1]);

                        String vehCatCode = iDataLoad.GetVehicleCategoryCode(vehicleCategoryID);

                        string[] ownerInfo = iDataLoad.GetOwnerInfo(vehInfoID);
                        string[] driverInfo = iDataLoad.GetDriverInfo(vehInfoID);
                        string[] violation = iDataLoad.GetViolation(vioID);
                        string[][] defectCodes = iDataLoad.GetDefectCodesForViolation(vioID);


                        storedInspection[i].time = DateTime.Parse(inspection[11].Trim());
                        storedInspection[i].address = locationExternalCode;


                        if (vioID != -1)
                        {
                            VSDApp.handHeldService.Defect[] defects = new VSDApp.handHeldService.Defect[defectCodes.Length];
                            for (int j = 0; j < defectCodes.Length; j++)
                            {
                                defects[j] = new VSDApp.handHeldService.Defect();
                                defects[j].code = defectCodes[j][0].Trim();
                                defects[j].comment = defectCodes[j][1].Trim();

                            }

                            handHeldService.Violation storedViolation = new VSDApp.handHeldService.Violation();
                            storedViolation.defects = defects;
                            storedViolation.comments = (violation[2] != "NULL") ? violation[2].Trim() : violation[3].Trim();
                            storedViolation.provisionalTicketNumber = violation[0].Trim();
                            storedInspection[i].violation = storedViolation;
                        }


                        storedInspection[i].channelPartInspectionCode = AppProperties.deviceCode;
                        storedInspection[i].inspectorName = (inspection[10].Trim());
                        storedInspection[i].inspectorEmployeeId = AppProperties.empID;
                        storedInspection[i].time = DateTime.Parse((inspection[11].Trim()));
                        storedInspection[i].timeSpecified = true;
                        storedInspection[i].frontPlateConditionName = (inspection[4].Trim() != "NULL") ? inspection[4].Trim() : "Good Condition";
                        storedInspection[i].rearPlateConditionName = (inspection[4].Trim() != "NULL") ? inspection[4].Trim() : "Good Condition";
                        storedInspection[i].isPlateConfiscated = inspection[6].Trim();
                        storedInspection[i].isRegistrationExpired = "F";


                        handHeldService.Vehicle veh = new VSDApp.handHeldService.Vehicle();
                        handHeldService.VehiclePlate plate = new VSDApp.handHeldService.VehiclePlate();

                        plate.source = iDataLoad.GetPlateSourceCode((vehicleInfo[4].Trim() != "") ? vehicleInfo[4].Trim() : vehicleInfo[6].Trim()).Trim();
                        plate.number = vehicleInfo[5].Trim();
                        plate.category = ("" != vehicleInfo[2].Trim()) ? vehicleInfo[2].Trim() : "NA";
                        plate.code = ("" != vehicleInfo[3].Trim()) ? vehicleInfo[3].Trim() : "NA";
                        veh.country = vehicleInfo[6].Trim();
                        veh.plateDetails = plate;
                        /*added by kashif abbasi on dated 30-sep-2015 for synch(offline voilations submitions) service*/
                        // veh.isHazard = vehicleInfo[14].Trim();

                        handHeldService.VehicleCategory vehCat = new VSDApp.handHeldService.VehicleCategory();
                        vehCat.code = vehCatCode.Trim();
                        veh.category = vehCat;
                        veh.mileage = "0";
                        App.VSDLog.Info("vehCat.code :" + vehCat.code);
                        

                        handHeldService.VehicleDriver driver = new VSDApp.handHeldService.VehicleDriver();
                        if (vioID != -1)
                        {
                            driver.licenseNumber = ("" != driverInfo[4].Trim()) ? driverInfo[4].Trim() : "NA";
                            driver.name = ("" != driverInfo[2].Trim()) ? driverInfo[2].Trim() : "NA";
                        }
                        else
                        {
                            driver.licenseNumber = "NA";
                            driver.name = "NA";
                        }

                        veh.driver = driver;
                        storedInspection[i].vehicle = veh;

                    }

                    handHeldService.AuthHeader header = new VSDApp.handHeldService.AuthHeader();
                    header.userName = AppProperties.empUserName;
                    header.password = AppProperties.empPassword;
                    service.authHeader = header;
                    service.Timeout = 180000;
                    resItem = service.submitOfflineViolation("RC123", storedInspection);


                    for (int j = 0; j < resItem.Length; j++)
                    {
                        App.VSDLog.Info(@"\n ViolationManager.SubmitOfflineViolation(): called service.submitOfflineViolation('RC123', storedInspection), 
                                            it returned code: " + resItem[j].response.code + "\n response Message is:" + resItem[j].response.message);
                        if (resItem[j].response.code.Equals("1000"))
                        {
                            
                            //System.Windows.MessageBox.Show(Resources.GetInstance().GetString("Offline Data Sysnchronized"));
                            AppProperties.errorMessageFromBusiness = "";
                            if (AppProperties.Selected_Resource == "English")
                            {
                                //  System.Windows.MessageBox.Show("Offline Data Sysnchronized");
                                AppProperties.errorMessageFromBusiness = "Offline Data Sysnchronized";
                            }
                            else
                            {
                                // System.Windows.MessageBox.Show("تم مزامنة البيانات");
                                AppProperties.errorMessageFromBusiness = "تم مزامنة البيانات";
                            }
                            ClearInspections();
                            return true;
                        }
                        else if (resItem[j].response.code.Equals("2000"))
                        {
                            App.VSDLog.Info("SubmitOfflineViolation()-- Service Response Code is 2000");
                            AppProperties.businessError = true;
                            AppProperties.errorMessageFromBusiness = resItem[j].response.message;
                            return true;

                        }
                        else
                        {
                            //   System.Windows.MessageBox.Show("Unable to upload data " + resItem[j].response.message);
                            AppProperties.errorMessageFromBusiness = resItem[j].response.message;
                            App.VSDLog.Info("SubmitOfflineViolation()-- Service Response other then success");
                            break;
                        }

                    }


                }
                else
                {
                    if (AppProperties.Selected_Resource == "English")
                    {
                        // System.Windows.MessageBox.Show("No offline data available");
                        AppProperties.errorMessageFromBusiness = "No offline data available";
                    }
                    else
                    {
                        // System.Windows.MessageBox.Show("لا توجد بيانات للمزامنة");
                        AppProperties.errorMessageFromBusiness = "لا توجد بيانات للمزامنة";
                    }

                }


                return true;
            }
            catch (Exception e)
            {
                App.VSDLog.Info("SubmitOfflineViolation()-- Exception");
                App.VSDLog.Info(e.StackTrace);
                if (e.InnerException != null && AppProperties.errorMessageFromBusiness != null)
                {
                    AppProperties.errorMessageFromBusiness = e.InnerException.Message;
                }
                else
                {

                    AppProperties.errorMessageFromBusiness = "Exception.InnerException = NULL  " + e.StackTrace;

                }
                //  System.Windows.MessageBox.Show("Unable to connect: " + e.Message);
            //    AppProperties.errorMessageFromBusiness = "Unable to connect: " + e.Message;
                App.VSDLog.Info(AppProperties.errorMessageFromBusiness);
                return false;
            }
        }

        #endregion
    }
}
