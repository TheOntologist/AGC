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

        private const string DEFAULT_DATETIME_FORMAT = "ddd d MMM . HH:mm";

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
            this.FormatedStartDate = string.Format(DEFAULT_DATETIME_FORMAT, this.Start);
            this.FormatedEndDate = string.Empty;
            this.RRule = String.Empty;
            this.Reminder = 10;
            this.IsFake = false;
        }

        /// <summary>
        /// Fake event constructor
        /// </summary>
        public CalendarEvent(String calContent)
        {
            this.Title = String.Empty;
            this.Content = calContent;
            this.Location = String.Empty;
            this.Start = DateTime.Today;
            this.End = null;
            this.FormatedStartDate = string.Empty;
            this.FormatedEndDate = string.Empty;
            this.RRule = String.Empty;
            this.IsFake = true;
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
            this.FormatedStartDate = string.Format(DEFAULT_DATETIME_FORMAT, this.Start);
            this.FormatedEndDate = this.End == null ? string.Empty : string.Format(DEFAULT_DATETIME_FORMAT, this.End);
            this.RRule = String.Empty;
            this.Reminder = 10;
            this.IsFake = false;
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
            this.FormatedStartDate = string.Format(DEFAULT_DATETIME_FORMAT, this.Start);
            this.FormatedEndDate = this.End == null ? string.Empty : string.Format(DEFAULT_DATETIME_FORMAT, this.End);
            this.RRule = calRRule;
            this.Reminder = 10;
            this.IsFake = false;
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
            this.FormatedStartDate = string.Format(DEFAULT_DATETIME_FORMAT, this.Start);
            this.FormatedEndDate = this.End == null ? string.Empty : string.Format(DEFAULT_DATETIME_FORMAT, this.End);
            this.IsFullDateEvent = calIsFullDayEvent;
            this.IsRecurrenceEvent = calIsRecurrenceEvent;
            this.Reminder = 10;
            this.IsFake = false;
        }

        #endregion

        #region Public Fields

        public string Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string Location { get; set; }

        public DateTime Start { get; set; }

        public DateTime? End { get; set; }

        public string FormatedStartDate { get; set; }

        public string FormatedEndDate { get; set; }

        public string EndDate { get; set; }

        public string RRule { get; set; }

        public bool IsFullDateEvent { get; set; }

        public bool IsRecurrenceEvent { get; set; }

        public int Reminder { get; set; }

        public bool IsFake { get; set; }

        public override string ToString() 
        {
            return String.Format("{0} . {1} . {2} . {3} . {4}", this.FormatedStartDate, this.Title, this.Content, this.Location, this.FormatedEndDate);
        }

        #endregion
    }
}
