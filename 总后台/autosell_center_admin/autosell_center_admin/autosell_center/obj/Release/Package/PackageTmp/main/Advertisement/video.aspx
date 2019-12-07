<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="video.aspx.cs" Inherits="autosell_center.main.Advertisement.video" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>视频管理-自动售卖终端中心控制系统</title>
    <link rel="shortcut icon" href="/main/public/images/favicon.ico"/>
    <link href="/main/public/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css"/>
    <link href="/main/public/css/common.css" rel="stylesheet" type="text/css"/>
    <link href="/main/public/css/style.css" rel="stylesheet" type="text/css"/>
    <script src="/main/public/script/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="/main/public/script/Titlelist.js" type="text/javascript"></script>
    <style>
        #adddd {
            position: absolute;
            right: 30px;
        }
         #addvideo {
             position: absolute;
             border-radius: 5px;
             background: #3a77d5;
             color: #fff;
             right: 0;
             height: 32px;
             top: 4px;
             width: 120px;
             line-height: 32px;
             font-size: 1rem;
         }
         .productlist li span {
             overflow: hidden;
            text-overflow:ellipsis;
            white-space: nowrap;
         }
         .productlist li .pronaiq {
             width: 10%;
         }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="header"></div>
<div class="main">
    <div class="addDiv change">
        <h4>查看视频</h4>
        <video src="/main/public/video/movie.mp4" controls="controls"></video>
    </div>
    <div class="popupbj" onclick="divOff()"></div>
    <div class="main_list">
        <div class="common_title">
            <h4>
                <i class="fa fa-plus"></i>
                <span>广告管理</span>
            </h4>
            
        </div>
        <div class="common_main">
            <div class="navlist">
                <dl>
                    <dt>广告管理<em class="fa fa-cog"></em></dt>
                    <dd>
                        <a class="change acolor"><i class="change icolor fa fa-video-camera"></i>视频管理</a>
                    </dd>
                   
                    <dd>
                        <a class="change" href="Jurisdiction.aspx"><i class="change fa fa-hdd-o"></i>机器添加视频</a>
                    </dd>
                </dl>
                 <dl>
                            <dt>小程序广告图<em class="fa fa-cog"></em></dt>
                            <dd>
                                <a class="change " href="hblist.aspx"><i class="change icolor fa fa-bars"></i>小程序广告图</a>
                            </dd>
                        </dl>
            </div>
            <section class="jiqlist">
                 <dl class="jiqlistTab">
                    <dd class="change ddcolor"><b>视频列表</b></dd>
                    <dd id="adddd">
                        <a id="addvideo" class="change" href="add_video.aspx">添加视频</a>
                    </dd>
                </dl>
                <ul class="jiqlistseach">
                    <li>
                        <input type="text" value="" placeholder="视频名称" id="videoName"/>
                    </li>
                    <li>
                        <input type="text" value="" placeholder="机器编号" id="mechineBH"/>
                    </li>
                    <li>
                        <asp:DropDownList ID="companyList" runat="server"></asp:DropDownList>
                    </li>
                     <li>
                        <select id="tgType"> 
                            <option value="-1">全部</option>
                             <option value="0">待审核</option>
                             <option value="1">已通过</option>
                             <option value="2">被拒绝</option>
                        </select>
                    </li>
                    <li>
                        <input type="button" value="查询" class="seachbtn"  id="search" onclick="sear()"/>
                    </li>
                </ul>
                <ul class="productlist" id="ull">
                    
                </ul>
                 <div class="commonPage">
                            <a class="change" onclick="getPage('first')">首页</a>
                            <a class="change" onclick="getPage('up')">上一页</a>
                            <a class="change" onclick="getPage('down')">下一页</a>
                            <a class="change" onclick="getPage('end')">尾页</a>
                            <select id="pageSel" onchange="pageChg()">
                            </select>
                        </div>
            </section>
        </div>
    </div>
</div>
         <input id="pageCurrentCount" runat="server" type="hidden" value="1" />
        <input id="pageTotalCount" runat="server" type="hidden" value="1" />
<!--<div class="login_foot">
    <span>青岛冰格科技公司版权所有 翻版必究</span>
</div>-->
    </form>
</body>
</html>
<script>
    $(function () {
        $("#li9").find("a").addClass("aborder");
        qx_judge('spgl');
        $(".jiqlistTab").find("dd").click(function() {
            $(".jiqlistTab dd").removeClass("ddcolor");
            $(this).addClass("ddcolor");
            var $liNum = $(this).index();
            $(".jiqlisttable").hide();
            $(".jiqlisttable").eq($liNum).fadeIn();
        });

        $("span.proname").find("em").hover(function() {
            $(this).find("i").fadeToggle(100);
        });
        $("span.proname").find("i").click(function () {
            setTimeout(function () {
                $(".addDiv").addClass("addDivshow");
            }, 100);
            $(".popupbj").fadeIn();
        });
        sear();
    });
    function getPage(val) {
        if (val == "first") {
            $("#pageCurrentCount").val("1");
        } else if (val == "up") {
            var index = parseInt($("#pageCurrentCount").val()) - 1;
            if (index >= 1) {
                $("#pageCurrentCount").val(index);
            }
        } else if (val == "down") {
            var index = parseInt($("#pageCurrentCount").val()) + 1;
            if (index <= parseInt($("#pageTotalCount").val())) {
                $("#pageCurrentCount").val(index);
            }
        } else if (val == "end") {
            $("#pageCurrentCount").val($("#pageTotalCount").val());
        }
        sear();
    }
    function pageChg() {
        $("#pageCurrentCount").val($("#pageSel").val());
        sear();
    }
    function divOff() {
        $(".addDiv").removeClass("addDivshow");
        setTimeout(function() {
            $(".popupbj").fadeOut();
        }, 300);
    }
    function sear()
    {
        $("#ull").empty();
        $(" <li>"
                      + "  <label class='proname'>视频名称</label>"
                      + "  <label style='width:10%'>机器编号</label>"
                      + "  <label style='width:10%'>公司</label>"
                      + "  <label style='width:10%'>联系人</label>"
                      + "  <label style='width:10%'>播放次数</label>"
                      + "  <label style='width:10%'>类型</label>"
                      + "  <label style='width:10%'>状态</label>"
                      + "  <label class='procz' style='width:20%'>操作</label>"
                      + " </li>").appendTo("#ull");
        $.ajax({
            type: "post",
            url: "video.aspx/getVideoList",
            contentType: "application/json; charset=utf-8",
            data: "{videoName:'" + $("#videoName").val() + "',qy:'" + $("#companyList").val() + "',mechineBH:'" + $("#mechineBH").val() + "',type:'" + $("#tgType").val() + "',pageCurrentCount:'" + $("#pageCurrentCount").val() + "'}",
            dataType: "json",
            success: function (data) {
                var count = data.d.count;
                if (parseInt(count) >= 0) {
                    $("#pageSel").empty();
                    for (var k = 1; k <= parseInt(count) ; k++) {
                        $("<option value='" + k + "'>" + k + "</option>").appendTo("#pageSel");
                    }
                }
                var serverdata = $.parseJSON(data.d.db);
                $("#pageTotalCount").val(count);
                var serverdatalist = serverdata.length;
                for (var i = 0; i < serverdatalist; i++) {
                    var zt = "";
                    if (serverdata[i].shType=="0")
                    {
                        zt = "待审核";
                    } else if (serverdata[i].shType == "1")
                    {
                        zt = "审核通过";
                    } else if (serverdata[i].shType == "2") {
                        zt = "审核不通过";
                    }
                   
                    $("  <li>"
                        //+"<span class='proname'>"
                        //+"  <em>"
                        //+"    <i class='fa fa-play-circle'></i>"
                        //+"  <img src='/main/public/images/video.jpg' />"
                        //+"</em>"
                        //+"<i>"+serverdata[i].name+"</i>"
                        //+"</span>"
                       + "<span class='proname'>" + serverdata[i].name + "</span>"
                        + "<span style='width:10%'>" + serverdata[i].bh + "</span>"
                        + "<span style='width:10%'>" + serverdata[i].companyName + "</span>"
                        + "<span style='width:10%'>" + serverdata[i].linkman + "</span>"
                      
                        + "<span style='width:10%'>" + serverdata[i].times + "次</span>"
                        + "<span style='width:10%'>" + serverdata[i].typeName + "</span>"
                         + "<span style='width:10%'>" + zt + "</span>"
                        + "<span class='procz' style='width:20%'>"
                        + "  <a href='videoaddlist.aspx?videoID=" + serverdata[i].videoID + "'>添加到机器</a>"
                       + " <a onclick='deLete(\"" + serverdata[i].id + "\",\"" + serverdata[i].path + "\")'>删除</a>"
                       + " <a onclick='look("+serverdata[i].id+")'>查看</a>"
                       + " <a onclick='pass(" + serverdata[i].id + ")'>通过</a>"
                       + " <a onclick='jujue(" + serverdata[i].id + ")'>拒绝</a>"
                       +" </span>"
                    +"</li>").appendTo("#ull");
                }
            }
        })
    }
    function deLete(id,path)
    {
        var msg = "您真的确定要删除吗？\n\n请确认！"; 
        if (confirm(msg)==true){ 
            $.ajax({
                type: "post",
                url: "video.aspx/deLete",
                contentType: "application/json; charset=utf-8",
                data: "{videoID:'" + id + "',path:'" + path + "'}",
                dataType: "json",
                success: function (data) {
                    if (data.d == "2") {
                        alert("删除成功");
                        window.location.reload();
                    } else if (data.d == "1") {
                        alert("有未到期的视频暂时无法删除");
                    } 
                }
            })
        } else {
            return false;
        }
    }
    function look(id)
    {
        $.ajax({
            type: "post",
            url: "video.aspx/look",
            contentType: "application/json; charset=utf-8",
            data: "{id:'" + id + "'}",
            dataType: "json",
            success: function (data) {
                if(data.d!="2"){
                    window.location.href = data.d
                } else {

                    alert("视频不存在")
                }
               
            }
        })
      
    }
    function pass(id)
    {
        $.ajax({
            type: "post",
            url: "video.aspx/pass",
            contentType: "application/json; charset=utf-8",
            data: "{id:'" + id + "'}",
            dataType: "json",
            success: function (data) {
                if (data.d == "2") {
                    alert("审核通过");
                    window.location.reload();
                } else if (data.d == "1") {
                    alert("已经审核通过");
                } else {
                    alert("审核失败");
                }                 
            }
        })
    }
    function jujue(id) {
        $.ajax({
            type: "post",
            url: "video.aspx/jujue",
            contentType: "application/json; charset=utf-8",
            data: "{id:'" + id + "'}",
            dataType: "json",
            success: function (data) {
                if (data.d == "2") {
                    alert("已经拒绝");
                } else if (data.d == "1") {
                    alert("该状态无法拒绝");
                } else {
                    alert("审核失败");
                }
            }
        })
    }
    function qx_judge(menuID) {
        //首先验证账号和密码正确
        $.ajax({
            url: "../../../ashx/asm.ashx",
            type: 'post',
            dataType: 'json',
            timeout: 10000,
            data: {
                action: "qx_judge",
                menu: menuID
            },
            success: function (resultData) {
                if (resultData.result == "ok")//允许查看跳转
                {
                    if (menuID == 'nqlb') {//会员列表
                        location.href = "SellCenter.aspx";
                    }
                    if (menuID == 'xznq') {//新增奶企
                        location.href = "FirmAdd.aspx";
                    }
                    if (menuID == 'sblb') {//设备类别
                        location.href = "mechineTypeList.aspx";
                    }
                    if (menuID == 'tjsb') {//添加设备
                        location.href = "equipmentadd.aspx";
                    }
                    if (menuID == 'qbsb') {//全部设备
                        location.href = "Allequipment.aspx";
                    }
                    if (menuID == 'qbsb') {//设备管理
                        location.href = "/main/equipment/Allequipment.aspx";
                    }
                    if (menuID == 'hylb') {//会员管理
                        location.href = "memberlist.aspx";
                    }
                    if (menuID == 'dgjl') {//订购记录
                        //location.href = "dglist.aspx";
                    }
                    if (menuID == 'gmjl') {//购买记录
                        location.href = "orderlist.aspx";
                    }
                    if (menuID == 'spgl') {//视频管理

                    }

                } else if (resultData.result == "notLogin")//没有查看权限
                {
                    $(".jiqlist").empty();
                    $(".jiqlist").html("<p class='noquanxian'>当前没有权限</p>").css({ "background": "#ddd" });
                }
                else if (resultData.result == "1")//请联系管理员给当前登录角色赋值权限
                {
                    alert("请联系管理员给当前登录角色赋值权限");
                }
                else if (resultData.result == "2")//跳转重新登录
                {
                    location.href = "../../../../index.aspx";
                }

            }
        })
    }
</script>
<script>
    $(function() {
        
    })
</script>