using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace VSDApp.com.rta.vsd.hh.db
{
   public interface IDBDataLoad
    {
        string[] GetLocation(string location);
        string[] GetCities(string city);
        string[] GetAreas(string area);
        string[] GetCountries();
        string[] GetAllCountriesForNationality();
        string[] GetACountriesForNationalityDriver();
        string[] GetPlateCategories(string emirate);
        Hashtable GetPlateDetailsInArabic(string plateCat, string plateCode);
        string[] GetPlateCodes(string plateCategory, string emirate);
        string[] GetVehicleCategories();
        string[] GetVehicleSubCategories(string vehicleCategory);
        string GetVehicleCategoryName(string vehicleCategoryName_A, string vehicleCategory);

        string[] GetDefects(string defect, string category, string recordType);
        string[] GetOffenceCategories(string type,string recordType);
        DataTable GetOffence(string OffenceCategory, string recordType);
        string GetAlternateVehicleCategory(string category, string locale);
        string GetAlternateVehicleSubCategory(string subcategory, string category);
        string GetVehicleCategoryCode(string vehCat);
        string GetVehicleSubCategoryCode(string subcategory, string category);       
        string GetPlateSourceCode(string plateSource);
        string GetPlateSourceCodeAr(string plateSource);
        string GetPlateEmirate(string plateSource);
        List<Object> GetStoredInspection();
        List<Object> GetStoredInspectionByTicketNumber(string ticektNumber);
        string GetLocationCode(int id);
        string[] GetVehicleInfo(int id);
        string GetVehicleCategoryCode(int id);
        string[] GetOwnerInfo(int vehInfoID);
        string[] GetDriverInfo(int vehInfoID);
        string[] GetViolation(int inspectionID);
        string[] GetViolationByTicketCode(string ticeketCode);
        string[][] GetDefectCodesForViolation(int violationID);
        string GetLocationCode(string emirate, string area, string location);
        string GetDefectCode(int defectID);
        string GetPlateEmirateByCode(string plateSource);
        string GetArabicLocationNameFromEn(string locationName);
        string GetSeverityByID(int sevID, string type);
        string[] GetSeverityNamesByID(int sevID);
        string[] GetDefectSeverityByDefectIDVehCat(int defectID, int vehicleCatID);
        int GetDefectIDByCode(string defectCode);
        string GetAlternateCity(string name);
        DateTime GetAppSynTime();
        void UpdateApplicationSynTime();
        string[][] GetTestType(string severity, int defectsNo);
        void SetDefaultConfiguration();
        string GetVehicleCategoryNameByCode(string code);
        string[] GetSafetyDefects();
        bool CanConfiscatePlate(string severity);
        string[][] GetDefectMainCategory();
        string[][] GetDefectMainCategoryByID(string defectid);
        int GetDefectCountPerCategory(string defectCat, string defectCodes);
        string GetRVDefectCodes();
        bool SetAuthCode(string authCode);
        string GetAuthCode();
        void GetCountryProperties(string emirate);
        string GetDefectNameByID(string id);
        string GetDefectNameArByID(string id);        
        string GetCountryTicketCode(string country);
        string GetDeviceCode();
        string[] GetInterestListVehicle();
        string[] GetImpoundingDays(string defectCode, string vehCat);
        string[] GetDefectsFineInfo(string defectCode, string vehCategory);
        DataTable GetOfflineViolationAllByTicketCode(string ticeketCode);
        DataTable GetCommentsByDefectID(string defect, string vehCat);
        DataTable GetDefectPropertyByID(string defectID);
        DataTable GetDefectSubAndNameFDefectCode(string defectCode);
        DataTable GetDefectCategoryFromDefectCode(string defectCode);
        DataTable LoadFuelEmissionType();
        DataTable LoadDeviceInspSubVat();

        DataTable GetDeviceInspParemtByVehCat(string vehicleSubCat);
        string GetVehCatIDFromVehCatName(string categoryName);
    }
}
