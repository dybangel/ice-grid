var cmStr = $("#allSendDate").html().trim();
var milkSendStatus = 3;//1-本月有订单 2-未配送 
var dayStatus = 0;//0-无订单；1-已完成；2-已过期；3-已转售；4-待取货；5-待配送
(function () {
    /*
     * 用于记录日期，显示的时候，根据dateObj中的日期的年月显示
     */
    var dateObj = (function () {
        var _date = new Date();    // 默认为当前系统时间
        return {
            getDate: function () {
                return _date;
            },
            setDate: function (date) {
                _date = date;
            }
        };
    })();

    // 设置calendar div中的html部分
    renderHtml();
    // 表格中显示日期
    showCalendarData();
    // 绑定事件
    bindEvent();

    /**
     * 渲染html结构
     */
    function renderHtml() {
        var calendar = document.getElementById("calendar");
        var titleBox = document.createElement("h4");  // 标题盒子 设置上一月 下一月 标题
        var bodyBox = document.createElement("section");  // 表格区 显示数据

        // 设置标题盒子中的html
        titleBox.className = 'calendar-title-box';
        titleBox.innerHTML = "<i class='prev-month fa fa-angle-left' id='prevMonth'></i>" +
            "<span class='calendar-title' id='calendarTitle'></span>" +
            "<i id='nextMonth' class='next-month fa fa-angle-right'></i>" +
            "<!-- <a>查看该订单日志</a> -->";
        calendar.appendChild(titleBox);    // 添加到calendar div中

        // 设置表格区的html结构
        bodyBox.className = 'calendar-body-box';
        var _headHtml = "<dl>" +
                  "<dd>日</dd>" +
                  "<dd>一</dd>" +
                  "<dd>二</dd>" +
                  "<dd>三</dd>" +
                  "<dd>四</dd>" +
                  "<dd>五</dd>" +
                  "<dd>六</dd>" +
                "</dl>";
        var _bodyHtml = "";

        // 一个月最多31天，所以一个月最多占6行表格
        for (var i = 0; i < 5; i++) {
            _bodyHtml += "<li>" +
                    "<a dostatus='' onclick='tanalert(this)'><h3></h3><span></span></a>" +
                    "<a dostatus='' onclick='tanalert(this)'><h3></h3><span></span></a>" +
                    "<a dostatus='' onclick='tanalert(this)'><h3></h3><span></span></a>" +
                    "<a dostatus='' onclick='tanalert(this)'><h3></h3><span></span></a>" +
                    "<a dostatus='' onclick='tanalert(this)'><h3></h3><span></span></a>" +
                    "<a dostatus='' onclick='tanalert(this)'><h3></h3><span></span></a>" +
                    "<a dostatus='' onclick='tanalert(this)'><h3></h3><span></span></a>" +
                  "</li>";
        }
        bodyBox.innerHTML = "<section id='calendarTable'><ul id='datelist'>" + _headHtml + _bodyHtml + "</ul></section>";
        // 添加到calendar div中
        calendar.appendChild(bodyBox);
    }

    /**
     * 表格中显示数据，并设置类名
     */
    function showCalendarData() {
        var _year = dateObj.getDate().getFullYear();
        var _month = dateObj.getDate().getMonth() + 1;
        var _dateStr = getDateStr(dateObj.getDate());

        // 设置顶部标题栏中的 年、月信息
        var calendarTitle = document.getElementById("calendarTitle");
        var titleStr = _dateStr.substr(0, 4) + "年" + _dateStr.substr(4, 2) + "月";
        calendarTitle.innerText = titleStr;

        // 设置表格中的日期数据
        var _table = document.getElementById("calendarTable");
        var _tds = _table.getElementsByTagName("h3");
        var _firstDay = new Date(_year, _month - 1, 1);  // 当前月第一天
        for (var i = 0; i < _tds.length; i++) {
            var _thisDay = new Date(_year, _month - 1, i + 1 - _firstDay.getDay());
            var _thisDayStr = getDateStr1(_thisDay);
            _tds[i].innerText = _thisDay.getDate();
            //_tds[i].data = _thisDayStr;
            _tds[i].setAttribute('data', _thisDayStr);
            if (_thisDayStr == getDateStr1(new Date())) {    // 当前天
                _tds[i].className = 'currentDay';
                //$(".currentDay").parent("a").find("span").html("待取货").addClass("spanday");
            } else if (_thisDayStr.substr(0, 6) == getDateStr1(_firstDay).substr(0, 6)) {
                _tds[i].className = 'currentMonth';  // 当前月
                //$(".currentMonth").parent("a").find("span").html("待配送").addClass("spanmonth");
            } else {    // 其他月
                _tds[i].className = 'otherMonth';
                //$(".otherMonth").parent("a").find("span").html("");
            }

        };


    }

    /**
     * 绑定上个月下个月事件
     */
    function bindEvent() {
        var prevMonth = document.getElementById("prevMonth");
        var nextMonth = document.getElementById("nextMonth");
        addEvent(prevMonth, 'click', toPrevMonth);
        addEvent(nextMonth, 'click', toNextMonth);
    }

    /**
     * 绑定事件
     */
    function addEvent(dom, eType, func) {
        if (dom.addEventListener) {  // DOM 2.0
            dom.addEventListener(eType, function (e) {
                func(e);
            });
        } else if (dom.attachEvent) {  // IE5+
            dom.attachEvent('on' + eType, function (e) {
                func(e);
            });
        } else {  // DOM 0
            dom['on' + eType] = function (e) {
                func(e);
            }
        }
    }

    /**
     * 点击上个月图标触发
     */
    function toPrevMonth() {
        var date = dateObj.getDate();
        dateObj.setDate(new Date(date.getFullYear(), date.getMonth() - 1, 1));
        showCalendarData();
        //加载“待配送”
        loadOtherMonthSend(date.getMonth() + 1, cmStr, 'prev');
        //加载“状态”
        loadDayOrderStatus(date.getMonth());
    }

    /**
     * 点击下个月图标触发
     */
    function toNextMonth() {
        var date = dateObj.getDate();
        dateObj.setDate(new Date(date.getFullYear(), date.getMonth() + 1, 1));
        showCalendarData();
        //加载“待配送”
        loadOtherMonthSend(date.getMonth() + 1, cmStr, 'next');
        //加载“状态”
        loadDayOrderStatus(date.getMonth() + 2);
    }

    /**
     * 日期转化为字符串， 4位年+2位月+2位日
     */
    function getDateStr(date) {
        var _year = date.getFullYear();
        var _month = date.getMonth() + 1;    // 月从0开始计数
        var _d = date.getDate();

        _month = (_month > 9) ? ("" + _month) : ("0" + _month);
        _d = (_d > 9) ? ("" + _d) : ("0" + _d);
        return _year + _month + _d;
    }

    function getDateStr1(date) {
        var _year = date.getFullYear();
        var _month = date.getMonth() + 1;    // 月从0开始计数
        var _d = date.getDate();

        _month = (_month > 9) ? ("" + _month) : ("0" + _month);
        _d = (_d > 9) ? ("" + _d) : ("0" + _d);
        return _year + "-" + _month + "-" + _d;
    }


})();
//*************************新增方法开始********************************
//初始化页面加载“当月待配送”
//curMonth--代表当前月份
//otherMonth--代表要跳转的月份
function loadCurrentMonthSend(cmStr) {
    //1-获取当前月
    var date = new Date();
    var curMonth = date.getMonth() + 1;
    var cmArry1 = cmStr.split(',');
    highLight(curMonth, cmArry1);
}
//其他月份“待配送”
function loadOtherMonthSend(curMonth, cmStr, btnStr) {
    var cmArry1 = cmStr.split(',');
    var otherMonth = 0;
    //1-将当前月已经高亮“待配送”的去掉
    if (btnStr == 'prev') {//点击按钮是“上个月”
        otherMonth = curMonth - 1;
    } else if (btnStr == 'next') {//点击按钮是“下个月”
        otherMonth = curMonth + 1;
    }
    cancelHighLight();
    //2-高亮其他月的“待配送”
    highLight(otherMonth, cmArry1);
}
//高亮方法（封装）
function highLight(month, cmArry1) {
    if (cmArry1 && cmArry1.length > 0) {
        var str = '';//将要高亮的日期（第几天）都拼接起来
        for (var i = 0; i < cmArry1.length; i++) {
            if (cmArry1[i]) {
                var dateMonth = cmArry1[i].split('-')[1];
                var dateDay = cmArry1[i].split('-')[2];
                if (parseInt(dateMonth) == month) {
                    //如果数组中的月份等于其他月，则摘取出来，高亮
                    if (dateDay.length == 1) {
                        str += '0' + dateDay + ',';
                    } else if (dateDay.length == 2) {
                        str += dateDay + ',';
                    }
                }
            }
        }
        $("h3").each(function () {
            var dayStr = $(this).html();
            if (dayStr.length == 1) {
                dayStr = '0' + dayStr;
            }
            if (str.indexOf(dayStr) >= 0) {
                //证明当前循环的日期在“日期字符串”中
                if ($(this).hasClass("otherMonth")) {
                    $(this).parent().find("span").html("").removeClass("spanmonth");
                } else if ($(this).hasClass("currentDay")) {
                    //
                } else {
                    //
                }
            }
        });
    }
}
//取消高亮方法（封装）
function cancelHighLight() {
    $("h3").each(function () {
        $(this).parent("a").find("span").html("").removeClass("spanmonth");
    });
}


//加载每天的“订单状态” 0-无订单；1-已完成；2-已过期；3-已转售；4-待取货；5-待配送
//monthNum-要记在状态的月份
function loadDayOrderStatus(monthNum) {
    //1-获取后五种状态数组
    var coArry = $("#completeOrder").html();//已完成 订单数组
    var uoArry = $("#uneffectOrder").html();//已过期 订单数组
    var sooArry = $("#sellOtherOrder").html();//已转售 订单数组
    var roArry = $("#recievingOrder").html();//待取货 订单数组
    var soArry = $("#sendingOrder").html();//待配送 订单数组
    if (monthNum && monthNum.length == 1) {
        monthNum = "0" + monthNum;
    }
    //2-获取相应月份日期五种状态字符串
    //var coStr = getStrByArry(coArry.trim(), monthNum);
    //var uoStr = getStrByArry(uoArry.trim(), monthNum);
    //var sooStr = getStrByArry(sooArry.trim(), monthNum);
    //var roStr = getStrByArry(roArry.trim(), monthNum);
    //var soStr = getStrByArry(soArry.trim(), monthNum);

    //获取当前遍历中的data值
    var eachH3 = $("a[dostatus]").find("h3");
    eachH3.each(function () {
        var eachData = $(this).attr("data");
        if (coArry.indexOf(eachData) >= 0) {
            $(this).removeClass("h3color").parent("a").removeClass("acolor").find("span").html("已完成").removeClass("spanday spanmonth otherMonth").addClass("spanok");
        } else if (uoArry.indexOf(eachData) >= 0) {
            $(this).parent("a").find("span").html("已过期");
        } else if (sooArry.indexOf(eachData) >= 0) {
            $(this).removeClass("h3color").parent("a").removeClass("acolor").find("span").html("已转售").removeClass("spanday spanok spanmonth otherMonth");
        } else if (roArry.indexOf(eachData) >= 0) {
            $(this).addClass("h3color").parent("a").addClass("acolor").find("span").html("待取货").removeClass("spanok spanmonth otherMonth").addClass("spanday");
        } else if (soArry.indexOf(eachData) >= 0) {
            $(this).removeClass("h3color").parent("a").removeClass("acolor").find("span").html("待配送").removeClass("spanday spanok otherMonth").addClass("spanmonth");
        } else {
            $(this).removeClass("h3color").parent("a").removeClass("acolor").find("span").html("").removeClass("spanday spanok spanmonth otherMonth");
        }
    });

}

function tanalert(obj) {
    var $a = $(obj);
    var coArry = $("#completeOrder").html();//已完成 订单数组
    var uoArry = $("#uneffectOrder").html();//已过期 订单数组
    var sooArry = $("#sellOtherOrder").html();//已转售 订单数组
    var roArry = $("#recievingOrder").html();//待取货 订单数组
    var soArry = $("#sendingOrder").html();//待配送 订单数组

    var thisDay = $a.find("h3").attr("data");
    $("#onDaydate").val(thisDay);

    //获取日期进行比对
    var mydate = new Date();
    var strYear = mydate.getFullYear();//年
    var strMonth = mydate.getMonth() + 1;//月
    var strDay = mydate.getDate();//日
    if (parseInt(strMonth) <= 9) {
        strMonth = "0" + strMonth;
    }
    if (parseInt(strDay) <= 9) {
        strDay = "0" + strDay;
    }
    var $toDay = strYear + strMonth + strDay;//获取今天日期(8位数字)

    var $thisDaydata = $a.find("h3").attr("data");//获取点击标签的日期(****-**-**)
    var $newsDay = $thisDaydata.replace(/-/gm, "");//获取点击标签的日期(8位数字)

    var $dataCha = parseInt($newsDay) - parseInt($toDay);//获取点击日期与当前日期的差额

    //var $select = "<select class='dayselect' id='sel'><option value='1'>1</option><option value='2'>2</option><option value='3'>3</option><option value='4'>4</option><option value='5'>5</option></select>";
    //var $select = " <input type='text' id='demo_date' placeholder='选择配送日期'/>";
    if (parseInt($newsDay) < parseInt($toDay) && coArry.indexOf($a.find("h3").attr("data")) >= 0) {
        alert("当天奶品已于2018-03-13 14:15:25取货完成!");
    } else if (parseInt($newsDay) < parseInt($toDay) && uoArry.indexOf($a.find("h3").attr("data")) >= 0) {
        alert("当天奶品已过期!");
    } else if (parseInt($newsDay) < parseInt($toDay) && sooArry.indexOf($a.find("h3").attr("data")) >= 0) {
        alert("当天奶品已于2018-03-13 14:15:25转售完成!!");
    } else if (parseInt($newsDay) == parseInt($toDay) && roArry.indexOf($a.find("h3").attr("data")) >= 0) {
        $(".popup").fadeIn();
        $("#xuanzOne").find("p").html("今日奶品已配送至售卖机<br/>请及时前往进行取货");
        //$("#xuanzOne").find("span").find("input").removeClass("inputbottom");
        $("#xuanzOne").find("dd:first-child").find("a").html("我要取货").attr({ "onclick": "getProduct()" });
        $("#xuanzOne").find("dd:last-child").find("a").html("一键转售").attr({ "onclick": "issell()" });
        $("#xuanzOne").addClass("ChoiceTop");
    } else if (parseInt($newsDay) > parseInt($toDay) && soArry.indexOf($a.find("h3").attr("data")) >= 0 && $dataCha >=2) {
        $(".popup").fadeIn();
        $("#xuanzOne").find("p").html("您可以延期配送当日奶品<br/>请选择日期");
        //$("#xuanzOne").find("span").find("input").addClass("inputbottom");
        $("#xuanzOne").find("dd:first-child").find("a").html("确定调整").attr({ "onclick": "okSet()" }).removeAttr("href");
        $("#xuanzOne").find("dd:last-child").find("a").html("取消").attr({ "onclick": "OffPopup()" }).removeAttr("href");
        $("#xuanzOne").addClass("ChoiceTop");
    }
}


//根据数组获取字符串
function getStrByArry(arryHtml, monthNum) {
    var str = ''; //将要高亮的日期（第几天）都拼接起来
    if (arryHtml && arryHtml.length > 10) {
        var arry = arryHtml.split(',');
        for (var i = 0; i < arry.length; i++) {
            if (arry[i]) {
                var dateMonth = arry[i].split('-')[1];
                var dateDay = arry[i].split('-')[2];
                if (parseInt(dateMonth) == monthNum) {
                    //如果数组中的月份等于其他月，则摘取出来，高亮
                    if (dateDay.length == 1) {
                        str += '0' + dateDay + ',';
                    } else if (dateDay.length == 2) {
                        str += dateDay + ',';
                    }
                }
            }
        }
    } else if (arryHtml && arryHtml.length == 10) {
        var dateMonth = arryHtml.split('-')[1];
        var dateDay = arryHtml.split('-')[2];
        if (parseInt(dateMonth) == monthNum) {
            //如果数组中的月份等于其他月，则摘取出来，高亮
            if (dateDay.length == 1) {
                str += '0' + dateDay + ',';
            } else if (dateDay.length == 2) {
                str += dateDay + ',';
            }
        }
    }
    return str;
}
//*************************新增方法结束********************************