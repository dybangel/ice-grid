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
    public partial class addvideo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void picb_Click(object sender, EventArgs e)
        {

        }
        [WebMethod]
        public static string valYZ(string name)
        {
           
            string sql = "select * from asm_video where name='"+name+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            
            if (dt.Rows.Count>0)
            {
                
                return "1";
            }
            return "2";
        }
        [WebMethod]
        public static string addRecord(string name,string des,string type)
        {
            
            if (type == "0")//横屏视频
            {
                string url = HttpContext.Current.Request.Url.Host;
                string strPath ="http://"+url+"/main/Advertisement/upload/hvideo/";
                string sql = "insert into asm_video (name,description,type,path,shType) values('" + name + "','" + des + "'," + type + ",'"+strPath+name+"',1)";
                DbHelperSQL.ExecuteSql(sql);
            }
            else if(type=="1"){//竖屏视频
                string url = HttpContext.Current.Request.Url.Host;
                string strPath ="http://"+url+"/main/Advertisement/upload/vvideo/";
                string sql = "insert into asm_video (name,description,type,path,shType) values('" + name + "','" + des + "'," + type + ",'"+strPath+name+"',1)";
                DbHelperSQL.ExecuteSql(sql);
            }
            RedisHelper.Remove("_VideoInfoList");
            return "1";
        }


    }
}