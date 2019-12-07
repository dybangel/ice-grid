<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="membersetup.aspx.cs" Inherits="Consumer.main.membersetup" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>个人设置-自助售卖系统</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />
    <link rel="shortcut icon" href="/main/public/images/favicon.ico" />
    <link href="/main/public/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/common.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/style.css" rel="stylesheet" type="text/css" />
    <script src="/main/public/script/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="/main/public/script/JavaScript.js" type="text/javascript"></script>
    <script type="text/javascript" src="/main/public/script/mobiscroll.custom.min.js"></script>
    <link href="/main/public/css/mobiscroll.custom.min.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <main class="setupq">
            <div class="change selectlist">
                <dl>
                    <dd onclick="setsex(0)">未知</dd>
                    <dd onclick="setsex(1)">男</dd>
                    <dd onclick="setsex(2)">女</dd>
                </dl>
                <a class="cancel" onclick="cancel()">取消</a>
            </div>
            <div class="popup" onclick="cancel()"></div>
            <h4 class="commontitle">
                <i class="fa fa-angle-left" onclick="goBack()"></i>
                个人设置
            <input type="button" value="保存" class="inputok" onclick="setupOk()" />
            </h4>
            <section class="setuplist">
                <ul>
                    <li>
                        <a id="pic1">
                            <label>头像</label>
                            <span>
                                <img id="pic" src="<%=headUrl %>"" alt="" />
                                <em class="fa fa-angle-right"></em>
                              <%--  <input type="file" id="upload" name="file" value="" style="display: none;" />--%>
                            </span>
                        </a>
                    </li>
                  <%--  <li>
                       
                        <a>
                            <label>账号</label>
                            <span>
                                <b>132afsa5</b>
                            </span>
                        </a>
                    </li>--%>
                    <li>
                        <%--  namesetup.aspx --%>
                        <a href="namesetup.aspx">
                            <label>姓名</label>
                            <span>
                                <b><%=name %></b>
                                <em class="fa fa-angle-right"></em>
                            </span>

                        </a>
                    </li>
                    <li>
                        <a href="phonesetup.aspx">
                            <label>手机号</label>
                            <span>
                                <b id="phone"><%=phone %></b>
                                <em class="fa fa-angle-right"></em>
                            </span>
                        </a>
                    </li>
                    <li>
                        <a onclick="gender()">
                            <label>性别</label>
                            <span id="gender">
                                <b id="gen"><%=sex %></b>
                                <em class="fa fa-angle-right"></em>
                            </span>

                        </a>
                    </li>
                     <li>
                        <a onclick="peisong()">
                            <label>出生年月</label>
                            <span>
                                <b><input type='text' id='demo_date' placeholder='请选择出生日期' runat="server" onchange="setbirthday()"/></b>
                                <em class="fa fa-angle-right"></em>
                            </span>

                        </a>
                    </li>
                </ul>
                <ul>
                  <li>
                        <a href="pwdsetup.aspx">
                            <label>支付密码</label>
                            <span>
                                <b>点击设置</b>
                                <em class="fa fa-angle-right"></em>
                            </span>
                        </a>
                    </li>
                </ul>
             <%--   <input type="button" value="退出账号" class="loginout" onclick="javascript: location.href = 'login.aspx'" />--%>
            </section>
        </main>
        <input  id="memberID" runat="server" type="hidden"/>
        <input  id="flag" runat="server" type="hidden"/>
    </form>
</body>
</html>
<script>

     var theme = "ios";
    var mode = "scroller";
    var display = "bottom";
    var lang = "zh";
    $('#demo_date').mobiscroll().date({
        theme: theme,
        mode: mode,
        display: display,
        lang: lang,
        minDate: new Date(1900,1,1),
        maxDate: new Date(2050, 12, 31),
        stepMinute: 1
    });
    $(function () {
        $("#pic").click(function () {
            $("#upload").click(); //隐藏了input:file样式后，点击头像就可以本地上传
            $("#upload").on("change", function () {
                var objUrl = getObjectURL(this.files[0]); //获取图片的路径，该路径不是图片在本地的路径
                if (objUrl) {
                    $("#pic").attr("src", objUrl); //将图片路径存入src中，显示出图片
                }
            });
        });
    });

    //建立一個可存取到該file的url
    function getObjectURL(file) {
        var url = null;
        if (window.createObjectURL != undefined) { // basic
            url = window.createObjectURL(file);
        } else if (window.URL != undefined) { // mozilla(firefox)
            url = window.URL.createObjectURL(file);
        } else if (window.webkitURL != undefined) { // webkit or chrome
            url = window.webkitURL.createObjectURL(file);
        }
        return url;
    }
</script>
<script>
    function gender() {
        $(".selectlist").addClass("selectlistTop");
        $(".popup").fadeIn();
    };

    function peisong() {
        $("#demo_date").click();
    }

    function cancel() {
        $(".selectlist").removeClass("selectlistTop");
        $(".popup").fadeOut();
    };

    function setsex(val)
    {
        var str = "未知";
        if(val=="0")
        {
            str = "未知";
        }else if(val=="1")
        {
            str = "男";
        }else if(val=="2")
        {
            str = "女";
        }
        $.ajax({
            type: "post",
            url: "membersetup.aspx/setsex",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{memberID:'" + $("#memberID").val() + "',gen:'" + str + "'}",
            success: function (data) {
                
            }
        })
    }
    function setbirthday() {
        $.ajax({
            type: "post",
            url: "membersetup.aspx/setbirthday",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{memberID:'" + $("#memberID").val() + "',birthday:'" + $("#demo_date").val() + "'}",
            success: function (data) {
                 
            }
        })
    }
    function setupOk() {
        $.ajax({
            type: "post",
            url: "membersetup.aspx/setupOk",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{memberID:'" + $("#memberID").val() + "',phone:'" + $("#phone").html() + "',gen:'" + $("#gen").html() + "',birthday:'" + $("#demo_date").val() + "'}",
            success: function (data) {
                if(data.d=="1")
                {
                    alert("保存成功");
                }else if(data.d=="2")
                {
                    alert("保存失败");
                }
            }
        })
        location.href = "member.aspx";
    }
    var $aHtml = $("#gender").find("b");
    $(".selectlist").find("dd").click(function () {
        var $dlHtml = $(this).html();
        $(".selectlist").removeClass("selectlistTop");
        $(".popup").fadeOut();
        setTimeout(function () {
            $aHtml.html($dlHtml);
        }, 100);
    });
</script>
