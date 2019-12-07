<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rolelist.aspx.cs" Inherits="autosell_center.main.Adminlist.rolelist" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>管理员管理-自动售卖终端中心控制系统</title>
    <link rel="shortcut icon" href="/main/public/images/favicon.ico" />
    <link href="/main/public/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/common.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/style.css" rel="stylesheet" type="text/css" />
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

        .memberlist {
            margin-top: 30px;
        }

            .memberlist a {
                color: #3a77d5;
                margin-right: 6px;
            }

            .memberlist b {
                font-weight: normal;
                color: #3a77d5;
            }

        .productlist li span {
            overflow: hidden;
            text-overflow: ellipsis;
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
                        <span>管理员管理</span>
                    </h4>
                </div>
                <div class="common_main">
                    <div class="navlist">
                        <dl>
                            <dt>管理员管理<em class="fa fa-cog"></em></dt>
                            <dd>
                                <a class="change" href="adminlist.aspx"><i class="change fa fa-user"></i>管理员管理</a>
                            </dd>

                            <dd>
                                <a class="change acolor"><i class="change icolor fa fa-user-circle"></i>角色管理</a>
                            </dd>
                        </dl>
                    </div>
                    <div id="addCaDiv" class="addDiv change">
                            <h4>添加角色</h4>
                            <ul>
                                <li>
                                    <label>角色名称</label>
                                    <input type="text" value="" placeholder="填写角色名称" id="addname"/>
                                </li>
                                <li>
                                    <label style="width: 100%">角色权限</label>
                                    <dl id="addqxlist" style="width: 100%; float: left; margin-top: 16px;">
                                        <dd>
                                           <span>奶企管理</span>
                                           <em title="nqlb">奶企列表<input type="hidden" value="0" /></em>
                                           <em title="xznq">新增奶企<input type="hidden" value="0" /></em>
                                        </dd>
                                        <dd>
                                           <span>设备管理</span>
                                           <em title="qbsb">全部设备<input type="hidden" value="0" /></em>
                                           <em title="tjsb">添加设备<input type="hidden" value="0" /></em>
                                           <em title="sblb">设备类别<input type="hidden" value="0" /></em>
                                       </dd>
                                        <dd>
                                           <span>会员管理</span>
                                           <em title="hylb">会员列表<input type="hidden" value="0" /></em>
                                       </dd>
                                        <dd>
                                           <span>管理员管理</span>
                                           <em title="glygl">管理员管理<input type="hidden" value="0" /></em>
                                           <em title="jsgl">角色管理<input type="hidden" value="0" /></em>
                                       </dd>
                                        <dd>
                                           <span>产品管理</span>
                                           <em title="cplb">产品列表<input type="hidden" value="0" /></em>
                                           <em title="cplbie">产品类别<input type="hidden" value="0" /></em>
                                       </dd>
                                        <dd>
                                           <span>订单管理</span>
                                           <em title="dgjl">订购记录<input type="hidden" value="0" /></em>
                                           <em title="gmjl">购买记录<input type="hidden" value="0" /></em>
                                       </dd>
                                        <dd>
                                           <span>数据统计与查询</span>
                                           <em title="zhcx">综合查询<input type="hidden" value="0" /></em>
                                       </dd>
                                        <dd>
                                           <span>分析</span>
                                           <em title="cjdtt">成交动态图<input type="hidden" value="0" /></em>
                                       </dd>
                                        <dd>
                                           <span>数据大屏</span>
                                           <em title="sjdp">数据大屏<input type="hidden" value="0" /></em>
                                       </dd>
                                        <dd>
                                           <span>广告管理</span>
                                           <em title="spgl">视频管理<input type="hidden" value="0" /></em>
                                           <em title="jqtjsp">机器添加视频<input type="hidden" value="0" /></em>
                                       </dd>
                                    </dl>
                                </li>
                                <li>
                                    <label></label>
                                    <input type="button" value="确认" onclick="btnAdd()" class="btnok"/>
                                    <input type="button" value="取消" class="btnoff" onclick="divOff()"/>
                                </li>
                            </ul>
                        </div>
                    <div id="updataCaDiv" class="addDiv change">
                            <h4>修改角色</h4>
                            <ul>
                                <li>
                                    <label>角色名称</label>
                                    <input type="text" value="" id="adminName" placeholder="填写角色名称"/>
                                </li>
                                <li>
                                    <label style="width: 100%">角色权限</label>
                                    <dl id="updqxlist" style="width: 100%; float: left; margin-top: 16px;">
                                       <dd>
                                           <span>奶企管理</span>
                                           <em title="nqlb">奶企列表<input type="hidden" value="0" /></em>
                                           <em title="xznq">新增奶企<input type="hidden" value="0" /></em>
                                        </dd>
                                        <dd>
                                           <span>设备管理</span>
                                           <em title="qbsb">全部设备<input type="hidden" value="0" /></em>
                                           <em title="tjsb">添加设备<input type="hidden" value="0" /></em>
                                           <em title="sblb">设备类别<input type="hidden" value="0" /></em>
                                       </dd>
                                        <dd>
                                           <span>会员管理</span>
                                           <em title="hylb">会员列表<input type="hidden" value="0" /></em>
                                       </dd>
                                         <dd>
                                           <span>管理员管理</span>
                                           <em title="glygl">管理员管理<input type="hidden" value="0" /></em>
                                           <em title="jsgl">角色管理<input type="hidden" value="0" /></em>
                                       </dd>
                                        <dd>
                                           <span>产品管理</span>
                                           <em title="cplb">产品列表<input type="hidden" value="0" /></em>
                                           <em title="cplbie">产品类别<input type="hidden" value="0" /></em>
                                       </dd>
                                        <dd>
                                           <span>订单管理</span>
                                           <em title="dgjl">订购记录<input type="hidden" value="0" /></em>
                                           <em title="gmjl">购买记录<input type="hidden" value="0" /></em>
                                       </dd>
                                        <dd>
                                           <span>数据统计与查询</span>
                                           <em title="zhcx">综合查询<input type="hidden" value="0" /></em>
                                       </dd>
                                        <dd>
                                           <span>分析</span>
                                           <em title="cjdtt">成交动态图<input type="hidden" value="0" /></em>
                                       </dd>
                                        <dd>
                                           <span>数据大屏</span>
                                           <em title="sjdp">数据大屏<input type="hidden" value="0" /></em>
                                       </dd>
                                        <dd>
                                           <span>广告管理</span>
                                           <em title="spgl">视频管理<input type="hidden" value="0" /></em>
                                           <em title="jqtjsp">机器添加视频<input type="hidden" value="0" /></em>
                                       </dd>
                                    </dl>
                                </li>
                                <li>
                                    <label></label>
                                    <input type="button" value="确认" onclick="btnUpdate()" class="btnok"/>
                                    <input type="button" value="取消" class="btnoff" onclick="divOff()"/>
                                </li>
                            </ul>
                        </div>
                        <div class="popupbj"></div>
                    <section class="jiqlist">
                        <dl class="jiqlistTab">
                            <dd class="change ddcolor"><b>角色列表</b></dd>
                            <dd id="adddd">
                                <a id="addvideo" onclick="addadmin()">添加角色</a>
                            </dd>
                        </dl>
                        <ul class="memberlist" id="ull">
                            <li>
                                <label style="width: 20%">角色名称</label>
                                <label style="width: 60%">权限</label>
                                <label style="width: 20%">操作</label>
                            </li>
                            <li>
                                <span class="adminNmae" style="width: 20%">角色一</span>
                                <span class="adminPhone" style="width: 60%">光明集团</span>
                                <span style="width: 20%">
                                    <a onclick="adminUpd(this)">修改</a>
                                    <a onclick="adminDel()">删除</a>
                                </span>
                            </li>
                        </ul>
                    </section>
                </div>
            </div>
        </div>
         <input  id="qxName" runat="server" type="hidden"/>
        <input  id="roleID" runat="server" type="hidden"/>
    </form>
</body>
</html>
<script>
    function addadmin() {
        $("#addCaDiv").addClass("addDivshow");
        setTimeout(function () {
            $(".popupbj").fadeIn();
        }, 100);
    }
    function adminUpd(obj) {
        getQX($(obj).parent().parent().find("input").val());
        $("#roleID").val($(obj).parent().parent().find("input").val());
        var adminName = $(obj).parent().parent().find(".adminNmae");
        $("#qxName").val(adminName);
        $("#adminName").val(adminName.html());
        $("#updataCaDiv").addClass("addDivshow");
        setTimeout(function () {
            $(".popupbj").fadeIn();
        }, 100);
    }
    function divOff() {
        $(".popupbj").hide();
        $(".addDiv").removeClass("addDivshow");
    }
    $(function() {
        $("#li3").find("a").addClass("aborder");
        $("#addqxlist,#updqxlist").find("dd").find("em").click(function () {
            if ($(this).find("input").val() == "0") {
                $(this).addClass("fa fa-check emcolor");
                $(this).find("input").val("1");
            } else if ($(this).find("input").val() == "1") {
                $(this).removeClass("fa fa-check emcolor");
                $(this).find("input").val("0");
            }
        });
        sear();
    });
    function sear()
    {
        $("#ull").empty();
        $(" <li>"
           +" <label style='width: 50%'>角色名称</label>"
           +" <label style='width: 50%'>操作</label>"
           +" </li>").appendTo("#ull");
        $.ajax({
            type: "post",
            url: "rolelist.aspx/sear",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{}",
            success: function (data) {
                var serverdata = $.parseJSON(data.d);
                var serverdatalist = serverdata.length;
                for (var i = 0; i < serverdatalist; i++) {
                    $(" <li>"
                       + " <span class='adminNmae' style='width: 50%'>" + serverdata[i].name + "</span>"
                       
                       +"  <span style='width: 50%'>"
                       +"    <a onclick='adminUpd(this)'>修改</a>"
                       +"    <a onclick='adminDel()'>删除</a>"
                       + "  </span>"
                       + "<input type='hidden' value='" + serverdata[i].id + "'></input>"
                       +" </li>").appendTo("#ull");
                }
            }
        })
    }
    function btnAdd()
    {
        var qx = "";
        $("#addqxlist").find("em").each(function () {
            var str = $(this).find("input");
            var title = $(this).attr("title");
            var val = str.val();
            qx += title + '-' + val + ",";
        });
        $.ajax({
            type: "post",
            url: "rolelist.aspx/add",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{name:'" + $("#addname").val() + "',qx:'"+qx+"'}",
            success: function (data) {
                if (data.d == "1") {
                    alert("名称已经存在");
                } else if (data.d == "2") {
                    alert("添加失败");
                } else if (data.d == "3") {
                    alert("添加成功");
                }
                location.reload();
            }
        })
    }
    function btnUpdate()
    {
        var qx = "";
        $("#updataCaDiv").find("em").each(function () {
            var str = $(this).find("input");
            var title = $(this).attr("title");
            var val = str.val();
            qx += title + '-' + val + ",";
        });
        $.ajax({
            type: "post",
            url: "rolelist.aspx/updateQX",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{name:'" + $("#adminName").val() + "',qx:'" + qx + "',roleID:'" + $("#roleID").val() + "'}",
            success: function (data) {
                if (data.d == "1") {
                    alert("名称已经存在");
                } else if (data.d == "2") {
                    alert("添加成功");
                }
                location.reload();
            }
        })
    }
    function getQX(id)
    {
        $.ajax({
            type: "post",
            url: "rolelist.aspx/getQX",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{qxID:'" + id + "'}",
            success: function (data) {
                var emName = $("#updqxlist").find("dd").find("em");
                emName.removeClass("fa fa-check emcolor");
                var serverdata = $.parseJSON(data.d);
                var serverdatalist = serverdata.length;
                for (var i = 0; i < serverdatalist; i++) {
                    var title = serverdata[i].menuID;
                    var val = serverdata[i].flag;
                    
                    emName.each(function () {
                        if ($(this).attr("title") == title) {
                            $(this).find("input").val(val);
                            if (val == "1") {
                                $(this).addClass("fa fa-check emcolor");
                            }
                        }
                    });
                    
                }
            }
        })
    }
</script>
