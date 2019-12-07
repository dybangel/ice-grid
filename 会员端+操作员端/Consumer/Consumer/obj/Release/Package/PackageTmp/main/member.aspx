<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="member.aspx.cs" Inherits="Consumer.main.member" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>会员中心-自助售卖系统</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />
    <link rel="shortcut icon" href="/main/public/images/favicon.ico" />
    <link href="/main/public/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/common.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/style.css" rel="stylesheet" type="text/css" />
    <script src="/main/public/script/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="/main/public/script/JavaScript.js" type="text/javascript"></script>
     <script>var _GetTokenError = ''; var wxinshare_title = ''; var wxinshare_desc = ''; var wxinshare_link = ''; var wxinshare_imgurl = '';; var fxShopName = ''</script>
    <script src="http://res.wx.qq.com/open/js/jweixin-1.0.0.js"></script>
    <script charset="utf-8" src="http://map.qq.com/api/js?v=2.exp&libraries=geometry"></script>
</head>
<body>
    <form id="form1" runat="server">
        <main class="main">
            <div class="popuper change">
                <h4>请选择要出货的产品</h4>
                <ul id="ull">
                </ul>
                <div class="popupBtn">
                    <input type="button" value="确定" class="qdokbtn" onclick="ok()" />
                    <input type="button" value="取消" onclick="offpopuper()" />
                </div>
            </div>
            <section class="memberTop">
                <a href="membersetup.aspx">
                    <img src="<%=headUrl %>"" alt="" />
                    <div>
                        <h4><%=name %></h4>
                        <p>
                            <i class="fa fa-mobile"></i><%=phone %>
                        </p>
                    </div>
                    <em class="fa fa-angle-right"></em>
                </a>
            </section>
            <section class="membermoney">
                <ul>
                    <li>
                        <span>
                            <img src="public/images/Icon/liyu.png" alt="" />余额：¥<%=money %>
                        </span>
                    </li>
                    <li>
                        <span>
                            <img src="public/images/Icon/liji.png" alt="" />积分：¥<%=point %>
                        </span>
                    </li>
                </ul>
            </section>
            <section class="memberlist">
                <ul>
                    <li>
                        <a href="purchase.aspx?companyID=<%=company_ID %>">
                            <img src="public/images/Icon/chongzhi.png" alt="" />
                            余额充值
                        <em class="fa fa-angle-right"></em>
                        </a>
                    </li>
                    <li>
                        <a href="transfer.aspx?companyID=<%=company_ID %>">
                            <img src="public/images/Icon/zhuanzhang.png" alt="" />
                            会员转账
                        <em class="fa fa-angle-right"></em>
                        </a>
                    </li>
                 <%--   <li>
                        <a href="integral.aspx">
                            <img src="public/images/Icon/jifen.png" alt="" />
                            积分变动
                        <em class="fa fa-angle-right"></em>
                        </a>
                    </li>--%>
                    <li>
                        <a href="balance.aspx">
                            <img src="public/images/Icon/yue.png" alt="" />
                            余额积分变动
                        <em class="fa fa-angle-right"></em>
                        </a>
                    </li>
                </ul>
                <ul>
                 <%--   <li>
                        <a href="resale.aspx">
                            <img src="public/images/Icon/zhuanmai.png" alt="" />
                            商品转售
                        <em class="fa fa-angle-right"></em>
                        </a>
                    </li>--%>
                    <li>
                        <a href="Resalerecord.aspx">
                            <img src="public/images/Icon/jilu.png" alt="" />
                            转售记录
                        <em class="fa fa-angle-right"></em>
                        </a>
                    </li>
                </ul>
                <ul>
                    <li>
                        <a onclick="Scan()">
                            <img src="public/images/Icon/sao.png" alt="" />
                            扫码取货
                        <em class="fa fa-angle-right"></em>
                        </a>
                    </li>
                    <li>
                        <a href="pickupcode.aspx">
                            <img src="public/images/Icon/qu.png" alt="" />
                            取货码取货
                        <em class="fa fa-angle-right"></em>
                        </a>
                    </li>
                </ul>
            </section>
        </main>
        <section class="homeNav"></section>
        <input id="member_ID" runat="server" type="hidden" />
        <input type="hidden" id="ur" runat="server" />
        <input type="hidden" id="ticket" runat="server" />
        <input type="hidden" id="te" runat="server" />
        <input type="hidden" id="appID" runat="server" />
        <input id="mechineID" runat="server" type="hidden" />
        <input id="companyID" runat="server" type="hidden" />
         <input  id="flag" runat="server" type="hidden"/>
    </form>
</body>
</html>
<script>
    if (/MicroMessenger/.test(window.navigator.userAgent)) {
        //微信浏览器

        $("#flag").val(1);

    } else {

        //其他浏览器
        $("#flag").val(0);
    }
      wx.config({
        debug: false, // 开启调试模式,调用的所有api的返回值会在客户端alert出来，若要查看传入的参数，可以在pc端打开，参数信息会通过log打出，仅在pc端时才会打印。
        //debug : true, // 开启调试模式,调用的所有api的返回值会在客户端alert出来，若要查看传入的参数，可以在pc端打开，参数信息会通过log打出，仅在pc端时才会打印。
        appId: '<%=app_id%>', // 必填，公众号的唯一标识
        timestamp: '<%=time%>', // 必填，生成签名的时间戳
        nonceStr: '<%=randstr%>', // 必填，生成签名的随机串
        signature: '<%=signstr%>',// 必填，签名，见附录1
        jsApiList: [
            'scanQRCode' //开启扫一扫功能，这里还可以添加更多的功能，比如微信支付
        ]
        // 必填，需要使用的JS接口列表，所有JS接口列表见附录2
      });
    function Scan() {
        wx.scanQRCode({
            needResult: 1, // 默认为0，扫描结果由微信处理，1则直接返回扫描结果，
            scanType: ["qrCode", "barCode"], // 可以指定扫二维码还是一维码，默认二者都有
            success: function (res) {
                //var mechineID = res.resultStr; // 当needResult 为 1 时，扫码返回的结果
                //$("#mechineID").val(mechineID)
                //getProductToday(mechineID);
                window.location.href = res.resultStr;
            }
        });
    }
    //根据机器id查询该会员在该机器下今天应该出的货
    function getProductToday(mechineID) {
        $("#ull").empty();
        $.ajax({
            type: "post",
            url: "member.aspx/getProductToday",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{memberID:'" + $("#member_ID").val() + "',mechineID:'" + mechineID + "'}",
            success: function (data) {
                $(".popuper").addClass("popuperTop");
                $(".popup").fadeIn();
                if (data.d == "1") {
                    alert("今天没有要取的商品");
                } else {
                    var serverdata = $.parseJSON(data.d);
                    var serverdatalist = serverdata.length;
                    for (var i = 0; i < serverdatalist; i++) {
                        $("<li>"
                        + "<input type='checkbox' value='' class='checkbox' onclick='gxproBtn(this)' />"
                        + "<input type='hidden' value='" + serverdata[i].productID + "' class='proID' />"
                        + "<input type='hidden' value='" + serverdata[i].code + "' class='proCode' />"
                        + "<input type='hidden' value='0' class='zthidden' />"
                        + "<div>"
                        + "    <span>" + serverdata[i].name + "</span>"
                        + "    <p>取货码：" + serverdata[i].code + "</p>"
                        + "</div>"
                        + "</li>").appendTo("#ull");
                    }
                }
            }
        })
    }
    function offpopuper() {
        $(".popuper").removeClass("popuperTop");
        $(".popup").hide();
    }
    function ok() {
        //获取选中的商品 id mechineID code  memberID
        var productID = "";
        var code = "";
        var liproID = $("#ull").find("li").find(".proID");
        liproID.each(function () {
            if ($(this).parent().find(".zthidden").val() == "1") {
                productID += $(this).val() + ",";
            }
        });
        productID = productID.substring(0, productID.length - 1);
        var liproCode = $("#ull").find("li").find(".proCode");
        liproCode.each(function () {
            if ($(this).parent().find(".zthidden").val() == "1") {
                code += $(this).val() + ",";
            }
        });
        code = code.substring(0, code.length - 1);
        okCH(productID, code, $("#member_ID").val(), $("#mechineID").val());
        $(".popuper").removeClass("popuperTop");
        $(".popup").hide();
         
    }
    function okCH(productID, code, memberID, mechineID) {
        $.ajax({
            type: "post",
            url: "member.aspx/okCH",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{productID:'" + productID + "',code:'" + code + "',memberID:'" + memberID + "',mechineID:'" + mechineID + "'}",
            success: function (data) {
                if (data.d == "1") {
                    //进度条消失
                } else if (data.d == "2") {
                    //弹出 进度条
                    alert("当前机器正在出货请稍后。。。");

                }

            }
        })
    }
    function gxproBtn(obj) {
        if ($(obj).parent().find(".zthidden").val() == "0") {
            $(obj).parent().find(".zthidden").val("1");
        } else if ($(obj).parent().find(".zthidden").val() == "1") {
            $(obj).parent().find(".zthidden").val("0");
        }
    }
    function getTodayProductCode() {
        $.ajax({
            type: "post",
            url: "homeIndex.aspx/getTodayProductCode",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{memberID:'" + $("#memberID").val() + "'}",
            success: function (data) {
                if (data.d == "1") {
                    location.href = "pickupcode.aspx";
                } else if (data.d == "2") {
                    alert("今日没有需要的取货");
                }
            }
        })
    }
</script>
