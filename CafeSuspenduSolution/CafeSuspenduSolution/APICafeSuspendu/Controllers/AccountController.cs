using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using APICafeSuspendu.Models;
using APICafeSuspendu.Providers;
using APICafeSuspendu.Results;
using System.Linq;
using System.Data.Entity;
using System.Web.Http.Description;
using System.Net.Mail;
using System.Text.RegularExpressions;
using APICafeSuspendu.Exceptions;

namespace APICafeSuspendu.Controllers
{
    [RoutePrefix("api/accounts")]

    //Username=Antoni&Password=AntoniMDP1&grant_type=password -> token
    public class AccountsController : BaseApiController
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        [AllowAnonymous]
        [Route("users")]
        public IHttpActionResult GetUsers()
        {
            return Ok(this
                .AppUserManager.Users
                .Include(u=>u.TimeTables) //inclut les horaires des cafés
                .ToList()
                .Select(u => this.TheModelFactory.Create(u)));
        }

        [AllowAnonymous]
        [Route("user/{id:guid}", Name = "GetUserById")]
        public async Task<IHttpActionResult> GetUser(string Id)
        {
            var user = await this.AppUserManager.FindByIdAsync(Id);

            if (user != null)
            {
                return Ok(this.TheModelFactory.Create(user));
            }

            return NotFound();

        }


        [AllowAnonymous]
        [Route("user/{username}")]
        public async Task<IHttpActionResult> GetUserByName(string username)
        {
            var user = await this.AppUserManager.FindByNameAsync(username);

            if (user != null)
            {
                return Ok(this.TheModelFactory.Create(user));
            }

            return NotFound();

        }

        [AllowAnonymous]
        [Route("create")]
        public async Task<IHttpActionResult> CreateUser(CreateUserBindingModel createUserModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var roles = db.Roles;

            foreach (string role in createUserModel.Roles)
            {
                var existingRole = roles.Where(r => r.Name == role).Single(); //renvoie une exception si un des rôles passés n'existe pas
            }

            var user = new ApplicationUser()
            {
                UserName = createUserModel.Username,
                Email = createUserModel.Email,
                CafeName = createUserModel.CafeName,
                Street = createUserModel.Street,
                Number = createUserModel.Number,
                NbCoffeeRequiredForPromotion = createUserModel.NbCoffeeRequiredForPromotion,
                PromotionValue = createUserModel.PromotionValue,
                Bookings = createUserModel.Bookings,
                TimeTables = new List<TimeTable>(),
                FirstName = createUserModel.FirstName,
                LastName = createUserModel.LastName,
                PhoneNumber = createUserModel.PhoneNumber,
            };

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            IdentityResult addUserResult = await this.AppUserManager.CreateAsync(user, createUserModel.Password);

            user = db.Users.Where(u => u.UserName == createUserModel.Username).Single();

            foreach (RegistrationTimeTable table in createUserModel.TimeTables)
            {
                var timeTable = new TimeTable()
                {
                    OpeningHour = table.OpeningHour,
                    ClosingHour = table.ClosingHour,
                    DayOfWeek = table.DayOfWeek,
                    ApplicationUser = user
                };

                db.TimeTables.Add(timeTable);
                user.TimeTables.Add(timeTable);
            }

            if (!addUserResult.Succeeded)
            {
                return GetErrorResult(addUserResult);
            }

            foreach (string role in createUserModel.Roles)
            {
                var addToRoleResult = userManager.AddToRole(
                 user.Id, role);
            }


            await db.SaveChangesAsync();


            return Ok(user);
        }

        [Route("getNbCoffeeForPerson")]
        [Authorize(Roles = "userperson")]
        public async Task<IHttpActionResult> GetNbCoffeeForPerson([FromUri]string userName)
        {
            var store = new RoleStore<IdentityRole>(db);
            var roleManager = new RoleManager<IdentityRole>(store);

            var user = db.Users.Where(u => u.UserName == userName).Single();
            var cafe = roleManager.FindByName("userperson").Users.Where(e => e.UserId == user.Id).Single(); //retourne une exception si le rôle de l'utilisateur n'est pas "userperson"

            var charities = db.Charities.Where(c => c.ApplicationUserPerson.Id == user.Id).ToArray();

            int? nbOfferedCofee = 0;

            foreach(Charity charity in charities)
            {
                nbOfferedCofee += charity.NbCoffeeOffered;
            }

            List<int?> list = new List<int?>();

            list.Add(nbOfferedCofee);

            return Ok(list);
        }
         
        [Route("getNbCoffeeForCafe")]
        [Authorize(Roles = "usercafe")]
        public async Task<IHttpActionResult> GetNbCoffeeForCafe([FromUri]string userName)
        {
            var store = new RoleStore<IdentityRole>(db);
            var roleManager = new RoleManager<IdentityRole>(store);

            var user = db.Users.Where(u => u.UserName == userName).Single();
            var cafe = roleManager.FindByName("usercafe").Users.Where(e => e.UserId == user.Id).Single(); //retourne une exception si le rôle de l'utilisateur n'est pas "usercafe"

            var charities = db.Charities.Where(c => c.ApplicationUserCoffee.Id == user.Id).ToArray();

            int? nbOfferedCofee = 0;

            foreach (Charity charity in charities)
            {
                nbOfferedCofee += charity.NbCoffeeOffered;
            }

            List<int?> list = new List<int?>();

            list.Add(nbOfferedCofee);

            return Ok(list);
        }

        [Route("updatePromotionInformations")]
        [Authorize(Roles = "usercafe")]
        [HttpPut]
        public async Task<IHttpActionResult> UpdatePromotionInformations([FromUri]string cafeId, [FromUri]int nbCoffeeRequiredForPromotion, [FromUri]double promotionValue)
        {
            //exceptions ?
            var store = new RoleStore<IdentityRole>(db);
            var roleManager = new RoleManager<IdentityRole>(store);

            var cafe = roleManager.FindByName("usercafe").Users.Where(e => e.UserId == cafeId).Single(); //retourne une exception si le rôle de l'utilisateur n'est pas "usercafe"

            ApplicationUser user = db.Users.Where(u => u.Id == cafeId).Single();

            ApplicationUser updatedUser = user;
            updatedUser.NbCoffeeRequiredForPromotion = nbCoffeeRequiredForPromotion;
            updatedUser.PromotionValue = promotionValue;
            db.Entry(user).CurrentValues.SetValues(updatedUser);
            await db.SaveChangesAsync();


            return Ok(user);        
        }

        [Route("changeEmail")]
        //[Authorize(Roles = "userperson")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> ChangeEmail([FromUri]string userId, [FromUri]string email)
        {
            MailAddress m = new MailAddress(email); //retourne une exception si la chaîne de caractères passée en argument n'est pas une adresse email.

            var store = new RoleStore<IdentityRole>(db);
            var roleManager = new RoleManager<IdentityRole>(store);

            var person = roleManager.FindByName("userperson").Users.Where(e => e.UserId == userId).Single(); //retourne une exception si le rôle de l'utilisateur n'est pas "userperson"

            ApplicationUser user = db.Users.Where(u => u.Id == userId).Single();

            ApplicationUser updatedUser = user;
            updatedUser.Email = email;
            db.Entry(user).CurrentValues.SetValues(updatedUser);
            await db.SaveChangesAsync();


            return Ok(user);
        }

        [Route("changePhoneNumber")]
        [Authorize(Roles = "userperson")]
        public async Task<IHttpActionResult> ChangePhoneNumber([FromUri]string userId, [FromUri]string phoneNumber)
        {
            //exceptions ?
            var store = new RoleStore<IdentityRole>(db);
            var roleManager = new RoleManager<IdentityRole>(store);

            if (!Regex.IsMatch(phoneNumber, @"^\d+$")) //que des chiffres
                throw new NonNumericPhoneNumberException();

            var person = roleManager.FindByName("userperson").Users.Where(e => e.UserId == userId).Single(); //retourne une exception si le rôle de l'utilisateur n'est pas "userperson"

            ApplicationUser user = db.Users.Where(u => u.Id == userId).Single();

            ApplicationUser updatedUser = user;
            updatedUser.PhoneNumber = phoneNumber;
            db.Entry(user).CurrentValues.SetValues(updatedUser);
            await db.SaveChangesAsync();


            return Ok(user);
        }



        [Authorize]
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            throw new NotImplementedException(); //pas géré
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("ConfirmEmail", Name = "ConfirmEmailRoute")]
        public async Task<IHttpActionResult> ConfirmEmail(string userId = "", string code = "")
        {
            throw new NotImplementedException(); //pas géré
        }

        //[Authorize]
        [Route("user/{id:guid}")]
        public async Task<IHttpActionResult> DeleteUser(string id)
        {
            throw new NotImplementedException(); //Pas nécessaire dans le cadre du projet
        }

    }


}