namespace StockSharer.Web.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public string FullName
        {
            get { return Forename + " " + Surname; }
        }
    }
}