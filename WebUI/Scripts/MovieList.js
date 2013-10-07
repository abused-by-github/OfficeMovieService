$(function () {
    var api = window.movieService.core.api;
    var xKo = window.movieService.core.ko;

    var viewModel = {
        movies: ko.observableArray(),
        currentPage: 0,
        isPageLoading: false,
        poll: ko.observable(),
        dialog: $("#saveDialog").dialog({ modal: true, autoOpen: false, resizable: false, width: 'auto', title: 'Add New Movie to Collection' }),
        loadMoreButton: $('#loadMoreButton'),
        currentMovie: ko.observable({ Name: "", Url: "" }),
        
        cancelDialog: function () {
            this.dialog.dialog('close');
        },
        
        save: function () {
            var self = this;
            self.showValidation($("#lblNameError"), false);
            self.showValidation($("#lblUrlError"), false);

            if (!self.currentMovie().Name || self.currentMovie().Name.length == 0) {
                self.showValidation($("#lblNameError"), true);
                return;
            }

            api.call('movie', 'save', self.currentMovie(), function (response) {
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
            viewModel.currentMovie(this);
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
            this.currentMovie({ Name: "", Url: "" });
            viewModel.dialog.dialog('open');
        },
        
        showValidation: function (field, show) {
            if (show) {
                field.show();
            } else {
                field.hide();
            }
        },

        loadPoll: function() {
            api.call('poll', 'GetCurrent', null, this.pollLoaded);
        },

        pollLoaded: function (r) {
            if (r.Data) {
                r.Data.ViewDate = xKo.observableDate(new Date(r.Data.ViewDate));
                r.Data.ExpirationDate = xKo.observableDate(new Date(r.Data.ExpirationDate));
            }

            viewModel.poll(r.Data);
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
    ko.applyBindings(viewModel, document.getElementById("scrollContainerPoll"));
    ko.applyBindings(viewModel, document.getElementById("saveDialog"));

    viewModel.loadPoll();
    viewModel.loadMore(true);

});