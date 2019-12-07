<%@ Page Language="C#" ValidateRequest="false" AutoEventWireup="true" CodeBehind="memberdjcontent.aspx.cs" Inherits="autosell_center.main.member.memberdjcontent" %>

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
    <link  href="../../scripts/skin/jedate.css" rel="stylesheet" type="text/css"/>
    <script src="../../scripts/jedate.js" type="text/javascript"></script>
    <script src="../../scripts/jedate.min.js"></script>
    <link href="../public/star/star.css" rel="stylesheet" />
    <link rel="stylesheet" href="/main/public/kindeditor/themes/default/default.css" />
    <link rel="stylesheet" href="/main/public/kindeditor/plugins/code/prettify.css" />
        <%--<script src="../public/fileupload/jquery.uploadify.js"></script>--%>
    <script src="/main/public/kindeditor/kindeditor.js" type="text/javascript"></script>
    <script src="/main/public/kindeditor/plugins/code/prettify.js" type="text/javascript"></script>
    <script src="/main/public/kindeditor/lang/zh_CN.js"></script>
    
</head>
<body>
    <form id="form1" runat="server">
        <div class="header"></div>
        <div class="main">
            <div class="main_list">
                <div class="common_title">
                    <h4>
                        <i class="fa fa-plus"></i>
                        <span>会员管理</span>
                    </h4>
                    
                </div>
                <div class="common_main">
                    <div class="navlist">
                        <dl>
                            <dt>会员管理<em class="fa fa-cog"></em></dt>
                           <dd>
                                <a class="change " href="memberlist.aspx" ><i class="change  fa fa-user"></i>会员列表</a>
                            </dd>
                          
                            <dd>
                                <a class="change " href="rechargelist.aspx"><i class="change  fa fa-file-text"></i>会员充值记录</a>
                            </dd>
                            <dd>
                                <a class="change " href="rechargetj.aspx"><i class="change  fa fa-file-text"></i>收入统计</a>
                            </dd>
                            <dd>
                                <a class="change acolor" href="memberdj.aspx"><i class="change icolor fa fa-file-text"></i>会员等级管理</a>
                            </dd>
                            <dd>
                                <a class="change " href="tqlist.aspx"><i class="change  fa fa-file-text"></i>特权管理</a>
                            </dd>
                             <dd>
                                <a class="change " href="memberdjcontent.aspx"><i class="change  fa fa-file-text"></i>会员等级说明</a>
                            </dd>
                        </dl>
                    </div>
                    <section class="jiqlist">
                       
                        <div class="addnaiq">
                            <ul class="addnaiqdata" id="addnaiqdata1" style="display: block;">
                               
                                <li>
                                    
                                    <div class="div1">
                                        <asp:TextBox ID="nContent" name="nContent" TextMode="MultiLine" runat="server"></asp:TextBox>
                                    </div>
                                </li>

                                <li>
                                    <asp:Button ID="Button1" class="firmbtn" runat="server" Text="提交" OnClick="Button1_Click" />
                                </li>
                            </ul>
                        </div>
                    </section>
                </div>
            </div>
        </div>
       <input id="companyID" runat="server" type="hidden"/>
         <input id="_operaID" runat="server" type="hidden" />
         <input id="contentID" runat="server" type="button"/>
    </form>
</body>
</html>
<script>
  
   
    $(function () {
        $("#li1").find("a").addClass("aborder");
        $(".readclass").click(function () {
            $(this).parent().find("dl").toggle();
        });
        $(".readdl").find("dd").click(function () {
            var $readClass = $(".readclass");
            var $raadDl = $(this).html();
            $readClass.val($raadDl);
            $(".readclass").parent().find("dl").hide();
        });
        
        
    })
    //editorsave();
    function editorsave() {
        //保存
        $.ajax({
            type: "post",
            url: "memberdjcontent.aspx/editorsave",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{companyID:'" + $("#companyID").val() + "',type:'1'}",
            success: function (data) {

                if (data.d == "1") {
                    alert("保存成功");
                } else if (data.d == "2") {
                    alert("保存失败");

                }
                search();




            }
        })
    };
    //function search() {
    //    //保存
    //    $.ajax({
    //        type: "post",
    //        url: "memberdjcontent.aspx/search",
    //        contentType: "application/json; charset=utf-8",
    //        dataType: "json",
    //        data: "{companyID:'" + $("#companyID").val() + "',type:'1'}",
    //        success: function (data) {
               
    //            alert($("#id").val());
                
              
              
               
    //        }
    //    })
    //};
    //富文本编辑器初始化
    KindEditor.ready(function (K) {
        var editor = K.create('#nContent', {
            //上传管理
            uploadJson: '../public/kindeditor/asp.net/upload_json.ashx',
            //文件管理
            fileManagerJson: '../public/kindeditor/asp.net/file_manager_json.ashx',
            allowFileManager: true,
            //设置编辑器创建后执行的回调函数
            afterCreate: function () {
                var self = this;
                K.ctrl(document, 13, function () {
                    self.sync();
                    K('form[name=example]')[0].submit();
                });
                K.ctrl(self.edit.doc, 13, function () {
                    self.sync();
                    K('form[name=example]')[0].submit();
                });
            },
            //上传文件后执行的回调函数,获取上传图片的路径
            afterUpload: function (url) {
                // alert(url);
            },
            //编辑器高度
            width: '100%',
            //编辑器宽度
            height: '450px;',
            //配置编辑器的工具栏
            items: [
                'source', '|', 'undo', 'redo', '|', 'preview', 'template', 'cut', 'copy', 'paste',
                'plainpaste', 'wordpaste', '|', 'justifyleft', 'justifycenter', 'justifyright',
                'justifyfull', 'insertorderedlist', 'insertunorderedlist', 'indent', 'outdent', 'subscript',
                'superscript', 'clearhtml', 'quickformat', 'selectall', '|', 'fullscreen',
                'formatblock', 'fontname', 'fontsize', '|', 'forecolor', 'hilitecolor', 'bold',
                'italic', 'underline', 'strikethrough', 'lineheight', 'removeformat', '|', 'image', 'multiimage',
                'media', 'insertfile', 'table', 'hr', 'emoticons', 'anchor', 'link', 'unlink'
            ],
            cssData: 'body {font-family: "微软雅黑"; font-size: 16px}',
            pasteType: 1
        });
        prettyPrint();
    });

  
</script>
