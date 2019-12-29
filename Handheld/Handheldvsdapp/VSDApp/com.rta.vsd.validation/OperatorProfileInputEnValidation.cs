using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace VSDApp.com.rta.vsd.validation
{
    class OperatorProfileInputEnValidation : IValidation
    {

      //  private vsd.hh.localisation.Resources _Resource;

        public string Validate(UserControl form)
        {
            //_Resource = VSDApp.com.rta.vsd.hh.localisation.Resources.GetInstance();
            vsd.hh.ui.ucSearchOperatorProfile validateForm = (vsd.hh.ui.ucSearchOperatorProfile)form;
            string Valid = "";
            bool validity = true;
            //implement Validation Checks here
            //char[] checkNumbers = validateForm.inputOperatorName.Text.ToCharArray();
            //bool allNumbers = true;
            //foreach (char i in checkNumbers)
            //{
            //    if (char.IsLetter(i) || i == ' ')
            //        allNumbers = true;
            //    else allNumbers = false;
            //    if (!(allNumbers))
            //        break;
            //}

            //if (!(allNumbers))
            //{
            //    validity = false;
            //    Valid += _Resource.GetString("Invalid Characters in Operator Name") + "\n";
            //}
            if (validateForm.txtTraffice.Text == "")
            {
                
                Valid += "Traffic File Number Name not entered" + "\n";
                if (validity)
                    validateForm.txtTraffice.Focus();
                validity = false;
              //  Valid += _Resource.GetString("Traffic File Number Name not entered") + "\n";
            }
            char[] checkNumbers = validateForm.txtTraffice.Text.ToCharArray();
            bool allNumbers = true;
            foreach (char i in checkNumbers)
            {
                if (char.IsNumber(i) || i == '.')
                    allNumbers = true;
                else allNumbers = false;
                if (!(allNumbers))
                    break;
            }

            if (!(allNumbers))
            { 
                Valid += "Traffic File Number must be digits and dots" + "\n";
                if (validity)
                    validateForm.txtTraffice.Focus();
                validity = false;
                //  Valid += _Resource.GetString("Traffic File Number must be digits and dots") + "\n";
            }
            if (validateForm.txtTraffice.Text.Length > 25)
            {
                
                Valid += "Allowed File Number Length Exceeded" + "\n";
                if (validity)
                    validateForm.txtTraffice.Focus();
                validity = false;
               // Valid += _Resource.GetString("Allowed File Number Length Exceeded") + "\n";
            }
            if (validity)
                Valid = "Valid";
            return Valid;
        }
    }
}