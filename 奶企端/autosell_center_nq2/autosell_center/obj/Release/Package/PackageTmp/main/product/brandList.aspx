<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="brandList.aspx.cs" Inherits="autosell_center.main.product.brandList" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>商品保质期-自动售卖终端中心控制系统</title>
    <link rel="shortcut icon" href="/main/public/images/favicon.ico" />
    <link href="/main/public/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/common.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/style.css" rel="stylesheet" type="text/css" />
    <script src="/main/public/script/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="/main/public/script/Titlelist.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="adminpopup" class="change zfpopup">
            <h4>品牌名称设置</h4>
            <ul>
                <li>
                    <h5>设置品牌名称</h5>
                    <label>
                           <input name="_brandName" type="text" id="_brandName" class="input" value="" placeholder="品牌名称" />
                    </label>
                </li>
            </ul>
            <dl>
                <dd>
                    <input type="button" value="确定" class="popupqdbtn" onclick="ok()" />
                </dd>
                <dd>
                    <input type="button" value="取消" onclick="qxClick()" />
                </dd>
            </dl>
        </div>
        <div class="header"></div>
        <div class="main">
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
                                <a class="change " href="productlist.aspx"><i class="change  fa fa-file-text"></i>商品列表</a>
                            </dd>
                             <dd>
                                <a class="change" href="xstjlist.aspx"><i class="change fa fa-file-text"></i>限时活动产品</a>
                            </dd>
                            <dd>
                                <a class="change " href="productadd.aspx"><i class="change  fa fa-plus-square"></i>添加商品</a>
                            </dd>
                            <dd>
                                <a class="change acolor" href="brandList.aspx"><i class="change icolor fa fa-window-restore"></i>品牌列表</a>
                            </dd>
                        </dl>
                    </div>
                    <section class="jiqlist">
                        <dl class="jiqlistTab">
                            <dd class="change ddcolor"><b>品牌列表</b></dd>
                        </dl>
                        <ul class="jiqlistseach">
                            <li>
                                <input type="text" value="" placeholder="品牌名称" id="brandName" />
                            </li>
                            <li class="naiqBtn" style="display: none">
                                <asp:DropDownList ID="companyList" runat="server" />
                            </li>
                            <li>
                                <input type="button" value="查询" class="seachbtn" onclick="search()" />
                            </li>
                             <li>
                                <input type="button" value="添加" class="seachbtn" onclick="adminSet()" />
                            </li>
                        </ul>
                        <ul class="productlist" id="product">
                        </ul>
                    </section>
                </div>
            </div>
        </div>
        <input id="companyId" runat="server" type="hidden" />
        <input id="_brandID" runat="server" type="hidden" />
        <input id="_operaID" runat="server" type="hidden" />
    </form>
</body>
</html>
<script>
    function judge() {
        $.ajax({
            type: "post",
            url: "brandList.aspx/judge",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{operaID:'" + $("#_operaID").val() + "',menuID:'pplb'}",
            success: function (data) {
                if (data.d.code == "500") {
                    $(".jiqlist").empty();
                    $(".jiqlist").html("<p class='noquanxian'>当前没有权限</p>").css({ "background": "#ddd" });
                }
            }
        })

    }
    function adminSet() {
        //$("#_brandID").val(id);
        //$("#_brandName").val(name);
        $(".popupbj").fadeIn();
        $("#adminpopup").addClass("zfpopup_on");
    }
    function qxClick() {
        $(".tangram-suggestion-main").hide();
        $(".popupbj").hide();
        $(".zfpopup").removeClass("zfpopup_on");
    }
    function ok()
    {
        $.ajax({
            type: "post",
            url: "brandList.aspx/ok",
            contentType: "application/json; charset=utf-8",
            data: "{brandID:'" + $("#_brandID").val() + "',brandName:'" + $("#_brandName").val() + "',companyId:'" + $("#companyId").val() + "'}",
            dataType: "json",
            success: function (data) {
                if (data.d.code == 0) {
                    window.location.reload();
                } else {
                    alert(data.d.msg);
                }
            }
        })
    }
    $(function () {
        
        $("#li3").find("a").addClass("aborder");
        judge()
        search();
    })
    function search()
    {
        $("#product").empty();
        $(" <li>"
                 +"  <label style='width:80%'>商品名称</label>"
                 + "  <label style='width:20%'>操作</label>"
                 +"  </li>").appendTo("#product");
        $.ajax({
            type: "post",
            url: "brandList.aspx/getBrandList",
            contentType: "application/json; charset=utf-8",
            data: "{keyword:'" + $("#brandName").val() + "',companyId:'" + $("#companyId").val() + "'}",
            dataType: "json",
            success: function (data) {
                var serverdata = $.parseJSON(data.d);
                var serverdatalist = serverdata.length;
                for (var i = 0; i < serverdatalist; i++) {
                    $(" <li>"
                             + "   <span style='width:80%;line-height:40px'>" + serverdata[i].brandName + "</span>"
                             + "   <span style='width:20%;line-height:40px'>"
                             //+ "       <a  onclick=\"adminSet('" + serverdata[i].id + "','" + serverdata[i].brandName + "')\" >修改</a>"
                             + "       <a onclick='deLete(" + serverdata[i].id + ")'>删除</a>"
                             +"   </span>"
                            +"</li>").appendTo("#product");
                }
            }
        })
    }
    
    function deLete(id)
    {
        if(confirm("是否确定删除该品牌"))
        {
            $.ajax({
                type: "post",
                url: "brandlist.aspx/del",
                contentType: "application/json; charset=utf-8",
                data: "{brandID:'" + id + "'}",
                dataType: "json",
                success: function (data) {
                    if (data.d.code == 0) {
                        window.location.reload();
                    } else {
                        alert(data.d.msg);
                    }
                }
            })
        }
        
    }
    
</script>
