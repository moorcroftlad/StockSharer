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