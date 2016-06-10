$(function() {
    getMore();
});

function getMore(date) {
    $.post('Gallery/GetGalleryContents',
        { galleryId: $('#data-container').data('gallery-id'), date: date },
        function (result) {
            var x = result;
        },
        'json');
}