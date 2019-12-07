<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Jurisdiction.aspx.cs" Inherits="autosell_center.main.Advertisement.Jurisdiction" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>权限设置-自动售卖终端中心控制系统</title>
    <link rel="shortcut icon" href="/main/public/images/favicon.ico" />
    <link href="/main/public/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/common.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/style.css" rel="stylesheet" type="text/css" />
    <script src="/main/public/script/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="/main/public/script/Titlelist.js" type="text/javascript"></script>
    <style>
        .zfpopup ul li{
            padding:0;
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
                        <i class="fa fa-plus"></i>
                        <span>奶企管理</span>
                    </h4>
                </div>
                <div class="common_main">
                    <div class="navlist">
                       <dl>
                            <dt>广告管理<em class="fa fa-cog"></em></dt>
                            <dd>
                                <a class="change " href="video.aspx"><i class="change  fa fa-video-camera"></i>视频管理</a>
                            </dd>

                            <dd>
                                <a class="change acolor" href="Jurisdiction.aspx"><i class="change icolor fa fa-hdd-o"></i>机器添加视频</a>
                            </dd>

                        </dl>
                        <dl>
                            <dt>小程序广告图<em class="fa fa-cog"></em></dt>
                            <dd>
                                <a class="change" href="../enterprise/hblist.aspx"><i class=" fa fa-bars"></i>小程序广告图</a>
                            </dd>
                        </dl>
                    </div>
                    <section class="jiqlist">
                        <div id="adminpopup" class="change zfpopup" style="overflow-y:scroll;top:10%;height:700px;">
                            <h4>选择添加视频</h4>
                            <ul id="uli">
                                <li>
                                    <span>视频名称</span>
                                    <span>添加时间</span>
                                    <span>添加</span>
                                </li>
                               
                            </ul>
                            <dl>
                                <dd>
                                    <input type="button" value="确定" class="popupqdbtn" onclick="addOk()" />
                                </dd>
                                <dd>
                                    <input type="button" value="取消" onclick="qxClick()" />
                                </dd>
                            </dl>
                        </div>
                        <div class="popupbj"></div>

                        <ul class="jiqlistseach">

                            <li>
                                <input type="text" value="" placeholder="设备编号" id="bh" />
                            </li>
                            <li>
                                <input type="button" value="查询" class="seachbtn" onclick="sear()" />
                            </li>
                        </ul>
                        <ul class="jiqlisttable" style="display: block;" id="ull">
                        </ul>

                    </section>
                </div>
            </div>
        </div>
        <input type="hidden" runat="server" id="companyId" />
        <input  id="mechineID" runat="server" type="hidden"/>
        <input id="_operaID" runat="server" type="hidden" />
    </form>
</body>
</html>
<script>
    function judge() {
        $.ajax({
            type: "post",
            url: "Jurisdiction.aspx/judge",
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
        judge()
        $(".jiqlistTab").find("dd").click(function () {
            $(".jiqlistTab dd").removeClass("ddcolor");
            $(this).addClass("ddcolor");
            var $liNum = $(this).index();
            $(".jiqlisttable").hide();
            $(".jiqlisttable").eq($liNum).fadeIn();
        });
        sear();
    });
    function sear()
    {
        $.ajax({
            type: "post",
            url: "Jurisdiction.aspx/search",
            contentType: "application/json; charset=utf-8",
            data: "{bh:'" + $("#bh").val() + "',companyID:'" + $("#companyId").val() + "'}",
            dataType: "json",
            success: function (data) {
                $("#ull").empty();
                var serverdata = $.parseJSON(data.d);
                var serverdatalist = serverdata.length;
                for (var i = 0; i < serverdatalist; i++) {
                    $(" <li>"
                             + "<dl>"
                             
                             + "<dd style='width:16%;margin-left:10px;'>机器编号</dd>"
                             + "<dd style='width:16%;'>机器名称</dd>"
                             +"<dd style='width:16%'>奶企</dd>"
                             + "<dd style='width:16%'>状态</dd>"
                             + "<dd style='width:16%'>当前视频数</dd>"
                             + "<dd style='width:16%'>操作</dd>"
                             + "</dl>"
        
                             + "<label style='width:16%;margin-left:10px;'>" + serverdata[i].bh + "</label>"
                             + "<label style='width:16%;'>" + serverdata[i].mechineName + "</label>"
                             + "<label style='width:16%'>" + serverdata[i].name + "</label>"
                             + "<label style='width:16%'><a>" + serverdata[i].sta + "</a></label>"
                             + "<label style='width:16%'><a>" + serverdata[i].num + "</a></label>"
                             + "<label style='width:16%'>"
                             + "<a onclick='showAddVideo(" + serverdata[i].id + ")'>添加视频</a>"
                             + "<a href='yvideolist.aspx?mechineID=" + serverdata[i].id + "'>设置</a>"
                             //+ "<a href='adVideoToMechine.aspx?mechineID=" + serverdata[i].id + "'>添加视频</a>"
                             +"</label>"
                             +"</li>").appendTo("#ull");
                }
            }
        });
    }
    
</script>
<script>
    $(function () {
        $("#li7").find("a").addClass("aborder");
    })

    function showAddVideo(id) {
        getVideoList(id);
        $(".popupbj").fadeIn();
        $("#adminpopup").addClass("zfpopup_on");
        $("#mechineID").val(id);
    }

    function qxClick() {
        $(".popupbj").hide();
        $(".zfpopup").removeClass("zfpopup_on");
    }
    
    function addVideo(obj) {
       
        if ($(obj).parent().find("input").eq(1).val() == "0") {
            $(obj).addClass("fa-check-circle")
            $(obj).parent().find("input").eq(1).val("1")
        } else if ($(obj).parent().find("input").eq(1).val() == "1") {
            $(obj).removeClass("fa-check-circle")
            $(obj).parent().find("input").eq(1).val("0")
        }
       
    }

    function addOk() {
        var id="";
     
        $(".a").each(function () {
            if ($(this).find("input").eq(1).val()=="1")
            {
                id += $(this).find("input").eq(0).val() + ",";
            }
           
        });
        id = id.substring(0, id.length - 1);
        if(id.length<=0)
        {
            alert("请先选择需要投放的视频");
            return;
        }
        $.ajax({
            type: "post",
            url: "Jurisdiction.aspx/addVideo",
            contentType: "application/json; charset=utf-8",
            data: "{mechineID:'" + $("#mechineID").val() + "',idStr:'" + id + "'}",
            dataType: "json",
            success: function (data) {
                if(data.d=="1")
                {
                    alert("视频添加完成");
                    location.reload();
                }
            }
        });
       
    }
    function getVideoList(id)
    {
        $("#uli").empty();
        $(" <li>"
            +" <span>视频名称</span>"
            +" <span>添加时间</span>"
            +"<span>添加</span>"
            + "</li>").appendTo("uli");
        $.ajax({
            type: "post",
            url: "Jurisdiction.aspx/getVideoList",
            contentType: "application/json; charset=utf-8",
            data: "{mechineID:'" + id + "',companyID:'" + $("#companyId").val() + "'}",
            dataType: "json",
            success: function (data) {
                var serverdata = $.parseJSON(data.d);
                var serverdatalist = serverdata.length;
               
                for (var i = 0; i < serverdatalist; i++) {
                    $(" <li class='a'>"
                       + "<b>" + serverdata[i].description + "</b>"
                       +"<b>" + serverdata[i].time + "</b>"
                       +"<b>"
                       + "<a class='fa' onclick='addVideo(this)'></a>"
                        + "<input type='hidden' value='" + serverdata[i].id + "' />"
                       +"<input type='hidden' value='0' />"
                       +"</b>"
                       +"</li>").appendTo("#uli");
                }
            }
        });

    }

</script>
