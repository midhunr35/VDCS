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

namespace VSDApp.com.rta.vsd.hh.ui
{
    /// <summary>
    /// Interaction logic for PrintPreview.xaml
    /// </summary>
    public partial class PrintPreview : Window
    {
        MainWindow m_MainWindow = null;
        ucViolationSummary m_ucViolationSummary = null;
        public PrintPreview(MainWindow mainWnd, ucViolationSummary ucViolationSumry)
        {
            InitializeComponent();
            m_MainWindow = mainWnd;
            this.m_ucViolationSummary = ucViolationSumry;

            if (AppProperties.Selected_Resource == "English")
            {
                imag.Source = new BitmapImage(new Uri(@"/Images/PrintPreviewFront.png", UriKind.Relative));
            }
            else
            {
                imag.Source = new BitmapImage(new Uri(@"/Images/ArabicNew.jpg", UriKind.Relative));
            }
        }

        private void btnback_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                m_ucViolationSummary.Print();
               // violationSummery.Print();
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}
