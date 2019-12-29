using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;
using log4net;

namespace VSDApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static readonly ILog VSDLog = LogManager.GetLogger("VSDAppLog");
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            //  Application.Current.MainWindow = new MainWindow();
            // Application.Current.MainWindow.Show();
        }

        public static void ChangeCulture(CultureInfo newCulture)
        {
            newCulture.NumberFormat.DigitSubstitution = DigitShapes.None;
            Thread.CurrentThread.CurrentCulture = newCulture;
            Thread.CurrentThread.CurrentUICulture = newCulture;
            Thread.CurrentThread.CurrentUICulture.NumberFormat.DigitSubstitution = DigitShapes.None;

            var oldWindow = Application.Current.MainWindow;

            Application.Current.MainWindow = new MainWindow();
            Application.Current.MainWindow.Show();
            if (newCulture.ToString() == "ar-AE")
            {

                Application.Current.MainWindow.FlowDirection = FlowDirection.RightToLeft;
            }
            else
            {
                Application.Current.MainWindow.FlowDirection = FlowDirection.LeftToRight;
            }
            Thread.CurrentThread.CurrentCulture.NumberFormat.DigitSubstitution = DigitShapes.None;
            oldWindow.Close();
            
        }
        public static void ChangeCultureCurrentWindow(CultureInfo newCulture, MainWindow mainWindow)
        {
            Thread.CurrentThread.CurrentCulture = newCulture;
            Thread.CurrentThread.CurrentUICulture = newCulture;

            var oldWindow = Application.Current.MainWindow;

            MainWindow wnd = new MainWindow();
            wnd = mainWindow;
            Application.Current.MainWindow = wnd;
            wnd.Show();
            Application.Current.MainWindow.Show();
            if (newCulture.ToString() == "ar-AE")
            {
                Application.Current.MainWindow.FlowDirection = FlowDirection.RightToLeft;
            }
            else
            {
                Application.Current.MainWindow.FlowDirection = FlowDirection.LeftToRight;
            }

            oldWindow.Close();
        }
    }
}
