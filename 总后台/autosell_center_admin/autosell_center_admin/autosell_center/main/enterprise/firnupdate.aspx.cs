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
    public partial class firnupdate : System.Web.UI.Page
    {
        private string company_ID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            this._companyID.Value = Request.QueryString["companyID"].ToString();
            company_ID = this._companyID.Value;
            if (!IsPostBack)
            {
                string sql = " select id,name from asm_opera where companyID=0";
                DataTable dd = DbHelperSQL.Query(sql).Tables[0];
                this.business.DataValueField = "id";
                this.business.DataTextField = "name";
                this.business.DataSource = dd;
                this.business.DataBind();
                string sql1 = "select * from asm_company where id='"+this._companyID.Value+"' ";
                DataTable dt = DbHelperSQL.Query(sql1).Tables[0];
                this.bh.Value = dt.Rows[0]["comBH"].ToString();
                this.code.Value= dt.Rows[0]["code"].ToString();
                this.companyName.Value= dt.Rows[0]["name"].ToString();
                this.fzr.Value= dt.Rows[0]["linkman"].ToString(); 
                this.cwr.Value= dt.Rows[0]["fianceman"].ToString(); 
                this.phone.Value=dt.Rows[0]["linkPhone"].ToString();
                this.business.SelectedValue= dt.Rows[0]["business"].ToString();
                this.pic.Src= "../../"+dt.Rows[0]["yyzzPath"].ToString();
                this.piclogo.Src= "../../"+dt.Rows[0]["path"].ToString();
            }
           
        }
        [WebMethod]
        public static string update(string id)
        {
            string sql = "select * from asm_mechine where id=" + id;
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["validateTime"].ToString() + "|" + dt.Rows[0]["zt"].ToString();
            }
            else
            {
                return "";
            }
        }
    }
}