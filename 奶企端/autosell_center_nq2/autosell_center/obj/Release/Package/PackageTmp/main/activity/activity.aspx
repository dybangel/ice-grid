<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="activity.aspx.cs" Inherits="autosell_center.main.activity.activity" %>

<%@ Register Src="~/ascx/CheckboxListControl.ascx" TagName="CheckboxListControl" TagPrefix="uc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>活动管理-自动售卖终端中心控制系统</title>
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

            jeDate({
                dateCell: "#Text1", //isinitVal:true,
                //format: "YYYY-MM-DD",
                isTime: false, //isClear:false,
                choose: function (val) { },
                minDate: "2014-09-19 00:00:00"
            });
            jeDate({
                dateCell: "#Text2",
                isinitVal: true,
                isTime: true, //isClear:false,
                minDate: "2014-09-19 00:00:00"
            });

            jeDate({
                dateCell: "#Text3", //isinitVal:true,
                //format: "YYYY-MM-DD",
                isTime: false, //isClear:false,
                choose: function (val) { },
                minDate: "2014-09-19 00:00:00"
            });
            jeDate({
                dateCell: "#Text4",
                isinitVal: true,
                isTime: true, //isClear:false,
                minDate: "2014-09-19 00:00:00"
            });
            jeDate({
                dateCell: "#czStart", //isinitVal:true,
                //format: "YYYY-MM-DD",
                isTime: false, //isClear:false,
                choose: function (val) { },
                minDate: "2014-09-19 00:00:00"
            });
            jeDate({
                dateCell: "#czEnd",
                isinitVal: true,
                isTime: true, //isClear:false,
                minDate: "2014-09-19 00:00:00"
            });
            jeDate({
                dateCell: "#mzstart", //isinitVal:true,
                //format: "YYYY-MM-DD",
                isTime: false, //isClear:false,
                choose: function (val) { },
                minDate: "2014-09-19 00:00:00"
            });
            jeDate({
                dateCell: "#mzend",
                isinitVal: true,
                isTime: true, //isClear:false,
                minDate: "2014-09-19 00:00:00"
            });
            jeDate({
                dateCell: "#updatemzstart", //isinitVal:true,
                //format: "YYYY-MM-DD",
                isTime: false, //isClear:false,
                choose: function (val) { },
                minDate: "2014-09-19 00:00:00"
            });
            jeDate({
                dateCell: "#updatemzend",
                isinitVal: true,
                isTime: true, //isClear:false,
                minDate: "2014-09-19 00:00:00"
            });
        }
    </script>
    <style>
        .productclass {
            margin-top: 30px;
        }

        #num, ._isOnqy {
            display: none;
        }

        ._isOnqy {
            margin-left: 6px;
        }

        #ul2 li label, #ul2 li span {
            width: 16.6%;
        }

        #ul2 li input {
            width: 80px;
            height: 36px;
            border-radius: 6px;
            border: 0;
            background: #3a77d5;
            color: #fff;
            float: right;
            margin: 12px auto;
            cursor: pointer;
        }

        #ull li input {
            width: 80px;
            height: 36px;
            border-radius: 6px;
            border: 0;
            background: #3a77d5;
            color: #fff;
            float: right;
            margin: 12px auto;
            cursor: pointer;
        }
          #ul3 li label, #ul3 li span {
            width: 16.6%;
        }
         #ul3 li input {
            width: 80px;
            height: 36px;
            border-radius: 6px;
            border: 0;
            background: #3a77d5;
            color: #fff;
            float: right;
            margin: 12px auto;
            cursor: pointer;
        }
        .productclass {
            display: none;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="header"></div>
        <div class="main">
             
            <div id="updataCaDiv" class="addDiv change">
                <h4>活动设置</h4>
                <ul>
                    <li>
                        <label>活动名称</label>
                        <input type="text" id="updatename" />
                    </li>
                    <li>
                        <label>活动标签</label>
                        <input type="text" id="updatetag" />
                    </li>
                    <li id="zhekou">
                        <label>周期</label>
                        <select id="updateselCycle">
                            <option value="1">1</option>
                            <option value="5">5</option>
                            <option value="7">7</option>
                            <option value="10">10</option>
                            <option value="20">20</option>
                            <option value="30">30</option>
                            <option value="60">60</option>
                            <option value="90">90</option>
                            <option value="180">180</option>
                            <option value="365">365</option>
                        </select>
                    </li>
                    <li>
                        <label>活动方式</label>
                        <select onchange="chkMode()" id="selModeupdate">
                            <option value="1">打折</option>
                            <option value="2">送天数</option>
                            <option value="3">送实物</option>
                        </select>
                    </li>
                    <li>
                        <label>折扣/天数</label>
                        <input type="text" value="" id="updatedisOrDay" />
                    </li>
                    <li>
                        <label>有效期开始</label>
                       
                            <input name="act_stop_timeks" type="text" id="Text1" runat="server"  value="" placeholder="有效期开始" readonly="true" />
                        
                    </li>
                    <li>
                        <label>有效期结束</label>
                       
                            <input name="act_stop_timeks" type="text" id="Text2" runat="server"  value="" placeholder="有效期结束" readonly="true" />
                    </li>
                    <li>
                        <label></label>
                        <input type="button" value="确认修改" onclick="btnUpdate()" class="btnok" />
                        <input type="button" value="取消" class="btnoff" onclick="divOff()" />
                    </li>
                </ul>
            </div>
            <div class="popupbj"></div>
            <div class="main_list">
                <div class="common_title">
                    <h4>
                        <i class="fa fa-plus"></i>
                        <span>活动设置</span>
                    </h4>
                </div>
                <div class="common_main">
                    <div class="navlist">
                        <dl>
                            <dt>活动设置<em class="fa fa-cog"></em></dt>
                            <dd>
                                <a class="change " href="activity.aspx"><i class="change  fa fa-video-camera"></i>设置活动</a>
                            </dd>
                            <dd>
                                <a class="change " href="activitylist.aspx"><i class="change  fa fa-video-camera"></i>活动记录</a>
                            </dd>
                        </dl>
                    </div>
                    <section class="jiqlist">
                        <div id="adminpopup" class="change zfpopup">
                            <h4>添加充值活动</h4>
                            <ul>
                                <li>
                                    <h5>活动名称</h5>
                                    <label>
                                        <input type="text" value="" placeholder="设置活动名称" id="name" />
                                    </label>
                                </li>
                                <li>
                                    <h5>活动标签</h5>
                                    <label>
                                        <input type="text" value="" placeholder="设置活动标签" id="tag" />
                                    </label>
                                </li>
                                <li>
                                    <h5>优惠方式</h5>
                                    <label>
                                        <select id="paymode">
                                            <option value="1">送金额</option>
                                            <option value="2">送实物</option>
                                        </select>
                                    </label>
                                </li>
                                <li>
                                    <h5>充值金额</h5>
                                    <label>
                                        <input type="number" value="" id="czMoney" />
                                    </label>
                                </li>
                                <li>
                                    <h5>到账金额</h5>
                                    <label>
                                        <input type="number" value="" id="dzMoney" />
                                    </label>
                                </li>
                                <li>
                                    <h5>有效期开始</h5>
                                    <label>
                                        <input name="act_stop_timeks" type="text" id="Text3" runat="server" class="input" value="" placeholder="有效期开始" readonly="true" />
                                    </label>
                                </li>
                                <li>
                                    <h5>有效期结束</h5>
                                    <label>
                                        <input name="act_stop_timeks" type="text" id="Text4" runat="server" class="input" value="" placeholder="有效期结束" readonly="true" />
                                    </label>
                                </li>
                            </ul>
                            <dl>
                                <dd>
                                    <input type="button" value="确定" class="popupqdbtn" onclick="okCZ()" />
                                </dd>
                                <dd>
                                    <input type="button" value="取消" onclick="qxClick()" />
                                </dd>
                            </dl>
                        </div>


                            <div id="adminCZ" class="change zfpopup">
                            <h4>修改充值活动</h4>
                            <ul>
                                <li>
                                    <h5>活动名称</h5>
                                    <label>
                                        <input type="text" value="" placeholder="设置活动名称" id="czname" />
                                    </label>
                                </li>
                                <li>
                                    <h5>活动标签</h5>
                                    <label>
                                        <input type="text" value="" placeholder="设置活动标签" id="cztag" />
                                    </label>
                                </li>
                                <li>
                                    <h5>优惠方式</h5>
                                    <label>
                                        <select id="czpaymode">
                                            <option value="1">送金额</option>
                                            <option value="2">送实物</option>
                                        </select>
                                    </label>
                                </li>
                                <li>
                                    <h5>充值金额</h5>
                                    <label>
                                        <input type="number" value="" id="czczMoney" />
                                    </label>
                                </li>
                                <li>
                                    <h5>到账金额</h5>
                                    <label>
                                        <input type="number" value="" id="czdzMoney" />
                                    </label>
                                </li>
                                <li>
                                    <h5>有效期开始</h5>
                                    <label>
                                        <input name="act_stop_timeks" type="text" id="czStart" runat="server" class="input" value="" placeholder="有效期开始" readonly="true" />
                                    </label>
                                </li>
                                <li>
                                    <h5>有效期结束</h5>
                                    <label>
                                        <input name="act_stop_timeks" type="text" id="czEnd" runat="server" class="input" value="" placeholder="有效期结束" readonly="true" />
                                    </label>
                                </li>
                            </ul>
                            <dl>
                                <dd>
                                    <input type="button" value="确定" class="popupqdbtn" onclick="updatePayActivity()" />
                                </dd>
                                <dd>
                                    <input type="button" value="取消" onclick="qxClick()" />
                                </dd>
                            </dl>
                        </div>


                        <div id="addZSAC" class="change zfpopup">
                            <h4>添加赠送活动</h4>
                            <ul>
                                <li>
                                    <h5>活动名称</h5>
                                    <label>
                                        <input type="text" value="" placeholder="设置活动名称" id="activityName" maxlength="20" />
                                    </label>
                                </li>
                                <li>
                                    <h5>活动标签</h5>
                                    <label>
                                        <input type="text" value="" placeholder="显示在周期底部 不能过长" id="activityTag" maxlength="8" />
                                    </label>
                                </li>
                                <li>
                                    <h5>周期</h5>
                                    <label>
                                        <select id="selCycle">
                                            <option value="1">1</option>
                                            <option value="5">5</option>
                                            <option value="7">7</option>
                                            <option value="10">10</option>
                                            <option value="20">20</option>
                                            <option value="30">30</option>
                                            <option value="60">60</option>
                                            <option value="90">90</option>
                                            <option value="180">180</option>
                                            <option value="365">365</option>
                                        </select>
                                    </label>
                                </li>
                                <li>
                                    <h5>活动方式</h5>
                                    <label>
                                        <select onchange="chkMode()" id="selMode">
                                            <option value="1">打折</option>
                                            <option value="2">送天数</option>
                                            <option value="3">送实物</option>
                                        </select>
                                    </label>
                                </li>
                                <li>
                                    <h5>折扣/天数</h5>
                                    <label>
                                        <input type="text" value="" id="disOrDay" />
                                    </label>
                                </li>
                                <li>
                                    <h5>有效期开始</h5>
                                    <label>
                                        <input name="act_stop_timeks" autocomplete="off" type="text" id="start" runat="server" class="input" value="" placeholder="有效期开始" readonly="true" />
                                    </label>
                                </li>
                                <li>
                                    <h5>有效期结束</h5>
                                    <label>
                                        <input name="act_stop_timeks" autocomplete="off" type="text" id="end" runat="server" class="input" value="" placeholder="有效期结束" readonly="true" />
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
                        <div id="addMZAC" class="change zfpopup">
                            <h4>添加满折活动</h4>
                            <ul>
                                <li>
                                    <h5>活动名称</h5>
                                    <label>
                                        <input type="text" value="" placeholder="设置活动名称" id="activityMZName" maxlength="20" />
                                    </label>
                                </li>
                                <li>
                                    <h5>活动标签</h5>
                                    <label>
                                        <input type="text" value="" placeholder="显示在周期底部 不能过长" id="activityMZTag" maxlength="8" />
                                    </label>
                                </li>
                                <li>
                                    <h5>购买数量</h5>
                                    <label>
                                        <input type="text" value="" placeholder="达到A份开始打折" id="fullnum" maxlength="8" />
                                    </label>
                                </li>
                                <li>
                                    <h5>打折设置</h5>
                                    <label>
                                        <input type="text" value="" placeholder="打折幅度" id="discountcontent" maxlength="8" />
                                    </label>
                                </li>
                               
                                <li>
                                    <h5>有效期开始</h5>
                                    <label>
                                        <input name="act_stop_timeks" autocomplete="off" type="text" id="mzstart" runat="server" class="input" value="" placeholder="有效期开始" readonly="true" />
                                    </label>
                                </li>
                                <li>
                                    <h5>有效期结束</h5>
                                    <label>
                                        <input name="act_stop_timeks" autocomplete="off" type="text" id="mzend" runat="server" class="input" value="" placeholder="有效期结束" readonly="true" />
                                    </label>
                                </li>
                            </ul>
                            <dl>
                                <dd>
                                    <input type="button" value="确定" class="popupqdbtn" onclick="okMZActivity()" />
                                </dd>
                                <dd>
                                    <input type="button" value="取消" onclick="qxClick()" />
                                </dd>
                            </dl>
                        </div>
                        <div id="updataMZDiv"  class="change zfpopup">
                            <h4>活动设置</h4>
                            <ul>
                                <li>
                                     <h5>活动名称</h5>
                                    <label><input type="text" id="updatemzname" /></label>
                                    
                                </li>
                                <li>
                                     <h5>活动标签</h5>
                                    <label> <input type="text" id="updatemztag" /></label>
                                   
                                </li>
                                <li >
                                     <h5>购买数量</h5>
                                    <label><input type="text" id="updatefullnum" /></label>
                                     
                                </li>
                                <li>
                                     <h5>打折</h5>
                                    <label><input type="text" id="updatediscountcontent" /></label>
                                    
                                </li>
                               
                                <li>
                                     <h5>有效期开始</h5>
                                    <label> <input name="act_stop_timeks" type="text" id="updatemzstart" runat="server"  value="" placeholder="有效期开始" readonly="true" /></label>
                       
                                       
                        
                                </li>
                                <li>
                                     <h5>有效期结束</h5>
                                    <label> <input name="act_stop_timeks" type="text" id="updatemzend" runat="server"  value="" placeholder="有效期结束" readonly="true" /></label>
                       
                                       
                                </li>
                               
                            </ul>
                             <dl>
                                <dd>
                                    <input type="button" value="确认修改" class="popupqdbtn" onclick="btnMZUpdate()" />
                                </dd>
                                <dd>
                                    <input type="button" value="取消" onclick="qxClick()" />
                                </dd>
                            </dl>
                       </div>
                        <dl class="jiqlistTab">
                            <dd class="change ddcolor"><b>赠送活动</b></dd>
                            <dd class="change"><b>充值活动</b></dd>
                            <dd class="change"><b>满折活动</b></dd>
                        </dl>
                        <ul class="productclass" id="ull" style="display: block;">
                        </ul>
                        <ul class="productclass" id="ul2" style="border: 0">
                        </ul>
                        <ul class="productclass" id="ul3" style="border: 0">
                        </ul>
                    </section>
                </div>
            </div>
        </div>
        <input type="hidden" runat="server" id="companyId" />
        <input type="hidden" runat="server" id="activityID" />
        <input type="hidden" runat="server" id="payActivityID" />
         <input type="hidden" runat="server" id="fulldiscountID" />
         <input id="_operaID" runat="server" type="hidden" />
    </form>
</body>
</html>
<script>
    function judge() {
        $.ajax({
            type: "post",
            url: "activity.aspx/judge",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{operaID:'" + $("#_operaID").val() + "',menuID:'szhd'}",
            success: function (data) {
                if (data.d.code == "500") {
                    $(".jiqlist").empty();
                    $(".jiqlist").html("<p class='noquanxian'>当前没有权限</p>").css({ "background": "#ddd" });
                }
            }
        })
    }
    function updatePay(id, payName, tag, type, czMoney, dzMoney, startTime, endTime) {
        $("#payActivityID").val(id);
        $("#czname").val(payName);
        $("#cztag").val(tag);
        $("#czpaymode").val(type);
        $("#czczMoney").val(czMoney);
        $("#czdzMoney").val(dzMoney);
        $("#czStart").val(startTime);
        $("#czEnd").val(endTime);
        $('#adminCZ').addClass('zfpopup_on')
        $('.popupbj').fadeIn();
    }
    function updatePayActivity(){
        $.ajax({
            type: "post",
            url: "activity.aspx/updatePayActivity",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{id:'" + $("#payActivityID").val() + "',name:'" + $("#czname").val() + "',tag:'" + $("#cztag").val() + "',payType:'" + $("#czpaymode").val() + "',czMoney:'" + $("#czczMoney").val() + "',czdzMoney:'" + $("#czdzMoney").val() + "',czStart:'" + $("#czStart").val() + "',czEnd:'" + $("#czEnd").val() + "'}",
            success: function (data) {
                if (data.d.code == 200) {
                    window.location.reload()
                } else {
                    alert(data.d.msg);
                }
            }
        })
    }
    function chkMode() {
        var val = $("#selMode").val();
        if (val == "3") {
            $("#disOrDay").attr("readonly", true);
            $("#disOrDay").val(0);
        } else {
            $("#disOrDay").attr("readonly", false);
        }
    }
        function okActivity() {
            //保存
            $.ajax({
                type: "post",
                url: "activity.aspx/okActivity",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: "{companyID:'" + $("#companyId").val() + "',activityName:'" + $("#activityName").val() + "',activityTag:'" + $("#activityTag").val() + "',cycle:'" + $("#selCycle").val() + "',mode:'" + $("#selMode").val() + "',num:'" + $("#disOrDay").val() + "',start:'"+$("#start").val()+"',end:'"+$("#end").val()+"'}",
                success: function (data) {
                    if (data.d.code == 0) {
                        window.location.reload()
                    } else {
                        alert(data.d.msg);
                    }
                }
            })
        }
        function okMZActivity() {
            //保存
            $.ajax({
                type: "post",
                url: "activity.aspx/okMZActivity",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: "{companyID:'" + $("#companyId").val() + "',activityName:'" + $("#activityMZName").val() + "',activityTag:'" + $("#activityMZTag").val() + "',fullnum:'" + $("#fullnum").val() + "',discountcontent:'" + $("#discountcontent").val() + "',mzstart:'" + $("#mzstart").val() + "',mzend:'" + $("#mzend").val() + "'}",
                success: function (data) {
                    if (data.d.code == 0) {
                        window.location.reload()
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
            $('#adminpopup').addClass('zfpopup_on')
            $('.popupbj').fadeIn();
        }
        $(function () {
            judge();
            $('.jiqlistTab').find("dd").click(function () {
                $('.jiqlistTab').find("dd").removeClass('ddcolor');
                $(this).addClass('ddcolor')
                var thisNum = $(this).index();
                $('.productclass').eq(thisNum).show().siblings('.productclass').hide();
            })

            $("#li6").find("a").addClass("aborder");
            $("#xuanzHuo").on('change', function () {
                if ($(this).val() == "1") {
                    $("#zhekou").show();
                    $("#num").hide();
                } else {
                    $("#num").show();
                    $("#zhekou").hide();
                }
            })

            jQuery("#Num").keyup(function () {
                var value = jQuery(this).val();
                if ((/^(\+|-)?\d+$/.test(value)) && value > 0) {
                    return true;
                } else if (value != "") {
                    alert("赠送数量必须为正整数");
                    jQuery("#Num").val("0");
                    return false;
                }
            });

            sear();
            getPayList();
            getFullDiscountList();
        });
        function sear() {
            $("#ull").empty();
            $("<li>"
                     + "  <input type='button' value='添加赠送活动' id='addZS' onclick='addZSActivity()' />"
                + "</li>"
                + "<li>"
                 + "<label style='width:12.5%'>活动名称</label>"
                 + "<label style='width:12.5%'>活动标签</label>"
                + "<label style='width:8%'>周期</label>"
                + "<label style='width:12.5%'>优惠方式</label>"
                + "<label style='width:12.5%'>是否开启活动</label>"
                 + "<label style='width:12.5%'>开始时间</label>"
                  + "<label style='width:12.5%'>结束时间</label>"
                + "<label style='width:17%'>操作</label>"
                + "</li>").appendTo("#ull");
            $.ajax({
                type: "post",
                url: "activity.aspx/sear",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: "{companyID:'" + $("#companyId").val() + "'}",
                success: function (data) {
                    var serverdata = $.parseJSON(data.d);
                    var serverdatalist = serverdata.length;
                    for (var i = 0; i < serverdatalist; i++) {
                        var str = "";
                        var zt = "";
                        var ss = "";
                        var sf = "";
                        if (serverdata[i].statu == "0") {
                            str = "暂无";
                            zt = "启用"
                            sf = "否";
                        } else {
                            zt = "不启用";
                            sf = "是";
                            if (serverdata[i].type == "1") {
                                str = "打" + serverdata[i].num + "折";
                            } else if (serverdata[i].type == "2") {
                                str = "赠送" + serverdata[i].num + "天";
                            } else if (serverdata[i].type == "3") {
                                str = "赠送实物";
                            }
                        }
                        $(" <li>"
                             + "<span style='width:12.5%'>" + serverdata[i].activityname + "</span>"
                             + "<span style='width:12.5%'>" + serverdata[i].activitytag + "</span>"
                             + "<span style='width:8%'>" + serverdata[i].zq + "</span>"
                             + "<span style='width:12.5%'>" + str + "</span>"
                             + "<span class='sfqiyong' style='width:12.5%'>" + sf + "</span>"
                             + "<span class='sfqiyong' style='width:12.5%'>" + serverdata[i].startTime + "</span>"
                                  + "<span class='sfqiyong' style='width:12.5%'>" + serverdata[i].endTime + "</span>"
                                + "<span style='width:17%'>"
                                + "<a onclick='_isOnOff(this," + serverdata[i].id + ")'>" + zt + "|</a>"
                                 + "<a  onclick='delActivity(" + serverdata[i].id + ")'>删除|</a>"
                                + "<a  onclick='updeted(" + serverdata[i].id + ")'>修改|</a>"
                                 + "<a  onclick='openW(" + serverdata[i].id + ")'>设置商品</a>"
                                + "</span>"
                                + "<input type='hidden' value='22' />"
                                + "</li>").appendTo("#ull");

                    }

                    var sfqiyong = $("#ull").find("li").find(".sfqiyong");
                    sfqiyong.each(function () {
                        if ($(this).html() == "是") {
                            $(this).parent().find("._isOnqy").show();
                        } else {
                            $(this).parent().find("._isOnqy").hide();
                        }
                    })


                }
            })
        }
        function getFullDiscountList() {
            $("#ul3").empty();
            $("<li>"
                     + "  <input type='button' value='添加满折活动' id='addmz' onclick='addMZActivity()' />"
                + "</li>"
                + "<li>"
                 + "<label style='width:12.5%'>活动名称</label>"
                 + "<label style='width:12.5%'>活动标签</label>"
                + "<label style='width:8%'>购买件数</label>"
                + "<label style='width:12.5%'>折扣</label>"
                + "<label style='width:12.5%'>是否开启活动</label>"
                 + "<label style='width:12.5%'>开始时间</label>"
                  + "<label style='width:12.5%'>结束时间</label>"
                + "<label style='width:17%'>操作</label>"
                + "</li>").appendTo("#ul3");
            $.ajax({
                type: "post",
                url: "activity.aspx/getFullDiscountList",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: "{companyID:'" + $("#companyId").val() + "'}",
                success: function (data) {
                    var serverdata = $.parseJSON(data.d);
                    var serverdatalist = serverdata.length;
                    for (var i = 0; i < serverdatalist; i++) {
                        var zt = "";
                        var ss = "";
                        var sf = "";
                        if (serverdata[i].statu == "0") {
                            zt = "启用"
                            sf = "否";
                        } else {
                            zt = "不启用";
                            sf = "是";
                            
                        }
                        $(" <li>"
                             + "<span style='width:12.5%'>" + serverdata[i].activityname + "</span>"
                             + "<span style='width:12.5%'>" + serverdata[i].activitytag + "</span>"
                             + "<span style='width:8%'>" + serverdata[i].fullnum + "</span>"
                             + "<span style='width:12.5%'>" + serverdata[i].discountcontent + "</span>"
                             + "<span class='sfqiyong' style='width:12.5%'>" + sf + "</span>"
                             + "<span class='sfqiyong' style='width:12.5%'>" + serverdata[i].startTime + "</span>"
                                  + "<span class='sfqiyong' style='width:12.5%'>" + serverdata[i].endTime + "</span>"
                                + "<span style='width:17%'>"
                                + "<a onclick='_isOnOffMZ(this," + serverdata[i].id + ")'>" + zt + "|</a>"
                                 + "<a  onclick='delMZActivity(" + serverdata[i].id + ")'>删除|</a>"
                                + "<a  onclick='fullupdeted(" + serverdata[i].id + ")'>修改|</a>"
                                 + "<a  onclick='openFullW(" + serverdata[i].id + ")'>设置商品</a>"
                                + "</span>"
                                + "<input type='hidden' value='22' />"
                                + "</li>").appendTo("#ul3");

                    }

                    var sfqiyong = $("#ul3").find("li").find(".sfqiyong");
                    sfqiyong.each(function () {
                        if ($(this).html() == "是") {
                            $(this).parent().find("._isOnqy").show();
                        } else {
                            $(this).parent().find("._isOnqy").hide();
                        }
                    })


                }
            })
        }
        function delActivity(id)
        {
            $.ajax({
                type: "post",
                url: "activity.aspx/delActivity",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: "{id:'" + id + "'}",
                success: function (data) {
                    if (data.d.code == 200) {
                        window.location.reload();
                    } else {
                        alert(data.d.msg);
                    }
                }
            })
        }
        //删除满折活动
        function delMZActivity(id) {
            $.ajax({
                type: "post",
                url: "activity.aspx/delMZActivity",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: "{id:'" + id + "'}",
                success: function (data) {
                    if (data.d.code == 200) {
                        window.location.reload();
                    } else {
                        alert(data.d.msg);
                    }
                }
            })
        }
        
        function addZSActivity() {
            $('#addZSAC').addClass('zfpopup_on')
            $('.popupbj').fadeIn();
        }
        function addMZActivity() {
            $('#addMZAC').addClass('zfpopup_on')
            $('.popupbj').fadeIn();
        }
        function openW(id) {
            var openUrl = "setActivity.aspx?activityID=" + id;
            var iWidth = 450; //弹出窗口的宽度;
            var iHeight = 600; //弹出窗口的高度;
            var iTop = (window.screen.availHeight - 30 - iHeight) / 2; //获得窗口的垂直位置;
            var iLeft = (window.screen.availWidth - 10 - iWidth) / 2; //获得窗口的水平位置;

            window.open(openUrl, "_blank", "height=" + iHeight + ", width=" + iWidth + ", top=" + iTop + ", left=" + iLeft + ",location=no,status=no,scrollbars=no");
        }
        //满折设置产品
        function openFullW(id) {
            var openUrl = "setFullDiscountActivity.aspx?activityID=" + id;
            var iWidth = 450; //弹出窗口的宽度;
            var iHeight = 600; //弹出窗口的高度;
            var iTop = (window.screen.availHeight - 30 - iHeight) / 2; //获得窗口的垂直位置;
            var iLeft = (window.screen.availWidth - 10 - iWidth) / 2; //获得窗口的水平位置;

            window.open(openUrl, "_blank", "height=" + iHeight + ", width=" + iWidth + ", top=" + iTop + ", left=" + iLeft + ",location=no,status=no,scrollbars=no");
        }

        function btnUpdate() {

            $.ajax({
                type: "post",
                url: "activity.aspx/btnUpdate",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: "{id:'" + $("#activityID").val() + "',name:'" + $("#updatename").val() + "',tag:'" + $("#updatetag").val() + "',zq:'" + $("#updateselCycle").val() + "',mode:'" + $("#selModeupdate").val() + "',disOrday:'" + $("#updatedisOrDay").val() + "',companyID:'" + $("#companyId").val() + "',start:'" + $("#Text1").val() + "',end:'" + $("#Text2").val() + "'}",
                success: function (data) {
                    if (data.d.code == 0) {
                        window.location.reload();
                    } else {
                        alert(data.d.msg);
                    }
                }
            })
        }
        //满折
        function btnMZUpdate() {

            $.ajax({
                type: "post",
                url: "activity.aspx/btnMZUpdate",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: "{id:'" + $("#fulldiscountID").val() + "',name:'" + $("#updatemzname").val() + "',tag:'" + $("#updatemztag").val() + "',fullnum:'" + $("#updatefullnum").val() + "',discountcontent:'" + $("#updatediscountcontent").val() + "',companyID:'" + $("#companyId").val() + "',start:'" + $("#updatemzstart").val() + "',end:'" + $("#updatemzend").val() + "'}",
                success: function (data) {
                    if (data.d.code == 0) {
                        window.location.reload();
                    } else {
                        alert(data.d.msg);
                    }
                }
            })
        }
        function updeted(id) {
            $("#updataCaDiv").addClass("addDivshow");
            setTimeout(function () {
                $(".popupbj").fadeIn();
            }, 100);
            $("#activityID").val(id);
            $.ajax({
                type: "post",
                url: "activity.aspx/Lookup",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: "{id:'" + id + "'}",
                success: function (data) {
                    var serverdata = $.parseJSON(data.d);
                    var serverdatalist = serverdata.length;
                    if (serverdatalist > 0) {
                        $("#updatename").val(serverdata[0].activityname);
                        $("#updatetag").val(serverdata[0].activitytag);
                        $("#updateselCycle").val(serverdata[0].zq);
                        $("#selModeupdate").val(serverdata[0].type);
                        $("#updatedisOrDay").val(serverdata[0].num);
                        $("#Text1").val(serverdata[0].startTime);
                        $("#Text2").val(serverdata[0].endTime);


                    }
                }

            })
        }
        //满折
        function fullupdeted(id) {
            $('#updataMZDiv').addClass('zfpopup_on')
            setTimeout(function () {
                $(".popupbj").fadeIn();
            }, 100);
            $("#fulldiscountID").val(id);
            $.ajax({
                type: "post",
                url: "activity.aspx/FullLookup",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: "{id:'" + id + "'}",
                success: function (data) {
                    var serverdata = $.parseJSON(data.d);
                    var serverdatalist = serverdata.length;
                    if (serverdatalist > 0) {
                        $("#updatemzname").val(serverdata[0].activityname);
                        $("#updatemztag").val(serverdata[0].activitytag);
                        $("#updatefullnum").val(serverdata[0].fullnum);
                        $("#updatediscountcontent").val(serverdata[0].discountcontent);
                        $("#updatemzstart").val(serverdata[0].startTime);
                        $("#updatemzend").val(serverdata[0].endTime);


                    }
                }

            })
        }
        function divOff() {
            $(".popupbj").hide();
            $(".addDiv").removeClass("addDivshow");
        }

        function _isOnOff(obj, id) {
            var $title = $.trim($(obj).html());
            if ($title == "启用") {
                $(obj).parent().find("a").eq(1).show();
                $(obj).html("不启用")
            } else if ($(obj).html() == "不启用") {
                $(obj).parent().find("a").eq(1).hide();
                $(obj).html("启用 ")
            }
            $.ajax({
                type: "post",
                url: "activity.aspx/qy",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: "{id:'" + id + "'}",
                success: function (data) {
                    if (data.d == "1") {
                        alert("保存成功");
                    } else if (data.d == "2") {
                        alert("保存失败");

                    }
                    location.reload();
                }

            })
        }
        //满折启用关闭
        function _isOnOffMZ(obj, id) {
            var $title = $.trim($(obj).html());
            if ($title == "启用") {
                $(obj).parent().find("a").eq(1).show();
                $(obj).html("不启用")
            } else if ($(obj).html() == "不启用") {
                $(obj).parent().find("a").eq(1).hide();
                $(obj).html("启用 ")
            }
            $.ajax({
                type: "post",
                url: "activity.aspx/qyMZ",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: "{id:'" + id + "'}",
                success: function (data) {
                    if (data.d == "1") {
                        alert("保存成功");
                    } else if (data.d == "2") {
                        alert("保存失败");

                    }
                    location.reload();
                }

            })
        }
        
        function getPayList() {
            $("#ul2").empty();
            $(" <li>"
                     + "  <input type='button' value='添加活动' id='addMoneyhd' onclick='addPayhd()' />"
                + "</li>"
                + " <li>"
                 + "  <label style='width:12.5%'>活动名称</label>"
                 + " <label style='width:12.5%'>活动标签</label>"
                 + " <label style='width:12.5%'>优惠方式</label>"
                 + "  <label style='width:12.5%'>充值金额</label>"
                 + "  <label style='width:12.5%'>到账金额</label>"
                  + "  <label style='width:12.5%'>开始时间</label>"
                   + "  <label style='width:12.5%'>结束时间</label>"
                 + "  <label style='width:12.5%'>操作</label>"
            + " </li>").appendTo("#ul2");
            $.ajax({
                type: "post",
                url: "activity.aspx/getPayList",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: "{companyID:'" + $("#companyId").val() + "'}",
                success: function (data) {
                    var serverdata = $.parseJSON(data.d);
                    var serverdatalist = serverdata.length;
                    for (var i = 0; i < serverdatalist; i++) {

                        var status = serverdata[i].status;
                        var zt;
                        if (status == "0") {
                            zt = "开启";
                        } else if (status == "1") {
                            zt = "关闭";
                        }
                        var type;
                        if (serverdata[i].type == "1") {
                            type = "赠送金额";
                        } else if (serverdata[i].type == "2") {
                            type = "赠送实物";
                        }
                        $(" <li>"
                                + "    <span style='width:12.5%'>" + serverdata[i].payName + "</span>"
                                + "    <span style='width:12.5%'>" + serverdata[i].tag + "</span>"
                                 + "    <span style='width:12.5%'>" + type + "</span>"
                                  + "    <span style='width:12.5%'>" + serverdata[i].czMoney + "</span>"
                                + "    <span style='width:12.5%'>" + serverdata[i].dzMoney + "</span>"
                                 + "    <span style='width:12.5%'>" + serverdata[i].startTime + "</span>"
                                  + "    <span style='width:12.5%'>" + serverdata[i].endTime + "</span>"
                                + "    <span style='width:12.5%'>"
                                + "        <a onclick='delPay(" + serverdata[i].id + ")'>删除</a>"
                                 + "        <a onclick='updatePay(\"" + serverdata[i].id + "\",\"" + serverdata[i].payName + "\",\"" + serverdata[i].tag + "\",\"" + serverdata[i].type + "\",\"" + serverdata[i].czMoney + "\",\"" + serverdata[i].dzMoney + "\",\"" + serverdata[i].startTime + "\",\"" + serverdata[i].endTime + "\")'>修改</a>"
                                 + "        <a onclick='setStatus(" + serverdata[i].id + ")'>" + zt + "</a>"
                                + "    </span>"
                                + "</li>").appendTo("#ul2");
                    }
                }

            })
        }
    
        function setStatus(id) {
            $.ajax({
                type: "post",
                url: "activity.aspx/setStatus",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: "{id:'" + id + "'}",
                success: function (data) {
                    if (data.d.code == "0") {
                        window.location.reload();
                    } else {
                        alert(data.d.msg);
                    }
                }

            })
        }

        function okCZ() {
            var reg = /^(0|[1-9][0-9]*)$/;
            if (!reg.test($("#czMoney").val())) {
                alert("输入的不是合法数字");
                return false;
            }
            if ($("#name").val() == "") {
                alert("名称不能为空");
                return false;
            }
            if (!reg.test($("#dzMoney").val())) {
                alert("输入的不是合法数字");
                return false;
            }
            $.ajax({
                type: "post",
                url: "activity.aspx/okCZ",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: "{name:'" + $("#name").val() + "',czMoney:'" + $("#czMoney").val() + "',dzMoney:'" + $("#dzMoney").val() + "',companyID:'" + $("#companyId").val() + "',mechineID:'" + $("#cbosDeparentment_hdscbo").val() + "',paymode:'" + $("#paymode").val() + "',tag:'" + $("#tag").val() + "',start:'"+$("#Text3").val()+"',end:'"+$("#Text4").val()+"'}",
                success: function (data) {
                    if (data.d == "1") {
                        alert("添加成功");
                    } else if (data.d == "2") {
                        alert("添加失败");

                    }
                    location.reload();
                }

            })
        }
        function delPay(id) {
            if (confirm("确定需要删除吗")) {
                $.ajax({
                    type: "post",
                    url: "activity.aspx/del",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: "{id:'" + id + "'}",
                    success: function (data) {
                        if (data.d == "1") {
                            alert("删除成功");
                        } else if (data.d == "2") {
                            alert("删除失败");

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
                        if (menuID == 'hylb') {//会员列表
                            location.href = "../member/memberlist.aspx";
                        }
                        if (menuID == 'jqddtj') {//订单管理
                            //location.href = "../order/orderform.aspx";
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
