<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="smch.aspx.cs" Inherits="Consumer.main.smch" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
 <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>首页-自助售卖系统</title>
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
            </main>
        <input  id="_companyID" runat="server" type="hidden"/>
        <input  id="_mechineID" runat="server" type="hidden"/>
        <input id="_memberID" runat="server" type="hidden"/>
    </form>
</body>
</html>
<script>
    $(function () {
        getProductToday();
    })
    function offpopuper() {
        $(".popuper").removeClass("popuperTop");
        $(".popup").hide();
    }
    function gxproBtn(obj) {
        if ($(obj).parent().find(".zthidden").val() == "0") {
            $(obj).parent().find(".zthidden").val("1");
        } else if ($(obj).parent().find(".zthidden").val() == "1") {
            $(obj).parent().find(".zthidden").val("0");
        }
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
       
        okCH(productID, code, $("#_memberID").val(), $("#_mechineID").val());
        $(".popuper").removeClass("popuperTop");
        $(".popup").hide();
        //offpopuper();
    }
    function okCH(productID, code, memberID, mechineID) {
        $.ajax({
            type: "post",
            url: "smch.aspx/okCH",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{productID:'" + productID + "',code:'" + code + "',memberID:'" + memberID + "',mechineID:'" + mechineID + "'}",
            success: function (data) {
                if (data.d == "1") {
                    //进度条消失
                    window.location.href = "homeIndex.aspx";
                } else if (data.d == "2") {
                    //弹出 进度条
                    alert("当前机器正在出货请稍后。。。");

                }

            }
        })
    }
    function getProductToday() {
        $("#ull").empty();
        $.ajax({
            type: "post",
            url: "smch.aspx/getProductToday",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{memberID:'" + $("#_memberID").val() + "',mechineID:'" + $("#_mechineID").val() + "'}",
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
</script>
