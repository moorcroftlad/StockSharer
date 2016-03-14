using System;
using System.Device.Location;

namespace StockSharer.Web.Location
{    
    public class DistanceCalculator
    {
        private const double KilometresPerMile = 1.609344;

        public int CalculateDistanceBetweenLocations(GeoLocation searchLocation, double latitude, double longitude)
        {
            if (searchLocation == null)
            {
                return 0;
            }
            var geoCoordinate1 = new GeoCoordinate(searchLocation.Latitude, searchLocation.Longitude);
            var geoCoordinate2 = new GeoCoordinate(latitude, longitude);
            var distanceInMiles = (geoCoordinate1.GetDistanceTo(geoCoordinate2) / 1000) / KilometresPerMile;
            return (int)Math.Round(distanceInMiles);
        }
    }
}