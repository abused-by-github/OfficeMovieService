$(function () {
    var api = window.movieService.core.api;
    var viewModel = {Movies: ko.observableArray()};

    window.pollInfo = {
        load: function () {
            api.call('poll', 'GetPollMovies', {}, this.OnSuccess);
        },

        OnSuccess: function(r) {
            if (r.Status) {
                if (r.Data && r.Data.Movies && r.Data.Movies.length > 0) {
                    viewModel.Movies(r.Data.Movies);
                    $("#ratedMovies").show();
                    $("#noMovies").hide();
                } else {
                    $("#ratedMovies").hide();
                    $("#noMovies").show();
                }
            }
        }
    };


    pollInfo.load();
    ko.applyBindings(viewModel, document.getElementById("votesContainer"));

});