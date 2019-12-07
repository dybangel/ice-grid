using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace autosell_center.main.enterprise
{
    public partial class Paymentzf : System.Web.UI.Page
    {
        public string company_ID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            company_ID = Request.QueryString["companyID"].ToString();
        }
    }
}