<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Thepublicjb.aspx.cs" Inherits="autosell_center.main.enterprise.Thepublicjb" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>公众号设置-自动售卖终端中心控制系统</title>
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
                      <%-- <dl>
                            <dt>App流量统计<em class="fa fa-cog"></em></dt>
                            <dd>
                                 <a class="change" href="applog.aspx"><i class="change fa fa-bars"></i>App流量统计</a>
                            </dd>
                        </dl>--%>
                       
                    </div>
                    <section class="jiqlist">
                        <div class="change zfpopup">
                            <h4>菜单设置</h4>
                            <ul>
                                <li>
                                    <h5></h5>
                                    <label></label>
                                </li>
                                <li style="">
                                    <h5></h5>
                                    <label></label>
                                </li>
                            </ul>
                            <input type="hidden" value="" id="weChatID" runat="server" />
                            <dl>
                                <dd>
                                    <input type="button" value="确定" class="popupqdbtn" />

                                </dd>
                                <dd>
                                    <input type="button" value="取消" onclick="qxClick()" />
                                </dd>
                            </dl>
                        </div>
                        <div class="popupbj"></div>
                        <dl class="jiqlistTab">
                            <dd class="change ddcolor"><b>基础信息</b></dd>
                        </dl>
                        <div class="wechatsetup">
                            <div class="wechatsetupleft">
                                <div class="moblietop">
                                    <i></i>
                                    <div class="moblietitle">主页</div>
                                </div>
                                <div class="mobliemain">
                                    <div class="white-box"></div>
                                    <div class="moblienav">
                                        <dl>
                                            <%
                                                if (dt.Rows.Count > 0)
                                                {
                                                    for (int i = 0; i < dt.Rows.Count; i++)
                                                    {%>

                                            <dd><%=dt.Rows[i]["name"].ToString() %></dd>
                                            <%}
                                                }
                                            %>
                                        </dl>
                                    </div>
                                </div>
                                <div class="clear-line">
                                    <div class="mobile-footer"></div>
                                </div>
                            </div>


                            <div class="wechatsetupright">
                                <dl class="wechatnavlist">
                                    <%
                                        if (dt.Rows.Count > 0)
                                        {
                                            for (int i = 0; i < dt.Rows.Count; i++)
                                            {%>
                                    <dd id="<%=dt.Rows[i]["id"].ToString() %>" onclick="GradeType('<%=dt.Rows[i]["id"].ToString() %>','<%=dt.Rows[i]["name"].ToString() %>')"><i onclick="del(this)">×</i><%=dt.Rows[i]["name"].ToString() %></dd>
                                    <%}
                                        }
                                    %>
                                    <dt onclick="addwechatNavs()">添加一级菜单</dt>
                                </dl>

                                <%
                                    if (dt.Rows.Count > 0)
                                    {
                                        for (int i = 0; i < dt.Rows.Count; i++)
                                        {%>
                                <div class="wechatnavmain">
                                    <h2>一级菜单</h2>
                                    <div class="wechatfirst">
                                        <span id="<%=dt.Rows[i]["id"].ToString() %>"><%=dt.Rows[i]["name"].ToString() %></span>

                                       <p>
                                            <a onclick="aNav(this)">编辑</a>
                                         <%--  <em>设置二级菜单以后，主链接已失效。</em>--%>
                                            <input type="hidden" value="<%=dt.Rows[i]["url"].ToString() %>" class="hiddenUrl" />
                                        </p>
                                    </div>
                                  <%--  <h4>二级菜单</h4>
                                    <ul class="erNavlist">
                                        <%
                                            if (dt7!=null&&dt7.Rows.Count>0) {
                                                for (int m = 0;m < dt7.Rows.Count;m++) { 
                                                %>

                                         <li>
                                            <span id="<%=dt7.Rows[m]["id"].ToString() %>"><%=dt7.Rows[m]["name"].ToString() %></span>
                                            <p>
                                                <a onclick='bNav(this)'>编辑</a>
                                                <a onclick='delTwo(<%=dt7.Rows[m]["id"].ToString() %>)'>删除</a>
                                                <input type='hidden' value='<%=dt7.Rows[m]["url"].ToString() %>' id='hiddenerUrl'  />

                                            </p>
                                        </li>
                                                <% }

                                            } %>

                                    </ul>
                                    <a class="addwechatnav">添加二级菜单</a>--%>
                                </div>

                                <%}
                                    }
                                %>                              
                            </div>
                        </div>
                        <asp:Button ID="Button2" class="seachbtnTwo" runat="server" Text="保存" OnClick="Button2_Click1" />
                      
                    </section>
                </div>
            </div>
        </div>
        <input id="com_id" runat="server" type="hidden"/>
        <input id="HF_accesstoken" runat="server" type="hidden"/>
         <input id="_operaID" runat="server" type="hidden" />
    </form>
</body>
</html>
<script>
    function judge() {
        $.ajax({
            type: "post",
            url: "Thepublicjb.aspx/judge",
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
        judge()
        $("#li8").find("a").addClass("aborder");
        var ddNum = $(".wechatnavlist").find("dd").length;
        if (ddNum >= 4) {
            $(".wechatnavlist").find("dt").hide();
        } else {
            $(".wechatnavlist").find("dt").show();
        }

        $(".wechatnavlist").find("dd").eq(0).addClass("ddcolor");
        $(".wechatnavmain").eq(0).show();


    });
    function save()
    {
        $.ajax({
            url: "Thepublicjb.aspx/save",
            type: "post",                //请求发送方式
            contentType: "application/json; charset=utf-8",
            dataType: "json",       //返回值有： text 为纯文本，json对象，html页面
            data: "{companyID:'" + $("#com_id").val() + "'} ",
            //请求成功执行
            success: function (data, textStatus) {
                var serverdata = $.parseJSON(data.d);
                var serverdatalist = serverdata.length;
                var liHtml = "";
                for (var i = 0; i < serverdatalist; i++) {
                    liHtml += "<li><span id=" + serverdata[i].id + ">" + serverdata[i].name + "</span><p><a onclick='bNav(this)'>编辑</a><a onclick='delTwo(" + serverdata[i].id + ")'>删除</a><input type='hidden' value='" + serverdata[i].url + "' id='hiddenerUrl'  /></p></li>";
                }
                $(".erNavlist").html(liHtml);
            }
        });
    }

    function GradeType(id, name) {
        $(".erNavlist").empty();
        $.ajax({
            url: "Thepublicjb.aspx/GradeType",
            type: "post",                //请求发送方式
            contentType: "application/json; charset=utf-8",
            dataType: "json",       //返回值有： text 为纯文本，json对象，html页面
            data: "{id:'" + id + "',comID:'" + $("#com_id").val() + "'} ",
            //请求成功执行
            success: function (data, textStatus) {
                var serverdata = $.parseJSON(data.d);
                var serverdatalist = serverdata.length;
                var liHtml = "";
                for (var i = 0; i < serverdatalist; i++) {
                     liHtml += "<li><span id=" + serverdata[i].id + ">" + serverdata[i].name + "</span><p><a onclick='bNav(this)'>编辑</a><a onclick='delTwo(" + serverdata[i].id + ")'>删除</a><input type='hidden' value='" + serverdata[i].url + "' id='hiddenerUrl'  /></p></li>";
                }
                $(".erNavlist").html(liHtml);
            }
        });
    }



    //确定
    function oneGrade() {
        var name = $("#TitleOne").val();
        var url = $("#ljOne").val();
        $.ajax({
            url: "Thepublicjb.aspx/oneGrade",
            type: "post",                //请求发送方式
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{name:'" + name + "',url:'" + url + "',companyID:'" + $("#com_id").val() + "'}",
            //请求成功执行
            success: function (data) {
                alert(data.d);
                window.location.reload(true);
                return;

            }
        });
    }

    function updateOne() {
        var title = $("#updateTOne").val();
        var url = $("#updateljOne").val();
        $.ajax({
            url: "Thepublicjb.aspx/updateOne",
            type: "post",                //请求发送方式
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{id:'" + $("#weChatID").val() + "',name:'" + title + "',url:'" + url + "'}",
            //请求成功执行
            success: function (data, textStatus) {
                alert(data.d);
                window.location.reload(true);
                return;

            }
        });
    }

    function updateTwo() {
        
        $.ajax({
            url: "Thepublicjb.aspx/updateTwo",
            type: "post",                //请求发送方式
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{id:'" + $("#weChatID").val() + "',name:'" + $("#updateTTwo").val() + "',url:'" + $("#updateljTwo").val() + "'}",
            //请求成功执行
            success: function (data, textStatus) {
                alert(data.d);
                window.location.reload(true);
                return;

            }
        });
    }

    function twoGrade() {
        $.ajax({
            url: "Thepublicjb.aspx/twoGrade",
            type: "post",                //请求发送方式
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{id:'" + $("#weChatID").val() + "',name:'" + $("#TitleTwo").val() + "',url:'" + $("#ljTwo").val() + "',companyID:'" + $("#com_id").val() + "'}",
            //请求成功执行
            success: function (data, textStatus) {
                alert(data.d);
                window.location.reload(true);
                return;

            }
        });
    }

    function del(obj) {
        var thisHtml = $(obj);
        $.ajax({
            url: "Thepublicjb.aspx/del",
            type: "post",                //请求发送方式
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{id:'" + thisHtml.parent("dd").attr("id") + "'}",
            //请求成功执行
            success: function (data, textStatus) {
                alert(data.d);
                window.location.reload(true);
                return;

            }
        });
    }


    function delTwo(id) {
        $.ajax({
            url: "Thepublicjb.aspx/delTwo",
            type: "post",                //请求发送方式
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{id:'" + id + "'}",
            //请求成功执行
            success: function (data, textStatus) {
                alert(data.d);
                window.location.reload(true);
                return;

            }
        });
    }

    function addwechatNavs() {
        $(".zfpopup").find("h4").html("添加一级菜单");
        $(".zfpopup").find("li").eq(0).find("h5").html("标题");
        $(".zfpopup").find("li").eq(0).find("label").html("<input type='text' value='' id='TitleOne' placeholder='' />");
        $(".zfpopup").find("li").eq(1).find("h5").html("链接");
        $(".zfpopup").find("li").eq(1).find("label").html("<input type='text' value='' id='ljOne' placeholder='' />");
        $(".popupbj").fadeIn();
        $(".zfpopup").addClass("zfpopup_on");
        $(".popupqdbtn").attr({ "onclick": "oneGrade()" });
    }

    function aNav(obj) {
        var thisNav = $(obj);
        var thisHtml = thisNav.parent("p").parent(".wechatfirst").find("span").html();
        var thisUrl = thisNav.parent("p").find(".hiddenUrl").val();
        var thisID = thisNav.parent("p").parent(".wechatfirst").find("span").attr("id");
        $("#weChatID").val(thisID);
        $(".zfpopup").find("h4").html("编辑一级菜单");
        $(".zfpopup").find("li").eq(0).find("h5").html("标题");
        $(".zfpopup").find("li").eq(0).find("label").html("<input type='text' value='" + thisHtml + "' id='updateTOne' placeholder='' />");
        $(".zfpopup").find("li").eq(1).find("h5").html("链接");
        $(".zfpopup").find("li").eq(1).find("label").html("<input type='text' value='" + thisUrl + "' id='updateljOne' placeholder='' />");
        $(".popupbj").fadeIn();
        $(".zfpopup").addClass("zfpopup_on");
        $(".popupqdbtn").attr({ "onclick": "updateOne()" });
    }
    function bNav(obj) {
        var thisNav = $(obj);
        var thisID = thisNav.parent("p").parent("li").parent(".erNavlist").parent(".wechatnavmain").find(".wechatfirst").find("span").attr("id");
        var thisHtml = thisNav.parent().parent("li").find("span").html();
        var thispUrl = thisNav.parent().find("#hiddenerUrl").val();
        var TwothisID = thisNav.parent("p").parent("li").find("span").attr("id");
        $(".zfpopup").find("h4").html("编辑二级菜单");
        $(".zfpopup").find("li").eq(0).find("h5").html("标题");
        $(".zfpopup").find("li").eq(0).find("label").html("<input type='text' value='' id='updateTTwo' placeholder='' />");
        $(".zfpopup").find("li").eq(1).find("h5").html("链接");
        $(".zfpopup").find("li").eq(1).find("label").html("<input type='text' value='' id='updateljTwo' placeholder='' />");
        $("#updateTTwo").val(thisHtml);
        $("#updateljTwo").val(thispUrl);
        $("#weChatID").val(TwothisID);
        $(".popupbj").fadeIn();
        $(".zfpopup").addClass("zfpopup_on");
        $(".popupqdbtn").attr({ "onclick": "updateTwo()" });
    }

    function qxClick() {
        $(".zfpopup").removeClass("zfpopup_on");
        setTimeout(function () { $(".popupbj").hide(); }, 300);
    }

    $(function () {
        
        $(".wechatnavlist").find("dd").click(function () {
            $(".wechatnavlist").find("dd").removeClass("ddcolor");
            $(this).addClass("ddcolor");
            var ddNum = $(this).index();
            $(".wechatnavmain").show().eq(ddNum).siblings(".wechatnavmain").hide();
        });

        $(".addwechatnav").click(function () {
            var thisID = $(this).parent(".wechatnavmain").find(".wechatfirst").find("span").attr("id");
            $("#weChatID").val(thisID);
            $(".zfpopup").find("h4").html("添加二级菜单");
            $(".zfpopup").find("li").eq(0).find("h5").html("标题");
            $(".zfpopup").find("li").eq(0).find("label").html("<input type='text' value='' id='TitleTwo' placeholder='' />");
            $(".zfpopup").find("li").eq(1).find("h5").html("链接");
            $(".zfpopup").find("li").eq(1).find("label").html("<input type='text' value='' id='ljTwo' placeholder='' />");
            $(".popupbj").fadeIn();
            $(".zfpopup").addClass("zfpopup_on");
            $(".popupqdbtn").attr({ "onclick": "twoGrade()" });
            //var addList = '<li><span>商城首页</span><p><a onclick="bNav()">编辑</a><a onclick="b()">删除</a></p></li>';
            //$(this).parent(".wechatnavmain").find(".erNavlist").append(addList);
        });

        $(".wechatnavlist").find("dd").hover(function () {
            if ($(this).hasClass("ddcolor")) {
                $(this).find("i").hide();
            } else {
                $(this).find("i").toggle();
            }
        });

        $(".wechatnavlist").find("dd").find("i").click(function () {
            if (!$(this).parent().hasClass("ddcolor")) {
                var thisPNum = $(this).parent().index();
                $(this).parent().remove();
                $(".wechatnavmain").eq(thisPNum).remove();
            }
        });

    });
    
</script>
