using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AGC.Library
{
    public class GoogleCalendar : IGoogleCalendar
    {
        private const string CLIENT_ID = "643150686764-lr52us0outlsd46hn6vhn58nhj6q85tr.apps.googleusercontent.com";
        private const string CLIENT_SECRET = "Bgms-ReGArsk8xzjUJhqiuUJ";
        private const string DEFAULT_CALENDAR = "primary";
        private const string APPLICATION_NAME = "Accessible Google Calendar";

        private const string DATE_FORMAT = "yyyy-MM-dd";
        private const string TIME_FORMAT = "yyyy-MM-ddTHH:MM:ss";

        private CalendarService service;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Constructor

        public GoogleCalendar()
        {
            Authorization();
        }

        #endregion

        #region Public Methods

        public bool CreateEvent(CalendarEvent ev)
        {
            log.Debug("Try to create new event title=\"" + ev.Title + "\"");

            try
            {
                // New event
                Event newEvent = new Event();

                newEvent.Summary = ev.Title;
                newEvent.Location = ev.Location;
                newEvent.Description = ev.Content;

                newEvent.Start = ConvertToEventDateTime(ev.Start, ev.IsFullDateEvent);
                newEvent.End = ev.IsFullDateEvent ? ConvertToEventDateTime(ev.Start, true) : ConvertToEventDateTime(ev.End, false);

                // Recurrence
                if (!String.IsNullOrEmpty(ev.RRule))
                {
                    newEvent.Recurrence = new String[] { ev.RRule };
                }

                // Reminder
                newEvent.Reminders = new Event.RemindersData()
                {
                    Overrides = new List<EventReminder> 
                    { 
                        new EventReminder()
                        {
                            Method = "email",
                            Minutes = ev.Reminder
                        }
                    },
                    UseDefault = false
                };

                service.Events.Insert(newEvent, DEFAULT_CALENDAR).Execute();
             
                log.Debug("New event was successfully created");

                return true;
            }
            catch (Exception ex)
            {
                log.Error("Event creation failed with error:", ex);
                log.Info("Event Details: " + ev.ToString());
                return false;               
            }
        }

        public bool AddQuickEvent(String eventText)
        {
            log.Debug("Try to add quick event");

            try
            {
                service.Events.QuickAdd(DEFAULT_CALENDAR, eventText).Execute();
                log.Debug("Quick event was successfully added");
                return true;
            }
            catch(Exception ex)
            {
                log.Error("Failed to add quick event. Error:", ex);
                log.Info("Event Details: " + eventText);
                return false;   
            }           
        }

        public bool DeleteEvent(CalendarEvent ev)
        {
            try
            {
                service.Events.Delete(DEFAULT_CALENDAR, ev.Id).Execute();
                return true;
            }
            catch(Exception ex)
            {
                log.Error("Event deleting failed with error:", ex);
                log.Info("Event Details: " + ev.ToString());
                return false;
            }
        }

        public bool DeleteSingleInstanceOfRecurringEvent(CalendarEvent ev)
        {
            return DeleteEvent(ev);
        }

        public bool DeleteAllInstancesOfRecurringEvent(CalendarEvent ev)
        {
            ev.Id = ev.Id.Split('_')[0];
            return DeleteEvent(ev);
        }

        public CalendarEventList GetAllEvents()
        {
            log.Debug("Try to get all events from Google Calendar");
            CalendarEventList calendarEvents = new CalendarEventList();

            try
            {
                EventsResource.ListRequest events = service.Events.List(DEFAULT_CALENDAR);
                events.SingleEvents = true;
                events.MaxResults = 2500;
                events.TimeMin = DateTime.Today.AddMonths(-2);
                events.TimeMax = DateTime.Today.AddMonths(10);
                Events eventList = events.Execute();
                calendarEvents = ConvertToCalendarEvents(eventList.Items);
                calendarEvents.SortByDate();
                log.Debug("Successfully got all events from Google Calendar");
            }
            catch(Exception ex)
            {
                log.Error("Failed to get all events from Google Calendar with error:", ex);
            }

            return calendarEvents;
        }

        public void DeleteEvents(CalendarEventList evs)
        {
            foreach (CalendarEvent ev in evs)
            {
                DeleteEvent(ev);
            }
        }

        #endregion

        #region Private Methods

        private void Authorization()
        {
            log.Debug("Start Authorization");

            try
            {                
                UserCredential credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets
                {
                    ClientId = CLIENT_ID,
                    ClientSecret = CLIENT_SECRET,
                },
                new[] { CalendarService.Scope.Calendar },
                "user",
                CancellationToken.None).Result;

                // Create the service.
                service = new CalendarService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = APPLICATION_NAME,
                });
            }
            catch(Exception ex)
            {
                log.Error("Authorization failed with error:", ex);
            }
            log.Debug("Finish Authorization");

        }

        private static EventDateTime ConvertToEventDateTime(DateTime? dateTime, bool isFullDateEvent)
        {
            EventDateTime eventDateTime = new EventDateTime();
            if (isFullDateEvent)
            {
                DateTime date = dateTime ?? DateTime.Now;
                eventDateTime.Date = date.ToString(DATE_FORMAT);
            }
            else
            {
                eventDateTime.DateTime = dateTime;
            }           
            eventDateTime.TimeZone = "Europe/London";
            return eventDateTime;
        }

        private static CalendarEvent ConvertGoogleEventToCalendarEvent(Event ev)
        {
            CalendarEvent calendarEvent;
            try
            {
                calendarEvent = new CalendarEvent(ev.Id, ev.Summary, ev.Description, ev.Location, GetEventStartDate(ev), GetEventEndDate(ev), IsFullDayEvent(ev), IsRecurringEvent(ev));
            }
            catch(Exception ex)
            {
                log.Error("Failed to convert Google Calendar event to Calendar event with error:", ex);
                log.Info("Event Details");
                log.Info("Event Id: " + ev.Id);
                log.Info("Event Summary: " + ev.Summary);
                log.Info("Event Description: " + ev.Description);
                log.Info("Event Location: " + ev.Location);
                log.Info("Event Start: " + ev.Start.DateTime);
                log.Info("Event End: " + ev.End.DateTime);
                calendarEvent = new CalendarEvent();
            }

            return calendarEvent;
        }

        private static CalendarEventList ConvertToCalendarEvents(IList<Event> googleEvents)
        {
            CalendarEventList calendarEvents = new CalendarEventList();
            
            foreach (Event ev in googleEvents)
            {
                calendarEvents.Add(ConvertGoogleEventToCalendarEvent(ev));
            }

            return calendarEvents;
        }

        private static DateTime GetEventStartDate(Event ev)
        {
            return ev.Start.Date == null ? (DateTime)ev.Start.DateTime : Convert.ToDateTime(ev.Start.Date);
        }

        private static DateTime? GetEventEndDate(Event ev)
        {
            return ev.End.Date == null ? ev.End.DateTime : Convert.ToDateTime(ev.End.Date).AddSeconds(-1);
        }

        private static bool IsFullDayEvent(Event ev)
        {
            if (!String.IsNullOrEmpty(ev.End.Date))
                return true;
            else
                return false;
        }

        private static bool IsRecurringEvent(Event ev)
        {
            if (ev.Id.Contains('_'))
                return true;
            else
                return false;
        }

        private static string GetRRule(IList<string> recurrence)
        {
            return recurrence[0];
        }

        #endregion
    }
}
