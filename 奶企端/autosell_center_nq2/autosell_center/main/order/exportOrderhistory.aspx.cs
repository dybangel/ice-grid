using autosell_center.cls;
using autosell_center.util;
using Consumer.cls;
using DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace autosell_center.main.order
{
    public partial class exportOrderhistory : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(OperUtil.Get("companyID")))
            {
                Response.Write("<script>alert('您尚未登录或已长时间未进行操作，请重新登录!');top.location.href='../../index.aspx';</script>");
                return;
            } 
            this.agentID.Value = OperUtil.Get("operaID");
            this.companyID.Value = OperUtil.Get("companyID");
        }
        [WebMethod]
        public static object getList(string companyID,string pageCurrentCount,string phone,string type,string start,string end,string tjr,string orderCode)
        {
            string sql1 = "";
            if (!string.IsNullOrEmpty(start))
            {
                sql1 += " and dg.createTime>'" + start + "'";
            }
            if (!string.IsNullOrEmpty(end))
            {
                sql1 += " and dg.createTime<'" + end + "'";
            }
            if (!string.IsNullOrEmpty(tjr))
            {
                sql1 += " and dg.tjr like  '%" + tjr + "%'";
            }
            if (!string.IsNullOrEmpty(phone))
            {
                sql1 += " and dg.phone='"+phone+"'";
            }
            if (type!="-1")
            {
                sql1 += " and dg.status="+type;
            }
            if (!string.IsNullOrEmpty(orderCode))
            {
                sql1 += " and dg.orderCode='"+orderCode+"'";
            }
            string sql = "select ISNULL(m.name, '暂无绑定') name ,ISNULL(m.phone, '暂无绑定') as mphone, dg.*, p.proname  from asm_dgOrder dg LEFT JOIN asm_product p  on p.bh=dg.productCode   left join asm_member m on dg.memberID = m.id where dg.companyID = " + companyID+sql1;

            int startIndex = (int.Parse(pageCurrentCount) - 1) * Config.pageSize + 1;
            int endIndex = int.Parse(pageCurrentCount) * Config.pageSize;

            DataTable dt = Config.getPageDataTable("order by T.id", sql, startIndex, endIndex);
            DataTable da = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                double d = double.Parse((da.Rows.Count / double.Parse(Config.pageSize.ToString())).ToString());
                return new { code = 200, db = OperUtil.DataTableToJsonWithJsonNet(dt), count = Math.Ceiling(d) };
            }
            else {
                return new { code=300,msg=""};
            }
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
        public static object sendMsg(string id)
        {
            try
            {
                string sql = " select * from  asm_dgOrder where id="+id;
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count>0)
                {
                    //需要异步执行的操作比如发邮件、发短信等
                    string phone = dt.Rows[0]["phone"].ToString();
                    string code = dt.Rows[0]["orderCode"].ToString();
                    string result = OperUtil.sendMessage5(phone, code);
                    return new { code = 200, msg = result };
                }
                return new { code = 500, msg = "没有查询到记录" };
            }
            catch {
                return new { code = 500, msg = "系统异常" };
            }
        }
        [WebMethod]
        public static  object delData(string id)
        {
            try
            {
                string sql = "select * from asm_dgOrder where id="+id;
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count>0)
                {
                    if (dt.Rows[0]["status"].ToString()=="1")
                    {
                        return new {code=500,msg="已兑换的订单无法删除" };
                    }
                    int a = DbHelperSQL.ExecuteSql("delete from asm_dgOrder where id="+id);
                    if (a>0)
                    {
                        return new { code=200,msg="删除成功"};
                    }
                    return new { code = 500, msg = "删除失败" };
                }
                return new { code = 500, msg = "订单查询异常" };
            }
            catch {
                return new { code=500,msg="系统异常"};
            }
        }
        protected void excel_Click(object sender, EventArgs e)
        {
            string sql1 = "";
            string start = this.start.Value;
            string end= this.end.Value;
            string tjr = this.tjr.Value;
            string phone = this.phone.Value;
            string type = this.dhtype.Value;
            if (!string.IsNullOrEmpty(start))
            {
                sql1 += " and dg.createTime>'" + start + "'";
            }
            if (!string.IsNullOrEmpty(end))
            {
                sql1 += " and dg.createTime<'" + end + "'";
            }
            if (!string.IsNullOrEmpty(tjr))
            {
                sql1 += " and dg.tjr like  '%" + tjr + "%'";
            }
            if (!string.IsNullOrEmpty(phone))
            {
                sql1 += " and dg.phone='" + phone + "'";
            }
            if (type != "-1")
            {
                sql1 += " and dg.status=" + type;
            }
            string sql = "select dg.phone,m.name,dg.productCode,(select proname from asm_product p where p.bh=dg.productCode)proname,dg.productPrice,dg.zq,dg.orderCode,case dg.status when 1 then '已兑换' else '未兑换' end status,dg.tjr,dg.createTime,dg.dhTime,dg.bz  from asm_dgOrder dg left join asm_member m on dg.phone=m.phone where dg.companyID=" + this.companyID.Value + sql1;
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            ExportToSpreadsheet(dt, DateTime.Now.ToString("yyyyMMdd") + "订单导入记录");
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
                if (column.ColumnName == "phone")
                {
                    context.Response.Write("手机号" + ",");
                }
                if (column.ColumnName == "name")
                {
                    context.Response.Write("会员名称" + ",");
                }
                if (column.ColumnName == "productCode")
                {
                    context.Response.Write("商品条码" + ",");
                }
                if (column.ColumnName == "proname")
                {
                    context.Response.Write("商品名称" + ",");
                }
                if (column.ColumnName == "productPrice")
                {
                    context.Response.Write("商品单价" + ",");
                }
                if (column.ColumnName == "zq")
                {
                    context.Response.Write("订单周期" + ",");
                }
                if (column.ColumnName == "orderCode")
                {
                    context.Response.Write("订奶码" + ",");
                }
                if (column.ColumnName == "status")
                {
                    context.Response.Write("状态" + ",");
                }
                if (column.ColumnName == "tjr")
                {
                    context.Response.Write("推荐人" + ",");
                }
                if (column.ColumnName == "createTime")
                {
                    context.Response.Write("导入时间" + ",");
                }
                if (column.ColumnName == "dhTime")
                {
                    context.Response.Write("兑换时间" + ",");
                }
                if (column.ColumnName == "bz")
                {
                    context.Response.Write("备注" + ",");
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