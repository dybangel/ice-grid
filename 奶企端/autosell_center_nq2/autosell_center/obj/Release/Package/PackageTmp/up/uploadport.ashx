<%@ WebHandler Language="C#" Class="myinfo" %>

using System;
using System.Web;

public class myinfo : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{

    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        //   WebApplication1.App_Code.CheckUserLogin.checkLogin(context);

        HttpPostedFile headpicture = context.Request.Files["MyHeadPicture"];
        string hz= System.IO.Path.GetExtension(headpicture.FileName);//后缀名
        if (hz.Contains("cr"))
        {
            string filename = headpicture.FileName;
            //string filename = DateTime.Now.ToString("yyyyMMddHHmmssfffffff") + System.IO.Path.GetExtension(headpicture.FileName);//有bug的，一毫秒内多个人上传多个文件
            headpicture.SaveAs(context.Server.MapPath("~/errLog/" + filename));
            context.Response.Write("Upload Success!");
        } else if (hz.Contains("db"))
        {
            
            string filename = DateTime.Now.ToString("yyyyMMddHHmmss")+"_" +  headpicture.FileName;//有bug的，一毫秒内多个人上传多个文件
            headpicture.SaveAs(context.Server.MapPath("~/db/" + filename));
            context.Response.Write("Upload Success!");
        }


    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}