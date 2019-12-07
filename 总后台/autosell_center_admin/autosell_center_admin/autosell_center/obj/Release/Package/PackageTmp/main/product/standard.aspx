<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="standard.aspx.cs" Inherits="autosell_center.main.product.standard" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>产品规格-自动售卖终端中心控制系统</title>
    <link rel="shortcut icon" href="/main/public/images/favicon.ico" />
    <link href="/main/public/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/common.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/style.css" rel="stylesheet" type="text/css" />
    <script src="/main/public/script/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="/main/public/script/Titlelist.js" type="text/javascript"></script>
    <style>
        .firmbtn {
            margin-left: 30px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="header"></div>
        <div class="main">
            <div id="addCaDiv" class="addDiv change">
                <h4>添加规格</h4>
                <ul>
                    <li>
                        <label>规格</label>
                        <input type="text" value="" placeholder="填写规格" />
                    </li>
                    <li>
                        <label>说明</label>
                        <input type="text" value="" placeholder="规格说明" />
                    </li>
                    <li>
                        <label></label>
                        <input type="button" value="确认添加" class="btnok" />
                        <input type="button" value="取消" class="btnoff" onclick="divOff()" />
                    </li>
                </ul>
            </div>
            <div id="updataCaDiv" class="addDiv change">
                <h4>修改规格</h4>
                <ul>
                    <li>
                        <label>规格</label>
                        <input type="text" id="updateCId" value="" placeholder="填写规格" />
                    </li>
                    <li>
                        <label>说明</label>
                        <input type="text" id="updateCN" value="" placeholder="规格说明" />
                    </li>
                    <li>
                        <label></label>
                        <input type="button" value="确认修改" class="btnok" />
                        <input type="button" value="取消" class="btnoff" onclick="divOff()" />
                    </li>
                </ul>
            </div>
            <div class="popupbj"></div>
            <div class="main_list">
                <div class="common_title">
                    <h4>
                        <i class="fa fa-cubes"></i>
                        <span>产品管理</span>
                    </h4>
                </div>
                <div class="common_main">
                    <div class="navlist">
                        <dl>
                            <dt>产品信息<em class="fa fa-cog"></em></dt>
                            <dd>
                                <a class="change" href="productlist.aspx"><i class="change fa fa-file-text"></i>产品列表</a>
                            </dd>
                            <dd>
                                <a class="change" href="productclass.aspx"><i class="change fa fa-window-restore"></i>产品类别</a>
                            </dd>
                        </dl>
                        <dl>
                            <dt>规格与保质期<em class="fa fa-cog"></em></dt>
                            <dd>
                                <a class="change acolor"><i class="change icolor fa fa-tags"></i>产品规格</a>
                            </dd>
                            <dd>
                                <a class="change" href="standardbzq.aspx"><i class="change fa fa-shield"></i>产品保质期</a>
                            </dd>
                        </dl>
                    </div>
                    <section class="jiqlist">
                        <dl class="jiqlistTab">
                            <dd class="change ddcolor"><b>产品规格</b></dd>
                        </dl>
                        <div class="addClass">
                            <a class="change" onclick="addDiv()">
                                <i class="fa fa-plus"></i>
                                新的规格
                            </a>
                        </div>
                        <ul class="productclass">
                            <li>
                                <label class="classid">排序</label>
                                <label class="classname">分类名称</label>
                                <label class="classdelete">操作</label>
                            </li>
                            <li>
                                <span class="classid">1</span>
                                <span class="classname">产品类别1</span>
                                <span class="classdelete">
                                    <a onclick="updeted(this)">修改</a>
                                    <a onclick="deleTed()">删除</a>
                                </span>
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
    function addDiv() {
        $("#addCaDiv").addClass("addDivshow");
        setTimeout(function() {
            $(".popupbj").fadeIn();
        }, 100);
    }
    function divOff() {
        $(".popupbj").hide();
        $(".addDiv").removeClass("addDivshow");
    }
    //修改弹框
    function updeted(obj) {
        var $a = $(obj);
        var $li = $a.parent().parent().eq(0);
        $("#updateCN").val($li.find("span.classname").eq(0).html());
        $("#updateCId").val($li.find("span.classid").eq(0).html());
        $("#updataCaDiv").addClass("addDivshow");
        setTimeout(function() {
            $(".popupbj").fadeIn();
        }, 100);
    }
</script>
<script>
    $(function () {
        $("#li3").find("a").addClass("aborder");
    })
</script>
