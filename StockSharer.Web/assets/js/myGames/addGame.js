$(document).ready(function() {
    var searchInputSelector = '#game-search input[name="term"]';
    $(searchInputSelector).liveSearch({ url: '/settings/games/search?term=' });

    $(document).click(function(event) {
        var searchResultsSelector = '#live-search-term';
        if (!$(event.target).closest(searchResultsSelector).length && !$(event.target).is(searchResultsSelector) && !$(event.target).is(searchInputSelector)) {
            if ($(searchResultsSelector).is(":visible")) {
                $(searchResultsSelector).empty();
                $(searchInputSelector).val('');
            }
        }
    });
});