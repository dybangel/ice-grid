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
    public partial class Distributor : System.Web.UI.Page
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
        public static string getMessageList(string companyID)
        {
            string sql = "select * from asm_wxTemplate where companyID="+companyID+ " and templateBH not in('OPENTM414811026','OPENTM403148135')";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                string ss = OperUtil.DataTableToJsonWithJsonNet(dt);
                return ss;
            }
            else
            {
                return "1";
            }
        }
        [WebMethod]
        public static string save(string companyID,string bh,string id)
        {
            bh = bh.Substring(0,bh.Length-1);
            id = id.Substring(0,id.Length-1);
            string[] bhArr = bh.Split(',');
            string[] idArr = id.Split(',');
            if (bhArr.Length>0)
            {
                for (int i=0; i<bhArr.Length;i++)
                {
                    string sql = "update asm_wxTemplate set templateID='"+idArr[i]+"' where companyID="+companyID+" and templateBH='"+bhArr[i]+"'";
                    DbHelperSQL.ExecuteSql(sql);
                }
            }
            return "1";
        }
    }
}