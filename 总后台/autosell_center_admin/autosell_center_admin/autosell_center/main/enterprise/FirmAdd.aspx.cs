using DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace autosell_center.main.enterprise
{
    public partial class FirmAdd : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string sql = " select id,name from asm_opera where companyID=0 ";
                DataTable dd = DbHelperSQL.Query(sql).Tables[0];
                this.business.DataValueField = "id";
                this.business.DataTextField = "name";
                this.business.DataSource = dd;
                this.business.DataBind();


            }
           
        }
    }
}