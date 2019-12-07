using autosell_center.util;
using DBUtility;
using System;
using System.Data;
using System.Web;
using System.Web.Services;
using System.Web.UI.WebControls;

namespace autosell_center.main.member
{
    public partial class memberlist : System.Web.UI.Page
    {
        public string totalMember = "0";
        public string todayMember = "0";
        public string yesdayMember = "0";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                initData();
                list_SelectedIndexChanged(null, null);
            }
        }

        protected void list_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = "select * from [asm_mechine] where companyID=" + this.companyList.SelectedValue;
            DataTable da = DbHelperSQL.Query(sql).Tables[0];
            if (da.Rows.Count > 0)
            {
                this.companyList2.DataTextField = "bh";
                this.companyList2.DataValueField = "id";
                this.companyList2.DataSource = da;
                this.companyList2.DataBind();
                companyList2.Items.Insert(0, new System.Web.UI.WebControls.ListItem("全部", "0"));
            }
            else
            {
                companyList2.Items.Clear();
                companyList2.Items.Insert(0, new System.Web.UI.WebControls.ListItem("全部", "0"));

            }
        }
        public void initData()
        {
            string sql = "select * from asm_company";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            this.companyList.DataTextField = "name";
            this.companyList.DataValueField = "id";
            this.companyList.DataSource = dt;
            this.companyList.DataBind();
            this.companyList.Items.Insert(0, new ListItem("全部", "0")); //添加项

            string sqlAll = "select COUNT(*) allVal,(select count(*) yesVal from asm_member  where datediff(dd,createDate,GETDATE())=1) yesVal,(select count(*) yesVal from asm_member  where datediff(dd,createDate,GETDATE())=0) todayVal from asm_member";
            DataTable dtAll = DbHelperSQL.Query(sqlAll).Tables[0];
            totalMember = dtAll.Rows[0]["allVal"].ToString();
            todayMember = dtAll.Rows[0]["todayVal"].ToString();
            yesdayMember = dtAll.Rows[0]["yesVal"].ToString();
        }
        [WebMethod]
        public static string getMemberList(string keyword, string qy, string qy2, string start, string end, string pageCurrentCount)
        {
            string sql = "1=1";
            if (keyword.Trim() != "")
            {
                sql += " and (A.memberBH='" + keyword + "' or A.name='" + keyword + "' or A.phone='" + keyword + "')";
            }
            if (qy != "0")
            {
                sql += " and A.companyID=" + qy;
            }
            if (qy2 != "0")
            {
                sql += " and A.mechineID=" + qy2;
            }
            if (start.Trim() != "")
            {
                sql += " and A.createDate>'" + start + "'";
            }
            if (end.Trim() != "")
            {
                sql += " and A.createDate<'" + end + "'";
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
            if (this.companyList.SelectedValue != "0")
            {
                sql += " and A.companyID=" + this.companyList.SelectedValue;
            }
            if (this.companyList2.SelectedValue != "0")
            {
                sql += " and A.mechineID=" + this.companyList2.SelectedValue;
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
            string sql1 = "select A.id,case A.dj when 0 then '游客' when 1 then '普通会员' when 2 then '白银会员' when '3' then '黄金会员' else '' end 'dj' ,A.hjhyDays,A.mechineName,A.name," +
                "A.phone,A.sex,A.consumeCount,A.sumConsume,A.sumRecharge,A.AvailableMoney,A.createDate,A.LastTime,A.bh ," +
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
                            else
                            {
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