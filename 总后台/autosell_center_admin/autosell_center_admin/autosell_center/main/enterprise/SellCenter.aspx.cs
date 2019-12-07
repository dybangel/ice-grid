using autosell_center.util;
using DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace autosell_center.main.enterprise
{
    public partial class SellCenter : System.Web.UI.Page
    {
        public DataTable dt;
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
        [WebMethod]
        public static string sear(string keyword)
        {
            string sql1 = "";
            if (keyword.Trim() != "")
            {
                sql1 = " where comBH like '%"+keyword+"%' or name like '%"+keyword+"%'";
            }
            string sql = "select *,(select count(*) from asm_mechine am where am.companyID=ac.id ) num from asm_company  ac "+sql1;
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count>0)
            {
                string ss = OperUtil.DataTableToJsonWithJsonNet(dt);
                return ss;
            }
            return "1";
        }
        [WebMethod]
        public static string getPath(string id)
        {
            string sql = "select * from asm_company where id=" + id;
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["yyzzPath"].ToString();
            }
            else
            {
                return "";
            }
        }
    }
}