$(function () {
   
    var name = getCookie("operaName");
    
    var titleImg = "<div class='header_logo'><a href='/main/enterprise/SellCenter.aspx'><img src='/main/public/images/logo.png' alt=''/></a></div>";
    var titleBegin = "<nav><ul>";
    var titleNqgl = "<li><a class='change' href='/main/enterprise/SellCenter.aspx'>奶企管理</a></li>";
    var titleSbgl = "<li><a class='change' href='/main/equipment/Allequipment.aspx'>设备管理</a></li>";
    var titleHygl = "<li><a class='change' href='/main/member/memberlist.aspx'>会员管理</a></li>";
    var titleGly = "<li><a class='change' href='/main/Adminlist/adminlist.aspx'>管理员管理</a></li>";
    var titleCpgl = "<li><a class='change' href='/main/product/productlist.aspx'>产品管理</a></li>";
    //var titleSell = "<li><a class='change' href='/main/product/dglist.aspx'>订单管理</a></li>";
    var titleSell = "<li><a class='change' href='/main/product/order.aspx'>订单管理</a></li>";
    var titleChart = "<li><a class='change' href='/main/Big_screen/bigChart.aspx'>成交动态</a></li>";
    var titleSjtj = "<li><a class='change' href='/main/datastatistics/Statisticalquery.aspx'>数据统计与查询</a></li>";
    //var titleFx = "<li><a class='change' href='#'    onclick=\"a('cjdtt')\">分析</a></li>";
    //var titleSjdp = "<li><a class='change' href='/main/Big_screen/big_screen.aspx' target='_blank'>数据大屏</a></li>";
    var titleGggl = "<li><a class='change' href='/main/Advertisement/video.aspx' >广告管理</a></li>";
    var titleXtsz = "<li><a class='change' href='/main/enterprise/syssetting.aspx'>系统设置</a></li>";
    var titleEnd = "</ul></nav>";
    var titleUser = "<dl class='user_set'><dd>欢迎你，" + name + "</dd><dd><a class='change' href='/main/upPassword/UpPassWord.aspx'>修改密码</a></dd><dd><a class='change' href='/index.aspx'>退出系统</a></dd></dl>";
    $(".header").html(titleImg + titleBegin + titleNqgl + titleSbgl + titleHygl + titleGly + titleCpgl + titleSell + titleChart + titleSjtj  + titleGggl + titleXtsz + titleEnd + titleUser);

    var i = 0;
    $(".header").find("nav").find("li").each(function () {
        this.id = "li" + i;
        i++;
    });
});
function getCookie(name) {  
  
    var arr, reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");  
  
    if (arr = document.cookie.match(reg)) 
        return unescape(arr[2]);  
    else  
        return null;  
}   