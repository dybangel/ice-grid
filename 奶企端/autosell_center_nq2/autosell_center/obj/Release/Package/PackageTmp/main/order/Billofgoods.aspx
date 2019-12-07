<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Billofgoods.aspx.cs" Inherits="autosell_center.main.order.Billofgoods" %>

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
    <style>
        .firmbtn {
            margin-left: 30px;
        }

        .memberlist {
            margin-top: 30px;
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
                            <dt>订单管理<em class="fa fa-cog"></em></dt>
                           
                           <%-- <dd>
                                <a class="change acolor" href="orderform.aspx"><i class="change icolor fa fa-window-restore"></i>机器统计订单</a>
                            </dd>--%>
                            <dd>
                                <a class="change" href="Productform.aspx"><i class="change fa fa-window-restore"></i>商品统计订单</a>
                            </dd>
                             <dd>
                                <a class="change" href="../equipment/orderlist.aspx"><i class="change fa fa-window-restore"></i>购买记录</a>
                            </dd>
                        </dl>
                    </div>
                    <section class="jiqlist">
                        <dl class="jiqlistTab">
                            <dd class="change ddcolor"><b>订货报表</b></dd>
                        </dl>
                        <ul class="memberlist" id="ull">
                          
                        </ul>
                    </section>
                </div>
            </div>
        </div>
        <input id="time" runat="server" type="hidden"/>
        <input  id="mechineID" runat="server" type="hidden"/>
    </form>
</body>
</html>
<script>
    $(function () {
        $("#li2").find("a").addClass("aborder");
        sear();
    })
    function sear() {
        $("#ull").empty();
        $(" <li>"
                   +"<label style='width:25%'>商品id</label>"
                   + "<label style='width:25%'>商品名称</label>"
                   + "<label style='width:25%'>订购数量</label>"
                    + "<label style='width:25%'>零售数量</label>"
                   
                   +"</li>").appendTo("#ull");
        $.ajax({
            type: "post",
            url: "Billofgoods.aspx/sear",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{mechineID:'" + $("#mechineID").val() + "',time:'" + $("#time").val() + "'}",
            success: function (data) {
                var serverdata = $.parseJSON(data.d);
                var serverdatalist = serverdata.length;
                for (var i = 0; i < serverdatalist; i++) {
                    $(" <li>"
                             + "   <span style='width:25%'>" + serverdata[i].productID + "</span>"
                             + "   <span style='width:25%'>" + serverdata[i].proName + "</span>"
                             + "   <span style='width:25%'>" + serverdata[i].dg + "</span>"
                               + "   <span style='width:25%'>" + serverdata[i].ls + "</span>"
                             
                            + "</li> ").appendTo("#ull");
                }
            }
        })
    }
</script>
