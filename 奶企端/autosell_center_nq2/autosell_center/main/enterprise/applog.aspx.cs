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

namespace autosell_center.main.enterprise
{
    public partial class applog : System.Web.UI.Page
    {
        public string comID = "";
        public string operaID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            comID = OperUtil.Get("companyID");
            operaID = OperUtil.Get("operaID");
            this._operaID.Value = OperUtil.Get("operaID");
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
        public static object sear(string mechineID,string start,string end,string pageCurrentCount)
        {
            string sql1 = "";
            if (!string.IsNullOrEmpty(mechineID))
            {
                sql1 += " and r.mechineID in("+mechineID+")";
            }
            if (!string.IsNullOrEmpty(start))
            {
                sql1 += " and timeStr>'"+start+"'";
            }
            if (!string.IsNullOrEmpty(end))
            {
                sql1 += " and timeStr<'"+end+"'";
            }
            string sql = "select r.*,m.name,(select mechineName from asm_mechine where asm_mechine.id=r.mechineID)mechineName,(select bh from asm_mechine where asm_mechine.id=r.mechineID)bh from  asm_mechineRecord r left join asm_member m on r.memberID=m.id where 1=1 " + sql1;
            int startIndex = (int.Parse(pageCurrentCount) - 1) * Config.pageSize + 1;
            int endIndex = int.Parse(pageCurrentCount) * Config.pageSize;

            DataTable dt = Config.getPageDataTable("order by T.timeStr desc", sql, startIndex, endIndex);
            
            DataTable da = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count>0)
            {
                double d = double.Parse((da.Rows.Count / double.Parse(Config.pageSize.ToString())).ToString());
                return new { code = 200, db = OperUtil.DataTableToJsonWithJsonNet(dt), count = Math.Ceiling(d) };
            }
            return new { code=500};
        }
        [WebMethod]
        public static object getMechineList(string companyID)
        {
            try
            {

                string sql = "select * from asm_mechine where  mechineName is not null and companyID=" + companyID;
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return new { code = 200, db = OperUtil.DataTableToJsonWithJsonNet(dt) };
                }
                return new { code = 200, db = OperUtil.DataTableToJsonWithJsonNet(dt) };
            }
            catch
            {
                return new { code = 500, msg = "系统异常" };
            }
        }

        
    }
}