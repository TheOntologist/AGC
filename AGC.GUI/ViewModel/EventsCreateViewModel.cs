using AGC.GUI.Common;
using AGC.Library;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;

namespace AGC.GUI.ViewModel
{
    public class EventsCreateViewModel : ViewModelBase
    {
        #region Constants

        private const string DAILY = "Daily";
        private const string WEEKLY_WORKDAYS = "Every Weekday (Mon-Fri)";
        private const string WEEKLY_MON_WED_FRI = "Every Mon., Wed., Fri.";
        private const string WEEKLY_TUESD_THURS = "Every Tuesd. and Thurs.";
        private const string WEEKLY = "Weekly";
        private const string MONTHLY = "Monthly";
        private const string YEARLY = "Yearly";

        private const string MINUTES = "minutes";
        private const string HOURS = "hours";
        private const string DAYS = "days";
        private const string WEEKS = "weeks";
        private const string MONTHS = "months";
        private const string YEARS = "years";

        private const int DEFAULT_EVENT_LENGTH_IN_HOURS = 1;

        #endregion

        #region Private Properties

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IGoogleCalendar calendar;
        private readonly IRepository repository;
        private readonly IMessanger messanger;
        private IRecurrenceSettings recurrence;
        private CalendarEventUpdater eventUpdater;

        private static List<string> RECURRENCE_TYPE = new List<string>(new string[] { DAILY, WEEKLY_WORKDAYS, WEEKLY_MON_WED_FRI, WEEKLY_TUESD_THURS, WEEKLY, MONTHLY, YEARLY });
        private static List<string> REMINDER_TYPE = new List<string>(new string[] { MINUTES, HOURS, DAYS });
        #endregion

        #region Commands

        public RelayCommand CreateEventCommand { get; private set; }
        public RelayCommand UpdateEventCommand { get; private set; }
        public RelayCommand QuickActionCommand { get; private set; }
        public RelayCommand CancelUpdateEventCommand { get; private set; }

        #endregion

        #region Constructor

        public EventsCreateViewModel(IGoogleCalendar googleCalendar, IRecurrenceSettings recurrenceSettings, IRepository commonRepository, IMessanger commonMessanger)
        {
            try
            {
                log.Debug("Loading EventsCreate view model...");

                calendar = googleCalendar;
                repository = commonRepository;
                messanger = commonMessanger;
                recurrence = recurrenceSettings;

                CreateEventCommand = new RelayCommand(CreateEvent);
                UpdateEventCommand = new RelayCommand(UpdateEvent);
                QuickActionCommand = new RelayCommand(QuickAction);
                CancelUpdateEventCommand = new RelayCommand(CancelUpdateEvent);

                // For Update Events
                eventUpdater = repository.GetEventUpdater();
                if (eventUpdater.Type != GoogleCalendar.ActionType.none)
                {
                    IsUpdate = true;
                    SetUpdateEventSettings(eventUpdater);
                }

                log.Debug("EventsCreate view model was succssfully loaded");
            }
            catch(Exception ex)
            {
                log.Error("Failed to load EventsCreate view model:", ex);
            }
        }

        #endregion

        #region Public Properties

        public const string IsNewEventPropertyName = "IsNewEvent";
        private bool _isNewEvent = true;
        public bool IsNewEvent
        {
            get
            {
                return _isNewEvent;
            }

            set
            {
                if (_isNewEvent == value)
                {
                    return;
                }

                RaisePropertyChanging(IsNewEventPropertyName);
                _isNewEvent = value;
                RaisePropertyChanged(IsNewEventPropertyName);

                if (value)
                {
                    IsUpdate = false;
                }            
            }
        }

        public const string IsUpdatePropertyName = "IsUpdate";
        private bool _isUpdate = false;
        public bool IsUpdate
        {
            get
            {
                return _isUpdate;
            }

            set
            {
                if (_isUpdate == value)
                {
                    return;
                }

                RaisePropertyChanging(IsUpdatePropertyName);
                _isUpdate = value;
                RaisePropertyChanged(IsUpdatePropertyName);

                if (value)
                {
                    IsNewEvent = false;
                }            
            }
        }

        public const string TitlePropertyName = "Title";
        private string _title = String.Empty;
        public string Title
        {
            get
            {
                return _title;
            }

            set
            {
                if (_title == value)
                {
                    return;
                }

                RaisePropertyChanging(TitlePropertyName);
                _title = value;
                RaisePropertyChanged(TitlePropertyName);
            }
        }

        public const string ContentPropertyName = "Content";
        private string _content = String.Empty;
        public string Content
        {
            get
            {
                return _content;
            }

            set
            {
                if (_content == value)
                {
                    return;
                }

                RaisePropertyChanging(ContentPropertyName);
                _content = value;
                RaisePropertyChanged(ContentPropertyName);
            }
        }

        public const string LocationPropertyName = "Location";
        private string _location = String.Empty;
        public string Location
        {
            get
            {
                return _location;
            }

            set
            {
                if (_location == value)
                {
                    return;
                }

                RaisePropertyChanging(LocationPropertyName);
                _location = value;
                RaisePropertyChanged(LocationPropertyName);
            }
        }

        public const string IsFullDayEventPropertyName = "IsFullDayEvent";
        private bool _isFullDayEvent = false;
        public bool IsFullDayEvent
        {
            get
            {
                return _isFullDayEvent;
            }

            set
            {
                if (_isFullDayEvent == value)
                {
                    return;
                }

                RaisePropertyChanging(IsFullDayEventPropertyName);
                _isFullDayEvent = value;
                RaisePropertyChanged(IsFullDayEventPropertyName);
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
                CalculateEndDateTimeFromStartDateTime();
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

        public const string StartTimeHoursPropertyName = "StartTimeHours";
        private int _startTimeHours = DateTime.Now.Hour;
        public int StartTimeHours
        {
            get
            {
                return _startTimeHours;
            }

            set
            {
                if (_startTimeHours == value)
                {
                    return;
                }

                RaisePropertyChanging(StartTimeHoursPropertyName);
                _startTimeHours = value;
                RaisePropertyChanged(StartTimeHoursPropertyName);
                CalculateEndDateTimeFromStartDateTime();
            }
        }

        public const string StartTimeMinutesPropertyName = "StartTimeMinutes";
        private int _startTimeMinutes = DateTime.Now.Minute;
        public int StartTimeMinutes
        {
            get
            {
                return _startTimeMinutes;
            }

            set
            {
                if (_startTimeMinutes == value)
                {
                    return;
                }

                RaisePropertyChanging(StartTimeMinutesPropertyName);
                _startTimeMinutes = value;
                RaisePropertyChanged(StartTimeMinutesPropertyName);
                CalculateEndDateTimeFromStartDateTime();
            }
        }

        public const string EndTimeHoursPropertyName = "EndTimeHours";
        private int _endTimeHours = DateTime.Now.Hour + DEFAULT_EVENT_LENGTH_IN_HOURS;
        public int EndTimeHours
        {
            get
            {
                return _endTimeHours;
            }

            set
            {
                if (_endTimeHours == value)
                {
                    return;
                }

                RaisePropertyChanging(EndTimeHoursPropertyName);
                _endTimeHours = value;
                RaisePropertyChanged(EndTimeHoursPropertyName);
            }
        }

        public const string EndTimeMinutesPropertyName = "EndTimeMinutes";
        private int _endTimeMinutes = DateTime.Now.Minute;
        public int EndTimeMinutes
        {
            get
            {
                return _endTimeMinutes;
            }

            set
            {
                if (_endTimeMinutes == value)
                {
                    return;
                }

                RaisePropertyChanging(EndTimeMinutesPropertyName);
                _endTimeMinutes = value;
                RaisePropertyChanged(EndTimeMinutesPropertyName);
            }
        }

        #region Recurrence Settings

        public const string IsRecurrenceEnabledPropertyName = "IsRecurrenceEnabled";
        private bool _isRecurrenceEnabled = true;
        public bool IsRecurrenceEnabled
        {
            get
            {
                return _isRecurrenceEnabled;
            }

            set
            {
                if (_isRecurrenceEnabled == value)
                {
                    return;
                }

                RaisePropertyChanging(IsRecurrenceEnabledPropertyName);
                _isRecurrenceEnabled = value;
                RaisePropertyChanged(IsRecurrenceEnabledPropertyName);
            }
        }

        public const string IsRecurringEventPropertyName = "IsRecurringEvent";
        private bool _isRecurringEvent = false;
        public bool IsRecurringEvent
        {
            get
            {
                return _isRecurringEvent;
            }

            set
            {
                if (_isRecurringEvent == value)
                {
                    return;
                }

                RaisePropertyChanging(IsRecurringEventPropertyName);
                _isRecurringEvent = value;
                RaisePropertyChanged(IsRecurringEventPropertyName);
            }
        }

        public const string RecurrenceTypePropertyName = "RecurrenceType";
        private List<string> _recurrenceType = RECURRENCE_TYPE;
        public List<string> RecurrenceType
        {
            get
            {
                return _recurrenceType;
            }

            set
            {
                if (_recurrenceType == value)
                {
                    return;
                }

                RaisePropertyChanging(RecurrenceTypePropertyName);
                _recurrenceType = value;
                RaisePropertyChanged(RecurrenceTypePropertyName);
            }
        }

        public const string SelectedRecurrenceTypePropertyName = "SelectedRecurrenceType";
        private string _selectedRecurrenceType = WEEKLY;
        public string SelectedRecurrenceType
        {
            get
            {
                return _selectedRecurrenceType;
            }

            set
            {
                if (_selectedRecurrenceType == value)
                {
                    return;
                }

                RaisePropertyChanging(SelectedRecurrenceTypePropertyName);
                _selectedRecurrenceType = value;
                RaisePropertyChanged(SelectedRecurrenceTypePropertyName);

                SetRecurrenceTypeControls(value);
            }
        }

        public const string IntervalListPropertyName = "IntervalList";
        private List<int> _intervalList = Enumerable.Range(1, 30).ToList();
        public List<int> IntervalList
        {
            get
            {
                return _intervalList;
            }

            set
            {
                if (_intervalList == value)
                {
                    return;
                }

                RaisePropertyChanging(IntervalListPropertyName);
                _intervalList = value;
                RaisePropertyChanged(IntervalListPropertyName);
            }
        }

        public const string IntervalPropertyName = "Interval";
        private int _interval = 1;
        public int Interval
        {
            get
            {
                return _interval;
            }

            set
            {
                if (_interval == value)
                {
                    return;
                }

                RaisePropertyChanging(IntervalPropertyName);
                _interval = value;
                RaisePropertyChanged(IntervalPropertyName);
            }
        }

        public const string IntervalTypePropertyName = "IntervalType";
        private string _intervalType = WEEKS;
        public string IntervalType
        {
            get
            {
                return _intervalType;
            }

            set
            {
                if (_intervalType == value)
                {
                    return;
                }

                RaisePropertyChanging(IntervalTypePropertyName);
                _intervalType = value;
                RaisePropertyChanged(IntervalTypePropertyName);
            }
        }

        public const string RepeatWeeklyPropertyName = "RepeatWeekly";
        private bool _repeatWeekly = true;
        public bool RepeatWeekly
        {
            get
            {
                return _repeatWeekly;
            }

            set
            {
                if (_repeatWeekly == value)
                {
                    return;
                }

                RaisePropertyChanging(RepeatWeeklyPropertyName);
                _repeatWeekly = value;
                RaisePropertyChanged(RepeatWeeklyPropertyName);
            }
        }

        public const string RepeatMonthlyPropertyName = "RepeatMonthly";
        private bool _repeatMonthly = false;
        public bool RepeatMonthly
        {
            get
            {
                return  _repeatMonthly;
            }

            set
            {
                if ( _repeatMonthly == value)
                {
                    return;
                }

                RaisePropertyChanging(RepeatMonthlyPropertyName);
                 _repeatMonthly = value;
                RaisePropertyChanged(RepeatMonthlyPropertyName);
            }
        }

        public const string CustomIntervalPropertyName = "CustomInterval";
        private bool _customInterval = true;
        public bool CustomInterval
        {
            get
            {
                return _customInterval;
            }

            set
            {
                if (_customInterval == value)
                {
                    return;
                }

                RaisePropertyChanging(CustomIntervalPropertyName);
                _customInterval = value;
                RaisePropertyChanged(CustomIntervalPropertyName);
            }
        }

        #region Weekdays

        public const string MondayPropertyName = "Monday";
        private bool _monday = false;
        public bool Monday
        {
            get
            {
                return _monday;
            }

            set
            {
                if (_monday == value)
                {
                    return;
                }

                RaisePropertyChanging(MondayPropertyName);
                _monday = value;
                RaisePropertyChanged(MondayPropertyName);
            }
        }

        public const string TuesdayPropertyName = "Tuesday";
        private bool _tuesday = false;
        public bool Tuesday
        {
            get
            {
                return _tuesday;
            }

            set
            {
                if (_tuesday == value)
                {
                    return;
                }

                RaisePropertyChanging(TuesdayPropertyName);
                _tuesday = value;
                RaisePropertyChanged(TuesdayPropertyName);
            }
        }

        public const string WednesdayPropertyName = "Wednesday";
        private bool _wednesday = false;
        public bool Wednesday
        {
            get
            {
                return _wednesday;
            }

            set
            {
                if (_wednesday == value)
                {
                    return;
                }

                RaisePropertyChanging(WednesdayPropertyName);
                _wednesday = value;
                RaisePropertyChanged(WednesdayPropertyName);
            }
        }

        public const string ThursdayPropertyName = "Thursday";
        private bool _thursday = false;
        public bool Thursday
        {
            get
            {
                return _thursday;
            }

            set
            {
                if (_thursday == value)
                {
                    return;
                }

                RaisePropertyChanging(ThursdayPropertyName);
                _thursday = value;
                RaisePropertyChanged(ThursdayPropertyName);
            }
        }

        public const string FridayPropertyName = "Friday";
        private bool _friday = false;
        public bool Friday
        {
            get
            {
                return _friday;
            }

            set
            {
                if (_friday == value)
                {
                    return;
                }

                RaisePropertyChanging(FridayPropertyName);
                _friday = value;
                RaisePropertyChanged(FridayPropertyName);
            }
        }

        public const string SaturdayPropertyName = "Saturday";
        private bool _saturday = false;
        public bool Saturday
        {
            get
            {
                return _saturday;
            }

            set
            {
                if (_saturday == value)
                {
                    return;
                }

                RaisePropertyChanging(SaturdayPropertyName);
                _saturday = value;
                RaisePropertyChanged(SaturdayPropertyName);
            }
        }

        public const string SundayPropertyName = "Sunday";
        private bool _sunday = false;
        public bool Sunday
        {
            get
            {
                return _sunday;
            }

            set
            {
                if (_sunday == value)
                {
                    return;
                }

                RaisePropertyChanging(SundayPropertyName);
                _sunday = value;
                RaisePropertyChanged(SundayPropertyName);
            }
        }

        #endregion

        public const string ByDayOfTheMonthPropertyName = "ByDayOfTheMonth";
        private bool _byDayOfTheMonth = true;
        public bool ByDayOfTheMonth
        {
            get
            {
                return _byDayOfTheMonth;
            }

            set
            {
                if (_byDayOfTheMonth == value)
                {
                    return;
                }

                RaisePropertyChanging(ByDayOfTheMonthPropertyName);
                _byDayOfTheMonth = value;
                RaisePropertyChanged(ByDayOfTheMonthPropertyName);
            }
        }

        public const string ByDayOfTheWeekPropertyName = "ByDayOfTheWeek";
        private bool _byDayOfTheWeek = false;
        public bool ByDayOfTheWeek
        {
            get
            {
                return _byDayOfTheWeek;
            }

            set
            {
                if (_byDayOfTheWeek == value)
                {
                    return;
                }

                RaisePropertyChanging(ByDayOfTheWeekPropertyName);
                _byDayOfTheWeek = value;
                RaisePropertyChanged(ByDayOfTheWeekPropertyName);
            }
        }

        #region Recurrence End

        public const string EndsNeverPropertyName = "EndsNever";
        private bool _endsNever = true;
        public bool EndsNever
        {
            get
            {
                return _endsNever;
            }

            set
            {
                if (_endsNever == value)
                {
                    return;
                }

                RaisePropertyChanging(EndsNeverPropertyName);
                _endsNever = value;
                RaisePropertyChanged(EndsNeverPropertyName);
            }
        }

        public const string EndsAfterPropertyName = "EndsAfter";
        private bool _endsAfter = false;
        public bool EndsAfter
        {
            get
            {
                return _endsAfter;
            }

            set
            {
                if (_endsAfter == value)
                {
                    return;
                }

                RaisePropertyChanging(EndsAfterPropertyName);
                _endsAfter = value;
                RaisePropertyChanged(EndsAfterPropertyName);
            }
        }

        public const string EndsOnPropertyName = "EndsOn";
        private bool _endsOn = false;
        public bool EndsOn
        {
            get
            {
                return _endsOn;
            }

            set
            {
                if (_endsOn == value)
                {
                    return;
                }

                RaisePropertyChanging(EndsOnPropertyName);
                _endsOn = value;
                RaisePropertyChanged(EndsOnPropertyName);
            }
        }

        public const string RecurrenceEndDatePropertyName = "RecurrenceEndDate";
        private DateTime _recurrenceEndDate = DateTime.Today.AddDays(7);
        public DateTime RecurrenceEndDate
        {
            get
            {
                return _recurrenceEndDate;
            }

            set
            {
                if (_recurrenceEndDate == value)
                {
                    return;
                }

                RaisePropertyChanging(RecurrenceEndDatePropertyName);
                _recurrenceEndDate = value;
                RaisePropertyChanged(RecurrenceEndDatePropertyName);
            }
        }

        public const string RecurrenceEventsCountPropertyName = "RecurrenceEventsCount";
        private int _recurrenceEventsCount = 1;
        public int RecurrenceEventsCount
        {
            get
            {
                return _recurrenceEventsCount;
            }

            set
            {
                if (_recurrenceEventsCount == value)
                {
                    return;
                }

                RaisePropertyChanging(RecurrenceEventsCountPropertyName);
                _recurrenceEventsCount = value;
                RaisePropertyChanged(RecurrenceEventsCountPropertyName);
            }
        }

        #endregion

        #endregion

        #region Reminder

        public const string RepeatValuePropertyName = "RepeatValue";
        private int _repeatValue = 10;
        public int RepeatValue
        {
            get
            {
                return _repeatValue;
            }

            set
            {
                if (_repeatValue == value)
                {
                    return;
                }

                RaisePropertyChanging(RepeatValuePropertyName);
                _repeatValue = value;
                RaisePropertyChanged(RepeatValuePropertyName);
            }
        }

        public const string RepeatTypeListPropertyName = "RepeatTypeList";
        private List<string> _repeatTypeList = REMINDER_TYPE;
        public List<string> RepeatTypeList
        {
            get
            {
                return _repeatTypeList;
            }

            set
            {
                if (_repeatTypeList == value)
                {
                    return;
                }

                RaisePropertyChanging(RepeatTypeListPropertyName);
                _repeatTypeList = value;
                RaisePropertyChanged(RepeatTypeListPropertyName);
            }
        }

        public const string SelectedRepeatTypePropertyName = "SelectedRepeatType";
        private string _selectedRepeatType = MINUTES;
        public string SelectedRepeatType
        {
            get
            {
                return _selectedRepeatType;
            }

            set
            {
                if (_selectedRepeatType == value)
                {
                    return;
                }

                RaisePropertyChanging(SelectedRepeatTypePropertyName);
                _selectedRepeatType = value;
                RaisePropertyChanged(SelectedRepeatTypePropertyName);
            }
        }

        #endregion

        public const string IsConfirmedPropertyName = "IsConfirmed";
        private bool _isConfirmed = true;
        public bool IsConfirmed
        {
            get
            {
                return _isConfirmed;
            }

            set
            {
                if (_isConfirmed == value)
                {
                    return;
                }

                RaisePropertyChanging(IsConfirmedPropertyName);
                _isConfirmed = value;
                RaisePropertyChanged(IsConfirmedPropertyName);
            }
        }

        #endregion

        #region Private Methods

        private void CreateEvent()
        {
            CalendarEvent ev = GetCalendarEvent();

            if (calendar.CreateEvent(ev))
            {
                messanger.Success("Created", false);
                CleanInputText();
                recurrence.Clear(); 
            }
            else
            {
                messanger.Error("Failed to create event. Please check log file for a detailed information about the error.", false);
            }
        }

        private void UpdateEvent()
        {
            CalendarEvent ev = GetCalendarEvent();
            ev.Id = eventUpdater.CalendarEvent.Id;

            if (calendar.UpdateEvent(ev, eventUpdater.Type))
            {
                messanger.Success("Updated", false);
            }
            else
            {
                messanger.Error("Failed to update event. Please check log file for a detailed information about the error.", false);
            }

            CloseUpdateWindow();
        }

        private void QuickAction()
        {
            if (IsUpdate)
            {
                UpdateEvent();
            }
            else
            {
                CreateEvent();
            }
        }

        private void CancelUpdateEvent()
        {
            CloseUpdateWindow();
        }
       
        private CalendarEvent GetCalendarEvent()
        {
            CalendarEvent ev = new CalendarEvent(Title, Content, Location, GetStartDateTime(), GetEndDateTime());

            if (IsRecurringEvent)
            {
                GetRecurrenceSettings();
                ev.RRule = recurrence.ToString();
            }

            if (IsFullDayEvent)
            {
                ev.IsFullDateEvent = true;
            }

            ev.Reminder = CalculateReminderMinutes();
            ev.Confirmed = IsConfirmed;

            return ev;
        }

        private void SetRecurrenceTypeControls(string recurrenceType)
        {
            switch (recurrenceType)
            {
                case DAILY:
                    {
                        RepeatWeekly = false;
                        RepeatMonthly = false;
                        CustomInterval = true;
                        IntervalType = DAYS;
                        break;
                    }
                case WEEKLY_WORKDAYS:
                    {
                        RepeatWeekly = false;
                        RepeatMonthly = false;
                        CustomInterval = false;
                        break;
                    }
                case WEEKLY_MON_WED_FRI:
                    {
                        RepeatWeekly = false;
                        RepeatMonthly = false;
                        CustomInterval = false;
                        break;
                    }
                case WEEKLY_TUESD_THURS:
                    {
                        RepeatWeekly = false;
                        RepeatMonthly = false;
                        CustomInterval = false;
                        break;
                    }
                case WEEKLY:
                    {
                        RepeatWeekly = true;
                        RepeatMonthly = false;
                        CustomInterval = true;
                        IntervalType = WEEKS;
                        break;
                    }
                case MONTHLY:
                    {
                        RepeatWeekly = false;
                        RepeatMonthly = true;
                        CustomInterval = true;
                        IntervalType = MONTHS;
                        break;
                    }
                case YEARLY:
                    {
                        RepeatWeekly = false;
                        RepeatMonthly = false;
                        CustomInterval = true;
                        IntervalType = YEARS;
                        break;
                    }
            }
        }

        private void GetRecurrenceSettings()
        {
            GetRecurrenceStartSettings();
            GetRecurrenceFrequencySettings();
            GetRecurrenceEndSettings();
        }

        private void GetRecurrenceStartSettings()
        {
            recurrence.StartsOn(new DateTime(StartDate.Year, StartDate.Month, StartDate.Day, StartTimeHours, StartTimeMinutes, 0));
        }

        private void GetRecurrenceFrequencySettings()
        {
            switch (SelectedRecurrenceType)
            {
                case DAILY:
                    {
                        recurrence.Daily().Interval(Interval);
                        break;
                    }
                case WEEKLY_WORKDAYS:
                    {
                        recurrence.Weekly().Interval(1).Monday().Tuesday().Wednesday().Thursday().Friday();
                        break;
                    }
                case WEEKLY_MON_WED_FRI:
                    {
                        recurrence.Weekly().Interval(1).Monday().Wednesday().Friday();
                        break;
                    }
                case WEEKLY_TUESD_THURS:
                    {
                        recurrence.Weekly().Interval(1).Tuesday().Thursday();
                        break;
                    }
                case WEEKLY:
                    {
                        recurrence.Weekly().Interval(Interval);
                        GetRecurrenceWeekdays();
                        break;
                    }
                case MONTHLY:
                    {
                        recurrence.Monthly().Interval(Interval);

                        if(ByDayOfTheMonth)
                        {
                            recurrence.ByDayOfMonth();
                        }
                        else
                        {
                            recurrence.ByDayOfWeek();
                        }
                        break;
                    }
                case YEARLY:
                    {
                        recurrence.Yearly().Interval(Interval);
                        break;
                    }
            }
        }

        private void GetRecurrenceEndSettings()
        {
            if (EndsNever)
            {
                recurrence.EndsNever();
            }                
            else if (EndsAfter)
            {
                recurrence.EndsAfter(RecurrenceEventsCount);
            }                
            else if (EndsOn)
            {
                recurrence.EndsOn(RecurrenceEndDate);
            }
        }

        private void GetRecurrenceWeekdays()
        {
            if (Monday)
                recurrence.Monday();
            if (Tuesday)
                recurrence.Tuesday();
            if (Wednesday)
                recurrence.Wednesday();
            if (Thursday)
                recurrence.Thursday();
            if (Friday)
                recurrence.Friday();
            if (Saturday)
                recurrence.Saturday();
            if (Sunday)
                recurrence.Sunday();
        }

        private DateTime GetStartDateTime()
        {
            return new DateTime(StartDate.Year, StartDate.Month, StartDate.Day, StartTimeHours, StartTimeMinutes, 0);
        }

        private DateTime GetEndDateTime()
        {
            return new DateTime(EndDate.Year, EndDate.Month, EndDate.Day, EndTimeHours, EndTimeMinutes, 0);
        }

        private int CalculateReminderMinutes()
        {
            switch (SelectedRepeatType)
            {
                case MINUTES:
                    return RepeatValue;
                case HOURS:
                    return RepeatValue * 60;
                case DAYS:
                    return RepeatValue * 60 * 24;
                default:
                    return 10;
            }
        }

        private void CalculateEndDateTimeFromStartDateTime()
        {
            if (!IsFullDayEvent)
            {
                DateTime start = new DateTime(StartDate.Year, StartDate.Month, StartDate.Day, StartTimeHours, StartTimeMinutes, 0);
                DateTime end = start.AddHours(DEFAULT_EVENT_LENGTH_IN_HOURS);

                EndDate = new DateTime(end.Year, end.Month, end.Day);

                EndTimeHours = end.Hour;
                EndTimeMinutes = end.Minute;
            }
        }

        private void CleanInputText()
        {
            Title = String.Empty;
            Content = String.Empty;
            Location = String.Empty;
        }

        private void SetUpdateEventSettings(CalendarEventUpdater eventUpdater)
        {
            CalendarEvent ev = eventUpdater.CalendarEvent;
            recurrence = eventUpdater.Recurrence == null ? recurrence : eventUpdater.Recurrence;

            // Set general events settings
            Title = ev.Title;
            Content = ev.Content;
            Location = ev.Location;

            IsFullDayEvent = ev.IsFullDateEvent;
            IsRecurringEvent = ev.IsRecurrenceEvent;

            StartDate = ev.Start;
            StartTimeHours = ev.Start.Hour;
            StartTimeMinutes = ev.Start.Minute;

            DateTime end = ev.End ?? ev.Start;
            EndDate = end;
            EndTimeHours = end.Hour;
            EndTimeMinutes = end.Minute;
                        
            // Disable recurrence settings if update single instace option was selected
            if (eventUpdater.Type == GoogleCalendar.ActionType.single && IsRecurringEvent)
            {
                IsRecurringEvent = false;
                IsRecurrenceEnabled = false;
                return;
            }

            SetUpdateEventRecurrenceSettings();
        }

        private void SetUpdateEventRecurrenceSettings()
        {
           // Set Recurrence Type
            if (recurrence.IsRepeatsDaily())
            {
                SelectedRecurrenceType = DAILY;
            }
            else if (recurrence.IsRepeatsWeeky())
            {
                if(recurrence.IsRepeatsOnMonday() && recurrence.IsRepeatsOnTuesday() && recurrence.IsRepeatsOnWednesday()
                    && recurrence.IsRepeatsOnThursday() && recurrence.IsRepeatsOnFriday()
                    && !recurrence.IsRepeatsOnSaturday() && !recurrence.IsRepeatsOnSunday())
                {
                    SelectedRecurrenceType = WEEKLY_WORKDAYS;
                }
                else if (recurrence.IsRepeatsOnMonday() && !recurrence.IsRepeatsOnTuesday() && recurrence.IsRepeatsOnWednesday()
                    && !recurrence.IsRepeatsOnThursday() && recurrence.IsRepeatsOnFriday()
                    && !recurrence.IsRepeatsOnSaturday() && !recurrence.IsRepeatsOnSunday())
                {
                    SelectedRecurrenceType = WEEKLY_MON_WED_FRI;
                }
                else if (!recurrence.IsRepeatsOnMonday() && recurrence.IsRepeatsOnTuesday() && !recurrence.IsRepeatsOnWednesday()
                    && recurrence.IsRepeatsOnThursday() && !recurrence.IsRepeatsOnFriday()
                    && !recurrence.IsRepeatsOnSaturday() && !recurrence.IsRepeatsOnSunday())
                {
                    SelectedRecurrenceType = WEEKLY_TUESD_THURS;
                }
                else
                {
                    SelectedRecurrenceType = WEEKLY;

                    Monday = recurrence.IsRepeatsOnMonday();
                    Tuesday = recurrence.IsRepeatsOnTuesday();
                    Wednesday = recurrence.IsRepeatsOnWednesday();
                    Thursday = recurrence.IsRepeatsOnThursday();
                    Friday = recurrence.IsRepeatsOnFriday();
                    Saturday = recurrence.IsRepeatsOnSaturday();
                    Sunday = recurrence.IsRepeatsOnSunday();
                }
            }
            else if (recurrence.IsRepeatsMonthly())
            {
                SelectedRecurrenceType = MONTHLY;
                ByDayOfTheMonth = recurrence.IsRepeatsByDayOfMonth();
                ByDayOfTheWeek = recurrence.IsRepeatsByDayOfWeek();
            }
            else if (recurrence.IsRepeatsYearly())
            {
                SelectedRecurrenceType = YEARLY;
            }

            // Set interval
            Interval = recurrence.GetInterval();

            // Set Exit condition settings
            if (recurrence.IsEndsNever())
            {
                EndsNever = true;
            }
            else if (recurrence.IsEndsAfterSpecifiedNumberOfOccurences())
            {
                EndsAfter = true;
                RecurrenceEventsCount = recurrence.Count();
            }
            else if (recurrence.IsEndsOnSpecifiedDate())
            {
                EndsOn = true;
                RecurrenceEndDate = recurrence.EndDate() ?? DateTime.Today;
            }
        }

        private void CloseUpdateWindow()
        {
            // Reset Update flag
            eventUpdater.Type = GoogleCalendar.ActionType.none;
            repository.SetEventUpdater(eventUpdater);

            Application.Current.Windows[1].Close();
        }

        #endregion
    }
}