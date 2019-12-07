using autosell_center.util;
using Consumer.cls;
using DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace autosell_center.main.Advertisement
{
    public partial class video : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                initData();
            }
        }
        public void initData()
        {
            string sql = "select * from asm_company";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            this.companyList.DataTextField = "name";
            this.companyList.DataValueField = "id";
            this.companyList.DataSource = dt;
            this.companyList.DataBind();
            this.companyList.Items.Insert(0, new ListItem("全部", "0")); //添加项


        }
        [WebMethod]
        public static object getVideoList(string videoName,string qy ,string mechineBH ,string type, string pageCurrentCount)
        {
            string sql1 = " 1=1 ";
            if (videoName.Trim()!="")
            {
                sql1 += " and (description like '%"+videoName+"%' or C.name like '%"+videoName+"%')";
            }
            if (qy!="0")
            {
                sql1 += " and companyID="+qy;
            }
            if (mechineBH.Trim()!="")
            {
                sql1 += " and bh='"+mechineBH+"'";
            }
            if (type != "-1")
            {
                sql1 += " and shType='" + type + "'";
            }
            string sql = @"select C.*,D.name companyName,D.linkman,case type when '0' then '横屏' when '1' then '竖屏' else '其他' end typeName from 
            (select A.*, B.bh from
            (select av.*, avam.mechineID, avam.videoID, avam.bz ,avam.times  from asm_video av left join asm_videoAddMechine avam on av.id = avam.videoID and avam.zt=0)A left join asm_mechine B
            on A.mechineID = B.id) C left join asm_company D on C.companyID = D.id  where 1=1 and " + sql1;

            int startIndex = (int.Parse(pageCurrentCount) - 1) * Config.pageSize + 1;
            int endIndex = int.Parse(pageCurrentCount) * Config.pageSize;

            DataTable dt = Config.getPageDataTable("order by videoID desc", sql, startIndex, endIndex);
            DataTable da = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                double d = double.Parse((da.Rows.Count / double.Parse(Config.pageSize.ToString())).ToString());
                return new { code = 200, db = OperUtil.DataTableToJsonWithJsonNet(dt), count = Math.Ceiling(d) };
            }
            return new { code = 500 };
        }
        [WebMethod]
        public static string pass(string id)
        {
            string sql = "select * from asm_video where id="+id;
            DataTable dd = DbHelperSQL.Query(sql).Tables[0];
            if (dd.Rows.Count>0)
            {
                if (dd.Rows[0]["shType"].ToString() == "1")
                {
                    return "1";
                }
                else if(dd.Rows[0]["shType"].ToString() == "0"|| dd.Rows[0]["shType"].ToString()==""|| dd.Rows[0]["shType"].ToString()==null)
                {
                    string sql1 = "update asm_video set shType=1 where id=" + id;
                    int a= DbHelperSQL.ExecuteSql(sql1);
                    if (a>0)
                    {
                        RedisHelper.Remove("_VideoInfoList");
                        return "2";
                    }
                }
            }
            return "0";
        }
        [WebMethod]
        public static string jujue(string id)
        {
            string sql = "select * from asm_video where id=" + id;
            DataTable dd = DbHelperSQL.Query(sql).Tables[0];
            if (dd.Rows.Count > 0)
            {
                if (dd.Rows[0]["shType"].ToString() == "1"|| dd.Rows[0]["shType"].ToString() == "2")
                {
                    return "1";
                }
                else if (dd.Rows[0]["shType"].ToString() == "0")
                {
                    string sql1 = "update asm_video set type=2 where id=" + id;
                    int a = DbHelperSQL.ExecuteSql(sql1);
                    if (a > 0)
                    {
                        RedisHelper.Remove("_VideoInfoList");
                        return "2";
                    }
                }
            }
            return "0";
        }
        [WebMethod]
        public static object deLete(string videoID, string path)
        {
            try
            {
                string sqlAddMechine = "select * from asm_videoAddMechine where  videoID='" + videoID + "'";
                DataTable dtlAddMechine = DbHelperSQL.Query(sqlAddMechine).Tables[0];
                if (dtlAddMechine.Rows.Count > 0)
                {
                    for (int i = 0; i < dtlAddMechine.Rows.Count; i++)
                    {
                        RedisHelper.Remove(dtlAddMechine.Rows[i]["mechineID"] + "_VideoAddMechine");

                    }

                }
                string sql = "delete asm_videoAddMechine where videoID='" + videoID + "'";
                DbHelperSQL.ExecuteSql(sql);
                string sql1 = "delete asm_video where id='" + videoID + "'";
               
                DbHelperSQL.ExecuteSql(sql1);
                RedisHelper.Remove("_VideoInfoList");
                string dir = "";//获取基目录
                if (path.Contains("http://admin.bingoseller.com/"))
                {
                    dir = "D:\\wwwroot\\zonghoutai\\wwwroot\\";
                    path = dir + path.Replace("http://admin.bingoseller.com/", "");
                }
                else if (path.Contains("http://nq.bingoseller.com/"))
                {
                    dir = "D:\\wwwroot\\naiqi\\wwwroot\\";
                    path = dir + path.Replace("http://nq.bingoseller.com/", "");
                }

                path = path.Replace("/", "\\");
                if (!Directory.Exists(path))
                {
                    File.Delete(path);
                }
                else

                {

                    string[] dirs = Directory.GetDirectories(path);

                    string[] files = Directory.GetFiles(path);


                    if (0 != dirs.Length)

                    {

                        foreach (string subDir in dirs)

                        {

                            if (null == Directory.GetFiles(subDir))

                            {
                                Directory.Delete(subDir);

                            }



                        }

                    }

                    if (0 != files.Length)
                    {

                        foreach (string file in files)

                        {

                            File.Delete(file);
                        }

                    }

                    else Directory.Delete(path);

                }


                return new { code = 200, msg = "成功" };
            }
            catch (Exception e)
            {
                return new { code = 500, msg = "系统异常" };
            }
            
          
        }

        [WebMethod]
        public static string look(string id)
        {
            string sql = "select * from asm_video where id=" + id ;
            DataTable dd = DbHelperSQL.Query(sql).Tables[0];
            if (dd.Rows.Count > 0)
            {
                return dd.Rows[0]["path"].ToString();//有未到期的视频暂时无法删除
            }
            else
            {
               
                return "2";
            }

        }
    }
}