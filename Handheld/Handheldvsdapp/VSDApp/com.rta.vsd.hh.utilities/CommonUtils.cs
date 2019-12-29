using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using VSDApp.WPFMessageBoxControl;

namespace VSDApp.com.rta.vsd.hh.utilities
{
    class CommonUtils
    {
        public static Hashtable Splitter(string[] list)
        {

            Hashtable hashMap = new Hashtable();
            if (null == list)
            {
                return hashMap;
            }
            else if (list.Length == 0)
            {
                return hashMap;
            }
            else if (list[0].Equals(""))
            {
                return hashMap;
            }
            else
            {

                foreach (string str in list)
                {
                    try
                    {
                        string[] temp = str.Split(new char[] { '^' });
                        //System.Windows.Forms.MessageBox.Show(temp[0] + " " + temp[1]);
                        if (!hashMap.Contains(temp[0].Trim()))
                            hashMap.Add(temp[0].Trim(), temp[1].Trim());
                    }
                    catch (Exception ex)
                    {
                        //App.VSDLog.Info(e.StackTrace);
                        // System.Windows.Forms.MessageBox.Show(e.Message);
                        WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
                    }
                }
                /*
                for (int i = 0; i < list.Length; i++)
                {
                    try
                    {
                        string[] temp = list[i].Split(new char[] { '^' });
                        //System.Windows.Forms.MessageBox.Show(temp[0] + " " + temp[1]);
                        hashMap.Add(temp[0].Trim(), temp[1].Trim());
                    }
                    catch (Exception e)
                    {
                        //App.VSDLog.Info(e.StackTrace);
                        System.Windows.Forms.MessageBox.Show(e.Message);
                    }
                }*/
                return hashMap;
            }
        }

        public static Hashtable SplitterWithOrder(string[] list, out List<string> lstLocation)
        {
            lstLocation = new List<string>();
            Hashtable hashMap = new Hashtable();
            if (null == list)
            {
                return hashMap;
            }
            else if (list.Length == 0)
            {
                return hashMap;
            }
            else if (list[0].Equals(""))
            {
                return hashMap;
            }
            else
            {

                foreach (string str in list)
                {
                    try
                    {
                        string[] temp = str.Split(new char[] { '^' });
                        //System.Windows.Forms.MessageBox.Show(temp[0] + " " + temp[1]);
                        if (!hashMap.Contains(temp[0].Trim()))
                        {
                            hashMap.Add(temp[0].Trim(), temp[1].Trim());
                            lstLocation.Add(temp[0].Trim());
                        }
                    }
                    catch (Exception ex)
                    {
                        //App.VSDLog.Info(e.StackTrace);
                        // System.Windows.Forms.MessageBox.Show(e.Message);
                        WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
                    }
                }
                /*
                for (int i = 0; i < list.Length; i++)
                {
                    try
                    {
                        string[] temp = list[i].Split(new char[] { '^' });
                        //System.Windows.Forms.MessageBox.Show(temp[0] + " " + temp[1]);
                        hashMap.Add(temp[0].Trim(), temp[1].Trim());
                    }
                    catch (Exception e)
                    {
                        //App.VSDLog.Info(e.StackTrace);
                        System.Windows.Forms.MessageBox.Show(e.Message);
                    }
                }*/
                return hashMap;
            }
        }

        public static Hashtable Splitter2(string[] list)
        {

            Hashtable hashMap = new Hashtable();
            if (null == list)
            {
                return hashMap;
            }
            else if (list.Length == 0)
            {
                return hashMap;
            }
            else if (list[0].Equals(""))
            {
                return hashMap;
            }
            else
            {
                for (int i = 0; i < list.Length; i++)
                {
                    try
                    {
                        string[] temp = list[i].Split(new char[] { '^' });
                        //System.Windows.Forms.MessageBox.Show(temp[0] + " " + temp[1]);
                        hashMap.Add(temp[1].Trim(), temp[0].Trim());
                    }
                    catch (Exception e)
                    {
                        //App.VSDLog.Info(e.StackTrace);
                        System.Windows.Forms.MessageBox.Show(e.Message);
                    }
                }
                return hashMap;
            }
        }

        public static List<String> Return_Sorted_List_Hashtable(string[] list)
        {
            List<string> sorted_lost = new List<string>(list.Length);
            for (int i = 0; i < list.Length; i++)
            {
                string[] temp = list[i].Split(new char[] { '^' });
                sorted_lost.Add(temp[0].Trim());
            }
            return sorted_lost;
        }

        public static Hashtable LoadInspectorsUserNameTable()
        {
            Hashtable hashMap = new Hashtable();
            try
            {
                hashMap.Add("nanoori", "Nasser Noori");
                hashMap.Add("jlrosales", "Jovener Rosales");
                hashMap.Add("mhbebars", "Mohamed Bebars");
                hashMap.Add("ahgarcia", "Alexander Garacia");
                hashMap.Add("wclopez", "Warren Lopez");
                hashMap.Add("njlasap", "Nino Lasap");
                hashMap.Add("yeahmed", "Yaser Elayek");
                hashMap.Add("ssanbbar", "Sultan Anbar");
                hashMap.Add("kaalbanna", "Khaled Albanna");
                hashMap.Add("skalbulooshi", "Seif Khalifa");
                hashMap.Add("tahableh", "Thaer Adnan");
                hashMap.Add("emahmed", "Elsayed Metwally");
                hashMap.Add("ambadran", "Ahmed Badran");
                hashMap.Add("hialbedwawi", "Hamad Albedwawi");
                hashMap.Add("vdcs_stgadmin01", "Hamad Albedwawi");
                hashMap.Add("falhammadi", "Faisal Alhammadi");
                hashMap.Add("vsd", "vsd");

            }
            catch (Exception)
            {

                throw;
            }

            return hashMap;
        }
        public static Hashtable LoadInspectorsUserNameArTable()
        {
            Hashtable hashMap = new Hashtable();
            try
            {
                hashMap.Add("nanoori", "ناصر نوري");
                hashMap.Add("jlrosales", "جوفينير روساليس");
                hashMap.Add("mhbebars", "محمد بيبرس");
                hashMap.Add("ahgarcia", "الكسندر جاروسيا");
                hashMap.Add("wclopez", "وارين لوبيز");
                hashMap.Add("njlasap", "نينو لاساب");
                hashMap.Add("yeahmed", "ياسر العايق");
                hashMap.Add("ssanbbar", "سلطان عنبر");
                hashMap.Add("kaalbanna", "خالد البنا");
                hashMap.Add("skalbulooshi", "سيف خليفة");
                hashMap.Add("tahableh", "ثائر عدنان");
                hashMap.Add("emahmed", "السيد متولي");
                hashMap.Add("ambadran", "أحمد بدران");
                hashMap.Add("hialbedwawi", "حمد البدواوي");
                hashMap.Add("vdcs_stgadmin01", "حمد البدواوي");
                hashMap.Add("falhammadi", "فيصل الحمادي");
                hashMap.Add("vsd", "حمد البدواوي");

            }
            catch (Exception)
            {

                throw;
            }

            return hashMap;
        }

        public string GetStringValue(string strName)
        {
            try
            {
                Random rnd = new Random();
                //int cultureNdx = rnd.Next(4, cultures.Length);
                CultureInfo originalCulture = Thread.CurrentThread.CurrentCulture;


                CultureInfo newCulture = new CultureInfo(Thread.CurrentThread.CurrentCulture.ToString());
                Thread.CurrentThread.CurrentCulture = newCulture;
                Thread.CurrentThread.CurrentUICulture = newCulture;
                ResourceManager rm = new ResourceManager("VSDApp.Properties.Resources", this.GetType().Assembly);
                string greeting = String.Format(rm.GetString(strName));


                return greeting;



                /*
                if (AppProperties.Selected_Resource == "Arabic")
                {
                    Random rnd = new Random();
                    //int cultureNdx = rnd.Next(4, cultures.Length);
                    CultureInfo originalCulture = Thread.CurrentThread.CurrentCulture;


                    CultureInfo newCulture = new CultureInfo("ar-AE");
                    Thread.CurrentThread.CurrentCulture = newCulture;
                    Thread.CurrentThread.CurrentUICulture = newCulture;
                    ResourceManager rm = new ResourceManager("VSDApp.Properties.Resources", this.GetType().Assembly);
                    string greeting = String.Format("The current culture is {0}.\n{1}",
                                                    Thread.CurrentThread.CurrentUICulture.Name,
                                                    rm.GetString(strName));


                    return greeting;
                }
                else
                {
                    Random rnd = new Random();
                    //int cultureNdx = rnd.Next(4, cultures.Length);
                    CultureInfo originalCulture = Thread.CurrentThread.CurrentCulture;


                    CultureInfo newCulture = new CultureInfo("en-US");
                    Thread.CurrentThread.CurrentCulture = newCulture;
                    Thread.CurrentThread.CurrentUICulture = newCulture;
                    ResourceManager rm = new ResourceManager("VSDApp.Properties.Resources", this.GetType().Assembly);
                    string greeting = String.Format("The current culture is {0}.\n{1}",
                                                    Thread.CurrentThread.CurrentUICulture.Name,
                                                    rm.GetString(strName));


                    return greeting;
                }*/


            }
            catch (Exception ex)
            {

                return null;

            }

        }
        public string ConvertToEasternArabicNumerals(string input)
        {
            try
            {
                if (input == null)
                    return "";
                System.Text.UTF8Encoding utf8Encoder = new UTF8Encoding();
                System.Text.Decoder utf8Decoder = utf8Encoder.GetDecoder();
                System.Text.StringBuilder convertedChars = new System.Text.StringBuilder();
                char[] convertedChar = new char[1];
                byte[] bytes = new byte[] { 217, 160 };
                char[] inputCharArray = input.ToCharArray();
                foreach (char c in inputCharArray)
                {
                    if (char.IsDigit(c))
                    {
                        bytes[1] = Convert.ToByte(160 + char.GetNumericValue(c));
                        utf8Decoder.GetChars(bytes, 0, 2, convertedChar, 0);
                        convertedChars.Append(convertedChar[0]);
                    }
                    else
                    {
                        convertedChars.Append(c);
                    }
                }
                return convertedChars.ToString();
            }

            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
                return "";
            }
        }

        public void validateTextInteger(object sender, EventArgs e)
        {
            Exception X = new Exception();

            TextBox T = (TextBox)sender;

            try
            {
                if (T.Text != "-")
                {
                    int x = int.Parse(T.Text);
                }
            }
            catch (Exception)
            {
                try
                {
                    int CursorIndex = T.SelectionStart - 1;
                    T.Text = T.Text.Remove(CursorIndex, 1);

                    //Align Cursor to same index
                    T.SelectionStart = CursorIndex;
                    T.SelectionLength = 0;
                }
                catch (Exception) { }
            }
        }

        public void validateTextCharacter(object sender, EventArgs e)
        {
            TextBox T = (TextBox)sender;
            try
            {
                //Not Allowing Numbers
                char[] UnallowedCharacters = { '0', '1',
                                           '2', '3', 
                                           '4', '5',
                                           '6', '7',
                                           '8', '9'};

                if (textContainsUnallowedCharacter(T.Text, UnallowedCharacters))
                {
                    int CursorIndex = T.SelectionStart - 1;
                    T.Text = T.Text.Remove(CursorIndex, 1);

                    //Align Cursor to same index
                    T.SelectionStart = CursorIndex;
                    T.SelectionLength = 0;
                }
            }
            catch (Exception) { }
        }

        public string GetScreenOrientation()
        {
            int theScreenRectHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
            int theScreenRectWidth = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
            //Compare height and width of screen and act accordingly.
            if (theScreenRectHeight > theScreenRectWidth)
            {
                // Run the application in portrait, as in:
                //  MessageBox.Show("Run in portrait.");
                return "Portrait";
            }
            else
            {
                return "LandScape";
                // Run the application in landscape, as in:
                //  MessageBox.Show("Run in landscape.");
            }
            // return "";
        }

        private bool textContainsUnallowedCharacter(string T, char[] UnallowedCharacters)
        {
            for (int i = 0; i < UnallowedCharacters.Length; i++)
                if (T.Contains(UnallowedCharacters[i]))
                    return true;

            return false;
        }

        public static void WriteLog(string logMessage)
        {
            
                  string _fileName = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase) + "\\VSDLog.txt";
         string logFilePath = _fileName.Substring(6);


            try
            {
                if (!File.Exists(logFilePath))
                {
                    File.Create(logFilePath);
                }

                float N = 1024; //Log File size 
                FileInfo logFile = new FileInfo(logFilePath);
                if (logFile.Length > 1024 * N)
                {
                    //logFile = null;
                    using (var stream = new FileStream(logFilePath, FileMode.Truncate))
                    {
                        using (StreamWriter writer = new StreamWriter(stream))
                        {

                            writer.Write("\r\nLog Entry : ");
                            writer.WriteLine("{0} {1}", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongDateString());
                            writer.WriteLine("  :");
                            writer.WriteLine("  :{0}", logMessage);
                            writer.WriteLine("-------------------------------");
                            writer.Close();
                            writer.Dispose();
                        }
                    }
                }
                else
                {
                    using (StreamWriter writer = new StreamWriter(logFilePath, true))
                    {

                        writer.Write("\r\nLog Entry : ");
                        writer.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
                        writer.WriteLine("  :");
                        writer.WriteLine("  :{0}", logMessage);
                        writer.WriteLine("-------------------------------");
                        writer.Close();
                        writer.Dispose();
                    }
                }


            }
            catch (Exception ex)
            {

                // WriteLog(ex.Message);
                //throw;
            }
        }

        /// <summary>
        /// Write Log to Latitude Longitude
        /// </summary>
        /// <param name="logMessage"></param>
        public static void WriteLocationLog(string logMessage)
        {

            string _fileName = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase) + "\\VSDLocationLog.txt";
            string logFilePath = _fileName.Substring(6);


            try
            {
                if (!File.Exists(logFilePath))
                {
                    File.Create(logFilePath);
                }

                float N = 1024; //Log File size 
                FileInfo logFile = new FileInfo(logFilePath);
                if (logFile.Length > 1024 * N)
                {
                    //logFile = null;
                    using (var stream = new FileStream(logFilePath, FileMode.Truncate))
                    {
                        using (StreamWriter writer = new StreamWriter(stream))
                        {

                            writer.Write("\r\nLog Entry : ");
                            writer.WriteLine("{0} {1}", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongDateString());
                            writer.WriteLine("  :");
                            writer.WriteLine("  :{0}", logMessage);
                            writer.WriteLine("-------------------------------");
                            writer.Close();
                            writer.Dispose();
                        }
                    }
                }
                else
                {
                    using (StreamWriter writer = new StreamWriter(logFilePath, true))
                    {

                        writer.Write("\r\nLog Entry : ");
                        writer.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
                        writer.WriteLine("  :");
                        writer.WriteLine("  :{0}", logMessage);
                        writer.WriteLine("-------------------------------");
                        writer.Close();
                        writer.Dispose();
                    }
                }


            }
            catch (Exception ex)
            {

                // WriteLog(ex.Message);
                //throw;
            }
        }

        public int GetPixelWidhthofString(string inputstring)
        {

            try
            {
                if (inputstring == "")
                    return 0;
                Bitmap objBmpImage = new Bitmap(1, 1);

                int intWidth = 0;
                int intHeight = 0;

                // Create the Font object for the image text drawing.
                Font objFont = new Font("Arial", 20, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);

                // Create a graphics object to measure the text's width and height.
                Graphics objGraphics = Graphics.FromImage(objBmpImage);

                // This is where the bitmap size is determined.
                intWidth = (int)objGraphics.MeasureString(inputstring, objFont).Width;
                intHeight = (int)objGraphics.MeasureString(inputstring, objFont).Height;

                // Create the bmpImage again with the correct size for the text and font.
                objBmpImage = new Bitmap(objBmpImage, new System.Drawing.Size(intWidth, intHeight));

                return objBmpImage.Width;
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
                return 0;
            }


        }

        public List<string> SplitStringIntoWords(String input, int length)
        {
            var splitedList = new List<string>();
            try
            {


                string block = "";
                var arabicBlock = "";

                foreach (char c in input)
                {

                    var b = (int)c;
                    // check here if charachter is arabic
                    // this is a sample, or you can use 'IsArabicChar'
                    //if (b > 6000)
                    if (IsArabicChar(c))
                    {
                        arabicBlock += c.ToString();
                    }
                    else
                    {
                       // block += arabicBlock + c;
                        arabicBlock +=c;
                        splitedList.Add(arabicBlock + " ");
                        arabicBlock = "";
                    }
                }
                splitedList.Add(arabicBlock);
                // splitedList.Add(block);
                return splitedList;
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
                return splitedList;
            }
        }
        public List<string> SplitStringIntoWords2(String input, int length)
        {
            var splitedList = new List<string>();
            try
            {


                string block = "";
                var arabicBlock = "";

                foreach (char c in input)
                {

                    var b = (int)c;
                    // check here if charachter is arabic
                    // this is a sample, or you can use 'IsArabicChar'
                    //if (b > 6000)
                    if (IsArabicChar(c))
                    {
                        arabicBlock += c.ToString();
                    }
                    else
                    {
                        // block += arabicBlock + c;
                        arabicBlock += c;
                        splitedList.Add(arabicBlock);
                        arabicBlock = "";
                    }
                }
                splitedList.Add(arabicBlock);
                // splitedList.Add(block);
                return splitedList;
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
                return splitedList;
            }
        }
        internal static bool IsArabicChar(Char character)
        {
            if (character >= 0x600 && character <= 0x6ff)
                return true;

            if (character >= 0x750 && character <= 0x77f)
                return true;

            if (character >= 0xfb50 && character <= 0xfc3f)
                return true;

            if (character >= 0xfe70 && character <= 0xfefc)
                return true;

            return false;
        }

        public bool IsEnglish(string inputstring)
        {
            try
            {
                Regex regex = new Regex(@"[A-Za-z0-9 .,-=+(){}\[\]\\]");
                MatchCollection matches = regex.Matches(inputstring);

                if (matches.Count.Equals(inputstring.Length))
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
                return false;
            }
        }

        public bool isWordFoundInList(List<string> mainList, String word)
        {
            try
            {
                if (mainList == null || word == null)
                    return false;
                List<string> created = new List<string>();
                foreach (string str in mainList)
                {
                    created.Add(str.Trim());
                }
                if (created.Contains(word))
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
                return false;
            }
        }

        public void ChangeControlFocous(KeyEventArgs e)
        {
            try
            {
                    // Creating a FocusNavigationDirection object and setting it to a
                    // local field that contains the direction selected.
                    FocusNavigationDirection focusDirection = FocusNavigationDirection.Next;

                    // MoveFocus takes a TraveralReqest as its argument.
                    TraversalRequest request = new TraversalRequest(focusDirection);

                    // Gets the element with keyboard focus.
                    UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;

                    // Change keyboard focus.
                    if (elementWithFocus != null)
                    {
                        if (elementWithFocus.MoveFocus(request)) e.Handled = true;
                    }
                
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        public static void ShowKeyBoard()
        {
            try
            {
                System.Diagnostics.Process.Start(@"C:\Program Files\Common Files\Microsoft Shared\ink\TabTip.exe");
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                // WPFMessageBoxResult _Result = WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
                MessageBox.Show(ex.Message.ToString());
            }
        }
        [DllImport("user32.dll")]
        public static extern int FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        public static extern int SendMessage(int hWnd, uint Msg, int wParam, int lParam);

        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_CLOSE = 0xF060;

        public static void CLoseKeyBoard()
        {
            try
            {
                // retrieve the handler of the window  
                int iHandle = FindWindow("IPTIP_Main_Window", "");
                if (iHandle > 0)
                {
                    // close the window using API        
                    SendMessage(iHandle, WM_SYSCOMMAND, SC_CLOSE, 0);
                }
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                // WPFMessageBoxResult _Result = WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
                MessageBox.Show(ex.Message.ToString());
            }
        }

        //added by kashif abbasi on dated 17-Nov-2015
        public static ImageSource BitmapFromUri(Uri source)
        {
            try
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = source;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                return bitmap;
            }
            catch (Exception ex)
            {

                MessageBox.Show("getting error while getting the image!. Contact with Administrator. "+ex.Message );
                App.VSDLog.Info("\n get error in CommonUtils.BitmapFormUri("+source.ToString()+"). "+ex.Message);
                return null;
            }
            
        }

        //added by kashif abbasi on dated 19-Nov-2015
        public static bool deleteFiles(string[] filePath)
        {
            try
            {
                if (filePath != null && filePath.Count() > 0)
                {
                    foreach (string path in filePath)
                    {
                        if (File.Exists(path))
                        {
                            File.Delete(path);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {               
                App.VSDLog.Info("\n get error in CommonUtils.deleteFiles(string[] filePath). " + ex.Message);
                return false;
            }

        }

        //added by kashif abbasi on dated 19-Nov-2015
        public static bool renameImgDirectory(string newDirName)
        {
            string strPath = Properties.Settings.Default.violationImagesPath;
            try
            {
                strPath += @"\" + DateTime.Now.Date.ToString("MMM") + DateTime.Now.Year;
                strPath += @"\" + DateTime.Now.Date.ToString("ddMMyyyy");
                strPath += @"\" + AppProperties.vehicle.Country.Replace(" ", "") + "_" + AppProperties.vehicle.PlateNumber.Trim() + "_" +
                           AppProperties.vehicle.PlateCategory.Replace(" ", "") + "_" + AppProperties.vehicle.PlateCode.Replace(" ", "");

                if (Directory.Exists(strPath))
                {
                    //Make new Dir Name
                    newDirName = strPath.Substring(0, strPath.LastIndexOf("\\")+1)+newDirName.Replace(".","_");
                    Directory.Move(strPath, newDirName);
                }
                return true;
            }
            catch (Exception ex)
            {
                App.VSDLog.Info("\n get error in CommonUtils.renameDirectory(string dirPath). " + ex.Message);
                return false;
            }

        }

        //added by kashif abbasi on dated 22-Nov-2015
        public static bool deleteImgDirectory(string strCurrentScreenName)
        {
            string strPath = Properties.Settings.Default.violationImagesPath;
            bool result = false;
            try
            {                
                if (AppProperties.vehicle != null)
                {
                    strPath += @"\" + DateTime.Now.Date.ToString("MMM") + DateTime.Now.Year;
                    strPath += @"\" + DateTime.Now.Date.ToString("ddMMyyyy");
                    strPath += @"\" + AppProperties.vehicle.Country.Replace(" ", "") + "_" + AppProperties.vehicle.PlateNumber.Trim() + "_" +
                               AppProperties.vehicle.PlateCategory.Replace(" ", "") + "_" + AppProperties.vehicle.PlateCode.Replace(" ", "");

                    if (Directory.Exists(strPath))
                    {
                        if (strCurrentScreenName.Equals("ucDefectAndViolationDetails"))
                        {
                            if (WPFMessageBox.Show(new CommonUtils().GetStringValue("Confirmation"), new CommonUtils().GetStringValue("ScreenChangeConfirmation"), WPFMessageBoxButtons.YesNo, WPFMessageBoxImage.Question) == WPFMessageBoxResult.Yes)
                            {
                                Directory.Delete(strPath, true);
                                result = true;
                            }
                        }
                        else
                        {
                            Directory.Delete(strPath, true);
                            result = true;
                        }
                    }
                    else 
                    {
                        result = true;
                    }
                }
                else
                {
                    result = true;
                }

                return result;
            }
            catch (Exception ex)
            {
                App.VSDLog.Info("\n get error in CommonUtils.deleteImgDirectory(). " + ex.Message);
                return result;
            }

        }       
//added by kashif abbasi on dated 31-dec-2015
        public static string getImgOFInspector(string inspectorName)
        {
            string inspector_pic_path = @"/Images/Inspectors/";
            try
            {
                if (AppProperties.Selected_Resource == "Arabic")
                {
                    inspector_pic_path = inspector_pic_path + inspectorName.Trim().ToLower() + "_Arabic.png";
                }
                else
                {
                    inspector_pic_path = inspector_pic_path + AppProperties.empUserName.ToString().Trim().ToLower() + ".png";
                }
            }
            catch (Exception ex)
            { 
            
            }
            return inspector_pic_path;
        }
    }
}
