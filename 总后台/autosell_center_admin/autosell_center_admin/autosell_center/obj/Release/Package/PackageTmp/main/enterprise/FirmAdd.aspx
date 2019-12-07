<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FirmAdd.aspx.cs" Inherits="autosell_center.main.enterprise.FirmAdd" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>添加设备-自动售卖终端中心控制系统</title>
    <link rel="shortcut icon" href="/main/public/images/favicon.ico" />
    <link href="/main/public/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/common.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/style.css" rel="stylesheet" type="text/css" />
    <script src="../public/script/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="../public/script/Titlelist.js" type="text/javascript"></script>
     <script src="../public/script/jquery.form.js" type="text/javascript"></script>
    <style>
        .drop{
            width: 60%;
            min-width: 400px;
            max-width: 800px;
            padding-left: 2%;
            height: 36px;
            border-radius: 6px;
            border: 1px solid #ddd
        }
    </style>
</head>
   
<body>
   <form id="ImageForm" method="post" enctype="multipart/form-data" runat="server">
        <div class="header"></div>
        <div class="main">
            <div class="main_list">
                <div class="common_title">
                    <h4>
                        <i class="fa fa-home"></i>
                        <span>选择企业客户进行操作</span>
                    </h4>
                </div>
                <div class="common_main">
                    <div class="navlist">
                        <dl>
                            <dt>奶企管理<em class="fa fa-cog"></em></dt>
                            <dd>
                                <a class="change" href="#" onclick="qx_judge('nqlb')"><i class="change fa fa-check-square"></i>奶企列表</a>
                            </dd>
                            <dd>
                                <a class="change acolor" href="#" onclick="qx_judge('xznq')"><i class="change icolor fa fa-plus-square"></i>新增奶企</a>
                            </dd>
                        </dl>
                    </div>
                    <section class="jiqlist">
                        <div class="addnaiq">
                            <h4 class="commonfut"><a class="change" href="#"  onclick="qx_judge('xznq')"><i class="change fa fa-angle-left"></i>奶企列表</a>/新增奶企</h4>
                            <dl>
                                <dd class="ddcolor">1.添加奶企基本信息</dd>
                                <dd>2.上传相关文件图片</dd>
                                <dd>3.添加完成</dd>
                            </dl>
                            <ul class="addnaiqdata" id="addnaiqdata1" style="display: block;">
                                  <li>
                                    <label>企业登录编号</label>
                                    <div>
                                        <input type="text" value="" placeholder="" id="bh"/>
                                        <p>企业登录编号</p>
                                    </div>
                                </li>
                                  <li>
                                    <label>企业登录密码</label>
                                    <div>
                                        <input type="text" value="" placeholder="" id="pwd"/>
                                        <p>企业登录密码</p>
                                    </div>
                                </li>
                                 <li>
                                    <label>企业简码</label>
                                    <div>
                                        <input type="text" value="" placeholder="" id="code"/>
                                        <p>企业简码使用英文大写字母</p>
                                    </div>
                                </li>
                                <li>
                                    <label>企业名称</label>
                                    <div>
                                        <input type="text" value="" placeholder="" id="companyName"/>
                                        <p>请填写企业全称</p>
                                    </div>
                                </li>
                                <li>
                                    <label>负责人</label>
                                    <div>
                                        <input type="text" value="" placeholder=""  id="fzr"/>
                                        <p>企业负责人姓名</p>
                                    </div>
                                </li>
                                <li>
                                    <label>财务联系人</label>
                                    <div>
                                        <input type="text" value="" placeholder="" id="cwr"/>
                                        <p>企业财务联系人姓名</p>
                                    </div>
                                </li>
                                <li>
                                    <label>联系方式</label>
                                    <div>
                                        <input type="text" value="" placeholder=""  id="phone"/>
                                        <p>请填写企业联系人电话，用于接收短信通知</p>
                                    </div>
                                </li>
                                <li>
                                    <label>业务员</label>
                                    <div>
                                         
                                        <asp:DropDownList ID="business" runat="server" CssClass="drop"></asp:DropDownList>
                                        <p>负责的业务员</p>
                                    </div>
                                </li>
                                <li>
                                    <input class="firmbtn" type="button" value="下一步" onclick="nextTow()" />
                                </li>
                            </ul>
                            <ul class="addnaiqdata" id="addnaiqdata2">
                                <li>
                                    <label>企业营业执照(.jgp)</label>
                                    <div>
                                        <img id="pic" src="/main/public/images/addimg.png" alt="" />
                                        <input type="file" id="upload" name="file" value="" style="display: none;" />
                                    </div>
                                </li>
                                <li>
                                    <label>企业logo(.jpg)</label>
                                    <div>
                                        <img id="piclogo" src="/main/public/images/addimg.png" alt="" />
                                        <input type="file" id="uploadlogo" name="file" value="" style="display: none;" />
                                    </div>
                                </li>
                                <li>
                                    <input class="firmbtn" type="button" value="上一步" onclick="preOne()" />
                                    <input class="firmbtn" type="button" value="下一步" onclick="nextthree()" />
                                </li>
                            </ul>
                            <div class="addnaiqdata" id="addnaiqdata3">
                                <div class="firmok">
                                    <img src="/main/public/images/firmok.png" alt="" />
                                    <div>
                                        <p>奶企名称：<span id="_name"></span></p>
                                        <p>登录编号：<span id="_bh"></span></p>
                                        <p>企业负责人：<span id="_fzr"></span></p>
                                    </div>
                                    <input type="button" value="返回企业列表" class="backlist" onclick="javascript: location.href = 'SellCenter.aspx';" />
                                    <input type="button" value="继续添加" onclick="javascript: location.href = 'FirmAdd.aspx';" />
                                </div>
                            </div>
                        </div>
                    </section>
                </div>
            </div>
        </div>
         
    </form>
</body>
</html>
<script>
    function check() {
        var index = document.getElementById("code").value;
        if (/^[A-Z]+$/.test(index))  //a-z
        {
            return true;
        }
        else {
            alert("请输入大写英文字母");
            return false;
        }
    }
    $(function () {
        $("#pic").click(function () {
            $("#upload").click(); //隐藏了input:file样式后，点击头像就可以本地上传
            $("#upload").on("change", function () {
                var objUrl = getObjectURL(this.files[0]); //获取图片的路径，该路径不是图片在本地的路径
                if (objUrl) {
                    $("#pic").attr("src", objUrl); //将图片路径存入src中，显示出图片
                }
            });
        });
        $("#piclogo").click(function () {
            $("#uploadlogo").click(); //隐藏了input:file样式后，点击头像就可以本地上传
            $("#uploadlogo").on("change", function () {
                var objUrl = getObjectURL(this.files[0]); //获取图片的路径，该路径不是图片在本地的路径
                if (objUrl) {
                    $("#piclogo").attr("src", objUrl); //将图片路径存入src中，显示出图片
                }
            });
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
</script>
<script>
    function nextTow() {
      
        //验证填写信息
        if ($("#bh").val().trim() == "") {
            alert("企业编号不能为空");
            return false;
        }
        if ($("#pwd").val().trim() == "") {
            alert("企业密码不能为空");
            return false;
        }
        if ($("#companyName").val().trim()=="")
        {
            alert("企业名称不能为空");
            return false;
        }
        if ($("#fzr").val().trim() == "") {
            alert("负责人不能为空");
            return false;
        }
        if ($("#cwr").val().trim() == "") {
            alert("财务联系人不能为空");
            return false;
        }
        if ($("#phone").val().trim() == "") {
            alert("联系方式不能为空");
            return false;
        }
        if ($("#business").val().trim() == "") {
            alert("业务员不能为空");
            return false;
        }
        var index = document.getElementById("code").value;
        if (/^[A-Z]+$/.test(index))  //a-z
        {
            
        }
        else {
            alert("企业简码请输入大写英文字母");
            return false;
        }
        $("#addnaiqdata1").hide();
        $("#addnaiqdata2").show();
        $(".addnaiq").find("dd").removeClass("ddcolor");
        $(".addnaiq").find("dd").eq(1).addClass("ddcolor");
    }

    function preOne() {
        $("#addnaiqdata1").show();
        $("#addnaiqdata2").hide();
        $(".addnaiq").find("dd").removeClass("ddcolor");
        $(".addnaiq").find("dd").eq(0).addClass("ddcolor");
    }

    function nextthree() {
        //验证是否上传营业执照和logo
        var str = $("#upload").val();
        var arr = str.split('\\');//注split可以用字符或字符串分割 
        var yyzz = arr[arr.length - 1].split('.')[1];//这就是要取得的图片名称 
        if(yyzz!="jpg")
        {
            alert("营业执照请上传jpg格式的字符串");
            return false;
        }
        var str = $("#uploadlogo").val();
        var arr = str.split('\\');//注split可以用字符或字符串分割 
        var logo = arr[arr.length - 1].split('.')[1];//这就是要取得的图片名称 
        if (logo != "jpg") {
            alert("企业logo请上传jpg格式的字符串");
            return false;
        }
        if (check())//验证简码是否是大写字母
        {
            upload();
        }
        
        $("#addnaiqdata3").show();
        $("#addnaiqdata2,#addnaiqdata1").hide();
        $(".addnaiq").find("dd").removeClass("ddcolor");
        $(".addnaiq").find("dd").eq(2).addClass("ddcolor");
    }
    function addCompany() {
        
        $.ajax({
            url: "../../ashx/asm.ashx",
            type: 'post',
            dataType: 'json',
            timeout: 10000,
            data: {
                action: "addCompany",
                bh: $("#bh").val(),
                pwd:$("#pwd").val(),
                comName: $("#companyName").val(),
                fzr: $("#fzr").val(),
                cwr: $("#cwr").val(),
                phone: $("#phone").val(),
                business: $("#business").val()
            },
            success: function (resultData) {
                if (resultData.d == "1") {
                    alert("保存成功！");
                } else if (resultData.d == "2") {
                    alert("保存失败！");
                } else if (resultData.d == "3") {
                    alert("企业名称重复");
                } else if (resultData.d == "4") {
                    alert("企业简称重复");
                }
            }
        })
    }
    
    function upload() {
        var u = "";
        var options = {
            url: "../../ashx/asm.ashx?action=yyzzUploads",//处理程序路径  
            type: "post",
            data: {
                comName: $("#companyName").val(),
                fzr: $("#fzr").val(),
                cwr: $("#cwr").val(),
                phone: $("#phone").val(),
                business: $("#business").val(),
                bh: $("#bh").val(),
                pwd: $("#pwd").val(),
                code:$("#code").val()
            },
            success: function (msg) {//回调函数--请求成功  
                if (msg.toString().substring(0, 3) == "ERR") {//  
                    alert(msg.substring(3, msg.length));
                }
                else {
                    $("#pic").attr("src", msg);//回显图片  
                }
                $("#_name").text($("#companyName").val())
                $("#_bh").text($("#bh").val());
                $("#_fzr").text($("#fzr").val());
            },
            error: function (err) {
               
            }
        };
        //将options传给ajaxForm  
        $('#ImageForm').ajaxSubmit(options);
    }
    function qx_judge(menuID) {
        //首先验证账号和密码正确
        $.ajax({
            url: "../../../ashx/asm.ashx",
            type: 'post',
            dataType: 'json',
            timeout: 10000,
            data: {
                action: "qx_judge",
                menu: menuID
            },
            success: function (resultData) {
                if (resultData.result == "ok")//允许查看跳转
                {
                    if (menuID == 'nqlb') {//会员列表
                        location.href = "SellCenter.aspx";
                    }
                    if (menuID == 'xznq') {//新增奶企
                        location.href = "FirmAdd.aspx";
                    }
                   

                } else if (resultData.result == "notLogin")//没有查看权限
                {
                    $(".jiqlist").empty();
                    $(".jiqlist").html("<p class='noquanxian'>当前没有权限</p>").css({ "background": "#ddd" });
                }
                else if (resultData.result == "1")//请联系管理员给当前登录角色赋值权限
                {
                    alert("请联系管理员给当前登录角色赋值权限");
                }
                else if (resultData.result == "2")//跳转重新登录
                {
                    location.href = "../../../../index.aspx";
                }

            }
        })
    }
</script>
<script>
    $(function () {
        $("#li0").find("a").addClass("aborder");
    })
</script>
