<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="orderlist.aspx.cs" Inherits="Consumer.main.orderlist" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>我的订单-自助售卖系统</title>
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
            <section class="orderbtn">
                <h4><a href="orderall.aspx#all">我的订单<span>全部订单<i class="fa fa-angle-right"></i></span></a></h4>
                <ul>
                     <li>
                        <a href="orderall.aspx#dzf">
                            <img src="public/images/Icon/chong.png" alt="" />
                            <span>待支付</span>
                        </a>
                    </li>
                    <li>
                        <a href="orderall.aspx#payment">
                            <img src="public/images/Icon/dfk.png" alt="" />
                            <span>完成</span>
                        </a>
                    </li>
                    <li>
                        <a href="orderall.aspx#pickup">
                            <img src="public/images/Icon/qhz.png" alt="" />
                            <span>取货中</span>
                        </a>
                    </li>
                    <li>
                        <a href="orderall.aspx#resale">
                            <img src="public/images/Icon/yzs.png" alt="" />
                            <span>已转售</span>
                        </a>
                    </li>
                    <li>
                        <a href="orderall.aspx#invalid">
                            <img src="public/images/Icon/ysx.png" alt="" />
                            <span>已失效</span>
                        </a>
                    </li>
                   
                </ul>
            </section>
            <section class="orderlist">
                <h4>最近下单</h4>
                <ul id="ull">
                     
                </ul>
                <a class="ordermore">更多<i class="fa fa-angle-down"></i></a>
            </section>
        </main>
        <section class="homeNav"></section>
        <input  id="memberID" runat="server" type="hidden"/>
        <input  id="flag" runat="server" type="hidden"/>
        <input  id="companyID" runat="server" type="hidden"/>
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
    })
    function sear()
    {
        $("#ull").empty();
        $.ajax({
            type: "post",
            url: "orderlist.aspx/getOrderList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{memberID:'" + $("#memberID").val() + "'}",
            success: function (data) {
                var serverdata = $.parseJSON(data.d);
                var serverdatalist = serverdata.length;
                for (var i = 0; i < serverdatalist; i++) {
                    var zt = "";
                    if(serverdata[i].zt=="0")
                    {
                        zt = "生产中";
                    } else if (serverdata[i].zt=="1")
                    {
                        zt = "配送中";
                    } else if (serverdata[i].zt == "3") {
                        zt = "配送完成";
                    } else if (serverdata[i].zt == "4") {
                        zt = "已兑换";
                    }
                    $(" <li>"
                        + "<a href='orderdetails.aspx?orderNO="+serverdata[i].orderNO+"'>"
                        +"    <div class='orderleft'>"
                        +"        <span>"
                        + "            <img src='" + serverdata[i].httpImageUrl + "' alt='' />"
                        +"        </span>"
                        + "        <h2>" + serverdata[i].proName + "</h2>"
                        + "        <p>" + serverdata[i].createTime + "</p>"
                        + "        <p>订购周期：" + serverdata[i].zq + "天</p>"
                        + "        <p>剩余天数：" + serverdata[i].syNum + "天</p>"
                        +"    </div>"
                        +"    <div class='orderright'>"
                        + "        <label>" +zt  + "</label>"
                        + "        <span>¥" + serverdata[i].totalMoney + "</span>"
                        +"    </div>"
                        +" </a>"
                    +"</li>").appendTo("#ull")
                }
            }
        })
    }
</script>
