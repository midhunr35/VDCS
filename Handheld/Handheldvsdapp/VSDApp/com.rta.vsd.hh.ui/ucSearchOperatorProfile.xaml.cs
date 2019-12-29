using System;
using System.Collections.Generic;
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
    /// Interaction logic for ucSearchOperatorProfile.xaml
    /// </summary>
    public partial class ucSearchOperatorProfile : UserControl
    {
        MainWindow m_mainWindow;
        private IValidation _iValidate;
        private string _validationResult;
        List<DisplayObject> operatorData;
        public ucSearchOperatorProfile(MainWindow mainWindow)
        {
            InitializeComponent();
            this.m_mainWindow = mainWindow;
        }

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            if (AppProperties.Selected_Resource == "English")
            {
                imagebtnBack.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Back.png", UriKind.Relative));
                imagebtnSearch.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Search.png", UriKind.Relative));
                imagebtnResetVehicleRecord.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Reset.png", UriKind.Relative));
            }
            else
            {
                imagebtnBack.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/back Arabic Up.png", UriKind.Relative));
                imagebtnSearch.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Search Arabic Up.png", UriKind.Relative));
                imagebtnResetVehicleRecord.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Reset Arabic Up.png", UriKind.Relative));
            }
            if (AppProperties.Selected_Resource == "Arabic")
            {
                btnStackePanel.FlowDirection = System.Windows.FlowDirection.LeftToRight;
            }

            PopulateData();
            ClearFields();

            if (AppProperties.Selected_Resource == "Arabic")
            {
                m_mainWindow.txtlocation.Visibility = System.Windows.Visibility.Visible;

                m_mainWindow.txtlocation.Text = AppProperties.Current_Selected_LocationAr;
                //  AppProperties.location.location = (string)locationTable[(string)cmboxLocation.SelectedItem];
                // AppProperties.recordedViolation.InspectionArea = AppProperties.location;
            }
            else
            {
                // AppProperties.location.city = ((string)((ucLocationSelectionEn)this).cmboxEmirates.Text).Trim();
                //AppProperties.location.area = ((string)((ucLocationSelectionEn)this).cmboxArea.Text).Trim();
                m_mainWindow.txtlocation.Visibility = System.Windows.Visibility.Visible;

                m_mainWindow.txtlocation.Text = AppProperties.Current_Selected_LocationEn;
            }
            //    imgOVRR1.Source = new BitmapImage(new Uri(@"/Images/OVRR1.png", UriKind.Relative));
            //   imgOVRR2.Source = new BitmapImage(new Uri(@"/Images/OVRR1.png", UriKind.Relative));
            //  imgOVRR3.Source = new BitmapImage(new Uri(@"/Images/OVRR1.png", UriKind.Relative));
        }

        public void PopulateData()
        {
            txtOperatorName.Text = (AppProperties.vehicle == null ? "" : (AppProperties.vehicle.Operator == null ? "" : AppProperties.vehicle.Operator.OperatorName));
            txtTraffice.Text = (AppProperties.vehicle == null ? "" : (AppProperties.vehicle.Operator == null ? "" : AppProperties.vehicle.Operator.TrafficFileNumber));

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

            this.m_mainWindow.MainContentControl.Content = null;
            //this.m_mainWindow.MainContentControl.Content = new ucLocationSelectionEn(m_mainWindow);
            this.m_mainWindow.MainContentControl.Content = new ucWellComeScreen(m_mainWindow);
        }

        public void SearchOperatorArHandler()
        {
            try
            {
                _iValidate = (IValidation)new OperatorProfileInputArValidation();
                _validationResult = _iValidate.Validate(this);

                if (_validationResult != "Valid")
                {
                    WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), _validationResult);
                    return;
                }
                else
                {
                    string companyName = txtOperatorName.Text;
                    string trafficFileNo = txtTraffice.Text;

                    IOperatorProfile iOperatorProfile = (IOperatorProfile)OperatorProfileManager.GetInstance();
                    if (AppProperties.vehicle == null)
                        AppProperties.vehicle = new Vehicle();
                    //  ProgressDialogResult result = ProgressDialog.ProgressDialog.Execute(this.m_mainWindow, "...جاري البحث عن بيانات المشغل", (bw, we) =>
                    // {
                    //Do Work
                    //    AppProperties.vehicle.Operator = iOperatorProfile.GetOperator(companyName, trafficFileNo);
                    // });

                    ProgressDialogResult result = ProgressDialog.ProgressDialog.Execute(this.m_mainWindow, new CommonUtils().GetStringValue("lblSearchOperator"), (bw, we) =>
                    {

                        AppProperties.vehicle.Operator = iOperatorProfile.GetOperator(companyName, trafficFileNo);

                        // So this check in order to avoid default processing after the Cancel button has been pressed.
                        // This call will set the Cancelled flag on the result structure.
                        ProgressDialog.ProgressDialog.CheckForPendingCancellation(bw, we);

                    }, ProgressDialogSettings.WithSubLabelAndCancel);

                    if (result == null || result.Cancelled)
                        return;
                    else if (result.OperationFailed)
                        return;


                    if (AppProperties.businessError)
                    {
                        AppProperties.vehicle = null;
                        AppProperties.recordedViolation = null;
                        AppProperties.recordedViolation = new Violation();
                        AppProperties.recordedViolation.InspectionArea = AppProperties.location;

                        WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("ErrorException"));
                        AppProperties.businessError = false;
                        ClearFields();
                        // this.m_mainWindow.MainContentControl.Content = null;
                        // this.m_mainWindow.MainContentControl.Content = new ucLocationSelectionEn(m_mainWindow);
                        // LandingScreenEn landing = new LandingScreenEn();
                        //_render.switchDisplay(form, landing);
                        return;
                    }
                    if (AppProperties.IsException)
                    {
                        AppProperties.IsException = false;
                        WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("ErrorException"));
                        AppProperties.vehicle = null;
                        ClearFields();
                        return;
                    }
                    if (AppProperties.NotFoundError)
                    {
                        AppProperties.NotFoundError = false;
                        WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("ErrorNotFound"));
                        AppProperties.vehicle = null;
                        ClearFields();
                        return;
                    }
                    if (AppProperties.vehicle.Operator != null)
                    {
                        if (!"".Equals(companyName))
                        {
                            if (!companyName.Equals(AppProperties.vehicle.Operator.OperatorName))
                            {
                                WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), "لم يتم العثور على بيانات المركبة");
                                return;
                            }
                        }
                        PopulateOperatorSearchedResult();
                        // OperatorProfileDetailScreenEn opDetail = new OperatorProfileDetailScreenEn();
                        // _render.switchDisplay(form, opDetail);
                        return;
                    }
                    else
                    {
                        WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), "لم يتم العثور على بيانات المركبة");
                        // LandingScreenEn landing = new LandingScreenEn();
                        // _render.switchDisplay(form, landing);
                        //   this.m_mainWindow.MainContentControl.Content = null;
                        // this.m_mainWindow.MainContentControl.Content = new ucLocationSelectionEn(m_mainWindow);
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
        public void SearchOperatorEnHandler()
        {
            try
            {
                _iValidate = (IValidation)new OperatorProfileInputEnValidation();
                _validationResult = _iValidate.Validate(this);

                if (_validationResult != "Valid")
                {
                    WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), _validationResult);
                }
                else
                {
                    string companyName = txtOperatorName.Text;
                    string trafficFileNo = txtTraffice.Text;

                    IOperatorProfile iOperatorProfile = (IOperatorProfile)OperatorProfileManager.GetInstance();
                    if (AppProperties.vehicle == null)
                        AppProperties.vehicle = new Vehicle();
                    //   ProgressDialogResult result = ProgressDialog.ProgressDialog.Execute(this.m_mainWindow, "Searching Operator Profile...", (bw, we) =>
                    //   {
                    //Do Work
                    //       AppProperties.vehicle.Operator = iOperatorProfile.GetOperator(companyName, trafficFileNo);
                    //   });

                    ProgressDialogResult result = ProgressDialog.ProgressDialog.Execute(this.m_mainWindow, new CommonUtils().GetStringValue("lblSearchOperator"), (bw, we) =>
                    {

                        AppProperties.vehicle.Operator = iOperatorProfile.GetOperator(companyName, trafficFileNo);

                        // So this check in order to avoid default processing after the Cancel button has been pressed.
                        // This call will set the Cancelled flag on the result structure.
                        ProgressDialog.ProgressDialog.CheckForPendingCancellation(bw, we);

                    }, ProgressDialogSettings.WithSubLabelAndCancel);

                    if (result == null || result.Cancelled)
                        return;
                    else if (result.OperationFailed)
                        return;

                    if (AppProperties.businessError)
                    {
                        AppProperties.vehicle = null;
                        AppProperties.recordedViolation = null;
                        AppProperties.recordedViolation = new Violation();
                        AppProperties.recordedViolation.InspectionArea = AppProperties.location;
                        WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("ErrorBusiness"));
                        AppProperties.businessError = false;
                        ClearFields();
                        // this.m_mainWindow.MainContentControl.Content = null;
                        // this.m_mainWindow.MainContentControl.Content = new ucLocationSelectionEn(this.m_mainWindow);
                        // LandingScreenEn landing = new LandingScreenEn();
                        //_render.switchDisplay(form, landing);
                        return;
                    }
                    if (AppProperties.IsException)
                    {
                        AppProperties.IsException = false;
                        WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("ErrorException"));
                        AppProperties.vehicle = null;
                        ClearFields();
                        return;
                    }
                    if (AppProperties.NotFoundError)
                    {
                        AppProperties.NotFoundError = false;
                        WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("ErrorNotFound"));
                        AppProperties.vehicle = null;
                        ClearFields();
                        return;
                    }

                    else if (AppProperties.vehicle.Operator != null)
                    {
                        if (!"".Equals(companyName))
                        {
                            if (!companyName.Equals(AppProperties.vehicle.Operator.OperatorName))
                            {
                                WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), "Operator not found");
                                return;
                            }
                        }

                        PopulateOperatorSearchedResult();
                        // OperatorProfileDetailScreenEn opDetail = new OperatorProfileDetailScreenEn();
                        // _render.switchDisplay(form, opDetail);
                        return;
                    }
                    else
                    {
                        WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), "Operator not found");
                        ClearFields();
                        // LandingScreenEn landing = new LandingScreenEn();
                        // _render.switchDisplay(form, landing);
                        // this.m_mainWindow.MainContentControl.Content = null;
                        //this.m_mainWindow.MainContentControl.Content = new ucLocationSelectionEn(this.m_mainWindow);
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
                SearchOperatorEnHandler();
            }
            else
            {
                imagebtnSearch.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Search Arabic Up.png", UriKind.Relative));
                SearchOperatorArHandler();
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
            ClearFields();
           

        }
        public void ClearFields()
        {
            this.txtOperatorName.Text = "";
            this.txtOVVRScore.Text = "";
            this.txtTraffice.Text = "";
            this.grdOperatorProfile.ItemsSource = null;
          //  this.canv.Visibility = System.Windows.Visibility.Collapsed;
            this.grdcanvasVRR.Visibility = System.Windows.Visibility.Collapsed;
            this.lblRattingTextwithColorVRR.Visibility = System.Windows.Visibility.Collapsed;
            this.grdcanvasDRR.Visibility = System.Windows.Visibility.Collapsed;
            this.lblRattingTextwithColorDRR.Visibility = System.Windows.Visibility.Collapsed;
        }
        public void PopulateOperatorSearchedResult()
        {
            try
            {
                vsd.hh.data.Operator operatorDetails;
                if (AppProperties.vehicle == null)
                    return;
                if (AppProperties.vehicle.Operator == null)
                    return;
                operatorDetails = AppProperties.vehicle.Operator;
                txtOVVRScore.Text = operatorDetails.OperatorOVRRScore;
                if (AppProperties.Selected_Resource == "English")
                {
                    txtOperatorName.Text = operatorDetails.OperatorName;
                }
                else
                {
                    txtOperatorName.Text = operatorDetails.OperatorNameAr;
                }
                //  int ovWidth = vsd.hh.utilities.CommonUtils.CalculateStringLength(showOvrrScore.Text, showOvrrScore.Font);
                //  showOvrrScore.Width = (ovWidth <= AppProperties.defaultControlWidth) ? AppProperties.defaultControlWidth : ovWidth;
                if (operatorDetails.TopViolatingVehicles == null)
                    return;
                vsd.hh.data.Vehicle[] ViolatingVehicles = operatorDetails.TopViolatingVehicles;
                operatorData = new List<DisplayObject>(ViolatingVehicles.Length);
                int count = 0;
                foreach (vsd.hh.data.Vehicle i in ViolatingVehicles)
                {
                    operatorData.Add(new DisplayObject(i));
                    count++;
                }
                grdOperatorProfile.ItemsSource = null;
                grdOperatorProfile.ItemsSource = operatorData;
                ShowOVRRRattingColour();
                ShowODRRRattingColour();
                // <Image x:Name="imgOVRR2" Grid.Row="1" Grid.Column="7" Width="110" Grid.RowSpan="6"/>

            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }
        public void ShowOVRRRattingColour()
        {
            try
            {
                if ((AppProperties.vehicle != null) && (AppProperties.vehicle.Operator.OperatorOVRRScore != ""))
                {
                    string risk_rating = AppProperties.vehicle.Operator.OperatorOVRRScore;

                    if (canvVRR.Children.Count > 0)
                    {
                        canvVRR.Children.RemoveAt(0);
                    }
                    canvVRR.Visibility = System.Windows.Visibility.Visible;
                    this.grdcanvasVRR.Visibility = System.Windows.Visibility.Visible;
                    lblRattingTextwithColorVRR.Visibility = System.Windows.Visibility.Visible;                    
                    lblRattingTextwithColorVRR.Content = risk_rating;
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
                        canvVRR.Children.Add(rect);
                    }
                    else if (risk_rating.Contains("A"))
                    {
                        rect.Stroke = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFCC00"));

                        rect.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFCC00"));
                        rect.Width = 50;
                        rect.Height = 40;
                        Canvas.SetLeft(rect, 0);
                        Canvas.SetTop(rect, 0);
                        canvVRR.Children.Add(rect);
                    }
                    else if (risk_rating.Contains("R"))
                    {
                        rect.Stroke = new SolidColorBrush(Colors.Red);
                        rect.Fill = new SolidColorBrush(Colors.Red);
                        rect.Width = 50;
                        rect.Height = 40;
                        Canvas.SetLeft(rect, 0);
                        Canvas.SetTop(rect, 0);
                        canvVRR.Children.Add(rect);
                    }
                    else if (risk_rating.Contains("New"))
                    {
                        rect.Stroke = new SolidColorBrush(Colors.Gray);
                        rect.Fill = new SolidColorBrush(Colors.Gray);
                        rect.Width = 50;
                        rect.Height = 40;
                        Canvas.SetLeft(rect, 0);
                        Canvas.SetTop(rect, 0);
                        canvVRR.Children.Add(rect);
                    }
                    else
                    {
                        canvVRR.Visibility = System.Windows.Visibility.Collapsed;
                        grdcanvasVRR.Visibility = System.Windows.Visibility.Collapsed;
                        lblRattingTextwithColorVRR.Visibility = System.Windows.Visibility.Collapsed;
                        
                        AppProperties.vehicle.RiskRating = "";
                    }
                }
                else
                {
                    canvVRR.Visibility = System.Windows.Visibility.Collapsed;
                    grdcanvasVRR.Visibility = System.Windows.Visibility.Collapsed;
                    lblRattingTextwithColorVRR.Visibility = System.Windows.Visibility.Collapsed;
                   
                }
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }
        public void ShowODRRRattingColour()
        {
            try
            {
                if ((AppProperties.vehicle != null) && (AppProperties.vehicle.Operator.OperatorODRRScore != ""))
                {
                    string risk_rating = AppProperties.vehicle.Operator.OperatorODRRScore;

                    if (canvDRR.Children.Count > 0)
                    {
                        canvDRR.Children.RemoveAt(0);
                    }
                    canvDRR.Visibility = System.Windows.Visibility.Visible;
                    this.grdcanvasDRR.Visibility = System.Windows.Visibility.Visible;
                    lblRattingTextwithColorDRR.Visibility = System.Windows.Visibility.Visible;
                    lblRattingTextwithColorDRR.Content = risk_rating;
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
                        canvDRR.Children.Add(rect);
                    }
                    else if (risk_rating.Contains("A"))
                    {
                        rect.Stroke = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFCC00"));

                        rect.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFCC00"));
                        rect.Width = 50;
                        rect.Height = 40;
                        Canvas.SetLeft(rect, 0);
                        Canvas.SetTop(rect, 0);
                        canvDRR.Children.Add(rect);
                    }
                    else if (risk_rating.Contains("R"))
                    {
                        rect.Stroke = new SolidColorBrush(Colors.Red);
                        rect.Fill = new SolidColorBrush(Colors.Red);
                        rect.Width = 50;
                        rect.Height = 40;
                        Canvas.SetLeft(rect, 0);
                        Canvas.SetTop(rect, 0);
                        canvDRR.Children.Add(rect);
                    }
                    else if (risk_rating.Contains("New"))
                    {
                        rect.Stroke = new SolidColorBrush(Colors.Gray);
                        rect.Fill = new SolidColorBrush(Colors.Gray);
                        rect.Width = 50;
                        rect.Height = 40;
                        Canvas.SetLeft(rect, 0);
                        Canvas.SetTop(rect, 0);
                        canvDRR.Children.Add(rect);
                    }
                    else
                    {
                        canvDRR.Visibility = System.Windows.Visibility.Collapsed;
                        grdcanvasDRR.Visibility = System.Windows.Visibility.Collapsed;
                        lblRattingTextwithColorDRR.Visibility = System.Windows.Visibility.Collapsed;

                        AppProperties.vehicle.RiskRating = "";
                    }
                }
                else
                {
                    canvVRR.Visibility = System.Windows.Visibility.Collapsed;
                    grdcanvasDRR.Visibility = System.Windows.Visibility.Collapsed;
                    lblRattingTextwithColorDRR.Visibility = System.Windows.Visibility.Collapsed;

                }
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }


        class DisplayObject : vsd.hh.data.Violation
        {
            private string plateDetails;
            private string riskRating;
            private string _plateNumber;
            private string _plateCode;
            private string _plateCategory;
            private string _emirate;
            private string _country;
            private string _plateSource;

            public string PlateSource
            {
                get { return _plateSource; }
                set { _plateSource = value; }
            }





            public DisplayObject(vsd.hh.data.Vehicle CopyVehicle)
            {
                PlateDetails = (CopyVehicle.PlateNumber.ToString() + " " + CopyVehicle.PlateCategory + " " + CopyVehicle.PlateCode + "," + ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetPlateSourceCode((null != CopyVehicle.Emirate) ? CopyVehicle.Emirate : CopyVehicle.Country)).Trim();
                RiskRating = CopyVehicle.RiskRating;

                PlateNumber = CopyVehicle.PlateNumber.ToString();
                PlateCode = CopyVehicle.PlateCode.ToString();
                PlateCategory = CopyVehicle.PlateCategory.ToString();
                Country = CopyVehicle.Country.ToString();
                Emirate = CopyVehicle.Emirate;
                PlateSource = CopyVehicle.PlateSource;


            }
            public string Country
            {
                get { return _country; }
                set { _country = value; }
            }

            public string Emirate
            {
                get { return _emirate; }
                set { _emirate = value; }
            }

            public string PlateCategory
            {
                get { return _plateCategory; }
                set { _plateCategory = value; }
            }

            public string PlateCode
            {
                get { return _plateCode; }
                set { _plateCode = value; }
            }

            public string PlateNumber
            {
                get { return _plateNumber; }
                set { _plateNumber = value; }
            }
            public string PlateDetails
            {
                get { return this.plateDetails; }
                set { this.plateDetails = value; }
            }

            public string RiskRating
            {
                get { return this.riskRating; }
                set { this.riskRating = value; }
            }
        }

        private void txtOperatorName_GotFocus_1(object sender, RoutedEventArgs e)
        {
            CommonUtils.ShowKeyBoard();
        }

        private void txtOperatorName_LostFocus_1(object sender, RoutedEventArgs e)
        {
            CommonUtils.CLoseKeyBoard();
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

        private void txtOperatorName_TextChanged_1(object sender, TextChangedEventArgs e)
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

        private void txtTraffice_TextChanged_1(object sender, TextChangedEventArgs e)
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

            if (new CommonUtils().GetScreenOrientation() == AppProperties.LandScapetMode)
            {
                // MessageBox.Show("LandScape");
                // ChangeControlDimensions(350);
                ChangeControlDimensions(AppProperties.LndScp_220_350);
                ChangeButtonsDimensions(AppProperties.LndScp_Btn_100_140);
                ChangeLableDimensions(AppProperties.LndScp_Lbl_20_25);
                ChangeHeaderDimensions(AppProperties.LndScp_Header_25_30);
                
            }
            else
            {
                //  MessageBox.Show("Potrait");
                 ChangeControlDimensions(250);
               // ChangeControlDimensions(300);
                ChangeButtonsDimensions(120);
                ChangeLableDimensions(AppProperties.Prtrt_Lbl_25_20);
                ChangeHeaderDimensions(AppProperties.Prtrt_Header_30_25);
                //this.grdOperatorProfile.Width = 320;
            }
        }
        public void ChangeControlDimensions(double width)
        {
            this.txtOperatorName.Width = width;
            this.txtOVVRScore.Width = width;
            this.txtTraffice.Width = width;
            this.grdOperatorProfile.Width = width;
            this.UpdateLayout();
        }
        public void ChangeButtonsDimensions(double width)
        {
            this.imagebtnResetVehicleRecord.Width = width;
            this.imagebtnBack.Width = width;
            this.imagebtnSearch.Width = width;
            this.btnBack.Width = width;
            this.btnSearch.Width = width;
            this.btnResetVehicleRecord.Width = width;
            this.UpdateLayout();
        }
        public void ChangeLableDimensions(double width)
        {
            this.lblOpetatoName2.FontSize = width;
            this.lblTraficeFileNo.FontSize = width;
            this.lblOprOVRR.FontSize = width;
            this.lblOprODRR.FontSize = width;
            this.lblTopFiveViolation.FontSize = width;
            // this.lblAppLogout.FontSize = 20;
            this.UpdateLayout();
        }
        public void ChangeHeaderDimensions(double width)
        {
            this.lblOprProfile.FontSize = width;
            this.lblOpPrfile.FontSize = width;
            this.UpdateLayout();
        }

        private void btnStartInspection_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (grdOperatorProfile.SelectedItem == null)
                    return;
                DisplayObject selcted_Violation_data = (DisplayObject)grdOperatorProfile.SelectedItem;
                if (selcted_Violation_data == null)
                    return;

                if (AppProperties.vehicle != null)
                {
                    AppProperties.vehicle.PlateNumber = selcted_Violation_data.PlateNumber.ToString();
                    AppProperties.vehicle.PlateCode = selcted_Violation_data.PlateCode.ToString();
                    AppProperties.vehicle.PlateCategory = selcted_Violation_data.PlateCategory.ToString();
                    AppProperties.vehicle.Emirate = selcted_Violation_data.Emirate.Trim();
                    AppProperties.vehicle.VehicleCategory = "Heavy Vehicle";
                    AppProperties.vehicle.VehicleCategoryAr = "مركبة ثقيلة";
                    AppProperties.vehicle.PlateSource = selcted_Violation_data.PlateSource;
                    
                }
                this.m_mainWindow.MainContentControl.Content = null;
                AppProperties.isFlowFromOperator = true;
                this.m_mainWindow.MainContentControl.Content = new ucDefectAndViolationDetails(this.m_mainWindow);


            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void txtOperatorName_PreviewKeyDown_1(object sender, KeyEventArgs e)
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

        private void txtTraffice_PreviewKeyDown_1(object sender, KeyEventArgs e)
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
    }
}