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
using VSDApp.WPFMessageBoxControl;
using VSDApp;

namespace Technewlogic.Samples.WpfModalDialog
{
    /// <summary>
    /// Interaction logic for ModalDialog.xaml
    /// </summary>
    public partial class ModalDialog : UserControl
    {

        public ModalDialog()
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
                "Message", typeof(string), typeof(ModalDialog), new UIPropertyMetadata(string.Empty));

        #endregion

        public string ShowHandlerDialog(string existingComment, string defect)
        {
            try
            {
                populateCommentList(existingComment, defect);
                if (chkComments.Items.Count > 0)
                {
                    chkComments.Visibility = Visibility.Visible;
                }
                else
                {
                    chkComments.Visibility = Visibility.Collapsed;
                    txtComments.Focus();
                }
                Message = existingComment;
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

        private void populateCommentList(string existingComment, string defect)
        {
            try
            {
                txtComments.Text = "";
                txtComments.Text = existingComment;
                chkComments.Items.Clear();
                chkComments.CheckedItems.Clear();
                // int vehCatID = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetVehicleCategoryID(AppProperties.vehicle.VehicleCategory.Trim());
                DataTable dtComments = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetCommentsByDefectID(defect, AppProperties.vehicle.VehicleCategory.Trim());
                string columnToPopulate = "";
                if (AppProperties.Selected_Resource == "English")
                    columnToPopulate = "DEFECT_COMMENT";
                else
                    columnToPopulate = "DEFECT_COMMENT_A";
                if (dtComments != null)
                {
                    if (dtComments.Rows.Count > 0)
                        foreach (DataRow dr in dtComments.Rows)
                        {
                            chkComments.Items.Add(dr[columnToPopulate]);
                        }
                }
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
                _result = txtComments.Text;
                ComentsPopup.IsOpen = false;
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
                ComentsPopup.IsOpen = false;
                HideHandlerDialog();
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void txtComments_GotFocus(object sender, RoutedEventArgs e)
        {
            CommonUtils.ShowKeyBoard();
        }

        private void chkComments_CheckedChanged(object sender, Microsoft.Windows.Controls.CheckListBoxCheckedChangedEventArgs e)
        {
            try
            {
                _result = "";
                int count = 0;
                foreach (var item in chkComments.CheckedItems)
                {
                    if (count == 0)
                        _result += item.ToString();
                    else
                        _result += ", " + item.ToString();
                    count++;
                }
                txtComments.Text = _result;
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void txtComments_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.DigitSubstitution = System.Globalization.DigitShapes.None;
                this.UpdateLayout();
                if (AppProperties.Selected_Resource == "English")
                {
                    if (txtComments.Text.Trim() == "")
                    {
                        ComentsPopup.IsOpen = false;
                    }
                    else
                    {
                        //  PopupTextBlock.Text = "Hint: Username and Password is Key Sensitive";
                        PopupTextBlock.Text = new CommonUtils().GetStringValue("ComentsToolTip");
                        ComentsPopup.IsOpen = true;
                    }
                }
                else
                {
                    System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.DigitSubstitution = System.Globalization.DigitShapes.None;
                    this.UpdateLayout();
                    if (txtComments.Text.Trim() == "")
                    {
                        ComentsPopup.IsOpen = false;
                    }
                    else
                    {
                        //  PopupTextBlock.Text = "يرجى مراعاة حجم الحروف";
                        PopupTextBlock.Text = new CommonUtils().GetStringValue("ComentsToolTip");
                        ComentsPopup.IsOpen = true;
                    }
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
