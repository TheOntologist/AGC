using AGC.GUI.Common;
using AGC.Library;
using GalaSoft.MvvmLight;

namespace AGC.GUI.ViewModel
{
    public class SoundsViewModel : ViewModelBase
    {
        private readonly IMessanger messanger;
        private SoundPreferences soundPreferences;

        public SoundsViewModel(IMessanger commonMessanger)
        {
            messanger = commonMessanger;
            soundPreferences = messanger.GetSoundPreferences();
            EnableSounds = soundPreferences.Enable;
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
                messanger.Success(value ? "Sound enabled" : "Sound disabled");
            }
        }

        #endregion
    }
}