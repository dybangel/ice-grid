<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="homeIndex.aspx.cs" Inherits="Consumer.main.homeIndex" %>

<%--NUhf3DfhMuF97uz15QjH3ykOEB1YURoi--%>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>首页-冰格自助售卖系统</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" />
    <link rel="shortcut icon" href="/main/public/images/favicon.ico" />
    <link href="/main/public/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/common.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/style.css" rel="stylesheet" type="text/css" />
    <script src="/main/public/script/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="/main/public/script/JavaScript.js" type="text/javascript"></script>
    <script>var _GetTokenError = ''; var wxinshare_title = ''; var wxinshare_desc = ''; var wxinshare_link = ''; var wxinshare_imgurl = '';; var fxShopName = ''</script>
    <script src="http://res.wx.qq.com/open/js/jweixin-1.0.0.js"></script>
    <script charset="utf-8" src="http://map.qq.com/api/js?v=2.exp&libraries=geometry"></script>
    <!-- 轮播样式js -->
    <link href="/main/public/css/swiper.min.css" rel="stylesheet" />
    <script src="/main/public/script/swiper.min.js" type="text/javascript"></script>
    <script charset="utf-8" src="http://map.qq.com/api/js?v=2.exp" type="text/javascript"></script>
	<script src="http://open.map.qq.com/apifiles/2/4/79/main.js" type="text/javascript"></script>
	<style>
            #address{height: 31px;padding: 0 10px;}
            .map-seach{background: #50a4ec;padding: 5px 20px;color: #fff;display: inline-block;}
            .map-seach:active{background: rgba(80, 164, 236, 0.4);}
    </style>
</head>
<body>
    <form id="form1" runat="server">
       <%-- <div class="redEnvelopesOk">
            <span>恭喜您</span>
            <h4>新人红包领取成功</h4>
            <input type="button" value="我知道了" id="_isYes" onclick="isYes()" />
        </div>
        <section class="redEnvelopes redEnvelopesHeight change">
            <div class="envelopesBtn" onclick="envelopesBtn()"></div>
            <div class="envelopesTxt">
                <h1>恭喜您!</h1>
                <h4>获得新人红包</h4>
                <span id="hby">
                    0
                    <em>元</em>
                </span>
                <dl>
                    <dt>使用说明</dt>
                    <dd>领取红包后将立即到您的个人账户中</dd>
                    <dd>当您首次充值后会激活此红包</dd>
                </dl>
            </div>
            <b onclick="offEnvelopes()">X</b>
        </section>--%>
    <%--    <div class="popupBg"></div>--%>
        <div id="allmap" style="display: block"></div>
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
            <div class="popup"></div>
            <section class="homeTop">
                <div class="homeLun swiper-container">
                    <div class="swiper-wrapper">
                        <div class="swiper-slide" id="div1">
                            <a href="#">
                                 <img src="" alt=""  runat="server" id="url1"/>
                            </a>
                           
                        </div>
                        <div class="swiper-slide" id="div2">
                            <a href="#">
                                  <img src="" alt="" runat="server" id="url2"/>
                            </a>
                          
                        </div>
                        <div class="swiper-slide" id="div3" >
                            <a href="#">
                                 <img src="" alt="" runat="server" id="url3"/>
                            </a>
                        </div>
                    </div>
                    <div class="swiper-pagination"></div>
                    <div class="swiper-button-next"></div>
                    <div class="swiper-button-prev"></div>
                </div>
            </section>
            <section class="homeBtn">
                <ul>
                    <li>
                        <a onclick="Scan()">
                            <img src="/main/public/images/Icon/sao.png" alt="" />
                            <span>扫码取货</span>
                        </a>
                    </li>
                  <%--<li>
                        <a onclick="ScanPay()">
                            <img src="/main/public/images/Icon/sao.png" alt="" />
                            <span>扫码支付</span>
                        </a>
                    </li>--%>
                    <li>
                        <a href="#" onclick="getTodayProductCode()">
                            <img src="/main/public/images/Icon/qu.png" alt="" />
                            <span>取货码</span>
                        </a>
                    </li>
                    <li>
                       <a href="Purchase.aspx?companyID=<%=comid %>">
                       <%--  <a href="../WebForm1.aspx">--%>
                            <img src="/main/public/images/Icon/chong.png" alt="" />
                            <span>充值</span>
                        </a>
                    </li>
                    <li>
                        <a href="balance.aspx">
                            <img src="/main/public/images/Icon/xiao.png" alt="" />
                            <span>余额</span>
                        </a>
                    </li>
                   <li>
                        <a href="transfer.aspx">
                            <img src="/main/public/images/Icon/zhuan.png" alt="" />
                            <span>转账</span>
                        </a>
                    </li>
                </ul>
            </section>
            <section class="homeSlist">
                <h4>
                    <span><i></i>附近的售卖机<i></i></span>
                </h4>
                <ul id="uld">
                </ul>
            </section>
        </main>
        <section class="homeNav"></section>
        <input id="memberID" runat="server" type="hidden" />
        <input type="hidden" id="ur" runat="server" />
        <input type="hidden" id="ticket" runat="server" />
        <input type="hidden" id="te" runat="server" />
        <input type="hidden" id="appID" runat="server" />
        <input id="mechineID" runat="server" type="hidden" />
        <input id="companyID" runat="server" type="hidden" />
        <input  id="flag" runat="server" type="hidden"/>
        <input  id="_openID" runat="server" type="hidden"/>
        <input  id="_url1" runat="server" type="hidden"/>
        <input  id="_url2" runat="server" type="hidden"/>
        <input  id="_url3" runat="server" type="hidden"/>
        <input  id="_jlc" runat="server" type="hidden"/>
        <input id="_p11" runat="server" type="hidden"/>
        <input id="_p12" runat="server" type="hidden"/>
    </form>
</body>
</html>
<script>
    var swiper = new Swiper('.swiper-container', {
        pagination: '.swiper-pagination',
        nextButton: '.swiper-button-next',
        prevButton: '.swiper-button-prev',
        paginationClickable: true,
        //spaceBetween: 30,
        centeredSlides: true,
        autoplay: 3000,
        autoplayDisableOnInteraction: false
    });
</script>
<script>
    if (/MicroMessenger/.test(window.navigator.userAgent)) {
        //微信浏览器
        $("#flag").val(1);
    } else {
        //其他浏览器
        $("#flag").val(0);
    }
    $(function () {
        
        if ($("#_url1").val() == "1") {
            $("#div1").css("display", "block");
        } else {
            $("#div1").css("display", "none");
        }
        if ($("#_url2").val() == "1") {
            $("#div2").css("display", "block");
        } else {
            $("#div2").css("display", "none");
        }
        if ($("#_url3").val() == "1") {
           
            $("#div3").css("display", "block");
        } else {
            $("#div3").css("display", "none");
            
        }
        $(".btn_t").click(function () {
            var thisBtn = $(this);
            var divT = thisBtn.parent().find("div").index();
            if (divT > 0) {
                $(".menulist").addClass("leftH");
                $(".erlist").addClass("erlistop");
                $(".goback").addClass("goshow");
                thisBtn.parent().find("div").addClass("show");
                thisBtn.parent().find("dd").addClass("ddH");
            }
        });
        $(".goback").click(function () {
            $(".menulist").removeClass("leftH");
            $(".erlist").removeClass("erlistop");
            $(".goback").removeClass("goshow");
            $(".btn_t").parent().find("div").removeClass("show");
            $(".btn_t").parent().find("dd").removeClass("ddH");
        });
        wx.getLocation({
            type: 'gcj02', //默认为wgs84的gps坐标，如果要返回直接给openLocation用的火星坐标，可传入'gcj02'
            success: function (res) {
                latitude = res.latitude; // 纬度，浮点数，范围为90 ~ -90
                longitude = res.longitude; // 经度，浮点数，范围为180 ~ -180。
                var speed = res.speed; // 速度，以米/每秒计
                var accuracy = res.accuracy; // 位置精度
                setTimeout(function () { getMechineList(); }, 100);
            },
            cancel: function (res) {
                alert('拒绝将无法获取附近的机器');
            }
        });
        //getMechineList()
      //setTimeout(function () { getMechineList(); }, 1000);
    })
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
        okCH(productID, code, $("#memberID").val(), $("#mechineID").val());
        $(".popuper").removeClass("popuperTop");
        $(".popup").hide();
        //offpopuper();
    }
    wx.config({
        debug: false, // 开启调试模式,调用的所有api的返回值会在客户端alert出来，若要查看传入的参数，可以在pc端打开，参数信息会通过log打出，仅在pc端时才会打印。
        //debug : true, // 开启调试模式,调用的所有api的返回值会在客户端alert出来，若要查看传入的参数，可以在pc端打开，参数信息会通过log打出，仅在pc端时才会打印。
        appId: '<%=app_id%>', // 必填，公众号的唯一标识
        timestamp: '<%=time%>', // 必填，生成签名的时间戳
        nonceStr: '<%=randstr%>', // 必填，生成签名的随机串
        signature: '<%=signstr%>',// 必填，签名，见附录1
        jsApiList: [
            'scanQRCode', 'getLocation', 'openLocation' //开启扫一扫功能，这里还可以添加更多的功能，比如微信支付
        ]
        // 必填，需要使用的JS接口列表，所有JS接口列表见附录2
    });
    function Scan() {
        wx.scanQRCode({
            needResult: 1, // 默认为0，扫描结果由微信处理，1则直接返回扫描结果，
            scanType: ["qrCode", "barCode"],// 可以指定扫二维码还是一维码，默认二者都有
            success: function (res) {
                //var mechineID = res.resultStr;//当needResult 为 1 时，扫码返回的结果
                //$("#mechineID").val(mechineID)
                //getProductToday(mechineID);
                window.location.href = res.resultStr;
            }
        });
    }
    function ScanPay()
    {
        //首先判断该会员是否设置支付密码
        $.ajax({
            type: "post",
            url: "homeIndex.aspx/payPwd",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{openID:'" + $("#_openID").val() + "'}",
            success: function (data) {
                if (data.d == "1") {
                    wx.scanQRCode({
                        needResult: 1, // 默认为0，扫描结果由微信处理，1则直接返回扫描结果，
                        scanType: ["qrCode", "barCode"], // 可以指定扫二维码还是一维码，默认二者都有
                        success: function (res) {
                            var url = res.resultStr; // 当needResult 为 1 时，扫码返回的结果
                            location.href = url + "&openID=" + $("#_openID").val();
                        }
                    });
                } else {
                    //跳转支付密码设置
                    location.href = "paypage.aspx";
                }
            }
        })
        
    }
    var longitude, latitude;
    wx.ready(function () {
        wx.getLocation({
            type: 'gcj02', // 默认为wgs84的gps坐标，如果要返回直接给openLocation用的火星坐标，可传入'gcj02'
            success: function (res) {
                latitude = res.latitude; // 纬度，浮点数，范围为90 ~ -90
                longitude = res.longitude; // 经度，浮点数，范围为180 ~ -180。
               
                var speed = res.speed; // 速度，以米/每秒计
                var accuracy = res.accuracy; // 位置精度
                setTimeout(function () { getMechineList(); }, 100);
            },
            cancel: function (res) {
                alert('拒绝将无法获取附近的机器');
            }
        });
    });

    function openUU($lat,$lng)
    {
        wx.openLocation({
            latitude: $lat, // 纬度，浮点数，范围为90 ~ -90  
            longitude: $lng, // 经度，浮点数，范围为180 ~ -180。  
            name: '', // 位置名  
            address: '', // 地址详情说明  
            scale: 1, // 地图缩放级别,整形值,范围从1~28。默认为最大  
            infoUrl: '' // 在查看位置界面底部显示的超链接,可点击跳转  
        });
    }
    //function Convert_GCJ02_To_BD09($lat,$lng){
    //    $x_pi = 3.14159265358979324 * 3000.0 / 180.0;
    //    $x = $lng;
    //    $y = $lat;
    //    $z =sqrt($x * $x + $y * $y) + 0.00002 * sin($y * $x_pi);
    //    $theta = atan2($y, $x) + 0.000003 * cos($x * $x_pi);
    //    $lng = $z * cos($theta) + 0.0065;
    //    $lat = $z * sin($theta) + 0.006;
    //    return array('lng'=>$lng,'lat'=>$lat);
    //    }
    function gxproBtn(obj) {
        if ($(obj).parent().find(".zthidden").val() == "0") {
            $(obj).parent().find(".zthidden").val("1");
        } else if ($(obj).parent().find(".zthidden").val() == "1") {
            $(obj).parent().find(".zthidden").val("0");
        }
    }
    //根据机器id查询该会员在该机器下今天应该出的货
    function getProductToday(mechineID) {
        $("#ull").empty();
        $.ajax({
            type: "post",
            url: "homeIndex.aspx/getProductToday",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{memberID:'" + $("#memberID").val() + "',mechineID:'" + $("#mechineID").val() + "'}",
            success: function (data) {
                $(".popuper").addClass("popuperTop");
                $(".popup").fadeIn();
                if (data.d == "1") {
                    alert("今天没有要取的商品");
                } else {
                    var serverdata = $.parseJSON(data.d);
                    var serverdatalist = serverdata.length;
                    for (var i = 0; i < serverdatalist; i++) {
                        $("   <li>"
                        + " <input type='checkbox' value='' class='checkbox' onclick='gxproBtn(this)' />"
                        + " <input type='hidden' value='" + serverdata[i].productID + "' class='proID' />"
                        + " <input type='hidden' value='" + serverdata[i].code + "' class='proCode' />"
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
    function pdzt() {
        $.ajax({
            type: "post",
            url: "homeIndex.aspx/del",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{code:''}",
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
    function okCH(productID,code,memberID,mechineID)
    {
        $.ajax({
            type: "post",
            url: "homeIndex.aspx/okCH",
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
    function getMechineList()
    {
        
        $("#uld").empty();
        $.ajax({
            type: "post",
            url: "homeIndex.aspx/getMechineList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{companyID:'" + $("#companyID").val() + "'}",
            success: function (data) {
                var serverdata = $.parseJSON(data.d);
                var serverdatalist = serverdata.length;
                for (var i = 0; i < serverdatalist; i++) {
                    var start = new qq.maps.LatLng(latitude, longitude), end = new qq.maps.LatLng(serverdata[i].zdY, serverdata[i].zdX);
                    
                    var jlc = Math.round(qq.maps.geometry.spherical.computeDistanceBetween(start, end) * 10) / 10;  //需要显示多少范围之内的可以在这里限制 

                    if ($("#_jlc").val() != "") {
                        if (parseFloat($("#_jlc").val()) > parseFloat(jlc)) {
                            $(" <li>"
                               + "<section>"
                               + "<div class='homeLleft' onclick=\"openUU('" + serverdata[i].zdY + "','" + serverdata[i].zdX + "')\">"
                               + "            <img src='/main/public/images/ji1.jpg' alt='' />"
                               + "            <h2>编号" + serverdata[i].bh + "</h2>"
                               + "            <p>" + serverdata[i].addres + "</p>"
                               + "            <p>" + jlc + "m</p>"
                               + "</div>"
                               + "<div class='homeLright'>"
                               + "</div>"
                               + "</section>"
                               + "<a  href='productlist.aspx?mechineID=" + serverdata[i].id + "'>订购</a>"
                               + "</li>").appendTo("#uld");
                        }
                    } else {
                        $(" <li>"
                           + "<section>"
                           + "<div class='homeLleft' onclick=\"openUU('" + serverdata[i].zdY + "','" + serverdata[i].zdX + "')\">"
                           + "            <img src='/main/public/images/ji1.jpg' alt='' />"
                           + "            <h2>编号" + serverdata[i].bh + "</h2>"
                           + "            <p>" + serverdata[i].addres + "</p>"
                           + "            <p>" + jlc + "m</p>"
                           + "</div>"
                           + "<div class='homeLright'>"
                           + "</div>"
                           + "</section>"
                           + "<a  href='productlist.aspx?mechineID=" + serverdata[i].id + "'>订购</a>"
                           + "</li>").appendTo("#uld");
                    }
                   
                }
            }
        })
    }

    function envelopesBtn() {
        //$('.redEnvelopes').removeClass('redEnvelopesHeight')
        $('.redEnvelopes').css({'z-index': '1'})
        $('.redEnvelopesOk').show()
    }

    function isYes() {

        $.ajax({
            type: "post",
            url: "homeIndex.aspx/accetHB",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{memberID:'" + memberID + "'}",
            success: function (data) {
                if (data.d == "1") {
                    //进度条消失
                } else if (data.d == "2") {
                    //弹出 进度条
                    alert("当前机器正在出货请稍后。。。");

                }

            }
        })
        $('.popupBg').fadeOut()
        $('.redEnvelopes').removeClass('redEnvelopesHeight')
        setTimeout(function () {
            $('.redEnvelopesOk').hide()
            $('.redEnvelopes').css({ 'z-index': '11' })
        }, 200)
      
    }

    function offEnvelopes() {
        $('.popupBg').fadeOut()
        $('.redEnvelopes').removeClass('redEnvelopesHeight')
    }
</script>
 <script>
</script>
