using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGC.Library
{
    public interface IGoogleCalendar
    {
        bool CreateEvent(CalendarEvent ev);

        bool AddQuickEvent(String eventText);

        bool DeleteEvent(CalendarEvent ev);

        bool DeleteSingleInstanceOfRecurringEvent(CalendarEvent ev);

        bool DeleteAllInstancesOfRecurringEvent(CalendarEvent ev);

        CalendarEventList GetAllEvents();

        void DeleteEvents(CalendarEventList evs);
    }
}
