using autosell_center.cls;
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

namespace autosell_center.main.member
{
    public partial class chgMoney : System.Web.UI.Page
    {
        public string comID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            comID = OperUtil.Get("companyID");
            this._operaID.Value = OperUtil.Get("operaID");
            this._memberID.Value = Request.QueryString["memberID"].ToString();
            if (string.IsNullOrEmpty(comID))
            {
                Response.Write("<script>alert('您尚未登录或已长时间未进行操作，请重新登录!');top.location.href='../../index.aspx';</script>");
                return;
            }
            this.companyID.Value = comID;
        }

        [WebMethod]
        public static string getSear(string start, string end, string pageCurrentCount, string memberID, string type)
        {
            string sql = "";
            if (!string.IsNullOrEmpty(start))
            {
                sql += " and payTime>'" + start + "'";
            }
            if (!string.IsNullOrEmpty(end))
            {
                sql += " and payTime<'" + end + "'";
            }
            if (!string.IsNullOrEmpty(memberID))
            {
                sql += " and memberID=" + memberID;
            }
            if (type!="0")
            {
                sql += " and a.type="+type;
            }
            string sql1 = "SELECT a.*,b.name,b.phone,CASE WHEN type in(1,3,5,7) THEN money  WHEN type in(2,4) THEN '-'+CONVERT(VARCHAR,money) ELSE money END as money1 FROM asm_chgMoney a LEFT JOIN asm_member b ON a.memberID=b.id where 1=1 " + sql;
            int startIndex = (int.Parse(pageCurrentCount) - 1) * Config.pageSize + 1;
            int endIndex = int.Parse(pageCurrentCount) * Config.pageSize;
            DataTable dt = Config.getPageDataTable("order by T.id desc", sql1, startIndex, endIndex);
            DataTable da = DbHelperSQL.Query(sql1).Tables[0];
            if (dt.Rows.Count > 0)
            {
                double d = double.Parse((da.Rows.Count / double.Parse(Config.pageSize.ToString())).ToString());
                string ss = Math.Ceiling(d) + "@@@" + OperUtil.DataTableToJsonWithJsonNet(dt);
                return ss;
            }
            else
            {
                return "1";
            }

        }
        protected void excel_Click(object sender, EventArgs e)
        {
            string sql = "";
            if (!string.IsNullOrEmpty(start.Value))
            {
                sql += " and payTime>'" + start + "'";
            }
            if (!string.IsNullOrEmpty(end.Value))
            {
                sql += " and payTime<'" + end + "'";
            }
            if (!string.IsNullOrEmpty(this._memberID.Value))
            {
                sql += " and memberID=" + _memberID.Value;
            }
            if (typeList.SelectedValue != "0")
            {
                sql += " and a.type=" + typeList.SelectedValue;
            }
            string sql1 = "SELECT a.memberID,b.name,b.phone,CASE WHEN type in(1,3,5,7) THEN money  WHEN type in(2,4) THEN '-'+CONVERT(VARCHAR,money) ELSE money END as money,a.payTime,a.bz,description FROM asm_chgMoney a LEFT JOIN asm_member b ON a.memberID=b.id  where 1=1 " + sql;
            DataTable dt = DbHelperSQL.Query(sql1).Tables[0];
            ExportToSpreadsheet(dt,DateTime.Now.ToString("yyyyMMdd"));
        }
        public static void ExportToSpreadsheet(DataTable table, string name)
        {
            Random r = new Random();
            string rf = "";
            for (int j = 0; j < 10; j++)
            {
                rf = r.Next(int.MaxValue).ToString();
            }
            HttpContext context = HttpContext.Current;
            context.Response.Clear();
            context.Response.ContentType = "application/ms-excel";
            context.Response.ContentEncoding = System.Text.Encoding.UTF8;
            context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + name + rf + ".xls");
            context.Response.BinaryWrite(System.Text.Encoding.UTF8.GetPreamble());

            foreach (DataColumn column in table.Columns)
            {
                if (column.ColumnName == "memberID")
                {
                    context.Response.Write("会员ID" + ",");
                }
                if (column.ColumnName == "name")
                {
                    context.Response.Write("会员名称" + ",");
                }
                if (column.ColumnName == "phone")
                {
                    context.Response.Write("手机号" + ",");
                }
                if (column.ColumnName == "money")
                {
                    context.Response.Write("变动金额" + ",");
                }
                if (column.ColumnName == "payTime")
                {
                    context.Response.Write("时间" + ",");
                }
                if (column.ColumnName == "bz")
                {
                    context.Response.Write("类型" + ",");
                }
                if (column.ColumnName == "description")
                {
                    context.Response.Write("描述" + ",");
                }
               
            }
            context.Response.Write(Environment.NewLine);
            double test;
            foreach (DataRow row in table.Rows)
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    switch (table.Columns[i].DataType.ToString())
                    {
                        case "System.String":
                            if (double.TryParse(row[i].ToString(), out test)) context.Response.Write("=");
                            context.Response.Write("\"" + row[i].ToString().Replace("\"", "\"\"") + "\",");
                            break;
                        case "System.DateTime":
                            if (row[i].ToString() != "")
                                context.Response.Write("\"" + ((DateTime)row[i]).ToString("yyyy-MM-dd HH:mm:ss") + "\",");
                            else
                                context.Response.Write("\"" + row[i].ToString().Replace("\"", "\"\"") + "\",");
                            break;
                        default:
                            context.Response.Write("\"" + row[i].ToString().Replace("\"", "\"\"") + "\",");
                            break;
                    }
                }
                context.Response.Write(Environment.NewLine);
            }
            context.Response.End();
        }
    }
}