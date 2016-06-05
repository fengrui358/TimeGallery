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
                var $searchShow = $("#search_show");
                if ($(this).val()) {
                    $searchShow.show();
                } else {
                    $searchShow.hide();
                }
            });

    $('#search_cancel')
        .on('touchend',
            function() {
                $("#search_show").hide();
                $('#search_input').val('');
            });

    $('#search_clear')
        .on('touchend',
            function () {
                $("#search_show").hide();
                $('#search_input').val('');
            });
});