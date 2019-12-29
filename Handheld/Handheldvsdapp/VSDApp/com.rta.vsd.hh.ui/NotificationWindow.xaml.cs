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
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows.Forms;
using System.Drawing;
using VSDApp.com.rta.vsd.hh.utilities;

namespace VSDApp.com.rta.vsd.hh.ui
{
    /// <summary>
    /// Interaction logic for NotificationWindow.xaml
    /// </summary>
    public partial class NotificationWindow : Window
    {
        Timer timr;
        public NotificationWindow(String text)
        {
            InitializeComponent();
            timr = new Timer();
            timr.Tick += timr_Tick;
            // tmr.Interval = 5000; 

            timr.Interval = 6000;
            timr.Enabled = true;
            if (AppProperties.Selected_Resource != "English")
            {
                grd.FlowDirection = System.Windows.FlowDirection.RightToLeft;
            }


            Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() =>
            {
                // txtMessage.Text = "";
                txtMessage.Inlines.Add(new Bold(new Run(text)));
                System.Drawing.Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
                // WorkingArea;

                var transform = PresentationSource.FromVisual(this).CompositionTarget.TransformFromDevice;
                var corner = transform.Transform(new System.Windows.Point(workingArea.Right, workingArea.Bottom));

                this.Left = corner.X - this.ActualWidth - 100;
                this.Top = corner.Y - this.ActualHeight;
            }));
        }

        void timr_Tick(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}