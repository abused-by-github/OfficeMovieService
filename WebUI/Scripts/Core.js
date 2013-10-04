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
