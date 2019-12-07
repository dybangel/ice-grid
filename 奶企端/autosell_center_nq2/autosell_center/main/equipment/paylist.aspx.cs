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

namespace autosell_center.main.equipment
{
    public partial class paylist : System.Web.UI.Page
    {
        public string comID = "";
        public string operaID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            comID = OperUtil.Get("companyID");
            this.agentID.Value = OperUtil.Get("operaID");
            operaID = this.agentID.Value;
            if (string.IsNullOrEmpty(comID) || string.IsNullOrEmpty(operaID))
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
        public static object getOrderList(string mechineIDList,string companyID,string start,string end,string pageCurrentCount,string selType,string fkzt,string keyword,string trxid)
        {

            string sql1 = "";
            if (!string.IsNullOrEmpty(mechineIDList))
            {
                sql1 += " and  mechineID in ("+mechineIDList+")";
            }
            if (!string.IsNullOrEmpty(start))
            {
                sql1 += " and paytime>'"+start.Replace("-","").Replace(":","").Replace(" ","")+"'";
            }
            if (!string.IsNullOrEmpty(end))
            {
                sql1 += " and paytime<'" + end.Replace("-", "").Replace(":", "").Replace(" ", "") + "'";
            }
            if (selType!="0")
            {
                sql1 += " and type="+selType;
            }
            if (fkzt!="-1")
            {
                sql1 += " and statu=" + fkzt;
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                string sqlM = "select * from asm_member where convert(varchar,id)='"+keyword+"' or phone='"+keyword+"'";
                DataTable dm = DbHelperSQL.Query(sqlM).Tables[0];
                if (dm.Rows.Count>0)
                {
                    sql1 += " and memberID='"+dm.Rows[0]["id"].ToString()+"'";
                }
               
            }
            if (!string.IsNullOrEmpty(trxid))
            {
                sql1 += " and trxid='"+trxid+"'";
            }
            string sql = "select * from(select A.*,(select bh from asm_mechine where id=A.mechineID) bh,"
                         +"  (select top 1 name from asm_member where unionID = A.unionID) name,"
                         + " (select top 1 id from asm_member where unionID = A.unionID) memberID,"
                         + " (select proName from asm_product where productID=A.productID)proname,"
                         + " (select top 1 phone from asm_member where unionID = A.unionID) phone"
                         +"  from"
                         + " (select p.*, s.productname, CONVERT(decimal(18,2),trxamt/100.00)totalMoney, s.bz from asm_pay_info p left join asm_sellDetail s on p.trxid = s.billno) A) C where companyID=" + companyID+sql1;
            int startIndex = (int.Parse(pageCurrentCount) - 1) * Config.pageSize + 1;
            int endIndex = int.Parse(pageCurrentCount) * Config.pageSize;

            DataTable dt = Config.getPageDataTable("order by T.id desc", sql, startIndex, endIndex);
            DataTable da = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count>0)
            {
                double d = double.Parse((da.Rows.Count / double.Parse(Config.pageSize.ToString())).ToString());
                return new { code=200,db=OperUtil.DataTableToJsonWithJsonNet(dt),count = Math.Ceiling(d) };
            }
            return null;
        }
        protected void excel_Click(object sender, EventArgs e)
        {
            string sql1 = "";
            if (!string.IsNullOrEmpty(_mechineList.Value))
            {
                sql1 += " and  p.mechineID in (" + _mechineList.Value + ")";
            }
            if (!string.IsNullOrEmpty(start.Value))
            {
                sql1 += " and paytime>'" + start.Value.Replace("-", "").Replace(":", "").Replace(" ", "") + "'";
            }
            if (!string.IsNullOrEmpty(end.Value))
            {
                sql1 += " and paytime<'" + end.Value.Replace("-", "").Replace(":", "").Replace(" ", "") + "'";
            }
            if (selType.SelectedValue != "0")
            {
                sql1 += " and p.type=" + selType.SelectedValue;
            }
            if (fkzt.SelectedValue != "-1")
            {
                sql1 += " and statu=" + fkzt.SelectedValue;
            }
            string sql = "select p.trxid,p.acct, p.createTime as paytime,p.trxdate,case  p.payType when '1' then '微信支付'when '2' then '支付宝' when '3' then '微信支付' when '4' then '零钱支付' else '其他' end payType, "
                        + " case  p.statu when '0' then '未支付'when '1' then '已支付'when '2' then '已退款' else '其他' end statu,case p.type when '1' then '充值' when '2' then '零售' when '3' then '订购' else '' end typeName,"
                        +" s.bz,CONVERT(decimal(18,2),trxamt/100.00)totalMoney,"
                       + " (select proName from asm_product where productID=p.productID)proname,"
                        + " (select top 1 name from asm_member where unionID = p.unionID) name,"
                        + " (select top 1 id from asm_member where unionID = p.unionID) memberID,"
                       + " (select top 1 phone from asm_member where unionID = p.unionID) phone,"
                       + " (select ''''+bh from  asm_mechine where id = p.mechineID) bh"
                       + "    from  asm_pay_info p left join asm_sellDetail s on p.trxid = s.billno where p.companyID="+this.companyID.Value+sql1;
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
                if (column.ColumnName == "trxid")
                {
                    context.Response.Write("支付流水号" + ",");
                }
                if (column.ColumnName == "proname")
                {
                    context.Response.Write("产品名称" + ",");
                }
                if (column.ColumnName == "totalMoney")
                {
                    context.Response.Write("金额" + ",");
                }

                if (column.ColumnName == "typeName")
                {
                    context.Response.Write("类型" + ",");
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
                    context.Response.Write("手机号" + ",");
                }
                if (column.ColumnName == "acct")
                {
                    context.Response.Write("支付ID" + ",");
                }
                if (column.ColumnName == "paytime")
                {
                    context.Response.Write("支付时间" + ",");
                }
                if (column.ColumnName == "trxdate")
                {
                    context.Response.Write("支付日" + ",");
                }
                if (column.ColumnName == "payType")
                {
                    context.Response.Write("支付方式" + ",");
                }
                if (column.ColumnName == "bh")
                {
                    context.Response.Write("机器编号" + ",");
                }
                if (column.ColumnName == "statu")
                {
                    context.Response.Write("交易状态" + ",");
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