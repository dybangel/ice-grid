using autosell_center.cls;
using autosell_center.util;
using DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace autosell_center.main.equipment
{
    
    public partial class temperaturelist : System.Web.UI.Page
    {
        public string name = "";
        public string timeS = "";
        public string dataS = "";
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (!IsPostBack)
            {
                this.mechineID.Value = Request.QueryString["mechineID"].ToString();
                string sql = "select * from asm_mechine where id=" + this.mechineID.Value;
                DataTable dd = DbHelperSQL.Query(sql).Tables[0];
                if (dd.Rows.Count > 0)
                {
                    name = dd.Rows[0]["mechineName"].ToString();
                }
            }
        }
        [WebMethod]
        public static string getData(string mechineID, string time)
        {
            string sql = "select distinct am.time,at.temperature from asm_Mtime am left join asm_temperature at on am.time=SUBSTRING(at.time,11,6) where mechineID="+mechineID;
            if (!string.IsNullOrEmpty(time))
            {
                sql += " and at.time like '" + time + "%'";
            }
            else {
                sql += " and at.time like '"+DateTime.Now.ToString("yyyy-MM-dd")+"%'";
            }
            sql += " order by am.time";
            string timeStr = "", dataStr = "" ;
            DataTable da = DbHelperSQL.Query(sql).Tables[0];
           
            if (da.Rows.Count > 0)
            {
                for (int i=0;i< da.Rows.Count; i++)
                {
                    //timeStr += OperUtil.ConvertDateTimeToInt(DateTime.Parse(dt.Rows[i]["time"].ToString())) +",";
                    timeStr += ""+ da.Rows[i]["time"].ToString().Trim() + ",";
                    dataStr += da.Rows[i]["temperature"].ToString() + ",";
                }
                timeStr = timeStr.Substring(0,timeStr.Length-1);
                dataStr = dataStr.Substring(0,dataStr.Length-1);
                string ss = OperUtil.DataTableToJsonWithJsonNet(da)+"@"+timeStr+"@"+dataStr;
                return ss;
            }
            else
            {
                return "1";
            }
            
        }
    }
}