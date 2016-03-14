using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;

namespace StockSharer.Web.Location
{
    public interface ICalulateLocation
    {
        GeoLocation CalculateLocation(string postcode);
    }

    public class FakeLocationCalculator : ICalulateLocation
    {
        public GeoLocation CalculateLocation(string postcode)
        {
            return new GeoLocation(0, 0, "");
        }
    }

    public class GoogleMapsApiLocationCalculator : ICalulateLocation
    {
        public GeoLocation CalculateLocation(string postcode)
        {
            var url = string.Format("https://maps.googleapis.com/maps/api/geocode/json?address={0}", postcode);
            using (var httpClient = new HttpClient())
            {
                var googleMapsApiResult = httpClient.GetStringAsync(url).Result;
                var response = JsonConvert.DeserializeObject<Response>(googleMapsApiResult);
                if (response.Status != "OK")
                {
                    return null;
                }
                return new GeoLocation(response.Results[0].Geometry.GoogleMapsLocation.Latitude, response.Results[0].Geometry.GoogleMapsLocation.Longitude, postcode);
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
        }
    }
}