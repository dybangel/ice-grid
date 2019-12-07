<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="page.aspx.cs" Inherits="autosell_center.page" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link rel="shortcut icon" href="/main/public/images/favicon.ico" />
    <link href="/main/public/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/common.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/style.css" rel="stylesheet" type="text/css" />
    <script src="/main/public/script/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="/main/public/script/Titlelist.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <a href="<%=url%>" style="font-size:50px">支付</a> 
    </div>
        <input  id="type" hidden="hidden"  runat="server"/>
    </form>
</body>
</html>
<script>
    $(function () {
        var str = IsWeixinOrAlipay();
        
        if (str == "wx")
        {
            $("#type").val("wx");
        } else if (str = "zfb") {
            $("#type").val("zfb");
        } else {
            
        }
    })
    function IsWeixinOrAlipay()
    {
        if (/MicroMessenger/.test(window.navigator.userAgent)) {
            //微信浏览器
            return "wx";
        } else if (/aplipay/.test(window.navigator.userAgent))
        {
            return "zfb";
        }
        else {
            
        }
    }
    
</script>