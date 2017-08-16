using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPBorne.Model
{
    class TimeTable
    {
        public TimeSpan OpeningHour { get; set; }

        public TimeSpan ClosingHour { get; set; }

        public int DayOfWeek { get; set; }
    }
}
