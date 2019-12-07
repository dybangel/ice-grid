using autosell_center.util;
using DBUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace autosell_center
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            //更新每日的asm_orderDetail的状态
            //string sql = "update asm_orderDetail set zt=4 where createTime='" + DateTime.Now.ToString("yyyy-MM-dd") + "' and zt=5";
            //DbHelperSQL.ExecuteSql(sql);
            //string sql1 = "update asm_orderDetail set zt=2 where  DATEDIFF(dd,GETDATE(),convert(datetime,createTime))<0 where zt in(3,4)";
            //DbHelperSQL.ExecuteSql(sql1);
            ////更新asm_order表 的订单状态zt由0变2为1
            //string sql3 = "update asm_order set zt=1 where zt=0 and  DATEDIFF(dd,GETDATE(),convert(datetime,createTime))<0";
            //DbHelperSQL.ExecuteSql(sql3);
            
           
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