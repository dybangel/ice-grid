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

namespace autosell_center.main.product
{
    public partial class orderlistOld : System.Web.UI.Page
    {
        public string comID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string sql1 = "select * from asm_company";
                DataTable dt = DbHelperSQL.Query(sql1).Tables[0];
                this.companyList.DataTextField = "name";
                this.companyList.DataValueField = "id";
                this.companyList.DataSource = dt;
                this.companyList.DataBind();
                this.companyList.Items.Insert(0, new ListItem("所有企业", "0")); //添加项
                string sql = "select * from asm_mechine ";
                DataTable dt2 = DbHelperSQL.Query(sql).Tables[0];
                this.mechineList.DataTextField = "bh";
                this.mechineList.DataValueField = "id";
                this.mechineList.DataSource = dt2;
                this.mechineList.DataBind();
                this.mechineList.Items.Insert(0, new ListItem("所有机器", "0")); //添加项
            }
           
        }
        [WebMethod]
        public static string getOrderList(string mechineID,string companyID, string pageCurrentCount)
        {
            string sql = " 1=1 ";
            if (!string.IsNullOrEmpty(mechineID) && mechineID != "0")
            {
                sql += " and C.mechineID=" + mechineID;
            }
            if (!string.IsNullOrEmpty(companyID) && companyID != "0")
            {
                sql += " and C.companyID=" + companyID;
            }
            string sql1 = "select C.*,D.name from (select A.*,B.bh,B.companyID from (select asd.*,ap.proname from asm_sellDetail asd left join asm_product ap on asd.productID=ap.productID) A left join asm_mechine B on A.mechineID=B.id) C left join  asm_company D on C.companyID=D.id where 1=1 and " + sql+ " ";
            int startIndex = (int.Parse(pageCurrentCount) - 1) * Config.pageSize + 1;
            int endIndex = int.Parse(pageCurrentCount) * Config.pageSize;

            DataTable dt = Config.getPageDataTable("order by T.id desc", sql1, startIndex, endIndex);
            DataTable da = DbHelperSQL.Query(sql1).Tables[0];
            if (dt.Rows.Count > 0)
            {

                double d = double.Parse((da.Rows.Count / double.Parse(Config.pageSize.ToString())).ToString());
                string ss = Math.Ceiling(d) + "@" + OperUtil.DataTableToJsonWithJsonNet(dt);

                //string ss =OperUtil.DataTableToJsonWithJsonNet(da);
                return ss;
            }
            else
            {
                return "1";
            }
        }
        protected void companyList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.companyList.SelectedValue != "0")
            {
                string sql2 = "select * from asm_mechine where companyID=" + this.companyList.SelectedValue;
                DataTable dt2 = DbHelperSQL.Query(sql2).Tables[0];
                this.mechineList.DataTextField = "bh";
                this.mechineList.DataValueField = "id";
                this.mechineList.DataSource = dt2;
                this.mechineList.DataBind();
            }
            else {
                string sql = "select * from asm_mechine ";
                DataTable dt2 = DbHelperSQL.Query(sql).Tables[0];
                this.mechineList.DataTextField = "bh";
                this.mechineList.DataValueField = "id";
                this.mechineList.DataSource = dt2;
                this.mechineList.DataBind();
                this.mechineList.Items.Insert(0, new ListItem("所有机器", "0")); //添加项
                this.mechineList.SelectedValue = "0";
            }

        }

        protected void excel_Click(object sender, EventArgs e)
        {
            string sql = " 1=1 ";
            if (!string.IsNullOrEmpty(this.companyList.SelectedValue) && this.mechineList.SelectedValue != "0")
            {
                sql += " and C.mechineID=" + this.mechineList.SelectedValue;
            }
            if (!string.IsNullOrEmpty(this.companyList.SelectedValue) && this.companyList.SelectedValue != "0")
            {
                sql += " and C.companyID=" + this.companyList.SelectedValue;
            }
            string sql1 = "select C.orderNO,C.proname,C.totalMoney,C.type,C.proLD,C.orderTime,C.bh,D.name from (select A.*,B.bh,B.companyID from (select asd.*,ap.proname from asm_sellDetail asd left join asm_product ap on asd.productID=ap.productID) A left join asm_mechine B on A.mechineID=B.id) C left join  asm_company D on C.companyID=D.id where 1=1 and " + sql + " order by orderTime desc";
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
            //context.Response.ContentType = "text/csv";
            context.Response.ContentType = "application/ms-excel";
            context.Response.ContentEncoding = System.Text.Encoding.UTF8;
            context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + name + rf + ".xls");
            context.Response.BinaryWrite(System.Text.Encoding.UTF8.GetPreamble());

            foreach (DataColumn column in table.Columns)
            {
                if (column.ColumnName == "orderNO")
                {
                    context.Response.Write("订单编号" + ",");
                }
                if (column.ColumnName == "proname")
                {
                    context.Response.Write("产品名称" + ",");
                }
                if (column.ColumnName == "totalMoney")
                {
                    context.Response.Write("单价" + ",");
                }
                if (column.ColumnName == "type")
                {
                    context.Response.Write("订购/零售" + ",");
                }
                if (column.ColumnName == "proLD")
                {
                    context.Response.Write("出货料道" + ",");
                }
                if (column.ColumnName == "orderTime")
                {
                    context.Response.Write("订单时间" + ",");
                }
                if (column.ColumnName == "bh")
                {
                    context.Response.Write("配送机器" + ",");
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
                                context.Response.Write("\"" + ((DateTime)row[i]).ToString("yyyy-MM-dd hh:mm:ss") + "\",");
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