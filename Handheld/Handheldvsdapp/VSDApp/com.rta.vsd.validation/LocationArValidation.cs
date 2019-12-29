using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace VSDApp.com.rta.vsd.validation
{
    class LocationArValidation : IValidation
    {
        #region IValidation Members

        public string Validate(UserControl form)
        {
            //  _Resource = VSDApp.com.rta.vsd.hh.localisation.Resources.GetInstance();
            vsd.hh.ui.ucLocationSelectionEn validateForm = (vsd.hh.ui.ucLocationSelectionEn)form;
            string Valid = "";
            bool validity = true;
            //implement Validation Checks here
            if ((string)validateForm.cmboxEmirates.Text == null || (string)validateForm.cmboxEmirates.Text == "")
            {
                // Valid += _Resource.GetString("City not selected") + "\n";
                Valid += "يرجى اختيار الإمارة" + "\n";
                validateForm.cmboxEmirates.Focus();
                validity = false;
            }
            if ((string)validateForm.cmboxArea.Text == null || (string)validateForm.cmboxArea.Text == "")
            {
                //Valid += _Resource.GetString("Area not selected") + "\n";
                Valid += "يرجى اختيار المنطقة" + "\n";
                if (validity)
                    validateForm.cmboxArea.Focus();
                validity = false;
            }
            if ((string)validateForm.cmboxLocation.Text == null || (string)validateForm.cmboxLocation.Text == "")
            {
                // Valid += _Resource.GetString("Location not selected") + "\n";
                Valid += "يرجى اختيار الموقع" + "\n";
                if (validity)
                    validateForm.cmboxLocation.Focus();
                validity = false;
            }
            if (validity)
            {
                // Valid = _Resource.GetString("Valid");
                Valid = "Valid";
            }
            return Valid;
        }

        #endregion
    }
}
