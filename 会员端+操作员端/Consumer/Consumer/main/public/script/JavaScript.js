
//底部导航点击效果
$(function () {

    var btnUl = "<ul class='btnList'>";
    var btnHome = "<li><a id='home'><img src='/main/public/images/Icon/home.png' alt=''/><span>首页</span></a></li>";
    var btnOrder = "<li><a id='order'><img src='/main/public/images/Icon/order.png' alt=''/><span>订单</span></a></li>";
    var btnNotice = "<li><a id='notice'><img src='/main/public/images/Icon/tong.png' alt=''/><span>通知</span></a></li>";
    var btnMy = "<li><a id='member'><img src='/main/public/images/Icon/my.png' alt=''/><span>我的</span></a></li>";
    var btnFen = "<li><a id='fen'><img src='/main/public/images/Icon/fenx.png' alt=''/><span>分享</span></a></li>";
    var btnUlw = "</ul>";

    $(".homeNav").html(btnUl + btnHome + btnNotice + btnOrder + btnMy + btnUlw);



    var $url = window.location.pathname;
    if ($url == "/main/homeIndex.aspx") {
        $("#home").find("img").attr({ "src": "/main/public/images/Icon/homeB.png" });
        $("#home").find("span").addClass("spancolor");
        $("#iframe").attr("src", "home.aspx");
    } else if ($url == "/main/orderlist.aspx") {
        $("#order").find("img").attr({ "src": "/main/public/images/Icon/orderB.png" });
        $("#order").find("span").addClass("spancolor");
    } else if ($url == "/main/notice.aspx") {
        $("#notice").find("img").attr({ "src": "/main/public/images/Icon/tongB.png" });
        $("#notice").find("span").addClass("spancolor");
    } else if ($url == "/main/member.aspx") {
        $("#member").find("img").attr({ "src": "/main/public/images/Icon/myB.png" });
        $("#member").find("span").addClass("spancolor");
    }
    $("#home").click(function() {
        if ($url != "/main/homeIndex.aspx") {
            location.href = "homeIndex.aspx";
        }
    });
    $("#order").click(function() {
        if ($url != "/main/orderlist.aspx") {
            location.href = "orderlist.aspx";
        }
    });
    $("#notice").click(function() {
        if ($url != "/main/notice.aspx") {
            location.href = "notice.aspx";
        }
    });
    $("#member").click(function() {
        if ($url != "/main/member.aspx") {
            location.href = "member.aspx";
        }
    });

    var $jiQzhuang = $(".homeLleft").find("span");
    $jiQzhuang.each(function () {
        
        if ($(this).attr("name") == "2" || $(this).attr("name") == "7") {
            $(this).html("正在运行");
            $(this).removeClass("spancolor");
            $(this).parents("a").find("em").html("可订购");
            $(this).parents("li").removeClass("licolor");
            $(this).parents("a").removeClass("azhuang");
        } else if ($(this).attr("name") == "1" || $(this).attr("name") == "3"||$(this).attr("name") == "6") {
            
            $(this).html("故障 暂停销售");
            $(this).addClass("spancolor");
            $(this).parents("a").find("em").html("暂不可订购");
            $(this).parents("li").addClass("licolor");
            $(this).parents("a").addClass("azhuang").removeAttr("href");
        }
    });

});

//点击按钮返回上一页
function goBack() {
    history.back();
}