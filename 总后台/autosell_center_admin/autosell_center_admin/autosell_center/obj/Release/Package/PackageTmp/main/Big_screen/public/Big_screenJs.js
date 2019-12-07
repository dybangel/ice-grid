

//  页面交互效果
$(function () {
    setInterval(function () {
        $(".screenNewsShow").fadeIn();
    }, 3000);
    setInterval(function () {
        $(".screenNewsShow").fadeOut();
    }, 6000);

    var num_s = $(".screenBigData").find("dl").find("dd").find("input");
    num_s.each(function () {
        var numOen = $(this).val();
        $(this).parent().find("h4").find("b").text(parseFloat(numOen).toLocaleString());
    });

    //  右侧消息实时推送 "0"表示不滚动，"1"表示滚动
    var offOn = $(".Off_On");
    if (offOn.find("input").val() == "0") {
        offOn.find("i").addClass("icolor");
        offOn.find("em").addClass("emcolor");
        $(".screenNewsList").find(".maquee").find("ul").addClass("ulOff");
    } else if (offOn.find("input").val() == "1") {
        offOn.find("i").removeClass("icolor");
        offOn.find("em").removeClass("emcolor");
        $(".screenNewsList").find(".maquee").find("ul").removeClass("ulOff");
    }
    offOn.click(function () {
        if (offOn.find("input").val() == "0") {// 开启推送
            offOn.find("i").removeClass("icolor");
            offOn.find("em").removeClass("emcolor");
            offOn.find("input").val("1");
            $(".screenNewsList").find(".maquee").find("ul").removeClass("ulOff");
            $("#indexNewsShowIs_").text("开启推送").fadeIn();
            setTimeout(function() {
                $("#indexNewsShowIs_").fadeOut();
            }, 1000);
        } else if (offOn.find("input").val() == "1") {// 关闭推送
            offOn.find("i").addClass("icolor");
            offOn.find("em").addClass("emcolor");
            offOn.find("input").val("0");
            $(".screenNewsList").find(".maquee").find("ul").addClass("ulOff");
            $("#indexNewsShowIs_").text("关闭推送").fadeIn();
            setTimeout(function () {
                $("#indexNewsShowIs_").fadeOut();
            }, 1000);
        }
    });

    //  查询城市回车事件
    $("#cityName").focus(function () {
        $(document).keyup(function (event) {
            if (event.keyCode == 13) {
                $("#query").click();
            }
        });
    });
});

//  重新加载echart图表
function DataSeachShuan() {
    $('#smallechartIframe').attr('src', $('#smallechartIframe').attr('src'));
    //$(".screenDataSeachChart").load("big_screen.aspx #screenSeachChart");
};

//  放大缩小echart图表
function DataSeachBig() {
    $(".commonBg").show();
    $(".EchartBig").addClass("EchartBigWandH");
    setTimeout(function () {
        $('#bigechartIframe').attr('src', $('#bigechartIframe').attr('src'));
        //$("#screenSeachChartBig").load("big_screen.aspx #screenSeachChartBig");
    }, 500);
}

function DataSeachSmall() {
    $(".EchartBig").removeClass("EchartBigWandH");
    setTimeout(function() {
        $(".commonBg").hide();
    }, 300);
}

function DataSeachBtn() {
    $(".screenseachMain").addClass("screenseachMainH");
}

function offscreenseachMain() {
    $(".screenseachMain").removeClass("screenseachMainH");
}
//  右侧查询事件


//  显示隐藏数据图层事件
function dataCengBtn(obj) {
    if ($(obj).val() == "隐藏数据图层") {
        $("section").hide();
        $(".screenTitle").show();
        $(obj).val("显示数据图层");
    } else {
        $("section").show();
        $(obj).val("隐藏数据图层");
    }
}

//  当屏幕大小发生变化时
//window.onresize = function () {
//    $('#smallechartIframe').attr('src', $('#smallechartIframe').attr('src'));
//    $('#bigechartIframe').attr('src', $('#bigechartIframe').attr('src'));
//}


// 按下键盘时触发事件
//document.onkeydown= function(event) {
    
//}