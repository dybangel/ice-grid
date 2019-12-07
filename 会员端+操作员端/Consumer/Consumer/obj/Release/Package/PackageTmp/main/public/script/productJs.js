$(function() {
    

    var linum = $('.product_hot ul').children('li').length;
    var lileng = $('.product_hot ul').children('li').width() + 16;
    var jieguo = linum * lileng;
    $(".product_hot ul").width(jieguo);

    var bodywidth = $(".body").width();
    $(".body").height(bodywidth * 3);
    $(".main_right").height(bodywidth * 2 - 100);
});

type = "text/javascript" >
    $(function () {
        //获取要定位元素距离浏览器顶部的距离
        var navH = $(".product_main").offset().top;

        //滚动条事件
        $(window).scroll(function () {
            //获取滚动条的滑动距离
            var scroH = $(this).scrollTop();
            //滚动条的滑动距离大于等于定位元素距离浏览器顶部的距离，就固定，反之就不固定
            if (scroH >= navH) {
                $(".main_left").css({ "position": "fixed", "top": 0 });
                $(".main_right").css({ "margin-left": "26%" });
            } else if (scroH < navH) {
                $(".main_left").css({ "position": "relative" });
                $(".main_right").css({ "margin-left": "0" });
            }
        });
    });

$(document).ready(function () {
    var anum = $(".main_list li").length;
    var listh = $(".main_list li").height();
    var aheight = anum * listh;
    var i = 0;
    $(".main_left a").each(function () {
        var idbtn = "btn" + i;
        $(this).attr("id", idbtn);
        i++;
    });
    var j = 0;
    $(".main_list").each(function () {
        var idlist = "list" + j;
        $(this).attr("id", idlist);
        j++;
    });

    $(".main_left a").click(function () {
        $(".main_left a").removeClass("a_col");
        $(this).addClass("a_col");
        var listNub = $(this).index();
        $('body,html').animate({
            scrollTop: $("#list" + listNub).offset().top
        }, 500);
    });

    $(window).scroll(function () {
        var scrlloHe = $(this).scrollTop();
        var x = 0;
        $(".main_list").each(function () {
            var listHe = $("#list" + x).offset().top - 280;
            var listNum = $("#list" + x).index();
            if (scrlloHe >= listHe) {
                $('.main_left a').eq(listNum).addClass("a_col").siblings('.main_left a').removeClass("a_col");
            }
            x++;

        });
        var obj = $(".main_list:last");//元素
        var h = obj.height();//元素高度
        obj.offset().top;//元素距离顶部高度

        var wh = $(window).height();//浏览器窗口高度
        $(document).scrollTop();//滚动条高度
        var xh = wh - (h + obj.offset().top - $(document).scrollTop());//元素到浏览器底部的高度
        if (xh >= -30) {
            $(".main_left a:last").addClass("a_col").siblings('.main_left a').removeClass("a_col");
        }
    });

});
