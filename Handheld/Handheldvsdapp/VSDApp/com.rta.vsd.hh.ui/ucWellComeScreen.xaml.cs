using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlServerCe;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using VSDApp.com.rta.vsd.hh.db;
using VSDApp.com.rta.vsd.hh.manager;
using VSDApp.com.rta.vsd.hh.utilities;
using VSDApp.ProgressDialog;
using VSDApp.WPFMessageBoxControl;

namespace VSDApp.com.rta.vsd.hh.ui
{
    /// <summary>
    /// Interaction logic for ucWellComeScreen.xaml
    /// </summary>
    public partial class ucWellComeScreen : UserControl
    {
        MainWindow m_MainWindow = null;
        public int logOffTime = 1;
        CloseAdminPermission closeadminWindow = null;
        System.Timers.Timer timer = new System.Timers.Timer();
        public delegate void ShowGoodByeScreen_Delegate();

        public ucWellComeScreen(MainWindow mainWnd)
        {
            InitializeComponent();
            m_MainWindow = mainWnd;
            //  timer_welcomescreen = tmr;
        }

        private void imagebtnNext_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
            if (AppProperties.Selected_Resource == "English")
            {
                imagebtnNext.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Next.png", UriKind.Relative));
            }
            else
            {
                imagebtnNext.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Next Arabic Up.png", UriKind.Relative));
            }
            this.m_MainWindow.MainContentControl.Content = new ucLocationSelectionEn(m_MainWindow);
        }

        private void imagebtnNext_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            if (AppProperties.Selected_Resource == "English")
            {
                imagebtnNext.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Next Down.png", UriKind.Relative));
            }
            else
            {
                imagebtnNext.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Next Arabic Down.png", UriKind.Relative));
            }
            this.m_MainWindow.MainContentControl.Content = new ucLocationSelectionEn(m_MainWindow);
        }

        public void ShowInspectorInformationOnMenueBar()
        {
            try
            {
                Hashtable hashMap = CommonUtils.LoadInspectorsUserNameTable();
                Hashtable hashMap_Ar = CommonUtils.LoadInspectorsUserNameArTable();
                String emp_full_name_En = "";
                String emp_full_name_Ar = "";
                foreach (DictionaryEntry entry in hashMap)
                {
                    if (entry.Key.ToString() == AppProperties.empUserName.ToString().Trim().ToLower())
                    {
                        AppProperties.empUserFullName = entry.Value.ToString();
                    }
                }
                foreach (DictionaryEntry entry in hashMap_Ar)
                {
                    if (entry.Key.ToString() == AppProperties.empUserName.ToString().Trim().ToLower())
                    {
                        AppProperties.empUserFullNameAr = entry.Value.ToString();
                    }
                }

                if (AppProperties.isOnline == true)
                {

                    //  m_MainWindow.tbuserName.Text = AppProperties.empUserName;


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
                    inspector_pic_path = inspector_pic_path + AppProperties.empUserName.ToString().Trim().ToLower() + "_Arabic.png";
                    m_MainWindow.lblInspectorName.Content = AppProperties.empUserFullNameAr;
                    m_MainWindow.lblInspectorName.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    inspector_pic_path = inspector_pic_path + AppProperties.empUserName.ToString().Trim().ToLower() + ".png";
                    m_MainWindow.lblInspectorName.Content = AppProperties.empUserFullName;
                    m_MainWindow.lblInspectorName.Visibility = System.Windows.Visibility.Visible;
                }

                //     new BitmapImage(new Uri(@"/Images/Buttons/Small/Start Inspection Down.png", UriKind.Relative));
                //  inspector_pic_path = @"/Images/Inspectors/hialbedwawi.png";
                // imageRTAInsp2.Source = new BitmapImage(new Uri(inspector_pic_path, UriKind.Relative));
                m_MainWindow.imageRTAInsp.Source = new BitmapImage(new Uri(inspector_pic_path, UriKind.Relative));


            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
              //  WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }
        public void ShowInspectorInformation()
        {
            try
            {
                Hashtable hashMap = CommonUtils.LoadInspectorsUserNameTable();
                Hashtable hashMap_Ar = CommonUtils.LoadInspectorsUserNameArTable();
                String emp_full_name_En = "";
                String emp_full_name_Ar = "";
                foreach (DictionaryEntry entry in hashMap)
                {
                    if (entry.Key.ToString() == AppProperties.empUserName.ToString().Trim().ToLower())
                    {
                        AppProperties.empUserFullName = entry.Value.ToString();
                    }
                }
                foreach (DictionaryEntry entry in hashMap_Ar)
                {
                    if (entry.Key.ToString() == AppProperties.empUserName.ToString().Trim().ToLower())
                    {
                        AppProperties.empUserFullNameAr = entry.Value.ToString();
                    }
                }

                if (AppProperties.isOnline == true)
                {

                    //  m_MainWindow.tbuserName.Text = AppProperties.empUserName;


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
                    inspector_pic_path = inspector_pic_path + AppProperties.empUserName.ToString().Trim().ToLower() + "_Arabic.png";
                    m_MainWindow.lblInspectorName.Content = AppProperties.empUserFullNameAr;
                    m_MainWindow.lblInspectorName.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    inspector_pic_path = inspector_pic_path + AppProperties.empUserName.ToString().Trim().ToLower() + ".png";
                    m_MainWindow.lblInspectorName.Content = AppProperties.empUserFullName;
                    m_MainWindow.lblInspectorName.Visibility = System.Windows.Visibility.Visible;
                }

                //     new BitmapImage(new Uri(@"/Images/Buttons/Small/Start Inspection Down.png", UriKind.Relative));
                //  inspector_pic_path = @"/Images/Inspectors/hialbedwawi.png";
                imageRTAInsp2.Source = new BitmapImage(new Uri(inspector_pic_path, UriKind.Relative));
                m_MainWindow.imageRTAInsp.Source = new BitmapImage(new Uri(inspector_pic_path, UriKind.Relative));



            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
              //  WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        public void PopulateInspectorDetails()
        {
            try
            {
                if (AppProperties.Selected_Resource == "English")
                {
                    m_MainWindow.lblInspectorName.Content = AppProperties.LoggedInUser.FirstName +" "+AppProperties.LoggedInUser.LastName;
                    m_MainWindow.lblInspectorDesigination.Content = AppProperties.LoggedInUser.Desigination;
                    m_MainWindow.lblInspectorPhone.Content = AppProperties.LoggedInUser.MobNumber;
                    m_MainWindow.lblInspectorEmail.Content = AppProperties.LoggedInUser.Email;



                 //   string inspector_pic_path = AppProperties.InspectorImagesPath + "\\" + AppProperties.LoggedInUser.UserName.ToLower() + ".png";

                 //   BitmapImage image = new BitmapImage(new Uri(inspector_pic_path));

                 //   m_MainWindow.imageRTAInsp.Source = image;
                    

                }
                else
                {
                    m_MainWindow.lblInspectorName.Content = AppProperties.LoggedInUser.FistNameAr + " " + AppProperties.LoggedInUser.LastNameAr;
                    m_MainWindow.lblInspectorDesigination.Content = AppProperties.LoggedInUser.DesiginationAr;
                    m_MainWindow.lblInspectorPhone.Content = AppProperties.LoggedInUser.MobNumber;
                    m_MainWindow.lblInspectorEmail.Content = AppProperties.LoggedInUser.Email;
                  //  string inspector_pic_path = AppProperties.InspectorImagesPath + "\\" + AppProperties.LoggedInUser.UserName.ToLower() + ".png";

                  //  BitmapImage image = new BitmapImage(new Uri(inspector_pic_path));

                 //   m_MainWindow.imageRTAInsp.Source = image;


                }
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
            }
        }


        public void ShowLastUpdateTime_IntrestedVehicle()
        {
            try
            {
                if (AppProperties.Is_NewLogin == false)
                    return;
                DateTime updateTime_veh = this.GetUpdatedTimeStampofVehicleList();
                NotificationWindow notification;

                if (updateTime_veh != Convert.ToDateTime("01/01/1900 00:00:00"))
                {
                    String notification_message = new CommonUtils().GetStringValue("lblIntrestedVehicleUpdatedTime") + "   :" + updateTime_veh.ToString();

                    // WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("lblIntrestedVehicleUpdatedTime") + updateTime_veh);

                    DateTime time_48_p = DateTime.Now.Subtract(new TimeSpan(48, 0, 0));
                    int campare_val = updateTime_veh.CompareTo(time_48_p);
                    if (campare_val == 0 || campare_val < 1)
                    {
                        //Delate Intrested List
                        // ClearListofVehicleofIntrestTable
                    }
                    notification = new NotificationWindow(notification_message);
                    // NotificationWindow notification = new NotificationWindow("Test");
                    notification.ShowDialog();



                }
                else
                {
                    String notification_message = new CommonUtils().GetStringValue("lblIntrestedVehicleNotFound");
                    notification = new NotificationWindow(notification_message);
                    // NotificationWindow notification = new NotificationWindow("Test");
                    notification.ShowDialog();
                }
                // AppProperties.Is_NewLogin = false;
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }
        public void ShowLastUpdatedTime_IntrestedDriver()
        {
            try
            {

            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        public void ClearListofVehicleofIntrestTable()
        {
            try
            {
                App.VSDLog.Info("Entering in ClearListofVehicleofIntrestTable");
                SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
                string sqlQuery;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCeCommand command;
                SqlCeResultSet rs;
                sqlQuery = "DELETE  FROM VSD_Interest_List_Vehicle";
                App.VSDLog.Info(sqlQuery);
                command = new SqlCeCommand(sqlQuery, con);
                rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);
                rs.Close();
                con.Close();
                App.VSDLog.Info("VSD_Interest_List_Vehicle Record deleted successfully");

            }
            catch (Exception ex)
            {
                App.VSDLog.Info("Exception in deleting VSD_Interest_List_Vehicle records" + ex.Message);
            }
        }

        public DateTime GetUpdatedTimeStampofVehicleList()
        {
            DateTime dtime = Convert.ToDateTime("01/01/1900 00:00:00");
            try
            {

                App.VSDLog.Info("Entering in GetUpdatedTimeStampofVehicleList ");
                SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
                string sqlQuery;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCeCommand command;
                SqlCeResultSet rs;
                sqlQuery = "SELECT   top(1)     CONVERT(DateTime, TimeStamp) AS TimeStamp FROM       VSD_Interest_List_Vehicle";
                command = new SqlCeCommand(sqlQuery, con);

                rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);

                int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;

                string dateTime = String.Empty;
                if (rs.Read())
                {
                    dtime = rs.GetDateTime(0);
                    // dtime = Convert.ToDateTime(dateTime,
                    //  dtime = DateTime.ParseExact(dateTime, "yyyy-MM-dd HH:mm:ss,fff", System.Globalization.CultureInfo.InvariantCulture);
                    return dtime;

                }

                rs.Close();
                con.Close();

                //   return dateTime;
                App.VSDLog.Info("Updated TimeStamp of Vehicle= " + dateTime);
                return dtime;
            }
            catch (Exception ex)
            {

                App.VSDLog.Info("Exception in ucWelcomScrren GetUpdatedTimeStampofVehicleList()" + ex.Message);
                return dtime;
            }
        }

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (AppProperties.Selected_Resource == "English")
                {
                    imagebtnNext.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Next.png", UriKind.Relative));
                }
                else
                {
                    imagebtnNext.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Next Arabic Up.png", UriKind.Relative));
                }
                m_MainWindow.txtlocation.Visibility = System.Windows.Visibility.Collapsed;
                AppProperties.Is_DeviceInspection = false;
                ShowInspectorInformation();
                ShowInspectorInformationOnMenueBar();
                PopulateInspectorDetails();
                EnableSearchOption(true);
                LoadButtonImages();
                //  _someLabel.Text = "Whatever";

                ShowLastUpdateTime_IntrestedVehicle();
             //   ucComercialActivityInspection.SetParent(grdMain);

                if (AppProperties.Is_NewLogin)
                {
                    if (ConfigurationManager.AppSettings["IsGeoLocationEnabled"].ToLower()=="true")
                    {
                    //uncommented for  geo location
                    VSDApp.com.rta.vsd.hh.gps.GPSLocation gpsLoc = new VSDApp.com.rta.vsd.hh.gps.GPSLocation(m_MainWindow);
                    gpsLoc.SaveHandHeldCurrentGeoCoardinates();
                    //  m_MainWindow.ucAlprSelectionOption.ShowHandlerDialog(m_MainWindow);
                   // m_MainWindow.getAllInspectorData();
                    }
                }


                ucInspectionTypeOption.SetParent(grdMain2);

                AppProperties.Is_NewLogin = false;


            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        public void EnableSearchOption(bool enab)
        {
            m_MainWindow.mainBanner.Visibility = System.Windows.Visibility.Visible;
            /*
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
            }*/

        }

        public void LoadButtonImages()
        {
            try
            {
                if (AppProperties.Selected_Resource == "English")
                {

                    // imageRTAInsp.Source = new BitmapImage(new Uri(@"/Images/Inspectors/User.png", UriKind.Relative));
                    /*
                    imgBtnInspect.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Inspection English Up.png", UriKind.Relative));

                    imgBtnLanguage.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Arabic Up.png", UriKind.Relative));

                    imgBtnLogout.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Logout Up.png", UriKind.Relative));
                    imagBtnSync.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Sync Up.png", UriKind.Relative));
                    imgBtnExit.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Exit Up.png", UriKind.Relative));
                    imgSerchVehcleProfile.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Vehicle Up.png", UriKind.Relative));
                    imgSerchVioHist.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Violation Up.png", UriKind.Relative));
                    imgSerchOpertor.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Operator Up.png", UriKind.Relative));
                    imgVsdTrafficFines.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/KB Up.png", UriKind.Relative));*/

                    imagIntellegentTargeting.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/intelligent_targeting.jpg", UriKind.Relative));
                    imagInspection.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/inspection.jpg", UriKind.Relative));
                    imageViolation.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/violation.jpg", UriKind.Relative));
                    imageVehicleSearch.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/vehicle_search.jpg", UriKind.Relative));
                    imageOperatorSearch.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/operator_search.jpg", UriKind.Relative));
                    imageKnowledge.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/knowledge.jpg", UriKind.Relative));
                    imageKnowledge.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/knowledge.jpg", UriKind.Relative));
                    imageSynch.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/sync.jpg", UriKind.Relative));
                    imageMainMenu.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/my_profile.jpg", UriKind.Relative));
                    imageLogout.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/logout.jpg", UriKind.Relative));


                }
                else
                {
                    imagIntellegentTargeting.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/intelligent_targeting_a.jpg", UriKind.Relative));
                    imagInspection.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/inspection_a.jpg", UriKind.Relative));
                    imageViolation.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/violation_a.jpg", UriKind.Relative));
                    imageVehicleSearch.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/vehicle_search_a.jpg", UriKind.Relative));
                    imageOperatorSearch.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/operator_search_a.jpg", UriKind.Relative));
                    imageKnowledge.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/knowledge_a.jpg", UriKind.Relative));
                    imageKnowledge.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/knowledge_a.jpg", UriKind.Relative));
                    imageSynch.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/sync_a.jpg", UriKind.Relative));
                    imageMainMenu.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/my_profile_a.jpg", UriKind.Relative));
                    imageLogout.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/logout_a.jpg", UriKind.Relative));
                    //  imageRTAInsp.Source = new BitmapImage(new Uri(@"/Images/Inspectors/User Arabic.png", UriKind.Relative));
                    /*
                    imgBtnInspect.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Inspection Arabic Up.png", UriKind.Relative));
                    imgBtnLanguage.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/English Up.png", UriKind.Relative));
                    imgBtnLogout.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Logout Arabic Up.png", UriKind.Relative));
                    imagBtnSync.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Sync Arabic Up.png", UriKind.Relative));
                    imgBtnExit.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Exit Arabic Up.png", UriKind.Relative));
                    imgSerchVehcleProfile.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Vehicle Arabic Up.png", UriKind.Relative));
                    imgSerchVioHist.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Violation Arabic Up.png", UriKind.Relative));
                    imgSerchOpertor.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Operator Arabic Up.png", UriKind.Relative));
                    imgVsdTrafficFines.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/KB Arabic Up.png", UriKind.Relative));*/


                }
            }
            catch (Exception ex)
            {
                WPFMessageBoxResult _Result = WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
                // throw;
            }
        }

        #region Events

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {


            if (AppProperties.Selected_Resource == "English")
            {
                imgBtnExit.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Exit Up.png", UriKind.Relative));
            }
            else
            {
                imgBtnExit.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Exit Arabic Up.png", UriKind.Relative));
            }

            if (closeadminWindow == null)
            {
                closeadminWindow = new CloseAdminPermission(m_MainWindow);
                closeadminWindow.Closing += closeadminWindow_Closing;
                closeadminWindow.Owner = m_MainWindow;
                closeadminWindow.Show();
            }
            /*
            WPFMessageBoxResult __Result = WPFMessageBox.Show(new CommonUtils().GetStringValue("Confirmation"), new CommonUtils().GetStringValue("lblCloseApplicationMessage"), WPFMessageBoxButtons.YesNo, WPFMessageBoxImage.Question);

           // if (MessageBox.Show(lblCloseApplicationMessage.Content.ToString() + "?", lblCloseApplication.Content.ToString(), MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            if(__Result == WPFMessageBoxResult.Yes)
            {
                this.Close();
            }*/
        }

        void closeadminWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (AppProperties.Is_ClosePermissionWindow == true)
                {
                    m_MainWindow.Close();
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
                imgBtnInspect.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Inspection English Up.png", UriKind.Relative));
            }
            else
            {
                imgBtnInspect.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Inspection Arabic Up.png", UriKind.Relative));
            }

            m_MainWindow.MainContentControl.Content = null;
            // m_MainWindow.MainContentControl.Content = new ucLocationSelectionEn(m_MainWindow);
            m_MainWindow.MainContentControl.Content = new ucLocationSelectionEn(m_MainWindow);


        }
        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
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

                //  ((IViolation)ViolationManager.GetInstance()).SubmitOfflineViolation();

                ProgressDialogResult result = ProgressDialog.ProgressDialog.Execute(this.m_MainWindow, new CommonUtils().GetStringValue("lblPleaseWait"), (bw, we) =>
                {

                    ((IViolation)ViolationManager.GetInstance()).SubmitOfflineViolation();

                    // So this check in order to avoid default processing after the Cancel button has been pressed.
                    // This call will set the Cancelled flag on the result structure.
                    ProgressDialog.ProgressDialog.CheckForPendingCancellation(bw, we);

                }, ProgressDialogSettings.WithSubLabelAndCancel);

                if (result == null || result.Cancelled)
                    return;
                else if (result.OperationFailed)
                    return;


                WPFMessageBoxResult _Result = WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), AppProperties.errorMessageFromBusiness);

            }
            catch (Exception ex)
            {
                WPFMessageBoxResult _Result = WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }

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

                if (AppProperties.isUserLoggedIn == true)
                {

                    if (WPFMessageBox.Show(new CommonUtils().GetStringValue("Confirmation"), new CommonUtils().GetStringValue("lblWouldYouLikeLang"), WPFMessageBoxButtons.YesNo, WPFMessageBoxImage.Question) == WPFMessageBoxResult.Yes)
                    {
                        m_MainWindow.synchInspectorInfo();
                        timer = new System.Timers.Timer();
                        timer.Elapsed += timer_Elapsed2;
                        timer.Interval = 10000;
                        timer.Enabled = true;
                        // m_MainWindow.MainContentControl.Content = null;
                        // m_MainWindow.MainContentControl.Content = new ucLoginEn(m_MainWindow);
                        //  m_MainWindow.MainContentControl.Content = new ucGoodByeScreen(m_MainWindow);
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
            catch (Exception ex)
            {
                WPFMessageBoxResult _Result = WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }

            // Properties.Resources.Culture = new CultureInfo("ar-AE");
            // Thread.CurrentThread.CurrentUICulture = new CultureInfo("ar-AE");
            // this.FlowDirection = System.Windows.FlowDirection.RightToLeft;
            //  this.UpdateLayout();
            //var culture = new CultureInfo("ar-AE");
            // Thread.CurrentThread.CurrentCulture = culture;
            // Thread.CurrentThread.CurrentUICulture = culture;
            //     FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
            //     this.UpdateLayout();

            // SetLogoLocation();
            //ResourceProvider.Refresh();
            //  ChangeCulture(new CultureInfo("ar-AE")); 
            //IEnumerable<Control> _controlList = FindVisualChildren<Control>(this);

            //foreach (Control c in _controlList)
            //{
            //    ComponentResourceManager resources = new ComponentResourceManager(typeof(Window));
            //    resources.ApplyResources(c, c.Name, new CultureInfo("ar-AE"));
            //}

        }

        private void MenuItem_Click_5(object sender, RoutedEventArgs e)
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
                // if (MessageBox.Show(lblWouldYouLike.Content.ToString(), lblAppLogout.Content.ToString(), MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                if (WPFMessageBox.Show(new CommonUtils().GetStringValue("Confirmation"), new CommonUtils().GetStringValue("lblAppLogout"), WPFMessageBoxButtons.YesNo, WPFMessageBoxImage.Question) == WPFMessageBoxResult.Yes)
                {
                    m_MainWindow.synchInspectorInfo();
                    timer = new System.Timers.Timer();
                    timer.Elapsed += timer_Elapsed;
                    timer.Interval = 8000;
                    timer.Enabled = true; //Вкючаем таймер.


                    AppProperties.isUserLoggedIn = false;
                    // this.UpdateLayout();

                }
            }
            catch (Exception ex)
            {
                WPFMessageBoxResult _Result = WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //throw new NotImplementedException();
            this.Dispatcher.Invoke(
                            new ShowGoodByeScreen_Delegate(this.SwitchToLoginScreen),
                            new object[] { }
                        );
            timer.Enabled = false;
            timer.Stop();
            timer.Dispose();
        }
        void timer_Elapsed2(object sender, System.Timers.ElapsedEventArgs e)
        {
            //throw new NotImplementedException();
            this.Dispatcher.Invoke(
                            new ShowGoodByeScreen_Delegate(this.SwitchLang),
                            new object[] { }
                        );
            timer.Enabled = false;
            timer.Stop();
            timer.Dispose();
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
            // m_MainWindow.MainContentControl.Content = null;
            //  m_MainWindow.MainContentControl.Content = new ucLoginEn(m_MainWindow);
            this.UpdateLayout();
        }

        private void MenuItem_Click_6(object sender, RoutedEventArgs e)
        {
            if (AppProperties.Selected_Resource == "English")
            {
                imgSerchVehcleProfile.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Vehicle Up.png", UriKind.Relative));
            }
            else
            {
                imgSerchVehcleProfile.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Vehicle Arabic Up.png", UriKind.Relative));
            }
            m_MainWindow.MainContentControl.Content = null;
            m_MainWindow.MainContentControl.Content = new ucSearchVehicle(m_MainWindow);

        }

        private void MenuItem_Click_7(object sender, RoutedEventArgs e)
        {
            if (AppProperties.Selected_Resource == "English")
            {
                imgSerchVioHist.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Violation Up.png", UriKind.Relative));
            }
            else
            {
                imgSerchVioHist.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Violation Arabic Up.png", UriKind.Relative));
            }

            m_MainWindow.MainContentControl.Content = null;
            m_MainWindow.MainContentControl.Content = new ucSearchViolationListing(m_MainWindow);



        }

        private void MenuItem_Click_8(object sender, RoutedEventArgs e)
        {
            if (AppProperties.Selected_Resource == "English")
            {
                imgSerchOpertor.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Operator Up.png", UriKind.Relative));
            }
            else
            {
                imgSerchOpertor.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Operator Arabic Up.png", UriKind.Relative));
            }
            m_MainWindow.MainContentControl.Content = null;
            m_MainWindow.MainContentControl.Content = new ucSearchOperatorProfile(m_MainWindow);



        }


        private void imgBtnHome_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {

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
            if (AppProperties.Selected_Resource == "English")
            {
                imgSerchOpertor.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Operator Down.png", UriKind.Relative));
            }
            else
            {
                imgSerchOpertor.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Operator Arabic Down.png", UriKind.Relative));
            }

        }

        private void imgVsdTrafficFines_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {


            if (AppProperties.Selected_Resource == "English")
            {
                imgVsdTrafficFines.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/KB Down.png", UriKind.Relative));
            }
            else
            {
                imgVsdTrafficFines.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/KB Arabic Down.png", UriKind.Relative));
            }


        }



        private void imgVsdTrafficFines_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
            if (AppProperties.Selected_Resource == "English")
            {
                imgVsdTrafficFines.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/KB Up.png", UriKind.Relative));
            }
            else
            {
                imgVsdTrafficFines.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/KB Arabic Up.png", UriKind.Relative));
            }
            m_MainWindow.MainContentControl.Content = null;
            m_MainWindow.MainContentControl.Content = new ucTrafficFines(m_MainWindow);


        }
        #endregion

        private void imgBtnInspect_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            if (AppProperties.Selected_Resource == "English")
            {
                imgBtnInspect.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Inspection English Down.png", UriKind.Relative));
            }
            else
            {
                imgBtnInspect.Source = new BitmapImage(new Uri(@"/Images/Buttons/Menue/Inspection Arabic Down.png", UriKind.Relative));
            }
        }

        private void imgBtnInspect_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {

            //   m_MainWindow.MainContentControl.Content = null;
            //  m_MainWindow.MainContentControl.Content = new ucLocationSelectionEn(m_MainWindow);
            ucInspectionTypeOption.ShowHandlerDialog("", m_MainWindow);
        }
        private void imageComercialActivities_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
           // ucComercialActivityInspection.ShowHandlerDialog(m_MainWindow);
        }

        private void UserControl_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {
            if (new CommonUtils().GetScreenOrientation() == AppProperties.LandScapetMode)
            {
                stckPanelMain.Orientation = Orientation.Horizontal;
                /*
                maingrid.Children.Remove(lblSelectOption);
                Grid.SetRow(lblSelectOption, 3);
                Grid.SetColumn(lblSelectOption, 5);
                maingrid.Children.Add(lblSelectOption);



                maingrid.Children.Remove(stackPanelButtons);
                Grid.SetRow(stackPanelButtons, 5);
                Grid.SetColumn(stackPanelButtons, 5);
                maingrid.Children.Add(stackPanelButtons); */
            }
            else
            {
                stckPanelMain.Orientation = Orientation.Vertical;
                /*
                maingrid.Children.Remove(lblSelectOption);
                Grid.SetRow(lblSelectOption, 7);
                Grid.SetColumn(lblSelectOption, 3);
                maingrid.Children.Add(lblSelectOption);

                maingrid.Children.Remove(stackPanelButtons);
                Grid.SetRow(stackPanelButtons, 9);
                Grid.SetColumn(stackPanelButtons, 3);
                maingrid.Children.Add(stackPanelButtons);*/
            }
        }

        private void UserControl_Initialized_1(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.DigitSubstitution = System.Globalization.DigitShapes.None;
            this.UpdateLayout();
        }

        private void imagIntellegentTargeting_MouseEnter(object sender, MouseEventArgs e)
        {
            if (AppProperties.Selected_Resource == "English")
            {
                imagIntellegentTargeting.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/intelligent_targeting_mo.jpg", UriKind.Relative));
            }
            else
            {
                imagIntellegentTargeting.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/intelligent_targeting_mo_a.jpg", UriKind.Relative));
            }
        }

        private void imagIntellegentTargeting_MouseLeave(object sender, MouseEventArgs e)
        {
            if (AppProperties.Selected_Resource == "English")
            {
                imagIntellegentTargeting.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/intelligent_targeting.jpg", UriKind.Relative));
            }
            else
            {
                imagIntellegentTargeting.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/intelligent_targeting_a.jpg", UriKind.Relative));
            }
        }


        private void imagInspection_MouseEnter(object sender, MouseEventArgs e)
        {
            if (AppProperties.Selected_Resource == "English")
            {
                imagInspection.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/inspection_mo.jpg", UriKind.Relative));
            }
            else
            {
                imagInspection.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/inspection_mo_a.jpg", UriKind.Relative));
            }
        }

        private void imagInspection_MouseLeave(object sender, MouseEventArgs e)
        {
            if (AppProperties.Selected_Resource == "English")
            {
                imagInspection.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/inspection.jpg", UriKind.Relative));
            }
            else
            {
                imagInspection.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/inspection_a.jpg", UriKind.Relative));
            }
        }


        private void imageViolation_MouseEnter(object sender, MouseEventArgs e)
        {
            if (AppProperties.Selected_Resource == "English")
            {
                imageViolation.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/violation_mo.jpg", UriKind.Relative));
            }
            else
            {
                imageViolation.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/violation_mo_a.jpg", UriKind.Relative));
            }
        }

        private void imageViolation_MouseLeave(object sender, MouseEventArgs e)
        {
            if (AppProperties.Selected_Resource == "English")
            {
                imageViolation.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/violation.jpg", UriKind.Relative));
            }
            else
            {
                imageViolation.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/violation_a.jpg", UriKind.Relative));
            }
        }


        private void imageVehicleSearch_MouseEnter(object sender, MouseEventArgs e)
        {
            if (AppProperties.Selected_Resource == "English")
            {
                imageVehicleSearch.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/vehicle_search_mo.jpg", UriKind.Relative));
            }
            else
            {
                imageVehicleSearch.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/vehicle_search_mo_a.jpg", UriKind.Relative));
            }
        }

        private void imageVehicleSearch_MouseLeave(object sender, MouseEventArgs e)
        {
            if (AppProperties.Selected_Resource == "English")
            {
                imageVehicleSearch.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/vehicle_search.jpg", UriKind.Relative));
            }
            else
            {
                imageVehicleSearch.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/vehicle_search_a.jpg", UriKind.Relative));
            }
        }



        private void imageOperatorSearch_MouseEnter(object sender, MouseEventArgs e)
        {
            if (AppProperties.Selected_Resource == "English")
            {
                imageOperatorSearch.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/operator_search_mo.jpg", UriKind.Relative));
            }
            else
            {
                imageOperatorSearch.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/operator_search_mo_a.jpg", UriKind.Relative));
            }
        }

        private void imageOperatorSearch_MouseLeave(object sender, MouseEventArgs e)
        {
            if (AppProperties.Selected_Resource == "English")
            {
                imageOperatorSearch.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/operator_search.jpg", UriKind.Relative));
            }
            else
            {
                imageOperatorSearch.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/operator_search_a.jpg", UriKind.Relative));
            }
        }


        private void imageKnowledge_MouseEnter(object sender, MouseEventArgs e)
        {
            if (AppProperties.Selected_Resource == "English")
            {
                imageKnowledge.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/knowledge_mo.jpg", UriKind.Relative));
            }
            else
            {
                imageKnowledge.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/knowledge_mo_a.jpg", UriKind.Relative));
            }
        }

        private void imageKnowledge_MouseLeave(object sender, MouseEventArgs e)
        {
            if (AppProperties.Selected_Resource == "English")
            {
                imageKnowledge.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/knowledge.jpg", UriKind.Relative));
            }
            else
            {
                imageKnowledge.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/knowledge_a.jpg", UriKind.Relative));
            }
        }


        private void imageSynch_MouseEnter(object sender, MouseEventArgs e)
        {
            if (AppProperties.Selected_Resource == "English")
            {
                imageSynch.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/sync_mo.jpg", UriKind.Relative));
            }
            else
            {
                imageSynch.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/sync_mo_a.jpg", UriKind.Relative));
            }
        }

        private void imageSynch_MouseLeave(object sender, MouseEventArgs e)
        {
            if (AppProperties.Selected_Resource == "English")
            {
                imageSynch.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/sync.jpg", UriKind.Relative));
            }
            else
            {
                imageSynch.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/sync_a.jpg", UriKind.Relative));
            }
        }


        private void imageMainMenu_MouseEnter(object sender, MouseEventArgs e)
        {
            if (AppProperties.Selected_Resource == "English")
            {
                imageMainMenu.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/my_profile_mo.jpg", UriKind.Relative));
            }
            else
            {
                imageMainMenu.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/my_profile_mo_a.jpg", UriKind.Relative));
            }
        }

        private void imageMainMenu_MouseLeave(object sender, MouseEventArgs e)
        {
            if (AppProperties.Selected_Resource == "English")
            {
                imageMainMenu.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/my_profile.jpg", UriKind.Relative));
            }
            else
            {
                imageMainMenu.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/my_profile_a.jpg", UriKind.Relative));
            }
        }



        private void imageLogout_MouseEnter(object sender, MouseEventArgs e)
        {
            if (AppProperties.Selected_Resource == "English")
            {
                imageLogout.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/logout_mo.jpg", UriKind.Relative));
            }
            else
            {
                imageLogout.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/logout_mo_a.jpg", UriKind.Relative));
            }
        }

        private void imageLogout_MouseLeave(object sender, MouseEventArgs e)
        {
            if (AppProperties.Selected_Resource == "English")
            {
                imageLogout.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/logout.jpg", UriKind.Relative));
            }
            else
            {
                imageLogout.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/logout_a.jpg", UriKind.Relative));
            }
        }

        private void imagIntellegentTargeting_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                m_MainWindow.MainContentControl.Content = null;
                m_MainWindow.MainContentControl.Content = new ucIntelligentTarget(m_MainWindow);
            }
            catch (Exception ex)
            {
                WPFMessageBoxResult _Result = WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }



        private void imageComercialActivities_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                if (AppProperties.Selected_Resource == "English")
                {
                    //imageComercialActivities.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/comercialActivity_mo.jpg", UriKind.Relative));
                }
                else
                {
                   // imageComercialActivities.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/comercialActivity_mo.jpg", UriKind.Relative));
                }
            }
            catch (Exception ex)
            {
                WPFMessageBoxResult _Result = WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void imageComercialActivities_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {
                if (AppProperties.Selected_Resource == "English")
                {
                   // imageComercialActivities.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/comercialActivity.jpg", UriKind.Relative));
                }
                else
                {
                 //   imageComercialActivities.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/comercialActivity.jpg", UriKind.Relative));
                }
            }
            catch (Exception ex)
            {
                WPFMessageBoxResult _Result = WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

       



    }
}
