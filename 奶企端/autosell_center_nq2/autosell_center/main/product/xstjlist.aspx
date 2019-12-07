<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="xstjlist.aspx.cs" Inherits="autosell_center.main.product.xstjlist" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>商品保质期-自动售卖终端中心控制系统</title>
     <link rel="shortcut icon" href="/main/public/images/favicon.ico" />
    <link href="/main/public/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/common.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/style.css" rel="stylesheet" type="text/css" />

    <link rel="stylesheet" type="text/css" href="../bootstrapSelect/css/bootstrap.css" />
    <link rel="stylesheet" type="text/css" href="../bootstrapSelect/dist/css/bootstrap-select.css" />
    <script src="../bootstrapSelect/dist/js/jquery.js"></script>
    <script src="../bootstrapSelect/dist/js/bootstrap.min.js"></script>
    <script src="../bootstrapSelect/dist/js/bootstrap-select.min.js" charset="gb2312"></script>


    <script src="/main/public/script/Titlelist.js" type="text/javascript"></script>
    <link href="../../scripts/skin/jedate.css" rel="stylesheet" type="text/css" />
    <script src="../../scripts/jedate.js" type="text/javascript"></script>
    <script src="../../scripts/jedate.min.js"></script>
    <style>
        .csssl {
            overflow: hidden;
            text-overflow: ellipsis;
            white-space: nowrap;
        }
        .zfpopup ul li span {
            display: inline-block;
            width: initial;
            float: initial;
            text-align: left;
            line-height: inherit;
            border: 0;
        }
         .setupPSY .bootstrap-select {
            width: 100% !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="header"></div>
       
        <div class="main"> 
            <div id="adminpopup" class="change zfpopup" style="top:7%">
                <h4 id="title"></h4>
                <ul>
                  <%--  <li>
                        <h5>设置机器</h5>
                        <label class="setupPSY">
                            <select id="mechineList" class="selectpicker" multiple data-live-search="true" data-live-search-placeholder="请输入搜索" data-actions-box="true">
                            </select>
                        </label>
                    </li>--%>
                    <li>
                        <h5>类型</h5>
                        <label>
                            <select id="type2" onchange="typeChg()">
                                <option value="1">周期特价</option>
                                <option value="2">阶段特价</option>
                            </select>
                        </label>
                    </li>
                    <li>
                        <h5>开始时间</h5>
                        <label>
                            <input name="act_stop_timeks" type="text" id="Text1" runat="server" class="input" value="" placeholder="开始时间" />
                        </label>
                    </li>
                     <li>
                        <h5>结束时间</h5>
                        <label>
                            <input name="act_stop_timeks" type="text" id="Text2" runat="server" class="input" value="" placeholder="结束时间" />
                        </label>
                    </li>
                    <li id="timeSpanLi">
                        <h5>时间段</h5>
                        <label>
                            <input name="timeSpan" type="text" id="timeSpan" runat="server" class="input" value="" placeholder="格式01:30-02:40,03:30-04:40" />
                        </label>
                    </li>
                     <li>
                        <h5>零售价</h5>
                        <label>
                            <input name="timeSpan" type="text" id="price0" runat="server" class="input" value="" placeholder="必填项,多个时间段多个零售价，隔开，会员价同零售价" />
                        </label>
                    </li>
                    <li>
                        <h5>普通会员价</h5>
                        <label>
                            <input name="timeSpan" type="text" id="price1" runat="server" class="input" value="" placeholder="普通会员价 0代表不开启" />
                        </label>
                    </li>
                    <li>
                        <h5>白银会员价</h5>
                        <label>
                            <input name="timeSpan" type="text" id="price2" runat="server" class="input" value="" placeholder="白银会员价 0代表不开启" />
                        </label>
                    </li>
                    <li>
                        <h5>黄金会员价</h5>
                        <label>
                            <input name="timeSpan" type="text" id="price3" runat="server" class="input" value="" placeholder="黄金会员价 0代表不开启" />
                        </label>
                    </li>
                     <li>
                        <h5>限购N小时</h5>
                        <label>
                            <input name="hours" type="text" id="hours" runat="server" class="input" value="" placeholder="N小时内限购" maxlength="4" />
                        </label>
                    </li>
                     <li>
                        <h5>限购M次</h5>
                        <label>
                            <input name="buyCount" type="text" id="buyCount" runat="server" class="input" value="" placeholder="N小时内限购M次" />
                        </label>
                    </li>
                </ul>
                <%--<dl>--%>
                    <dd>
                        <input type="button" value="确定" class="popupqdbtn" onclick="saveActivity()" />
                    </dd>
                    <dd>
                        <input type="button" value="取消" onclick="qxClick()" />
                    </dd>
                </dl>
            </div>


            <div class="main_list">
                <div class="common_title">
                    <h4>
                        <i class="fa fa-cubes"></i>
                        <span>商品管理</span>
                    </h4>
                </div>
                <div class="common_main">
                    <div class="navlist">
                        <dl>
                            <dt>商品信息<em class="fa fa-cog"></em></dt>
                           <dd>
                                <a class="change " href="productlist.aspx"><i class="change  fa fa-file-text"></i>商品列表</a>
                            </dd>
                             <dd>
                                <a class="change acolor" href="xstjlist.aspx"><i class="change icolor fa fa-file-text"></i>限时活动产品</a>
                            </dd>
                            <dd>
                                <a class="change " href="productadd.aspx"><i class="change  fa fa-plus-square"></i>添加商品</a>
                            </dd>
                            <dd>
                                <a class="change" href="brandList.aspx"><i class="change fa fa-window-restore"></i>品牌列表</a>
                            </dd>
                        </dl>
                    </div>
                    <section class="jiqlist">
                        <dl class="jiqlistTab">
                            <dd class="change ddcolor"><b>商品列表</b></dd>
                        </dl>
                        <ul class="jiqlistseach">
                            <li>
                                <select id="mechineList" class="selectpicker" multiple data-live-search="true" data-live-search-placeholder="请输入搜索" data-actions-box="true">
                                </select>
                            </li>
                            <li>
                                <input type="text" value="" placeholder="商品名称、条码" id="keyword" />
                            </li>
                            
                            <li>
                                <input name="act_stop_timeks" autocomplete="off" type="text" id="start" runat="server" class="input" value="" placeholder="开始时间" />
                            </li>
                            <li >
                                <input name="act_stop_timeks" autocomplete="off" type="text" id="end" runat="server" class="input" value="" placeholder="结束时间" />
                            </li>
                           <li>
                               <select id="type">
                                   <option value="0">全部</option>
                                   <option value="1">周期特价</option>
                                   <option value="2">阶段特价</option>
                               </select>
                           </li>
                           
                            <li>
                                <input type="button" value="查询" class="seachbtn" onclick="search()" />
                            </li>
                             <li>
                                <input type="button" value="一键删除" onclick="deleteALL()" class="seachbtn" />
                            </li>
                          <%--  <li>
                                <input type="button" value="导出" class="seachbtn" onclick="exportExcel()" />

                            </li>--%>
                        </ul>
                        <ul class="productlist" id="product">
                        </ul>
                           <div class="commonPage">
                            <a class="change" onclick="getPage('first')">首页</a>
                            <a class="change" onclick="getPage('up')">上一页</a>
                            <a class="change" onclick="getPage('down')">下一页</a>
                            <a class="change" onclick="getPage('end')">尾页</a>
                            <select id="pageSel" onchange="pageChg()">
                            </select>
                        </div>
                    </section>
                </div>
            </div>
        </div>
        <input id="companyId" runat="server" type="hidden" />
        <input id="_productID" runat="server" type="hidden"/>
        <input id="_operaID" runat="server" type="hidden" />
        <input id="_hidID" runat="server" type="hidden"/>
        <input id="_mechineID" runat="server" type="hidden"/>
        <input id="pageCurrentCount" runat="server" type="hidden" value="1" />
        <input id="pageTotalCount" runat="server" type="hidden" value="1" />
    </form>
</body>
</html>
<script>
    function judge() {
        $.ajax({
            type: "post",
            url: "xstjlist.aspx/judge",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{operaID:'" + $("#_operaID").val() + "',menuID:'xshdcp'}",
            success: function (data) {
                if (data.d.code == "500") {
                    $(".jiqlist").empty();
                    $(".jiqlist").html("<p class='noquanxian'>当前没有权限</p>").css({ "background": "#ddd" });
                }
            }
        })
    }
    function getPage(val) {
        if (val == "first") {
            $("#pageCurrentCount").val("1");
        } else if (val == "up") {
            $("#pageCurrentCount").val($("#pageCurrentCount").val() - 1);
        } else if (val == "down") {
            $("#pageCurrentCount").val($("#pageCurrentCount").val() + 1)
        } else if (val == "end") {

        }
    }
    function getPage(val) {
        if (val == "first") {
            $("#pageCurrentCount").val("1");
        } else if (val == "up") {
            var index = parseInt($("#pageCurrentCount").val()) - 1;
            if (index >= 1) {
                $("#pageCurrentCount").val(index);
            }
        } else if (val == "down") {
            var index = parseInt($("#pageCurrentCount").val()) + 1;
            if (index <= parseInt($("#pageTotalCount").val())) {
                $("#pageCurrentCount").val(index);
            }
        } else if (val == "end") {
            $("#pageCurrentCount").val($("#pageTotalCount").val());
        }
        search();
    }
    function pageChg() {
        $("#pageCurrentCount").val($("#pageSel").val());
        search();
    }
    $(function () {
        jeDate({
            dateCell: "#start", //isinitVal:true,
            //format: "YYYY-MM-DD",
            isTime: false, //isClear:false,
            choose: function (val) { },
            minDate: "2014-09-19 00:00:00"
        });
        jeDate({
            dateCell: "#end",
            isinitVal: true,
            isTime: true, //isClear:false,
            minDate: "2014-09-19 00:00:00"
        });
        jeDate({
            dateCell: "#Text1", //isinitVal:true,
            //format: "YYYY-MM-DD",
            isTime: false, //isClear:false,
            choose: function (val) { },
            minDate: "2014-09-19 00:00:00"
        });
        jeDate({
            dateCell: "#Text2",
            isinitVal: true,
            isTime: true, //isClear:false,
            minDate: "2014-09-19 00:00:00"
        });
        judge()
        getMechineList();
        $("#li3").find("a").addClass("aborder");
        search();

         
    })
    function sellAll()
    {
        $("input[name='cbServerType']").each(function (index, DomEle) {
            if(DomEle.checked){
                $(DomEle).attr("checked",null);
            }else{
                $(DomEle).attr("checked","checked");
            }
        })
     }
    function search()
    {
        $("#product").empty();
        $(" <li>"
                 + "   <label style='width:6%'><a onclick='sellAll()'>全选</a></label>"
                 + "   <label style='width:9%'>商品名称</label>"
                 + "   <label style='width:9%'>机器编号</label>"
                 + "   <label style='width:9%'>类型</label>"
                 + "   <label style='width:6%'>零售价</label>"
                 + "   <label style='width:6%'>普通价格</label>"
                 + "   <label style='width:6%'>白银价格</label>"
                 + "   <label style='width:6%'>黄金价格</label>"
                 + "   <label style='width:12%'>开始时间</label>"
                 + "   <label style='width:12%'>结束时间</label>"
                 + "   <label style='width:9%'>时间段</label>"
                 + "     <label class='procz' style='width:9%;'>操作</label>"
                 +"   </li>").appendTo("#product");
        $.ajax({
            type: "post",
            url: "xstjlist.aspx/getList",
            contentType: "application/json; charset=utf-8",
            data: "{companyID:'" + $("#companyId").val() + "',mechineID:'" + $("#mechineList").val() + "',keyword:'" + $("#keyword").val() + "',type:'" + $("#type").val() + "',start:'" + $("#start").val() + "',end:'" + $("#end").val() + "',pageCurrentCount:'" + $("#pageCurrentCount").val() + "'}",
            dataType: "json",
            success: function (data) {

                var count = data.d.count;
                if (parseInt(count) >= 0) {
                    $("#pageSel").empty();
                    for (var k = 1; k <= parseInt(count) ; k++) {
                        $("<option value='" + k + "'>" + k + "</option>").appendTo("#pageSel");
                    }
                }
                var serverdata = $.parseJSON(data.d.db);
                $("#pageTotalCount").val(count); 
                var serverdatalist = serverdata.length;
                for (var i = 0; i < serverdatalist; i++) {
                    $(" <li>"
                             + "  <span style='width:6%'><input type='checkbox' name='cbServerType' value='" + serverdata[i].id + "'/></span>"
                             + "  <span style='width:9%'>" + serverdata[i].proName + "</span>"
                             + "  <span style='width:9%'>" + serverdata[i].bh + "</span>"
                             + "  <span style='width:9%'>" + (serverdata[i].type=="1"?"周期特价":"阶段特价") + "</span>"
                             + "  <span style='width:6%'>" + (serverdata[i].price0).toFixed(2) + "</span>"
                             + "  <span style='width:6%'>" + (serverdata[i].price1).toFixed(2) + "</span>"
                             + "  <span style='width:6%'>" + (serverdata[i].price2).toFixed(2) + "</span>"
                             + "  <span style='width:6%'>" + (serverdata[i].price3).toFixed(2) + "</span>"
                             + "  <span style='width:12%'>"+ serverdata[i].startTime + "</span>"
                             + "  <span style='width:12%'>"+ serverdata[i].endTime + "</span>"
                             + "  <span style='width:9%'>" + (serverdata[i].timeSpan == "" ? "无" : serverdata[i].timeSpan) + "</span>"
                             + "  <span class='procz' style='width:9%'>"
                             + "    <a onclick='deLete(" + serverdata[i].id + ")'>删除</a>"
                             + "    <a onclick='getActivity(" + serverdata[i].id + "," + serverdata[i].productID + "," + serverdata[i].mechineID + ",\"" + serverdata[i].proName + "\")'>查看</a>"
                             +"   </span>"
                       +"</li>").appendTo("#product");
                }
                 
            }
        })
    }
    function qxClick() {
        $(".selectpicker").selectpicker("val", '');
        $(".selectpicker").selectpicker("refresh");
        $("#type").val('');
        $("#start").val('');
        $("#end").val('');
        $("#timeSpan").val('');

        $("#price0").val('');
        $("#price1").val('');
        $("#price2").val('');
        $("#price3").val('');
        $(".tangram-suggestion-main").hide();
        $(".popupbj").hide();
        $(".zfpopup").removeClass("zfpopup_on");
    }
    function saveActivity(id) {
        $.ajax({
            type: "post",
            url: "xstjlist.aspx/saveActivity",
            contentType: "application/json; charset=utf-8",
            data: "{mechineList:'" + $("#_mechineID").val() + "',type:'" + $("#type2").val() + "',start:'" + $("#Text1").val() + "',end:'" + $("#Text2").val() + "',timeSpan:'" + $("#timeSpan").val() + "',productID:'" + $("#_productID").val() + "',price0:'" + $("#price0").val() + "',price1:'" + $("#price1").val() + "',price2:'" + $("#price2").val() + "',price3:'" + $("#price3").val() + "',companyID:'" + $("#companyId").val() + "',hours:'" + $("#hours").val() + "',buyCount:'" + $("#buyCount").val() + "'}",
            dataType: "json",
            success: function (data) {
                if (data.d.code == "200") {
                    alert(data.d.msg);
                    window.location.reload();
                } else if (data.d.code == "500") {
                    alert(data.d.msg);
                }
            }
        })
    }
    function getActivity(id,productID, mechineID,proname) {
        $.ajax({
            type: "post",
            url: "xstjlist.aspx/getActivity",
            contentType: "application/json; charset=utf-8",
            data: "{productID:'" + productID + "',mechineID:'"+mechineID+"'}",
            dataType: "json",
            success: function (data) {
                if (data.d.code == "200") {
                    var arr = data.d.db.split(',');
                    $(".selectpicker").selectpicker("val", arr);
                    $(".selectpicker").selectpicker("refresh");
                    $("#type").val(data.d.type);
                    $("#Text1").val(data.d.start);
                    $("#Text2").val(data.d.end);
                    if (data.d.type == "1") {
                        $("#timeSpan").val(data.d.timeSpan);
                        $("#timeSpanLi").show();
                    } else {
                        $("#timeSpanLi").hide();
                    }
                    $("#title").html(proname)
                    $("#price0").val(data.d.price0);
                    $("#price1").val(data.d.price1);
                    $("#price2").val(data.d.price2);
                    $("#price3").val(data.d.price3);
                    $("#hours").val(data.d.hours);
                    $("#buyCount").val(data.d.buycount);

                }
            }
        })
        $("#_mechineID").val(mechineID)
        $("#_productID").val(productID);
        $(".popupbj").fadeIn();
        $("#adminpopup").addClass("zfpopup_on");
    }
        function deleteALL()
        {
            var str = "";
            $.each($('input:checkbox:checked'), function () {
                str += $(this).val()+","
            });
            if (str.length > 0) {
                str = str.substr(0, str.length - 1);
                $("#_hidID").val(str);
                $.ajax({
                    type: "post",
                    url: "xstjlist.aspx/deleteALL",
                    contentType: "application/json; charset=utf-8",
                    data: "{id:'" + str + "',companyID:'" + $("#companyId").val() + "'}",
                    dataType: "json",
                    success: function (data) {
                        if (data.d.code == "200") {
                            window.location.reload();
                            alert("删除成功");
                        } else if (data.d.code == "500") {
                            alert("删除失败");
                        }
                    }
                })

            } else {
                alert("请勾选需要删除的产品");
            }
      
        }
    
   
        function deLete(id)
        {
            if(confirm("是否确定删除该活动"))
            {
                $.ajax({
                    type: "post",
                    url: "xstjlist.aspx/delete",
                    contentType: "application/json; charset=utf-8",
                    data: "{id:'" + id + "',companyID:'" + $("#companyId").val() + "'}",
                    dataType: "json",
                    success: function (data) {
                        if (data.d.code == "200") {
                            window.location.reload();
                            alert("删除成功");
                        } else if (data.d.code == "500") {
                            alert("删除失败");
                        }
                    }
                })
            }
        
        }
    
        function getMechineList() {
            $.ajax({
                type: "post",
                url: "xstjlist.aspx/getMechineList",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: "{companyID:'" + $("#companyId").val() + "'}",
                success: function (data) {
                    if (data.d.code == "200") {
                        var serverdata = $.parseJSON(data.d.db);
                        var serverdatalist = serverdata.length;

                        var optionString = "";
                        for (i = 0; i < serverdatalist; i++) {
                            optionString += "<option value=\'" + serverdata[i].id + "\'>" + serverdata[i].mechineName + "</option>";
                        }
                        var myobj = document.getElementById("mechineList");
                        $("#mechineList").html(optionString);
                        $("#mechineList").selectpicker('refresh');

                    } else {
                        //alert(data.d.msg);
                    }
                }
            })
        }
</script>
