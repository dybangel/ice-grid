<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="purchase.aspx.cs" Inherits="Consumer.main.purchase" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>会员充值-自助售卖系统</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" />
    <link rel="shortcut icon" href="/main/public/images/favicon.ico" />
    <link href="/main/public/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/common.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/style.css" rel="stylesheet" type="text/css" />
    <script src="/main/public/script/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="/main/public/script/JavaScript.js" type="text/javascript"></script>
    <script src="https://res.wx.qq.com/open/js/jweixin-1.3.2.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <main class="setupq">
            <h4 class="commontitle">
                <i class="fa fa-angle-left" onclick="goBack()"></i>
                账户充值
            </h4>
             <div class="isMyMoney">
                 <span>余额</span>
                 <h2><%=yue %>元</h2>
             </div>
            <div class="currency_a" id="div1">
               
               <%-- <a id="otherMoney" onclick="otherMoney()">其他金额</a>--%>
            </div>
            <div class="currency_je">
                <span style="float: left;">金额（¥）</span>
                <input name="money" type="text" id="money" placeholder="请输入大于0的整数" />
            </div>
            <div class="currency_fs">
                <%--<h4>充值方式</h4>--%>
                <input type="hidden" value="" id="liId" />
                <input type="button" value="确定" onclick="chongBtn()" class="commonbtn" />
            </div>
        </main>
        <input id="memberID" runat="server" type="hidden" />
        <input id="flag" runat="server" type="hidden" />
        <input type="hidden" id="jine" runat="server" />
        <input type="hidden" id="orderNO" runat="server" />
        <input type="hidden" id="_openID" runat="server"/>
        <input type="hidden" id="_companyID" runat="server"/>
        <input type="hidden" id="_wxJsApiParam" runat="server"/>
        <input type="hidden" id="_dzMoney" runat="server" />
        <input type="hidden" id="_money" runat="server" />
    </form>
</body>
</html>
<script>
    function IsWeixinOrAlipay() {
        //判断是不是微信
        if (strpos($_SERVER['HTTP_USER_AGENT'], 'MicroMessenger') !== false) {
            return "WeiXIN";
        }
        //判断是不是支付宝
        if (strpos($_SERVER['HTTP_USER_AGENT'], 'AlipayClient') !== false) {
            return "Alipay:true";
        }
        //哪个都不是
        return "false";
    }

    if (/MicroMessenger/.test(window.navigator.userAgent)) {
        //微信浏览器
        $("#flag").val(1);
    } else {

        //其他浏览器
        $("#flag").val(0);
    }
    function isNumber(value) {         //验证是否为数字
        var patrn = /^(-)?\d+(\.\d+)?$/;
        if (patrn.exec(value) == null || value == "") {
            return false
        } else
        {
            return true
        }
    }
    function chongBtn() {
        
        if (isNumber($("#money").val())) {
            if(parseFloat($("#money").val())<=0)
            {
                alert("充值金额必须是大于0的整数");
                return;
            }
            $("#_money").val($("#money").val());
            $("#_dzMoney").val($("#money").val());
            location.href = "ylpay.aspx?companyID=" + $("#_companyID").val() + "&openID=" + $("#_openID").val() + "&money=" + $("#_money").val() + "&dzMoney=" + $("#_dzMoney").val();
            //wx.miniProgram.navigateTo({ url: 'pages/pay/pay' })
        } else {
            alert("请输入正确的金额");
            return;
        }
    };
    function offpopup() {
        $(".bankxz").removeClass("bankxzTop");
        $(".popup").fadeOut();
    }
    $(function () {
        jQuery("#money").keyup(function () {
            var value = jQuery(this).val();
            if ((/^(\+|-)?\d+$/.test(value)) && value > 0) {
                return true;
            } else if (value != "") {
                alert("充值金额中请输入正整数！");
                jQuery("#money").val("0");
                return false;
            }
        });
    });

    function otherMoney() {
        $('.currency_je,.currency_fs').show();
        $('#money').focus();
    }
    function getPayList()
    {
        $("#div1").empty();
        $.ajax({
            type: "post",
            url: "purchase.aspx/getPayList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{companyID:'" + $("#_companyID").val() + "'}",
            success: function (data) {
                var serverdata = $.parseJSON(data.d);
                var serverdatalist = serverdata.length;
                for (var i = 0; i < serverdatalist; i++) {
                    $("<a onclick='getMoney(this)'>"
                     +"<b></b>"
                     +"<span>" + serverdata[i].dzMoney + "</span>"
                     +"<p>售价：<em class='ema'>" + serverdata[i].czMoney + "</em>元</p>"
                     +"</a>").appendTo("#div1")
                }
                $("<a id='otherMoney' onclick='otherMoney()'>其他金额</a>").appendTo("#div1");
            }
        })
    }
    function getMoney(obj)
    {
        var dzMoney = $(obj).find("span").html();
        var czMOney = $(obj).find(".ema").html();
        $("#_money").val(czMOney);
        $("#_dzMoney").val(dzMoney);
        location.href = "ylpay.aspx?companyID=" + $("#_companyID").val() + "&openID=" + $("#_openID").val() + "&money=" + czMOney + "&dzMoney=" + dzMoney;
        //wx.miniProgram.navigateTo({ url: 'pages/pay/pay' })
    }
    $(function ()
    {
        getPayList();
    })
</script>
