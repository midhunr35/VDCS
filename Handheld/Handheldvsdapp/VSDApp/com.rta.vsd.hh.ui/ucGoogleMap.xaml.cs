using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml.Linq;
using Microsoft.Win32;

namespace VSDApp.com.rta.vsd.hh.ui
{
    /// <summary>
    /// Interaction logic for ucGoogleMap.xaml
    /// </summary>
    public partial class ucGoogleMap : UserControl
    {
        public ucGoogleMap()
        {
            InitializeComponent();

        }
        #region Fields
        private XDocument geoDoc;
        private string location;
        private int zoom;
        private SaveFileDialog saveDialog = new SaveFileDialog();
        private string mapType;
        private double lat;
        #endregion
        private double lng;

        private void GetGeocodeData()
        {
            string geocodeURL = "http://maps.googleapis.com/maps/api/" + "geocode/xml?address=" + location + "&sensor=false";
            try
            {
                geoDoc = XDocument.Load(geocodeURL);

            }
            catch (WebException ex)
            {
                this.Dispatcher.BeginInvoke(new ThreadStart(HideProgressBar), DispatcherPriority.Normal, null);
                MessageBox.Show("Ensure that internet connection is available.", "Map App", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            this.Dispatcher.BeginInvoke(new ThreadStart(ShowGeocodeData), DispatcherPriority.Normal, null);
        }

        private void ShowGeocodeData()
        {
            /*
            dynamic responseStatus = geoDoc.Single.Value();
            if ((responseStatus == "OK")) {
                dynamic formattedAddress = geoDoc(0).Value();
                dynamic latitude = geoDoc(0).Element("lat").Value();
                dynamic longitude = geoDoc(0).Element("lng").Value();
                dynamic locationType = geoDoc(0).Value();

                AddressTxtBlck.Text = formattedAddress;
                LatitudeTxtBlck.Text = latitude;
                LongitudeTxtBlck.Text = longitude;

                switch (locationType) {
                    case "APPROXIMATE":
                        AccuracyTxtBlck.Text = "Approximate";
                        break;
                    case "ROOFTOP":
                        AccuracyTxtBlck.Text = "Precise";
                        break;
                    default:
                        AccuracyTxtBlck.Text = "Approximate";
                        break;
                }

                lat = double.Parse(latitude);
                lng = double.Parse(longitude);

                if ((SaveButton.IsEnabled == false)) {
                    SaveButton.IsEnabled = true;
                    RoadmapToggleButton.IsEnabled = true;
                    TerrainToggleButton.IsEnabled = true;
                }

            } else if ((responseStatus == "ZERO_RESULTS")) {
                MessageBox.Show("Unable to show results for: " + Constants.vbCrLf + location, "Unknown Location", MessageBoxButton.OK, MessageBoxImage.Information);
                DisplayXXXXXXs();
                AddressTxtBox.SelectAll();
            }*/
            ShowMapButton.IsEnabled = true;
            ZoomInButton.IsEnabled = true;
            ZoomOutButton.IsEnabled = true;
            MapProgressBar.Visibility = System.Windows.Visibility.Hidden;
        }

        // Get and display map image in Image ctrl.
        private void ShowMapImage()
        {
            BitmapImage bmpImage = new BitmapImage();
            string mapURL = "http://maps.googleapis.com/maps/api/staticmap?" + "size=500x400&markers=size:mid%7Ccolor:red%7C" + location + "&zoom=" + zoom + "&maptype=" + mapType + "&sensor=false";

            bmpImage.BeginInit();
            bmpImage.UriSource = new Uri(mapURL);
            bmpImage.EndInit();

            MapImage.Source = bmpImage;
        }

        private void ShowMapUsingLatLng()
        {
            BitmapImage bmpImage = new BitmapImage();
            string mapURL = "http://maps.googleapis.com/maps/api/staticmap?" + "center=" + lat + "," + lng + "&" + "size=500x400&markers=size:mid%7Ccolor:red%7C" + location + "&zoom=" + zoom + "&maptype=" + mapType + "&sensor=false";
            bmpImage.BeginInit();
            bmpImage.UriSource = new Uri(mapURL);
            bmpImage.EndInit();

            MapImage.Source = bmpImage;
        }

        // Zoom-in on map.
        private void ZoomIn()
        {
            if ((zoom < 21))
            {
                zoom += 1;
                ShowMapUsingLatLng();

                if ((ZoomOutButton.IsEnabled == false))
                {
                    ZoomOutButton.IsEnabled = true;
                }
            }
            else
            {
                ZoomInButton.IsEnabled = false;
            }
        }

        // Zoom-out on map.
        private void ZoomOut()
        {
            if ((zoom > 0))
            {
                zoom -= 1;
                ShowMapUsingLatLng();

                if ((ZoomInButton.IsEnabled == false))
                {
                    ZoomInButton.IsEnabled = true;
                }
            }
            else
            {
                ZoomOutButton.IsEnabled = false;
            }
        }

        private void SaveMap()
        {
            string mapURL = "http://maps.googleapis.com/maps/api/staticmap?" + "center=" + lat + "," + lng + "&" + "size=500x400&markers=size:mid%7Ccolor:red%7C" + location + "&zoom=" + zoom + "&maptype=" + mapType + "&sensor=false";
            WebClient webClient = new WebClient();
            try
            {
                byte[] imageBytes = webClient.DownloadData(mapURL);
                using (MemoryStream ms = new MemoryStream(imageBytes))
                {
                    //Image.FromStream(ms).Save(saveDialog.FileName, System.Drawing.Imaging.ImageFormat.Png);
                }
            }
            catch (WebException ex)
            {
                MessageBox.Show("Unable to save map. Ensure that you are" + " connected to the internet.", "Error!", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }
        }

        private void MoveUp()
        {
            // Default zoom is 15 and at this level changing
            // the center point is done by 0.003 degrees. 
            // Shifting the center point is done by higher values
            // at zoom levels less than 15.
            double diff = 0;
            double shift = 0;
            // Use 88 to avoid values beyond 90 degrees of lat.
            if ((lat < 88))
            {
                if ((zoom == 15))
                {
                    lat += 0.003;
                }
                else if ((zoom > 15))
                {
                    diff = zoom - 15;
                    shift = ((15 - diff) * 0.003) / 15;
                    lat += shift;
                }
                else
                {
                    diff = 15 - zoom;
                    shift = ((15 + diff) * 0.003) / 15;
                    lat += shift;
                }
                ShowMapUsingLatLng();
            }
            else
            {
                lat = 90;
            }
        }

        private void MoveDown()
        {
            double diff = 0;
            double shift = 0;
            if ((lat > -88))
            {
                if ((zoom == 15))
                {
                    lat -= 0.003;
                }
                else if ((zoom > 15))
                {
                    diff = zoom - 15;
                    shift = ((15 - diff) * 0.003) / 15;
                    lat -= shift;
                }
                else
                {
                    diff = 15 - zoom;
                    shift = ((15 + diff) * 0.003) / 15;
                    lat -= shift;
                }
                ShowMapUsingLatLng();
            }
            else
            {
                lat = -90;
            }
        }

        private void MoveLeft()
        {
            double diff = 0;
            double shift = 0;
            // Use -178 to avoid negative values below -180.
            if ((lng > -178))
            {
                if ((zoom == 15))
                {
                    lng -= 0.003;
                }
                else if ((zoom > 15))
                {
                    diff = zoom - 15;
                    shift = ((15 - diff) * 0.003) / 15;
                    lng -= shift;
                }
                else
                {
                    diff = 15 - zoom;
                    shift = ((15 + diff) * 0.003) / 15;
                    lng -= shift;
                }
                ShowMapUsingLatLng();
            }
            else
            {
                lng = 180;
            }
        }

        private void MoveRight()
        {
            double diff = 0;
            double shift = 0;
            if ((lng < 178))
            {
                if ((zoom == 15))
                {
                    lng += 0.003;
                }
                else if ((zoom > 15))
                {
                    diff = zoom - 15;
                    shift = ((15 - diff) * 0.003) / 15;
                    lng += shift;
                }
                else
                {
                    diff = 15 - zoom;
                    shift = ((15 + diff) * 0.003) / 15;
                    lng += shift;
                }
                ShowMapUsingLatLng();
            }
            else
            {
                lng = -180;
            }
        }

        private void DisplayXXXXXXs()
        {
            AddressTxtBlck.Text = "XXXXXXXXX, XXXXX, XXXXXX";
            LatitudeTxtBlck.Text = "XXXXXXXXXX";
            LongitudeTxtBlck.Text = "XXXXXXXXXX";
            AccuracyTxtBlck.Text = "XXXXXXXXX";
        }

        private void HideProgressBar()
        {
            MapProgressBar.Visibility = System.Windows.Visibility.Hidden;
            ShowMapButton.IsEnabled = true;
        }
        public void MoveMapToLocation(String locationName)
        {
            AddressTxtBox.Text = locationName;
            if ((AddressTxtBox.Text != string.Empty))
            {
                location = AddressTxtBox.Text.Replace(" ", "+");
                zoom = 15;
                mapType = "roadmap";
                Thread geoThread = new Thread(GetGeocodeData);
                geoThread.Start();

                ShowMapImage();
                AddressTxtBox.SelectAll();
                ShowMapButton.IsEnabled = false;
                MapProgressBar.Visibility = System.Windows.Visibility.Visible;

                if ((RoadmapToggleButton.IsChecked == false))
                {
                    RoadmapToggleButton.IsChecked = true;
                    TerrainToggleButton.IsChecked = false;
                }
                MapProgressBar.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                MessageBox.Show("Enter location address.", "Map App", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                AddressTxtBox.Focus();
            }
        }
        // ShowMapButton click event handler.
        private void ShowMapButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if ((AddressTxtBox.Text != string.Empty))
            {
                location = AddressTxtBox.Text.Replace(" ", "+");
                zoom = 15;
                mapType = "roadmap";
                Thread geoThread = new Thread(GetGeocodeData);
                geoThread.Start();

                ShowMapImage();
                AddressTxtBox.SelectAll();
                ShowMapButton.IsEnabled = false;
                MapProgressBar.Visibility = System.Windows.Visibility.Visible;

                if ((RoadmapToggleButton.IsChecked == false))
                {
                    RoadmapToggleButton.IsChecked = true;
                    TerrainToggleButton.IsChecked = false;
                }
                MapProgressBar.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                MessageBox.Show("Enter location address.", "Map App", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                AddressTxtBox.Focus();
            }
        }

        // SaveFileDialog FileOk event handler.
        private void saveDialog_FileOk(object sender, EventArgs e)
        {
            Thread td = new Thread(SaveMap);
            td.Start();
        }

        // ZoomInButton click event handler.
        private void ZoomInButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ZoomIn();
        }

        // ZoomOutButton click event handler.
        private void ZoomOutButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ZoomOut();
        }

        // SaveButton click event handler.
        private void SaveButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            saveDialog.ShowDialog();
        }

        // RoadmapToggleButton Checked event handler.
        private void RoadmapToggleButton_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            if ((mapType != "roadmap"))
            {
                mapType = "roadmap";
                ShowMapUsingLatLng();
                TerrainToggleButton.IsChecked = false;
            }
        }

        // TerrainToggleButton Checked event handler.
        private void TerrainToggleButton_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            if ((mapType != "terrain"))
            {
                mapType = "terrain";
                ShowMapUsingLatLng();
                RoadmapToggleButton.IsChecked = false;
            }
        }

        private void MapImage_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if ((location != null))
            {
                string gMapURL = "http://maps.google.com/maps?q=" + location;
                Process.Start("IExplore.exe", gMapURL);
            }
        }

        private void Window1_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            AddressTxtBox.Focus();

            var _with1 = saveDialog;
            _with1.DefaultExt = "png";
            _with1.Title = "Save Map Image";
            _with1.OverwritePrompt = true;
            _with1.Filter = "(*.png)|*.png";

            saveDialog.FileOk += saveDialog_FileOk;
            MoveMapToLocation("Dubai");
        }

        private void MinimizeButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
           // this.WindowState = System.Windows.WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
           // this.Close();
        }

        private void BgndRectangle_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
           // this.DragMove();
        }

        private void MoveUpButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MoveUp();
        }

        private void MoveDownButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MoveDown();
        }

        private void MoveLeftButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MoveLeft();
        }

        private void MoveRightButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MoveRight();
        }

    }
}