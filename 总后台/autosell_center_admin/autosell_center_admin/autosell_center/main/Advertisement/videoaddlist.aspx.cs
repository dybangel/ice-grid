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

namespace autosell_center.main.Advertisement
{
    public partial class videoaddlist : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string sql = "select * from asm_company";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            this.companyList.DataTextField = "comBH";
            this.companyList.DataValueField = "id";
            this.companyList.DataSource = dt;
            this.companyList.DataBind();
            this.companyList.Items.Insert(0, new ListItem("全部", "0")); //添加项
            this.videoID.Value = Request.QueryString["videoID"].ToString();
        }
        [WebMethod]
        public static string sear(string bh,string companyID,string province,string city,string country)
        {
            string sql1 = "";
            if (bh!="")
            {
                sql1 += " and bh='"+bh+"'";
            }
            if (companyID!="0")
            {
                sql1 += " and companyID="+companyID;
            }
            if (province!="省份")
            {
                sql1 += " and province like '%"+province+"%'";
            }
            if (city!="地级市")
            {
                sql1 += " and city like '%"+city+"%'";
            }
            if (country != "市、县级市")
            {
                sql1 += " and  country like '%"+country+"%'";
            }
            string sql = "select id,bh,mechineName,province,city,country,(select name from asm_company where asm_mechine.companyID=asm_company.id) companyName,case zt when '1' then '禁用' when '2' then '正常' when '3' then '过期' else '其他' end zt  from asm_mechine where 1=1 "+sql1;
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count>0)
            {
                return OperUtil.DataTableToJsonWithJsonNet(dt);
            }
            return "1";
        }
        [WebMethod]
        public static string add(string videoID,string mechineIDS)
        {
            string[] mechineIDArr = mechineIDS.Split(',');
            if (mechineIDArr.Length>0)
            {
                for (int i=0;i<mechineIDArr.Length;i++)
                {
                    //先判断该视频这个机器上是否存在
                    string sql = "select * from [dbo].[asm_videoAddMechine] where videoID="+videoID+" and mechineID="+mechineIDArr[i]+" and zt=0";
                    DataTable dd = DbHelperSQL.Query(sql).Tables[0];
                    if (dd.Rows.Count <= 0)
                    {
                        //查询该视频的状态
                        string sqlZT = "select * from asm_video where id="+videoID;
                        DataTable dt = DbHelperSQL.Query(sqlZT).Tables[0];
                        string insertSQL = @"insert into asm_videoAddMechine(mechineID,videoID,type,bz,times,tfTime,tfType,tfcs,valiDate,zt) 
                        values(" + mechineIDArr[i] + "," + videoID + ",'" + dt.Rows[0]["type"].ToString() + "','',0,'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','1',1000,'',0)";
                        DbHelperSQL.ExecuteSql(insertSQL);
                        RedisHelper.Remove(mechineIDArr[i] + "_VideoAddMechine");
                    }
                    else {
                        //说明该机器已经存在该视频 无需添加
                    }
                }
            }
            return "1";
        }
    }
}