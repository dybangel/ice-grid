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

namespace autosell_center.main.enterprise
{
    public partial class mallImg : System.Web.UI.Page
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
            this.companyID.Value = comID;
           
        }
        [WebMethod]
        public static object getImage(string companyID)
        {
           
            string sql = "select * from asm_company where id=" + companyID;
            DataTable dd = DbHelperSQL.Query(sql).Tables[0];
            if (dd.Rows.Count > 0)
            {

                return new { result = 200, img1=dd.Rows[0]["p5"], img2 = dd.Rows[0]["p6"], img3 = dd.Rows[0]["p7"] };
            }
            return new { result = 100, msg = "" };
        }
    }
}