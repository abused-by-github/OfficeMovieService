$(function () {
    var api = window.movieService.core.api;
    //var xKo = window.movieService.core.ko;
    var viewModel = {Movies: ko.observableArray()};

    window.pollInfo = {
        load: function () {
            api.call('poll', 'GetPollMovies', {}, this.OnSuccess);
        },

        OnSuccess: function(r) {
            if (r.Status) {
                if (r.Data && r.Data.Movies && r.Data.Movies.length > 0) {
                    viewModel.Movies(r.Data.Movies);
                    //viewModel = ko.mapping.fromJS(r.Data);
                    //viewModel.Poll.ExpirationDate = xKo.observableDate(new Date(r.Data.Poll.ExpirationDate));
                    $("#ratedMovies").show();
                    $("#noMovies").hide();
                    //$("#noPoll").hide();
                    //$("#mainView").show();
                    //$(window).scrollTop($(document).height());
                } else {
                    $("#ratedMovies").hide();
                    $("#noMovies").show();
                    //$("#mainView").hide();
                    //$("#noPoll").show();
                }
            }
        }
    };


    pollInfo.load();
    ko.applyBindings(viewModel, document.getElementById("votesContainer"));

});