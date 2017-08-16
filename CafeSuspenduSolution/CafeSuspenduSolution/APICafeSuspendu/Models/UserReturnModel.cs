using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace APICafeSuspendu.Models
{
    public class UserReturnModel
    {
        public string Url { get; set; }
        public string Id { get; set; }
        public string UserName { get; set; }
        [MaxLength(30)]
        public string CafeName { get; set; }
        [MaxLength(50)]
        public string Street { get; set; }
        [MaxLength(5)]
        public string Number { get; set; }
        [Range(0, 100)]
        public int? NbCoffeeRequiredForPromotion { get; set; }
        [Range(0.0, 100.0)]
        public double PromotionValue { get; set; }

        public ICollection<Booking> Bookings { get; set; }
        public ICollection<TimeTableReturnModel> TimeTables { get; set; }

        [MaxLength(30)]
        public string FirstName { get; set; }
        [MaxLength(30)]
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }

        public IList<string> Roles { get; set; }
        public IList<System.Security.Claims.Claim> Claims { get; set; }
    }
}