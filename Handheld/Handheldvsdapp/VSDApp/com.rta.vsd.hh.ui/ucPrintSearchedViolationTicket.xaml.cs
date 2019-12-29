﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
using Microsoft.Win32.SafeHandles;
using VSDApp.com.rta.vsd.hh.data;
using VSDApp.com.rta.vsd.hh.db;
using VSDApp.com.rta.vsd.hh.utilities;
using VSDApp.ProgressDialog;
using VSDApp.WPFMessageBoxControl;

namespace VSDApp.com.rta.vsd.hh.ui
{
    /// <summary>
    /// Interaction logic for ucPrintSearchedViolationTicket.xaml
    /// </summary>
    public partial class ucPrintSearchedViolationTicket : UserControl
    {

        private MainWindow m_MainWindow = null;
        int count_arabic_print = 0;
        int count_eng_print = 0;
        ViolationTicket violationTicketDetials;
        public bool Is_Violation_Listing = false;
        Object user_Control_Old;
        bool is_Enforce_Testing = false;
        bool is_elligble_pocession = false;

        public ucPrintSearchedViolationTicket(MainWindow mainWnd, ViolationTicket violtnTickt, bool is_from_vio, Object usercont)
        {
            InitializeComponent();
            m_MainWindow = mainWnd;
            violationTicketDetials = violtnTickt;
            Is_Violation_Listing = is_from_vio;
            this.user_Control_Old = usercont;
        }

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            this.txtImpounding.Text = "0";
            if (AppProperties.Selected_Resource == "English")
            {
                imagebtnBack.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Back.png", UriKind.Relative));
                imageBtnPrint.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Print.png", UriKind.Relative));


            }
            else
            {
                imagebtnBack.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/back Arabic Up.png", UriKind.Relative));
                imageBtnPrint.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Print Arabic Up.png", UriKind.Relative));
                btnStackePanel.FlowDirection = System.Windows.FlowDirection.LeftToRight;

            }
            PopulateData();
            rdoBtnArabLang.IsChecked = true;
            System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.DigitSubstitution = System.Globalization.DigitShapes.None;

            this.UpdateLayout();
        }

        private void UserControl_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {
            if (new CommonUtils().GetScreenOrientation() == AppProperties.LandScapetMode)
            {
                // MessageBox.Show("LandScape");
                // ChangeControlDimensions(350);
                ChangeControlDimensions(AppProperties.LndScp_220_350);
                ChangeButtonsDimensions(AppProperties.LndScp_Btn_100_150);
                ChangeLableDimensions(AppProperties.LndScp_Lbl_20_25);
                ChangeHeaderDimensions(AppProperties.LndScp_Header_25_30);



            }
            else
            {
                //  MessageBox.Show("Potrait");
                // ChangeControlDimensions(250);
                ChangeControlDimensions(AppProperties.Prtrt_350_220);
                ChangeButtonsDimensions(AppProperties.Prtrt_Btn_150_100);
                ChangeLableDimensions(AppProperties.Prtrt_Lbl_25_20);
                ChangeHeaderDimensions(AppProperties.Prtrt_Header_30_25);
                //  this.grdAddedDefects.Width = 500;
            }

        }

        private void btnback_Click_1(object sender, MouseButtonEventArgs e)
        {
            if (AppProperties.Selected_Resource == "English")
            {
                imagebtnBack.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Back.png", UriKind.Relative));
            }
            else
            {
                imagebtnBack.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/back Arabic Up.png", UriKind.Relative));
            }
            if (Is_Violation_Listing)
            {
                m_MainWindow.MainContentControl.Content = null;
                ucSearchViolationListing.isBackFrmPrint = true;
                ucSearchViolationListing.isClearGridonBack = true;
                m_MainWindow.MainContentControl.Content = this.user_Control_Old;
                System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.DigitSubstitution = System.Globalization.DigitShapes.None;
                this.UpdateLayout();
                //  m_MainWindow.MainContentControl.Content = new ucSearchViolationListing(this.m_MainWindow);

                Is_Violation_Listing = false;
            }
            else
            {
                m_MainWindow.MainContentControl.Content = null;
                ucSearchedVehicleDetials.isBackFrmPrint = true;
                m_MainWindow.MainContentControl.Content = this.user_Control_Old;
                //  m_MainWindow.MainContentControl.Content = new ucSearchedVehicleDetials(this.m_MainWindow)

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


        private void imageBtnPrint_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            if (AppProperties.Selected_Resource == "English")
            {
                imageBtnPrint.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Print Down.png", UriKind.Relative));
            }
            else
            {
                imageBtnPrint.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Print Arabic Down.png", UriKind.Relative));
            }
        }

        private void rdoBtnEngLang_Checked_1(object sender, RoutedEventArgs e)
        {
            rdoBtnArabLang.IsChecked = false;
        }

        private void rdoBtnArabLang_Checked_1(object sender, RoutedEventArgs e)
        {
            rdoBtnEngLang.IsChecked = false;
        }
        private void PopulateData()
        {
            try
            {
                /////////////////////////
                //Adding English and Arabic Option
                /////////////////////////////
                if (cmboxLanguageSelction.Items.Count == 0)
                {
                    cmboxLanguageSelction.Items.Add("English");
                    cmboxLanguageSelction.Items.Add("العربية");
                    if (Thread.CurrentThread.CurrentCulture.ToString() == "ar-AE")
                    {
                        cmboxLanguageSelction.SelectedItem = "العربية";
                    }
                    else
                    {
                        cmboxLanguageSelction.SelectedItem = "English";
                    }
                }


                if (AppProperties.Selected_Resource == "English")
                {
                    txtboxViolationSeverity.Text = violationTicketDetials.ViolationAdvice;
                    txtboxDueDate.Text = violationTicketDetials.DueDate;
                    txtViolationID.Text = violationTicketDetials.ViolationID;
                    /////////////////////////////////////////////////////////////
                    //  txtImpounding.Text = AppProperties.vehicle.TotalImpoundingDays;
                    String Grace_Period = string.Empty;
                    if (violationTicketDetials.GraePeriod == "T")
                        Grace_Period = "24h";
                    else
                        Grace_Period = "None";
                    txtGracePeriod.Text = Grace_Period;
                    List<VSDApp.com.rta.vsd.hh.data.Violation.DefectFines> AddedFines = new List<VSDApp.com.rta.vsd.hh.data.Violation.DefectFines>();
                    App.VSDLog.Info("\n starting foreach loop in PopulateData function\n");
                    foreach (VSDApp.com.rta.vsd.hh.data.Violation.Defects defct in violationTicketDetials.DefectDetails)
                    {

                        App.VSDLog.Info("\ndefect fine amount:" + defct.FineAmount + "\n");
                        if ((defct.FineAmount != null) && (defct.FineAmount != ""))
                        {
                            if (Int32.Parse(defct.FineAmount) > 0)
                            {
                                VSDApp.com.rta.vsd.hh.data.Violation.DefectFines fine = new VSDApp.com.rta.vsd.hh.data.Violation.DefectFines();
                                fine.FineName = defct.FineName;
                                fine.FineAmmount = defct.FineAmount;
                                bool is_fine_added = false;
                                if (this.txtImpounding.Text == null || this.txtImpounding.Text == "")
                                {
                                    this.txtImpounding.Text = "0";
                                }
                                txtImpounding.Text = (Int32.Parse(this.txtImpounding.Text) + Int32.Parse(defct.ImpoundingDays)).ToString();
                                foreach (VSDApp.com.rta.vsd.hh.data.Violation.DefectFines fne in AddedFines)
                                {
                                    if (fne.FineName.Equals(fine.FineName))
                                        is_fine_added = true;
                                }
                                if (!is_fine_added)
                                {
                                    AddedFines.Add(fine);
                                }
                            }
                        }
                    }

                    grdAddedFines.ItemsSource = null;
                    grdAddedFines.ItemsSource = AddedFines;

                }
                else
                {
                    txtboxViolationSeverity.Text = violationTicketDetials.ViolationAdvice;
                    txtboxDueDate.Text = violationTicketDetials.DateTime;
                    txtViolationID.Text = violationTicketDetials.ViolationID;
                    ////////////////////////////////////////////////////////////

                    String Grace_Period = string.Empty;
                    if (violationTicketDetials.GraePeriod == "T")
                        Grace_Period = "24h";
                    else
                        Grace_Period = "لا يوجد";
                    txtGracePeriod.Text = Grace_Period;
                    List<VSDApp.com.rta.vsd.hh.data.Violation.DefectFines> AddedFines = new List<VSDApp.com.rta.vsd.hh.data.Violation.DefectFines>();
                    App.VSDLog.Info("\n starting foreach loop in PopulateData function\n");
                    foreach (VSDApp.com.rta.vsd.hh.data.Violation.Defects defct in violationTicketDetials.DefectDetails)
                    {

                        App.VSDLog.Info("\ndefect fine amount:" + defct.FineAmount + "\n");
                        if ((defct.FineAmount != null) && (defct.FineAmount != ""))
                        {
                            if (Int32.Parse(defct.FineAmount) > 0)
                            {
                                VSDApp.com.rta.vsd.hh.data.Violation.DefectFines fine = new VSDApp.com.rta.vsd.hh.data.Violation.DefectFines();
                                fine.FineName = defct.FineNameAr;
                                fine.FineAmmount = defct.FineAmount;
                                bool is_fine_added = false;
                                txtImpounding.Text = (Int32.Parse(this.txtImpounding.Text) + Int32.Parse(defct.ImpoundingDays)).ToString();
                                foreach (VSDApp.com.rta.vsd.hh.data.Violation.DefectFines fne in AddedFines)
                                {
                                    if (fne.FineName.Equals(fine.FineName))
                                        is_fine_added = true;
                                }
                                if (!is_fine_added)
                                {
                                    AddedFines.Add(fine);
                                }
                            }
                        }
                    }

                    grdAddedFines.ItemsSource = null;
                    grdAddedFines.ItemsSource = AddedFines;
                }

                // showDefectAdvice.Width = (AppProperties.defaultControlWidth > Math.Max(Math.Max(vsd.hh.utilities.CommonUtils.CalculateStringLength(showViolationID.Text, showViolationID.Font), vsd.hh.utilities.CommonUtils.CalculateStringLength(showDefectAdvice.Text, showDefectAdvice.Font)), vsd.hh.utilities.CommonUtils.CalculateStringLength(showDueDate.Text, showDueDate.Font)) ? AppProperties.defaultControlWidth : Math.Max(Math.Max(vsd.hh.utilities.CommonUtils.CalculateStringLength(showViolationID.Text, showViolationID.Font), vsd.hh.utilities.CommonUtils.CalculateStringLength(showDefectAdvice.Text, showDefectAdvice.Font)), vsd.hh.utilities.CommonUtils.CalculateStringLength(showDueDate.Text, showDueDate.Font)));
                //showViolationID.Width = showDefectAdvice.Width;
                //showDueDate.Width = showDefectAdvice.Width;

                //printButton.Location = new Point(showDueDate.Location.X + showDueDate.Width - printButton.Width, showDueDate.Location.Y + showDueDate.Height + vsd.hh.utilities.AppProperties.padding);
                //nextButton.Location = new Point(printButton.Location.X - vsd.hh.utilities.AppProperties.padding - nextButton.Width, printButton.Location.Y);
                //backButton.Location = new Point(printButton.Location.X + printButton.Width - backButton.Width, printButton.Location.Y + printButton.Height + vsd.hh.utilities.AppProperties.padding);

                if (!AppProperties.canPrintViolation)
                {
                    //btnPrint.IsEnabled = false;
                    // AppProperties.isComprehensive = false;
                    AppProperties.isSafety = true;
                    AppProperties.isViolation = false;
                }

                if ((txtViolationID.Text != "" && txtViolationID.Text.StartsWith("p", StringComparison.CurrentCultureIgnoreCase)) || AppProperties.isComprehensive)
                {
                    if (txtViolationID.Text.StartsWith("p", StringComparison.CurrentCultureIgnoreCase))
                    {
                        txtViolationID.IsEnabled = true;
                    }
                    AppProperties.isComprehensive = false;
                    AppProperties.isSafety = true;
                    AppProperties.isViolation = false;

                }
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        #region new design 
        private void PrintEnglishRecipt()
        {
            try
            {


                String Violation_ID = violationTicketDetials.ViolationID;
                String Violation_Advice = violationTicketDetials.ViolationAdvice;
                String datetime = violationTicketDetials.DateTime;

                String Inspection_Location = violationTicketDetials.Location;
                String Emp_No = violationTicketDetials.RtaEmpNo;
                String Liscense_No = violationTicketDetials.DriverLicNo;
                String plate_details = violationTicketDetials.VehicleDetials;

                /// string[] splitted_Veh_Info = plate_details.Split(" ");

                //////////////////////////

                //Vehicle New attributes VDCS

                String Vehicle_Risk_Ratting = string.Empty;
                if (violationTicketDetials.VRR == null || violationTicketDetials.VRR.Equals(""))
                    Vehicle_Risk_Ratting = "NA";
                else
                    Vehicle_Risk_Ratting = violationTicketDetials.VRR;
                String Driver_Risk_Ratting = string.Empty;
                if (violationTicketDetials.DRR == null || violationTicketDetials.DRR.Equals(""))
                    Driver_Risk_Ratting = "NA";
                else
                    Driver_Risk_Ratting = violationTicketDetials.DRR;
                String Imponding_days = this.txtImpounding.Text;
                String Grace_Period = string.Empty;
                string Due_date = string.Empty;
                if (Violation_Advice.Trim().Equals("Minor Defect Advice"))
                {
                    Due_date = "No Test Required";
                }
                else
                {
                    Due_date = violationTicketDetials.DueDate;
                }
                //////////////////////////////////////////////
                ////Eligibility for posession certificate
                /////////////////////////////////////////////                
                if (AppProperties.vehicle != null)
                {
                    if (AppProperties.vehicle.IsElligibleForPocession != null && AppProperties.vehicle.IsElligibleForPocession.Equals("True"))
                    {
                        if (AppProperties.recordedViolation.ViolationSeverity.Contains("Severe"))
                        {
                            is_elligble_pocession = true;
                        }

                    }
                }
                if (violationTicketDetials.GraePeriod == "T")
                    Grace_Period = "24h";
                else
                    Grace_Period = "NA";

                string Data_Print = string.Empty;
                Data_Print = Data_Print + "^XA^MD13^CWX,E:TT0003M_.FNT^FS" +

                ////////////////////////////////////////////////////////////////

                                            "^FT164,100^AXN,17,18^FD" + Violation_ID + "^FS" +
                                           " ^FT164,121^AXN,17,18^FD" + Violation_Advice + "^FS" +

                                           "^FT164,246^AXN,17,18^FD" + datetime + "^FS" +
                                           "^FT164,271^AXN,17,18^FD" + Inspection_Location + "^FS" +
                                           "^FT164,297^AXN,17,18^FD" + Emp_No + "^FS" +
                                           "^FT164,368^AXN,17,18^FD" + Liscense_No + "^FS" +
                                           "^FT164,400^AXN,17,18^FD" + plate_details + "^FS";



                if (!AppProperties.isOfflineDataPrint)
                {
                    //Changed on -07-10-2018 for printer design change  

                    #region old printer design
                    //Data_Print = Data_Print + "^FT343,610^AXN,17,18^FD" + Driver_Risk_Ratting + "^FS" +
                    //                          "^FT223,610^AXN,17,18^FDVehicle Risk Ratting^FS" +
                    //                          "^FT203,610^AXN,17,18^FD" + Vehicle_Risk_Ratting + "^FS " +
                    //                          "^FT400,610^AXN,17,18^FDDriver Risk Ratting^FS " +
                    //                          "^FO22,618^GB545,1,2^FS";

                    #endregion

                    #region new printer change
                    Data_Print = Data_Print + "^FT305,610^AXN,17,18^FD" + Vehicle_Risk_Ratting + "^FS" +
                                               "^FT22,610^AXN,17,18^^FS" +
                                               "^FT305,630^AXN,17,18^FD" + Driver_Risk_Ratting + "^FS " +
                                               "^FT400,630^AXN,17,18^FD^FS ";
                    #endregion
                }

                //Changed on -07-10-2018 for printer design change  

                #region old printer design
                //Data_Print = Data_Print + "^FT22,637^AXN,17,18^FDSection 4: Fines and Defects:^FS" +
                //                            "^FO22,640^GB230,0,1^FS ";
                #endregion
                ///////////////////

                string zpl_defect_categories = string.Empty;

                for (int i = 0; i < violationTicketDetials.DefectDetails.Length; i++)
                {
                    string defect_cat = violationTicketDetials.DefectDetails[i].DefectCategory;
                    if (defect_cat == "Vehicle Identification")
                    {
                        zpl_defect_categories += "^FO30,510^GC30,2,B^FS ";
                    }
                    if ((defect_cat == "Engine") || (defect_cat == "Steering and Suspension"))
                    {
                        zpl_defect_categories += "^FO50,510^GC30,2,B^FS ";
                    }
                    if (defect_cat == "Safety Requirements")
                    {
                        zpl_defect_categories += "^FO250,510^GC30,2,B^FS ";
                    }
                    if ((defect_cat == "Brake") || (defect_cat == "Wheels and Tyres"))
                    {
                        zpl_defect_categories += "^FO457,545^GC30,2,B^FS ";
                    }
                    if (defect_cat == "Lighting")
                    {
                        zpl_defect_categories += "^FO500,545^GC30,2,B^FS ";
                    }

                }
                Data_Print = Data_Print + zpl_defect_categories;

                //Changed on -07-10-2018 for printer design change
                //int vertical_start_Point = 660;
                int vertical_start_Point = 690;


                int max_defect_lenght = 40;
                int max_fine_length = 40;
                int max_Coments_lenght = 35;
                String previous_fine_ID = "";
                bool is_1stTime = true;

                DataTable defect_data = GetDataTableFromDefectFineList(violationTicketDetials.DefectDetails);




                foreach (DataRow defect in defect_data.Rows)
                {

                    String Defect_Code = defect["DefectCode"].ToString();
                    String Defect_Name = defect["DefectName"].ToString();
                    String Defect_Severity = defect["DefectSeverity"].ToString();
                    String Coments = defect["DefectValue"].ToString();
                    String Fine_Name = Convert.ToString(defect["FineName"]);
                    String Fine_ID = defect["FineID"].ToString();
                    //  String EnforceTesting = defect["EnforceTesting"].ToString();

                    //Spliting of defect into two lines
                    string[] splitedDefectList = Defect_Name.Split(' ');

                    String Defect_Name_1stLine = String.Empty;
                    String Defect_Name_2ndLine = String.Empty;

                    foreach (string str in splitedDefectList)
                    {
                        if (Defect_Name_1stLine.Length + str.Length < max_defect_lenght)
                        {
                            Defect_Name_1stLine = Defect_Name_1stLine + str + " ";
                        }
                        else
                        {
                            Defect_Name_2ndLine += str + " ";
                        }
                    }
                    // Spliting of Fines into 2 liens

                    string[] splitedFineName = Fine_Name.Split(' ');

                    String Fine_Name_1stLine = String.Empty;
                    String Fine_Name_2ndLine = String.Empty;

                    foreach (string str in splitedFineName)
                    {
                        if (Fine_Name_1stLine.Length + str.Length < max_fine_length)
                        {
                            Fine_Name_1stLine = Fine_Name_1stLine + str + " ";
                        }
                        else
                        {
                            Fine_Name_2ndLine += str + " ";
                        }
                    }
                    if (is_1stTime)
                    {
                        if (!Fine_ID.Equals("0"))
                        {

                            Data_Print = Data_Print + " ^FT22," + vertical_start_Point + "^AXN,17,18^FDFine:^FS";
                            Data_Print = Data_Print + " ^FT85," + vertical_start_Point + "^AXN,17,18^FD" + Fine_Name_1stLine + "^FS";
                            if (Fine_Name_2ndLine != String.Empty)
                            {
                                vertical_start_Point = vertical_start_Point + 22;
                                Data_Print = Data_Print + " ^FT85," + vertical_start_Point + "^AXN,17,18^FD" + Fine_Name_2ndLine + "^FS";
                            }

                            vertical_start_Point = vertical_start_Point + 22;
                            is_1stTime = false;


                        }
                    }
                    else
                    {
                        if ((!Fine_ID.Equals("0")))
                        {
                            if (Fine_ID.Equals(previous_fine_ID))
                            {
                                vertical_start_Point = vertical_start_Point + 22;
                            }
                            else
                            {
                                vertical_start_Point = vertical_start_Point + 5;
                                //Changed on -07-10-2018 for printer design change
                                Data_Print = Data_Print + "^FO22," + vertical_start_Point + "^GB545,1,2^FS";
                                vertical_start_Point = vertical_start_Point + 20;
                                Data_Print = Data_Print + " ^FT22," + vertical_start_Point + "^AXN,17,18^FDFine:^FS";
                                Data_Print = Data_Print + " ^FT85," + vertical_start_Point + "^AXN,17,18^FD" + Fine_Name_1stLine + "^FS";
                                if (Fine_Name_2ndLine != String.Empty)
                                {
                                    vertical_start_Point = vertical_start_Point + 22;
                                    Data_Print = Data_Print + " ^FT85," + vertical_start_Point + "^AXN,17,18^FD" + Fine_Name_2ndLine + "^FS";
                                }
                                vertical_start_Point = vertical_start_Point + 22;
                            }
                        }
                        else
                        {
                            vertical_start_Point = vertical_start_Point + 5;
                            Data_Print = Data_Print + "^FO22," + vertical_start_Point + "^GB545,1,2^FS";
                            vertical_start_Point = vertical_start_Point + 22;
                        }
                    }

                    ////////////////////
                    Data_Print = Data_Print + " ^FT22," + vertical_start_Point + "^AXN,17,18^FDDefect:^FS";

                    Data_Print = Data_Print + " ^FT85," + vertical_start_Point + "^AXN,17,18^FD" + Defect_Code + "^FS";
                    Data_Print = Data_Print + " ^FT145," + vertical_start_Point + "^AXN,17,18^FD" + Defect_Name_1stLine + "^FS";
                    Data_Print = Data_Print + " ^FT496," + vertical_start_Point + "^AXN,17,18^FD" + Defect_Severity + "^FS";
                    if (Defect_Name_2ndLine != String.Empty)
                    {
                        vertical_start_Point = vertical_start_Point + 22;
                        Data_Print = Data_Print + " ^FT145," + vertical_start_Point + "^AXN,17,18^FD" + Defect_Name_2ndLine + "^FS";
                    }


                    var splitedCmment = new List<string>();
                    String Coments_Name_1stLine = String.Empty;
                    String Comentst_Name_2ndLine = String.Empty;
                    if (Coments != null)
                    {
                        if (new CommonUtils().IsEnglish(Coments))
                        {
                            splitedCmment = Coments.Split(' ').ToList<string>();
                            foreach (string str in splitedCmment)
                            {
                                if (Coments_Name_1stLine.Length + str.Length < max_Coments_lenght)
                                {
                                    Coments_Name_1stLine = Coments_Name_1stLine + str + " ";
                                }
                                else
                                {
                                    Comentst_Name_2ndLine += str + " ";
                                }
                            }

                        }
                        else
                        {
                            splitedCmment = new CommonUtils().SplitStringIntoWords2(Coments, Coments.Length);
                            foreach (string str in splitedCmment)
                            {
                                if (Coments_Name_1stLine.Length + str.Length < max_Coments_lenght)
                                {
                                    // Coments_Name_1stLine += str + " ";
                                    int n;
                                    bool isNumeric = int.TryParse(str, out n);
                                    if (isNumeric)
                                    {

                                        Coments_Name_1stLine += "";
                                        Coments_Name_1stLine += str;

                                    }
                                    else
                                    {
                                        Coments_Name_1stLine += str;
                                    }
                                }
                                else
                                {
                                    // Coments_Name_2ndLine += str + " ";
                                    int n;
                                    bool isNumeric = int.TryParse(str, out n);
                                    // Coments_Name_2ndLine += str + " ";
                                    if (isNumeric)
                                    {
                                        Comentst_Name_2ndLine += "";
                                        Comentst_Name_2ndLine += str;
                                    }
                                    else
                                    {

                                        Comentst_Name_2ndLine += str;
                                    }
                                }
                            }
                        }
                    }


                    //  string[] splitedCmment = Coments.Split(' ');

                    if (!Coments_Name_1stLine.Equals(String.Empty) && Coments_Name_1stLine.Length > 1)
                    {
                        vertical_start_Point = vertical_start_Point + 22;
                        Data_Print = Data_Print + " ^FT85," + vertical_start_Point + "^AXN,17,18^FDComments:^FS";

                        Data_Print = Data_Print + " ^FT185," + vertical_start_Point + "^CI28^AXN,17,18^F8^FD" + Coments_Name_1stLine + "^FS";
                        if (Comentst_Name_2ndLine != String.Empty)
                        {
                            vertical_start_Point = vertical_start_Point + 22;
                            Data_Print = Data_Print + " ^FT185," + vertical_start_Point + "^CI28^AXN,17,18^F8^FD" + Comentst_Name_2ndLine + "^FS";
                        }
                    }


                    previous_fine_ID = Fine_ID;
                    //  string[] splitedCmment = Coments.Split(' ');







                    //////////////////////////




                }
                vertical_start_Point = vertical_start_Point + 13;

                Data_Print = Data_Print + "^FO22," + vertical_start_Point + "^GB545,1,2^FS";



                if (is_elligble_pocession)
                {
                    Data_Print = Data_Print + "^FO22,1340^GB545,1,2^FS" +
                    "^FT22,1365^AXN,17,18^FDPossession certificate required for impound release^FS";
                }


                Data_Print = Data_Print + " ^FO22,1375^GB545,1,2^FS" +

                   "^FT22,1400^AXN,17,18^FDDefect Test Due Date:^FS" +
                   "^FT208,1400^AXN,17,18^FD" + Due_date + "^FS";



                //Data_Print = Data_Print + "^FO22,1405^GB545,1,2^FS" +

                #region old printer design
                //Data_Print = Data_Print + "^FO22,1405^GB545,1,2^FS" +
                //"^FT22,1430^AXN,17,18^FDImponding days^FS" +

                //"^FT208,1430^AXN,17,18^FD" + Imponding_days + "^FS" +


                //"^FT400,1430^AXN,17,18^FDGrace Period^FS" +

                //"^FT348,1430^AXN,17,18^FD" + Grace_Period + "^FS" +

                //"^FO22,1445^GB545,1,2^FS" +
                //"^FT22,1470^AXN,17,18^FD(*) Means, Defect required to be fixed inside the custody^FS";


                #endregion

                #region new printer change
                Data_Print = Data_Print + "^FT22,1455^AXN,17,18^FD^FS" +

             "^FT300,1455^AXN,17,18^FD" + Imponding_days + "^FS" +


             "^FT400,1475^AXN,17,18^FD^FS" +

             "^FT300,1475^AXN,17,18^FD" + Grace_Period + "^FS";


                Data_Print = Data_Print + "^PQ1,0,1,Y^XZ";

                #endregion

                UTF8Encoding utf8 = new UTF8Encoding();

                byte[] encodedBytes = utf8.GetBytes(Data_Print);

                if (count_eng_print >= 0)
                {
                    if (WPFMessageBox.Show(new CommonUtils().GetStringValue("Confirmation"), new CommonUtils().GetStringValue("lblTicketConfirmation"), WPFMessageBoxButtons.YesNo, WPFMessageBoxImage.Question) == WPFMessageBoxResult.Yes)
                    {
                        // MessageBox.Show("Print");

                        ProgressDialogResult result = ProgressDialog.ProgressDialog.Execute(this.m_MainWindow, new CommonUtils().GetStringValue("Printing"), (bw, we) =>
                        {


                            RawPrinterHelper.SendBytes("ZDesigner iMZ320 (ZPL)", encodedBytes);

                            ProgressDialog.ProgressDialog.CheckForPendingCancellation(bw, we);

                        }, ProgressDialogSettings.WithSubLabelAndCancel);

                        if (result == null || result.Cancelled)
                            return;
                        else if (result.OperationFailed)
                            return;
                        count_eng_print++;
                    }
                }
                else
                {

                    //  MessageBox.Show("Print");
                    ProgressDialogResult result = ProgressDialog.ProgressDialog.Execute(this.m_MainWindow, new CommonUtils().GetStringValue("Printing"), (bw, we) =>
                    {
                        RawPrinterHelper.SendBytes("ZDesigner iMZ320 (ZPL)", encodedBytes);
                        ProgressDialog.ProgressDialog.CheckForPendingCancellation(bw, we);

                    }, ProgressDialogSettings.WithSubLabelAndCancel);

                    if (result == null || result.Cancelled)
                        return;
                    else if (result.OperationFailed)
                        return;
                    count_eng_print++;
                }
            }

            catch (Exception ex)
            {
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }


        }
        private void PrintArabicRecipt()
        {
            try
            {

                string violationTicketCode = violationTicketDetials.ViolationID;
                string issueDate = violationTicketDetials.DateTime;
                string emp_ID = violationTicketDetials.RtaEmpNo;

                string LiscenseNo = violationTicketDetials.DriverLicNo;
                // string plateDetail = violationTicketDetials.VehicleDetailsAr;

                ////////////////////////////////////////////////////////////////////
                Hashtable plateDetails = new Hashtable();
                string plateCatAr = string.Empty;
                string plateCodeAr = string.Empty;
                /*

                if ((AppProperties.vehicle.Country == AppProperties.defaultCountry)|| (AppProperties.vehicle.Country == "AE"))
                {
                   
                    
                    plateDetails = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetPlateDetailsInArabic(AppProperties.vehicle.PlateCategory, AppProperties.vehicle.PlateCode);
                    if (plateDetails != null)
                    {
                        plateCatAr = (string)plateDetails[AppProperties.vehicle.PlateCategory];
                        plateCodeAr = (string)plateDetails[AppProperties.vehicle.PlateCode];
                    }
                   
                   
                   
                }
                else
                {
                    //  plateDetails = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetPlateDetailsInArabic(AppProperties.vehicle.PlateCategory, null);

                    //  plateCatAr = (string)plateDetails[AppProperties.vehicle.PlateCategory];
                    plateCatAr = AppProperties.vehicle.PlateCategory;
                    plateCodeAr = AppProperties.vehicle.PlateCode;
                }

                string plate_Source_Ar = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetPlateSourceCodeAr(("" != AppProperties.vehicle.Emirate.Trim()) ? AppProperties.vehicle.Emirate.Trim() : AppProperties.vehicle.Country.Trim());

               // string plateDetail;
                // plateDetail = (plate_Source_Ar + " , " + plateCodeAr + " " + new CommonUtils().ConvertToEasternArabicNumerals(ReverseString(AppProperties.vehicle.PlateNumber.ToString())) );
                if (AppProperties.vehicle.PlateCategory == "Public Transportation")
                    plateDetail = (plate_Source_Ar + " , " + plateCodeAr + " " + AppProperties.vehicle.PlateNumber.ToString());
                else
                    plateDetail = (plate_Source_Ar + " , " + plateCodeAr + " " + "  " + plateCatAr + " " + AppProperties.vehicle.PlateNumber.ToString());

                */
                ///////////////////////////////////////////////////////////////////

                //  string InspectionLocationAr = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetArabicLocationNameFromEn(AppProperties.location.location);

                string plateDetail = violationTicketDetials.VehicleDetials;
                string InspectionLocationAr = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetArabicLocationNameFromEn(violationTicketDetials.Location);
                string print_str = string.Empty;


                string Due_date = string.Empty;
                if (violationTicketDetials.ViolationAdviceAr.Trim().Equals("إخطار بعطل بسيط"))
                {
                    Due_date = "لا يتوجب فحص"; ;
                }
                else
                {
                    Due_date = violationTicketDetials.DueDate;
                }

                //Vehicle Risk Ratting
                String Vehicle_Risk_Ratting = string.Empty;
                if (violationTicketDetials.VRR == null || violationTicketDetials.VRR.Equals(""))
                    Vehicle_Risk_Ratting = "NA";
                else
                    Vehicle_Risk_Ratting = AppProperties.vehicle.RiskRating;
                String Driver_Risk_Ratting = string.Empty;
                if (violationTicketDetials.DRR == null || violationTicketDetials.DRR.Equals(""))
                    Driver_Risk_Ratting = "NA";
                else
                    Driver_Risk_Ratting = violationTicketDetials.DRR;
                String Imponding_days = this.txtImpounding.Text;
                String Grace_Period = string.Empty;
                if (violationTicketDetials.GraePeriod == "T")
                    Grace_Period = "24h";
                else
                    Grace_Period = "NA";



                print_str = print_str + "^XA^MD13^CWX,E:TT0003M_.FNT^FS" +

               ///////////



                "^FT450,90,1^CI28^AXN,20,20^F8^FD" + violationTicketCode + "^FS" +
                "  ^FT450,111,1^CI28^AXN,20,20^F8^FD" + violationTicketDetials.ViolationAdviceAr + "^FS" +

                "^FT450,236,1^CI28^AXN,20,20^F8^FD" + issueDate + "^FS" +
                 "^FT450,261,1^CI28^AXN,20,20^F8^FD" + violationTicketDetials.LocaitonAr + "^FS" +
                 "^FT450,287,1^CI28^AXN,20,20^F8^FD" + emp_ID + "^FS" +
                 "^FT450,358,1^CI28^AXN,20,20^F8^FD" + LiscenseNo + "^FS" +
                 "^FT450,390,1^CI28^AXN,20,20^F8^FD" + plateDetail + "^FS";

                if (!AppProperties.isOfflineDataPrint)
                {
                    //Changed on -07-10-2018 for printer design change

                    #region new printer change
                    print_str = print_str + "^FT305,600,1^CI28^AXN,20,20^F8^FD" + Vehicle_Risk_Ratting + "^FS" +
                                            "^FT22,600,1^CI28^AXN,20,20^F8^FD^FS" +
                                               "^FT305,630^AXN,17,18^FD" + Driver_Risk_Ratting + "^FS " +
                                               "^FT180,630,1^CI28^AXN,20,20^F8^FD^FS";
                    #endregion
                }

                ////////////////

                string zpl_defect_categories = string.Empty;

                for (int i = 0; i < violationTicketDetials.DefectDetails.Length; i++)
                {
                    string defect_cat = violationTicketDetials.DefectDetails[i].DefectCategory;
                    if (defect_cat == "Vehicle Identification")
                    {
                        zpl_defect_categories += "^FO30,510^GC30,2,B^FS ";
                    }
                    if ((defect_cat == "Engine") || (defect_cat == "Steering and Suspension"))
                    {
                        zpl_defect_categories += "^FO50,510^GC30,2,B^FS ";
                    }
                    if (defect_cat == "Safety Requirements")
                    {
                        zpl_defect_categories += "^FO250,510^GC30,2,B^FS ";
                    }
                    if ((defect_cat == "Brake") || (defect_cat == "Wheels and Tyres"))
                    {
                        zpl_defect_categories += "^FO457,545^GC30,2,B^FS ";
                    }
                    if (defect_cat == "Lighting")
                    {
                        zpl_defect_categories += "^FO500,545^GC30,2,B^FS ";
                    }

                }
                print_str = print_str + zpl_defect_categories;

                //Changed on -07-10-2018 for printer design change

                //int vertical_Start_Point = 665;
                int vertical_Start_Point = 690;
                int max_defect_lenght = 40;
                int max_fine_lenght = 45;
                int max_coments_lenght = 35;

                string previous_fine_ID = string.Empty;
                bool is_1stTime = true;
                DataTable defect_data = GetDataTableFromDefectFineList(violationTicketDetials.DefectDetails);
                foreach (DataRow defect in defect_data.Rows)
                {

                    String Defect_Code = defect["DefectCode"].ToString();
                    String Defect_Name = defect["DefectNameAr"].ToString();
                    String Defect_Severity = defect["DefectSeverityAr"].ToString();
                    String Coments = defect["DefectValue"].ToString();
                    String Fine_Name = Convert.ToString(defect["FineNameAr"]);
                    String Fine_ID = defect["FineID"].ToString();
                    // For Right ALignment


                    var splitedDefectList = new CommonUtils().SplitStringIntoWords(Defect_Name, Defect_Name.Length);

                    String Defect_Name_1stLine = "";
                    String Defect_Name_2ndLine = "";

                    foreach (string str in splitedDefectList)
                    {
                        if (Defect_Name_1stLine.Length + str.Length < max_defect_lenght)
                        {
                            Defect_Name_1stLine += str;
                        }
                        else
                        {
                            Defect_Name_2ndLine += str;
                        }
                    }
                    var splitedFineList = new CommonUtils().SplitStringIntoWords(Fine_Name, Fine_Name.Length);

                    String Fine_1stLine = "";
                    String Fine_2ndLine = "";

                    foreach (string str in splitedFineList)
                    {
                        if (Fine_1stLine.Length + str.Length < max_fine_lenght)
                        {
                            Fine_1stLine += str;
                        }
                        else
                        {
                            Fine_2ndLine += str;
                        }
                    }
                    if (is_1stTime)
                    {
                        if (!Fine_ID.Equals("0"))
                        {
                            print_str = print_str + " ^^FT550," + vertical_Start_Point + ",1^CI28^AXN,20,20^F8^FDنوع المخالفة :^FS^CI0";

                            print_str = print_str + " ^FT440," + vertical_Start_Point + ",1^CI28^AXN,20,20^F8^FD" + Fine_1stLine + "^FS^CI0";
                            if (Fine_2ndLine != String.Empty)
                            {
                                vertical_Start_Point = vertical_Start_Point + 25;
                                print_str = print_str + " ^FT440," + vertical_Start_Point + ",1^CI28^AXN,20,20^F8^FD" + Fine_2ndLine + "^FS^CI0";
                            }
                            vertical_Start_Point = vertical_Start_Point + 25;
                            is_1stTime = false;
                        }
                    }
                    else
                    {
                        if ((!Fine_ID.Equals("0")))
                        {
                            if (Fine_ID.Equals(previous_fine_ID))
                            {
                                vertical_Start_Point = vertical_Start_Point + 25;
                            }
                            else
                            {
                                vertical_Start_Point = vertical_Start_Point + 10;
                                //Changed on -07-10-2018 for printer design change
                                print_str = print_str + "^FO22," + vertical_Start_Point + "^GB545,1,2^FS";
                                vertical_Start_Point = vertical_Start_Point + 25;
                                print_str = print_str + " ^^FT550," + vertical_Start_Point + ",1^CI28^AXN,20,20^F8^FDنوع المخالفة :^FS^CI0";

                                print_str = print_str + " ^FT440," + vertical_Start_Point + ",1^CI28^AXN,20,20^F8^FD" + Fine_1stLine + "^FS^CI0";
                                if (Fine_2ndLine != String.Empty)
                                {
                                    vertical_Start_Point = vertical_Start_Point + 25;
                                    print_str = print_str + " ^FT440," + vertical_Start_Point + ",1^CI28^AXN,20,20^F8^FD" + Fine_2ndLine + "^FS^CI0";
                                }
                                vertical_Start_Point = vertical_Start_Point + 25;
                            }
                        }
                        else
                        {
                            vertical_Start_Point = vertical_Start_Point + 10;
                            print_str = print_str + "^FO22," + vertical_Start_Point + "^GB545,1,2^FS";
                            vertical_Start_Point = vertical_Start_Point + 25;

                        }
                    }



                    print_str = print_str + "^FT550," + vertical_Start_Point + ",1^CI28^AXN,20,20^F8^FDالعطل :^FS^CI0";

                    print_str = print_str + " ^FT460," + vertical_Start_Point + ",1^CI28^AXN,20,20^F8^FD" + Defect_Code + "^FS";
                    print_str = print_str + " ^FT400," + vertical_Start_Point + ",1^CI28^AXN,20,20^F8^FD" + Defect_Name_1stLine + "^FS";
                    print_str = print_str + " ^FT82," + vertical_Start_Point + ",1^CI28^AXN,20,20^F8^FD" + Defect_Severity + "^FS";
                    if (Defect_Name_2ndLine != "")
                    {
                        vertical_Start_Point = vertical_Start_Point + 25;
                        print_str = print_str + " ^FT400," + vertical_Start_Point + ",1^CI28^AXN,20,20^F8^FD" + Defect_Name_2ndLine + "^FS";
                    }



                    var splitedComentList = new List<string>();
                    String Coments_Name_1stLine = "";
                    String Coments_Name_2ndLine = "";
                    if (Coments != null)
                    {
                        if (new CommonUtils().IsEnglish(Coments))
                        {
                            splitedComentList = Coments.Split(' ').ToList<string>();
                            foreach (string str in splitedComentList)
                            {
                                if (Coments_Name_1stLine.Length + str.Length < max_coments_lenght)
                                {
                                    Coments_Name_1stLine += str + " ";
                                }
                                else
                                {
                                    Coments_Name_2ndLine += str + " ";
                                }
                            }
                            /* foreach (string str in splitedComentList)
                             {
                                 if (Coments_Name_1stLine.Length + str.Length < max_coments_lenght)
                                 {
                                     // Coments_Name_1stLine += str + " ";
                                     int n;
                                     bool isNumeric = int.TryParse(str, out n);
                                     if (isNumeric)
                                         Coments_Name_1stLine += str;
                                     else
                                         Coments_Name_1stLine += str + " ";
                                 }
                                 else
                                 {
                                     // Coments_Name_2ndLine += str + " ";
                                     int n;
                                     bool isNumeric = int.TryParse(str, out n);
                                    // Coments_Name_2ndLine += str + " ";
                                     if (isNumeric)
                                         Coments_Name_2ndLine += str;
                                     else
                                         Coments_Name_2ndLine += str + " ";
                                 }
                             } */
                        }
                        else
                        {
                            splitedComentList = new CommonUtils().SplitStringIntoWords2(Coments, Coments.Length);
                            foreach (string str in splitedComentList)
                            {
                                if (Coments_Name_1stLine.Length + str.Length < max_coments_lenght)
                                {
                                    // Coments_Name_1stLine += str + " ";
                                    int n;
                                    bool isNumeric = int.TryParse(str, out n);
                                    if (isNumeric)
                                    {
                                        Coments_Name_1stLine += "";
                                        Coments_Name_1stLine += str;
                                    }
                                    else
                                    {
                                        Coments_Name_1stLine += str;
                                    }

                                }
                                else
                                {
                                    // Coments_Name_2ndLine += str + " ";
                                    int n;
                                    bool isNumeric = int.TryParse(str, out n);
                                    // Coments_Name_2ndLine += str + " ";
                                    if (isNumeric)
                                    {
                                        Coments_Name_2ndLine += "";
                                        Coments_Name_2ndLine += str;
                                    }
                                    else
                                    {
                                        Coments_Name_2ndLine += str;
                                    }
                                }
                            }
                        }
                    }



                    if (!Coments_Name_1stLine.Equals(String.Empty) && Coments_Name_1stLine.Length > 1)
                    {
                        vertical_Start_Point = vertical_Start_Point + 22;
                        print_str = print_str + "^FT550," + vertical_Start_Point + ",1^CI28^AXN,20,20^F8^FDالملاحظات^FS";
                        print_str = print_str + " ^FT450," + vertical_Start_Point + ",1^CI28^AXN,20,20^F8^FD" + Coments_Name_1stLine + "^FS";
                        if (Coments_Name_2ndLine != "")
                        {
                            vertical_Start_Point = vertical_Start_Point + 22;
                            print_str = print_str + "^FT550," + vertical_Start_Point + ",1^CI28^AXN,20,20^F8^FD" + Coments_Name_2ndLine + "^FS";
                        }
                    }

                    previous_fine_ID = Fine_ID;
                }
                vertical_Start_Point = vertical_Start_Point + 20;
                print_str = print_str + "^FO22," + vertical_Start_Point + "^GB545,1,2^FS";


                print_str = print_str + " ^FO22,1360^GB545,1,2^FS" +
                             "^FT550,1385,1^CI28^AXN,20,20^F8^FD تاريخ الاستحقاق^FS" +
                             "^FT208,1385^AXN,17,18^FD" + Due_date + "^FS";


                //Changed on -07-10-2018 for printer design change

                #region old printer design
                //print_str = print_str + "^FO22,1400^GB545,1,2^FS" +

                //"^FT120,1420^AXN,17,18^FD ايام الحجز^FS" +
                //"^FT208,1420^AXN,17,18^FD" + Imponding_days + "^FS" +


                //"^FT550,1420,1^CI28^AXN,20,20^F8^FD فترة سماح^FS" +
                //"^FT348,1420^AXN,17,18^FD" + Grace_Period + "^FS" +


                //"^FO22,1440^GB545,1,2^FS" +
                //"^FT550,1460,1^CI28^AXN,20,20^F8^FD(*)تعني, يمكن إصلاح الأعطال في موقع الحجز^FS";

                #endregion

                #region new printer change
                print_str = print_str + "^FT120,1455^AXN,17,18^FD^FS" +
               "^FT300,1455^AXN,17,18^FD" + Imponding_days + "^FS" +


               "^FT550,1475,1^CI28^AXN,20,20^F8^FD^FS" +
               "^FT300,1475^AXN,17,18^FD" + Grace_Period + "^FS";


                print_str = print_str + " ^PQ1,0,1,Y^XZ";

                #endregion


                UTF8Encoding utf8 = new UTF8Encoding();

                byte[] encodedBytes = utf8.GetBytes(print_str);

                if (count_arabic_print >= 0)
                {
                    if (WPFMessageBox.Show(new CommonUtils().GetStringValue("Confirmation"), new CommonUtils().GetStringValue("lblTicketConfirmation"), WPFMessageBoxButtons.YesNo, WPFMessageBoxImage.Question) == WPFMessageBoxResult.Yes)
                    {
                        ProgressDialogResult result = ProgressDialog.ProgressDialog.Execute(this.m_MainWindow, new CommonUtils().GetStringValue("Printing"), (bw, we) =>
                        {


                            RawPrinterHelper.SendBytes("ZDesigner iMZ320 (ZPL)", encodedBytes);
                            ProgressDialog.ProgressDialog.CheckForPendingCancellation(bw, we);

                        }, ProgressDialogSettings.WithSubLabelAndCancel);

                        if (result == null || result.Cancelled)
                            return;
                        else if (result.OperationFailed)
                            return;
                        count_arabic_print++;
                    }
                }
                else
                {

                    ProgressDialogResult result = ProgressDialog.ProgressDialog.Execute(this.m_MainWindow, new CommonUtils().GetStringValue("Printing"), (bw, we) =>
                    {

                        RawPrinterHelper.SendBytes("ZDesigner iMZ320 (ZPL)", encodedBytes);
                        ProgressDialog.ProgressDialog.CheckForPendingCancellation(bw, we);

                    }, ProgressDialogSettings.WithSubLabelAndCancel);

                    if (result == null || result.Cancelled)
                        return;
                    else if (result.OperationFailed)
                        return;

                    count_arabic_print++;
                }

            }
            catch (Exception ex)
            {
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }
        #endregion 

        public DataTable GetDataTableFromDefectFineList(Violation.Defects[] defectList)
        {
            try
            {

                DataTable dt = new DataTable();
                dt.Columns.Add("DefectID");
                dt.Columns.Add("DefectCode");
                dt.Columns.Add("DefectName");
                dt.Columns.Add("DefectNameAr");
                dt.Columns.Add("DefectSeverity");
                dt.Columns.Add("DefectSeverityAr");
                dt.Columns.Add("DefectValue");
                dt.Columns.Add("FineID", typeof(double));
                dt.Columns.Add("FineName");
                dt.Columns.Add("FineNameAr");
                //  dt.Columns.Add("EnforceTesting");
                DataRow dr;
                string vehCategory = string.Empty;
                if (AppProperties.vehicle != null && AppProperties.vehicle.VehicleCategory != null && AppProperties.vehicle.VehicleCategory != "")
                {
                    vehCategory = AppProperties.vehicle.VehicleCategory;
                }
                else
                {
                    vehCategory = "Heavy Vehicle";
                }
                foreach (Violation.Defects defect in defectList)
                {
                    dr = dt.NewRow();
                    dr["DefectID"] = defect.DefectID;
                    dr["DefectCode"] = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectCode(defect.DefectID).Trim();
                    dr["DefectName"] = defect.DefectName;
                    dr["DefectNameAr"] = defect.DefectNameAr;
                    dr["DefectSeverity"] = defect.DefectSeverity;
                    dr["DefectSeverityAr"] = defect.DefectSeverityAr;
                    dr["DefectValue"] = (null != defect.DefectValue) ? defect.DefectValue : "";

                    if (defect.FineID.Equals("") || defect.FineID.Equals("NA") || defect.FineID == null)
                    {
                        dr["FineID"] = 0;
                    }
                    else
                        dr["FineID"] = Convert.ToDouble(defect.FineID);
                    dr["FineName"] = defect.FineName;
                    dr["FineNameAr"] = defect.FineNameAr;
                    //dr["EnforceTesting"] = defect.EnforceTesting;
                    // if (defect.EnforceTesting.Equals("T"))
                    // {
                    // is_Enforce_Testing = true;
                    // }
                    dt.Rows.Add(dr);
                }

                //sort datatable
                DataView dv = dt.DefaultView;
                dv.Sort = "FineID desc";
                DataTable sortedDT = dv.ToTable();
                return sortedDT;
            }
            catch (Exception ex)
            {
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
                return null;
            }
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern SafeFileHandle CreateFile(string lpFileName, FileAccess dwDesiredAccess,
        uint dwShareMode, IntPtr lpSecurityAttributes, FileMode dwCreationDisposition,
        uint dwFlagsAndAttributes, IntPtr hTemplateFile);
        public void btnPrint_Click_1(object sender, RoutedEventArgs e)
        {

            try
            {
                if ((string)cmboxLanguageSelction.SelectedItem == "English")
                {
                    PrintEnglishRecipt();
                    //  PrintPreview printpre = new PrintPreview(m_MainWindow, this);
                    // printpre.Show();
                }
                else if ((string)cmboxLanguageSelction.SelectedItem == "العربية")
                {
                    PrintArabicRecipt();
                    //  PrintPreview printpre = new PrintPreview(m_MainWindow, this);
                    // printpre.Show();
                }

                else
                {
                    MessageBox.Show("Please Select Printing Language");
                }


            }
            catch (Exception ex)
            {
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        public static string ReverseString(string code)
        {
            try
            {
                if (code == null)
                    return "";
                string Reverse_Code = string.Empty;
                char[] Array_Reverse = code.ToCharArray();
                Array_Reverse = Array_Reverse.Reverse().ToArray();
                for (int i = 0; i < Array_Reverse.Length; i++)
                {
                    Reverse_Code = Reverse_Code + Array_Reverse[i];
                }
                return Reverse_Code;
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
                return "NULL";
            }
        }
        public void ChangeControlDimensions(double width)
        {
            this.txtViolationID.Width = width;
            this.txtboxDueDate.Width = width;
            this.txtboxViolationSeverity.Width = width;


            // this.txtYear.Width = width;

            this.UpdateLayout();
        }
        public void ChangeButtonsDimensions(double width)
        {
            this.imagebtnBack.Width = width;
            this.imageBtnPrint.Width = width;

            this.UpdateLayout();
        }
        public void ChangeButtonsDimensions2(double width)
        {
            // this.btnAddImage.Width = width;
        }
        public void ChangeLableDimensions(double width)
        {
            this.lblOvrAllSev.FontSize = width;
            this.lblVioSum.FontSize = width;
            this.lblDueDate.FontSize = width;
            this.lblPrintLang.FontSize = width;
            // this.lblYear.FontSize = width;
            // this.lblAppLogout.FontSize = 20;
            this.UpdateLayout();
        }
        public void ChangeHeaderDimensions(double width)
        {
            this.lblViolSum.FontSize = width;

            this.UpdateLayout();
        }

        private void UserControl_Initialized_1(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.DigitSubstitution = System.Globalization.DigitShapes.None;
            this.UpdateLayout();
        }

        private void btnback_Click(object sender, RoutedEventArgs e)
        {
            if (Is_Violation_Listing)
            {


                if (count_arabic_print == 0 && count_eng_print == 0)
                {
                    WPFMessageBoxResult res = WPFMessageBox.Show(new CommonUtils().GetStringValue("Confirmation"), new CommonUtils().GetStringValue("lblPrintConfirmation"), WPFMessageBoxButtons.YesNo, WPFMessageBoxImage.Question);
                    if (res == WPFMessageBoxResult.Yes)
                    {
                        m_MainWindow.MainContentControl.Content = null;
                        ucSearchViolationListing.isBackFrmPrint = true;
                        ucSearchViolationListing.isClearGridonBack = true;
                        m_MainWindow.MainContentControl.Content = this.user_Control_Old;
                        System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.DigitSubstitution = System.Globalization.DigitShapes.None;
                        this.UpdateLayout();
                        Is_Violation_Listing = false;
                    }
                }
                else
                {
                    m_MainWindow.MainContentControl.Content = null;
                    ucSearchViolationListing.isBackFrmPrint = true;
                    ucSearchViolationListing.isClearGridonBack = true;
                    m_MainWindow.MainContentControl.Content = this.user_Control_Old;
                    System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.DigitSubstitution = System.Globalization.DigitShapes.None;
                    this.UpdateLayout();
                    Is_Violation_Listing = false;
                }
            }
            else
            {
                if (count_arabic_print == 0 && count_eng_print == 0)
                {
                    WPFMessageBoxResult res = WPFMessageBox.Show(new CommonUtils().GetStringValue("Confirmation"), new CommonUtils().GetStringValue("lblPrintConfirmation"), WPFMessageBoxButtons.YesNo, WPFMessageBoxImage.Question);
                    if (res == WPFMessageBoxResult.Yes)
                    {
                        m_MainWindow.MainContentControl.Content = null;
                        ucSearchedVehicleDetials.isBackFrmPrint = true;
                        m_MainWindow.MainContentControl.Content = this.user_Control_Old;
                        //  m_MainWindow.MainContentControl.Content = new ucSearchedVehicleDetials(this.m_MainWindow)
                    }
                }
                else
                {
                    m_MainWindow.MainContentControl.Content = null;
                    ucSearchedVehicleDetials.isBackFrmPrint = true;
                    m_MainWindow.MainContentControl.Content = this.user_Control_Old;
                    //  m_MainWindow.MainContentControl.Content = new ucSearchedVehicleDetials(this.m_MainWindow)
                }


            }
        }
    }



}