<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="adminlist.aspx.cs" Inherits="autosell_center.main.Adminlist.adminlist" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>管理员管理-自动售卖终端中心控制系统</title>
    <link rel="shortcut icon" href="/main/public/images/favicon.ico" />
    <link href="/main/public/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/common.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/style.css" rel="stylesheet" type="text/css" />
    <script src="/main/public/script/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="/main/public/script/Titlelist.js" type="text/javascript"></script>
    <style>
        #adddd {
            position: absolute;
            right: 30px;
        }

        #addvideo {
            position: absolute;
            border-radius: 5px;
            background: #3a77d5;
            color: #fff;
            right: 0;
            height: 32px;
            top: 4px;
            width: 120px;
            line-height: 32px;
            font-size: 1rem;
        }

        .memberlist {
            margin-top: 30px;
        }

            .memberlist a {
                color: #3a77d5;
                margin-right: 6px;
            }

            .memberlist b {
                font-weight: normal;
                color: #3a77d5;
            }

        .productlist li span {
            overflow: hidden;
            text-overflow: ellipsis;
            white-space: nowrap;
        }

        .productlist li .pronaiq {
            width: 10%;
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
                        <span>管理员管理</span>
                    </h4>
                </div>
                <div class="common_main">
                    <div class="navlist">
                        <dl>
                            <dt>管理员管理<em class="fa fa-cog"></em></dt>
                            <dd>
                                <a class="change acolor"><i class="change icolor fa fa-user"></i>管理员管理</a>
                            </dd>

                            <dd>
                                <a class="change" href="rolelist.aspx"><i class="change fa fa-user-circle"></i>角色管理</a>
                            </dd>
                        </dl>
                    </div>
                    <div id="addCaDiv" class="addDiv change">
                            <h4>添加管理员</h4>
                            <ul>
                                <li>
                                    <label>管理员编号</label>
                                    <input type="text" value="" placeholder="填写管理员编号用于登录" id="addbh"/>
                                </li>
                                <li>
                                    <label>管理员名称</label>
                                    <input type="text" value="" placeholder="填写管理员名称" id="addname"/>
                                </li>
                                <li>
                                    <label>管理员电话</label>
                                    <input type="text" value="" placeholder="填写管理员电话" id="addphone"/>
                                </li>
                                <li>
                                    <label>选择角色</label>
                                     <asp:DropDownList ID="roleList" runat="server"></asp:DropDownList>
                                </li>
                                <li>
                                    <label>设置密码</label>
                                    <input type="password" value="" id="addPwd1"/>
                                </li>
                                <li>
                                    <label>确认密码</label>
                                    <input type="password" value="" id="addPwd2"/>
                                </li>
                                <li>
                                    <label></label>
                                    <input type="button" value="确认" onclick="add()" class="btnok"/>
                                    <input type="button" value="取消" class="btnoff" onclick="divOff()"/>
                                </li>
                            </ul>
                        </div>
                    <div id="updataCaDiv" class="addDiv change">
                            <h4>修改管理员</h4>
                            <ul>
                                 <li>
                                    <label>管理员编号</label>
                                    <input type="text" value="" id="updateBH" placeholder="填写管理员编号" readonly="true"/>
                                </li>
                                <li>
                                    <label>管理员名称</label>
                                    <input type="text" value="" id="updateName" placeholder="填写管理员名称"/>
                                </li>
                                <li>
                                    <label>管理员电话</label>
                                    <input type="text" value="" id="updatePhone" placeholder="填写管理员电话"/>
                                </li>
                                <li>
                                    <label>选择角色</label>
                                     <asp:DropDownList ID="DropDownList1" runat="server"></asp:DropDownList>
                                </li>
                                <li>
                                    <label>设置密码</label>
                                    <input type="password" id="updatePwd1" value="" />
                                </li>
                                <li>
                                    <label>确认密码</label>
                                    <input type="password" id="updatePwd2" value="" />
                                </li>
                                <li>
                                    <label></label>
                                    <input type="button" value="确认" onclick="update()" class="btnok"/>
                                    <input type="button" value="取消" class="btnoff" onclick="divOff()"/>
                                </li>
                            </ul>
                        </div>
                        <div class="popupbj"></div>
                    <section class="jiqlist">
                        <dl class="jiqlistTab">
                            <dd class="change ddcolor"><b>管理员列表</b></dd>
                            <dd id="adddd">
                                <a id="addvideo" onclick="addadmin()">添加管理员</a>
                            </dd>
                        </dl>
                        <ul class="jiqlistseach">
                            <li>
                                <input type="text" value="" placeholder="管理员名称" id="keywords1" />
                            </li>
                            <li>
                                <input type="text" value="" placeholder="电话" id="keywords2" />
                            </li>
                            <li>
                                <input type="button" value="查询" class="seachbtn" id="search" onclick="sear()" />
                            </li>
                        </ul>
                        <ul class="memberlist" id="ull">
                            <li>
                                <label style="width: 25%">管理名称</label>
                                <label style="width: 25%">电话</label>
                                <label style="width: 25%">角色</label>
                                <label style="width: 25%">操作</label>
                            </li>
                            <li>
                                <span class="adminNmae" style="width: 25%">管理员一</span>
                                <span class="adminPhone" style="width: 25%">13333333333</span>
                                <span class="adminQx" style="width: 25%"><b>维保</b></span>
                                <span style="width: 25%">
                                    <a onclick="adminUpd(this)">修改</a>
                                    <a onclick="adminDel()">删除</a>
                                </span>
                            </li>
                            <li>
                                <span class="adminNmae" style="width: 20%">管理员一</span>
                                <span class="adminPhone" style="width: 20%">13333333333</span>
                                <span class="adminQx" style="width: 20%"><b>维保</b></span>
                                <span class="adminQy" style="width: 20%">光明集团</span>
                                <span style="width: 20%">
                                    <a onclick="adminUpd(this)">修改</a>
                                    <a onclick="adminDel()">删除</a>
                                </span>
                            </li>
                        </ul>
                    </section>
                </div>
            </div>
        </div>
      <input id="operID" runat="server" type="hidden"/>
    </form>
</body>
</html>
<script>
    function addadmin() {
        $("#addCaDiv").addClass("addDivshow");
        setTimeout(function () {
            $(".popupbj").fadeIn();
        }, 100);
    }
    function adminUpd(id) {
        //var adminBH = $(obj).parent().parent().find(".adminBH");
        //var adminName = $(obj).parent().parent().find(".adminName");
        //var adminPhone = $(obj).parent().parent().find(".adminPhone");
        //var adminQx = $(obj).parent().parent().find(".adminQx");
        //$("#updateBH").val(adminBH.html());
        //$("#updateName").val(adminName.val());
        //$("#updatePhone").val(adminPhone.html());
        //$("#adminQx").val(adminQx.find("b").html());
        $("#updataCaDiv").addClass("addDivshow");
        getInfo(id)
        setTimeout(function () {
            $(".popupbj").fadeIn();
        }, 100);
    }
    function divOff() {
        $(".popupbj").hide();
        $(".addDiv").removeClass("addDivshow");
    }

    $(function () {
        qx_judge('glygl');
        $("#li3").find("a").addClass("aborder");
        sear();
    });
    function add()
    {
        if ($("#addbh").val()=="")
        {
            alert("管理员编号不能为空");
            return;
        }
        if($("#addbh").val()=="admin")
        {
            alert("编号已经存在");
            return;
        }
        if ($("#addname").val() == "") {
            alert("管理员名称不能为空");
            return;
        }
        if ($("#addname").val() == "admin") {
            alert("名称已经存在");
            return;
        }
        if ($("#addphone").val() == "") {
            alert("管理员电话不能为空");
            return;
        }
        if ($("#addPwd1").val() == "") {
            alert("管理员密码不能为空");
            return;
        }
        if ($("#addPwd2").val() == "") {
            alert("管理员密码不能为空");
            return;
        }
        if ($("#addPwd2").val() != $("#addPwd1").val())
        {
            alert("两次输入的密码不一致");
            return;
        }
        $.ajax({
            type: "post",
            url: "adminlist.aspx/add",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{bh:'" + $("#addbh").val() + "',name:'" + $("#addname").val() + "',phone:'" + $("#addphone").val() + "',pwd:'" + $("#addPwd1").val() + "',qx:'" + $("#roleList").val() + "'}",
            success: function (data) {
                if (data.d == "1") {
                    alert("编号重复");
                } else if (data.d == "2") {
                    alert("添加成功");
                }
                location.reload();
            }
        })
    }
    function sear()
    {
        $("#ull").empty();
        $(" <li>"
           +" <label style='width: 20%'>管理编号</label>"
             +" <label style='width:20%'>管理名称</label>"
           +" <label style='width: 20%'>电话</label>"
           +" <label style='width: 20%'>角色</label>"
           + " <label style='width: 20%'>操作</label>"
           +" </li>").appendTo("#ull");
        $.ajax({
            type: "post",
            url: "adminlist.aspx/sear",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{name:'" + $("#keywords1").val() + "',phone:'" + $("#keywords2").val() + "'}",
            success: function (data) {
                var serverdata = $.parseJSON(data.d);
                var serverdatalist = serverdata.length;
                for (var i = 0; i < serverdatalist; i++) {
                    $("  <li>"
                          + "<span class='adminBH' style='width: 20%'>" + serverdata[i].name + "</span>"
                           + "<span class='adminName' style='width: 20%'>" + serverdata[i].nickName + "</span>"
                          + "<span class='adminPhone' style='width: 20%'>" + serverdata[i].linkphone + "</span>"
                          + "<span class='adminQx' style='width: 20%'><b>" + serverdata[i].roleName + "</b></span>"
                          +"<span style='width: 20%'>"
                          + "<a onclick='adminUpd(" + serverdata[i].id+ ")'>修改</a>"
                          +"<a onclick='adminDel("+ serverdata[i].id+")'>删除</a>"
                          + "</span>"
                          +"  </li>").appendTo("#ull");
                }
            }
        })
    }
    function getInfo(operId)
    {
        $("#operID").val(operId);
        $.ajax({
            type: "post",
            url: "adminlist.aspx/getInfo",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{operaID:'" + operId + "'}",
            success: function (data) {
                var serverdata = $.parseJSON(data.d);
                var serverdatalist = serverdata.length;
                for (var i = 0; i < serverdatalist; i++) {
                    $("#updateBH").val(serverdata[i].name);
                    $("#updateName").val(serverdata[i].nickName);
                    $("#updatePhone").val(serverdata[i].linkphone);
                    $("#DropDownList1").val(serverdata[i].qx);
                    
                }
            }
        })
    }
    function update()
    {
        if ($("#updateBH").val() == "") {
            alert("管理员编号不能为空");
            return;
        }
        if ($("#updateName").val() == "") {
            alert("管理员名称不能为空");
            return;
        }
        if ($("#updatePhone").val() == "") {
            alert("管理员电话不能为空");
            return;
        }
        
        if ($("#updatePwd1").val() != $("#updatePwd2").val()) {
            alert("两次输入的密码不一致");
            return;
        }
        $.ajax({
            type: "post",
            url: "adminlist.aspx/update",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{bh:'" + $("#updateBH").val() + "',name:'" + $("#updateName").val() + "',phone:'" + $("#updatePhone").val() + "',pwd:'" + $("#updatePwd1").val() + "',qx:'" + $("#DropDownList1").val() + "',id:'"+ $("#operID").val()+"'}",
            success: function (data) {
                if (data.d == "1") {
                    alert("修改成功");
                } else if (data.d == "2") {
                    alert("修改失败");
                }
                location.reload();
            }
        })
    }
    function adminDel(id)
    {
        if(confirm("确定删除该账号吗  删除之后无法撤销"))
        {
         $.ajax({
            type: "post",
            url: "adminlist.aspx/adminDel",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{id:'"+id+"'}",
            success: function (data) {
                if (data.d == "1") {
                    alert("修改成功");
        } else if (data.d == "2") {
                    alert("修改失败");
        }
                location.reload();
        }
        })
        }
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
                    if (menuID == 'sblb') {//设备类别
                        location.href = "mechineTypeList.aspx";
                    }
                    if (menuID == 'tjsb') {//添加设备
                        location.href = "equipmentadd.aspx";
                    }
                    if (menuID == 'qbsb') {//全部设备
                        location.href = "Allequipment.aspx";
                    }
                    if (menuID == 'qbsb') {//设备管理
                        location.href = "/main/equipment/Allequipment.aspx";
                    }
                    if (menuID == 'hylb') {//会员管理
                        //location.href = "memberlist.aspx";
                    }
                    if (menuID == 'glygl') {//管理员管理
                        //location.href = "/main/Adminlist/adminlist.aspx";
                    }
                    if (menuID == 'cplb') {//产品管理
                        location.href = "/main/product/productlist.aspx";
                    }
                    if (menuID == 'dgjl') {//订单管理
                        location.href = "/main/product/dglist.aspx";
                    }
                    if (menuID == 'zhcx') {//数据统计与查询
                        location.href = "/main/datastatistics/Statisticalquery.aspx";
                    }
                    if (menuID == 'cjdtt') {//分析
                        location.href = "/main/Analysis/Analysis.aspx'";
                    }
                    if (menuID == 'sjdp') {//数据大屏
                        window.open("/main/Big_screen/big_screen.aspx");
                    }
                    if (menuID == 'spgl') {//广告管理
                        location.href = "/main/Advertisement/video.aspx";
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
