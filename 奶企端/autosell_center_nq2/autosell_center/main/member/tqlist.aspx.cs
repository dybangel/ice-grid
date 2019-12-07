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

namespace autosell_center.main.member
{
    public partial class tqlist : System.Web.UI.Page
    {
        public string comID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            comID = OperUtil.Get("companyID");
            this._operaID.Value = OperUtil.Get("operaID");
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
        public static object saveInfo(string companyID,bool yd, bool czChk, bool membChk,string totalDay)
        {

            string sql = "select * from asm_tqlist where companyID="+companyID;
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            int ydInt = 0, czChkInt = 0, membChkInt = 0 ;
            if (yd)
            {
                ydInt = 1;
            }
            if (czChk)
            {
                czChkInt = 1;
            }
            if (membChk)
            {
                membChkInt = 1;
            }
            if (dt.Rows.Count > 0)
            {    
                string update = "update asm_tqlist set ydbuy="+ydInt+ ",czswitch="+czChkInt+",memberprice="+membChkInt + ",totalDay='"+ totalDay + "' where companyID=" + companyID;
                DbHelperSQL.ExecuteSql(update);
            }
            else {
                string insert = "insert into asm_tqlist (ydbuy,czswitch,memberprice,companyID,totalDay) values(" + ydInt+","+czChkInt+","+membChkInt+","+companyID+",'"+ totalDay + "')";
                DbHelperSQL.ExecuteSql(insert);
            }
            RedisHelper.Remove(companyID + "_memberprice");
            Util.ClearRedisMemberprice(companyID);
            return new {code=0,msg="保存成功" };
        }
        [WebMethod]
        public static object getInfo(string companyID)
        {
            string sql = "select * from asm_tqlist where companyID=" + companyID;
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count>0)
            {
                return new { code=0,yd=dt.Rows[0]["ydbuy"].ToString(), czswitch=dt.Rows[0]["czswitch"].ToString(), memberprice=dt.Rows[0]["memberprice"].ToString(),totalDay= dt.Rows[0]["totalDay"].ToString() };
            }
            return new {code=500 };
        }
        [WebMethod]
        public static object okActivity(string companyID,string payMoney,string days,string start,string end)
        {
            try
            {
                double _money=double.Parse(payMoney);
                int _days=int.Parse(days);
                if (_money <= 0 || _days <= 0)
                {
                    return new { code = 500, msg = "输入数字不正确" };
                }
                else {
                    string sql = "insert into asm_tqdetail (companyID,money,day,startTime,endTime) values('" + companyID+"',"+payMoney+","+days+",'"+start+"','"+end+"')";
                    int a= DbHelperSQL.ExecuteSql(sql);
                    if (a > 0)
                    {
                        return new { code = 0, msg = "添加成功" };
                    }
                    else {
                        return new { code=500,msg="添加失败"};
                    }
                }
            }
            catch {
                return new { code = 500, msg = "输入数字不正确" };
            }
        }
        [WebMethod]
        public static object initData(string companyID)
        {
            try
            {
                string sql = "select * from asm_tqdetail where companyID="+companyID;
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
            catch {
                return "1";
            }
        }
        [WebMethod]
        public static object delActivity(string id)
        {
            try
            {
                string sql = "delete from asm_tqdetail where id="+id;
                int a= DbHelperSQL.ExecuteSql(sql);
                if (a > 0)
                {
                    return new { code = 0, msg = "删除成功" };
                }
                else {
                    return new { code=500,msg="删除失败"};
                }
            }
            catch
            {
                return "1";
            }
        }
    }
}