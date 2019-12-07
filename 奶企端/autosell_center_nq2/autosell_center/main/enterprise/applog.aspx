<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="applog.aspx.cs" Inherits="autosell_center.main.enterprise.applog" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
   
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>公众号设置-自动售卖终端中心控制系统</title>
    <link rel="shortcut icon" href="/main/public/images/favicon.ico" />
      <link rel="shortcut icon" href="/main/public/images/favicon.ico" />
    <link href="/main/public/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/common.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/style.css" rel="stylesheet" type="text/css" />

    <link rel="stylesheet" type="text/css" href="../bootstrapSelect/css/bootstrap.css"/>
    <link rel="stylesheet" type="text/css" href="../bootstrapSelect/dist/css/bootstrap-select.css"/>
   <script src="../bootstrapSelect/dist/js/jquery.js"></script>
   
    <script src="../bootstrapSelect/dist/js/bootstrap.min.js"></script> 
    <script src="../bootstrapSelect/dist/js/bootstrap-select.min.js" charset="gb2312"></script>
   
   
    <script src="/main/public/script/Titlelist.js" type="text/javascript"></script>
    <link  href="../../scripts/skin/jedate.css" rel="stylesheet" type="text/css"/>
    <script src="../../scripts/jedate.js" type="text/javascript"></script>
    <script src="../../scripts/jedate.min.js"></script>
    <style>
      .whiteDiv {
    position: fixed;
    width: 36%;
    height: 16vh;
    top: 30vh;
    left: 32%;
    background: #fff;
    border-radius: 5px;
    z-index: 999;
    padding: 30px 0;
    box-sizing: border-box;
}

    .whiteDiv span {
        width: 100%;
        text-align: center;
        float: left;
        font-size: 16px;
        color: #999;
    }

    .whiteDiv div {
        width: 100%;
        float: left;
    }

    .whiteDiv a {
        width: 40%;
        margin-left: 30%;
        display: block;
        height: 36px;
        line-height: 36px;
        color: #f0f0f0;
        background: #18AC15;
        outline: none;
        text-decoration: none;
        text-align: center;
        margin-top: 30px;
        border-radius: 5px;
        font-size: 16px;
    }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="header"></div>
        <div class="main">
            <div class="popupbj"></div>
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
                            <dt>公众号设置<em class="fa fa-cog"></em></dt>
                            <dd>
                                <a class="change acolor" href="authPage.aspx"><i class="change icolor fa fa-wechat"></i>公众号设置</a>
                            </dd>
                        </dl>
                         <dl>
                            <dt>自定义菜单设置<em class="fa fa-cog"></em></dt>
                            <dd>
                                <a class="change " href="Thepublicjb.aspx"><i class="change  fa fa-wechat"></i>自定义菜单设置</a>
                            </dd>
                        </dl>
                        <dl>
                            <dt>模板消息<em class="fa fa-cog"></em></dt>
                            <dd>
                                <a class="change" href="Distributor.aspx"><i class="change fa fa-envelope"></i>模板消息</a>
                            </dd>
                        </dl>
                         <dl>
                            <dt>参数设置<em class="fa fa-cog"></em></dt>
                            <dd>
                                 <a class="change" href="syssetting.aspx"><i class="change fa fa-bars"></i>参数设置</a>
                            </dd>
                        </dl>
                       <dl>
                            <dt>App流量统计<em class="fa fa-cog"></em></dt>
                            <dd>
                                 <a class="change" href="applog.aspx"><i class="change fa fa-bars"></i>App流量统计</a>
                            </dd>
                        </dl>
                     
                    </div>
                        <section class="jiqlist">
                        <dl class="jiqlistTab">
                            <dd class="change ddcolor"><b>商品报表</b></dd>
                        </dl>
                        <ul class="jiqlistseach">
                            <li>
                                 <input name="act_stop_timeks" autocomplete="off" type="text" id="start"   runat="server"  class="input" value="" placeholder=""  />
                            </li>
                             <li>
                                 <input name="act_stop_timeks" autocomplete="off" type="text" id="end"   runat="server"  class="input" value="" placeholder=""  />
                            </li>
                             <li>
                             <select id="mechineList"  class="selectpicker" multiple data-live-search="true" data-live-search-placeholder="请输入搜索" data-actions-box="true" onchange="mechineChg()">
                             </select> 
                            </li>
                           
                            <li>
                                <input type="button" value="查询" class="seachbtn"  onclick="sear()"/>
                            </li>
                            <li>
                               <input type="button" value="导出" class="seachbtn"  onclick="ExportRecord()"/>
                            </li>
                            
                        </ul>
                        <ul class="memberlist" id="ull">
                        </ul>
                          <div class="commonPage">
                       <a class="change" onclick="getPage('first')">首页</a>
                       <a class="change" onclick="getPage('up')">上一页</a>
                       <a class="change" onclick="getPage('down')">下一页</a>
                       <a class="change" onclick="getPage('end')">尾页</a>
                       <select id="pageSel" onchange="pageChg()">
                           
                       </select>
                   </div>
                    </section>
                </div>
            </div>
        </div>
         <input id="pageCurrentCount" runat="server" type="hidden" value="1"/>
         <input id="pageTotalCount" runat="server" type="hidden" value="1"/>
         <input  id="companyID" runat="server" type="hidden"/>
        <input id="agentID" runat="server" type="hidden"/>
         <input id="_mechineList" runat="server" type="hidden"/>
        <input id="_brandList" runat="server" type="hidden"/>
         <input id="_operaID" runat="server" type="hidden" />
    </form>
   
    <script src="../script/jquery-3.2.1.min.js"></script>
    <script>
        function judge() {
            $.ajax({
                type: "post",
                url: "syssetting.aspx/judge",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: "{operaID:'" + $("#_operaID").val() + "',menuID:'gzhsz'}",
                success: function (data) {
                    if (data.d.code == "500") {
                        $(".jiqlist").empty();
                        $(".jiqlist").html("<p class='noquanxian'>当前没有权限</p>").css({ "background": "#ddd" });
                    }
                }
            })
        }
        $(function () {
            judge()
            $("#li8").find("a").addClass("aborder");
            jeDate({
                dateCell: "#start", //isinitVal:true,

                isTime: false, //isClear:false,
                choose: function (val) { },
                minDate: "2014-09-19 00:00:00"
            });
            jeDate({
                dateCell: "#end",
                isinitVal: true,
                isTime: true, //isClear:false,
                minDate: "2014-09-19 00:00:00"
            });
            getMechineList();
            sear();
        });
        function ExportRecord()
        {
            if ($("#mechineList").val()=="") {
                alert("请选择机器");
                return;
            }
            var url = "&start=" + $("#start").val() + "&end=" + $("#end").val() + "&mechineList=" + $("#mechineList").val();
            window.location.href = "../../api/ExportExcel.ashx?action=ExportRecord" + url;
        }
        function getMechineList() {
            $.ajax({
                type: "post",
                url: "applog.aspx/getMechineList",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: "{companyID:'" + $("#companyID").val() + "'}",
                success: function (data) {
                    if (data.d.code == "200") {
                        var serverdata = $.parseJSON(data.d.db);
                        var serverdatalist = serverdata.length;

                        var optionString = "";
                        for (i = 0; i < serverdatalist; i++) {
                            optionString += "<option value=\'" + serverdata[i].id + "\'>" + serverdata[i].mechineName + "</option>";
                        }
                        var myobj = document.getElementById("mechineList");
                        $("#mechineList").html(optionString);
                        $("#mechineList").selectpicker('refresh');

                    } else {
                        //alert(data.d.msg);
                    }
                }
            })
        }
        function getPage(val) {
            if (val == "first") {
                $("#pageCurrentCount").val("1");
            } else if (val == "up") {
                var index = parseInt($("#pageCurrentCount").val()) - 1;

                if (index >= 1) {
                    $("#pageCurrentCount").val(index);
                }

            } else if (val == "down") {
                var index = parseInt($("#pageCurrentCount").val()) + 1;
                if (index <= parseInt($("#pageTotalCount").val())) {
                    $("#pageCurrentCount").val(index);
                }

            } else if (val == "end") {
                $("#pageCurrentCount").val($("#pageTotalCount").val());
            }
            sear();
        }
        function pageChg() {
            $("#pageCurrentCount").val($("#pageSel").val());
            sear();
        }
        function sear() {
            $("#ull").empty();
            $(" <li>"

                       + "<label style='width:10%'>机器名称</label>"
                       + "<label style='width:10%'>机器编号</label>"
                       + "<label style='width:10%'>会员名称/ID</label>"
                       + "<label style='width:5%'>屏幕唤醒</label>"
                       + "<label style='width:5%'>扫码出货</label>"
                       + "<label style='width:5%'>零售支付</label>"
                       + "<label style='width:5%'>支付完成</label>"
                       + "<label style='width:10%'>时间</label>"
                       + "<label style='width:40%'>备注</label>"
                       + "</li>").appendTo("#ull");
            $.ajax({
                type: "post",
                url: "applog.aspx/sear",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: "{mechineID:'" + $("#mechineList").val() + "',start:'" + $("#start").val() + "',end:'" + $("#end").val() + "' ,pageCurrentCount:'" + $("#pageCurrentCount").val() + "'}",
                success: function (data) {
                    if(data.d.code=="200")
                    {
                    var count = data.d.count
                    if (parseInt(count) >= 0) {
                        $("#pageSel").empty();
                        for (var k = 1; k <= parseInt(count) ; k++) {
                            $("<option value='" + k + "'>" + k + "</option>").appendTo("#pageSel");
                        }
                    }
                    var serverdata = $.parseJSON(data.d.db);

                    $("#pageTotalCount").val(count);
                    var serverdatalist = serverdata.length;
                    for (var i = 0; i < serverdatalist; i++) {
                        $(" <li>"
                                 + "   <span style='width:10%'>" + serverdata[i].mechineName + "</span>"
                                 + "   <span style='width:10%'>" + serverdata[i].bh + "</span>"
                                 + "   <span style='width:10%'>" + (serverdata[i].name == "" ? serverdata[i].memberStr : serverdata[i].name) + "</span>"
                                 + "   <span style='width:5%'>" + serverdata[i].indexCount + "</span>"
                                 + "   <span style='width:5%'>" + serverdata[i].smCount + "</span>"
                                 + "   <span style='width:5%'>" + serverdata[i].productCount + "</span>"
                                 + "   <span style='width:5%'>" + serverdata[i].endCount + "</span>"
                                 + "   <span style='width:10%'>" + serverdata[i].timeStr + "</span>"
                                 + "   <span style='width:40%'>" + serverdata[i].productStr + "</span>"
                                + "</li> ").appendTo("#ull");
                    }
                    }
                }
            })
        }
        
    </script>
</body>
</html>

