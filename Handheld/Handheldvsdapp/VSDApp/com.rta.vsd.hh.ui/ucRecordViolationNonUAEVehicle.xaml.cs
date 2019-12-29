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
using VSDApp.com.rta.vsd.hh.manager;
using VSDApp.com.rta.vsd.hh.utilities;
using VSDApp.com.rta.vsd.validation;
using VSDApp.WPFMessageBoxControl;

namespace VSDApp.com.rta.vsd.hh.ui
{
    /// <summary>
    /// Interaction logic for ucRecordViolationNonUAEVehicle.xaml
    /// </summary>
    public partial class ucRecordViolationNonUAEVehicle : UserControl
    {
        MainWindow m_MainWindow = null;
        private IValidation _iValidate;
        private string _validationResult;
        private ucVehicleSelection vehicleSelection;
        public bool _isUAEVehicle;
        public ucRecordViolationNonUAEVehicle(MainWindow mainWnd, ucVehicleSelection ucVehicleSeletion)
        {
            InitializeComponent();
            this.m_MainWindow = mainWnd;
            vehicleSelection = ucVehicleSeletion;
        }
        private void btnBack_Click_2(object sender, RoutedEventArgs e)
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
            // this.m_MainWindow.MainContentControl.Content = new ucLocationSelectionEn(m_MainWindow);
            this.m_MainWindow.MainContentControl.Content = new ucLocationSelectionEn(m_MainWindow);
        }

        private void btnStartInspection_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (AppProperties.Selected_Resource == "Arabic")
                {
                    imagebtnNext.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Start Inspection Arabic Up.png", UriKind.Relative));
                    _iValidate = (IValidation)new RecordViolationNonDubaiArValidation();
                }
                else
                {
                    imagebtnNext.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Start Inspection.png", UriKind.Relative));
                    _iValidate = (IValidation)new RecordViolationNonDubaiEnValidation();
                }

                // this.txtBoxOperatorName.Text = Regex.Replace(this.txtBoxOperatorName.Text, "[^a-zA-Z]+", "", RegexOptions.Compiled);
                this.txtBoxOperatorName.Text = this.txtBoxOperatorName.Text;
                if (_isUAEVehicle)
                    _validationResult = _iValidate.Validate(this);
                else
                {
                     if(vehicleSelection!=null)
                         if (vehicleSelection.txtBoxPlateNumber.Text.Trim().Equals(""))
                         {
                             if(AppProperties.Selected_Resource.ToString().Equals("English"))
                                WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), "Plate number not entered");
                             else
                                WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), "يرجى إدخال رقم اللوحة");
                             return;
                         }
                    
                        _validationResult = "Valid";
                }

                if (_validationResult != "Valid")
                {
                    WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), _validationResult);
                    return;
                }
                else
                {

                    //////////////////
                    if (AppProperties.vehicle == null)
                    {
                        AppProperties.vehicle = new Vehicle();
                        if (AppProperties.Selected_Resource == "Arabic")
                        {
                            AppProperties.vehicle.Country = (string)vehicleSelection.countryTable[vehicleSelection.cmboxCountry.SelectedValue.ToString()];
                            if (null == (string)vehicleSelection.cmboxEmirates.SelectedItem)
                            {
                                AppProperties.vehicle.Emirate = "";
                            }
                            else
                            {
                                AppProperties.vehicle.Emirate = (string)vehicleSelection.emirateTable[vehicleSelection.cmboxEmirates.SelectedValue.ToString()];
                            }
                            if (vehicleSelection.cmboxPlateCode.Visibility == System.Windows.Visibility.Collapsed)
                            {
                                AppProperties.vehicle.PlateCode = vehicleSelection.txtPlateCode.Text;
                            }
                            else
                            {
                                AppProperties.vehicle.PlateCode = (string)vehicleSelection.plateCatTable[vehicleSelection.cmboxPlateCategory.SelectedValue.ToString()];
                            }
                            if (null == (string)vehicleSelection.cmboxPlateCategory.SelectedItem)
                            {
                                AppProperties.vehicle.PlateCategory = "";
                            }
                            else
                            {
                                AppProperties.vehicle.PlateCategory = (string)vehicleSelection.plateCatTable[vehicleSelection.cmboxPlateCategory.SelectedValue.ToString()];
                            }
                            //   vehicleSelection.cmboxCountry.SelectedValue.ToString();
                        }
                        else
                        {
                            AppProperties.vehicle.Country = vehicleSelection.cmboxCountry.SelectedValue.ToString();
                            if (null == (string)vehicleSelection.cmboxEmirates.SelectedItem)
                            {
                                AppProperties.vehicle.Emirate = "";
                            }
                            else
                            {
                                AppProperties.vehicle.Emirate = vehicleSelection.cmboxEmirates.SelectedValue.ToString();
                            }
                            if (vehicleSelection.cmboxPlateCode.Visibility == System.Windows.Visibility.Collapsed)
                            {
                                AppProperties.vehicle.PlateCode = vehicleSelection.txtPlateCode.Text;
                            }
                            else
                            {
                                AppProperties.vehicle.PlateCode = vehicleSelection.cmboxPlateCode.SelectedValue.ToString();
                            }
                            if (null == (string)vehicleSelection.cmboxPlateCategory.SelectedItem)
                            {
                                AppProperties.vehicle.PlateCategory = "";
                            }
                            else
                            {
                                AppProperties.vehicle.PlateCategory = vehicleSelection.cmboxPlateCategory.SelectedValue.ToString();
                            }
                        }




                        AppProperties.vehicle.PlateNumber = vehicleSelection.txtBoxPlateNumber.Text;


                        string vehicleCatAr = string.Empty;
                        if (AppProperties.Selected_Resource == "Arabic")
                        {
                            vehicleCatAr = ((IVehicleProfile)VehicleProfileManager.GetInstance()).GetAlternateCategory((string)vehicleSelection.vehicleCatTable[vehicleSelection.cmboxVehicleCategoty.Text], "ar-AE");
                            AppProperties.vehicle.VehicleCategory = (string)vehicleSelection.vehicleCatTable[vehicleSelection.cmboxVehicleCategoty.Text];
                        }

                        else
                        {
                            //    vehicleCatAr = vehicleCatAr;
                            //});
                            vehicleCatAr = ((IVehicleProfile)VehicleProfileManager.GetInstance()).GetAlternateCategory(vehicleSelection.cmboxVehicleCategoty.SelectedValue.ToString(), "en-US");
                            AppProperties.vehicle.VehicleCategory = vehicleSelection.cmboxVehicleCategoty.SelectedValue.ToString();
                        }


                        // string vehicleCatAr = ((IVehicleProfile)VehicleProfileManager.GetInstance()).GetAlternateCategory(vehicleSelection.cmboxVehicleCategoty.SelectedValue.ToString(), "en-US");
                        AppProperties.vehicle.VehicleCategoryAr = vehicleCatAr;
                    }





                    ////////////////////
                    if (AppProperties.vehicle.Operator == null)
                    {
                        AppProperties.vehicle.Operator = new Operator();
                    }
                    AppProperties.vehicle.Operator.OperatorName = txtBoxOperatorName.Text;
                    AppProperties.vehicle.ChassisNumber = txtBoxChassisNumber.Text;
                    AppProperties.vehicle.Make = txtBoxMake.Text;
                    AppProperties.vehicle.Model = txtModel.Text;
                    AppProperties.vehicle.Year = txtYear.Text;

                    //}
                    //Add Defect Scree
                    // RecordViolationDefectScreenEn RVDefect = new RecordViolationDefectScreenEn();

                    this.m_MainWindow.MainContentControl.Content = null;
                    this.m_MainWindow.MainContentControl.Content = new ucDefectAndViolationDetails(this.m_MainWindow);
                    return;
                }
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);

            }

        }
        public void PopulateVehicleSearchRecords()
        {
            txtBoxChassisNumber.Text = (AppProperties.vehicle == null ? "" : AppProperties.vehicle.ChassisNumber);
            txtBoxMake.Text = (AppProperties.vehicle == null ? "" : AppProperties.vehicle.Make);
            txtModel.Text = (AppProperties.vehicle == null ? "" : AppProperties.vehicle.Model);
            txtBoxOperatorName.Text = (AppProperties.vehicle == null ? "" : (AppProperties.vehicle.Operator == null ? "" : AppProperties.vehicle.Operator.OperatorName));
            txtYear.Text = (AppProperties.vehicle == null ? "" : AppProperties.vehicle.Year);

        }
        public void IsEnabledFileds(bool IsEnabled, bool isUAEVehicle)
        {
            //IsReadonly= true mean IsEnaled=false and vice versa
            bool IsReadOnly = !IsEnabled;
            txtBoxChassisNumber.IsReadOnly = IsReadOnly;
            txtBoxMake.IsReadOnly = IsReadOnly;
            txtBoxOperatorName.IsReadOnly = IsReadOnly;
            txtModel.IsReadOnly = IsReadOnly;
            txtYear.IsReadOnly = IsReadOnly;

            imagebtnNext.IsEnabled = !IsEnabled;
            _isUAEVehicle = isUAEVehicle;

            if (IsEnabled==false)
            {
                txtBoxChassisNumber.Background = Brushes.Gray;
                txtBoxMake.Background = Brushes.Gray;
                txtBoxOperatorName.Background = Brushes.Gray;
                txtModel.Background = Brushes.Gray;
                txtYear.Background = Brushes.Gray;
               
            }
        }

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            if (AppProperties.Selected_Resource == "Arabic")
            {
                btnStackePanel.FlowDirection = System.Windows.FlowDirection.LeftToRight;
            }
            ClearVehicleResultFields();
            // PopulateVehicleSearchRecords();
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
          //  this.txtBoxOperatorName.Focus();
        }
        public void ClearVehicleResultFields()
        {
            txtBoxChassisNumber.Text = "";
            txtBoxMake.Text = "";
            txtBoxOperatorName.Text = "";
            txtModel.Text = "";
            txtYear.Text = "";
        }

        private void txtBoxOperatorName_GotFocus_1(object sender, RoutedEventArgs e)
        {
            CommonUtils.ShowKeyBoard();
        }

        private void txtBoxOperatorName_LostFocus_1(object sender, RoutedEventArgs e)
        {
            CommonUtils.CLoseKeyBoard();
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

        private void txtYear_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            try
            {
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
            ChangeControlPosition();
            if (new CommonUtils().GetScreenOrientation() == AppProperties.LandScapetMode)
            {
                
                ChangeControlDimensions(AppProperties.LndScp_130_280);
                ChangeButtonsDimensions(135);
                
            }
            else
            {
                
                ChangeControlDimensions(280);
                ChangeButtonsDimensions(135);
                
            }
        }
        public void ChangeControlPosition()
        {
            // MessageBox.Show("Orientation Changed");
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

                grdVehSerch.Children.Remove(btnStackePanel);
                Grid.SetRow(btnStackePanel, 5);
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

                grdVehSerch.Children.Remove(btnStackePanel);
                Grid.SetRow(btnStackePanel, 11);
                Grid.SetColumn(btnStackePanel, 3);
                grdVehSerch.Children.Add(btnStackePanel);
            }
        }
        public void ChangeControlDimensions(double width)
        {
            this.txtBoxOperatorName.Width = width;
            this.txtBoxChassisNumber.Width = width;
            this.txtModel.Width = width;
            this.txtBoxMake.Width = width;
            this.txtYear.Width = width;

            this.UpdateLayout();
        }
        public void ChangeButtonsDimensions(double width)
        {
            //this.imagebtnback.Width = width;
            //this.imagebtnNext.Width = width;

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
            this.lblYear.FontSize = width;
            // this.lblAppLogout.FontSize = 20;
            this.UpdateLayout();
        }
        public void ChangeHeaderDimensions(double width)
        {
            this.UpdateLayout();
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
            System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.DigitSubstitution = System.Globalization.DigitShapes.None;
            this.UpdateLayout();
        }


    }
}
