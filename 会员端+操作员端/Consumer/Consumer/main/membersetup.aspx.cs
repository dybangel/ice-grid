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

namespace Consumer.main
{
    public partial class membersetup : System.Web.UI.Page
    {
        public string name = "";
        public string phone = "";
        public string headUrl = "";
        public string money = "0";
        public string birthday = "";
        public string sex = "未知";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (OperUtil.getCooki("vshop_openID") != "0")
            {
                this.memberID.Value = Util.getMemberID();
                initData();
            }
            else
            {
                string userAgent = Request.UserAgent;
                if (userAgent.ToLower().Contains("micromessenger"))
                {
                    Response.Redirect("WXCallback.aspx?companyID=" + OperUtil.getCooki("companyID"));
                    return;
                }

            }



        }
        public void initData()
        {
            string sql = "select * from asm_member where id=" + memberID.Value;
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                name = dt.Rows[0]["nickname"].ToString();
               
                phone = dt.Rows[0]["phone"].ToString();
                headUrl = dt.Rows[0]["headurl"].ToString();
                money = dt.Rows[0]["AvailableMoney"].ToString();
                birthday = dt.Rows[0]["birthday"].ToString();
                sex = dt.Rows[0]["sex"].ToString();
                this.demo_date.Value = birthday;
            }
        }
        [WebMethod]
        public static string setupOk(string memberID,string phone,string gen,string birthday)
        {
            string sql = "update asm_member set phone='"+phone+"' ,sex='"+gen+"',birthday='"+birthday+"' where id="+memberID;
            int a=DbHelperSQL.ExecuteSql(sql);
            if (a>0)
            {
                return "1";
            }
            return "2";
        }
        [WebMethod]
        public static string setsex(string memberID,string gen)
        {
            string sql = "update asm_member set sex='" + gen + "' where id=" + memberID;
            int a = DbHelperSQL.ExecuteSql(sql);
            if (a > 0)
            {
                string sql3 = "select * from asm_member where id='" + memberID + "'";
                DataTable dd = DbHelperSQL.Query(sql3).Tables[0];
                if (dd.Rows.Count > 0)
                {
                    if (!dd.Rows[0]["certF"].ToString().Contains("sex"))
                    {
                        string sql2 = "update asm_member set certF=isnull(certF,'')+',sex' where id='" + memberID + "'";
                        DbHelperSQL.ExecuteSql(sql2);
                    }
                }
                return "1";
            }
            return "2";
        }
        [WebMethod]
        public static string setbirthday(string memberID, string birthday)
        {
            string sql = "update asm_member set birthday='" + birthday + "' where id=" + memberID;
            int a = DbHelperSQL.ExecuteSql(sql);
            if (a > 0)
            {
                string sql3 = "select * from asm_member where id='" + memberID + "'";
                DataTable dd = DbHelperSQL.Query(sql3).Tables[0];
                if (dd.Rows.Count > 0)
                {
                    if (!dd.Rows[0]["certF"].ToString().Contains("birthday"))
                    {
                        string sql2 = "update asm_member set certF=isnull(certF,'')+',birthday' where id='" + memberID + "'";
                        DbHelperSQL.ExecuteSql(sql2);
                    }
                }
                return "1";
            }
            return "2";
        }
    }
}