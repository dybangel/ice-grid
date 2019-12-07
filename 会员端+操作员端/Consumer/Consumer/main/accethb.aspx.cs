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
    public partial class accethb : System.Web.UI.Page
    {
        public string companyID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            this._companyID.Value = Request.QueryString["companyID"].ToString();
            this._openID.Value = Request.QueryString["openID"].ToString();
            string sql12 = "select * from asm_company where id="+this._companyID.Value;
            DataTable dd = DbHelperSQL.Query(sql12).Tables[0];
            if (dd.Rows.Count > 0)
            {
                this._p11.Value = dd.Rows[0]["p4"].ToString();
                this._p12.Value = dd.Rows[0]["p12"].ToString();
            }
        }
        [WebMethod]
        public static string accetHB(string openID,string companyID)
        {
            string sql = "select * from asm_member where openID='"+openID+"'";

            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                //更新字段hongbaoF
                string sql1 = "select * from asm_company where id="+companyID;
                DataTable d1 = DbHelperSQL.Query(sql1).Tables[0];
                if (d1.Rows.Count>0&&d1.Rows[0]["p11"].ToString()!="")
                {
                    try
                    {
                        double.Parse(d1.Rows[0]["p11"].ToString());
                        string update = "update asm_member set hongbaoF='"+ d1.Rows[0]["p4"].ToString() + "' where openID='"+openID+"'";
                        DbHelperSQL.ExecuteSql(update);
                        return "1";
                    }
                    catch {
                        return "3";//红包金额设置错误
                    }
                }
             
            }
            else {
                return "2";
            }
            return "4";
        } 
    }
}