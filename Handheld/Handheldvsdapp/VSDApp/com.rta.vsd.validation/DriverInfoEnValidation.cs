using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using VSDApp.com.rta.vsd.hh.utilities;

namespace VSDApp.com.rta.vsd.validation
{
   public class DriverInfoEnValidation : IValidation
    {
       // private vsd.hh.localisation.Resources _Resource;

        public string Validate(UserControl form)
        {
          //  _Resource = VSDApp.com.rta.vsd.hh.localisation.Resources.GetInstance();
            vsd.hh.ui.ucDefectAndViolationDetails validateForm = (vsd.hh.ui.ucDefectAndViolationDetails)form;
            string Valid = "";
            bool validity = true;
            //implement Validation Checks here
            
            if (validateForm.txtDriverLiscenseNumber.Text == "")
            {
                validity = false;
                Valid += "Driver License Required" + "\n";
               // Valid += _Resource.GetString("Driver License not entered") + "\n";
                validateForm.txtDriverLiscenseNumber.Focus();
            }
            if ((string)validateForm.cmboxDriverCountry.Text == null || (string)validateForm.cmboxDriverCountry.Text == "")
            {                
                Valid += "Driver Nationality Not Selected" + "\n";
                // Valid += _Resource.GetString("Driver License not entered") + "\n";
                if (validity)
                    validateForm.cmboxDriverCountry.Focus();
                validity = false;
            }
            if ((string)validateForm.txtDriverNationality.Text == null || (string)validateForm.txtDriverNationality.Text == "")
            {
                Valid += "Driver Nationality Not Selected" + "\n";
                // Valid += _Resource.GetString("Driver License not entered") + "\n";
                if (validity)
                    validateForm.btnSearchLicNo.Focus();
                validity = false;
            }
            /*
            if (!AppProperties.Is_DriverDataVerified)
            {
                Valid += "Driver Data Not Verified " + "\n";
                // Valid += _Resource.GetString("Driver License not entered") + "\n";               
                validity = false;
            }*/
            if ((string)validateForm.cmboxEmirates.Text == null || (string)validateForm.cmboxEmirates.Text == "")
            {
                Valid += "Driver Emirates Not Selected" + "\n";
                // Valid += _Resource.GetString("Driver License not entered") + "\n";
                if (validity)
                    validateForm.cmboxDriverCountry.Focus();
                validity = false;
            }
            if (validity)
                Valid = "Valid";
            return Valid;
        }

        public string PartialValidate(UserControl form)
        {
            //  _Resource = VSDApp.com.rta.vsd.hh.localisation.Resources.GetInstance();
            vsd.hh.ui.ucDefectAndViolationDetails validateForm = (vsd.hh.ui.ucDefectAndViolationDetails)form;
            string Valid = "";
            bool validity = true;
            //implement Validation Checks here

            if (validateForm.txtDriverLiscenseNumber.Text == "")
            {
                validity = false;
                Valid += "Driver License Required" + "\n";
                // Valid += _Resource.GetString("Driver License not entered") + "\n";
                validateForm.txtDriverLiscenseNumber.Focus();
            }
            if (validity)
                Valid = "Valid";
            return Valid;
        }
    }


}