$.extend({
    showWarning: function (msg, delay) {
        var warningMsg = "警告";
        if (arguments.length !== 0) {
            warningMsg = msg;
        }

        var defaultDelay = 3000;
        if (delay != null && typeof delay === "number") {
            defaultDelay = delay;
        }

        $('.js_tooltips').html(warningMsg);
        $('.js_tooltips').show();
        setTimeout(function () {
            $('.js_tooltips').hide();
        }, defaultDelay);
    }
});