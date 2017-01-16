$(document).ready(function() {
    $('.js-btn-reserve').on('click', function(event) {
        event.preventDefault();
        $('#reservationModal').modal('show');
        //TODO - clear current modal contents
        $('#reservationModal').load('/reservation/validate/', {
                reference: $(this).data('reference'),
                gameName: $(this).data('name')
            }, function() {
                initialiseModal();
            });
    });

    var updateDate = function(start, end) {
        $('#dateRange span').html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'));
        $('#reservation input[name="EndDate"]').val(end.format('YYYY-MM-DD'));
    };

    var initialiseDatePicker = function () {
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
        updateDate(window.moment(), window.moment().add(2, 'days'));
    };

    var initialiseReservationForm = function () {
        $('#reservation').on('submit', function (event) {
            event.preventDefault();
            var form = $(this);
            $('.js-no-request-made').hide();
            $('.js-request-made').show();
            $.post(form.attr('action'), form.serialize());
        });
    };

    var initialiseModal = function () {
        initialiseDatePicker();
        initialiseReservationForm();
    };
});