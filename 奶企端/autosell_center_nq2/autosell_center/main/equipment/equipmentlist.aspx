<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="equipmentlist.aspx.cs" Inherits="autosell_center.main.equipment.equipmentlist" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link rel="shortcut icon" href="/main/public/images/favicon.ico" />
    <link href="/main/public/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/common.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/style.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="../bootstrapSelect/css/bootstrap.css"/>
    <link rel="stylesheet" type="text/css" href="../bootstrapSelect/dist/css/bootstrap-select.css"/>
    <script src="../bootstrapSelect/dist/js/jquery.js"></script>
    <script src="../bootstrapSelect/dist/js/bootstrap.min.js"></script> 
    <script src="../bootstrapSelect/dist/js/bootstrap-select.min.js" charset="gb2312"></script>
    <script src="/main/public/script/Titlelist.js" type="text/javascript"></script>
        <link  href="../../scripts/skin/jedate.css" rel="stylesheet" type="text/css"/>
    <script src="../../scripts/jedate.js" type="text/javascript"></script>
    <script src="../../scripts/jedate.min.js"></script>
    <script charset="utf-8" src="http://map.qq.com/api/js?v=2.exp" type="text/javascript"></script>
	<script src="http://open.map.qq.com/apifiles/2/4/79/main.js" type="text/javascript"></script>
    <script src="https://open.ys7.com/sdk/js/1.3/ezuikit.js"></script>
    <script charset="utf-8" src="http://map.qq.com/api/js?v=2.exp&key=WJZBZ-Q3PRP-VFBDS-LEF5P-AOGW5-QUFI5"></script>
     <script type="text/javascript">
            window.onload = function () {
                jeDate({
                    dateCell: "#updateTime", //isinitVal:true,
                    //format: "YYYY-MM-DD",
                    isTime: true, //isClear:false,
                    choose: function (val) { },
                    minDate: "2014-09-19 00:00:00"
                });
              
            }
    </script>
	<style>
         #address{height: 31px;padding: 0 10px;}
         .map-seach{background: #50a4ec;padding: 5px 20px;color: #fff;display: inline-block;}
         .map-seach:active{background: rgba(80, 164, 236, 0.4);}
         .smnoprint,.smnoprint{display:none}
         .jiqlisttable li .jiqzt2{width:10%}
         .jiqlisttable li .jiqcz2{width:16%;}
	    .csssl {
	        overflow:hidden;
            text-overflow:ellipsis;
            white-space:nowrap

        }
         .setupPSY .bootstrap-select {
            width: 100% !important;
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
                        <span>设备管理</span>
                    </h4>
                </div>
                <div class="common_main">
                    <div class="navlist">
                        <dl>
                            <dt>设备管理<em class="fa fa-cog"></em></dt>
                            <dd>
                                <a class="change acolor" href="#" onclick="qx_judge('sblb')"><i class="change icolor fa fa-inbox"></i>设备列表</a>
                            </dd>
                        </dl>
                    </div>
                    <section class="jiqlist">
                         <div class="change zfpopup" id="setUpSoftUpdate">
                        <h4>软件更新设置</h4>
                        <ul>
                           
                            <li>
                                <h5>新版本</h5>
                                <label>
                                    <input name="path" type="text" id="newsoftversion" runat="server" class="input" value="" placeholder="新版本" />
                                </label>
                            </li>
                            <li>
                                <h5>新版本号</h5>
                                <label>
                                    <input name="path" type="text" id="newVerCode" runat="server" class="input" value="" placeholder="新版本号" />
                                </label>
                            </li>
                              <li>
                                <h5>下载地址</h5>
                                <label>
                                    <input name="path" type="text" id="downUrl" runat="server" class="input" value="" placeholder="下载地址" />
                                </label>
                            </li>
                             <li>
                                <h5>更新时间</h5>
                                <label>
                                    <input name="act_stop_timeks"  autocomplete="off" type="text" id="updateTime" runat="server" class="input" value="" placeholder="更新时间" />
                                </label>
                            </li>
                        </ul>
                        <dl>
                            <dd>
                                <input type="button" value="确定" class="popupqdbtn" onclick="okSoftUpdateBtn()" />
                            </dd>
                            <dd>
                                <input type="button" value="取消" onclick="qxClick()" />
                            </dd>
                        </dl>
                    </div>
                          <div class="change zfpopup" id="setPath">
                        <h4>设置</h4>
                        <ul>
                            <li>
                                <h5>摄像头地址</h5>
                                <label>
                                    <input name="path" type="text" id="path" runat="server" class="input" value="" placeholder="摄像头地址" />
                                </label>
                            </li>
                            <li>
                                <h5>温度提醒设置</h5>
                                <label>
                                    <input name="path" type="text" id="setTem" runat="server" class="input" value="" placeholder="5-10  温度小于5到大于10提醒" />
                                </label>
                            </li>
                             <li style="display:none">
                                <h5>支付宝红包设置</h5>
                                <label>
                                  <asp:DropDownList ID="hblist" runat="server"></asp:DropDownList>
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
                    </div>
                   <div id="updataCaDiv" class="addDiv change">
                      <h4>视频实况<a style="float:right;color:#fff;font-size:1.8rem;margin-right:16px;" onclick="divOff()">×</a></h4>
                      <video id="myPlayer" poster="" controls playsInline webkit-playsinline autoplay>
                        <source src="" type="" id="vid"/>
                      <%--  <source src="http://hls.open.ys7.com/openlive/f01018a141094b7fa138b9d0b856507b.m3u8" type="application/x-mpegURL" />--%>

                    </video>
                     <%--  <iframe id="vid" src="" style=""></iframe>--%>
                   </div>
                        <div id="adminpopup" class="change zfpopup">
                            <h4>配送员设置</h4>
                            <ul>
                                <li>
                                    <h5>设置配送员</h5>
                                    <label>
                                        <asp:DropDownList ID="sel1" runat="server"></asp:DropDownList>
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
                          <div id="dlspopup" class="change zfpopup">
                            <h4>管理员设置</h4>
                            <ul>
                                <li>
                                    <h5>管理员设置</h5>
                                    <label>
                                        <asp:DropDownList ID="DropDownList1" runat="server"></asp:DropDownList>

                                    </label>
                                </li>
                            </ul>
                            <dl>
                                <dd>
                                    <input type="button" value="确定" class="popupqdbtn" onclick="okDls()" />
                                </dd>
                                <dd>
                                    <input type="button" value="取消" onclick="qxClick()" />
                                </dd>
                            </dl>
                        </div>
                        <div id="shebeipopup" class="change zfpopup">
                            <h4>设备定位</h4>
                            <table>
                                <tbody>
                                    <tr>
                                        <td>
                                            <div id="container" style="width:100%;height:100%;"></div>
                                        </td>
                                        <td>
                                            <div id="r-result">
                                                请输入: 
                                                <button type="button"  id="mapseacrh" class="btn btn-primary">搜索</button>
                                                <input type="text" id="address" class="form-control" value=""/>
                                            </div>
                                            <div id="searchResultPanel" style="border: 1px solid #C0C0C0; width: 150px; height: auto; display: none;"></div>
                                            <div>机器名称:<input type="text" id="name" size="20" value=""  placeholder="例如:中天商务楼15号楼1号机器"/></div>
                                            <div>经度:<input type="text" class="form-control" id="longitude" name="longitude" value=""/></div>
                                            <div>纬度: <input type="text" class="form-control" id="latitude" name="latitude" value=""/></div>
                                            <dl>
                                                <dd>
                                                    <input type="button" value="确定" class="popupqdbtn" onclick="setLocation()" />
                                                </dd>
                                                <dd>
                                                    <input type="button" value="取消" onclick="qxClick()" />
                                                </dd>
                                            </dl>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <div class="popupbj"></div>
                        <dl class="jiqlistTab">
                            <dd class="change ddcolor"><b>设备列表</b></dd>
                        </dl>
                        <ul class="jiqlistseach">
                            <li>
                                <select id="mechineList"  class="selectpicker" multiple data-live-search="true" data-live-search-placeholder="请输入搜索" data-actions-box="true">
                               
                                </select> 
                            </li>
                            <li class="naiqBtn">
                                <input type="text" id="mulkWords" value="" placeholder="设备管理员" />
                            </li>
                            <li>
                                <input type="button" value="查询" class="seachbtn" onclick="sear()" />
                            </li>
                            <li>
                                <input type="button" value="导出Excel" class="seachbtn" onclick="exporttotalKC()" />
                            </li>
                             <li>
                                <input type="button" value="软件更新" class="seachbtn" onclick="setUpSoftUpdate()" />
                            </li>
                        </ul>
                        <ul class="jiqlisttable" style="display: block;" id="ull">
                        </ul>
                      
                    </section>
                </div>
            </div>
        </div>
        <input id="mechineID" runat="server" type="hidden" />
        <input id="companyId" runat="server" type="hidden" />
        <input id="sjX" runat="server" type="hidden"/>
        <input id="sjY" runat="server" type="hidden"/>
         <input id="agentID" runat="server" type="hidden"/>
         <input id="opera_id" runat="server" type="hidden"/>
    </form>
</body>
</html>
<script>
    var player = new EZUIPlayer('myPlayer');
    player.on('error', function () {
        console.log('error');
    });
    player.on('play', function () {
        console.log('play');
    });
    player.on('pause', function () {
        console.log('pause');
    });
    function divOff() {
        $(".popupbj").hide();
        $(".addDiv").removeClass("addDivshow");
        window.location.reload();
    }
    function judge() {
        $.ajax({
            type: "post",
            url: "equipmentlist.aspx/judge",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{operaID:'" + $("#agentID").val() + "',menuID:'sblb'}",
            success: function (data) {
                if (data.d.code == "500") {
                    $(".jiqlist").empty();
                    $(".jiqlist").html("<p class='noquanxian'>当前没有权限</p>").css({ "background": "#ddd" });
                }
            }
        })
    }
    function exporttotalKC()
    {
        if ($("#mechineList").val()=="")
        {
            alert("请选择机器");
            return;
        }
        window.location.href = "../../api/ExportExcel.ashx?action=exporttotalKC&mechineID=" + $("#mechineList").val();
    }
    function getMechineList() {
        $.ajax({
            type: "post",
            url: "equipmentlist.aspx/getMechineList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{companyID:'" + $("#companyId").val() + "'}",
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
       
        $("#li4").find("a").addClass("aborder");
        getMechineList();
        sear();
        
       
    });

    $(function () {
        judge()
        $(".naiqBtn").find("input").focus(function () {
            $(this).parent().find("dl").show();
            $(this).parent().find("table").hide();
            this.select();
        });
        $(".jiqlisttable").click(function () {
            $(".naiqBtn").find("dl").hide();
        });

        $(".naiqBtn").find("dd").click(function () {
            var $ddName = $(this).html();
            $(".naiqBtn").find("input").val($ddName);
            $(this).parent("dl").hide();
        });
        $(".naiqBtn").find("input").keydown(function () {
            $(this).parent().find("dl").hide();
            $(this).parent().find("table").show();
        });
        $(".naiqBtn").find("input").keyup(function () {
            var $mulkWordsInput = $("#mulkWords").eq(0);
            var inputCon = $mulkWordsInput.val();
            var thName = $mulkWordsInput.parent().find("th").eq(0);
            //var $tdArry = $mulkWordsInput.parent().find("td");
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

        })
        $(".naiqBtn").find("tr").click(function () {
            var $tdName = $(this).find("td").eq(0).html();
            var $thTitle = $(this).find("th").length > 0;
            if ($thTitle === false) {
                $(".naiqBtn").find("input").val($tdName);
                $(".naiqBtn").find("table").hide();
            }
        });
    });
    function sear() {
        $.ajax({
            type: "post",
            url: "equipmentlist.aspx/search",
            contentType: "application/json; charset=utf-8",
            data: "{mechineID:'" + $("#mechineList").val() + "',opera:'" + $("#mulkWords").val() + "',companyID:'" + $("#companyId").val() + "',agentID:'" + $("#agentID").val() + "'}",
            dataType: "json",
            success: function (data) {
                $("#ull").empty();
                var serverdata = $.parseJSON(data.d);
                var serverdatalist = serverdata.length;
                for (var i = 0; i < serverdatalist; i++) {
                    var des = "暂无设置";
                    if (serverdata[i].hbdes != "0" && serverdata[i].hbdes!=null)
                    {
                        des = serverdata[i].hbdes;
                    }
                    var pause="";
                    if (serverdata[i].openStatus == "0")//营业中
                    {
                        pause = "营业中";
                    } else if (serverdata[i].openStatus == "1") {//停售中
                        pause = "停售中";
                    }
                    $("<li>"
                            + " <dl>"
                            + "     <dd style='width:19%;padding-left:30px'>机器</dd>"
                            + "     <dd style='width:8%'>有效期</dd>"
                            + "     <dd style='width:7%'>当前配送员</dd>"
                              + "   <dd style='width:10%'>管理员</dd>"
                            + "     <dd style='width:15%'>设备定位</dd>"
                            + "     <dd style='width:7%'>当前状态</dd>"
                            + "     <dd style='width:8%'>支付宝红包</dd>"
                            + "     <dd style='width:6%'>实时温度</dd>"
                            + "     <dd style='width:20%;'>操作</dd>"
                            + " </dl>"
                            + " <label style='width:19%;padding-left:30px'>"
                            + "     <span>" + serverdata[i].bh + "</span>"
                            + "     <em>" + serverdata[i].mechineName + "</em>"
                            + " </label>"
                            + " <label style='width:8%'>" + serverdata[i].validateTime.replace('T', ' ').substring(0,10) + "</label>"
                            + " <label style='width:7%'>" + serverdata[i].operaName + "<a onclick='adminSet(" + serverdata[i].id + ")'>设置</a></label>"
                              + " <label style='width:10%'>" + serverdata[i].dlsName + "<a onclick='SetDls(" + serverdata[i].id + ")'>设置</a></label>"
                            + " <label style='width:15%' ><label style='width:80%' class='csssl'>" + serverdata[i].addres + "</label><a onclick='SetAddres(" + serverdata[i].id + ")'>设置</a></label>"
                            + " <label style='width:7%'>" + serverdata[i].sta + "</label>"
                            + " <label style='width:8%'>" + des + "</label>"
                            + " <label style='width:6%'><a href='temperaturelist.aspx?mechineID=" + serverdata[i].id + "'> <b>" + serverdata[i].temperture + "</b></a></label>"
                          
                            + "<label style='width:20%'>"
                            //+ "     <a href='equipmentclass.aspx?mechineID=" + serverdata[i].id + "'>料道类型</a>"
                            + "     <a href='kcList.aspx?mechineID=" + serverdata[i].id + "' >库存</a>"
                            + "     <a href='orderMe.aspx?mechineID=" + serverdata[i].id + "'>订单</a>"
                            + "     <a onclick='setUp(" + serverdata[i].id + ")'>设置</a>"
                             // + "     <a onclick='setUpSoftUpdate(" + serverdata[i].id + ")'>版本设置</a>"
                            + "     <a onclick='lookUp(" + serverdata[i].id + ")'>查看视频</a>"
                            + "     <a onclick='pauseBtn(" + serverdata[i].id + ")'>"+pause+"</a>"
                            + " </label>"
                            + "</li>").appendTo("#ull");
                }
            }
        });
    };
    function pauseBtn(id)
    {
        $.ajax({
            type: "post",
            url: "equipmentlist.aspx/pause",
            contentType: "application/json; charset=utf-8",
            data: "{id:'" + id + "'}",
            dataType: "json",
            success: function (data) {
                if (data.d.code == "200") {
                    window.location.reload();
                } else {
                    alert(data.d.msg);
                }
            }
        });
    }
    function SetAddres(id) {
        //location.href = "map.aspx";
         
        $(".popupbj").fadeIn();
        $("#shebeipopup").addClass("zfpopup_on");
        $("#mechineID").val(id);
       
        //查询该机器设置的经纬度
        $.ajax({
            type: "post",
            url: "equipmentlist.aspx/searchJWD",
            contentType: "application/json; charset=utf-8",
            data: "{mechineID:'" + id + "'}",
            dataType: "json",
            success: function (data) {
                var serverdata = $.parseJSON(data.d);
                var serverdatalist = serverdata.length;
                if (serverdatalist>0)
                {
                    $("#address").val(serverdata[0].addres);
                    $("#name").val(serverdata[0].mechineName);
                    $("#longitude").val(serverdata[0].zdX);
                    $("#latitude").val(serverdata[0].zdY);
                    showMap();
                    $("#sjX").val(serverdata[0].sjX);
                    $("#sjY").val(serverdata[0].sjY);
                      
                }
            }
        });
         
    }
    function setUpSoftUpdate() {
     
        //根据id从后台在取有效期和状态
        
        $(".popupbj").fadeIn();
        $("#setUpSoftUpdate").addClass("zfpopup_on");

    };
    function setUp(id) {
        //根据id从后台在取有效期和状态
        $("#mechineID").val(id);
        $.ajax({
            type: "post",
            url: "equipmentlist.aspx/getPath",
            contentType: "application/json; charset=utf-8",
            data: "{id:'" + id + "'}",
            dataType: "json",
            success: function (data) {
                $("#path").val(data.d.split('||')[0]);
                $("#setTem").val(data.d.split('||')[1]);
                $("#hblist").val(data.d.split('||')[2]);
            }
        });
        $(".popupbj").fadeIn();
        $("#setPath").addClass("zfpopup_on");

    };
    function okBtn() {
        if ($("#path").val() == "") {
            alert("请输入有效的地址");
            return;
        }
        $.ajax({
            type: "post",
            url: "equipmentlist.aspx/addPath",
            contentType: "application/json; charset=utf-8",
            data: "{id:'" + $("#mechineID").val() + "',path:'" + $("#path").val() + "',setTem:'" + $("#setTem").val() + "',hbid:'" + $("#hblist").val() + "'}",
            dataType: "json",
            success: function (data) {
                if (data.d == "1") {
                    alert("保存成功");
                    window.location.reload();
                } else if (data.d == "2") {
                    alert("保存失败");
                }
            }
        });
    }
    
    function okSoftUpdateBtn() {
        $.ajax({
            type: "post",
            url: "equipmentlist.aspx/addSoftUpdateBtn",
            contentType: "application/json; charset=utf-8",
            data: "{mechineID:'" +$("#mechineList").val()+ "',newsoftversion:'" + $("#newsoftversion").val() + "',newVerCode:'" + $("#newVerCode").val() + "',downUrl:'" + $("#downUrl").val() + "',updateTime:'" + $("#updateTime").val() + "'}",
            dataType: "json",
            success: function (data) {
                if (data.d == "1") {
                    alert("保存成功");
                    window.location.reload();
                } else if (data.d == "2") {
                    alert("保存失败");
                }
            }
        });
    }
    function lookUp(id) {
        //根据id从后台在取有效期和状态
        
        $.ajax({
            type: "post",
            url: "equipmentlist.aspx/getPath",
            contentType: "application/json; charset=utf-8",
            data: "{id:'" + id + "'}",
            dataType: "json",
            success: function (data) {
                if (data.d != "") {
                    $("#vid").attr("src", data.d);
                    var player = new EZUIPlayer('myPlayer');
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
    function adminSet(id) {
        $("#mechineID").val(id);
        $(".popupbj").fadeIn();
        $("#adminpopup").addClass("zfpopup_on");
    }
    function SetDls(id) {
          $.ajax({
            type: "post",
            url: "equipmentlist.aspx/yz",
            contentType: "application/json; charset=utf-8",
            data: "{operaID:'" + $("#opera_id").val() + "'}",
            dataType: "json",
            success: function (data) {
                if (data.d == "2") {
                    $("#mechineID").val(id);
                    $(".popupbj").fadeIn();
                    $("#dlspopup").addClass("zfpopup_on");
                } else {
                    alert("没有修改管理员权限");
                }
            }
        });
       
    }
    function okDls()
    {
        $.ajax({
            type: "post",
            url: "equipmentlist.aspx/okDls",
            contentType: "application/json; charset=utf-8",
            data: "{mechineID:'" + $("#mechineID").val() + "',operaID:'" + $("#DropDownList1").val() + "'}",
            dataType: "json",
            success: function (data) {
                if (data.d == "1") {
                    alert("设置成功");
                } else if (data.d == "2") {
                    alert("设置失败");
                }
                location.reload();
            }
        });
    }
    function qxClick() {
        $(".tangram-suggestion-main").hide();
        $(".popupbj").hide();
        $(".zfpopup").removeClass("zfpopup_on");
    }
    function ok() {
        $.ajax({
            type: "post",
            url: "equipmentlist.aspx/setAdmin",
            contentType: "application/json; charset=utf-8",
            data: "{mechineID:'" + $("#mechineID").val() + "',operaID:'" + $("#sel1").val() + "'}",
            dataType: "json",
            success: function (data) {
                if (data.d == "1") {
                    alert("设置成功");
                } else if (data.d == "2") {
                    alert("设置失败");
                }
                location.reload();
            }
        });
    }
    function setLocation()
    {
        if ($("#suggestId").val()=="")
        {
            alert("请输入机器地址");
            return;
        }
        if ($("#longitude").val() == "") {
            alert("请在地图上选择机器的位置");
            return;
        }
        if ($("#latitude").val() == "") {
            alert("请在地图上选择机器的位置");
            return;
        }
        if ($("#name").val() == "") {
            alert("机器名称不能为空");
            return;
        }
       
        var address, pro, city, distinct;
        $.ajax({
        type : 'get',
        url : 'http://apis.map.qq.com/ws/geocoder/v1',
        dataType:'jsonp',
        data : {
            key: "WJZBZ-Q3PRP-VFBDS-LEF5P-AOGW5-QUFI5",//开发密钥
            location: "" + $("#latitude").val() + "," + $("#longitude").val() + "",//位置坐标
            //location: "34.86445745937386,117.5678367074579",//位置坐标
            get_poi:"1",//是否返回周边POI列表：1.返回；0不返回(默认)
            coord_type:"1",//输入的locations的坐标类型,1 GPS坐标
            parameter:{"scene_type":"tohome","poi_num":20},//附加控制功能
            output:"jsonp"
            },                 
        success: function (data, textStatus) {
            
            if (data.status == 0) {
                 address = data.result.formatted_addresses.recommend;
                 pro=data.result.address_component.province;
                 city=data.result.address_component.city;
                 distinct=data.result.address_component.district;
                 $.ajax({
                        type: "post",
                        url: "equipmentlist.aspx/setLocation",
                        contentType: "application/json; charset=utf-8",
                        data: "{mechineID:'" + $("#mechineID").val() + "',jd:'" + $("#longitude").val() + "',wd:'" + $("#latitude").val() + "',address:'" + $("#address").val() + "',name:'" + $("#name").val() + "',province:'" + pro + "',city:'" + city + "',distinct:'" + distinct + "',addre:'" + address + "'}",
                        dataType: "json",
                        success: function (data) {
                            if (data.d == "1") {
                                alert("设置成功");
                            } else if (data.d == "2") {
                                alert("设置失败");
                            }
                            location.reload();
                        }
                    });
                  //alert("省："+pro+"市："+city+"区:"+distinct);
            }else {
                alert("1系统错误，请联系管理员！")
            }
        },
        error : function() {
            alert("系统错误，请联系管理员！")
        }
        });
      
        
    }
    //function getAddres(lng,lat)
    //{
       
    //    var address, pro, city, distinct;
    //    $.ajax({
    //        type: 'get',
    //        url: 'http://apis.map.qq.com/ws/geocoder/v1',
    //        dataType: 'jsonp',
    //        data: {
    //            key: "EPQBZ-XS4CJ-2C3FM-KMPYV-RI2EZ-4IBS3",//开发密钥
    //            location: ""+lat+","+lng+"",//位置坐标
    //            get_poi: "1",//是否返回周边POI列表：1.返回；0不返回(默认)
    //            coord_type: "1",//输入的locations的坐标类型,1 GPS坐标
    //            parameter: { "scene_type": "tohome", "poi_num": 20 },//附加控制功能
    //            output: "jsonp"
    //        },
    //        success: function (data, textStatus) {
    //            if (data.status == 0) {
    //                address = data.result.formatted_addresses.recommend;
    //                alert(address);
    //                pro = data.result.address_component.province;
    //                city = data.result.address_component.city;
    //                distinct = data.result.address_component.district;
    //            } else {
    //                alert("系统错误，请联系管理员！")
    //            }
    //        },
    //        error: function () {
    //            alert("系统错误，请联系管理员！")
    //        }
    //          alert("省="+pro+"市="+city+"区="+distinct); 
    //    });
    //}
</script>

<script type="text/javascript">
    function showMap()
    {
        var geocoder, citylocation, map, marker = null;
        var markersArray = [];
        var x = $("#longitude").val();
        var y = $("#latitude").val();
        if (x == "")
        {
            x = 39.916527;
        }
        if(y=="")
        {
            y == 116.397128;
        }
        var center = new qq.maps.LatLng(y, x);
        map = new qq.maps.Map(document.getElementById('container'), {
            center: center,
            zoom: 18
        });
        geocoder = new qq.maps.Geocoder({
            complete: function (result) {
                map.setCenter(result.detail.location);
                var marker = new qq.maps.Marker({
                    map: map,
                    position: result.detail.location
                });
            }
        });
        marker = new qq.maps.Marker({
            position: new qq.maps.LatLng(y, x),
            map: map
        });
        //获取城市列表接口设置中心点
        if (y == '' || x == '') {
            citylocation = new qq.maps.CityService({
                complete: function (result) {
                    map.setCenter(result.detail.latLng);
                }
            });
            //调用searchLocalCity();方法    根据用户IP查询城市信息。
            citylocation.searchLocalCity();
        }
        //绑定单击事件添加参数
        qq.maps.event.addListener(map, 'click', function (event) {
            //alert('您点击的位置为: [' + event.latLng.getLat() + ', ' + event.latLng.getLng() + ']');
            $("#longitude").val(event.latLng.getLng());
            $("#latitude").val(event.latLng.getLat());
            //getAddres(event.latLng.getLng(), event.latLng.getLat());
            qq.maps.event.addListener(map, 'click', function (event) {
                marker.setMap(null);
                $("#longitude").attr("value", "");
                $("#longitude").attr("value", event.latLng.getLng());
                $("#latitude").attr("value", "");
                $("#latitude").attr("value", event.latLng.getLat());
                marker = new qq.maps.Marker({
                    position: new qq.maps.LatLng(event.latLng.getLat(), event.latLng.getLng()),
                    map: map
                });
            });
        });
        geocoder = new qq.maps.Geocoder({
            complete: function (result) {
                marker.setMap(null);
                map.setCenter(result.detail.location);
                marker = new qq.maps.Marker({
                    map: map,
                    position: result.detail.location
                });
                $("#latitude").attr("value", marker.position.lat);
                $("#longitude").attr("value", marker.position.lng);
            }
        });
        $("#mapseacrh").click(function () {
            var address = $("#address").val();
            //通过getLocation();方法获取位置信息值
            geocoder.getLocation(address);
        });
    }
    $(function () {
        var geocoder, citylocation, map, marker = null;
        var markersArray = [];
        var x = $("#longitude").val();
        var y = $("#latitude").val();
        var center = new qq.maps.LatLng(y, x);
        map = new qq.maps.Map(document.getElementById('container'), {
            center: center,
            zoom: 18
        });
        geocoder = new qq.maps.Geocoder({
            complete: function (result) {
                map.setCenter(result.detail.location);
                var marker = new qq.maps.Marker({
                    map: map,
                    position: result.detail.location
                });
            }
        });
        marker = new qq.maps.Marker({
            position: new qq.maps.LatLng(y, x),
            map: map
        });
        //获取城市列表接口设置中心点
        if (y == '' || x == '') {
            citylocation = new qq.maps.CityService({
                complete: function (result) {
                    
                    map.setCenter(result.detail.latLng);
                }
            });
            //调用searchLocalCity();方法    根据用户IP查询城市信息。
            citylocation.searchLocalCity();
        }

        //绑定单击事件添加参数
        qq.maps.event.addListener(map, 'click', function (event) {
            //alert('您点击的位置为: [' + event.latLng.getLat() + ', ' + event.latLng.getLng() + ']');
            $("#longitude").val(event.latLng.getLng());
            $("#latitude").val(event.latLng.getLat());
            qq.maps.event.addListener(map, 'click', function (event) {
                marker.setMap(null);
                $("#longitude").attr("value", "");
                $("#longitude").attr("value", event.latLng.getLng());
                $("#latitude").attr("value", "");
                $("#latitude").attr("value", event.latLng.getLat());
                marker = new qq.maps.Marker({
                    position: new qq.maps.LatLng(event.latLng.getLat(), event.latLng.getLng()),
                    map: map
                });
                codeLatLng(event.latLng.getLat(), event.latLng.getLng());//地址逆解析
            });
        });
        geocoder = new qq.maps.Geocoder({
            complete: function (result) {
                marker.setMap(null);
                map.setCenter(result.detail.location);
                marker = new qq.maps.Marker({
                    map: map,
                    position: result.detail.location
                });
                $("#latitude").attr("value", marker.position.lat);
                $("#longitude").attr("value", marker.position.lng);
            }
        });
        $("#mapseacrh").click(function () {
            var address = $("#address").val();
            //通过getLocation();方法获取位置信息值
            geocoder.getLocation(address);
        });
    });
    function codeLatLng(lat, lng) {

        //获取经纬度数值   按照,分割字符串 取出前两位 解析成浮点数
        var latLng = new qq.maps.LatLng(lat, lng);
        //调用信息窗口
        var info = new qq.maps.InfoWindow({ map: map });

        //调用获取位置方法
        geocoder.getAddress(latLng);
    }
</script>