(function () {
    var api = window.movieService.core.api;
    var xKo = window.movieService.core.ko;

    var PollViewModel = function (data) {
        this.Name = ko.observable(data.Name).extend({ required: true });
        this.ViewDate = xKo.observableDate(new Date(data.ViewDate));
        this.ExpirationDate = xKo.observableDate(new Date(data.ExpirationDate));
        this.Winner = ko.observable(data.Winner);
        this.Id = ko.observable(data.Id);
        this.IsVoteable = ko.observable(data.IsVoteable);
        this.IsMine = ko.observable(data.IsMine);
    };

    PollViewModel.prototype.load = function () {
        var self = this;

        api.call('poll', 'GetCurrent', null, function (r) {
            self.setData(r.Data || PollViewModel.getDefaultData());
        });
    };

    PollViewModel.prototype.save = function () {
        var self = this;

        api.call('poll', 'save', this.getData(), function (r) {
            $('#editPollContainer').dialog('close');
            self.setData(r.Data);
        });
    };

    PollViewModel.prototype.cancel = function () {
        var self = this;

        api.call('poll', 'CancelCurrent', null, function () {
            $('#editPollContainer').dialog('close');
            self.setDefaultData();
        });
    };

    PollViewModel.prototype.edit = function () {
        $('#editPollContainer').dialog({
            modal: true,
            autoOpen: true,
            resizable: false,
            width: 'auto',
            title: 'Edit Poll'
        });
    };

    PollViewModel.prototype.getData = function () {
        return {
            Name: this.Name(),
            ExpirationDate: this.ExpirationDate.iso(),
            ViewDate: this.ViewDate.iso(),
            Id: this.Id()
        };
    };

    PollViewModel.prototype.setData = function (data) {
        this.Name(data.Name);
        this.ViewDate.iso(data.ViewDate);
        this.ExpirationDate.iso(data.ExpirationDate);
        this.Winner(data.Winner);
        this.Id(data.Id);
        this.IsVoteable(data.IsVoteable);
        this.IsMine(data.IsMine);
    };

    PollViewModel.getDefaultData = function () {
        return {
            Name: '',
            ViewDate: null,
            ExpirationDate: null,
            Winner: null,
            Id: 0,
            IsVoteable: false,
            IsMine: false
        };
    };

    PollViewModel.prototype.setDefaultData = function () {
        this.setData(PollViewModel.getDefaultData());
    };

    PollViewModel.getDefault = function () {
        return new PollViewModel(PollViewModel.getDefaultData());
    };

    window.movieService.poll = PollViewModel.getDefault();
})();