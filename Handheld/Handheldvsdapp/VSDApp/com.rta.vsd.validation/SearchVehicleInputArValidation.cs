using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VSDApp.com.rta.vsd.hh.utilities;

namespace VSDApp.com.rta.vsd.validation
{
    class SearchVehicleInputArValidation : IValidation
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
                
                Valid += "يرجى اختيار المنطقة" + "\n";
                validateForm.cmboxCountry.Focus();
                validity = false;
                // Valid += _Resource.GetString("Country not Selected") + "\n";
            }
            if (validateForm.cmboxEmirates.SelectedValue != null)
            {

                if (((string)validateForm.cmboxEmirates.Text == null || (string)validateForm.cmboxEmirates.Text == ""))
                {
                   
                    Valid += "يرجى اختيار الإمارة" + "\n";
                    if (validity)
                        validateForm.cmboxEmirates.Focus();
                    validity = false;
                    // Valid += _Resource.GetString("Emirate not Selected") + "\n";
                }
            }
            else
            {
                validity = false;
                Valid += "يرجى اختيار الإمارة" + "\n";
            }
            if (validateForm.cmboxPlateCategory.SelectedValue != null)
            {

                if ((string)validateForm.cmboxPlateCategory.SelectedValue.ToString() == null || (string)validateForm.cmboxPlateCategory.SelectedValue.ToString() == "")
                {
                   
                    Valid += "لوحة الفئة لا مختارة" + "\n";
                    if (validity)
                        validateForm.cmboxPlateCategory.Focus();
                    validity = false;
                    //   Valid += _Resource.GetString("Plate Category not Selected") + "\n";
                }
            }
            else
            {
                
                Valid += "لوحة الفئة لا مختارة" + "\n";
                if(validity)
                    validateForm.cmboxPlateCategory.Focus();
                validity = false;
            }
            if (validateForm.cmboxPlateCode.SelectedValue != null)
            {
                if ((string)validateForm.cmboxPlateCode.SelectedValue.ToString() == null || (string)validateForm.cmboxPlateCode.SelectedValue.ToString() == "")
                {
                   
                    Valid += "يرجى إدخال رمز اللوحة" + "\n";
                    if (validity)
                        validateForm.cmboxPlateCode.Focus();
                    validity = false;
                    //Valid += _Resource.GetString("Plate Code not Selected") + "\n";
                }
            }
            else
            {
                validity = false;
                Valid += "يرجى إدخال رمز اللوحة" + "\n";
            }




            //if (validateForm.inputPlateNo.Text == "")
            //{
            //    validity = false;
            //    Valid += _Resource.GetString("Plate Number not entered") + "\n";
            //}
            if ((string)validateForm.txtBoxPlateNumber.Text == null || (string)validateForm.txtBoxPlateNumber.Text == "")
            {
                
                Valid += "يرجى إدخال رقم اللوحة" + "\n";
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
               
                Valid += "يرجى إدخال قيمة رقمية في خانة رقم اللوحة";
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
