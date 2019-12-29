using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
using VSDApp.WPFMessageBoxControl;

namespace VSDApp.com.rta.vsd.hh.ui
{
    /// <summary>
    /// Interaction logic for ucLocationSelectionEn.xaml
    /// </summary>
    public partial class ucLocationSelectionEn : UserControl
    {
        //  String sURL = AppDomain.CurrentDomain.BaseDirectory + "html/map.html";
        //String sURL = @"/html/map.html";
        #region DataMembers
        private MainWindow m_MainWindow = null;
        private List<string> _emirates;// = new List<string>();
        private List<string> _locations = new List<string>();
        private List<string> _areas = new List<string>();
        bool _isDataLoad = false;


        public Hashtable citiesTable = new Hashtable();
        public Hashtable areasTable = new Hashtable();
        public Hashtable locationTable = new Hashtable();
        ucGoogleMap showLocationOnMap = new ucGoogleMap();

        // private MainWindow m_MainWindow = null;
        private IValidation _iValidate;
        private string _validationResult;
        public delegate void ShowGoodBteScreen_Delegate();
        System.Timers.Timer tmr = new System.Timers.Timer();

        #endregion

        #region Events
        private void cmboxEmirates_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if ((string)cmboxEmirates.SelectedItem != "")
                {
                    if (cmboxArea.Items.Count > 0)
                    {
                        cmboxArea.Text = "";
                        cmboxArea.Items.Clear();
                        cmboxArea.SelectedIndex = -1;

                    }
                    if (cmboxEmirates.SelectedValue != null)
                    {
                        if (AppProperties.Selected_Resource == "Arabic")
                        {

                            areasTable = CommonUtils.Splitter(((IViolation)ViolationManager.GetInstance()).GetLocation(((string)citiesTable[(string)cmboxEmirates.SelectedItem]), "area"));
                            string[] areaArray = new string[areasTable.Count];
                            areasTable.Keys.CopyTo(areaArray, 0);
                            _areas = new List<string>(areaArray);
                            _areas.Sort();
                            foreach (string str in _areas)
                            {
                                cmboxArea.Items.Add(str.Trim());
                            }
                            if (AppProperties.Previous_Selected_AreaAr != string.Empty)
                            {
                                // string str = (string)citiesTable[(string)AppProperties.Previous_Selected_Area];
                                cmboxArea.SelectedItem = AppProperties.Previous_Selected_AreaAr;
                            }
                            else
                            {
                                cmboxArea.SelectedItem = AppProperties.defaultAreaAr;
                            }
                        }
                        else
                        {
                            _areas = new List<string>(((IViolation)ViolationManager.GetInstance()).GetLocation(cmboxEmirates.SelectedValue.ToString(), "area"));
                            _areas.Sort();
                            foreach (string str in _areas)
                            {
                                cmboxArea.Items.Add(str.Trim());
                            }
                            if (AppProperties.Previous_Selected_AreaEn != string.Empty)
                            {
                                cmboxArea.SelectedItem = AppProperties.Previous_Selected_AreaEn;
                            }
                            else
                            {
                                cmboxArea.SelectedItem = AppProperties.defaultAreaEn;
                            }
                        }
                    }
                    else
                        _areas = new List<string>();

                    cmboxArea.UpdateLayout();
                    //  showLocationOnMap.MoveMapToLocation(cmboxEmirates.SelectedValue.ToString() + cmboxArea.SelectedValue.ToString() + cmboxLocation.SelectedValue.ToString());
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

        private void cmboxArea_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if ((string)cmboxArea.SelectedItem != "")
                {
                    if (cmboxLocation.Items.Count > 0)
                    {
                        cmboxLocation.Text = "";
                        cmboxLocation.Items.Clear();
                        cmboxLocation.SelectedIndex = 1;
                    }
                    cmboxLocation.UpdateLayout();
                    if (cmboxArea.SelectedValue != null)
                    {
                        if (AppProperties.Selected_Resource == "Arabic")
                        {

                            //locationTable = CommonUtils.Splitter(((IViolation)ViolationManager.GetInstance()).GetLocation((string)areasTable[(string)cmboxArea.SelectedItem], "location"));
                            List<string>  lstLocation = new List<string>();
                            locationTable = CommonUtils.SplitterWithOrder(((IViolation)ViolationManager.GetInstance()).GetLocation((string)areasTable[(string)cmboxArea.SelectedItem], "location"), out lstLocation);
                            string[] locArray = new string[locationTable.Count];
                            locationTable.Keys.CopyTo(locArray, 0);

                            //_locations = new List<string>(locArray);
                            _locations = lstLocation;



                            //_locations.Sort();
                        }
                        else
                        {
                            _locations = new List<string>(((IViolation)ViolationManager.GetInstance()).GetLocation(cmboxArea.SelectedValue.ToString(), "location"));
                            //_locations.Sort();
                        }
                    }
                    //
                    else
                        _locations = new List<string>();
                    foreach (string str in _locations)
                    {
                        cmboxLocation.Items.Add(str.Trim());
                    }
                    if (cmboxLocation.Items.Count > 0)
                        cmboxLocation.SelectedIndex = 0;
                    cmboxLocation.DisplayMemberPath = "";
                    cmboxLocation.UpdateLayout();
                    //  ShowInspectionLocation();

                    if (AppProperties.Selected_Resource == "Arabic")
                    {
                        if ((AppProperties.Previous_Selected_LocationAr != string.Empty) && (AppProperties.Previous_Selected_LocationAr != ""))
                        {
                            cmboxLocation.SelectedValue = AppProperties.Previous_Selected_LocationAr;
                        }
                        else
                        {
                            cmboxLocation.SelectedIndex = 0;
                        }


                    }
                    else
                    {
                        if ((AppProperties.Previous_Selected_LocationEn != string.Empty) && (AppProperties.Previous_Selected_LocationEn != ""))
                        {
                            cmboxLocation.SelectedValue = AppProperties.Previous_Selected_LocationEn;

                        }
                        else
                        {
                            cmboxLocation.SelectedIndex = 0;
                        }
                    }

                    // showLocationOnMap.MoveMapToLocation(cmboxEmirates.SelectedValue.ToString() + cmboxArea.SelectedValue.ToString() + cmboxLocation.SelectedValue.ToString());

                }
                else
                {
                    cmboxArea.Items.Clear();
                    cmboxArea.Items.Add("");
                }
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void cmboxLocation_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            // showLocationOnMap.MoveMapToLocation(cmboxEmirates.SelectedValue.ToString() + cmboxArea.SelectedValue.ToString() + cmboxLocation.SelectedValue.ToString());
            /*
             if (cmboxLocation.SelectedValue != null)
             {
                 showLocationOnMap.MoveMapToLocation("Dubai" + cmboxLocation.SelectedValue.ToString());
             }*/
            ShowLocationOnMap();
            ShowInspectionLocation();
            this.UpdateLayout();

            //Hashtable plateDetails = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetPlateDetailsInArabic("Private", "");

            // string plateCat = (string)plateDetails["Private"];
            // string plateCode = (string)plateDetails["A"];
            //   string str =   

            //

        }



        public void ShowInspectionLocation()
        {
            if (cmboxLocation.SelectedValue == null)
                return;
            string selected_location = cmboxLocation.SelectedValue.ToString();
            if (AppProperties.Selected_Resource == "Arabic")
            {
                m_MainWindow.txtlocation.Visibility = System.Windows.Visibility.Visible;
                m_MainWindow.txtlocation.Text = "";
                m_MainWindow.txtlocation.Text = selected_location;
                AppProperties.Current_Selected_LocationAr = selected_location;
                //  AppProperties.location.location = (string)locationTable[(string)cmboxLocation.SelectedItem];
                // AppProperties.recordedViolation.InspectionArea = AppProperties.location;
            }
            else
            {
                // AppProperties.location.city = ((string)((ucLocationSelectionEn)this).cmboxEmirates.Text).Trim();
                //AppProperties.location.area = ((string)((ucLocationSelectionEn)this).cmboxArea.Text).Trim();
                m_MainWindow.txtlocation.Visibility = System.Windows.Visibility.Visible;
                m_MainWindow.txtlocation.Text = "";
                m_MainWindow.txtlocation.Text = selected_location;
                AppProperties.Current_Selected_LocationEn = selected_location;
            }
            m_MainWindow.txtlocation.UpdateLayout();
        }


        private void setupObjectForScripting(object sender, RoutedEventArgs e)
        {
            ((WebBrowser)sender).ObjectForScripting = new HtmlInteropInternalTestClass();
        }
        public void ShowLocationOnMap()
        {
            try
            {
                if (cmboxLocation.SelectedValue == null)
                    return;
                string selected_location = cmboxLocation.SelectedValue.ToString();
                switch (selected_location)
                {
                    case "DAFZA":
                        imag_location.BeginInit();
                        imag_location.Source = new BitmapImage(new Uri(@"/Images/GoogleLocationImages/DafazDubai.jpg", UriKind.Relative));
                        // imag_location.CacheMode = BitmapCacheOption.OnLoad;
                        imag_location.EndInit();
                        break;
                    case "Al-Hamriya Port":
                        imag_location.BeginInit();
                        imag_location.Source = new BitmapImage(new Uri(@"/Images/GoogleLocationImages/Alhamriya-Daira.jpg", UriKind.Relative));
                        imag_location.EndInit();
                        break;
                    case "University City Road":
                        imag_location.BeginInit();
                        imag_location.Source = new BitmapImage(new Uri(@"/Images/GoogleLocationImages/UniveristyRoad.jpg", UriKind.Relative));
                        imag_location.EndInit();
                        break;
                    default:
                        break;

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

            // this.ContentControlShowMap.Content = showLocationOnMap;
            if (AppProperties.Selected_Resource == "Arabic")
            {

                imagebtnNext.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Start Inspection Arabic Up.png", UriKind.Relative));
                imagebtnback.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/back Arabic Up.png", UriKind.Relative));
            }
            else
            {

                imagebtnNext.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Start Inspection.png", UriKind.Relative));
                imagebtnback.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Back.png", UriKind.Relative));
            }
            /*
            Uri uri_1 = new Uri(sURL);
            webBrowser1.Navigate(uri_1);


            ///////////////////////////
            
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "html\\map_route.html"))
            {
                StreamReader objReader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "html\\map2.html");
                string line = "";
                line = objReader.ReadToEnd();
                objReader.Close();
                 line = line.Replace("[origin]","25.0476643, 55.1817407.50607");
                 line = line.Replace("[destination]", "25.0476643, 55.1817407.50607");
                StreamWriter page = File.CreateText(AppDomain.CurrentDomain.BaseDirectory + "html\\map2.html");
                page.Write(line);
                page.Close();
                Uri uri = new Uri(AppDomain.CurrentDomain.BaseDirectory + "html\\map2.html");
                webBrowser1.Navigate(uri);
               // datos.Width = 140;
            }

            */


            /////////////
            //  EnableSearchOption(true);
            if (AppProperties.Selected_Resource == "Arabic")
            {
                btnStackePanel.FlowDirection = System.Windows.FlowDirection.LeftToRight;
            }
            //if (!_isDataLoad)
            //{
            FillLocationCombox();
            ShowInspectorInformation();
            this.ShowLocationOnMap();

            //  _isDataLoad = true;
            //}
        }

        public void ShowInspectorInformation()
        {
            try
            {
                if (AppProperties.isOnline == true)
                {
                    m_MainWindow.systemDate.Visibility = System.Windows.Visibility.Visible;
                    m_MainWindow.tbuserName.Visibility = System.Windows.Visibility.Collapsed;
                    //  m_MainWindow.tbuserName.Text = AppProperties.empUserName;
                    m_MainWindow.tbuserName.Text = AppProperties.empUserName;
                    Hashtable hashMap = CommonUtils.LoadInspectorsUserNameTable();
                    // m_MainWindow.tbuserName.Text = (string)hashMap[AppProperties.empUserName.Trim().ToLower()];
                }
                /*
                Hashtable tb = new Hashtable();
                tb = CommonUtils.LoadInspectorsUserNameTable();
                string name = (string)tb["mhbebars"];
                string name2 = (string)tb["Mohamed Bebars"];*/
                string inspector_pic_path = @"/Images/Inspectors/";
                if (AppProperties.Selected_Resource == "Arabic")
                {
                    inspector_pic_path = inspector_pic_path + m_MainWindow.tbuserName.Text.ToLower() + "_Arabic.png";
                }
                else
                {
                    inspector_pic_path = inspector_pic_path + m_MainWindow.tbuserName.Text.Trim().ToLower() + ".png";
                }

                //     new BitmapImage(new Uri(@"/Images/Buttons/Small/Start Inspection Down.png", UriKind.Relative));
                m_MainWindow.imageRTAInsp.Source = new BitmapImage(new Uri(inspector_pic_path, UriKind.Relative));
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }
        public void EnableSearchOption(bool enab)
        {
            m_MainWindow.menuSerchVehProf.IsEnabled = enab;
            m_MainWindow.menuViolHist.IsEnabled = enab;
            m_MainWindow.menuOperProf.IsEnabled = enab;
            m_MainWindow.mainMenue.IsEnabled = enab;
            m_MainWindow.menuLogout.IsEnabled = enab;
            m_MainWindow.menutItemSynch.IsEnabled = enab;

            m_MainWindow.imgBtnHome.IsEnabled = enab;
            m_MainWindow.imgBtnLogout.IsEnabled = enab;
            m_MainWindow.imagBtnSync.IsEnabled = enab;
            m_MainWindow.imgSerchVehcleProfile.IsEnabled = enab;
            m_MainWindow.imgSerchVioHist.IsEnabled = enab;
            m_MainWindow.imgSerchOpertor.IsEnabled = enab;

            if (enab)
            {
                m_MainWindow.imgBtnHome.Visibility = System.Windows.Visibility.Visible;
                m_MainWindow.imgBtnLogout.Visibility = System.Windows.Visibility.Visible;
                m_MainWindow.imagBtnSync.Visibility = System.Windows.Visibility.Visible;
                m_MainWindow.imgSerchVehcleProfile.Visibility = System.Windows.Visibility.Visible;
                m_MainWindow.imgSerchVioHist.Visibility = System.Windows.Visibility.Visible;
                m_MainWindow.imgSerchOpertor.Visibility = System.Windows.Visibility.Visible;
                m_MainWindow.imageRTAInsp.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                m_MainWindow.imgBtnHome.Visibility = System.Windows.Visibility.Collapsed;
                m_MainWindow.imgBtnLogout.Visibility = System.Windows.Visibility.Collapsed;
                m_MainWindow.imagBtnSync.Visibility = System.Windows.Visibility.Collapsed;
                m_MainWindow.imgSerchVehcleProfile.Visibility = System.Windows.Visibility.Collapsed;
                m_MainWindow.imgSerchVioHist.Visibility = System.Windows.Visibility.Collapsed;
                m_MainWindow.imgSerchOpertor.Visibility = System.Windows.Visibility.Collapsed;
                m_MainWindow.imageRTAInsp.Visibility = System.Windows.Visibility.Collapsed;
            }

        }

        private void btnNext_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {



                if (AppProperties.Selected_Resource == "Arabic")
                {
                    _iValidate = (IValidation)new LocationArValidation();
                    imagebtnNext.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Start Inspection Arabic Up.png", UriKind.Relative));
                    AppProperties.Current_Selected_LocationAr = cmboxLocation.Text;
                    AppProperties.Previous_Selected_AreaAr = cmboxArea.Text;
                    if (AppProperties.Previous_Selected_LocationAr.Equals(AppProperties.Current_Selected_LocationAr))
                    {
                        if (AppProperties.Selected_Location_Count == 5)
                        {
                            if (WPFMessageBox.Show(new CommonUtils().GetStringValue("Confirmation"), new CommonUtils().GetStringValue("lblInspectionConfirmationMessage") + "\"" + AppProperties.Current_Selected_LocationAr + "\"" + "؟", WPFMessageBoxButtons.YesNo, WPFMessageBoxImage.Question) == WPFMessageBoxResult.No)
                            {

                                return;
                            }
                        }
                    }
                }
                else
                {
                    _iValidate = (IValidation)new LocationEnValidation();
                    imagebtnNext.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Start Inspection.png", UriKind.Relative));
                    AppProperties.Current_Selected_LocationEn = cmboxLocation.Text;
                    AppProperties.Previous_Selected_AreaEn = cmboxArea.Text;
                    if (AppProperties.Previous_Selected_LocationEn.Equals(AppProperties.Current_Selected_LocationEn))
                    {
                        if (AppProperties.Selected_Location_Count == 5)
                        {
                            if (WPFMessageBox.Show(new CommonUtils().GetStringValue("Confirmation"), new CommonUtils().GetStringValue("lblInspectionConfirmationMessage") + "\"" + AppProperties.Current_Selected_LocationEn + "\"?", WPFMessageBoxButtons.YesNo, WPFMessageBoxImage.Question) == WPFMessageBoxResult.No)
                            {

                                return;
                            }
                        }
                    }
                }
                _validationResult = _iValidate.Validate(this);
                if (_validationResult != "Valid")
                {
                    WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), _validationResult);
                    return;
                }

                if (AppProperties.recordedViolation == null)
                {
                    AppProperties.recordedViolation = new Violation();
                }
                AppProperties.location = new Violation.InspectionLocation();
                if (AppProperties.Selected_Resource == "Arabic")
                {
                    AppProperties.location.city = (string)citiesTable[(string)cmboxEmirates.SelectedItem];
                    AppProperties.location.area = (string)areasTable[(string)cmboxArea.SelectedItem];
                    AppProperties.location.location = (string)locationTable[(string)cmboxLocation.SelectedItem];
                    // AppProperties.recordedViolation.InspectionArea = AppProperties.location;
                }
                else
                {
                    AppProperties.location.city = ((string)((ucLocationSelectionEn)this).cmboxEmirates.Text).Trim();
                    AppProperties.location.area = ((string)((ucLocationSelectionEn)this).cmboxArea.Text).Trim();
                    AppProperties.location.location = ((string)((ucLocationSelectionEn)this).cmboxLocation.Text).Trim();
                }
                AppProperties.recordedViolation.InspectionArea = AppProperties.location;
                this.m_MainWindow.MainContentControl.Content = m_MainWindow.m_PagesList[4];
                //  this.m_MainWindow.MainContentControl.Content = new  ucRecordViolationNonUAEVehicle(m_MainWindow,this));

                // LandingScreenEn landingEn = new LandingScreenEn();
                // _render.switchDisplay(form, landingEn);

                return;

            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }

        }
        void tmr_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //throw new NotImplementedException();
            this.Dispatcher.Invoke(
                            new ShowGoodBteScreen_Delegate(this.SwitchToLoginScreen),
                            new object[] { }
                        );
            tmr.Enabled = false;
            tmr.Stop();
            tmr.Dispose();
            //  this.Logoff2("");
        }
        public void SwitchToLoginScreen()
        {
            this.m_MainWindow.MainContentControl.Content = null;
            this.m_MainWindow.MainContentControl.Content = new ucLoginEn(m_MainWindow);
            this.m_MainWindow.UpdateLayout();
        }
        private void btnback_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                this.m_MainWindow.MainContentControl.Content = null;
                this.m_MainWindow.MainContentControl.Content = new ucWellComeScreen(m_MainWindow);
                /*
                if (AppProperties.Selected_Resource == "English")
                {
                    imagebtnback.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Back.png", UriKind.Relative));
                }
                else
                {
                    imagebtnback.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/back Arabic up.png", UriKind.Relative));
                }
                if (WPFMessageBox.Show(new CommonUtils().GetStringValue("Confirmation"), new CommonUtils().GetStringValue("lblAppLogout"), WPFMessageBoxButtons.YesNo, WPFMessageBoxImage.Question) == WPFMessageBoxResult.Yes)
                {
                    tmr = new System.Timers.Timer();
                    tmr.Elapsed += tmr_Elapsed;
                    tmr.Interval = 5000;
                    tmr.Enabled = true; //Вкючаем таймер.                       
                    this.m_MainWindow.MainContentControl.Content = new ucGoodByeScreen(m_MainWindow);
                   // this.m_MainWindow.MainContentControl.Content = null;
                   // this.m_MainWindow.MainContentControl.Content = new ucLoginEn(m_MainWindow);
                    //this.m_MainWindow.UpdateLayout();
                }*/
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }
        #endregion


        #region Public Functions
        public ucLocationSelectionEn(MainWindow mainWnd)
        {
            InitializeComponent();
            this.m_MainWindow = mainWnd;
            // FillLocationCombox();
        }
        public void FillLocationCombox()
        {
            try
            {
                if (cmboxEmirates.Items.Count > 0)
                    cmboxEmirates.Items.Clear();
                if (_emirates == null)
                    _emirates = new List<string>();


                if (AppProperties.Selected_Resource == "Arabic")
                {
                    citiesTable = new Hashtable();
                    citiesTable = CommonUtils.Splitter(((IViolation)ViolationManager.GetInstance()).GetLocation(AppProperties.defaultCountry, "city"));
                    string[] cityArray = new string[citiesTable.Count];
                    citiesTable.Keys.CopyTo(cityArray, 0);
                    _emirates = new List<string>(cityArray);
                    _emirates.Sort();
                    foreach (string str in _emirates)
                    {
                        cmboxEmirates.Items.Add(str.Trim());
                    }
                    if (cmboxEmirates.Items.Count > 0)
                    {
                        cmboxEmirates.SelectedItem = AppProperties.defaultEmirateAr;
                    }
                }
                else
                {
                    _emirates = new List<string>(((IViolation)ViolationManager.GetInstance()).GetLocation(AppProperties.defaultCountry, "city"));
                    _emirates.Sort();
                    // string[] cityArray = new string[citiesTable.Count];
                    //citiesTable.Keys.CopyTo(cityArray, 1);
                    //_emirates = new List<string>(cityArray);
                    foreach (string str in _emirates)
                    {
                        cmboxEmirates.Items.Add(str.Trim());
                    }
                    if (cmboxEmirates.Items.Count > 0)
                    {
                        cmboxEmirates.SelectedItem = AppProperties.defaultEmirate;
                    }
                }
                // 


                /*
                if (AppProperties.Selected_Resource == "Arabic")
                {
                    cmboxEmirates.SelectedValue = AppProperties.defaultEmirateAr;
                }
                else
                {
                    cmboxEmirates.SelectedValue = AppProperties.defaultEmirate;
                }*/
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
            // _emirates = 

        }
        #endregion

        private void imagebtnback_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            if (AppProperties.Selected_Resource == "English")
            {
                imagebtnback.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Back Down.png", UriKind.Relative));
            }
            else
            {
                imagebtnback.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/back Arabic Down.png", UriKind.Relative));
            }

        }

        private void imagebtnNext_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {

            if (AppProperties.Selected_Resource == "English")
            {
                imagebtnNext.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Start Inspection Down.png", UriKind.Relative));
            }
            else
            {
                imagebtnNext.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Start Inspection Arabic Down.png", UriKind.Relative));
            }
        }

        private void UserControl_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {

            if (new CommonUtils().GetScreenOrientation() == AppProperties.LandScapetMode)
            {
                // MessageBox.Show("LandScape");
                // ChangeControlDimensions(350);

                //ChangeControlDimensions(AppProperties.LndScp_220_350);
                //ChangeButtonsDimensions(AppProperties.LndScp_Btn_100_140);
                //ChangeLableDimensions(AppProperties.LndScp_Lbl_20_25);
                //ChangeHeaderDimensions(AppProperties.LndScp_Header_25_30);
                //imagebtnNext.Margin = new Thickness(60, 0, 0, 0);

             //   ChangeControlDimensions(350);
             //   ChangeButtonsDimensions(140);
                //   ChangeLableDimensions(AppProperties.LndScp_Lbl_20_25);
                //   ChangeHeaderDimensions(AppProperties.LndScp_Header_25_30);
               // imagebtnNext.Margin = new Thickness(40, 0, 0, 0);
               // imagebtnback.Margin = new Thickness(20, 0, 0, 0);

            }
            else
            {
                //  MessageBox.Show("Potrait");
                // ChangeControlDimensions(250);
              //  ChangeControlDimensions(300);
              //  ChangeButtonsDimensions(120);
                //  ChangeLableDimensions(AppProperties.Prtrt_Lbl_25_20);
                //  ChangeHeaderDimensions(AppProperties.Prtrt_Header_30_25);
              //  imagebtnNext.Margin = new Thickness(20, 0, 0, 0);
               // imagebtnback.Margin = new Thickness(40, 0, 0, 0);
            }
        }
        public void ChangeControlDimensions(double width)
        {
            this.cmboxArea.Width = width;
            this.cmboxEmirates.Width = width;
            this.cmboxLocation.Width = width;
            this.UpdateLayout();
        }
        public void ChangeButtonsDimensions(double width)
        {
            this.imagebtnback.Width = width;
            this.imagebtnNext.Width = width;
            this.UpdateLayout();
        }
        public void ChangeLableDimensions(double width)
        {
            this.lblEmirate.FontSize = width;
            this.lblArea.FontSize = width;
            this.lblLoc.FontSize = width;
            // this.lblAppLogout.FontSize = 20;
            this.UpdateLayout();
        }
        public void ChangeHeaderDimensions(double width)
        {
            this.lblLocationDetail.FontSize = width;
            this.UpdateLayout();
        }
        private void txtBoxPlateNumber_GotFocus_1(object sender, RoutedEventArgs e)
        {
            CommonUtils.ShowKeyBoard();
        }

        private void txtBoxPlateNumber_LostFocus_1(object sender, RoutedEventArgs e)
        {
            CommonUtils.CLoseKeyBoard();

            if (!new CommonUtils().isWordFoundInList(_emirates, cmboxEmirates.Text))
            {
                cmboxEmirates.Text = "";
            }
            if (!new CommonUtils().isWordFoundInList(_areas, cmboxArea.Text))
            {
                cmboxArea.Text = "";
            }
            if (!new CommonUtils().isWordFoundInList(_locations, cmboxLocation.Text))
            {
                cmboxLocation.Text = "";
            }

        }

        private void cmboxEmirates_PreviewKeyDown_1(object sender, KeyEventArgs e)
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

       
        private void cmboxLocation_PreviewKeyDown_2(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Return)
                {
                    btnNext_Click_1(sender, e);
                }
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void cmboxEmirates_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (!cmboxEmirates.IsDropDownOpen)
                cmboxEmirates.IsDropDownOpen = true;
        }

        private void cmboxArea_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (!cmboxArea.IsDropDownOpen)
                cmboxArea.IsDropDownOpen = true;
        }

        private void cmboxLocation_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (!cmboxLocation.IsDropDownOpen)
                cmboxLocation.IsDropDownOpen = true;
        }




    }
    // Object used for communication from JS -> WPF
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public class HtmlInteropInternalTestClass
    {
        public void endDragMarkerCS(Decimal Lat, Decimal Lng)
        {
            // this.tbLocation.Text = Math.Round(Lat, 5) + "," + Math.Round(Lng, 5);
        }
    }
}
