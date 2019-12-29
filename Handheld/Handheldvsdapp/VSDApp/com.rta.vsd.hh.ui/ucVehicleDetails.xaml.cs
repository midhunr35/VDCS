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
using System.Threading;
using System.Windows.Threading;
using System.Collections.ObjectModel;
using VSDApp.com.rta.vsd.hh.db;
using System.Data;
using VSDApp.com.rta.vsd.hh.utilities;
using VSDApp.com.rta.vsd.hh.data;
using VSDApp.WPFMessageBoxControl;

namespace VSDApp.com.rta.vsd.hh.ui
{
    /// <summary>
    /// Interaction logic for ucVehicleDetails.xaml
    /// </summary>
    public partial class ucVehicleDetails : UserControl
    {
        public ucVehicleDetails()
        {
            InitializeComponent();
            Visibility = Visibility.Hidden;
        }

        private bool _hideRequest = false;
        private string _result = "";
        private UIElement _parent;

        public void SetParent(UIElement parent)
        {
            _parent = parent;
        }

        #region Message

        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Message.
        // This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register(
                "Message", typeof(string), typeof(ucVehicleDetails), new UIPropertyMetadata(string.Empty));

        #endregion

        public string ShowHandlerDialog(string message)
        {
            try
            {
                chngBtnsImgs();
                VehicleInformation();
                //if (chkComments.Items.Count > 0)
                //{
                Message = message;
                Visibility = Visibility.Visible;
                _parent.IsEnabled = false;
                _hideRequest = false;
                while (!_hideRequest)
                {
                    // HACK: Stop the thread if the application is about to close
                    if (this.Dispatcher.HasShutdownStarted ||
                        this.Dispatcher.HasShutdownFinished)
                    {
                        break;
                    }

                    // HACK: Simulate "DoEvents"
                    this.Dispatcher.Invoke(
                        DispatcherPriority.Background,
                        new ThreadStart(delegate { }));
                    Thread.Sleep(20);
                }
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
            return _result;
        }

        private void chngBtnsImgs()
        {

        }

        //public void PopulateVehHistoryGrid()
        //{
        //    try
        //    {

        //        if (AppProperties.vehicle == null)
        //            return;

        //        vsd.hh.data.Vehicle VehicleToInspect;
        //        VehicleToInspect = AppProperties.vehicle;

        //        if (AppProperties.vehicle.Violations == null)
        //            return;
        //        violationData = new List<DisplayObject>(AppProperties.vehicle.Violations.Length);
        //        int count = 0;
        //        foreach (vsd.hh.data.Violation i in AppProperties.vehicle.Violations)
        //        {
        //            violationData.Add(new DisplayObject(i));
        //            count++;
        //        }
        //        grdViolationDetails.ItemsSource = null;
        //        grdViolationDetails.ItemsSource = violationData;

        //        System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.DigitSubstitution = System.Globalization.DigitShapes.None;
        //        this.UpdateLayout();

        //    }
        //    catch (Exception ex)
        //    {
        //        App.VSDLog.Info(ex.StackTrace);
        //        WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
        //    }



        //}


        public void VehicleInformation()
        {
            try
            {
                App.VSDLog.Info("ucVehicle Selection PopulateData_VehicelProfileInspection()-----");
                if (AppProperties.vehicle != null)
                {
                    txtChassisNumber.Text = (AppProperties.vehicle == null ? "" : AppProperties.vehicle.ChassisNumber);
                    txtMake.Text = (AppProperties.vehicle == null ? "" : AppProperties.vehicle.Make);
                    txtModel.Text = (AppProperties.vehicle == null ? "" : AppProperties.vehicle.Model);
                    txtYear.Text = (AppProperties.vehicle == null ? "" : AppProperties.vehicle.Year);
                    if (AppProperties.Selected_Resource == "English")
                    {
                        txtSubCategory.Text = (AppProperties.vehicle == null ? "" : AppProperties.vehicle.SubCategoryAr);
                        txtCategory.Text = (AppProperties.vehicle == null ? "" : AppProperties.vehicle.VehicleCategory);
                    }
                    else
                    {
                        txtSubCategory.Text = (AppProperties.vehicle == null ? "" : AppProperties.vehicle.SubCategoryAr);
                        txtCategory.Text = (AppProperties.vehicle == null ? "" : AppProperties.vehicle.VehicleCategoryAr);
                    }
                    txtregistrationExpiry.Text = AppProperties.vehicle.RegExpiry.ToString();
                   // txtregistrationExpiry.Text = "2016-01-31";
                    if (AppProperties.vehicle != null)
                    {
                        if (AppProperties.Selected_Resource == "English")
                        {
                            if (AppProperties.vehicle.Operator != null)
                            {
                                if ((AppProperties.vehicle.Operator.OperatorName == null) || (AppProperties.vehicle.Operator.OperatorName == ""))
                                {
                                    txtlbloperatorName.Text = AppProperties.vehicle.Operator.TrafficFileNumber;
                                }
                                else
                                {
                                    txtlbloperatorName.Text = AppProperties.vehicle.Operator.OperatorName;
                                }
                            }
                        }
                        else
                        {
                            if (AppProperties.vehicle.Operator != null)
                            {
                                if ((AppProperties.vehicle.Operator.OperatorNameAr == null) || (AppProperties.vehicle.Operator.OperatorNameAr == ""))
                                {
                                    txtlbloperatorName.Text = AppProperties.vehicle.Operator.TrafficFileNumber;
                                }
                                else
                                {
                                    txtlbloperatorName.Text = AppProperties.vehicle.Operator.OperatorNameAr;
                                }
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                App.VSDLog.Info("PopulateData_VehicelProfileInspection() Exception");
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }



        private void HideHandlerDialog()
        {
            try
            {
                _hideRequest = true;
                Visibility = Visibility.Hidden;
                _parent.IsEnabled = true;
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _result = "";
                HideHandlerDialog();
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }
    }
}
