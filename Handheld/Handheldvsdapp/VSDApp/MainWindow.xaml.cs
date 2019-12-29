using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using log4net;
using VSDApp.com.rta.vsd.hh.manager;
using VSDApp.com.rta.vsd.hh.ui;
using VSDApp.com.rta.vsd.hh.utilities;
using VSDApp.ProgressDialog;
using VSDApp.WPFMessageBoxControl;
using VSDApp.com.rta.vsd.hh.db;
namespace VSDApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml Height="650" Width="1024"
    /// </summary>
    public partial class MainWindow : Window
    {

        #region DataMembers
        public System.Collections.Generic.Dictionary<int, UserControl> m_PagesList = null;
        private int m_currentPage = 1;
        public int logOffTime = 45;
        DispatcherTimer statusTimer;
        CloseAdminPermission closeadminWindow = null;
        public delegate void ShowGoodByeScreen_Delegate();
        System.Timers.Timer timer;
        System.Timers.Timer tmr;
        public delegate void InternetConnectivity_Delegate();
        BackgroundWorker m_oWorker;
        public bool isNetworkConnected = false;
        #endregion
       
       

        #region Properties
        public int CurrentPage
        {
            get { return m_currentPage; }
            set { m_currentPage = value; }
        }
        #endregion


        public MainWindow()
        {
            //Properties.Resources.Culture = new CultureInfo("ar-AE");
            //this.FlowDirection = System.Windows.FlowDirection.RightToLeft;
            AppProperties.Selected_Resource = "English";
            InitializeComponent();
            log4net.Config.XmlConfigurator.Configure();
            // this.MainContentControl = new ucLogin();
            m_PagesList = new Dictionary<int, UserControl>();
            // ucLoginEn LoginControl = new ucLoginEn();
            //this.MainContentControl = LoginControl;
            this.HideMinimizeAndMaximizeButtons();

            m_oWorker = new BackgroundWorker();


            m_oWorker.DoWork += m_oWorker_DoWork;

            m_oWorker.RunWorkerCompleted += m_oWorker_RunWorkerCompleted;
            m_oWorker.WorkerReportsProgress = true;
            m_oWorker.WorkerSupportsCancellation = true;
            ucInspectorSumeryDialog.SetParent(grdMain);
            ucAlprSelectionOption.SetParent(grdMain2);
           
           // App.VSDLog.Debug("Test 3");
          //  App.VSDLog.Info("Test Message");
            

        }

        void m_oWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Dispatcher.Invoke(
                            new InternetConnectivity_Delegate(this.SetLogoLocation),
                            new object[] { }
                        );

        }

        void m_oWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            CheckForInternetConnection();
        }
        public bool CheckForInternetConnection()
        {
            try
            {
                isNetworkConnected = false;
                AppProperties.isNetworkConnected = false;
                using (var client = new WebClient())
                using (var stream = client.OpenRead("http://172.17.85.2/HHService/HandHeldService/WEB-INF/wsdl/HandHeldService.wsdl"))
                {
                    isNetworkConnected = true;
                    AppProperties.isNetworkConnected = true;
                    return true;

                }
            }
            catch
            {
                isNetworkConnected = false;
                AppProperties.isNetworkConnected = false;
                return false;
            }
        }
        //public void ChangeIconStatus()
        //{

        //    if (isNetworkConnected)
        //    {
        //        imgBtnNetworkConnectivity.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Network_Connect.png", UriKind.Relative));
        //    }
        //    else
        //    {
        //        imgBtnNetworkConnectivity.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Network_Disconnet.png", UriKind.Relative));
        //    }
        //}

        #region Events



        /// <summary>
        /// Windows Loaded Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // (string)citiesTable[(string)cmboxEmirates.SelectedItem];

                // var dateString = dateTime.ToString("dd MMMM yyyy", System.Globalization.CultureInfo.GetCultureInfo("ar"));
                //  this.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "Images/background.png")));





                //   System.Drawing.Rectangle size =    System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size;



                // ViewManagment
                //////////////////////
                LoadPages();

                MainContentControl.Content = null;
                MainContentControl.Content = m_PagesList[m_currentPage];
                this.UpdateLayout();
                ///


                this.statusTimer = new DispatcherTimer();
                this.statusTimer.Interval = TimeSpan.FromSeconds(1);
                this.statusTimer.Start();
                this.statusTimer.Tick += new EventHandler(statusTimer_Tick);
                ReadConfigInfo();
                InitializeAutoLogoffFeature();

                //SetLogoLocation();
                AppProperties.isUserLoggedIn = false;

                if (AppProperties.Selected_Resource == "Arabic")
                {
                    //imgNew.FlowDirection = System.Windows.FlowDirection.LeftToRight;
                }

                //Height
                /*
                int MonitorHeightinInches = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height / 96;
                int exactHeigh = MonitorHeightinInches * 96;
                //Width
                int MonitorWidthinInches = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width / 96;
                int exactWidth = MonitorWidthinInches * 96;*/
                /*
                
                                System.Drawing.Rectangle resolution = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
                               int height1= resolution.Height;
                               int width1 = resolution.Width;


                               this.Height = height;
                               this.Width = width;
                               FormState formState = new FormState();*/
                //  System.Windows.Forms.Form form = this;
                // formState.Maximize(this as System.Windows.Forms.Form);
                this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;

                timer = new System.Timers.Timer();
                timer.Elapsed += timer_Elapsed;
                timer.Interval = 500;
                timer.Enabled = true;


            }
            catch (Exception ex)
            {
                WPFMessageBoxResult _Result = WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
                // throw;
            }

        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!m_oWorker.IsBusy)
                m_oWorker.RunWorkerAsync();
        }
        #endregion

        #region Functions Implelemtation
        private void LoadPages()
        {
            m_PagesList.Add(1, new ucLoginEn(this));
            m_PagesList.Add(2, new ucLocationSelectionEn(this));
            m_PagesList.Add(3, new ucLandingScreenEn(this));
            m_PagesList.Add(4, new ucVehicleSelection(this));
            // m_PagesList.Add(5, new ucRecordViolationNonUAEVehicle(this));
            m_PagesList.Add(6, new ucVehicleProfileInspection(this));

        }
        public void SetLogoLocation()
        {
            try
            {
                if (AppProperties.Selected_Resource == "English")
                {

                    this.imgRTARight.Source = new BitmapImage(new Uri(@"/Images/RTA_ENG_Logo.png", UriKind.Relative));
                    if (isNetworkConnected)
                    { this.imgRTALeft.Source = new BitmapImage(new Uri(@"/Images/RTA_Urdu_OnlineEn_Logo.png", UriKind.Relative)); }
                    else
                    { this.imgRTALeft.Source = new BitmapImage(new Uri(@"/Images/RTA_Urdu_OfflineEn_Logo.png", UriKind.Relative)); }



                }
                else
                {
                    this.imgRTALeft.Source = new BitmapImage(new Uri(@"/Images/RTA_ENG_Logo.png", UriKind.Relative));
                    if (isNetworkConnected)
                        this.imgRTARight.Source = new BitmapImage(new Uri(@"/Images/RTA_Urdu_OnlineAr_Logo.png", UriKind.Relative));
                    else
                        this.imgRTARight.Source = new BitmapImage(new Uri(@"/Images/RTA_Urdu_OfflineAr_Logo.png", UriKind.Relative));

                }

                imgRTALeft.Width = 300;
                imgRTARight.Width = 300;
                this.UpdateLayout();
            }
            catch (Exception ex)
            {
                WPFMessageBoxResult _Result = WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }
        #endregion

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {

            /*
            if (AppProperties.Selected_Resource == "English")
            {
                imgBtnExit.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Exit Up.png", UriKind.Relative));
            }
            else
            {
                imgBtnExit.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Exit Arabic Up.png", UriKind.Relative));
            }
            */

            /*
            if (closeadminWindow == null)
            {
                closeadminWindow = new CloseAdminPermission(this);
                closeadminWindow.Closing += closeadminWindow_Closing;
                closeadminWindow.Owner = this;
                closeadminWindow.Show();
            }
            */
            WPFMessageBoxResult __Result = WPFMessageBox.Show(new CommonUtils().GetStringValue("Confirmation"), new CommonUtils().GetStringValue("lblCloseApplicationMessage"), WPFMessageBoxButtons.YesNo, WPFMessageBoxImage.Question);

           // if (MessageBox.Show(lblCloseApplicationMessage.Content.ToString() + "?", lblCloseApplication.Content.ToString(), MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            if(__Result == WPFMessageBoxResult.Yes)
            {
                this.Close();
            }
        }

        void closeadminWindow_Closing(object sender, CancelEventArgs e)
        {
            try
            {
                if (AppProperties.Is_ClosePermissionWindow == true)
                {
                    this.Close();
                    Application.Current.Shutdown();
                    closeadminWindow = null;

                }
                else
                {
                    closeadminWindow = null;
                }

            }
            catch (Exception ex)
            {
                WPFMessageBoxResult _Result = WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
                // throw;
            }


        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            if (AppProperties.Selected_Resource == "English")
            {
                imgBtnHome.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Home Up.png", UriKind.Relative));
            }
            else
            {
                imgBtnHome.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Home Arabic Up.png", UriKind.Relative));
            }
            //------added by Kashif abbasi on dated 22-Nov-2015 ----------------
            string strScreenName = this.MainContentControl.Content.ToString().Substring(this.MainContentControl.Content.ToString().LastIndexOf(".") + 1);
            bool result = CommonUtils.deleteImgDirectory(strScreenName);//delete the defect image diractory if created made img and thn not submitted violation
            //-------------------------------------------------------------------
            if (result)
            {
                this.MainContentControl.Content = null;
                //this.MainContentControl.Content = new ucLocationSelectionEn(this);
                this.MainContentControl.Content = new ucWellComeScreen(this);
            }
        }


        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
        public static IEnumerable<DependencyObject> FindInVisualTreeDown(DependencyObject obj, Type type)
        {
            if (obj != null)
            {
                if (obj.GetType() == type)
                {
                    yield return obj;
                }

                for (var i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
                {
                    foreach (var child in FindInVisualTreeDown(VisualTreeHelper.GetChild(obj, i), type))
                    {
                        if (child != null)
                        {
                            yield return child;
                        }
                    }
                }
            }

            yield break;
        }

        public static void ChangeCulture(CultureInfo newCulture)
        {
            Thread.CurrentThread.CurrentCulture = newCulture;
            Thread.CurrentThread.CurrentUICulture = newCulture;

            var OldWindow = Application.Current.MainWindow;
            Application.Current.MainWindow = new Window();
            Application.Current.MainWindow.Show();
            OldWindow.Close();
        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {


        }
        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {

            try
            {

                if (AppProperties.Selected_Resource == "English")
                {
                    imgBtnLanguage.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Arabic Up.png", UriKind.Relative));
                }
                else
                {

                    imgBtnLanguage.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/English Up.png", UriKind.Relative));
                }

                //------added by Kashif abbasi on dated 22-Nov-2015 ----------------
                string strScreenName = this.MainContentControl.Content.ToString().Substring(this.MainContentControl.Content.ToString().LastIndexOf(".") + 1);
                bool result = CommonUtils.deleteImgDirectory(strScreenName);//delete the defect image diractory if created made img and thn not submitted violation
                //-------------------------------------------------------------------
                if (result)
                {
                    if (AppProperties.isUserLoggedIn == true)
                    {

                        if (WPFMessageBox.Show(new CommonUtils().GetStringValue("Confirmation"), lblWouldYouLikeLang.Content.ToString(), WPFMessageBoxButtons.YesNo, WPFMessageBoxImage.Question) == WPFMessageBoxResult.Yes)
                        {
                            synchInspectorInfo();
                            tmr = new System.Timers.Timer();
                            tmr.Elapsed += tmr_Elapsed2;
                            tmr.Interval = 8000;
                            tmr.Enabled = true;


                        }
                    }
                    else
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
                }

            }
            catch (Exception ex)
            {
                WPFMessageBoxResult _Result = WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }



        }
        void tmr_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //throw new NotImplementedException();
           /*
            this.Dispatcher.Invoke(
                            new ShowGoodByeScreen_Delegate(this.SwitchToLoginScreen),
                            new object[] { }
                        );*/
            tmr.Enabled = false;
            tmr.Stop();
            tmr.Dispose();
            //  this.Logoff2("");
        }
        void tmr_Elapsed2(object sender, System.Timers.ElapsedEventArgs e)
        {
            //throw new NotImplementedException();
            /*
            this.Dispatcher.Invoke(
                            new ShowGoodByeScreen_Delegate(this.SwitchLang),
                            new object[] { }
                        );*/
            tmr.Enabled = false;
            tmr.Stop();
            tmr.Dispose();
            //  this.Logoff2("");
        }
        public void SwitchLang()
        {
            AppProperties.isUserLoggedIn = false;
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
        public void SwitchToLoginScreen()
        {
            this.MainContentControl.Content = null;
            this.MainContentControl.Content = new ucLoginEn(this);
            this.UpdateLayout();
        }
        private void MenuItem_Click_5(object sender, RoutedEventArgs e)
        {

        }



        private void MenuItem_Click_6(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Click_7(object sender, RoutedEventArgs e)
        {



        }

        private void MenuItem_Click_8(object sender, RoutedEventArgs e)
        {


        }

        private void Window_ContentRendered_1(object sender, EventArgs e)
        {

            SetLogoLocation();
            LoadButtonImages();

        }

        private void Window_Closing_1(object sender, CancelEventArgs e)
        {
            try
            {
                if (AppProperties.Is_CloseMainApp == true)
                {
                    e.Cancel = false;
                }
                else
                {
                    e.Cancel = true;
                    //e.Cancel = true;
                }

            }
            catch (Exception ex)
            {
                WPFMessageBoxResult _Result = WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
                // throw;
            }

            // e.Cancel = true;



        }

        public void LoadButtonImages()
        {
            try
            {
                if (AppProperties.Selected_Resource == "English")
                {
                    /*
                      <Image x:Name="imageRTAInsp" Grid.Row="1" Grid.Column="1" Source="Images\Inspectors\User.png" Width="60" />

                                <Image x:Name="imgBtnHome" Grid.Row="3" Grid.Column="1" Source="Images\Buttons\Menue\Home Up.png" Width="60" MouseLeftButtonUp="MenuItem_Click_2" MouseLeftButtonDown="imgBtnHome_MouseLeftButtonDown_1"/>
                                        
                               
                                    <Image x:Name="imgBtnLanguage" Grid.Row="5" Grid.Column="1" Source="Images\Buttons\Menue\English Up.png" Width="60" MouseLeftButtonDown="imgBtnLanguage_MouseLeftButtonDown_1" MouseLeftButtonUp="MenuItem_Click_3"/>
                                <Image x:Name="imgBtnLogout" Grid.Row="7" Grid.Column="1"  Source="Images\Buttons\Menue\Logout Up.png" Width="60" MouseLeftButtonDown="imgBtnLogout_MouseLeftButtonDown_1" MouseLeftButtonUp="MenuItem_Click_5"/>

                                <Image x:Name="imagBtnSync" Grid.Row="9" Grid.Column="1" Source="Images\Buttons\Menue\Sync Up.png" Width="60" MouseLeftButtonDown="imagBtnSync_MouseLeftButtonDown_1" MouseLeftButtonUp="MenuItem_Click_4"/>
                                <Image x:Name="imgBtnExit" Grid.Row="11" Grid.Column="1" Source="Images\Buttons\Menue\Exit Up.png" Width="60" MouseLeftButtonDown="imgBtnExit_MouseLeftButtonDown_1" MouseLeftButtonUp="MenuItem_Click_1"/>

                                <Image x:Name="imgSerchVehcleProfile" Grid.Row="13" Grid.Column="1" Source="Images\Buttons\Menue\Vehicle Up.png" Width="60" MouseLeftButtonDown="imgSerchVehcleProfile_MouseLeftButtonDown_1" MouseLeftButtonUp="MenuItem_Click_6" VerticalAlignment="Bottom" />
                                <Image x:Name="imgSerchVioHist" Grid.Row="15" Grid.Column="1" Source="Images\Buttons\Menue\Violation Up.png" Width="60" VerticalAlignment="Bottom" MouseLeftButtonDown="imgSerchVioHist_MouseLeftButtonDown_1" MouseLeftButtonUp="MenuItem_Click_7" />
                                <Image x:Name="imgSerchOpertor" Grid.Row="17" Grid.Column="1" Source="Images\Buttons\Menue\Operator Up.png" Width="60" VerticalAlignment="Bottom" MouseLeftButtonDown="imgSerchOpertor_MouseLeftButtonDown_1" MouseLeftButtonUp="MenuItem_Click_8" />
                                <Image x:Name="imgVsdLogo" Grid.Row="19" Grid.ColumnSpan="3" Grid.Column="0" Source="Images\VSD Logo.png" Width="65" VerticalAlignment="Bottom" />
                    */

                    imageRTAInsp.Source = new BitmapImage(new Uri(@"/Images/Inspectors/User.png", UriKind.Relative));
                    imgBtnHome.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Home Up.png", UriKind.Relative));

                    imgBtnLanguage.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Arabic Up.png", UriKind.Relative));

                    imgBtnLogout.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Logout Up.png", UriKind.Relative));
                    imagBtnSync.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Sync Up.png", UriKind.Relative));
                    imgBtnExit.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Exit Up.png", UriKind.Relative));
                    //imgBtnNetworkConnectivity.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Network_Disconnet.png", UriKind.Relative));
                    imgSerchVehcleProfile.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Vehicle Up.png", UriKind.Relative));
                    imgSerchVioHist.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Violation Up.png", UriKind.Relative));
                    imgSerchOpertor.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Operator Up.png", UriKind.Relative));
                    imgInspection.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Inspection English Up.png", UriKind.Relative));
                    imgVsdTrafficFines.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/KB Up.png", UriKind.Relative));


                }
                else
                {
                    imageRTAInsp.Source = new BitmapImage(new Uri(@"/Images/Inspectors/User Arabic.png", UriKind.Relative));
                    imgBtnHome.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Home Arabic Up.png", UriKind.Relative));
                    imgBtnLanguage.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/English Up.png", UriKind.Relative));
                    imgBtnLogout.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Logout Arabic Up.png", UriKind.Relative));
                    imagBtnSync.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Sync Arabic Up.png", UriKind.Relative));
                    imgBtnExit.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Exit Arabic Up.png", UriKind.Relative));
                    // imgBtnNetworkConnectivity.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Network_Disconnet.png", UriKind.Relative));
                    imgSerchVehcleProfile.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Vehicle Arabic Up.png", UriKind.Relative));
                    imgSerchVioHist.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Violation Arabic Up.png", UriKind.Relative));
                    imgSerchOpertor.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Operator Arabic Up.png", UriKind.Relative));
                    imgInspection.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Inspection Arabic Up.png", UriKind.Relative));
                    imgVsdTrafficFines.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/KB Arabic Up.png", UriKind.Relative));




                }
            }
            catch (Exception ex)
            {
                WPFMessageBoxResult _Result = WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
                // throw;
            }
        }

        private void imgBtnHome_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            if (AppProperties.Selected_Resource == "English")
            {
                imgBtnHome.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Home Down.png", UriKind.Relative));
            }
            else
            {
                imgBtnHome.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Home Arabic Down.png", UriKind.Relative));
            }

        }

        private void imgBtnLanguage_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            if (AppProperties.Selected_Resource == "English")
            {

                imgBtnLanguage.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Arabic Down.png", UriKind.Relative));
            }
            else
            {
                imgBtnLanguage.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/English Down.png", UriKind.Relative));
            }

        }

        private void imagBtnSync_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            if (AppProperties.Selected_Resource == "English")
            {
                imagBtnSync.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Sync Down.png", UriKind.Relative));
            }
            else
            {
                imagBtnSync.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Sync Arabic Down.png", UriKind.Relative));
            }

        }

        private void imgBtnLogout_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            if (AppProperties.Selected_Resource == "English")
            {
                imgBtnLogout.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Logout Down.png", UriKind.Relative));
            }
            else
            {
                imgBtnLogout.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Logout Arabic Down.png", UriKind.Relative));
            }

        }

        private void imgBtnExit_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            if (AppProperties.Selected_Resource == "English")
            {
                imgBtnExit.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Exit Down.png", UriKind.Relative));
            }
            else
            {
                imgBtnExit.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Exit Arabic Down.png", UriKind.Relative));
            }

        }

        private void imgSerchVehcleProfile_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            if (AppProperties.Selected_Resource == "English")
            {
                imgSerchVehcleProfile.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Vehicle Down.png", UriKind.Relative));
            }
            else
            {
                imgSerchVehcleProfile.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Vehicle Arabic Down.png", UriKind.Relative));
            }

        }

        private void imgSerchVioHist_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            if (AppProperties.Selected_Resource == "English")
            {
                imgSerchVioHist.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Violation Down.png", UriKind.Relative));
            }
            else
            {
                imgSerchVioHist.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Violation Arabic Down.png", UriKind.Relative));
            }

        }

        private void imgSerchOpertor_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            //if (AppProperties.Selected_Resource == "English")
            //{
            //    imgSerchOpertor.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Operator Down.png", UriKind.Relative));
            //}
            //else
            //{
            //    imgSerchOpertor.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Operator Arabic Down.png", UriKind.Relative));
            //}

        }

        private void imgVsdTrafficFines_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {


            //if (AppProperties.Selected_Resource == "English")
            //{
            //    imgVsdTrafficFines.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/KB Down.png", UriKind.Relative));
            //}
            //else
            //{
            //    imgVsdTrafficFines.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/KB Arabic Down.png", UriKind.Relative));
            //}


        }

        private void Window_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {
            if (new CommonUtils().GetScreenOrientation() == AppProperties.LandScapetMode)
            {
                this.txtRightsReserve.Text = "Copyright © 2016 Roads and Transport Authority, All Rights Reserved.This Application is maintained by the Roads and Transport Authority";
            }
            else
            {
                this.txtRightsReserve.Text = "Copyright © 2016 Roads and Transport Authority";
            }
            setUcInspectorsSummerySettings();
            this.UpdateLayout();
        }

        private void imgVsdTrafficFines_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {

        }

        #region AutoLogoff Implementation
        private void ReadConfigInfo()
        {
            try
            {
                logOffTime = Convert.ToInt16(Properties.Settings.Default.AppAutoLogOutTime);
                string userName = string.Empty;
                // System.Configuration.ConfigurationManager.AppSettings["UserName"];
                //  tblLogoffTime.Text = "Logoff Time: " + strlogOffTime;
                tbuserName.Text = userName;
            }
            catch
            { }
        }

        private void InitializeAutoLogoffFeature()
        {
            HwndSource windowSpecificOSMessageListener = HwndSource.FromHwnd(new WindowInteropHelper(this).Handle);
            windowSpecificOSMessageListener.AddHook(new HwndSourceHook(CallBackMethod));
            AutoLogOffHelper.LogOffTime = logOffTime;
            AutoLogOffHelper.MakeAutoLogOffEvent += new AutoLogOffHelper.MakeAutoLogOff(AutoLogOffHelper_MakeAutoLogOffEvent);
            AutoLogOffHelper.StartAutoLogoffOption();
            string time = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt");
            // tblStatus.Text = "Timer is started at " + ": " + time;
        }
        private IntPtr CallBackMethod(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            //  Listening OS message to test whether it is a user activity
            if ((msg >= 0x0200 && msg <= 0x020A) || (msg <= 0x0106 && msg >= 0x00A0) || msg == 0x0021)
            {
                AutoLogOffHelper.ResetLogoffTimer();
                string time = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt");
                // tblStatus.Text = "Timer is reset on user activity at " + ": " + time;
                // tblLastUserActivityTypeOSMessage.Text = "Last user activity type OS message the application considers " + ": 0x" + msg.ToString("X");
            }
            else
            {
                //  tblLastOSMessage.Text = "Last OS Message " + ":  0x" + msg.ToString("X");
                // For debugging purpose
                // If this auto logoff does not work for some user activity, you can detect the integer code of that activity  using the following line.
                //Then All you need to do is adding this integer code to the above if condition.
                System.Diagnostics.Debug.WriteLine(msg.ToString());
            }
            return IntPtr.Zero;
        }

        void AutoLogOffHelper_MakeAutoLogOffEvent()
        {
            Logoff();
        }

        private void Logoff()
        {
            this.MainContentControl.Content = null;
            this.MainContentControl.Content = new ucLoginEn(this);
            this.UpdateLayout();
        }
        void statusTimer_Tick(object sender, EventArgs e)
        {
            this.systemDate.Text = DateTime.Now.ToString("hh:mm:ss tt");

        }
        #endregion

        private void imgInspection_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {

        }


        private void imgInspection_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            //if (AppProperties.Selected_Resource == "English")
            //{
            //    imgInspection.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Inspection English Down.png", UriKind.Relative));
            //}
            //else
            //{
            //    imgInspection.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Inspection Arabic Down.png", UriKind.Relative));
            //}
        }

        private void Window_SizeChanged_2(object sender, SizeChangedEventArgs e)
        {

        }

        private void imgBtnIntellegentTarget_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //this.MainContentControl.Content = null;
            //this.MainContentControl.Content =new ucIntelligentTarget(this);

        }

        private void imgVsdPortal_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.MainContentControl.Content = null;
            this.MainContentControl.Content = new ucVSDPortalWPF(this);
        }

        private void menueIntellegentTargeting_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.MainContentControl.Content = null;
                this.MainContentControl.Content = new ucIntelligentTarget(this);
            }
            catch (Exception ex)
            {
                WPFMessageBoxResult _Result = WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void menuInspection_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (AppProperties.Selected_Resource == "English")
                {
                    imgInspection.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Inspection English Up.png", UriKind.Relative));
                }
                else
                {
                    imgInspection.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Inspection Arabic Up.png", UriKind.Relative));
                }
                //------added by Kashif abbasi on dated 22-Nov-2015 ----------------
                string strScreenName = this.MainContentControl.Content.ToString().Substring(this.MainContentControl.Content.ToString().LastIndexOf(".") + 1);
                bool result = CommonUtils.deleteImgDirectory(strScreenName);//delete the defect image diractory if created made img and thn not submitted violation
                //-------------------------------------------------------------------
                if (result)
                {
                    this.MainContentControl.Content = null;
                    this.MainContentControl.Content = new ucLocationSelectionEn(this);
                }
            }
            catch (Exception ex)
            {
                WPFMessageBoxResult _Result = WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void meneViolationSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (AppProperties.Selected_Resource == "English")
                {
                    imgSerchVioHist.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Violation Up.png", UriKind.Relative));
                }
                else
                {
                    imgSerchVioHist.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Violation Arabic Up.png", UriKind.Relative));
                }
                //------added by Kashif abbasi on dated 22-Nov-2015 ----------------
                string strScreenName = this.MainContentControl.Content.ToString().Substring(this.MainContentControl.Content.ToString().LastIndexOf(".") + 1);
                bool result = CommonUtils.deleteImgDirectory(strScreenName);//delete the defect image diractory if created made img and thn not submitted violation
                //-------------------------------------------------------------------
                if (result)
                {
                    this.MainContentControl.Content = null;
                    this.MainContentControl.Content = new ucSearchViolationListing(this);
                }
            }
            catch (Exception ex)
            {
                WPFMessageBoxResult _Result = WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void menueVehicleSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (AppProperties.Selected_Resource == "English")
                {
                    imgSerchVehcleProfile.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Vehicle Up.png", UriKind.Relative));
                }
                else
                {
                    imgSerchVehcleProfile.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Vehicle Arabic Up.png", UriKind.Relative));
                }
                //------added by Kashif abbasi on dated 22-Nov-2015 ----------------
                string strScreenName = this.MainContentControl.Content.ToString().Substring(this.MainContentControl.Content.ToString().LastIndexOf(".") + 1);
                bool result = CommonUtils.deleteImgDirectory(strScreenName);//delete the defect image diractory if created made img and thn not submitted violation
                //-------------------------------------------------------------------
                if (result)
                {
                    this.MainContentControl.Content = null;
                    this.MainContentControl.Content = new ucSearchVehicle(this);
                }
            }
            catch (Exception ex)
            {
                WPFMessageBoxResult _Result = WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void menurOperatorSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (AppProperties.Selected_Resource == "English")
                {
                    imgSerchOpertor.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Operator Up.png", UriKind.Relative));
                }
                else
                {
                    imgSerchOpertor.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Operator Arabic Up.png", UriKind.Relative));
                }
                //------added by Kashif abbasi on dated 22-Nov-2015 ----------------
                string strScreenName = this.MainContentControl.Content.ToString().Substring(this.MainContentControl.Content.ToString().LastIndexOf(".") + 1);
                bool result = CommonUtils.deleteImgDirectory(strScreenName);//delete the defect image diractory if created made img and thn not submitted violation
                //-------------------------------------------------------------------
                if (result)
                {
                    this.MainContentControl.Content = null;
                    this.MainContentControl.Content = new ucSearchOperatorProfile(this);
                }
            }
            catch (Exception ex)
            {
                WPFMessageBoxResult _Result = WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void menueKnowledge_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (AppProperties.Selected_Resource == "English")
                {
                    imgVsdTrafficFines.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/KB Up.png", UriKind.Relative));
                }
                else
                {
                    imgVsdTrafficFines.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/KB Arabic Up.png", UriKind.Relative));
                }
                //------added by Kashif abbasi on dated 22-Nov-2015 ----------------
                string strScreenName = this.MainContentControl.Content.ToString().Substring(this.MainContentControl.Content.ToString().LastIndexOf(".") + 1);
                bool result = CommonUtils.deleteImgDirectory(strScreenName);//delete the defect image diractory if created made img and thn not submitted violation
                //-------------------------------------------------------------------
                if (result)
                {
                    this.MainContentControl.Content = null;
                    this.MainContentControl.Content = new ucTrafficFines(this);
                }
            }
            catch (Exception ex)
            {
                WPFMessageBoxResult _Result = WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void menueSync_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (AppProperties.Selected_Resource == "English")
                {
                    imagBtnSync.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Sync Up.png", UriKind.Relative));
                }
                else
                {
                    imagBtnSync.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Sync Arabic Up.png", UriKind.Relative));
                }
                //------added by Kashif abbasi on dated 22-Nov-2015 ----------------
                string strScreenName = this.MainContentControl.Content.ToString().Substring(this.MainContentControl.Content.ToString().LastIndexOf(".") + 1);
                bool result = CommonUtils.deleteImgDirectory(strScreenName);//delete the defect image diractory if created made img and thn not submitted violation
                //-------------------------------------------------------------------
                if (result)
                {
                    //((IViolation)ViolationManager.GetInstance()).SubmitOfflineViolation();
                    ProgressDialogResult result_progress = ProgressDialog.ProgressDialog.Execute(this, new CommonUtils().GetStringValue("lblPleaseWait"), (bw, we) =>
                    {

                        ((IViolation)ViolationManager.GetInstance()).SubmitOfflineViolation();

                        // So this check in order to avoid default processing after the Cancel button has been pressed.
                        // This call will set the Cancelled flag on the result structure.
                        ProgressDialog.ProgressDialog.CheckForPendingCancellation(bw, we);

                    }, ProgressDialogSettings.WithSubLabelAndCancel);

                    if (result_progress == null || result_progress.Cancelled)
                        return;
                    else if (result_progress.OperationFailed)
                        return;


                    WPFMessageBoxResult _Result = WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), AppProperties.errorMessageFromBusiness);

                }
            }
            catch (Exception ex)
            {
                WPFMessageBoxResult _Result = WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void menueLogout_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (AppProperties.Selected_Resource == "English")
                {
                    imgBtnLogout.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Logout Up.png", UriKind.Relative));
                }
                else
                {
                    imgBtnLogout.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Logout Arabic Up.png", UriKind.Relative));
                }
                //------added by Kashif abbasi on dated 22-Nov-2015 ----------------
                string strScreenName = this.MainContentControl.Content.ToString().Substring(this.MainContentControl.Content.ToString().LastIndexOf(".") + 1);
                bool result = CommonUtils.deleteImgDirectory(strScreenName);//delete the defect image diractory if created made img and thn not submitted violation

                //-------------------------------------------------------------------
                if (result)
                {
                    // if (MessageBox.Show(lblWouldYouLike.Content.ToString(), lblAppLogout.Content.ToString(), MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    if (WPFMessageBox.Show(new CommonUtils().GetStringValue("Confirmation"), new CommonUtils().GetStringValue("lblAppLogout"), WPFMessageBoxButtons.YesNo, WPFMessageBoxImage.Question) == WPFMessageBoxResult.Yes)
                    {
                        synchInspectorInfo();
                        tmr = new System.Timers.Timer();
                        tmr.Elapsed += tmr_Elapsed;
                        tmr.Interval = 12000;
                        tmr.Enabled = true; //Вкючаем таймер.
                        //this.MainContentControl.Content = null;
                        //  this.MainContentControl.Content = new ucLoginEn(this);
                        //  this.MainContentControl.Content = new ucGoodByeScreen(this);
                        AppProperties.isUserLoggedIn = false;
                        // this.UpdateLayout();

                    }
                }
            }
            catch (Exception ex)
            {
                WPFMessageBoxResult _Result = WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void rdbtnEnglis_Checked(object sender, RoutedEventArgs e)
        {
            try
            {

                if (AppProperties.Selected_Resource == "English")
                {
                    imgBtnLanguage.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Arabic Up.png", UriKind.Relative));
                }
                else
                {

                    imgBtnLanguage.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/English Up.png", UriKind.Relative));
                }

                //------added by Kashif abbasi on dated 22-Nov-2015 ----------------
                string strScreenName = this.MainContentControl.Content.ToString().Substring(this.MainContentControl.Content.ToString().LastIndexOf(".") + 1);
                bool result = CommonUtils.deleteImgDirectory(strScreenName);//delete the defect image diractory if created made img and thn not submitted violation
                //-------------------------------------------------------------------
                if (result)
                {
                    if (AppProperties.isUserLoggedIn == true)
                    {

                        if (WPFMessageBox.Show(new CommonUtils().GetStringValue("Confirmation"), lblWouldYouLikeLang.Content.ToString(), WPFMessageBoxButtons.YesNo, WPFMessageBoxImage.Question) == WPFMessageBoxResult.Yes)
                        {
                            synchInspectorInfo();
                            tmr = new System.Timers.Timer();
                            tmr.Elapsed += tmr_Elapsed2;
                            tmr.Interval = 12000;
                            tmr.Enabled = true; //Вкючаем таймер.
                            //this.MainContentControl.Content = null;
                            //  this.MainContentControl.Content = new ucLoginEn(this);
                            // this.MainContentControl.Content = new ucGoodByeScreen(this);

                        }
                        else
                        {
                            rdbtnArabic.IsChecked = true;
                        }
                    }
                    else
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
                }

            }
            catch (Exception ex)
            {
                WPFMessageBoxResult _Result = WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }

        }

        private void rdbtnArabic_Checked(object sender, RoutedEventArgs e)
        {
            try
            {

                if (AppProperties.Selected_Resource == "English")
                {
                    imgBtnLanguage.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Arabic Up.png", UriKind.Relative));
                }
                else
                {

                    imgBtnLanguage.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/English Up.png", UriKind.Relative));
                }

                //------added by Kashif abbasi on dated 22-Nov-2015 ----------------
                string strScreenName = this.MainContentControl.Content.ToString().Substring(this.MainContentControl.Content.ToString().LastIndexOf(".") + 1);
                bool result = CommonUtils.deleteImgDirectory(strScreenName);//delete the defect image diractory if created made img and thn not submitted violation
                //-------------------------------------------------------------------
                if (result)
                {
                    if (AppProperties.isUserLoggedIn == true)
                    {

                        if (WPFMessageBox.Show(new CommonUtils().GetStringValue("Confirmation"), lblWouldYouLikeLang.Content.ToString(), WPFMessageBoxButtons.YesNo, WPFMessageBoxImage.Question) == WPFMessageBoxResult.Yes)
                        {
                            synchInspectorInfo();
                            tmr = new System.Timers.Timer();
                            tmr.Elapsed += tmr_Elapsed2;
                            tmr.Interval = 12000;
                            tmr.Enabled = true; //Вкючаем таймер.
                            //this.MainContentControl.Content = null;
                            //  this.MainContentControl.Content = new ucLoginEn(this);
                            //  this.MainContentControl.Content = new ucGoodByeScreen(this);

                        }
                        else
                        {
                            rdbtnEnglis.IsChecked = true;
                        }
                    }
                    else
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
                }

            }
            catch (Exception ex)
            {
                WPFMessageBoxResult _Result = WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }

        }

        private void mainMenu_Drop(object sender, DragEventArgs e)
        {
            mainMenu.Background = Brushes.Black;
        }

        private void imageRTAInsp_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            getAllInspectorData();
        }
        //added by Kashif abbasi on dated 03-Jan-2016
        private void setUcInspectorsSummerySettings()
        {
            if (new CommonUtils().GetScreenOrientation() == AppProperties.LandScapetMode)
            {
                ucInspectorSumeryDialog.stkPnlAllInspectors.Orientation = Orientation.Horizontal;
                ucInspectorSumeryDialog.stkPnlMainInspector.Orientation = Orientation.Vertical;
                ucInspectorSumeryDialog.stkPnlMainInspector.Margin = new Thickness(5, 25, 8, 0);
                ucInspectorSumeryDialog.mcChart.Width = 850;
                ucInspectorSumeryDialog.mcChart.Height = 350;
                ucInspectorSumeryDialog.mcChart.Margin = new Thickness(2, 25, 0, 2);

            }
            else
            {
                ucInspectorSumeryDialog.stkPnlAllInspectors.Orientation = Orientation.Vertical;
                ucInspectorSumeryDialog.stkPnlMainInspector.Orientation = Orientation.Horizontal;
                ucInspectorSumeryDialog.stkPnlMainInspector.Margin = new Thickness(15, 25, 20, 2);
                ucInspectorSumeryDialog.mcChart.Width = 600;
                ucInspectorSumeryDialog.mcChart.Height = 350;
                ucInspectorSumeryDialog.mcChart.Margin = new Thickness(2, 25, 0, 2);

            }
        }
        public void getAllInspectorData()
        {
            try
            {
                App.VSDLog.Info("\n Entered in MainWindow.getAllInspectorData() ");
                DataTable dtInspectors = new DataTable();
                // IVehicleProfile iVehicleProfile = ((IVehicleProfile)VehicleProfileManager.GetInstance());
                IInspectorsManager iInspectorsManager = ((IInspectorsManager)InspectorsManager.GetInstance());

                ProgressDialogResult result = ProgressDialog.ProgressDialog.Execute(this, new CommonUtils().GetStringValue("gettingInspectors"), (bw, we) =>
                {
                    App.VSDLog.Info(" \n MainWindow.getAllInspectorData(): Going to call InspactorManager.getInspactorSummery(inspactorID,toDate,fromDate)");
                    dtInspectors = iInspectorsManager.GetInspactorsSummery("", "", "");
                    ProgressDialog.ProgressDialog.CheckForPendingCancellation(bw, we);

                }, ProgressDialogSettings.WithSubLabelAndCancel);

                if (result.Cancelled)
                    return;
                else if (result.OperationFailed)
                    return;

                if (dtInspectors != null)
                {
                    if (dtInspectors.Rows.Count > 0)
                        ucInspectorSumeryDialog.ShowHandlerDialog("", dtInspectors);
                    else
                        WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), new CommonUtils().GetStringValue("Probleminfetchinginspectorsreport"), WPFMessageBoxImage.Information);
                }
                else
                {
                    WPFMessageBox.Show("Information", "Problem in fetching inspectors report. ", WPFMessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(" \n MainWindow.getAllInspectorData(): Exception :" + ex.Message + "\n" + ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), new CommonUtils().GetStringValue("lblProblemInFetchingInscInfo"), WPFMessageBoxImage.Error);
            }
        }

        //added by kashif abbasi on dated 02-Feb-2016
        public void synchInspectorInfo()
        {
            try
            {

                DataTable dtInspectors = new DataTable();
                // IVehicleProfile iVehicleProfile = ((IVehicleProfile)VehicleProfileManager.GetInstance());
                IInspectorsManager iInspectorsManager = ((IInspectorsManager)InspectorsManager.GetInstance());
                App.VSDLog.Info("\n ucGoodByScreen.synchInspectorInfo(): entered in function and start synching offlineViolation first");

                ProgressDialogResult result = ProgressDialog.ProgressDialog.Execute(this, new CommonUtils().GetStringValue("synchAndGetInspData"), (bw, we) =>
                {
                    //First synch all the data from device to backend
                    ((IViolation)ViolationManager.GetInstance()).SubmitOfflineViolation();
                    App.VSDLog.Info(" \n ucGoodByScreen.synchInspectorInfo(): SubmitOfflineViolation() successfully executed now Going to call InspactorManager.GetGoodByeInspactorSummery('',''," + Convert.ToString(AppProperties.empID) + ")");
                    dtInspectors = iInspectorsManager.GetGoodByeInspactorSummery("", "", Convert.ToString(AppProperties.empID.Trim()));
                    ProgressDialog.ProgressDialog.CheckForPendingCancellation(bw, we);

                }, ProgressDialogSettings.WithSubLabelAndCancel);

                if (result.Cancelled)
                    return;
                else if (result.OperationFailed)
                    return;
                if (this.MainContentControl.Content != null)
                    this.MainContentControl.Content = null;
                this.MainContentControl.Content = new ucGoodByeScreen(this, dtInspectors);
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(" \n MainWindow.getAllInspectorData(): Exception :"  + "\n" + ex.StackTrace);
                App.VSDLog.Info(ex.Message);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), new CommonUtils().GetStringValue("lblProblemInFetchingInscInfo"), WPFMessageBoxImage.Error);
            }
        }

        private void menueImpoundingScreen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.MainContentControl.Content = null;
                this.MainContentControl.Content = new ucVSDPortalWPF(this);
            }
            catch (Exception ex)
            {

                WPFMessageBox.Show("Exception", "Problem in Impounding Screen ", ex.StackTrace, WPFMessageBoxImage.Error);
            }
        }

        private void imageRTAInsp_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            System.Windows.Rect r = new System.Windows.Rect(e.NewSize);
            RectangleGeometry gm = new RectangleGeometry(r, 40, 40); // 40 is radius here
            ((UIElement)sender).Clip = gm; 


        }

        private void menueProfileManagment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.MainContentControl.Content = null;
                this.MainContentControl.Content = new ucProfileManagment(this);
            }
            catch (Exception ex)
            {

                App.VSDLog.Info(" \n MainWindow.getAllInspectorData(): Exception :" + ex.Message + "\n" + ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.StackTrace, WPFMessageBoxImage.Error);

            }
        }

        private void menuComercialActivity_Click(object sender, RoutedEventArgs e)
        {

        }

        private void menuRecordInspection_Click(object sender, RoutedEventArgs e)
        {
           //  this.MainContentControl.Content = null;
           //  this.MainContentControl.Content = new ucCarRentalAgencySearch(this);

        }

        private void menuInspection_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void menuRoadSideInspection_Click(object sender, RoutedEventArgs e)
        {
            AppProperties.Is_DeviceInspection = false;
            this.MainContentControl.Content = null;
            this.MainContentControl.Content = new ucLocationSelectionEn(this);
        }

        private void menuDeviceInspection_Click(object sender, RoutedEventArgs e)
        {

            AppProperties.Is_DeviceInspection = true;
            this.MainContentControl.Content = null;
            this.MainContentControl.Content = new ucLocationSelectionEn(this);
        }

        private void menueAlprSelection_Click(object sender, RoutedEventArgs e)
        {
           // ucAlprSelectionOption.ShowHandlerDialog(this);
        }

    }



    internal static class WindowExtensions
    {
        // from winuser.h
        private const int GWL_STYLE = -16,
                          WS_MAXIMIZEBOX = 0x10000,
                          WS_MINIMIZEBOX = 0x20000;

        [DllImport("user32.dll")]
        extern private static int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        extern private static int SetWindowLong(IntPtr hwnd, int index, int value);

        internal static void HideMinimizeAndMaximizeButtons(this Window window)
        {
            IntPtr hwnd = new System.Windows.Interop.WindowInteropHelper(window).Handle;
            var currentStyle = GetWindowLong(hwnd, GWL_STYLE);

            SetWindowLong(hwnd, GWL_STYLE, (currentStyle & ~WS_MAXIMIZEBOX & ~WS_MINIMIZEBOX));
        }
    }


}
