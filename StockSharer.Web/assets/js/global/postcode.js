$(document).ready(function() {
    $('.js-btn-game-search').on('click', function (event) {
        if (docCookies.getItem('GeoLocation') === null) {
            event.preventDefault();
            $('#postcodeModal').modal('show');
        }
    });

    $('#save-postcode-form').on('submit', function (event) {
        var $form = $(this);
        var url = $form.attr('action');
        event.preventDefault(); 
        $.ajax({
            type: "POST",
            url: url,
            data: $form.serialize(),
            success: function (data) {
                if (data.Success) {
                    window.location = '/search';
                } else {
                    $form.find('.help-block').text(data.Reason);
                }
            }
        });
    });
});