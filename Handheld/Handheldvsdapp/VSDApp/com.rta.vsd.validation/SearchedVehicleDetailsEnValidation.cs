using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace VSDApp.com.rta.vsd.validation
{
    class SearchedVehicleDetailsEnValidation : IValidation
    {
        public string Validate(UserControl form)
        {
            // _Resource = VSDApp.com.rta.vsd.hh.localisation.Resources.GetInstance();
            vsd.hh.ui.ucSearchedVehicleDetials validateUserControl = (vsd.hh.ui.ucSearchedVehicleDetials)form;
            string Valid = "";
            bool validity = true;
            //implement Validation Checks here

            if (validateUserControl.txtBoxOperatorName.Text == "")
            {
                
                Valid += "Operator Name not Entered" + "\n";
                if (validity)
                    validateUserControl.txtBoxOperatorName.Focus();
                validity = false;
                // Valid += _Resource.GetString("Operator Name not Entered") + "\n";
            }
            char[] checkNumbers = validateUserControl.txtBoxOperatorName.Text.ToCharArray();
            bool allNumbers = true;
            foreach (char i in checkNumbers)
            {
                if (char.IsLetter(i) || i == ' ')
                    allNumbers = true;
                else allNumbers = false;
                if (!(allNumbers))
                    break;
            }
            /*
            if (!(allNumbers))
            {
                validity = false;
                Valid += "Invalid Characters in Operator Name" + "\n";
                // Valid += _Resource.GetString("Invalid Characters in Operator Name") + "\n";
            }*/
            if (validateUserControl.txtBoxChassisNumber.Text == "")
            {
                
                Valid += "Chassis Number not Entered" + "\n";
                if (validity)
                    validateUserControl.txtBoxChassisNumber.Focus();
                validity = false;
                // Valid += _Resource.GetString("Chassis Number not Entered") + "\n";
            }
            checkNumbers = validateUserControl.txtBoxChassisNumber.Text.ToCharArray();
            allNumbers = true;
            foreach (char i in checkNumbers)
            {
                if (char.IsLetterOrDigit(i) || i == '.' || i == '-' || i == ' ')
                    allNumbers = true;
                else allNumbers = false;
                if (!(allNumbers))
                    break;
            }

          //  if (!(allNumbers))
          //  {
          //      validity = false;
           //     Valid += "Invalid Characters in Chassis Number" + "\n";
                // Valid += _Resource.GetString("Invalid Characters in Chassis Number") + "\n";
          //  }
            if (validateUserControl.txtBoxMake.Text == "")
            {
               
                Valid += "Vehicle Make not Entered" + "\n";
                if (validity)
                    validateUserControl.txtBoxMake.Focus();
                validity = false;
                // Valid += _Resource.GetString("Vehicle Make not Entered") + "\n";
            }
            if (validateUserControl.txtModel.Text == " ")
            {
               
                Valid += "Vehicle Model not Entered" + "\n";
                if (validity)
                    validateUserControl.txtModel.Focus();
                validity = false;
                // Valid += _Resource.GetString("Vehicle Model not Entered") + "\n";
            }
            /*
            if (validateUserControl.txtYear.Text == "")
            {
                validity = false;
                Valid += "Vehicle Year not Entered" + "\n";
                // Valid += _Resource.GetString("Vehicle Year not Entered") + "\n";
            }
            checkNumbers = validateUserControl.txtYear.Text.ToCharArray();
            allNumbers = true;
            foreach (char i in checkNumbers)
            {
                allNumbers = char.IsNumber(i);
                if (!(allNumbers))
                    break;
            }

            if (!(allNumbers))
            {
                validity = false;
                Valid += "Year must be in digits" + "\n";
                // Valid += _Resource.GetString("Year must be in digits") + "\n";
            }
            if (checkNumbers.Length != 4)
            {
                validity = false;
                Valid += "Year must be a four digit number" + "\n";
                // Valid += _Resource.GetString("Year must be a four digit number") + "\n";
            }*/
            if (validateUserControl.txtBoxChassisNumber.Text.Length > 50)
            {
                
                Valid += "Allowed Chassis Number Length Exceeded" + "\n";
                if (validity)
                    validateUserControl.txtBoxChassisNumber.Focus();
                validity = false;
                //Valid += _Resource.GetString("Allowed Chassis Number Length Exceeded") + "\n";
            }
            if (validity)
                Valid = "Valid";
            return Valid;
        }
    }
}
