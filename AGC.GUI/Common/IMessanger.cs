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

        void Success(string info);

        void Neutral(string info);

        void Error(string err);

        void Delete(string info);
    }
}
