﻿using System;
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
        private const string DEFAULT_CALENDAR = "primary";

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

        public CalendarEventList GetEventsMultipleCalendars(IGoogleCalendar calendar, TimeIntervals period, UserCalendarsPreferences preferences)
        {
            return GetEventsMultipleCalendars(calendar, period.Start, period.End, preferences);
        }

        public CalendarEventList GetEventsMultipleCalendars(IGoogleCalendar calendar, DateTime start, DateTime end, UserCalendarsPreferences preferences)
        {
            CalendarEventList selectedEvents = new CalendarEventList();

            var userCalendars = preferences.UserCalendars;

            foreach (var userCalendar in userCalendars)
            {
                CalendarEventList events = new CalendarEventList(); 
                if (userCalendar.IsVisible == true)
                {
                    calendar.SetCalendar(userCalendar.Id);
                    events = GetEvents(calendar, start, end);
                    calendar.SetCalendar(DEFAULT_CALENDAR);
                }
                // Mark events from not default calendar as fake
                if (userCalendar.IsPrimary == false)
                {
                    foreach (var ev in events)
                    {
                        ev.IsFake = true;
                    }
                }
                selectedEvents.AddRange(events);
            }

            selectedEvents = AddEmptyDaysFakeEvents(selectedEvents, start, end, preferences);
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

            bool multipleMonths;

            // Find if there are events from different month in the list by comparing years and month data of the first and last event in the list
            if (allEvents.Count > 1)
            {
                multipleMonths = allEvents[0].Start.Year != allEvents[allEvents.Count - 1].Start.Year ? true :
                                 allEvents[0].Start.Month != allEvents[allEvents.Count - 1].Start.Month;
            }
            else
            {
                multipleMonths = false;
            }
            
            try
            {
                int month = 0;

                foreach (CalendarEvent ev in allEvents)
                {
                    if (preferences.GroupByMonth && multipleMonths && month != ev.Start.Month)
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
                if (ev.Start.DayOfWeek == DayOfWeek.Monday && preferences.Monday)
                {
                    filteredEvents.Add(ev);
                }
                else if (ev.Start.DayOfWeek == DayOfWeek.Tuesday && preferences.Tuesday)
                {
                    filteredEvents.Add(ev);
                }
                else if (ev.Start.DayOfWeek == DayOfWeek.Wednesday && preferences.Wednesday)
                {
                    filteredEvents.Add(ev);
                }
                else if (ev.Start.DayOfWeek == DayOfWeek.Thursday && preferences.Thursday)
                {
                    filteredEvents.Add(ev);
                }
                else if (ev.Start.DayOfWeek == DayOfWeek.Friday && preferences.Friday)
                {
                    filteredEvents.Add(ev);
                }
                else if (ev.Start.DayOfWeek == DayOfWeek.Saturday && preferences.Saturday)
                {
                    filteredEvents.Add(ev);
                }
                else if (ev.Start.DayOfWeek == DayOfWeek.Sunday && preferences.Sunday)
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

        private CalendarEventList AddEmptyDaysFakeEvents(CalendarEventList events, DateTime start, DateTime end, UserCalendarsPreferences preferences)
        {
            if (preferences.ShowEmptyDays || preferences.ShowEmptyWeekends)
            {
                HashSet<DateTime> emptyDays = new HashSet<DateTime>();

                // Add all days in the period to empty days list
                foreach (DateTime day in EachDay(start, end))
                {
                    emptyDays.Add(new DateTime(day.Year, day.Month, day.Day));
                }

                // Remove days from empty day list if there is an event in this day
                foreach (var ev in events)
                {
                    emptyDays.Remove(new DateTime(ev.Start.Year, ev.Start.Month, ev.Start.Day));
                    DateTime stop = ev.End ?? ev.Start;
                    emptyDays.Remove(new DateTime(stop.Year, stop.Month, stop.Day));
                }

                // Add Fake Events for empty days
                foreach (var emptyDay in emptyDays)
                {
                    if (preferences.ShowEmptyDays == true && preferences.ShowEmptyWeekends == false)
                    {
                        events.Add(new CalendarEvent("Empty", emptyDay));
                    }
                    else if (preferences.ShowEmptyDays == false && preferences.ShowEmptyWeekends == true)
                    {
                        if (IsWeekend(emptyDay))
                        {
                            events.Add(new CalendarEvent("Weekend", emptyDay));
                        }
                    }
                    else if (preferences.ShowEmptyDays == true && preferences.ShowEmptyWeekends == true)
                    {
                        if (IsWeekend(emptyDay))
                        {
                            events.Add(new CalendarEvent("Weekend", emptyDay));
                        }
                        else
                        {
                            events.Add(new CalendarEvent("Empty", emptyDay));
                        }
                    }
                }
                events.Sort((ev1, ev2) => DateTime.Compare(ev1.Start, ev2.Start));
            }

            return events;
        }

        private IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }

        private bool IsWeekend(DateTime date)
        {
            return (int)date.DayOfWeek == 0 || (int)date.DayOfWeek == 6 ? true : false;
        }

        #endregion
    }
}
