using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using VSDApp.com.rta.vsd.hh.utilities;

namespace VSDApp.com.rta.vsd.validation
{
    class ViolationSeacrchArValidation : IValidation
    {
        // private vsd.hh.localisation.Resources _Resource;

        public string Validate(UserControl form)
        {
            // _Resource = VSDApp.com.rta.vsd.hh.localisation.Resources.GetInstance();
            vsd.hh.ui.ucSearchViolationListing validateForm = (vsd.hh.ui.ucSearchViolationListing)form;
            string Valid = "";
            bool validity = true;
            //implement Validation Checks here
            if (validateForm.txtBoxViolationID.Text == "" && validateForm.rdoBtnByViolationID.IsChecked == true)
            {
                validity = false;
                Valid += "لم يتم إدخال رقم التجاوز" + "\n";
                validateForm.txtBoxViolationID.Focus();
                // Valid += _Resource.GetString("Violation ID not entered") + "\n";
            }
            char[] checkNumbers = validateForm.txtBoxViolationID.Text.ToCharArray();
            bool allNumbers = true;
            foreach (char i in checkNumbers)
            {
                if (char.IsNumber(i) || i == '.')
                    allNumbers = true;
                else allNumbers = false;
                if (!(allNumbers))
                    break;
            }

            if (!(allNumbers) && validateForm.rdoBtnByViolationID.IsChecked == true)
            {
                
                Valid += "يجب أن يكون معرف مخالفة الأرقام والنقاط" + "\n";
                if (validity)
                    validateForm.rdoBtnByViolationID.Focus();
                validity = false;
                // Valid += _Resource.GetString("Violation ID must be digits and dots") + "\n";
            }
            if (validateForm.cmboxEmirates.SelectedValue != null)
            {
                if (((string)validateForm.cmboxEmirates.SelectedValue.ToString() == null || (string)validateForm.cmboxEmirates.SelectedValue.ToString() == "") && validateForm.rdoBtnPlateNumber.IsChecked == true && (string)validateForm.cmboxCountry.SelectedValue.ToString() == AppProperties.defaultCountry)
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
                /*
                Valid += "يرجى اختيار الإمارة" + "\n";
                if (validity)
                    validateForm.cmboxEmirates.Focus();
                validity = false;*/
            }
            if (validateForm.cmboxPlateCategory.SelectedValue != null)
            {
                if (((string)validateForm.cmboxPlateCategory.SelectedValue.ToString() == null || (string)validateForm.cmboxPlateCategory.SelectedValue.ToString() == "") && validateForm.rdoBtnPlateNumber.IsChecked == true && (string)validateForm.cmboxCountry.SelectedValue.ToString() == AppProperties.defaultCountry)
                {
                    
                    Valid += "يرجى إدخال رمز اللوحة" + "\n";
                    if (validity)
                        validateForm.cmboxPlateCategory.Focus();
                    validity = false;
                    // Valid += _Resource.GetString("Plate Category not Selected") + "\n";
                }
            }
            else
            {
               /*
                Valid += "يرجى إدخال رمز اللوحة" + "\n";
                if (validity)
                    validateForm.cmboxPlateCategory.Focus();
                validity = false;*/
            }
            if (validateForm.txtBoxPlateNumber.Text == "" && validateForm.rdoBtnPlateNumber.IsChecked == true)
            {
                
                Valid += "يرجى إدخال رقم اللوحة" + "\n";
                if (validity)
                    validateForm.txtBoxPlateNumber.Focus();
                validity = false;
                //  Valid += _Resource.GetString("Plate Number not entered") + "\n";
            }
            checkNumbers = validateForm.txtBoxPlateNumber.Text.ToCharArray();
            allNumbers = true;
            foreach (char i in checkNumbers)
            {
                allNumbers = char.IsNumber(i);
                if (!(allNumbers))
                    break;
            }

            if (!(allNumbers) && validateForm.rdoBtnPlateNumber.IsChecked == true)
            {
                
                Valid += "يرجى إدخال قيمة رقمية في خانة رقم اللوحة" + "\n";
                if (validity)
                    validateForm.rdoBtnPlateNumber.Focus();
                validity = false;
                // Valid += _Resource.GetString("Plate Number must be digits") + "\n";
            }
            if (validateForm.cmboxPlateCode.SelectedValue != null)
            {
                if (((string)validateForm.cmboxPlateCode.SelectedValue.ToString() == null || (string)validateForm.cmboxPlateCode.SelectedValue.ToString() == "") && validateForm.rdoBtnPlateNumber.IsChecked == true && (string)validateForm.cmboxCountry.SelectedValue.ToString() == AppProperties.defaultCountry)
                {
                    
                    Valid += "يرجى إدخال رمز اللوحة" + "\n";
                    if (validity)
                        validateForm.cmboxPlateCode.Focus();
                    validity = false;
                    // Valid += _Resource.GetString("Plate Code not Selected") + "\n";
                }
            }
            else
            {
                /*
                Valid += "يرجى إدخال رمز اللوحة" + "\n";
                if (validity)
                    validateForm.cmboxPlateCode.Focus();
                validity = false;*/
            }

            if (validateForm.txtBoxPlateNumber.Text.Length > 10 && validateForm.rdoBtnPlateNumber.IsChecked == true)
            {
                validity = false;
                Valid += "طول رقم لوحة تجاوز" + "\n";
                if (validity)
                    validateForm.txtBoxPlateNumber.Focus();
                // Valid += _Resource.GetString("Allowed Plate Number Length Exceeded") + "\n";
            }
            if (validity)
                Valid = "Valid";
            return Valid;
        }
    }
}
