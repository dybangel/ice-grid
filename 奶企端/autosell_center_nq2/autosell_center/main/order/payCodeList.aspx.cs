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

namespace autosell_center.main.order
{
    public partial class payCodeList : System.Web.UI.Page
    {
        public string comID = "";
        public string operaID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
           this._cardbh.Value = Request.QueryString["bh"].ToString();
            
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
        public static object sear(string pcbh, string keyword, string statuType,string pageCurrentCount,string cardNO)
        {
            string sql1 = "";
            if (!string.IsNullOrEmpty(pcbh))
            {
                sql1 += " and codeBH='" + pcbh + "'";
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                sql1 += " and (m.name like '%" + keyword + "%' or m.phone like '%" + keyword + "%')";
            }
            if (!string.IsNullOrEmpty(cardNO))
            {
                sql1 += " and cardNO like '%"+cardNO+"%'";
            }
            if (statuType!="0")
            {
                sql1 += " and statu="+statuType;
            }
            string sql = "select p.*,m.name,m.phone from asm_payCodeList p left join asm_member m on p.memberID=m.id  where 1=1 "+sql1;
            int startIndex = (int.Parse(pageCurrentCount) - 1) * Config.pageSize + 1;
            int endIndex = int.Parse(pageCurrentCount) * Config.pageSize;

            DataTable dt = Config.getPageDataTable("order by T.id desc", sql, startIndex, endIndex);
            DataTable da = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                double d = double.Parse((da.Rows.Count / double.Parse(Config.pageSize.ToString())).ToString());
                return new { code = 200, db = OperUtil.DataTableToJsonWithJsonNet(dt), count = Math.Ceiling(d) };
            }
            return new { code = 500, msg="" };
        }
        [WebMethod]
        public static object setZF(string id)
        {
            string sql = "select * from asm_payCodeList where id="+id;
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count>0)
            {
                if (dt.Rows[0]["statu"].ToString() == "1")
                {
                    string update = "update asm_payCodeList set statu=3 where id="+id;
                    int a= DbHelperSQL.ExecuteSql(update);
                    if (a>0)
                    {
                        return new { code = 200, msg = "已作废成功" };
                    }
                    return new { code = 500, msg = "作废失败" };
                }
                else {
                    return new { code=500,msg="状态不允许作废"};
                }
            }
            return new { code=500,msg="查询异常"};
        }
        protected void excel_Click(object sender, EventArgs e)
        {
            string sql1 = "";
            if (!string.IsNullOrEmpty(_cardbh.Value))
            {
                sql1 += " and codeBH='" + _cardbh.Value + "'";
            }
            if (!string.IsNullOrEmpty(keyword.Value))
            {
                sql1 += " and (m.name like '%" + keyword.Value + "%' or m.phone like '%" + keyword.Value + "%')";
            }
            if (!string.IsNullOrEmpty(cardNO.Value))
            {
                sql1 += " and cardNO like '%" + cardNO.Value + "%'";
            }
            if (statuType.SelectedValue != "0")
            {
                sql1 += " and statu=" + statuType.SelectedValue;
            }
            string sql = "select p.codeBH,p.cardNO,p.mzMoney,p.pwd,p.startTime,p.endTime,case when statu='1' then '待兑换' when statu='2' then '已兑换' when statu='3' then '已作废' else '' end stuName"
                + ",m.name,m.phone from asm_payCodeList p left join asm_member m on p.memberID=m.id  where 1=1 " + sql1;
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            ExportToSpreadsheet(dt,DateTime.Now.ToString("yyyyMMddHHmm"));
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
                if (column.ColumnName == "codeBH")
                {
                    context.Response.Write("批次" + ",");
                }
                if (column.ColumnName == "cardNO")
                {
                    context.Response.Write("卡号" + ",");
                }
                if (column.ColumnName == "mzMoney")
                {
                    context.Response.Write("面值" + ",");
                }
                if (column.ColumnName == "pwd")
                {
                    context.Response.Write("密码" + ",");
                }
                if (column.ColumnName == "name")
                {
                    context.Response.Write("会员名称" + ",");
                }
                if (column.ColumnName == "phone")
                {
                    context.Response.Write("手机号" + ",");
                }
                if (column.ColumnName == "startTime")
                {
                    context.Response.Write("开始时间" + ",");
                }
                if (column.ColumnName == "endTime")
                {
                    context.Response.Write("结束时间" + ",");
                }
               
                if (column.ColumnName == "stuName")
                {
                    context.Response.Write("状态" + ",");
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