using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSDApp.com.rta.vsd.hh.utilities
{
    class AutoLogOffHelper
    {
        static System.Windows.Forms.Timer _timer = null;
        static private int _logOffTime;
        static public int LogOffTime
        {
            get { return _logOffTime; }
            set { _logOffTime = value; }
        }

        public delegate void MakeAutoLogOff();
        static public event MakeAutoLogOff MakeAutoLogOffEvent;
        public AutoLogOffHelper()
        {

        }
        static public void StartAutoLogoffOption()
        {
            System.Windows.Interop.ComponentDispatcher.ThreadIdle += new EventHandler(DispatcherQueueEmptyHandler);
        }
        static void _timer_Tick(object sender, EventArgs e)
        {
            if (_timer != null)
            {
                System.Windows.Interop.ComponentDispatcher.ThreadIdle -= new EventHandler(DispatcherQueueEmptyHandler);
                _timer.Stop();
                _timer = null;
                if (MakeAutoLogOffEvent != null)
                {
                    MakeAutoLogOffEvent();
                }

            }
        }

        static void DispatcherQueueEmptyHandler(object sender, EventArgs e)
        {
            if (_timer == null)
            {
                _timer = new System.Windows.Forms.Timer();
                _timer.Interval = LogOffTime * 60 * 1000;
                _timer.Tick += new EventHandler(_timer_Tick);
                _timer.Enabled = true;
            }
            else if (_timer.Enabled == false)
            {
                _timer.Enabled = true;
            }
        }

        static public void ResetLogoffTimer()
        {
            if (_timer != null)
            {
                _timer.Enabled = false;
                _timer.Enabled = true;
            }
        }

    }
}
