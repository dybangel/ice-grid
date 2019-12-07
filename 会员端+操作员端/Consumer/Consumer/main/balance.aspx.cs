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
    public partial class balance : System.Web.UI.Page
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
                    Response.Redirect("WXCallback.aspx?companyID=" + OperUtil.getCooki("companyID"));
                    return;
                }

            }
            
        }
        [WebMethod]
        public static string sear(string memberID)
        {
            string sql = "select * from asm_moneyChange where memberID="+memberID+" order by id desc";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                string ss = OperUtil.DataTableToJsonWithJsonNet(dt);
                return ss;
            }
            return "1";
        }
    }
}