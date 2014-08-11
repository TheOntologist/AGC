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
    public class QuickEventsTemplates : IQuickEventsTemplates, ISerializable
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const int VERSION = 0;

        // Default templates
        private const string DAILY_RECURRING_EVENT_SPECIFIED_START_AND_END_DATES = "TV Series August 5 daily until August 20";
        private const string ONE_HOUR_EVENT_NEXT_AVAILABLE_TIME = "Meeting at 5 pm";
        private const string FULL_DAY_EVENT_ON_SPECIFIC_DATE = "Day off 2/8";
        private const string SEVERAL_HOURS_EVENT_NEXT_WEEKDAY = "Lectures 9:00 - 12:00 next Monday";
        private const string SEVERAL_DAYS_EVENT_IN_SPECIFIC_LOCATION = "Vacation 9/23 - 9/26 in Spain";
        private const string RECURRING_EVENT_ON_WEEKDAY_FOR_ONE_HOUR = "Meeting at work every Monday at 2pm";
        private const string DAILY_RECURRING_EVENT_FOR_TEN_TIMES = "Lunch every day 1-2 pm for 10 days";

        private static List<string> DEFAULT_TEMPLATES = new List<string>(new string[] { 
            DAILY_RECURRING_EVENT_SPECIFIED_START_AND_END_DATES,
            ONE_HOUR_EVENT_NEXT_AVAILABLE_TIME,
            FULL_DAY_EVENT_ON_SPECIFIC_DATE,
            SEVERAL_HOURS_EVENT_NEXT_WEEKDAY,
            SEVERAL_DAYS_EVENT_IN_SPECIFIC_LOCATION,
            RECURRING_EVENT_ON_WEEKDAY_FOR_ONE_HOUR,
            DAILY_RECURRING_EVENT_FOR_TEN_TIMES
        });

        public List<string> Templates { get; set; }

        public QuickEventsTemplates()
        {
            this.Templates = DEFAULT_TEMPLATES;
        }

         //Deserialization constructor
        public QuickEventsTemplates(SerializationInfo info, StreamingContext ctxt)
        {
            int version = (int)info.GetValue("Version", typeof(int));
            if (version == 0)
            {
                // Restore properties for version 0
                Templates = (List<string>)info.GetValue("Templates", typeof(List<string>));
            }
        }

        //Serialization function.
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("Version", VERSION);
            info.AddValue("Templates", Templates);
        }

        public bool Save()
        {
            try
            {
                log.Debug("Saving QuickEventsTemplates...");
                Stream stream = File.Open("QuickEventsTemplates.config", FileMode.Create);
                BinaryFormatter bformatter = new BinaryFormatter();
                bformatter.Serialize(stream, this);
                stream.Close();
                log.Debug("QuickEventsTemplates were saved");
                return true;
            }
            catch(Exception ex)
            {
                log.Error("Failed to save QuickEventsTemplates:", ex);
                return false;
            }
        }

        public QuickEventsTemplates Load()
        {
            QuickEventsTemplates templates = new QuickEventsTemplates();

            try
            {
                if (File.Exists("QuickEventsTemplates.config"))
                {
                    log.Debug("Loading QuickEventsTemplates...");
                    Stream stream = File.Open("QuickEventsTemplates.config", FileMode.Open);
                    BinaryFormatter bformatter = new BinaryFormatter();
                    templates = (QuickEventsTemplates)bformatter.Deserialize(stream);
                    stream.Close();
                    log.Debug("QuickEventsTemplates were loaded");
                }
            }
            catch (Exception ex)
            {
                log.Error("Failed to load QuickEventsTemplates:", ex);
            }
            return templates;
        }
    }
}
