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

namespace autosell_center.main.enterprise
{
    public partial class syssetting : System.Web.UI.Page
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
           
            this.companyID.Value = comID;
            string sql = "select * from asm_company where id="+comID;
            DataTable dd = DbHelperSQL.Query(sql).Tables[0];
            if (dd.Rows.Count>0)
            {
                //this.appTime.Value = dd.Rows[0]["p2"].ToString();
                this.getTime.Value = dd.Rows[0]["p3"].ToString();
                //this.downTime.Value = dd.Rows[0]["p1"].ToString();
                
                this.jlc.Value = dd.Rows[0]["jlc"].ToString();
                
                this.syqx.Value = dd.Rows[0]["p12"].ToString();
                this.biz.Value = dd.Rows[0]["biz"].ToString();
                this.tkbs.Value = dd.Rows[0]["tkbs"].ToString();
                this.minAppid.Value= dd.Rows[0]["minAppid"].ToString();
                this.minAppSerect.Value = dd.Rows[0]["minSecret"].ToString();
                this.phone.Value= dd.Rows[0]["customerPhone"].ToString();
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
        public static string save(string companyID,string getTime,string jlc,string syqx,string biz,string tkbs,string phone)
        {
            try
            {
                DateTime.Parse(getTime);
            }
            catch {
                return "3";
            }
            string sql = "update asm_company set p3='"+getTime+"',jlc='"+jlc+"',p12='"+syqx+"',biz='"+biz+"',tkbs='"+tkbs+ "',customerPhone='"+phone+"' where id=" + companyID;
            int a= DbHelperSQL.ExecuteSql(sql);
            if (a > 0)
            {
                return "1";
            }
            else {
                return "0";
            }
        }
    }
}