<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="devicelist.aspx.cs" Inherits="autosell_center.main.enterprise.devicelist" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>设备设置-自动售卖终端中心控制系统</title>
    <link rel="shortcut icon" href="/main/public/images/favicon.ico" />
    <link href="/main/public/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/common.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/style.css" rel="stylesheet" type="text/css" />
    <script src="/main/public/script/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="/main/public/script/Titlelist.js" type="text/javascript"></script>
    <link href="../../scripts/skin/jedate.css" rel="stylesheet" type="text/css" />
    <script src="../../scripts/jedate.js" type="text/javascript"></script>
    <script src="../../scripts/jedate.min.js"></script>
    <style>
        .fsopen span{
            position:absolute;
            left:126px;
        }
        .fsopen b{
            font-weight:300;
            display:block;
            float:right;
            line-height:36px;
            font-size:1.2rem;
            color:#666;
        }
    </style>
    <script type="text/javascript">
        window.onload = function () {
            jeDate({
                dateCell: "#start", //isinitVal:true,
                //format: "YYYY-MM-DD",
                isTime: false, //isClear:false,
                choose: function(val) {},
                minDate: "2014-09-19 00:00:00"
            });
            jeDate({
                dateCell: "#end",
                //isinitVal: true,
                isTime: true, //isClear:false,
                minDate: "2014-09-19 00:00:00"
            });
            jeDate({
                dateCell: "#yxqEnd",
                isinitVal: true,
                isTime: true, //isClear:false,
                minDate: "2014-09-19 00:00:00"
            });

        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="header"></div>
        <div class="main">
            <div class="main_list">
                <div class="common_title">
                    <h4>
                        <i class="fa fa-plus"></i>
                        <span>奶企管理</span>
                    </h4>
                </div>
                <div class="common_main">
                    <div class="navlist">
                        <dl>
                            <dt>奶企设备<em class="fa fa-cog"></em></dt>
                            <dd>
                                <a class="change acolor"><i class="change icolor fa fa-check-square"></i>设备列表</a>
                            </dd>
                            <dd>
                                <a class="change" href="firmcon.aspx?companyID=<%=company_ID %>"><i class="change fa fa-plus-square"></i>新增设备</a>
                            </dd>
                        </dl>
                        <dl>
                            <dt>奶企设置<em class="fa fa-cog"></em></dt>
                            <dd>
                                <a class="change" href="Thepublicjb.aspx?companyID=<%=company_ID %>"><i class="change fa fa-wechat"></i>公众号设置</a>
                            </dd>
                        <%--    <dd>
                                <a class="change" href="Paymentzf.aspx?companyID=<%=company_ID %>"><i class="change fa fa-money"></i>支付设置</a>
                            </dd>
                            <dd>
                                <a class="change" href="Profit.aspx?companyID=<%=company_ID %>"><i class="change fa fa-database"></i>分润设置</a>
                            </dd>--%>
                        </dl>
                    </div>
                    <section class="jiqlist">
                        <div class="change zfpopup">
                            <h4>机器设置</h4>
                            <ul>
                                <li>
                                    <h5>有效期至</h5>
                                    <label>
                                          <input name="act_stop_timeks" type="text" id="yxqEnd" runat="server" class="input" value="" placeholder="有效期截止时间" readonly="true" />
                                    </label>
                                </li>
                                <li class="fsopen">
                                    <h5>状态</h5>
                                    <span>
                                        <i class="change iopen"></i>
                                        <em class="change emopen"></em>
                                        <input type="hidden"   id="EquType"/>
                                    </span>
                                    <b></b>
                                </li>
                            </ul>
                            <dl>
                                <dd>
                                    <input type="button" value="确定" class="popupqdbtn" onclick="okBtn()" />
                                </dd>
                                <dd>
                                    <input type="button" value="取消" onclick="qxClick()" />
                                </dd>
                            </dl>
                        </div>
                      
                        <div class="popupbj" style="z-index:-2"></div>
                        <dl class="jiqlistTab">
                            <dd class="change ddcolor"><b>设备列表</b></dd>
                        </dl>
                        <ul class="jiqlistseach" id="top">
                            <li>
                                <input name="act_stop_timeks" type="text" id="start" runat="server" class="input" value="" placeholder="开始时间" readonly="true" />
                            </li>
                            <li>
                                <input name="act_stop_timeks" type="text" id="end" runat="server" class="input" value="" placeholder="截止时间" readonly="true" />
                            </li>
                            <li>
                                <input type="text" value="" placeholder="设备编号" id="bh" runat="server" />
                            </li>
                            <li>
                                <asp:DropDownList ID="typeDrop" runat="server">
                                    <asp:ListItem Value="0">全部分类</asp:ListItem>
                                    <asp:ListItem Value="2">正常</asp:ListItem>
                                    <asp:ListItem Value="1">禁用</asp:ListItem>
                                    <asp:ListItem Value="3">过期</asp:ListItem>
                                </asp:DropDownList>
                            </li>
                            <li>
                                <input onclick="sear()" class="seachbtn" value="查询" type="button" />
                            </li>
                        </ul>
                        <ul class="jiqlisttable" style="display: block;" id="ul1">
                            
                        </ul>
                        <ul class="jiqlisttable" id="ul2">
                            <%
                                if (gqData != null)
                                {
                                    for (int j = 0; j < gqData.Rows.Count; j++)
                                    {%>
                            <li>
                                <dl>
                                    <dd style="width:25%">机器</dd>
                                    <dd style="width:12%">有效期</dd>
                                    <dd style="width:12%">状态</dd>
                                      <dd style="width:12%">运行状态</dd>
                                    <dd style="width:12%">公司</dd>
                                    <dd style="width:12%">类型</dd>
                                    <dd style="width:12%">操作</dd>
                                </dl>
                                <label style="width:25%">
                                    <img src="/main/public/images/smjicon.png" alt="" />
                                    <span><%=gqData.Rows[j]["bh"].ToString() %></span>
                                    <em>注册时间:<i><%=gqData.Rows[j]["regTime"].ToString() %></i></em>
                                </label>
                                <label style="width:12%"><%=gqData.Rows[j]["validateTime"] %></label>
                                <label style="width:12%"><b><%=gqData.Rows[j]["sta"] %></b></label>
                                 <label style="width:12%"><b><%=gqData.Rows[j]["t"] %></b></label>
                                <label style="width:12%"><%=gqData.Rows[j]["sta"] %></label>
                                 <label style="width:12%"><%=gqData.Rows[j]["mechineType"] %></label>
                                <label style="width:12%">
                                    <a onclick="shanchu(<%=gqData.Rows[j]["id"] %>)">删除</a>
                                </label>
                            </li>
                            <%}
                                }
                            %>
                        </ul>
                    </section>
                </div>
            </div>
        </div>
        <input id="mechineID" runat="server" type="hidden"/>
        <input id="companyID" runat="server" type="hidden"/>
    </form>
</body>

</html>
<script>
    $(function () {
        $(".jiqlistTab").find("dd").click(function () {
            $(".jiqlistTab dd").removeClass("ddcolor");
            $(this).addClass("ddcolor");
            var $liNum = $(this).index();
            if ($liNum == "1") {
                $("#top").hide();
            } else {
                $("#top").show();
            }
            $(".jiqlisttable").hide();
            $(".jiqlisttable").eq($liNum).fadeIn();
        });
       
        $(".fsopen").find("span").click(function () {
            $(this).find("em").toggleClass("emopen");
            $(this).find("i").toggleClass("iopen");
            if ($(this).find("em").hasClass("emopen")) {
                $(this).children("input").val("1");
                $("#EquType").val("2");
                fsopenB.html("已开启");
            } else {
                $(this).children("input").val("2");
                $("#EquType").val("1");
                fsopenB.html("已关闭");
            }
        });
        sear();
    });
    function shanchu(id) {
        $.ajax({
            url: "../../ashx/asm.ashx",
            type: 'post',
            dataType: 'json',
            timeout: 10000,
            data: {
                action: "deleteEqui",
                 
                equID: id
            },
            success: function (resultData) {
                if (resultData == "1") {
                    alert("删除成功");
                } else if (resultData == "2") {
                    alert("删除失败");
                }
            }

        })
    }
    function sear() {
        $.ajax({
            type: "post",
            url: "devicelist.aspx/search",
            contentType: "application/json; charset=utf-8",
            data: "{startTime:'" + $("#start").val() + "',endTime:'" + $("#end").val() + "',bh:'" + $("#bh").val() + "',type:'" + $("#typeDrop").val() + "',companyID:'" + $("#companyID").val() + "'}",
            dataType: "json",
            success: function (data) {
                $("#ul1").empty();
                var serverdata = $.parseJSON(data.d);
                var serverdatalist = serverdata.length;
                for (var i = 0; i < serverdatalist; i++) {
                    $("<li><dl><dd style='width:25%'>机器</dd>"
                      + " <dd style='width:12%'>有效期</dd>"
                      +" <dd style='width:12%'>状态</dd>"
                      +"  <dd style='width:12%'>运行状态</dd>" 
                      + " <dd style='width:12%'>公司</dd> "
                      + "  <dd style='width:12%'>类型</dd>"
                      + " <dd style='width:12%'>操作</dd></dl>"
                       +" <label style='width:25%'><img src='/main/public/images/smjicon.png' alt='' />"
                       +"<span>" + serverdata[i].bh + "</span><em>注册时间:<i>" + serverdata[i].regTime.substring(0, 10) + "</i></em></label>"
                       +"<label style='width:12%'>" + serverdata[i].validateTime.substring(0, 10) + "</label>"
                       +"<label style='width:12%'><b>" + serverdata[i].sta + "</a></label><label style='width:12%'><b>" + serverdata[i].t + "</a></label>"
                       +"<label style='width:12%'>" + serverdata[i].name + "</label> <label style='width:12%'>" + serverdata[i].mechineType + "</label>"
                       +"<label style='width:12%'>"
                       + "<a onclick='setUp(" + serverdata[i].id + ")'>设置</a> "
                       + "<a href='kclist.aspx?mechineID=" + serverdata[i].id + "&companyID=" + $("#companyID").val() + "'>库存</a> "
                        + ""
                       +"</label></li>").appendTo("#ul1");
                }
            }
        })
    };
    function okBtn()
    {
        $.ajax({
            url: "../../ashx/asm.ashx",
            type: 'post',
            dataType: 'json',
            timeout: 10000,
            data: {
                action: "saveEquiSet",
                yxq: $("#yxqEnd").val(),
                type: $("#EquType").val(),
                equID: $("#mechineID").val()
            },
            success: function (resultData) {
                if (resultData == "1") {                 
                    alert("修改成功");
                    qxClick();
                    sear();
                } else if (resultData == "2") {
                    alert("修改失败");
                }
            }
        })
    }
  
    function setUp(id) {
        //根据id从后台在取有效期和状态
        $("#mechineID").val(id);
        $.ajax({
            type: "post",
            url: "devicelist.aspx/getInfo",
            contentType: "application/json; charset=utf-8",
            data: "{id:'"+id+"'}",
            dataType: "json",
            success: function (data) {
                $("#yxqEnd").val(data.d.split('|')[0]);
                $("#EquType").val(data.d.split('|')[1]);

                var $emOpen = $("#EquType");
                var fsopenB = $(".fsopen").find("b");
                
                if ($emOpen.val() == "2") {
                    fsopenB.html("已开启");
                    $(".fsopen").find("i").addClass("iopen");
                    $(".fsopen").find("em").addClass("emopen");
                } else {
                    fsopenB.html("已关闭");
                    $(".fsopen").find("i").removeClass("iopen");
                    $(".fsopen").find("em").removeClass("emopen");
                }
            }
        })
        $(".popupbj").fadeIn();
        $(".zfpopup").addClass("zfpopup_on");
        
    };
    function qxClick() {
        $(".zfpopup").removeClass("zfpopup_on");
        setTimeout(function () { $(".popupbj").hide(); }, 300);

    }
</script>
 <script>
    $(function () {
        $("#li0").find("a").addClass("aborder");
    })
</script>
