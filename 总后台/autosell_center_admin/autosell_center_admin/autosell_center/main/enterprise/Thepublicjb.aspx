<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Thepublicjb.aspx.cs" Inherits="autosell_center.main.enterprise.Thepublicjb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>公众号设置-自动售卖终端中心控制系统</title>
    <link rel="shortcut icon" href="/main/public/images/favicon.ico" />
    <link href="/main/public/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/common.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/style.css" rel="stylesheet" type="text/css" />
    <script src="/main/public/script/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="/main/public/script/Titlelist.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="header"></div>
        <div class="main">
            <div id="updataCaDiv" class="addDiv change">
                <h4></h4>
                <ul>
                    <li>
                        <label></label>
                        <input type="text" id="updateCN" value="" placeholder="" />
                    </li>
                    <li>
                        <label></label>
                        <input type="button" value="确认修改" class="btnok" onclick="update()" />
                        <input type="button" value="取消" class="btnoff" onclick="divOff()" />
                    </li>
                </ul>
                <input type="hidden" value="" id="hiddenVal" />
            </div>
            <div class="popupbj"></div>
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
                                <a class="change" href="devicelist.aspx?companyID=<%=companyID %>"><i class="change fa fa-check-square"></i>设备列表</a>
                            </dd>
                            <dd>
                                <a class="change" href="firmcon.aspx?companyID=<%=companyID %>"><i class="change fa fa-plus-square"></i>新增设备</a>
                            </dd>
                        </dl>
                        <dl>
                            <dt>奶企设置<em class="fa fa-cog"></em></dt>
                            <dd>
                                <a class="change acolor"><i class="change icolor fa fa-wechat"></i>公众号设置</a>
                            </dd>
                           <%-- <dd>
                                <a class="change" href="Paymentzf.aspx?companyID=<%=companyID %>"><i class="change fa fa-money"></i>支付设置</a>
                            </dd>
                            <dd>
                                <a class="change" href="Profit.aspx?companyID=<%=companyID %>"><i class="change fa fa-database"></i>分润设置</a>
                            </dd>--%>
                        </dl>
                    </div>
                    <section class="jiqlist">
                        <dl class="jiqlistTab">
                            <dd class="change ddcolor"><b>基础信息</b></dd>
                            <%--  <dd class="change" onclick="javascript: location.href = 'Thepublicdx.aspx';"><b>短信信息</b></dd>
                            <dd class="change" onclick="javascript: location.href = 'Thepublicsyz.aspx';"><b>使用者管理</b></dd>--%>
                        </dl>
                        <ul class="thepublic" style="display: none">
                            <li>
                                <dl>
                                    <dd>公众号设置</dd>
                                </dl>
                            </li>
                            <li>
                                <label>头像</label>
                                <img id="pic" src="/main/public/images/nopic.png" alt="" />
                                <input type="file" id="upload" name="file" value="" style="display: none;" />
                                <a id="pica" class="public_p">修改</a>
                            </li>
                            <li>
                                <label>二维码</label>
                                <img id="pic2" src="/main/public/images/nopic.png" alt="" />
                                <input type="file" id="upload2" name="file" value="" style="display: none;" />
                                <a id="picb" class="public_p">修改</a>
                            </li>
                            <li>
                                <label>公众号名称</label>
                                <input type="text" value="" readonly="readonly" />
                                <a class="public_p">修改</a>
                            </li>
                            <li>
                                <label>账号</label>
                                <input type="text" value="" readonly="readonly" />
                                <a class="public_p">修改</a>
                            </li>
                            <li>
                                <label>原始ID</label>
                                <input type="text" value="" readonly="readonly" />
                                <a class="public_p">修改</a>
                            </li>
                            <li>
                                <label>类型</label>
                                <input type="text" value="普通订阅号" readonly="readonly" />
                                <a class="public_p">修改</a>
                            </li>
                            <li>
                                <label>接入方式</label>
                                <input type="text" value="普通接入" readonly="readonly" />
                                <!--<a class="public_p">修改</a>-->
                            </li>
                            <li>
                                <label>到期时间</label>
                                <input type="text" value="永久" readonly="readonly" />
                                <a class="public_p">修改</a>
                            </li>
                        </ul>
                        <ul class="thepublic">
                            <li>
                                <dl>
                                    <dd>自定义菜单通讯设置</dd>
                                </dl>
                            </li>
                            <li>
                                <label>AppID</label>
                                <input type="text" value="" readonly="readonly" id="appID"  runat="server"/>
                                <a class="public_p">修改</a>
                            </li>
                            <li>
                                <label>AppSecret</label>
                                <input type="text" value="" readonly="readonly" id="appSecret" runat="server"/>
                                <a class="public_p">修改</a>
                            </li>
                           <%-- <li>
                                <label>Token</label>
                                <input type="text" value="" readonly="readonly" id="token" runat="server"/>
                                <a class="public_p">修改</a>
                            </li>--%>
                        </ul>
                        <ul class="thepublic">
                            <li>
                                <dl>
                                    <dd>通联支付设置</dd>
                                </dl>
                            </li>
                            <li>
                                <label>APPID</label>
                                <input type="text" value="" readonly="readonly" id="APP_ID" runat="server"/>
                                <a class="public_p">修改</a>
                            </li>
                            <li>
                                <label>CUSID</label>
                                <input type="text" value="" readonly="readonly" id="CUSID" maxlength="32" runat="server"/>
                                <a class="public_p">修改</a>
                            </li>
                              <li>
                                <label>APPKEY</label>
                                <input type="text" value="" readonly="readonly" id="APPKEY" maxlength="32" runat="server"/>
                                <a class="public_p">修改</a>
                            </li>
                        </ul>
                    </section>
                </div>
            </div>
        </div>
        <input  id="val" runat="server" type="hidden"/>
        <input id="company_ID" runat="server" type="hidden"/>
       <%-- <input id="wx_appid" runat="server" type="hidden"/>
        <input id="wx_appsecret" runat="server" type="hidden"/>
        <input id="wx_token" runat="server" type="hidden"/>
        <input id="wx_mch_id" runat="server" type="hidden"/>
        <input id="wx_partnerKey" runat="server" type="hidden"/>--%>
    </form>
</body>
</html>
<script>
    $(function () {
        $("#pica").click(function () {
            $("#upload").click(); //隐藏了input:file样式后，点击头像就可以本地上传
            $("#upload").on("change", function () {
                var objUrl = getObjectURL(this.files[0]); //获取图片的路径，该路径不是图片在本地的路径
                if (objUrl) {
                    $("#pic").attr("src", objUrl); //将图片路径存入src中，显示出图片
                }
            });
        });
        $("#picb").click(function () {
            $("#upload2").click(); //隐藏了input:file样式后，点击头像就可以本地上传
            $("#upload2").on("change", function () {
                var objUrl = getObjectURL(this.files[0]); //获取图片的路径，该路径不是图片在本地的路径
                if (objUrl) {
                    $("#pic2").attr("src", objUrl); //将图片路径存入src中，显示出图片
                }
            });
        });
    });
    //建立一個可存取到該file的url
    function getObjectURL(file) {
        var url = null;
        if (window.createObjectURL != undefined) { // basic
            url = window.createObjectURL(file);
        } else if (window.URL != undefined) { // mozilla(firefox)
            url = window.URL.createObjectURL(file);
        } else if (window.webkitURL != undefined) { // webkit or chrome
            url = window.webkitURL.createObjectURL(file);
        }
        return url;
    }
</script>
<script>
    $(function () {
        $("#li0").find("a").addClass("aborder");
        $(".public_p").click(function () {
            var thisID = $(this).parent().find("input").attr("id");
            var popDivH4 = $("#updataCaDiv").find("h4");
            var popDivLaberl = $("#updataCaDiv").find("li").eq(0).find("label");
            var popDivBtn = $("#updataCaDiv").find("li").eq(1).find(".btnok");
            $("#hiddenVal").val(thisID);
            if (thisID == "appID") {
                popDivH4.html("AppID设置");
                popDivLaberl.html("AppID");
                $("#updateCN").val($("#appID").val());
               // popDivBtn.attr({ "onclick": "a()" });
                $("#val").val(1);
            } else if (thisID == "appSecret") {
                popDivH4.html("AppSecret设置");
                popDivLaberl.html("AppSecret");
                $("#updateCN").val($("#appSecret").val());
               // popDivBtn.attr({ "onclick": "b()" });
                $("#val").val(2);
            } else if (thisID == "token") {
                popDivH4.html("Token设置");
                popDivLaberl.html("Token");
                $("#updateCN").val($("#token").val());
                //popDivBtn.attr({ "onclick": "c()" });
                $("#val").val(3);
            } else if (thisID == "APP_ID") {
                popDivH4.html("APPID");
                popDivLaberl.html("APPID");
                $("#updateCN").val($("#APP_ID").val());
                //popDivBtn.attr({ "onclick": "d()" });
                $("#val").val(4);
            } else if (thisID == "CUSID") {
                popDivH4.html("CUSID");
                popDivLaberl.html("CUSID");
                $("#updateCN").val($("#CUSID").val());
                
                $("#val").val(5);
            } else if (thisID == "APPKEY") {
                popDivH4.html("APPKEY");
                popDivLaberl.html("APPKEY");
                $("#updateCN").val($("#APPKEY").val());

                $("#val").val(6);
            }
            $(".addDiv").addClass("addDivshow");
            $(".popupbj").fadeIn();
        });
    })
    function divOff() {
        $(".popupbj").hide();
        $(".addDiv").removeClass("addDivshow");
    }
    function update()
    {
        var  str = $("#updateCN").val();
        if(str=="")
        {
            alert("请填写信息");
            return;
        } 
        $.ajax({
            type: "post",
            url: "Thepublicjb.aspx/update",
            contentType: "application/json; charset=utf-8",
            data: "{val:'" + $("#val").val() + "',str:'" + str + "',companyID:'" + $("#company_ID").val() + "'}",
            dataType: "json",
            success: function (data) {
                if (data.d == "1") {
                    location.reload();
                } else {
                    alert("保存失败");
                }
            }
        })
    }
</script>
