using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace AGC.Library
{
    [Serializable()]
    public class DateTimePreferences : IDateTimePreferences, ISerializable 
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const int VERSION = 0;
        private const string PAUSE = " . ";

        public string Day { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }

        public string Time { get; set; }

        public char Delimeter { get; set; }

        public bool DateFormatUS { get; set; }
        public bool HideYear { get; set; }
        public bool HideEndDate { get; set; }
        public bool HideStartTimeAndEndDateIfFullDay { get; set; }
        public bool HideMonthIfCurrent { get; set; }

        //Default constructor
        public DateTimePreferences()
        {
            this.Day = "ddd d";
            this.Month = "MMM";
            this.Year = "yyyy";

            this.Time = "HH:mm";
            this.Delimeter = ' ';

            this.DateFormatUS = false;
            this.HideYear = true;
            this.HideEndDate = false;
            this.HideStartTimeAndEndDateIfFullDay = true;
            this.HideMonthIfCurrent = true;
        }

        //Deserialization constructor
        public DateTimePreferences(SerializationInfo info, StreamingContext ctxt)
        {
            int version = (int)info.GetValue("Version", typeof(int));
            if (version == 0)
            {
                // Restore properties for version 0
                Day = (string)info.GetValue("Day", typeof(string));
                Month = (string)info.GetValue("Month", typeof(string));
                Year = (string)info.GetValue("Year", typeof(string));
                Time = (string)info.GetValue("Time", typeof(string));
                Delimeter = (char)info.GetValue("Delimeter", typeof(char));
                DateFormatUS = (bool)info.GetValue("DateFormatUS", typeof(bool));
                HideYear = (bool)info.GetValue("HideYear", typeof(bool));
                HideEndDate = (bool)info.GetValue("HideEndDate", typeof(bool));
                HideStartTimeAndEndDateIfFullDay = (bool)info.GetValue("HideStartTimeAndEndDateIfFullDay", typeof(bool));
                HideMonthIfCurrent = (bool)info.GetValue("HideMonthIfCurrent", typeof(bool));
            }
            if (version == 1)
            {
                // ....
            }
        }

        //Serialization function.
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            //You can use any custom name for your name-value pair. But make sure you
            // read the values with the same name. For ex:- If you write EmpId as "EmployeeId"
            // then you should read the same with "EmployeeId"
            info.AddValue("Version", VERSION);
            info.AddValue("Day", Day);
            info.AddValue("Month", Month);
            info.AddValue("Year", Year);
            info.AddValue("Time", Time);
            info.AddValue("Delimeter", Delimeter);
            info.AddValue("DateFormatUS", DateFormatUS);
            info.AddValue("HideYear", HideYear);
            info.AddValue("HideEndDate", HideEndDate);
            info.AddValue("HideStartTimeAndEndDateIfFullDay", HideStartTimeAndEndDateIfFullDay);
            info.AddValue("HideMonthIfCurrent", HideMonthIfCurrent);
        }

        public bool Save()
        {
            try
            {
                log.Debug("Saving DateTimePreferences...");
                Stream stream = File.Open("DateTimePreferences.config", FileMode.Create);
                BinaryFormatter bformatter = new BinaryFormatter();
                bformatter.Serialize(stream, this);
                stream.Close();
                log.Debug("DateTimePreferences were saved");
                return true;
            }
            catch(Exception ex)
            {
                log.Error("Failed to save DateTimePreferences:", ex);
                return false;
            }
        }

        public DateTimePreferences Load()
        {
            DateTimePreferences preferences = new DateTimePreferences();

            try
            {
                if (File.Exists("DateTimePreferences.config"))
                {
                    log.Debug("Loading DateTimePreferences...");
                    Stream stream = File.Open("DateTimePreferences.config", FileMode.Open);
                    BinaryFormatter bformatter = new BinaryFormatter();
                    preferences = (DateTimePreferences)bformatter.Deserialize(stream);
                    stream.Close();
                    log.Debug("DateTimePreferences were loaded");
                }
            }
            catch (Exception ex)
            {
                log.Error("Failed to load DateTimePreferences:", ex);
            }
            return preferences;
        }

        public string StartDateTime(CalendarEvent ev)
        {
            string year = HideYear ? string.Empty : Delimeter + Year;
            string time = HideStartTimeAndEndDateIfFullDay && ev.IsFullDateEvent ? string.Empty : Time;

            if (HideMonthIfCurrent && ev.Start.Month == DateTime.Today.Month && ev.Start.Year == DateTime.Today.Year)
            {
                return String.Format("{0:" + Day + year + PAUSE + time + "}", ev.Start);
            }

            return DateFormatUS ? String.Format("{0:" + Month + Delimeter + Day + year + PAUSE + time + "}", ev.Start) :
                                  String.Format("{0:" + Day + Delimeter + Month + year + PAUSE + time + "}", ev.Start);
        }

        public string EndDateTime(CalendarEvent ev)
        {
            if (HideEndDate)
            {
                return string.Empty;
            }

            if (HideStartTimeAndEndDateIfFullDay && ev.IsFullDateEvent)
            {
                return string.Empty;
            }

            if (ev.End == null)
            {
                return string.Empty;
            }

            string year = HideYear ? string.Empty : Delimeter + Year;
            DateTime end = ev.End ?? DateTime.Today;

            if (HideMonthIfCurrent && end.Month == DateTime.Today.Month && end.Year == DateTime.Today.Year)
            {
                return String.Format("{0:" + Day + year + PAUSE + Time + "}", end);
            }

            return DateFormatUS ? String.Format("{0:" + Month + Delimeter + Day + year + PAUSE + Time + "}", end) :
                                  String.Format("{0:" + Day + Delimeter + Month + year + PAUSE + Time + "}", end);
        }
    }
}