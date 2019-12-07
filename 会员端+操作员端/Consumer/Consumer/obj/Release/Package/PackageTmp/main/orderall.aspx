<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="orderall.aspx.cs" Inherits="Consumer.main.orderall" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>查看订单-自助售卖系统</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />
    <link rel="shortcut icon" href="/main/public/images/favicon.ico" />
    <link href="/main/public/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/common.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/style.css" rel="stylesheet" type="text/css" />
    <script src="/main/public/script/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="/main/public/script/JavaScript.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <main class="setupq">
            <h4 class="commontitle">
                <i class="fa fa-angle-left" onclick="javascript:location.href='orderlist.aspx'"></i>
                查看订单
            </h4>
            <section class="orderclass">
                <dl>
                    <dd><a href="#all" class="acolor">全部</a></dd>
                    <dd><a href="#pickup">取货中</a></dd>
                    <dd><a href="#payment">完成</a></dd>
                    <dd><a href="#resale">已转售</a></dd>
                    <dd><a href="#invalid">已失效</a></dd>
                    <dd><a href="#dzf">待支付</a></dd>
                </dl>
            </section>
            <section class="orderlist">
                <ul class="orderallcon" id="all">
                  
                </ul>
                <ul class="orderallcon" id="pickup">
                     
                </ul>
                <ul class="orderallcon" id="payment">
                     
                </ul>
                <ul class="orderallcon" id="resale">
                    
                </ul>
                <ul class="orderallcon" id="invalid">
                     
                </ul>
                 <ul class="orderallcon" id="dzf">
                     
                </ul>
            </section>
            <div class="alipayBtn" id="ljzf">
                <input type="button" value="立即支付" id="createOrder" onclick="pay()"/>
            </div>
        </main>
        <input  id="memberID" runat="server" type="hidden"/>
        <input  id="companyID" runat="server" type="hidden"/>
        
    </form>
</body>
</html>
<script>
    $(function () {
        $(".orderallcon").eq(0).show();
        $(".orderclass").find("a").click(function () {
            $(".orderclass").find("a").removeClass("acolor");
            $(this).addClass("acolor");
            var indexNum = $(this).parent("dd").index();
            $(".orderallcon").eq(indexNum).show().siblings(".orderallcon").hide();
            if (indexNum=="5") {

                $("#ljzf").show();

            } else  {
                $("#ljzf").hide();

            }  
            
        });
       
        searAll();
        searPickup();
        searPayment();
        searResale();
        searInvalid();
        searDZF();
        var $url = location.hash;
        if ($url == "#all") {
            $(".orderclass").find("a").removeClass("acolor");
            $(".orderclass").find("a").eq(0).addClass("acolor");
            $(".orderallcon").eq(0).show().siblings(".orderallcon").hide();
            $("#ljzf").hide();
           
        } else if ($url == "#pickup") {
            $(".orderclass").find("a").removeClass("acolor");
            $(".orderclass").find("a").eq(1).addClass("acolor");
            $(".orderallcon").eq(1).show().siblings(".orderallcon").hide();
            $("#ljzf").hide();
            
        } else if ($url == "#payment") {
            $(".orderclass").find("a").removeClass("acolor");
            $(".orderclass").find("a").eq(2).addClass("acolor");
            $(".orderallcon").eq(2).show().siblings(".orderallcon").hide();
            $("#ljzf").hide();
           
        } else if ($url == "#resale") {
            $(".orderclass").find("a").removeClass("acolor");
            $(".orderclass").find("a").eq(3).addClass("acolor");
            $(".orderallcon").eq(3).show().siblings(".orderallcon").hide();
            $("#ljzf").hide();
           
        } else if ($url == "#invalid") {
            $(".orderclass").find("a").removeClass("acolor");
            $(".orderclass").find("a").eq(4).addClass("acolor");
            $(".orderallcon").eq(4).show().siblings(".orderallcon").hide();
            $("#ljzf").hide();
            
        }
        else if ($url == "#dzf") {
            $(".orderclass").find("a").removeClass("acolor");
            $(".orderclass").find("a").eq(5).addClass("acolor");
            $(".orderallcon").eq(5).show().siblings(".orderallcon").hide();
            $("#ljzf").show();

        }
    })
    function searAll() {
        $("#all").empty();
        var type = "";//all全部  pickup取货中 payment待付款  resale已转售  invalid已失效
        $.ajax({
            type: "post",
            url: "orderall.aspx/getOrderList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{memberID:'" + $("#memberID").val() + "',type:'all'}",
            success: function (data) {
                var serverdata = $.parseJSON(data.d);
                var serverdatalist = serverdata.length;
                for (var i = 0; i < serverdatalist; i++) {
                    var zt = "";
                    if (serverdata[i].zt == "0") {
                        zt = "生产中";
                    } else if (serverdata[i].zt == "1") {
                        zt = "配送中";
                    } else if (serverdata[i].zt == "2") {
                        zt = "已转售";
                    } else if (serverdata[i].zt == "3") {
                        zt = "配送完成";
                    } else if (serverdata[i].zt == "4") {
                        zt = "已兑换";
                    }
                    $(" <li>"
                        + "<a href='orderdetails.aspx?orderNO=" + serverdata[i].orderNO+ "'>"
                        + "    <div class='orderleft'>"
                        + "        <span>"
                        + "            <img src='" + serverdata[i].httpImageUrl + "' alt='' />"
                        + "        </span>"
                        + "        <h2>" + serverdata[i].proName + "</h2>"
                        + "        <p>" + serverdata[i].createTime + "</p>"
                        + "        <p>订购周期：" + serverdata[i].zq + "天</p>"
                        + "        <p>剩余天数：" + serverdata[i].syNum + "天</p>"
                        + "    </div>"
                        + "    <div class='orderright'>"
                        + "        <label>" + zt + "</label>"
                        + "        <span>¥" + serverdata[i].totalMoney + "</span>"
                        + "    </div>"
                        + " </a>"
                    + "</li>").appendTo("#all")
                }
            }
        })
    }
    function searPickup() {
        $("#pickup").empty();
        var type = "";//all全部  pickup取货中 payment待付款  resale已转售  invalid已失效
        $.ajax({
            type: "post",
            url: "orderall.aspx/getOrderList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{memberID:'" + $("#memberID").val() + "',type:'pickup'}",
            success: function (data) {
                var serverdata = $.parseJSON(data.d);
                var serverdatalist = serverdata.length;
                for (var i = 0; i < serverdatalist; i++) {
                    var zt = "";
                    if (serverdata[i].zt == "0") {
                        zt = "生产中";
                    } else if (serverdata[i].zt == "1") {
                        zt = "配送中";
                    } else if (serverdata[i].zt == "2") {
                        zt = "已转售";
                    } else if (serverdata[i].zt == "3") {
                        zt = "配送完成";
                    } else if (serverdata[i].zt == "4") {
                        zt = "已兑换";
                    }
                    $(" <li>"
                        + "<a href='orderdetails.aspx?orderNO=" + serverdata[i].orderNO + "'>"
                        + "    <div class='orderleft'>"
                        + "        <span>"
                        + "            <img src='" + serverdata[i].httpImageUrl + "' alt='' />"
                        + "        </span>"
                        + "        <h2>" + serverdata[i].proName + "</h2>"
                        + "        <p>" + serverdata[i].createTime + "</p>"
                        + "        <p>订购周期：" + serverdata[i].zq + "天</p>"
                        + "        <p>剩余天数：" + serverdata[i].syNum + "天</p>"
                        + "    </div>"
                        + "    <div class='orderright'>"
                        + "        <label>取货中</label>"
                        + "        <span>¥" + serverdata[i].totalMoney + "</span>"
                        + "    </div>"
                        + " </a>"
                    + "</li>").appendTo("#pickup")
                }
            }
        })
    }
    function searPayment() {
        $("#payment").empty();
        var type = "";//all全部  pickup取货中 payment待付款  resale已转售  invalid已失效
        $.ajax({
            type: "post",
            url: "orderall.aspx/getOrderList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{memberID:'" + $("#memberID").val() + "',type:'payment'}",
            success: function (data) {
                var serverdata = $.parseJSON(data.d);
                var serverdatalist = serverdata.length;
                for (var i = 0; i < serverdatalist; i++) {
                    var zt = "";
                    if (serverdata[i].zt == "0") {
                        zt = "生产中";
                    } else if (serverdata[i].zt == "1") {
                        zt = "配送中";
                    } else if (serverdata[i].zt == "2") {
                        zt = "已转售";
                    } else if (serverdata[i].zt == "3") {
                        zt = "配送完成";
                    } else if (serverdata[i].zt == "4") {
                        zt = "已兑换";
                    }
                    $(" <li>"
                        + "<a href='orderdetails.aspx?orderNO=" + serverdata[i].orderNO + "'>"
                        + "    <div class='orderleft'>"
                        + "        <span>"
                        + "            <img src='" + serverdata[i].httpImageUrl + "' alt='' />"
                        + "        </span>"
                        + "        <h2>" + serverdata[i].proName + "</h2>"
                        + "        <p>" + serverdata[i].createTime + "</p>"
                        + "        <p>订购周期：" + serverdata[i].zq + "天</p>"
                        + "        <p>剩余天数：" + serverdata[i].syNum + "天</p>"
                        + "    </div>"
                        + "    <div class='orderright'>"
                        + "        <label>" + zt + "</label>"
                        + "        <span>¥" + serverdata[i].totalMoney + "</span>"
                        + "    </div>"
                        + " </a>"
                    + "</li>").appendTo("#payment")
                }
            }
        })
    }
    function searResale() {
        $("#resale").empty();
        var type = "";//all全部  pickup取货中 payment待付款  resale已转售  invalid已失效
        $.ajax({
            type: "post",
            url: "orderall.aspx/getOrderList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{memberID:'" + $("#memberID").val() + "',type:'resale'}",
            success: function (data) {
                var serverdata = $.parseJSON(data.d);
                var serverdatalist = serverdata.length;
                for (var i = 0; i < serverdatalist; i++) {
                    var zt = "";
                    if (serverdata[i].zt == "0") {
                        zt = "生产中";
                    } else if (serverdata[i].zt == "1") {
                        zt = "配送中";
                    } else if (serverdata[i].zt == "2") {
                        zt = "已转售";
                    } else if (serverdata[i].zt == "3") {
                        zt = "配送完成";
                    } else if (serverdata[i].zt == "4") {
                        zt = "已兑换";
                    }
                    $(" <li>"
                        + "<a href='#'>"
                        + "    <div class='orderleft'>"
                        + "        <span>"
                        + "            <img src='" + serverdata[i].httpImageUrl + "' alt='' />"
                        + "        </span>"
                        + "        <h2>" + serverdata[i].proName + "</h2>"
                        + "        <p>" + serverdata[i].createTime + "</p>"
                        + "        <p>订单编号：" + serverdata[i].orderNO + "</p>"
                        + "        <p>转售价格：" + serverdata[i].sellPrice + "</p>"
                        + "    </div>"
                        + "    <div class='orderright'>"
                        + "        <label>已转售</label>"
                       // + "        <span>¥" + serverdata[i].totalMoney + "</span>"
                        + "    </div>"
                        + " </a>"
                    + "</li>").appendTo("#resale")
                }
            }
        })
    }
    function searInvalid() {
        $("#invalid").empty();
        var type = "";//all全部  pickup取货中 payment待付款  resale已转售  invalid已失效
        $.ajax({
            type: "post",
            url: "orderall.aspx/getOrderList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{memberID:'" + $("#memberID").val() + "',type:'invalid'}",
            success: function (data) {
                var serverdata = $.parseJSON(data.d);
                var serverdatalist = serverdata.length;
                for (var i = 0; i < serverdatalist; i++) {
                    
                    $(" <li>"
                        + "<a href='orderdetails.aspx?orderNO=" + serverdata[i].orderNO + "'>"
                        + "    <div class='orderleft'>"
                        + "        <span>"
                        + "            <img src='" + serverdata[i].httpImageUrl + "' alt='' />"
                        + "        </span>"
                        + "        <h2>" + serverdata[i].proName + "</h2>"
                        + "        <p>" + serverdata[i].createTime + "</p>"
                        + "        <p>订单编号：" + serverdata[i].orderNO + "</p>"
                        + "        <p>取货码：" + serverdata[i].code + "</p>"
                        + "    </div>"
                        + "    <div class='orderright'>"
                        + "        <label>已失效</label>"
                        + "        <span></span>"
                        + "    </div>"
                        + " </a>"
                    + "</li>").appendTo("#invalid")
                }
            }
        })
    }
    function searDZF() {
        $("#invalid").empty();
        var type = "";//all全部  pickup取货中 payment待付款  resale已转售  invalid已失效 dzf待支付
        $.ajax({
            type: "post",
            url: "orderall.aspx/getOrderList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{memberID:'" + $("#memberID").val() + "',type:'dzf'}",
            success: function (data) {
                var serverdata = $.parseJSON(data.d);
                var serverdatalist = serverdata.length;
                for (var i = 0; i < serverdatalist; i++) {
                    var str = "";
                    var bz = "";
                    var link = "";
                    if (isBool(serverdata[i].createTime)) {
                        bz = "待支付";
                        str = "<input type='checkbox' name='chk' value='" + serverdata[i].id + "' style='display:inline-block;width:14px;float: left;position: relative;top: 20px;' onclick='cli(\"" + serverdata[i].createTime + "\")'>";
                        link = "<a style='width: calc(100% - 22px)' href='orderdetails.aspx?orderNO=" + serverdata[i].orderNO + "'>";
                    } else {
                        str = "<label style='display:inline-block;width:14px;float: left;position: relative;top: 20px;'>&nbsp;&nbsp;&nbsp;</label>";
                        bz = "支付超时";
                        link = "<a style='width: calc(100% - 22px)' href='#'>"
                    }

                    $(" <li>"
                        + str
                        + link
                        + "    <div class='orderleft'>"
                        + "        <span>"
                        + "            <img src='" + serverdata[i].httpImageUrl + "' alt='' />"
                        + "        </span>"
                        + "        <h2>" + serverdata[i].proName + "</h2>"
                       + "        <p>派送日期：" + serverdata[i].qsDate + "</p>"
                        + "        <p>周期：" + serverdata[i].zq + "</p>"
                        + "        <p>总价：" + serverdata[i].totalMoney + "</p>"
                        + "    </div>"
                        + "    <div class='orderright'>"
                        + "        <label>"+bz+"</label>"
                        + "        <label onclick='del(" + serverdata[i].id + ")'>删除</label>"
                        + "        <span></span>"
                        + "    </div>"
                        + " </a>"
                    + "</li>").appendTo("#dzf")
                }
            }
        })
    }
    function isBool(time)
    {
        var date1 = time.replace("-", "/").replace("-", "/");  //开始时间
       
        var date2 = new Date();    //结束时间
 
        var date3 = date2.getTime() - new Date(date1).getTime();   //时间差的毫秒数      
        //计算出相差天数
        var days = Math.floor(date3 / (24 * 3600 * 1000))
        if(days=="0")
        {
            return true;
        }
        return false;
    }
    function cli(val)
    {
        //判断只能支付当天下的单
        if (!isBool(val))
        {
            alert("只能合并支付当天的订单");
           
        }
        $(this).find("input[type=checkbox]:checked").attr("checked", false);
        
    }
    function del(val)
    {
        if(confirm("是否确定删除"))
        {
            $.ajax({
                type: "post",
                url: "orderall.aspx/del",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: "{id:'" + val + "'}",
                success: function (data) {
                    alert(data.d.msg);
                    location.reload();
                }
            })
        }
       
    }
    function pay()
    {
        var id = "";
        if ($('input[type=checkbox]:checked').length <= 0) return;
        $('input[name="chk"]:checked').each(function () {
             id =id+$(this).val()+",";
            //判断是否有订单的开始派送日期低于当前支付日期的 有的话过滤掉不能支付重新下单

        });
        id = id.substring(0, id.length - 1);
        var url = "allpay.aspx?idArr=" + id + "&companyID=" + $("#companyID").val();
        location.href = url;
    }
</script>
