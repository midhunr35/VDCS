using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSDApp.com.rta.vsd.hh.utilities;

namespace VSDApp.com.rta.vsd.validation
{
    class ProfileManagmentValidation : IValidation
    {
        #region IValidation Members

        public string Validate(System.Windows.Controls.UserControl form)
        {
            vsd.hh.ui.ucProfileManagment validateForm = (vsd.hh.ui.ucProfileManagment)form;
            string Valid = "";
            bool validity = true;
            if (validateForm.txtBoxMob.Text == "")
            {

                Valid +=  new CommonUtils().GetStringValue("lblMobileNumberValidation");
                if (validity)
                    validateForm.txtBoxMob.Focus();
                validity = false;
                
            }
            char[] checkNumbers = validateForm.txtBoxMob.Text.ToCharArray();
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
                Valid += new CommonUtils().GetStringValue("lblMobileNumberNumbericValidation"); ;
                if (validity)
                    validateForm.txtBoxMob.Focus();
                validity = false;
                //  Valid += _Resource.GetString("Traffic File Number must be digits and dots") + "\n";
            }
            if (validity)
                Valid = "Valid";
            return Valid;
        }

        #endregion
    }
}
