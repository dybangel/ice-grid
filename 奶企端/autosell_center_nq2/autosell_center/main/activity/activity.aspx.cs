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

namespace autosell_center.main.activity
{
    public partial class activity : System.Web.UI.Page
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
            this.companyId.Value = comID;
            //string sqlme = "select  id sCode,mechineName sName from  asm_mechine where companyID=" + comID;
            //DataSet dd = DbHelperSQL.Query(sqlme);
            //this.cbosDeparentment.dtDataList = dd;
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
        public static object delActivity(string id)
        {
            string sql = "update asm_activity set is_del=1 where id="+id;
            int a= DbHelperSQL.ExecuteSql(sql);
            if (a > 0)
            {
                string sql1 = "SELECT  * FROM asm_activity_detail  WHERE  activityID=" + id;
                DataTable d1 = DbHelperSQL.Query(sql1).Tables[0];
                if (d1.Rows.Count>0)
                {
                    for (int i=0;i<d1.Rows.Count;i++)
                    {
                        string productID = d1.Rows[i]["productID"].ToString();
                        string mechineID = d1.Rows[i]["mechineID"].ToString();
                        string SQL2 = "SELECT  * FROM asm_activity_detail WHERE mechineID="+mechineID+" AND productID="+productID+ " AND  activityID!="+id;
                        DataTable D2 = DbHelperSQL.Query(SQL2).Tables[0];
                        if (D2.Rows.Count<=0)
                        {
                            string SQL3 = "DELETE FROM asm_activity_detail WHERE mechineID="+mechineID+" AND productID="+productID+" AND activityID="+id;
                            DbHelperSQL.ExecuteSql(SQL3);
                        }

                    }
                }
                return new { code = 200, msg = "删除成功" };
            }
            return new { code = 500, msg = "删除失败" };
        }
        [WebMethod]
        public static object delMZActivity(string id)
        {
            string sql = "update asm_activity_fulldiscount set is_del=1 where id=" + id;
            int a = DbHelperSQL.ExecuteSql(sql);
            if (a > 0)
            {
                string sql1 = "SELECT  * FROM asm_activity_fulldiscount_detail  WHERE  activityID=" + id;
                DataTable d1 = DbHelperSQL.Query(sql1).Tables[0];
                if (d1.Rows.Count > 0)
                {
                    for (int i = 0; i < d1.Rows.Count; i++)
                    {
                        string productID = d1.Rows[i]["productID"].ToString();
                        string mechineID = d1.Rows[i]["mechineID"].ToString();
                        string SQL2 = "SELECT  * FROM asm_activity_fulldiscount_detail WHERE mechineID=" + mechineID + " AND productID=" + productID + " AND  activityID!=" + id;
                        DataTable D2 = DbHelperSQL.Query(SQL2).Tables[0];
                        if (D2.Rows.Count <= 0)
                        {
                            string SQL3 = "DELETE FROM asm_activity_fulldiscount_detail WHERE mechineID=" + mechineID + " AND productID=" + productID + " AND activityID=" + id;
                            DbHelperSQL.ExecuteSql(SQL3);
                        }

                    }
                }
                return new { code = 200, msg = "删除成功" };
            }
            return new { code = 500, msg = "删除失败" };
        }
        
        [WebMethod]
        public static string sear(string companyID)
        {
           
            string sql = "select * from asm_activity where is_del=0 and companyID="+companyID;
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
          
            if (dt.Rows.Count > 0)
            {
                return OperUtil.DataTableToJsonWithJsonNet(dt);
            }
            else {
                return "1";
            }
        }
        [WebMethod]
        public static string getFullDiscountList(string companyID)
        {

            string sql = "select * from asm_activity_fulldiscount where is_del=0 and companyID=" + companyID;
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];

            if (dt.Rows.Count > 0)
            {
                return OperUtil.DataTableToJsonWithJsonNet(dt);
            }
            else
            {
                return "1";
            }
        }
        
        [WebMethod]
        public static object okActivity(string companyID,string activityName,string activityTag,string cycle,string mode,string num,string start,string end)
        {
            string sql = "select * from asm_activity where companyID="+companyID+ " and zq="+cycle+" and is_del=0";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return new { code = 500, msg = "存在同周期活动" };
            }
            else {
                string sql1 = "select * from asm_activity where companyID="+companyID+" and is_del=0";
                DataTable dt1 = DbHelperSQL.Query(sql1).Tables[0];
                if (dt1.Rows.Count>10)
                {
                    return new { code = 500,msg="为了会员端展示效果活动不能多于10个" };
                }
                string insert = "insert into asm_activity (companyID,zqName,zq,statu,type,num,activitytag,activityname,startTime,endTime) values('" + companyID+"','"+cycle+"天','"+cycle+"',0,'"+ mode + "','"+num+"','"+activityTag+"','"+activityName+"','"+start+"','"+end+"')";
                int a= DbHelperSQL.ExecuteSql(insert);
                if (a > 0)
                {
                    string update1 = "update asm_activity set statu=0 where  startTime is not null and endTime is not null and endTime<GETDATE()";
                    string update2 = "update asm_activity set statu=1 where  startTime is not null and endTime is not null and GETDATE()>startTime and GETDATE()<endTime";
                    List<string> list = new List<string>();
                    list.Add(update1);
                    list.Add(update2);
                    DbHelperSQL.ExecuteSqlTran(list);
                    return new { code = 0, msg = "添加成功" };
                }
                else {
                    return new { code = 500, msg = "添加失败" };
                }
            }
        }
        //保存满折活动
        [WebMethod]
        public static object okMZActivity(string companyID, string activityName, string activityTag, string fullnum, string discountcontent, string mzstart, string mzend)
        {
            try
            {
                int.Parse(fullnum);

                double.Parse(discountcontent);


                string insert = "insert into asm_activity_fulldiscount (companyID,statu,fullnum,discountcontent,activitytag,activityname,startTime,endTime) values('" + companyID + "',0,'" + fullnum + "','" + discountcontent + "','" + activityTag + "','" + activityName + "','" + mzstart + "','" + mzend + "')";
                int a = DbHelperSQL.ExecuteSql(insert);
                if (a > 0)
                {
                    string update1 = "update asm_activity_fulldiscount set statu=0 where  startTime is not null and endTime is not null and endTime<GETDATE()";
                    string update2 = "update asm_activity_fulldiscount set statu=1 where  startTime is not null and endTime is not null and GETDATE()>startTime and GETDATE()<endTime";
                    List<string> list = new List<string>();
                    list.Add(update1);
                    list.Add(update2);
                    DbHelperSQL.ExecuteSqlTran(list);
                    return new { code = 0, msg = "添加成功" };
                }
                else
                {
                    return new { code = 500, msg = "添加失败" };
                }
            }
            catch
            {
                return new { code = 500, msg = "参数不正确" };
            }
        
        }
        [WebMethod]
        public static object btnUpdate(string id,string name,string tag,string zq,string mode,string disOrday,string companyID,string start,string end)
        {
           
            try
            {
                if (mode == "1")
                {
                    //折扣
                    double.Parse(disOrday);
                } else if (mode == "2")
                {
                    int.Parse(disOrday);
                }
                string sql = "select * from asm_activity where companyID="+companyID+" and zq="+zq;
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count>0)
                {
                   // return new { code=500,msg="存在相同周期活动"};
                }

                string supdateql = "update asm_activity set type=" + mode + ",num=" + disOrday + ",activitytag='"+tag+ "',activityname='"+name+"',zqName='"+zq+"天',zq='"+zq+ "',startTime='"+start+ "',endTime='"+end+"' where id=" + id + " and companyID=" + companyID;
                int a = DbHelperSQL.ExecuteSql(supdateql);
              
                if (a > 0)
                {
                    string update1 = "update asm_activity set statu=0 where  startTime is not null and endTime is not null and endTime<GETDATE()";
                    string update2 = "update asm_activity set statu=1 where  startTime is not null and endTime is not null and GETDATE()>startTime and GETDATE()<endTime";
                    List<string> list = new List<string>();
                    list.Add(update1);
                    list.Add(update2);
                    DbHelperSQL.ExecuteSqlTran(list);
                    return new { code = 0, msg = "保存成功" };
                }
                else
                {
                    return new { code = 500, msg = "保存失败" };
                }
            }
            catch {
                return new { code = 500, msg = "参数不正确" };
            }
           
        }
        //满折活动修改
        [WebMethod]
        public static object btnMZUpdate(string id, string name, string tag, string fullnum, string discountcontent, string companyID, string start, string end)
        {

            try
            {
                int.Parse(fullnum);

                double.Parse(discountcontent);
                string supdateql = "update asm_activity_fulldiscount set activitytag='" + tag + "',activityname='" + name + "',fullnum='" + fullnum + "',discountcontent='" + discountcontent + "',startTime='" + start + "',endTime='" + end + "' where id=" + id + " and companyID=" + companyID;
                int a = DbHelperSQL.ExecuteSql(supdateql);

                if (a > 0)
                {
                    string update1 = "update asm_activity_fulldiscount set statu=0 where  startTime is not null and endTime is not null and endTime<GETDATE()";
                    string update2 = "update asm_activity_fulldiscount set statu=1 where  startTime is not null and endTime is not null and GETDATE()>startTime and GETDATE()<endTime";
                    List<string> list = new List<string>();
                    list.Add(update1);
                    list.Add(update2);
                    DbHelperSQL.ExecuteSqlTran(list);
                    return new { code = 0, msg = "保存成功" };
                }
                else
                {
                    return new { code = 500, msg = "保存失败" };
                }
            }
            catch
            {
                return new { code = 500, msg = "参数不正确" };
            }

        }
        [WebMethod]
        public static string qy(string id)
        {
           
            string sql1 = "select * from asm_activity where id="+id;
            DataTable dd = DbHelperSQL.Query(sql1).Tables[0];
            if (dd.Rows.Count>0)
            {
                if (dd.Rows[0]["statu"].ToString() == "1")
                {
                    string sql = "update asm_activity set statu=0,type='2',num='0' where id=" + id;
                    DbHelperSQL.ExecuteSql(sql);
                }
                else {
                    string sql = "update asm_activity set statu=1 where id=" + id;
                    DbHelperSQL.ExecuteSql(sql);
                }
            }
            return "";
        }
        [WebMethod]
        public static string qyMZ(string id)
        {

            string sql1 = "select * from asm_activity_fulldiscount where id=" + id;
            DataTable dd = DbHelperSQL.Query(sql1).Tables[0];
            if (dd.Rows.Count > 0)
            {
                if (dd.Rows[0]["statu"].ToString() == "1")
                {
                    string sql = "update asm_activity_fulldiscount set statu=0 where id=" + id;
                    DbHelperSQL.ExecuteSql(sql);
                }
                else
                {
                    string sql = "update asm_activity_fulldiscount set statu=1 where id=" + id;
                    DbHelperSQL.ExecuteSql(sql);
                }
            }
            return "";
        }
        
        [WebMethod]
        public static string Lookup(string id)
        {
          
            string sql1 = "select * from asm_activity where id=" + id;
            DataTable dd = DbHelperSQL.Query(sql1).Tables[0];
            if (dd.Rows.Count>0)
            {
                return OperUtil.DataTableToJsonWithJsonNet(dd);
            }
            return "";
        }
        [WebMethod]
        public static string FullLookup(string id)
        {

            string sql1 = "select * from asm_activity_fulldiscount where id=" + id;
            DataTable dd = DbHelperSQL.Query(sql1).Tables[0];
            if (dd.Rows.Count > 0)
            {
                return OperUtil.DataTableToJsonWithJsonNet(dd);
            }
            return "";
        }
        
        [WebMethod]
        public static string getPayList(string companyID)
        {

            string sql = "select * from asm_pay_activity where companyID="+companyID+" order by czMoney";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return OperUtil.DataTableToJsonWithJsonNet(dt);
            }
            else
            {
                return "1";
            }
        }
        [WebMethod]
        public static object setStatus(string id)
        {
            string sql = " update asm_pay_activity set status =case when status=0 then 1  when status=1 then 0 else status  end  where id=" + id;
            int a= DbHelperSQL.ExecuteSql(sql);
            if (a>0)
            {
                return new { code = 0, msg = "设置成功" };
            }
            return new {code=500,msg="设置失败" };
        }
        [WebMethod]
        public static string okCZ(string name,string czMoney,string dzMoney,string companyID,string mechineID,string paymode,string tag,string start,string end)
        {

            string sql = "insert into asm_pay_activity (payName,czMoney,dzMoney,companyID,mechineID,type,tag,status,startTime,endTime) values('" + name+"','"+czMoney+"','"+dzMoney+"','"+ companyID + "','"+ mechineID + "',"+paymode+",'"+tag+"',0,'"+start+"','"+end+"')";
            int a=DbHelperSQL.ExecuteSql(sql);
            if (a > 0)
            {
                string update3 = "update asm_pay_activity set statu=0 where  startTime is not null and endTime is not null and endTime<GETDATE()";
                string update4 = "update asm_pay_activity set statu=1 where  startTime is not null and endTime is not null and GETDATE()>startTime and GETDATE()<endTime";
                List<string> list = new List<string>();
                list.Add(update3);
                list.Add(update4);
                DbHelperSQL.ExecuteSqlTran(list);
                return "1";
            }
            else {
                return "2";
            }
        }
        [WebMethod]
        public static string del(string id)
        {

            string sql = "delete from asm_pay_activity where id="+id;
            int a=DbHelperSQL.ExecuteSql(sql);
            if (a > 0)
            {
                return "1";
            }
            else {
                return "2";
            }
        }
        [WebMethod]
        public static object updatePayActivity(string id,string name,string tag,string payType,string czMoney,string czdzMoney,string czStart,string czEnd)
        {
            try
            {
                if (string.IsNullOrEmpty(name)||string.IsNullOrEmpty(tag)||string.IsNullOrEmpty(payType)||string.IsNullOrEmpty(czMoney)||string.IsNullOrEmpty(czdzMoney)||string.IsNullOrEmpty(czStart)||string.IsNullOrEmpty(czEnd))
                {
                    return new { code=500,msg="参数不全"};
                }
                try
                {
                    double.Parse(czMoney);
                    double.Parse(czdzMoney);
                }
                catch {
                    return new {code=500,msg="充值金额不正确" };
                }
                string sql = "update asm_pay_activity set payName='"+name+"',czMoney="+czMoney+",dzMoney="+czdzMoney+",type="+payType+",startTime='"+czStart+"',endTime='"+czEnd+"' where id="+id;
                int a= DbHelperSQL.ExecuteSql(sql);
                if (a>0)
                {
                    string update3 = "update asm_pay_activity set statu=0 where  startTime is not null and endTime is not null and endTime<GETDATE()";
                    string update4 = "update asm_pay_activity set statu=1 where  startTime is not null and endTime is not null and GETDATE()>startTime and GETDATE()<endTime";
                    List<string> list = new List<string>();
                    list.Add(update3);
                    list.Add(update4);
                    DbHelperSQL.ExecuteSqlTran(list);
                    return new { code = 200, msg = "修改完成" };
                }
                return new { code = 200, msg = "修改失败" };
            }
            catch {
                return new { code = 500, msg = "系统异常" };
            }

        }
         
    }
}