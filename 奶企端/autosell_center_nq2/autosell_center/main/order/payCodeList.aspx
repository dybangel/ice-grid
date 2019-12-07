<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="payCodeList.aspx.cs" Inherits="autosell_center.main.order.payCodeList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
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
        .firmbtn {
            margin-left: 30px;
        }

        .memberlist {
            margin-top: 30px;
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
                            <dt>订单管理<em class="fa fa-cog"></em></dt>

                            <dd>
                                <a class="change " href="#" onclick="qx_judge('cpddtj')"><i class="change  fa fa-window-restore"></i>商品订单统计</a>
                            </dd>
                            <dd>
                                <a class="change" href="#" onclick="qx_judge('gmjl')"><i class="change fa fa-window-restore"></i>购买记录</a>
                            </dd>
                            <dd>
                                <a class="change " href="../equipment/paylist.aspx"><i class="change  fa fa-window-restore"></i>支付记录</a>
                            </dd>
                            <dd>
                                <a class="change" href="caigoushangpintj.aspx" onclick="qx_judge('cgsptj')"><i class="change fa fa-window-restore"></i>采购分拣单据</a>
                            </dd>
                            <dd>
                                <a class="change" href="historylist.aspx"><i class="change fa fa-window-restore"></i>历史清单</a>
                            </dd>
                            <dd>
                                <a class="change" href="exportOrder.aspx"><i class="change fa fa-window-restore"></i>订购订单导入</a>
                            </dd>
                            <dd>
                                <a class="change " href="exportOrderhistory.aspx"><i class="change  fa fa-window-restore"></i>订单导入记录</a>
                            </dd>
                            <dd>
                                <a class="change acolor" href="payCode.aspx"><i class="change acolor fa fa-window-restore"></i>充值卡列表</a>
                            </dd>
                        </dl>
                    </div>
                    <section class="jiqlist">
                        <dl class="jiqlistTab">
                            <dd class="change ddcolor"><b>商品报表</b></dd>
                        </dl>
                        <ul class="jiqlistseach">
                            <li>
                               <input type="text" runat="server" class="input" id="keyword" placeholder="会员昵称、手机号"/>
                            </li>
                            <li>
                               <input type="text" runat="server" class="input" id="cardNO" placeholder="卡号"/>
                            </li>
                            <li>
                                 <asp:DropDownList ID="statuType" runat="server">
                                    <asp:ListItem Value="0">全部</asp:ListItem>
                                    <asp:ListItem value="1">待兑换</asp:ListItem>
                                    <asp:ListItem value="2">已兑换</asp:ListItem>
                                    <asp:ListItem value="3">已作废</asp:ListItem>
                                </asp:DropDownList>
                            </li>
                            <li>
                                <input type="button" value="查询" class="seachbtn" onclick="sear()" />
                            </li>
                            <li>
                                <asp:Button ID="excel" runat="server" OnClick="excel_Click" Text="导出Excel" CssClass="seachbtn" />
                            </li>
                           
                        </ul>
                        <ul class="memberlist" id="ull">
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
        <input id="companyID" runat="server" type="hidden" />
        <input id="agentID" runat="server" type="hidden" />
        <input id="_cardbh" runat="server" type="hidden"/>
    </form>
</body>
</html>
<script>
    //window.onload = function () {
    //    jeDate({
    //        dateCell: "#start", //isinitVal:true,
    //        isTime: true, //isClear:false,
    //        choose: function (val) { },
    //        minDate: "2014-09-19 00:00:00"
    //    });
    //    jeDate({
    //        dateCell: "#end",
    //        isinitVal: true,
    //        isTime: true, //isClear:false,
    //        minDate: "2014-09-19 00:00:00"
    //    });
    //    jeDate({
    //        dateCell: "#startTime", //isinitVal:true,
    //        isTime: true, //isClear:false,
    //        choose: function (val) { },
    //        minDate: "2014-09-19 00:00:00"
    //    });
    //    jeDate({
    //        dateCell: "#endTime",
    //        isinitVal: true,
    //        isTime: true, //isClear:false,
    //        minDate: "2014-09-19 00:00:00"
    //    });
    //}
    
    function addPayCode()
    {
        var timestamp = Date.parse(new Date());
        $("#pcbh").val(timestamp);
        $(".popupbj").fadeIn();
        $("#adminpopup").addClass("zfpopup_on");
    }
    function qxClick() {
       
        $(".tangram-suggestion-main").hide();
        $(".popupbj").hide();
        $(".zfpopup").removeClass("zfpopup_on");
    }
    $(function () {
        
        $("#li2").find("a").addClass("aborder");
        sear();

    })
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
        $("#ull").empty();
        $(" <li>"
                  
                   + "<label style='width:10%'>批次编号</label>"
                   + "<label style='width:10%'>卡号</label>"
                   + "<label style='width:10%'>会员名称</label>"
                   + "<label style='width:10%'>手机号</label>"
                   + "<label style='width:10%'>面值</label>"
                   + "<label style='width:10%'>密码</label>"
                   + "<label style='width:10%'>开始时间</label>"
                   + "<label style='width:10%'>结束时间</label>"
                   + "<label style='width:10%'>状态</label>"
                   + "<label style='width:10%'>操作</label>"
                   + "</li>").appendTo("#ull");
        $.ajax({
            type: "post",
            url: "payCodeList.aspx/sear",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{pcbh:'" + $("#_cardbh").val() + "',keyword:'" + $("#keyword").val() + "',statuType:'" + $("#statuType").val() + "',pageCurrentCount:'" + $("#pageCurrentCount").val() + "',cardNO:'" + $("#cardNO").val() + "'}",
            success: function (data) {
                if(data.d.code=="200")
                {
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
                        var sta = serverdata[i].statu;
                        $(" <li>"
                                 + "   <span style='width:10%'>" + serverdata[i].codeBH + "</span>"
                                 + "   <span style='width:10%'>" + serverdata[i].cardNO + "</span>"
                                 + "   <span style='width:10%'>" + (serverdata[i].name == null ? "-" : serverdata[i].name) + "</span>"
                                 + "   <span style='width:10%'>" + (serverdata[i].phone == null ? "-" : serverdata[i].phone) + "</span>"
                                 + "   <span style='width:10%'>" + serverdata[i].mzMoney + "</span>"
                                 + "   <span style='width:10%'>" + serverdata[i].pwd + "</span>"
                                 + "   <span style='width:10%'>" + serverdata[i].startTime + "</span>"
                                 + "   <span style='width:10%'>" + serverdata[i].endTime + "</span>"
                                 + "   <span style='width:10%'>" + (sta=="1"?"待兑换":(sta=="2"?"已兑换":(sta=="3"?"已禁用":""))) + "</span>"
                                 + "   <span style='width:10%'><a onclick='setZF(" + serverdata[i].id+ ")'>"+(sta=="1"?"禁用":"")+"</a></span>"
                                + "</li> ").appendTo("#ull");
                    }
                }
            }
        })
    }
    function setZF(id)
    {
        if(confirm("作废该充值卡将不可用"))
        {
            $.ajax({
                type: "post",
                url: "payCodeList.aspx/setZF",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: "{id:'" + id + "'}",
                success: function (data) {
                    if (data.d.code == "200") {
                        window.location.reload();
                    }
                    else {
                        alert(data.d.msg);
                    }
                }
            })
        }
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
                    if (menuID == 'hylb') {//会员列表
                        location.href = "../member/memberlist.aspx";
                    }
                    if (menuID == 'jqddtj') {//订单管理
                       // location.href = "../order/orderform.aspx";
                    }
                    if (menuID == 'hyddgl') {//会员订单管理

                        location.href = "../order/order.aspx";
                    }
                    if (menuID == 'cgsptj') {//采购统计

                        location.href = "../order/caigoushangpintj.aspx";
                    }
                    if (menuID == 'cpddtj') {//商品订单统计
                       
                        location.href = "../order/Productform.aspx";
                    }
                    if (menuID == 'tjcp') {//添加商品

                        location.href = "../product/productadd.aspx";
                    }
                    if (menuID == 'gmjl') {//购买记录

                        location.href = "../equipment/orderlist.aspx";
                    }
                    if (menuID == 'cplb') {//商品列表
                        location.href = "../product/productlist.aspx";
                    }
                    if (menuID == 'sblb') {//设备管理
                        location.href = "../equipment/equipmentlist.aspx";
                    }
                    if (menuID == 'glygl') {//管理员管理
                        location.href = "../Administrators/adminlist.aspx";
                    }
                    if (menuID == 'jsgl') {//角色管理
                        location.href = "../Administrators/rolelist.aspx";
                    }
                    if (menuID == 'szhd') {//活动管理
                        location.href = "../activity/activity.aspx";
                    }
                    if (menuID == 'spgl') {//广告管理
                        location.href = "../Advertisement/video.aspx";
                    }
                    if (menuID == 'jqtjsp') {//机器添加视频
                        location.href = "../Advertisement/Jurisdiction.aspx";
                    }
                    if (menuID == 'gzhsz') {//公众号管理
                        location.href = "../enterprise/Thepublicjb.aspx";
                    }
                    if (menuID == 'mbxx') {//模板消息
                        location.href = "../enterprise/Distributor.aspx";
                    }
                    if (menuID == 'sjdp') {//数据大屏
                        window.open("/main/Big_screen/big_screen.aspx");
                    }
                    if (menuID == 'zhcx') {//数据统计与查询
                        location.href = "../datatj/Statisticalquery.aspx";
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

