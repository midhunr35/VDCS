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
    /// Interaction logic for ucSearchVehicle.xaml
    /// </summary>
    public partial class ucSearchVehicle : UserControl
    {
        MainWindow m_mainWindow = null;
        bool m_IsDataLoaded = false;
        private IValidation _iValidate;
        private string _validationResult;


        public Hashtable countryTable = new Hashtable();
        public Hashtable emirateTable = new Hashtable();
        public Hashtable plateCatTable = new Hashtable();
        public Hashtable plateCodeTable = new Hashtable();
        public Hashtable vehicleCatTable = new Hashtable();

        private List<string> _countryList = new List<string>();
        private List<string> _emirateList = new List<string>();
        private List<string> _plateCategoryList = new List<string>();
        private List<string> _plateCodeList = new List<string>();

        public ucSearchVehicle(MainWindow mainWndw)
        {
            InitializeComponent();
            this.m_mainWindow = mainWndw;
        }
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
                if (AppProperties.Selected_Resource == "Arabic")
                {
                    try
                    {
                        countryTable = CommonUtils.Splitter(((IViolation)ViolationManager.GetInstance()).GetLocation(null, null));
                    }
                    catch (Exception ex)
                    {
                        // App.VSDLog.Info(e.StackTrace);
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

            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }
        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            if (AppProperties.Selected_Resource == "English")
            {
                imagebtnBack.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Back.png", UriKind.Relative));
                imagebtnSearch.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Search.png", UriKind.Relative));
            }
            else
            {
                imagebtnBack.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Back Arabic Up.png", UriKind.Relative));
                imagebtnSearch.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Search Arabic Up.png", UriKind.Relative));
            }

            AppProperties.vehicle = null;
            AppProperties.recordedViolation = null;
            AppProperties.previousDefectIDs = new List<int>();
            AppProperties.recordedViolation = new Violation();
            AppProperties.recordedViolation.InspectionArea = AppProperties.location;
            AppProperties.confiscatePlates = false;
            if (AppProperties.Selected_Resource == "Arabic")
            {
                btnStackePanel.FlowDirection = System.Windows.FlowDirection.LeftToRight;
            }
            PopulateData();

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
                        }
                        else
                        {
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
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
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
                            plateCatTable = new Hashtable();
                            plateCatTable = CommonUtils.Splitter(((IVehicleProfile)VehicleProfileManager.GetInstance()).GetVehiclePlateCategories((string)emirateTable[(string)cmboxEmirates.SelectedItem]));
                            string[] category = new string[plateCatTable.Count];
                            plateCatTable.Keys.CopyTo(category, 0);

                            _plateCategoryList = new List<string>(category);
                            _plateCategoryList.Sort();
                        }
                        else
                        {
                            _plateCategoryList = new List<string>(((IVehicleProfile)VehicleProfileManager.GetInstance()).GetVehiclePlateCategories((string)cmboxEmirates.SelectedValue.ToString()));
                        }
                    }
                    else
                    {
                        _plateCategoryList = new List<string>();
                        _plateCategoryList.Sort();

                    }
                    foreach (string str in _plateCategoryList)
                    {
                        cmboxPlateCategory.Items.Add(str.Trim());
                    }
                    if (AppProperties.Selected_Resource == "Arabic")
                    {

                        cmboxPlateCategory.SelectedItem = AppProperties.defaultPlateCategoryAr;
                    }
                    else
                    {

                        cmboxPlateCategory.SelectedItem = AppProperties.defaultPlateCategoryEn;
                    }
                    cmboxPlateCategory.UpdateLayout();
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
        public void SearchVehicleEnHandler()
        {
            try
            {
                _iValidate = (IValidation)new SearchVehicleInputEnValidation();
                _validationResult = _iValidate.Validate(this);
                if (_validationResult != "Valid")
                {
                    WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), _validationResult);
                }
                else
                {
                    string country = cmboxCountry.SelectedValue.ToString().Trim();
                    string source = cmboxEmirates.SelectedValue.ToString().Trim();
                    string category = cmboxPlateCategory.SelectedValue.ToString().Trim();
                    string code = cmboxPlateCode.SelectedValue.ToString().Trim();
                    string number = txtBoxPlateNumber.Text.Trim();

                    ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetCountryProperties((source.Equals("") ? country : source));
                    IVehicleProfile iVehicleProfile = (IVehicleProfile)VehicleProfileManager.GetInstance();
                    // ProgressDialogResult result = ProgressDialog.ProgressDialog.Execute(this.m_mainWindow, lblSearchingVehicle.Content.ToString() , (bw, we) =>
                    // {
                    //Do Work
                    //   AppProperties.vehicle = iVehicleProfile.GetVehicleProfileDetails(country, source, category, number, code);
                    //  });

                    ProgressDialogResult result = ProgressDialog.ProgressDialog.Execute(this.m_mainWindow, lblSearchingVehicle.Content.ToString(), (bw, we) =>
                    {

                        ((IViolationHistory)ViolationHistoryManager.GetInstance()).GetViolationHistoryByPlateNumber(country, source, category, number, code);

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
                        AppProperties.vehicle = null;
                        AppProperties.recordedViolation = null;
                        AppProperties.recordedViolation = new Violation();
                        AppProperties.recordedViolation.InspectionArea = AppProperties.location;
                        // System.Windows.Forms.MessageBox.Show(AppProperties.errorMessageFromBusiness);
                        WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("ErrorBusiness"));
                        AppProperties.businessError = false;
                        //  this.m_mainWindow.MainContentControl.Content = null;
                        // this.m_mainWindow.MainContentControl.Content = new ucLocationSelectionEn(m_mainWindow);
                        //  LandingScreenEn landing = new LandingScreenEn();
                        // _render.switchDisplay(form, landing);
                        return;
                    }
                    if (AppProperties.IsException)
                    {
                        AppProperties.IsException = false;
                        WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("ErrorException"));
                        AppProperties.vehicle = null;
                        AppProperties.recordedViolation = null;
                        AppProperties.recordedViolation = new Violation();
                        // ClearFields();
                        return;
                    }
                    if (AppProperties.NotFoundError)
                    {
                        AppProperties.NotFoundError = false;
                        WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("ErrorNotFound"));
                        AppProperties.vehicle = null;
                        AppProperties.recordedViolation = null;
                        AppProperties.recordedViolation = new Violation();
                        //  ClearFields();
                        return;
                    }


                    if (AppProperties.vehicle == null)
                    {
                        //System.Windows.Forms.MessageBox.Show("Vehicle not found");
                        // this.m_mainWindow.MainContentControl.Content = null;
                        //this.m_mainWindow.MainContentControl.Content = new ucLocationSelectionEn(m_mainWindow);
                        return;
                    }
                    else
                    {
                        this.m_mainWindow.MainContentControl.Content = null;
                        ucSearchedVehicleDetials SearchedVehicleDetials = new ucSearchedVehicleDetials(this.m_mainWindow);
                        SearchedVehicleDetials.SetVehicleRatting();
                        this.m_mainWindow.MainContentControl.Content = SearchedVehicleDetials;
                        // VehicleProfileInspectionScreenEn vehicleInspection = new VehicleProfileInspectionScreenEn();
                        //_render.switchDisplay(form, vehicleInspection);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
                App.VSDLog.Info(ex.StackTrace);
            }
        }
        public void SearchVehicleArHandler()
        {
            try
            {
                _iValidate = (IValidation)new SearchVehicleInputArValidation();
                _validationResult = _iValidate.Validate(this);
                if (_validationResult != "Valid")
                {
                    WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), _validationResult);
                }
                else
                {
                    string country = (string)this.countryTable[cmboxCountry.Text];
                    string source = (string)this.emirateTable[cmboxEmirates.Text];
                    string category = (string)this.plateCatTable[cmboxPlateCategory.Text];
                    string code = (string)this.plateCodeTable[cmboxPlateCode.Text];
                    string number = txtBoxPlateNumber.Text.Trim();




                    ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetCountryProperties((source.Equals("") ? country : source));
                    IVehicleProfile iVehicleProfile = (IVehicleProfile)VehicleProfileManager.GetInstance();
                    //    ProgressDialogResult result = ProgressDialog.ProgressDialog.Execute(this.m_mainWindow, lblSearchingVehicle.Content.ToString(), (bw, we) =>
                    //   {
                    //Do Work
                    //       AppProperties.vehicle = iVehicleProfile.GetVehicleProfileDetails(country, source, category, number, code);
                    //   });

                    ProgressDialogResult result = ProgressDialog.ProgressDialog.Execute(this.m_mainWindow, lblSearchingVehicle.Content.ToString(), (bw, we) =>
                    {

                        ((IViolationHistory)ViolationHistoryManager.GetInstance()).GetViolationHistoryByPlateNumber(country, source, category, number, code);

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
                        AppProperties.vehicle = null;
                        AppProperties.recordedViolation = null;
                        AppProperties.recordedViolation = new Violation();
                        AppProperties.recordedViolation.InspectionArea = AppProperties.location;
                        WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("ErrorBusiness"));
                        //  System.Windows.Forms.MessageBox.Show(AppProperties.errorMessageFromBusiness);
                        AppProperties.businessError = false;
                        // this.m_mainWindow.MainContentControl.Content = null;
                        // this.m_mainWindow.MainContentControl.Content = new ucLocationSelectionEn(m_mainWindow);
                        //  LandingScreenEn landing = new LandingScreenEn();
                        // _render.switchDisplay(form, landing);
                        return;
                    }
                    if (AppProperties.IsException)
                    {
                        AppProperties.IsException = false;
                        WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("ErrorException"));
                        AppProperties.vehicle = null;
                        AppProperties.recordedViolation = null;
                        AppProperties.recordedViolation = new Violation();
                        // ClearFields();
                        return;
                    }
                    if (AppProperties.NotFoundError)
                    {
                        AppProperties.NotFoundError = false;
                        WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("ErrorNotFound"));
                        AppProperties.vehicle = null;
                        AppProperties.recordedViolation = null;
                        AppProperties.recordedViolation = new Violation();
                        //  ClearFields();
                        return;
                    }

                    if (AppProperties.vehicle == null)
                    {
                        //System.Windows.Forms.MessageBox.Show("Vehicle not found");
                        // this.m_mainWindow.MainContentControl.Content = null;
                        // this.m_mainWindow.MainContentControl.Content = new ucLocationSelectionEn(m_mainWindow);
                        return;
                    }
                    else
                    {
                        ucSearchedVehicleDetials SearchedVehicleDetials = new ucSearchedVehicleDetials(this.m_mainWindow);
                        SearchedVehicleDetials.SetVehicleRatting();
                        this.m_mainWindow.MainContentControl.Content = SearchedVehicleDetials;
                        // VehicleProfileInspectionScreenEn vehicleInspection = new VehicleProfileInspectionScreenEn();
                        //_render.switchDisplay(form, vehicleInspection);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
                App.VSDLog.Info(ex.StackTrace);
            }
        }
        private void btnSearch_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {



                if (AppProperties.Selected_Resource == "English")
                {
                    imagebtnSearch.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Search.png", UriKind.Relative));
                    SearchVehicleEnHandler();
                }
                else
                {
                    imagebtnSearch.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Search Arabic Up.png", UriKind.Relative));
                    SearchVehicleArHandler();
                }


            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void btnResetVehicleRecord_Click_1(object sender, RoutedEventArgs e)
        {

            if (AppProperties.Selected_Resource == "English")
            {
                imagebtnBack.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Back.png", UriKind.Relative));
            }
            else
            {
                imagebtnBack.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Back Arabic Up.png", UriKind.Relative));
            }
            this.m_mainWindow.MainContentControl.Content = null;
            // this.m_mainWindow.MainContentControl.Content = new ucLocationSelectionEn(m_mainWindow);
            this.m_mainWindow.MainContentControl.Content = new ucWellComeScreen(m_mainWindow);
        }

        private void txtBoxPlateNumber_GotFocus_1(object sender, RoutedEventArgs e)
        {
            CommonUtils.ShowKeyBoard();
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

        private void imagebtnSearch_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            if (AppProperties.Selected_Resource == "English")
            {
                imagebtnSearch.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Search Down.png", UriKind.Relative));
            }
            else
            {
                imagebtnSearch.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Search Arabic Down.png", UriKind.Relative));
            }

        }

        private void imagebtnBack_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {

            if (AppProperties.Selected_Resource == "English")
            {
                imagebtnBack.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Back Down.png", UriKind.Relative));
            }
            else
            {
                imagebtnBack.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/back Arabic Down.png", UriKind.Relative));
            }
        }

        private void txtBoxPlateNumber_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            try
            {
                System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.DigitSubstitution = System.Globalization.DigitShapes.None;
                this.UpdateLayout();
                new CommonUtils().validateTextInteger(sender, e);
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void UserControl_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {

            if (new CommonUtils().GetScreenOrientation() == AppProperties.LandScapetMode)
            {
                // MessageBox.Show("LandScape");
                // ChangeControlDimensions(350);
              //  ChangeControlDimensions(AppProperties.LndScp_220_350);
               // ChangeButtonsDimensions(AppProperties.LndScp_Btn_100_140);
                // ChangeLableDimensions(AppProperties.LndScp_Lbl_20_25);
                //  ChangeHeaderDimensions(AppProperties.LndScp_Header_25_30);
            }
            else
            {
                //  MessageBox.Show("Potrait");
                // ChangeControlDimensions(250);
              //  ChangeControlDimensions(300);
               // ChangeButtonsDimensions(120);
                //  ChangeLableDimensions(AppProperties.Prtrt_Lbl_25_20);
                // ChangeHeaderDimensions(AppProperties.Prtrt_Header_30_25);
            }
        }
        public void ChangeControlDimensions(double width)
        {
            this.cmboxCountry.Width = width;
            this.cmboxEmirates.Width = width;
            this.cmboxPlateCategory.Width = width;
            this.cmboxPlateCode.Width = width;
            this.txtBoxPlateNumber.Width = width;
            this.UpdateLayout();
        }
        public void ChangeButtonsDimensions(double width)
        {
            this.imagebtnBack.Width = width;
            this.imagebtnSearch.Width = width;
            this.UpdateLayout();
        }
        public void ChangeLableDimensions(double width)
        {
            this.lblEmirate.FontSize = width;
            this.lblCountry.FontSize = width;
            this.lblPlateCateogry.FontSize = width;
            this.lblPlateNo.FontSize = width;
            this.lblPlateCode.FontSize = width;
            // this.lblAppLogout.FontSize = 20;
            this.UpdateLayout();
        }
        public void ChangeHeaderDimensions(double width)
        {
            this.lblVehProfile.FontSize = width;
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

        private void cmboxPlateCode_PreviewKeyDown_1(object sender, KeyEventArgs e)
        {


            try
            {
                if (e.Key == Key.Return)
                {
                    btnSearch_Click_1(sender, e);
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
    }
}
