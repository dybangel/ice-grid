<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wxorbalanceceshi.aspx.cs" Inherits="Consumer.main.wxorbalanceceshi" %>
<!DOCTYPE html>
<html>
	<head>
		<meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no">
		<meta charset="UTF-8">
        <meta http-equiv="Cache-Control" content="no-cache, no-store, must-revalidate" />
        <meta http-equiv="Pragma" content="no-cache" />
        <meta http-equiv="Expires" content="0" />

		<title>支付方式</title>
		<link rel="stylesheet" type="text/css" href="public/css/common1.css" />
		<link rel="stylesheet" type="text/css" href="public/css/payway.css" />
        <script src="public/script/jquery-3.2.1.min.js"></script>
        <script src='https://res.wx.qq.com/open/js/jweixin-1.3.0.js'></script>
        <style>
            .btnPay {
                width: 100px;
                height:30px;
                color: #fff;
                padding: 0;
                background: #07BF62;
                cursor: pointer;
                border-radius:5px;
                border:none;
                right:10px;
                position:absolute;
                margin-top:-25px;
                display:block;
            }
        </style>
	</head>
	<body>
		<form runat="server">
            
		<div class="main" style="height:100vh;padding-bottom:10px;">
			<div class="yoghourtData">
				<div class="yoghourtIcon">
					<img src="<%=dt1.Rows[0]["httpImageUrl"].ToString() %>"" />
				</div>
				<div class="product">
					<h4><%=dt1.Rows[0]["proName"].ToString() %></h4>
					<p><span>¥：<%=dt1.Rows[0]["price0"].ToString() %></span><%=djName %>¥<%=price %></p>
                     <%
                    if (isMZ)
                    {%>
                     <p> <%=mzContent %></p>
                     <p> <%=chanum %>¥<%=afterMoney %></p> <% }%>
                    
				</div>
			</div>
			<div class="method">
				<ul>
                     <li>
						<div class="wallet" onclick="balancepay()">
							<img  src="public/images/wallet.png" />
						</div>
						<div class="payment"  >
							<h4 onclick="balancepay()">会员钱包支付</h4>
							<span onclick="balancepay()">钱包支付享<%=djName %>价¥<%=price %></span>
						</div>
                         <%
                 if (double.Parse(dt2.Rows[0]["AvailableMoney"].ToString()) < double.Parse(price))
                {%>
                     
                         <input  type="button"  class="btnPay" value="充值" onclick="czBtn()"/>
                            <% }
                                else {%>
                         <p onclick="balancepay()">余额：<%=dt2.Rows[0]["AvailableMoney"].ToString() %></p>
                                <%}
                         %>
						
					</li>
					<li onclick="wxpay()">
						<div class="wallet">
							<img  src="public/images/wechat2.png" />
						</div>
						<div class="payment weixin">
							<h4>微信支付</h4>
						</div>
					</li>
				</ul>
			</div>
			<div class="tips">
				温馨提示：<br />
			支付时请注意环境，注意安全
			</div>
	    </div>
          <input type="hidden" id="_mechineID" runat="server"/>
          <input type="hidden" id="_companyID" runat="server"/>
          <input type="hidden" id="_unionID" runat="server"/>
          <input type="hidden" id="_money" runat="server"/>
          <input type="hidden" id="_productID" runat="server"/>
          <input type="hidden" id="_openID" runat="server"/>
          <input type="hidden" id="_dgOrderID" runat="server"/>
          <input type="hidden" id="_type" runat="server"/>
          <input type="hidden" id="_sftj" runat="server"/>
            
		</form>
	</body>
</html>
<script>
    function czBtn()
    { 
        var url = "https://mp.weixin.qq.com/mp/homepage?__biz=MzU4NDAzMTE5MQ==&hid=1&sn=620f9e09f953c9f939883e1fa30a9a8b";
        window.location.href = url;
    }
    $(function () {
        document.body.style.overflow = 'hidden';
    })
    function wxpay()
    {
        setTimeout(function () {
            console.log("执行了关闭1");
            window.close();
        }, 2000);
        var url = "wxpay.aspx?companyID=" + $("#_companyID").val() + "&mechineID=" + $("#_mechineID").val() + "&money=" + $("#_money").val() + "&unionID=" + $("#_unionID").val() + "&productID=" + $("#_productID").val() + "&openID=" + $("#_openID").val() + "&dgOrderDetailID=" + $("#_dgOrderID").val() + "&type=" + $("#_type").val() + "&sftj=" + $("#_sftj").val();
        window.location.href = url;
        
    }
    var flag=true;
    function balancepay()
    {
        if(flag){//限制支付一单
            flag=false;
        }else{
            return;
        }
        if(confirm("将从您的余额直接扣除"))
        {
          
            $.ajax({
                type: "post",
                url: "wxorbalanceceshi.aspx/yzPwd",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: "{unionID:'" + $("#_unionID").val() + "',money:'" + $("#_money").val() + "',companyID:'" + $("#_companyID").val() + "',productID:'" + $("#_productID").val() + "',mechineID:'" + $("#_mechineID").val() + "',type:'" + $("#_type").val() + "',dgOrderDetailID:'" + $("#_dgOrderID").val() + "',sftj:'" + $("#_sftj").val() + "'}",
                success: function (data) {
                    if (data.d == "1") {
                        alert("支付密码不正确");
                    } else if (data.d == "2") {
                        alert("余额不足");
                    } else if (data.d == "3") {
                        alert("支付完成");
                        $('.paypageMain').removeClass('paypageMainTop');
                        location.href = "success.aspx?money=" +<%=price%> +"&companyID=" + $("#_companyID").val();
                    } else if (data.d == "4") {
                        alert("该笔订单已经支付完成");
                    } else if (data.d == "5") {
                        alert("当前机器库存不足，请等待配送员上货");
                    }
                }
            })
        }else{
        
        }
        return;
        var url = "paypage.aspx?companyID=" + $("#_companyID").val() + "&mechineID=" + $("#_mechineID").val() + "&money=" + $("#_money").val() + "&unionID=" + $("#_unionID").val() + "&productID=" + $("#_productID").val() + "&dgOrderDetailID=" + $("#_dgOrderID").val() + "&type=" + $("#_type").val();
        //window.location.href = url;
    }
</script>
 