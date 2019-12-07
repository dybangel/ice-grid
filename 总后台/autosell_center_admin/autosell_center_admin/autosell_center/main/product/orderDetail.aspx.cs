
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
    public partial class orderDetail : System.Web.UI.Page
    {
        public string orderNO = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            orderNO = Request.QueryString["orderNO"].ToString();
            this._orderID.Value = orderNO;
            
        }
        
        [WebMethod]
        public static string getOrderList(string zt,string orderNO, string pageCurrentCount)
        {
            
            string sql = " orderNO = '" + orderNO + "' ";

            if (zt != "0")
            {
                sql += " and zt='"+zt+"'";
            }
            string sql1 = "select *,"
                        + " (select name from asm_member where asm_member.id = asm_orderlistDetail.memberID) name ,"
                        + " (select proName from asm_product where asm_product.productID = asm_orderlistDetail.productID) pname,"
                        + " case zt when '1' then '已完成' when '2' then '已失效' when '3' then '已转售' when '4' then '待取货' when '5' then '待配送' when '6' then '已售出' when '7' then '兑换' else '' end ztName"
                        + " from asm_orderlistDetail  where  " + sql;

            int startIndex = (int.Parse(pageCurrentCount) - 1) * Config.pageSize + 1;
            int endIndex = int.Parse(pageCurrentCount) * Config.pageSize;

            DataTable dt = Config.getPageDataTable("order by T.id ", sql1, startIndex, endIndex);
            DataTable da = DbHelperSQL.Query(sql1).Tables[0];
            if (dt.Rows.Count > 0)
            {

                double d = double.Parse((da.Rows.Count / double.Parse(Config.pageSize.ToString())).ToString());
                string ss = Math.Ceiling(d) + "@" + OperUtil.DataTableToJsonWithJsonNet(dt);

                //string ss =OperUtil.DataTableToJsonWithJsonNet(da);
                return ss;
            }
            else
            {
                return "1";
            }
        }

    }
}