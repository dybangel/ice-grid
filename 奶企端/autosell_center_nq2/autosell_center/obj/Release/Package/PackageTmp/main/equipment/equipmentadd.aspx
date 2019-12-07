<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="equipmentadd.aspx.cs" Inherits="autosell_center.main.equipment.equipmentadd" %>
<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>全部设备-自动售卖终端中心控制系统</title>
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
                        <dt>设备管理<em class="fa fa-cog"></em></dt>
                        <dd>
                            <a class="change" href="productlist.aspx"><i class="change fa fa-inbox"></i>设备列表</a>
                        </dd>
                        <dd>
                            <a class="change acolor"><i class="change icolor fa fa-plus-square"></i>添加设备</a>
                        </dd>
                    </dl>
                </div>
                <section class="jiqlist">
                    <dl class="jiqlistTab">
                        <dd class="change ddcolor"><b>设备列表</b></dd>
                    </dl>
                    <ul class="jiqlistseach">
                        <li>
                            <input type="text" value="" placeholder="设备开启时间"/>
                        </li>
                        <li>
                            <input type="text" value="" placeholder="设备编号"/>
                        </li>
                        <li class="naiqBtn">
                            <input type="text" id="mulkWords" value="" placeholder="选择或填写奶企" />
                            <!--<i class="fa fa-caret-down"></i>-->
                            <dl>
                                <dt>支持字母输入</dt>
                                <dd title="伊利">伊利</dd>
                                <dd title="伊利">蒙牛</dd>
                                <dd title="伊利">光明</dd>
                                <dd title="伊利">伊利</dd>
                                <dd title="伊利">蒙牛</dd>
                                <dd title="伊利">光明</dd>
                                <dd title="伊利">伊利</dd>
                                <dd title="伊利">蒙牛</dd>
                                <dd title="伊利">光明</dd>
                                <dd title="伊利">伊利</dd>
                                <dd title="伊利">蒙牛</dd>
                                <dd title="伊利">光明</dd>
                            </dl>
                            <table>
                                <tr>
                                    <th>奶企名称/全拼/首字母</th>
                                </tr>
                                <tr>
                                    <td>伊利</td>
                                    <td>yili</td>
                                </tr>
                                <tr>
                                    <td>蒙牛</td>
                                    <td>mengniu</td>
                                </tr>
                                <tr>
                                    <td>光明</td>
                                    <td>guangming</td>
                                </tr>
                                <tr>
                                    <td>伊利</td>
                                    <td>yili</td>
                                </tr>
                                <tr>
                                    <td>蒙牛</td>
                                    <td>mengniu</td>
                                </tr>
                                <tr>
                                    <td>光明</td>
                                    <td>guangming</td>
                                </tr>
                            </table>
                        </li>
                        <li>
                            <input type="button" value="查询" class="seachbtn"/>
                        </li>
                    </ul>
                    <ul class="jiqlisttable" style="display: block;">
                        <li>
                            <dl>
                                <dd class="jiqname2">机器</dd>
                                <dd class="jiqtime2">有效期</dd>
                                <dd class="jiqnq">奶企</dd>
                                <dd class="jiqtime2">设备定位</dd>
                                <dd class="jiqzt2">状态</dd>
                                <dd class="jiqcz2">操作</dd>
                            </dl>
                            <label class="jiqname2">
                                <img src="/main/public/images/smjicon.png" alt=""/>
                                <span>SMJ3224568526</span>
                                <em>开启时间:<i>2018-02-09 12:12:12</i></em>
                            </label>
                            <label class="jiqtime2">2020-02-09 12:12:12过期</label>
                            <label class="jiqnq">光明乳业</label>
                            <label class="jiqtime2">青岛市黄岛区中央广场4楼32号机</label>
                            <label class="jiqzt2"><b>正常运行</b></label>
                            <label class="jiqcz2">
                                <a onclick="tingJi()">停机</a>
                                <a onclick="shanchu()">远程更新</a>
                            </label>
                        </li>
                        <li>
                            <dl>
                                <dd class="jiqname2">机器</dd>
                                <dd class="jiqtime2">有效期</dd>
                                <dd class="jiqnq">奶企</dd>
                                <dd class="jiqtime2">设备定位</dd>
                                <dd class="jiqzt2">状态</dd>
                                <dd class="jiqcz2">操作</dd>
                            </dl>
                            <label class="jiqname2">
                                <img src="/main/public/images/smjicon.png" alt=""/>
                                <span>SMJ3224568526</span>
                                <em>开启时间:<i>2018-02-09 12:12:12</i></em>
                            </label>
                            <label class="jiqtime2">2020-02-09 12:12:12过期</label>
                            <label class="jiqnq">光明乳业</label>
                            <label class="jiqtime2">青岛市黄岛区中央广场4楼32号机</label>
                            <label class="jiqzt2"><b>正常运行</b></label>
                            <label class="jiqcz2">
                                <a onclick="tingJi()">停机</a>
                                <a onclick="shanchu()">远程更新</a>
                            </label>
                        </li>
                        <li>
                            <dl>
                                <dd class="jiqname2">机器</dd>
                                <dd class="jiqtime2">有效期</dd>
                                <dd class="jiqnq">奶企</dd>
                                <dd class="jiqtime2">设备定位</dd>
                                <dd class="jiqzt2">状态</dd>
                                <dd class="jiqcz2">操作</dd>
                            </dl>
                            <label class="jiqname2">
                                <img src="/main/public/images/smjicon.png" alt=""/>
                                <span>SMJ3224568526</span>
                                <em>开启时间:<i>2018-02-09 12:12:12</i></em>
                            </label>
                            <label class="jiqtime2">2020-02-09 12:12:12过期</label>
                            <label class="jiqnq">光明乳业</label>
                            <label class="jiqtime2">青岛市黄岛区中央广场4楼32号机</label>
                            <label class="jiqzt2"><b>正常运行</b></label>
                            <label class="jiqcz2">
                                <a onclick="tingJi()">停机</a>
                                <a onclick="shanchu()">远程更新</a>
                            </label>
                        </li>
                        <li>
                            <dl>
                                <dd class="jiqname2">机器</dd>
                                <dd class="jiqtime2">有效期</dd>
                                <dd class="jiqnq">奶企</dd>
                                <dd class="jiqtime2">设备定位</dd>
                                <dd class="jiqzt2">状态</dd>
                                <dd class="jiqcz2">操作</dd>
                            </dl>
                            <label class="jiqname2">
                                <img src="/main/public/images/smjicon.png" alt=""/>
                                <span>SMJ3224568526</span>
                                <em>开启时间:<i>2018-02-09 12:12:12</i></em>
                            </label>
                            <label class="jiqtime2">2020-02-09 12:12:12过期</label>
                            <label class="jiqnq">光明乳业</label>
                            <label class="jiqtime2">青岛市黄岛区中央广场4楼32号机</label>
                            <label class="jiqzt2"><b>正常运行</b></label>
                            <label class="jiqcz2">
                                <a onclick="tingJi()">停机</a>
                                <a onclick="shanchu()">远程更新</a>
                            </label>
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
        $("#li3").find("a").addClass("aborder");
    });

    $(function() {
        $(".naiqBtn").find("input").focus(function () {
                $(this).parent().find("dl").show();
                $(this).parent().find("table").hide();
                this.select();
        });
        $(".jiqlisttable").click(function () {
            $(".naiqBtn").find("dl").hide();
        });

        $(".naiqBtn").find("dd").click(function () {
            var $ddName = $(this).html();
            $(".naiqBtn").find("input").val($ddName);
            $(this).parent("dl").hide();
        });

        $(".naiqBtn").find("input").keydown(function() {
            $(this).parent().find("dl").hide();
            $(this).parent().find("table").show();
        });



        $(".naiqBtn").find("input").keyup(function () {
            var $mulkWordsInput = $("#mulkWords").eq(0);
            var inputCon = $mulkWordsInput.val();
            var thName = $mulkWordsInput.parent().find("th").eq(0);
            //var $tdArry = $mulkWordsInput.parent().find("td");
            if (inputCon) {
                thName.html("按'" + inputCon + "'搜索");
                $(".naiqBtn").find("td").parent("tr").hide();
                $mulkWordsInput.parent().find("td").each(function () {
                    if ($(this).html().indexOf(inputCon) != -1) {
                        $(this).parent().show();
                    }
                });
            } else {
                thName.html("奶企名称/全拼/首字母");

            }
        });



        $(".naiqBtn").find("input").bind('input propertychange', function () {
            $(".naiqBtn").find("input").val($(this).val());
            //alert($(this).value);
        });

        $(".naiqBtn").find("tr").click(function () {
            var $tdName = $(this).find("td").eq(0).html();
            var $thTitle = $(this).find("th").length > 0;
            if ($thTitle === false) {
                $(".naiqBtn").find("input").val($tdName);
                $(".naiqBtn").find("table").hide();
            }
        });
    });
</script>