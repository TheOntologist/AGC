using AGC.GUI.Common;
using AGC.Library;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;

namespace AGC.GUI.ViewModel
{
    public class EventsListViewModel : ViewModelBase
    {
        #region Private Properties

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IGoogleCalendar calendar;
        private readonly ICalendarEventService service;
        private readonly ITimeIntervals period;
        private readonly IRepository repository;

        private enum EventsListType
        {
            Today,
            Tomorrow,
            ThisWeek,
            NextWeek,
            ThisMonth,
            NextMonth,
            Period,
            Search
        }

        private EventsListType eventListType = EventsListType.Today;

        #endregion

        #region Commands

        public RelayCommand GetTodayEventsCommand { get; private set; }
        public RelayCommand GetTomorrowEventsCommand { get; private set; }
        public RelayCommand GetThisWeekEventsCommand { get; private set; }
        public RelayCommand GetNextWeekEventsCommand { get; private set; }
        public RelayCommand GetThisMonthEventsCommand { get; private set; }
        public RelayCommand GetNextMonthEventsCommand { get; private set; }
        public RelayCommand GetPeriodEventsCommand { get; private set; }
        public RelayCommand SearchEventsCommand { get; private set; }
        public RelayCommand DeleteEventCommand { get; private set; }
        public RelayCommand ShowChooseDateEventsControlsCommand { get; private set; }
        public RelayCommand HideChooseDateEventsControlsCommand { get; private set; }
        public RelayCommand GetChooseDateEventsCommand { get; private set; }

        #endregion

        #region Constructor

        public EventsListViewModel(IGoogleCalendar googleCalendar, ICalendarEventService eventService, ITimeIntervals timeInterval, IRepository commonRepository)
        {
            try
            {
                log.Debug("Loading EventsList view model...");

                calendar = googleCalendar;
                service = eventService;
                period = timeInterval;
                repository = commonRepository;
                Events = service.GetEvents(calendar.GetAllEvents(), period.Today());

                GetTodayEventsCommand = new RelayCommand(GetTodayEvents);
                GetTomorrowEventsCommand = new RelayCommand(GetTomorrowEvents);
                GetThisWeekEventsCommand = new RelayCommand(GetThisWeekEvents);
                GetNextWeekEventsCommand = new RelayCommand(GetNextWeekEvents);
                GetThisMonthEventsCommand = new RelayCommand(GetThisMonthEvents);
                GetNextMonthEventsCommand = new RelayCommand(GetNextMonthEvents);
                GetPeriodEventsCommand = new RelayCommand(GetPeriodEvents);
                SearchEventsCommand = new RelayCommand(SearchEvents);
                DeleteEventCommand = new RelayCommand(DeleteEvent);
                ShowChooseDateEventsControlsCommand = new RelayCommand(ShowChooseDateEventsControls);
                HideChooseDateEventsControlsCommand = new RelayCommand(HideChooseDateEventsControls);
                GetChooseDateEventsCommand = new RelayCommand(GetChooseDateEvents);

                log.Debug("EventsList view model was succssfully loaded");
            }
            catch(Exception ex)
            {
                log.Error("Failed to load EventsList view model:", ex);
            }
        }

        #endregion

        #region Public Properties

        public const string EventsPropertyName = "Events";
        private CalendarEventList _events;
        public CalendarEventList Events
        {
            get
            {
                return _events;
            }

            set
            {
                if (_events == value)
                {
                    return;
                }

                RaisePropertyChanging(EventsPropertyName);
                _events = value;
                RaisePropertyChanged(EventsPropertyName);
            }
        }

        public const string NumberOfMonthToAddPropertyName = "NumberOfMonthToAdd";
        private int _numberOfMonhToAdd = 0;
        public int NumberOfMonthToAdd
        {
            get
            {
                return _numberOfMonhToAdd;
            }

            set
            {
                if (_numberOfMonhToAdd == value)
                {
                    return;
                }

                RaisePropertyChanging(NumberOfMonthToAddPropertyName);
                _numberOfMonhToAdd = value;
                RaisePropertyChanged(NumberOfMonthToAddPropertyName);
            }
        }

        public const string NumberOfMonthToAddRangePropertyName = "NumberOfMonthToAddRange";
        private int[] _numberOfMonthToAddRange = new int[] {12,11,10,9,8,7,6,5,4,3,2,1,0};
        public int[] NumberOfMonthToAddRange
        {
            get
            {
                return _numberOfMonthToAddRange;
            }

            set
            {
                if (_numberOfMonthToAddRange == value)
                {
                    return;
                }

                RaisePropertyChanging(NumberOfMonthToAddRangePropertyName);
                _numberOfMonthToAddRange = value;
                RaisePropertyChanged(NumberOfMonthToAddRangePropertyName);
            }
        }

        public const string SingleMonthPeriodPropertyName = "SingleMonthPeriod";
        private bool _singleMonthPeriod = false;
        public bool SingleMonthPeriod
        {
            get
            {
                return _singleMonthPeriod;
            }

            set
            {
                if (_singleMonthPeriod == value)
                {
                    return;
                }

                RaisePropertyChanging(SingleMonthPeriodPropertyName);
                _singleMonthPeriod = value;
                RaisePropertyChanged(SingleMonthPeriodPropertyName);
            }
        }

        public const string TextToSearchPropertyName = "TextToSearch";
        private string _textToSearch = String.Empty;
        public String TextToSearch
        {
            get
            {
                return _textToSearch;
            }

            set
            {
                if (_textToSearch == value)
                {
                    return;
                }

                RaisePropertyChanging(TextToSearchPropertyName);
                _textToSearch = value;
                RaisePropertyChanged(TextToSearchPropertyName);
            }
        }

        public const string IsEventsListFocusedPropertyName = "IsEventsListFocused";
        private bool _isEventsListFocused = false;
        public bool IsEventsListFocused
        {
            get
            {
                return _isEventsListFocused;
            }

            set
            {
                if (_isEventsListFocused == value)
                {
                    RaisePropertyChanging(IsEventsListFocusedPropertyName);
                    _isEventsListFocused = false;
                    RaisePropertyChanged(IsEventsListFocusedPropertyName);
                }

                RaisePropertyChanging(IsEventsListFocusedPropertyName);
                _isEventsListFocused = value;
                RaisePropertyChanged(IsEventsListFocusedPropertyName);
            }
        }

        public const string IsDefaultControlsFocusedPropertyName = "IsDefaultControlsFocused";
        private bool _isDefaultControlsFocused = false;
        public bool IsDefaultControlsFocused
        {
            get
            {
                return _isDefaultControlsFocused;
            }

            set
            {
                if (_isDefaultControlsFocused == value)
                {
                    return;
                }

                RaisePropertyChanging(IsDefaultControlsFocusedPropertyName);
                _isDefaultControlsFocused = value;
                RaisePropertyChanged(IsDefaultControlsFocusedPropertyName);
            }
        }

        public const string IsChooseDateControlsFocusedPropertyName = "IsChooseDateControlsFocused";
        private bool _isChooseDateControlsFocused = false;
        public bool IsChooseDateControlsFocused
        {
            get
            {
                return _isChooseDateControlsFocused;
            }

            set
            {
                if (_isChooseDateControlsFocused == value)
                {
                    return;
                }

                RaisePropertyChanging(IsChooseDateControlsFocusedPropertyName);
                _isChooseDateControlsFocused = value;
                RaisePropertyChanged(IsChooseDateControlsFocusedPropertyName);
            }
        }

        public const string SelectedEventPropertyName = "SelectedEvent";
        private CalendarEvent _selectedEvent;
        public CalendarEvent SelectedEvent
        {
            get
            {
                return  _selectedEvent;
            }

            set
            {
                if ( _selectedEvent == value)
                {
                    return;
                }

                RaisePropertyChanging(SelectedEventPropertyName);
                 _selectedEvent = value;
                RaisePropertyChanged(SelectedEventPropertyName);
            }
        }

        public const string ShowChooseDateControlsPropertyName = "ShowChooseDateControls";
        private bool _showChooseDateControls = false;
        public bool ShowChooseDateControls
        {
            get
            {
                return _showChooseDateControls;
            }

            set
            {
                if (_showChooseDateControls == value)
                {
                    return;
                }

                RaisePropertyChanging(ShowChooseDateControlsPropertyName);
                _showChooseDateControls = value;
                RaisePropertyChanged(ShowChooseDateControlsPropertyName);
            }
        }

        public const string ShowDefaultControlsPropertyName = "ShowDefaultControls";
        private bool _showDefaultControls = true;
        public bool ShowDefaultControls
        {
            get
            {
                return _showDefaultControls;
            }

            set
            {
                if (_showDefaultControls == value)
                {
                    return;
                }

                RaisePropertyChanging(ShowDefaultControlsPropertyName);
                _showDefaultControls = value;
                RaisePropertyChanged(ShowDefaultControlsPropertyName);
            }
        }

        public const string EndDateSpecifiedPropertyName = "EndDateSpecified";
        private bool _endDateSpecified = false;
        public bool EndDateSpecified
        {
            get
            {
                return _endDateSpecified;
            }

            set
            {
                if (_endDateSpecified == value)
                {
                    return;
                }

                RaisePropertyChanging(EndDateSpecifiedPropertyName);
                _endDateSpecified = value;
                RaisePropertyChanged(EndDateSpecifiedPropertyName);
            }
        }

        public const string StartDatePropertyName = "StartDate";
        private DateTime _startDate = DateTime.Today;
        public DateTime StartDate
        {
            get
            {
                return _startDate;
            }

            set
            {
                if (_startDate == value)
                {
                    return;
                }

                RaisePropertyChanging(StartDatePropertyName);
                _startDate = value;
                RaisePropertyChanged(StartDatePropertyName);
            }
        }

        public const string EndDatePropertyName = "EndDate";
        private DateTime _endDate = DateTime.Today;
        public DateTime EndDate
        {
            get
            {
                return _endDate;
            }

            set
            {
                if (_endDate == value)
                {
                    return;
                }

                RaisePropertyChanging(EndDatePropertyName);
                _endDate = value;
                RaisePropertyChanged(EndDatePropertyName);
            }
        }

        #endregion

        #region Private Methods

        private void GetTodayEvents()
        {
            #region Test
            log.Debug("[TEST] Get Today Events. Dates ");
            DateTime BaseDate = DateTime.Today;
            log.Debug("[TEST] BaseDate " + DateTime.Today);
            DateTime end = BaseDate.AddDays(1).AddSeconds(-1);
            log.Debug("[TEST] End " + end);
            log.Debug("[TEST] Period start " + period.Today().Start + " End " + period.Today().End);
            #endregion

            
            Events = service.GetEvents(calendar.GetAllEvents(), period.Today());
            eventListType = EventsListType.Today;
            ShowResults();   
        }

        private void GetTomorrowEvents()
        {
            #region Test
            log.Debug("[TEST] Get Tomorrow Events. Dates ");
            DateTime BaseDate = DateTime.Today;
            log.Debug("[TEST] BaseDate " + DateTime.Today);
            DateTime start = BaseDate.AddDays(1);
            DateTime end = BaseDate.AddDays(2).AddSeconds(-1);
            log.Debug("[TEST] start " + start);
            log.Debug("[TEST] end " + end);
            log.Debug("[TEST] Period start " + period.Tomorrow().Start + " End " + period.Tomorrow().End);
            #endregion

            Events = service.GetEvents(calendar.GetAllEvents(), period.Tomorrow());
            eventListType = EventsListType.Tomorrow;
            ShowResults();  
        }

        private void GetThisWeekEvents()
        {
            Events = service.GetEvents(calendar.GetAllEvents(), period.ThisWeek());
            eventListType = EventsListType.ThisWeek;
            ShowResults();
        }

        private void GetNextWeekEvents()
        {
            Events = service.GetEvents(calendar.GetAllEvents(), period.NextWeek());
            eventListType = EventsListType.NextWeek;
            ShowResults();
        }

        private void GetThisMonthEvents()
        {
            Events = service.GetEvents(calendar.GetAllEvents(), period.ThisMonth());
            eventListType = EventsListType.ThisMonth;
            ShowResults();
        }

        private void GetNextMonthEvents()
        {
            Events = service.GetEvents(calendar.GetAllEvents(), period.NextMonth());
            eventListType = EventsListType.NextMonth;
            ShowResults();
        }

        private void GetPeriodEvents()
        {
            Events = service.GetEvents(calendar.GetAllEvents(), period.NextNMonth(NumberOfMonthToAdd, SingleMonthPeriod));
            eventListType = EventsListType.Period;
            ShowResults();
        }

        private void SearchEvents()
        {
            Events = service.SearchEvents(calendar.GetAllEvents(), TextToSearch);
            eventListType = EventsListType.Search;
            ShowResults();
        }

        private void DeleteEvent()
        {
            if (SelectedEvent.IsRecurrenceEvent)
            {
                repository.SetCurrentEvent(SelectedEvent);
                var deleteEventWindow = new Views.DeleteEventView();
                deleteEventWindow.ShowDialog();
            }
            else
            {
                if (calendar.DeleteEvent(SelectedEvent))
                {
                    MessageBox.Show(Application.Current.MainWindow, "Deleted", "Information", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                }
                else
                {
                    MessageBox.Show(Application.Current.MainWindow, "Failed to delete event. Please check log file for a detailed information about the error.", "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                }
            }
            RefreshEventsList();
        }

        private void ShowChooseDateEventsControls()
        {
            ShowChooseDateControls = true;
            ShowDefaultControls = false;
            IsDefaultControlsFocused = true;
        }

        private void HideChooseDateEventsControls()
        {
            ShowChooseDateControls = false;
            ShowDefaultControls = true;
            IsChooseDateControlsFocused = true;
        }

        private void GetChooseDateEvents()
        {
            Events = service.GetEvents(calendar.GetAllEvents(), StartDate, EndDate.AddHours(23).AddMinutes(59).AddSeconds(59));
            eventListType = EventsListType.ThisMonth;
            ShowResults();
        }

        private void ShowResults()
        {
            if (Events.Count == 0)
            {
                MessageBox.Show(Application.Current.MainWindow, "No events", "Information", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
            }                
            else
            {
                IsEventsListFocused = true;
            }
        }

        private void RefreshEventsList()
        {
            switch (eventListType)
            {
                case EventsListType.Today:
                    {
                        GetTodayEvents();
                        break;
                    }
                case EventsListType.Tomorrow:
                    {
                        GetTomorrowEvents();
                        break;
                    }
                case EventsListType.ThisWeek:
                    {
                        GetThisWeekEvents();
                        break;
                    }
                case EventsListType.NextWeek:
                    {
                        GetNextWeekEvents();
                        break;
                    }
                case EventsListType.ThisMonth:
                    {
                        GetThisMonthEvents();
                        break;
                    }
                case EventsListType.NextMonth:
                    {
                        GetNextMonthEvents();
                        break;
                    }
                case EventsListType.Period:
                    {
                        GetPeriodEvents();
                        break;
                    }
                case EventsListType.Search:
                    {
                        SearchEvents();
                        break;
                    }
            }
        }

        #endregion
    }
}