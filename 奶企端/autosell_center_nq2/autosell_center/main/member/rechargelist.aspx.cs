using autosell_center.cls;
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

namespace autosell_center.main.member
{
    public partial class rechargelist : System.Web.UI.Page
    {
        public string comID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            comID = OperUtil.Get("companyID");
            this._operaID.Value = OperUtil.Get("operaID");
            if (string.IsNullOrEmpty(comID))
            {
                Response.Write("<script>alert('您尚未登录或已长时间未进行操作，请重新登录!');top.location.href='../../index.aspx';</script>");
                return;
            }
            this.companyID.Value = comID;
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
        public static string getSear(string start, string end, string keyword, string pageCurrentCount,string companyID,string payType,string trxid)
        {
            string sql = " 1=1 ";
            if (!string.IsNullOrEmpty(start))
            {
                sql += " and paytime>'"+start.Replace("-","").Replace(":","").Replace(" ","")+"'";
            }
            if (!string.IsNullOrEmpty(end))
            {
                sql += " and paytime<'" + end.Replace("-", "").Replace(":", "").Replace(" ", "") + "'";
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                sql += " and (name like '%"+keyword+ "%' or CONVERT(varchar,m.id)='" + keyword+"' or phone='"+keyword+"')";
            }
            if (payType != "0")
            {
                sql += " and payType = " + payType;
            }
            else {
                sql += " and payType in (3,5)";
            }
            if (!string.IsNullOrEmpty(trxid))
            {
                sql += " and p.trxid like '%"+trxid+"%'";
            }
            string sqlc = "select tl_APPID from asm_company where id="+companyID;
            DataTable dc = DbHelperSQL.Query(sqlc).Tables[0];
            sql += " and appid='"+ dc.Rows[0]["tl_APPID"].ToString() + "'";
            string sql1 = "select m.name,m.phone,m.id as memberID,Round(convert(float,trxamt)/100,2) as money,p.* "
                          + "  from asm_pay_info p left"
                          +"  join asm_member m"
                          + "  on  p.unionID=m.unionID where statu = 1 and type = 1   and m.nickname is not null and " + sql ;
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

            string sql = " 1=1 ";
            if (!string.IsNullOrEmpty(start.Value))
            {
                sql += " and paytime>'" + start.Value.Replace("-", "").Replace(":", "").Replace(" ", "") + "'";
            }
            if (!string.IsNullOrEmpty(end.Value))
            {
                sql += " and paytime<'" + end.Value.Replace("-", "").Replace(":", "").Replace(" ", "") + "'";
            }
            if (!string.IsNullOrEmpty(keyword.Value))
            {
                sql += " and name like '%" + keyword.Value + "%'";
            }
            if (payType.SelectedValue != "0")
            {
                sql += " and payType=" + payType.SelectedValue;
            }
            else {
                sql += " and payType in (3,5)";
            }
            string sqlc = "select tl_APPID from asm_company where id=" + this.companyID.Value;
            DataTable dc = DbHelperSQL.Query(sqlc).Tables[0];
            sql += " and appid='" + dc.Rows[0]["tl_APPID"].ToString() + "'";
            string sql1 = "select m.id as memberID,m.name,m.phone,Round(convert(float,trxamt)/100,2) as money,p.trxid,p.afterMoney,createTime as paytime, case when p.payType=3 then '微信充值' when p.payType=5 then '充值卡' else '' end payTypeName,p.cardName,p.cardBH "
                          + "  from asm_pay_info p left"
                          + "  join asm_member m"
                          + "  on  p.unionID=m.unionID where statu = 1 and type = 1   and m.nickname is not null and " + sql;
            DataTable dt = DbHelperSQL.Query(sql1).Tables[0];
            ExportToSpreadsheet(dt, DateTime.Now.ToString("yyyyMMdd"));

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
                    context.Response.Write("充值金额" + ",");
                }
                if (column.ColumnName == "afterMoney")
                {
                    context.Response.Write("充值后金额" + ",");
                }
                if (column.ColumnName == "trxid")
                {
                    context.Response.Write("流水号" + ",");
                }
                if (column.ColumnName == "paytime")
                {
                    context.Response.Write("支付时间" + ",");
                }
                if (column.ColumnName == "payTypeName")
                {
                    context.Response.Write("充值方式" + ",");
                }
                if (column.ColumnName == "cardName")
                {
                    context.Response.Write("充值卡名称" + ",");
                }
                if (column.ColumnName == "cardBH")
                {
                    context.Response.Write("充值卡卡号" + ",");
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