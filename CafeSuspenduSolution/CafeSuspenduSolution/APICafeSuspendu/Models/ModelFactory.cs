using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Routing;

namespace APICafeSuspendu.Models
{
    public class ModelFactory
    {
        private UrlHelper _UrlHelper;
        private ApplicationUserManager _AppUserManager;

        public ModelFactory(HttpRequestMessage request, ApplicationUserManager appUserManager)
        {
            _UrlHelper = new UrlHelper(request);
            _AppUserManager = appUserManager;
        }

        public UserReturnModel Create(ApplicationUser appUser)
        {
            return new UserReturnModel
            {
                Url = _UrlHelper.Link("GetUserById", new { id = appUser.Id }),
                Id = appUser.Id,
                UserName = appUser.UserName,
                CafeName = appUser.CafeName,
                Street = appUser.Street,
                Number = appUser.Number,
                NbCoffeeRequiredForPromotion = appUser.NbCoffeeRequiredForPromotion,
                PromotionValue = appUser.PromotionValue,
                Bookings = appUser.Bookings,
                TimeTables = TimeTableToTimeTableReturnModel(appUser.TimeTables),
                FirstName = appUser.FirstName,
                LastName = appUser.LastName,
                Email = appUser.Email,
                EmailConfirmed = appUser.EmailConfirmed,
                PhoneNumber = appUser.PhoneNumber,


                Roles = _AppUserManager.GetRolesAsync(appUser.Id).Result,
                Claims = _AppUserManager.GetClaimsAsync(appUser.Id).Result
            };
        }

        private List<TimeTableReturnModel> TimeTableToTimeTableReturnModel(ICollection<TimeTable> TimeTables)
        {
            List<TimeTableReturnModel> returnModels = new List<TimeTableReturnModel>();
            foreach (TimeTable table in TimeTables)
            {
                returnModels.Add(new Models.TimeTableReturnModel()
                {
                    TimeTableID = table.TimeTableID,
                    OpeningHour = table.OpeningHour,
                    ClosingHour = table.ClosingHour,
                    DayOfWeek = table.DayOfWeek
                });
            }

            return returnModels;
        }
    }



}