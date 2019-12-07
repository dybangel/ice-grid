<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Material.aspx.cs" Inherits="autosell_center.main.equipment.Material" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>料道设置-自动售卖终端中心控制系统</title>
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
                        <i class="fa fa-cubes"></i>
                        <span>设备管理</span>
                    </h4>
                </div>
                <div class="common_main">
                    <div class="navlist">
                        <dl>
                            <dt>料道设置<em class="fa fa-cog"></em></dt>
                            <dd>
                                <a class="change acolor"><i class="change icolor fa fa-inbox"></i>料道设置</a>
                            </dd>
                        </dl>
                    </div>
                    <section class="jiqlist">

                        <div class="materiallist">
                            <dl>
                                <dd>第一层</dd>
                                <dd>第二层</dd>
                                <dd>第三层</dd>
                                <dd>第四层</dd>
                                <dd>第五层</dd>
                                <dd>第六层</dd>
                            </dl>
                            <ul id="jiqlistTabUl">
                            </ul>
                            <input type="submit" value="刷新" class="firmbtn" />
                        </div>
                    </section>

                </div>
            </div>
        </div>
        <input  id="mechineID" runat="server" type="hidden"/>
    </form>
</body>
</html>
<script>
    $(function () {
        $("#li1").find("a").addClass("aborder");
        $("#li1").find("a").addClass("aborder");
        $.ajax({
            url: "../../ashx/asm.ashx",
            type: 'post',
            dataType: 'json',
            timeout: 10000,
            data: {
                action: "getLDInfoByMechineID",
                mechineID: $("#mechineID").val()
            },
            success: function (data) {
                var str = data.result.split('|');
                var liHtmlStr = '';
                for (var i = 0; i < str.length; i++) {
                    if (str[i].split('-')[1] == "0") {
                        //不可用
                        liHtmlStr += '<li class="change"><i class="ion change ioff"></i><em class="emoff fa fa-times change"></em><h4 class="change h4color">' + str[i].split('-')[0] + '</h4><span class="change spancolor">不可用</span><input type="hidden" value="' + str[i].split('-')[1] + '" /></li>';
                    } else {
                        //可用
                        liHtmlStr += '<li class="change"><i class="ion change"></i><em class="emon fa fa-check change"></em><h4 class="change">' + str[i].split('-')[0] + '</h4><span class="change">已开启</span><input type="hidden" value="' + str[i].split('-')[1] + '" /></li>';
                    }
                }
                $("#jiqlistTabUl").html(liHtmlStr);
                $("#jiqlistTabUl").find("li").click(function () {
                    $(this).find("i").toggleClass("ioff");
                    $(this).find("h4").toggleClass("h4color");
                     
                    update($(this).find("h4").html());
                     
                    if ($(this).find("i").hasClass("ioff")) {
                        $(this).find("input").val("0");
                        $(this).find("em").removeClass("emon fa-check");
                        $(this).find("em").addClass("emoff fa-times");
                        $(this).find("span").html("不可用").addClass("spancolor");
                    } else {
                        $(this).find("input").val("1");
                        $(this).find("em").removeClass("emoff fa-times");
                        $(this).find("em").addClass("emon fa-check");
                        $(this).find("span").html("已开启").removeClass("spancolor");
                    }
                });
            }
        })
    });
    function update(val)
    {
         
        $.ajax({
            url: "../../ashx/asm.ashx",
            type: 'post',
            dataType: 'json',
            timeout: 10000,
            data: {
                action: "updateLdInfo",
                mechineID: $("#mechineID").val(),
                ldNO:val
                 
            },
            success: function (resultData) {
                if (resultData.result=="1")
                {
                    alert("保存成功");
                } else if (resultData.result == "2")
                {
                    alert("保存失败");
                }
            }
        })
    }
</script>
