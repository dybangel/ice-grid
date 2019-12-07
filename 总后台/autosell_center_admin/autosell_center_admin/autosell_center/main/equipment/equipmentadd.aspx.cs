using DBUtility;
using System;
using System.Data;

namespace autosell_center.main.equipment
{
    public partial class equipmentadd : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string sql = "select * from asm_mechineType ";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            this.mechineType.DataTextField = "name";
            this.mechineType.DataValueField = "id";
            this.mechineType.DataSource = dt;
            this.mechineType.DataBind();
        }
    }
}