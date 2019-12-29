using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using VSDApp.com.rta.vsd.hh.encryption;
using VSDApp.com.rta.vsd.hh.utilities;
using VSDApp.handHeldService;
using Windows.UI.Xaml.Media.Imaging;

namespace VSDApp.com.rta.vsd.hh.db
{
    class DBAppLoginManager : IDBManager
    {
        private static DBAppLoginManager _dbAppLoginManager;
        private DBAppLoginManager() { }

        public static DBAppLoginManager GetInstance()
        {
            if (_dbAppLoginManager == null)
            {
                _dbAppLoginManager = new DBAppLoginManager();
            }
            return _dbAppLoginManager;
        }
        bool IDBManager.AuthenticateUser(string userName, string userPass)
        {
            App.VSDLog.Info("\n ucLoginEn.LogingOffline(): from local DB:");
            IDBManager iDBManager = (IDBManager)DBConnectionManager.GetInstance();
            // byte[] pass = ((IEncryptionDecryptionManager)new EncryptionManager()).Encrypt(userPass, AppProperties.encryptionKey, AppProperties.iVector);

            string sql = "select * from VSD_USER_ACCOUNT where User_Name like @empUserName";//and Password like @empPassword
            App.VSDLog.Info("\n ucLoginEn.LogingOffline(): SQL" + sql);
            SqlCeConnection con = iDBManager.GetConnection();

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            SqlCeCommand command = new SqlCeCommand(sql, iDBManager.GetConnection());
            command.Parameters.Add("@empUserName", SqlDbType.NChar, 100).Value = userName;
            //command.Parameters.Add("@empPassword", SqlDbType.VarBinary, 20).Value = empPassword;
            try
            {

                SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);
                int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;

                byte[] encryptedBytes;
                string decryptedPassword;
                if (rs.HasRows)
                {
                    //return false;
                    rs.ReadFirst();
                    encryptedBytes = (byte[])rs.GetValue(2);
                    decryptedPassword = ((IEncryptionDecryptionManager)new DecryptionManager()).Decrypt(encryptedBytes, AppProperties.encryptionKey, AppProperties.iVector);
                    if (userPass == decryptedPassword)
                    {
                        AppProperties.empID = rs.GetString(4).Trim();
                        AppProperties.LoggedInUser.UserName = (rs.GetString(1) == null) ? "" : rs.GetString(1).Trim();
                        AppProperties.LoggedInUser.EmpPassword = decryptedPassword;

                        AppProperties.LoggedInUser.FirstName = (rs.GetString(5) == null) ? "" : rs.GetString(5).Trim();
                        AppProperties.LoggedInUser.FistNameAr = (rs.GetString(6) == null) ? "" : rs.GetString(6).Trim();
                        AppProperties.LoggedInUser.LastName = (rs.GetString(7) == null) ? "" : rs.GetString(7).Trim();
                        AppProperties.LoggedInUser.LastNameAr = (rs.GetString(8) == null) ? "" : rs.GetString(8).Trim();
                        AppProperties.LoggedInUser.Desigination = (rs.GetString(9) == null) ? "" : rs.GetString(9).Trim();
                        AppProperties.LoggedInUser.DesiginationAr = (rs.GetString(10) == null) ? "" : rs.GetString(10).Trim();
                        AppProperties.LoggedInUser.MobNumber = (rs.GetString(11) == null) ? "" : rs.GetString(11).Trim();
                        // AppProperties.LoggedInUser.PictureString = (rs.GetString(12) == null) ? "" : rs.GetString(12).Trim();
                        AppProperties.LoggedInUser.PictureFormat = (rs.GetString(13) == null) ? "" : rs.GetString(13).Trim();
                        AppProperties.LoggedInUser.Email = (rs.GetString(14) == null) ? "" : rs.GetString(14).Trim();

                        return true;
                    }

                    while (rs.NextResult())
                    {
                        encryptedBytes = (byte[])rs.GetValue(2);
                        decryptedPassword = ((IEncryptionDecryptionManager)new DecryptionManager()).Decrypt(encryptedBytes, AppProperties.encryptionKey, AppProperties.iVector);
                        if (userPass == decryptedPassword)
                        {
                            AppProperties.empID = rs.GetString(4).Trim();
                            AppProperties.LoggedInUser.UserName = (rs.GetString(1) == null) ? "" : rs.GetString(1).Trim();
                            AppProperties.LoggedInUser.EmpPassword = decryptedPassword;

                            AppProperties.LoggedInUser.FirstName = (rs.GetString(5) == null) ? "" : rs.GetString(5).Trim();
                            AppProperties.LoggedInUser.FistNameAr = (rs.GetString(6) == null) ? "" : rs.GetString(6).Trim();
                            AppProperties.LoggedInUser.LastName = (rs.GetString(7) == null) ? "" : rs.GetString(7).Trim();
                            AppProperties.LoggedInUser.LastNameAr = (rs.GetString(8) == null) ? "" : rs.GetString(8).Trim();
                            AppProperties.LoggedInUser.Desigination = (rs.GetString(9) == null) ? "" : rs.GetString(9).Trim();
                            AppProperties.LoggedInUser.DesiginationAr = (rs.GetString(10) == null) ? "" : rs.GetString(10).Trim();
                            AppProperties.LoggedInUser.MobNumber = (rs.GetString(11) == null) ? "" : rs.GetString(11).Trim();
                            // AppProperties.LoggedInUser.PictureString = (rs.GetString(12) == null) ? "" : rs.GetString(12).Trim();
                            AppProperties.LoggedInUser.PictureFormat = (rs.GetString(13) == null) ? "" : rs.GetString(13).Trim();
                            AppProperties.LoggedInUser.Email = (rs.GetString(14) == null) ? "" : rs.GetString(14).Trim();

                            return true;
                        }
                    }

                }


            }
            catch (Exception ex)
            {
                App.VSDLog.Info("\n ucLoginEn.LogingOffline(): Exception while login offlien from local DB");
                // System.Windows.Forms.MessageBox.Show(ex.Message)

                ; return false;
            }

            return false;
        }


        #region IDBManager Members

        bool IDBManager.SaveUserCredentials(handHeldService.SynchronizeConfigDataResponseItem response)
        {
            //throw new NotImplementedException();
            try
            {

                App.VSDLog.Info("\n DBAPPLoginManager: SaveUserCredentials--------------------------Start-----------------------------");
                SqlCeConnection cn = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();

                if (cn.State == ConnectionState.Closed)
                {
                    cn.Open();
                }



                //    command.ExecuteNonQuery();

                //}
                //catch (Exception ex)
                //{
                //    System.Windows.Forms.MessageBox.Show("Table exists"+ex.Message);
                //    return true;

                //}

                byte[] pass = ((IEncryptionDecryptionManager)new EncryptionManager()).Encrypt(AppProperties.empPassword, AppProperties.encryptionKey, AppProperties.iVector);

                string sql = "delete from VSD_USER_ACCOUNT";//and Password like @empPassword
                SqlCeCommand deleteCommand = new SqlCeCommand(sql, cn);


                SqlCeCommand insertCommand = new SqlCeCommand(sql, cn);
                deleteCommand.Parameters.Add("@empUserName", SqlDbType.NChar, 100).Value = AppProperties.empUserName.ToLower();


                SqlCeResultSet rs = deleteCommand.ExecuteResultSet(ResultSetOptions.Scrollable);


                sql = "insert into VSD_USER_ACCOUNT (USER_EMP_ID,User_Account_ID,User_Name,User_Password,User_Status,USER_FIRSTNAME,USER_FIRSTNAMEAr,USER_LASTNAME,USER_LASTNAMEAr,USER_DESIGINATION,USER_DESIGINATIONAr,USER_MOBILENUMBER,USER_PICTURESTRING,USER_PICTUREFORMATE,USER_EMAIL) " +
                    "values (@EmpID,'1',@empUserName,@empPassword,@USER_STATUS,@USER_FIRSTNAME,@USER_FIRSTNAMEAr,@USER_LASTNAME,@USER_LASTNAMEAr,@USER_DESIGINATION,@USER_DESIGINATIONAr,@USER_MOBILENUMBER,@USER_PICTURESTRING,@USER_PICTUREFORMATE,@USER_EMAIL)";

                if (cn.State == ConnectionState.Closed) cn.Open();
                insertCommand = new SqlCeCommand(sql, cn);
                // insertCommand.Parameters.Add("@User_ACCOUNT_ID",SqlDbType.Int,4).Value = "1";
                insertCommand.Parameters.Add("@empUserName", SqlDbType.NChar, 100).Value = AppProperties.empUserName;
                EncryptionManager Encryptor = new EncryptionManager();
                IEncryptionDecryptionManager Iencryptor = (IEncryptionDecryptionManager)Encryptor;
                insertCommand.Parameters.Add("@empPassword", SqlDbType.VarBinary, 512).Value = Iencryptor.Encrypt(AppProperties.empPassword, AppProperties.encryptionKey, AppProperties.iVector);
                insertCommand.Parameters.Add("@USER_STATUS", SqlDbType.TinyInt, 1).Value = "1";
                insertCommand.Parameters.Add("@EmpID", SqlDbType.NChar, 20).Value = (AppProperties.empID == null) ? "" : AppProperties.empID;
                insertCommand.Parameters.Add("@USER_FIRSTNAME", SqlDbType.NVarChar, 1000).Value = (response.firstNameEn == null) ? "" : response.firstNameEn;
                insertCommand.Parameters.Add("@USER_FIRSTNAMEAr", SqlDbType.NVarChar, 1000).Value = (response.firstNameAr == null) ? "" : response.firstNameAr;
                insertCommand.Parameters.Add("@USER_LASTNAME", SqlDbType.NVarChar, 1000).Value = (response.lastNameEn == null) ? "" : response.lastNameEn;
                insertCommand.Parameters.Add("@USER_LASTNAMEAr", SqlDbType.NVarChar, 1000).Value = (response.lastNameAr == null) ? "" : response.lastNameAr;
                insertCommand.Parameters.Add("@USER_DESIGINATION", SqlDbType.NVarChar, 1000).Value = (response.designationEn == null) ? "" : response.designationEn;
                insertCommand.Parameters.Add("@USER_DESIGINATIONAr", SqlDbType.NVarChar, 1000).Value = (response.designationAr == null) ? "" : response.designationAr;
                insertCommand.Parameters.Add("@USER_MOBILENUMBER", SqlDbType.NVarChar, 1000).Value = (response.mobileNumber == null) ? "" : response.mobileNumber;
                insertCommand.Parameters.Add("@USER_PICTURESTRING", SqlDbType.NVarChar, 4000).Value = "";
                insertCommand.Parameters.Add("@USER_PICTUREFORMATE", SqlDbType.NVarChar, 1000).Value = (response.pictureMimeType == null) ? "" : response.pictureMimeType;
                insertCommand.Parameters.Add("@USER_EMAIL", SqlDbType.NVarChar, 1000).Value = (response.email == null) ? "" : response.email;

                try
                {
                    insertCommand.ExecuteNonQuery();
                    if (cn.State == ConnectionState.Open) cn.Close();
                }
                catch (Exception ex)
                {
                    App.VSDLog.Info("\n DBAPPLoginManager: SaveUserCredentials--- Internal -Exception");
                    App.VSDLog.Info(ex.StackTrace);
                    //System.Windows.Forms.MessageBox.Show(ex.Message);

                }
                App.VSDLog.Info("\n DBAPPLoginManager: SaveUserCredentials--------------------------Start-----------------------------");

            }
            catch (SqlCeException sqlEx)
            {
                App.VSDLog.Info("\n DBAPPLoginManager: SaveUserCredentials--- SQL EX -Exception");
                App.VSDLog.Info(sqlEx.StackTrace);
                // System.Windows.Forms.MessageBox.Show(sqlEx.Message);
            }

            return true;
        }

        void IDBManager.PopulateOnlineLoggedInUserInfo(SynchronizeConfigDataResponseItem response)
        {
            try
            {
                App.VSDLog.Info("\n DBAPPLoginManager: PopulateOnlineLoggedInUserInfo----------START---------------------");
                AppProperties.LoggedInUser.UserName = AppProperties.empUserName;
                AppProperties.LoggedInUser.EmpPassword = AppProperties.empPassword;
                AppProperties.LoggedInUser.EmployeID = (response.employeeId == null) ? "" : response.employeeId;
                AppProperties.LoggedInUser.FirstName = (response.firstNameEn == null) ? "" : response.firstNameEn;
                AppProperties.LoggedInUser.FistNameAr = (response.firstNameAr == null) ? "" : response.firstNameAr;
                AppProperties.LoggedInUser.LastName = (response.lastNameEn == null) ? "" : response.lastNameEn;
                AppProperties.LoggedInUser.LastNameAr = (response.lastNameAr == null) ? "" : response.lastNameAr;
                AppProperties.LoggedInUser.Desigination = (response.designationEn == null) ? "" : response.designationEn;
                AppProperties.LoggedInUser.DesiginationAr = (response.designationAr == null) ? "" : response.designationAr;
                AppProperties.LoggedInUser.MobNumber = (response.mobileNumber == null) ? "" : response.mobileNumber;
                AppProperties.LoggedInUser.PictureString = response.picture;
                AppProperties.LoggedInUser.PictureFormat = (response.pictureMimeType == null) ? "" : response.pictureMimeType;
                AppProperties.LoggedInUser.Email = (response.email == null) ? "" : response.email;
                ////////////////////////////////

                string PicString = Encoding.UTF8.GetString(response.picture);
                if (PicString == "IA==" || PicString == "" || PicString == " " || PicString.Equals(string.Empty))
                    return;
                byte[] imageBytes = Convert.FromBase64String(PicString);
                MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                System.Windows.Media.Imaging.BitmapImage bImg = new System.Windows.Media.Imaging.BitmapImage();
                bImg.BeginInit();
                bImg.StreamSource = new MemoryStream(imageBytes.ToArray());
                bImg.EndInit();

                // m_MainWindow.imageRTAInsp.Source = bImg;
                //  String myDir=AppProperties.applicationPath + "\\Inspectors_Images";
                //



                string inspector_pic_path = AppProperties.InspectorImagesPath + "\\" + AppProperties.LoggedInUser.UserName.ToLower() + ".png";
                string icon_path = new Uri(inspector_pic_path).LocalPath;
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create((System.Windows.Media.Imaging.BitmapImage)bImg));
                using (FileStream stream = new FileStream(inspector_pic_path, FileMode.Create))
                    encoder.Save(stream);

            }
            catch (SqlCeException sqlEx)
            {
                App.VSDLog.Info("\n DBAPPLoginManager: PopulateOnlineLoggedInUserInfo--- SQL EX -Exception");
                App.VSDLog.Info(sqlEx.StackTrace);
                // System.Windows.Forms.MessageBox.Show(sqlEx.Message);
            }
        }

        public SqlCeConnection GetConnection()
        {
            throw new NotImplementedException();
        }

        public bool StoreOfflineData()
        {
            throw new NotImplementedException();
        }

        public bool OfflineDataExist()
        {
            throw new NotImplementedException();
        }

        public bool CreateInitialDataBase()
        {
            throw new NotImplementedException();
        }


        public void Synchronize_defectCategoty(handHeldService.SynchronizeConfigDataResponseItem test)
        {
            try
            {
                App.VSDLog.Info("\n SynchronizeConfig().Synchronize_defectCategoty: --------------- START");
                SqlCeConnection con = ((VSDApp.com.rta.vsd.hh.db.IDBManager)VSDApp.com.rta.vsd.hh.db.DBConnectionManager.GetInstance()).GetConnection();
                string sqlQuery;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCeCommand command;
                SqlCeResultSet rs;
                int rowCount;
                int temp = -1;
                int count = 0;
                //check for defect category updates
                if (test.defectCategory != null)
                {
                    handHeldService.DefectCategory[] WebServiceDefects = new VSDApp.handHeldService.DefectCategory[test.defectCategory.Length];
                    WebServiceDefects = test.defectCategory;

                    foreach (handHeldService.DefectCategory x in WebServiceDefects)
                    {
                        if (con.State == ConnectionState.Closed) { con.Open(); }
                        /*
                         * IF EXISTS (SELECT * FROM VSD_Defect WHERE DefectID = @DefectID)
                            UPDATE Table1 SET (...) WHERE Column1='SomeValue'
                            ELSE
                            INSERT INTO Table1 VALUES (...)
                         */
                        //rountine for Defect Category Table
                        sqlQuery = "SELECT COUNT(*) AS Expr1"
                                    + " FROM VSD_Defect_Category"
                                    + " WHERE (Defect_Category_ID = @DefectCategoryID )";
                        command = new SqlCeCommand(sqlQuery, con);
                        command.Parameters.Clear();
                        command.Parameters.Add("@DefectCategoryID", SqlDbType.Int).Value = Int32.Parse(x.id);
                        rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);
                        rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;
                        String.Format("{0:M/d/yyyy HH:mm:ss}", DateTime.Now);
                        if (rowCount > 0)
                        {
                            rs.ReadFirst();
                            temp = rs.GetInt32(0);
                        }
                        if (temp == 0)
                        {
                            if ((x.parentID == null || x.parentID.Trim().Equals("")))
                            {
                                sqlQuery = " INSERT INTO VSD_Defect_Category (Defect_Category_ID ,Record_Type, Category_Code,Category_Desc,Category_Desc_A,Category_Name,Category_Name_A,IsDisabled) VALUES (" + (x.id != null ? "" + x.id + "" : "NULL") + "," + (x.recordType != null ? "'" + x.recordType + "'" : "NULL") + "," + (x.categoryCode != null ? "'" + x.categoryCode + "'" : "NULL") + "," + (x.categoryDescription != null ? "'" + x.categoryDescription.Replace("'", "`") + "'" : "NULL") + "," + (x.categoryDescriptionArabic != null ? "'" + x.categoryDescriptionArabic.Replace("'", "`") + "'" : "NULL") + "," + (x.categoryName != null ? "'" + x.categoryName.Replace("'", "`") + "'" : "NULL") + "," + (x.categoryNameArabic != null ? "'" + x.categoryNameArabic.Replace("'", "`") + "'" : "NULL") + "," + (x.isDisabled != null ? "'" + x.isDisabled + "'" : "NULL") + ")";
                            }
                            else
                            {
                                sqlQuery = " INSERT INTO VSD_Defect_Category (Defect_Category_ID ,Parent_Defect_Category_ID,Record_Type, Category_Code,Category_Desc,Category_Desc_A,Category_Name,Category_Name_A,IsDisabled) VALUES (" + (x.id != null ? "" + x.id + "" : "NULL") + "," + (x.parentID != null ? "" + x.parentID + "" : "NULL") + "," + (x.recordType != null ? "'" + x.recordType + "'" : "NULL") + "," + (x.categoryCode != null ? "'" + x.categoryCode + "'" : "NULL") + "," + (x.categoryDescription != null ? "'" + x.categoryDescription.Replace("'", "`") + "'" : "NULL") + "," + (x.categoryDescriptionArabic != null ? "'" + x.categoryDescriptionArabic.Replace("'", "`") + "'" : "NULL") + "," + (x.categoryName != null ? "'" + x.categoryName.Replace("'", "`") + "'" : "NULL") + "," + (x.categoryNameArabic != null ? "'" + x.categoryNameArabic.Replace("'", "`") + "'" : "NULL") + "," + (x.isDisabled != null ? "'" + x.isDisabled + "'" : "NULL") + ")";
                            }


                            // sqlQuery = " INSERT INTO VSD_Defect_Category (Defect_Category_ID , Category_Code,Category_Desc,Category_Desc_A,Category_Name,Category_Name_A,IsDisabled) VALUES (" + (x.id != null ? "" + x.id + "" : "NULL") + "," + (x.categoryCode != null ? "'" + x.categoryCode + "'" : "NULL") + "," + (x.categoryDescription != null ? "'" + x.categoryDescription.Replace("'", "`") + "'" : "NULL") + "," + (x.categoryDescriptionArabic != null ? "'" + x.categoryDescriptionArabic.Replace("'", "`") + "'" : "NULL") + "," + (x.categoryName != null ? "'" + x.categoryName.Replace("'", "`") + "'" : "NULL") + "," + (x.categoryNameArabic != null ? "'" + x.categoryNameArabic.Replace("'", "`") + "'" : "NULL") + "," + (x.isDisabled != null ? "'" + x.isDisabled + "'" : "NULL") + ")";

                        }
                        else
                        {

                            if ((x.parentID == null || x.parentID.Trim().Equals("")))
                            {
                                sqlQuery = " UPDATE VSD_Defect_Category SET Defect_Category_ID = " + (x.id != null ? "" + x.id + "" : "NULL") + ",Record_Type= " + (x.recordType != null ? "'" + x.recordType + "'" : "NULL") + ",Category_Code= " + (x.categoryCode != null ? "'" + x.categoryCode + "'" : "NULL") + " ,Category_Desc = " + (x.categoryDescription != null ? "'" + x.categoryDescription.Replace("'", "`") + "'" : "NULL") + ",Category_Desc_A = " + (x.categoryDescriptionArabic != null ? "'" + x.categoryDescriptionArabic.Replace("'", "`") + "'" : "NULL") + ",Category_Name = " + (x.categoryName != null ? "'" + x.categoryName.Replace("'", "`") + "'" : "NULL") + ",Category_Name_A = " + (x.categoryNameArabic != null ? "'" + x.categoryNameArabic.Replace("'", "`") + "'" : "NULL") + ",isdisabled = " + (x.isDisabled != null ? "'" + x.isDisabled + "'" : "NULL") + "  WHERE Defect_Category_ID = " + x.id;
                            }
                            else
                            {
                                sqlQuery = " UPDATE VSD_Defect_Category SET Defect_Category_ID = " + (x.id != null ? "" + x.id + "" : "NULL") + ",Parent_Defect_Category_ID= " + ((x.parentID == null || x.parentID.Trim().Equals("")) ? "" + DBNull.Value + "" : "" + x.parentID + "") + ",Record_Type= " + (x.recordType != null ? "'" + x.recordType + "'" : "NULL") + ",Category_Code= " + (x.categoryCode != null ? "'" + x.categoryCode + "'" : "NULL") + " ,Category_Desc = " + (x.categoryDescription != null ? "'" + x.categoryDescription.Replace("'", "`") + "'" : "NULL") + ",Category_Desc_A = " + (x.categoryDescriptionArabic != null ? "'" + x.categoryDescriptionArabic.Replace("'", "`") + "'" : "NULL") + ",Category_Name = " + (x.categoryName != null ? "'" + x.categoryName.Replace("'", "`") + "'" : "NULL") + ",Category_Name_A = " + (x.categoryNameArabic != null ? "'" + x.categoryNameArabic.Replace("'", "`") + "'" : "NULL") + ",isdisabled = " + (x.isDisabled != null ? "'" + x.isDisabled + "'" : "NULL") + "  WHERE Defect_Category_ID = " + x.id;
                            }

                            // sqlQuery = " UPDATE VSD_Defect_Category SET Defect_Category_ID = " + (x.id != null ? "" + x.id + "" : "NULL") + ",Category_Code= " + (x.categoryCode != null ? "'" + x.categoryCode + "'" : "NULL") + " ,Category_Desc = " + (x.categoryDescription != null ? "'" + x.categoryDescription.Replace("'", "`") + "'" : "NULL") + ",Category_Desc_A = " + (x.categoryDescriptionArabic != null ? "'" + x.categoryDescriptionArabic.Replace("'", "`") + "'" : "NULL") + ",Category_Name = " + (x.categoryName != null ? "'" + x.categoryName.Replace("'", "`") + "'" : "NULL") + ",Category_Name_A = " + (x.categoryNameArabic != null ? "'" + x.categoryNameArabic.Replace("'", "`") + "'" : "NULL") + ",isdisabled = " + (x.isDisabled != null ? "'" + x.isDisabled + "'" : "NULL") + "  WHERE Defect_Category_ID = " + x.id;
                        }
                        //test_sql = sqlQuery;
                        command = new SqlCeCommand(sqlQuery, con);
                        command.Parameters.Add("@CategoryDefectID", SqlDbType.Int).Value = x.id;
                        command.Parameters.Add("@DefectCategoryCode", SqlDbType.NVarChar).Value = x.categoryCode;
                        command.Parameters.Add("@CategoryDescription", SqlDbType.NVarChar).Value = x.categoryDescription;
                        command.Parameters.Add("@CategoryDescriptionArabic", SqlDbType.NVarChar).Value = x.categoryDescriptionArabic;
                        command.Parameters.Add("@CategoryName", SqlDbType.NVarChar).Value = x.categoryName;
                        command.Parameters.Add("@CategoryNameArabic", SqlDbType.NVarChar).Value = x.categoryNameArabic;
                        try
                        {
                            rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);
                        }
                        catch (Exception ex)
                        {
                            App.VSDLog.Info("\n SynchronizeConfig().Synchronize_defectCategoty:Exception sql" + sqlQuery);
                            //  throw;
                        }


                        rs.Close();
                        con.Close();


                    }
                    App.VSDLog.Info("\n SynchronizeConfig().Synchronize_defectCategoty: --------------- " + WebServiceDefects.Length);
                }
                App.VSDLog.Info("\n SynchronizeConfig().Synchronize_defectCategoty: --------------- END");
            }
            catch (Exception ex)
            {
                App.VSDLog.Info("\n SynchronizeConfig().Synchronize_defectCategoty:Exception" + ex.StackTrace);
                //  throw;
            }

        }
        public void Synchronize_defect(handHeldService.SynchronizeConfigDataResponseItem test)
        {
            try
            {
                App.VSDLog.Info("\n SynchronizeConfig().Synchronize_defect: --------------- START");
                SqlCeConnection con = ((VSDApp.com.rta.vsd.hh.db.IDBManager)VSDApp.com.rta.vsd.hh.db.DBConnectionManager.GetInstance()).GetConnection();
                string sqlQuery;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCeCommand command;
                SqlCeResultSet rs;
                int rowCount;
                int temp = -1;

                if (test.defect != null)
                {


                    handHeldService.Defect1[] WebServiceDefects = new VSDApp.handHeldService.Defect1[test.defect.Length];
                    WebServiceDefects = test.defect;

                    int count = 0;
                    foreach (handHeldService.Defect1 x in WebServiceDefects)
                    {
                        if (con.State == ConnectionState.Closed) { con.Open(); }
                        /*
                         * IF EXISTS (SELECT * FROM VSD_Defect WHERE DefectID = @DefectID)
                            UPDATE Table1 SET (...) WHERE Column1='SomeValue'
                            ELSE
                            INSERT INTO Table1 VALUES (...)
                         */
                        sqlQuery = "SELECT COUNT(*) AS Expr1"
                                    + " FROM VSD_Defect"
                                    + " WHERE (Defect_ID = @DefectID)";
                        command = new SqlCeCommand(sqlQuery, con);
                        command.Parameters.Add("@DefectID", SqlDbType.Int).Value = Int32.Parse(x.id);
                        rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);
                        rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;

                        //rountine for Defect Table
                        if (rowCount > 0)
                        {
                            rs.ReadFirst();
                            temp = rs.GetInt32(0);
                        }
                        if (temp == 0)
                        {
                            sqlQuery = " INSERT INTO VSD_Defect (Defect_ID ,Defect_Code_Prefix ,Defect_Code ,Defect_Desc ,Defect_Desc_A ,IsDisabled ,Defect_Name ,Defect_Name_A ,Defect_Type,Defect_Category_ID ) VALUES (@DefectID," + (x.codePrefix != null ? "'" + x.codePrefix + "'" : "NULL") + "," + (x.code != null ? "'" + x.code + "'" : "NULL") + "," + (x.description != null ? "'" + x.description.Replace("'", "`") + "'" : "NULL") + "," + (x.descriptionArabic != null ? "'" + x.descriptionArabic.Replace("'", "`") + "'" : "NULL") + ",'" + x.isDisabled + "'," + (x.name != null ? "'" + x.name.Replace("'", "`") + "'" : "NULL") + "," + (x.nameArabic != null ? "'" + x.nameArabic.Replace("'", "`") + "'" : "NULL") + "," + (x.type != null ? "'" + x.type + "'" : "NULL") + "," + x.category + ")";
                        }
                        else
                        {
                            sqlQuery = " UPDATE VSD_Defect SET Defect_Code_Prefix = " + (x.codePrefix != null ? "'" + x.codePrefix + "'" : "NULL") + ",Defect_Code = " + (x.code != null ? "'" + x.code + "'" : "NULL") + ",Defect_Desc = " + (x.description != null ? "'" + x.description.Replace("'", "`") + "'" : "NULL") + ",Defect_Desc_A = " + (x.descriptionArabic != null ? "'" + x.descriptionArabic.Replace("'", "`") + "'" : "NULL") + ",IsDisabled = " + (x.isDisabled != null ? "'" + x.isDisabled + "'" : "NULL") + ",Defect_Name = " + (x.name != null ? "'" + x.name.Replace("'", "`") + "'" : "NULL") + ",Defect_Name_A = " + (x.nameArabic != null ? "'" + x.nameArabic.Replace("'", "`") + "'" : "NULL") + ",Defect_Type = " + (x.type != null ? "'" + x.type + "'" : "NULL") + ", Defect_Category_ID = " + x.category + " WHERE Defect_ID = " + x.id;
                        }


                        command = new SqlCeCommand(sqlQuery, con);


                        command.Parameters.Add("@DefectCodePrefix", SqlDbType.NVarChar).Value = x.codePrefix;
                        command.Parameters.Add("@DefectCode", SqlDbType.NVarChar).Value = x.code;
                        command.Parameters.Add("@DefectDescription", SqlDbType.NVarChar).Value = x.description;
                        command.Parameters.Add("@DefectDescriptionArabic", SqlDbType.NVarChar).Value = x.descriptionArabic;
                        command.Parameters.Add("@IsDisabled", SqlDbType.NVarChar).Value = x.isDisabled;
                        command.Parameters.Add("@DefectName", SqlDbType.NVarChar).Value = x.name;
                        command.Parameters.Add("@DefectNameArabic", SqlDbType.NVarChar).Value = x.nameArabic;
                        command.Parameters.Add("@DefectType", SqlDbType.NVarChar).Value = x.type;
                        command.Parameters.Add("@DefectID", SqlDbType.Int).Value = Int32.Parse(x.id);
                        command.Parameters.Add("@DefectCategoryID", SqlDbType.Int).Value = Int32.Parse(x.category);

                        try
                        {
                            rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);
                            count++;
                        }
                        catch (Exception ex)
                        {
                            App.VSDLog.Info("\n SynchronizeConfig().Synchronize_defect: --------------- Exception sql" + sqlQuery);

                            // throw;
                        }


                        rs.Close();
                        con.Close();

                    }
                    App.VSDLog.Info("\n SynchronizeConfig().Synchronize_defect:" + WebServiceDefects.Length);
                }
                App.VSDLog.Info("\n SynchronizeConfig().Synchronize_defect: --------------- START");

            }
            catch (Exception ex)
            {
                App.VSDLog.Info("\n SynchronizeConfig().Synchronize_defect: --------------- Exception" + ex.StackTrace);

                // throw;
            }
        }
        public void Synchronize_country(handHeldService.SynchronizeConfigDataResponseItem test)
        {
            try
            {
                App.VSDLog.Info("\n SynchronizeConfig().Synchronize_country: --------------- START");
                SqlCeConnection con = ((VSDApp.com.rta.vsd.hh.db.IDBManager)VSDApp.com.rta.vsd.hh.db.DBConnectionManager.GetInstance()).GetConnection();
                string sqlQuery;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCeCommand command;
                SqlCeResultSet rs;
                int rowCount;
                int temp = -1;


                //check for country updates
                if (test.country != null)
                {
                    handHeldService.Country[] WebServiceDefects = new VSDApp.handHeldService.Country[test.country.Length];
                    WebServiceDefects = test.country;
                    foreach (handHeldService.Country x in WebServiceDefects)
                    {
                        if (con.State == ConnectionState.Closed) { con.Open(); }
                        /*
                         * IF EXISTS (SELECT * FROM VSD_Defect WHERE DefectID = @DefectID)
                            UPDATE Table1 SET (...) WHERE Column1='SomeValue'
                            ELSE
                            INSERT INTO Table1 VALUES (...)
                         */
                        sqlQuery = "SELECT COUNT(*) AS Expr1"
                                    + " FROM VSD_Country"
                                    + " WHERE (Country_ID = @CountryID)";
                        command = new SqlCeCommand(sqlQuery, con);
                        command.Parameters.Add("@CountryID", SqlDbType.Int).Value = Int32.Parse(x.id);
                        rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);
                        rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;

                        //rountine for Defect Table
                        if (rowCount > 0)
                        {
                            rs.ReadFirst();
                            temp = rs.GetInt32(0);
                        }
                        if (temp == 0)
                        {
                            sqlQuery = " INSERT INTO VSD_Country (Country_ID ,Country_Code,Country_Name,Parent_Country_ID) VALUES (" + (x.id != null ? x.id : "NULL") + "," + (x.code != null ? "'" + x.code + "'" : "NULL") + "," + (x.name != null ? "'" + x.name.Replace("'", "`") + "'" : "NULL") + "," + (x.parentId != null ? "" + x.parentId + "" : "NULL") + ") ";
                        }
                        else
                        {
                            sqlQuery = " UPDATE VSD_Country SET Country_Code = " + (x.code != null ? "'" + x.code + "'" : "NULL") + ",Country_Name = " + (x.name != null ? "'" + x.name.Replace("'", "`") + "'" : "NULL") + ",Parent_Country_ID = " + (x.parentId != null ? "" + x.parentId + "" : "NULL") + " WHERE Country_ID = " + (x.id != null ? "" + x.id + "" : "NULL");
                        }
                        command = new SqlCeCommand(sqlQuery, con);
                        command.Parameters.Add("@CountryID", SqlDbType.Int).Value = Int32.Parse(x.id);
                        command.Parameters.Add("@CountryCode", SqlDbType.NChar, 25).Value = x.code;
                        command.Parameters.Add("@CountryName", SqlDbType.NChar, 50).Value = x.name;
                        rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);
                        rs.Close();
                        con.Close();

                    }
                    App.VSDLog.Info("\n SynchronizeConfig().Synchronize_country: " + WebServiceDefects.Length);
                }
                App.VSDLog.Info("\n SynchronizeConfig().Synchronize_country: --------------- END");

            }
            catch (Exception)
            {

                // throw;
            }
        }
        public void Synchronize_countryProperty(handHeldService.SynchronizeConfigDataResponseItem test)
        {
            try
            {
                App.VSDLog.Info("\n SynchronizeConfig().Synchronize_countryProperty: --------------- START");
                SqlCeConnection con = ((VSDApp.com.rta.vsd.hh.db.IDBManager)VSDApp.com.rta.vsd.hh.db.DBConnectionManager.GetInstance()).GetConnection();
                string sqlQuery;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCeCommand command;
                SqlCeResultSet rs;
                int rowCount;
                int temp = -1;

                //check for country property updates
                if (test.countryProperty != null)
                {
                    handHeldService.CountryProperty[] WebServiceDefects = new VSDApp.handHeldService.CountryProperty[test.countryProperty.Length];
                    WebServiceDefects = test.countryProperty;
                    foreach (handHeldService.CountryProperty x in WebServiceDefects)
                    {
                        if (con.State == ConnectionState.Closed) { con.Open(); }
                        /*
                         * IF EXISTS (SELECT * FROM VSD_Defect WHERE DefectID = @DefectID)
                            UPDATE Table1 SET (...) WHERE Column1='SomeValue'
                            ELSE
                            INSERT INTO Table1 VALUES (...)
                         */
                        sqlQuery = "SELECT COUNT(*) AS Expr1"
                                    + " FROM VSD_Country_Prop"
                                    + " WHERE (Country_Prop_ID = @CountryPropID)";
                        command = new SqlCeCommand(sqlQuery, con);
                        command.Parameters.Add("@CountryPropID", SqlDbType.Int).Value = Int32.Parse(x.id);
                        rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);
                        rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;

                        //rountine for Defect Table
                        if (rowCount > 0)
                        {
                            rs.ReadFirst();
                            temp = rs.GetInt32(0);
                        }
                        if (temp == 0)
                        {
                            sqlQuery = " INSERT INTO VSD_Country_Prop (Can_Confiscate_Plates,Can_Fetch_Info,Can_Force_Vehicle_Testing,Can_Inspect,Can_Print_Viol_Notice,Can_Raise_Violation, Can_Send_Fine,Country_ID,Country_Prop_ID,IsDefault,IsDisabled) VALUES (" + (x.canConfiscatePlate != null ? "'" + x.canConfiscatePlate + "'" : "NULL") + "," + (x.canFetchInfo != null ? "'" + x.canFetchInfo + "'" : "NULL") + "," + (x.canForceVehicleTesting != null ? "'" + x.canForceVehicleTesting + "'" : "NULL") + "," + (x.canInspect != null ? "'" + x.canInspect + "'" : "NULL") + "," + (x.canPrintViolationTicket != null ? "'" + x.canPrintViolationTicket + "'" : "NULL") + "," + (x.canRaiseViolation != null ? "'" + x.canRaiseViolation + "'" : "NULL") + "," + (x.canSendFine != null ? "'" + x.canSendFine + "'" : "NULL") + "," + (x.countryId != null ? "" + x.countryId + "" : "NULL") + "," + (x.id != null ? "'" + x.id + "'" : "NULL") + "," + (x.isDefault != null ? "'" + x.isDefault + "'" : "NULL") + "," + (x.isDisabled != null ? "'" + x.isDisabled + "'" : "NULL") + ") ";
                        }
                        else
                        {
                            sqlQuery = " UPDATE VSD_Country_Prop SET Can_Confiscate_Plates = " + (x.canConfiscatePlate != null ? "'" + x.canConfiscatePlate + "'" : "NULL") + " , Can_Fetch_Info = " + (x.canFetchInfo != null ? "'" + x.canFetchInfo + "'" : "NULL") + ",Can_Force_Vehicle_Testing = " + (x.canForceVehicleTesting != null ? "'" + x.canForceVehicleTesting + "'" : "NULL") + ",Can_Inspect = " + (x.canInspect != null ? "'" + x.canInspect + "'" : "NULL") + ",Can_Print_Viol_Notice = " + (x.canPrintViolationTicket != null ? "'" + x.canPrintViolationTicket + "'" : "NULL") + ",Can_Raise_Violation = " + (x.canRaiseViolation != null ? "'" + x.canRaiseViolation + "'" : "NULL") + ", Can_Send_Fine = " + (x.canSendFine != null ? "'" + x.canSendFine + "'" : "NULL") + ",Country_ID = " + (x.canConfiscatePlate != null ? "" + x.countryId + "" : "NULL") + ",Country_Prop_ID = " + (x.id != null ? "'" + x.id + "'" : "NULL") + ",IsDefault = " + (x.isDefault != null ? "'" + x.isDefault + "'" : "NULL") + ",IsDisabled = " + (x.isDisabled != null ? "'" + x.isDisabled + "'" : "NULL") + " WHERE Country_Prop_ID = " + (x.id != null ? "'" + x.id + "'" : "NULL") + " ";
                        }

                        command = new SqlCeCommand(sqlQuery, con);

                        command.Parameters.Add("@CanConfiscatePlates", SqlDbType.NChar, 1).Value = x.canConfiscatePlate;
                        command.Parameters.Add("@CanFetchInfo", SqlDbType.NChar, 1).Value = x.canFetchInfo;
                        command.Parameters.Add("@CanForceVehicleTesting", SqlDbType.NChar, 1).Value = x.canForceVehicleTesting;
                        command.Parameters.Add("@CanInspect", SqlDbType.NChar, 1).Value = x.canInspect;
                        command.Parameters.Add("@CanPrintViolTicket", SqlDbType.NChar, 1).Value = x.canPrintViolationTicket;
                        command.Parameters.Add("@CanRaiseViolation", SqlDbType.NChar, 1).Value = x.canRaiseViolation;
                        command.Parameters.Add("@CanSendFine", SqlDbType.NChar, 1).Value = x.canSendFine;
                        command.Parameters.Add("@CountryID", SqlDbType.Int).Value = x.countryId;
                        command.Parameters.Add("@CountryPropID", SqlDbType.Int).Value = x.id;
                        command.Parameters.Add("@IsDefault", SqlDbType.NChar, 1).Value = x.isDefault;
                        command.Parameters.Add("@IsDisabled", SqlDbType.NChar, 1).Value = (x.isDisabled.ToString())[0];

                        rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);
                        rs.Close();
                        con.Close();
                    }
                    App.VSDLog.Info("\n SynchronizeConfig().Synchronize_countryProperty:" + WebServiceDefects.Length);
                }
                App.VSDLog.Info("\n SynchronizeConfig().Synchronize_countryProperty: --------------- END");
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
            }
        }
        public void Synchronize_location(handHeldService.SynchronizeConfigDataResponseItem test)
        {
            try
            {
                App.VSDLog.Info("\n SynchronizeConfig().Synchronize_location: --------------- START");
                SqlCeConnection con = ((VSDApp.com.rta.vsd.hh.db.IDBManager)VSDApp.com.rta.vsd.hh.db.DBConnectionManager.GetInstance()).GetConnection();
                string sqlQuery;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCeCommand command;
                SqlCeResultSet rs;
                int rowCount;
                int temp = -1;

                //check for location updates
                if (test.location != null)
                {
                    handHeldService.Location[] WebServiceDefects = new VSDApp.handHeldService.Location[test.location.Length];
                    WebServiceDefects = test.location;
                    foreach (handHeldService.Location x in WebServiceDefects)
                    {
                        if (con.State == ConnectionState.Closed) { con.Open(); }
                        /*
                         * IF EXISTS (SELECT * FROM VSD_Defect WHERE DefectID = @DefectID)
                            UPDATE Table1 SET (...) WHERE Column1='SomeValue'
                            ELSE
                            INSERT INTO Table1 VALUES (...)
                         */
                        sqlQuery = "SELECT COUNT(*) AS Expr1"
                                    + " FROM VSD_Location"
                                    + " WHERE (Location_ID = @LocationID)";
                        command = new SqlCeCommand(sqlQuery, con);
                        command.Parameters.Add("@LocationID", SqlDbType.Int).Value = Int32.Parse(x.id);
                        rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);
                        rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;

                        //rountine for Defect Table
                        if (rowCount > 0)
                        {
                            rs.ReadFirst();
                            temp = rs.GetInt32(0);
                        }
                        if (temp == 0)
                        {
                            sqlQuery = " INSERT INTO VSD_Location (Location_Area_Code , Location_Area_Name , Location_Area_Name_A ,Country_ID , Location_ID , IsDisabled ,Parent_Location_ID ) VALUES ( " + (x.areaCode != null ? "'" + x.areaCode + "'" : "NULL") + ",  " + (x.areaName != null ? "'" + x.areaName.Replace("'", "`") + "'" : "NULL") + ",  " + (x.areaNameArabic != null ? "'" + x.areaNameArabic.Replace("'", "`") + "'" : "NULL") + "," + (x.country.id != null ? "" + x.country.id + "" : "NULL") + ", " + (x.id != null ? "" + x.id + "" : "NULL") + ", " + (x.isDisabled != null ? "'" + x.isDisabled + "'" : "NULL") + "," + (x.parentId != null ? "" + x.parentId + "" : "NULL") + ")";
                        }
                        else
                        {
                            sqlQuery = " UPDATE VSD_Location SET Location_Area_Code = " + (x.areaCode != null ? "'" + x.areaCode + "'" : "NULL") + ", Location_Area_Name = " + (x.areaName != null ? "'" + x.areaName.Replace("'", "`") + "'" : "NULL") + ", Location_Area_Name_A = " + (x.areaNameArabic != null ? "'" + x.areaNameArabic.Replace("'", "`") + "'" : "NULL") + ",Country_ID = " + (x.country.id != null ? "" + x.country.id + "" : "NULL") + ", Location_ID = " + (x.id != null ? "" + x.id + "" : "NULL") + ", IsDisabled = " + (x.isDisabled != null ? "'" + x.isDisabled + "'" : "NULL") + ",Parent_Location_ID = " + (x.parentId != null ? "" + x.parentId + "" : "NULL") + " WHERE Location_ID = " + (x.id != null ? "" + x.id + "" : "NULL") + " ";
                        }
                        command = new SqlCeCommand(sqlQuery, con);
                        command.Parameters.Add("@LocationAreaCode", SqlDbType.NChar, 50).Value = x.areaCode;
                        command.Parameters.Add("@LocationAreaName", SqlDbType.NChar, 100).Value = x.areaName;
                        command.Parameters.Add("@LocationAreaNameArabic", SqlDbType.NChar, 200).Value = x.areaNameArabic;
                        command.Parameters.Add("@CountryID", SqlDbType.Int).Value = x.country.id;
                        command.Parameters.Add("@LocationID", SqlDbType.Int).Value = x.id;
                        command.Parameters.Add("@IsDisabled", SqlDbType.Int).Value = x.isDisabled;
                        if (x.parentId != null)
                        {
                            command.Parameters.Add("@ParentLocationID", Int32.Parse(x.parentId));
                        }
                        else
                        {
                            command.Parameters.Add("@ParentLocationID", DBNull.Value);
                        }
                        rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);
                        rs.Close();
                        con.Close();
                    }
                    App.VSDLog.Info("\n SynchronizeConfig().Synchronize_location:" + WebServiceDefects.Length);
                }
                App.VSDLog.Info("\n SynchronizeConfig().Synchronize_location: --------------- END");
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
            }
        }
        public void Synchronize_propertyType(handHeldService.SynchronizeConfigDataResponseItem test)
        {
            try
            {
                App.VSDLog.Info("\n SynchronizeConfig().Synchronize_propertyType: --------------- START");
                SqlCeConnection con = ((VSDApp.com.rta.vsd.hh.db.IDBManager)VSDApp.com.rta.vsd.hh.db.DBConnectionManager.GetInstance()).GetConnection();
                string sqlQuery;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCeCommand command;
                SqlCeResultSet rs;
                int rowCount;
                int temp = -1;

                //check for property type updates

                if (test.propertyType != null)
                {
                    handHeldService.PropertyType[] WebServiceDefects = new VSDApp.handHeldService.PropertyType[test.propertyType.Length];
                    WebServiceDefects = test.propertyType;
                    foreach (handHeldService.PropertyType x in WebServiceDefects)
                    {
                        if (con.State == ConnectionState.Closed) { con.Open(); }
                        /*
                         * IF EXISTS (SELECT * FROM VSD_Defect WHERE DefectID = @DefectID)
                            UPDATE Table1 SET (...) WHERE Column1='SomeValue'
                            ELSE
                            INSERT INTO Table1 VALUES (...)
                         */
                        sqlQuery = "SELECT COUNT(*) AS Expr1"
                                    + " FROM VSD_Property_Type"
                                    + " WHERE (Property_Type_ID = @PropertyTypeID)";
                        command = new SqlCeCommand(sqlQuery, con);
                        command.Parameters.Add("@PropertyTypeID", SqlDbType.Int).Value = Int32.Parse(x.id);

                        rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);
                        rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;

                        //rountine for Defect Table
                        if (rowCount > 0)
                        {
                            rs.ReadFirst();
                            temp = rs.GetInt32(0);
                        }
                        if (temp == 0)
                        {
                            sqlQuery = " INSERT INTO VSD_Property_Type (Property_type_id,Property_Type_Name,Property_Type_Name_A , Property_Type_Desc, Property_Type_Desc_A ,Property_Type_Code , IsDisabled ) VALUES ( " + (x.id != null ? "" + x.id + "" : "NULL") + "," + (x.name != null ? "'" + x.name.Replace("'", "`") + "'" : "NULL") + ", " + (x.nameArabic != null ? "'" + x.nameArabic.Replace("'", "`") + "'" : "NULL") + " , " + (x.description != null ? "'" + x.description.Replace("'", "`") + "'" : "NULL") + "," + (x.descriptionArabic != null ? "'" + x.descriptionArabic.Replace("'", "`") + "'" : "NULL") + " ," + (x.code != null ? "'" + x.code + "'" : "NULL") + ", " + (x.isDisabled != null ? "'" + x.isDisabled + "'" : "NULL") + ")";
                        }
                        else
                        {
                            sqlQuery = " UPDATE VSD_Property_Type SET Property_Type_Name = " + (x.name != null ? "'" + x.name.Replace("'", "`") + "'" : "NULL") + ",Property_Type_Name_A = " + (x.nameArabic != null ? "'" + x.nameArabic.Replace("'", "`") + "'" : "NULL") + " , Property_Type_Desc = " + (x.description != null ? "'" + x.description.Replace("'", "`") + "'" : "NULL") + ", Property_Type_Desc_A = " + (x.descriptionArabic != null ? "'" + x.descriptionArabic.Replace("'", "`") + "'" : "NULL") + " ,Property_Type_Code = " + (x.code != null ? "'" + x.code + "'" : "NULL") + ", IsDisabled = " + (x.isDisabled != null ? "'" + x.isDisabled + "'" : "NULL") + " WHERE Property_Type_ID = " + (x.id != null ? "" + x.id + "" : "NULL") + " ";
                        }

                        command = new SqlCeCommand(sqlQuery, con);
                        // command.Parameters.Add("@PropertyTypeDescription", SqlDbType.Int).Value = x.description;
                        command.Parameters.Add("@PropertyTypeName", SqlDbType.NChar, 100).Value = x.name;
                        command.Parameters.Add("@PropertyTypeNameArabic", SqlDbType.NChar, 100).Value = x.nameArabic;
                        command.Parameters.Add("@PropertyTypeDescription", SqlDbType.NChar, 250).Value = x.description;
                        command.Parameters.Add("@PropertyTypeDescriptionArabic", SqlDbType.NChar, 250).Value = x.descriptionArabic;
                        command.Parameters.Add("@PropertyTypeCode", SqlDbType.NChar, 100).Value = x.code;
                        command.Parameters.Add("@IsDisabled", SqlDbType.NChar, 1).Value = ((x.isDisabled).ToString())[0];

                        rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);
                        rs.Close();
                        con.Close();
                    }
                    App.VSDLog.Info("\n SynchronizeConfig().Synchronize_propertyType:" + WebServiceDefects.Length);
                }
                App.VSDLog.Info("\n SynchronizeConfig().Synchronize_propertyType: --------------- START");

            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
            }
        }
        public void Synchronize_propertyValue(handHeldService.SynchronizeConfigDataResponseItem test)
        {
            try
            {
                App.VSDLog.Info("\n SynchronizeConfig().Synchronize_propertyValue: --------------- START");
                SqlCeConnection con = ((VSDApp.com.rta.vsd.hh.db.IDBManager)VSDApp.com.rta.vsd.hh.db.DBConnectionManager.GetInstance()).GetConnection();
                string sqlQuery;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCeCommand command;
                SqlCeResultSet rs;
                int rowCount;
                int temp = -1;

                //check for property value updates
                if (test.propertyValue != null)
                {
                    handHeldService.PropertyValue[] WebServiceDefects = new VSDApp.handHeldService.PropertyValue[test.propertyValue.Length];
                    WebServiceDefects = test.propertyValue;
                    foreach (handHeldService.PropertyValue x in WebServiceDefects)
                    {
                        if (con.State == ConnectionState.Closed) { con.Open(); }
                        /*
                         * IF EXISTS (SELECT * FROM VSD_Defect WHERE DefectID = @DefectID)
                            UPDATE Table1 SET (...) WHERE Column1='SomeValue'
                            ELSE
                            INSERT INTO Table1 VALUES (...)
                         */
                        sqlQuery = "SELECT COUNT(*) AS Expr1"
                                    + " FROM VSD_Property_Value"
                                    + " WHERE (Property_Type_Value_ID = @PropertyValueID)";
                        command = new SqlCeCommand(sqlQuery, con);
                        command.Parameters.Add("@PropertyValueID", SqlDbType.Int).Value = Int32.Parse(x.id);
                        rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);
                        rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;

                        //rountine for Defect Table
                        if (rowCount > 0)
                        {
                            rs.ReadFirst();
                            temp = rs.GetInt32(0);
                        }
                        if (temp == 0)
                        {
                            sqlQuery = " INSERT INTO VSD_Property_Value (Property_Type_Value_ID ,Property_Type_ID,Property_Type_Name,Property_Type_Value,IsDisabled) VALUES (" + x.id + " ," + (x.propertyTypeId != null ? "" + x.propertyTypeId + "" : "NULL") + "," + (x.valueName != null ? "'" + x.valueName.Replace("'", "`") + "'" : "NULL") + "," + (x.value != null ? "'" + x.value.Replace("'", "`") + "'" : "NULL") + "," + (x.isDisabled != null ? "'" + x.isDisabled + "'" : "NULL") + ")";
                        }
                        else
                        {
                            sqlQuery = " UPDATE VSD_Property_Value SET Property_Type_Value_ID = " + (x.id != null ? "" + x.id + "" : "NULL") + " ,Property_Type_ID = " + (x.propertyTypeId != null ? "" + x.propertyTypeId + "" : "NULL") + ", Property_Type_Name = " + (x.valueName != null ? "'" + x.valueName.Replace("'", "`") + "'" : "NULL") + " ,Property_Type_Value = " + (x.value != null ? "'" + x.value.Replace("'", "`") + "'" : "NULL") + ",IsDisabled = " + (x.isDisabled != null ? "'" + x.isDisabled + "'" : "NULL") + " WHERE PROPERTY_TYPE_VALUE_ID = " + (x.id != null ? "" + x.id + "" : "NULL") + " ";
                        }
                        command = new SqlCeCommand(sqlQuery, con);
                        command.Parameters.Add("@PropertyTypeValueID", SqlDbType.Int).Value = x.id;
                        command.Parameters.Add("@PropertyTypeID", SqlDbType.Int).Value = x.propertyTypeId;
                        command.Parameters.Add("@PropertyTypeName", SqlDbType.NChar, 50).Value = x.valueName;
                        command.Parameters.Add("@PropertyTypeValue", SqlDbType.NChar, 50).Value = x.value;
                        command.Parameters.Add("@IsDisabled", SqlDbType.NChar, 1).Value = x.isDisabled;

                        rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);
                        rs.Close();
                        con.Close();
                    }
                    App.VSDLog.Info("\n SynchronizeConfig().Synchronize_propertyValue:" + WebServiceDefects.Length);
                }
                App.VSDLog.Info("\n SynchronizeConfig().Synchronize_propertyValue: --------------- END");
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
            }
        }
        public void Synchronize_severityLevel(handHeldService.SynchronizeConfigDataResponseItem test)
        {
            try
            {
                App.VSDLog.Info("\n SynchronizeConfig().Synchronize_severityLevel: --------------- START");
                SqlCeConnection con = ((VSDApp.com.rta.vsd.hh.db.IDBManager)VSDApp.com.rta.vsd.hh.db.DBConnectionManager.GetInstance()).GetConnection();
                string sqlQuery;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCeCommand command;
                SqlCeResultSet rs;
                int rowCount;
                int temp = -1;


                //check for severity value updates
                if (test.severityLevel != null)
                {
                    handHeldService.SeverityLevel1[] WebServiceDefects = new VSDApp.handHeldService.SeverityLevel1[test.severityLevel.Length];
                    WebServiceDefects = test.severityLevel;
                    foreach (handHeldService.SeverityLevel1 x in WebServiceDefects)
                    {
                        if (con.State == ConnectionState.Closed) { con.Open(); }
                        /*
                         * IF EXISTS (SELECT * FROM VSD_Defect WHERE DefectID = @DefectID)
                            UPDATE Table1 SET (...) WHERE Column1='SomeValue'
                            ELSE
                            INSERT INTO Table1 VALUES (...)
                         */
                        sqlQuery = "SELECT COUNT(*) AS Expr1"
                                    + " FROM VSD_Severity_Level"
                                    + " WHERE (Severity_Level_ID = " + x.id + ")";
                        command = new SqlCeCommand(sqlQuery, con);
                        rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);
                        rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;

                        //rountine for Defect Table
                        if (rowCount > 0)
                        {
                            rs.ReadFirst();
                            temp = rs.GetInt32(0);
                        }
                        if (temp == 0)
                        {
                            sqlQuery = " INSERT INTO VSD_Severity_Level (Severity_Level_ID,Severity_Level,Severity_Level_Code,Severity_Level_Code_Prefix,Severity_Level_Name,Severity_Level_Name_A,Severity_Level_Desc,Severity_Level_Desc_A,Timestamp,IsDisabled) VALUES (" + x.id + "," + (x.value != null ? "'" + x.value.Replace("'", "`") + "'" : "NULL") + "," + (x.code != null ? "'" + x.code + "'" : "NULL") + "," + (x.codePrefix != null ? "'" + x.codePrefix + "'" : "NULL") + "," + (x.name != null ? "'" + x.name.Replace("'", "`") + "'" : "NULL") + "," + (x.nameArabic != null ? "'" + x.nameArabic.Replace("'", "`") + "'" : "NULL") + "," + (x.description != null ? "'" + x.description.Replace("'", "`") + "'" : "NULL") + "," + (x.descriptionArabic != null ? "'" + x.descriptionArabic.Replace("'", "`") + "'" : "NULL") + ")";
                        }
                        else
                        {
                            sqlQuery = " UPDATE VSD_Severity_Level SET Severity_Level_ID = " + x.id + ", Severity_Level = " + (x.value != null ? "'" + x.value.Replace("'", "`") + "'" : "NULL") + ", Severity_Level_Code = " + (x.code != null ? "'" + x.code + "'" : "NULL") + ",Severity_Level_Code_Prefix = " + (x.codePrefix != null ? "'" + x.codePrefix + "'" : "NULL") + ",Severity_Level_Name = " + (x.name != null ? "'" + x.name.Replace("'", "`") + "'" : "NULL") + ",Severity_Level_Name_A = " + (x.nameArabic != null ? "'" + x.nameArabic.Replace("'", "`") + "'" : "NULL") + ",Severity_Level_Desc = " + (x.description != null ? "'" + x.description.Replace("'", "`") + "'" : "NULL") + ",Severity_Level_Desc_A = " + (x.descriptionArabic != null ? "'" + x.descriptionArabic.Replace("'", "`") + "'" : "NULL") + " WHERE Severity_Level_ID = " + x.id;
                        }

                        command = new SqlCeCommand(sqlQuery, con);

                        command.Parameters.Add("@SeverityLevelID", SqlDbType.Int).Value = x.id;
                        command.Parameters.Add("@SeverityLevel", SqlDbType.Int).Value = x.value;
                        command.Parameters.Add("@SeverityLevelCode", SqlDbType.NChar, 25).Value = x.code;
                        command.Parameters.Add("@SeverityLevelCodePrefix", SqlDbType.NChar, 25).Value = x.codePrefix;
                        command.Parameters.Add("@SeverityLevelName", SqlDbType.NChar, 100).Value = x.name;
                        command.Parameters.Add("@SeverityLevelNameArabic", SqlDbType.NChar, 100).Value = x.nameArabic;
                        command.Parameters.Add("@SeverityLevelDesc", SqlDbType.NChar, 250).Value = x.description;
                        command.Parameters.Add("@SeverityLevelDescA", SqlDbType.NChar, 250).Value = x.descriptionArabic;

                        rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);
                        rs.Close();
                        con.Close();
                    }
                    App.VSDLog.Info("\n SynchronizeConfig().Synchronize_severityLevel:" + WebServiceDefects.Length);
                }
                App.VSDLog.Info("\n SynchronizeConfig().Synchronize_severityLevel: --------------- END");
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
            }
        }
        public void Synchronize_severityLevelProperty(handHeldService.SynchronizeConfigDataResponseItem test)
        {
            try
            {
                App.VSDLog.Info("\n SynchronizeConfig().Synchronize_severityLevelProperty: --------------- START");
                SqlCeConnection con = ((VSDApp.com.rta.vsd.hh.db.IDBManager)VSDApp.com.rta.vsd.hh.db.DBConnectionManager.GetInstance()).GetConnection();
                string sqlQuery;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCeCommand command;
                SqlCeResultSet rs;
                int rowCount;
                int temp = -1;

                //check for severity level property updates
                if (test.severityLevelProperty != null)
                {
                    handHeldService.SeverityLevelProperties[] WebServiceDefects = new VSDApp.handHeldService.SeverityLevelProperties[test.severityLevelProperty.Length];
                    WebServiceDefects = test.severityLevelProperty;
                    foreach (handHeldService.SeverityLevelProperties x in WebServiceDefects)
                    {
                        if (con.State == ConnectionState.Closed) { con.Open(); }
                        /*
                         * IF EXISTS (SELECT * FROM VSD_Defect WHERE DefectID = @DefectID)
                            UPDATE Table1 SET (...) WHERE Column1='SomeValue'
                            ELSE
                            INSERT INTO Table1 VALUES (...)
                         */
                        sqlQuery = "SELECT COUNT(*) AS Expr1"
                                    + " FROM VSD_Severity_Level_Prop"
                                    + " WHERE (Severity_Level_Prop_ID = @SeverityLevelPropID)";
                        command = new SqlCeCommand(sqlQuery, con);
                        command.Parameters.Add("@SeverityLevelPropID", SqlDbType.Int).Value = x.id;
                        rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);
                        rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;
                        int SeverityLevelPropID = x.id;
                        //rountine for Defect Table
                        if (rowCount > 0)
                        {
                            rs.ReadFirst();
                            temp = rs.GetInt32(0);
                        }
                        if (temp == 0)
                        {
                            sqlQuery = " INSERT INTO VSD_Severity_Level_Prop (Severity_Level_Prop_ID,Severity_Level_ID,Min_No_Of_Defects,Max_No_Of_Defects,Due_Days,Vehicle_Service_Suspension_Days,Comp_Service_Suspension_Days,Req_Plate_Confiscation,Receipt_Title,Receipt_Title_A,IsDisabled) VALUES (" + x.id + "," + (x.severityId != null ? "" + x.severityId + "" : "NULL") + "," + (x.minDefects != null ? "" + x.minDefects + "" : "NULL") + "," + (x.maxDefects != null ? "" + x.maxDefects + "" : "NULL") + "," + (x.dueDays != null ? "" + x.dueDays + "" : "NULL") + "," + (x.vehicleServiceSuspendDays != null ? "" + x.vehicleServiceSuspendDays + "" : "NULL") + "," + (x.companyServiceSuspenseDays != null ? "" + x.companyServiceSuspenseDays + "" : "NULL") + "," + (x.requirePlateConfiscation != null ? "'" + x.requirePlateConfiscation + "'" : "NULL") + "," + (x.receiptTitle != null ? "'" + x.receiptTitle.Replace("'", "`") + "'" : "NULL") + "," + (x.receiptTitleArabic != null ? "'" + x.receiptTitleArabic.Replace("'", "`") + "'" : "NULL") + "," + (x.isDisabled != null ? "'" + x.isDisabled + "'" : "NULL") + ")";
                        }
                        else
                        {
                            sqlQuery = " UPDATE VSD_Severity_Level_Prop SET Severity_Level_Prop_ID = " + x.id + ",Severity_Level_ID = " + (x.severityId != null ? "" + x.severityId + "" : "NULL") + ",Min_No_Of_Defects = " + (x.minDefects != null ? "" + x.minDefects + "" : "NULL") + ",Max_No_Of_Defects = " + (x.maxDefects != null ? "" + x.maxDefects + "" : "NULL") + ",Due_Days = " + (x.dueDays != null ? "" + x.dueDays + "" : "NULL") + ",Vehicle_Service_Suspension_Days = " + (x.vehicleServiceSuspendDays != null ? "" + x.vehicleServiceSuspendDays + "" : "NULL") + ",Comp_Service_Suspension_Days = " + (x.companyServiceSuspenseDays != null ? "" + x.companyServiceSuspenseDays + "" : "NULL") + ",Req_Plate_Confiscation = " + (x.requirePlateConfiscation != null ? "'" + x.requirePlateConfiscation + "'" : "NULL") + ",Receipt_Title = " + (x.receiptTitle != null ? "'" + x.receiptTitle.Replace("'", "`") + "'" : "NULL") + ",Receipt_Title_A = " + (x.receiptTitleArabic != null ? "'" + x.receiptTitleArabic.Replace("'", "`") + "'" : "NULL") + ",IsDisabled = " + (x.isDisabled != null ? "'" + x.isDisabled + "'" : "NULL") + " WHERE Severity_Level_Prop_ID = " + x.id;
                        }

                        // command.Parameters.Add("@SeverityLevelPropID", SqlDbType.Int).Value = SeverityLevelPropID;
                        command.Parameters.Add("@SeverityLevelID", SqlDbType.Int).Value = SeverityLevelPropID;
                        command.Parameters.Add("@MinNoOfDefects", SqlDbType.Int).Value = x.minDefects;
                        command.Parameters.Add("@MaxNoOfDefects", SqlDbType.Int).Value = x.maxDefects;
                        command.Parameters.Add("@CompServiceSuspensionDays", SqlDbType.Int).Value = x.companyServiceSuspenseDays;
                        command.Parameters.Add("@ReqPlateConfiscation", SqlDbType.NChar, 1).Value = (null == x.requirePlateConfiscation) ? "F" : "T";
                        command.Parameters.Add("@VehRegBlockDays", SqlDbType.Int).Value = x.vehicleServiceSuspendDays;
                        command.Parameters.Add("@ReceiptTitle", SqlDbType.NChar, 100).Value = x.receiptTitle;
                        command.Parameters.Add("@ReceiptTitleA", SqlDbType.NChar, 100).Value = x.receiptTitleArabic;
                        command.Parameters.Add("@IsDisabled", SqlDbType.Int).Value = ("F" == x.isDisabled) ? "F" : "T";
                        command.Parameters.Add("@ConfiscationDays", SqlDbType.Int).Value = x.plateConfiscationDays;
                        command = new SqlCeCommand(sqlQuery, con);
                        rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);
                        rs.Close();
                        con.Close();
                    }
                    App.VSDLog.Info("\n SynchronizeConfig().Synchronize_severityLevelProperty:" + WebServiceDefects.Length);

                }
                App.VSDLog.Info("\n SynchronizeConfig().Synchronize_severityLevelProperty: --------------- END");
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);

            }
        }
        public void Synchronize_vehicleCategory(handHeldService.SynchronizeConfigDataResponseItem test)
        {
            try
            {
                App.VSDLog.Info("\n SynchronizeConfig().Synchronize_vehicleCategory: --------------- START");
                SqlCeConnection con = ((VSDApp.com.rta.vsd.hh.db.IDBManager)VSDApp.com.rta.vsd.hh.db.DBConnectionManager.GetInstance()).GetConnection();
                string sqlQuery;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCeCommand command;
                SqlCeResultSet rs;
                int rowCount;
                int temp = -1;



                //check for Vehicle Category Defect updates
                if (test.vehicleCategory != null)
                {

                    handHeldService.VehicleCategory1[] WebServiceDefects = new VSDApp.handHeldService.VehicleCategory1[test.vehicleCategory.Length];
                    WebServiceDefects = test.vehicleCategory;
                    foreach (handHeldService.VehicleCategory1 x in WebServiceDefects)
                    {
                        if (con.State == ConnectionState.Closed) { con.Open(); }
                        /*
                         * IF EXISTS (SELECT * FROM VSD_Defect WHERE DefectID = @DefectID)
                            UPDATE Table1 SET (...) WHERE Column1='SomeValue'
                            ELSE
                            INSERT INTO Table1 VALUES (...)
                         */

                        sqlQuery = "SELECT Vehicle_Category_ID FROM VSD_Vehicle_Category ORDER BY Vehicle_Category_ID DESC";
                        command = new SqlCeCommand(sqlQuery, con);

                        SqlCeResultSet res = command.ExecuteResultSet(ResultSetOptions.Scrollable);
                        int vInfoID = -1;
                        if (res.HasRows)
                        {

                            res.Read();
                            vInfoID = Convert.ToInt32(res["VEHICLE_CATEGORY_ID"]) + 1;
                            res.Close();
                        }


                        sqlQuery = "Select vehicle_category_id from vsd_vehicle_category where category_code = '" + x.code + "'";
                        command = new SqlCeCommand(sqlQuery, con);
                        SqlCeDataReader tempReader = command.ExecuteReader();
                        if (tempReader.Read())
                        {
                            vInfoID = Int32.Parse(tempReader[0].ToString());

                        }
                        if (vInfoID == -1)
                        {
                            vInfoID = 1;
                        }

                        sqlQuery = "Select Vehicle_Category_ID from vsd_vehicle_category where category_code ='" + x.parentCategoryCode + "'";
                        command = new SqlCeCommand(sqlQuery, con);
                        tempReader = command.ExecuteReader();
                        string parentCatCode;
                        if (tempReader.Read())
                        {
                            parentCatCode = tempReader[0].ToString();
                        }
                        else
                        {
                            parentCatCode = null;
                        }
                        sqlQuery = "SELECT COUNT(*) AS Expr1"
                        + " FROM VSD_Vehicle_Category"
                        + " WHERE (Vehicle_Category_ID = @Vehicle_Category_ID)";
                        command = new SqlCeCommand(sqlQuery, con);
                        command.Parameters.Add("@Vehicle_Category_ID", SqlDbType.Int).Value = vInfoID;
                        rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);
                        rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;






                        //rountine for Defect Table
                        if (rowCount > 0)
                        {
                            rs.ReadFirst();
                            temp = rs.GetInt32(0);
                        }
                        if (temp == 0)
                        {
                            sqlQuery = " INSERT INTO VSD_Vehicle_Category ( Vehicle_Category_ID,Parent_Vehicle_Category_ID,Record_Type,Category_Code,Category_Name,Category_Name_A,Category_Desc,Category_Desc_A,IsDisabled ) VALUES ( " + vInfoID.ToString() + "," + (parentCatCode != null ? "'" + parentCatCode + "'" : "NULL") + "," + (x.recordType != null ? "'" + x.recordType + "'" : "NULL") + "," + (x.code != null ? "'" + x.code + "'" : "NULL") + "," + (x.name != null ? "'" + x.name.Replace("'", "`") + "'" : "NULL") + "," + (x.nameArabic != null ? "'" + x.nameArabic.Replace("'", "`") + "'" : "NULL") + "," + (x.description != null ? "'" + x.description.Replace("'", "`") + "'" : "NULL") + "," + (x.descriptionArabic != null ? "'" + x.descriptionArabic.Replace("'", "`") + "'" : "NULL") + "," + (x.isDisabled != null ? "'" + x.isDisabled + "'" : "NULL") + " )";
                        }
                        else
                        {
                            sqlQuery = " UPDATE VSD_Vehicle_Category SET Vehicle_Category_ID = " + vInfoID.ToString() + ",Parent_Vehicle_Category_ID = " + (parentCatCode != null ? "'" + parentCatCode + "'" : "NULL") + ",Record_Type = " + (x.recordType != null ? "'" + x.recordType + "'" : "NULL") + ",Category_Code = " + (x.code != null ? "'" + x.code + "'" : "NULL") + ",Category_Name = " + (x.name != null ? "'" + x.name.Replace("'", "`") + "'" : "NULL") + ",Category_Name_A = " + (x.nameArabic != null ? "'" + x.nameArabic.Replace("'", "`") + "'" : "NULL") + ",Category_Desc = " + (x.description != null ? "'" + x.description.Replace("'", "`") + "'" : "NULL") + ",Category_Desc_A = " + (x.descriptionArabic != null ? "'" + x.descriptionArabic.Replace("'", "`") + "'" : "NULL") + ",IsDisabled = " + (x.isDisabled != null ? "'" + x.isDisabled + "'" : "NULL") + "  WHERE Vehicle_Category_ID =" + vInfoID.ToString();
                        }
                        command = new SqlCeCommand(sqlQuery, con);
                        command.Parameters.Add("@Vehicle_Category_ID", SqlDbType.Int).Value = vInfoID;
                        if (x.parentCategoryCode != null)
                        {
                            command.Parameters.Add("@Parent_Vehicle_Category_ID", Int32.Parse(x.parentCategoryCode));
                        }
                        else
                        {
                            command.Parameters.Add("@Parent_Vehicle_Category_ID", DBNull.Value);
                        }
                        command.Parameters.Add("@Record_Type", SqlDbType.NChar, 25).Value = x.recordType;
                        command.Parameters.Add("@Category_Code", SqlDbType.NChar, 25).Value = x.code;
                        command.Parameters.Add("@Category_Name", SqlDbType.NChar, 100).Value = x.name;
                        command.Parameters.Add("@Category_Name_A", SqlDbType.NChar, 100).Value = x.nameArabic;
                        command.Parameters.Add("@Category_Desc", SqlDbType.NChar, 250).Value = x.description;
                        command.Parameters.Add("@Category_Desc_A", SqlDbType.NChar, 250).Value = x.descriptionArabic;
                        command.Parameters.Add("@IsDisabled", SqlDbType.NChar, 1).Value = (x.isDisabled.ToString())[0];
                        rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);
                        rs.Close();




                        con.Close();
                    }
                    App.VSDLog.Info("\n SynchronizeConfig().Synchronize_vehicleCategory:" + WebServiceDefects.Length);
                }
                App.VSDLog.Info("\n SynchronizeConfig().Synchronize_vehicleCategory: --------------- END");
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
            }
        }
        public void Synchronize_vehicle_Plate_Cat_Code(handHeldService.SynchronizeConfigDataResponseItem test)
        {
            try
            {
                App.VSDLog.Info("\n SynchronizeConfig().Synchronize_Vehicle_Plate_Cat_Code: --------------- START");
                SqlCeConnection con = ((VSDApp.com.rta.vsd.hh.db.IDBManager)VSDApp.com.rta.vsd.hh.db.DBConnectionManager.GetInstance()).GetConnection();
                string sqlQuery;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCeCommand command;
                SqlCeResultSet rs;
                int rowCount;
                int temp = -1;



                //check for Vehicle Category Defect updates
                if (test.vehicleCategory != null)
                {

                    handHeldService.VehPlateCatCode[] WebServiceDefects = new VSDApp.handHeldService.VehPlateCatCode[test.platCodes.Length];
                    WebServiceDefects = test.platCodes;
                    foreach (handHeldService.VehPlateCatCode x in WebServiceDefects)
                    {
                        if (con.State == ConnectionState.Closed) { con.Open(); }
                        ///////////////////////






                        /*
                         * IF EXISTS (SELECT * FROM VSD_Defect WHERE DefectID = @DefectID)
                            UPDATE Table1 SET (...) WHERE Column1='SomeValue'
                            ELSE
                            INSERT INTO Table1 VALUES (...)
                         * VSD_Veh_Plate_Code_Cat
                         * Veh_Plate_Code_Cat_ID
                         */


                        sqlQuery = "SELECT COUNT(*) AS Expr1"
                        + " FROM VSD_Veh_Plate_Code_Cat"
                        + " WHERE (Veh_Plate_Code_Cat_ID = @Veh_Plate_Code_Cat_ID)";
                        command = new SqlCeCommand(sqlQuery, con);
                        command.Parameters.Add("@Veh_Plate_Code_Cat_ID", SqlDbType.Int).Value = x.vehPlateCatCodeId;
                        rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);
                        rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;




                        string ParentCatCode = string.Empty;
                        if (x.parentVehPlateCatCodeId.Equals(0))
                        {
                            ParentCatCode = "Null";
                        }
                        else
                        {
                            ParentCatCode = x.parentVehPlateCatCodeId.ToString();
                        }

                        //rountine for Defect Table
                        if (rowCount > 0)
                        {
                            rs.ReadFirst();
                            temp = rs.GetInt32(0);
                        }
                        if (temp == 0)
                        {
                            // sqlQuery = " INSERT INTO VSD_Veh_Plate_Code_Cat ( Veh_Plate_Code_Cat_ID,Parent_Veh_Plate_Code_ID,Country_ID,Record_Type,Category_Name,Category_Name_A,IsDisabled,TimeStamp ) VALUES ( " + x.vehPlateCatCodeId.ToString() + "," + (((x.parentVehPlateCatCodeId != null) || x.parentVehPlateCatCodeId.Equals(0)) ? "'" + x.parentVehPlateCatCodeId + "'" : "NULL") + "," + (x.countryId != null ? "'" + x.countryId + "'" : "NULL") + "," + (x.recordType != null ? "'" + x.recordType + "'" : "NULL") + "," + (x.categoryName != null ? "'" + x.categoryName + "'" : "NULL") + "," + (x.categoryNameA != null ? "'" + x.categoryNameA + "'" : "NULL") + "," + (x.isDisabled != null ? "'" + x.isDisabled + "'" : "NULL") + "," + ("'" + DateTime.Now + "'") + " )";
                            sqlQuery = " INSERT INTO VSD_Veh_Plate_Code_Cat ( Veh_Plate_Code_Cat_ID,Parent_Veh_Plate_Code_ID,Country_ID,Record_Type,Category_Name,Category_Name_A,IsDisabled,TimeStamp ) VALUES ( " + x.vehPlateCatCodeId.ToString() + "," + (((x.parentVehPlateCatCodeId != null) || x.parentVehPlateCatCodeId.Equals(0)) ? "" + ParentCatCode + "" : "NULL") + "," + (x.countryId != null ? "'" + x.countryId + "'" : "NULL") + "," + (x.recordType != null ? "'" + x.recordType + "'" : "NULL") + "," + (x.categoryName != null ? "'" + x.categoryName + "'" : "NULL") + "," + (x.categoryNameA != null ? "'" + x.categoryNameA + "'" : "NULL") + "," + (x.isDisabled != null ? "'" + x.isDisabled + "'" : "NULL") + "," + ("'" + DateTime.Now + "'") + " )";
                        }
                        else
                        {
                            // sqlQuery = " UPDATE VSD_Veh_Plate_Code_Cat SET Veh_Plate_Code_Cat_ID = " + x.vehPlateCatCodeId.ToString() + ",Parent_Veh_Plate_Code_ID = " + (((x.parentVehPlateCatCodeId != null)||x.parentVehPlateCatCodeId.Equals(0) ) ? "'" + x.parentVehPlateCatCodeId + "'" : "NULL") + ",Country_ID = " + (x.countryId != null ? "'" + x.countryId + "'" : "NULL") + ",Record_Type = " + (x.recordType != null ? "'" + x.recordType + "'" : "NULL") + ",Category_Name = " + (x.categoryName != null ? "'" + x.categoryName + "'" : "NULL") + ",Category_Name_A = " + (x.categoryNameA != null ? "'" + x.categoryNameA + "'" : "NULL") + ",IsDisabled = " + (x.isDisabled != null ? "'" + x.isDisabled + "'" : "NULL") + ",Timestamp = " + ("'" + DateTime.Now + "'") + "  WHERE Veh_Plate_Code_Cat_ID =" + x.vehPlateCatCodeId.ToString();
                            sqlQuery = " UPDATE VSD_Veh_Plate_Code_Cat SET Veh_Plate_Code_Cat_ID = " + x.vehPlateCatCodeId.ToString() + ",Parent_Veh_Plate_Code_ID = " + (((x.parentVehPlateCatCodeId != null) || x.parentVehPlateCatCodeId.Equals(0)) ? "" + ParentCatCode + "" : "NULL") + ",Country_ID = " + (x.countryId != null ? "'" + x.countryId + "'" : "NULL") + ",Record_Type = " + (x.recordType != null ? "'" + x.recordType + "'" : "NULL") + ",Category_Name = " + (x.categoryName != null ? "'" + x.categoryName + "'" : "NULL") + ",Category_Name_A = " + (x.categoryNameA != null ? "'" + x.categoryNameA + "'" : "NULL") + ",IsDisabled = " + (x.isDisabled != null ? "'" + x.isDisabled + "'" : "NULL") + ",Timestamp = " + ("'" + DateTime.Now + "'") + "  WHERE Veh_Plate_Code_Cat_ID =" + x.vehPlateCatCodeId.ToString();
                        }
                        App.VSDLog.Info("Created Query:" + sqlQuery);
                        command = new SqlCeCommand(sqlQuery, con);
                        rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);
                        rs.Close();




                        con.Close();
                    }
                    App.VSDLog.Info("\n SynchronizeConfig().Synchronize_vehicle_pate_cat_code:" + WebServiceDefects.Length);
                }
                App.VSDLog.Info("\n SynchronizeConfig().Synchronize_vehicle_pate_cat_code: --------------- END");
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.Message);
                App.VSDLog.Info(ex.StackTrace);
            }
        }
        public void Synchronize_vehicleCatDefectSeverity(handHeldService.SynchronizeConfigDataResponseItem test)
        {
            try
            {
                App.VSDLog.Info("\n SynchronizeConfig().Synchronize_vehicleCatDefectSeverity: --------------- START");
                SqlCeConnection con = ((VSDApp.com.rta.vsd.hh.db.IDBManager)VSDApp.com.rta.vsd.hh.db.DBConnectionManager.GetInstance()).GetConnection();
                string sqlQuery;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCeCommand command;
                SqlCeResultSet rs;
                int rowCount;
                int temp = -1;
                ///////////////////////////
                //check for Vehicle Category Defect updates
                if (test.vehicleCatDefectSeverity != null)
                {
                    handHeldService.VehicleCategoryDefectSeverity[] WebServiceDefects = new VSDApp.handHeldService.VehicleCategoryDefectSeverity[test.vehicleCatDefectSeverity.Length];
                    WebServiceDefects = test.vehicleCatDefectSeverity;
                    foreach (handHeldService.VehicleCategoryDefectSeverity x in WebServiceDefects)
                    {
                        if (con.State == ConnectionState.Closed) { con.Open(); }
                        /*
                         * IF EXISTS (SELECT * FROM VSD_Defect WHERE DefectID = @DefectID)
                            UPDATE Table1 SET (...) WHERE Column1='SomeValue'
                            ELSE
                            INSERT INTO Table1 VALUES (...)
                         */
                        sqlQuery = "SELECT COUNT(*) AS Expr1"
                                    + " FROM VSD_Veh_Cat_Defect"
                                    + " WHERE (Veh_Cat_Defect_id = @VehCatDefectID)";
                        command = new SqlCeCommand(sqlQuery, con);
                        command.Parameters.Add("@VehCatDefectID", SqlDbType.Int).Value = x.id;
                        rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);
                        rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;



                        //rountine for Defect Table
                        if (rowCount > 0)
                        {
                            rs.ReadFirst();
                            temp = rs.GetInt32(0);
                        }
                        if (temp == 0)
                        {
                            sqlQuery = " INSERT INTO VSD_Veh_Cat_Defect (Veh_Cat_Defect_ID,Defect_ID,Severity_Level_ID,Vehicle_category_ID,IsDisabled) VALUES (" + x.id + "," + (x.defectId != null ? "" + x.defectId + "" : "NULL") + "," + (x.severityLevelId != null ? "" + x.severityLevelId + "" : "NULL") + "," + (x.vehicleCategoryId != null ? "" + x.vehicleCategoryId + "" : "NULL") + "," + (x.isDisabled != null ? "'" + x.isDisabled + "'" : "NULL") + ")";
                        }
                        else
                        {
                            sqlQuery = " UPDATE VSD_Veh_Cat_Defect SET Veh_Cat_Defect_ID = " + x.id + ",Defect_ID = " + (x.defectId != null ? "" + x.defectId + "" : "NULL") + ",Severity_Level_ID = " + (x.severityLevelId != null ? "" + x.severityLevelId + "" : "NULL") + ",Vehicle_category_ID = " + (x.vehicleCategoryId != null ? "" + x.vehicleCategoryId + "" : "NULL") + ",IsDisabled = " + (x.isDisabled != null ? "'" + x.isDisabled + "'" : "NULL") + " WHERE Veh_Cat_Defect_ID = " + x.id;
                        }
                        command = new SqlCeCommand(sqlQuery, con);

                        command.Parameters.Add("@Veh_Cat_Defect_ID", SqlDbType.Int).Value = x.id;
                        command.Parameters.Add("@Defect_ID", SqlDbType.Int).Value = x.defectId;
                        command.Parameters.Add("@Severity_Level_ID", SqlDbType.Int).Value = x.severityLevelId;
                        command.Parameters.Add("@Vehicle_category_ID", SqlDbType.Int).Value = x.vehicleCategoryId;
                        command.Parameters.Add("@IsDisabled", SqlDbType.NChar, 1).Value = (x.isDisabled.ToString())[0];
                        try
                        {
                            rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);

                        }
                        catch (Exception ex)
                        {
                            App.VSDLog.Info("\n SynchronizeConfig().Synchronize_vehicleCatDefectSeverity: --------------- Exception sql" + sqlQuery);

                            // throw;
                        }
                        rs.Close();
                        con.Close();
                    }
                    App.VSDLog.Info("\n SynchronizeConfig().Synchronize_vehicleCatDefectSeverity: " + WebServiceDefects.Length);
                }
                App.VSDLog.Info("\n SynchronizeConfig().Synchronize_vehicleCatDefectSeverity: --------------- END");
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
            }
        }
        public void Synchronize_testType(handHeldService.SynchronizeConfigDataResponseItem test)
        {
            try
            {
                App.VSDLog.Info("\n SynchronizeConfig().Synchronize_testType: --------------- START");
                SqlCeConnection con = ((VSDApp.com.rta.vsd.hh.db.IDBManager)VSDApp.com.rta.vsd.hh.db.DBConnectionManager.GetInstance()).GetConnection();
                string sqlQuery;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCeCommand command;
                SqlCeResultSet rs;
                int rowCount;
                int temp = -1;


                //check for test type updates
                if (test.testType != null)
                {
                    handHeldService.TestType[] WebServiceDefects = new VSDApp.handHeldService.TestType[test.testType.Length];
                    WebServiceDefects = test.testType;
                    foreach (handHeldService.TestType x in WebServiceDefects)
                    {
                        if (con.State == ConnectionState.Closed) { con.Open(); }
                        /*
                         * IF EXISTS (SELECT * FROM VSD_Defect WHERE DefectID = @DefectID)
                            UPDATE Table1 SET (...) WHERE Column1='SomeValue'
                            ELSE
                            INSERT INTO Table1 VALUES (...)
                         */
                        sqlQuery = "SELECT COUNT(*) AS Expr1"
                                    + " FROM VSD_Test_type"
                                    + " WHERE (test_type_id = " + x.id + ")";
                        command = new SqlCeCommand(sqlQuery, con);
                        rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);
                        rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;

                        //rountine for Defect Table
                        if (rowCount > 0)
                        {
                            rs.ReadFirst();
                            temp = rs.GetInt32(0);
                        }
                        if (temp == 0)
                        {
                            sqlQuery = " INSERT INTO VSD_test_type (test_type_ID,record_type,test_type_code,test_type_name,test_type_name_a,test_type_desc,test_Type_desc_a,is_comprehensive_test,days_valid,isdisabled) VALUES (" + x.id + "," + (x.recordType != null ? "'" + x.recordType + "'" : "NULL") + "," + (x.code != null ? "'" + x.code + "'" : "NULL") + "," + (x.name != null ? "'" + x.name.Replace("'", "`") + "'" : "NULL") + "," + (x.nameArabic != null ? "'" + x.nameArabic.Replace("'", "`") + "'" : "NULL") + "," + (x.Description != null ? "'" + x.Description.Replace("'", "`") + "'" : "NULL") + "," + (x.DescriptionArabic != null ? "'" + x.DescriptionArabic.Replace("'", "`") + "'" : "NULL") + "," + (x.isComprehensive != null ? "'" + x.isComprehensive + "'" : "NULL") + "," + (x.daysValid != null ? "" + x.daysValid.ToString() + "" : "NULL") + "," + (x.isDisabled != null ? "'" + x.isDisabled + "'" : "NULL") + ")";
                        }
                        else
                        {
                            sqlQuery = " UPDATE vsd_test_type SET test_type_id = " + x.id + ", record_type = " + (x.recordType != null ? "'" + x.recordType + "'" : "NULL") + ", test_type_code = " + (x.code != null ? "'" + x.code + "'" : "NULL") + ",test_type_name = " + (x.name != null ? "'" + x.name.Replace("'", "`") + "'" : "NULL") + ",test_type_name_a = " + (x.nameArabic != null ? "'" + x.nameArabic.Replace("'", "`") + "'" : "NULL") + ",test_type_desc = " + (x.Description != null ? "'" + x.Description.Replace("'", "`") + "'" : "NULL") + ",test_type_desc_a = " + (x.DescriptionArabic != null ? "'" + x.DescriptionArabic.Replace("'", "`") + "'" : "NULL") + ",is_comprehensive_test = " + (x.isComprehensive != null ? "'" + x.isComprehensive + "'" : "NULL") + ",days_valid = " + (x.daysValid != null ? "" + x.daysValid.ToString() + "" : "NULL") + ",isdisabled = " + (x.isDisabled != null ? "'" + x.isDisabled + "'" : "NULL") + " WHERE test_type_id = " + x.id;
                        }

                        command = new SqlCeCommand(sqlQuery, con);

                        rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);
                        rs.Close();
                        con.Close();
                    }
                    App.VSDLog.Info("\n SynchronizeConfig().Synchronize_testType:" + WebServiceDefects.Length);

                }
                App.VSDLog.Info("\n SynchronizeConfig().Synchronize_testType: --------------- END");
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="test"></param>
        public void Synchronize_violationSeverityFeeRule(handHeldService.SynchronizeConfigDataResponseItem test)
        {
            try
            {
                App.VSDLog.Info("\n SynchronizeConfig().Synchronize_violationSeverityFeeRule: --------------- START");
                SqlCeConnection con = ((VSDApp.com.rta.vsd.hh.db.IDBManager)VSDApp.com.rta.vsd.hh.db.DBConnectionManager.GetInstance()).GetConnection();
                string sqlQuery;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCeCommand command;
                SqlCeResultSet rs;
                int rowCount;
                int temp = -1;
                //check for Vehicle Category Defect updates
                if (test.violationSeverityFeeRule != null)
                {

                    handHeldService.ViolationSeverityFeeRule[] WebServiceDefects = new VSDApp.handHeldService.ViolationSeverityFeeRule[test.violationSeverityFeeRule.Length];
                    WebServiceDefects = test.violationSeverityFeeRule;
                    foreach (handHeldService.ViolationSeverityFeeRule x in WebServiceDefects)
                    {
                        if (con.State == ConnectionState.Closed) { con.Open(); }
                        /*
                         * IF EXISTS (SELECT * FROM VSD_Defect WHERE DefectID = @DefectID)
                            UPDATE Table1 SET (...) WHERE Column1='SomeValue'
                            ELSE
                            INSERT INTO Table1 VALUES (...)
                         */


                        sqlQuery = "SELECT COUNT(*) AS Expr1"
                                    + " FROM VSD_Vio_Sev_Fee_Rule"
                                    + " WHERE (Vio_Sev_Fee_Rule_ID = @Vio_Sev_Fee_Rule_ID)";
                        command = new SqlCeCommand(sqlQuery, con);
                        command.Parameters.Add("@Vio_Sev_Fee_Rule_ID", SqlDbType.Int).Value = x.id;
                        rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);
                        rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;

                        //rountine for Defect Table
                        if (rowCount > 0)
                        {
                            rs.ReadFirst();
                            temp = rs.GetInt32(0);
                        }
                        if (temp == 0)
                        {
                            sqlQuery = " INSERT INTO VSD_Vio_Sev_Fee_Rule ( Vio_Sev_Fee_Rule_ID,Severity_Level_ID,Test_Type_ID,Test_Attempts,Min_No_Of_Defects,Max_No_Of_Defects,Is_Under_DueDate,Is_Fee_Applied,Fine_Amount,IsDisabled ) VALUES ( " + x.id + "," + (x.severityId != null ? "" + x.severityId + "" : "NULL") + "," + (x.testTypeId != null ? "" + x.testTypeId + "" : "NULL") + "," + (x.testAttepmts != null ? "" + x.testAttepmts + "" : "NULL") + "," + (x.minDefects != null ? "" + x.minDefects + "" : "NULL") + "," + (x.maxDefects != null ? "" + x.maxDefects + "" : "NULL") + "," + (x.isUnderDueDate != null ? "'" + x.isUnderDueDate + "'" : "NULL") + "," + (x.isFeeApplied != null ? "'" + x.isFeeApplied + "'" : "NULL") + "," + (x.feeAmount != null ? "" + x.feeAmount + "" : "NULL") + "," + (x.isDisabled != null ? "'" + x.isDisabled + "'" : "NULL") + " )";
                        }
                        else
                        {
                            sqlQuery = " UPDATE VSD_Vio_Sev_Fee_Rule SET Vio_Sev_Fee_Rule_ID = " + x.id + ",Severity_Level_ID = " + (x.severityId != null ? "" + x.severityId + "" : "NULL") + ",Test_Type_ID = " + (x.testTypeId != null ? "" + x.testTypeId + "" : "NULL") + ",Test_Attempts = " + (x.testAttepmts != null ? "" + x.testAttepmts + "" : "NULL") + ",Min_No_Of_Defects = " + (x.minDefects != null ? "" + x.minDefects + "" : "NULL") + ",Max_No_Of_Defects = " + (x.maxDefects != null ? "" + x.maxDefects + "" : "NULL") + ",Is_Under_DueDate = " + (x.isUnderDueDate != null ? "'" + x.isUnderDueDate + "'" : "NULL") + ",Is_Fee_Applied = " + (x.isFeeApplied != null ? "'" + x.isFeeApplied + "'" : "NULL") + ",Fine_Amount = " + (x.feeAmount != null ? "" + x.feeAmount + "" : "NULL") + ",IsDisabled  = " + (x.isDisabled != null ? "'" + x.isDisabled + "'" : "NULL") + "  WHERE Vio_Sev_Fee_Rule_ID =" + x.id;
                        }

                        command = new SqlCeCommand(sqlQuery, con);
                        command.Parameters.Add("@Vio_Sev_Fee_Rule_ID", SqlDbType.Int).Value = x.id;
                        command.Parameters.Add("@Severity_Level_ID", SqlDbType.Int).Value = x.severityId;
                        command.Parameters.Add("@Test_Type_ID", SqlDbType.Int).Value = x.testTypeId;
                        command.Parameters.Add("@Test_Attempts", SqlDbType.Int).Value = x.testAttepmts;
                        command.Parameters.Add("@Min_No_Of_Defects", SqlDbType.Int).Value = x.minDefects;
                        command.Parameters.Add("@Max_No_Of_Defects", SqlDbType.Int).Value = x.maxDefects;
                        command.Parameters.Add("@Is_Under_DueDate", SqlDbType.NVarChar).Value = x.isUnderDueDate;
                        command.Parameters.Add("@Is_Fee_Applied", SqlDbType.NVarChar).Value = x.isFeeApplied;
                        command.Parameters.Add("@Fine_Amount", SqlDbType.Int).Value = x.feeAmount;
                        command.Parameters.Add("@IsDisabled", SqlDbType.NVarChar).Value = x.isDisabled;
                        rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);
                        rs.Close();
                        con.Close();
                    }
                    App.VSDLog.Info("\n SynchronizeConfig().Synchronize_violationSeverityFeeRule: " + WebServiceDefects.Length);

                }
                App.VSDLog.Info("\n SynchronizeConfig().Synchronize_violationSeverityFeeRule: --------------- END");
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="test"></param>
        public void Synchronize_DefectProperty(handHeldService.SynchronizeConfigDataResponseItem test)
        {
            try
            {
                App.VSDLog.Info("\n SynchronizeConfig().Synchronize_DefectProperty: --------------- START");
                SqlCeConnection con = ((VSDApp.com.rta.vsd.hh.db.IDBManager)VSDApp.com.rta.vsd.hh.db.DBConnectionManager.GetInstance()).GetConnection();
                string sqlQuery;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCeCommand command;
                SqlCeResultSet rs;
                int rowCount;
                int temp = -1;
                ///////////////////////////
                //check for Vehicle Category Defect updates
                if (test.vehicleCategoryDefectSeverityFine != null)
                {
                    handHeldService.DefectProperty[] WebServiceDefectsProperty = new VSDApp.handHeldService.DefectProperty[test.DefectProperty.Length];
                    WebServiceDefectsProperty = test.DefectProperty;
                    foreach (handHeldService.DefectProperty x in WebServiceDefectsProperty)
                    {
                        if (con.State == ConnectionState.Closed) { con.Open(); }
                        /*
                         * IF EXISTS (SELECT * FROM VSD_Defect WHERE DefectID = @DefectID)
                            UPDATE Table1 SET (...) WHERE Column1='SomeValue'
                            ELSE
                            INSERT INTO Table1 VALUES (...)
                         */
                        sqlQuery = "SELECT COUNT(*) AS Expr1"
                                    + " FROM VSD_Defect_Property"
                                    + " WHERE (DEFECT_PROPERTY_ID = @DefectPropertyID)";
                        command = new SqlCeCommand(sqlQuery, con);
                        command.Parameters.Add("@DefectPropertyID", SqlDbType.Int).Value = x.defectPropertyId;
                        rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);
                        rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;



                        //rountine for Defect Table
                        if (rowCount > 0)
                        {
                            rs.ReadFirst();
                            temp = rs.GetInt32(0);
                        }

                        if (temp == 0)
                        {
                            //  sqlQuery = " INSERT INTO VSD_Defect_Property (DEFECT_PROPERTY_ID,DEFECT_ID,ENFORCE_ISSUE_VIOLATION,ENFORCE_FINE,ENFORCE_TESTING,IS_DISABLED,IS_DELETED) VALUES (" + (x.defectPropertyId != null ? "" + x.defectPropertyId + "" : "NULL") + "," + (x.defectId != null ? "" + x.defectId + "" : "NULL") + "," + (x.enforceIssueViolation != null ? "'" + x.enforceIssueViolation + "'" : "NULL") + "," + (x.enforceFine != null ? "'" + x.enforceFine + "'" : "NULL") + "," + (x.enforceTesting != null ? "'" + x.enforceTesting + "'" : "NULL") + "," + (x.isDisabled != null ? "'" + x.isDisabled + "'" : "NULL") + "," + (x.isDeleted != null ? "'" + x.isDeleted + "'" : "NULL") + ")";
                            sqlQuery = " INSERT INTO VSD_Defect_Property (DEFECT_PROPERTY_ID,DEFECT_ID,ENFORCE_ISSUE_VIOLATION,ENFORCE_FINE,ENFORCE_TESTING,IS_DISABLED,IS_DELETED,TimeStamp) VALUES (" + (x.defectPropertyId != null ? "" + x.defectPropertyId + "" : "NULL") + "," + (x.defectId != null ? "" + x.defectId + "" : "NULL") + "," + (x.enforceIssueViolation != null ? "'" + x.enforceIssueViolation + "'" : "NULL") + "," + (x.enforceFine != null ? "'" + x.enforceFine + "'" : "NULL") + "," + (x.enforceTesting != null ? "'" + x.enforceTesting + "'" : "NULL") + "," + (x.isDisabled != null ? "'" + x.isDisabled + "'" : "NULL") + "," + (x.isDeleted != null ? "'" + x.isDeleted + "'" : "NULL") + ("'" + String.Format("{0:M/d/yyyy HH:mm:ss}", DateTime.Now) + "'") + ")";
                        }
                        else
                        {
                            sqlQuery = " UPDATE VSD_Defect_Property SET DEFECT_PROPERTY_ID = " + (x.defectPropertyId != null ? "" + x.defectPropertyId + "" : "NULL") + ",DEFECT_ID = " + (x.defectId != null ? "" + x.defectId + "" : "NULL") + ",ENFORCE_ISSUE_VIOLATION = " + (x.enforceIssueViolation != null ? "'" + x.enforceIssueViolation + "'" : "NULL") + ",ENFORCE_FINE = " + (x.enforceFine != null ? "'" + x.enforceFine + "'" : "NULL") + ",ENFORCE_TESTING = " + (x.enforceTesting != null ? "'" + x.enforceTesting + "'" : "NULL") + ",IS_DISABLED = " + (x.isDisabled != null ? "'" + x.isDisabled + "'" : "NULL") + ",IS_DELETED = " + (x.isDeleted != null ? "'" + x.isDeleted + "'" : "NULL") + " WHERE DEFECT_PROPERTY_ID = " + x.defectPropertyId;
                        }
                        command = new SqlCeCommand(sqlQuery, con);
                        rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);
                        rs.Close();
                        con.Close();
                    }
                    App.VSDLog.Info("\n SynchronizeConfig().Synchronize_DefectProperty:" + WebServiceDefectsProperty.Length);
                }
                App.VSDLog.Info("\n SynchronizeConfig().Synchronize_DefectProperty: --------------- END");

            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
            }
        }
        /// <summary>
        /// Synchonize Vehcile Catgoery Defect Severity Fine Info local DB
        /// </summary>
        /// <param name="test"></param>
        public void Synchronize_VehCatDefSevFine(handHeldService.SynchronizeConfigDataResponseItem test)
        {
            try
            {
                App.VSDLog.Info("\n SynchronizeConfig().Synchronize_VehCatDefSevFine: --------------- START");
                SqlCeConnection con = ((VSDApp.com.rta.vsd.hh.db.IDBManager)VSDApp.com.rta.vsd.hh.db.DBConnectionManager.GetInstance()).GetConnection();
                string sqlQuery;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCeCommand command;
                SqlCeResultSet rs;
                int rowCount;
                int temp = -1;
                ///////////////////////////
                //check for Vehicle Category Defect updates
                if (test.vehicleCategoryDefectSeverityFine != null)
                {
                    handHeldService.VehicleCategoryDefectSeverityFine[] WebServiceDefectsSevFine = new VSDApp.handHeldService.VehicleCategoryDefectSeverityFine[test.vehicleCategoryDefectSeverityFine.Length];
                    WebServiceDefectsSevFine = test.vehicleCategoryDefectSeverityFine;
                    foreach (handHeldService.VehicleCategoryDefectSeverityFine x in WebServiceDefectsSevFine)
                    {
                        if (con.State == ConnectionState.Closed) { con.Open(); }
                        /*
                         * IF EXISTS (SELECT * FROM VSD_Defect WHERE DefectID = @DefectID)
                            UPDATE Table1 SET (...) WHERE Column1='SomeValue'
                            ELSE
                            INSERT INTO Table1 VALUES (...)
                         */
                        sqlQuery = "SELECT COUNT(*) AS Expr1"
                                    + " FROM VSD_Veh_Cat_Def_Sev_Fine"
                                    + " WHERE (Veh_Cat_Def_Sev_Fine_ID = @VehCatDefSevFineID)";
                        command = new SqlCeCommand(sqlQuery, con);
                        command.Parameters.Add("@VehCatDefSevFineID", SqlDbType.Int).Value = x.id;
                        rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);
                        rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;



                        //rountine for Defect Table
                        if (rowCount > 0)
                        {
                            rs.ReadFirst();
                            temp = rs.GetInt32(0);
                        }
                        if (temp == 0)
                        {
                            sqlQuery = " INSERT INTO VSD_Veh_Cat_Def_Sev_Fine (Veh_Cat_Def_Sev_Fine_ID,Veh_Cat_Defect_ID,Fine_Amount,BlackPoints,Required_Grounding,Grounded_Days,Is_Disabled,Partner_Fine_ID) VALUES (" + x.id + "," + (x.vehicleCategoryDefectSeverityId != null ? "" + x.vehicleCategoryDefectSeverityId + "" : "NULL") + "," + (x.fineAmount != null ? "" + x.fineAmount + "" : "NULL") + "," + (x.blackPoints != null ? "" + x.blackPoints + "" : "NULL") + "," + (x.requireGrounding != null ? "'" + x.requireGrounding + "'" : "NULL") + "," + (x.groundedDays != null ? "" + x.groundedDays + "" : "NULL") + "," + (x.isDisabled != null ? "'" + x.isDisabled + "'" : "NULL") + "," + (x.partnerFineId != null ? "'" + x.partnerFineId + "'" : "NULL") + ")";
                        }
                        else
                        {
                            sqlQuery = " UPDATE VSD_Veh_Cat_Def_Sev_Fine SET Veh_Cat_Def_Sev_Fine_ID = " + x.id + ",Veh_Cat_Defect_ID = " + (x.vehicleCategoryDefectSeverityId != null ? "" + x.vehicleCategoryDefectSeverityId + "" : "NULL") + ",Fine_Amount = " + (x.fineAmount != null ? "" + x.fineAmount + "" : "NULL") + ",BlackPoints = " + (x.blackPoints != null ? "" + x.blackPoints + "" : "NULL") + ",Required_Grounding = " + (x.requireGrounding != null ? "'" + x.requireGrounding + "'" : "NULL") + ",Grounded_Days = " + (x.groundedDays != null ? "" + x.groundedDays + "" : "NULL") + ",Is_Disabled = " + (x.isDisabled != null ? "'" + x.isDisabled + "'" : "NULL") + ",Partner_Fine_ID = " + (x.partnerFineId != null ? "'" + x.partnerFineId + "'" : "NULL") + " WHERE Veh_Cat_Def_Sev_Fine_ID = " + x.id;
                        }
                        command = new SqlCeCommand(sqlQuery, con);
                        rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);
                        rs.Close();
                        con.Close();
                    }
                    App.VSDLog.Info("\n SynchronizeConfig().Synchronize_VehCatDefSevFine:" + WebServiceDefectsSevFine.Length);
                }
                App.VSDLog.Info("\n SynchronizeConfig().Synchronize_VehCatDefSevFine: --------------- END");
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
            }
        }

        /// <summary>
        /// VSD Partner Finto Info Updation
        /// </summary>
        /// <param name="test"></param>
        public void Synchronize_PartnerFine(handHeldService.SynchronizeConfigDataResponseItem test)
        {
            try
            {
                App.VSDLog.Info("\n SynchronizeConfig().Synchronize_PartnerFine: --------------- START");
                SqlCeConnection con = ((VSDApp.com.rta.vsd.hh.db.IDBManager)VSDApp.com.rta.vsd.hh.db.DBConnectionManager.GetInstance()).GetConnection();
                string sqlQuery;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCeCommand command;
                SqlCeResultSet rs;
                int rowCount;
                int temp = -1;
                ///////////////////////////
                //check for Vehicle Category Defect updates
                if (test.partnerFine != null)
                {
                    handHeldService.PartnerFine[] WebServicepartnerFine = new VSDApp.handHeldService.PartnerFine[test.partnerFine.Length];

                    WebServicepartnerFine = test.partnerFine;
                    foreach (handHeldService.PartnerFine x in WebServicepartnerFine)
                    {
                        if (con.State == ConnectionState.Closed) { con.Open(); }
                        /*
                         * IF EXISTS (SELECT * FROM VSD_Defect WHERE DefectID = @DefectID)
                            UPDATE Table1 SET (...) WHERE Column1='SomeValue'
                            ELSE
                            INSERT INTO Table1 VALUES (...)
                         */
                        sqlQuery = "SELECT COUNT(*) AS Expr1"
                                    + " FROM VSD_Partner_Fine"
                                    + " WHERE (Partner_Fine_ID = @PartnerFineID)";
                        command = new SqlCeCommand(sqlQuery, con);
                        command.Parameters.Add("@PartnerFineID", SqlDbType.Int).Value = x.id;
                        rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);
                        rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;



                        //rountine for Defect Table
                        if (rowCount > 0)
                        {
                            rs.ReadFirst();
                            temp = rs.GetInt32(0);
                        }
                        if (temp == 0)
                        {
                            sqlQuery = " INSERT INTO VSD_Partner_Fine (Partner_Fine_ID,Partner_ID,Fine_ID,Ext_Fine_Code,Is_Disabled) VALUES (" + x.id + "," + (x.partnerId != null ? "'" + x.partnerId + "'" : "NULL") + "," + (x.fineId != null ? "'" + x.fineId + "'" : "NULL") + "," + (x.extFineCode != null ? "'" + x.extFineCode + "'" : "NULL") + "," + (x.isDisabled != null ? "'" + x.isDisabled + "'" : "NULL") + ")";
                        }
                        else
                        {
                            sqlQuery = " UPDATE VSD_Partner_Fine SET Partner_Fine_ID = " + x.id + ",Partner_ID = " + (x.partnerId != null ? "'" + x.partnerId + "'" : "NULL") + ",Fine_ID = " + (x.fineId != null ? "'" + x.fineId + "'" : "NULL") + ",Ext_Fine_Code = " + (x.extFineCode != null ? "'" + x.extFineCode + "'" : "NULL") + ",Is_Disabled = " + (x.isDisabled != null ? "'" + x.isDisabled + "'" : "NULL") + " WHERE Partner_Fine_ID = " + x.id; ; ;
                        }
                        command = new SqlCeCommand(sqlQuery, con);


                        rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);
                        rs.Close();
                        con.Close();
                    }
                    App.VSDLog.Info("\n SynchronizeConfig().Synchronize_PartnerFine: " + WebServicepartnerFine.Length);
                }
                App.VSDLog.Info("\n SynchronizeConfig().Synchronize_PartnerFine: --------------- END");
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
            }
        }


        /// <summary>
        /// Synchonize Partner Info into local DB
        /// </summary>
        /// <param name="test"></param>
        public void Synchronize_Partner(handHeldService.SynchronizeConfigDataResponseItem test)
        {
            try
            {
                App.VSDLog.Info("\n SynchronizeConfig().Synchronize_Partner: --------------- START");
                SqlCeConnection con = ((VSDApp.com.rta.vsd.hh.db.IDBManager)VSDApp.com.rta.vsd.hh.db.DBConnectionManager.GetInstance()).GetConnection();
                string sqlQuery;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCeCommand command;
                SqlCeResultSet rs;
                int rowCount;
                int temp = -1;
                ///////////////////////////
                //check for Vehicle Category Defect updates
                if (test.partner != null)
                {
                    handHeldService.Partner[] WebServicePartner = new VSDApp.handHeldService.Partner[test.partner.Length];

                    WebServicePartner = test.partner;
                    foreach (handHeldService.Partner x in WebServicePartner)
                    {
                        if (con.State == ConnectionState.Closed) { con.Open(); }
                        /*
                         * IF EXISTS (SELECT * FROM VSD_Defect WHERE DefectID = @DefectID)
                            UPDATE Table1 SET (...) WHERE Column1='SomeValue'
                            ELSE
                            INSERT INTO Table1 VALUES (...)
                         */
                        sqlQuery = "SELECT COUNT(*) AS Expr1"
                                    + " FROM VSD_Partner"
                                    + " WHERE (Partner_ID = @PartnerID)";
                        command = new SqlCeCommand(sqlQuery, con);
                        command.Parameters.Add("@PartnerID", SqlDbType.Int).Value = x.id;
                        rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);
                        rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;



                        //rountine for Defect Table
                        if (rowCount > 0)
                        {
                            rs.ReadFirst();
                            temp = rs.GetInt32(0);
                        }
                        if (temp == 0)
                        {
                            sqlQuery = " INSERT INTO VSD_Partner (Partner_ID,Location_ID,Partner_Code,Partner_Name,Partner_Name_A,Is_Disabled) VALUES (" + x.id + "," + (x.locationId != null ? "'" + x.locationId + "'" : "NULL") + "," + (x.partnerCode != null ? "'" + x.partnerCode + "'" : "NULL") + "," + (x.partnerName != null ? "'" + x.partnerName + "'" : "NULL") + "," + (x.partnerNameA != null ? "'" + x.partnerNameA + "'" : "NULL") + "," + (x.isDisabled != null ? "'" + x.isDisabled + "'" : "NULL") + ")";
                        }
                        else
                        {
                            sqlQuery = " UPDATE VSD_Partner SET Partner_ID = " + x.id + ",Location_ID = " + (x.locationId != null ? "'" + x.locationId + "'" : "NULL") + ",Partner_Code = " + (x.partnerCode != null ? "'" + x.partnerCode + "'" : "NULL") + ",Partner_Name = " + (x.partnerName != null ? "'" + x.partnerName + "'" : "NULL") + ",Partner_Name_A = " + (x.partnerNameA != null ? "'" + x.partnerNameA + "'" : "NULL") + ",Is_Disabled = " + (x.isDisabled != null ? "'" + x.isDisabled + "'" : "NULL") + " WHERE Partner_ID = " + x.id; ;
                        }
                        command = new SqlCeCommand(sqlQuery, con);

                        rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);
                        rs.Close();
                        con.Close();
                    }
                    App.VSDLog.Info("\n SynchronizeConfig().Synchronize_Partner:" + WebServicePartner.Length);
                }
                App.VSDLog.Info("\n SynchronizeConfig().Synchronize_Partner: --------------- END");
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
            }
        }

        /// <summary>
        /// Synchonize list of Vehicle fo Intrest into local DB
        /// </summary>
        /// <param name="test"></param>
        public void Synchronize_FineAmmount(handHeldService.SynchronizeConfigDataResponseItem test)
        {
            try
            {
                App.VSDLog.Info("\n SynchronizeConfig().Synchronize_FineAmmount: --------------- START");
                SqlCeConnection con = ((VSDApp.com.rta.vsd.hh.db.IDBManager)VSDApp.com.rta.vsd.hh.db.DBConnectionManager.GetInstance()).GetConnection();
                string sqlQuery;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCeCommand command;
                SqlCeResultSet rs;
                int rowCount;
                int temp = -1;
                ///////////////////////////
                //check for Vehicle Category Defect updates
                if (test.fine != null)
                {
                    handHeldService.Fine[] WebServiceFine = new VSDApp.handHeldService.Fine[test.fine.Length];
                    WebServiceFine = test.fine;
                    foreach (handHeldService.Fine x in WebServiceFine)
                    {
                        if (con.State == ConnectionState.Closed) { con.Open(); }
                        /*
                         * IF EXISTS (SELECT * FROM VSD_Defect WHERE DefectID = @DefectID)
                            UPDATE Table1 SET (...) WHERE Column1='SomeValue'
                            ELSE
                            INSERT INTO Table1 VALUES (...)
                         */
                        sqlQuery = "SELECT COUNT(*) AS Expr1"
                                    + " FROM VSD_Fine"
                                    + " WHERE (Fine_ID = @FineID)";
                        command = new SqlCeCommand(sqlQuery, con);
                        command.Parameters.Add("@FineID", SqlDbType.Int).Value = x.id;
                        rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);
                        rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;



                        //rountine for Defect Table
                        if (rowCount > 0)
                        {
                            rs.ReadFirst();
                            temp = rs.GetInt32(0);
                        }
                        if (temp == 0)
                        {
                            sqlQuery = " INSERT INTO VSD_Fine (Fine_ID,Record_Type,Fine_Name,Fine_Name_a,Fine_Desc,Fine_Desc_a,Is_Disabled) VALUES (" + x.id + "," + (x.recordType != null ? "'" + x.recordType + "'" : "NULL") + "," + (x.fineName != null ? "'" + x.fineName + "'" : "NULL") + "," + (x.fineNameA != null ? "'" + x.fineNameA + "'" : "NULL") + "," + (x.fineDesc != null ? "'" + x.fineDesc + "'" : "NULL") + "," + (x.fineDescA != null ? "'" + x.fineDescA + "'" : "NULL") + "," + (x.isDisabled != null ? "'" + x.isDisabled + "'" : "NULL") + ")";
                        }
                        else
                        {
                            sqlQuery = " UPDATE VSD_Fine SET Fine_ID = " + x.id + ",Record_Type = " + (x.recordType != null ? "'" + x.recordType + "'" : "NULL") + ",Fine_Name = " + (x.fineName != null ? "'" + x.fineName + "'" : "NULL") + ",Fine_Name_a = " + (x.fineNameA != null ? "'" + x.fineNameA + "'" : "NULL") + ",Fine_Desc = " + (x.fineDesc != null ? "'" + x.fineDesc + "'" : "NULL") + ",Fine_Desc_a = " + (x.fineDescA != null ? "'" + x.fineDescA + "'" : "NULL") + ",Is_Disabled = " + (x.isDisabled != null ? "'" + x.isDisabled + "'" : "NULL") + " WHERE Fine_ID = " + x.id;
                        }
                        command = new SqlCeCommand(sqlQuery, con);

                        //  App.VSDLog.Info(" SynchronizeConfig().Synchronize_FineAmmount: TestsqlQuery:"+sqlQuery);
                        try
                        {
                            rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);
                        }
                        catch (Exception ex)
                        {
                        }

                        rs.Close();
                        con.Close();
                    }
                    App.VSDLog.Info("\n SynchronizeConfig().Synchronize_FineAmmount:" + WebServiceFine.Length);
                }
                App.VSDLog.Info("\n SynchronizeConfig().Synchronize_FineAmmount: --------------- END");
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(" SynchronizeConfig().Synchronize_FineAmmount : Exception");
                App.VSDLog.Info(ex.StackTrace);
            }
        }
        /// <summary>
        /// Synchronize 
        /// </summary>
        /// <param name="test"></param>
        public void Synchronize_VehicleofIntrest(handHeldService.SynchronizeConfigDataResponseItem test)
        {
            try
            {

            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
            }
        }

        int IDBManager.SynchronizeConfig(handHeldService.SynchronizeConfigDataResponseItem test)
        {

            if (test != null)
            {

                SqlCeConnection con = ((VSDApp.com.rta.vsd.hh.db.IDBManager)VSDApp.com.rta.vsd.hh.db.DBConnectionManager.GetInstance()).GetConnection();
                string sqlQuery;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCeCommand command;
                SqlCeResultSet rs;
                int rowCount;
                int temp = -1;
                App.VSDLog.Info("\n DBAppLoginManager.SynchronizeConfig(): --------------- START");

                Synchronize_defectCategoty(test);
                Synchronize_defect(test);
                Synchronize_country(test);
                Synchronize_countryProperty(test);
                Synchronize_location(test);
                Synchronize_propertyType(test);
                Synchronize_propertyValue(test);
                Synchronize_severityLevel(test);
                Synchronize_severityLevelProperty(test);
                Synchronize_vehicleCategory(test);
                Synchronize_vehicle_Plate_Cat_Code(test);
                Synchronize_vehicleCatDefectSeverity(test);
                Synchronize_testType(test);
                Synchronize_violationSeverityFeeRule(test);

                // New Synchoronization Function()
                Sychronize_defectComments(test);
                Synchronize_VehicleofIntrest(test);
                Synchronize_FineAmmount(test);
                Synchronize_Partner(test);
                Synchronize_PartnerFine(test);
                Synchronize_VehCatDefSevFine(test);
                Synchronize_DefectProperty(test);
                App.VSDLog.Info("\n DBAppLoginManager.SynchronizeConfig(): --------------- END");


            }
            return Int32.Parse((test.response.code));
        }

        private void Sychronize_defectComments(SynchronizeConfigDataResponseItem test)
        {
            try
            {
                SqlCeConnection con = ((VSDApp.com.rta.vsd.hh.db.IDBManager)VSDApp.com.rta.vsd.hh.db.DBConnectionManager.GetInstance()).GetConnection();
                string sqlQuery;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCeCommand command;
                SqlCeResultSet rs;
                int rowCount;
                int temp = -1;


                //check for country updates
                if (test.comments != null)
                {
                    handHeldService.Comment[] WebServiceDefectsComments = new VSDApp.handHeldService.Comment[test.comments.Length];
                    WebServiceDefectsComments = test.comments;
                    foreach (handHeldService.Comment x in WebServiceDefectsComments)
                    {
                        if (con.State == ConnectionState.Closed) { con.Open(); }

                        sqlQuery = "SELECT COUNT(*) AS Expr1"
                                    + " FROM VSD_Defect_Comment"
                                    + " WHERE (DEFECT_COMMENT_ID = @DefectCommentID)";
                        command = new SqlCeCommand(sqlQuery, con);
                        command.Parameters.Add("@DefectCommentID", SqlDbType.Int).Value = Int32.Parse(x.defectCommentId);
                        rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);
                        rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;

                        //rountine for Defect Table
                        if (rowCount > 0)
                        {
                            rs.ReadFirst();
                            temp = rs.GetInt32(0);
                        }
                        string isDeleted = "F", isDisabled = "F";


                        if (temp == 0)
                        {

                            sqlQuery = @" INSERT INTO VSD_Defect_Comment (DEFECT_COMMENT_ID ,DEFECT_ID,VEHICLE_CATEGORY_ID,DEFECT_COMMENT,DEFECT_COMMENT_A
                                        ,IS_DISABLED, IS_DELETED) 
                                        VALUES (" + (x.defectCommentId != null ? x.defectCommentId : "NULL") +
                                                "," + (x.defectId != null ? x.defectId : "NULL") +
                                                "," + (x.vehicleCategoryId != null ? x.vehicleCategoryId : "NULL") +
                                                "," + (x.defectComment != null ? "'" + x.defectComment + "'" : "NULL") +
                                                "," + (x.defectCommentArabic != null ? "'" + x.defectCommentArabic + "'" : "NULL") +
                                                "," + (x.isDisabled.Equals("True") ? "'T'" : "'F'") +
                                                "," + (x.isDeleted.Equals("True") ? "'T'" : "'F'") +
                                //"," + (x.createdBy != null ? "'" + x.createdBy + "'" : "NULL") + 
                                //"," + (x.createdTimestamp != null ? "'" + x.createdTimestamp + "'" : "NULL") + 
                                //"," + (x.lastUpdatedBy != null ? "'" + x.lastUpdatedBy + "'" : "NULL") + 
                                //"," + (x.updatedTimestamp != null ? "" + x.updatedTimestamp + "" : "NULL") +
                                                ") ";
                        }
                        else
                        {
                            sqlQuery = " UPDATE VSD_Defect_Comment SET DEFECT_COMMENT_ID = " + (x.defectCommentId != null ? x.defectCommentId : "NULL") +
                                                ",DEFECT_ID = " + (x.defectId != null ? x.defectId : "NULL") +
                                                ",VEHICLE_CATEGORY_ID = " + (x.vehicleCategoryId != null ? x.vehicleCategoryId : "NULL") +
                                                ",DEFECT_COMMENT=" + (x.defectComment != null ? "'" + x.defectComment + "'" : "NULL") +
                                                ",DEFECT_COMMENT_A=" + (x.defectCommentArabic != null ? "'" + x.defectCommentArabic + "'" : "NULL") +
                                                ",IS_DISABLED=" + (x.isDisabled.Equals("True") ? "'T'" : "'F'") +
                                                ",IS_DELETED=" + (x.isDeleted.Equals("True") ? "'T'" : "'F'") +
                                //",CREATED_BY=" + (x.createdBy != null ? "'" + x.createdBy + "'" : "NULL") + 
                                //",CREATED_TIMESTAMP=" + (x.createdTimestamp != null ? "'" + x.createdTimestamp + "'" : "NULL") + 
                                //",LAST_UPDATED_BY=" + (x.lastUpdatedBy != null ? "'" + x.lastUpdatedBy + "'" : "NULL") + 
                                //",UPDATED_TIMESTAMP" + (x.updatedTimestamp != null ? "" + x.updatedTimestamp + "" : "NULL") +

                                                " WHERE DEFECT_COMMENT_ID = " + (x.defectCommentId != null ? "" + x.defectCommentId + "" : "NULL");
                        }
                        command = new SqlCeCommand(sqlQuery, con);
                        rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);
                        rs.Close();
                        con.Close();

                    }
                }

            }
            catch (Exception ex)
            {
                App.VSDLog.Info("\n DBApploginManager.Sychronize_defectComments() Field to insert/update record in VSD_DEFECT_Comment table. " + ex.Message + "\n " + ex.StackTrace);
            }
        }
        public string[] GetDefectSeverityBusinessRules(string defectName, string defectSubCategory, string defectMainCat, string value)
        {
            throw new NotImplementedException();
        }

        public string[] GetConfigurationDataForDueDays(string severity, int numberOfDefects)
        {
            throw new NotImplementedException();
        }

        public string CalculateSeverity(string currentSeverity, string newSeverity, string type)
        {
            throw new NotImplementedException();
        }

        public string GetRecommendation(DateTime dueDate, string violationStatus, string emirate, string severity)
        {
            throw new NotImplementedException();
        }

        public string GetDefectCodes()
        {
            throw new NotImplementedException();
        }

        VSDApp.com.rta.vsd.hh.data.Violation[] IDBManager.GetOfflineData()
        {
            throw new NotImplementedException();
        }
        public DataTable GetDefectSevAndPropertyBusinesRule(string defectID)
        {
            throw new NotImplementedException();
        }


        #endregion
    }
}
