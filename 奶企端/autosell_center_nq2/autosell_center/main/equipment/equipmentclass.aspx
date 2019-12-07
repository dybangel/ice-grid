<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="equipmentclass.aspx.cs" Inherits="autosell_center.main.equipment.equipmentclass" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
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
            <!--00未设置  11已选中 1订购 2零售-->
            <div class="main_list">
                <div class="common_title">
                    <h4>
                        <i class="fa fa-plus"></i>
                        <span>设备管理</span>
                    </h4>
                </div>
                <div class="common_main">
                    <div class="navlist">
                        <dl>
                            <dt>设备管理<em class="fa fa-cog"></em></dt>
                            <dd>
                                <a class="change" href="equipmentlist.aspx"><i class="change fa fa-inbox"></i>设备列表</a>
                            </dd>
                        </dl>
                    </div>
                    <section class="jiqlist">
                        <dl class="jiqlistTab">
                            <dd class="change ddcolor"><b>设备编号：<%=dt.Rows[0]["bh"].ToString() %></b></dd>
                        </dl>
                        <div class="memberjs">
                            <p>
                                <em class="fa fa-info"></em>
                                <span><i><b></b><a>表示未设置当前料道；</a><b></b><a>表示选中当前料道；</a><b></b><a>表示当前料道为订购料道；</a><b></b><a>表示当前料道为零售料道.</a></i></span>
                            </p>
                        </div>
                        <div class="setbtn">
                            <i class="fa fa-check" onclick="allliao()"></i>
                            <em onclick="allliao()"></em>
                            <span onclick="allliao()">全选</span>
                            <input type="button" value="批量设为订购" class="setding" onclick="dingOn()" />
                            <input type="button" value="批量设为零售" class="setling" onclick="lingOn()" />
                            <input type="button" value="批量设为空料道" class="setqing" onclick="qingOn()" />
                        </div>
                        <div class="liaolist">
                            <h4>
                                <i class="fa fa-inbox"></i>
                                选择料道进行操作
                            </h4>
                            <div class="materall">
                                <dl>
                                    <dd>点击选择</dd>
                                </dl>
                                <ul id="liaodaoUl" class="materlist">
                                   <%-- <li class="liling"><i></i><em class="fa fa-check-circle commoncolor"></em><span class="commoncolor">10</span><b class="commoncolor">零售</b><input type="hidden" value="2"></li>
                                    <li class="liling"><i></i><em class="fa fa-check-circle commoncolor"></em><span class="commoncolor">11</span><b class="commoncolor">零售</b><input type="hidden" value="2"></li>
                                    <li class="liling"><i></i><em class="fa fa-check-circle commoncolor"></em><span class="commoncolor">12</span><b class="commoncolor">零售</b><input type="hidden" value="2"></li>
                                    <li class="liling"><i></i><em class="fa fa-check-circle commoncolor"></em><span class="commoncolor">13</span><b class="commoncolor">零售</b><input type="hidden" value="2"></li>
                                    <li class="liling"><i></i><em class="fa fa-check-circle commoncolor"></em><span class="commoncolor">14</span><b class="commoncolor">零售</b><input type="hidden" value="2"></li>
                                    <li class="liling"><i></i><em class="fa fa-check-circle commoncolor"></em><span class="commoncolor">15</span><b class="commoncolor">零售</b><input type="hidden" value="2"></li>
                                    <li class="liling"><i></i><em class="fa fa-check-circle commoncolor"></em><span class="commoncolor">16</span><b class="commoncolor">零售</b><input type="hidden" value="2"></li>
                                    <li class="liling"><i></i><em class="fa fa-check-circle commoncolor"></em><span class="commoncolor">17</span><b class="commoncolor">零售</b><input type="hidden" value="2"></li>
                                    <li class="liling"><i></i><em class="fa fa-check-circle commoncolor"></em><span class="commoncolor">18</span><b class="commoncolor">零售</b><input type="hidden" value="2"></li>
                                    <li class="liling"><i></i><em class="fa fa-check-circle commoncolor"></em><span class="commoncolor">19</span><b class="commoncolor">零售</b><input type="hidden" value="2"></li>--%>
                                </ul>
                            </div>
                        </div>
                    </section>
                </div>
            </div>
        </div>
        <input id="mechine_id"   runat="server" type="hidden"/>
    </form>
</body>
</html>
<script>
    $(function () {
        $("#li3").find("a").addClass("aborder");
        var $liHtml = '';
        $.ajax({
            type: "post",
            url: "equipmentclass.aspx/getLDInfo",
            contentType: "application/json; charset=utf-8",
            data: "{mechineID:'" + $("#mechine_id").val() + "'}",
            dataType: "json",
            success: function (data) {
                $("#ull").empty();
                var serverdata = $.parseJSON(data.d);
                var serverdatalist = serverdata.length;
                for (var i = 0; i < serverdatalist; i++) {
                    if (serverdata[i].type == "0") {
                        $("<li><i></i><em class='fa fa-circle-o'></em><span>" + serverdata[i].ldNO + "</span><b>未设置</b><input type='hidden' value='00' /></li>").appendTo("#liaodaoUl");
                    } else {
                        $("<li><i></i><em class='fa fa-circle-o'></em><span>" + serverdata[i].ldNO + "</span><b>未设置</b><input type='hidden' value='" + serverdata[i].type + "' /></li>").appendTo("#liaodaoUl");
                    }
                }
            }
        });
        setTimeout(function () {
            var $mateLi = $(".materlist").find("li");
            $mateLi.each(function () {
                if ($(this).find("input").val() == "1") {
                    $(this).addClass("liding");
                    $(this).find("em").removeClass("fa-circle-o").addClass("fa-check-circle commoncolor");
                    $(this).find("span").addClass("commoncolor");
                    $(this).find("b").addClass("commoncolor").html("订购");
                } else if ($(this).find("input").val() == "2") {
                    $(this).addClass("liling");
                    $(this).find("em").removeClass("fa-circle-o").addClass("fa-check-circle commoncolor");
                    $(this).find("span").addClass("commoncolor");
                    $(this).find("b").addClass("commoncolor").html("零售");
                } else if ($(this).find("input").val() == "11") {
                    $(this).removeClass("liling,liding").addClass("licolor");
                    $(this).find("em").addClass("fa-check-circle emcolor").removeClass("fa-circle-o commoncolor");
                    $(this).find("span").removeClass("commoncolor").addClass("spancolor");
                    $(this).find("b").removeClass("commoncolor").html("未设置").addClass("bcolor");
                } else if ($(this).find("input").val() == "00") {
                    $(this).removeClass("liling,liding");
                    $(this).find("em").addClass("fa-circle-o").removeClass("fa-check-circle commoncolor");
                    $(this).find("span").removeClass("commoncolor");
                    $(this).find("b").removeClass("commoncolor").html("未设置");
                }

            });

            $(".materlist").find("li").click(function () {
                if ($(this).find("input").val() == "00") {
                    $(".materlist").find(".liding,.liling").addClass("liop");
                    $(this).addClass("licolor").removeClass("liop");
                    $(this).find("em").removeClass("fa-circle-o").addClass("fa-check-circle emcolor");
                    $(this).find("span").addClass("spancolor");
                    $(this).find("b").addClass("bcolor").html("已选中");
                    $(this).find("input").val("11");
                } else if ($(this).find("input").val() == "1" || $(this).find("input").val() == "2" || $(this).find("input").val() == "11") {
                    $(this).removeAttr("class");
                    $(this).find("em").addClass("fa-circle-o").removeClass("fa-check-circle emcolor commoncolor");
                    $(this).find("span").removeClass("spancolor commoncolor");
                    $(this).find("b").removeClass("bcolor commoncolor").html("未设置");
                    $(this).find("input").val("00");
                }
            });

        }, 500);
       
    });

    function allliao() {
        if ($(".setbtn").find("i").is(":hidden")) {
            $(".setbtn").find("i").show();
            $("#liaodaoUl").find("li").addClass("licolor");
            $("#liaodaoUl").find("li").find("input").val("11");
        } else {
            $(".setbtn").find("i").hide();
            $("#liaodaoUl").find("li").removeClass("licolor");
            location.reload();
        }
    }

    function dingOn() {
        var str = "";
        $("#liaodaoUl").find("li").each(function () {
            if ($(this).find("input").val() == "11") {
                $(this).find("input").val("1");
                str += $(this).find("span").html()+",";
            }
        });
        str = str.substring(0, str.length - 1);
        $.ajax({
            url: "../../ashx/asm.ashx",
            type: 'post',
            dataType: 'json',
            timeout: 10000,
            data: {
                action: "setDG",
                mechineID: $("#mechine_id").val(),
                ldNO: str
            },
            success: function (resultData) {
        
                if (resultData.result == "1") {
                    alert("批量设置订购成功!");
                   
                } else if (resultData.result == "2") {
                    alert("设置失败");
                }
                location.reload();
            }
        })
    }
    function lingOn() {
        var str = "";
        $("#liaodaoUl").find("li").each(function () {
            if ($(this).find("input").val() == "11") {
                $(this).find("input").val("2");
                str += $(this).find("span").html()+",";
            }
        });
        str = str.substring(0, str.length - 1);
        $.ajax({
            url: "../../ashx/asm.ashx",
            type: 'post',
            dataType: 'json',
            timeout: 10000,
            data: {
                action: "setLS",
                mechineID: $("#mechine_id").val(),
                ldNO: str
            },
            success: function (resultData) {
                if (resultData.result == "1") {
                    alert("批量设置订购成功!");
                   
                } else if (resultData.result == "2") {
                    alert("设置失败");
                }
                location.reload();
            }

        })
        alert("批量设置零售成功!");
        location.reload();
    }

    function qingOn(){
        var str = "";
        $("#liaodaoUl").find("li").each(function () {
            if ($(this).find("input").val() == "11") {
                $(this).find("input").val("2");
                str += $(this).find("span").html() + ",";
            }
        });
        str = str.substring(0, str.length - 1);
        
        $.ajax({
            url: "../../ashx/asm.ashx",
            type: 'post',
            dataType: 'json',
            timeout: 10000,
            data: {
                action: "setQK",
                mechineID: $("#mechine_id").val(),
                ldNO: str
            },
            success: function (resultData) {
                if (resultData.result == "1") {
                    alert("批量设置订购成功!");
                } else if (resultData.result == "2") {
                    alert("设置失败");
                }
                location.reload();
            }

        })
        
    }
</script>
