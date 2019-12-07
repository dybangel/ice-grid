<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Resalerecord.aspx.cs" Inherits="Consumer.main.Resalerecord" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>转售记录-自助售卖系统</title>
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
        <main class="setupq">
            <h4 class="commontitle">
                <i class="fa fa-angle-left" onclick="goBack()"></i>
                转售记录
            </h4>
            <section class="resalerecord">
                <ul id="ull">
                   
                    
                </ul>
            </section>
        </main>
        <input id="memberID" runat="server" type="hidden"/>
    </form>
</body>
</html>
<script>
    $(function () {
        sear();
    })
    function sear() {
        $("#ull").empty();
        $.ajax({
            type: "post",
            url: "Resalerecord.aspx/getOrderList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{memberID:'" + $("#memberID").val() + "'}",
            success: function (data) {
                var serverdata = $.parseJSON(data.d);
                var serverdatalist = serverdata.length;
                for (var i = 0; i < serverdatalist; i++) {
                    var zt=""
                    if (serverdata[i].zt=="3")
                    {
                        zt = "已转售";
                    } else if (serverdata[i].zt == "6")
                    {
                        zt = "已售出";
                    }
                    $(" <li>"
                       + " <h4>转售单号：" + serverdata[i].orderNO + "<em>" + zt + "</em></h4>"
                       +" <dl>"
                       +"     <dd>"
                       +"         <label>产品名称：</label>"
                       + "         <span>" + serverdata[i].proName + "</span>"
                       +"     </dd>"
                       +"     <dd>"
                       +"         <label>出售日期：</label>"
                       + "         <span>" + serverdata[i].sellTime + "</span>"
                       +"     </dd>"
                       +"     <dd>"
                       +"         <label>出售价格：</label>"
                       + "         <span><a>¥" + serverdata[i].sellPrice + "</a></span>"
                       +"     </dd>"
                       +"</dl>"
                       +"</li>").appendTo("#ull");
                }
            }
        })
    }
</script>