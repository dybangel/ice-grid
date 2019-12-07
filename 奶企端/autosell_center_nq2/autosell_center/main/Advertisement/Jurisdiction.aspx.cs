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

namespace autosell_center.main.Advertisement
{
    public partial class Jurisdiction : System.Web.UI.Page
    {
        private string comID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            comID = OperUtil.Get("companyID");
            this.companyId.Value = comID;
            this._operaID.Value = OperUtil.Get("operaID");
            if (string.IsNullOrEmpty(comID))
            {
                Response.Write("<script>alert('您尚未登录或已长时间未进行操作，请重新登录!');top.location.href='../../index.aspx';</script>");
                return;
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
        public static string search(string bh,string companyID)
        {
            
            string sql = " where 1=1 and  companyID="+companyID;
            if (bh != "")
            {
                sql += " and am.bh='" + bh + "'";
            }
            string sql1 = "select am.*,ac.name,amt.name mechineType,(select count(*) from asm_videoAddMechine left join asm_video asv on asm_videoAddMechine.videoID=asv.id where mechineID=am.id and asv.companyID!=0) num, case am.statu when '0' then '正常' when '1' then '脱机' when '2' then '温度异常'    else '其他' end sta from asm_mechine am left join asm_company ac on am.companyID=ac.id  left join asm_mechineType amt on am.version=amt.id " + sql;
            DataTable dt = DbHelperSQL.Query(sql1).Tables[0];
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
        public static string getVideoList(string mechineID,string companyID)
        {
            string sql = @"select *,name + '   ' +case type when '0' then '横屏' when '1' then '竖屏' else '其他' end name from asm_video where id not in (select videoID from asm_videoAddMechine where mechineID = " + mechineID + ")  and companyID="+companyID;
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
        public static string addVideo(string mechineID,string idStr)
        {
            Util.ClearRedisVideoByMechineID(mechineID);
            string[] idArr = idStr.Split(',');
            if (idArr.Length>0)
            {
                for (int i=0;i<idArr.Length;i++)
                {
                    string sql = "select * from [dbo].[asm_video] where id="+idArr[i];
                    DataTable dd = DbHelperSQL.Query(sql).Tables[0];
                    string insertSQL = @"insert into asm_videoAddMechine(mechineID,videoID,type,bz,times,tfTime,tfType,tfcs,valiDate,zt) 
                        values(" + mechineID+","+idArr[i]+",'"+dd.Rows[0]["type"].ToString()+"','',0,'"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','0',0,'',0)";
                    DbHelperSQL.ExecuteSql(insertSQL);
                    Util.ClearRedisVideoByMechineID(mechineID);
                }
            }
            RedisHelper.Remove(mechineID + "_VideoAddMechine");
            return "1";
        }
    }
}