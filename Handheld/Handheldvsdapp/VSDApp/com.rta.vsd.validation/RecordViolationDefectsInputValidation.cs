using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSDApp.com.rta.vsd.validation
{
    class RecordViolationDefectsInputValidation: IValidation
    {
        public string Validate(System.Windows.Controls.UserControl form)
        {
            vsd.hh.ui.ucDefectAndViolationDetails validateForm = (vsd.hh.ui.ucDefectAndViolationDetails)form;
            string Valid = "";
            bool validity = true;
            if ((string)validateForm.cmboxType.Text == null || (string)validateForm.cmboxType.Text == "")
            {
                
                Valid += "Input Type not Selected" + "\n";
                validateForm.cmboxType.Focus();
                validity = false;
                // Valid += _Resource.GetString("Country not Selected") + "\n";
            }
            if ((string)validateForm.cmboxDefect.Text == null || (string)validateForm.cmboxDefect.Text == "")
            {
                
                Valid += "Defect not Selected" + "\n";
                if (validity)
                    validateForm.cmboxDefect.Focus();
                    validity = false;
                // Valid += _Resource.GetString("Country not Selected") + "\n";
            }
            if ((string)validateForm.cmboxDefectCategory.Text == null || (string)validateForm.cmboxDefectCategory.Text == "")
            {
                
                Valid += "Defect Category not Selected" + "\n";
                if (validity)
                    validateForm.cmboxDefectCategory.Focus();
                // Valid += _Resource.GetString("Country not Selected") + "\n";
                validity = false;
            }
            if ((string)validateForm.cmboxDefectSubCateogry.Text == null || (string)validateForm.cmboxDefectSubCateogry.Text == "")
            {               
                Valid += "Defect SubCateogory not Selected" + "\n";
                if (validity)
                    validateForm.cmboxDefectSubCateogry.Focus();
                // Valid += _Resource.GetString("Country not Selected") + "\n";
                validity = false;
            }
            if (validity)
                Valid = "Valid";
            return Valid;
        }
    }
}
