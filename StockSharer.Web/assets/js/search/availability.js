$(document).ready(function() {
    $('.js-btn-availability').on('click', function(event) {
        event.preventDefault();
        if (window.loggedIn) {
            $('#availabilityModal').modal('show');
        } else {
            window.location.href = "/user/login?ReturnUrl=/search";
        }
    });

    var updateDate = function(start, end) {
        $('#dateRange span').html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'));
    };

    $('#dateRange').daterangepicker({
        ranges: {
            'One night': [window.moment(), window.moment().add(1, 'day')],
            'Two nights': [window.moment(), window.moment().add(2, 'days')],
            'Three nights': [window.moment(), window.moment().add(3, 'days')],
            'One week': [window.moment(), window.moment().add(7, 'days')]
        },
        locale: {
            format: 'YYYY-MM-DD'
        },
        minDate: window.moment()
    }, updateDate);
    
    updateDate(window.moment(), window.moment().add(1, 'day'));
});