<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="productdetails.aspx.cs" Inherits="Consumer.main.productdetails" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>产品中心-自助售卖系统</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" />
    <link rel="shortcut icon" href="/main/public/images/favicon.ico" />
    <link href="/main/public/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/common.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/style.css" rel="stylesheet" type="text/css" />
    <script src="/main/public/script/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="/main/public/script/JavaScript.js" type="text/javascript"></script>
    <link href="../scripts/skin/jedate.css" rel="stylesheet" type="text/css" />
    <script src="../scripts/jedate.js" type="text/javascript"></script>
    <script src="../scripts/jedate.min.js"></script>

    <script type="text/javascript" src="/main/public/script/mobiscroll.custom.min.js"></script>
    <link href="/main/public/css/mobiscroll.custom.min.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <main class="setupq">
            <div class="popup_w change">
                <div class="popup_m">
                    <h4>订单信息<em onclick="offpopup()">取消</em></h4>
                    <ul>
                        <li>
                            <label>配送周期</label>
                            <select id="sel1" onchange="chg1()">
                               <%-- <option value="1">1天</option>
                                <option value="2">30天</option>
                                <option value="3">90天</option>
                                <option value="4">半年(180天)</option>
                                <option value="5">1年</option>--%>
                            </select>
                        </li>
                        <li>
                            <label>开始配送</label>
                            <input type="text" id="demo_date" placeholder="选择配送日期"  onchange="inputChg()"/>
                        </li>       
                        <li>
                            <label>止送日期</label>
                            <input type="text" readonly="readonly" style="background: #f0f0f0;" id="zdDate" />
                        </li>
                        <li>
                            <label>配送方式</label>
                            <select id="psfs" onchange="chg()">
                                <option value="1">按天配送</option>
                                <option value="2">自定时间配送</option>
                            </select>
                        </li>
                        <li id="showfs"></li>
                    </ul>
                    <a class="orderpopupBtn" onclick="createOrder()">确定下单</a>
                </div>
            </div>
            <div class="popup" onclick="offpopup()"></div>
            <div class="product">
                <em id="gb_btn">×</em>
                  <% 
                    if (dt.Rows.Count > 0)
                    {%>
                <div class="productimg">
                    <img src="<%=dt.Rows[0]["httpImageUrl"].ToString() %>" alt="" />
                </div>
              
                <h4><%=dt.Rows[0]["proName"].ToString() %></h4>
                <span>月售<%=dt.Rows[0]["ljxs"] %></span>
                <p><%=dt.Rows[0]["description"].ToString() %></p>
                <label>¥<%=double.Parse(dt.Rows[0]["price2"].ToString()).ToString("f2") %> * 1</label>
                <%}
                %>
                <input type="button" value="创建订单" onclick="orderpopup()" class="btn pro_btn" />
            </div>
        </main>
        <input id="product_id" runat="server" type="hidden" />
        <input id="companyID" runat="server" type="hidden"/>
        <input id="_mechineID" runat="server" type="hidden"/>
        <input id="_memberID" runat="server" type="hidden"/>
    </form>
</body>
</html>
<script>
    // 未付款自动刷新
    function orderpopup() {
        $(".popup").show();
        $(".popup_w").addClass("active");
    }

    function offpopup() {
        $(".popup").hide();
        $(".popup_w").removeClass("active");
    }
  
    $(function () {
       
        $("#gb_btn").click(function () {
            history.back();
        });

        $('#psfs').change(function () {
            var geDay = "<a onclick='a(this,0)' class='aaa qda'>每天配送</a><a onclick='a(this,1)' class='aaa'>隔一天</a><a onclick='a(this,2)' class='aaa'>隔二天</a><a onclick='a(this,3)' class='aaa'>隔三天</a>";
            "<span onclick='b(this)'><em name='周一'></em>周一</span><span onclick='b(this)'><em name='周二'></em>周二</span><span onclick='b(this)'><em name='周三'></em>周三</span><span onclick='b(this)'><em name='周四'></em>周四</span><span onclick='b(this)'><em name='周五'></em>周五</span><span onclick='b(this)'><em name='周六'></em>周六</span><span onclick='b(this)'><em name='周日'></em>周日</span><a class='bbb qda' onclick='okBtn()'>确定</a>";
            if ($(this).val() == "1") {
                $("#showfs").html(geDay);
            } else if ($(this).val() == "2") {
                $("#showfs").html(xzDay);
            }
        });

        var geDay = "<a onclick='a(this,0)' class='aaa qda'>每天配送</a><a onclick='a(this,1)' class='aaa'>隔一天</a><a onclick='a(this,2)' class='aaa'>隔二天</a><a onclick='a(this,3)' class='aaa'>隔三天</a>";
        var xzDay = "<span onclick='b(this)'><em name='周一'></em>周一</span><span onclick='b(this)'><em name='周二'></em>周二</span><span onclick='b(this)'><em name='周三'></em>周三</span><span onclick='b(this)'><em name='周四'></em>周四</span><span onclick='b(this)'><em name='周五'></em>周五</span><span onclick='b(this)'><em name='周六'></em>周六</span><span onclick='b(this)'><em name='周日'></em>周日</span><a class='bbb qda' onclick='okBtn()'>确定</a>";
        if ($("#psfs").val() == "1") {
            $("#showfs").html(geDay);
        } else if ($("#psfs").val() == "2") {
            $("#showfs").html(xzDay);
        }
        getActivityList();
       
    });
    var txtStr = "";//记录选择的日期
    function okBtn() {
        var spanenName = $("#showfs").find("span");
        txtStr = "";
        spanenName.each(function () {
            if ($(this).find("em").hasClass("emcolor fa fa-check")) {
                txtStr += $(this).find("em").attr("name")+"|";
            };
        });
        
    }
    var day = 0;
    function a(obj, val) {
        var thisa = $(obj);
        $("#showfs").find("a").removeClass("qda");
        thisa.addClass("qda");
        day = val;
        jsCount();
    }
    function b(obj) {
        var thisSpan = $(obj);
        thisSpan.find("em").toggleClass("emcolor fa fa-check");
        thisSpan.toggleClass("spancolor");
        okBtn();
        jsCount(0);
    }
    var theme = "ios";
    var mode = "scroller";
    var display = "bottom";
    var lang = "zh";
   
    $('#demo_date').mobiscroll().date({
        theme: theme,
        mode: mode,
        display: display,
        lang: lang,
        minDate: new Date(GetDateStrYear(2), GetDateStrMonth(1), GetDateStrDay(1)),
       // minDate: new Date(2018, 02, 28),
        maxDate: new Date(2050, 12, 31),
        stepMinute: 1
    });

    function compareDate(s1, s2) {
       
        return ((new Date(s1.replace(/-/g, "\/"))) > (new Date(s2.replace(/-/g, "\/"))));
    }
    function GetDateStr2(AddDayCount)
    {
        var dd = new Date();
        dd.setDate(dd.getDate()+AddDayCount);//获取AddDayCount天后的日期
        var y = dd.getFullYear();
        var m = dd.getMonth()+1;//获取当前月份的日期
        var d = dd.getDate();
        return y+"-"+m+"-"+d;
    }
    function createOrder()
    {
        if ($("#demo_date").val() == "") {
            alert("请选择开始配送日期");
            return;
        }
        var myDateT = new Date();
        if (myDateT.getHours() >= 22) {
            //必选选择后天之后的时间
            if (compareDate($("#demo_date").val(),GetDateStr2(1))) {
                //前边的时间大
            } else {
                alert("晚上10点之后下单，开始派送时间请选择后天开始派送");
            }
        }
       
        var yhfs = $("#sel1").find("option:selected").text();
        if (yhfs.indexOf("赠送") > -1) {

            yhfs = yhfs.substring(yhfs.indexOf("赠送"), yhfs.length);

        } else if (yhfs.indexOf("折")) {
            yhfs = yhfs.substring(yhfs.indexOf("打"), yhfs.length);
            
        }


        var dayT = $("#sel1").val();//1 1天 2 30天  3 90天 4半年 5 一年
        var type = $("#psfs").val();//1 按天配送  2自定时间配送
        var pszq = 1;//配送周期
        var qsDate = $("#demo_date").val();//起订日期
        var zdDate = "";//止定日期
        var psfs = $("#psfs").val();//配送方式
        var psStr = "";//如果选择的是 按天派送存 每天 隔天等等 如果存的是自定义时间存的是周几
        var selDate = "";//获取根据条件选择完之后 应该是哪几天派送
        //if (dayT == "1") {
        //    pszq = 1;
        //} else if (dayT == "2") {
        //    pszq = 30;
        //} else if (dayT == "3") {
        //    pszq = 90;
        //} else if (dayT == "4") {
        //    pszq = 180;
        //} else if (dayT == "5") {
        //    pszq = 365;
        //}
        pszq = dayT;
        if (type == "1") {
            jsCount();
            if (day == 0)
            {
                psStr = "每天配送";
            }else if(day==1)
            {
                psStr="隔一天";
            } else if (day == 2) {
                psStr = "隔两天";
            } else if (day == 3) {
                psStr = "隔三天";
            }
            zdDate = $("#zdDate").val();
            if(zdDate=="")
            {
                alert("止订日期为空 请重新下单");
                return;
            }
            selDate = SSTime;
            var Dd=getNowFormatDate();
            var timestamp = (new Date()).getTime();
            console.log(selDate);

            location.href = "placeorderok.aspx?pszq=" + pszq + "&qsDate=" + qsDate + "&zdDate=" + zdDate + "&psStr=" + psStr + "&psfs=" + psfs + "&selDate=" + selDate + "&productID=" +<%=productID%> +"&mechineID=" +<%=mechineID%> +"&createTime=" + Dd + "&yhfs=" + yhfs + "&orderNO=" + timestamp + "&memberID=" + $("#_memberID").val();
        } else {
            if (txtStr == "") {
                alert("请先选择周几");
                return;
            }
           
            zdDate = $("#zdDate").val();
            if (zdDate=="") {
                alert("止订日期为空 请重新下单");
                return;
            }
            selDate = SSTime;
           
            psStr = txtStr.substring(0,txtStr.length - 1);
             
            jsCount();
            var Dd = getNowFormatDate();
            console.log(selDate);
            location.href = "placeorderok.aspx?pszq=" + pszq + "&qsDate=" + qsDate + "&zdDate=" + zdDate + "&psStr=" + psStr + "&psfs=" + psfs + "&selDate=" + selDate + "&productID=" +<%=productID%> +"&mechineID=" +<%=mechineID%> +"&createTime="+Dd+"&yhfs="+yhfs;
        }
    }
    var SSTime = "";
    function jsCount() {
        SSTime = "";
        if ($("#demo_date").val() == "") {
            alert("请选择开始配送日期");
            return;
        }
        var n = 1;
        var m = 1;
        var dayT = $("#sel1").val();//1 1天 2 30天  3 90天 4半年 5 一年
        var type = $("#psfs").val();//1 按天配送  2自定时间配送
        if (type == "1") {
            //if (dayT == "1") {
            //    n = 1;
            //} else if (dayT == "2") {
            //    n = 30;
            //} else if (dayT == "3") {
            //    n = 90;
            //} else if (dayT == "4") {
            //    n = 180;
            //} else if (dayT == "5") {
            //    n = 365;
            //}
            n = parseInt(dayT) + parseInt(getZS());
            
            if (day == "1")//隔天派送
            {
                m = n * 2 - 1;
            } else if (day == "2")//隔2天派送
            {
                m = n * 3 - 2;
            } else if (day == "3")//隔3天派送
            {
                m = n * 4 - 3;
            } else {
                m = n * 1;//每天派送
            }
            //获取应该配送的日期 应该循环m
            var N=1;//自增变量
            while(N<=m)
            {
                var t = GetDateStr(N-1, $("#demo_date").val());
               
                if(day=="1")
                {
                    N = N + 2;
                }else if(day=="2")
                {
                    N = N + 3;
                } else if (day == "3") {
                    N = N + 4;
                } else if(day=="0"){
                    N = N + 1;
                }
            
                SSTime += t+",";
            }
            SSTime = SSTime.substring(0, SSTime.length - 1);
            $("#zdDate").val(GetDateStr(m-1, $("#demo_date").val()));
        } else {
            if (txtStr == "")
            {
                alert("请先选择周几");
                return;
            }
            //if (dayT == "1") {
            //    n = 1;
            //} else if (dayT == "2") {
            //    n = 30;
            //} else if (dayT == "3") {
            //    n = 90;
            //} else if (dayT == "4") {
            //    n = 180;
            //} else if (dayT == "5") {
            //    n = 365;
            //}
            n = parseInt(dayT) + parseInt(getZS());
            var count = 0;
             
            for (var i = 0; i < 1000000;i++)
            {
                var time = GetDateStr(i, $("#demo_date").val());
                var week = getDay(time);
                if (txtStr.indexOf(week) > -1) {
                    SSTime += time + ",";
                    count++;
                }
                if(count==n)
                {
                    $("#zdDate").val(time);
                    break;
                }
            }
            SSTime=SSTime.substring(0,SSTime.length-1);
        }
    }
    function inputChg()
    {
       
        var type = $("#psfs").val();
        if (type == "1" && $("#demo_date").val() != "")
        {
            //按天派送
            jsCount(0);
        } else if (type == "2" && $("#demo_date").val() != "" && txtStr != "")
        {
            jsCount(0);
        }
    }
    //选择的天数
    function chg1()
    {  
        var type = $("#psfs").val();
        var dayT = $("#sel1").val();
        if (type == "1" && $("#demo_date").val()!="")
        {
            //按天派送
            jsCount(0);
        } else if (type == "2" && $("#demo_date").val() != "" && txtStr!="")
        {
            //自定义时间派送
            jsCount(0);
        }
    }
    //配送方式
    function chg()
    {
        if ($("#psfs").val() == "1" && $("#demo_date").val() != "")
        {
            jsCount(0);
        } else if ($("#psfs").val() == "2")
        {
            $("#zdDate").val("");
        }
    }
    function GetDateStrYear(AddDayCount) {
        var dd = new Date();
        dd.setDate(dd.getDate() + AddDayCount);//获取AddDayCount天后的日期
        var y = dd.getFullYear();
        var m = dd.getMonth() + 1;//获取当前月份的日期
        var d = dd.getDate();
        return y;
    }
    function GetDateStrMonth(AddDayCount) {
        var dd = new Date();
        dd.setDate(dd.getDate() + AddDayCount);//获取AddDayCount天后的日期
        var y = dd.getFullYear();
        var m = dd.getMonth();//获取当前月份的日期 
        return m;
    }
    function GetDateStrDay(AddDayCount) {
        var dd = new Date();
        dd.setDate(dd.getDate() + AddDayCount);//获取AddDayCount天后的日期
        var y = dd.getFullYear();
        var m = dd.getMonth() + 1;//获取当前月份的日期
        var d = dd.getDate();
        return d;
    }
    //输入的时间格式为yyyy-MM-dd
    function convertDateFromString(dateString) {
        if (dateString) {
            var date = new Date(dateString.replace(/-/, "/"))
            return date;
        }
    }
    function GetDateStr(AddDayCount, time) {
        var dd = convertDateFromString(time);
        dd.setDate(dd.getDate() + AddDayCount );//获取AddDayCount天后的日期    
        var y = dd.getFullYear();
        var m = (dd.getMonth() + 1) < 10 ? "0" + (dd.getMonth() + 1) : (dd.getMonth() + 1);//获取当前月份的日期，不足10补0    
        var d = dd.getDate() < 10 ? "0" + dd.getDate() : dd.getDate();//获取当前几号，不足10补0    
        return y + "-" + m + "-" + d;
    }

    function getDay(time) {
        var value = time;
        if (value == "") {
            return;
        } else {
            //text 内容不能改与页面一致
            var day = new Date(value).getDay(),
                text = "";
            switch (day) {
                case 0:
                    text = "周日";
                    break;
                case 1:
                    text = "周一";
                    break;
                case 2:
                    text = "周二";
                    break;
                case 3:
                    text = "周三";
                    break;
                case 4:
                    text = "周四";
                    break;
                case 5:
                    text = "周五";
                    break;
                case 6:
                    text = "周六";
                    break;
            }
            return text;
        }
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
        return currentdate;
    }
    function getActivityList()
    {
        $.ajax({
            type: "post",
            url: "productdetails.aspx/getActivityList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{companyID:'" + $("#companyID").val() + "',productID:'" + $("#product_id").val() + "',mechineID:'" + $("#_mechineID").val() + "'}",
            success: function (data) {
                var serverdata = $.parseJSON(data.d);
                var serverdatalist = serverdata.length;
                var str = "";
                for (var i = 0; i < serverdatalist; i++)
                {
                    //if (serverdata[i].type == "1") {
                    //    str = "打" + serverdata[i].num + "折";
                    //} else if (serverdata[i].type == "2") {
                    //    str = "赠送" + serverdata[i].num + "天";
                    //} else {
                    //    str = "暂无设置";
                    //}
                    var value = serverdata[i].zq;
                    var text = serverdata[i].zqName;
                    $("<option value='" + value + "'>" + "周期：" + text + "     ；优惠：" + serverdata[i].str + "</option>").appendTo("#sel1");
                   
                }
            }

        })
    }
   
    function getZS()
    {
        //获取当前选中的配送周期  如果是赠送的话 返回赠送的天数  如果是打折的话返回折扣
        var str = $("#sel1").find("option:selected").text();
        if (str.indexOf("赠送") > -1) {
          
            str = str.substring(str.indexOf("赠送"), str.length).replace("赠送","").replace("天","");
            
        } else if (str.indexOf("折")) {
            //str = str.substring(str.indexOf("打"), str.length).replace("打", "").replace("折", "");
            str = "0";
        }
        return str;
    }
</script>
