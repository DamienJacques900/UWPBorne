using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Charity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? CharityId { get; set; }

        [Required]
        private int? nbCoffeeOffered;
        public int? NbCoffeeOffered
        {
              get { return nbCoffeeOffered; }
              set
              {
                  if (value > 0)
                      nbCoffeeOffered = value;
              }
        }

        [Required]
        private int? nbCoffeeConsumed;
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

        [Timestamp]
        public byte[] RowVersion { get; set; }

        [Required]
        public string UserPersonId { get; set; }

        [ForeignKey("UserPersonId")]
        public UserPerson UserPerson { get; set; }

        //[Required]
        //public string UserCafeId { get; set; }

        //[ForeignKey("UserCafeId")]
        public UserCafe UserCafe { get; set; }

    }
}