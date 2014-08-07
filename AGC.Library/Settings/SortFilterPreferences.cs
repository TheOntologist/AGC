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
    public class SortFilterPreferences : ISortFilterPreferences, ISerializable 
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const int VERSION = 1;

        public enum SortBy
        {
            start,
            status,
            title,
            location,
            content,
            end
        }

        public bool Enable { get; set; }
        public bool EnableSorting { get; set; }
        public SortBy SortParam { get; set; }
        public bool SortOrderAscending { get; set; }

        public bool EnableTimeFilter { get; set; }
        public int TimeInMinutesMin { get; set; }
        public int TimeInMinutesMax { get; set; }

        public bool EnableDayOfWeekFilter { get; set; }
        //public DayOfWeek Weekday { get; set; }
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }

        public bool EnableStatusFilter { get; set; }
        public bool ShowConfirmedOnly { get; set; }

        //Default constructor
        public SortFilterPreferences()
        {
            Enable = false;
            EnableSorting = false;
            EnableTimeFilter = false;
            EnableDayOfWeekFilter = false;
            EnableStatusFilter = false;
            TimeInMinutesMin = 540;
            TimeInMinutesMax = 600;
            SortOrderAscending = true;
            ShowConfirmedOnly = true;
        }

        //Deserialization constructor
        public SortFilterPreferences(SerializationInfo info, StreamingContext ctxt)
        {
            int version = (int)info.GetValue("Version", typeof(int));
            if (version == 0)
            {
                // Restore properties for version 0
                Enable = (bool)info.GetValue("Enable", typeof(bool));
                EnableSorting = (bool)info.GetValue("EnableSorting", typeof(bool));
                SortParam = (SortBy)info.GetValue("SortParam", typeof(SortBy));
                SortOrderAscending = (bool)info.GetValue("SortOrderAscending", typeof(bool));
                EnableTimeFilter = (bool)info.GetValue("EnableTimeFilter", typeof(bool));
                TimeInMinutesMin = (int)info.GetValue("TimeInMinutesMin", typeof(int));
                TimeInMinutesMax = (int)info.GetValue("TimeInMinutesMax", typeof(int));
                EnableDayOfWeekFilter = (bool)info.GetValue("EnableDayOfWeekFilter", typeof(bool));
                EnableStatusFilter = (bool)info.GetValue("EnableStatusFilter", typeof(bool));
                ShowConfirmedOnly = (bool)info.GetValue("ShowConfirmedOnly", typeof(bool));
            }
            else if (version == 1)
            {
                Enable = (bool)info.GetValue("Enable", typeof(bool));
                EnableSorting = (bool)info.GetValue("EnableSorting", typeof(bool));
                SortParam = (SortBy)info.GetValue("SortParam", typeof(SortBy));
                SortOrderAscending = (bool)info.GetValue("SortOrderAscending", typeof(bool));
                EnableTimeFilter = (bool)info.GetValue("EnableTimeFilter", typeof(bool));
                TimeInMinutesMin = (int)info.GetValue("TimeInMinutesMin", typeof(int));
                TimeInMinutesMax = (int)info.GetValue("TimeInMinutesMax", typeof(int));
                EnableDayOfWeekFilter = (bool)info.GetValue("EnableDayOfWeekFilter", typeof(bool));
                EnableStatusFilter = (bool)info.GetValue("EnableStatusFilter", typeof(bool));
                ShowConfirmedOnly = (bool)info.GetValue("ShowConfirmedOnly", typeof(bool));

                Monday = (bool)info.GetValue("Monday", typeof(bool));
                Tuesday = (bool)info.GetValue("Tuesday", typeof(bool));
                Wednesday = (bool)info.GetValue("Wednesday", typeof(bool));
                Thursday = (bool)info.GetValue("Thursday", typeof(bool));
                Friday = (bool)info.GetValue("Friday", typeof(bool));
                Saturday = (bool)info.GetValue("Saturday", typeof(bool));
                Sunday = (bool)info.GetValue("Sunday", typeof(bool));
            }
        }

        //Serialization function.
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("Version", VERSION);
            info.AddValue("Enable", Enable);
            info.AddValue("EnableSorting", EnableSorting);
            info.AddValue("SortParam", SortParam);
            info.AddValue("SortOrderAscending", SortOrderAscending);
            info.AddValue("EnableTimeFilter", EnableTimeFilter);
            info.AddValue("TimeInMinutesMin", TimeInMinutesMin);
            info.AddValue("TimeInMinutesMax", TimeInMinutesMax);
            info.AddValue("EnableDayOfWeekFilter", EnableDayOfWeekFilter);

            info.AddValue("Monday", Monday);
            info.AddValue("Tuesday", Tuesday);
            info.AddValue("Wednesday", Wednesday);
            info.AddValue("Thursday", Thursday);
            info.AddValue("Friday", Friday);
            info.AddValue("Saturday", Saturday);
            info.AddValue("Sunday", Sunday);

            info.AddValue("EnableStatusFilter", EnableStatusFilter);
            info.AddValue("ShowConfirmedOnly", ShowConfirmedOnly);
        }

        public bool Save()
        {
            try
            {
                log.Debug("Saving SortFilterPreferences...");
                Stream stream = File.Open("SortFilterPreferences.config", FileMode.Create);
                BinaryFormatter bformatter = new BinaryFormatter();
                bformatter.Serialize(stream, this);
                stream.Close();
                log.Debug("SortFilterPreferences were saved");
                return true;
            }
            catch (Exception ex)
            {
                log.Error("Failed to save SortFilterPreferences:", ex);
                return false;
            }
        }

        public SortFilterPreferences Load()
        {
            SortFilterPreferences preferences = new SortFilterPreferences();

            try
            {
                if (File.Exists("SortFilterPreferences.config"))
                {
                    log.Debug("Loading SortFilterPreferences...");
                    Stream stream = File.Open("SortFilterPreferences.config", FileMode.Open);
                    BinaryFormatter bformatter = new BinaryFormatter();
                    preferences = (SortFilterPreferences)bformatter.Deserialize(stream);
                    stream.Close();
                    log.Debug("SortFilterPreferences were loaded");
                }
            }
            catch (Exception ex)
            {
                log.Error("Failed to load SortFilterPreferences:", ex);
            }
            return preferences;
        }

        public CalendarEventList Sort(CalendarEventList allEvents)
        {
            switch (SortParam)
            {
                case SortBy.start:
                    {
                        return allEvents.SortByStartDate(SortOrderAscending);
                    }
                case SortBy.status:
                    {
                        return allEvents.SortByStatus(SortOrderAscending);
                    }
                case SortBy.title:
                    {
                        return allEvents.SortByTitle(SortOrderAscending);
                    }
                case SortBy.location:
                    {
                        return allEvents.SortByLocation(SortOrderAscending);
                    }
                case SortBy.content:
                    {
                        return allEvents.SortByContent(SortOrderAscending);
                    }
                case SortBy.end:
                    {
                        return allEvents.SortByEndDate(SortOrderAscending);
                    }
                default:
                    {
                        return allEvents;
                    }
            }
        }
    }
}
