$(document).ready(function () {
    $('#game-search input[name="term"]').liveSearch({ url: '/settings/games/search?term=' });
});