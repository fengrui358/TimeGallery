$(function () {    
    $('#registerSubmit').on('click', function () {
        var galleryName = $('#galleryName').val();
        if (galleryName == null || galleryName === '') {
            $.showWarning("相册名不能为空");
            return;
        }        
    });    
});