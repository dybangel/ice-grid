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
    public partial class orderlist : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (OperUtil.getCooki("vshop_openID") != "0")
            {
                this.memberID.Value = Util.getMemberID();
            }
            else
            {
                string userAgent = Request.UserAgent;
                if (userAgent.ToLower().Contains("micromessenger"))
                {
                    Response.Redirect("WXCallback.aspx?companyID=" + this.companyID.Value);
                    return;
                }

            }


        }
        [WebMethod]
        public static string getOrderList(string memberID)
        {
            string sql = "select ao.*,ap.proName,ap.description,ap.httpImageUrl from asm_order ao left join asm_product ap on ao.productID=ap.productID where memberID=" + memberID+ " and fkzt=1 order by createTime desc";
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