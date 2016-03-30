$(document).ready(function() {
    $('.js-btn-availability').on('click', function(event) {
        event.preventDefault();
        $('#availabilityModal').modal('show');
    });
});