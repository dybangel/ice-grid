<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="paylist.aspx.cs" Inherits="autosell_center.main.equipment.paylist" %>

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
                                <a class="change " href="orderlist.aspx"><i class="change icolor fa fa-file-text"></i>购买记录</a>
                            </dd>    
                            <dd>
                                <a class="change " href="Productform.aspx"><i class="change  fa fa-user"></i>商品订单统计</a>
                            </dd>
                         <dd>
                                <a class="change acolor" href="paylist.aspx"><i class="change  fa fa-user"></i>支付记录</a>
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
                        <dd class="change ddcolor"><b>支付记录</b></dd>
                    </dl>
                    <ul class="jiqlistseach">
                         <li>
                                <asp:DropDownList ID="companyList" runat="server" onchange="haha()" ></asp:DropDownList>
                            </li>
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
                            <asp:DropDownList ID="fkzt" runat="server">
                                <asp:ListItem Value="-1">全部</asp:ListItem>
                                <asp:ListItem Value="0">未支付</asp:ListItem>
                                <asp:ListItem Value="1">已支付</asp:ListItem>
                                <asp:ListItem Value="2">交易退款</asp:ListItem>
                            </asp:DropDownList>
                        </li>
                        <li>
                            <asp:DropDownList ID="selType" runat="server">
                                <asp:ListItem Value="0">全部</asp:ListItem>
                                <asp:ListItem Value="1">充值</asp:ListItem>
                                <asp:ListItem Value="2">零售</asp:ListItem>
                                <asp:ListItem Value="3">订购</asp:ListItem>
                            </asp:DropDownList>
                        </li>
                       <li>
                           <input id="keyword" runat="server" placeholder="手机号、ID"/>
                       </li>
                         <li>
                           <input id="trxid" runat="server" placeholder="支付流水"/>
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
    
    function mechineChg() {
        $("#_mechineList").val($("#mechineList").val());
    }
     
    function getMechineList() {
        $.ajax({
            type: "post",
            url: "paylist.aspx/getMechineList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{companyID:'" + $("#companyList").val() + "'}",
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
        getOrderList2();
        getMechineList();
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
    function getOrderList2() {
        $("#memberList").empty();
        $(" <li>"
              + " <label style='width:7%'>序号</label>"
              + " <label style='width:9%'>流水号</label>"
              + " <label style='width:9%'>商品名称</label>"
              + " <label style='width:4%'>金额</label>"
              + " <label style='width:9%'>类型</label>" 
              + " <label style='width:4%'>会员ID</label>"
              + " <label style='width:7%'>手机号</label>"
              + " <label style='width:9%'>支付ID</label>"
              + " <label style='width:9%'>订单时间</label>"
              + " <label style='width:9%'>支付方式</label>"
              + " <label style='width:9%'>配送机器</label>"
              + " <label style='width:9%'>交易状态</label>"
              + " <label style='width:4%'>备注</label>"
        + "  </li>").appendTo("#memberList");
        $.ajax({
            type: "post",
            url: "paylist.aspx/getOrderList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{'mechineIDList':'" + $("#mechineList").val() + "',companyID:'" + $("#companyList").val() + "',start:'" + $("#start").val() + "',end:'" + $("#end").val() + "',pageCurrentCount:'" + $("#pageCurrentCount").val() + "',selType:'" + $("#selType").val() + "',fkzt:'" + $("#fkzt").val() + "',keyword:'" + $("#keyword").val() + "',trxid:'" + $("#trxid").val() + "'}",
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
                    var zt = "";
                    if (serverdata[i].payType == "1") {
                        zt = "微信扫码支付";
                    } else if (serverdata[i].payType == "2") {
                        zt = "支付宝扫码";
                    } else if (serverdata[i].payType == "4") {
                        zt = "余额支付";
                    } else if (serverdata[i].payType == "3") {
                        zt = "微信支付";
                    }
                    var type = serverdata[i].type;
                    var statu=serverdata[i].statu;
                    $(" <li>"
                        + "   <span style='width:7%'>" + serverdata[i].Row + "</span>"
                        + "   <span style='width:9%'>" + serverdata[i].trxid + "</span>"
                        + "   <span style='width:9%'>" + serverdata[i].proname + "</span>"
                        + "   <span style='width:4%'>" + serverdata[i].totalMoney.toFixed(2) + "</span>"
                        + "   <span style='width:9%'>" + (type=="1"?"充值":(type=="2"?"零售":(type=="3"?"订购":"")))+ "</span>"
                        + "   <span style='width:4%'>" + serverdata[i].memberID + "</span>"
                        + "   <span style='width:7%'>" + serverdata[i].phone + "</span>"
                        + "   <span style='width:9%'>" + serverdata[i].acct + "</span>"
                        + "   <span style='width:9%'>" + serverdata[i].paytime + "</span>"
                        + "   <span style='width:9%'>" + zt + "</span>"
                        + "   <span style='width:9%'>" + serverdata[i].bh + "</span>"
                        + "   <span style='width:9%'>" + (statu == "0" ? "未支付" : (statu == "1" ? "已支付" : (statu == "2" ? "已退款" : ""))) + "</span>"
                        + "   <span style='width:4%'>" + serverdata[i].bz + "</span>"
                        + "</li>").appendTo("#memberList");


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
    function judge() {
        $.ajax({
            type: "post",
            url: "paylist.aspx/judge",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{operaID:'" + $("#agentID").val() + "',menuID:'zfjl'}",
            success: function (data) {
                if (data.d.code == "500") {
                    $(".jiqlist").empty();
                    $(".jiqlist").html("<p class='noquanxian'>当前没有权限</p>").css({ "background": "#ddd" });
                }
            }
        })

    }
</script>