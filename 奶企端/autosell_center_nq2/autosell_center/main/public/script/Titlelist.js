$(function () {

    var name = getCookie("operaName");
    var titleImg = "<div class='header_logo'><a href='#'><img src='/main/public/images/logo.png' alt=''/></a></div>";
    var titleBegin = "<nav><ul>";
    // var titleNqgl = "<li><a class='change' href='/main/enterprise/Distributor.html'>经销商管理</a></li>";
    var titleSbgl = "<li><a class='change' onclick=\"qx_judge('hylb')\">会员管理</a></li>";
    var titleHygl = "<li><a class='change'  href='/main/order/Productform.aspx'>订单管理</a></li>";
    var titleSjdp = "<li><a class='change' onclick=\"qx_judge('cplb')\">商品管理</a></li>";
    var titleCpgl = "<li><a class='change' onclick=\"qx_judge('sblb')\">设备管理</a></li>";
    var titleSjtj = "<li><a class='change' onclick=\"qx_judge('glygl')\">管理员管理</a></li>";
    var titleGggl = "<li><a class='change' onclick=\"qx_judge('szhd')\">活动管理</a></li>";
    var titleXtsz = "<li><a class='change' onclick=\"qx_judge('spgl')\">广告管理</a></li>";
    var titleGzh = "<li><a class='change' href='/main/enterprise/authPage.aspx' >系统设置</a></li>";// href='/main/enterprise/authPage.aspx' onclick=\"qx_judge('gzhsz')\" 
    var titleFx = "<li><a class='change' target= '_blank'  onclick=\"qx_judge('sjdp')\">数据大屏</a></li>";
    var titleSJ1 = "<li><a class='change' onclick=\"qx_judge('zhcx')\">数据统计与查询</a></li>";
    var titleSJ = "<li><a class='change' href='/main/Big_screen/bigChart.aspx'>成交动态图</a></li>";
    var titleEnd = "</ul></nav>";
    var titleUser = "<dl class='user_set'><dd>欢迎你，" + name + "</dd><dd><a class='change' href='/main/upPassword/UpPassWord.aspx'>修改密码</a></dd><dd><a class='change' href='/index.aspx'>退出系统</a></dd></dl>";

    $(".header").html(titleImg + titleBegin + titleSbgl + titleHygl + titleSjdp + titleCpgl + titleSjtj + titleGggl + titleXtsz + titleGzh + titleFx + titleSJ1 + titleSJ + titleEnd + titleUser);

    var i = 1;
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
function qx_judge(menuID)
{
   
    //首先验证账号和密码正确
    $.ajax({
        url: "../../../ashx/asm.ashx",
        type: 'post',
        dataType: 'json',
        timeout: 10000,
        data: {
            action: "qx_judge",
            menu:menuID
        },
        success: function (resultData) {
           
            if (resultData.result=="ok")//允许查看跳转
            {
                if (menuID == 'hylb') {//会员列表
                    
                    location.href = "/main/member/memberlist.aspx";
                }
                if (menuID == 'jqddtj') {//订单管理
                   
                    // location.href = "/main/order/orderform.aspx";
                    location.href = "/main/order/Productform.aspx";
                }
                if (menuID == 'cplb') {//商品列表
                    location.href = "/main/product/productlist.aspx";
                }
                if (menuID == 'sblb') {//设备管理
                    location.href = "/main/equipment/equipmentlist.aspx";
                }
                if (menuID == 'glygl') {//管理员管理
                    location.href = "/main/Administrators/adminlist.aspx";
                }
                if (menuID == 'szhd') {//活动管理
                    location.href = "/main/activity/activity.aspx";
                }
                if (menuID == 'spgl') {//广告管理
                    location.href = "/main/Advertisement/video.aspx";
                }
                if (menuID == 'gzhsz') {//公众号管理
                    location.href = "/main/enterprise/Thepublicjb.aspx";
                }
                if (menuID == 'sjdp') {//数据大屏
                   
                  
                }
                if (menuID == 'zhcx') {//数据统计与查询
                    location.href = "/main/datatj/Statisticalquery.aspx";
                }
            } else if (resultData.result == "notLogin")//没有查看权限
            {
                location.href = "../../../../blank.aspx";
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