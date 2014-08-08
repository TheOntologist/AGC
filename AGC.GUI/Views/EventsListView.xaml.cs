using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace AGC.GUI.Views
{
    /// <summary>
    /// Description for EventsListView.
    /// </summary>
    public partial class EventsListView : UserControl
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Initializes a new instance of the EventsListView class.
        /// </summary>
        public EventsListView()
        {
            try
            {
                log.Debug("Loading EventsList view...");
                InitializeComponent();
                this.PreviewKeyDown += new KeyEventHandler(Escape_PreviewKeyDown);
                // These controls were loaded to be focusable in future
                ChooseDate.Visibility = System.Windows.Visibility.Hidden;
                log.Debug("EventsList view was succssfully loaded");
            }
            catch(Exception ex)
            {
                log.Error("Failed to load EventsList view:", ex);
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