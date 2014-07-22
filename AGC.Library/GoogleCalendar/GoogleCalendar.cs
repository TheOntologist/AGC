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

        public enum ActionType
        {
            none,
            single,
            all,
            following
        }

        public bool CreateEvent(CalendarEvent ev)
        {
            log.Debug("Try to create new event title=\"" + ev.Title + "\"");

            try
            {
                // New event
                Event newEvent = ConvertCalendarEventToGoogleEvent(ev, false);

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

        public bool TestCreateEvent(CalendarEvent ev)
        {
            log.Debug("Try to create new event title=\"" + ev.Title + "\"");

            try
            {
                // New event
                Event newEvent = ConvertCalendarEventToGoogleEvent(ev, false);
                newEvent.Status = "cancelled";

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

        public bool UpdateEvent(CalendarEvent ev, ActionType type)
        {
            log.Debug("Try to update event title=\"" + ev.Title + "\"");
            try
            {
                switch (type)
                {
                    case ActionType.single:
                        {
                            ev.RRule = String.Empty;
                            break;
                        }
                    case ActionType.all:
                        {
                            ev.Id = GetMainEventId(ev.Id);
                            break;
                        }
                    case ActionType.following:
                        {
                            // Create recurrence event with new settings
                            CreateEvent(ev);
                            ev = GetAllPreviousEvents(ev);
                            break;
                        }
                }

                Event newEvent = ConvertCalendarEventToGoogleEvent(ev, true);

                // Increate sequence number... I hate you Google API for your crazy things >_<
                newEvent = UpdateSequenceNumber(newEvent);

                service.Events.Update(newEvent, DEFAULT_CALENDAR, newEvent.Id).Execute();

                log.Debug("New event was successfully updated");

                return true;
            }
            catch (Exception ex)
            {
                log.Error("Event update failed with error:", ex);
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

        public bool DeleteEvent(CalendarEvent ev, ActionType type)
        {
            try
            {
                switch (type)
                {
                    case ActionType.single:
                        {
                            service.Events.Delete(DEFAULT_CALENDAR, ev.Id).Execute();
                            break;
                        }
                    case ActionType.all:
                        {
                            ev.Id = GetMainEventId(ev.Id);
                            service.Events.Delete(DEFAULT_CALENDAR, ev.Id).Execute();
                            break;
                        }
                    case ActionType.following:
                        {
                            UpdateEvent(GetAllPreviousEvents(ev), ActionType.all);
                            break;
                        }
                }
                return true;
            }
            catch(Exception ex)
            {
                log.Error("Event deleting failed with error:", ex);
                log.Info("Event Details: " + ev.ToString());
                return false;
            }
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
                DeleteEvent(ev, ActionType.single);
            }
        }

        public RecurrenceSettings GetRecurrenceSettings(CalendarEvent ev)
        {
            if (!ev.IsRecurrenceEvent)
            {
                return new RecurrenceSettings();
            }

            return new RecurrenceSettings(ev.Start, GetGoogleEventById(GetMainEventId(ev.Id)).Recurrence[0]);
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

        private static Event ConvertCalendarEventToGoogleEvent(CalendarEvent ev, bool rememberId)
        {
            try
            {
                Event googleEvent = new Event();

                if (!string.IsNullOrEmpty(ev.Id) && rememberId)
                {
                    googleEvent.Id = ev.Id;
                }

                googleEvent.Summary = ev.Title;
                googleEvent.Location = ev.Location;
                googleEvent.Description = ev.Content;

                googleEvent.Start = ConvertToEventDateTime(ev.Start, ev.IsFullDateEvent);
                googleEvent.End = ev.IsFullDateEvent ? ConvertToEventDateTime(ev.Start, true) : ConvertToEventDateTime(ev.End, false);

                // Recurrence
                if (!String.IsNullOrEmpty(ev.RRule))
                {
                    googleEvent.Recurrence = new String[] { ev.RRule };
                }

                // Reminder
                googleEvent.Reminders = ConvertMinutesToGoogleEventReminder(ev.Reminder);

                return googleEvent;
            }
            catch (Exception ex)
            {
                log.Error("CalendarEvent convertation to GoogleEvent failed with error:", ex);
                log.Info("Event Details: " + ev.ToString());
                return null;
            }
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

        private static Event.RemindersData ConvertMinutesToGoogleEventReminder(int minutes)
        {
            Event.RemindersData reminder = new Event.RemindersData()
            {
                Overrides = new List<EventReminder> 
                    { 
                        new EventReminder()
                        {
                            Method = "email",
                            Minutes = minutes
                        }
                    },
                UseDefault = false
            };
            return reminder;
        }

        private Event GetGoogleEventById(string id)
        {
            return service.Events.Get(DEFAULT_CALENDAR, id).Execute();
        }

        private CalendarEvent GetAllPreviousEvents(CalendarEvent ev)
        {
            // Get recurrence event using it's single instance event id
            CalendarEvent old = ConvertGoogleEventToCalendarEvent(GetGoogleEventById(GetMainEventId(ev.Id)));

            // Get old event recurrence settings
            RecurrenceSettings previous = GetRecurrenceSettings(old);

            // Change it to end one day before new event
            previous.EndsOn(ev.Start.AddDays(-1));
            old.RRule = previous.ToString();

            return old;
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

        private static string GetMainEventId(string id)
        {
            return id.Split('_')[0];
        }

        private Event UpdateSequenceNumber(Event ev)
        {
            int sequence = service.Events.Get(DEFAULT_CALENDAR, ev.Id).Execute().Sequence ?? 0;
            sequence++;
            ev.Sequence = sequence;
            return ev;
        }

        private static string GetRRule(Event ev)
        {
            if (!IsFullDayEvent(ev))
            {
                return String.Empty;
            }

            return ev.Recurrence != null ? ev.Recurrence[0] : String.Empty;
        }

        private static int GetReminder(Event ev)
        {
            if (ev.Reminders.UseDefault == true)
            {
                return 10;
            }
            else
            {
                return ev.Reminders.Overrides[0].Minutes ?? 10;
            }           
        }

        #endregion
    }
}
