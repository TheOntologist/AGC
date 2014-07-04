using System;
using System.Windows.Controls;

namespace AGC.GUI.Views
{
    /// <summary>
    /// Description for AddQuickEventView.
    /// </summary>
    public partial class AddQuickEventView : UserControl
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Initializes a new instance of the AddQuickEventView class.
        /// </summary>
        public AddQuickEventView()
        {
            try
            {
                log.Debug("Loading AddQuickEvent view...");
                InitializeComponent();
                log.Debug("AddQuickEvent view was succssfully loaded");
            }
            catch (Exception ex)
            {
                log.Error("Failed to load AddQuickEvent view:", ex);
            }
        }
    }
}