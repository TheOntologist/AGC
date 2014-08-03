using AGC.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGC.GUI.Common
{
    public interface IRepository
    {
        CalendarEvent GetCurrentEvent();
        void SetCurrentEvent(CalendarEvent calEvent);

        CalendarEventUpdater GetEventUpdater();
        void SetEventUpdater(CalendarEventUpdater calEventUpdater);

        DateTimePreferences GetDateTimePreferences();
        void SetDateTimePreferences(DateTimePreferences preferences);

        SortFilterPreferences GetSortFilterPreferences();
        void SetSortFilterPreferences(SortFilterPreferences preferences);
    }
}
