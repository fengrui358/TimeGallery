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
    },

    closePage: function() {
        if (navigator.userAgent.indexOf("MSIE") > 0) {
            if (navigator.userAgent.indexOf("MSIE 6.0") > 0) {
                window.opener = null;
                window.close();
            } else {
                window.open('', '_top');
                window.top.close();
            }
        }
        else if (navigator.userAgent.indexOf("Firefox") > 0) {
            window.location.href = 'about:blank ';
        } else {
            window.opener = null;
            window.open('', '_self', '');
            window.close();
        }
    }
});