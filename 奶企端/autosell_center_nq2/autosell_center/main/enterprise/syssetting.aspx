<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="syssetting.aspx.cs" Inherits="autosell_center.main.enterprise.syssetting" %>

<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>首页-自动售卖终端中心控制系统</title>
    <meta charset="utf-8" />
    <link rel="shortcut icon" href="/main/public/images/favicon.ico" />
    <link href="/main/public/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/common.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/style.css" rel="stylesheet" type="text/css" />
    <script src="/main/public/script/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="/main/public/script/Titlelist.js" type="text/javascript"></script>
    <style>
        .productclass li .classid,.productclass li .classname{
            width:20%;
        }
        .productclass li .classdelete{
            width:60%;
        }
        .productclass li .classdelete input{
            width:80%;
        }
        .productclass{
            margin-top:30px;
        }
    </style>
</head>
<body>
    <form>
        <div class="header"></div>
    <div class="main">
        <div class="main_list">
            <div class="common_title">
                    <h4>
                        <i class="fa fa-plus"></i>
                        <span>公众号设置</span>
                    </h4>
                </div>
            <div class="common_main">
                <div class="navlist">
                       <dl>
                            <dt>公众号设置<em class="fa fa-cog"></em></dt>
                            <dd>
                                <a class="change acolor" href="authPage.aspx"><i class="change icolor fa fa-wechat"></i>公众号设置</a>
                            </dd>
                        </dl>
                         <dl>
                            <dt>自定义菜单设置<em class="fa fa-cog"></em></dt>
                            <dd>
                                <a class="change " href="Thepublicjb.aspx"><i class="change  fa fa-wechat"></i>自定义菜单设置</a>
                            </dd>
                        </dl>
                        <dl>
                            <dt>模板消息<em class="fa fa-cog"></em></dt>
                            <dd>
                                <a class="change" href="Distributor.aspx"><i class="change fa fa-envelope"></i>模板消息</a>
                            </dd>
                        </dl>
                         <dl>
                            <dt>参数设置<em class="fa fa-cog"></em></dt>
                            <dd>
                                 <a class="change" href="syssetting.aspx"><i class="change fa fa-bars"></i>参数设置</a>
                            </dd>
                        </dl>
                     <%--  <dl>
                            <dt>App流量统计<em class="fa fa-cog"></em></dt>
                            <dd>
                                 <a class="change" href="applog.aspx"><i class="change fa fa-bars"></i>App流量统计</a>
                            </dd>
                        </dl>--%>
                    </div>
                <section class="jiqlist">
                    <dl class="jiqlistTab">
                        <dd class="change ddcolor"><b>参数设置</b></dd>
                    </dl>
                    <table class="syssettingTable">
                      
                        <tr>
                            <td>取货定点通知</td>
                            <td>
                                <input type="text" value="" id="getTime" placeholder="格式:17:00"  runat="server" maxlength="5"/>
                            </td>
                             
                        </tr>
                        <tr>
                            <td></td>
                            <td><p style="display:block;font-size:10px;color:#999">格式:09:00</p></td>
                        </tr>
                       <%-- <tr>
                            <td>充值赠送红包</td>
                            <td>
                                <input type="text" value="" id="p4" placeholder="输入0代表不开启赠送红包"  runat="server"/>
                            </td>
                        </tr>
                        <tr>
                             <td></td>
                             <td><p style="display:block;font-size:10px;color:#999">0代表不开启,请输入整数金额</p></td>
                        </tr>
                          <tr>
                            <td>激活红包</td>
                            <td>
                                <input type="text" value="" id="jhhb" placeholder=""  runat="server"/>
                            </td>
                            
                        </tr>
                        <tr>
                             <td></td>
                             <td><p style="display:block;font-size:10px;color:#999">激活红包下限</p></td>
                        </tr>--%>
                          <tr>
                            <td>使用期限</td>
                            <td>
                                <input type="text" value="" id="syqx" placeholder=""  runat="server"/>
                            </td>
                            
                        </tr>
                        <tr>
                             <td></td>
                             <td><p style="display:block;font-size:10px;color:#999">注册会员成功后，n天内使用</p></td>
                        </tr>
                         <tr>
                            <td>显示范围</td>
                            <td>
                                <input type="text" value="" id="jlc" placeholder=""  runat="server"/>
                            </td>
                        </tr>
                         <tr>
                             <td></td>
                             <td><p style="display:block;font-size:10px;color:#999">微信端显示机器多少米范围内</p></td>
                        </tr>
                          <tr>
                            <td>公众号biz码</td>
                            <td>
                                <input type="text" value="" id="biz" placeholder=""  runat="server"/>
                            </td>
                        </tr>
                        <tr>
                            <td>小程序Appid</td>
                            <td>
                                <input type="text" value="" id="minAppid" placeholder=""  runat="server"/>
                            </td>
                        </tr>
                         <tr>
                            <td>小程序AppSerect</td>
                            <td>
                                <input type="text" value="" id="minAppSerect" placeholder=""  runat="server"/>
                            </td>
                        </tr>
                         <tr>
                             <td></td>
                             <td><p style="display:block;font-size:10px;color:#999">用于关注公众号生成链接</p></td>
                        </tr>
                         <tr>
                            <td>退款标识0/1</td>
                            <td>
                                <input type="text" value="" id="tkbs" placeholder=""  runat="server"/>
                            </td>
                        </tr>
                        
                         <tr>
                             <td></td>
                             <td><p style="display:block;font-size:10px;color:#999">当机器出货错误时,1系统自动退款，0系统自动退款关闭</p></td>
                        </tr>
                         <tr>
                            <td>客服电话</td>
                            <td>
                                <input type="text" value="" id="phone" placeholder=""  runat="server"/>
                            </td>
                        </tr>
                    </table>
                    <input type="button" value="保存" class="seachbtnTwo" onclick="save()"/>
                </section>
            </div>
        </div>
    </div>
        <input  id="companyID" runat="server" type="hidden"/>
         <input id="_operaID" runat="server" type="hidden" />
    </form>
    
</body>
</html>
<script>
    function judge() {
        $.ajax({
            type: "post",
            url: "syssetting.aspx/judge",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{operaID:'" + $("#_operaID").val() + "',menuID:'gzhsz'}",
            success: function (data) {
                if (data.d.code == "500") {
                    $(".jiqlist").empty();
                    $(".jiqlist").html("<p class='noquanxian'>当前没有权限</p>").css({ "background": "#ddd" });
                }
            }
        })
    }
    function SubmitCk() {
        var reg = /^(0|[1-9][0-9]*)$/;
        if (!reg.test($("#p4").val())) {
            alert("输入的不是合法数字");
            return false;
        }
    }
    $(function () {
        judge()
        $("#li8").find("a").addClass("aborder");
       
    })
    function save() {

        if ($("#getTime").val().indexOf(":") <= 0 || $("#getTime").val().length < 5)
        {
            alert("输入的时间格式不合法");
            return false;
        }

        var reg = /^(0|[1-9][0-9]*)$/;
       
        if (!reg.test($("#syqx").val())) {
            alert("使用期限不是合法数字");
            return false;
        }
        if (!reg.test($("#jlc").val())) {
            alert("范围输入的不是合法数字");
            return false;
        }
        if ($("#tkbs").val() != "0" && $("#tkbs").val() != "1")
        {
            alert("输入的退款标识不合法");
            return false;
        }
        if ($("#minAppid").val()=="")
        {
            alert("小程序AppID不能为空");
            return false;
        }
        if ($("#minAppSerect").val() == "") {
            alert("小程序AppSerect不能为空");
            return false;
        }
        $.ajax({
            type: "post",
            url: "syssetting.aspx/save",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{companyID:'" + $("#companyID").val() + "',getTime:'" + $("#getTime").val() + "',jlc:'" + $("#jlc").val() + "',syqx:'" + $("#syqx").val() + "',biz:'" + $("#biz").val() + "',tkbs:'" + $("#tkbs").val() + "',minAppid:'" + $("#minAppid").val() + "',minAppSerect:'" + $("#minAppSerect").val() + "',phone:'" + $("#phone").val() + "'}",
            success: function (data) {
                if (data.d == "1") {
                    alert("保存成功");
                } else if (data.d == "0")
                {
                    alert("保存失败");
                } else if (data.d == "3") {
                    alert("时间格式不正确");
                } else if (data.d == "4") {
                    alert("视屏下载间隔不正确 请输入正整数");
                }  
            }
        })
    }
    
</script>
