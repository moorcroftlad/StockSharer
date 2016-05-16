﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper;
using Newtonsoft.Json;
using StockSharer.Web.Data;
using StockSharer.Web.Location;
using StockSharer.Web.Models;
using StockSharer.Web.ViewModels;

namespace StockSharer.Web.Controllers
{
    public class SearchController : BaseController
    {
        private const int DefaultRadius = 1500;
        private readonly ICalulateLocation _locationCalculator;
        private readonly UserRepository _userRepository;
        private readonly DistanceCalculator _distanceCalculator;

        public SearchController()
        {
            _locationCalculator = new GoogleMapsApiLocationCalculator();
            _userRepository = new UserRepository();
            _distanceCalculator = new DistanceCalculator();
        }

        public ActionResult Index(string postcode, int? radius = null)
        {
            GeoLocation geoLocation = null;
            if (!string.IsNullOrEmpty(postcode))
            {
                postcode = postcode.Replace(" ", "").ToUpper();
                geoLocation = _locationCalculator.CalculateLocation(postcode.Replace(" ", "").ToUpper());
                Response.Cookies.Add(new HttpCookie("GeoLocation", JsonConvert.SerializeObject(geoLocation))
                {
                    Expires = DateTime.Now.AddYears(10)
                });
            }
            else
            {
                var geoLocationCookie = Request.Cookies["GeoLocation"];
                if (geoLocationCookie != null)
                {
                    try
                    {
                        geoLocation = JsonConvert.DeserializeObject<GeoLocation>(geoLocationCookie.Value);
                        postcode = geoLocation.Postcode;
                    }
                    catch
                    {
                        Response.Cookies.Remove("GeoLocation");
                    }
                }
            }
            var searchResults = new List<SearchResult>();
            if (geoLocation != null)
            {
                var userResults = RetrieveUsersInArea(geoLocation, radius.GetValueOrDefault(DefaultRadius));
                searchResults = RetrieveSearchResults(userResults);
            }
            var searchResultsViewModel = new SearchResultsViewModel
            {
                SearchResults = searchResults.OrderBy(x => x.Distance).ToList(),
                Postcode = postcode,
                Radius = radius
            };
            return View(searchResultsViewModel);
        }

        private List<UserResult> RetrieveUsersInArea(GeoLocation searchLocation, int radius)
        {
            var users = _userRepository.RetrieveAllActiveUsers();
            var usersInArea = new List<UserResult>();
            foreach (var user in users)
            {
                var distanceBetweenLocations = _distanceCalculator.CalculateDistanceBetweenLocations(searchLocation, user.Latitude, user.Longitude);
                if (distanceBetweenLocations < radius)
                {
                    usersInArea.Add(new UserResult
                        {
                            UserId = user.UserId,
                            Distance = distanceBetweenLocations,
                            Rating = user.Rating
                        });
                }
            }
            return usersInArea;
        }

        private List<SearchResult> RetrieveSearchResults(List<UserResult> userResults)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["StockSharerDatabase"].ToString()))
            {
                const string sql = @"   SELECT	ga.GameAvailabilityId, ga.Reference, a.Name Availability, g.Name GameName, g.HostedImageUrl, ga.UserId, p.Name PlatformName,
		                                        CASE WHEN r.UserId IS NOT NULL THEN 1 ELSE 0 END AS RequestedToday
                                        FROM 	GameAvailability ga
		                                        INNER JOIN Game g ON g.GameId = ga.GameId
                                                INNER JOIN Availability a ON a.AvailabilityId = ga.AvailabilityId
                                                INNER JOIN Platform p ON p.PlatformId = g.PlatformId
		                                        LEFT OUTER JOIN Request r on r.GameAvailabilityId = ga.GameAvailabilityId and r.StartDate = cast (GETDATE() as DATE) and (NULL = @LoggedInUserId OR r.UserId = @LoggedInUserId)
                                        WHERE   ga.UserId in @UserIds";
                var searchResults = connection.Query<SearchResult>(sql, new
                    {
                        UserIds = userResults.Select(x => x.UserId),
                        LoggedInUserId = User != null ? User.UserId : (int?)null
                    }).ToList();
                foreach (var searchResult in searchResults)
                {
                    var user = userResults.Single(x => x.UserId == searchResult.UserId);
                    searchResult.Distance = user.Distance;
                    searchResult.Rating = user.Rating;
                }
                return searchResults;
            }
        }
    }
}