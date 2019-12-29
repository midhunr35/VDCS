using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VSDApp.com.rta.vsd.hh.data;

namespace VSDApp.com.rta.vsd.hh.utilities
{
    class AppProperties
    {
        // public static System.Drawing.Font fontRegular = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular);
        // public static System.Drawing.Font fontMedium = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Regular);
        // public static System.Drawing.Font fontLarge = new System.Drawing.Font("Tahoma", 20F, System.Drawing.FontStyle.Regular);
        // public Schweers.Localization.ArabicConverter arab = new Schweers.Localization.ArabicConverter();
        //public static int height = 12;
        //public static int width;
        public static bool isOnline = true;
        public static bool isOfflineDataPrint = false;
        public static bool isUserLoggedIn = false;

        public static string applicationPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);

        public static string _filePathforInspectorImage = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase) + "\\Inspectors_Images";
        public static string InspectorImagesPath = _filePathforInspectorImage.Substring(6);

        private static string _fileName = applicationPath + "/VSDAppDB.sdf";
        public static string connectionStringInitial = string.Format("DataSource=\"{0}\"", _fileName);
        public static string connectionString = connectionStringInitial.Replace("file:\\", "");




        public static string logFilePath = applicationPath + "/VSDLog.txt";
        // public static string connectionString

        //private static string _password = "login12!";
        public static byte[] iVector = new byte[] { 69, 110, 99, 34, 42, 14, 77, 69, 69, 110, 99, 34, 42, 14, 77, 69 };
        public static byte[] encryptionKey = new byte[] { 54, 13, 110, 42, 35, 17, 98, 110, 12, 34, 67, 43, 81, 54, 120, 56, 54, 13, 110, 42, 35, 17, 98, 110, 12, 34, 67, 43, 81, 54, 120, 56 };

        //public static string connectionString = string.Format("DataSource=\"{0}\";Encryption Mode = PPC2003 Compatibility", _fileName);
        // public static string connectionString = string.Format("DataSource=\"{0}\"", _fileName);

        public static int xCordinate = 5;
        public static int yCordinate = 20;

        public static string defaultCountry = "United Arab Emirates";
        public static string defaultEmirate = "Dubai";
        public static string defaultAreaEn = "Bur Dubai";
        public static string defaultCountryAr = "الإمارات العربية المتحدة";
        public static string defaultEmirateAr = "دبي";
       // public static string defaultAreaAr = "منطقة ديرة";
        public static string defaultAreaAr = "بردبي";


        public static string defaultVehicleCategoryEn = "Heavy Vehicle";
        public static string defaultVehicleCategoryAr = "مركبة ثقيلة";
        public static string defaultPlateCategoryEn = "Public Transportation";
        public static string defaultPlateCategoryAr = "نقل عام";

        public static string AdminPassword = "P@ssw0rd";
        public static bool Is_CloseMainApp = false;
        public static bool Is_ClosePermissionWindow = false;

        //        public static string userCode = Schweers.Sys.Device.SerialNumber;

        public static int increments = 20;
        public static int topMargin = 3;

        public static int leftMargin = 5;
        public static int rightMargin = 5;

        public static int xCordinateAR = 315;
        public static int padding = 5;

        public static bool canRecordDefects = true;
        public static int defaultControlWidth = (int)(320 * .35);

        public static Vehicle vehicle;
        public static User LoggedInUser = new User();

        //public static Operator _Operator;
        //public static Violation[] _OpenViolations;

        public static bool routeFromRecordViolation = false;
        public static bool routeFromDefect = false;
        public static bool routeFromConfiscation = false;

        public static bool isSafety = true;
        public static bool isViolation = false;

        public static bool isFlowFromOperator = false;

        // public static Hashtable _DefectsList = new Hashtable();

        public static int selectedViolation = -1;

        public static Violation recordedViolation;
        public static CarRentalAgency[] searchedCarRentalResult;
        public static CarRentalAgencyInspectionInfo searchedRentalCarInspectionInfo;
        public static string receiptTitle;
        public static string receiptTitleAr;
        public static bool confiscatePlates = false;

        public static string empPassword = "tibco01";
        public static string empUserName = "tibcouser";
        public static string empUserFullName = "";
        public static string empUserFullNameAr = "";

        public static string PortraitMode = "Portrait";
        public static string LandScapetMode = "LandScape";
        //Single Control Screen
        public static double LndScp_220_350 = 350;
        public static double LndScp_Btn_100_140 = 140;
        public static double LndScp_Btn_65_130 = 130;
        public static double LndScp_Btn_75_140 = 140;
        public static double LndScp_Lbl_20_25 = 25;
        public static double LndScp_Header_25_30 = 30;

        public static double Prtrt_350_220 = 220;
        public static double Prtrt_Btn_140_100 = 100;
        public static double Prtrt_Btn_130_65 = 65;
        public static double Prtrt_Btn_140_75 = 75;
        public static double Prtrt_Lbl_25_20 = 20;
        public static double Prtrt_Header_30_25 = 25;

        //Multiple Control Screen
        public static double LndScp_130_280 = 280;
        public static double LndScp_Btn_130_280 = 280;

        public static double Prtrt_280_130 = 150;
        public static double Prtrt_Btn__280_130 = 130;

        //Multipl Controls Defect Screen
        public static double LndScp_130_300 = 300;
        public static double LndScp_140_260 = 260;
        public static double LndScp_Btn_130_300 = 300;

        public static double Prtrt_300_130 = 140;
        public static double Prtrt_260_140 = 140;
        public static double Prtrt_Btn__300_130 = 140;

        ///
        public static double LndScp_Btn_100_150 = 150;
        public static double Prtrt_Btn_150_100 = 100;



        //public static string userCode = "01";

        public static string empID;
        public static string errorMessageFromBusiness;
        public static bool businessError = false;
        public static bool NotFoundError = false;
        public static bool IsStoreOfflineData = false;
        public static bool IsException = false;
        public static bool IsServiceResponseNull = false;
        public static string exceptionFromServiceCall;
        public static string authorityCode = "01";
        public static string deviceCode = "01";

        public static Violation.InspectionLocation location;
        public static string rvDefectCodes = "('-2000')";

        public static bool canFetchInfo = false;
        public static bool canRaiseViolation = false;
        public static bool canInspect = false;
        public static bool canPrintViolation = false;
        public static bool canConfiscatePlates = false;

        public static bool isComprehensive = false;
        public static bool isInspectionUploaded = false;
        public static List<string[]> selectedDefectsEn = new List<string[]>();
        public static List<int> previousDefectIDs = new List<int>();

        public static List<InterestedVehicle> InterestListVehicle = new List<InterestedVehicle>();

        public static string Selected_Resource = String.Empty;
        //public static List<string[]> selectedDefectsAr = new List<string[]>();
        public static string Previous_Selected_LocationEn = string.Empty;
        public static string Previous_Selected_AreaEn = string.Empty;
        public static string Previous_Selected_LocationAr = string.Empty;
        public static string Previous_Selected_AreaAr = string.Empty;
        public static int Selected_Location_Count = 0;
        public static string Current_Selected_LocationEn = string.Empty;
        public static string Current_Selected_LocationAr = string.Empty;

        public static int Total_Vehicle_Inspected = 0;

        public static bool Is_SubmitVilation = false;

        public static bool Is_HHLocationSubmition = false;
        public static bool Is_NewLogin = true;

        public static bool Is_DriverDataVerified = false;

        public static bool Is_DeviceInspection = false;

        public string Default_UserName = "VDCS_STGADMIN01";
        public string Default_Password = "password@123";


        public static string Defect_Length_Code = "9.3.6";
        public static string Defect_Width_Code = "9.3.6";
        public static string Defect_Height_Code = "9.3.6";
        public static string Defect_Emission_Code = "7.3.2";

        public static string default_no_axels = "16";
        public static string Defect_GrossWeight_Code = "9.3.5";
        public static string Selected_ALPR_Name = string.Empty;
        public static bool isNetworkConnected = false;
    }
}
