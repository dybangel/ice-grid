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
    public partial class memberdj : System.Web.UI.Page
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
        public static object getInfo(string companyID)
        {
            string sql = "select * from asm_dj where companyID=" + companyID+" order by djID";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count>0)
            {
                return new {code=0, ptday =dt.Rows[0]["consumeDay"].ToString(), byday= dt.Rows[1]["consumeDay"].ToString(), hjday= dt.Rows[2]["consumeDay"].ToString() };
            }
            return new {code=500,msg="无数据" };
        }
        [WebMethod]
        public static object saveInfo(string ptday,string byday,string hjday,string companyID)
        {
            string sql = "select * from asm_dj where companyID="+companyID;
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                string update1 = "update asm_dj set consumeDay=" + ptday + " where companyID=" + companyID + " and djID=1";
                DbHelperSQL.ExecuteSql(update1);
                string update2 = "update asm_dj set consumeDay=" + byday + " where companyID=" + companyID + " and djID=2";
                DbHelperSQL.ExecuteSql(update2);
                string update3 = "update asm_dj set consumeDay=" + hjday + " where companyID=" + companyID + " and djID=3";
                DbHelperSQL.ExecuteSql(update3);
            }
            else {
                string insert1 = "insert into asm_dj (consumeDay,djID,companyID) values("+ ptday + ",1,"+companyID+")";
                DbHelperSQL.ExecuteSql(insert1);
                string insert2 = "insert into asm_dj (consumeDay,djID,companyID) values(" + byday + ",2," + companyID + ")";
                DbHelperSQL.ExecuteSql(insert2);
                string insert3 = "insert into asm_dj (consumeDay,djID,companyID) values(" + hjday + ",3," + companyID + ")";
                DbHelperSQL.ExecuteSql(insert3);
            }
            return new { code = 0, msg = "设置成功" };
        }
    }
}