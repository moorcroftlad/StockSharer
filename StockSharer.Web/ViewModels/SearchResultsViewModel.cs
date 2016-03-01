using System.Collections.Generic;
using StockSharer.Web.Models;

namespace StockSharer.Web.ViewModels
{
    public class SearchResultsViewModel
    {
        public List<SearchResult> SearchResults { get; set; }
        public string Postcode { get; set; }
        public int? Radius { get; set; }
    }
}