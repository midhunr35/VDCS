using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VSDApp.com.rta.vsd.hh.data;
using VSDApp.com.rta.vsd.hh.db;
using VSDApp.com.rta.vsd.hh.manager;
using VSDApp.com.rta.vsd.hh.utilities;
using VSDApp.com.rta.vsd.validation;
using VSDApp.ProgressDialog;
using VSDApp.WPFMessageBoxControl;

namespace VSDApp.com.rta.vsd.hh.ui
{
    /// <summary>
    /// Interaction logic for ucVehicleSelection.xaml
    /// </summary>
    /// 
    public partial class ucVehicleSelection : UserControl
    {
        MainWindow m_mainWindow = null;
        bool is_intrested = false;
        bool m_IsDataLoaded = false;
        private IValidation _iValidate2;
        private string _validationResult2;
        public bool _isUAEVehicle;
        List<vsd.hh.data.Violation> openViolations = null;
        private IValidation _iValidate;
        private string _validationResult;

        public Hashtable countryTable = new Hashtable();
        public Hashtable emirateTable = new Hashtable();
        public Hashtable plateCatTable = new Hashtable();
        public Hashtable plateCodeTable = new Hashtable();
        public Hashtable vehicleCatTable = new Hashtable();
        public Hashtable vehiclesubCatTable = new Hashtable();

        private List<string> _countryList = new List<string>();
        private List<string> _emirateList = new List<string>();
        private List<string> _plateCategoryList = new List<string>();
        private List<string> _plateCodeList = new List<string>();
        private List<string> _categoryList = new List<string>();
        private List<string> _subcategoryList = new List<string>();
        private bool _IsShowKeyboard = true;
        public delegate void blink_inspectionrecomd_delegate();
        System.Timers.Timer timer1 = null;

        public void PopulateData()
        {
            AppProperties.vehicle = null;
            AppProperties.recordedViolation = null;
            AppProperties.previousDefectIDs = new List<int>();
            AppProperties.recordedViolation = new Violation();
            AppProperties.recordedViolation.InspectionArea = AppProperties.location;
            AppProperties.confiscatePlates = false;
            try
            {
                txtBoxPlateNumber.Text = (AppProperties.vehicle == null ? "" : AppProperties.vehicle.PlateNumber.ToString());
                if (cmboxCountry.Text == null || cmboxCountry.Text == "")
                {
                    cmboxCountry.Text = (AppProperties.vehicle == null ? "" : AppProperties.vehicle.Country);
                }
                if (cmboxEmirates.Text == null || cmboxEmirates.Text == "")
                {
                    cmboxEmirates.Text = (AppProperties.vehicle == null ? "" : AppProperties.vehicle.Emirate);
                }
                cmboxPlateCategory.Text = (AppProperties.vehicle == null ? "" : AppProperties.vehicle.PlateCategory);
                cmboxPlateCode.Text = (AppProperties.vehicle == null ? "" : AppProperties.vehicle.PlateCode);


                if (cmboxCountry.Items.Count > 0)
                    cmboxCountry.Items.Clear();
                // if (_countryList != null)
                //   _countryList = new List<string>();
                //else
                if (AppProperties.Selected_Resource == "Arabic")
                {
                    try
                    {
                        countryTable = CommonUtils.Splitter(((IViolation)ViolationManager.GetInstance()).GetLocation(null, null));
                    }
                    catch (Exception ex)
                    {
                        App.VSDLog.Info(ex.StackTrace);
                        WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
                    }

                    string[] countryArray = new string[countryTable.Count];
                    countryTable.Keys.CopyTo(countryArray, 0);
                    _countryList = new List<string>(countryArray);
                    _countryList.Sort();
                    foreach (string str in _countryList)
                    {
                        cmboxCountry.Items.Add(str.Trim());
                    }


                    cmboxCountry.SelectedItem = AppProperties.defaultCountryAr;
                }
                else
                {
                    _countryList = new List<string>(((IViolation)ViolationManager.GetInstance()).GetLocation(null, null));
                    _countryList.Sort();
                    foreach (string str in _countryList)
                    {
                        cmboxCountry.Items.Add(str.Trim());
                    }


                    cmboxCountry.SelectedItem = AppProperties.defaultCountry;
                }




                //if ((string)cmboxCountry.SelectedItem == AppProperties.defaultCountry || cmboxCountry.Text == AppProperties.defaultCountry)
                //{

                //    selectPlateCode.Show();


                //    inputPlateCode.Hide();

                //}
                //else
                //{

                //    inputPlateCode.Show();


                //    selectPlateCode.Hide();
                //}

                if (cmboxVehicleCategoty.Items.Count > 0)
                    cmboxVehicleCategoty.Items.Clear();
                if (AppProperties.Selected_Resource == "Arabic")
                {
                    vehicleCatTable = CommonUtils.Splitter(((IVehicleProfile)VehicleProfileManager.GetInstance()).GetVehicleCategories());
                    string[] vehCat = new string[vehicleCatTable.Count];
                    vehicleCatTable.Keys.CopyTo(vehCat, 0);
                    _categoryList = new List<string>(vehCat);
                    _categoryList.Sort();
                    foreach (string str in _categoryList)
                    {
                        cmboxVehicleCategoty.Items.Add(str.Trim());
                    }
                    if (cmboxVehicleCategoty.Items.Count > 0)
                        cmboxVehicleCategoty.SelectedIndex = 0;

                    cmboxVehicleCategoty.SelectedItem = AppProperties.defaultVehicleCategoryAr;
                }
                else
                {
                    _categoryList = new List<string>(((IVehicleProfile)VehicleProfileManager.GetInstance()).GetVehicleCategories());
                    _categoryList.Sort();
                    foreach (string str in _categoryList)
                    {
                        cmboxVehicleCategoty.Items.Add(str.Trim());
                    }
                    if (cmboxVehicleCategoty.Items.Count > 0)
                        cmboxVehicleCategoty.SelectedIndex = 0;
                    cmboxVehicleCategoty.SelectedItem = AppProperties.defaultVehicleCategoryEn;
                }

                GetVehicleSubCategory(cmboxVehicleCategoty.SelectedItem.ToString());

                //#region vehicleSubcategory

                //if (cmboxVehiclesubCategoty.Items.Count > 0)
                //    cmboxVehiclesubCategoty.Items.Clear();
                //if (AppProperties.Selected_Resource == "Arabic")
                //{
                //    vehicleCatTable = CommonUtils.Splitter(((IVehicleProfile)VehicleProfileManager.GetInstance()).GetVehicleSubCategories(cmboxVehicleCategoty.SelectedItem.ToString()));
                //    string[] vehCat = new string[vehicleCatTable.Count];
                //    vehicleCatTable.Keys.CopyTo(vehCat, 0);
                //    _subcategoryList = new List<string>(vehCat);
                //    _subcategoryList.Sort();
                //    foreach (string str in _subcategoryList)
                //    {
                //        cmboxVehiclesubCategoty.Items.Add(str.Trim());
                //    }
                //    if (cmboxVehiclesubCategoty.Items.Count > 0)
                //        cmboxVehiclesubCategoty.SelectedIndex = 0;
                //}
                //else
                //{
                //    _subcategoryList = new List<string>(((IVehicleProfile)VehicleProfileManager.GetInstance()).GetVehicleSubCategories(cmboxVehicleCategoty.SelectedItem.ToString()));
                //    _subcategoryList.Sort();
                //    foreach (string str in _categoryList)
                //    {
                //        cmboxVehiclesubCategoty.Items.Add(str.Trim());
                //    }
                //    if (cmboxVehiclesubCategoty.Items.Count > 0)
                //        cmboxVehiclesubCategoty.SelectedIndex = 0;
                //}

                //#endregion





                //_categoryList = new List<string>(_controller.GetVehicleCategory());



            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }


        public ucVehicleSelection(MainWindow mainWinow)
        {
            InitializeComponent();
            this.m_mainWindow = mainWinow;
            // dialogVehHistory.SetParent(MainGrid);
        }

        private void GetVehicleSubCategory(string category)
        {

            //#region vehicleSubcategory

            if (cmboxVehiclesubCategoty.Items.Count > 0)
                cmboxVehiclesubCategoty.Items.Clear();
            if (AppProperties.Selected_Resource == "Arabic")
            {
                vehiclesubCatTable = CommonUtils.Splitter(((IVehicleProfile)VehicleProfileManager.GetInstance()).GetVehicleSubCategories(cmboxVehicleCategoty.SelectedItem.ToString()));
                string[] vehCat = new string[vehiclesubCatTable.Count];
                vehiclesubCatTable.Keys.CopyTo(vehCat, 0);
                _subcategoryList = new List<string>(vehCat);
                _subcategoryList.Sort();
                foreach (string str in _subcategoryList)
                {
                    cmboxVehiclesubCategoty.Items.Add(str.Trim());
                }
                if (cmboxVehiclesubCategoty.Items.Count > 0)
                    cmboxVehiclesubCategoty.SelectedIndex = 0;
                if (_subcategoryList.Count > 0)
                    cmboxVehiclesubCategoty.SelectedItem = _subcategoryList.First().Trim();
            }
            else
            {
                _subcategoryList = new List<string>(((IVehicleProfile)VehicleProfileManager.GetInstance()).GetVehicleSubCategories(category));
                _subcategoryList.Sort();
                foreach (string str in _subcategoryList)
                {
                    cmboxVehiclesubCategoty.Items.Add(str.Trim());
                }
                if (cmboxVehiclesubCategoty.Items.Count > 0)
                    cmboxVehiclesubCategoty.SelectedIndex = 0;
                if (_subcategoryList.Count > 0)
                    cmboxVehiclesubCategoty.SelectedItem = _subcategoryList.First().Trim();
            }

            //#endregion
        }

        private void btnSearchViewEnable(bool enable)
        {

            if (enable)//when button enable
            {
                this.btnSearchVehicleRecord.IsEnabled = true;
                this.btnSearchVehicleRecord.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF3334"));
            }
            else
            {
                this.btnSearchVehicleRecord.IsEnabled = false;
                this.btnSearchVehicleRecord.Background = Brushes.Gray;
            }

        }

        private void cmboxCountry_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if ((string)cmboxCountry.SelectedItem != "")
                {
                    if (cmboxEmirates.Items.Count > 0)
                    {
                        cmboxEmirates.Text = "";
                        cmboxEmirates.Items.Clear();
                        cmboxEmirates.SelectedIndex = -1;
                    }
                    if (cmboxCountry.SelectedValue != null)
                    {
                        if (AppProperties.Selected_Resource == "Arabic")
                        {
                            // GetLocationDetails((string)countryTable[(string)selectCountry.SelectedItem], "emirate"));
                            if (cmboxCountry.SelectedValue.ToString().Trim() == AppProperties.defaultCountryAr.Trim())
                            {
                                //cmboxEmirates.IsEnabled = true;
                                //cmboxPlateCategory.IsEnabled = true;
                                EnableDisableFields(false);
                                lblemirates.Content = "*" + lblemirates.Content.ToString();
                                lblPlateCategory.Content = "*" + lblPlateCategory.Content.ToString();
                                lblPlateCode.Content = "*" + lblPlateCode.Content.ToString();
                                txtPlateCode.Visibility = System.Windows.Visibility.Collapsed;
                                cmboxPlateCode.Visibility = System.Windows.Visibility.Visible;
                                emirateTable = CommonUtils.Splitter(((IViolation)ViolationManager.GetInstance()).GetLocation((string)countryTable[(string)cmboxCountry.SelectedItem], "emirate"));
                                string[] emirate = new string[emirateTable.Count];
                                emirateTable.Keys.CopyTo(emirate, 0);


                                _emirateList = new List<string>(emirate);
                                _emirateList.Sort();
                                foreach (string str in _emirateList)
                                {
                                    cmboxEmirates.Items.Add(str.Trim());
                                }
                                if (cmboxEmirates.Items.Count > 0)
                                    cmboxEmirates.SelectedItem = AppProperties.defaultEmirateAr;
                                cmboxEmirates.UpdateLayout();


                                this.ucVehicleProfileSelection_Loaded();
                                btnVehHistory.Visibility = Visibility.Collapsed;
                                btnVehDetails.Visibility = Visibility.Collapsed;
                                ClearDataFields();
                                IsEnabledFileds(false, true);
                                //imagebtnSearch.IsEnabled = true;

                                btnSearchViewEnable(true);
                            }
                            else
                            {

                                lblemirates.Content = lblemirates.Content.ToString().Replace("*", "");
                                EnableDisableFields(true);
                                lblPlateCategory.Content = lblPlateCategory.Content.ToString().Replace("*", "");
                                lblPlateCode.Content = lblPlateCode.Content.ToString().Replace("*", "");
                                txtPlateCode.Visibility = System.Windows.Visibility.Visible;
                                cmboxPlateCode.Visibility = System.Windows.Visibility.Collapsed;
                                emirateTable = CommonUtils.Splitter(((IViolation)ViolationManager.GetInstance()).GetLocation((string)countryTable[(string)cmboxCountry.SelectedItem], "emirate"));
                                string[] emirate = new string[emirateTable.Count];
                                emirateTable.Keys.CopyTo(emirate, 0);


                                _emirateList = new List<string>(emirate);
                                _emirateList.Sort();
                                foreach (string str in _emirateList)
                                {
                                    cmboxEmirates.Items.Add(str.Trim());
                                }
                                if (cmboxEmirates.Items.Count > 0)
                                    cmboxEmirates.SelectedItem = AppProperties.defaultEmirateAr;
                                cmboxEmirates.UpdateLayout();


                                btnVehHistory.Visibility = Visibility.Collapsed;
                                btnVehDetails.Visibility = Visibility.Collapsed;

                                ClearDataFields();
                                IsEnabledFileds(false, false);

                                //imagebtnSearch.IsEnabled = false;
                                btnSearchViewEnable(false);

                            }

                        }
                        else
                        {
                            if (cmboxCountry.SelectedValue.ToString() == AppProperties.defaultCountry)
                            {
                                //cmboxEmirates.IsEnabled = true;
                                //cmboxPlateCategory.IsEnabled = true;
                                EnableDisableFields(false);
                                lblemirates.Content = lblemirates.Content.ToString() + "*";
                                lblPlateCategory.Content = lblPlateCategory.Content.ToString() + "*";
                                lblPlateCode.Content = lblPlateCode.Content.ToString() + "*";

                                txtPlateCode.Visibility = System.Windows.Visibility.Collapsed;
                                cmboxPlateCode.Visibility = System.Windows.Visibility.Visible;

                                ucVehicleProfileSelection_Loaded();

                                ClearDataFields();
                                IsEnabledFileds(false, true);
                                //imagebtnSearch.IsEnabled = true;

                                btnSearchViewEnable(true);

                            }
                            else
                            {

                                lblemirates.Content = lblemirates.Content.ToString().Replace("*", "");
                                //cmboxEmirates.IsEnabled = false;
                                //cmboxPlateCategory.IsEnabled = false;
                                EnableDisableFields(true);
                                lblPlateCategory.Content = lblPlateCategory.Content.ToString().Replace("*", "");

                                lblPlateCode.Content = lblPlateCode.Content.ToString().Replace("*", "");
                                txtPlateCode.Visibility = System.Windows.Visibility.Visible;
                                cmboxPlateCode.Visibility = System.Windows.Visibility.Collapsed;



                                btnVehHistory.Visibility = Visibility.Collapsed;
                                btnVehDetails.Visibility = Visibility.Collapsed;

                                ClearDataFields();
                                IsEnabledFileds(false, false);

                                // imagebtnSearch.IsEnabled = false;
                                btnSearchViewEnable(false);
                            }

                            _emirateList = new List<string>(((IViolation)ViolationManager.GetInstance()).GetLocation(cmboxCountry.SelectedValue.ToString(), "emirate"));
                            _emirateList.Sort();
                            foreach (string str in _emirateList)
                            {
                                cmboxEmirates.Items.Add(str.Trim());
                            }
                            if (cmboxEmirates.Items.Count > 0)
                                cmboxEmirates.SelectedItem = AppProperties.defaultEmirate;
                            cmboxEmirates.UpdateLayout();
                        }
                    }


                }
                else
                {
                    cmboxCountry.Items.Clear();
                    cmboxCountry.Items.Add("");
                }
                ResetFiledsOnSlectionChanged();


            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void EnableDisableFields(bool isReadOnly)
        {
            //cmboxEmirates.IsReadOnly = isReadOnly;
            //cmboxPlateCategory.IsReadOnly = isReadOnly;
            if (isReadOnly)
            {
                //cmboxEmirates.Background = Brushes.Gray;
                //  cmboxPlateCategory.Background = Brushes.Gray;
                cmboxEmirates.Visibility = Visibility.Collapsed;
                txtBoxEmirats.Visibility = Visibility.Visible;
                cmboxPlateCategory.Visibility = Visibility.Collapsed;
                txtBoxPlateCategory.Visibility = Visibility.Visible;

            }
            else
            {
                //cmboxEmirates.Background = Brushes.White;
                //cmboxPlateCategory.Background = Brushes.White;
                cmboxEmirates.Visibility = Visibility.Visible;
                txtBoxEmirats.Visibility = Visibility.Collapsed;
                cmboxPlateCategory.Visibility = Visibility.Visible;
                txtBoxPlateCategory.Visibility = Visibility.Collapsed;
            }
        }

        private void cmboxEmirates_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if ((string)cmboxEmirates.SelectedItem != "")
                {
                    if (cmboxPlateCategory.Items.Count > 0)
                    {
                        cmboxPlateCategory.Text = "";
                        cmboxPlateCategory.Items.Clear();
                        cmboxPlateCategory.SelectedIndex = -1;

                    }
                    if (cmboxEmirates.SelectedValue != null)
                    {
                        if (AppProperties.Selected_Resource == "Arabic")
                        {
                            // _controller.GetVehiclePlateCategories()
                            ClearDataFields();
                            IsEnabledFileds(false, false);
                            plateCatTable = new Hashtable();
                            plateCatTable = CommonUtils.Splitter(((IVehicleProfile)VehicleProfileManager.GetInstance()).GetVehiclePlateCategories((string)emirateTable[(string)cmboxEmirates.SelectedItem]));
                            string[] category = new string[plateCatTable.Count];
                            plateCatTable.Keys.CopyTo(category, 0);

                            _plateCategoryList = new List<string>(category);
                            _plateCategoryList.Sort();

                        }
                        else
                        {
                            ClearDataFields();
                            IsEnabledFileds(false, false);
                            _plateCategoryList = new List<string>(((IVehicleProfile)VehicleProfileManager.GetInstance()).GetVehiclePlateCategories((string)cmboxEmirates.SelectedValue.ToString()));
                            _plateCategoryList.Sort();
                        }
                    }
                    else
                        _plateCategoryList = new List<string>();
                    foreach (string str in _plateCategoryList)
                    {
                        cmboxPlateCategory.Items.Add(str.Trim());
                    }
                    if (cmboxPlateCategory.Items.Count > 0)
                        cmboxPlateCategory.SelectedIndex = 0;
                    cmboxPlateCategory.UpdateLayout();
                    if (AppProperties.Selected_Resource == "Arabic")
                    {
                        if (_plateCategoryList.Count > 0)
                        {
                            System.Text.RegularExpressions.Regex regEx = new System.Text.RegularExpressions.Regex("عام");
                            var qry = _plateCategoryList.Where<string>(item => regEx.IsMatch(item)).Last();
                            cmboxPlateCategory.SelectedItem = qry.Trim();
                        }

                        //AppProperties.defaultPlateCategoryAr;
                    }
                    else
                    {
                        if (_plateCategoryList.Count > 0)
                        {
                            System.Text.RegularExpressions.Regex regEx = new System.Text.RegularExpressions.Regex("Public");
                            var qry = _plateCategoryList.Where<string>(item => regEx.IsMatch(item)).Last();
                            cmboxPlateCategory.SelectedItem = qry.Trim();
                        }
                        //AppProperties.defaultPlateCategoryEn;
                    }
                    ResetFiledsOnSlectionChanged();
                }
                else
                {
                    cmboxEmirates.Items.Clear();
                    cmboxEmirates.Items.Add("");
                }
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void cmboxPlateCategory_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if ((string)cmboxPlateCategory.SelectedItem != "")
                {
                    if (cmboxPlateCode.Items.Count > 0)
                    {
                        cmboxPlateCode.Text = "";
                        cmboxPlateCode.Items.Clear();
                        cmboxPlateCode.SelectedIndex = -1;

                    }
                    if (cmboxPlateCategory.SelectedValue != null)
                    {
                        if (AppProperties.Selected_Resource == "Arabic")
                        {
                            // _controller.GetVehiclePlateCodes()#
                            plateCodeTable = new Hashtable();
                            plateCodeTable = CommonUtils.Splitter(((IVehicleProfile)VehicleProfileManager.GetInstance()).GetVehiclePlateCodes((string)plateCatTable[(string)cmboxPlateCategory.SelectedItem], (null != (string)cmboxEmirates.SelectedItem) ? (string)emirateTable[(string)cmboxEmirates.SelectedItem] : (string)emirateTable[(string)cmboxEmirates.Text]));
                            string[] code = new string[plateCodeTable.Count];
                            plateCodeTable.Keys.CopyTo(code, 0);
                            _plateCodeList = new List<string>(code);
                            _plateCodeList.Sort();
                        }
                        else
                        {
                            _plateCodeList = new List<string>(((IVehicleProfile)VehicleProfileManager.GetInstance()).GetVehiclePlateCodes((string)cmboxPlateCategory.SelectedValue.ToString(), (null != (string)cmboxEmirates.SelectedValue.ToString()) ? (string)cmboxEmirates.SelectedValue.ToString() : (string)cmboxEmirates.SelectedValue.ToString()));
                            _plateCodeList.Sort();
                        }
                    }
                    else
                        _plateCodeList = new List<string>();
                    if (_plateCodeList != null && _plateCodeList.Count > 0)
                    {
                        _plateCodeList.Sort();
                    }
                    foreach (string str in _plateCodeList)
                    {
                        cmboxPlateCode.Items.Add(str.Trim());
                    }
                    if (cmboxPlateCode.Items.Count > 0)
                        cmboxPlateCode.SelectedIndex = 0;
                    cmboxPlateCode.UpdateLayout();
                }
                else
                {
                    cmboxPlateCategory.Items.Clear();
                    cmboxPlateCategory.Items.Add("");
                }
                ResetFiledsOnSlectionChanged();
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void cmboxPlateCode_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmboxVehicleCategoty_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

            try
            {
                if ((string)cmboxVehicleCategoty.SelectedItem != "")
                {
                    if (cmboxVehiclesubCategoty.Items.Count > 0)
                    {
                        cmboxVehiclesubCategoty.Text = "";
                        cmboxVehiclesubCategoty.Items.Clear();
                        cmboxVehiclesubCategoty.SelectedIndex = -1;

                    }
                    if (cmboxVehicleCategoty.SelectedValue != null)
                    {

                        if (AppProperties.Selected_Resource == "Arabic")
                        {
                            vehiclesubCatTable = CommonUtils.Splitter(((IVehicleProfile)VehicleProfileManager.GetInstance()).GetVehicleSubCategories(cmboxVehicleCategoty.SelectedItem.ToString()));
                            string[] vehCat = new string[vehiclesubCatTable.Count];
                            vehiclesubCatTable.Keys.CopyTo(vehCat, 0);
                            _subcategoryList = new List<string>(vehCat);
                            _subcategoryList.Sort();

                        }
                        else
                        {
                            _subcategoryList = new List<string>(((IVehicleProfile)VehicleProfileManager.GetInstance()).GetVehicleSubCategories(cmboxVehicleCategoty.SelectedItem.ToString()));
                            _subcategoryList.Sort();
                        }
                    }
                    else
                        _subcategoryList = new List<string>();
                    if (_subcategoryList != null && _subcategoryList.Count > 0)
                    {
                        _subcategoryList.Sort();
                    }
                    foreach (string str in _subcategoryList)
                    {
                        cmboxVehiclesubCategoty.Items.Add(str.Trim());
                    }
                    if (cmboxVehiclesubCategoty.Items.Count > 0)
                        cmboxVehiclesubCategoty.SelectedIndex = 0;
                    if (_subcategoryList.Count > 0)
                        cmboxVehiclesubCategoty.SelectedItem = _subcategoryList.First().Trim();
                    cmboxVehiclesubCategoty.UpdateLayout();
                }
                else
                {
                    cmboxPlateCategory.Items.Clear();
                    cmboxPlateCategory.Items.Add("");
                }
               // ResetFiledsOnSlectionChanged();
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void cmboxVehicleSubCategoty_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            //AppProperties.vehicle.SubCategory = cmboxVehiclesubCategoty.SelectedItem.ToString();

            //AppProperties.vehicle.SubCategoryAr = cmboxVehiclesubCategoty.SelectedItem.ToString();
        }

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.DigitSubstitution = System.Globalization.DigitShapes.None;
            this.UpdateLayout();
            if (!m_IsDataLoaded)
            {
                PopulateData();
            }
            ClearDataFields();
            btnSearchViewEnable(true);
            //if (AppProperties.Selected_Resource == "English")
            //{
            //    imagebtnSearch.Source = new BitmapImage(new Uri(@"/Images/Buttons/Large/Search.png", UriKind.Relative));


            //}
            //else
            //{
            //    imagebtnSearch.Source = new BitmapImage(new Uri(@"/Images/Buttons/Large/Search Arabic Up Large.png", UriKind.Relative));

            //}
            //  EnableDisableVehcileResult(false);

            //ucRecordViolationNonUAEVehicle
            ucVehicleProfileSelection_Loaded();
            btnVehHistory.Visibility = Visibility.Collapsed;
            btnVehDetails.Visibility = Visibility.Collapsed;

            ClearDataFields();
            IsEnabledFileds(false, true);//onload country would be default


            lblemirates.Content = lblemirates.Content.ToString().Replace("*", "");
            lblPlateCategory.Content = lblPlateCategory.Content.ToString().Replace("*", "");
            lblPlateCode.Content = lblPlateCode.Content.ToString().Replace("*", "");

            if (AppProperties.Selected_Resource == "Arabic")
            {
                m_mainWindow.txtlocation.Visibility = System.Windows.Visibility.Visible;

                m_mainWindow.txtlocation.Text = AppProperties.Current_Selected_LocationAr;
                //  AppProperties.location.location = (string)locationTable[(string)cmboxLocation.SelectedItem];
                // AppProperties.recordedViolation.InspectionArea = AppProperties.location;
            }
            else
            {
                // AppProperties.location.city = ((string)((ucLocationSelectionEn)this).cmboxEmirates.Text).Trim();
                //AppProperties.location.area = ((string)((ucLocationSelectionEn)this).cmboxArea.Text).Trim();
                m_mainWindow.txtlocation.Visibility = System.Windows.Visibility.Visible;

                m_mainWindow.txtlocation.Text = AppProperties.Current_Selected_LocationEn;
            }
            // this.cntControlVehcileSerachRecord.Content = this.m_mainWindow.m_PagesList[5];
            //this.cntControlVehcileSerachRecord.IsEnabled = false;
        }


        public void EnableDisableVehicleInputDetails(bool _enable)
        {
            /*cmboxCountry.IsEnabled = _enable;
            cmboxEmirates.IsEnabled = _enable;
            cmboxPlateCategory.IsEnabled = _enable;
            cmboxPlateCode.IsEnabled = _enable;
            cmboxVehicleCategoty.IsEnabled = _enable;
            txtBoxPlateNumber.IsEnabled = _enable;
            this.btnSearch.IsEnabled = _enable;
             * */

        }


        public void SearchVehicleFromEtraffiEnHandler()
        {
            try
            {
                App.VSDLog.Info("\n ucVehicleSelection.SearchVehicleEnHandler()");


                _iValidate = (IValidation)new RecordViolationInputEnValidation();
                _validationResult = _iValidate.Validate(this);
                if (_validationResult != "Valid")
                {
                    WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), _validationResult);
                    return;
                }
                else
                {

                    // string vehicleCatAr = ((IVehicleProfile)VehicleProfileManager.GetInstance()).GetAlternateCategory(cmboxVehicleCategoty.SelectedValue.ToString(), _resources.GetLocale()).Trim();   
                    //  IVehicleProfile iVehicleProfile = ((IVehicleProfile)VehicleProfileManager.GetInstance());


                    //  string vehicleCatAr = iVehicleProfile.GetAlternateCategory(cmboxVehicleCategoty.SelectedValue.ToString(), "en-US");


                    //    ProgressDialogResult result = ProgressDialog.ProgressDialog.Execute(this.m_mainWindow, "Searching Vehicle...", (bw, we) =>
                    //{
                    string vehicleCatAr = ((IVehicleProfile)VehicleProfileManager.GetInstance()).GetAlternateCategory(cmboxVehicleCategoty.SelectedValue.ToString(), "en-US");
                    //    vehicleCatAr = vehicleCatAr;
                    //});


                    if (cmboxCountry.SelectedValue.ToString().Trim() != AppProperties.defaultCountry)
                    {
                        App.VSDLog.Info("\n Entered in if without default country");
                        //No UAE Vehicle
                        if (AppProperties.vehicle == null)
                        {
                            AppProperties.vehicle = new Vehicle();
                        }
                        AppProperties.vehicle.Country = cmboxCountry.SelectedValue.ToString();
                        if (null == (string)cmboxEmirates.SelectedItem)
                        {
                            AppProperties.vehicle.Emirate = "";
                        }
                        else
                        {
                            AppProperties.vehicle.Emirate = cmboxEmirates.SelectedValue.ToString();
                        }
                        if (cmboxPlateCode.Visibility == System.Windows.Visibility.Collapsed)
                        {
                            AppProperties.vehicle.PlateCode = txtPlateCode.Text;
                        }
                        else
                        {
                            AppProperties.vehicle.PlateCode = cmboxPlateCode.SelectedValue.ToString();
                        }
                        if (null == (string)cmboxPlateCategory.SelectedItem)
                        {
                            AppProperties.vehicle.PlateCategory = "";
                        }
                        else
                        {
                            AppProperties.vehicle.PlateCategory = "";
                        }


                        AppProperties.vehicle.PlateNumber = txtBoxPlateNumber.Text;
                        AppProperties.vehicle.VehicleCategory = cmboxVehicleCategoty.SelectedValue.ToString();
                        AppProperties.vehicle.VehicleCategoryAr = vehicleCatAr;
                        vsd.hh.utilities.AppProperties.routeFromRecordViolation = false;

                        //  EnableDisableVehicleInputDetails(false);
                        ucVehicleProfileSelection_Loaded();



                        // ucNonUAEVeh.txtBoxOperatorName.Focus();
                        this.UpdateLayout();
                        // this.cntControlVehcileSerachRecord.Content = new ucRecordViolationNonUAEVehicle(this.m_mainWindow);

                        //  this.cntControlVehcileSerachRecord.Content = this.m_mainWindow.m_PagesList[5];
                        // EnableDisableVehicleInputDetails(false);
                        //Non UAE Vehicle Violation Screen


                    }
                    else
                    {
                        //UAE Vehcile
                        App.VSDLog.Info("\n Entered in Else , UAE Vehicle");

                        IVehicleProfile iVehicleProfile = ((IVehicleProfile)VehicleProfileManager.GetInstance());
                        string country = cmboxCountry.SelectedValue.ToString().Trim();
                        string source = cmboxEmirates.SelectedValue.ToString().Trim();
                        string category = cmboxPlateCategory.SelectedValue.ToString().Trim();
                        string code = cmboxPlateCode.SelectedValue.ToString().Trim();
                        string number = txtBoxPlateNumber.Text.Trim();
                        //  ProgressDialogResult result2 = ProgressDialog.ProgressDialog.Execute(this.m_mainWindow, lblSearchingVehicle.Content.ToString(), (bw, we) =>
                        //{
                        //  iVehicleProfile.GetVehicleProfileDetails(country, source, category, number, code);
                        // });

                        ProgressDialogResult result = ProgressDialog.ProgressDialog.Execute(this.m_mainWindow, lblSearchingVehicle.Content.ToString(), (bw, we) =>
                        {
                            App.VSDLog.Info(" \n Going to call ucVehicleSelection.GetVehicleProfileDetails(" + country + "," + source + "," + category + "," + number + "," + code + ")");
                            iVehicleProfile.GetVehicleProfileDetailsfromEtraffic(country, source, category, number, code);

                            // So this check in order to avoid default processing after the Cancel button has been pressed.
                            // This call will set the Cancelled flag on the result structure.
                            ProgressDialog.ProgressDialog.CheckForPendingCancellation(bw, we);

                        }, ProgressDialogSettings.WithSubLabelAndCancel);

                        if (result == null || result.Cancelled)
                            return;
                        else if (result.OperationFailed)
                            return;
                        if (AppProperties.businessError)
                        {
                            App.VSDLog.Info(" \n got businessError in ucVehicleSelection.GetVehicleProfileDetails");
                            AppProperties.vehicle = null;
                            AppProperties.recordedViolation = null;
                            AppProperties.recordedViolation = new Violation();
                            AppProperties.recordedViolation.InspectionArea = AppProperties.location;
                            WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("ErrorBusiness"), AppProperties.errorMessageFromBusiness);

                            this.txtBoxOperatorName.Focus();
                            this.UpdateLayout();
                            AppProperties.businessError = false;
                            // busyIndicator.IsBusy = false;
                            //    LandingScreenEn landing = new LandingScreenEn();
                            //   _render.switchDisplay(form, landing);
                            //  this.m_mainWindow.MainContentControl.Content = this.m_mainWindow.m_PagesList[3];
                            return;
                        }
                        if (AppProperties.IsException)
                        {
                            App.VSDLog.Info(" \n got Exception in responce while calling ucVehicleSelection.GetVehicleProfileDetails");
                            AppProperties.IsException = false;
                            WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("ErrorException"), AppProperties.errorMessageFromBusiness);

                            IsEnabledFileds(true, true);
                            ClearDataFields();

                            // ucNonUAEVehNew.txtBoxOperatorName.Focus();
                            this.UpdateLayout();
                            AppProperties.vehicle = null;
                            return;
                        }
                        if (null == AppProperties.vehicle || AppProperties.IsServiceResponseNull || AppProperties.NotFoundError)
                        {
                            App.VSDLog.Info(" \n ucVehicleSelection.GetVehicleProfileDetails(): vehicle data not found.");


                            WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("VehicleNotFound"), AppProperties.errorMessageFromBusiness);
                            IsEnabledFileds(true, true);

                            this.UpdateLayout();
                            _IsShowKeyboard = false;
                            return;
                        }
                        //  AppProperties.vehicle.VehicleCategory = cmboxVehicleCategoty.SelectedValue.ToString();
                        //  AppProperties.vehicle.VehicleCategoryAr = vehicleCatAr;
                        btnVehHistory.Visibility = Visibility.Visible;
                        btnVehDetails.Visibility = Visibility.Visible;

                        if (AppProperties.vehicle.Emirate.Equals(AppProperties.defaultEmirate, StringComparison.CurrentCultureIgnoreCase) && AppProperties.isOnline)
                        {
                            App.VSDLog.Info(" \n ucVehicleSelection.GetVehicleProfileDetails(): Emirate vehicle found.");
                            vsd.hh.utilities.AppProperties.routeFromRecordViolation = true;
                            // EnableDisableVehicleInputDetails(false);

                            this.ChangeVehicleCategoryAsPerServiceResponse(AppProperties.vehicle.VehicleCategory);
                            this.ChangeVehicleSubCategoryAsPerServiceResponse(AppProperties.vehicle.VehicleCategory, AppProperties.vehicle.SubCategoryAr);
                            this.PopulateData_VehicelProfileInspection();
                            this.IsEnabledFileds(true, true);

                            btnStartInspection.Focus();

                            this.UpdateLayout();
                            _IsShowKeyboard = false;
                            return;

                        }
                        else
                        {
                            App.VSDLog.Info(" \n ucVehicleSelection.GetVehicleProfileDetails(): Non UAE vehicle found.");
                            vsd.hh.utilities.AppProperties.routeFromRecordViolation = true;
                            // EnableDisableVehicleInputDetails(false);

                            this.ChangeVehicleCategoryAsPerServiceResponse(AppProperties.vehicle.VehicleCategory);
                            this.ChangeVehicleSubCategoryAsPerServiceResponse(AppProperties.vehicle.VehicleCategory, AppProperties.vehicle.SubCategoryAr);
                            this.PopulateData_VehicelProfileInspection();
                            this.IsEnabledFileds(true, true);

                            btnStartInspection.Focus();

                            this.UpdateLayout();
                            _IsShowKeyboard = false;
                            return;
                        }

                    }
                }

            }

            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                // busyIndicator.IsBusy = false;
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }
        public void SearchVehicleEnHandler()
        {
            try
            {
                App.VSDLog.Info("\n ucVehicleSelection.SearchVehicleEnHandler()");


                _iValidate = (IValidation)new RecordViolationInputEnValidation();
                _validationResult = _iValidate.Validate(this);
                if (_validationResult != "Valid")
                {
                    WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), _validationResult);
                    return;
                }
                else
                {

                    // string vehicleCatAr = ((IVehicleProfile)VehicleProfileManager.GetInstance()).GetAlternateCategory(cmboxVehicleCategoty.SelectedValue.ToString(), _resources.GetLocale()).Trim();   
                    //  IVehicleProfile iVehicleProfile = ((IVehicleProfile)VehicleProfileManager.GetInstance());


                    //  string vehicleCatAr = iVehicleProfile.GetAlternateCategory(cmboxVehicleCategoty.SelectedValue.ToString(), "en-US");


                    //    ProgressDialogResult result = ProgressDialog.ProgressDialog.Execute(this.m_mainWindow, "Searching Vehicle...", (bw, we) =>
                    //{
                    string vehicleCatAr = ((IVehicleProfile)VehicleProfileManager.GetInstance()).GetAlternateCategory(cmboxVehicleCategoty.SelectedValue.ToString(), "en-US");
                    //    vehicleCatAr = vehicleCatAr;
                    //});


                    if (cmboxCountry.SelectedValue.ToString().Trim() != AppProperties.defaultCountry)
                    {
                        App.VSDLog.Info("\n Entered in if without default country");
                        //No UAE Vehicle
                        if (AppProperties.vehicle == null)
                        {
                            AppProperties.vehicle = new Vehicle();
                        }
                        AppProperties.vehicle.Country = cmboxCountry.SelectedValue.ToString();
                        if (null == (string)cmboxEmirates.SelectedItem)
                        {
                            AppProperties.vehicle.Emirate = "";
                        }
                        else
                        {
                            AppProperties.vehicle.Emirate = cmboxEmirates.SelectedValue.ToString();
                        }
                        if (cmboxPlateCode.Visibility == System.Windows.Visibility.Collapsed)
                        {
                            AppProperties.vehicle.PlateCode = txtPlateCode.Text;
                        }
                        else
                        {
                            AppProperties.vehicle.PlateCode = cmboxPlateCode.SelectedValue.ToString();
                        }
                        if (null == (string)cmboxPlateCategory.SelectedItem)
                        {
                            AppProperties.vehicle.PlateCategory = "";
                        }
                        else
                        {
                            AppProperties.vehicle.PlateCategory = "";
                        }


                        AppProperties.vehicle.PlateNumber = txtBoxPlateNumber.Text;
                        AppProperties.vehicle.VehicleCategory = cmboxVehicleCategoty.SelectedValue.ToString();
                        AppProperties.vehicle.VehicleCategoryAr = vehicleCatAr;
                        vsd.hh.utilities.AppProperties.routeFromRecordViolation = false;

                        //  EnableDisableVehicleInputDetails(false);
                        ucVehicleProfileSelection_Loaded();



                        // ucNonUAEVeh.txtBoxOperatorName.Focus();
                        this.UpdateLayout();
                        // this.cntControlVehcileSerachRecord.Content = new ucRecordViolationNonUAEVehicle(this.m_mainWindow);

                        //  this.cntControlVehcileSerachRecord.Content = this.m_mainWindow.m_PagesList[5];
                        // EnableDisableVehicleInputDetails(false);
                        //Non UAE Vehicle Violation Screen


                    }
                    else
                    {
                        //UAE Vehcile
                        App.VSDLog.Info("\n Entered in Else , UAE Vehicle");

                        IVehicleProfile iVehicleProfile = ((IVehicleProfile)VehicleProfileManager.GetInstance());
                        string country = cmboxCountry.SelectedValue.ToString().Trim();
                        string source = cmboxEmirates.SelectedValue.ToString().Trim();
                        string category = cmboxPlateCategory.SelectedValue.ToString().Trim();
                        string code = cmboxPlateCode.SelectedValue.ToString().Trim();
                        string number = txtBoxPlateNumber.Text.Trim();
                        //  ProgressDialogResult result2 = ProgressDialog.ProgressDialog.Execute(this.m_mainWindow, lblSearchingVehicle.Content.ToString(), (bw, we) =>
                        //{
                        //  iVehicleProfile.GetVehicleProfileDetails(country, source, category, number, code);
                        // });

                        ProgressDialogResult result = ProgressDialog.ProgressDialog.Execute(this.m_mainWindow, lblSearchingVehicle.Content.ToString(), (bw, we) =>
                        {
                            App.VSDLog.Info(" \n Going to call ucVehicleSelection.GetVehicleProfileDetails(" + country + "," + source + "," + category + "," + number + "," + code + ")");
                            iVehicleProfile.GetVehicleProfileDetails(country, source, category, number, code);

                            // So this check in order to avoid default processing after the Cancel button has been pressed.
                            // This call will set the Cancelled flag on the result structure.
                            ProgressDialog.ProgressDialog.CheckForPendingCancellation(bw, we);

                        }, ProgressDialogSettings.WithSubLabelAndCancel);

                        if (result == null || result.Cancelled)
                            return;
                        else if (result.OperationFailed)
                            return;
                        if (AppProperties.businessError)
                        {
                            App.VSDLog.Info(" \n got businessError in ucVehicleSelection.GetVehicleProfileDetails");
                            AppProperties.vehicle = null;
                            AppProperties.recordedViolation = null;
                            AppProperties.recordedViolation = new Violation();
                            AppProperties.recordedViolation.InspectionArea = AppProperties.location;
                            if (!AppProperties.isNetworkConnected)
                            {
                                WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("lblOfflinDeviceMessage"), AppProperties.errorMessageFromBusiness);
                            }
                            else
                            {
                                WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("ErrorBusiness"), AppProperties.errorMessageFromBusiness);
                            }
                            this.txtBoxOperatorName.Focus();
                            this.UpdateLayout();
                            AppProperties.businessError = false;
                            // busyIndicator.IsBusy = false;
                            //    LandingScreenEn landing = new LandingScreenEn();
                            //   _render.switchDisplay(form, landing);
                            //  this.m_mainWindow.MainContentControl.Content = this.m_mainWindow.m_PagesList[3];
                            return;
                        }
                        if (AppProperties.IsException)
                        {
                            App.VSDLog.Info(" \n got Exception in responce while calling ucVehicleSelection.GetVehicleProfileDetails");
                            AppProperties.IsException = false;
                            if (!AppProperties.isNetworkConnected)
                            {
                                WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("lblOfflinDeviceMessage"), AppProperties.errorMessageFromBusiness);
                            }
                            else
                            {
                                WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("ErrorException"), AppProperties.errorMessageFromBusiness);
                            }
                            IsEnabledFileds(true, true);
                            ClearDataFields();

                            // ucNonUAEVehNew.txtBoxOperatorName.Focus();
                            this.UpdateLayout();
                            AppProperties.vehicle = null;
                            return;
                        }
                        if (null == AppProperties.vehicle || AppProperties.IsServiceResponseNull || AppProperties.NotFoundError)
                        {
                            App.VSDLog.Info(" \n ucVehicleSelection.GetVehicleProfileDetails(): vehicle data not found.");


                            if (!AppProperties.isNetworkConnected)
                            {
                                WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("lblOfflinDeviceMessage"), AppProperties.errorMessageFromBusiness);
                            }
                            else
                            {
                                WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("VehicleNotFound"), AppProperties.errorMessageFromBusiness);

                            } IsEnabledFileds(true, true);

                            this.UpdateLayout();
                            _IsShowKeyboard = false;
                            return;
                        }
                        //  AppProperties.vehicle.VehicleCategory = cmboxVehicleCategoty.SelectedValue.ToString();
                        //  AppProperties.vehicle.VehicleCategoryAr = vehicleCatAr;
                        btnVehHistory.Visibility = Visibility.Visible;
                        btnVehDetails.Visibility = Visibility.Visible;

                        if (AppProperties.vehicle.Emirate.Equals(AppProperties.defaultEmirate, StringComparison.CurrentCultureIgnoreCase) && AppProperties.isOnline)
                        {

                            App.VSDLog.Info(" \n ucVehicleSelection.GetVehicleProfileDetails(): Emirate vehicle found.");
                            vsd.hh.utilities.AppProperties.routeFromRecordViolation = true;
                            // EnableDisableVehicleInputDetails(false);

                            this.ChangeVehicleCategoryAsPerServiceResponse(AppProperties.vehicle.VehicleCategory);
                            this.ChangeVehicleSubCategoryAsPerServiceResponse(AppProperties.vehicle.VehicleCategory, AppProperties.vehicle.SubCategoryAr);
                            this.PopulateData_VehicelProfileInspection();
                            this.IsEnabledFileds(true, true);
                            btnStartInspection.Focus();
                            this.UpdateLayout();
                            _IsShowKeyboard = false;
                            return;

                        }
                        else
                        {
                            App.VSDLog.Info(" \n ucVehicleSelection.GetVehicleProfileDetails(): Non UAE vehicle found.");
                            AppProperties.isOnline = true;
                            vsd.hh.utilities.AppProperties.routeFromRecordViolation = true;
                            EnableDisableVehicleInputDetails(false);

                            this.ChangeVehicleCategoryAsPerServiceResponse(AppProperties.vehicle.VehicleCategory);
                            this.ChangeVehicleSubCategoryAsPerServiceResponse(AppProperties.vehicle.VehicleCategory, AppProperties.vehicle.SubCategoryAr);
                            this.PopulateData_VehicelProfileInspection();
                            this.IsEnabledFileds(true, true);

                            btnStartInspection.Focus();

                            this.UpdateLayout();
                            _IsShowKeyboard = false;
                            return;
                        }

                    }
                }

            }

            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                // busyIndicator.IsBusy = false;
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }
        public void SearchVehicleArHandler()
        {


            try
            {

                App.VSDLog.Info(" \n Entered in  ucVehicleSelection.SearchVehicleArHandler()");

                _iValidate = (IValidation)new RecordViolationInputArValidation();
                _validationResult = _iValidate.Validate(this);
                if (_validationResult != "Valid")
                {
                    WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), _validationResult);
                    return;
                }
                else
                {

                    // string vehicleCatAr = ((IVehicleProfile)VehicleProfileManager.GetInstance()).GetAlternateCategory(cmboxVehicleCategoty.SelectedValue.ToString(), _resources.GetLocale()).Trim();   
                    //  IVehicleProfile iVehicleProfile = ((IVehicleProfile)VehicleProfileManager.GetInstance());


                    //  string vehicleCatAr = iVehicleProfile.GetAlternateCategory(cmboxVehicleCategoty.SelectedValue.ToString(), "en-US");


                    //    ProgressDialogResult result = ProgressDialog.ProgressDialog.Execute(this.m_mainWindow, "Searching Vehicle...", (bw, we) =>
                    //{
                    string vehicleCatAr = ((IVehicleProfile)VehicleProfileManager.GetInstance()).GetAlternateCategory((string)this.vehicleCatTable[cmboxVehicleCategoty.Text], "ar-AE");
                    //    vehicleCatAr = vehicleCatAr;
                    //});


                    if (cmboxCountry.SelectedValue.ToString().Trim() != AppProperties.defaultCountryAr)
                    {
                        //No UAE Vehicle
                        App.VSDLog.Info(" \n Entered in  ucVehicleSelection.SearchVehicleArHandler(), Non UAE Vehicle Block");
                        if (AppProperties.vehicle == null)
                        {
                            AppProperties.vehicle = new Vehicle();
                        }
                        AppProperties.vehicle.Country = (string)this.countryTable[cmboxCountry.Text];

                        if (null == (string)this.emirateTable[cmboxEmirates.Text])
                        {
                            AppProperties.vehicle.Emirate = "";
                        }
                        else
                        {
                            AppProperties.vehicle.Emirate = (string)this.emirateTable[cmboxEmirates.Text];
                        }
                        if (cmboxPlateCode.Visibility == System.Windows.Visibility.Collapsed)
                        {
                            AppProperties.vehicle.PlateCode = txtPlateCode.Text;
                        }
                        else
                        {
                            AppProperties.vehicle.PlateCode = (string)this.plateCodeTable[cmboxPlateCode.Text];
                        }
                        if (null == (string)this.plateCatTable[cmboxPlateCategory.Text])
                        {
                            AppProperties.vehicle.PlateCategory = "";
                        }
                        else
                        {
                            AppProperties.vehicle.PlateCategory = (string)this.plateCatTable[cmboxPlateCategory.Text];
                        }


                        AppProperties.vehicle.PlateNumber = this.txtBoxPlateNumber.Text;
                        AppProperties.vehicle.VehicleCategory = (string)this.vehicleCatTable[cmboxVehicleCategoty.Text];
                        AppProperties.vehicle.VehicleCategoryAr = vehicleCatAr;
                        vsd.hh.utilities.AppProperties.routeFromRecordViolation = false;

                        EnableDisableVehicleInputDetails(false);
                        // PopulateData_VehicelProfileInspection();
                        // btnStartInspection.Focus();
                        IsEnabledFileds(true, false);

                        // ucNonUAEVeh.txtBoxOperatorName.Focus();
                        this.UpdateLayout();



                    }
                    else
                    {
                        //UAE Vehcile

                        App.VSDLog.Info(" \n Entered in  ucVehicleSelection.SearchVehicleArHandler(), UAE Vehicle");
                        IVehicleProfile iVehicleProfile = ((IVehicleProfile)VehicleProfileManager.GetInstance());
                        string country = (string)this.countryTable[cmboxCountry.Text];
                        string source = (string)this.emirateTable[cmboxEmirates.Text];
                        string category = (string)this.plateCatTable[cmboxPlateCategory.Text];
                        string code = (string)this.plateCodeTable[cmboxPlateCode.Text];
                        string number = txtBoxPlateNumber.Text.Trim();
                        // ProgressDialogResult result2 = ProgressDialog.ProgressDialog.Execute(this.m_mainWindow, lblSearchingVehicle.Content.ToString(), (bw, we) =>
                        //{
                        //  iVehicleProfile.GetVehicleProfileDetails(country, source, category, number, code);
                        // });
                        ProgressDialogResult result = ProgressDialog.ProgressDialog.Execute(this.m_mainWindow, lblSearchingVehicle.Content.ToString(), (bw, we) =>
                        {
                            App.VSDLog.Info(" \n going to call iVehicleProfile.GetVehicleProfileDetails(country, source, category, number, code); in ucVehicleSelection.SearchVehicleArHandler()");
                            iVehicleProfile.GetVehicleProfileDetails(country, source, category, number, code);

                            // So this check in order to avoid default processing after the Cancel button has been pressed.
                            // This call will set the Cancelled flag on the result structure.
                            ProgressDialog.ProgressDialog.CheckForPendingCancellation(bw, we);

                        }, ProgressDialogSettings.WithSubLabelAndCancel);
                        App.VSDLog.Info("AppProperties.vehicle.VehicleCategoryAr0 : " + AppProperties.vehicle.VehicleCategoryAr);
                        if (result == null || result.Cancelled)
                            return;
                        else if (result.OperationFailed)
                            return;


                        if (AppProperties.businessError)
                        {
                            App.VSDLog.Info(" \n Business error in in ucVehicleSelection.SearchVehicleArHandler()");
                            AppProperties.vehicle = null;
                            AppProperties.recordedViolation = null;
                            AppProperties.recordedViolation = new Violation();
                            AppProperties.recordedViolation.InspectionArea = AppProperties.location;
                            if (!AppProperties.isNetworkConnected)
                            {
                                WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("lblOfflinDeviceMessage"), AppProperties.errorMessageFromBusiness);
                            }
                            else
                            {
                                WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("ErrorBusiness"), AppProperties.errorMessageFromBusiness);

                            }
                            AppProperties.businessError = false;
                            // busyIndicator.IsBusy = false;
                            //    LandingScreenEn landing = new LandingScreenEn();
                            //   _render.switchDisplay(form, landing);
                            //    this.m_mainWindow.MainContentControl.Content = this.m_mainWindow.m_PagesList[3];
                            return;
                        }
                        if (AppProperties.IsException)
                        {
                            App.VSDLog.Info(" \n Exception error in in ucVehicleSelection.SearchVehicleArHandler()");
                            AppProperties.IsException = false;
                            if (!AppProperties.isNetworkConnected)
                            {
                                WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("lblOfflinDeviceMessage"), AppProperties.errorMessageFromBusiness);
                            }
                            else
                            {
                                WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("ErrorException"), AppProperties.errorMessageFromBusiness);
                            }
                            IsEnabledFileds(true, true);
                            _isUAEVehicle = true;
                            ClearDataFields();

                            this.txtBoxOperatorName.Focus();
                            this.UpdateLayout();
                            AppProperties.vehicle = null;
                            //  ClearFields();
                            return;
                        }

                        if (null == AppProperties.vehicle || AppProperties.IsServiceResponseNull || AppProperties.NotFoundError)
                        {
                            App.VSDLog.Info(" \n ucVehicleSelection.GetVehicleProfileDetails(): vehicle data not found.");


                            if (!AppProperties.isNetworkConnected)
                            {
                                WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("lblOfflinDeviceMessage"), AppProperties.errorMessageFromBusiness);
                            }
                            else
                            {
                                WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("VehicleNotFound"), AppProperties.errorMessageFromBusiness);

                            } IsEnabledFileds(true, true);

                            this.UpdateLayout();
                            _IsShowKeyboard = false;
                            return;
                        }

                        //  AppProperties.vehicle.VehicleCategory = (string)this.vehicleCatTable[cmboxVehicleCategoty.Text];
                        // AppProperties.vehicle.VehicleCategoryAr = vehicleCatAr;
                        btnVehHistory.Visibility = Visibility.Visible;
                        btnVehDetails.Visibility = Visibility.Visible;

                        if (AppProperties.vehicle.Emirate.Equals(AppProperties.defaultEmirate, StringComparison.CurrentCultureIgnoreCase) && AppProperties.isOnline)
                        {

                            vsd.hh.utilities.AppProperties.routeFromRecordViolation = true;
                            // MessageBox.Show("Vehicle Information Found!!!");
                            EnableDisableVehicleInputDetails(false);
                            this.IsEnabledFileds(true, true);

                            this.ucVehicleProfileSelection_Loaded();
                            App.VSDLog.Info("AppProperties.vehicle.VehicleCategoryAr1 : " + AppProperties.vehicle.VehicleCategoryAr);
                            this.ChangeVehicleCategoryAsPerServiceResponse(AppProperties.vehicle.VehicleCategoryAr);
                            App.VSDLog.Info("AppProperties.vehicle.VehicleCategoryAr : " + AppProperties.vehicle.VehicleCategoryAr);
                            this.ChangeVehicleSubCategoryAsPerServiceResponse(AppProperties.vehicle.VehicleCategoryAr, AppProperties.vehicle.SubCategoryAr);
                            this.PopulateData_VehicelProfileInspection();
                            this.IsEnabledFileds(true, true);

                            this.UpdateLayout();


                            btnStartInspection.Focus();
                            _IsShowKeyboard = false;
                            //  this.cntControlVehcileSerachRecord.Content = this.m_mainWindow.m_PagesList[6];
                            return;

                        }
                        else
                        {
                            AppProperties.isOnline = true;


                            //EnableDisableVehicleInputDetails(false);



                            this.ucVehicleProfileSelection_Loaded();


                            vsd.hh.utilities.AppProperties.routeFromRecordViolation = true;
                            // MessageBox.Show("Vehicle Information Found!!!");
                            EnableDisableVehicleInputDetails(false);
                            this.IsEnabledFileds(true, true);

                            this.ucVehicleProfileSelection_Loaded();
                            this.ChangeVehicleCategoryAsPerServiceResponse(AppProperties.vehicle.VehicleCategoryAr);
                            App.VSDLog.Info("AppProperties.vehicle.VehicleCategoryAr : " + AppProperties.vehicle.VehicleCategoryAr);
                            this.ChangeVehicleSubCategoryAsPerServiceResponse(AppProperties.vehicle.VehicleCategoryAr, AppProperties.vehicle.SubCategoryAr);
                            this.PopulateData_VehicelProfileInspection();
                            this.IsEnabledFileds(true, true);

                            this.UpdateLayout();


                            btnStartInspection.Focus();
                            _IsShowKeyboard = false;
                            //  this.cntControlVehcileSerachRecord.Content = this.m_mainWindow.m_PagesList[6];
                            return;
                        }

                    }
                }

            }

            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        public void SearchVehicleFromEtrafficArHandler()
        {


            try
            {

                App.VSDLog.Info(" \n Entered in  ucVehicleSelection.SearchVehicleArHandler()");

                _iValidate = (IValidation)new RecordViolationInputArValidation();
                _validationResult = _iValidate.Validate(this);
                if (_validationResult != "Valid")
                {
                    WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), _validationResult);
                    return;
                }
                else
                {

                    // string vehicleCatAr = ((IVehicleProfile)VehicleProfileManager.GetInstance()).GetAlternateCategory(cmboxVehicleCategoty.SelectedValue.ToString(), _resources.GetLocale()).Trim();   
                    //  IVehicleProfile iVehicleProfile = ((IVehicleProfile)VehicleProfileManager.GetInstance());


                    //  string vehicleCatAr = iVehicleProfile.GetAlternateCategory(cmboxVehicleCategoty.SelectedValue.ToString(), "en-US");


                    //    ProgressDialogResult result = ProgressDialog.ProgressDialog.Execute(this.m_mainWindow, "Searching Vehicle...", (bw, we) =>
                    //{
                    string vehicleCatAr = ((IVehicleProfile)VehicleProfileManager.GetInstance()).GetAlternateCategory((string)this.vehicleCatTable[cmboxVehicleCategoty.Text], "ar-AE");
                    //    vehicleCatAr = vehicleCatAr;
                    //});


                    if (cmboxCountry.SelectedValue.ToString().Trim() != AppProperties.defaultCountryAr)
                    {
                        //No UAE Vehicle
                        App.VSDLog.Info(" \n Entered in  ucVehicleSelection.SearchVehicleArHandler(), Non UAE Vehicle Block");
                        if (AppProperties.vehicle == null)
                        {
                            AppProperties.vehicle = new Vehicle();
                        }
                        AppProperties.vehicle.Country = (string)this.countryTable[cmboxCountry.Text];

                        if (null == (string)this.emirateTable[cmboxEmirates.Text])
                        {
                            AppProperties.vehicle.Emirate = "";
                        }
                        else
                        {
                            AppProperties.vehicle.Emirate = (string)this.emirateTable[cmboxEmirates.Text];
                        }
                        if (cmboxPlateCode.Visibility == System.Windows.Visibility.Collapsed)
                        {
                            AppProperties.vehicle.PlateCode = txtPlateCode.Text;
                        }
                        else
                        {
                            AppProperties.vehicle.PlateCode = (string)this.plateCodeTable[cmboxPlateCode.Text];
                        }
                        if (null == (string)this.plateCatTable[cmboxPlateCategory.Text])
                        {
                            AppProperties.vehicle.PlateCategory = "";
                        }
                        else
                        {
                            AppProperties.vehicle.PlateCategory = (string)this.plateCatTable[cmboxPlateCategory.Text];
                        }


                        AppProperties.vehicle.PlateNumber = this.txtBoxPlateNumber.Text;
                        AppProperties.vehicle.VehicleCategory = (string)this.vehicleCatTable[cmboxVehicleCategoty.Text];
                        AppProperties.vehicle.VehicleCategoryAr = vehicleCatAr;
                        vsd.hh.utilities.AppProperties.routeFromRecordViolation = false;

                        //EnableDisableVehicleInputDetails(false);
                        // PopulateData_VehicelProfileInspection();
                        // btnStartInspection.Focus();
                        IsEnabledFileds(true, false);

                        // ucNonUAEVeh.txtBoxOperatorName.Focus();
                        this.UpdateLayout();



                    }
                    else
                    {
                        //UAE Vehcile

                        App.VSDLog.Info(" \n Entered in  ucVehicleSelection.SearchVehicleArHandler(), UAE Vehicle");
                        IVehicleProfile iVehicleProfile = ((IVehicleProfile)VehicleProfileManager.GetInstance());
                        string country = (string)this.countryTable[cmboxCountry.Text];
                        string source = (string)this.emirateTable[cmboxEmirates.Text];
                        string category = (string)this.plateCatTable[cmboxPlateCategory.Text];
                        string code = (string)this.plateCodeTable[cmboxPlateCode.Text];
                        string number = txtBoxPlateNumber.Text.Trim();
                        // ProgressDialogResult result2 = ProgressDialog.ProgressDialog.Execute(this.m_mainWindow, lblSearchingVehicle.Content.ToString(), (bw, we) =>
                        //{
                        //  iVehicleProfile.GetVehicleProfileDetails(country, source, category, number, code);
                        // });
                        ProgressDialogResult result = ProgressDialog.ProgressDialog.Execute(this.m_mainWindow, lblSearchingVehicle.Content.ToString(), (bw, we) =>
                        {
                            App.VSDLog.Info(" \n going to call iVehicleProfile.GetVehicleProfileDetails(country, source, category, number, code); in ucVehicleSelection.SearchVehicleArHandler()");
                            iVehicleProfile.GetVehicleProfileDetailsfromEtraffic(country, source, category, number, code);

                            // So this check in order to avoid default processing after the Cancel button has been pressed.
                            // This call will set the Cancelled flag on the result structure.
                            ProgressDialog.ProgressDialog.CheckForPendingCancellation(bw, we);

                        }, ProgressDialogSettings.WithSubLabelAndCancel);

                        if (result == null || result.Cancelled)
                            return;
                        else if (result.OperationFailed)
                            return;


                        if (AppProperties.businessError)
                        {
                            App.VSDLog.Info(" \n Business error in in ucVehicleSelection.SearchVehicleArHandler()");
                            AppProperties.vehicle = null;
                            AppProperties.recordedViolation = null;
                            AppProperties.recordedViolation = new Violation();
                            AppProperties.recordedViolation.InspectionArea = AppProperties.location;

                            WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("ErrorBusiness"), AppProperties.errorMessageFromBusiness);
                            AppProperties.businessError = false;
                            // busyIndicator.IsBusy = false;
                            //    LandingScreenEn landing = new LandingScreenEn();
                            //   _render.switchDisplay(form, landing);
                            //    this.m_mainWindow.MainContentControl.Content = this.m_mainWindow.m_PagesList[3];
                            return;
                        }
                        if (AppProperties.IsException)
                        {
                            App.VSDLog.Info(" \n Exception error in in ucVehicleSelection.SearchVehicleArHandler()");
                            AppProperties.IsException = false;
                            WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("ErrorException"), AppProperties.errorMessageFromBusiness);

                            IsEnabledFileds(true, true);
                            _isUAEVehicle = true;
                            ClearDataFields();

                            this.txtBoxOperatorName.Focus();
                            this.UpdateLayout();
                            AppProperties.vehicle = null;
                            //  ClearFields();
                            return;
                        }

                        if (null == AppProperties.vehicle)
                        {
                            App.VSDLog.Info(" \n Vehicle object is null in ucVehicleSelection.SearchVehicleArHandler()");
                            WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("VehicleNotFound"), AppProperties.errorMessageFromBusiness);

                            IsEnabledFileds(true, true);
                            this.UpdateLayout();
                            // this.m_mainWindow.MainContentControl.Content = this.m_mainWindow.m_PagesList[3];
                            //  this.m_mainWindow.MainContentControl.Content = new ucLocationSelectionEn(m_mainWindow);
                            return;
                        }
                        if (AppProperties.IsServiceResponseNull || AppProperties.NotFoundError)
                        {
                            App.VSDLog.Info(" \n ucVehicleSelection.GetVehicleProfileDetails(): vehicle data not found.");


                            WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("VehicleNotFound"), AppProperties.errorMessageFromBusiness);
                            IsEnabledFileds(true, true);

                            this.UpdateLayout();
                            _IsShowKeyboard = false;
                            return;
                        }
                        //  AppProperties.vehicle.VehicleCategory = (string)this.vehicleCatTable[cmboxVehicleCategoty.Text];
                        // AppProperties.vehicle.VehicleCategoryAr = vehicleCatAr;
                        btnVehHistory.Visibility = Visibility.Visible;
                        btnVehDetails.Visibility = Visibility.Visible;

                        if (AppProperties.vehicle.Emirate.Equals(AppProperties.defaultEmirate, StringComparison.CurrentCultureIgnoreCase) && AppProperties.isOnline)
                        {

                            vsd.hh.utilities.AppProperties.routeFromRecordViolation = true;
                            // MessageBox.Show("Vehicle Information Found!!!");
                            EnableDisableVehicleInputDetails(false);
                            this.IsEnabledFileds(true, true);

                            this.ucVehicleProfileSelection_Loaded();
                            this.ChangeVehicleCategoryAsPerServiceResponse(AppProperties.vehicle.VehicleCategoryAr);
                            this.ChangeVehicleSubCategoryAsPerServiceResponse(AppProperties.vehicle.VehicleCategoryAr, AppProperties.vehicle.SubCategoryAr);
                            this.PopulateData_VehicelProfileInspection();
                            this.IsEnabledFileds(true, true);

                            this.UpdateLayout();


                            btnStartInspection.Focus();
                            _IsShowKeyboard = false;
                            //  this.cntControlVehcileSerachRecord.Content = this.m_mainWindow.m_PagesList[6];
                            return;

                        }
                        else
                        {
                            AppProperties.isOnline = true;
                            vsd.hh.utilities.AppProperties.routeFromRecordViolation = false; ;

                            //EnableDisableVehicleInputDetails(false);




                            // MessageBox.Show("Vehicle Information Found!!!");
                            EnableDisableVehicleInputDetails(false);
                            this.IsEnabledFileds(true, true);

                            this.ucVehicleProfileSelection_Loaded();
                            this.ChangeVehicleCategoryAsPerServiceResponse(AppProperties.vehicle.VehicleCategoryAr);
                            App.VSDLog.Info("AppProperties.vehicle.VehicleCategoryAr : " + AppProperties.vehicle.VehicleCategoryAr);
                            this.ChangeVehicleSubCategoryAsPerServiceResponse(AppProperties.vehicle.VehicleCategoryAr, AppProperties.vehicle.SubCategoryAr);
                            this.PopulateData_VehicelProfileInspection();
                            this.IsEnabledFileds(true, true);

                            this.UpdateLayout();


                            btnStartInspection.Focus();
                            _IsShowKeyboard = false;
                            //  this.cntControlVehcileSerachRecord.Content = this.m_mainWindow.m_PagesList[6];
                            return;
                        }

                    }
                }

            }

            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }


        private void btnSearch_Click_1(object sender, RoutedEventArgs e)
        {

            if (AppProperties.Selected_Resource == "English")
            {
                if (AppProperties.Is_DeviceInspection)
                {
                    SearchVehicleFromEtraffiEnHandler();
                }
                else
                {
                    SearchVehicleEnHandler();
                }
            }
            else
            {
                if (AppProperties.Is_DeviceInspection)
                    SearchVehicleFromEtrafficArHandler();
                else
                    SearchVehicleArHandler();
            }

        }
        /// <summary>
        /// Reset Screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnResetVehicleRecord_Click_1(object sender, RoutedEventArgs e)
        {
            //EnableDisableVehicleInputDetails(true);

            SetScreenAsDefault();

        }
        public void ResetFiledsOnSlectionChanged()
        {
            IsEnabledFileds(false, false);
            //this.txtBoxPlateNumber.Text = "";
            this.txtPlateCode.Text = "";

            this.txtBoxOperatorName.Text = "";
            this.txtBoxChassisNumber.Text = "";
            this.txtModel.Text = "";
            this.txtYear.Text = "";
            this.txtBoxMake.Text = "";
            //this.lblRecomendation.Content = "";
            stckPanelVRR.Visibility = System.Windows.Visibility.Collapsed;
            lblVRR.Visibility = System.Windows.Visibility.Collapsed;






        }
        public void SetScreenAsDefault()
        {
            // cmboxCountry.SelectedValue = AppProperties.defaultCountry;
            //  cmboxEmirates.SelectedValue = AppProperties.defaultEmirate;

            txtBoxPlateNumber.Text = "";
            this.txtPlateCode.Text = "";



        }

        private void txtBoxPlateNumber_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            //  ResetFiledsOnSlectionChanged();
            System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.DigitSubstitution = System.Globalization.DigitShapes.None;
            this.UpdateLayout();
            try
            {
                new CommonUtils().validateTextInteger(sender, e);
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void txtBoxPlateNumber_GotFocus_1(object sender, RoutedEventArgs e)
        {
            CommonUtils.ShowKeyBoard();
            System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.DigitSubstitution = System.Globalization.DigitShapes.None;
            this.UpdateLayout();
        }

        private void txtBoxPlateNumber_LostFocus_1(object sender, RoutedEventArgs e)
        {
            CommonUtils.CLoseKeyBoard();
            if (!new CommonUtils().isWordFoundInList(_countryList, cmboxCountry.Text))
            {
                cmboxCountry.Text = "";
            }
            if (!new CommonUtils().isWordFoundInList(_emirateList, cmboxEmirates.Text))
            {
                cmboxEmirates.Text = "";
            }

            if (!new CommonUtils().isWordFoundInList(_plateCategoryList, cmboxPlateCategory.Text))
            {
                cmboxPlateCategory.Text = "";
            }
            if (!new CommonUtils().isWordFoundInList(_plateCodeList, cmboxPlateCode.Text))
            {
                cmboxPlateCode.Text = "";
            }
        }

        private void imagebtnNext_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {


        }

        private void UserControl_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {

            ChangeControlPosition();
            if (new CommonUtils().GetScreenOrientation() == AppProperties.LandScapetMode)
            {

                ChangeControlDimensions(265);
                ChangeButtonsDimensions(265);
                ChangeSmallButtonDimensions(125);

            }
            else
            {

                ChangeControlDimensions(265);
                ChangeButtonsDimensions(265);
                ChangeSmallButtonDimensions(125);


            }
            changeUcVehHistoryDialogControlSize();
        }

        public void ChangeSmallButtonDimensions(double width)
        {
            this.btnStartInspection.Width = width;
            this.btnBack.Width = width;
        }
        public void ChangeControlPosition()
        {
            if (new CommonUtils().GetScreenOrientation() == AppProperties.LandScapetMode)
            {

                grdVehSerch.Children.Remove(lblCountry);
                Grid.SetRow(lblCountry, 3);
                Grid.SetColumn(lblCountry, 1);
                grdVehSerch.Children.Add(lblCountry);

                grdVehSerch.Children.Remove(cmboxCountry);
                Grid.SetRow(cmboxCountry, 3);
                Grid.SetColumn(cmboxCountry, 3);
                grdVehSerch.Children.Add(cmboxCountry);

                grdVehSerch.Children.Remove(lblemirates);
                Grid.SetRow(lblemirates, 3);
                Grid.SetColumn(lblemirates, 5);
                grdVehSerch.Children.Add(lblemirates);

                grdVehSerch.Children.Remove(cmboxEmirates);
                Grid.SetRow(cmboxEmirates, 3);
                Grid.SetColumn(cmboxEmirates, 7);
                grdVehSerch.Children.Add(cmboxEmirates);

                grdVehSerch.Children.Remove(txtBoxEmirats);
                Grid.SetRow(txtBoxEmirats, 3);
                Grid.SetColumn(txtBoxEmirats, 7);
                grdVehSerch.Children.Add(txtBoxEmirats);

                grdVehSerch.Children.Remove(lblPlateCategory);
                Grid.SetRow(lblPlateCategory, 5);
                Grid.SetColumn(lblPlateCategory, 1);
                grdVehSerch.Children.Add(lblPlateCategory);

                grdVehSerch.Children.Remove(cmboxPlateCategory);
                Grid.SetRow(cmboxPlateCategory, 5);
                Grid.SetColumn(cmboxPlateCategory, 3);
                grdVehSerch.Children.Add(cmboxPlateCategory);

                grdVehSerch.Children.Remove(txtBoxPlateCategory);
                Grid.SetRow(txtBoxPlateCategory, 5);
                Grid.SetColumn(txtBoxPlateCategory, 3);
                grdVehSerch.Children.Add(txtBoxPlateCategory);

                grdVehSerch.Children.Remove(lblPlateNumber);
                Grid.SetRow(lblPlateNumber, 5);
                Grid.SetColumn(lblPlateNumber, 5);
                grdVehSerch.Children.Add(lblPlateNumber);

                grdVehSerch.Children.Remove(txtBoxPlateNumber);
                Grid.SetRow(txtBoxPlateNumber, 5);
                Grid.SetColumn(txtBoxPlateNumber, 7);
                grdVehSerch.Children.Add(txtBoxPlateNumber);

                grdVehSerch.Children.Remove(lblPlateCode);
                Grid.SetRow(lblPlateCode, 7);
                Grid.SetColumn(lblPlateCode, 1);
                grdVehSerch.Children.Add(lblPlateCode);

                grdVehSerch.Children.Remove(cmboxPlateCode);
                Grid.SetRow(cmboxPlateCode, 7);
                Grid.SetColumn(cmboxPlateCode, 3);
                grdVehSerch.Children.Add(cmboxPlateCode);

                grdVehSerch.Children.Remove(txtPlateCode);
                Grid.SetRow(txtPlateCode, 7);
                Grid.SetColumn(txtPlateCode, 3);
                grdVehSerch.Children.Add(txtPlateCode);



                grdVehSerch.Children.Remove(lblVehicleCategory);
                Grid.SetRow(lblVehicleCategory, 7);
                Grid.SetColumn(lblVehicleCategory, 5);
                grdVehSerch.Children.Add(lblVehicleCategory);

                grdVehSerch.Children.Remove(cmboxVehicleCategoty);
                Grid.SetRow(cmboxVehicleCategoty, 7);
                Grid.SetColumn(cmboxVehicleCategoty, 7);
                grdVehSerch.Children.Add(cmboxVehicleCategoty);



                grdVehSerch.Children.Remove(btnSearchVehicleRecord);
                Grid.SetRow(btnSearchVehicleRecord, 9);
                Grid.SetColumn(btnSearchVehicleRecord, 7);
                grdVehSerch.Children.Add(btnSearchVehicleRecord);

                ///////////////////////////////////////////////////////

                grdVehSerch.Children.Remove(stackPanleVehicleDetial);
                Grid.SetRow(stackPanleVehicleDetial, 11);
                Grid.SetColumn(stackPanleVehicleDetial, 1);
                Grid.SetColumnSpan(stackPanleVehicleDetial, 7);
                grdVehSerch.Children.Add(stackPanleVehicleDetial);



                grdVehSerch.Children.Remove(lblOprName);
                Grid.SetRow(lblOprName, 13);
                Grid.SetColumn(lblOprName, 1);
                grdVehSerch.Children.Add(lblOprName);

                grdVehSerch.Children.Remove(txtBoxOperatorName);
                Grid.SetRow(txtBoxOperatorName, 13);
                Grid.SetColumn(txtBoxOperatorName, 3);
                grdVehSerch.Children.Add(txtBoxOperatorName);

                grdVehSerch.Children.Remove(lblChassisNo);
                Grid.SetRow(lblChassisNo, 13);
                Grid.SetColumn(lblChassisNo, 5);
                grdVehSerch.Children.Add(lblChassisNo);

                grdVehSerch.Children.Remove(txtBoxChassisNumber);
                Grid.SetRow(txtBoxChassisNumber, 13);
                Grid.SetColumn(txtBoxChassisNumber, 7);
                grdVehSerch.Children.Add(txtBoxChassisNumber);

                grdVehSerch.Children.Remove(lblMake);
                Grid.SetRow(lblMake, 15);
                Grid.SetColumn(lblMake, 1);
                grdVehSerch.Children.Add(lblMake);

                grdVehSerch.Children.Remove(txtBoxMake);
                Grid.SetRow(txtBoxMake, 15);
                Grid.SetColumn(txtBoxMake, 3);
                grdVehSerch.Children.Add(txtBoxMake);

                grdVehSerch.Children.Remove(lblModel);
                Grid.SetRow(lblModel, 15);
                Grid.SetColumn(lblModel, 5);
                grdVehSerch.Children.Add(lblModel);

                grdVehSerch.Children.Remove(txtModel);
                Grid.SetRow(txtModel, 15);
                Grid.SetColumn(txtModel, 7);
                grdVehSerch.Children.Add(txtModel);

                grdVehSerch.Children.Remove(lblYear);
                Grid.SetRow(lblYear, 17);
                Grid.SetColumn(lblYear, 1);
                grdVehSerch.Children.Add(lblYear);

                grdVehSerch.Children.Remove(txtYear);
                Grid.SetRow(txtYear, 17);
                Grid.SetColumn(txtYear, 3);
                grdVehSerch.Children.Add(txtYear);


                grdVehSerch.Children.Remove(lblVRR);
                Grid.SetRow(lblVRR, 19);
                Grid.SetColumn(lblVRR, 1);
                grdVehSerch.Children.Add(lblVRR);

                grdVehSerch.Children.Remove(stckPanelVRR);
                Grid.SetRow(stckPanelVRR, 19);
                Grid.SetColumn(stckPanelVRR, 3);
                grdVehSerch.Children.Add(stckPanelVRR);



                grdVehSerch.Children.Remove(lblVehSubCat);
                Grid.SetRow(lblVehSubCat, 17);
                Grid.SetColumn(lblVehSubCat, 5);
                grdVehSerch.Children.Add(lblVehSubCat);

                grdVehSerch.Children.Remove(cmboxVehiclesubCategoty);
                Grid.SetRow(cmboxVehiclesubCategoty, 17);
                Grid.SetColumn(cmboxVehiclesubCategoty, 7);
                grdVehSerch.Children.Add(cmboxVehiclesubCategoty);



                //grdVehSerch.Children.Remove(lblRecomendation);
                //Grid.SetRow(lblRecomendation, 21);
                //Grid.SetColumn(lblRecomendation, 1);
                //Grid.SetColumnSpan(lblRecomendation, 3);
                //grdVehSerch.Children.Add(lblRecomendation);




                grdVehSerch.Children.Remove(btnStackePanel);
                Grid.SetRow(btnStackePanel, 19);
                Grid.SetColumn(btnStackePanel, 7);
                grdVehSerch.Children.Add(btnStackePanel);

            }
            else
            {
                grdVehSerch.Children.Remove(lblCountry);
                Grid.SetRow(lblCountry, 3);
                Grid.SetColumn(lblCountry, 1);
                grdVehSerch.Children.Add(lblCountry);

                grdVehSerch.Children.Remove(cmboxCountry);
                Grid.SetRow(cmboxCountry, 3);
                Grid.SetColumn(cmboxCountry, 3);
                grdVehSerch.Children.Add(cmboxCountry);

                grdVehSerch.Children.Remove(lblemirates);
                Grid.SetRow(lblemirates, 5);
                Grid.SetColumn(lblemirates, 1);
                grdVehSerch.Children.Add(lblemirates);

                grdVehSerch.Children.Remove(cmboxEmirates);
                Grid.SetRow(cmboxEmirates, 5);
                Grid.SetColumn(cmboxEmirates, 3);
                grdVehSerch.Children.Add(cmboxEmirates);

                grdVehSerch.Children.Remove(txtBoxEmirats);
                Grid.SetRow(txtBoxEmirats, 5);
                Grid.SetColumn(txtBoxEmirats, 3);
                grdVehSerch.Children.Add(txtBoxEmirats);

                grdVehSerch.Children.Remove(lblPlateCategory);
                Grid.SetRow(lblPlateCategory, 7);
                Grid.SetColumn(lblPlateCategory, 1);
                grdVehSerch.Children.Add(lblPlateCategory);

                grdVehSerch.Children.Remove(cmboxPlateCategory);
                Grid.SetRow(cmboxPlateCategory, 7);
                Grid.SetColumn(cmboxPlateCategory, 3);
                grdVehSerch.Children.Add(cmboxPlateCategory);

                grdVehSerch.Children.Remove(txtBoxPlateCategory);
                Grid.SetRow(txtBoxPlateCategory, 7);
                Grid.SetColumn(txtBoxPlateCategory, 3);
                grdVehSerch.Children.Add(txtBoxPlateCategory);

                grdVehSerch.Children.Remove(lblPlateNumber);
                Grid.SetRow(lblPlateNumber, 9);
                Grid.SetColumn(lblPlateNumber, 1);
                grdVehSerch.Children.Add(lblPlateNumber);

                grdVehSerch.Children.Remove(txtBoxPlateNumber);
                Grid.SetRow(txtBoxPlateNumber, 9);
                Grid.SetColumn(txtBoxPlateNumber, 3);
                grdVehSerch.Children.Add(txtBoxPlateNumber);

                grdVehSerch.Children.Remove(lblPlateCode);
                Grid.SetRow(lblPlateCode, 11);
                Grid.SetColumn(lblPlateCode, 1);
                grdVehSerch.Children.Add(lblPlateCode);

                grdVehSerch.Children.Remove(cmboxPlateCode);
                Grid.SetRow(cmboxPlateCode, 11);
                Grid.SetColumn(cmboxPlateCode, 3);
                grdVehSerch.Children.Add(cmboxPlateCode);

                grdVehSerch.Children.Remove(txtPlateCode);
                Grid.SetRow(txtPlateCode, 11);
                Grid.SetColumn(txtPlateCode, 3);
                grdVehSerch.Children.Add(txtPlateCode);



                grdVehSerch.Children.Remove(lblVehicleCategory);
                Grid.SetRow(lblVehicleCategory, 13);
                Grid.SetColumn(lblVehicleCategory, 1);
                grdVehSerch.Children.Add(lblVehicleCategory);

                grdVehSerch.Children.Remove(cmboxVehicleCategoty);
                Grid.SetRow(cmboxVehicleCategoty, 13);
                Grid.SetColumn(cmboxVehicleCategoty, 3);
                grdVehSerch.Children.Add(cmboxVehicleCategoty);



                grdVehSerch.Children.Remove(btnSearchVehicleRecord);
                Grid.SetRow(btnSearchVehicleRecord, 15);
                Grid.SetColumn(btnSearchVehicleRecord, 3);
                grdVehSerch.Children.Add(btnSearchVehicleRecord);

                ////////////////////////////////////////

                grdVehSerch.Children.Remove(stackPanleVehicleDetial);
                Grid.SetRow(stackPanleVehicleDetial, 17);
                Grid.SetColumn(stackPanleVehicleDetial, 1);
                Grid.SetColumnSpan(stackPanleVehicleDetial, 7);
                grdVehSerch.Children.Add(stackPanleVehicleDetial);



                grdVehSerch.Children.Remove(lblOprName);
                Grid.SetRow(lblOprName, 19);
                Grid.SetColumn(lblOprName, 1);
                grdVehSerch.Children.Add(lblOprName);

                grdVehSerch.Children.Remove(txtBoxOperatorName);
                Grid.SetRow(txtBoxOperatorName, 19);
                Grid.SetColumn(txtBoxOperatorName, 3);
                grdVehSerch.Children.Add(txtBoxOperatorName);

                grdVehSerch.Children.Remove(lblChassisNo);
                Grid.SetRow(lblChassisNo, 21);
                Grid.SetColumn(lblChassisNo, 1);
                grdVehSerch.Children.Add(lblChassisNo);

                grdVehSerch.Children.Remove(txtBoxChassisNumber);
                Grid.SetRow(txtBoxChassisNumber, 21);
                Grid.SetColumn(txtBoxChassisNumber, 3);
                grdVehSerch.Children.Add(txtBoxChassisNumber);

                grdVehSerch.Children.Remove(lblMake);
                Grid.SetRow(lblMake, 23);
                Grid.SetColumn(lblMake, 1);
                grdVehSerch.Children.Add(lblMake);

                grdVehSerch.Children.Remove(txtBoxMake);
                Grid.SetRow(txtBoxMake, 23);
                Grid.SetColumn(txtBoxMake, 3);
                grdVehSerch.Children.Add(txtBoxMake);

                grdVehSerch.Children.Remove(lblModel);
                Grid.SetRow(lblModel, 25);
                Grid.SetColumn(lblModel, 1);
                grdVehSerch.Children.Add(lblModel);

                grdVehSerch.Children.Remove(txtModel);
                Grid.SetRow(txtModel, 25);
                Grid.SetColumn(txtModel, 3);
                grdVehSerch.Children.Add(txtModel);

                grdVehSerch.Children.Remove(lblYear);
                Grid.SetRow(lblYear, 27);
                Grid.SetColumn(lblYear, 1);
                grdVehSerch.Children.Add(lblYear);

                grdVehSerch.Children.Remove(txtYear);
                Grid.SetRow(txtYear, 27);
                Grid.SetColumn(txtYear, 3);
                grdVehSerch.Children.Add(txtYear);


                grdVehSerch.Children.Remove(lblVRR);
                Grid.SetRow(lblVRR, 31);
                Grid.SetColumn(lblVRR, 1);
                grdVehSerch.Children.Add(lblVRR);

                grdVehSerch.Children.Remove(stckPanelVRR);
                Grid.SetRow(stckPanelVRR, 31);
                Grid.SetColumn(stckPanelVRR, 3);
                grdVehSerch.Children.Add(stckPanelVRR);


                grdVehSerch.Children.Remove(lblVehSubCat);
                Grid.SetRow(lblVehSubCat, 29);
                Grid.SetColumn(lblVehSubCat, 1);
                grdVehSerch.Children.Add(lblVehSubCat);

                grdVehSerch.Children.Remove(cmboxVehiclesubCategoty);
                Grid.SetRow(cmboxVehiclesubCategoty, 29);
                Grid.SetColumn(cmboxVehiclesubCategoty, 3);
                grdVehSerch.Children.Add(cmboxVehiclesubCategoty);



                //grdVehSerch.Children.Remove(lblRecomendation);
                //Grid.SetRow(lblRecomendation, 33);
                //Grid.SetColumn(lblRecomendation, 1);
                //grdVehSerch.Children.Add(lblRecomendation);




                grdVehSerch.Children.Remove(btnStackePanel);
                Grid.SetRow(btnStackePanel, 33);
                Grid.SetColumn(btnStackePanel, 3);
                grdVehSerch.Children.Add(btnStackePanel);
            }
        }
        public void ChangeControlDimensions(double width)
        {
            this.cmboxCountry.Width = width;
            this.cmboxEmirates.Width = width;
            this.cmboxPlateCategory.Width = width;
            this.cmboxPlateCode.Width = width;
            this.cmboxVehicleCategoty.Width = width;
            this.txtBoxPlateNumber.Width = width;
            this.txtBoxEmirats.Width = width;
            this.txtBoxPlateCategory.Width = width;
            this.txtPlateCode.Width = width;
            this.txtBoxOperatorName.Width = width;
            this.txtBoxChassisNumber.Width = width;
            this.txtModel.Width = width;

            this.stckPanelVRR.Width = width;
            this.txtYear.Width = width;
            this.txtBoxMake.Width = width;
            this.cmboxVehiclesubCategoty.Width = width;

            this.UpdateLayout();
        }
        public void ChangeButtonsDimensions(double width)
        {
            this.btnSearchVehicleRecord.Width = width;

            this.UpdateLayout();
        }
        public void ChangeLableDimensions(double width)
        {
            this.lblCountry.FontSize = width;
            this.lblemirates.FontSize = width;
            this.lblPlateCategory.FontSize = width;
            this.lblPlateCode.FontSize = width;
            this.lblPlateNumber.FontSize = width;
            this.lblVehicleCategory.FontSize = width;

            // this.lblAppLogout.FontSize = 20;
            this.UpdateLayout();
        }
        public void ChangeHeaderDimensions(double width)
        {
            this.lblVechSer.FontSize = width;
            this.lblVehicleDetailRecord.FontSize = width;
            this.UpdateLayout();
        }

        private void cmboxCountry_PreviewKeyDown_1(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Return)
                {
                    new CommonUtils().ChangeControlFocous(e);

                }
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void cmboxVehicleCategoty_PreviewKeyDown_1(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Return)
                {
                    btnSearch_Click_1(sender, e);
                    new CommonUtils().ChangeControlFocous(e);

                }
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void cmboxCountry_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (!cmboxCountry.IsDropDownOpen)
                cmboxCountry.IsDropDownOpen = true;
        }

        private void cmboxEmirates_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (!cmboxEmirates.IsDropDownOpen)
                cmboxEmirates.IsDropDownOpen = true;
        }

        private void cmboxPlateCategory_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (!cmboxPlateCategory.IsDropDownOpen)
                cmboxPlateCategory.IsDropDownOpen = true;
        }

        private void cmboxPlateCode_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (!cmboxPlateCode.IsDropDownOpen)
                cmboxPlateCode.IsDropDownOpen = true;
        }

        private void cmboxVehicleCategoty_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (!cmboxVehicleCategoty.IsDropDownOpen)
                cmboxVehicleCategoty.IsDropDownOpen = true;
        }
        //added by kashif abbasi dated 10-Dec-2015
        private void btnVehHistory_Click(object sender, RoutedEventArgs e)
        {
            dialogVehHistory.SetParent(MainGrid);
            dialogVehHistory.ShowHandlerDialog("");
        }
        private void btnVehDetails_Click(object sender, RoutedEventArgs e)
        {
            vehicledetails.SetParent(MainGrid);
            vehicledetails.ShowHandlerDialog("");
        }
        //added by kashif abbasi on dated 03-Dec-2016
        private void cmboxCountry_GotFocus_1(object sender, RoutedEventArgs e)
        {
            if (_IsShowKeyboard)
                CommonUtils.ShowKeyBoard();

            _IsShowKeyboard = true;
        }
        //added by kashif abbasi on dated 03-Dec-2016
        private void changeUcVehHistoryDialogControlSize()
        {
            if (new CommonUtils().GetScreenOrientation() == AppProperties.LandScapetMode)
            {
                dialogVehHistory.grdViolationDetails.Width = 700;
                dialogVehHistory.grdViolationDetails.Height = 400;
            }
            else
            {
                dialogVehHistory.grdViolationDetails.Width = 550;
                dialogVehHistory.grdViolationDetails.Height = 400;
            }
        }


        #region VehicleProfileSelection Work

        public void ucVehicleProfileSelection_Loaded()
        {
            try
            {
                ClearDataFields();
                // PopulateData_VehicelProfileInspection();

            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }
        public void ChangeVehicleCategoryAsPerServiceResponse(string vehicleCategory)
        {
            App.VSDLog.Info("ChangeVehicleCategoryAsPerServiceResponse : " + vehicleCategory + AppProperties.vehicle.VehicleCategoryAr);
            try
            {
                if (vehicleCategory != null && vehicleCategory != "" && vehicleCategory != "NA")
                {
                    App.VSDLog.Info("AppProperties.Selected_Resource : " + AppProperties.Selected_Resource);
                    if (AppProperties.Selected_Resource == "English")
                    {
                        if (_categoryList.Contains(vehicleCategory.Trim()))
                        {
                            if (vehicleCategory != cmboxVehicleCategoty.SelectedItem.ToString())
                            {
                                cmboxVehicleCategoty.SelectedItem = vehicleCategory;
                                AppProperties.vehicle.VehicleCategory = cmboxVehicleCategoty.SelectedItem.ToString();
                            }
                            // cmboxVehicleCategoty.SelectedItem = vehicleCategory;
                        }
                    }
                    else
                    {
                        if (vehicleCategory != cmboxVehicleCategoty.SelectedItem.ToString())
                        {
                            App.VSDLog.Info("vehicleCategory : " + vehicleCategory);
                            cmboxVehicleCategoty.SelectedItem = vehicleCategory;
                            AppProperties.vehicle.VehicleCategoryAr = cmboxVehicleCategoty.SelectedItem.ToString();
                            App.VSDLog.Info("changed vehicleCategory : " + AppProperties.vehicle.VehicleCategoryAr);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        #region SubcategoryUpdate
        public void ChangeVehicleSubCategoryAsPerServiceResponse(string vehicleCategory, string vehiclesubCategory)
        {
            App.VSDLog.Info("ChangeVehicleSubCategoryAsPerServiceResponse........." + vehicleCategory + " " + vehiclesubCategory);
            try
            {
                string SubcategoryName = string.Empty;
                if (vehicleCategory != null && vehicleCategory != "" && vehicleCategory != "NA")
                {
                    #region vehicleSubcategory

                    if (cmboxVehiclesubCategoty.Items.Count > 0)
                    {
                        cmboxVehiclesubCategoty.Items.Clear();
                    }
                    if (AppProperties.Selected_Resource == "Arabic")
                    {
                        vehiclesubCatTable = CommonUtils.Splitter(((IVehicleProfile)VehicleProfileManager.GetInstance()).GetVehicleSubCategories(vehicleCategory));
                        string[] vehCat = new string[vehiclesubCatTable.Count];
                        vehiclesubCatTable.Keys.CopyTo(vehCat, 0);
                        _subcategoryList = new List<string>(vehCat);
                        _subcategoryList.Sort();
                        App.VSDLog.Info("_subcategoryList.Count " + _subcategoryList.Count);
                        foreach (string str in _subcategoryList)
                        {
                            cmboxVehiclesubCategoty.Items.Add(str.Trim());
                        }
                        if (cmboxVehiclesubCategoty.Items.Count > 0)
                            cmboxVehiclesubCategoty.SelectedIndex = 0;

                        if (_subcategoryList.Count > 0)
                            cmboxVehiclesubCategoty.SelectedItem = _subcategoryList.First().Trim();
                        SubcategoryName = vehiclesubCategory;
                    }
                    else
                    {
                        SubcategoryName = ((IVehicleProfile)VehicleProfileManager.GetInstance()).GetVehicleSubCategoryName(vehiclesubCategory, vehicleCategory);
                        App.VSDLog.Info("SubcategoryName01 -" + SubcategoryName);
                        _subcategoryList = new List<string>(((IVehicleProfile)VehicleProfileManager.GetInstance()).GetVehicleSubCategories(vehicleCategory));
                        _subcategoryList.Sort();
                        foreach (string str in _subcategoryList)
                        {
                            cmboxVehiclesubCategoty.Items.Add(str.Trim());
                        }
                        if (cmboxVehiclesubCategoty.Items.Count > 0)
                            cmboxVehiclesubCategoty.SelectedIndex = 0;
                        if (_subcategoryList.Count > 0)
                            cmboxVehiclesubCategoty.SelectedItem = _subcategoryList.First().Trim();
                    }
                    if (_subcategoryList.Contains(SubcategoryName.Trim()))
                    {
                        App.VSDLog.Info("VehicleCategory");
                        cmboxVehiclesubCategoty.SelectedItem = SubcategoryName.Trim();
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public void PopulateData_VehicelProfileInspection()
        {
            try
            {
                App.VSDLog.Info("ucVehicle Selection PopulateData_VehicelProfileInspection()-----");
                if (AppProperties.vehicle != null)
                {
                    txtBoxChassisNumber.Text = (AppProperties.vehicle == null ? "" : AppProperties.vehicle.ChassisNumber);
                    txtBoxMake.Text = (AppProperties.vehicle == null ? "" : AppProperties.vehicle.Make);
                    txtModel.Text = (AppProperties.vehicle == null ? "" : AppProperties.vehicle.Model);
                    txtYear.Text = (AppProperties.vehicle == null ? "" : AppProperties.vehicle.Year);
                    if (AppProperties.vehicle != null)
                    {
                        if (AppProperties.Selected_Resource == "English")
                        {
                            if (AppProperties.vehicle.Operator != null)
                            {
                                if ((AppProperties.vehicle.Operator.OperatorName == null) || (AppProperties.vehicle.Operator.OperatorName == ""))
                                {
                                    txtBoxOperatorName.Text = AppProperties.vehicle.Operator.TrafficFileNumber;
                                }
                                else
                                {
                                    txtBoxOperatorName.Text = AppProperties.vehicle.Operator.OperatorName;
                                }
                            }
                        }
                        else
                        {
                            if (AppProperties.vehicle.Operator != null)
                            {
                                if ((AppProperties.vehicle.Operator.OperatorNameAr == null) || (AppProperties.vehicle.Operator.OperatorNameAr == ""))
                                {
                                    txtBoxOperatorName.Text = AppProperties.vehicle.Operator.TrafficFileNumber;
                                }
                                else
                                {
                                    txtBoxOperatorName.Text = AppProperties.vehicle.Operator.OperatorNameAr;
                                }
                            }
                        }

                    }
                    //txtBoxOperatorName.Text = (AppProperties.vehicle == null ? "" : (AppProperties.vehicle.Operator == null ? "" : (AppProperties.vehicle.Operator.OperatorName==null ? AppProperties.vehicle.Operator.TrafficFileNumber)));
                    // btnStartInspection.Content = (AppProperties.vehicle.Recomendation != "") ? "Confiscate" : "Inspect";
                    //  txtYear.Text = (AppProperties.vehicle == null ? "" : AppProperties.vehicle.Year);
                    PopulateOwnerVehicleInfo();
                    ShowVehicleRiskRatting();
                    PoplateVehicleRecomendation();
                    PopulateDeviceInspectionData();
                }
            }
            catch (Exception ex)
            {
                App.VSDLog.Info("PopulateData_VehicelProfileInspection() Exception");
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }
        private void PopulateDeviceInspectionData()
        {
            try
            {
                App.VSDLog.Info("PopulateDeviceInspectionData() -------------- ");

                App.VSDLog.Info("VehicleCategory =" + AppProperties.vehicle.VehicleCategory + "SubCategory = " + AppProperties.vehicle.SubCategoryAr);

                if (AppProperties.Is_DeviceInspection)
                {
                    lblVehSubCat.Visibility = System.Windows.Visibility.Visible;
                    cmboxVehiclesubCategoty.Visibility = System.Windows.Visibility.Visible;

                    lblVRR.Visibility = System.Windows.Visibility.Collapsed;
                    canv.Visibility = System.Windows.Visibility.Collapsed;
                    grdcanvas.Visibility = System.Windows.Visibility.Collapsed;

                    //if (AppProperties.Selected_Resource == "English")
                    //    this.txtVehSubCat.Text = AppProperties.vehicle.SubCategory;
                    //else
                    //    this.txtVehSubCat.Text = AppProperties.vehicle.SubCategoryAr;


                    if (AppProperties.Selected_Resource == "English")
                    {
                        ChangeVehicleSubCategoryAsPerServiceResponse(AppProperties.vehicle.VehicleCategory, AppProperties.vehicle.SubCategoryAr);
                    }
                    else
                    {
                        ChangeVehicleSubCategoryAsPerServiceResponse(AppProperties.vehicle.VehicleCategoryAr, AppProperties.vehicle.SubCategoryAr);
                    }



                }
                else
                {
                    lblVehSubCat.Visibility = System.Windows.Visibility.Visible;
                    cmboxVehiclesubCategoty.Visibility = System.Windows.Visibility.Visible;

                    //lblVehSubCat.Visibility = System.Windows.Visibility.Collapsed;
                    //txtVehSubCat.Visibility = System.Windows.Visibility.Collapsed;

                    lblVRR.Visibility = System.Windows.Visibility.Visible;
                    canv.Visibility = System.Windows.Visibility.Visible;
                    grdcanvas.Visibility = System.Windows.Visibility.Visible;


                    //ChangeVehicleSubCategoryAsPerServiceResponse(AppProperties.vehicle.VehicleCategory, AppProperties.vehicle.SubCategory);

                    //if (AppProperties.Selected_Resource == "English")
                    //    this.cmboxVehiclesubCategoty.Text = AppProperties.vehicle.SubCategory;
                    //else
                    //    this.cmboxVehiclesubCategoty.Text = AppProperties.vehicle.SubCategoryAr;

                    if (AppProperties.Selected_Resource == "English")
                    {
                        ChangeVehicleSubCategoryAsPerServiceResponse(AppProperties.vehicle.VehicleCategory, AppProperties.vehicle.SubCategoryAr);
                    }
                    else
                    {
                        ChangeVehicleSubCategoryAsPerServiceResponse(AppProperties.vehicle.VehicleCategoryAr, AppProperties.vehicle.SubCategoryAr);
                    }

                }

            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);

            }
        }
        /// <summary>
        /// Show Vehicle Risk Ratting with Coloring scheme
        /// </summary>
        private void ShowVehicleRiskRatting()
        {
            try
            {
                App.VSDLog.Info("ShowVehicleRiskRatting() -------------- ");
                if ((AppProperties.vehicle != null) && (AppProperties.vehicle.RiskRating != "") && (AppProperties.vehicle.RiskRating != null))
                {
                    string risk_rating = AppProperties.vehicle.RiskRating;

                    if (canv.Children.Count > 0)
                    {
                        canv.Children.RemoveAt(0);
                    }
                    canv.Visibility = System.Windows.Visibility.Visible;
                    this.grdcanvas.Visibility = System.Windows.Visibility.Visible;
                    lblRattingTextwithColor.Visibility = System.Windows.Visibility.Visible;
                    //lblDRR.Visibility = System.Windows.Visibility.Visible;
                    lblRattingTextwithColor.Content = risk_rating;
                    System.Windows.Shapes.Ellipse rect;
                    rect = new System.Windows.Shapes.Ellipse();
                    rect.StrokeThickness = 2;
                    if (risk_rating.Contains("G"))
                    {
                        rect.Stroke = new SolidColorBrush(Colors.ForestGreen);
                        rect.Fill = new SolidColorBrush(Colors.ForestGreen);
                        rect.Width = 50;
                        rect.Height = 40;
                        Canvas.SetLeft(rect, 0);
                        Canvas.SetTop(rect, 0);
                        canv.Children.Add(rect);
                    }
                    else if (risk_rating.Contains("A"))
                    {
                        // rect.Stroke = new SolidColorBrush(Colors.Yellow);
                        rect.Stroke = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFCC00"));
                        rect.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFCC00"));
                        rect.Width = 50;
                        rect.Height = 40;
                        Canvas.SetLeft(rect, 0);
                        Canvas.SetTop(rect, 0);
                        canv.Children.Add(rect);
                    }
                    else if (risk_rating.Contains("R"))
                    {
                        rect.Stroke = new SolidColorBrush(Colors.Red);

                        rect.Fill = new SolidColorBrush(Colors.Red);
                        rect.Width = 50;
                        rect.Height = 40;
                        Canvas.SetLeft(rect, 0);
                        Canvas.SetTop(rect, 0);
                        canv.Children.Add(rect);
                    }
                    else if (risk_rating.Contains("New"))
                    {
                        rect.Stroke = new SolidColorBrush(Colors.Gray);
                        // new SolidColorBrush(Colors.Gray);
                        rect.Fill = new SolidColorBrush(Colors.Gray);
                        rect.Width = 50;
                        rect.Height = 40;
                        Canvas.SetLeft(rect, 0);
                        Canvas.SetTop(rect, 0);
                        canv.Children.Add(rect);
                    }
                    else
                    {
                        canv.Visibility = System.Windows.Visibility.Collapsed;
                        grdcanvas.Visibility = System.Windows.Visibility.Collapsed;
                        lblRattingTextwithColor.Visibility = System.Windows.Visibility.Collapsed;
                        // lblDRR.Visibility = System.Windows.Visibility.Collapsed;
                        AppProperties.vehicle.RiskRating = "";
                    }
                }
                else
                {
                    canv.Visibility = System.Windows.Visibility.Collapsed;
                    grdcanvas.Visibility = System.Windows.Visibility.Collapsed;
                    lblRattingTextwithColor.Visibility = System.Windows.Visibility.Collapsed;
                    lblVRR.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(" ShowVehicleRiskRatting() Exception");
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Populate Recomendations on Intrested Vehicles
        /// </summary>
        private void PoplateVehicleRecomendation()
        {
            try
            {
                App.VSDLog.Info("PoplateVehicleRecomendation()-----------");
                List<string> vehicle_intered;
                String chassisNo_SearchedVehicle = (AppProperties.vehicle == null ? "" : AppProperties.vehicle.ChassisNumber);
                is_intrested = false;

                vehicle_intered = new List<string>(((IDBDataLoad)DBDataLoadManager.GetInstance()).GetInterestListVehicle());

                if (vehicle_intered.Count > 0)
                {
                    foreach (string str in vehicle_intered)
                    {
                        if (str == chassisNo_SearchedVehicle)
                            is_intrested = true;
                    }
                }
                if (is_intrested)
                {
                    //lblRecomendation.Content = new CommonUtils().GetStringValue("InspectionRecomended");
                    //lblRecomendation.Foreground = Brushes.Red;
                    //lblRecomendation.FontWeight = FontWeights.Medium;
                }
                else
                {
                    //lblRecomendation.Content = new CommonUtils().GetStringValue("InspectionNotRecomended");
                    //lblRecomendation.Foreground = Brushes.Green;
                    //lblRecomendation.FontWeight = FontWeights.Bold;
                }
                if (timer1 == null)
                {

                    timer1 = new System.Timers.Timer();
                    timer1.Elapsed += timer1_Tick;
                    timer1.Interval = 650;
                    timer1.Start();
                }
            }
            catch (Exception ex)
            {
                App.VSDLog.Info("PoplateVehicleRecomendation() Exception");
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Dispatcher.Invoke(
                           new blink_inspectionrecomd_delegate(this.blinkLabel),
                           new object[] { }
                       );

        }
        public void blinkLabel()
        {
            //if (is_intrested)
            //{
            //    if (this.lblRecomendation.Foreground == Brushes.Red)
            //    {
            //        this.lblRecomendation.Foreground = Brushes.White;
            //    }
            //    else
            //    {
            //        this.lblRecomendation.Foreground = Brushes.Red;
            //    }
            //}
            //else
            //{
            //    if (this.lblRecomendation.Foreground == Brushes.Green)
            //    {
            //        this.lblRecomendation.Foreground = Brushes.White;
            //    }
            //    else
            //    {
            //        this.lblRecomendation.Foreground = Brushes.Green;
            //    }
            //}
        }
        public void IsEnabledFileds(bool IsEnabled, bool isUAEVehicle)
        {
            //IsReadonly= true mean IsEnaled=false and vice versa
            bool IsReadOnly = !IsEnabled;
            txtBoxChassisNumber.IsReadOnly = IsReadOnly;
            txtBoxMake.IsReadOnly = IsReadOnly;
            txtBoxOperatorName.IsReadOnly = IsReadOnly;
            txtModel.IsReadOnly = IsReadOnly;
            txtYear.IsReadOnly = IsReadOnly;
            cmboxVehiclesubCategoty.IsReadOnly = IsReadOnly;




            // imagebtnNext.IsEnabled = !IsEnabled;
            //_isUAEVehicle = isUAEVehicle;

            if (IsEnabled == false)
            {
                txtBoxChassisNumber.Background = Brushes.Gray;
                txtBoxMake.Background = Brushes.Gray;
                txtBoxOperatorName.Background = Brushes.Gray;
                txtModel.Background = Brushes.Gray;

                txtYear.Background = Brushes.Gray;
                // txtRecomendation.Background = Brushes.Gray;
                //lblRecomendation.Visibility = System.Windows.Visibility.Collapsed;
                //lblRecomendation.Visibility = System.Windows.Visibility.Collapsed;
                lblVRR.Visibility = System.Windows.Visibility.Collapsed;
                canv.Visibility = Visibility.Collapsed;
                stckPanelVRR.Visibility = System.Windows.Visibility.Collapsed;
                lblVRR.Visibility = System.Windows.Visibility.Collapsed;

                if (AppProperties.Is_DeviceInspection)
                {
                    this.cmboxVehiclesubCategoty.Visibility = System.Windows.Visibility.Visible;
                    this.lblVehSubCat.Visibility = System.Windows.Visibility.Visible;
                    cmboxVehiclesubCategoty.Background = Brushes.Gray;
                }
                else
                {
                    this.cmboxVehiclesubCategoty.Visibility = System.Windows.Visibility.Visible;
                    this.lblVehSubCat.Visibility = System.Windows.Visibility.Visible;
                    cmboxVehiclesubCategoty.Background = Brushes.Gray;
                }



            }
            else
            {
                txtBoxChassisNumber.Background = Brushes.White;
                txtBoxMake.Background = Brushes.White;
                txtBoxOperatorName.Background = Brushes.White;
                txtModel.Background = Brushes.White;

                txtYear.Background = Brushes.White;
                //  txtRecomendation.Background = Brushes.White;
                // txtRecomendation.Visibility = System.Windows.Visibility.Visible;


                //lblRecomendation.Visibility = System.Windows.Visibility.Visible;
                lblVRR.Visibility = System.Windows.Visibility.Visible;
                canv.Visibility = Visibility.Visible;
                stckPanelVRR.Visibility = System.Windows.Visibility.Visible;
                lblVRR.Visibility = System.Windows.Visibility.Visible;

                if (AppProperties.Is_DeviceInspection)
                {
                    cmboxVehiclesubCategoty.Background = Brushes.White;

                    this.cmboxVehiclesubCategoty.Visibility = System.Windows.Visibility.Visible;
                    this.lblVehSubCat.Visibility = System.Windows.Visibility.Visible;

                    canv.Visibility = Visibility.Collapsed;
                    stckPanelVRR.Visibility = System.Windows.Visibility.Collapsed;
                    lblVRR.Visibility = System.Windows.Visibility.Collapsed;

                }
                else
                {
                    cmboxVehiclesubCategoty.Background = Brushes.White;

                    this.cmboxVehiclesubCategoty.Visibility = System.Windows.Visibility.Visible;
                    this.lblVehSubCat.Visibility = System.Windows.Visibility.Visible;
                }

            }
        }



        private void PopulateOwnerVehicleInfo()
        {
            try
            {
                App.VSDLog.Info("PopulateOwnerVehicleInfo() -------------- ");
                vsd.hh.data.Vehicle VehicleToInspect;
                VehicleToInspect = AppProperties.vehicle;

                if (AppProperties.vehicle != null)
                {

                    //  txtInspectionInstructions.Text = VehicleToInspect.Instruction;
                    //txtOvrrScore.Text = VehicleToInspect.VehicleOVRRScore;
                    //txtRecomendations.Text = VehicleToInspect.Recomendation;
                }


                openViolations = new List<VSDApp.com.rta.vsd.hh.data.Violation>();
                if (AppProperties.vehicle != null && AppProperties.vehicle.Violations != null)
                {
                    foreach (vsd.hh.data.Violation x in VehicleToInspect.Violations)
                    {
                        if (x.ViolationStatus.StartsWith("Open", StringComparison.CurrentCultureIgnoreCase)) if (x.ViolationStatus.StartsWith("Open", StringComparison.CurrentCultureIgnoreCase))
                            {
                                openViolations.Add(x);
                            }
                    }
                }


            }
            catch (Exception ex)
            {
                App.VSDLog.Info("PopulateOwnerVehicleInfo() -------------- Exception");
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }



        }

        public void ClearDataFields()
        {
            //this.lblRecomendation.Content = "";
            // this.txtOvrrScore.Text = "";
            this.txtModel.Text = "";

            this.txtYear.Text = "";

            this.txtBoxOperatorName.Text = "";
            this.txtBoxMake.Text = "";
            this.txtBoxChassisNumber.Text = "";
            //this.cmboxVehiclesubCategoty.Text = "";
            //  this.txtRecomendation.Text = "";
            if (openViolations != null)
                openViolations.Clear();
            canv.Visibility = System.Windows.Visibility.Collapsed;
            grdcanvas.Visibility = System.Windows.Visibility.Collapsed;
            if (AppProperties.Is_DeviceInspection)
            {
                lblVehSubCat.Visibility = System.Windows.Visibility.Visible;
                cmboxVehiclesubCategoty.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                lblVehSubCat.Visibility = System.Windows.Visibility.Visible;
                cmboxVehiclesubCategoty.Visibility = System.Windows.Visibility.Visible;

                //lblVehSubCat.Visibility = System.Windows.Visibility.Collapsed;
                //txtVehSubCat.Visibility = System.Windows.Visibility.Collapsed;
            }
            UpdateLayout();

        }

        private void txtBoxOperatorName_GotFocus_1(object sender, RoutedEventArgs e)
        {
            CommonUtils.ShowKeyBoard();
        }

        private void txtBoxOperatorName_LostFocus_1(object sender, RoutedEventArgs e)
        {
            CommonUtils.CLoseKeyBoard();
        }

        private void txtBoxOperatorName_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            try
            {
                new CommonUtils().validateTextCharacter(sender, e);
                System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.DigitSubstitution = System.Globalization.DigitShapes.None;
                this.UpdateLayout();
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void txtBoxOperatorName_PreviewKeyDown_1(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Return)
                {
                    new CommonUtils().ChangeControlFocous(e);

                }
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
            System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.DigitSubstitution = System.Globalization.DigitShapes.None;
            this.UpdateLayout();
        }

        private void txtYear_PreviewKeyDown_1(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Return)
                {
                    btnStartInspection_Click_1(sender, e);

                }
                System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.DigitSubstitution = System.Globalization.DigitShapes.None;
                this.UpdateLayout();
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        public bool IsDefectExistForVehCat(string vehicleCat)
        {
            try
            {
                List<string> defectNameList = new List<string>(((IViolation)ViolationManager.GetInstance()).GetDefects("Brake System", " ", "Defect"));

                if (defectNameList != null && defectNameList.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
                // return true;
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
                return false;
            }


        }

        private void btnStartInspection_Click_1(object sender, RoutedEventArgs e)
         {
            try
            {
                string selected_Country = string.Empty;
                if (AppProperties.Selected_Resource == "Arabic")
                {
                    selected_Country = (string)countryTable[(string)cmboxCountry.SelectedItem];
                    if (selected_Country.Equals(AppProperties.defaultCountry))
                    {
                        _isUAEVehicle = true;
                    }
                    else
                    {
                        _isUAEVehicle = false;
                    }
                    _iValidate = (IValidation)new VehicleProfileInspectionArValidation();
                }
                else
                {
                    selected_Country = Convert.ToString(cmboxCountry.SelectedValue);
                    if (selected_Country.Equals(AppProperties.defaultCountry))
                        _isUAEVehicle = true;
                    else
                        _isUAEVehicle = false;

                    _iValidate = (IValidation)new VehicleProfileInspectionEnValidation();
                }


                if (_isUAEVehicle)
                {

                    if (this.txtBoxPlateNumber.Text.Trim().Equals(""))
                    {
                        if (AppProperties.Selected_Resource.ToString().Equals("English"))
                            WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), "Plate number not entered");
                        else
                            WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), "يرجى إدخال رقم اللوحة");
                        return;
                    }
                }
                else
                {
                    if (this.txtBoxPlateNumber.Text.Trim().Equals(""))
                    {
                        if (AppProperties.Selected_Resource.ToString().Equals("English"))
                            WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), "Plate number not entered");
                        else
                            WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), "يرجى إدخال رقم اللوحة");
                        return;
                    }

                }


                _validationResult = _iValidate.Validate(this);
                this.txtBoxOperatorName.Text = this.txtBoxOperatorName.Text;

                if (_validationResult != "Valid" && _isUAEVehicle == true)
                {
                    WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), _validationResult);
                    return;
                }
                else
                {

                    //////////////////
                    if (AppProperties.vehicle == null)
                    {
                        AppProperties.vehicle = new Vehicle();
                        if (AppProperties.Selected_Resource == "Arabic")
                        {
                            AppProperties.vehicle.Country = (string)this.countryTable[this.cmboxCountry.SelectedValue.ToString()];
                            if (null == (string)this.cmboxEmirates.SelectedItem)
                            {
                                AppProperties.vehicle.Emirate = "";
                            }
                            else
                            {
                                AppProperties.vehicle.Emirate = (string)this.emirateTable[this.cmboxEmirates.SelectedValue.ToString()];
                            }
                            if (this.cmboxPlateCode.Visibility == System.Windows.Visibility.Collapsed)
                            {
                                AppProperties.vehicle.PlateCode = this.txtPlateCode.Text;
                            }
                            else
                            {
                                AppProperties.vehicle.PlateCode = (string)this.plateCatTable[this.cmboxPlateCategory.SelectedValue.ToString()];
                            }
                            if (null == (string)this.cmboxPlateCategory.SelectedItem)
                            {
                                AppProperties.vehicle.PlateCategory = "";
                            }
                            else
                            {
                                AppProperties.vehicle.PlateCategory = (string)this.plateCatTable[this.cmboxPlateCategory.SelectedValue.ToString()];
                            }
                            //   vehicleSelection.cmboxCountry.SelectedValue.ToString();
                        }
                        else
                        {
                            AppProperties.vehicle.Country = this.cmboxCountry.SelectedValue.ToString();
                            if (null == (string)this.cmboxEmirates.SelectedItem)
                            {
                                AppProperties.vehicle.Emirate = "";
                            }
                            else
                            {
                                AppProperties.vehicle.Emirate = this.cmboxEmirates.SelectedValue.ToString();
                            }
                            if (this.cmboxPlateCode.Visibility == System.Windows.Visibility.Collapsed)
                            {
                                AppProperties.vehicle.PlateCode = this.txtPlateCode.Text;
                            }
                            else
                            {
                                AppProperties.vehicle.PlateCode = this.cmboxPlateCode.SelectedValue.ToString();
                            }
                            if (null == (string)this.cmboxPlateCategory.SelectedItem)
                            {
                                AppProperties.vehicle.PlateCategory = "";
                            }
                            else
                            {
                                AppProperties.vehicle.PlateCategory = this.cmboxPlateCategory.SelectedValue.ToString();
                            }
                        }




                        AppProperties.vehicle.PlateNumber = this.txtBoxPlateNumber.Text;


                        string vehicleCatAr = string.Empty;
                        if (AppProperties.Selected_Resource == "Arabic")
                        {
                            vehicleCatAr = ((IVehicleProfile)VehicleProfileManager.GetInstance()).GetAlternateCategory((string)this.vehicleCatTable[this.cmboxVehicleCategoty.Text], "ar-AE");
                            AppProperties.vehicle.VehicleCategory = (string)this.vehicleCatTable[this.cmboxVehicleCategoty.Text];
                        }

                        else
                        {
                            //    vehicleCatAr = vehicleCatAr;
                            //});
                            vehicleCatAr = ((IVehicleProfile)VehicleProfileManager.GetInstance()).GetAlternateCategory(this.cmboxVehicleCategoty.SelectedValue.ToString(), "en-US");
                            AppProperties.vehicle.VehicleCategory = this.cmboxVehicleCategoty.SelectedValue.ToString();
                        }


                        // string vehicleCatAr = ((IVehicleProfile)VehicleProfileManager.GetInstance()).GetAlternateCategory(vehicleSelection.cmboxVehicleCategoty.SelectedValue.ToString(), "en-US");
                        AppProperties.vehicle.VehicleCategoryAr = vehicleCatAr;



                        string vehiclesubCatAr = string.Empty;
                        if (AppProperties.Selected_Resource == "Arabic")
                        {
                           // vehiclesubCatAr = ((IVehicleProfile)VehicleProfileManager.GetInstance()).GetAlternateCategory((string)this.vehiclesubCatTable[this.cmboxVehiclesubCategoty.Text], "ar-AE");
                           // vehiclesubCatAr = ((IVehicleProfile)VehicleProfileManager.GetInstance()).GetAlternateVehicleSubCategory((string)this.vehiclesubCatTable[this.cmboxVehiclesubCategoty.Text], (string)this.vehicleCatTable[this.cmboxVehicleCategoty.Text]);
                            vehiclesubCatAr = (string)this.vehiclesubCatTable[this.cmboxVehiclesubCategoty.Text];
                            AppProperties.vehicle.SubCategoryAr = this.cmboxVehiclesubCategoty.SelectedValue.ToString();
                        }

                        else
                        {
                            //    vehicleCatAr = vehicleCatAr;
                            //});
                            App.VSDLog.Info("Reached here "); 
                            App.VSDLog.Info("selected -"+ vehiclesubCatTable[this.cmboxVehiclesubCategoty.Text]);
                            //vehiclesubCatAr = ((IVehicleProfile)VehicleProfileManager.GetInstance()).GetAlternateVehicleSubCategory((string)this.vehiclesubCatTable[this.cmboxVehiclesubCategoty.Text], (string)this.vehicleCatTable[this.cmboxVehicleCategoty.Text]);
                            vehiclesubCatAr = ((IVehicleProfile)VehicleProfileManager.GetInstance()).GetAlternateVehicleSubCategory(this.cmboxVehiclesubCategoty.SelectedValue.ToString(), this.cmboxVehicleCategoty.SelectedValue.ToString());
                            AppProperties.vehicle.SubCategoryAr = vehiclesubCatAr;
                        }


                        // string vehicleCatAr = ((IVehicleProfile)VehicleProfileManager.GetInstance()).GetAlternateCategory(vehicleSelection.cmboxVehicleCategoty.SelectedValue.ToString(), "en-US");
                        AppProperties.vehicle.SubCategoryAr = vehicleCatAr;
                    }





                    ////////////////////
                    if (AppProperties.vehicle.Operator == null)
                    {
                        AppProperties.vehicle.Operator = new Operator();
                    }
                    AppProperties.vehicle.Operator.OperatorName = txtBoxOperatorName.Text;
                    AppProperties.vehicle.ChassisNumber = txtBoxChassisNumber.Text;
                    AppProperties.vehicle.Make = txtBoxMake.Text;
                    AppProperties.vehicle.Model = txtModel.Text;
                    AppProperties.vehicle.Year = txtYear.Text;
                    string vehicleCatAr2 = string.Empty;
                    if (AppProperties.Selected_Resource == "Arabic")
                    {

                        AppProperties.vehicle.VehicleCategoryAr = ((IVehicleProfile)VehicleProfileManager.GetInstance()).GetAlternateCategory((string)this.vehicleCatTable[this.cmboxVehicleCategoty.Text], "ar-AE");
                        AppProperties.vehicle.VehicleCategory = (string)this.vehicleCatTable[this.cmboxVehicleCategoty.Text];
                    }

                    else
                    {
                        //    vehicleCatAr = vehicleCatAr;
                        //});
                        // vehicleCatAr2 = ((IVehicleProfile)VehicleProfileManager.GetInstance()).GetAlternateCategory(this.cmboxVehicleCategoty.SelectedValue.ToString(), "en-US");
                        AppProperties.vehicle.VehicleCategory = this.cmboxVehicleCategoty.SelectedValue.ToString();
                    }


                    string vehiclesubCatAr2 = string.Empty;
                    if (AppProperties.Selected_Resource == "Arabic")
                    {
                       // vehiclesubCatAr2 = ((IVehicleProfile)VehicleProfileManager.GetInstance()).GetAlternateCategory((string)this.vehiclesubCatTable[this.cmboxVehiclesubCategoty.Text], "ar-AE");
                        //vehiclesubCatAr2 = ((IVehicleProfile)VehicleProfileManager.GetInstance()).GetAlternateVehicleSubCategory((string)this.vehiclesubCatTable[this.cmboxVehiclesubCategoty.Text], (string)this.vehicleCatTable[this.cmboxVehicleCategoty.Text]);
                        //AppProperties.vehicle.SubCategoryAr = (string)this.vehiclesubCatTable[this.cmboxVehiclesubCategoty.Text];

                        vehiclesubCatAr2 = (string)this.vehiclesubCatTable[this.cmboxVehiclesubCategoty.Text];
                        AppProperties.vehicle.SubCategoryAr = this.cmboxVehiclesubCategoty.SelectedValue.ToString();
                    }

                    else
                    {
                        //    vehicleCatAr = vehicleCatAr;
                        //});
                        //vehiclesubCatAr2 = ((IVehicleProfile)VehicleProfileManager.GetInstance()).GetAlternateCategory(this.cmboxVehiclesubCategoty.SelectedValue.ToString(), "en-US");cmboxVehiclesubCategoty
                        //AppProperties.vehicle.SubCategoryAr = this.cmboxVehiclesubCategoty.SelectedValue.ToString();

                        vehiclesubCatAr2 = ((IVehicleProfile)VehicleProfileManager.GetInstance()).GetAlternateVehicleSubCategory(this.cmboxVehiclesubCategoty.SelectedValue.ToString(), this.cmboxVehicleCategoty.SelectedValue.ToString());
                        AppProperties.vehicle.SubCategoryAr = vehiclesubCatAr2;
                    }
                    /// Check If selcted Vehicle Category have defect
                    /// 

                    if (AppProperties.Is_DeviceInspection)
                    {
                        this.m_mainWindow.MainContentControl.Content = null;
                        this.m_mainWindow.MainContentControl.Content = new ucDriverInspectionDefectScreen(this.m_mainWindow);
                        return;
                    }
                    else
                    {

                        if (!IsDefectExistForVehCat(AppProperties.vehicle.VehicleCategory))
                        {
                            if (WPFMessageBox.Show(new CommonUtils().GetStringValue("Confirmation"), new CommonUtils().GetStringValue("lblVehCatDefectConfirmation"), WPFMessageBoxButtons.YesNo, WPFMessageBoxImage.Question) == WPFMessageBoxResult.Yes)
                            {
                                this.m_mainWindow.MainContentControl.Content = null;
                                this.m_mainWindow.MainContentControl.Content = new ucDefectAndViolationDetails(this.m_mainWindow);
                                return;
                            }
                        }
                        else
                        {
                            this.m_mainWindow.MainContentControl.Content = null;
                            this.m_mainWindow.MainContentControl.Content = new ucDefectAndViolationDetails(this.m_mainWindow);
                            return;
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);

            }
        }

        private void btnBack_Click_1(object sender, RoutedEventArgs e)
        {

            this.m_mainWindow.MainContentControl.Content = null;
            //this.m_MainWindow.MainContentControl.Content = new ucLocationSelectionEn(m_MainWindow);
            this.m_mainWindow.MainContentControl.Content = new ucLocationSelectionEn(m_mainWindow);
        }
        #endregion


    }
}
