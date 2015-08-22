using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGC.Library
{
    public class UserCalendar
    {
        public UserCalendar(string id, string name, bool primary)
        {
            this.Id = id;
            this.Name = name;
            this.IsVisible = true;
            this.IsPrimary = primary;
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public bool IsVisible { get; set; }

        public bool IsPrimary { get; set; }
    }
}
