using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using APICafeSuspendu.Models;
using System.Data.SqlClient;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace APICafeSuspendu.Controllers
{
    public class BookingsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Bookings
        public IEnumerable<Booking> GetBookings()
        {
             return db.Bookings.Include(b => b.ApplicationUser).ToList(); //Inclut les utilisateurs liés à la réservation
        }

        //POST: api/Bookings
        [ResponseType(typeof(Booking))]
        public async Task<IHttpActionResult> PostBooking(PushBookingInformations informations)
        {
            //Récupération du terminal concerné en fonction de son ID ou exception
            Terminal terminal = db.Terminals.Where(t => t.TerminalId == informations.TerminalId).Single();

            //Récupération du café concerné en fonction de son nom ou exception
            ApplicationUser cafe = db.Users.Where(u => u.CafeName == informations.CafeName).Single();

            //Ajout de la réservation
            Booking newBooking = new Booking()
                {
                    DateBooking = DateTime.Now,
                    Name = informations.BookingName,
                    ApplicationUser = cafe,
                    Terminal = terminal

                };
            db.Bookings.Add(newBooking);


            Terminal updatedTerminal = terminal;
            updatedTerminal.NbBookedCoffees++;
            db.Entry(terminal).CurrentValues.SetValues(updatedTerminal);

            bool charityModified = false;

            var charities = db.Charities.Where(c => c.ApplicationUserCoffee.Id == cafe.Id && c.NbCoffeeConsumed < c.NbCoffeeOffered).Include(c => c.ApplicationUserCoffee).Include(c => c.ApplicationUserPerson).ToArray(); //Linq defferred execution

            foreach(Charity charity in charities)
            {
                try
                {
                    //méthode tryToUpdateDatabase
                    Charity updatedCharity = charity;
                    updatedCharity.NbCoffeeConsumed++;
                    db.Entry(charity).CurrentValues.SetValues(updatedCharity);
                    await db.SaveChangesAsync();
                    charityModified = true;
                    break;
                }
                catch(Exception e)
                {
                    continue;
                }
            }

            if (charityModified)
            {
                return Ok(newBooking);
            }
            else
            {
                throw new DBConcurrencyException(); //si il y a un une édition concurrente pour TOUTES les charity avec un café encore disponible pour l'établissement donné
            }
        }

        //private void tryToUpdateDatabase

        

        // DELETE: api/Bookings/5
        [ResponseType(typeof(Booking))]
        [Authorize(Roles = "usercafe")]
        public async Task<IHttpActionResult> DeleteBooking([FromUri] int id, [FromUri] bool isCoffeeConsumed)
        {
            //Booking booking = await db.Bookings.FindAsync(id);
            Booking booking = db.Bookings.Where(b => b.BookingID == id).Include(b => b.ApplicationUser).Single();

            if (booking == null)
            {
                return NotFound();
            }

            ApplicationUser test = booking.ApplicationUser;


            if (!isCoffeeConsumed)
            {
                var charities = db.Charities.Where(c => c.ApplicationUserCoffee.Id == booking.ApplicationUser.Id).Include(c => c.ApplicationUserPerson);

                foreach (Charity charity in charities)
                {
                    if (charity.NbCoffeeConsumed > 0)
                    {
                        Charity updatedCharity = charity;
                        updatedCharity.NbCoffeeConsumed--;
                        db.Entry(charity).CurrentValues.SetValues(updatedCharity);
                        break;
                    }

                }
            }

            db.Bookings.Remove(booking);

            await db.SaveChangesAsync();

            return Ok(booking);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BookingExists(int id)
        {
            return db.Bookings.Count(e => e.BookingID == id) > 0;
        }
    }
}