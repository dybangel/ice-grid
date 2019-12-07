<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wxpaytest.aspx.cs" Inherits="Consumer.main.wxpaytest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <input  type="button" onclick="pay()"/>
    </div>
    </form>
</body>
</html>
 <script>
        function onBridgeReady() {
            // pay_info值为接口中返回的data.pay_info
            WeixinJSBridge.invoke('getBrandWCPayRequest', , function(res){
                if(res.err_msg == 'get_brand_wcpay_request:ok') {
                    // 支付成功
                    alert('支付成功');
                } else if(res.err_msg == 'get_brand_wcpay_request:cancel') {
                    // 取消支付！
                    alert('取消支付');
                } else if(res.err_msg == 'get_brand_wcpay_request:fail'){
                    // 支付失败！
                    alert('支付失败：'+res.err_desc+res.err_msg);
                    // 使用以上方式判断前端返回,微信团队郑重提示：res.err_msg将在用户支付成功后返回 ok，但并不保证它绝对可靠。
                }
            });
        }
        function pay()
        {
            if (typeof WeixinJSBridge == "undefined"){
                if(document.addEventListener ){
                    document.addEventListener('WeixinJSBridgeReady', onBridgeReady, false);
                }else if (document.attachEvent){
                    document.attachEvent('WeixinJSBridgeReady', onBridgeReady);
                    document.attachEvent('onWeixinJSBridgeReady', onBridgeReady);
                }
            }else{
                onBridgeReady();
            }
        }
    </script>
