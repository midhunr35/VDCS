using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VSDApp.com.rta.vsd.hh.data;

namespace VSDApp.com.rta.vsd.hh.manager
{
    interface IVehicleProfile
    {
        Vehicle GetVehicleProfileDetails(string country, string emirate, string plateCategory, string plateNumber, string plateCode);
        Vehicle GetVehicleProfileDetailsfromEtraffic(string country, string emirate, string plateCategory, string plateNumber, string plateCode);
        string GetAlternateCategory(string category, string locale);
        string[] GetVehicleCategories();
        string[] GetVehiclePlateCategories(string emirate);
        string[] GetVehiclePlateCodes(string plateCategory, string emirate);
        string[] GetVehicleSubCategories(string vehicleCategory);
        string GetVehicleSubCategoryName(string subcategory, string vehicleCategory);
        string GetAlternateVehicleSubCategory(string subcategory, string category);

    }

}
