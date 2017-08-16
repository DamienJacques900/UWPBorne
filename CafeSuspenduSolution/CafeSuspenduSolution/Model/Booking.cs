using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Booking
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? BookingID { get; set; }

        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime DateBooking { get; set; }

        [Required]
        [MaxLength(20)]
        [MinLength(6)]
        public string Name { get; set; }

        [Required]
        public string UserCafeId { get; set; }

        [ForeignKey("UserCafeId")]
        public UserCafe UserCafe { get; set; }

        [Required]
        public int? TerminalId { get; set; }

        [ForeignKey("TerminalId")]
        public Terminal Terminal { get; set; }
    }
}