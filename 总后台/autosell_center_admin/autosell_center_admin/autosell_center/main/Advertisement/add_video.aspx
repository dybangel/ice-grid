<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="add_video.aspx.cs" Inherits="autosell_center.main.Advertisement.add_video" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>视频管理-自动售卖终端中心控制系统</title>
    <link rel="shortcut icon" href="/main/public/images/favicon.ico" />
    <link href="../public/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../public/css/common.css" rel="stylesheet" type="text/css" />
    <link href="../public/css/style.css" rel="stylesheet" type="text/css" />
    <script src="../public/script/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="../public/script/Titlelist.js" type="text/javascript"></script>
    <link href="../public/css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <script src="../public/script/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
    <script src="fileupload/jquery-1.4.4.min.js" type="text/javascript" language="JavaScript"></script>
    <script src="fileupload/jquery.uploadify.js" type="text/javascript" charset="utf-8" language="JavaScript"></script>
    <link href="fileupload/uploadify.css" rel="stylesheet" type="text/css" />
    <style>
        .uploadify {
            left: 0;
            top: 6px;
            float: left;
        }

        .swfupload {
            left: 0;
        }

        .uploadify-button-text {
            position: absolute;
            top: 0;
            width: 100%;
            left: 0;
        }

        .uploadify-queue {
            width: 68%;
            position: relative;
            top: -12px;
            float: left;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="addvideopopup">
            <dl>
                <dt>
                    <img src="../public/images/loading.gif" />
                </dt>
                <dd>正在上传，请勿进行其他操作!</dd>
            </dl>
        </div>
        <div class="header"></div>
        <div class="main">
            <div class="addDiv change">
                <h4>查看视频</h4>
                <video src="/main/public/video/movie.mp4" controls="controls"></video>
            </div>
            <div class="popupbj" onclick="divOff()"></div>
            <div class="main_list">
                <div class="common_title">
                    <h4>
                        <i class="fa fa-plus"></i>
                        <span>广告管理</span>
                    </h4>
                </div>
                <div class="common_main">
                    <div class="navlist">
                        <dl>
                            <dt>广告管理<em class="fa fa-cog"></em></dt>
                            <dd>
                                <a class="change acolor" href="video.aspx"><i class="change icolor fa fa-video-camera"></i>视频管理</a>
                            </dd>
                            <dd>
                                <a class="change" href="Jurisdiction.aspx"><i class="change fa fa-hdd-o"></i>机器添加视频</a>
                            </dd>
                        </dl>
                    </div>
                    <section class="jiqlist">
                        <dl class="jiqlistTab">
                            <dd class="change ddcolor"><b>添加视频</b></dd>
                        </dl>
                        <ul class="thepublic">
                            <li>
                                <dl>
                                    <dd>视频设置</dd>
                                </dl>
                            </li>
                            <li>
                                <label>视频类型</label>
                                <asp:DropDownList ID="videoType" runat="server">
                                    <asp:ListItem Value="0">横屏</asp:ListItem>
                                    <asp:ListItem Value="1">竖屏</asp:ListItem>
                                </asp:DropDownList>
                            </li>
                            <li>
                                <label>视频简介</label>
                                <input type="text" value="" placeholder="请输入视频简介" id="description"  runat="server"/>
                            </li>
                            <li>
                                <label>视频(mp4;flv)</label>
                                    <asp:FileUpload ID="FileUpload1" runat="server" />
                                <a href="javascript:$('#file_upload').uploadify('cancel')">删除</a>
                            </li>
                        </ul>
                        <asp:Button ID="Button1" runat="server" Text="上传" OnClick="Button1_Click" CssClass="firmbtn" Style="margin-left: 30px; padding: 8px 40px;" OnClientClick="return aa()" />
                    </section>
                </div>
            </div>
        </div>
       <%-- <input  id="company_id" runat="server" type="hidden"/>--%>
        <input  id="video_size" runat="server" type="hidden"/>
        <input id="vv" runat="server" type="hidden" value="1" />
    </form>
</body>
</html>
<script>
    $(function () {
        $("#li7").find("a").addClass("aborder");
         
    });
    function aa() {
        getFileSize("FileUpload1");
        if (size < 30) {
            $("#video_size").val(size);
            $(".addvideopopup").show();
        } else {
            alert("上传视频不得大于30MB!");
            return false;
        }
    }
    var size = 0;
    function getFileSize(eleId) {
        try {
            if ($.browser.msie) {//ie旧版浏览器
                var fileMgr = new ActiveXObject("Scripting.FileSystemObject");
                var filePath = $('#' + eleId)[0].value;
                var fileObj = fileMgr.getFile(filePath);
                size = fileObj.size; //byte
                size = size / 1024;//kb
                //size = size / 1024;//mb
            } else {//其它浏览器
                size = $('#' + eleId)[0].files[0].size;//byte
                size = size / 1024;//kb
                size = size / 1024;//mb
            }
        } catch (e) {
            alert("错误：" + e);
        }
    }

</script>
