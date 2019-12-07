<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="integral.aspx.cs" Inherits="Consumer.main.integral" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>昵称设置-自助售卖系统</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />
    <link rel="shortcut icon" href="/main/public/images/favicon.ico" />
    <link href="/main/public/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/common.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/style.css" rel="stylesheet" type="text/css" />
    <script src="/main/public/script/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="/main/public/script/JavaScript.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <main class="setupq">
            <h4 class="commontitle">
                <i class="fa fa-angle-left" onclick="goBack()"></i>
                积分变动
            </h4>
            <section class="changelist">
                <ul id="jifenlist">
                    <li>
                        <div class="listleft">
                            <h4>签到获得积分</h4>
                            <p>2018-01-01 16:55:24</p>
                        </div>
                        <span><em>-15</em>积分</span>
                    </li>
                    <li>
                        <div class="listleft">
                            <h4>签到获得积分</h4>
                            <p>2018-01-01 16:55:24</p>
                        </div>
                        <span><em>15</em>积分</span>
                    </li>
                    <li>
                        <div class="listleft">
                            <h4>签到获得积分</h4>
                            <p>2018-01-01 16:55:24</p>
                        </div>
                        <span><em>15</em>积分</span>
                    </li>
                    <li>
                        <div class="listleft">
                            <h4>签到获得积分</h4>
                            <p>2018-01-01 16:55:24</p>
                        </div>
                        <span><em>-15</em>积分</span>
                    </li>
                </ul>
            </section>
        </main>
    </form>
</body>
</html>
<script>
    $(function () {
        var $Span = $("#jifenlist").find("li").find("span");
        $Span.each(function () {
            if (parseInt($(this).find("em").html()) <= 0) {
                $(this).addClass("spancolor");
            }
        });
    })
</script>
