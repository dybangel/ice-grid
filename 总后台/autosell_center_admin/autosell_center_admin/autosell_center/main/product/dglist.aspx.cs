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
    public partial class dglist : System.Web.UI.Page
    {
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
                string sql2 = "select * from asm_mechine";
                DataTable dt2 = DbHelperSQL.Query(sql2).Tables[0];
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
                sql += " and E.mechineID=" + mechineID;
            }
            if (!string.IsNullOrEmpty(companyID) && companyID != "0")
            {
                sql += " and E.companyID=" + companyID;
            }
            string sql1 = "select E.*,F.proName,F.price2 from (select C.*,D.name nickName from (select A.*,B.name from (select ao.*, am.bh,am.companyID from asm_order ao left join asm_mechine am on ao.mechineID=am.id) A left join asm_company B on A.companyID=B.id) C left join asm_member D on C.memberID=D.id) E left join asm_product F on E.productID=F.productID where 1=1 and " + sql;
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
            if (this.companyList.SelectedValue!="0")
            {
                string sql2 = "select * from asm_mechine where companyID=" + this.companyList.SelectedValue;
                DataTable dt2 = DbHelperSQL.Query(sql2).Tables[0];
                this.mechineList.DataTextField = "bh";
                this.mechineList.DataValueField = "id";
                this.mechineList.DataSource = dt2;
                this.mechineList.DataBind();
            }
            else
            {
                string sql2 = "select * from asm_mechine";
                DataTable dt2 = DbHelperSQL.Query(sql2).Tables[0];
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
            if (!string.IsNullOrEmpty(this.mechineList.SelectedValue) && this.mechineList.SelectedValue != "0")
            {
                sql += " and E.mechineID=" + this.mechineList.SelectedValue;
            }
            if (!string.IsNullOrEmpty(this.companyList.SelectedValue) && this.companyList.SelectedValue != "0")
            {
                sql += " and E.companyID=" + this.companyList.SelectedValue;
            }
            string sql1 = "select E.orderNO,F.proName,F.price2,E.nickName,E.zq,E.syNum,E.qsDate,E.zdDate,E.psStr,E.totalMoney,E.bh,E.name from (select C.*,D.name nickName from (select A.*,B.name from (select ao.*, am.bh,am.companyID from asm_order ao left join asm_mechine am on ao.mechineID=am.id) A left join asm_company B on A.companyID=B.id) C left join asm_member D on C.memberID=D.id) E left join asm_product F on E.productID=F.productID where 1=1 and " + sql;
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
                if (column.ColumnName == "proName")
                {
                    context.Response.Write("产品名称" + ",");
                }
                if (column.ColumnName == "price2")
                {
                    context.Response.Write("单价" + ",");
                }
                if (column.ColumnName == "nickName")
                {
                    context.Response.Write("会员昵称" + ",");
                }
                if (column.ColumnName == "zq")
                {
                    context.Response.Write("周期" + ",");
                }
                if (column.ColumnName == "syNum")
                {
                    context.Response.Write("剩余天数" + ",");
                }
                if (column.ColumnName == "qsDate")
                {
                    context.Response.Write("起送日期" + ",");
                }
                if (column.ColumnName == "zdDate")
                {
                    context.Response.Write("止送日期" + ",");
                }
                if (column.ColumnName == "psStr")
                {
                    context.Response.Write("派送方式" + ",");
                }
                if (column.ColumnName == "totalMoney")
                {
                    context.Response.Write("总金额" + ",");
                }
                if (column.ColumnName == "bh")
                {
                    context.Response.Write("机器编号" + ",");
                }
                if (column.ColumnName == "name")
                {
                    context.Response.Write("所属企业" + ",");
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