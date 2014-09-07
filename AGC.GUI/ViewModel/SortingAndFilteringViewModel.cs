using AGC.GUI.Common;
using AGC.Library;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Windows;

namespace AGC.GUI.ViewModel
{
    public class SortingAndFilteringViewModel : ViewModelBase
    {
        #region Constants

        private const string START_DATE = "start date";
        private const string STATUS = "status";
        private const string TITLE = "title";
        private const string LOCATION = "location";
        private const string CONTENT = "content";
        private const string END_DATE = "end date";

        private const string CONFIRMED = "confirmed";
        private const string TENTATIVE = "tentative";

        private const int DEFAULT_TIME_FILTER_PERIOD_IN_HOURS = 1;

        #endregion

        #region Private Properties

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IRepository repository;
        private readonly IMessanger messanger;
        private SortFilterPreferences sortFilterPreferences;

        private static List<string> SORT_BY_PARAM_LIST = new List<string>(new string[] { START_DATE, STATUS, TITLE, LOCATION, CONTENT, END_DATE });
        private static List<string> STATUS_LIST = new List<string>(new string[] { CONFIRMED, TENTATIVE });

        #endregion

        #region Commands

        public RelayCommand SavePreferencesAsDefaultCommand { get; private set; }
        public RelayCommand SortAndFilterCommand { get; private set; }
        public RelayCommand CancelSortAndFilterCommand { get; private set; }

        #endregion

        #region Constructor

        public SortingAndFilteringViewModel(IRepository commonRepository, IMessanger commonMessanger)
        {
            repository = commonRepository;
            messanger = commonMessanger;
            sortFilterPreferences = repository.GetSortFilterPreferences();
            LoadSortFilterPreferences();

            SavePreferencesAsDefaultCommand = new RelayCommand(SaveAsDefault);
            SortAndFilterCommand = new RelayCommand(SortAndFilter);
            CancelSortAndFilterCommand = new RelayCommand(CancelSortAndFilter);
        }

        #endregion

        #region Public Properties

        public const string EnableSortingPropertyName = "EnableSorting";
        private bool _enableSorting = false;
        public bool EnableSorting
        {
            get
            {
                return _enableSorting;
            }

            set
            {
                if (_enableSorting == value)
                {
                    return;
                }

                RaisePropertyChanging(EnableSortingPropertyName);
                _enableSorting = value;
                RaisePropertyChanged(EnableSortingPropertyName);
            }
        }

        public const string EnableTimeFilterPropertyName = "EnableTimeFilter";
        private bool _enableTimeFilter = false;
        public bool EnableTimeFilter
        {
            get
            {
                return _enableTimeFilter;
            }

            set
            {
                if (_enableTimeFilter == value)
                {
                    return;
                }

                RaisePropertyChanging(EnableTimeFilterPropertyName);
                _enableTimeFilter = value;
                RaisePropertyChanged(EnableTimeFilterPropertyName);
            }
        }

        public const string EnableDayOfWeekFilterPropertyName = "EnableDayOfWeekFilter";
        private bool _enableDayOfWeekFilter = false;
        public bool EnableDayOfWeekFilter
        {
            get
            {
                return _enableDayOfWeekFilter;
            }

            set
            {
                if (_enableDayOfWeekFilter == value)
                {
                    return;
                }

                RaisePropertyChanging(EnableDayOfWeekFilterPropertyName);
                _enableDayOfWeekFilter = value;
                RaisePropertyChanged(EnableDayOfWeekFilterPropertyName);
            }
        }

        public const string EnableStatusFilterPropertyName = "EnableStatusFilter";
        private bool _enableStatusFilter = false;
        public bool EnableStatusFilter
        {
            get
            {
                return _enableStatusFilter;
            }

            set
            {
                if (_enableStatusFilter == value)
                {
                    return;
                }

                RaisePropertyChanging(EnableStatusFilterPropertyName);
                _enableStatusFilter = value;
                RaisePropertyChanged(EnableStatusFilterPropertyName);
            }
        }

        public const string SortByParamListPropertyName = "SortByParamList";
        private List<string> _sortByParamList = SORT_BY_PARAM_LIST;
        public List<string> SortByParamList
        {
            get
            {
                return _sortByParamList;
            }

            set
            {
                if (_sortByParamList == value)
                {
                    return;
                }

                RaisePropertyChanging(SortByParamListPropertyName);
                _sortByParamList = value;
                RaisePropertyChanged(SortByParamListPropertyName);
            }
        }

        public const string SelectedSortByParamPropertyName = "SelectedSortByParam";
        private string _selectedSortByParam = START_DATE;
        public string SelectedSortByParam
        {
            get
            {
                return _selectedSortByParam;
            }

            set
            {
                if (_selectedSortByParam == value)
                {
                    return;
                }

                RaisePropertyChanging(SelectedSortByParamPropertyName);
                _selectedSortByParam = value;
                RaisePropertyChanged(SelectedSortByParamPropertyName);
            }
        }

        public const string SortOrderAscendingPropertyName = "SortOrderAscending";
        private bool _sortOrderAscending = true;
        public bool SortOrderAscending
        {
            get
            {
                return _sortOrderAscending;
            }

            set
            {
                if (_sortOrderAscending == value)
                {
                    return;
                }

                RaisePropertyChanging(SortOrderAscendingPropertyName);
                _sortOrderAscending = value;
                RaisePropertyChanged(SortOrderAscendingPropertyName);
            }
        }

        public const string FromTimeHoursPropertyName = "FromTimeHours";
        private int _fromTimeHours = 9;
        public int FromTimeHours
        {
            get
            {
                return _fromTimeHours;
            }

            set
            {
                if (_fromTimeHours == value)
                {
                    return;
                }

                RaisePropertyChanging(FromTimeHoursPropertyName);
                _fromTimeHours = value;
                RaisePropertyChanged(FromTimeHoursPropertyName);
                CalculateEndTimeFromStartTime();
            }
        }

        public const string FromTimeMinutesPropertyName = "FromTimeMinutes";
        private int _fromTimeMinutes = 0;
        public int FromTimeMinutes
        {
            get
            {
                return _fromTimeMinutes;
            }

            set
            {
                if (_fromTimeMinutes == value)
                {
                    return;
                }

                RaisePropertyChanging(FromTimeMinutesPropertyName);
                _fromTimeMinutes = value;
                RaisePropertyChanged(FromTimeMinutesPropertyName);
                CalculateEndTimeFromStartTime();
            }
        }

        public const string ToTimeHoursPropertyName = "ToTimeHours";
        private int _toTimeHours = 10;
        public int ToTimeHours
        {
            get
            {
                return _toTimeHours;
            }

            set
            {
                if (_toTimeHours == value)
                {
                    return;
                }

                RaisePropertyChanging(ToTimeHoursPropertyName);
                _toTimeHours = value;
                RaisePropertyChanged(ToTimeHoursPropertyName);
            }
        }

        public const string ToTimeMinutesPropertyName = "ToTimeMinutes";
        private int _toTimeMinutes = 0;
        public int ToTimeMinutes
        {
            get
            {
                return _toTimeMinutes;
            }

            set
            {
                if (_toTimeMinutes == value)
                {
                    return;
                }

                RaisePropertyChanging(ToTimeMinutesPropertyName);
                _toTimeMinutes = value;
                RaisePropertyChanged(ToTimeMinutesPropertyName);
            }
        }

        public const string SelectedDayOfWeekPropertyName = "SelectedDayOfWeek";
        private string _selectedDayOfWeek = "WE";
        public string SelectedDayOfWeek
        {
            get
            {
                return _selectedDayOfWeek;
            }

            set
            {
                if (_selectedDayOfWeek == value)
                {
                    return;
                }

                RaisePropertyChanging(SelectedDayOfWeekPropertyName);
                _selectedDayOfWeek = value;
                RaisePropertyChanged(SelectedDayOfWeekPropertyName);
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

        public const string StatusListPropertyName = "StatusList";
        private List<string> _statusList = STATUS_LIST;
        public List<string> StatusList
        {
            get
            {
                return _statusList;
            }

            set
            {
                if (_statusList == value)
                {
                    return;
                }

                RaisePropertyChanging(StatusListPropertyName);
                _statusList = value;
                RaisePropertyChanged(StatusListPropertyName);
            }
        }

        public const string SelectedStatusPropertyName = "SelectedStatus";
        private string _selectedStatus = CONFIRMED;
        public string SelectedStatus
        {
            get
            {
                return _selectedStatus;
            }

            set
            {
                if (_selectedStatus == value)
                {
                    return;
                }

                RaisePropertyChanging(SelectedStatusPropertyName);
                _selectedStatus = value;
                RaisePropertyChanged(SelectedStatusPropertyName);
            }
        }

        #endregion

        #region Private Methods

        private void CalculateEndTimeFromStartTime()
        {
            DateTime start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, FromTimeHours, FromTimeMinutes, 0);
            DateTime end = start.AddHours(DEFAULT_TIME_FILTER_PERIOD_IN_HOURS);

            ToTimeHours = end.Hour;
            ToTimeMinutes = end.Minute;
        }


        private void LoadSortFilterPreferences()
        {
            EnableSorting = sortFilterPreferences.EnableSorting;
            EnableTimeFilter = sortFilterPreferences.EnableTimeFilter;
            EnableDayOfWeekFilter = sortFilterPreferences.EnableDayOfWeekFilter;
            EnableStatusFilter = sortFilterPreferences.EnableStatusFilter;

            switch(sortFilterPreferences.SortParam)
            {
                case SortFilterPreferences.SortBy.start:
                    {
                        SelectedSortByParam = START_DATE;
                        break;
                    }
                case SortFilterPreferences.SortBy.status:
                    {
                        SelectedSortByParam = STATUS;
                        break;
                    }
                case SortFilterPreferences.SortBy.title:
                    {
                        SelectedSortByParam = TITLE;
                        break;
                    }
                case SortFilterPreferences.SortBy.location:
                    {
                        SelectedSortByParam = LOCATION;
                        break;
                    }
                case SortFilterPreferences.SortBy.content:
                    {
                        SelectedSortByParam = CONTENT;
                        break;
                    }
                case SortFilterPreferences.SortBy.end:
                    {
                        SelectedSortByParam = END_DATE;
                        break;
                    }
            }

            SortOrderAscending = sortFilterPreferences.SortOrderAscending;

            FromTimeHours = sortFilterPreferences.TimeInMinutesMin / 60;
            FromTimeMinutes = sortFilterPreferences.TimeInMinutesMin % 60;

            ToTimeHours = sortFilterPreferences.TimeInMinutesMax / 60;
            ToTimeMinutes = sortFilterPreferences.TimeInMinutesMax % 60;

            Monday = sortFilterPreferences.Monday;
            Tuesday = sortFilterPreferences.Tuesday;
            Wednesday = sortFilterPreferences.Wednesday;
            Thursday = sortFilterPreferences.Thursday;
            Friday = sortFilterPreferences.Friday;
            Saturday = sortFilterPreferences.Saturday;
            Sunday = sortFilterPreferences.Sunday;

            SelectedStatus = sortFilterPreferences.ShowConfirmedOnly ? CONFIRMED : TENTATIVE;
        }

        private void SaveSortFilterPreferences()
        {
            sortFilterPreferences.EnableSorting = EnableSorting;
            sortFilterPreferences.EnableTimeFilter = EnableTimeFilter;
            sortFilterPreferences.EnableDayOfWeekFilter = EnableDayOfWeekFilter;
            sortFilterPreferences.EnableStatusFilter = EnableStatusFilter;

            switch(SelectedSortByParam)
            {
                case START_DATE:
                    {
                        sortFilterPreferences.SortParam = SortFilterPreferences.SortBy.start;
                        break;
                    }
                case STATUS:
                    {
                        sortFilterPreferences.SortParam = SortFilterPreferences.SortBy.status;
                        break;
                    }
                case TITLE:
                    {
                        sortFilterPreferences.SortParam = SortFilterPreferences.SortBy.title;
                        break;
                    }
                case LOCATION:
                    {
                        sortFilterPreferences.SortParam = SortFilterPreferences.SortBy.location;
                        break;
                    }
                case CONTENT:
                    {
                        sortFilterPreferences.SortParam = SortFilterPreferences.SortBy.content;
                        break;
                    }
                case END_DATE:
                    {
                        sortFilterPreferences.SortParam = SortFilterPreferences.SortBy.end;
                        break;
                    }
            }

            sortFilterPreferences.SortOrderAscending = SortOrderAscending;

            sortFilterPreferences.TimeInMinutesMin = FromTimeHours * 60 + FromTimeMinutes;
            sortFilterPreferences.TimeInMinutesMax = ToTimeHours * 60 + ToTimeMinutes;

            sortFilterPreferences.Monday = Monday;
            sortFilterPreferences.Tuesday = Tuesday;
            sortFilterPreferences.Wednesday = Wednesday;
            sortFilterPreferences.Thursday = Thursday;
            sortFilterPreferences.Friday = Friday;
            sortFilterPreferences.Saturday = Saturday;
            sortFilterPreferences.Sunday = Sunday;

            sortFilterPreferences.ShowConfirmedOnly = SelectedStatus == CONFIRMED ? true : false;
        }

        private void SaveAsDefault()
        {
            SaveSortFilterPreferences();

            if (sortFilterPreferences.Save())
            {
                messanger.Success("Saved", false);
                repository.SetSortFilterPreferences(sortFilterPreferences);
            }
            else
            {
                messanger.Error("Failed to save Sorting and Filtering preferences. Please check log file for a detailed information about the error.", false);
            }

            sortFilterPreferences.Enable = true;
            repository.SetSortFilterPreferences(sortFilterPreferences);
            CloseWindow();
        }

        private void SortAndFilter()
        {
            SaveSortFilterPreferences();
            sortFilterPreferences.Enable = true;
            repository.SetSortFilterPreferences(sortFilterPreferences);
            CloseWindow();
        }

        private void CancelSortAndFilter()
        {
            sortFilterPreferences.Enable = false;
            repository.SetSortFilterPreferences(sortFilterPreferences);
            CloseWindow();
        }

        private void CloseWindow()
        {
            Application.Current.Windows[1].Close();
        }
        #endregion
    }
}