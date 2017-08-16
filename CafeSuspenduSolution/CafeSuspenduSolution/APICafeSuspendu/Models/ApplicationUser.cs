using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace APICafeSuspendu.Models
{
    public class ApplicationUser : IdentityUser
    {

        //café

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

        public ICollection<TimeTable> TimeTables { get; set; }

        //person

        [MaxLength(30)]
        public string FirstName { get; set; }

        [MaxLength(30)]
        public string LastName { get; set; }


        //Rest of code is removed for brevity
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }

}