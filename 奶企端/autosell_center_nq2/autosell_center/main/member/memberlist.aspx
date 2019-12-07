<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="memberlist.aspx.cs" Inherits="autosell_center.main.member.memberlist" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link rel="shortcut icon" href="/main/public/images/favicon.ico" />
    <link href="/main/public/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/common.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/style.css" rel="stylesheet" type="text/css" />
    <script src="/main/public/script/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="/main/public/script/Titlelist.js" type="text/javascript"></script>
    <link href="../../scripts/skin/jedate.css" rel="stylesheet" type="text/css" />
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
                <div id="adminpopup" class="change zfpopup">
                    <h4>信息修改</h4>
                    <ul>
                        <li>
                            <h5>体验天数</h5>
                            <label>
                                <input type="text" placeholder="黄金会员体验天数" id="days"/>
                            </label>
                        </li>
                         <li>
                            <h5>手机号</h5>
                            <label>
                               <input type="text" placeholder="手机号" id="phone"/>
                            </label>
                        </li>
                    </ul>
                    <dl>
                        <dd>
                            <input type="button" value="确定" class="popupqdbtn" onclick="ok()" />
                        </dd>
                        <dd>
                            <input type="button" value="取消" onclick="qxClick()" />
                        </dd>
                    </dl>
                </div>
                 <div id="adminSendMsg" class="change zfpopup">
                    <h4>发送模板消息</h4>
                    <ul>
                        <li>
                            <h5>会员ID</h5>
                            <label>
                                <input type="text" placeholder="多个会员ID 用，隔开" id="memberID"/>
                            </label>
                        </li>
                         <li style="display:none">
                            <h5>标题</h5>
                            <label>
                               <input type="text" placeholder="恭喜你中奖了！" id="title"/>
                            </label>
                        </li>
                         <li style="display:none">
                            <h5>活动</h5>
                            <label>
                               <input type="text" placeholder="今日幸运用户" id="activity"/>
                            </label>
                        </li>
                         <li style="display:none">
                            <h5>奖品</h5>
                            <label>
                               <input type="text" placeholder="1天黄金会员体验" id="prize"/>
                            </label>
                        </li>
                        <li>
                            <h5>赠送天数</h5>
                            <label>
                               <input type="text" placeholder="赠送黄金会员天数" id="zsdays"/>
                            </label>
                        </li>
                         <li style="display:none">
                            <h5>跳转链接</h5>
                            <label>
                               <input type="text" placeholder="跳转链接" id="url"/>
                            </label>
                        </li>
                    </ul>
                    <dl>
                        <dd>
                            <input type="button" value="确定" class="popupqdbtn" onclick="sendok()" />
                        </dd>
                        <dd>
                            <input type="button" value="取消" onclick="qxClick()" />
                        </dd>
                    </dl>
                </div>
                <div class="common_main">
                    <div class="navlist">
                        <dl>
                            <dt>会员管理<em class="fa fa-cog"></em></dt>
                            <dd>
                                <a class="change acolor" href="memberlist.aspx" ><i class="change icolor fa fa-user"></i>会员列表</a>
                            </dd>
                      
                            <dd>
                                <a class="change " href="rechargelist.aspx"><i class="change  fa fa-file-text"></i>会员充值记录</a>
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
                            <dd>
                                <a class="change " href="memberdjcontent.aspx"><i class="change  fa fa-file-text"></i>会员等级说明</a>
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
                                    <label>机器</label>
                                    <asp:DropDownList ID="companyList" runat="server" />
                                </li>

                                <li>
                                    <label>注册时间</label>
                                    <input name="act_stop_timeks" autocomplete="off" type="text" id="start" runat="server" class="input" value="" placeholder="注册时间" readonly="true" />
                                </li>
                                <li>
                                    <span>-</span>
                                    <input name="act_stop_timeks" autocomplete="off" type="text" id="end" runat="server" class="input" value="" placeholder="注册时间" readonly="true" />
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
                                <li>
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
        <input id="companyID" runat="server" type="hidden" />
        <input id="_operaID" runat="server" type="hidden" />
        <input id="_memberID" runat="server" type="hidden"/>
        <input id="pageCurrentCount" runat="server" type="hidden" value="1" />
        <input id="pageTotalCount" runat="server" type="hidden" value="1" />
    </form>
</body>
</html>
<script>
    function test() {
        $.ajax({
            type: "post",
            url: "http://192.168.2.81:8085/user/register",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{username:'zhangsan212',password:'s123456',phone:'1358'}",
            success: function (data) {
                if (data.d.code == "200") {
                    window.location.reload();
                } else {
                    alert(data.d.msg);
                }
            }
        })
    }
    function ok()
    {
        $.ajax({
            type: "post",
            url: "memberlist.aspx/updateMember",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{id:'" + $("#_memberID").val() + "',days:'"+$("#days").val()+"',phone:'"+$("#phone").val()+"'}",
            success: function (data) {
                if (data.d.code == "200") {
                    window.location.reload();
                } else {
                    alert(data.d.msg);
                }
            }
        })
    }
    function sendok()
    {
        $.ajax({
            type: "post",
            url: "memberlist.aspx/sendok",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{companyID:'" + $("#companyID").val() + "',memberID:'" + $("#memberID").val() + "',zsdays:'" + $("#zsdays").val() + "'}",
            success: function (data) {
                if (data.d.code == "200") {
                    alert(data.d.msg);
                    window.location.reload();
                } else {
                    alert(data.d.msg);
                }
            }
        })
    }
    function sendMsg()
    {
        $(".popupbj").fadeIn();
        $("#adminSendMsg").addClass("zfpopup_on");
    }
    $(function () {
        $("#li1").find("a").addClass("aborder");

        $(".readclass").click(function () {
            $(this).parent().find("dl").toggle();
        });
        $(".readdl").find("dd").click(function () {
            var $readClass = $(".readclass");
            var $raadDl = $(this).html();
            $readClass.val($raadDl);
            $(".readclass").parent().find("dl").hide();
        });
        judge();
        getMemberList();
    })
    function getPage(val) {
        if (val == "first") {
            $("#pageCurrentCount").val("1");
        } else if (val == "up") {
            $("#pageCurrentCount").val($("#pageCurrentCount").val() - 1);
        } else if (val == "down") {
            $("#pageCurrentCount").val($("#pageCurrentCount").val() + 1)
        } else if (val == "end") {

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
        getMemberList();
    }
    function pageChg() {
        $("#pageCurrentCount").val($("#pageSel").val());
        getMemberList();
    }
    function getMemberList() {
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
                 + "  <label style='width:8.3%'>所属机器</label>"
                 + "  <label style='width:17%'>操作</label>"
                 + "  </li>").appendTo("#memberList");
        $.ajax({
            type: "post",
            url: "memberlist.aspx/getMemberList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{keyword:'" + $("#keyword").val() + "',qy:'" + $("#companyID").val() + "',start:'" + $("#start").val() + "',end:'" + $("#end").val() + "',pageCurrentCount:'" + $("#pageCurrentCount").val() + "',mechineID:'" + $("#companyList").val() + "',dj:'" + $("#memberdj").val() + "',minMoney:'" + $("#minMoney").val() + "',maxMoney:'" + $("#maxMoney").val() + "'}",
            success: function (data) {
                var count = data.d.split('@@@')[0];
                if (parseInt(count) >= 0) {
                    $("#pageSel").empty();
                    for (var k = 1; k <= parseInt(count) ; k++) {
                        $("<option value='" + k + "'>" + k + "</option>").appendTo("#pageSel");
                    }
                }
                var serverdata = $.parseJSON(data.d.split('@@@')[1]);
                console.log(serverdata);
                $("#pageTotalCount").val(count);
                
                var serverdatalist = serverdata.length;

                for (var i = 0; i < serverdatalist; i++) {
                    var bh = "";
                    if (serverdata[i].bh == "null" || serverdata[i].bh == "" || serverdata[i].bh == null) {
                        bh = "暂无绑定";
                    } else {
                        bh = serverdata[i].bh;
                    }
                    var dj = serverdata[i].dj;
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
                    + "  <span style='width:8.3%'>" + serverdata[i].createDate.substring(0,10) + "</span>"
                    + "  <span style='width:8.3%'>" + serverdata[i].LastTime  + "</span>"
                    + "  <span style='width:8.3%'>" + bh + "</span>"
                    + "  <span style='width:17%'>"
                    + "       <a style='color:#0094ff' href='xfmx.aspx?memberID=" + serverdata[i].id + "'>消费明细</a>"
                     + "       <a style='color:#0094ff' href='chgMoney.aspx?memberID=" + serverdata[i].id + "'>余额变动</a>"
                     + "      <a style='color:#0094ff' onclick='updateMember(\"" + serverdata[i].id + "\",\"" + serverdata[i].hjhyDays + "\",\"" + serverdata[i].phone + "\")'>修改</a>"
                    + "   </span>"
                    + "  </li>").appendTo("#memberList");
                }
            }
        })
    }
    function updateMember(id,days,phone)
    {
        $("#days").val(days);
        $("#phone").val(phone);
        $("#_memberID").val(id);
        $(".popupbj").fadeIn();
        $("#adminpopup").addClass("zfpopup_on");
    }
    function qxClick() {
        $(".tangram-suggestion-main").hide();
        $(".popupbj").hide();
        $(".zfpopup").removeClass("zfpopup_on");
    }
     
    function judge()
    {
        $.ajax({
            type: "post",
            url: "memberlist.aspx/judge",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{operaID:'" + $("#_operaID").val() + "',menuID:'hylb'}",
            success: function (data) {
                if(data.d.code=="500")
                {
                    $(".jiqlist").empty();
                    $(".jiqlist").html("<p class='noquanxian'>当前没有权限</p>").css({ "background": "#ddd" });
                }
            }
        })
    }
</script>
