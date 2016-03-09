$(document).ready(function() {
    $('.js-btn-game-search').on('click', function (event) {
        if (/*TODO: postcode cookie exists*/false) {

        } else {
            event.preventDefault();
            $('#postcodeModal').modal('show');
        }
    });
});