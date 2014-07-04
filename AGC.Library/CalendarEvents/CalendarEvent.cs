using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGC.Library
{
    public class CalendarEvent
    {
        #region Constants

        private const string DATE_FORMAT = "ddd d MMM";
        private const string DATE_FORMAT_CURRENT_MONTH = "ddd d";
        private const string TIME_FORMAT = "HH:mm";

        #endregion

        #region Constructors

        /// <summary>
        /// Empty Constructor
        /// </summary>
        public CalendarEvent()
        {
            this.Title = String.Empty;
            this.Content = String.Empty;
            this.Location = String.Empty;
            this.Start = DateTime.Today;
            this.End = null;
            this.RRule = String.Empty;
            this.Reminder = 10;
        }

        /// <summary>
        /// Create New Event from User data (no ID)
        /// </summary>
        /// <param name="calTitle"></param>
        /// <param name="calContent"></param>
        /// <param name="calLocation"></param>
        /// <param name="calStart"></param>
        /// <param name="calEnd"></param>
        public CalendarEvent(String calTitle, String calContent, String calLocation, DateTime calStart, DateTime? calEnd)
        {
            this.Title = calTitle;
            this.Content = calContent;
            this.Location = calLocation;
            this.Start = calStart;
            this.End = calEnd;
            this.RRule = String.Empty;
            this.Reminder = 10;
        }

        /// <summary>
        /// Create New Recurrence event from User data (no ID)
        /// </summary>
        /// <param name="calTitle"></param>
        /// <param name="calContent"></param>
        /// <param name="calLocation"></param>
        /// <param name="calStart"></param>
        /// <param name="calEnd"></param>
        /// <param name="calRRule"></param>
        public CalendarEvent(String calTitle, String calContent, String calLocation, DateTime calStart, DateTime? calEnd, String calRRule)
        {
            this.Title = calTitle;
            this.Content = calContent;
            this.Location = calLocation;
            this.Start = calStart;
            this.End = calEnd;
            this.RRule = calRRule;
            this.Reminder = 10;
        }

        /// <summary>
        /// Create Event from Google Calendar data (with ID)
        /// </summary>
        /// <param name="calId"></param>
        /// <param name="calTitle"></param>
        /// <param name="calContent"></param>
        /// <param name="calLocation"></param>
        /// <param name="calStart"></param>
        /// <param name="calEnd"></param>
        public CalendarEvent(String calId, String calTitle, String calContent, String calLocation, DateTime calStart, DateTime? calEnd, bool calIsFullDayEvent, bool calIsRecurrenceEvent)
        {
            this.Id = calId;
            this.Title = calTitle;
            this.Content = calContent;
            this.Location = calLocation;
            this.Start = calStart;
            this.End = calEnd;
            this.IsFullDateEvent = calIsFullDayEvent;
            this.IsRecurrenceEvent = calIsRecurrenceEvent;
            this.Reminder = 10;
        }

        #endregion

        #region Public Fields

        public string Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string Location { get; set; }

        public DateTime Start { get; set; }

        public DateTime? End { get; set; }

        public string RRule { get; set; }

        public bool IsFullDateEvent { get; set; }

        public bool IsRecurrenceEvent { get; set; }

        public int Reminder { get; set; }

        public override string ToString() 
        {
            /*
            bool thisMonthEventStart = DateTime.Today.Month == this.Start.Value.Month && DateTime.Today.Year == this.Start.Value.Year;
            bool thisMonthEventEnd = DateTime.Today.Month == this.End.Value.Month && DateTime.Today.Year == this.End.Value.Year;

            string startDateFormat = thisMonthEventStart ? DATE_FORMAT_CURRENT_MONTH : DATE_FORMAT;
            string endDateFormat = thisMonthEventEnd ? DATE_FORMAT_CURRENT_MONTH : DATE_FORMAT;
            */
            //return String.Format("{0} (.) {1} (.) {2}", this.Title, this.Content, this.Location);
            return String.Format("{0:" + DATE_FORMAT + "} . {1:HH:mm} . {2} . {3} . {4} . {5:" + DATE_FORMAT + "} . {6:HH:mm}", this.Start, this.Start, this.Title, this.Content, this.Location, this.End, this.End);
        }

        #endregion
    }
}
