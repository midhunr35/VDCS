using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using VSDApp.com.rta.vsd.hh.data;
using VSDApp.com.rta.vsd.hh.db;
using VSDApp.com.rta.vsd.hh.manager;
using VSDApp.com.rta.vsd.hh.utilities;
using VSDApp.ProgressDialog;
using VSDApp.WPFMessageBoxControl;

namespace VSDApp.com.rta.vsd.hh.ui
{
    /// <summary>
    /// Interaction logic for ucDriverInspectionDefectScreen.xaml
    /// </summary>
    public partial class ucDriverInspectionDefectScreen : UserControl
    {
        List<string> FuleEmissionList;
        List<DeviceInspectionDefects> addedDefectDeviceInspection = new List<DeviceInspectionDefects>();
        DataTable dtFuleEmission;
        DataTable dtVehSubCat;
        DataTable dtDeviceInspParames;
        MainWindow m_MainWindow;
        string vehicleSubCat = string.Empty;
        private string[] _strAllImagesOfDefect;
        DeviceInspectionDefects _selectedDefectInfo;
        string vehicleCategory = string.Empty;



        public ucDriverInspectionDefectScreen(MainWindow mainWnd)
        {
            InitializeComponent();
            m_MainWindow = mainWnd;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.DigitSubstitution = System.Globalization.DigitShapes.None;
                LoadFuelEmissionData();
                // GetVehInspParams();
                LoadNoofAxels();
                LoadVehicleSubCat();
                if (AppProperties.vehicle != null && AppProperties.vehicle.DeviceInspectionparm != null)
                {
                    this.txtGrossWeightActual.Text = AppProperties.vehicle.DeviceInspectionparm.CarryWeight;
                }
                if (AppProperties.vehicle.VehicleCategory != null || AppProperties.vehicle.VehicleCategory != "")
                    vehicleCategory = AppProperties.vehicle.VehicleCategory;
                else
                    vehicleCategory = "Heavy Vehicle";

            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }
        public void GetVehInspParams()
        {

        }

        public void LoadNoofAxels()
        {
            try
            {
                if (cmboxNoofAxels.Items.Count > 0)
                    cmboxNoofAxels.Items.Clear();
                for (int i = 1; i <= Convert.ToInt32(AppProperties.default_no_axels); i++)
                {
                    if (i % 2 == 0)
                    {
                        cmboxNoofAxels.Items.Add(i.ToString());
                    }
                }

                cmboxNoofAxels.SelectedItem = AppProperties.default_no_axels;

            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }

        }

        public void LoadVehicleSubCat()
        {
            try
            {
                dtVehSubCat = ((IDBDataLoad)DBDataLoadManager.GetInstance()).LoadDeviceInspSubVat();
                if (dtVehSubCat == null)
                    return;
                if (cmboxSubCat.Items.Count > 0)
                {
                    cmboxSubCat.Items.Clear();
                }
                foreach (DataRow dr in dtVehSubCat.Rows)
                {
                    if (AppProperties.Selected_Resource == "English")
                    {
                        if (Convert.ToString(dr["VEHICLE_CATEGORY_Name"]) != null)
                        {
                            cmboxSubCat.Items.Add(Convert.ToString(dr["VEHICLE_CATEGORY_Name"]));
                        }
                    }
                    else
                    {
                        if (Convert.ToString(dr["VEHICLE_CATEGORY_Name_Ar"]) != null)
                        {
                            cmboxSubCat.Items.Add(Convert.ToString(dr["VEHICLE_CATEGORY_Name_Ar"]));
                        }
                    }

                }
                if (cmboxSubCat.Items.Count > 0)
                {
                    cmboxSubCat.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }
        public void LoadFuelEmissionData()
        {

            try
            {
                FuleEmissionList = new List<string>();
                dtFuleEmission = ((IDBDataLoad)DBDataLoadManager.GetInstance()).LoadFuelEmissionType();
                foreach (DataRow dr in dtFuleEmission.Rows)
                {
                    if (AppProperties.Selected_Resource == "English")
                    {
                        if (dr["VEHICLE_FUEL_TYPE"] != null)
                        {
                            FuleEmissionList.Add(dr["VEHICLE_FUEL_TYPE"].ToString().Trim());
                        }
                    }
                    else
                    {
                        if (dr["VEHICLE_FUEL_TYPE_AR"] != null)
                        {
                            FuleEmissionList.Add(dr["VEHICLE_FUEL_TYPE_AR"].ToString().Trim());
                        }
                        else
                        {
                            if (dr["VEHICLE_FUEL_TYPE"] != null)
                            {
                                FuleEmissionList.Add(dr["VEHICLE_FUEL_TYPE"].ToString());
                            }
                        }
                    }
                }

                if (cmboxFuelType.Items.Count > 0)
                {
                    cmboxFuelType.Items.Clear();
                }
                foreach (string str in FuleEmissionList)
                {
                    cmboxFuelType.Items.Add(str);
                }
                if (cmboxFuelType.Items.Count > 0)
                {
                    cmboxFuelType.SelectedIndex = 0;
                }


            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }



        private void expender_Dimension_Collapsed(object sender, RoutedEventArgs e)
        {

        }

        private void txtBoxLength_GotFocus(object sender, RoutedEventArgs e)
        {
            CommonUtils.ShowKeyBoard();
            System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.DigitSubstitution = System.Globalization.DigitShapes.None;
            this.UpdateLayout();
        }

        private void txtBoxLength_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.DigitSubstitution = System.Globalization.DigitShapes.None;
                CommonUtils.CLoseKeyBoard();
                /*
                 * Emission Levels Exceeding Allowable Values :          7.3.2
                    Overloading                               :          12.1.2
                    Vehicle Length                            :          12.2.3
                    Width                                     :          12.2.2
                    Heigh                                     :          12.2.1
                 * 
                 row[0] = rs["Defect_ID"].ToString();
                row[1] = rs["Fine_Name"].ToString();
                row[2] = rs["Fine_Name_a"].ToString();
                row[3] = rs["Fine_Amount"].ToString();
                row[4] = rs["Is_Disabled"].ToString();
                row[5] = rs["Fine_ID"].ToString();
                */


                DeviceInspectionDefects defect = new DeviceInspectionDefects();
                string actual_Length = txtBoxLengthActual.Text;
                string enterd_Length = txtBoxLengthActualEnterd.Text;
                string Fine_Name = String.Empty;
                string Fine_Ammount = String.Empty;
                string Fine_ID = String.Empty;
                if ((actual_Length != null) && (enterd_Length != null) && (enterd_Length != ""))
                {
                    if (Convert.ToDouble(enterd_Length) > Convert.ToDouble(actual_Length))
                    {
                        if (AppProperties.Selected_Resource == "English")
                        {
                            lblLengthStatus.Visibility = Visibility.Visible;
                            lblLengthStatus.Content = new CommonUtils().GetStringValue("lblFail");
                            lblLengthStatus.Foreground = Brushes.Red;

                            string defect_Code = AppProperties.Defect_Length_Code;
                            int defect_ID = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectIDByCode(defect_Code);
                            string defect_Name = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectNameByID(defect_Code);
                            string[] array_Fine_Info = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectsFineInfo(defect_ID.ToString(), vehicleCategory);
                            if (array_Fine_Info != null && array_Fine_Info.Length > 0)
                            {
                                // string defect_ID = array_Fine_Info[0];
                                Fine_Name = array_Fine_Info[1];
                                Fine_Ammount = array_Fine_Info[3];
                                Fine_ID = array_Fine_Info[5];
                            }
                            defect.DefectCode = defect_Code;
                            defect.DefectID = defect_ID.ToString();
                            defect.DefectName = defect_Name;
                            defect.FineAmmount = Fine_Ammount;
                            defect.FineName = Fine_Name;
                            defect.ActualValue = enterd_Length;

                            //addedDefectDeviceInspection.RemoveAll((x) => x.Contains(defect_Code.ToString()));

                            addedDefectDeviceInspection.RemoveAll((x) => x.DefectCode.Contains(defect_Code));

                            addedDefectDeviceInspection.Add(defect);
                            grdAddedDefects.ItemsSource = null;
                            grdAddedDefects.ItemsSource = addedDefectDeviceInspection;

                        }
                        else
                        {
                            lblLengthStatus.Visibility = Visibility.Visible;
                            lblLengthStatus.Content = new CommonUtils().GetStringValue("lblFail");
                            lblLengthStatus.Foreground = Brushes.Red;

                            string defect_Code = AppProperties.Defect_Length_Code;
                            int defect_ID = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectIDByCode(defect_Code);
                            string defect_Name = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectNameArByID(defect_Code);
                            string[] array_Fine_Info = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectsFineInfo(defect_ID.ToString(), vehicleCategory);
                            if (array_Fine_Info != null && array_Fine_Info.Length > 0)
                            {
                                // string defect_ID = array_Fine_Info[0];
                                Fine_Name = array_Fine_Info[2];
                                Fine_Ammount = array_Fine_Info[3];
                                Fine_ID = array_Fine_Info[5];
                            }
                            defect.DefectCode = defect_Code;
                            defect.DefectID = defect_ID.ToString();
                            defect.DefectName = defect_Name;
                            defect.FineAmmount = Fine_Ammount;
                            defect.FineName = Fine_Name;
                            defect.ActualValue = enterd_Length;

                            //addedDefectDeviceInspection.RemoveAll((x) => x.Contains(defect_Code.ToString()));

                            addedDefectDeviceInspection.RemoveAll((x) => x.DefectCode.Contains(defect_Code));

                            addedDefectDeviceInspection.Add(defect);
                            grdAddedDefects.ItemsSource = null;
                            grdAddedDefects.ItemsSource = addedDefectDeviceInspection;

                        }
                    }
                    else
                    {
                        lblLengthStatus.Content = new CommonUtils().GetStringValue("lblPass");

                        lblLengthStatus.Visibility = Visibility.Visible;
                        lblLengthStatus.Foreground = Brushes.Green;
                        addedDefectDeviceInspection.RemoveAll((x) => x.DefectCode.Contains(AppProperties.Defect_Length_Code));
                        grdAddedDefects.ItemsSource = null;
                        grdAddedDefects.ItemsSource = addedDefectDeviceInspection;
                        grdAddedDefects.UpdateLayout();
                    }
                }
                else
                {
                    lblLengthStatus.Content = "";
                    addedDefectDeviceInspection.RemoveAll((x) => x.DefectCode.Contains(AppProperties.Defect_Length_Code));
                    grdAddedDefects.ItemsSource = null;
                    grdAddedDefects.ItemsSource = addedDefectDeviceInspection;
                    grdAddedDefects.UpdateLayout();
                }

            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void txtBoxLength_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Return)
                {
                    new CommonUtils().ChangeControlFocous(e);

                }
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void txtBoxLength_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txtBoxLength_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void cmboxFuelType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if ((string)cmboxFuelType.SelectedItem != "" && cmboxFuelType.SelectedItem != null)
                {

                    string selectedType = (string)cmboxFuelType.SelectedItem;
                    string selectedEmissionPerc = string.Empty;
                    DataRow[] filteredRows = dtFuleEmission.Select("VEHICLE_FUEL_TYPE LIKE '%" + selectedType + "%' OR VEHICLE_FUEL_TYPE_AR LIKE '%" + selectedType + "%'");

                    if (filteredRows != null && filteredRows.Count() > 0)
                    {
                        selectedEmissionPerc = filteredRows[0]["VEH_FUEL_MAX_EMISSION_PERCENT"].ToString();
                    }
                    txtEmissionPercActual.Text = selectedEmissionPerc;
                }
                else
                {
                    txtEmissionPercActual.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void grdAddedDefects_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //check Is this  defect have an image in following diractor..if yes than show its thumbnail below grid
            string strPath = Properties.Settings.Default.violationImagesPath; //@"C:\RTA_VSD_IMAGES";
            _strAllImagesOfDefect = null;
            _selectedDefectInfo = ((DeviceInspectionDefects)grdAddedDefects.SelectedItem);
            imgDefectImageThumbnil.Source = null;
            lblDefImgCount.Content = "";
            try
            {
                // Determine whether the directory exists.
                strPath += @"\" + DateTime.Now.Date.ToString("MMM") + DateTime.Now.Year;
                strPath += @"\" + DateTime.Now.Date.ToString("ddMMyyyy");
                strPath += @"\" + AppProperties.vehicle.Country.Replace(" ", "") + "_" + AppProperties.vehicle.PlateNumber.Trim() + "_" +
                           AppProperties.vehicle.PlateCategory.Replace(" ", "") + "_" + AppProperties.vehicle.PlateCode.Replace(" ", "");
                if (Directory.Exists(strPath))
                {
                    if (grdAddedDefects.SelectedItem != null)
                    {
                        string[] strAllImages = Directory.GetFiles(strPath, ((DeviceInspectionDefects)grdAddedDefects.SelectedItem).DefectID + "_*.jpg");

                        if (strAllImages != null && strAllImages.Count() > 0)
                        {
                            imgDefectImageThumbnil.Source = CommonUtils.BitmapFromUri(new Uri(strAllImages[0].ToString(), UriKind.RelativeOrAbsolute));
                            lblDefImgCount.Content = strAllImages.Count().ToString();
                            _strAllImagesOfDefect = strAllImages;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                App.VSDLog.Info("fail to get image for selected defect" + ex.Message);
                //MessageBox.Show("Fail to creat diractory" + e.Message);

            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (grdAddedDefects.SelectedItem != null)
                {
                    if (((VSDApp.com.rta.vsd.hh.data.DeviceInspectionDefects)(grdAddedDefects.SelectedItem)).DefectCode.Equals(AppProperties.Defect_Length_Code))
                    {
                        this.txtBoxLengthActualEnterd.Text = "";
                        lblLengthStatus.Content = "";
                        return;
                    }
                    if (((VSDApp.com.rta.vsd.hh.data.DeviceInspectionDefects)(grdAddedDefects.SelectedItem)).DefectCode.Equals(AppProperties.Defect_Width_Code))
                    {
                        this.txtWidthEntered.Text = "";
                        lblWidthStatus.Content = "";
                        return;
                    }
                    if (((VSDApp.com.rta.vsd.hh.data.DeviceInspectionDefects)(grdAddedDefects.SelectedItem)).DefectCode.Equals(AppProperties.Defect_Height_Code))
                    {
                        this.txtHeightEntered.Text = "";
                        lblHeightStatus.Content = "";
                        return;
                    }
                    if (((VSDApp.com.rta.vsd.hh.data.DeviceInspectionDefects)(grdAddedDefects.SelectedItem)).DefectCode.Equals(AppProperties.Defect_Emission_Code))
                    {
                        this.txtEmissionPercEntered.Text = "";
                        lblEmissionStatus.Content = "";
                        return;
                    }
                    if (((VSDApp.com.rta.vsd.hh.data.DeviceInspectionDefects)(grdAddedDefects.SelectedItem)).DefectCode.Equals(AppProperties.Defect_Emission_Code))
                    {
                        this.txtGrossWeightEntered.Text = "";
                        lblGrossWeightStatus.Content = "";
                        return;
                    }
                }

                /*
                                var Length_defect = addedDefectDeviceInspection.FirstOrDefault((x) => x.DefectCode.Contains(AppProperties.Defect_Length_Code));
                                if (Length_defect != null)
                                {
                    
                                }
                                var Width_defect = addedDefectDeviceInspection.FirstOrDefault((x) => x.DefectCode.Contains(AppProperties.Defect_Width_Code));
                                if (Width_defect != null)
                                {
                   
                                }
                                var Height_defect = addedDefectDeviceInspection.FirstOrDefault((x) => x.DefectCode.Contains(AppProperties.Defect_Height_Code));
                                if (Height_defect != null)
                                {
                   
                                }*/

            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }

        }

        private void imgTakeDefectImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                int imgThrshHold = 2, defImgCount = 0;
                if (Properties.Settings.Default.ImagesPerDefect != null)
                    imgThrshHold = Convert.ToInt16(Properties.Settings.Default.ImagesPerDefect);
                if (_strAllImagesOfDefect != null)
                    defImgCount = _strAllImagesOfDefect.Count();

                if (defImgCount < imgThrshHold)
                {
                    if (grdAddedDefects.SelectedItem == null)
                        return;
                    // data.Violation.Defects selectedDefect = new Violation.Defects();
                    DeviceInspectionDefects selectedDefect = (DeviceInspectionDefects)grdAddedDefects.SelectedItem;
                    if (selectedDefect == null)
                        return;

                    CameraWindow cmraWindow = new CameraWindow(selectedDefect, _strAllImagesOfDefect);
                    cmraWindow.ShowDialog();
                    grdAddedDefects_SelectionChanged(sender, null);
                }
                else
                {
                    WPFMessageBox.Show(new CommonUtils().GetStringValue("Information"), new CommonUtils().GetStringValue("DefectImgCount"), WPFMessageBoxButtons.OK, WPFMessageBoxImage.Information);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Problem to go on camera screen");
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void txtWidthEntered_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.DigitSubstitution = System.Globalization.DigitShapes.None;
                vehicleSubCat = "";
                DeviceInspectionDefects defect = new DeviceInspectionDefects();
                string actual_Width = txtWidthActual.Text;
                string enterd_Width = txtWidthEntered.Text;
                string Fine_Name = String.Empty;
                string Fine_Ammount = String.Empty;
                string Fine_ID = String.Empty;
                if ((actual_Width != null) && (enterd_Width != null) && (enterd_Width != ""))
                {
                    if (Convert.ToDouble(enterd_Width) > Convert.ToDouble(actual_Width))
                    {
                        if (AppProperties.Selected_Resource == "English")
                        {
                            lblWidthStatus.Visibility = Visibility.Visible;
                            lblWidthStatus.Content = new CommonUtils().GetStringValue("lblFail");
                            lblWidthStatus.Foreground = Brushes.Red;

                            string defect_Code = AppProperties.Defect_Width_Code;
                            int defect_ID = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectIDByCode(defect_Code);
                            string defect_Name = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectNameByID(defect_Code);
                            string[] array_Fine_Info = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectsFineInfo(defect_ID.ToString(), vehicleCategory);
                            if (array_Fine_Info != null && array_Fine_Info.Length > 0)
                            {
                                // string defect_ID = array_Fine_Info[0];
                                Fine_Name = array_Fine_Info[1];
                                Fine_Ammount = array_Fine_Info[3];
                                Fine_ID = array_Fine_Info[5];
                            }
                            defect.DefectCode = defect_Code;
                            defect.DefectID = defect_ID.ToString();
                            defect.DefectName = defect_Name;
                            defect.FineAmmount = Fine_Ammount;
                            defect.FineName = Fine_Name;
                            defect.ActualValue = enterd_Width;

                            //addedDefectDeviceInspection.RemoveAll((x) => x.Contains(defect_Code.ToString()));

                            addedDefectDeviceInspection.RemoveAll((x) => x.DefectCode.Contains(defect_Code));

                            addedDefectDeviceInspection.Add(defect);
                            grdAddedDefects.ItemsSource = null;
                            grdAddedDefects.ItemsSource = addedDefectDeviceInspection;

                        }
                        else
                        {
                            lblWidthStatus.Visibility = Visibility.Visible;
                            lblWidthStatus.Content = new CommonUtils().GetStringValue("lblFail");

                            lblWidthStatus.Foreground = Brushes.Red;

                            string defect_Code = AppProperties.Defect_Width_Code;
                            int defect_ID = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectIDByCode(defect_Code);
                            string defect_Name = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectNameArByID(defect_Code);
                            string[] array_Fine_Info = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectsFineInfo(defect_ID.ToString(), vehicleCategory);
                            if (array_Fine_Info != null && array_Fine_Info.Length > 0)
                            {
                                // string defect_ID = array_Fine_Info[0];
                                Fine_Name = array_Fine_Info[2];
                                Fine_Ammount = array_Fine_Info[3];
                                Fine_ID = array_Fine_Info[5];
                            }
                            defect.DefectCode = defect_Code;
                            defect.DefectID = defect_ID.ToString();
                            defect.DefectName = defect_Name;
                            defect.FineAmmount = Fine_Ammount;
                            defect.FineName = Fine_Name;
                            defect.ActualValue = enterd_Width;

                            //addedDefectDeviceInspection.RemoveAll((x) => x.Contains(defect_Code.ToString()));

                            addedDefectDeviceInspection.RemoveAll((x) => x.DefectCode.Contains(defect_Code));

                            addedDefectDeviceInspection.Add(defect);
                            grdAddedDefects.ItemsSource = null;
                            grdAddedDefects.ItemsSource = addedDefectDeviceInspection;

                        }
                    }
                    else
                    {
                        lblWidthStatus.Content = new CommonUtils().GetStringValue("lblPass");
                        lblWidthStatus.Visibility = Visibility.Visible;
                        lblWidthStatus.Foreground = Brushes.Green;
                        addedDefectDeviceInspection.RemoveAll((x) => x.DefectCode.Contains(AppProperties.Defect_Width_Code));
                        grdAddedDefects.ItemsSource = null;
                        grdAddedDefects.ItemsSource = addedDefectDeviceInspection;
                        grdAddedDefects.UpdateLayout();
                    }
                }
                else
                {
                    lblWidthStatus.Content = "";
                    addedDefectDeviceInspection.RemoveAll((x) => x.DefectCode.Contains(AppProperties.Defect_Width_Code));
                    grdAddedDefects.ItemsSource = null;
                    grdAddedDefects.ItemsSource = addedDefectDeviceInspection;
                    grdAddedDefects.UpdateLayout();
                }

            }

            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void txtHeightEntered_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void txtHeightEntered_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.DigitSubstitution = System.Globalization.DigitShapes.None;
                CommonUtils.CLoseKeyBoard();
                vehicleSubCat = "";
                DeviceInspectionDefects defect = new DeviceInspectionDefects();
                string actual_Height = txtHeightActual.Text;
                string enterd_Height = txtHeightEntered.Text;
                string Fine_Name = String.Empty;
                string Fine_Ammount = String.Empty;
                string Fine_ID = String.Empty;
                if ((actual_Height != null) && (enterd_Height != null) && (enterd_Height != ""))
                {
                    if (Convert.ToDouble(enterd_Height) > Convert.ToDouble(actual_Height))
                    {
                        if (AppProperties.Selected_Resource == "English")
                        {
                            lblHeightStatus.Visibility = Visibility.Visible;
                            lblHeightStatus.Content = new CommonUtils().GetStringValue("lblFail");
                            lblHeightStatus.Foreground = Brushes.Red;

                            string defect_Code = AppProperties.Defect_Height_Code;
                            int defect_ID = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectIDByCode(defect_Code);
                            string defect_Name = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectNameByID(defect_Code);
                            string[] array_Fine_Info = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectsFineInfo(defect_ID.ToString(), vehicleCategory);
                            if (array_Fine_Info != null && array_Fine_Info.Length > 0)
                            {
                                // string defect_ID = array_Fine_Info[0];
                                Fine_Name = array_Fine_Info[1];
                                Fine_Ammount = array_Fine_Info[3];
                                Fine_ID = array_Fine_Info[5];
                            }
                            defect.DefectCode = defect_Code;
                            defect.DefectID = defect_ID.ToString();
                            defect.DefectName = defect_Name;
                            defect.FineAmmount = Fine_Ammount;
                            defect.FineName = Fine_Name;
                            defect.ActualValue = enterd_Height;

                            //addedDefectDeviceInspection.RemoveAll((x) => x.Contains(defect_Code.ToString()));

                            addedDefectDeviceInspection.RemoveAll((x) => x.DefectCode.Contains(defect_Code));

                            addedDefectDeviceInspection.Add(defect);
                            grdAddedDefects.ItemsSource = null;
                            grdAddedDefects.ItemsSource = addedDefectDeviceInspection;

                        }
                        else
                        {
                            lblHeightStatus.Visibility = Visibility.Visible;
                            lblHeightStatus.Content = new CommonUtils().GetStringValue("lblFail");
                            lblHeightStatus.Foreground = Brushes.Red;

                            string defect_Code = AppProperties.Defect_Height_Code;
                            int defect_ID = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectIDByCode(defect_Code);
                            string defect_Name = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectNameArByID(defect_Code);
                            string[] array_Fine_Info = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectsFineInfo(defect_ID.ToString(), vehicleCategory);
                            if (array_Fine_Info != null && array_Fine_Info.Length > 0)
                            {
                                // string defect_ID = array_Fine_Info[0];
                                Fine_Name = array_Fine_Info[2];
                                Fine_Ammount = array_Fine_Info[3];
                                Fine_ID = array_Fine_Info[5];
                            }
                            defect.DefectCode = defect_Code;
                            defect.DefectID = defect_ID.ToString();
                            defect.DefectName = defect_Name;
                            defect.FineAmmount = Fine_Ammount;
                            defect.FineName = Fine_Name;
                            defect.ActualValue = enterd_Height;

                            //addedDefectDeviceInspection.RemoveAll((x) => x.Contains(defect_Code.ToString()));

                            addedDefectDeviceInspection.RemoveAll((x) => x.DefectCode.Contains(defect_Code));

                            addedDefectDeviceInspection.Add(defect);
                            grdAddedDefects.ItemsSource = null;
                            grdAddedDefects.ItemsSource = addedDefectDeviceInspection;

                        }
                    }
                    else
                    {
                        lblHeightStatus.Content = new CommonUtils().GetStringValue("lblPass");
                        lblHeightStatus.Visibility = Visibility.Visible;
                        lblHeightStatus.Foreground = Brushes.Green;
                        addedDefectDeviceInspection.RemoveAll((x) => x.DefectCode.Contains(AppProperties.Defect_Height_Code));
                        grdAddedDefects.ItemsSource = null;
                        grdAddedDefects.ItemsSource = addedDefectDeviceInspection;
                        grdAddedDefects.UpdateLayout();
                    }
                }
                else
                {
                    lblHeightStatus.Content = "";
                    addedDefectDeviceInspection.RemoveAll((x) => x.DefectCode.Contains(AppProperties.Defect_Height_Code));
                    grdAddedDefects.ItemsSource = null;
                    grdAddedDefects.ItemsSource = addedDefectDeviceInspection;
                    grdAddedDefects.UpdateLayout();
                }

            }

            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }



        private void txtEmissionPercEntered_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.DigitSubstitution = System.Globalization.DigitShapes.None;
                CommonUtils.CLoseKeyBoard();
                vehicleSubCat = "";
                DeviceInspectionDefects defect = new DeviceInspectionDefects();
                string actual_Emission = txtEmissionPercActual.Text;
                string enterd_Emission = txtEmissionPercEntered.Text;
                string Fine_Name = String.Empty;
                string Fine_Ammount = String.Empty;
                string Fine_ID = String.Empty;
                if ((actual_Emission != null) && (enterd_Emission != null) && (enterd_Emission != ""))
                {
                    if (Convert.ToDouble(enterd_Emission) > Convert.ToDouble(actual_Emission))
                    {
                        if (AppProperties.Selected_Resource == "English")
                        {
                            lblEmissionStatus.Visibility = Visibility.Visible;
                            lblEmissionStatus.Content = new CommonUtils().GetStringValue("lblFail");
                            lblEmissionStatus.Foreground = Brushes.Red;

                            string defect_Code = AppProperties.Defect_Emission_Code;
                            int defect_ID = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectIDByCode(defect_Code);
                            string defect_Name = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectNameByID(defect_Code);
                            string[] array_Fine_Info = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectsFineInfo(defect_ID.ToString(), vehicleCategory);
                            if (array_Fine_Info != null && array_Fine_Info.Length > 0)
                            {
                                // string defect_ID = array_Fine_Info[0];
                                Fine_Name = array_Fine_Info[1];
                                Fine_Ammount = array_Fine_Info[3];
                                Fine_ID = array_Fine_Info[5];
                            }
                            defect.DefectCode = defect_Code;
                            defect.DefectID = defect_ID.ToString();
                            defect.DefectName = defect_Name;
                            defect.FineAmmount = Fine_Ammount;
                            defect.FineName = Fine_Name;
                            defect.ActualValue = enterd_Emission;

                            //addedDefectDeviceInspection.RemoveAll((x) => x.Contains(defect_Code.ToString()));

                            addedDefectDeviceInspection.RemoveAll((x) => x.DefectCode.Contains(defect_Code));

                            addedDefectDeviceInspection.Add(defect);
                            grdAddedDefects.ItemsSource = null;
                            grdAddedDefects.ItemsSource = addedDefectDeviceInspection;

                        }
                        else
                        {
                            lblEmissionStatus.Visibility = Visibility.Visible;
                            lblEmissionStatus.Content = new CommonUtils().GetStringValue("lblFail");
                            lblEmissionStatus.Foreground = Brushes.Red;

                            string defect_Code = AppProperties.Defect_Emission_Code;
                            int defect_ID = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectIDByCode(defect_Code);
                            string defect_Name = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectNameArByID(defect_Code);
                            string[] array_Fine_Info = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectsFineInfo(defect_ID.ToString(), vehicleCategory);
                            if (array_Fine_Info != null && array_Fine_Info.Length > 0)
                            {
                                // string defect_ID = array_Fine_Info[0];
                                Fine_Name = array_Fine_Info[2];
                                Fine_Ammount = array_Fine_Info[3];
                                Fine_ID = array_Fine_Info[5];
                            }
                            defect.DefectCode = defect_Code;
                            defect.DefectID = defect_ID.ToString();
                            defect.DefectName = defect_Name;
                            defect.FineAmmount = Fine_Ammount;
                            defect.FineName = Fine_Name;
                            defect.ActualValue = enterd_Emission;

                            //addedDefectDeviceInspection.RemoveAll((x) => x.Contains(defect_Code.ToString()));

                            addedDefectDeviceInspection.RemoveAll((x) => x.DefectCode.Contains(defect_Code));

                            addedDefectDeviceInspection.Add(defect);
                            grdAddedDefects.ItemsSource = null;
                            grdAddedDefects.ItemsSource = addedDefectDeviceInspection;

                        }
                    }
                    else
                    {
                        lblEmissionStatus.Content = new CommonUtils().GetStringValue("lblPass");
                        lblEmissionStatus.Visibility = Visibility.Visible;
                        lblEmissionStatus.Foreground = Brushes.Green;
                        addedDefectDeviceInspection.RemoveAll((x) => x.DefectCode.Contains(AppProperties.Defect_Emission_Code));
                        grdAddedDefects.ItemsSource = null;
                        grdAddedDefects.ItemsSource = addedDefectDeviceInspection;
                        grdAddedDefects.UpdateLayout();
                    }
                }
                else
                {
                    lblEmissionStatus.Content = "";
                    addedDefectDeviceInspection.RemoveAll((x) => x.DefectCode.Contains(AppProperties.Defect_Emission_Code));
                    grdAddedDefects.ItemsSource = null;
                    grdAddedDefects.ItemsSource = addedDefectDeviceInspection;
                    grdAddedDefects.UpdateLayout();
                }

            }

            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void txtNoOfAxelsEntered_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void txtGrossWeightEntered_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.DigitSubstitution = System.Globalization.DigitShapes.None;
                CommonUtils.CLoseKeyBoard();
                vehicleSubCat = "";
                DeviceInspectionDefects defect = new DeviceInspectionDefects();
                string actual_GrossWeight = txtGrossWeightActual.Text;
                string enterd_GrossWeight = txtGrossWeightEntered.Text;
                string Fine_Name = String.Empty;
                string Fine_Ammount = String.Empty;
                string Fine_ID = String.Empty;
                if ((actual_GrossWeight != null) && (enterd_GrossWeight != null) && (enterd_GrossWeight != ""))
                {
                    if (Convert.ToDouble(enterd_GrossWeight) > Convert.ToDouble(actual_GrossWeight))
                    {
                        if (AppProperties.Selected_Resource == "English")
                        {
                            lblGrossWeightStatus.Visibility = Visibility.Visible;
                            lblGrossWeightStatus.Content = new CommonUtils().GetStringValue("lblFail");
                            lblGrossWeightStatus.Foreground = Brushes.Red;

                            string defect_Code = AppProperties.Defect_GrossWeight_Code;
                            int defect_ID = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectIDByCode(defect_Code);
                            string defect_Name = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectNameByID(defect_Code);
                            string[] array_Fine_Info = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectsFineInfo(defect_ID.ToString(), vehicleCategory);
                            if (array_Fine_Info != null && array_Fine_Info.Length > 0)
                            {
                                // string defect_ID = array_Fine_Info[0];
                                Fine_Name = array_Fine_Info[1];
                                Fine_Ammount = array_Fine_Info[3];
                                Fine_ID = array_Fine_Info[5];
                            }
                            defect.DefectCode = defect_Code;
                            defect.DefectID = defect_ID.ToString();
                            defect.DefectName = defect_Name;
                            defect.FineAmmount = Fine_Ammount;
                            defect.FineName = Fine_Name;
                            defect.ActualValue = enterd_GrossWeight;

                            //addedDefectDeviceInspection.RemoveAll((x) => x.Contains(defect_Code.ToString()));

                            addedDefectDeviceInspection.RemoveAll((x) => x.DefectCode.Contains(defect_Code));

                            addedDefectDeviceInspection.Add(defect);
                            grdAddedDefects.ItemsSource = null;
                            grdAddedDefects.ItemsSource = addedDefectDeviceInspection;

                        }
                        else
                        {
                            lblGrossWeightStatus.Visibility = Visibility.Visible;
                            lblGrossWeightStatus.Content = new CommonUtils().GetStringValue("lblFail");
                            lblGrossWeightStatus.Foreground = Brushes.Red;

                            string defect_Code = AppProperties.Defect_GrossWeight_Code;
                            int defect_ID = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectIDByCode(defect_Code);
                            string defect_Name = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectNameArByID(defect_Code);
                            string[] array_Fine_Info = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectsFineInfo(defect_ID.ToString(), vehicleCategory);
                            if (array_Fine_Info != null && array_Fine_Info.Length > 0)
                            {
                                // string defect_ID = array_Fine_Info[0];
                                Fine_Name = array_Fine_Info[2];
                                Fine_Ammount = array_Fine_Info[3];
                                Fine_ID = array_Fine_Info[5];
                            }
                            defect.DefectCode = defect_Code;
                            defect.DefectID = defect_ID.ToString();
                            defect.DefectName = defect_Name;
                            defect.FineAmmount = Fine_Ammount;
                            defect.FineName = Fine_Name;
                            defect.ActualValue = enterd_GrossWeight;

                            //addedDefectDeviceInspection.RemoveAll((x) => x.Contains(defect_Code.ToString()));

                            addedDefectDeviceInspection.RemoveAll((x) => x.DefectCode.Contains(defect_Code));

                            addedDefectDeviceInspection.Add(defect);
                            grdAddedDefects.ItemsSource = null;
                            grdAddedDefects.ItemsSource = addedDefectDeviceInspection;

                        }
                    }
                    else
                    {
                        lblGrossWeightStatus.Content = new CommonUtils().GetStringValue("lblPass");
                        lblGrossWeightStatus.Visibility = Visibility.Visible;
                        lblGrossWeightStatus.Foreground = Brushes.Green;
                        addedDefectDeviceInspection.RemoveAll((x) => x.DefectCode.Contains(AppProperties.Defect_GrossWeight_Code));
                        grdAddedDefects.ItemsSource = null;
                        grdAddedDefects.ItemsSource = addedDefectDeviceInspection;
                        grdAddedDefects.UpdateLayout();
                    }
                }
                else
                {
                    lblGrossWeightStatus.Content = "";
                    addedDefectDeviceInspection.RemoveAll((x) => x.DefectCode.Contains(AppProperties.Defect_GrossWeight_Code));
                    grdAddedDefects.ItemsSource = null;
                    grdAddedDefects.ItemsSource = addedDefectDeviceInspection;
                    grdAddedDefects.UpdateLayout();
                }

            }

            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void btnInspect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Violation.Defects[] defects = new Violation.Defects[addedDefectDeviceInspection.Count];
                if (addedDefectDeviceInspection.Count > 0)
                {
                    for (int i = 0; i < addedDefectDeviceInspection.Count; i++)
                    {
                        defects[i] = new Violation.Defects();
                        defects[i].DefectID = Convert.ToInt32(addedDefectDeviceInspection[i].DefectID);
                        defects[i].DefectCode = addedDefectDeviceInspection[i].DefectCode;
                        defects[i].DefectName = addedDefectDeviceInspection[i].DefectName;
                        defects[i].DefectValue = addedDefectDeviceInspection[i].ActualValue;
                        defects[i].FineName = addedDefectDeviceInspection[i].FineName;
                    }
                    // AppProperties.recordedViolation.ViolationSeverity = ((IViolation)ViolationManager.GetInstance()).CalculateSeverity(AppProperties.recordedViolation.ViolationSeverity, row[5].Trim(), "");
                    //  AppProperties.recordedViolation.ViolationSeverityAr = ((IViolation)ViolationManager.GetInstance()).CalculateSeverity(AppProperties.recordedViolation.ViolationSeverityAr, row[6].Trim(), "");

                    AppProperties.recordedViolation.Defect = defects;

                    AppProperties.recordedViolation.IsConfiscated = true;
                    AppProperties.recordedViolation.PlateCondition = ""; //this.cmboxPlateCndition.SelectedValue.ToString();
                    AppProperties.recordedViolation.ConfiscationReason = "";// this.txtReasonForConfiscation.Text.ToString();


                    if (WPFMessageBox.Show(new CommonUtils().GetStringValue("Confirmation"), new CommonUtils().GetStringValue("lblSubmitViolation"), WPFMessageBoxButtons.YesNo, WPFMessageBoxImage.Question) == WPFMessageBoxResult.No)
                    {
                        return;
                    }


                    ProgressDialogResult result_offline = ProgressDialog.ProgressDialog.Execute(this.m_MainWindow, new CommonUtils().GetStringValue("lblSubmitViolationMessage"), (bw, we) =>
                    {

                        AppProperties.isOnline = ((IViolation)ViolationManager.GetInstance()).SubmitViolation();

                        // So this check in order to avoid default processing after the Cancel button has been pressed.
                        // This call will set the Cancelled flag on the result structure.
                        ProgressDialog.ProgressDialog.CheckForPendingCancellation(bw, we);

                    }, ProgressDialogSettings.WithSubLabelAndCancel);

                    if (result_offline == null || result_offline.Cancelled)
                        return;
                    else if (result_offline.OperationFailed)
                        return;
                    App.VSDLog.Info("is business Error" + AppProperties.businessError);
                    if (AppProperties.businessError)
                    {
                        AppProperties.isComprehensive = false;
                        AppProperties.confiscatePlates = false;
                        AppProperties.vehicle = null;
                        AppProperties.recordedViolation = null;
                        AppProperties.recordedViolation = new Violation();
                        AppProperties.recordedViolation.InspectionArea = AppProperties.location;
                        // System.Windows.Forms.MessageBox.Show(AppProperties.errorMessageFromBusiness);
                        WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("ErrorBusiness"), AppProperties.errorMessageFromBusiness);
                        AppProperties.businessError = false;
                        this.m_MainWindow.MainContentControl.Content = null;
                        //this.m_mainWindow.MainContentControl.Content = new ucLocationSelectionEn(m_mainWindow);
                        this.m_MainWindow.MainContentControl.Content = new ucWellComeScreen(m_MainWindow);
                        return;

                    }
                    else if (AppProperties.IsException)
                    {
                        AppProperties.IsException = false;
                        App.VSDLog.Info("\nIsException in PopulateData function:" + AppProperties.IsException);
                        this.m_MainWindow.MainContentControl.Content = null;
                        //this.m_mainWindow.MainContentControl.Content = new ucLocationSelectionEn(m_mainWindow);
                        this.m_MainWindow.MainContentControl.Content = new ucWellComeScreen(m_MainWindow);
                        WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("ErrorException"), AppProperties.errorMessageFromBusiness);
                        return;
                    }
                    else if (AppProperties.NotFoundError)
                    {
                        AppProperties.NotFoundError = false;
                        WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("ErrorException"), AppProperties.errorMessageFromBusiness);
                        this.m_MainWindow.MainContentControl.Content = null;
                        //this.m_mainWindow.MainContentControl.Content = new ucLocationSelectionEn(m_mainWindow);
                        this.m_MainWindow.MainContentControl.Content = new ucWellComeScreen(m_MainWindow);
                        return;
                    }
                    else if (AppProperties.IsServiceResponseNull)
                    {
                        App.VSDLog.Info("\n Submit Violation Response is NULL:");
                        WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("ErrorBusiness"), AppProperties.errorMessageFromBusiness);
                        this.m_MainWindow.MainContentControl.Content = null;
                        this.m_MainWindow.MainContentControl.Content = new ucWellComeScreen(m_MainWindow);
                        return;
                    }
                    else
                    {
                        this.m_MainWindow.MainContentControl.Content = null;
                        //this.m_mainWindow.MainContentControl.Content = new ucLocationSelectionEn(m_mainWindow);
                        this.m_MainWindow.MainContentControl.Content = new ucDeviceInpVioSummary(m_MainWindow);
                    }
                }
                else
                {
                    App.VSDLog.Info("No Defect in inspection, proceding with Inspection Submition");
                    AppProperties.recordedViolation.IsConfiscated = true;
                    AppProperties.recordedViolation.PlateCondition = ""; //this.cmboxPlateCndition.SelectedValue.ToString();
                    AppProperties.recordedViolation.ConfiscationReason = "";// this.txtReasonForConfiscation.Text.ToString();

                    if (WPFMessageBox.Show(new CommonUtils().GetStringValue("Confirmation"), new CommonUtils().GetStringValue("lblSubmitViolation"), WPFMessageBoxButtons.YesNo, WPFMessageBoxImage.Question) == WPFMessageBoxResult.No)
                    {
                        return;
                    }

                    ProgressDialogResult result_offline = ProgressDialog.ProgressDialog.Execute(this.m_MainWindow, new CommonUtils().GetStringValue("lblSubmitViolationMessage"), (bw, we) =>
                    {

                        AppProperties.isOnline = ((IViolation)ViolationManager.GetInstance()).SubmitViolation();

                        // So this check in order to avoid default processing after the Cancel button has been pressed.
                        // This call will set the Cancelled flag on the result structure.
                        ProgressDialog.ProgressDialog.CheckForPendingCancellation(bw, we);

                    }, ProgressDialogSettings.WithSubLabelAndCancel);

                    if (result_offline == null || result_offline.Cancelled)
                        return;
                    else if (result_offline.OperationFailed)
                        return;
                    App.VSDLog.Info("is business Error" + AppProperties.businessError);
                    if (AppProperties.businessError)
                    {
                        AppProperties.isComprehensive = false;
                        AppProperties.confiscatePlates = false;
                        AppProperties.vehicle = null;
                        AppProperties.recordedViolation = null;
                        AppProperties.recordedViolation = new Violation();
                        AppProperties.recordedViolation.InspectionArea = AppProperties.location;
                        // System.Windows.Forms.MessageBox.Show(AppProperties.errorMessageFromBusiness);
                        WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("ErrorBusiness"), AppProperties.errorMessageFromBusiness);
                        AppProperties.businessError = false;
                        this.m_MainWindow.MainContentControl.Content = null;
                        //this.m_mainWindow.MainContentControl.Content = new ucLocationSelectionEn(m_mainWindow);
                        this.m_MainWindow.MainContentControl.Content = new ucWellComeScreen(m_MainWindow);
                        return;

                    }
                    else if (AppProperties.IsException)
                    {
                        AppProperties.IsException = false;
                        App.VSDLog.Info("\nIsException in PopulateData function:" + AppProperties.IsException);
                        this.m_MainWindow.MainContentControl.Content = null;
                        //this.m_mainWindow.MainContentControl.Content = new ucLocationSelectionEn(m_mainWindow);
                        this.m_MainWindow.MainContentControl.Content = new ucWellComeScreen(m_MainWindow);
                        WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("ErrorException"), AppProperties.errorMessageFromBusiness);
                        return;
                    }
                    else if (AppProperties.NotFoundError)
                    {
                        AppProperties.NotFoundError = false;
                        WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("ErrorException"), AppProperties.errorMessageFromBusiness);
                        this.m_MainWindow.MainContentControl.Content = null;
                        //this.m_mainWindow.MainContentControl.Content = new ucLocationSelectionEn(m_mainWindow);
                        this.m_MainWindow.MainContentControl.Content = new ucWellComeScreen(m_MainWindow);
                        return;
                    }
                    else if (AppProperties.IsServiceResponseNull)
                    {
                        App.VSDLog.Info("\n Submit Violation Response is NULL:");
                        WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("ErrorBusiness"), AppProperties.errorMessageFromBusiness);
                        this.m_MainWindow.MainContentControl.Content = null;
                        this.m_MainWindow.MainContentControl.Content = new ucWellComeScreen(m_MainWindow);
                        return;
                    }
                    else
                    {
                        if (AppProperties.isInspectionUploaded)
                        {
                            if (AppProperties.Selected_Resource == "English")
                                //System.Windows.MessageBox.Show("Inspection Uploaded");
                                WPFMessageBox.Show(new CommonUtils().GetStringValue("Confirmation"), "Inspection Uploaded");
                            else
                                WPFMessageBox.Show(new CommonUtils().GetStringValue("Confirmation"), "تم تسجيل عملية التفتيش بنجاح");

                            this.m_MainWindow.MainContentControl.Content = null;
                            this.m_MainWindow.MainContentControl.Content = new ucLocationSelectionEn(m_MainWindow);
                        }
                    }

                }

            }

            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.m_MainWindow.MainContentControl.Content = null;
                this.m_MainWindow.MainContentControl.Content = new ucVehicleSelection(m_MainWindow);
            }

            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }



        private void expender_Emission_Expanded(object sender, RoutedEventArgs e)
        {
            if (expender_Dimension != null && expender_Weight != null && expender_Emission != null)
            {
                expender_Dimension.IsExpanded = false;
                expender_Emission.IsExpanded = true;
                expender_Weight.IsExpanded = false;
            }
        }

        private void expender_Weight_Expanded(object sender, RoutedEventArgs e)
        {
            if (expender_Dimension != null && expender_Weight != null && expender_Emission != null)
            {
                expender_Dimension.IsExpanded = false;
                expender_Emission.IsExpanded = false;
                expender_Weight.IsExpanded = true;
            }
        }
        private void expender_Dimension_Expanded(object sender, RoutedEventArgs e)
        {
            if (expender_Dimension != null && expender_Weight != null && expender_Emission != null)
            {
                expender_Dimension.IsExpanded = true;
                expender_Emission.IsExpanded = false;
                expender_Weight.IsExpanded = false;
            }
        }

        private void expender_DefectDetails_Expanded(object sender, RoutedEventArgs e)
        {

        }

        private void expender_Emission_Collapsed(object sender, RoutedEventArgs e)
        {

        }

        private void expender_Weight_Collapsed(object sender, RoutedEventArgs e)
        {

        }

        private void expender_DefectDetails_Collapsed(object sender, RoutedEventArgs e)
        {

        }

        private void txtBoxLengthActualEnterd_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

            Regex regex = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");

            e.Handled = !regex.IsMatch(e.Text);
        }

        private void imgDefectImageThumbnil_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (_strAllImagesOfDefect != null && _strAllImagesOfDefect.Count() > 0)
                {
                    //show images
                    ViewDefectImgs viewDefectImages = new ViewDefectImgs(_strAllImagesOfDefect, _selectedDefectInfo);
                    viewDefectImages.ShowDialog();
                    grdAddedDefects_SelectionChanged(sender, null);
                }
                else
                {
                    MessageBox.Show("No image available for this defect");
                }
            }

            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void cmboxSubCat_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void cmboxSubCat_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void cmboxSubCat_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void cmboxSubCat_GotFocus(object sender, RoutedEventArgs e)
        {
            CommonUtils.ShowKeyBoard();
        }

        public void ClearDeviceInspectionEnterdData()
        {
            try
            {
                this.txtBoxLengthActualEnterd.Text = string.Empty;
                txtGrossWeightEntered_LostFocus(null, null);
                this.lblLengthStatus.Content = string.Empty;
                this.txtWidthEntered.Text = string.Empty;
                txtWidthEntered_LostFocus(null, null);
                this.lblWidthStatus.Content = string.Empty;
                this.txtHeightEntered.Text = string.Empty;
                txtHeightEntered_LostFocus(null, null);
                this.lblHeightStatus.Content = string.Empty;
                this.txtEmissionPercEntered.Text = string.Empty;
                txtEmissionPercEntered_LostFocus(null, null);
                lblEmissionStatus.Content = string.Empty;
                this.txtGrossWeightEntered.Text = string.Empty;
                txtGrossWeightEntered_LostFocus(null, null);
                this.lblGrossWeightStatus.Content = string.Empty;

            }

            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void cmboxSubCat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                try
                {
                    ClearDeviceInspectionEnterdData();
                    if ((string)cmboxSubCat.SelectedItem != "" && cmboxSubCat.SelectedItem != null)
                    {
                        vehicleSubCat = (string)cmboxSubCat.SelectedItem;



                        /*
                        App.VSDLog.Info("Vehicle Subcat:" + vehicleSubCat);
                        if (AppProperties.vehicle != null && AppProperties.vehicle.DeviceInspectionparm != null)
                        {
                            vehicleSubCat = AppProperties.vehicle.SubCategory;
                        }
                        // get from previous screen

                        string Veh_SubCat_ID = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetVehCatIDFromVehCatName(vehicleSubCat);
                        Veh_SubCat_ID = "21";
                        */
                        App.VSDLog.Info("Vehicle Subcat ID:" + vehicleSubCat);
                        string Length_Meter = string.Empty;
                        string Width_Meter = string.Empty;
                        string Height_Meter = string.Empty;

                        dtDeviceInspParames = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDeviceInspParemtByVehCat(vehicleSubCat);
                        if (dtDeviceInspParames != null && dtDeviceInspParames.Rows.Count > 0)
                        {
                            DataRow selectedRow = dtDeviceInspParames.Rows[0];
                            Length_Meter = selectedRow["VEHICLE_LENGTH_METER"].ToString();
                            Width_Meter = selectedRow["VEHICLE_WIDTH_METER"].ToString();
                            Height_Meter = selectedRow["VEHICLE_HEIGHT_METER"].ToString();

                            this.txtBoxLengthActual.Text = Length_Meter;
                            this.txtHeightActual.Text = Height_Meter;
                            this.txtWidthActual.Text = Width_Meter;
                        }
                        else
                        {
                            this.txtBoxLengthActual.Text = string.Empty;
                            this.txtHeightActual.Text = string.Empty;
                            this.txtWidthActual.Text = string.Empty;
                        }
                    }
                }
                catch (Exception ex)
                {
                    App.VSDLog.Info(ex.StackTrace);
                    WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
                }
            }

            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void cmboxSubCat_LostFocus(object sender, RoutedEventArgs e)
        {
            CommonUtils.CLoseKeyBoard();
        }


    }
}
