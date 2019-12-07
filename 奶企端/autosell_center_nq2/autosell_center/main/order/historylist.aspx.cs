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

namespace autosell_center.main.order
{
    public partial class historylist : System.Web.UI.Page
    {
        public string comID = "";
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                comID = OperUtil.Get("companyID");
                this.companyID.Value = comID;
                this.agentID.Value = OperUtil.Get("operaID");
                if (string.IsNullOrEmpty(comID))
                {
                    Response.Write("<script>alert('您尚未登录或已长时间未进行操作，请重新登录!');top.location.href='../../index.aspx';</script>");
                    return;
                }
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
        public static object getList(string companyID,string pageCurrentCount,string start,string end ,string operaList,string type)
        {
            try
            {
                string sql1 = "";
                if (!string.IsNullOrEmpty(start))
                {
                    sql1 += " and asm_excellist.createTime>'" + start+"'";
                }
                if (!string.IsNullOrEmpty(end))
                {
                    sql1 += " and asm_excellist.createTime<'" + end+"'";
                }
                if (!string.IsNullOrEmpty(operaList))
                {
                    sql1 += " and loginID in (" + operaList+")";//当时登录人
                }
                if (type!="0")
                {
                    sql1 += " and type="+type;
                }
                string sql = "select asm_excellist.*,asm_opera.name from asm_excellist left join  asm_opera on asm_excellist.loginID=asm_opera.id where asm_excellist.companyID=" + companyID+" and 1=1 "+sql1;

                int startIndex = (int.Parse(pageCurrentCount) - 1) * Config.pageSize + 1;
                int endIndex = int.Parse(pageCurrentCount) * Config.pageSize;

                DataTable dt = Config.getPageDataTable("order by T.id desc", sql, startIndex, endIndex);
                DataTable da = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count>0)
                {
                    double d = double.Parse((da.Rows.Count / double.Parse(Config.pageSize.ToString())).ToString());
                    return new { code = 200, db = OperUtil.DataTableToJsonWithJsonNet(dt),count= Math.Ceiling(d) };
                }
                return new { code = 300, msg = "暂无记录" };
            }
            catch {
                return new { code = 500, msg = "系统异常" };
            }
        }
        [WebMethod]
        public static object getOperaList(string companyID)
        {
            try
            {
                string sql = "select * from asm_opera where companyID=" + companyID;
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return new { code = 200, db = OperUtil.DataTableToJsonWithJsonNet(dt) };
                }
                return new { code = 300, msg = "暂无记录" };
            }
            catch
            {
                return new { code = 500, msg = "系统异常" };
            }
        }
    }
}