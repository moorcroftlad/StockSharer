$(document).ready(function() {
    $('.js-btn-availability').on('click', function(event) {
        event.preventDefault();
        if (window.loggedIn) {
            var clickElement = $(this);
            var reference = clickElement.data('reference');
            var name = clickElement.data('name');
            $('#availabilityRequest input[name="Reference"]').val(reference);
            $('#availabilityRequest').find('.js-availability-request-header').text(name);
            if (clickElement.data('requested-today')) {
                hideRequestButton();
            } else {
                showRequestButton();
            }
            $('#availabilityModal').modal('show');
        } else {
            window.location.href = "/user/login?ReturnUrl=/search";
        }
    });

    var updateDate = function(start, end) {
        $('#dateRange span').html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'));
        $('#availabilityRequest input[name="EndDate"]').val(end.format('YYYY-MM-DD'));
    };

    var hideRequestButton = function () {
        $('#availabilityRequest').find('.js-no-request-made').hide();
        $('#availabilityRequest').find('.js-request-made').show();
    };
    
    var showRequestButton = function () {
        $('#availabilityRequest').find('.js-no-request-made').show();
        $('#availabilityRequest').find('.js-request-made').hide();
    };

    updateDate(window.moment(), window.moment().add(2, 'days'));

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
        startDate: window.moment(),
        endDate: window.moment().add(2, 'days'),
        minDate: window.moment()
    }, updateDate);

    $('#availabilityRequest').on('submit', function (event) {
        event.preventDefault();
        var form = $(this);
        var reference = form.find('input[name="Reference"]');
        $('.js-btn-availability[data-reference="' + reference.val() + '"]').data('requested-today', true);
        hideRequestButton();
        $.post(form.attr('action'), form.serialize());
    });
});