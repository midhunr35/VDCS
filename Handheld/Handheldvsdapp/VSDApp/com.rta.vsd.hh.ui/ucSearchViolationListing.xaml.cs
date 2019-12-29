using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for ucSearchViolationListing.xaml
    /// </summary>
    public partial class ucSearchViolationListing : UserControl
    {
        MainWindow m_MainWindow = null;
        bool m_IsDataLoaded = false;
        private IValidation _iValidate;
        private string _validationResult;
        public Hashtable countryTable = new Hashtable();
        public Hashtable emirateTable = new Hashtable();
        public Hashtable plateCatTable = new Hashtable();
        public Hashtable plateCodeTable = new Hashtable();
        public Hashtable vehicleCatTable = new Hashtable();
        public static bool isBackFrmPrint = false;
        public static bool isClearGridonBack = false;

        private List<string> _countryList = new List<string>();
        private List<string> _emirateList = new List<string>();

        private List<string> _plateCategoryList = new List<string>();
        private List<string> _plateCodeList = new List<string>();
        private List<string> _categoryList = new List<string>();

        public int violationRecordNumber = 0;
        //  List<vsd.hh.data.Violation> violationData = null;
        List<DisplayObject> violationData;
        public DataTable dtProvisioanlViolationsData;




        public ucSearchViolationListing(MainWindow mainWndw)
        {
            InitializeComponent();
            this.m_MainWindow = mainWndw;
        }

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.DigitSubstitution = System.Globalization.DigitShapes.None;
            this.UpdateLayout();
           
            // System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.DigitSubstitution = System.Globalization.DigitShapes.None;          
            if (!isBackFrmPrint)
            {
                AppProperties.isOfflineDataPrint = false;
                if (AppProperties.Selected_Resource == "English")
                {
                    imagebtnBack.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Back.png", UriKind.Relative));
                    imagebtnSearch.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Search.png", UriKind.Relative));
                    imagebtnResetVehicleRecord.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Reset.png", UriKind.Relative));
                    m_MainWindow.txtlocation.Visibility = System.Windows.Visibility.Visible;

                    m_MainWindow.txtlocation.Text = AppProperties.Current_Selected_LocationEn;
                }
                else
                {
                    imagebtnBack.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/back Arabic Up.png", UriKind.Relative));
                    imagebtnSearch.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Search Arabic Up.png", UriKind.Relative));
                    imagebtnResetVehicleRecord.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Reset Arabic Up.png", UriKind.Relative));
                    m_MainWindow.txtlocation.Visibility = System.Windows.Visibility.Visible;

                    m_MainWindow.txtlocation.Text = AppProperties.Current_Selected_LocationAr;
                }

                if (AppProperties.Selected_Resource == "Arabic")
                {
                    btnStackePanel.FlowDirection = System.Windows.FlowDirection.LeftToRight;
                }
                rdoBtnByViolationID.IsChecked = true;
                PopulateData();
                System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.DigitSubstitution = System.Globalization.DigitShapes.None;
                this.UpdateLayout();

            }
            else
            {
                isBackFrmPrint = false;
                PopulateViolationHistoryDetails();
                if (AppProperties.isOfflineDataPrint)
                {
                   // PopulateOfflineViolationHistoryDetails();
                }
                System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.DigitSubstitution = System.Globalization.DigitShapes.None;
                this.UpdateLayout();
            }



        }

        public void SearchViolationEnHandler()
        {
            try
            {
                _iValidate = (IValidation)new ViolationSeacrchEnValidation();
                _validationResult = _iValidate.Validate(this);


                //////////////////
                dtProvisioanlViolationsData = new DataTable();

                if (_validationResult != "Valid")
                {
                    WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), _validationResult);
                }
                else
                {
                    
                    string country = string.Empty;
                    string emirate = string.Empty;
                    string plateCat = string.Empty;
                    string plateCode =string.Empty;
                    string plateNumber=string.Empty;
                    string violationID = string.Empty;

                    if (cmboxCountry.SelectedValue.ToString().Trim() != AppProperties.defaultCountry)
                    {
                        country = cmboxCountry.SelectedValue.ToString();                       
                        emirate = "";
                          
                        if (cmboxPlateCode.Visibility == System.Windows.Visibility.Collapsed)
                        {
                            plateCode = txtPlateCode.Text;
                        }
                        else
                        {
                            plateCode = cmboxPlateCode.SelectedValue.ToString();
                        }
                        if (null == (string)cmboxPlateCategory.SelectedItem)
                        {
                            plateCat = "";
                        }
                        else
                        {
                            plateCat = "";
                        }
                        plateNumber = txtBoxPlateNumber.Text;
                        violationID = this.txtBoxViolationID.Text.Trim();
                    }
                    else
                    {
                        country = this.cmboxCountry.SelectedValue.ToString().Trim();
                        emirate = this.cmboxEmirates.SelectedValue.ToString().Trim();
                        plateCat = this.cmboxPlateCategory.SelectedValue.ToString().Trim();
                        plateCode = this.cmboxPlateCode.SelectedValue.ToString().Trim();
                        plateNumber = this.txtBoxPlateNumber.Text.Trim();
                        violationID = this.txtBoxViolationID.Text.Trim();
                    }
                   

                    if (rdoBtnPlateNumber.IsChecked == true)
                    {
                        AppProperties.isOfflineDataPrint = false;
                        //AppProperties.vehicle = null;
                        // ProgressDialogResult result = ProgressDialog.ProgressDialog.Execute(this.m_MainWindow, "Searching Vehicle...", (bw, we) =>
                        // {
                        //Do Work
                        //      ((IVehicleProfile)VehicleProfileManager.GetInstance()).GetVehicleProfileDetails(country, emirate, plateCat, plateNumber, plateCode);
                        //   });

                        ProgressDialogResult result = ProgressDialog.ProgressDialog.Execute(this.m_MainWindow, new CommonUtils().GetStringValue("SearchingViolationHistory"), (bw, we) =>
                        {

                            ((IViolationHistory)ViolationHistoryManager.GetInstance()).GetViolationHistoryByPlateNumber(country, emirate, plateCat, plateNumber, plateCode);

                            // So this check in order to avoid default processing after the Cancel button has been pressed.
                            // This call will set the Cancelled flag on the result structure.
                            ProgressDialog.ProgressDialog.CheckForPendingCancellation(bw, we);

                        }, ProgressDialogSettings.WithSubLabelAndCancel);

                        if (result == null || result.Cancelled)
                            return;
                        else if (result.OperationFailed)
                            return;

                    }
                    else if (rdoBtnByViolationID.IsChecked == true)
                    {
                        AppProperties.isOfflineDataPrint = false;
                        if (AppProperties.vehicle != null)
                        {
                            AppProperties.vehicle.Violations = null;
                        }
                        //  ProgressDialogResult result = ProgressDialog.ProgressDialog.Execute(this.m_MainWindow, "Searching Violation History...", (bw, we) =>
                        //  {
                        //Do Work
                        //   ((IViolationHistory)ViolationHistoryManager.GetInstance()).GetViolationHistoryByID(violationID);
                        //  });

                        ProgressDialogResult result = ProgressDialog.ProgressDialog.Execute(this.m_MainWindow, new CommonUtils().GetStringValue("SearchingViolationHistory"), (bw, we) =>
                        {

                            ((IViolationHistory)ViolationHistoryManager.GetInstance()).GetViolationHistoryByID(violationID);

                            // So this check in order to avoid default processing after the Cancel button has been pressed.
                            // This call will set the Cancelled flag on the result structure.
                            ProgressDialog.ProgressDialog.CheckForPendingCancellation(bw, we);

                        }, ProgressDialogSettings.WithSubLabelAndCancel);

                        if (result == null || result.Cancelled)
                            return;
                        else if (result.OperationFailed)
                            return;

                    }
                    else if (rdoProvisionalVioaltion.IsChecked == true && !violationID.Equals(""))
                    {
                        // Provisional Vioaltion Search for one record                     



                        ProgressDialogResult result = ProgressDialog.ProgressDialog.Execute(this.m_MainWindow, new CommonUtils().GetStringValue("SearchingViolationHistory"), (bw, we) =>
                        {

                            ((IViolationHistory)ViolationHistoryManager.GetInstance()).GetOfflineViolationHistoryByID(violationID);

                            // So this check in order to avoid default processing after the Cancel button has been pressed.
                            // This call will set the Cancelled flag on the result structure.
                            ProgressDialog.ProgressDialog.CheckForPendingCancellation(bw, we);

                        }, ProgressDialogSettings.WithSubLabelAndCancel);

                        if (result == null || result.Cancelled)
                            return;
                        else if (result.OperationFailed)
                            return;
                    }
                    else
                    {
                        // Provisional Vioaltion Search for all records                       
                        AppProperties.isOfflineDataPrint = true;
                        this.grdViolationDetails.ItemsSource = null;
                        ProgressDialogResult result = ProgressDialog.ProgressDialog.Execute(this.m_MainWindow, new CommonUtils().GetStringValue("SearchingViolationHistory"), (bw, we) =>
                        {
                            //((IViolationHistory)ViolationHistoryManager.GetInstance()).GetOfflineViolationHistoryByID(violationID);
                            dtProvisioanlViolationsData = ((IViolationHistory)ViolationHistoryManager.GetInstance()).GetOfflineViolationHistoryAllByID(violationID);
                            // So this check in order to avoid default processing after the Cancel button has been pressed.
                            // This call will set the Cancelled flag on the result structure.
                            ProgressDialog.ProgressDialog.CheckForPendingCancellation(bw, we);

                        }, ProgressDialogSettings.WithSubLabelAndCancel);

                        if (result == null || result.Cancelled)
                            return;
                        else if (result.OperationFailed)
                            return;
                    }

                    if (AppProperties.businessError)
                    {
                        AppProperties.vehicle = null;
                        AppProperties.recordedViolation = null;
                        AppProperties.recordedViolation = new Violation();
                        AppProperties.recordedViolation.InspectionArea = AppProperties.location;
                        // WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), AppProperties.errorMessageFromBusiness);
                        WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("ErrorBusiness"),AppProperties.errorMessageFromBusiness);
                        AppProperties.businessError = false;
                        AppProperties.vehicle = null;
                        ClearFields();

                        // LandingScreenEn landing = new LandingScreenEn();
                        // _render.switchDisplay(form, landing);
                        //  this.m_MainWindow.MainContentControl.Content = null;
                        //  this.m_MainWindow.MainContentControl.Content = new ucLocationSelectionEn(m_MainWindow);
                        return;
                    }

                    if (AppProperties.IsException)
                    {
                        AppProperties.IsException = false;
                        WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("ErrorException"), AppProperties.errorMessageFromBusiness);
                        AppProperties.vehicle = null;
                        ClearFields();
                        return;
                    }
                    if (AppProperties.NotFoundError)
                    {
                        AppProperties.NotFoundError = false;
                        WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("ErrorNotFound"), AppProperties.errorMessageFromBusiness);
                        AppProperties.vehicle = null;
                        ClearFields();
                        return;
                    }

                    if (null != AppProperties.vehicle && !AppProperties.isOfflineDataPrint)
                    {

                        if (null != AppProperties.vehicle.Violations && AppProperties.vehicle.Violations.Length > 0)
                        {
                            PopulateViolationHistoryDetails();
                            return;
                        }
                        else
                        {
                            AppProperties.vehicle = null;
                            WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), "No violations found");
                            return;

                        }
                    }
                    else if (dtProvisioanlViolationsData != null && dtProvisioanlViolationsData.Rows.Count > 0)
                    {
                        //populate offline violation data
                        AppProperties.isOfflineDataPrint = true;
                        PopulateOfflineViolationHistoryDetails();
                        return;

                    }
                    else
                    {
                        if (this.rdoBtnPlateNumber.IsChecked == true)
                        {
                            WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), "Vehicle not found");
                        }
                        else
                        {
                            WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), "Violation not found");
                        }
                        AppProperties.vehicle = null;
                        //  this.m_MainWindow.MainContentControl.Content = null;
                        //   this.m_MainWindow.MainContentControl.Content = new ucLocationSelectionEn(m_MainWindow);
                        //   this.m_MainWindow.MainContentControl.Content = new ucWellComeScreen(m_MainWindow);
                        // LandingScreenEn landing = new LandingScreenEn();
                        //_render.switchDisplay(form, landing);
                        return;
                    }

                }
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }
        public void SearchViolationArHandler()
        {
            try
            {
                _iValidate = (IValidation)new ViolationSeacrchArValidation();
                _validationResult = _iValidate.Validate(this);

                if (_validationResult != "Valid")
                {
                    WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), _validationResult);
                }
                else
                {

                    string country = string.Empty;
                    string emirate = string.Empty;
                    string plateCat = string.Empty;
                    string plateCode = string.Empty;
                    string plateNumber = string.Empty;
                    string violationID = string.Empty;

                    if (cmboxCountry.SelectedValue.ToString().Trim() != AppProperties.defaultCountryAr)
                    {
                        country = (string)this.countryTable[cmboxCountry.Text];
                        emirate = "";
                        plateNumber = this.txtBoxPlateNumber.Text.Trim();
                        violationID = this.txtBoxViolationID.Text.Trim();

                        if (cmboxPlateCode.Visibility == System.Windows.Visibility.Collapsed)
                        {
                            plateCode = txtPlateCode.Text;
                        }
                        else
                        {
                            plateCode = (string)this.plateCodeTable[cmboxPlateCode.Text];
                        }
                        if (null == (string)cmboxPlateCategory.SelectedItem)
                        {
                            plateCat = "";
                        }
                        else
                        {
                            plateCat = "";
                        }
                    }
                    else
                    {
                        country = (string)this.countryTable[cmboxCountry.Text];
                        emirate = (string)this.emirateTable[cmboxEmirates.Text];
                        plateCat = (string)this.plateCatTable[cmboxPlateCategory.Text];
                        plateCode = (string)this.plateCodeTable[cmboxPlateCode.Text];
                        plateNumber = this.txtBoxPlateNumber.Text.Trim();
                        violationID = this.txtBoxViolationID.Text.Trim();

                    }

                    if (rdoBtnPlateNumber.IsChecked == true)
                    {
                        //AppProperties.vehicle = null;
                        // ProgressDialogResult result = ProgressDialog.ProgressDialog.Execute(this.m_MainWindow, "جاري البحث عن بيانات المركبة...", (bw, we) =>
                        // {
                        //Do Work
                        //    ((IVehicleProfile)VehicleProfileManager.GetInstance()).GetVehicleProfileDetails(country, emirate, plateCat, plateNumber, plateCode);
                        //   });

                        ProgressDialogResult result = ProgressDialog.ProgressDialog.Execute(this.m_MainWindow, new CommonUtils().GetStringValue("SearchingViolationHistory"), (bw, we) =>
                        {

                            ((IViolationHistory)ViolationHistoryManager.GetInstance()).GetViolationHistoryByPlateNumber(country, emirate, plateCat, plateNumber, plateCode);

                            // So this check in order to avoid default processing after the Cancel button has been pressed.
                            // This call will set the Cancelled flag on the result structure.
                            ProgressDialog.ProgressDialog.CheckForPendingCancellation(bw, we);

                        }, ProgressDialogSettings.WithSubLabelAndCancel);

                        if (result == null || result.Cancelled)
                            return;
                        else if (result.OperationFailed)
                            return;

                    }
                    else if (rdoBtnByViolationID.IsChecked == true)
                    {
                        if (AppProperties.vehicle != null)
                        {
                            AppProperties.vehicle.Violations = null;
                        }
                        //   ProgressDialogResult result = ProgressDialog.ProgressDialog.Execute(this.m_MainWindow, "...جاري البحث على بيانات التجاوزات", (bw, we) =>
                        //  {
                        //Do Work
                        //    ((IViolationHistory)ViolationHistoryManager.GetInstance()).GetViolationHistoryByID(violationID);
                        //  });

                        ProgressDialogResult result = ProgressDialog.ProgressDialog.Execute(this.m_MainWindow, new CommonUtils().GetStringValue("SearchingViolationHistory"), (bw, we) =>
                        {

                            ((IViolationHistory)ViolationHistoryManager.GetInstance()).GetViolationHistoryByID(violationID);

                            // So this check in order to avoid default processing after the Cancel button has been pressed.
                            // This call will set the Cancelled flag on the result structure.
                            ProgressDialog.ProgressDialog.CheckForPendingCancellation(bw, we);

                        }, ProgressDialogSettings.WithSubLabelAndCancel);

                        if (result == null || result.Cancelled)
                            return;
                        else if (result.OperationFailed)
                            return;

                    }
                    else if (rdoProvisionalVioaltion.IsChecked == true && !violationID.Equals(""))
                    {
                        // Provisional Vioaltion Search



                        ProgressDialogResult result = ProgressDialog.ProgressDialog.Execute(this.m_MainWindow, new CommonUtils().GetStringValue("SearchingViolationHistory"), (bw, we) =>
                        {

                            ((IViolationHistory)ViolationHistoryManager.GetInstance()).GetOfflineViolationHistoryByID(violationID);

                            // So this check in order to avoid default processing after the Cancel button has been pressed.
                            // This call will set the Cancelled flag on the result structure.
                            ProgressDialog.ProgressDialog.CheckForPendingCancellation(bw, we);

                        }, ProgressDialogSettings.WithSubLabelAndCancel);

                        if (result == null || result.Cancelled)
                            return;
                        else if (result.OperationFailed)
                            return;
                    }
                    else
                    {
                        // Provisional Vioaltion Search for all records                       

                        ProgressDialogResult result = ProgressDialog.ProgressDialog.Execute(this.m_MainWindow, new CommonUtils().GetStringValue("SearchingViolationHistory"), (bw, we) =>
                        {
                            //((IViolationHistory)ViolationHistoryManager.GetInstance()).GetOfflineViolationHistoryByID(violationID);
                            dtProvisioanlViolationsData = ((IViolationHistory)ViolationHistoryManager.GetInstance()).GetOfflineViolationHistoryAllByID(violationID);
                            // So this check in order to avoid default processing after the Cancel button has been pressed.
                            // This call will set the Cancelled flag on the result structure.
                            ProgressDialog.ProgressDialog.CheckForPendingCancellation(bw, we);

                        }, ProgressDialogSettings.WithSubLabelAndCancel);

                        if (result == null || result.Cancelled)
                            return;
                        else if (result.OperationFailed)
                            return;
                    }


                    if (AppProperties.businessError)
                    {
                        AppProperties.vehicle = null;
                        AppProperties.recordedViolation = null;
                        AppProperties.recordedViolation = new Violation();
                        AppProperties.recordedViolation.InspectionArea = AppProperties.location;
                        //System.Windows.Forms.MessageBox.Show(AppProperties.errorMessageFromBusiness);
                        WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("ErrorBusiness"),AppProperties.errorMessageFromBusiness);
                        AppProperties.businessError = false;
                        AppProperties.vehicle = null;
                        // LandingScreenEn landing = new LandingScreenEn();
                        // _render.switchDisplay(form, landing);
                        //   this.m_MainWindow.MainContentControl.Content = null;
                        //  this.m_MainWindow.MainContentControl.Content = new ucLocationSelectionEn(m_MainWindow);
                        return;
                    }
                    if (AppProperties.IsException)
                    {
                        AppProperties.IsException = false;
                        WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("ErrorException"),AppProperties.errorMessageFromBusiness);
                        AppProperties.vehicle = null;
                        ClearFields();
                        return;
                    }
                    if (AppProperties.NotFoundError)
                    {
                        AppProperties.NotFoundError = false;
                        WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("ErrorNotFound"),AppProperties.errorMessageFromBusiness);
                        AppProperties.vehicle = null;
                        ClearFields();
                        return;
                    }


                    if (null != AppProperties.vehicle)
                    {
                        // System.Windows.MessageBox.Show("Vehicle Data Found");
                        if (null != AppProperties.vehicle.Violations && AppProperties.vehicle.Violations.Length > 0)
                        {
                            PopulateViolationHistoryDetails();
                            // ViolationHistoryDetailScreenEn violationDetail = new ViolationHistoryDetailScreenEn();
                            //_render.switchDisplay(form, violationDetail);
                            return;
                        }
                        else
                        {
                            AppProperties.vehicle = null;
                            WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), "لم يم العثور على تجاوزات");

                            //  LandingScreenEn landing = new LandingScreenEn();
                            //  _render.switchDisplay(form, landing);
                            //  this.m_MainWindow.MainContentControl.Content = null;
                            // this.m_MainWindow.MainContentControl.Content = new ucLocationSelectionEn(m_MainWindow);
                            return;

                        }
                    }
                    else if (dtProvisioanlViolationsData != null && dtProvisioanlViolationsData.Rows.Count > 0)
                    {
                        //populate offline violation data
                        PopulateOfflineViolationHistoryDetails();
                        return;

                    }
                    else
                    {
                        if (this.rdoBtnPlateNumber.IsChecked == true)
                        {
                            WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), "لم يتم العثور على بيانات المركبة");
                        }
                        else
                        {
                            WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), "لم يم العثور على تجاوزات");
                            //System.Windows.Forms.MessageBox.Show("لم يم العثور على تجاوزات");
                        }
                        AppProperties.vehicle = null;
                        // this.m_MainWindow.MainContentControl.Content = null;
                        // this.m_MainWindow.MainContentControl.Content = new ucLocationSelectionEn(m_MainWindow);
                        // LandingScreenEn landing = new LandingScreenEn();
                        //_render.switchDisplay(form, landing);
                        return;
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
                imagebtnSearch.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Search.png", UriKind.Relative));
                SearchViolationEnHandler();
            }
            else
            {
                imagebtnSearch.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Search Arabic Up.png", UriKind.Relative));
                SearchViolationArHandler();
            }




        }
        private void btnResetVehicleRecord_Click_1(object sender, RoutedEventArgs e)
        {
            if (AppProperties.Selected_Resource == "English")
            {
                imagebtnResetVehicleRecord.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Reset.png", UriKind.Relative));
            }
            else
            {
                imagebtnResetVehicleRecord.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Reset Arabic Up.png", UriKind.Relative));
            }

            AppProperties.vehicle = null;
            this.m_MainWindow.MainContentControl.Content = null;
            this.m_MainWindow.MainContentControl.Content = new ucSearchViolationListing(this.m_MainWindow);
        }
        public void PopulateViolationHistoryDetails()
        {
            try
            {


                if (AppProperties.vehicle == null)
                    return;
                if (AppProperties.vehicle.Violations == null)
                {
                    WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), "No Violations Available");
                    return;
                }

                violationData = new List<DisplayObject>(AppProperties.vehicle.Violations.Length);
                int count = 0;
                foreach (vsd.hh.data.Violation i in AppProperties.vehicle.Violations)
                {
                    violationData.Add(new DisplayObject(i));
                    count++;
                }
                grdViolationDetails.ItemsSource = null;
                grdViolationDetails.ItemsSource = violationData;
                System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.DigitSubstitution = System.Globalization.DigitShapes.None;
                this.UpdateLayout();

            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }
        public void PopulateOfflineViolationHistoryDetails()
        {
            try
            {

                violationData = new List<DisplayObject>(dtProvisioanlViolationsData.Rows.Count);
                DataTable dtDistinctProvViolation = dtProvisioanlViolationsData.DefaultView.ToTable(true, "Violation_ID");
                foreach (DataRow dr in dtDistinctProvViolation.Rows)
                {
                    DataRow[] drVio = dtProvisioanlViolationsData.Select("Violation_ID = " + dr["Violation_ID"]);
                    violationData.Add(new DisplayObject(drVio));
                }
                grdViolationDetails.ItemsSource = null;
                grdViolationDetails.ItemsSource = violationData.Distinct().ToList();
                System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.DigitSubstitution = System.Globalization.DigitShapes.None;
                this.UpdateLayout();
                AppProperties.isOfflineDataPrint = true;
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
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
                System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.DigitSubstitution = System.Globalization.DigitShapes.None;
                this.UpdateLayout();

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

                        cmboxPlateCategory.SelectedItem = AppProperties.defaultPlateCategoryAr;
                    }
                    else
                    {

                        cmboxPlateCategory.SelectedItem = AppProperties.defaultPlateCategoryEn;
                    }
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
                            if (cmboxCountry.SelectedValue.ToString().Trim() == AppProperties.defaultCountryAr.Trim())
                                EnableDisableFields(false);
                            else
                                EnableDisableFields(true);
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
                            if (cmboxCountry.SelectedValue.ToString().Trim() == AppProperties.defaultCountry.Trim())
                                EnableDisableFields(false);
                            else
                                EnableDisableFields(true);
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



        private void rdoBtnPlateNumber_Checked_1(object sender, RoutedEventArgs e)
        {
            collapseExpandGrd(false);
            EnableDisablePlateNoSearch(true);
            EnableDisableViolationIDSearch(false);
            EnableDisableSearchResult(false);
            ClearFields();
            rdoBtnByViolationID.IsChecked = false;
            rdoProvisionalVioaltion.IsChecked = false;
            System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.DigitSubstitution = System.Globalization.DigitShapes.None;
            this.UpdateLayout();
        }
        private void rdoBtnByViolationID_Checked_1(object sender, RoutedEventArgs e)
        {
            collapseExpandGrd(false);
            EnableDisablePlateNoSearch(false);
            EnableDisableViolationIDSearch(true);
            EnableDisableSearchResult(false);
            ClearFields();
            rdoBtnPlateNumber.IsChecked = false;
            rdoProvisionalVioaltion.IsChecked = false;
            ChangetoPrivionalID(false);
            System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.DigitSubstitution = System.Globalization.DigitShapes.None;
            this.UpdateLayout();
        }
        private void rdoProvisionalVioaltion_Checked(object sender, RoutedEventArgs e)
        {
            collapseExpandGrd(true);
            EnableDisablePlateNoSearch(false);
            EnableDisableViolationIDSearch(true);

            ClearFields();
            rdoBtnPlateNumber.IsChecked = false;
            rdoBtnByViolationID.IsChecked = false;
            ChangetoPrivionalID(true);
            System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.DigitSubstitution = System.Globalization.DigitShapes.None;
            this.UpdateLayout();
            btnSearch_Click_1(sender, e);
        }

        private void collapseExpandGrd(bool isExpand)
        {
            if (isExpand)
            {//mean grid should be expand and hide all other Controls                   
                lblViolID.Visibility = Visibility.Collapsed;
                txtBoxViolationID.Visibility = Visibility.Collapsed;
                lblCountry.Visibility = Visibility.Collapsed;
                cmboxCountry.Visibility = Visibility.Collapsed;
                lblEmirates.Visibility = Visibility.Collapsed;
                cmboxEmirates.Visibility = Visibility.Collapsed;
                lblPlateCat.Visibility = Visibility.Collapsed;
                cmboxPlateCategory.Visibility = Visibility.Collapsed;
                lblPlateCode.Visibility = Visibility.Collapsed;
                cmboxPlateCode.Visibility = Visibility.Collapsed;
                txtPlateCode.Visibility = Visibility.Collapsed;
                lblPlateNumber.Visibility = Visibility.Collapsed;
                txtBoxPlateNumber.Visibility = Visibility.Collapsed;
                btnStackePanel.Visibility = Visibility.Collapsed;
                grdViolationDetails.Height = 250;
            }
            else
            {//grid should be collapsed and show all controls                    
                lblViolID.Visibility = Visibility.Visible;
                txtBoxViolationID.Visibility = Visibility.Visible;
                lblCountry.Visibility = Visibility.Visible;
                cmboxCountry.Visibility = Visibility.Visible;
                lblEmirates.Visibility = Visibility.Visible;
                cmboxEmirates.Visibility = Visibility.Visible;
                lblPlateCat.Visibility = Visibility.Visible;
                cmboxPlateCategory.Visibility = Visibility.Visible;
                lblPlateCode.Visibility = Visibility.Visible;
                cmboxPlateCode.Visibility = Visibility.Visible;
                txtPlateCode.Visibility = System.Windows.Visibility.Collapsed;
                lblPlateNumber.Visibility = Visibility.Visible;
                txtBoxPlateNumber.Visibility = Visibility.Visible;
                btnStackePanel.Visibility = Visibility.Visible;
                grdViolationDetails.Height = 100;
            }

        }
        public void ChangetoPrivionalID(bool enab)
        {
            if (enab)
            {
                txtBoxViolationID.Text = "";//"P02.01.04.02.";
            }
            else
            {
                txtBoxViolationID.Text = "02.01.04.02.";
            }
        }
        public void EnableDisablePlateNoSearch(bool enable)
        {
            this.cmboxCountry.IsEnabled = enable;
            this.cmboxEmirates.IsEnabled = enable;
            this.cmboxPlateCategory.IsEnabled = enable;
            this.cmboxPlateCode.IsEnabled = enable;
            this.txtPlateCode.IsEnabled = enable;
            this.txtBoxPlateNumber.IsEnabled = enable;
            if (!enable)
            {

                this.cmboxCountry.Background = Brushes.Gray;
                this.cmboxEmirates.Background = Brushes.Gray;
                this.cmboxPlateCategory.Background = Brushes.Gray;
                this.cmboxPlateCode.Background = Brushes.Gray;
                this.txtPlateCode.Background = Brushes.Gray;
                this.txtBoxPlateNumber.Background = Brushes.Gray;
            }
            else
            {
                this.cmboxCountry.Background = Brushes.White;
                this.cmboxEmirates.Background = Brushes.White;
                this.cmboxPlateCategory.Background = Brushes.White;
                this.cmboxPlateCode.Background = Brushes.White;
                this.txtPlateCode.Background = Brushes.White;
                this.txtBoxPlateNumber.Background = Brushes.White;
            }

        }



        public void ClearFields()
        {

            this.txtBoxViolationID.Text = "02.01.04.02.";
            this.txtBoxPlateNumber.Text = "";
            if (!isClearGridonBack)
            {
                this.grdViolationDetails.ItemsSource = null;
            }
            else
            {
                isClearGridonBack = false;
            }

        }
        public void EnableDisableViolationIDSearch(bool enable)
        {
            this.txtBoxViolationID.IsEnabled = enable;
            if (!enable)
                this.txtBoxViolationID.Background = Brushes.Gray;
            else
                this.txtBoxViolationID.Background = Brushes.White;
        }
        public void EnableDisableSearchResult(bool enable)
        {
            // this.grdViolationDetails.IsEnabled = enable;
        }

        private void btnNext_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void btnBack_Click_1(object sender, RoutedEventArgs e)
        {
            if (AppProperties.Selected_Resource == "English")
            {
                imagebtnBack.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Back.png", UriKind.Relative));
            }
            else
            {
                imagebtnBack.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/back Arabic Up.png", UriKind.Relative));
            }
            this.m_MainWindow.MainContentControl.Content = null;
            // this.m_MainWindow.MainContentControl.Content = new ucLocationSelectionEn(m_MainWindow);
            this.m_MainWindow.MainContentControl.Content = new ucWellComeScreen(m_MainWindow);
        }

        class DisplayObject : Violation
        {
            private string _ChassisNumber;
            private string _VehicleDetail;
            private string _OperatorDetail;
            private string _IssueDate;
            private string _dueDate;


            //For Printing Ticket
            private string _inspection_Location;
            private string _inspection_LocationAr;
            Defects[] _defectDetails;
            private string _VRR;
            private string _DRR;
            private string _totalImpoundginDays;

            public string TotalImpoundginDays
            {
                get { return _totalImpoundginDays; }
                set { _totalImpoundginDays = value; }
            }

            private string _vehCategory;
            public string VRR
            {
                get { return _VRR; }
                set { _VRR = value; }
            }

            public string DRR
            {
                get { return _DRR; }
                set { _DRR = value; }
            }
            public string DueDate
            {
                get { return _dueDate; }
                set { _dueDate = value; }
            }


            public string VehCategory
            {
                get { return _vehCategory; }
                set { _vehCategory = value; }
            }

            public Defects[] DefectDetails
            {
                get { return _defectDetails; }
                set { _defectDetails = value; }
            }





            public DisplayObject(vsd.hh.data.Violation CopyViolation)
            {
                ViolationSeverity = CopyViolation.ViolationSeverity;
                ViolationSeverityAr = CopyViolation.ViolationSeverityAr;
                ViolationID = CopyViolation.ViolationTicketCode;
                IssueDate = CopyViolation.ViolationIssueDate.ToString("dd/MM/yyyy");
                ViolationStatus = CopyViolation.ViolationStatus;
                DueDate = CopyViolation.ViolationDueDays.ToString("dd/MM/yyyy");

                if (AppProperties.vehicle.PlateCategory == "Public Transportation")
                {
                    VehicleDetail = (AppProperties.vehicle.PlateNumber.ToString() + " " + AppProperties.vehicle.PlateCode + "," + ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetPlateSourceCode((null != AppProperties.vehicle.Emirate) ? AppProperties.vehicle.Emirate : AppProperties.vehicle.Country)); ;
                }
                else
                {
                    VehicleDetail = (AppProperties.vehicle.PlateNumber.ToString() + " " + AppProperties.vehicle.PlateCategory + " " + AppProperties.vehicle.PlateCode + "," + ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetPlateSourceCode((null != AppProperties.vehicle.Emirate) ? AppProperties.vehicle.Emirate : AppProperties.vehicle.Country)); ;
                }


                VehicleCategory = AppProperties.vehicle.VehicleCategory;
                OperatorDetail = AppProperties.vehicle.Operator.OperatorName;
                ChassisNumber = AppProperties.vehicle.ChassisNumber;
                Inspection_location = CopyViolation.Inspection_location;
                Inspection_locationAr = CopyViolation.Inspection_locationAr;
                //string[] vehcile_Def_Sev_Fine = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetImpoundingDays(defect.DefectID.ToString(), vehCategory);
                //int grounding_days = ((null != vehcile_Def_Sev_Fine[6]) && (("" != vehcile_Def_Sev_Fine[6].Trim()))) ? Int32.Parse(vehcile_Def_Sev_Fine[6]) : 0;

                foreach (Defects def in CopyViolation.Defect)
                {
                    string[] Fines_Info = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectsFineInfo(def.DefectID.ToString(), "Heavy Vehicle");
                    def.FineName = ((null != Fines_Info[1]) && (("" != Fines_Info[1].Trim()))) ? Fines_Info[1].ToString() : "";
                    def.FineNameAr = ((null != Fines_Info[2]) && (("" != Fines_Info[2].Trim()))) ? Fines_Info[2].ToString() : "";
                    def.FineID = ((null != Fines_Info[5]) && (("" != Fines_Info[5].Trim()))) ? Fines_Info[5].ToString() : "0";
                    def.FineAmount = ((null != Fines_Info[3]) && (("" != Fines_Info[3].Trim()))) ? Fines_Info[3].ToString() : "0";

                    string[] vehcile_Def_Sev_Fine = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetImpoundingDays(def.DefectID.ToString(), "Heavy Vehicle");
                    int grounding_days = ((null != vehcile_Def_Sev_Fine[6]) && (("" != vehcile_Def_Sev_Fine[6].Trim()))) ? Int32.Parse(vehcile_Def_Sev_Fine[6]) : 0;
                    def.ImpoundingDays = grounding_days.ToString();
                    DataTable dtDefectProperty = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectPropertyByID(Convert.ToString(def.DefectID));
                    
                    if (dtDefectProperty.Rows.Count > 0)
                    {
                        DataRow dr = dtDefectProperty.Rows[0];
                        string isElliglbe = dr["ENFORCE_TESTING"].ToString();
                        if (isElliglbe != null || isElliglbe != "")
                        {
                            if (isElliglbe.Equals("F"))
                            {
                                def.DefectSeverity += "(*)";
                                def.DefectSeverityAr += "(*(";
                            }
                        }
                    }
                }

                DefectDetails = CopyViolation.Defect;
                DriverLicNo = CopyViolation.DriverLicNo;
                RtaEmpID = CopyViolation.RtaEmpID;

                ViolationSeverityAr = CopyViolation.ViolationCommentsAr;
                if (CopyViolation.PlateNumber == null)
                {
                    PlateNumber = AppProperties.vehicle.PlateNumber;
                }
                else
                {
                    PlateNumber = CopyViolation.PlateNumber;
                }
                if (CopyViolation.PlateCategory == null)
                {
                    PlateCategory = AppProperties.vehicle.PlateCategory;
                }
                else
                {
                    PlateCategory = CopyViolation.PlateCategory;
                }
                if (CopyViolation.PlateCode == null)
                {
                    PlateCode = AppProperties.vehicle.PlateCode;
                }
                else
                {
                    PlateCode = CopyViolation.PlateCode;
                }
                VRR = AppProperties.vehicle.RiskRating;
                DRR = AppProperties.vehicle.DriverRiskRattingName;

                GracePeriod = CopyViolation.GracePeriod;


            }

            public DisplayObject(DataRow dr)
            {
                try
                {
                    ViolationSeverity = dr["Severity_Level_Name"].ToString().Trim(); // CopyViolation.ViolationSeverity;
                    ViolationSeverityAr = dr["Severity_Level_Name_A"].ToString().Trim(); //CopyViolation.ViolationSeverityAr;
                    ViolationID = dr["Violation_Ticket_Code"].ToString().Trim();  //CopyViolation.ViolationTicketCode;
                    IssueDate = Convert.ToDateTime(dr["Inspection_Timestamp"]).ToString("dd/MM/yyyy");
                    DueDate = Convert.ToDateTime(dr["Due_Date"]).ToString("dd/MM/yyyy");
                    ViolationStatus = dr["Violation_Status"].ToString().Trim();
                    GracePeriod = dr["Is_Grace_Period"].ToString().Trim();
                    TotalImpoundginDays = dr["Total_Impounding_Days"].ToString().Trim();

                    if (dr["Vehicle_Plate_Category"].ToString().Trim().Equals("Public Transportation"))
                    {
                        // VehicleDetail = (AppProperties.vehicle.PlateNumber.ToString() + " " + AppProperties.vehicle.PlateCode + "," + ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetPlateSourceCode((null != AppProperties.vehicle.Emirate) ? AppProperties.vehicle.Emirate : AppProperties.vehicle.Country)); ;
                        VehicleDetail = (dr["Vehicle_Plate_Number"].ToString().Trim() + " " + dr["Vehicle_Plate_Code"].ToString().Trim() + "," + ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetPlateSourceCode((null != dr["Vehicle_Country"].ToString().Trim()) ? dr["Vehicle_Country"].ToString().Trim() : dr["Vehicle_Country"].ToString().Trim())); ;
                    }
                    else
                    {
                        //VehicleDetail = (AppProperties.vehicle.PlateNumber.ToString() + " " + AppProperties.vehicle.PlateCategory + " " + AppProperties.vehicle.PlateCode + "," + ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetPlateSourceCode((null != AppProperties.vehicle.Emirate) ? AppProperties.vehicle.Emirate : AppProperties.vehicle.Country)); ;
                        VehicleDetail = (dr["Vehicle_Plate_Number"].ToString().Trim() + " " + dr["Vehicle_Plate_Category"].ToString().Trim() + " " + dr["Vehicle_Plate_Code"].ToString().Trim() + "," + ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetPlateSourceCode((null != dr["Vehicle_Country"].ToString().Trim()) ? dr["Vehicle_Country"].ToString().Trim() : dr["Vehicle_Country"].ToString().Trim())); ;
                    }

                    OperatorDetail = dr["Owner_Name"].ToString().Trim();//AppProperties.vehicle.Operator.OperatorName;
                    ChassisNumber = dr["Vehicle_Chassis_Number"].ToString().Trim();
                    Inspection_location = dr["Location_Area_Name"].ToString().Trim();// CopyViolation.Inspection_location;
                    Inspection_locationAr = dr["Location_Area_Name_A"].ToString().Trim(); //CopyViolation.Inspection_locationAr;
                    //DefectDetails = CopyViolation.Defect;

                    vsd.hh.data.Violation.Defects[] def = new vsd.hh.data.Violation.Defects[1];
                    def[0] = new Violation.Defects();
                    def[0].DefectCategory = dr["dc_Category_Name"].ToString();
                    def[0].DefectCode = dr["dc_Category_Code"].ToString();
                    def[0].DefectID = Convert.ToInt32(dr["Defect_ID"]);
                    def[0].DefectName = dr["defect_name"].ToString().Trim();
                    def[0].DefectNameAr = dr["defect_name_a"].ToString().Trim();
                    def[0].DefectSeverity = dr["Severity_Level_Name"].ToString().Trim();
                    def[0].DefectSeverityAr = dr["Severity_Level_Name_A"].ToString().Trim();
                    def[0].DefectSubCat = "";
                    def[0].DefectType = "Defect";
                    def[0].DefectValue = dr["Defect_Value"].ToString().Trim();



                    string[] Fines_Info = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectsFineInfo(def[0].DefectID.ToString(), "Heavy Vehicle");
                    def[0].FineName = ((null != Fines_Info[1]) && (("" != Fines_Info[1].Trim()))) ? Fines_Info[1].ToString() : "";
                    def[0].FineNameAr = ((null != Fines_Info[2]) && (("" != Fines_Info[2].Trim()))) ? Fines_Info[2].ToString() : "";
                    def[0].FineID = ((null != Fines_Info[5]) && (("" != Fines_Info[5].Trim()))) ? Fines_Info[5].ToString() : "0";
                    def[0].FineAmount = ((null != Fines_Info[3]) && (("" != Fines_Info[3].Trim()))) ? Fines_Info[3].ToString() : "0";

                    string[] vehcile_Def_Sev_Fine = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetImpoundingDays(def[0].DefectID.ToString(), "Heavy Vehicle");
                    int grounding_days = ((null != vehcile_Def_Sev_Fine[6]) && (("" != vehcile_Def_Sev_Fine[6].Trim()))) ? Int32.Parse(vehcile_Def_Sev_Fine[6]) : 0;
                    def[0].ImpoundingDays = grounding_days.ToString();
                    DataTable dtDefectProperty = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectPropertyByID(Convert.ToString(def[0].DefectID));

                    if (dtDefectProperty.Rows.Count > 0)
                    {
                        DataRow datarow = dtDefectProperty.Rows[0];
                        string isElliglbe = datarow["ENFORCE_TESTING"].ToString();
                        if (isElliglbe != null || isElliglbe != "")
                        {
                            if (isElliglbe.Equals("F"))
                            {
                                def[0].DefectSeverity += "(*)";
                                def[0].DefectSeverityAr += "(*(";
                            }
                        }
                    }



                    DefectDetails = def;

                    DriverLicNo = dr["Driver_License_Number"].ToString().Trim();
                    RtaEmpID = RtaEmpID = ((null != AppProperties.empID) && (("" != AppProperties.empID))) ? AppProperties.empID : "123456"; ;


                    ViolationSeverityAr = dr["Comments_A"].ToString().Trim();//CopyViolation.ViolationCommentsAr;
                    PlateNumber = dr["Vehicle_Plate_Number"].ToString().Trim();
                    PlateCategory = dr["Vehicle_Plate_Category"].ToString().Trim();
                    PlateCode = dr["Vehicle_Plate_Code"].ToString().Trim();
                }
                catch (Exception ex)
                {
                    App.VSDLog.Info(ex.StackTrace);
                    WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
                }

            }

            //added by kashif abbasi on dated 26-Feb-2016
            public DisplayObject(DataRow[] drViolation)
            {
                try
                {
                    vsd.hh.data.Violation.Defects[] def = new vsd.hh.data.Violation.Defects[drViolation.Length];
                    int count = 0;
                    foreach (DataRow dr in drViolation)
                    {
                        if (count == 0)
                        {
                            ViolationSeverity = dr["Severity_Level_Name"].ToString().Trim(); // CopyViolation.ViolationSeverity;
                            ViolationSeverityAr = dr["Severity_Level_Name_A"].ToString().Trim(); //CopyViolation.ViolationSeverityAr;
                            ViolationID = dr["Violation_Ticket_Code"].ToString().Trim();  //CopyViolation.ViolationTicketCode;
                            IssueDate = Convert.ToDateTime(dr["Inspection_Timestamp"]).ToString("dd/MM/yyyy");
                            DueDate = Convert.ToDateTime(dr["Due_Date"]).ToString("dd/MM/yyyy");
                            ViolationStatus = dr["Violation_Status"].ToString().Trim();
                            GracePeriod = dr["Is_Grace_Period"].ToString().Trim();
                            TotalImpoundginDays = dr["Total_Impounding_Days"].ToString().Trim();



                            if (dr["Vehicle_Plate_Category"].ToString().Trim().Equals("Public Transportation"))
                            {
                                // VehicleDetail = (AppProperties.vehicle.PlateNumber.ToString() + " " + AppProperties.vehicle.PlateCode + "," + ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetPlateSourceCode((null != AppProperties.vehicle.Emirate) ? AppProperties.vehicle.Emirate : AppProperties.vehicle.Country)); ;
                                VehicleDetail = (dr["Vehicle_Plate_Number"].ToString().Trim() + " " + dr["Vehicle_Plate_Code"].ToString().Trim() + "," + ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetPlateSourceCode((null != dr["Vehicle_Country"].ToString().Trim()) ? dr["Vehicle_Country"].ToString().Trim() : dr["Vehicle_Country"].ToString().Trim())); ;
                            }
                            else
                            {
                                //VehicleDetail = (AppProperties.vehicle.PlateNumber.ToString() + " " + AppProperties.vehicle.PlateCategory + " " + AppProperties.vehicle.PlateCode + "," + ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetPlateSourceCode((null != AppProperties.vehicle.Emirate) ? AppProperties.vehicle.Emirate : AppProperties.vehicle.Country)); ;
                                VehicleDetail = (dr["Vehicle_Plate_Number"].ToString().Trim() + " " + dr["Vehicle_Plate_Category"].ToString().Trim() + " " + dr["Vehicle_Plate_Code"].ToString().Trim() + "," + ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetPlateSourceCode((null != dr["Vehicle_Country"].ToString().Trim()) ? dr["Vehicle_Country"].ToString().Trim() : dr["Vehicle_Country"].ToString().Trim())); ;
                            }

                            OperatorDetail = dr["Owner_Name"].ToString().Trim();//AppProperties.vehicle.Operator.OperatorName;
                            ChassisNumber = dr["Vehicle_Chassis_Number"].ToString().Trim();
                            Inspection_location = dr["Location_Area_Name"].ToString().Trim();// CopyViolation.Inspection_location;
                            Inspection_locationAr = dr["Location_Area_Name_A"].ToString().Trim(); //CopyViolation.Inspection_locationAr;
                            //DefectDetails = CopyViolation.Defect;
                            DriverLicNo = dr["Driver_License_Number"].ToString().Trim();
                            RtaEmpID = "";//CopyViolation.RtaEmpID;
                            ViolationSeverityAr = dr["Comments_A"].ToString().Trim();//CopyViolation.ViolationCommentsAr;
                            PlateNumber = dr["Vehicle_Plate_Number"].ToString().Trim();
                            PlateCategory = dr["Vehicle_Plate_Category"].ToString().Trim();
                            PlateCode = dr["Vehicle_Plate_Code"].ToString().Trim();
                        }

                        def[count] = new Violation.Defects();
                        def[count].DefectCategory = dr["dc_Category_Name"].ToString();
                        def[count].DefectCode = dr["dc_Category_Code"].ToString();
                        def[count].DefectID = Convert.ToInt32(dr["Defect_ID"]);
                        def[count].DefectName = dr["defect_name"].ToString().Trim();
                        def[count].DefectNameAr = dr["defect_name_a"].ToString().Trim();
                        def[count].DefectSeverity = dr["Severity_Level_Name"].ToString().Trim();
                        def[count].DefectSeverityAr = dr["Severity_Level_Name_A"].ToString().Trim();
                        def[count].DefectSubCat = "";
                        def[count].DefectType = "Defect";
                        def[count].DefectValue = dr["Defect_Value"].ToString().Trim();

                        string[] Fines_Info = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectsFineInfo(def[count].DefectID.ToString(), "Heavy Vehicle");
                        def[count].FineName = ((null != Fines_Info[1]) && (("" != Fines_Info[1].Trim()))) ? Fines_Info[1].ToString() : "";
                        def[count].FineNameAr = ((null != Fines_Info[2]) && (("" != Fines_Info[2].Trim()))) ? Fines_Info[2].ToString() : "";
                        def[count].FineID = ((null != Fines_Info[5]) && (("" != Fines_Info[5].Trim()))) ? Fines_Info[5].ToString() : "0";
                        def[count].FineAmount = ((null != Fines_Info[3]) && (("" != Fines_Info[3].Trim()))) ? Fines_Info[3].ToString() : "0";

                        string[] vehcile_Def_Sev_Fine = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetImpoundingDays(def[count].DefectID.ToString(), "Heavy Vehicle");
                        int grounding_days = ((null != vehcile_Def_Sev_Fine[6]) && (("" != vehcile_Def_Sev_Fine[6].Trim()))) ? Int32.Parse(vehcile_Def_Sev_Fine[6]) : 0;
                        def[count].ImpoundingDays = grounding_days.ToString();
                        DataTable dtDefectProperty = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectPropertyByID(Convert.ToString(def[count].DefectID));

                        if (dtDefectProperty.Rows.Count > 0)
                        {
                            DataRow datarow = dtDefectProperty.Rows[0];
                            string isElliglbe = datarow["ENFORCE_TESTING"].ToString();
                            if (isElliglbe != null || isElliglbe != "")
                            {
                                if (isElliglbe.Equals("F"))
                                {
                                    def[count].DefectSeverity += "(*)";
                                    def[count].DefectSeverityAr += "(*(";
                                }
                            }
                        }

                        count++;
                    }
                    RtaEmpID = RtaEmpID = ((null != AppProperties.empID) && (("" != AppProperties.empID))) ? AppProperties.empID : "123456"; ;
                    DefectDetails = def;
                }
                catch (Exception ex)
                {
                    App.VSDLog.Info(ex.StackTrace);
                    WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
                }

            }

            public string ChassisNumber
            {
                get { return this._ChassisNumber; }
                set { this._ChassisNumber = value; }
            }

            public string VehicleDetail
            {
                get { return this._VehicleDetail; }
                set { this._VehicleDetail = value; }
            }

            public string OperatorDetail
            {
                get { return this._OperatorDetail; }
                set { this._OperatorDetail = value; }
            }

            public string IssueDate
            {
                get { return this._IssueDate; }
                set { this._IssueDate = value; }
            }
            public string Inspection_LocationAr
            {
                get { return _inspection_LocationAr; }
                set { _inspection_LocationAr = value; }
            }

            public string Inspection_Location
            {
                get { return _inspection_Location; }
                set { _inspection_Location = value; }
            }
        }

        private void txtBoxViolationID_GotFocus_1(object sender, RoutedEventArgs e)
        {
            CommonUtils.ShowKeyBoard();
        }

        private void txtBoxViolationID_LostFocus_1(object sender, RoutedEventArgs e)
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

        private void imagebtnResetVehicleRecord_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {

            if (AppProperties.Selected_Resource == "English")
            {
                imagebtnResetVehicleRecord.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Reset Down.png", UriKind.Relative));
            }
            else
            {
                imagebtnResetVehicleRecord.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Reset Arabic Down.png", UriKind.Relative));
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
                new CommonUtils().validateTextInteger(sender, e);
                System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.DigitSubstitution = System.Globalization.DigitShapes.None;
                this.UpdateLayout();
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void UserControl_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {
            ChangeControlPositions();
            System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.DigitSubstitution = System.Globalization.DigitShapes.None;
            this.UpdateLayout();
            if (new CommonUtils().GetScreenOrientation() == AppProperties.LandScapetMode)
            {


                this.grdViolationDetails.Width = 940;


            }
            else
            {

                this.grdViolationDetails.Width = 480;
            }
        }
        public void ChangeControlPositions()
        {
            if (new CommonUtils().GetScreenOrientation() == AppProperties.LandScapetMode)
            {


                MainGrid.Children.Remove(rdoBtnPlateNumber);
                Grid.SetRow(rdoBtnPlateNumber, 3);
                Grid.SetColumn(rdoBtnPlateNumber, 5);
                MainGrid.Children.Add(rdoBtnPlateNumber);

                MainGrid.Children.Remove(rdoProvisionalVioaltion);
                Grid.SetRow(rdoProvisionalVioaltion, 3);
                Grid.SetColumn(rdoProvisionalVioaltion, 7);
                MainGrid.Children.Add(rdoProvisionalVioaltion);

                MainGrid.Children.Remove(lblViolID);
                Grid.SetRow(lblViolID, 5);
                Grid.SetColumn(lblViolID, 1);
                MainGrid.Children.Add(lblViolID);

                MainGrid.Children.Remove(txtBoxViolationID);
                Grid.SetRow(txtBoxViolationID, 5);
                Grid.SetColumn(txtBoxViolationID, 3);
                MainGrid.Children.Add(txtBoxViolationID);

                MainGrid.Children.Remove(lblCountry);
                Grid.SetRow(lblCountry, 5);
                Grid.SetColumn(lblCountry, 5);
                MainGrid.Children.Add(lblCountry);

                MainGrid.Children.Remove(cmboxCountry);
                Grid.SetRow(cmboxCountry, 5);
                Grid.SetColumn(cmboxCountry, 7);
                MainGrid.Children.Add(cmboxCountry);

                MainGrid.Children.Remove(lblEmirates);
                Grid.SetRow(lblEmirates, 7);
                Grid.SetColumn(lblEmirates, 1);
                MainGrid.Children.Add(lblEmirates);

                MainGrid.Children.Remove(cmboxEmirates);
                Grid.SetRow(cmboxEmirates, 7);
                Grid.SetColumn(cmboxEmirates, 3);
                MainGrid.Children.Add(cmboxEmirates);

                MainGrid.Children.Remove(lblPlateCat);
                Grid.SetRow(lblPlateCat, 7);
                Grid.SetColumn(lblPlateCat, 5);
                MainGrid.Children.Add(lblPlateCat);

                MainGrid.Children.Remove(cmboxPlateCategory);
                Grid.SetRow(cmboxPlateCategory, 7);
                Grid.SetColumn(cmboxPlateCategory, 7);
                MainGrid.Children.Add(cmboxPlateCategory);

                MainGrid.Children.Remove(lblPlateNumber);
                Grid.SetRow(lblPlateNumber, 9);
                Grid.SetColumn(lblPlateNumber, 1);
                MainGrid.Children.Add(lblPlateNumber);

                MainGrid.Children.Remove(txtBoxPlateNumber);
                Grid.SetRow(txtBoxPlateNumber, 9);
                Grid.SetColumn(txtBoxPlateNumber, 3);
                MainGrid.Children.Add(txtBoxPlateNumber);

                MainGrid.Children.Remove(lblPlateCode);
                Grid.SetRow(lblPlateCode, 9);
                Grid.SetColumn(lblPlateCode, 5);
                MainGrid.Children.Add(lblPlateCode);

                MainGrid.Children.Remove(cmboxPlateCode);
                Grid.SetRow(cmboxPlateCode, 9);
                Grid.SetColumn(cmboxPlateCode, 7);
                MainGrid.Children.Add(cmboxPlateCode);

                MainGrid.Children.Remove(txtPlateCode);
                Grid.SetRow(txtPlateCode, 9);
                Grid.SetColumn(txtPlateCode, 7);
                MainGrid.Children.Add(txtPlateCode);
                //   Grid.Column="1" Grid.Row="11" Grid.ColumnSpan="4"

                MainGrid.Children.Remove(lblVioDetail);
                Grid.SetRow(lblVioDetail, 11);
                Grid.SetColumn(lblVioDetail, 1);
                Grid.SetColumnSpan(lblVioDetail, 4);
                MainGrid.Children.Add(lblVioDetail);

                MainGrid.Children.Remove(btnStackePanel);
                Grid.SetRow(btnStackePanel, 11);
                Grid.SetColumn(btnStackePanel, 7);
                MainGrid.Children.Add(btnStackePanel);

                MainGrid.Children.Remove(grdViolationDetails);
                Grid.SetRow(grdViolationDetails, 13);
                Grid.SetColumn(grdViolationDetails, 1);
                Grid.SetColumnSpan(grdViolationDetails, 7);
                MainGrid.Children.Add(grdViolationDetails);

                MainGrid.Children.Remove(stkPanleReset);
                Grid.SetRow(stkPanleReset, 15);
                Grid.SetColumn(stkPanleReset, 7);
                MainGrid.Children.Add(stkPanleReset);
            }
            else
            {
                MainGrid.Children.Remove(rdoBtnPlateNumber);
                Grid.SetRow(rdoBtnPlateNumber, 5);
                Grid.SetColumn(rdoBtnPlateNumber, 1);
                MainGrid.Children.Add(rdoBtnPlateNumber);

                MainGrid.Children.Remove(rdoProvisionalVioaltion);
                Grid.SetRow(rdoProvisionalVioaltion, 5);
                Grid.SetColumn(rdoProvisionalVioaltion, 3);
                MainGrid.Children.Add(rdoProvisionalVioaltion);

                MainGrid.Children.Remove(lblViolID);
                Grid.SetRow(lblViolID, 7);
                Grid.SetColumn(lblViolID, 1);
                MainGrid.Children.Add(lblViolID);

                MainGrid.Children.Remove(txtBoxViolationID);
                Grid.SetRow(txtBoxViolationID, 7);
                Grid.SetColumn(txtBoxViolationID, 3);
                MainGrid.Children.Add(txtBoxViolationID);

                MainGrid.Children.Remove(lblCountry);
                Grid.SetRow(lblCountry, 9);
                Grid.SetColumn(lblCountry, 1);
                MainGrid.Children.Add(lblCountry);

                MainGrid.Children.Remove(cmboxCountry);
                Grid.SetRow(cmboxCountry, 9);
                Grid.SetColumn(cmboxCountry, 3);
                MainGrid.Children.Add(cmboxCountry);

                MainGrid.Children.Remove(lblEmirates);
                Grid.SetRow(lblEmirates, 11);
                Grid.SetColumn(lblEmirates, 1);
                MainGrid.Children.Add(lblEmirates);

                MainGrid.Children.Remove(cmboxEmirates);
                Grid.SetRow(cmboxEmirates, 11);
                Grid.SetColumn(cmboxEmirates, 3);
                MainGrid.Children.Add(cmboxEmirates);

                MainGrid.Children.Remove(lblPlateCat);
                Grid.SetRow(lblPlateCat, 13);
                Grid.SetColumn(lblPlateCat, 1);
                MainGrid.Children.Add(lblPlateCat);

                MainGrid.Children.Remove(cmboxPlateCategory);
                Grid.SetRow(cmboxPlateCategory, 13);
                Grid.SetColumn(cmboxPlateCategory, 3);
                MainGrid.Children.Add(cmboxPlateCategory);

                MainGrid.Children.Remove(lblPlateNumber);
                Grid.SetRow(lblPlateNumber, 15);
                Grid.SetColumn(lblPlateNumber, 1);
                MainGrid.Children.Add(lblPlateNumber);

                MainGrid.Children.Remove(txtBoxPlateNumber);
                Grid.SetRow(txtBoxPlateNumber, 15);
                Grid.SetColumn(txtBoxPlateNumber, 3);
                MainGrid.Children.Add(txtBoxPlateNumber);

                MainGrid.Children.Remove(lblPlateCode);
                Grid.SetRow(lblPlateCode, 17);
                Grid.SetColumn(lblPlateCode, 1);
                MainGrid.Children.Add(lblPlateCode);

                MainGrid.Children.Remove(cmboxPlateCode);
                Grid.SetRow(cmboxPlateCode, 17);
                Grid.SetColumn(cmboxPlateCode, 3);
                MainGrid.Children.Add(cmboxPlateCode);

                MainGrid.Children.Remove(txtPlateCode);
                Grid.SetRow(txtPlateCode, 17);
                Grid.SetColumn(txtPlateCode, 3);
                MainGrid.Children.Add(txtPlateCode);

                MainGrid.Children.Remove(btnStackePanel);
                Grid.SetRow(btnStackePanel, 19);
                Grid.SetColumn(btnStackePanel, 3);
                MainGrid.Children.Add(btnStackePanel);

                MainGrid.Children.Remove(lblVioDetail);
                Grid.SetRow(lblVioDetail, 21);
                Grid.SetColumn(lblVioDetail, 1);
                Grid.SetColumnSpan(lblVioDetail, 4);
                MainGrid.Children.Add(lblVioDetail);



                MainGrid.Children.Remove(grdViolationDetails);
                Grid.SetRow(grdViolationDetails, 23);
                Grid.SetColumn(grdViolationDetails, 1);
                Grid.SetColumnSpan(grdViolationDetails, 7);
                MainGrid.Children.Add(grdViolationDetails);

                MainGrid.Children.Remove(stkPanleReset);
                Grid.SetRow(stkPanleReset, 25);
                Grid.SetColumn(stkPanleReset, 1);
                MainGrid.Children.Add(stkPanleReset);
            }


        }
        public void ChangeControlDimensions(double width)
        {
            this.cmboxCountry.Width = width;
            this.cmboxEmirates.Width = width;
            this.cmboxPlateCategory.Width = width;
            this.cmboxPlateCode.Width = width;
            this.txtBoxPlateNumber.Width = width;
            this.txtBoxViolationID.Width = width;
            //   this.txtRecomendations.Width = width;


            // this.txtYear.Width = width;

            this.UpdateLayout();
        }
        public void ChangeButtonsDimensions(double width)
        {
            this.imagebtnResetVehicleRecord.Width = width;
            this.imagebtnSearch.Width = width;
            this.imagebtnBack.Width = width;

            this.UpdateLayout();
        }
        public void ChangeButtonsDimensions2(double width)
        {
            // this.btnAddImage.Width = width;
        }
        public void ChangeLableDimensions(double width)
        {
            this.lblSerchBy.FontSize = width;
            this.lblViolID.FontSize = width;
            this.lblCountry.FontSize = width;
            this.lblEmirates.FontSize = width;
            this.lblPlateCat.FontSize = width;

            this.lblPlateNumber.FontSize = width;
            this.lblPlateCode.FontSize = width;

            // this.lblYear.FontSize = width;
            // this.lblAppLogout.FontSize = 20;
            this.UpdateLayout();
        }
        public void ChangeHeaderDimensions(double width)
        {
            this.lblViolHist.FontSize = width;
            this.lblVioDetail.FontSize = width;

            this.UpdateLayout();
        }

        private void btnPrintRecipt_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {


                if (grdViolationDetails.SelectedItem == null)
                    return;

                DisplayObject selcted_Violation_data = (DisplayObject)grdViolationDetails.SelectedItem;
                if (selcted_Violation_data == null)
                    return;
                ViolationTicket vioaltion_ticket_data = new ViolationTicket();
                vioaltion_ticket_data.ViolationID = selcted_Violation_data.ViolationID;

                // vioaltion_ticket_data.vi = selcted_Violation_data.ViolationSeverityAr;
                vioaltion_ticket_data.DateTime = selcted_Violation_data.IssueDate;
                vioaltion_ticket_data.DueDate = selcted_Violation_data.DueDate;
                vioaltion_ticket_data.LocaitonAr = ((VSDApp.com.rta.vsd.hh.data.Violation)(selcted_Violation_data)).Inspection_locationAr;
                vioaltion_ticket_data.Location = ((VSDApp.com.rta.vsd.hh.data.Violation)(selcted_Violation_data)).Inspection_location;
                vioaltion_ticket_data.VehicleDetials = selcted_Violation_data.VehicleDetail;
                vioaltion_ticket_data.VehicleDetailsAr = selcted_Violation_data.VehicleDetail;
                vioaltion_ticket_data.DefectDetails = selcted_Violation_data.DefectDetails;
                vioaltion_ticket_data.DriverLicNo = selcted_Violation_data.DriverLicNo;
                vioaltion_ticket_data.RtaEmpNo = selcted_Violation_data.RtaEmpID;
                vioaltion_ticket_data.ViolationAdviceAr = selcted_Violation_data.ViolationCommentsAr;
                vioaltion_ticket_data.PlateNumber = selcted_Violation_data.PlateNumber;
                vioaltion_ticket_data.PlateCode = selcted_Violation_data.PlateCode;
                vioaltion_ticket_data.PlateCategory = selcted_Violation_data.PlateCategory;
                vioaltion_ticket_data.VRR = selcted_Violation_data.VRR;
                vioaltion_ticket_data.DRR = selcted_Violation_data.DRR;
                vioaltion_ticket_data.GraePeriod = selcted_Violation_data.GracePeriod;



                string[] info;
                info = ((IViolation)ViolationManager.GetInstance()).GetConfigurationDataForSeverity(selcted_Violation_data.ViolationSeverity, vioaltion_ticket_data.DefectDetails.Length);

                if (info != null)
                {
                    vioaltion_ticket_data.ViolationAdvice = info[4];
                    vioaltion_ticket_data.ViolationAdviceAr = info[5];

                }


                ///////////////////////////////////
                /*
                string violation_id = selcted_Violation_data.ViolationID;
                string violation_severity = selcted_Violation_data.ViolationSeverity;
                string violation_severityAr = selcted_Violation_data.ViolationSeverityAr;
                string date_time = selcted_Violation_data.IssueDate;
                string inspection_LocationAr = ((VSDApp.com.rta.vsd.hh.data.Violation)(selcted_Violation_data)).Inspection_locationAr;
                string inspection_Location = ((VSDApp.com.rta.vsd.hh.data.Violation)(selcted_Violation_data)).Inspection_location;
                string vehicle_detials = selcted_Violation_data.VehicleDetail;
                string defect_code = selcted_Violation_data.DefectDetails[0].DefectCode;
              //  selcted_Violation_data.DefectDetails[0]._defecNameAr
                string defect_name = selcted_Violation_data.DefectDetails[0].DefectName;
                string defect_nameAr = selcted_Violation_data.DefectDetails[0].DefectNameAr;
                string defect_severity = selcted_Violation_data.DefectDetails[0].DefectSeverity;
                string defect_severityAr = selcted_Violation_data.DefectDetails[0].DefectSeverityAr;
                string defect_coments = selcted_Violation_data.DefectDetails[0].DefectValue;
                */
                this.m_MainWindow.MainContentControl.Content = null;
                this.m_MainWindow.MainContentControl.Content = new ucPrintSearchedViolationTicket(this.m_MainWindow, vioaltion_ticket_data, true, this);
                // this.grdViolationDetails.ItemsSource = violationData_Saved;
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void UserControl_Initialized_1(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.DigitSubstitution = System.Globalization.DigitShapes.None;
            this.UpdateLayout();
        }

        private void txtBoxViolationID_PreviewKeyDown_1(object sender, KeyEventArgs e)
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

        private void txtBoxViolationID_Initialized_1(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.DigitSubstitution = System.Globalization.DigitShapes.None;
            this.UpdateLayout();
        }

        private void txtBoxViolationID_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.DigitSubstitution = System.Globalization.DigitShapes.None;
            this.UpdateLayout();
        }

        private void txtBoxPlateNumber_GotFocus(object sender, RoutedEventArgs e)
        {

        }
        private void EnableDisableFields(bool isReadOnly)
        {
            //cmboxEmirates.IsReadOnly = isReadOnly;
            //cmboxPlateCategory.IsReadOnly = isReadOnly;
            if (isReadOnly)
            {
                cmboxEmirates.Background = Brushes.Gray;
                cmboxPlateCategory.Background = Brushes.Gray;
                cmboxPlateCategory.IsEnabled = false;
                cmboxEmirates.IsEnabled = false;
               // txtBoxEmirats.Visibility = Visibility.Visible;
              
               
                txtPlateCode.Visibility = System.Windows.Visibility.Visible;
                cmboxPlateCode.Visibility = System.Windows.Visibility.Collapsed;
                cmboxPlateCode.IsEnabled = false;
                cmboxPlateCode.Background = Brushes.Gray;
               

            }
            else
            {

                cmboxEmirates.Background = Brushes.White; ;
               
                cmboxEmirates.IsEnabled = true;
                cmboxPlateCategory.Background = Brushes.White;
                cmboxPlateCategory.IsEnabled = true;
               
              
                txtPlateCode.Visibility = System.Windows.Visibility.Collapsed;
                cmboxPlateCode.Visibility = System.Windows.Visibility.Visible;
                cmboxPlateCode.IsEnabled = true;
                cmboxPlateCode.Background = Brushes.White;
            }
        }
    }
}