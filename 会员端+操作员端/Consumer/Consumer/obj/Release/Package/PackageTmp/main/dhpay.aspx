<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dhpay.aspx.cs" Inherits="Consumer.main.dhpay" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>首页-自助售卖系统</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" />
    <link rel="shortcut icon" href="/main/public/images/favicon.ico" />
    <link href="/main/public/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/common.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/style.css" rel="stylesheet" type="text/css" />
    <script src="/main/public/script/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="/main/public/script/JavaScript.js" type="text/javascript"></script>
    <script>var _GetTokenError = ''; var wxinshare_title = ''; var wxinshare_desc = ''; var wxinshare_link = ''; var wxinshare_imgurl = '';; var fxShopName = ''</script>
    <script src="http://res.wx.qq.com/open/js/jweixin-1.0.0.js"></script>
    <script charset="utf-8" src="http://map.qq.com/api/js?v=2.exp&libraries=geometry"></script>
    <!-- 轮播样式js -->
    <link href="/main/public/css/swiper.min.css" rel="stylesheet" />
    <script src="/main/public/script/swiper.min.js" type="text/javascript"></script>
    <script charset="utf-8" src="http://map.qq.com/api/js?v=2.exp" type="text/javascript"></script>
	<script src="http://open.map.qq.com/apifiles/2/4/79/main.js" type="text/javascript"></script>
	<style>
            #address{height: 31px;padding: 0 10px;}
            .map-seach{background: #50a4ec;padding: 5px 20px;color: #fff;display: inline-block;}
            .map-seach:active{background: rgba(80, 164, 236, 0.4);}
    </style>
</head>
<body>
    <form id="form1" runat="server">
   
         <input id="_companyID" runat="server" type="hidden"/>
         <input id="_openID" runat="server" type="hidden"/>
        <input id="_orderNO" runat="server" type="hidden"/>
        <input id="_syMoney" runat="server" type="hidden"/>
        <input id="_need_money" runat="server" type="hidden"/>
        <input id="_zq" runat="server" type="hidden"/>
        <input id="_productID" runat="server" type="hidden"/>
        <input id="_yhfs" runat="server" type="hidden"/>
        <input id="_mechineID" runat="server" type="hidden"/>
         <input id="_trxid" runat="server" type="hidden"/>
        
    </form>
</body>
</html>
<script>
    //调用微信JS api 支付
    function callpay() {
        if (typeof WeixinJSBridge == "undefined") {
            if (document.addEventListener) {
                document.addEventListener('WeixinJSBridgeReady', jsApiCall, false);
            }
            else if (document.attachEvent) {
                document.attachEvent('WeixinJSBridgeReady', jsApiCall);
                document.attachEvent('onWeixinJSBridgeReady', jsApiCall);
            }
        }
        else {
            jsApiCall();
        }
    }
       function jsApiCall() {
          
            WeixinJSBridge.invoke('getBrandWCPayRequest', <%=wxJsApiParam%>,//josn串
              function (res) {
                  
                  WeixinJSBridge.log(res.err_msg);
                  if (res.err_msg == "get_brand_wcpay_request:ok") {
                      //
                      dh();
                  }// 使用以上方式判断前端返回,微信团队郑重提示：res.err_msg将在用户支付成功后返回    ok，但并不保证它绝对可靠。 
              });
        }
    $(function () {
         
        if(parseFloat($("#_syMoney").val())>parseFloat($("#_need_money").val()))
        {
            if(confirm("确定需要兑换吗？如有剩余钱会打到会员钱包"))
            {
                dh();
            }
        }else{
            callpay();
        }
           
        })
      
        function dh()
        {
           
            $.ajax({
                type: "post",
                url: "dhpay.aspx/dh",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: "{orderNO:'" + $("#_orderNO").val() + "',syMoney:'" + $("#_syMoney").val() + "',need_money:'" + $("#_need_money").val() + "',zq:'" + $("#_zq").val() + "',productID:'" + $("#_productID").val() + "',yhfs:'"+$("#_yhfs").val()+"'}",
                success: function (data) {      
                    if(data.d=="1")
                    {
                        alert("兑换成功");
                        window.location.href="orderlist.aspx";
                    }else if(data.d=="2")
                    {
                        alert("当前状态无法兑换");
                    }
                    OffPopup();
                }
            })
        }
</script>
