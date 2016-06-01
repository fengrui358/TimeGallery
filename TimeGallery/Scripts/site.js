$.extend({
    showWarning: function (msg) {
        var warningMsg = "警告";
        if (arguments.length === 1) {
            warningMsg = msg;
        }

        $('.js_tooltips').show();
        setTimeout(function () {
            $('.js_tooltips').hide();
        }, 3000);
    }
});