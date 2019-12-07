<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Statisticalquery.aspx.cs" Inherits="autosell_center.main.datastatistics.Statisticalquery" %>
<%@ Register Src="~/ascx/CheckboxListControl.ascx" TagName="CheckboxListControl" TagPrefix="uc1" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>综合查询-自动售卖终端中心控制系统</title>
    <link rel="shortcut icon" href="/main/public/images/favicon.ico" />
    <link href="/main/public/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/common.css" rel="stylesheet" type="text/css" />
    <link href="/main/public/css/style.css" rel="stylesheet" type="text/css" />
    <script src="/main/public/script/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="/main/public/script/Titlelist.js" type="text/javascript"></script>
     <link  href="../../scripts/skin/jedate.css" rel="stylesheet" type="text/css"/>
    <script src="../../scripts/jedate.js" type="text/javascript"></script>
    <script src="../../scripts/jedate.min.js"></script>
    <script type="text/javascript">
        window.onload = function () {
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
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="header"></div>
        <div class="main">
            <div class="main_list">
                <div class="common_title">
                    <h4>
                        <i class="fa fa-plus"></i>
                        <span>综合数据统计与查询</span>
                    </h4>
                </div>
                <div class="common_main">
                    <div class="navlist">
                        <dl>
                            <dt>综合查询<em class="fa fa-cog"></em></dt>
                            <dd>
                                <a class="change acolor"><i class="change icolor fa fa-pie-chart"></i>综合查询</a>
                            </dd>
                        </dl>
                        <dl>
                            <dt>App流量统计<em class="fa fa-cog"></em></dt>
                            <dd>
                                 <a class="change" href="../enterprise/applog.aspx"><i class="change fa fa-bars"></i>App流量统计</a>
                            </dd>
                        </dl>
                    </div>
                    <section class="jiqlist">
                       
                        <div class="memberseach">
                        <h4>查询</h4>
                        <ul>
                             <li>
                                <label>经销商</label>
                                 <select id="czyList" onchange="comchg()">
                               </select>
                            </li>
                             <li>
                                <label>机器列表</label>
                                <uc1:CheckboxListControl ID="cbosDeparentment" runat="server" />
                            </li>
                              <li>
                                <label>商品</label>
                                <select id="productlist">
                                     
                                </select>
                            </li>
                            <li>
                                <label>地区</label>
                                <span>省：</span><select id="s_province" name="s_province"></select>
                                <span>市：</span><select id="s_city" name="s_city"></select>
                                <span>（县）：</span><select id="s_county" name="s_county" ></select>
                            </li>
                            <li>
                                <label>查询时间段</label>
                                <input name="act_stop_timeks" autocomplete="off" type="text" id="start" runat="server" class="input" value="" placeholder="开始时间" readonly="true" />
                            </li>
                            <li>
                                <span>-</span>
                                <input name="act_stop_timeks" autocomplete="off" type="text" id="end" runat="server" class="input" value="" placeholder="结束时间" readonly="true" />
                            </li>
                            <li class="change">
                                <label>销售类型</label>
                                <asp:DropDownList runat="server" id="sellType">
                                    <asp:ListItem Value="0">全部</asp:ListItem>
                                    <asp:ListItem Value="1">订购</asp:ListItem>
                                    <asp:ListItem Value="2">零售</asp:ListItem>
                                </asp:DropDownList>
                            </li>
                             <li>
                                <input  type="text" id="p1"   runat="server"  class="input" value="" placeholder="价格区间" />
                            </li>
                             <li>
                                <input  type="text" id="p2"   runat="server"  class="input" value="" placeholder="价格区间" />
                            </li>
                               <li>
                                <input  type="text" id="memberName"   runat="server"  class="input" value="" placeholder="会员手机、昵称" />
                            </li>
                            <li>
                                <input  type="button" onclick="getData()" value="查看" class="seachbtn"/>
                        

                            </li>
                          
                        </ul>
                    </div>
                        <ul class="bigdata">
                            <li>
                                <h4>总销售量</h4>
                                <div>
                                    <span>
                                        <i class="fa fa-line-chart"></i>
                                         <label id="totalCount"></label>件
                                    </span>
                                    <p>截止到<%=time %></p>
                                </div>
                            </li>
                            <li>
                                <h4>总销售金额</h4>
                                <div>
                                    <span>
                                        <i class="fa fa-money"></i>
                                         <label id="totalMoney"></label>元
                                    </span>
                                    <p>截止到<%=time %></p>
                                </div>
                            </li>
                            <li>
                                <h4>总订单</h4>
                                <div>
                                    <span>
                                        <i class="fa fa-list-alt"></i>
                                         <label id="totalOrder"></label>笔
                                    </span>
                                    <p>截止到<%=time %></p>
                                </div>
                            </li>
                             <li>
                                <h4>总库存</h4>
                                <div>
                                    <span style="width:50%">
                                        <i>订购</i>
                                        <label id="dgNum"></label>
                                    </span>
                                     <span style="width:50%">
                                        <i>零售</i>
                                        <label id="lsNum"></label>
                                    </span>
                                     <p>　</p>
                                </div>
                            </li>
                        </ul>
                    </section>
                </div>
            </div>
        </div>
        <input  id="companyID" runat="server" type="hidden"/>
        <input id="_province" runat="server" type="hidden"/>
        <input id="_city" runat="server" type="hidden"/>
        <input id="_country" runat="server" type="hidden"/>
        <input id="agentID" runat="server" type="hidden"/>
    </form>
</body>
</html>
 
  <script src="../Big_screen/public/area.js"></script>
  <script type="text/javascript">_init_area();</script>
  <script type="text/javascript">
      $(function () {
          qx_judge('zhcx');
          $("#li10").find("a").addClass("aborder");
          getProductList();
          getOperaList();
          //getData();
      })
      function getOperaList()
      {
          $("#czyList").empty();
          if ($("#agentID").val()=="0")
          {
              $(" <option value='0'>全部</option>").appendTo("#czyList");
          }
          $.ajax({
              type: "post",
              url: "Statisticalquery.aspx/getOperaList",
              contentType: "application/json; charset=utf-8",
              dataType: "json",
              data: "{companyID:'" + $("#companyID").val() + "',agentID:'" + $("#agentID").val() + "'}",
              success: function (data) {
                  var serverdata = $.parseJSON(data.d);
                  var serverdatalist = serverdata.length;
                  for (var i = 0; i < serverdatalist; i++) {
                      $(" <option value='" + serverdata[i].id + "'>" + serverdata[i].name + "</option>").appendTo("#czyList");
                  }
              }
          })
      }
      function comchg()
      {
          $("#txtcboName").val("");
          $("#cbosDeparentment_hdscbo").val("");
          $("#divCheckBoxList").empty();
          $.ajax({
              type: "post",
              url: "Statisticalquery.aspx/getMechineList",
              contentType: "application/json; charset=utf-8",
              dataType: "json",
              data: "{operaID:'" + $("#czyList").val() + "',companyID:'" + $("#companyID").val() + "'}",
              success: function (data) {
                  var serverdata = $.parseJSON(data.d);
                  var serverdatalist = serverdata.length;
                  for (var i = 0; i < serverdatalist; i++) {
                      $("<div><input type='checkbox' name='subBox' onclick='changeinfo()' value='" + serverdata[i].sCode + "'>" + serverdata[i].sName + "</div>").appendTo("#divCheckBoxList");
                  }
                   
              }
          })
      }
      function getProductList() {
          $("#productlist").empty();
          $.ajax({
              type: "post",
              url: "Statisticalquery.aspx/getProductList",
              contentType: "application/json; charset=utf-8",
              dataType: "json",
              data: "{companyID:'" + $("#companyID").val() + "'}",
              success: function (data) {
                  var serverdata = $.parseJSON(data.d);
                  var serverdatalist = serverdata.length;
                  for (var i = 0; i < serverdatalist; i++) {
                      $(" <option value='" + serverdata[i].id + "'>" + serverdata[i].name + "</option>").appendTo("#productlist");
                  }
              }
          })
      }
    function getData()
    {
        $.ajax({
            type: "post",
            url: "Statisticalquery.aspx/getData",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{province:'" + $("#s_province").val() + "',city:'" + $("#s_city").val() + "',country:'" + $("#s_county").val() + "',start:'" + $("#start").val() + "',end:'" + $("#end").val() + "',type:'" + $("#sellType").val() + "',companyID:'" + $("#companyID").val() + "',mechineIDStr:'" + $("#cbosDeparentment_hdscbo").val() + "',memberKey:'" + $("#memberName").val() + "',productId:'" + $("#productlist").val() + "',price1:'" + $("#p1").val() + "',price2:'" + $("#p2").val() + "',operaID:'" + $("#czyList").val() + "'}",
            success: function (data) {
                var arr = data.d.split('|');
                $("#totalCount").html(arr[0]);
                $("#totalMoney").html(arr[1]);
                $("#totalOrder").html(arr[2]);
                $("#dgNum").html(arr[3]);
                $("#lsNum").html(arr[4]);
            }
        })
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
                        window.open("/main/Big_screen/big_screen.aspx");
                    }
                    if (menuID == 'zhcx') {//数据统计与查询
                        //location.href = "../datatj/Statisticalquery.aspx";
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
    </script>

 
