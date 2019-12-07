<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Jurisdiction.aspx.cs" Inherits="autosell_center.main.Advertisement.Jurisdiction" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>权限设置-自动售卖终端中心控制系统</title>
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
                </div>
                <div class="common_main">
                     <div class="navlist">
                        <dl>
                            <dt>广告管理<em class="fa fa-cog"></em></dt>
                            <dd>
                                <a class="change " href="video.aspx"><i class="change  fa fa-video-camera"></i>视频管理</a>
                            </dd>
                  
                            <dd>
                                <a class="change acolor" ><i class="change icolor fa fa-hdd-o"></i>机器添加视频</a>
                            </dd>

                        </dl>
                           <dl>
                            <dt>小程序广告图<em class="fa fa-cog"></em></dt>
                            <dd>
                                <a class="change" href="hblist.aspx"><i class="change icolor fa fa-bars"></i>小程序广告图</a>
                            </dd>
                        </dl>
                    </div>
                    <section class="jiqlist">
                    
                        <ul class="jiqlistseach">
                           <li>
                               <asp:DropDownList  class="ipt" ID="companyList" runat="server" ></asp:DropDownList>
                           </li>
                            <li>
                                <input type="text" value="" placeholder="设备编号"  id="bh"/>
                            </li>
                            <li>
                                <input type="button" value="查询" class="seachbtn"  onclick="sear()"/>
                            </li>
                        </ul>
                        <ul class="jiqlisttable" style="display: block;" id="ull">
                            
                          
                        </ul>
                        
                    </section>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
<script>
    $(function () {
        $(".jiqlistTab").find("dd").click(function () {
            $(".jiqlistTab dd").removeClass("ddcolor");
            $(this).addClass("ddcolor");
            var $liNum = $(this).index();
            $(".jiqlisttable").hide();
            $(".jiqlisttable").eq($liNum).fadeIn();
        });
        sear();
    });
    function sear()
    {
        $.ajax({
            type: "post",
            url: "Jurisdiction.aspx/search",
            contentType: "application/json; charset=utf-8",
            data: "{gsid:'" + $("#companyList").val() + "',bh:'" + $("#bh").val() + "'}",
            dataType: "json",
            success: function (data) {
                $("#ull").empty();
                var serverdata = $.parseJSON(data.d);
                var serverdatalist = serverdata.length;
                for (var i = 0; i < serverdatalist; i++) {
                    $(" <li>"
                             +"   <dl>"
                             +"       <dd class='jiqname'>机器</dd>"
                             +"       <dd class='jiqtime'>奶企</dd>"
                             + "       <dd class='jiqzt'>状态</dd>"
                               + "     <dd class='jiqzt'>当前视频数</dd>"
                             +"       <dd class='jiqcz'>操作</dd>"
                             +"   </dl>"
                             +"   <label class='jiqname'>"
                             +"       <img src='/main/public/images/smjicon.png' alt='' />"
                             +"       <span>"+serverdata[i].bh+"</span>"
                             + "       <em>开启时间:<i>" + serverdata[i].regTime+ "</i></em>"
                             +"   </label>"
                             +"   <label class='jiqtime'>"+serverdata[i].name+"</label>"
                             + "   <label class='jiqzt'><a>" + serverdata[i].sta + "</a></label>"
                               + "   <label class='jiqzt'><a>"+serverdata[i].num+"</a></label>"
                             +"   <label class='jiqcz'>"
                             + "       <a href='adVideoToMechine.aspx?mechineID=" + serverdata[i].id + "'>添加视频</a>"
                             + "<a href='yvideolist.aspx?mechineID=" + serverdata[i].id + "'>设置</a>"
                             +"   </label>"
                             +"</li>").appendTo("#ull");
                }
            }
        });
    }
</script>
<script>
    $(function () {
        $("#li9").find("a").addClass("aborder");
    })
</script>
