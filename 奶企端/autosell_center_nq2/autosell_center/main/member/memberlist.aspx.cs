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

namespace autosell_center.main.member
{
    public partial class memberlist : System.Web.UI.Page
    {
        public string totalMember = "0";
        public string todayMember = "0";
        public string yesdayMember = "0";
        public string comID = "";
        public string operaID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            comID = OperUtil.Get("companyID");
            this._operaID.Value = OperUtil.Get("operaID");
            operaID = OperUtil.Get("operaID");
            if (string.IsNullOrEmpty(comID)||string.IsNullOrEmpty(operaID))
            {
                Response.Write("<script>alert('您尚未登录或已长时间未进行操作，请重新登录!');top.location.href='../../index.aspx';</script>");
                return;
            } 
            this.companyID.Value = comID;
            if (!IsPostBack)
            {
                initData();
            }
        }
        [WebMethod]
        public static object sendok(string companyID,string memberID, string zsdays) {

            try
            {
                if (string.IsNullOrEmpty(companyID)||string.IsNullOrEmpty(memberID)||string.IsNullOrEmpty(zsdays))
                {
                    return new { code=500,msg="请填写对应的信息"};
                }
                try
                {
                    int.Parse(zsdays);
                }
                catch {
                    return new { code = 500, msg = "赠送天数不正确" };
                }
                string[] memberIDArr = memberID.Split(',');
                if (memberIDArr.Length>0)
                {
                    List<string> list = new List<string>();
                    for (int i=0;i<memberIDArr.Length;i++)
                    {
                        string sql = "select * from asm_member where id="+memberIDArr[i]+" and openID is not null and openID !=''";
                        DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                        if (dt.Rows.Count>0)
                        {
                            string update = "update asm_member set dj=3 ,hjhyDays="+zsdays+" where id="+memberIDArr[i];
                            list.Add(update);
                            //string openID = dt.Rows[0]["openID"].ToString();
                            //wxHelper wx = new wxHelper(companyID);
                            //string data = TemplateMessage.getPrize(openID, OperUtil.getMessageID(companyID, "OPENTM411984357"), title,activity,prize,url);
                            //TemplateMessage.SendTemplateMsg(wx.IsExistAccess_Token(companyID), data);
                        }
                    }
                    DbHelperSQL.ExecuteSqlTran(list);
                    return new { code = 200, msg = "发送成功" };
                }
            }
            catch {

            }

            return new { code = 500, msg = "系统错误" };
        }
        [WebMethod]
        public static object judge(string operaID,string menuID)
        {
           Boolean b= Util.judge(operaID,menuID);
            if (b)
            {
                return new { code = 200 };
            }
            else {
                return new { code = 500 };
            }
        }
        public void initData()
        {
            
            string sql = "select * from [dbo].[asm_mechine] where companyID=" + comID;
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            this.companyList.DataTextField = "bh";
            this.companyList.DataValueField = "id";
            this.companyList.DataSource = dt;
            this.companyList.DataBind();
            this.companyList.Items.Insert(0, new ListItem("全部", "0")); //添加项

            string sqlAll = "select COUNT(*) allVal,(select count(*) yesVal from asm_member  where datediff(dd,createDate,GETDATE())=1) yesVal,(select count(*) yesVal from asm_member  where datediff(dd,createDate,GETDATE())=0) todayVal from asm_member where companyID="+comID;
            DataTable dtAll = DbHelperSQL.Query(sqlAll).Tables[0];
            totalMember = dtAll.Rows[0]["allVal"].ToString();
            todayMember = dtAll.Rows[0]["todayVal"].ToString();
            yesdayMember = dtAll.Rows[0]["yesVal"].ToString();
        }
        [WebMethod]
        public static object updateMember(string id,string days,string phone)
        {
            try
            {
                string sql = "select * from asm_member where id="+id;
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count>0)
                {
                    string sql1 = "select * from  asm_member where id!="+id+" and phone='"+phone+"'";
                    DataTable d1 = DbHelperSQL.Query(sql1).Tables[0];
                    if (d1.Rows.Count>0)
                    {
                        return new { code = 500, msg = "手机号重复" };
                    }
                    if (phone.Length!=11||phone.Substring(0,1)!="1")
                    {
                        return new { code = 500, msg = "手机号不正确" };
                    }
                    try
                    {
                        int day = int.Parse(days);
                        if (day > 0)
                        {
                            string update = "update asm_member set hjhydays=" + day + ",dj=3,phone='"+phone+"' where id=" + id;
                            DbHelperSQL.ExecuteSql(update);
                        }
                        else {
                            string sql3 = "select * from  [dbo].[View_member_consumeCount30] where id="+id;
                            DataTable d3 = DbHelperSQL.Query(sql3).Tables[0];
                            string sql2 = "SELECT top 1 * FROM asm_dj where companyID="+dt.Rows[0]["companyID"].ToString()+" and consumeDay<"+d3.Rows[0][""].ToString()+" ORDER BY djID DESC";
                            DataTable d2 = DbHelperSQL.Query(sql2).Tables[0];
                            string update = "update asm_member set hjhydays=" + day + ",dj="+d2.Rows[0]["djID"].ToString()+",phone='"+phone+"' where id=" + id;
                            DbHelperSQL.ExecuteSql(update);
                        }
                        return new { code = 200, msg = "设置正确" };
                    }
                    catch {
                        return new { code = 500, msg = "天数设置异常" };
                    }
                }
            }
            catch {
                return new { code=500,msg="系统异常"};
            }
            return new { code = 500, msg = "系统异常" };
        }
        [WebMethod]
        public static string getMemberList(string keyword, string qy, string start, string end,string pageCurrentCount,string mechineID,string dj,string minMoney,string maxMoney)
        {
          
            string sql = "1=1";
            if (keyword.Trim() != "")
            {
                sql += " and (A.memberBH='" + keyword + "' or A.name='" + keyword + "' or A.phone='" + keyword + "' or CONVERT(varchar,A.id)='"+keyword+"')";
            }
            if (qy != "0")
            {
                sql += " and A.companyID=" + qy;
            }
            if (mechineID != "0")
            {
                sql += " and A.mechineID=" + mechineID;
            }
            if (start.Trim() != "")
            {
                sql += " and A.createDate>'" + start + "'";
            }
            if (end.Trim() != "")
            {
                sql += " and A.createDate<'" + end + "'";
            }
            if (dj!="-1")
            {
                sql += " and A.dj="+dj;
            }
            if (minMoney!="")
            {
                sql += " and A.AvailableMoney>'"+minMoney+"'";
            }
            if (maxMoney!="")
            {
                sql += " and A.AvailableMoney<'" + maxMoney + "'";
            }
            string sql1 = "select A.*,B.name companyName from (select am.*,ac.bh   from asm_member am left join asm_mechine ac on am.mechineID=ac.id) A left join asm_company B on A.companyID=B.id  where 1=1 and " + sql;
            int startIndex = (int.Parse(pageCurrentCount) - 1) * Config.pageSize + 1;
            int endIndex = int.Parse(pageCurrentCount) * Config.pageSize;

            DataTable dt = Config.getPageDataTable("order by T.id desc", sql1, startIndex, endIndex);
            DataTable da = DbHelperSQL.Query(sql1).Tables[0];
            if (dt.Rows.Count > 0)
            {

                double d = double.Parse((da.Rows.Count / double.Parse(Config.pageSize.ToString())).ToString());
                string ss = Math.Ceiling(d) + "@@@" + OperUtil.DataTableToJsonWithJsonNet(dt);
                Util.Debuglog("ss="+ss,"会员列表.txt");
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
           
            string sql = "1=1";
            if (this.keyword.Value.Trim() != "")
            {
                sql += " and (A.memberBH='" + this.keyword.Value.Trim() + "' or A.name='" + this.keyword.Value.Trim() + "' or A.phone='" + this.keyword.Value.Trim() + "')";
            }
            if (this.companyID.Value != "0")
            {
                sql += " and A.companyID=" + this.companyID.Value;
            }
            if (this.start.Value.Trim() != "")
            {
                sql += " and A.createDate>'" + this.start.Value.Trim() + "'";
            }
            if (this.end.Value != "")
            {
                sql += " and A.createDate<'" + this.end.Value + "'";
            }
            if (memberdj.SelectedValue != "-1")
            {
                sql += " and A.dj=" + memberdj.SelectedValue;
            }
            if (minMoney.Value != "")
            {
                sql += " and A.AvailableMoney>'" + minMoney.Value + "'";
            }
            if (maxMoney.Value != "")
            {
                sql += " and A.AvailableMoney<'" + maxMoney.Value + "'";
            }
            string sql1 = "select A.id,case A.dj when 0 then '游客' when 1 then '普通会员' when 2 then '白银会员' when '3' then '黄金会员' else '' end 'dj' ,A.hjhyDays,A.mechineName,A.name,"+
                "A.phone,A.sex,A.consumeCount,A.sumConsume,A.sumRecharge,A.AvailableMoney,A.createDate,A.LastTime,A.bh ,"+
                "B.name companyName ,(select COUNT(s.num) num from (select COUNT(*) num from  asm_sellDetail where memberID=A.id group by productID) S) buyNum "
                + "from (select am.*,ac.bh ,ac.mechineName  from asm_member am left join asm_mechine ac on am.mechineID=ac.id) A left join asm_company B on A.companyID=B.id  where 1=1 and " + sql;
            DataTable dt = DbHelperSQL.Query(sql1).Tables[0];
            ExportToSpreadsheet(dt, DateTime.Now.ToString("yyyyMMddHHmmss"));
    
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
                if (column.ColumnName == "id")
                {
                    context.Response.Write("会员ID" + ",");
                }
                if (column.ColumnName == "name")
                {
                    context.Response.Write("会员名称" + ",");
                }
                if (column.ColumnName == "dj")
                {
                    context.Response.Write("会员等级" + ",");
                }
                if (column.ColumnName == "hjhyDays")
                {
                    context.Response.Write("黄金会员体验天数" + ",");
                }
                if (column.ColumnName == "phone")
                {
                    context.Response.Write("手机号" + ",");
                }
                if (column.ColumnName == "sex")
                {
                    context.Response.Write("性别" + ",");
                }
                if (column.ColumnName == "consumeCount")
                {
                    context.Response.Write("消费次数" + ",");
                }
                if (column.ColumnName == "sumConsume")
                {
                    context.Response.Write("累计消费" + ",");
                }
                if (column.ColumnName == "sumRecharge")
                {
                    context.Response.Write("累计储值" + ",");
                }
                if (column.ColumnName == "AvailableMoney")
                {
                    context.Response.Write("可用余额" + ",");
                }
                if (column.ColumnName == "createDate")
                {
                    context.Response.Write("登记时间" + ",");
                }
                if (column.ColumnName == "LastTime")
                {
                    context.Response.Write("最后一次消费时间" + ",");
                }
                if (column.ColumnName == "mechineName")
                {
                    context.Response.Write("机器名称" + ",");
                }
                if (column.ColumnName == "buyNum")
                {
                    context.Response.Write("SKU数量" + ",");
                }
                if (column.ColumnName == "bh")
                {
                    context.Response.Write("机器编号" + ",");
                }
                if (column.ColumnName == "companyName")
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
                            {
                                string time = DateTime.Parse(row[i].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                                context.Response.Write("\"" + time + "\",");
                            }
                            else {
                                context.Response.Write("\"" + row[i].ToString().Replace("\"", "\"\"") + "\",");
                            }
                               
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