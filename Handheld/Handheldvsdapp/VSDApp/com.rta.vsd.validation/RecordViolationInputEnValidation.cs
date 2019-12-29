using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VSDApp.com.rta.vsd.hh.utilities;


namespace VSDApp.com.rta.vsd.validation
{
    class RecordViolationInputEnValidation : IValidation
    {

       // private vsd.hh.localisation.Resources _Resource;

        public string Validate(System.Windows.Controls.UserControl form)
        {
           // _Resource = VSDApp.com.rta.vsd.hh.localisation.Resources.GetInstance();
            vsd.hh.ui.ucVehicleSelection validateForm = (vsd.hh.ui.ucVehicleSelection)form;
            string Valid = "";
            bool validity = true;
            //implement Validation Checks here
            //System.Windows.Forms.MessageBox.Show((string)validateForm._SelectCountry.Text);
            if ((string)validateForm.cmboxCountry.Text == null || (string)validateForm.cmboxCountry.Text == "")
            {
                validity = false;
                Valid += "Country not Selected" + "\n";
                validateForm.cmboxCountry.Focus();
               // Valid += _Resource.GetString("Country not Selected") + "\n";
            }
            if ((string)validateForm.cmboxCountry.SelectedValue.ToString() == AppProperties.defaultCountry)
            {
                if (((string)validateForm.cmboxEmirates.Text == null || (string)validateForm.cmboxEmirates.Text == ""))
                {
                    
                    Valid += "Emirate not Selected" + "\n";
                    if (validity)
                        validateForm.cmboxEmirates.Focus();
                    validity = false;
                    // Valid += _Resource.GetString("Emirate not Selected") + "\n";
                }
                if ((string)validateForm.cmboxPlateCategory.SelectedValue.ToString() == null || (string)validateForm.cmboxPlateCategory.SelectedValue.ToString() == "")
                {
                   
                    Valid += "Plate Category not Selected" + "\n";
                    if (validity)
                        validateForm.cmboxPlateCategory.Focus();
                    validity = false;
                    //   Valid += _Resource.GetString("Plate Category not Selected") + "\n";
                }
                if ((string)validateForm.cmboxPlateCode.SelectedValue == null || (string)validateForm.cmboxPlateCode.SelectedValue == "")
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
                if ((string)validateForm.txtPlateCode.Text == null || (string)validateForm.txtPlateCode.Text == "")
                {                    
                    Valid += "Plate Code not Entered" + "\n";
                    if (validity)
                        validateForm.txtPlateCode.Focus();
                    validity = false;
                }
                char[] checkNumbers2 = validateForm.txtPlateCode.Text.ToCharArray();
                bool allNumbers2 = true;
                checkNumbers2 = validateForm.txtPlateCode.Text.ToCharArray();
                allNumbers2 = true;
                foreach (char i in checkNumbers2)
                {
                    if (char.IsLetterOrDigit(i) || i == '.' || i == '-' || i == ' ')
                        allNumbers2 = true;
                    else allNumbers2 = false;
                    if (!(allNumbers2))
                        break;
                }

                if (!(allNumbers2))
                {
                   // validity = false;
                   // Valid += "Invalid Characters in Plate Code" + "\n";
                    // Valid += _Resource.GetString("Invalid Characters in Chassis Number") + "\n";
                }
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
            if ((string)validateForm.cmboxVehicleCategoty.SelectedValue.ToString() == null || (string)validateForm.cmboxVehicleCategoty.SelectedValue.ToString() == "")
            {
                Valid += "vehicle Category not entered" + "\n";
                if (validity)
                    validateForm.cmboxVehicleCategoty.Focus();
                //Valid += _Resource.GetString("vehicle Category not entered") + "\n";

                validity = false;

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
