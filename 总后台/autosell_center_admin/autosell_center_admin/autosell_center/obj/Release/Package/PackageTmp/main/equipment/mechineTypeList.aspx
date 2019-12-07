<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="mechineTypeList.aspx.cs" Inherits="autosell_center.main.equipment.mechineTypeList" %>
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
    <script src="../public/script/jquery.form.js"></script>
    <script src="/main/public/script/Titlelist.js" type="text/javascript"></script>
    <link href="../../scripts/skin/jedate.css" rel="stylesheet" type="text/css" />
    <script src="../../scripts/jedate.js" type="text/javascript"></script>
    <style>
        .firmbtn {
            margin-left: 30px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="header"></div>
        <div class="main">
            <div id="addCaDiv" class="addDiv change">
                <h4>添加分类</h4>
                <ul>
                    <li>
                        <label>分类名称</label>
                        <input type="text" value="" placeholder="填写分类名称"  id="addName"/>
                    </li>
                   
                    <li>
                        <label></label>
                        <input type="button" value="确认添加" class="btnok"  onclick="add()" id="bt_ok"/>
                        <input type="button" value="取消" class="btnoff" onclick="divOff()" />
                    </li>
                </ul>
            </div>
            <div id="updataCaDiv" class="addDiv change">
                <h4>修改分类</h4>
                <ul>
                    <li>
                        <label>分类名称</label>
                        <input type="text" id="updateCN" value="" placeholder="填写分类名称" />
                    </li>
                    
                    <li>
                        <label></label>
                        <input type="button" value="确认修改" class="btnok"  onclick="update()"/>
                        <input type="button" value="取消" class="btnoff" onclick="divOff()" />
                    </li>
                </ul>
            </div>
            <div class="popupbj"></div>
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
                                <input type="hidden" id="EquType" />
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
                <div class="common_main">
                    <div class="navlist">
                        <dl>
                            <dt>设备管理<em class="fa fa-cog"></em></dt>
                            <dd>
                                <a class="change" href="#" onclick="qx_judge('qbsb')"><i class="change fa fa-inbox"></i>全部设备</a>
                            </dd>
                            <dd>
                                <a class="change"  href="#" onclick="qx_judge('tjsb')"><i class="change fa fa-plus-square"></i>添加设备</a>
                            </dd>
                        </dl>
                        <dl>
                            <dt>设备类别<em class="fa fa-cog"></em></dt>
                            <dd>
                                <a class="change acolor" href="#" onclick="qx_judge('sblb')"><i class="change icolor fa fa-server"></i>设备类别</a>
                            </dd>
                        </dl>
                    </div>
                    <section class="jiqlist">
                        <dl class="jiqlistTab">
                            <dd class="change ddcolor"><b>设备分类</b></dd>
                        </dl>
                        <div class="addClass">
                            <a class="change" onclick="addDiv()">
                                <i class="fa fa-plus"></i>
                                添加分类
                            </a>
                        </div>
                        <ul class="productclass" id="ull">
                            
                        </ul>
                    </section>
                </div>
            </div>
        </div>
        <input id="mechineID" runat="server" type="hidden" />
        <input id="mechineTypeID" runat="server" type="hidden" />
    </form>
</body>
</html>
<script>
    function addDiv() {
        $("#addCaDiv").addClass("addDivshow");
        setTimeout(function() {
            $(".popupbj").fadeIn();
        }, 100);
    }
    function divOff() {
        $(".popupbj").hide();
        $(".addDiv").removeClass("addDivshow");
    }
    //修改弹框
    function updeted(obj,id,name) {
        var $a = $(obj);
        var $li = $a.parent().parent().eq(0);
        $("#updateCN").val($li.find("span.classname").eq(0).html());
        $("#updateCId").val($li.find("span.classid").eq(0).html());
        $("#updataCaDiv").addClass("addDivshow");
        setTimeout(function() {
            $(".popupbj").fadeIn();
        }, 100);
        $("#updateCN").val(name);
        $("#mechineTypeID").val(id);
    }
    function deleTed(id)
    {
        $.ajax({
            type: "post",
            url: "mechineTypeList.aspx/del",
            contentType: "application/json; charset=utf-8",
            data: "{id:'"+id+"'}",
            dataType: "json",
            success: function (data) {
                if(data.d=="1")
                {
                    alert("删除成功");
                    sear();
                }else if(data.d="2")
                {
                    alert("删除失败");

                } else if (data.d = "3")
                {
                    alert("该类型已经使用无法删除");
                }
            }
        });
    }
</script>
<script>
    function sear() {
        $("#ull").empty();
        $(" <li>"
                 +" <label class='classid'>行号</label>"
                 +" <label class='classname'>分类名称</label>"
                 +" <label class='classdelete'>操作</label>"
                 +" </li>").appendTo("#ull");
        $.ajax({
            type: "post",
            url: "mechineTypeList.aspx/look",
            contentType: "application/json; charset=utf-8",
            data: "{str:'1'}",
            dataType: "json",
            success: function (data) {
                var serverdata = $.parseJSON(data.d);
                var serverdatalist = serverdata.length;
                for (var i = 0; i < serverdatalist; i++) {
                   
                    $("  <li>"
                             + "  <span class='classid'>" + serverdata[i].rowNum + "</span>"
                             +"   <span class='classname'>"+serverdata[i].name+"</span>"
                             +"   <span class='classdelete'>" 
                             + "       <a href='equipmentclass.aspx?mechineTypeID=" + serverdata[i].id + "'>料道设置</a>"
                             + "      <a onclick='updeted(this,&apos;" + serverdata[i].id + "&apos;,&apos;" + serverdata[i].name + "&apos;)'>修改</a>"
                             +"       <a onclick='deleTed("+serverdata[i].id+")'>删除</a>"
                             +"   </span>"
                             +" </li>").appendTo("#ull");
                }
            }
        });
        
    }; 
    function ddList(val, id) {
        $("#mechineID").val(id);
        $(".naiqBtn").find("input").val(val);
        $("#dll").hide();
    };
    $(function () {
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
        //根据id从后台在取有效期和状态
        $("#mechineID").val(id);
        $.ajax({
            type: "post",
            url: "../enterprise/devicelist.aspx/getInfo",
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
        });
        $(".popupbj").fadeIn();
        $(".zfpopup").addClass("zfpopup_on");

    };
    function qxClick() {
        $(".zfpopup").removeClass("zfpopup_on");
        setTimeout(function () { $(".popupbj").hide(); }, 300);

    }
    function okBtn() {
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
        });
    }
    function add()
    {
        //按钮设置成不可点击
 
        $("#bt_ok").attr("disabled", true);
        $.ajax({
            url: "../../ashx/asm.ashx",
            type: 'post',
            dataType: 'json',
            timeout: 10000,
            data: {
                action: "addMechineType",
                name: $("#addName").val()
            },
            success: function (resultData) {
                if (resultData.result=="1")
                {
                    alert("添加成功");
                    sear();

                } else if (resultData.result == "2")
                {
                    alert("添加失败");

                } else if (resultData.result == "3")
                {
                    alert("该类型的名称已经存在");
                }
                divOff();
                
            }
        });
        $("#bt_ok").attr("disabled", false);
    }
    function update()
    {
        $.ajax({
            url: "../../ashx/asm.ashx",
            type: 'post',
            dataType: 'json',
            timeout: 10000,
            data: {
                action: "updateMechineType",
                name: $("#updateCN").val(),
                mechineTypeID:  $("#mechineTypeID").val()
                
            },
            success: function (resultData) {
                if (resultData.result == "1") {
                    alert("修改成功");
                    sear();

                } else if (resultData.result == "2") {
                    alert("修改失败");

                } else if (resultData.result == "3") {
                    alert("该类型的名称已经存在");
                }
                divOff();
            }
        });
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
                        location.href = "/main/member/memberlist.aspx";
                    }
                    if (menuID == 'glygl') {//管理员管理
                        location.href = "/main/Adminlist/adminlist.aspx";
                    }
                    if (menuID == 'cplb') {//产品管理
                        location.href = "/main/product/productlist.aspx";
                    }
                    if (menuID == 'dgjl') {//订单管理
                        location.href = "/main/product/dglist.aspx";
                    }
                    if (menuID == 'zhcx') {//数据统计与查询
                        location.href = "/main/datastatistics/Statisticalquery.aspx";
                    }
                    if (menuID == 'cjdtt') {//分析
                        location.href = "/main/Analysis/Analysis.aspx'";
                    }
                    if (menuID == 'sjdp') {//数据大屏
                        window.open("/main/Big_screen/big_screen.aspx");
                    }
                    if (menuID == 'spgl') {//广告管理
                        location.href = "/main/Advertisement/video.aspx";
                    }

                } else if (resultData.result == "notLogin")//没有查看权限
                {
                    location.href = "../../../../blank.aspx";
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
