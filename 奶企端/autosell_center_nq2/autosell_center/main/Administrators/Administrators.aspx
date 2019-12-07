<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Administrators.aspx.cs" Inherits="autosell_center.main.Administrators.Administrators" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link rel="shortcut icon" href="/main/public/images/favicon.ico" />
    <link href="/main/public/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/common.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/style.css" rel="stylesheet" type="text/css" />
    <script src="/main/public/script/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="/main/public/script/Titlelist.js" type="text/javascript"></script>
    <style>
        .productclass {
            margin-top: 50px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="header"></div>
        <div class="main">
            <div class="main_list">
                <div class="common_title">
                    <h4>
                        <i class="fa fa-line-chart"></i>
                        <span>设备管理员</span>
                    </h4>
                </div>
                <div class="common_main">
                    <div class="navlist">
                        <dl>
                            <dt>分析<em class="fa fa-cog"></em></dt>
                            <dd>
                                <a class="change acolor"><i class="change icolor fa fa-users"></i>管理员</a>
                            </dd>
                            <dd>
                                <a class="change" href="adminadd.aspx"><i class="change fa fa-user-plus"></i>添加管理员</a>
                            </dd>
                        </dl>
                    </div>
                    <section class="jiqlist">
                        <div class="change zfpopup">
                            <h4>修改密码</h4>
                            <ul>
                                <li>
                                    <h5>输入新密码</h5>
                                    <label>
                                        <input type="text" value="" placeholder="" id="pwd"/>
                                    </label>
                                </li>
                                <li>
                                    <h5>手机号</h5>
                                    <label>
                                        <input type="text" value="" placeholder=""  id="phone"/>
                                    </label>
                                </li>
                              
                            </ul>
                            <dl>
                                <dd>
                                    <input type="button" value="确定" class="popupqdbtn"  onclick="ok()"/>
                                </dd>
                                <dd>
                                    <input type="button" value="取消" onclick="qxClick()" />
                                </dd>
                            </dl>
                        </div>
                        <div class="popupbj"></div>
                        <dl class="jiqlistTab">
                            <dd class="change ddcolor"><b>管理员设置</b></dd>
                        </dl>
                        <ul class="jiqlisttable" style="display: block;" id="ull">
                            
                        </ul>
                    </section>
                </div>
            </div>
        </div>
       <input  id="companyID" runat="server" type="hidden"/>
        <input id="menu" runat="server" type="hidden"/>
        <input  id="operaID" runat="server" type="hidden"/>
    </form>
</body>
</html>
<script>
    function setUppwd(id) {
        $("#menu").val(1);//1代表密码设置
        $("#operaID").val(id);
        $(".zfpopup").find("h4").html("密码设置");
        $(".zfpopup").find("li").eq(0).find("h5").html("输入原密码");
        $(".zfpopup").find("li").eq(0).find("label").html("<input type='text' value='' placeholder='' id='pwd'/>");
        $(".zfpopup").find("li").eq(1).show();
        $(".zfpopup").find("li").eq(2).show();
        $(".popupbj").fadeIn();
        $(".zfpopup").addClass("zfpopup_on");
         
    }
    function power(id) {
        $("#menu").val(2);//2代表权限设置
        $("#operaID").val(id);
        $(".zfpopup").find("h4").html("权限设置");
        $(".zfpopup").find("li").eq(0).find("h5").html("设置权限");
        $(".zfpopup").find("li").eq(0).find("label").html("<select id='sel1'><option id='1' value='1'>维保</option><option id='2' value='2'>运维</option></select>");
        $(".zfpopup").find("li").eq(1).hide();
        $(".zfpopup").find("li").eq(2).hide();
        $(".popupbj").fadeIn();
        $(".zfpopup").addClass("zfpopup_on");
    }
    function qxClick() {
        $(".zfpopup").removeClass("zfpopup_on");
        setTimeout(function () { $(".popupbj").hide(); }, 300);
        location.reload();
    }
    function ok()
    {
        if($("#menu").val()=="1")
        {
            
            alert($("#pwd").val());
            if ($("#pwd").val() == "") {
                alert("密码不能为空");
                return;
            }
            if ($("#phone").val() == "") {
                alert("密码不能为空");
                return;
            }
            if ($("#phone").val().substring(0, 1) != "1" || $("#phone").val().length < 11) {
                alert("手机号不正确");
                return;
            }
            $.ajax({
                type: "post",
                url: "Administrators.aspx/setPwd",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: "{operaID:'" + $("#operaID").val() + "',pwd:'" + $("#pwd").val() + "',phone:'" + $("#phone").val() + "'}",
                success: function (data) {
                    if(data.d=="1")
                    {
                        alert("原密码不正确");
                    }else if(data.d=="2")
                    {
                        alert("修改成功");
                    }
                }
            })
        }else if($("#menu").val()=="2")
        {

            $.ajax({
                type: "post",
                url: "Administrators.aspx/setQX",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: "{operaID:'" + $("#operaID").val() + "',qx:'" + $("#sel1").val() + "'}",
                success: function (data) {
                    if (data.d == "1") {
                        alert("权限设置成功");
                    } else if (data.d == "2") {
                        alert("权限设置失败");
                    }
                }
            })
        }
    }
    $(function () {
        $("#li5").find("a").addClass("aborder");
        sear();
    })
    function sear()
    {
        $("#ull").empty();
        $.ajax({
            type: "post",
            url: "Administrators.aspx/getadminList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{companyID:'" + $("#companyID").val() + "'}",
            success: function (data) {
                var serverdata = $.parseJSON(data.d);
                var serverdatalist = serverdata.length;
                for (var i = 0; i < serverdatalist; i++) {
                    var zt = "";
                    if (serverdata[i].qx=="1")
                    {
                        zt = "维保";
                    } else if (serverdata[i].qx == "2")
                    {
                        zt="运维";
                    }
                    $("<li>"
                           +"     <dl>"
                           +"         <dd style='width:25%'>管理员</dd>"
                            + "         <dd style='width:25%'>联系电话</dd>"
                             + "         <dd style='width:25%'>修改</dd>"
                            + "         <dd style='width:25%'>权限设置</dd>"
                            +"     </dl>"
                            + "     <label style='width:25%'>"
                               +"      <img src='/main/public/images/admin.png' alt='' />"
                                + "     <span>" + serverdata[i].name + "</span>"
                                 + "    <em>创建时间:<i>" + serverdata[i].createTime + "</i></em>"
                                 + "</label>"
                                 + " <label style='width:25%'> <span>" + serverdata[i].linkphone + "</span></label>"
                                 + "<label style='width:25%'>"
                                  + "   <a onclick='setUppwd(" + serverdata[i].id + ")'>修改</a>"
                                 +"</label>"
                                 + "<label style='width:25%'>"
                                  +"   <b>"+zt+"</b>"
                                   + "  <a onclick='power(" + serverdata[i].id + ")'>设置</a>"
                                +" </label>"
                            +" </li>").appendTo("#ull");
                }
            }
        })
    }
</script>
