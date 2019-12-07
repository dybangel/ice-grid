<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="order.aspx.cs" Inherits="autosell_center.main.order.order" %>

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
    <link href="../../scripts/skin/jedate.css" rel="stylesheet" type="text/css" />
    <script src="../../scripts/jedate.js" type="text/javascript"></script>
    <script src="../../scripts/jedate.min.js"></script>
    <script type="text/javascript">
        window.onload = function () {
            //jeDate({
            //    dateCell: "#start", //isinitVal:true,
            //    //format: "YYYY-MM-DD",
            //    isTime: false, //isClear:false,
            //    choose: function(val) {},
            //    minDate: "2014-09-19 00:00:00"
            //});
            //jeDate({
            //    dateCell: "#end",
            //    isinitVal: true,
            //    isTime: true, //isClear:false,
            //    minDate: "2014-09-19 00:00:00"
            //});
            jeDate({
                dateCell: "#startTime", //isinitVal:true,
                //format: "YYYY-MM-DD",
                isTime: false, //isClear:false,
                choose: function (val) { },
                minDate: "2014-09-19 00:00:00"
            });
            jeDate({
                dateCell: "#endTime",
                isinitVal: true,
                isTime: true, //isClear:false,
                minDate: "2014-09-19 00:00:00"
            });
        }
    </script>
    <style>
        .jiqlistseach {
            margin-bottom: 30px;
        }

        .memberlist li .membname {
            width: 16%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="header"></div>
        <div class="main">
            <%-- <div id="adminpopup" class="change zfpopup">
                <h4>配送员设置</h4>
                <ul>
                    <li>
                        <h5>延期至</h5>
                        <label>
                             <input name="act_stop_timeks" type="text" id="start"   runat="server"  class="input" value="" placeholder="延期至，当日派送"  readonly="true"  />
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
            </div>--%>

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
                                <a class="change acolor" href="order.aspx"><i class="change  fa fa-user"></i>订购记录</a>
                            </dd>
                            <dd>
                                <a class="change " href="orderlist.aspx"><i class="change icolor fa fa-file-text"></i>购买记录</a>
                            </dd>    
                            <dd>
                                <a class="change " href="Productform.aspx"><i class="change  fa fa-user"></i>商品订单统计</a>
                            </dd>
                         <dd>
                                <a class="change" href="paylist.aspx"><i class="change  fa fa-user"></i>支付记录</a>
                            </dd>
                        </dl>
                    </div>
                    <section class="jiqlist">
                        <dl class="jiqlistTab">
                            <dd class="change ddcolor"><b>订单列表</b></dd>
                        </dl>
                        <ul class="jiqlistseach">
                            <li>
                                <input type="text" value="" placeholder="订单编号" id="bh" runat="server" />
                            </li>
                            <li>
                                <asp:DropDownList ID="companyList" runat="server" OnSelectedIndexChanged="companyList_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </li>
                            <li>
                                <asp:DropDownList ID="mechineList" runat="server"></asp:DropDownList>
                            </li>
                            

                            <li>
                                <asp:DropDownList ID="zqList" runat="server">
                                    <%--  <asp:ListItem Value="0">所有周期</asp:ListItem>
                                <asp:ListItem Value="1">1天</asp:ListItem>
                                <asp:ListItem Value="30">30天</asp:ListItem>
                                <asp:ListItem Value="90">90天</asp:ListItem>
                                <asp:ListItem Value="180">180天</asp:ListItem>
                                <asp:ListItem Value="365">365天</asp:ListItem>--%>
                                </asp:DropDownList>
                            </li>
                            <li>
                                <%--0生产中1配送中2已转售3配送完成4已兑换--%>
                                <asp:DropDownList ID="DropDownList1" runat="server">
                                    <asp:ListItem Value="-1">所有状态</asp:ListItem>
                                    <asp:ListItem Value="0">生产中</asp:ListItem>
                                    <asp:ListItem Value="1">配送中</asp:ListItem>
                                    <asp:ListItem Value="3">配送完成</asp:ListItem>
                                    <asp:ListItem Value="4">已兑换</asp:ListItem>
                                    <asp:ListItem Value="6">暂停</asp:ListItem>
                                </asp:DropDownList>
                            </li>
                            <li>
                                <input type="text" placeholder="会员手机号/会员ID" id="keywords" runat="server" />
                            </li>
                            <li>
                                <input name="act_stop_timeks" type="text" id="startTime" runat="server" class="input" value="" placeholder="" />
                            </li>
                            <li>

                                <input name="act_stop_timeks" type="text" id="endTime" runat="server" class="input" value="" placeholder="" />
                            </li>
                            <li>
                                <input type="button" value="查询" class="seachbtn" onclick="getOrderList()" />
                            </li>
                            <li>
                                <asp:Button ID="excel" runat="server" OnClick="excel_Click" Text="导出Excel" CssClass="seachbtn" />
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
        <input id="_operaID" runat="server" type="hidden" />
    </form>
</body>
</html>
<script>
    $(function () {
        //qx_judge('hyddgl');
        $("#li1").find("a").addClass("aborder");
        // judge()
        getOrderList();
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
        getOrderList();
    }
    function pageChg() {
        $("#pageCurrentCount").val($("#pageSel").val());
        getOrderList();
    }
    function getOrderList() {
        $("#memberList").empty();
        $(" <li>"
              + " <label style='width:2%'>序号</label>"
                + " <label style='width:8%'>订单编号</label>"
                + " <label style='width:5%'>会员名称</label>"
                + " <label style='width:8%'>手机号</label>"
                + " <label style='width:5%'>商品名称</label>"
                + " <label style='width:5%'>周期/总数</label>"
                + " <label style='width:5%'>剩余天数</label>"
                + " <label style='width:5%'>订单状态</label>"
                + " <label style='width:5%'>配送时间</label>"
                + " <label style='width:10%'>优惠方式</label>"
                + " <label style='width:8%'>首送日期</label>"
                + " <label style='width:8%'>止送日期</label>"

                + " <label style='width:10%'>订单时间</label>"
                + " <label style='width:8%'>配送机器</label>"
                + " <label style='width:8%'>操作</label>"
        + "  </li>").appendTo("#memberList");
        $.ajax({
            type: "post",
            url: "order.aspx/getOrderList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{bh:'" + $("#bh").val() + "',mechineID:'" + $("#mechineList").val() + "',zq:'" + $("#zqList").val() + "',companyID:'" + $("#companyList").val() + "',pageCurrentCount:'" + $("#pageCurrentCount").val() + "',zt:'" + $("#DropDownList1").val() + "',keywords:'" + $("#keywords").val() + "',start:'" + $("#startTime").val() + "',end:'" + $("#endTime").val() + "'}",
            success: function (data) {

                var count = data.d.split('@@@')[0];
                if (parseInt(count) >= 0) {
                    $("#pageSel").empty();
                    for (var k = 1; k <= parseInt(count) ; k++) {
                        $("<option value='" + k + "'>" + k + "</option>").appendTo("#pageSel");
                    }
                }

                var serverdata = $.parseJSON(data.d.split('@@@')[1]);
                $("#pageTotalCount").val(count);
                var serverdatalist = serverdata.length;

                for (var i = 0; i < serverdatalist; i++) {
                    var zt = "";
                    if (serverdata[i].orderZT == "0") {
                        zt = "等待配送";
                    } else if (serverdata[i].orderZT == "1") {
                        zt = "配送中";
                    } else if (serverdata[i].orderZT == "2") {
                        zt = "已转售";
                    } else if (serverdata[i].orderZT == "3") {
                        zt = "配送完成";
                    } else if (serverdata[i].orderZT == "4") {
                        zt = "已兑换";
                    } else if (serverdata[i].orderZT == "6") {
                        zt = "已暂停";
                    }
                    var pay = "";
                    if (serverdata[i].payType == "1") {
                        pay = "微信扫码支付";
                    } else if (serverdata[i].payType == "2") {
                        pay = "支付宝扫码";
                    } else if (serverdata[i].payType == "3") {
                        pay = "微信公众号";
                    } else if (serverdata[i].payType == "4") {
                        pay = "会员卡余额支付";
                    }
                    $(" <li>"
                         + "   <span style='width:2%'>" + serverdata[i].Row + "</span>"
                         + "   <span style='width:8%'>" + serverdata[i].orderNO + "</span>"
                         + "   <span style='width:5%' title='" + serverdata[i].name + "'>" + serverdata[i].name + "</span>"
                        + "   <span style='width:8%'>" + serverdata[i].phone + "</span>"
                         + "   <span style='width:5%'>" + serverdata[i].proName + "</span>"
                         + "   <span style='width:5%'>" + serverdata[i].zqNum + "天/" + serverdata[i].totalNum + "天</span>"
                         + "   <span style='width:5%'>" + serverdata[i].syNum + "天</span>"
                         + "   <span style='width:5%'>" + zt + "</span>"
                         + "   <span style='width:5%'>" + serverdata[i].psModeStr + "</span>"
                         + "   <span style='width:10%'>" + serverdata[i].psCycle + "</span>"
                          + "   <span style='width:8%'>" + serverdata[i].startTime + "</span>"
                         + "   <span style='width:8%'>" + serverdata[i].endTime + "</span>"
                         + "   <span style='width:10%'>" + serverdata[i].createTime.replace('T', ' ') + "</span>"
                         + "   <span style='width:8%'>" + serverdata[i].bh + "</span>"
                         + "   <span style='width:8%;color:red;'>"
                         + "       <a style='color:rgb(0,148,255)' href='orderDetail.aspx?orderNO=" + serverdata[i].orderNO + "'>明细</a>"
                         + "       <a style='color:rgb(0,148,255)' onclick='setDelay(" + serverdata[i].orderNO + ")'>" + (serverdata[i].orderZT == "6" ? "恢复" : "暂停") + "</a>"
                         //+"       <a>取消订单</a>"
                         + "   </span>"
                         + "</li>").appendTo("#memberList");
                }
            }
        })
    }
    function setDelay(orderNO) {
        $.ajax({
            type: "post",
            url: "order.aspx/setDelay",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{orderNO:'" + orderNO + "'}",
            success: function (data) {
                if (data.d.code == "200") {
                    window.location.reload();
                } else {
                    alert(data.d.msg);
                }
            }
        })
    }
    function setDelayTC(orderNO) {
        $(".popupbj").fadeIn();
        $("#adminpopup").addClass("zfpopup_on");
    }
    function qxClick() {
        $(".tangram-suggestion-main").hide();
        $(".popupbj").hide();
        $(".zfpopup").removeClass("zfpopup_on");
    }
    function judge() {
        $.ajax({
            type: "post",
            url: "order.aspx/judge",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{operaID:'" + $("#_operaID").val() + "',menuID:'hyddgl'}",
            success: function (data) {
                if (data.d.code == "500") {
                    $(".jiqlist").empty();
                    $(".jiqlist").html("<p class='noquanxian'>当前没有权限</p>").css({ "background": "#ddd" });
                }
            }
        })

    }
</script>
