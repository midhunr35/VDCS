using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlServerCe;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading;
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
using ZSDK_API.Comm;
using ZebraBluetoothAdapter;
using System.IO;
using System.IO.Packaging;
using VSDApp.WPFMessageBoxControl;
using System.Diagnostics;
using System.Runtime.InteropServices;



namespace VSDApp.com.rta.vsd.hh.ui
{
    /// <summary>
    /// Interaction logic for ucLoginEn.xaml
    /// </summary>
    public partial class ucLoginEn : UserControl
    {

        #region Data Members
        private MainWindow m_MainWindow = null;
        private IValidation _iValidate;
        private string _validationResult;
        BackgroundWorker worker = new BackgroundWorker();
        #endregion

        public delegate void Login_Delegate();
        public delegate void UpdateTextCallback(string mesg);
        public delegate void ShowWelComeScreen_Delegate();
        System.Timers.Timer tmr = new System.Timers.Timer();
        public ucLoginEn(MainWindow mainWnd)
        {
            InitializeComponent();
            m_MainWindow = mainWnd;
            //  this.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "Images/background.png")));
            // new BitmapImage(new Uri(@"/Images/RTA_ENG_Logo.png", UriKind.Relative));

            //  maingrid

        }

        public void EnableSearchOption(bool enab)
        {
            m_MainWindow.mainBanner.Visibility = System.Windows.Visibility.Collapsed;
            /*
            m_MainWindow.menuSerchVehProf.IsEnabled = enab;
            m_MainWindow.menuViolHist.IsEnabled = enab;
            m_MainWindow.menuOperProf.IsEnabled = enab;
            m_MainWindow.mainMenue.IsEnabled = enab;
            m_MainWindow.menuLogout.IsEnabled = enab;
            m_MainWindow.menutItemSynch.IsEnabled = enab;
            m_MainWindow.imgBtnHome.IsEnabled = enab;
           
            m_MainWindow.imagBtnSync.IsEnabled = enab;
            m_MainWindow.imgSerchVehcleProfile.IsEnabled = enab;
            m_MainWindow.imgSerchVioHist.IsEnabled = enab;
            m_MainWindow.imgSerchOpertor.IsEnabled = enab;
            m_MainWindow.imgBtnIntellegentTarget.IsEnabled = enab;
            if (enab)
            {
                m_MainWindow.imgBtnHome.Visibility = System.Windows.Visibility.Visible;
                m_MainWindow.imgBtnLogout.Visibility = System.Windows.Visibility.Visible;
                m_MainWindow.imagBtnSync.Visibility = System.Windows.Visibility.Visible;
                m_MainWindow.imgSerchVehcleProfile.Visibility = System.Windows.Visibility.Visible;
                m_MainWindow.imgSerchVioHist.Visibility = System.Windows.Visibility.Visible;
                m_MainWindow.imgSerchOpertor.Visibility = System.Windows.Visibility.Visible;
                m_MainWindow.imageRTAInsp.Visibility = System.Windows.Visibility.Visible;
                m_MainWindow.imgInspection.Visibility = System.Windows.Visibility.Visible;
                m_MainWindow.imgVsdTrafficFines.Visibility = System.Windows.Visibility.Visible;
                m_MainWindow.imgBtnIntellegentTarget.Visibility = System.Windows.Visibility.Visible;
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
                m_MainWindow.imgInspection.Visibility = System.Windows.Visibility.Collapsed;
                m_MainWindow.imgVsdTrafficFines.Visibility = System.Windows.Visibility.Collapsed;
                m_MainWindow.imgBtnIntellegentTarget.Visibility = System.Windows.Visibility.Collapsed;
            }
            */
        }


        #region Events
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            //Test



            /////////////////
            imagebuttonetest2.Source = new BitmapImage(new Uri(@"/Images/Buttons/Large/Login.png", UriKind.Relative));


            //Test Message Box

            //((IDBDataLoad)DBDataLoadManager.GetInstance()).SetDefaultConfiguration();
            //if (AppProperties.recordedViolation == null)
            //{
            //    AppProperties.recordedViolation = new Violation();
            //}
            //AppProperties.recordedViolation.Inspector = AppProperties.empUserName;

            try
            {
                // LoginUsingWorkerThread();
                // MessageBox.Show(AppProperties.recordedViolation.Inspector);
                // ProgressDialogResult result = ProgressDialog.ProgressDialog.Execute(this.m_MainWindow, "Loading data...", (bw, we) =>
                //{

                // Thread.Sleep(4000);

                //   String str = ;

                if (AppProperties.Selected_Resource == "English")
                {
                    imagebuttonetest2.Source = new BitmapImage(new Uri(@"/Images/Buttons/Large/Login.png", UriKind.Relative));
                }
                else
                {
                    imagebuttonetest2.Source = new BitmapImage(new Uri(@"/Images/Buttons/Large/Login Arabic Up.png", UriKind.Relative));
                }



                // PrintPreview p = new PrintPreview();
                //p.Show();
                Login();


                //  });

                //  if (result.OperationFailed)
                //     MessageBox.Show("ProgressDialog failed.");
                //  else
                //     MessageBox.Show("ProgressDialog successfully executed.");


            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBoxResult _Result = WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
                // MessageBox.Show(ex.Message);
            }

        }




        #endregion

        public void Login()
        {
            try
            {
                //MessageBox.Show("Button Click");
                //  busyIndicator.IsBusy = true;
              //  CultureInfo culture = new CultureInfo("en-US");
                             

            //    DateTime testValue = new DateTime(2013, 12, 15, 15, 33, 44);

               
                //////////////////////////////////////////////////////////////////
                if (AppProperties.Selected_Resource == "English")
                {

                    _iValidate = (IValidation)new LoginEnValidation();
                }
                else
                {
                    _iValidate = (IValidation)new LoginArValidation();
                }
                _validationResult = _iValidate.Validate(this);

                if (_validationResult != "Valid")
                {

                    //WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), _validationResult, "" );
                   
                    MessageBox.Show(_validationResult,"",MessageBoxButton.OK,MessageBoxImage.Information);
                    return;
                }

                if (AppProperties.recordedViolation == null)
                {
                    AppProperties.recordedViolation = new Violation();
                }
                // MessageBox.Show("Connecting to Database");

                // SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
                //MessageBox.Show(con.ConnectionString);
                //((IDBDataLoad)DBDataLoadManager.GetInstance()).SetDefaultConfiguration();
                //AppProperties.recordedViolation.Inspector = AppProperties.empUserName;





                ILoginManager iLogin = (ILoginManager)LoginManager.GetInstance();
                AppProperties.empUserName = ((ucLoginEn)this).txtBoxUserName.Text;
                AppProperties.empPassword = ((ucLoginEn)this).txtpswd.Password.ToString();

                bool check = false;
                //  check = iLogin.LoginOnline(AppProperties.empUserName, AppProperties.empPassword);

                try
                {



                    /*
                     ProgressDialogResult result = ProgressDialog.ProgressDialog.Execute(this.m_MainWindow, lblLoggingIn.Content.ToString(), (bw, we) =>
                 {
                     check = iLogin.LoginOnline(AppProperties.empUserName, AppProperties.empPassword);
                 },ProgressDialogSettings.WithSubLabelAndCancel);


                     if (result.Cancelled)
                         return;
                     else if (result.OperationFailed)
                         return;
                   */


                    ProgressDialogResult result = ProgressDialog.ProgressDialog.Execute(this.m_MainWindow, new CommonUtils().GetStringValue("lblLoggingIn"), (bw, we) =>
                    {

                        check = iLogin.LoginOnline(AppProperties.empUserName, AppProperties.empPassword);

                        // So this check in order to avoid default processing after the Cancel button has been pressed.
                        // This call will set the Cancelled flag on the result structure.
                        ProgressDialog.ProgressDialog.CheckForPendingCancellation(bw, we);

                    }, ProgressDialogSettings.WithSubLabelAndCancel);

                    if (result == null || result.Cancelled)
                        return;
                    else if (result.OperationFailed)
                        return;

                    //if (check)
                    if (check)
                    {
                        App.VSDLog.Info("\n ucLoginEn.Loging(): successfully login online(from backend service):");
                        if (AppProperties.recordedViolation == null)
                        {
                            AppProperties.recordedViolation = new Violation();
                        }
                        ((IDBDataLoad)DBDataLoadManager.GetInstance()).SetDefaultConfiguration();
                        AppProperties.recordedViolation.Inspector = AppProperties.empUserName;
                        // LocationScreenEn locationScreen = new LocationScreenEn();
                        // _render.switchDisplay(form, locationScreen);
                        busyIndicator.IsBusy = false;
                        // this.m_MainWindow.MainContentControl.Content = m_MainWindow.m_PagesList[2];
                        //   this.m_MainWindow.MainContentControl.Content = new ucLocationSelectionEn(m_MainWindow);



                        //Timer to show Weolcom Screen
                        tmr = new System.Timers.Timer();
                        tmr.Elapsed += tmr_Elapsed;
                        // tmr.Interval = 5000; 

                        tmr.Interval = 5000;
                        tmr.Enabled = true; //Вкючаем таймер.   

                        this.m_MainWindow.MainContentControl.Content = new ucWellComeScreen(m_MainWindow);


                        AppProperties.isUserLoggedIn = true;


                    }
                    else
                    {

                        //  WPFMessageBoxResult _ServiceResult = WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), AppProperties.exceptionFromServiceCall, "", WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
                        //  ProgressDialogResult result2 = ProgressDialog.ProgressDialog.Execute(this.m_MainWindow, lblLoggingIn.Content.ToString() + "...", (bw, we) =>
                        //{

                        // });


                        ProgressDialogResult result_offline = ProgressDialog.ProgressDialog.Execute(this.m_MainWindow, new CommonUtils().GetStringValue("lblLoggingIn"), (bw, we) =>
                        {

                            check = iLogin.OfflineLogin(AppProperties.empUserName, AppProperties.empPassword);

                            // So this check in order to avoid default processing after the Cancel button has been pressed.
                            // This call will set the Cancelled flag on the result structure.
                            ProgressDialog.ProgressDialog.CheckForPendingCancellation(bw, we);

                        }, ProgressDialogSettings.WithSubLabelAndCancel);

                        if (result_offline == null || result_offline.Cancelled)
                            return;
                        else if (result_offline.OperationFailed)
                            return;
                        if (check)
                        {
                            App.VSDLog.Info("\n ucLoginEn.Loging(): successfully offline login(from local DB):");
                            if (AppProperties.recordedViolation == null)
                            {
                                AppProperties.recordedViolation = new Violation();
                            }
                            ((IDBDataLoad)DBDataLoadManager.GetInstance()).SetDefaultConfiguration();
                            AppProperties.recordedViolation.Inspector = AppProperties.empUserName;
                            busyIndicator.IsBusy = false;
                            AppProperties.isUserLoggedIn = true;
                            tmr = new System.Timers.Timer();
                            tmr.Elapsed += tmr_Elapsed;
                            tmr.Interval = 5000;
                            tmr.Enabled = true;                        
                            this.m_MainWindow.MainContentControl.Content = new ucWellComeScreen(m_MainWindow);

                            //  this.m_MainWindow.MainContentControl.Content = m_MainWindow.m_PagesList[2];


                        }
                        else
                        {
                            App.VSDLog.Info("\n ucLoginEn.Loging(): Not Authorized from local DB:");
                            WPFMessageBoxResult _Result = WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), lblMessageBoxNotAuthorized.Content.ToString());
                            // MessageBox.Show(lblMessageBoxNotAuthorized.Content.ToString());

                          //  App.VSDLog.Info(e.StackTrace); 
                            //busyIndicator.IsBusy = false;
                            //To do 
                            // this.m_MainWindow.MainContentControl.Content = m_MainWindow.m_PagesList[2];
                        }
                        // MessageBox.Show(lblMessageBoxNotAuthorized.Content.ToString());
                        //busyIndicator.IsBusy = false;
                        //check = true;
                    }
                    check = true;
                }
                catch (Exception e)
                {

                    App.VSDLog.Info(e.StackTrace);
                    WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), AppProperties.exceptionFromServiceCall, "", WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
                    //System.Windows.Forms.MessageBox.Show(_resources.GetString("Unable to connect to the remote server"));
                    // System.Windows.Forms.MessageBox.Show(e.Message);
                    if (!check)
                    {
                        check = iLogin.OfflineLogin(AppProperties.empUserName, AppProperties.empPassword);
                        if (check)
                        {
                            App.VSDLog.Info("\n ucLoginEn.Loging(): login offline(from local DB) through catch block:");
                            if (AppProperties.recordedViolation == null)
                            {
                                AppProperties.recordedViolation = new Violation();
                            }
                            ((IDBDataLoad)DBDataLoadManager.GetInstance()).SetDefaultConfiguration();
                            AppProperties.recordedViolation.Inspector = AppProperties.empUserName;
                            busyIndicator.IsBusy = false;
                            tmr = new System.Timers.Timer();
                            tmr.Elapsed += tmr_Elapsed;
                            tmr.Interval = 5000;
                            tmr.Enabled = true; //Вкючаем таймер.                       
                            this.m_MainWindow.MainContentControl.Content = new ucWellComeScreen(m_MainWindow);
                            //  this.m_MainWindow.MainContentControl.Content = new ucLocationSelectionEn(m_MainWindow);


                        }
                        else
                        {
                            WPFMessageBoxResult _Result = WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), lblMessageBoxNotAuthorized.Content.ToString());
                            busyIndicator.IsBusy = false;
                            //this.m_MainWindow.MainContentControl.Content = m_MainWindow.m_PagesList[2];
                        }
                        check = true;
                    }
                }
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBoxResult _Result = WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
                //MessageBox.Show(ex.Message.ToString());
            }
        }

        void tmr_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //throw new NotImplementedException();
            /*
            this.Dispatcher.Invoke(
                            new ShowWelComeScreen_Delegate(this.SwichToLocationScreen),
                            new object[] {  }
                        );
            tmr.Enabled = false;
            tmr.Stop();
            tmr.Dispose();*/
            //  this.Logoff2("");
        }


        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            m_MainWindow.tbuserName.Visibility = System.Windows.Visibility.Collapsed;
            m_MainWindow.txtlocation.Visibility = System.Windows.Visibility.Collapsed;
            AppProperties.Previous_Selected_LocationEn = string.Empty;
            AppProperties.Previous_Selected_LocationAr = string.Empty;
            AppProperties.Previous_Selected_AreaEn = string.Empty;
            AppProperties.Previous_Selected_AreaAr = string.Empty;
            AppProperties.Current_Selected_LocationAr = string.Empty;
            AppProperties.Current_Selected_LocationEn = string.Empty;
            AppProperties.Selected_Location_Count = 0;
            AppProperties.Total_Vehicle_Inspected = 0;
            AppProperties.Is_NewLogin = true;
            EnableSearchOption(false);
            SelectLanguage();

           // VSDApp.com.rta.vsd.hh.gps.GPSLocation gpsLoc = new VSDApp.com.rta.vsd.hh.gps.GPSLocation(m_MainWindow);
          //  gpsLoc.SaveHandHeldCurrentGeoCoardinates();
           // NotificationWindow w = new NotificationWindow("Test");
           // w.Show();
           // Thread.Sleep(new TimeSpan(0, 0, 15));
          //   w.Close();

              //  gpsLoc.SaveHandHeldCurrentLocation();
            if (cmbLanguageSelection.Items.Count == 0)
            {
                cmbLanguageSelection.Items.Add("English");
                cmbLanguageSelection.Items.Add("العربية");
                if (Thread.CurrentThread.CurrentCulture.ToString() == "ar-AE")
                {
                    cmbLanguageSelection.SelectedItem = "العربية";
                }
                else
                {
                    cmbLanguageSelection.SelectedItem = "English";
                }
            }

            txtBoxUserName.Text = new CommonUtils().GetStringValue("UserName");
            txtBoxUserName.Foreground = Brushes.LightGray;
            txtpswd.Foreground = Brushes.LightGray;
           
           // txt
            txtpswd.Password = new CommonUtils().GetStringValue("Password");
            txtBoxUserNamePopup.IsOpen = false;


        }

        private void txtBoxUserName_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            

            //  txtBoxUserName.ToolTip = "UserName Would in CAPS";
            /*
            Boolean Capslock = Console.CapsLock;
            if (Capslock == true)
            {
                txtBoxUserNamePopup.IsOpen = false;

               
            }
            else
            {
                PopupTextBlock.Text = "UserName would be in CAPS";
                txtBoxUserNamePopup.IsOpen = true;
            }*/
            if (AppProperties.Selected_Resource == "English")
            {
                if (txtBoxUserName.Text.Trim() == "" || txtBoxUserName.Text.Equals(new CommonUtils().GetStringValue("UserName")))
                {
                    txtBoxUserNamePopup.IsOpen = false;
                }
                else
                {
                    PopupTextBlock.Text = "Hint: Username and Password is Key Sensitive";
                    txtBoxUserNamePopup.IsOpen = true;
                }
            }
            else
            {
                if (txtBoxUserName.Text.Trim() == "" || txtBoxUserName.Text.Equals(new CommonUtils().GetStringValue("UserName")))
                {
                    txtBoxUserNamePopup.IsOpen = false;
                }
                else
                {
                    PopupTextBlock.Text = "يرجى مراعاة حجم الحروف";
                    txtBoxUserNamePopup.IsOpen = true;
                }
            }


        }


        private void txtBoxUserName_LostFocus_1(object sender, RoutedEventArgs e)
        {
            txtBoxUserNamePopup.IsOpen = false;
            CommonUtils.CLoseKeyBoard();
            if (txtBoxUserName.Text.Length < 1)
            {
                txtBoxUserName.Text = new CommonUtils().GetStringValue("UserName");
                txtBoxUserName.Foreground = Brushes.LightGray;
            }
            else
            {
                txtBoxUserName.Foreground = Brushes.Black;                
            }

        }
        private void txtpswd_LostFocus_1(object sender, RoutedEventArgs e)
        {
            CommonUtils.CLoseKeyBoard();
            if (txtpswd.Password.Length < 1)
            {
                txtpswd.Password = new CommonUtils().GetStringValue("Password");
                txtpswd.Foreground = Brushes.LightGray;
               
            }
            else
            {
                txtpswd.Foreground = Brushes.Black;
            }
        }

        private void txtBoxUserName_GotFocus_1(object sender, RoutedEventArgs e)
        {
            CommonUtils.ShowKeyBoard();
            if (txtBoxUserName.Text.Equals(new CommonUtils().GetStringValue("UserName")))
            {
                txtBoxUserName.Text = "";
                txtBoxUserName.Foreground = Brushes.Black;
            }
         
        }
        private void txtpswd_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtpswd.Password.ToString().Equals((new CommonUtils().GetStringValue("Password"))))
            {
                txtpswd.Password = "";
                txtpswd.Foreground = Brushes.Black;
              
            }
            else
            {
               
            }
            CommonUtils.ShowKeyBoard();

        }
       
        private void Image_MouseDown_1(object sender, MouseButtonEventArgs e)
        {


        }

        private void Image_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {

        }

        private void Image_MouseLeave_1(object sender, MouseEventArgs e)
        {

        }


        private void imagebuttonetest2_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {


            if (AppProperties.Selected_Resource == "English")
            {
                imagebuttonetest2.Source = new BitmapImage(new Uri(@"/Images/Buttons/Large/Login Down.png", UriKind.Relative));
            }
            else
            {
                imagebuttonetest2.Source = new BitmapImage(new Uri(@"/Images/Buttons/Large/Login Arabic Down.png", UriKind.Relative));
            }

        }

        private void imagebuttonetest2_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {

        }

        private void Hyperlink_Click_1(object sender, RoutedEventArgs e)
        {            
            try
            {
                Thread.CurrentThread.CurrentCulture.ToString();
                AppProperties.Is_CloseMainApp = true;
                if (Thread.CurrentThread.CurrentCulture.ToString() == "ar-AE")
                {
                    App.ChangeCulture(new CultureInfo("en-US"));
                    AppProperties.Selected_Resource = "English";
                    // this.FlowDirection = System.Windows.FlowDirection.RightToLeft;
                }
                else
                {
                    App.ChangeCulture(new CultureInfo("ar-AE"));
                    AppProperties.Selected_Resource = "Arabic";
                    //this.FlowDirection = System.Windows.FlowDirection;
                }

            }
            catch (Exception ex)
            {
                WPFMessageBoxResult _Result = WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void Hyperlink_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                Thread.CurrentThread.CurrentCulture.ToString();
                AppProperties.Is_CloseMainApp = true;

                if (Thread.CurrentThread.CurrentCulture.ToString() == "ar-AE")
                {
                    App.ChangeCulture(new CultureInfo("en-US"));
                    AppProperties.Selected_Resource = "English";
                    // this.FlowDirection = System.Windows.FlowDirection.RightToLeft;
                }
                else
                {
                    App.ChangeCulture(new CultureInfo("ar-AE"));
                    AppProperties.Selected_Resource = "Arabic";
                    //  Thread.CurrentThread.CurrentCulture.NumberFormat.DigitSubstitution = DigitShapes.NativeNational;
                    //this.FlowDirection = System.Windows.FlowDirection;
                }
                ChangeButtonVisibility();
            }
            catch (Exception ex)
            {
                WPFMessageBoxResult _Result = WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        public void SelectLanguage()
        {
            try
            {

                if (Thread.CurrentThread.CurrentCulture.ToString() == "ar-AE")
                {
                    txtBlockLanguageArb.IsEnabled = false;
                    hyperlinkArb.Foreground = Brushes.Wheat;
                    hyperlinkEng.Foreground = Brushes.Blue;
                    txtBlockLanguageEng.IsEnabled = true;
                    m_MainWindow.rdbtnArabic.IsChecked = true;
                    m_MainWindow.rdbtnEnglis.IsChecked = false;
                   // cmbLanguageSelection.SelectedIndex = 1;
                    imagebuttonetest2.Source = new BitmapImage(new Uri(@"/Images/Buttons/Large/Login Arabic Up.png", UriKind.Relative));
                    m_MainWindow.imageRTAInsp.Source = new BitmapImage(new Uri(@"/Images/Inspectors/User Arabic Up.png", UriKind.Relative));

                    // this.FlowDirection = System.Windows.FlowDirection.RightToLeft;

                }
                else
                {
                    txtBlockLanguageArb.IsEnabled = true;
                    txtBlockLanguageEng.IsEnabled = false;
                    hyperlinkEng.Foreground = Brushes.Wheat;
                    hyperlinkArb.Foreground = Brushes.Blue;
                    m_MainWindow.rdbtnArabic.IsChecked = false;
                    m_MainWindow.rdbtnEnglis.IsChecked = true;
                   // cmbLanguageSelection.SelectedIndex = 0;
                    imagebuttonetest2.Source = new BitmapImage(new Uri(@"/Images/Buttons/Large/Login.png", UriKind.Relative));
                    m_MainWindow.imageRTAInsp.Source = new BitmapImage(new Uri(@"/Images/Inspectors/User.png", UriKind.Relative));
                    //this.FlowDirection = System.Windows.FlowDirection;
                }

            }
            catch (Exception ex)
            {
                WPFMessageBoxResult _Result = WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }
        public void ChangeButtonVisibility()
        {
            if (AppProperties.Selected_Resource == "English")
            {
                imagebuttonetest2.Source = new BitmapImage(new Uri(@"/Images/Buttons/Large/Login.png", UriKind.Relative));
                m_MainWindow.imageRTAInsp.Source = new BitmapImage(new Uri(@"/Images/Inspectors/User.png", UriKind.Relative));
            }
            else
            {
                imagebuttonetest2.Source = new BitmapImage(new Uri(@"/Images/Buttons/Large/Login Arabic Up.png", UriKind.Relative));
                m_MainWindow.imageRTAInsp.Source = new BitmapImage(new Uri(@"/Images/Inspectors/User Arabic Up.png", UriKind.Relative));
            }
        }

        private void hyperlinkForgotName_Click_1(object sender, RoutedEventArgs e)
        {

            WPFMessageBoxResult _Result = WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("lblForgotUsernameMessage"));
           // this.m_MainWindow.MainContentControl.Content = null;
          //  this.m_MainWindow.MainContentControl.Content = new ucVSDPortalWPF(m_MainWindow);
        }


        private void SwichToLocationScreen()
        {
            this.m_MainWindow.MainContentControl.Content = null;
            this.m_MainWindow.MainContentControl.Content = new ucLocationSelectionEn(m_MainWindow);
            this.UpdateLayout();
            // MessageBox.Show("LogOf");
        }

        private void txtBoxUserName_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            CommonUtils.ShowKeyBoard();
        }
        private void UserControl_Initialized_1(object sender, EventArgs e)
        {
            //System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.DigitSubstitution = System.Globalization.DigitShapes.None;
            // this.UpdateLayout();
        }
        private void txtBoxUserName_PreviewKeyDown_1(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Return)
                {
                    new CommonUtils().ChangeControlFocous(e);
                    /*
                    // Creating a FocusNavigationDirection object and setting it to a
                    // local field that contains the direction selected.
                    FocusNavigationDirection focusDirection = FocusNavigationDirection.Next;

                    // MoveFocus takes a TraveralReqest as its argument.
                    TraversalRequest request = new TraversalRequest(focusDirection);

                    // Gets the element with keyboard focus.
                    UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;

                    // Change keyboard focus.
                    if (elementWithFocus != null)
                    {
                        if (elementWithFocus.MoveFocus(request)) e.Handled = true;
                    }*/
                }
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void txtpswd_PreviewKeyDown_1(object sender, KeyEventArgs e)
        {

        }

        private void TextBlock_PreviewKeyDown_1(object sender, KeyEventArgs e)
        {
           
            try
            {
                if (e.Key == Key.Return)
                {
                    Button_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                WPFMessageBoxResult _Result = WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void btnResetVehicleRecord_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (new CommonUtils().GetScreenOrientation() == AppProperties.LandScapetMode)
            {
                stckPanelUpperMain.Orientation = Orientation.Horizontal;
               
             //   lblRtaApp.Margin = t;
               
            }
            else
            {
                stckPanelUpperMain.Orientation = Orientation.Vertical;
               
               // lblRtaApp.Margin = t;
            }
        }

        private void cmboxEnglish_Selected(object sender, RoutedEventArgs e)
        {
            try
            {
                Thread.CurrentThread.CurrentCulture.ToString();
                AppProperties.Is_CloseMainApp = true;
                if (Thread.CurrentThread.CurrentCulture.ToString() == "ar-AE")
                {
                    App.ChangeCulture(new CultureInfo("en-US"));
                    AppProperties.Selected_Resource = "English";
                    // this.FlowDirection = System.Windows.FlowDirection.RightToLeft;
                }
                else
                {
                    App.ChangeCulture(new CultureInfo("ar-AE"));
                    AppProperties.Selected_Resource = "Arabic";
                    //this.FlowDirection = System.Windows.FlowDirection;
                }

            }
            catch (Exception ex)
            {
                WPFMessageBoxResult _Result = WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void cmboxArabic_Selected(object sender, RoutedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                WPFMessageBoxResult _Result = WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void cmbLanguageSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if ((string)cmbLanguageSelection.SelectedItem != "")
                {
                    Thread.CurrentThread.CurrentCulture.ToString();
                    AppProperties.Is_CloseMainApp = true;

                    if (Thread.CurrentThread.CurrentCulture.ToString() == "ar-AE" && (string)cmbLanguageSelection.SelectedItem == "English")
                    {
                        App.ChangeCulture(new CultureInfo("en-US"));
                        AppProperties.Selected_Resource = "English";
                      //  cmbLanguageSelection.SelectedIndex = 1;
                    }
                    else if ((Thread.CurrentThread.CurrentCulture.ToString() == "en-US" || Thread.CurrentThread.CurrentCulture.ToString() == "en-GB") && (string)cmbLanguageSelection.SelectedItem == "العربية")
                    {
                        App.ChangeCulture(new CultureInfo("ar-AE"));
                        AppProperties.Selected_Resource = "Arabic";
                      //  cmbLanguageSelection.SelectedIndex = 2;
                        //this.FlowDirection = System.Windows.FlowDirection;
                    }
                   

                }

            }
            catch (Exception ex)
            {
                WPFMessageBoxResult _Result = WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

       
       



    }
}

