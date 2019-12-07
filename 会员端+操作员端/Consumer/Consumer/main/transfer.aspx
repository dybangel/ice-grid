<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="transfer.aspx.cs" Inherits="Consumer.main.transfer" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>会员转账-自助售卖系统</title>
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
                会员转账
            </h4>
            <section class="membertarn">
                <ul>
                   <%-- <li>
                        <label>收款会员</label>
                        <input type="text" value="" placeholder="收款人姓名" />
                    </li>--%>
                    <li>
                        <label>收款账号</label>
                        <input type="text" value="" placeholder="收款人手机号" id="b_h"/>
                    </li>
                </ul>
                <ul>
                    <li>
                        <label>转账金额</label>
                        <input type="number" value=""  id="mo_ney"/>
                    </li>
                </ul>
                <input type="button" value="确定转账" class="qdBtn"  onclick="ok()"/>
                <p class="moneyYue">账户余额:<span><%=availableMpney %></span>元</p>
            </section>
        </main>
        <input  id="memberID" runat="server" type="hidden"/>
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
    function ok()
    {
        $.ajax({
            type: "post",
            url: "transfer.aspx/transfer1",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{bh:'" + $("#b_h").val() + "',money:'" + $("#mo_ney").val() + "',member_ID:'" + $("#memberID").val() + "'}",
            success: function (data) {
                if (data.d == "1") {
                    alert("该编号不存在");
                } else if (data.d == "2") {
                    alert("当前余额不足");
                } else if (data.d == "3") {
                    alert("转账成功");
                } else if (data.d == "4") {
                    alert("当前没有绑定手机号，请前往会员中心绑定手机号");
                }
            }
        })
        location.reload();
    }
</script>
