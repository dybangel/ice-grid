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
    public partial class notice : System.Web.UI.Page
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
            string sql = "select top 30 * from asm_notice where memberID="+memberID+" order by id desc";
            Util.Debuglog("消息通知菜单sql="+sql,"_消息通知菜单.txt");
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count>0)
            {
                return OperUtil.DataTableToJsonWithJsonNet(dt);
            }
            return "1";
        }
    }
}