using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VSDApp.com.rta.vsd.hh.data;
using VSDApp.com.rta.vsd.hh.db;
using VSDApp.com.rta.vsd.hh.utilities;

namespace VSDApp.com.rta.vsd.hh.manager
{
    class VehicleProfileManager : IVehicleProfile
    {
        private static VehicleProfileManager _vehicleProfileManager;
        private VehicleProfileManager() { }
        public static VehicleProfileManager GetInstance()
        {
            if (_vehicleProfileManager == null)
            {
                _vehicleProfileManager = new VehicleProfileManager();
            }
            return _vehicleProfileManager;
        }

        #region IVehicleProfile Members

        Vehicle IVehicleProfile.GetVehicleProfileDetails(string country, string emirate, string plateCategory, string plateNumber, string plateCode)
        {
            try
            {
                App.VSDLog.Info("\n IVehicleProfile.GetVehicleProfileDetails------------START-------------------");
                AppProperties.isComprehensive = false;
                AppProperties.isSafety = true;

                handHeldService.HandHeldService service = new VSDApp.handHeldService.HandHeldService();


                handHeldService.AuthHeader header = new VSDApp.handHeldService.AuthHeader();
                header.password = AppProperties.empPassword;
                header.userName = AppProperties.empUserName;

                service.authHeader = header;


                handHeldService.VehiclePlate vehPlate = new VSDApp.handHeldService.VehiclePlate();
                vehPlate.category = (plateCategory != "") ? plateCategory.Trim() : "NA";
                vehPlate.code = (plateCode != "") ? plateCode.Trim() : "NA";
                vehPlate.number = plateNumber.Trim();
                vehPlate.source = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetPlateSourceCode((emirate == "") ? country : emirate).Trim();

                if (AppProperties.vehicle == null)
                {
                    AppProperties.vehicle = new Vehicle();
                }
                if (AppProperties.vehicle.VehiclDriver == null)
                {
                    AppProperties.vehicle.VehiclDriver = new AuthorizedDriver();
                }
                AppProperties.vehicle.Country = country.Trim();
                AppProperties.vehicle.Emirate = emirate.Trim();
                AppProperties.vehicle.PlateCategory = vehPlate.category.Trim();
                AppProperties.vehicle.PlateNumber = vehPlate.number.Trim();
                AppProperties.vehicle.PlateCode = vehPlate.code.Trim();

                App.VSDLog.Info(@"\n plate.source=" + vehPlate.source
                   + ", \n plate.number=" + vehPlate.number
                   + ", \n plate.category=" + vehPlate.category
                   + ", \n plate.code=" + vehPlate.code
                   + ", \n veh.country=" + AppProperties.vehicle.Country);


                handHeldService.InquireVehicleProfileResponseItem respItem = new VSDApp.handHeldService.InquireVehicleProfileResponseItem();
                service.Timeout = 180000;

                AppProperties.businessError = false;
                AppProperties.IsServiceResponseNull = false;
                AppProperties.IsException = false;
                AppProperties.NotFoundError = false;
                AppProperties.errorMessageFromBusiness = string.Empty;
                //  H-HH-IVP-1  H-PS-IVP-1

                respItem = service.inquireVehicleProfile("H-PS-IVP-1", vehPlate);

                if (respItem.response != null && respItem.response.code != null)
                {
                    if (respItem.response.code.Equals("1000"))
                    {
                        App.VSDLog.Info(@"\n inquireVehicleProfile() Service response is 1000");

                        AppProperties.vehicle.ChassisNumber = respItem.vehicle.chassisNumber;
                        AppProperties.vehicle.Year = respItem.vehicle.manufacturedYear;
                        AppProperties.vehicle.RegExpiry = respItem.vehicle.registrationExpiry;
                        if (AppProperties.vehicle.Operator == null)
                        {
                            AppProperties.vehicle.Operator = new Operator();
                        }
                        if (AppProperties.Selected_Resource == "English")
                        {
                            if (respItem.vehicle.manufacturer.name == null || respItem.vehicle.manufacturer.name == "")
                                AppProperties.vehicle.Make = respItem.vehicle.manufacturer.nameArabic;
                            else
                                AppProperties.vehicle.Make = respItem.vehicle.manufacturer.name;
                            if (respItem.vehicle.manufacturer.model == null || respItem.vehicle.manufacturer.model == "")
                                AppProperties.vehicle.Model = respItem.vehicle.manufacturer.modelArabic;
                            else
                                AppProperties.vehicle.Model = respItem.vehicle.manufacturer.model;
                            if (respItem.vehicle.owner.ownerName == null || respItem.vehicle.owner.ownerName == "")
                                AppProperties.vehicle.Operator.OperatorName = respItem.vehicle.owner.ownerNameArabic;
                            else
                                AppProperties.vehicle.Operator.OperatorName = respItem.vehicle.owner.ownerName;
                        }
                        else
                        {
                            if (respItem.vehicle.manufacturer.name != null || respItem.vehicle.manufacturer.name != "")
                                AppProperties.vehicle.Make = respItem.vehicle.manufacturer.nameArabic;
                            else
                                AppProperties.vehicle.Make = respItem.vehicle.manufacturer.name;
                            if (respItem.vehicle.manufacturer.model != null || respItem.vehicle.manufacturer.model != "")
                                AppProperties.vehicle.Model = respItem.vehicle.manufacturer.model;
                            else
                                AppProperties.vehicle.Model = respItem.vehicle.manufacturer.modelArabic;
                            if (respItem.vehicle.owner.ownerName != null || respItem.vehicle.owner.ownerName != "")
                                AppProperties.vehicle.Operator.OperatorNameAr = respItem.vehicle.owner.ownerNameArabic;
                            else
                                AppProperties.vehicle.Operator.OperatorNameAr = respItem.vehicle.owner.ownerName;
                        }


                        //Vehicle Risk Ratting
                        if (respItem.vehicle.riskRating != null)
                        {
                            AppProperties.vehicle.RiskRating = (respItem.vehicle.riskRating.riskRatingName != null) ? respItem.vehicle.riskRating.riskRatingName.ToString() : "";
                            AppProperties.vehicle.RiskRattingColor = (respItem.vehicle.riskRating.riskRatingColor != null) ? respItem.vehicle.riskRating.riskRatingColor.ToString() : "";
                        }
                        else
                        {
                            AppProperties.vehicle.RiskRating = null;
                            AppProperties.vehicle.RiskRattingColor = null;
                        }
                        AppProperties.vehicle.IsElligibleForPocession = (respItem.vehicle.isEligibleForPossessionCertificate != null) ? respItem.vehicle.isEligibleForPossessionCertificate.ToString() : "";

                        //AppProperties.vehicle.Operator.OperatorNameAr = respItem.vehicle.owner.ownerNameArabic;
                        AppProperties.vehicle.Operator.TrafficFileNumber = respItem.vehicle.owner.trafficFileNumber;
                        AppProperties.vehicle.PlateCategory = plateCategory;
                        AppProperties.vehicle.PlateCode = plateCode;
                        AppProperties.vehicle.PlateNumber = plateNumber;
                        if (respItem.vehicle.category != null)
                        {
                            if (respItem.vehicle.category.code == null)
                            {
                                AppProperties.vehicle.VehicleCategory = "Heavy Vehicle";
                                AppProperties.vehicle.VehicleCategoryAr = "مركبة ثقيلة";

                            }
                            else
                            {
                                AppProperties.vehicle.VehicleCategory = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetVehicleCategoryNameByCode(respItem.vehicle.category.code).Trim();
                                if (AppProperties.vehicle.VehicleCategory != null && AppProperties.vehicle.VehicleCategory != "")
                                {
                                    AppProperties.vehicle.VehicleCategoryAr = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetAlternateVehicleCategory(AppProperties.vehicle.VehicleCategory, "");
                                }
                                else
                                {
                                    AppProperties.vehicle.VehicleCategoryAr = respItem.vehicle.category.nameArabic;
                                }

                            }
                        }
                        else
                        {
                            AppProperties.vehicle.VehicleCategory = "Heavy Vehicle";
                            AppProperties.vehicle.VehicleCategoryAr = "مركبة ثقيلة";
                        }
                        // Device Inspection Changes
                        AppProperties.vehicle.SubCategory = (respItem.vehicle.subCategoryNameArabic != null) ? respItem.vehicle.subCategoryNameArabic.ToString() : "";
                        AppProperties.vehicle.SubCategoryAr = (respItem.vehicle.subCategoryNameArabic != null) ? respItem.vehicle.subCategoryNameArabic.ToString() : "";

                        App.VSDLog.Info("AppProperties.vehicle.VehicleCategoryAr00 : " + AppProperties.vehicle.VehicleCategoryAr + AppProperties.vehicle.VehicleCategory);
                        App.VSDLog.Info(@"\n respItem.vehicle.subCategoryNameArabic " + respItem.vehicle.subCategoryNameArabic); 

                        if (AppProperties.vehicle.DeviceInspectionparm == null)
                            AppProperties.vehicle.DeviceInspectionparm = new VehicleDeviceInspParams();
                            
                        AppProperties.vehicle.DeviceInspectionparm.CarryWeight = (respItem.vehicle.carryWeight != null) ? respItem.vehicle.carryWeight.ToString() : "";

                        
                        AppProperties.vehicle.VehicleOVRRScore = (null != respItem.vehicle.riskRating) ? respItem.vehicle.riskRating.riskRatingName : "";

                        //AppProperties.vehicle.Recomendation = ((null != respItem.plateConfiscationInstruction) ? ((respItem.plateConfiscationInstruction.Equals("false", StringComparison.CurrentCultureIgnoreCase)) ? "" : Resources.GetInstance().GetString("Confiscate")) : "");
                        AppProperties.vehicle.Recomendation = ((null != respItem.plateConfiscationInstruction) ? ((respItem.plateConfiscationInstruction.Equals("false", StringComparison.CurrentCultureIgnoreCase)) ? "" : "Confiscate") : "");
                        AppProperties.vehicle.Instruction = (null != respItem.inspectionInstruction) ? respItem.inspectionInstruction : "";
                       
                       
                        // Driver Details

                        if (respItem.vehicle.driver != null)
                        {
                            AppProperties.vehicle.VehiclDriver.LicNumber = (respItem.vehicle.driver.licenseNumber != null) ? respItem.vehicle.driver.licenseNumber.ToString() : "";
                            AppProperties.vehicle.VehiclDriver.IssuingEmirates = (respItem.vehicle.driver.licenseIssuingEmirate != null) ? respItem.vehicle.driver.licenseIssuingEmirate.ToString() : "";
                            AppProperties.vehicle.VehiclDriver.IssuingEmiratesAr = (respItem.vehicle.driver.licenseIssuingEmirate != null) ? respItem.vehicle.driver.licenseIssuingEmirate.ToString() : "";
                        }
                        else
                        {
                            AppProperties.vehicle.VehiclDriver.LicNumber = "";
                            AppProperties.vehicle.VehiclDriver.IssuingEmirates = "";
                            AppProperties.vehicle.VehiclDriver.IssuingEmiratesAr = "";
                        }


                        if (null != respItem.violations)
                        {

                            Violation[] violations = new Violation[respItem.violations.Length];

                            for (int i = 0; i < respItem.violations.Length; i++)
                            {
                                violations[i] = new Violation();
                                violations[i].ConfiscationReason = respItem.violations[i].plateConfiscatedReason;
                                violations[i].ConfiscationReasonAr = respItem.violations[i].plateConfiscatedReasonArabic;
                                violations[i].Inspector = respItem.violations[i].inspectorName;
                                violations[i].Inspection_location = respItem.violations[i].address;
                                violations[i].Inspection_locationAr = respItem.violations[i].addressArabic;
                                // violations[i].InspectionArea = respItem.violations[i].kloca;
                                violations[i].IsConfiscated = (respItem.violations[i].isPlateConfiscated != null) ? true : false;
                                violations[i].ViolationComments = respItem.violations[i].violation.comments;
                                violations[i].ViolationCommentsAr = respItem.violations[i].violation.commentsArabic;
                                violations[i].ViolationDueDays = respItem.violations[i].violation.dueDate;
                                violations[i].ViolationIssueDate = respItem.violations[i].time;
                                violations[i].ViolationID = (null == respItem.violations[i].violation.violationId) ? "0" : respItem.violations[i].violation.violationId.ToString();
                                violations[i].ViolationSeverity = respItem.violations[i].violation.severityLevel.name;
                                violations[i].ViolationSeverityAr = respItem.violations[i].violation.severityLevel.nameArabic;
                                //violations[i].ViolationSource = respItem.violations[i].violation.severityLevel.channel;
                                violations[i].ViolationStatus = respItem.violations[i].violation.statusName;
                                violations[i].ViolationTicketCode = respItem.violations[i].violation.ticketNumber;

                                //Driver Lic No

                                // violations[i]respItem.vehicle.driver.licenseNumber;
                                violations[i].RtaEmpID = respItem.violations[i].inspectorEmployeeId;
                                if (respItem.violations[i].vehicle != null)
                                {
                                    if (respItem.violations[i].vehicle.driver != null)
                                    {
                                        violations[i].DriverLicNo = (respItem.violations[i].vehicle.driver.licenseNumber == null) ? " " : respItem.violations[i].vehicle.driver.licenseNumber;
                                    }
                                }




                                if (null != respItem.violations[i].violation.testTypeName)
                                {
                                    if (!AppProperties.isComprehensive)
                                    {

                                        AppProperties.isComprehensive = ((respItem.violations[i].violation.testTypeName.StartsWith("Comprehens", StringComparison.CurrentCultureIgnoreCase)) && !(violations[i].ViolationStatus.Equals("Closed", StringComparison.CurrentCultureIgnoreCase))) ? true : false;
                                    }
                                }

                                // if (Resources.GetInstance().GetLocale().Equals(Resources.locale_EN))
                                if (AppProperties.Selected_Resource == "English")
                                {
                                    //checking if a major or severe violation already exists
                                    ((IDBManager)DBBusinessRuleManager.GetInstance()).GetRecommendation(violations[i].ViolationDueDays, violations[i].ViolationStatus, AppProperties.vehicle.Emirate, violations[i].ViolationSeverity);

                                }
                                else
                                {
                                    //checking a major or severe violation already exists
                                    ((IDBManager)DBBusinessRuleManager.GetInstance()).GetRecommendation(violations[i].ViolationDueDays, violations[i].ViolationStatus, AppProperties.vehicle.Emirate, violations[i].ViolationSeverityAr);
                                }

                                if (respItem.violations[i].violation.defects.Length > 0)
                                {
                                    Violation.Defects[] defects = new Violation.Defects[respItem.violations[i].violation.defects.Length];
                                    for (int j = 0; j < defects.Length; j++)
                                    {
                                        int id;
                                        defects[j] = new Violation.Defects();
                                        defects[j].DefectID = ((id = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectIDByCode(respItem.violations[i].violation.defects[j].code)) != -1) ? id : 0;

                                        //  string[][] cat = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectMainCategory();


                                        if (violations[i].ViolationStatus.StartsWith("Open", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            AppProperties.previousDefectIDs.Add(defects[j].DefectID);
                                        }
                                        defects[j].DefectType = respItem.violations[i].violation.defects[j].type;
                                        defects[j].DefectName = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectNameByID(respItem.violations[i].violation.defects[j].code);

                                        defects[j].DefectNameAr = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectNameArByID(respItem.violations[i].violation.defects[j].code);
                                        defects[j].DefectCode = respItem.violations[i].violation.defects[j].code;
                                        defects[j].DefectValue = respItem.violations[i].violation.defects[j].comment;
                                        //  defects[j].DefectCategory = respItem.violations[i].violation.defects[j].category;
                                        string[][] cat = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectMainCategoryByID(defects[j].DefectCode.ToString());

                                        if (cat.Length > 0)
                                        {
                                            defects[j].DefectCategory = cat[0][0].Trim();

                                        }


                                        if (null != respItem.violations[i].violation.defects[j].vehicleCategories && respItem.violations[i].violation.defects[j].vehicleCategories.Length > 0)
                                        {
                                            defects[j].DefectSeverity = respItem.violations[i].violation.defects[j].vehicleCategories[0].defectSeverityLevel.name;
                                            defects[j].DefectSeverityAr = respItem.violations[i].violation.defects[j].vehicleCategories[0].defectSeverityLevel.nameArabic;
                                        }



                                    }
                                    violations[i].Defect = defects;
                                }

                            }
                            AppProperties.vehicle.Violations = violations;
                        }


                    }
                    else if (respItem.response.code.Equals("2000"))
                    {
                        App.VSDLog.Info(@"\n inquireVehicleProfile() Service response is 2000");
                        AppProperties.businessError = true;
                        if ((respItem.response != null) && (respItem.response.message != null) && (respItem.response.message != ""))
                        {
                            AppProperties.errorMessageFromBusiness = "Service Responce Code: " + respItem.response.code + "\n Responce Message: " + respItem.response.message;
                        }
                        else
                        {
                            AppProperties.errorMessageFromBusiness = "un successfull responce code:2000";
                        }

                    }
                    else
                    {
                        App.VSDLog.Info(@"\n inquireVehicleProfile() WebSerive Response Message Description: NULL");
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
                        return null;
                    }
                }
                else
                {
                    AppProperties.IsServiceResponseNull = true;
                    AppProperties.errorMessageFromBusiness = "WebSerive Response: NULL";
                    App.VSDLog.Info(@"\n inquireVehicleProfile() WebSerive Response Message Description: NULL");
                    return null;
                }

            }
            ////catch (NullReferenceException exNull)
            ////{
            ////    System.Windows.Forms.MessageBox.Show("Vehicle Info not found");
            ////    AppProperties.isOnline = false;
            ////    return null;
            ////}
            catch (Exception e)
            {
                // App.VSDLog.Info(e.StackTrace);
                //  System.Windows.Forms.MessageBox.Show(e.Message);
                //  AppProperties.isOnline = false;
                AppProperties.IsException = true;

                if (e.InnerException != null && AppProperties.errorMessageFromBusiness != null)
                {
                    AppProperties.errorMessageFromBusiness = "Exception -----------  ";
                    AppProperties.errorMessageFromBusiness = e.InnerException.Message;
                    App.VSDLog.Info(e.InnerException.Message);
                    App.VSDLog.Info(e.StackTrace);

                }
                else
                {
                    AppProperties.errorMessageFromBusiness = "NULL Exception -----------  ";
                    AppProperties.errorMessageFromBusiness = "Exception.InnerException = NULL  ";
                }
                return null;
            }
            return AppProperties.vehicle;
        }
        /// <summary>
        /// From EtrFFIC
        /// </summary>
        /// <param name="category"></param>
        /// <param name="locale"></param>
        /// <returns></returns>

        Vehicle IVehicleProfile.GetVehicleProfileDetailsfromEtraffic(string country, string emirate, string plateCategory, string plateNumber, string plateCode)
        {
            try
            {
                AppProperties.isComprehensive = false;
                AppProperties.isSafety = true;

                handHeldService.HandHeldService service = new VSDApp.handHeldService.HandHeldService();


                handHeldService.AuthHeader header = new VSDApp.handHeldService.AuthHeader();
                header.password = AppProperties.empPassword;
                header.userName = AppProperties.empUserName;

                service.authHeader = header;


                handHeldService.VehiclePlate vehPlate = new VSDApp.handHeldService.VehiclePlate();
                vehPlate.category = (plateCategory != "") ? plateCategory.Trim() : "NA";
                vehPlate.code = (plateCode != "") ? plateCode.Trim() : "NA";
                vehPlate.number = plateNumber.Trim();
                vehPlate.source = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetPlateSourceCode((emirate == "") ? country : emirate).Trim();

                if (AppProperties.vehicle == null)
                {
                    AppProperties.vehicle = new Vehicle();
                }
                AppProperties.vehicle.Country = country.Trim();
                AppProperties.vehicle.Emirate = emirate.Trim();
                AppProperties.vehicle.PlateCategory = vehPlate.category.Trim();
                AppProperties.vehicle.PlateNumber = vehPlate.number.Trim();
                AppProperties.vehicle.PlateCode = vehPlate.code.Trim();



                handHeldService.InquireVehicleProfileResponseItem respItem = new VSDApp.handHeldService.InquireVehicleProfileResponseItem();
                service.Timeout = 180000;

                AppProperties.businessError = false;
                AppProperties.IsServiceResponseNull = false;
                AppProperties.IsException = false;
                AppProperties.NotFoundError = false;
                AppProperties.errorMessageFromBusiness = string.Empty;
                //  H-HH-IVP-1  H-PS-IVP-1

                respItem = service.inquireVehicleProfileFromETraffic("H-PS-IVP-1", vehPlate);

                if (respItem.response != null && respItem.response.code != null)
                {
                    if (respItem.response.code.Equals("1000"))
                    {

                        AppProperties.vehicle.ChassisNumber = respItem.vehicle.chassisNumber;
                        AppProperties.vehicle.Year = respItem.vehicle.manufacturedYear;
                        AppProperties.vehicle.RegExpiry = respItem.vehicle.registrationExpiry;
                        if (AppProperties.vehicle.Operator == null)
                        {
                            AppProperties.vehicle.Operator = new Operator();
                        }
                        if (AppProperties.Selected_Resource == "English")
                        {
                            if (respItem.vehicle.manufacturer.name == null || respItem.vehicle.manufacturer.name == "")
                                AppProperties.vehicle.Make = respItem.vehicle.manufacturer.nameArabic;
                            else
                                AppProperties.vehicle.Make = respItem.vehicle.manufacturer.name;
                            if (respItem.vehicle.manufacturer.model == null || respItem.vehicle.manufacturer.model == "")
                                AppProperties.vehicle.Model = respItem.vehicle.manufacturer.modelArabic;
                            else
                                AppProperties.vehicle.Model = respItem.vehicle.manufacturer.model;
                            if (respItem.vehicle.owner.ownerName == null || respItem.vehicle.owner.ownerName == "")
                                AppProperties.vehicle.Operator.OperatorName = respItem.vehicle.owner.ownerNameArabic;
                            else
                                AppProperties.vehicle.Operator.OperatorName = respItem.vehicle.owner.ownerName;
                        }
                        else
                        {
                            if (respItem.vehicle.manufacturer.name != null || respItem.vehicle.manufacturer.name != "")
                                AppProperties.vehicle.Make = respItem.vehicle.manufacturer.nameArabic;
                            else
                                AppProperties.vehicle.Make = respItem.vehicle.manufacturer.name;
                            if (respItem.vehicle.manufacturer.model != null || respItem.vehicle.manufacturer.model != "")
                                AppProperties.vehicle.Model = respItem.vehicle.manufacturer.model;
                            else
                                AppProperties.vehicle.Model = respItem.vehicle.manufacturer.modelArabic;
                            if (respItem.vehicle.owner.ownerName != null || respItem.vehicle.owner.ownerName != "")
                                AppProperties.vehicle.Operator.OperatorNameAr = respItem.vehicle.owner.ownerNameArabic;
                            else
                                AppProperties.vehicle.Operator.OperatorNameAr = respItem.vehicle.owner.ownerName;
                        }


                        //Vehicle Risk Ratting
                        if (respItem.vehicle.riskRating != null)
                        {
                            AppProperties.vehicle.RiskRating = (respItem.vehicle.riskRating.riskRatingName != null) ? respItem.vehicle.riskRating.riskRatingName.ToString() : "";
                            AppProperties.vehicle.RiskRattingColor = (respItem.vehicle.riskRating.riskRatingColor != null) ? respItem.vehicle.riskRating.riskRatingColor.ToString() : "";
                        }
                        else
                        {
                            AppProperties.vehicle.RiskRating = null;
                            AppProperties.vehicle.RiskRattingColor = null;
                        }
                        AppProperties.vehicle.IsElligibleForPocession = (respItem.vehicle.isEligibleForPossessionCertificate != null) ? respItem.vehicle.isEligibleForPossessionCertificate.ToString() : "";

                        //AppProperties.vehicle.Operator.OperatorNameAr = respItem.vehicle.owner.ownerNameArabic;
                        AppProperties.vehicle.Operator.TrafficFileNumber = respItem.vehicle.owner.trafficFileNumber;
             
                       // AppProperties.vehicle.RegExpiry = "2016-01-31";
                        AppProperties.vehicle.PlateCategory = plateCategory;
                        AppProperties.vehicle.PlateCode = plateCode;
                        AppProperties.vehicle.PlateNumber = plateNumber;                      
                        if (respItem.vehicle.category != null)
                        {
                            if (respItem.vehicle.category.code == null)
                            {
                                AppProperties.vehicle.VehicleCategory = "Heavy Vehicle";
                                AppProperties.vehicle.VehicleCategoryAr = "مركبة ثقيلة";

                            }
                            else
                            {
                                AppProperties.vehicle.VehicleCategory = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetVehicleCategoryNameByCode(respItem.vehicle.category.code).Trim();
                                if (AppProperties.vehicle.VehicleCategory != null && AppProperties.vehicle.VehicleCategory != "")
                                {
                                    AppProperties.vehicle.VehicleCategoryAr = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetAlternateVehicleCategory(AppProperties.vehicle.VehicleCategory, "");
                                }
                                else
                                {
                                    AppProperties.vehicle.VehicleCategoryAr = respItem.vehicle.category.nameArabic;
                                }

                            }
                        }
                        else
                        {
                            AppProperties.vehicle.VehicleCategory = "Heavy Vehicle";
                            AppProperties.vehicle.VehicleCategoryAr = "مركبة ثقيلة";
                        }
                        // Device Inspection Changes
                        AppProperties.vehicle.SubCategory = (respItem.vehicle.subCategoryNameArabic != null) ? respItem.vehicle.subCategoryNameArabic.ToString() : "";
                        AppProperties.vehicle.SubCategoryAr = (respItem.vehicle.subCategoryNameArabic != null) ? respItem.vehicle.subCategoryNameArabic.ToString() : "";

                        if (AppProperties.vehicle.DeviceInspectionparm == null)
                            AppProperties.vehicle.DeviceInspectionparm = new VehicleDeviceInspParams();

                        AppProperties.vehicle.DeviceInspectionparm.CarryWeight = (respItem.vehicle.carryWeight != null) ? respItem.vehicle.carryWeight.ToString() : "";


                        AppProperties.vehicle.VehicleOVRRScore = (null != respItem.vehicle.riskRating) ? respItem.vehicle.riskRating.riskRatingName : "";

                        //AppProperties.vehicle.Recomendation = ((null != respItem.plateConfiscationInstruction) ? ((respItem.plateConfiscationInstruction.Equals("false", StringComparison.CurrentCultureIgnoreCase)) ? "" : Resources.GetInstance().GetString("Confiscate")) : "");
                        AppProperties.vehicle.Recomendation = ((null != respItem.plateConfiscationInstruction) ? ((respItem.plateConfiscationInstruction.Equals("false", StringComparison.CurrentCultureIgnoreCase)) ? "" : "Confiscate") : "");
                        AppProperties.vehicle.Instruction = (null != respItem.inspectionInstruction) ? respItem.inspectionInstruction : "";

                        if (null != respItem.violations)
                        {

                            Violation[] violations = new Violation[respItem.violations.Length];

                            for (int i = 0; i < respItem.violations.Length; i++)
                            {
                                violations[i] = new Violation();
                                violations[i].ConfiscationReason = respItem.violations[i].plateConfiscatedReason;
                                violations[i].ConfiscationReasonAr = respItem.violations[i].plateConfiscatedReasonArabic;
                                violations[i].Inspector = respItem.violations[i].inspectorName;
                                violations[i].Inspection_location = respItem.violations[i].address;
                                violations[i].Inspection_locationAr = respItem.violations[i].addressArabic;
                                // violations[i].InspectionArea = respItem.violations[i].kloca;
                                violations[i].IsConfiscated = (respItem.violations[i].isPlateConfiscated != null) ? true : false;
                                violations[i].ViolationComments = respItem.violations[i].violation.comments;
                                violations[i].ViolationCommentsAr = respItem.violations[i].violation.commentsArabic;
                                violations[i].ViolationDueDays = respItem.violations[i].violation.dueDate;
                                violations[i].ViolationIssueDate = respItem.violations[i].time;
                                violations[i].ViolationID = (null == respItem.violations[i].violation.violationId) ? "0" : respItem.violations[i].violation.violationId.ToString();
                                violations[i].ViolationSeverity = respItem.violations[i].violation.severityLevel.name;
                                violations[i].ViolationSeverityAr = respItem.violations[i].violation.severityLevel.nameArabic;
                                //violations[i].ViolationSource = respItem.violations[i].violation.severityLevel.channel;
                                violations[i].ViolationStatus = respItem.violations[i].violation.statusName;
                                violations[i].ViolationTicketCode = respItem.violations[i].violation.ticketNumber;

                                //Driver Lic No

                                // violations[i]respItem.vehicle.driver.licenseNumber;
                                violations[i].RtaEmpID = respItem.violations[i].inspectorEmployeeId;
                                if (respItem.violations[i].vehicle != null)
                                {
                                    if (respItem.violations[i].vehicle.driver != null)
                                    {
                                        violations[i].DriverLicNo = (respItem.violations[i].vehicle.driver.licenseNumber == null) ? "" : respItem.violations[i].vehicle.driver.licenseNumber;
                                    }
                                }




                                if (null != respItem.violations[i].violation.testTypeName)
                                {
                                    if (!AppProperties.isComprehensive)
                                    {

                                        AppProperties.isComprehensive = ((respItem.violations[i].violation.testTypeName.StartsWith("Comprehens", StringComparison.CurrentCultureIgnoreCase)) && !(violations[i].ViolationStatus.Equals("Closed", StringComparison.CurrentCultureIgnoreCase))) ? true : false;
                                    }
                                }

                                // if (Resources.GetInstance().GetLocale().Equals(Resources.locale_EN))
                                if (AppProperties.Selected_Resource == "English")
                                {
                                    //checking if a major or severe violation already exists
                                    ((IDBManager)DBBusinessRuleManager.GetInstance()).GetRecommendation(violations[i].ViolationDueDays, violations[i].ViolationStatus, AppProperties.vehicle.Emirate, violations[i].ViolationSeverity);

                                }
                                else
                                {
                                    //checking a major or severe violation already exists
                                    ((IDBManager)DBBusinessRuleManager.GetInstance()).GetRecommendation(violations[i].ViolationDueDays, violations[i].ViolationStatus, AppProperties.vehicle.Emirate, violations[i].ViolationSeverityAr);
                                }

                                if (respItem.violations[i].violation.defects.Length > 0)
                                {
                                    Violation.Defects[] defects = new Violation.Defects[respItem.violations[i].violation.defects.Length];
                                    for (int j = 0; j < defects.Length; j++)
                                    {
                                        int id;
                                        defects[j] = new Violation.Defects();
                                        defects[j].DefectID = ((id = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectIDByCode(respItem.violations[i].violation.defects[j].code)) != -1) ? id : 0;

                                        //  string[][] cat = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectMainCategory();


                                        if (violations[i].ViolationStatus.StartsWith("Open", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            AppProperties.previousDefectIDs.Add(defects[j].DefectID);
                                        }
                                        defects[j].DefectType = respItem.violations[i].violation.defects[j].type;
                                        defects[j].DefectName = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectNameByID(respItem.violations[i].violation.defects[j].code);

                                        defects[j].DefectNameAr = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectNameArByID(respItem.violations[i].violation.defects[j].code);
                                        defects[j].DefectCode = respItem.violations[i].violation.defects[j].code;
                                        defects[j].DefectValue = respItem.violations[i].violation.defects[j].comment;
                                        //  defects[j].DefectCategory = respItem.violations[i].violation.defects[j].category;
                                        string[][] cat = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectMainCategoryByID(defects[j].DefectCode.ToString());

                                        if (cat.Length > 0)
                                        {
                                            defects[j].DefectCategory = cat[0][0].Trim();

                                        }


                                        if (null != respItem.violations[i].violation.defects[j].vehicleCategories && respItem.violations[i].violation.defects[j].vehicleCategories.Length > 0)
                                        {
                                            defects[j].DefectSeverity = respItem.violations[i].violation.defects[j].vehicleCategories[0].defectSeverityLevel.name;
                                            defects[j].DefectSeverityAr = respItem.violations[i].violation.defects[j].vehicleCategories[0].defectSeverityLevel.nameArabic;
                                        }



                                    }
                                    violations[i].Defect = defects;
                                }

                            }
                            AppProperties.vehicle.Violations = violations;
                        }


                    }
                    else if (respItem.response.code.Equals("2000"))
                    {
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

                    }
                    else
                    {
                        AppProperties.NotFoundError = true;
                       // AppProperties.businessError = true;
                        if ((respItem.response != null) && (respItem.response.message != null) && (respItem.response.message != ""))
                        {
                            AppProperties.errorMessageFromBusiness = "Service Responce Code: " + respItem.response.code + "\n Responce Message: " + respItem.response.message;
                        }
                        else
                        {
                            AppProperties.errorMessageFromBusiness = "WebSerive Response Message Description: NULL";
                        }
                        AppProperties.vehicle = null;
                        return null;
                    }
                }
                else
                {
                    AppProperties.IsServiceResponseNull = true;
                    AppProperties.errorMessageFromBusiness = "WebSerive Response: NULL";
                    return null;
                }

            }
            ////catch (NullReferenceException exNull)
            ////{
            ////    System.Windows.Forms.MessageBox.Show("Vehicle Info not found");
            ////    AppProperties.isOnline = false;
            ////    return null;
            ////}
            catch (Exception e)
            {
                // App.VSDLog.Info(e.StackTrace);
                //  System.Windows.Forms.MessageBox.Show(e.Message);
                //  AppProperties.isOnline = false;
                AppProperties.IsException = true;

                if (e.InnerException != null && AppProperties.errorMessageFromBusiness != null)
                {
                    AppProperties.errorMessageFromBusiness = "Exception -----------  ";
                    AppProperties.errorMessageFromBusiness = e.InnerException.Message;
                    App.VSDLog.Info(e.InnerException.Message);
                    App.VSDLog.Info(e.StackTrace);

                }
                else
                {
                    AppProperties.errorMessageFromBusiness = "NULL Exception -----------  ";
                    AppProperties.errorMessageFromBusiness = "Exception.InnerException = NULL  ";
                }
                return null;
            }
            return AppProperties.vehicle;
        }
        string IVehicleProfile.GetAlternateCategory(string category, string locale)
        {
            return ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetAlternateVehicleCategory(category, locale);
        }

        string IVehicleProfile.GetAlternateVehicleSubCategory(string subcategory, string category)
        {
            return ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetAlternateVehicleSubCategory(subcategory, category);
        }

        string[] IVehicleProfile.GetVehicleCategories()
        {
            IDBDataLoad iDBDataLoad = (IDBDataLoad)DBDataLoadManager.GetInstance();
            return iDBDataLoad.GetVehicleCategories();
        }

        string[] IVehicleProfile.GetVehiclePlateCategories(string emirate)
        {
            IDBDataLoad iDataLoad = (IDBDataLoad)DBDataLoadManager.GetInstance();
            if (null != emirate)
                emirate = emirate.Trim();
            return iDataLoad.GetPlateCategories(emirate);
        }
        string[] IVehicleProfile.GetVehiclePlateCodes(string plateCategory, string emirate)
        {
            IDBDataLoad iDataLoad = (IDBDataLoad)DBDataLoadManager.GetInstance();
            if (null != plateCategory)
                plateCategory = plateCategory.Trim();
            return iDataLoad.GetPlateCodes(plateCategory, emirate);
        }


        string[] IVehicleProfile.GetVehicleSubCategories(string vehicleCategory)
        {
            IDBDataLoad iDBDataLoad = (IDBDataLoad)DBDataLoadManager.GetInstance();
            return iDBDataLoad.GetVehicleSubCategories(vehicleCategory);
        }

        string IVehicleProfile.GetVehicleSubCategoryName(string subcategory, string vehicleCategory)
        {
            IDBDataLoad iDBDataLoad = (IDBDataLoad)DBDataLoadManager.GetInstance();
            return iDBDataLoad.GetVehicleCategoryName(subcategory, vehicleCategory);
        }


        #endregion
    }
}
