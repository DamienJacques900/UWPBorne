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

namespace APICafeSuspendu.Controllers
{
    public class CharitiesController : ApiController
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Charities
        public IEnumerable<Charity> GetCharities()
        {
            return db.Charities.Include(c => c.ApplicationUserCoffee).Include(c => c.ApplicationUserPerson).ToList(); //Voir dataReader
        }

        // GET: api/Charities/5
        [ResponseType(typeof(Charity))]
        public async Task<IHttpActionResult> GetCharity(int id)
        {
            Charity charity = await db.Charities.FindAsync(id);
            if (charity == null)
            {
                return NotFound();
            }

            return Ok(charity);
        }

        private List<Charity> GetAvailableCharities(List<Charity> allCharities)
        {
            List<Charity> availableCharities = new List<Charity>();
            foreach(Charity charity in allCharities)
            {
                if (charity.NbCoffeeConsumed < charity.NbCoffeeOffered)
                    availableCharities.Add(charity);
            }

            return availableCharities;
        }

        private List<TimeTable> GetTimeTableForCafe(string userID)
        {
            var users = db.Users.Include(u => u.TimeTables);
            var rightUser = users.Where(u => u.Id == userID).Single();

            return rightUser.TimeTables.ToList();
        }

        [Route("api/Charities/CafeWithAvailableCharities")]
        [HttpGet]
        public IEnumerable<CoffeesWithCharities> GetCafeWithAvailableCharities()
        {
            var coffees = db
                .Charities
                .Include(c => c.ApplicationUserCoffee)
               .GroupBy(d => d.ApplicationUserCoffee);

            List<CoffeesWithCharities> cafeList = new List<CoffeesWithCharities>();

            foreach(IGrouping<ApplicationUser, Charity> userAndCharities in coffees)
            {
                List<Charity> availableCharities = GetAvailableCharities(userAndCharities.ToList());
                if (availableCharities.Count() != 0)
                {
                    cafeList.Add(new CoffeesWithCharities()
                    {
                        Cafe = userAndCharities.Key,
                        Charities = userAndCharities.ToList()

                    });
                }
            }

            foreach (CoffeesWithCharities userWithCharities in cafeList)
            {
                userWithCharities.Cafe.TimeTables = GetTimeTableForCafe(userWithCharities.Cafe.Id);
            }

            return cafeList;
        }


        // PUT: api/Charities/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCharity(int id, Charity charity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != charity.CharityId)
            {
                return BadRequest();
            }

            db.Entry(charity).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CharityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Charities
        [ResponseType(typeof(Charity))]
        [Authorize(Roles = "usercafe")]
        public async Task<IHttpActionResult> PostCharity(CharityReturnModel charityReturnModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ApplicationUser userCafe = await db.Users.FirstOrDefaultAsync(u => u.Email == charityReturnModel.ApplicationUserCoffeeEmail);
            ApplicationUser userPerson = await db.Users.FirstOrDefaultAsync(u => u.Email == charityReturnModel.ApplicationUserPersonEmail);


            Charity charity = new Models.Charity()
            {
                NbCoffeeOffered = charityReturnModel.NbCoffeeOffered,
                NbCoffeeConsumed = charityReturnModel.NbCoffeeConsumed,
                OfferingTime = charityReturnModel.OfferingTime,
                ApplicationUserCoffee = userCafe,
                ApplicationUserPerson = userPerson
            };

            db.Charities.Add(charity);

            await db.SaveChangesAsync();
            

            return CreatedAtRoute("DefaultApi", new { id = charity.CharityId }, charity);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CharityExists(int id)
        {
            return db.Charities.Count(e => e.CharityId == id) > 0;
        }
    }
}