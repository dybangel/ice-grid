<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Productform.aspx.cs" Inherits="autosell_center.main.order.Productform" %>

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
                                <a class="change acolor" href="Productform.aspx"><i class="change icolor fa fa-window-restore"></i>商品订单统计</a>
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
                                <a class="change "  href="exportOrderhistory.aspx"><i class="change  fa fa-window-restore"></i>订单导入记录</a>
                            </dd>
                            <dd>
                                <a class="change "  href="payCode.aspx"><i class="change  fa fa-window-restore"></i>充值卡列表</a>
                            </dd>
                        </dl>
                    </div>
                    <section class="jiqlist">
                        <dl class="jiqlistTab">
                            <dd class="change ddcolor"><b>商品订单统计</b></dd>
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
                             <select id="brandList" class="selectpicker" style="width:120%;" multiple data-live-search="true" data-live-search-placeholder="请输入搜索" data-actions-box="true" onchange="brandChg()">
                              </select> 
                            </li>
                            <li>
                                <input type="button" value="查询" class="seachbtn"  onclick="sear()"/>
                            </li>
                            <li>
                                 <asp:Button  ID="excel" runat="server" OnClick="excel_Click"  Text="导出Excel" CssClass="seachbtn"/>
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
    function getBrandList() {
        $.ajax({
            type: "post",
            url: "Productform.aspx/getBrandList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{companyID:'" + $("#companyID").val() + "'}",
            success: function (data) {
                if (data.d.code == "200") {
                    var serverdata = $.parseJSON(data.d.db);
                    var serverdatalist = serverdata.length;

                    var optionString = "";
                    for (i = 0; i < serverdatalist; i++) {
                        optionString += "<option value=\'" + serverdata[i].id + "\'>" + serverdata[i].brandName + "</option>";
                    }
                    var CompanentId = "brandList";
                    var myobj = document.getElementById(CompanentId);

                    if (myobj.options.length == 0) {
                        $("#brandList").html(optionString);
                        $("#brandList").selectpicker('refresh');
                    }

                } else {
                    alert(data.d.msg);
                }

            }
        })
    }
    function mechineChg() {
        $("#_mechineList").val($("#mechineList").val());
    }
    function brandChg() {
        $("#_brandList").val($("#brandList").val());
    }
    function getMechineList() {
        $.ajax({
            type: "post",
            url: "Productform.aspx/getMechineList",
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
        getBrandList();
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
                  
                   + "<label style='width:12.5%'>商品名称</label>"
                   + "<label style='width:12.5%'>品牌</label>"
                   + "<label style='width:12.5%'>平均零售单价</label>"
                   + "<label style='width:12.5%'>零售销量</label>"
                   + "<label style='width:12.5%'>零售总金额</label>"
                   + "<label style='width:12.5%'>零售用户数</label>"
                   + "<label style='width:12.5%'>订购取货量</label>"
                   + "<label style='width:12.5%'>总计</label>"
                   + "</li>").appendTo("#ull");
        $.ajax({
            type: "post",
            url: "Productform.aspx/sear",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{start:'" + $("#start").val() + "',end:'" + $("#end").val() + "',mechineID:'" + $("#mechineList").val() + "' ,pageCurrentCount:'" + $("#pageCurrentCount").val() + "',brandID:'" + $("#brandList").val() + "',companyID:'" + $("#companyID").val() + "'}",
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
                             + "   <span style='width:12.5%'>" + serverdata[i].proname + "</span>"
                             + "   <span style='width:12.5%'>" + serverdata[i].brandName + "</span>"
                             + "   <span style='width:12.5%'>" + serverdata[i].avgprice.toFixed(2) + "</span>"
                             + "   <span style='width:12.5%'>" + serverdata[i].lsNum + "</span>"
                             + "   <span style='width:12.5%'>" + serverdata[i].totalMoney.toFixed(2) + "</span>"
                             + "   <span style='width:12.5%'>" + serverdata[i].lsMemberNum + "</span>"
                             + "   <span style='width:12.5%'>" + serverdata[i].dgNum + "</span>"
                             + "   <span style='width:12.5%'>" + serverdata[i].totalNum + "</span>"
                            + "</li> ").appendTo("#ull");
                }
            }
        })
    }
    function judge() {
        $.ajax({
            type: "post",
            url: "Productform.aspx/judge",
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
