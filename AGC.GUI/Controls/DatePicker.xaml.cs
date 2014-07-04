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

        // Using a DependencyProperty as the backing store for Date.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DateProperty =
            DependencyProperty.Register("Date", typeof(DateTime), typeof(DatePicker), new FrameworkPropertyMetadata(DateTime.Now, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)); //OnCurrentDatePropertyChanged, OnCoerceDateProperty FrameworkPropertyMetadataOptions.BindsTwoWayByDefault

        /*
        public event PropertyChangedEventHandler PropertyChanged;
        void SetValueDp(DependencyProperty property, object value,
            [System.Runtime.CompilerServices.CallerMemberName] String p = null)
        {
            SetValue(property, value);
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(p));
        }
        */

        private static void OnCurrentDatePropertyChanged(DependencyObject source,
        DependencyPropertyChangedEventArgs e)
        {
            DatePicker control = source as DatePicker;
            DateTime date = (DateTime)e.NewValue;
            // Put some update logic here...
            
        }

        private static object OnCoerceDateProperty(DependencyObject sender, object data)
        {

            data = DateTime.Now;

            return data;
        }

        private void LoadData()
        {
            YearsList.ItemsSource = GetYearsList();
            MonthsList.ItemsSource = GetMonthNames();
            DaysList.ItemsSource = GetDefaultDaysList();

            YearsList.SelectedItem = Date.Year;
            MonthsList.SelectedIndex = Date.Month - 1;
            DaysList.SelectedItem = Date.Day;
            SetDayOfWeek(Date);
        }

        private static List<int> GetYearsList()
        {
            List<int> years = Enumerable.Range(DateTime.Now.Year - 10, 20).ToList();
            years.Reverse();
            return years;
        }

        private static string[] GetMonthNames()
        {
            string[] monthNames = DateTimeFormatInfo.CurrentInfo.AbbreviatedMonthNames;
            Array.Resize(ref monthNames, 12);
            return monthNames;
        }

        private static List<int> GetDefaultDaysList()
        {
            List<int> days = Enumerable.Range(1, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month)).ToList();
            return days;
        }

        private void SetDayOfWeek(DateTime date)
        {
            DayOfWeek.Text = String.Format(DAY_OF_WEEK_FORMAT, date);
        }

        private void YearsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Date = new DateTime((int)YearsList.SelectedItem, Date.Month, Date.Day);
            SetDayOfWeek(Date);
        }

        private void MonthsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Date = new DateTime(Date.Year, MonthsList.SelectedIndex + 1, Date.Day);
            SetDayOfWeek(Date);
        }

        private void DaysList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Date = new DateTime(Date.Year, Date.Month, (int)DaysList.SelectedItem);
            SetDayOfWeek(Date);
        }
    }
}
