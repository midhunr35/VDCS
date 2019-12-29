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
using VSDApp.com.rta.vsd.hh.db;
using VSDApp.com.rta.vsd.hh.utilities;
using VSDApp.com.rta.vsd.validation;
using VSDApp.WPFMessageBoxControl;

namespace VSDApp.com.rta.vsd.hh.ui
{
    /// <summary>
    /// Interaction logic for ucVehicleProfileInspection.xaml
    /// </summary>
    public partial class ucVehicleProfileInspection : UserControl
    {
        MainWindow m_MainWindow;
        List<vsd.hh.data.Violation> openViolations = null;
        private IValidation     _iValidate;
        private string _validationResult;
        public ucVehicleProfileInspection(MainWindow mainWnd)
        {
            InitializeComponent();
            this.m_MainWindow = mainWnd;
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
                    //txtBoxOperatorName.Text = (AppProperties.vehicle == null ? "" : (AppProperties.vehicle.Operator == null ? "" : (AppProperties.vehicle.Operator.OperatorName==null ? AppProperties.vehicle.Operator.TrafficFileNumber)));
                    // btnStartInspection.Content = (AppProperties.vehicle.Recomendation != "") ? "Confiscate" : "Inspect";
                    //  txtYear.Text = (AppProperties.vehicle == null ? "" : AppProperties.vehicle.Year);
                    PopulateOwnerVehicleInfo();
                    ShowVehicleRiskRatting();
                    PoplateVehicleRecomendation();
                }
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }
        //

        // To Show Vehicle Risk Ratting


        #region Public Function to Populate Data
        /// <summary>
        /// Show Vehicle Risk Ratting with Coloring scheme
        /// </summary>
        private void ShowVehicleRiskRatting()
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
                    lblVRR.Visibility = System.Windows.Visibility.Visible;
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
                    txtRecomendation.Text = new CommonUtils().GetStringValue("InspectionRecomended");
                }
                else
                {
                    txtRecomendation.Text = new CommonUtils().GetStringValue("InspectionNotRecomended");
                }
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }


        private void PopulateOwnerVehicleInfo()
        {
            try
            {
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

                grdOpenViolations.ItemsSource = openViolations;
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }



        }
        
        public void ChangeControlPosition()
        {
            if (new CommonUtils().GetScreenOrientation() == AppProperties.LandScapetMode)
            {
                grdVehSerch.Children.Remove(lblOprName);
                Grid.SetRow(lblOprName, 1);
                Grid.SetColumn(lblOprName, 1);
                grdVehSerch.Children.Add(lblOprName);

                grdVehSerch.Children.Remove(txtBoxOperatorName);
                Grid.SetRow(txtBoxOperatorName, 1);
                Grid.SetColumn(txtBoxOperatorName, 3);
                grdVehSerch.Children.Add(txtBoxOperatorName);

                grdVehSerch.Children.Remove(lblChassisNo);
                Grid.SetRow(lblChassisNo, 1);
                Grid.SetColumn(lblChassisNo, 5);
                grdVehSerch.Children.Add(lblChassisNo);

                grdVehSerch.Children.Remove(txtBoxChassisNumber);
                Grid.SetRow(txtBoxChassisNumber, 1);
                Grid.SetColumn(txtBoxChassisNumber, 7);
                grdVehSerch.Children.Add(txtBoxChassisNumber);

                grdVehSerch.Children.Remove(lblMake);
                Grid.SetRow(lblMake, 3);
                Grid.SetColumn(lblMake, 1);
                grdVehSerch.Children.Add(lblMake);

                grdVehSerch.Children.Remove(txtBoxMake);
                Grid.SetRow(txtBoxMake, 3);
                Grid.SetColumn(txtBoxMake, 3);
                grdVehSerch.Children.Add(txtBoxMake);

                grdVehSerch.Children.Remove(lblModel);
                Grid.SetRow(lblModel, 3);
                Grid.SetColumn(lblModel, 5);
                grdVehSerch.Children.Add(lblModel);

                grdVehSerch.Children.Remove(txtModel);
                Grid.SetRow(txtModel, 3);
                Grid.SetColumn(txtModel, 7);
                grdVehSerch.Children.Add(txtModel);

                grdVehSerch.Children.Remove(lblYear);
                Grid.SetRow(lblYear, 5);
                Grid.SetColumn(lblYear, 1);
                grdVehSerch.Children.Add(lblYear);

                grdVehSerch.Children.Remove(txtYear);
                Grid.SetRow(txtYear, 5);
                Grid.SetColumn(txtYear, 3);
                grdVehSerch.Children.Add(txtYear);


                grdVehSerch.Children.Remove(lblVRR);
                Grid.SetRow(lblVRR, 5);
                Grid.SetColumn(lblVRR, 5);
                grdVehSerch.Children.Add(lblVRR);

                grdVehSerch.Children.Remove(grdcanvas);
                Grid.SetRow(grdcanvas, 5);
                Grid.SetColumn(grdcanvas, 7);
                grdVehSerch.Children.Add(grdcanvas);



                grdVehSerch.Children.Remove(lblRecomendation);
                Grid.SetRow(lblRecomendation, 7);
                Grid.SetColumn(lblRecomendation, 1);
                grdVehSerch.Children.Add(lblRecomendation);

                grdVehSerch.Children.Remove(txtRecomendation);
                Grid.SetRow(txtRecomendation, 7);
                Grid.SetColumn(txtRecomendation, 3);
                grdVehSerch.Children.Add(txtRecomendation);


                grdVehSerch.Children.Remove(btnStackePanel);
                Grid.SetRow(btnStackePanel, 7);
                Grid.SetColumn(btnStackePanel, 7);
                grdVehSerch.Children.Add(btnStackePanel);



            }
            else
            {
                grdVehSerch.Children.Remove(lblOprName);
                Grid.SetRow(lblOprName, 1);
                Grid.SetColumn(lblOprName, 1);
                grdVehSerch.Children.Add(lblOprName);

                grdVehSerch.Children.Remove(txtBoxOperatorName);
                Grid.SetRow(txtBoxOperatorName, 1);
                Grid.SetColumn(txtBoxOperatorName, 3);
                grdVehSerch.Children.Add(txtBoxOperatorName);

                grdVehSerch.Children.Remove(lblChassisNo);
                Grid.SetRow(lblChassisNo, 3);
                Grid.SetColumn(lblChassisNo, 1);
                grdVehSerch.Children.Add(lblChassisNo);

                grdVehSerch.Children.Remove(txtBoxChassisNumber);
                Grid.SetRow(txtBoxChassisNumber, 3);
                Grid.SetColumn(txtBoxChassisNumber, 3);
                grdVehSerch.Children.Add(txtBoxChassisNumber);

                grdVehSerch.Children.Remove(lblMake);
                Grid.SetRow(lblMake, 5);
                Grid.SetColumn(lblMake, 1);
                grdVehSerch.Children.Add(lblMake);

                grdVehSerch.Children.Remove(txtBoxMake);
                Grid.SetRow(txtBoxMake, 5);
                Grid.SetColumn(txtBoxMake, 3);
                grdVehSerch.Children.Add(txtBoxMake);

                grdVehSerch.Children.Remove(lblModel);
                Grid.SetRow(lblModel, 7);
                Grid.SetColumn(lblModel, 1);
                grdVehSerch.Children.Add(lblModel);

                grdVehSerch.Children.Remove(txtModel);
                Grid.SetRow(txtModel, 7);
                Grid.SetColumn(txtModel, 3);
                grdVehSerch.Children.Add(txtModel);

                grdVehSerch.Children.Remove(lblYear);
                Grid.SetRow(lblYear, 9);
                Grid.SetColumn(lblYear, 1);
                grdVehSerch.Children.Add(lblYear);

                grdVehSerch.Children.Remove(txtYear);
                Grid.SetRow(txtYear, 9);
                Grid.SetColumn(txtYear, 3);
                grdVehSerch.Children.Add(txtYear);


                grdVehSerch.Children.Remove(lblRecomendation);
                Grid.SetRow(lblRecomendation, 11);
                Grid.SetColumn(lblRecomendation, 1);
                grdVehSerch.Children.Add(lblRecomendation);

                grdVehSerch.Children.Remove(txtRecomendation);
                Grid.SetRow(txtRecomendation, 11);
                Grid.SetColumn(txtRecomendation, 3);
                grdVehSerch.Children.Add(txtRecomendation);


                grdVehSerch.Children.Remove(lblVRR);
                Grid.SetRow(lblVRR, 13);
                Grid.SetColumn(lblVRR, 1);
                grdVehSerch.Children.Add(lblVRR);

                grdVehSerch.Children.Remove(grdcanvas);
                Grid.SetRow(grdcanvas, 13);
                Grid.SetColumn(grdcanvas, 3);
                grdVehSerch.Children.Add(grdcanvas);


                grdVehSerch.Children.Remove(btnStackePanel);
                Grid.SetRow(btnStackePanel, 15);
                Grid.SetColumn(btnStackePanel, 3);
                grdVehSerch.Children.Add(btnStackePanel);
            }
        }
        public void ChangeControlDimensions(double width)
        {
            this.txtBoxOperatorName.Width = width;
            this.txtBoxChassisNumber.Width = width;
            this.txtModel.Width = width;
            this.txtYear.Width = width;
            this.txtBoxMake.Width = width;
            this.txtRecomendation.Width = width;
            // this.txtYear.Width = width;

            this.UpdateLayout();
        }
        public void ChangeSmallButtonDimensions(double width)
        {

        }
        public void ChangeButtonsDimensions(double width)
        {
            this.imagebtnback.Width = width;
            this.imagebtnNext.Width = width;
           this.btnStartInspection.Width = width;
           this.btnBack.Width = width;

            this.UpdateLayout();
        }
        public void ChangeLableDimensions(double width)
        {
            this.lblOprName.FontSize = width;
            this.lblChassisNo.FontSize = width;
            this.lblMake.FontSize = width;
            this.lblModel.FontSize = width;
            this.lblVRR.FontSize = width;
            this.lblRecomendation.FontSize = width;
            // this.lblYear.FontSize = width;
            // this.lblAppLogout.FontSize = 20;
            this.UpdateLayout();
        }
        public void ChangeHeaderDimensions(double width)
        {
            this.UpdateLayout();
        }
        public void ClearDataFields()
        {
            this.txtRecomendation.Text = "";
            // this.txtOvrrScore.Text = "";
            this.txtModel.Text = "";
            this.txtYear.Text = "";

            this.txtBoxOperatorName.Text = "";
            this.txtBoxMake.Text = "";
            this.txtBoxChassisNumber.Text = "";
            if (openViolations != null)
                openViolations.Clear();

        }

        #endregion

        #region Events
        private void btnStartInspection_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (AppProperties.Selected_Resource == "Arabic")
                {
                    imagebtnNext.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Start Inspection Arabic Up.png", UriKind.Relative));
                    _iValidate = (IValidation)new VehicleProfileInspectionArValidation();
                }
                else
                {
                    imagebtnNext.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Start Inspection.png", UriKind.Relative));
                    _iValidate = (IValidation)new VehicleProfileInspectionEnValidation();
                }


                _validationResult = _iValidate.Validate(this);
                if (_validationResult != "Valid")
                {
                    WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), _validationResult);
                }
                else
                {
                    this.m_MainWindow.MainContentControl.Content = null;
                    this.m_MainWindow.MainContentControl.Content = new ucDefectAndViolationDetails(this.m_MainWindow);
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
            this.m_MainWindow.MainContentControl.Content = null;
            //this.m_MainWindow.MainContentControl.Content = new ucLocationSelectionEn(m_MainWindow);
            this.m_MainWindow.MainContentControl.Content = new ucLocationSelectionEn(m_MainWindow);
        }

        private void txtBoxOperatorName_GotFocus_1(object sender, RoutedEventArgs e)
        {
            CommonUtils.ShowKeyBoard();
        }

        private void txtBoxOperatorName_LostFocus_1(object sender, RoutedEventArgs e)
        {
            CommonUtils.CLoseKeyBoard();
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

        private void txtBoxOperatorName_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            try
            {
                new CommonUtils().validateTextCharacter(sender, e);
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
        }

        private void txtYear_PreviewKeyDown_1(object sender, KeyEventArgs e)
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


        }
        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            if (AppProperties.Selected_Resource == "Arabic")
            {
                btnStackePanel.FlowDirection = System.Windows.FlowDirection.LeftToRight;
            }
            ClearDataFields();
            PopulateData();
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
            // this.txtBoxOperatorName.Focus();

        }




        private void UserControl_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {
            ChangeControlPosition();
            if (new CommonUtils().GetScreenOrientation() == AppProperties.LandScapetMode)
            {
                
                ChangeControlDimensions(AppProperties.LndScp_130_280);
                ChangeButtonsDimensions(AppProperties.LndScp_Btn_65_130);
                
            }
            else
            {
               
              ChangeControlDimensions(280);
              ChangeButtonsDimensions(130);
                
            }
            // Creating a FocusNavigationDirection object and setting it to a
            // local field that contains the direction selected.
            FocusNavigationDirection focusDirection = FocusNavigationDirection.First;

            // MoveFocus takes a TraveralReqest as its argument.
            TraversalRequest request = new TraversalRequest(focusDirection);

            // Gets the element with keyboard focus.
            UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;

            // Change keyboard focus.
            if (elementWithFocus != null)
            {
                if (elementWithFocus.MoveFocus(request)) e.Handled = true;
            }
        }
        #endregion
       
       
        

       
        

       


    }
}
