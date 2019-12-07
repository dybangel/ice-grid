<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Analysis.aspx.cs" Inherits="autosell_center.main.Analysis.Analysis" %>

<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>成交分析-自动售卖终端中心控制系统</title>
    <meta charset="utf-8" />
    <link rel="shortcut icon" href="/main/public/images/favicon.ico" />
    <link href="/main/public/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/common.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/style.css" rel="stylesheet" type="text/css" />
    <script src="/main/public/script/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="/main/public/script/Titlelist.js" type="text/javascript"></script>
    <style>
        #container,#container1,#container2 {
            height: auto;
            min-height: 600px;
            float: left;
            position: relative;
        }
         #container div,#container1 div {
             width: 100% !important;
         }
         #container1 div:last-child {
             width: auto !important;
         }
        .jiqlisttable {
            display: block;
            position: absolute;
            top: 50px;
            z-index: -99;
        }
    </style>
</head>
<body>
    <div class="header"></div>
    <div class="main">
        <div class="main_list">
            <div class="common_title">
                <h4>
                    <i class="fa fa-line-chart"></i>
                    <span>分析动态图</span>
                </h4>
                <!--<a class="change" href="SellCenter.html">
                    <i class="fa fa-reorder"></i>
                    切换奶企
                </a>-->
            </div>
            <div class="common_main">
                <div class="navlist">
                    <dl>
                        <dt>分析<em class="fa fa-cog"></em></dt>
                        <dd>
                            <a class="change acolor"><i class="change icolor fa fa-line-chart"></i>成交动态图</a>
                        </dd>
                    </dl>
                </div>
                <section class="jiqlist">
                    <dl class="jiqlistTab">
                        <dd class="change ddcolor"><b>成交动态趋势</b></dd>
                        <dd class="change"><b>订单状况与消费排名</b></dd>
                    </dl>
                    <div class="jiqlisttable change" style="z-index: 1">
                        <div id="container" style="width: 100%;left: -80px;"></div>
                    </div>
                    <div class="jiqlisttable change">
                        <div id="container1" style="width: 50%;"></div>
                        <div id="container2" style="width: 50%;"></div>
                    </div>
                </section>
            </div>
        </div>
    </div>
    <!--<div class="login_foot">
        <span>青岛冰格科技公司版权所有 翻版必究</span>
    </div>-->
</body>
</html>
<script type="text/javascript" src="http://echarts.baidu.com/gallery/vendors/echarts/echarts.min.js"></script>
<script type="text/javascript" src="http://echarts.baidu.com/gallery/vendors/echarts-gl/echarts-gl.min.js"></script>
<script type="text/javascript" src="http://echarts.baidu.com/gallery/vendors/echarts-stat/ecStat.min.js"></script>
<script type="text/javascript" src="http://echarts.baidu.com/gallery/vendors/echarts/extension/dataTool.min.js"></script>
<script type="text/javascript" src="http://echarts.baidu.com/gallery/vendors/echarts/map/js/china.js"></script>
<script type="text/javascript" src="http://echarts.baidu.com/gallery/vendors/echarts/map/js/world.js"></script>
<script type="text/javascript" src="http://api.map.baidu.com/api?v=2.0&ak=ZUONbpqGBsYGXNIYHicvbAbM"></script>
<script type="text/javascript" src="http://echarts.baidu.com/gallery/vendors/echarts/extension/bmap.min.js"></script>
<script type="text/javascript" src="http://echarts.baidu.com/gallery/vendors/simplex.js"></script>


<script type="text/javascript">    //成交表 
    var dom = document.getElementById("container");
    var myChart = echarts.init(dom);
    var app = {};
    option = null;
    app.title = '多 X 轴示例';

    var colors = ['#5793f3', '#d14a61', '#675bba'];


    option = {
        color: colors,

        tooltip: {
            trigger: 'none',
            axisPointer: {
                type: 'cross'
            }
        },
        legend: {
            data: ['订购成交', '零售成交']
        },
        grid: {
            top: 70,
            bottom: 50
        },
        xAxis: [
            {
                type: 'category',
                axisTick: {
                    alignWithLabel: true
                },
                axisLine: {
                    onZero: false,
                    lineStyle: {
                        color: colors[1]
                    }
                },
                axisPointer: {
                    label: {
                        formatter: function(params) {
                            return '零售成交  ' + params.value
                                + (params.seriesData.length ? '：' + params.seriesData[0].data : '');
                        }
                    }
                },
                data: ["2018-01-01", "2018-01-02", "2018-01-03", "2018-01-04", "2018-01-05", "2018-01-06", "2018-01-07", "2018-01-08", "2018-01-09", "2018-01-10", "2018-01-11", "2018-01-12", "2018-01-13", "2018-01-14", "2018-01-15", "2018-01-16", "2018-01-17", "2018-01-18", "2018-01-19", "2018-01-20", "2018-01-21", "2018-01-22", "2018-01-23", "2018-01-24", "2018-01-25", "2018-01-26", "2018-01-27", "2018-01-28", "2018-01-29", "2018-01-30", "2018-01-31"]
            },
            {
                type: 'category',
                axisTick: {
                    alignWithLabel: true
                },
                axisLine: {
                    onZero: false,
                    lineStyle: {
                        color: colors[0]
                    }
                },
                axisPointer: {
                    label: {
                        formatter: function(params) {
                            return '订购成交  ' + params.value
                                + (params.seriesData.length ? '：' + params.seriesData[0].data : '');
                        }
                    }
                },
                data: ["2018-01-01", "2018-01-02", "2018-01-03", "2018-01-04", "2018-01-05", "2018-01-06", "2018-01-07", "2018-01-08", "2018-01-09", "2018-01-10", "2018-01-11", "2018-01-12", "2018-01-13", "2018-01-14", "2018-01-15", "2018-01-16", "2018-01-17", "2018-01-18", "2018-01-19", "2018-01-20", "2018-01-21", "2018-01-22", "2018-01-23", "2018-01-24", "2018-01-25", "2018-01-26", "2018-01-27", "2018-01-28", "2018-01-29", "2018-01-30", "2018-01-31"]
            }
        ],
        yAxis: [
            {
                type: 'value'
            }
        ],
        series: [
            {
                name: '订购成交',
                type: 'line',
                xAxisIndex: 1,
                smooth: true,
                data: [2.6, 5.9, 9.0, 26.4, 28.7, 70.7, 175.6, 182.2, 48.7, 18.8, 6.0, 2.3, 26.4, 26.4, 28.7, 18.8, 6.0, 2.3, 48.7, 5.9, 9.0, 182.2, 48.7, 18.8, 18.8, 6.0, 2.3, 182.2, 48.7, 18.8, 182.2, 48.7]
            },
            {
                name: '零售成交',
                type: 'line',
                smooth: true,
                data: [3.9, 5.9, 11.1, 18.7, 48.3, 69.2, 231.6, 46.6, 55.4, 18.4, 10.3, 0.7, 70.7, 175.6, 182.2, 48.7, 18.8, 6.0, 2.3, 26.4, 26.4, 28.7, 18.8, 6.0, 2.3, 3.9, 5.9, 11.1, 18.7, 48.3, 69.2]
            }
        ]
    };;
    if (option && typeof option === "object") {
        myChart.setOption(option, true);
    }
</script>

<script type="text/javascript">  //订单表
    var dom = document.getElementById("container1");
    var myChart = echarts.init(dom);
    var app = {};
    option = null;
    option = {
        title : {
            text: '各商品销量比例情况',
            subtext: '近一月销量',
            x:'center'
        },
        tooltip : {
            trigger: 'item',
            formatter: "{a} <br/>{b} : {c} ({d}%)"
        },
        legend: {
            orient: 'vertical',
            left: 'left',
            data: ['蒙牛-特仑苏', '伊利-金典', '光明', '雀巢', '旺仔', '完达山', '维他奶', '晨光', '卫岗']
        },
        series : [
            {
                name: '访问来源',
                type: 'pie',
                radius : '55%',
                center: ['50%', '60%'],
                data:[
                    { value: 335, name: '蒙牛-特仑苏' },
                    { value: 310, name: '伊利-金典' },
                    { value: 234, name: '光明' },
                    { value: 135, name: '雀巢' },
                    { value: 1548, name: '旺仔' },
                    { value: 351, name: '完达山' },
                    { value: 864, name: '维他奶' },
                    { value: 125, name: '晨光' },
                    { value: 675, name: '卫岗' }
                ],
                itemStyle: {
                    emphasis: {
                        shadowBlur: 10,
                        shadowOffsetX: 0,
                        shadowColor: 'rgba(0, 0, 0, 0.5)'
                    }
                }
            }
        ]
    };
    ;
    if (option && typeof option === "object") {
        myChart.setOption(option, true);
    }
</script>

<script type="text/javascript">  //消费表
    var dom = document.getElementById("container2");
    var myChart = echarts.init(dom);
    var app = {};
    option = null;
    app.title = '各省消费排行';

    option = {
        title: {
            text: '各省消费排行',
            subtext: '近一月消费排行'
        },
        tooltip: {
            trigger: 'axis',
            axisPointer: {
                type: 'shadow'
            }
        },
        legend: {
            data: ['订购', '零售']
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
            data: ['北京市','天津市','河北省','山东省','黑龙江省','吉林省','辽宁省','内蒙古','陕西省','山西省','新疆维吾尔族自治区','西藏自治区','青海省','福建省','浙江省','江苏省','河南省','湖北省','湖南省']
        },
        series: [
            {
                name: '订购',
                type: 'bar',
                data: [18203, 23489, 29034, 104970, 131744, 630230, 23489, 29034, 104970, 131744, 630230, 23489, 29034, 104970, 131744, 630230, 630230, 23489, 29034]
            },
            {
                name: '零售',
                type: 'bar',
                data: [19325, 23438, 31000, 121594, 134141, 681807, 23438, 31000, 121594, 134141, 681807, 23438, 31000, 121594, 134141, 681807, 121594, 134141, 681807]
            }
        ]
    };
    ;
    if (option && typeof option === "object") {
        myChart.setOption(option, true);
    }
</script>


<script>
    $(function() {
        $(".jiqlistTab").find("dd").click(function() {
            $(".jiqlistTab dd").removeClass("ddcolor");
            $(this).addClass("ddcolor");
            var $liNum = $(this).index();
            $(".jiqlisttable").css({ "z-index": "-99","opacity":"0" });
            $(".jiqlisttable").eq($liNum).css({ "z-index": "1", "opacity": "1" });
        });
    });
</script>
<script>
    $(function() {
        $("#li5").find("a").addClass("aborder");
    })
</script>
