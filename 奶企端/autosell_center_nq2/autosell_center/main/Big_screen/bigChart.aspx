<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="bigChart.aspx.cs" Inherits="autosell_center.main.Big_screen.bigChart" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>成交动态图</title>
    <link href="/main/public/css/common.css" rel="stylesheet" />
    <link href="public/Big_screenStyle.css" rel="stylesheet" />
    <link rel="shortcut icon" href="/main/public/images/favicon.ico" />
    <link href="/main/public/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/common.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/style.css" rel="stylesheet" type="text/css" />
    <script src="/main/public/script/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="/main/public/script/Titlelist.js" type="text/javascript"></script>
    <script src="../public/script/jquery-3.2.1.min.js"></script>
    <%--<script type="text/javascript" src="http://echarts.baidu.com/gallery/vendors/echarts/echarts.min.js"></script>--%>
    <script type="text/javascript" src="http://echarts.baidu.com/examples/vendors/echarts/echarts.min.js"></script>
    <style>
        html,body {
            min-width: 100%;
            background: initial;
        }
        form {
            width: 100%;
            height: 100%;
        }
         .jiqlistseach {
            margin-bottom: 30px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
           <div class="header"></div>
    <div class="main">
        <div class="main_list">
            <div class="common_title">
                <h4>
                    <i class="fa fa-cubes"></i>
                    <span>成交动态图</span>
                </h4>
            </div>
            <div class="common_main">
                <div class="navlist">
                    <dl>
                        <dt>成交动态图<em class="fa fa-cog"></em></dt>
                            <dd>
                                <a class="change acolor" href="echartMain.aspx"><i class="change fa fa-window-restore"></i>成交动态图</a>
                            </dd>
                            
                    </dl>
                </div>
                <section class="jiqlist">
                     <div id="screenSeachChart" style="height: 100%"></div>
                </section>
            </div>
        </div>
    </div>
       
       <input id="company_id" runat="server" type="hidden"/>
    </form>
</body>
</html>
<script type="text/javascript">
    $(function () {
        $("#li6").find("a").addClass("aborder");
    })
var dom = document.getElementById("screenSeachChart");
var myChart = echarts.init(dom);
var app = {};
option = null;
app.title = '多 Y 轴示例';
var colors = ['#5793f3', '#d14a61', '#675bba'];
option = {
    color: colors,
    tooltip: {
        trigger: 'axis',
        axisPointer: {
            type: 'cross'
        }
    },
    grid: {
        right: '20%'
    },
    toolbox: {
        feature: {
            dataView: {show: true, readOnly: false},
            restore: {show: true},
            saveAsImage: {show: true}
        }
    },
    legend: {
       data: ['销量', '金额', '订单'],
	    textStyle: {
                //图例文字的样式
                x: 'left', //图例显示在左边
                color: '#fff',
                fontSize: 12
            }
    },
    xAxis: [
        {
		splitLine:{show: false},//去除网格线
            type: 'category',
            axisTick: {
                alignWithLabel: true
            },
            data: [<%=time%>]
        }
    ],
    yAxis: [
        {
            type: 'value',
            name: '金额',
            min: 0,
            max: <%=max_money%>,
            position: 'right',
			splitLine:{show: false},//去除网格线
            axisLine: {
                lineStyle: {
                    color: colors[0]
                }
            },
            axisLabel: {
                formatter: '{value} 元'
            }
        },
        {
            type: 'value',
            name: '订单',
            min: 0,
            max: <%=max_dd%>,
            position: 'right',
            offset: 80,
			splitLine:{show: false},//去除网格线
            axisLine: {
                lineStyle: {
                    color: colors[1]
                }
            },
            axisLabel: {
                formatter: '{value} 笔'
            }
        },
        {
            type: 'value',
            name: '销量',
            min: 0,
            max: <%=max_xl%>,
            position: 'left',
			splitLine:{show: false},//去除网格线
            axisLine: {
                lineStyle: {
                    color: colors[2]
                }
            },
            axisLabel: {
                formatter: '{value} 件'
            }
        }
    ],
    series: [
        {
            name:'金额',
            type:'bar',
            data:[<%=money%>]
        },
        {
            name:'订单',
            type:'bar',
            yAxisIndex: 1,
            data:[<%=ddNum%>]
        },
        {
            name:'销量',
            type:'line',
            yAxisIndex: 2,
            data:[<%=xsNUm%>]
        }
    ]
};
;
    if (option && typeof option === "object") {
        myChart.setOption(option, true);
        //myChartBig.setOption(option, true);
        fuzhi();
    }
    function fuzhi()  
    {  
        $(window.parent.$("#totalDD").html(<%=totalDD%>+"笔"));
        $(window.parent.$("#totalXL").html(<%=totalXL%>+"件"));
        $(window.parent.$("#totalMoney").html(<%=totalMoney%>+"元"));
    }  
</script>