using autosell_center.util;
using Consumer.cls;
using DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
        private string comID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            comID = OperUtil.Get("companyID");
            this.company_id.Value = comID;
            this._operaID.Value = OperUtil.Get("operaID");
            if (string.IsNullOrEmpty(comID))
            {
                Response.Write("<script>alert('您尚未登录或已长时间未进行操作，请重新登录!');top.location.href='../../index.aspx';</script>");
                return;
            }
            if (!IsPostBack)
            {
              
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
        public static object ok(string videoID,string mechineListID,string sel1,string num,string start,string startTime)
        {
            try
            {
                if (string.IsNullOrEmpty(videoID)||string.IsNullOrEmpty(mechineListID)||sel1=="0")
                {
                    return new { code = 500, msg = "参数不全" };
                }
               
                if (sel1 == "1")
                {
                    try
                    {
                        int.Parse(num);
                    }
                    catch {
                        return new { code = 500, msg = "次数设置不正确" };
                    }
                } else if (sel1=="2")
                {
                    try
                    {
                        DateTime.Parse(start);
                        DateTime.Parse(startTime);
                    }
                    catch {
                        return new { code = 500, msg = "有效期设置不正确" };
                    }
                }
                string[] arr = mechineListID.Split(',');
                if (arr.Length>0)
                {
                    string sql1 = "select * from asm_video where id="+videoID;
                    DataTable dt1 = DbHelperSQL.Query(sql1).Tables[0];
                    List<string> list = new List<string>();
                    for (int i=0;i<arr.Length;i++)
                    {
                        Util.ClearRedisVideoByMechineID(arr[i]);
                        string sql= "insert into asm_videoAddMechine(mechineID,videoID,type,tfTime,tfType,tfcs,valiDate,startTime) values("+arr[i]+","+dt1.Rows[0]["id"].ToString()+",'"+dt1.Rows[0]["type"].ToString()+"','"+ DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+sel1+"','"+num+"','"+start+ "','"+(string.IsNullOrEmpty(startTime) ?  DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"): startTime) +"')";
                        list.Add(sql);
                    }
                    int a= DbHelperSQL.ExecuteSqlTran(list);
                    if (a>0)
                    {
                        for (int i = 0; i < arr.Length; i++)
                        {
                            RedisHelper.Remove(arr[i] + "_VideoAddMechine");
                        }
                        return new { code = 200, msg = "添加成功" };
                    }
                }
                return new { code = 500, msg = "系统异常" };
            }
            catch {
                return new { code = 500, msg = "系统异常" };
            }
        }
        [WebMethod]
        public static object getMechineList(string companyID)
        {
            try
            {
                 
                string sql = "select * from asm_mechine where  mechineName is not null and companyID=" + companyID ;
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return new { code = 200, db = OperUtil.DataTableToJsonWithJsonNet(dt) };
                }
                return new { code = 200, db = OperUtil.DataTableToJsonWithJsonNet(dt) };
            }
            catch
            {
                return new { code = 500, msg = "系统异常" };
            }
        }
        [WebMethod]
        public static object deleteVideo(string videoID, string path)
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
            catch(Exception e)
            {
                return new { code = 500, msg = "系统异常" };
            }
        }
        
        [WebMethod]
        public static string getVideoList(string videoName,string companyID)
        {

            string sql = "select * from  [dbo].[asm_video] where companyID="+companyID;
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return OperUtil.DataTableToJsonWithJsonNet(dt);
            }
            else {
                return "1";
            }
        }
    }
}