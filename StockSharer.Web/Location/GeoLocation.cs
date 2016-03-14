namespace StockSharer.Web.Location
{
    public class GeoLocation
    {
        public GeoLocation(double latitude, double longitude, string postcode)
        {
            Latitude = latitude;
            Longitude = longitude;
            Postcode = postcode;
        }

        public double Latitude { get; private set; }
        public double Longitude { get; private set; }
        public string Postcode { get; private set; }
    }
}