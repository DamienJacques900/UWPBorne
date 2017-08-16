using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace APICafeSuspendu.Models
{
    public class CharityReturnModel
    {
        
        private int? nbCoffeeOffered;
        [Required]
        public int? NbCoffeeOffered
        {
            get { return nbCoffeeOffered; }
            set
            {
                if (value > 0)
                    nbCoffeeOffered = value;
            }
        }

        
        private int? nbCoffeeConsumed;
        [Required]
        public int? NbCoffeeConsumed
        {
            get { return nbCoffeeConsumed; }
            set
            {
                if (value >= 0 && value <= NbCoffeeOffered)
                    nbCoffeeConsumed = value;
            }
        }

        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime OfferingTime { get; set; }

        [Required]
        public string ApplicationUserPersonEmail { get; set; }

        [Required]
        public string ApplicationUserCoffeeEmail { get; set; }
    }
}