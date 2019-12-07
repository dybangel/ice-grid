<%@ WebHandler Language="C#" Class="Handler" %>
using System;
using System.Web;
public class Handler : IHttpHandler {
    public void ProcessRequest (HttpContext context) {
        if (context.Request.Files.Count > 0)
        {
            string strName = context.Request.Files[0].FileName;
            //从cooki读取type
            string type = context.Request["type"];
           
            if (type == "0")
            {
                string sql = "select * from asm_video where name='"+strName+"'";
                System.Data.DataTable dt = DBUtility.DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count<=0)
                {  string strPath = System.Web.HttpContext.Current.Server.MapPath("/main/Advertisement/upload/hvideo/");
                    context.Request.Files[0].SaveAs(System.IO.Path.Combine(strPath, strName));
                }
            }
            else if(type=="1") {
                string sql = "select * from asm_video where name='"+strName+"'";
                System.Data.DataTable dt = DBUtility.DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count<=0)
                {
                    string strPath = System.Web.HttpContext.Current.Server.MapPath("/main/Advertisement/upload/vvideo/");
                    context.Request.Files[0].SaveAs(System.IO.Path.Combine(strPath, strName));
                }
            }

        }
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}