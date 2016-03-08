using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;

namespace StockSharer.Web.Controllers
{
    public interface ICalulateLocation
    {
        Location CalculateLocation(string postcode);
    }

    public class FakeLocationCalculator : ICalulateLocation
    {
        public Location CalculateLocation(string postcode)
        {
            return new Location(0, 0);
        }
    }

    public class GoogleMapsApiLocationCalculator : ICalulateLocation
    {
        public Location CalculateLocation(string postcode)
        {
            var url = string.Format("https://maps.googleapis.com/maps/api/geocode/json?address={0}", postcode);
            using (var httpClient = new HttpClient())
            {
                var googleMapsApiResult = httpClient.GetStringAsync(url).Result;
                var response = JsonConvert.DeserializeObject<Response>(googleMapsApiResult);
                return response.Status == "OK" ? response.Results[0].Geometry.GoogleMapsLocation.Location : null;
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
            public GoogleMapsLocation GoogleMapsLocation { get; set; }
        }

        internal class GoogleMapsLocation
        {
            [JsonProperty(PropertyName = "lat")]
            public double Latitude { get; set; }
            [JsonProperty(PropertyName = "lng")]
            public double Longitude { get; set; }

            public Location Location
            {
                get { return new Location(Latitude, Longitude); }
            }
        }
    }

    public class Location
    {
        public Location(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }
    }
}