using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using HtmlAgilityPack;
using StockSharer.Web.Models;

namespace ConsoleApplication1
{
    internal class Program
    {
        private const string BaseUrl = "https://www.gumtree.com";

        private static void Main()
        {
            var url = string.Format("{0}/search?search_category=all", BaseUrl);
            ScrapeUrl(url, 1);
        }

        private static void ScrapeUrl(string url, int parentSearchLocationId)
        {
            var htmlWeb = new HtmlWeb
                {
                    UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36"
                };
            var htmlDocument = htmlWeb.Load(url);
            var htmlNodes = htmlDocument.DocumentNode.SelectNodes("//*[@id=\"fullListings\"]/div[1]/div/div/nav/aside/section[1]/div/ul/li/a");
            if (htmlNodes == null) return;
            foreach (var htmlNode in htmlNodes)
            {
                var href = htmlNode.Attributes["href"].Value.Replace("&amp;", "&");
                var searchLocationId = InsertSearchLocation(new SearchLocation
                    {
                        Name = htmlNode.InnerText.Replace("\n", ""),
                        UrlToken = href.Replace("/search?search_category=all&search_location=", ""),
                        ParentSearchLocationId = parentSearchLocationId
                    });
                if (searchLocationId > 0)
                {
                    ScrapeUrl(string.Format("{0}{1}", BaseUrl, href), searchLocationId);
                }
            }
        }


        private static int InsertSearchLocation(SearchLocation searchLocation)
        {
            try
            {
                using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["StockSharerDatabase"].ToString()))
                {
                    const string sql = @"   INSERT SearchLocation (Name, UrlToken, ParentSearchLocationId)
                                                VALUES (@Name, @UrlToken, @ParentSearchLocationId)
                                                SELECT CAST(SCOPE_IDENTITY() as int)";
                    return connection.Query<int>(sql, searchLocation).SingleOrDefault();
                }
            }
            catch
            {
                Console.WriteLine("Error inserting location, name: {0}, parentLocationId: {1}, urlToken: {2}", searchLocation.Name, searchLocation.ParentSearchLocationId, searchLocation.UrlToken);
                return 0;
            }
        }
    }
}