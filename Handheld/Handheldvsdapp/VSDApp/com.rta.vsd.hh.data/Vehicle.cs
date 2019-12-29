using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSDApp.com.rta.vsd.hh.data
{
    class Vehicle
    {
        private string _tradeLicenseNumber;
        private string _plateCode;
        private string _plateNumber;
        private string _emirate;
        private string _plateCategory;
        private string _make;
        private string _model;
        private string _year;
        private string _country;
        private string _chassisNumber;
        private CircularBlocks[] _circulars;
        private string _riskRating;
        private string _inspectionInstruction;
        private Violation[] _violations;
        private Operator _operator;
        private string _recomendation;
        private string _vehicleOVRRScore;
        private string _vehicleCategory;
        private string _vehicleCategoryAr;
        private DateTime _vehicleSuspensionDate;
        private string _plateSource;
        private string _riskRattingColor;
        private string _GroundingDays;
        private string _requiredGrounding;
        private string _blackPoints;
        private string _totalFineAmmount;
        private string _TotalImpoundingDays;
        private string _IsImpoundingGracePeriod;
        private string _mileage;
        private string _driverLicense;
        private string _driverName;
        private string _driverNameAr;
        private string _driverCountry;
        private string _driverEmirates;
        private string _driverRiskRattingScore;
        private string _driverRiskRattingColor;
        private string _driverRiskRattingName;
        private string _isElligibleForPocession;
        private string _testDueDate;
        private string _subCategory;
        private string _subCategoryAr;
        private VehicleDeviceInspParams _deviceInspectionparm;
        private AuthorizedDriver _vehiclDriver;
        private DateTime _RegExpiry;

        internal AuthorizedDriver VehiclDriver
        {
            get { return _vehiclDriver; }
            set { _vehiclDriver = value; }
        }

        internal VehicleDeviceInspParams DeviceInspectionparm
        {
            get { return _deviceInspectionparm; }
            set { _deviceInspectionparm = value; }
        }

        public string SubCategoryAr
        {
            get { return _subCategoryAr; }
            set { _subCategoryAr = value; }
        }

        public string SubCategory
        {
            get { return _subCategory; }
            set { _subCategory = value; }
        }

        public string TestDueDate
        {
            get { return _testDueDate; }
            set { _testDueDate = value; }
        }

        public string IsElligibleForPocession
        {
            get { return _isElligibleForPocession; }
            set { _isElligibleForPocession = value; }
        }

        public string DriverRiskRattingName
        {
            get { return _driverRiskRattingName; }
            set { _driverRiskRattingName = value; }
        }

        public string DriverRiskRattingColor
        {
            get { return _driverRiskRattingColor; }
            set { _driverRiskRattingColor = value; }
        }

        public string DriverRiskRattingScore
        {
            get { return _driverRiskRattingScore; }
            set { _driverRiskRattingScore = value; }
        }


        private bool _isHazard;
        public string DriverEmirates
        {
            get { return _driverEmirates; }
            set { _driverEmirates = value; }
        }

        public string DriverCountry
        {
            get { return _driverCountry; }
            set { _driverCountry = value; }
        }


        public string TotalFineAmmount
        {
            get { return _totalFineAmmount; }
            set { _totalFineAmmount = value; }
        }
        public string Mileage
        {
            get { return _mileage; }
            set { _mileage = value; }
        }

        public string IsImpoundingGracePeriod
        {
            get { return _IsImpoundingGracePeriod; }
            set { _IsImpoundingGracePeriod = value; }
        }



        public string TotalImpoundingDays
        {
            get { return _TotalImpoundingDays; }
            set { _TotalImpoundingDays = value; }
        }


        public string RiskRattingColor
        {
            get { return _riskRattingColor; }
            set { _riskRattingColor = value; }
        }
        public bool IsHazard
        {
            get { return _isHazard; }
            set { _isHazard = value; }
        }
        public string PlateSource
        {
            get { return _plateSource; }
            set { _plateSource = value; }
        }


        public class CircularBlocks
        {
            string _CircularNumber;
            string _CircularSource;
            DateTime _CircularDate;
            string _CircularDescription;


            public string CircularNumer
            {
                get { return (this._CircularNumber != null) ? this._CircularNumber : ""; }
                set { this._CircularNumber = value; }
            }
            public string CircularSource
            {
                get { return (this._CircularSource != null) ? this._CircularSource : ""; }
                set { this._CircularSource = value; }
            }
            public DateTime CircularDate
            {
                get { return this._CircularDate; }
                set { this._CircularDate = value; }
            }
            public string CircularDescription
            {
                get { return (this._CircularDescription != null) ? this._CircularDescription : ""; }
                set { this._CircularDescription = value; }
            }

        }



        public DateTime VehicleSuspensionDate
        {
            get { return this._vehicleSuspensionDate; }
            set { this._vehicleSuspensionDate = value; }
        }
        public string DriverName
        {
            get { return (this._driverName != null) ? this._driverName : ""; }
            set { this._driverName = value; }
        }
        public string DriverNameAr
        {
            get { return (this._driverNameAr != null) ? this._driverNameAr : ""; }
            set { this._driverNameAr = value; }
        }
        public string DriverLicense
        {
            get { return (this._driverLicense != null) ? this._driverLicense : ""; }
            set { this._driverLicense = value; }
        }

        public string VehicleCategory
        {
            get { return (this._vehicleCategory != null) ? this._vehicleCategory : ""; }
            set { this._vehicleCategory = value; }
        }
        public string VehicleCategoryAr
        {
            get { return (this._vehicleCategoryAr != null) ? this._vehicleCategoryAr : ""; }
            set { this._vehicleCategoryAr = value; }
        }
        public string VehicleOVRRScore
        {
            get { return (this._vehicleOVRRScore != null) ? this._vehicleOVRRScore : ""; }
            set { this._vehicleOVRRScore = value; }
        }
        public string Recomendation
        {
            get { return (this._recomendation != null) ? this._recomendation : ""; }
            set { this._recomendation = value; }
        }

        public string PlateCategory
        {
            get { return (this._plateCategory != null) ? this._plateCategory : ""; }
            set { this._plateCategory = value; }
        }
        public Violation[] Violations
        {
            get { return this._violations; }
            set { this._violations = value; }
        }
        public Operator Operator
        {
            get { return this._operator; }
            set { this._operator = value; }
        }
        public CircularBlocks[] Circulars
        {
            get { return this._circulars; }
            set { this._circulars = value; }
        }

        public string TradeLicenseNumber
        {
            get { return (this._tradeLicenseNumber != null) ? this._tradeLicenseNumber : ""; }
            set { this._tradeLicenseNumber = value; }
        }


        public string PlateCode
        {
            get { return (this._plateCode != null) ? this._plateCode : ""; }
            set { this._plateCode = value; }
        }

        public string PlateNumber
        {
            get { return (this._plateNumber != null) ? this._plateNumber : ""; }
            set { this._plateNumber = value; }
        }

        public string Emirate
        {
            get { return (this._emirate != null) ? this._emirate : ""; }
            set { this._emirate = value; }
        }

        public string Make
        {
            get { return (this._make != null) ? this._make : ""; }
            set { this._make = value; }
        }
        public string Model
        {
            // get { return (this._model != null) ? this._model : ""; }
            get { return (this._model != null) ? this._model : ""; }
            set { this._model = value; }
        }
        public string Year
        {
            get { return (this._year != null) ? this._year : ""; }
            set { this._year = value; }
        }
        public string Country
        {
            get { return (this._country != null) ? this._country : ""; }
            set { this._country = value; }
        }

        public string ChassisNumber
        {
            get { return (this._chassisNumber != null) ? this._chassisNumber : ""; }
            set { this._chassisNumber = value; }
        }
        public CircularBlocks[] VehicleCirculars
        {
            get { return this._circulars; }
            set { this._circulars = value; }
        }
        public string RiskRating
        {
            get { return (this._riskRating != null) ? this._riskRating : ""; }
            set { this._riskRating = value; }
        }
        public string Instruction
        {
            get { return _inspectionInstruction; }
            set { this._inspectionInstruction = value; }
        }

        public DateTime RegExpiry
        {
             get { return this._RegExpiry; }
            set { this._RegExpiry = value; }
        }

          
    }

    class VehicleDeviceInspParams
    {
        private string _carryWeight;
        private string _vehCategoryID;
        private string _vehLengthMeter;
        private string _vehWidthMeter;
        private string _vehHeightMeter;
        private string _emissionPercentage;
        private string _noOfAxel;


        public string NoOfAxel
        {
            get { return _noOfAxel; }
            set { _noOfAxel = value; }
        }

        public string EmissionPercentage
        {
            get { return _emissionPercentage; }
            set { _emissionPercentage = value; }
        }

        public string VehHeightMeter
        {
            get { return _vehHeightMeter; }
            set { _vehHeightMeter = value; }
        }

        public string VehWidthMeter
        {
            get { return _vehWidthMeter; }
            set { _vehWidthMeter = value; }
        }

        public string VehLengthMeter
        {
            get { return _vehLengthMeter; }
            set { _vehLengthMeter = value; }
        }

        public string VehCategoryID
        {
            get { return _vehCategoryID; }
            set { _vehCategoryID = value; }
        }

        public string CarryWeight
        {
            get { return _carryWeight; }
            set { _carryWeight = value; }
        }
    }

    class AuthorizedDriver
    {
        string _licNumber;
        string _issuingEmirates;
        string _issuingEmiratesAr;

        public string IssuingEmiratesAr
        {
            get { return _issuingEmiratesAr; }
            set { _issuingEmiratesAr = value; }
        }

        public string IssuingEmirates
        {
            get { return _issuingEmirates; }
            set { _issuingEmirates = value; }
        }

        public string LicNumber
        {
            get { return _licNumber; }
            set { _licNumber = value; }
        }
    }
}
