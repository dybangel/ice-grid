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
    public partial class adVideoToMechine : System.Web.UI.Page
    {
        public string mechineID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            this.mechine_ID.Value = Request.QueryString["mechineID"].ToString();
        }
        //除了该机器之外的所有视频
        [WebMethod]
        public static string search(string mechineID)
        {
           
            string sql = @"select *,name + '   ' +case type when '0' then '横屏' when '1' then '竖屏' else '其他' end name from asm_video where id not in (select videoID from asm_videoAddMechine where mechineID = "+mechineID+ ")  and companyID!=0";
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
        //该机器上的视频
        [WebMethod]
        public static string search2(string mechineID)
        {
           
            string sql = "select avm.*,av.description,av.path,av.name+'   ' +case av.type when '0' then '横屏' when '1' then '竖屏' else '其他' end name,av.type from asm_videoAddMechine  avm left join asm_video av on avm.videoID=av.id where mechineID=" + mechineID;
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
        public static string okbtn(string id, string mechine_ID)
        {
            Util.ClearRedisVideoByMechineID(mechine_ID);
            if (id.Trim() != "")
            {
                string[] idArr = id.Split(',');
                //先清除asm_videoAddMechine表里的该机器的视频  然后在重新添加
                string sql3 = "delete from asm_videoAddMechine where mechineID=" + mechine_ID;
                DbHelperSQL.ExecuteSql(sql3);
               
                for (int i = 0; i < idArr.Length; i++)
                {
                    string sql1 = "select * from asm_video where id=" + idArr[i];
                    DataTable dt = DbHelperSQL.Query(sql1).Tables[0];
                    string sql = "insert into asm_videoAddMechine (mechineID,videoID,type) values(" + mechine_ID + "," + idArr[i] + "," + dt.Rows[0]["type"].ToString() + ")";
                    int a = DbHelperSQL.ExecuteSql(sql);
                    Util.ClearRedisVideoByMechineID(mechine_ID);
                }
                RedisHelper.Remove(mechine_ID + "_VideoAddMechine");
                return "1";
            }
            else {
                string sql3 = "delete from asm_videoAddMechine where mechineID=" + mechine_ID;
                DbHelperSQL.ExecuteSql(sql3);
                RedisHelper.Remove(mechine_ID + "_VideoAddMechine");
            }
            return "1";
        }
    }
}