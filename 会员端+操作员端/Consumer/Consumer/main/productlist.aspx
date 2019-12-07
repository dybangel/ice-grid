<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="productlist.aspx.cs" Inherits="Consumer.main.productlist" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>产品中心-自助售卖系统</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" />
    <link rel="shortcut icon" href="/main/public/images/favicon.ico" />
    <link href="/main/public/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/common.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/style.css" rel="stylesheet" type="text/css" />
    <script src="/main/public/script/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="/main/public/script/JavaScript.js" type="text/javascript"></script>
    <script src="/main/public/script/productJs.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="lodaing">
            <img src="public/images/loading.gif" alt="" />
            <span>加载中...</span>
        </div>
        <main class="setupq">
            <h4 class="commontitle">
                <i class="fa fa-angle-left" onclick="goBack()"></i>
                商品列表
            </h4>
            <section class="productTop">
                <img src="public/images/ji1.jpg" alt="" />
                <div>
                    <h4><%=name %></h4>
                    <p><%=address %></p>
                </div>
            </section>
            <section class="priductlist">
                <div class="product_hot">
                    <h2>热卖推荐</h2>
                    <div>
                        <ul id="ull2">
                        </ul>
                    </div>
                </div>
                <div class="product_main">
                    <div class="main_left" id="div1">
                    </div>
                    <div class="main_right">
                        <div class="main_list">
                            <h4>热销</h4>
                            <ul id="ull">
                            </ul>
                        </div>


                    </div>
                </div>
            </section>
        </main>
        <input id="mechine_id" runat="server" type="hidden" />
        <input id="company_ID" runat="server" type="hidden" />
    </form>
</body>
</html>
<script>
    $(function () {
        getType();
        getProductList(0);//0为全部
        getProductList2();
        setTimeout(function () {
            $(".lodaing").hide();
            var hotProduct = $("#ull2");
            var hotList = hotProduct.find("li");
            var hotlistW = hotProduct.find("li").width() + 16;
            var hotListNum = hotList.length;
            var newHotlist = hotlistW * hotListNum;
            $("#ull2").width(newHotlist);
        }, 1500);
    });
    function getType() {
        $("<a class='a_col' onclick='getProductList(0)'> <img src='/main/public/images/icon/hot.png' /> 全部</a>").appendTo("#div1");
        $.ajax({
            type: "post",
            url: "productlist.aspx/getProductType",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{companyID:'" + $("#company_ID").val() + "'}",
            success: function (data) {
                var serverdata = $.parseJSON(data.d);
                var serverdatalist = serverdata.length;
                for (var i = 0; i < serverdatalist; i++) {
                    $("<a class='a_col' onclick='getProductList(" + serverdata[i].productTypeID + ")'> <img src='/main/public/images/icon/hot.png' /> " + serverdata[i].typeName + "</a>").appendTo("#div1");
                }
            }
        })
    }
    function getProductList(typeID) {
        $("#ull").empty();
        $.ajax({
            type: "post",
            url: "productlist.aspx/getProductList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{typeID:'" + typeID + "',companyID:'" + $("#company_ID").val() + "'}",
            success: function (data) {
                var serverdata = $.parseJSON(data.d);
                var serverdatalist = serverdata.length;
                for (var i = 0; i < serverdatalist; i++) {
                    var zk = (serverdata[i].price2 / serverdata[i].price1) * 10;
                    $(" <li>"
                             + "       <a>"
                             + "          <div class='list_left'>"
                             + "              <img src='" + serverdata[i].httpImageUrl + "' />"
                             + "          </div>"
                             + "          <div class='list_right'>"
                             + "              <h3>" + serverdata[i].proName + "</h3>"
                             + "              <p>" + serverdata[i].description + "</p>"
                             + "              <label>"
                             + "                  <span>"
                             + "                      <em>¥" + serverdata[i].price2 + "</em>"
                             + "                      <i>¥" + serverdata[i].price1 + "</i>"
                             + "                  </span>"
                             + "                 <input type='button' class='btn' value='选购' onclick='buy(" + serverdata[i].productID + "," + $("#mechine_id").val() + ")'/>"
                             + "             </label>"
                             + "             <label><i class='fa fa-tag'></i>" + zk.toFixed(2) + "折</label>"
                             + "         </div>"
                             + "     </a>"
                             + " </li>").appendTo("#ull");
                }
            }
        })
    }
    //顶部
    function getProductList2() {
        $.ajax({
            type: "post",
            url: "productlist.aspx/getProductList2",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{companyID:'" + $("#company_ID").val() + "'}",
            success: function (data) {
                var serverdata = $.parseJSON(data.d);
                var serverdatalist = serverdata.length;
                for (var i = 0; i < serverdatalist; i++) {
                    $(" <li>"
                             + "   <img src='" + serverdata[i].httpImageUrl + "' />"
                             + "   <h4>" + serverdata[i].proName + "</h4>"
                             + "   <p>"
                             + "       <span>¥" + serverdata[i].price1 + "</span>"
                             + "       <a href='productdetails.aspx?productID=" + serverdata[i].productID + "&mechineID=" + $("#mechine_id").val() + "'>选购</a>"
                             + "   </p>"
                             + " </li>").appendTo("#ull2");
                }
            }
        });
    }
    function buy(productID, mechineID) {
        window.location.href = "productdetails.aspx?productID=" + productID + "&mechineID=" + mechineID;
    }
</script>
