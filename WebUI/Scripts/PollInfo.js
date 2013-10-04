﻿$(function () {
    var api = window.movieService.core.api;
    var viewModel;

    var pollInfo = {
        load: function () {
            api.call('poll', 'GetPollMovies', {}, this.OnSuccess);
        },

        OnSuccess: function(r) {
            if (r.Status) {
                if (r.Data) {
                    viewModel = ko.mapping.fromJS(r.Data);
                    viewModel.showVoters = pollInfo.showVoters;
                    viewModel.CurrentVoters = ko.observableArray();
                    ko.applyBindings(viewModel);
                    $("#noPoll").hide();
                    $("#mainView").show();
                    $(window).scrollTop($(document).height());
                } else {
                    $("#mainView").hide();
                    $("#noPoll").show();
                }
            }
        },
        
        showVoters: function () {
            viewModel.CurrentVoters(this.Voters());
        }
    };


    pollInfo.load();

});