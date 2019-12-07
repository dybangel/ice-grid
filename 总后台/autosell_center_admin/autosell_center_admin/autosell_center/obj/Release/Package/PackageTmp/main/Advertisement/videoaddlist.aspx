<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="videoaddlist.aspx.cs" Inherits="autosell_center.main.Advertisement.videoaddlist" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>视频管理-自动售卖终端中心控制系统</title>
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

        .productlist li span {
            overflow: hidden;
            text-overflow: ellipsis;
            white-space: nowrap;
        }

        .productlist li .pronaiq {
            width: 24%;
        }
        .jiqlisttable li .jiqzt {
            width: 42%;
            text-align: right;
            padding-right: 30px;
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
                                <a class="change acolor"><i class="change icolor fa fa-video-camera"></i>视频管理</a>
                            </dd>

                            <dd>
                                <a class="change" href="Jurisdiction.aspx"><i class="change fa fa-hdd-o"></i>机器添加视频</a>
                            </dd>
                        </dl>
                    </div>
                    <section class="jiqlist">
                        <dl class="jiqlistTab">
                            <dd class="change ddcolor"><b>视频添加到机器</b></dd>
                        </dl>
                        <ul class="jiqlistseach">
                            <li>
                                <input type="text" value="" placeholder="机器编号" id="bh" />
                            </li>
                            <li>
                               <asp:DropDownList ID="companyList" runat="server"></asp:DropDownList>
                            </li>
                            <li>
                                <select id="s_province" name="s_province"></select>
                            </li>
                            <li>
                                 <select id="s_city" name="s_city"></select>
                            </li>
                            <li>
                                 <select id="s_county" name="s_county"></select>
                            </li>

                            <li>
                                <input type="button" value="查询" class="seachbtn" id="search" onclick="sear()" />
                            </li>
                            <li style="text-align: right">
                                <input type="button" value="确定添加" class="seachbtn" onclick="add()"/>
                            </li>
                        </ul>
                        <ul class="jiqlisttable" style="display: block;" id="ull">
                           
                            
                        </ul>
                    </section>
                </div>
            </div>
        </div>
        <input  id="videoID" runat="server" type="hidden"/>
    </form>
</body>
</html>
<script>
    $(function () {
        sear();
       
    })
    function sear() {
        $("#ull").empty();
        $.ajax({
            type: "post",
            url: "videoaddlist.aspx/sear",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{bh:'" + $("#bh").val() + "',companyID:'" + $("#companyList").val() + "',province:'" + $("#s_province").val() + "',city:'" + $("#s_city").val() + "',country:'" + $("#s_county").val() + "'}",
            success: function (data) {
                var serverdata = $.parseJSON(data.d);
                var serverdatalist = serverdata.length;
                for (var i = 0; i < serverdatalist; i++) {
                    $(" <li class='a'>"
                                +"<dl>"
                                +"    <dd style='padding-left:30px;width:5%'>选择</dd>"
                                +"    <dd style='width:14%'>机器编号</dd>"
                                +"    <dd style='width:14%'>机器名称</dd>"
                                +"    <dd style='width:14%'>企业</dd>"
                                +"    <dd style='width:13%'>省</dd>"
                                +"    <dd style='width:13%'>市</dd>"
                                +"    <dd style='width:13%'>县</dd>"
                                +"    <dd style='width:14%'>状态</dd>"
                                +"</dl>"
                                +"<label style='padding-left:30px;width:5%'>"
                                + "    <input type='checkbox' onclick='checkxz(this)' />"
                                 + "   <input type='hidden' value='" + serverdata[i].id+ "' name='_thiszt' />"
                                +"</label>"
                                + "<label style='width:14%'>" + serverdata[i].bh + "</label>"
                                + "<label style='width:14%'>" + serverdata[i].mechineName + "</label>"
                                + "<label style='width:14%'>" + serverdata[i].companyName + "</label>"
                                + "<label style='width:13%'>" + serverdata[i].province + "</label>"
                                + "<label style='width:13%'>" + serverdata[i].city + "</label>"
                                + "<label style='width:13%'>" + serverdata[i].country + "</label>"
                               +" <label style='width:14%'>"
                                + "    <a>" + serverdata[i].zt + "</a>"
                                +"</label>"
                           +" </li>").appendTo("#ull");
                }
            }
        })
    }

    function checkxz(obj) {
        //if()
        $(obj).attr({ "checked": "checked" });
    }

    function add()
    {
        var id = "";
        $(".a").each(function () {
            if ($(this).find("label").find("input").is(":checked"))
            {
                id += $(this).find("label").find("input").eq(1).val() + ",";
            }
        })
        id = id.substring(0, id.length - 1);
        if(id.length<=0)
        {
            alert("请勾选机器");
            return;
        }
        $.ajax({
            type: "post",
            url: "videoaddlist.aspx/add",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{videoID:'" + $("#videoID").val() + "',mechineIDS:'"+id+"'}",
            success: function (data) {
                if(data.d=="1")
                {
                    alert("视频添加成功");
                }
            }
        })
    }
</script>



<!-- 全国省市县三级联动 -->
<script src="../Big_screen/public/area.js"></script>
<script type="text/javascript">_init_area();</script>
<script type="text/javascript">
    //  全国省市县三级联动
    var Gid = document.getElementById;
    var showArea = function () {
        Gid('show').innerHTML = "<h3>省" + Gid('s_province').value + " - 市" +
            Gid('s_city').value + " - 县/区" +
            Gid('s_county').value + "</h3>";
    }
    Gid('s_county').setAttribute('onchange', 'showArea()');
  
</script>


