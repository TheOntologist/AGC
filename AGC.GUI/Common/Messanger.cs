using AGC.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using System.Windows;

namespace AGC.GUI.Common
{
    public class Messanger : IMessanger
    {
        private const string MP3 = "mp3";
        private const string WAV = "wav";

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static SoundPreferences soundPreferences = new SoundPreferences().Load();
        private static string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\AGC\Sounds\";
        private bool SoundsOn = soundPreferences.Enable;

        public SoundPreferences GetSoundPreferences()
        {
            return soundPreferences;
        }

        public void SetSoundPreferences(SoundPreferences preferences)
        {
            soundPreferences = preferences;
            SoundsOn = preferences.Enable;
        }

        public void PlaySound(string file)
        {
            try
            {
                string format = GetAudioFormat(file);

                if (format == MP3)
                {
                    WMPLib.WindowsMediaPlayer wplayer = new WMPLib.WindowsMediaPlayer();
                    wplayer.URL = file;
                    wplayer.controls.play();
                }
                else if (format == WAV)
                {
                    SoundPlayer player = new SoundPlayer(file);
                    player.Play();
                }
                else
                {
                    log.Error("Cannot play sound, unkown file format.");
                }
            }
            catch (Exception ex)
            {
                log.Error("Failed to play success sound:", ex);
            }
        }

        public void Success(string info, bool alwaysShowMessageBox)
        {
            if (SoundsOn && !alwaysShowMessageBox)
            {
                PlaySound(appdata + soundPreferences.Success);
            }
            else
            {
                MessageBox.Show(Application.Current.MainWindow, info, "Information", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
            }
        }

        public void Neutral(string info, bool alwaysShowMessageBox)
        {
            if (SoundsOn && !alwaysShowMessageBox)
            {
                PlaySound(appdata + soundPreferences.Neutral);
            }
            else
            {
                MessageBox.Show(Application.Current.MainWindow, info, "Information", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
            }
        }

        public void Error(string err, bool alwaysShowMessageBox)
        {
            if (SoundsOn && !alwaysShowMessageBox)
            {
                PlaySound(appdata + soundPreferences.Error);
            }
            else
            {
                MessageBox.Show(Application.Current.MainWindow, err, "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
        }

        public void Delete(string info, bool alwaysShowMessageBox)
        {
            if (SoundsOn && !alwaysShowMessageBox)
            {
                PlaySound(appdata + soundPreferences.Delete);
            }
            else
            {
                MessageBox.Show(Application.Current.MainWindow, info, "Information", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
            }
        }

        private string GetAudioFormat(string file)
        {
            string[] parts = file.Split('.');
            return parts[parts.Count() - 1];
        }
    }
}
