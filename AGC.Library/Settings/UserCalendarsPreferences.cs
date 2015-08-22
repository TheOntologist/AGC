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
    public class UserCalendarsPreferences : IUserCalendarsPreferences, ISerializable
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly string CONFIG = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\AGC\\" + "UserCalendarsPreferences.bin";

        private const int VERSION = 0;

        public List<UserCalendar> UserCalendars { get; set; }

        public bool ShowEmptyDays { get; set; }

        public bool ShowEmptyWeekends { get; set; }

        public UserCalendarsPreferences()
        {
            this.UserCalendars = new List<UserCalendar>();
            this.ShowEmptyDays = true;
            this.ShowEmptyWeekends = true;
        }

        //Deserialization constructor
        public UserCalendarsPreferences(SerializationInfo info, StreamingContext ctxt)
        {
            int version = (int)info.GetValue("Version", typeof(int));
            if (version == 0)
            {
                // Restore properties for version 0
                UserCalendars = (List<UserCalendar>)info.GetValue("UserCalendars", typeof(List<UserCalendar>));
                ShowEmptyDays = (bool)info.GetValue("ShowEmptyDays", typeof(bool));
                ShowEmptyWeekends = (bool)info.GetValue("ShowEmptyWeekends", typeof(bool));
            }
        }

        //Serialization function.
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("Version", VERSION);
            info.AddValue("UserCalendars", UserCalendars);
            info.AddValue("ShowEmptyDays", ShowEmptyDays);
            info.AddValue("ShowEmptyWeekends", ShowEmptyWeekends);
        }

        public bool Save()
        {
            try
            {
                log.Debug("Saving UserCalendarsPreferences...");
                Stream stream = File.Open(CONFIG, FileMode.Create);
                BinaryFormatter bformatter = new BinaryFormatter();
                bformatter.Serialize(stream, this);
                stream.Close();
                log.Debug("UserCalendarsPreferences were saved");
                return true;
            }
            catch(Exception ex)
            {
                log.Error("Failed to save UserCalendarsPreferences:", ex);
                return false;
            }
        }

        public UserCalendarsPreferences Load()
        {
            UserCalendarsPreferences calendars = new UserCalendarsPreferences();

            try
            {
                if (File.Exists(CONFIG))
                {
                    log.Debug("Loading UserCalendarsPreferences...");
                    Stream stream = File.Open(CONFIG, FileMode.Open);
                    BinaryFormatter bformatter = new BinaryFormatter();
                    calendars = (UserCalendarsPreferences)bformatter.Deserialize(stream);
                    stream.Close();
                    log.Debug("UserCalendarsPreferences were loaded");
                }
            }
            catch (Exception ex)
            {
                log.Error("Failed to load UserCalendarsPreferences:", ex);
            }
            return calendars;
        }
    }
}
