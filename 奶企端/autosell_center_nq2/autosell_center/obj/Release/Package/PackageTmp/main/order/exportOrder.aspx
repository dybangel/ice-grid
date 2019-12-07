<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="exportOrder.aspx.cs" Inherits="autosell_center.main.order.exportOrder" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link rel="shortcut icon" href="/main/public/images/favicon.ico" />
    <link href="/main/public/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/common.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/style.css" rel="stylesheet" type="text/css" />

    <link rel="stylesheet" type="text/css" href="../bootstrapSelect/css/bootstrap.css" />
    <link rel="stylesheet" type="text/css" href="../bootstrapSelect/dist/css/bootstrap-select.css" />
    <script src="../bootstrapSelect/dist/js/jquery.js"></script>
    <script src="../public/script/jquery.form.js" type="text/javascript"></script>
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
                isTime: false, //isClear:false,
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
    <form id="form1" runat="server"  class="form-horizontal"  role="form" method="post" enctype="multipart/form-data">
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
                                <a class="change"  href="caigoushangpintj.aspx"><i class="change fa fa-window-restore"></i>采购分拣单据</a>
                            </dd>
                              <dd>
                                <a class="change"  href="historylist.aspx"><i class="change fa fa-window-restore"></i>历史清单</a>
                            </dd>
                             <dd>
                                <a class="change acolor"  href="exportOrder.aspx"><i class="change icolor fa fa-window-restore"></i>订购订单导入</a>
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
                            <dd class="change ddcolor"><b>订购订单导入</b></dd>
                        </dl>
                        <ul class="jiqlistseach">
                            
                             <li>
                                <input type="file" id="fileUpload" name="fileUpload" />
                                  <input id="fileText" type="text" class="myFileUpload" disabled="disabled" style="display:none"/>
                            </li>
                            <li>
                                <input type="button" id="export" value="查看" style="background-color:rgb(58,119,213);color:#fff" onclick="exportExcel()"/>
                            </li>
                          <%--   <li>
                                <input type="button" id="testData" value="检测数据" style="background-color:rgb(58,119,213);color:#fff" onclick="exportExcel()"/>
                            </li>--%>
                             <li>
                                <input type="button" id="exportData" value="导入订单" style="background-color:rgb(58,119,213);color:#fff" onclick="exportDataStr()"/>
                            </li>
                              <li>
                                <input type="button" id="testData" value="模板下载" style="background-color:rgb(58,119,213);color:#fff" onclick="downExcel()"/>
                            </li>
                              <li>
                              <label> phone:接收验证码手机号；</br>productCode：商品条码；</br>productPrice：单价；</br></label>
                              <label>zq：周期（只填数字）；</br>tjr：推荐人；</br>bz：备注</label>
                            </li>
                        </ul>

                        <ul class="memberlist" id="memberList">
                        </ul>
                         
                    </section>
                </div>
            </div>
        </div>
       
        <input id="companyID" runat="server" type="hidden" />
        <input id="_content" runat="server" type="hidden"/>
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
            data: "{operaID:'" + $("#agentID").val() + "',menuID:'dgdddr'}",
            success: function (data) {
                if (data.d.code == "500") {
                    $(".jiqlist").empty();
                    $(".jiqlist").html("<p class='noquanxian'>当前没有权限</p>").css({ "background": "#ddd" });
                }
            }
        })

    }
    function downExcel()
    {
        window.location.href = "http://nq.bingoseller.com/线下订奶码导入模板.xlsx";
    }
    let ary = new Array();//用于判断手机号是否重复
    function exportDataStr()
    {
        if ($("#_content").val()=="")
        {
            alert("导入的数据为空");
            return;
        }
        if (isRepeat(ary)) {
            if (confirm("存在相同的手机号是否继续导入")) {
                $.ajax({
                    type: "post",
                    url: "exportOrder.aspx/exportData",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: "{companyID:'" + $("#companyID").val() + "',str:'" + $("#_content").val() + "'}",
                    success: function (data) {
                        if (data.d.code == "200") {

                        }

                    }
                })
            }
        } else {
            $.ajax({
                type: "post",
                url: "exportOrder.aspx/exportData",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: "{companyID:'" + $("#companyID").val() + "',str:'" + $("#_content").val() + "'}",
                success: function (data) {
                    if (data.d.code == "200") {
                        alert(data.d.msg);
                    } else {
                        alert(data.d.msg);
                    }

                }
            })
        }
        
         
        
    }
    
    function exportExcel()
    {
        var options = {
            url: "../../api/ExportExcel.ashx",//处理程序路径  
            type: "post",
            dataType: 'json',
            data: {
                action: "exportExcel"
            },
            success: function (res) {
                console.log(res.db);
                //回调函数--请求成功  
                if (res.code == "200") {
                    $("#_content").val(JSON.stringify(res.db));
                    $("#memberList").empty();
                    $(" <li>"
                          + " <label style='width:16.6%'>手机号</label>"
                          + " <label style='width:16.6%'>商品条码</label>"
                          + " <label style='width:16.6%'>商品单价</label>"
                          + " <label style='width:16.6%'>订单周期</label>"
                          
                          + " <label style='width:16.6%'>推荐人</label>"
                          + " <label style='width:16.6%'>备注</label>"
                    + "  </li>").appendTo("#memberList");
                    var serverdata = res.db;
                    console.log(serverdata);
                    var serverdatalist = serverdata.length;
                    for (var i = 0; i < serverdatalist; i++) {
                        ary.push(serverdata[i].phone);
                        $(" <li>"
                            + "   <span style='width:16.6%'>" + serverdata[i].phone + "</span>"
                            + "   <span style='width:16.6%'>" + serverdata[i].productCode + "</span>"
                            + "   <span style='width:16.6%'>" + serverdata[i].productPrice + "</span>"
                            + "   <span style='width:16.6%'>" + serverdata[i].zq + "</span>" 
                            + "   <span style='width:16.6%'>" + serverdata[i].tjr + "</span>"
                            + "   <span style='width:16.6%'>" + serverdata[i].bz + "</span>"
                          
                            + "</li>").appendTo("#memberList");
                    }
                  
                } else {
                    alert(res.msg);
                }
            }
        };
        //将options传给ajaxForm  
        $('#form1').ajaxSubmit(options);
    }

    $(function () {
        $("#li2").find("a").addClass("aborder");
        judge()
       
    })
    function isRepeat(arr) {
        let hash = {};
        for (let i in arr) {
            if (hash[arr[i]]) {
                return true;
            }
            hash[arr[i]] = true;
        }
        return false;
    }
    function getList()
    {
        $("#memberList").empty();
        $(" <li>"
              + " <label style='width:14.2%'>手机号</label>"
              + " <label style='width:14.2%'>商品条码</label>"
              + " <label style='width:14.2%'>商品单价</label>"
              + " <label style='width:14.2%'>订单周期</label>"
              + " <label style='width:14.2%'>件数</label>"
              + " <label style='width:14.2%'>推荐人</label>"
              + " <label style='width:14.2%'>备注</label>"
        + "  </li>").appendTo("#memberList");
        $.ajax({
            type: "post",
            url: "historylist.aspx/getList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{companyID:'" + $("#companyID").val() + "',pageCurrentCount:'" + $("#pageCurrentCount").val() + "',start:'" + $("#start").val() + "',end:'" + $("#end").val() + "',operaList:'" + $("#operaList").val() + "',type:'" + $("#exceltype").val() + "'}",
            success: function (data) {
                if(data.d.code=="200")
                {
                    var serverdata = $.parseJSON(data.d.db);
                    var serverdatalist = serverdata.length;
                    for (var i = 0; i < serverdatalist; i++) {
                        $(" <li>"
                             + "  <span style='width:5%'>" + serverdata[i].Row + "</span>"
                            + "   <span style='width:19%'>" + serverdata[i].excelName + "</span>"
                            + "   <span style='width:19%'>" + serverdata[i].createTime + "</span>"
                            + "   <span style='width:19%'>" + (type=="1"?"采购单":(type=="2"?"拣货单":(type=="3"?"上货单":""))) + "</span>"
                            + "   <span style='width:19%'>" + serverdata[i].name + "</span>"
                            + "   <span style='width:19%'>"
                            + "       <a style='color:#0094ff' href='" + serverdata[i].downUrl + "')'>下载</a>"
                            + "   </span>"
                            + "</li>").appendTo("#memberList");
                    }
                }
               
            }
        })
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
        getList();
    }
    function pageChg() {
        $("#pageCurrentCount").val($("#pageSel").val());
        getList();
    }
    function getOperaList() {
        $.ajax({
            type: "post",
            url: "historylist.aspx/getOperaList",
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