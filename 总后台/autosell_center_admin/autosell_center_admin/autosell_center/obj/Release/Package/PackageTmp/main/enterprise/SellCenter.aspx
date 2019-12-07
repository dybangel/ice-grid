<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SellCenter.aspx.cs" Inherits="autosell_center.main.enterprise.SellCenter" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>首页-自动售卖终端中心控制系统</title>
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
            <div class="main_list">
                <div class="common_title">
                    <h4>
                        <i class="fa fa-home"></i>
                        <span>选择企业客户进行操作</span>
                    </h4>
                </div>
                  <div id="updataCaDiv" class="addDiv change addDivshow" style="display:none">
                          <h4>视频实况<a style="float:right;color:#fff;font-size:1.8rem;margin-right:16px;" onclick="divOff()">×</a></h4>
                              <img  id="vid" src=""/>
                       </div>
                <div class="common_main">
                    <div class="navlist">
                        <dl>
                            <dt>奶企管理<em class="fa fa-cog"></em></dt>
                            <dd>
                                <a class="change acolor" href="#" onclick="qx_judge('nqlb')"><i class="change icolor fa fa-check-square"></i>奶企列表</a>
                            </dd>
                            <dd>
                                <a class="change" href="#"  onclick="qx_judge('xznq')"><i class="change fa fa-plus-square"></i>新增奶企</a>
                            </dd>
                        </dl>
                    </div>
                      <section class="jiqlist" style="height:70px">
                       
                        <ul class="jiqlistseach">
                            
                            <li>
                                <input type="text" value="" placeholder="奶企名称或编号" id="bh" />
                            </li>
                             
                            <li>
                                <input type="button" value="查询" class="seachbtn" onclick="sear()" />
                            </li>
                        </ul>
                       
                    </section>
                    <section class="jiqlist">
                         
                        <ul class="firm_list" id="ull">
                            
                          
                        </ul>
                    </section>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
<script>
    $(function () {
        qx_judge("nqlb");
        $("#li0").find("a").addClass("aborder");
        sear();
    })
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
                    if (menuID == 'nqlb')
                    {
                        //会员列表
                        //location.href = "SellCenter.aspx";
                    }
                    if (menuID == 'xznq')
                    {
                        //新增奶企
                        location.href = "FirmAdd.aspx";
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
    function sear()
    {
        $("#ull").empty();
        $.ajax({
            type: "post",
            url: "SellCenter.aspx/sear",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{keyword:'" + $("#bh").val() + "'}",
            success: function (data) {
                var serverdata = $.parseJSON(data.d);
                var serverdatalist = serverdata.length;
                for (var i = 0; i < serverdatalist; i++) {
                    $(" <li>"
                             +"<div class='firm_mask change'>"
                             + "<a href='devicelist.aspx?companyID=" + serverdata[i].id + "'>"
                             +"<span>查看企业 ></span>"
                             +"</a>"
                             +"</div>"
                             +"<div class='firm_logo'>"
                             + "<img src='../../" + serverdata[i].path + "' alt='' />"
                             +"</div>"
                             + "<span>" + serverdata[i].name + "</span>"
                              + "<p>企业ID：" + serverdata[i].id + "</p>"
                             + "<p>共" + serverdata[i].num + "台机器</p>"
                             
                             + "<a href='firnupdate.aspx?companyID=" + serverdata[i].id + "' class='nqBtn' style='text-align:center;line-height:32px'>查看</a>"
                             +"</li>").appendTo("#ull");
                }
            }
        })
    }
    function lookUp(id) {
        $.ajax({
            type: "post",
            url: "SellCenter.aspx/getPath",
            contentType: "application/json; charset=utf-8",
            data: "{id:'" + id + "'}",
            dataType: "json",
            success: function (data) {
                if (data.d != "") {

                    $("#vid").attr("src", "../../" + data.d);


                } else {
                    alert("找不到图片");
                }
            }
        });
        $("#updataCaDiv").addClass("addDivshow");
        setTimeout(function () {
            $(".popupbj").fadeIn();
        }, 100);
    }
</script>
