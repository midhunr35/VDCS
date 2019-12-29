using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using VSDApp.com.rta.vsd.hh.data;
using VSDApp.com.rta.vsd.hh.db;
using VSDApp.com.rta.vsd.hh.utilities;

namespace VSDApp.com.rta.vsd.hh.manager
{
    class ViolationHistoryManager : IViolationHistory
    {
        private static ViolationHistoryManager _violationHistoryManager;
        private ViolationHistoryManager() { }
        public static ViolationHistoryManager GetInstance()
        {
            if (_violationHistoryManager == null)
            {
                _violationHistoryManager = new ViolationHistoryManager();
            }
            return _violationHistoryManager;
        }




        #region IViolationHistory Members

        Vehicle IViolationHistory.GetViolationHistoryByPlateNumber(string country, string emirate, string plateCategory, string plateNumber, string plateCode)
        {
            try
            {
                App.VSDLog.Info("\n----- IViolationHistory.GetViolationHistoryByPlateNumber()------- ");
                violationService.ViolationInformationServiceClient service = new violationService.ViolationInformationServiceClient();



                

                //violationService.ViolationInformationService service = new violationService.ViolationInformationService();
                //   violationService.ViolationInformationService service = new violationService.ViolationBlockedService()


                violationService.Inspection[] inspection;
                violationService.AuthHeader header = new VSDApp.violationService.AuthHeader();
                header.userName = AppProperties.empUserName;
                header.password = AppProperties.empPassword;

                // violationService.
                //   service.authHeader = header;

                // service.Timeout = 180000;
                violationService.Response response = new VSDApp.violationService.Response();

               

                violationService.VehiclePlate vehPlate = new violationService.VehiclePlate();
                vehPlate.category = (plateCategory != "") ? plateCategory.Trim() : "NA";
                vehPlate.code = (plateCode != "") ? plateCode.Trim() : "NA";
                vehPlate.number = plateNumber.Trim();
                vehPlate.source = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetPlateSourceCode((emirate == "") ? country : emirate).Trim();
                App.VSDLog.Info(@"\n plate.source=" + vehPlate.source
                   + ", \n plate.number=" + vehPlate.number
                   + ", \n plate.category=" + vehPlate.category
                   + ", \n plate.code=" + vehPlate.code);
                //service.max

                //  inspection = service.inquireViolationById("S-VI-IV-1", vioID, out response);
                inspection = service.inquireViolationByPlate(header, "S-VI-IV-1", vehPlate, out response);
                //  service.inquireViolationByPlate(header,"",

             //   02.01.04.01.160061153



                if (response.code.Equals("1000"))
                {
                    App.VSDLog.Info(@"\n inquireViolationByPlate response code:1000");
                    // Resources res = Resources.GetInstance();
                    // bool isEng = (res.GetLocale().Equals(Resources.locale_EN));
                    bool isEng = true;
                    AppProperties.vehicle = null;

                    AppProperties.vehicle = new Vehicle();
                    AppProperties.vehicle.PlateCategory = inspection[0].vehicle.plateDetails.category;
                    AppProperties.vehicle.PlateCode = inspection[0].vehicle.plateDetails.code;
                    AppProperties.vehicle.Emirate = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetPlateEmirateByCode(inspection[0].vehicle.plateDetails.source).Trim();
                    AppProperties.vehicle.PlateNumber = inspection[0].vehicle.plateDetails.number;
                    
                    // AppProperties.vehicle.Country = inspection[0].vehicle.country;
                    if (inspection[0].vehicle.country == null)
                        AppProperties.vehicle.Country = country;
                    else
                        AppProperties.vehicle.Country = inspection[0].vehicle.country;
                    AppProperties.vehicle.ChassisNumber = inspection[0].vehicle.chassisNumber;
                    AppProperties.vehicle.Year = inspection[0].vehicle.manufacturedYear;
                    if (AppProperties.vehicle.Operator == null)
                    {
                        AppProperties.vehicle.Operator = new Operator();
                    }
                    if (AppProperties.Selected_Resource == "English")
                    {
                        if (inspection[0].vehicle.manufacturer.name == null || inspection[0].vehicle.manufacturer.name == "")
                            AppProperties.vehicle.Make = inspection[0].vehicle.manufacturer.nameArabic;
                        else
                            AppProperties.vehicle.Make = inspection[0].vehicle.manufacturer.name;
                        if (inspection[0].vehicle.manufacturer.model == null || inspection[0].vehicle.manufacturer.model == "")
                            AppProperties.vehicle.Model = inspection[0].vehicle.manufacturer.modelArabic;
                        else
                            AppProperties.vehicle.Model = inspection[0].vehicle.manufacturer.model;
                        if (inspection[0].vehicle.owner.ownerName == null || inspection[0].vehicle.owner.ownerName == "")
                            AppProperties.vehicle.Operator.OperatorName = inspection[0].vehicle.owner.ownerNameArabic;
                        else
                            AppProperties.vehicle.Operator.OperatorName = inspection[0].vehicle.owner.ownerName;
                    }
                    else
                    {
                        if (inspection[0].vehicle.manufacturer.name != null || inspection[0].vehicle.manufacturer.name != "")
                            AppProperties.vehicle.Make = inspection[0].vehicle.manufacturer.nameArabic;
                        else
                            AppProperties.vehicle.Make = inspection[0].vehicle.manufacturer.name;
                        if (inspection[0].vehicle.manufacturer.model != null || inspection[0].vehicle.manufacturer.model != "")
                            AppProperties.vehicle.Model = inspection[0].vehicle.manufacturer.model;
                        else
                            AppProperties.vehicle.Model = inspection[0].vehicle.manufacturer.modelArabic;
                        if (inspection[0].vehicle.owner.ownerName != null || inspection[0].vehicle.owner.ownerName != "")
                            AppProperties.vehicle.Operator.OperatorName = inspection[0].vehicle.owner.ownerNameArabic;
                        else
                            AppProperties.vehicle.Operator.OperatorName = inspection[0].vehicle.owner.ownerName;
                    }


                    //  AppProperties.vehicle.Operator = new Operator();
                    AppProperties.vehicle.Operator.OperatorName = inspection[0].vehicle.owner.ownerName;
                    AppProperties.vehicle.Operator.OperatorNameAr = inspection[0].vehicle.owner.ownerNameArabic;
                    AppProperties.vehicle.Operator.TrafficFileNumber = inspection[0].vehicle.owner.trafficFileNumber;

                    
                   // AppProperties.vehicle.VehicleOVRRScore = (null != inspection[0].vehicle.riskRating) ? inspection[0].vehicle.riskRating.riskRatingName : "";

                    if ((inspection[0].vehicle.riskRating != null) && (inspection[0].vehicle.riskRating.riskRatingName!=null))
                    {
                        AppProperties.vehicle.RiskRating = (inspection[0].vehicle.riskRating.riskRatingName != null) ? Convert.ToString(inspection[0].vehicle.riskRating.riskRatingName) : "";
                        
                    }                    
                    else
                    {    
                        //To Do
                        AppProperties.vehicle.RiskRating = "";
                    }
                    
                    if ((inspection[0].vehicle.driver != null) && (inspection[0].vehicle.driver.riskRatingDriver != null))
                    {
                        AppProperties.vehicle.DriverRiskRattingName = (inspection[0].vehicle.driver.riskRatingDriver.riskRatingName != null) ? Convert.ToString(inspection[0].vehicle.driver.riskRatingDriver.riskRatingName) : "";
                    }
                    else
                    {
                        AppProperties.vehicle.DriverRiskRattingName = "";
                    }
                    
                   // AppProperties.vehicle.IsImpoundingGracePeriod = "F";
                   
                    
                    
                    //AppProperties.vehicle.Recomendation = ((null != respItem.plateConfiscationInstruction) ? ((respItem.plateConfiscationInstruction.Equals("false", StringComparison.CurrentCultureIgnoreCase)) ? "" : Resources.GetInstance().GetString("Confiscate")) : "");
                    // AppProperties.vehicle.Recomendation = ((null != inspection[0].plateConfiscationInstruction) ? ((inspection[0].plateConfiscationInstruction.Equals("false", StringComparison.CurrentCultureIgnoreCase)) ? "" : "Confiscate") : "");
                    // AppProperties.vehicle.Instruction = (null != inspection[0].inspectionInstruction) ? inspection[0].inspectionInstruction : "";

                    // Violation[] searchedViolation = new Violation[1];
                    if (null != inspection)
                    {

                        Violation[] searchedViolation = new Violation[inspection.Length];

                        for (int i = 0; i < inspection.Length; i++)
                        {
                            searchedViolation[i] = new Violation();



                            //  searchedViolation[0] = new Violation();

                            searchedViolation[i].ViolationComments = inspection[i].violation.comments;
                            searchedViolation[i].ViolationCommentsAr = inspection[i].violation.commentsArabic;
                            searchedViolation[i].Inspection_location = inspection[i].locationName;
                            searchedViolation[i].Inspection_locationAr = inspection[i].locationNameArabic;
                            searchedViolation[i].ViolationDueDays = inspection[i].violation.dueDate;
                            searchedViolation[i].ViolationIssueDate = inspection[i].time;
                            searchedViolation[i].ViolationID = inspection[i].violation.ticketNumber;
                            searchedViolation[i].ViolationTicketCode = inspection[i].violation.ticketNumber;
                            searchedViolation[i].ViolationStatus = (isEng) ? inspection[i].violation.statusName : inspection[i].violation.statusNameArabic;
                            searchedViolation[i].ViolationSeverity = inspection[i].violation.severityLevel.name;
                            searchedViolation[i].ViolationSeverityAr = inspection[i].violation.severityLevel.nameArabic;
                            searchedViolation[i].DriverLicNo = inspection[i].vehicle.driver.licenseNumber;
                            searchedViolation[i].RtaEmpID = inspection[i].inspectorEmployeeId;
                           //TO Do
                            searchedViolation[i].GracePeriod = inspection[i].violation.groundingGracePeriod;
                            
                            Violation.Defects[] searchedDefects = new Violation.Defects[inspection[i].violation.defects.Length];
                            for (int j = 0; j < searchedDefects.Length; j++)
                            {
                                int id;
                                searchedDefects[j] = new Violation.Defects();
                                searchedDefects[j].DefectID = ((id = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectIDByCode(inspection[i].violation.defects[j].code)) != -1) ? id : 0;
                                //  searchedDefects[i].DefectName = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectNameByID(inspection[0].violation.defects[i].code);
                                searchedDefects[j].DefectName = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectNameByID(inspection[i].violation.defects[j].code);
                                searchedDefects[j].DefectNameAr = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectNameArByID(inspection[i].violation.defects[j].code);
                                searchedDefects[j].DefectCode = inspection[i].violation.defects[j].code;
                                searchedDefects[j].DefectType = inspection[i].violation.defects[j].type;
                                searchedDefects[j].DefectCategory = inspection[i].violation.defects[j].category;
                                searchedDefects[j].DefectValue = inspection[i].violation.defects[j].comment;
                                string[][] cat = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectMainCategoryByID(searchedDefects[j].DefectCode.ToString());

                                if (cat.Length > 0)
                                {
                                    searchedDefects[j].DefectCategory = cat[0][0].Trim();

                                }
                                if (null != inspection[i].violation.defects[j].vehicleCategories && inspection[i].violation.defects[j].vehicleCategories.Length > 0)
                                {
                                    searchedDefects[j].DefectSeverity = inspection[i].violation.defects[j].vehicleCategories[0].defectSeverityLevel.name;
                                    searchedDefects[j].DefectSeverityAr = inspection[i].violation.defects[j].vehicleCategories[0].defectSeverityLevel.nameArabic;
                                }
                            }
                            searchedViolation[i].Defect = searchedDefects;
                            AppProperties.vehicle.Violations = searchedViolation;

                        }
                    }
                }
                else if (response.code.Equals("2000"))
                {
                    AppProperties.businessError = true;
                    AppProperties.errorMessageFromBusiness = response.message;
                    App.VSDLog.Info(@"\n inquireViolationByPlate response code:2000");
                    App.VSDLog.Info(AppProperties.errorMessageFromBusiness);


                }
                else
                {
                    AppProperties.NotFoundError = true;
                    AppProperties.errorMessageFromBusiness = response.message;
                    //System.Windows.Forms.MessageBox.Show(response.message);
                    App.VSDLog.Info(@"\n inquireViolationByPlate Service response Not 1000,2000");
                    App.VSDLog.Info(AppProperties.errorMessageFromBusiness);
                }

            }
            catch (Exception connectionEx)
            {
               // App.VSDLog.Info(connectionEx.StackTrace)
                App.VSDLog.Info(@"\n inquireViolationByPlate Service Calling exception");
                    AppProperties.IsException = true;
                    if (connectionEx.InnerException != null)
                    {
                        AppProperties.errorMessageFromBusiness = connectionEx.InnerException.Message;
                    }
                    else
                    {
                        AppProperties.errorMessageFromBusiness = connectionEx.Message;
                    }
               // System.Windows.Forms.MessageBox.Show(connectionEx.Message);
            }
            return null;
        }


        Vehicle IViolationHistory.GetViolationHistoryByID(string vioID)
        {
            try
            {
                App.VSDLog.Info(@"IViolationHistory.GetViolationHistoryByID: ------------------START----------------------");
                
                violationService.ViolationInformationServiceClient service = new violationService.ViolationInformationServiceClient();
                //violationService.ViolationInformationService service = new violationService.ViolationInformationService();
                //   violationService.ViolationInformationService service = new violationService.ViolationBlockedService()

                App.VSDLog.Info(@"IViolationHistory.GetViolationHistoryByID:VIolation ID : "+vioID);
                violationService.Inspection[] inspection;
                violationService.AuthHeader header = new VSDApp.violationService.AuthHeader();
                header.userName = AppProperties.empUserName;
                header.password = AppProperties.empPassword;


                //   service.authHeader = header;

                // service.Timeout = 180000;
                violationService.Response response = new VSDApp.violationService.Response();
                //  inspection = service.inquireViolationById("S-VI-IV-1", vioID, out response);
                inspection = service.inquireViolationById(header, "S-VI-IV-1", vioID, out response);
                // service.inquireViolationByPlate(



                

                if (response.code.Equals("1000"))
                {
                    App.VSDLog.Info(@"IViolationHistory.GetViolationHistoryByID:Service Response Code: 1000 : ");
                    // Resources res = Resources.GetInstance();
                    // bool isEng = (res.GetLocale().Equals(Resources.locale_EN));
                    bool isEng = true;
                    AppProperties.vehicle = null;

                    AppProperties.vehicle = new Vehicle();
                    AppProperties.vehicle.PlateCategory = inspection[0].vehicle.plateDetails.category;
                    AppProperties.vehicle.PlateCode = inspection[0].vehicle.plateDetails.code;
                    AppProperties.vehicle.Emirate = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetPlateEmirateByCode(inspection[0].vehicle.plateDetails.source).Trim();
                    AppProperties.vehicle.PlateNumber = inspection[0].vehicle.plateDetails.number;
                    AppProperties.vehicle.Country = inspection[0].vehicle.country;

                    AppProperties.vehicle.ChassisNumber = inspection[0].vehicle.chassisNumber;
                    AppProperties.vehicle.Make = (isEng) ? inspection[0].vehicle.manufacturer.name : inspection[0].vehicle.manufacturer.nameArabic;
                    AppProperties.vehicle.Model = (isEng) ? inspection[0].vehicle.manufacturer.model : inspection[0].vehicle.manufacturer.modelArabic;
                    AppProperties.vehicle.Year = inspection[0].vehicle.manufacturedYear;


                    AppProperties.vehicle.Operator = new Operator();
                    AppProperties.vehicle.Operator.OperatorName = inspection[0].vehicle.owner.ownerName;
                    AppProperties.vehicle.Operator.OperatorNameAr = inspection[0].vehicle.owner.ownerNameArabic;
                    AppProperties.vehicle.Operator.TrafficFileNumber = inspection[0].vehicle.owner.trafficFileNumber;

                    if ((inspection[0].vehicle.riskRating != null) && (inspection[0].vehicle.riskRating.riskRatingName != null))
                    {
                        AppProperties.vehicle.RiskRating = (inspection[0].vehicle.riskRating.riskRatingName != null) ? Convert.ToString(inspection[0].vehicle.riskRating.riskRatingName) : "";

                    }
                    else
                    {
                        //To Do
                        AppProperties.vehicle.RiskRating = "";
                    }
                    if ((inspection[0].vehicle.driver != null) && (inspection[0].vehicle.driver.riskRatingDriver != null))
                    {
                        AppProperties.vehicle.DriverRiskRattingName = (inspection[0].vehicle.driver.riskRatingDriver.riskRatingName != null) ? Convert.ToString(inspection[0].vehicle.driver.riskRatingDriver.riskRatingName) : "";
                    }
                    else
                    {
                        AppProperties.vehicle.DriverRiskRattingName = "";
                    }



                    Violation[] searchedViolation = new Violation[1];
                    searchedViolation[0] = new Violation();

                    searchedViolation[0].ViolationComments = inspection[0].violation.comments;
                    searchedViolation[0].ViolationCommentsAr = inspection[0].violation.commentsArabic;
                    searchedViolation[0].Inspection_location = inspection[0].locationName;
                    searchedViolation[0].Inspection_locationAr = inspection[0].locationNameArabic;
                    searchedViolation[0].ViolationDueDays = inspection[0].violation.dueDate;
                    searchedViolation[0].ViolationIssueDate = inspection[0].time;
                    searchedViolation[0].ViolationID = vioID;
                    searchedViolation[0].ViolationTicketCode = vioID;
                    searchedViolation[0].ViolationStatus = (isEng) ? inspection[0].violation.statusName : inspection[0].violation.statusNameArabic;
                    searchedViolation[0].ViolationSeverity = inspection[0].violation.severityLevel.name;
                    searchedViolation[0].ViolationSeverityAr = inspection[0].violation.severityLevel.nameArabic;
                    //TO Do
                    searchedViolation[0].GracePeriod = inspection[0].violation.groundingGracePeriod;
                    searchedViolation[0].DriverLicNo = inspection[0].vehicle.driver.licenseNumber;
                    searchedViolation[0].RtaEmpID = inspection[0].inspectorEmployeeId;

                    Violation.Defects[] searchedDefects = new Violation.Defects[inspection[0].violation.defects.Length];
                    for (int i = 0; i < searchedDefects.Length; i++)
                    {
                        int id;
                        searchedDefects[i] = new Violation.Defects();
                        searchedDefects[i].DefectID = ((id = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectIDByCode(inspection[0].violation.defects[i].code)) != -1) ? id : 0;
                        //  searchedDefects[i].DefectName = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectNameByID(inspection[0].violation.defects[i].code);
                        searchedDefects[i].DefectName = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectNameByID(inspection[0].violation.defects[i].code);
                        searchedDefects[i].DefectNameAr = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectNameArByID(inspection[0].violation.defects[i].code);
                        searchedDefects[i].DefectCode = inspection[0].violation.defects[i].code;
                        searchedDefects[i].DefectType = inspection[0].violation.defects[i].type;
                        searchedDefects[i].DefectCategory = inspection[0].violation.defects[i].category;
                        searchedDefects[i].DefectValue = inspection[0].violation.defects[i].comment;
                        string[][] cat = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectMainCategoryByID(searchedDefects[i].DefectCode.ToString());

                        if (cat.Length > 0)
                        {
                            searchedDefects[i].DefectCategory = cat[0][0].Trim();

                        }
                        if (null != inspection[0].violation.defects[i].vehicleCategories && inspection[0].violation.defects[i].vehicleCategories.Length > 0)
                        {
                            searchedDefects[i].DefectSeverity = inspection[0].violation.defects[i].vehicleCategories[0].defectSeverityLevel.name;

                            searchedDefects[i].DefectSeverityAr = inspection[0].violation.defects[i].vehicleCategories[0].defectSeverityLevel.name;
                        }
                    }
                    searchedViolation[0].Defect = searchedDefects;
                    AppProperties.vehicle.Violations = searchedViolation;

                }
                else if (response.code.Equals("2000"))
                {
                    App.VSDLog.Info(@"IViolationHistory.GetViolationHistoryByID:Service Response Code: 2000 : ");
                    AppProperties.businessError = true;
                    AppProperties.errorMessageFromBusiness = response.message;

                }
                else
                {
                    App.VSDLog.Info(@"IViolationHistory.GetViolationHistoryByID:Service Response is NULL ");
                    AppProperties.NotFoundError = true;
                    AppProperties.errorMessageFromBusiness = response.message;
                   // System.Windows.Forms.MessageBox.Show(response.message);
                }

            }
            catch (Exception connectionEx)
            {
                App.VSDLog.Info(@"IViolationHistory.GetViolationHistoryByID:Exception while calling Service ");
                App.VSDLog.Info(connectionEx.StackTrace);
               // System.Windows.Forms.MessageBox.Show(connectionEx.Message);
                AppProperties.IsException = true;
                AppProperties.errorMessageFromBusiness = connectionEx.InnerException.Message;
            }
            return null;
        }

        Vehicle IViolationHistory.GetOfflineViolationHistoryByID(string PrivisionalViolationID)
        {


            try
            {
                
                IDBDataLoad iDataLoad = ((IDBDataLoad)DBDataLoadManager.GetInstance());
                string[] violation = iDataLoad.GetViolationByTicketCode(PrivisionalViolationID);                
                /*
                rs.ReadFirst();
                row[0] = rs["Violation_ID"].ToString();
                row[1] = rs["Due_Date"].ToString();
                row[2] = rs["Comments"].ToString();
                row[3] = rs["Comments_A"].ToString();
                row[4] = rs["Fine_Amount"].ToString();
                row[5] = rs["Timestamp"].ToString();
                row[6] = rs["Violation_Status"].ToString();
                row[7] = rs["Created_By"].ToString();
                */
                if (violation[0] == null)
                {
                    //No Violation Found error
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

                    return null;
                }

                List<Object> inspections;
                inspections = iDataLoad.GetStoredInspectionByTicketNumber(violation[0]);
                //rows[0] = rs["Inspection_ID"].ToString();
                //rows[1] = rs["Vehicle_Info_ID"].ToString();
                //rows[2] = rs["Violation_ID"].ToString();
                //rows[3] = rs["Location_ID"].ToString();
                //rows[4] = rs["Plate_Condition"].ToString();
                //rows[5] = rs["Is_Reg_Exp"].ToString();
                //rows[6] = rs["Is_Plate_Confiscated"].ToString();
                //rows[7] = rs["Plate_Confiscation_Reason"].ToString();
                //rows[8] = rs["Plate_Confiscation_Reason_A"].ToString();
                //rows[9] = rs["Reported_Inspector_Name"].ToString();
                //rows[10] = rs["Reported_Inspector_Name_A"].ToString();
                //rows[11] = rs["Inspection_Timestamp"].ToString();
                string[] Severity;
                Severity = iDataLoad.GetSeverityNamesByID(Int32.Parse(violation[8]));
               // row[0] = rs["Severity_Level"].ToString();
              //  row[1] = rs["Severity_Level_Name"].ToString();
              //  row[2] = rs["Severity_Level_Name_A"].ToString();

                int inspectionID = -1;
                int vehInfoID = -1;
                int vioID = -1;
               
                for (int i = 0; i < inspections.Count; i++)
                {
                  //  storedInspection[i] = new VSDApp.handHeldService.Inspection();
                    string[] inspection = (string[])inspections[i];
                    inspectionID = Int32.Parse(inspection[0]);
                    vehInfoID = Int32.Parse(inspection[1]);
                    vioID = Int32.Parse(inspection[2]);
                    int locID = Int32.Parse(inspection[3]);

                    string locationExternalCode = iDataLoad.GetLocationCode(locID);

                    string[] vehicleInfo = iDataLoad.GetVehicleInfo(vehInfoID);
                    
                    /*
                     * vehicleInfo[0] = rs["Vehicle_Info_ID"].ToString();
                        vehicleInfo[1] = rs["Vehicle_Category_ID"].ToString();
                vehicleInfo[2] = rs["Vehicle_Plate_Category"].ToString();
                vehicleInfo[3] = rs["Vehicle_Plate_Code"].ToString();
                vehicleInfo[4] = rs["Vehicle_Plate_Source"].ToString();
                vehicleInfo[5] = rs["Vehicle_Plate_Number"].ToString();
                vehicleInfo[6] = rs["Vehicle_Country"].ToString();
                vehicleInfo[7] = rs["Vehicle_Model"].ToString();
                vehicleInfo[8] = rs["Vehicle_Model_A"].ToString();
                vehicleInfo[9] = rs["Make_Year"].ToString();
                vehicleInfo[10] = rs["Vehicle_Chassis_Number"].ToString();
                vehicleInfo[11] = rs["Vehicle_Reg_Expiry"].ToString();
                vehicleInfo[12] = rs["IsDisabled"].ToString();
                vehicleInfo[13] = rs["Timestamp"].ToString();
                    */
                    int vehicleCategoryID = Int32.Parse(vehicleInfo[1]);

                    String vehCatCode = iDataLoad.GetVehicleCategoryCode(vehicleCategoryID);

                    string[] ownerInfo = iDataLoad.GetOwnerInfo(vehInfoID);

                    /*
                     *  row[0] = rs["Owner_Info_ID"].ToString();
                row[1] = rs["Vehicle_Info_ID"].ToString();
                row[2] = rs["Owner_Name"].ToString();
                row[3] = rs["Owner_Name_A"].ToString();
                row[4] = rs["Trade_License_Number"].ToString();
                row[5] = rs["Traffic_File_Number"].ToString();
                row[6] = rs["IsDisabled"].ToString();
                row[7] = rs["Timestamp"].ToString();
                   */

                    string[] driverInfo = iDataLoad.GetDriverInfo(vehInfoID);
                    /*
                    row[0] = rs["Driver_Info_ID"].ToString();
                    row[1] = rs["Vehicle_Info_ID"].ToString();
                    row[2] = rs["Driver_Name"].ToString();
                    row[3] = rs["Driver_Name_A"].ToString();
                    row[4] = rs["Driver_License_Number"].ToString();
                    row[5] = rs["IsDisabled"].ToString();
                    row[6] = rs["Timestamp"].ToString();*/
                  //  string[] violation = iDataLoad.GetViolation(Int32.Parse(vioID));
                    string[][] defectCodes = iDataLoad.GetDefectCodesForViolation(vioID);



                    AppProperties.vehicle = null;

                    AppProperties.vehicle = new Vehicle();
                    AppProperties.vehicle.PlateCategory = ("" != vehicleInfo[2].Trim()) ? vehicleInfo[2].Trim() : "NA";
                    AppProperties.vehicle.PlateCode = ("" != vehicleInfo[3].Trim()) ? vehicleInfo[3].Trim() : "NA";
                    AppProperties.vehicle.PlateNumber = vehicleInfo[5].Trim();
                    AppProperties.vehicle.PlateSource = iDataLoad.GetPlateSourceCode((vehicleInfo[4].Trim() != "") ? vehicleInfo[4].Trim() : vehicleInfo[6].Trim()).Trim();


                    



                 //  AppProperties.vehicle.Emirate = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetPlateEmirateByCode(inspection[0].vehicle.plateDetails.source).Trim();
                   
                   
                   AppProperties.vehicle.Country = ("" != vehicleInfo[6].Trim()) ? vehicleInfo[6].Trim() : "NA";

                   AppProperties.vehicle.ChassisNumber = ("" != vehicleInfo[10].Trim()) ? vehicleInfo[10].Trim() : "NA";
                   AppProperties.vehicle.Make = ("" != vehicleInfo[9].Trim()) ? vehicleInfo[9].Trim() : "NA";
                   AppProperties.vehicle.Model = ("" != vehicleInfo[7].Trim()) ? vehicleInfo[7].Trim() : "NA";
                   AppProperties.vehicle.Year = ("" != vehicleInfo[7].Trim()) ? vehicleInfo[7].Trim() : "NA";


                   AppProperties.vehicle.Operator = new Operator();


                   AppProperties.vehicle.Operator.OperatorName = ("" != ownerInfo[2].Trim()) ? ownerInfo[2].Trim() : "NA";
                   AppProperties.vehicle.Operator.OperatorNameAr = ("" != ownerInfo[3].Trim()) ? ownerInfo[3].Trim() : "NA";
                   AppProperties.vehicle.Operator.TrafficFileNumber = ("" != ownerInfo[5].Trim()) ? ownerInfo[5].Trim() : "NA";

                   


                   Violation[] searchedViolation = new Violation[1];
                   searchedViolation[0] = new Violation();

                   searchedViolation[0].ViolationComments = ("" != violation[2].Trim()) ? violation[2].Trim() : "NA";
                   searchedViolation[0].ViolationCommentsAr = ("" != violation[3].Trim()) ? violation[3].Trim() : "NA";
                  // searchedViolation[0].Inspection_location = inspection[0].locationName;
                  // searchedViolation[0].Inspection_locationAr = inspection[0].locationNameArabic;
                   searchedViolation[0].ViolationDueDays = DateTime.Parse(("" != violation[1].Trim()) ? violation[1].Trim() : "NA");
                   searchedViolation[0].ViolationIssueDate = DateTime.Parse(("" != violation[5].Trim()) ? violation[5].Trim() : "NA");
                   searchedViolation[0].ViolationID = ("" != violation[0].Trim()) ? violation[0].Trim() : "NA";
                   searchedViolation[0].ViolationTicketCode = PrivisionalViolationID;
                  // searchedViolation[0].ViolationStatus = (isEng) ? inspection[0].violation.statusName : inspection[0].violation.statusNameArabic;
                   searchedViolation[0].ViolationStatus = ("" != violation[6].Trim()) ? violation[6].Trim() : "NA";
                   if (Severity.Length > 0)
                   {
                       searchedViolation[0].ViolationSeverity = ("" != Severity[1].Trim()) ? Severity[1].Trim() : "NA";
                       searchedViolation[0].ViolationSeverityAr = ("" != Severity[2].Trim()) ? Severity[2].Trim() : "NA";
                   }

                   searchedViolation[0].DriverLicNo = ("" != driverInfo[4].Trim()) ? driverInfo[4].Trim() : "NA";
                  // searchedViolation[0].RtaEmpID = inspection[0].inspectorEmployeeId;




                   Violation.Defects[] searchedDefects = new Violation.Defects[defectCodes.Length];

                   for (int j = 0; j < searchedDefects.Length; j++)
                   {
                       int id;
                      // defects[j] = new VSDApp.handHeldService.Defect();
                     //  defects[j].code = defectCodes[j][0].Trim();
                     //  defects[j].comment = defectCodes[j][1].Trim();


                       searchedDefects[j] = new Violation.Defects();
                       searchedDefects[j].DefectID = ((id = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectIDByCode(defectCodes[j][0].Trim())) != -1) ? id : 0;
                       searchedDefects[j].DefectName = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectNameByID(defectCodes[j][0].Trim());
                       searchedDefects[j].DefectNameAr = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectNameArByID(defectCodes[j][0].Trim());
                       searchedDefects[j].DefectCode = defectCodes[j][0].Trim();
                      // searchedDefects[i].DefectType = inspection[0].violation.defects[i].type;
                      // searchedDefects[i].DefectCategory = inspection[0].violation.defects[i].category;
                       searchedDefects[i].DefectValue = defectCodes[j][1];
                       string[][] cat = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectMainCategoryByID(searchedDefects[j].DefectCode.ToString());

                       if (cat.Length > 0)
                       {
                           searchedDefects[j].DefectCategory = cat[0][0].Trim();

                       }
                       string[] DefectSeverities = iDataLoad.GetDefectSeverityByDefectIDVehCat(searchedDefects[j].DefectID, vehicleCategoryID);
                       if (DefectSeverities.Length > 0)
                       {
                           searchedDefects[j].DefectSeverity = ("" != DefectSeverities[1].Trim()) ? DefectSeverities[1].Trim() : "NA";
                           searchedDefects[j].DefectSeverityAr = ("" != DefectSeverities[2].Trim()) ? DefectSeverities[2].Trim() : "NA";
                       }
                      // if (null != inspection[0].violation.defects[i].vehicleCategories && inspection[0].violation.defects[i].vehicleCategories.Length > 0)
                     //  {
                     //      searchedDefects[i].DefectSeverity = inspection[0].violation.defects[i].vehicleCategories[0].defectSeverityLevel.name;

                         //  searchedDefects[i].DefectSeverityAr = inspection[0].violation.defects[i].vehicleCategories[0].defectSeverityLevel.name;
                      // }
                   }
                   searchedViolation[0].Defect = searchedDefects;
                   AppProperties.vehicle.Violations = searchedViolation;
                   


                    /*
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

                    handHeldService.VehicleCategory vehCat = new VSDApp.handHeldService.VehicleCategory();
                    vehCat.code = vehCatCode.Trim();
                    veh.category = vehCat;

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

                    */

                }

                return AppProperties.vehicle;
            }
            catch (Exception connectionEx)
            {
                App.VSDLog.Info(connectionEx.StackTrace);
               // System.Windows.Forms.MessageBox.Show(connectionEx.Message);
                AppProperties.IsException = true;
                AppProperties.errorMessageFromBusiness = connectionEx.InnerException.Message;
                return null;
            }
        }

        DataTable IViolationHistory.GetOfflineViolationHistoryAllByID(string PrivisionalViolationID)
        {
            try
            {

                IDBDataLoad iDataLoad = ((IDBDataLoad)DBDataLoadManager.GetInstance());
                //string[] violation = iDataLoad.GetViolationByTicketCode(PrivisionalViolationID);
                DataTable dt = iDataLoad.GetOfflineViolationAllByTicketCode(PrivisionalViolationID);
                /*
                rs.ReadFirst();
                row[0] = rs["Violation_ID"].ToString();
                row[1] = rs["Due_Date"].ToString();
                row[2] = rs["Comments"].ToString();
                row[3] = rs["Comments_A"].ToString();
                row[4] = rs["Fine_Amount"].ToString();
                row[5] = rs["Timestamp"].ToString();
                row[6] = rs["Violation_Status"].ToString();
                row[7] = rs["Created_By"].ToString();
                row[8] = rs["Severity_Level_ID"].ToString();
                */
                if (dt == null || dt.Rows.Count<1)
                {
                    //No Violation Found error
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

                    return null;
                }
                AppProperties.IsException = false;
                return dt;
            }
            catch (Exception connectionEx)
            {
                App.VSDLog.Info(connectionEx.StackTrace);
                AppProperties.IsException = true;
                if (connectionEx.InnerException != null)
                {
                    AppProperties.errorMessageFromBusiness = connectionEx.InnerException.Message;
                }
                else
                {
                    AppProperties.errorMessageFromBusiness = "Exception while fetching the Offline Data";
                }
                return null;
            }
        }


        #endregion
    }
}
