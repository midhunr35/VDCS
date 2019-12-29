using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Technewlogic.Samples.WpfModalDialog;
using VSDApp.com.rta.vsd.hh.data;
using VSDApp.com.rta.vsd.hh.db;
using VSDApp.com.rta.vsd.hh.manager;
using VSDApp.com.rta.vsd.hh.utilities;
using VSDApp.com.rta.vsd.validation;
using VSDApp.ProgressDialog;
using VSDApp.WPFMessageBoxControl;

namespace VSDApp.com.rta.vsd.hh.ui
{
    /// <summary>
    /// Interaction logic for ucDefectAndViolationDetails.xaml
    /// </summary>
    public partial class ucDefectAndViolationDetails : UserControl
    {
        #region Data Members
        /// <summary>
        /// 
        /// </summary>
        MainWindow m_mainWindow = null;
        public List<string> defectTypeList;
        public List<string> defectList;
        public List<string> defectSubsList;
        public List<string> defectNameList;
        public List<string> _countries = new List<string>();
        public Hashtable citiesTable = new Hashtable();
        public Hashtable citiesTableEng = new Hashtable();
        List<vsd.hh.data.Violation.Defects> AddedDefects = new List<data.Violation.Defects>();


        public bool m_IsPlateConfiscation = false;
        private IValidation _iValidate;
        private string _validationResult;
        bool _fromRecordViolationFlow = false;

        public Hashtable defectTable = new Hashtable();
        public Hashtable defectSubTable = new Hashtable();
        public Hashtable defectNameTable = new Hashtable();
        public Hashtable safetydefectTable = new Hashtable();
        public bool Is_OpenDropDown = false;


        public Hashtable emirateTable = new Hashtable();
        private List<string> _emirateList = new List<string>();
        private List<string> _countryList = new List<string>();

        private string[] _strAllImagesOfDefect;
        private data.Violation.Defects _selectedDefectInfo;
        string telematics_defect_ID = string.Empty;
        BackgroundWorker m_oWorker;
        public delegate void Delegate_LoadDefectScreenData();

        #endregion


        #region Events
        private void cmboxType_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if ((string)cmboxType.SelectedItem != "" && cmboxType.SelectedItem != null)
                {

                    if (cmboxDefectCategory.Items.Count > 0)
                    {
                        cmboxDefectCategory.Text = "";
                        cmboxDefectCategory.Items.Clear();
                        cmboxDefectCategory.SelectedIndex = -1;
                    }
                    if (AppProperties.Selected_Resource == "English")
                    {

                        if (cmboxType.SelectedItem.ToString() == new CommonUtils().GetStringValue("Defect"))
                        {
                            defectList = new List<string>(((IViolation)ViolationManager.GetInstance()).GetDefects(null, "maincat", null));
                        }
                        else
                        {
                            //  defectList = new List<string>(((IDBDataLoad)DBDataLoadManager.GetInstance()).GetSafetyDefects());
                        }
                    }
                    else
                    {
                        if (cmboxType.SelectedItem.ToString() == new CommonUtils().GetStringValue("Defect"))
                        {
                            // defectList = new List<string>(((IViolation)ViolationManager.GetInstance()).GetDefects(null, "maincat", null));
                            defectTable = new Hashtable();
                            string[] list_defect = ((IViolation)ViolationManager.GetInstance()).GetDefects(null, "maincat", null);
                            defectTable = CommonUtils.Splitter(list_defect);
                            List<String> list_sorted_defects = CommonUtils.Return_Sorted_List_Hashtable(list_defect);
                            //defectTable.Add(_resource.GetString("No Defects"), "No Defects");
                            string[] defectsMain = new string[defectTable.Count];
                            defectTable.Keys.CopyTo(defectsMain, 0);
                            //    defectList = new List<string>(defectsMain);

                            defectList = list_sorted_defects;
                            /////////////////////////////
                            //For Ordering in Arabic
                            /////////////////////////////




                        }
                        else
                        {
                            //defectList = new List<string>(((IDBDataLoad)DBDataLoadManager.GetInstance()).GetSafetyDefects());
                            defectTable = new Hashtable();
                            defectTable = CommonUtils.Splitter(((IDBDataLoad)DBDataLoadManager.GetInstance()).GetSafetyDefects());
                            //  defectTable.Add(_resource.GetString("No Defects"), "No Defects");
                            string[] safety = new string[defectTable.Count];
                            defectTable.Keys.CopyTo(safety, 0);
                            defectList = new List<string>(safety);
                        }
                    }
                    if (defectList != null)
                    {
                        foreach (string defect in defectList)
                        {
                            cmboxDefectCategory.Items.Add(defect.Trim());
                        }
                    }
                    if (cmboxDefectCategory.Items.Count > 0)
                    {
                        cmboxDefectCategory.SelectedIndex = 0;
                    }
                    AddedDefects = new List<data.Violation.Defects>();
                    AppProperties.selectedDefectsEn.Clear();
                    grdAddedDefects.ItemsSource = null;
                    ClearFieldOnSelectionChange();
                }
                else
                {

                }


                // 



            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }

        }

        private void cmboxDefectCategory_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                txtActualValue.Text = "";
                if ((string)cmboxDefectCategory.SelectedItem != "" && cmboxDefectCategory.SelectedItem != null)
                {
                    startImageBlinkOfSelectedDefect(cmboxDefectCategory.SelectedItem.ToString());
                    if (cmboxDefectSubCateogry.Items.Count > 0)
                    {
                        cmboxDefectSubCateogry.Text = "";
                        cmboxDefectSubCateogry.Items.Clear();
                        cmboxDefectSubCateogry.SelectedIndex = -1;
                    }
                    if (cmboxDefectCategory.SelectedValue != null)
                    {
                        if (AppProperties.Selected_Resource == "English")
                        {
                            defectSubsList = new List<string>(((IViolation)ViolationManager.GetInstance()).GetDefects((string)cmboxDefectCategory.SelectedItem.ToString(), "subcat", (cmboxType.SelectedItem.ToString() == new CommonUtils().GetStringValue("Defect")) ? "Defect" : "Safety Violation"));
                        }
                        else
                        {
                            defectSubTable = new Hashtable();

                            string[] list_defect = ((IViolation)ViolationManager.GetInstance()).GetDefects((string)defectTable[(string)cmboxDefectCategory.SelectedItem], "subcat", (cmboxType.SelectedItem.ToString() == new CommonUtils().GetStringValue("Defect")) ? "Defect" : "Safety Violation");
                            defectSubTable = CommonUtils.Splitter(list_defect);
                            // defectSubTable.Add(_resource.GetString("No Defects"), "No Defects");
                            string[] subs = new string[defectSubTable.Count];
                            defectSubTable.Keys.CopyTo(subs, 0);
                            //  defectSubsList = new List<string>(subs);

                            defectSubsList = CommonUtils.Return_Sorted_List_Hashtable(list_defect);

                        }
                    }
                    foreach (string defectSubCategory in defectSubsList)
                    {
                        cmboxDefectSubCateogry.Items.Add(defectSubCategory.Trim().ToString());
                    }
                    if (cmboxDefectSubCateogry.Items.Count > 0)
                    {
                        cmboxDefectSubCateogry.SelectedIndex = 0;
                    }
                }
                else
                {
                    // cmboxDefectSubCateogry.Items.Clear();
                    // cmboxDefectSubCateogry.Items.Add("");
                }
                // ClearFieldOnSelectionChange();

            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);

            }
        }
        //added by kashif abbasi on dated 03-dec-2016
        private void startImageBlinkOfSelectedDefect(string selectedDefect)
        {
            Image imgToBlink = new Image();
            if (grdVehicleImageSlice.IsVisible)
            {
                if (selectedDefect.Trim().Equals("Wheels and Tires") || selectedDefect.Trim().Equals("الإطارات"))
                {
                    imgToBlink = Image9_WheelAndTyre1;
                }
                else if (selectedDefect.Trim().Equals("Vehicle Identification") || selectedDefect.Trim().Equals("هوية المركبة"))
                {
                    imgToBlink = Image3_VehicleIdentification1;
                }
                else if (selectedDefect.Trim().Equals("Brake") || selectedDefect.Trim().Equals("نظام الفرامل"))
                {
                    imgToBlink = Image12_Brake1;
                }
                //else if (selectedDefect.Trim().Equals("Steering and Suspension") || selectedDefect.Trim().Equals("نظام المقود والتعليق"))
                //{
                //    imgToBlink = Image14_StearingSuspension;
                //}

                else if (selectedDefect.Trim().Equals("Driver Identification") || selectedDefect.Trim().Equals("هوية السائق"))
                {
                    imgToBlink = Image14_StearingSuspension;
                }
                else if (selectedDefect.Trim().Equals("Body Condition") || selectedDefect.Trim().Equals("بدن المركبة"))
                {
                    imgToBlink = Image25_BodyCondition;
                }
                else if (selectedDefect.Trim().Equals("Lighting") || selectedDefect.Trim().Equals("الأنوار"))
                {
                    imgToBlink = Image2_Lightining2;
                }
                else if (selectedDefect.Trim().Equals("Engine") || selectedDefect.Trim().Equals("المحرك"))
                {
                    imgToBlink = Image7_EngineCompartment;
                }
                else if (selectedDefect.Trim().Equals("Modification") || selectedDefect.Trim().Equals("المركبات المعدلة"))
                {
                    imgToBlink = Image296_ModifiedVehicle;
                }
                else if (selectedDefect.Trim().Equals("Safety Requirements") || selectedDefect.Trim().Equals("معدات السلامة"))
                {
                    imgToBlink = Image31_RoadSafety;
                }
            }
            else if (grdPickup.IsVisible)
            {
                if (selectedDefect.Trim().Equals("Wheels and Tires") || selectedDefect.Trim().Equals("الإطارات"))
                {
                    imgToBlink = ImgPickup15_Wheel_1;
                }
                else if (selectedDefect.Trim().Equals("Vehicle Identification") || selectedDefect.Trim().Equals("هوية المركبة"))
                {
                    imgToBlink = ImgPickup6_Front_PlateNo;
                }
                else if (selectedDefect.Trim().Equals("Brake") || selectedDefect.Trim().Equals("نظام الفرامل"))
                {
                    imgToBlink = ImgPickup19_Break_1;
                }
                //else if (selectedDefect.Trim().Equals("Steering and Suspension") || selectedDefect.Trim().Equals("نظام المقود والتعليق"))
                //{
                //    imgToBlink = ImgPickup23_StearingSuspension;
                //}
                else if (selectedDefect.Trim().Equals("Driver Identification") || selectedDefect.Trim().Equals("هوية السائق"))
                {
                    imgToBlink = ImgPickup23_StearingSuspension;
                }
                else if (selectedDefect.Trim().Equals("Body Condition") || selectedDefect.Trim().Equals("بدن المركبة"))
                {
                    imgToBlink = ImgPickup27_BodyCondition;
                }
                else if (selectedDefect.Trim().Equals("Lighting") || selectedDefect.Trim().Equals("الأنوار"))
                {
                    imgToBlink = ImgPickup4_FrontLight1;
                }
                else if (selectedDefect.Trim().Equals("Engine") || selectedDefect.Trim().Equals("المحرك"))
                {
                    imgToBlink = ImgPickup11_EngineCompartment;
                }
                else if (selectedDefect.Trim().Equals("Modification") || selectedDefect.Trim().Equals("المركبات المعدلة"))
                {
                    imgToBlink = ImgPickup31_ModifiedVehicle;
                }
                else if (selectedDefect.Trim().Equals("Safety Requirements") || selectedDefect.Trim().Equals("معدات السلامة"))
                {
                    imgToBlink = ImgPickup33_RoadSafty;
                }
            }
            else if (grdBus.IsVisible)
            {
                if (selectedDefect.Trim().Equals("Wheels and Tires") || selectedDefect.Trim().Equals("الإطارات"))
                {
                    imgToBlink = ImgBus13_Wheel_1;
                }
                else if (selectedDefect.Trim().Equals("Vehicle Identification") || selectedDefect.Trim().Equals("هوية المركبة"))
                {
                    imgToBlink = ImgBus4_Front_PlatNo;
                }
                else if (selectedDefect.Trim().Equals("Brake") || selectedDefect.Trim().Equals("نظام الفرامل"))
                {
                    imgToBlink = ImgBus16_Break_1;
                }
                //else if (selectedDefect.Trim().Equals("Steering and Suspension") || selectedDefect.Trim().Equals("نظام المقود والتعليق"))
                //{
                //    imgToBlink = ImgBus18_StearingSuspension;
                //}
                else if (selectedDefect.Trim().Equals("Driver Identification") || selectedDefect.Trim().Equals("هوية السائق"))
                {
                    imgToBlink = ImgBus18_StearingSuspension;
                }
                else if (selectedDefect.Trim().Equals("Body Condition") || selectedDefect.Trim().Equals("بدن المركبة"))
                {
                    imgToBlink = ImgBus23_BodyCondition;
                }
                else if (selectedDefect.Trim().Equals("Lighting") || selectedDefect.Trim().Equals("الأنوار"))
                {
                    imgToBlink = ImgBus2_FrontLight;
                }
                else if (selectedDefect.Trim().Equals("Engine") || selectedDefect.Trim().Equals("المحرك"))
                {
                    imgToBlink = ImgBus10_EngineCompartment;
                }
                else if (selectedDefect.Trim().Equals("Modification") || selectedDefect.Trim().Equals("المركبات المعدلة"))
                {
                    imgToBlink = ImgBus27_ModifiedVehicle;
                }
                else if (selectedDefect.Trim().Equals("Safety Requirements") || selectedDefect.Trim().Equals("معدات السلامة"))
                {
                    imgToBlink = ImgBus29_RoadSafty;
                }
            }
            else if (grdVan.IsVisible)
            {
                if (selectedDefect.Trim().Equals("Wheels and Tires") || selectedDefect.Trim().Equals("الإطارات"))
                {
                    imgToBlink = ImgVan14_Wheel_1;
                }
                else if (selectedDefect.Trim().Equals("Vehicle Identification") || selectedDefect.Trim().Equals("هوية المركبة"))
                {
                    imgToBlink = ImgVan4_Front_PlateNo;
                }
                else if (selectedDefect.Trim().Equals("Brake") || selectedDefect.Trim().Equals("نظام الفرامل"))
                {
                    imgToBlink = ImgVan19_Break_1;
                }
                //else if (selectedDefect.Trim().Equals("Steering and Suspension") || selectedDefect.Trim().Equals("نظام المقود والتعليق"))
                //{
                //    imgToBlink = ImgVan24_StearingSuspension;
                //}
                else if (selectedDefect.Trim().Equals("Driver Identification") || selectedDefect.Trim().Equals("هوية السائق"))
                {
                    imgToBlink = ImgVan24_StearingSuspension;
                }
                else if (selectedDefect.Trim().Equals("Body Condition") || selectedDefect.Trim().Equals("بدن المركبة"))
                {
                    imgToBlink = ImgVan28_BodyCondition;
                }
                else if (selectedDefect.Trim().Equals("Lighting") || selectedDefect.Trim().Equals("الأنوار"))
                {
                    imgToBlink = ImgVan2_FrontLight1;
                }
                else if (selectedDefect.Trim().Equals("Engine") || selectedDefect.Trim().Equals("المحرك"))
                {
                    imgToBlink = ImgVan10_EngineCompartment;
                }
                else if (selectedDefect.Trim().Equals("Modification") || selectedDefect.Trim().Equals("المركبات المعدلة"))
                {
                    imgToBlink = ImgVan32_ModifiedVehicle;
                }
                else if (selectedDefect.Trim().Equals("Safety Requirements") || selectedDefect.Trim().Equals("معدات السلامة"))
                {
                    imgToBlink = ImgVan34_RoadSafty;
                }
            }
            else if (grdHeavyMach.IsVisible)
            {
                if (selectedDefect.Trim().Equals("Wheels and Tires") || selectedDefect.Trim().Equals("الإطارات"))
                {
                    imgToBlink = ImgHeavyMach14;
                }
                else if (selectedDefect.Trim().Equals("Vehicle Identification") || selectedDefect.Trim().Equals("هوية المركبة"))
                {
                    imgToBlink = ImgHeavyMach3;
                }
                else if (selectedDefect.Trim().Equals("Brake") || selectedDefect.Trim().Equals("نظام الفرامل"))
                {
                    imgToBlink = ImgHeavyMach17;
                }
                //else if (selectedDefect.Trim().Equals("Steering and Suspension") || selectedDefect.Trim().Equals("نظام المقود والتعليق"))
                //{
                //    imgToBlink = ImgHeavyMach19;
                //}
                else if (selectedDefect.Trim().Equals("Driver Identification") || selectedDefect.Trim().Equals("هوية السائق"))
                {
                    imgToBlink = ImgHeavyMach19;
                }
                else if (selectedDefect.Trim().Equals("Body Condition") || selectedDefect.Trim().Equals("بدن المركبة"))
                {
                    imgToBlink = ImgHeavyMach24;
                }
                else if (selectedDefect.Trim().Equals("Lighting") || selectedDefect.Trim().Equals("الأنوار"))
                {
                    imgToBlink = ImgHeavyMach6;
                }
                else if (selectedDefect.Trim().Equals("Engine") || selectedDefect.Trim().Equals("المحرك"))
                {
                    imgToBlink = ImgHeavyMach11;
                }
                else if (selectedDefect.Trim().Equals("Modification") || selectedDefect.Trim().Equals("المركبات المعدلة"))
                {
                    imgToBlink = ImgHeavyMach28;
                }
                else if (selectedDefect.Trim().Equals("Safety Requirements") || selectedDefect.Trim().Equals("معدات السلامة"))
                {
                    imgToBlink = ImgHeavyMach30;
                }
            }
            startImageBlink(imgToBlink);
        }

        private void cmboxDefectSubCateogry_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            try
            {

                txtActualValue.Text = "";
                if ((string)cmboxDefectSubCateogry.SelectedItem != "" && cmboxDefectSubCateogry.SelectedItem != null)
                {
                    if (cmboxDefect.Items.Count > 0)
                    {
                        cmboxDefect.Text = "";
                        cmboxDefect.Items.Clear();
                        cmboxDefect.SelectedIndex = -1;
                    }
                    if (cmboxDefectSubCateogry.SelectedValue != null)
                    {
                        if (AppProperties.Selected_Resource == "English")
                        {
                            App.VSDLog.Info("Defect Screen" + (string)cmboxDefectSubCateogry.SelectedItem.ToString() + "\n" + cmboxType.SelectedItem.ToString());
                            defectNameList = new List<string>(((IViolation)ViolationManager.GetInstance()).GetDefects((string)cmboxDefectSubCateogry.SelectedItem.ToString(), " ", (cmboxType.SelectedItem.ToString() == new CommonUtils().GetStringValue("Defect")) ? "Defect" : "Safety Violation"));
                        }
                        else
                        {
                            defectNameTable = new Hashtable();
                            string[] list_defect_Name = ((IViolation)ViolationManager.GetInstance()).GetDefects((string)defectSubTable[(string)cmboxDefectSubCateogry.SelectedItem], " ", (cmboxType.SelectedItem.ToString() == new CommonUtils().GetStringValue("Defect")) ? "Defect" : "Safety Violation");
                            //  defectNameTable.Add(_resource.GetString("No Defects"), "No Defects");
                            //  string[] defName = new string[defectNameTable.Count];
                            // defectNameTable.Keys.CopyTo(defName, 0);
                            // defectNameList = new List<string>(defName);
                            defectNameList = CommonUtils.Return_Sorted_List_Hashtable(list_defect_Name);


                            defectNameTable = new Hashtable();
                            defectNameTable = CommonUtils.Splitter(((IViolation)ViolationManager.GetInstance()).GetDefects((string)defectSubTable[(string)cmboxDefectSubCateogry.SelectedItem], " ", (cmboxType.SelectedItem.ToString() == new CommonUtils().GetStringValue("Defect")) ? "Defect" : "Safety Violation"));
                            //  defectNameTable.Add(_resource.GetString("No Defects"), "No Defects");
                            string[] defName = new string[defectNameTable.Count];
                            defectNameTable.Keys.CopyTo(defName, 0);
                            //  defectNameList = new List<string>(defName);



                        }
                    }
                    foreach (string defectSubCategory in defectNameList)
                    {
                        cmboxDefect.Items.Add(defectSubCategory.Trim().ToString());
                    }
                    if (cmboxDefect.Items.Count > 0)
                    {
                        cmboxDefect.SelectedIndex = 0;
                    }
                }
                else
                {
                    //  cmboxDefectSubCateogry.Items.Clear();
                    //  cmboxDefectSubCateogry.Items.Add("");
                }
                // ClearFieldOnSelectionChange();

            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void cmboxDefect_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            // ClearFieldOnSelectionChange();

            txtActualValue.Text = "";

        }
        /// <summary>
        /// Screen is Taking Time to Load that Y added separate thread
        /// </summary>
        public void LoadDefectScreenViaSepateThred()
        {
            try
            {
                App.VSDLog.Info("LoadDefectScreenViaSepateThred()-> Loading Defect Screen Via Separete Thread");

                m_oWorker = new BackgroundWorker();
                m_oWorker.DoWork += m_oWorker_DoWork;
                m_oWorker.RunWorkerCompleted += m_oWorker_RunWorkerCompleted;
                m_oWorker.WorkerReportsProgress = true;
                m_oWorker.WorkerSupportsCancellation = true;
                m_oWorker.RunWorkerAsync();



            }
            catch (Exception ex)
            {
                App.VSDLog.Info("LoadDefectScreenViaSepateThred()-> Exception" + ex.Message);
            }
        }

        void m_oWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        void m_oWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (m_oWorker.IsBusy)
                this.Dispatcher.Invoke(new Delegate_LoadDefectScreenData(this.LoadDefectScreenInitialData), new object[] { });

            // LoadDefectScreenInitialData();
        }

        public void LoadDefectScreenInitialData()
        {
            HazardImageChange();
            showVehicleGridByCategory();
            PopulateData();
            /*
            ProgressDialogResult load = ProgressDialog.ProgressDialog.Execute(this.m_mainWindow, new CommonUtils().GetStringValue("lblSubmitViolationMessage"), (bw, we) =>
                {
                    HazardImageChange();
                    showVehicleGridByCategory();
                    PopulateData();

                    

                }, ProgressDialogSettings.WithSubLabelAndCancel);*/


        }
        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {

            //  LoadDefectScreenInitialData();
            LoadDefectScreenViaSepateThred();

            //  HazardImageChange();
            //   showVehicleGridByCategory();
            //   PopulateData();


            AppProperties.Is_SubmitVilation = false;
            AppProperties.Is_DriverDataVerified = false;
            if (AppProperties.Selected_Resource == "Arabic")
            {
                btnStackePanel.FlowDirection = System.Windows.FlowDirection.LeftToRight;
                if (grdVehicleImageSlice.IsVisible)
                    grdVehicleImageSlice.FlowDirection = System.Windows.FlowDirection.LeftToRight;
                else if (grdBus.IsVisible)
                    grdBus.FlowDirection = System.Windows.FlowDirection.LeftToRight;
                else if (grdPickup.IsVisible)
                    grdPickup.FlowDirection = System.Windows.FlowDirection.LeftToRight;
                else if (grdVan.IsVisible)
                    grdVan.FlowDirection = System.Windows.FlowDirection.LeftToRight;
                else if (grdHeavyMach.IsVisible)
                    grdHeavyMach.FlowDirection = System.Windows.FlowDirection.LeftToRight;
                lblgrdSeverityEng.Visibility = System.Windows.Visibility.Collapsed;
                lblgrdSeverityAr.Visibility = System.Windows.Visibility.Visible;

            }
            else
            {
                lblgrdSeverityEng.Visibility = System.Windows.Visibility.Visible;
                lblgrdSeverityAr.Visibility = System.Windows.Visibility.Collapsed;
            }


            AppProperties.vehicle.TotalFineAmmount = "0";

            m_IsPlateConfiscation = false;
            AppProperties.recordedViolation.ViolationSeverity = "";
            AppProperties.recordedViolation.ViolationSeverityAr = "";
            //  AppProperties.confiscatePlates = false;
            if (AppProperties.confiscatePlates)
            {
                //  EnableDisableAddedDefectsOptions(false);
                // PopulateConfiscateOption();
                // EnableConfiscateOptions(true);
                EnableDisableDriverInfoFields(true);

            }
            else
            {

                // PopulateConfiscateOption();
                EnableDisableDriverInfoFields(true);
                //EnableConfiscateOptions(false);
                // EnableDisableAddedDefectsOptions(true);
            }
            System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.DigitSubstitution = System.Globalization.DigitShapes.None;
            this.UpdateLayout();

        }

        public void BtnAddEnHandler()
        {
            if (cmboxType.SelectedItem.ToString() == "")
            {
                WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), lblDataNotValid.Content.ToString());
                return;
            }
            else if (cmboxDefectCategory.SelectedItem.ToString() == "")
            {
                WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), lblDataNotValid.Content.ToString());
                return;
            }
            else if (cmboxDefectSubCateogry.SelectedItem.ToString() == "")
            {
                WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), lblDataNotValid.Content.ToString());
                return;
            }
            else if (cmboxDefect.SelectedItem.ToString() == "")
            {
                WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), lblDataNotValid.Content.ToString());
                return;
            }/*
            else if (AppProperties.isComprehensive && cmboxType.SelectedItem.ToString() == "Defect")
            {
                WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), lblComprehensivetestexists.Content.ToString());
                return;
            }*/

            for (int i = 0; i < AppProperties.selectedDefectsEn.Count; i++)
            {
                string[] data = AppProperties.selectedDefectsEn[i];
                if (data[4].Trim().Equals((string)cmboxDefect.SelectedValue.ToString().Trim()) && data[3].Trim().Equals((string)cmboxDefectCategory.SelectedValue.ToString().Trim()) && data[2].Trim().Equals((string)cmboxDefectSubCateogry.SelectedValue.ToString().Trim()))
                {
                    // System.Windows.Forms.MessageBox.Show(data[1].Trim() + lblalreadyadded.Content.ToString());
                    WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), lblalreadyadded.Content.ToString());
                    return;
                }
            }
            // string[] details = _controller.GetDefectSeverity((string)cmboxDefect.SelectedValue.ToString(), (string)cmboxDefectCategory.SelectedValue.ToString(), (string)cmboxDefectSubCateogry.SelectedValue.ToString(), txtActualValue.Text);
            string[] details = ((IViolation)ViolationManager.GetInstance()).GetDefectSeverity((string)cmboxDefect.SelectedValue.ToString().Trim(), (string)cmboxDefectSubCateogry.SelectedValue.ToString().Trim(), (string)cmboxDefectCategory.SelectedValue.ToString().Trim(), txtActualValue.Text);
            /* if (null != details)
         {
             if (AppProperties.previousDefectIDs.Contains(Convert.ToInt32(details[0])))
             {
                 // System.Windows.Forms.MessageBox.Show(details[4].Trim() + lblexistsopenviolation.Content.ToString());
                 WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), details[4].Trim() + lblexistsopenviolation.Content.ToString());
                 return;
             }

         }
         */
            if (null == details)
            {

                WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("lblDefectInfoMissing"));
                return;

            }
            else
            {
                AppProperties.selectedDefectsEn.Add(details);
                // defectsTable.Rows.Add(details[1], details[2], details[5], "No");

                data.Violation.Defects defct = new data.Violation.Defects();
                //if (cmboxType.SelectedItem.ToString() == "Defect")
                //{
                //    defct.DefectType = "Defect";
                //}
                //else
                //{
                //    defct.DefectType = "Safety Violation";
                //}
                defct.DefectID = Convert.ToInt32(details[0].ToString().Trim());
                // defct.DefectType = details[1].Trim().ToString();
                defct.DefectType = new CommonUtils().GetStringValue(details[1].Trim().ToString().Replace(" ", string.Empty));
                defct.DefectName = details[4].ToString().Trim();
                defct.DefectCategory = details[3].ToString().Trim();
                defct.DefectSubCat = details[2].ToString().Trim();
                defct.DefectValue = details[7].ToString().Trim();
                defct.DefectSeverity = details[5].ToString().Trim();
                defct.EnforceTesting = details[10].ToString().Trim();
                defct.EnforceFine = details[11].ToString().Trim();
                if (defct.EnforceTesting != null || defct.EnforceTesting != "")
                {
                    if (defct.EnforceTesting.ToString().Equals("F"))
                    {
                        defct.DefectSeverity += "(*)";
                        defct.DefectSeverityAr += "(*(";
                    }
                }

                string vehCat = AppProperties.vehicle.VehicleCategory;
                if ((vehCat == "") || (vehCat == null))
                    vehCat = "Heavy Vehicle";
                defct.ImpoundingDays = GetImpoundingDays(defct.DefectID.ToString(), vehCat).ToString();
                UpdateGroundingDays(defct.ImpoundingDays);

                this.lblgrdSeverityEng.Visibility = System.Windows.Visibility.Visible;
                this.lblgrdSeverityAr.Visibility = System.Windows.Visibility.Collapsed;

                AddedDefects.Add(defct);
                grdAddedDefects.ItemsSource = null;
                grdAddedDefects.ItemsSource = AddedDefects;
                txtActualValue.Text = "";
            }
        }
        public void BtnArEnHandler()
        {

            if (cmboxType.SelectedItem.ToString() == "")
            {
                WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), lblDataNotValid.Content.ToString());
                return;
            }
            else if (cmboxDefectCategory.SelectedItem.ToString() == "")
            {
                WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), lblDataNotValid.Content.ToString());
                return;
            }
            else if (cmboxDefectSubCateogry.SelectedItem.ToString() == "")
            {
                WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), lblDataNotValid.Content.ToString());
                return;
            }
            else if (cmboxDefect.SelectedItem.ToString() == "")
            {
                WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), lblDataNotValid.Content.ToString());
                return;
            }
            /*
        else if (AppProperties.isComprehensive && cmboxType.SelectedItem.ToString() == "Defect")
        {
            WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), lblComprehensivetestexists.Content.ToString());
            return;
        }*/

            for (int i = 0; i < AppProperties.selectedDefectsEn.Count; i++)
            {
                string[] data = AppProperties.selectedDefectsEn[i];
                if (data[4].Trim().Equals((string)defectNameTable[(string)cmboxDefect.SelectedValue].ToString().Trim()) && data[2].Trim().Equals((string)defectSubTable[(string)cmboxDefectSubCateogry.SelectedValue].ToString().Trim()) && data[3].Trim().Equals((string)defectTable[(string)cmboxDefectCategory.SelectedValue].ToString().Trim()))
                {
                    WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), lblalreadyadded.Content.ToString());
                    return;
                }
            }
            // string[] details = _controller.GetDefectSeverity((string)cmboxDefect.SelectedValue.ToString(), (string)cmboxDefectCategory.SelectedValue.ToString(), (string)cmboxDefectSubCateogry.SelectedValue.ToString(), txtActualValue.Text);
            // string[] details = ((IViolation)ViolationManager.GetInstance()).GetDefectSeverity((string)cmboxDefect.SelectedValue.ToString(), (string)cmboxDefectSubCateogry.SelectedValue.ToString(), (string)cmboxDefectCategory.SelectedValue.ToString(), txtActualValue.Text);
            string[] details = ((IViolation)ViolationManager.GetInstance()).GetDefectSeverity((string)defectNameTable[(string)cmboxDefect.SelectedValue.ToString().Trim()], (string)defectSubTable[(string)cmboxDefectSubCateogry.SelectedValue.ToString().Trim()], (string)defectTable[(string)cmboxDefectCategory.SelectedValue.ToString().Trim()], txtActualValue.Text);
            /* if (null != details)
                {
                    if (AppProperties.previousDefectIDs.Contains(Convert.ToInt32(details[0])))
                    {
                        // System.Windows.Forms.MessageBox.Show(details[4].Trim() + lblexistsopenviolation.Content.ToString());
                        WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), details[4].Trim() + lblexistsopenviolation.Content.ToString());
                        return;
                    }

                }
                */
            if (null == details)
            {

                WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("lblDefectInfoMissing"));
                return;

            }
            else
            {
                AppProperties.selectedDefectsEn.Add(details);
                // defectsTable.Rows.Add(details[1], details[2], details[5], "No");

                data.Violation.Defects defct = new data.Violation.Defects();
                //if (cmboxType.SelectedItem.ToString() == "Defect")
                //{
                //    defct.DefectType = "Defect";
                //}
                //else
                //{
                //    defct.DefectType = "Safety Violation";
                //}
                defct.DefectID = Convert.ToInt32(details[0].ToString().Trim());
                // defct.DefectType = details[1].Trim().ToString();
                defct.DefectType = new CommonUtils().GetStringValue(details[1].Trim().ToString().Replace(" ", string.Empty));

                defct.DefectName = details[9].ToString().Trim();
                defct.DefectSeverity = details[5].ToString().Trim();
                defct.DefectSeverityAr = details[6].ToString().Trim();

                defct.EnforceTesting = details[10].ToString().Trim();
                defct.EnforceFine = details[11].ToString().Trim();
                if (defct.EnforceTesting != null || defct.EnforceTesting != "")
                {
                    if (defct.EnforceTesting.ToString().Equals("F"))
                    {

                        defct.DefectSeverity += "(*)";
                        defct.DefectSeverityAr += "(*)";
                    }
                }

                defct.DefectCategory = (string)cmboxDefectCategory.SelectedValue.ToString().Trim();
                //details[3].ToString().Trim();
                defct.DefectSubCat = (string)cmboxDefectSubCateogry.SelectedValue.ToString().Trim();
                //details[2].ToString().Trim();


                string vehCat = AppProperties.vehicle.VehicleCategory;
                if ((vehCat == "") || (vehCat == null))
                    vehCat = "Heavy Vehicle";

                defct.DefectValue = details[7].ToString().Trim();
                defct.ImpoundingDays = GetImpoundingDays(defct.DefectID.ToString(), vehCat).ToString();
                UpdateGroundingDays(defct.ImpoundingDays);

                this.lblgrdSeverityEng.Visibility = System.Windows.Visibility.Collapsed;
                this.lblgrdSeverityAr.Visibility = System.Windows.Visibility.Visible;
                AddedDefects.Add(defct);
                grdAddedDefects.ItemsSource = null;
                grdAddedDefects.ItemsSource = AddedDefects;
                txtActualValue.Text = "";
                System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.DigitSubstitution = System.Globalization.DigitShapes.None;
                this.UpdateLayout();
                // AddImpoundingDays(details[0].ToString().Trim());
            }
        }
        public void UpdateGroundingDays(string groundingDays)
        {
            try
            {
                this.txtImpounding.Text = (Int32.Parse(this.txtImpounding.Text) + Int32.Parse(groundingDays)).ToString();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public int GetImpoundingDays(string defectID, string vehCat)
        {
            try
            {

                string[] vehcile_Def_Sev_Fine = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetImpoundingDays(defectID, vehCat);
                /*
                   row[0] = rs["V_CAT_DEF_SEV_FINE_ID"].ToString();
                 row[1] = rs["VEH_CAT_DEFECT_SEV_ID"].ToString();
                 row[2] = rs["PARTNER_FINE_ID"].ToString();
                 row[3] = rs["FINE_AMOUNT"].ToString();
                 row[4] = rs["BLACKPOINTS"].ToString();
                 row[5] = rs["Required_Grounding"].ToString();
                 row[6] = rs["Grounded_Days"].ToString();
                 row[7] = rs["Is_Disabled"].ToString();
                   */
                //  AppProperties.vehicle.VehicleCategory
                /*
                if((vehcile_Def_Sev_Fine != null) && (vehcile_Def_Sev_Fine.Count() > 0))
                {
                    AppProperties.vehicle.GroundingDays = ((null != vehcile_Def_Sev_Fine[6])&& (("" != vehcile_Def_Sev_Fine[6].Trim()))) ? vehcile_Def_Sev_Fine[6].Trim() : "NA";
                    AppProperties.vehicle.RequiredGrounding = ((null != vehcile_Def_Sev_Fine[5]) && (("" != vehcile_Def_Sev_Fine[5].Trim()))) ? vehcile_Def_Sev_Fine[5].Trim() : "NA";
                    AppProperties.vehicle.BlackPoints = ((null != vehcile_Def_Sev_Fine[4]) && (("" != vehcile_Def_Sev_Fine[4].Trim()))) ? vehcile_Def_Sev_Fine[4].Trim() : "NA";
                    AppProperties.vehicle.FineAmmount = ((null != vehcile_Def_Sev_Fine[3]) && (("" != vehcile_Def_Sev_Fine[4].Trim()))) ? vehcile_Def_Sev_Fine[3].Trim() : "NA";
               }*/
                int grounding_days = ((null != vehcile_Def_Sev_Fine[6]) && (("" != vehcile_Def_Sev_Fine[6].Trim()))) ? Int32.Parse(vehcile_Def_Sev_Fine[6]) : 0;

                return grounding_days;
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
                return 0;
            }
        }

        private void btnAdd_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                //stop image blink
                stopImageBlinkAndChangeImg();

                // imagebuttonetest2.Source = new BitmapImage(new Uri(@"/Images/Buttons/Large/Login.png", UriKind.Relative));
                if (AppProperties.Selected_Resource == "Arabic")
                {
                    btnAddImage.Source = new BitmapImage(new Uri(@"/Images/Buttons/Large/Add Arabic Up.png", UriKind.Relative));
                    _iValidate = (IValidation)new RecordViolationDefectsInputValidationAr();
                    _validationResult = _iValidate.Validate(this);

                    if (_validationResult != "Valid")
                    {
                        WPFMessageBoxResult _Result = WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), _validationResult);
                        return;
                    }
                    BtnArEnHandler();
                }
                else
                {
                    btnAddImage.Source = new BitmapImage(new Uri(@"/Images/Buttons/Large/Add.png", UriKind.Relative));
                    _iValidate = (IValidation)new RecordViolationDefectsInputValidation();
                    _validationResult = _iValidate.Validate(this);

                    if (_validationResult != "Valid")
                    {
                        WPFMessageBoxResult _Result = WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), _validationResult);
                        return;
                    }
                    BtnAddEnHandler();
                }
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (grdAddedDefects.SelectedItem == null)
                    return;
                data.Violation.Defects selectedDefect = (data.Violation.Defects)grdAddedDefects.SelectedItem;
                if (selectedDefect == null)
                    return;
                this.txtImpounding.Text = (Int32.Parse(txtImpounding.Text) - Int32.Parse(selectedDefect.ImpoundingDays)).ToString();
                if (selectedDefect.DefectCode != null)
                {
                    if (selectedDefect.DefectCode.Equals(Properties.Settings.Default.telematicsDefectCode))
                    {
                        this.txtDriverLiscenseNumber.Text = "";
                        this.txtDriverName.Text = "";
                        this.txtDriverNationality.Text = "";
                        this.txttickCross.Text = "";
                    }
                }
                AddedDefects.Remove(selectedDefect);
                // AppProperties.selectedDefectsEn.RemoveAt(grdAddedDefects.SelectedIndex);
                AppProperties.selectedDefectsEn.RemoveAll((x) => x.Contains(selectedDefect.DefectID.ToString()));

                //-kashif abbasi 19-Nov-2015 --Delete Image of this deleted defects------
                deleteAllImagesOfDefect();
                //--------------------------------------------------

                grdAddedDefects.ItemsSource = null;
                grdAddedDefects.ItemsSource = AddedDefects;


                changeImageRadishToOrignal(selectedDefect);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
            // data.Violation.Defects selectedDefct = 

        }

        private void btnBack_Click_1(object sender, RoutedEventArgs e)
        {
            btnBackImage.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Back.png", UriKind.Relative));
            if (AppProperties.Selected_Resource == "English")
            {
                btnBackImage.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Back.png", UriKind.Relative));
            }
            else
            {
                btnBackImage.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/back Arabic Up.png", UriKind.Relative));
            }
            //------added by Kashif abbasi on dated 22-Nov-2015 ----------------
            bool result = CommonUtils.deleteImgDirectory("ucDefectAndViolationDetails");//delete the defect image diractory
            //-------------------------------------------------------------------
            if (result)
            {
                if (_fromRecordViolationFlow == true)
                {
                    this.m_mainWindow.MainContentControl.Content = null;
                    this.m_mainWindow.MainContentControl.Content = new ucVehicleSelection(m_mainWindow);
                }
                else
                {
                    if (AppProperties.isFlowFromOperator == true)
                    {
                        this.m_mainWindow.MainContentControl.Content = null;
                        this.m_mainWindow.MainContentControl.Content = new ucSearchOperatorProfile(m_mainWindow);
                        AppProperties.isFlowFromOperator = false;
                    }

                    else
                    {
                        this.m_mainWindow.MainContentControl.Content = null;
                        ucSearchedVehicleDetials SearchedVehicleDetials = new ucSearchedVehicleDetials(this.m_mainWindow);
                        SearchedVehicleDetials.SetVehicleRatting();
                        this.m_mainWindow.MainContentControl.Content = SearchedVehicleDetials;
                    }

                }
            }
        }

        public void SbmitViolation()
        {
            try
            {
                App.VSDLog.Info("\nentered in ucDefectAndViolationDetails.SbmitViolation() function\n");
                // EnableDisableDriverInfoFields(true);
                if (AppProperties.vehicle == null)
                {
                    AppProperties.vehicle = new Vehicle();
                }
                //*******************************************
                //Plate Confiscation Detials
                //********************************************
                if (!AppProperties.isSafety)
                {
                    AppProperties.recordedViolation.IsConfiscated = true;
                    AppProperties.recordedViolation.PlateCondition = ""; //this.cmboxPlateCndition.SelectedValue.ToString();
                    AppProperties.recordedViolation.ConfiscationReason = "";// this.txtReasonForConfiscation.Text.ToString();
                }
                //*******************************************
                //Driver Detials
                //********************************************
                // AppProperties.vehicle.DriverName = this.txtDriver.Text;
                if (AppProperties.Selected_Resource == "Arabic")
                {
                    AppProperties.vehicle.DriverCountry = (string)citiesTable[(string)cmboxDriverCountry.SelectedItem];
                    AppProperties.vehicle.DriverEmirates = (string)emirateTable[(string)cmboxEmirates.SelectedItem];

                    AppProperties.vehicle.DriverName = this.txtDriverName.Text;
                    AppProperties.vehicle.DriverNameAr = this.txtDriverName.Text;

                }
                else
                {
                    AppProperties.vehicle.DriverCountry = ((string)(this).cmboxDriverCountry.Text).Trim();
                    AppProperties.vehicle.DriverEmirates = ((string)(this).cmboxEmirates.Text).Trim();

                    AppProperties.vehicle.DriverName = this.txtDriverName.Text;
                    AppProperties.vehicle.DriverNameAr = this.txtDriverName.Text;

                }

                AppProperties.vehicle.DriverLicense = this.txtDriverLiscenseNumber.Text;
                //  AppProperties.recordedViolation.ViolationComments = this.txtComents.Text;

                AppProperties.vehicle.IsHazard = (bool)chkIsHazard.IsChecked;


                ////Defect Summary

                DefectSummaryScreen summary_Screen = new DefectSummaryScreen(m_mainWindow);
                summary_Screen.PopulateDefectSummary(AddedDefects);
                bool retVal = (bool)summary_Screen.ShowDialog();


                if (AppProperties.Is_SubmitVilation == false)
                    return;



                // if (MessageBox.Show(lblSubmitViolation.Content.ToString(), lblSubmitViolationMessage.Content.ToString(), MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                // if (WPFMessageBox.Show(new CommonUtils().GetStringValue("Confirmation"), new CommonUtils().GetStringValue("lblSubmitViolation"), WPFMessageBoxButtons.YesNo, WPFMessageBoxImage.Question) == WPFMessageBoxResult.Yes)
                if (AppProperties.Is_SubmitVilation == true)
                {
                    // this.m_mainWindow.MainContentControl.Content = null;
                    // this.m_mainWindow.MainContentControl.Content = new ucViolationSummary(m_mainWindow);
                    //return;

                    //   ProgressDialogResult result = ProgressDialog.ProgressDialog.Execute(this.m_mainWindow, lblSubmitViolationMessage.Content.ToString(), (bw, we) =>
                    // {
                    //    AppProperties.isOnline = ((IViolation)ViolationManager.GetInstance()).SubmitViolation();
                    // });



                    ProgressDialogResult result_offline = ProgressDialog.ProgressDialog.Execute(this.m_mainWindow, new CommonUtils().GetStringValue("lblSubmitViolationMessage"), (bw, we) =>
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
                        if (!AppProperties.isNetworkConnected)
                        {
                            WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("lblOfflinDeviceMessage"), AppProperties.errorMessageFromBusiness);
                        }
                        else
                        {
                            WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("ErrorBusiness"), AppProperties.errorMessageFromBusiness);

                        } AppProperties.businessError = false;
                        this.m_mainWindow.MainContentControl.Content = null;
                        //this.m_mainWindow.MainContentControl.Content = new ucLocationSelectionEn(m_mainWindow);
                        this.m_mainWindow.MainContentControl.Content = new ucWellComeScreen(m_mainWindow);
                        //   this.m_mainWindow.MainContentControl.Content = null;
                        //  this.m_mainWindow.MainContentControl.Content = new ucLocationSelectionEn(m_mainWindow);
                        //  LandingScreenEn landing = new LandingScreenEn();
                        //_render.switchDisplay(form, landing);
                        return;
                    }
                    if (AppProperties.IsException)
                    {
                        App.VSDLog.Info("\nIsException in PopulateData function:" + AppProperties.IsException);
                        AppProperties.IsException = false;
                        if (!AppProperties.isNetworkConnected)
                        {
                            WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("lblOfflinDeviceMessage"), AppProperties.errorMessageFromBusiness);
                        }
                        else
                        {
                            WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("ErrorException"), AppProperties.errorMessageFromBusiness);
                        }
                        App.VSDLog.Info("\nIsStore Offline Data:" + AppProperties.IsStoreOfflineData);
                        if (AppProperties.IsStoreOfflineData)
                        {
                            // IsStoreOfflineData
                            WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("offlinedata"));
                            AppProperties.IsStoreOfflineData = false;
                        }
                        else
                        {
                            WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("UnableToStoreLocalData"));
                        }
                        this.m_mainWindow.MainContentControl.Content = null;
                        //this.m_mainWindow.MainContentControl.Content = new ucLocationSelectionEn(m_mainWindow);
                        this.m_mainWindow.MainContentControl.Content = new ucWellComeScreen(m_mainWindow);

                        // return;
                    }
                    if (AppProperties.NotFoundError)
                    {
                        AppProperties.NotFoundError = false;
                        if (!AppProperties.isNetworkConnected)
                        {
                            WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("lblOfflinDeviceMessage"), AppProperties.errorMessageFromBusiness);
                        }
                        else
                        {
                            WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("ErrorException"), AppProperties.errorMessageFromBusiness);
                        }
                        this.m_mainWindow.MainContentControl.Content = null;
                        //this.m_mainWindow.MainContentControl.Content = new ucLocationSelectionEn(m_mainWindow);
                        this.m_mainWindow.MainContentControl.Content = new ucWellComeScreen(m_mainWindow);
                        //  return;
                    }
                    if (AppProperties.IsServiceResponseNull)
                    {
                        App.VSDLog.Info("\n Submit Violation Response is NULL:");
                        if (!AppProperties.isNetworkConnected)
                        {
                            WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("lblOfflinDeviceMessage"), AppProperties.errorMessageFromBusiness);
                        }
                        else
                        {
                            WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("ErrorBusiness"), AppProperties.errorMessageFromBusiness);
                        }
                        this.m_mainWindow.MainContentControl.Content = null;
                        this.m_mainWindow.MainContentControl.Content = new ucWellComeScreen(m_mainWindow);
                    }

                    if (AppProperties.vehicle == null)
                    {
                        App.VSDLog.Info("\n Submit Violation Response is NULL:");
                        this.m_mainWindow.MainContentControl.Content = null;
                        this.m_mainWindow.MainContentControl.Content = new ucWellComeScreen(m_mainWindow);
                        return;
                    }
                    if (AppProperties.isSafety)
                    {
                        AppProperties.isComprehensive = false;
                        AppProperties.confiscatePlates = false;
                        if (!AppProperties.isNetworkConnected)
                        {
                            WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("lblOfflinDeviceMessage"), AppProperties.errorMessageFromBusiness);
                        }
                        else
                        {
                            WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), AppProperties.errorMessageFromBusiness.ToString());
                        } this.m_mainWindow.MainContentControl.Content = null;
                        this.m_mainWindow.MainContentControl.Content = new ucWellComeScreen(m_mainWindow);
                        return;
                    }
                    if (AppProperties.isOnline)
                    {
                        App.VSDLog.Info("is online app" + AppProperties.isOnline);
                        AppProperties.isComprehensive = false;
                        AppProperties.confiscatePlates = false;
                        this.m_mainWindow.MainContentControl.Content = null;
                        this.m_mainWindow.MainContentControl.Content = new ucViolationSummary(m_mainWindow);
                        return;
                    }
                    else
                    {
                        AppProperties.isComprehensive = false;
                        AppProperties.confiscatePlates = false;
                        //AppProperties.recordedViolation.ViolationTicketCode = "P11.03.03.02.110000523";
                        this.m_mainWindow.MainContentControl.Content = null;
                        this.m_mainWindow.MainContentControl.Content = new ucViolationSummary(m_mainWindow);
                        return;
                    }
                }
                else
                {
                    //  System.Windows.Forms.MessageBox.Show("Select yes from the drop-down first");
                }
            }
            catch (Exception ex)
            {
                App.VSDLog.Info("\nenterted in exception:\n" + ex.Message);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void btnNext_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (AppProperties.Selected_Resource == "English")
                {
                    btnNextImage.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Next.png", UriKind.Relative));
                }
                else
                {
                    btnNextImage.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Next Arabic Up.png", UriKind.Relative));
                }



                //***************************************************************
                //For Handling Defect Details on Next Button

                //*******************************************************************
                List<string[]> dataRows = AppProperties.selectedDefectsEn;
                if (null == AppProperties.recordedViolation)
                {
                    AppProperties.recordedViolation = new Violation();
                }

                Violation.Defects[] defects = new Violation.Defects[dataRows.Count];

                int numberOfdays = 0;


                if (defects.Length > 0)
                {
                    if (AppProperties.Selected_Resource == "Arabic")
                    {
                        _iValidate = (IValidation)new DriverInfoArValidation();
                    }
                    else
                    {
                        _iValidate = (IValidation)new DriverInfoEnValidation();
                    }
                    _validationResult = _iValidate.Validate(this);
                    if (_validationResult != "Valid")
                    {
                        WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), _validationResult);
                        return;
                    }
                    for (int i = 0; i < defects.Length; i++)
                    {
                        string[] row = dataRows[i];
                        defects[i] = new Violation.Defects();
                        defects[i].DefectType = row[1].Trim();
                        defects[i].DefectName = row[4].Trim();
                        defects[i].DefectNameAr = row[9].Trim();
                        defects[i].DefectSeverity = row[5].Trim();
                        defects[i].DefectSeverityAr = row[6].Trim();
                        defects[i].DefectValue = row[7].Trim();
                        defects[i].DefectID = Int32.Parse(row[8].Trim());
                        defects[i].EnforceTesting = row[10].Trim();
                        defects[i].EnforceFine = row[11].Trim();
                        if (defects[i].EnforceTesting != null || defects[i].EnforceTesting != "")
                        {
                            if (defects[i].EnforceTesting.ToString().Equals("F"))
                            {
                                defects[i].DefectSeverity += "(*)";
                                defects[i].DefectSeverityAr += "(*(";
                            }
                        }
                        string vehCat = "";
                        if (AppProperties.vehicle != null)
                            vehCat = AppProperties.vehicle.VehicleCategory;
                        if ((vehCat == "") || (vehCat == null))
                            vehCat = "Heavy Vehicle";

                        string[] Fines_Info = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectsFineInfo(defects[i].DefectID.ToString(), vehCat);


                        defects[i].FineAmount = ((null != Fines_Info[3]) && (("" != Fines_Info[3].Trim()))) ? Fines_Info[3].ToString() : "0";
                        defects[i].FineName = ((null != Fines_Info[1]) && (("" != Fines_Info[1].Trim()))) ? Fines_Info[1].ToString() : "NA";
                        defects[i].FineNameAr = ((null != Fines_Info[2]) && (("" != Fines_Info[2].Trim()))) ? Fines_Info[2].ToString() : "NA";
                        defects[i].FineID = ((null != Fines_Info[5]) && (("" != Fines_Info[5].Trim()))) ? Fines_Info[5].ToString() : "NA";
                        if (AppProperties.vehicle.TotalFineAmmount == null)
                            AppProperties.vehicle.TotalFineAmmount = "0";

                        Int64 previous_value = Int64.Parse(AppProperties.vehicle.TotalFineAmmount);
                        Int64 current_value = Int64.Parse(defects[i].FineAmount);
                        Int64 result = previous_value + current_value;
                        // AppProperties.vehicle.TotalFineAmmount = (Int64.Parse(AppProperties.vehicle.TotalFineAmmount) + Int64.Parse(defects[i].FineAmount).ToString());

                        AppProperties.vehicle.TotalFineAmmount = result.ToString();



                        if (defects[i].DefectType.Trim().Equals("Defect"))
                        {
                            AppProperties.isViolation = true;
                            AppProperties.isSafety = false;
                        }
                        if (defects[i].DefectType.Trim().Equals("Defect") && !AppProperties.isViolation)
                        {
                            AppProperties.isSafety = false;

                        }
                        //  AppProperties.recordedViolation.ViolationSeverity = ((IViolation)ViolationManager.GetInstance()).CalculateSeverity(AppProperties.recordedViolation.ViolationSeverity, row[5].Trim(), Resources.locale_EN);
                        // AppProperties.recordedViolation.ViolationSeverityAr = ((IViolation)ViolationManager.GetInstance()).CalculateSeverity(AppProperties.recordedViolation.ViolationSeverityAr, row[6].Trim(), Resources.locale_AR);
                        AppProperties.recordedViolation.ViolationSeverity = ((IViolation)ViolationManager.GetInstance()).CalculateSeverity(AppProperties.recordedViolation.ViolationSeverity, row[5].Trim(), "");
                        AppProperties.recordedViolation.ViolationSeverityAr = ((IViolation)ViolationManager.GetInstance()).CalculateSeverity(AppProperties.recordedViolation.ViolationSeverityAr, row[6].Trim(), "");

                    }
                    AppProperties.vehicle.TotalImpoundingDays = this.txtImpounding.Text;
                    if (chkBoxGracePeriod.IsChecked == true)
                        AppProperties.vehicle.IsImpoundingGracePeriod = "T";
                    else
                        AppProperties.vehicle.IsImpoundingGracePeriod = "F";
                    AppProperties.vehicle.Mileage = this.txtMileage.Text;

                    AppProperties.recordedViolation.Defect = null;
                    AppProperties.recordedViolation.Defect = defects;
                    string[] info;
                    //if (_resources.GetLocale().Equals(Resources.locale_EN))
                    //{
                    if (!AppProperties.isSafety)
                    {
                        info = ((IViolation)ViolationManager.GetInstance()).GetConfigurationDataForSeverity(AppProperties.recordedViolation.ViolationSeverity, defects.Length);
                    }
                    else
                    {
                        string[] calculation = ((IViolation)ViolationManager.GetInstance()).GetConfigurationDataForSeverity(AppProperties.recordedViolation.ViolationSeverity, defects.Length);
                        if (calculation != null)
                        {
                            numberOfdays = Int32.Parse(calculation[0]);
                            DateTime dt = DateTime.Now;
                            AppProperties.recordedViolation.ViolationDueDays = dt.AddDays(numberOfdays);
                        }
                        //Confiscations Not Required
                        // EnableDisableAddedDefectsOptions(false);
                        // EnableConfiscateOptions(false);
                        EnableDisableDriverInfoFields(false);
                        SbmitViolation();

                        return;
                    }
                    //}
                    //else
                    //{
                    //    info = ((IViolation)ViolationManager.GetInstance()).GetConfigurationDataForSeverity(AppProperties.recordedViolation.ViolationSeverityAr, defects.Length);
                    //}
                    if (null != info)
                    {
                        numberOfdays = Int32.Parse(info[0]);
                        int numberOfVehServSuspDays = Int32.Parse(info[1]);
                        int numberOfCompServSuspDays = Int32.Parse(info[2]);
                        //if (AppProperties.recordedViolation.ViolationSeverity.Equals("Severe",StringComparison.CurrentCultureIgnoreCase))
                        //{
                        //    AppProperties.confiscatePlates = true;

                        //}
                        DateTime currentDate = DateTime.Now;
                        DateTime vehServSuspDays = currentDate;
                        DateTime compServSuspDays = currentDate;

                        if (info[3].Equals("T", StringComparison.CurrentCultureIgnoreCase) && Int32.Parse(info[6]) == 0)
                        {
                            AppProperties.confiscatePlates = true;
                        }

                        AppProperties.receiptTitle = info[4];
                        AppProperties.receiptTitleAr = info[5];


                        AppProperties.vehicle.VehicleSuspensionDate = vehServSuspDays.AddDays(numberOfVehServSuspDays);
                        if (AppProperties.vehicle.Operator != null)
                            AppProperties.vehicle.Operator.CompanySuspensionDate = compServSuspDays.AddDays(numberOfCompServSuspDays);



                        // check if Emirate is null, don't go to confiscation screen
                        IDBDataLoad iDataLoad = ((IDBDataLoad)DBDataLoadManager.GetInstance());

                        if (AppProperties.vehicle.Emirate == null || AppProperties.vehicle.Emirate == "")
                        {
                            iDataLoad.GetCountryProperties(AppProperties.vehicle.Country);
                        }
                        else
                        {
                            iDataLoad.GetCountryProperties(AppProperties.vehicle.Emirate);
                        }
                        if (!AppProperties.canConfiscatePlates)
                        {
                            AppProperties.confiscatePlates = false;
                        }

                        if (AppProperties.confiscatePlates)
                        {
                            DateTime dt = currentDate;

                            AppProperties.recordedViolation.ViolationDueDays = dt.AddDays(numberOfdays);
                            AppProperties.routeFromDefect = true;
                            //Confiscate Option Enable
                            EnableDisableDriverInfoFields(false);
                            //  EnableConfiscateOptions(true);
                            //  PopulateConfiscateOption();
                            //  EnableDisableAddedDefectsOptions(false);
                            // return;
                        }
                        else
                        {
                            DateTime dt = currentDate;
                            AppProperties.recordedViolation.ViolationDueDays = dt.AddDays(numberOfdays);
                            EnableDisableDriverInfoFields(false);
                            //  EnableConfiscateOptions(false);
                            //  PopulateConfiscateOption();
                            //  EnableDisableAddedDefectsOptions(false);
                            // RecordViolationConfirmationScreenEn RVConfirmation = new RecordViolationConfirmationScreenEn();
                            // _render.switchDisplay(form, RVConfirmation);
                            // return;
                        }
                    }
                    else
                    {
                        //throw custom exception to notify that no configuration data exists
                    }
                    SbmitViolation();

                }
                else
                {
                    //Its just an inspection
                    if (AppProperties.Selected_Resource == "Arabic")
                    {
                        _iValidate = (IValidation)new DriverInfoArValidation();
                    }
                    else
                    {
                        _iValidate = (IValidation)new DriverInfoEnValidation();
                    }
                    _validationResult = _iValidate.Validate(this);
                    if (_validationResult != "Valid")
                    {
                        WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), _validationResult);
                        return;
                    }

                    if (AppProperties.vehicle != null)
                        AppProperties.vehicle.IsHazard = (bool)chkIsHazard.IsChecked;

                    AppProperties.Is_SubmitVilation = false;

                    AppProperties.vehicle.TotalImpoundingDays = this.txtImpounding.Text;
                    if (chkBoxGracePeriod.IsChecked == true)
                        AppProperties.vehicle.IsImpoundingGracePeriod = "T";
                    else
                        AppProperties.vehicle.IsImpoundingGracePeriod = "F";
                    AppProperties.vehicle.Mileage = this.txtMileage.Text;

                    if (WPFMessageBox.Show(new CommonUtils().GetStringValue("Confirmation"), new CommonUtils().GetStringValue("lblSubmitInspection"), WPFMessageBoxButtons.YesNo, WPFMessageBoxImage.Question) == WPFMessageBoxResult.Yes)
                    {
                        // ProgressDialogResult result = ProgressDialog.ProgressDialog.Execute(this.m_mainWindow, lblSubmitInspectionMessage.Content.ToString(), (bw, we) =>
                        //  {
                        //     AppProperties.isOnline = ((IViolation)ViolationManager.GetInstance()).SubmitViolation();
                        //  });


                        ProgressDialogResult result = ProgressDialog.ProgressDialog.Execute(this.m_mainWindow, new CommonUtils().GetStringValue("lblSubmitInspectionMessage"), (bw, we) =>
                        {

                            AppProperties.isOnline = ((IViolation)ViolationManager.GetInstance()).SubmitViolation();

                            // So this check in order to avoid default processing after the Cancel button has been pressed.
                            // This call will set the Cancelled flag on the result structure.
                            ProgressDialog.ProgressDialog.CheckForPendingCancellation(bw, we);

                        }, ProgressDialogSettings.WithSubLabelAndCancel);

                        if (result == null || result.Cancelled)
                            return;
                        else if (result.OperationFailed)
                            return;
                        else
                        {
                            if (AppProperties.isInspectionUploaded)
                            {
                                if (AppProperties.Selected_Resource == "English")
                                    //System.Windows.MessageBox.Show("Inspection Uploaded");
                                    WPFMessageBox.Show(new CommonUtils().GetStringValue("Confirmation"), "Inspection Uploaded");
                                else
                                    WPFMessageBox.Show(new CommonUtils().GetStringValue("Confirmation"), "تم تسجيل عملية التفتيش بنجاح");
                            }
                            AppProperties.isInspectionUploaded = false;
                        }


                        //  AppProperties.isOnline = ((IViolation)ViolationManager.GetInstance()).SubmitViolation();

                        if (AppProperties.businessError)
                        {
                            if (!string.IsNullOrEmpty(AppProperties.errorMessageFromBusiness))
                            {
                                WPFMessageBox.Show(new CommonUtils().GetStringValue("Information"), AppProperties.errorMessageFromBusiness, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Information);
                            }
                            else
                            {
                                WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("ErrorBusiness"));
                            }
                            AppProperties.businessError = false;
                            this.m_mainWindow.MainContentControl.Content = null;
                            this.m_mainWindow.MainContentControl.Content = new ucLocationSelectionEn(this.m_mainWindow);

                        }
                        if (AppProperties.IsException)
                        {
                            App.VSDLog.Info("\nIsException in PopulateData function:" + AppProperties.IsException);
                            AppProperties.IsException = false;
                            if (!AppProperties.isNetworkConnected)
                            {
                                WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("lblOfflinDeviceMessage"), AppProperties.errorMessageFromBusiness);
                            }
                            else
                            {
                                WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("ErrorException"), AppProperties.errorMessageFromBusiness);
                            }
                            App.VSDLog.Info("\nIsStore Offline Data:" + AppProperties.IsStoreOfflineData);
                            if (AppProperties.IsStoreOfflineData)
                            {
                                // IsStoreOfflineData
                                WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("offlinedata"));
                                AppProperties.IsStoreOfflineData = false;
                            }
                            else
                            {
                                WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("UnableToStoreLocalData"));
                            }

                        }
                        if (AppProperties.NotFoundError)
                        {
                            WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("ErrorNotFound"));
                            AppProperties.NotFoundError = false;
                            this.m_mainWindow.MainContentControl.Content = null;
                            this.m_mainWindow.MainContentControl.Content = new ucLocationSelectionEn(this.m_mainWindow);
                            return;
                        }
                        AppProperties.recordedViolation = null;
                        AppProperties.recordedViolation = new Violation();
                        AppProperties.recordedViolation.InspectionArea = AppProperties.location;
                        AppProperties.vehicle = null;
                        AppProperties.Total_Vehicle_Inspected = AppProperties.Total_Vehicle_Inspected + 1;
                        if (AppProperties.Selected_Resource == "English")
                        {
                            if (AppProperties.Previous_Selected_LocationEn.Equals(AppProperties.Current_Selected_LocationEn))
                            {
                                AppProperties.Selected_Location_Count = AppProperties.Selected_Location_Count + 1;
                                AppProperties.Previous_Selected_LocationEn = AppProperties.Current_Selected_LocationEn;
                                // AppProperties.Previous_Selected_AreaEn = AppProperties.Previous_Selected_AreaEn
                            }
                            else
                            {
                                AppProperties.Previous_Selected_LocationEn = AppProperties.Current_Selected_LocationEn;
                                AppProperties.Selected_Location_Count = AppProperties.Selected_Location_Count + 1;
                                // AppProperties.Previous_Selected_AreaEn = AppProperties.location.area;
                            }

                        }
                        else
                        {
                            if (AppProperties.Previous_Selected_LocationAr.Equals(AppProperties.Current_Selected_LocationAr))
                            {
                                AppProperties.Selected_Location_Count = AppProperties.Selected_Location_Count + 1;
                                AppProperties.Previous_Selected_LocationAr = AppProperties.Current_Selected_LocationAr;
                                // AppProperties.Previous_Selected_AreaAr = AppProperties.location.area;
                            }
                            else
                            {
                                AppProperties.Previous_Selected_LocationAr = AppProperties.Current_Selected_LocationAr;
                                AppProperties.Selected_Location_Count = AppProperties.Selected_Location_Count + 1;
                                // AppProperties.Previous_Selected_AreaAr = AppProperties.location.area;
                            }
                        }
                        this.m_mainWindow.MainContentControl.Content = null;
                        this.m_mainWindow.MainContentControl.Content = new ucLocationSelectionEn(this.m_mainWindow);
                        return;
                    }


                }



                //}
                //else
                //{
                //    System.Windows.Forms.MessageBox.Show("Select yes from the drop-down first");
                //}
                //  }
                // BtnNextDefectHandler();
            }

            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }
        /*
        public void BtnNextDefectHandler()
        {
            try
            {

                List<string[]> dataRows = AppProperties.selectedDefectsEn;
                if (null == AppProperties.recordedViolation)
                {
                    AppProperties.recordedViolation = new Violation();
                }

                Violation.Defects[] defects = new Violation.Defects[dataRows.Count];

                int numberOfdays = 0;


                if (defects.Length > 0)
                {
                    for (int i = 0; i < defects.Length; i++)
                    {
                        string[] row = dataRows[i];
                        defects[i] = new Violation.Defects();
                        defects[i].DefectType = row[1].Trim();
                        defects[i].DefectName = row[4].Trim();
                        defects[i].DefectSeverity = row[5].Trim();
                        defects[i].DefectValue = row[7].Trim();
                        defects[i].DefectID = Int32.Parse(row[8].Trim());
                        if (defects[i].DefectType.Trim().Equals("Defect"))
                        {
                            AppProperties.isViolation = true;
                            AppProperties.isSafety = false;
                        }
                        if (defects[i].DefectType.Trim().Equals("Defect") && !AppProperties.isViolation)
                        {
                            AppProperties.isSafety = false;

                        }
                        //  AppProperties.recordedViolation.ViolationSeverity = ((IViolation)ViolationManager.GetInstance()).CalculateSeverity(AppProperties.recordedViolation.ViolationSeverity, row[5].Trim(), Resources.locale_EN);
                        // AppProperties.recordedViolation.ViolationSeverityAr = ((IViolation)ViolationManager.GetInstance()).CalculateSeverity(AppProperties.recordedViolation.ViolationSeverityAr, row[6].Trim(), Resources.locale_AR);
                        AppProperties.recordedViolation.ViolationSeverity = ((IViolation)ViolationManager.GetInstance()).CalculateSeverity(AppProperties.recordedViolation.ViolationSeverity, row[5].Trim(), "");
                        AppProperties.recordedViolation.ViolationSeverityAr = ((IViolation)ViolationManager.GetInstance()).CalculateSeverity(AppProperties.recordedViolation.ViolationSeverityAr, row[6].Trim(), "");

                    }

                    AppProperties.recordedViolation.Defect = null;
                    AppProperties.recordedViolation.Defect = defects;
                    string[] info;
                    //if (_resources.GetLocale().Equals(Resources.locale_EN))
                    //{
                    if (!AppProperties.isSafety)
                    {
                        info = ((IViolation)ViolationManager.GetInstance()).GetConfigurationDataForSeverity(AppProperties.recordedViolation.ViolationSeverity, defects.Length);
                    }
                    else
                    {
                        string[] calculation = ((IViolation)ViolationManager.GetInstance()).GetConfigurationDataForSeverity(AppProperties.recordedViolation.ViolationSeverity, defects.Length);
                        if (calculation != null)
                        {
                            numberOfdays = Int32.Parse(calculation[0]);
                            DateTime dt = DateTime.Now;
                            AppProperties.recordedViolation.ViolationDueDays = dt.AddDays(numberOfdays);
                        }
                        //Confiscations Not Required
                        // EnableDisableAddedDefectsOptions(false);
                        // EnableConfiscateOptions(false);
                        EnableDisableDriverInfoFields(false);

                        return;
                    }
                    //}
                    //else
                    //{
                    //    info = ((IViolation)ViolationManager.GetInstance()).GetConfigurationDataForSeverity(AppProperties.recordedViolation.ViolationSeverityAr, defects.Length);
                    //}
                    if (null != info)
                    {
                        numberOfdays = Int32.Parse(info[0]);
                        int numberOfVehServSuspDays = Int32.Parse(info[1]);
                        int numberOfCompServSuspDays = Int32.Parse(info[2]);
                        //if (AppProperties.recordedViolation.ViolationSeverity.Equals("Severe",StringComparison.CurrentCultureIgnoreCase))
                        //{
                        //    AppProperties.confiscatePlates = true;

                        //}
                        DateTime currentDate = DateTime.Now;
                        DateTime vehServSuspDays = currentDate;
                        DateTime compServSuspDays = currentDate;

                        if (info[3].Equals("T", StringComparison.CurrentCultureIgnoreCase) && Int32.Parse(info[6]) == 0)
                        {
                            AppProperties.confiscatePlates = true;
                        }

                        AppProperties.receiptTitle = info[4];
                        AppProperties.receiptTitleAr = info[5];


                        AppProperties.vehicle.VehicleSuspensionDate = vehServSuspDays.AddDays(numberOfVehServSuspDays);
                        AppProperties.vehicle.Operator.CompanySuspensionDate = compServSuspDays.AddDays(numberOfCompServSuspDays);



                        // check if Emirate is null, don't go to confiscation screen
                        IDBDataLoad iDataLoad = ((IDBDataLoad)DBDataLoadManager.GetInstance());

                        if (AppProperties.vehicle.Emirate == null || AppProperties.vehicle.Emirate == "")
                        {
                            iDataLoad.GetCountryProperties(AppProperties.vehicle.Country);
                        }
                        else
                        {
                            iDataLoad.GetCountryProperties(AppProperties.vehicle.Emirate);
                        }
                        if (!AppProperties.canConfiscatePlates)
                        {
                            AppProperties.confiscatePlates = false;
                        }

                        if (AppProperties.confiscatePlates)
                        {
                            DateTime dt = currentDate;

                            AppProperties.recordedViolation.ViolationDueDays = dt.AddDays(numberOfdays);
                            AppProperties.routeFromDefect = true;
                            //Confiscate Option Enable
                            EnableDisableDriverInfoFields(false);
                            //  EnableConfiscateOptions(true);
                            //  PopulateConfiscateOption();
                            //  EnableDisableAddedDefectsOptions(false);
                            return;
                        }
                        else
                        {
                            DateTime dt = currentDate;
                            AppProperties.recordedViolation.ViolationDueDays = dt.AddDays(numberOfdays);
                            EnableDisableDriverInfoFields(false);
                            //  EnableConfiscateOptions(false);
                            //  PopulateConfiscateOption();
                            //  EnableDisableAddedDefectsOptions(false);
                            // RecordViolationConfirmationScreenEn RVConfirmation = new RecordViolationConfirmationScreenEn();
                            // _render.switchDisplay(form, RVConfirmation);
                            return;
                        }
                    }
                    else
                    {
                        //throw custom exception to notify that no configuration data exists
                    }


                }
                else
                {
                    //Its just an inspection



                    if (MessageBox.Show("Would You Like To Submit Inspection !!", "Inspection Submition", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        ProgressDialogResult result = ProgressDialog.ProgressDialog.Execute(this.m_mainWindow, "Submitting Inspection...", (bw, we) =>
                        {
                            AppProperties.isOnline = ((IViolation)ViolationManager.GetInstance()).SubmitViolation();
                        });


                        //  AppProperties.isOnline = ((IViolation)ViolationManager.GetInstance()).SubmitViolation();

                        if (AppProperties.businessError)
                        {
                            System.Windows.Forms.MessageBox.Show(AppProperties.errorMessageFromBusiness);
                            AppProperties.businessError = false;

                        }
                        AppProperties.recordedViolation = null;
                        AppProperties.recordedViolation = new Violation();
                        AppProperties.recordedViolation.InspectionArea = AppProperties.location;
                        AppProperties.vehicle = null;
                        this.m_mainWindow.MainContentControl.Content = null;
                        this.m_mainWindow.MainContentControl.Content = new ucLocationSelectionEn(this.m_mainWindow);
                        return;
                    }
                    else
                    {
                        // EnableDisableDriverInfoFields(false);
                    }
                }
                // EnableDisableDriverInfoFields(true);


            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                MessageBox.Show(ex.Message);
            }
        }
        */
        private void btnNextDefect_Click_1(object sender, RoutedEventArgs e)
        {
            //  BtnNextDefectHandler();
        }
        #endregion

        public ucDefectAndViolationDetails(MainWindow mainWnd)
        {
            InitializeComponent();
            this.m_mainWindow = mainWnd;
            _fromRecordViolationFlow = true;
            AppProperties.isFlowFromOperator = false;
            dialogComments.SetParent(MainGrid);
        }
        public ucDefectAndViolationDetails(MainWindow mainWnd, bool fromReocrdViolation)
        {
            InitializeComponent();
            this.m_mainWindow = mainWnd;
            this._fromRecordViolationFlow = false;
        }


        #region Public Functions
        public void EnableDisableAddedDefectsOptions(bool enable)
        {
            /*
            this.cmboxType.IsEnabled = enable;
            this.cmboxDefectCategory.IsEnabled = enable;
            this.cmboxDefectSubCateogry.IsEnabled = enable;
            this.cmboxDefect.IsEnabled = enable;
            this.grdAddedDefects.IsEnabled = enable;
            this.btnAdd.IsEnabled = enable;
            this.btnNextDefect.IsEnabled = enable;*/
        }

        public void PopulateCountrydata()
        {
            try
            {
                if (cmboxDriverCountry.Items.Count > 0)
                    cmboxDriverCountry.Items.Clear();
                if (_countries == null)
                    _countries = new List<string>();


                if (AppProperties.Selected_Resource == "Arabic")
                {
                    citiesTable = new Hashtable();
                    //citiesTable=CommonUtils.Splitter(((IViolation)ViolationManager.GetInstance()).GetLocation(null, null));

                    citiesTable = CommonUtils.Splitter(((IDBDataLoad)DBDataLoadManager.GetInstance()).GetAllCountriesForNationality());

                    string[] cityArray = new string[citiesTable.Count];
                    citiesTable.Keys.CopyTo(cityArray, 0);

                    _countries = new List<string>(cityArray);
                    _countries.Sort();
                    int i = 0;
                    if (_countries.Contains(""))
                    {
                        _countries.Remove("");
                        cmboxDriverCountry.Items.Add("");
                        i++;
                    }
                    if (_countries.Contains("باكستان"))
                    {
                        _countries.Remove("باكستان");
                        cmboxDriverCountry.Items.Add("باكستان");
                        i++;
                    }
                    if (_countries.Contains("الهند"))
                    {
                        _countries.Remove("الهند");
                        cmboxDriverCountry.Items.Add("الهند");
                        i++;
                    }
                    if (_countries.Contains("أفغانستان"))
                    {
                        _countries.Remove("أفغانستان");
                        cmboxDriverCountry.Items.Add("أفغانستان");
                        i++;
                    }
                    if (_countries.Contains("بنغلاديش"))
                    {
                        _countries.Remove("بنغلاديش");
                        cmboxDriverCountry.Items.Add("بنغلاديش");
                        i++;
                    }

                    if (_countries.Contains("سوريا"))
                    {
                        _countries.Remove("سوريا");
                        cmboxDriverCountry.Items.Add("سوريا");
                        i++;
                    }
                    if (_countries.Contains("نيبال"))
                    {
                        _countries.Remove("نيبال");
                        cmboxDriverCountry.Items.Add("نيبال");
                        i++;
                    }
                    if (_countries.Contains("الفلبين"))
                    {
                        _countries.Remove("الفلبين");
                        cmboxDriverCountry.Items.Add("الفلبين");
                        i++;
                    }
                    if (_countries.Contains("مصر"))
                    {
                        _countries.Remove("مصر");
                        cmboxDriverCountry.Items.Add("مصر");
                        i++;
                    }
                    if (_countries.Contains("إيران"))
                    {
                        _countries.Remove("إيران");
                        cmboxDriverCountry.Items.Add("إيران");
                        i++;
                    }

                    if (_countries.Contains("سلطنة عمان"))
                    {
                        _countries.Remove("سلطنة عمان");
                        cmboxDriverCountry.Items.Add("سلطنة عمان");
                        i++;
                    }
                    if (_countries.Contains("الأردن"))
                    {
                        _countries.Remove("الأردن");
                        cmboxDriverCountry.Items.Add("الأردن");
                        i++;
                    }
                    if (_countries.Contains("سريلانكا"))
                    {
                        _countries.Remove("سريلانكا");
                        cmboxDriverCountry.Items.Add("سريلانكا");
                        i++;

                    }
                    if (_countries.Contains("الصومال"))
                    {
                        _countries.Remove("الصومال");
                        cmboxDriverCountry.Items.Add("الصومال");
                        i++;

                    }
                    if (_countries.Contains("كينيا"))
                    {
                        _countries.Remove("كينيا");
                        cmboxDriverCountry.Items.Add("كينيا");
                        i++;

                    }
                    if (_countries.Contains("جمهورية جنوب السودان"))
                    {
                        _countries.Remove("جمهورية جنوب السودان");
                        cmboxDriverCountry.Items.Add("جمهورية جنوب السودان");
                        i++;
                    }


                    if (_countries.Contains("غانا"))
                    {
                        _countries.Remove("غانا");
                        cmboxDriverCountry.Items.Add("غانا");
                        i++;

                    }
                    for (int j = 0; j < _countries.Count; j++)
                    {
                        cmboxDriverCountry.Items.Add(_countries[j]);
                    }
                    if (cmboxDriverCountry.Items.Count > 0)
                    {
                        // cmboxDriverCountry.SelectedItem = AppProperties.defaultEmirateAr;
                        cmboxDriverCountry.SelectedItem = AppProperties.defaultCountryAr;
                    }

                }
                else
                {
                    citiesTableEng = new Hashtable();

                    
                    citiesTableEng = CommonUtils.Splitter2(((IDBDataLoad)DBDataLoadManager.GetInstance()).GetAllCountriesForNationality());
                    //citiesTableEng = CommonUtils.Splitter2(((IViolation)ViolationManager.GetInstance()).GetLocation(null, null));
                    string[] cityArray = new string[citiesTableEng.Count];
                    citiesTableEng.Keys.CopyTo(cityArray, 0);

                    _countries = new List<string>(cityArray);
                    _countries.Sort();
                    //Sort According to List
                    //   foreach (string str in _countries)
                    //  {
                    //     cmboxDriverCountry.Items.Add(str.Trim());
                    // }

                    int i = 0;
                    if (_countries.Contains(" "))
                    {
                        _countries.Remove(" ");
                        cmboxDriverCountry.Items.Add(" ");
                        //  _countries.Add
                        i++;
                    }
                    if (_countries.Contains("Pakistan"))
                    {
                        _countries.Remove("Pakistan");
                        cmboxDriverCountry.Items.Add("Pakistan");
                        //  _countries.Add
                        i++;
                    }
                    if (_countries.Contains("India"))
                    {
                        _countries.Remove("India");
                        cmboxDriverCountry.Items.Add("India");
                        i++;
                    }
                    if (_countries.Contains("Afghanistan"))
                    {
                        _countries.Remove("Afghanistan");
                        cmboxDriverCountry.Items.Add("Afghanistan");
                        i++;
                    }
                    if (_countries.Contains("Bangladesh"))
                    {
                        _countries.Remove("Bangladesh");
                        cmboxDriverCountry.Items.Add("Bangladesh");
                        i++;
                    }

                    if (_countries.Contains("Syrian Arab Republic"))
                    {
                        _countries.Remove("Syrian Arab Republic");
                        cmboxDriverCountry.Items.Add("Syrian Arab Republic");
                        i++;
                    }
                    if (_countries.Contains("Nepal"))
                    {
                        _countries.Remove("Nepal");
                        cmboxDriverCountry.Items.Add("Nepal");
                        i++;
                    }



                    if (_countries.Contains("Philippines"))
                    {
                        _countries.Remove("Philippines");
                        cmboxDriverCountry.Items.Add("Philippines");
                        i++;
                    }
                    if (_countries.Contains("Egypt"))
                    {
                        _countries.Remove("Egypt");
                        cmboxDriverCountry.Items.Add("Egypt");
                        i++;
                    }


                    if (_countries.Contains("Iran, Islamic Republic of"))
                    {
                        _countries.Remove("Iran, Islamic Republic of");
                        cmboxDriverCountry.Items.Add("Iran, Islamic Republic of");
                        i++;
                    }


                    if (_countries.Contains("Oman"))
                    {
                        _countries.Remove("Oman");
                        cmboxDriverCountry.Items.Add("Oman");
                        i++;
                    }

                    if (_countries.Contains("Jordan"))
                    {
                        _countries.Remove("Jordan");
                        cmboxDriverCountry.Items.Add("Jordan");
                        i++;
                    }
                    if (_countries.Contains("Sri Lanka"))
                    {
                        _countries.Remove("Sri Lanka");
                        cmboxDriverCountry.Items.Add("Sri Lanka");
                        i++;

                    }
                    if (_countries.Contains("Somalia"))
                    {
                        _countries.Remove("Somalia");
                        cmboxDriverCountry.Items.Add("Somalia");
                        i++;

                    }


                    if (_countries.Contains("Kenya"))
                    {
                        _countries.Remove("Kenya");
                        cmboxDriverCountry.Items.Add("Kenya");
                        i++;

                    }

                    if (_countries.Contains("Republic of South Sudan"))
                    {
                        _countries.Remove("Republic of South Sudan");
                        cmboxDriverCountry.Items.Add("Republic of South Sudan");
                        i++;
                    }

                    if (_countries.Contains("Ghana"))
                    {
                        _countries.Remove("Ghana");
                        cmboxDriverCountry.Items.Add("Ghana");
                        i++;

                    }

                    for (int j = 0; j < _countries.Count; j++)
                    {
                        cmboxDriverCountry.Items.Add(_countries[j]);
                    }



                    if (cmboxDriverCountry.Items.Count > 0)
                    {
                        // cmboxDriverCountry.SelectedItem = AppProperties.defaultEmirateAr;
                        cmboxDriverCountry.SelectedIndex = 0;
                        cmboxDriverCountry.SelectedItem = AppProperties.defaultCountry;
                    }

                    //    ///
                    //    _countries = new List<string>(((IDBDataLoad)DBDataLoadManager.GetInstance()).GetCountries());
                    //    _countries.Sort();




                    //    // string[] cityArray = new string[citiesTable.Count];
                    //    //citiesTable.Keys.CopyTo(cityArray, 1);
                    //    //_emirates = new List<string>(cityArray);
                    //    foreach (string str in _countries)
                    //    {
                    //        cmboxDriverCountry.Items.Add(str.Trim());
                    //    }
                    //    if (cmboxDriverCountry.Items.Count > 0)
                    //    {
                    //        // cmboxDriverCountry.SelectedItem = AppProperties.defaultEmirate;
                    //        cmboxDriverCountry.SelectedIndex = 0;
                    //    }
                }
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        public void PopulateData()
        {
            try
            {

                defectTypeList = new List<string>();
                defectTypeList.Add((new CommonUtils().GetStringValue("Defect")));
                cmboxType.Items.Add(new CommonUtils().GetStringValue("Defect"));
                // cmboxType.Items.Add(new CommonUtils().GetStringValue("SafetyViolation"));
                if (cmboxType.Items.Count > 0)
                    cmboxType.SelectedIndex = 0;

                PopulateCountrydata();
                //defectList = new List<string>(((IViolation)ViolationManager.GetInstance()).GetDefects(null, "maincat", null));
                //if (defectList != null)
                //{
                //    foreach (string defect in defectList)
                //    {
                //        cmboxDefectCategory.Items.Add(defect.Trim());
                //    }
                //}
                //if (cmboxDefectCategory.Items.Count > 0)
                //{
                //    cmboxDefectCategory.SelectedIndex = 0;
                //}


                lblDefImgCount.Content = "";
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }

        }
        public void EnableDisableDriverInfoFields(bool enable)
        {
            /*   this.txtDriver.IsEnabled = enable;
               this.txtDriverLiscenseNumber.IsEnabled = enable;
               this.txtComents.IsEnabled = enable;
               this.btnNext.IsEnabled = enable;

            this.txtDriver.IsReadOnly = enable;
            this.txtDriverLiscenseNumber.IsReadOnly = enable;
            this.txtComents.IsReadOnly = enable;
             */
            // this.btnNext.IsReadOnly = enable;
        }
        public void PopulateConfiscateOption()
        {
            try
            {
                /*
                if (cmboxIsPlateConfiscated.Items.Count == 0)
                {
                    cmboxIsPlateConfiscated.Items.Add(new CommonUtils().GetStringValue("Yes"));
                    cmboxIsPlateConfiscated.Items.Add(new CommonUtils().GetStringValue("No"));
                    cmboxIsPlateConfiscated.SelectedIndex = 1;
                }
                if (cmboxPlateCndition.Items.Count == 0)
                {
                    // cmboxPlateCndition.Items.Add("Good Condition");
                    cmboxPlateCndition.Items.Add(new CommonUtils().GetStringValue("GoodCondition"));
                    // = new CommonUtils().GetStringValue("Good Condition");
                    cmboxPlateCndition.Items.Add(new CommonUtils().GetStringValue("Replaceable"));
                    cmboxPlateCndition.SelectedIndex = 0;
                }
                bool enable = false;
                if (AppProperties.vehicle == null || AppProperties.vehicle.Recomendation == "")
                {
                    enable = false;
                    m_IsPlateConfiscation = false;
                }
                else
                {
                    enable = true;
                    m_IsPlateConfiscation = true;
                }
                // Fill the Fields
                if (AppProperties.routeFromDefect)
                {
                    if (AppProperties.Selected_Resource == "English")
                    {
                        txtReasonForConfiscation.Text = AppProperties.recordedViolation.ViolationSeverity;
                    }
                    else
                    {
                        txtReasonForConfiscation.Text = AppProperties.recordedViolation.ViolationSeverityAr;
                    }
                }
                else
                {
                    if (AppProperties.isComprehensive)
                    {
                        txtReasonForConfiscation.Text = new CommonUtils().GetStringValue("Majorandoverdue");
                    }
                }
                // EnableConfiscateOptions(enable);
                */

            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }
        public void EnableConfiscateOptions(bool enable)
        {
            //  this.txtIsPlateConfisReq.IsEnabled = enable;
            //  this.txtReasonForConfiscation.IsEnabled = enable;
            //  this.cmboxPlateCndition.IsEnabled = enable;
            //  this.cmboxIsPlateConfiscated.IsEnabled = enable;
        }

        public void ClearFieldOnSelectionChange()
        {
            // this.txtDriver.Text = "";
            this.cmboxDriverCountry.SelectedIndex = 0;
            this.txtDriverLiscenseNumber.Text = "";
            this.txtDriverName.Text = "";
            this.txtDriverNationality.Text = "";
            this.cmboxEmirates.SelectedIndex = 0;

            AddedDefects = new List<data.Violation.Defects>();
            AppProperties.selectedDefectsEn.Clear();
            grdAddedDefects.ItemsSource = null;
        }
        public void ValidateAddedDefects()
        {

        }
        #endregion

        private void btnRestDefects_Click_1(object sender, RoutedEventArgs e)
        {
            this.m_mainWindow.MainContentControl.Content = null;
            AppProperties.confiscatePlates = false;
            AppProperties.recordedViolation.ViolationSeverity = "";
            AppProperties.recordedViolation.ViolationSeverityAr = "";
            this.m_mainWindow.MainContentControl.Content = new ucDefectAndViolationDetails(this.m_mainWindow);
        }

        private void txtActualValue_GotFocus_1(object sender, RoutedEventArgs e)
        {
            CommonUtils.ShowKeyBoard();
        }
        private void txtComments_GotFocus_1(object sender, RoutedEventArgs e)
        {
            ComentsPopup.IsOpen = false;
            if (!cmboxDefect.SelectedValue.Equals(""))
            {
                string res = dialogComments.ShowHandlerDialog(txtActualValue.Text, cmboxDefect.SelectedValue.ToString().Trim());
                if (!res.Equals(""))
                    txtActualValue.Text = res;
            }
            CommonUtils.ShowKeyBoard();
            //txtActualValue.Focus();

        }

        private void txtActualValue_LostFocus_1(object sender, RoutedEventArgs e)
        {
            CommonUtils.CLoseKeyBoard();

            if (ComentsPopup.IsOpen)
                ComentsPopup.IsOpen = false;

            //List<string> list = new List<string>();   //cmboxDriverCountry.ItemsSource;
            //foreach (var item in cmboxDriverCountry.Items)
            //{
            //    list.Add(item.ToString());
            //}

            if (!(cmboxType.Text == new CommonUtils().GetStringValue("Defect")))
            {
                cmboxType.Text = "";
                cmboxDefectCategory.Text = "";
                cmboxDefectSubCateogry.Text = "";
                cmboxDefect.Text = "";
            }
            //if (!new CommonUtils().isWordFoundInList(list, cmboxDriverCountry.Text))
            //{
            //    cmboxDriverCountry.Text = "";

            //}
            //if (!new CommonUtils().isWordFoundInList(_countries, cmboxDriverCountry.Text))
            //{
            //    cmboxDriverCountry.Text = "";

            //}


            //  defectList

            if (!new CommonUtils().isWordFoundInList(defectList, cmboxDefectCategory.Text))
            {
                cmboxDefectCategory.Text = "";
            }
            if (!new CommonUtils().isWordFoundInList(defectSubsList, cmboxDefectSubCateogry.Text))
            {
                cmboxDefectSubCateogry.Text = "";
            }
            if (!new CommonUtils().isWordFoundInList(defectNameList, cmboxDefect.Text))
            {
                cmboxDefect.Text = "";
            }
            //if (cmboxDriverCountry.Items.Count < 200)
            //{
            //    cmboxDriverCountry.Items.Clear();
            //    foreach (var item in _countries)
            //    {
            //        cmboxDriverCountry.Items.Add(item);
            //    }
            //}
        }

        private void btnAddImage_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {

            if (AppProperties.Selected_Resource == "English")
            {
                btnAddImage.Source = new BitmapImage(new Uri(@"/Images/Buttons/Large/Add Down.png", UriKind.Relative));
            }
            else
            {
                btnAddImage.Source = new BitmapImage(new Uri(@"/Images/Buttons/Large/Add Arabic Down.png", UriKind.Relative));
            }
        }

        private void btnNextImage_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            if (AppProperties.Selected_Resource == "English")
            {
                btnNextImage.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Next Down.png", UriKind.Relative));
            }
            else
            {
                btnNextImage.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Next Arabic Down.png", UriKind.Relative));
            }
        }

        private void btnBackImage_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {

            if (AppProperties.Selected_Resource == "English")
            {
                btnBackImage.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Back Down.png", UriKind.Relative));
            }
            else
            {
                btnBackImage.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/back Arabic Down.png", UriKind.Relative));
            }
        }

        private void txtDriverLiscenseNumber_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            try
            {
                //new CommonUtils().validateTextInteger(sender, e);
                System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.DigitSubstitution = System.Globalization.DigitShapes.None;
                this.UpdateLayout();
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void txtComents_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            try
            {
                new CommonUtils().validateTextCharacter(sender, e);
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void UserControl_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {

            ChangeControlPositions();
            System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.DigitSubstitution = System.Globalization.DigitShapes.None;
            this.UpdateLayout();
            if (new CommonUtils().GetScreenOrientation() == AppProperties.LandScapetMode)
            {


                ChangeControlDimensions(195);
                ChangeButtonsDimensions(95);
                ChangeButtonsDimensions2(195);
                this.grdAddedDefects.Width = 695;
                this.grdAddedDefects.Height = 100;
                ChangeDriversControlsDimensions(220);
            }

            else
            {


                ChangeControlDimensions(190);
                ChangeButtonsDimensions(90);
                ChangeButtonsDimensions2(190);
                this.grdAddedDefects.Width = 385;
                this.grdAddedDefects.Height = 150;
                ChangeDriversControlsDimensions(190);
            }
        }
        public void ChangeControlDimensions(double width)
        {
            this.cmboxType.Width = width;
            this.cmboxDefect.Width = width;
            this.cmboxDefectCategory.Width = width;
            this.cmboxDefectSubCateogry.Width = width;
            this.txtActualValue.Width = width - 25;


            this.imagebtnDriverSearch.Width = width;

            this.txtImpounding.Width = width;
            this.txtMileage.Width = width;

            // this.txtYear.Width = width;

            this.UpdateLayout();
        }
        public void ChangeDriversControlsDimensions(double width)
        {
            this.cmboxDriverCountry.Width = width;
            this.txtDriverLiscenseNumber.Width = width - 40;
            this.txtDriverName.Width = width;
            this.txtDriverNationality.Width = width;
            this.cmboxEmirates.Width = width;
            this.btnSearchLicNo.Width = width;
        }


        public void ChangeButtonsDimensions(double width)
        {
            this.btnBack.Width = width;
            this.btnNext.Width = width;

            this.UpdateLayout();
        }
        public void ChangeButtonsDimensions2(double width)
        {
            this.btnAdd.Width = width;
            // this.btnSearchLicNo.Width = width;
        }
        public void ChangeLableDimensions(double width)
        {
            this.lblSelectType.FontSize = width;
            this.lblCat.FontSize = width;
            this.lblSubCat.FontSize = width;
            this.lblDefect.FontSize = width;
            this.lblComments.FontSize = width;

            this.lblemirates.FontSize = width;
            this.lblLicNo.FontSize = width;
            this.cmboxDriverCountry.FontSize = width;
            this.lblDriverName.FontSize = width;
            this.lblDRR.FontSize = width;
            this.lblImpounding.FontSize = width;
            this.lblMileage.FontSize = width;

            this.lblDriverName.FontSize = width;
            // this.lblYear.FontSize = width;
            // this.lblAppLogout.FontSize = 20;
            this.UpdateLayout();
        }
        public void ChangeHeaderDimensions(double width)
        {
            // this.lblAddDefects.FontSize = width;
            // this.lblDriverData.FontSize = width;
            this.UpdateLayout();
        }
        public void ChangeControlPositions()
        {
            try
            {


                if (new CommonUtils().GetScreenOrientation() == AppProperties.LandScapetMode)
                {

                    grdAddDefect.Children.Remove(lblSelectType);
                    Grid.SetRow(lblSelectType, 4);
                    Grid.SetColumn(lblSelectType, 3);
                    grdAddDefect.Children.Add(lblSelectType);

                    grdAddDefect.Children.Remove(cmboxType);
                    Grid.SetRow(cmboxType, 4);
                    Grid.SetColumn(cmboxType, 5);
                    grdAddDefect.Children.Add(cmboxType);

                    grdAddDefect.Children.Remove(lblCat);
                    Grid.SetRow(lblCat, 4);
                    Grid.SetColumn(lblCat, 7);
                    grdAddDefect.Children.Add(lblCat);

                    grdAddDefect.Children.Remove(cmboxDefectCategory);
                    Grid.SetRow(cmboxDefectCategory, 4);
                    Grid.SetColumn(cmboxDefectCategory, 9);
                    grdAddDefect.Children.Add(cmboxDefectCategory);

                    grdAddDefect.Children.Remove(lblSubCat);
                    Grid.SetRow(lblSubCat, 6);
                    Grid.SetColumn(lblSubCat, 3);
                    grdAddDefect.Children.Add(lblSubCat);

                    grdAddDefect.Children.Remove(cmboxDefectSubCateogry);
                    Grid.SetRow(cmboxDefectSubCateogry, 6);
                    Grid.SetColumn(cmboxDefectSubCateogry, 5);
                    grdAddDefect.Children.Add(cmboxDefectSubCateogry);

                    grdAddDefect.Children.Remove(lblDefect);
                    Grid.SetRow(lblDefect, 6);
                    Grid.SetColumn(lblDefect, 7);
                    grdAddDefect.Children.Add(lblDefect);

                    grdAddDefect.Children.Remove(cmboxDefect);
                    Grid.SetRow(cmboxDefect, 6);
                    Grid.SetColumn(cmboxDefect, 9);
                    grdAddDefect.Children.Add(cmboxDefect);

                    grdAddDefect.Children.Remove(lblComments);
                    Grid.SetRow(lblComments, 8);
                    Grid.SetColumn(lblComments, 3);
                    grdAddDefect.Children.Add(lblComments);

                    grdAddDefect.Children.Remove(stkPnlComments);
                    Grid.SetRow(stkPnlComments, 8);
                    Grid.SetColumn(stkPnlComments, 5);
                    grdAddDefect.Children.Add(stkPnlComments);

                    grdAddDefect.Children.Remove(chkIsHazard);
                    Grid.SetRow(chkIsHazard, 8);
                    Grid.SetColumn(chkIsHazard, 7);
                    grdAddDefect.Children.Add(chkIsHazard);


                    grdAddDefect.Children.Remove(btnAdd);
                    Grid.SetRow(btnAdd, 8);
                    Grid.SetColumn(btnAdd, 9);
                    grdAddDefect.Children.Add(btnAdd);

                    //  grdAddedDefects

                    grdAddDefect.Children.Remove(grdAddedDefects);
                    Grid.SetRow(grdAddedDefects, 10);
                    Grid.SetColumn(grdAddedDefects, 3);
                    Grid.SetColumnSpan(grdAddedDefects, 7);
                    grdAddDefect.Children.Add(grdAddedDefects);


                    //Impounding

                    grdAddDefect.Children.Remove(lblImpounding);
                    Grid.SetRow(lblImpounding, 12);
                    Grid.SetColumn(lblImpounding, 3);
                    grdAddDefect.Children.Add(lblImpounding);

                    grdAddDefect.Children.Remove(txtImpounding);
                    Grid.SetRow(txtImpounding, 12);
                    Grid.SetColumn(txtImpounding, 5);
                    grdAddDefect.Children.Add(txtImpounding);


                    grdAddDefect.Children.Remove(viewBoxGracePeriod);
                    Grid.SetRow(viewBoxGracePeriod, 12);
                    Grid.SetColumn(viewBoxGracePeriod, 7);
                    Grid.SetColumnSpan(viewBoxGracePeriod, 3);
                    grdAddDefect.Children.Add(viewBoxGracePeriod);

                    grdAddDefect.Children.Remove(lblMileage);
                    Grid.SetRow(lblMileage, 14);
                    Grid.SetColumn(lblMileage, 3);
                    grdAddDefect.Children.Add(lblMileage);

                    grdAddDefect.Children.Remove(txtMileage);
                    Grid.SetRow(txtMileage, 14);
                    Grid.SetColumn(txtMileage, 5);
                    grdAddDefect.Children.Add(txtMileage);

                    grdAddDefect.Children.Remove(canvasDefectImgThumbnil);
                    Grid.SetRow(canvasDefectImgThumbnil, 16);
                    Grid.SetColumn(canvasDefectImgThumbnil, 9);
                    grdAddDefect.Children.Add(canvasDefectImgThumbnil);

                    //Driver


                    //  MainGrid.Children.Remove(lblDriverData);
                    // Grid.SetRow(lblDriverData, 16);
                    //Grid.SetColumn(lblDriverData, 3);
                    //Grid.SetColumnSpan(lblDriverData, 3);
                    //MainGrid.Children.Add(lblDriverData);


                    /*
                                        addDriverDetailGrid.Children.Remove(lblCountry);
                                        Grid.SetRow(lblCountry, 1);
                                        Grid.SetColumn(lblCountry, 3);
                                        addDriverDetailGrid.Children.Add(lblCountry);

                                        addDriverDetailGrid.Children.Remove(cmboxDriverCountry);
                                        Grid.SetRow(cmboxDriverCountry, 1);
                                        Grid.SetColumn(cmboxDriverCountry, 5);
                                        addDriverDetailGrid.Children.Add(cmboxDriverCountry);
                                        */

                    addDriverDetailGrid.Children.Remove(lblCountry);
                    Grid.SetRow(lblCountry, 1);
                    Grid.SetColumn(lblCountry, 3);
                    addDriverDetailGrid.Children.Add(lblCountry);

                    addDriverDetailGrid.Children.Remove(cmboxDriverCountry);
                    Grid.SetRow(cmboxDriverCountry, 1);
                    Grid.SetColumn(cmboxDriverCountry, 5);
                    addDriverDetailGrid.Children.Add(cmboxDriverCountry);

                    addDriverDetailGrid.Children.Remove(lblemirates);
                    Grid.SetRow(lblemirates, 3);
                    Grid.SetColumn(lblemirates, 3);
                    addDriverDetailGrid.Children.Add(lblemirates);

                    addDriverDetailGrid.Children.Remove(cmboxEmirates);
                    Grid.SetRow(cmboxEmirates, 3);
                    Grid.SetColumn(cmboxEmirates, 5);
                    addDriverDetailGrid.Children.Add(cmboxEmirates);


                    addDriverDetailGrid.Children.Remove(lblLicNo);
                    Grid.SetRow(lblLicNo, 3);
                    Grid.SetColumn(lblLicNo, 7);
                    addDriverDetailGrid.Children.Add(lblLicNo);

                    addDriverDetailGrid.Children.Remove(stckPanelDriver);
                    Grid.SetRow(stckPanelDriver, 3);
                    Grid.SetColumn(stckPanelDriver, 9);
                    addDriverDetailGrid.Children.Add(stckPanelDriver);


                    addDriverDetailGrid.Children.Remove(lblDriverName);
                    Grid.SetRow(lblDriverName, 5);
                    Grid.SetColumn(lblDriverName, 3);
                    addDriverDetailGrid.Children.Add(lblDriverName);

                    addDriverDetailGrid.Children.Remove(txtDriverName);
                    Grid.SetRow(txtDriverName, 5);
                    Grid.SetColumn(txtDriverName, 5);
                    addDriverDetailGrid.Children.Add(txtDriverName);


                    //////////////////
                    addDriverDetailGrid.Children.Remove(lblDRR);
                    Grid.SetRow(lblDRR, 9);
                    Grid.SetColumn(lblDRR, 7);
                    addDriverDetailGrid.Children.Add(lblDRR);

                    addDriverDetailGrid.Children.Remove(grdcanvas);
                    Grid.SetRow(grdcanvas, 9);
                    Grid.SetColumn(grdcanvas, 9);
                    addDriverDetailGrid.Children.Add(grdcanvas);


                    addDriverDetailGrid.Children.Remove(lblDriverNationality);
                    Grid.SetRow(lblDriverNationality, 11);
                    Grid.SetColumn(lblDriverNationality, 3);
                    addDriverDetailGrid.Children.Add(lblDriverNationality);

                    addDriverDetailGrid.Children.Remove(txtDriverNationality);
                    Grid.SetRow(txtDriverNationality, 11);
                    Grid.SetColumn(txtDriverNationality, 5);
                    addDriverDetailGrid.Children.Add(txtDriverNationality);





                    addDriverDetailGrid.Children.Remove(btnSearchLicNo);
                    Grid.SetRow(btnSearchLicNo, 11);
                    Grid.SetColumn(btnSearchLicNo, 9);
                    addDriverDetailGrid.Children.Add(btnSearchLicNo);

                    //   addDriverDetailGrid.Children.Remove(btnStackePanel);
                    //    Grid.SetRow(btnStackePanel, 24);
                    //   Grid.SetColumn(btnStackePanel, 9);
                    //  addDriverDetailGrid.Children.Add(btnStackePanel);





                }
                else
                {
                    //portrait mode
                    //  grdAddDefect.Children.Remove(lblAddDefects);
                    // Grid.SetRow(lblAddDefects, 2);
                    // Grid.SetColumn(lblAddDefects, 3);
                    // Grid.SetColumnSpan(lblAddDefects, 3);
                    //grdAddDefect.Children.Add(lblAddDefects);

                    grdAddDefect.Children.Remove(lblSelectType);
                    Grid.SetRow(lblSelectType, 4);
                    Grid.SetColumn(lblSelectType, 3);
                    grdAddDefect.Children.Add(lblSelectType);

                    grdAddDefect.Children.Remove(cmboxType);
                    Grid.SetRow(cmboxType, 4);
                    Grid.SetColumn(cmboxType, 5);
                    grdAddDefect.Children.Add(cmboxType);

                    grdAddDefect.Children.Remove(lblCat);
                    Grid.SetRow(lblCat, 6);
                    Grid.SetColumn(lblCat, 3);
                    grdAddDefect.Children.Add(lblCat);

                    grdAddDefect.Children.Remove(cmboxDefectCategory);
                    Grid.SetRow(cmboxDefectCategory, 6);
                    Grid.SetColumn(cmboxDefectCategory, 5);
                    grdAddDefect.Children.Add(cmboxDefectCategory);

                    grdAddDefect.Children.Remove(lblSubCat);
                    Grid.SetRow(lblSubCat, 8);
                    Grid.SetColumn(lblSubCat, 3);
                    grdAddDefect.Children.Add(lblSubCat);

                    grdAddDefect.Children.Remove(cmboxDefectSubCateogry);
                    Grid.SetRow(cmboxDefectSubCateogry, 8);
                    Grid.SetColumn(cmboxDefectSubCateogry, 5);
                    grdAddDefect.Children.Add(cmboxDefectSubCateogry);

                    grdAddDefect.Children.Remove(lblDefect);
                    Grid.SetRow(lblDefect, 10);
                    Grid.SetColumn(lblDefect, 3);
                    grdAddDefect.Children.Add(lblDefect);

                    grdAddDefect.Children.Remove(cmboxDefect);
                    Grid.SetRow(cmboxDefect, 10);
                    Grid.SetColumn(cmboxDefect, 5);
                    grdAddDefect.Children.Add(cmboxDefect);

                    grdAddDefect.Children.Remove(lblComments);
                    Grid.SetRow(lblComments, 12);
                    Grid.SetColumn(lblComments, 3);
                    grdAddDefect.Children.Add(lblComments);

                    grdAddDefect.Children.Remove(stkPnlComments);
                    Grid.SetRow(stkPnlComments, 12);
                    Grid.SetColumn(stkPnlComments, 5);
                    grdAddDefect.Children.Add(stkPnlComments);

                    //  grdAddedDefects
                    grdAddDefect.Children.Remove(chkIsHazard);
                    Grid.SetRow(chkIsHazard, 14);
                    Grid.SetColumn(chkIsHazard, 3);
                    grdAddDefect.Children.Add(chkIsHazard);

                    grdAddDefect.Children.Remove(btnAdd);
                    Grid.SetRow(btnAdd, 14);
                    Grid.SetColumn(btnAdd, 5);
                    grdAddDefect.Children.Add(btnAdd);


                    //Impounding

                    grdAddDefect.Children.Remove(lblImpounding);
                    Grid.SetRow(lblImpounding, 16);
                    Grid.SetColumn(lblImpounding, 3);
                    grdAddDefect.Children.Add(lblImpounding);

                    grdAddDefect.Children.Remove(txtImpounding);
                    Grid.SetRow(txtImpounding, 16);
                    Grid.SetColumn(txtImpounding, 5);
                    grdAddDefect.Children.Add(txtImpounding);

                    grdAddDefect.Children.Remove(viewBoxGracePeriod);
                    Grid.SetRow(viewBoxGracePeriod, 18);
                    Grid.SetColumn(viewBoxGracePeriod, 3);
                    Grid.SetColumnSpan(viewBoxGracePeriod, 5);
                    grdAddDefect.Children.Add(viewBoxGracePeriod);

                    grdAddDefect.Children.Remove(lblMileage);
                    Grid.SetRow(lblMileage, 20);
                    Grid.SetColumn(lblMileage, 3);
                    grdAddDefect.Children.Add(lblMileage);

                    grdAddDefect.Children.Remove(txtMileage);
                    Grid.SetRow(txtMileage, 20);
                    Grid.SetColumn(txtMileage, 5);
                    grdAddDefect.Children.Add(txtMileage);


                    grdAddDefect.Children.Remove(canvasDefectImgThumbnil);
                    Grid.SetRow(canvasDefectImgThumbnil, 22);
                    Grid.SetColumn(canvasDefectImgThumbnil, 5);
                    grdAddDefect.Children.Add(canvasDefectImgThumbnil);



                    grdAddDefect.Children.Remove(grdAddedDefects);
                    Grid.SetRow(grdAddedDefects, 24);
                    Grid.SetColumn(grdAddedDefects, 1);
                    Grid.SetColumnSpan(grdAddedDefects, 5);
                    grdAddDefect.Children.Add(grdAddedDefects);
                    /*
                        MainGrid.Children.Remove(grdAddedDefects);
                        Grid.SetRow(grdAddedDefects, 16);
                        Grid.SetColumn(grdAddedDefects, 3);
                        Grid.SetColumnSpan(grdAddedDefects, 7);
                        MainGrid.Children.Add(grdAddedDefects);*/


                    // MainGrid.Children.Remove(lblDriverData);
                    // Grid.SetRow(lblDriverData, 22);
                    // Grid.SetColumn(lblDriverData, 3);
                    // Grid.SetColumnSpan(lblDriverData, 5);
                    // MainGrid.Children.Add(lblDriverData);

                    addDriverDetailGrid.Children.Remove(lblCountry);
                    Grid.SetRow(lblCountry, 1);
                    Grid.SetColumn(lblCountry, 3);
                    addDriverDetailGrid.Children.Add(lblCountry);

                    addDriverDetailGrid.Children.Remove(cmboxDriverCountry);
                    Grid.SetRow(cmboxDriverCountry, 1);
                    Grid.SetColumn(cmboxDriverCountry, 5);
                    addDriverDetailGrid.Children.Add(cmboxDriverCountry);

                    addDriverDetailGrid.Children.Remove(lblemirates);
                    Grid.SetRow(lblemirates, 3);
                    Grid.SetColumn(lblemirates, 3);
                    addDriverDetailGrid.Children.Add(lblemirates);

                    addDriverDetailGrid.Children.Remove(cmboxEmirates);
                    Grid.SetRow(cmboxEmirates, 3);
                    Grid.SetColumn(cmboxEmirates, 5);
                    addDriverDetailGrid.Children.Add(cmboxEmirates);

                    addDriverDetailGrid.Children.Remove(lblLicNo);
                    Grid.SetRow(lblLicNo, 5);
                    Grid.SetColumn(lblLicNo, 3);
                    addDriverDetailGrid.Children.Add(lblLicNo);

                    addDriverDetailGrid.Children.Remove(stckPanelDriver);
                    Grid.SetRow(stckPanelDriver, 5);
                    Grid.SetColumn(stckPanelDriver, 5);
                    addDriverDetailGrid.Children.Add(stckPanelDriver);


                    addDriverDetailGrid.Children.Remove(lblDriverName);
                    Grid.SetRow(lblDriverName, 7);
                    Grid.SetColumn(lblDriverName, 3);
                    addDriverDetailGrid.Children.Add(lblDriverName);

                    addDriverDetailGrid.Children.Remove(txtDriverName);
                    Grid.SetRow(txtDriverName, 7);
                    Grid.SetColumn(txtDriverName, 5);
                    addDriverDetailGrid.Children.Add(txtDriverName);




                    addDriverDetailGrid.Children.Remove(lblDRR);
                    Grid.SetRow(lblDRR, 9);
                    Grid.SetColumn(lblDRR, 3);
                    addDriverDetailGrid.Children.Add(lblDRR);

                    addDriverDetailGrid.Children.Remove(grdcanvas);
                    Grid.SetRow(grdcanvas, 9);
                    Grid.SetColumn(grdcanvas, 5);
                    addDriverDetailGrid.Children.Add(grdcanvas);

                    addDriverDetailGrid.Children.Remove(lblDriverNationality);
                    Grid.SetRow(lblDriverNationality, 11);
                    Grid.SetColumn(lblDriverNationality, 3);
                    addDriverDetailGrid.Children.Add(lblDriverNationality);

                    addDriverDetailGrid.Children.Remove(txtDriverNationality);
                    Grid.SetRow(txtDriverNationality, 11);
                    Grid.SetColumn(txtDriverNationality, 5);
                    addDriverDetailGrid.Children.Add(txtDriverNationality);



                    addDriverDetailGrid.Children.Remove(btnSearchLicNo);
                    Grid.SetRow(btnSearchLicNo, 11);
                    Grid.SetColumn(btnSearchLicNo, 9);
                    addDriverDetailGrid.Children.Add(btnSearchLicNo);







                    // MainGrid.Children.Remove(btnStackePanel);
                    // Grid.SetRow(btnStackePanel, 36);
                    // Grid.SetColumn(btnStackePanel, 5);
                    // MainGrid.Children.Add(btnStackePanel);





                }
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }


        private void UserControl_Initialized_1(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.DigitSubstitution = System.Globalization.DigitShapes.None;
            this.UpdateLayout();
            SetVisibility(false);
        }

        private void txtActualValue_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            try
            {
                System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.DigitSubstitution = System.Globalization.DigitShapes.None;
                this.UpdateLayout();
                if (AppProperties.Selected_Resource == "English")
                {
                    if (txtActualValue.Text.Trim() == "")
                    {
                        ComentsPopup.IsOpen = false;
                    }
                    else
                    {
                        //  PopupTextBlock.Text = "Hint: Username and Password is Key Sensitive";
                        PopupTextBlock.Text = new CommonUtils().GetStringValue("ComentsToolTip");
                        ComentsPopup.IsOpen = true;
                    }
                }
                else
                {
                    System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.DigitSubstitution = System.Globalization.DigitShapes.None;
                    this.UpdateLayout();
                    if (txtActualValue.Text.Trim() == "")
                    {
                        ComentsPopup.IsOpen = false;
                    }
                    else
                    {
                        //  PopupTextBlock.Text = "يرجى مراعاة حجم الحروف";
                        PopupTextBlock.Text = new CommonUtils().GetStringValue("ComentsToolTip");
                        ComentsPopup.IsOpen = true;
                    }
                }
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void cmboxDefectSubCateogry_PreviewKeyDown_1(object sender, KeyEventArgs e)
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

        private void txtActualValue_PreviewKeyDown_1(object sender, KeyEventArgs e)
        {


            try
            {
                if (e.Key == Key.Return)
                {
                    btnAdd_Click_1(sender, e);
                    new CommonUtils().ChangeControlFocous(e);

                }
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void txtComents_PreviewKeyDown_1(object sender, KeyEventArgs e)
        {

            try
            {
                if (e.Key == Key.Return)
                {
                    btnNext_Click_1(sender, e);

                }
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void cmboxType_KeyDown_1(object sender, KeyEventArgs e)
        {
            try
            {
                Is_OpenDropDown = true;
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }

        }

        private void txtActualValue_Initialized_1(object sender, EventArgs e)
        {

        }

        private void cmboxType_MouseEnter_1(object sender, MouseEventArgs e)
        {

        }

        private void cmboxCountry_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

            try
                {
                if ((string)cmboxDriverCountry.SelectedItem != "")
                {
                    this.grdcanvas.Visibility = System.Windows.Visibility.Collapsed;
                    this.lblDRR.Visibility = System.Windows.Visibility.Collapsed;
                    if (cmboxEmirates.Items.Count > 0)
                    {
                        cmboxEmirates.Text = "";
                        cmboxEmirates.Items.Clear();
                        cmboxEmirates.SelectedIndex = -1;
                    }
                    if (cmboxDriverCountry.SelectedValue != null)
                    {
                        if (AppProperties.Selected_Resource == "Arabic")
                        {
                           
                            // GetLocationDetails((string)countryTable[(string)selectCountry.SelectedItem], "emirate"));
                           // emirateTable = CommonUtils.Splitter(((IViolation)ViolationManager.GetInstance()).GetLocation((string)citiesTable[(string)cmboxDriverCountry.SelectedItem], "emirate"));
                            emirateTable = CommonUtils.Splitter(((IViolation)ViolationManager.GetInstance()).GetLocation((string)citiesTable[(string)AppProperties.defaultCountry], "emirate"));
                            string[] emirate = new string[emirateTable.Count];
                            emirateTable.Keys.CopyTo(emirate, 0);


                            _emirateList = new List<string>(emirate);
                            _emirateList.Sort();
                            foreach (string str in _emirateList)
                            {
                                cmboxEmirates.Items.Add(str.Trim());
                            }
                            if (cmboxEmirates.Items.Count > 0)
                                cmboxEmirates.SelectedItem = AppProperties.defaultEmirateAr;
                            cmboxEmirates.UpdateLayout();

                            if (cmboxDriverCountry.SelectedValue.ToString().Trim() != AppProperties.defaultCountryAr.Trim())
                            {
                                lblemirates.Visibility = Visibility.Collapsed;
                                cmboxEmirates.Visibility = Visibility.Collapsed;
                                txtDriverNationality.Background = Brushes.White;
                                txtDriverNationality.IsReadOnly = false;
                                this.txtDriverNationality.Text = cmboxDriverCountry.SelectedValue.ToString();
                                txtDriverName.Background = Brushes.White;
                                txtDriverName.IsReadOnly = false;
                               
                            }
                            else
                            {
                                lblemirates.Visibility = Visibility.Visible;
                                cmboxEmirates.Visibility = Visibility.Visible;
                                txtDriverNationality.Background = Brushes.White;
                                txtDriverNationality.IsReadOnly = false;
                                txtDriverName.Background = Brushes.White;
                                txtDriverName.IsReadOnly = false;
                               // cmboxEmirates.UpdateLayout();
                            }
                        }
                        else
                        {
                            
                           // _emirateList = new List<string>(((IViolation)ViolationManager.GetInstance()).GetLocation(cmboxDriverCountry.SelectedValue.ToString(), "emirate"));
                            _emirateList = new List<string>(((IViolation)ViolationManager.GetInstance()).GetLocation(AppProperties.defaultCountry.ToString(), "emirate"));
                            _emirateList.Sort();
                            foreach (string str in _emirateList)
                            {
                                cmboxEmirates.Items.Add(str.Trim());
                            }
                            if (cmboxEmirates.Items.Count > 0)
                                cmboxEmirates.SelectedItem = AppProperties.defaultEmirate;
                            cmboxEmirates.UpdateLayout();

                            if (cmboxDriverCountry.SelectedValue.ToString().Trim() != AppProperties.defaultCountry.Trim())
                            {
                                lblemirates.Visibility = Visibility.Collapsed;
                                cmboxEmirates.Visibility = Visibility.Collapsed;
                                txtDriverNationality.Background = Brushes.White;
                                txtDriverNationality.IsReadOnly = false;
                                this.txtDriverNationality.Text = cmboxDriverCountry.SelectedValue.ToString();
                                txtDriverName.Background = Brushes.White;
                                txtDriverName.IsReadOnly = false;
                            }
                            else
                            {
                                lblemirates.Visibility = Visibility.Visible;
                                cmboxEmirates.Visibility = Visibility.Visible;
                                txtDriverNationality.Background = Brushes.White;
                                txtDriverNationality.IsReadOnly = false;
                                txtDriverName.Background = Brushes.White;
                                txtDriverName.IsReadOnly = false;
                            }
                        }
                    }


                }
                else
                {
                    cmboxDriverCountry.Items.Clear();
                    cmboxDriverCountry.Items.Add("");
                }
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void cmboxDefectCategory_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (!cmboxDefectCategory.IsDropDownOpen)
                cmboxDefectCategory.IsDropDownOpen = true;
        }

        private void cmboxDefectSubCateogry_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (!cmboxDefectSubCateogry.IsDropDownOpen)
                cmboxDefectSubCateogry.IsDropDownOpen = true;
        }

        private void cmboxDefect_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (!cmboxDefect.IsDropDownOpen)
                cmboxDefect.IsDropDownOpen = true;
        }

        private void cmboxCountry_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (!cmboxDriverCountry.IsDropDownOpen)
                cmboxDriverCountry.IsDropDownOpen = true;
        }

        private void txtActualValue_KeyDown_1(object sender, KeyEventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.DigitSubstitution = System.Globalization.DigitShapes.None;
            this.UpdateLayout();
        }

        private void chkIsHazard_Click(object sender, RoutedEventArgs e)
        {
            HazardImageChange();
        }

        private void imagebtnDriverSearch_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (AppProperties.Selected_Resource == "English")
            {
                imagebtnDriverSearch.Source = new BitmapImage(new Uri(@"/Images/Buttons/Large/Search Down.png", UriKind.Relative));
            }
            else
            {
                imagebtnDriverSearch.Source = new BitmapImage(new Uri(@"/Images/Buttons/Large/Search Arabic Down Large.png", UriKind.Relative));
            }

        }


        private void txtDriverName_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Return)
                {
                    this.btnSearchLicNo_Click(sender, e);
                    new CommonUtils().ChangeControlFocous(e);

                }
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }
        private void imagebtnDriverSearch_MouseLeftButtonUp(object sender, KeyEventArgs e)
        {

        }
        public void SearchDriverEnHandler()
        {
            try
            {
                if (AppProperties.Selected_Resource == "Arabic")
                {
                    //_iValidate = (IValidation)new DriverInfoArValidation();
                    DriverInfoArValidation objDriverInfoArValidation = new DriverInfoArValidation();
                    _validationResult = objDriverInfoArValidation.PartialValidate(this);
                }
                else
                {
                   // _iValidate = (IValidation)new DriverInfoEnValidation();
                    DriverInfoEnValidation objDriverInfoEnValidation = new DriverInfoEnValidation();
                    _validationResult = objDriverInfoEnValidation.PartialValidate(this);
                }
               // _validationResult = _iValidate.Validate(this);
                if (_validationResult != "Valid")
                {
                    WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), _validationResult);
                    return;
                }
                string driver_country = string.Empty;
                string driver_emirates = string.Empty;
                string driver_liscNum = string.Empty;
                if (AppProperties.Selected_Resource == "Arabic")
                {
                    // driver_country = (string)citiesTable[(string)cmboxDriverCountry.SelectedItem];
                    driver_emirates = (string)emirateTable[(string)cmboxEmirates.SelectedItem];

                    // AppProperties.vehicle.DriverName = this.txtDriverName.Text;
                    //  AppProperties.vehicle.DriverNameAr = this.txtDriverName.Text;

                }
                else
                {
                    //  driver_country = ((string)(this).cmboxDriverCountry.Text).Trim();
                    driver_emirates = ((string)(this).cmboxEmirates.Text).Trim();


                    // AppProperties.vehicle.DriverName = this.txtDriverName.Text;
                    // AppProperties.vehicle.DriverNameAr = this.txtDriverName.Text;

                }

                driver_liscNum = this.txtDriverLiscenseNumber.Text;
                bool status = false;
                ProgressDialogResult result_offline = ProgressDialog.ProgressDialog.Execute(this.m_mainWindow, new CommonUtils().GetStringValue("lblSearchingDriver"), (bw, we) =>
                {

                    status = ((IDriverDetailManager)DriverDetailManager.GetInstance()).InquireDriverDetails(driver_country, driver_emirates, driver_liscNum);

                    // So this check in order to avoid default processing after the Cancel button has been pressed.
                    // This call will set the Cancelled flag on the result structure.
                    ProgressDialog.ProgressDialog.CheckForPendingCancellation(bw, we);

                }, ProgressDialogSettings.WithSubLabelAndCancel);

                if (result_offline == null || result_offline.Cancelled)
                    return;
                else if (result_offline.OperationFailed)
                    return;

                if (status == true)
                {


                    AppProperties.Is_DriverDataVerified = true;
                    this.txtDriverName.Text = AppProperties.vehicle.DriverName;
                    this.txtDriverNationality.Text = AppProperties.vehicle.DriverCountry;
                    ShowDriverRiskRatting();
                    this.txttickCross.Text = "✓";
                    this.txttickCross.Foreground = Brushes.Green;
                    // this.cmboxDriverCountry.SelectedValue = AppProperties.vehicle.DriverCountry;
                    if (string.IsNullOrEmpty(this.txtDriverNationality.Text) || string.IsNullOrEmpty(this.txtDriverName.Text))
                    {
                        SetVisibility(true);
                    }
                }
                else
                {
                    if (!AppProperties.isNetworkConnected)
                    {
                        WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("lblOfflinDeviceMessage"), AppProperties.errorMessageFromBusiness);
                    }
                    else
                    {
                        WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("ErrorNotFound"));
                    }// this.txtDriverName.Text = AppProperties.vehicle.DriverName;
                    // this.txtDriverLiscenseNumber.Background = Brushes.Red;

                    SetVisibility(true);
                    AppProperties.Is_DriverDataVerified = false;
                    ShowDriverRiskRatting();
                    this.txtDriverName.Text = "";
                    this.txtDriverNationality.Text = "";
                    //  AppProperties.vehicle.DriverCountry =
                    this.txttickCross.Text = "X";
                    this.txttickCross.Foreground = Brushes.Red;

                    // this.cmboxDriverCountry.SelectedItem = (string)citiesTable[(string)cmboxDriverCountry.SelectedItem];

                }


            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void SearchDriverArHandler()
        {
            try
            {
                if (AppProperties.Selected_Resource == "Arabic")
                {
                    DriverInfoArValidation objDriverInfoArValidation = new DriverInfoArValidation();
                    _validationResult = objDriverInfoArValidation.PartialValidate(this);
                    //_iValidate = (IValidation)new DriverInfoArValidation();
                }
                else
                {
                    //_iValidate = (IValidation)new DriverInfoEnValidation();
                    DriverInfoEnValidation objDriverInfoEnValidation = new DriverInfoEnValidation();
                    _validationResult = objDriverInfoEnValidation.PartialValidate(this);
                }
               // _validationResult = _iValidate.Validate(this);
                if (_validationResult != "Valid")
                {
                    WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), _validationResult);
                    return;
                }
                string driver_country = string.Empty;
                string driver_emirates = string.Empty;
                string driver_liscNum = string.Empty;
                if (AppProperties.Selected_Resource == "Arabic")
                {
                    // driver_country = (string)citiesTable[(string)cmboxDriverCountry.SelectedItem];
                    driver_emirates = (string)emirateTable[(string)cmboxEmirates.SelectedItem];

                    // AppProperties.vehicle.DriverName = this.txtDriverName.Text;
                    //  AppProperties.vehicle.DriverNameAr = this.txtDriverName.Text;

                }
                else
                {
                    // driver_country = ((string)(this).cmboxDriverCountry.Text).Trim();
                    driver_emirates = ((string)(this).cmboxEmirates.Text).Trim();


                    // AppProperties.vehicle.DriverName = this.txtDriverName.Text;
                    // AppProperties.vehicle.DriverNameAr = this.txtDriverName.Text;

                }

                driver_liscNum = this.txtDriverLiscenseNumber.Text;
                bool status = false;
                ProgressDialogResult result_offline = ProgressDialog.ProgressDialog.Execute(this.m_mainWindow, new CommonUtils().GetStringValue("lblSearchingDriver"), (bw, we) =>
                {

                    status = ((IDriverDetailManager)DriverDetailManager.GetInstance()).InquireDriverDetails(driver_country, driver_emirates, driver_liscNum);

                    // So this check in order to avoid default processing after the Cancel button has been pressed.
                    // This call will set the Cancelled flag on the result structure.
                    ProgressDialog.ProgressDialog.CheckForPendingCancellation(bw, we);

                }, ProgressDialogSettings.WithSubLabelAndCancel);

                if (result_offline == null || result_offline.Cancelled)
                    return;
                else if (result_offline.OperationFailed)
                    return;

                if (status == true)
                {

                    this.txttickCross.Text = "✓";
                    this.txttickCross.Foreground = Brushes.Green;


                    // string driver_nationlity    = (string)citiesTable[A];
                    this.txtDriverName.Text = AppProperties.vehicle.DriverNameAr;
                  
                    //  var itemKey = citiesTable.Keys.OfType<string>().Where(k => k == AppProperties.vehicle.DriverCountry);
                    //   var itemKey2 = citiesTable.Values.OfType<string>().Where(k => k == AppProperties.vehicle.DriverCountry;
                    //this.txtDriverNationality.Text =(string)citiesTable[AppProperties.vehicle.DriverCountry];




                   // this.txtDriverNationality.Text = AppProperties.vehicle.DriverCountry;

                    #region Commented -Driver Nationality issue
                    string arabic_Nationality = string.Empty;

                    Hashtable countryTable = new Hashtable();
                    //citiesTable=CommonUtils.Splitter(((IViolation)ViolationManager.GetInstance()).GetLocation(null, null));

                    countryTable = CommonUtils.Splitter(((IDBDataLoad)DBDataLoadManager.GetInstance()).GetACountriesForNationalityDriver());

                    string[] cityArray = new string[countryTable.Count];
                    countryTable.Keys.CopyTo(cityArray, 0);

                    foreach (DictionaryEntry ht in countryTable)
                    {
                        if (ht.Value.Equals(AppProperties.vehicle.DriverCountry.Trim()))
                        {
                            arabic_Nationality = ht.Key.ToString();
                        }
                    }
                    this.txtDriverNationality.Text = arabic_Nationality;

                    #endregion

                    ShowDriverRiskRatting();
                    if(string.IsNullOrEmpty(this.txtDriverNationality.Text) || string.IsNullOrEmpty(this.txtDriverName.Text))
                    {
                        SetVisibility(true);
                    }

                }
                else
                {
                    if (!AppProperties.isNetworkConnected)
                    {
                        WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("lblOfflinDeviceMessage"), AppProperties.errorMessageFromBusiness);
                    }
                    else
                    {
                        WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("ErrorNotFound"));
                    }
                    SetVisibility(true);
                    this.txtDriverName.Text = "";
                    this.txtDriverNationality.Text = "";
                    ShowDriverRiskRatting();

                    this.txttickCross.Text = "X";
                    this.txttickCross.Foreground = Brushes.Red;

                }


            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }



        private void ShowDriverRiskRatting()
        {
            try
            {
                if ((AppProperties.vehicle != null) && (AppProperties.vehicle.DriverRiskRattingName != "") && (AppProperties.vehicle.DriverRiskRattingName != null))
                {
                    string risk_rating = AppProperties.vehicle.DriverRiskRattingName;

                    if (canv.Children.Count > 0)
                    {
                        canv.Children.RemoveAt(0);
                    }
                    canv.Visibility = System.Windows.Visibility.Visible;
                    this.grdcanvas.Visibility = System.Windows.Visibility.Visible;
                    lblRattingTextwithColor.Visibility = System.Windows.Visibility.Visible;
                    lblDRR.Visibility = System.Windows.Visibility.Visible;
                    lblRattingTextwithColor.Content = risk_rating;
                    System.Windows.Shapes.Ellipse rect;
                    rect = new System.Windows.Shapes.Ellipse();
                    rect.StrokeThickness = 2;
                    if (risk_rating.Contains("G"))
                    {
                        rect.Stroke = new SolidColorBrush(Colors.ForestGreen);
                        rect.Fill = new SolidColorBrush(Colors.ForestGreen);
                        rect.Width = 50;
                        rect.Height = 40;
                        Canvas.SetLeft(rect, 0);
                        Canvas.SetTop(rect, 0);
                        canv.Children.Add(rect);
                    }
                    else if (risk_rating.Contains("A"))
                    {
                        rect.Stroke = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFCC00"));

                        rect.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFCC00"));
                        rect.Width = 50;
                        rect.Height = 40;
                        Canvas.SetLeft(rect, 0);
                        Canvas.SetTop(rect, 0);
                        canv.Children.Add(rect);
                    }
                    else if (risk_rating.Contains("R"))
                    {
                        rect.Stroke = new SolidColorBrush(Colors.Red);
                        rect.Fill = new SolidColorBrush(Colors.Red);
                        rect.Width = 50;
                        rect.Height = 40;
                        Canvas.SetLeft(rect, 0);
                        Canvas.SetTop(rect, 0);
                        canv.Children.Add(rect);
                    }
                    else if (risk_rating.Contains("New"))
                    {
                        rect.Stroke = new SolidColorBrush(Colors.Gray);
                        rect.Fill = new SolidColorBrush(Colors.Gray);
                        rect.Width = 50;
                        rect.Height = 40;
                        Canvas.SetLeft(rect, 0);
                        Canvas.SetTop(rect, 0);
                        canv.Children.Add(rect);
                    }
                    else
                    {
                        canv.Visibility = System.Windows.Visibility.Collapsed;
                        grdcanvas.Visibility = System.Windows.Visibility.Collapsed;
                        lblRattingTextwithColor.Visibility = System.Windows.Visibility.Collapsed;
                        lblDRR.Visibility = System.Windows.Visibility.Collapsed;
                        AppProperties.vehicle.RiskRating = "";
                    }
                }
                else
                {
                    canv.Visibility = System.Windows.Visibility.Collapsed;
                    grdcanvas.Visibility = System.Windows.Visibility.Collapsed;
                    lblRattingTextwithColor.Visibility = System.Windows.Visibility.Collapsed;
                    lblDRR.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void HazardImageChange()
        {
            if (chkIsHazard.IsChecked == true)
                if (AppProperties.Selected_Resource.Equals("English"))
                    imgHazard.Source = new BitmapImage(new Uri(@"/Images/Icons/Hazard.png", UriKind.Relative));
                else
                    imgHazard.Source = new BitmapImage(new Uri(@"/Images/Icons/Hazard_Ar.png", UriKind.Relative));
            else
                if (AppProperties.Selected_Resource.Equals("English"))
                    imgHazard.Source = new BitmapImage(new Uri(@"/Images/Icons/Hazard_Dimmed.png", UriKind.Relative));
                else
                    imgHazard.Source = new BitmapImage(new Uri(@"/Images/Icons/HazardAr_Dimmed.png", UriKind.Relative));
        }

        #region Images Animation functions
        Storyboard storyboardObj;
        Image _imageObj;
        private void startImageBlink(Image imgName)
        {
            //--first Stop storyBoard if it is already running-----
            if (storyboardObj != null)
                storyboardObj.Stop(this);
            //---------------------------------------------------
            _imageObj = imgName;
            var blink = new DoubleAnimation()
            {
                From = 1,
                To = 0,
                BeginTime = TimeSpan.FromSeconds(0),
                Duration = TimeSpan.FromSeconds(1),
                RepeatBehavior = RepeatBehavior.Forever,
            };

            Storyboard.SetTarget(blink, imgName);
            Storyboard.SetTargetProperty(blink, new PropertyPath(Image.OpacityProperty));

            storyboardObj = new Storyboard();
            storyboardObj.Children.Add(blink);

            storyboardObj.Begin(this, true);
        }

        private void stopImageBlinkAndChangeImg()
        {
            if (storyboardObj != null)
                storyboardObj.Stop(this);

            if (_imageObj != null)
                changImage();
        }
        private void changImage()
        {
            if (_imageObj.Source != null)
            {
                String img_Name = (_imageObj.Source).ToString().Substring((_imageObj.Source).ToString().LastIndexOf("/") + 1).Replace(".", "_R.");
                String strImgSource = getImgsPath() + img_Name;
                if (!strImgSource.Trim().Equals("") && !strImgSource.Contains("_R_R."))
                    _imageObj.Source = new BitmapImage(new Uri(strImgSource, UriKind.Relative));
            }
        }

        private void changeImageRadishToOrignal(Violation.Defects selectedDefect)
        {
            if (grdVehicleImageSlice.IsVisible)
            {
                if (selectedDefect.DefectCategory.Trim().Equals("Wheels and Tires") || selectedDefect.DefectCategory.Trim().Equals("الإطارات"))
                {
                    Image9_WheelAndTyre1.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/HeavyVeh_Image/truck_Wheel_1.gif", UriKind.Relative));
                    Image11_WheelAndTyre2.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/HeavyVeh_Image/truck_Wheel_2.gif", UriKind.Relative));
                    Image18_WheelAndTypre3.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/HeavyVeh_Image/truck_Wheel_3.gif", UriKind.Relative));
                    Image20_WheelAndTyre4.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/HeavyVeh_Image/truck_Wheel_4.gif", UriKind.Relative));
                    Image35_WheelAndTyre5.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/HeavyVeh_Image/truck_Wheel_5.gif", UriKind.Relative));
                    Image37_WheelAndTyre6.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/HeavyVeh_Image/truck_Wheel_6.gif", UriKind.Relative));

                }
                else if (selectedDefect.DefectCategory.Trim().Equals("Vehicle Identification") || selectedDefect.DefectCategory.Trim().Equals("هوية المركبة"))
                {
                    Image3_VehicleIdentification1.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/HeavyVeh_Image/truck_Front_PlateNo.gif", UriKind.Relative));
                    Image46_VehicleInden.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/HeavyVeh_Image/truck_Back_PlateNo.gif", UriKind.Relative));
                }
                else if (selectedDefect.DefectCategory.Trim().Equals("Brake") || selectedDefect.DefectCategory.Trim().Equals("نظام الفرامل"))
                {
                    Image12_Brake1.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/HeavyVeh_Image/truck_Break_1.gif", UriKind.Relative));
                    Image16_Brake2.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/HeavyVeh_Image/truck_Break_2.gif", UriKind.Relative));
                    Image21_Brake.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/HeavyVeh_Image/truck_Break_3.gif", UriKind.Relative));
                    Image23_Brake.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/HeavyVeh_Image/truck_Break_4.gif", UriKind.Relative));
                    Image40_Brake3.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/HeavyVeh_Image/truck_Break_5.gif", UriKind.Relative));
                    Image42_Brake.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/HeavyVeh_Image/truck_Break_6.gif", UriKind.Relative));

                }
                //else if (selectedDefect.DefectCategory.Trim().Equals("Steering and Suspension") || selectedDefect.DefectCategory.Trim().Equals("نظام المقود والتعليق"))
                //{
                //    Image14_StearingSuspension.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/HeavyVeh_Image/truck_StearingSuspension.gif", UriKind.Relative));
                //}
                else if (selectedDefect.DefectCategory.Trim().Equals("Driver Identification") || selectedDefect.DefectCategory.Trim().Equals("هوية السائق"))
                {
                    Image14_StearingSuspension.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/HeavyVeh_Image/truck_StearingSuspension.gif", UriKind.Relative));
                }
                else if (selectedDefect.DefectCategory.Trim().Equals("Body Condition") || selectedDefect.DefectCategory.Trim().Equals("بدن المركبة"))
                {
                    Image25_BodyCondition.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/HeavyVeh_Image/truck_BodyCondition.gif", UriKind.Relative));
                }
                else if (selectedDefect.DefectCategory.Trim().Equals("Lighting") || selectedDefect.DefectCategory.Trim().Equals("الأنوار"))
                {
                    Image2_Lightining2.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/HeavyVeh_Image/truck_FrontLight1.gif", UriKind.Relative));
                    Image4_Lightining1.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/HeavyVeh_Image/truck_FrontLight2.gif", UriKind.Relative));
                    Image44_Lightining.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/HeavyVeh_Image/truck_BackLight1.gif", UriKind.Relative));
                    Image48_Lightining.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/HeavyVeh_Image/truck_BackLight2.gif", UriKind.Relative));
                }
                else if (selectedDefect.DefectCategory.Trim().Equals("Engine") || selectedDefect.DefectCategory.Trim().Equals("المحرك"))
                {
                    Image7_EngineCompartment.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/HeavyVeh_Image/truck_EngineCompartment.gif", UriKind.Relative));
                }
                else if (selectedDefect.DefectCategory.Trim().Equals("Modification") || selectedDefect.DefectCategory.Trim().Equals("المركبات المعدلة"))
                {
                    Image296_ModifiedVehicle.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/HeavyVeh_Image/truck_ModifiedVehicle.gif", UriKind.Relative));
                }
                else if (selectedDefect.DefectCategory.Trim().Equals("Safety Requirements") || selectedDefect.DefectCategory.Trim().Equals("معدات السلامة"))
                {
                    Image31_RoadSafety.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/HeavyVeh_Image/truck_RoadSafty.gif", UriKind.Relative));
                }
            }
            else if (grdPickup.IsVisible)
            {
                if (selectedDefect.DefectCategory.Trim().Equals("Wheels and Tires") || selectedDefect.DefectCategory.Trim().Equals("الإطارات"))
                {
                    ImgPickup15_Wheel_1.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/Pickup_Image/pickup_Wheel_1.gif", UriKind.Relative));
                    ImgPickup17_Wheel_2.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/Pickup_Image/pickup_Wheel_2.gif", UriKind.Relative));
                    ImgPickup37_Wheel_3.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/Pickup_Image/pickup_Wheel_3.gif", UriKind.Relative));
                    ImgPickup39_Wheel_4.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/Pickup_Image/pickup_Wheel_4.gif", UriKind.Relative));
                }
                else if (selectedDefect.DefectCategory.Trim().Equals("Vehicle Identification") || selectedDefect.DefectCategory.Trim().Equals("هوية المركبة"))
                {
                    ImgPickup6_Front_PlateNo.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/Pickup_Image/pickup_Front_PlateNo.gif", UriKind.Relative));
                    ImgPickup47_Back_PlateNo.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/Pickup_Image/pickup_Back_PlateNo.gif", UriKind.Relative));
                }
                else if (selectedDefect.DefectCategory.Trim().Equals("Brake") || selectedDefect.DefectCategory.Trim().Equals("نظام الفرامل"))
                {
                    ImgPickup19_Break_1.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/Pickup_Image/pickup_Break_1.gif", UriKind.Relative));
                    ImgPickup21_Break_2.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/Pickup_Image/pickup_Break_2.gif", UriKind.Relative));
                    ImgPickup41_Break_3.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/Pickup_Image/pickup_Break_3.gif", UriKind.Relative));
                    ImgPickup43_Break_4.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/Pickup_Image/pickup_Break_4.gif", UriKind.Relative));
                }
                //else if (selectedDefect.DefectCategory.Trim().Equals("Steering and Suspension") || selectedDefect.DefectCategory.Trim().Equals("نظام المقود والتعليق"))
                //{
                //    ImgPickup23_StearingSuspension.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/Pickup_Image/pickup_StearingSuspension.gif", UriKind.Relative));
                //}
                else if (selectedDefect.DefectCategory.Trim().Equals("Driver Identification") || selectedDefect.DefectCategory.Trim().Equals("هوية السائق"))
                {
                    ImgPickup23_StearingSuspension.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/Pickup_Image/pickup_StearingSuspension.gif", UriKind.Relative));
                }
                else if (selectedDefect.DefectCategory.Trim().Equals("Body Condition") || selectedDefect.DefectCategory.Trim().Equals("بدن المركبة"))
                {
                    ImgPickup27_BodyCondition.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/Pickup_Image/pickup_BodyCondition.gif", UriKind.Relative));
                }
                else if (selectedDefect.DefectCategory.Trim().Equals("Lighting") || selectedDefect.DefectCategory.Trim().Equals("الأنوار"))
                {
                    ImgPickup4_FrontLight1.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/Pickup_Image/pickup_FrontLight1.gif", UriKind.Relative));
                    ImgPickup8_FrontLight2.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/Pickup_Image/pickup_FrontLight2.gif", UriKind.Relative));
                    ImgPickup45_BackLight1.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/Pickup_Image/pickup_BackLight1.gif", UriKind.Relative));
                    ImgPickup49_BackLight2.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/Pickup_Image/pickup_BackLight2.gif", UriKind.Relative));
                }
                else if (selectedDefect.DefectCategory.Trim().Equals("Engine") || selectedDefect.DefectCategory.Trim().Equals("المحرك"))
                {
                    ImgPickup11_EngineCompartment.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/Pickup_Image/pickup_EngineCompartment.gif", UriKind.Relative));
                }
                else if (selectedDefect.DefectCategory.Trim().Equals("Modification") || selectedDefect.DefectCategory.Trim().Equals("المركبات المعدلة"))
                {
                    ImgPickup31_ModifiedVehicle.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/Pickup_Image/pickup_ModifiedVehicle.gif", UriKind.Relative));
                }
                else if (selectedDefect.DefectCategory.Trim().Equals("Safety Requirements") || selectedDefect.DefectCategory.Trim().Equals("معدات السلامة"))
                {
                    ImgPickup33_RoadSafty.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/Pickup_Image/pickup_RoadSafty.gif", UriKind.Relative));
                }
            }
            else if (grdBus.IsVisible)
            {
                if (selectedDefect.DefectCategory.Trim().Equals("Wheels and Tires") || selectedDefect.DefectCategory.Trim().Equals("الإطارات"))
                {
                    ImgBus13_Wheel_1.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/Bus_Image/bus_Wheel_1.gif", UriKind.Relative));
                    ImgBus15_Wheel_2.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/Bus_Image/bus_Wheel_2.gif", UriKind.Relative));
                    ImgBus32_Wheel_3.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/Bus_Image/bus_Wheel_3.gif", UriKind.Relative));
                    ImgBus34_Wheel_4.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/Bus_Image/bus_Wheel_4.gif", UriKind.Relative));
                }
                else if (selectedDefect.DefectCategory.Trim().Equals("Vehicle Identification") || selectedDefect.DefectCategory.Trim().Equals("هوية المركبة"))
                {
                    ImgBus4_Front_PlatNo.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/Bus_Image/bus_Front_PlateNo.gif", UriKind.Relative));
                    ImgBus41_Back_PlatNo.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/Bus_Image/bus_Back_PlateNo.gif", UriKind.Relative));
                }
                else if (selectedDefect.DefectCategory.Trim().Equals("Brake") || selectedDefect.DefectCategory.Trim().Equals("نظام الفرامل"))
                {
                    ImgBus16_Break_1.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/Bus_Image/bus_Break_1.gif", UriKind.Relative));
                    ImgBus20_Break_2.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/Bus_Image/bus_Break_2.gif", UriKind.Relative));
                    ImgBus35_Break_3.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/Bus_Image/bus_Break_3.gif", UriKind.Relative));
                    ImgBus37_Break_4.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/Bus_Image/bus_Break_4.gif", UriKind.Relative));
                }
                //else if (selectedDefect.DefectCategory.Trim().Equals("Steering and Suspension") || selectedDefect.DefectCategory.Trim().Equals("نظام المقود والتعليق"))
                //{
                //    ImgBus18_StearingSuspension.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/Bus_Image/bus_StearingSuspension.gif", UriKind.Relative));
                //}
                else if (selectedDefect.DefectCategory.Trim().Equals("Driver Identification") || selectedDefect.DefectCategory.Trim().Equals("هوية السائق"))
                {
                    ImgBus18_StearingSuspension.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/Bus_Image/bus_StearingSuspension.gif", UriKind.Relative));
                }
                else if (selectedDefect.DefectCategory.Trim().Equals("Body Condition") || selectedDefect.DefectCategory.Trim().Equals("بدن المركبة"))
                {
                    ImgBus23_BodyCondition.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/Bus_Image/bus_BodyCondition.gif", UriKind.Relative));
                }
                else if (selectedDefect.DefectCategory.Trim().Equals("Lighting") || selectedDefect.DefectCategory.Trim().Equals("الأنوار"))
                {
                    ImgBus2_FrontLight.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/Bus_Image/bus_FrontLight1.gif", UriKind.Relative));
                    ImgBus6_FrontLight.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/Bus_Image/bus_FrontLight2.gif", UriKind.Relative));
                    ImgBus39_BackLight1.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/Bus_Image/bus_BackLight1.gif", UriKind.Relative));
                    ImgBus43_BackLight2.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/Bus_Image/bus_BackLight2.gif", UriKind.Relative));
                }
                else if (selectedDefect.DefectCategory.Trim().Equals("Engine") || selectedDefect.DefectCategory.Trim().Equals("المحرك"))
                {
                    ImgBus10_EngineCompartment.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/Bus_Image/bus_EngineCompartment.gif", UriKind.Relative));
                }
                else if (selectedDefect.DefectCategory.Trim().Equals("Modification") || selectedDefect.DefectCategory.Trim().Equals("المركبات المعدلة"))
                {
                    ImgBus27_ModifiedVehicle.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/Bus_Image/bus_ModifiedVehicle.gif", UriKind.Relative));
                }
                else if (selectedDefect.DefectCategory.Trim().Equals("Safety Requirements") || selectedDefect.DefectCategory.Trim().Equals("معدات السلامة"))
                {
                    ImgBus29_RoadSafty.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/Bus_Image/bus_RoadSafty.gif", UriKind.Relative));
                }
            }
            else if (grdVan.IsVisible)
            {
                if (selectedDefect.DefectCategory.Trim().Equals("Wheels and Tires") || selectedDefect.DefectCategory.Trim().Equals("الإطارات"))
                {
                    ImgVan14_Wheel_1.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/Van_Image/Van_Wheel_1.gif", UriKind.Relative));
                    ImgVan16_Wheel_2.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/Van_Image/Van_Wheel_2.gif", UriKind.Relative));
                    ImgVan37_Wheel_3.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/Van_Image/Van_Wheel_3.gif", UriKind.Relative));
                    ImgVan39_Wheel_4.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/Van_Image/Van_Wheel_4.gif", UriKind.Relative));
                }
                else if (selectedDefect.DefectCategory.Trim().Equals("Vehicle Identification") || selectedDefect.DefectCategory.Trim().Equals("هوية المركبة"))
                {
                    ImgVan4_Front_PlateNo.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/Van_Image/Van_Front_PlateNo.gif", UriKind.Relative));
                    ImgVan47_Back_PlateNo.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/Van_Image/Van_Back_PlateNo.gif", UriKind.Relative));
                }
                else if (selectedDefect.DefectCategory.Trim().Equals("Brake") || selectedDefect.DefectCategory.Trim().Equals("نظام الفرامل"))
                {
                    ImgVan19_Break_1.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/Van_Image/Van_Break_1.gif", UriKind.Relative));
                    ImgVan21_Break_2.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/Van_Image/Van_Break_2.gif", UriKind.Relative));
                    ImgVan40_Break_3.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/Van_Image/Van_Break_3.gif", UriKind.Relative));
                    ImgVan42_Break_4.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/Van_Image/Van_Break_4.gif", UriKind.Relative));
                }
                //else if (selectedDefect.DefectCategory.Trim().Equals("Steering and Suspension") || selectedDefect.DefectCategory.Trim().Equals("نظام المقود والتعليق"))
                //{
                //    ImgVan24_StearingSuspension.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/Van_Image/Van_StearingSuspension.gif", UriKind.Relative));
                //}
                else if (selectedDefect.DefectCategory.Trim().Equals("Driver Identification") || selectedDefect.DefectCategory.Trim().Equals("هوية السائق"))
                {
                    ImgVan24_StearingSuspension.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/Van_Image/Van_StearingSuspension.gif", UriKind.Relative));
                }
                else if (selectedDefect.DefectCategory.Trim().Equals("Body Condition") || selectedDefect.DefectCategory.Trim().Equals("بدن المركبة"))
                {
                    ImgVan28_BodyCondition.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/Van_Image/Van_BodyCondition.gif", UriKind.Relative));
                }
                else if (selectedDefect.DefectCategory.Trim().Equals("Lighting") || selectedDefect.DefectCategory.Trim().Equals("الأنوار"))
                {
                    ImgVan2_FrontLight1.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/Van_Image/Van_FrontLight1.gif", UriKind.Relative));
                    ImgVan6_FrontLight2.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/Van_Image/Van_FrontLight2.gif", UriKind.Relative));
                    ImgVan45_BackLight1.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/Van_Image/Van_BackLight1.gif", UriKind.Relative));
                    ImgVan49_BackLight2.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/Van_Image/Van_BackLight2.gif", UriKind.Relative));
                }
                else if (selectedDefect.DefectCategory.Trim().Equals("Engine") || selectedDefect.DefectCategory.Trim().Equals("المحرك"))
                {
                    ImgVan10_EngineCompartment.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/Van_Image/Van_EngineCompartment.gif", UriKind.Relative));
                }
                else if (selectedDefect.DefectCategory.Trim().Equals("Modification") || selectedDefect.DefectCategory.Trim().Equals("المركبات المعدلة"))
                {
                    ImgVan32_ModifiedVehicle.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/Van_Image/Van_ModifiedVehicle.gif", UriKind.Relative));
                }
                else if (selectedDefect.DefectCategory.Trim().Equals("Safety Requirements") || selectedDefect.DefectCategory.Trim().Equals("معدات السلامة"))
                {
                    ImgVan34_RoadSafty.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/Van_Image/Van_RoadSafty.gif", UriKind.Relative));
                }
            }
            else if (grdHeavyMach.IsVisible)
            {
                if (selectedDefect.DefectCategory.Trim().Equals("Wheels and Tires") || selectedDefect.DefectCategory.Trim().Equals("الإطارات"))
                {
                    ImgHeavyMach14.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/HeavyMach_Image/HeavyMachinary_14.gif", UriKind.Relative));
                    ImgHeavyMach16.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/HeavyMach_Image/HeavyMachinary_16.gif", UriKind.Relative));
                    ImgHeavyMach33.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/HeavyMach_Image/HeavyMachinary_33.gif", UriKind.Relative));
                    ImgHeavyMach35.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/HeavyMach_Image/HeavyMachinary_35.gif", UriKind.Relative));
                }
                else if (selectedDefect.DefectCategory.Trim().Equals("Vehicle Identification") || selectedDefect.DefectCategory.Trim().Equals("هوية المركبة"))
                {
                    ImgHeavyMach3.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/HeavyMach_Image/HeavyMachinary_03.gif", UriKind.Relative));
                    ImgHeavyMach46.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/HeavyMach_Image/HeavyMachinary_46.gif", UriKind.Relative));
                }
                else if (selectedDefect.DefectCategory.Trim().Equals("Brake") || selectedDefect.DefectCategory.Trim().Equals("نظام الفرامل"))
                {
                    ImgHeavyMach17.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/HeavyMach_Image/HeavyMachinary_17.gif", UriKind.Relative));
                    ImgHeavyMach21.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/HeavyMach_Image/HeavyMachinary_21.gif", UriKind.Relative));
                    ImgHeavyMach36.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/HeavyMach_Image/HeavyMachinary_36.gif", UriKind.Relative));
                    ImgHeavyMach38.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/HeavyMach_Image/HeavyMachinary_38.gif", UriKind.Relative));
                }
                //else if (selectedDefect.DefectCategory.Trim().Equals("Steering and Suspension") || selectedDefect.DefectCategory.Trim().Equals("نظام المقود والتعليق"))
                //{
                //    ImgHeavyMach19.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/HeavyMach_Image/HeavyMachinary_19.gif", UriKind.Relative));
                //}
                else if (selectedDefect.DefectCategory.Trim().Equals("Driver Identification") || selectedDefect.DefectCategory.Trim().Equals("هوية السائق"))
                {
                    ImgHeavyMach19.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/HeavyMach_Image/HeavyMachinary_19.gif", UriKind.Relative));
                }
                else if (selectedDefect.DefectCategory.Trim().Equals("Body Condition") || selectedDefect.DefectCategory.Trim().Equals("بدن المركبة"))
                {
                    ImgHeavyMach24.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/HeavyMach_Image/HeavyMachinary_24.gif", UriKind.Relative));
                }
                else if (selectedDefect.DefectCategory.Trim().Equals("Lighting") || selectedDefect.DefectCategory.Trim().Equals("الأنوار"))
                {
                    ImgHeavyMach6.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/HeavyMach_Image/HeavyMachinary_06.gif", UriKind.Relative));
                    ImgHeavyMach8.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/HeavyMach_Image/HeavyMachinary_08.gif", UriKind.Relative));
                    ImgHeavyMach41.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/HeavyMach_Image/HeavyMachinary_41.gif", UriKind.Relative));
                    ImgHeavyMach43.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/HeavyMach_Image/HeavyMachinary_43.gif", UriKind.Relative));
                }
                else if (selectedDefect.DefectCategory.Trim().Equals("Engine") || selectedDefect.DefectCategory.Trim().Equals("المحرك"))
                {
                    ImgHeavyMach11.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/HeavyMach_Image/HeavyMachinary_11.gif", UriKind.Relative));
                }
                else if (selectedDefect.DefectCategory.Trim().Equals("Modification") || selectedDefect.DefectCategory.Trim().Equals("المركبات المعدلة"))
                {
                    ImgHeavyMach28.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/HeavyMach_Image/HeavyMachinary_28.gif", UriKind.Relative));
                }
                else if (selectedDefect.DefectCategory.Trim().Equals("Safety Requirements") || selectedDefect.DefectCategory.Trim().Equals("معدات السلامة"))
                {
                    ImgHeavyMach30.Source = new BitmapImage(new Uri("/Images/Vehicle_Image/HeavyMach_Image/HeavyMachinary_30.gif", UriKind.Relative));
                }
            }
        }

        #endregion

        //--------------------code written by kashif abbasi for 10-Nov-2015 -----
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
                    data.Violation.Defects selectedDefect = (data.Violation.Defects)grdAddedDefects.SelectedItem;
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

        private void grdAddedDefects_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //check Is this  defect have an image in following diractor..if yes than show its thumbnail below grid
            string strPath = Properties.Settings.Default.violationImagesPath; //@"C:\RTA_VSD_IMAGES";
            _strAllImagesOfDefect = null;
            _selectedDefectInfo = ((data.Violation.Defects)grdAddedDefects.SelectedItem);
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
                        string[] strAllImages = Directory.GetFiles(strPath, ((data.Violation.Defects)grdAddedDefects.SelectedItem).DefectID + "_*.jpg");

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

        private void imgDefectImageThumbnil_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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

        private void deleteAllImagesOfDefect()
        {
            try
            {
                imgDefectImageThumbnil.Source = null;
                lblDefImgCount.Content = "";
                bool isDeleted = CommonUtils.deleteFiles(_strAllImagesOfDefect);
                if (!isDeleted)
                {
                    WPFMessageBox.Show("Error", "getting Problem while deletion of images of selected defect ");
                }
            }
            catch (Exception ex)
            {

                WPFMessageBox.Show("Error", "getting Problem while deletion of images of selected defect " + ex.Message);
                App.VSDLog.Info("\n ucDefectAndViolationDetails.deleteAllImagesOfDefect(): getting Problem while deletion of images of selected defect " + ex.Message);
            }

        }

        private void showVehicleGridByCategory()
        {
            //Motor Cycle                       دراجة نارية
            //Handicap Carriage                	مركبة معاقين
            //Light Vehicle                    	مركبة خفيفة
            //Heavy Vehicle                    	مركبة ثقيلة
            //Light Bus                       	باص خفيف
            //Heavy Bus                       	باص ثقيل
            //Light Mechnical Equipement       	جهاز ميكانيكي خفيف
            //Heavy Mechinical Equipement      	جهاز ميكانيكي ثقيل
            //Entertainment Motorcycle        	دراجة نارية ترفيهية

            if (AppProperties.vehicle.VehicleCategory.Trim().ToUpper().Equals("HEAVY VEHICLE") || AppProperties.vehicle.VehicleCategory.Trim().Equals("مركبة ثقيلة"))
            {
                grdVehicleImageSlice.Visibility = Visibility.Visible;
                grdBus.Visibility = Visibility.Collapsed;
                grdPickup.Visibility = Visibility.Collapsed;
                grdVan.Visibility = Visibility.Collapsed;
                grdHeavyMach.Visibility = Visibility.Collapsed;
            }
            else if (AppProperties.vehicle.VehicleCategory.Trim().ToUpper().Equals("LIGHT VEHICLE") || AppProperties.vehicle.VehicleCategory.Trim().Equals("مركبة خفيفة"))
            {//pickup is light vehicle
                grdVehicleImageSlice.Visibility = Visibility.Collapsed;
                grdBus.Visibility = Visibility.Collapsed;
                grdPickup.Visibility = Visibility.Visible;
                grdVan.Visibility = Visibility.Collapsed;
                grdHeavyMach.Visibility = Visibility.Collapsed;
            }
            else if (AppProperties.vehicle.VehicleCategory.Trim().ToUpper().Equals("HEAVY BUS") || AppProperties.vehicle.VehicleCategory.Trim().ToUpper().Equals("باص ثقيل"))
            {//Bus is heavy bus
                grdVehicleImageSlice.Visibility = Visibility.Collapsed;
                grdBus.Visibility = Visibility.Visible;
                grdPickup.Visibility = Visibility.Collapsed;
                grdVan.Visibility = Visibility.Collapsed;
                grdHeavyMach.Visibility = Visibility.Collapsed;
            }
            else if (AppProperties.vehicle.VehicleCategory.Trim().ToUpper().Equals("LIGHT BUS") || AppProperties.vehicle.VehicleCategory.Trim().Equals("باص خفيف"))
            {//van is light buss
                grdVehicleImageSlice.Visibility = Visibility.Collapsed;
                grdBus.Visibility = Visibility.Collapsed;
                grdPickup.Visibility = Visibility.Collapsed;
                grdVan.Visibility = Visibility.Visible;
                grdHeavyMach.Visibility = Visibility.Collapsed;
            }
            else if (AppProperties.vehicle.VehicleCategory.Trim().ToUpper().Equals("HEAVY MECHINICAL EQUIPEMENT") || AppProperties.vehicle.VehicleCategory.Trim().Equals("جهاز ميكانيكي ثقيل"))
            {
                grdVehicleImageSlice.Visibility = Visibility.Collapsed;
                grdBus.Visibility = Visibility.Collapsed;
                grdPickup.Visibility = Visibility.Collapsed;
                grdVan.Visibility = Visibility.Collapsed;
                grdHeavyMach.Visibility = Visibility.Visible;
            }
            else if (AppProperties.vehicle.VehicleCategory.Trim().ToUpper().Equals("LIGHT MECHINICAL EQUIPEMENT") || AppProperties.vehicle.VehicleCategory.Trim().Equals("جهاز ميكانيكي خفيف"))
            {
                grdVehicleImageSlice.Visibility = Visibility.Collapsed;
                grdBus.Visibility = Visibility.Collapsed;
                grdPickup.Visibility = Visibility.Collapsed;
                grdVan.Visibility = Visibility.Collapsed;
                grdHeavyMach.Visibility = Visibility.Visible;
            }
            else
            {
                grdVehicleImageSlice.Visibility = Visibility.Visible;
                grdBus.Visibility = Visibility.Collapsed;
                grdPickup.Visibility = Visibility.Collapsed;
                grdVan.Visibility = Visibility.Collapsed;
                grdHeavyMach.Visibility = Visibility.Collapsed;
            }
        }

        private string getImgsPath()
        {
            //Motor Cycle                       دراجة نارية
            //Handicap Carriage                	مركبة معاقين
            //Light Vehicle                    	مركبة خفيفة
            //Heavy Vehicle                    	مركبة ثقيلة
            //Light Bus                       	باص خفيف
            //Heavy Bus                       	باص ثقيل
            //Light Mechnical Equipement       	جهاز ميكانيكي خفيف
            //Heavy Mechinical Equipement      	جهاز ميكانيكي ثقيل
            //Entertainment Motorcycle        	دراجة نارية ترفيهية

            string strVehCat = AppProperties.vehicle.VehicleCategory.Trim();
            if (strVehCat.ToUpper().Equals("Heavy Vehicle") || strVehCat.Equals("مركبة ثقيلة"))
            {
                return "/Images/Vehicle_Image/HeavyVeh_Image/";
            }
            else if (strVehCat.ToUpper().Equals("LIGHT VEHICLE") || strVehCat.Equals("مركبة ثقيلة"))
            {
                return "/Images/Vehicle_Image/Pickup_Image/";
            }
            else if (strVehCat.ToUpper().Equals("HEAVY BUS") || strVehCat.ToUpper().Equals("HEAVY BUS") || strVehCat.Equals("باص ثقيل"))
            {
                return "/Images/Vehicle_Image/Bus_Image/";
            }
            else if (strVehCat.ToUpper().Equals("LIGHT BUS") || strVehCat.ToUpper().Equals("LIGHT BUS") || strVehCat.Equals("باص خفيف"))
            {
                return "/Images/Vehicle_Image/Van_Image/";
            }
            else if (strVehCat.ToUpper().Equals("HEAVY MECHINICAL EQUIPEMENT") || strVehCat.Equals("جهاز ميكانيكي ثقيل"))
            {
                return "/Images/Vehicle_Image/HeavyMach_Image/";
            }
            else if (strVehCat.ToUpper().Equals("LIGHT MECHINICAL EQUIPEMENT") || strVehCat.Equals("جهاز ميكانيكي خفيف"))
            {
                return "/Images/Vehicle_Image/HeavyMach_Image/";
            }
            else
            {
                return "/Images/Vehicle_Image/HeavyVeh_Image/";
            }
        }

        private void SelectDefectCategoryInCmbBox(string strSelectItem)
        {
            try
            {
                string strItm = strSelectItem.Split(',')[0];
                if (AppProperties.Selected_Resource == "English")
                {
                    strItm = strSelectItem.Split(',')[0];
                    if (cmboxDefectCategory.Items.Contains(strItm))
                    {
                        cmboxDefectCategory.SelectedValue = strItm;
                    }
                }
                else
                {
                    strItm = strSelectItem.Split(',')[1];
                    if (cmboxDefectCategory.Items.Contains(strItm))
                    {
                        cmboxDefectCategory.SelectedValue = strItm;
                    }
                }
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        #region Ligh Vehicle Events (Heavy Vehicle,Light Vehicle,Bus, light Bus)

        private void ImgPickup4_FrontLight1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //call image blink code
            SelectDefectCategoryInCmbBox("Lighting,الأنوار");
            startImageBlink(((System.Windows.Controls.Image)(sender)));

        }

        private void ImgPickup6_Front_PlateNo_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //call image blink code
            SelectDefectCategoryInCmbBox("Vehicle Identification,هوية المركبة");
            startImageBlink(((System.Windows.Controls.Image)(sender)));
        }

        private void ImgPickup11_EngineCompartment_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //call image blink code
            startImageBlink(((System.Windows.Controls.Image)(sender)));
            SelectDefectCategoryInCmbBox("Engine,المحرك");
        }

        private void ImgPickup15_Wheel_1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SelectDefectCategoryInCmbBox("Wheels and Tires,الإطارات");
            startImageBlink(((System.Windows.Controls.Image)(sender)));
        }

        private void ImgPickup19_Break_1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SelectDefectCategoryInCmbBox("Brake,نظام الفرامل");
            startImageBlink(((System.Windows.Controls.Image)(sender)));

        }

        private void ImgPickup23_StearingSuspension_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            startImageBlink(((System.Windows.Controls.Image)(sender)));
            //SelectDefectCategoryInCmbBox("Steering and Suspension,نظام المقود والتعليق");
            SelectDefectCategoryInCmbBox("Driver Identification,هوية السائق");
        }

        private void ImgPickup27_BodyCondition_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            startImageBlink(((System.Windows.Controls.Image)(sender)));
            SelectDefectCategoryInCmbBox("Body Condition,بدن المركبة");
        }

        private void ImgPickup31_ModifiedVehicle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            startImageBlink(((System.Windows.Controls.Image)(sender)));
            SelectDefectCategoryInCmbBox("Modification,المركبات المعدلة");
        }

        private void ImgPickup33_RoadSafty_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            startImageBlink(((System.Windows.Controls.Image)(sender)));
            SelectDefectCategoryInCmbBox("Safety Requirements,معدات السلامة");
        }
        #endregion

        private void btnSearchLicNo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (AppProperties.Selected_Resource == "English")
                {
                    imagebtnDriverSearch.Source = new BitmapImage(new Uri(@"/Images/Buttons/Large/Search.png", UriKind.Relative));

                    SearchDriverEnHandler();
                }
                else
                {
                    imagebtnDriverSearch.Source = new BitmapImage(new Uri(@"/Images/Buttons/Large/Search Arabic Up Large.png", UriKind.Relative));
                    SearchDriverArHandler();
                }
            }
            catch (Exception ex)
            {
                CommonUtils.WriteLog(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
            /*
            try
            {
                if (AppProperties.Selected_Resource == "English")
                {
                    SearchDriverEnHandler();
                    if (AppProperties.vehicle.VehiclDriver != null)
                    {
                        if (!txtDriverLiscenseNumber.Text.Equals(AppProperties.vehicle.VehiclDriver.LicNumber))
                        {

                            AddTelemeticDefects();
                            //    WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("ErrorTelematicDefect"));
                            //   this.txtDriverName.Text = "";
                            // this.txtDriverNationality.Text = "";
                            // AppProperties.vehicle.DriverCountry =
                            // this.txttickCross.Text = "X";
                            // this.txttickCross.Foreground = Brushes.Red;
                            expender_AddDefects.IsExpanded = true;
                            expender_DriverDetials.IsExpanded = false;
                            return;

                        }
                    }
                    else
                    {
                        AddTelemeticDefects();
                        // WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("ErrorTelematicDefect"));
                        //   this.txtDriverName.Text = "";
                        // this.txtDriverNationality.Text = "";
                        // AppProperties.vehicle.DriverCountry =
                        // this.txttickCross.Text = "X";
                        // this.txttickCross.Foreground = Brushes.Red;
                        expender_AddDefects.IsExpanded = true;
                        expender_DriverDetials.IsExpanded = false;
                        return;
                    }

                }
                else
                {

                    SearchDriverArHandler();
                    if (AppProperties.vehicle.VehiclDriver != null)
                    {
                        if (!txtDriverLiscenseNumber.Text.Equals(AppProperties.vehicle.VehiclDriver.LicNumber))
                        {

                            AddTelemeticDefects();

                            //   this.txtDriverName.Text = "";
                            // this.txtDriverNationality.Text = "";
                            // AppProperties.vehicle.DriverCountry =
                            // this.txttickCross.Text = "X";
                            // this.txttickCross.Foreground = Brushes.Red;
                            expender_AddDefects.IsExpanded = true;
                            expender_DriverDetials.IsExpanded = false;
                            return;

                        }
                    }
                    else
                    {
                        AddTelemeticDefects();
                        // WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("ErrorTelematicDefect"));
                        //   this.txtDriverName.Text = "";
                        // this.txtDriverNationality.Text = "";
                        // AppProperties.vehicle.DriverCountry =
                        // this.txttickCross.Text = "X";
                        // this.txttickCross.Foreground = Brushes.Red;
                        expender_AddDefects.IsExpanded = true;
                        expender_DriverDetials.IsExpanded = false;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            } */
        }


        public void AddEnDriverTelemticDefect()
        {
            try
            {
                string telematics_Defect_Name = "Driver Using Incorrect ID";
                string telematics_Defect_Name_A = string.Empty;
                string telematics_Defect_Category = "Telematics Events";
                string telematics_Defect_Category_A = string.Empty;
                string telematics_defect_SubCategory = "Driver Violations";
                string telematics_defect_SubCategory_A = string.Empty;
                string telematics_defect_Code = Properties.Settings.Default.telematicsDefectCode;

                string telematics_defect_Type = string.Empty;
                string telematics_defect_Type_A = string.Empty;


                //  GetDefectNameByID
                //  GetDefectNameArByID
                // string[][] IDBDataLoad.GetDefectMainCategoryByID(string defectID)
                // GetDefectMainCategory(where type like defect

                DataTable dtTelDefectCat = new DataTable();
                dtTelDefectCat = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectCategoryFromDefectCode(telematics_defect_Code);
                if ((dtTelDefectCat == null) || (dtTelDefectCat.Rows.Count <= 0))
                    return;

                telematics_Defect_Category = Convert.ToString(dtTelDefectCat.Rows[0]["Category_Name"]);
                telematics_Defect_Category_A = Convert.ToString(dtTelDefectCat.Rows[0]["Category_Name_A"]);

                DataTable dtTelDefectSubCatName = new DataTable();
                dtTelDefectSubCatName = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectSubAndNameFDefectCode(telematics_defect_Code);
                if ((dtTelDefectSubCatName == null) || (dtTelDefectSubCatName.Rows.Count <= 0))
                    return;

                telematics_Defect_Name = Convert.ToString(dtTelDefectSubCatName.Rows[0]["Defect_Name"]);
                telematics_Defect_Name_A = Convert.ToString(dtTelDefectSubCatName.Rows[0]["Defect_Name_A"]);
                telematics_defect_SubCategory = Convert.ToString(dtTelDefectSubCatName.Rows[0]["Category_Name"]);
                telematics_defect_SubCategory_A = Convert.ToString(dtTelDefectSubCatName.Rows[0]["Category_Name_A"]);
                telematics_defect_ID = Convert.ToString(dtTelDefectSubCatName.Rows[0]["Defect_ID"]);
                telematics_defect_Type = Convert.ToString(dtTelDefectSubCatName.Rows[0]["Defect_Type"]);
                //  telematics_defect_ID = Convert.ToString(dtTelDefectSubCatName.Rows[0]["Defect_ID"]);



                for (int i = 0; i < AppProperties.selectedDefectsEn.Count; i++)
                {
                    string[] data = AppProperties.selectedDefectsEn[i];
                    if (data[4].Trim().Equals(telematics_Defect_Name) && data[2].Trim().Equals(telematics_defect_SubCategory.Trim()) && data[3].Trim().Equals(telematics_Defect_Category.Trim()))
                    {
                        WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), lblalreadyadded.Content.ToString());
                        return;
                    }
                }
                string[] details2 = ((IViolation)ViolationManager.GetInstance()).GetDefectSeverity(telematics_Defect_Name, telematics_defect_SubCategory, telematics_Defect_Category, "");

                //  if (null == details)
                if (false)
                {

                    WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("lblDefectInfoMissing"));
                    return;

                }
                else
                {
                    string[] details = new string[12];
                    details[0] = telematics_defect_ID;
                    details[1] = telematics_defect_Type;
                    details[4] = telematics_Defect_Name;

                    details[2] = telematics_defect_SubCategory;
                    details[3] = telematics_Defect_Category;
                    details[5] = telematics_defect_Code;
                    AppProperties.selectedDefectsEn.Add(details);


                    data.Violation.Defects defct = new data.Violation.Defects();

                    defct.DefectID = Convert.ToInt32(telematics_defect_ID);

                    // defct.DefectType = new CommonUtils().GetStringValue(details[1].Trim().ToString().Replace(" ", string.Empty));
                    defct.DefectType = telematics_defect_Type;

                    defct.DefectName = telematics_Defect_Name;
                    defct.DefectCode = Properties.Settings.Default.telematicsDefectCode; ;
                    //  defct.DefectSeverity = details[5].ToString().Trim();
                    //  defct.DefectSeverityAr = details[6].ToString().Trim();
                    defct.DefectSeverity = "";
                    defct.DefectSeverityAr = "";

                    //  defct.EnforceTesting = details[10].ToString().Trim();
                    defct.EnforceTesting = "";
                    // defct.EnforceFine = details[11].ToString().Trim();
                    defct.EnforceFine = "";
                    // if (defct.EnforceTesting != null || defct.EnforceTesting != "")
                    // {
                    //   if (defct.EnforceTesting.ToString().Equals("F"))
                    // {

                    //   defct.DefectSeverity += "(*)";
                    // defct.DefectSeverityAr += "(*)";
                    // }
                    // }

                    defct.DefectCategory = telematics_Defect_Category;

                    defct.DefectSubCat = telematics_defect_SubCategory;



                    string vehCat = AppProperties.vehicle.VehicleCategory;
                    if ((vehCat == "") || (vehCat == null))
                        vehCat = "Heavy Vehicle";

                    // defct.DefectValue = details[7].ToString().Trim();
                    //defct.ImpoundingDays = GetImpoundingDays(defct.DefectID.ToString(), vehCat).ToString();
                    defct.ImpoundingDays = "0";
                    //UpdateGroundingDays(defct.ImpoundingDays);

                    this.lblgrdSeverityEng.Visibility = System.Windows.Visibility.Collapsed;
                    this.lblgrdSeverityAr.Visibility = System.Windows.Visibility.Visible;
                    AddedDefects.Add(defct);
                    grdAddedDefects.ItemsSource = null;
                    grdAddedDefects.ItemsSource = AddedDefects;
                    txtActualValue.Text = "";
                    System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.DigitSubstitution = System.Globalization.DigitShapes.None;
                    this.UpdateLayout();
                    // AddImpoundingDays(details[0].ToString().Trim());
                    WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("ErrorTelematicDefect"));
                }
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }
        public void AddArDriverTelemeticDefect()
        {
            try
            {
                string telematics_Defect_Name = string.Empty;
                string telematics_Defect_Name_A = string.Empty;
                string telematics_Defect_Category = string.Empty;
                string telematics_Defect_Category_A = string.Empty;
                string telematics_defect_SubCategory = string.Empty;
                string telematics_defect_SubCategory_A = string.Empty;
                string telematics_defect_Code = Properties.Settings.Default.telematicsDefectCode;

                string telematics_defect_Type = string.Empty;
                string telematics_defect_Type_A = string.Empty;


                //  GetDefectNameByID
                //  GetDefectNameArByID
                // string[][] IDBDataLoad.GetDefectMainCategoryByID(string defectID)
                // GetDefectMainCategory(where type like defect

                DataTable dtTelDefectCat = new DataTable();
                dtTelDefectCat = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectCategoryFromDefectCode(telematics_defect_Code);
                if ((dtTelDefectCat == null) || (dtTelDefectCat.Rows.Count <= 0))
                    return;

                telematics_Defect_Category = Convert.ToString(dtTelDefectCat.Rows[0]["Category_Name"]);
                telematics_Defect_Category_A = Convert.ToString(dtTelDefectCat.Rows[0]["Category_Name_A"]);

                DataTable dtTelDefectSubCatName = new DataTable();
                dtTelDefectSubCatName = ((IDBDataLoad)DBDataLoadManager.GetInstance()).GetDefectSubAndNameFDefectCode(telematics_defect_Code);
                if ((dtTelDefectSubCatName == null) || (dtTelDefectSubCatName.Rows.Count <= 0))
                    return;

                telematics_Defect_Name = Convert.ToString(dtTelDefectSubCatName.Rows[0]["Defect_Name"]);
                telematics_Defect_Name_A = Convert.ToString(dtTelDefectSubCatName.Rows[0]["Defect_Name_A"]);
                telematics_defect_SubCategory = Convert.ToString(dtTelDefectSubCatName.Rows[0]["Category_Name"]);
                telematics_defect_SubCategory_A = Convert.ToString(dtTelDefectSubCatName.Rows[0]["Category_Name_A"]);
                telematics_defect_ID = Convert.ToString(dtTelDefectSubCatName.Rows[0]["Defect_ID"]);
                telematics_defect_Type = Convert.ToString(dtTelDefectSubCatName.Rows[0]["Defect_Type"]);
                telematics_defect_Type_A = Convert.ToString(dtTelDefectSubCatName.Rows[0]["Defect_Type_A"]);



                for (int i = 0; i < AppProperties.selectedDefectsEn.Count; i++)
                {
                    string[] data = AppProperties.selectedDefectsEn[i];
                    if (data[4].Trim().Equals(telematics_Defect_Name) && data[2].Trim().Equals(telematics_defect_SubCategory.Trim()) && data[3].Trim().Equals(telematics_Defect_Category.Trim()))
                    {
                        WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), lblalreadyadded.Content.ToString());
                        return;
                    }
                }
                //  string[] details = ((IViolation)ViolationManager.GetInstance()).GetDefectSeverity(telematics_Defect_Name, telematics_defect_SubCategory,telematics_Defect_Category, "");

                //  if (null == details)
                if (false)
                {

                    WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("lblDefectInfoMissing"));
                    return;

                }
                else
                {
                    string[] details = new string[12];
                    details[0] = telematics_defect_ID;
                    details[1] = telematics_defect_Type;
                    details[4] = telematics_Defect_Name;

                    details[2] = telematics_defect_SubCategory;
                    details[3] = telematics_Defect_Category;
                    details[5] = telematics_defect_Code;
                    AppProperties.selectedDefectsEn.Add(details);


                    data.Violation.Defects defct = new data.Violation.Defects();

                    defct.DefectID = Convert.ToInt32(telematics_defect_ID);

                    // defct.DefectType = new CommonUtils().GetStringValue(details[1].Trim().ToString().Replace(" ", string.Empty));
                    defct.DefectType = telematics_defect_Type_A;
                    defct.DefectNameAr = telematics_Defect_Name_A;

                    defct.DefectName = telematics_Defect_Name;
                    defct.DefectCode = Properties.Settings.Default.telematicsDefectCode; ;
                    //  defct.DefectSeverity = details[5].ToString().Trim();
                    //  defct.DefectSeverityAr = details[6].ToString().Trim();
                    defct.DefectSeverity = "";
                    defct.DefectSeverityAr = "";

                    //  defct.EnforceTesting = details[10].ToString().Trim();
                    defct.EnforceTesting = "";
                    // defct.EnforceFine = details[11].ToString().Trim();
                    defct.EnforceFine = "";
                    // if (defct.EnforceTesting != null || defct.EnforceTesting != "")
                    // {
                    //   if (defct.EnforceTesting.ToString().Equals("F"))
                    // {

                    //   defct.DefectSeverity += "(*)";
                    // defct.DefectSeverityAr += "(*)";
                    // }
                    // }

                    defct.DefectCategory = telematics_Defect_Category_A;



                    defct.DefectSubCat = telematics_defect_SubCategory_A;



                    string vehCat = AppProperties.vehicle.VehicleCategory;
                    if ((vehCat == "") || (vehCat == null))
                        vehCat = "Heavy Vehicle";

                    // defct.DefectValue = details[7].ToString().Trim();
                    //defct.ImpoundingDays = GetImpoundingDays(defct.DefectID.ToString(), vehCat).ToString();
                    defct.ImpoundingDays = "0";
                    //UpdateGroundingDays(defct.ImpoundingDays);

                    this.lblgrdSeverityEng.Visibility = System.Windows.Visibility.Collapsed;
                    this.lblgrdSeverityAr.Visibility = System.Windows.Visibility.Visible;
                    AddedDefects.Add(defct);
                    grdAddedDefects.ItemsSource = null;
                    grdAddedDefects.ItemsSource = AddedDefects;
                    txtActualValue.Text = "";
                    System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.DigitSubstitution = System.Globalization.DigitShapes.None;
                    this.UpdateLayout();
                    // AddImpoundingDays(details[0].ToString().Trim());
                    WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("ErrorTelematicDefect"));
                }
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }


        /// <summary>
        /// Telematics
        /// </summary>
        public void AddTelemeticDefects()
        {
            try
            {



                if (AppProperties.Selected_Resource == "English")
                {

                    AddEnDriverTelemticDefect();
                }
                else
                {
                    AddArDriverTelemeticDefect();
                }
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }


        private void expender_AddDefects_Expanded(object sender, RoutedEventArgs e)
        {
            if ((expender_DriverDetials != null) && expender_AddDefects != null)
            {
                if (expender_AddDefects.IsExpanded)
                    expender_DriverDetials.IsExpanded = false;

            }
        }

        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            if ((expender_DriverDetials != null) && expender_AddDefects != null)
            {
                if (expender_DriverDetials.IsExpanded)
                    expender_AddDefects.IsExpanded = false;


            }
        }

        private void expender_DriverDetials_Collapsed(object sender, RoutedEventArgs e)
        {
            if ((expender_DriverDetials != null) && expender_AddDefects != null)
            {
                expender_AddDefects.IsExpanded = true;
                return;
            }
        }

        private void expender_AddDefects_Collapsed(object sender, RoutedEventArgs e)
        {
            expender_DriverDetials.IsExpanded = true;
            return;
        }

        private void cmboxEmirates_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.lblDRR.Visibility = System.Windows.Visibility.Collapsed;
            this.grdcanvas.Visibility = System.Windows.Visibility.Collapsed;
            this.txtDriverLiscenseNumber.Text = "";
            this.txttickCross.Text = "";
            this.txtDriverName.Text = "";
            this.txtDriverNationality.Text = "";
            this.txtDriverLiscenseNumber.Background = Brushes.White;
        }

        private void txtDriverLiscenseNumber_LostFocus(object sender, RoutedEventArgs e)
        {
            /*
                       try
                       {
                           if ((txtDriverLiscenseNumber.Text != null) && (txtDriverLiscenseNumber.Text != null) && (txtDriverLiscenseNumber.Text != ""))
                           {
                               if (AppProperties.vehicle.VehiclDriver != null)
                               {
                                   if (!txtDriverLiscenseNumber.Text.Equals(AppProperties.vehicle.VehiclDriver.LicNumber))
                                   {
                                       if (AppProperties.Selected_Resource == "English")
                                       {
                                           SearchDriverEnHandler();
                                       }
                                       else
                                       {
                                           SearchDriverArHandler();
                                       }
                                       AddTelemeticDefects();
                                       WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("ErrorTelematicDefect"));
                                       // this.txtDriverName.Text = "";
                                       // this.txtDriverNationality.Text = "";
                                       //  AppProperties.vehicle.DriverCountry =
                                       // this.txttickCross.Text = "X";
                                       // this.txttickCross.Foreground = Brushes.Red;
                                       // expender_AddDefects.IsExpanded = true;
                                       // expender_DriverDetials.IsExpanded = false;
                                       return;

                                   }
                                   else
                                   {
                                       if (AppProperties.Selected_Resource == "English")
                                       {
                                           SearchDriverEnHandler();

                                       }
                                       else
                                       {
                                           SearchDriverArHandler();
                                       }
                                   }

                               }
                           }
                           else
                           {

                               // data.Violation.Defects selectedDefect = (data.Violation.Defects)grdAddedDefects.SelectedItem;
                               // = 
                               data.Violation.Defects selectedDefect = AddedDefects.Single((x) => x.DefectID.Equals(Convert.ToInt32(telematics_defect_ID)));
                               if (selectedDefect == null)
                               {
                                   this.txtDriverLiscenseNumber.Text = "";
                                   this.txtDriverName.Text = "";
                                   this.txtDriverNationality.Text = "";
                                   this.txttickCross.Text = "";
                                   return;
                               }

                               this.txtImpounding.Text = (Int32.Parse(txtImpounding.Text) - Int32.Parse(selectedDefect.ImpoundingDays)).ToString();
                               if (selectedDefect.DefectCode.Equals(Properties.Settings.Default.telematicsDefectCode))
                               {
                                   this.txtDriverLiscenseNumber.Text = "";
                                   this.txtDriverName.Text = "";
                                   this.txtDriverNationality.Text = "";
                                   this.txttickCross.Text = "";
                               }
                               AddedDefects.Remove(selectedDefect);

                               AppProperties.selectedDefectsEn.RemoveAll((x) => x.Contains(selectedDefect.DefectID.ToString()));


                               deleteAllImagesOfDefect();
                               //--------------------------------------------------

                               grdAddedDefects.ItemsSource = null;
                               grdAddedDefects.ItemsSource = AddedDefects;


                               changeImageRadishToOrignal(selectedDefect);
                           }
                       }
                       catch (Exception ex)
                       {
                           App.VSDLog.Info(ex.StackTrace);
                           WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
                       } */
        }

        private void SetVisibility(bool IsError)
        {
            if (IsError)
            {
                txtDriverNationality.Background = Brushes.White;
                txtDriverNationality.IsReadOnly = false;
                txtDriverName.Background = Brushes.White;
                txtDriverName.IsReadOnly = false;
            }
            else
            {
                //txtDriverNationality.Background = Brushes.Gray;
                ////txtDriverNationality.IsReadOnly = true;
                //txtDriverName.Background = Brushes.Gray;
                //txtDriverName.IsReadOnly = true;

                txtDriverNationality.Background = Brushes.White;
                txtDriverNationality.IsReadOnly = false;
                txtDriverName.Background = Brushes.White;
                txtDriverName.IsReadOnly = false;
            }
        }
        //------------------------------------------------------------------------------------------------

    }
}
