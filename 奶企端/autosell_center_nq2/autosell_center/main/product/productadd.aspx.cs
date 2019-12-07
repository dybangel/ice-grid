using autosell_center.util;
using Consumer.cls;
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

namespace autosell_center.main.product
{
    public partial class productadd : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.companyID.Value = OperUtil.Get("companyID");
            this._operaID.Value = OperUtil.Get("operaID");
            if (string.IsNullOrEmpty(OperUtil.Get("companyID")))
            {
                Response.Write("<script>alert('您尚未登录或已长时间未进行操作，请重新登录!');top.location.href='../../index.aspx';</script>");
                return;
            }
           
            string sql = "select * from asm_protype where 1=1";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            this.typeLB.DataTextField = "typeName";
            this.typeLB.DataValueField = "productTypeID";
            this.typeLB.DataSource = dt;
            this.typeLB.DataBind();

            string sql1 = "select * from asm_brand where companyID="+this.companyID.Value;
            DataTable dts = DbHelperSQL.Query(sql1).Tables[0];
            this.brandlist.DataTextField = "brandName";
            this.brandlist.DataValueField = "id";
            this.brandlist.DataSource = dts;
            this.brandlist.DataBind();
              
        }
        [WebMethod]
        public static object judge(string operaID, string menuID)
        {
            Boolean b = Util.judge(operaID, menuID);
            if (b)
            {
                return new { code = 200 };
            }
            else
            {
                return new { code = 500 };
            }
        }
    }
}