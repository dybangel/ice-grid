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
    public partial class orderMe : System.Web.UI.Page
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
            this.mechine_id.Value = Request.QueryString["mechineID"].ToString();
            string sql = "select * from asm_mechine where companyID=" + comID;
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            this.mechineList.DataTextField = "bh";
            this.mechineList.DataValueField = "id";
            this.mechineList.DataSource = dt;
            this.mechineList.DataBind();
            this.mechineList.Items.Insert(0, new ListItem("所有机器", "0")); //添加项
            this.mechineList.SelectedValue = this.mechine_id.Value;
        }
        [WebMethod]
        public static string getOrderList(string bh, string mechineID, string zq,string companyID)
        {
           
            string sql = " 1=1 ";
            if (!string.IsNullOrEmpty(bh))
            {
                sql += "  and A.orderNO='" + bh + "'";
            }
            if (!string.IsNullOrEmpty(mechineID) && mechineID != "0")
            {
                sql += " and A.mechineID=" + mechineID;
            }
            if (!string.IsNullOrEmpty(zq) && zq != "0")
            {
                sql += " and zq=" + zq;
            }
            string sql1 = "select A.*,b.bh from (select ao.*,am.name,am.phone from asm_order ao left join asm_member  am on ao.memberID=am.id where am.companyID=" + companyID + ") A left join asm_mechine B on A.mechineID=B.id where  1=1 and " + sql;
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