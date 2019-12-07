<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wxpayNew.aspx.cs" Inherits="autosell_center.wxpayNew" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script src="public/script/jquery-3.2.1.min.js"></script>
   
</head>
<body>
    <form id="form1" runat="server">
     <input id="_companyID" runat="server" type="hidden"/>
     <input id="_openID" runat="server" type="hidden"/>
     <input id="_money" runat="server" type="hidden"/>
     <input id="_dzMOney" runat="server" type="hidden"/>
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
                  //alert(JSON.stringify(res));
                  WeixinJSBridge.log(res.err_msg);
                  if (res.err_msg == "get_brand_wcpay_request:ok") {
                      $.ajax({
                          type: "post",
                          url: "ylpay.aspx/addInsert",
                          contentType: "application/json; charset=utf-8",
                          dataType: "json",
                          data: "{money:'" + $("#_money").val() + "',openID:'"+$("#_openID").val()+"',dzMoney:'"+$("#_dzMOney").val()+"',companyID:'"+$("#_companyID").val()+"'}",
                          success: function (data) {
                              
                          }
                      })
                      
                  }// 使用以上方式判断前端返回,微信团队郑重提示：res.err_msg将在用户支付成功后返回    ok，但并不保证它绝对可靠。 
              });
        }
        $(function () {
            callpay();
        })

</script>
