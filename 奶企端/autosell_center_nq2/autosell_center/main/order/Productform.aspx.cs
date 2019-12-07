using autosell_center.cls;
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

namespace autosell_center.main.order
{
    public partial class Productform : System.Web.UI.Page
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
        public static string sear(string start,string end,string mechineID, string pageCurrentCount,string brandID,string companyID)
        {
            string sql1 = "";
            if (!string.IsNullOrEmpty(start))
            {
                sql1 += " and  CONVERT(datetime, orderTime) > '" + start + "'";
            }
            if (!string.IsNullOrEmpty(end))
            {
                sql1 += " and CONVERT(datetime, orderTime) < '" + end + "'";
            }
            if (!string.IsNullOrEmpty(mechineID))
            {
                sql1 += " and mechineID in(" + mechineID + ")";
            }
            else {
                string mechineSql = "SELECT  companyID ,value = ( STUFF(( SELECT    ',' + convert(varchar,id) FROM asm_mechine"
                       +" WHERE companyID = Test.companyID  FOR XML PATH('') ), 1, 1, '') )FROM asm_mechine  AS Test where companyID = "+companyID+"   GROUP BY companyID";
                DataTable mechineDt = DbHelperSQL.Query(mechineSql).Tables[0];
                if (mechineDt.Rows.Count>0)
                {
                    sql1 += " and mechineID in(" + mechineDt.Rows[0]["value"].ToString() + ")";
                }
            }
            if (!string.IsNullOrEmpty(brandID))
            {
                string brandSql = "SELECT STUFF((SELECT ','+CONVERT(varchar,id) FROM  asm_brand where id in("+brandID+") for xml path('')),1,1,'') id ";
                DataTable brandDt = DbHelperSQL.Query(brandSql).Tables[0];
                if (brandDt.Rows.Count>0&&!string.IsNullOrEmpty(brandDt.Rows[0]["id"].ToString()))
                {
                    string sqlp = "SELECT STUFF((SELECT ','+CONVERT(varchar,productID) FROM  asm_product where brandID in("+ brandDt.Rows[0]["id"].ToString() + ") for xml path('')),1,1,'') id ";
                    DataTable dp = DbHelperSQL.Query(sqlp).Tables[0];
                    if (dp.Rows.Count>0&&!string.IsNullOrEmpty(dp.Rows[0]["id"].ToString()))
                    {
                        sql1 += " and productID in(" + dp.Rows[0]["id"].ToString() + ")";
                    }
                }
                //string brandSql = "SELECT  companyID ,value = ( STUFF(( SELECT    ',' + convert(varchar,productID) FROM asm_product"
                //        + " WHERE companyID = Test.companyID  FOR XML PATH('') ), 1, 1, '') )FROM asm_product  AS Test where companyID = " + companyID + " and brandID in(" + brandID + ") GROUP BY companyID ";
                //DataTable brandDt = DbHelperSQL.Query(brandSql).Tables[0];
                //if (brandDt.Rows.Count > 0)
                //{
                //    sql1 += " and productID in(" + brandDt.Rows[0]["value"].ToString() + ")";
                //}  
            }
            string sql = "select productID,"
                       + " (select proname from  asm_product where productID=A.productID)proname,"
                       + " (select brandName from  asm_brand where id in(select brandID from  asm_product where productID=A.productID)) brandName,"
                       + " ISNULL(sum(case when type = '1' then totalNum end), 0) as dgNum,"
                       +" ISNULL(sum(case when type = '2' then totalNum end), 0) as lsNum,"
                       +" (ISNULL(sum(case when type = '1' then totalNum end), 0) + ISNULL(sum(case when type = '2' then totalNum end), 0)) as totalNum,"
                       + " ROUND(ISNULL(sum(case when type = '2' then totalMoney end), 0) / ISNULL(sum(case when type = '2' then totalNum end), 1),2) as avgprice,"
                       + " ISNULL(sum(case when type = '2' then totalMoney end), 0) as totalMoney,"
                       + " ("
                       +" select COUNT(*) num from(select productID, memberID, count(memberID) num from asm_sellDetail s"
                       +" where 1=1 "+sql1+" and s.productID = A.productID  group by productID, memberID) A  group by productID"
                       +" ) lsMemberNum"
                       +" from("
                       +" select productID, COUNT(*) totalNum, type, SUM(totalMoney) totalMoney from asm_sellDetail s"
                       +" where 1=1 "+sql1
                       +" group by productID, type, memberID"
                       +" ) A"
                       +" group by A.productID";
           
            int startIndex = (int.Parse(pageCurrentCount) - 1) * Config.pageSize + 1;
            int endIndex = int.Parse(pageCurrentCount) * Config.pageSize;

            DataTable dt = Config.getPageDataTable("order by T.productID desc", sql, startIndex, endIndex);
            DataTable da = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {

                double d = double.Parse((da.Rows.Count / double.Parse(Config.pageSize.ToString())).ToString());
                string ss = Math.Ceiling(d) + "@" + OperUtil.DataTableToJsonWithJsonNet(dt);
                
                return ss;
            }
            else
            {
                return "1";
            }
        }

        protected void excel_Click(object sender, EventArgs e)
        {
            string sql1 = "";
            if (!string.IsNullOrEmpty(start.Value))
            {
                sql1 += " and  CONVERT(datetime, orderTime) > '" + start.Value + "'";
            }
            if (!string.IsNullOrEmpty(end.Value))
            {
                sql1 += " and CONVERT(datetime, orderTime) < '" + end.Value + "'";
            }
            if (!string.IsNullOrEmpty(this._mechineList.Value))
            {
                sql1 += " and mechineID in(" + this._mechineList.Value + ")";
            }
            else
            {
                string mechineSql = "SELECT  companyID ,value = ( STUFF(( SELECT    ',' + convert(varchar,id) FROM asm_mechine"
                       + " WHERE companyID = Test.companyID  FOR XML PATH('') ), 1, 1, '') )FROM asm_mechine  AS Test where companyID = " + this.companyID.Value + "   GROUP BY companyID";
                DataTable mechineDt = DbHelperSQL.Query(mechineSql).Tables[0];
                if (mechineDt.Rows.Count > 0)
                {
                    sql1 += " and mechineID in(" + mechineDt.Rows[0]["value"].ToString() + ")";
                }
            }
            if (!string.IsNullOrEmpty(this._brandList.Value))
            {
                string brandSql = "SELECT  brandID ,value = ( STUFF(( SELECT    ',' + convert(varchar,productID) FROM asm_product"
                        + " WHERE brandID = Test.brandID  FOR XML PATH('') ), 1, 1, '') )FROM asm_product  AS Test where companyID = " + this.companyID.Value + " and brandID in(" + this._brandList.Value + ") GROUP BY brandID; ";
                DataTable brandDt = DbHelperSQL.Query(brandSql).Tables[0];
                if (brandDt.Rows.Count > 0)
                {
                    sql1 += " and productID in(" + brandDt.Rows[0]["value"].ToString() + ")";
                }
            }
            string sql = "select "
                       + " (select bh from  asm_product where productID=A.productID) bh,"
                       + " (select proname from  asm_product where productID=A.productID)proname,"
                       + " (select brandName from  asm_brand where id in(select brandID from  asm_product where productID=A.productID)) brandName,"
                       + " ISNULL(sum(case when type = '1' then totalNum end), 0) as dgNum,"
                       + " ISNULL(sum(case when type = '2' then totalNum end), 0) as lsNum,"
                       + " (ISNULL(sum(case when type = '1' then totalNum end), 0) + ISNULL(sum(case when type = '2' then totalNum end), 0)) as totalNum,"
                       + " ROUND(ISNULL(sum(case when type = '2' then totalMoney end), 0) / ISNULL(sum(case when type = '2' then totalNum end), 1),2) as avgprice,"
                       + " ISNULL(sum(case when type = '2' then totalMoney end), 0) as totalMoney,"
                       + " ("
                       + " select COUNT(*) num from(select productID, memberID, count(memberID) num from asm_sellDetail s"
                       + " where 1=1 " + sql1 + " and s.productID = A.productID  group by productID, memberID) A  group by productID"
                       + " ) lsMemberNum"
                       + " from("
                       + " select productID, COUNT(*) totalNum, type, SUM(totalMoney) totalMoney from asm_sellDetail s"
                       + " where 1=1 " + sql1
                       + " group by productID, type, memberID"
                       + " ) A"
                       + " group by A.productID";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
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
                if (column.ColumnName == "bh")
                {
                    context.Response.Write("商品条码" + ",");
                }
                if (column.ColumnName == "proname")
                {
                    context.Response.Write("商品名称" + ",");
                }
                if (column.ColumnName == "brandName")
                {
                    context.Response.Write("品牌" + ",");
                }
                if (column.ColumnName == "avgprice")
                {
                    context.Response.Write("平均零售单价" + ",");
                }
                if (column.ColumnName == "lsNum")
                {
                    context.Response.Write("零售销售数量" + ",");
                }
                if (column.ColumnName == "totalNum")
                {
                    context.Response.Write("总数量" + ",");
                }
                if (column.ColumnName == "totalMoney")
                {
                    context.Response.Write("零售总金额" + ",");
                }
                if (column.ColumnName == "lsMemberNum")
                {
                    context.Response.Write("零售用户数" + ",");
                }
                if (column.ColumnName == "dgNum")
                {
                    context.Response.Write("订购取货数量" + ",");
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
        [WebMethod]
        public static object getBrandList(string companyID)
        {
            try
            {
                string sql = "select * from asm_brand where companyID=" + companyID;
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return new { code = 200, db = OperUtil.DataTableToJsonWithJsonNet(dt) };
                }
                return new { code = 300, msg = "暂无记录" };
            }
            catch
            {
                return new { code = 500, msg = "系统异常" };
            }
        }
    }
}