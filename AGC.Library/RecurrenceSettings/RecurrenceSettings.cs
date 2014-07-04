using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGC.Library
{
    public class RecurrenceSettings : IRecurrenceSettings
    {
        #region Constants

        private const string RECURRENCE = "RRULE:FREQ={0};INTERVAL={1};{2};{3};WKST=MO";
        private const string DATETIME_FORMAT = "yyyyMMddTHHmmssZ";

        private const string BYDAY = "BYDAY={0};";
        private const string BYMONTHDAY = "BYMONTHDAY={0};";
        private const string UNTIL = "UNTIL={0:" + DATETIME_FORMAT+ "};";
        private const string COUNT = "COUNT={0};";        

        private const string DAILY = "DAILY";
        private const string WEEKLY = "WEEKLY";
        private const string MONTHLY = "MONTHLY";
        private const string YEARLY = "YEARLY";

        #endregion

        #region Private variables

        private DateTime startDate = DateTime.Today;

        private string freq = DAILY;
        private int interval = 1;

        private bool monday = false;
        private bool tuesday = false;
        private bool wednesday = false;
        private bool thursday = false;
        private bool friday = false;
        private bool saturday = false;
        private bool sunday = false;

        private bool byDayOfMonth = false;
        private bool byDayOfWeek = false;
        
        private DateTime? endDate = null;
        private int occurrences = 0;
        private bool neverEnds = true;

        #endregion

        #region Constructor

        public RecurrenceSettings()
        {

        }

        public RecurrenceSettings(DateTime start)
        {
            startDate = start;
        }

        #endregion

        #region Public Methods

        public RecurrenceSettings StartsOn(DateTime start)
        {
            startDate = start;
            return this;
        }

        #region Frequency type

        public RecurrenceSettings Daily()
        {
            this.freq = DAILY;
            return this;
        }

        public RecurrenceSettings Weekly()
        {
            this.freq = WEEKLY;
            return this;
        }

        public RecurrenceSettings Monthly()
        {
            this.freq = MONTHLY;
            return this;
        }

        public RecurrenceSettings Yearly()
        {
            this.freq = YEARLY;
            return this;
        }

        #endregion

        public RecurrenceSettings Interval(int number)
        {
            interval = number;
            return this;
        }

        #region Weekly Optional Settings

        public RecurrenceSettings Monday()
        {
            monday = true;
            return this;
        }

        public RecurrenceSettings Tuesday()
        {
            tuesday = true;
            return this;
        }

        public RecurrenceSettings Wednesday()
        {
            wednesday = true;
            return this;
        }

        public RecurrenceSettings Thursday()
        {
            thursday = true;
            return this;
        }

        public RecurrenceSettings Friday()
        {
            friday = true;
            return this;
        }

        public RecurrenceSettings Saturday()
        {
            saturday = true;
            return this;
        }

        public RecurrenceSettings Sunday()
        {
            sunday = true;
            return this;
        }

        #endregion

        #region Month Optional Settings

        public RecurrenceSettings ByDayOfMonth()
        {
            byDayOfMonth = true;
            byDayOfWeek = false;
            return this;
        }

        public RecurrenceSettings ByDayOfWeek()
        {
            byDayOfMonth = false;
            byDayOfWeek = true;
            return this;
        }

        #endregion

        #region Exit Optional Settings

        public RecurrenceSettings EndsOn(DateTime end)
        {
            endDate = end;
            occurrences = 0;
            neverEnds = false;
            return this;
        }

        public RecurrenceSettings EndsAfter(int numberOfOccurrences)
        {
            occurrences = numberOfOccurrences;
            endDate = null;
            neverEnds = false;
            return this;
        }

        public RecurrenceSettings EndsNever()
        {
            neverEnds = true;
            endDate = null;
            occurrences = 0;
            return this;
        }

        #endregion

        public override string ToString()
        {
            switch (freq)
            {
                case DAILY:
                    return CreateRecurrenceString(freq, interval, String.Empty, GetEndConditionOptionalParameter());
                case WEEKLY:
                    return CreateRecurrenceString(freq, interval, GetWeekOptionalParameter(), GetEndConditionOptionalParameter());
                case MONTHLY:
                    return CreateRecurrenceString(freq, interval, GetMonthOptionalParameter(), GetEndConditionOptionalParameter());
                case YEARLY:
                    return CreateRecurrenceString(freq, interval, String.Empty, GetEndConditionOptionalParameter());
            }

            return String.Empty;
        }

        public string GetDayOfWeekFirstTwoLetters(DateTime time)
        {
            switch (time.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    return "MO";
                case DayOfWeek.Tuesday:
                    return "TU";
                case DayOfWeek.Wednesday:
                    return "WE";
                case DayOfWeek.Thursday:
                    return "TH";
                case DayOfWeek.Friday:
                    return "FR";
                case DayOfWeek.Saturday:
                    return "SA";
                case DayOfWeek.Sunday:
                    return "SU";
                default:
                    return String.Empty;
            }
        }

        #endregion

        #region Private Methods

        private string CreateRecurrenceString(String frequency, int interval, String optionalWeekOrMonthParameters, String endCondition)
        {
            return String.Format(RECURRENCE, frequency, interval, optionalWeekOrMonthParameters, endCondition);
        }

        private string GetWeekOptionalParameter()
        {
            string param = String.Empty;

            param = monday ? param += "MO," : param;
            param = tuesday ? param += "TU," : param;
            param = wednesday ? param += "WE," : param;
            param = thursday ? param += "TH," : param;
            param = friday ? param += "FR," : param;
            param = saturday ? param += "SA," : param;
            param = sunday ? param += "SU," : param;

            return param = String.IsNullOrEmpty(param) ? param : String.Format(BYDAY, param);
        }

        private string GetMonthOptionalParameter()
        {
            string param = String.Empty;

            param = byDayOfMonth ? String.Format(BYMONTHDAY, startDate.Day) : param;
            param = byDayOfWeek ? String.Format(BYDAY, GetMonthWeekdayNumber(startDate) + GetDayOfWeekFirstTwoLetters(startDate)) : param;

            return param;
        }

        private string GetEndConditionOptionalParameter()
        {
            string param = String.Empty;

            if (!neverEnds)
            {
                param = endDate != null ? String.Format(UNTIL, endDate) : param;
                param = occurrences > 0 ? String.Format(COUNT, occurrences) : param;
            }

            return param;
        }

        // Example if start date is 2nd friday of the Month, than method will return 2
        private int GetMonthWeekdayNumber(DateTime date)
        {
            int firstDayOfTheMonthWeekdayNumber = GetWeekdayNumber(date.AddDays(1 - date.Day));
            int startDayOfTheEventWeekdayNumber = GetWeekdayNumber(date);
            int startDayWeekNumber = date.GetWeekOfMonth();
            return startDayOfTheEventWeekdayNumber > firstDayOfTheMonthWeekdayNumber ? startDayWeekNumber : startDayWeekNumber - 1;
        }

        // returns 1 for Monday and 7 for Sunday
        private int GetWeekdayNumber(DateTime date)
        {
            return ((int)date.DayOfWeek == 0) ? 7 : (int)date.DayOfWeek;
        }

        #endregion
    }
}
