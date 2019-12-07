<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="tqlist.aspx.cs" Inherits="autosell_center.main.member.tqlist" %>

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
    <link href="../../scripts/skin/jedate.css" rel="stylesheet" type="text/css" />
    <script src="../../scripts/jedate.js" type="text/javascript"></script>
    <script src="../../scripts/jedate.min.js"></script>
    <script type="text/javascript">
        window.onload = function () {
            jeDate({
                dateCell: "#start", //isinitVal:true,
                //format: "YYYY-MM-DD",
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
        }
    </script>
    <style>
        .memberseach ul.tqList {
            width: 100%;
            border-bottom: 1px solid #ddd;
        }

            .memberseach ul.tqList li {
                line-height: 120px;
            }

            .memberseach ul.tqList .labelView {
                width: 360px;
                float: left;
                height: 100%;
                top: 0;
            }

            .memberseach ul.tqList li label {
                height: 120px;
            }

            .memberseach ul.tqList li .seachbtn {
                height: 30px;
                width: 60px;
            }

            .memberseach ul.tqList li input {
                width: 18px;
                height: 18px;
                margin: 51px 0 51px 36px;
                display: inline-block;
                float: initial;
            }

                .memberseach ul.tqList li input#money {
                    width: 80px;
                    height: 24px;
                    float: left;
                    position: relative;
                    background: #f5f5f5;
                    border: 1px solid #ddd;
                    margin: 48px 12px 48px 6px;
                }

            .memberseach ul.tqList li select#byday {
                width: auto;
                height: 24px;
                background: #f5f5f5;
                border: 1px solid #ddd;
                margin: 48px 12px 48px 6px;
            }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="popupbj"></div>
        <div id="addZSAC" class="change zfpopup">
            <h4>添加充值活动</h4>
            <ul>
                <li>
                    <h5>充值金额</h5>
                    <label>
                        <input type="text" value="" placeholder="充值金额" id="payMoney" maxlength="10" />
                    </label>
                </li>
                <li>
                    <h5>天数</h5>
                    <label>
                        <input type="text" value="" placeholder="享受黄金会员天数" id="days" maxlength="8" />
                    </label>
                </li>
                <li>
                    <h5>开始日期</h5>
                    <label>
                      <input name="act_stop_timeks" autocomplete="off" type="text" id="start"   runat="server"  class="input" value="" placeholder="开始日期"  readonly="true"  />
                    </label>
                </li>
                <li>
                    <h5>结束日期</h5>
                    <label>
                       <input name="act_stop_timeks" autocomplete="off" type="text" id="end"   runat="server"  class="input" value="" placeholder="结束日期"  readonly="true"  />
                    </label>
                </li>
                 <li>
                    
                    <label style="border: 0px solid ">
                       时间段内会员只可以享受一次，不设置时间段会员可以无限次享受
                    </label>
                </li>
            </ul>
            <dl>
                <dd>
                    <input type="button" value="确定" class="popupqdbtn" onclick="okActivity()" />
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
                                <a class="change " href="memberdj.aspx"><i class="change  fa fa-file-text"></i>会员等级管理</a>
                            </dd>
                            <dd>
                                <a class="change acolor" href="tqlist.aspx"><i class="change icolor fa fa-file-text"></i>特权管理</a>
                            </dd>
                        </dl>
                    </div>
                    <section class="jiqlist">
                        <div class="memberseach">
                            <h4>预定充值福利设置</h4>
                            <ul class="tqList">
                                <li style="width: 100%; font-size: 15px">
                                    <div class="labelView">
                                        <label>预定购买后+套餐天数内成为黄金会员</label>
                                    </div>

                                    <input type="checkbox" id="ydChk" />
                                </li>
                            </ul>
                            <ul class="tqList">
                                <li style="width: 100%; font-size: 15px">
                                    <div class="labelView">
                                        <label>按充值金额赠送相应黄金会员体验天数</label>
                                    </div>

                                    <input type="checkbox" id="czChk" />
                                    <input type="button" value="增加" class="seachbtn" onclick="addPayhd()    "/>
                                </li>
                            </ul>
                            <ul class="tqList" id="ull">
                             

                            </ul>
                            <ul class="tqList">
                                <li style="width: 100%; font-size: 15px">
                                    <div class="labelView">
                                        <label>不同等级会员享受不同等级会员价</label>
                                    </div>
                                    <input type="checkbox" id="membChk" />
                                </li>
                            </ul>
                              <ul class="tqList">
                                <li style="width: 100%; font-size: 15px">
                                    <div class="labelView">
                                        <label>黄金会员天数体验上限</label>
                                    </div>
                                    <input type="number" id="totalDay" style="width:100px;height:30px;"  min="0"/>
                                </li>
                            </ul>
                            <ul style="margin-bottom: 10px;">
                                <li style="width: 100%; font-size: 15px;">
                                    <input type="button" value="保存" class="seachbtn" onclick="saveInfo()" />
                                </li>
                            </ul>
                        </div>
                    </section>
                </div>
            </div>
        </div>
        <input id="companyID" runat="server" type="hidden" />
        <input id="_operaID" runat="server" type="hidden" />

    </form>
</body>
</html>
<script>
    function initData()
    {
        $.ajax({
            type: "post",
            url: "tqlist.aspx/initData",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{companyID:'" + $("#companyID").val() + "'}",
            success: function (data) {
                var serverdata = $.parseJSON(data.d);
                var serverdatalist = serverdata.length;
                for (var i = 0; i < serverdatalist; i++) {
                    var bz = "长期有效";
                    if (serverdata[i].startTime != "" && serverdata[i].endTime!="")
                    {
                        bz = serverdata[i].startTime + "~" + serverdata[i].endTime;
                    }
                    $("<li style='width: 100%; font-size: 15px'>"
                                  +"  <div class='labelView' style='height:30px;'>"
                                  +"      <label style='float: left'>充值</label>"
                                  + "      <input type='text' id='money' value='" + serverdata[i].money + "' readonly='true'/><label>元，体验</label>"
                                  + "      <span>" + serverdata[i].day + "天黄金会员</span>"
                                  
                                  + "  </div>"
                                   + "     <span>" + bz+ "</span>"
                                  + "  <input type='button' value='删除' class='seachbtn' onclick='delActivity(" + serverdata[i].id + ")'/>"
                                +"</li>").appendTo("#ull");
                }
            }
        })
    }
    function delActivity(id)
    {
        if(confirm("是否确认删除？"))
        {
         $.ajax({
            type: "post",
            url: "tqlist.aspx/delActivity",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{id:'"+id+"'}",
            success: function (data) {
                if (data.d.code == "0") {
                    window.location.reload();
                } else {
                    alert(data.d.msg);
                }
             }
        })
        }
    }
    function okActivity()
    {
        if($("#payMoney").val()=="")
        {
            alert("充值金额不能为空");
            return;
        }
        if ($("#days").val() == "") {
            alert("赠送天数不能为空");
            return;
        }
        $.ajax({
            type: "post",
            url: "tqlist.aspx/okActivity",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{companyID:'" + $("#companyID").val() + "',payMoney:'" + $("#payMoney").val() + "',days:'" + $("#days").val() + "',start:'" + $("#start").val() + "',end:'"+$("#end").val()+"'}",
            success: function (data) {
                if (data.d.code == 0) {
                    window.location.reload();
                } else {
                    alert(data.d.msg);
                }
            }
        })
    }

    function qxClick() {
        $(".popupbj").hide();
        $(".zfpopup").removeClass("zfpopup_on");
    }
    function addPayhd() {
        $('#addZSAC').addClass('zfpopup_on')
        $('.popupbj').fadeIn();
    }
    function saveInfo() {
        $.ajax({
            type: "post",
            url: "tqlist.aspx/saveInfo",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{companyID:'" + $("#companyID").val() + "',yd:" + $("#ydChk").get(0).checked + ",czChk:" + $("#czChk").get(0).checked + ",membChk:" + $("#membChk").get(0).checked + ",totalDay:'" + $("#totalDay").val() + "'}",
            success: function (data) {
                if (data.d.code == 0) {
                    alert(data.d.msg);
                }
            }
        })
    }
    function getInfo() {
        $.ajax({
            type: "post",
            url: "tqlist.aspx/getInfo",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{companyID:'" + $("#companyID").val() + "'}",
            success: function (data) {

                if (data.d.code == 0) {
                    if (data.d.yd == "1") {

                        $("input[type='checkbox']:eq(0)").prop("checked", true);
                    }
                  
                    if (data.d.czswitch == "1") {

                        $("input[type='checkbox']:eq(1)").prop("checked", true);
                    }
                    if (data.d.memberprice == "1") {

                        $("input[type='checkbox']:eq(2)").prop("checked", true);
                    }
                    $("#totalDay").val(data.d.totalDay);

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
        initData();
    })
    function initSelect() {
        for (var i = 1; i <= 30; i++) {
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
            url: "tqlist.aspx/judge",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{operaID:'" + $("#_operaID").val() + "',menuID:'hytqgl'}",
            success: function (data) {
                if (data.d.code == "500") {
                    $(".jiqlist").empty();
                    $(".jiqlist").html("<p class='noquanxian'>当前没有权限</p>").css({ "background": "#ddd" });
                }
            }
        })

    }
</script>
