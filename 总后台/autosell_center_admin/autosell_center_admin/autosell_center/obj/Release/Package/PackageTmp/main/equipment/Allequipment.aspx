<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Allequipment.aspx.cs" Inherits="autosell_center.main.equipment.Allequipment" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>全部设备-自动售卖终端中心控制系统</title>
    <link rel="shortcut icon" href="/main/public/images/favicon.ico" />
    <link href="/main/public/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/common.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/style.css" rel="stylesheet" type="text/css" />
    <script src="/main/public/script/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="/main/public/script/Titlelist.js" type="text/javascript"></script>
    <link href="../../scripts/skin/jedate.css" rel="stylesheet" type="text/css" />
    <script src="../../scripts/jedate.js" type="text/javascript"></script>
    <script src="../../scripts/jedate.min.js"></script>
    <script>
        window.onload = function () {
            jeDate({
                dateCell: "#start", //isinitVal:true,
                //format: "YYYY-MM-DD",
                isTime: false, //isClear:false,
                choose: function (val) { },
                minDate: "2014-09-19 00:00:00"
            })
            jeDate({
                dateCell: "#end",
                //isinitVal: true,
                isTime: true, //isClear:false,
                minDate: "2014-09-19 00:00:00"
            })
            jeDate({
                dateCell: "#yxqEnd",
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
            <div class="main_list">
                <div class="common_title">
                    <h4>
                        <i class="fa fa-plus"></i>
                        <span>设备管理</span>
                    </h4>
                </div>
                 <div class="change zfpopup">
                            <h4>机器设置</h4>
                            <ul>
                                <li>
                                    <h5>有效期至</h5>
                                    <label>
                                          <input name="act_stop_timeks" type="text" id="yxqEnd" runat="server" class="input" value="" placeholder="有效期截止时间" readonly="true" />
                                    </label>
                                </li>
                                <li class="fsopen">
                                    <h5>状态</h5>
                                    <span>
                                        <i class="change iopen"></i>
                                        <em class="change emopen"></em>
                                        <input type="hidden"   id="EquType"/>
                                    </span>
                                    <b></b>
                                </li>
                            </ul>
                            <dl>
                                <dd>
                                    <input type="button" value="确定" class="popupqdbtn" onclick="okBtn()" />
                                </dd>
                                <dd>
                                    <input type="button" value="取消" onclick="qxClick()" />
                                </dd>
                            </dl>
                        </div>
              <%--  <div class="change zfpopup">
                    <h4>摄像头地址设置</h4>
                    <ul>
                        <li>
                            <h5>摄像头地址</h5>
                            <label>
                                <input name="path" type="text" id="path" runat="server" class="input" value="" placeholder="摄像头地址" />
                            </label>
                        </li>
                        
                    </ul>
                    <dl>
                        <dd>
                            <input type="button" value="确定" class="popupqdbtn" onclick="okBtn()" />
                        </dd>
                        <dd>
                            <input type="button" value="取消" onclick="qxClick()" />
                        </dd>
                    </dl>
                </div>--%>
                   <div id="updataCaDiv" class="addDiv change">
                      <h4>视频实况<a style="float:right;color:#fff;font-size:1.8rem;margin-right:16px;" onclick="divOff()">×</a></h4>
                           <video src="" id="vid" controls="controls" autoplay="autoplay"></video>
                      
                   </div>
                <div class="common_main">
                    <div class="navlist">
                        <dl>
                            <dt>设备管理<em class="fa fa-cog"></em></dt>
                            <dd>
                                <a class="change acolor" href="#" onclick="qx_judge('qbsb')"><i class="change icolor fa fa-inbox"></i>全部设备</a>
                            </dd>
                            <dd>
                                <a class="change" href="#" onclick="qx_judge('tjsb')"><i class="change fa fa-plus-square"></i>添加设备</a>
                            </dd>
                        </dl>
                        <dl>
                            <dt>设备类别<em class="fa fa-cog"></em></dt>
                            <dd>
                                <a class="change" href="#" onclick="qx_judge('sblb')"><i class="change fa fa-server"></i>设备类别</a>
                            </dd>
                        </dl>
                    </div>
                    <section class="jiqlist">
                        <dl class="jiqlistTab">
                            <dd class="change ddcolor"><b>设备列表</b></dd>
                        </dl>
                        <ul class="jiqlistseach">
                            <li>
                                <input name="act_stop_timeks" type="text" id="start" runat="server" class="input" value="" placeholder="有效期开始时间" readonly="true" />
                            </li>
                            <li>
                                <input name="act_stop_timeks" type="text" id="end" runat="server" class="input" value="" placeholder="有效期截止时间" readonly="true" />
                            </li>
                            <li>
                                <input type="text" value="" placeholder="设备编号" id="bh" />
                            </li>
                            <li class="naiqBtn">
                                <input type="text" id="mulkWords" value="" placeholder="选择或填写奶企" />
                                <dl id="dll">
                                </dl>
                            </li>
                            <li>
                                <asp:DropDownList ID="typeDrop" runat="server">
                                    <asp:ListItem Value="0">全部分类</asp:ListItem>
                                    <asp:ListItem Value="2">正常</asp:ListItem>
                                    <asp:ListItem Value="1">禁用</asp:ListItem>
                                    <asp:ListItem Value="3">过期</asp:ListItem>
                                </asp:DropDownList>
                            </li>
                            <li>
                                <input type="button" value="查询" class="seachbtn" onclick="sear()" />
                            </li>
                        </ul>
                        <ul class="jiqlisttable" style="display: block;" id="ull">
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
          <input id="pageCurrentCount" runat="server" type="hidden" value="1"/>
         <input id="pageTotalCount" runat="server" type="hidden" value="1"/>
        <input id="mechineID" runat="server" type="hidden" />
    </form>
</body>
</html>
<script>
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
    function sear() {
        $.ajax({
            type: "post",
            url: "Allequipment.aspx/search",
            contentType: "application/json; charset=utf-8",
            data: "{startTime:'" + $("#start").val() + "',endTime:'" + $("#end").val() + "',bh:'" + $("#bh").val() + "',type:'" + $("#typeDrop").val() + "',name:'" + $("#mulkWords").val() + "',pageCurrentCount:'" + $("#pageCurrentCount").val() + "'}",
            dataType: "json",
            success: function (data) {
                $("#ull").empty();
                var count = data.d.split('@')[0];
                if (parseInt(count) >= 0) {
                    $("#pageSel").empty();
                    for (var k = 1; k <= parseInt(count) ; k++) {
                        $("<option value='" + k + "'>" + k + "</option>").appendTo("#pageSel");
                    }
                }
                var serverdata = $.parseJSON(data.d.split('@')[1]);
                $("#pageTotalCount").val(count);
                var serverdatalist = serverdata.length;
                console.log()
               
                for (var i = 0; i < serverdatalist; i++) {
                    var pause = "";
                   
                    if (serverdata[i].openStatus == "0")//营业中
                    {
                        pause = "营业中";
                    } else if (serverdata[i].openStatus == "1") {//停售中
                        pause = "停售中";
                    }
                    console.log("fdsfsfsdfffdsfsf"+pause);
                    $(" <li><dl>"
                        + "  <dd style='width:5%;padding-left:20px'>序号</dd>"
                        + "  <dd style='width:12.5%'>机器编号</dd>"
                        + " <dd style='width:12.5%'>有效期</dd>"
                        + " <dd style='width:10%'>设备类型</dd>"
                        + " <dd style='width:12.5%'>奶企</dd>"
                        + " <dd style='width:12.5%'>设备定位</dd>"
                        + " <dd style='width:10%'>状态</dd>"
                        + " <dd style='width:10%'>运行状态</dd>"
                        + " <dd style='width:12%'>操作</dd>"
                        + " </dl>"
                         + "<label style='width:5%;padding-left:20px'>" + serverdata[i].Row + "</label>"
                       + "<label style='width:12.5%'>" + serverdata[i].bh+ "</label>"
                        + "<label style='width:12.5%'>" + serverdata[i].validateTime.substring(0, 10) + "</label>"
                        + "<label style='width:10%'>" + serverdata[i].mechineType + "</label>"
                        + "<label style='width:12.5%'>" + serverdata[i].name + "</label>"
                        + "<label style='width:12.5%'>" + serverdata[i].addres + "</label>"
                        + "<label style='width:10%'><b>" + serverdata[i].sta + " " + serverdata[i].temperture + " ℃</b></label>"
                        + "<label style='width:10%'><b>" + serverdata[i].t + "</b>/" + pause + "</label>"
                        + "<label style='width:12%'>"
                        + "<a href='Material.aspx?mechineID="+serverdata[i].id+"'>料道设置</a>"
                         + "<a onclick='setUp(" + serverdata[i].id + ")'>设置</a>"
                         + "<a onclick='lookUp(" + serverdata[i].id + ")'>查看视频</a>"
              
                        + "</label></li>").appendTo("#ull");
                }
            }
        });
    };


    function ddList(val, id) {
        $("#mechineID").val(id);
        $(".naiqBtn").find("input").val(val);
        $("#dll").hide();
    };
     
    function divOff() {
        $(".popupbj").hide();
        $(".addDiv").removeClass("addDivshow");
    }
    $(function () {
        qx_judge('qbsb');
        $("#li1").find("a").addClass("aborder");
        $.ajax({
            type: "post",
            url: "Allequipment.aspx/getCompanyList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                var serverdata = $.parseJSON(data.d);
                var serverdatalist = serverdata.length;
                $(" <dt>企业列表</dt>").appendTo("#dll");
                for (var i = 0; i < serverdatalist; i++) {
                    $("<dd onclick=\"ddList('" + serverdata[i].name + "','" + serverdata[i].id + "')\">" + serverdata[i].name + "</dd>").appendTo("#dll");
                }
            }
        });
        $(".naiqBtn").find("input").focus(function () {
            $(this).parent().find("dl").show();
            $(this).parent().find("table").hide();
            this.select();
        });
        $(".jiqlisttable").click(function () {
            $(".naiqBtn").find("dl").hide();
        });
        $(".naiqBtn").find("dd").click(function () {

        });

        $(".naiqBtn").find("input").keydown(function () {
            $(this).parent().find("dl").hide();
            $(this).parent().find("table").show();
        });

        $(".naiqBtn").find("input").keyup(function () {
            var $mulkWordsInput = $("#mulkWords").eq(0);
            var inputCon = $mulkWordsInput.val();
            var thName = $mulkWordsInput.parent().find("th").eq(0);
            if (inputCon) {
                thName.html("按'" + inputCon + "'搜索");
                $(".naiqBtn").find("td").parent("tr").hide();
                $mulkWordsInput.parent().find("td").each(function () {
                    if ($(this).html().indexOf(inputCon) != -1) {
                        $(this).parent().show();
                    }
                });
            } else {
                thName.html("奶企名称/全拼/首字母");
            }
        });
        $(".naiqBtn").find("input").bind('input propertychange', function () {
            $(".naiqBtn").find("input").val($(this).val());
        });
        $(".naiqBtn").find("tr").click(function () {
            var $tdName = $(this).find("td").eq(0).html();
            var $thTitle = $(this).find("th").length > 0;
            if ($thTitle === false) {
                $(".naiqBtn").find("input").val($tdName);
                $(".naiqBtn").find("table").hide();
            }
        });
        var fsopenB = $(".fsopen").find("b");

        $(".fsopen").find("span").click(function () {
            $(this).find("em").toggleClass("emopen");
            $(this).find("i").toggleClass("iopen");
            if ($(this).find("em").hasClass("emopen")) {
                $(this).find("input").val("2");
                fsopenB.html("已开启");
            } else {
                $(this).find("input").val("1");
                fsopenB.html("已关闭");
            }
        });
        sear();
    });
    function setUp(id) {
        ////根据id从后台在取有效期和状态
        //$("#mechineID").val(id);
        //$.ajax({
        //    type: "post",
        //    url: "Allequipment.aspx/getPath",
        //    contentType: "application/json; charset=utf-8",
        //    data: "{id:'" + id + "'}",
        //    dataType: "json",
        //    success: function (data) {
        //        $("#path").val(data.d);
        //    }
        //});
        //$(".popupbj").fadeIn();
        //$(".zfpopup").addClass("zfpopup_on");
        //根据id从后台在取有效期和状态
        $("#mechineID").val(id);
        $.ajax({
            type: "post",
            url: "devicelist.aspx/getInfo",
            contentType: "application/json; charset=utf-8",
            data: "{id:'" + id + "'}",
            dataType: "json",
            success: function (data) {
                $("#yxqEnd").val(data.d.split('|')[0]);
                $("#EquType").val(data.d.split('|')[1]);

                var $emOpen = $("#EquType");
                var fsopenB = $(".fsopen").find("b");

                if ($emOpen.val() == "2") {
                    fsopenB.html("已开启");
                    $(".fsopen").find("i").addClass("iopen");
                    $(".fsopen").find("em").addClass("emopen");
                } else {
                    fsopenB.html("已关闭");
                    $(".fsopen").find("i").removeClass("iopen");
                    $(".fsopen").find("em").removeClass("emopen");
                }
            }
        })
        $(".popupbj").fadeIn();
        $(".zfpopup").addClass("zfpopup_on");
    };
    function lookUp(id) {
        //根据id从后台在取有效期和状态
         
        $.ajax({
            type: "post",
            url: "Allequipment.aspx/getPath",
            contentType: "application/json; charset=utf-8",
            data: "{id:'" + id + "'}",
            dataType: "json",
            success: function (data) {
                if (data.d != "") {
                    $("#vid").attr("src", data.d);
                } else {
                    alert("找不到视频资源");
                }
            }
        });
        $("#updataCaDiv").addClass("addDivshow");
 
        setTimeout(function () {
            $(".popupbj").fadeIn();
        }, 100);

    };
    function qxClick() {
        $(".zfpopup").removeClass("zfpopup_on");
        setTimeout(function () { $(".popupbj").hide(); }, 300);
    }
    function okBtn() {
        var equType = $("#EquType").val();
        
        if (equType == '' || equType == undefined || equType == null) {
            alert("请选择状态！");
            return;
        }
        $.ajax({
            url: "../../ashx/asm.ashx",
            type: 'post',
            dataType: 'json',
            timeout: 10000,
            data: {
                action: "saveEquiSet",
                yxq: $("#yxqEnd").val(),
                type: $("#EquType").val(),
                equID: $("#mechineID").val()
            },
            success: function (resultData) {
                if (resultData == "1") {
                    alert("修改成功");
                    qxClick();
                    sear();
                } else if (resultData == "2") {
                    alert("修改失败");
                }
            }
        })
    }
    //function okBtn() {
    //    if ($("#path").val()=="")
    //    {
    //        alert("请输入有效的地址");
    //        return;
    //    }
    //    $.ajax({
    //        type: "post",
    //        url: "Allequipment.aspx/addPath",
    //        contentType: "application/json; charset=utf-8",
    //        data: "{id:'" + $("#mechineID").val() + "',path:'" + $("#path").val() + "'}",
    //        dataType: "json",
    //        success: function (data) {
    //            if(data.d=="1")
    //            {
    //                alert("保存成功");
    //                window.location.reload();
    //            }else if(data.d=="2")
    //            {
    //                alert("保存失败");
    //            }
    //        }
    //    });
    //}
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
                        //location.href = "Allequipment.aspx";
                    }
                    if (menuID == 'qbsb') {//设备管理
                        //location.href = "/main/equipment/Allequipment.aspx";
                    }
                    if (menuID == 'hylb') {//会员管理
                        location.href = "memberlist.aspx";
                    }
                    if (menuID == 'dgjl') {//订购记录
                        location.href = "dglist.aspx";
                    }
                    if (menuID == 'gmjl') {//购买记录
                        location.href = "orderlist.aspx";
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
