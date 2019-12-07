<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="balance.aspx.cs" Inherits="Consumer.main.balance" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>余额变动-自助售卖系统</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />
    <link rel="shortcut icon" href="/main/public/images/favicon.ico" />
    <link href="/main/public/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/common.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/style.css" rel="stylesheet" type="text/css" />
    <script src="/main/public/script/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="/main/public/script/JavaScript.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <main class="setupq">
            <h4 class="commontitle">
                <i class="fa fa-angle-left" onclick="goBack()"></i>
                余额积分变动
            </h4>
            <section class="changelist">
                <ul id="yuelist">
                   
                </ul>
            </section>
        </main>
        <input id="memberID" runat="server" type="hidden" />
          <input  id="flag" runat="server" type="hidden"/>
    </form>
</body>
</html>
<script>
    if (/MicroMessenger/.test(window.navigator.userAgent)) {
        //微信浏览器
        $("#flag").val(1);

    } else {

        //其他浏览器
        $("#flag").val(0);
    }
    $(function () {
        var $Span = $("#yuelist").find("li").find(".listright").find("h4");
        $Span.each(function () {
            if (parseInt($(this).find("em").html()) <= 0) {
                $(this).addClass("h4color");
            }
        });
        sear();
    })
    function sear()
    {
       $("#yuelist").empty();
        $.ajax({
            type: "post",
            url: "balance.aspx/sear",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{memberID:'" + $("#memberID").val() + "'}",
            success: function (data) {
                var serverdata = $.parseJSON(data.d);
                var serverdatalist = serverdata.length;
                for (var i = 0; i < serverdatalist; i++) {
                    if (serverdata[i].type == "1" || serverdata[i].type == "4" || serverdata[i].bz == "会员转账收入" || serverdata[i].type == "5"|| serverdata[i].type == "7")//充值  转售红色
                    {
                        $("  <li>"
                          + " <div class='listleft'>"
                          + "     <h4>" + serverdata[i].bz + "</h4>"
                          + "     <p>" + serverdata[i].payTime + "</p>"
                          + " </div>"
                          + " <div class='listright'>"
                          + "     <h4 style='color:#fa3428'>"
                          + "         <em>+" + serverdata[i].money + "</em>"
                          + "         元"
                          + "     </h4>"
                          + "     <p>余额：" + serverdata[i].AvaiilabMOney + "元</p>"
                          + " </div>"
                       + "</li>").appendTo("#yuelist");
                    } else {
                        $("  <li>"
                         + " <div class='listleft'>"
                         + "     <h4>" + serverdata[i].bz + "</h4>"
                         + "     <p>" + serverdata[i].payTime + "</p>"
                         + " </div>"
                         + " <div class='listright'>"
                         + "     <h4 style='color:#2dbb5f'>"
                         + "         <em>-" + serverdata[i].money + "</em>"
                         + "         元"
                         + "     </h4>"
                         + "     <p>余额：" + serverdata[i].AvaiilabMOney + "元</p>"
                         + " </div>"
                      + "</li>").appendTo("#yuelist");
                    }
                   
                }
            }
        })
    }
</script>
