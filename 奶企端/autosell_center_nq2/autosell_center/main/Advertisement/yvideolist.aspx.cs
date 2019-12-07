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
    public partial class yvideolist : System.Web.UI.Page
    {
        public string mechineName = "";
        public string bh = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            this.mechineID.Value = Request.QueryString["mechineID"].ToString();
            string sql = "select * from asm_mechine where id="+this.mechineID.Value;
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            mechineName = dt.Rows[0]["mechineName"].ToString();
            bh = dt.Rows[0]["bh"].ToString();
        }
        [WebMethod]
        public static string sear(string mechineID)
        {
            
            string sql = "select avm.*, av.description,av.path,av.name from [dbo].[asm_videoAddMechine] avm left join asm_video av on avm.videoID=av.id where  av.companyID!=0 and avm.mechineID=" + mechineID;
            DataTable dd = DbHelperSQL.Query(sql).Tables[0];
            if (dd.Rows.Count>0)
            {
                string ss = OperUtil.DataTableToJsonWithJsonNet(dd);
                return ss;
            }
            return "1";
        }
        [WebMethod]
        public static string setOK(string mechineID,string type,string val,string id,string startTime)
        {
            //string sql = "select * from asm_videoAddMechine where videoID="+videoID+" and mechineID="+mechineID;
            //DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            Util.ClearRedisVideoByMechineID(mechineID);
            if (type == "1")
            {
                //按照次数
                string sql = "update asm_videoAddMechine set tfType=1,tfcs="+val+ ",valiDate='',startTime='"+startTime+"'  where  mechineID=" + mechineID+" and ID="+id;
                DbHelperSQL.ExecuteSql(sql);
            } else if (type=="2")
            {
                //按照时间
                string sql = "update asm_videoAddMechine set tfType=2,tfcs=0,valiDate='"+val+ "',startTime='" + startTime + "'  where  mechineID=" + mechineID + " and ID=" + id;
                DbHelperSQL.ExecuteSql(sql);
            }
            RedisHelper.Remove(mechineID + "_VideoAddMechine");
            return "";
        }
        [WebMethod]
        public static string is_open(string id, string mechineID)
        {
            string sql = "select * from asm_videoAddMechine  where id=" + id;
            Util.ClearRedisVideoByMechineID(mechineID);
            DataTable dd = DbHelperSQL.Query(sql).Tables[0];
            if (dd.Rows.Count>0)
            {
                if (dd.Rows[0]["is_open"].ToString() == "0")
                {
                    string update1 = "update asm_videoAddMechine set is_open=1 where id=" + id;
                    DbHelperSQL.ExecuteSql(update1);
                } else if (dd.Rows[0]["is_open"].ToString() == "1")
                {
                    string update1 = "update asm_videoAddMechine set is_open=0 where id=" + id;
                    DbHelperSQL.ExecuteSql(update1);
                }
                RedisHelper.Remove(mechineID + "_VideoAddMechine");
                return "1";
            }
            return "2";
        }
        [WebMethod]
        public static string del(string id,string mechineID)
        {
            Util.ClearRedisVideoByMechineID(mechineID);
            string sql = "select * from asm_videoAddMechine  where id="+id;
            DataTable dd = DbHelperSQL.Query(sql).Tables[0];
            if (dd.Rows.Count>0)
            {
                //if (dd.Rows[0]["tfType"].ToString() == "1")
                //{
                //    //按照次数
                    
                //    if (int.Parse(dd.Rows[0]["tfcs"].ToString())>int.Parse(dd.Rows[0]["times"].ToString()))
                //    {
                       
                //        return "1";
                //    }
                //} else if (dd.Rows[0]["tfType"].ToString() == "2")
                //{
                //    //按照时间
                //    if (DateTime.Parse(dd.Rows[0]["valiDate"].ToString())>DateTime.Now)
                //    {
                        
                //        return "1";
                //    }
                //}
                //执行删除
                string delSQL = "delete from  [dbo].[asm_videoAddMechine] where id="+id;
                DbHelperSQL.ExecuteSql(delSQL);
                string dl = "delete from asm_videoRecord where mechineID="+dd.Rows[0]["mechineID"].ToString()+" and videoID="+ dd.Rows[0]["videoID"].ToString() + " and time>'"+ dd.Rows[0]["tfTime"].ToString() + "'";
                DbHelperSQL.ExecuteSql(dl);
                RedisHelper.Remove(mechineID + "_VideoAddMechine");
                return "2";
            }
          
            return "0";
        }
        [WebMethod]
        public static string getStatu(string id)
        {
            
            string sql = "select * from asm_videoAddMechine where id="+id;
            DataTable dd = DbHelperSQL.Query(sql).Tables[0];
           
            if (dd.Rows[0]["zt"].ToString() == "1")
            {
                //过期
                return "1";
            }
            else {
                return "0";
            }
        }
    }
}