using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Media.Imaging;
using VSDApp.com.rta.vsd.hh.manager;
using VSDApp.com.rta.vsd.hh.utilities;
using VSDApp.WPFMessageBoxControl;

namespace VSDApp.com.rta.vsd.hh.db
{
    public class DBDataLoadManager : IDBDataLoad
    {
        private static DBDataLoadManager _dbDataLoadManager;
        private DBDataLoadManager() { }
        public static DBDataLoadManager GetInstance()
        {
            if (_dbDataLoadManager == null)
            {
                _dbDataLoadManager = new DBDataLoadManager();
                //populate all data here in refernce variables 
            }
            return _dbDataLoadManager;
        }

        #region IDBDataLoad Members

        string[] IDBDataLoad.GetLocation(string area)
        {

            // Schweers.Localization.ArabicConverter arab = new Schweers.Localization.ArabicConverter();

            SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
            App.VSDLog.Info("Connection String" + con.ConnectionString);
            string sqlQuery;

            if (con.State == ConnectionState.Closed) { con.Open(); }
            SqlCeCommand command;


            if (area == null)
            {
                return new string[] { "" };
            }
            else
            {

                sqlQuery = "SELECT * FROM VSD_Location WHERE PARENT_Location_ID IN (SELECT Location_ID FROM VSD_Location WHERE Location_Area_NAME = @Area) AND ISDISABLED ='F' order by Location_ID";
                command = new SqlCeCommand(sqlQuery, con);
                command.Parameters.Add("@Area", SqlDbType.NChar, 200).Value = area;

            }

            SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);
            int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;

            // System.Windows.Forms.MessageBox.Show(sqlQuery);
            string[] location = new string[rowCount];

            //// if (Resources.GetInstance().GetLocale().Equals(Resources.locale_EN))
            if (AppProperties.Selected_Resource == "English")
            {


                if (rowCount > 0)
                {
                    rs.ReadFirst();
                    try
                    {
                        location[0] = rs.GetString(3);
                    }
                    catch (System.Data.SqlTypes.SqlNullValueException ex)
                    {
                        // App.VSDLog.Info(ex.StackTrace);
                        location[0] = "Null";
                    }

                    if (rowCount > 1)
                    {
                        rs.Read();
                        for (int i = 1; i < rowCount; i++)
                        {
                            try
                            {
                                location[i] = rs.GetString(3);
                                rs.Read();
                            }
                            catch (System.Data.SqlTypes.SqlNullValueException ex)
                            {
                                // App.VSDLog.Info(ex.StackTrace);
                                location[i] = "Null"; continue;
                            }

                        }
                    }

                }
                rs.Close();
                con.Close();
                return location;
            }
            else
            {

                if (rowCount > 0)
                {
                    rs.ReadFirst();
                    try
                    {
                        location[0] = rs.GetString(4) + "^" + rs.GetString(3);
                        //location[0] = "";
                    }
                    catch (System.Data.SqlTypes.SqlNullValueException sqlex)
                    {
                        // App.VSDLog.Info(sqlex.StackTrace);
                        location[0] = "Null";
                    }

                    if (rowCount > 1)
                    {
                        rs.Read();
                        for (int i = 1; i < rowCount; i++)
                        {
                            try
                            {
                                location[i] = rs.GetString(4) + "^" + rs.GetString(3);
                                // location[i]="";
                                rs.Read();
                            }
                            catch (System.Data.SqlTypes.SqlNullValueException ex)
                            {
                                // App.VSDLog.Info(ex.StackTrace);
                                location[i] = "Null";
                                continue;
                            }

                        }
                    }
                }
                rs.Close();
                con.Close();
                return location;
            }

        }
        /// <summary>
        /// Query from Local DB to get Intrested List Chassis No
        /// </summary>
        /// <returns></returns>
        string[] IDBDataLoad.GetInterestListVehicle()
        {
            SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
            string sqlQuery;

            if (con.State == ConnectionState.Closed) { con.Open(); }
            SqlCeCommand command;

            sqlQuery = "SELECT * FROM VSD_Interest_List_Vehicle where VSD_Interest_List_Vehicle.IconType != 'amber.png' ";
            command = new SqlCeCommand(sqlQuery, con);


            SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);
            int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;



            string[] vehicles = new string[rowCount];

            if (rowCount > 0)
            {
                rs.ReadFirst();
                vehicles[0] = rs.GetString(0);
                if (rowCount > 1)
                {
                    rs.Read();
                    for (int i = 1; i < rowCount; i++)
                    {
                        try
                        {
                            vehicles[i] = rs.GetString(0);
                            rs.Read();
                        }
                        catch (System.Data.SqlTypes.SqlNullValueException ex)
                        {
                            //App.VSDLog.Info(ex.StackTrace);
                            vehicles[i] = "Null"; continue;
                        }

                    }
                }

            }
            rs.Close();
            con.Close();
            return vehicles;

        }

        string[] IDBDataLoad.GetCities(string country)
        {
            // Schweers.Localization.ArabicConverter arab = new Schweers.Localization.ArabicConverter();

            SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
            string sqlQuery;

            if (con.State == ConnectionState.Closed) { con.Open(); }
            SqlCeCommand command;


            if (country == null)
            {
                sqlQuery = "SELECT * FROM VSD_Country INNER JOIN VSD_Country_Prop ON " +
                    "VSD_Country.Country_ID = VSD_Country_Prop.Country_ID WHERE " +
                  "(VSD_Country_Prop.IsDisabled = 'F') AND (VSD_Country.Parent_Country_ID IS NOT NULL)";
                command = new SqlCeCommand(sqlQuery, con);
            }
            else
            {


                sqlQuery = "SELECT * FROM VSD_Country INNER JOIN VSD_Country_Prop ON " +
                "VSD_Country.Country_ID = VSD_Country_Prop.Country_ID WHERE " +
              "(VSD_Country_Prop.IsDisabled = 'F') AND (VSD_Country.Parent_Country_ID IN " +
              "(SELECT Country_ID FROM VSD_Country AS VSD_Country_1 WHERE (Country_Name = @COUNTRY)))";
                command = new SqlCeCommand(sqlQuery, con);
                command.Parameters.Add("@COUNTRY", SqlDbType.NChar, 50).Value = country;

            }

            SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);
            int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;

            string[] countries = new string[rowCount];

            /// if (Resources.GetInstance().GetLocale().Equals(Resources.locale_EN))
            if (AppProperties.Selected_Resource == "English")
            {


                if (rowCount > 0)
                {
                    rs.ReadFirst();
                    countries[0] = rs.GetString(4);
                    if (rowCount > 1)
                    {
                        rs.Read();
                        for (int i = 1; i < rowCount; i++)
                        {
                            try
                            {
                                countries[i] = rs.GetString(4);
                                rs.Read();
                            }
                            catch (System.Data.SqlTypes.SqlNullValueException ex)
                            {
                                //App.VSDLog.Info(ex.StackTrace);
                                countries[i] = "Null"; continue;
                            }

                        }
                    }

                }
                rs.Close();
                con.Close();
                return countries;
            }
            else
            {

                if (rowCount > 0)
                {
                    rs.ReadFirst();
                    //  countries[0] = arab.Format(rs.GetString(5)) + "^" + arab.Format(rs.GetString(4));
                    countries[0] = rs.GetString(5) + "^" + rs.GetString(4);
                    // countries[0] = "";
                    if (rowCount > 1)
                    {
                        rs.Read();
                        for (int i = 1; i < rowCount; i++)
                        {
                            try
                            {
                                //  countries[i] = arab.Format(rs.GetString(5)) + "^" + arab.Format(rs.GetString(4));
                                countries[i] = rs.GetString(5) + "^" + rs.GetString(4);
                                // countries[0] = "";
                                rs.Read();
                            }
                            catch (System.Data.SqlTypes.SqlNullValueException ex)
                            {
                                // App.VSDLog.Info(ex.StackTrace);
                                countries[i] = "Null";
                                continue;
                            }

                        }
                    }
                }
                rs.Close();
                con.Close();
                return countries;
            }


        }

        string IDBDataLoad.GetArabicLocationNameFromEn(string locationName)
        {
            if (locationName == null || locationName == "")
            {
                return "";
            }
            SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
            string sqlQuery;




            if (con.State == ConnectionState.Closed) { con.Open(); }
            SqlCeCommand command;

            sqlQuery = "SELECT Location_Area_Name_A FROM VSD_Location WHERE  (Location_Area_Name = @locationName)";




            command = new SqlCeCommand(sqlQuery, con);

            command.Parameters.Add("@locationName", SqlDbType.NChar, 200).Value = locationName;

            SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);

            int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;

            string LocationNameAr = "";
            if (rs.Read())
            {
                LocationNameAr = rs.GetString(0);
            }

            rs.Close();
            con.Close();
            return LocationNameAr;


        }

        string[] IDBDataLoad.GetAreas(string emirate)
        {
            // Schweers.Localization.ArabicConverter arab = new Schweers.Localization.ArabicConverter();
            if (emirate == null)
            {
                return new string[] { "" };
            }
            else
            {

                SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
                string sqlQuery;

                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCeCommand command;


                sqlQuery = "SELECT * FROM  VSD_Location WHERE (Country_ID IN (SELECT VSD_COUNTRY.Country_ID FROM VSD_Country WHERE VSD_COUNTRY.Country_Name = @Emirate))  AND (Parent_Location_ID IS NULL) AND ISDISABLED ='F'";
                command = new SqlCeCommand(sqlQuery, con);
                command.Parameters.Add("@Emirate", SqlDbType.NChar, 200).Value = emirate.Trim();




                SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);

                int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;

                //System.Windows.Forms.MessageBox.Show(sqlQuery);
                string[] areas = new string[rowCount];


                //// if (Resources.GetInstance().GetLocale().Equals(Resources.locale_EN))
                if (AppProperties.Selected_Resource == "English")
                {

                    if (rowCount > 0)
                    {
                        rs.ReadFirst();
                        areas[0] = rs.GetString(3);
                        if (rowCount > 1)
                        {
                            rs.Read();
                            for (int i = 1; i < rowCount; i++)
                            {
                                try
                                {
                                    areas[i] = rs.GetString(3);
                                    rs.Read();
                                }
                                catch (System.Data.SqlTypes.SqlNullValueException ex)
                                {
                                    // App.VSDLog.Info(ex.StackTrace);
                                    areas[i] = "Null"; continue;
                                }

                            }
                        }

                    }
                    rs.Close();
                    con.Close();
                    return areas;
                }
                else
                {


                    if (rowCount > 0)
                    {
                        rs.ReadFirst();
                        try
                        {
                            areas[0] = rs.GetString(4) + "^" + rs.GetString(3);
                            // areas[0] = "";
                        }
                        catch (System.Data.SqlTypes.SqlNullValueException sqlex)
                        {
                            //App.VSDLog.Info(sqlex.StackTrace);
                            areas[0] = "Null";
                        }

                        if (rowCount > 1)
                        {
                            rs.Read();
                            for (int i = 1; i < rowCount; i++)
                            {
                                try
                                {
                                    areas[i] = rs.GetString(4) + "^" + rs.GetString(3);
                                    //areas[0] = "";
                                    rs.Read();
                                }
                                catch (System.Data.SqlTypes.SqlNullValueException ex)
                                {
                                    //  App.VSDLog.Info(ex.StackTrace);
                                    areas[i] = "Null";
                                    continue;
                                }

                            }
                        }
                    }
                    rs.Close();
                    con.Close();
                    return areas;
                }
            }

        }

        string[] IDBDataLoad.GetCountries()
        {
            // Schweers.Localization.ArabicConverter arab = new Schweers.Localization.ArabicConverter();

            SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();


            string sqlQuery = "SELECT * FROM VSD_Country INNER JOIN VSD_Country_Prop ON " +
                    "VSD_Country.Country_ID = VSD_Country_Prop.Country_ID WHERE " +
                  "(VSD_Country_Prop.IsDisabled = 'F') AND (VSD_Country.Parent_Country_ID IS NULL)";


            /*
             string sqlQuery = "SELECT * FROM VSD_Country INNER JOIN VSD_Country_Prop ON " +
                    "VSD_Country.Country_ID = VSD_Country_Prop.Country_ID WHERE " +
                  "(VSD_Country.Parent_Country_ID IS NULL)";*/

            if (con.State == ConnectionState.Closed) { con.Open(); }
            SqlCeCommand command = new SqlCeCommand(sqlQuery, con);

            SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);

            int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;

            string[] countries = new string[rowCount];



            // if (Resources.GetInstance().GetLocale().Equals(Resources.locale_EN))
            if (AppProperties.Selected_Resource == "English")
            {

                if (rowCount > 0)
                {
                    rs.ReadFirst();
                    countries[0] = rs.GetString(4);
                    if (rowCount > 1)
                    {
                        rs.Read();
                        for (int i = 1; i < rowCount; i++)
                        {
                            try
                            {
                                countries[i] = rs.GetString(4);
                                rs.Read();
                            }
                            catch (System.Data.SqlTypes.SqlNullValueException ex)
                            {
                                // App.VSDLog.Info(ex.StackTrace);
                                countries[i] = "Null"; continue;
                            }

                        }
                    }

                }
                rs.Close();
                con.Close();
                return countries;
            }
            else
            {


                if (rowCount > 0)
                {
                    rs.ReadFirst();
                    // countries[0] = arab.Format(rs.GetString(5)).Trim() + "^" + arab.Format(rs.GetString(4)).Trim();
                    countries[0] = rs.GetString(5).Trim() + "^" + rs.GetString(4).Trim();
                    // countries[0] = "";
                    if (rowCount > 1)
                    {
                        rs.Read();
                        for (int i = 1; i < rowCount; i++)
                        {
                            try
                            {
                                // countries[i] = arab.Format(rs.GetString(5)) + "^" + arab.Format(rs.GetString(4)).Trim();
                                countries[i] = rs.GetString(5).Trim() + "^" + rs.GetString(4).Trim();
                                // countries[0] = "";
                                rs.Read();
                            }
                            catch (System.Data.SqlTypes.SqlNullValueException ex)
                            {
                                // App.VSDLog.Info(ex.StackTrace);
                                countries[i] = "Null";
                                continue;
                            }

                        }
                    }
                }
                rs.Close();
                con.Close();
                return countries;
            }
        }


        string[] IDBDataLoad.GetAllCountriesForNationality()
        {
            // Schweers.Localization.ArabicConverter arab = new Schweers.Localization.ArabicConverter();

            SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();


            //string sqlQuery = "SELECT * FROM VSD_Country INNER JOIN VSD_Country_Prop ON " +
            //       "VSD_Country.Country_ID = VSD_Country_Prop.Country_ID WHERE " +
            //     "(VSD_Country.Parent_Country_ID IS NULL)";

            string sqlQuery = "SELECT * FROM VSD_Country INNER JOIN VSD_Country_Prop ON " +
                    "VSD_Country.Country_ID = VSD_Country_Prop.Country_ID WHERE " +
                  "(VSD_Country_Prop.IsDisabled = 'F') AND (VSD_Country.Parent_Country_ID IS NULL)";

            if (con.State == ConnectionState.Closed) { con.Open(); }
            SqlCeCommand command = new SqlCeCommand(sqlQuery, con);

            SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);

            int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;

            string[] countries = new string[rowCount];



            // if (Resources.GetInstance().GetLocale().Equals(Resources.locale_EN))
            /*
            if (AppProperties.Selected_Resource == "English")
            {

                if (rowCount > 0)
                {
                    rs.ReadFirst();
                    countries[0] = rs.GetString(4);
                    if (rowCount > 1)
                    {
                        rs.Read();
                        for (int i = 1; i < rowCount; i++)
                        {
                            try
                            {
                                countries[i] = rs.GetString(4);
                                rs.Read();
                            }
                            catch (System.Data.SqlTypes.SqlNullValueException ex)
                            {
                                // App.VSDLog.Info(ex.StackTrace);
                                countries[i] = "Null"; continue;
                            }

                        }
                    }

                }
                rs.Close();
                con.Close();
                return countries;
            }
            else
            {
                */

            if (rowCount > 0)
            {
                rs.ReadFirst();
                // countries[0] = arab.Format(rs.GetString(5)).Trim() + "^" + arab.Format(rs.GetString(4)).Trim();
                countries[0] = rs.GetString(5).Trim() + "^" + rs.GetString(4).Trim();
                // countries[0] = "";
                if (rowCount > 1)
                {
                    rs.Read();
                    for (int i = 1; i < rowCount; i++)
                    {
                        try
                        {
                            // countries[i] = arab.Format(rs.GetString(5)) + "^" + arab.Format(rs.GetString(4)).Trim();
                            countries[i] = rs.GetString(5).Trim() + "^" + rs.GetString(4).Trim();
                            // countries[0] = "";
                            rs.Read();
                        }
                        catch (System.Data.SqlTypes.SqlNullValueException ex)
                        {
                            // App.VSDLog.Info(ex.StackTrace);
                            countries[i] = "Null";
                            continue;
                        }

                    }
                }
            }
            rs.Close();
            con.Close();
            return countries;
            //  }
        }


        string[] IDBDataLoad.GetACountriesForNationalityDriver()
        {
            // Schweers.Localization.ArabicConverter arab = new Schweers.Localization.ArabicConverter();

            SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();


            string sqlQuery = "SELECT * FROM VSD_Country INNER JOIN VSD_Country_Prop ON " +
                   "VSD_Country.Country_ID = VSD_Country_Prop.Country_ID WHERE " +
                 "(VSD_Country.Parent_Country_ID IS NULL)";

            //string sqlQuery = "SELECT * FROM VSD_Country INNER JOIN VSD_Country_Prop ON " +
            //        "VSD_Country.Country_ID = VSD_Country_Prop.Country_ID WHERE " +
            //      "(VSD_Country_Prop.IsDisabled = 'F') AND (VSD_Country.Parent_Country_ID IS NULL)";

            if (con.State == ConnectionState.Closed) { con.Open(); }
            SqlCeCommand command = new SqlCeCommand(sqlQuery, con);

            SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);

            int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;

            string[] countries = new string[rowCount];



            // if (Resources.GetInstance().GetLocale().Equals(Resources.locale_EN))
            /*
            if (AppProperties.Selected_Resource == "English")
            {

                if (rowCount > 0)
                {
                    rs.ReadFirst();
                    countries[0] = rs.GetString(4);
                    if (rowCount > 1)
                    {
                        rs.Read();
                        for (int i = 1; i < rowCount; i++)
                        {
                            try
                            {
                                countries[i] = rs.GetString(4);
                                rs.Read();
                            }
                            catch (System.Data.SqlTypes.SqlNullValueException ex)
                            {
                                // App.VSDLog.Info(ex.StackTrace);
                                countries[i] = "Null"; continue;
                            }

                        }
                    }

                }
                rs.Close();
                con.Close();
                return countries;
            }
            else
            {
                */

            if (rowCount > 0)
            {
                rs.ReadFirst();
                // countries[0] = arab.Format(rs.GetString(5)).Trim() + "^" + arab.Format(rs.GetString(4)).Trim();
                countries[0] = rs.GetString(5).Trim() + "^" + rs.GetString(4).Trim();
                // countries[0] = "";
                if (rowCount > 1)
                {
                    rs.Read();
                    for (int i = 1; i < rowCount; i++)
                    {
                        try
                        {
                            // countries[i] = arab.Format(rs.GetString(5)) + "^" + arab.Format(rs.GetString(4)).Trim();
                            countries[i] = rs.GetString(5).Trim() + "^" + rs.GetString(4).Trim();
                            // countries[0] = "";
                            rs.Read();
                        }
                        catch (System.Data.SqlTypes.SqlNullValueException ex)
                        {
                            // App.VSDLog.Info(ex.StackTrace);
                            countries[i] = "Null";
                            continue;
                        }

                    }
                }
            }
            rs.Close();
            con.Close();
            return countries;
            //  }
        }

        Hashtable IDBDataLoad.GetPlateDetailsInArabic(string plateCat, string plateCode)
        {
            // Schweers.Localization.ArabicConverter arab = new Schweers.Localization.ArabicConverter();

            if ((null == plateCat) || ("" == plateCat))
            {
                // return new string[] { "" };
                return null;
            }

            SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();

            string sqlQuery = string.Empty;

            if (plateCode == "")
            {
                sqlQuery = "SELECT DISTINCT Category_Name, Category_Name_A FROM VSD_Veh_Plate_Code_Cat WHERE (Category_Name IN (@plateCat))";
            }
            else
            {
                sqlQuery = "SELECT DISTINCT Category_Name, Category_Name_A FROM VSD_Veh_Plate_Code_Cat WHERE (Category_Name IN (@plateCat, @plateCode))";

            }

            if (con.State == ConnectionState.Closed) { con.Open(); }

            SqlCeCommand command = new SqlCeCommand(sqlQuery, con);
            command.Parameters.Add("@plateCat", SqlDbType.NChar, 200).Value = plateCat;
            command.Parameters.Add("@plateCode", SqlDbType.NChar, 200).Value = plateCode;

            SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);

            int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;

            // string[]  = new string[rowCount];

            Hashtable plateDetails = new Hashtable();
            for (int i = 0; i < rowCount; i++)
            {  //rs.ReadFirst();

                //  plateCategory[0] = "";
                rs.Read();
                if (!plateDetails.Contains(rs.GetString(0).Trim()))
                    plateDetails.Add(rs.GetString(0).Trim(), rs.GetString(1).Trim());






            }
            rs.Close();
            con.Close();
            return plateDetails;


        }

        string[] IDBDataLoad.GetPlateCategories(string emirate)
        {
            // Schweers.Localization.ArabicConverter arab = new Schweers.Localization.ArabicConverter();

            if (null == emirate)
            {
                return new string[] { "" };
            }

            SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
            if (AppProperties.Selected_Resource == "English")
            {


                string sqlQuery = "SELECT * FROM VSD_Veh_Plate_Code_Cat WHERE PARENT_Veh_Plate_Code_ID IS NULL and COUNTRY_ID IN"
                    + " (Select Country_ID from VSD_COUNTRY where Country_Name = @emirate) AND ISDISABLED ='F'";

                if (con.State == ConnectionState.Closed) { con.Open(); }

                SqlCeCommand command = new SqlCeCommand(sqlQuery, con);
                command.Parameters.Add("@emirate", SqlDbType.NChar).Value = emirate;

                SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);

                int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;

                string[] plateCategory = new string[rowCount];

                if (rowCount > 0)
                {

                    rs.ReadFirst();
                    plateCategory[0] = rs.GetString(4);
                    if (rowCount > 1)
                    {
                        rs.Read();
                        for (int i = 1; i < rowCount; i++)
                        {
                            try
                            {
                                plateCategory[i] = rs.GetString(4);
                                rs.Read();
                            }
                            catch (System.Data.SqlTypes.SqlNullValueException ex)
                            {
                                //App.VSDLog.Info(ex.StackTrace);
                                plateCategory[i] = "Null"; continue;
                            }

                        }
                    }

                }
                rs.Close();
                con.Close();
                return plateCategory;
            }
            else
            {
                string sqlQuery = "SELECT * FROM VSD_Veh_Plate_Code_Cat WHERE PARENT_Veh_Plate_Code_ID IS NULL and COUNTRY_ID IN"
                   + " (Select Country_ID from VSD_COUNTRY where Country_Name = @emirate) AND ISDISABLED ='F'";

                if (con.State == ConnectionState.Closed) { con.Open(); }

                SqlCeCommand command = new SqlCeCommand(sqlQuery, con);
                command.Parameters.Add("@emirate", SqlDbType.NChar).Value = emirate;

                SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);

                int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;

                string[] plateCategory = new string[rowCount];

                if (rowCount > 0)
                {
                    rs.ReadFirst();
                    plateCategory[0] = rs.GetString(5) + "^" + rs.GetString(4);
                    //  plateCategory[0] = "";
                    if (rowCount > 1)
                    {
                        rs.Read();
                        for (int i = 1; i < rowCount; i++)
                        {
                            try
                            {
                                plateCategory[i] = rs.GetString(5) + "^" + rs.GetString(4);
                                // plateCategory[i] = "";
                                rs.Read();
                            }
                            catch (System.Data.SqlTypes.SqlNullValueException ex)
                            {
                                // App.VSDLog.Info(ex.StackTrace);
                                plateCategory[i] = "Null";
                                continue;
                            }

                        }
                    }
                }
                rs.Close();
                con.Close();
                return plateCategory;
            }

        }

        string[] IDBDataLoad.GetPlateCodes(string plateCategory, string emirate)
        {
            // Schweers.Localization.ArabicConverter arab = new Schweers.Localization.ArabicConverter();

            if (null == plateCategory)
            {
                return new string[] { };
            }

            //if (Resources.GetInstance().GetLocale().Equals(Resources.locale_EN))
            //{
            SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
            SqlCeCommand command;

            string sqlQuery = "SELECT * FROM VSD_Veh_Plate_Code_Cat WHERE PARENT_Veh_Plate_Code_ID IN "
            + " (Select Veh_Plate_Code_Cat_ID from VSD_Veh_Plate_Code_Cat where Category_Name = @plateCat) AND ISDISABLED ='F'" +
            " AND (Country_ID IN (SELECT Country_ID  FROM VSD_Country WHERE (Country_Name = @emirate)))";

            if (con.State == ConnectionState.Closed) { con.Open(); }

            command = new SqlCeCommand(sqlQuery, con);
            command.Parameters.Add("@plateCat", SqlDbType.NChar, 100).Value = plateCategory;
            command.Parameters.Add("@emirate", SqlDbType.NChar).Value = (null != emirate) ? emirate : "No Value";


            SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);

            int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;

            string[] plateCode = new string[rowCount];

            // if (Resources.GetInstance().GetLocale().Equals(Resources.locale_EN))
            if (AppProperties.Selected_Resource == "English")
            {

                if (rowCount > 0)
                {

                    rs.ReadFirst();
                    plateCode[0] = rs.GetString(4);
                    if (rowCount > 1)
                    {
                        rs.Read();
                        for (int i = 1; i < rowCount; i++)
                        {
                            try
                            {
                                plateCode[i] = rs.GetString(4);
                                rs.Read();
                            }
                            catch (System.Data.SqlTypes.SqlNullValueException ex)
                            {
                                // App.VSDLog.Info(ex.StackTrace);
                                plateCode[i] = "Null"; continue;
                            }

                        }
                    }

                }
            }
            else
            {
                if (rowCount > 0)
                {

                    rs.ReadFirst();
                    plateCode[0] = rs.GetString(5).Trim() + "^" + rs.GetString(4).Trim();
                    // plateCode[0] = "";
                    if (rowCount > 1)
                    {
                        rs.Read();
                        for (int i = 1; i < rowCount; i++)
                        {
                            try
                            {
                                plateCode[i] = rs.GetString(5).Trim() + "^" + rs.GetString(4).Trim();
                                // plateCode[i] = "";
                                rs.Read();
                            }
                            catch (System.Data.SqlTypes.SqlNullValueException ex)
                            {
                                // App.VSDLog.Info(ex.StackTrace);
                                plateCode[i] = "Null"; continue;
                            }

                        }
                    }

                }
            }
            rs.Close();
            con.Close();
            return plateCode;



        }

        string[] IDBDataLoad.GetVehicleCategories()
        {
            //  Schweers.Localization.ArabicConverter arab = new Schweers.Localization.ArabicConverter();

            SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
            string sqlQuery = "SELECT * FROM VSD_VEHICLE_CATEGORY WHERE PARENT_VEHICLE_CATEGORY_ID IS NULL AND ISDISABLED ='F'";

            if (con.State == ConnectionState.Closed) { con.Open(); }

            SqlCeCommand command = new SqlCeCommand(sqlQuery, con);

            SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);

            int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;

            string[] vehicleCategory = new string[rowCount];


            // if (Resources.GetInstance().GetLocale().Equals(Resources.locale_EN))
            if (AppProperties.Selected_Resource == "English")
            {

                if (rowCount > 0)
                {

                    rs.ReadFirst();
                    vehicleCategory[0] = rs.GetString(4);
                    if (rowCount > 1)
                    {
                        rs.Read();
                        for (int i = 1; i < rowCount; i++)
                        {
                            try
                            {
                                vehicleCategory[i] = rs.GetString(4).Trim();
                                rs.Read();
                            }
                            catch (System.Data.SqlTypes.SqlNullValueException ex)
                            {
                                // App.VSDLog.Info(ex.StackTrace);
                                vehicleCategory[i] = "Null"; continue;
                            }

                        }
                    }

                }
                rs.Close();
                con.Close();
                return vehicleCategory;
            }
            else
            {

                if (rowCount > 0)
                {
                    rs.ReadFirst();
                    vehicleCategory[0] = rs.GetString(5).Trim() + "^" + rs.GetString(4).Trim();
                    //  vehicleCategory[0] = "";
                    if (rowCount > 1)
                    {
                        rs.Read();
                        for (int i = 1; i < rowCount; i++)
                        {
                            try
                            {
                                vehicleCategory[i] = rs.GetString(5).Trim() + "^" + rs.GetString(4).Trim();
                                //  vehicleCategory[i] = "";
                                rs.Read();
                            }
                            catch (System.Data.SqlTypes.SqlNullValueException ex)
                            {
                                // App.VSDLog.Info(ex.StackTrace);
                                vehicleCategory[i] = "Null";
                                continue;
                            }

                        }
                    }
                }
                rs.Close();
                con.Close();
                return vehicleCategory;
            }
        }


        string[] IDBDataLoad.GetVehicleSubCategories(string vehicleCategory)
        {
            //  Schweers.Localization.ArabicConverter arab = new Schweers.Localization.ArabicConverter();

            SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
            string sqlQuery = string.Empty;
            if (AppProperties.Selected_Resource == "English")
            {
                 sqlQuery = "SELECT * FROM VSD_VEHICLE_CATEGORY WHERE ISDISABLED ='F' AND PARENT_VEHICLE_CATEGORY_ID IN " +" (SELECT Vehicle_Category_ID FROM VSD_VEHICLE_CATEGORY WHERE ISDISABLED ='F' and Category_Name ='"+vehicleCategory+"')";
            }
            else
            {
                sqlQuery = "SELECT * FROM VSD_VEHICLE_CATEGORY WHERE ISDISABLED ='F' AND PARENT_VEHICLE_CATEGORY_ID IN " + " (SELECT Vehicle_Category_ID FROM VSD_VEHICLE_CATEGORY WHERE ISDISABLED ='F' and Category_Name_A='"+vehicleCategory+"')";
            }
            if (con.State == ConnectionState.Closed) { con.Open(); }

            SqlCeCommand command = new SqlCeCommand(sqlQuery, con);

            SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);

            int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;

            string[] vehiclesubCategory = new string[rowCount];


             //if (Resources.GetInstance().GetLocale().Equals(Resources.locale_EN))
            if (AppProperties.Selected_Resource == "English")
            {

                if (rowCount > 0)
                {

                    rs.ReadFirst();
                    vehiclesubCategory[0] = rs.GetString(4);
                    if (rowCount > 1)
                    {
                        rs.Read();
                        for (int i = 1; i < rowCount; i++)
                        {
                            try
                            {
                                vehiclesubCategory[i] = rs.GetString(4).Trim();
                                rs.Read();
                            }
                            catch (System.Data.SqlTypes.SqlNullValueException ex)
                            {
                                // App.VSDLog.Info(ex.StackTrace);
                                vehiclesubCategory[i] = "Null"; continue;
                            }

                        }
                    }

                }
                rs.Close();
                con.Close();
                return vehiclesubCategory;
            }
            else
            {

                if (rowCount > 0)
                {
                    rs.ReadFirst();
                    vehiclesubCategory[0] = rs.GetString(5).Trim() + "^" + rs.GetString(4).Trim();
                    //  vehicleCategory[0] = "";
                    if (rowCount > 1)
                    {
                        rs.Read();
                        for (int i = 1; i < rowCount; i++)
                        {
                            try
                            {
                                vehiclesubCategory[i] = rs.GetString(5).Trim() + "^" + rs.GetString(4).Trim();
                                //  vehicleCategory[i] = "";
                                rs.Read();
                            }
                            catch (System.Data.SqlTypes.SqlNullValueException ex)
                            {
                                // App.VSDLog.Info(ex.StackTrace);
                                vehiclesubCategory[i] = "Null";
                                continue;
                            }

                        }
                    }
                }
                rs.Close();
                con.Close();
                return vehiclesubCategory;
            }
        }

        public string GetVehicleCategoryName(string vehicleCategorysubName_A,string vehicleCategory)
        {
            App.VSDLog.Info("vehicleCategorysubName_A " + vehicleCategorysubName_A + "vehicleCategorysubName_A  " +vehicleCategory );
            string categoryName = string.Empty;
            try
            {
                SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
                string sqlQuery;

                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCeCommand command;

                if (AppProperties.Selected_Resource == "English")
                {
                    sqlQuery = "SELECT Category_Name FROM VSD_VEHICLE_CATEGORY WHERE (Category_Name_A = @vehCat)";
                }
                else
                {
                    sqlQuery = "SELECT Category_Name FROM VSD_VEHICLE_CATEGORY WHERE (Category_Name_A = @vehCat)";
                }
                command = new SqlCeCommand(sqlQuery, con);

                command.Parameters.Add("@vehCat", SqlDbType.NChar, 100).Value = vehicleCategorysubName_A;

                SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);

                int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;

                
                if (rs.Read())
                {
                    categoryName = rs.GetString(0);
                }

                rs.Close();
                con.Close();
            }
            catch(Exception ex)
            {
                categoryName = "";
            }
            return categoryName;
        }

        string[] IDBDataLoad.GetOffenceCategories(string type, string recordType)
        {
            DataTable dtOffenceCateogorie = new DataTable();
            App.VSDLog.Info("GetOffenceCategories()" + type);
            try
            {

                SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
                string sqlQuery;
                SqlCeCommand command;
                if (type == "Infraction")
                {
                    sqlQuery = @"SELECT  Distinct DC.Defect_Category_ID, Parent_Defect_Category_ID, Category_Code, Category_Name,Category_Name_A
                                    FROM   VSD_Defect_Category DC
                                    INNER JOIN VSD_Defect ON VSD_Defect.Defect_Category_ID = DC.Defect_Category_ID 
                                    INNER JOIN   VSD_Veh_Cat_Defect ON VSD_Defect.Defect_ID = VSD_Veh_Cat_Defect.Defect_ID 
                                    INNER JOIN   VSD_Veh_CAT_DEF_SEV_FINE ON VSD_Veh_Cat_Defect.VEH_CAT_DEFECT_ID = VSD_Veh_CAT_DEF_SEV_FINE.VEH_CAT_DEFECT_ID
                                    where (Parent_Defect_Category_ID IN (SELECT        Defect_Category_ID
                                        FROM            VSD_Defect_Category  VSD_Defect_Category_1
                                        WHERE        (Category_Name = '" + recordType + @"')))
                                        
                                        AND (VSD_Veh_Cat_Defect.IsDisabled = 'F') AND (VSD_Veh_CAT_DEF_SEV_FINE.Is_Disabled = 'F')";


                }
                else
                {
                    sqlQuery = @"SELECT  Distinct DC.Defect_Category_ID, Parent_Defect_Category_ID, Category_Code, Category_Name,Category_Name_A
                                FROM   VSD_Defect_Category DC
                                INNER JOIN VSD_Defect ON VSD_Defect.Defect_Category_ID = DC.Defect_Category_ID 
                                LEFT OUTER JOIN   VSD_Veh_Cat_Defect ON VSD_Defect.Defect_ID = VSD_Veh_Cat_Defect.Defect_ID 
                                LEFT OUTER JOIN   VSD_Veh_CAT_DEF_SEV_FINE ON VSD_Veh_Cat_Defect.VEH_CAT_DEFECT_ID = VSD_Veh_CAT_DEF_SEV_FINE.VEH_CAT_DEFECT_ID
                                where (Parent_Defect_Category_ID IN (SELECT        Defect_Category_ID
                                                                        FROM            VSD_Defect_Category  VSD_Defect_Category_1
                                                                        WHERE        (Category_Name = 'Offences_RN'))) 
                                                                        AND (DC.Defect_Category_ID NOT IN 
										                                (SELECT  Distinct DC.Defect_Category_ID
                                FROM   VSD_Defect_Category DC
                                INNER JOIN VSD_Defect ON VSD_Defect.Defect_Category_ID = DC.Defect_Category_ID 
                                INNER JOIN   VSD_Veh_Cat_Defect ON VSD_Defect.Defect_ID = VSD_Veh_Cat_Defect.Defect_ID 
                                INNER JOIN   VSD_Veh_CAT_DEF_SEV_FINE ON VSD_Veh_Cat_Defect.VEH_CAT_DEFECT_ID = VSD_Veh_CAT_DEF_SEV_FINE.VEH_CAT_DEFECT_ID
                                where (Parent_Defect_Category_ID IN (SELECT        Defect_Category_ID
                                        FROM            VSD_Defect_Category  VSD_Defect_Category_1
                                        WHERE        (Category_Name = '" + recordType + @"')))
                                        AND (VSD_Veh_Cat_Defect.IsDisabled = 'F') AND (VSD_Veh_CAT_DEF_SEV_FINE.Is_Disabled = 'F')))";
                }
                App.VSDLog.Info("Query:" + sqlQuery);
                if (con.State == ConnectionState.Closed) { con.Open(); }
                command = new SqlCeCommand(sqlQuery, con);
                SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);

                int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;

                string[] offenceCateogories = new string[rowCount];


                //    if (Resources.GetInstance().GetLocale().Equals(Resources.locale_EN))
                if (AppProperties.Selected_Resource == "English")
                {

                    if (rowCount > 0)
                    {

                        rs.ReadFirst();
                        offenceCateogories[0] = rs.GetString(3);
                        if (rowCount > 1)
                        {
                            rs.Read();
                            for (int i = 1; i < rowCount; i++)
                            {
                                try
                                {
                                    offenceCateogories[i] = rs.GetString(3);
                                    rs.Read();
                                }
                                catch (System.Data.SqlTypes.SqlNullValueException ex)
                                {
                                    // App.VSDLog.Info(ex.StackTrace);
                                    offenceCateogories[i] = "Null"; continue;
                                }

                            }
                        }

                    }
                    rs.Close();
                    con.Close();
                    return offenceCateogories;
                }
                else
                {

                    if (rowCount > 0)
                    {
                        rs.ReadFirst();
                        offenceCateogories[0] = rs.GetString(4).Trim() + "^" + rs.GetString(3).Trim();
                        // defects[0] = "";
                        if (rowCount > 1)
                        {
                            rs.Read();
                            for (int i = 1; i < rowCount; i++)
                            {
                                try
                                {
                                    offenceCateogories[i] = rs.GetString(4).Trim() + "^" + rs.GetString(3).Trim();
                                    // defects[i] = "";
                                    rs.Read();
                                }
                                catch (System.Data.SqlTypes.SqlNullValueException ex)
                                {
                                    //App.VSDLog.Info(ex.StackTrace);
                                    offenceCateogories[i] = "Null";
                                    continue;
                                }

                            }
                        }
                    }
                    rs.Close();
                    con.Close();
                    return offenceCateogories;
                }
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
                return null;
            }
        }

        DataTable IDBDataLoad.GetOffence(string recordType, string OffenceCategory)
        {
            DataTable dt = new DataTable();
            App.VSDLog.Info("GetOffence()" + OffenceCategory);
            try
            {

                string sqlQuery;
                SqlCeCommand command;
                /* sqlQuery = @"SELECT Distinct VSD_DEFECT.DEFECT_NAME,VSD_DEFECT.DEFECT_NAME_A,VSD_DEFECT.Defect_Code FROM VSD_Defect 
                             INNER JOIN VSD_Defect_Category ON VSD_Defect.Defect_Category_ID = VSD_Defect_Category.Defect_Category_ID 
                             INNER JOIN   VSD_Veh_Cat_Defect_Sev ON VSD_Defect.Defect_ID = VSD_Veh_Cat_Defect_Sev.Defect_ID 
                             INNER JOIN   VSD_V_CAT_DEF_SEV_FINE ON VSD_Veh_Cat_Defect_Sev.VEH_CAT_DEFECT_SEV_ID = VSD_V_CAT_DEF_SEV_FINE.VEH_CAT_DEFECT_SEV_ID
                             Where (VSD_DEFECT_CATEGORY.Defect_category_ID IN ( Select DEFECT_CATEGORY_ID FROM VSD_DEFECT_CATEGORY 
                             WHERE CATEGORY_NAME = '"+OffenceCategory+@"'))  
                             and (VSD_Veh_Cat_Defect_Sev.Is_Disabled = 'F')  and  ( VSD_DEFECT.defect_type = '"+recordType+@"') 
                             ORDER BY VSD_Defect.Defect_Code";*/

                sqlQuery = @"SELECT Distinct VSD_DEFECT.DEFECT_NAME,VSD_DEFECT.DEFECT_NAME_A,VSD_DEFECT.Defect_Code,VSD_Defect.Defect_Value_Size,VSD_Defect.defect_ID FROM VSD_Defect 
                            INNER JOIN VSD_Defect_Category ON VSD_Defect.Defect_Category_ID = VSD_Defect_Category.Defect_Category_ID 
                            INNER JOIN   VSD_Veh_Cat_Defect ON VSD_Defect.Defect_ID = VSD_Veh_Cat_Defect.Defect_ID 
                            INNER JOIN   VSD_Veh_CAT_DEF_SEV_FINE ON VSD_Veh_Cat_Defect.VEH_CAT_DEFECT_ID = VSD_Veh_CAT_DEF_SEV_FINE.VEH_CAT_DEFECT_ID
                            Where (VSD_DEFECT_CATEGORY.Defect_category_ID IN ( Select DEFECT_CATEGORY_ID FROM VSD_DEFECT_CATEGORY WHERE CATEGORY_NAME = '" + OffenceCategory + @"')) 
                            and (VSD_Veh_Cat_Defect.IsDisabled = 'F')  and  ( VSD_DEFECT.defect_type = '" + recordType + @"') 
                            ORDER BY VSD_Defect.Defect_Code";
                App.VSDLog.Info("Query:" + sqlQuery);
                SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
                command = new SqlCeCommand(sqlQuery, con);
                dt = new DataTable();
                if (con.State == ConnectionState.Closed) { con.Open(); }
                dt.Load(command.ExecuteReader());

                con.Close();
                return dt;

            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
                return dt;
            }
        }

        string[] IDBDataLoad.GetDefects(string defect, string type, string recordType)
        {
            App.VSDLog.Info("Get Defect: 2" + defect + "///" + type + "//" + recordType);
            if (type.Equals("maincat") || type.Equals("subcat"))
            {
                // Schweers.Localization.ArabicConverter arab = new Schweers.Localization.ArabicConverter();

                SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
                string sqlQuery;
                SqlCeCommand command;
                if (type.Equals("maincat"))
                {
                    sqlQuery = "SELECT * FROM VSD_Defect_Category WHERE (Defect_Category_ID IN " +
                          "(SELECT     Parent_Defect_Category_ID FROM VSD_Defect_Category AS " +
                          " VSD_Defect_Category_1 WHERE (Defect_Category_ID IN (SELECT Defect_Category_ID " +
                          " FROM VSD_DEFECT WHERE (Defect_Type LIKE 'Defec%') AND (IsDisabled = 'F'))) AND (IsDisabled = 'F'))) AND (IsDisabled = 'F') ORDER BY Category_Code";

                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    command = new SqlCeCommand(sqlQuery, con);

                }
                else
                {
                    if (null == defect)
                    {
                        return new string[] { "" };
                    }

                    if (recordType.Equals("Defect"))
                    {

                        sqlQuery = "SELECT * FROM VSD_DEFECT_CATEGORY WHERE PARENT_Defect_CATEGORY_ID IN"
                          + " (Select DEFECT_CATEGORY_ID FROM VSD_DEFECT_CATEGORY WHERE CATEGORY_NAME = @defect) AND ISDISABLED ='F' ORDER BY Category_Code";
                        if (con.State == ConnectionState.Closed) { con.Open(); }
                        command = new SqlCeCommand(sqlQuery, con);
                        command.Parameters.Add("@defect", SqlDbType.NChar, 100).Value = defect;

                    }
                    else
                    {

                        sqlQuery = "SELECT * FROM VSD_Defect_Category " +
                        " WHERE (Defect_Category_ID IN (SELECT Defect_Category_ID FROM VSD_Defect " +
                        " WHERE (Defect_Type LIKE 'SAFETY%') AND (IsDisabled = 'F'))) AND (Parent_Defect_Category_ID IN " +
                        " (SELECT Defect_Category_ID " +
                        " FROM  VSD_Defect_Category AS VSD_Defect_Category_1 " +
                        " WHERE (Category_Name = @defect))) ORDER BY Category_Code";

                        if (con.State == ConnectionState.Closed) { con.Open(); }
                        command = new SqlCeCommand(sqlQuery, con);
                        command.Parameters.Add("@defect", SqlDbType.NChar).Value = defect;

                    }

                }


                App.VSDLog.Info("Query:" + sqlQuery);

                SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);

                int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;

                string[] defects = new string[rowCount];


                //    if (Resources.GetInstance().GetLocale().Equals(Resources.locale_EN))
                if (AppProperties.Selected_Resource == "English")
                {

                    if (rowCount > 0)
                    {

                        rs.ReadFirst();
                        defects[0] = rs.GetString(4);
                        if (rowCount > 1)
                        {
                            rs.Read();
                            for (int i = 1; i < rowCount; i++)
                            {
                                try
                                {
                                    defects[i] = rs.GetString(4);
                                    rs.Read();
                                }
                                catch (System.Data.SqlTypes.SqlNullValueException ex)
                                {
                                    // App.VSDLog.Info(ex.StackTrace);
                                    defects[i] = "Null"; continue;
                                }

                            }
                        }

                    }
                    rs.Close();
                    con.Close();
                    return defects;
                }
                else
                {

                    if (rowCount > 0)
                    {
                        rs.ReadFirst();
                        defects[0] = rs.GetString(5).Trim() + "^" + rs.GetString(4).Trim();
                        // defects[0] = "";
                        if (rowCount > 1)
                        {
                            rs.Read();
                            for (int i = 1; i < rowCount; i++)
                            {
                                try
                                {
                                    defects[i] = rs.GetString(5).Trim() + "^" + rs.GetString(4).Trim();
                                    // defects[i] = "";
                                    rs.Read();
                                }
                                catch (System.Data.SqlTypes.SqlNullValueException ex)
                                {
                                    //App.VSDLog.Info(ex.StackTrace);
                                    defects[i] = "Null";
                                    continue;
                                }

                            }
                        }
                    }
                    rs.Close();
                    con.Close();
                    return defects;
                }
            }
            else
            {
                // Schweers.Localization.ArabicConverter arab = new Schweers.Localization.ArabicConverter();

                SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
                string sqlQuery;
                SqlCeCommand command;
                if (null == defect)
                {
                    return new string[] { "" };
                }

                int vID = GetVehicleCategoryID(AppProperties.vehicle.VehicleCategory);

                sqlQuery = "SELECT Distinct VSD_DEFECT.DEFECT_NAME,VSD_DEFECT.DEFECT_NAME_A,VSD_DEFECT.Defect_Code FROM VSD_Defect INNER JOIN VSD_Defect_Category " +
                    "ON VSD_Defect.Defect_Category_ID = VSD_Defect_Category.Defect_Category_ID INNER JOIN   " +
                    "VSD_Veh_Cat_Defect ON VSD_Defect.Defect_ID = VSD_Veh_Cat_Defect.Defect_ID Where " +
                    "(VSD_DEFECT_CATEGORY.Defect_category_ID IN ( Select DEFECT_CATEGORY_ID FROM VSD_DEFECT_CATEGORY " +
                    "WHERE CATEGORY_NAME = @defect)) and (VSD_VEH_CAT_DEFECT.VEHICLE_CATEGORY_ID = @vID) and " +
                    " ( VSD_DEFECT.defect_type = @_defectType) AND (VSD_Veh_Cat_Defect.IsDisabled = 'F') AND (VSD_Defect.IsDisabled = 'F') ORDER BY VSD_Defect.Defect_Code";

                if (con.State == ConnectionState.Closed) { con.Open(); }
                command = new SqlCeCommand(sqlQuery, con);
                command.Parameters.Add("@defect", SqlDbType.NChar).Value = defect;
                command.Parameters.Add("@_defectType", SqlDbType.NChar).Value = recordType;
                command.Parameters.Add("@vID", SqlDbType.Int, 4).Value = vID;

                SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);

                int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;

                string[] defects = new string[rowCount];


                //  if (Resources.GetInstance().GetLocale().Equals(Resources.locale_EN))
                if (AppProperties.Selected_Resource == "English")
                {

                    if (rowCount > 0)
                    {

                        rs.ReadFirst();
                        defects[0] = rs.GetString(0);
                        if (rowCount > 1)
                        {
                            rs.Read();
                            for (int i = 1; i < rowCount; i++)
                            {
                                try
                                {
                                    defects[i] = rs.GetString(0);
                                    rs.Read();
                                }
                                catch (System.Data.SqlTypes.SqlNullValueException ex)
                                {
                                    // App.VSDLog.Info(ex.StackTrace);
                                    defects[i] = "No Defects"; continue;
                                }

                            }
                        }

                    }
                    rs.Close();
                    con.Close();
                    return defects;
                }
                else
                {

                    if (rowCount > 0)
                    {
                        rs.ReadFirst();
                        defects[0] = rs.GetString(1).Trim() + "^" + rs.GetString(0).Trim();
                        // defects[0] = "";
                        if (rowCount > 1)
                        {
                            rs.Read();
                            for (int i = 1; i < rowCount; i++)
                            {
                                try
                                {
                                    defects[i] = rs.GetString(1).Trim() + "^" + rs.GetString(0).Trim();
                                    //defects[0] = "";
                                    rs.Read();
                                }
                                catch (System.Data.SqlTypes.SqlNullValueException ex)
                                {
                                    // App.VSDLog.Info(ex.StackTrace);
                                    defects[i] = "No Defect";
                                    continue;
                                }

                            }
                        }
                    }
                    rs.Close();
                    con.Close();
                    return defects;
                }
            }
        }

        int GetVehicleCategoryID(string vehicleCategory)
        {
            SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
            string sqlQuery;

            if (con.State == ConnectionState.Closed) { con.Open(); }
            SqlCeCommand command;


            sqlQuery = "SELECT Vehicle_category_ID FROM VSD_VEHICLE_CATEGORY WHERE (Category_Name = @vehCat)";
            command = new SqlCeCommand(sqlQuery, con);

            command.Parameters.Add("@vehCat", SqlDbType.NChar, 100).Value = vehicleCategory;

            SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);

            int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;

            int id = -1;
            if (rs.Read())
            {
                id = (int)rs.GetInt32(0);
            }

            rs.Close();
            con.Close();
            return id;
        }


        string IDBDataLoad.GetAlternateVehicleSubCategory(string subcategory, string vehicleCategory)
        {
            App.VSDLog.Info("subcategory " + subcategory + "category -" + vehicleCategory); 
            SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
            string sqlQuery;

            if (con.State == ConnectionState.Closed) { con.Open(); }
            SqlCeCommand command;

            sqlQuery = "SELECT Category_Name_A FROM VSD_VEHICLE_CATEGORY WHERE (Category_Name = @vehCat) AND PARENT_VEHICLE_CATEGORY_ID IN " + " (SELECT Vehicle_Category_ID FROM VSD_VEHICLE_CATEGORY WHERE ISDISABLED ='F' and Category_Name='" + vehicleCategory + "')";

            //sqlQuery = "SELECT Category_Name_A FROM VSD_VEHICLE_CATEGORY WHERE (Category_Name = @vehCat)";
            command = new SqlCeCommand(sqlQuery, con);

            command.Parameters.Add("@vehCat", SqlDbType.NChar, 100).Value = subcategory;

            SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);

            int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;

            string alernateCategory = "";
            if (rs.Read())
            {
                alernateCategory = rs.GetString(0);
            }

            rs.Close();
            con.Close();
            return alernateCategory;
        }


        string IDBDataLoad.GetAlternateVehicleCategory(string category, string locale)
        {
            SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
            string sqlQuery;

            if (con.State == ConnectionState.Closed) { con.Open(); }
            SqlCeCommand command;



            sqlQuery = "SELECT Category_Name_A FROM VSD_VEHICLE_CATEGORY WHERE (Category_Name = @vehCat)";
            command = new SqlCeCommand(sqlQuery, con);

            command.Parameters.Add("@vehCat", SqlDbType.NChar, 100).Value = category;

            SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);

            int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;

            string alernateCategory = "";
            if (rs.Read())
            {
                alernateCategory = rs.GetString(0);
            }

            rs.Close();
            con.Close();
            return alernateCategory;
        }

        string IDBDataLoad.GetPlateSourceCode(string plateSource)
        {
            SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
            string sqlQuery;


            Regex regExp = new Regex("[a-zA-Z0-9 ]*");
            bool isEng = regExp.IsMatch(plateSource);

            if (con.State == ConnectionState.Closed) { con.Open(); }
            SqlCeCommand command;

            sqlQuery = "SELECT Country_Code FROM VSD_Country WHERE (Country_Name = @country)";
            command = new SqlCeCommand(sqlQuery, con);

            command.Parameters.Add("@country", SqlDbType.NChar, 50).Value = plateSource;


            SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);

            int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;

            string countryCode = "";
            if (rs.Read())
            {
                countryCode = rs.GetString(0).Trim();
            }

            rs.Close();
            con.Close();
            return countryCode;
        }


        string IDBDataLoad.GetPlateSourceCodeAr(string plateSource)
        {
            SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
            string sqlQuery;


            Regex regExp = new Regex("[a-zA-Z0-9 ]*");
            bool isEng = regExp.IsMatch(plateSource);

            if (con.State == ConnectionState.Closed) { con.Open(); }
            SqlCeCommand command;

            sqlQuery = "SELECT Country_Name_A FROM VSD_Country WHERE (Country_Name = @country)";
            command = new SqlCeCommand(sqlQuery, con);

            command.Parameters.Add("@country", SqlDbType.NChar, 50).Value = plateSource;


            SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);

            int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;

            string countryCode = "";
            if (rs.Read())
            {
                countryCode = rs.GetString(0).Trim();
            }

            rs.Close();
            con.Close();
            return countryCode;
        }


        string IDBDataLoad.GetPlateEmirate(string plateSource)
        {
            SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
            string sqlQuery;


            Regex regExp = new Regex("[a-zA-Z0-9 ]*");
            bool isEng = regExp.IsMatch(plateSource);

            if (con.State == ConnectionState.Closed) { con.Open(); }
            SqlCeCommand command;

            sqlQuery = "SELECT Country_Name FROM VSD_Country WHERE (Country_Code = @country)";
            command = new SqlCeCommand(sqlQuery, con);

            command.Parameters.Add("@country", SqlDbType.NChar).Value = plateSource;


            SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);

            int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;

            string countryCode = "";
            if (rs.Read())
            {
                countryCode = rs.GetString(0);
            }

            rs.Close();
            con.Close();
            return countryCode;
        }
        string IDBDataLoad.GetVehicleCategoryCode(string vehicleCatgeory)
        {
            SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
            string sqlQuery;

            Regex regExp = new Regex("[a-zA-Z0-9 ]*");
            bool isEng = regExp.IsMatch(vehicleCatgeory);

            if (con.State == ConnectionState.Closed) { con.Open(); }
            SqlCeCommand command;
            if (isEng)
                sqlQuery = "SELECT Category_Code FROM VSD_VEHICLE_CATEGORY WHERE (Category_Name = @vehCat)";
            else
                sqlQuery = "SELECT Category_Code FROM VSD_VEHICLE_CATEGORY WHERE (Category_Name_A = @vehCat)";
            command = new SqlCeCommand(sqlQuery, con);

            command.Parameters.Add("@vehCat", SqlDbType.NChar, 100).Value = vehicleCatgeory;



            SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);

            int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;

            string vehicleCategoryCode = "";
            if (rs.Read())
            {
                vehicleCategoryCode = rs.GetString(0);
            }

            rs.Close();
            con.Close();
            return vehicleCategoryCode;
        }

        string IDBDataLoad.GetVehicleSubCategoryCode(string subcategory, string category)
        {
            App.VSDLog.Info("subcategory " + subcategory + "category -" + category); 
            SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
            string sqlQuery;

            Regex regExp = new Regex("[a-zA-Z0-9 ]*");
            bool isEng = regExp.IsMatch(category);

            if (con.State == ConnectionState.Closed) { con.Open(); }
            SqlCeCommand command;
            if (isEng)
                sqlQuery = "SELECT Category_Code FROM VSD_VEHICLE_CATEGORY WHERE (Category_Name_A = @vehCat) AND PARENT_VEHICLE_CATEGORY_ID IN " + " (SELECT Vehicle_Category_ID FROM VSD_VEHICLE_CATEGORY WHERE Category_Name_A ='" + category + "')";
            else
                sqlQuery = "SELECT Category_Code FROM VSD_VEHICLE_CATEGORY WHERE (Category_Name_A = @vehCat) AND PARENT_VEHICLE_CATEGORY_ID IN " + " (SELECT Vehicle_Category_ID FROM VSD_VEHICLE_CATEGORY WHERE Category_Name_A ='" + category + "')";
            command = new SqlCeCommand(sqlQuery, con);

            command.Parameters.Add("@vehCat", SqlDbType.NChar, 100).Value = subcategory;



            SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);

            int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;

            string vehicleCategoryCode = "";
            if (rs.Read())
            {
                vehicleCategoryCode = rs.GetString(0);
            }

            rs.Close();
            con.Close();
            return vehicleCategoryCode;
        }

        List<Object> IDBDataLoad.GetStoredInspectionByTicketNumber(string vid)
        {
            SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
            string sqlQuery;

            List<Object> inspections = new List<object>();
            if (con.State == ConnectionState.Closed) { con.Open(); }
            SqlCeCommand command;

            sqlQuery = "SELECT * FROM VSD_Inspection where Violation_ID = @id";
            command = new SqlCeCommand(sqlQuery, con);
            command.Parameters.Add("@id", SqlDbType.NChar, 3).Value = vid;

            SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);

            int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;

            if (rowCount > 0)
            {
                rs.ReadFirst();

                for (int i = 0; i < rowCount; i++)
                {
                    string[] rows = new string[rs.ResultSetView.Columns.Length];
                    rows[0] = rs["Inspection_ID"].ToString();
                    rows[1] = rs["Vehicle_Info_ID"].ToString();
                    rows[2] = rs["Violation_ID"].ToString();
                    rows[3] = rs["Location_ID"].ToString();
                    rows[4] = rs["Plate_Condition"].ToString();
                    rows[5] = rs["Is_Reg_Exp"].ToString();
                    rows[6] = rs["Is_Plate_Confiscated"].ToString();
                    rows[7] = rs["Plate_Confiscation_Reason"].ToString();
                    rows[8] = rs["Plate_Confiscation_Reason_A"].ToString();
                    rows[9] = rs["Reported_Inspector_Name"].ToString();
                    rows[10] = rs["Reported_Inspector_Name_A"].ToString();
                    rows[11] = rs["Inspection_Timestamp"].ToString();

                    inspections.Insert(i, rows);
                    rs.Read();
                }

            }

            inspections.TrimExcess();
            rs.Close();
            con.Close();
            return inspections;
        }

        List<Object> IDBDataLoad.GetStoredInspection()
        {
            SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
            string sqlQuery;

            List<Object> inspections = new List<object>();
            if (con.State == ConnectionState.Closed) { con.Open(); }
            SqlCeCommand command;

            sqlQuery = "SELECT * FROM VSD_Inspection";
            command = new SqlCeCommand(sqlQuery, con);

            SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);

            int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;

            if (rowCount > 0)
            {
                rs.ReadFirst();

                for (int i = 0; i < rowCount; i++)
                {
                    string[] rows = new string[rs.ResultSetView.Columns.Length];
                    rows[0] = rs["Inspection_ID"].ToString();
                    rows[1] = rs["Vehicle_Info_ID"].ToString();
                    rows[2] = rs["Violation_ID"].ToString();
                    rows[3] = rs["Location_ID"].ToString();
                    rows[4] = rs["Plate_Condition"].ToString();
                    rows[5] = rs["Is_Reg_Exp"].ToString();
                    rows[6] = rs["Is_Plate_Confiscated"].ToString();
                    rows[7] = rs["Plate_Confiscation_Reason"].ToString();
                    rows[8] = rs["Plate_Confiscation_Reason_A"].ToString();
                    rows[9] = rs["Reported_Inspector_Name"].ToString();
                    rows[10] = rs["Reported_Inspector_Name_A"].ToString();
                    rows[11] = rs["Inspection_Timestamp"].ToString();

                    inspections.Insert(i, rows);
                    rs.Read();
                }

            }

            inspections.TrimExcess();
            rs.Close();
            con.Close();
            return inspections;
        }
        string IDBDataLoad.GetLocationCode(int id)
        {
            SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
            string sqlQuery;

            List<Object> inspections = new List<object>();
            if (con.State == ConnectionState.Closed) { con.Open(); }
            SqlCeCommand command;

            sqlQuery = "SELECT EXT_LOCATION_AREA_CODE FROM VSD_Location where location_id = @id";
            command = new SqlCeCommand(sqlQuery, con);
            command.Parameters.Add("@id", SqlDbType.Int, 4).Value = id;

            SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);

            int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;
            string externalCode = "";

            if (rowCount > 0)
            {
                rs.ReadFirst();
                externalCode = rs.GetString(0);

            }

            rs.Close();
            con.Close();
            return externalCode;
        }
        string[] IDBDataLoad.GetVehicleInfo(int id)
        {
            SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
            string sqlQuery;

            List<Object> inspections = new List<object>();
            if (con.State == ConnectionState.Closed) { con.Open(); }
            SqlCeCommand command;

            sqlQuery = "SELECT * FROM VSD_Vehicle_Info where Vehicle_Info_ID = @id";
            command = new SqlCeCommand(sqlQuery, con);
            command.Parameters.Add("@id", SqlDbType.Int, 4).Value = id;

            SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);
            int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;
            string[] vehicleInfo = new string[rs.ResultSetView.Columns.Length];

            if (rowCount > 0)
            {
                rs.ReadFirst();

                vehicleInfo[0] = rs["Vehicle_Info_ID"].ToString();
                vehicleInfo[1] = rs["Vehicle_Category_ID"].ToString();
                vehicleInfo[2] = rs["Vehicle_Plate_Category"].ToString();
                vehicleInfo[3] = rs["Vehicle_Plate_Code"].ToString();
                vehicleInfo[4] = rs["Vehicle_Plate_Source"].ToString();
                vehicleInfo[5] = rs["Vehicle_Plate_Number"].ToString();
                vehicleInfo[6] = rs["Vehicle_Country"].ToString();
                vehicleInfo[7] = rs["Vehicle_Model"].ToString();
                vehicleInfo[8] = rs["Vehicle_Model_A"].ToString();
                vehicleInfo[9] = rs["Make_Year"].ToString();
                vehicleInfo[10] = rs["Vehicle_Chassis_Number"].ToString();
                vehicleInfo[11] = rs["Vehicle_Reg_Expiry"].ToString();
                vehicleInfo[12] = rs["IsDisabled"].ToString();
                vehicleInfo[13] = rs["Timestamp"].ToString();
                vehicleInfo[14] = rs["Total_Impounding_Days"].ToString();
                vehicleInfo[15] = rs["Mileage"].ToString();
                vehicleInfo[16] = rs["Is_Grace_Period"].ToString();
                vehicleInfo[17] = rs["IsHazard"].ToString();

            }

            rs.Close();
            con.Close();
            return vehicleInfo;

        }

        string IDBDataLoad.GetVehicleCategoryCode(int id)
        {
            try
            {
                SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
                string sqlQuery;


                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCeCommand command;

                sqlQuery = "SELECT CATEGORY_CODE FROM VSD_VEHICLE_CATEGORY where VEHICLE_CATEGORY_ID = @id";
                command = new SqlCeCommand(sqlQuery, con);
                command.Parameters.Add("@id", SqlDbType.Int, 4).Value = id;

                SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);

                int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;
                string externalCode = "";

                if (rowCount > 0)
                {
                    rs.ReadFirst();
                    externalCode = rs.GetString(0);

                }

                rs.Close();
                con.Close();
                return externalCode;

            }
            catch (Exception e)
            {
                // App.VSDLog.Info(e.StackTrace);
                return null;
            }

        }

        string[] IDBDataLoad.GetOwnerInfo(int vehInfoID)
        {
            SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
            string sqlQuery;


            if (con.State == ConnectionState.Closed) { con.Open(); }
            SqlCeCommand command;

            sqlQuery = "SELECT * FROM VSD_OWNER_INFO where VEHICLE_INFO_ID = @id";
            command = new SqlCeCommand(sqlQuery, con);
            command.Parameters.Add("@id", SqlDbType.Int, 4).Value = vehInfoID;

            SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);

            int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;
            string[] row = new string[rs.ResultSetView.Columns.Length];

            if (rowCount > 0)
            {
                rs.ReadFirst();
                row[0] = rs["Owner_Info_ID"].ToString();
                row[1] = rs["Vehicle_Info_ID"].ToString();
                row[2] = rs["Owner_Name"].ToString();
                row[3] = rs["Owner_Name_A"].ToString();
                row[4] = rs["Trade_License_Number"].ToString();
                row[5] = rs["Traffic_File_Number"].ToString();
                row[6] = rs["IsDisabled"].ToString();
                row[7] = rs["Timestamp"].ToString();

            }

            rs.Close();
            con.Close();
            return row;
        }
        string[] IDBDataLoad.GetDriverInfo(int vehInfoID)
        {
            SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
            string sqlQuery;


            if (con.State == ConnectionState.Closed) { con.Open(); }
            SqlCeCommand command;

            sqlQuery = "SELECT * FROM VSD_Driver_INFO where VEHICLE_INFO_ID = @id";
            command = new SqlCeCommand(sqlQuery, con);
            command.Parameters.Add("@id", SqlDbType.Int, 4).Value = vehInfoID;

            SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);

            int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;
            string[] row = new string[rs.ResultSetView.Columns.Length];

            if (rowCount > 0)
            {
                rs.ReadFirst();
                row[0] = rs["Driver_Info_ID"].ToString();
                row[1] = rs["Vehicle_Info_ID"].ToString();
                row[2] = rs["Driver_Name"].ToString();
                row[3] = rs["Driver_Name_A"].ToString();
                row[4] = rs["Driver_License_Number"].ToString();
                row[5] = rs["IsDisabled"].ToString();
                row[6] = rs["Timestamp"].ToString();

            }

            rs.Close();
            con.Close();
            return row;
        }
        string[] IDBDataLoad.GetViolationByTicketCode(string ticeketCode)
        {
            SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
            string sqlQuery;


            if (con.State == ConnectionState.Closed) { con.Open(); }
            SqlCeCommand command;

            sqlQuery = "SELECT * FROM VSD_VIOLATION where VIOLATION_Ticket_Code = @id";
            command = new SqlCeCommand(sqlQuery, con);
            command.Parameters.Add("@id", SqlDbType.NChar, 50).Value = ticeketCode;

            SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);

            int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;
            string[] row = new string[rs.ResultSetView.Columns.Length];

            if (rowCount > 0)
            {
                rs.ReadFirst();
                row[0] = rs["Violation_ID"].ToString();
                row[1] = rs["Due_Date"].ToString();
                row[2] = rs["Comments"].ToString();
                row[3] = rs["Comments_A"].ToString();
                row[4] = rs["Fine_Amount"].ToString();
                row[5] = rs["Timestamp"].ToString();
                row[6] = rs["Violation_Status"].ToString();
                row[7] = rs["Created_By"].ToString();
                row[8] = rs["Severity_Level_ID"].ToString();

            }

            rs.Close();
            con.Close();
            return row;
        }
        string[] IDBDataLoad.GetViolation(int inspectionID)
        {
            SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
            string sqlQuery;


            if (con.State == ConnectionState.Closed) { con.Open(); }
            SqlCeCommand command;

            sqlQuery = "SELECT * FROM VSD_VIOLATION where VIOLATION_ID = @id";
            command = new SqlCeCommand(sqlQuery, con);
            command.Parameters.Add("@id", SqlDbType.Int, 4).Value = inspectionID;

            SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);

            int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;
            string[] row = new string[rs.ResultSetView.Columns.Length];

            if (rowCount > 0)
            {
                rs.ReadFirst();
                row[0] = rs["Violation_Ticket_Code"].ToString();
                row[1] = rs["Due_Date"].ToString();
                row[2] = rs["Comments"].ToString();
                row[3] = rs["Comments_A"].ToString();
                row[4] = rs["Fine_Amount"].ToString();
                row[5] = rs["Timestamp"].ToString();
                row[6] = rs["Violation_Status"].ToString();
                row[7] = rs["Created_By"].ToString();

            }

            rs.Close();
            con.Close();
            return row;
        }

        string[][] IDBDataLoad.GetDefectCodesForViolation(int violationID)
        {
            SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
            string sqlQuery;


            if (con.State == ConnectionState.Closed) { con.Open(); }
            SqlCeCommand command;

            sqlQuery = "SELECT DISTINCT VSD_DEFECT.DEFECT_CODE, VSD_Channel_Defect.Defect_Value FROM VSD_Channel_Defect INNER JOIN VSD_Defect ON VSD_Channel_Defect.Defect_ID = VSD_Defect.Defect_ID" +
                " WHERE     (VSD_Channel_Defect.Violation_ID = @id)";
            command = new SqlCeCommand(sqlQuery, con);
            command.Parameters.Add("@id", SqlDbType.Int, 4).Value = violationID;

            SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);

            int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;
            string[][] row = new string[rowCount][];

            if (rowCount > 0)
            {
                rs.ReadFirst();
                for (int i = 0; i < rowCount; i++)
                {
                    row[i] = new string[2];
                    row[i][0] = rs.GetString(0).Trim();
                    row[i][1] = rs.GetString(1).Trim();
                    rs.Read();
                }
            }

            rs.Close();
            con.Close();
            return row;
        }

        string IDBDataLoad.GetLocationCode(string emirate, string area, string location)
        {
            try
            {
                SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
                string sqlQuery;
                Regex regExp = new Regex("[a-zA-Z0-9 ]*");
                bool isEng = regExp.IsMatch(emirate);

                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCeCommand command = null;


                sqlQuery = "SELECT EXT_LOCATION_AREA_CODE " +
                            " FROM VSD_Location WHERE (Location_Area_Name = @loc) AND (Parent_Location_ID IN" +
                            " (SELECT Location_ID FROM VSD_Location AS VSD_LOCATION_1" +
                            " WHERE (Location_Area_Name = @area) AND (Country_ID IN" +
                            " (SELECT Country_ID FROM VSD_Country WHERE (Country_Name = @emirate)))))";


                command = new SqlCeCommand(sqlQuery, con);
                command.Parameters.Add("@emirate", SqlDbType.NChar, 50).Value = emirate;
                command.Parameters.Add("@area", SqlDbType.NChar, 200).Value = area;
                command.Parameters.Add("@loc", SqlDbType.NChar, 200).Value = location;




                SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);

                int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;
                string externalCode = "";

                if (rowCount > 0)
                {
                    rs.ReadFirst();
                    externalCode = rs.GetString(0);

                }

                rs.Close();
                con.Close();
                return externalCode;

            }
            catch (Exception e)
            {
                // App.VSDLog.Info(e.StackTrace);
                return null;
            }
        }
        string IDBDataLoad.GetDefectCode(int defectID)
        {
            try
            {
                SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
                string sqlQuery;


                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCeCommand command;

                sqlQuery = "SELECT VSD_DEFECT.DEFECT_CODE FROM VSD_Defect " +
                            " WHERE (DEFECT_ID = @id)";
                command = new SqlCeCommand(sqlQuery, con);
                command.Parameters.Add("@id", SqlDbType.Int, 4).Value = defectID;

                SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);

                int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;
                string externalCode = "";

                if (rowCount > 0)
                {
                    rs.ReadFirst();
                    externalCode = rs.GetString(0);

                }

                rs.Close();
                con.Close();
                return externalCode;

            }
            catch (Exception e)
            {
                //App.VSDLog.Info(e.StackTrace);
                return null;
            }
        }
        int IDBDataLoad.GetDefectIDByCode(string defectCode)
        {
            try
            {
                SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
                string sqlQuery;


                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCeCommand command;

                sqlQuery = "SELECT VSD_DEFECT.DEFECT_ID FROM VSD_Defect " +
                            " WHERE (DEFECT_Code = @id)";
                command = new SqlCeCommand(sqlQuery, con);
                command.Parameters.Add("@id", SqlDbType.NChar, 25).Value = defectCode;

                SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);

                int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;
                int id = -1;

                if (rowCount > 0)
                {
                    rs.ReadFirst();
                    id = (int)rs.GetInt32(0);

                }

                rs.Close();
                con.Close();
                return id;

            }
            catch (Exception e)
            {
                // App.VSDLog.Info(e.StackTrace);
                return -1;
            }
        }
        string IDBDataLoad.GetPlateEmirateByCode(string plateSource)
        {
            SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
            string sqlQuery;


            Regex regExp = new Regex("[a-zA-Z0-9 ]*");
            bool isEng = regExp.IsMatch(plateSource);

            if (con.State == ConnectionState.Closed) { con.Open(); }
            SqlCeCommand command;

            sqlQuery = "SELECT Country_Name FROM VSD_Country WHERE (Country_Code = @source)";
            command = new SqlCeCommand(sqlQuery, con);

            command.Parameters.Add("@source", SqlDbType.NChar, 25).Value = plateSource;

            SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);

            int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;

            string countryCode = "";
            if (rs.Read())
            {
                countryCode = rs.GetString(0);
            }

            rs.Close();
            con.Close();
            return countryCode;
        }
        string IDBDataLoad.GetSeverityByID(int sevID, string type)
        {
            SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
            string sqlQuery;

            if (con.State == ConnectionState.Closed) { con.Open(); }
            SqlCeCommand command;



            sqlQuery = "SELECT Severity_Level_Name FROM VSD_Severity_Level WHERE SEVERITY_LEVEL_ID = @sevID";
            command = new SqlCeCommand(sqlQuery, con);
            command.Parameters.Add("@sevName", SqlDbType.Int, 4).Value = sevID;

            SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);

            int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;

            string sevName = "";
            if (rs.Read())
            {
                sevName = rs.GetString(0);
            }

            rs.Close();
            con.Close();
            return sevName;
        }

        string[] IDBDataLoad.GetSeverityNamesByID(int sevID)
        {
            SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
            string sqlQuery;


            if (con.State == ConnectionState.Closed) { con.Open(); }
            SqlCeCommand command;

            sqlQuery = "SELECT * FROM VSD_Severity_Level where Severity_Level_ID = @id";
            command = new SqlCeCommand(sqlQuery, con);
            command.Parameters.Add("@id", SqlDbType.NChar, 50).Value = sevID;

            SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);

            int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;
            string[] row = new string[rs.ResultSetView.Columns.Length];

            if (rowCount > 0)
            {
                rs.ReadFirst();
                row[0] = rs["Severity_Level"].ToString();
                row[1] = rs["Severity_Level_Name"].ToString();
                row[2] = rs["Severity_Level_Name_A"].ToString();


            }

            rs.Close();
            con.Close();
            return row;
        }

        string[] IDBDataLoad.GetDefectSeverityByDefectIDVehCat(int defectID, int vehicleCatID)
        {
            SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
            string sqlQuery;


            if (con.State == ConnectionState.Closed) { con.Open(); }
            SqlCeCommand command;

            sqlQuery = "SELECT VCD.Severity_Level_ID, VSL.Severity_Level_Name, VSL.Severity_level_Name_A from VSD_Veh_Cat_defect VCD, VSD_Severity_Level VSL where VCD.Severity_Level_ID = VSL.Severity_Level_ID AND VCD.Defect_ID  = @id AND VCD.Vehicle_category_ID = @vehicleCatID";
            command = new SqlCeCommand(sqlQuery, con);
            command.Parameters.Add("@id", SqlDbType.Int, 10).Value = defectID;
            command.Parameters.Add("@vehicleCatID", SqlDbType.Int, 10).Value = vehicleCatID;

            SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);

            int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;
            string[] row = new string[rs.ResultSetView.Columns.Length];

            if (rowCount > 0)
            {
                rs.ReadFirst();
                row[0] = rs["Severity_Level_ID"].ToString();
                row[1] = rs["Severity_Level_Name"].ToString();
                row[2] = rs["Severity_Level_Name_A"].ToString();

            }

            rs.Close();
            con.Close();
            return row;
        }
        string IDBDataLoad.GetAlternateCity(string name)
        {
            SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
            string sqlQuery;

            if (con.State == ConnectionState.Closed) { con.Open(); }
            SqlCeCommand command;
            Regex regExp = new Regex("[A-Za-z09]*");
            bool isEng = regExp.IsMatch(name);



            sqlQuery = "SELECT Country_Name_A FROM  VSD_Country WHERE (Country_Name = @name)";
            command = new SqlCeCommand(sqlQuery, con);
            command.Parameters.Add("@name", SqlDbType.NChar, 50).Value = name;

            SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);

            int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;

            string sevName = "";
            if (rs.Read())
            {
                sevName = rs.GetString(0);
            }

            rs.Close();
            con.Close();
            return sevName;
        }
        DateTime IDBDataLoad.GetAppSynTime()
        {
            SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
            string sqlQuery;

            if (con.State == ConnectionState.Closed) { con.Open(); }
            SqlCeCommand command;
           sqlQuery = "SELECT Timestamp FROM  VSD_PROPERTY_TYPE WHERE (PROPERTY_TYPE_NAME = 'APPUPDATETIME')";
            command = new SqlCeCommand(sqlQuery, con);

            SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);

            int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;

            DateTime dateTime = new DateTime();
            if (rs.Read())
            {
                dateTime = rs.GetDateTime(0);
            }

            rs.Close();
            con.Close();

            return dateTime;

        }
        void IDBDataLoad.UpdateApplicationSynTime()
        {
            try
            {
                SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
                string sqlQuery;

                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCeCommand command;
                sqlQuery = "UPDATE  VSD_Property_Type SET Timestamp = @time WHERE (Property_Type_Name = 'APPUPDATETIME')";
                command = new SqlCeCommand(sqlQuery, con);
                command.Parameters.Add("@time", SqlDbType.DateTime, 8).Value = System.DateTime.Now;

                int rowCount = command.ExecuteNonQuery();

                if (rowCount > 0)
                {
                    //  if (Resources.GetInstance().GetLocale().Equals(Resources.locale_EN))
                    if (AppProperties.Selected_Resource == "English")
                    {

                    }
                    else
                    {

                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        string[][] IDBDataLoad.GetTestType(string severity, int defectsNo)
        {
            string[][] testType;

            SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
            string sqlQuery;

            if (con.State == ConnectionState.Closed) { con.Open(); }
            SqlCeCommand command;
            sqlQuery = "SELECT DISTINCT VSD_Test_Type.Test_Type_Name, VSD_Test_Type.Test_Type_Name_A" +
                       " FROM VSD_Test_Type INNER JOIN " +
                       " VSD_Vio_Sev_Fee_Rule ON VSD_Test_Type.Test_Type_ID = VSD_Vio_Sev_Fee_Rule.Test_Type_ID INNER JOIN " +
                       " VSD_Severity_Level ON VSD_Vio_Sev_Fee_Rule.Severity_Level_ID = VSD_Severity_Level.Severity_Level_ID " +
                       " WHERE (VSD_Severity_Level.Severity_Level_ID IN (SELECT Severity_Level_ID " +
                       " FROM VSD_Severity_Level AS VSD_Severity_Level_1 " +
                       " WHERE (Severity_Level_Name = @sev))) AND (@noDefects BETWEEN VSD_Vio_Sev_Fee_Rule.Min_No_Of_Defects AND  " +
                       " VSD_Vio_Sev_Fee_Rule.Max_No_Of_Defects)  AND (VSD_Vio_Sev_Fee_Rule.Test_Attempts = '1') AND " +
                      " (VSD_Vio_Sev_Fee_Rule.Is_Under_DueDate = 'F') AND (VSD_Vio_Sev_Fee_Rule.Is_Under_Suspension_DueDate = 'F')";


            command = new SqlCeCommand(sqlQuery, con);
            command.Parameters.Add("@sev", SqlDbType.NChar).Value = severity;
            command.Parameters.Add("@noDefects", SqlDbType.Int).Value = defectsNo;



            SqlCeResultSet res = command.ExecuteResultSet(ResultSetOptions.Scrollable);

            testType = new string[1][];

            testType[0] = new string[2];
            if (res.HasRows)
            {
                res.ReadFirst();

                testType[0][0] = res.GetString(0);
                testType[0][1] = res.GetString(1);

            }

            return testType;

        }

        void IDBDataLoad.SetDefaultConfiguration()
        {

            //update default Emirate
            SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
            string sqlQuery;
            SqlCeResultSet rs;
            SqlCeResultSet res;

            sqlQuery = "SELECT Country_ID FROM VSD_Country_Prop WHERE (IsDefault = 'T')";
            if (con.State == ConnectionState.Closed) { con.Open(); }
            SqlCeCommand getDefaultCountry = new SqlCeCommand(sqlQuery, con);
            rs = getDefaultCountry.ExecuteResultSet(ResultSetOptions.Scrollable);
            if (rs.HasRows)
            {
                sqlQuery = "SELECT Country_Name,Country_Name_A FROM VSD_Country WHERE (Country_ID = @CountryID)";
                rs.Read();
                getDefaultCountry.Parameters.Add("@CountryID", SqlDbType.Int).Value = (int)rs.GetInt32(0);
                rs.Close();
                getDefaultCountry.CommandText = sqlQuery;
                rs = getDefaultCountry.ExecuteResultSet(ResultSetOptions.Scrollable);
                rs.ReadFirst();
                AppProperties.defaultEmirate = rs.GetString(0).Trim();
                AppProperties.defaultEmirateAr = rs.GetString(1).Trim();
                rs.Close();
            }

            //update default country

            sqlQuery = "SELECT Parent_Country_ID FROM VSD_Country WHERE (Country_Name = @emirate)";
            if (con.State == ConnectionState.Closed) { con.Open(); }
            getDefaultCountry = new SqlCeCommand(sqlQuery, con);
            getDefaultCountry.Parameters.Add("@emirate", SqlDbType.NChar).Value = AppProperties.defaultEmirate;
            res = getDefaultCountry.ExecuteResultSet(ResultSetOptions.Scrollable);
            if (res.HasRows)
            {
                sqlQuery = "SELECT Country_Name,Country_Name_A FROM VSD_Country WHERE (Country_ID = @CountryID)";
                res.Read();
                getDefaultCountry.Parameters.Add("@CountryID", SqlDbType.Int).Value = (int)res.GetInt32(0);
                res.Close();
                getDefaultCountry.CommandText = sqlQuery;
                res = getDefaultCountry.ExecuteResultSet(ResultSetOptions.Scrollable);
                res.ReadFirst();
                AppProperties.defaultCountry = res.GetString(0).Trim();
                AppProperties.defaultCountryAr = res.GetString(1).Trim();
                res.Close();
            }

        }

        string IDBDataLoad.GetVehicleCategoryNameByCode(string code)
        {
            try
            {
                SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
                string sqlQuery;


                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCeCommand command;

                sqlQuery = "SELECT CATEGORY_NAME FROM VSD_VEHICLE_CATEGORY where CATEGORY_CODE = @code";
                command = new SqlCeCommand(sqlQuery, con);
                command.Parameters.Add("@code", SqlDbType.NChar).Value = code;

                SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);

                int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;
                string externalCode = "";

                if (rowCount > 0)
                {
                    rs.ReadFirst();
                    externalCode = rs.GetString(0);

                }

                rs.Close();
                con.Close();
                return externalCode;

            }
            catch (Exception e)
            {
                // App.VSDLog.Info(e.StackTrace);
                return null;
            }
        }

        string[] IDBDataLoad.GetSafetyDefects()
        {
            // Schweers.Localization.ArabicConverter arab = new Schweers.Localization.ArabicConverter();
            SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
            string sqlQuery;
            SqlCeCommand command;




            sqlQuery = "SELECT Category_Name,Category_Name_A FROM VSD_Defect_Category WHERE (Defect_Category_ID IN " +
                          "(SELECT     Parent_Defect_Category_ID FROM VSD_Defect_Category AS " +
                          " VSD_Defect_Category_1 WHERE (Defect_Category_ID IN (SELECT Defect_Category_ID " +
                          " FROM VSD_DEFECT WHERE (Defect_Type LIKE 'SAFETY%') AND (IsDisabled = 'F'))) AND (IsDisabled = 'F'))) AND (IsDisabled = 'F')";

            if (con.State == ConnectionState.Closed) { con.Open(); }
            command = new SqlCeCommand(sqlQuery, con);




            SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);

            int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;

            string[] defects = new string[rowCount];


            //  if (Resources.GetInstance().GetLocale().Equals(Resources.locale_EN))
            if (AppProperties.Selected_Resource == "English")
            {

                if (rowCount > 0)
                {

                    rs.ReadFirst();
                    defects[0] = rs.GetString(0);
                    if (rowCount > 1)
                    {
                        rs.Read();
                        for (int i = 1; i < rowCount; i++)
                        {
                            try
                            {
                                defects[i] = rs.GetString(0);
                                rs.Read();
                            }
                            catch (System.Data.SqlTypes.SqlNullValueException ex)
                            {
                                //App.VSDLog.Info(ex.StackTrace);
                                defects[i] = "Null"; continue;
                            }

                        }
                    }

                }
                rs.Close();
                con.Close();

                return defects;
            }

            else
            {

                if (rowCount > 0)
                {

                    rs.ReadFirst();
                    defects[0] = rs.GetString(1).Trim() + "^" + rs.GetString(0).Trim();
                    // defects[0] = "";
                    if (rowCount > 1)
                    {
                        rs.Read();
                        for (int i = 1; i < rowCount; i++)
                        {
                            try
                            {
                                defects[i] = rs.GetString(1).Trim() + "^" + rs.GetString(0).Trim();
                                //defects[0] = "";
                                rs.Read();
                            }
                            catch (System.Data.SqlTypes.SqlNullValueException ex)
                            {
                                // App.VSDLog.Info(ex.StackTrace);
                                defects[i] = "Null"; continue;
                            }

                        }
                    }

                }
                rs.Close();
                con.Close();
                return defects;

            }
            //return new string[0];
        }

        bool IDBDataLoad.CanConfiscatePlate(string severity)
        {
            return true;
        }


        string[][] IDBDataLoad.GetDefectMainCategory()
        {
            string[][] defectCats;

            SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
            string sqlQuery;

            if (con.State == ConnectionState.Closed) { con.Open(); }
            SqlCeCommand command;
            sqlQuery = "SELECT Category_Name,Category_Name_A FROM VSD_Defect_Category WHERE (Defect_Category_ID IN " +
                          "(SELECT     Parent_Defect_Category_ID FROM VSD_Defect_Category AS " +
                          " VSD_Defect_Category_1 WHERE (Defect_Category_ID IN (SELECT Defect_Category_ID " +
                          " FROM VSD_DEFECT WHERE (Defect_Type LIKE 'Defec%') AND (IsDisabled = 'F'))) AND (IsDisabled = 'F'))) AND (IsDisabled = 'F')";

            command = new SqlCeCommand(sqlQuery, con);

            SqlCeResultSet res = command.ExecuteResultSet(ResultSetOptions.Scrollable);
            int rowCount = ((System.Collections.ICollection)res.ResultSetView).Count;
            defectCats = new string[rowCount][];


            if (rowCount > 0)
            {
                res.ReadFirst();
                defectCats[0] = new string[2];
                defectCats[0][0] = res.GetString(0);
                defectCats[0][1] = res.GetString(1);

                if (rowCount > 1)
                {
                    res.Read();

                    for (int i = 1; i < rowCount; i++)
                    {

                        defectCats[i] = new string[2];
                        defectCats[i][0] = res.GetString(0);
                        defectCats[i][1] = res.GetString(1);
                        res.Read();
                    }
                }

            }

            return defectCats;
        }

        string[][] IDBDataLoad.GetDefectMainCategoryByID(string defectID)
        {
            string[][] defectCats;

            SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
            string sqlQuery;

            if (con.State == ConnectionState.Closed) { con.Open(); }
            SqlCeCommand command;

            //  sqlQuery = "SELECT Category_Name,Category_Name_A FROM VSD_Defect_Category WHERE Defect_Category_ID =(SELECT Parent_Defect_Category_ID FROM VSD_Defect_Category WHERE Defect_Category_ID = (SELECT Defect_Category_ID FROM VSD_DEFECT WHERE Defect_ID = @defectID AND IsDisabled = 'F') AND IsDisabled = 'F') AND IsDisabled = 'F'";

            sqlQuery = "SELECT Category_Name,Category_Name_A FROM VSD_Defect_Category WHERE (Defect_Category_ID IN " +
                         "(SELECT     Parent_Defect_Category_ID FROM VSD_Defect_Category AS " +
                         " VSD_Defect_Category_1 WHERE (Defect_Category_ID IN (SELECT Defect_Category_ID " +
                         " FROM VSD_DEFECT WHERE (Defect_Code = @defectID) AND (IsDisabled = 'F'))) AND (IsDisabled = 'F'))) AND (IsDisabled = 'F')";

            command = new SqlCeCommand(sqlQuery, con);
            command.Parameters.Add("@defectID", SqlDbType.NChar, 25).Value = defectID;

            SqlCeResultSet res = command.ExecuteResultSet(ResultSetOptions.Scrollable);
            int rowCount = ((System.Collections.ICollection)res.ResultSetView).Count;
            defectCats = new string[rowCount][];


            if (rowCount > 0)
            {
                res.ReadFirst();
                defectCats[0] = new string[2];
                defectCats[0][0] = res.GetString(0);
                defectCats[0][1] = res.GetString(1);

                if (rowCount > 1)
                {
                    res.Read();

                    for (int i = 1; i < rowCount; i++)
                    {

                        defectCats[i] = new string[2];
                        defectCats[i][0] = res.GetString(0);
                        defectCats[i][1] = res.GetString(1);
                        res.Read();
                    }
                }

            }

            return defectCats;
        }

        int IDBDataLoad.GetDefectCountPerCategory(string defectCat, string defectCodes)
        {
            try
            {
                SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
                string sqlQuery;


                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCeCommand command;


                sqlQuery = "SELECT COUNT(*) AS Expr1 FROM         VSD_Defect_Category INNER JOIN " +
                           " VSD_Defect ON VSD_Defect_Category.Defect_Category_ID = VSD_Defect.Defect_Category_ID " +
                           " WHERE (VSD_Defect.Defect_ID IN " + defectCodes + ") AND (VSD_Defect_Category.Parent_Defect_Category_ID IN " +
                           " (SELECT Defect_Category_ID FROM          VSD_Defect_Category AS VSD_Defect_Category_1 " +
                           " WHERE (Category_Name = @defectCat)))";
                command = new SqlCeCommand(sqlQuery, con);
                command.Parameters.Add("@defectCat", SqlDbType.NChar).Value = defectCat;

                SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);

                int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;
                int id = -1;

                if (rowCount > 0)
                {
                    rs.ReadFirst();
                    id = (int)rs.GetInt32(0);

                }

                rs.Close();
                con.Close();
                return id;

            }
            catch (Exception e)
            {
                // App.VSDLog.Info(e.StackTrace);
                return -1;
            }

        }

        string IDBDataLoad.GetRVDefectCodes()
        {
            string start = "(";
            string end = ")";
            string value = "'-2000'";
            bool firstEntry = true;
            if (AppProperties.rvDefectCodes == "('-2000')")
            {
                if (null != AppProperties.recordedViolation && AppProperties.recordedViolation.Defect.Length > 0)
                {
                    for (int i = 0; i < AppProperties.recordedViolation.Defect.Length; i++)
                    {

                        if (firstEntry)
                        {
                            value = "" + AppProperties.recordedViolation.Defect[i].DefectID + "";
                            firstEntry = false;
                        }
                        else
                        {
                            value += "," + AppProperties.recordedViolation.Defect[i].DefectID + "";
                        }



                    }
                    AppProperties.rvDefectCodes = start + value + end;

                }
                return AppProperties.rvDefectCodes;
            }
            else
            {
                return AppProperties.rvDefectCodes;
            }


        }

        bool IDBDataLoad.SetAuthCode(string authCode)
        {
            SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
            string sqlQuery;


            if (con.State == ConnectionState.Closed) { con.Open(); }
            SqlCeCommand command;

            sqlQuery = "SELECT * from VSD_PROPERTY_TYPE WHERE PROPERTY_TYPE_NAME = 'AUTHCODE'";
            command = new SqlCeCommand(sqlQuery, con);
            SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);

            try
            {
                if (rs.HasRows)
                {
                    sqlQuery = "UPDATE VSD_PROPERTY_TYPE SET PROPERTY_TYPE_CODE = @authCode WHERE PROPERTY_TYPE_NAME = 'AUTHCODE'";
                    command = new SqlCeCommand(sqlQuery, con);
                    command.Parameters.Add("@authCode", SqlDbType.NChar).Value = AppProperties.authorityCode;
                    command.ExecuteNonQuery();
                    con.Close();
                }
                else
                {
                    command = new SqlCeCommand("select property_type_id from vsd_property_type order by property_type_id desc", con);
                    SqlCeDataReader tempReader = command.ExecuteReader();
                    if (tempReader.Read())
                    {
                        sqlQuery = "INSERT INTO VSD_PROPERTY_TYPE(PROPERTY_TYPE_CODE,PROPERTY_TYPE_ID,PROPERTY_TYPE_NAME,PROPERTY_TYPE_DESC) VALUES (@authCode,@id,'AUTHCODE','AUTHORITY CODE') ";
                        command = new SqlCeCommand(sqlQuery, con);
                        command.Parameters.Add("@authCode", SqlDbType.NChar, 100).Value = AppProperties.authorityCode;
                        command.Parameters.Add("@id", SqlDbType.Int).Value = Int32.Parse(tempReader[0].ToString()) + 1;
                        command.ExecuteNonQuery();
                    }
                    else
                    {
                        sqlQuery = "INSERT INTO VSD_PROPERTY_TYPE(PROPERTY_TYPE_CODE,PROPERTY_TYPE_ID,PROPERTY_TYPE_NAME,PROPERTY_TYPE_DESC) VALUES (@authCode,6,'AUTHCODE','AUTHORITY CODE') ";
                        command = new SqlCeCommand(sqlQuery, con);
                        command.Parameters.Add("@authCode", SqlDbType.NChar, 100).Value = AppProperties.authorityCode;
                        command.ExecuteNonQuery();
                    }


                    con.Close();
                }
                return true;
            }
            catch (Exception e)
            {
                con.Close();
                // App.VSDLog.Info(e.StackTrace);
                return false;
            }
        }


        string IDBDataLoad.GetAuthCode()
        {
            SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
            string sqlQuery;

            if (con.State == ConnectionState.Closed) { con.Open(); }
            SqlCeCommand command;

            try
            {
                sqlQuery = "SELECT * from VSD_PROPERTY_TYPE WHERE PROPERTY_TYPE_NAME = AUTHCODE";
                command = new SqlCeCommand(sqlQuery, con);
                SqlCeDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    return reader.GetString(5);
                }
            }
            catch (Exception ex)
            {
                // App.VSDLog.Info(ex.StackTrace);
                return null;
            }

            con.Close();
            return null;
        }
        void IDBDataLoad.GetCountryProperties(string emirate)
        {
            // Schweers.Localization.ArabicConverter arab = new Schweers.Localization.ArabicConverter();
            SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
            string sqlQuery;
            SqlCeCommand command;




            sqlQuery = "SELECT VSD_Country_Prop.Can_Fetch_Info, VSD_Country_Prop.Can_Raise_Violation, VSD_Country_Prop.Can_Inspect, " +
                  " VSD_Country_Prop.Can_Print_Viol_Notice, VSD_Country_Prop.Can_Confiscate_Plates " +
                  " FROM VSD_Country_Prop INNER JOIN VSD_Country ON VSD_Country_Prop.Country_ID = VSD_Country.Country_ID " +
                  " WHERE (VSD_Country.Country_Name = @emirate)";
            if (con.State == ConnectionState.Closed) { con.Open(); }
            command = new SqlCeCommand(sqlQuery, con);
            command.Parameters.Add("@emirate", SqlDbType.NVarChar).Value = emirate;

            SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);
            int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;


            if (rowCount > 0)
            {
                rs.ReadFirst();
                AppProperties.canFetchInfo = (rs.GetString(0).Equals("T", StringComparison.CurrentCultureIgnoreCase)) ? true : false;
                AppProperties.canRaiseViolation = (rs.GetString(1).Equals("T", StringComparison.CurrentCultureIgnoreCase)) ? true : false;
                AppProperties.canInspect = (rs.GetString(2).Equals("T", StringComparison.CurrentCultureIgnoreCase)) ? true : false;
                AppProperties.canPrintViolation = (rs.GetString(3).Equals("T", StringComparison.CurrentCultureIgnoreCase)) ? true : false;
                AppProperties.canConfiscatePlates = (rs.GetString(4).Equals("T", StringComparison.CurrentCultureIgnoreCase)) ? true : false;



            }
            rs.Close();
            con.Close();

        }

        string IDBDataLoad.GetDefectNameByID(string code)
        {
            try
            {
                SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
                string sqlQuery;


                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCeCommand command;

                sqlQuery = "SELECT Defect_Name FROM VSD_Defect WHERE (Defect_Code = @code)";
                command = new SqlCeCommand(sqlQuery, con);
                command.Parameters.Add("@code", SqlDbType.NChar).Value = code;

                SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);

                int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;
                string externalCode = "";

                if (rowCount > 0)
                {
                    rs.ReadFirst();
                    externalCode = rs.GetString(0);

                }

                rs.Close();
                con.Close();
                return externalCode;

            }
            catch (Exception e)
            {
                // App.VSDLog.Info(e.StackTrace);
                return null;
            }
        }


        string IDBDataLoad.GetDefectNameArByID(string code)
        {
            try
            {
                SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
                string sqlQuery;


                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCeCommand command;

                sqlQuery = "SELECT Defect_Name_A FROM VSD_Defect WHERE (Defect_Code = @code)";
                command = new SqlCeCommand(sqlQuery, con);
                command.Parameters.Add("@code", SqlDbType.NChar).Value = code;

                SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);

                int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;
                string externalCode = "";

                if (rowCount > 0)
                {
                    rs.ReadFirst();
                    externalCode = rs.GetString(0);

                }

                rs.Close();
                con.Close();
                return externalCode;

            }
            catch (Exception e)
            {
                // App.VSDLog.Info(e.StackTrace);
                return null;
            }
        }

        string IDBDataLoad.GetCountryTicketCode(string country)
        {
            string sqlQuery = "";
            try
            {
                SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();



                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCeCommand command;

                sqlQuery = "SELECT Ticket_Code FROM VSD_Country WHERE (Country_Name = @country)";
                command = new SqlCeCommand(sqlQuery, con);
                command.Parameters.Add("@country", SqlDbType.NChar).Value = country;

                SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);

                int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;
                string ticketCode = "";

                if (rowCount > 0)
                {
                    rs.ReadFirst();
                    ticketCode = rs.GetString(0);

                }

                rs.Close();
                con.Close();
                return ticketCode;

            }
            catch (Exception e)
            {
                App.VSDLog.Info("Error in DBDataLodadManager.GetCountryTicketCode()... query : " + sqlQuery + " --Trace---\n" + e.StackTrace);
                return null;
            }
        }
        string IDBDataLoad.GetDeviceCode()
        {
            SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
            string sqlQuery;

            if (con.State == ConnectionState.Closed) { con.Open(); }
            SqlCeCommand command;

            try
            {
                sqlQuery = "SELECT Property_Type_Code from VSD_PROPERTY_TYPE WHERE PROPERTY_TYPE_NAME = 'DEVICECODE'";
                command = new SqlCeCommand(sqlQuery, con);
                SqlCeDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    return reader.GetString(0);
                }
            }
            catch (Exception ex)
            {
                //App.VSDLog.Info(ex.StackTrace);
                return null;
            }

            con.Close();
            return null;
        }

        string[] IDBDataLoad.GetImpoundingDays(string defectCode, string vehCategory)
        {

            string partner_fine_id = "1";
            string partner_fine_Name = "RTA";
            int vehCat = this.GetVehicleCategoryID(vehCategory);

            SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
            string sqlQuery;
            SqlCeCommand command;

            /*
            sqlQuery = "SELECT VCDSF.Veh_Cat_Def_Sev_Fine_ID, VCDSF.Veh_Cat_Defect_ID, VCDSF.Partner_Fine_ID, VCDSF.Fine_Amount, VCDSF.BlackPoints, VCDSF.Required_Grounding, " +
                         " VCDSF.Grounded_Days, VCDSF.Is_Disabled" +
                          " FROM VSD_Defect AS DEFECT INNER JOIN" +
                         " VSD_Veh_Cat_Defect AS VCDS ON DEFECT.Defect_ID = VCDS.Defect_ID INNER JOIN" +
                        " VSD_Veh_Cat_Def_Sev_Fine AS VCDSF ON VCDS.Veh_Cat_Defect_ID = VCDSF.Veh_Cat_Defect_ID" +
                        " WHERE        (DEFECT.Defect_ID = '" + @defectCode + "') AND (VCDS.Vehicle_category_ID = '" + @vehCat + "') AND (VCDSF.Partner_Fine_ID = '" + @partner_fine_id + "')";

           
                */
            sqlQuery = "   SELECT VCDSF.Veh_Cat_Def_Sev_Fine_ID, VCDSF.Veh_Cat_Defect_ID, VCDSF.Partner_Fine_ID, VCDSF.Fine_Amount, VCDSF.BlackPoints, VCDSF.Required_Grounding," +
                            " VCDSF.Grounded_Days, VCDSF.Is_Disabled, VP.Partner_Name" +
                            " FROM  VSD_Partner AS VP INNER JOIN" +
                            " VSD_Partner_Fine AS VPF ON VP.Partner_ID = VPF.Partner_Fine_ID CROSS JOIN" +
                            " VSD_Defect AS DEFECT INNER JOIN" +
                            " VSD_Veh_Cat_Defect AS VCDS ON DEFECT.Defect_ID = VCDS.Defect_ID INNER JOIN" +
                            " VSD_Veh_Cat_Def_Sev_Fine AS VCDSF ON VCDS.Veh_Cat_Defect_ID = VCDSF.Veh_Cat_Defect_ID" +
                           " WHERE  (DEFECT.Defect_ID = '" + @defectCode + "') AND (VCDS.Vehicle_category_ID = '" + @vehCat + "')  AND (VCDS.IsDisabled = 'F') AND (VCDSF.Is_Disabled = 'F') AND (UPPER(VP.Partner_Name) = '" + partner_fine_Name + "')";
            command = new SqlCeCommand(sqlQuery, con);
            /*
            command.Parameters.Add("@defectCode", SqlDbType.Int).Value = Int32.Parse( defectCode);
            command.Parameters.Add("@vehCat", SqlDbType.Int).Value = vehCat;
            command.Parameters.Add("@partner_fine_id", SqlDbType.Int).Value = Int32.Parse(partner_fine_id); 
            */
            //command.Parameters.Add("@defectCode", SqlDbType.NChar).Value = defectCode;
            //command.Parameters.Add("@vehCat", SqlDbType.NChar).Value = vehCat.ToString();
            //command.Parameters.Add("@partner_fine_id", SqlDbType.NChar).Value = partner_fine_id; 
            if (con.State == ConnectionState.Closed) { con.Open(); }
            command = new SqlCeCommand(sqlQuery, con);




            SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);

            int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;

            string[] row = new string[rs.ResultSetView.Columns.Length];

            if (rowCount > 0)
            {
                rs.ReadFirst();
                row[0] = rs["Veh_Cat_Def_Sev_Fine_ID"].ToString();
                row[1] = rs["Veh_Cat_Defect_ID"].ToString();
                row[2] = rs["Partner_Fine_ID"].ToString();
                row[3] = rs["Fine_Amount"].ToString();
                row[4] = rs["BlackPoints"].ToString();
                row[5] = rs["Required_Grounding"].ToString();
                row[6] = rs["Grounded_Days"].ToString();
                row[7] = rs["Is_Disabled"].ToString();

            }
            rs.Close();
            con.Close();

            return row;

        }
        string[] IDBDataLoad.GetDefectsFineInfo(string defectCode, string vehCategory)
        {


            int vehCat = this.GetVehicleCategoryID(vehCategory);

            SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
            string sqlQuery;
            SqlCeCommand command;

            sqlQuery = "SELECT  DISTINCT      DEFECT.Defect_ID, F.Fine_ID, F.Fine_Name, F.Fine_Name_a, VCDSF.Fine_Amount, VCDSF.Is_Disabled" +
                        " FROM  VSD_Defect AS DEFECT INNER JOIN " +
                         " VSD_Veh_Cat_Defect AS VCDS ON DEFECT.Defect_ID = VCDS.Defect_ID INNER JOIN " +
                         " VSD_Veh_Cat_Def_Sev_Fine AS VCDSF ON VCDS.Veh_Cat_Defect_ID = VCDSF.Veh_Cat_Defect_ID INNER JOIN " +
                         " VSD_Partner_Fine AS PF ON PF.Partner_Fine_ID = VCDSF.Partner_Fine_ID INNER JOIN " +
                         " VSD_Fine AS F ON F.Fine_ID = PF.Fine_ID " +
                         " WHERE        (DEFECT.Defect_ID = '" + @defectCode + "') AND (VCDS.IsDisabled = 'F') AND (VCDSF.Is_Disabled = 'F') AND (VCDS.Vehicle_category_ID = '" + @vehCat + "')";


            command = new SqlCeCommand(sqlQuery, con);

            if (con.State == ConnectionState.Closed) { con.Open(); }
            command = new SqlCeCommand(sqlQuery, con);




            SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);

            int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;

            string[] row = new string[rs.ResultSetView.Columns.Length];

            if (rowCount > 0)
            {
                rs.ReadFirst();
                row[0] = rs["Defect_ID"].ToString();
                row[1] = rs["Fine_Name"].ToString();
                row[2] = rs["Fine_Name_a"].ToString();
                row[3] = rs["Fine_Amount"].ToString();
                row[4] = rs["Is_Disabled"].ToString();
                row[5] = rs["Fine_ID"].ToString();

            }
            rs.Close();
            con.Close();

            return row;

        }

        //--added by kashif abbasi 
        DataTable IDBDataLoad.GetOfflineViolationAllByTicketCode(string ticeketCode)
        {
            SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
            string sqlQuery;
            SqlCeCommand command;

            sqlQuery = @"SELECT  v.Violation_ID, v.Severity_Level_ID, v.Violation_Ticket_Code, v.Due_Date, v.Comments, v.Comments_A, v.Fine_Amount, v.Violation_Status, 
                        i.Inspection_ID, i.Location_ID, i.Plate_Condition, i.Is_Reg_Exp, i.Is_Plate_Confiscated, i.Plate_Confiscation_Reason, 
                        i.Plate_Confiscation_reason_A, i.Reported_Inspector_Name, i.Reported_Inspector_Name_A, i.Inspection_Timestamp,
                        vi.Vehicle_Info_ID, vi.Vehicle_Plate_Category, vi.Vehicle_Plate_Code, vi.Vehicle_Plate_Source, vi.Vehicle_Plate_Number, vi.Vehicle_Country, vi.Vehicle_Model, 
                        vi.Vehicle_Model_A, vi.Make_Year, vi.Vehicle_Chassis_Number, vi.Vehicle_Reg_Expiry, vi.IsHazard,vi.Total_Impounding_Days,vi.Is_Grace_Period,
                        vc.Vehicle_Category_ID, vc.Parent_Vehicle_Category_ID, vc.Record_Type, vc.Category_Code, vc.Category_Name, vc.Category_Name_A, vc.Category_Desc, vc.Category_Desc_A, 
                        cd.Channel_Defect_ID, cd.Defect_Value,
                        d.Defect_ID, d.Defect_Category_ID, d.Defect_Code_Prefix, d.Defect_Code, d.Defect_Type, d.defect_name, d.defect_name_a, d.defect_desc, d.defect_desc_a, d.Defect_Value_Size, 
                        d.Defect_Type_A,
		                l.Location_Area_Name, l.Location_Area_Name_A, sl.Severity_Level, sl.Severity_Level_Name, sl.Severity_Level_Name_A ,  
                        oi.Owner_Info_ID, oi.Owner_Name, oi.Owner_Name_A, oi.Trade_License_Number, oi.Traffic_File_Number,
                        di.Driver_Info_ID, di.Driver_Name, di.Driver_Name_A, di.Driver_License_Number, 
                        dc.Parent_Defect_Category_ID, dc.Category_Code dc_Category_Code, dc.Category_Name dc_Category_Name, dc.Category_Name_A dc_Category_Name_A, dc.Category_Desc dc_Category_Desc, 
                        dc.Category_Desc_A dc_Category_Desc_A 
                         FROM         
                        VSD_Inspection i
                        inner join VSD_Violation v on i.Violation_ID=v.Violation_ID
                        inner join VSD_Vehicle_Info vi on i.Vehicle_Info_ID=vi.Vehicle_Info_ID
                        inner join VSD_Vehicle_Category vc on vc.Vehicle_Category_ID=vi.Vehicle_Category_ID
                        inner join VSD_Channel_Defect cd on cd.Violation_ID=i.Violation_ID
                        inner join VSD_Defect d on cd.Defect_ID=d.Defect_ID
                        inner join VSD_LOCATION l on l.Location_ID=i.Location_ID
                        inner join VSD_Severity_Level sl on sl.Severity_Level_ID=v.Severity_Level_ID
                        inner join VSD_OWNER_INFO oi on oi.VEHICLE_INFO_ID=vi.Vehicle_Info_ID
                        inner join VSD_Driver_Info di on di.VEHICLE_INFO_ID=vi.VEHICLE_INFO_ID
                        inner join VSD_Defect_Category dc on dc.Defect_Category_ID=d. Defect_Category_ID 
                        ";

            if (!ticeketCode.Trim().Equals(""))
            {
                sqlQuery += "WHERE v.Violation_Ticket_Code='@id'";
                command = new SqlCeCommand(sqlQuery, con);
                command.Parameters.Add("@id", SqlDbType.NChar, 50).Value = ticeketCode;
            }
            else
            {
                command = new SqlCeCommand(sqlQuery, con);
            }

            DataTable dt = new DataTable();

            //command.ExecuteResultSet(ResultSetOptions.Scrollable);
            if (con.State == ConnectionState.Closed) { con.Open(); }
            dt.Load(command.ExecuteReader());

            //int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;
            //string[] row = new string[rs.ResultSetView.Columns.Length];

            //if (rowCount > 0)
            //{
            //    rs.ReadFirst();
            //    row[0] = rs["Violation_ID"].ToString();
            //    row[1] = rs["Due_Date"].ToString();
            //    row[2] = rs["Comments"].ToString();
            //    row[3] = rs["Comments_A"].ToString();
            //    row[4] = rs["Fine_Amount"].ToString();
            //    row[5] = rs["Timestamp"].ToString();
            //    row[6] = rs["Violation_Status"].ToString();
            //    row[7] = rs["Created_By"].ToString();
            //    row[8] = rs["Severity_Level_ID"].ToString();

            //}

            //rs.Close();

            con.Close();
            return dt;
        }
        //--added by kashif abbasi on dated 08-dec-2015----
        DataTable IDBDataLoad.GetCommentsByDefectID(string defect, string vehCat)
        {
            DataTable dt = null;
            try
            {
                int vehCatID = GetVehicleCategoryID(vehCat);

                string sqlQuery;
                SqlCeCommand command;
                if (AppProperties.Selected_Resource == "English")
                    sqlQuery = @"SELECT dc.DEFECT_COMMENT, dc.DEFECT_COMMENT_A, dc.DEFECT_COMMENT_ID
                         FROM  VSD_Defect_Comment AS dc INNER JOIN
                         VSD_Defect AS d ON dc.DEFECT_ID = d.Defect_ID INNER JOIN
                         VSD_Vehicle_Category AS vc ON dc.VEHICLE_CATEGORY_ID = vc.Vehicle_Category_ID
                         WHERE d.DEFECT_Name='" + defect + "' AND dc.VEHICLE_CATEGORY_ID=" + vehCatID;
                else
                    sqlQuery = @"SELECT dc.DEFECT_COMMENT, dc.DEFECT_COMMENT_A, dc.DEFECT_COMMENT_ID
                         FROM  VSD_Defect_Comment AS dc INNER JOIN
                         VSD_Defect AS d ON dc.DEFECT_ID = d.Defect_ID INNER JOIN
                         VSD_Vehicle_Category AS vc ON dc.VEHICLE_CATEGORY_ID = vc.Vehicle_Category_ID
                         WHERE d.DEFECT_Name_A='" + defect + "' AND dc.VEHICLE_CATEGORY_ID=" + vehCatID;

                SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
                command = new SqlCeCommand(sqlQuery, con);
                dt = new DataTable();
                if (con.State == ConnectionState.Closed) { con.Open(); }
                dt.Load(command.ExecuteReader());

                con.Close();
                return dt;
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
                return dt;
            }
        }

        DataTable IDBDataLoad.GetDefectPropertyByID(string defectID)
        {
            DataTable dt = null;
            string sqlQuery = string.Empty;
            try
            {


                SqlCeCommand command;

                sqlQuery = @"SELECT * from VSD_DEFECT_PROPERTY where Defect_ID='" + defectID + "' AND IS_DISABLED = 'F'";



                SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
                command = new SqlCeCommand(sqlQuery, con);
                dt = new DataTable();
                if (con.State == ConnectionState.Closed) { con.Open(); }
                dt.Load(command.ExecuteReader());

                con.Close();
                return dt;
            }
            catch (Exception e)
            {
                App.VSDLog.Info("Error in DBDataLodadManager.GetDefectPropertyByID()... query : " + sqlQuery + " --Trace---\n" + e.StackTrace);
                return null;
            }
        }


        DataTable IDBDataLoad.GetDefectCategoryFromDefectCode(string defectCode)
        {

            // Schweers.Localization.ArabicConverter arab = new Schweers.Localization.ArabicConverter();
            SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
            string sqlQuery;
            SqlCeCommand command;
            DataTable dtTelematic = new DataTable();
            try
            {
                App.VSDLog.Info("GetDefectCategoryFromDefectCode:" + defectCode);
                sqlQuery = "SELECT Category_Name,Category_Name_A,Category_Code FROM VSD_Defect_Category" +
                " WHERE Defect_Category_ID IN ( SELECT     Parent_Defect_Category_ID FROM VSD_Defect_Category " +
                 "  WHERE Defect_Category_ID IN ((SELECT Defect_Category_ID  FROM VSD_DEFECT WHERE (Defect_Code ='" + defectCode + "')" +
                 " AND (IsDisabled = 'F')) )       AND (IsDisabled = 'F')    ) AND (IsDisabled = 'F')";
                App.VSDLog.Info("Query:" + sqlQuery);
                // SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
                command = new SqlCeCommand(sqlQuery, con);
                dtTelematic = new DataTable();
                if (con.State == ConnectionState.Closed) { con.Open(); }
                dtTelematic.Load(command.ExecuteReader());

                con.Close();
                return dtTelematic;

            }
            catch (Exception ex)
            {
                App.VSDLog.Info("Query:" + ex.Message);
                return dtTelematic;
            }




        }
        DataTable IDBDataLoad.GetDefectSubAndNameFDefectCode(string defectCode)
        {

            // Schweers.Localization.ArabicConverter arab = new Schweers.Localization.ArabicConverter();
            SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
            string sqlQuery;
            SqlCeCommand command;
            DataTable dtTelematic = new DataTable();
            try
            {
                App.VSDLog.Info("GetDefectCategoryFromDefectCode:" + defectCode);

                sqlQuery = "select D.Defect_ID, D.Defect_Category_ID,D.Defect_Code,D.Defect_Type,D.Defect_Type_A,D.Defect_Name,D.Defect_Name_A,DC.Category_Code,DC.Category_Name,DC.Category_Name_A " +
                            " from VSD_Defect D , VSD_Defect_Category DC " +
                            " where D.Defect_Category_ID = DC.Defect_Category_ID " +
                            " AND D.Defect_Code = '" + defectCode + "'" +
                            " AND D.IsDisabled = 'F' " +
                            " AND DC.IsDisabled='F'";

                App.VSDLog.Info("Query:" + sqlQuery);
                // SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
                command = new SqlCeCommand(sqlQuery, con);
                dtTelematic = new DataTable();
                if (con.State == ConnectionState.Closed) { con.Open(); }
                dtTelematic.Load(command.ExecuteReader());

                con.Close();
                return dtTelematic;

            }
            catch (Exception ex)
            {
                App.VSDLog.Info("Query:" + ex.Message);
                return dtTelematic;
            }




        }


        public DataTable LoadFuelEmissionType()
        {

            DataTable dt = null;
            try
            {
                string sqlQuery;
                SqlCeCommand command;
                sqlQuery = @"SELECT * FROM   VSD_FUEL_EMISSION WHERE (IS_DISABLED = 'F')";

                SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
                command = new SqlCeCommand(sqlQuery, con);
                dt = new DataTable();
                if (con.State == ConnectionState.Closed) { con.Open(); }
                dt.Load(command.ExecuteReader());

                con.Close();
                return dt;
            }

            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
                return dt;
            }
        }
        public DataTable LoadDeviceInspSubVat()
        {

            DataTable dt = null;
            try
            {
                string sqlQuery;
                SqlCeCommand command;
                sqlQuery = @"SELECT VEHICLE_CATEGORY_Name,VEHICLE_CATEGORY_Name_Ar FROM   VSD_Vehicle_DEV_INSPECT_PARMS WHERE (IS_DISABLED = 'F')";

                SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
                command = new SqlCeCommand(sqlQuery, con);
                dt = new DataTable();
                if (con.State == ConnectionState.Closed) { con.Open(); }
                dt.Load(command.ExecuteReader());

                con.Close();
                return dt;
            }

            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
                return dt;
            }
        }
        public DataTable GetDeviceInspParemtByVehCat(string vehicleSubCat)
        {

            DataTable dt = null;
            try
            {
                string sqlQuery;
                SqlCeCommand command;
                sqlQuery = @"SELECT * FROM    VSD_Vehicle_DEV_INSPECT_PARMS WHERE (VEHICLE_CATEGORY_Name = '" + vehicleSubCat + "' OR VEHICLE_CATEGORY_Name_Ar = '" + vehicleSubCat + "' )  AND (IS_DISABLED = 'F')";

                SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
                command = new SqlCeCommand(sqlQuery, con);
                dt = new DataTable();
                if (con.State == ConnectionState.Closed) { con.Open(); }
                dt.Load(command.ExecuteReader());

                con.Close();
                return dt;
            }

            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
                return dt;
            }
        }
        public string GetVehCatIDFromVehCatName(string categoryName)
        {
            string Catergory_ID = string.Empty;
            string parentCategory = "Heavy Vehicle";

            try
            {
                SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
                string sqlQuery;
                SqlCeCommand command;
                sqlQuery = "SELECT a.Vehicle_Category_ID FROM VSD_Vehicle_Category a WHERE (Category_Name = '" + @categoryName + "') AND " +
                    "(Parent_Vehicle_Category_ID IN (SELECT        Category_Code  FROM            VSD_Vehicle_Category " +
                     "WHERE        (Category_Name = '" + parentCategory + "')))";


                // SELECT   Vehicle_Category_ID FROM SD_Vehicle_Category WHERE   (Category_Name = 'Van')
                command = new SqlCeCommand(sqlQuery, con);

                if (con.State == ConnectionState.Closed) { con.Open(); }
                command = new SqlCeCommand(sqlQuery, con);




                SqlCeResultSet rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);

                int rowCount = ((System.Collections.ICollection)rs.ResultSetView).Count;

                // string[] row = new string[rs.ResultSetView.Columns.Length];

                if (rowCount > 0)
                {
                    rs.ReadFirst();
                    Catergory_ID = rs["Vehicle_Category_ID"].ToString();

                }
                rs.Close();
                con.Close();


                return Catergory_ID;
            }

            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                //  WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
                return null;
            }
        }

        #endregion

        public void SaveDefectAttachmendInDB(string path, string violationID)
        {
            string strPath = Properties.Settings.Default.violationImagesPath;
            try
            {
                strPath += @"\" + DateTime.Now.Date.ToString("MMM") + DateTime.Now.Year;
                strPath += @"\" + DateTime.Now.Date.ToString("ddMMyyyy");
                strPath += @"\" + AppProperties.vehicle.Country.Replace(" ", "") + "_" + AppProperties.vehicle.PlateNumber.Trim() + "_" +
                           AppProperties.vehicle.PlateCategory.Replace(" ", "") + "_" + AppProperties.vehicle.PlateCode.Replace(" ", "");



                strPath = strPath.Substring(0, strPath.LastIndexOf("\\") + 1) + violationID.Replace(".", "_");
                if (!Directory.Exists(strPath))
                    return;
                string[] violationDefects = Directory.GetFiles(@strPath);

                // DB Insert
                ///C:\RTA_VSD_IMAGES\Aug2016\24082016\P02_01_04_02_130_625240325

                SqlCeConnection con = ((VSDApp.com.rta.vsd.hh.db.IDBManager)VSDApp.com.rta.vsd.hh.db.DBConnectionManager.GetInstance()).GetConnection();

                SqlCeCommand command;
                SqlCeResultSet rs;

                foreach (string str in violationDefects)
                {
                    string sqlQuery;
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    string[] temp = str.Substring(str.LastIndexOf("\\") + 1).Split('_');

                    string defect_ID = temp[0];
                    sqlQuery = " INSERT INTO VSD_Defect_Attachment ( Violation_ID,Defect_ID,Image_Path,TimeStamp,IS_DELETED) VALUES ( " + (violationID != null ? "'" + violationID + "'" : "NULL") + "," + (defect_ID != null ? "'" + defect_ID + "'" : "NULL") + "," + (str != null ? "'" + str + "'" : "NULL") + "," + ("'" + DateTime.Now.ToString() + "'") + ", 'F' )";
                    command = new SqlCeCommand(sqlQuery, con);
                    rs = command.ExecuteResultSet(ResultSetOptions.Scrollable);
                    rs.Close();
                    con.Close();
                }

            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                //  WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);

            }
        }
       
        /*
        public void SynDefectAttachmendInDB()
        {

            DataTable dt = null;
            try
            {


                string sqlQuery;
                SqlCeCommand command;
                sqlQuery = @"SELECT DISTINCT Violation_ID, Defect_ID FROM VSD_Defect_Attachment";
                SqlCeConnection con = ((IDBManager)DBConnectionManager.GetInstance()).GetConnection();
                command = new SqlCeCommand(sqlQuery, con);
                dt = new DataTable();
                if (con.State == ConnectionState.Closed) { con.Open(); }
                dt.Load(command.ExecuteReader());

                con.Close();
                // return dt;
                foreach (DataRow selectedRow in dt.Rows)
                {
                    string violation_ID = Convert.ToString(selectedRow["Violation_ID"]);
                    string Defect_ID = Convert.ToString(selectedRow["Defect_ID"]);

                    string Image_Path = string.Empty;
                  
                    DataTable dtIndividualDefectImage = new DataTable();
                    sqlQuery = @"SELECT * from VSD_Defect_Attachment where Violation_ID = '" + violation_ID + "' AND Defect_ID = '" + Defect_ID + "'";
                    command = new SqlCeCommand(sqlQuery, con);
                    dtIndividualDefectImage = new DataTable();
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    dtIndividualDefectImage.Load(command.ExecuteReader());
                    con.Close();
                    foreach (DataRow row in dtIndividualDefectImage.Rows)
                    {
                        Defect_ID = Convert.ToString(row["Defect_ID"]);
                        Image_Path = Convert.ToString(row["Image_Path"]);
                        ///
                        BitmapImage image = new BitmapImage(new Uri(Image_Path));



                       
                        FileStream fileStream = new FileStream(Image_Path, FileMode.Open, FileAccess.Read);
                        byte[] buffer = new byte[fileStream.Length];
                        fileStream.Read(buffer, 0, (int)fileStream.Length);
                        fileStream.Close();
                        string Chosen_Pic_String = Convert.ToBase64String(buffer);
                        SynchdefectWithHHService("02.01.04.02.100000234", "110", Chosen_Pic_String);

                    }


                }
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);

            }
        }

        
        public void SynchdefectWithHHService(string vio_id, string defect_ID, string image_binary)
        {
            try
            {
                handHeldService.SynchronizeDefectAttachmentsResponseItem respItem = new VSDApp.handHeldService.SynchronizeDefectAttachmentsResponseItem();

                //  handHeldService. viol = new VSDApp.handHeldService.Violation();
                //  incp.violation = viol;
                //viol.
                handHeldService.Violation[] violation_Array = new handHeldService.Violation[1];
                VSDApp.handHeldService.Defect[] defects = new VSDApp.handHeldService.Defect[1];
                defects[0] = new handHeldService.Defect();
                defects[0].defectId = defect_ID;
                VSDApp.handHeldService.FileAttachment[] file = new VSDApp.handHeldService.FileAttachment[1];
                file[0] = new handHeldService.FileAttachment();
                //  file[0].fileContent = image_binary;
                file[0].fileContent = "AAA";
                file[0].fileName = defect_ID;
                file[0].fileType = "jpg";
                defects[0].attachements = file;
                violation_Array[0] = new handHeldService.Violation();
                violation_Array[0].defects = defects;

                violation_Array[0].ticketNumber = vio_id;

                handHeldService.ViolationId violation_id = new handHeldService.ViolationId();
                //  violation_id.violationId = vio_id;

                handHeldService.HandHeldService hh = new VSDApp.handHeldService.HandHeldService();
                handHeldService.AuthHeader header = new VSDApp.handHeldService.AuthHeader();
                header.userName = "vsd";
                header.password = "vsd";
                hh.authHeader = header;
                hh.Timeout = 180000;

                respItem = hh.synchronizeDefectAttachments("RC123", violation_Array);
                if (respItem.reponse.code != null)
                {

                }



            }
            catch (Exception)
            {

                throw;
            }
        }*/


    }
}
