<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="yvideolist.aspx.cs" Inherits="autosell_center.main.Advertisement.yvideolist" %>

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
     <link  href="../../scripts/skin/jedate.css" rel="stylesheet" type="text/css"/>
    <script src="../../scripts/jedate.js" type="text/javascript"></script>
    <script src="../../scripts/jedate.min.js"></script>
     <script type="text/javascript">
         window.onload = function () {
             jeDate({
                 dateCell: "#start", //isinitVal:true,
                 //format: "YYYY-MM-DD",
                 isTime: true, //isClear:false,
                 choose: function(val) {},
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
    <style>
         .memberlist {
                margin-top: 30px;
            }
            .memberlist li a {
                color: #3a77d5;
            }
            #setNum,#setTime{
                display:none;
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
                    </div>
                    <section class="jiqlist">
                        <div id="adminpopup" class="change zfpopup">
                            <h4>视频设置</h4>
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
                                        <input name="act_stop_timeks" type="text" id="end"   runat="server"  class="input" value="" placeholder="到期时间" />
                                    </label>
                                </li>
                                <li id="setNum">
                                    <h5>设置次数</h5>
                                    <label>
                                        <input type="text" value=""  id="num" maxlength="10"/>
                                    </label>
                                </li>
                                <li id="setTime">
                                    <h5>设置到期时间</h5>
                                    <label>
                                        <input name="act_stop_timeks" type="text" id="start"   runat="server"  class="input" value="" placeholder="到期时间" />
                                    </label>
                                </li>
                            </ul>
                            <dl>
                                <dd>
                                    <input type="button" value="确定" class="popupqdbtn" onclick="setOk()" />
                                </dd>
                                <dd>
                                    <input type="button" value="取消" onclick="qxClick()" />
                                </dd>
                            </dl>
                        </div>
                        <div class="popupbj"></div>
                      
                        <div class="memberjs">
                            <p>
                                <em class="fa fa-info"></em>
                                <span>视频需设置<i>播放类型</i>之后方可进行播放，否则无法进行正常播放。</span>
                               
                            </p>
                              <span>机器编号:<%=bh %>;&nbsp;&nbsp;&nbsp;机器名称:<%=mechineName %></span>
                        </div>
                        <ul class="memberlist" id="ull">
                            <li>
                                <label style="width: 12.5%">视频名称</label>
                                <label style="width: 12.5%">视频简介</label>
                                <label style="width: 12.5%">上传时间</label>
                                <label style="width: 12.5%">视频类型</label>
                                <label style="width: 12.5%">播放类型</label>
                                <label style="width: 12.5%">次数</label>
                                <label style="width: 12.5%">有效期</label>
                                <label style="width: 12.5%">操作</label>
                            </li>
                            <li>
                                <span style="width: 12.5%">视频名称一</span>
                                <span style="width: 12.5%">简介</span>
                                <span style="width: 12.5%">2018-02-02 16:14:54</span>
                                <span style="width: 12.5%">横屏</span>
                                <span style="width: 12.5%">按次数</span>
                                <span style="width: 12.5%">1000</span>
                                <span style="width: 12.5%">-</span>
                                <span style="width: 12.5%">
                                    <a onclick="showSet()">设置</a>
                                    <a>移除</a>
                                </span>
                            </li>

                        </ul>
                    </section>
                </div>
            </div>
        </div>
        <input id="companyID" runat="server" type="hidden" />
        <input id="mechineID" runat="server" type="hidden" />
        
        <input id="ID" runat="server" type="hidden"/>
    </form>
</body>
</html>
<script>
    $(function () {
        $("#li7").find("a").addClass("aborder");
        sear();
    })
    $(function () {
        $("#sel1").click(function () {
            if ($(this).val() == "1") {
                $("#setNum").show().siblings("#setTime").hide()
            } else if ($(this).val() == "2") {
                $("#setTime").show().siblings("#setNum").hide()
            } else  {
                $("#setNum").show();
            }
        })
    })

    function showSet(id) {
        //获取当前状态
        $.ajax({
            type: "post",
            url: "yvideolist.aspx/getStatu",
            contentType: "application/json; charset=utf-8",
            data: "{id:'"+id+"'}",
            dataType: "json",
            success: function (data) {
                if (data.d == "1") {
                    alert("当前视频已经过期");
                } else {
                    $(".popupbj").fadeIn();
                    $("#adminpopup").addClass("zfpopup_on");
                    $("#ID").val(id);
                }
            }
        });
       
    }

    function qxClick() {
        $(".popupbj").hide();
        $(".zfpopup").removeClass("zfpopup_on");
    }

    function setOk() {
        var str = "";
        if ($("#sel1").val()=="0")
        {
            alert("请设置播放类型");
            return;
        }
        if($("#sel1").val()=="1")
        {
            str=$("#num").val();
        }else if($("#sel1").val()=="2")
        {
            str=$("#start").val();
        }
        $.ajax({
            type: "post",
            url: "yvideolist.aspx/setOK",
            contentType: "application/json; charset=utf-8",
            data: "{mechineID:'" + $("#mechineID").val() + "',type:'" + $("#sel1").val() + "',val:'" + str + "',id:'" + $("#ID").val() + "',startTime:'" + $("#end").val() + "'}",
            dataType: "json",
            success: function (data) {
                alert("设置成功!")
                location.reload();
            }
        });
       
    }
    function sear()
    {
        $("#ull").empty();
        $("<li>"
            +"<label style='width: 8.3%'>视频名称</label>"
            +"<label style='width: 8.3%'>视频简介</label>"
            +"<label style='width: 8.3%'>上传时间</label>"
            +"<label style='width: 5%'>视频类型</label>"
            +"<label style='width: 8.3%'>播放类型</label>"
            +"<label style='width: 8.3%'>次数</label>"
            + "<label style='width: 8.3%'>已播放次数</label>"
            + "<label style='width: 8.3%'>开始时间</label>"
            + "<label style='width: 8.3%'>有效期</label>"
            + "<label style='width: 8.3%'>状态</label>"
             + "<label style='width: 8.3%'>播放状态</label>"
            +"<label style='width: 11.6%'>操作</label>"
            +"</li>").appendTo("#ull");
        $.ajax({
            type: "post",
            url: "yvideolist.aspx/sear",
            contentType: "application/json; charset=utf-8",
            data: "{mechineID:'" + $("#mechineID").val() + "'}",
            dataType: "json",
            success: function (data) {
               
                var serverdata = $.parseJSON(data.d);
                var serverdatalist = serverdata.length;
                for (var i = 0; i < serverdatalist; i++) {
                    var type = "";
                    if (serverdata[i].type=="0")
                    {
                        type = "横屏";
                    } else if (serverdata[i].type == "1")
                    {
                        type = "竖屏";
                    }
                    var tfType = "";
                    var tfcs = "-";
                    var ybf = "-";
                    var valiDate = "-";
                    if (serverdata[i].tfType=="0")
                    {
                        tfType = "未设置";
                    } else if (serverdata[i].tfType == "1") {
                        tfType = "按次数投放";
                        tfcs = serverdata[i].tfcs;
                        ybf = serverdata[i].ybf
                    } else if (serverdata[i].tfType == "2") {
                        tfType = "按时间投放";
                        valiDate = serverdata[i].valiDate;
                        ybf = serverdata[i].ybf
                    }
                    var zt = "";
                    if (serverdata[i].zt=="0")
                    {
                        zt = "未到期";
                    }else if(serverdata[i].zt=="1")
                    {
                        zt = "已到期";
                    }
                    $(" <li>"
                        +"<span style='width: 8.3%'>" + serverdata[i].name + "</span>"
                        +"<span style='width: 8.3%'>" + serverdata[i].description + "</span>"
                        +"<span style='width: 8.3%'>" + serverdata[i].tfTime + "</span>"
                        +"<span style='width: 5%'>"+type+"</span>"
                        +"<span style='width: 8.3%'>" + tfType + "</span>"
                        +"<span style='width: 8.3%'>" + tfcs + "</span>"
                        + "<span style='width: 8.3%'>" + serverdata[i].times + "</span>"
                         + "<span style='width: 8.3%'>" + serverdata[i].startTime + "</span>"
                        + "<span style='width: 8.3%'>" + valiDate + "</span>"
                        + "<span style='width: 8.3%'>" + zt + "</span>"
                         + "<span style='width: 8.3%'>" + (serverdata[i].is_open == "0" ? "播放" : (serverdata[i].is_open=="1"?"暂停":"")) + "</span>"
                        +"<span style='width: 11.6%'>"
                        +"<a onclick='showSet(" + serverdata[i].ID + ")'>设置</a>|"
                        + "<a onclick='del(" + serverdata[i].ID + ")'>移除</a>"
                        + "<a onclick='is_open(" + serverdata[i].ID + ")'>暂停/开启</a>|"
                         + "<a onclick='downVideo(\"" + serverdata[i].path + "\")'>下载</a>"
                        +"</span>"
                        +"</li>").appendTo("#ull");
                }
            }
        });
    }
    function downVideo(path)
    {
        window.location.href = path;
    }
    function is_open(id)
    {
        $.ajax({
            type: "post",
            url: "yvideolist.aspx/is_open",
            contentType: "application/json; charset=utf-8",
            data: "{id:'" + id + "',mechineID:'" + $("#mechineID").val() + "'}",
            dataType: "json",
            success: function (data) {
                if (data.d == "1") {
                    location.reload();
                }
              
            }
        });
    }
    function del(id)
    {
        if(confirm("是否确认删除")){
             $.ajax({
            type: "post",
            url: "yvideolist.aspx/del",
            contentType: "application/json; charset=utf-8",
            data: "{id:'" + id + "',mechineID:'" + $("#mechineID").val() + "'}",
            dataType: "json",
            success: function (data) {
                if(data.d=="1")
                {
                            alert("当前视频有未完成的任务无法删除");
                            return;
                }else if(data.d=="2")
                {
                            alert("删除成功");
                            return;
                }else if(data.d=="0")
                {
                            alert("删除失败");
                            return;
                }
                        location.reload();
                }
                });
        }

    }
</script>