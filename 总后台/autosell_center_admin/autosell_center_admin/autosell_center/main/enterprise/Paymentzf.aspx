<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Paymentzf.aspx.cs" Inherits="autosell_center.main.enterprise.Paymentzf" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>支付设置-自动售卖终端中心控制系统</title>
    <link rel="shortcut icon" href="/main/public/images/favicon.ico"/>
    <link href="/main/public/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css"/>
    <link href="/main/public/css/common.css" rel="stylesheet" type="text/css"/>
    <link href="/main/public/css/style.css" rel="stylesheet" type="text/css"/>
    <script src="/main/public/script/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="/main/public/script/Titlelist.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="header"></div>
<div class="main">
    <div class="main_list">
        <div class="common_title">
            <h4>
                <i class="fa fa-plus"></i>
                <span>奶企管理</span>
            </h4>
            <!--<a class="change" href="SellCenter.aspx">
                <i class="fa fa-reorder"></i>
                切换奶企
            </a>-->
        </div>
        <div class="common_main">
            <div class="navlist">
                <dl>
                    <dt>奶企设备<em class="fa fa-cog"></em></dt>
                    <dd>
                        <a class="change" href="devicelist.aspx?companyID=<%=company_ID %>"><i class="change fa fa-check-square"></i>设备列表</a>
                    </dd>
                    <dd>
                        <a class="change" href="firmcon.aspx?companyID=<%=company_ID %>"><i class="change fa fa-plus-square"></i>新增设备</a>
                    </dd>
                </dl>
                <dl>
                    <dt>奶企设置<em class="fa fa-cog"></em></dt>
                    <dd>
                        <a class="change" href="Thepublicjb.aspx?companyID=<%=company_ID %>"><i class="change fa fa-wechat"></i>公众号设置</a>
                    </dd>
                    <dd>
                        <a class="change acolor"><i class="change icolor fa fa-money"></i>支付设置</a>
                    </dd>
                    <dd>
                        <a class="change" href="Profit.aspx?companyID=<%=company_ID %>"><i class="change fa fa-database"></i>分润设置</a>
                    </dd>
                </dl>
            </div>
            <section class="jiqlist">
                <div class="change zfpopup">
                    <h4></h4>
                    <ul>
                        <li class="fsopen">
                            <h2>您的支付宝账号必须支持手机网页即时到账接口, 才能使用手机支付功能, <a href="https://b.alipay.com/signing/productSet.htm?navKey=all" target="_black">申请及详情请查看这里.</a>
                            </h2>
                            <h5></h5>
                            <span>
                                <i class="change iopen"></i>
                                <em class="change emopen"></em>
                                <input type="hidden" value="1" />
                            </span>
                        </li>
                        <li>
                            <h5></h5>
                            <label>
                                <input type="text" value="" placeholder=""/>
                                <b class="fa fa-exclamation-circle"></b>
                            </label>
                            <p>如果开启兑换或交易功能，请填写真实有效的支付宝账号，用于收取用户以现金兑换交易积分的相关款项。如账号无效或安全码有误，将导致用户支付后无法正确对其积分账户自动充值，或进行正常的交易对其积分账户自动充值，或进行正常的交易。 如您没有支付宝帐号， <a class="change" href="https://memberprod.alipay.com/account/reg/index.htm" target="_black">请点击这里注册</a>
                            </p>
                        </li>
                        <li>
                            <h5></h5>
                            <label>
                                <input type="text" value="" placeholder=""/>
                                <b class="fa fa-exclamation-circle"></b>
                            </label>
                            <p>支付宝签约用户请在此处填写支付宝分配给您的合作者身份，签约用户的手续费按照您与支付宝官方的签约协议为准。如果您还未签约， <a class="change" href="https://memberprod.alipay.com/account/reg/enterpriseIndex.htm" target="_black">请点击这里签约</a> ； 如果已签约, <a class="change" href="https://memberprod.alipay.com/account/reg/index.htm" target="_black">请点击这里获取PID、Key</a> ; 如果在签约时出现合同模板冲突，请咨询010-3456789
                            </p>
                        </li>
                        <li>
                            <h5></h5>
                            <label>
                                <input type="text" value="" placeholder=""/>
                                <b class="fa fa-exclamation-circle"></b>
                            </label>
                            <p>支付宝签约用户可以在此处填写支付宝分配给您的交易安全校验码，此校验码您可以到支付宝官方的商家服务功能处查看</p>
                        </li>
                    </ul>
                    <dl>
                        <dd>
                            <input type="button" value="确定" class="popupqdbtn"/>
                        </dd>
                        <dd>
                            <input type="button" value="取消" onclick="qxClick()" />
                        </dd>
                    </dl>
                </div>
                <div class="popupbj"></div>
                <dl class="jiqlistTab">
                    <dd class="change ddcolor"><b>支付设置</b></dd>
                    <!--<dd class="change" onclick="javascript: location.href = 'Paymenttk.aspx';">退款配置</dd>-->
                </dl>
                <ul class="thepublic">
                    <li>
                        <dl>
                            <dd>支付参数</dd>
                        </dl>
                    </li>
                    <!--<li class="fsopen">
                        <label>支付宝支付</label>
                        <span>
                            <i class="change iopen"></i>
                            <em class="change emopen"></em>
                            <input type="hidden" value="1" />
                        </span>
                    </li>-->
                    <li>
                        <label>支付宝支付</label>
                        <input type="text" value="关闭" readonly="readonly"/>
                        <a class="public_p" onclick="zfbClick()">修改</a>
                    </li>
                    <li>
                        <label>微信支付</label>
                        <input type="text" value="关闭" readonly="readonly"/>
                        <a class="public_p" onclick="wxClick()">修改</a>
                    </li>
                    <li>
                        <label>平台支付</label>
                        <input type="text" value="关闭" readonly="readonly"/>
                        <a class="public_p" onclick="ptClick()">修改</a>
                    </li>
                </ul>
            </section>
        </div>
    </div>
</div>
 <input  id=""/>
    </form>
</body>
</html>
<script>
    var $popupT = $(".zfpopup").find("h4");
    var $popupH2 = $(".zfpopup").find("h2");
    var $popupH5 = $(".zfpopup").find("li").eq(0).find("h5");
    var $popupP = $(".zfpopup").find("p");
    var $ulOne = $(".zfpopup").find("li").eq(1);
    var $ulTwo = $(".zfpopup").find("li").eq(2);
    var $ulThree = $(".zfpopup").find("li").eq(3);
    var $qdBtn = $(".popupqdbtn");
    function zfbClick() {
        $popupT.html("支付宝支付");
        $popupH2.show();
        $popupH5.html("是否开启支付宝支付");
        $popupP.show();
        $(".zfpopup").find("b").show();
        $ulOne.find("h5").html("收款支付宝账号");
        $ulTwo.find("h5").html("合作者身份");
        $ulTwo.find("input").attr({ "type": "type" });
        $ulThree.show().find("h5").html("校验密钥");
        $qdBtn.attr("onclick","1");
        $(".popupbj").fadeIn();
        $(".zfpopup").addClass("zfpopup_on");
    }

    function wxClick() {
        $popupT.html("微信支付");
        $popupH2.hide();
        $popupH5.html("是否开启微信支付");
        $popupP.hide();
        $ulOne.find("h5").html("微信收款账号");
        $ulTwo.find("h5").html("微信收款二维码");
        $(".zfpopup").find("b").hide();
        $ulTwo.find("input").attr({ "type": "file" });
        $ulThree.hide();
        $qdBtn.attr("onclick", "2");
        $(".popupbj").fadeIn();
        $(".zfpopup").addClass("zfpopup_on");
    }

    function ptClick() {
        $popupT.html("平台支付");
        $popupH2.hide();
        $popupH5.html("是否开启银行卡支付");
        $popupP.hide();
        $ulOne.find("h5").html("银行卡收款账号");
        $ulTwo.find("h5").html("银行收款人姓名");
        $(".zfpopup").find("b").hide();
        $ulTwo.find("input").attr({ "type": "text" });
        $ulThree.hide();
        $qdBtn.attr("onclick", "3");
        $(".popupbj").fadeIn();
        $(".zfpopup").addClass("zfpopup_on");
    }

    function qxClick() {
        $(".zfpopup").removeClass("zfpopup_on");
        setTimeout(function () { $(".popupbj").hide(); }, 300);
        
    }
</script>
<script>
    $(function() {
        $(".fsopen").find("span").click(function() {
            $(this).find("em").toggleClass("emopen");
            $(this).find("i").toggleClass("iopen");
            if ($(this).find("em").hasClass("emopen")) {
                $(this).children("input").val("1");
            } else {
                $(this).children("input").val("0");
            }
        });
    });
</script>
<script>
    $(function() {
        $("#li0").find("a").addClass("aborder");
    })
</script>