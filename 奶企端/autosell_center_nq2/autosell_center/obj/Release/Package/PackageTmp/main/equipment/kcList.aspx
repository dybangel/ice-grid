<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="kcList.aspx.cs" Inherits="autosell_center.main.equipment.kcList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link rel="shortcut icon" href="/main/public/images/favicon.ico" />
    <link href="/main/public/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/common.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/style.css" rel="stylesheet" type="text/css" />
    <script src="/main/public/script/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="/main/public/script/Titlelist.js" type="text/javascript"></script>
    <style>
        .materall ul li{
            height:80px;
            padding-left:6px;
        }
        .materall ul li span{
            line-height:30px;
            font-size:1.4rem;
        }
        .materall ul li b{
            width:100%;
            float:left;
            margin:0 auto;
            line-height:20px;
            font-size:1rem;
            overflow: hidden;
            text-overflow:ellipsis;
            white-space: nowrap;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="header"></div>
        <div class="main">
            <!--00未设置  11已选中 1订购 2零售-->
            <div class="main_list">
                <div class="common_title">
                    <h4>
                        <i class="fa fa-plus"></i>
                        <span>设备管理</span>
                    </h4>
                </div>
                <div class="common_main">
                    <div class="navlist">
                        <dl>
                            <dt>设备管理<em class="fa fa-cog"></em></dt>
                            <dd>
                                <a class="change" href="equipmentlist.aspx"><i class="change fa fa-inbox"></i>设备列表</a>
                            </dd>
                              <dd>
                                <a class="change" href="kclist2.aspx?mechineID=<%=mechineID %>"><i class="change fa fa-inbox"></i>库存</a>
                            </dd>
                        </dl>
                    </div>
                    <section class="jiqlist">
                        <dl class="jiqlistTab">
                            <dd class="change ddcolor"><b>设备编号：<%=dt.Rows[0]["bh"].ToString() %></b></dd>
                        </dl>
                       
                        <div class="setbtn">
                            <i class="fa fa-check" onclick="allliao()"></i>
                            
                            <input type="button" value="订购料道" class="setding" onclick="dingOn()" />
                            <input type="button" value="零售料道" class="setling" onclick="lingOn()" />
                             <input type="button" value="导出Excel" class="setqing" onclick="excelData()" />
                            
                        </div>
                        <div class="liaolist">
                            
                            <div class="materall">
                                <dl>
                                    <dd></dd>
                                </dl>
                                <ul id="liaodaoUl" class="materlist">
                                    
                                </ul>
                            </div>
                        </div>
                    </section>
                </div>
            </div>
        </div>
        
        <input id="mechine_id" runat="server" type="hidden" />
    </form>
</body>
</html>
<script>
    function excelData()
    {
        
        window.location.href = "../../api/ExportExcel.ashx?action=exportExcelKC&mechineID=" + $("#mechine_id").val();
    }
    $(function ()
    {
        $.ajax({
            type: "post",
            url: "kcList.aspx/getLDInfo",
            contentType: "application/json; charset=utf-8",
            data: "{mechineID:'" + $("#mechine_id").val() + "'}",
            dataType: "json",
            success: function (data) {
                $("#ull").empty();
                var serverdata = $.parseJSON(data.d);
                var serverdatalist = serverdata.length;
                for (var i = 0; i < serverdatalist; i++) {
                    $("  <li class='liling'>"
                                      + " <span class='commoncolor'>" + serverdata[i].ldNO + "料道</span>"
                                     + " <b class='commoncolor'>" + serverdata[i].proName + "</b>"
                                     + " <b class='commoncolor'>" + serverdata[i].ld_productNum + "件</b>"
                                  + " </li>").appendTo("#liaodaoUl");

                }
            }
        });
    })
</script>
