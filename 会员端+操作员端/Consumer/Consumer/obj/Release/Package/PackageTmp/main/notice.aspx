<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="notice.aspx.cs" Inherits="Consumer.main.notice" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>通知-自助售卖系统</title>
    <meta charset="utf-8" />
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
        <main class="main">
           
            <section class="noticelist">
                <ul id="ull">
                    
                </ul>
            </section>
        </main>
        <section class="homeNav"></section>
        <input id="memberID" runat="server" type="hidden"/>
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
        sear();
    });
    function sear()
    {
        
        $.ajax({
            type: "post",
            url: "notice.aspx/sear",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{memberID:'" + $("#memberID").val() + "'}",
            success: function (data) {
                var serverdata = $.parseJSON(data.d);
                var serverdatalist = serverdata.length;
                for (var i = 0; i < serverdatalist; i++) {
                    $("  <li>"
                       +" <a href='#'>"
                       + "     <span>" + serverdata[i].time + "</span>"
                       + "     <h4>" + serverdata[i].title + "</h4>"
                       + "     <p>" + serverdata[i].con + "</p>"
                       +"     <i class='fa fa-angle-right'></i>"
                       +" </a>"
                       +"</li>").appendTo("#ull");
                }
            }
        })
    }
</script>
