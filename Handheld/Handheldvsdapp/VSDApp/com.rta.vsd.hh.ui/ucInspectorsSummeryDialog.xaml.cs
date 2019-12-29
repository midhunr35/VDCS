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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Windows.Threading;
using System.Collections.ObjectModel;
using VSDApp.com.rta.vsd.hh.db;
using System.Data;
using VSDApp.com.rta.vsd.hh.utilities;
using VSDApp.com.rta.vsd.hh.data;
using VSDApp.WPFMessageBoxControl;
using System.Windows.Controls.DataVisualization.Charting;
using System.Collections;
using System.IO;

namespace VSDApp.com.rta.vsd.hh.ui
{
    /// <summary>
    /// Interaction logic for ucInspectorsSummeryDialog.xaml
    /// </summary>
    public partial class ucInspectorsSummeryDialog : UserControl
    {

        public ucInspectorsSummeryDialog()
        {
            InitializeComponent();
            Visibility = Visibility.Hidden;


        }

        private int _noOfInspToShowOnGraph = 9;
        private bool _hideRequest = false;
        private string _result = "";
        private UIElement _parent;

        Dictionary<string, int> _htInspections = null;
        Dictionary<string, int> _htViolations = null;
        private int _currentInspectorRating = 0;
        // private List<DisplayObject> violationData;

        public void SetParent(UIElement parent)
        {
            _parent = parent;
        }

        #region Message

        public string Message_InspectorDialog
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Message.
        // This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register(
                "Message", typeof(string), typeof(ucInspectorsSummeryDialog), new UIPropertyMetadata(string.Empty));

        #endregion

        public string ShowHandlerDialog(string message, DataTable dtInspectors)
        {
            try
            {
                chngBtnsImgs();
                string strImgPath = CommonUtils.getImgOFInspector(AppProperties.empUserName.ToString());
                imgInspector.Source = new BitmapImage(new Uri(strImgPath, UriKind.Relative));
                PopulateInspectorsGraph(dtInspectors);

                imgInspector.ToolTip = AppProperties.empUserName.ToString();

                Message_InspectorDialog = message;
                Visibility = Visibility.Visible;
                _parent.IsEnabled = false;
                _hideRequest = false;
                while (!_hideRequest)
                {
                    // HACK: Stop the thread if the application is about to close
                    if (this.Dispatcher.HasShutdownStarted ||
                        this.Dispatcher.HasShutdownFinished)
                    {
                        break;
                    }

                    // HACK: Simulate "DoEvents"
                    this.Dispatcher.Invoke(
                        DispatcherPriority.Background,
                        new ThreadStart(delegate { }));
                    Thread.Sleep(20);
                }
                System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.DigitSubstitution = System.Globalization.DigitShapes.None;
                this.UpdateLayout();
            }
            catch (Exception ex)
            {

            }
            return _result;
        }

        private void chngBtnsImgs()
        {
            if (AppProperties.Selected_Resource == "English")
            {
                btnOk.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Back.png", UriKind.Relative));
            }
            else
            {
                btnOk.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/back Arabic Up.png", UriKind.Relative));
            }
        }

        public void PopulateInspectorsGraph(DataTable dt)
        {
            try
            {

                _htInspections = new Dictionary<string, int>();
                _htViolations = new Dictionary<string, int>();
                dt.DefaultView.Sort = "totalInspections DESC";
                dt = dt.DefaultView.ToTable();

                int countInsp = 1;
                bool isLogedInInspectorAddedInHt = false;
                foreach (DataRow dr in dt.Rows)
                {
                    if (countInsp >= _noOfInspToShowOnGraph)
                    {
                        if (isLogedInInspectorAddedInHt)
                        {
                            break;
                        }
                        else
                        {
                            //adding loggedIn inspector in hashtables.... to show on graph;
                            if (dr["inspectorId"].ToString().Trim().ToUpper().Equals(AppProperties.empID.ToString().Trim().ToUpper()))
                            {
                                addRowDataInHashTable(dr);
                                isLogedInInspectorAddedInHt = true;
                                _currentInspectorRating = countInsp++;
                                break;
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }

                    addRowDataInHashTable(dr);

                    if (dr["inspectorId"].ToString().Trim().ToUpper().Equals(AppProperties.empID.ToString().Trim().ToUpper()))
                    {
                        isLogedInInspectorAddedInHt = true;
                        _currentInspectorRating = countInsp;
                    }

                    countInsp++;
                }

                ((ColumnSeries)mcChart.Series[0]).ItemsSource = _htInspections;
                ((ColumnSeries)mcChart.Series[1]).ItemsSource = _htViolations;
                lblInspectorRating.Text = "Rating: " + _currentInspectorRating.ToString();
                System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.DigitSubstitution = System.Globalization.DigitShapes.None;
                this.UpdateLayout();
            }
            catch (Exception ex)
            {
                App.VSDLog.Info("\n Error in ucinspectorsSummeryDialog.PopulateInspectorsGraph(dt) \n" + ex.Message + "\n" + ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }



        }

        private void addRowDataInHashTable(DataRow dr)
        {
            App.VSDLog.Info("Enter in ucInspectorsSummeryDialog.addRowDataInHashTable()");
            string strInspectorName = "";
            try
            {
                if (AppProperties.Selected_Resource == "English")
                    strInspectorName = dr["inspectorName"].ToString().Substring(0, dr["inspectorName"].ToString().IndexOf(" "));
                else
                    strInspectorName = dr["inspectorNameArabic"].ToString().Substring(0, dr["inspectorNameArabic"].ToString().IndexOf(" "));
                //if inspector first name already exist in hashtable as a KEY, than we use his/her UserName for KEY
                //if (_htInspections.ContainsKey(strInspectorName))
                //    strInspectorName = dr["inspectorUserName"].ToString().Trim();
                //x-Axis label should be InspectorName\nInspectorID
                strInspectorName += "\n" + dr["inspectorId"].ToString();
                _htInspections.Add(strInspectorName, Convert.ToInt16(dr["totalInspections"]));
                _htViolations.Add(strInspectorName, Convert.ToInt16(dr["totalViolations"]));
                System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.DigitSubstitution = System.Globalization.DigitShapes.None;
                this.UpdateLayout();
            }
            catch (Exception ex)
            {
                App.VSDLog.Info("Error in ucInspectorsSummeryDialog.addRowDataInHashTable() while adding Key(" + strInspectorName + ") in hashtable: " + ex.Message + "\n" + ex.StackTrace);
            }
        }

        private void HideHandlerDialog()
        {
            _hideRequest = true;
            Visibility = Visibility.Hidden;
            _parent.IsEnabled = true;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            _result = "";
            HideHandlerDialog();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            _result = "";
            HideHandlerDialog();
        }


        private void btnOk_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (AppProperties.Selected_Resource == "English")
            {
                btnOk.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Back Down.png", UriKind.Relative));
            }
            else
            {
                btnOk.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/back Arabic Down.png", UriKind.Relative));
            }
        }

        private void root_Initialized(object sender, EventArgs e)
        {
            //System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.DigitSubstitution = System.Globalization.DigitShapes.None;
            this.UpdateLayout();
        }


    }

}
