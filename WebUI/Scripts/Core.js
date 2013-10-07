window.movieService.core = {
    api: {
        call: function (api, method, data, onSuccess, onComplete) {
            $.ajax({
                type: 'POST',
                url: window.movieService.environment.baseUrl + '/api/' + api + '/' + method,
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                async: true,
                data: JSON.stringify(data),
                processData: false,
                success: function(r) {
                    if (r.Status) {
                        onSuccess(r);
                    } else {
                        $('<span>' + r.ErrorMessage + '</span>').dialog({ modal: true, title: 'An error occured.' });
                    }
                },
                complete: onComplete,
                statusCode: {
                    401: function() { //Unauthorized
                        window.location.href = window.movieService.environment.baseUrl + 'Account/Login';
                    }
                }
            });
        }
    },
    ko: {
        observableDate: function (value) {
            //Knockout need an obserbale inside the computed for some reason.
            var observable = ko.observable(value);
            return {
                iso: ko.computed({
                    read: function () {
                        return observable().toISOString();
                    },
                    write: function (v) {
                        observable(new Date(v));
                    }
                }),
                ui: ko.computed({
                    read: function () {
                        return moment(observable()).format('DD.MM.YYYY HH:mm');
                    },
                    write: function (v) {
                        observable(moment(v, 'DD.MM.YYYY HH:mm').toDate());
                    }
                })
            };

            
        }
    }
};

//Revert 'button' function to JQuery UI implementation and add new alias for Twitter button.
//JQuery UI button is required for dialog.
$.fn.twButton = $.fn.button.noConflict();

ko.validation.configure({
    insertMessages: true,
    errorMessageClass: 'error',
    decorateElements: true
});

(function () {
    //https://gist.github.com/dperini/729294 with some tweaks.
    var urlRegex = new RegExp(
      "^" +
        // protocol identifier
        "(?:(?:https?|ftp)://)?" +
        "(?:" +
          // IP address exclusion
          // private & local networks
          "(?!10(?:\\.\\d{1,3}){3})" +
          "(?!127(?:\\.\\d{1,3}){3})" +
          "(?!169\\.254(?:\\.\\d{1,3}){2})" +
          "(?!192\\.168(?:\\.\\d{1,3}){2})" +
          "(?!172\\.(?:1[6-9]|2\\d|3[0-1])(?:\\.\\d{1,3}){2})" +
          // IP address dotted notation octets
          // excludes loopback network 0.0.0.0
          // excludes reserved space >= 224.0.0.0
          // excludes network & broacast addresses
          // (first & last IP address of each class)
          "(?:[1-9]\\d?|1\\d\\d|2[01]\\d|22[0-3])" +
          "(?:\\.(?:1?\\d{1,2}|2[0-4]\\d|25[0-5])){2}" +
          "(?:\\.(?:[1-9]\\d?|1\\d\\d|2[0-4]\\d|25[0-4]))" +
        "|" +
          // host name
          "(?:(?:[a-z\\u00a1-\\uffff0-9]+-?)*[a-z\\u00a1-\\uffff0-9]+)" +
          // domain name
          "(?:\\.(?:[a-z\\u00a1-\\uffff0-9]+-?)*[a-z\\u00a1-\\uffff0-9]+)*" +
          // TLD identifier
          "(?:\\.(?:[a-z\\u00a1-\\uffff]{2,}))" +
        ")" +
        // port number
        "(?::\\d{2,5})?" +
        // resource path
        "(?:/[^\\s]*)?" +
      "$", "i"
);

    ko.validation.rules['url'] = {
        validator: function (value) {
            return urlRegex.test(value);
        },
        message: 'The field must be a valid URL'
    };
    ko.validation.registerExtenders();

})();
