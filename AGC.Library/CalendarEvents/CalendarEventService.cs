using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGC.Library
{
    public class CalendarEventService : ICalendarEventService
    {
        #region Constants

        private const string CONFIRMED = "";
        private const string TENTATIVE = "*";

        #endregion

        #region Private Variables

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly string[] months = new string[] { string.Empty, "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

        #endregion

        #region Constructor

        public CalendarEventService()
        { }

        #endregion

        #region Public Metods

        public CalendarEventList GetEvents(IGoogleCalendar calendar, TimeIntervals period)
        {
            return GetEvents(calendar, period.Start, period.End);
        }

        public CalendarEventList GetEvents(IGoogleCalendar calendar, DateTime start, DateTime end)
        {
            log.Debug("Select events, period=" + start + " - " + end);
            CalendarEventList selectedEvents = new CalendarEventList();


            foreach (CalendarEvent ev in calendar.GetEvents(start, end))
            {
                try
                {
                    if (MatchStartDate(ev.Start, start) && MatchEndDate(ev.End, end))
                    {
                        selectedEvents.Add(ev);
                    }                    
                }
                catch (Exception ex)
                {
                    log.Error("Failed to get events for a specified period", ex);
                    log.Info("Event info: " + ev.ToString());
                }
            }
            log.Debug("Successfully selected events for a specified period");
            return selectedEvents;
        }

        public CalendarEventList SearchEvents(CalendarEventList events, String keyword)
        {
            CalendarEventList selectedEvents = new CalendarEventList();
            try
            {
                if (!String.IsNullOrEmpty(keyword))
                {
                    foreach (CalendarEvent ev in events)
                    {
                        if (!string.IsNullOrEmpty(ev.Title))
                        {
                            if (ev.Title.ToLower().Contains(keyword.ToLower()))
                            {
                                selectedEvents.Add(ev);
                            }
                        }
                        else if (!string.IsNullOrEmpty(ev.Location))
                        {
                            if (ev.Location.ToLower().Contains(keyword.ToLower()))
                            {
                                selectedEvents.Add(ev);
                            }
                        }
                        else if (!string.IsNullOrEmpty(ev.Location))
                        {
                            if (ev.Content.ToLower().Contains(keyword.ToLower()))
                            {
                                selectedEvents.Add(ev);
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                log.Error("Failed to get events for a specified keyword \"" + keyword + "\"", ex);
            }

            return selectedEvents;
        }

        public CalendarEventList FormatEventsDatesStringRepresentation(CalendarEventList allEvents, DateTimePreferences preferences)
        {
            CalendarEventList formatedEvents = new CalendarEventList();

            try
            {
                int month = 0;

                foreach (CalendarEvent ev in allEvents)
                {
                    if(preferences.GroupByMonth && month != ev.Start.Month)
                    {
                        formatedEvents.Add(new CalendarEvent(months[ev.Start.Month]));
                        month = ev.Start.Month;
                    }

                    ev.FormatedStartDate = preferences.StartDateTime(ev);
                    ev.FormatedEndDate = preferences.EndDateTime(ev);
                    formatedEvents.Add(ev);                    
                }
                return formatedEvents;
            }
            catch(Exception ex)
            {
                log.Error("Failed to format events dates", ex);
                return allEvents;
            }
        }

        public CalendarEventList Sort(CalendarEventList allEvents, SortFilterPreferences preferences)
        {
            return preferences.Sort(allEvents);
        }

        public CalendarEventList FilterByStartTime(CalendarEventList allEvents, SortFilterPreferences preferences)
        {
            if (!preferences.EnableTimeFilter)
            {
                return allEvents;
            }

            CalendarEventList filteredEvents = new CalendarEventList();

            foreach (CalendarEvent ev in allEvents)
            {
                int starTimeInMinutes = ev.Start.Hour * 60 + ev.Start.Minute;
                if (starTimeInMinutes >= preferences.TimeInMinutesMin && starTimeInMinutes <= preferences.TimeInMinutesMax)
                {
                    filteredEvents.Add(ev);
                }
            }
            return filteredEvents;
        }

        public CalendarEventList FilterByDayOfWeek(CalendarEventList allEvents, SortFilterPreferences preferences)
        {
            if (!preferences.EnableDayOfWeekFilter)
            {
                return allEvents;
            }

            CalendarEventList filteredEvents = new CalendarEventList();

            foreach (CalendarEvent ev in allEvents)
            {
                if (ev.Start.DayOfWeek == preferences.Weekday)
                {
                    filteredEvents.Add(ev);
                }
            }
            return filteredEvents;
        }

        public CalendarEventList FilterByStatus(CalendarEventList allEvents, SortFilterPreferences preferences)
        {
            if (!preferences.EnableStatusFilter)
            {
                return allEvents;
            }

            CalendarEventList filteredEvents = new CalendarEventList();

            foreach (CalendarEvent ev in allEvents)
            {
                if (ev.Status == (preferences.ShowConfirmedOnly ? CONFIRMED : TENTATIVE))
                {
                    filteredEvents.Add(ev);
                }
            }
            return filteredEvents;
        }

        #endregion

        #region Private Methods

        private static Boolean MatchStartDate(DateTime? eventStartNullable, DateTime start)
        {
            if (eventStartNullable == null)
            {
                return true;
            }
            else
            {
                DateTime eventStart = eventStartNullable ?? DateTime.Now;

                int result = DateTime.Compare(eventStart, start);

                if (result >= 0)
                    return true;
                else
                    return false;
            }
        }

        private static Boolean MatchEndDate(DateTime? eventEndNullable, DateTime end)
        {
            if (eventEndNullable == null)
            {
                return true;
            }
            else
            {
                DateTime eventEnd = eventEndNullable ?? DateTime.Now;

                int result = DateTime.Compare(eventEnd, end);

                if (result <= 0)
                    return true;
                else
                    return false;
            }
        }

        #endregion
    }
}
