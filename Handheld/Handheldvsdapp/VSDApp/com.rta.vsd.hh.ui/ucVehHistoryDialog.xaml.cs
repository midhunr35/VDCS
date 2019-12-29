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
    /// Interaction logic for ucVehHishoryDialog.xaml
    /// </summary>
    public partial class ucVehHistoryDialog : UserControl
    {

        public ucVehHistoryDialog()
        {
            InitializeComponent();
            Visibility = Visibility.Hidden;


        }

        private bool _hideRequest = false;
        private string _result = "";
        private UIElement _parent;
        private List<DisplayObject> violationData;

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
                "Message", typeof(string), typeof(ucVehHistoryDialog), new UIPropertyMetadata(string.Empty));

        #endregion

        public string ShowHandlerDialog(string message)
        {
            try
            {
                chngBtnsImgs();
                PopulateVehHistoryGrid();
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

        public void PopulateVehHistoryGrid()
        {
            try
            {

                if (AppProperties.vehicle == null)
                    return;

                vsd.hh.data.Vehicle VehicleToInspect;
                VehicleToInspect = AppProperties.vehicle;

                if (AppProperties.vehicle.Violations == null)
                    return;
                violationData = new List<DisplayObject>(AppProperties.vehicle.Violations.Length);
                int count = 0;
                foreach (vsd.hh.data.Violation i in AppProperties.vehicle.Violations)
                {
                    violationData.Add(new DisplayObject(i));
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

        private void CancelButton_Click(object sender, RoutedEventArgs e)
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


        private void btnOk_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
        }


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
            DefectDetails = CopyViolation.Defect;
            DriverLicNo = CopyViolation.DriverLicNo;
            RtaEmpID = CopyViolation.RtaEmpID;
            ViolationSeverityAr = CopyViolation.ViolationCommentsAr;
            PlateNumber = CopyViolation.PlateNumber;
            PlateCode = CopyViolation.PlateCode;
            PlateCategory = CopyViolation.PlateCategory;

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

        public string IssueDate
        {
            get { return this._IssueDate; }
            set { this._IssueDate = value; }
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
}
