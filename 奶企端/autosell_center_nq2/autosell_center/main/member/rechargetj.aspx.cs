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
    public partial class rechargetj : System.Web.UI.Page
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
        public static string getSear(string start, string end, string pageCurrentCount,string companyID)
        {
            string sql = "";
            if (!string.IsNullOrEmpty(start))
            {
                sql += " and time>='"+start.Replace("-","")+"'";
            }
            if (!string.IsNullOrEmpty(end))
            {
                sql += " and time<='"+end.Replace("-","")+"'";
            }
            string sql1 = " select * from asm_inComeTJ where companyID="+companyID+sql;
            int startIndex = (int.Parse(pageCurrentCount) - 1) * Config.pageSize + 1;
            int endIndex = int.Parse(pageCurrentCount) * Config.pageSize;
            DataTable dt = Config.getPageDataTable("order by time desc", sql1, startIndex, endIndex);
            DataTable da = DbHelperSQL.Query(sql1).Tables[0];
            if (dt.Rows.Count > 0)
            {
                double d = double.Parse((da.Rows.Count / double.Parse(Config.pageSize.ToString())).ToString());
                string ss = Math.Ceiling(d) + "@" + OperUtil.DataTableToJsonWithJsonNet(dt);
                double ls = 0,lsAvai=0,dg=0,cz=0,tk=0,zsr=0;
                for (int i=0;i< da.Rows.Count;i++)
                {
                    ls += double.Parse(da.Rows[i]["lsMoney"].ToString());
                    lsAvai += double.Parse(da.Rows[i]["lsMoneyAvai"].ToString());
                    dg += double.Parse(da.Rows[i]["dgMoney"].ToString());
                    cz += double.Parse(da.Rows[i]["czMoney"].ToString());
                    tk += double.Parse(da.Rows[i]["tkMoney"].ToString());
                    zsr += double.Parse(da.Rows[i]["totalMoney"].ToString());
                }
                return ss+"@"+ls+"@"+lsAvai+"@"+dg+"@"+cz+"@"+tk+"@"+zsr;
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
            if (!string.IsNullOrEmpty(endTime.Value))
            {
                sql += " and time<='" + endTime.Value.Replace("-", "") + "'";
            }
            string sql1 = " select time,lsMoney,dgMoney,czMoney,tkMoney,totalMoney from asm_inComeTJ where companyID=" + companyID.Value+ sql;
           
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
    }
}