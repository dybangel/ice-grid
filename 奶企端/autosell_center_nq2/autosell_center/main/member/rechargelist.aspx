<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rechargelist.aspx.cs" Inherits="autosell_center.main.member.rechargelist" %>

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
    <link  href="../../scripts/skin/jedate.css" rel="stylesheet" type="text/css"/>
    <script src="../../scripts/jedate.js" type="text/javascript"></script>
    <script src="../../scripts/jedate.min.js"></script>
     <script type="text/javascript">
         window.onload = function () {
             jeDate({
                 dateCell: "#start", //isinitVal:true,
                 //format: "YYYY-MM-DD",
                 isTime: true, //isClear:false,
                 choose: function(val) {},
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
                                <a class="change " href="memberlist.aspx" ><i class="change  fa fa-user"></i>会员列表</a>
                            </dd>
                           
                            <dd>
                                <a class="change acolor" href="rechargelist.aspx"><i class="change icolor fa fa-file-text"></i>会员充值记录</a>
                            </dd>
                            <dd>
                                <a class="change " href="rechargetj.aspx"><i class="change  fa fa-file-text"></i>收入统计</a>
                            </dd>
                            <dd>
                                <a class="change " href="memberdj.aspx"><i class="change  fa fa-file-text"></i>会员等级管理</a>
                            </dd>
                            <dd>
                                <a class="change " href="tqlist.aspx"><i class="change  fa fa-file-text"></i>特权管理</a>
                            </dd>
                    </dl>
                </div>
                <section class="jiqlist">
                    <dl class="jiqlistTab">
                        <dd class="change ddcolor"><b>订单列表</b></dd>
                    </dl>
                    <ul class="jiqlistseach">
                        <li>
                            <input type="text" value="" placeholder="会员昵称/ID/手机号" id="keyword"  runat="server"/>
                        </li>
                        <li>
                            <asp:DropDownList ID="payType" runat="server">
                                <asp:ListItem Value="0">全部</asp:ListItem>
                                <asp:ListItem value="3">微信支付</asp:ListItem>
                                <asp:ListItem value="5">充值卡</asp:ListItem>
                            </asp:DropDownList>
                        </li>
                        <li>
                             
                             <input name="act_stop_timeks" autocomplete="off" type="text" id="start"   runat="server"  class="input" value="" placeholder="开始时间"  readonly="true"  />
                        </li>
                        <li>
                             
                             <input name="act_stop_timeks" autocomplete="off" type="text" id="end"   runat="server"  class="input" value="" placeholder="结束时间"  readonly="true"  />
                        </li>
                          <li>
                            <input id="trxid" runat="server" placeholder="支付流水"/>
                            
                        </li>
                        <li>
                            <input type="button" value="查询" class="seachbtn" onclick="getOrderList()" />
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
         <input id="_operaID" runat="server" type="hidden" />
    </form>
</body>
</html>
<script>
    $(function () {
        //qx_judge('hyddgl');
        $("#li1").find("a").addClass("aborder");
        judge()
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
              + " <label style='width:9%'>序号</label>"
              + " <label style='width:13%'>会员ID</label>"
              + " <label style='width:13%'>会员名称</label>"
              + " <label style='width:13%'>手机号</label>"
              + " <label style='width:13%'>充值金额</label>"
              + " <label style='width:13%'>充值后</label>"
              + " <label style='width:13%'>支付时间</label>"
              + " <label style='width:13%'>流水号</label>"
        + "  </li>").appendTo("#memberList");
        $.ajax({
            type: "post",
            url: "rechargelist.aspx/getSear",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{start:'" + $("#start").val() + "',end:'" + $("#end").val() + "',keyword:'" + $("#keyword").val() + "',pageCurrentCount:'" + $("#pageCurrentCount").val() + "',companyID:'" + $("#companyID").val() + "',payType:'" + $("#payType").val() + "',trxid:'" + $("#trxid").val() + "'}",
            success: function (data) {
                var count = data.d.split('@@@')[0];
                if(parseInt(count)>=0)
                {
                    $("#pageSel").empty();
                    for (var k = 1; k <=parseInt(count) ;k++)
                    {
                        $("<option value='" + k + "'>" + k + "</option>").appendTo("#pageSel");
                    }
                }
                var serverdata = $.parseJSON(data.d.split('@@@')[1]);
                $("#pageTotalCount").val(count);
                var serverdatalist = serverdata.length;
              
                for (var i = 0; i < serverdatalist; i++) {
                    var zt = "";
                    $(" <li>"
                         + "   <span style='width:9%'>" + serverdata[i].Row + "</span>"
                         + "   <span style='width:13%'>" + serverdata[i].memberID + "</span>"
                         + "   <span style='width:13%'>" + serverdata[i].name + "</span>"
                          + "  <span style='width:13%'>" + serverdata[i].phone + "</span>"
                         + "   <span style='width:13%' >" + serverdata[i].money.toFixed(2) + "</span>"
                         + "   <span style='width:13%' >" + serverdata[i].afterMoney.toFixed(2) + "</span>"
                         + "   <span style='width:13%'>" + serverdata[i].paytime + "</span>"
                         + "   <span style='width:13%'>" + serverdata[i].trxid + "</span>"
                         + "</li>").appendTo("#memberList");
                }
            }
        })
    }
    function judge() {
        $.ajax({
            type: "post",
            url: "rechargelist.aspx/judge",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{operaID:'" + $("#_operaID").val() + "',menuID:'hyczjl'}",
            success: function (data) {
                if (data.d.code == "500") {
                    $(".jiqlist").empty();
                    $(".jiqlist").html("<p class='noquanxian'>当前没有权限</p>").css({ "background": "#ddd" });
                }
            }
        })

    }
</script>
