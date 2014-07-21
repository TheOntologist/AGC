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
    /// Interaction logic for TimePicker.xaml
    /// </summary>
    public partial class TimePicker : UserControl
    {
        public TimePicker()
        {
            InitializeComponent();
            LoadData();
        }

        public int Hours
        {
            get { return (int)GetValue(HoursProperty); }
            set { SetValue(HoursProperty, value); }
        }

        public int Minutes
        {
            get { return (int)GetValue(MinutesProperty); }
            set { SetValue(MinutesProperty, value); }
        }

        public static readonly DependencyProperty HoursProperty =
            DependencyProperty.Register("Hours", typeof(int), typeof(TimePicker), new FrameworkPropertyMetadata(-1, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnCurrentHoursPropertyChanged));

        public static readonly DependencyProperty MinutesProperty =
            DependencyProperty.Register("Minutes", typeof(int), typeof(TimePicker), new FrameworkPropertyMetadata(-1, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnCurrentMinutesPropertyChanged));

        private static void OnCurrentHoursPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            TimePicker control = source as TimePicker;
            int hours = (int)e.NewValue;
            control.HoursList.SelectedItem = hours;
        }

        private static void OnCurrentMinutesPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            TimePicker control = source as TimePicker;
            int minutes = (int)e.NewValue;
            control.MinutesList.SelectedItem = minutes;
        }

        private void LoadData()
        {
            HoursList.ItemsSource = GetHoursList();
            MinutesList.ItemsSource = GetMinutesList();
        }

        private static List<int> GetHoursList()
        {
            List<int> hours = Enumerable.Range(0, 24).ToList();
            hours.Reverse();
            return hours;
        }

        private static List<int> GetMinutesList()
        {
            List<int> minutes = Enumerable.Range(0, 60).ToList();
            minutes.Reverse();
            return minutes;
        }

        private void HoursList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Hours = (int)HoursList.SelectedItem;
        }

        private void MinutesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Minutes = (int)MinutesList.SelectedItem;
        }

        private bool nextTimeSwitchToMinHour = false;
        private bool nextTimeSwitchToMaxHour = false;

        private void HoursList_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                if (nextTimeSwitchToMaxHour)
                {
                    HoursList.SelectedIndex = 0;
                    nextTimeSwitchToMaxHour = false;
                }
                else
                {
                    if (HoursList.SelectedIndex == HoursList.Items.Count - 1)
                        nextTimeSwitchToMaxHour = true;
                }
            }

            if (e.Key == Key.Up)
            {
                if (nextTimeSwitchToMinHour)
                {
                    HoursList.SelectedIndex = HoursList.Items.Count - 1;
                    nextTimeSwitchToMinHour = false;
                }
                else
                {
                    if (HoursList.SelectedIndex == 0)
                        nextTimeSwitchToMinHour = true;
                }
            }
        }

        private bool nextTimeSwitchToMinMinute = false;
        private bool nextTimeSwitchToMaxMinute = false;

        private void MinutesList_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                if (nextTimeSwitchToMaxMinute)
                {
                    MinutesList.SelectedIndex = 0;
                    nextTimeSwitchToMaxMinute = false;
                }
                else
                {
                    if (MinutesList.SelectedIndex == MinutesList.Items.Count - 1)
                        nextTimeSwitchToMaxMinute = true;
                }
            }

            if (e.Key == Key.Up)
            {
                if (nextTimeSwitchToMinMinute)
                {
                    MinutesList.SelectedIndex = MinutesList.Items.Count - 1;
                    nextTimeSwitchToMinMinute = false;
                }
                else
                {
                    if (MinutesList.SelectedIndex == 0)
                        nextTimeSwitchToMinMinute = true;
                }
            }
        }
    }
}
