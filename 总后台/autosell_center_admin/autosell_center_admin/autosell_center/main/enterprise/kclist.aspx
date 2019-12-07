<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="kclist.aspx.cs" Inherits="autosell_center.main.enterprise.kclist" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link rel="shortcut icon" href="/main/public/images/favicon.ico" />
    <link href="/main/public/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/common.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/style.css" rel="stylesheet" type="text/css" />
    <script src="/main/public/script/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="/main/public/script/Titlelist.js" type="text/javascript"></script>
    <script charset="utf-8" src="http://map.qq.com/api/js?v=2.exp" type="text/javascript"></script>
	<script src="http://open.map.qq.com/apifiles/2/4/79/main.js" type="text/javascript"></script>
    <script src="https://open.ys7.com/sdk/js/1.3/ezuikit.js"></script>
      <link  href="../../scripts/skin/jedate.css" rel="stylesheet" type="text/css"/>
    <script src="../../scripts/jedate.js" type="text/javascript"></script>
    <script src="../../scripts/jedate.min.js"></script>
    <script type="text/javascript" src="http://echarts.baidu.com/gallery/vendors/echarts/echarts.min.js"></script>
    <script type="text/javascript" src="http://echarts.baidu.com/gallery/vendors/echarts-gl/echarts-gl.min.js"></script>
    <script type="text/javascript" src="http://echarts.baidu.com/gallery/vendors/echarts-stat/ecStat.min.js"></script>
    <script type="text/javascript" src="http://echarts.baidu.com/gallery/vendors/echarts/extension/dataTool.min.js"></script>
    <script type="text/javascript" src="http://echarts.baidu.com/gallery/vendors/echarts/map/js/china.js"></script>
    <script type="text/javascript" src="http://echarts.baidu.com/gallery/vendors/echarts/map/js/world.js"></script>
    <script type="text/javascript" src="http://api.map.baidu.com/api?v=2.0&ak=ZUONbpqGBsYGXNIYHicvbAbM"></script>
    <script type="text/javascript" src="http://echarts.baidu.com/gallery/vendors/echarts/extension/bmap.min.js"></script>
    <script type="text/javascript" src="http://echarts.baidu.com/gallery/vendors/simplex.js"></script>
	<style>
            #address{height: 31px;padding: 0 10px;}
            .map-seach{background: #50a4ec;padding: 5px 20px;color: #fff;display: inline-block;}
            .map-seach:active{background: rgba(80, 164, 236, 0.4);}
            .smnoprint,.smnoprint{display:none}
            .jiqlisttable li .jiqzt2{width:10%}
            .jiqlisttable li .jiqcz2{width:16%;}
    </style>
</head>
<body>
    <form id="form1" runat="server">
     <div class="header"></div>
        <div class="main">
            <div class="main_list" >
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
                                <a class="change " href="devicelist.aspx?companyID=<%=CompanyID %>" ><i class="change  fa fa-inbox"></i>设备列表</a>
                            </dd>
                             <dd>
                                <a class="change acolor" href="#"><i class="change icolor fa fa-inbox"></i>库存</a>
                            </dd>
                        </dl>
                    </div>
                    <section class="jiqlist">
                        <div style="width:400px;border-right:1px solid #ddd;float:left;height:100%;overflow-x:hidden">
                        <ul class="memberlist" style="display: block;" id="ull">
                             
                        </ul>
                        </div>
                        <div style="width:calc(100% - 401px);float:left;height: 100%; margin: 0;">
                            <div style="margin-top:60px;height:600px;width:100%;float:left">
                               <div id="container" style="height: 100%;width:100%;"></div>
                            </div>
                        </div>
                    </section>
                </div>
            </div>
        </div>
        <input id="mechineID" runat="server" type="hidden" />
        <input id="company_ID" runat="server" type="hidden" />
        
    </form>
</body>
</html>
<script type="text/javascript">
    var dom = document.getElementById("container");
    var myChart = echarts.init(dom);
    var app = {};
    option = null;
    app.title = '';
    $(function () {
        sear();
    });
    function sear()
    {
        $.ajax({
            type: "post",
            url: "kclist.aspx/getList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{mechineID:'" + $("#mechineID").val() + "'}",
            success: function (data) {
                $(" <li>"
                           + "  <span style='width:50%'>产品</span>"
                           + "  <span style='width:50%'>库存</span>"
                           + "  </li>").appendTo("#ull");
                var numS = data.d.split('@@@')[0];
                var nameS = data.d.split('@@@')[1];
                if (data.d != "1") {

                    var serverdata = $.parseJSON(data.d.split('@@@')[2]);
                    var serverdatalist = serverdata.length;
                    for (var i = 0; i < serverdatalist; i++) {
                        $(" <li>"
                            + "  <span style='width:50%'>" + serverdata[i].name + "</span>"
                            + "  <span style='width:50%'>" + serverdata[i].num + "</span>"
                            + "  </li>").appendTo("#ull");
                    }
                    option = {
                        title: {
                            text: '<%=title%>'+'库存',
                            subtext: ''
                        },
                        tooltip: {
                            trigger: 'axis',
                            axisPointer: {
                                type: 'shadow'
                            }
                        },
                        grid: {
                            left: '3%',
                            right: '4%',
                            bottom: '3%',
                            containLabel: true
                        },
                        xAxis: {
                            type: 'value',
                            boundaryGap: [0, 0.01]
                        },
                        yAxis: {
                            type: 'category',
                            data: nameS.split(',')
                        },
                        series: [
                            {
                                type: 'bar',
                                data: numS.split(',')
                            }
                        ]
                    };
                    if (option && typeof option === "object") {
                        myChart.setOption(option, true);
                    }

                }
            }
       
    })
    }

  
  
</script>
