using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;
using Technewlogic.Samples.WpfModalDialog;
using VSDApp.com.rta.vsd.hh.utilities;
using VSDApp.WPFMessageBoxControl;

namespace VSDApp.com.rta.vsd.hh.ui
{
    /// <summary>
    /// Interaction logic for ucAplrSelection.xaml
    /// </summary>
    public partial class ucAplrSelection : UserControl
    {
        private UIElement _parent;
        private bool _hideRequest = false;
        private string _result = "";
        MainWindow m_MainWindow;
        BackgroundWorker m_oWorker;
        System.Timers.Timer timer;
        string SelectedALPRVehicle = string.Empty;
        public delegate void ShowTargetedVehicle_Delegate();
        public ucAplrSelection()
        {
            InitializeComponent();
        }
        public void SetParent(UIElement parent)
        {
            _parent = parent;
        }
        #region Message

        public string Message_Alpr
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Message.
        // This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register(
                "Message_Alpr", typeof(string), typeof(ModalDialog), new UIPropertyMetadata(string.Empty));

        #endregion

        public string ShowHandlerDialog(MainWindow mainWindow)
        {
            try
            {
                // Message_InspectorDialog = message;
                Visibility = Visibility.Visible;
                _parent.IsEnabled = false;
                _hideRequest = false;
                m_MainWindow = mainWindow;
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
                System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.DigitSubstitution = System.Globalization.DigitShapes.None;
                this.UpdateLayout();
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
            return _result;
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
        private void cmboxAlprSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void UserControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                _hideRequest = true;
                Visibility = Visibility.Hidden;
                _parent.IsEnabled = true;
                StartCallingALPRVehicles();

            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void grdMain_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void btnok_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmboxAlprSelection.SelectedItem == null)
                    return;
                AppProperties.Selected_ALPR_Name = Convert.ToString(cmboxAlprSelection.SelectedValue);
                _hideRequest = true;
                Visibility = Visibility.Hidden;
                _parent.IsEnabled = true;
               

               StartCallingALPRVehicles();

            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }
        private void StartCallingALPRVehicles()
        {
            try
            {
                m_oWorker = new BackgroundWorker();


                m_oWorker.DoWork += m_oWorker_DoWork;

                m_oWorker.RunWorkerCompleted += m_oWorker_RunWorkerCompleted;
                m_oWorker.WorkerReportsProgress = true;
                m_oWorker.WorkerSupportsCancellation = true;
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        void m_oWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                 this.Dispatcher.Invoke(
                            new ShowTargetedVehicle_Delegate(this.ShowMessageToInspector),
                            new object[] { }
                        );
                
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        void m_oWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                AlprTargetedVehicles alprVeh = new AlprTargetedVehicles(m_MainWindow);
               SelectedALPRVehicle  = alprVeh.StartGettingAlprIntrestedVehicle();
               // if (SelectedVehicle != string.Empty && SelectedVehicle != null)
               // {
                  //  WPFMessageBox.Show(SelectedVehicle);
               // }
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }
        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (m_oWorker != null &&!m_oWorker.IsBusy)
                m_oWorker.RunWorkerAsync();
        }
        public void ShowMessageToInspector()
        {
            if (SelectedALPRVehicle == string.Empty || SelectedALPRVehicle == "")
                return;
          //  WPFMessageBox.Show(SelectedALPRVehicle);
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                string AlprCam01Name = Properties.Settings.Default.AlprCam01_Name;
                string AlprCam02Name = Properties.Settings.Default.AlprCam02_Name;
                string Alpr_Timer_Min = Properties.Settings.Default.AlprCam_Timer;
                cmboxAlprSelection.Items.Add(AlprCam01Name);
                cmboxAlprSelection.Items.Add(AlprCam02Name);
                cmboxAlprSelection.SelectedIndex = 0;
                timer = new System.Timers.Timer();
                timer.Elapsed += timer_Elapsed;               
                if (Alpr_Timer_Min != null)
                {
                    timer.Interval = Convert.ToInt32(Alpr_Timer_Min) * 60000;
                }
                else
                {
                    timer.Interval = 60000;
                }
                timer.Enabled = true;
                
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                //WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }
    }
}
