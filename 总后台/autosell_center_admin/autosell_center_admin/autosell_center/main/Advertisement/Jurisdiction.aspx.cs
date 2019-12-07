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

namespace autosell_center.main.Advertisement
{
    public partial class Jurisdiction : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string sql1 = "select * from asm_company";
            DataTable dt = DbHelperSQL.Query(sql1).Tables[0];
            this.companyList.DataTextField = "name";
            this.companyList.DataValueField = "id";
            this.companyList.DataSource = dt;
            this.companyList.DataBind();
            this.companyList.Items.Insert(0, new ListItem("所有企业", "0")); //添加项
        }
        [WebMethod]
        public static string search(string gsid, string bh)
        {
            string sql = " where 1=1";
            if (bh != "")
            {
                sql += " and am.bh='" + bh + "'";
            }
            if (gsid != "0")
            {
                sql += " and am.companyID='" + gsid + "'";
            }
            string sql1 = "select am.*,ac.name,amt.name mechineType,(select count(*) from asm_videoAddMechine where mechineID=am.id) num,case am.statu when '0' then '正常' when '1' then '脱机' when '2' then '温度异常'    else '其他' end sta from asm_mechine am left join asm_company ac on am.companyID=ac.id  left join asm_mechineType amt on am.version=amt.id " + sql;
            DataTable dt = DbHelperSQL.Query(sql1).Tables[0];
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