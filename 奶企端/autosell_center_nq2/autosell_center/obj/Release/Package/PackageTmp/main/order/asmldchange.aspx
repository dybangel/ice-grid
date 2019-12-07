<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="asmldchange.aspx.cs" Inherits="autosell_center.main.order.asmldchange" %>

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

   <%-- <script src="/main/public/script/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="/main/public/script/Titlelist.js" type="text/javascript"></script>
      <link  href="../../scripts/skin/jedate.css" rel="stylesheet" type="text/css"/>
    <script src="../../scripts/jedate.js" type="text/javascript"></script>
    <script src="../../scripts/jedate.min.js"></script>--%>
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
                                <a class="change" href="Productform.aspx"><i class="change fa fa-window-restore"></i>商品订单统计</a>
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
                                <a class="change acolor" href="asmldchange.aspx"><i class="change acolor fa fa-window-restore"></i>库存变动</a>
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
                                <a class="change "  href="exportOrderhistory.aspx"><i class="change  fa fa-window-restore"></i>订单导入记录</a>
                            </dd>
                            <dd>
                                <a class="change "  href="payCode.aspx"><i class="change  fa fa-window-restore"></i>充值卡列表</a>
                            </dd>
                        </dl>
                    </div>
                    <section class="jiqlist">
                        <dl class="jiqlistTab">
                            <dd class="change ddcolor"><b>库存变动</b></dd>
                        </dl>
                        <ul class="jiqlistseach">
                            <li>
                                 <input name="act_stop_timeks" autocomplete="off" type="text" id="start"   runat="server"  class="input" value="" placeholder=""  />
                            </li>
                             <li>
                                 <input name="act_stop_timeks" autocomplete="off" type="text" id="end"   runat="server"  class="input" value="" placeholder=""  />
                            </li>
                             <li>
                             <select id="mechineList"   class="selectpicker" multiple data-live-search="true" data-live-search-placeholder="请输入搜索" data-actions-box="true" onchange="mechineChg()">
                             </select> 
                            </li>
                            <li>
                             <input id="productInfo"  placeholder="产品名称、编号" runat="server"/>
                            </li>
                             <li>
                                 <asp:DropDownList ID="statusList" runat="server">
                                     <asp:ListItem Value="0">全部</asp:ListItem>
                                     <asp:ListItem Value="1">增加</asp:ListItem>
                                     <asp:ListItem Value="-1">减少</asp:ListItem>
                                 </asp:DropDownList>
                            </li>
                            <li>
                                <asp:DropDownList ID="typeList" runat="server">
                                    <asp:ListItem Value="0">全部</asp:ListItem>
                                    <asp:ListItem Value="1">补货任务</asp:ListItem>
                                    <asp:ListItem Value="2">零售出货</asp:ListItem>
                                    <asp:ListItem Value="3">订购出货</asp:ListItem>
                                    <asp:ListItem Value="4">料道纠错</asp:ListItem>
                                </asp:DropDownList>
                            </li>
                            
                            <li>
                                <input type="button" value="查询" class="seachbtn"  onclick="sear()"/>
                            </li>
                            <li>
                                 <asp:Button  ID="excel" runat="server" OnClick="excel_Click"  Text="导出Excel" CssClass="seachbtn"/>
                            </li>
                             <li style="color:red;width:400px;">
                                 注意:2019-06-13以前为测试数据
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
         <input id="pageCurrentCount" runat="server" type="hidden" value="1"/>
         <input id="pageTotalCount" runat="server" type="hidden" value="1"/>
          <input  id="companyID" runat="server" type="hidden"/>
        <input id="agentID" runat="server" type="hidden"/>
         <input id="_mechineList" runat="server" type="hidden"/>
        <input id="_brandList" runat="server" type="hidden"/>
    </form>
</body>
</html>
<script>
    window.onload = function () {
        jeDate({
            dateCell: "#start", //isinitVal:true,
            
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
    
    function mechineChg() {
        $("#_mechineList").val($("#mechineList").val());
    }
    
    function getMechineList() {
        $.ajax({
            type: "post",
            url: "asmldchange.aspx/getMechineList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{companyID:'" + $("#companyID").val() + "'}",
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
        
        $("#li2").find("a").addClass("aborder");
        judge()
        getMechineList();
       
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
                  
                   + "<label style='width:11.1%'>机器名称</label>"
                   + "<label style='width:11.1%'>产品名称</label>"
                   + "<label style='width:11.1%'>料道编号</label>"
                   + "<label style='width:11.1%'>变化前</label>"
                   + "<label style='width:11.1%'>变化</label>"
                   + "<label style='width:11.1%'>变化后</label>"
                   + "<label style='width:11.1%'>类型</label>"
                   + "<label style='width:11.1%'>原因</label>"
                   + "<label style='width:11.1%'>时间</label>"
                   + "</li>").appendTo("#ull");
        $.ajax({
            type: "post",
            url: "asmldchange.aspx/sear",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{start:'" + $("#start").val() + "',end:'" + $("#end").val() + "',mechineID:'" + $("#mechineList").val() + "' ,pageCurrentCount:'" + $("#pageCurrentCount").val() + "',productInfo:'" + $("#productInfo").val() + "',companyID:'" + $("#companyID").val() + "',statusList:'" + $("#statusList").val() + "',typeList:'" + $("#typeList").val() + "'}",
            success: function (data) {
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
                    $(" <li>"
                             + "   <span style='width:11.1%'>" + serverdata[i].mechineName + "</span>"
                             + "   <span style='width:11.1%'>" + serverdata[i].proName + "</span>"
                             + "   <span style='width:11.1%'>" + serverdata[i].ldNO + "</span>"
                             + "   <span style='width:11.1%'>" + serverdata[i].beforeNum + "</span>"
                             + "   <span style='width:11.1%'>" + serverdata[i].changeNum + "</span>"
                             + "   <span style='width:11.1%'>" + serverdata[i].afterNum + "</span>"
                             + "   <span style='width:11.1%'>" + serverdata[i].statusName + "</span>"
                             + "   <span style='width:11.1%'>" + serverdata[i].typeName + "</span>"
                             + "   <span style='width:11.1%'>" + serverdata[i].chgTime + "</span>"
                             
                            + "</li> ").appendTo("#ull");
                }
            }
        })
    }
    function judge() {
        $.ajax({
            type: "post",
            url: "asmldchange.aspx/judge",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{operaID:'" + $("#agentID").val() + "',menuID:'cpddtj'}",
            success: function (data) {
                if (data.d.code == "500") {
                    $(".jiqlist").empty();
                    $(".jiqlist").html("<p class='noquanxian'>当前没有权限</p>").css({ "background": "#ddd" });
                }
            }
        })

    }
</script>