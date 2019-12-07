using autosell_center.cls;
using autosell_center.util;
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
    public partial class orderform : System.Web.UI.Page
    {
        public string comID = "";
        public string operaID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            comID = OperUtil.Get("companyID");
            operaID = OperUtil.Get("operaID");
            this._operaID.Value = operaID;
            if (string.IsNullOrEmpty(comID)||string.IsNullOrEmpty(operaID))
            {
                Response.Write("<script>alert('您尚未登录或已长时间未进行操作，请重新登录!');top.location.href='../../index.aspx';</script>");
                return;
            }
            this.companyID.Value = comID;

            if (operaID != "0")
            {
                string sqlme = "select  id sCode,bh sName from  asm_mechine where dls='"+operaID+"' and companyID=" + comID;
                DataSet dd = DbHelperSQL.Query(sqlme);
                this.cbosDeparentment.dtDataList = dd;

                string sql1 = "select * from asm_opera where agentID='"+operaID+"' and  companyID=" + comID;
                DataTable dt1 = DbHelperSQL.Query(sql1).Tables[0];
                this.operaList.DataTextField = "name";
                this.operaList.DataValueField = "id";
                this.operaList.DataSource = dt1;
                this.operaList.DataBind();
                //this.operaList.Items.Insert(0, new ListItem("全部", "0")); //添加项
            }
            else {
                string sqlme = "select  id sCode,bh sName from  asm_mechine where companyID=" + comID;
                DataSet dd = DbHelperSQL.Query(sqlme);
                this.cbosDeparentment.dtDataList = dd;

                string sql1 = "select * from asm_opera where companyID=" + comID;
                DataTable dt1 = DbHelperSQL.Query(sql1).Tables[0];
                this.operaList.DataTextField = "name";
                this.operaList.DataValueField = "id";
                this.operaList.DataSource = dt1;
                this.operaList.DataBind();
                this.operaList.Items.Insert(0, new ListItem("全部", "0")); //添加项
            }
          
        }
        [WebMethod]
        public static string sear(string mechineID,string companyID,string time, string operaID,string pageCurrentCount,string endTime,string dls_ID)
        {
         
            string sql1 = "";
            string sql = "";
            string sql3 = "select * from asm_mechine where companyid='" + companyID + "'";
            if (dls_ID != "0")
            {
                 sql3 = "select * from asm_mechine where dls='"+ dls_ID + "' and companyid='" + companyID + "'";
            }
           
            DataTable d3 = DbHelperSQL.Query(sql3).Tables[0];
            string mechine_ID = "";
            if (d3.Rows.Count > 0)
            {
                for (int i = 0; i < d3.Rows.Count; i++)
                {
                    mechine_ID += d3.Rows[i]["id"].ToString() + ",";
                }
                mechine_ID = mechine_ID.Substring(0, mechine_ID.Length - 1);
            }

            if (!string.IsNullOrEmpty(mechineID))
            {
                sql += "  and mechineID in(" + mechineID + ")";
            }
            else {
                sql += "  and mechineID in(" + mechine_ID + ")";
            }
            if (!string.IsNullOrEmpty(time))
            {
                sql += " and createTime>='" + time+ "'";
            }
            if (!string.IsNullOrEmpty(endTime))
            {
                sql += " and createTime<='" + endTime + "'";
            }
            if (operaID!="0")
            {
                sql += " and operaID="+operaID;
            }
            sql1 = @"select F.createTime,F.mechineID,F.mechineName,F.bh,F.operaName,F.operaID,SUM(f.num) num from (select A.*,B.mechineName,B.bh,(select name from asm_opera ao where ao.id=B.operaID) operaName,B.operaID from 
                    (select createTime, count(*) num, mechineID, '1' type from asm_orderDetail where 1 = 1 and zt!=7 and zt!=2 group by createTime, mechineID
                    union all
                    select convert(varchar(100), delivertTime, 23) delTime, sum(num) num, mechineID, '2' type from asm_reserve group by convert(varchar(100), delivertTime, 23), mechineiD) A left join asm_mechine B on A.mechineID = B.id where 1=1 " + sql+ ") F group by F.createTime,F.mechineID,F.mechineName,F.bh,F.operaName,F.operaID ";
           
            int startIndex = (int.Parse(pageCurrentCount) - 1) * Config.pageSize + 1;
            int endIndex = int.Parse(pageCurrentCount) * Config.pageSize;

            DataTable dt = Config.getPageDataTable("order by T.mechineID", sql1, startIndex, endIndex);
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

        protected void excel_Click(object sender, EventArgs e)
        {
           
            string sql1 = " 1=1";
            if (this.start.Value.Trim()!="")
            {
                sql1 += " and createTime='"+this.start.Value.Trim()+"'";
            }
            string mechineID =this.mechine_ID.Value;
            if (string.IsNullOrEmpty(mechineID))
            {
                if (this._operaID.Value != "0")
                {
                    string sql12 = "select * from asm_mechine where dls='"+this._operaID.Value+"' and  companyID=" + this.companyID.Value;
                    DataTable dd = DbHelperSQL.Query(sql12).Tables[0];
                    if (dd.Rows.Count > 0)
                    {
                        for (int i = 0; i < dd.Rows.Count; i++)
                        {
                            mechineID += dd.Rows[i]["id"].ToString() + ",";
                        }
                        mechineID = mechineID.Substring(0, mechineID.Length - 1);
                    }
                    else
                    {
                        mechineID = "0";
                    }
                }
                else {
                    string sql12 = "select * from asm_mechine where companyID=" + this.companyID.Value;
                    DataTable dd = DbHelperSQL.Query(sql12).Tables[0];
                    if (dd.Rows.Count > 0)
                    {
                        for (int i = 0; i < dd.Rows.Count; i++)
                        {
                            mechineID += dd.Rows[i]["id"].ToString() + ",";
                        }
                        mechineID = mechineID.Substring(0, mechineID.Length - 1);
                    }
                    else
                    {
                        mechineID = "0";
                    }
                }
              
            }
            string sql = "select productID,"
                         +"   proName,"
                         +"   sum(dgNUm) dg,"
                         +"   sum(lsNUM)  ls,"
                         +"   sum(dgNUm)+sum(lsNUM) totalNum,"
                         +"   createTime,"
                         + "  (select bh from asm_mechine where id=mechineID) bh,"
                         + "  (select name operaName from asm_opera where id in(select operaID from asm_mechine where id in ("+ mechineID + "))) operaName"
                         +"   from("
                         +"   select ao.productID, ap.proName,ao.mechineID, count(*) dgNUm, '0'lsNUM,createTime from asm_orderDetail ao left join asm_product ap on ao.productID = ap.productID where ao.mechineID  in("+mechineID+ ") and ao.zt!=7 group by ao.productID, ap.proName,createTime,ao.mechineID"
                         + "   union all "
                         +"   select ar.productID, ap.proName,ar.mechineID, '0' dgNUm, sum(num) lsNUM,convert(varchar(100), ar.delivertTime, 23) from asm_reserve ar left join asm_product ap on ar.productID = ap.productID where ar.mechineID in("+mechineID+")  group by ar.productID,ar.mechineID, ap.proName,convert(varchar(100), ar.delivertTime, 23)) "
                         +"   C  where " + sql1+" group by productID, proName,createTime,mechineID order by productID";
            DataTable dda = DbHelperSQL.Query(sql).Tables[0];
            ExportToSpreadsheet(dda, DateTime.Now.ToString("yyyyMMdd"));
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
                if (column.ColumnName == "productID")
                {
                    context.Response.Write("商品ID" + ",");
                }
                if (column.ColumnName == "proName")
                {
                    context.Response.Write("商品名称" + ",");
                }
                if (column.ColumnName == "dg")
                {
                    context.Response.Write("订购数量" + ",");
                }
                if (column.ColumnName == "ls")
                {
                    context.Response.Write("零售数量" + ",");
                }
                if (column.ColumnName == "totalNum")
                {
                    context.Response.Write("订单总量" + ",");
                }
                if (column.ColumnName == "createTime")
                {
                    context.Response.Write("订单日期" + ",");
                }
                if (column.ColumnName == "bh")
                {
                    context.Response.Write("机器编号" + ",");
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