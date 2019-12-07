using autosell_center.util;
using DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace autosell_center.main.order
{
    public partial class Billofgoods : System.Web.UI.Page
    {
        public string comID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            comID = OperUtil.Get("companyID");
            if (string.IsNullOrEmpty(comID))
            {
                Response.Write("<script>alert('您尚未登录或已长时间未进行操作，请重新登录!');top.location.href='../../index.aspx';</script>");
                return;
            }
            this.time.Value = Request.QueryString["time"].ToString();
            this.mechineID.Value = Request.QueryString["mechineID"].ToString();
             
        }
        [WebMethod]
        public static string sear(string mechineID,string time)
        {
           
            string sql = "select productID,proName,sum(dgNUm) dg,sum(lsNUM)  ls"
                       +" from("
                       +" select ao.productID, ap.proName, count(*) dgNUm, '0'lsNUM from asm_orderDetail ao left join asm_product ap on ao.productID = ap.productID where createTime = '"+time+"' and ao.mechineID = "+mechineID+" and zt!=7 group by ao.productID, ap.proName"
                       +" union all"
                       +" select ar.productID, ap.proName, '0' dgNUm, sum(num) lsNUM from asm_reserve ar left join asm_product ap on ar.productID = ap.productID where ar.mechineID = "+mechineID+" and convert(varchar(100), ar.delivertTime, 23) = '"+time+"'group by ar.productID, ap.proName) C group by productID, proName order by productID";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                string ss = OperUtil.DataTableToJsonWithJsonNet(dt);
                return ss;
            }
            else
            {
                return "1";
            }
        } 
    }
}