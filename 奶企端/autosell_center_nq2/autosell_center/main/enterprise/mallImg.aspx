<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="mallImg.aspx.cs" Inherits="autosell_center.main.enterprise.mallImg" %>

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
     <script src="../public/script/jquery.form.js" type="text/javascript"></script>
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
                            <dt>广告管理<em class="fa fa-cog"></em></dt>
                            <dd>
                                <a class="change " href="#" onclick="qx_judge('spgl')"><i class="change  fa fa-video-camera"></i>视频管理</a>
                            </dd>

                            <dd>
                                <a class="change" href="#" onclick="qx_judge('jqtjsp')"><i class="change fa fa-hdd-o"></i>机器添加视频</a>
                            </dd>

                        </dl>
                       <%-- <dl>
                            <dt>商城轮播图<em class="fa fa-cog"></em></dt>
                            <dd>
                                <a class="change acolor" href="#" onclick="qx_judge('sclbt')"><i class="change icolor fa fa-bars"></i>商城轮播图</a>
                            </dd>
                        </dl>--%>

                        <dl>
                            <dt>小程序广告图<em class="fa fa-cog"></em></dt>
                            <dd>
                                <a class="change" href="#" onclick="qx_judge('hblist')"><i class=" fa fa-bars"></i>小程序广告图</a>
                            </dd>
                        </dl>
                    </div>
                
                <section class="jiqlist">
                    <dl class="jiqlistTab">
                        <dd class="change ddcolor"><b>商城轮播图设置</b></dd>
                    </dl>
                    <h4 class="fileImgText">请上传<b>960px * 500px</b>尺寸的轮播图，并确保所有上传的图片尺寸一致!</h4>
                    
                    <div class="bannerList">
                        <ul>
                            <form id="ImageForm1" method="post" enctype="multipart/form-data">
                                 <li>
                                
                                <div class="bannerList-Img">
                                    <img id="pic1" src="/main/public/images/addimg.png" alt="" />
                                   <input type="file" id="upload1" name="file" value="" style="display: none;" />
                                </div>
                                <p>
                                    
                                    <a class="updateImg" href="javascript:updateImg1()">上传</a>
                                    <a class="" href="javascript:deleteImg1()">删除</a>
                                </p>
                            </li>
                            </form>
                           <form  id="ImageForm2" method="post" enctype="multipart/form-data">
                                <li>
                                <div class="bannerList-Img">
                                    <img id="pic2" src="/main/public/images/addimg.png" alt="" />
                                   <input type="file" id="upload2" name="file" value="" style="display: none;" />
                                </div>
                                <p>
                                    <a class="updateImg" href="javascript:updateImg2()">上传</a>
                                    <a class="" href="javascript:deleteImg2()">删除</a>
                                </p>
                            </li>
                           </form>
                           <form id="ImageForm3" method="post" enctype="multipart/form-data">
                                <li>
                                <div class="bannerList-Img">
                                   <img id="pic3" src="/main/public/images/addimg.png" alt="" />
                                   <input type="file" id="upload3" name="file" value="" style="display: none;" />
                                </div>
                                <p>
                                    <a class="updateImg" href="javascript:updateImg3()">上传</a>
                                    <a class="" href="javascript:deleteImg3()">删除</a>
                                </p>
                            </li>
                                <input  id="companyID" runat="server" type="hidden"/>
                           </form>
                           
                        </ul>
                    </div>
                </section>
            </div>
        </div>
    </div>
</body>
</html>
<script>
    $(function () {
        getImage();
        $("#li8").find("a").addClass("aborder");
        $("#pic1").click(function () {
            $("#upload1").click(); //隐藏了input:file样式后，点击头像就可以本地上传
            $("#upload1").on("change", function () {
                var objUrl = getObjectURL(this.files[0]); //获取图片的路径，该路径不是图片在本地的路径
                if (objUrl) {
                    $("#pic1").attr("src", objUrl); //将图片路径存入src中，显示出图片
                }
            });
        });
        $("#pic2").click(function () {
            $("#upload2").click(); //隐藏了input:file样式后，点击头像就可以本地上传
            $("#upload2").on("change", function () {
                var objUrl = getObjectURL(this.files[0]); //获取图片的路径，该路径不是图片在本地的路径
                if (objUrl) {
                    $("#pic2").attr("src", objUrl); //将图片路径存入src中，显示出图片
                }
            });
        });
        $("#pic3").click(function () {
            $("#upload3").click(); //隐藏了input:file样式后，点击头像就可以本地上传
            $("#upload3").on("change", function () {
                var objUrl = getObjectURL(this.files[0]); //获取图片的路径，该路径不是图片在本地的路径
                if (objUrl) {
                    $("#pic3").attr("src", objUrl); //将图片路径存入src中，显示出图片
                }
            });
        });

    })
    function getImage()
    {
        $.ajax({
            type: "post",
            url: "mallImg.aspx/getImage",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{companyID:'" + $("#companyID").val() + "'}",
            success: function (data) {
                if (data.d.result == 200) {
                    if (data.d.img1 != "" && data.d.img1!=null)
                    {
                        $("#pic1").attr("src","../"+ data.d.img1);
                    }
                    if (data.d.img2 != "" && data.d.img2 != null) {
                        $("#pic2").attr("src", "../"+data.d.img2);
                    }
                    if (data.d.img3 != "" && data.d.img3 != null) {
                        $("#pic3").attr("src", "../"+data.d.img3);
                    }
                }
            }
        })
    }
    function updateImg1()
    {
        var u = "";
        var options = {
            url: "../../ashx/asm.ashx",//处理程序路径  
            type: "post",
            dataType: 'text',
            data: {
                action: "lbt1",
                comID: $("#companyID").val()
            },
            success: function (msg) {
                //回调函数--请求成功  
                if (msg.toString().substring(0, 3) == "ERR") {
                    alert(msg.substring(3, msg.length));
                }
                else {
                    $("#pic1").attr("src", msg);//回显图片  
                }
            },
            error: function (err) {
                alert("添加成功");
            }
        };
        //将options传给ajaxForm  
        $('#ImageForm1').ajaxSubmit(options);
    }
    function deleteImg1()
    {

    }
    function updateImg2() {
        var u = "";
        var options = {
            url: "../../ashx/asm.ashx",//处理程序路径  
            type: "post",
            dataType: 'text',
            data: {
                action: "lbt2",
                comID: $("#companyID").val()
            },
            success: function (msg) {
                //回调函数--请求成功  
                if (msg.toString().substring(0, 3) == "ERR") {
                    alert(msg.substring(3, msg.length));
                }
                else {
                    $("#pic2").attr("src", msg);//回显图片  
                }
            },
            error: function (err) {
                alert("添加成功");
            }
        };
        //将options传给ajaxForm  
        $('#ImageForm2').ajaxSubmit(options);
    }
    function deleteImg2() {

    }
    function updateImg3() {
        var u = "";
        var options = {
            url: "../../ashx/asm.ashx",//处理程序路径  
            type: "post",
            dataType: 'text',
            data: {
                action: "lbt3",
                comID: $("#companyID").val()
            },
            success: function (msg) {
                //回调函数--请求成功  
                if (msg.toString().substring(0, 3) == "ERR") {
                    alert(msg.substring(3, msg.length));
                }
                else {
                    $("#pic3").attr("src", msg);//回显图片  
                }
            },
            error: function (err) {
                alert("添加成功");
            }
        };
        //将options传给ajaxForm  
        $('#ImageForm3').ajaxSubmit(options);
    }
    function deleteImg3() {

    }

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
    function save() {
        $.ajax({
            type: "post",
            url: "syssetting.aspx/save",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{companyID:'" + $("#companyID").val() + "',getTime:'" + $("#getTime").val() + "'}",
            success: function (data) {
                
            }
        })
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
                    if (menuID == 'hylb') {//会员列表
                        location.href = "../member/memberlist.aspx";
                    }
                    if (menuID == 'jqddtj') {//订单管理
                       // location.href = "../order/orderform.aspx";
                    }
                    if (menuID == 'hyddgl') {//会员订单管理

                        location.href = "../order/order.aspx";
                    }
                    if (menuID == 'cpddtj') {//商品订单统计

                        location.href = "../order/Productform.aspx";
                    }
                    if (menuID == 'tjcp') {//添加商品

                        location.href = "../product/productadd.aspx";
                    }
                    if (menuID == 'gmjl') {//购买记录

                        location.href = "../equipment/orderlist.aspx";
                    }
                    if (menuID == 'cplb') {//商品列表
                        location.href = "../product/productlist.aspx";
                    }
                    if (menuID == 'sblb') {//设备管理
                        location.href = "../equipment/equipmentlist.aspx";
                    }
                    if (menuID == 'glygl') {//管理员管理
                        location.href = "../Administrators/adminlist.aspx";
                    }
                    if (menuID == 'jsgl') {//角色管理
                        location.href = "../Administrators/rolelist.aspx";
                    }
                    if (menuID == 'szhd') {//活动管理
                        location.href = "../activity/activity.aspx";
                    }
                    if (menuID == 'spgl') {//广告管理
                        location.href = "../Advertisement/video.aspx";
                    }
                    if (menuID == 'jqtjsp') {//机器添加视频
                        location.href = "../Advertisement/Jurisdiction.aspx";
                    }
                    if (menuID == 'gzhsz') {//公众号管理
                        location.href = "../enterprise/Thepublicjb.aspx";
                    }
                    if (menuID == 'mbxx') {//模板消息
                       
                        location.href = "../enterprise/Distributor.aspx";
                    }
                    if (menuID == 'sjdp') {//数据大屏
                        window.open("/main/Big_screen/big_screen.aspx");
                    }
                    if (menuID == 'zhcx') {//数据统计与查询
                        location.href = "../datatj/Statisticalquery.aspx";
                    }
                    if (menuID == 'cssz') {//数据统计与查询
                        location.href = "../enterprise/syssetting.aspx";
                    }
                    if (menuID == 'sclbt') {//商城轮播图
                        location.href = "../enterprise/mallImg.aspx";
                    }
                    if (menuID == 'hblist') {//支付宝红包
                        location.href = "../enterprise/hblist.aspx";
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

