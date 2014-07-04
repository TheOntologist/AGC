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
        private IRecurrenceSettings recurrence;

        private static List<string> RECURRENCE_TYPE = new List<string>(new string[] { DAILY, WEEKLY_WORKDAYS, WEEKLY_MON_WED_FRI, WEEKLY_TUESD_THURS, WEEKLY, MONTHLY, YEARLY });
        private static List<string> REMINDER_TYPE = new List<string>(new string[] { MINUTES, HOURS, DAYS });
        #endregion

        #region Commands

        public RelayCommand CreateEventCommand { get; private set; }

        #endregion

        #region Constructor

        public EventsCreateViewModel(IGoogleCalendar googleCalendar, IRecurrenceSettings recurrenceSettings)
        {
            try
            {
                log.Debug("Loading EventsCreate view model...");

                calendar = googleCalendar;
                recurrence = recurrenceSettings;

                CreateEventCommand = new RelayCommand(CreateEvent);

                log.Debug("EventsCreate view model was succssfully loaded");
            }
            catch(Exception ex)
            {
                log.Error("Failed to load EventsCreate view model:", ex);
            }
        }

        #endregion

        #region Public Properties

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

        public const string StartDatePropertyName = "StartDate";
        private DateTime _myProperty = DateTime.Today;
        public DateTime StartDate
        {
            get
            {
                return _myProperty;
            }

            set
            {
                if (_myProperty == value)
                {
                    return;
                }

                RaisePropertyChanging(StartDatePropertyName);
                _myProperty = value;
                RaisePropertyChanged(StartDatePropertyName);
            }
        }

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
                CalculateEndDateTimeFromStartDateTime();
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
                CalculateEndDateTimeFromStartDateTime();
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
                CalculateEndDateTimeFromStartDateTime();
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

        #region Common Time Properties

        public const string HoursListPropertyName = "HoursList";
        private List<int> _hoursList = GetHoursList();
        public List<int> HoursList
        {
            get
            {
                return _hoursList;
            }

            set
            {
                if (_hoursList == value)
                {
                    return;
                }

                RaisePropertyChanging(HoursListPropertyName);
                _hoursList = value;
                RaisePropertyChanged(HoursListPropertyName);
            }
        }
 
        public const string MinutesListPropertyName = "MinutesList";
        private List<int> _minutesList = GetMinutesList();
        public List<int> MinutesList
        {
            get
            {
                return _minutesList;
            }

            set
            {
                if (_minutesList == value)
                {
                    return;
                }

                RaisePropertyChanging(MinutesListPropertyName);
                _minutesList = value;
                RaisePropertyChanged(MinutesListPropertyName);
            }
        }

        #endregion

        #region Start Time

        public const string SelectedStartHoursPropertyName = "SelectedStartHours";
        private int _selectedStartHours = DateTime.Now.Hour;
        public int SelectedStartHours
        {
            get
            {
                return _selectedStartHours;
            }

            set
            {
                if (_selectedStartHours == value)
                {
                    return;
                }

                RaisePropertyChanging(SelectedStartHoursPropertyName);
                _selectedStartHours = value;
                RaisePropertyChanged(SelectedStartHoursPropertyName);
                CalculateEndDateTimeFromStartDateTime();
            }
        }

        public const string SelectedStartMinutesPropertyName = "SelectedStartMinutes";
        private int _selectedStartMinutes = DateTime.Now.Minute;
        public int SelectedStartMinutes
        {
            get
            {
                return _selectedStartMinutes;
            }

            set
            {
                if (_selectedStartMinutes == value)
                {
                    return;
                }

                RaisePropertyChanging(SelectedStartMinutesPropertyName);
                _selectedStartMinutes = value;
                RaisePropertyChanged(SelectedStartMinutesPropertyName);
                CalculateEndDateTimeFromStartDateTime();
            }
        }

        #endregion

        #region End Time

        public const string SelectedEndHoursPropertyName = "SelectedEndHours";
        private int _selectedEndHours = DateTime.Now.Hour;
        public int SelectedEndHours
        {
            get
            {
                return _selectedEndHours;
            }

            set
            {
                if (_selectedEndHours == value)
                {
                    return;
                }

                RaisePropertyChanging(SelectedEndHoursPropertyName);
                _selectedEndHours = value;
                RaisePropertyChanged(SelectedEndHoursPropertyName);
            }
        }

        public const string SelectedEndMinutesPropertyName = "SelectedEndMinutes";
        private int _selectedEndMinutes = DateTime.Now.Minute;
        public int SelectedEndMinutes
        {
            get
            {
                return _selectedEndMinutes;
            }

            set
            {
                if (_selectedEndMinutes == value)
                {
                    return;
                }

                RaisePropertyChanging(SelectedEndMinutesPropertyName);
                _selectedEndMinutes = value;
                RaisePropertyChanged(SelectedEndMinutesPropertyName);
            }
        }

        #endregion

        #region Recurrence Settings

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

        #region Recurrence End Date

        public const string RecurrenceEndDayListPropertyName = "RecurrenceEndDayList";
        private List<int> _recurrenceEndDayList = Enumerable.Range(1, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month)).ToList();
        public List<int> RecurrenceEndDayList
        {
            get
            {
                return _recurrenceEndDayList;
            }

            set
            {
                if (_recurrenceEndDayList == value)
                {
                    return;
                }

                RaisePropertyChanging(RecurrenceEndDayListPropertyName);
                _recurrenceEndDayList = value;
                RaisePropertyChanged(RecurrenceEndDayListPropertyName);
            }
        }

        public const string RecurrenceEndDayOfWeekPropertyName = "RecurrenceEndDayOfWeek";
        private string _recurrenceEndDayOfWeek = String.Format("{0:ddd}", DateTime.Today);
        public string RecurrenceEndDayOfWeek
        {
            get
            {
                return _recurrenceEndDayOfWeek;
            }

            set
            {
                if (_recurrenceEndDayOfWeek == value)
                {
                    return;
                }

                RaisePropertyChanging(RecurrenceEndDayOfWeekPropertyName);
                _recurrenceEndDayOfWeek = value;
                RaisePropertyChanged(RecurrenceEndDayOfWeekPropertyName);
            }
        }

        public const string SelectedRecurrenceEndYearPropertyName = "SelectedRecurrenceEndYear";
        private int _selectedRecurrenceEndYear = DateTime.Today.Year;
        public int SelectedRecurrenceEndYear
        {
            get
            {
                return _selectedRecurrenceEndYear;
            }

            set
            {
                if (_selectedRecurrenceEndYear == value)
                {
                    return;
                }

                RaisePropertyChanging(SelectedRecurrenceEndYearPropertyName);
                _selectedRecurrenceEndYear = value;
                RaisePropertyChanged(SelectedRecurrenceEndYearPropertyName);

                SetRecurrenceEndDaysList();
                SetRecurrenceEndDayOfWeek();
            }
        }

        public const string SelectedRecurrenceEndMonthPropertyName = "SelectedRecurrenceEndMonth";
        private int _selectedRecurrenceEndMonth = DateTime.Today.Month - 1;
        public int SelectedRecurrenceEndMonth
        {
            get
            {
                return _selectedRecurrenceEndMonth;
            }

            set
            {
                if (_selectedRecurrenceEndMonth == value)
                {
                    return;
                }

                RaisePropertyChanging(SelectedRecurrenceEndMonthPropertyName);
                _selectedRecurrenceEndMonth = value;
                RaisePropertyChanged(SelectedRecurrenceEndMonthPropertyName);

                SetRecurrenceEndDaysList();
                SetRecurrenceEndDayOfWeek();
            }
        }

        public const string SelectedRecurrenceEndDayPropertyName = "SelectedRecurrenceEndDay";
        private int _selectedRecurrenceEndDay = DateTime.Today.Day;
        public int SelectedRecurrenceEndDay
        {
            get
            {
                return _selectedRecurrenceEndDay;
            }

            set
            {
                if (_selectedRecurrenceEndDay == value)
                {
                    return;
                }

                RaisePropertyChanging(SelectedRecurrenceEndDayPropertyName);
                _selectedRecurrenceEndDay = value;
                RaisePropertyChanged(SelectedRecurrenceEndDayPropertyName);

                SetRecurrenceEndDayOfWeek();
            }
        }

        #endregion

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

        #endregion

        #region Private Methods

        private void CreateEvent()
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
            if (calendar.CreateEvent(ev))
            {
                MessageBox.Show(Application.Current.MainWindow, "Created", "Information", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                CleanInputText();
            }
            else
            {
                MessageBox.Show(Application.Current.MainWindow, "Failed to create event. Please check log file for a detailed information about the error.", "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
        }

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

        private static List<int> GetHoursList()
        {
            List<int> hours = Enumerable.Range(0, 24).ToList();
            hours.Reverse();
            return hours;
        }

        private static List<int> GetMinutesList()
        {
            List<int> minutes = Enumerable.Range(0, 60).ToList();
            minutes.Reverse();
            return minutes;
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

        private void SetRecurrenceEndDaysList()
        {
            List<int> days = Enumerable.Range(1, DateTime.DaysInMonth(SelectedRecurrenceEndYear, SelectedRecurrenceEndMonth + 1)).ToList();
            days.Reverse();
            RecurrenceEndDayList = days;
            SelectedRecurrenceEndDay = SelectedRecurrenceEndDay > days.Count ? days.Count : SelectedRecurrenceEndDay;
        }
       
        private void SetStartDayOfWeek()
        {
            StartDayOfWeek = String.Format("{0:ddd}", new DateTime(SelectedStartYear, SelectedStartMonth + 1, SelectedStartDay));
        }

        private void SetEndDayOfWeek()
        {
            EndDayOfWeek = String.Format("{0:ddd}", new DateTime(SelectedEndYear, SelectedEndMonth + 1, SelectedEndDay));
        }

        private void SetRecurrenceEndDayOfWeek()
        {
            RecurrenceEndDayOfWeek = String.Format("{0:ddd}", new DateTime(SelectedRecurrenceEndYear, SelectedRecurrenceEndMonth + 1, SelectedRecurrenceEndDay));
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
            recurrence.StartsOn(new DateTime(SelectedStartYear, SelectedStartMonth + 1, SelectedStartDay, SelectedStartHours, SelectedStartMinutes, 0));
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
                recurrence.EndsOn(new DateTime(SelectedRecurrenceEndYear, SelectedRecurrenceEndMonth + 1, SelectedRecurrenceEndDay));
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
            return new DateTime(SelectedStartYear, SelectedStartMonth + 1, SelectedStartDay, SelectedStartHours, SelectedStartMinutes, 0);
        }

        private DateTime GetEndDateTime()
        {
            return new DateTime(SelectedEndYear, SelectedEndMonth + 1, SelectedEndDay, SelectedEndHours, SelectedEndMinutes, 0);
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
                DateTime start = new DateTime(SelectedStartYear, SelectedStartMonth, SelectedStartDay, SelectedStartHours, SelectedStartMinutes, 0);
                DateTime end = start.AddHours(DEFAULT_EVENT_LENGTH_IN_HOURS);

                SelectedEndYear = end.Year;
                SelectedEndMonth = end.Month;
                SelectedEndDay = end.Day;

                SelectedEndHours = end.Hour;
                SelectedEndMinutes = end.Minute;
            }
        }

        private void CleanInputText()
        {
            Title = String.Empty;
            Content = String.Empty;
            Location = String.Empty;
        }

        #endregion
    }
}