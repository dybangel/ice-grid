using autosell_center.util;
using Consumer.cls;
using DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Consumer.main
{
    public partial class Resalerecord : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Util.getMemberID() != "0")
            {
                this.memberID.Value = Util.getMemberID();
            }
            else
            {
                Response.Redirect("../WXCallback.aspx");
            }
            

        }
        [WebMethod]
        public static string getOrderList(string memberID)
        {
            string sql = "select ao.*,ap.proName,ap.description from asm_orderDetail ao left join asm_product ap on ao.productID=ap.productID where memberID=" + memberID+" and zt in (3,6)";
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