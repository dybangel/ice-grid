<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Distributoradd.aspx.cs" Inherits="autosell_center.main.enterprise.Distributoradd" %>

<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>添加设备-自动售卖终端中心控制系统</title>
    <meta charset="utf-8"/>
    <link rel="shortcut icon" href="/main/public/images/favicon.ico"/>
    <link href="/main/public/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css"/>
    <link href="/main/public/css/common.css" rel="stylesheet" type="text/css"/>
    <link href="/main/public/css/style.css" rel="stylesheet" type="text/css"/>
    <script src="/main/public/script/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="/main/public/script/Titlelist.js" type="text/javascript"></script>
</head>
<body>
<div class="header"></div>
<div class="main">
    <div class="main_list">
        <div class="common_title">
            <h4>
                <i class="fa fa-home"></i>
                <span>选择企业客户进行操作</span>
            </h4>
            <!--<a class="change" href="SellCenter.html">
            <i class="fa fa-reorder"></i>
            切换奶企
        </a>-->
        </div>
        <div class="common_main">
            <div class="navlist">
                <dl>
                    <dt>经销商管理<em class="fa fa-cog"></em></dt>
                    <dd>
                        <a class="change" href="Distributor.aspx"><i class="change fa fa-check-square"></i>经销商列表</a>
                    </dd>
                    <dd>
                        <a class="change acolor"><i class="change icolor fa fa-plus-square"></i>添加经销商</a>
                    </dd>
                </dl>
            </div>
            <section class="jiqlist">
                <div class="addnaiq">
                    <h4 class="commonfut"><a class="change" href="Distributor.html"><i class="change fa fa-angle-left"></i>经销商列表</a>/新增经销商</h4>
                    <!--<dl>
                        <dd class="ddcolor">1.添加奶企基本信息</dd>
                        <dd>2.上传相关文件图片</dd>
                        <dd>3.添加完成</dd>
                    </dl>-->
                    <ul class="addnaiqdata" id="addnaiqdata1" style="display: block;">
                        <li>
                            <label>经销商名称</label>
                            <div>
                                <input type="text" value="" placeholder="" />
                            </div>
                        </li>
                        <li>
                            <label>负责人</label>
                            <div>
                                <input type="text" value="" placeholder="" />
                            </div>
                        </li>
                        <li>
                            <label>联系电话</label>
                            <div>
                                <input type="text" value="" placeholder="" />
                            </div>
                        </li>
                        <li>
                            <label>店铺地址</label>
                            <div>
                                <input type="text" value="" placeholder="" />
                            </div>
                        </li>
                        <li>
                            <input class="firmbtn" type="button" value="添加" onclick="nextTow()" />
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
</body>
</html>
<script>
    $(function () {
        $("#pic").click(function () {
            $("#upload").click(); //隐藏了input:file样式后，点击头像就可以本地上传
            $("#upload").on("change", function () {
                var objUrl = getObjectURL(this.files[0]); //获取图片的路径，该路径不是图片在本地的路径
                if (objUrl) {
                    $("#pic").attr("src", objUrl); //将图片路径存入src中，显示出图片
                }
            });
        });

        $("#piclogo").click(function () {
            $("#uploadlogo").click(); //隐藏了input:file样式后，点击头像就可以本地上传
            $("#uploadlogo").on("change", function () {
                var objUrl = getObjectURL(this.files[0]); //获取图片的路径，该路径不是图片在本地的路径
                if (objUrl) {
                    $("#piclogo").attr("src", objUrl); //将图片路径存入src中，显示出图片
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
    function nextTow() {
        $("#addnaiqdata1").hide();
        $("#addnaiqdata2").show();
        $(".addnaiq").find("dd").removeClass("ddcolor");
        $(".addnaiq").find("dd").eq(1).addClass("ddcolor");
    }

    function preOne() {
        $("#addnaiqdata1").show();
        $("#addnaiqdata2").hide();
        $(".addnaiq").find("dd").removeClass("ddcolor");
        $(".addnaiq").find("dd").eq(0).addClass("ddcolor");
    }

    function nextthree() {
        $("#addnaiqdata3").show();
        $("#addnaiqdata2,#addnaiqdata1").hide();
        $(".addnaiq").find("dd").removeClass("ddcolor");
        $(".addnaiq").find("dd").eq(2).addClass("ddcolor");
    }
</script>
<script>
    $(function() {
        $("#li0").find("a").addClass("aborder");
    })
</script>