using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGC.Library
{
    public interface IUserCalendarsPreferences
    {
        bool Save();

        UserCalendarsPreferences Load();
    }
}
