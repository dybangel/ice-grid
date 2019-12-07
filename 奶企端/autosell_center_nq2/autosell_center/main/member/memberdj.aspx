<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="memberdj.aspx.cs" Inherits="autosell_center.main.member.memberdj" %>

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
                        </dl>
                    </div>
                    <section class="jiqlist">
                        <div class="memberseach">
                            <h4>会员等级管理</h4>
                            <ul>
                                <li style="width:100%;font-size:15px">
                                    <label style="width:20%">普通会员</label>
                                    <label style="width:20%">微商城/小程序登录+绑定手机号</label>
                                     <label style="width:20%">近30天消费</label>
                                    <select id="ptday">
                                    </select>
                                </li>
                            </ul>
                            <ul>
                                <li style="width:100%;font-size:15px">
                                    <label style="width:20%">白银会员</label>
                                    <label style="width:20%">微商城/小程序登录+绑定手机号</label>
                                    <label style="width:20%">近30天消费</label>
                                     <select id="byday">
                                         
                                    </select>
                                </li>
                            </ul>
                            <ul>
                                <li style="width:100%;font-size:15px">
                                    <label style="width:20%">黄金会员</label>
                                    <label style="width:20%">微商城/小程序登录+绑定手机号</label>
                                    <label style="width:20%">近30天消费</label>
                                     <select id="hjday">
                                         
                                    </select>
                                </li>
                            </ul>
                            <ul style="margin-bottom:10px;">
                                <li style="width:100%;font-size:15px;text-align:center;">
                                   <input  type="button" value="保存" class="seachbtn" style="float:initial;" onclick="saveInfo()"/>
                                </li>
                            </ul>
                        </div>
                    </section>
                </div>
            </div>
        </div>
       <input id="companyID" runat="server" type="hidden"/>
         <input id="_operaID" runat="server" type="hidden" />
    </form>
</body>
</html>
<script>
    function saveInfo()
    {
        $.ajax({
            type: "post",
            url: "memberdj.aspx/saveInfo",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{ptday:'" + $("#ptday").val() + "',byday:'" + $("#byday").val() + "',hjday:'" + $("#hjday").val() + "',companyID:'" + $("#companyID").val() + "'}",
            success: function (data) {
                if(data.d.code==0)
                {
                    alert(data.d.msg);
                }
            }
        })
    }
    function getInfo()
    {
        $.ajax({
            type: "post",
            url: "memberdj.aspx/getInfo",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{companyID:'" + $("#companyID").val() + "'}",
            success: function (data) {
               
                if (data.d.code == 0) {
                    $("#ptday").val(data.d.ptday);
                    $("#byday").val(data.d.byday);
                    $("#hjday").val(data.d.hjday);
                }
            }
        })
    }
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
        judge()
        initSelect();
        getInfo();
    })
    function initSelect()
    {
        for (var i = 0; i <= 30;i++)
        {
            $("<option value='" + i + "'>" + i + "</option>").appendTo("#ptday");
            $("<option value='" + i + "'>" + i + "</option>").appendTo("#byday");
            $("<option value='" + i + "'>" + i + "</option>").appendTo("#hjday");
        }
    }
    function getPage(val) {
        if (val == "first") {
            $("#pageCurrentCount").val("1");
        } else if (val == "up") {
            $("#pageCurrentCount").val($("#pageCurrentCount").val() - 1);
        } else if (val == "down") {
            $("#pageCurrentCount").val($("#pageCurrentCount").val() + 1)
        } else if (val == "end") {

        }
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
        getMemberList();
    }
    function pageChg() {
        $("#pageCurrentCount").val($("#pageSel").val());
        getMemberList();
    }
    
    function judge() {
        $.ajax({
            type: "post",
            url: "memberdj.aspx/judge",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{operaID:'" + $("#_operaID").val() + "',menuID:'hydjgl'}",
            success: function (data) {
                if (data.d.code == "500") {
                    $(".jiqlist").empty();
                    $(".jiqlist").html("<p class='noquanxian'>当前没有权限</p>").css({ "background": "#ddd" });
                }
            }
        })

    }
</script>
