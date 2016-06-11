var globalOptions = {

};

$(function () {
    initWidth();

    getMore();
});

function initWidth() {
    //todo:考虑翻转屏幕的情况
    if (window.screen.width > window.screen.height) {
        globalOptions.moreWidth = window.screen.width / 4;
    } else {
        globalOptions.moreWidth = window.screen.height / 4;
    }

    globalOptions.width = window.screen.width / 4;
}

function getMore(date) {
    $.post('Gallery/GetGalleryContents',
        { galleryId: $('#data-container').data('gallery-id'), date: date },
        function(result) {

            for (var i = 0; i < result.data.length; i++) {
                var groupHtmlStart = '<div class="row group-header"><p class="lead col-xs-8">' +
                    result.data[i].Date +
                    '</p><p class="col-xs-4 text-right">四川*成都</p></div><div class="row group-content">';

                for (var j = 0; j < result.data[i].ContentModels.length; j++) {
                    var contentHtml = '<img class="col-xs-3 nopadding" width="' +
                        globalOptions.width +
                        '" height="' +
                        globalOptions.width +
                        '" src="' +
                        result.data[i].ContentModels[j].Url + getImagePostfix() +
                        '">';
                    groupHtmlStart += contentHtml;
                }

                var groupHtmlEnd = '</div><br/>';
                groupHtmlStart += groupHtmlEnd;

                $('.content-container').append(groupHtmlStart);
            }
        },
        'json');
}

function getImagePostfix() {
    //更宽的部分

    var postfix = '?imageView2/1/w/' + parseInt(globalOptions.moreWidth) + '/h/' + parseInt(globalOptions.moreWidth);
    return postfix;
}