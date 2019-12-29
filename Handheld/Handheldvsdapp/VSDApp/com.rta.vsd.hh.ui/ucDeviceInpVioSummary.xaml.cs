using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// Interaction logic for ucDeviceInpVioSummary.xaml
    /// </summary>
    public partial class ucDeviceInpVioSummary : UserControl
    {
        MainWindow m_MainWind = null;
        public ucDeviceInpVioSummary(MainWindow maiWind)
        {
            InitializeComponent();
            m_MainWind = maiWind;
        }

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadViolationSummaryData();
            }

            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }

        }
        public void LoadViolationSummaryData()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.DigitSubstitution = System.Globalization.DigitShapes.None;
            App.VSDLog.Info("\nentered in PopulateData function\n");
            if (AppProperties.Selected_Resource == "English")
            {
                txtboxViolationSeverity.Text = AppProperties.recordedViolation.ViolationSeverity;
                txtboxDueDate.Text = AppProperties.recordedViolation.ViolationDueDays.ToString("dd/MM/yyyy");
                txtViolationID.Text = AppProperties.recordedViolation.ViolationTicketCode;
            }
            else
            {
                if (AppProperties.recordedViolation.ViolationSeverityAr == null)
                {
                    txtboxViolationSeverity.Text = AppProperties.recordedViolation.ViolationSeverity;

                }
                else
                {
                    txtboxViolationSeverity.Text = AppProperties.recordedViolation.ViolationSeverityAr;
                }
                txtboxDueDate.Text = AppProperties.recordedViolation.ViolationDueDays.ToString("dd/MM/yyyy");
                txtViolationID.Text = AppProperties.recordedViolation.ViolationTicketCode;
            }
        }

        private void UserControl_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {

        }

        private void UserControl_Initialized_1(object sender, EventArgs e)
        {

        }

        private void btnPrint_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void btnback_Click_1(object sender, RoutedEventArgs e)
        {
            m_MainWind.MainContentControl.Content = null;
            m_MainWind.MainContentControl.Content = new ucLocationSelectionEn(this.m_MainWind);
        }
    }
}
