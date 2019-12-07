<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="syssetting.aspx.cs" Inherits="autosell_center.main.enterprise.syssetting" %>

<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>首页-自动售卖终端中心控制系统</title>
    <meta charset="utf-8" />
    <link rel="shortcut icon" href="/main/public/images/favicon.ico" />
    <link href="/main/public/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/common.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/style.css" rel="stylesheet" type="text/css" />
    <script src="/main/public/script/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="/main/public/script/Titlelist.js" type="text/javascript"></script>
    <style>
        .productclass li .classid,.productclass li .classname{
            width:20%;
        }
        .productclass li .classdelete{
            width:60%;
        }
        .productclass li .classdelete input{
            width:80%;
        }
        .productclass{
            margin-top:30px;
        }
    </style>
</head>
<body>
    <form>
        <div class="header"></div>
    <div class="main">
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
                            <dt>参数设置<em class="fa fa-cog"></em></dt>
                            <dd>
                               <a class="change acolor" href="syssetting.aspx"><i class=" fa fa-bars"></i>参数设置</a>
                            </dd>
                        </dl>
                    </div>
                <section class="jiqlist">
                    <dl class="jiqlistTab">
                        <dd class="change ddcolor"><b>参数设置</b></dd>
                    </dl>
                    <table class="syssettingTable">
                        <tr>
                            <td>APP定点更新</td>
                            <td>
                                <input type="text" value="" id="appTime" placeholder="格式:08:00"  runat="server" maxlength="5"/>
                            </td>
                        </tr>
                      
                        <tr>
                            <td>视屏下载间隔</td>
                            <td>
                                <input type="text" value="" id="downTime" placeholder="例如间隔60分钟请输入60，单位分钟"  runat="server"/>分钟
                            </td>
                        </tr>
                    </table>
                    <input type="button" value="保存" class="seachbtnTwo" onclick="save()"/>
                </section>
            </div>
        </div>
    </div>
       
    </form>
    
</body>
</html>
<script>
    $(function () {
        $("#li10").find("a").addClass("aborder");
       
    })
    function save() {
        $.ajax({
            type: "post",
            url: "syssetting.aspx/save",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{appTime:'" + $("#appTime").val() + "',downTime:'" + $("#downTime").val() + "'}",
            success: function (data) {
                if (data.d == "1") {
                    alert("保存成功");
                } else if (data.d == "0")
                {
                    alert("保存失败");
                } else if (data.d == "3") {
                    alert("时间格式不正确");
                } else if (data.d == "4") {
                    alert("视屏下载间隔不正确 请输入正整数");
                }  
            }
        })
    }
   
</script>