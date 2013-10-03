$(function () {
    var api = window.movieService.core.api;

    var viewModel = {
        movies: ko.observableArray(),
        currentPage: 0,
        isPageLoading: false,
        dialog: $("#saveDialog"),
        currentMovie: ko.observable({Name: "", Url: ""}),
        
        cancelDialog: function () {
            this.dialog.hide();
        },
        
        save: function () {
            var self = this;
            self.showValidation($("#lblNameError"), false);
            self.showValidation($("#lblUrlError"), false);

            if (!self.currentMovie().Name || self.currentMovie().Name.length == 0) {
                self.showValidation($("#lblNameError"), true);
                return;
            }

            //var url = self.currentMovie().url;
            //if (url && url.length > 0 && !self.validURL(url)) {
            //    self.showValidation($("#lblUrlError"), true);
            //    return;
            //}

            api.call('movie', 'save', self.currentMovie(), function (response) {
                if (!!response.Status) {
                    self.dialog.hide();
                    this.currentPage = 0;
                    viewModel.loadMore();
                } else {
                    alert(response.ErrorMessage);
                }
            });
        },

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
        },

        fetchMoreMoviesComplete: function () {
            viewModel.isPageLoading = false;
        },
        
        edit: function() {
            viewModel.currentMovie(this);
            viewModel.dialog.show();
        },

        deleteMovie: function () {
            api.call('movie', 'delete', this.Id, function (response) {
                if (!!response.Status) {
                    this.currentPage = 0;
                    viewModel.loadMore();
                } else {
                    alert(response.ErrorMessage);
                }
            });
        },
        
        openPopup: function () {
            this.currentMovie({ Name: "", Url: "" });
            viewModel.dialog.show();
        },
        
        showValidation: function (field, show) {
            if (show) {
                field.show();
            } else {
                field.hide();
            }
        }
    };

    ko.applyBindings(viewModel);

    viewModel.loadMore();

});