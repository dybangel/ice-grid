using autosell_center.util;
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
    public partial class setActivity : System.Web.UI.Page
    {
        public string comID = "";
        public string activityID = "";
        protected void Page_Load(object sender, EventArgs e)
        {

            comID = OperUtil.Get("companyID");

            if (string.IsNullOrEmpty(comID))
            {
                Response.Write("<script>alert('您尚未登录或已长时间未进行操作，请重新登录!');top.location.href='../../index.aspx';</script>");
                return;
            }
          
            this._activityID.Value = Request.QueryString["activityID"].ToString();
            this.companyID.Value = comID;
            //获取选中的列表
            string sql = "select * from asm_activity_detail  where activityID='"+this._activityID.Value+"' and companyID='"+companyID.Value+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            
            if (dt.Rows.Count>0)
            {
                for (int i=0;i<dt.Rows.Count;i++)
                {
                    this.menu_id.Value += dt.Rows[i]["mechineID"].ToString() + "_" + dt.Rows[i]["productID"].ToString()+"-";
                }
                this.menu_id.Value = this.menu_id.Value.Substring(0,this.menu_id.Value.Length-1);
            }
        }
        [WebMethod]
        public static string qxChoose(string id,string companyID,string activityID)
        {
           
            string[] idArr = id.Split(',');
            if (idArr.Length>0)
            {
                //首先删除该企业下该活动的参加的活动
                string sqlD = "delete from asm_activity_detail where companyID='"+companyID+"' and activityID='"+activityID+"'";
                DbHelperSQL.ExecuteSql(sqlD);
                for (int i=0;i<idArr.Length-1;i++)
                {
                    string mechineID = idArr[i].Split('_')[0];
                    string productID = idArr[i].Split('_')[1];
                    string sql = "insert into asm_activity_detail(companyID,mechineID,productID,type,activityID) values('"+companyID+"','"+mechineID+"','"+productID+"','1','"+activityID+"')";
                    DbHelperSQL.ExecuteSql(sql);
                }
            }
          
            return "1";
        }
    }
}