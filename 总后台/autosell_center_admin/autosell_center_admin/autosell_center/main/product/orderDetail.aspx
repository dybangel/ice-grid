<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="orderDetail.aspx.cs" Inherits="autosell_center.main.order.orderDetail" %>

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
                <div class="navlist">
                     <dl>
                            <dt>订单管理<em class="fa fa-cog"></em></dt>

                            <dd>
                                <a class="change acolor" href="order.aspx"><i class="change  fa fa-user"></i>订购记录</a>
                            </dd>
                            <dd>
                                <a class="change " href="orderlist.aspx"><i class="change icolor fa fa-file-text"></i>购买记录</a>
                            </dd>
    
                        </dl>
                </div>
                <section class="jiqlist">
                    <dl class="jiqlistTab">
                        <dd class="change ddcolor"><b>订单列表</b></dd>
                    </dl>
                    <ul class="jiqlistseach">
                       
                        <li>
                            <asp:DropDownList ID="zt" runat="server">
                                <asp:ListItem Value="0">全部</asp:ListItem>
                                <asp:ListItem Value="1">已完成</asp:ListItem>
                                <asp:ListItem Value="2">已失效</asp:ListItem>
                                <asp:ListItem Value="3">已转售</asp:ListItem>
                                <asp:ListItem Value="4">待取货</asp:ListItem>
                                <asp:ListItem Value="5">待配送</asp:ListItem>
                                <asp:ListItem Value="6">已出售</asp:ListItem>
                                <asp:ListItem Value="7">兑换</asp:ListItem>
                            </asp:DropDownList>
                        </li>
                         
                        <li>
                            <input type="button" value="查询" class="seachbtn" onclick="getOrderList()" />
                        </li>
                       <%--  <li>
                            <asp:Button  ID="excel" runat="server" OnClick="excel_Click"  Text="导出Excel" CssClass="seachbtn"/>
                         </li>--%>
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
        <input id="pageCurrentCount" runat="server" type="hidden" value="1"/>
         <input id="pageTotalCount" runat="server" type="hidden" value="1"/>
        <input id="_orderID" runat="server" type="hidden"/>
    </form>
</body>
</html>
<script>
    $(function () {
    
        $("#li1").find("a").addClass("aborder");
        getOrderList();
    })
    function getPage(val)
    {
        if (val == "first")
        {
            $("#pageCurrentCount").val("1");
        } else if (val == "up")
        {
            var index = parseInt($("#pageCurrentCount").val()) - 1;
            
            if(index>=1)
            {
                $("#pageCurrentCount").val(index);
            }
           
        } else if (val == "down") {
            var index = parseInt($("#pageCurrentCount").val()) + 1;
            if (index <=parseInt($("#pageTotalCount").val()))
            {
              $("#pageCurrentCount").val(index);
            }
          
        } else if (val == "end") {
            $("#pageCurrentCount").val($("#pageTotalCount").val());
        }
        getOrderList();
    }
    function pageChg()
    {
        $("#pageCurrentCount").val($("#pageSel").val());
        getOrderList();
    }
    function getOrderList() {
        $("#memberList").empty();
        $(" <li>"
              + " <label style='width:2%'>序号</label>"
                +" <label style='width:16.6%'>订单编号</label>"
                + " <label style='width:16.6%'>会员名称</label>"
                 + " <label style='width:16.6%'>商品名称</label>"
                + " <label style='width:14.6%'>取货码</label>"
                + " <label style='width:16.6%'>取货时间</label>"
                + " <label style='width:16.6%'>订单状态</label>"
        + "  </li>").appendTo("#memberList");
        $.ajax({
            type: "post",
            url: "orderDetail.aspx/getOrderList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{zt:'" + $("#zt").val() + "',orderNO:'" + $("#_orderID").val() + "',pageCurrentCount:'" + $("#pageCurrentCount").val() + "'}",
            success: function (data) {
                
                var count = data.d.split('@')[0];
                if(parseInt(count)>=0)
                {
                    $("#pageSel").empty();
                    for (var k = 1; k <=parseInt(count) ;k++)
                    {
                        $("<option value='" + k + "'>" + k + "</option>").appendTo("#pageSel");
                    }
                }
                var serverdata = $.parseJSON(data.d.split('@')[1]);
                $("#pageTotalCount").val(count);
                var serverdatalist = serverdata.length;
              
                for (var i = 0; i < serverdatalist; i++) {
                 
                    $(" <li>"
                        + "   <span style='width:2%'>" + serverdata[i].Row + "</span>"
                         + "   <span style='width:16.6%'>" + serverdata[i].orderNO + "</span>"
                         + "   <span style='width:16.6%'>" + serverdata[i].name + "</span>"
                         + "   <span style='width:16.6%' title='" + serverdata[i].pname + "'>" + serverdata[i].pname + "</span>"
                         + "   <span style='width:14.6%'>" + serverdata[i].code + "</span>"
                         + "   <span style='width:16.6%'>" + serverdata[i].createTime + "</span>"
                         + "   <span style='width:16.6%'>" + serverdata[i].ztName+ "</span>"
                     
                       
                         + "</li>").appendTo("#memberList");
                }
            }
        })
    }
   
</script>