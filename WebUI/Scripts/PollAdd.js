$(function () {
    var api = window.movieService.core.api;

    $('[data-format=date]').datepicker();
    $('[data-format=date]').datepicker('option', 'dateFormat', 'dd.mm.yy');

    var viewModel = {
        Name: ko.observable('Just a poll.'),
        ExpirationDate: ko.observable(new Date()),
        ViewDate: ko.observable(new Date()),

        save: function() {
            var data = {
                Name: this.Name(),
                ExpirationDate: this.ExpirationDate.isoDate(),
                ViewDate: this.ViewDate.isoDate()
            };
            api.call('poll', 'save', data, this.onSaved);
        },

        load: function() {
            api.call('poll', 'GetCurrent', null, this.onLoaded);
        },

        onSaved: function() {
            
        },

        onLoaded: function (r) {
            viewModel.Name(r.data.Name);
            
            viewModel.ExpirationDate.isoDate(r.data.ExpirationDate);
            viewModel.ViewDate.isoDate(r.data.ViewDate);
        }
    };

    ko.applyBindings(viewModel);

    if (window.movieService.current.action == 'edit') {
        viewModel.load();
    }
});