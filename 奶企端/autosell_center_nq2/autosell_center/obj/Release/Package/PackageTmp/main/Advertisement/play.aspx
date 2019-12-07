<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="play.aspx.cs" Inherits="autosell_center.main.Advertisement.play" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>播放管理-自动售卖终端中心控制系统</title>
    <link rel="shortcut icon" href="/main/public/images/favicon.ico" />
    <link href="/main/public/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/common.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/style.css" rel="stylesheet" type="text/css" />
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
                                <a class="change" href="video.aspx"><i class="change fa fa-check-square"></i>视频管理</a>
                            </dd>
                            <dd>
                                <a class="change acolor"><i class="change icolor fa fa-plus-square"></i>播放管理</a>
                            </dd>
                            <dd>
                                <a class="change" href="Jurisdiction.aspx"><i class="change fa fa-plus-square"></i>设备管理</a>
                            </dd>
                        </dl>
                    </div>
                    <section class="jiqlist">
                        <dl class="jiqlistTab">
                            <dd class="change ddcolor"><b>商品列表</b></dd>
                        </dl>
                        <ul class="jiqlistseach">
                            <li>
                                <input type="text" value="" placeholder="视频名称">
                            </li>
                            <li>
                                <input type="text" value="" placeholder="公司">
                            </li>
                            <li>
                                <input type="text" value="" placeholder="联系人">
                            </li>
                            <li>
                                <input type="text" value="" placeholder="状态">
                            </li>
                            <li>
                                <input type="button" value="查询" class="seachbtn">
                            </li>
                        </ul>
                        <ul class="productlist">
                            <li>
                                <label class="proname">视频名称</label>
                                <label class="proclass">公司</label>
                                <label class="pronaiq">联系人</label>
                                <label class="projiag">广告审批许可</label>
                                <label class="proguig">播放次数</label>
                                <label class="probao">状态</label>
                                <%--<label class="procz">操作</label>--%>
                            </li>
                            <li>
                                <span class="proname">
                                    <em>
                                        <i class="fa fa-play-circle"></i>
                                        <img src="/main/public/images/video.jpg" />
                                    </em>
                                    <i>美汁源果粒橙淳萃橙宣传片</i>
                                </span>
                                <span class="proclass">可口可乐公司</span>
                                <span class="pronaiq">王大锤</span>
                                <span class="projiag">FDSAS124552dFAfs</span>
                                <span class="proguig">32453次</span>
                                <span class="probao">正常</span>
                                <%--<span class="procz">
                                    <a onclick="upDete()">修改</a>
                                    <a onclick="deLete()">删除</a>
                                </span>--%>
                            </li>
                        </ul>
                    </section>
                </div>
            </div>
        </div>
        <!--<div class="login_foot">
    <span>青岛冰格科技公司版权所有 翻版必究</span>
</div>-->
    </form>
</body>
</html>
<script>
    $(function () {
        $(".jiqlistTab").find("dd").click(function () {
            $(".jiqlistTab dd").removeClass("ddcolor");
            $(this).addClass("ddcolor");
            var $liNum = $(this).index();
            $(".jiqlisttable").hide();
            $(".jiqlisttable").eq($liNum).fadeIn();
        });
    });
</script>
<script>
    $(function () {
        $("#li7").find("a").addClass("aborder");
    })
</script>
