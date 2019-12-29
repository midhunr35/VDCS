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
using System.Windows.Controls.DataVisualization.Charting;
using System.Collections;
using System.IO;
using Technewlogic.Samples.WpfModalDialog;


namespace VSDApp.com.rta.vsd.hh.ui
{
    /// <summary>
    /// Interaction logic for ucInspectionType.xaml
    /// </summary>
    public partial class ucInspectionType : UserControl
    {
        private UIElement _parent;
        private bool _hideRequest = false;
        private string _result = "";
        MainWindow m_MainWindow;
        public ucInspectionType()
        {
          
            InitializeComponent();
            Visibility = Visibility.Hidden;
        }
        public void SetParent(UIElement parent)
        {
            _parent = parent;
        }
        #region Message

        public string Message_InspectionType
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Message.
        // This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register(
                "Message_InspectionType", typeof(string), typeof(ModalDialog), new UIPropertyMetadata(string.Empty));

        #endregion

        public string ShowHandlerDialog(String message,MainWindow mainWindow)
        {
            try
            {
                Message_InspectionType = message;
                AppProperties.Is_DeviceInspection = false;
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

        private void btnCancel_Click(object sender, RoutedEventArgs e)
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

        private void UserControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            btnCancel_Click(sender, null);
        }

        private void grdMain_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void imageRecordInspection_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                AppProperties.Is_DeviceInspection = false;
                this.m_MainWindow.MainContentControl.Content = null;
                this.m_MainWindow.MainContentControl.Content = new ucLocationSelectionEn(m_MainWindow);

                // WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), "Record Inspection");
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }

        }

        private void imageProvisionalInspection_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                AppProperties.Is_DeviceInspection = true;
                this.m_MainWindow.MainContentControl.Content = null;
                this.m_MainWindow.MainContentControl.Content = new ucLocationSelectionEn(m_MainWindow);
               
                //WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), "Provisional Inspection");
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void imageRecordInspection_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                if (AppProperties.Selected_Resource == "English")
                {
                    imageRecordInspection.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/inspection_mo.png", UriKind.Relative));
                }
                else
                {
                    imageRecordInspection.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/inspection_mo_a.png", UriKind.Relative));
                }

            }
            catch (Exception ex)
            {
                WPFMessageBoxResult _Result = WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void imageRecordInspection_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {
                if (AppProperties.Selected_Resource == "English")
                {
                    imageRecordInspection.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/inspection.png", UriKind.Relative));
                }
                else
                {
                    imageRecordInspection.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/inspection_a.png", UriKind.Relative));
                }

            }
            catch (Exception ex)
            {
                WPFMessageBoxResult _Result = WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void imageProvisionalInspection_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                if (AppProperties.Selected_Resource == "English")
                {
                    imageProvisionalInspection.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/Device inspection_mo.png", UriKind.Relative));
                }
                else
                {
                    imageProvisionalInspection.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/Device inspection_mo_a.png", UriKind.Relative));
                }
            }
            catch (Exception ex)
            {
                WPFMessageBoxResult _Result = WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void imageProvisionalInspection_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {
                if (AppProperties.Selected_Resource == "English")
                {
                    imageProvisionalInspection.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/Device inspection.png", UriKind.Relative));
                }
                else
                {
                    imageProvisionalInspection.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/Device inspection_a.png", UriKind.Relative));
                }
            }
            catch (Exception ex)
            {
                WPFMessageBoxResult _Result = WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (AppProperties.Selected_Resource == "English")
            {
                imageRecordInspection.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/inspection.png", UriKind.Relative));
                imageProvisionalInspection.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/Device inspection.png", UriKind.Relative));
            }
            else
            {
                imageRecordInspection.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/inspection_a.png", UriKind.Relative));
                imageProvisionalInspection.Source = new BitmapImage(new Uri(@"/Images/Buttons/NewMenue/Device inspection_a.png", UriKind.Relative));
            }
        }
    }
}
