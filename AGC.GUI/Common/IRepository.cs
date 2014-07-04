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
        //void GetCurrentEvent(Action<CalendarEvent> callback);

        CalendarEvent GetCurrentEvent();

        void SetCurrentEvent(CalendarEvent calEvent);
    }
}
