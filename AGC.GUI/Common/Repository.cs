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
        private DateTimePreferences dateTimePreferences = new DateTimePreferences().Load();
        private SortFilterPreferences sortFilterPreferences = new SortFilterPreferences().Load();
        private QuickEventsTemplates quickEventsTemplates = new QuickEventsTemplates().Load();

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

        public DateTimePreferences GetDateTimePreferences()
        {
            return dateTimePreferences;
        }

        public void SetDateTimePreferences(DateTimePreferences preferences)
        {
            dateTimePreferences = preferences;
        }

        public SortFilterPreferences GetSortFilterPreferences()
        {
            return sortFilterPreferences;
        }

        public void SetSortFilterPreferences(SortFilterPreferences preferences)
        {
            sortFilterPreferences = preferences;
        }

        public QuickEventsTemplates GetQuickEventsTemplates()
        {
            return quickEventsTemplates;
        }

        public void SetQuickEventsTemplates(QuickEventsTemplates templates)
        {
            quickEventsTemplates = templates;
        }
    }
}
