using AGC.GUI.Common;
using AGC.Library;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Windows;

namespace AGC.GUI.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {
        #region Constants

        private const string DATE_FORMAT_DAY_FIRST = "day first";
        private const string DATE_FORMAT_MONTH_FIRST = "month first";

        private const string MONTH_FORMAT_SHORT_NAME = "short name";
        private const string MONTH_FORMAT_NAME = "name";
        private const string MONTH_FORMAT_NUMBER = "number";

        private const string HOUR_FORMAT_24 = "24-hour";
        private const string HOUR_FORMAT_AM_PM = "am/pm";

        private const string FIELD_SEPARATOR_SPACE = "space";
        private const string FIELD_SEPARATOR_UNDERSCORE = "_";
        private const string FIELD_SEPARATOR_SLASH = "/";
        private const string FIELD_SEPARATOR_POINT = ".";


        private const string MONTH_STR_FORMAT_SHORT_NAME = "MMM";
        private const string MONTH_STR_FORMAT_NAME = "MMMM";
        private const string MONTH_STR_FORMAT_NUMBER = "M";

        private const string TIME_STR_FORMAT_24 = "HH:mm";
        private const string TIME_STR_FORMAT_AM_PM = "h:mm tt";

        private const char FIELD_SEPARATOR_FORMAT_SPACE = ' ';
        private const char FIELD_SEPARATOR_FORMAT_UNDERSCORE = '_';
        private const char FIELD_SEPARATOR_FORMAT_SLASH = '/';
        private const char FIELD_SEPARATOR_FORMAT_POINT = '.';

        #endregion

        #region Private Properties

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IRepository repository;
        private DateTimePreferences dateTimePreferences;

        private static List<string> DATE_FORMAT_LIST = new List<string>(new string[] { DATE_FORMAT_DAY_FIRST, DATE_FORMAT_MONTH_FIRST });
        private static List<string> MONTH_FORMAT_LIST = new List<string>(new string[] { MONTH_FORMAT_SHORT_NAME, MONTH_FORMAT_NAME, MONTH_FORMAT_NUMBER });
        private static List<string> HOUR_FORMAT_LIST = new List<string>(new string[] { HOUR_FORMAT_24, HOUR_FORMAT_AM_PM });
        private static List<string> FIELD_SEPARATOR_LIST = new List<string>(new string[] { FIELD_SEPARATOR_SPACE, FIELD_SEPARATOR_UNDERSCORE, FIELD_SEPARATOR_SLASH, FIELD_SEPARATOR_POINT });

        #endregion

        #region Commands

        public RelayCommand SaveDateTimePreferencesCommand { get; private set; }

        #endregion

        #region Constructor

        public SettingsViewModel(IRepository commonRepository)
        {
            repository = commonRepository;
            dateTimePreferences = repository.GetDateTimePreferences();
            LoadDateTimePreferences();
            
            SaveDateTimePreferencesCommand = new RelayCommand(SaveDateTimePreferences);
        }

        #endregion

        #region Public Properties

        #region Date-Time preferences

        public const string DateFormatListPropertyName = "DateFormatList";
        private List<string> _dateFormatList = DATE_FORMAT_LIST;
        public List<string> DateFormatList
        {
            get
            {
                return _dateFormatList;
            }

            set
            {
                if (_dateFormatList == value)
                {
                    return;
                }

                RaisePropertyChanging(DateFormatListPropertyName);
                _dateFormatList = value;
                RaisePropertyChanged(DateFormatListPropertyName);
            }
        }

        public const string SelectedDateFormatPropertyName = "SelectedDateFormat";
        private string _selectedDateFormat = DATE_FORMAT_DAY_FIRST;
        public string SelectedDateFormat
        {
            get
            {
                return _selectedDateFormat;
            }

            set
            {
                if (_selectedDateFormat == value)
                {
                    return;
                }

                RaisePropertyChanging(SelectedDateFormatPropertyName);
                _selectedDateFormat = value;
                RaisePropertyChanged(SelectedDateFormatPropertyName);
            }
        }

        public const string MonthFormatListPropertyName = "MonthFormatList";
        private List<string> _monthFormatList = MONTH_FORMAT_LIST;
        public List<string> MonthFormatList
        {
            get
            {
                return _monthFormatList;
            }

            set
            {
                if (_monthFormatList == value)
                {
                    return;
                }

                RaisePropertyChanging(MonthFormatListPropertyName);
                _monthFormatList = value;
                RaisePropertyChanged(MonthFormatListPropertyName);
            }
        }

        public const string SelectedMonthFormatPropertyName = "SelectedMonthFormat";
        private string _selectedMonthFormat = MONTH_FORMAT_SHORT_NAME;
        public string SelectedMonthFormat
        {
            get
            {
                return _selectedMonthFormat;
            }

            set
            {
                if (_selectedMonthFormat == value)
                {
                    return;
                }

                RaisePropertyChanging(SelectedMonthFormatPropertyName);
                _selectedMonthFormat = value;
                RaisePropertyChanged(SelectedMonthFormatPropertyName);
            }
        }

        public const string TimeFormatListPropertyName = "TimeFormatList";
        private List<string> _timeFormatList = HOUR_FORMAT_LIST;
        public List<string> TimeFormatList
        {
            get
            {
                return _timeFormatList;
            }

            set
            {
                if (_timeFormatList == value)
                {
                    return;
                }

                RaisePropertyChanging(TimeFormatListPropertyName);
                _timeFormatList = value;
                RaisePropertyChanged(TimeFormatListPropertyName);
            }
        }

        public const string SelectedTimeFormatPropertyName = "SelectedTimeFormat";
        private string _selectedTimeFormat = HOUR_FORMAT_24;
        public string SelectedTimeFormat
        {
            get
            {
                return _selectedTimeFormat;
            }

            set
            {
                if (_selectedTimeFormat == value)
                {
                    return;
                }

                RaisePropertyChanging(SelectedTimeFormatPropertyName);
                _selectedTimeFormat = value;
                RaisePropertyChanged(SelectedTimeFormatPropertyName);
            }
        }

        public const string FieldSeparatorListPropertyName = "FieldSeparatorList";
        private List<string> _fieldSeparatorList = FIELD_SEPARATOR_LIST;
        public List<string> FieldSeparatorList
        {
            get
            {
                return _fieldSeparatorList;
            }

            set
            {
                if (_fieldSeparatorList == value)
                {
                    return;
                }

                RaisePropertyChanging(FieldSeparatorListPropertyName);
                _fieldSeparatorList = value;
                RaisePropertyChanged(FieldSeparatorListPropertyName);
            }
        }

        public const string SelectedFieldSeparatorPropertyName = "SelectedFieldSeparator";
        private string _selectedFieldSeparator = FIELD_SEPARATOR_SPACE;
        public string SelectedFieldSeparator
        {
            get
            {
                return _selectedFieldSeparator;
            }

            set
            {
                if (_selectedFieldSeparator == value)
                {
                    return;
                }

                RaisePropertyChanging(SelectedFieldSeparatorPropertyName);
                _selectedFieldSeparator = value;
                RaisePropertyChanged(SelectedFieldSeparatorPropertyName);
            }
        }

        public const string ShowEndTimePropertyName = "ShowEndTime";
        private bool _showEndTime = true;
        public bool ShowEndTime
        {
            get
            {
                return _showEndTime;
            }

            set
            {
                if (_showEndTime == value)
                {
                    return;
                }

                RaisePropertyChanging(ShowEndTimePropertyName);
                _showEndTime = value;
                RaisePropertyChanged(ShowEndTimePropertyName);
            }
        }

        public const string ShowYearPropertyName = "ShowYear";
        private bool _showYear = false;
        public bool ShowYear
        {
            get
            {
                return _showYear;
            }

            set
            {
                if (_showYear == value)
                {
                    return;
                }

                RaisePropertyChanging(ShowYearPropertyName);
                _showYear = value;
                RaisePropertyChanged(ShowYearPropertyName);
            }
        }

        public const string DoNotShowTimeAndForFullDayEventsPropertyName = "DoNotShowTimeAndForFullDayEvents";
        private bool _doNotShowTimeAndForFullDayEvents = true;
        public bool DoNotShowTimeAndForFullDayEvents
        {
            get
            {
                return _doNotShowTimeAndForFullDayEvents;
            }

            set
            {
                if (_doNotShowTimeAndForFullDayEvents == value)
                {
                    return;
                }

                RaisePropertyChanging(DoNotShowTimeAndForFullDayEventsPropertyName);
                _doNotShowTimeAndForFullDayEvents = value;
                RaisePropertyChanged(DoNotShowTimeAndForFullDayEventsPropertyName);
            }
        }

        public const string DoNotShowMonthForCurrentMonthEventsPropertyName = "DoNotShowMonthForCurrentMonthEvents";
        private bool _doNotShowMonthForCurrentMonthEvents = true;
        public bool DoNotShowMonthForCurrentMonthEvents
        {
            get
            {
                return _doNotShowMonthForCurrentMonthEvents;
            }

            set
            {
                if (_doNotShowMonthForCurrentMonthEvents == value)
                {
                    return;
                }

                RaisePropertyChanging(DoNotShowMonthForCurrentMonthEventsPropertyName);
                _doNotShowMonthForCurrentMonthEvents = value;
                RaisePropertyChanged(DoNotShowMonthForCurrentMonthEventsPropertyName);
            }
        }

        #endregion

        #endregion

        #region Private Methods

        private void LoadDateTimePreferences()
        {
            SelectedDateFormat = dateTimePreferences.DateFormatUS ? DATE_FORMAT_MONTH_FIRST : DATE_FORMAT_DAY_FIRST;

            switch (dateTimePreferences.Month)
            {
                case MONTH_STR_FORMAT_SHORT_NAME:
                    {
                        SelectedMonthFormat = MONTH_FORMAT_SHORT_NAME;
                        break;
                    }
                case MONTH_STR_FORMAT_NAME:
                    {
                        SelectedMonthFormat = MONTH_FORMAT_NAME;
                        break;
                    }
                case MONTH_STR_FORMAT_NUMBER:
                    {
                        SelectedMonthFormat = MONTH_FORMAT_NUMBER;
                        break;
                    }
            }

            switch (dateTimePreferences.Time)
            {
                case TIME_STR_FORMAT_24:
                    {
                        SelectedTimeFormat = HOUR_FORMAT_24;
                        break;
                    }
                case TIME_STR_FORMAT_AM_PM:
                    {
                        SelectedTimeFormat = HOUR_FORMAT_AM_PM;
                        break;
                    }
            }

            switch (dateTimePreferences.Delimeter)
            {
                case FIELD_SEPARATOR_FORMAT_SPACE:
                    {
                        SelectedFieldSeparator = FIELD_SEPARATOR_SPACE;
                        break;
                    }
                case FIELD_SEPARATOR_FORMAT_UNDERSCORE:
                    {
                        SelectedFieldSeparator = FIELD_SEPARATOR_UNDERSCORE;
                        break;
                    }
                case FIELD_SEPARATOR_FORMAT_SLASH:
                    {
                        SelectedFieldSeparator = FIELD_SEPARATOR_SLASH;
                        break;
                    }
                case FIELD_SEPARATOR_FORMAT_POINT:
                    {
                        SelectedFieldSeparator = FIELD_SEPARATOR_POINT;
                        break;
                    }
            }

            ShowEndTime = !dateTimePreferences.HideEndDate;
            ShowYear = !dateTimePreferences.HideYear;
            DoNotShowTimeAndForFullDayEvents = dateTimePreferences.HideStartTimeAndEndDateIfFullDay;
            DoNotShowMonthForCurrentMonthEvents = dateTimePreferences.HideMonthIfCurrent;
        }

        private void SaveDateTimePreferences()
        {
            dateTimePreferences.DateFormatUS = SelectedDateFormat == DATE_FORMAT_MONTH_FIRST ? true : false;

            switch (SelectedMonthFormat)
            {
                case MONTH_FORMAT_SHORT_NAME:
                    {
                        dateTimePreferences.Month = MONTH_STR_FORMAT_SHORT_NAME;
                        break;
                    }
                case MONTH_FORMAT_NAME:
                    {
                        dateTimePreferences.Month = MONTH_STR_FORMAT_NAME;
                        break;
                    }
                case MONTH_FORMAT_NUMBER:
                    {
                        dateTimePreferences.Month = MONTH_STR_FORMAT_NUMBER;
                        break;
                    }
            }

            switch (SelectedTimeFormat)
            {
                case HOUR_FORMAT_24:
                    {
                        dateTimePreferences.Time = TIME_STR_FORMAT_24;
                        break;
                    }
                case HOUR_FORMAT_AM_PM:
                    {
                        dateTimePreferences.Time = TIME_STR_FORMAT_AM_PM;
                        break;
                    }
            }

            switch (SelectedFieldSeparator)
            {
                case FIELD_SEPARATOR_SPACE:
                    {
                        dateTimePreferences.Delimeter = FIELD_SEPARATOR_FORMAT_SPACE;
                        break;
                    }
                case FIELD_SEPARATOR_UNDERSCORE:
                    {
                        dateTimePreferences.Delimeter = FIELD_SEPARATOR_FORMAT_UNDERSCORE;
                        break;
                    }
                case FIELD_SEPARATOR_SLASH:
                    {
                        dateTimePreferences.Delimeter = FIELD_SEPARATOR_FORMAT_SLASH;
                        break;
                    }
                case FIELD_SEPARATOR_POINT:
                    {
                        dateTimePreferences.Delimeter = FIELD_SEPARATOR_FORMAT_POINT;
                        break;
                    }
            }

            dateTimePreferences.HideEndDate = !ShowEndTime;
            dateTimePreferences.HideYear = !ShowYear;
            dateTimePreferences.HideStartTimeAndEndDateIfFullDay = DoNotShowTimeAndForFullDayEvents;
            dateTimePreferences.HideMonthIfCurrent = DoNotShowMonthForCurrentMonthEvents;

            if (dateTimePreferences.Save())
            {
                MessageBox.Show(Application.Current.MainWindow, "Saved", "Information", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                repository.SetDateTimePreferences(dateTimePreferences);
            }
            else
            {
                MessageBox.Show(Application.Current.MainWindow, "Failed to save Date-Time preferences. Please check log file for a detailed information about the error.", "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
        }

        #endregion
    }
}