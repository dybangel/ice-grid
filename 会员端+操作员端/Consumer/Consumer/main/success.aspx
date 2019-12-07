<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="success.aspx.cs" Inherits="Consumer.main.success" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
     <meta charset="utf-8" />
    <!-- 轮播样式js -->
    <link href="/main/public/css/swiper.min.css" rel="stylesheet" />
    <script src="/main/public/script/swiper.min.js" type="text/javascript"></script>
    <style>
        .swiper-container{
            position: absolute;
            bottom: 0;
            left: 0;
            width: 100%;
            max-width: 100%;
        }
        .swiper-wrapper,.swiper-slide{
            max-width: 100%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" style="width: 100%;">
        
        <div style="margin-top:22vh;text-align:center">
        
            <div style="width:100%;">
                 <img src="public/images/lgo.png" style="text-align:center;width:250px;height:250px;"/>
            </div>
            <h2 style="text-align:center;font-size:50px;color:#64B3E1">支付成功</h2>
            <h3 style="text-align:center;font-size:40px;color:#999;">本次消费￥<%=money %></h3>
      </div>
        <div>
             <section class="homeTop" >
                <div class="homeLun swiper-container">
                    <div class="swiper-wrapper">
                       <% 
                           if (dt != null && dt.Rows.Count > 0)
                           {
                               for (int i=0;i<dt.Rows.Count;i++) {%>
                                 <div class="swiper-slide">
                                    <a href="<%=dt.Rows[i]["url"].ToString() %>"">
                                         <img src="<%=dt.Rows[i]["path"].ToString() %>"" alt=""/>
                                    </a>
                                 </div>
                              <% } %>
                                
                           <%}else {%>
                                 <div class="swiper-slide">
                                    <a href="#">
                                         <img src="/main/public/images/12.jpg" alt=""/>
                                    </a>
                                 </div>
                           <%} %>
                        
                    </div>
                    <div class="swiper-pagination"></div>
                    <div class="swiper-button-next"></div>
                    <div class="swiper-button-prev"></div>
                </div>
            </section>
        </div>
    </form>
</body>
</html>
<script>
    var swiper = new Swiper('.swiper-container', {
        pagination: '.swiper-pagination',
        nextButton: '.swiper-button-next',
        prevButton: '.swiper-button-prev',
        paginationClickable: true,
        //spaceBetween: 30,
        centeredSlides: true,
        autoplay: 3000,
        autoplayDisableOnInteraction: false
    });
</script>
