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
    public partial class productclass : System.Web.UI.Page
    {
        public string comID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            comID = OperUtil.Get("companyID");
            this.companyID.Value = comID;
            if (string.IsNullOrEmpty(comID))
            {
                Response.Write("<script>alert('您尚未登录或已长时间未进行操作，请重新登录!');top.location.href='../../index.aspx';</script>");
                return;
            }
        }
        [WebMethod]
        public static string update(string id, string name, string companyID)
        {
           
            string sql = "select * from asm_protype where typeName='" + name + "' and companyID=" + companyID;
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return "2";
            }
            else
            {
                string update = "update asm_protype set typeName='" + name + "' where productTypeID=" + id;
                DbHelperSQL.ExecuteSql(update);
                RedisHelper.Remove("_productTypeInfoSet");
            }
            return "1";
        }
        [WebMethod]
        public static string add(string name, string companyID)
        {

            string sql = "select * from asm_protype where typeName='" + name + "' and companyID=" + companyID;
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return "2";
            }
            else
            {
                string sql1 = "insert into asm_protype (typeName,companyID) values('" + name + "'," + companyID + ")";
                DbHelperSQL.ExecuteSql(sql1);
                RedisHelper.Remove("_productTypeInfoSet");
            }
            return "1";
        }
        [WebMethod]
        public static string sear(string companyID)
        {
            string sql = "select ROW_NUMBER() OVER(ORDER BY productTypeID DESC) AS num,* from asm_protype where  companyID=" + companyID;
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
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
        public static string deleTed(string id, string companyID)
        {
            string sql = "select * from asm_product where protype=1";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return "2";
            }
            else
            {
                string sql1 = "delete from asm_protype where productTypeID=" + id;
                DbHelperSQL.ExecuteSql(sql1);
                RedisHelper.Remove("_productTypeInfoSet");
                return "1";
            }
        }
    }
}