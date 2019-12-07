<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="pwdsetup.aspx.cs" Inherits="Consumer.main.pwdsetup" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>密码设置-自助售卖系统</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />
    <link rel="shortcut icon" href="/main/public/images/favicon.ico" />
    <link href="/main/public/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/common.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/style.css" rel="stylesheet" type="text/css" />
    <script src="/main/public/script/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="/main/public/script/JavaScript.js" type="text/javascript"></script>
     <style>
        #_isUpPwdBtn{
            width:calc(100% - 32px);
            margin: 30px 16px;
            height:40px;
            background:#6949ff;
            color:#fff;
            font-size:1.26rem;
            border:0;
            border-radius:150px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
       <main class="setupq">
            <h4 class="commontitle">
                <i class="fa fa-angle-left" onclick="goBack()"></i>
                密码设置
      <%--  <input type="button" value="保存" class="inputok" onclick="inputok()" />--%>
            </h4>
            <div class="inputli">
                <input type="text" value=""  id="phone" runat="server" placeholder="请填写手机号"/>
                <i class="fa fa-times-circle" onclick="deleteinp()"></i>
            </div>
             <div class="inputli" style="width:calc(100% - 130px)">
                <input type="text" value=""  id="_yzm" runat="server" placeholder="验证码"/>
                <i class="fa fa-times-circle" onclick="deleteinp()"></i>
            </div>
             <span id="getyzm" style="display:block;width:120px;float:left">
                <input type="button" onclick="sendMess()" style=" width: 100%;
			        height: 48px;
			        border-radius: 6px;
			        margin-top: 18px;
                    margin-left:10px;
                    border:0;
			        background: #6949ff;
			        color: #fff;
			        font-size: 1.1rem;float:left" value="发送短信验证码" />
            </span>
            <div class="inputli">
                <input type="text" value=""  id="newPwd" runat="server" placeholder="新密码"/>
                <i class="fa fa-times-circle" onclick="deleteinp()"></i>
            </div>
			<input type="button" onclick="inputok()" value="确定" id="_isUpPwdBtn" />
        </main>
        <input  id="member_ID" runat="server" type="hidden"/>
        <input  id="companyID" runat="server" type="hidden"/>
    </form>
</body>
</html>
<script>
    function sendMess() {
        var Num = "";
        for (var i = 0; i < 6; i++) {
            Num += Math.floor(Math.random() * 10);
        }
        document.cookie = "yzm=" + Num;
        $.ajax({
            type: "post",
            url: "pwdsetup.aspx/sendMess",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{phone:'" + $("#phone").val() + "',memberID:'" + $("#member_ID").val() + "',yzm:'" + Num + "'}",
            success: function (data) {
                if (data.d == "1") {
                    alert("手机号不能为空");
                } else if (data.d == "2") {
                    alert("手机号不正确");
                } else if (data.d == "3") {
                    alert("服务器异常");
                } else if (data.d == "4") {
                    alert("发送成功");
                }
            }
        })
    }
    $(function () {
        $(".pwdsetup").find("li").find("input").focus(function (){
            $(this).parent("li").find("i").addClass("ion");
        });
        $(".pwdsetup").find("li").find("input").blur(function (){
            $(this).parent("li").find("i").removeClass("ion");
        });
    });
    function get_cookie(Name) {
        var search = Name + "="//查询检索的值
        var returnvalue = "";//返回值
        if (document.cookie.length > 0) {
            sd = document.cookie.indexOf(search);
            if (sd != -1) {
                sd += search.length;
                end = document.cookie.indexOf(";", sd);
                if (end == -1)
                    end = document.cookie.length;
                //unescape() 函数可对通过 escape() 编码的字符串进行解码。
                returnvalue = unescape(document.cookie.substring(sd, end))
            }
        }
        return returnvalue;
    }
    function deleteinp(obj) {
        var $ion = $(obj);
        $ion.parent("li").find("input").val("");
        $ion.parent("li").find("input").focus();
    };

    function inputok() {
        if ($("#_yzm").val() == "")
        {
            alert("验证码不能为空");
            return;
        }
        //判断验证码是否一致
        if ($("#_yzm").val() != get_cookie("yzm")) {
            alert("验证码不正确");
            return false;
        }
        $.ajax({
            type: "post",
            url: "pwdsetup.aspx/save",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{memberID:'" + $("#member_ID").val() + "',yzm:'" + $("#_yzm").val() + "',newpwd:'" + $("#newPwd").val() + "',companyID:'" + $("#companyID").val() + "'}",
            success: function (data) {
               
                if (data.d == "1") {
                    alert("修改成功");
                    location.href = "membersetup.aspx";
                } else if (data.d == "2") {
                    alert("原密码不正确");
                }  else if (data.d == "4")
                {
                    alert("保存失败");
                }
            }
        })
        
    }
</script>
