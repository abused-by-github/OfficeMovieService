$(function () {
    var api = window.movieService.core.api;
    var viewModel;

    var pollInfo = {
        load: function () {
            api.call('poll', 'getcurrent', {}, this.fetchMoreMoviesSuccess);
        },

        fetchMoreMoviesSuccess: function(r) {
            if (r.Status) {
                viewModel = ko.mapping.fromJS(r.Data);
                viewModel.showVoters = pollInfo.showVoters;
                viewModel.CurrentVoters = ko.observable({Voters: ko.observableArray()});
                ko.applyBindings(viewModel, document.getElementById("mainView"));
                ko.applyBindings(viewModel.CurrentVoters, document.getElementById("voters"));
                $(window).scrollTop($(document).height());
            }
        },
        
        showVoters: function () {
            viewModel.CurrentVoters(this);
        }
    };


    pollInfo.load();

});