<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="accethb.aspx.cs" Inherits="Consumer.main.accethb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
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
        <div class="redEnvelopesOk">
            <span>恭喜您</span>
            <h4>新人红包领取成功</h4>
            <input type="button" value="我知道了" id="_isYes" onclick="isYes()" />
        </div>
        <section class="redEnvelopes redEnvelopesHeight change">
            <div class="envelopesBtn" onclick="envelopesBtn()"></div>
            <div class="envelopesTxt">
                <h1>恭喜您!</h1>
                <h4>获得新人红包</h4>
                <span id="hby">
                    0
                    <em>元</em>
                </span>
                <dl>
                    <dt>使用说明</dt>
                    <dd>领取红包后将立即到您的个人账户中</dd>
                    <dd>当您首次充值后会激活此红包</dd>
                </dl>
            </div>
            <b onclick="offEnvelopes()">X</b>
        </section>
        <input id="_p11" runat="server" type="hidden"/>
        <input id="_p12" runat="server" type="hidden"/>
         <input id="_companyID" runat="server" type="hidden"/>
        <input id="_openID" runat="server" type="hidden"/>
    </form>
</body>
</html>
<script>
    $(function(){
        $("#hby").text($("#_p11").val());
    })
    function isYes() {

        $.ajax({
            type: "post",
            url: "accethb.aspx/accetHB",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{companyID:'" + $("#_companyID").val() + "',openID:'" + $("#_openID").val() + "'}",
            success: function (data) {
                if(data.d=="1")
                {
                    $('.popupBg').fadeOut()
                    $('.redEnvelopes').removeClass('redEnvelopesHeight')
                    setTimeout(function () {
                        $('.redEnvelopesOk').hide()
                        $('.redEnvelopes').css({ 'z-index': '11' })
                    }, 200);
                    window.location.href = "homeIndex.aspx";
                }else if(data.d=="2")
                {
                    alert("当前会员信息读取失败");
                } else if (data.d == "3") {
                    alert("当前设置红包金额错误");
                } else if (data.d == "4") {
                    alert("未知错误");
                }

            }
        })
       

    }
    function envelopesBtn() {
        //$('.redEnvelopes').removeClass('redEnvelopesHeight')
        $('.redEnvelopes').css({ 'z-index': '1' })
        $('.redEnvelopesOk').show()
    }
    function offEnvelopes() {
        $('.popupBg').fadeOut()
        $('.redEnvelopes').removeClass('redEnvelopesHeight')
    }
</script>