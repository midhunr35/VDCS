
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
using VSDApp.com.rta.vsd.hh.db;
using VSDApp.com.rta.vsd.hh.utilities;
using VSDApp.WPFMessageBoxControl;
using DirectX.Capture;



namespace VSDApp.com.rta.vsd.hh.ui
{
    /// <summary>
    /// Interaction logic for ComeraWindow.xaml
    /// Developer: Kashif Abbasi(kashifi1@pk.ibm.com)
    /// Dated:  11-Nov-2015
    /// This window will use to take images of defects
    /// </summary>
    public partial class CameraWindow : Window
    {
        #region Data Member
        private data.Violation.Defects _selectedDefect;

        private bool Flash = true;
        private string savePath = "";
        private string[] _strAllImagesOfDefect;
        private int _imgOfDefCount = 0;
        private int _imgThrshHold = 0;

        public DirectX.Capture.Filter Camera;
        public DirectX.Capture.Capture CaptureInfo;
        public DirectX.Capture.Filters CamContainer;
        System.Windows.Controls.Image captureImage;

        #endregion
        #region Constructor
        public CameraWindow(data.Violation.Defects selectedDefect, string[] strAllImagesOfDefect)
        {
            _selectedDefect = selectedDefect;
            int defImgCount = 0;
            if (strAllImagesOfDefect != null)
                defImgCount = strAllImagesOfDefect.Count();

            if (defImgCount > 0)
            {
                _strAllImagesOfDefect = strAllImagesOfDefect;
                _imgOfDefCount = _strAllImagesOfDefect.Count();
            }


            if (Properties.Settings.Default.ImagesPerDefect != null)
                _imgThrshHold = Convert.ToInt16(Properties.Settings.Default.ImagesPerDefect);
            InitializeComponent();
        }
        /// <summary>
        /// Loaded from 2nd search window
        /// </summary>
        /// <param name="selectedDefectDevice"></param>
        /// <param name="strAllImagesOfDefect"></param>
        public CameraWindow(DeviceInspectionDefects selectedDefectDevice, string[] strAllImagesOfDefect)
        {
            if (_selectedDefect == null)
            {
                _selectedDefect = new data.Violation.Defects();
            }
            _selectedDefect.DefectID = Convert.ToInt32(selectedDefectDevice.DefectID);
            _selectedDefect.DefectCode = selectedDefectDevice.DefectCode;
            _selectedDefect.DefectName = selectedDefectDevice.DefectName;
            _selectedDefect.DefectValue = selectedDefectDevice.ActualValue;
            _selectedDefect.FineAmount = selectedDefectDevice.FineAmmount;
            _selectedDefect.FineName = selectedDefectDevice.FineName;
            int defImgCount = 0;
            if (strAllImagesOfDefect != null)
                defImgCount = strAllImagesOfDefect.Count();

            if (defImgCount > 0)
            {
                _strAllImagesOfDefect = strAllImagesOfDefect;
                _imgOfDefCount = _strAllImagesOfDefect.Count();
            }


            if (Properties.Settings.Default.ImagesPerDefect != null)
                _imgThrshHold = Convert.ToInt16(Properties.Settings.Default.ImagesPerDefect);
            InitializeComponent();
        }
        #endregion

        #region Methods
        private void populateCamerasInCmbBx()
        {
            try
            {
                /*
                Camera[] cameras = Camera.Cameras;
                cmbBxActiveCamera.Items.Clear();

                foreach (Camera camera in cameras)
                {
                    cmbBxActiveCamera.Items.Add(camera.FriendlyName);

                }
                /*
                /*
                if (cameras.Length > 1)
                    cmbBxActiveCamera.SelectedIndex = 1;
                else
                   cmbBxActiveCamera.SelectedIndex = 0;*/
            }
            catch (Exception ex)
            {
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), new CommonUtils().GetStringValue("CameraError"), WPFMessageBoxImage.Error);
                //  WPFMessageBox.Show("Error", "getting error to load camera" + ex.Message, ex.StackTrace,WPFMessageBoxImage.Error);
                // WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), new CommonUtils().GetStringValue("CameraError"), WPFMessageBoxImage.Error);
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string createDirectoriesAndFileName()
        {
            //path should be C:\RTA_VSD_IMAGES\{MONYYYY}\{DD}\{VIOLATION_ID}\{DEFECT_ID}.jpg
            string strPath = Properties.Settings.Default.violationImagesPath;
            try
            {

                // Determine whether the directory exists.
                if (!Directory.Exists(strPath))
                {
                    Directory.CreateDirectory(strPath);
                }

                strPath += @"\" + DateTime.Now.Date.ToString("MMM") + DateTime.Now.Year;
                if (!Directory.Exists(strPath))
                {
                    Directory.CreateDirectory(strPath);
                }

                strPath += @"\" + DateTime.Now.Date.ToString("ddMMyyyy");
                if (!Directory.Exists(strPath))
                {
                    Directory.CreateDirectory(strPath);
                }

                strPath += @"\" + AppProperties.vehicle.Country.Replace(" ", "") + "_" + AppProperties.vehicle.PlateNumber.Trim() + "_" +
                            AppProperties.vehicle.PlateCategory.Replace(" ", "") + "_" + AppProperties.vehicle.PlateCode.Replace(" ", "");
                if (!Directory.Exists(strPath))
                {
                    Directory.CreateDirectory(strPath);
                }

                strPath += @"\" + _selectedDefect.DefectID + "_" + DateTime.Now.ToLongTimeString().Replace(":", "") + ".jpg";
                return strPath;
            }
            catch (Exception e)
            {
                App.VSDLog.Info("Fail to creat diractory" + e.Message);
                // MessageBox.Show("Fail to creat diractory" + e.Message);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), new CommonUtils().GetStringValue("NoDevicesDetected"), WPFMessageBoxImage.Error);
                return "";
            }
        }
        /// <summary>
        ///
        /// </summary>
        private void resizeWindowsControl()
        {
            if (new CommonUtils().GetScreenOrientation() == AppProperties.LandScapetMode)
            {
                this.Width = 860;
                this.Height = 600;
                winFrmHostPnlCmraView.Width = 840;
                winFrmHostPnlCmraView.Height = 450;
                this.Title = "Landscape Mod";
                this.VerticalAlignment = VerticalAlignment.Top;
                this.HorizontalAlignment = HorizontalAlignment.Left;
                //if (ActiveCamera.IsPlaying)
                //ActiveCamera.CameraViewOrientation = RotateFlipType.RotateNoneFlipX;
                //Window.HorizontalAlignmentProperty = AlignmentX.Center;
                //Window.VerticalAlignmentProperty = AlignmentY.Center;
            }
            else
            {
                this.Width = 650;
                this.Height = 900;
                winFrmHostPnlCmraView.Width = 580;
                winFrmHostPnlCmraView.Height = 750;
                this.Title = "Portrait Mod";
                this.VerticalAlignment = VerticalAlignment.Top;
                this.HorizontalAlignment = HorizontalAlignment.Left;
                //if (ActiveCamera.IsPlaying)
                //ActiveCamera.CameraViewOrientation = RotateFlipType.Rotate270FlipNone;

            }
            this.UpdateLayout();
        }

        private void loadButtonsImgs()
        {

        }
        #endregion

        #region Events

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            try
            {
                string defect_Code = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectCode(_selectedDefect.DefectID);
                lblDefectDetails.Content = defect_Code.Trim() + " :  " + _selectedDefect.DefectName;
                // ActiveCamera = cameras[0];
                // ActiveCamera.StartPreview(ActiveCamera.CameraFormats[0], pnlCameraView.Handle);
                //  populateCamerasInCmbBx();
                // chkBxFlash_Click(sender, e);


                ////New API Changes

                CamContainer = new DirectX.Capture.Filters();


                int no_of_cam = CamContainer.VideoInputDevices.Count;

                for (int i = 0; i < no_of_cam; i++)
                {

                   
                    Camera = CamContainer.VideoInputDevices[i];

                    // initialize the Capture using the video input device
                    CaptureInfo = new DirectX.Capture.Capture(Camera, null);

                    // set the input video preview window 
                    CaptureInfo.PreviewWindow = this.pnlCameraView;

                    // Capture the frame from input device 
                    CaptureInfo.Cue();
                    CaptureInfo.Start();
                   
                    break;
                }

            }
            catch (Exception ex)
            {
                App.VSDLog.Info("erro on window_loaded event\n" + ex.Message + "\n" + ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), new CommonUtils().GetStringValue("NoDevicesDetected"), ex.Message, WPFMessageBoxImage.Error);

            }

            /*
            try
            {
                loadButtonsImgs();
                Camera[] cameras = Camera.Cameras;
                if (cameras == null || cameras.Length == 0)
                {
                    // No devices so end the application
                    //  MessageBox.Show("No Devices Detected.");
                    WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), new CommonUtils().GetStringValue("NoDevicesDetected"), WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
                    this.Close();
                    return;
                }
                string defect_Code = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectCode(_selectedDefect.DefectID);
                lblDefectDetails.Content = defect_Code.Trim() + " :  " + _selectedDefect.DefectName;
                ActiveCamera = cameras[0];
                ActiveCamera.StartPreview(ActiveCamera.CameraFormats[0], pnlCameraView.Handle);
                populateCamerasInCmbBx();
                chkBxFlash_Click(sender, e);
            }
            catch (Exception ex)
            {
                App.VSDLog.Info("erro on window_loaded event\n" + ex.Message + "\n" + ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), new CommonUtils().GetStringValue("NoDevicesDetected"), ex.Message, WPFMessageBoxImage.Error);
               
            } */
        }
        public void RefreshImage(System.Windows.Forms.PictureBox frame)
        {
            try
            {
                System.Drawing.Image img = frame.Image;
                if (savePath != "")
                {
                    img.Save(savePath);
                }
            }
            catch (Exception ex)
            {
                App.VSDLog.Info("erro on window_loaded event\n" + ex.Message + "\n" + ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.Message, WPFMessageBoxImage.Error);

            }

        }
        private void btnCaptureImg_Click(object sender, EventArgs e)
        {
            try
            {
                if (_imgOfDefCount < _imgThrshHold)
                {
                    // ActiveCamera.PauseVideo();
                    //  WPFMessageBoxResult res = WPFMessageBox.Show(new CommonUtils().GetStringValue("Confirmation"), new CommonUtils().GetStringValue("DefImgSaveConfirmation"), WPFMessageBoxButtons.YesNo, WPFMessageBoxImage.Question);
                    // if (res == WPFMessageBoxResult.Yes)
                    //{
                    savePath = createDirectoriesAndFileName();

                    // Bitmap bmp = new Bitmap(pnlCameraView.Width, pnlCameraView.Height);
                    // pnlCameraView.DrawToBitmap(bmp, new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height));



                    //     bmp2.Save(@"C:\MyPanelImage.jpg");

                    if (!savePath.Equals(""))
                    {
                        /*
                        this.btnCaptureImg.Visibility = System.Windows.Visibility.Collapsed;
                        this.btnExit.Visibility = System.Windows.Visibility.Collapsed;
                      
                        ScreenCapture sc = new ScreenCapture();
                        System.Drawing.Image j = sc.CaptureScreen();                       
                        IntPtr windowHandle = new System.Windows.Interop.WindowInteropHelper(this).Handle;
                        sc.CaptureWindowToFile(windowHandle,savePath, System.Drawing.Imaging.ImageFormat.Png);
                        */
                        System.Drawing.Rectangle rect = pnlCameraView.RectangleToScreen(pnlCameraView.Bounds);

                        Bitmap bmp2 = new Bitmap(rect.Width, rect.Height);
                        Graphics g = Graphics.FromImage(bmp2);

                        g.CopyFromScreen(rect.Left, rect.Top, 0, 0, bmp2.Size, CopyPixelOperation.SourceCopy);


                        bmp2.Save(savePath);

                        _imgOfDefCount++;
                        WPFMessageBox.Show(new CommonUtils().GetStringValue("Information"), new CommonUtils().GetStringValue("ImageSuccess"), WPFMessageBoxButtons.OK, WPFMessageBoxImage.Information);

                        CaptureInfo.Stop();
                        this.Close();
                    }


                    else
                    {
                        this.btnCaptureImg.Visibility = System.Windows.Visibility.Visible;
                        this.btnExit.Visibility = System.Windows.Visibility.Visible;
                        //  MessageBox.Show("Can't save image. Path doesn't exist", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        WPFMessageBox.Show(new CommonUtils().GetStringValue("Information"), new CommonUtils().GetStringValue("ImageFail"), WPFMessageBoxButtons.OK, WPFMessageBoxImage.Information);
                    }


                    // }
                    // ActiveCamera.ResumeVideo();
                }
                else
                {
                    WPFMessageBox.Show(new CommonUtils().GetStringValue("Information"), new CommonUtils().GetStringValue("DefectImgCount"), WPFMessageBoxButtons.OK, WPFMessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {

                App.VSDLog.Info("Getting error on btnCaptureImg: " + ex.Message);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), new CommonUtils().GetStringValue("CameraError"), WPFMessageBoxImage.Error);
                // Camera.Deinitialize();
                return;
            }
        }

        private void chkBxFlash_Click(object sender, RoutedEventArgs e)
        {
            /*
            if (chkBxFlash.IsChecked == true)
            {
                imgCamFlash.Source = new BitmapImage(new Uri(@"/Images/Icons/Camera/Cam_FlashOn.jpg", UriKind.Relative));
                imgCamFlash.ToolTip = "Clik to off flash";
                // chkBxFlash.Content = "Flash Off";
                Flash = true;
            }
            else
            {
                imgCamFlash.Source = new BitmapImage(new Uri(@"/Images/Icons/Camera/Cam_FlashOff.jpg", UriKind.Relative));
                imgCamFlash.ToolTip = "Clik to On flash";
                
                Flash = false;
            }*/
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            // CaptureInfo.CaptureFrame();

            if (CaptureInfo != null) CaptureInfo.Stop();


            this.Close();
            /*
            Camera.Deinitialize();
            this.Close();
            */
        }

        private void cmbBxActiveCamera_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                /*
                if (ActiveCamera != null)
                {
                    if (ActiveCamera == Camera.Cameras[0])
                    {
                        ActiveCamera.EndPreview();
                        ActiveCamera = Camera.Cameras[1];
                        ActiveCamera.StartPreview(Camera.Cameras[1].CameraFormats[0], pnlCameraView.Handle);

                    }
                    else
                    {
                        ActiveCamera.EndPreview();
                        ActiveCamera = Camera.Cameras[0];
                        ActiveCamera.StartPreview(Camera.Cameras[0].CameraFormats[0], pnlCameraView.Handle);

                    }
                }*/
            }
            catch (Exception ex)
            {
                //  ActiveCamera.ResumeVideo();
                App.VSDLog.Info("Getting error on Camera selected index change: " + ex.Message);

                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), new CommonUtils().GetStringValue("CameraError"), WPFMessageBoxImage.Error);
                return;
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            resizeWindowsControl();
            // populateCamerasInCmbBx();

        }

        private void btnCaptureImg_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void btnExit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }


        #endregion

        private void Window_Closed(object sender, EventArgs e)
        {
            if (CaptureInfo != null) CaptureInfo.Stop();


            this.Close();
        }




    }


}
