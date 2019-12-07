<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="orderMe.aspx.cs" Inherits="autosell_center.main.equipment.orderMe" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link rel="shortcut icon" href="/main/public/images/favicon.ico" />
    <link href="/main/public/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/common.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/style.css" rel="stylesheet" type="text/css" />
    <script src="/main/public/script/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="/main/public/script/Titlelist.js" type="text/javascript"></script>
    <style>
        .jiqlistseach {
            margin-bottom: 30px;
        }
        .memberlist li .membname{
            width:16%;
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
                <%--<div class="navlist">
                    <dl>
                        <dt>订单管理<em class="fa fa-cog"></em></dt>
                       
                       <dd>
                                <a class="change acolor" href="../member/memberlist.aspx"><i class="change icolor fa fa-user"></i>会员列表</a>
                            </dd>
                             <dd>
                                <a class="change acolor" href="../order/order.aspx"><i class="change icolor fa fa-file-text"></i>会员订单管理</a>
                             </dd>
                    </dl>
                </div>--%>
                <section class="jiqlist">
                    <dl class="jiqlistTab">
                        <dd class="change ddcolor"><b>订单列表</b></dd>
                    </dl>
                    <ul class="jiqlistseach">
                        <li>
                            <input type="text" value="" placeholder="订单编号" id="bh" />
                        </li>
                        <li>
                             
                            <asp:DropDownList ID="mechineList" runat="server"></asp:DropDownList>
                        </li>
                        <li>
                            
                            <asp:DropDownList ID="zqList" runat="server">
                                 <asp:ListItem Value="0">所有周期</asp:ListItem>
                                <asp:ListItem Value="1">1天</asp:ListItem>
                                <asp:ListItem Value="30">30天</asp:ListItem>
                                <asp:ListItem Value="90">90天</asp:ListItem>
                                <asp:ListItem Value="180">180天</asp:ListItem>
                                <asp:ListItem Value="365">365天</asp:ListItem>
                            </asp:DropDownList>
                        </li>
                        <li>
                            <input type="button" value="查询" class="seachbtn" onclick="getOrderList()" />
                        </li>
                    </ul>
                    <ul class="memberlist" id="memberList">
                       
                    </ul>
                </section>
            </div>
        </div>
    </div>
           <input id="mechine_id" runat="server" type="hidden" />
         <input id="companyID" runat="server" type="hidden"/>
    </form>
</body>
</html>
<script>
    $(function () {
        $("#li2").find("a").addClass("aborder");
        getOrderList();
    })
    function getOrderList() {
        $("#memberList").empty();
        $(" <li>"
                +" <label  style='width:12.5%'>订单编号</label>"
                + " <label style='width:12.5%'>会员名称</label>"
                + " <label style='width:12.5%'>联系电话</label>"
                + " <label style='width:12.5%'>订购周期</label>"
                + " <label style='width:12.5%'>剩余天数</label>"
                + " <label style='width:12.5%'>订单状态</label>"
                + " <label style='width:12.5%'>订单时间</label>"
                + " <label style='width:12.5%'>配送机器</label>"
                //+" <label class='membcz'>操作</label>"
        + "  </li>").appendTo("#memberList");
        $.ajax({
            type: "post",
            url: "orderMe.aspx/getOrderList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{bh:'" + $("#bh").val() + "',mechineID:'" + $("#mechineList").val() + "',zq:'" + $("#zqList").val() + "',companyID:'" + $("#companyID").val() + "'}",
            success: function (data) {
                var serverdata = $.parseJSON(data.d);
                var serverdatalist = serverdata.length;

                for (var i = 0; i < serverdatalist; i++) {
                    var zt = "";
                    if (serverdata[i].zt=="0")
                    {
                        zt = "等待配送";
                    } else if (serverdata[i].zt == "1")
                    {
                        zt = "配送中";
                    } else if (serverdata[i].zt == "2") {
                        zt = "已转售";
                    } else if (serverdata[i].zt == "3") {
                        zt = "配送完成";
                    }
                    $(" <li>"
                         + "   <span style='width:12.5%'>" + serverdata[i].orderNO + "</span>"
                         + "   <span style='width:12.5%' title='" + serverdata[i].name + "'>" + serverdata[i].name + "</span>"
                         + "   <span style='width:12.5%'>" + serverdata[i].phone + "</span>"
                         + "   <span style='width:12.5%'>" + serverdata[i].zq + "天</span>"
                         + "   <span style='width:12.5%'>" + serverdata[i].syNum + "天</span>"
                         + "   <span style='width:12.5%'>" + zt + "</span>"
                         + "   <span style='width:12.5%'>" + serverdata[i].createTime + "</span>"
                         + "   <span style='width:12.5%'>" + serverdata[i].bh + "</span>"
                         //+"   <span class='membcz'>"
                         //+"       <a>配送</a>"
                         //+"       <a>修改日期</a>"
                         //+"       <a>取消订单</a>"
                         //+"   </span>"
                         + "</li>").appendTo("#memberList");
                }
            }
        })
    }
</script>
