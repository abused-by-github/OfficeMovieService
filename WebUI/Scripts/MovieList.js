$(function () {
    var api = window.movieService.core.api;

    var viewModel = {
        movies: ko.observableArray(),
        currentPage: 0,
        isPageLoading: false,

        loadMore: function() {
            var paging = { PageNumber: this.currentPage + 1, PageSize: 10 };
            this.isPageLoading = true;
            api.call('movie', 'list', paging, this.fetchMoreMoviesSuccess, this.fetchMoreMoviesComplete);
        },

        fetchMoreMoviesSuccess: function (r) {
            $.each(r.Data.Items, function (i, e) {
                viewModel.movies.push(e);
            });
            $(window).scrollTop($(document).height());
            ++viewModel.currentPage;
        },

        fetchMoreMoviesComplete: function () {
            viewModel.isPageLoading = false;
        }
    };

    ko.applyBindings(viewModel);

    viewModel.loadMore();

});