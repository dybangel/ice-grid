using autosell_center.util;
using DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace autosell_center.main.equipment
{
    public partial class map : System.Web.UI.Page
    {
        public DataTable tb = new DataTable();
        public string strmapid = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
            }

        }
       
        protected void Button1_Click(object sender, EventArgs e)
        {
           
        }
    }
}