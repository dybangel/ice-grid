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
    public partial class rechargetj : System.Web.UI.Page
    {
 
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string sql = "select * from asm_company ";
                DataTable dd = DbHelperSQL.Query(sql).Tables[0];
                this.companyList.DataTextField = "name";
                this.companyList.DataValueField = "id";
                this.companyList.DataSource = dd;
                this.companyList.DataBind();
                this.companyList.Items.Insert(0, new ListItem("全部", "0")); //添加项
            }
        }

        [WebMethod]
        public static string getSear(string start, string end, string pageCurrentCount, string companyID)
        {
            string sql = "";
            if (!string.IsNullOrEmpty(start))
            {
                sql += " and time>='" + start.Replace("-", "") + "'";
            }
            if (!string.IsNullOrEmpty(end))
            {
                sql += " and time<='" + end.Replace("-", "") + "'";
            }
            if (!companyID.Equals("0")) {
                sql += " and companyID='" + companyID + "'";
            }

            string sql1 = " select * from asm_inComeTJ where 1=1 "+ sql;
            int startIndex = (int.Parse(pageCurrentCount) - 1) * Config.pageSize + 1;
            int endIndex = int.Parse(pageCurrentCount) * Config.pageSize;
            DataTable dt = Config.getPageDataTable("order by time desc", sql1, startIndex, endIndex);
            DataTable da = DbHelperSQL.Query(sql1).Tables[0];
            if (dt.Rows.Count > 0)
            {
                double d = double.Parse((da.Rows.Count / double.Parse(Config.pageSize.ToString())).ToString());
                string ss = Math.Ceiling(d) + "@" + OperUtil.DataTableToJsonWithJsonNet(dt);
                double ls = 0, lsAvai = 0, dg = 0, cz = 0, tk = 0, zsr = 0;
                for (int i = 0; i < da.Rows.Count; i++)
                {
                    ls += double.Parse(da.Rows[i]["lsMoney"].ToString());
                    lsAvai += double.Parse(da.Rows[i]["lsMoneyAvai"].ToString());
                    dg += double.Parse(da.Rows[i]["dgMoney"].ToString());
                    cz += double.Parse(da.Rows[i]["czMoney"].ToString());
                    tk += double.Parse(da.Rows[i]["tkMoney"].ToString());
                    zsr += double.Parse(da.Rows[i]["totalMoney"].ToString());
                }
                return ss + "@" + ls + "@" + lsAvai + "@" + dg + "@" + cz + "@" + tk + "@" + zsr;
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
                sql += " and time>='" + start.Value.Replace("-", "") + "'";
            }
            if (!string.IsNullOrEmpty(end.Value))
            {
                sql += " and time<='" + end.Value.Replace("-", "") + "'";
            }
            string companyID = this.companyList.SelectedValue;
            if (!companyID.Equals("0"))
            {
                sql += " and companyID='" + companyID + "'";
            }
            string sql1 = " select time,lsMoney,dgMoney,czMoney,tkMoney,totalMoney from asm_inComeTJ where 1=1 " + sql;

            DataTable da = DbHelperSQL.Query(sql1).Tables[0];
            ExportToSpreadsheet(da, DateTime.Now.ToString("yyyyMMdd"));
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
                if (column.ColumnName == "time")
                {
                    context.Response.Write("时间" + ",");
                }
                if (column.ColumnName == "lsMoney")
                {
                    context.Response.Write("零售金额" + ",");
                }
                if (column.ColumnName == "dgMoney")
                {
                    context.Response.Write("订购金额" + ",");
                }
                if (column.ColumnName == "czMoney")
                {
                    context.Response.Write("充值金额" + ",");
                }
                if (column.ColumnName == "tkMoney")
                {
                    context.Response.Write("退款金额" + ",");
                }
                if (column.ColumnName == "totalMoney")
                {
                    context.Response.Write("总收入" + ",");
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

        //[WebMethod]
        //public static string getSear(string start, string end, string pageCurrentCount,string companyID)
        //{
        //    string sql = " and 1=1 ";
        //    string sql12 = " and 1=1 ";
        //    if (!string.IsNullOrEmpty(start))
        //    {
        //        sql += " and paytime>='" + start.Replace("-","").Replace(":","").Replace(" ","") + "'";
        //        sql12 += " and time>='"+start+"'";
        //    }
        //    if (!string.IsNullOrEmpty(end))
        //    {
        //        sql += " and paytime<='" + end.Replace("-", "").Replace(":", "").Replace(" ", "") + "'";
        //        sql12 += " and time<='"+end+"'";
        //    }
        //    if (companyID != "0"&&companyID!="")
        //    {
        //        string sqlc = "select tl_APPID from asm_company where id=" + companyID;
        //        DataTable dc = DbHelperSQL.Query(sqlc).Tables[0];
        //        sql += " and appid='" + dc.Rows[0]["tl_APPID"].ToString() + "'";
        //    }
        //    string sql1 = "select A.time1,cast(round(isnull(A.lsMoney,0)/100.0,2)  as  decimal(18,2)) lsMoney,"
        //                  +"  cast(round(isnull(A.dgMoney, 0) / 100.0, 2) as decimal(18, 2)) dgMoney,"
        //                  +"  cast(round(isnull(A.czMoney, 0) / 100.0, 2) as decimal(18, 2)) czMoney,"
        //                  + " cast(round(isnull(A.tkMoney, 0) / 100.0, 2) as decimal(18, 2)) tkMoney,"
        //                  + " cast(round(isnull(A.lsMoney, 0) / 100.0, 2) + round(isnull(A.dgMoney, 0) / 100.0, 2) + round(isnull(A.czMoney, 0) / 100.0, 2) as decimal(18, 2)) totalMoney"
        //                  +"  from(select time1,"
        //                  + "  (select SUM(ISNULL(trxamt, 0)) from asm_pay_info where payType in(1, 2,4) and statu=1 and type=2 and chzt is not null and paytime is not null and SUBSTRING(paytime,0, 9)= t.time1 " + sql+" group by SUBSTRING(paytime, 0, 9)) lsMoney,"
        //                  + "  (select SUM(ISNULL(trxamt, 0)) from asm_pay_info where payType in(3) and statu=1 and type = 2 and paytime is not null and SUBSTRING(paytime,0, 9)= t.time1 " + sql+" group by SUBSTRING(paytime, 0, 9)) dgMoney,"
        //                  + "  (select SUM(ISNULL(trxamt, 0)) from asm_pay_info where type in(1) and statu=1 and payType in(3) and paytime is not null and SUBSTRING(paytime,0, 9)= t.time1 " + sql+" group by SUBSTRING(paytime, 0, 9)) czMoney,"
        //                  + " (select SUM(ISNULL(trxamt, 0)) from asm_pay_info where type in(2) and statu=2 and paytime is not null and SUBSTRING(paytime,0, 9)= t.time1 " + sql + " group by SUBSTRING(paytime, 0, 9)) tkMoney"
        //                  + " from asm_time t where 1=1 "+sql12+") A ";
        //    int startIndex = (int.Parse(pageCurrentCount) - 1) * Config.pageSize + 1;
        //    int endIndex = int.Parse(pageCurrentCount) * Config.pageSize;
        //    DataTable dt = Config.getPageDataTable("order by time1 desc", sql1, startIndex, endIndex);
        //    DataTable da = DbHelperSQL.Query(sql1).Tables[0];
        //    if (dt.Rows.Count > 0)
        //    {
        //        double d = double.Parse((da.Rows.Count / double.Parse(Config.pageSize.ToString())).ToString());
        //        string ss = Math.Ceiling(d) + "@" + OperUtil.DataTableToJsonWithJsonNet(dt);
        //        double ls = 0, dg = 0, cz = 0, tk = 0, zsr = 0;
        //        for (int i = 0; i < da.Rows.Count; i++)
        //        {
        //            ls += double.Parse(da.Rows[i]["lsMoney"].ToString());
        //            dg += double.Parse(da.Rows[i]["dgMoney"].ToString());
        //            cz += double.Parse(da.Rows[i]["czMoney"].ToString());
        //            tk += double.Parse(da.Rows[i]["tkMoney"].ToString());
        //            zsr += double.Parse(da.Rows[i]["totalMoney"].ToString());
        //        }
        //        return ss + "@" + ls + "@" + dg + "@" + cz + "@" + tk + "@" + zsr;
        //    }
        //    else
        //    {
        //        return "1";
        //    }

        //}
    }
}