using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGC.Library
{
    public interface IDateTimePreferences
    {
        bool Save();

        DateTimePreferences Load();

        string StartDateTime(CalendarEvent ev);

        string EndDateTime(CalendarEvent ev);
    }
}
