using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class TimeTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? TimeTableID { get; set; }

        private TimeSpan openingHour;
        public TimeSpan OpeningHour
        {
             get { return openingHour; }
             set
             {
                 if (ClosingHour != null)
                 {
                     if (TimeSpan.Compare(value, ClosingHour) == -1)
                         openingHour = value;
                 }
                 else
                     openingHour = value;
             }
        }

        private TimeSpan closingHour;
        public TimeSpan ClosingHour
        {
             get { return closingHour; }
             set
             {
                 if (OpeningHour != null)
                 {
                     if (TimeSpan.Compare(value, OpeningHour) == 1)
                         closingHour = value;
                 }
                 else
                     closingHour = value;
             }
        }

        [Required]
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

        public string UserCafeId { get; set; }

        [ForeignKey("UserCafeId")]
        public UserCafe UserCafe { get; set; }


    }
}