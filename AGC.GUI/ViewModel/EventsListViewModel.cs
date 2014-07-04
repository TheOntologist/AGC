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

        #region Choose Date

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

        #region Common Date Properties

        public const string YearListPropertyName = "YearList";
        private List<int> _yearList = GetYearsList();
        public List<int> YearList
        {
            get
            {
                return _yearList;
            }

            set
            {
                if (_yearList == value)
                {
                    return;
                }

                RaisePropertyChanging(YearListPropertyName);
                _yearList = value;
                RaisePropertyChanged(YearListPropertyName);
            }
        }

        public const string MonthListPropertyName = "MonthList";
        private string[] _monthList = GetMonthNames();
        public string[] MonthList
        {
            get
            {
                return _monthList;
            }

            set
            {
                if (_monthList == value)
                {
                    return;
                }

                RaisePropertyChanging(MonthListPropertyName);
                _monthList = value;
                RaisePropertyChanged(MonthListPropertyName);
            }
        }

        #endregion

        #region Start Date

        public const string StartDayListPropertyName = "StartDayList";
        private List<int> _startDayList = Enumerable.Range(1, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month)).ToList();
        public List<int> StartDayList
        {
            get
            {
                return _startDayList;
            }

            set
            {
                if (_startDayList == value)
                {
                    return;
                }

                RaisePropertyChanging(StartDayListPropertyName);
                _startDayList = value;
                RaisePropertyChanged(StartDayListPropertyName);
            }
        }

        public const string StartDayOfWeekPropertyName = "StartDayOfWeek";
        private string _startDayOfWeek = String.Format("{0:ddd}", DateTime.Today);
        public string StartDayOfWeek
        {
            get
            {
                return _startDayOfWeek;
            }

            set
            {
                if (_startDayOfWeek == value)
                {
                    return;
                }

                RaisePropertyChanging(StartDayOfWeekPropertyName);
                _startDayOfWeek = value;
                RaisePropertyChanged(StartDayOfWeekPropertyName);
            }
        }

        public const string SelectedStartYearPropertyName = "SelectedStartYear";
        private int _selectedStartYear = DateTime.Today.Year;
        public int SelectedStartYear
        {
            get
            {
                return _selectedStartYear;
            }

            set
            {
                if (_selectedStartYear == value)
                {
                    return;
                }

                RaisePropertyChanging(SelectedStartYearPropertyName);
                _selectedStartYear = value;
                RaisePropertyChanged(SelectedStartYearPropertyName);
                SetStartDaysList();
                SetStartDayOfWeek();
            }
        }

        public const string SelectedStartMonthPropertyName = "SelectedStartMonth";
        private int _selectedStartMonth = DateTime.Today.Month - 1;
        public int SelectedStartMonth
        {
            get
            {
                return _selectedStartMonth;
            }

            set
            {
                if (_selectedStartMonth == value)
                {
                    return;
                }

                RaisePropertyChanging(SelectedStartMonthPropertyName);
                _selectedStartMonth = value;
                RaisePropertyChanged(SelectedStartMonthPropertyName);
                SetStartDaysList();
                SetStartDayOfWeek();
            }
        }

        public const string SelectedStartDayPropertyName = "SelectedStartDay";
        private int _selectedStartDay = DateTime.Today.Day;
        public int SelectedStartDay
        {
            get
            {
                return _selectedStartDay;
            }

            set
            {
                if (_selectedStartDay == value)
                {
                    return;
                }

                RaisePropertyChanging(SelectedStartDayPropertyName);
                _selectedStartDay = value;
                RaisePropertyChanged(SelectedStartDayPropertyName);
                SetStartDayOfWeek();
            }
        }

        #endregion

        #region End Date

        public const string EndDayListPropertyName = "EndDayList";
        private List<int> _endDayList = Enumerable.Range(1, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month)).ToList();
        public List<int> EndDayList
        {
            get
            {
                return _endDayList;
            }

            set
            {
                if (_endDayList == value)
                {
                    return;
                }

                RaisePropertyChanging(EndDayListPropertyName);
                _endDayList = value;
                RaisePropertyChanged(EndDayListPropertyName);
            }
        }

        public const string EndDayOfWeekPropertyName = "EndDayOfWeek";
        private string _endDayOfWeek = String.Format("{0:ddd}", DateTime.Today);
        public string EndDayOfWeek
        {
            get
            {
                return _endDayOfWeek;
            }

            set
            {
                if (_endDayOfWeek == value)
                {
                    return;
                }

                RaisePropertyChanging(EndDayOfWeekPropertyName);
                _endDayOfWeek = value;
                RaisePropertyChanged(EndDayOfWeekPropertyName);
            }
        }

        public const string SelectedEndYearPropertyName = "SelectedEndYear";
        private int _selectedEndYear = DateTime.Today.Year;
        public int SelectedEndYear
        {
            get
            {
                return _selectedEndYear;
            }

            set
            {
                if (_selectedEndYear == value)
                {
                    return;
                }

                RaisePropertyChanging(SelectedEndYearPropertyName);
                _selectedEndYear = value;
                RaisePropertyChanged(SelectedEndYearPropertyName);
                SetEndDaysList();
                SetEndDayOfWeek();
            }
        }

        public const string SelectedEndMonthPropertyName = "SelectedEndMonth";
        private int _selectedEndMonth = DateTime.Today.Month - 1;
        public int SelectedEndMonth
        {
            get
            {
                return _selectedEndMonth;
            }

            set
            {
                if (_selectedEndMonth == value)
                {
                    return;
                }

                RaisePropertyChanging(SelectedEndMonthPropertyName);
                _selectedEndMonth = value;
                RaisePropertyChanged(SelectedEndMonthPropertyName);
                SetEndDaysList();
                SetEndDayOfWeek();
            }
        }

        public const string SelectedEndDayPropertyName = "SelectedEndDay";
        private int _selectedEndDay = DateTime.Today.Day;
        public int SelectedEndDay
        {
            get
            {
                return _selectedEndDay;
            }

            set
            {
                if (_selectedEndDay == value)
                {
                    return;
                }

                RaisePropertyChanging(SelectedEndDayPropertyName);
                _selectedEndDay = value;
                RaisePropertyChanged(SelectedEndDayPropertyName);
                SetEndDayOfWeek();
            }
        }

        #endregion

        #endregion

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
            Events = service.GetEvents(calendar.GetAllEvents(), GetChooseDateStart(), GetChooseDateEnd());
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

        private DateTime GetChooseDateStart()
        {
            return new DateTime(SelectedStartYear, SelectedStartMonth + 1, SelectedStartDay);
        }

        private DateTime GetChooseDateEnd()
        {
            DateTime end = new DateTime(SelectedEndYear, SelectedEndMonth + 1, SelectedEndDay, 23, 59, 59);
            return EndDateSpecified ? end : end.AddYears(100);
        }

        #region Date Pickers 

        private static string[] GetMonthNames()
        {
            string[] monthNames = DateTimeFormatInfo.CurrentInfo.AbbreviatedMonthNames;
            Array.Resize(ref monthNames, 12);
            return monthNames;
        }

        private static List<int> GetYearsList()
        {
            List<int> years = Enumerable.Range(DateTime.Now.Year - 10, 20).ToList();
            years.Reverse();
            return years;
        }

        private void SetStartDaysList()
        {
            List<int> days = Enumerable.Range(1, DateTime.DaysInMonth(SelectedStartYear, SelectedStartMonth + 1)).ToList();
            days.Reverse();
            StartDayList = days;
            SelectedStartDay = SelectedStartDay > days.Count ? days.Count : SelectedStartDay;
        }

        private void SetEndDaysList()
        {
            List<int> days = Enumerable.Range(1, DateTime.DaysInMonth(SelectedEndYear, SelectedEndMonth + 1)).ToList();
            days.Reverse();
            EndDayList = days;
            SelectedEndDay = SelectedEndDay > days.Count ? days.Count : SelectedEndDay;
        }

        private void SetStartDayOfWeek()
        {
            StartDayOfWeek = String.Format("{0:ddd}", new DateTime(SelectedStartYear, SelectedStartMonth + 1, SelectedStartDay));
        }

        private void SetEndDayOfWeek()
        {
            EndDayOfWeek = String.Format("{0:ddd}", new DateTime(SelectedEndYear, SelectedEndMonth + 1, SelectedEndDay));
        }

        #endregion

        #endregion
    }
}