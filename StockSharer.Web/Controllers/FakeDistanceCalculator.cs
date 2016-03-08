using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Net.Http;
using Newtonsoft.Json;

namespace StockSharer.Web.Controllers
{
    public interface ICalulateDistance
    {
        int CalculateDistanceBetweenPostcodes(string postcode, decimal latitude, decimal longitude);
    }

    public class FakeDistanceCalculator : ICalulateDistance
    {
        public int CalculateDistanceBetweenPostcodes(string postcode, decimal latitude, decimal longitude)
        {
            return 0;
        }
    }

    public class GoogleMapsApiDistanceCalculator : ICalulateDistance
    {
        private const double KilometresPerMile = 1.609344;

        public int CalculateDistanceBetweenPostcodes(string postcode, decimal latitude, decimal longitude)
        {
            var location = RetrieveLocation(postcode);
            if (location == null) return 0;
            var gameLocation = new GeoCoordinate((double)latitude, (double)longitude);
            var userLocation = new GeoCoordinate(location.Latitude, location.Longitude);
            var distanceInMiles = (gameLocation.GetDistanceTo(userLocation) / 1000) / KilometresPerMile;
            return (int)Math.Round(distanceInMiles);
        }

        private static Location RetrieveLocation(string postcode)
        {
            var url = string.Format("https://maps.googleapis.com/maps/api/geocode/json?address={0}", postcode);
            using (var httpClient = new HttpClient())
            {
                var googleMapsApiResult = httpClient.GetStringAsync(url).Result;
                var response = JsonConvert.DeserializeObject<Response>(googleMapsApiResult);
                return response.Status == "OK" ? response.Results[0].Geometry.Location : null;
            }
        }

        internal class Response
        {
            [JsonProperty(PropertyName = "status")]
            public string Status { get; set; }
            [JsonProperty(PropertyName = "results")]
            public List<Result> Results { get; set; }
        }

        internal class Result
        {
            [JsonProperty(PropertyName = "geometry")]
            public Geometry Geometry { get; set; }
        }

        internal class Geometry
        {
            [JsonProperty(PropertyName = "location")]
            public Location Location { get; set; }
        }

        internal class Location
        {
            [JsonProperty(PropertyName = "lat")]
            public double Latitude { get; set; }
            [JsonProperty(PropertyName = "lng")]
            public double Longitude { get; set; }
        }
    }
}