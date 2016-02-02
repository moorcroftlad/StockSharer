$(document).ready(function() {
    $('#game-search input[name="term"]').liveSearch({ url: '/settings/games/search?term=' });

    $(document).click(function(event) {
        if (!$(event.target).closest('#live-search-term').length && !$(event.target).is('#live-search-term') && !$(event.target).is('#game-search input[name="term"]')) {
            if ($('#live-search-term').is(":visible")) {
                $('#live-search-term').hide();
            }
        } else {
            $('#live-search-term').empty();
            $('#live-search-term').show();
        }
    });
});