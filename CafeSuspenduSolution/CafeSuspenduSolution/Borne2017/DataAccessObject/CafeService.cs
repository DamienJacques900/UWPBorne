using Borne2017.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using UWPBorne.Exceptions;

namespace Borne2017.DataAccessObject
{
    public class CafeService
    {
        public async Task<IEnumerable<CafeWithCharities>> GetCafe()
        {
            try
            {
                var client = new HttpClient();
                var cafes = await client.GetStringAsync(new Uri("http://cafesuspenduappweb.azurewebsites.net/api/charities/CafeWithAvailableCharities"));
                cafes = @"{ ""data"" :[" + cafes + "]}";
                var rawCafes = JObject.Parse(cafes);
                //Children() permet de parcourir tout les enfants
                var allCafe = rawCafes["data"][0].Children().Select(d => new CafeWithCharities
                {
                    Cafe = new ApplicationUser
                    {
                        CafeName = d["cafe"]["cafeName"].Value<string>(),
                        Street = d["cafe"]["street"].Value<string>(),
                        Number = d["cafe"]["number"].Value<string>(),
                        TimeTables = IncludeTimeTables(d).ToList()
                    },
                    Charities = IncludeCharities(d).ToList()
                });

                return allCafe;
            }
            catch (Exception)
            {
                throw new DataNotReachableException();
            }
        }

        private IEnumerable<TimeTable> IncludeTimeTables(Newtonsoft.Json.Linq.JToken d)
        {
            var jsonTables = d["cafe"]["timeTables"].Children().Select(t => new TimeTable()
            {
                OpeningHour = new TimeSpan(int.Parse(t["openingHour"].Value<string>().Substring(0, 2)), int.Parse(t["openingHour"].Value<string>().Substring(3, 2)), int.Parse(t["openingHour"].Value<string>().Substring(6, 2))),
                ClosingHour = new TimeSpan(int.Parse(t["closingHour"].Value<string>().Substring(0, 2)), int.Parse(t["closingHour"].Value<string>().Substring(3, 2)), int.Parse(t["closingHour"].Value<string>().Substring(6, 2))),
                DayOfWeek = t["dayOfWeek"].Value<int>()

            });

            return jsonTables;
        }

        private IEnumerable<Charity> IncludeCharities(Newtonsoft.Json.Linq.JToken d)
        {
            var jsonCharities = d["charities"].Children().Select(t => new Charity()
            {
                CharityId = t["charityId"].Value<int>(),
                NbCoffeeOffered = t["nbCoffeeOffered"].Value<int>(),
                NbCoffeeConsumed = t["nbCoffeeConsumed"].Value<int>(),
                RowVersion = t["rowVersion"].Value<string>()
            });

            return jsonCharities;

        }

        public async Task ProceedBooking(string cafeName, int terminalId, string bookingName)
        {
            try
            {
                PushBookingInformations reservation = new PushBookingInformations()
                {
                    CafeName = cafeName,
                    TerminalId = terminalId,
                    BookingName = bookingName
                };

                var json = JsonConvert.SerializeObject(reservation);

                var buffer = System.Text.Encoding.UTF8.GetBytes(json);
                var byteContent = new ByteArrayContent(buffer);

                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var client = new HttpClient();
                var result = await client.PostAsync(new Uri("http://cafesuspenduappweb.azurewebsites.net/api/Bookings"), byteContent);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                result.EnsureSuccessStatusCode();
            }
            catch(Exception)
            {
                throw new DataUpdateNotPossibleException();
            }
        }
    }
}
