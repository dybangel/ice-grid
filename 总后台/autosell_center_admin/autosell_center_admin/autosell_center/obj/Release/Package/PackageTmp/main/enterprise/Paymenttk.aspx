<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Paymenttk.aspx.cs" Inherits="autosell_center.main.enterprise.Paymenttk" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>支付设置-自动售卖终端中心控制系统</title>
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
                                <a class="change" href="Thepublicjb.aspx"><i class="change fa fa-wechat"></i>公众号设置</a>
                            </dd>
                            <dd>
                                <a class="change acolor"><i class="change icolor fa fa-money"></i>支付设置</a>
                            </dd>
                            <dd>
                                <a class="change" href="Profit.aspx"><i class="change fa fa-database"></i>分润设置</a>
                            </dd>
                        </dl>
                    </div>
                    <section class="jiqlist">
                        <dl class="jiqlistTab">
                            <dd class="change" onclick="javascript: location.href = 'Paymentzf.aspx';">支付配置</dd>
                            <dd class="change ddcolor">退款配置</dd>
                        </dl>
                        <ul class="thepublic">
                            <li>
                                <dl>
                                    <dd>公众号设置</dd>
                                </dl>
                            </li>
                            <li>
                                <label>头像</label>
                                <img id="pic" src="public/images/nopic.png" alt="" />
                                <input type="file" id="upload" name="file" value="" style="display: none;" />
                                <a id="pica" class="public_p">修改</a>
                            </li>
                            <li>
                                <label>二维码</label>
                                <img id="pic2" src="public/images/nopic.png" alt="" />
                                <input type="file" id="upload2" name="file" value="" style="display: none;" />
                                <a id="picb" class="public_p">修改</a>
                            </li>
                            <li>
                                <label>公众号名称</label>
                                <span></span>
                                <a class="public_p">修改</a>
                            </li>
                            <li>
                                <label>账号</label>
                                <span></span>
                                <a class="public_p">修改</a>
                            </li>
                            <li>
                                <label>原始ID</label>
                                <span></span>
                                <a class="public_p">修改</a>
                            </li>
                            <li>
                                <label>类型</label>
                                <span>普通订阅号</span>
                                <a class="public_p">修改</a>
                            </li>
                            <li>
                                <label>接入方式</label>
                                <span>普通接入</span>
                                <!--<a class="public_p">修改</a>-->
                            </li>
                            <li>
                                <label>到期时间</label>
                                <span>永久<em>（随该公众号主管理员的到期时间，未设置主管理员时默认为创始人）</em></span>
                                <a class="public_p">修改</a>
                            </li>
                        </ul>
                        <ul class="thepublic">
                            <li>
                                <dl>
                                    <dd>自定义菜单通讯设置</dd>
                                </dl>
                            </li>
                            <li>
                                <label>AppId</label>
                                <span></span>
                                <a class="public_p">修改</a>
                            </li>
                            <li>
                                <label>AppSecret</label>
                                <span></span>
                                <a class="public_p">修改</a>
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
        $("#pica").click(function () {
            $("#upload").click(); //隐藏了input:file样式后，点击头像就可以本地上传
            $("#upload").on("change", function () {
                var objUrl = getObjectURL(this.files[0]); //获取图片的路径，该路径不是图片在本地的路径
                if (objUrl) {
                    $("#pic").attr("src", objUrl); //将图片路径存入src中，显示出图片
                }
            });
        });
        $("#picb").click(function () {
            $("#upload2").click(); //隐藏了input:file样式后，点击头像就可以本地上传
            $("#upload2").on("change", function () {
                var objUrl = getObjectURL(this.files[0]); //获取图片的路径，该路径不是图片在本地的路径
                if (objUrl) {
                    $("#pic2").attr("src", objUrl); //将图片路径存入src中，显示出图片
                }
            });
        });
    });

    //建立一個可存取到該file的url
    function getObjectURL(file) {
        var url = null;
        if (window.createObjectURL != undefined) { // basic
            url = window.createObjectURL(file);
        } else if (window.URL != undefined) { // mozilla(firefox)
            url = window.URL.createObjectURL(file);
        } else if (window.webkitURL != undefined) { // webkit or chrome
            url = window.webkitURL.createObjectURL(file);
        }
        return url;
    }
</script>
<script>
    $(function () {
        $("#li0").find("a").addClass("aborder");
    })
</script>
