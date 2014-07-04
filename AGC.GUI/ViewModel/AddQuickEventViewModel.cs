using AGC.Library;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Windows;

namespace AGC.GUI.ViewModel
{

    public class AddQuickEventViewModel : ViewModelBase
    {
        #region Constants

        private const string QUICK_EVENT_TEXT_EXAMPLE = "Example: Dinner with Michael 7 p.m. tomorrow";

        #endregion

        #region Private Properties

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IGoogleCalendar calendar;

        #endregion

        #region Commands

        public RelayCommand AddQuickEventCommand { get; private set; }

        #endregion

        #region Constructor

        public AddQuickEventViewModel(IGoogleCalendar googleCalendar)
        {
            try
            {
                log.Debug("Loading AddQuickEvent view model...");

                calendar = googleCalendar;

                AddQuickEventCommand = new RelayCommand(AddQuickEvent);

                log.Debug("AddQuickEvent view model was succssfully loaded");
            }
            catch (Exception ex)
            {
                log.Error("Failed to load AddQuickEvent view model:", ex);
            }
        }

        #endregion

        #region Public Properties

        public const string QuickEventTextPropertyName = "QuickEventText";
        private string _quickEventText = QUICK_EVENT_TEXT_EXAMPLE;
        public string QuickEventText
        {
            get
            {
                return _quickEventText;
            }

            set
            {
                if (_quickEventText == value)
                {
                    return;
                }

                RaisePropertyChanging(QuickEventTextPropertyName);
                _quickEventText = value;
                RaisePropertyChanged(QuickEventTextPropertyName);
            }
        }

        #endregion

        #region Private Methods

        private void AddQuickEvent()
        {
            if (calendar.AddQuickEvent(QuickEventText))
            {
                MessageBox.Show(Application.Current.MainWindow, "Added", "Information", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
            }
            else
            {
                MessageBox.Show(Application.Current.MainWindow, "Failed to add quick event. Please check log file for a detailed information about the error.", "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
        }

        #endregion
    }
}