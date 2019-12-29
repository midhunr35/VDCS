using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VSDApp.com.rta.vsd.hh.utilities;

namespace VSDApp.com.rta.vsd.validation
{
    class LoginEnValidation : IValidation
    {
        #region IValidation Members

        public string Validate(System.Windows.Controls.UserControl control)
        {
            try
            {
                // _Resource = VSDApp.com.rta.vsd.hh.localisation.Resources.GetInstance();
                //implement Validation Checks here
                vsd.hh.ui.ucLoginEn validationControl = (vsd.hh.ui.ucLoginEn)control;
                string Valid = "";
                bool validity = true;

                if (validationControl.txtBoxUserName.Text == "" || validationControl.txtBoxUserName.Text == "Username")
                {
                    //Valid += _Resource.GetString("Username not entered") + "\n";
                    Valid += "Username not entered" + "\n";
                    validity = false;
                    validationControl.txtBoxUserName.Focus();
                }
                if (validationControl.txtpswd.Password.ToString() == "" || validationControl.txtpswd.Password.ToString() == "Password")
                {
                    // Valid += _Resource.GetString("Password not entered") + "\n";
                    Valid += "Password not entered" + "\n";
                    if (validity)
                        validationControl.txtpswd.Focus();
                    validity = false;
                   
                }
                if (validity)
                {
                    Valid = "Valid";
                    // Valid = _Resource.GetString("Valid");
                }

                return Valid;
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                return "null";

            }
        }

        #endregion
    }
}
