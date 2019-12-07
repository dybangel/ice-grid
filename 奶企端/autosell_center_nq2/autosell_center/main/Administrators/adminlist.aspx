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
                                <a class="change acolor" href="adminlist.aspx"><i class="change icolor fa fa-user"></i>管理员管理</a>
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
                                    <input type="text" value="" placeholder="填写管理员编号用于登录" id="addbh"   runat="server"/>
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
                                    <label>APP权限</label>
                                     <asp:DropDownList ID="appqx" runat="server">
                                         <asp:ListItem Value="0">无权限登录</asp:ListItem>
                                         <asp:ListItem Value="1">管理员</asp:ListItem>
                                         <asp:ListItem Value="2">补货员</asp:ListItem>
                                     </asp:DropDownList>
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
                                     <asp:DropDownList ID="DropDownList1" runat="server">

                                     </asp:DropDownList>
                                </li>
                                 <li>
                                    <label>APP权限</label>
                                     <asp:DropDownList ID="DropDownList2" runat="server">
                                         <asp:ListItem Value="0">无权限登录</asp:ListItem>
                                         <asp:ListItem Value="1">管理员</asp:ListItem>
                                         <asp:ListItem Value="2">补货员</asp:ListItem>
                                     </asp:DropDownList>
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
      <input id="companyID" runat="server" type="hidden"/>
    </form>
</body>
</html>
<script>
    function judge() {
        $.ajax({
            type: "post",
            url: "adminlist.aspx/judge",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{operaID:'" + $("#operID").val() + "',menuID:'glygl'}",
            success: function (data) {
                if (data.d.code == "500") {
                    $(".jiqlist").empty();
                    $(".jiqlist").html("<p class='noquanxian'>当前没有权限</p>").css({ "background": "#ddd" });
                }
            }
        })
    }
    function addadmin() {
        $.ajax({
            type: "post",
            url: "adminlist.aspx/getMaxBH",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{companyID:'" + $("#companyID").val() + "'}",
            success: function (data) {
                $("#addbh").val(data.d);
            }
        })
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
        judge()
        $("#li5").find("a").addClass("aborder");
        sear();
    });
    function add()
    {
        
        if ($("#addbh").val()=="")
        {
            alert("管理员编号不能为空");
            return;
        }
        if ($("#addname").val() == "") {
            alert("管理员名称不能为空");
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
            data: "{bh:'" + $("#addbh").val() + "',name:'" + $("#addname").val() + "',phone:'" + $("#addphone").val() + "',pwd:'" + $("#addPwd1").val() + "',qx:'" + $("#roleList").val() + "',companyID:'" + $("#companyID").val() + "',operaID:'" + $("#operID").val() + "',appqx:'" + $("#appqx").val() + "'}",
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
           +" <label style='width: 16.6%'>管理编号</label>"
             +" <label style='width:16.6%'>管理名称</label>"
           +" <label style='width: 16.6%'>电话</label>"
           + " <label style='width: 16.6%'>角色</label>"
           + " <label style='width: 16.6%'>APP权限</label>"
           + " <label style='width: 16.6%'>操作</label>"
           +" </li>").appendTo("#ull");
        $.ajax({
            type: "post",
            url: "adminlist.aspx/sear",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{name:'" + $("#keywords1").val() + "',phone:'" + $("#keywords2").val() + "',companyID:'" + $("#companyID").val() + "',operaID:'" + $("#operID").val() + "'}",
            success: function (data) {
                var serverdata = $.parseJSON(data.d);
                var serverdatalist = serverdata.length;
                for (var i = 0; i < serverdatalist; i++) {
                    var appQXName = "";
                    if (serverdata[i].appQX == "0")
                    {
                        appQXName = "无权限登录";
                    } else if (serverdata[i].appQX == "1")
                    {
                        appQXName = "管理员";
                    } else if (serverdata[i].appQX == "2") {
                        appQXName = "补货员";
                    } else {
                        appQXName = "暂未设置";
                    }
                    $("  <li>"
                          + "<span class='adminBH' style='width: 16.6%'>" + serverdata[i].name + "</span>"
                          + "<span class='adminName' style='width: 16.6%'>" + serverdata[i].nickName + "</span>"
                          + "<span class='adminPhone' style='width: 16.6%'>" + serverdata[i].linkphone + "</span>"
                          + "<span class='adminQx' style='width: 16.6%'><b>" + serverdata[i].roleName + "</b></span>"
                          + "<span class='adminQx' style='width: 16.6%'><b>" +appQXName + "</b></span>"
                          +"<span style='width: 16.6%'>"
                          + "<a onclick='adminUpd(" + serverdata[i].id+ ")'>修改</a>"
                          +"<a onclick='adminDel("+ serverdata[i].id+")'>删除</a>"
                          + "</span>"
                          +" </li>").appendTo("#ull");
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
            data: "{operaID:'" + operId + "',companyID:'" + $("#companyID").val() + "'}",
            success: function (data) {
                var serverdata = $.parseJSON(data.d);
                var serverdatalist = serverdata.length;
                for (var i = 0; i < serverdatalist; i++) {
                    $("#updateBH").val(serverdata[i].name);
                    $("#updateName").val(serverdata[i].nickName);
                    $("#updatePhone").val(serverdata[i].linkphone);
                    $("#DropDownList1").val(serverdata[i].qx);
                    $("#DropDownList2").val(serverdata[i].appQX);
                    
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
            data: "{bh:'" + $("#updateBH").val() + "',name:'" + $("#updateName").val() + "',phone:'" + $("#updatePhone").val() + "',pwd:'" + $("#updatePwd1").val() + "',qx:'" + $("#DropDownList1").val() + "',id:'" + $("#operID").val() + "',appqx:'" + $("#DropDownList2").val() + "'}",
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
     
</script>
