$(function () {
    var api = window.movieService.core.api;

    var viewModel = {
        movies: ko.observableArray(),
        Owner: {Name: "Bruce Wayne"},
        ExpirationDate: "04/10/2013 12:00",

        load: function () {
            this.movies.removeAll();
            api.call('poll', 'getcurrent', {}, this.fetchMoreMoviesSuccess);
        },

        fetchMoreMoviesSuccess: function (r) {
            //$.each(r.Data.Items, function (i, e) {
            //    viewModel.movies.push(e);
            //});
            $(window).scrollTop($(document).height());
        }
    };

    ko.applyBindings(viewModel);

    viewModel.load();

});