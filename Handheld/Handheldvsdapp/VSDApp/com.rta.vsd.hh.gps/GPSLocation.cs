using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using Windows.Devices.Geolocation;
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
using VSDApp.ProgressDialog;
using VSDApp.com.rta.vsd.hh.manager;
using System.ComponentModel;

namespace VSDApp.com.rta.vsd.hh.gps
{
    class GPSLocation : Window
    {

        #region Data Members
		 
	
        Geolocator geolocator;
        Geoposition geoposition;
        MainWindow m_MainWindow;
        BackgroundWorker m_oWorker;
        public delegate void SendHHLocationtoService_delegate(PositionChangedEventArgs args);
        public delegate void CallThreadToGetLatLon_delegate();
        PositionChangedEventArgs currentPosition;
        string previous_latitude = "";
        string previous_longitude = "";
       
       

        #endregion


        #region Functions
        /// <summary>
        /// Constuructor
        /// </summary>
        /// <param name="main"></param>
		public GPSLocation(MainWindow main)
        {
            m_MainWindow = main;
            Button btn = new Button();
            btn.Click += btn_Click;
        }

        
        
        /// <summary>
        /// This function will  call Background Worker theread to get the current postion of HH using GeoLocationAPI
        /// </summary>
        public void SaveHandHeldCurrentGeoCoardinates()
        {
            CommonUtils.WriteLocationLog("Entered in GPSLocation.cs.SaveHandHeldCurrentLocation()");
            m_oWorker = new BackgroundWorker();
            m_oWorker.DoWork += m_oWorker_DoWork;
            m_oWorker.RunWorkerCompleted += m_oWorker_RunWorkerCompleted;
            m_oWorker.WorkerReportsProgress = true;
            m_oWorker.WorkerSupportsCancellation = true;
            m_oWorker.RunWorkerAsync();
            //  GetLatLon();
        }
        /// <summary>
        /// Worker thered Completed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_oWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {


        }
        /// <summary>
        /// Do work will run the call Aynchronously to get Lat/Long
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_oWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (m_oWorker.IsBusy)
                this.Dispatcher.Invoke(new CallThreadToGetLatLon_delegate(this.GetGeoCoardinates), new object[] { });
        }


       public void GetGeoCoardinates()
       {
           //uncommented for  the geo location on 19-08-2017
           btn_Click(null, null);
          
       }

       /// <summary>
       /// Send location to BackendService
       /// </summary>
       /// <param name="args"></param>
       private void SetGeoCoardinates(PositionChangedEventArgs args)
       {

           try
           {
               CommonUtils.WriteLocationLog("GPSLocation.cs.SetLocation()--START--");
               HandHeldGPSLocation location = new HandHeldGPSLocation();
               if ((args.Position != null) && (args.Position.Coordinate != null) && (args.Position.Coordinate.Latitude != null))
               {
                   if ((args.Position.Coordinate.Latitude.ToString() != "") || (args.Position.Coordinate.Latitude.ToString() != "0"))
                   {  
                       location.Latitude = args.Position.Coordinate.Latitude.ToString();
                       previous_latitude = location.Latitude;
                   }
                   else
                   {
                       location.Latitude = (previous_latitude.ToString() != "") ? previous_latitude.ToString(): "";
                   }
                //   location.Latitude = (args.Position.Coordinate.Latitude.ToString() != "") ? args.Position.Coordinate.Latitude.ToString() : "0";
                 //  location.Longitude = (args.Position.Coordinate.Longitude.ToString() != "") ? args.Position.Coordinate.Longitude.ToString() : "0";
                   if ((args.Position.Coordinate.Longitude.ToString() != "") || (args.Position.Coordinate.Longitude.ToString() != "0"))
                   {
                       location.Longitude = args.Position.Coordinate.Longitude.ToString();
                       previous_longitude = location.Longitude;
                   }
                   else
                   {
                       location.Longitude = (previous_longitude.ToString() != "") ? previous_longitude.ToString() : "";
                   }

               }
               else
               {
               }
  
               CommonUtils.WriteLocationLog("GPSLocation.cs..SetLocation() Capruted Latitide:" + location.Latitude + "..Longitude:" + location.Longitude + "\n GPSLocation.cs.SetLocation() ---END---");
               IGPSLocation iGPSManager = (IGPSLocation)GPSLocationManager.GetInstance();

               CommonUtils.WriteLocationLog("Sending info to WebService Capruted Latitide:" + location.Latitude + "..Longitude:" + location.Longitude + "\n GPSLocation.cs.SetLocation() ---END---");
               bool check = false;
               if (!location.Latitude.Equals("") && !location.Longitude.Equals(""))
               {
                   //uncommented the code on 2-08-2017 - To test the Geolocation
                   check = iGPSManager.SubmitHandHeldGeoLocation(location);
               }

               // ProgressDialogResult result = ProgressDialog.ProgressDialog.Execute(this.m_MainWindow, new CommonUtils().GetStringValue("lblSendingLocation"), (bw, we) =>
               //{

               //    check = iGPSManager.SubmitHandHeldLocation(location);

               //    // So this check in order to avoid default processing after the Cancel button has been pressed.
               //    // This call will set the Cancelled flag on the result structure.
               //    ProgressDialog.ProgressDialog.CheckForPendingCancellation(bw, we);

               //}, ProgressDialogSettings.WithSubLabelAndCancel);

               // if (result.Cancelled)
               //     return;
               // else if (result.OperationFailed)
               //     return;
               CommonUtils.WriteLocationLog("Status After Service Call of SubmitHHLocation()@: is Submited properly = " + check);
           }
           catch (System.Exception ex)
           {
               CommonUtils.WriteLocationLog("Exception in GPSLocation.cs.SetLocation()" + ex.Message);
           }

       }
	#endregion
        

       #region Events
       
      

       /// <summary>
        /// Asyn FUnction to get cchange location
        /// </summary>
      async void btn_Click(object sender, RoutedEventArgs e)
        {
            try 
	        {
               
	            geolocator = new Geolocator();
                //Property
                geolocator.ReportInterval = (uint)TimeSpan.FromSeconds(10).TotalMilliseconds;
                geolocator.PositionChanged += geolocator_PositionChanged;

              //  await geolocator.GetGeopositionAsync();
        }
	    catch (System.Exception ex)
	    { 
            CommonUtils.WriteLocationLog("Exception in GPSLocation.cs.GetLatLon()"+ex.Message);
	    }
        

            
        }
       /// <summary>
       /// 
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="args"></param>
        void geolocator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            // Dispatcher.InvokeAsync((() => lbl.Content = "Latitude" + args.Position.Coordinate.Latitude.ToString() + "longitude" + args.Position.Coordinate.Longitude.ToString()), System.Windows.Threading.DispatcherPriority.Normal);

            //Code change done -  " currentposition " line has been commented and args object passed

            CommonUtils.WriteLocationLog("In Geolocator_PositionChanged");
            this.Dispatcher.Invoke(
                               new SendHHLocationtoService_delegate(this.SetGeoCoardinates),
                                // new object[] { currentPosition }
                               new object[] { args });
            
            //  this.Dispatcher.BeginInvoke(

            //if (!AppProperties.Is_HHLocationSubmition)
          //  {
               


           // }


               
        }

       
       #endregion


    }
}
