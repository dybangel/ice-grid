<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="xfmx.aspx.cs" Inherits="autosell_center.main.member.xfmx" %>
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
                    <span>会员管理</span>
                </h4>
            </div>
            <div class="common_main">
                <div class="navlist">
                    <dl>
                        <dt>订单管理<em class="fa fa-cog"></em></dt>
                           
                            <dd>
                                <a class="change acolor" href="#" onclick="qx_judge('hylb')"><i class="change icolor fa fa-user"></i>会员列表</a>
                            </dd>
                             <dd>
                                <a class="change " href="#" onclick="qx_judge('hyddgl')"><i class="change  fa fa-file-text"></i>会员订单管理</a>
                             </dd>
                             <dd>
                                <a class="change " href="#" onclick="qx_judge('hyczjl')"><i class="change  fa fa-file-text"></i>会员充值记录</a>
                             </dd>
                             <dd>
                                <a class="change " href="#" onclick="qx_judge('hycztj')"><i class="change  fa fa-file-text"></i>收入统计</a>
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
                        <dd class="change ddcolor"><b>订单列表</b></dd>
                    </dl>
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
        <input id="memberID" runat="server" type="hidden"/>
       
    </form>
</body>
</html>
<script> 
    $(function () {
      
        $("#li1").find("a").addClass("aborder");
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
              + " <label style='width:2%'>序号</label>"
                + " <label style='width:10%'>订单编号</label>"
                + " <label style='width:10%'>商品名称</label>"
                + " <label style='width:5%'>单价</label>"
                + " <label style='width:5%'>类型</label>"
                + " <label style='width:10%'>出货料道</label>"
                + " <label style='width:10%'>订单时间</label>"
                + " <label style='width:10%'>支付方式</label>"
                + " <label style='width:10%'>流水号</label>"
                + " <label style='width:10%'>配送机器</label>"
                + " <label style='width:10%'>交易状态</label>"
                + " <label style='width:8%'>操作</label>"
        + "  </li>").appendTo("#memberList");
        $.ajax({
            type: "post",
            url: "xfmx.aspx/getDataList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{'memberID':'" + $("#memberID").val() + "',pageCurrentCount:'" + $("#pageCurrentCount").val() + "'}",
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
                        zt = "微信公众号";
                    } else if (serverdata[i].payType == "4") {
                        zt = "会员卡余额支付";
                    }
                    if (serverdata[i].bz == "料道错误" || serverdata[i].bz == "交易序列号相同" || serverdata[i].bz == "料道故障" || serverdata[i].bz == "校验错误" || serverdata[i].bz == "出货失败") {
                        $(" <li>"
                            + "   <span style='width:2%'>" + serverdata[i].Row + "</span>"
                            + "   <span style='width:10%'>" + serverdata[i].orderNO + "</span>"
                            + "   <span style='width:10%'>" + serverdata[i].proname + "</span>"
                            + "   <span style='width:5%'>" + serverdata[i].totalMoney + "</span>"
                            + "   <span style='width:5%'>" + serverdata[i].stu + "</span>"
                            + "   <span style='width:10%'>" + serverdata[i].proLD + "</span>"
                            + "   <span style='width:10%'>" + serverdata[i].orderTime + "</span>"
                            + "   <span style='width:10%'>" + zt + "</span>"
                            + "   <span style='width:10%'>" + serverdata[i].billno + "</span>"
                            + "   <span style='width:10%'>" + serverdata[i].bh + "</span>"
                            + "   <span style='width:10%'>" + serverdata[i].bz + "</span>"
                            + "   <span style='width:8%'>"
                            + "       <a style='color:#0094ff' onclick='tk(\"" + serverdata[i].billno + "\")'>退款</a>"
                            + "   </span>"
                            + "</li>").appendTo("#memberList");
                    } else {
                        $(" <li>"
                         + "   <span style='width:2%'>" + serverdata[i].Row + "</span>"
                        + "   <span style='width:10%'>" + serverdata[i].orderNO + "</span>"
                        + "   <span style='width:10%'>" + serverdata[i].proname + "</span>"
                        + "   <span style='width:5%'>" + serverdata[i].totalMoney + "</span>"

                        + "   <span style='width:5%'>" + serverdata[i].stu + "</span>"
                        + "   <span style='width:10%'>" + serverdata[i].proLD + "</span>"
                        + "   <span style='width:10%'>" + serverdata[i].orderTime + "</span>"
                        + "   <span style='width:10%'>" + zt + "</span>"
                        + "   <span style='width:10%'>" + serverdata[i].billno + "</span>"
                        + "   <span style='width:10%'>" + serverdata[i].bh + "</span>"
                        + "   <span style='width:10%'>" + serverdata[i].bz + "</span>"
                        + "   <span style='width:8%'>"
                        + "       <a style='color:#999'>退款</a>"

                        + "   </span>"
                        + "</li>").appendTo("#memberList");
                    }


                }
            }
        })
    }
    function ok() {
        if ($("#_billno").val() == "") {
            alert("读取流水号失败，无法完成退款");
            return;
        }
        $.ajax({
            type: "post",
            url: "xfmx.aspx/ok",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{billno:'" + $("#_billno").val() + "',pwd:'" + $("#pwd2").val() + "',companyID:'" + $("#companyID").val() + "'}",
            success: function (data) {
                alert(data.d.msg);
                window.location.reload();
            }
        })
    }
    function tk(billno) {
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
