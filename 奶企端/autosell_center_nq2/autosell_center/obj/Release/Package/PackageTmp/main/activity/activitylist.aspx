<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="activitylist.aspx.cs" Inherits="autosell_center.main.activity.activitylist" %>

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
                 isTime: false, //isClear:false,
                 choose: function (val) { },
                 minDate: "2014-09-19 00:00:00"
             });
             jeDate({
                 dateCell: "#endTime", //isinitVal:true,
                 //format: "YYYY-MM-DD",
                 isTime: false, //isClear:false,
                 isinitVal:true,
                 choose: function (val) { },
                 minDate: "2014-09-19 00:00:00"
             });
             getOrderList();
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
                            <dt>活动设置<em class="fa fa-cog"></em></dt>
                            <dd>
                                <a class="change " href="activity.aspx"><i class="change  fa fa-video-camera"></i>设置活动</a>
                            </dd>
                              <dd>
                                <a class="change " href="#"><i class="change  fa fa-video-camera"></i>活动记录</a>
                            </dd>
                        </dl>
                    </div>
                <section class="jiqlist">
                      <div id="adminpopup" class="change zfpopup">
                            <h4>活动处理</h4>
                            <ul>
                                 <li style="display:none" id="li">
                                    <h5>处理时间</h5>
                                    <label style="border :0px;" id="lbl">
                                       2019-03-20 20：00：00
                                    </label>
                                </li>
                                <li>
                                    <h5>备注</h5>
                                    <label>
                                       <input  id="bz" placeholder="备注信息"/>
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

                    <dl class="jiqlistTab">
                        <dd class="change ddcolor"><b>活动记录</b></dd>
                    </dl>
                    <ul class="jiqlistseach">
                        <li>
                             
                             <input name="act_stop_timeks" autocomplete="off" type="text" id="start"   runat="server"  class="input" value="" placeholder="开始时间"  readonly="true"  />
                        </li>
                        <li>
                             <input name="act_stop_timeks" autocomplete="off" type="text" id="endTime"   runat="server"  class="input" value="" placeholder="结束时间"  readonly="true"  />
                        </li>
                        <li>
                            <input id="memberPhone" class="input" placeholder="会员电话"/>
                        </li>
                        <li>
                            <select id="activityType">
                                <option value="0">全部</option>
                                <option value="1">充值</option>
                                <option value="2">订购</option>
                            </select>
                        </li>
                         <li>
                            <select id="deltype">
                                 <option value="-1">全部</option>
                                <option value="0">未处理</option>
                                <option value="1">已处理</option>
                               
                            </select>
                        </li>
                        <li >
                            <input type="button" value="查询" class="seachbtn" onclick="getOrderList()" />
                        </li>
                         <li >
                            <input type="button" value="导出" class="seachbtn" onclick="excel()" />
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
        <input id="_id" runat="server" type="hidden"/>
         <input id="_operaID" runat="server" type="hidden" />
    </form>
</body>
</html>
<script> 
    function judge() {
        $.ajax({
            type: "post",
            url: "activitylist.aspx/judge",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{operaID:'" + $("#_operaID").val() + "',menuID:'szhdjl'}",
            success: function (data) {
                if (data.d.code == "500") {
                    $(".jiqlist").empty();
                    $(".jiqlist").html("<p class='noquanxian'>当前没有权限</p>").css({ "background": "#ddd" });
                }
            }
        })
    }
    function excel()
    {
        var url = "&start=" + $("#start").val() + "&end=" + $("#endTime").val() + "&keyword=" + $("#memberPhone").val() + "&activityType=" + $("#activityType").val() + "&deltype=" + $("#deltype").val() + "&companyID=" + $("#companyID").val();
        window.location.href = "../../api/ExportExcel.ashx?action=ExportActivityList" + url;
    }
    $(function () {
        //qx_judge('hyddgl');
        judge()
        $("#li6").find("a").addClass("aborder");
        //getOrderList();
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
                + " <label style='width:11.1%'>活动类别</label>"
                + " <label style='width:11.1%'>购买内容</label>"
                + " <label style='width:11.1%'>会员昵称</label>"
                + " <label style='width:11.1%'>会员电话</label>"
                + " <label style='width:11.1%'>订单金额</label>"
                + " <label style='width:11.1%'>参与时间</label>"
                + " <label style='width:11.1%'>活动奖励内容</label>"
                + " <label style='width:11.1%'>是否处理</label>"
                + " <label style='width:11.1%'>操作</label>"
        + "  </li>").appendTo("#memberList");
       
        $.ajax({
            type: "post",
            url: "activitylist.aspx/getSear",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{start:'" + $("#start").val() + "',end:'" + $("#endTime").val() + "',phone:'" + $("#memberPhone").val() + "',type:'" + $("#activityType").val() + "',companyID:'" + $("#companyID").val() + "',pageCurrentCount:'" + $("#pageCurrentCount").val() + "',deltype:'" + $("#deltype").val() + "'}",
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
                    var cssStr="";
                    if (serverdata[i].status=="1")
                    {
                        cssStr = "background: #C1FFC1";
                    }
                    $(" <li style='"+cssStr+"'>"
                         + "   <span style='width:11.1%'>" + (serverdata[i].type=="1"?"订购":"充值") + "</span>"
                         + "   <span style='width:11.1%'>" + serverdata[i].activityName + "</span>"
                         + "   <span style='width:11.1%'>" + serverdata[i].nickname + "</span>"
                         + "   <span style='width:11.1%'>" + serverdata[i].phone + "</span>"
                         + "   <span style='width:11.1%'>" + serverdata[i].totalMoney + "</span>"
                         + "   <span style='width:11.1%'>" + serverdata[i].partTime + "</span>"
                         + "   <span style='width:11.1%'>" + serverdata[i].activityContent + "</span>"
                         + "   <span style='width:11.1%'>" + (serverdata[i].status=="0"?"未处理":"已处理") + "</span>"
                         + "   <span style='width:11.1%'>"
                         + "     <a style='color:#0094ff' onclick='del(\"" + serverdata[i].id + "\",\"" + serverdata[i].status + "\",\"" + serverdata[i].delTime + "\",\"" + serverdata[i].bz + "\")'>" + (serverdata[i].status == "0" ? "处理" : "查看") + "</a>"
                         +"  </span>"
                         + "</li>").appendTo("#memberList");
                }
            }
        })
    }
    function del(id,status,time,bz)
    {
        if (status == "0") {
            $("#_id").val(id);
            $(".popupbj").fadeIn();
            $("#adminpopup").addClass("zfpopup_on");
        } else {
            $("#li").show();
            $("#_id").val(id);
            $("#lbl").html(time);
            $("#bz").val(bz);
            $(".popupbj").fadeIn();
            $("#adminpopup").addClass("zfpopup_on");
        }
       
    }
    function ok()
    {
        $.ajax({
            type: "post",
            url: "activitylist.aspx/ok",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{id:'" + $("#_id").val() + "',bz:'" + $("#bz").val() + "'}",
            success: function (data) {
                if (data.d.code == "200") {
                    window.location.reload();
                } else {
                    alert(data.d.msg);
                }
            }
        })
    }
    function qxClick() {
        $(".tangram-suggestion-main").hide();
        $(".popupbj").hide();
        $(".zfpopup").removeClass("zfpopup_on");
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
                    if (menuID == 'hylb') {//会员列表
                        location.href = "../member/memberlist.aspx";
                    }
                    if (menuID == 'hyczjl') {//会员充值记录
                        location.href = "../member/rechargelist.aspx";
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
