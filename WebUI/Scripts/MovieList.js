$(function () {
    var api = window.movieService.core.api;

    var MovieViewModel = function (data) {
        $.extend(this, data);
        this.Name = ko.observable(data.Name).extend({ required: true });
        this.Url = ko.observable(data.Url).extend({ required: true, url: true });
        this.ImageUrl = ko.observable(data.ImageUrl).extend({ required: true });

        this.errors = ko.validation.group(this);
    };

    MovieViewModel.prototype.validate = function () {
        var result = true;
        if (this.errors().length > 0) {
            this.errors.showAllMessages();
            result = false;
        }
        return result;
    };

    MovieViewModel.prototype.setData = function (data) {
        this.Name(data.Name);
        this.Url(data.Url);
        this.ImageUrl(data.ImageUrl);
        this.Id = data.Id;
        this.errors.showAllMessages(false);
    };

    MovieViewModel.prototype.setDefaultData = function () {
        this.setData(MovieViewModel.getDefaultData());
    };

    MovieViewModel.prototype.getData = function () {
        return {
            Name: this.Name(),
            Url: this.Url(),
            ImageUrl: this.ImageUrl(),
            Id: this.Id
        };
    };

    MovieViewModel.getDefault = function () {
        return new MovieViewModel(this.getDefaultData());
    };

    MovieViewModel.getDefaultData = function() {
        return {
            Name: '',
            Url: '',
            ImageUrl: '',
            Id: 0
        };
    };

    var viewModel = {
        movies: ko.observableArray(),
        currentPage: 0,
        isPageLoading: false,
        poll: window.movieService.poll,
        dialog: $("#saveDialog").dialog({ modal: true, autoOpen: false, resizable: false, width: 'auto', title: 'Add New Movie to Collection' }),
        loadMoreButton: $('#loadMoreButton'),
        currentMovie: MovieViewModel.getDefault(),

        cancelDialog: function () {
            this.dialog.dialog('close');
        },

        save: function () {
            var self = this;

            if (!this.currentMovie.validate()) {
                return;
            }

            api.call('movie', 'save', self.currentMovie.getData(), function (response) {
                if (!!response.Status) {
                    self.dialog.dialog('close');
                    self.reload();
                } else {
                    alert(response.ErrorMessage);
                }
            });
        },

        reload: function () {
            this.currentPage = 0;
            this.movies.removeAll();
            this.loadMore(true);
        },

        loadMore: function (doNotScroll) {
            this.loadMoreButton.twButton('loading');
            var paging = { PageNumber: this.currentPage + 1, PageSize: 10 };
            this.isPageLoading = true;
            api.call('movie', 'list', paging, function(r) {
                 viewModel.fetchMoreMoviesSuccess(r, doNotScroll);
            }, this.fetchMoreMoviesComplete);
        },

        fetchMoreMoviesSuccess: function (r, doNotScroll) {
            $.each(r.Data.Items, function (i, e) {
                e.IsVoting = ko.observable(false);
                e.IsVoted = ko.observable(e.IsVoted);
                viewModel.movies.push(e);
            });
            if (!doNotScroll) {
                $(window).scrollTop($(document).height());
            }
            ++viewModel.currentPage;
        },

        fetchMoreMoviesComplete: function () {
            viewModel.isPageLoading = false;
            viewModel.loadMoreButton.twButton('reset');
        },

        edit: function() {
            viewModel.currentMovie.setData(this);
            viewModel.dialog.dialog('open');
        },

        deleteMovie: function () {
            api.call('movie', 'delete', this.Id, function (response) {
                if (!!response.Status) {
                    viewModel.reload();
                } else {
                    alert(response.ErrorMessage);
                }
            });
        },

        openPopup: function () {
            this.currentMovie.setDefaultData();
            viewModel.dialog.dialog('open');
        },

        vote: function () {
            var movie = this;
            movie.IsVoting(true);
            api.call('poll', 'vote', { id: this.Id }, function () {
                movie.IsVoted(true);
            }, function() {
                movie.IsVoting(false);
            });
        },

        unvote: function () {
            var movie = this;
            movie.IsVoting(true);
            api.call('poll', 'unvote', { id: this.Id }, function () {
                movie.IsVoted(false);
            }, function () {
                movie.IsVoting(false);
            });
        }
    };

    ko.applyBindings(viewModel, document.getElementById("scrollContainer"));
    ko.applyBindings(window.movieService.poll, document.getElementById("pollSummary"));
    var addMovie = document.getElementById("addMovie");
    if (addMovie) {
        ko.applyBindings(viewModel, addMovie);
    }
    var saveDialog = document.getElementById("saveDialog");
    if (saveDialog) { //If signed in
        ko.applyBindings(viewModel, saveDialog);
    }

    var editPollContainer = document.getElementById('editPollContainer');
    if (editPollContainer) {
        ko.applyBindings(window.movieService.poll, editPollContainer);
    }

    window.movieService.poll.load();
    viewModel.loadMore(true);

});