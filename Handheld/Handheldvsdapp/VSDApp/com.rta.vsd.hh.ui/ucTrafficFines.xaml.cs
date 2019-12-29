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
using VSDApp.com.rta.vsd.hh.data;
using VSDApp.com.rta.vsd.hh.utilities;
using VSDApp.WPFMessageBoxControl;

namespace VSDApp.com.rta.vsd.hh.ui
{
    /// <summary>
    /// Interaction logic for ucTrafficFines.xaml
    /// </summary>
    public partial class ucTrafficFines : UserControl
    {
        List<TrafficeFines> TrafficFineList;
        TrafficeFines fine;
        MainWindow m_MainWindow = null;
        public ucTrafficFines(MainWindow m_mainWnd)
        {
            InitializeComponent();
            m_MainWindow = m_mainWnd; ;
        }

        private void imagebtnback_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
            if (AppProperties.Selected_Resource == "English")
            {
                imagebtnback.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Back.png", UriKind.Relative));
            }
            else
            {
                imagebtnback.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/back Arabic Up.png", UriKind.Relative));
            }

            //this.Close();
            m_MainWindow.MainContentControl.Content = null;
            m_MainWindow.MainContentControl.Content = new ucWellComeScreen(m_MainWindow);
        }

        private void imagebtnback_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            if (AppProperties.Selected_Resource == "English")
            {
                imagebtnback.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Back Down.png", UriKind.Relative));
            }
            else
            {
                imagebtnback.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/back Arabic Down.png", UriKind.Relative));
            }
        }

        public void ShowEnglihColumns()
        {
            this.headBlackPointEn.Visibility = System.Windows.Visibility.Visible;
            this.headDescriptionEN.Visibility = System.Windows.Visibility.Visible;
            this.headFineAmmountEn.Visibility = System.Windows.Visibility.Visible;
            this.headTypeEn.Visibility = System.Windows.Visibility.Visible;
            this.headVehicelConfisPeriodEn.Visibility = System.Windows.Visibility.Visible;

            this.headBlackPointsAr.Visibility = System.Windows.Visibility.Collapsed;
            this.headDesctiptionAr.Visibility = System.Windows.Visibility.Collapsed;
            this.headFineAmmountAr.Visibility = System.Windows.Visibility.Collapsed;
            this.headTypeAr.Visibility = System.Windows.Visibility.Collapsed;
            this.headVehicelConfisPeriodAr.Visibility = System.Windows.Visibility.Collapsed;
        }
        public void ShowArabicColumns()
        {
            this.headBlackPointEn.Visibility = System.Windows.Visibility.Collapsed;
            this.headDescriptionEN.Visibility = System.Windows.Visibility.Collapsed;
            this.headFineAmmountEn.Visibility = System.Windows.Visibility.Collapsed;
            this.headTypeEn.Visibility = System.Windows.Visibility.Collapsed;
            this.headVehicelConfisPeriodEn.Visibility = System.Windows.Visibility.Collapsed;

            this.headBlackPointsAr.Visibility = System.Windows.Visibility.Visible;
            this.headDesctiptionAr.Visibility = System.Windows.Visibility.Visible;
            this.headFineAmmountAr.Visibility = System.Windows.Visibility.Visible;
            this.headTypeAr.Visibility = System.Windows.Visibility.Visible;
            this.headVehicelConfisPeriodAr.Visibility = System.Windows.Visibility.Visible;
        }


        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (AppProperties.Selected_Resource == "English")
                {
                    imagebtnback.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Back.png", UriKind.Relative));
                    ShowEnglihColumns();

                }
                else
                {
                    imagebtnback.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/back Arabic Up.png", UriKind.Relative));
                    ShowArabicColumns();
                }
                TrafficFineList = new List<TrafficeFines>(148);
                ///////////////////////////////
                //Traffic
                ////////////////////
                ////Traffic





                #region Technical Fines












                //////////////////////////////////////////////////////////////








                /////////////////////////////////////
                ///Technical
                //////////////////////////////
                fine = new TrafficeFines();
                fine.DescriptionEn = "Driving a heavy vehicle that does not comply with safety and security conditions";
                fine.FineAmmount = "500";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "30 Days";
                fine.Type = "Technical";
                fine.TypeAr = "فنية";
                fine.VehicleConfiscationPeriodAr = "30 يوم";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "500";
                fine.DescriptionAr = "قيادة مركبة ثقيلة لا تتوفر فيها شروط الأمن والسلامة";
                TrafficFineList.Add(fine);


                fine = new TrafficeFines();
                fine.DescriptionEn = "Driving a noisy vehicle";
                fine.FineAmmount = "500";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "30 Days";
                fine.Type = "Technical";
                fine.TypeAr = "فنية";
                fine.VehicleConfiscationPeriodAr = "30 يوم";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "500";
                fine.DescriptionAr = "قيادة مركبة تسبب ضجيج";
                TrafficFineList.Add(fine);


                fine = new TrafficeFines();
                fine.DescriptionEn = "Loading a heavy vehicle in a way that may pose danger to others or to the road";
                fine.FineAmmount = "500";
                fine.BlackPoints = "6";
                fine.VehicleConfiscationPeriod = "7 Days";
                fine.Type = "Technical";
                fine.TypeAr = "فنية";
                fine.VehicleConfiscationPeriodAr = "6 يوم";
                fine.BlackPointsAr = "6";
                fine.FineAmmountsAr = "500";
                fine.DescriptionAr = "تحميل المركبة الثقيلة بصورة تشكل خطورة على الغير ، أو تلحق أضرار بالطريق  ";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Overload or protruding load from a heavy vehicle without permission";
                fine.FineAmmount = "500";
                fine.BlackPoints = "6";
                fine.VehicleConfiscationPeriod = "7 Days";
                fine.Type = "Technical";
                fine.TypeAr = "فنية";
                fine.VehicleConfiscationPeriodAr = "6 يوم";
                fine.BlackPointsAr = "6";
                fine.FineAmmountsAr = "500";
                fine.DescriptionAr = "الحمولة الزائدة، أو بروز الحمولة في المركبات الثقيلة عمّا هو مقرر دون ترخيص";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Driving dangerously (racing)";
                fine.FineAmmount = "2000";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "30 Days";
                fine.Type = "Technical";
                fine.TypeAr = "فنية";
                fine.VehicleConfiscationPeriodAr = "30 يوم";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "2000";
                fine.DescriptionAr = "قيادة المركبة بصورة تشكل خطرا  ";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Driving a vehicle that causes pollution";
                fine.FineAmmount = "500";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Technical";
                fine.TypeAr = "فنية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "500";
                fine.DescriptionAr = "قيادة مركبة تحدث تلوثاً للبيئة";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Modifying vehicle’s engine without permission";
                fine.FineAmmount = "400";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Technical";
                fine.TypeAr = "فنية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "400";
                fine.DescriptionAr = "إحداث تغييرات في محرك المركبة بدون ترخيص  ";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Driving dangerously (racing)";
                fine.FineAmmount = "2000";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "30 Days";
                fine.Type = "Technical";
                fine.TypeAr = "فنية";
                fine.VehicleConfiscationPeriodAr = "30 يوم";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "2000";
                fine.DescriptionAr = "قيادة المركبة بصورة تشكل خطرا  ";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Modifying vehicle’s chasses without permission";
                fine.FineAmmount = "400";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Technical";
                fine.TypeAr = "فنية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "400";
                fine.DescriptionAr = "إحداث تغييرات في قاعدة ( شاسى ) بدون ترخيص";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Changing vehicle’s color without permission";
                fine.FineAmmount = "400";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Technical";
                fine.TypeAr = "فنية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "400";
                fine.DescriptionAr = "إحداث تغييرات في لون المركبة بدون ترخيص  ";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Driving dangerously (racing)";
                fine.FineAmmount = "2000";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "30 Days";
                fine.Type = "Technical";
                fine.TypeAr = "فنية";
                fine.VehicleConfiscationPeriodAr = "30 يوم";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "2000";
                fine.DescriptionAr = "قيادة المركبة بصورة تشكل خطرا  ";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Driving a vehicle that omits gases or fumes with substances exceeding permitted rates";
                fine.FineAmmount = "300";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Technical";
                fine.TypeAr = "فنية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "300";
                fine.DescriptionAr = "قيادة مركبة تطلق غازات أو أبخرة تحتوي على مركّبات تزيد على النسب المقررة  ";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "No lights on the back or sides of trailer container";
                fine.FineAmmount = "200";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Technical";
                fine.TypeAr = "فنية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "200";
                fine.DescriptionAr = "عدم وجود المصابيح خلف المقطورة ، أو على جوانبها   ";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Lights on the back or sides of container not working";
                fine.FineAmmount = "200";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Technical";
                fine.TypeAr = "فنية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "200";
                fine.DescriptionAr = "عدم صلاحية المصابيح خلف المقطورة، أو على جوانبها   ";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Vehicle unfit for driving";
                fine.FineAmmount = "200";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "7 Days";
                fine.Type = "Technical";
                fine.TypeAr = "فنية";
                fine.VehicleConfiscationPeriodAr = "7 يوم";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "200";
                fine.DescriptionAr = "عدم صلاحية المركبة للسير  ";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Not lifting exhaust of trucks";
                fine.FineAmmount = "200";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "7 Days";
                fine.Type = "Technical";
                fine.TypeAr = "فنية";
                fine.VehicleConfiscationPeriodAr = "7 يوم";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "200";
                fine.DescriptionAr = "عدم رفع جهاز العادم في الشاحنات  ";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Not covering loads of trucks";
                fine.FineAmmount = "3000";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "7 Days";
                fine.Type = "Technical";
                fine.TypeAr = "فنية";
                fine.VehicleConfiscationPeriodAr = "7 يوم";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "3000";
                fine.DescriptionAr = "عدم تغطية الحمولة في الشاحنات  ";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Using vehicle for purposes other than designated";
                fine.FineAmmount = "200";
                fine.BlackPoints = "4";
                fine.VehicleConfiscationPeriod = "7 Days";
                fine.Type = "Technical";
                fine.TypeAr = "فنية";
                fine.VehicleConfiscationPeriodAr = "7 يوم";
                fine.BlackPointsAr = "4";
                fine.FineAmmountsAr = "200";
                fine.DescriptionAr = "استعمال المركبة في غير الغرض المخصص له  ";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Overload or protruding load on light vehicles without permission";
                fine.FineAmmount = "200";
                fine.BlackPoints = "3";
                fine.VehicleConfiscationPeriod = "7 Days";
                fine.Type = "Technical";
                fine.TypeAr = "فنية";
                fine.VehicleConfiscationPeriodAr = "7 يوم";
                fine.BlackPointsAr = "3";
                fine.FineAmmountsAr = "200";
                fine.DescriptionAr = "الحمولة الزائدة ، أو بروز الحمولة في المركبات الخفيفة عمّا هو مقرر دون ترخيص";
                TrafficFineList.Add(fine);


                fine = new TrafficeFines();
                fine.DescriptionEn = "Falling or leaking load";
                fine.FineAmmount = "3000";
                fine.BlackPoints = "12";
                fine.VehicleConfiscationPeriod = "30 Days";
                fine.Type = "Technical";
                fine.TypeAr = "فنية";
                fine.VehicleConfiscationPeriodAr = "30 يوم";
                fine.BlackPointsAr = "12";
                fine.FineAmmountsAr = "3000";
                fine.DescriptionAr = "تسرب أو تساقط أشياء من حمولة المركبة  ";
                TrafficFineList.Add(fine);


                fine = new TrafficeFines();
                fine.DescriptionEn = "Using multi-colored lights";
                fine.FineAmmount = "200";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Technical";
                fine.TypeAr = "فنية";
                fine.VehicleConfiscationPeriodAr = "";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "200";
                fine.DescriptionAr = "استعمال أنوار دوارة متعددة الألوان  ";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Driving with tires in poor condition";
                fine.FineAmmount = "200";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "7 Days";
                fine.Type = "Technical";
                fine.TypeAr = "فنية";
                fine.VehicleConfiscationPeriodAr = "7 يوم";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "200";
                fine.DescriptionAr = "عدم صلاحية إطارات المركبة أثناء السير  ";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Not fixing number plates in designated places";
                fine.FineAmmount = "200";
                fine.BlackPoints = "2";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Technical";
                fine.TypeAr = "فنية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "2";
                fine.FineAmmountsAr = "200";
                fine.DescriptionAr = "عدم تثبيت لوحات الأرقام في المكان المخصص لها  ";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Using not matching number plates for trailer and container";
                fine.FineAmmount = "200";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Technical";
                fine.TypeAr = "فنية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "2000";
                fine.DescriptionAr = "اختلاف لوحات الأرقام بين القاطرة والمقطورة ، وشبه المقطورة  ";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Not fixing reflective stickers at the back of trucks and heavy vehicles";
                fine.FineAmmount = "200";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Technical";
                fine.TypeAr = "فنية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "200";
                fine.DescriptionAr = "عدم تثبيت ملصقات عاكسة بالمؤخرة للشاحنات، ومركبات النقل  ";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Failure to have vehicle examined after carrying out major modification to engine or body";
                fine.FineAmmount = "200";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "7 Days";
                fine.Type = "Technical";
                fine.TypeAr = "فنية";
                fine.VehicleConfiscationPeriodAr = "30 يوم";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "200";
                fine.DescriptionAr = "عدم فحص المركبة بعد إجراء أي تعديل جوهري في محركها أو هيكلها  ";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Number plates with unclear numbers";
                fine.FineAmmount = "200";
                fine.BlackPoints = "3";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Technical";
                fine.TypeAr = "فنية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "3";
                fine.FineAmmountsAr = "200";
                fine.DescriptionAr = "عدم وضوح أرقام اللوحات  ";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Not fixing a sign indicating licensed overload";
                fine.FineAmmount = "200";
                fine.BlackPoints = "3";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Technical";
                fine.TypeAr = "فنية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "3";
                fine.FineAmmountsAr = "2000";
                fine.DescriptionAr = "عدم تعليق ما يشير إلى الحمولة الزائدة المرخص بها  ";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Broken lights";
                fine.FineAmmount = "200";
                fine.BlackPoints = "6";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Technical";
                fine.TypeAr = "فنية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "6";
                fine.FineAmmountsAr = "200";
                fine.DescriptionAr = "عدم صلاحية أنوار الإضاءة  ";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Not displaying truck’s load on both sides";
                fine.FineAmmount = "100";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Technical";
                fine.TypeAr = "فنية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "100";
                fine.DescriptionAr = "عدم كتابة حمولة الشاحنة على جانبيه  ";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Broken indicator lights";
                fine.FineAmmount = "100";
                fine.BlackPoints = "2";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Technical";
                fine.TypeAr = "فنية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "2";
                fine.FineAmmountsAr = "100";
                fine.DescriptionAr = "عدم صلاحية إشارات تغيير الاتجاه  ";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Driving dangerously (racing)";
                fine.FineAmmount = "2000";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "30 Days";
                fine.Type = "Technical";
                fine.TypeAr = "فنية";
                fine.VehicleConfiscationPeriodAr = "30 يوم";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "2000";
                fine.DescriptionAr = "قيادة المركبة بصورة تشكل خطرا  ";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Having no red light at the back of vehicle";
                fine.FineAmmount = "100";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Technical";
                fine.TypeAr = "فنية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "100";
                fine.DescriptionAr = "عدم وجود نور أحمر بمؤخرة المركبة  ";
                TrafficFineList.Add(fine);


                #endregion


                #region Traffic Fines


                ///////////////////////////////////////////////////////////////////////////
                //////////////////////////////////////////////////////////////////
                fine = new TrafficeFines();
                fine.DescriptionEn = "Driving under the influence of alcohol, drugs or similar substances";
                fine.FineAmmount = "Decided by Court";
                fine.BlackPoints = "24";
                fine.VehicleConfiscationPeriod = "60 Days";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "60 يوم";
                fine.BlackPointsAr = "24";
                fine.FineAmmountsAr = "تقررها المحكمة";
                fine.DescriptionAr = "قيادة مركبة تحت تأثير الكحول، أو المخدر أو ما في حكمه ";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Driving a vehicle without number plates";
                fine.FineAmmount = "1000";
                fine.BlackPoints = "24";
                fine.VehicleConfiscationPeriod = "60 Days";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "60 يوم";
                fine.BlackPointsAr = "24";
                fine.FineAmmountsAr = "1000";
                fine.DescriptionAr = "قيادة مركبة على الطريق العام بدون لوحات أرقام";
                TrafficFineList.Add(fine);


                fine = new TrafficeFines();
                fine.DescriptionEn = "Causing death of others";
                fine.FineAmmount = "Decided by Court";
                fine.BlackPoints = "12";
                fine.VehicleConfiscationPeriod = "30 Days";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "30 يوم";
                fine.BlackPointsAr = "12";
                fine.FineAmmountsAr = "تقررها المحكمة";
                fine.DescriptionAr = "التسبب في وفاة شخص";
                TrafficFineList.Add(fine);


                fine = new TrafficeFines();
                fine.DescriptionEn = "Not stopping after causing an accident that resulted in injuries";
                fine.FineAmmount = "Decided By Court";
                fine.BlackPoints = "24";
                fine.VehicleConfiscationPeriod = "60 Days";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "60 يوم";
                fine.BlackPointsAr = "24";
                fine.FineAmmountsAr = "تقررها المحكمة";
                fine.DescriptionAr = "عدم الوقوف عند التسبب في حادث تنتج عنه إصابات بدنية";
                TrafficFineList.Add(fine);


                fine = new TrafficeFines();
                fine.DescriptionEn = "Reckless driving";
                fine.FineAmmount = "2000";
                fine.BlackPoints = "12";
                fine.VehicleConfiscationPeriod = "30 Days";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "30 يوم";
                fine.BlackPointsAr = "12";
                fine.FineAmmountsAr = "2000";
                fine.DescriptionAr = "قيادة المركبة بتهور";
                TrafficFineList.Add(fine);


                fine = new TrafficeFines();
                fine.DescriptionEn = "Exceeding maximum speed limit by more than 60km/h";
                fine.FineAmmount = "1000";
                fine.BlackPoints = "12";
                fine.VehicleConfiscationPeriod = "30 Days";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "30 يوم";
                fine.BlackPointsAr = "12";
                fine.FineAmmountsAr = "1000";
                fine.DescriptionAr = "تجاوز الحد الأقصى للسرعة المقررة بما يزيد عن 60 كم / ساعة";
                TrafficFineList.Add(fine);


                fine = new TrafficeFines();
                fine.DescriptionEn = "Driving in a way that is dangerous to the public";
                fine.FineAmmount = "1000";
                fine.BlackPoints = "12";
                fine.VehicleConfiscationPeriod = "30 Days";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "30 يوم";
                fine.BlackPointsAr = "12";
                fine.FineAmmountsAr = "2000";
                fine.DescriptionAr = "قيادة المركبة بصورة تشكل خطرا على الجمهور";
                TrafficFineList.Add(fine);


                fine = new TrafficeFines();
                fine.DescriptionEn = "Jumping a red light";
                fine.FineAmmount = "800";
                fine.BlackPoints = "8";
                fine.VehicleConfiscationPeriod = "15 Days";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "15 يوم";
                fine.BlackPointsAr = "8";
                fine.FineAmmountsAr = "800";
                fine.DescriptionAr = "تجاوز الإشارة الضوئية الحمراء";
                TrafficFineList.Add(fine);


                fine = new TrafficeFines();
                fine.DescriptionEn = "Running away from a traffic policeman";
                fine.FineAmmount = "800";
                fine.BlackPoints = "12";
                fine.VehicleConfiscationPeriod = "30 Days";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "30 يوم";
                fine.BlackPointsAr = "12";
                fine.FineAmmountsAr = "800";
                fine.DescriptionAr = "الهروب من شرطي المرور";
                TrafficFineList.Add(fine);


                fine = new TrafficeFines();
                fine.DescriptionEn = "Dangerous overtaking by trucks";
                fine.FineAmmount = "800";
                fine.BlackPoints = "24";
                fine.VehicleConfiscationPeriod = "60 Days";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "60 يوم";
                fine.BlackPointsAr = "24";
                fine.FineAmmountsAr = "800";
                fine.DescriptionAr = "قيام سائقي الشاحنات بالتجاوز بصورة خطرة";
                TrafficFineList.Add(fine);


                fine = new TrafficeFines();
                fine.DescriptionEn = "Causing a car to overturn";
                fine.FineAmmount = "Decided By Court";
                fine.BlackPoints = "8";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "8";
                fine.FineAmmountsAr = "تقررها المحكمة";
                fine.DescriptionAr = "التسبب في وقوع حادث تدهور";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Causing serious injuries";
                fine.FineAmmount = "Decided By Court";
                fine.BlackPoints = "8";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "8";
                fine.FineAmmountsAr = "تقررها المحكمة";
                fine.DescriptionAr = "التسبب في إصابة شخص إصابات بليغة";
                TrafficFineList.Add(fine);


                fine = new TrafficeFines();
                fine.DescriptionEn = "Exceeding maximum speed limit by not more than 60km/h";
                fine.FineAmmount = "900";
                fine.BlackPoints = "6";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "6";
                fine.FineAmmountsAr = "900";
                fine.DescriptionAr = "تجاوز الحد الأقصى للسرعة المقررة بما لا يزيد عن 60 كم/ ساعة";
                TrafficFineList.Add(fine);


                fine = new TrafficeFines();
                fine.DescriptionEn = "Exceeding maximum speed limit by not more than 50km/h";
                fine.FineAmmount = "800";
                fine.BlackPoints = "6";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "6";
                fine.FineAmmountsAr = "800";
                fine.DescriptionAr = "تجاوز الحد الأقصى للسرعة المقررة بما لا يزيد عن50 كم/ ساعة";
                TrafficFineList.Add(fine);


                fine = new TrafficeFines();
                fine.DescriptionEn = "Overtaking on the hard shoulder";
                fine.FineAmmount = "600";
                fine.BlackPoints = "6";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "6";
                fine.FineAmmountsAr = "600";
                fine.DescriptionAr = "التجاوز من ناحية كتف الطريق";
                TrafficFineList.Add(fine);


                fine = new TrafficeFines();
                fine.DescriptionEn = "Entering road dangerously";
                fine.FineAmmount = "600";
                fine.BlackPoints = "6";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "6";
                fine.FineAmmountsAr = "600";
                fine.DescriptionAr = "الانتقال إلى الطريق بصورة خطرة";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Heavy vehicle lane discipline";
                fine.FineAmmount = "600";
                fine.BlackPoints = "6";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "6";
                fine.FineAmmountsAr = "600";
                fine.DescriptionAr = "عدم التزام المركبة الثقيلة بخط السير الإلزامي";
                TrafficFineList.Add(fine);


                fine = new TrafficeFines();
                fine.DescriptionEn = "Causing moderate injury";
                fine.FineAmmount = "Decided By Court";
                fine.BlackPoints = "6";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "6";
                fine.FineAmmountsAr = "تقررها المحكمة";
                fine.DescriptionAr = "التسبب في إصابة شخص إصابة متوسطة";
                TrafficFineList.Add(fine);


                fine = new TrafficeFines();
                fine.DescriptionEn = "Causing serious damage to a vehicle";
                fine.FineAmmount = "Decided By Court";
                fine.BlackPoints = "6";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "6";
                fine.FineAmmountsAr = "تقررها المحكمة";
                fine.DescriptionAr = "التسبب في إحداث أضرار بليغة في المركبة";
                TrafficFineList.Add(fine);


                fine = new TrafficeFines();
                fine.DescriptionEn = "Overtaking from a prohibited place";
                fine.FineAmmount = "2000";
                fine.BlackPoints = "6";
                fine.VehicleConfiscationPeriod = "";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "6";
                fine.FineAmmountsAr = "600";
                fine.DescriptionAr = "التجاوز في مكان ممنوع فيه التجاوز";
                TrafficFineList.Add(fine);


                fine = new TrafficeFines();
                fine.DescriptionEn = "Exceeding maximum speed limit by not more than 40km/h";
                fine.FineAmmount = "700";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "600";
                fine.DescriptionAr = "تجاوز الحد الأقصى للسرعة المقررة بما لا يزيد عن 40 كم/ ساعة";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Parking in fire hydrant places, spaces allocated for people with special needs and ambulance parking";
                fine.FineAmmount = "1000";
                fine.BlackPoints = "4";
                fine.VehicleConfiscationPeriod = "";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "4";
                fine.FineAmmountsAr = "1000";
                fine.DescriptionAr = "وقوف المركبات أمام فوهات الحريق ، والأماكن المخصصة لذوي الاحتياجات الخاصة ومركبات الإسعاف";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Exceeding maximum speed limit by not more than 30km/h";
                fine.FineAmmount = "600";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "600";
                fine.DescriptionAr = "تجاوز الحد الأقصى للسرعة المقررة بما لا يزيد عن30 كم/ ساعة";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Driving against traffic";
                fine.FineAmmount = "400";
                fine.BlackPoints = "4";
                fine.VehicleConfiscationPeriod = "";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "4";
                fine.FineAmmountsAr = "400";
                fine.DescriptionAr = "قيادة المركبة بعكس اتجاه السير";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Allowing children under 10 years old to sit in the front seat of a vehicle";
                fine.FineAmmount = "400";
                fine.BlackPoints = "4";
                fine.VehicleConfiscationPeriod = "";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "4";
                fine.FineAmmountsAr = "400";
                fine.DescriptionAr = "السماح للأطفال دون سن العاشرة بالجلوس في المقعد الأمامي في المركبة";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Failure to fasten seat belt while driving";
                fine.FineAmmount = "400";
                fine.BlackPoints = "4";
                fine.VehicleConfiscationPeriod = "";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "4";
                fine.FineAmmountsAr = "400";
                fine.DescriptionAr = "عدم ربط حزام الأمان أثناء القيادة";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Failure to leave a safe distance";
                fine.FineAmmount = "400";
                fine.BlackPoints = "4";
                fine.VehicleConfiscationPeriod = "";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "4";
                fine.FineAmmountsAr = "400";
                fine.DescriptionAr = "عدم ترك مسافة كافية خلف المركبات الأمامية";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Failure to follow the directions of a traffic policeman";
                fine.FineAmmount = "-";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "-";
                fine.DescriptionAr = "عدم إتباع إرشادات شرطي المرور";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Exceeding maximum speed limit by not more than 20km/h";
                fine.FineAmmount = "500";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "500";
                fine.DescriptionAr = "تجاوز الحد الأقصى للسرعة المقررة بما لا يزيد عن20 كم/ ساعة";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Entering a road without ensuring that it is clear";
                fine.FineAmmount = "400";
                fine.BlackPoints = "4";
                fine.VehicleConfiscationPeriod = "30 Days";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "30 يوم";
                fine.BlackPointsAr = "4";
                fine.FineAmmountsAr = "400";
                fine.DescriptionAr = "دخول الطريق دون التأكد من خلوه";
                TrafficFineList.Add(fine);


                fine = new TrafficeFines();
                fine.DescriptionEn = "Exceeding permitted level of car window tinting";
                fine.FineAmmount = "500";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "30 Days";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "30 يوم";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "500";
                fine.DescriptionAr = "زيادة نسبة تلوين زجاج مركبة عما هو مصرح به";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Stopping vehicle on the left side of the road in prohibited places";
                fine.FineAmmount = "500";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "500";
                fine.DescriptionAr = "إيقاف المركبة على الجانب الأيسر من الطريق العام في غير الأماكن المسموح فيها";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Stopping vehicle on pedestrian crossing";
                fine.FineAmmount = "500";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "500";
                fine.DescriptionAr = "إيقاف المركبة على ممر عبور المشاة";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Teaching driving in a training vehicle that does not bear a learning sign";
                fine.FineAmmount = "500";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "500";
                fine.DescriptionAr = "تعليم القيادة بمركبة تعليم لا تحمل لوحة تعليم";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Teaching driving in a non-training vehicle without permission from licensing authority";
                fine.FineAmmount = "500";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "500";
                fine.DescriptionAr = "تعليم القيادة بمركبة غير مخصصة للتعليم دون أذن من سلطة الترخيص";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Placing marks on the road that may damage the road or block traffic";
                fine.FineAmmount = "500";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "500";
                fine.DescriptionAr = "وضع علامات على الطريق تضر به ، أو تعطل حركة السير";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Operating industrial, construction and mechanical vehicles and tractors without permission from licensing authority";
                fine.FineAmmount = "500";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "7 Days";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "7 أيام";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "500";
                fine.DescriptionAr = "تسيير المركبات الصناعية والإنشائية والجرارات والأجهزة الميكانيكية دون تصريح من سلطة الترخيص";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Exceeding maximum speed limit by not more than 10km/h";
                fine.FineAmmount = "300";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "300";
                fine.DescriptionAr = "تجاوز الحد الأقصى للسرعة المقررة بما لا يزيد عن10 كم/ ساعة";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Driving with a driving license issued by a foreign country except in permitted cases";
                fine.FineAmmount = "400";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "400";
                fine.DescriptionAr = "القيادة برخصة قيادة صادرة من دولة أجنبية في غير الحالات المرخص به";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Violating the terms of the driving license";
                fine.FineAmmount = "300";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "500";
                fine.DescriptionAr = "القيادة بخلاف الرخصة الممنوحة";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Parking behind vehicles and blocking their movement";
                fine.FineAmmount = "300";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "300";
                fine.DescriptionAr = "الوقوف خلف المركبات مما يعوق تحركه";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Towing a vehicle or a boat with an unprepared vehicle";
                fine.FineAmmount = "300";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "300";
                fine.DescriptionAr = "ترك المركبة في الطريق ، ومحركها دائر";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Taxis, which have designated pickup areas, stopping in undesignated places";
                fine.FineAmmount = "200";
                fine.BlackPoints = "4";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "4";
                fine.FineAmmountsAr = "200";
                fine.DescriptionAr = "وقوف مركبات الأجرة المخصص لها مواقف لنقل الركاب في غير الأماكن المصرح بها";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Prohibited entry";
                fine.FineAmmount = "200";
                fine.BlackPoints = "4";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "4";
                fine.FineAmmountsAr = "200";
                fine.DescriptionAr = "الدخول في مكان ممنوع";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Blocking traffic";
                fine.FineAmmount = "200";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "200";
                fine.DescriptionAr = "عرقلة حركة السير";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Driving a light vehicle that does not comply with safety and security conditions";
                fine.FineAmmount = "200";
                fine.BlackPoints = "4";
                fine.VehicleConfiscationPeriod = "7 Days";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "7 أيام";
                fine.BlackPointsAr = "4";
                fine.FineAmmountsAr = "200";
                fine.DescriptionAr = "عدم التزام سائقي المركبات بعلامات وإرشادات المرور";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Heavy vehicle prohibited entry";
                fine.FineAmmount = "200";
                fine.BlackPoints = "4";
                fine.VehicleConfiscationPeriod = "7 Days";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "7 أيام";
                fine.BlackPointsAr = "4";
                fine.FineAmmountsAr = "200";
                fine.DescriptionAr = "مرور أو دخول المركبات الثقيلة في الطرق والأماكن الممنوعة";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Violating loading or unloading regulations in parking";
                fine.FineAmmount = "200";
                fine.BlackPoints = "4";
                fine.VehicleConfiscationPeriod = "7 Days";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "7 أيام";
                fine.BlackPointsAr = "4";
                fine.FineAmmountsAr = "200";
                fine.DescriptionAr = "عدم الالتزام بنظام التحميل أو التفريغ في المواقف";
                TrafficFineList.Add(fine);


                fine = new TrafficeFines();
                fine.DescriptionEn = "Carrying and transporting passengers illegally";
                fine.FineAmmount = "200";
                fine.BlackPoints = "4";
                fine.VehicleConfiscationPeriod = "7 Days";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "7 أيام";
                fine.BlackPointsAr = "4";
                fine.FineAmmountsAr = "200";
                fine.DescriptionAr = "تحميل ونقل الركاب بطريقة غير قانونية";
                TrafficFineList.Add(fine);



                fine = new TrafficeFines();
                fine.DescriptionEn = "Writing phrases or placing stickers on vehicle without permission";
                fine.FineAmmount = "200";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "200";
                fine.DescriptionAr = "كتابة عبارات أو وضع ملصقات على المركبة بدون تصريح";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Not taking road safety measures during vehicle breakdowns";
                fine.FineAmmount = "200";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "200";
                fine.DescriptionAr = "عدم اتخاذ اللازم لسلامة حركة السير عند تعطل المركبة";
                TrafficFineList.Add(fine);


                fine = new TrafficeFines();
                fine.DescriptionEn = "Turning at undesignated points";
                fine.FineAmmount = "200";
                fine.BlackPoints = "4";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "4";
                fine.FineAmmountsAr = "200";
                fine.DescriptionAr = "الدوران بالمركبة من غير المكان المخصص لذلك";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Turning the wrong way";
                fine.FineAmmount = "200";
                fine.BlackPoints = "4";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "4";
                fine.FineAmmountsAr = "200";
                fine.DescriptionAr = "الدوران بالمركبة بطريقة خاطئة";
                TrafficFineList.Add(fine);


                fine = new TrafficeFines();
                fine.DescriptionEn = "Pedestrians crossing from undesignated places (If any existed)";
                fine.FineAmmount = "200";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "500";
                fine.DescriptionAr = "عبور المشاة للطريق من غير الأماكن المخصص لعبورهم (إن وجدت)";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Opening left door of taxi";
                fine.FineAmmount = "200";
                fine.BlackPoints = "3";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "3";
                fine.FineAmmountsAr = "200";
                fine.DescriptionAr = "فتح الباب الأيسر لمركبة الأجرة";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Driving without spectacles or contact lenses";
                fine.FineAmmount = "100";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "100";
                fine.DescriptionAr = "القيادة بدون استعمال النظارة الطبية، أو العدسات";
                TrafficFineList.Add(fine);


                fine = new TrafficeFines();
                fine.DescriptionEn = "Using horn in a disturbing way";
                fine.FineAmmount = "100";
                fine.BlackPoints = "2";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "2";
                fine.FineAmmountsAr = "100";
                fine.DescriptionAr = "استعمال آلة التنبيه بصورة مزعجة";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Not using interior light in buses at night";
                fine.FineAmmount = "100";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "100";
                fine.DescriptionAr = "عدم استخدام الإضاءة الداخلية في الحافلات ليلا";
                TrafficFineList.Add(fine);


                fine = new TrafficeFines();
                fine.DescriptionEn = "Not carrying driving license while driving";
                fine.FineAmmount = "100";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "100";
                fine.DescriptionAr = "جمع الركاب بالمناداة مع وجود لافتات";
                TrafficFineList.Add(fine);




                fine = new TrafficeFines();
                fine.DescriptionEn = "Not giving way to emergency, police and public service vehicles or official convoys";
                fine.FineAmmount = "500";
                fine.BlackPoints = "4";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "4";
                fine.FineAmmountsAr = "500";
                fine.DescriptionAr = "عدم إعطاء أفضلية الطريق لمركبات الطوارئ، والشرطة، والخدمة العامة، والمواكب الرسمية";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Failure to stop after causing an accident";
                fine.FineAmmount = "500";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "500";
                fine.DescriptionAr = "السماح للغير بقيادة مركبه غير مرخصة";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Failure to abide by traffic signs and directions";
                fine.FineAmmount = "500";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "500";
                fine.DescriptionAr = "عدم التزام سائقي المركبات بعلامات وإرشادات المرور";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Allowing others to drive a vehicle for which they are unlicensed";
                fine.FineAmmount = "500";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "500";
                fine.DescriptionAr = "عدم إعطاء أفضلية الطريق لمركبات الطوارئ، والشرطة، والخدمة العامة، والمواكب الرسمية";
                TrafficFineList.Add(fine);


                fine = new TrafficeFines();
                fine.DescriptionEn = "Stopping on the road for no reason";
                fine.FineAmmount = "500";
                fine.BlackPoints = "4";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "4";
                fine.FineAmmountsAr = "500";
                fine.DescriptionAr = "الوقوف وسط الطريق دون مبرر";
                TrafficFineList.Add(fine);


                fine = new TrafficeFines();
                fine.DescriptionEn = "Stopping on a yellow box";
                fine.FineAmmount = "500";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "500";
                fine.DescriptionAr = "الوقوف في الصندوق الأصفر";
                TrafficFineList.Add(fine);





                fine = new TrafficeFines();
                fine.DescriptionEn = "Not giving pedestrians way on pedestrian crossings";
                fine.FineAmmount = "500";
                fine.BlackPoints = "6";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "6";
                fine.FineAmmountsAr = "500";
                fine.DescriptionAr = "عدم إعطاء الأولوية للمشاة في الأماكن المخصصة لعبورهم)  ";
                TrafficFineList.Add(fine);


                fine = new TrafficeFines();
                fine.DescriptionEn = "Failure to abide by traffic signs and directions";
                fine.FineAmmount = "500";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "500";
                fine.DescriptionAr = "عدم التزام سائقي المركبات بعلامات وإرشادات المرور";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Throwing waste from vehicles onto roads";
                fine.FineAmmount = "500";
                fine.BlackPoints = "4";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "4";
                fine.FineAmmountsAr = "500";
                fine.DescriptionAr = "إلقاء المخلفات من المركبات بالطريق العام";
                TrafficFineList.Add(fine);


                ////////////////////////////////////////////////////////////////


                fine = new TrafficeFines();
                fine.DescriptionEn = "Not abiding by taxi drivers obligatory uniform or not keeping it in good condition";
                fine.FineAmmount = "100";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "100";
                fine.DescriptionAr = "عدم ارتداء الزي المقرر لسائقي مركبات الأجرة ، أو عدم الاعتناء به";
                TrafficFineList.Add(fine);


                fine = new TrafficeFines();
                fine.DescriptionEn = "Using hand-held mobile phone while driving";
                fine.FineAmmount = "200";
                fine.BlackPoints = "4";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "4";
                fine.FineAmmountsAr = "200";
                fine.DescriptionAr = "استخدام الهاتف النقال بواسطة اليد أثناء القيادةا";
                TrafficFineList.Add(fine);


                fine = new TrafficeFines();
                fine.DescriptionEn = "Smoking inside taxis and buses";
                fine.FineAmmount = "200";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "200";
                fine.DescriptionAr = "التدخين داخل مركبات الأجرة والحافلات";
                TrafficFineList.Add(fine);


                fine = new TrafficeFines();
                fine.DescriptionEn = "Failure to keep taxis and buses clean inside and outside";
                fine.FineAmmount = "200";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "200";
                fine.DescriptionAr = "عدم نظافة مركبات الأجرة والحافلات من الداخل أو الخارج";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Not taking road safety measures during vehicle breakdowns";
                fine.FineAmmount = "200";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "200";
                fine.DescriptionAr = "عدم اتخاذ اللازم لسلامة حركة السير عند تعطل المركبة";
                TrafficFineList.Add(fine);


                fine = new TrafficeFines();
                fine.DescriptionEn = "Driving below minimum speed limit";
                fine.FineAmmount = "200";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "200";
                fine.DescriptionAr = "عدم استخدام الإضاءة الداخلية في الحافلات ليلا";
                TrafficFineList.Add(fine);

                ////////////////////////////

                fine = new TrafficeFines();
                fine.DescriptionEn = "Using horn in prohibited areas";
                fine.FineAmmount = "200";
                fine.BlackPoints = "2";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "2";
                fine.FineAmmountsAr = "200";
                fine.DescriptionAr = "استعمال آلة التنبيه في أماكن محظو رة";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Failure to display tariff of buses or taxis or not showing them when required";
                fine.FineAmmount = "200";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "200";
                fine.DescriptionAr = "عدم وضع تعريفة الأجور المقررة في الحافلات العمومية ومركبات الأجرة ، أو عدم إبرازها عند الطلب";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Failure to abide by specified color for taxis or training cars";
                fine.FineAmmount = "200";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "200";
                fine.DescriptionAr = "عدم الالتزام باللون المقرر لمركبات الأجرة أو التدريبا";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Using interior lights for no reason while driving";
                fine.FineAmmount = "100";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "100";
                fine.DescriptionAr = "استعمال الإنارة الداخلية أثناء سير المركبة بدون مبرر";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Not fixing taxi sign where required";
                fine.FineAmmount = "200";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "200";
                fine.DescriptionAr = "عدم تثبيت علامة الأجرة في الأماكن المخصصة لها بالمركبة";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Not showing driving license when required";
                fine.FineAmmount = "200";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "200";
                fine.DescriptionAr = "عدم إبراز ملكية المركبة عند الطلبا";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Not showing vehicle registration card when required";
                fine.FineAmmount = "200";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "200";
                fine.DescriptionAr = "عدم إبراز رخصة القيادة عند الطلبا";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Parking vehicles on pavement";
                fine.FineAmmount = "200";
                fine.BlackPoints = "3";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "3";
                fine.FineAmmountsAr = "200";
                fine.DescriptionAr = "إيقاف المركبات على الأرصفة";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Light vehicle lane discipline";
                fine.FineAmmount = "200";
                fine.BlackPoints = "2";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "2";
                fine.FineAmmountsAr = "200";
                fine.DescriptionAr = "عدم التزام المركبة الخفيفة بخط السير الإلزامي";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Violating tariff";
                fine.FineAmmount = "200";
                fine.BlackPoints = "6";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "3";
                fine.FineAmmountsAr = "200";
                fine.DescriptionAr = "عدم التقيد بالتعرفة المقررة";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Not wearing helmet while driving motorbike";
                fine.FineAmmount = "200";
                fine.BlackPoints = "4";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "4";
                fine.FineAmmountsAr = "200";
                fine.DescriptionAr = "عدم ارتداء الخوذة أثناء قيادة الدراجة";
                TrafficFineList.Add(fine);




                fine = new TrafficeFines();
                fine.DescriptionEn = "Parking on road shoulder except in cases of emergency";
                fine.FineAmmount = "200";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "200";
                fine.DescriptionAr = "الوقوف على كتف الطريق العام في غير الحالات الطارئة";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Driving a taxi with an expired warranty";
                fine.FineAmmount = "200";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "200";
                fine.DescriptionAr = "قيادة مركبة أجرة منتهية الكفالة";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Driving unlicensed vehicle";
                fine.FineAmmount = "200";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "7 Days";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "7 أيام";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "200";
                fine.DescriptionAr = "قيادة مركبة غير مرخصة من سلطة الترخيصا";
                TrafficFineList.Add(fine);
                ///////////////////////////



                fine = new TrafficeFines();
                fine.DescriptionEn = "Driving with one number plate";
                fine.FineAmmount = "200";
                fine.BlackPoints = "2";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "2";
                fine.FineAmmountsAr = "200";
                fine.DescriptionAr = "القيادة بلوحة واحدة";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Taxi refusing to carry passengers";
                fine.FineAmmount = "200";
                fine.BlackPoints = "4";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "4";
                fine.FineAmmountsAr = "200";
                fine.DescriptionAr = "رفض نقل ركاب بمركبة الأجرة";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Driving at night or in foggy weather without lights";
                fine.FineAmmount = "200";
                fine.BlackPoints = "4";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "4";
                fine.FineAmmountsAr = "200";
                fine.DescriptionAr = "السير ليلا أو في أوقات الضباب دون استعمال الأنوار";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Transporting passengers by vehicle undesignated for this purpose";
                fine.FineAmmount = "400";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "400";
                fine.DescriptionAr = "نقل الركاب في مركبة غير مخصصه لنقلهم";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Driving a taxi with an expired warranty";
                fine.FineAmmount = "200";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "200";
                fine.DescriptionAr = "قيادة مركبة أجرة منتهية الكفالة";
                TrafficFineList.Add(fine);

                fine = new TrafficeFines();
                fine.DescriptionEn = "Parking in prohibited places";
                fine.FineAmmount = "200";
                fine.BlackPoints = "2";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "2";
                fine.FineAmmountsAr = "200";
                fine.DescriptionAr = "الوقوف في مكان ممنوع";
                TrafficFineList.Add(fine);









                //////////////////////////////////////////
                fine = new TrafficeFines();
                fine.DescriptionEn = "Failure to abide by traffic signs and directions";
                fine.FineAmmount = "500";
                fine.BlackPoints = "-";
                fine.VehicleConfiscationPeriod = "-";
                fine.Type = "Traffic";
                fine.TypeAr = "مرورية";
                fine.VehicleConfiscationPeriodAr = "-";
                fine.BlackPointsAr = "-";
                fine.FineAmmountsAr = "500";
                fine.DescriptionAr = "عدم التزام سائقي المركبات بعلامات وإرشادات المرور";
                TrafficFineList.Add(fine);
                #endregion



                /////////////////////////////////////////////////////////////////
                grdTrafficFines.ItemsSource = null;
                grdTrafficFines.ItemsSource = TrafficFineList;
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void UserControl_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {
            if (new CommonUtils().GetScreenOrientation() == AppProperties.LandScapetMode)
            {
               // this.grdTrafficFines.Width = 750;
               // this.grdTrafficFines.Height = 350;
            }
            else
            {


               // this.grdTrafficFines.Width = 500;
               // this.grdTrafficFines.Height = 750;
            }
        }

        private void txtFilterDescription_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            try
            {

                var TrafficFineListTemp = TrafficFineList.Where(p => p.DescriptionEn.ToLower().Contains(txtDespFilter.Text.ToLower()));
                grdTrafficFines.ItemsSource = TrafficFineListTemp;
                grdTrafficFines.UpdateLayout();

           }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void txtType_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            try
            {
                
                var TrafficFineListTemp = TrafficFineList.Where(p => p.Type.ToLower().Contains(txtTypeEn.Text.ToLower()));
                grdTrafficFines.ItemsSource = TrafficFineListTemp;
                grdTrafficFines.UpdateLayout();

            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void txtTypeAr_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            try
            {

                var TrafficFineListTemp = TrafficFineList.Where(p => p.TypeAr.Contains(txtTypeAr.Text));
                grdTrafficFines.ItemsSource = TrafficFineListTemp;
                grdTrafficFines.UpdateLayout();

            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void txtDespFilterAr_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            try
            {

                var TrafficFineListTemp = TrafficFineList.Where(p => p.DescriptionAr.Contains(txtDespFilterAr.Text));
                grdTrafficFines.ItemsSource = TrafficFineListTemp;
                grdTrafficFines.UpdateLayout();

            }
            catch (Exception ex)
            {
                App.VSDLog.Info(ex.StackTrace);
                WPFMessageBox.Show(new CommonUtils().GetStringValue("Exception"), ex.Message, ex.StackTrace, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
            }
        }

        private void txtDespFilter_GotFocus_1(object sender, RoutedEventArgs e)
        {
            CommonUtils.ShowKeyBoard();
        }

        private void txtDespFilter_LostFocus_1(object sender, RoutedEventArgs e)
        {
            CommonUtils.CLoseKeyBoard();
        }

        private void txtDespFilter_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {

        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (AppProperties.Selected_Resource == "English")
            {
                imagebtnback.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/Back.png", UriKind.Relative));
            }
            else
            {
                imagebtnback.Source = new BitmapImage(new Uri(@"/Images/Buttons/Small/back Arabic Up.png", UriKind.Relative));
            }

            //this.Close();
            m_MainWindow.MainContentControl.Content = null;
            m_MainWindow.MainContentControl.Content = new ucWellComeScreen(m_MainWindow);
        }
    }
}
