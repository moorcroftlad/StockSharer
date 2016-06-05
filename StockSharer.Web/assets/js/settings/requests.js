$(document).ready(function () {
    $('.js-view-address').on('click', function (event) {
        event.preventDefault();
        var source = $("#view-address-template").html();
        var template = Handlebars.compile(source);
        var clickElement = $(this);
        var context = {
            line1: clickElement.data('line1'),
            line2: clickElement.data('line2'),
            town: clickElement.data('town'),
            county: clickElement.data('county'),
            postcode: clickElement.data('postcode'),
            forename: clickElement.data('forename'),
            surname: clickElement.data('surname'),
            startTime: clickElement.data('start-time'),
            endTime: clickElement.data('end-time')
        };
        var html = template(context);
        $('.js-view-address-holder').html(html);
        $('#viewAddressModal').modal('show');
    });
});