using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSDApp.com.rta.vsd.hh.data
{
    public class CarRentalAgency
    {
        private string _agencyNameEn;
        private string _agencyNameAr;
        private string _tradeLicNumber;
        private string _tradeLicExpiry;
        private string _trafficeFileNo;
        private string _lastInspectionDate;
        private string _inspectionRecomendationEn;
        private string _inspectionRecomendationAr;
        private string _class;
        private string _areaEn;
        private string _areaAr;
        private string _locationEn;
        private string _locationAr;
        private string _lastinspectionDate;
        private string _agencyID;
        private string _selectedAgencyID;
        private string _agencyMobileNo;
        private string _agencyPhoneNo;
        private string _agencyAddress;
        private string _agencyEmail;

        public string AgencyEmail
        {
            get { return _agencyEmail; }
            set { _agencyEmail = value; }
        }

        public string AgencyAddress
        {
            get { return _agencyAddress; }
            set { _agencyAddress = value; }
        }

        public string AgencyPhoneNo
        {
            get { return _agencyPhoneNo; }
            set { _agencyPhoneNo = value; }
        }

        public string AgencyMobileNo
        {
            get { return _agencyMobileNo; }
            set { _agencyMobileNo = value; }
        }

        public string SelectedAgencyID
        {
            get { return _selectedAgencyID; }
            set { _selectedAgencyID = value; }
        }


        public string AgencyID
        {
            get { return _agencyID; }
            set { _agencyID = value; }
        }

        public string LastinspectionDate
        {
            get { return _lastinspectionDate; }
            set { _lastinspectionDate = value; }
        }

        public string LocationAr
        {
            get { return _locationAr; }
            set { _locationAr = value; }
        }

        public string LocationEn
        {
            get { return _locationEn; }
            set { _locationEn = value; }
        }

        public string AreaAr
        {
            get { return _areaAr; }
            set { _areaAr = value; }
        }

        public string AreaEn
        {
            get { return _areaEn; }
            set { _areaEn = value; }
        }

        public string _class1
        {
            get { return _class; }
            set { _class = value; }
        }

        public string InspectionRecomendationAr
        {
            get { return _inspectionRecomendationAr; }
            set { _inspectionRecomendationAr = value; }
        }

        public string InspectionRecomendationEn
        {
            get { return _inspectionRecomendationEn; }
            set { _inspectionRecomendationEn = value; }
        }

        public string TrafficeFileNo
        {
            get { return _trafficeFileNo; }
            set { _trafficeFileNo = value; }
        }

        public string TradeLicExpiry
        {
            get { return _tradeLicExpiry; }
            set { _tradeLicExpiry = value; }
        }

        public string TradeLicNumber
        {
            get { return _tradeLicNumber; }
            set { _tradeLicNumber = value; }
        }

        public string AgencyNameAr
        {
            get { return _agencyNameAr; }
            set { _agencyNameAr = value; }
        }

        public string AgencyNameEn
        {
            get { return _agencyNameEn; }
            set { _agencyNameEn = value; }
        }
    }

    public class CarRentalAgencyInspectionInfo
    {
        private CarRentalAgency _agencyInformation;

        private List<OpenOffenses> _agencyListOfOpenOffense;

        public List<OpenOffenses> AgencyListOfOpenOffense
        {
            get { return _agencyListOfOpenOffense; }
            set { _agencyListOfOpenOffense = value; }
        }

        public CarRentalAgency AgencyInformation
        {
            get { return _agencyInformation; }
            set { _agencyInformation = value; }
        }
    }

    public class OpenOffenses
    {
        private string _categoryEn;
        private string _categoryAr;
        private string _offenseEn;
        private string _offenseAr;
        private string _issueDate;
        private string _closeOffense;
        private string _issueInfraction;
        private string _channelDefectID;
        private string _offenseID;

        public string OffenseID
        {
            get { return _offenseID; }
            set { _offenseID = value; }
        }

        public string ChannelDefectID
        {
            get { return _channelDefectID; }
            set { _channelDefectID = value; }
        }

        public string IssueInfraction
        {
            get { return _issueInfraction; }
            set { _issueInfraction = value; }
        }

        public string CloseOffense
        {
            get { return _closeOffense; }
            set { _closeOffense = value; }
        }

        public string IssueDate
        {
            get { return _issueDate; }
            set { _issueDate = value; }
        }

        public string OffenseAr
        {
            get { return _offenseAr; }
            set { _offenseAr = value; }
        }

        public string OffenseEn
        {
            get { return _offenseEn; }
            set { _offenseEn = value; }
        }

        public string CategoryAr
        {
            get { return _categoryAr; }
            set { _categoryAr = value; }
        }

        public string CategoryEn
        {
            get { return _categoryEn; }
            set { _categoryEn = value; }
        }


    }
}
