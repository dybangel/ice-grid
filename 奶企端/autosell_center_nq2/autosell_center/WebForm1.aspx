<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="autosell_center.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script src="main/public/script/jquery-3.2.1.min.js"></script>
    <script>
        var BASE_URL = "http://114.116.16.200/api/api.ashx";
        function ch()
        {
            $.ajax({
                url: BASE_URL,
                type: 'post',
                dataType: 'json',
                timeout: 10000,
                data: { action: 'ch', ldNO: '23', mechineID: '25' },
                success: function (resultData) {
                    // 成功
                }
            })
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
     <div>
         <input  type="button" onclick="ch()" value="测试出货"/>

     </div>
    </form>
</body>
</html>
