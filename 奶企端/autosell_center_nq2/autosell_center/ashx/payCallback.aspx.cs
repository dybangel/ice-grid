using DBUtility;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace autosell_center.ashx
{
    public partial class payCallback : System.Web.UI.Page
    {
        public override void ProcessRequest(HttpContext context)
        {
          
            context.Response.ContentType = "multipart/form-data";
            string billNo = context.Request.Form["billno"];
            string payTime = context.Request.Form["pay_time"];
            string out_trade_no = context.Request.Form["out_trade_no"];
            //支付成功向asm_pay 表插入记录
            string sql = "insert into asm_pay(out_trade_no, billno, payTime,statu) values('"+out_trade_no+"', '"+billNo+"', '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',0)";
            int a= DbHelperSQL.ExecuteSql(sql);
           
            if (a>0)
            {
                context.Response.Write("SUCCESS");
            }
           

        }
    }
}