using autosell_center.util;
using Consumer.cls;
using DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Consumer.main
{
    public partial class pickupcode : System.Web.UI.Page
    {
        public DataTable dt;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (OperUtil.getCooki("vshop_openID") != "0")
            {
                this.memberID.Value = Util.getMemberID();
                initData();
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
        public void initData()
        {
            string sql = "select * from asm_orderDetail  where memberID="+this.memberID.Value+" and createTime='"+DateTime.Now.ToString("yyyy-MM-dd")+ "' and zt=4 and ldNO!=''";
            dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count>0)
            {
                
            }
        }
    }
}