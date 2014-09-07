using AGC.GUI.Common;
using AGC.Library;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Media;

namespace AGC.GUI.ViewModel
{
    public class SoundsViewModel : ViewModelBase
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IMessanger messanger;
        private SoundPreferences soundPreferences;
        private static string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\AGC\Sounds\";

        private const string SUCCESS_TYPE = "Success";
        private const string NEUTRAL_TYPE = "Neutral";
        private const string ERROR_TYPE = "Error";
        private const string DELETE_TYPE = "Delete";
        private static List<string> SOUND_TYPES = new List<string>(new string[] { 
            SUCCESS_TYPE,
            NEUTRAL_TYPE,
            ERROR_TYPE,
            DELETE_TYPE
        });

        public RelayCommand PlaySoundCommand { get; private set; }
        public RelayCommand SetSoundCommand { get; private set; }
        public RelayCommand DeleteSoundCommand { get; private set; }
        public RelayCommand AddNewSoundCommand { get; private set; }

        public SoundsViewModel(IMessanger commonMessanger)
        {
            messanger = commonMessanger;
            soundPreferences = messanger.GetSoundPreferences();
            EnableSounds = soundPreferences.Enable;

            PlaySoundCommand = new RelayCommand(PlaySound);
            SetSoundCommand = new RelayCommand(SetSound);
            DeleteSoundCommand = new RelayCommand(DeleteSound);
            AddNewSoundCommand = new RelayCommand(AddSound);

            LoadData();
        }

        #region Sound Preferences

        public const string EnableSoundsPropertyName = "EnableSounds";
        private bool _enableSounds = false;
        public bool EnableSounds
        {
            get
            {
                return _enableSounds;
            }

            set
            {
                if (_enableSounds == value)
                {
                    return;
                }

                RaisePropertyChanging(EnableSoundsPropertyName);
                _enableSounds = value;
                RaisePropertyChanged(EnableSoundsPropertyName);

                soundPreferences.Enable = value;
                soundPreferences.Save();
                messanger.SetSoundPreferences(soundPreferences);
                messanger.Success(value ? "Sound enabled" : "Sound disabled", false);
            }
        }

        public const string SoundTypeListPropertyName = "SoundTypeList";
        private List<string> _soundTypeList = SOUND_TYPES;
        public List<string> SoundTypeList
        {
            get
            {
                return _soundTypeList;
            }

            set
            {
                if (_soundTypeList == value)
                {
                    return;
                }

                RaisePropertyChanging(SoundTypeListPropertyName);
                _soundTypeList = value;
                RaisePropertyChanged(SoundTypeListPropertyName);
            }
        }

        public const string SelectedSoundTypePropertyName = "SelectedSoundType";
        private string _selectedSoundType = SUCCESS_TYPE;
        public string SelectedSoundType
        {
            get
            {
                return _selectedSoundType;
            }

            set
            {
                if (_selectedSoundType == value)
                {
                    return;
                }

                RaisePropertyChanging(SelectedSoundTypePropertyName);
                _selectedSoundType = value;
                RaisePropertyChanged(SelectedSoundTypePropertyName);
                SelectedSoundTypeChanged();
            }
        }

        public const string SoundsListPropertyName = "SoundsList";
        private ObservableCollection<string> _soundsList = new ObservableCollection<string>();
        public ObservableCollection<string> SoundsList
        {
            get
            {
                return _soundsList;
            }

            set
            {
                if (_soundsList == value)
                {
                    return;
                }

                RaisePropertyChanging(SoundsListPropertyName);
                _soundsList = value;
                RaisePropertyChanged(SoundsListPropertyName);
            }
        }

        public const string SelectedSoundPropertyName = "SelectedSound";
        private string _selectedSound = string.Empty;
        public string SelectedSound
        {
            get
            {
                return _selectedSound;
            }

            set
            {
                if (_selectedSound == value)
                {
                    return;
                }

                RaisePropertyChanging(SelectedSoundPropertyName);
                _selectedSound = value;
                RaisePropertyChanged(SelectedSoundPropertyName);
            }
        }

        #endregion

        private void LoadData()
        {
            foreach (var item in soundPreferences.Sounds)
            {
                SoundsList.Add(item);
            }

            SelectedSoundTypeChanged();
        }

        private void SelectedSoundTypeChanged()
        {
            switch (SelectedSoundType)
            {
                case SUCCESS_TYPE:
                    {
                        SelectedSound = soundPreferences.Success;
                        break;
                    }
                case NEUTRAL_TYPE:
                    {
                        SelectedSound = soundPreferences.Neutral;
                        break;
                    }
                case ERROR_TYPE:
                    {
                        SelectedSound = soundPreferences.Error;
                        break;
                    }
                case DELETE_TYPE:
                    {
                        SelectedSound = soundPreferences.Delete;
                        break;
                    }
            }
        }

        private void PlaySound()
        {
            messanger.PlaySound(appdata + SelectedSound);
        }

        private void SetSound()
        {
            switch (SelectedSoundType)
            {
                case SUCCESS_TYPE:
                    {
                        soundPreferences.Success = SelectedSound;
                        break;
                    }
                case NEUTRAL_TYPE:
                    {
                        soundPreferences.Neutral = SelectedSound;
                        break;
                    }
                case ERROR_TYPE:
                    {
                        soundPreferences.Error = SelectedSound;
                        break;
                    }
                case DELETE_TYPE:
                    {
                        soundPreferences.Delete = SelectedSound;
                        break;
                    }
            }
            soundPreferences.Save();
            messanger.Success("New " + SelectedSoundType + " sound is " + SelectedSound, false);
        } 

        private void DeleteSound()
        {
            // Check that sound is not in use
            if (SelectedSound == soundPreferences.Success)
            {
                messanger.Error("Cannot delete sound file as it is used for Success sound", true);
            }
            else if(SelectedSound == soundPreferences.Neutral)
            {
                messanger.Error("Cannot delete sound file as it is used for Neutral sound", true);
            }
            else if (SelectedSound == soundPreferences.Error)
            {
                messanger.Error("Cannot delete sound file as it is used for Error sound", true);
            }
            else if (SelectedSound == soundPreferences.Delete)
            {
                messanger.Error("Cannot delete sound file as it is used for Delete sound", true);
            }
            else
            {
                // Delete sound
                soundPreferences.Sounds.Remove(SelectedSound);
                soundPreferences.Save();
                if (File.Exists(appdata + SelectedSound))
                {
                    File.Delete(appdata + SelectedSound);
                }                
                SoundsList.Remove(SelectedSound);
                SelectedSound = SoundsList.Count > 0 ? SoundsList[0] : string.Empty;
                messanger.Delete("Deleted", false);
            }
        }

        private void AddSound()
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".mp3";
            dlg.Filter = "Audio Files|*.mp3;*.wav;*.wmp"; ;

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name
            if (result == true)
            {
                // Copy sound file to AppData Sounds folder
                File.Copy(dlg.FileName, appdata + dlg.SafeFileName, true);

                // Add new sound file to sound preferences list
                soundPreferences.Sounds.Add(dlg.SafeFileName);
                soundPreferences.Save();
                SoundsList.Add(dlg.SafeFileName);
                messanger.Success("Added", false);
            }
        }

    }
}