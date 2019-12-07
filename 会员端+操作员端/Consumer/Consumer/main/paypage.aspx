<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="paypage.aspx.cs" Inherits="Consumer.main.paypage" %>

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
    <style>
        .login-button { /* 按钮美化 */
	        height: 40px; /* 高度 */
	        border-width: 0px; /* 边框宽度 */
	        border-radius: 3px; /* 边框半径 */
	        background: #F8F8FF; /* 背景颜色 */
	        cursor: pointer; /* 鼠标移入按钮范围时出现手势 */
	        outline: none; /* 不显示轮廓线 */
	        font-family: Microsoft YaHei; /* 设置字体 */
	        color: black; /* 字体颜色 */
	        font-size: 20px; /* 字体大小 */
            font-weight:bold;
        }
        .login-button:hover { 
            /* 鼠标移入按钮范围时改变颜色 */
	        background: #5599FF;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="paypageMain change">
            <h4>
                <i onclick="close()">×</i>
                <img src="<%=headURL %>"" alt="" />
                <span>请输入支付密码</span>
            </h4>
            <dl>
                <dt><%=companyName %></dt>
                <dd>¥  <%=money %></dd>
            </dl>
            <p>
                余额支付
                <em>当前余额：¥ <%=ye %></em>
            </p>
            <input type="password" value="" placeholder="支付密码..." id="isInput" readonly="true"/>
          <%--  <input type="button" value="确定" id="isQDBtn" onclick="okPay()"/>--%>
            <table style="width:100%;margin-bottom:20px;">
                <tr>
                    <td style="width:33%"><input class="login-button" type="button" value="1" onclick="btn(1)"/></td>
                    <td style="width:33%"><input class="login-button" type="button" value="2" onclick="btn(2)"/></td>
                    <td style="width:33%"><input class="login-button" type="button" value="3" onclick="btn(3)"/></td>
                </tr>
                <tr>
                    <td style="width:33%"><input class="login-button" type="button" value="4" onclick="btn(4)"/></td>
                    <td style="width:33%"><input class="login-button" type="button" value="5" onclick="btn(5)"/></td>
                    <td style="width:33%"><input class="login-button" type="button" value="6" onclick="btn(6)"/></td>
                </tr>
                <tr>
                    <td style="width:33%"><input class="login-button" type="button" value="7" onclick="btn(7)"/></td>
                    <td style="width:33%"><input class="login-button" type="button" value="8" onclick="btn(8)"/></td>
                    <td style="width:33%"><input class="login-button" type="button" value="9" onclick="btn(9)"/></td>
                </tr>
                 <tr>
                    <td style="width:33%"><input class="login-button" type="button" value="0" onclick="btn(0)"/></td>
                    <td style="width:33%"><input class="login-button" type="button" value="删除" onclick="btn(11)"/></td>
                    <td style="width:33%"><input class="login-button" type="button" value="确定" onclick="btn(12)"/></td>
                </tr>
            </table>
        </div>
        <div class="paypageBg"></div>
        <input id="_money" runat="server" type="hidden"/>
        <input id="_trxid" runat="server" type="hidden"/>
        <input id="_companyID" runat="server" type="hidden"/>
        <input id="_openID" runat="server" type="hidden"/>
    </form>
</body>
</html>
<script>
    $(function () {
        InputShow()
    })
    function btn(val)
    {
        if(val<10)
        {
            $("#isInput").val($("#isInput").val()+val);
        }
        if(val==11)
        {
            $("#isInput").val($("#isInput").val().substring(0, $("#isInput").val().length-1));
        }
        if(val==12)
        {
            okPay();
        }
    }
    function InputShow() {
        $('.paypageMain').addClass('paypageMainTop');
        $('#isInput').focus();
    }
    function close()
    {
        window.close();
    }
    function okPay()
    {
        $.ajax({
            type: "post",
            url: "paypage.aspx/yzPwd",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{openID:'" + $("#_openID").val() + "',pwd:'" + $("#isInput").val() + "',money:'" + $("#_money").val() + "',trxid:'" + $("#_trxid").val() + "',companyID:'" + $("#_companyID").val() + "'}",
            success: function (data) {
                if(data.d=="1")
                {
                    alert("支付密码不正确");
                } else if (data.d == "2") {
                    alert("余额不足");
                } else if (data.d == "3") {
                    alert("支付完成");
                    location.href = "homeIndex.aspx?companyID=" + $("#_companyID").val();
                } else if (data.d == "4") {
                    alert("该笔订单已经支付完成");

                }
            }
        })
    }
</script>