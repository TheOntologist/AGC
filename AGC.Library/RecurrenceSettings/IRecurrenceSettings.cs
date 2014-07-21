using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGC.Library
{
    public interface IRecurrenceSettings
    {
        RecurrenceSettings SetRecurrence(DateTime start, string rrule);

        RecurrenceSettings StartsOn(DateTime start);

        RecurrenceSettings Daily();
        RecurrenceSettings Weekly();
        RecurrenceSettings Monthly();
        RecurrenceSettings Yearly();

        RecurrenceSettings Interval(int number);

        RecurrenceSettings Monday();
        RecurrenceSettings Tuesday();
        RecurrenceSettings Wednesday();
        RecurrenceSettings Thursday();
        RecurrenceSettings Friday();
        RecurrenceSettings Saturday();
        RecurrenceSettings Sunday();

        RecurrenceSettings ByDayOfMonth();
        RecurrenceSettings ByDayOfWeek();

        RecurrenceSettings EndsOn(DateTime end);
        RecurrenceSettings EndsAfter(int numberOfOccurrences);
        RecurrenceSettings EndsNever();

        bool IsRepeatsDaily();
        bool IsRepeatsWeeky();
        bool IsRepeatsMonthly();
        bool IsRepeatsYearly();

        int GetInterval();

        bool IsRepeatsOnMonday();
        bool IsRepeatsOnTuesday();
        bool IsRepeatsOnWednesday();
        bool IsRepeatsOnThursday();
        bool IsRepeatsOnFriday();
        bool IsRepeatsOnSaturday();
        bool IsRepeatsOnSunday();

        bool IsRepeatsByDayOfMonth();
        bool IsRepeatsByDayOfWeek();

        bool IsEndsOnSpecifiedDate();
        bool IsEndsAfterSpecifiedNumberOfOccurences();
        bool IsEndsNever();

        DateTime? EndDate();        
        int Count();        

        string ToString();

        string GetDayOfWeekFirstTwoLetters(DateTime time);

        RecurrenceSettings Clear();
    }
}
