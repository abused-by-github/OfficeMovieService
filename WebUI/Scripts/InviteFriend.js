(function () {
    var api = window.movieService.core.api;
    var ui = window.movieService.core.ui;

    var InviteFriendViewModel = function () {
        this.Email = ko.observable('').extend({ required: true, email: true });
        this.errors = ko.validation.group(this);
    };

    InviteFriendViewModel.prototype.activate = function () {
        this.setDefaultData();
        this.errors.showAllMessages(false);
        $('#inviteFriendDialog').dialog({
            width: 'auto',
            minHeight: 'auto',
            title: 'Invite Friend',
            modal: true
        });
    };

    InviteFriendViewModel.prototype.setDefaultData = function() {
        this.Email('');
    };

    InviteFriendViewModel.prototype.invite = function () {
        if (this.validate()) {
            var data = { Name: this.Email() };
            api.call('account', 'invite', data, function() {
                ui.showInfo('User with email ' + data.Email + ' has been added. Now he/she can sign in.');
                $('#inviteFriendDialog').dialog('close');
            });
        }
    };

    InviteFriendViewModel.prototype.validate = function () {
        var result = true;
        if (this.errors().length > 0) {
            this.errors.showAllMessages();
            result = false;
        }
        return result;
    };

    window.movieService.InviteFriendViewModel = InviteFriendViewModel;
})();