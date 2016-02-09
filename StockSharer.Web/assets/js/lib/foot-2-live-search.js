/**
 * LiveSearch 1.0
 *
 * TODO
 */
var LiveSearch = {
    init: function (input, conf) {
        var config = {
            url: conf.url || false,
            appendTo: conf.appendTo || 'after',
            data: conf.data || {}
        };

        var appendTo = appendTo || 'after';

        input.setAttribute('autocomplete', 'off');

        // Create search container
        var container = document.createElement('div');

        container.id = 'live-search-' + input.name;

        container.classList.add('live-search');

        // Append search container
        if (appendTo == 'after') {
            input.parentNode.classList.add('live-search-wrap');
            input.parentNode.insertBefore(container, input.nextSibling);
        }
        else {
            appendTo.appendChild(container);
        }

        var performSearch = function (element, waitTime) {
            element.classList.add('loading');

            var q = element.value;

            // Clear previous ajax request
            if (element.liveSearchTimer) {
                clearTimeout(element.liveSearchTimer);
            }

            // Build the URL
            var url = config.url + q;

            if (config.data) {
                if (url.indexOf('&') != -1 || url.indexOf('?') != -1) {
                    url += '&' + LiveSearch.serialize(config.data);
                } else {
                    url += '?' + LiveSearch.serialize(config.data);
                }
            }

            // Wait a little then send the request
            var self = element;
            element.liveSearchTimer = setTimeout(function() {
                SimpleAjax.xhr({
                    method: 'get',
                    url: url,
                    callback: function(data) {
                        self.classList.remove('loading');
                        container.innerHTML = data;
                        bindGameClick();
                    }
                });
            }, waitTime);
        };

        input.addEventListener('keyup', function () {
            performSearch(this, 300);
        });
        input.addEventListener('focus', function () {
            performSearch(this, 300);
        });
        
        var bindGameClick = function () {
            $('.js-owns-game').off('click').on('click', function(event) {
                event.preventDefault();
            });

            $('.js-game').off('click').on('click', function (event) {
                event.preventDefault();
                var gameId = $(this).data('game-id');
                $.post("/settings/games/addgameavailability/" + gameId, function (gameAvailability) {
                    $('#live-search-term').empty();
                    $('#game-search input[name="term"]').val('');
                    var row = '<tr><td><img src="' + gameAvailability.HostedImageUrl + '" width="50" height="63"></td><td>' + gameAvailability.GameName + '</td><td>' + gameAvailability.PlatformName + '</td><td>' + formatDate(gameAvailability.DateAdded) + '</td><td>' + gameAvailability.AvailabilityName + '</td></tr>';
                    $('.js-game-list').append(row);
                });
            });
        };

        var formatDate = function(jsonDate) {
            var monthNames = [
              "January", "February", "March",
              "April", "May", "June", "July",
              "August", "September", "October",
              "November", "December"
            ];
            var date = new Date(parseInt(jsonDate.substr(6)));
            var day = date.getDate();
            var monthIndex = date.getMonth();
            var year = date.getFullYear();
            return day + ' ' + monthNames[monthIndex] + ' ' + year;
        };
    },

    // http://stackoverflow.com/questions/1714786/querystring-encoding-of-a-javascript-object
    serialize: function (obj) {
        var str = [];

        for (var p in obj) {
            if (obj.hasOwnProperty(p)) {
                str.push(encodeURIComponent(p) + '=' + encodeURIComponent(obj[p]));
            }
        }

        return str.join('&');
    }
};

if (typeof (jQuery) != 'undefined') {
    jQuery.fn.liveSearch = function (conf) {
        return this.each(function () {
            LiveSearch.init(this, conf);
        });
    };
}