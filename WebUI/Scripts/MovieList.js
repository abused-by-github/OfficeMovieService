$(function () {
    var viewModel = {
        movies: ko.observableArray()
    };

    ko.applyBindings(viewModel);

    $('#test').on('click', function() {
        window.movieService.core.api.call('movie', 'list', null, function(data) {
            viewModel.movies.removeAll();
            $.each(data, function(i, e) {
                viewModel.movies.push(e);
            });
        });
    });
});