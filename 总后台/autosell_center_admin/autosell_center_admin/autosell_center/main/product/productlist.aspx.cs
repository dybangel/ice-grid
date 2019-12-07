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

namespace autosell_center.main.product
{
    public partial class productlist : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                initData();
            }
        }
        public void initData()
        {
            string sql = "select * from asm_company";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            this.companyList.DataTextField = "name";
            this.companyList.DataValueField = "id";
            this.companyList.DataSource = dt;
            this.companyList.DataBind();
            this.companyList.Items.Insert(0, new ListItem("全部", "0")); //添加项
        }
        [WebMethod]
        public static string getProductList(string keyword,string qy, string pageCurrentCount)
        {
            string sql1 = " and 1=1";
            if (keyword.Trim()!="")
            {
                sql1 += " and C.proName like '%"+keyword+"%'";
            }
            if (qy!="0")
            {
                sql1 += " and C.companyID="+qy;
            }

            string sql = "select C.*,D.typeName from (select A.*,B.name  from(select ap.* from asm_product ap left join  asm_mechine ac on ap.mechineID=ac.id) A left join asm_company B  on A.companyID=B.id) C left join asm_protype D on C.protype=D.productTypeID where 1=1 " + sql1;
            int startIndex = (int.Parse(pageCurrentCount) - 1) * Config.pageSize + 1;
            int endIndex = int.Parse(pageCurrentCount) * Config.pageSize;

            DataTable dt = Config.getPageDataTable("order by T.productID ", sql, startIndex, endIndex);
            DataTable da = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {

                double d = double.Parse((da.Rows.Count / double.Parse(Config.pageSize.ToString())).ToString());
                string ss = Math.Ceiling(d) + "@" + OperUtil.DataTableToJsonWithJsonNet(dt);

                //string ss =OperUtil.DataTableToJsonWithJsonNet(da);
                return ss;
            }
            else
            {
                return "1";
            }
        }
    }
}