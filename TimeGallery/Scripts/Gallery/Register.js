$(function() {
    $('#registerSubmit')
        .on('click',
            function() {
                var galleryName = $('#galleryName').val();
                if (galleryName == null || galleryName === '') {
                    $.showWarning("相册名不能为空");
                    return;
                }

                $('#loadingToast').show();
                $.post("RegisterSubmit",
                    { name: galleryName, description: $('#galleryDescription').val() },
                    function(data) {
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
                    },
                    "json");
            });

    $('.weui_uploader_input')
        .on('change',
            function() {
                if (this.files.length > 0) {
                    try {
                        var file = this.files[0];
                        var reader = new FileReader();
                        if (/image\/\w+/.test(file.type)) {
                            reader.onload = function() {
                                //todo:修改上传图片的显示
                                $('#galleryCoverImg').attr('src', this.result);                                
                            }

                            reader.readAsDataURL(file);
                        }
                    } catch (e) {
                        console.debug(e);
                    }
                }
            });
});