using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

        private void Weekdays_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {

            Action<FocusNavigationDirection> moveFocus = focusDirection =>
            {
                e.Handled = true;
                var request = new TraversalRequest(focusDirection);

                var focusedElement = Keyboard.FocusedElement as CheckBox;
                if (((string)focusedElement.Content == "SU" && request.FocusNavigationDirection == FocusNavigationDirection.Next))
                {
                    Monday.Focus();
                }
                else if (((string)focusedElement.Content == "MO" && request.FocusNavigationDirection == FocusNavigationDirection.Previous))
                {
                    Sunday.Focus();
                }
                else
                {
                    focusedElement.MoveFocus(request);
                }                
            };

            if (e.Key == Key.Down)
            {
                moveFocus(FocusNavigationDirection.Previous);
            }             
            else if (e.Key == Key.Up)
            {
                moveFocus(FocusNavigationDirection.Next);
            }           
            if (e.Key == Key.Tab)
            {
                Sunday.Focus();
            }              
        }

    }
}