<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="hblist.aspx.cs" Inherits="autosell_center.main.enterprise.hblist" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link rel="shortcut icon" href="/main/public/images/favicon.ico" />
    <link href="/main/public/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/common.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/style.css" rel="stylesheet" type="text/css" />
    <script src="/main/public/script/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="/main/public/script/Titlelist.js" type="text/javascript"></script>
    
    <style>
        .jiqlistseach {
            margin-bottom: 30px;
        }
        .memberlist li .membname{
            width:16%;
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
                    <i class="fa fa-cubes"></i>
                    <span>订单管理</span>
                </h4>
            </div>
            <div class="common_main">
                <div class="navlist">
                  <dl>
                            <dt>广告管理<em class="fa fa-cog"></em></dt>
                            <dd>
                                <a class="change " href="../Advertisement/video.aspx"><i class="change  fa fa-video-camera"></i>视频管理</a>
                            </dd>
                            <dd>
                                <a class="change" href="../Advertisement/Jurisdiction.aspx"><i class="change fa fa-hdd-o"></i>机器添加视频</a>
                            </dd>
                        </dl>
                        <dl>
                            <dt>小程序广告图<em class="fa fa-cog"></em></dt>
                            <dd>
                                <a class="change acolor" href="hblist.aspx"><i class="change icolor fa fa-bars"></i>小程序广告图</a>
                            </dd>
                        </dl>
                </div>
                <section class="jiqlist">
                    <dl class="jiqlistTab">
                        <dd class="change ddcolor"><b>红包列表</b></dd>
                    </dl>
                    <ul class="jiqlistseach">
                       
                        <li >
                            <input type="button" value="添加" class="seachbtn" onclick="add()" />
                        </li>
                    
                    </ul>
                    <ul class="memberlist" id="memberList">
                       
                    </ul>
                  
                </section>
            </div>
        </div>
    </div>
       
        <input id="companyID" runat="server" type="hidden"/>
        <input id="_operaID" runat="server" type="hidden" />
    </form>
</body>
</html>
<script> 
    function judge() {
        $.ajax({
            type: "post",
            url: "hblist.aspx/judge",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{operaID:'" + $("#_operaID").val() + "',menuID:'spgl'}",
            success: function (data) {
                if (data.d.code == "500") {
                    $(".jiqlist").empty();
                    $(".jiqlist").html("<p class='noquanxian'>当前没有权限</p>").css({ "background": "#ddd" });
                }
            }
        })
    }
    $(function () {
        $("#li7").find("a").addClass("aborder");
        judge()
        getList();
    })
    function add()
    {
        window.location.href = "zfbhb.aspx";
    }
    function getList()
    {
        $("#memberList").empty();
        $(" <li>"
                + " <label style='width:16.6%'>标题</label>"
               + " <label style='width:16.6%'>类型</label>"
               + " <label style='width:16.6%'>状态</label>"
                + " <label style='width:16.6%'>开始时间</label>"
                 + " <label style='width:16.6%'>结束时间</label>"
                + " <label style='width:16.6%'>操作</label>"
        + "  </li>").appendTo("#memberList");
        $.ajax({
            type: "post",
            url: "hblist.aspx/getList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{companyID:'" + $("#companyID").val() + "'}",
            success: function (data) {
                var serverdata = $.parseJSON(data.d);
                var serverdatalist = serverdata.length;
                for (var i = 0; i < serverdatalist; i++) {
                    $(" <li>"
                        + " <span style='width:16.6%'>" + serverdata[i].des + "</span>"
                         + " <span style='width:16.6%'>" +(serverdata[i].type == "1" ? "顶部轮播" :(serverdata[i].type=="2"?"底部广告":"手机支付完成")) + "</span>"
                         + " <span style='width:16.6%'>" +(serverdata[i].status == "0" ? "正常" : "失效") + "</span>"
                         + " <span style='width:16.6%'>" + serverdata[i].startTime + "</span>"
                         + " <span style='width:16.6%'>" + serverdata[i].endTime + "</span>"
                        + "  <span style='width:16.6%'>"
                        + "     <a style='color:#0094ff' href='zfbhb.aspx?id=" + serverdata[i].id + "'>查看</a>"
                        + "     <a style='color:#0094ff' onclick='del(" + serverdata[i].id + ")'>删除</a>"
                        + " </span>"
                        + "</li>").appendTo("#memberList");
                }
            }
        })
    }
    function del(val)
    {
        if(confirm("确定需要删除吗"))
        {
            $.ajax({
                type: "post",
                url: "hblist.aspx/del",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: "{id:'" + val + "'}",
                success: function (data) {
                    alert(data.d.msg);
                }
            })
        }
        
    }
   
</script>