using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGC.Library
{
    public class CalendarEventList : List<CalendarEvent>
    {
        #region Public Methods

        public CalendarEventList SortByDate()
        {
            this.Sort((x, y) => DateTime.Compare(x.Start, y.Start));
            return this;
        }

        public void WriteConsoleLog()
        {
            if (this != null)
            foreach (CalendarEvent ev in this)
            {
                Console.WriteLine("--------------------------------------------------------------------------------");
                Console.WriteLine("Title: " + ev.Title);
                Console.WriteLine("Content: " + ev.Content);
                Console.WriteLine("Location: " + ev.Location);
                Console.WriteLine("Start: " + ev.Start);
                Console.WriteLine("End: " + ev.End);
                Console.WriteLine("--------------------------------------------------------------------------------");
                Console.WriteLine();
            }
        }

        public void WriteConsoleLogShort()
        {
            if (this != null)
                foreach (CalendarEvent ev in this)
                {
                    Console.WriteLine("{0} {1:dd.MM.yy HH:mm:ss} {2:dd.MM.yy HH:mm:ss}", ev.Title, ev.Start, ev.End);
                }
        }

        #endregion
    }
}
