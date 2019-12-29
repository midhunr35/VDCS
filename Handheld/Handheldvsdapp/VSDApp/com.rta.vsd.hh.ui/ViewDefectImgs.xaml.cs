using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace VSDApp.com.rta.vsd.hh.ui
{
    /// <summary>
    /// Interaction logic for ViewDefectImgs.xaml
    /// created by kashif abbasi
    /// dated 15 nov 2015
    /// </summary>
    public partial class ViewDefectImgs : Window
    {
        #region Data Members
        private string[] _defectAllImg;
        private data.Violation.Defects _selectedDefectInfo;
        int _imgIndex;
        string _defectCode;
        #endregion

        #region Constructors

        public ViewDefectImgs(string[] defectAllImg, data.Violation.Defects selectedDefectInfo)
        {
            InitializeComponent();
            _defectAllImg = defectAllImg;
            _selectedDefectInfo = selectedDefectInfo;
            if (_selectedDefectInfo != null)
                _defectCode = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectCode(_selectedDefectInfo.DefectID);
        }
        public ViewDefectImgs(string[] defectAllImg, DeviceInspectionDefects selectedDefectDevice)
        {
            if (_selectedDefectInfo == null)
            {
                _selectedDefectInfo = new data.Violation.Defects();
            }
            _selectedDefectInfo.DefectID = Convert.ToInt32(selectedDefectDevice.DefectID);
            _selectedDefectInfo.DefectCode = selectedDefectDevice.DefectCode;
            _selectedDefectInfo.DefectName = selectedDefectDevice.DefectName;
            _selectedDefectInfo.DefectValue = selectedDefectDevice.ActualValue;
            _selectedDefectInfo.FineAmount = selectedDefectDevice.FineAmmount;
            _selectedDefectInfo.FineName = selectedDefectDevice.FineName;
            InitializeComponent();
            _defectAllImg = defectAllImg;
            // _selectedDefectInfo = selectedDefectInfo;
            if (_selectedDefectInfo != null)
                _defectCode = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectCode(_selectedDefectInfo.DefectID);
        }
        #endregion

        #region Methods
        void enableDisableNxtPreBtns()
        {
            if (_imgIndex == 0)
            {
                if (_defectAllImg.Count() == 1)
                {
                    btnNextImg.IsEnabled = false;
                    btnPreviousImg.IsEnabled = false;
                }
                else
                {
                    btnNextImg.IsEnabled = true;
                    btnPreviousImg.IsEnabled = false;
                }
            }
            else if (_imgIndex == _defectAllImg.Count() - 1)
            {
                btnNextImg.IsEnabled = false;
                btnPreviousImg.IsEnabled = true;
            }
            else
            {
                btnNextImg.IsEnabled = true;
                btnPreviousImg.IsEnabled = true;
            }
        }

        private void loadButtonImages()
        {
            if (AppProperties.Selected_Resource == "English")
                btnDeletImg.Source = new BitmapImage(new Uri(@"/Images/Icons/Camera/DeleteUp.png", UriKind.Relative));
            else
                btnDeletImg.Source = new BitmapImage(new Uri(@"/Images/Icons/Camera/DeleteUpAr.png", UriKind.Relative));
        }

        #endregion

        #region Events

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loadButtonImages();
            if (_defectAllImg != null && _defectAllImg.Count() > 0)
            {
                _imgIndex = 0;
                imgShowDefect.Source = new BitmapImage(new Uri(_defectAllImg[_imgIndex].ToString(), UriKind.RelativeOrAbsolute));
                lblPathAndName.Content = _defectCode.Trim() + ": " + _selectedDefectInfo.DefectName;// +"    Image Name: " + _defectAllImg[_imgIndex].Substring(_defectAllImg[_imgIndex].LastIndexOf("\\"));//_defectAllImg[_imgIndex];
                enableDisableNxtPreBtns();
            }
            else
            {
                MessageBox.Show("All Images of this defect are deleted!");
                this.Close();
            }

        }


        private void btnPreviousImg_Click(object sender, RoutedEventArgs e)
        {
            if (_imgIndex > 0)
            {
                _imgIndex--;
                imgShowDefect.Source = new BitmapImage(new Uri(_defectAllImg[_imgIndex].ToString(), UriKind.RelativeOrAbsolute));
                lblPathAndName.Content = _defectCode.Trim() + ": " + _selectedDefectInfo.DefectName;// +"    Image Name: " + _defectAllImg[_imgIndex].Substring(_defectAllImg[_imgIndex].LastIndexOf("\\"));//_defectAllImg[_imgIndex];
                enableDisableNxtPreBtns();
            }
        }

        private void btnNextImg_Click(object sender, RoutedEventArgs e)
        {
            if (_imgIndex < _defectAllImg.Count())
            {
                _imgIndex++;
                imgShowDefect.Source = CommonUtils.BitmapFromUri(new Uri(_defectAllImg[_imgIndex].ToString(), UriKind.RelativeOrAbsolute));
                lblPathAndName.Content = _defectCode.Trim() + ": " + _selectedDefectInfo.DefectName;//+ "    Image Name: " + _defectAllImg[_imgIndex].Substring(_defectAllImg[_imgIndex].LastIndexOf("\\") );//_defectAllImg[_imgIndex];
                enableDisableNxtPreBtns();
            }
        }

        private void btnDeletImg_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (WPFMessageBox.Show(new CommonUtils().GetStringValue("Confirmation"), new CommonUtils().GetStringValue("DefImgDelConfirmation"), WPFMessageBoxButtons.YesNo, WPFMessageBoxImage.Question) == WPFMessageBoxResult.Yes)
                {
                    imgShowDefect.Source = null;
                    File.Delete(_defectAllImg[_imgIndex]);
                    _defectAllImg[_imgIndex].Remove(0, _defectAllImg[_imgIndex].Length - 1);
                    //delete that image path from array..
                    _defectAllImg = _defectAllImg.Where(val => val != _defectAllImg[_imgIndex]).ToArray();
                    //Now call load event again so it shows other images.
                    Window_Loaded(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in deleting image " + ex.Message);
            }
        }

        #endregion
    }
}
