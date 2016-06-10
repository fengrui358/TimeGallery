$(function() {
    $('#search_input')
        .on('focus',
            function() {
                var $weuiSearchBar = $('#search_bar');
                $weuiSearchBar.addClass('weui_search_focusing');
            });

    $('#search_input')
        .on('blur',
            function() {
                var $weuiSearchBar = $('#search_bar');
                $weuiSearchBar.removeClass('weui_search_focusing');
                if ($(this).val()) {
                    $('#search_text').hide();
                } else {
                    $('#search_text').show();
                }
            });

    $('#search_input')
        .on('input',
            function() {
                var $searchShow = $('#search_show');
                var content = $(this).val();
                if (content) {
                    //todo:分页

                    $.post('SearchGalleryList',
                        { searchKey: content },
                        function(result) {                            
                            if (result.state !== 0) {
                                $.showWarning(result.message);
                                return;
                            }

                            if (result.state === 0) {
                                $searchShow.empty();

                                var resultHtml = '';
                                for (var i = 0; i < result.data.length; i++) {
                                    //todo:换成可点击的节点
                                    var d = result.data[i];
                                    resultHtml = resultHtml +
                                        '<div class="weui_cell"><div class="weui_cell_bd weui_cell_primary"><p>' + d.Name + '</p></div></div>';
                                }

                                $searchShow.append(resultHtml);
                                $searchShow.show();
                            }
                        }, "json");
                } else {
                    $searchShow.hide();
                }
            });

    $('#search_cancel')
        .on('touchend',
            function() {
                $('#search_show').hide();
                $('#search_input').val('');
            });

    $('#search_clear')
        .on('touchend',
            function () {
                $("#search_show").hide();
                $('#search_input').val('');
            });
});