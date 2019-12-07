<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="big_screen.aspx.cs" Inherits="autosell_center.main.Big_screen.big_screen" %>
<%@ Register Src="~/ascx/CheckboxListControl.ascx" TagName="CheckboxListControl" TagPrefix="uc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>自动售卖终端实时监控大屏</title>
    <link rel="shortcut icon" href="/main/public/images/favicon.ico" />
    <link href="/main/public/css/common.css" rel="stylesheet" />
    <link href="public/font/iconfont.css" rel="stylesheet" />
    <link href="public/screenFont/font.css" rel="stylesheet" />
    <link href="public/Big_screenStyle.css" rel="stylesheet" />
    <script src="../public/script/jquery-3.2.1.min.js"></script>
    <script src="public/Big_screenJs.js"></script>
     <link  href="../../scripts/skin/jedate.css" rel="stylesheet" type="text/css"/>
    <script src="../../scripts/jedate.js" type="text/javascript"></script>
    <script src="../../scripts/jedate.min.js"></script>
    <!-- 左侧滚动显示 -->
    <script src="public/scroll.js"></script>
    
    <style>
        .news {
            height: 201px;
        }

        .box {
            width: 300px;
            height: 200px;
        }

        .bcon {
            width: 300px;
            height: 200px;
            overflow: hidden;
        }
    </style>
    <script type="text/javascript">
        $(function () {
          
            $("div.list_lh").myScroll({
                speed: 80, //数值越大，速度越慢
                rowHeight: $(".news div").height() //li的高度
            });
            jeDate({
                dateCell: "#start", //isinitVal:true,
                format: "YYYY-MM",
                isTime: false, //isClear:false,
                choose: function (val) { },
                minDate: "2014-09-19"
            });
            jeDate({
                dateCell: "#end",
                format: "YYYY-MM",
                isinitVal: true,
                isTime: true, //isClear:false,
                minDate: "2014-09-19"
            });
        });
       
    </script>
    <!-- 地图引用 -->
    <link rel="stylesheet" href="http://cache.amap.com/lbs/static/main1119.css" />
    <script type="text/javascript" src="http://webapi.amap.com/maps?v=1.4.5&amp;key=c06fb704492b7033999605e4311caba3"></script>
    <script type="text/javascript" src="http://cache.amap.com/lbs/static/addToolbar.js"></script>
    <style>
        .amap-controls {
            display: none;
        }
        #screenSeachChart div:first-child, #screenSeachChart canvas {
            height: 32vh !important;
        }
        #screenSeachChart {
            width: 100% !important;
        }
    </style>

    <!-- Echarts插件 -->
    <%--<script type="text/javascript" src="http://echarts.baidu.com/gallery/vendors/echarts/echarts.min.js"></script>--%>
    <script type="text/javascript" src="http://echarts.baidu.com/examples/vendors/echarts/echarts.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <!-- 消息推送是否滚动提示 -->
        <span id="indexNewsShowIs_"></span>
        <!-- 全屏显示数据图表 -->
        <div class="EchartBig change">
            <h2>销售量、销售金额、订单数数据查询<em class="icon iconfont icon-Icon_suoxiao" onclick="DataSeachSmall()"></em></h2>
            <iframe src="echartMain.aspx" id="bigechartIframe" class="echartIframe"></iframe>
            <%--<div id="screenSeachChartBig" style="height: 100%"></div>--%>
        </div>
        <div class="commonBg"></div>
        <main class="screenmain">
            <section class="screenTitle">
                <h4>自动售卖终端实时监控大屏</h4>
                <p id="getTime"></p>
                <div class="button-group2 change">
                    <input type="checkbox" onclick="addorremove()" checked name="bg" />背景
                    <input type="checkbox" onclick="addorremove()" checked name="road" />道路
                    <input type="checkbox" onclick="addorremove()" checked name="building" />建筑物
                    <input type="checkbox" onclick="addorremove()" checked name="point" />标注
                </div>
                <input type="button" value="隐藏数据图层" id="DataCengBtn" onclick="dataCengBtn(this)" />
                <div class="button-group">
                    <input id="cityName" class="inputtext" placeholder="请输入城市的名称" type="text" />
                    <input id="query" class="button" value="到指定的城市" type="button" />
                </div>
            </section>
            <span class="screenNewsShow">
                <a id="newsShowMain"></a>
                
            </span>
            <section class="screenBigData">
                <dl>
                    <dd>
                        <span>总销售量</span>
                        <h4><b id="_totalDealNum"></b><em>件</em></h4>
                        <input type="hidden" value="0" />
                    </dd>
                    <dd>
                        <span>总销售额</span>
                        <h4><b id="_totalMoney"></b><em>元</em></h4>
                        <input type="hidden" value="0"/>
                    </dd>
                    <dd>
                        <span>总订单数</span>
                       
                         <h4><b id="_totalOrderNum"></b><em>笔</em></h4>
                        <input type="hidden" value="0" />
                    </dd>
                </dl>
            </section>
            <section class="screenOrder">
                <h2>今日机器订单信息</h2>
                <div class="list_lh">
                    <div class="news">
                        <ul id="ull1">
                          <%--  <li>2018-03-27 03:35:45机器编号<em>GXT3546622</em>有新订单创建成功，该机器属<i>伊利集团</i>。 </li>--%>
                        </ul>
                    </div>
                </div>
                <div class="commonMainBg change">
                    <i></i><em></em>
                    <i></i><em></em>
                    <i></i><em></em>
                    <i></i><em></em>
                </div>
            </section>
            <section class="screenHot">
                <h2>热销商品
                <span>
                    <input type="text" value="" placeholder="搜索企业..." id="_companyName"/>
                    <i class="icon iconfont icon-Icon_seach" onclick="getHotProduct()" style="cursor:pointer"></i>
                </span>
                </h2>
                <div class="commonOvl">
                    <ul class="screenCommonUl" id="_hotProduct">
                      
                    </ul>
                </div>
                <div class="commonMainBg change">
                    <i></i><em></em>
                    <i></i><em></em>
                    <i></i><em></em>
                    <i></i><em></em>
                </div>
            </section>
            <section class="screenNotice">
                <ul class="screenCommonUl" id="brokenList">
                    <li>
                        <label>级别</label>
                        <label>机器编号</label>
                        <label>告警时间</label>
                        <label>告警内容</label>
                        <label>当前状态</label>
                       <%-- <label>操作</label>--%>
                    </li>
                    <li>
                        <span>
                            <b class="red">紧急</b>
                        </span>
                        <span>GTX185415254</span>
                        <span>2018-02-03 16:24:15</span>
                        <span class="screenNoticeMain">机器温度过高告警</span>
                        <span class="screenNoticeZttz">停止</span>
                       <%-- <span>
                            <a onclick="screenNoticeBtn(this)">详细...</a>
                        </span>--%>
                        <input type="hidden" value="2" class="screenNoticeHidden" />
                    </li>
                    <li>
                        <span>
                            <b class="yellow">警告</b>
                        </span>
                        <span>GTX185415254</span>
                        <span>2018-02-03 16:24:15</span>
                        <span class="screenNoticeMain">机器温度过高告警</span>
                        <span class="screenNoticeZt">运行中</span>
                       <%-- <span>
                            <a onclick="screenNoticeBtn(this)">详细...</a>
                        </span>--%>
                        <input type="hidden" value="1" class="screenNoticeHidden" />
                    </li>
                    <li>
                        <span>
                            <b class="blue">处理中</b>
                        </span>
                        <span>GTX185415254</span>
                        <span>2018-02-03 16:24:15</span>
                        <span class="screenNoticeMain">机器温度过高告警</span>
                        <span class="screenNoticeZttz">停止</span>
                       <%-- <span>
                            <a onclick="screenNoticeBtn(this)">详细...</a>
                        </span>--%>
                        <input type="hidden" value="0" class="screenNoticeHidden" />
                    </li>
                    <li>
                        <h5>共555条记录</h5>
                        <p>
                            <input type="button" value="上一页" class="screenNoticePrev" />
                            <a id="onThis">1</a>
                            <a>2</a>
                            <a>3</a>
                            <input type="button" value="下一页" class="screenNoticeNext" />
                        </p>
                    </li>
                </ul>
                <div class="commonMainBg change">
                    <i></i><em></em>
                    <i></i><em></em>
                    <i></i><em></em>
                    <i></i><em></em>
                </div>
            </section>
            <section class="screenJiqiList">
                <h2>机器列表
                    <span>
                        <a  onclick="mechineList1(1)">按销量</a>
                        <a  onclick="mechineList1(2)">按销售额</a>
                        <a  onclick="mechineList1(3)">按订单</a>
                    </span>
                </h2>
                <div class="commonOvl">
                    <ul class="screenCommonUl" id="mechList">
                        <li>
                            <label></label>
                            <label>机器编号</label>
                            <label>所属企业</label>
                            <label>机器地址</label>
                        </li>
                        <li>
                            <span class="screenNum">1</span>
                            <span>GTX2546664484</span>
                            <span>伊利集团</span>
                            <span>山东省青岛市黄岛区中心街道中央广场4层</span>
                        </li>
                        <li>
                            <span class="screenNum">1</span>
                            <span>GTX2546664484</span>
                            <span>伊利集团</span>
                            <span>山东省青岛市黄岛区中心街道中央广场4层</span>
                        </li>
                        <li>
                            <span class="screenNum">1</span>
                            <span>GTX2546664484</span>
                            <span>伊利集团</span>
                            <span>山东省青岛市黄岛区中心街道中央广场4层</span>
                        </li>
                        <li>
                            <span class="screenNum">1</span>
                            <span>GTX2546664484</span>
                            <span>伊利集团</span>
                            <span>山东省青岛市黄岛区中心街道中央广场4层</span>
                        </li>
                    </ul>
                </div>
                <div class="commonMainBg change">
                    <i></i><em></em>
                    <i></i><em></em>
                    <i></i><em></em>
                    <i></i><em></em>
                </div>
            </section>
            <section class="screenNewsList">
                <h2>消息实时推送
                <span>
                    <b class="Off_On">
                        <i class="change icolor"></i>
                        <em class="change emcolor"></em>
                        <input type="hidden" value="1" />
                    </b>
                    <a onclick="NewsListMore()">查看全部 ></a>
                </span>
                </h2>
                <div class="maquee">
                    <ul id="ssts">
                        <li>会员啷个哩个啷°于2018-03-27 03:35:45登记成功，目前累计消费0元，累计充值0元，账户余额0元.</li>
                        <li>会员啷个哩个啷°于2018-03-27 03:35:45登记成功，目前累计消费0元，累计充值0元，账户余额0元.</li>
                        <li>会员啷个哩个啷°于2018-03-27 03:35:45登记成功，目前累计消费0元，累计充值0元，账户余额0元.</li>
                        <li>会员啷个哩个啷°于2018-03-27 03:35:45登记成功，目前累计消费0元，累计充值0元，账户余额0元.</li>
                        <li>会员啷个哩个啷°于2018-03-27 03:35:45登记成功，目前累计消费0元，累计充值0元，账户余额0元.</li>
                        <li>会员啷个哩个啷°于2018-03-27 03:35:45登记成功，目前累计消费0元，累计充值0元，账户余额0元.</li>
                    </ul>
                </div>
                <div class="commonMainBg change">
                    <i></i><em></em>
                    <i></i><em></em>
                    <i></i><em></em>
                    <i></i><em></em>
                </div>
            </section>
            <section class="screenDataSeach">
                <h2>销售量、销售金额、订单数据查询
                    <span>
                        <em class="icon iconfont icon-Icon_seach" onclick="DataSeachBtn()"></em>
                        <em class="icon iconfont icon-Icon_shuaxin" onclick="DataSeachShuan()"></em>
                        <em class="icon iconfont icon-I_con_fangda" onclick="DataSeachBig()"></em>
                    </span>
                </h2>
                <div class="change screenseachMain">
                    <h4>选择检索条件进行检索<em class="icon iconfont icon-Icon_off" onclick="offscreenseachMain()"></em></h4>
                    <ul>
                         <li>
                              <label>企业</label>
                             <span>
                                  <select id="companyList" onchange="comchg()" style="width:45%"> </select>
                                  <uc1:CheckboxListControl ID="cbosDeparentment" runat="server" style="width:45%;float:right"/>
                             </span>
                              
                          </li>
                         
                        <li>
                            <label>地区</label>
                            <span id="province">
                                <select id="s_province" name="s_province"></select>
                                <select id="s_city" name="s_city"></select>
                                <select id="s_county" name="s_county"></select>
                            </span>
                        </li>
                        <li>
                            <label>时间段</label>
                            <span>
                                 <input name="act_stop_timeks" type="text" id="start" runat="server" class="input" value="" placeholder="开始时间" readonly="true" />
                            </span>
                        </li>
                        <li>
                            <label>-</label>
                            <span>
                               <input name="act_stop_timeks" type="text" id="end"   runat="server"  class="input" value="" placeholder="注册时间"  readonly="true"  />
                            </span>
                        </li>
                        <li>
                            <label>销售类型</label>
                            <span>
                                <select id="sxType">
                                    <option value="0">全部</option>
                                    <option value="1">订购</option>
                                    <option value="2">零售</option>
                                </select>
                            </span>
                        </li>
                         <li>
                            <label>性别/年龄</label>
                            <span id="people">
                                <select id="sex" name="sex" style="width:45%">
                                    <option value="全部">全部</option>
                                     <option value="男">男</option>
                                     <option value="女">女</option>
                                </select>
                               
                               <select id="age" style="width:45%;float:right">

                               </select>
                            </span>
                        </li>
                        <li>
                            <input type="button" value="查询" id="screenSeachBtn" onclick="screenSeachBtnchange()" />
                        </li>
                    </ul>
                </div>
                <p>搜索条件“<em id="seachTiaojian">北京市</em>”各个数据显示情况如下:</p>
                <dl class="screenSeachData">
                    <dd>
                        <span>销售量</span>
                        <p id="totalXL"></p>
                    </dd>
                    <dd>
                        <span>销售金额</span>
                        <p id="totalMoney">0元</p>
                    </dd>
                    <dd>
                        <span>订单数</span>
                        <p id="totalDD">0笔</p>
                    </dd>
                </dl>
                <div class="screenDataSeachChart">
                    <iframe src="echartMain.aspx" id="smallechartIframe" class="echartIframe"></iframe>
                </div>
                <div class="commonMainBg change">
                    <i></i><em></em>
                    <i></i><em></em>
                    <i></i><em></em>
                    <i></i><em></em>
                </div>
            </section>
            <div class="screenMap">
                <div id="container" class="map" tabindex="0"></div>
            </div>
        </main>
    </form>
</body>
</html>
<script type="text/javascript">
    $(function () {
        refresh();
        refreshShow();
        getBrokenDownList();
        getCompanyList();
        mechineList1(1);
        ssts();
    })
    //初始化地图对象，加载地图
    var map = new AMap.Map('container', {
        resizeEnable: true,
        center: [107.623475, 33.755651],//地图中心点
        zoom: 5//地图显示的缩放级别
    });
    function refresh(enName) {
        map.setMapStyle('amap://styles/blue');
    };

    function addorremove() {
        var boxes = document.getElementsByTagName('input');
        var features = [];
        for (var i = 0; i < boxes.length; i += 1) {
            if (boxes[i].checked === true) {
                features.push(boxes[i].name);
            }
        }
        map.setFeatures(features);
    }

    //异常机器
    var markers = [], positions = [<%=ycposttion%>]
    for (var i = 0, marker; i < positions.length; i++) {
        
        marker = new AMap.Marker({
            map: map,
            icon: "public/centergz.gif",
            //icon: 'http://webapi.amap.com/theme/v1.3/markers/n/mark_b' + (i + 1) + '.png', // 显示排序编号
            position: positions[i]
        });
        markers.push(marker);
        // 设置点标记的动画效果，此处为弹跳效果
        //marker.setAnimation('AMAP_ANIMATION_BOUNCE');
    }
    //设置城市
    AMap.event.addDomListener(document.getElementById('query'), 'click', function () {
        var cityName = document.getElementById('cityName').value;
        if (!cityName) {
            cityName = '北京市';
        }
        map.setCity(cityName);
    });
    //[116.405467, 29.907761], [106.415467, 19.907761], [56.415467, 39.917761], [35.425467, 49.907761], [42.85467, 35.907761]  正常机器
    var markers = [], positions = [<%=position%>]
    for (var i = 0, marker; i < positions.length; i++) {
        marker = new AMap.Marker({
            map: map,
            icon: "public/center.gif",
            //icon: 'http://webapi.amap.com/theme/v1.3/markers/n/mark_b' + (i + 1) + '.png', // 显示排序编号
            position: positions[i]
        });
        markers.push(marker);
        // 设置点标记的动画效果，此处为弹跳效果
        //marker.setAnimation('AMAP_ANIMATION_BOUNCE');
    }
  
    function refreshShow() {
        var boxes = document.getElementsByTagName('input');
        // bg背景，road道路，building建筑物，point标注
        var features = ["bg", "road", "point"];
        for (var i = 0; i < boxes.length; i += 1) {
            if (boxes[i].checked === true) {
                features.push(boxes[i].name);
            }
        }
        map.setFeatures(features);
    }
</script>

<script type="text/javascript">
    // 右侧逐条轮播显示
    function autoScroll(obj) {
        $(obj).find("ul").animate({
            marginTop: "-3.1vh"
        }, 1000, function () {
            $(this).css({ marginTop: "0px" }).find("li:first").appendTo(this);
        });
    }
    function fenxiang() {
        alert("功能开发中，敬请期待...");
    }

    $(function () {
        var scroll = setInterval('autoScroll(".maquee")', 3000);
        $(".maquee").hover(function () {
            console.log("aaa");
            clearInterval(scroll);
        }, function () {
            scroll = setInterval('autoScroll(".maquee")', 3000);
        });
        setAge();
        setInterval('excute()', 1000);
         
    });
    function setAge()
    {
        $("#age").append("<option value='0'>全部</option>");
        for (var i = 10; i <= 70;i++)
        {
            
            $("#age").append("<option value='"+i+"'>"+i+"</option>");
        }
    }
    function excute()
    {
        getNowFormatDate();
        getTotalNum();
        getOrderList();
        getBrokenDownListNew();
    }
    function getNowFormatDate() {
         
        var date = new Date();
        var seperator1 = "-";
        var seperator2 = ":";
        var month = date.getMonth() + 1;
        var strDate = date.getDate();
        if (month >= 1 && month <= 9) {
            month = "0" + month;
        }
        if (strDate >= 0 && strDate <= 9) {
            strDate = "0" + strDate;
        }
        var currentdate = date.getFullYear() + seperator1 + month + seperator1 + strDate
                + " " + date.getHours() + seperator2 + date.getMinutes()
                + seperator2 + date.getSeconds();
        $("#getTime").html(currentdate);
        return currentdate;
    }
    function getTotalNum()
    {
        $.ajax({
            type: "post",
            url: "big_screen.aspx/getTotalNum",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{}",
            success: function (data) {
                var arr = data.d.split('|');
                $("#_totalDealNum").html(arr[0]);
                $("#_totalOrderNum").html(arr[1]);
                $("#_totalMoney").html(arr[2]);
            }
        })
    }
    function getOrderList()
    {
        //$("#ull1").empty();
        $.ajax({
            type: "post",
            url: "big_screen.aspx/getOrderList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{}",
            success: function (data) {
                var serverdata = $.parseJSON(data.d);
                var serverdatalist = serverdata.length;
                for (var i = 0; i < serverdatalist; i++)
                {
                    if (serverdata[i].type == "1") {
                        $(" <li>" + serverdata[i].createTime + "机器编号<em>" + serverdata[i].bh + "</em>有新订单创建成功，该机器属<i>" + serverdata[i].name + "</i>。 </li>").appendTo("#ull1");
                    } else if (serverdata[i].type == "2") {
                        if (serverdata[i].bz == "交易成功") {
                            $(" <li>" + serverdata[i].createTime + "机器编号<em>" + serverdata[i].bh + "</em>有新商品售卖成功，该机器属<i>" + serverdata[i].name + "</i>。 </li>").appendTo("#ull1");
                        } else {
                            $(" <li style='color:red'>" + serverdata[i].createTime + "机器编号<em>" + serverdata[i].bh + "</em>有新商品售卖,机器故障：" + serverdata[i].bz + "，该机器属<i>" + serverdata[i].name + "</i>。 </li>").appendTo("#ull1");
                        }
                      
                    }
                    
                }
            }
        })
    }
    function getHotProduct()
    {
        $("#_hotProduct").val();
        $("  <li>"
                  +"<label></label>"
                  +"<label>产品名称</label>"
                  +"<label>产品类别</label>"
                  +"<label>所属奶企</label>"
                  +"</li>").appendTo("_hotProduct");
        $.ajax({
            type: "post",
            url: "big_screen.aspx/getHotProduct",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{name:'" + $("#_companyName").val() + "'}",
            success: function (data) {
                var serverdata = $.parseJSON(data.d);
                var serverdatalist = serverdata.length;
                for (var i = 0; i < serverdatalist; i++) {
                    $(" <li>"
                           +" <span class='screenNum'>1</span>"
                           + " <span>" + serverdata[i].proName + "</span>"
                           + " <span>" + serverdata[i].typeName + "</span>"
                           + " <span>" + serverdata[i].companyName + "</span>"
                        +"</li>").appendTo("#_hotProduct")
                }
            }
        })
    }
    function getBrokenDownList()
    {
        $("#brokenList").empty();
        $(" <li>"
                 +"       <label>级别</label>"
                 +"       <label>机器编号</label>"
                 +"       <label>告警时间</label>"
                  +"      <label>告警内容</label>"
                  +"      <label>当前状态</label>"
                 +"   </li>").appendTo("#brokenList");
        $.ajax({
            type: "post",
            url: "big_screen.aspx/getBrokenDownList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{name:'" + $("#_companyName").val() + "'}",
            success: function (data) {
                var serverdata = $.parseJSON(data.d);
                var serverdatalist = serverdata.length;
                for (var i = 0; i < serverdatalist; i++) {
                    if(serverdata[i].statu=="1")
                    {
                        $(" <li>"
                        +"<span>"
                         +"   <b class='red'>紧急</b>"
                        +"</span>"
                        + "<span>" + serverdata[i].bh + "</span>"
                        + "<span>" + serverdata[i].brokenTime + "</span>"
                        + "<span class='screenNoticeMain'>" + serverdata[i].brokenName + "</span>"
                        + "<span class='screenNoticeZttz'>" + serverdata[i].runStatu + "</span>"
                       <%-- <span>
                            <a onclick="screenNoticeBtn(this)">详细...</a>
                        </span>--%>
                        +"<input type='hidden' value='2' class='screenNoticeHidden' />"
                    +"</li>").appendTo("#brokenList");
                    }else if(serverdata[i].statu=="2"){
                         $(" <li>"
                                    +"<span>"
                                     +"     <b class='yellow'>警告</b>"
                                    +"</span>"
                                   + "<span><a onclick='getPath('" + serverdata[i].id + "')'>" + serverdata[i].bh + "</a></span>"
                                    + "<span>" + serverdata[i].brokenTime + "</span>"
                                    + "<span class='screenNoticeMain'>" + serverdata[i].brokenName + "</span>"
                                    + "<span class='screenNoticeZttz'>" + serverdata[i].runStatu + "</span>"
                                   <%-- <span>
                                        <a onclick="screenNoticeBtn(this)">详细...</a>
                                    </span>--%>
                                    +"<input type='hidden' value='2' class='screenNoticeHidden' />"
                                +"</li>").appendTo("#brokenList");
                  }
                   

                }
            }
        })
    }
    function getBrokenDownListNew()
    {
        $("#newsShowMain").empty();
        $.ajax({
            type: "post",
            url: "big_screen.aspx/getBrokenDownListNew",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{}",
            success: function (data) {
               
                var serverdata = $.parseJSON(data.d);
                var serverdatalist = serverdata.length;
                var bh = "";
                var time = "";
                var sta = "";
                for (var i = 0; i < serverdatalist; i++) {
                    bh = serverdata[i].bh;
                    time = serverdata[i].brokenTime;
                    sta = serverdata[i].brokenName;
                    $("有新告警：机器" + bh + "于" + time + "发出" + sta + "警告，已通知机器管理员前往...").appendTo("#newsShowMain");

                }
              
            }
            
        })
    }
    function getCompanyList() {
        $.ajax({
            type: "post",
            url: "big_screen.aspx/getCompanyList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{}",
            success: function (data) {
                var serverdata = $.parseJSON(data.d);
                var serverdatalist = serverdata.length;
                for (var i = 0; i < serverdatalist; i++) {
                    $(" <option value='" + serverdata[i].id + "'>" + serverdata[i].name + "</option>").appendTo("#companyList");
                }
            }
        })
    }
    function comchg() {
        $("#txtcboName").val("");
        $("#cbosDeparentment_hdscbo").val("");
        $("#divCheckBoxList").empty();
        $.ajax({
            type: "post",
            url: "big_screen.aspx/getMechineList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{companyID:'" + $("#companyList").val() + "'}",
            success: function (data) {
                var serverdata = $.parseJSON(data.d);
                var serverdatalist = serverdata.length;
                for (var i = 0; i < serverdatalist; i++) {
                    $("<div><input type='checkbox' name='subBox' onclick='changeinfo()' value='" + serverdata[i].sCode + "'>" + serverdata[i].sName + "</div>").appendTo("#divCheckBoxList");
                }
            }
        })
    }
    function screenSeachBtnchange() {
        var province = $("#s_province").val();
        var city = $("#s_city").val();
        var country = $("#s_county").val();
        var tj = "";
        if(province!="省份")
        {
            tj += province+",";
        }
        if (city != "地级市") {
            tj += city + ",";
        }
        if (country != "市、县级市") {
            tj += country + ",";
        }
        if($("#start").val()!="")
        {
            tj += $("#start").val() + ",";
        }
        if ($("#end").val() != "") {
            tj += $("#end").val() + ",";
        }
        //jquery给iframe src赋值
         
        $("#smallechartIframe").attr('src', 'echartMain.aspx?province=' + province + "&city=" + city + "&country=" + country + "&start=" + $("#start").val() + "&end=" + $("#end").val() + "&sxType=" + $("#sxType").val() + "&companyID=" + $("#companyList").val() + "&mechineIDStr=" + $("#cbosDeparentment_hdscbo").val()+"&sex="+$("#sex").val()+"&age="+$("#age").val());
        //$('iframe').attr('src', 'src的值');
       
        tj += $("#sxType").find("option:selected").text();
        $(".screenseachMain").removeClass("screenseachMainH");
        var thisHtml = "";
        $("#seachTiaojian").html(tj.substring(0,tj.length-1));
    }
    function mechineList1(type)
    {
        $("#mechList").empty();
        $(" <li>"
                +"<label></label>"
                +"<label>机器编号</label>"
                +"<label>所属企业</label>"
                +"<label>机器地址</label>"
                +"</li>").appendTo("#mechList");
        $.ajax({
            type: "post",
            url: "big_screen.aspx/mechineList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{type:'"+type+"'}",
            success: function (data) {
                var serverdata = $.parseJSON(data.d);
                var serverdatalist = serverdata.length;
                for (var i = 0; i < serverdatalist; i++) {
                    $("  <li> "
                         +"   <span class='screenNum'>1</span>"
                         + "   <span>" + serverdata[i].bh + "</span>"
                         + "   <span>" + serverdata[i].companyName + "</span>"
                         + "   <span>" + serverdata[i].addres + "</span>"
                        +"</li>").appendTo("#mechList");
                }
            }
        })
    }
    function ssts()
    {
        $("#ssts").empty();
        $.ajax({
            type: "post",
            url: "big_screen.aspx/ssts",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{}",
            success: function (data) {
                var serverdata = $.parseJSON(data.d);
                var serverdatalist = serverdata.length;
                for (var i = 0; i < serverdatalist; i++) {
                    $("  <li>" + serverdata[i].name + "于" + serverdata[i].time + "登记成功，目前累计消费" + serverdata[i].sumConsume + "元，累计充值" + serverdata[i].sumRecharge + "元，账户余额" + serverdata[i].AvailableMoney + "元.</li>").appendTo("#ssts");
                }
            }
        })
    }
    function getPosition() {
        $.ajax({
            type: "post",
            url: "big_screen.aspx/getPosition",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{}",
            success: function (data) {
                
            }
        })
    }
    function getPath(id) {
        $.ajax({
            type: "post",
            url: "big_screen.aspx/getPath",
            contentType: "application/json; charset=utf-8",
            data: "{id:'" + id + "'}",
            dataType: "json",
            success: function (data) {
                if (data.d != "") {

                    $("#vid").attr("src", data.d);
                } else {
                    alert("找不到视频资源");
                }
            }

        });
        $("#updataCaDiv").addClass("addDivshow");
        setTimeout(function () {
            $(".popupbj").fadeIn();
        }, 100);
    }
</script>

<!-- 全国省市县三级联动 -->
<script src="public/area.js"></script>
<script type="text/javascript">_init_area();</script>
<script type="text/javascript">
    //  全国省市县三级联动
    var Gid = document.getElementById;
    var showArea = function () {
        Gid('show').innerHTML = "<h3>省" + Gid('s_province').value + " - 市" +
            Gid('s_city').value + " - 县/区" +
            Gid('s_county').value + "</h3>";
    }
    Gid('s_county').setAttribute('onchange', 'showArea()');
</script>
