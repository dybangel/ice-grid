using autosell_center.util;
using DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace Consumer
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {


            //更新剩余订单数量
            string sql = "UPDATE asm_order  SET   syNum=totalNum-A.num    FROM (select COUNT(*) num,orderNO from asm_orderDetail where zt in (1,2,3,6,7)  group by orderNO having COUNT(*) >0) A,asm_order   WHERE A.orderNO = asm_order.orderNO";
            DbHelperSQL.ExecuteSql(sql);
            string sql1 = "update asm_orderDetail set zt=2 where  DATEDIFF(dd,GETDATE(),convert(datetime,createTime))<0 and zt in(4)";
            DbHelperSQL.ExecuteSql(sql1);
            string sql2 = "update asm_order set zt=3 where syNum=0";
            DbHelperSQL.ExecuteSql(sql2);
            //更新每日的asm_orderDetail的状态
            string sql4 = "update asm_orderDetail set zt=4 where createTime='" + DateTime.Now.ToString("yyyy-MM-dd") + "' and zt=5";
            DbHelperSQL.ExecuteSql(sql4);
         
            //更新asm_order表 的订单状态zt由0变2为1
            string sql3 = "update asm_order set zt=1 where zt=0 and  DATEDIFF(dd,GETDATE(),convert(datetime,createTime))<0";
            DbHelperSQL.ExecuteSql(sql3);
            //生成机器二维码 
            
            //需要更新订购订单的剩余天数
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}