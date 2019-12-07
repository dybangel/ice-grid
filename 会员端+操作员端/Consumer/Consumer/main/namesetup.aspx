<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="namesetup.aspx.cs" Inherits="Consumer.main.namesetup" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>昵称设置-自助售卖系统</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />
    <link rel="shortcut icon" href="/main/public/images/favicon.ico" />
    <link href="/main/public/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/common.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/style.css" rel="stylesheet" type="text/css" />
    <script src="/main/public/script/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="/main/public/script/JavaScript.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <main class="setupq">
            <h4 class="commontitle">
                <i class="fa fa-angle-left" onclick="goBack()"></i>
                姓名设置
            <input type="button" value="保存" class="inputok" onclick="inputok()" />
            </h4>
            <div class="inputli">
                <input type="text" value="" id="nickname" runat="server"/>
                <i class="fa fa-times-circle" onclick="deleteinp()"></i>
            </div>
        </main>
          <input id="member_ID" runat="server" type="hidden"/>
    </form>
</body>
</html>
<script>
    $(function () {
        $(".inputli").find("input").focus(function () {
            $(".inputli").find("i").addClass("iop");
        });
        $(".inputli").find("input").blur(function () {
            $(".inputli").find("i").removeClass("iop");
        });
    });

    function deleteinp() {
        $(".inputli").find("input").val("");
        $(".inputli").find("input").focus();
    };

    function inputok() {
        $.ajax({
            type: "post",
            url: "namesetup.aspx/setupOk",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{nickname:'" + $("#nickname").val() + "',memberID:'" + $("#member_ID").val() + "'}",
            success: function (data) {
                if (data.d == "1") {
                    alert("修改成功");
                    location.href = "membersetup.aspx";
                } else if (data.d == "2") {
                    alert("修改失败");
                }  
            }
        })
 
    }
</script>
