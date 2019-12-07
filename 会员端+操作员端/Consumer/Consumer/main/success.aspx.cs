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
    public partial class success : System.Web.UI.Page
    {
        public string money = "0";
        public string companyID = "";
        public DataTable dt;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                money = Request.QueryString["money"].ToString();
                companyID = Request.QueryString["companyID"].ToString();
                string sql = "SELECT * FROM asm_zfbhb WHERE type=3 and companyID=" + companyID;
                dt = DbHelperSQL.Query(sql).Tables[0];
            }
            catch {

            }
        }
    }
}