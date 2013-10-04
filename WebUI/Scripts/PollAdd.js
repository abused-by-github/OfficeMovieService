$(function () {
    var api = window.movieService.core.api;
    var env = window.movieService.environment;
    var xKo = window.movieService.core.ko;

    $('[data-format=date]').datetimepicker({
        onSelect: function () {
            $(this).change();
        },
        minDate: 0,
        dateFormat: 'dd.mm.yy'
    });

    var viewModel = {
        Id: ko.observable(0),
        isEditMode: ko.observable(false),
        Name: ko.observable('Just a poll.'),
        ExpirationDate: xKo.observableDate(new Date()),
        ViewDate: xKo.observableDate(new Date()),

        save: function() {
            var data = {
                Name: this.Name(),
                ExpirationDate: this.ExpirationDate.iso(),
                ViewDate: this.ViewDate.iso(),
                Id: this.Id()
            };
            api.call('poll', 'save', data, this.onSaved);
        },

        load: function() {
            api.call('poll', 'GetCurrent', null, this.onLoaded);
        },

        cancel: function() {
            api.call('poll', 'CancelCurrent', null, this.onCanceled);
        },

        onSaved: function() {
            
        },

        onCanceled: function() {
            window.location.href = env.baseUrl + 'movie/list';
        },

        onLoaded: function (r) {
            viewModel.isEditMode(true);
            viewModel.Name(r.Data.Name);
            viewModel.ExpirationDate.iso(r.Data.ExpirationDate);
            viewModel.ViewDate.iso(r.Data.ViewDate);
            viewModel.Id(r.Data.Id);
        }
    };

    ko.applyBindings(viewModel);

    if (window.movieService.current.action == 'edit') {
        viewModel.load();
    }
});