$(function() {
    var inviteViewModel = new window.movieService.InviteFriendViewModel();
    $('#buttonInvite, #inviteFriendDialog').koBind(inviteViewModel);
});
