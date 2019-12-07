<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="firmcon.aspx.cs" Inherits="autosell_center.main.enterprise.firmcon" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>设备设置-自动售卖终端中心控制系统</title>
    <link rel="shortcut icon" href="/main/public/images/favicon.ico" />
    <link href="/main/public/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/common.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/style.css" rel="stylesheet" type="text/css" />
    <script src="/main/public/script/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="/main/public/script/Titlelist.js" type="text/javascript"></script>
    <link href="../../scripts/skin/jedate.css" rel="stylesheet" type="text/css" />
    <script src="../../scripts/jedate.js" type="text/javascript"></script>
    <script src="../../scripts/jedate.min.js"></script>
    <style>
        .firmconlist li dl{
            display:block;
        }
        .firmconlist li dl h4,.firmconlist li dl h5{
            display:none;
        }
        .firmconlist li dl dd{
            padding-left:30px;
        }
    </style>
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
                </div>
                <div class="common_main">
                    <div class="navlist">
                        <dl>
                            <dt>奶企设备<em class="fa fa-cog"></em></dt>
                            <dd>
                                <a class="change" href="devicelist.aspx?companyID=<%=company_id %>"><i class="change fa fa-check-square"></i>设备列表</a>
                            </dd>
                            <dd>
                                <a class="change acolor"><i class="change icolor fa fa-plus-square"></i>新增设备</a>
                            </dd>
                        </dl>
                        <dl>
                            <dt>奶企设置<em class="fa fa-cog"></em></dt>
                            <dd>
                                <a class="change" href="Thepublicjb.aspx?companyID=<%=company_id %>"><i class="change fa fa-wechat"></i>公众号设置</a>
                            </dd>
                          <%--  <dd>
                                <a class="change" href="Paymentzf.aspx?companyID=<%=company_id %>"><i class="change fa fa-money"></i>支付设置</a>
                            </dd>
                            <dd>
                                <a class="change" href="Profit.aspx?companyID=<%=company_id %>"><i class="change fa fa-database"></i>分润设置</a>
                            </dd>--%>
                        </dl>
                    </div>
                    <div class="jiqadd">
                        <section class="addnaiq">
                            <h4 class="commonfut"><a class="change" href="SellCenter.aspx"><i class="change fa fa-angle-left"></i>奶企列表</a>/添加设备</h4>
                            <div class="addfirmcon" >
                                <ul class="firmconlist" id="ull">
                                   <li> 
                                      <dl class="addfirmcon" id="dll">
                                           
                                        </dl>
                                    </li>
                                   
                                </ul>
                                <div class="zhuanImg">
                                    <img src="/main/public/images/zhuan.png" alt="" />
                                </div>
                                <ul class="firmcon_ok" id="selected">

                                </ul>
                                <dl class="firmconBtn">
                                    <dd>
                                        <input type="button" value="确定"  onclick="okbtn()"/>
                                    </dd>
                                    <dd></dd>
                                    <dd>
                                        <input type="button" value="取消" />
                                    </dd>
                                </dl>
                            </div>
                        </section>
                    </div>
                </div>
            </div>
        </div>
        <input id="companyID" runat="server" type="hidden" />
    </form>
</body>
</html>
<script>
    $(function () {
        $("#li0").find("a").addClass("aborder");
        sear();
        setTimeout(function () {
            $(".firmconlist").find("dd").click(function () {
                var $ddb = $(this).find("b");
                var $ddspan = $(this).find("span").html();
                var $spanHtml = $(this).find("span").attr("id");
                var $okli = '<li><em class="fa fa-bookmark"></em><span id=' + $spanHtml + '>' + $ddspan + '</span></li>';
                if ($ddb.hasClass("bcolor")) {
                    $ddb.removeClass("bcolor");
                    $(".firmcon_ok").find("#" + $spanHtml).parent("li").remove();
                } else {
                    $ddb.addClass("bcolor");
                    $(".firmcon_ok").append($okli);
                }
            });
        }, 500);
    });
    function okbtn()
    {
        var idStr = "";
        var $liOk = $(".firmcon_ok").find("li");
        $liOk.find("span").each(function () {
            idStr += $(this).attr("id") + ",";
        });
        idStr = idStr.substring(0, idStr.length - 1);
        if (idStr!="")
        {
            $.ajax({
                type: "post",
                url: "firmcon.aspx/okbtn",
                contentType: "application/json; charset=utf-8",
                data: "{id:'" + idStr + "',companyID:'" + $("#companyID").val() + "'}",
                dataType: "json",
                success: function (data) {
                    if (data.d == "1") {
                        alert("添加机器成功");
                    } else if(data.d=="2"){
                        alert("添加失败");
                    }
                }
            });
        }
    }
    function sear() {
        $("#dll").empty();
        $.ajax({
            type: "post",
            url: "firmcon.aspx/search",
            contentType: "application/json; charset=utf-8",
            data: "{}",
            dataType: "json",
            success: function (data) {
                var serverdata = $.parseJSON(data.d);
                var serverdatalist = serverdata.length;
                for (var i = 0; i < serverdatalist; i++) {
                    $(" <dd>"
                               +"<h5></h5>"
                               +"<h4></h4>"
                               +"<em class='fa fa-bookmark'></em>"
                               +"<span id='"+serverdata[i].id+"'>"+serverdata[i].bh+"</span>"
                               +"<b class='fa fa-check-circle-o'></b>"
                               +"</dd>").appendTo("#dll");
                }
            }
        });
    };
    function a(obj)
    {
        var $liid = obj.id;
        var $aaa = $("#" + obj.id + "").attr("id");
        var $iOn = $("");
        if ($("#" + obj.id + "").hasClass("fa-plus-square-o")) {
            $("#" + obj.id + "").removeClass("fa-plus-square-o");
            $("#" + obj.id + "").addClass("fa-minus-square-o");
            $("#" + obj.id + "").parent().find("dl").show();
            $("#" + obj.id + "").parent().find("h1").addClass("h1height");
        } else {
            $("#" + obj.id + "").removeClass("fa-minus-square-o");
            $("#" + obj.id + "").addClass("fa-plus-square-o");
            $("#" + obj.id + "").parent().find("dl").hide();
            $("#" + obj.id + "").parent().find("h1").removeClass("h1height");
        }
    }
    $(function () {
        
    });
</script>
