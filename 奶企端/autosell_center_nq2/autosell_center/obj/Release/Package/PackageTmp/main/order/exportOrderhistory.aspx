<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="exportOrderhistory.aspx.cs" Inherits="autosell_center.main.order.exportOrderhistory" %>

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
    <script src="../public/script/jquery.form.js" type="text/javascript"></script>
    <script src="../bootstrapSelect/dist/js/bootstrap.min.js"></script>
    <script src="../bootstrapSelect/dist/js/bootstrap-select.min.js" charset="gb2312"></script>


    <script src="/main/public/script/Titlelist.js" type="text/javascript"></script>
    <link href="../../scripts/skin/jedate.css" rel="stylesheet" type="text/css" />
    <script src="../../scripts/jedate.js" type="text/javascript"></script>
    <script src="../../scripts/jedate.min.js"></script>
    <style>
        .jiqlistseach {
            margin-bottom: 30px;
        }
    </style>
    <script type="text/javascript">
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
    <form id="form1" runat="server"  class="form-horizontal"  role="form" method="post" enctype="multipart/form-data">
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
                                <a class="change " href="Productform.aspx"><i class="change  fa fa-window-restore"></i>商品订单统计</a>
                            </dd>
                             <dd>
                                <a class="change " href="order.aspx"><i class="change icolor fa fa-window-restore"></i>会员订单管理</a>
                            </dd>
                             <dd>
                                <a class="change" href="../equipment/orderlist.aspx"><i class="change fa fa-window-restore"></i>购买记录</a>
                            </dd>
                             <dd>
                                <a class="change " href="../equipment/paylist.aspx"><i class="change  fa fa-window-restore"></i>支付记录</a>
                            </dd>
                              <dd>
                                <a class="change " href="asmldchange.aspx"><i class="change  fa fa-window-restore"></i>库存变动</a>
                            </dd>
                            <dd> 
                                <a class="change"  href="caigoushangpintj.aspx"><i class="change fa fa-window-restore"></i>采购分拣单据</a>
                            </dd>
                              <dd>
                                <a class="change"  href="historylist.aspx"><i class="change fa fa-window-restore"></i>历史清单</a>
                            </dd>
                             <dd>
                                <a class="change"  href="exportOrder.aspx"><i class="change fa fa-window-restore"></i>订购订单导入</a>
                            </dd>
                            <dd>
                                <a class="change acolor"  href="exportOrderhistory.aspx"><i class="change icolor fa fa-window-restore"></i>订单导入记录</a>
                            </dd>
                            <dd>
                                <a class="change "  href="payCode.aspx"><i class="change  fa fa-window-restore"></i>充值卡列表</a>
                            </dd>
                        </dl>
                    </div>
                     
                    <section class="jiqlist">
                        <dl class="jiqlistTab">
                            <dd class="change ddcolor"><b>订单导入记录</b></dd>
                        </dl>
                        <ul class="jiqlistseach">
                             <li>
                                <input name="act_stop_timeks" autocomplete="off" type="text" id="start" runat="server" class="input" value="" placeholder="开始日期" readonly="true" />
                            </li>
                            <li>
                                <input name="act_stop_timeks" autocomplete="off" type="text" id="end" runat="server" class="input" value="" placeholder="结束日期" readonly="true" />
                            </li>
                              <li>
                                <input type="text" id="tjr" runat="server" placeholder="推荐人"/>
                            </li>
                             <li>
                                <input type="text" id="phone" runat="server" placeholder="手机号"/>
                            </li>
                            <li>
                               <select id="dhtype" runat="server">
                                   <option value="-1">全部</option>
                                   <option value="0">未兑换</option>
                                   <option value="1">已兑换</option>
                               </select>
                            </li>
                             <li>
                                <input type="text" id="orderCode" runat="server" placeholder="订奶码"/>
                            </li>
                             <li>
                                <input type="button" id="exportData" value="查询" style="background-color:rgb(58,119,213);color:#fff" onclick="getList()"/>
                            </li>
                            <li>
                                 <asp:Button  ID="excel" runat="server" OnClick="excel_Click"  Text="导出Excel" CssClass="seachbtn"/>
                            </li>
                        </ul>

                        <ul class="memberlist" id="memberList">
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
        <input id="_content" runat="server" type="hidden"/>
        <input id="agentID" runat="server" type="hidden"/>
    </form>
</body>
</html>
<script>
    function judge() {
        $.ajax({
            type: "post",
            url: "exportOrderhistory.aspx/judge",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{operaID:'" + $("#agentID").val() + "',menuID:'dddrjl'}",
            success: function (data) {
                if (data.d.code == "500") {
                    $(".jiqlist").empty();
                    $(".jiqlist").html("<p class='noquanxian'>当前没有权限</p>").css({ "background": "#ddd" });
                }
            }
        })

    }
    function getPage(val) {
        if (val == "first") {
            $("#pageCurrentCount").val("1");
        } else if (val == "up") {
            $("#pageCurrentCount").val(parseInt($("#pageCurrentCount").val()) - 1);
        } else if (val == "down") {
            $("#pageCurrentCount").val(parseInt($("#pageCurrentCount").val()) + 1)
        } else if (val == "end") {

        }
        getList();
    }
    function pageChg() {
        $("#pageCurrentCount").val($("#pageSel").val());
        getList();
    }
    $(function () {
        $("#li2").find("a").addClass("aborder");
        judge();
        getList();
    })
   
    function getList()
    {
        $("#memberList").empty();
        $(" <li>"
              + " <label style='width:8.3%'>兑换手机号</label>"
               + " <label style='width:8.3%'>会员手机号</label>"
              + " <label style='width:8.3%'>会员名称</label>"
              + " <label style='width:8.3%'>商品条码</label>"
              + " <label style='width:8.3%'>商品名称</label>"
              + " <label style='width:5%'>商品单价</label>"
              + " <label style='width:5%'>订单周期</label>"
              + " <label style='width:6%'>订奶码</label>"
              + " <label style='width:6%'>状态</label>"
              + " <label style='width:6%'>推荐人</label>"
              + " <label style='width:8.3%'>导入时间</label>"
              + " <label style='width:8.3%'>兑换时间</label>"
              + " <label style='width:8.0%'>备注</label>"
              + " <label style='width:5.5%'>操作</label>"
        + "  </li>").appendTo("#memberList");
        $.ajax({
            type: "post",
            url: "exportOrderhistory.aspx/getList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{companyID:'" + $("#companyID").val() + "',pageCurrentCount:'" + $("#pageCurrentCount").val() + "',phone:'" + $("#phone").val() + "',type:'" + $("#dhtype").val() + "',start:'" + $("#start").val() + "',end:'" + $("#end").val() + "',tjr:'" + $("#tjr").val() + "',orderCode:'" + $("#orderCode").val() + "'}",
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
                    var serverdatalist = serverdata.length;
                    for (var i = 0; i < serverdatalist; i++) {
                        $(" <li>"
                            + "  <span style='width:8.3%'>" + serverdata[i].phone + "</span>"
                             + "   <span style='width:8.3%'>" + serverdata[i].mphone + "</span>"
                            + "   <span style='width:8.3%'>" + serverdata[i].name + "</span>"
                            + "   <span style='width:8.3%'>" + serverdata[i].productCode + "</span>"
                            + "   <span style='width:8.3%'>" + serverdata[i].proname + "</span>"
                            + "   <span style='width:5%'>" + serverdata[i].productPrice + "</span>"
                            + "   <span style='width:5%'>" + serverdata[i].zq + "</span>"
                            + "   <span style='width:6%'>" + serverdata[i].orderCode + "</span>"
                            + "   <span style='width:6%'>" + (serverdata[i].status=="1"?"已兑换":"未兑换") + "</span>"
                            + "   <span style='width:6%'>" + serverdata[i].tjr + "</span>"
                            + "   <span style='width:8.3%'>" + serverdata[i].createTime + "</span>"
                            + "   <span style='width:8.3%'>" + serverdata[i].dhTime + "</span>"
                            + "   <span style='width:8.0%'>" + serverdata[i].bz + "</span>"
                            + "   <span style='width:5.5%'>"
                            + "       <a style='color:#0094ff' onclick='delData(" + serverdata[i].id + ")'>删除</a>"
                             + "       <a style='color:#0094ff' onclick='sendMsg(" + serverdata[i].id + ")'>发送短信</a>"
                            + "   </span>"
                            + "</li>").appendTo("#memberList");
                    }
                }
               
            }
        })
    }
    function sendMsg(id)
    {
        $.ajax({
            type: "post",
            url: "exportOrderhistory.aspx/sendMsg",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{id:'" + id + "'}",
            success: function (data) {
               alert(data.d.msg)

            }
        })
    }
    function delData(id)
    {
        $.ajax({
            type: "post",
            url: "exportOrderhistory.aspx/delData",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{id:'" + id + "'}",
            success: function (data) {
                if (data.d.code == "200") {
                    window.location.reload();
                }

            }
        })
    }
    
    function pageChg() {
        $("#pageCurrentCount").val($("#pageSel").val());
        getList();
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

                    if (menuID == 'zfjl') {//采购统计1111

                        location.href = "../equipment/paylist.aspx";
                    }
                    if (menuID == 'lsqd') {//采购统计

                        location.href = "historylist.aspx";
                    }
                    if (menuID == 'dgdddr') {//采购统计

                        location.href = "exportOrder.aspx";
                    }
                    if (menuID == 'dddrjl') {//采购统计

                        location.href = "exportOrderhistory.aspx";
                    }
                    if (menuID == 'czklb') {//采购统计

                        location.href = "payCode.aspx";
                    }

                    if (menuID == 'hylb') {//会员列表
                        location.href = "../member/memberlist.aspx";
                    }
                    if (menuID == 'jqddtj') {//订单管理
                        // location.href = "../order/orderform.aspx";
                    }
                    if (menuID == 'hyddgl') {//会员订单管理

                        location.href = "../order/order.aspx";
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
