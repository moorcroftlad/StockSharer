using System;
using System.Device.Location;

namespace StockSharer.Web.Controllers
{    
    public class DistanceCalculator
    {
        private const double KilometresPerMile = 1.609344;

        public int CalculateDistanceBetweenLocations(Location location1, Location location2)
        {
            if (location1 == null || location2 == null)
            {
                return 0;
            }
            var geoCoordinate1 = new GeoCoordinate(location1.Latitude, location1.Longitude);
            var geoCoordinate2 = new GeoCoordinate(location2.Latitude, location2.Longitude);
            var distanceInMiles = (geoCoordinate1.GetDistanceTo(geoCoordinate2) / 1000) / KilometresPerMile;
            return (int)Math.Round(distanceInMiles);
        }
    }
}