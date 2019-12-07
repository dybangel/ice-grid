<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="memberadd.aspx.cs" Inherits="autosell_center.main.member.memberadd" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>新增会员-自动售卖终端中心控制系统</title>
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
                        <span>会员管理</span>
                    </h4>
                    <!--<a class="change" href="SellCenter.aspx">
                <i class="fa fa-reorder"></i>
                切换奶企
            </a>-->
                </div>
                <div class="common_main">
                    <div class="navlist">
                        <dl>
                            <dt>会员管理<em class="fa fa-cog"></em></dt>
                            <dd>
                                <a class="change" href="memberlist.aspx"><i class="change fa fa-user"></i>会员列表</a>
                            </dd>
                            <dd>
                                <a class="change acolor"><i class="change icolor fa fa-plus-square"></i>新增会员</a>
                            </dd>
                        </dl>
                    </div>
                    <section class="jiqlist">
                        <div class="addnaiq">
                            <h4 class="commonfut"><a class="change" href="memberlist.aspx"><i class="change fa fa-angle-left"></i>会员列表</a>/添加会员</h4>
                            <ul class="addnaiqdata" id="addnaiqdata1" style="display: block;">
                                <li>
                                    <label>会员名称</label>
                                    <div>
                                        <input type="text" value="" placeholder="">
                                        <p>请填写会员真实姓名</p>
                                    </div>
                                </li>
                                <li>
                                    <label>手机号</label>
                                    <div>
                                        <input type="text" value="" placeholder="">
                                        <p>会员手机号码</p>
                                    </div>
                                </li>
                                <li>
                                    <label>登录密码</label>
                                    <div>
                                        <input type="text" value="" placeholder="">
                                        <p>密码最少为8位</p>
                                    </div>
                                </li>
                                <li>
                                    <label>邮箱</label>
                                    <div>
                                        <input type="text" value="" placeholder="">
                                        <!--<p>密码最少为8位</p>-->
                                    </div>
                                </li>
                                <li>
                                    <label>积分</label>
                                    <div>
                                        <input type="text" value="0" placeholder="">
                                        <!--<p>密码最少为8位</p>-->
                                    </div>
                                </li>
                                <li>
                                    <label>余额</label>
                                    <div>
                                        <input type="text" value="0" placeholder="">
                                        <!--<p>密码最少为8位</p>-->
                                    </div>
                                </li>
                                <li>
                                    <input class="firmbtn" type="button" value="确定添加" onclick="addok()"/>
                                </li>
                            </ul>
                        </div>
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
        $("#li2").find("a").addClass("aborder");
    })
</script>
