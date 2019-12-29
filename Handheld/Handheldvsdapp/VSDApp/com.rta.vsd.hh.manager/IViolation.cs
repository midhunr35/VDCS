using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSDApp.com.rta.vsd.hh.manager
{
    interface IViolation
    {

        string CalculateSeverity(string previousSeverity, string newSeverity, string locale);
        string[] GetConfigurationDataForSeverity(string severity, int defectsLength);
        bool StoreOfflineViolation();
        string[] GetLocation(string name, string type);
        string[] GetDefects(string defect, string category, string defecttype);
        string[] GetDefectSeverity(string defectName, string defectSubCategory, string defectMainCat, string value);
        bool SubmitViolation();
        bool SubmitOfflineViolation();

    }
}
