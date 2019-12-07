<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="caigoushangpintj.aspx.cs" Inherits="autosell_center.main.order.caigoushangpintj" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
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

  
    <style>
        .jiqlistseach {
            margin-bottom: 30px;
            border:2px solid #3A77D5;
            width:calc(100% - 60px);
            padding:0px;
            padding-right: 50px;
            color:#ffffff;
            margin-left:30px;
        }
        .jiqlistseach .title {
             width:calc(100% + 50px);
             background-color:#3A77D5;
             max-width:calc(100% + 50px);
             padding-left:20px;
        }
         .jiqlistseach .bt {
             width:15%;
             
             padding-left:50px;
             color: #000;
        }
        .jiqlistseach .content {
            width:25%;
            color:#333;
            max-width:initial;
            margin:0 auto;
            padding:0 0 0 50px;
        }
        .jiqlistseach .content h4{
            width:100%;
            font-size:1rem;
            height:30px;
            line-height:30px;
            display:inline-block;
        }
        .jiqlistseach .content .contentDiv {
            width:100%;
            float:left;
        }
        .jiqlistseach .content .contentDiv input, .jiqlistseach .content .contentDiv .btn-group {
            border-radius:0;
            float:left;
        }
        .jiqlistseach .content .contentDiv .btnClear, .jiqlistseach .content .contentDiv .btnSelect{
            height:36px;
            border:0;
            color:#fff;
            float:left;
            padding: 0 8px;
            position:relative;
            top:-3px;
        }
        .jiqlistseach .content .contentDiv .btnClear{
            background:#FB5557;
        }
        .jiqlistseach .content .contentDiv .btnSelect{
            background:#3A77D5;
        }
        .jiqlistseach .content .contentDiv .button{
            border-radius:0;
        }
        .jiqlistseach .contentRight{
            float:right;
        }
         .jiqlistseach .contentRight .button{
             position:relative;
             top:-3px;
         }
        .jiqlistseach .content .contentDiv .btnGreen{
            background:#47C9B3;
            margin-left:6px;
        }
        .btnGreen:active{
            background: #000000; 
            opacity: 0.5;    
        }
         .btnSelect:active{
            background: #000000; 
            opacity: 0.5;    
        }
        .btnClear:active{
            background: #000000; 
            opacity: 0.5;    
        }
        .operaGroup{
            width:100%;
            float:left;
            padding:0 30px;
            margin-top:16px;
        }
        .memberlist li h1, .operaGroup h1{
            width:100%;
            font-size:1.3rem;
            line-height:48px;
            color:#fff;
            padding:0 12px;
            background:#3A77D5;
        }
        .memberlist li span{
            height:36px;
            line-height:36px;
        }
        .operaGroup ul{
            margin:16px 0 0 0;
        }
        .operaGroup ul:nth-child(odd){
            float:left;
        }
        .operaGroup ul:nth-child(even){
            float:right;
        }
         .operaGroup ul li h2{
             width:100%;
             height:42px;
             padding:0 12px;
             background:#4b9fba;
         }
         .operaGroup ul li h2 span{
             float:left;
             font-size:1.2rem;
             border:0;
             color:#fff;
         }
         .operaGroup ul li h2 div{
             float:right;
         }
         .operaGroup ul li h2 div input{
             width:auto;
             display:inline-block;
             height:32px;
             margin:5px 0 5px 8px;
             border:0;
             background:#fff;
         }
          .dcjhd:active{
            background: #000000; 
            opacity: 0.5;    
        }
         .dcshd:active{
            background: #000000; 
            opacity: 0.5;    
        }
    </style>
     <script type="text/javascript">
            window.onload = function () {
                jeDate({
                    dateCell: "#start", //isinitVal:true,
                    format: "YYYY-MM-DD",
                    isTime: false, //isClear:false,
                    choose: function (val) { },
                    minDate: "2014-09-19 00:00:00"
                });
                jeDate({
                    dateCell: "#end",
                    format: "YYYY-MM-DD",
                    isinitVal: true,
                    isTime: true, //isClear:false,
                    minDate: "2014-09-19 00:00:00"
                });

                jeDate({
                    dateCell: "#lsTime",
                    //format: "YYYY-MM-DD",
                    isinitVal: true,
                    isTime: true, //isClear:false,
                    minDate: "2014-09-19 00:00:00"
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
                                <a class="change acolor"  href="caigoushangpintj.aspx"><i class="change icolor fa fa-window-restore"></i>采购分拣单据</a>
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
                    <ul class="jiqlistseach">
                       <li class="title">采购分拣单据</li>
                       <li class="content">
                           <h4>选择零售时间点</h4>
                           <div class="contentDiv">
                              <input name="act_stop_timeks" type="text" id="lsTime"   runat="server"  class="input isHasBtn" value="" placeholder="开始日期" />
                           <%--   <button class="btnClear" type="button" onclick="btnClearLsTime()">清除</button>--%>
                           </div>
                       </li>
                        <li class="content">
                           <h4>选择订购时间段</h4>
                           <div class="contentDiv">
                               <input name="act_stop_timeks" autocomplete="off" type="text" id="start"   runat="server"  class="input" value="" placeholder="开始日期"/>
                           </div>
                       </li>
                        <li class="content">
                           <h4>选择订购时间段</h4>
                           <div class="contentDiv">
                               <input name="act_stop_timeks" autocomplete="off" type="text" id="end"   runat="server"  class="input" value="" placeholder="开始日期"/>
                           </div>
                       </li>
                    
                        <li class="content">
                           <h4>选择品牌</h4>
                           <div class="contentDiv">
                                <select id="brandList" class="selectpicker" multiple data-live-search="true" data-live-search-placeholder="请输入搜索" data-actions-box="true">
                              
                                </select> 
                              <%-- <button class="btnSelect" type="button">选择品牌</button>--%>
                             <%--  <button class="btnClear" type="button" onclick="btnClearBrand()">清除</button>--%>
                           </div>
                       </li>
                       <li class="content">
                           <h4>选择配送员</h4>
                           <div class="contentDiv">
                                <select id="operaList" onchange="chgOpera()" class="selectpicker" multiple data-live-search="true" data-live-search-placeholder="请输入搜索" data-actions-box="true">
                                 
                                </select> 
                               <%--<button class="btnSelect" type="button">选择配送员</button>--%>
                             <%--<button class="btnClear" type="button">清除</button>--%>
                           </div>
                       </li>
                       <li class="content">
                           <h4>选择机器</h4>
                           <div class="contentDiv">
                                <select id="mechineList"  class="selectpicker" multiple data-live-search="true" data-live-search-placeholder="请输入搜索" data-actions-box="true">
                               
                                </select> 
                           
                           </div>
                       </li>
                        <li class="content">
                             <h4>产品名称、条码、简称</h4>
                           <div class="contentDiv">
                                <input type="text" runat="server" id="productName" />
                           
                           </div>
                        </li>
                        <li class="content contentRight">
                           <h4></h4>
                           <div class="contentDiv">
                               <button class="btnSelect" type="button" type="button" onclick="getOrderList()">搜索商品</button>
                              
                               <input type="button" value="导出采购单" class="button btnSelect btnGreen" onclick="exportCgd()"/>
                           </div>
                       </li>
                    </ul>
                    
                    <ul class="memberlist" id="memberList">
                       
                    </ul>
                 
                    <div class="operaGroup">
                        <h1>分拣单</h1>
                       <div id="fjdlist">
                           

                       </div>
                    </div>
                </section>
            </div>
        </div>
    </div>
        <div style="display: none">
            <a id="downloadRul" href="">
                <p>
                </p>
            </a>
        </div>
        <input id="pageCurrentCount" runat="server" type="hidden" value="1"/>
        <input id="pageTotalCount" runat="server" type="hidden" value="1"/>
        <input id="companyID" runat="server" type="hidden"/>
        <input id="_billno" runat="server" type="hidden"/>
        <input id="agentID" runat="server" type="hidden"/>
    </form>
</body>
</html>
<script>
    function judge() {
        $.ajax({
            type: "post",
            url: "Productform.aspx/judge",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{operaID:'" + $("#agentID").val() + "',menuID:'cgsptj'}",
            success: function (data) {
                if (data.d.code == "500") {
                    $(".jiqlist").empty();
                    $(".jiqlist").html("<p class='noquanxian'>当前没有权限</p>").css({ "background": "#ddd" });
                }
            }
        })

    }
    function exportCgd()
    {
        
        var url = "&lsTime=" + $("#lsTime").val() + "&dgStart=" + $("#start").val() + "&dgEnd=" + $("#end").val() + "&brandList=" + $("#brandList").val() + "&operaList=" + $("#operaList").val() + "&mechineList=" + $("#mechineList").val() + "&companyID=" + $("#companyID").val() + "&productName=" + $("#productName").val() + "&loginID=" + $("#agentID").val();
        window.location.href = "../../api/ExportExcel.ashx?action=ExportExcel1" + url;
    }
    function chgOpera()
    {
        getMechineList();
    }

    function exportJHD(operaID)
    {
        var url = "&operaID=" + operaID + "&companyID=" + $("#companyID").val() + "&lsTime=" + $("#lsTime").val() + "&dgStart=" + $("#start").val() + "&dgEnd=" + $("#end").val() + "&productName=" + $("#productName").val() + "&loginID=" + $("#agentID").val() + "&brandList=" + $("#brandList").val() + "&mechineIDList=" + $("#mechineList").val();
        window.location.href = "../../api/ExportExcel.ashx?action=ExportJHD" + url;
    }
    function exportSHD(operaID)
    {
        var url = "&operaID=" + operaID + "&companyID=" + $("#companyID").val() + "&lsTime=" + $("#lsTime").val() + "&dgStart=" + $("#start").val() + "&dgEnd=" + $("#end").val() + "&productName=" + $("#productName").val() + "&loginID=" + $("#agentID").val() + "&brandList=" + $("#brandList").val() + "&mechineIDList=" + $("#mechineList").val();
        window.location.href = "../../api/ExportExcel.ashx?action=exportSHD" + url;
    }
    function getFjdList(operaID)
    {
        $("#fjdlist").empty();
        $.ajax({
            type: "post",
            url: "caigoushangpintj.aspx/getFjdList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{operaID:'" + operaID + "',brandList:'" + $("#brandList").val() + "',mechineList:'" + $("#mechineList").val() + "',companyID:'" + $("#companyID").val() + "',productName:'" + $("#productName").val() + "',dgStart:'" + $("#start").val() + "',dgEnd:'" + $("#end").val() + "',lsTime:'" + $("#lsTime").val() + "',brandList:'" + $("#brandList").val() + "',mechinelist:'" + $("#mechineList").val() + "'}",
            success: function (data) {
                if (data.d.code == "200") {
                    var serverdata = $.parseJSON(data.d.db);
                    var serverdatalist = serverdata.length;
                    var list = "";
                    var ullistStr = " <ul class='memberlist' style='width:49%;'>"
                          +" <li>"
                          + "     <h2>"
                          + "         <span>当前配送员：" + data.d.name + "</span>"
                          + "         <div class='butGroup'>"
                          + "             <input type='button' value='导出拣货单' class='dcjhd' onclick='exportJHD(" + operaID + ")'/>"
                          + "             <input type='button' value='导出上货单' class='dcshd' onclick='exportSHD(" + operaID + ")'/>"
                          + "         </div>"
                          + "     </h2>"
                          + " </li>"
                          + " <li>"
                          + "     <label style='width:14.2%'>商品名称</label>"
                          + "     <label style='width:14.2%'>品牌</label>"
                          + "     <label style='width:14.2%'>规格</label>"
                          + "     <label style='width:14.2%'>包装</label>"
                          + "     <label style='width:14.2%'>零售补货量</label>"
                          + "     <label style='width:14.2%'>订购补货量</label>"
                          + "     <label style='width:14.2%'>总量</label>"
                          + " </li>";

                   
                    
                    for (i = 0; i < serverdatalist; i++) {
                        var bz = serverdata[i].sluid;
                        if (parseInt((serverdata[i].imbalance + serverdata[i].totalDG)) > 0)
                        {
                            list += " <li>"
                              + "     <span style='width:14.2%'>" + serverdata[i].proname + "</span>"
                              + "     <span style='width:14.2%'>" + serverdata[i].brandName + "</span>"
                              + "     <span style='width:14.2%'>" + serverdata[i].progg + "</span>"
                               + "    <span style='width:14.2%'>" + (bz == "1" ? "盒" : (bz == "2" ? "袋" : (bz == "3" ? "杯" : (bz == "4" ? "瓶" : (bz == "5" ? "个" : (bz == "6" ? "包" : "")))))) + "</span>"
                              + "     <span style='width:14.2%'>" + serverdata[i].imbalance + "</span>"
                              + "     <span style='width:14.2%'>" + serverdata[i].totalDG + "</span>"
                               + "   <span style='width:14.2%'>" + (serverdata[i].imbalance + serverdata[i].totalDG) + "</span>"
                              + " </li>";
                        }
                    }
                   
                    $(ullistStr + list + "</ul>").appendTo("#fjdlist");
                    
                } else {
                    
                }

            }
        })
    }
    function getBrandList()
    {
        $.ajax({
            type: "post",
            url: "caigoushangpintj.aspx/getBrandList",
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
                  
                    if (myobj.options.length == 0)
                    {
                        $("#brandList").html(optionString);
                        $("#brandList").selectpicker('refresh');
                    }
                   
                } else {
                    alert(data.d.msg);
                }
                
            }
        })
    }
    function getOperaList() {
        $.ajax({
            type: "post",
            url: "caigoushangpintj.aspx/getOperaList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{companyID:'" + $("#companyID").val() + "'}",
            success: function (data) {
                if (data.d.code == "200") {
                    var serverdata = $.parseJSON(data.d.db);
                    var serverdatalist = serverdata.length;

                    var optionString = "";
                    for (i = 0; i < serverdatalist; i++) {
                        optionString += "<option value=\'" + serverdata[i].id + "\'>" + serverdata[i].nickName + "</option>";
                    }
                 
                    var myobj = document.getElementById("operaList");

                    if (myobj.options.length == 0) {
                        $("#operaList").html(optionString);
                        $("#operaList").selectpicker('refresh');
                    }

                } else {
                    alert(data.d.msg);
                }

            }
        })
    }
    function getMechineList() {
        $.ajax({
            type: "post",
            url: "caigoushangpintj.aspx/getMechineList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{companyID:'" + $("#companyID").val() + "',operalist:'" + $("#operaList").val() + "'}",
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
        judge();
        setTimeout(function () {
            getMenuInfo();
        }, 100);
        setTimeout(function () {
            getOperaList();
        }, 200);

        setTimeout(function () {
            getMechineList();
        }, 300);
        setTimeout(function () {
            getBrandList();
        }, 400);
       
    })
    function getMenuInfo() { // 计算顶部按钮宽度
        for (var a = 0; a < $('.jiqlistseach').find('li.content').length; a++) {
            var thisLi = $('.jiqlistseach').find('li.content').eq(a);
            var thisInput = thisLi.find('.contentDiv input');
            var thisBootstrap = thisLi.find('.contentDiv').find('div').eq(0);
            var thisSelect = thisLi.find('.contentDiv .btnSelect');
            var thisClear = thisLi.find('.contentDiv .btnClear');
            if (thisLi.find('.btnSelect').length >= 1 && thisLi.find('.btnClear').length <= 0) {
                thisInput.width(thisLi.width() - 50 - thisSelect.width());
                thisBootstrap.width(thisLi.width() - 50 - thisSelect.width());
            }
            if (thisLi.find('.btnClear').length >= 1 && thisLi.find('.btnSelect').length <= 0) {
                thisInput.width(thisLi.width() - 50 - thisClear.width() - 20);
                thisBootstrap.width(thisLi.width() - 50 - thisClear.width());
            }
            if (thisClear && thisSelect) {
                thisInput.width(thisLi.width() - 50 - (thisClear.width() + thisSelect.width()) - 20);
                thisBootstrap.width(thisLi.width() - 50 - (thisClear.width() + thisSelect.width()) - 20);
            }
            if (thisLi.find('.btnSelect').length >= 2) {
                thisSelect.width((thisLi.width() - 90) / thisLi.find('.btnSelect').length)
            }

        }
    }
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
        $("<li><h1>采购单</h1></li> <li>"
              
                +" <label style='width:14.2%'>商品名称</label>"
                + " <label style='width:14.2%'>品牌</label>"
                + " <label style='width:14.2%'>规格</label>"
                 + " <label style='width:14.2%'>包装</label>"
                + " <label style='width:14.2%'>零售补货量</label>"
                + " <label style='width:14.2%'>订购补货量</label>"
                + " <label style='width:14.2%'>总量</label>"
        + "  </li>").appendTo("#memberList");
        $.ajax({
            type: "post",
            url: "caigoushangpintj.aspx/getOrderList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{lsTime:'" + $("#lsTime").val() + "',dgStart:'" + $("#start").val() + "',dgEnd:'" + $("#end").val() + "',brandList:'" + $("#brandList").val() + "',operaList:'" + $("#operaList").val() + "',mechineList:'" + $("#mechineList").val() + "',companyID:'" + $("#companyID").val() + "',productName:'" + $("#productName").val() + "'}",
            success: function (data) {
                
                if (data.d.code == "200") {

                    var serverdata = $.parseJSON(data.d.db);
                    var serverdatalist = serverdata.length;
                    for (var i = 0; i < serverdatalist; i++) {
                        var bz = serverdata[i].sluid;
                        //1盒2袋3杯4瓶5个6包
                        if ((serverdata[i].imbalance + serverdata[i].totalDG) > 0)
                        {
                        $(" <li>"
                                 + "  <span style='width:14.2%'>" + serverdata[i].proname + "</span>"
                                 + "  <span style='width:14.2%'>" + serverdata[i].brandName + "</span>"
                                 + "  <span style='width:14.2%'>" + serverdata[i].progg + "</span>"
                                 + "  <span style='width:14.2%'>" + (bz == "1" ? "盒" : (bz == "2" ? "袋" : (bz == "3" ? "杯" : (bz == "4" ? "瓶" : (bz == "5" ? "个" : (bz == "6" ? "包" : "")))))) + "</span>"
                                 + "  <span style='width:14.2%'>" + serverdata[i].imbalance + "</span>"
                                 + "  <span style='width:14.2%'>" + serverdata[i].totalDG + "</span>"
                                 + "  <span style='width:14.2%'>" + (serverdata[i].imbalance + serverdata[i].totalDG) + "</span>"
                                 + "</li>").appendTo("#memberList");
                        }

                    }
                    
                    var listStr = $("#operaList").val().toString();
                    var arr = listStr.split(",");
                    if (arr.length)
                    {
                        for (var j = 0; j < arr.length; j++)
                        {
                            getFjdList(arr[j])
                        }
                    }
                       
                } else {
                    alert(data.d.msg);
                }
               
            }
        })
    }
     
    function ok()
    {
        if ($("#_billno").val()=="")
        {
            alert("读取流水号失败，无法完成退款");
            return;
        }
        $.ajax({
            type: "post",
            url: "orderlist.aspx/ok",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{billno:'" + $("#_billno").val() + "',pwd:'" + $("#pwd2").val() + "',companyID:'" + $("#companyID").val() + "'}",
            success: function (data) {
                alert(data.d.msg);
                window.location.reload();
            }
        })
    }
    function tk(billno)
    {
        $("#_billno").val(billno);
        $("#addCaDiv").addClass("addDivshow");
        setTimeout(function () {
            $(".popupbj").fadeIn();
        }, 100);
        //验证该笔订单合理性
       
    }
    function divOff() {
        $(".popupbj").hide();
        $(".addDiv").removeClass("addDivshow");
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