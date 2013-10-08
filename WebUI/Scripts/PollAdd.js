$(function () {
    
    var env = window.movieService.environment;
    var xKo = window.movieService.core.ko;


    var viewModel = {
        Id: ko.observable(0),
        isEditMode: ko.observable(false),
        Name: ko.observable('Just a poll.'),
        ExpirationDate: xKo.observableDate(new Date()),
        ViewDate: xKo.observableDate(new Date()),


        onCanceled: function() {
            window.location.href = env.baseUrl + 'movie/list';
        },

        setData: function (data) {
            viewModel.isEditMode(!!data.Id);
            viewModel.Name(data.Name);
            viewModel.ExpirationDate.iso(data.ExpirationDate);
            viewModel.ViewDate.iso(data.ViewDate);
            viewModel.Id(data.Id);
        },

        setDefaultData: function() {
            this.setData(this.getDefaultData());
        },

        getDefaultData: function() {
            return {
                Name: '',
                ExpirationDate: new Date().toISOString(),
                ViewDate: new Date().toISOString(),
                Id: 0
            };
        }
    };

    var bindingRoot = document.getElementById('editPollContainer');
    if (bindingRoot) {
        ko.applyBindings(viewModel);
    }

    window.movieService.editPoll = viewModel;
});