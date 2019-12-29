using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for ucSearchedVehicleDetials.xaml
    /// </summary>
    public partial class ucSearchedVehicleDetials : UserControl
    {
        MainWindow m_mainWindow;
        List<vsd.hh.data.Violation> openViolations = null;
        private IValidation _iValidate;
        private string _validationResult;
        List<DisplayObject> violationData;
        public static bool isBackFrmPrint = false;

        public ucSearchedVehicleDetials(MainWindow mainWnd)
        {
            InitializeComponent();
            this.m_mainWindow = mainWnd;
        }

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            if (!isBackFrmPrint)
            {
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
                if (AppProperties.Selected_Resource == "Arabic")
                {
                    btnStackePanel.FlowDirection = System.Windows.FlowDirection.LeftToRight;
                }
                ClearFields();
                PopulateData();
                SetVehicleRatting();
                PoplateVehicleRecomendation();
            }
            else
            {
                isBackFrmPrint = true;
                PopulateData();
                SetVehicleRatting();
                PopulateOwnerVehicleInfo();
            }
        }
        public void PopulateData()
        {
            try
            {
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
                    PopulateOwnerVehicleInfo();
                }
                /*
                if (AppProperties.vehicle != null)
                {
                    txtBoxChassisNumber.Text = (AppProperties.vehicle == null ? "" : AppProperties.vehicle.ChassisNumber);
                    txtBoxMake.Text = (AppProperties.vehicle == null ? "" : AppProperties.vehicle.Make);
                    txtModel.Text = (AppProperties.vehicle == null ? "" : AppProperties.vehicle.Model);
                    txtBoxOperatorName.Text = (AppProperties.vehicle == null ? "" : (AppProperties.vehicle.Operator == null ? "" : AppProperties.vehicle.Operator.OperatorName));
                   // btnStartInspection.Content = (AppProperties.vehicle.Recomendation != "") ? "Confiscate" : "Inspect";
                    //  txtYear.Text = (AppProperties.vehicle == null ? "" : AppProperties.vehicle.Year);
                   
                }*/
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }
        public void PopulateOwnerVehicleInfo()
        {
            try
            {

                if (AppProperties.vehicle == null)
                    return;
                //if (AppProperties.vehicle.Violations == null)
                //{
                //    MessageBox.Show("No Violations Available");
                //    return;
                //}
                vsd.hh.data.Vehicle VehicleToInspect;
                VehicleToInspect = AppProperties.vehicle;

                if (AppProperties.vehicle.Violations == null)
                    return;
                violationData = new List<DisplayObject>(AppProperties.vehicle.Violations.Length);
                int count = 0;
                foreach (vsd.hh.data.Violation i in AppProperties.vehicle.Violations)
                {
                    // violationData[count] = new DisplayObject(i);
                    violationData.Add(new DisplayObject(i));
                    //  violationData[count].ChassisNumber = AppProperties.vehicle.ChassisNumber;
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
        public void ClearFields()
        {
           
           // this.txtOvrrScore.Text = "";
            this.txtModel.Text = "";
            this.txtYear.Text = "";
            this.txtBoxOperatorName.Text = "";
            this.txtBoxMake.Text = "";
            this.txtBoxChassisNumber.Text = "";
            if (openViolations != null)
                openViolations.Clear();
        }

        private void btnStartInspection_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {

                if (AppProperties.Selected_Resource == "English")
                {
                    imagebtnNext.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Start Inspection.png", UriKind.Relative));
                    _iValidate = (IValidation)new SearchedVehicleDetailsEnValidation();
                    //  this.txtBoxOperatorName.Text = Regex.Replace(this.txtBoxOperatorName.Text, "[^a-zA-Z]+", "", RegexOptions.Compiled);
                    _validationResult = _iValidate.Validate(this);
                    if (AppProperties.vehicle.VehicleCategoryAr != null || AppProperties.vehicle.VehicleCategoryAr == "") 
                    {
                        AppProperties.vehicle.VehicleCategory = "Heavy Vehicle";
                        AppProperties.vehicle.VehicleCategoryAr = "مركبة ثقيلة";
                    }
                }
                else
                {
                    _iValidate = (IValidation)new SearchedVehicleDetailsArValidation();
                    //  this.txtBoxOperatorName.Text = Regex.Replace(this.txtBoxOperatorName.Text, "[^a-zA-Z]+", "", RegexOptions.Compiled);
                    _validationResult = _iValidate.Validate(this);
                    imagebtnNext.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Start Inspection Arabic Up.png", UriKind.Relative));
                     if (AppProperties.vehicle.VehicleCategory != null || AppProperties.vehicle.VehicleCategory == "") 
                    {
                        AppProperties.vehicle.VehicleCategory = "Heavy Vehicle";
                        AppProperties.vehicle.VehicleCategoryAr = "مركبة ثقيلة";
                    }
                }
                if (_validationResult != "Valid")
                {
                    WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), _validationResult);
                }
                else
                {
                    this.m_mainWindow.MainContentControl.Content = null;

                    this.m_mainWindow.MainContentControl.Content = new ucDefectAndViolationDetails(this.m_mainWindow, false);
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

            if (AppProperties.Selected_Resource == "English")
            {
                imagebtnback.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Back.png", UriKind.Relative));
            }
            else
            {
                imagebtnback.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/back Arabic up.png", UriKind.Relative));
            }
            this.m_mainWindow.MainContentControl.Content = null;
            //  this.m_mainWindow.MainContentControl.Content = new ucLocationSelectionEn(this.m_mainWindow);
            this.m_mainWindow.MainContentControl.Content = new ucSearchVehicle(this.m_mainWindow);
        }
        class DisplayObject : Violation
        {
            private string _ChassisNumber;
            private string _VehicleDetail;
            private string _OperatorDetail;
            private string _IssueDate;

            //For Printing Ticket
            private string _inspection_Location;
            private string _inspection_LocationAr;
            Defects[] _defectDetails;
            private string _VRR;
            private string _DRR;
           
            private string _vehCategory;
            private string _gracePeriod;
            private string _dueDate;

            public string DueDate
            {
                get { return _dueDate; }
                set { _dueDate = value; }
            }

            public string GracePeriod
            {
                get { return _gracePeriod; }
                set { _gracePeriod = value; }
            }

            public string VehCategory
            {
                get { return _vehCategory; }
                set { _vehCategory = value; }
            }
            public string IssueDate
            {
                get { return this._IssueDate; }
                set { this._IssueDate = value; }
            }
           

           
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
                DueDate = CopyViolation.ViolationDueDays.ToString("dd/MM/yyyy");
                ViolationStatus = CopyViolation.ViolationStatus;
               

                if (AppProperties.vehicle.PlateCategory == "Public Transportation")
                {
                    VehicleDetail = (AppProperties.vehicle.PlateNumber.ToString() + " " + AppProperties.vehicle.PlateCode + "," + ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetPlateSourceCode((null != AppProperties.vehicle.Emirate) ? AppProperties.vehicle.Emirate : AppProperties.vehicle.Country)); ;
                }
                else
                {
                    VehicleDetail = (AppProperties.vehicle.PlateNumber.ToString() + " " + AppProperties.vehicle.PlateCategory + " " + AppProperties.vehicle.PlateCode + "," + ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetPlateSourceCode((null != AppProperties.vehicle.Emirate) ? AppProperties.vehicle.Emirate : AppProperties.vehicle.Country)); ;
                }



                OperatorDetail = AppProperties.vehicle.Operator.OperatorName;
                ChassisNumber = AppProperties.vehicle.ChassisNumber;
                Inspection_location = CopyViolation.Inspection_location;
                Inspection_locationAr = CopyViolation.Inspection_locationAr;
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
                }
                DefectDetails = CopyViolation.Defect;
                DriverLicNo = CopyViolation.DriverLicNo;
                RtaEmpID = CopyViolation.RtaEmpID;
                ViolationSeverityAr = CopyViolation.ViolationCommentsAr;
                PlateNumber = CopyViolation.PlateNumber;
                PlateCode = CopyViolation.PlateCode;
                PlateCategory = CopyViolation.PlateCategory;
                VRR = AppProperties.vehicle.RiskRating;
                DRR = AppProperties.vehicle.DriverRiskRattingName;
               
                GracePeriod = CopyViolation.GracePeriod;
                

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

        private void txtBoxOperatorName_GotFocus_1(object sender, RoutedEventArgs e)
        {
            CommonUtils.ShowKeyBoard();
        }

        private void txtBoxOperatorName_LostFocus_1(object sender, RoutedEventArgs e)
        {
            CommonUtils.CLoseKeyBoard();
        }
        public void SetVehicleRatting()
        {
            try
            {
                try
                {
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
                            // lblRattingTextwithColor.Visibility = System.Windows.Visibility.Collapsed;
                          //  lblVRR.Visibility = System.Windows.Visibility.Collapsed;
                            AppProperties.vehicle.RiskRating = "";
                        }
                    }
                    else
                    {
                        canv.Visibility = System.Windows.Visibility.Collapsed;
                        grdcanvas.Visibility = System.Windows.Visibility.Collapsed;
                         lblRattingTextwithColor.Visibility = System.Windows.Visibility.Collapsed;
                      // lblVRR.Visibility = System.Windows.Visibility.Collapsed;
                    }
                }
                catch (Exception ex)
                {
                    App.VSDLog.Info(ex.StackTrace);
                    WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
                }

            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void PoplateVehicleRecomendation()
        {
            try
            {
                List<string> vehicle_intered;
                String chassisNo_SearchedVehicle = (AppProperties.vehicle == null ? "" : AppProperties.vehicle.ChassisNumber);
                bool is_intrested = false;

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
                    lblRecmnd.Content = new CommonUtils().GetStringValue("InspectionRecomended");
                    lblRecmnd.Foreground = Brushes.Red;
                    lblRecmnd.FontWeight = FontWeights.Medium;
                }
                else
                {
                    lblRecmnd.Content = new CommonUtils().GetStringValue("InspectionNotRecomended");
                    lblRecmnd.Foreground = Brushes.Green;
                    lblRecmnd.FontWeight = FontWeights.Bold;
                }
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

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
            ChangeControlPositions();
            System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.DigitSubstitution = System.Globalization.DigitShapes.None;
            this.UpdateLayout();
            if (new CommonUtils().GetScreenOrientation() == AppProperties.LandScapetMode)
            {
                // MessageBox.Show("LandScape");
                // ChangeControlDimensions(350);
                 ChangeControlDimensions(AppProperties.LndScp_130_280);
                  ChangeButtonsDimensions(120);
                //  ChangeLableDimensions(AppProperties.LndScp_Lbl_20_25);
                // ChangeHeaderDimensions(AppProperties.LndScp_Header_25_30);
                this.grdViolationDetails.Width = 730;


            }
            else
            {
                //  MessageBox.Show("Potrait");
                // ChangeControlDimensions(250);
                  ChangeControlDimensions(280);
                 ChangeButtonsDimensions(120);
                // ChangeLableDimensions(AppProperties.Prtrt_Lbl_25_20);
                //  ChangeHeaderDimensions(AppProperties.Prtrt_Header_30_25);
                this.grdViolationDetails.Width = 500;
            }
        }
        public void ChangeControlPositions()
        {
            if (new CommonUtils().GetScreenOrientation() == AppProperties.LandScapetMode)
            {
                MainGrid.Children.Remove(lblOprName);
                Grid.SetRow(lblOprName, 3);
                Grid.SetColumn(lblOprName, 1);
                MainGrid.Children.Add(lblOprName);

                MainGrid.Children.Remove(txtBoxOperatorName);
                Grid.SetRow(txtBoxOperatorName, 3);
                Grid.SetColumn(txtBoxOperatorName, 3);
                MainGrid.Children.Add(txtBoxOperatorName);

                MainGrid.Children.Remove(lblChassisName);
                Grid.SetRow(lblChassisName, 3);
                Grid.SetColumn(lblChassisName, 5);
                MainGrid.Children.Add(lblChassisName);

                MainGrid.Children.Remove(txtBoxChassisNumber);
                Grid.SetRow(txtBoxChassisNumber, 3);
                Grid.SetColumn(txtBoxChassisNumber, 7);
                MainGrid.Children.Add(txtBoxChassisNumber);

                MainGrid.Children.Remove(lblMake);
                Grid.SetRow(lblMake, 5);
                Grid.SetColumn(lblMake, 1);
                MainGrid.Children.Add(lblMake);

                MainGrid.Children.Remove(txtBoxMake);
                Grid.SetRow(txtBoxMake, 5);
                Grid.SetColumn(txtBoxMake, 3);
                MainGrid.Children.Add(txtBoxMake);

                MainGrid.Children.Remove(lblModel);
                Grid.SetRow(lblModel, 5);
                Grid.SetColumn(lblModel, 5);
                MainGrid.Children.Add(lblModel);

                MainGrid.Children.Remove(txtModel);
                Grid.SetRow(txtModel, 5);
                Grid.SetColumn(txtModel, 7);
                MainGrid.Children.Add(txtModel);

                MainGrid.Children.Remove(lblYear);
                Grid.SetRow(lblYear, 7);
                Grid.SetColumn(lblYear, 1);
                MainGrid.Children.Add(lblYear);

                MainGrid.Children.Remove(txtYear);
                Grid.SetRow(txtYear, 7);
                Grid.SetColumn(txtYear, 3);
                MainGrid.Children.Add(txtYear);

                MainGrid.Children.Remove(VehOVRR);
                Grid.SetRow(VehOVRR, 7);
                Grid.SetColumn(VehOVRR, 5);
                Grid.SetColumnSpan(VehOVRR, 3);
                MainGrid.Children.Add(VehOVRR);

                MainGrid.Children.Remove(stckPanelVRR);
                Grid.SetRow(stckPanelVRR, 7);
                Grid.SetColumn(stckPanelVRR, 7);
                MainGrid.Children.Add(stckPanelVRR);
                //   Grid.Column="1" Grid.Row="11" Grid.ColumnSpan="4"

                MainGrid.Children.Remove(lblRecmnd);
                Grid.SetRow(lblRecmnd, 9);
                Grid.SetColumn(lblRecmnd, 1);
                Grid.SetColumnSpan(lblRecmnd,3);
                MainGrid.Children.Add(lblRecmnd);

               



                MainGrid.Children.Remove(btnStackePanel);
                Grid.SetRow(btnStackePanel, 9);
                Grid.SetColumn(btnStackePanel, 7);
                MainGrid.Children.Add(btnStackePanel);

                MainGrid.Children.Remove(lblOpnVio);
                Grid.SetRow(lblOpnVio, 11);
                Grid.SetColumn(lblOpnVio, 1);
                MainGrid.Children.Add(lblOpnVio);

                MainGrid.Children.Remove(grdViolationDetails);
                Grid.SetRow(grdViolationDetails, 11);
                Grid.SetColumn(grdViolationDetails, 3);
                Grid.SetColumnSpan(grdViolationDetails, 7);
                MainGrid.Children.Add(grdViolationDetails);





            }
            else
            {
                MainGrid.Children.Remove(lblOprName);
                Grid.SetRow(lblOprName, 3);
                Grid.SetColumn(lblOprName, 1);
                MainGrid.Children.Add(lblOprName);

                MainGrid.Children.Remove(txtBoxOperatorName);
                Grid.SetRow(txtBoxOperatorName, 3);
                Grid.SetColumn(txtBoxOperatorName, 3);
                MainGrid.Children.Add(txtBoxOperatorName);

                MainGrid.Children.Remove(lblChassisName);
                Grid.SetRow(lblChassisName, 5);
                Grid.SetColumn(lblChassisName, 1);
                MainGrid.Children.Add(lblChassisName);

                MainGrid.Children.Remove(txtBoxChassisNumber);
                Grid.SetRow(txtBoxChassisNumber, 5);
                Grid.SetColumn(txtBoxChassisNumber, 3);
                MainGrid.Children.Add(txtBoxChassisNumber);

                MainGrid.Children.Remove(lblMake);
                Grid.SetRow(lblMake, 7);
                Grid.SetColumn(lblMake, 1);
                MainGrid.Children.Add(lblMake);

                MainGrid.Children.Remove(txtBoxMake);
                Grid.SetRow(txtBoxMake, 7);
                Grid.SetColumn(txtBoxMake, 3);
                MainGrid.Children.Add(txtBoxMake);

                MainGrid.Children.Remove(lblModel);
                Grid.SetRow(lblModel, 9);
                Grid.SetColumn(lblModel, 1);
                MainGrid.Children.Add(lblModel);

                MainGrid.Children.Remove(txtModel);
                Grid.SetRow(txtModel, 9);
                Grid.SetColumn(txtModel, 3);
                MainGrid.Children.Add(txtModel);

                MainGrid.Children.Remove(lblYear);
                Grid.SetRow(lblYear, 11);
                Grid.SetColumn(lblYear, 1);
                MainGrid.Children.Add(lblYear);

                MainGrid.Children.Remove(txtYear);
                Grid.SetRow(txtYear, 11);
                Grid.SetColumn(txtYear, 3);
                MainGrid.Children.Add(txtYear);

                MainGrid.Children.Remove(VehOVRR);
                Grid.SetRow(VehOVRR, 13);
                Grid.SetColumn(VehOVRR, 1);
                Grid.SetColumnSpan(VehOVRR, 3);
                MainGrid.Children.Add(VehOVRR);

                MainGrid.Children.Remove(stckPanelVRR);
                Grid.SetRow(stckPanelVRR, 13);
                Grid.SetColumn(stckPanelVRR, 3);
                MainGrid.Children.Add(stckPanelVRR);
                //   Grid.Column="1" Grid.Row="11" Grid.ColumnSpan="4"

                MainGrid.Children.Remove(lblRecmnd);
                Grid.SetRow(lblRecmnd, 15);
                Grid.SetColumn(lblRecmnd, 1);
                Grid.SetColumnSpan(lblRecmnd, 3);
                MainGrid.Children.Add(lblRecmnd);

               



                MainGrid.Children.Remove(btnStackePanel);
                Grid.SetRow(btnStackePanel, 17);
                Grid.SetColumn(btnStackePanel, 3);
                MainGrid.Children.Add(btnStackePanel);


                MainGrid.Children.Remove(lblOpnVio);
                Grid.SetRow(lblOpnVio, 19);
                Grid.SetColumn(lblOpnVio, 1);
                MainGrid.Children.Add(lblOpnVio);

                MainGrid.Children.Remove(grdViolationDetails);
                Grid.SetRow(grdViolationDetails, 21);
                Grid.SetColumn(grdViolationDetails, 1);
                Grid.SetColumnSpan(grdViolationDetails, 5);
                MainGrid.Children.Add(grdViolationDetails);




            }


        }
        public void ChangeControlDimensions(double width)
        {
            this.txtBoxOperatorName.Width = width;
            this.txtBoxChassisNumber.Width = width;
            this.txtBoxMake.Width = width;
            this.txtModel.Width = width;
            this.txtYear.Width = width;
            this.stckPanelVRR.Width = width;
            


            // this.txtYear.Width = width;

            this.UpdateLayout();
        }
        public void ChangeButtonsDimensions(double width)
        {
            this.btnNext.Width = width;
            this.btnBack.Width = width;
            this.btnStartInspection.Width = width;

            this.UpdateLayout();
        }
        public void ChangeButtonsDimensions2(double width)
        {
            // this.btnAddImage.Width = width;
        }
        public void ChangeLableDimensions(double width)
        {
            this.lblOprName.FontSize = width;
            this.lblChassisName.FontSize = width;
            this.lblMake.FontSize = width;
            this.lblModel.FontSize = width;
            this.lblYear.FontSize = width;

            this.VehOVRR.FontSize = width;
            this.lblRecmnd.FontSize = width;
            this.lblOpnVio.FontSize = width;
            // this.lblYear.FontSize = width;
            // this.lblAppLogout.FontSize = 20;
            this.UpdateLayout();
        }
        public void ChangeHeaderDimensions(double width)
        {
            this.lblVehDetail.FontSize = width;

            this.UpdateLayout();
        }

        private void btnPrintRecipt_Click_1(object sender, RoutedEventArgs e)
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


            this.m_mainWindow.MainContentControl.Content = null;
            this.m_mainWindow.MainContentControl.Content = new ucPrintSearchedViolationTicket(this.m_mainWindow, vioaltion_ticket_data, false, this);
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
        }

        private void txtRecomendations_PreviewKeyDown_1(object sender, KeyEventArgs e)
        {

            try
            {
                if (e.Key == Key.Return)
                {
                    btnStartInspection_Click_1(sender, e);

                }
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


    }
}