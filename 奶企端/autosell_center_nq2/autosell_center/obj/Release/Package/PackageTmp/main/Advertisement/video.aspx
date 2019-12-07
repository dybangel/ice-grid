<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="video.aspx.cs" Inherits="autosell_center.main.Advertisement.video" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>视频管理-自动售卖终端中心控制系统</title>
    <link rel="shortcut icon" href="/main/public/images/favicon.ico" />
    <link href="/main/public/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/common.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/style.css" rel="stylesheet" type="text/css" />

    <link rel="stylesheet" type="text/css" href="../bootstrapSelect/css/bootstrap.css" />
    <link rel="stylesheet" type="text/css" href="../bootstrapSelect/dist/css/bootstrap-select.css" />
    <script src="../bootstrapSelect/dist/js/jquery.js"></script>
    <script src="../bootstrapSelect/dist/js/bootstrap.min.js"></script>
    <script src="../bootstrapSelect/dist/js/bootstrap-select.min.js" charset="gb2312"></script>


    <script src="/main/public/script/Titlelist.js" type="text/javascript"></script>
    <link href="../../scripts/skin/jedate.css" rel="stylesheet" type="text/css" />
    <script src="../../scripts/jedate.js" type="text/javascript"></script>
    <script src="../../scripts/jedate.min.js"></script>
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
            text-overflow: ellipsis;
            white-space: nowrap;
        }

        .productlist li .pronaiq {
            width: 24%;
        }

        .zfpopup ul li span {
            display: inline-block;
            width: initial;
            float: initial;
            text-align: left;
            line-height: inherit;
            border: 0;
        }

        .setupPSY .bootstrap-select {
            width: 100% !important;
        }
    </style>
    <script>
        window.onload = function () {
            jeDate({
                dateCell: "#start", //isinitVal:true,
                //format: "YYYY-MM-DD",
                isTime: true, //isClear:false,
                choose: function (val) { },
                minDate: "2014-09-19 00:00:00"
            });
            jeDate({
                dateCell: "#end",
                isinitVal: true,
                isTime: true, //isClear:false,
                minDate: "2014-09-19 00:00:00"
            });
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="header"></div>
        <div class="main">
            <div id="adminpopup" class="change zfpopup">
                <h4 id="title">添加机器设置</h4>
                <ul>
                    <li>
                        <h5>播放类型</h5>
                        <label>
                            <select name="sel1" id="sel1">
                                <option value="0">请选择类型</option>
                                <option value="1">按次数</option>
                                <option value="2">按到期时间</option>
                            </select>
                        </label>
                    </li>
                     <li id="startTime">
                                    <h5>设置开始时间</h5>
                                    <label>
                                        <input name="act_stop_timeks" autocomplete="off" type="text" id="end"   runat="server"  class="input" value="" placeholder="到期时间" />
                                    </label>
                                </li>
                    <li id="setNum" style="display:none">
                        <h5>设置次数</h5>
                        <label>
                            <input type="text" value="" id="num" maxlength="10" />
                        </label>
                    </li>
                    <li id="setTime" style="display:none">
                        <h5>设置到期时间</h5>
                        <label>
                            <input name="act_stop_timeks" autocomplete="off" type="text" id="start" runat="server" class="input" value="" placeholder="到期时间" />
                        </label>
                    </li>
                    <li>
                        <h5>设置机器</h5>
                        <label class="setupPSY">
                            <select id="mechineList" class="selectpicker" multiple data-live-search="true" data-live-search-placeholder="请输入搜索" data-actions-box="true">
                            </select>
                        </label>
                    </li>
                </ul>
                <dl>
                    <dd>
                        <input type="button" value="确定" class="popupqdbtn" onclick="ok()" />
                    </dd>
                    <dd>
                        <input type="button" value="取消" onclick="qxClick()" />
                    </dd>
                </dl>
            </div>


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
                                <a class="change acolor" href="video.aspx"><i class="change icolor fa fa-video-camera"></i>视频管理</a>
                            </dd>

                            <dd>
                                <a class="change" href="Jurisdiction.aspx"><i class="change fa fa-hdd-o"></i>机器添加视频</a>
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
                        <dl class="jiqlistTab">
                            <dd class="change ddcolor"><b>视频列表</b></dd>
                            <dd id="adddd">
                                <a id="addvideo" class="change" href="add_video.aspx">添加视频</a>
                            </dd>
                        </dl>
                        <ul class="jiqlistseach">
                            <li>
                                <input type="text" value="" placeholder="视频名称" id="videoName" />
                            </li>
                            <li>
                                <input type="text" value="" placeholder="机器编号" id="mechineBH" />
                            </li>

                            <li></li>
                            <li>
                                <input type="button" value="查询" class="seachbtn" id="search" onclick="sear()" />
                            </li>
                        </ul>
                        <ul class="productlist" id="ull">
                        </ul>
                    </section>
                </div>
            </div>
        </div>
        <input id="company_id" runat="server" type="hidden" />
        <input id="_videoID" runat="server" type="hidden" />
        <input id="_operaID" runat="server" type="hidden" />
    </form>
</body>
</html>
<script>
    function judge() {
        $.ajax({
            type: "post",
            url: "video.aspx/judge",
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
    function ok() {
        
        $.ajax({
            type: "post",
            url: "video.aspx/ok",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{videoID:'" + $("#_videoID").val() + "',mechineListID:'" + $("#mechineList").val() + "',sel1:'" + $("#sel1").val() + "',num:'" + $("#num").val() + "',start:'" + $("#start").val() + "',startTime:'" + $("#end").val() + "'}",
            success: function (data) {
                if (data.d.code == "200") {
                     
                     alert(data.d.msg);
                } else {
                    alert(data.d.msg);
                }

            }
        })
    }
    function getMechineList() {
        $.ajax({
            type: "post",
            url: "video.aspx/getMechineList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{companyID:'" + $("#company_id").val() + "'}",
            success: function (data) {
                if (data.d.code == "200") {
                    var serverdata = $.parseJSON(data.d.db);
                    var serverdatalist = serverdata.length;
                    var optionString = "";
                    for (i = 0; i < serverdatalist; i++) {
                        optionString += "<option value=\'" + serverdata[i].id + "\'>" + serverdata[i].mechineName + "</option>";
                    }
                    var myobj = document.getElementById("mechineList");

                    $("#mechineList").html(optionString);
                    $("#mechineList").selectpicker('refresh');

                } else {
                    //alert(data.d.msg);
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

        $("span.proname").find("em").hover(function () {
            $(this).find("i").fadeToggle(100);
        });
        $("span.proname").find("i").click(function () {
            setTimeout(function () {
                $(".addDiv").addClass("addDivshow");
            }, 100);
            $(".popupbj").fadeIn();
        });
        $("#sel1").click(function () {
            if ($(this).val() == "1") {
                $("#setNum").show().siblings("#setTime").hide()
            } else if ($(this).val() == "2") {
                $("#setTime").show().siblings("#setNum").hide()
            } else if ($(this).val() == "0") {
                $("#setNum,#setTime").hide();
            }
        })
        getMechineList();
        sear();
    });
    function divOff() {
        $(".addDiv").removeClass("addDivshow");
        setTimeout(function () {
            $(".popupbj").fadeOut();
        }, 300);
    }
    function sear() {
        $("#ull").empty();
        $(" <li>"
                      + "  <label style='width:20%'>视频名称</label>"
                      + "  <label  style='width:20%'>播放次数</label>"
                      + "  <label  style='width:20%'>类型</label>"
                      + "  <label style='width:20%'>状态</label>"
                      + "  <label class='procz' style='width:20%;'>操作</label>"
                      + " </li>").appendTo("#ull");
        $.ajax({
            type: "post",
            url: "video.aspx/getVideoList",
            contentType: "application/json; charset=utf-8",
            data: "{videoName:'" + $("#videoName").val() + "',companyID:'" + $("#company_id").val() + "'}",
            dataType: "json",
            success: function (data) {
                var serverdata = $.parseJSON(data.d);
                var serverdatalist = serverdata.length;
                for (var i = 0; i < serverdatalist; i++) {
                    $("<li><span style='width:20%'>" + serverdata[i].description + "</span>"
                        + "<span style='width:20%'>" + serverdata[i].totalTimes + "</span>"
                        + "<span style='width:20%'>" + (serverdata[i].type == "0" ? "横屏" : "竖屏") + "</span>"
                        + "<span style='width:20%'>" + (serverdata[i].shType == "0" ? "待审核" : (serverdata[i].shType == "1" ? "通过" : "不通过")) + "</span>"
                        + "<span style='width:20%' class='procz'>"
                          + " <a onclick='deleteVideo(\"" + serverdata[i].id + "\",\"" + serverdata[i].path +"\")'>删除该视频</a>"
                        + " <a onclick='showAddVideo(\"" + serverdata[i].id + "\",\"" + serverdata[i].shType + "\",\"" + serverdata[i].description + "\")'>添加到机器</a>"
                         + " <a onclick='lookVideo(\"" + serverdata[i].path+ "\")'>下载</a>"
                        + "</span>"
                    + "</li>").appendTo("#ull");
                }
            }
        })
    }
    function lookVideo(path) {
        window.location.href = path;
    }
    function deleteVideo(id, path) {
        var msg = "您真的确定要删除吗？\n\n请确认！"; 
        if (confirm(msg)==true){ 
            $.ajax({
                type: "post",
                url: "video.aspx/deleteVideo",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: "{videoID:'" + id + "',path:'" + path + "'}",
                success: function (data) {
                    if (data.d.code == "200") {
                        window.location.reload();
                        alert(data.d.msg);

                    } else {
                        //alert(data.d.msg);
                    }

                }
            })
        }else{ 
            return false; 
        } 
    }
    function showAddVideo(id, type,name) {
        if (type != "1") {
            alert("未审核的视频不允许添加");
            return;
        }
        $("#title").html(name)
        $("#_videoID").val(id);
        $(".popupbj").fadeIn();
        $("#adminpopup").addClass("zfpopup_on");
    }
    function qxClick() {
        $(".tangram-suggestion-main").hide();
        $(".popupbj").hide();
        $(".zfpopup").removeClass("zfpopup_on");
    }
    
</script>
<script>
    $(function () {
        $("#li7").find("a").addClass("aborder");
    })
</script>
