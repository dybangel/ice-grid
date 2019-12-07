<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="uniondemo.Test" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>demo</title>
    <style type="text/css">
        body {
            background-color: #fff;
            font-family: "Microsoft YaHei";
            font-size: 14px;
        }

        .info {
            width: 970px;
            margin: 10px auto;
            white-space: 0;
            border: 1px solid #ccc;
        }

            .info td {
                padding: 5px 10px;
                background-color: #eee;
            }

                .info td span {
                    margin-left: 5px;
                }

                .info td label {
                    color: red;
                    margin-right: 5px;
                    font-size: 16px;
                    vertical-align: middle;
                }

                .info td.title {
                    width: 200px;
                    text-align: right;
                }

                .info td.content {
                    text-align: left;
                }

            .info input[type=text] {
                min-width: 200px;
                padding: 5px 7px;
                border: 1px solid #ccc;
                background-color: #fff;
                border-radius: 2px;
                -moz-border-radius: 2px;
                font-family: "Microsoft YaHei";
            }

            .info select {
                min-width: 200px;
                padding: 4px 7px;
                border: 1px solid #ccc;
                background-color: #fff;
                border-radius: 2px;
                -moz-border-radius: 2px;
                font-family: "Microsoft YaHei";
            }

            .info textarea {
                width: 80%;
                height: 100px;
                padding: 0 7px;
                line-height: 30px;
                border: 1px solid #ccc;
                background-color: #fff;
                border-radius: 2px;
                -moz-border-radius: 2px;
                font-family: "Microsoft YaHei";
            }

        .btn {
            width: 970px;
            margin: 20px auto 20px;
            text-align: center;
        }

        .btn_submit {
            width: 98px;
            height: 28px;
            margin: 0 10px;
            color: #fff;
            font-family: "Microsoft YaHei";
            background-color: #2C7FBF;
            border: 1px solid #0d90c5;
            border-radius: 2px;
            -moz-border-radius: 2px;
            cursor: pointer;
            outline: 0;
        }

        .btn_cancel {
            width: 98px;
            height: 28px;
            margin: 0 10px;
            font-family: "Microsoft YaHei";
            background-color: #eee;
            border: 1px solid #ccc;
            border-radius: 2px;
            -moz-border-radius: 2px;
            cursor: pointer;
            outline: 0;
        }
    </style>
</head>

<body>
    <form id="form1" runat="server">
        <div>
            <table class="info">
                
                <tr>
                    <td class="content">
                        <asp:Button ID="btn_pay" runat="server" OnClick="btn_pay_Click" Text="支付" class="btn_submit" />
                        <asp:Button ID="btn_cancel" runat="server" OnClick="btn_cancel_Click" Text="撤销" class="btn_submit"/>
                        <asp:Button ID="btn_refund" runat="server" OnClick="btn_refund_Click" Text="退款" class="btn_submit"/>
                        <asp:Button ID="btn_query" runat="server" OnClick="btn_query_Click" Text="查询" class="btn_submit"/>
                    </td>
                </tr>
            </table>
            <table class="info">
        	<tr>
                <td class="title">返回结果：</td>
                <td class="content">
                    <textarea  id="tblank" runat="server"></textarea>
                </td>
            </tr>
            
        </table>
        </div>
        <br />
        
    </form>
</body>
</html>
