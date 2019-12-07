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

namespace autosell_center.main.activity
{
    public partial class activitylist : System.Web.UI.Page
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
        public static string getSear(string start,string end,string phone,string type,string companyID, string pageCurrentCount,string deltype)
        {
            string sql1 = "";
            if (!string.IsNullOrEmpty(start))
            {
                sql1 += " and partTime>='"+start+"' ";
            }
            if (!string.IsNullOrEmpty(end))
            {
                sql1 += " and partTime<='" + end + "' ";
            }
            if (!string.IsNullOrEmpty(phone))
            {
                sql1 += " and m.phone like '%" + phone + "%'";
            }
            if (type!="0")
            {
                sql1 += " and type="+type;
            }
            if (deltype!="-1")
            {
                sql1 += " and status="+deltype;
            }
            string sql = "select p.*,m.nickname,m.phone from asm_partActivity p left join asm_member m on p.memberID=m.id where p.companyID="+companyID+sql1;
            int startIndex = (int.Parse(pageCurrentCount) - 1) * Config.pageSize + 1;
            int endIndex = int.Parse(pageCurrentCount) * Config.pageSize;
            DataTable dt = Config.getPageDataTable("order by T.id desc", sql, startIndex, endIndex);
            DataTable da = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {

                double d = double.Parse((da.Rows.Count / double.Parse(Config.pageSize.ToString())).ToString());
                string ss = Math.Ceiling(d) + "@@@" + OperUtil.DataTableToJsonWithJsonNet(dt);
             
                return ss;
            }
            else
            {
                return "1";
            }
        }
        [WebMethod]
        public static object ok(string id,string bz)
        {
            try
            {
                if (string.IsNullOrEmpty(id)||string.IsNullOrEmpty(bz))
                {
                    return new { code=500,msg="参数不全"};
                }
                string sql = "update asm_partActivity set bz='"+bz+"',delTime='"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',status=1 where id="+id;
                int a = DbHelperSQL.ExecuteSql(sql);
                if (a>0)
                {
                    string sql1 = "select p.*,m.openID from asm_partActivity p left join asm_member m on p.memberID=m.id where p.id="+id;
                    DataTable d1 = DbHelperSQL.Query(sql1).Tables[0];
                    if (d1.Rows.Count>0&&!string.IsNullOrEmpty(d1.Rows[0]["openID"].ToString()))
                    {
                        try
                        {
                            //发通知
                            if (!string.IsNullOrEmpty(d1.Rows[0]["openID"].ToString()))
                            {

                                string companyID = d1.Rows[0]["companyID"].ToString();
                                string openID = d1.Rows[0]["openID"].ToString();
                                wxHelper wx = new wxHelper(companyID);
                                string data = TemplateMessage.getPrize(openID, "hPFDCcfuANnDAGaIaAjsAnDKfgFXK-Y0SYGK12iIsAM", "活动奖励通知", d1.Rows[0]["activityContent"].ToString(), d1.Rows[0]["activityName"].ToString(), "请尽快到小程序查看奖励");
                                TemplateMessage.SendTemplateMsg(wx.IsExistAccess_Token(d1.Rows[0]["companyID"].ToString()), data);
                            }
                        }
                        catch { }
                    }
                   
                    return new { code=200,msg="处理成功"};
                }
                return new { code = 500, msg = "处理失败" };
            }
            catch {
                return new { code = 200, msg = "系统异常" };
            }
        }
    }
}