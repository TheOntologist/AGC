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
    public class SoundPreferences : ISoundPreferences, ISerializable 
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly string CONFIG = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\AGC\\" + "SoundPreferences.bin";

        private const int VERSION = 1;
        private const string DEFAULT_SUCCESS_SOUND = "success.wav";
        private const string DEFAULT_NEUTRAL_SOUND = "neutral.wav";
        private const string DEFAULT_ERROR_SOUND = "error.wav";
        private const string DEFAULT_DELETE_SOUND = "delete.wav";
        private static List<string> DEFAULT_SOUNDS = new List<string>(new string[] { 
            DEFAULT_SUCCESS_SOUND,
            DEFAULT_NEUTRAL_SOUND,
            DEFAULT_ERROR_SOUND,
            DEFAULT_DELETE_SOUND
        });

        public bool Enable { get; set; }
        public string Success { get; set; }
        public string Neutral { get; set; }
        public string Error { get; set; }
        public string Delete { get; set; }
        public List<string> Sounds { get; set; }


        //Default constructor
        public SoundPreferences()
        {
            Enable = true;
            Success = DEFAULT_SUCCESS_SOUND;
            Neutral = DEFAULT_NEUTRAL_SOUND;
            Error = DEFAULT_ERROR_SOUND;
            Delete = DEFAULT_DELETE_SOUND;
            Sounds = DEFAULT_SOUNDS;
        }

        //Deserialization constructor
        public SoundPreferences(SerializationInfo info, StreamingContext ctxt)
        {
            int version = (int)info.GetValue("Version", typeof(int));
            if (version == 0)
            {
                // Restore properties for version 0
                Enable = (bool)info.GetValue("Enable", typeof(bool));
                Success = (string)info.GetValue("Success", typeof(string));
                Neutral = (string)info.GetValue("Neutral", typeof(string));
                Error = (string)info.GetValue("Error", typeof(string));
                Delete = (string)info.GetValue("Delete", typeof(string));
                Sounds = DEFAULT_SOUNDS;
            }
            else if (version == 1)
            {
                Enable = (bool)info.GetValue("Enable", typeof(bool));
                Success = (string)info.GetValue("Success", typeof(string));
                Neutral = (string)info.GetValue("Neutral", typeof(string));
                Error = (string)info.GetValue("Error", typeof(string));
                Delete = (string)info.GetValue("Delete", typeof(string));
                Sounds = (List<string>)info.GetValue("Sounds", typeof(List<string>));
            }
        }

        //Serialization function.
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("Version", VERSION);
            info.AddValue("Enable", Enable);
            info.AddValue("Success", Success);
            info.AddValue("Neutral", Neutral);
            info.AddValue("Error", Error);
            info.AddValue("Delete", Delete);
            info.AddValue("Sounds", Sounds);
        }

        public bool Save()
        {
            try
            {
                log.Debug("Saving SoundPreferences...");
                Stream stream = File.Open(CONFIG, FileMode.Create);
                BinaryFormatter bformatter = new BinaryFormatter();
                bformatter.Serialize(stream, this);
                stream.Close();
                log.Debug("SoundPreferences were saved");
                return true;
            }
            catch (Exception ex)
            {
                log.Error("Failed to save SoundPreferences:", ex);
                return false;
            }
        }

        public SoundPreferences Load()
        {
            SoundPreferences preferences = new SoundPreferences();

            try
            {
                if (File.Exists(CONFIG))
                {
                    log.Debug("Loading SoundPreferences...");
                    Stream stream = File.Open(CONFIG, FileMode.Open);
                    BinaryFormatter bformatter = new BinaryFormatter();
                    preferences = (SoundPreferences)bformatter.Deserialize(stream);
                    stream.Close();
                    log.Debug("SoundPreferences were loaded");
                }
            }
            catch (Exception ex)
            {
                log.Error("Failed to load SoundPreferences:", ex);
            }
            return preferences;
        }
    }
}
