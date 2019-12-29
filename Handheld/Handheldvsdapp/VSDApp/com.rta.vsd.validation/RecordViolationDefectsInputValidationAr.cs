using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSDApp.com.rta.vsd.validation
{
    class RecordViolationDefectsInputValidationAr : IValidation
    {
        #region IValidation Members

        public string Validate(System.Windows.Controls.UserControl form)
        {
            vsd.hh.ui.ucDefectAndViolationDetails validateForm = (vsd.hh.ui.ucDefectAndViolationDetails)form;
            string Valid = "";
            bool validity = true;
            if ((string)validateForm.cmboxType.Text == null || (string)validateForm.cmboxType.Text == "")
            {                
                Valid += "نوع الإدخال غير المقيد" + "\n";
                if (validity)
                    validateForm.cmboxType.Focus();
                // Valid += _Resource.GetString("Country not Selected Input Type Not Entered") + "\n";
                validity = false;
            }
            if ((string)validateForm.cmboxDefect.Text == null || (string)validateForm.cmboxDefect.Text == "")
            {               
                Valid += "يرجى اختيار الاعطال" + "\n";
                if (validity)
                    validateForm.cmboxDefect.Focus();
                // Valid += _Resource.GetString("Defect not Selected") + "\n";
                validity = false;
            }
            if ((string)validateForm.cmboxDefectCategory.Text == null || (string)validateForm.cmboxDefectCategory.Text == "")
            {                
                Valid += "يرجى اختيار صنف الاعطال" + "\n";
                if (validity)
                    validateForm.cmboxDefectCategory.Focus();
                // Valid += _Resource.GetString("Defect Category not Selected") + "\n";
                validity = false;
            }
            if ((string)validateForm.cmboxDefectSubCateogry.Text == null || (string)validateForm.cmboxDefectSubCateogry.Text == "")
            {               
                Valid += "يرجى اختيار فئة الاعطال" + "\n";
                if (validity)
                    validateForm.cmboxDefectSubCateogry.Focus();
                validity = false;
                // Valid += _Resource.GetString("Defect SubCateogory not Selected") + "\n";
            }
            if (validity)
                Valid = "Valid";
            return Valid;
        }

        #endregion
    }
}
