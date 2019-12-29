using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
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
using VSDApp.com.rta.vsd.hh.utilities;
using VSDApp.WPFMessageBoxControl;

namespace VSDApp.com.rta.vsd.hh.ui
{
    /// <summary>
    /// Interaction logic for ucGoodByeScreen.xaml
    /// </summary>
    public partial class ucGoodByeScreen : UserControl
    {
        MainWindow m_MainWindow;
        DataTable _dtInspector;
        public ucGoodByeScreen(MainWindow mainWnd, DataTable dtInspector)
        {
            InitializeComponent();
            m_MainWindow = mainWnd;
            _dtInspector = dtInspector;
        }
        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            try
            {
                App.VSDLog.Info("\n ucGoodByScreen.UserControl_Loaded_1: entered in loaded event of ucGoodByScreen");
                ShowInspectorInformation();
                //lblTotalVehicleInspected.Text = AppProperties.Total_Vehicle_Inspected.ToString();
                //  _someLabel.Text = "Whatever";


                //  while (n != 4) ; //Таймер тикает 4 раза.
                // ReadConfigInfo();
                //  InitializeAutoLogoffFeature();

            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }
        public void ShowInspectorInformation()
        {
            try
            {
                if (AppProperties.isOnline == true)
                {

                    //  m_MainWindow.tbuserName.Text = AppProperties.empUserName;
                   
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
                    inspector_pic_path = inspector_pic_path + AppProperties.empUserName.ToString().Trim().ToLower() + "_Arabic.png";
                    lblInspectorName.Content = AppProperties.empUserFullNameAr;
                }
                else
                {
                    inspector_pic_path = inspector_pic_path + AppProperties.empUserName.ToString().Trim().ToLower() + ".png";
                    lblInspectorName.Content = AppProperties.empUserFullName;
                }

                //     new BitmapImage(new Uri(@"/Images/Buttons/Small/Start Inspection Down.png", UriKind.Relative));
                //  inspector_pic_path = @"/Images/Inspectors/hialbedwawi.png";
                imageRTAInsp.Source = new BitmapImage(new Uri(inspector_pic_path, UriKind.Relative));
                PopulateInspectorInfo();

            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }
        //added by kashif abbasi on dated 02-Feb-2016
        public void PopulateInspectorInfo()
        {
            App.VSDLog.Info(" \n Entered in ucGoodByeScreen.PopulateInspectorInfo():");
            try
            {
                System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.DigitSubstitution = System.Globalization.DigitShapes.None;
                this.UpdateLayout();
                if (AppProperties.Selected_Resource == "Arabic")
                {
                    if (_dtInspector != null)
                    {
                        if (_dtInspector.Rows.Count > 0)
                        {
                            try
                            {
                                if (!_dtInspector.Rows[0]["totalInspectionWithFine"].ToString().Equals("") && !_dtInspector.Rows[0]["totalInspectionWithoutFine"].ToString().Equals(""))
                                {
                                    lblTotalInspection.Content = (Convert.ToInt16(_dtInspector.Rows[0]["totalInspectionWithFine"]) + Convert.ToInt16(_dtInspector.Rows[0]["totalInspectionWithoutFine"])).ToString("00");
                                }
                                else
                                {
                                    int cont2 = (2 - 2);
                                    string content = cont2.ToString("00");
                                    lblTotalInspection.Content = content.ToString(new CultureInfo("en-US"));
                                }
                            }
                            catch (Exception ex)
                            {
                                App.VSDLog.Info(" \n ucGoodByeScreen.PopulateInspectorInfo(): Problem adding inspectionsWithFine and WithoutFine Exception :" + ex.Message + "\n" + ex.StackTrace);
                                int cont2 = (2 - 2);
                                string content = cont2.ToString("00");
                                lblTotalInspection.Content = content.ToString(new CultureInfo("en-US"));
                            }
                            lblInspectionWithFine.Content = Convert.ToInt16( _dtInspector.Rows[0]["totalInspectionWithFine"].ToString()).ToString("00");
                            lblInspectionWithoutFine.Content = Convert.ToInt16(_dtInspector.Rows[0]["totalInspectionWithoutFine"].ToString()).ToString("00");
                            lblTotalViolations.Content = Convert.ToInt16(_dtInspector.Rows[0]["totalDefects"].ToString()).ToString("00");
                            lblFineAmount.Content = Convert.ToInt16(_dtInspector.Rows[0]["totalFineAmout"].ToString()).ToString("00");
                            txtblkMessage.Text = new CommonUtils().GetStringValue("goodByScreenMsg");
                           // lblTotalInspection.Content = Thread.CurrentThread.CurrentUICulture.NumberFormat.NativeDigits;
                        }
                        else
                        {

                            int cont2 = (2 - 2);
                            string content = cont2.ToString("00");
                            lblTotalInspection.Content = content.ToString(new CultureInfo("en-US"));
                            
                            //lblTotalInspection.Content = Thread.CurrentThread.CurrentUICulture.NumberFormat.;
                            lblInspectionWithFine.Content = content.ToString(new CultureInfo("en-US"));
                            lblInspectionWithoutFine.Content = content.ToString(new CultureInfo("en-US"));
                            lblTotalViolations.Content = content.ToString(new CultureInfo("en-US"));
                            lblFineAmount.Content = content.ToString(new CultureInfo("en-US"));

                            txtblkMessage.Text = new CommonUtils().GetStringValue("goodByScreenErrMsg");
                            // WPFMessageBox.Show("Information", "Problem in fetching inspector report. ", WPFMessageBoxImage.Information);
                        }
                    }
                    else
                    {
                        
                        int cont2 = (2 - 2);
                        string content = cont2.ToString("00");
                        //There is a connection problem, so we can't retrieve your data
                        lblInspectionWithFine.Content = content.ToString(new CultureInfo("en-US"));
                        lblInspectionWithoutFine.Content = content.ToString(new CultureInfo("en-US"));
                        lblTotalViolations.Content = content.ToString(new CultureInfo("en-US"));
                        lblFineAmount.Content = content.ToString(new CultureInfo("en-US"));
                        txtblkMessage.Text = new CommonUtils().GetStringValue("goodByScreenErrMsg");

                        // WPFMessageBox.Show("Information", "Problem in fetching inspector report. ", WPFMessageBoxImage.Information);
                    }
                }
                else
                {
                    if (_dtInspector != null)
                    {
                        if (_dtInspector.Rows.Count > 0)
                        {
                            try
                            {
                                if (!_dtInspector.Rows[0]["totalInspectionWithFine"].ToString().Equals("") && !_dtInspector.Rows[0]["totalInspectionWithoutFine"].ToString().Equals(""))
                                    lblTotalInspection.Content = Convert.ToInt16(_dtInspector.Rows[0]["totalInspectionWithFine"]) + Convert.ToInt16(_dtInspector.Rows[0]["totalInspectionWithoutFine"]);
                                else
                                    lblTotalInspection.Content = "0";
                            }
                            catch (Exception ex)
                            {
                                App.VSDLog.Info(" \n ucGoodByeScreen.PopulateInspectorInfo(): Problem adding inspectionsWithFine and WithoutFine Exception :" + ex.Message + "\n" + ex.StackTrace);
                                lblTotalInspection.Content = "0";
                            }
                            lblInspectionWithFine.Content = _dtInspector.Rows[0]["totalInspectionWithFine"].ToString();
                            lblInspectionWithoutFine.Content = _dtInspector.Rows[0]["totalInspectionWithoutFine"].ToString();
                            lblTotalViolations.Content = _dtInspector.Rows[0]["totalDefects"].ToString();
                            lblFineAmount.Content = _dtInspector.Rows[0]["totalFineAmout"].ToString();
                            txtblkMessage.Text = new CommonUtils().GetStringValue("goodByScreenMsg");
                        }
                        else
                        {
                            lblTotalInspection.Content = "0";
                            lblInspectionWithFine.Content = "0";
                            lblInspectionWithoutFine.Content = "0";
                            lblTotalViolations.Content = "0";
                            lblFineAmount.Content = "0";

                            txtblkMessage.Text = new CommonUtils().GetStringValue("goodByScreenErrMsg");
                            // WPFMessageBox.Show("Information", "Problem in fetching inspector report. ", WPFMessageBoxImage.Information);
                        }
                    }
                    else
                    {
                        //There is a connection problem, so we can't retrieve your data
                        lblInspectionWithFine.Content = "0";
                        lblInspectionWithoutFine.Content = "0";
                        lblTotalViolations.Content = "0";
                        lblFineAmount.Content = "0";
                        txtblkMessage.Text = new CommonUtils().GetStringValue("goodByScreenErrMsg");

                        // WPFMessageBox.Show("Information", "Problem in fetching inspector report. ", WPFMessageBoxImage.Information);
                    }
                }
                App.VSDLog.Info("Inspection With Fine " + lblInspectionWithFine.Content +
                 "Inspection WithOut Fine" + lblInspectionWithoutFine.Content +
                 "Total Violations" + lblTotalViolations.Content +
                 "Total Fine Ammount" + lblFineAmount.Content);
                System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.DigitSubstitution = System.Globalization.DigitShapes.None;
                this.UpdateLayout();
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(" \n ucGoodByeScreen.PopulateInspectorInfo(): Exception :"   + "\n" + ex.StackTrace);
                App.VSDLog.Info(ex.Message);
                WPFMessageBox.Show("Exception", "Problem in fetching inspectors report. ", ex.StackTrace, WPFMessageBoxImage.Error);
            }
        }
       
        private void imagebtnNext_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {

        }

        private void imagebtnNext_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {

        }

        private void UserControl_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {
            if (new CommonUtils().GetScreenOrientation() == AppProperties.LandScapetMode)
            {
                stckPanelUpperMain.Orientation = Orientation.Horizontal;
            }
            else
            {
                stckPanelUpperMain.Orientation = Orientation.Vertical;
            }
           
            /*
            if (new CommonUtils().GetScreenOrientation() == AppProperties.LandScapetMode)
            {
                maingrid.Children.Remove(lblInspectionResult);
                Grid.SetRow(lblInspectionResult, 3);
                Grid.SetColumn(lblInspectionResult, 5);
                maingrid.Children.Add(lblInspectionResult);

                maingrid.Children.Remove(StackPanelResult);
                Grid.SetRow(StackPanelResult, 5);
                Grid.SetColumn(StackPanelResult, 5);
                maingrid.Children.Add(StackPanelResult);


                maingrid.Children.Remove(lblAlwaysReme);
                Grid.SetRow(lblAlwaysReme, 7);
                Grid.SetColumn(lblAlwaysReme, 3);
                maingrid.Children.Add(lblAlwaysReme);

                maingrid.Children.Remove(lblCreativity);
                Grid.SetRow(lblCreativity, 9);
                Grid.SetColumn(lblCreativity, 3);
                maingrid.Children.Add(lblCreativity);
            }
            else
            {
                maingrid.Children.Remove(lblInspectionResult);
                Grid.SetRow(lblInspectionResult, 7);
                Grid.SetColumn(lblInspectionResult, 3);
                maingrid.Children.Add(lblInspectionResult);

                maingrid.Children.Remove(StackPanelResult);
                Grid.SetRow(StackPanelResult, 9);
                Grid.SetColumn(StackPanelResult, 3);
                maingrid.Children.Add(StackPanelResult);

                maingrid.Children.Remove(lblAlwaysReme);
                Grid.SetRow(lblAlwaysReme, 11);
                Grid.SetColumn(lblAlwaysReme, 3);
                maingrid.Children.Add(lblAlwaysReme);

                maingrid.Children.Remove(lblCreativity);
                Grid.SetRow(lblCreativity, 13);
                Grid.SetColumn(lblCreativity, 3);
                maingrid.Children.Add(lblCreativity);

            }*/
        }

        private void btnBackImage2_Click(object sender, RoutedEventArgs e)
        {
            m_MainWindow.MainContentControl.Content = null;
            m_MainWindow.MainContentControl.Content = new ucLoginEn(m_MainWindow);
        }

        private void UserControl_Initialized(object sender, EventArgs e)
        {

        }

        private void lblTotalInspection_TextInput(object sender, TextCompositionEventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.DigitSubstitution = System.Globalization.DigitShapes.None;
            this.UpdateLayout();
        }

        private void lblFineAmount_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.DigitSubstitution = System.Globalization.DigitShapes.None;
            this.UpdateLayout();
        }

    }
}
