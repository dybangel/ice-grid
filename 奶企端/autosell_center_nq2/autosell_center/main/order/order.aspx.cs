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
    public partial class order : System.Web.UI.Page
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

            if (!IsPostBack)
            {
                this.companyID.Value = comID;
                string sql = "select * from asm_mechine where companyID=" + comID;
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                this.mechineList.DataTextField = "bh";
                this.mechineList.DataValueField = "id";
                this.mechineList.DataSource = dt;
                this.mechineList.DataBind();
                this.mechineList.Items.Insert(0, new ListItem("所有机器", "0")); //添加项

                string sql1 = "select * from  asm_activity where companyID=" + this.companyID.Value + " and  is_del=0";
                DataTable d1 = DbHelperSQL.Query(sql1).Tables[0];
                this.zqList.DataTextField = "zqName";
                this.zqList.DataValueField = "zq";
                this.zqList.DataSource = d1;
                this.zqList.DataBind();
                this.zqList.Items.Insert(0, new ListItem("所有周期", "0")); //添加项
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
        public static string getOrderList(string bh,string mechineID,string zq,string companyID,string pageCurrentCount,string zt,string keywords,string start,string end)
        {
           
            string sql = " 1=1 ";
            if (!string.IsNullOrEmpty(bh))
            {
                sql += "  and A.orderNO='" + bh + "'";
            }
            if (!string.IsNullOrEmpty(mechineID)&&mechineID!="0")
            {
                sql += " and A.mechineID="+mechineID;
            }
            if (!string.IsNullOrEmpty(zq)&&zq!="0")
            {
                sql += " and zqNum=" + zq;
            }
            if (!string.IsNullOrEmpty(zt) && zt != "-1")
            {
                sql += " and A.orderZT=" + zt;
            }
            if (!string.IsNullOrEmpty(keywords))
            {
                sql += " and (A.phone like '%"+keywords+"%' or A.memberID='"+keywords+"')";
            }
            if (!string.IsNullOrEmpty(start))
            {
                sql += " and A.createTime>'"+start+"'";
            }
            if (!string.IsNullOrEmpty(end))
            {
                sql += " and A.createTime<'" + end + "'";
            }
            string sql1 = "select A.*,b.bh,(select payType from asm_pay_info api where api.trxid=A.trxID) payType1 ,(select proName from asm_product ap where ap.productID=A.productID) proName from (select ao.*,am.name,am.phone ,(case when ao.orderZT=4 and exists(select 1 from asm_orderlistDetail aold where aold.zt=4 and  aold.orderNO=ao.orderNO ) then 1 else 0 end) changebefornum  from asm_orderlist ao left join asm_member  am on ao.memberID=am.id  where   fkzt=1 and am.companyID=" + companyID + ") A left join asm_mechine B on A.mechineID=B.id where  1=1 and " + sql;

            int startIndex = (int.Parse(pageCurrentCount)-1)*Config.pageSize+1;
            int endIndex = int.Parse(pageCurrentCount)*Config.pageSize;

            DataTable dt = Config.getPageDataTable("order by T.id desc", sql1,startIndex,endIndex);
            DataTable da = DbHelperSQL.Query(sql1).Tables[0];
            if (dt.Rows.Count > 0)
            {

                double d = double.Parse((da.Rows.Count / double.Parse(Config.pageSize.ToString())).ToString());
                string ss = Math.Ceiling(d) +"@@@"+OperUtil.DataTableToJsonWithJsonNet(dt);

                //string ss =OperUtil.DataTableToJsonWithJsonNet(da);
                return ss;
            }
            else
            {
                return "1";
            }
        }
        [WebMethod]
        public static object setDelay(string orderNO)
        {
            try
            {
                if (string.IsNullOrEmpty(orderNO))
                {
                    return new { code=500,msg="参数不全"};
                }
                string sql = "select * from asm_orderlist where orderNO='"+orderNO+"'";
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["orderZT"].ToString() == "1"|| dt.Rows[0]["orderZT"].ToString() == "0")
                    {
                        Util.Debuglog("暂停订单号=" + orderNO, "订单暂停.txt");
                        //设置为暂停
                        //把日期以后的订单zt=5的 设置为8
                        string update = "update asm_orderlist set orderZT=6 where id=" + dt.Rows[0]["id"].ToString();
                        int a = DbHelperSQL.ExecuteSql(update);
                        if (a > 0)
                        {
                            string update2 = "update asm_orderlistDetail set zt=8 where orderNO='" + orderNO+ "' and zt=5 and  DATEDIFF(dd,GETDATE(), createTime )>=2 ";
                            int a2 = DbHelperSQL.ExecuteSql(update2);
                            Util.Debuglog("暂停数量=" + a2, "订单暂停.txt");
                            Util.ClearRedisProductInfoByMechineID(dt.Rows[0]["mechineID"].ToString());
                          //  Util.orderDelay();
                            return new { code = 200, msg = "设置成功" };
                        }
                    } else if (dt.Rows[0]["orderZT"].ToString() == "6")
                    {
                        Util.Debuglog("恢复订单号=" + orderNO, "订单暂停.txt");
                        Util.SleepOrder(dt);
                       
                            return new { code = 200, msg = "设置成功" };
                       
                    }
                    return new { code = 500, msg = "订单状态不允许暂停" };
                }
                else {
                    return new { code = 500, msg = "订单查询异常" };
                }
            }
            catch {
                return new { code = 500, msg = "系统异常" };
            }
        }
        protected void excel_Click(object sender, EventArgs e)
        {
            
            string sql = " 1=1 ";
            if (!string.IsNullOrEmpty(this.bh.Value))
            {
                sql += "  and A.orderNO='" + this.bh.Value + "'";
            }
            if (!string.IsNullOrEmpty(this.mechineList.SelectedValue) && this.mechineList.SelectedValue != "0")
            {
                sql += " and A.mechineID=" + this.mechineList.SelectedValue;
            }
            if (!string.IsNullOrEmpty(this.zqList.SelectedValue) && this.zqList.SelectedValue != "0")
            {
                sql += " and zqNum=" + this.zqList.SelectedValue;
            }
            if (!string.IsNullOrEmpty(startTime.Value))
            {
                sql += " and A.createTime>'" + startTime.Value + "'";
            }
            if (!string.IsNullOrEmpty(endTime.Value))
            {
                sql += " and A.createTime<'" + endTime.Value + "'";
            }
            if (!string.IsNullOrEmpty(keywords.Value))
            {
                sql += " and (A.phone like '%" + keywords.Value + "%' or A.memberID='" + keywords.Value + "')";
            }
            string sql1 = "select A.orderNO,A.memberID,A.name,A.phone,A.productID,(select proName from asm_product ap where ap.productID=A.productID) proName,A.zqNum,A.totalNum,A.syNum,A.price,A.totalMoney,A.psModeStr,A.psCycle,A.startTime,A.endTime,A.trxid,case A.orderZT when '0' then '生产中' when '1' then '配送中' when '3' then '配送完成' when '4' then '已兑换' else '其他' end stu ,A.createTime ,A.mechineID,b.bh from (select ao.*,am.name,am.phone from asm_orderlist ao left join asm_member  am on ao.memberID=am.id  where am.companyID=" + this.companyID.Value + ") A left join asm_mechine B on A.mechineID=B.id where   fkzt=1 and " + sql;
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
                if (column.ColumnName == "orderNO")
                {
                    context.Response.Write("订单编号" + ",");
                }
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
                    context.Response.Write("联系电话" + ",");
                }
                if (column.ColumnName == "productID")
                {
                    context.Response.Write("商品ID" + ",");
                }
                if (column.ColumnName == "proName")
                {
                    context.Response.Write("商品名称" + ",");
                }
                if (column.ColumnName == "price")
                {
                    context.Response.Write("商品单价" + ",");
                }
                if (column.ColumnName == "totalMoney")
                {
                    context.Response.Write("订单总价" + ",");
                }
                if (column.ColumnName == "totalNum")
                {
                    context.Response.Write("总数" + ",");
                }
                if (column.ColumnName == "zqNum")
                {
                    context.Response.Write("周期" + ",");
                }
                if (column.ColumnName == "syNum")
                {
                    context.Response.Write("剩余天数" + ",");
                }
                if (column.ColumnName == "stu")
                {
                    context.Response.Write("订单状态" + ",");
                }
                if (column.ColumnName == "psModeStr")
                {
                    context.Response.Write("配送周期" + ",");
                }
                if (column.ColumnName == "startTime")
                {
                    context.Response.Write("开始日期" + ",");
                }
                if (column.ColumnName == "endTime")
                {
                    context.Response.Write("结束日期" + ",");
                }
                if (column.ColumnName == "psCycle")
                {
                    context.Response.Write("优惠方式" + ",");
                }
                if (column.ColumnName == "trxid")
                {
                    context.Response.Write("支付流水号" + ",");
                }
                if (column.ColumnName == "createTime")
                {
                    context.Response.Write("订单时间" + ",");
                }
                if (column.ColumnName == "mechineID")
                {
                    context.Response.Write("机器ID" + ",");
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