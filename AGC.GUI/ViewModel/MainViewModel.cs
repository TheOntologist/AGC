using GalaSoft.MvvmLight;
using AGC.Library;
using System;
using GalaSoft.MvvmLight.Command;

namespace AGC.GUI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region Private Properties

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static GoogleCalendar calendar;

        #endregion

        public RelayCommand SelectEventsTabCommand { get; private set; }

        #region Constructor

        public MainViewModel(GoogleCalendar googleCalendar)
        {
            try
            {
                log.Debug("Loading MainWindow view model...");
                calendar = googleCalendar;
                SelectEventsTabCommand = new RelayCommand(SelectEventsTab);
                log.Debug("MainWindow view model was succssfully loaded");
            }
            catch(Exception ex)
            {
                log.Error("Failed to load MainWindow view model:", ex);
            }            
        }

        #endregion

        public const string EventsTabSelectedPropertyName = "EventsTabSelected";
        private bool _eventsTabSelected = true;
        public bool EventsTabSelected
        {
            get
            {
                return _eventsTabSelected;
            }

            set
            {
                if (_eventsTabSelected == value)
                {
                    return;
                }

                RaisePropertyChanging(EventsTabSelectedPropertyName);
                _eventsTabSelected = value;
                RaisePropertyChanged(EventsTabSelectedPropertyName);
            }
        }

        private void SelectEventsTab()
        {
            EventsTabSelected = true;
        }
    }
}