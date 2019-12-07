<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="orderform.aspx.cs" Inherits="autosell_center.main.order.orderform" %>
<%@ Register Src="~/ascx/CheckboxListControl.ascx" TagName="CheckboxListControl" TagPrefix="uc1" %>
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
    <style>
        .firmbtn {
            margin-left: 30px;
        }
        .jiqlistseach li table{
            display:block;
            top:0;
            min-height:100%;
        }
        .jiqlistseach li #divCustomCheckBoxList, #divCheckBoxList, #divCheckBoxList div, #divCheckBoxListClose{
            width:100% !important;
            padding-left:0 !important;
            color:#333;
        }
        .jiqlistseach li #divCustomCheckBoxList{
            position:relative;
        }
        .jiqlistseach li #divCustomCheckBoxList table{
            position:absolute;
        }
        .jiqlistseach li table td{
            width:100%;
        }
        .jiqlistseach li table tr:first-child{
            border:0;
        }
        .jiqlistseach li table td:last-child{
            text-align:left;
        }
        .jiqlistseach li table td{
            padding-left:0;
        }
        .jiqlistseach li table td{
            line-height:normal;
        }
        #divCheckBoxList div{
            padding-left:36px !important;
        }
        #divCheckBoxList input{
            left:0 !important;
            position:absolute !important;
        }
        .jiqlistseach li table{
            border:0 !important;
        }
    </style>
     <script type="text/javascript">
         window.onload = function () {
             jeDate({
                 dateCell: "#start", //isinitVal:true,
                 format: "YYYY-MM-DD",
                 isTime: false, //isClear:false,
                 choose: function(val) {},
                 minDate: "2014-09-19 00:00:00"
             });
             jeDate({
                 dateCell: "#endTime", //isinitVal:true,
                 //format: "YYYY-MM-DD",
                 isTime: false, //isClear:false,
                 format: "YYYY-MM-DD",
                 isinitVal: true,
                 choose: function (val) { },
                 minDate: "2014-09-19 23:59:59"
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
                        <i class="fa fa-cubes"></i>
                        <span>订单管理</span>
                    </h4>
                </div>
                <div class="common_main">
                    <div class="navlist">
                        <dl>
                            <dt>订单管理<em class="fa fa-cog"></em></dt> 
                            <dd>
                                <a class="change acolor" href="#" onclick="qx_judge('jqddtj')"><i class="change icolor fa fa-window-restore"></i>机器订单统计</a>
                            </dd>
                            <dd>
                                <a class="change" href="#" onclick="qx_judge('cpddtj')"><i class="change fa fa-window-restore"></i>商品订单统计</a>
                            </dd>
                            <dd>
                                <a class="change" href="#" onclick="qx_judge('gmjl')"><i class="change fa fa-window-restore"></i>购买记录</a>
                            </dd>
                            <dd>
                                <a class="change"  href="caigoushangpintj.aspx" onclick="qx_judge('cgsptj')"><i class="change fa fa-window-restore"></i>采购分拣单据</a>
                            </dd>
                             <dd>
                                <a class="change"  href="historylist.aspx" ><i class="change fa fa-window-restore"></i>历史清单</a>
                            </dd>
                              <dd>
                                <a class="change"  href="exportOrder.aspx" ><i class="change fa fa-window-restore"></i>订购订单导入</a>
                            </dd>
                              <dd>
                                <a class="change "  href="exportOrderhistory.aspx" ><i class="change  fa fa-window-restore"></i>订单导入记录</a>
                            </dd>
                        </dl>
                    </div>
                    <section class="jiqlist">
                        <dl class="jiqlistTab">
                            <dd class="change ddcolor"><b>订货报表</b></dd>
                        </dl>
                        <ul class="jiqlistseach">
                            <li style="width:228px;border:0">
                                <uc1:CheckboxListControl ID="cbosDeparentment" runat="server" />
                            </li>
                             <li>
                                <asp:DropDownList ID="operaList" runat="server"></asp:DropDownList>
                            </li>
                            <li>
                                 <input name="act_stop_timeks" autocomplete="off" type="text" id="start"   runat="server"  class="input" value="" placeholder="订单日期"  readonly="true"  />
                            </li>
                             <li>
                             
                             <input name="act_stop_timeks" autocomplete="off" type="text" id="endTime"   runat="server"  class="input" value="" placeholder="结束时间"  readonly="true"  />
                             </li>
                            <li>
                                <input type="button" value="查询" class="seachbtn"  onclick="sear()"/>
                               
                            </li>
                             <li>
                                 <asp:Button  ID="excel" runat="server" OnClick="excel_Click" Text="导出Excel" CssClass="seachbtn"/>
                            </li>
                        </ul>
                        <ul class="jiqlisttable" style="display: block;">
                            <li id="li">
                              
                            </li>
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
        <input  id="companyID" runat="server" type="hidden"/>
        <input  id="mechine_ID" runat="server" type="hidden"/>
        <input  id="_operaID" runat="server" type="hidden"/>
    </form>
</body>
</html>
<script>
    $(function () {
       // qx_judge('jqddtj');
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
    function sear()
    {
        $("#li").empty();
        $("<dl>" + "<dd style='width:5%'>序号</dd>"
                  + "<dd style='width:30%'>机器</dd>"
                  + "<dd style='width:25%'>订单日期</dd>"
                  + "<dd style='width:20%'>数量</dd>"
                  + "<dd style='width:20%'>操作</dd>"
                  + "</dl>").appendTo("#li");
        $("#mechine_ID").val($("#cbosDeparentment_hdscbo").val());
        $.ajax({
            type: "post",
            url: "orderform.aspx/sear",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{mechineID:'" + $("#cbosDeparentment_hdscbo").val() + "',companyID:'" + $("#companyID").val() + "',time:'" + $("#start").val() + "',operaID:'" + $("#operaList").val() + "',pageCurrentCount:'" + $("#pageCurrentCount").val() + "',endTime:'" + $("#endTime").val() + "',dls_ID:'" + $("#_operaID").val() + "'}",
            success: function (data) {
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
                for (var i = 0; i < serverdatalist; i++) {
                    $("<label style='width:5%'>" + serverdata[i].Row + "</label>  <label style='width:30%'>"
                                + "<img src='/main/public/images/ji1.jpg' alt='' />"
                                + "<span>" + serverdata[i].bh + "</span>"
                                + "</label>"
                                + "<label style='width:25%'>" + serverdata[i].createTime + "</label>"
                                + "<label style='width:20%'>" + serverdata[i].num + "</label>"
                                + "<label style='width:20%'>"
                                + "<a href='Billofgoods.aspx?mechineID=" + serverdata[i].mechineID + "&time=" + serverdata[i].createTime + "'>查看配货单</a>"
                                + "</label>").appendTo("#li");
                }
            }
        })
    }
    function excel()
    {
        $.ajax({
            url: "../../ashx/asm.ashx",
            type: 'post',
            dataType: 'json',
            timeout: 10000,
            data: {
                action: "exportExcel"
            },
            success: function (resultData) {
                alert(resultData);
            }
        })
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
