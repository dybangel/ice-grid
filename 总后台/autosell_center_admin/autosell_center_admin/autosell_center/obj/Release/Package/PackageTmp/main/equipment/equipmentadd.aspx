<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="equipmentadd.aspx.cs" Inherits="autosell_center.main.equipment.equipmentadd" %>
 
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
    <link  href="../../scripts/skin/jedate.css" rel="stylesheet" type="text/css"/>
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
            })
            jeDate({
                dateCell: "#end",
                isinitVal: true,
                isTime: true, //isClear:false,
                minDate: "2014-09-19 00:00:00"
            })

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
                    <!--<a class="change" href="SellCenter.aspx">
                     <i class="fa fa-reorder"></i>
                         切换奶企
                    </a>-->
                </div>
                <div class="common_main">
                    <div class="navlist">
                        <dl>
                            <dt>设备管理<em class="fa fa-cog"></em></dt>
                            <dd>
                                <a class="change" href="#" onclick="qx_judge('qbsb')"><i class="change fa fa-inbox"></i>全部设备</a>
                            </dd>
                            <dd>
                                <a class="change acolor" href="#" onclick="qx_judge('tjsb')"><i class="change icolor fa fa-plus-square"></i>添加设备</a>
                            </dd>
                        </dl>
                        <dl>
                            <dt>设备类别<em class="fa fa-cog"></em></dt>
                            <dd>
                                <a class="change" href="#" onclick="qx_judge('sblb')"><i class="change fa fa-server"></i>设备类别</a>
                            </dd>
                        </dl>
                    </div>
                    <div class="jiqadd">
                        <section class="addnaiq">
                            <h4 class="commonfut"><a class="change" href="Allequipment.aspx"><i class="change fa fa-angle-left"></i>设备列表</a>/添加设备</h4>
                            <ul class="addnaiqdata" id="addnaiqdata1" style="display: block;">
                                <li>
                                    <label>设备有效期</label>
                                    <div>
                                        <input name="act_stop_timeks" type="text" id="start"   runat="server"  class="input" value="" placeholder="有效期截止时间"  readonly="true"  />
                                        <p>机器的有效时间</p>
                                    </div>
                                </li>
                                <li>
                                    <label>设备编号</label>
                                    <div>
                                        <input type="text" value="" placeholder=""  id="bh"/>
                                        <p>机器唯一识别编号</p>
                                    </div>
                                </li>
                                <li>
                                    <label>设备类型</label>
                                    <div>
                                        <asp:DropDownList ID="mechineType" runat="server" style="width: 60%;min-width: 400px; max-width: 800px;padding-left: 2%;height: 36px;border-radius: 6px;border: 1px solid #ddd;">
                                           
                                        </asp:DropDownList>
                                        <p>设备类型</p>
                                    </div>
                                </li>
                                 <li>
                                    <label>设备密码</label>
                                    <div>
                                        <input type="text" value="" placeholder=""  id="pwd"/>
                                        <p>用于机器登录</p>
                                    </div>
                                </li>
                                <li>
                                    <label>确认密码</label>
                                    <div>
                                        <input type="text" value="" placeholder=""  id="pwdAgain"/>
                                        <p>确认密码</p>
                                    </div>
                                </li>
                                <li>
                                    <input class="firmbtn" type="button" value="确定添加" onclick="addok()" />
                                </li>
                            </ul>
                        </section>
                    </div>
                </div>
            </div>
        </div>
        <input  id="companyID" runat="server" type="hidden"/>
    </form>
</body>
</html>
<script>
    function addok() {
        if ($("#start").val().trim()=="")
        {
            alert("请选择设备有效期");
            return;
        }
        if($("#bh").val().trim()=="")
        {
            alert("设备编号不能为空");
            return;
        }
        if ($("#pwd").val().trim() == "" || $("#pwdAgain").val().trim()=="")
        {
            alert("设备密码不能为空");
            return;
        }
        if ($("#pwd").val().trim() != $("#pwdAgain").val().trim())
        {
            alert("两次输入的密码不一致");
            return;
        }
        $.ajax({
            url: "../../ashx/asm.ashx",
            type: 'post',
            dataType: 'json',
            timeout: 10000,
            data: {
                action: "addEquipment",
                start: $("#start").val(),
                bh: $("#bh").val(),
                pwd: $("#pwd").val(),
                type: $("#mechineType").val()
            },
            success: function (resultData) {
                if (resultData.result == "1") {
                    alert("所选日期必须大于当前日期！");
                } else if (resultData.result == "2") {
                    alert("添加成功！");
                } else if (resultData.result == "3") {
                    alert("添加失败！");
                } else if (resultData.result == "4") {
                    alert("机器编号重复");
                }
            }
        }) 
        
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
                        location.href = "memberlist.aspx";
                    }
                    if (menuID == 'dgjl') {//订购记录
                        location.href = "dglist.aspx";
                    }
                    if (menuID == 'gmjl') {//购买记录
                        location.href = "orderlist.aspx";
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
<script>
    $(function () {
        $("#li1").find("a").addClass("aborder");
    })
</script>
