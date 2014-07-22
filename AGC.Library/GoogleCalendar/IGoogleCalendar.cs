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

        bool UpdateEvent(CalendarEvent ev, GoogleCalendar.ActionType type);

        bool AddQuickEvent(String eventText);

        bool DeleteEvent(CalendarEvent ev, GoogleCalendar.ActionType type);

        CalendarEventList GetAllEvents();

        void DeleteEvents(CalendarEventList evs);

        RecurrenceSettings GetRecurrenceSettings(CalendarEvent ev);
    }
}
