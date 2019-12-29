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
using VSDApp.com.rta.vsd.hh.data;
using VSDApp.com.rta.vsd.hh.utilities;
using VSDApp.WPFMessageBoxControl;

namespace VSDApp.com.rta.vsd.hh.ui
{
    /// <summary>
    /// Interaction logic for DefectSummaryScreen.xaml
    /// </summary>
    public partial class DefectSummaryScreen : Window
    {
        MainWindow m_mainWindow;
        List<vsd.hh.data.Violation.Defects> AddedDefect = new List<data.Violation.Defects>();

        public DefectSummaryScreen(MainWindow mainWnd)
        {
            InitializeComponent();
            m_mainWindow = mainWnd;

            
            
           
        }
        public void PopulateDefectSummary(List<vsd.hh.data.Violation.Defects> defect)
        {
            try
            {
                this.grdAddedDefects.ItemsSource = defect;
                System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.DigitSubstitution = System.Globalization.DigitShapes.None;
                this.UpdateLayout();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            AppProperties.Is_SubmitVilation = false;
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            AppProperties.Is_SubmitVilation = true;
            this.Close();
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            if (AppProperties.Selected_Resource == "Arabic")
            {
                stckPanelbtn.FlowDirection = System.Windows.FlowDirection.LeftToRight;
                lblConfirmation.FlowDirection = System.Windows.FlowDirection.LeftToRight;
                lblgrdSeverityEn.Visibility = System.Windows.Visibility.Collapsed;
                lblgrdSeverityAr.Visibility = System.Windows.Visibility.Visible;

                internalGrid.Children.Remove(stckPanelbtn);
                Grid.SetRow(stckPanelbtn, 0);
                Grid.SetColumn(stckPanelbtn, 0);
                internalGrid.Children.Add(stckPanelbtn);

                internalGrid.Children.Remove(lblConfirmation);
                Grid.SetRow(lblConfirmation, 0);
                Grid.SetColumn(lblConfirmation, 1);
                internalGrid.Children.Add(lblConfirmation);


            }
            else
            {
                lblgrdSeverityEn.Visibility = System.Windows.Visibility.Visible;
                lblgrdSeverityAr.Visibility = System.Windows.Visibility.Collapsed; ;

                internalGrid.Children.Remove(stckPanelbtn);
                Grid.SetRow(stckPanelbtn, 0);
                Grid.SetColumn(stckPanelbtn, 1);
                internalGrid.Children.Add(stckPanelbtn);

                internalGrid.Children.Remove(lblConfirmation);
                Grid.SetRow(lblConfirmation, 0);
                Grid.SetColumn(lblConfirmation, 0);
                internalGrid.Children.Add(lblConfirmation);

            }
        }

        private void Window_Initialized_1(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.DigitSubstitution = System.Globalization.DigitShapes.None;
            this.UpdateLayout();
        }
    }

    public class GPSLocation :Window
    {
        
    }
}
