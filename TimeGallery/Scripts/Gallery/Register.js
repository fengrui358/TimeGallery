$(function() {
    $('#registerSubmit').on('click', function() {
        var galleryName = $('#galleryName').val();
        if (galleryName == null || galleryName === '') {
            $.showWarning("相册名不能为空");
            return;
        }

        $('#loadingToast').show();
        $.post("RegisterSubmit", { name: galleryName, description: $('#galleryDescription').val() }, function (data) {
            $('#loadingToast').hide();
            
            if (data.state !== 0) {
                $.showWarning(data.message);
                return;
            }

            if (data.state === 0) {
                $('.weui_msg .weui_msg_desc').html(data.message);

                $('#registerContainer').hide();
                $('.weui_msg').show();
            }
        }, "json");
    });

    //$('.weui_msg .weui_btn_default').on('click', function() {
    //    window.close();
    //});
});