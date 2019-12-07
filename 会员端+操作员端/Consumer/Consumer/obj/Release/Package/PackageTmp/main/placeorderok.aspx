<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="placeorderok.aspx.cs" Inherits="Consumer.main.placeorderok" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>下单成功-自助售卖系统</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" />
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
        <section class="order_ok">
            <div class="orderok_top">
                <img src="/main/public/images/Icon/order_ok.png" alt=""/>
                <span>下单成功</span>
            </div>
            <dl class="orderoktext">
                <dt>订单编号：<%=orderNO %></dt>
                <dd>商品名称：牛奶奶</dd>
                <dd>订购天数：<%=pszq %>天</dd>
                <dd>起订日期：<%=qsDate %></dd>
                <dd>止订日期：<%=zdDate %></dd>
                <dd>配送方式：<%=psfs=="1"?"按天派送":"自定义时间" %></dd>
                <dd>配送周期：<%=psStr %></dd>
                 <dd>优惠方式：<%=yhfs %></dd>
                <dd>您已成功下单，请及时完成付款</dd>
              <%--  <dd>如有疑问请致电400-0000-000</dd>--%>
            </dl>
            <div class="orderokBtn">
                <a href="orderdetails.aspx?pszq=<%=pszq %>&qsDate=<%=qsDate %>&zdDate=<%=zdDate %>&psfs=<%=psfs %>&psStr=<%=psStr %>&orderNO=<%=orderNO %>&selDate=<%=selDate %>&productID=<%=productID %>&mechineID=<%=mechineID %>&createTime=<%=createTime %>&yhfs=<%=yhfs %>" class="orderpay">前往支付</a>
                <a href="productlist.aspx?mechineID=<%=mechineID %>">继续下单</a>
                <a href="orderall.aspx#dzf">待支付订单</a>
            </div>
        </section>
    </main>
    </form>
</body>
</html>
