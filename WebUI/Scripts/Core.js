window.movieService.core = {
    api: {
        call: function(api, method, data, onSuccess, onComplete) {
            $.ajax({
                type: 'POST',
                url: window.movieService.environment.baseUrl + '/api/' + api + '/' + method,
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                async: true,
                data: JSON.stringify(data),
                processData: false,
                success: onSuccess,
                complete: onComplete
            });
        }
    }
};

ko.observable.fn.uiDate = function() {
    return moment(this()).format("DD.MM.YYYY");
};

ko.observable.fn.isoDate = function (str) {
    return str ? this(new Date(str)) : this().toISOString();
};
