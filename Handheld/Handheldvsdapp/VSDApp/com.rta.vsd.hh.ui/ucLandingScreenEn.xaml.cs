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
    /// <summary>
    /// Interaction logic for ucLandingScreenEn.xaml Nokia here map
    /// </summary>
    public partial class ucLandingScreenEn : UserControl
    {
        MainWindow m_MainWindow = null;
        public ucLandingScreenEn(MainWindow mainWnd)
        {
            InitializeComponent();
            this.m_MainWindow = mainWnd;
        }

        private void btnRecordViolation_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                this.m_MainWindow.MainContentControl.Content = m_MainWindow.m_PagesList[4];
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSearchVehicleProfile_Click_1(object sender, RoutedEventArgs e)
        {
            this.m_MainWindow.MainContentControl.Content = null;
            this.m_MainWindow.MainContentControl.Content = new ucSearchOptionScreen(this.m_MainWindow);
        }
    }
}
