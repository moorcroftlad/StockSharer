using System.Collections.Generic;
using StockSharer.Web.Models;

namespace StockSharer.Web.ViewModels
{
    public class SearchResultsViewModel
    {
        public List<SearchResult> SearchResults { get; set; }
        public SearchFilter SearchFilter { get; set; }
    }

    public class LocationSearchResultsViewModel : SearchResultsViewModel
    {
        public string LocationName { get; set; }
    }
}