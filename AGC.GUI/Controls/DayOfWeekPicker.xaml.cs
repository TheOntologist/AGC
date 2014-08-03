using System;
using System.Collections.Generic;
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

namespace AGC.GUI.Controls
{
    /// <summary>
    /// Interaction logic for DayOfWeekPicker.xaml
    /// </summary>
    public partial class DayOfWeekPicker : UserControl
    {
        private const string MO = "MO";
        private const string TU = "TU";
        private const string WE = "WE";
        private const string TH = "TH";
        private const string FR = "FR";
        private const string SA = "SA";
        private const string SU = "SU";

        private static List<string> DAYS_OF_WEEK_REVERSE_ORDER = new List<string>(new string[] { SU, SA, FR, TH, WE, TU, MO });

        public DayOfWeekPicker()
        {
            InitializeComponent();
            LoadData();
        }

        public string DayOfWeek
        {
            get { return (string)GetValue(DayOfWeekProperty); }
            set { SetValue(DayOfWeekProperty, value); }
        }

        public static readonly DependencyProperty DayOfWeekProperty =
            DependencyProperty.Register("DayOfWeek", typeof(string), typeof(DayOfWeekPicker), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnCurrentDayOfWeekPropertyChanged));

        private static void OnCurrentDayOfWeekPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            DayOfWeekPicker control = source as DayOfWeekPicker;
            string dayOfWeek = (string)e.NewValue;

            control.DayOfWeekList.SelectedItem = dayOfWeek;
        }

        private void LoadData()
        {
            DayOfWeekList.ItemsSource = DAYS_OF_WEEK_REVERSE_ORDER;
        }

        private void DayOfWeekList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DayOfWeek = DayOfWeekList.SelectedItem.ToString();
        }

        private bool nextTimeSwitchToMinDayOfWeek = false;
        private bool nextTimeSwitchToMaxDayOfWeek = false;

        private void DayOfWeekList_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                if (nextTimeSwitchToMaxDayOfWeek)
                {
                    DayOfWeekList.SelectedIndex = 0;
                    nextTimeSwitchToMaxDayOfWeek = false;
                }
                else
                {
                    if (DayOfWeekList.SelectedIndex == DayOfWeekList.Items.Count - 1)
                        nextTimeSwitchToMaxDayOfWeek = true;
                }
            }

            if (e.Key == Key.Up)
            {
                if (nextTimeSwitchToMinDayOfWeek)
                {
                    DayOfWeekList.SelectedIndex = DayOfWeekList.Items.Count - 1;
                    nextTimeSwitchToMinDayOfWeek = false;
                }
                else
                {
                    if (DayOfWeekList.SelectedIndex == 0)
                        nextTimeSwitchToMinDayOfWeek = true;
                }
            }
        }
    }
}
