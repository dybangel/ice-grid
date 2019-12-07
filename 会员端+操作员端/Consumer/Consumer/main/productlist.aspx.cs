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

namespace Consumer.main
{
    public partial class productlist : System.Web.UI.Page
    {
        public string mechineID = "";
        public string name = "";
        public string address = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            this.mechine_id.Value = Request.QueryString["mechineID"].ToString();
            mechineID = this.mechine_id.Value;
            string sql = "select * from asm_mechine where id="+this.mechine_id.Value;
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count>0)
            {
                name = dt.Rows[0]["mechineName"].ToString();
                address = dt.Rows[0]["addres"].ToString();
                this.company_ID.Value = dt.Rows[0]["companyID"].ToString();

            }
        }
        [WebMethod]
        public static string getProductType(string companyID)
        {
            string sql = "select * from asm_protype where companyID="+companyID;
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
        [WebMethod]
        public static string getProductList(string typeID,string companyID)
        {
            if (typeID != "0")
            {
                string sql = "select * from asm_product where companyID=" + companyID + " and protype=" + typeID;
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
            else {
                string sql = "select * from asm_product where companyID=" + companyID;
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
        [WebMethod]
        public static string getProductList2(string companyID)
        {
            string sql = "select * from asm_product where companyID=" + companyID;
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