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
    /// Interaction logic for ucIntelligentTarget.xaml
    /// </summary>
    public partial class ucIntelligentTarget : UserControl
    {
        MainWindow m_MainWindow;
        public ucIntelligentTarget(MainWindow mainWnd)
        {
            InitializeComponent();
            m_MainWindow = mainWnd;

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                SHowHideBannerInfoMainPage(false);
                browserMaps.Source = new Uri(Properties.Settings.Default.IntellegentTargetingGIS);
                if (AppProperties.Selected_Resource == "English")
                {
                    btnBackImage.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Back.png", UriKind.Relative));

                }
                else
                {
                    btnBackImage.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/back Arabic Up.png", UriKind.Relative));
                }
            }
            catch (Exception ex)
            {
                WPFMessageBoxResult _Result = WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
                // throw;
            }
        }
        public void SHowHideBannerInfoMainPage(bool isEnab)
        {
            try
            {
                if (isEnab)
                {
                    m_MainWindow.mainBanner.Visibility = System.Windows.Visibility.Visible;
                    m_MainWindow.imgRTALeft.Visibility = System.Windows.Visibility.Visible;
                    m_MainWindow.imgRTARight.Visibility = System.Windows.Visibility.Visible;
                    m_MainWindow.imgComplete.Visibility = System.Windows.Visibility.Visible;
                   
                    m_MainWindow.maingrdRow1.Height = new GridLength(10);
                    
                    m_MainWindow.maingrdRow2.Height = new GridLength(46, GridUnitType.Auto);
                    m_MainWindow.maingrdRow3.Height = new GridLength(33);
                }
                else
                {
                    m_MainWindow.mainBanner.Visibility = System.Windows.Visibility.Collapsed;
                   m_MainWindow.imgRTALeft.Visibility = System.Windows.Visibility.Collapsed;
                    m_MainWindow.imgRTARight.Visibility = System.Windows.Visibility.Collapsed;
                    m_MainWindow.imgComplete.Visibility = System.Windows.Visibility.Collapsed;
                   
                    m_MainWindow.maingrdRow1.Height = new GridLength(0, GridUnitType.Auto);

                    m_MainWindow.maingrdRow2.Height = new GridLength(0, GridUnitType.Auto);
                    m_MainWindow.maingrdRow3.Height = new GridLength(0, GridUnitType.Auto);

                }
            }
            catch (Exception ex)
            {
                WPFMessageBoxResult _Result = WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
                // throw;
            }
        }

        private void imagebtnBack_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {

            if (AppProperties.Selected_Resource == "English")
            {
                btnBackImage.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Back Down.png", UriKind.Relative));
            }
            else
            {
                btnBackImage.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/back Arabic Down.png", UriKind.Relative));
            }
        }

        private void btnback_Click_1(object sender, MouseButtonEventArgs e)
        {
            if (AppProperties.Selected_Resource == "English")
            {
                btnBackImage.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Back.png", UriKind.Relative));
            }
            else
            {
                btnBackImage.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/back Arabic Up.png", UriKind.Relative));
            }
            m_MainWindow.MainContentControl.Content = null;
            m_MainWindow.MainContentControl.Content = new ucWellComeScreen(this.m_MainWindow);
        }

        private void btnback_Click(object sender, RoutedEventArgs e)
        {
            SHowHideBannerInfoMainPage(true);
            m_MainWindow.MainContentControl.Content = null;
            m_MainWindow.MainContentControl.Content = new ucWellComeScreen(this.m_MainWindow);
        }
    }
}
