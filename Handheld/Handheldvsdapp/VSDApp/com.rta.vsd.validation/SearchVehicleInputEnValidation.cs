using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VSDApp.com.rta.vsd.hh.utilities;

namespace VSDApp.com.rta.vsd.validation
{
    class SearchVehicleInputEnValidation : IValidation
    {
       

        public string Validate(System.Windows.Controls.UserControl form)
        {
            // _Resource = VSDApp.com.rta.vsd.hh.localisation.Resources.GetInstance();
            vsd.hh.ui.ucSearchVehicle validateForm = (vsd.hh.ui.ucSearchVehicle)form;
            string Valid = "";
            bool validity = true;
            //implement Validation Checks here
            //System.Windows.Forms.MessageBox.Show((string)validateForm._SelectCountry.Text);
            if ((string)validateForm.cmboxCountry.Text == null || (string)validateForm.cmboxCountry.Text == "")
            {
                
                Valid += "Country not Selected" + "\n";
                if (validity)
                    validateForm.cmboxCountry.Focus();
                validity = false;
                // Valid += _Resource.GetString("Country not Selected") + "\n";
            }
            if (validateForm.cmboxEmirates.SelectedValue != null)
            {
                if (((string)validateForm.cmboxEmirates.Text == null || (string)validateForm.cmboxEmirates.Text == ""))
                {
                   
                    Valid += "Emirate not Selected" + "\n";
                    if (validity)
                        validateForm.cmboxEmirates.Focus();
                    validity = false;
                    // Valid += _Resource.GetString("Emirate not Selected") + "\n";
                }
            }
            else
            {
                validity = false;
                Valid += "Emirate not Selected" + "\n";
            }
            if (validateForm.cmboxPlateCategory.SelectedValue != null)
            {
                if ((string)validateForm.cmboxPlateCategory.SelectedValue.ToString() == null || (string)validateForm.cmboxPlateCategory.SelectedValue.ToString() == "")
                {
                    validity = false;
                    Valid += "Plate Category not Selected" + "\n";
                    if (validity)
                        validateForm.cmboxPlateCategory.Focus();
                    
                    //   Valid += _Resource.GetString("Plate Category not Selected") + "\n";
                }
            }
            else
            {
                
                Valid += "Plate Category not Selected" + "\n";
                if (validity)
                    validateForm.cmboxPlateCode.Focus();
                validity = false;
            }
            if (validateForm.cmboxPlateCode.SelectedValue != null)
            {

                if ((string)validateForm.cmboxPlateCode.SelectedValue.ToString() == null || (string)validateForm.cmboxPlateCode.SelectedValue.ToString() == "")
                {
                    
                    Valid += "Plate Code not Selected" + "\n";
                    if (validity)
                        validateForm.cmboxPlateCode.Focus();
                    validity = false;
                    //Valid += _Resource.GetString("Plate Code not Selected") + "\n";
                }
            }
            else
            {
                
                Valid += "Plate Code not Selected" + "\n";
                if (validity)
                    validateForm.cmboxPlateCode.Focus();
                validity = false;
            }

           


            //if (validateForm.inputPlateNo.Text == "")
            //{
            //    validity = false;
            //    Valid += _Resource.GetString("Plate Number not entered") + "\n";
            //}
            if ((string)validateForm.txtBoxPlateNumber.Text == null || (string)validateForm.txtBoxPlateNumber.Text == "")
            {
               
                Valid += "Plate Number not entered" + "\n";
                if (validity)
                    validateForm.txtBoxPlateNumber.Focus();
                validity = false;
                // Valid += _Resource.GetString("Plate Number not entered") + "\n";
            }
           


            char[] checkNumbers = validateForm.txtBoxPlateNumber.Text.ToCharArray();
            bool allNumbers = true;
            foreach (char i in checkNumbers)
            {
                allNumbers = char.IsNumber(i);
                if (!(allNumbers))
                    break;
            }

            if (!(allNumbers))
            {
                
                Valid += "Plate Number must be digits";
                if (validity)
                    validateForm.txtBoxPlateNumber.Focus();
                validity = false;
                // Valid += _Resource.GetString("Plate Number must be digits");
            }

            //if (validateForm.inputPlateNo.Text.Length > 10 )
            //{
            //    validity = false;
            //    Valid += _Resource.GetString("Allowed Plate Number Length Exceeded") + "\n";
            //}
            // if (validity)
            //    Valid = _Resource.GetString("Valid");
            if (validity)
                Valid = "Valid";
            return Valid;
        }
    }
}
