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
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static SoundPreferences soundPreferences = new SoundPreferences().Load();
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


        public void Success(string info)
        {
            if (SoundsOn)
            {
                PlaySuccessSound();
            }
            else
            {
                MessageBox.Show(Application.Current.MainWindow, info, "Information", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
            }
        }

        public void Neutral(string info)
        {
            if (SoundsOn)
            {
                PlayNeutralSound();
            }
            else
            {
                MessageBox.Show(Application.Current.MainWindow, info, "Information", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
            }
        }

        public void Error(string err)
        {
            if (SoundsOn)
            {
                PlayErrorSound();
            }
            else
            {
                MessageBox.Show(Application.Current.MainWindow, err, "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
        }

        public void Delete(string info)
        {
            if (SoundsOn)
            {
                PlayDeleteSound();
            }
            else
            {
                MessageBox.Show(Application.Current.MainWindow, info, "Information", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
            }
        }

        private void PlaySuccessSound()
        {
            try
            {
                SoundPlayer player = new SoundPlayer(@"Sounds\" + soundPreferences.Success);
                player.Play();
            }
            catch (Exception ex)
            {
                log.Error("Failed to play success sound:", ex);
            }
        }

        private void PlayNeutralSound()
        {
            try
            {
                SoundPlayer player = new SoundPlayer(@"Sounds\" + soundPreferences.Neutral);
                player.Play();
            }
            catch (Exception ex)
            {
                log.Error("Failed to play neutral sound:", ex);
            }
        }

        private void PlayErrorSound()
        {
            try
            {
                SoundPlayer player = new SoundPlayer(@"Sounds\" + soundPreferences.Error);
                player.Play();
            }
            catch (Exception ex)
            {
                log.Error("Failed to play error sound:", ex);
            }
        }

        private void PlayDeleteSound()
        {
            try
            {
                SoundPlayer player = new SoundPlayer(@"Sounds\" + soundPreferences.Delete);
                player.Play();
            }
            catch (Exception ex)
            {
                log.Error("Failed to play delete sound:", ex);
            }
        }
    }
}
