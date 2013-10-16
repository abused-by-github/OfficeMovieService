$(function () {
    var api = window.movieService.core.api;

    var MovieViewModel = function (data) {
        var self = this;

        this.TmdbMovie = ko.observable();
        this.TmdbUrl = ko.observable();
        this.TmdbId = ko.observable();
        this.ImageUrl = ko.observable();
        this.Name = ko.observable().extend({ required: true });

        var needValidation = function () {
            return !self.TmdbId();
        };

        this.Url = ko.observable(data.Url).extend({
            required: {
                onlyIf: needValidation
            },
            url: {
                onlyIf: needValidation
            }
        });

        this.CustomImageUrl = ko.observable(data.CustomImageUrl).extend({
            required: {
                onlyIf: needValidation
            },
            url: {
                onlyIf: needValidation
            }
        });

        this.TmdbId.subscribe(function (newValue) {
            if (newValue) {
                self.Url("http://www.themoviedb.org/movie/" + newValue);
                self.CustomImageUrl(null);
            }
        });

        this.errors = ko.validation.group(this);

        this.setData(data);
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
        this.CustomImageUrl(data.CustomImageUrl);
        this.Id = data.Id;
        this.ImageUrl(data.ImageUrl);
        this.TmdbId(data.TmdbMovie && data.TmdbMovie.TmdbId);
        this.TmdbUrl(data.TmdbMovie && data.TmdbMovie.PosterPath);
        this.TmdbMovieId = data.TmdbMovieId;
        this.TmdbMovie(data.TmdbMovie);
        this.errors.showAllMessages(false);
    };

    MovieViewModel.prototype.setDefaultData = function () {
        this.setData(MovieViewModel.getDefaultData());
    };

    MovieViewModel.prototype.getData = function () {
        return {
            Name: this.Name(),
            Url: this.Url(),
            CustomImageUrl: this.CustomImageUrl(),
            Id: this.Id,
            TmdbMovieId: this.TmdbMovieId || null,
            TmdbMovie: this.TmdbId() ? { TmdbId: this.TmdbId(), PosterPath: this.TmdbUrl() } : null
        };
    };

    MovieViewModel.prototype.clearTmdb = function () {
        this.TmdbMovie(null);
        this.TmdbMovieId = 0;
        this.TmdbId(0);
    };

    MovieViewModel.getDefault = function () {
        return new MovieViewModel(this.getDefaultData());
    };

    MovieViewModel.getDefaultData = function() {
        return {
            Name: '',
            Url: '',
            ImageUrl: '',
            CustomImageUrl: null,
            Id: 0,
            TmdbMovieId: 0,
            TmdbMovie: null
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
        LeftVotes: ko.observable(),
        inviteFriend: new window.movieService.InviteFriendViewModel(),
        totalMovies: ko.observable(0),
        cancelDialog: function () {
            viewModel.dialog.dialog('close');
        },

        save: function () {
            var self = viewModel;

            if (!self.currentMovie.validate()) {
                return;
            }

            api.call('movie', 'save', self.currentMovie.getData(), function (response) {
                if (!!response.Status) {
                    self.dialog.dialog('close');
                    self.reload();
                    self.updateRatings();
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
            var paging = { PageNumber: this.currentPage + 1, PageSize: 12 };
            this.isPageLoading = true;
            window.waiter.show({ targetId: 'body' });
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
            viewModel.totalMovies(r.Data.Total);
            if (!doNotScroll) {
                $(window).scrollTop($(document).height());
            }
            ++viewModel.currentPage;
            viewModel.LeftVotes(r.Data.leftVotes);
        },

        fetchMoreMoviesComplete: function () {
            viewModel.isPageLoading = false;
            window.waiter.hide({ targetId: 'body' });
            viewModel.loadMoreButton.twButton('reset');
        },

        edit: function() {
            viewModel.currentMovie.setData(this);
            viewModel.dialog.dialog('open');
        },

        deleteMovie: function () {
            if (!confirm("Are you sure you want to delete movie?"))
                return;
            api.call('movie', 'delete', this.Id, function (response) {
                if (!!response.Status) {
                    viewModel.reload();
                    viewModel.updateRatings();
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
                viewModel.updateRatings();
                viewModel.LeftVotes(viewModel.LeftVotes() - 1);
            }, function() {
                movie.IsVoting(false);
            });
        },

        unvote: function () {
            var movie = this;
            movie.IsVoting(true);
            api.call('poll', 'unvote', { id: this.Id }, function () {
                movie.IsVoted(false);
                viewModel.updateRatings();
                viewModel.LeftVotes(viewModel.LeftVotes() + 1);
            }, function () {
                movie.IsVoting(false);
            });
        },

        updateRatings: function () {
            if (pollInfo && pollInfo.load) {
                pollInfo.load();
            }
        },

        tmdbAutocomplete: function (term, onDone) {
            $.ajax({
                url: "http://api.themoviedb.org/3/search/movie?api_key=b2b05f39c1a1b7cdf7d32f076edb450d&search_type=ngram&query=" + term,
                dataType: "jsonp",
                data: null,
                success: function (r) {
                    onDone(r.results);
                }
            });
        }
    };

    viewModel.hasMoreMovies = ko.computed(function() {
        return this.totalMovies() > this.movies().length;
    }, viewModel);

    $('#scrollContainer, #addMovie, #saveDialog').koBind(viewModel);
    $('#pollSummary, #editPollContainer').koBind(window.movieService.poll);

    window.movieService.poll.load();
    viewModel.loadMore(true);

    $("#errorDiv").on("click", function() {
        $(this).hide();
    });

});