$(function () {
    
    window.movieAdd = {
        init: function() {
            $('#btnAdd').on('click', function () {
                showValidation(false);
                var name = $("#txtName").val().trim();
                var url = $("#txtUrl").val();

                if (name.length == 0) {
                    showValidation(true);
                    return;
                }
                

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
    
});