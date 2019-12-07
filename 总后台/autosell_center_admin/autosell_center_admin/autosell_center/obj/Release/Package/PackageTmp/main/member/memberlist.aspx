<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="memberlist.aspx.cs" Inherits="autosell_center.main.member.memberlist" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>会员列表-自动售卖终端中心控制系统</title>
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
                 isTime: false, //isClear:false,
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
</head>
<body>
    <form id="form1" runat="server">
        <div class="header"></div>
        <div class="main">
            <div class="main_list">
                <div class="common_title">
                    <h4>
                        <i class="fa fa-plus"></i>
                        <span>会员管理</span>
                    </h4>
                    
                </div>
                <div class="common_main">
                    <div class="navlist">
                        <dl>
                            <dt>会员管理<em class="fa fa-cog"></em></dt>
                            <dd>
                                <a class="change acolor"><i class="change icolor fa fa-user"></i>会员列表</a>
                            </dd>
                             <dd>
                                <a class="change" href="#" onclick="qx_judge('hyczjl')"><i class="change fa fa-user"></i>会员充值记录</a>
                            </dd>
                             <dd>
                                <a class="change"  href="#" onclick="qx_judge('hycztj')"><i class="change fa fa-user"></i>收入统计</a>
                            </dd>
                        </dl>
                    </div>
                    <section class="jiqlist">
                        <div class="memberjs">
                            <p>
                                <em class="fa fa-info"></em>
                                <span>当前会员总数:<i><%=totalMember %></i>。</span>
                                <span>今日新增会员:<i><%=todayMember %></i>。</span>
                                <span>昨日新增会员:<i><%=yesdayMember %></i>。</span>
                            </p>
                        </div>
                     

                           <div class="memberseach">
                            <h4>查询</h4>
                            <ul>
                                <li>
                                    <label>会员名称/手机号/ID</label>
                                    <input type="text" value="" placeholder="" id="keyword" runat="server" />
                                </li>
                                    <li>
                                    <label>企业</label>
                                    <asp:DropDownList ID="companyList" AutoPostBack="true" OnSelectedIndexChanged="list_SelectedIndexChanged"  runat="server" />
                                </li>
                                <li>
                                    <label>机器</label>
                                    <asp:DropDownList ID="companyList2" runat="server" />
                                </li>

                                <li>
                                    <label>注册时间</label>
                                    <input name="act_stop_timeks" type="text" id="start" runat="server" class="input" value="" placeholder="注册时间" readonly="true" />
                                </li>
                                <li>
                                    <span>-</span>
                                    <input name="act_stop_timeks" type="text" id="end" runat="server" class="input" value="" placeholder="注册时间" readonly="true" />
                                </li>
                                <li>
                                    <label>等级</label>
                                    <asp:DropDownList ID="memberdj" runat="server">
                                        <asp:ListItem Value="-1">全部</asp:ListItem>
                                        <asp:ListItem Value="0">游客</asp:ListItem>
                                        <asp:ListItem Value="1">普通会员</asp:ListItem>
                                        <asp:ListItem Value="2">白银会员</asp:ListItem>
                                        <asp:ListItem Value="3">黄金会员</asp:ListItem>
                                    </asp:DropDownList>
                                </li>
                                <li>
                                    <label>余额</label>
                                    <input type="text" value="" placeholder="" id="minMoney" runat="server" />
                                </li>
                                <li>
                                    <label>-</label>
                                    <input type="text" value="" placeholder="" id="maxMoney" runat="server" />
                                </li>
                                <li>
                                    <input type="button" value="查询" class="seachbtn" onclick="getMemberList()" />
                                </li>
                                <li>
                                    <asp:Button ID="excel" runat="server" OnClick="excel_Click" Text="导出Excel" CssClass="seachbtn" />
                                </li>
                                <li style="display:none;">
                                    <input type="button" value="发送模板消息" onclick="sendMsg()" class="seachbtn"/>
                                </li>
                            </ul>

                        </div>
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
        qx_judge('hylb');
        $("#li2").find("a").addClass("aborder");
        $(".readclass").click(function () {
            $(this).parent().find("dl").toggle();
        });
        $(".readdl").find("dd").click(function () {
            var $readClass = $(".readclass");
            var $raadDl = $(this).html();
            $readClass.val($raadDl);
            $(".readclass").parent().find("dl").hide();
        });
        getMemberList();
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
        getMemberList();
    }
    function pageChg() {
        $("#pageCurrentCount").val($("#pageSel").val());
        getMemberList();
    }
    function getMemberList()
    {
       
        $("#memberList").empty();
        $(" <li>"
                + "  <label style='width:8.3%'>会员ID</label>"
                + "  <label style='width:8.3%'>会员名称</label>"
                + "  <label style='width:8.3%'>等级</label>"
                + "  <label style='width:5%'>剩余天数</label>"
                + "  <label style='width:8.3%'>手机</label>"
                + "  <label style='width:3.3%'>次数</label>"
                + "  <label style='width:5.3%'>累计消费</label>"
                + "  <label style='width:5.3%'>累计储值</label>"
                + "  <label style='width:5.3%'>可用余额</label>"
                + "  <label style='width:8.3%'>登记时间</label>"
                + "  <label style='width:8.3%'>最后一次消费时间</label>"
                  + "  <label style='width:7%'>所属公司</label>"
                + "  <label style='width:8.3%'>所属机器</label>"
                + "  <label style='width:10%'>操作</label>"
                + "  </li>").appendTo("#memberList");
        $.ajax({
            type: "post",
            url: "memberlist.aspx/getMemberList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{keyword:'" + $("#keyword").val() + "',qy:'" + $("#companyList").val() + "',qy2:'" + $("#companyList2").val() + "',start:'" + $("#start").val() + "',end:'" + $("#end").val() + "',pageCurrentCount:'" + $("#pageCurrentCount").val() + "'}",
            success: function (data) {
               // alert(data);
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
                console.log(serverdata)
                
                for (var i = 0; i < serverdatalist; i++) {
                    var bh = "";
                    if (serverdata[i].bh == "null" || serverdata[i].bh == "" || serverdata[i].bh == null) {
                        bh = "暂无绑定";
                    } else {
                        bh = serverdata[i].bh;
                    }
                    var dj = serverdata[i].dj;
                    var gs = serverdata[i].companyID;
                    $(" <li>"
                 + "  <span style='width:8.3%'>" + serverdata[i].id + "</span>"
                 + "  <span style='width:8.3%'>" + serverdata[i].name + "</span>"
                 + "  <span style='width:8.3%'>" + (dj == "0" ? "游客" : (dj == "1" ? "普通会员" : (dj == "2" ? "白银会员" : (dj == "3" ? "黄金会员" : "游客")))) + "</span>"
                 + "  <span style='width:5.3%'>" + serverdata[i].hjhyDays + "</span>"
                 + "  <span style='width:8.3%'>" + serverdata[i].phone + "</span>"
                 + "  <span style='width:3.3%'>" + serverdata[i].consumeCount + "</span>"
                 + "  <span style='width:5.3%'>" + serverdata[i].sumConsume.toFixed(2) + "</span>"
                 + "  <span style='width:5.3%'>" + serverdata[i].sumRecharge.toFixed(2) + "</span>"
                 + "  <span style='width:5.3%'>" + serverdata[i].AvailableMoney.toFixed(2) + "</span>"
                 + "  <span style='width:8.3%'>" + serverdata[i].createDate.substring(0, 10) + "</span>"
                 + "  <span style='width:8.3%'>" + serverdata[i].LastTime + "</span>"
                   + "  <span style='width:7%'>" + serverdata[i].companyName+ "</span>"
                 + "  <span style='width:8.3%'>" + bh + "</span>"
                 + "  <span style='width:10%'>"
                 + "       <a style='color:#0094ff' href='xfmx.aspx?memberID=" + serverdata[i].id + "'>消费明细</a>"
                  + "       <a style='color:#0094ff' href='chgMoney.aspx?memberID=" + serverdata[i].id + "'>余额变动</a>"
                  //+ "      <a style='color:#0094ff' onclick='updateMember(\"" + serverdata[i].id + "\",\"" + serverdata[i].hjhyDays + "\",\"" + serverdata[i].phone + "\")'>修改</a>"
                 + "   </span>"
                 + "  </li>").appendTo("#memberList");
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
                    if (menuID == 'hyczjl') {//会员充值记录
                        location.href = "rechargelist.aspx";
                    }
                    if (menuID == 'hycztj') {//会员充值统计
                        location.href = "rechargetj.aspx";
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
                        //location.href = "memberlist.aspx";
                    }
                    if (menuID == 'glygl') {//管理员管理
                        location.href = "/main/Adminlist/adminlist.aspx";
                    }
                    if (menuID == 'cplb') {//产品管理
                        location.href = "/main/product/productlist.aspx";
                    }
                    if (menuID == 'dgjl') {//订单管理
                        location.href = "/main/product/dglist.aspx";
                    }
                    if (menuID == 'zhcx') {//数据统计与查询
                        location.href = "/main/datastatistics/Statisticalquery.aspx";
                    }
                    if (menuID == 'cjdtt') {//分析
                        location.href = "/main/Analysis/Analysis.aspx'";
                    }
                    if (menuID == 'sjdp') {//数据大屏
                        window.open("/main/Big_screen/big_screen.aspx");
                    }
                    if (menuID == 'spgl') {//广告管理
                        location.href = "/main/Advertisement/video.aspx";
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
