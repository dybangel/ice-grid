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
    public partial class syssetting : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string sql = "select * from asm_company";
            DataTable dd = DbHelperSQL.Query(sql).Tables[0];
            this.appTime.Value = dd.Rows[0]["p2"].ToString();
     
            this.downTime.Value = dd.Rows[0]["p1"].ToString();
        }
        [WebMethod]
        public static string save(string appTime,string downTime)
        {
            try
            {
                DateTime.Parse(appTime);
               
            }
            catch
            {
                return "3";
            }
            try
            {
                if (int.Parse(downTime) <= 0)
                {
                    return "4";
                }
            }
            catch
            {
                return "4";
            }

            string sql = "update asm_company set p1='" + downTime + "',p2='" + appTime + "'";
            int a = DbHelperSQL.ExecuteSql(sql);
            if (a > 0)
            {
                //清空机器缓存
                string sql1 = "select * from asm_mechine";
                DataTable dt = DbHelperSQL.Query(sql1).Tables[0];
                if (dt.Rows.Count>0)
                {
                    for (int i=0;i<dt.Rows.Count;i++) {
                        RedisHelper.SetRedisModel<string>(dt.Rows[i]["id"].ToString()+ "_mechineInfo",null,new TimeSpan(0,0,0));
                    }
                }
                return "1";
            }
            else
            {
                return "0";
            }
        }
    }
}