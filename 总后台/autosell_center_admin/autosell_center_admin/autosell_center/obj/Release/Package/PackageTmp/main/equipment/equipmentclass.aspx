<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="equipmentclass.aspx.cs" Inherits="autosell_center.main.equipment.equipmentclass" %>

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
                                <dd>
                                    <span>第一层</span>
                                    <select id="sel1">
                                        <option>1</option>
                                        <option>2</option>
                                        <option>3</option>
                                        <option>4</option>
                                        <option>5</option>
                                        <option>6</option>
                                        <option>7</option>
                                        <option>8</option>
                                        <option>9</option>
                                        <option>10</option>
                                    </select>
                                </dd>
                                <dd>
                                    <span>第二层</span>
                                    <select id="sel2">
                                        <option>1</option>
                                        <option>2</option>
                                        <option>3</option>
                                        <option>4</option>
                                        <option>5</option>
                                         <option>6</option>
                                        <option>7</option>
                                        <option>8</option>
                                        <option>9</option>
                                        <option>10</option>
                                    </select>
                                </dd>
                                <dd>
                                    <span>第三层</span>
                                    <select id="sel3">
                                        <option>1</option>
                                        <option>2</option>
                                        <option>3</option>
                                        <option>4</option>
                                        <option>5</option>
                                        <option>6</option>
                                        <option>7</option>
                                        <option>8</option>
                                        <option>9</option>
                                        <option>10</option>
                                    </select>
                                </dd>
                                <dd>
                                    <span>第四层</span>
                                    <select id="sel4">
                                        <option>1</option>
                                        <option>2</option>
                                        <option>3</option>
                                        <option>4</option>
                                        <option>5</option>
                                         <option>6</option>
                                        <option>7</option>
                                        <option>8</option>
                                        <option>9</option>
                                        <option>10</option>
                                    </select>
                                </dd>
                                <dd>
                                    <span>第五层</span>
                                    <select id="sel5">
                                        <option>1</option>
                                        <option>2</option>
                                        <option>3</option>
                                        <option>4</option>
                                        <option>5</option>
                                        <option>6</option>
                                        <option>7</option>
                                        <option>8</option>
                                        <option>9</option>
                                        <option>10</option>
                                    </select>
                                </dd>
                                <dd>
                                    <span>第六层</span>
                                    <select id="sel6">
                                        <option>1</option>
                                        <option>2</option>
                                        <option>3</option>
                                        <option>4</option>
                                        <option>5</option>
                                         <option>6</option>
                                        <option>7</option>
                                        <option>8</option>
                                        <option>9</option>
                                        <option>10</option>
                                    </select>
                                </dd>
                            </dl>
                            <ul id="jiqlistTabUl">
                            </ul>
                            <input type="button" value="保存" class="firmbtn"  onclick="add()"/>
                        </div>
                    </section>

                </div>
            </div>
        </div>
        <input  id="mechine_TypeID" runat="server" type="hidden"/>
    </form>
</body>
</html>
<script>
    $(function () {
        $("#li1").find("a").addClass("aborder");
        $.ajax({
            url: "../../ashx/asm.ashx",
            type: 'post',
            dataType: 'json',
            timeout: 10000,
            data: {
                action: "getLDInfo",
                type: $("#mechine_TypeID").val()
            },
            success: function (data) {
                var str = data.result.split('|');
                var liHtmlStr = '';
                for (var i = 0; i < str.length; i++) {
                    if (str[i].split('-')[1]=="0") {
                        //不可用
                        liHtmlStr += '<li class="change"><i class="ion change ioff"></i><em class="emoff fa fa-times change"></em><h4 class="change h4color">' + str[i].split('-')[0] + '</h4><span class="change spancolor">不可用</span><input type="hidden" value="' + str[i].split('-')[1] + '" /></li>';
                    } else {
                        //可用
                        liHtmlStr += '<li class="change"><i class="ion change"></i><em class="emon fa fa-check change"></em><h4 class="change">' + str[i].split('-')[0] + '</h4><span class="change">已开启</span><input type="hidden" value="' + str[i].split('-')[1] + '" /></li>';
                    }
                }
                var sel = data.sel.split('|');
               
                $("#sel1").val(sel[0]);
                $("#sel2").val(sel[1]);
                $("#sel3").val(sel[2]);
                $("#sel4").val(sel[3]);
                $("#sel5").val(sel[4]);
                $("#sel6").val(sel[5]);
                $("#jiqlistTabUl").html(liHtmlStr);
                $("#jiqlistTabUl").find("li").click(function () {
                    $(this).find("i").toggleClass("ioff");
                    //$(this).find("em").toggleClass("emoff");
                    $(this).find("h4").toggleClass("h4color");
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
    function add()
    {
        var str1="", str2="", str3="", str4="", str5="", str6="";
        $("h4.change").each(function () {
            var value = $(this).html(); // 001
            var input_val = $(this).parent().find("input").val();// 隐藏的值
            if (parseInt(value)<20)
            {
                str1 += value + "-" + input_val+"|";
            } else if (parseInt(value) < 30)
            {
                str2 += value + "-" + input_val + "|";
            } else if (parseInt(value) <40)
            {
                str3 += value + "-" + input_val + "|";
            } else if (parseInt(value) <50)
            {
                str4 += value + "-" + input_val + "|";
            } else if (parseInt(value) <60)
            {
                str5 += value + "-" + input_val + "|";
            } else if (parseInt(value) <70)
            {
                str6 += value + "-" + input_val + "|";
            }
            
        });
       
        $.ajax({
            url: "../../ashx/asm.ashx",
            type: 'post',
            dataType: 'json',
            timeout: 10000,
            data: {
                action: "addLD",
                type: $("#mechine_TypeID").val(),
                sel1: $("#sel1").val(),
                sel2: $("#sel2").val(),
                sel3: $("#sel3").val(),
                sel4: $("#sel4").val(),
                sel5: $("#sel5").val(),
                sel6: $("#sel6").val(),
                val1: str1, val2: str2, val3: str3, val4: str4, val5: str5, val6: str6
            },
            success: function (resultData) {
                alert("保存成功");
            }
        })

    }
</script>

