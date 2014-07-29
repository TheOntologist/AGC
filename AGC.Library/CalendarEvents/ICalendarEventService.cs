using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGC.Library
{
    public interface ICalendarEventService
    {
        CalendarEventList GetEvents(CalendarEventList allEvents, TimeIntervals period);

        CalendarEventList GetEvents(CalendarEventList allEvents, DateTime start, DateTime end);

        CalendarEventList SearchEvents(CalendarEventList allEvents, String keyword);

        CalendarEventList FormatEventsDatesStringRepresentation(CalendarEventList allEvents, DateTimePreferences preferences);
    }
}
