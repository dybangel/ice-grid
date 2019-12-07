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
    public partial class payCode : System.Web.UI.Page
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
        public static object sear(string companyID,string startTime,string endTime,string keyword,string statu, string pageCurrentCount)
        {
            string sql1 = "";
            if (!string.IsNullOrEmpty(companyID))
            {
                sql1 += " and companyID="+companyID;
            }
            if (!string.IsNullOrEmpty(startTime))
            {
                sql1 += " and createTime>'" + startTime+"'";
            }
            if (!string.IsNullOrEmpty(endTime))
            {
                sql1 += " and createTime<'" + endTime+"'";
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                sql1 += " and (bh like '%"+keyword+ "%' or name like '%"+keyword+"%')";
            }
            if (statu!="0")
            {
                sql1 += " and statu="+statu;
            }
            string sql = "select *,(select name from asm_opera where id=createOpera) operaName from asm_payCode where 1=1 " + sql1;
            int startIndex = (int.Parse(pageCurrentCount) - 1) * Config.pageSize + 1;
            int endIndex = int.Parse(pageCurrentCount) * Config.pageSize;

            DataTable dt = Config.getPageDataTable("order by T.id desc", sql, startIndex, endIndex);
            DataTable da = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count>0)
            {
                double d = double.Parse((da.Rows.Count / double.Parse(Config.pageSize.ToString())).ToString());
                return new { code=200,db=OperUtil.DataTableToJsonWithJsonNet(dt),count= Math.Ceiling(d) };
            }
            return new { code=500,msg=""};
        }
        [WebMethod]
        public static object setZF(string bh)
        {
            string sql = "select * from asm_payCode where bh='"+bh+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["statu"].ToString() == "1")
                {
                    string update = "update asm_payCode set statu=2 where bh='" + bh + "' and statu=1";
                    int a = DbHelperSQL.ExecuteSql(update);
                    if (a > 0)
                    {
                        string update1 = "update asm_payCodeList set statu=3 where codeBH='" + bh + "' and statu=1";
                        DbHelperSQL.ExecuteSql(update1);
                        return new { code = 200, msg = "已作废成功" };
                    }
                    return new { code = 500, msg = "作废失败" };
                } else if (dt.Rows[0]["statu"].ToString() == "2")
                {
                    string update = "update asm_payCode set statu=1 where bh='" + bh + "' and statu=2";
                    int a = DbHelperSQL.ExecuteSql(update);
                    if (a > 0)
                    {
                        string update1 = "update asm_payCodeList set statu=1 where codeBH='" + bh + "' and statu=3";
                        DbHelperSQL.ExecuteSql(update1);
                        return new { code = 200, msg = "已启用" };
                    }
                    return new { code = 500, msg = "启用失败" };
                }
                 
            }
            return new { code = 500, msg = "查询异常" };
        }
        [WebMethod]
        public static object saveAdd(string bh,string name,string startNO,string cardNum,string cardMoney,string startTime,string endTime,string operaID,string loginPwd, string companyID)
        {
            string sqlC = "select * from  asm_company where id="+companyID+" and pwd2='"+loginPwd+"'";
            DataTable dc = DbHelperSQL.Query(sqlC).Tables[0];
            if (dc.Rows.Count<=0)
            {
                return new { code=500,msg = "密码错误"};
            }
            string sql = "select * from asm_payCode where bh='"+bh+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count>0)
            {
                return new { code = 500, msg = "批次编号重复" };
            }
            sql = "select * from asm_payCode where name='" + name + "'";
            dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return new { code = 500, msg = "卡片名称重复" };
            }
            try
            {
                
                if (int.Parse(cardNum)<=0)
                {
                    return new { code = 500, msg = "发行数量不正确" };
                }
            }
            catch {
                return new { code = 500, msg = "发行数量不正确" };
            }
            try
            {
                if (double.Parse(cardMoney)<=0)
                {
                    return new { code = 500, msg = "卡面值不正确" };
                }
            }
            catch {
                return new { code = 500, msg = "卡面值不正确" };
            }
            if (startNO.Length!=12)
            {
                return new { code = 500, msg = "起始号码为12位" };
            }
            if (startNO.Substring(0,1)=="0")
            {
                return new { code = 500, msg = "起始号码不正确" };
            }
            if (string.IsNullOrEmpty(startTime)||string.IsNullOrEmpty(endTime))
            {
                return new { code = 500, msg = "请输入卡片有效期" };
            }
            string insert = "insert into asm_payCode (name,startNO,num,mzMoney,createOpera,startTime,endTime,statu,bh,companyID,createTime)values('" + name+"','"+startNO+"','"+cardNum+"','"+cardMoney+"','"+operaID+"','"+startTime+"','"+endTime+"','1','"+bh+"','"+ companyID + "','"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"')";
            int a= DbHelperSQL.ExecuteSql(insert);
            if (a>0)
            {
                List<string> list = new List<string>();
                for (int i=0;i<int.Parse(cardNum);i++)
                { 
                    int pwd= new Random(Guid.NewGuid().GetHashCode()).Next(100000, 999999);
                    string cardNO = (Convert.ToInt64(startNO) + i).ToString();
                    string sqlInsert = "insert into asm_payCodeList(codeBH,cardNO,mzMoney,pwd,startTime,endTime,statu,companyID) values('" + bh+"','"+cardNO+"','"+cardMoney+"','"+pwd+"','"+startTime+"','"+endTime+"','1','"+ companyID + "')";
                    list.Add(sqlInsert);
                }
                int b= DbHelperSQL.ExecuteSqlTran(list);
                if (b>0)
                {
                    return new { code = 200, msg = "生成成功" };
                }
            }
            
            return new { code = 500, msg = "生成失败" };
        }
        protected void excel_Click(object sender, EventArgs e)
        {
            string sql1 = "";
            if (!string.IsNullOrEmpty(companyID.Value))
            {
                sql1 += " and companyID=" + companyID.Value;
            }
            if (!string.IsNullOrEmpty(startTime.Value))
            {
                sql1 += " and createTime>'" + startTime.Value + "'";
            }
            if (!string.IsNullOrEmpty(endTime.Value))
            {
                sql1 += " and createTime<'" + endTime.Value + "'";
            }
            if (!string.IsNullOrEmpty(bh.Value))
            {
                sql1 += " and (bh like '%" + bh.Value + "%' or name like '%" + bh.Value + "%')";
            }
            if (statuType.SelectedValue != "0")
            {
                sql1 += " and statu=" + statuType.SelectedValue;
            }
            string sql = "select bh,name,num,mzMoney,startNO,startTime,endTime,case when statu='1' then '正常' when statu='2' then '禁用'  else '' end stuName,createTime,"
                + " (select name from asm_opera where id=createOpera) operaName from asm_payCode where 1=1 " + sql1;
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
                if (column.ColumnName == "bh")
                {
                    context.Response.Write("批次" + ",");
                }
                if (column.ColumnName == "name")
                {
                    context.Response.Write("卡名称" + ",");
                }
                if (column.ColumnName == "num")
                {
                    context.Response.Write("发行数量" + ",");
                }
                if (column.ColumnName == "mzMoney")
                {
                    context.Response.Write("单张面值" + ",");
                }
                if (column.ColumnName == "startNO")
                {
                    context.Response.Write("起始号" + ",");
                }
                if (column.ColumnName == "startTime")
                {
                    context.Response.Write("开始时间" + ",");
                }
                if (column.ColumnName == "endTime")
                {
                    context.Response.Write("结束时间" + ",");
                }
                if (column.ColumnName == "createTime")
                {
                    context.Response.Write("创建日期" + ",");
                }
                if (column.ColumnName == "stuName")
                {
                    context.Response.Write("状态" + ",");
                }
                if (column.ColumnName == "operaName")
                {
                    context.Response.Write("创建人" + ",");
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