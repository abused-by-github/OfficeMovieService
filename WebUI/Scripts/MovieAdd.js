$(function () {
    
    window.movieAdd = {
        init: function() {
            $('#btnAdd').on('click', function () {
                movieAdd.showValidation(false);
                var name = $("#txtName").val().trim();
                var url = $("#txtUrl").val();

                if (name.length == 0) {
                    movieAdd.showValidation(true);
                    return;
                }
                
                window.movieService.core.api.call('movie', 'add', {Name: name, Url: url}, function (response) {
                    if (!!response.Status) {
                        window.location.href = "http://localhost/MovieService/Movie/List";
                    } else {
                        alert(response.ErrorMessage);
                    }
                });
            });
        },
        
        showValidation: function(show) {
            if (show) {
                $("#lblNameError").show();
            } else {
                $("#lblNameError").hide();
            }
        }
    };
    
    window.movieAdd.init();
});
