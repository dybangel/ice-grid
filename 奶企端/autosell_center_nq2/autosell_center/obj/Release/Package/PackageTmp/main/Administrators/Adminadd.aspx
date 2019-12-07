<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Adminadd.aspx.cs" Inherits="autosell_center.main.Administrators.Adminadd" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link rel="shortcut icon" href="/main/public/images/favicon.ico" />
    <link href="/main/public/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/common.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/style.css" rel="stylesheet" type="text/css" />
    <script src="/main/public/script/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="/main/public/script/Titlelist.js" type="text/javascript"></script>
    <style>
        .productclass {
            margin-top: 50px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="header"></div>
    <div class="main">
        <div class="main_list">
            <div class="common_title">
                <h4>
                    <i class="fa fa-line-chart"></i>
                    <span>设备管理员</span>
                </h4>
            </div>
            <div class="common_main">
                <div class="navlist">
                    <dl>
                        <dt>分析<em class="fa fa-cog"></em></dt>
                        <dd>
                            <a class="change" href="Administrators.aspx"><i class="change fa fa-users"></i>管理员</a>
                        </dd>
                        <dd>
                            <a class="change acolor"><i class="change icolor fa fa-user-plus"></i>添加管理员</a>
                        </dd>
                    </dl>
                </div>
                <div class="jiqadd">
                    <section class="addnaiq">
                        <h4 class="commonfut"><a class="change" href="Administrators.aspx"><i class="change fa fa-angle-left"></i>管理员管理</a>/添加管理员</h4>
                        <ul class="addnaiqdata" id="addnaiqdata1" style="display: block;">
                            <li>
                                <label>管理员账号</label>
                                <div>
                                    <input type="text" value="" placeholder="管理员账号" id="bh"/>
                                    <p>输入管理员账号</p>
                                </div>
                            </li>
                              <li>
                                <label>联系电话</label>
                                <div>
                                    <input type="password" value="" placeholder="" id="phone"/>
                                     <p>用于机器异常短信通知</p>
                                </div>
                            </li>
                            <li>
                                <label>密码</label>
                                <div>
                                    <input type="password" value="" placeholder="" id="pwd1"/>
                                </div>
                            </li>
                             <li>
                                <label>密码</label>
                                <div>
                                    <input type="password" value="" placeholder="" id="pwd2"/>
                                </div>
                            </li>
                           <li>
                                <label>管理员权限</label>
                                <div>
                                    <select id="sel">
                                        <option value="1">维保</option>
                                        <option value="2">运维</option>
                                    </select>
                                    <p>选择一个管理员的权限</p>
                                </div>
                            </li>
                            <li>
                                <input class="firmbtn" type="button" value="确定添加" onclick="addok()"/>
                            </li>
                        </ul>
                    </section>
                </div>
            </div>
        </div>
    </div>
        <input id="companyId" runat="server" type="hidden"/>
    </form>
</body>
</html>
<script>
    $(function() {
        $("#li5").find("a").addClass("aborder");
    })
    function addok()
    {
        if($("#bh").val()=="")
        {
            alert("账号不能为空");
            return;
        }
        if ($("#pwd1").val() == "") {
            alert("密码不能为空");
            return;
        }
        if ($("#pwd2").val() == "") {
            alert("密码确认不能为空");
            return;
        }
        if($("#pwd1").val()!=$("#pwd2").val())
        {
            alert("两次输入的密码不一致");
            return;
        }
        if($("#phone").val()=="")
        {
            alert("手机号不能为空");
            return;
        }
        if ($("#phone").val().substring(0, 1) != "1" || $("#phone").val().length<11)
        {
            alert("手机号不正确");
            return;
        }
        $.ajax({
            type: "post",
            url: "Adminadd.aspx/add",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{bh:'" + $("#bh").val() + "',pwd1:'" + $("#pwd1").val() + "',qx:'" + $("#sel").val() + "',companyID:'" + $("#companyId").val() + "',phone:'"+$("#phone").val()+"'}",
            success: function (data) {
                if(data.d=="1")
                {
                    alert("添加失败");
                }else if(data.d=="2")
                {
                    alert("添加成功");
                }
            }
        })
    }
</script>