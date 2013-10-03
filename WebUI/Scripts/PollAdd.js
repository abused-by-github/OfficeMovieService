$(function () {
    var api = window.movieService.core.api;

    var viewModel = {
        Name: ko.observable('2009-02-15T00:00:00Z'),
        ExpirationDate: ko.observable('2009-02-15T00:00:00Z'),
        ViewDate: ko.observable('2009-02-15T00:00:00Z'),

        save: function() {
            var data = {
                Name: this.Name(),
                ExpirationDate: this.ExpirationDate()
            };
            api.call('poll', 'save', data, this.onSaved);
        },

        onSaved: function() {
            
        }
    };

    ko.applyBindings(viewModel);
});