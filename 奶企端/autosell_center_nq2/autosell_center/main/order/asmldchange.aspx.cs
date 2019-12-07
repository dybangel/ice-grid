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

namespace autosell_center.main.order
{
    public partial class asmldchange : System.Web.UI.Page
    {
        public string comID = "";
        public string operaID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            comID = OperUtil.Get("companyID");
            operaID = OperUtil.Get("operaID");
            this.agentID.Value = operaID;
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
        public static object sear(string start, string end, string mechineID, string pageCurrentCount, string productInfo, string companyID, string statusList, string typeList)
        {
            string sql1 = " and ld.companyID=" + companyID;
            if (!string.IsNullOrEmpty(start))
            {
                sql1 += " and chgTime>'" + start + "'";
            }
            if (!string.IsNullOrEmpty(end))
            {
                sql1 += " and chgTime<'" + end + "'";
            }
            if (!string.IsNullOrEmpty(mechineID))
            {
                sql1 += " and ld.mechineID in (" + mechineID + ")";
            }
            if (!string.IsNullOrEmpty(productInfo))
            {
                sql1 += " and (p.proName like '%" + productInfo + "%' or p.bh like '%" + productInfo + "%')";
            }
            if (statusList != "0")
            {
                sql1 += " and ld.status=" + statusList;
            }
            if (typeList!="0")
            {
                sql1 += " and ld.type="+typeList;
            }
            string sql = "SELECT m.bh,m.mechineName,p.proName,ld.* ,CASE WHEN ld.status=1 THEN '增加' WHEN ld.status=-1 THEN '减少' else '' END as statusName, "
                        + " CASE WHEN ld.type=1 THEN '补货任务'  WHEN ld.type=2 THEN '零售出货'  WHEN ld.type=3 THEN '订购出货'  WHEN ld.type=4 THEN '料道纠错' else  ''  END as typeName"
                        + " ,(SELECT name FROM asm_opera where id=ld.operaID) operaName FROM asm_ld_change ld"
                        + " left join asm_mechine m on ld.mechineID = m.id left join asm_product p on ld.productID = p.productID where 1=1 "+sql1;
            int startIndex = (int.Parse(pageCurrentCount) - 1) * Config.pageSize + 1;
            int endIndex = int.Parse(pageCurrentCount) * Config.pageSize;

            DataTable dt = Config.getPageDataTable("order by T.id desc", sql, startIndex, endIndex);
            DataTable da = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                double d = double.Parse((da.Rows.Count / double.Parse(Config.pageSize.ToString())).ToString());
                return new { code = 200,db= OperUtil.DataTableToJsonWithJsonNet(dt),count= Math.Ceiling(d) };
            }
            else
            {
                return new { code = 500 };
            }
          
        }
        [WebMethod]
        public static object getMechineList(string companyID)
        {
            try
            {

                string sql = "select * from asm_mechine where  mechineName is not null and companyID=" + companyID;
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return new { code = 200, db = OperUtil.DataTableToJsonWithJsonNet(dt) };
                }
                return new { code = 200, db = OperUtil.DataTableToJsonWithJsonNet(dt) };
            }
            catch
            {
                return new { code = 500, msg = "系统异常" };
            }
        }

        protected void excel_Click(object sender, EventArgs e)
        {
            string sql1 = " and ld.companyID=" + companyID.Value;
            if (!string.IsNullOrEmpty(start.Value))
            {
                sql1 += " and chgTime>'" + start.Value + "'";
            }
            if (!string.IsNullOrEmpty(end.Value))
            {
                sql1 += " and chgTime<'" + end + "'";
            }
            if (!string.IsNullOrEmpty(_mechineList.Value))
            {
                sql1 += " and ld.mechineID in (" + _mechineList.Value + ")";
            }
            if (statusList.SelectedValue != "0")
            {
                sql1 += " and ld.status=" + statusList.SelectedValue;
            }
            if (typeList.SelectedValue != "0")
            {
                sql1 += " and ld.type=" + typeList.SelectedValue;
            }
            if (!string.IsNullOrEmpty(productInfo.Value))
            {
                sql1 += " and (p.proName like '%" + productInfo.Value + "%' or p.bh like '%" + productInfo.Value + "%')";
            }
            string sql = "SELECT m.bh,m.mechineName,p.proName,ld.ldNO,ld.changeNum,ld.afterNum,ld.beforeNum,ld.chgTime, CASE WHEN ld.status=1 THEN '增加' WHEN ld.status=-1 THEN '减少' else '' END as statusName,"
                        + " CASE WHEN ld.type=1 THEN '补货任务'  WHEN ld.type=2 THEN '零售出货'  WHEN ld.type=3 THEN '订购出货'  WHEN ld.type=4 THEN '料道纠错' else  ''  END as typeName"
                        + " ,(SELECT name FROM asm_opera where id=ld.operaID) operaName FROM asm_ld_change ld"
                        + " left join asm_mechine m on ld.mechineID = m.id left join asm_product p on ld.productID = p.productID where 1=1 " + sql1;
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            ExportToSpreadsheet(dt,DateTime.Now.ToString("yyyyMMdd")+"库存变动记录");
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
                if (column.ColumnName == "bh")
                {
                    context.Response.Write("机器编号" + ",");
                }
                if (column.ColumnName == "mechineName")
                {
                    context.Response.Write("机器名称" + ",");
                }
                if (column.ColumnName == "proName")
                {
                    context.Response.Write("产品名称" + ",");
                }
                if (column.ColumnName == "ldNO")
                {
                    context.Response.Write("料道编号" + ",");
                }
                if (column.ColumnName == "changeNum")
                {
                    context.Response.Write("变化量" + ",");
                }
                if (column.ColumnName == "beforeNum")
                {
                    context.Response.Write("变化前" + ",");
                }
                if (column.ColumnName == "afterNum")
                {
                    context.Response.Write("变化后" + ",");
                }
                if (column.ColumnName == "statusName")
                {
                    context.Response.Write("类型" + ",");
                }
                if (column.ColumnName == "typeName")
                {
                    context.Response.Write("原因" + ",");
                }
                if (column.ColumnName == "chgTime")
                {
                    context.Response.Write("变动时间" + ",");
                }
                if (column.ColumnName == "operaName")
                {
                    context.Response.Write("操作员" + ",");
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