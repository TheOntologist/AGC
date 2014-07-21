using AGC.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGC.GUI.Common
{
    public class Repository : IRepository
    {
        private CalendarEvent currentEvent;
        private CalendarEventUpdater eventUpdater = new CalendarEventUpdater();

        public CalendarEvent GetCurrentEvent()
        {
            return currentEvent;
        }

        public void SetCurrentEvent(CalendarEvent calEvent)
        {
            currentEvent = calEvent;
        }

        public CalendarEventUpdater GetEventUpdater()
        {
            return eventUpdater;
        }

        public void SetEventUpdater(CalendarEventUpdater calEventUpdater)
        {
            eventUpdater = calEventUpdater;
        }
    }
}
