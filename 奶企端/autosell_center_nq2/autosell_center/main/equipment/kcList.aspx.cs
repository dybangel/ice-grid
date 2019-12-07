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

namespace autosell_center.main.equipment
{
    public partial class kcList : System.Web.UI.Page
    {
        public DataTable dt;
        public string mechineID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            
            this.mechine_id.Value = Request.QueryString["mechineID"].ToString();
            mechineID = this.mechine_id.Value;
            string sql = "select * from asm_mechine where id=" + this.mechine_id.Value;
            dt = DbHelperSQL.Query(sql).Tables[0];

        }
        [WebMethod]
        public static string getLDInfo(string mechineID)
        {
            
            string sql = "select al.*,ap.proName from asm_ldinfo al left join asm_product ap on al.productID=ap.productID where al.mechineID="+mechineID+ " and al.statu=1 ORDER BY ldno";
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