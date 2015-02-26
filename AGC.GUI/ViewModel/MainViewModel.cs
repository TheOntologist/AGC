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
        public RelayCommand SelectCreateEventsTabCommand { get; private set; }
        public RelayCommand SelectQuickEventsTabCommand { get; private set; }
        public RelayCommand SelectAnotherCalendarTabCommand { get; private set; }

        #region Constructor

        public MainViewModel(GoogleCalendar googleCalendar)
        {
            try
            {
                log.Debug("Loading MainWindow view model...");
                calendar = googleCalendar;
                SelectEventsTabCommand = new RelayCommand(SelectEventsTab);
                SelectCreateEventsTabCommand = new RelayCommand(SelectCreateEventsTab);
                SelectQuickEventsTabCommand = new RelayCommand(SelectQuickEventsTab);
                SelectAnotherCalendarTabCommand = new RelayCommand(SelectAnotherCalendarTab);
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

        public const string CreateEventsTabSelectedPropertyName = "CreateEventsTabSelected";
        private bool _createEventsTabSelected = false;
        public bool CreateEventsTabSelected
        {
            get
            {
                return _createEventsTabSelected;
            }

            set
            {
                if (_createEventsTabSelected == value)
                {
                    return;
                }

                _createEventsTabSelected = value;
                RaisePropertyChanged(CreateEventsTabSelectedPropertyName);
            }
        }

        public const string QuickEventsTabSelectedPropertyName = "QuickEventsTabSelected";
        private bool _quickEventsTabSelected = false;
        public bool QuickEventsTabSelected
        {
            get
            {
                return _quickEventsTabSelected;
            }

            set
            {
                if (_quickEventsTabSelected == value)
                {
                    return;
                }

                _quickEventsTabSelected = value;
                RaisePropertyChanged(QuickEventsTabSelectedPropertyName);
            }
        }

        public const string AnotherCalendarTabSelectedPropertyName = "AnotherCalendarTabSelected";
        private bool _anotherCalendarTabSelected = false;
        public bool AnotherCalendarTabSelected
        {
            get
            {
                return _anotherCalendarTabSelected;
            }

            set
            {
                if (_anotherCalendarTabSelected == value)
                {
                    return;
                }

                _anotherCalendarTabSelected = value;
                RaisePropertyChanged(AnotherCalendarTabSelectedPropertyName);
            }
        }
        
        private void SelectEventsTab()
        {
            EventsTabSelected = true;
        }

        private void SelectCreateEventsTab()
        {
            CreateEventsTabSelected = true;
        }

        private void SelectQuickEventsTab()
        {
            QuickEventsTabSelected = true;
        }

        private void SelectAnotherCalendarTab()
        {
            AnotherCalendarTabSelected = true;
        }
    }
}