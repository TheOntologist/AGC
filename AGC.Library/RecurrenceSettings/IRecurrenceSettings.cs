using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGC.Library
{
    public interface IRecurrenceSettings
    {
        RecurrenceSettings StartsOn(DateTime start);

        #region Frequency type

        RecurrenceSettings Daily();

        RecurrenceSettings Weekly();

        RecurrenceSettings Monthly();

        RecurrenceSettings Yearly();

        #endregion

        RecurrenceSettings Interval(int number);

        #region Weekly Optional Settings

        RecurrenceSettings Monday();

        RecurrenceSettings Tuesday();

        RecurrenceSettings Wednesday();

        RecurrenceSettings Thursday();

        RecurrenceSettings Friday();

        RecurrenceSettings Saturday();

        RecurrenceSettings Sunday();

        #endregion

        #region Month Optional Settings

        RecurrenceSettings ByDayOfMonth();

        RecurrenceSettings ByDayOfWeek();

        #endregion

        #region Exit Optional Settings

        RecurrenceSettings EndsOn(DateTime end);

        RecurrenceSettings EndsAfter(int numberOfOccurrences);

        RecurrenceSettings EndsNever();

        #endregion

        string ToString();

        string GetDayOfWeekFirstTwoLetters(DateTime time);
    }
}
