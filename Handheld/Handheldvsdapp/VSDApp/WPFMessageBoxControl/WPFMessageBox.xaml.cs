using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace VSDApp.WPFMessageBoxControl
{
    /// <summary>
    /// Interaction logic for WPFMessageBox.xaml
    /// </summary>
    public partial class WPFMessageBox : Window
    {
        public WPFMessageBox(string culture)
        {
            InitializeComponent();
            
           
        }
        public WPFMessageBox()
        {
            InitializeComponent();
          //  this.Owner = Application.Current.MainWindow;
        }
        public WPFMessageBoxResult Result { get; set; }

        public static WPFMessageBoxResult Show(string message)
        {
            return Show(string.Empty, message, string.Empty, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Default);
        }

        public static WPFMessageBoxResult Show(string title, string message)
        {
            return Show(title, message, string.Empty, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Default);
        }

        public static WPFMessageBoxResult Show(string title, string message, string details)
        {
            return Show(title, message, details, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Default);
        }

        public static WPFMessageBoxResult Show(string title, string message, WPFMessageBoxButtons buttonOption)
        {
            return Show(title, message, string.Empty, buttonOption, WPFMessageBoxImage.Default);
        }

        public static WPFMessageBoxResult Show(string title, string message, string details, WPFMessageBoxButtons buttonOption)
        {
            return Show(title, message, details, buttonOption, WPFMessageBoxImage.Default);
        }

        public static WPFMessageBoxResult Show(string title, string message, WPFMessageBoxImage image)
        {
            return Show(title, message, string.Empty, WPFMessageBoxButtons.OK, image);
        }

        public static WPFMessageBoxResult Show(string title, string message, string details, WPFMessageBoxImage image)
        {
            return Show(title, message, details, WPFMessageBoxButtons.OK, image);
        }

        public static WPFMessageBoxResult Show(string title, string message, WPFMessageBoxButtons buttonOption, WPFMessageBoxImage image)
        {
           
            return Show(title, message, string.Empty, buttonOption, image);
        }

        public static WPFMessageBoxResult Show(string title, string message, string details, WPFMessageBoxButtons buttonOption, WPFMessageBoxImage image)
        {
            ___MessageBox = new WPFMessageBox();
            MessageBoxViewModel __ViewModel = new MessageBoxViewModel(___MessageBox, title, message, details, buttonOption, image);
            ___MessageBox.DataContext = __ViewModel;
            ___MessageBox.ShowDialog();
            return ___MessageBox.Result;
        }

        [ThreadStatic]
        static WPFMessageBox ___MessageBox;

        protected override void OnSourceInitialized(EventArgs e)
        {
            IconHelper.RemoveIcon(this);
        }
        public static void ChangeCultureInternally(CultureInfo newCulture)
        {
            
        }
        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            Thread.CurrentThread.CurrentCulture.ToString();
            if (Thread.CurrentThread.CurrentCulture.ToString() == "ar-AE")
            {
                this.FlowDirection = FlowDirection.RightToLeft;
            }
            else
            {
                this.FlowDirection = FlowDirection.LeftToRight;
            }
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            e.Handled = true;
        }

       

    }
}
