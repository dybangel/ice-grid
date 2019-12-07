using autosell_center.util;
using Consumer.cls;
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
    public partial class brandList : System.Web.UI.Page
    {
        private string comID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            comID = OperUtil.Get("companyID");
            this.companyId.Value = comID;
            this._operaID.Value = OperUtil.Get("operaID");
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
        [WebMethod]
        public static string getBrandList(string keyword,string companyId)
        {
            string strSql = " 1=1 ";
            if (!string.IsNullOrEmpty(keyword))
            {
                strSql += " and brandName like '%"+keyword+"%'";
            }
            if (!string.IsNullOrEmpty(companyId))
            {
                strSql += " and companyID=" + companyId;
            }
            string sql = "select * from asm_brand where is_del=0 and  "+strSql;
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count>0)
            {
                return OperUtil.DataTableToJsonWithJsonNet(dt);
            }
            return "";
        }
        [WebMethod]
        public static object ok(string brandName,string companyId)
        {
            string sql = "select * from asm_brand where brandName='"+brandName+ "' and companyID="+companyId;
            DataTable dt= DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count>0)
            {
                return new  { code = 500,msg="名称重复" };
            }
            else
            {
                string update = "insert into asm_brand(brandName,companyID) values('"+brandName+"','"+companyId+"')";
                int a= DbHelperSQL.ExecuteSql(update);
                if (a > 0)
                {
                    return new { code = 0, msg = "添加成功" };
                }
                else {
                    return new { code = 500, msg = "添加失败" };
                }
            }
        }
        [WebMethod]
        public static object del(string brandID)
        {
            string sql = "select * from asm_product where is_del=0 and brandID="+brandID;
            DataTable dt= DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count>0)
            {
                return new { code = 500, msg = "该品牌正在使用无法删除" };
            }
            string del = "update asm_brand set is_del=1 where id="+brandID;
            int a= DbHelperSQL.ExecuteSql(del);
            if (a > 0)
            {
                return new { code = 0, msg = "删除成功" };
            }
            return new { code = 500, msg = "删除失败" };
        }
    }
}