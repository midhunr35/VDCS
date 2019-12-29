using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using VSDApp.com.rta.vsd.hh.manager;
using VSDApp.com.rta.vsd.hh.utilities;
using VSDApp.com.rta.vsd.validation;
using VSDApp.ProgressDialog;
using VSDApp.WPFMessageBoxControl;

namespace VSDApp.com.rta.vsd.hh.ui
{
    /// <summary>
    /// Interaction logic for ucProfileManagment.xaml
    /// </summary>
    public partial class ucProfileManagment : UserControl
    {
        MainWindow m_MainWindow;
        bool is_PicSelected = false;
        String Chosen_Pic_String = "IA==";
        String Chosen_Pic_Format = string.Empty;
        IValidation _iValidate;
        private string _validationResult;
        public ucProfileManagment(MainWindow mainWnd)
        {
            InitializeComponent();
            m_MainWindow = mainWnd;
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ChangeControlPositions();
            System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.DigitSubstitution = System.Globalization.DigitShapes.None;
            this.UpdateLayout();
           
           
        }
        public void ChangeControlPositions()
        {
            try
            {
                if (new CommonUtils().GetScreenOrientation() == AppProperties.LandScapetMode)
                {
                    MainGrid.Children.Remove(lblUserName);
                    Grid.SetRow(lblUserName, 3);
                    Grid.SetColumn(lblUserName, 1);
                    MainGrid.Children.Add(lblUserName);

                    MainGrid.Children.Remove(txtBoxUserName);
                    Grid.SetRow(txtBoxUserName, 3);
                    Grid.SetColumn(txtBoxUserName, 3);
                    MainGrid.Children.Add(txtBoxUserName);

                    MainGrid.Children.Remove(lblRTAEmployeNumber);
                    Grid.SetRow(lblRTAEmployeNumber, 5);
                    Grid.SetColumn(lblRTAEmployeNumber, 1);
                    MainGrid.Children.Add(lblRTAEmployeNumber);

                    MainGrid.Children.Remove(txtRTAEmpNo);
                    Grid.SetRow(txtRTAEmpNo, 5);
                    Grid.SetColumn(txtRTAEmpNo, 3);
                    MainGrid.Children.Add(txtRTAEmpNo);

                    MainGrid.Children.Remove(lblFirstName);
                    Grid.SetRow(lblFirstName, 7);
                    Grid.SetColumn(lblFirstName, 1);
                    MainGrid.Children.Add(lblFirstName);

                    MainGrid.Children.Remove(txtFirstName);
                    Grid.SetRow(txtFirstName, 7);
                    Grid.SetColumn(txtFirstName, 3);
                    MainGrid.Children.Add(txtFirstName);


                    MainGrid.Children.Remove(lblLastName);
                    Grid.SetRow(lblLastName, 9);
                    Grid.SetColumn(lblLastName, 1);
                    MainGrid.Children.Add(lblLastName);

                    MainGrid.Children.Remove(txtBoxLastName);
                    Grid.SetRow(txtBoxLastName, 9);
                    Grid.SetColumn(txtBoxLastName, 3);
                    MainGrid.Children.Add(txtBoxLastName);

                    MainGrid.Children.Remove(lblFirstNameAr);
                    Grid.SetRow(lblFirstNameAr, 11);
                    Grid.SetColumn(lblFirstNameAr, 1);
                    MainGrid.Children.Add(lblFirstNameAr);

                    MainGrid.Children.Remove(txtBoxFirstNameAr);
                    Grid.SetRow(txtBoxFirstNameAr, 11);
                    Grid.SetColumn(txtBoxFirstNameAr, 3);
                    MainGrid.Children.Add(txtBoxFirstNameAr);

                    MainGrid.Children.Remove(lblLastNameAr);
                    Grid.SetRow(lblLastNameAr, 13);
                    Grid.SetColumn(lblLastNameAr, 1);
                    MainGrid.Children.Add(lblLastNameAr);

                    MainGrid.Children.Remove(txtBoxLastNameAr);
                    Grid.SetRow(txtBoxLastNameAr, 13);
                    Grid.SetColumn(txtBoxLastNameAr, 3);
                    MainGrid.Children.Add(txtBoxLastNameAr);

                    MainGrid.Children.Remove(lblDesiginationAr);
                    Grid.SetRow(lblDesiginationAr, 15);
                    Grid.SetColumn(lblDesiginationAr, 1);
                    MainGrid.Children.Add(lblDesiginationAr);

                    MainGrid.Children.Remove(txtBoxDesiginationAr);
                    Grid.SetRow(txtBoxDesiginationAr, 15);
                    Grid.SetColumn(txtBoxDesiginationAr, 3);
                    MainGrid.Children.Add(txtBoxDesiginationAr);

                    MainGrid.Children.Remove(imagRTAInsp);
                    Grid.SetRow(imagRTAInsp, 1);
                    Grid.SetColumn(imagRTAInsp, 7);
                    Grid.SetRowSpan(imagRTAInsp, 5);                    
                    MainGrid.Children.Add(imagRTAInsp);


                    MainGrid.Children.Remove(grdInternal);
                    Grid.SetRow(grdInternal, 7);
                    Grid.SetColumn(grdInternal, 7);
                    MainGrid.Children.Add(grdInternal);



                    MainGrid.Children.Remove(lblPicFormat);
                    Grid.SetRow(lblPicFormat, 5);
                    Grid.SetColumn(lblPicFormat, 5);
                    Grid.SetColumnSpan(lblPicFormat, 3);
                    MainGrid.Children.Add(lblPicFormat);



                    MainGrid.Children.Remove(lblDesigination);
                    Grid.SetRow(lblDesigination, 9);
                    Grid.SetColumn(lblDesigination, 5);                   
                    MainGrid.Children.Add(lblDesigination);

                    MainGrid.Children.Remove(txtBoxDesigination);
                    Grid.SetRow(txtBoxDesigination, 9);
                    Grid.SetColumn(txtBoxDesigination, 7);
                    MainGrid.Children.Add(txtBoxDesigination);

                    MainGrid.Children.Remove(lblMobileNo);
                    Grid.SetRow(lblMobileNo, 11);
                    Grid.SetColumn(lblMobileNo, 5);
                    MainGrid.Children.Add(lblMobileNo);

                    MainGrid.Children.Remove(txtBoxMob);
                    Grid.SetRow(txtBoxMob, 11);
                    Grid.SetColumn(txtBoxMob, 7);
                    MainGrid.Children.Add(txtBoxMob);

                    MainGrid.Children.Remove(lblEmail);
                    Grid.SetRow(lblEmail, 13);
                    Grid.SetColumn(lblEmail, 5);
                    MainGrid.Children.Add(lblEmail);

                    MainGrid.Children.Remove(txtBoxEmail);
                    Grid.SetRow(txtBoxEmail, 13);
                    Grid.SetColumn(txtBoxEmail, 7);
                    MainGrid.Children.Add(txtBoxEmail);


                    MainGrid.Children.Remove(grdIntenal2nd);
                    Grid.SetRow(grdIntenal2nd, 15);
                    Grid.SetColumn(grdIntenal2nd, 7);
                    MainGrid.Children.Add(grdIntenal2nd);

                }
                else
                {

                    MainGrid.Children.Remove(lblPicFormat);
                    Grid.SetRow(lblPicFormat, 3);
                    Grid.SetColumn(lblPicFormat, 1);
                    Grid.SetColumnSpan(lblPicFormat, 3);
                    MainGrid.Children.Add(lblPicFormat);

                    MainGrid.Children.Remove(imagRTAInsp);
                    Grid.SetRow(imagRTAInsp, 3);
                    Grid.SetColumn(imagRTAInsp, 3);
                    Grid.SetRowSpan(imagRTAInsp, 5);
                    MainGrid.Children.Add(imagRTAInsp);
                    

                    MainGrid.Children.Remove(grdInternal);
                    Grid.SetRow(grdInternal, 5);
                    Grid.SetColumn(grdInternal, 3);
                    MainGrid.Children.Add(grdInternal);


                    MainGrid.Children.Remove(lblUserName);
                    Grid.SetRow(lblUserName, 7);
                    Grid.SetColumn(lblUserName, 1);
                    MainGrid.Children.Add(lblUserName);

                    MainGrid.Children.Remove(txtBoxUserName);
                    Grid.SetRow(txtBoxUserName, 7);
                    Grid.SetColumn(txtBoxUserName, 3);
                    MainGrid.Children.Add(txtBoxUserName);

                    MainGrid.Children.Remove(lblRTAEmployeNumber);
                    Grid.SetRow(lblRTAEmployeNumber, 9);
                    Grid.SetColumn(lblRTAEmployeNumber, 1);
                    MainGrid.Children.Add(lblRTAEmployeNumber);

                    MainGrid.Children.Remove(txtBoxUserName);
                    Grid.SetRow(txtBoxUserName, 9);
                    Grid.SetColumn(txtBoxUserName, 3);
                    MainGrid.Children.Add(txtBoxUserName);

                    MainGrid.Children.Remove(lblFirstName);
                    Grid.SetRow(lblFirstName, 11);
                    Grid.SetColumn(lblFirstName, 1);
                    MainGrid.Children.Add(lblFirstName);

                    MainGrid.Children.Remove(txtFirstName);
                    Grid.SetRow(txtFirstName, 11);
                    Grid.SetColumn(txtFirstName, 3);
                    MainGrid.Children.Add(txtFirstName);


                    MainGrid.Children.Remove(lblLastName);
                    Grid.SetRow(lblLastName, 13);
                    Grid.SetColumn(lblLastName, 1);
                    MainGrid.Children.Add(lblLastName);

                    MainGrid.Children.Remove(txtBoxLastName);
                    Grid.SetRow(txtBoxLastName, 13);
                    Grid.SetColumn(txtBoxLastName, 3);
                    MainGrid.Children.Add(txtBoxLastName);

                    MainGrid.Children.Remove(lblFirstNameAr);
                    Grid.SetRow(lblFirstNameAr, 15);
                    Grid.SetColumn(lblFirstNameAr, 1);
                    MainGrid.Children.Add(lblFirstNameAr);

                    MainGrid.Children.Remove(txtBoxFirstNameAr);
                    Grid.SetRow(txtBoxFirstNameAr, 15);
                    Grid.SetColumn(txtBoxFirstNameAr, 3);
                    MainGrid.Children.Add(txtBoxFirstNameAr);

                    MainGrid.Children.Remove(lblLastNameAr);
                    Grid.SetRow(lblLastNameAr, 17);
                    Grid.SetColumn(lblLastNameAr, 1);
                    MainGrid.Children.Add(lblLastNameAr);

                    MainGrid.Children.Remove(txtBoxLastNameAr);
                    Grid.SetRow(txtBoxLastNameAr, 17);
                    Grid.SetColumn(txtBoxLastNameAr, 3);
                    MainGrid.Children.Add(txtBoxLastNameAr);

                    MainGrid.Children.Remove(lblDesiginationAr);
                    Grid.SetRow(lblDesiginationAr, 19);
                    Grid.SetColumn(lblDesiginationAr, 1);
                    MainGrid.Children.Add(lblDesiginationAr);

                    MainGrid.Children.Remove(txtBoxDesiginationAr);
                    Grid.SetRow(txtBoxDesiginationAr, 19);
                    Grid.SetColumn(txtBoxDesiginationAr, 3);
                    MainGrid.Children.Add(txtBoxDesiginationAr);

                  

                    



                    MainGrid.Children.Remove(lblDesigination);
                    Grid.SetRow(lblDesigination, 21);
                    Grid.SetColumn(lblDesigination, 1);
                    MainGrid.Children.Add(lblDesigination);

                    MainGrid.Children.Remove(txtBoxDesigination);
                    Grid.SetRow(txtBoxDesigination, 21);
                    Grid.SetColumn(txtBoxDesigination, 3);
                    MainGrid.Children.Add(txtBoxDesigination);

                    MainGrid.Children.Remove(lblMobileNo);
                    Grid.SetRow(lblMobileNo, 23);
                    Grid.SetColumn(lblMobileNo, 1);
                    MainGrid.Children.Add(lblMobileNo);

                    MainGrid.Children.Remove(txtBoxMob);
                    Grid.SetRow(txtBoxMob, 23);
                    Grid.SetColumn(txtBoxMob, 3);
                    MainGrid.Children.Add(txtBoxMob);

                    MainGrid.Children.Remove(lblEmail);
                    Grid.SetRow(lblEmail, 25);
                    Grid.SetColumn(lblEmail, 1);
                    MainGrid.Children.Add(lblEmail);

                    MainGrid.Children.Remove(txtBoxEmail);
                    Grid.SetRow(txtBoxEmail, 25);
                    Grid.SetColumn(txtBoxEmail,3);
                    MainGrid.Children.Add(txtBoxEmail);


                    MainGrid.Children.Remove(grdIntenal2nd);
                    Grid.SetRow(grdIntenal2nd, 27);
                    Grid.SetColumn(grdIntenal2nd, 3);
                    MainGrid.Children.Add(grdIntenal2nd);

                }
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void UserControl_Initialized(object sender, EventArgs e)
        {

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
              //  PopulateLoggedInUserData();
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }

        }

        private void txtBoxUserName_GotFocus(object sender, RoutedEventArgs e)
        {
            CommonUtils.ShowKeyBoard();
        }

        private void txtBoxUserName_LostFocus(object sender, RoutedEventArgs e)
        {
            
            CommonUtils.CLoseKeyBoard();
        }

        private void txtBoxUserName_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CommonUtils.ShowKeyBoard();
        }



        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                String Chosen_File = "";
                is_PicSelected = false;
                System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();

                openFileDialog1.Filter = "Image Files (*.JPG;*.PNG,*.JPEG)|*.JPG;*.PNG;*.JPEG";
                //  "All files (*.*)|*.*"; ;
                openFileDialog1.InitialDirectory = "C:";
                openFileDialog1.Title = "Select Image";
                // openFileDialog1.FileName = "";
                openFileDialog1.CheckFileExists = true;
                if (openFileDialog1.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
                {
                    Chosen_File = openFileDialog1.FileName;

                    BitmapImage image = new BitmapImage(new Uri(openFileDialog1.FileName));
                    float hres = image.PixelWidth;
                    float vres = image.PixelHeight;
                    if (hres > 1000 || vres > 1000)
                    {
                        WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("lblPicWarning"));
                        return;
                    }
                    
                    imagRTAInsp.Source = image;
                    is_PicSelected = true;
                    FileStream fileStream = new FileStream(Chosen_File, FileMode.Open, FileAccess.Read);
                    byte[] buffer = new byte[fileStream.Length];
                    fileStream.Read(buffer, 0, (int)fileStream.Length);
                    fileStream.Close();
                    Chosen_Pic_String = Convert.ToBase64String(buffer);
                    string ext = System.IO.Path.GetExtension(Chosen_File);
                    Chosen_Pic_Format = ext.Replace(".",@"image/");
                }
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void btnBtnRemove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string inspector_pic_path = AppProperties.InspectorImagesPath + "\\" + AppProperties.LoggedInUser.UserName.ToLower() + ".png";

                BitmapImage image = new BitmapImage(new Uri(inspector_pic_path));

                imagRTAInsp.Source = image;
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }



        private void imagRTAInsp_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {
            System.Windows.Rect r = new System.Windows.Rect(e.NewSize);
            RectangleGeometry gm = new RectangleGeometry(r, 40, 40); // 40 is radius here
            ((UIElement)sender).Clip = gm;
        }
        public void PopulateLoggedInUserData()
        {
            try
            {
                this.txtBoxUserName.Text = AppProperties.LoggedInUser.UserName;
                this.txtFirstName.Text = AppProperties.LoggedInUser.FirstName;
                this.txtBoxFirstNameAr.Text = AppProperties.LoggedInUser.FistNameAr;
                this.txtBoxLastName.Text = AppProperties.LoggedInUser.LastName;
                this.txtBoxLastNameAr.Text = AppProperties.LoggedInUser.LastNameAr;
                this.txtBoxDesigination.Text = AppProperties.LoggedInUser.Desigination;
                this.txtBoxDesiginationAr.Text = AppProperties.LoggedInUser.DesiginationAr;
                this.txtBoxEmail.Text = AppProperties.LoggedInUser.Email;
                this.txtBoxMob.Text = AppProperties.LoggedInUser.MobNumber;
                this.txtRTAEmpNo.Text = AppProperties.LoggedInUser.EmployeID;
                string inspector_pic_path = AppProperties.InspectorImagesPath + "\\" + AppProperties.LoggedInUser.UserName.ToLower() + ".png";

                BitmapImage image = new BitmapImage(new Uri(inspector_pic_path));

                imagRTAInsp.Source = image;
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                App.VSDLog.Info("\n ProfileManagment.Update Info()------------ START--------------- ");
                _iValidate = (IValidation)new ProfileManagmentValidation();
                _validationResult = _iValidate.Validate(this);
             //   dtProvisioanlViolationsData = new DataTable();

                if (_validationResult != "Valid")
                {
                    WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), _validationResult);
                    return;
                }
                IUserAccountManager iUserAccount = (IUserAccountManager)UserAccountManager.GetInstance();
                bool check = false;
                string mobileNum = this.txtBoxMob.Text;
               // Chosen_Pic_Format = @"image\png";
                if (!is_PicSelected)
                    Chosen_Pic_String = Convert.ToBase64String(AppProperties.LoggedInUser.PictureString);
                

                ProgressDialogResult result_offline = ProgressDialog.ProgressDialog.Execute(this.m_MainWindow, new CommonUtils().GetStringValue("lblUpdatingUserAccount"), (bw, we) =>
                {

                    check = iUserAccount.UpdateUsreAccounDetials(Chosen_Pic_Format, Chosen_Pic_String, mobileNum);
                    ProgressDialog.ProgressDialog.CheckForPendingCancellation(bw, we);

                }, ProgressDialogSettings.WithSubLabelAndCancel);

                if (result_offline == null || result_offline.Cancelled)
                    return;
                else if (result_offline.OperationFailed)
                    return;
                if (check)
                {
                    WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("lblUserProfileUpdated"));
                    AppProperties.LoggedInUser.MobNumber = mobileNum;
                    m_MainWindow.lblInspectorPhone.Content = mobileNum;
                    if (Chosen_Pic_String == "IA==")
                        return;
                    byte[] imageBytes = Convert.FromBase64String(Chosen_Pic_String);
                    MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);


                    System.Windows.Media.Imaging.BitmapImage bImg = new System.Windows.Media.Imaging.BitmapImage();
                    bImg.BeginInit();
                    bImg.StreamSource = new MemoryStream(imageBytes.ToArray());
                    bImg.EndInit();
                    m_MainWindow.imageRTAInsp.Source = bImg;                  
                    AppProperties.LoggedInUser.PictureString = imageBytes;
                   // AppProperties.LoggedInUser.MobNumber = mobileNum; 
                }
                else
                {
                    Chosen_Pic_String = Convert.ToBase64String(AppProperties.LoggedInUser.PictureString); ;
                    App.VSDLog.Info("ProfileManagment.Update Info():Exception While Updating info");                    
                    WPFMessageBox.Show(new CommonUtils().GetStringValue("DataValidation"), new CommonUtils().GetStringValue("ErrorException"), AppProperties.errorMessageFromBusiness);

                }
                App.VSDLog.Info("\n ProfileManagment.Update Info()------------ START--------------- ");
            }
            catch (Exception ex)
            {
                App.VSDLog.Info("\n ProfileManagment.Update Info()Exception- ");
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }

        }

        private void txtBoxMob_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.m_MainWindow.MainContentControl.Content = null;
                this.m_MainWindow.MainContentControl.Content = new ucWellComeScreen(this.m_MainWindow);
                
            }
            catch (Exception ex)
            {
                App.VSDLog.Info("\n ProfileManagment.Button Back Exception- ");
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void MainGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            CommonUtils.CLoseKeyBoard();
        }
    }
}
