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
                <p id="getTime">2018-03-02 12:45:55</p>
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
                <a id="newsShowMain">有新告警：机器GXT35445于2018-05-04 16:21:22发出温度过高警告，已微信通知机器管理员前往...</a>
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
                        <h4><b id="_totalOrderNum"></b><em>元</em></h4>
                        <input type="hidden" value="0"/>
                    </dd>
                    <dd>
                        <span>总订单数</span>
                       
                         <h4><b id="_totalMoney"></b><em>笔</em></h4>
                        <input type="hidden" value="0" />
                    </dd>
                </dl>
            </section>
            <section class="screenOrder">
                <h2>今日机器订单信息</h2>
                <div class="list_lh">
                    <div class="news">
                        <ul id="ull1">
                            
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
                    <input type="text" value="" placeholder="搜索企业..."  id="_companyName"/>
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
                        <label>操作</label>
                    </li>
                    <li>
                        <span>
                            <b class="red">紧急</b>
                        </span>
                        <span>GTX185415254</span>
                        <span>2018-02-03 16:24:15</span>
                        <span class="screenNoticeMain">机器温度过高告警</span>
                        <span class="screenNoticeZttz">停止</span>
                        <span>
                            <a onclick="screenNoticeBtn(this)">详细...</a>
                        </span>
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
                        <span>
                            <a onclick="screenNoticeBtn(this)">详细...</a>
                        </span>
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
                        <span>
                            <a onclick="screenNoticeBtn(this)">详细...</a>
                        </span>
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
                              <label>机器列表</label>
                             <span>
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
                                 <input name="act_stop_timeks" autocomplete="off" type="text" id="start" runat="server" class="input" value="" placeholder="开始时间" readonly="true" />
                            </span>
                        </li>
                        <li>
                            <label>-</label>
                            <span>
                                <input name="act_stop_timeks" autocomplete="off" type="text" id="end"   runat="server"  class="input" value="" placeholder="结束时间"  readonly="true"  />
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
                        <p id="totalMoney">362533.52元</p>
                    </dd>
                    <dd>
                        <span>订单数</span>
                        <p id="totalDD">36253笔</p>
                    </dd>
                </dl>
                <div class="screenDataSeachChart">
                    <iframe src="echartMain.aspx" id="smallechartIframe" class="echartIframe"></iframe>
                    <%--<div id="screenSeachChart" style="height: 100%"></div>--%>
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
        <input  id="_companyID" runat="server" type="hidden"/>
    </form>
</body>
</html>
<script type="text/javascript">
    $(function () {
        refresh();
        refreshShow();
        getBrokenDownList();
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

    //添加点标记，并使用自己的icon
    //new AMap.Marker({
    //    map: map,
    //    position: [117.5678367074579, 34.86445745937386],
    //    icon: new AMap.Icon({
    //        size: new AMap.Size(20, 20),  //图标大小
    //        image: "public/center.gif",
    //        imageOffset: new AMap.Pixel(0, 0)
    //    })
    //});

    //设置城市
    AMap.event.addDomListener(document.getElementById('query'), 'click', function () {
        var cityName = document.getElementById('cityName').value;
        if (!cityName) {
            cityName = '北京市';
        }
        map.setCity(cityName);
    });

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
         setInterval('excute()', 1000);
        //excute()
    });
    function excute() {
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
    function qx_judge(menuID) {

        //首先验证账号和密码正确
        $.ajax({
            url: "../../../ashx/asm.ashx",
            type: 'post',
            dataType: 'json',
            timeout: 10000,
            data: {
                action: "qx_judge",
                menu: menuID
            },
            success: function (resultData) {
                if (resultData.result == "ok")//允许查看跳转
                {
                    if (menuID == 'hylb') {//会员列表
                        location.href = "../member/memberlist.aspx";
                    }
                    if (menuID == 'jqddtj') {//订单管理
                       // location.href = "../order/orderform.aspx";
                    }
                    if (menuID == 'hyddgl') {//会员订单管理

                        location.href = "../order/order.aspx";
                    }
                    if (menuID == 'cpddtj') {//商品订单统计

                        location.href = "../order/Productform.aspx";
                    }
                    if (menuID == 'tjcp') {//添加商品

                        location.href = "../product/productadd.aspx";
                    }
                    if (menuID == 'gmjl') {//购买记录

                        location.href = "../equipment/orderlist.aspx";
                    }
                    if (menuID == 'cplb') {//商品列表
                        location.href = "../product/productlist.aspx";
                    }
                    if (menuID == 'sblb') {//设备管理
                        location.href = "../equipment/equipmentlist.aspx";
                    }
                    if (menuID == 'glygl') {//管理员管理
                        location.href = "../Administrators/adminlist.aspx";
                    }
                    if (menuID == 'jsgl') {//角色管理
                        location.href = "../Administrators/rolelist.aspx";
                    }
                    if (menuID == 'szhd') {//活动管理
                        location.href = "../activity/activity.aspx";
                    }
                    if (menuID == 'spgl') {//广告管理
                        location.href = "../Advertisement/video.aspx";
                    }
                    if (menuID == 'jqtjsp') {//机器添加视频
                        location.href = "../Advertisement/Jurisdiction.aspx";
                    }
                    if (menuID == 'gzhsz') {//公众号管理
                        location.href = "../enterprise/Thepublicjb.aspx";
                    }
                    if (menuID == 'mbxx') {//模板消息
                        location.href = "../enterprise/Distributor.aspx";
                    }
                    if (menuID == 'sjdp') {//数据大屏
                        
                       // window.open("/main/Big_screen/big_screen.aspx");
                    }
                    if (menuID == 'zhcx') {//数据统计与查询
                        location.href = "../datatj/Statisticalquery.aspx";
                    }
                } else if (resultData.result == "notLogin")//没有查看权限
                {

                    $(".jiqlist").empty();
                    $(".jiqlist").html("<p class='noquanxian'>当前没有权限</p>").css({ "background": "#ddd" });
                }
                else if (resultData.result == "1")//请联系管理员给当前登录角色赋值权限
                {
                    alert("请联系管理员给当前登录角色赋值权限");
                }
                else if (resultData.result == "2")//跳转重新登录
                {
                    location.href = "../../../../index.aspx";
                }

            }
        })
    }

    function getTotalNum() {
      
        $.ajax({
            type: "post",
            url: "big_screen.aspx/getTotalNum",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{companyID:'"+$("#_companyID").val()+"'}",
            success: function (data) {
                var arr = data.d.split('|');
               
                $("#_totalDealNum").html(arr[0]);
                $("#_totalOrderNum").html(arr[2]);
                $("#_totalMoney").html(arr[1]);
            }
        })
    }
    function getOrderList() {
   
        $.ajax({
            type: "post",
            url: "big_screen.aspx/getOrderList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{companyID:'" + $("#_companyID").val() + "'}",
            success: function (data) {
                var serverdata = $.parseJSON(data.d);
                var serverdatalist = serverdata.length;
                for (var i = 0; i < serverdatalist; i++) {
                    if (serverdata[i].type == "1") {
                        $(" <li>" + serverdata[i].createTime + "机器编号<em>" + serverdata[i].bh + "</em>有新订单创建成功。 </li>").appendTo("#ull1");
                    } else if (serverdata[i].type == "2") {
                        if (serverdata[i].bz == "交易成功") {
                            $(" <li>" + serverdata[i].createTime + "机器编号<em>" + serverdata[i].bh + "</em>有新商品售卖成功。 </li>").appendTo("#ull1");
                        } else {
                            $(" <li style='color:red'>" + serverdata[i].createTime + "机器编号<em>" + serverdata[i].bh + "</em>有新商品售卖,机器故障：" + serverdata[i].bz + "。 </li>").appendTo("#ull1");
                        }

                    }

                }
            }
        })
    }
    function getHotProduct() {
        $("#_hotProduct").val();
        $("  <li>"
                  + "<label></label>"
                  + "<label>商品名称</label>"
                  + "<label>商品类别</label>"
                  + "<label>所属奶企</label>"
                  + "</li>").appendTo("_hotProduct");
        $.ajax({
            type: "post",
            url: "big_screen.aspx/getHotProduct",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{name:'" + $("#_companyName").val() + "',companyID:'" + $("#_companyID").val() + "'}",
            success: function (data) {
                var serverdata = $.parseJSON(data.d);
                var serverdatalist = serverdata.length;
                for (var i = 0; i < serverdatalist; i++) {
                    $(" <li>"
                           + " <span class='screenNum'>1</span>"
                           + " <span>" + serverdata[i].proName + "</span>"
                           + " <span>" + serverdata[i].typeName + "</span>"
                           + " <span>" + serverdata[i].companyName + "</span>"
                        + "</li>").appendTo("#_hotProduct")
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
            data: "{name:'" + $("#_companyName").val() + "',companyID:'" + $("#_companyID").val() + "'}",
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
                        +"<input type='hidden' value='2' class='screenNoticeHidden' />"
                    +"</li>").appendTo("#brokenList");
                    }else if(serverdata[i].statu=="2"){
                         $(" <li>"
                                    +"<span>"
                                     +"     <b class='yellow'>警告</b>"
                                    +"</span>"
                                   + "<span>" + serverdata[i].bh + "</span>"
                                    + "<span>" + serverdata[i].brokenTime + "</span>"
                                    + "<span class='screenNoticeMain'>" + serverdata[i].brokenName + "</span>"
                                    + "<span class='screenNoticeZttz'>" + serverdata[i].runStatu + "</span>"
                                    +"<input type='hidden' value='2' class='screenNoticeHidden' />"
                                +"</li>").appendTo("#brokenList");
                  }
                   

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
            data: "{companyID:'" + $("#_companyID").val() + "'}",
            success: function (data) {
                var serverdata = $.parseJSON(data.d);
                var serverdatalist = serverdata.length;
                for (var i = 0; i < serverdatalist; i++) {
                    $("<div><input type='checkbox' name='subBox' onclick='changeinfo()' value='" + serverdata[i].sCode + "'>" + serverdata[i].sName + "</div>").appendTo("#divCheckBoxList");
                }
            }
        })
    }
    //  右侧查询事件
    function screenSeachBtnchange() {
        var province = $("#s_province").val();
        var city = $("#s_city").val();
        var country = $("#s_county").val();
        var tj = "";
        if (province != "省份") {
            tj += province + ",";
        }
        if (city != "地级市") {
            tj += city + ",";
        }
        if (country != "市、县级市") {
            tj += country + ",";
        }
        if ($("#start").val() != "") {
            tj += $("#start").val() + ",";
        }
        if ($("#end").val() != "") {
            tj += $("#end").val() + ",";
        }
        //jquery给iframe src赋值

        $("#smallechartIframe").attr('src', 'echartMain.aspx?province=' + province + "&city=" + city + "&country=" + country + "&start=" + $("#start").val() + "&end=" + $("#end").val() + "&sxType=" + $("#sxType").val() + "&companyID=" + $("#companyList").val() + "&mechineIDStr=" + $("#cbosDeparentment_hdscbo").val());
        //$('iframe').attr('src', 'src的值');

        tj += $("#sxType").find("option:selected").text();
        $(".screenseachMain").removeClass("screenseachMainH");
        var thisHtml = "";
        $("#seachTiaojian").html(tj.substring(0, tj.length - 1));
    }
    function ssts() {
        $("#ssts").empty();

        $.ajax({
            type: "post",
            url: "big_screen.aspx/ssts",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{companyID:'" + $("#_companyID").val() + "'}",
            success: function (data) {
                var serverdata = $.parseJSON(data.d);
                var serverdatalist = serverdata.length;
                for (var i = 0; i < serverdatalist; i++) {
                    $("  <li>" + serverdata[i].name + "于" + serverdata[i].time + "登记成功，目前累计消费" + serverdata[i].sumConsume + "元，累计充值" + serverdata[i].sumRecharge + "元，账户余额" + serverdata[i].AvailableMoney + "元.</li>").appendTo("#ssts");
                }
            }
        })
    }
    function mechineList1(type) {
       
        $("#mechList").empty();
        $(" <li>"
                     + "<label></label>"
                     + "<label>机器编号</label>"
                     + "<label>所属企业</label>"
                     + "<label>机器地址</label>"
                     + "</li>").appendTo("#mechList");
        $.ajax({
            type: "post",
            url: "big_screen.aspx/mechineList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{type:'" + type + "',companyID:'" + $("#_companyID").val() + "'}",
            success: function (data) {
                var serverdata = $.parseJSON(data.d);
                var serverdatalist = serverdata.length;
                for (var i = 0; i < serverdatalist; i++) {
                    $("  <li> "
                         + "   <span class='screenNum'>1</span>"
                         + "   <span>" + serverdata[i].bh + "</span>"
                         + "   <span>" + serverdata[i].companyName + "</span>"
                         + "   <span>" + serverdata[i].addres + "</span>"
                        + "</li>").appendTo("#mechList");
                }
            }
        })
    }
    function getBrokenDownListNew() {
        $("#newsShowMain").empty();
        $.ajax({
            type: "post",
            url: "big_screen.aspx/getBrokenDownListNew",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{companyID:'" + $("#_companyID").val() + "'}",
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
    function getPath(id)
    {
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
    }
    function getDate() {
        var myDate = new Date();
        //获取当前年
        var year = myDate.getFullYear();
        //获取当前月
        var month = myDate.getMonth() + 1;
        //获取当前日
        var date = myDate.getDate();
        var h = myDate.getHours(); //获取当前小时数(0-23)
        var m = myDate.getMinutes(); //获取当前分钟数(0-59)
        var s = myDate.getSeconds();
        //获取当前时间
        var now = year + '-' + conver(month) + "-" + conver(date) + " " + conver(h) + ':' + conver(m) + ":" + conver(s);
    }

    //日期时间处理
    function conver(s) {
        return s < 10 ? '0' + s : s;
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
