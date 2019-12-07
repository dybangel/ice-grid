<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dglist.aspx.cs" Inherits="autosell_center.main.product.dglist" %>

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
                                <a class="change acolor" href="#" onclick="qx_judge('dgjl')"><i class="change fa fa-window-restore"></i>订购记录</a>
                            </dd>
                             <dd>
                                <a class="change " href="#" onclick="qx_judge('gmjl')"><i class="change icolor fa fa-window-restore"></i>购买记录</a>
                            </dd>
                    </dl>
                </div>
                <section class="jiqlist">
                    <dl class="jiqlistTab">
                        <dd class="change ddcolor"><b>订单列表</b></dd>
                    </dl>
                    <ul class="jiqlistseach">
                        <li>
                            <asp:DropDownList ID="companyList" runat="server" OnSelectedIndexChanged="companyList_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                        </li>
                        <li>
                            <asp:DropDownList ID="mechineList" runat="server"></asp:DropDownList>
                        </li>
                        <li>
                            <input type="button" value="查询" class="seachbtn" onclick="getOrderList()" />
                        </li>
                         <li>
                                 <asp:Button  ID="excel" runat="server" OnClick="excel_Click" Text="导出Excel" CssClass="seachbtn"/>
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
          <input id="pageCurrentCount" runat="server" type="hidden" value="1"/>
         <input id="pageTotalCount" runat="server" type="hidden" value="1"/>
    </form>
</body>
</html>
<script>
    $(function () {
        qx_judge('dgjl');
        $("#li5").find("a").addClass("aborder");
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
             + " <label style='width:4%'>序号</label>"
                +" <label style='width:8%'>订单编号</label>"
                + " <label style='width:8%'>产品名称</label>"
                + " <label style='width:8%'>单价</label>"
                + " <label style='width:8%'>会员昵称</label>"
                + " <label style='width:8%'>周期</label>"
                 + " <label style='width:8%'>剩余天数</label>"
                + " <label style='width:8%'>起送日期</label>"
                + " <label style='width:8%'>止送日期</label>"
                + " <label style='width:8%'>派送方式</label>"
                + " <label style='width:8%'>总金额</label>"
                + " <label style='width:8%'>机器编号</label>"
                + " <label style='width:8%'>所属企业</label>"
        + "  </li>").appendTo("#memberList");
        $.ajax({
            type: "post",
            url: "dglist.aspx/getOrderList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{'mechineID':'" + $("#mechineList").val() + "',companyID:'" + $("#companyList").val() + "',pageCurrentCount:'" + $("#pageCurrentCount").val() + "'}",
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
                   
                    $(" <li>"
                          + "  <span style='width:4%'>" + serverdata[i].Row + "</span>"
                         + "   <span style='width:8%' title='" + serverdata[i].orderNO + "'>" + serverdata[i].orderNO + "</span>"
                         + "  <span style='width:8%' title='" + serverdata[i].proName + "'>" + serverdata[i].proName + "</span>"
                         + "  <span style='width:8%'>" + serverdata[i].price2 + "</span>"
                         + "  <span style='width:8%'>" + serverdata[i].nickName + "</span>"
                         + "  <span style='width:8%'>" + serverdata[i].zq + "</span>"
                           + "  <span style='width:8%'>" + serverdata[i].syNum + "</span>"
                         + "  <span style='width:8%'>" + serverdata[i].qsDate + "</span>"
                         + "  <span style='width:8%'>" + serverdata[i].zdDate + "</span>"
                         + "  <span style='width:8%'>" + serverdata[i].psStr + "</span>"
                         + "  <span style='width:8%'>" + serverdata[i].totalMoney + "</span>"
                         + "  <span style='width:8%'>" + serverdata[i].bh + "</span>"
                         + "  <span style='width:8%'>" + serverdata[i].name + "</span>"
                        
                         + "</li>").appendTo("#memberList");
                }
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
                        location.href = "memberlist.aspx";
                    }
                    if (menuID == 'dgjl') {//订购记录
                        //location.href = "dglist.aspx";
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

