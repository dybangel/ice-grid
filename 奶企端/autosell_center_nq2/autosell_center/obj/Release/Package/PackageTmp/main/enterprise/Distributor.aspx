<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Distributor.aspx.cs" Inherits="autosell_center.main.enterprise.Distributor" %>

<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>首页-自动售卖终端中心控制系统</title>
    <meta charset="utf-8" />
    <link rel="shortcut icon" href="/main/public/images/favicon.ico" />
    <link href="/main/public/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/common.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/style.css" rel="stylesheet" type="text/css" />
    <script src="/main/public/script/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="/main/public/script/Titlelist.js" type="text/javascript"></script>
    <style>
        .productclass li .classid,.productclass li .classname{
            width:20%;
        }
        .productclass li .classdelete{
            width:60%;
        }
        .productclass li .classdelete input{
            width:80%;
        }
        .productclass{
            margin-top:30px;
        }
    </style>
</head>
<body>
    <form>
        <div class="header"></div>
    <div class="main">
        <div class="main_list">
            <div class="common_title">
                    <h4>
                        <i class="fa fa-plus"></i>
                        <span>公众号设置</span>
                    </h4>
                </div>
            <div class="common_main">
                <div class="navlist">
                        <dl>
                            <dt>公众号设置<em class="fa fa-cog"></em></dt>
                            <dd>
                                <a class="change acolor" href="authPage.aspx"><i class="change icolor fa fa-wechat"></i>公众号设置</a>
                            </dd>
                        </dl>
                         <dl>
                            <dt>自定义菜单设置<em class="fa fa-cog"></em></dt>
                            <dd>
                                <a class="change " href="Thepublicjb.aspx"><i class="change  fa fa-wechat"></i>自定义菜单设置</a>
                            </dd>
                        </dl>
                        <dl>
                            <dt>模板消息<em class="fa fa-cog"></em></dt>
                            <dd>
                                <a class="change" href="Distributor.aspx"><i class="change fa fa-envelope"></i>模板消息</a>
                            </dd>
                        </dl>
                         <dl>
                            <dt>参数设置<em class="fa fa-cog"></em></dt>
                            <dd>
                                 <a class="change" href="syssetting.aspx"><i class="change fa fa-bars"></i>参数设置</a>
                            </dd>
                        </dl>
                     <%--  <dl>
                            <dt>App流量统计<em class="fa fa-cog"></em></dt>
                            <dd>
                                 <a class="change" href="applog.aspx"><i class="change fa fa-bars"></i>App流量统计</a>
                            </dd>
                        </dl>--%>
                    </div>
                <section class="jiqlist">
                    <dl class="jiqlistTab">
                        <dd class="change ddcolor"><b>模板消息</b></dd>
                    </dl>
                    <ul class="productclass" id="ull">
                        
                    </ul>
                      <input type="button" value="保存" class="seachbtnTwo" onclick="save()"/>
                </section>
              
            </div>
        </div>
    </div>
        <input  id="companyID" runat="server" type="hidden"/>
         <input id="_operaID" runat="server" type="hidden" />
    </form>
    
</body>
</html>
<script>
    function judge() {
        $.ajax({
            type: "post",
            url: "Distributor.aspx/judge",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{operaID:'" + $("#_operaID").val() + "',menuID:'gzhsz'}",
            success: function (data) {
                if (data.d.code == "500") {
                    $(".jiqlist").empty();
                    $(".jiqlist").html("<p class='noquanxian'>当前没有权限</p>").css({ "background": "#ddd" });
                }
            }
        })
    }
    $(function () {
        judge();
        $("#li8").find("a").addClass("aborder");
        sear();
    })
    function sear()
    {
        $(" <li>"
            + "<label class='classid'>消息标题</label>"
            + "<label class='classname'>模板编号</label>"
            + "<label class='classdelete'>微信模板ID</label>"
            + "</li>").appendTo("#ull");
        $("#ull").empty();
        $.ajax({
            type: "post",
            url: "Distributor.aspx/getMessageList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{companyID:'" + $("#companyID").val() + "'}",
            success: function (data) {
                var serverdata = $.parseJSON(data.d);
                var serverdatalist = serverdata.length;
                for (var i = 0; i < serverdatalist; i++) {
                    $(" <li class='classll'>"
                          + " <span class='classid'>" + serverdata[i].templateTitle + "</span>"
                          + " <span class='classname'>" + serverdata[i].templateBH + "</span>"
                          +"  <span class='classdelete'>"
                          + "      <input type='text' value='" + serverdata[i].templateID + "' placeholder='微信模板ID' />"
                          +"  </span>"
                        +"</li>").appendTo("#ull");
                }
            }
        })
    }
    function save()
    {
        var $li = $("#ull").find("")
        var bh = "";
        var id = "";
        $(".classll").each(function () {
            bh += $(this).find("span").eq(1).html() + ",";
            id += $(this).find("input").val()+","
            
        });
        $.ajax({
            type: "post",
            url: "Distributor.aspx/save",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{companyID:'" + $("#companyID").val() + "',bh:'"+bh+"',id:'"+id+"'}",
            success: function (data) {
                if(data.d=="1")
                {
                    alert("保存成功");
                }
            }
        })
    }
    
</script>
