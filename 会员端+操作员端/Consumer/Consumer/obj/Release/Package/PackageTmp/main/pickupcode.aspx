<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="pickupcode.aspx.cs" Inherits="Consumer.main.pickupcode" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>取货码-自助售卖系统</title>
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
                取货码
            </h4>
            <section class="quhuomain">
                <span>我的取货码：</span>
                <%
                    if (dt.Rows.Count>0)
                    {
                        for (int i=0;i<dt.Rows.Count;i++)
                        {%>
                              <h2><%=dt.Rows[i]["code"].ToString() %></h2>
                       <% }
                    }
                %>
              
               
            </section>
            <section class="quhuoxin">
                <p>使用过程中如有任何疑问请咨询客服</p>
                <span><i class="fa fa-phone"></i>客服电话：400-000-0000</span>
                <dl>
                    <dd>
                        <em>1</em>
                        <img src="/main/public/images/Icon/liji.png" alt="" />
                        <span>售卖机选择<br />
                            扫码取货</span>
                    </dd>
                    <dd>
                        <em>2</em>
                        <img src="/main/public/images/Icon/liji.png" alt="" />
                        <span>输入以上<br />取货码</span>
                    </dd>
                    <dd>
                        <em>3</em>
                        <img src="/main/public/images/Icon/liji.png" alt="" />
                        <span>点击确定<br />完成取货</span>
                    </dd>
                </dl>
            </section>
        </main>
        <input id="memberID" runat="server" type="hidden"/>
          <input  id="flag" runat="server" type="hidden"/>
    </form>
</body>
</html>
<script>
    if (/MicroMessenger/.test(window.navigator.userAgent)) {
        //微信浏览器
        $("#flag").val(1);
    } else {

        //其他浏览器
        $("#flag").val(0);
    }
</script>
