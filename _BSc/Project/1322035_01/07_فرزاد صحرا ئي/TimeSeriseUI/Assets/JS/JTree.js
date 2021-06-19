$(function () {
    //$('.tree1  li:has(ul)').addClass('parent_li').find(' > span').attr('title', 'باز کردن این شاخص');

    $('.tree1 li.parent_li > span').on('click', function (e) {
        var children = $(this).parent('li.parent_li').find(' > ul > li');
        if (children.is(":visible")) {
            children.hide('fast');
            $(this).attr('title', 'بازکردن این شاخص')
            //$(this).attr('title', 'بازکردن این شاخه').find(' > i').addClass('glyphicon-chevron-left').removeClass('glyphicon-chevron-down');
            $(this).removeClass("ClickTree");
            $(this).closest("ul").find(' > li > ul').removeClass('bckColor')
            $(this).find('> i').toggleClass("down");
        } else {
            children.show('fast');
            $(this).attr('title', 'بستن این شاخص')
            //$(this).attr('title', 'بستن این شاخه').find(' > i').addClass('glyphicon-chevron-down').removeClass('glyphicon-plus-sign');
            $(this).addClass("ClickTree");

            $(this).closest("ul").find(' > li > ul').addClass('bckColor')
            $(this).find('> i').toggleClass("down");
        }
        e.stopPropagation();
    });

});