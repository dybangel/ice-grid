<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="productclass.aspx.cs" Inherits="autosell_center.main.product.productclass" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>购买类型-自动售卖终端中心控制系统</title>
    <link rel="shortcut icon" href="/main/public/images/favicon.ico" />
    <link href="/main/public/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/common.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/style.css" rel="stylesheet" type="text/css" />
    <script src="/main/public/script/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="/main/public/script/Titlelist.js" type="text/javascript"></script>
    <style>
        .firmbtn {
            margin-left: 30px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="header"></div>
        <div class="main">
            <div id="addCaDiv" class="addDiv change">
                <h4>添加分类</h4>
                <ul>
                    <li>
                        <label>分类名称</label>
                        <input type="text" value="" placeholder="填写分类名称"  id="addName"/>
                    </li>
                  
                    <li>
                        <label></label>
                        <input type="button" value="确认添加" class="btnok" onclick="btnok()"/>
                        <input type="button" value="取消" class="btnoff" onclick="divOff()" />
                    </li>
                </ul>
            </div>
            <div id="updataCaDiv" class="addDiv change">
                <h4>修改分类</h4>
                <ul>
                    <li>
                        <label>分类名称</label>
                        <input type="text" id="updateCN" value="" placeholder="填写分类名称" />
                    </li>
                 
                    <li>
                        <label></label>
                        <input type="button" value="确认修改" onclick="btnUpdate()" class="btnok"/>
                        <input type="button" value="取消" class="btnoff" onclick="divOff()" />
                    </li>
                </ul>
            </div>
            <div class="popupbj"></div>
            <div class="main_list">
                <div class="common_title">
                    <h4>
                        <i class="fa fa-cubes"></i>
                        <span>商品管理</span>
                    </h4>
                </div>
                <div class="common_main">
                    <div class="navlist">
                        <dl>
                            <dt>商品信息<em class="fa fa-cog"></em></dt>
                            <dd>
                                <a class="change" href="productlist.aspx"><i class="change fa fa-file-text"></i>商品列表</a>
                            </dd>
                            <dd>
                               <a class="change" href="productclass.aspx"><i class="change fa fa-window-restore"></i>添加分类</a>
                            </dd>
                            <dd>
                                <a class="change acolor" href="productadd.aspx"><i class="change icolor fa fa-window-restore"></i>添加商品</a>
                            </dd>
                        </dl>
                      <%--  <dl>
                            <dt>规格与保质期<em class="fa fa-cog"></em></dt>
                            <dd>
                                <a class="change" href="standard.aspx"><i class="change fa fa-tags"></i>商品规格</a>
                            </dd>
                            <dd>
                                <a class="change"><i class="change fa fa-shield"></i>商品保质期</a>
                            </dd>
                        </dl>--%>
                    </div>
                    <section class="jiqlist">
                        <dl class="jiqlistTab">
                            <dd class="change ddcolor"><b>商品分类</b></dd>
                        </dl>
                        <div class="addClass">
                            <a class="change" onclick="addDiv()">
                                <i class="fa fa-plus"></i>
                                添加分类
                            </a>
                        </div>
                        <ul class="productclass" id="ull">
                           
                           
                        </ul>
                    </section>
                </div>
            </div>
        </div>
       <input  id="companyID" runat="server" type="hidden"/>
        <input  id="typeID" runat="server" type="hidden"/>
    </form>
</body>
</html>
<script>
    $(function () {
        
        sear();
    })
    function addDiv() {
        $("#addCaDiv").addClass("addDivshow");
        setTimeout(function() {
            $(".popupbj").fadeIn();
        }, 100);
    }
    function divOff() {
        $(".popupbj").hide();
        $(".addDiv").removeClass("addDivshow");
    }
    //修改弹框
    function updeted(obj) {
        var $a = $(obj);
        var $li = $a.parent().parent().eq(0);
        var va = $li.find("input").val();
        
        $("#typeID").val(va);
        $("#updateCN").val($li.find("span.classname").eq(0).html());
        $("#updateCId").val($li.find("span.classid").eq(0).html());
        $("#updataCaDiv").addClass("addDivshow");
        setTimeout(function() {
            $(".popupbj").fadeIn();
        }, 100);
    }
    //删除
    function deleTed(obj) {
        var $a = $(obj);
        var $li = $a.parent().parent().eq(0);
        var va = $li.find("input").val();
       
        $.ajax({
            type: "post",
            url: "productclass.aspx/deleTed",
            contentType: "application/json; charset=utf-8",
            data: "{id:'" + va+ "',companyID:'" + $("#companyID").val() + "'}",
            dataType: "json",
            success: function (data) {
                if (data.d == "1") {
                    alert("删除成功");
                } else if (data.d == "2") {
                    alert("该类型已经使用无法删除");
                }
                location.reload();
            }
        })
       
    }
    function sear()
    {
        $("#ull").empty();
        $(" <li>"
                      + "<label class='classid'>行号</label>"
                      + "<label class='classname'>分类名称</label>"
                      + "<label class='classdelete'>操作</label>"
                      + "</li>").appendTo("#ull");
       
        $.ajax({
            type: "post",
            url: "productclass.aspx/sear",
            contentType: "application/json; charset=utf-8",
            data: "{companyID:'" + $("#companyID").val() + "'}",
            dataType: "json",
            success: function (data) {
                var serverdata = $.parseJSON(data.d);
                var serverdatalist = serverdata.length;
                for (var i = 0; i < serverdatalist; i++) {
                   
                    $(" <li>"
                            + "    <span class='classid'>" + serverdata[i].num + "</span>"
                            +"    <span class='classname'>"+serverdata[i].typeName+"</span>"
                            +"    <span class='classdelete'>"
                            +"        <a onclick='updeted(this)'>修改</a>"
                            +"        <a onclick='deleTed(this)'>删除</a>"
                            + "    </span>"
                            + " <input type='hidden' value='" + serverdata[i].productTypeID + "'/>"
                            +"</li>").appendTo("#ull");
                }
            }
        })
    }
    function btnok()
    {
        if ($("#addName").val().trim() == "") {
            alert("名称不能为空");
            return;
        }
        $.ajax({
            type: "post",
            url: "productclass.aspx/add",
            contentType: "application/json; charset=utf-8",
            data: "{name:'" + $("#addName").val() + "',companyID:'" + $("#companyID").val() + "'}",
            dataType: "json",
            success: function (data) {
                if (data.d == "1") {
                    alert("添加成功");
                } else if(data.d=="2"){
                    alert("该类型已经存在");
                }
                location.reload();
            }
        })
    }
    function btnUpdate() {
        if ($("#updateCN").val().trim()=="")
        {
            alert("名称不能为空");
            return;
        }
        $.ajax({
            type: "post",
            url: "productclass.aspx/update",
            contentType: "application/json; charset=utf-8",
            data: "{id:'" + $("#typeID").val() + "',name:'" + $("#updateCN").val() + "',companyID:'" + $("#companyID").val() + "'}",
            dataType: "json",
            success: function (data) {
                if (data.d == "1") {
                    alert("修改成功");
                } else if (data.d == "2") {
                    alert("该类型已经存在");
                }
                location.reload();
            }
        })
    }
</script>
<script>
    $(function () {
        $("#li3").find("a").addClass("aborder");
    })
</script>