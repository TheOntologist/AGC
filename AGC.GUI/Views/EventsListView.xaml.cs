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
                log.Debug("EventsList view was succssfully loaded");
            }
            catch(Exception ex)
            {
                log.Error("Failed to load EventsList view:", ex);
            }           
        }

        public void Refresh()
        { }
    }
}