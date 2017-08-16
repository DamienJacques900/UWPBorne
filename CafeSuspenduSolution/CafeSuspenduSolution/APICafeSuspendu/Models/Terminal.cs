using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace APICafeSuspendu.Models
{
    public class Terminal
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? TerminalId { get; set; }

        
        private int? nbBookedCoffees;
        [Required]
        public int? NbBookedCoffees
        {
            get { return nbBookedCoffees; }
            set
            {
                if (value >= 0)
                    nbBookedCoffees = value;
            }

        }

        
        private double latitude;
        [Required]
        public double Latitude
        {
            get { return latitude; }
            set
            {
                if (value >= -90 && value <= 90)
                    latitude = value;
            }
        }

        
        private double longitude;
        [Required]
        public double Longitude
        {
            get { return longitude; }
            set
            {
                if (value >= -180 && value <= 180)
                    longitude = value;
            }
        }

        [Required]
        public ICollection<Booking> Bookings { get; set; }
    }
}