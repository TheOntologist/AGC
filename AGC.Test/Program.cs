using AGC.Library;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace AGC.Test
{
    class Program
    {
        #region Private Variables

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static GoogleCalendar calendar = new GoogleCalendar();
        private static TimeIntervals period = new TimeIntervals();
        private static CalendarEventList allEvents = calendar.GetAllEvents();
        private static ICalendarEventService service = new CalendarEventService();

        #endregion

        static void Main(string[] args)
        {
            Console.WriteLine("################################################################################");
            Console.WriteLine("#                                 START TESTS                                  #");
            Console.WriteLine("################################################################################");
            Console.WriteLine();

            calendar.TestCreateEvent(new CalendarEvent("Today", "Content", "Location", period.Today().Start, period.Today().End));

            /*
            CalendarEventList all = calendar.GetAllEvents();

            foreach (CalendarEvent ev in all)
            {
                if(ev.IsRecurrenceEvent)
                {
                    RecurrenceSettings rec = calendar.GetRecurrenceSettings(ev);
                    int i = 0;
                }
            }
             */
            //CreateTestEvents();
            //allEvents.SortByDate();
            //calendar.DeleteEvents(calendar.GetAllEvents());

            Console.WriteLine("################################################################################");
            Console.WriteLine("#                                  END TESTS                                   #");
            Console.WriteLine("################################################################################");
            Console.ReadLine();
        }


        #region TestMethods

        private static void CreateTestEvents()
        {
            calendar.CreateEvent(new CalendarEvent("Today", "Content", "Location", period.Today().Start, period.Today().End));
            calendar.CreateEvent(new CalendarEvent("Tomorrow", "Content", "Location", period.Tomorrow().Start, period.Tomorrow().End));
            calendar.CreateEvent(new CalendarEvent("This Week Wednesday", "Content", "Location", period.ThisWeek().Start.AddDays(2), period.ThisWeek().Start.AddDays(2).AddHours(5)));
            calendar.CreateEvent(new CalendarEvent("Next Week Friday", "Content", "Location", period.NextWeek().Start.AddDays(4), period.NextWeek().Start.AddDays(4).AddHours(5)));
            calendar.CreateEvent(new CalendarEvent("This Month 20", "Content", "Location", period.ThisMonth().Start.AddDays(19), period.ThisMonth().Start.AddDays(19).AddHours(5)));
            calendar.CreateEvent(new CalendarEvent("Next Month 25", "Content", "Location", period.NextMonth().Start.AddDays(24), period.NextMonth().Start.AddDays(24).AddHours(5)));
            //calendar.CreateEvent(new CalendarEvent("Next + 3 Month Single (last month 19)", "Content", "Location", period.NextNMonth(3, true).Start.AddDays(18), period.NextNMonth(3, true).Start.AddDays(18).AddHours(5)));
            //calendar.CreateEvent(new CalendarEvent("Next + 3 Month Period (+ 2 month 17)s", "Content", "Location", period.NextNMonth(2, false).Start.AddMonths(2).AddDays(16), period.NextNMonth(2, false).Start.AddMonths(2).AddDays(16).AddHours(5)));
        }

        private static void DeleteTestEvents()
        {
            calendar.DeleteEvents(service.SearchEvents(allEvents, "Content"));
        }

        private static void DeleteAllEvents()
        {
            calendar.DeleteEvents(calendar.GetAllEvents());
        }

        private static void TestAddQuickEvent()
        {
            calendar.AddQuickEvent("Dinner with Michael 7 p.m. tomorrow");
        }

        private static void TestTimeIntervals()
        {
            Console.WriteLine("Today");
            period.Today().WriteConsoleLog();
            Console.WriteLine();

            Console.WriteLine("Tomorrow");
            period.Tomorrow().WriteConsoleLog();
            Console.WriteLine();

            Console.WriteLine("This Week");
            period.ThisWeek().WriteConsoleLog();
            Console.WriteLine();

            Console.WriteLine("Next Week");
            period.NextWeek().WriteConsoleLog();
            Console.WriteLine();

            Console.WriteLine("This Month");
            period.ThisMonth().WriteConsoleLog();
            Console.WriteLine();

            Console.WriteLine("Next Month");
            period.NextMonth().WriteConsoleLog();
            Console.WriteLine();

            //Console.WriteLine("Next + 3 Month Single");
            //period.NextNMonth(3, true).WriteConsoleLog();
            //Console.WriteLine();

            //Console.WriteLine("Next + 3 Month Period");
            //period.NextNMonth(3, false).WriteConsoleLog();
            //Console.WriteLine();
        }

        private static void TestCalendarEventService()
        {
            Console.WriteLine("################################################################################");
            Console.WriteLine("#                                    Today                                     #");
            Console.WriteLine("################################################################################");
            service.GetEvents(allEvents, period.Today()).WriteConsoleLogShort();
            Console.WriteLine("################################################################################");
            Console.WriteLine("#                                 Tomorrow                                     #");
            Console.WriteLine("################################################################################");
            service.GetEvents(allEvents, period.Tomorrow()).WriteConsoleLogShort();
            Console.WriteLine("################################################################################");
            Console.WriteLine("#                                This Week                                     #");
            Console.WriteLine("################################################################################");
            service.GetEvents(allEvents, period.ThisWeek()).WriteConsoleLogShort();
            Console.WriteLine("################################################################################");
            Console.WriteLine("#                                Next Week                                     #");
            Console.WriteLine("################################################################################");
            service.GetEvents(allEvents, period.NextWeek()).WriteConsoleLogShort();
            Console.WriteLine("################################################################################");
            Console.WriteLine("#                                This Month                                    #");
            Console.WriteLine("################################################################################");
            service.GetEvents(allEvents, period.ThisMonth()).WriteConsoleLogShort();
            Console.WriteLine("################################################################################");
            Console.WriteLine("#                                Next Month                                    #");
            Console.WriteLine("################################################################################");
            service.GetEvents(allEvents, period.NextMonth()).WriteConsoleLogShort();
        }

        private static void TestCalendarEventServiceSearch()
        {
            Console.WriteLine("Keyword: Today");
            service.SearchEvents(allEvents, "Today").WriteConsoleLogShort();
            Console.WriteLine();

            Console.WriteLine("Keyword: Content");
            service.SearchEvents(allEvents, "Content").WriteConsoleLogShort();
            Console.WriteLine();

            Console.WriteLine("Keyword: loCaTiOn");
            service.SearchEvents(allEvents, "loCaTiOn").WriteConsoleLogShort();
            Console.WriteLine();

            Console.WriteLine("Keyword: NotExits");
            service.SearchEvents(allEvents, "NotExits").WriteConsoleLogShort();
            Console.WriteLine();
        }

        private static void CreateRecurringTestEvents()
        {
            calendar.CreateEvent(new CalendarEvent("rrule Weekly MO FR 5 occ", "Content", "Location", period.Today().Start.AddHours(10), period.Today().End.AddHours(-10), 
                                 new RecurrenceSettings(DateTime.Today).Weekly().Monday().Friday().EndsAfter(5).ToString()));
            calendar.CreateEvent(new CalendarEvent("rrule Daily ends Today + 10 days", "Content", "Location", period.Today().Start.AddHours(10), period.Today().End.AddHours(-10),
                                  new RecurrenceSettings(DateTime.Today).Daily().EndsOn(DateTime.Today.AddDays(10)).ToString()));
            calendar.CreateEvent(new CalendarEvent("rrule Monthly interval 2, by day of month 15 occr", "Content", "Location", period.Today().Start.AddHours(10), period.Today().End.AddHours(-10),
                                  new RecurrenceSettings(DateTime.Today).Monthly().Interval(2).ByDayOfMonth().EndsAfter(15).ToString()));
            calendar.CreateEvent(new CalendarEvent("rrule Monthly interval 2, by day of week 15 occ", "Content", "Location", period.Today().Start.AddHours(10), period.Today().End.AddHours(-10),
                                  new RecurrenceSettings(DateTime.Today).Monthly().Interval(2).ByDayOfWeek().EndsAfter(15).ToString()));
        }

        #endregion
    }
}
