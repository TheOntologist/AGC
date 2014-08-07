using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AGC.GUI.Views
{
    /// <summary>
    /// Description for SortingAndFilteringView.
    /// </summary>
    public partial class SortingAndFilteringView : Window
    {
        /// <summary>
        /// Initializes a new instance of the SortingAndFilteringView class.
        /// </summary>
        public SortingAndFilteringView()
        {
            InitializeComponent();
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