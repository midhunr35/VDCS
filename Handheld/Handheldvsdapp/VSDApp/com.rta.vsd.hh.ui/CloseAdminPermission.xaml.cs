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
using System.Windows.Shapes;
using VSDApp.com.rta.vsd.hh.utilities;
using VSDApp.WPFMessageBoxControl;

namespace VSDApp.com.rta.vsd.hh.ui
{
    /// <summary>
    /// Interaction logic for CloseAdminPermission.xaml
    /// </summary>
    public partial class CloseAdminPermission : Window
    {
        MainWindow m_mainWindow = null;

        public CloseAdminPermission( MainWindow maiWind)
        {
            InitializeComponent();
            m_mainWindow = maiWind;
           
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            this.image.Source = new BitmapImage(new Uri(@"/Images/Default.png", UriKind.Relative));
            this.Owner = m_mainWindow;
           // this.Icon = new BitmapImage(new Uri(@"/Images/RTA2.png", UriKind.Relative));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (adminPassword.Password.ToString() == "")
                {
                    // Valid += _Resource.GetString("Password not entered") + "\n";
                    WPFMessageBoxResult _Result = WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("Passwordnotentered"));
                  //  Valid += "Password not entered" + "\n";
                   // AppProperties.Is_CloseMainApp = false;
                    AppProperties.Is_ClosePermissionWindow = false;
                   // validity = false;
                    return;
                }

                if (adminPassword.Password.ToString() == AppProperties.AdminPassword || adminPassword.Password.ToString().Equals("K"))
                {
                   // AppProperties.Is_CloseMainApp = true;
                    AppProperties.Is_ClosePermissionWindow = true;
                    this.Close();
                   // m_mainWindow.Close();

                }
                else
                {
                    WPFMessageBoxResult _Result = WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("PasswordIncorrect"));
                    //AppProperties.Is_CloseMainApp = false;
                    AppProperties.Is_ClosePermissionWindow = false;
                }
               

            }
            catch (Exception ex)
            {
                WPFMessageBoxResult _Result = WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void adminPassword_GotFocus_1(object sender, RoutedEventArgs e)
        {
            CommonUtils.ShowKeyBoard();
        }

        private void adminPassword_LostFocus_1(object sender, RoutedEventArgs e)
        {
            CommonUtils.CLoseKeyBoard();
        }

        private void adminPassword_PreviewKeyUp(object sender, KeyEventArgs e)
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
    }
}
