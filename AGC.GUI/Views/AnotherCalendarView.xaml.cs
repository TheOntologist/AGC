using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace AGC.GUI.Views
{
    /// <summary>
    /// Description for AnotherCalendarView.
    /// </summary>
    public partial class AnotherCalendarView : UserControl
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Initializes a new instance of the AnotherCalendarView class.
        /// </summary>
        public AnotherCalendarView()
        {
            try
            {
                log.Debug("Loading AnotherCalendar view...");
                InitializeComponent();
                this.PreviewKeyDown += new KeyEventHandler(Escape_PreviewKeyDown);
                // These controls were loaded to be focusable in future
                ChooseDate.Visibility = System.Windows.Visibility.Hidden;
                log.Debug("AnotherCalendar view was succssfully loaded");
            }
            catch(Exception ex)
            {
                log.Error("Failed to load AnotherCalendar view:", ex);
            }           
        }

        void Escape_PreviewKeyDown(object sender, KeyEventArgs e)
        {
                if (e.Key == Key.Escape)
            {
                ChooseDate.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        public void Refresh()
        { }

        private void ShowChooseDate(object sender, System.Windows.RoutedEventArgs e)
        {
            ChooseDate.Visibility = System.Windows.Visibility.Visible;
        }

        private void HideChooseDate(object sender, System.Windows.RoutedEventArgs e)
        {
            ChooseDate.Visibility = System.Windows.Visibility.Hidden;
        }
    }
}