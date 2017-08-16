using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Terminal
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? TerminalId { get; set; }

        [Required]
        private int? nbBookedCoffees;
        public int? NbBookedCoffees
        {
            get { return nbBookedCoffees; }
            set
            {
                if (value >= 0)
                    nbBookedCoffees = value;
            }

        }

        [Required]
        private double latitude;
        public double Latitude
        {
             get { return latitude; }
             set
             {
                 if (value >= -90 && value <= 90)
                     latitude = value;
             }
        }

        [Required]
        private double longitude;
        public double Longitude
        {
            get { return longitude; }
            set
            {
                if (value >= -180 && value <= 180)
                    longitude = value;
            }
        }

        public ICollection<Booking> Bookings { get; set; }

    }
}