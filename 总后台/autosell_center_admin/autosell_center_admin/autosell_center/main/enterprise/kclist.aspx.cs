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
    public partial class kclist : System.Web.UI.Page
    {
        public string title = "";
        public string CompanyID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            this.mechineID.Value = Request.QueryString["mechineID"].ToString();
            this.company_ID.Value = Request.QueryString["companyID"].ToString();
            CompanyID = this.company_ID.Value;
            if (this.mechineID.Value != "")
            {
                string sql = "select * from asm_mechine where id=" + this.mechineID.Value;
                DataTable dd = DbHelperSQL.Query(sql).Tables[0];
                if (dd.Rows.Count > 0)
                {
                    title = dd.Rows[0]["mechineName"].ToString();
                }
            }
        }
        [WebMethod]
        public static string getList(string mechineID)
        {
            string sql = "select * from(select sum(ld_productNum) num,(select proName from asm_product where productID=al.productID) name,(select isnull(sum(totalMoney),0) totalMoney from asm_sellDetail where mechineID=" + mechineID + " and bz!='退款成功' and asm_sellDetail.productID=al.productID) money from asm_ldInfo al where mechineID=" + mechineID + " and productID!='' group by productID) A  ";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                string numStr = "", nameStr = "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    numStr += dt.Rows[i]["num"].ToString().Trim() + ",";
                    nameStr += dt.Rows[i]["name"].ToString() + ",";
                }
                numStr = numStr.Substring(0, numStr.Length - 1);
                nameStr = nameStr.Substring(0, nameStr.Length - 1);

                return numStr + "@@@" + nameStr + "@@@" + OperUtil.DataTableToJsonWithJsonNet(dt);
            }
            return "1";
        }
    }
}