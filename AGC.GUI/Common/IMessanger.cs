using AGC.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGC.GUI.Common
{
    public interface IMessanger
    {
        SoundPreferences GetSoundPreferences();

        void SetSoundPreferences(SoundPreferences preferences);

        void PlaySound(string file);

        void Success(string info, bool alwaysShowMessageBox);

        void Neutral(string info, bool alwaysShowMessageBox);

        void Error(string err, bool alwaysShowMessageBox);

        void Delete(string info, bool alwaysShowMessageBox);
    }
}
