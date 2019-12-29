using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VSDApp.com.rta.vsd.hh.utilities;

namespace VSDApp.com.rta.vsd.hh.data
{
    public class Violation
    {
        private string _violationID;

        public struct InspectionLocation
        {
            public string city;
            public string area;
            public string location;
        }
        private InspectionLocation _location;


        private string _inspector;
        private string _violationSource;
        private string _violationStatus;
        private string _violationTestType;
        private string _violationFeeAmount;
        private DateTime _violationDueDays;
        private string _violationComments;
        private string _violationCommentsAr;
        private string _violationTicketCode;
        private bool _isConfiscated;
        private string _confiscationReason;
        private string _confiscationReasonAr;
        private string _plateCondition;
        private string _inspection_location;
        private string _inspection_locationAr;
        private string _rtaEmpID;
        private string _driverLicNo;
        private string _plateNumber;
        private string _plateCode;
        private string _plateCategory;
        private string _vehicleCategory;
        private string _gracePeriod;




        public string GracePeriod
        {
            get { return _gracePeriod; }
            set { _gracePeriod = value; }
        }


        public string VehicleCategory
        {
            get { return _vehicleCategory; }
            set { _vehicleCategory = value; }
        }


        public string PlateCategory
        {
            get { return _plateCategory; }
            set { _plateCategory = value; }
        }

        public string PlateCode
        {
            get { return _plateCode; }
            set { _plateCode = value; }
        }

        public string PlateNumber
        {
            get { return _plateNumber; }
            set { _plateNumber = value; }
        }


        public string DriverLicNo
        {
            get { return _driverLicNo; }
            set { _driverLicNo = value; }
        }

        public string RtaEmpID
        {
            get { return _rtaEmpID; }
            set { _rtaEmpID = value; }
        }

        public string Inspection_locationAr
        {
            get { return _inspection_locationAr; }
            set { _inspection_locationAr = value; }
        }


        public string Inspection_location
        {
            get { return _inspection_location; }
            set { _inspection_location = value; }
        }


        public class DefectFines
        {
            string _defect_ID;
            string _fineName;

            string _fineAmmount;

            public string FineAmmount
            {
                get { return _fineAmmount; }
                set { _fineAmmount = value; }
            }



            public string FineName
            {
                get { return _fineName; }
                set { _fineName = value; }
            }

            public string Defect_ID
            {
                get { return _defect_ID; }
                set { _defect_ID = value; }
            }
        }
        public class Defects
        {
            private int _defectID;
            private string _defectName;
            private string _defectSeverity;
            private string _defectType;
            private string _defectValue;
            private string _defectCategory;
            private string _defectSubCat;

            private string _defectCode;
            private string _defecNameAr;
            private string _defectSeverityAr;
            private string _ImpoundingDays;
            private string _blackPoints;
            private string _FineAmount;
            private string _FineName;
            private string _FineNameAr;
            private string _fineID;
            private string _enforceTesting;
            private string _enforceFine;

            public string EnforceFine
            {
                get { return _enforceFine; }
                set { _enforceFine = value; }
            }

            public string EnforceTesting
            {
                get { return _enforceTesting; }
                set { _enforceTesting = value; }
            }

            public string FineID
            {
                get { return _fineID; }
                set { _fineID = value; }
            }

            public string FineNameAr
            {
                get { return _FineNameAr; }
                set { _FineNameAr = value; }
            }

            public string FineName
            {
                get { return _FineName; }
                set { _FineName = value; }
            }

            public string FineAmount
            {
                get { return _FineAmount; }
                set { _FineAmount = value; }
            }

            public string ImpoundingDays
            {
                get { return _ImpoundingDays; }
                set { _ImpoundingDays = value; }
            }


            public string BlackPoints
            {
                get { return _blackPoints; }
                set { _blackPoints = value; }
            }

            public string DefectSeverityAr
            {
                get { return _defectSeverityAr; }
                set { _defectSeverityAr = value; }
            }

            public string DefectNameAr
            {
                get { return _defecNameAr; }
                set { _defecNameAr = value; }
            }

            public string DefectCode
            {
                get { return _defectCode; }
                set { _defectCode = value; }
            }

            public string DefectSubCat
            {
                get { return _defectSubCat; }
                set { _defectSubCat = value; }
            }

            public string DefectCategory
            {
                get { return _defectCategory; }
                set { _defectCategory = value; }
            }

            public int DefectID
            {
                get { return this._defectID; }
                set { this._defectID = value; }
            }

            public string DefectValue
            {
                get { return this._defectValue; }
                set { this._defectValue = value; }
            }

            public string DefectName
            {
                get { return this._defectName; }
                set { this._defectName = value; }
            }

            public string DefectSeverity
            {
                get { return this._defectSeverity; }
                set { this._defectSeverity = value; }
            }

            public string DefectType
            {
                get { return this._defectType; }
                set { this._defectType = value; }
            }

        }
        private Defects[] _Defects;


        private DateTime _violationIssueDate;
        private string _violationSeverity;
        private string _violationSeverityAr;


        public bool IsConfiscated
        {
            get { return this._isConfiscated; }
            set { this._isConfiscated = value; }
        }
        public string ConfiscationReason
        {
            get { return this._confiscationReason; }
            set { this._confiscationReason = value; }
        }
        public string ConfiscationReasonAr
        {
            get { return this._confiscationReasonAr; }
            set { this._confiscationReasonAr = value; }
        }
        public string PlateCondition
        {
            get { return this._plateCondition; }
            set { this._plateCondition = value; }
        }
        public string ViolationTicketCode
        {
            get { return this._violationTicketCode; }
            set { this._violationTicketCode = value; }
        }

        public string ViolationComments
        {
            get { return this._violationComments; }
            set { this._violationComments = value; }
        }
        public string ViolationCommentsAr
        {
            get { return this._violationCommentsAr; }
            set { this._violationCommentsAr = value; }
        }
        public DateTime ViolationDueDays
        {
            get { return this._violationDueDays; }
            set { this._violationDueDays = value; }
        }
        public string ViolationTestType
        {
            get { return this._violationTestType; }
            set { this._violationTestType = value; }
        }
        public string ViolationFee
        {
            get { return this._violationFeeAmount; }
            set { this._violationFeeAmount = value; }
        }
        public string ViolationID
        {
            get { return this._violationID; }
            set { this._violationID = value; }
        }

        public string Inspector
        {
            get { return this._inspector; }
            set { this._inspector = value; }
        }
        public string ViolationSource
        {
            get { return this._violationSource; }
            set { this._violationSource = value; }
        }
        public string ViolationStatus
        {
            get { return this._violationStatus; }
            set { this._violationStatus = value; }
        }
        public string ViolationSeverity
        {
            get { return this._violationSeverity; }
            set { this._violationSeverity = value; }
        }
        public string ViolationSeverityAr
        {
            get { return this._violationSeverityAr; }
            set { this._violationSeverityAr = value; }
        }
        public DateTime ViolationIssueDate
        {
            get { return this._violationIssueDate; }
            set { this._violationIssueDate = value; }
        }

        public InspectionLocation InspectionArea
        {
            get { return this._location; }
            set { this._location = value; }
        }
        public Defects[] Defect
        {
            get { return this._Defects; }
            set { this._Defects = value; }
        }

    }


    public class TrafficeFines
    {
        private String descriptionEn;
        private String fineAmmount;
        private String descriptionAr;
        private String blackPoints;
        private String vehicleConfiscationPeriod;
        private String type;
        private String typeAr;
        private String vehicleConfiscationPeriodAr;
        private String blackPointsAr;
        private String fineAmmountsAr;

        public String FineAmmountsAr
        {
            get { return fineAmmountsAr; }
            set { fineAmmountsAr = value; }
        }

        public String BlackPointsAr
        {
            get { return blackPointsAr; }
            set { blackPointsAr = value; }
        }

        public String VehicleConfiscationPeriodAr
        {
            get { return vehicleConfiscationPeriodAr; }
            set { vehicleConfiscationPeriodAr = value; }
        }

        public String TypeAr
        {
            get { return typeAr; }
            set { typeAr = value; }
        }

        public String Type
        {
            get { return type; }
            set { type = value; }
        }

        public String VehicleConfiscationPeriod
        {
            get { return vehicleConfiscationPeriod; }
            set { vehicleConfiscationPeriod = value; }
        }

        public String BlackPoints
        {
            get { return blackPoints; }
            set { blackPoints = value; }
        }



        public String DescriptionAr
        {
            get { return descriptionAr; }
            set { descriptionAr = value; }
        }

        public String FineAmmount
        {
            get { return fineAmmount; }
            set { fineAmmount = value; }
        }

        public String DescriptionEn
        {
            get { return descriptionEn; }
            set { descriptionEn = value; }
        }
    }


    public class ViolationTicket
    {
        private String _violationID;
        private String _violationAdvice;
        private String _dateTime;
        private String _location;
        private String _locaitonAr;
        private String _vehicleDetials;
        private String _driverLicNo;
        private String _rtaEmpNo; private String _vehicleDetailsAr;
        private VSDApp.com.rta.vsd.hh.data.Violation.Defects[] _defectDetails;
        private String _violationAdviceAr;
        private string _plateNumber;
        private string _plateCategory;
        private string _plateCode;
        private string _vehicleCategory;
        private string _VRR;
        private string _DRR;
        private string _graePeriod;
        private string _dueDate;

        public string DueDate
        {
            get { return _dueDate; }
            set { _dueDate = value; }
        }

        public string GraePeriod
        {
            get { return _graePeriod; }
            set { _graePeriod = value; }
        }

        public string DRR
        {
            get { return _DRR; }
            set { _DRR = value; }
        }

        public string VRR
        {
            get { return _VRR; }
            set { _VRR = value; }
        }

        public string VehicleCategory
        {
            get { return _vehicleCategory; }
            set { _vehicleCategory = value; }
        }

        public string PlateCode
        {
            get { return _plateCode; }
            set { _plateCode = value; }
        }

        public string PlateCategory
        {
            get { return _plateCategory; }
            set { _plateCategory = value; }
        }

        public string PlateNumber
        {
            get { return _plateNumber; }
            set { _plateNumber = value; }
        }

        public String ViolationAdviceAr
        {
            get { return _violationAdviceAr; }
            set { _violationAdviceAr = value; }
        }

        public String RtaEmpNo
        {
            get { return _rtaEmpNo; }
            set { _rtaEmpNo = value; }
        }

        public String DriverLicNo
        {
            get { return _driverLicNo; }
            set { _driverLicNo = value; }
        }


        public VSDApp.com.rta.vsd.hh.data.Violation.Defects[] DefectDetails
        {
            get { return _defectDetails; }
            set { _defectDetails = value; }
        }

        public String VehicleDetailsAr
        {
            get { return _vehicleDetailsAr; }
            set { _vehicleDetailsAr = value; }
        }

        public String VehicleDetials
        {
            get { return _vehicleDetials; }
            set { _vehicleDetials = value; }
        }

        public String LocaitonAr
        {
            get { return _locaitonAr; }
            set { _locaitonAr = value; }
        }

        public String Location
        {
            get { return _location; }
            set { _location = value; }
        }

        public String DateTime
        {
            get { return _dateTime; }
            set { _dateTime = value; }
        }

        public String ViolationAdvice
        {
            get { return _violationAdvice; }
            set { _violationAdvice = value; }
        }

        public String ViolationID
        {
            get { return _violationID; }
            set { _violationID = value; }
        }
    }

    public class InterestedVehicle
    {
        private string _chassisNo;

        public string ChassisNo
        {
            get { return _chassisNo; }
            set { _chassisNo = value; }
        }
    }

    public class HandHeldGPSLocation
    {
        private string _Latitude;
        private string _Longitude;

        public string Longitude
        {
            get { return _Longitude; }
            set { _Longitude = value; }
        }

        public string Latitude
        {
            get { return _Latitude; }
            set { _Latitude = value; }
        }
    }

    public class DeviceInspectionDefects
    {
        private string defectID;
        private string defectCode;
        private string defectName;
        private string fineName;
        private string fineNameAr;
        private string fineAmmount;
        private string actualValue;

        public string ActualValue
        {
            get { return actualValue; }
            set { actualValue = value; }
        }

        public string FineAmmount
        {
            get { return fineAmmount; }
            set { fineAmmount = value; }
        }

        public string FineNameAr
        {
            get { return fineNameAr; }
            set { fineNameAr = value; }
        }

        public string FineName
        {
            get { return fineName; }
            set { fineName = value; }
        }

        public string DefectName
        {
            get { return defectName; }
            set { defectName = value; }
        }

        public string DefectCode
        {
            get { return defectCode; }
            set { defectCode = value; }
        }

        public string DefectID
        {
            get { return defectID; }
            set { defectID = value; }
        }
    }


}
