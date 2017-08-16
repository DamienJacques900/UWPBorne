using Borne2017.Model;
using GalaSoft.MvvmLight.Views;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UWPBorne.Exceptions;

namespace Borne2017.DataAccessObject
{
    public class PositionService
    {
        public async Task<LatitudeAndLongitude> GetLatitudeAndLongitude(string address)
        {
            try
            {
                var client = new HttpClient();
                address = address.Replace(" ", "+");

                var formatAddress = "https://maps.googleapis.com/maps/api/geocode/json?address=" + address + "&key=AIzaSyC4NOA0LXMFGZxnnuBiP7UE3AQ11raZbDk";
                var position = await client.GetStringAsync(new Uri(formatAddress));
                var rawData = JObject.Parse(position);
                var latitude = rawData["results"][0]["geometry"]["location"]["lat"].Value<Double>();
                var longitude = rawData["results"][0]["geometry"]["location"]["lng"].Value<Double>();

                return new LatitudeAndLongitude()
                {
                    Latitude = latitude,
                    Longitude = longitude
                };
            }
            catch (Exception)
            {
                throw new DataNotReachableException();
            }
        }
    }
}
