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
using VSDApp.com.rta.vsd.hh.utilities;

namespace VSDApp.com.rta.vsd.hh.ui
{
    //********************************************
    //******************Excluded******************
    //********************************************



    /// <summary>
    /// Interaction logic for ucSearchOptionScreen.xaml
    /// </summary>
    public partial class ucSearchOptionScreen : UserControl
    {
        MainWindow m_MainWindow = null;
        public ucSearchOptionScreen(MainWindow mainWnd)
        {
            InitializeComponent();
            m_MainWindow = mainWnd;
        }

        private void btnback_Click_1(object sender, RoutedEventArgs e)
        {
            this.m_MainWindow.MainContentControl.Content = null;
          //  this.m_MainWindow.MainContentControl.Content = new ucLocationSelectionEn(m_MainWindow);
            this.m_MainWindow.MainContentControl.Content = new ucWellComeScreen(m_MainWindow);
        }

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            //this.rdoBtnVehicleProfile.IsChecked = true;
        }

        private void btnNext_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {


                if (rdoBtnViolationHistory.IsChecked == true)
                {
                    this.m_MainWindow.MainContentControl.Content = null;
                    this.m_MainWindow.MainContentControl.Content = new ucSearchViolationListing(this.m_MainWindow);
                }
                else if (rdoBtnVehicleProfile.IsChecked == true)
                {
                    this.m_MainWindow.MainContentControl.Content = null;
                    this.m_MainWindow.MainContentControl.Content = new ucSearchVehicle(this.m_MainWindow);
                }
                else if (rdoBtnOperatorProfile.IsChecked == true)
                {
                    this.m_MainWindow.MainContentControl.Content = null;
                    this.m_MainWindow.MainContentControl.Content = new ucSearchOperatorProfile(this.m_MainWindow);
                }
                else
                {
                    MessageBox.Show("No Option Selected For Search");
                }
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
            }
        }
    }
}
