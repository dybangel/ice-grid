using autosell_center.util;
using Consumer.cls;
using DBUtility;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;

namespace autosell_center.main.Advertisement
{
    public partial class add_video : System.Web.UI.Page
    {
        private bool pageRefreshed = false; //页面是否刷新提交
        private bool refreshState = false;  //ViewState中暂存的状态
        private string comID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                comID = OperUtil.Get("companyID");
                this.company_id.Value = comID;
                if (string.IsNullOrEmpty(comID))
                {
                    Response.Write("<script>alert('您尚未登录或已长时间未进行操作，请重新登录!');top.location.href='../../index.aspx';</script>");
                    return;
                }
            }
        }
        protected override void LoadViewState(object savedState)
        {
            object[] states = (object[])savedState;
            base.LoadViewState(states[0]);
            refreshState = (bool)states[1];
            if (Session["__PAGE_REFRESHED"] == null)
                pageRefreshed = false;
            else
                pageRefreshed = refreshState != (bool)Session["__PAGE_REFRESHED"];
        }

        protected override object SaveViewState()
        {
            Session["__PAGE_REFRESHED"] = !refreshState;
            object[] states = new object[2];
            states[0] = base.SaveViewState();
            states[1] = !refreshState;
            return states;
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
           
            if (pageRefreshed)
            {
               //页面刷新效果
            }
            else
            {
               
                if (string.IsNullOrEmpty(this.description.Value.Trim()))
                {
                    Response.Write("<script type='text/javascript'>alert('视频简介不能为空');</script>");
                    return;
                }
               
                this.vv.Value = "0";
                string imgfileExp = this.FileUpload1.PostedFile.FileName.Substring(this.FileUpload1.PostedFile.FileName.LastIndexOf(".") + 1);
                if (string.IsNullOrEmpty(imgfileExp))
                {
                    Response.Write("<script type='text/javascript'>alert('请选择视频');</script>");
                    return;
                }
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss")+"."+imgfileExp;  //文件名称
                string fileExtension = Path.GetExtension(FileUpload1.PostedFile.FileName).ToLower();//文件的后缀名(小写)
                                                                                                    //string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") ;  //文件名称
                                                      //判断是否重名
                string sql = "select * from asm_video where companyID="+this.company_id.Value+" and name='"+fileName+""+"'";
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                
                if (dt.Rows.Count>0)
                {
       
                    Response.Write("<script type='text/javascript'>alert('存在同名文件无法上传');</script>");
                    return;
                }
                string path = "";
                if (videoType.SelectedValue=="0")//横屏上传图片
                {

                }
                if (imgfileExp.ToLower() == "flv" || imgfileExp.ToLower() == "mp4"|| imgfileExp.ToLower()=="jpg"|| imgfileExp.ToLower()=="png")
                {
                    if (this.videoType.SelectedValue == "1")
                    {
                        if (imgfileExp.ToLower() == "jpg" || imgfileExp.ToLower() == "png")
                        {
                            Response.Write("<script type='text/javascript'>alert('竖屏请上传视频格式');</script>");
                            return;
                        }
                        this.FileUpload1.PostedFile.SaveAs(Server.MapPath("upload") + "\\vvideo\\" + fileName+ "");
                        string url = HttpContext.Current.Request.Url.Host;
                        path = "http://" + url + "/main/Advertisement/upload/vvideo/" + fileName+ "";
                        this.vv.Value = "1";
                    }
                    else if (this.videoType.SelectedValue == "0")
                    {
                        if (imgfileExp.ToLower() == "flv" || imgfileExp.ToLower() == "mp4")
                        {
                            Response.Write("<script type='text/javascript'>alert('横屏请上传图片格式jpg，png');</script>");
                            return;
                        }
                        this.FileUpload1.PostedFile.SaveAs(Server.MapPath("upload") + "\\hvideo\\" + fileName+ "");
                        string url = HttpContext.Current.Request.Url.Host;
                        path = "http://" + url + "/main/Advertisement/upload/hvideo/" + fileName+ "";
                        this.vv.Value = "1";
                    }
                    //向数据库中添加记录
                   
                    string sql2 = "insert into asm_video(description,path,name,type,companyID,size,shType,time) values('" + this.description.Value + "','" + path + "','" + fileName+ "" + "'," + this.videoType.SelectedValue + ","+this.company_id.Value+","+this.video_size.Value+",0,'"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"')";
                    DbHelperSQL.ExecuteSql(sql2);
                    RedisHelper.Remove("_VideoInfoList");
                    this.description.Value = "";
                  
                }
                else {
                 
                    Response.Write("<script type='text/javascript'>alert('请上传flv,mp4格式的文件');</script>");
                    return;
                }
            }
            
        }
    }
}