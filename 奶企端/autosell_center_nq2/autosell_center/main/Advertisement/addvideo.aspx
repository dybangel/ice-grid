<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="addvideo.aspx.cs" Inherits="autosell_center.main.Advertisement.addvideo"  ResponseEncoding="gb2312"%>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>视频管理-自动售卖终端中心控制系统</title>
    <link rel="shortcut icon" href="/main/public/images/favicon.ico" />
    <link  href="../public/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css"/>
    <link href="../public/css/common.css" rel="stylesheet" type="text/css"/>
    <link href="../public/css/style.css" rel="stylesheet" type="text/css"/>
    <script src="../public/script/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="../public/script/Titlelist.js" type="text/javascript"></script>
    <link href="../public/css/jquery-ui-1.8.16.custom.css"  rel="stylesheet" type="text/css"/>
    <script src="../public/script/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
    <script src="fileupload/jquery-1.4.4.min.js" type="text/javascript" language="JavaScript"></script>
    <script src="fileupload/jquery.uploadify.js" type="text/javascript" charset="utf-8" language="JavaScript"></script>
    <link href="fileupload/uploadify.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(function () {
            $("#file_upload").uploadify({
                'auto': false,                      //是否自动上传
                'swf': 'fileupload/uploadify.swf',      //上传swf控件,不可更改
                'uploader': 'Handler.ashx',            //上传处理页面,可以aspx
                'fileTypeDesc': '请上传视频文件',
                'fileTypeExts': '*.mp4;*.flv',   //文件类型限制,默认不受限制
                'buttonText': '浏览文件',//按钮文字
                'width': 100,
                'height': 26,
                //最大文件数量'uploadLimit':
                'multi': false,//单选            
                'fileSizeLimit': '30MB',
                'queueSizeLimit': 1,  //队列限制
                'removeCompleted': false,
                'method': 'get',
                'formData': { 'type': $("#videoType").val() }
               
            });
    });
    </script>
    <style>
        .uploadify{
            left:0;
            top:6px;
            float:left;
        }
        .swfupload{
            left:0;
        }
        .uploadify-button-text{
            position: absolute;
            top: 0;
            width: 100%;
            left: 0;
        }
        .uploadify-queue{
            width:68%;
            position:relative;
            top:-12px;
            float:left;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
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
                             <select id="videoType" onchange="a()">
                              <option value ="0">横屏视频</option>
                              <option value ="1">竖屏视频</option>
                              
                            </select>
                        </li>
                        <li>
                            <label>视频简介</label>
                            <input type="text" value="" placeholder="请输入视频简介" id="description" />
                        </li>
                        <li>
                            <label>视频(mp4;flv)</label>
                            <input type="file" name="file_upload" id="file_upload" />
                            <a href="javascript:$('#file_upload').uploadify('cancel')" >删除</a>
                        </li>
                       
                    </ul>
                     <a   onclick="upload()" class="firmbtn" style="margin-left: 30px;padding: 8px 40px;">上传</a>
                </section>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
<script>
    $(function() {
        $("#picb").click(function() {
            $("#upload2").click(); //隐藏了input:file样式后，点击头像就可以本地上传
            $("#upload2").on("change", function() {
                var objUrl = getObjectURL(this.files[0]); //获取图片的路径，该路径不是图片在本地的路径
                if (objUrl) {
                    $("#pic2").attr("src", objUrl); //将图片路径存入src中，显示出图片
                }
            });
        });
        //建立一個可存取到該file的url
        function getObjectURL(file) {
            var url = null;
            if (window.createObjectURL != undefined) { // basic
                url = window.createObjectURL(file);
            } else if (window.URL != undefined) { // mozilla(firefox)
                url = window.URL.createObjectURL(file);
            } else if (window.webkitURL != undefined) { // webkit or chrome
                url = window.webkitURL.createObjectURL(file);
            }
            return url;
        }
    });
    function a()
    {
        $("#file_upload").uploadify({
            'auto': false,                      //是否自动上传
            'swf': '/fileupload/uploadify.swf',      //上传swf控件,不可更改
            'uploader': '/Handler.ashx',            //上传处理页面,可以aspx
            'fileTypeDesc': '请上传视频文件',
            'fileTypeExts': '*.mp4;*.flv',   //文件类型限制,默认不受限制
            'buttonText': '浏览文件',//按钮文字
            'width': 100,
            'height': 26,
            'multi': false,//单选            
            'fileSizeLimit': '30MB',
            'queueSizeLimit': 1,  //队列限制
            'removeCompleted': false,
            'method': 'get',
            'formData': { 'type': $("#videoType").val() }
        });
    }
    function upload()
    {
        if ($("#description").val().trim()=="")
        {
            alert("请填写视频简介");
            return;
        }
        $('#file_upload').uploadify('upload', '*');  
    }
</script>
<script>
    $(function() {
        $("#li7").find("a").addClass("aborder");
    })
</script>