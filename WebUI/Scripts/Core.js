window.movieService.core = {
    api: {
        call: function (api, method, data, onSuccess, onComplete) {
            $.ajax({
                type: 'POST',
                url: window.movieService.environment.baseUrl + 'api/' + api + '/' + method,
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                async: true,
                data: JSON.stringify(data),
                processData: false,
                success: function(r) {
                    if (r.Status) {
                        onSuccess(r);
                    } else {
                        window.movieService.core.ui.showError(r.ErrorMessage);
                    }
                },
                complete: onComplete,
                statusCode: {
                    401: function() { //Unauthorized
                        window.location.href = window.movieService.environment.baseUrl;
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
                        var date = observable();
                        return date ? date.toISOString() : '';
                    },
                    write: function (v) {
                        observable(v ? new Date(v) : null);
                    }
                }),
                ui: ko.computed({
                    read: function () {
                        var date = observable();
                        return date ? moment(date).format('DD.MM.YYYY HH:mm') : '';
                    },
                    write: function (v) {
                        observable(v ? moment(v, 'DD.MM.YYYY HH:mm').toDate() : null);
                    }
                })
            };
        }
    },

    ui: {
        showInfo: function (text) {
            $('<div>' + text + '</div>').dialog();
        },
        showError: function(text) {
            $('<div>' + text + '</div>').dialog({
                width: 'auto',
                minHeight: 'auto',
                title: 'Error',
                modal: true,
                dialogClass: 'error'
            });
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

ko.bindingHandlers.dateTimePicker = {
    init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
        $(element).datetimepicker({
            onSelect: function () {
                $(this).change();
            },
            dateFormat: 'dd.mm.yy'
        });
    },
    update: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
        // This will be called once when the binding is first applied to an element,
        // and again whenever the associated observable changes value.
        // Update the DOM element based on the supplied values here.
    }
};

/*
 * Binding accepts sinle parameter - options.
 * "source" - function (term, onDone). "term" - text, typed in textbox; "onDone" - callback which function should call when data are ready.
 * "credits" - id of an element. Inner HTML of this elements will be put to autocomplete as "special" last item. Item won't be selectable.
 * "minLength" - min lenght of user input which causes autocomplete.
 * "values" - hash. Each key corresponds to key in data items which were returned by "source". Each value - observable which will be updated with corresponding data.
 * For example, values: { id: TmdbId, title: Name }. Each data item returned by source must have keys "id" and "title". There must be observables "TmdbId" and "Name"
 * in binding context. When an item is selected from autocomplete, next will happen: TmdbId(item.id); Name(item.title), where item - and item returned by source.
 * "labelKey" - key in items returned by source, which will be shown as item caption in autocomplete list.
 */
ko.bindingHandlers.autocomplete = {
    init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
        var options = valueAccessor();
        $(element).autocomplete({
            source: function (request, response) {
                options.source(request.term, response);
            },
            minLength: options.minLength,
            select: function(event, ui) {
                var item = ui.item;
                if (item.hasOwnProperty('__selectable') && !item['__selectable']) {
                    event.preventDefault();
                } else {
                    for (var key in options.values) {
                        options.values[key](item[key]);
                    }
                }
                return false;
            },
            focus: function () {
                return false;
            },
            create: function () {
                $(this).data('ui-autocomplete')._renderItem = function (ul, item) {
                    var li = $('<li>')
                        .append('<a>' + item[options.labelKey] + '</a>')
                        .appendTo(ul);
                    return li;
                };

                $(this).data('ui-autocomplete')._renderMenu = function (ul, items) {
                    var that = this;
                    $.each(items, function (index, item) {
                        that._renderItemData(ul, item);
                    });
                    if (options.credits) {
                        var li = $($('#' + options.credits).html());
                        li.data("ui-autocomplete-item", { __selectable: false });
                        li.insertAfter($(ul).find("li:last"));
                    }
                };
            },
        });
    },
    update: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
        // This will be called once when the binding is first applied to an element,
        // and again whenever the associated observable changes value.
        // Update the DOM element based on the supplied values here.
    }
};

$.fn.koBind = function (viewModel) {
    this.each(function(i, e) {
        ko.applyBindings(viewModel, e);
    });
    return this;
};
