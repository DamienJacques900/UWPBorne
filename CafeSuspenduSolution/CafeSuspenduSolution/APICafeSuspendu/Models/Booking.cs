using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace APICafeSuspendu.Models
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
        public ApplicationUser ApplicationUser{ get; set; }

        [Required]
        public Terminal Terminal { get; set; }
    }
}