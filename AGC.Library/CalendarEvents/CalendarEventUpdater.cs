using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGC.Library
{
    public class CalendarEventUpdater
    {
        public CalendarEventUpdater()
        {
            this.Type = GoogleCalendar.ActionType.none;
        }

        public CalendarEventUpdater(GoogleCalendar.ActionType UpdateType, CalendarEvent calEvent, RecurrenceSettings calRecurrence)
        {
            this.Type = UpdateType;
            this.CalendarEvent = calEvent;
            this.Recurrence = calRecurrence;
            this.Reminder = 10;
        }

        public CalendarEventUpdater(GoogleCalendar.ActionType UpdateType, CalendarEvent calEvent)
        {
            this.Type = UpdateType;
            this.CalendarEvent = calEvent;
            this.Reminder = 10;
        }

        public GoogleCalendar.ActionType Type { get; set; }

        public CalendarEvent CalendarEvent { get; set; }

        public RecurrenceSettings Recurrence { get; set; }

        public int Reminder { get; set; }
    }
}
