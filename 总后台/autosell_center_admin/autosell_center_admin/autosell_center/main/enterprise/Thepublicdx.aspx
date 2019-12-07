<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Thepublicdx.aspx.cs" Inherits="autosell_center.main.enterprise.Thepublicdx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>公众号设置-自动售卖终端中心控制系统</title>
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
                                <a class="change" href="devicelist.aspx"><i class="change fa fa-check-square"></i>设备列表</a>
                            </dd>
                            <dd>
                                <a class="change" href="firmcon.aspx"><i class="change fa fa-plus-square"></i>新增设备</a>
                            </dd>
                        </dl>
                        <dl>
                            <dt>奶企设置<em class="fa fa-cog"></em></dt>
                            <dd>
                                <a class="change acolor"><i class="change icolor fa fa-wechat"></i>公众号设置</a>
                            </dd>
                            <dd>
                                <a class="change" href="Paymentzf.aspx"><i class="change fa fa-money"></i>支付设置</a>
                            </dd>
                            <dd>
                                <a class="change" href="Profit.aspx"><i class="change fa fa-database"></i>分润设置</a>
                            </dd>
                        </dl>
                    </div>
                    <section class="jiqlist">
                        <dl class="jiqlistTab">
                            <dd class="change" onclick="javascript: location.href = 'Thepublicjb.aspx';"><b>基础信息</b></dd>
                            <dd class="change ddcolor"><b>短信信息</b></dd>
                            <dd class="change" onclick="javascript: location.href = 'Thepublicsyz.aspx';"><b>使用者管理</b></dd>
                        </dl>
                        <ul class="thepublic">
                            <li>
                                <dl>
                                    <dd>剩余条数</dd>
                                </dl>
                            </li>
                            <li>
                                <label>0条</label>
                                <a class="public_p">分配短信</a>
                                <a class="public_p">设置短信签名</a>
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
        $("#li0").find("a").addClass("aborder");
    })
</script>
