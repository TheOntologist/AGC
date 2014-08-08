using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
    /// Interaction logic for DatePicker.xaml
    /// </summary>
    public partial class DatePicker : UserControl
    {
        private const string DAY_OF_WEEK_FORMAT = "{0:ddd}";

        public DatePicker()
        {
            InitializeComponent();
            LoadData();
        }

        public DateTime Date
        {
            get { return (DateTime)GetValue(DateProperty); }
            set { SetValue(DateProperty, value); }
        }

        public bool Focused
        {
            get { return (bool)GetValue(FocusedProperty); }
            set { SetValue(FocusedProperty, value); }
        }

        public static readonly DependencyProperty DateProperty =
            DependencyProperty.Register("Date", typeof(DateTime), typeof(DatePicker), new FrameworkPropertyMetadata(DateTime.Now, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnCurrentDatePropertyChanged));

        public static readonly DependencyProperty FocusedProperty =
            DependencyProperty.Register("Focused", typeof(bool), typeof(DatePicker), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnCurrentFocusedPropertyChanged));

        private static void OnCurrentDatePropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            DatePicker control = source as DatePicker;
            DateTime date = (DateTime)e.NewValue;

            control.YearsList.SelectedItem = date.Year;
            control.MonthsList.SelectedIndex = GetMonthIntValue(date.Month - 1);
            control.DaysList.SelectedItem = date.Day;
            control.DayOfWeek.Text = String.Format(DAY_OF_WEEK_FORMAT, date);
        }

        private static void OnCurrentFocusedPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            DatePicker control = source as DatePicker;
            bool focused = (bool)e.NewValue;

            if(focused)
            {
                control.YearsList.Focus();
            }
        }

        private void LoadData()
        {
            YearsList.ItemsSource = GetYearsList();
            MonthsList.ItemsSource = GetMonthNames();
            DaysList.ItemsSource = GetDefaultDaysList();
        }

        private static List<int> GetYearsList()
        {
            List<int> years = Enumerable.Range(DateTime.Now.Year - 4, 8).ToList();
            years.Reverse();
            return years;
        }

        private static string[] GetMonthNames()
        {
            string[] monthNames = DateTimeFormatInfo.CurrentInfo.AbbreviatedMonthNames;
            Array.Resize(ref monthNames, 12);
            Array.Reverse(monthNames);
            return monthNames;
        }

        private static List<int> GetDefaultDaysList()
        {
            List<int> days = Enumerable.Range(1, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month)).ToList();
            days.Reverse();
            return days;
        }

        private void YearsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Date = new DateTime((int)YearsList.SelectedItem, Date.Month, Date.Day);      
        }

        private void MonthsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<int> days = Enumerable.Range(1, DateTime.DaysInMonth(Date.Year, GetMonthIntValue(MonthsList.SelectedIndex) + 1)).ToList();
            days.Reverse();
            if (Date.Day > days.Count)
            {
                Date = new DateTime(Date.Year, Date.Month, days.Count);
            }
            else
            {
                Date = new DateTime(Date.Year, GetMonthIntValue(MonthsList.SelectedIndex) + 1, Date.Day);
            }
            DaysList.ItemsSource = days;
        }

        private void DaysList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Date = new DateTime(Date.Year, Date.Month, (int)DaysList.SelectedItem);
        }

        private static int GetMonthIntValue(int i)
        {
            return 11 - i;
        }

        private bool nextTimeSwitchToMinYear = false;
        private bool nextTimeSwitchToMaxYear = false;

        private void YearsList_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                if (nextTimeSwitchToMaxYear)
                {
                    YearsList.SelectedIndex = 0;
                    nextTimeSwitchToMaxYear = false;
                }
                else
                {
                    if (YearsList.SelectedIndex == YearsList.Items.Count - 1)
                        nextTimeSwitchToMaxYear = true;
                }
            }

            if (e.Key == Key.Up)
            {
                if (nextTimeSwitchToMinYear)
                {
                    YearsList.SelectedIndex = YearsList.Items.Count - 1;
                    nextTimeSwitchToMinYear = false;
                }
                else
                {
                    if (YearsList.SelectedIndex == 0)
                        nextTimeSwitchToMinYear = true;
                }
            }
        }

        private bool nextTimeSwitchToMinMonth = false;
        private bool nextTimeSwitchToMaxMonth = false;

        private void MonthsList_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                if (nextTimeSwitchToMaxMonth)
                {
                    MonthsList.SelectedIndex = 0;
                    nextTimeSwitchToMaxMonth = false;
                }
                else
                {
                    if (MonthsList.SelectedIndex == MonthsList.Items.Count - 1)
                        nextTimeSwitchToMaxMonth = true;
                }
            }

            if (e.Key == Key.Up)
            {
                if (nextTimeSwitchToMinMonth)
                {
                    MonthsList.SelectedIndex = MonthsList.Items.Count - 1;
                    nextTimeSwitchToMinMonth = false;
                }
                else
                {
                    if (MonthsList.SelectedIndex == 0)
                        nextTimeSwitchToMinMonth = true;
                }
            }

            nextTimeSwitchToMinDay = false;
            nextTimeSwitchToMaxDay = false;
        }

        private bool nextTimeSwitchToMinDay = false;
        private bool nextTimeSwitchToMaxDay = false;

        private void DaysList_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                if (nextTimeSwitchToMaxDay)
                {
                    DaysList.SelectedIndex = 0;
                    nextTimeSwitchToMaxDay = false;
                }
                else
                {
                    if (DaysList.SelectedIndex == DaysList.Items.Count - 1)
                        nextTimeSwitchToMaxDay = true;
                }
            }

            if (e.Key == Key.Up)
            {
                if (nextTimeSwitchToMinDay)
                {
                    DaysList.SelectedIndex = DaysList.Items.Count - 1;
                    nextTimeSwitchToMinDay = false;
                }
                else
                {
                    if (DaysList.SelectedIndex == 0)
                        nextTimeSwitchToMinDay = true;
                }
            }
        }
    }
}
