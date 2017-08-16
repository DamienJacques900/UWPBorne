using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace APICafeSuspendu.Models
{
    public class TimeTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? TimeTableID { get; set; }

        public TimeSpan OpeningHour { get; set; }
        public TimeSpan ClosingHour { get; set; }
        
        private int? dayOfWeek;
        [Required]
        public int? DayOfWeek
        {
            get { return dayOfWeek; }
            set
            {
                if (value > 0 && value <= 7)
                    dayOfWeek = value;
            }
        }

        [Required]
        [JsonIgnore]
        public ApplicationUser ApplicationUser { get; set; }
    }
}