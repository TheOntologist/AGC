using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGC.Library
{
    public interface ISortFilterPreferences
    {
        bool Save();

        SortFilterPreferences Load();

        CalendarEventList Sort(CalendarEventList allEvents);
    }
}
