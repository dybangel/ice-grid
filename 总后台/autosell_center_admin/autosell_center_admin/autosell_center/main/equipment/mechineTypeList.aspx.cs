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

namespace autosell_center.main.equipment
{
    public partial class mechineTypeList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        [WebMethod]
        public static string look(string str)
        {
            string sql1 = "select ROW_NUMBER() OVER ( ORDER BY id ) AS rowNum,* from asm_mechineType";
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
        [WebMethod]
        public static string del(string id)
        {
            string sql = "delete from asm_mechineType where id="+id;
            int a= DbHelperSQL.ExecuteSql(sql);
            if (a > 0)
            {
                //删除该类型的料道默认信息
                string sql2 = "select * from asm_mechine where [version]="+id;
                DataTable dt = DbHelperSQL.Query(sql2).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return "3";
                }
                else {
                    string sql1 = " delete  from asm_ldModel where mechineType=" + id;
                    DbHelperSQL.ExecuteSql(sql1);
                   
                }
                return "1";
            }
            else {
                return "2";
            }
        }
    }
}