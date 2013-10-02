$(function () {
    var api = window.movieService.core.api;

    var viewModel = {
        movies: ko.observableArray(),
        currentPage: 0,
        isPageLoading: false
    };

    ko.applyBindings(viewModel);

    var fetchMoreMovies = function () {
        var paging = { PageNumber: viewModel.currentPage + 1 };
        viewModel.isPageLoading = true;
        api.call('movie', 'list', paging, fetchMoreMoviesSuccess, fetchMoreMoviesComplete);
    };

    var fetchMoreMoviesSuccess = function (r) {
        $.each(r.Data.Items, function (i, e) {
            viewModel.movies.push(e);
        });
    };

    var fetchMoreMoviesComplete = function () {
        viewModel.isPageLoading = false;
    };

    $('#scrollContainer').scroll(function () {
        if ($(window).scrollTop() >= $('#scrollContainer').height() - $(window).height() - 10) {
            fetchMoreMovies();
        }
    });

    fetchMoreMovies();
});