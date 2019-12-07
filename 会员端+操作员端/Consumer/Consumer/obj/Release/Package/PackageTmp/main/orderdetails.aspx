<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="orderdetails.aspx.cs" Inherits="Consumer.main.orderdetails" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>我的订单-自助售卖系统</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />
    <link rel="shortcut icon" href="/main/public/images/favicon.ico" />
    <link href="/main/public/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/common.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/style.css" rel="stylesheet" type="text/css" />
    <script src="/main/public/script/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="/main/public/script/JavaScript.js" type="text/javascript"></script>
    <%--<link href="../scripts/skin/jedate.css" rel="stylesheet" type="text/css" />
    <script src="../scripts/jedate.js" type="text/javascript"></script>
    <script src="../scripts/jedate.min.js"></script>--%>
    <script src="/main/public/script/datePicker.js" type="text/javascript"></script>
    <%--<link href="/main/public/css/mobiscroll.custom.min.css" rel="stylesheet" type="text/css" />--%>
    <style>
        .orderdetalist ul li select {
            width: 60%;
            float: right;
            height: 36px;
            border: 0;
            border-bottom: 1px solid #eee;
            border-radius: 0;
            color: #666;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="paypageMain change">
            <h4>
                <i onclick="closeClick()">×</i>
                <img src="<%=headURL %>" alt="" />
                <span>请输入支付密码</span>
            </h4>
            <dl>
                <dt><%=companyName %></dt>
                <dd>¥  <%=totalMoney %></dd>
            </dl>
            <p>
                余额支付
                <em>当前余额：¥ <%=ye %></em>
            </p>
            <input type="password" value="" placeholder="支付密码..." id="isInput" />
            <input type="button" value="确定" id="isQDBtn" onclick="okPay()" />
        </div>
        <div class="paypageBg" onclick="closeClick()"></div>
        <div class="issell">
            <h4>当前出售价格为<em>0</em>元</h4>
            <p>确定转售此产品？</p>
            <dl>
                <dd>
                    <input type="button" value="确定" class="isSellok" onclick="isSellOK()" />
                </dd>
                <dd>
                    <input type="button" value="取消" onclick="offissell()" />
                </dd>
            </dl>
        </div>
        <div class="issellbg"></div>
        <main class="setupq">
            <div id="xuanzOne" class="Choice change">
                <i onclick="OffPopup()">×</i>
                <img src="/main/public/images/Icon/milk.png" alt="" />
                <p></p>
                <span>
                    <input type='text' id='demo_date' readonly="readonly" placeholder='选择配送日期' />
                </span>
                <dl>
                    <dd>
                        <a href="#" class="qdbtn"></a>
                    </dd>
                    <dd>
                        <a href="#"></a>
                    </dd>
                </dl>
            </div>
            <div id="xuanzTwo" class="Choice change">
                <i onclick="OffPopup()">×</i>
                <img src="/main/public/images/Icon/milk.png" alt="" />
                <p></p>
                <h4>
                    <label id="lblNewProduct" style="display: block; text-align: left; line-height: 32px;">当前产品：　<b id="lblTotalMoney" style="font-weight: normal; color: #666;">5*30=150</b>元</label>
                </h4>
                <h4>
                    <em>兑换产品：</em>
                    <select id="product" onchange="chgProduct()">
                    </select>
                </h4>
                <h4>
                    <em>兑换周期：</em>
                    <select id="zq" onchange="chgZQ()">
                    </select>
                </h4>
                <h4>
                    <label id="lblsyMoney" style="display: block; text-align: left; line-height: 32px;"></label>
                </h4>
                <dl>
                    <dd>
                        <a href="#" class="qdbtn" onclick="dh_pay()">确定兑换</a>
                    </dd>
                    <dd>
                        <a onclick="OffPopup()">取消</a>
                    </dd>
                </dl>
            </div>
            <div class="popup" onclick="OffPopup()"></div>
            <h4 class="commontitle">
                <i class="fa fa-angle-left" onclick="goBack()"></i>
                订单详情
            </h4>
            <section class="orderdetaTop">
                <a href="productdetails.aspx?productID=<%=productID %>&mechineID=<%=mechineID %>">
                    <div class="orderdetaimg">
                        <img src="<%=httpImgUrl %>" alt="" />
                    </div>
                    <div class="orderdetatext">
                        <h4><%=productName %></h4>
                        <p><%=description %></p>
                        <span>¥<%=double.Parse(totalMoney).ToString("f2") %></span>
                    </div>
                    <em class="fa fa-angle-right"></em>
                </a>
            </section>

            <section class="orderdate" id="calendar"></section>
            <section class="orderdetalist">
                <h4>取货信息</h4>
                <ul>
                    <li>
                        <label>取货时间</label>
                        <span><%=qhDate %></span>
                    </li>
                    <li>
                        <label>取货地址</label>
                        <span><%=qhAddress %></span>
                    </li>
                </ul>
                <h4>订单信息</h4>
                <ul>
                    <li>
                        <label>订单号</label>
                        <span><%=orderNO %></span>
                    </li>
                    <li>
                        <label>下单时间</label>
                        <span><%=createTime %></span>
                    </li>
                    <li>
                        <label>起送时间</label>
                        <span><%=qsDate %></span>
                    </li>
                    <li>
                        <label>止送时间</label>
                        <span><%=zdDate %></span>
                    </li>
                    <li id="zfzt">
                        <label>状态</label>
                        <span><b><%=fkzt=="0"?"待付款":"已付款" %></b></span>
                    </li>
                    <li>
                        <label>订购周期</label>
                        <span><%=pszq %>天</span>
                    </li>
                    <li>
                        <label>剩余天数</label>
                        <span><%=syNum %>天</span>
                    </li>
                </ul>
                <h4>客服</h4>
                <ul>
                    <li>
                        <label>客服电话</label>
                        <span><%=phone %></span>
                    </li>
                </ul>
                <h4 class="payStyle">支付方式</h4>
                <ul class="payStyle">
                    <li>
                        <label>支付方式</label>
                        <select id="paySelect">
                            <option value="3">微信支付</option>
                            <%-- <option value="4">余额支付</option>--%>
                        </select>
                    </li>
                </ul>
            </section>
            <div class="alipayBtn" id="ljzf">
                <input type="button" value="" id="createOrder" />
            </div>
            <!-- 将要被后台赋值的所有“待取货”的日期 -->
            <div id="allSendDate" runat="server" style="display: none;">
            </div>
            <!-- 将要被后台赋值的所有 “状态”日期（1-已完成；2-已过期；3-已转售；4-待取货；5-待配送） -->
            <!-- 已完成 -->
            <div id="completeOrder" runat="server" style="display: none;">
                <%=wcDate %>
            </div>
            <!-- 已过期 -->
            <div id="uneffectOrder" runat="server" style="display: none;">
                <%=sxDate %>
            </div>
            <!-- 已转售 -->
            <div id="sellOtherOrder" runat="server" style="display: none;">
                <%=yzsDate %>
            </div>
            <!-- 待取货 -->
            <div id="recievingOrder" runat="server" style="display: none;">
                <%=dqhDate %>
            </div>
            <!-- 待配送 -->
            <div id="sendingOrder" runat="server" style="display: none;">
                <%=selDate %>
            </div>
        </main>
        <input id="mechine_id" runat="server" type="hidden" />
        <input id="product_id" runat="server" type="hidden" />
        <input id="_pszq" runat="server" type="hidden" />
        <input id="_qsDate" runat="server" type="hidden" />
        <input id="_zdDate" runat="server" type="hidden" />
        <input id="_psStr" runat="server" type="hidden" />
        <input id="_psfs" runat="server" type="hidden" />
        <input id="_selDate" runat="server" type="hidden" />
        <input id="_orderNO" runat="server" type="hidden" />
        <input id="_createTime" runat="server" type="hidden" />
        <input id="_fkzt" runat="server" type="hidden" />
        <input id="_totalMoney" runat="server" type="hidden" />
        <input id="onDaydate" runat="server" type="hidden" />
        <input id="halfPrice" runat="server" type="hidden" />
        <input id="_sxDate" runat="server" type="hidden" />
        <input id="_wcDate" runat="server" type="hidden" />
        <input id="_dqhDate" runat="server" type="hidden" />
        <input id="member_ID" runat="server" type="hidden" />
        <input id="_yhfs" runat="server" type="hidden" />
        <input id="_syMoney" runat="server" type="hidden" />
        <input id="needMoney" runat="server" type="hidden" />
        <input id="flag" runat="server" type="hidden" />
        <input id="_proName" runat="server" type="hidden" />
        <input id="_companyID" runat="server" type="hidden" />
        <input id="_trxid" runat="server" type="hidden" />
    </form>
</body>
</html>
<script src="/main/public/script/sq_datecontrol.js"></script>
<!-- 选择日期 -->
<script>
    if (/MicroMessenger/.test(window.navigator.userAgent)) {
        //微信浏览器
        $("#flag").val(1);
    } else {

        //其他浏览器
        $("#flag").val(0);
    }
    $(function () {
        if ($("#_fkzt").val() == "0") {
            $("#createOrder").val("立即支付").attr({ "onclick": "pay()" })
        } else if ($("#_fkzt").val() == "1") {
            $("#createOrder").val("兑换").attr({ "onclick": "createDuihuan(this)" })
            $(".payStyle").hide();
        }
        var $zhuangB = $("#zfzt").find("b");
        if ($zhuangB.html() == "待付款") {
            $zhuangB.addClass("bdfk");
        } else if ($zhuangB.html() == "已转售") {
            $zhuangB.addClass("bdfk");
        }
        var mydate = new Date();
        var $weekNum = mydate.getDay(); //获取当前星期X(0-6,0代表星期天)
        var $liHtml = '';
        $("#dateUl").html($liHtml);
        //加载当月所有“待配送”
        loadCurrentMonthSend($("#allSendDate").html().trim());
        //加载当月所有日期的“状态”
        loadDayOrderStatus(mydate.getMonth() + 1);
        var zt = '<%=fkzt%>';

        if (zt == "1") {
            $("#ljfk").attr("style", "display:none;");
        } else {
            $("#ljfk").attr("style", "display:block;");
        }
        $(".issell").find("h4").find("em").html($("#halfPrice").val());
    });

    function OffPopup() {
        $(".Choice").removeClass("ChoiceTop");
        $(".popup").fadeOut();
    }
    function pay() {
        var thisVal = $('#paySelect').val();
        if (thisVal == '3') {
            
            <%--  var url = "orderpay.aspx?companyID=" + $("#_companyID").val() + "&openID=" + $("#_openID").val() + "&money=" + <%=double.Parse(totalMoney).ToString("f2") %> +"&proname=" + $("#_proName").val();
            url += "&mechine_id=" + $("#mechine_id").val() + "&product_id=" + $("#product_id").val() + "&_pszq=" + $("#_pszq").val() + "&_qsDate=" + $("#_qsDate").val() + "&_zdDate=" + $("#_zdDate").val() + "&_psStr=" + $("#_psStr").val();
            url += "&_psfs=" + $("#_psfs").val() + "&_selDate=" + $("#_selDate").val() + "&_orderNO=" + $("#_orderNO").val() + "&_createTime=" + $("#_createTime").val() + "&_fkzt=" + $("#_fkzt").val() + "&_totalMoney=" + $("#_totalMoney").val() + "&_yhfs=" + $("#_yhfs").val();
            location.href = url;--%>
            var url = "orderpay.aspx?companyID=" + $("#_companyID").val() + "&openID=" + $("#_openID").val() + "&money=" + <%=double.Parse(totalMoney).ToString("f2") %> +"&proname=" + $("#_proName").val();
            url += "&mechine_id=" + $("#mechine_id").val() + "&product_id=" + $("#product_id").val() + "&_pszq=" + $("#_pszq").val() + "&_qsDate=" + $("#_qsDate").val() + "&_zdDate=" + $("#_zdDate").val() + "&_psStr=" + $("#_psStr").val();
            url += "&_psfs=" + $("#_psfs").val() + "&_orderNO=" + $("#_orderNO").val() + "&_createTime=" + $("#_createTime").val() + "&_fkzt=" + $("#_fkzt").val() + "&_totalMoney=" + $("#_totalMoney").val() + "&_yhfs=" + $("#_yhfs").val();
            location.href = url;
        } else {
            $('.paypageMain').addClass('paypageMainTop');
            $('#isInput').focus();
            $('.paypageBg').fadeIn().css({ 'z-index': '1' });
        }
    }
    function closeClick() {
        $('.paypageMain').removeClass('paypageMainTop');
        $('#isInput').blur();
        $('.paypageBg').hide().css({ 'z-index': '0' });
    }
    function createOr() {
        $.ajax({
            url: "../api/asm.ashx",
            type: 'post',
            dataType: 'json',
            timeout: 10000,
            data: {
                action: "createOrder",
                mechine_id: $("#mechine_id").val(),
                product_id: $("#product_id").val(),
                _pszq: $("#_pszq").val(),
                _qsDate: $("#_qsDate").val(),
                _zdDate: $("#_zdDate").val(),
                _psStr: $("#_psStr").val(),
                _psfs: $("#_psfs").val(),
                _selDate: $("#_selDate").val(),
                _orderNO: $("#_orderNO").val(),
                _createTime: $("#_createTime").val(),
                _fkzt: $("#_fkzt").val(),
                _totalMoney: $("#_totalMoney").val(),
                _yhfs: $("#_yhfs").val(),
                _trxID: $("#_trxid").val()
            },
            success: function (resultData) {
                if (resultData.result == "1") {
                    alert("订单提交异常!");

                } else if (resultData.result == "2") {
                    alert("订单提交失败");
                }
                else if (resultData.result == "3") {
                    alert("当前没有登录");
                }
                else if (resultData.result == "4") {
                    alert("订单提交成功");
                }

                location.href = "homeIndex.aspx";
            }
        })
    }

    function createDuihuan() {
        //先判断订单状态
        $.ajax({
            type: "post",
            url: "orderdetails.aspx/pdzt",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{orderNO:'" + $("#_orderNO").val() + "'}",
            success: function (data) {
                if (data.d == "1") {
                    $(".popup").fadeIn();
                    $("#xuanzTwo").addClass("ChoiceTop");
                    getProductList();
                    getZQ();
                    getSYMoney();
                    chgProduct();
                } else if (data.d == "2") {
                    alert("当前状态无法兑换");
                }

            }
        })

    }

    function okSet() {
        var date = $("#onDaydate").val();
        var day = $("#demo_date").val().replace('/', '-').replace('/', '-');
        $.ajax({
            type: "post",
            url: "orderdetails.aspx/okSet",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{date:'" + date + "',day:'" + day + "',orderNO:'" + $("#_orderNO").val() + "'}",
            success: function (data) {
                if (data.d == "1") {
                    alert("当前状态无法延期");
                } else if (data.d == "2") {
                    alert("延期成功");
                } else if (data.d == "3") {
                    alert("延期失败");
                }
                location.reload();
            }
        })
    }

    function getProduct() {
        //先判断当前日期的状态是不是待取货4状态
        var date = $("#onDaydate").val();//选中日期
        $.ajax({
            type: "post",
            url: "orderdetails.aspx/getProduct",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{date:'" + date + "',orderNO:'" + $("#_orderNO").val() + "'}",
            success: function (data) {
                if (data.d == "4") {
                    location.href = "pickupcode.aspx";
                } else if (data.d == "3") {
                    alert("当前状态已出售无法取货");
                }

            }
        })
    }
    function getProductList() {
        $("#product").empty();
        $.ajax({
            type: "post",
            url: "orderdetails.aspx/getProductList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{mechineID:'" + $("#mechine_id").val() + "',productID:'" + $("#product_id").val() + "'}",
            success: function (data) {
                var serverdata = $.parseJSON(data.d);
                var serverdatalist = serverdata.length;
                $(" <option value='0'>请选择商品</option>").appendTo("#product");
                for (var i = 0; i < serverdatalist; i++) {
                    $(" <option value='" + serverdata[i].productID + "'>" + serverdata[i].proName + "</option>").appendTo("#product");
                }
            }
        })
    }
    function getZQ() {
        $("#zq").empty();
        $.ajax({
            type: "post",
            url: "orderdetails.aspx/getActivityList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{mechineID:'" + $("#mechine_id").val() + "'}",
            success: function (data) {
                var serverdata = $.parseJSON(data.d);
                var serverdatalist = serverdata.length;
                var str = "";
                for (var i = 0; i < serverdatalist; i++) {
                    if (serverdata[i].type == "1") {
                        str = "打" + serverdata[i].num + "折";
                    } else if (serverdata[i].type == "2") {
                        str = "赠送" + serverdata[i].num + "天";
                    } else {
                        str = "暂无设置";
                    }
                    var value = serverdata[i].zq;
                    var text = serverdata[i].zqName;
                    $("<option value='" + value + "'>" + "周期：" + text + "     ；优惠：" + str + "</option>").appendTo("#zq");

                }
            }
        })
    }
    function getSYMoney() {
        $.ajax({
            type: "post",
            url: "orderdetails.aspx/getSYMoney",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{orderNO:'" + $("#_orderNO").val() + "'}",
            success: function (data) {
                $("#_syMoney").val(data.d);
                $("#lblsyMoney").html("当前产品剩余价值：" + data.d);
            }
        })
    }
    function chgProduct() {
        $.ajax({
            type: "post",
            url: "orderdetails.aspx/getProductPrice2",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{productID:'" + $("#product").val() + "'}",
            success: function (data) {
                //获取周期的下拉
                var zqDay = $("#zq").val();
                var totalMoney = parseFloat(zqDay) * parseFloat(data.d);
                $("#lblTotalMoney").html((data.d) + "*" + zqDay + "=" + totalMoney);
                $("#needMoney").val(totalMoney);
            }
        })
    }
    function chgZQ() {
        chgProduct();
    }
    function dh_pay() {
        
        if ($("#product").val() == "0" || $("#product").val() == "undefined")
        {
            alert("请选择需要兑换的商品");
            return;
        }
        
        var yhfs = $("#zq").find("option:selected").text();
        if (yhfs.indexOf("赠送") > -1) {
            yhfs = yhfs.substring(yhfs.indexOf("赠送"), yhfs.length);
        } else if (yhfs.indexOf("折")) {
            yhfs = yhfs.substring(yhfs.indexOf("打"), yhfs.length);
        }
       
        var url = "dhpay.aspx?orderNO=" + $("#_orderNO").val() + "&syMoney=" + $("#_syMoney").val() + "&need_money=" + (parseFloat($("#needMoney").val())) + "&zq=" + $("#zq").val() + "&productID=" + $("#product").val() + "&yhfs=" + yhfs + "&mechineID=" + $("#mechine_id").val() + "&companyID=" + $("#mechine_id").val();
        location.href = url;
    }
    //function dh()
    //{
    //    var yhfs = $("#zq").find("option:selected").text();
    //    if (yhfs.indexOf("赠送") > -1) {
    //        yhfs = yhfs.substring(yhfs.indexOf("赠送"), yhfs.length);
    //    } else if (yhfs.indexOf("折")) {
    //        yhfs = yhfs.substring(yhfs.indexOf("打"), yhfs.length);
    //    }
    //    $.ajax({
    //        type: "post",
    //        url: "orderdetails.aspx/dh",
    //        contentType: "application/json; charset=utf-8",
    //        dataType: "json",
    //        data: "{orderNO:'" + $("#_orderNO").val() + "',syMoney:'" + $("#_syMoney").val() + "',need_money:'" + $("#needMoney").val() + "',zq:'" + $("#zq").val() + "',productID:'" + $("#product").val() + "',yhfs:'"+yhfs+"'}",
    //        success: function (data) {      
    //            if(data.d=="1")
    //            {
    //                alert("兑换成功");
    //            }else if(data.d=="2")
    //            {
    //                alert("当前状态无法兑换");
    //            }
    //            OffPopup();
    //        }
    //    })
    //}
    function getDateTime() {
        $.ajax({
            type: "post",
            url: "orderdetails.aspx/getDataTime",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{}",
            success: function (data) {
                alert(data.d);
            }
        })
    }
    function okPay() {
        $.ajax({
            type: "post",
            url: "orderdetails.aspx/yzPwd",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{memberID:'" + $("#member_ID").val() + "',money:'" + $("#_totalMoney").val() + "',pwd:'" + $("#isInput").val() + "',companyID:'" + $("#_companyID").val() + "',trxid:'" + $("#_trxid").val() + "'}",
            success: function (data) {
                if (data.d == "1") {
                    alert("支付密码不正确");
                } else if (data.d == "2") {
                    alert("余额不足");
                } else if (data.d == "3") {
                    createOr();

                }
            }
        })
    }
</script>
<script>
    var calendar = new datePicker();
    calendar.init({
        'trigger': '#demo_date', /*按钮选择器，用于触发弹出插件*/
        'type': 'date',/*模式：date日期；datetime日期时间；time时间；ym年月；*/
        'minDate': '1900-1-1',/*最小日期*/
        'maxDate': '2100-12-31',/*最大日期*/
        'onSubmit': function () {/*确认时触发事件*/
            var theSelectData = calendar.value;
        },
        'onClose': function () {/*取消时触发事件*/
        }
    });

</script>
<%--<script type="text/javascript" src="/main/public/script/mobiscroll.custom.min.js"></script>--%>
<script>
    function GetDateStrYear(AddDayCount, time) {

        var dd = convertDateFromString(time);

        dd.setDate(dd.getDate() + AddDayCount - 1);//获取AddDayCount天后的日期    
        var y = dd.getFullYear() - 1;
        var m = (dd.getMonth() + 1) < 10 ? "0" + (dd.getMonth() + 1) : (dd.getMonth() + 1);//获取当前月份的日期，不足10补0    
        var d = dd.getDate() < 10 ? "0" + dd.getDate() : dd.getDate();//获取当前几号，不足10补0    
        return y;
    }
    //输入的时间格式为yyyy-MM-dd
    function convertDateFromString(dateString) {
        if (dateString) {
            var date = new Date(dateString.replace(/-/, "/"))
            return date;
        }
    }
    function getPreMonth(date) {
        var arr = date.split('-');
        var year = arr[0]; //获取当前日期的年份
        var month = arr[1]; //获取当前日期的月份
        var day = arr[2]; //获取当前日期的日
        var days = new Date(year, month, 0);
        days = days.getDate(); //获取当前日期中月的天数
        var year2 = year;
        var month2 = parseInt(month) - 1;
        if (month2 == 0) {
            year2 = parseInt(year2) - 1;
            month2 = 12;
        }
        var day2 = day;
        var days2 = new Date(year2, month2, 0);
        days2 = days2.getDate();
        if (day2 > days2) {
            day2 = days2;
        }
        if (month2 < 10) {
            month2 = '0' + month2;
        }
        var t2 = year2 + '-' + month2 + '-' + day2;
        return month2;
    }
    function GetDateStrDay(AddDayCount, time) {
        var dd = convertDateFromString(time);
        dd.setDate(dd.getDate() + AddDayCount);//获取AddDayCount天后的日期    
        var y = dd.getFullYear();
        var m = (dd.getMonth() + 1) < 10 ? "0" + (dd.getMonth() + 1) : (dd.getMonth() + 1);//获取当前月份的日期，不足10补0    
        var d = dd.getDate() < 10 ? "0" + dd.getDate() : dd.getDate();//获取当前几号，不足10补0    
        return d;
    }
    var theme = "ios";
    var mode = "scroller";
    var display = "bottom";
    var lang = "zh";
    $('#demo_date').mobiscroll().date({
        theme: theme,
        mode: mode,
        display: display,
        lang: lang,
        minDate: new Date(GetDateStrYear(0, '<%=zdDate%>'), getPreMonth('<%=zdDate%>'), GetDateStrDay(1, '<%=zdDate%>')),
        maxDate: new Date(2050, 12, 31),
        stepMinute: 1
    });
    function issell() {
        $(".issell").fadeIn();
        $(".issellbg").fadeIn();
    }
    function offissell() {
        $(".issell").hide();
        $(".issellbg").hide();
    }
    function isSellOK() {
        var date = $("#onDaydate").val();//选中日期
        $.ajax({
            type: "post",
            url: "orderdetails.aspx/isSellOK",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{date:'" + date + "',orderNO:'" + $("#_orderNO").val() + "',sellPrice:'" + $("#halfPrice").val() + "'}",
            success: function (data) {
                if (data.d == "1") {
                    alert("当前日期不允许转售");
                } else if (data.d == "2") {
                    alert("转售成功，成功出售后金额将转到您的余额账户");
                } else if (data.d == "3") {
                    alert("当前机器操作员还未上货，请稍后转售");
                } else if (data.d == "4") {
                    alert("当前订单未支付，无法转售");
                }
                location.reload();
            }
        })
    }
</script>
