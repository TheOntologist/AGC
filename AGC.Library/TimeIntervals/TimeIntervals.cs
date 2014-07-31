using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGC.Library
{
    public class TimeIntervals : ITimeIntervals
    {
        #region Constants

        private const bool FirstDayOfWeekIsMonday = true;    

        #endregion

        #region Private Variables

        private DateTime start;
        private DateTime end;
        private static bool TodayIsSunday = (int)DateTime.Today.DayOfWeek == 0;
        
        #endregion

        #region Public Fields

        public DateTime Start { get { return start; } }

        public DateTime End { get { return end; } }

        public enum PeriodType
        {
            SingleMonth,
            AllMonths,
            InterveningMonths
        }

        #endregion

        #region Constructor

        public TimeIntervals()
        {
            Today();
        }

        #endregion

        #region Public Methods

        public TimeIntervals Today()
        {
            start = DateTime.Today;
            end = DateTime.Today.AddDays(1).AddSeconds(-1);
            return this;
        }

        public TimeIntervals Tomorrow()
        {
            start = DateTime.Today.AddDays(1);
            end = DateTime.Today.AddDays(2).AddSeconds(-1);
            return this;
        }

        public TimeIntervals ThisWeek()
        {
            if (FirstDayOfWeekIsMonday && TodayIsSunday)
            {
                start = DateTime.Today.AddDays(-6);
            }
            else
            {
                start = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
            }
            
            end = start.AddDays(7).AddSeconds(-1);
            return this;
        }

        public TimeIntervals NextWeek()
        {
            if (FirstDayOfWeekIsMonday && TodayIsSunday)
            {
                start = DateTime.Today.AddDays(1);
            }
            else
            {
                start = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek).AddDays(7);
            }

            end = start.AddDays(7).AddSeconds(-1);
            return this;
        }

        public TimeIntervals ThisMonth()
        {
            MonthsPeriod(DateTime.Today.Month, PeriodType.SingleMonth);
            return this;
        }

        public TimeIntervals NextMonth()
        {
            MonthsPeriod(DateTime.Today.AddMonths(1).Month, PeriodType.SingleMonth);
            return this;
        }

        public TimeIntervals MonthsPeriod(int endMonth, PeriodType periodType)
        {
            // Add 1 Month, because month lists starts with 0, end month should be in range 1-12
            endMonth++;

            int currentMonth = DateTime.Today.Month;
            int numberOfMonthToAdd = endMonth > currentMonth ? endMonth - currentMonth : (12 - currentMonth) + endMonth;

            start = DateTime.Today.AddDays(1 - DateTime.Today.Day);
            end = start.AddMonths(numberOfMonthToAdd);

            switch (periodType)
            {
                case PeriodType.SingleMonth:
                    {
                        start = end.AddDays(1 - end.Day).AddMonths(-1);
                        break;
                    }
                case PeriodType.AllMonths:
                    {
                        break;
                    }
                case PeriodType.InterveningMonths:
                    {
                        start = start.AddMonths(1);
                        end = end.AddMonths(-1);
                        break;
                    }
            }

            if (start.Month == DateTime.Today.Month)
            {
                start = DateTime.Today;
            }

            end = end.AddSeconds(-1);
            
            return this;
        }

        public TimeIntervals All()
        {
            start = DateTime.Today.AddYears(-4);
            end = DateTime.Today.AddYears(4);
            return this;
        }

        public void WriteConsoleLog()
        {
            Console.WriteLine("START: {0:dd.MM.yy HH:mm:ss zzz} END: {1:dd.MM.yy HH:mm:ss zzz}", start, end); 
        }

        #endregion
    }
}
