using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APICafeSuspendu.Models
{
    public class RegistrationTimeTable
    {

        public TimeSpan OpeningHour { get; set; }
        public TimeSpan ClosingHour { get; set; }
        private int? dayOfWeek;
        public int? DayOfWeek
        {
            get { return dayOfWeek; }
            set
            {
                if (value > 0 && value <= 7)
                    dayOfWeek = value;
            }
        }
    }
}