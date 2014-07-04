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
        private static DateTime BaseDate = DateTime.Today;
        private static bool TodayIsSunday = (int)BaseDate.DayOfWeek == 0;
        
        #endregion

        #region Public Fields

        public DateTime Start { get { return start; } }

        public DateTime End { get { return end; } }

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
            start = BaseDate;
            end = BaseDate.AddDays(1).AddSeconds(-1);
            return this;
        }

        public TimeIntervals Tomorrow()
        {
            start = BaseDate.AddDays(1);
            end = BaseDate.AddDays(2).AddSeconds(-1);
            return this;
        }

        public TimeIntervals ThisWeek()
        {
            if (FirstDayOfWeekIsMonday && TodayIsSunday)
            {
                start = BaseDate.AddDays(-6);
            }
            else
            {
                start = BaseDate.AddDays(-(int)BaseDate.DayOfWeek);
            }
            
            end = start.AddDays(7).AddSeconds(-1);
            return this;
        }

        public TimeIntervals NextWeek()
        {
            if (FirstDayOfWeekIsMonday && TodayIsSunday)
            {
                start = BaseDate.AddDays(1);
            }
            else
            {
                start = BaseDate.AddDays(-(int)BaseDate.DayOfWeek).AddDays(7);
            }

            end = start.AddDays(7).AddSeconds(-1);
            return this;
        }

        public TimeIntervals ThisMonth()
        {
            NextNMonth(0, true);
            return this;
        }

        public TimeIntervals NextMonth()
        {
            NextNMonth(1, true);
            return this;
        }

        public TimeIntervals NextNMonth(int numberOfMonth, bool getSingleMonth)
        {
            numberOfMonth++;

            start = BaseDate.AddDays(1 - BaseDate.Day);
            end = start.AddMonths(numberOfMonth);

            if (getSingleMonth)
                start = end.AddDays(1 - end.Day).AddMonths(-1);

            end = end.AddSeconds(-1);

            return this;
        }

        public void WriteConsoleLog()
        {
            Console.WriteLine("START: {0:dd.MM.yy HH:mm:ss zzz} END: {1:dd.MM.yy HH:mm:ss zzz}", start, end); 
        }

        #endregion
    }
}
