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

namespace autosell_center
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        [WebMethod]
        public static string login(string name,string pwd,string qx)
        {
            if (qx == "1")
            {
                string sql = "select * from [dbo].[asm_manager] where bh='" + name + "' and pwd='" + pwd + "'";
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    OperUtil.Add("operaID", "0");
                    OperUtil.Add("AdminOperaID", "0");
                    OperUtil.setCooki("operaName",dt.Rows[0]["bh"].ToString());
                    return "1";
                }
                else
                {
                    return "2";
                }
            }
            else if (qx == "2")
            {
                string sql = "select * from asm_opera where name='" + name + "' and pwd='" + pwd + "' and companyID=0";
                DataTable dd = DbHelperSQL.Query(sql).Tables[0];
                if (dd.Rows.Count > 0)
                {
                    OperUtil.Add("operaID", dd.Rows[0]["id"].ToString());
                    OperUtil.Add("AdminOperaID", dd.Rows[0]["id"].ToString());
                    OperUtil.setCooki("operaName", dd.Rows[0]["bh"].ToString());
                    return "1";
                }
                else {
                    return "2";
                }
            }
            return "2";
           
        }
    }
}