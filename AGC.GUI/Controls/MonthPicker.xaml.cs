using System;
using System.Collections.Generic;
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
    /// Interaction logic for MonthPicker.xaml
    /// </summary>
    public partial class MonthPicker : UserControl
    {
        public MonthPicker()
        {
            InitializeComponent();
            LoadData();
        }

        public int Month
        {
            get { return (int)GetValue(DateProperty); }
            set { SetValue(DateProperty, value); }
        }

        public static readonly DependencyProperty DateProperty =
            DependencyProperty.Register("Month", typeof(int), typeof(MonthPicker), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnCurrentMonthPropertyChanged));

        private static void OnCurrentMonthPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            MonthPicker control = source as MonthPicker;
            int month = (int)e.NewValue;

            control.MonthsList.SelectedIndex = GetMonthIntValue(month - 1);
        }

        private void LoadData()
        {
            MonthsList.ItemsSource = GetMonthNames();
        }

        private static string[] GetMonthNames()
        {
            string[] monthNames = DateTimeFormatInfo.CurrentInfo.AbbreviatedMonthNames;
            Array.Resize(ref monthNames, 12);
            Array.Reverse(monthNames);
            return monthNames;
        }

        private void MonthsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
                Month = GetMonthIntValue(MonthsList.SelectedIndex) + 1;
        }

        private static int GetMonthIntValue(int i)
        {
            return 11 - i;
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
        }
    }
}
