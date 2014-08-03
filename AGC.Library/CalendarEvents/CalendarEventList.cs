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

        public CalendarEventList SortByStartDate(bool ascending)
        {
            if (ascending)
            {
                this.Sort((x, y) => DateTime.Compare(x.Start, y.Start));
            }
            else
            {
                this.Sort((x, y) => DateTime.Compare(y.Start, x.Start));
            }
            return this;
        }

        public CalendarEventList SortByStatus(bool ascending)
        {
            if (ascending)
            {
                this.Sort((x, y) => string.Compare(x.Status, y.Status));
            }
            else
            {
                this.Sort((x, y) => string.Compare(y.Status, x.Status));
            }
            return this;
        }

        public CalendarEventList SortByTitle(bool ascending)
        {
            if (ascending)
            {
                this.Sort((x, y) => string.Compare(x.Title, y.Title));
            }
            else
            {
                this.Sort((x, y) => string.Compare(y.Title, x.Title));
            }
            return this;
        }

        public CalendarEventList SortByLocation(bool ascending)
        {
            if (ascending)
            {
                this.Sort((x, y) => string.Compare(x.Location, y.Location));
            }
            else
            {
                this.Sort((x, y) => string.Compare(y.Location, x.Location));
            }
            return this;
        }

        public CalendarEventList SortByContent(bool ascending)
        {
            if (ascending)
            {
                this.Sort((x, y) => string.Compare(x.Content, y.Content));
            }
            else
            {
                this.Sort((x, y) => string.Compare(y.Content, x.Content));
            }
            return this;
        }

        public CalendarEventList SortByEndDate(bool ascending)
        {
            if (ascending)
            {
                this.Sort((x, y) => DateTime.Compare(x.End ?? x.Start, y.End ?? y.Start));
            }
            else
            {
                this.Sort((x, y) => DateTime.Compare(y.End ?? y.Start, x.End ?? x.Start));
            }
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