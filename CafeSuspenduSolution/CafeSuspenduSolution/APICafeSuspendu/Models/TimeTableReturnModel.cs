using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace APICafeSuspendu.Models
{
    public class TimeTableReturnModel
    {
        public int? TimeTableID { get; set; }

        public TimeSpan OpeningHour { get; set; }

        public TimeSpan ClosingHour { get; set; }

        [Required]
        public int? DayOfWeek { get; set; }
    }
}