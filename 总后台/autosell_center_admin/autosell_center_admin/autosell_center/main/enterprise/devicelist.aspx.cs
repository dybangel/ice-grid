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

namespace autosell_center.main.enterprise
{
    public partial class devicelist : System.Web.UI.Page
    {
        public DataTable runData;//正在运行机器
        public DataTable gqData;//即将过期机器
        public String company_ID = "";//企业id
        protected void Page_Load(object sender, EventArgs e)
        {
            company_ID = Request.QueryString["companyID"].ToString();
            this.companyID.Value = company_ID;
            initData();
        }
        public void initData()
        {
            string sql1 = "select am.*,ac.name,amt.name mechineType, case am.statu when '0' then '正常' when '1' then '脱机' when '2' then '温度异常'   else '其他' end sta,case am.zt when '1' then '禁用' when '2' then '正常' when '3' then '过期'   else '其他' end t from asm_mechine am left join asm_company ac on am.companyID=ac.id   left join asm_mechineType amt on am.version=amt.id where companyID=" + this.companyID.Value.Trim();
            runData = DbHelperSQL.Query(sql1).Tables[0];

            string sql2 = "select *,amt.name mechineType,case am.statu when '0' then '正常' when '1' then '脱机' when '2' then '温度异常'   else '其他' end sta,case am.zt when '1' then '禁用' when '2' then '正常' when '3' then '过期'   else '其他' end t from asm_mechine am left join asm_mechineType amt on am.version=amt.id where DATEDIFF(dd,GETDATE(),CONVERT(datetime,validateTime))<=7   order by validateTime  ";
            gqData = DbHelperSQL.Query(sql2).Tables[0];

        }
        
        [WebMethod]
        public static string search(string startTime,string endTime,string bh,string type,string companyID)
        {
            string sql = " where companyID= "+companyID;
            if (startTime != "")
            {
                sql += " and am.validateTime>'" + startTime + "'";
            }
            if (endTime != "")
            {
                sql += " and am.validateTime<'" + endTime + "'";
            }
            if (bh != "")
            {
                sql += " and am.bh='" + bh + "'";
            }
            if (type != "0")
            {
                sql += " and am.zt=" + type;
            }
            string sql1 = "select am.*,ac.name,amt.name mechineType, case am.statu when '0' then '正常' when '1' then '脱机' when '2' then '温度异常'   else '其他' end sta,case am.zt when '1' then '禁用' when '2' then '正常' when '3' then '过期'   else '其他' end t from asm_mechine am left join asm_company ac on am.companyID=ac.id  left join asm_mechineType amt on am.version=amt.id  " + sql;
            DataTable dt = DbHelperSQL.Query(sql1).Tables[0];
            if (dt.Rows.Count > 0)
            {
                string ss = OperUtil.DataTableToJsonWithJsonNet(dt);
                return ss;
            }
            else {
                return "1";
            }
        }
        [WebMethod]
        public static string getInfo(string id)
        {
            string sql = "select * from asm_mechine where id="+id;
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["validateTime"].ToString() + "|" + dt.Rows[0]["zt"].ToString();
            }
            else {
                return "";
            }
        }
        [WebMethod]
        public static string getPath(string id)
        {
            string sql = "select * from asm_company where id=" + id;
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["yyzzPath"].ToString();
            }
            else
            {
                return "";
            }
        }
    }
}