var Login = function () {

    var handleLogin = function () {

        $('#btnsubmit').click(function () {
            formLogin();
        })
        $('input').keypress(function (e) {
            if (e.which == 13) {
                formLogin();
            }
        });

        function formLogin() {
            $("#spanerror").hide().text('');
            var obj = $('#loginform').serialize();
            //obj += '&rememberMe=' + false;
            $.post("/Admin/Login/Login", obj, function (data) {
                if (data.Success) {
                    var returl = $("#btnsubmit").attr("returl");
                    if (returl) {
                        window.location.href = returl;
                    } else {
                        window.location.href = "/Admin/Home/Index";
                    }
                }
                else {
                    $("#spanerror").show().text(data.ErrorMsg);
                }
            })
        }
    }
    return {
        //main function to initiate the module
        init: function () {
            handleLogin();
        }

    };

}();

jQuery(document).ready(function () {
    Login.init();
});