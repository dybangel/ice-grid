<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="productlist.aspx.cs" Inherits="autosell_center.main.product.productlist" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>产品保质期-自动售卖终端中心控制系统</title>
    <link rel="shortcut icon" href="/main/public/images/favicon.ico" />
    <link href="/main/public/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/common.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/style.css" rel="stylesheet" type="text/css" />
    <script src="/main/public/script/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="/main/public/script/Titlelist.js" type="text/javascript"></script>
    <style>
        .productlist li span{
            overflow: hidden;
            text-overflow:ellipsis;
            white-space: nowrap;
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
                        <span>产品管理</span>
                    </h4>
                </div>
                <div class="common_main">
                    <div class="navlist">
                        <dl>
                            <dt>产品信息<em class="fa fa-cog"></em></dt>
                            <dd>
                                <a class="change acolor" href="#" onclick="qx_judge('cplb')"><i class="change icolor fa fa-file-text"></i>产品列表</a>
                            </dd>
                           <dd>
                                <a class="change" href="#" onclick="qx_judge('cplbie')"><i class="change fa fa-window-restore"></i>产品类别</a>
                            </dd>
                        </dl>
                      <%--  <dl>
                            <dt>规格与保质期<em class="fa fa-cog"></em></dt>
                            <dd>
                                <a class="change" href="standard.aspx"><i class="change fa fa-tags"></i>产品规格</a>
                            </dd>
                            <dd>
                                <a class="change"><i class="change fa fa-shield"></i>产品保质期</a>
                            </dd>
                        </dl>--%>
                    </div>
                    <section class="jiqlist">
                        <dl class="jiqlistTab">
                            <dd class="change ddcolor"><b>产品列表</b></dd>
                        </dl>
                        <ul class="jiqlistseach">
                            <li>
                                <input type="text" value="" placeholder="产品名称" id="productName" />
                            </li>
                            <li class="naiqBtn">
                                 <asp:DropDownList ID="companyList"  runat="server" />
                            </li>
                            <li>
                                <input type="button" value="查询" class="seachbtn"  onclick="search()"/>
                            </li>
                        </ul>
                        <ul class="productlist" id="product">
                           
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
        qx_judge('cplb');
        $("#li4").find("a").addClass("aborder");
        search();
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
        search();
    }
    function pageChg() {
        $("#pageCurrentCount").val($("#pageSel").val());
        search();
    }
    function search()
    {
        $("#product").empty();
        $(" <li>"
              //+ "   <label style='width:4%'>序号</label>"
                 +"   <label class='proname' style='width:30%'>产品名称</label>"
                 +"   <label style='width:10%'>产品类别</label>"
                 + "   <label style='width:10%'>所属奶企</label>"
                 + "   <label style='width:10%'>普通价格</label>"
                   + "   <label style='width:10%'>会员价格</label>"
                 + "   <label style='width:10%'>产品类型</label>"
                 + "   <label style='width:10%'>保质期</label>"
                 + "   <label style='width:10%'>规格</label>"
                 +"   </li>").appendTo("#product");
        $.ajax({
            type: "post",
            url: "productlist.aspx/getProductList",
            contentType: "application/json; charset=utf-8",
            data: "{keyword:'" + $("#productName").val() + "',qy:'" + $("#companyList").val() + "',pageCurrentCount:'" + $("#pageCurrentCount").val() + "'}",
            dataType: "json",
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
                    var gg = "";
                    if (serverdata[i].progg == "0") {
                        gg = "盒";
                    } else if (serverdata[i].progg == "1") {
                        gg = "瓶";

                    } else if (serverdata[i].progg == "2") {
                        gg = "包";
                    } else if (serverdata[i].progg == "3") {
                        gg = "袋";
                    } else if (serverdata[i].progg == "4") {
                        gg = "杯";
                    }
                    $(" <li>"
                          //+ "   <span style='width:4%'>" + serverdata[i].Row + "</span>"
                             + "   <span class='proname' style='width:30%'>"
                             +"       <em>"
                             + "           <img src='" + serverdata[i].httpImageUrl + "' />"
                             +"       </em>"
                             +"       <i>"+serverdata[i].proName+"</i>"
                             +"   </span>"
                             + "   <span style='width:10%'>液态</span>"
                             + "   <span style='width:10%'>" + serverdata[i].name + "</span>"
                             + "   <span style='width:10%'>" + serverdata[i].price1 + "</span>"
                              + "   <span style='width:10%'>" + serverdata[i].price2 + "</span>"
                             + "   <span style='width:10%'>" + serverdata[i].typeName + "</span>"
                             + "   <span style='width:10%'>" + serverdata[i].bzq + "天</span>"
                              + "   <span style='width:10%'>" + gg + "</span>"
                             //+"   <span class='procz'>"
                             //+"       <a onclick='upDete()'>修改</a>"
                             //+"       <a onclick='deLete()'>删除</a>"
                             //+"   </span>"
                            +"</li>").appendTo("#product");
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
                        location.href = "memberlist.aspx";
                    }
                    if (menuID == 'glygl') {//管理员管理
                        location.href = "/main/Adminlist/adminlist.aspx";
                    }
                    if (menuID == 'cplb') {//产品管理
                        //location.href = "/main/product/productlist.aspx";
                    }
                    if (menuID == 'cplbie') {//产品类别
                        location.href = "productclass.aspx";
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
