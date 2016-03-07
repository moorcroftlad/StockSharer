using System.Collections.Generic;
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
        public int CalculateDistanceBetweenPostcodes(string postcode, decimal latitude, decimal longitude)
        {
            var location = RetrieveLocation(postcode);
            //TODO: return distance between location and latitude / longitude
            return 0;
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
            public decimal Latitude { get; set; }
            [JsonProperty(PropertyName = "lng")]
            public decimal Longitude { get; set; }
        }
    }
}