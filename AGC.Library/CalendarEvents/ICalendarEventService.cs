using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGC.Library
{
    public interface ICalendarEventService
    {
        CalendarEventList GetEvents(IGoogleCalendar calendar, TimeIntervals period);

        CalendarEventList GetEvents(IGoogleCalendar calendar, DateTime start, DateTime end);

        CalendarEventList SearchEvents(IGoogleCalendar calendar, TimeIntervals period, String keyword);

        CalendarEventList FormatEventsDatesStringRepresentation(CalendarEventList allEvents, DateTimePreferences preferences);
    }
}
