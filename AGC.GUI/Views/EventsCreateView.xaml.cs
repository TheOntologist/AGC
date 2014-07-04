using System;
using System.Windows.Controls;

namespace AGC.GUI.Views
{
    /// <summary>
    /// Description for EventsCreateView.
    /// </summary>
    public partial class EventsCreateView : UserControl
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Initializes a new instance of the EventsCreateView class.
        /// </summary>
        public EventsCreateView()
        {
            try
            {
                log.Debug("Loading EventsCreate view...");
                InitializeComponent();
                log.Debug("EventsCreate view was succssfully loaded");
            }
            catch(Exception ex)
            {
                log.Error("Failed to load EventsCreate view:", ex);
            }
        }
    }
}