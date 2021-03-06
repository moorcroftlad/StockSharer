﻿using System;

namespace StockSharer.Web.Areas.Settings.Models
{
    public class GameRequest
    {
        public Guid Reference { get; set; }
        public string GameName { get; set; }
        public int Nights { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? Accepted { get; set; }
        public DateTime? Rejected { get; set; }
        public DateTime Timestamp { get; set; }
        public string Origin { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Town { get; set; }
        public string County { get; set; }
        public string Postcode { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }

        public bool Received
        {
            get { return Origin == "Received"; }
        }

        public bool IsToday
        {
            get { return StartDate.Date.Equals(DateTime.Today); }
        }

        public bool AcceptedToday
        {
            get { return Accepted.GetValueOrDefault(DateTime.MinValue).Date.Equals(DateTime.Today); }
        }

        public bool RejectedToday
        {
            get { return Rejected.GetValueOrDefault(DateTime.MinValue).Date.Equals(DateTime.Today); }
        }

        public bool UserIsAvailable
        {
            get { return DateTime.Now.TimeOfDay < EndTime.TimeOfDay; }
        }
    }
}