<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="authPage.aspx.cs" Inherits="OpenPlatForm.api.authPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
   
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>公众号设置-自动售卖终端中心控制系统</title>
    <link rel="shortcut icon" href="/main/public/images/favicon.ico" />
    <link href="/main/public/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/common.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/style.css" rel="stylesheet" type="text/css" />
    <script src="/main/public/script/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="/main/public/script/Titlelist.js" type="text/javascript"></script>
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
                     <%--  <dl>
                            <dt>App流量统计<em class="fa fa-cog"></em></dt>
                            <dd>
                                 <a class="change" href="applog.aspx"><i class="change fa fa-bars"></i>App流量统计</a>
                            </dd>
                        </dl>--%>
                     
                    </div>
                    <%-- 弹框+背景 --%>
                    <div class="greybg" style="display: none;"></div>
                    <div class="whiteDiv" style="display: none;">
                        <span>您当前尚未绑定微信公众号，点击下方按钮，进行公众平台帐号授权</span>
                        <div><a  href='<%=PhoneAuthPageUrl %>'>公众平台帐号授权</a></div>
                    </div>
                    <section class="jiqlist" id="userinfoDiv" style="display: none;">
                        <dl class="jiqlistTab">
                            <dd class="change ddcolor"><b>基础信息</b></dd>
                        </dl>
                        <ul class="thepublic">
                            <li>
                                <dl>
                                    <dd>公众号信息</dd>
                                </dl>
                            </li>
                         
                            <li>
                                <label>公众号昵称</label>
                                <input type="text" value="<%=(DC!=null)?DC.Rows[0]["nick_name"].ToString():"" %>" readonly="readonly" />
                                <%--<a class="public_p">修改</a>--%>
                            </li>
                            <li>
                                <label>公众号主体</label>
                                <input type="text" value="<%=(DC!=null)?DC.Rows[0]["principal_name"].ToString():"" %>" readonly="readonly" />
                                <%--<a class="public_p">修改</a>--%>
                            </li>
                            <li>
                                <label>微信公众号</label>
                                <input type="text" value="<%=(DC!=null)?DC.Rows[0]["alias"].ToString():"" %>" readonly="readonly" />
                                <%--<a class="public_p">修改</a>--%>
                            </li>
                            <li>
                                <label>公众号AppId</label>
                                <input type="text" value="<%=(DC!=null)?DC.Rows[0]["appId"].ToString():"" %>" readonly="readonly" />
                                <%--<a class="public_p">修改</a>--%>
                            </li>
                            <li>
                                <label>微信账号类型</label>
                                <input type="text" value="<%=(DC!=null)?(DC.Rows[0]["service_type_info"].ToString()=="2"?"服务号":"订阅号"):"订阅号" %>" readonly="readonly" />
                            </li>
                              <li>
                                <label>订阅回复内容</label>
                                <textarea id="textarea" style="width:1000px;height:200px " > <%=(DC!=null)?(DC.Rows[0]["subscribe_info"].ToString()):"订阅号" %></textarea> 
                            </li>
                             <li>
                                <label>保存</label>
                                <button  style="background:#3a77d5;color:#fff;border:0;width:100px;height:36px" onclick="saveSubscribeInfo()">确定</button>
                            </li>
                            <%-- <li>
                                <div><a onclick="setIndus()">设置行业</a></div>
                            </li>--%>
                        </ul>
                    </section>
                </div>
            </div>
        </div>
        <input id="val" runat="server" type="hidden" />
        <input id="HF_userId" runat="server" type="hidden" />
        <input id="HF_isOpen" runat="server" type="hidden" />
         <input id="_operaID" runat="server" type="hidden" />
    </form>
    <input type="hidden" id="HF_url" value="<%=PhoneAuthPageUrl %>" />
    <input type="hidden" id="HF_authAppid" value="<%=authAppid %>" />
    <input type="hidden" id="HF_refreshToken" value="<%=refreshToken %>" />
    <input type="hidden" id="HF_userInfoId" runat="server"/>
    <input  type="hidden" id="_companyID" runat="server"/>
    <script src="../script/jquery-3.2.1.min.js"></script>
    <script>
        function judge() {
            $.ajax({
                type: "post",
                url: "authPage.aspx/judge",
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
            judge();
            $("#li8").find("a").addClass("aborder");
            var ddNum = $(".wechatnavlist").find("dd").length;
            if (ddNum >= 4) {
                $(".wechatnavlist").find("dt").hide();
            } else {
                $(".wechatnavlist").find("dt").show();
            }

            $(".wechatnavlist").find("dd").eq(0).addClass("ddcolor");
            $(".wechatnavmain").eq(0).show();

            var url = $("#HF_url").val();
            if ($.trim(url)) {
                //授权连接不为空的话，证明没授权过,强制弹框展示“授权按钮”
                $(".greybg").show();
                $(".whiteDiv").fadeIn(500);
            } else {
                //授权过，展示授权信息
                $("#userinfoDiv").show();
            }
        });
        function saveSubscribeInfo() {
            $.ajax({
                type: "post",
                url: "authPage.aspx/saveSubscribeInfo",
                contentType: "application/json; charset=utf-8",
                data: "{subscribeInfo:'" + $("#textarea").val() + "',companyID:'" + $("#_companyID").val() + "'}",
                dataType: "json",
                success: function (databack) {
                    if (databack.d.result == 200) {

                        window.location.reload();
                    } else {
                        alert(databack.d.msg);
                    }
                }
            });
        }
        
        function setIndus() {
            $.ajax({
                type: "post",
                url: "authPage.aspx/setIndus",
                contentType: "application/json; charset=utf-8",
                data: "{companyID:'" + $("#_companyID").val() + "'}",
                dataType: "json",
                success: function (databack) {
                    alert(databack.d.msg);
                }
            });
        }
        //重新授权
        function authAgain() {
            var authAppid = $("#HF_authAppid").val();
            var refreshToken = $("#HF_refreshToken").val();
            var userId = $("#HF_userInfoId").val();
            if ($.trim(authAppid) && $.trim(refreshToken)) {
                $.ajax({
                    type: "post",
                    url: "/api/authPage.aspx/AuthAgain",
                    contentType: "application/json; charset=utf-8",
                    data: "{authAppid:'" + authAppid + "',refreshToken:'" + refreshToken + "',userId:'" + userId + "'}",
                    dataType: "json",
                    success: function (databack) {
                        if (databack.d.result == 0) {
                           
                            window.location.reload();
                        } else {
                            alert(databack.d.msg);
                        }
                        return false;
                    }
                });
            }
        }
      
    </script>
</body>
</html>
