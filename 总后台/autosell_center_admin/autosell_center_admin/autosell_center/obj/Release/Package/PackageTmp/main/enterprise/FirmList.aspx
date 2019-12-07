<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FirmList.aspx.cs" Inherits="autosell_center.main.enterprise.FirmList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>首页-自动售卖终端中心控制系统</title>
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
                        <span>添加企业</span>
                    </h4>
                    <!--<a class="change" href="SellCenter.aspx">
                    <i class="fa fa-reorder"></i>
                    切换奶企
                </a>-->
                </div>
                <div class="common_main">
                    <div class="common_left">
                        <div class="common_list">
                            <h4>新增企业</h4>
                            <ul>
                                <li>
                                    <a href="#">新增企业</a>
                                </li>
                            </ul>
                        </div>
                        <div class="common_list">
                            <h4>新增设备</h4>
                            <ul>
                                <li>
                                    <a href="#">设备有效期</a>
                                </li>
                                <li>
                                    <a href="#">设备编号</a>
                                </li>
                                <li>
                                    <a href="#">开启时间</a>
                                </li>
                            </ul>
                        </div>
                        <div class="common_list">
                            <h4>奶企设置</h4>
                            <ul>
                                <li>
                                    <a href="#">公众号设置</a>
                                </li>
                                <li>
                                    <a href="#">支付设置</a>
                                </li>
                                <li>
                                    <a href="#">分润设置</a>
                                </li>
                            </ul>
                        </div>
                    </div>
                    <div class="common_right">
                    </div>
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
