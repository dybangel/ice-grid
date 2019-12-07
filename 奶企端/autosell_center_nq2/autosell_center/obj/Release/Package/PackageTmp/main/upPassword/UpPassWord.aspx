<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UpPassWord.aspx.cs" Inherits="autosell_center.main.upPassword.UpPassWord" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>首页-自动售卖终端中心控制系统</title>
    <link rel="shortcut icon" href="/main/public/images/favicon.ico" />
    <link href="/main/public/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/common.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/style.css" rel="stylesheet" type="text/css" />
    <script src="/main/public/script/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="/main/public/script/Titlelist.js" type="text/javascript"></script>
    <style>
        #btn {
        width: 120px;
        margin: 0 30px;
        height: 36px;
        background: #3a77d5;
        color: #fff;
        border: 0;
        border-radius: 6px;
        cursor: pointer;
        }
    </style>
</head>
<body>
    <form method="post" action="./SellCenter.aspx" id="form1">
        <div class="header"></div>
        <div class="main">
            <div class="main_list">
                <div class="common_title">
                    <h4>
                        <i class="fa fa-home"></i>
                        <span>修改登录密码</span>
                    </h4>
                </div>
                <div class="common_main">
                    <div class="navlist">
                        <dl>
                            <dt>密码修改<em class="fa fa-cog"></em></dt>
                            <dd>
                                <a class="change acolor"><i class="change icolor fa fa-check-square"></i>修改密码</a>
                            </dd>
                        </dl>
                    </div>
                    <section class="jiqlist">
                        <ul class="upPwd">
                            <li>
                                <label>原密码</label>
                                <input type="password" value="" placeholder="请输入原密码"  id="oldpwd"/>
                            </li>
                            <li>
                                <label>新密码</label>
                                <input type="password" value="" placeholder="请输入新密码" id="newpwd"/>
                            </li>
                            <li>
                                <label>确认新密码</label>
                                <input type="password" value="" placeholder="请确认新密码" id="againpwd"/>
                            </li>
                             <li>
                                  <input type="button" value="保存" id="upPwdBtn" onclick="update()"/>
                            </li>
                        </ul>
                      

                         <ul class="upPwd" id="tkpwd">
                            <li>
                                <label>原退款密码</label>
                                <input type="password" value="" placeholder="请输入原密码"  id="oldpwd1"/>
                            </li>
                            <li>
                                <label>新退款密码</label>
                                <input type="password" value="" placeholder="请输入新密码" id="newpwd1"/>
                            </li>
                            <li>
                                <label>确认新密码</label>
                                <input type="password" value="" placeholder="请确认新密码" id="againpwd1"/>
                            </li>
                              <li>
                                  <input type="button" value="保存" id="btn" onclick="update1()" />
                            </li>
                        </ul>
                      
                    </section>
                </div>
            </div>
        </div>
          <input id="operaID" runat="server" type="hidden"/>
        <input id="OPID" runat="server" type="hidden"/>
    </form>
</body>
</html>
<script>
    $(function () {
        if ($("#OPID").val() == "0") {
            $("#tkpwd").show();
        } else {
            $("#tkpwd").hide();
        }
    })
    function update()
    {
        if ($("#oldpwd").val()=="")
        {
            alert("请输入原密码");
            return;
        }
        if ($("#newpwd").val() == "")
        {
            alert("请输入新密码");
            return;
        }
        if ($("#againpwd").val() == "")
        {
            alert("请输入确认新密码");
            return;
        }
        if ($("#newpwd").val() != $("#againpwd").val())
        {
            alert("两次输入的密码不一致");
            return;
        }
        $.ajax({
            type: "post",
            url: "UpPassWord.aspx/update",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{id:'" + $("#operaID").val() + "',oldpwd:'" + $("#oldpwd").val() + "',newpwd:'" + $("#newpwd").val() + "',OPID:'" + $("#OPID").val() + "'}",
            success: function (data) {
                
                alert(data.d);
            }
        })
    }
    function update1() {
        
        if ($("#newpwd1").val() == "") {
            alert("请输入新密码");
            return;
        }
        if ($("#againpwd1").val() == "") {
            alert("请输入确认新密码");
            return;
        }
        if ($("#newpwd1").val() != $("#againpwd1").val()) {
            alert("两次输入的密码不一致");
            return;
        }
        $.ajax({
            type: "post",
            url: "UpPassWord.aspx/update1",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{id:'" + $("#operaID").val() + "',oldpwd:'" + $("#oldpwd1").val() + "',newpwd:'" + $("#newpwd1").val() + "',OPID:'" + $("#OPID").val() + "'}",
            success: function (data) {

                alert(data.d);
            }
        })
    }
</script>