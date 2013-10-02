$(function() {
    $('#test').on('click', function() {
        window.movieService.core.api.call('movie', 'list', null, function() {
            
        });
    });
});