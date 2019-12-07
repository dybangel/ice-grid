<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="orderlist.aspx.cs" Inherits="autosell_center.main.product.orderlist" %>
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
       <link rel="stylesheet" type="text/css" href="../bootstrapSelect/css/bootstrap.css"/>
    <link rel="stylesheet" type="text/css" href="../bootstrapSelect/dist/css/bootstrap-select.css"/>
   <script src="../bootstrapSelect/dist/js/jquery.js"></script>
   
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
                                <a class="change " href="order.aspx"><i class="change  fa fa-user"></i>订购记录</a>
                            </dd>
                            <dd>
                                <a class="change acolor" href="orderlist.aspx"><i class="change icolor fa fa-file-text"></i>购买记录</a>
                            </dd>    
                            <dd>
                                <a class="change " href="Productform.aspx"><i class="change  fa fa-user"></i>商品订单统计</a>
                            </dd>
                         <dd>
                                <a class="change" href="paylist.aspx"><i class="change  fa fa-user"></i>支付记录</a>
                            </dd>
                        </dl>
                </div>
                <div id="addCaDiv" class="addDiv change">
                            <h4>请输入退款密码</h4>
                            <ul>
                                <li>
                                    <label>退款密码</label>
                                    <input type="text" value="" placeholder="" id="pwd2" runat="server"/>
                                </li>
                                <li>
                                    <label></label>
                                    <input type="button" value="确认" onclick="ok()" class="btnok"/>
                                    <input type="button" value="取消" class="btnoff" onclick="divOff()"/>
                                </li>
                            </ul>
                        </div>
                <section class="jiqlist">
                    <dl class="jiqlistTab">
                        <dd class="change ddcolor"><b>购买记录</b></dd>
                    </dl>
                    <ul class="jiqlistseach">
                         <li>
                                <asp:DropDownList ID="companyList" runat="server" onchange="haha()" ></asp:DropDownList>
                            </li>
                         <%--   <li>
                                <asp:DropDownList ID="companyList2" runat="server"></asp:DropDownList>
                            </li>--%>
                         <li>
                               <input name="act_stop_timeks" type="text" id="start"   runat="server"  class="input" value="" placeholder="开始日期"  readonly="true"  />
                         </li>
                         <li>
                               <input name="act_stop_timeks" type="text" id="end"   runat="server"  class="input" value="" placeholder="结束日期"  readonly="true"  />
                        </li>
                        <li>
                             <select id="mechineList"  class="selectpicker" multiple data-live-search="true" data-live-search-placeholder="请输入搜索" data-actions-box="true" onchange="mechineChg()">
                             </select> 
                        </li>
                        <li>
                             <select id="brandList" class="selectpicker" multiple data-live-search="true" data-live-search-placeholder="请输入搜索" data-actions-box="true" onchange="brandChg()">
                              </select> 
                        </li>
                        <li>
                           <%-- <select id="selType">
                                <option value="0">全部</option>
                                <option value="1">订购</option>
                                <option value="2">零售</option>
                            </select>--%>
                            <asp:DropDownList ID="selType" runat="server">
                                <asp:ListItem Value="0">全部</asp:ListItem>
                                <asp:ListItem Value="1">订购</asp:ListItem>
                                <asp:ListItem Value="2">零售</asp:ListItem>
                            </asp:DropDownList>
                        </li>
                        <li>
                            <input type="text" id="productName" runat="server" placeholder="产品名称、条码、简称"/>
                        </li>
                        <li>
                            <asp:DropDownList ID="ztlist" runat="server">
                                <asp:ListItem Value="0">全部</asp:ListItem>
                                <asp:ListItem Value="1">交易成功</asp:ListItem>
                                <asp:ListItem Value="2">料道错误</asp:ListItem>
                                <asp:ListItem Value="3">交易序列号相同</asp:ListItem>
                                <asp:ListItem Value="4">退款成功</asp:ListItem>
                                <asp:ListItem Value="5">出货失败</asp:ListItem>
                            </asp:DropDownList>
                        </li>
                         <li>
                            <input type="text" id="idORphone" runat="server" placeholder="会员ID、手机号"/>
                        </li>
                         <li>
                            <input type="text" id="orderNO" runat="server" placeholder="订单编号、支付流水号、支付ID"/>
                        </li>
                        <li>
                            <input type="button" value="查询" class="seachbtn" onclick="getOrderList2()" />
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
        <input id="pageCurrentCount" runat="server" type="hidden" value="1"/>
        <input id="pageTotalCount" runat="server" type="hidden" value="1"/>
        <input id="companyID" runat="server" type="hidden"/>
        <input id="_billno" runat="server" type="hidden"/>
        <input id="agentID" runat="server" type="hidden"/>
        <input id="_mechineList" runat="server" type="hidden"/>
        <input id="_brandList" runat="server" type="hidden"/>
    </form>
</body>
</html>
<script>
    function haha() {
        getMechineList();
        getBrandList();
        return false;
    }

    function getBrandList() {
        $.ajax({
            type: "post",
            url: "orderlist.aspx/getBrandList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{companyID:'" + $("#companyList").val() + "'}",
            success: function (data) {
                if (data.d.code == "200") {
                    $("#brandList").html('');
                    $("#brandList").selectpicker('refresh');
                    if (data.d.db == "")
                    {
                      
                        return;
                    }

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
            url: "orderlist.aspx/getMechineList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{companyID:'" + $("#companyList").val() + "'}",
            success: function (data) {
                if (data.d.code == "200") {
                    $("#mechineList").html('');
                    $("#mechineList").selectpicker('refresh');
                    if (data.d.db == "") {
                        return;
                    }
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
        //judge();
        getOrderList2();
        getMechineList();
        getBrandList();
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
         
        getOrderList2();
    }
    function pageChg() {
        $("#pageCurrentCount").val($("#pageSel").val());
        getOrderList2();
    }
    function getOrderList() {
        $("#memberList").empty();
        $(" <li>"
              + " <label style='width:7%'>序号</label>"
                +" <label style='width:10%'>订单编号</label>"
                + " <label style='width:10%'>商品名称</label>"
                + " <label style='width:5%'>单价</label>"
                + " <label style='width:5%'>类型</label>"
                + " <label style='width:5%'>出货料道</label>"
                + " <label style='width:5%'>会员ID</label>"
                + " <label style='width:8%'>支付宝ID</label>"
                + " <label style='width:10%'>订单时间</label>"
                + " <label style='width:5%'>支付方式</label>"
                + " <label style='width:10%'>流水号</label>"
                + " <label style='width:10%'>配送机器</label>"
                + " <label style='width:10%'>交易状态</label>"
                //+ " <label style='width:5%'>操作</label>"
        + "  </li>").appendTo("#memberList");
        $.ajax({
            type: "post",
            url: "orderlist.aspx/getOrderList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{'mechineID':'" + $("#mechineList").val() + "',companyID:'" + $("#companyList").val() + "',start:'" + $("#start").val() + "',end:'" + $("#end").val() + "',pageCurrentCount:'" + $("#pageCurrentCount").val() + "',ztlist:'" + $("#ztlist").find("option:selected").text() + "',brandList:'" + $("#brandList").val() + "',selType:'" + $("#selType").val() + "'}",
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
                    var zt = "";
                    if (serverdata[i].payType=="1")
                    {
                        zt = "微信扫码支付";
                    } else if (serverdata[i].payType == "2")
                    {
                        zt = "支付宝扫码";
                    } else if (serverdata[i].payType == "3") {
                        zt = "余额支付";
                    }  
                    if (serverdata[i].bz == "料道错误" || serverdata[i].bz == "交易序列号相同" || serverdata[i].bz == "料道故障" || serverdata[i].bz == "校验错误" || serverdata[i].bz == "出货失败") {
                        $(" <li>"
                             + "   <span style='width:7%'>" + serverdata[i].Row + "</span>"
                            + "   <span style='width:10%'>" + serverdata[i].orderNO + "</span>"
                            + "   <span style='width:10%'>" + serverdata[i].proname + "</span>"
                            + "   <span style='width:5%'>" + serverdata[i].totalMoney.toFixed(2) + "</span>"

                            + "   <span style='width:5%'>" + serverdata[i].stu + "</span>"
                            + "   <span style='width:5%'>" + serverdata[i].proLD + "</span>"
                             + "   <span style='width:5%'>" + serverdata[i].memberID + "</span>"
                              + "   <span style='width:5%'>" + serverdata[i].memberID + "</span>"
                            + "   <span style='width:10%'>" + serverdata[i].orderTime + "</span>"
                            + "   <span style='width:5%'>" + zt + "</span>"
                            + "   <span style='width:10%'>" + serverdata[i].billno + "</span>"
                            + "   <span style='width:10%'>" + serverdata[i].bh + "</span>"
                            + "   <span style='width:10%'>" + serverdata[i].bz + "</span>"
                            //+ "   <span style='width:5%'>"
                            //+ "       <a style='color:#0094ff' onclick='tk(\"" + serverdata[i].billno + "\")'>退款</a>"
                            //+ "   </span>"
                            + "</li>").appendTo("#memberList");
                    } else {
                        $(" <li>"
                         + "   <span style='width:7%'>" + serverdata[i].Row + "</span>"
                        + "   <span style='width:10%'>" + serverdata[i].orderNO + "</span>"
                        + "   <span style='width:10%'>" + serverdata[i].proname + "</span>"
                        + "   <span style='width:5%'>" + serverdata[i].totalMoney.toFixed(2) + "</span>"

                        + "   <span style='width:5%'>" + serverdata[i].stu + "</span>"
                        + "   <span style='width:5%'>" + serverdata[i].proLD + "</span>"
                          + "   <span style='width:5%'>" + serverdata[i].memberID + "</span>"
                        + "   <span style='width:10%'>" + serverdata[i].orderTime + "</span>"
                        + "   <span style='width:10%'>" + zt + "</span>"
                        + "   <span style='width:10%'>" + serverdata[i].billno + "</span>"
                        + "   <span style='width:10%'>" + serverdata[i].bh + "</span>"
                        + "   <span style='width:10%'>" + serverdata[i].bz + "</span>"
                        //+ "   <span style='width:8%'>"
                        //+ "       <a style='color:#999'>退款</a>"

                        //+ "   </span>"
                        + "</li>").appendTo("#memberList");
                    }

                   
                }
            }
        })
    }
    function getOrderList2() {
        $("#memberList").empty();
        $(" <li>"
              + " <label style='width:7%'>序号</label>"
               + " <label style='width:10%'>流水号</label>"
              + " <label style='width:8%'>订单编号</label>"
              + " <label style='width:10%'>商品名称</label>"
              + " <label style='width:5%'>单价</label>"
              + " <label style='width:5%'>类型</label>"
              + " <label style='width:5%'>出货料道</label>"
               + " <label style='width:5%'>会员ID</label>"
               + " <label style='width:10%'>支付ID</label>"
              + " <label style='width:10%'>订单时间</label>"
              + " <label style='width:5%'>支付方式</label>"
             
              + " <label style='width:10%'>配送机器</label>"
              + " <label style='width:10%'>交易状态</label>"
              //+ " <label style='width:5%'>操作</label>"
        + "  </li>").appendTo("#memberList");
        $.ajax({
            type: "post",
            url: "orderlist.aspx/getOrderList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{'mechineID':'" + $("#mechineList").val() + "',companyID:'" + $("#companyList").val() + "',start:'" + $("#start").val() + "',end:'" + $("#end").val() + "',pageCurrentCount:'" + $("#pageCurrentCount").val() + "',ztlist:'" + $("#ztlist").find("option:selected").text() + "',brandList:'" + $("#brandList").val() + "',selType:'" + $("#selType").val() + "',productName:'" + $("#productName").val() + "',idORphone:'" + $("#idORphone").val() + "',orderNO:'" + $("#orderNO").val() + "'}",
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
                    var zt = "";
                    if (serverdata[i].payType == "1") {
                        zt = "微信扫码支付";
                    } else if (serverdata[i].payType == "2") {
                        zt = "支付宝扫码";
                    } else if (serverdata[i].payType == "3") {
                        zt = "余额支付";
                    }
                    if (serverdata[i].bz == "料道错误" || serverdata[i].bz == "交易序列号相同" || serverdata[i].bz == "料道故障" || serverdata[i].bz == "校验错误" || serverdata[i].bz == "出货失败") {
                        $(" <li>"
                             + "   <span style='width:7%'>" + serverdata[i].Row + "</span>"
                               + " <span style='width:10%'>" + serverdata[i].billno + "</span>"
                            + "   <span style='width:8%'>" + serverdata[i].orderNO + "</span>"
                            
                            + "   <span style='width:10%'>" + serverdata[i].proname + "</span>"
                            + "   <span style='width:5%'>" + serverdata[i].totalMoney.toFixed(2) + "</span>"

                            + "   <span style='width:5%'>" + serverdata[i].stu + "</span>"
                            + "   <span style='width:5%'>" + serverdata[i].proLD + "</span>"
                             + "   <span style='width:5%'>" + serverdata[i].memberID + "</span>"
                              + "   <span style='width:10%'>"+ serverdata[i].acct + "</span>"
                            + "   <span style='width:10%'>" + serverdata[i].orderTime + "</span>"
                            + "   <span style='width:5%'>" + zt + "</span>"
                          
                            + "   <span style='width:10%'>" + serverdata[i].bh + "</span>"
                            + "   <span style='width:10%'>" + serverdata[i].bz + "</span>"
                            //+ "   <span style='width:5%'>"
                            //+ "       <a style='color:#0094ff' onclick='tk(\"" + serverdata[i].billno + "\")'>退款</a>"
                            //+ "   </span>"
                            + "</li>").appendTo("#memberList");
                    } else {
                        $(" <li>"
                         + "   <span style='width:7%'>" + serverdata[i].Row + "</span>"
                          + "   <span style='width:10%'>" + serverdata[i].billno + "</span>"
                        + "   <span style='width:8%'>" + serverdata[i].orderNO + "</span>"
                        + "   <span style='width:10%'>" + serverdata[i].proname + "</span>"
                        + "   <span style='width:5%'>" + serverdata[i].totalMoney.toFixed(2) + "</span>"

                        + "   <span style='width:5%'>" + serverdata[i].stu + "</span>"
                        + "   <span style='width:5%'>" + serverdata[i].proLD + "</span>"
                          + "   <span style='width:5%'>" + serverdata[i].memberID + "</span>"
                           + "   <span style='width:10%'>" + serverdata[i].acct + "</span>"
                        + "   <span style='width:10%'>" + serverdata[i].orderTime + "</span>"
                        + "   <span style='width:5%'>" + zt + "</span>"
                       
                        + "   <span style='width:10%'>" + serverdata[i].bh + "</span>"
                        + "   <span style='width:10%'>" + serverdata[i].bz + "</span>"
                        //+ "   <span style='width:5%'>"
                        //+ "       <a style='color:#999'>退款</a>"

                        //+ "   </span>"
                        + "</li>").appendTo("#memberList");
                    }


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
            data: "{billno:'" + $("#_billno").val() + "',pwd:'" + $("#pwd2").val() + "',companyID:'" + $("#companyList").val() + "'}",
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
    function judge() {
        $.ajax({
            type: "post",
            url: "orderlist.aspx/judge",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{operaID:'" + $("#agentID").val() + "',menuID:'gmjl'}",
            success: function (data) {
                if (data.d.code == "500") {
                    $(".jiqlist").empty();
                    $(".jiqlist").html("<p class='noquanxian'>当前没有权限</p>").css({ "background": "#ddd" });
                }
            }
        })

    }
</script>