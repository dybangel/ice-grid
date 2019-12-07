<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="autosell_center.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>欢迎登陆-自动售卖终端中心控制系统</title>
    <link rel="shortcut icon" href="/main/public/images/favicon.ico" />
    <link href="/main/public/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/common.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/style.css" rel="stylesheet" type="text/css" />
    <script src="/main/public/script/jquery-3.2.1.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="login">
            <div class="login_main">
                <div class="login_title">
                    <img src="/main/public/images/logo.png" />
                    <span>自动售卖终端中心控制系统</span>
                </div>
                <div class="login_con">
                    <span class="login_user">
                        <i class="fa fa-user"></i>
                        <input type="text" name="auser" value="" placeholder="用户名" />
                    </span>
                    <span class="login_pwd">
                        <i class="fa fa-lock"></i>
                        <input type="password" name="apwd" value="" placeholder="密码" />
                    </span>
                    <span class="login_user">
                         <select id="qxType">
                       <option value="1">管理员</option>
                       <option value="2">操作员</option>
                   </select>
                    </span>
                 <%--   <img id="imgVerifyCode" class="verify_img" alt="点击更换" title="点击更换" src="http://lpp.vip.suqiangkeji.com/VerifyCodeImage.aspx"/>--%>
                    <input type="button" value="登 录" name="submit" class="alogin_nav"  onclick="login()"/>
                    <div class="WeChat"></div>
                    <div class="Mobile"></div>
                    <div class="shop"></div>
                    <div class="group"></div>
                </div>
                <div class="login_foot">
                    <span>青岛冰格科技公司版权所有 翻版必究</span>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
<script>
    function login()
    {
        var user = $(".login_user").find("input").val();
        var pwd = $(".login_pwd").find("input").val();
        if (user == "" || pwd == "") {
            alert("账号或密码不能为空!");
        }


        $.ajax({
            url: "../../ashx/asm.ashx",
            type: 'post',
            dataType: 'json',
            timeout: 10000,
            data: {
                action: "login",
                name: user,
                pwd: pwd,
                qx: $("#qxType").val()
            },
            success: function (resultData) {

                if (resultData.result == "1") {
                    window.location.href = "/main/enterprise/SellCenter.aspx";
                } else if (resultData.result == "2") {
                    alert("账号或密码错误");
                }
            }

        })
       

    }

</script>
