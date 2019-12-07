<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="distribution.aspx.cs" Inherits="autosell_center.main.order.distribution" %>

<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>设置配货单-自动售卖终端中心控制系统</title>
    <meta charset="utf-8" />
    <link rel="shortcut icon" href="/main/public/images/favicon.ico" />
    <link href="/main/public/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/common.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/style.css" rel="stylesheet" type="text/css" />
    <script src="/main/public/script/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="/main/public/script/Titlelist.js" type="text/javascript"></script>
</head>
<body>
    <div class="header"></div>
    <div class="main">
        <!--<div class="prompt change">
            <img src="/main/public/images/_ok.png" alt=""/>
            <span>修改成功!</span>
        </div>-->
        <div class="main_list">
            <div class="common_title">
                <h4>
                    <i class="fa fa-plus"></i>
                    <span>设备管理</span>
                </h4>
                <!--<a class="change" href="SellCenter.html">
                    <i class="fa fa-reorder"></i>
                    切换奶企
                </a>-->
            </div>
            <div class="common_main">
                <div class="navlist">
                    <dl>
                        <dt>订单管理<em class="fa fa-cog"></em></dt>
                        <dd>
                            <a class="change" href="order.aspx"><i class="change fa fa-file-text"></i>订货管理</a>
                        </dd>
                        <dd>
                            <a class="change acolor"><i class="change icolor fa fa-window-restore"></i>订货报表</a>
                        </dd>
                    </dl>
                </div>
                <section class="jiqlist">
                    <div class="change zfpopup">
                        <h4>微信支付</h4>
                        <input type="hidden" value="" id="lihidden" />
                        <ul>
                            <li id="liproduct">
                                <h5>选择商品</h5>
                                <label>
                                    <select class="produ" onchange="parselect();">
                                        <option value="商品1">商品1</option>
                                        <option value="商品2">商品2</option>
                                        <option value="商品3">商品3</option>
                                    </select>
                                </label>
                            </li>
                            <li id="linum">
                                <h5>设置数量</h5>
                                <label>
                                    <select class="pronum">
                                        <option value="1">1</option>
                                        <option value="2">2</option>
                                        <option value="3">3</option>
                                        <option value="4">4</option>
                                        <option value="5">5</option>
                                    </select>
                                </label>
                            </li>
                        </ul>
                        <dl>
                            <dd>
                                <input type="button" value="确定" class="popupqdbtn" onclick="okBtn()">
                            </dd>
                            <dd>
                                <input type="button" value="取消" onclick="qxClick()">
                            </dd>
                        </dl>
                    </div>
                    <div class="popupbj"></div>
                    <dl class="jiqlistTab">
                        <dd class="change ddcolor"><b>设备列表</b></dd>
                    </dl>
                    <ul class="orderform">
                        <li>
                            <h4>
                                <em></em>
                                <input type="button" value="设置"/>
                            </h4>
                            <div>
                                <h2>001</h2>
                                <p>订货料道</p>
                            </div>
                            <span>已配货</span>
                            <input type="hidden" value="" class="hidde_product"/>
                            <input type="hidden" value="" class="hidde_num"/>
                        </li>
                        
                    </ul>
                </section>
            </div>
        </div>
    </div>
    <!--<div class="login_foot">
        <span>青岛冰格科技公司版权所有 翻版必究</span>
    </div>-->
</body>
</html>
<script>
    $(function () {
        
        $("#li2").find("a").addClass("aborder");

        var $liIndex = '';
        for (var i = 1; i < 61; i++) {
            if (i <= 9) {
                $liIndex += '<li><h4><em>当前剩余:1</em><input type="button" value="设置" /></h4><div><h2>00' + i + '</h2><p>订货料道</p></div><span>已配货</span><input type="hidden" value="" class="hidde_product" /><input type="hidden" value="" class="hidde_num" /></li>';
            } else {
                $liIndex += '<li><h4><em>当前剩余:1</em><input type="button" value="设置" /></h4><div><h2>00' + i + '</h2><p>订货料道</p></div><span>已配货</span><input type="hidden" value="" class="hidde_product" /><input type="hidden" value="" class="hidde_num" /></li>';
            }
        }
        $(".orderform").html($liIndex);

        var $liinputone = $(".orderform").find("li").find(".hidde_product");
        $liinputone.each(function () { //判断当前li是否设置
            if ($(this).val() != "") {
                $(this).parent().find("span").show();
            }
        });

        $(".orderform").find("li").each(function () { //给每个li添加id
            var $liNum = $(this).index();
            $(this).attr("class","li" + $liNum);
        });

        $(".orderform").find("li").find("input").click(function () { //点击设置事件
            $(".zfpopup").addClass("zfpopup_on");
            $(".popupbj").fadeIn();
            var $thisNum = $(this).parent().parent("li").attr("class");
            $("#lihidden").val($thisNum);
        });
    });


    function okBtn() { //点击确定事件
        var $liOne = $(".produ option:selected").text();
        var $liTwo = $(".pronum option:selected").text();
        var $popupVal = $("#lihidden").val();
        $("." + $popupVal).find(".hidde_product").val($liOne);
        $("." + $popupVal).find(".hidde_num").val($liTwo);

        $("." + $popupVal).find("em").eq(0).html("当前剩余:2");
        $("." + $popupVal).find("span").show();
        $(".zfpopup").removeClass("zfpopup_on");
        $(".popupbj").fadeOut();
    }

    function qxClick() { //点击取消事件
        $(".zfpopup").removeClass("zfpopup_on");
        $(".popupbj").fadeOut();
    }
</script>