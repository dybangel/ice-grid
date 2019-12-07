using autosell_center.api;
using autosell_center.cls;
using autosell_center.util;
using Consumer.cls;
using DBUtility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace autosell_center.ashx
{
    /// <summary>
    /// asm 的摘要说明
    /// </summary>
    public class asm : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string text = context.Request["action"];
            string key;
            switch (key = text)
            {
                case "login":  //奶企登录
                    this.login(context);
                    return;
                case "addCompany":  //注册的
                    this.addCompany(context);
                    return;
                case "yyzzUploads"://上传图片
                    this.yyzzUploads(context);
                    return;
                case "addEquipment"://添加设备
                    this.addEquipment(context);
                    return;
                case "saveEquiSet"://更新设备有效期和状态
                    this.saveEquiSet(context);
                    return;
                case "deleteEqui"://删除设备
                    this.deleteEqui(context);
                    return;
                case "setNormal"://设置机器状态
                    this.setNormal(context);
                    return;
                case "addLD"://设置料道
                    this.addLD(context);
                    return;
                case "getLDInfo"://获取料道信息
                    this.getLDInfo(context);
                    return;
                case "getLDInfoByMechineID"://获取料道信息
                    this.getLDInfoByMechineID(context);
                    return;
                case "getMchineType"://获取机器类型
                    this.getMchineType(context);
                    return;
                case "addMechineType"://添加机器类型
                    this.addMechineType(context);
                    return;
                case "updateMechineType"://修改机器类型
                    this.updateMechineType(context);
                    return;
                case "updateLdInfo"://更新料道信息
                    this.updateLdInfo(context);
                    return;
                case "uploadFile"://更新料道信息
                    this.uploadFile(context);
                    return;
                case "setDG"://设置料道为订购类型
                    this.setDG(context);
                    return;
                case "setLS"://设置料道为零售类型
                    this.setLS(context);
                    return;
                case "setQK"://设置料道为未设置状态
                    this.setQK(context);
                    return;
                case "uploadProduct"://上传商品图片
                    this.uploadProduct(context);
                    return;
                case "updateProduct"://上传商品图片
                    this.updateProduct(context);
                    return;
                case "valProduct"://验证商品是否重名
                    this.valProduct(context);
                    return;
                case "exportExcel"://导出excel
                    this.exportExcel(context);
                    return;
                case "qx_judge"://判断权限登录
                    this.qx_judge(context);
                    return;
                case "lbt1"://上传轮播图
                    this.lbt1(context);
                    return;
                case "lbt2"://上传轮播图
                    this.lbt2(context);
                    return;
                case "lbt3"://上传轮播图
                    this.lbt3(context);
                    return;
                case "addHB"://上传支付宝红包
                    this.addHB(context);
                    return;
                case "update"://
                    this.update(context);
                    return;
            }
        }
        public void update(HttpContext context)
        {
            //string sql = "select * from asm_mechine where 1=1";
            //DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            //for (int i=0;i<dt.Rows.Count;i++)
            //{
            //    string mechineID = dt.Rows[i]["id"].ToString();
            //    string sql1 = "SELECT * FROM asm_kcDetail k WHERE(id IN(SELECT MAX([id])   FROM asm_kcDetail  where  mechineID = "+mechineID+" group by productID))";
            //    DataTable d1 = DbHelperSQL.Query(sql1).Tables[0];
            //    if (d1.Rows.Count>0)
            //    {
            //        for (int j=0;j<d1.Rows.Count;j++)
            //        {
            //            string productID = d1.Rows[j]["productID"].ToString();
            //            string sql2 = "select * from asm_ldinfo where mechineID=" + mechineID + " and productID=" + productID;
            //            DataTable d2 = DbHelperSQL.Query(sql2).Tables[0];
            //            if (d2.Rows.Count <= 0)
            //            {
            //                string companyID = d1.Rows[j]["companyID"].ToString();
            //                string insert = "insert into asm_kcDetail (companyID,mechineID,productID,dateTime,dgNum,lsNum,totalLsNum,imbalance,totalDgNum) " +
            //                       "values(" + companyID + "," + mechineID + "," + productID + ",'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',0,0,0,0,0)";
            //                DbHelperSQL.ExecuteSql(insert);
            //            }
            //        }
            //    }
            //}
            //context.Response.Write("执行完成");

            //context.Response.Write("===== 同步调用，阻塞当前线程  =====");
            //AsyEventClass ac = new AsyEventClass();
            //AsyEventClass.AsyncEventHandler asy = new AsyEventClass.AsyncEventHandler(ac.ToDealData);
            //string result = asy.Invoke(1, 2);
            //context.Response.Write("result="+result);

            //context.Response.Write("=====异步调用，不会阻塞 ，但EndInvoke会因等待结果而阻塞====");
            //AsyEventClass ac = new AsyEventClass();
            //AsyEventClass.AsyncEventHandler asy = new AsyEventClass.AsyncEventHandler(ac.ToDealData);
            //IAsyncResult result = asy.BeginInvoke(1, 2, null, null);
            //context.Response.Write("此处不受影响，继续执行");
            //context.Response.Write("result=" + asy.EndInvoke(result));// 如果异步处理时间过长，此处会阻塞线程，知道等到结果

            //for (int i=0;i<10000;i++)
            //{
            //    context.Response.Write("=====异步回调，不会阻塞，自动回调 ====");
            //    AsyEventClass ac = new AsyEventClass();
            //    AsyEventClass.AsyncEventHandler asy = new AsyEventClass.AsyncEventHandler(ac.ToDealData);
            //    IAsyncResult result = asy.BeginInvoke(1, 2, new AsyncCallback(ac.Callback), asy);//处理完自动回调callback方法，不会造成线程阻塞
            //    context.Response.Write("此处不受影响，继续执行");
            //}

            //string sql = "select A.time1,"
            //      +"  cast(round(isnull(A.lsMoney, 0) / 100.0, 2) as decimal(18, 2)) lsMoney,  "
            //      +"  cast(round(isnull(A.dgMoney, 0) / 100.0, 2) as decimal(18, 2)) dgMoney,  "
            //      +"  cast(round(isnull(A.czMoney, 0) / 100.0, 2) as decimal(18, 2)) czMoney,  "
            //      +"  cast(round(isnull(A.tkMoney, 0) / 100.0, 2) as decimal(18, 2)) tkMoney,  "
            //      +"  cast(round(isnull(A.lsMoney, 0) / 100.0, 2) + round(isnull(A.dgMoney, 0) / 100.0, 2) + round(isnull(A.czMoney, 0) / 100.0, 2) as decimal(18, 2)) totalMoney"
            //      +"  from(select time1,"
            //      +"  (select SUM(ISNULL(trxamt, 0)) from asm_pay_info where payType in(1, 2, 4) and statu = 1 and type = 2  and paytime is not null and SUBSTRING(paytime,0, 9)= t.time1  and 1 = 1  and paytime<= '20190525084350' and appid = '00097421' group by SUBSTRING(paytime, 0, 9)) lsMoney,  "
            //      + "  (select SUM(ISNULL(trxamt, 0)) from asm_pay_info where payType in(3) and statu = 1 and type = 3 and paytime is not null and SUBSTRING(paytime,0, 9)= t.time1  and 1 = 1  and paytime<= '20190525084350' and appid = '00097421' group by SUBSTRING(paytime, 0, 9)) dgMoney,  "
            //      + "  (select SUM(ISNULL(trxamt, 0)) from asm_pay_info where type in(1) and statu = 1 and payType in(3,5) and paytime is not null and SUBSTRING(paytime,0, 9)= t.time1  and 1 = 1  and paytime<= '20190525084350' and appid = '00097421' group by SUBSTRING(paytime, 0, 9)) czMoney,"
            //      + "  (select SUM(ISNULL(trxamt, 0)) from asm_pay_info where type in(2) and statu = 2  and paytime is not null and SUBSTRING(paytime,0, 9)= t.time1  and 1 = 1  and paytime<= '20190525084350' and appid = '00097421' group by SUBSTRING(paytime, 0, 9)) tkMoney"
            //      + "         from asm_time t where 1 = 1  and 1 = 1  and time<= '2019-05-25 08:43:50') A ";
            //DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            //if (dt.Rows.Count > 0)
            //{
            //    List<string> list = new List<string>();
            //    for (int i = 0; i < dt.Rows.Count; i++)
            //    {
            //        string time = dt.Rows[i]["time1"].ToString();
            //        string lsMoney = dt.Rows[i]["lsMoney"].ToString();
            //        string dgMoney = dt.Rows[i]["dgMoney"].ToString();
            //        string czMoney = dt.Rows[i]["czMoney"].ToString();
            //        string tkMoney = dt.Rows[i]["tkMoney"].ToString();
            //        string totalMoney = dt.Rows[i]["totalMoney"].ToString();
            //        string insert = "insert into asm_inComeTJ(companyID,time,lsMoney,dgMoney,czMoney,tkMoney,totalMoney) " +
            //            "values(21,'" + time + "'," + lsMoney + "," + dgMoney + "," + czMoney + "," + tkMoney + "," + totalMoney + ")";
            //        list.Add(insert);
            //    }
            //    int a = DbHelperSQL.ExecuteSqlTran(list);
            //    if (a > 0)
            //    {
            //        context.Response.Write("完成");
            //    }
            //    else {
            //        context.Response.Write("失败");
            //    }

            //}

            try
            {
                string company = Util.getCompany("14");
                 
                JArray arry = (JArray)JsonConvert.DeserializeObject(company);
                wxHelper wx = new wxHelper("14");
                string data = TemplateMessage.getDJChange("o1_mf1eslvJx6E2V3EV6GwjBaOiY", OperUtil.getMessageID("14", "OPENTM406811407"),
                    "尊敬的会员，您的会员等级发生变更", "3", "1", "如有疑问，请拨打会员服务热线" + arry[0]["customerPhone"]);
                TemplateMessage.SendTemplateMsg(wx.IsExistAccess_Token("14"), data);
            }
            catch
            {

            }

        }
        public string getChildren(int i)
        {
            Dictionary<string, string> clild = new Dictionary<string, string>();
            if (i == 2)
            {
               
                clild.Add("name", "cldNmae");
                clild.Add("id", "cliID");
                clild.Add("title", "clititle");
                clild.Add("children", getChildren(1));
            }
            else {
                
                clild.Add("name", "cldNmae");
                clild.Add("id", "cliID");
                clild.Add("title", "clititle");
                //clild.Add("children", getChildren(0));
            }
          
            return Util.ToJSON(clild);
        }
        public void qx_judge(HttpContext context)
        {
           
            string menu = context.Request["menu"].ToString();
            string operaID = OperUtil.Get("operaID");//为0说明是管理员登录 
            if (operaID == "0")
            {
                //允许查看登录
                System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                stringBuilder.Append("{");
                stringBuilder.Append("\"result\":\"ok\""); //完成
                stringBuilder.Append("}");
                context.Response.Write(stringBuilder.ToString());
            }
            else {
                string sql = "select * from asm_opera where id='"+operaID+"'";
                DataTable dd = DbHelperSQL.Query(sql).Tables[0];
                if (dd.Rows.Count > 0)
                {
                    string sql2 = "select * from asm_qx where roleID="+dd.Rows[0]["qx"].ToString()+" and menuID='"+menu+"' ";
                    DataTable dt = DbHelperSQL.Query(sql2).Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["flag"].ToString() == "0")
                        {
                            //不允许登录
                            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                            stringBuilder.Append("{");
                            stringBuilder.Append("\"result\":\"notLogin\""); //完成
                            stringBuilder.Append("}");
                            context.Response.Write(stringBuilder.ToString());
                        } else if (dt.Rows[0]["flag"].ToString() == "1")
                        {
                            //允许登录
                            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                            stringBuilder.Append("{");
                            stringBuilder.Append("\"result\":\"ok\""); //完成
                            stringBuilder.Append("}");
                            context.Response.Write(stringBuilder.ToString());
                        }
                    }
                    else {
                        //请联系管理员给当前登录角色赋值权限
                        System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                        stringBuilder.Append("{");
                        stringBuilder.Append("\"result\":\"1\""); //完成
                        stringBuilder.Append("}");
                        context.Response.Write(stringBuilder.ToString());
                    }
                }
                else {
                    //跳转重新登录
                    System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                    stringBuilder.Append("{");
                    stringBuilder.Append("\"result\":\"2\""); //完成
                    stringBuilder.Append("}");
                    context.Response.Write(stringBuilder.ToString());
                }
            }

        }
        public void lbt1(HttpContext context)
        {
            HttpFileCollection files = context.Request.Files;
            string path = "";//照片路径  
            bool errorflag = true;
            string tishi = "";
            string companyID= context.Request["comID"].ToString();
            if (files.Count > 0)//Form中获取文件对象  
            {
                HttpPostedFile file = files[0];
                if (file.ContentLength > 0)//文件大小大于零  
                {
                    string fileName = file.FileName;//文件名  
                    int fileSize = file.ContentLength;//文件大小  
                    if (fileName.Substring(fileName.Length - 4, 4).ToLower() == ".jpg")//只支持.jpg文件上传  
                    {
                        try
                        {
                            Bitmap bitmap = new Bitmap(file.InputStream);//获取图片文件  
                        }
                        catch (Exception e)
                        {
                            
                            errorflag = false;
                            tishi = "照片错误，上传文件非图像文件！";
                        }
                    }
                    else
                    {
                        errorflag = false;
                        tishi = "照片格式错误，请上传JPG格式照片文件！";
                    }
                }
            }
            else
            {
                errorflag = false;
                tishi = "照片错误，上传照片文件为0字节！";
            }
            if (errorflag)
            {
                try
                {
                    string extension = ".jpg";
                    dateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                    path = "../lbt/lbt1"+ extension;//指定保存路径以及文件名，也就是完整相对路径  
                    string httpImgUrl = "http://" + HttpContext.Current.Request.Url.Host + path.Replace("..", "");

                    files[0].SaveAs(System.Web.HttpContext.Current.Server.MapPath(path));//保存文件（将相对路径转化为绝对路径） 
                    string update1 = "update asm_company set p8='"+httpImgUrl+"',p5='"+path+"' where id='"+companyID+"'";
                    DbHelperSQL.ExecuteSql(update1); 
                    tishi = "上传成功";
                    context.Response.Write("ERR" + tishi);
                }
                catch (Exception e)
                {
                    
                }

            }
            else
            {
                context.Response.Write("ERR" + tishi);
            }
        }
        public void lbt2(HttpContext context)
        {
            HttpFileCollection files = context.Request.Files;
            string path = "";//照片路径  
            bool errorflag = true;
            string companyID = context.Request["comID"].ToString();
            string tishi = "";
            if (files.Count > 0)//Form中获取文件对象  
            {
                HttpPostedFile file = files[0];
                if (file.ContentLength > 0)//文件大小大于零  
                {
                    string fileName = file.FileName;//文件名  
                    int fileSize = file.ContentLength;//文件大小  
                    if (fileName.Substring(fileName.Length - 4, 4).ToLower() == ".jpg")//只支持.jpg文件上传  
                    {
                        try
                        {
                            Bitmap bitmap = new Bitmap(file.InputStream);//获取图片文件  
                        }
                        catch (Exception e)
                        {

                            errorflag = false;
                            tishi = "照片错误，上传文件非图像文件！";
                        }
                    }
                    else
                    {
                        errorflag = false;
                        tishi = "照片格式错误，请上传JPG格式照片文件！";
                    }
                }
            }
            else
            {
                errorflag = false;
                tishi = "照片错误，上传照片文件为0字节！";
            }
            if (errorflag)
            {
                try
                {
                    string extension = ".jpg";
                    dateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                    path = "../lbt/lbt2" + extension;//指定保存路径以及文件名，也就是完整相对路径  
                    string httpImgUrl = "http://" + HttpContext.Current.Request.Url.Host + path.Replace("..", "");

                    files[0].SaveAs(System.Web.HttpContext.Current.Server.MapPath(path));//保存文件（将相对路径转化为绝对路径）  
                    tishi = "上传成功";
                    string update1 = "update asm_company set p9='" + httpImgUrl + "',p6='"+path+"' where id='" + companyID + "'";
                    DbHelperSQL.ExecuteSql(update1);
                    context.Response.Write("ERR" + tishi);
                }
                catch (Exception e)
                {

                }

            }
            else
            {
                context.Response.Write("ERR" + tishi);
            }
        }
        /// <summary>
        /// 上传支付宝红包
        /// </summary>
        /// <param name="context"></param>
        public void addHB(HttpContext context)
        {
            HttpFileCollection files = context.Request.Files;
            string path = "";//照片路径  
        
            string companyID = context.Request["comID"].ToString();
            string des = context.Request["des"].ToString();
            string url = context.Request["url"].ToString();
            string type = context.Request["type"].ToString();
            string start = context.Request["start"].ToString();
            string end = context.Request["end"].ToString();
            if (files.Count > 0)//Form中获取文件对象  
            {
                HttpPostedFile file = files[0];
                if (file.ContentLength > 0)//文件大小大于零  
                {
                    string fileName = file.FileName;//文件名  
                    int fileSize = file.ContentLength;//文件大小  
                    if (fileName.Substring(fileName.Length - 4, 4).ToLower() == ".jpg")//只支持.jpg文件上传  
                    {
                        try
                        {
                            string extension = ".jpg";
                            dateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                            path = "../hblist/" + dateTime + extension;//指定保存路径以及文件名，也就是完整相对路径  

                            string httpImgUrl = "http://" + HttpContext.Current.Request.Url.Host + path.Replace("..", "");
                            files[0].SaveAs(System.Web.HttpContext.Current.Server.MapPath(path));//保存文件（将相对路径转化为绝对路径）  
                        
                            string sql = "insert into asm_zfbhb (path,des,companyID,name,url,type,startTime,endTime) values('" + httpImgUrl + "','" + des + "','" + companyID + "','" + dateTime + extension + "','" + url + "','" + type + "','" + start + "','" + end + "')";
                            DbHelperSQL.ExecuteSql(sql);
                            context.Response.Write("{\"code\":\"200\",\"msg\":\"上传成功\"}");
                            return;
                        }
                        catch (Exception e)
                        {
                            context.Response.Write("{\"code\":\"500\",\"msg\":\"图片上传失败\"}");
                            return;
                        }
                    }
                    else
                    {
                       
                        context.Response.Write("{\"code\":\"500\",\"msg\":\"照片格式错误，请上传JPG格式照片文件\"}");
                        return;
                    }
                }
            }
            else
            {
               
                context.Response.Write("{\"code\":\"500\",\"msg\":\"照片错误，上传照片文件为0字节\"}");
                return;
            }
            
        }
        public void lbt3(HttpContext context)
        {
            HttpFileCollection files = context.Request.Files;
            string path = "";//照片路径  
            bool errorflag = true;
            string tishi = "";
            string companyID = context.Request["comID"].ToString();
            if (files.Count > 0)//Form中获取文件对象  
            {
                HttpPostedFile file = files[0];
                if (file.ContentLength > 0)//文件大小大于零  
                {
                    string fileName = file.FileName;//文件名  
                    int fileSize = file.ContentLength;//文件大小  
                    if (fileName.Substring(fileName.Length - 4, 4).ToLower() == ".jpg")//只支持.jpg文件上传  
                    {
                        try
                        {
                            Bitmap bitmap = new Bitmap(file.InputStream);//获取图片文件  
                        }
                        catch (Exception e)
                        {

                            errorflag = false;
                            tishi = "照片错误，上传文件非图像文件！";
                        }
                    }
                    else
                    {
                        errorflag = false;
                        tishi = "照片格式错误，请上传JPG格式照片文件！";
                    }
                }
            }
            else
            {
                errorflag = false;
                tishi = "照片错误，上传照片文件为0字节！";
            }
            if (errorflag)
            {
                try
                {
                    string extension = ".jpg";
                    dateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                    path = "../lbt/lbt3" + extension;//指定保存路径以及文件名，也就是完整相对路径  

                    string httpImgUrl = "http://" + HttpContext.Current.Request.Url.Host + path.Replace("..", "");
                    files[0].SaveAs(System.Web.HttpContext.Current.Server.MapPath(path));//保存文件（将相对路径转化为绝对路径）  
                    tishi = "上传成功";
                   
                    string update1 = "update asm_company set p10='" + httpImgUrl + "',p7='"+path+"' where id='" + companyID + "'";
                    DbHelperSQL.ExecuteSql(update1);
                   
                    context.Response.Write("ERR" + tishi);
                }
                catch (Exception e)
                {

                }

            }
            else
            {
                context.Response.Write("ERR" + tishi);
            }
        }
        public void uploadProduct(HttpContext context)
        {
            HttpFileCollection files = context.Request.Files;
            string path = "";//照片路径  
           
            if (files.Count > 0)//Form中获取文件对象  
            {
                HttpPostedFile file = files[0];
                if (file.ContentLength > 0)//文件大小大于零  
                {
                    string fileName = file.FileName;//文件名  
                    int fileSize = file.ContentLength;//文件大小  
                    if (fileName.Substring(fileName.Length - 4, 4).ToLower() == ".jpg")//只支持.jpg文件上传  
                    {
                        try
                        {
                            string extension = ".jpg";
                            dateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                            path = "../img/" + dateTime + extension;//指定保存路径以及文件名，也就是完整相对路径  
                            string result = addProduct(context, path);
                            files[0].SaveAs(System.Web.HttpContext.Current.Server.MapPath(path));//保存文件（将相对路径转化为绝对路径）
                            context.Response.Write(result);
                            return;
                        }
                        catch (Exception e)
                        {
                            context.Response.Write("{\"code\":\"500\",\"msg\":\"图片上传失败\"}");
                            return;
                        }
                    }
                    else
                    {
                        context.Response.Write("{\"code\":\"500\",\"msg\":\"照片格式错误，请上传JPG格式照片文件\"}");
                        return;
                        
                    }
                }
            }
            else
            {
                context.Response.Write("{\"code\":\"500\",\"msg\":\"照片错误，上传照片文件为0字节\"}");
                return;
               
            }
            
        }
        public void updateProduct(HttpContext context)
        {
            HttpFileCollection files = context.Request.Files;
            string path = "";//照片路径  
            if (files.Count > 0)//Form中获取文件对象  
            {
                HttpPostedFile file = files[0];
                if (file.ContentLength > 0)//文件大小大于零  
                {
                    string fileName = file.FileName;//文件名  
                    int fileSize = file.ContentLength;//文件大小  
                    if (fileName.Substring(fileName.Length - 4, 4).ToLower() == ".jpg")//只支持.jpg文件上传  
                    {
                        try
                        {
                            string extension = ".jpg";
                            dateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                            path = "../img/" + dateTime + extension;//指定保存路径以及文件名，也就是完整相对路径  
                            string result = upProduct(context, path);
                            files[0].SaveAs(System.Web.HttpContext.Current.Server.MapPath(path));//保存文件（将相对路径转化为绝对路径）  

                            context.Response.Write(result);
                            return;
                        }
                        catch (Exception e)
                        {
                            context.Response.Write("{\"code\":\"500\",\"msg\":\"图片上传失败\"}");
                            return;
                        }
                    }
                    else
                    {
                        context.Response.Write("{\"code\":\"500\",\"msg\":\"照片格式错误，请上传JPG格式照片文件！\"}");
                        return;
                    }
                }
            }
            else
            {
                string result = upProduct(context, "");
                
                context.Response.Write(result);
                return;
            }
            
        }
        public string upProduct(HttpContext context, string path)
        {

            try
            {
                string name = context.Request["name"].ToString();
                string lb = context.Request["lb"].ToString();
                string des = context.Request["des"].ToString();
                string ls_price = context.Request["ls_price"].ToString();
                string pt_price = context.Request["pt_price"].ToString();
                string by_price = context.Request["by_price"].ToString();
                string hj_price = context.Request["hj_price"].ToString();
                string companyID = context.Request["companyID"].ToString();
                string bzq = context.Request["bzq"].ToString();
                string product_id = context.Request["product_id"].ToString();
                string zt = context.Request["zt"].ToString();
                string progg = context.Request["progg"].ToString();
                string brandID = context.Request["brandID"].ToString();
                string shortname = context.Request["shortname"].ToString();
                string bh = context.Request["bh"].ToString();
                string tag = context.Request["tag"].ToString();
                string type = context.Request["type"].ToString();
                string startSend = context.Request["startSend"].ToString();
                string weight = context.Request["weight"].ToString();
                string sql1 = "select * from asm_product  where (proName='" + name + "' or bh='" + bh + "' or shortName='" + shortname + "') and productID!=" + product_id + " and   companyID=" + companyID;
                DataTable dt = DbHelperSQL.Query(sql1).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return "{\"code\":\"500\",\"msg\":\"商品名、条码或简称重复\"}";
                }
                //验证该企业是否有重名的商品
                //需要清空redis
                RedisHelper.Remove(companyID + "_ProductListSet");
                Util.ClearRedisProductInfo();
                RedisHelper.SetRedisModel<string>(companyID + "_productList", null, new TimeSpan(0, 0, 0));
                if (path == "")
                {
                    if (!string.IsNullOrEmpty(pt_price))
                    {
                        try
                        {
                            double.Parse(pt_price);
                            string sqlp = "update asm_product set price1=" + double.Parse(pt_price).ToString("f2") + " where productID=" + product_id;
                            DbHelperSQL.ExecuteSql(sqlp);
                        }
                        catch
                        {
                            return "{\"code\":\"500\",\"msg\":\"普通价格设置不正确\"}";
                        }
                    }
                    else {
                        string sqlp = "update asm_product set price1='0' where productID=" + product_id;
                        DbHelperSQL.ExecuteSql(sqlp);
                    }
                    if (!string.IsNullOrEmpty(by_price))
                    {
                        try
                        {
                            double.Parse(by_price);
                            string sqlp = "update asm_product set price2=" + double.Parse(by_price).ToString("f2") + " where productID=" + product_id;
                            DbHelperSQL.ExecuteSql(sqlp);
                        }
                        catch
                        {
                            return "{\"code\":\"500\",\"msg\":\"白银价格设置不正确\"}";
                        }
                    }
                    else {
                        string sqlp = "update asm_product set price2='0' where productID=" + product_id;
                        DbHelperSQL.ExecuteSql(sqlp);
                    }
                    if (!string.IsNullOrEmpty(hj_price))
                    {
                        try
                        {
                            double.Parse(hj_price);
                            string sqlp = "update asm_product set price3=" + double.Parse(hj_price).ToString("f2") + " where productID=" + product_id;
                            DbHelperSQL.ExecuteSql(sqlp);
                        }
                        catch
                        {
                            return "{\"code\":\"500\",\"msg\":\"黄金价格设置不正确\"}";
                        }
                    }
                    else {
                        string sqlp = "update asm_product set price3='0' where productID=" + product_id;
                        DbHelperSQL.ExecuteSql(sqlp);
                    }
                    string sql = "update asm_product set weight='"+weight+"',brandID='" + brandID + "',proName='" + name + "',progg='" + progg + "', protype='" + lb + "',description='" + des + "',price0='" + ls_price + "', bzq='" + bzq + "',sluid='" + zt + "',bh='" + bh + "',shortName='" + shortname + "',tag='" + tag + "',dstype='" + type + "',startSend='" + startSend + "' where productID=" + product_id;
                    int a = DbHelperSQL.ExecuteSql(sql);
                    if (a > 0)
                    {
                        return "{\"code\":\"200\",\"msg\":\"修改成功\"}";
                    }
                    else
                    {
                        return "{\"code\":\"500\",\"msg\":\"修改失败\"}";
                    }
                }
                else
                {
                    string httpImgUrl = "http://"+HttpContext.Current.Request.Url.Host + path.Replace("..", "");

                    string sql = "update asm_product set proName='" + name + "',protype='" + lb + "',description='" + des + "',price1='" + pt_price + "',price2='" + by_price + "',price3='" + hj_price + "',bzq='" + bzq + "',path='" + path.Replace("..", "") + "',sluid='" + zt + "',httpImageUrl='" + httpImgUrl + "',bh='" + bh + "',shortName='" + shortname + "',tag='" + tag + "',dstype='" + type + "',startSend='" + startSend + "'  where productID=" + product_id;
                    int a = DbHelperSQL.ExecuteSql(sql);

                    if (a > 0)
                    {
                        return "{\"code\":\"200\",\"msg\":\"修改成功\"}";
                    }
                    else
                    {
                        return "{\"code\":\"500\",\"msg\":\"修改失败\"}";
                    }
                }
            }
            catch {
                return "{\"code\":\"500\",\"msg\":\"系统异常\"}";
            }
            
        }
        public string addProduct(HttpContext context, string path)
        {

            try
            {
                string name = context.Request["name"].ToString();
                string lb = context.Request["lb"].ToString();
                string des = context.Request["des"].ToString();
                string ls_price = context.Request["ls_price"].ToString();
                string pt_price = context.Request["pt_price"].ToString();
                string by_price = context.Request["by_price"].ToString();
                string hj_price = context.Request["hj_price"].ToString();
                string companyID = context.Request["companyID"].ToString();
                string bzq = context.Request["bzq"].ToString();
                string zt = context.Request["zt_gyt"].ToString();
                string progg = context.Request["progg"].ToString();
                string brandID = context.Request["brandID"].ToString();
                string shortname = context.Request["shortname"].ToString();
                string bh = context.Request["bh"].ToString();
                string tag = context.Request["tag"].ToString();
                string type = context.Request["type"].ToString();
                string weight = context.Request["weight"].ToString();
                string startSend = context.Request["startSend"].ToString();//首送日期
                                                                           //验证该企业是否有重名的商品
                string sql = "select * from asm_product  where (proName='" + name + "' or bh='" + bh + "' or shortName='" + shortname + "')  and  companyID=" + companyID;

                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {

                    return "{\"code\":\"500\",\"msg\":\"商品名、商品条码或简称重复\"}";
                }
                else
                {
                    Util.ClearRedisProductInfo();//清空redis
                    RedisHelper.SetRedisModel<string>(companyID + "_productList", null, new TimeSpan(0, 0, 0));
                    RedisHelper.Remove(companyID + "_ProductListSet");
                    string httpImgUrl = "http://" + HttpContext.Current.Request.Url.Host + path.Replace("..", "");
                    int a = 0;
                    string sql1 = "";
                    sql1 = "insert into asm_product(proName,price0,price1,price2,price3,path,protype,description,companyID,bzq,httpImageUrl,sluid,progg,brandID,shortName,bh,tag,dstype,startSend,weight) "
                        + "values('@proName','@price0','@price1','@price2','@price3','@path',@protype,'@description',@companyID,@bzq,'@httpImageUrl','@sluid','@progg','@brandID','@shortName','@bh','@tag','@dstype','@startSend','"+ weight + "')";
                    sql1 = sql1.Replace("@proName",name);
                    sql1 = sql1.Replace("@price0", double.Parse(ls_price).ToString("f2"));
                    if (!string.IsNullOrEmpty(pt_price))
                    {
                        try
                        {
                            double.Parse(pt_price);
                            sql1 = sql1.Replace("@price1", double.Parse(pt_price).ToString("f2"));
                        }
                        catch
                        {
                            return "{\"code\":\"500\",\"msg\":\"普通会员价格设置有误\"}";
                        }
                    }
                    else {
                        sql1 = sql1.Replace("@price1", "");
                    }
                    if (!string.IsNullOrEmpty(by_price))
                    {
                        try
                        {
                            double.Parse(by_price);
                            sql1 = sql1.Replace("@price2", double.Parse(by_price).ToString("f2"));
                        }
                        catch
                        {
                            return "{\"code\":\"500\",\"msg\":\"白银会员价格设置有误\"}";
                        }
                    }
                    else
                    {
                        sql1 = sql1.Replace("@price2", "");
                    }
                    if (!string.IsNullOrEmpty(hj_price))
                    {
                        try
                        {
                            double.Parse(hj_price);
                            sql1 = sql1.Replace("@price3", double.Parse(hj_price).ToString("f2"));
                        }
                        catch
                        {
                            return "{\"code\":\"500\",\"msg\":\"黄金会员价格设置有误\"}";
                        }
                    }
                    else
                    {
                        sql1 = sql1.Replace("@price3", "");
                    }
                    sql1 = sql1.Replace("@path", path.Replace("..", ""));
                    sql1 = sql1.Replace("@protype", lb);
                    sql1 = sql1.Replace("@description", des);
                    sql1 = sql1.Replace("@companyID", companyID);
                    sql1 = sql1.Replace("@bzq", bzq);
                    sql1 = sql1.Replace("@httpImageUrl", httpImgUrl);
                    sql1 = sql1.Replace("@sluid", zt);
                    sql1 = sql1.Replace("@progg", progg);
                    sql1 = sql1.Replace("@brandID", brandID);
                    sql1 = sql1.Replace("@shortName", shortname);
                    sql1 = sql1.Replace("@bh", bh);
                    sql1 = sql1.Replace("@tag", tag);
                    sql1 = sql1.Replace("@dstype", type);
                    sql1 = sql1.Replace("@startSend", startSend);
                    
                    a = DbHelperSQL.ExecuteSql(sql1);

                    if (a > 0)
                    {
                        return "{\"code\":\"200\",\"msg\":\"添加成功\"}";
                    }
                    else
                    {
                        return "{\"code\":\"500\",\"msg\":\"添加失败\"}";
                    }
                }
            }
            catch {
                return "{\"code\":\"500\",\"msg\":\"系统异常\"}";
            }
        }
        public void valProduct(HttpContext context)
        {
            
            string name = context.Request["name"].ToString();
            string companyID = context.Request["companyID"].ToString();
            string sql = "select * from asm_product  where proName='" + name + "' and  companyID=" + companyID;
           
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                stringBuilder.Append("{");
                stringBuilder.Append("\"result\":\"1\""); //完成
                stringBuilder.Append("}");
                context.Response.Write(stringBuilder.ToString());

            }
            else
            {
                System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                stringBuilder.Append("{");
                stringBuilder.Append("\"result\":\"2\""); //完成
                stringBuilder.Append("}");
                context.Response.Write(stringBuilder.ToString());
            }
           
        }
        public void setQK(HttpContext context)
        {
            
            string mechine_id = context.Request["mechineID"].ToString();
            string ldNO = context.Request["ldNO"].ToString();
            string sql = "update asm_ldInfo set type=0 where ldNO in(" + ldNO + ")  and mechineID="+mechine_id;//料道类型0未启用1订购2零售
           
            int a = DbHelperSQL.ExecuteSql(sql);
            if (a > 0)
            {
                System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                stringBuilder.Append("{");
                stringBuilder.Append("\"result\":\"1\""); //完成
                stringBuilder.Append("}");
                context.Response.Write(stringBuilder.ToString());
            }
            else
            {
                System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                stringBuilder.Append("{");
                stringBuilder.Append("\"result\":\"2\""); //完成
                stringBuilder.Append("}");
                context.Response.Write(stringBuilder.ToString());
            }
           
        }
        public void setDG(HttpContext context)
        {
            
            string mechine_id = context.Request["mechineID"].ToString();
            string ldNO = context.Request["ldNO"].ToString();
            string sql = "update asm_ldInfo set type=1 where ldNO in("+ldNO+")  and mechineID="+mechine_id;//料道类型0未启用1订购2零售
          
            int a=DbHelperSQL.ExecuteSql(sql);
            if (a > 0)
            {
                System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                stringBuilder.Append("{");
                stringBuilder.Append("\"result\":\"1\""); //完成
                stringBuilder.Append("}");
                context.Response.Write(stringBuilder.ToString());
            }
            else {
                System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                stringBuilder.Append("{");
                stringBuilder.Append("\"result\":\"2\""); //完成
                stringBuilder.Append("}");
                context.Response.Write(stringBuilder.ToString());
            }
           
        }
        public void setLS(HttpContext context)
        {
             
            string mechine_id = context.Request["mechineID"].ToString();
            string ldNO = context.Request["ldNO"].ToString();
            string sql = "update asm_ldInfo set type=2 where ldNO in(" + ldNO + ")  and mechineID="+mechine_id;//料道类型0未启用1订购2零售
          
            int a = DbHelperSQL.ExecuteSql(sql);
            if (a > 0)
            {
                System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                stringBuilder.Append("{");
                stringBuilder.Append("\"result\":\"1\""); //完成
                stringBuilder.Append("}");
                context.Response.Write(stringBuilder.ToString());
            }
            else
            {
                System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                stringBuilder.Append("{");
                stringBuilder.Append("\"result\":\"2\""); //完成
                stringBuilder.Append("}");
                context.Response.Write(stringBuilder.ToString());
            }
           
        }
        public void login(HttpContext context)
        {
            
            string name = context.Request["name"].ToString();
            string pwd = context.Request["pwd"].ToString();
            string qx = context.Request["qx"].ToString();
            if (qx == "1")//管理员
            {
                string sql = "select * from asm_company where comBH='" + name + "' and pwd='" + pwd + "'";
               
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {

                    OperUtil.Add("companyID", dt.Rows[0]["id"].ToString());
                    OperUtil.Add("operaID","0");
                    OperUtil.Add("comOperID", dt.Rows[0]["id"].ToString());
                    OperUtil.setCooki("operaName",name);
                    System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                    stringBuilder.Append("{");
                    stringBuilder.Append("\"result\":\"1\""); //完成
                    stringBuilder.Append("}");
                    context.Response.Write(stringBuilder.ToString());
                }
                else
                {
                    System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                    stringBuilder.Append("{");
                    stringBuilder.Append("\"result\":\"2\""); //完成
                    stringBuilder.Append("}");
                    context.Response.Write(stringBuilder.ToString());
                }
            } else if (qx=="2")
            {
                string sql = "select * from asm_opera where name='"+name+"' and pwd='"+pwd+"' and companyID!=0";
                DataTable dd = DbHelperSQL.Query(sql).Tables[0];
                if (dd.Rows.Count > 0)
                {
                    OperUtil.Add("companyID", dd.Rows[0]["companyID"].ToString());
                    OperUtil.Add("operaID", dd.Rows[0]["id"].ToString());
                    OperUtil.Add("comOperID", dd.Rows[0]["id"].ToString());
                    OperUtil.setCooki("operaName", name);
                    //判断当前操作员是否是代理商
                    string sql1 = "select * from asm_qx where  menuID='szdls' and flag=1  and roleID=" + dd.Rows[0]["qx"].ToString();
                    DataTable d1 = DbHelperSQL.Query(sql1).Tables[0];
                    if (d1.Rows.Count > 0)
                    {
                        OperUtil.Add("isdls", "1");//1是代理商
                    }
                    else {
                        OperUtil.Add("isdls", "0");//0不是代理商
                    }
                    System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                    stringBuilder.Append("{");
                    stringBuilder.Append("\"result\":\"1\""); //完成
                    stringBuilder.Append("}");
                    context.Response.Write(stringBuilder.ToString());
                }
                else {
                    System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                    stringBuilder.Append("{");
                    stringBuilder.Append("\"result\":\"2\""); //完成
                    stringBuilder.Append("}");
                    context.Response.Write(stringBuilder.ToString());
                }
            }
           
        }
        public void uploadFile(HttpContext context)
        {
            //如果提交的文件名是空，则不处理
            if (context.Request.Files.Count == 0 || string.IsNullOrWhiteSpace(context.Request.Files[0].FileName))
                return;
            //获取文件流
            Stream stream = context.Request.Files[0].InputStream;
            //获取文件名称
            string fileName = Path.GetFileName(context.Request.Files[0].FileName);
            //声明字节数组
            byte[] buffer;
            //为什么是4096呢？这是操作系统中最小的分配空间，如果你的文件只有100个字节，其实它占用的空间是4096个字节
            int bufferSize = 4096;
            //获取上传文件流的总长度
            long totalLength = stream.Length;
            //已经写入的字节数，用于做上传的百分比
            long writtenSize = 0;
            //创建文件
            using (FileStream fs = new FileStream(@"C:\" + fileName, FileMode.Create, FileAccess.Write))
            {
                //如果写入文件的字节数小于上传的总字节数，就一直写，直到写完为止
                while (writtenSize < totalLength)
                {
                    //如果剩余的字节数不小于最小分配空间
                    if (totalLength - writtenSize >= bufferSize)
                    {
                        //用最小分配空间创建新的字节数组
                        buffer = new byte[bufferSize];
                    }
                    else
                        //用剩余的字节数创建字节数组
                        buffer = new byte[totalLength - writtenSize];
                    //读取上传的文件到字节数组
                    stream.Read(buffer, 0, buffer.Length);
                    //将读取的字节数组写入到新建的文件流中
                    fs.Write(buffer, 0, buffer.Length);
                    //增加写入的字节数
                    writtenSize += buffer.Length;
                    //计算当前上传文件的百分比
                    long percent = writtenSize * 100 / totalLength;
                }
            }
        }
        public void updateLdInfo(HttpContext context)
        {
            
          
            string mechineID = context.Request["mechineID"].ToString();
            string ldNO = context.Request["ldNO"].ToString();
            string sql1 = "select * from asm_ldInfo where ldNO='"+ldNO+"' and mechineID="+mechineID;
            DataTable dt = DbHelperSQL.Query(sql1).Tables[0];
            if (dt.Rows.Count>0)
            {
                if (dt.Rows[0]["statu"].ToString() == "1")
                {
                    string sql = "update asm_ldInfo set statu=0 where ldNO='" + ldNO + "' and mechineID=" + mechineID;
                    int a= DbHelperSQL.ExecuteSql(sql);
                    if (a > 0)
                    {
                        System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                        stringBuilder.Append("{");
                        stringBuilder.Append("\"result\":\"1\""); //完成
                        stringBuilder.Append("}");
                        context.Response.Write(stringBuilder.ToString());
                    }
                    else {
                        System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                        stringBuilder.Append("{");
                        stringBuilder.Append("\"result\":\"2\""); //完成
                        stringBuilder.Append("}");
                        context.Response.Write(stringBuilder.ToString());
                    }
                }
                else {
                    string sql = "update asm_ldInfo set statu=1 where ldNO='" + ldNO + "' and mechineID=" + mechineID;
                    int a= DbHelperSQL.ExecuteSql(sql);
                    if (a > 0)
                    {
                        System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                        stringBuilder.Append("{");
                        stringBuilder.Append("\"result\":\"1\""); //完成
                        stringBuilder.Append("}");
                        context.Response.Write(stringBuilder.ToString());
                    }
                    else
                    {
                        System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                        stringBuilder.Append("{");
                        stringBuilder.Append("\"result\":\"2\""); //完成
                        stringBuilder.Append("}");
                        context.Response.Write(stringBuilder.ToString());
                    }
                }
            }
          
        }
        public void updateMechineType(HttpContext context)
        {
            
          
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            string name = context.Request["name"].ToString();
            string mechineTypeID = context.Request["mechineTypeID"].ToString();
            string sql1 = "select * from asm_mechineType where name='" + name + "'";
            DataTable dt = DbHelperSQL.Query(sql1).Tables[0];
            if (dt.Rows.Count > 0)
            {
                System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                stringBuilder.Append("{");
                stringBuilder.Append("\"result\":\"3\""); //完成
                stringBuilder.Append("}");
                context.Response.Write(stringBuilder.ToString());
            }
            else {
                string sql = "update asm_mechineType set name='"+name+"' where id="+mechineTypeID;
                int b= DbHelperSQL.ExecuteSql(sql);
                if (b > 0)
                {
                    System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                    stringBuilder.Append("{");
                    stringBuilder.Append("\"result\":\"1\""); //完成
                    stringBuilder.Append("}");
                    context.Response.Write(stringBuilder.ToString());
                }
                else {
                    System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                    stringBuilder.Append("{");
                    stringBuilder.Append("\"result\":\"2\""); //完成
                    stringBuilder.Append("}");
                    context.Response.Write(stringBuilder.ToString());
                }
            }
          
        }
        public void addMechineType(HttpContext context)
        {
            
         
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            string name = context.Request["name"].ToString();
            string sql1 = "select * from asm_mechineType where name='"+name+"'";
            DataTable dt= DbHelperSQL.Query(sql1).Tables[0];
            if (dt.Rows.Count > 0)
            {
                System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                stringBuilder.Append("{");
                stringBuilder.Append("\"result\":\"3\""); //完成
                stringBuilder.Append("}");
                context.Response.Write(stringBuilder.ToString());
            }
            else {
                string sql = "insert into asm_mechineType(name) values('" + name + "');select @@IDENTITY";
                object obj = DbHelperSQL.GetSingle(sql);
                if (obj == null)
                {
                    System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                    stringBuilder.Append("{");
                    stringBuilder.Append("\"result\":\"2\""); //完成
                    stringBuilder.Append("}");
                    context.Response.Write(stringBuilder.ToString());
                }
                else {
                    //往asm_ldModel添加60条记录
                    for (int i = 1; i <= 60; i++)
                    {
                        string sql2 = "insert into asm_ldModel (mechineType,ldNO,statu,ldNum,type) values("+Convert.ToInt32(obj)+",'"+i.ToString()+"',1,1,0)";
                        DbHelperSQL.ExecuteSql(sql2);
                    }
                    System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                    stringBuilder.Append("{");
                    stringBuilder.Append("\"result\":\"1\""); //完成
                    stringBuilder.Append("}");
                    context.Response.Write(stringBuilder.ToString());
                }
                
            }
           

        }
        public void getMchineType(HttpContext context)
        {
             
         
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            string sql = "select * from asm_mechineType ";
            string str = "";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                //str= DataTableToJsonWithJsonNet(dt);
                str=js.Serialize(dt);
            }
           
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            stringBuilder.Append("{");
            stringBuilder.Append("\"result\":\""+str+"\""); //完成
            stringBuilder.Append("}");
            context.Response.Write(stringBuilder.ToString());
          
        }
        public void getLDInfoByMechineID(HttpContext context)
        {
             
           
            string mechineID = context.Request["mechineID"].ToString();
            string sql = "select * from asm_ldInfo where mechineID=" + mechineID;
            string str = "";
    
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    str += dt.Rows[i]["ldNO"].ToString() + "-" + dt.Rows[i]["statu"].ToString() + "|";
                     
                }
                str = str.Substring(0, str.Length - 1);
               
            }
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            stringBuilder.Append("{");
            stringBuilder.Append("\"result\":\"" + str + "\""); //完成
        
            stringBuilder.Append("}");
            context.Response.Write(stringBuilder.ToString());
          
        }
        public void getLDInfo(HttpContext context)
        {
            
         
            string type = context.Request["type"].ToString();
            string sql = "select * from asm_ldModel where mechineType="+type;
            string str = "";
            string str1 = "";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count>0)
            {
                for (int i=0;i<dt.Rows.Count;i++)
                {
                    str += dt.Rows[i]["ldNO"].ToString() + "-" + dt.Rows[i]["statu"].ToString() + "|";
                    if (dt.Rows[i]["ldNO"].ToString()=="001"|| dt.Rows[i]["ldNO"].ToString() == "011"|| dt.Rows[i]["ldNO"].ToString() == "021"|| dt.Rows[i]["ldNO"].ToString() == "031"|| dt.Rows[i]["ldNO"].ToString() == "041"|| dt.Rows[i]["ldNO"].ToString() == "051")
                    {
                        str1 += dt.Rows[i]["ldNum"].ToString()+"|";
                    }
                }
                str = str.Substring(0,str.Length-1);
                str1 = str1.Substring(0, str1.Length - 1);
            }
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            stringBuilder.Append("{");
            stringBuilder.Append("\"result\":\""+str+"\""); //完成
            stringBuilder.Append(",\"sel\":\"" + str1 + "\"");
            stringBuilder.Append("}");
            context.Response.Write(stringBuilder.ToString());
          
        }
        public void addLD(HttpContext context)
        {
             
            string sel1 = context.Request["sel1"].ToString();
            string sel2 = context.Request["sel2"].ToString();
            string sel3 = context.Request["sel3"].ToString();
            string sel4 = context.Request["sel4"].ToString();
            string sel5 = context.Request["sel5"].ToString();
            string sel6 = context.Request["sel6"].ToString();
            string val1 = context.Request["val1"].ToString();
            string val2 = context.Request["val2"].ToString();
            string val3 = context.Request["val3"].ToString();
            string val4 = context.Request["val4"].ToString();
            string val5 = context.Request["val5"].ToString();
            string val6 = context.Request["val6"].ToString();
            string type = context.Request["type"].ToString();
            //先删除信息
            string del_sql = "delete from asm_ldModel where mechineType="+type;
            DbHelperSQL.ExecuteSql(del_sql);
            for (int i=0;i<10;i++)
            {
                string sql1 = "insert into asm_ldModel (mechineType,ldNO,statu,ldNum,type) values("+type+",'"+val1.Split('|')[i].Split('-')[0] +"',"+ val1.Split('|')[i].Split('-')[1] + ","+sel1+",0)";
                DbHelperSQL.ExecuteSql(sql1);
            }
            for (int i = 0; i < 10; i++)
            {
                string sql1 = "insert into asm_ldModel (mechineType,ldNO,statu,ldNum,type) values(" + type + ",'" + val2.Split('|')[i].Split('-')[0] + "'," + val2.Split('|')[i].Split('-')[1] + "," + sel2 + ",0)";
                DbHelperSQL.ExecuteSql(sql1);
            }
            for (int i = 0; i < 10; i++)
            {
                string sql1 = "insert into asm_ldModel (mechineType,ldNO,statu,ldNum,type) values(" + type + ",'" + val3.Split('|')[i].Split('-')[0]+ "'," + val3.Split('|')[i].Split('-')[1] + "," + sel3+ ",0)";
                DbHelperSQL.ExecuteSql(sql1);
            }
            for (int i = 0; i < 10; i++)
            {
                string sql1 = "insert into asm_ldModel (mechineType,ldNO,statu,ldNum,type) values(" + type + ",'" + val4.Split('|')[i].Split('-')[0] + "'," + val4.Split('|')[i].Split('-')[1] + "," + sel4 + ",0)";
                DbHelperSQL.ExecuteSql(sql1);
            }
            for (int i = 0; i < 10; i++)
            {
                string sql1 = "insert into asm_ldModel (mechineType,ldNO,statu,ldNum,type) values(" + type + ",'" + val5.Split('|')[i].Split('-')[0] + "'," + val5.Split('|')[i].Split('-')[1] + "," + sel5 + ",0)";
                DbHelperSQL.ExecuteSql(sql1);
            }
            for (int i = 0; i < 10; i++)
            {
                string sql1 = "insert into asm_ldModel (mechineType,ldNO,statu,ldNum,type) values(" + type + ",'" + val6.Split('|')[i].Split('-')[0]+ "'," + val6.Split('|')[i].Split('-')[1] + "," + sel6 + ",0)";
                DbHelperSQL.ExecuteSql(sql1);
            }
            
            context.Response.Write("1");//成功
        }
        public void setNormal(HttpContext context)
        {
            
            string id = context.Request["id"].ToString();
            string type = context.Request["type"].ToString();
            string sql = "update asm_mechine set statu="+type+" where id="+id;
            int a= DbHelperSQL.ExecuteSql(sql);
            if (a > 0)
            {
                if (id == "68" || id == "69")
                {
                    string mechineInfo = RedisUtil.getMechine(id);
                    JArray jay = RedisUtil.DeserializeObject(mechineInfo);

                    jay[0]["statu"] = type;
                   
                    RedisHelper.SetRedisModel<string>(id + "_mechineInfoSet", JsonConvert.SerializeObject(jay));

                }
                context.Response.Write("1");//成功
            }
            else {
                context.Response.Write("2");//
            }
          
        }
        public void deleteEqui(HttpContext context)
        {
            
            string equID = context.Request["equID"].ToString();
            string sql = "delete from asm_mechine where id="+equID;
            int a = DbHelperSQL.ExecuteSql(sql);
            if (a > 0)
            {
                if (equID == "68" || equID == "69")
                {
                    
                    RedisHelper.Remove(equID + "_mechineInfoSet");

                }
                context.Response.Write("1");//成功
            }
            else
            {
                context.Response.Write("2");//失败
            }
          
        }
        public void saveEquiSet(HttpContext context)
        {
            string yxq = context.Request["yxq"].ToString();
            string type = context.Request["type"].ToString();
            string equID = context.Request["equID"].ToString();
            string update = "update asm_mechine set statu="+type+",validateTime='"+yxq+"' where id="+equID;
            int a=DbHelperSQL.ExecuteSql(update);
            if (a > 0)
            {
                Util.ClearRedisMechineInfoByMechineID(equID);
                if (equID == "68" || equID == "69")
                {
                    string mechineInfo = RedisUtil.getMechine(equID);
                    JArray jay = RedisUtil.DeserializeObject(mechineInfo);

                    jay[0]["statu"] = type;
                    jay[0]["validateTime"] = yxq;
                    RedisHelper.SetRedisModel<string>(equID + "_mechineInfoSet", JsonConvert.SerializeObject(jay));

                }
                context.Response.Write("1");//成功
            }
            else {
                context.Response.Write("2");//失败
            }
           
        }
        public void addEquipment(HttpContext context)
        {
            
            string start = context.Request["start"].ToString();
            string bh = context.Request["bh"].ToString();
            string pwd = context.Request["pwd"].ToString();
            string mechineType = context.Request["type"].ToString();
            //验证所选的日期要大于今天
            int a=DateTime.Compare(DateTime.Parse(start),DateTime.Now);
            if (a<0)
            {
                System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                stringBuilder.Append("{");
                stringBuilder.Append("\"result\":\"1\""); //完成

                stringBuilder.Append("}");
                context.Response.Write(stringBuilder.ToString());
                return;
            }
            string sql3 = "select * from asm_mechine where bh='"+bh+"'";
            DataTable dt1 = DbHelperSQL.Query(sql3).Tables[0];
            if (dt1.Rows.Count>0)
            {
                System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                stringBuilder.Append("{");
                stringBuilder.Append("\"result\":\"4\""); //完成

                stringBuilder.Append("}");
                context.Response.Write(stringBuilder.ToString());
                return;
            }
            string sql = "insert into asm_mechine (bh,pwd,validateTime,statu,regTime,version) values('" + bh+"','"+pwd+"','"+start+"',5,'"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+ "',"+ mechineType + ");select @@IDENTITY";
            object obj = DbHelperSQL.GetSingle(sql);
            if (obj == null)
            {
                System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                stringBuilder.Append("{");
                stringBuilder.Append("\"result\":\"3\""); //完成
                stringBuilder.Append("}");
                context.Response.Write(stringBuilder.ToString());
            }
            else
            {
                if (Convert.ToInt32(obj).ToString()=="68"||Convert.ToInt32(obj).ToString() == "69") {
                    Util.addMechineToList(Convert.ToInt32(obj).ToString());
                }
               
                //向asm_ldInfo表里添加料道
                string sql1 = "select * from asm_ldModel where mechineType="+mechineType;
                DataTable dt = DbHelperSQL.Query(sql1).Tables[0];
                if (dt.Rows.Count>0)
                {
                    for (int i=0;i<dt.Rows.Count;i++)
                    {
                        string sql2 = "insert into asm_ldInfo (ldNO,type,mechineID,statu,ldNum,ld_productNum) values('"+dt.Rows[i]["ldNO"].ToString() +"',0,"+ Convert.ToInt32(obj) + ","+dt.Rows[i]["statu"].ToString() +","+dt.Rows[i]["ldNum"].ToString()+",0)";
                        DbHelperSQL.ExecuteSql(sql2);
                    }
                }
                System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                stringBuilder.Append("{");
                stringBuilder.Append("\"result\":\"2\""); //完成
                stringBuilder.Append("}");
                context.Response.Write(stringBuilder.ToString());

            }
         
        }
        public string dateTime = "";
        public void addCompany(HttpContext context)
        {
          
            string comName = context.Request["comName"].ToString();
            string fzr = context.Request["fzr"].ToString();
            string cwr = context.Request["cwr"].ToString();
            string phone = context.Request["phone"].ToString();
            string business = context.Request["business"].ToString();
            string logoPath = "logo/"+dateTime+".jpg";//logo上传路径
            string yyzzPath = "yyzz/"+dateTime+".jpg";

            string sql1 = "select * from asm_company where name='"+comName+"'";
            DataTable dt1 = DbHelperSQL.Query(sql1).Tables[0];
            if (dt1.Rows.Count>0)
            {
                context.Response.Write("3");//保存成功
                return;
            }

            string sql = @"INSERT INTO [dbo].[asm_company]
                       ([name]
                       ,[linkman]
                       ,[linkPhone]
                       ,[province]
                       ,[city]
                       ,[country]
                       ,[linkaddress]
                       ,[path]
                       ,[statu]
                       ,[fianceman]
                       ,[business]
                       ,[yyzzPath])
                 VALUES
                       ('"+comName+"', '"+fzr+"', '"+phone+"', '', '', '', '', '"+logoPath+"', 1, '"+cwr+"', '"+business+"', '"+yyzzPath+"')";
            int a=DbHelperSQL.ExecuteSql(sql);
            if (a > 0)
            {
                context.Response.Write("1");//保存成功
            }
            else {
                context.Response.Write("2");//保存失败
            }
        
        }
        //上传营业执照
       
        public void yyzzUploads(HttpContext context)
        {
            HttpFileCollection files = context.Request.Files;
            string path = "";//照片路径  
            bool errorflag = true;
            string tishi = "";
            if (files.Count > 0)//Form中获取文件对象  
            {
                HttpPostedFile file = files[0];
                if (file.ContentLength > 0)//文件大小大于零  
                {
                    string fileName = file.FileName;//文件名  
                    int fileSize = file.ContentLength;//文件大小  
                    if (fileName.Substring(fileName.Length - 4, 4).ToLower() == ".jpg")//只支持.jpg文件上传  
                    {
                        try
                        {
                            Bitmap bitmap = new Bitmap(file.InputStream);//获取图片文件  
                            //if (bitmap.Width > 480 || bitmap.Height > 640)
                            //{
                            //    errorflag = false;
                            //    tishi = "照片太大，请将照片调整为320*240像素尺寸！";
                            //}
                            //if (bitmap.Width < 120 || bitmap.Height < 160)
                            //{
                            //    errorflag = false;
                            //    tishi = "照片太小，请将照片调整为320*240像素尺寸！";
                            //}
                            //if (bitmap.Width > bitmap.Height)
                            //{
                            //    errorflag = false;
                            //    tishi = "照片的宽度比不符合要求，请将高度比调整为4:3！";
                            //}
                            //if (fileSize > 100 * 1024)
                            //{
                            //    errorflag = false;
                            //    tishi = "照片的大小不符合要求，请将照片调整为100kb以内！";
                            //}
                        }
                        catch
                        {
                            errorflag = false;
                            tishi = "照片错误，上传文件非图像文件！";
                        }
                    }
                    else
                    {
                        errorflag = false;
                        tishi = "照片格式错误，请上传JPG格式照片文件！";
                    }
                }
            }
            else
            {
                errorflag = false;
                tishi = "照片错误，上传照片文件为0字节！";
            }
            if (errorflag)
            {
                string extension = ".jpg";
                dateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                path = "../yyzz/"+dateTime+ extension;//指定保存路径以及文件名，也就是完整相对路径  
                files[0].SaveAs(System.Web.HttpContext.Current.Server.MapPath(path));//保存文件（将相对路径转化为绝对路径）  
                context.Response.Write(path);//相应给客户端该照片的相对路径  
                logoUploads(context, dateTime);
                addCompany(context);
            }
            else
            {
                context.Response.Write("ERR" + tishi);
            }
        }
        public void logoUploads(HttpContext context,string name)
        {
            HttpFileCollection files = context.Request.Files;
            string path = "";//照片路径  
            bool errorflag = true;
            string tishi = "";
            if (files.Count > 0)//Form中获取文件对象  
            {
                HttpPostedFile file = files[1];
                if (file.ContentLength > 0)//文件大小大于零  
                {
                    string fileName = file.FileName;//文件名  
                    int fileSize = file.ContentLength;//文件大小  
                    if (fileName.Substring(fileName.Length - 4, 4).ToLower() == ".jpg")//只支持.jpg文件上传  
                    {
                        try
                        {
                            Bitmap bitmap = new Bitmap(file.InputStream);//获取图片文件  
                            
                        }
                        catch
                        {
                            errorflag = false;
                            tishi = "照片错误，上传文件非图像文件！";
                        }
                    }
                    else
                    {
                        errorflag = false;
                        tishi = "照片格式错误，请上传JPG格式照片文件！";
                    }
                }
            }
            else
            {
                errorflag = false;
                tishi = "照片错误，上传照片文件为0字节！";
            }
            if (errorflag)
            {
                string extension = ".jpg";
                path = "../logo/" + name + extension;//指定保存路径以及文件名，也就是完整相对路径  
                files[1].SaveAs(System.Web.HttpContext.Current.Server.MapPath(path));//保存文件（将相对路径转化为绝对路径）  
                context.Response.Write(path);//相应给客户端该照片的相对路径  

            }
            else
            {
                context.Response.Write("ERR" + tishi);
            }
        }
        public void exportExcel(HttpContext context)
        {
          
            string sql = @"select productID,
                            proName,
                            sum(dgNUm) dg,
                            sum(lsNUM)  ls,
                            createTime,
                            mechineID
                            from(
                            select ao.productID, ap.proName,ao.mechineID, count(*) dgNUm, '0'lsNUM,createTime from asm_orderDetail ao left join asm_product ap on ao.productID = ap.productID where ao.mechineID = 24 group by ao.productID, ap.proName,createTime,ao.mechineID
                            union all 
                            select ar.productID, ap.proName,ar.mechineID, '0' dgNUm, sum(num) lsNUM,convert(varchar(100), ar.delivertTime, 23) from asm_reserve ar left join asm_product ap on ar.productID = ap.productID where ar.mechineID = 24  group by ar.productID,ar.mechineID, ap.proName,convert(varchar(100), ar.delivertTime, 23))
                            C group by productID, proName,createTime,mechineID order by productID";
            DataTable dda = DbHelperSQL.Query(sql).Tables[0];
            ExportToSpreadsheet(dda, DateTime.Now.ToString("yyyyMMdd"));
            
        }

        public static void ExportToSpreadsheet(DataTable table, string name)
        {
            Random r = new Random();
            string rf = "";
            for (int j = 0; j < 10; j++)
            {
                rf = r.Next(int.MaxValue).ToString();
            }
            HttpContext context = HttpContext.Current;
            context.Response.Clear();
            context.Response.ContentType = "application/ms-excel";
            context.Response.ContentEncoding = System.Text.Encoding.UTF8;
            context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + name + rf + ".xls");
            context.Response.BinaryWrite(System.Text.Encoding.UTF8.GetPreamble());

            foreach (DataColumn column in table.Columns)
            {
                if (column.ColumnName == "proName")
                {
                    context.Response.Write("商品名称" + ",");
                }
                if (column.ColumnName == "dg")
                {
                    context.Response.Write("订购数量" + ",");
                }
                if (column.ColumnName == "ls")
                {
                    context.Response.Write("零售数量" + ",");
                }
                if (column.ColumnName == "createTime")
                {
                    context.Response.Write("订单日期" + ",");
                }
                if (column.ColumnName == "mechineID")
                {
                    context.Response.Write("111" + ",");
                }
            }
            context.Response.Write(Environment.NewLine);
            double test;
            foreach (DataRow row in table.Rows)
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    switch (table.Columns[i].DataType.ToString())
                    {
                        case "System.String":
                            if (double.TryParse(row[i].ToString(), out test)) context.Response.Write("=");
                            context.Response.Write("\"" + row[i].ToString().Replace("\"", "\"\"") + "\",");
                            break;
                        case "System.DateTime":
                            if (row[i].ToString() != "")
                                context.Response.Write("\"" + ((DateTime)row[i]).ToString("yyyy-MM-dd HH:mm:ss") + "\",");
                            else
                                context.Response.Write("\"" + row[i].ToString().Replace("\"", "\"\"") + "\",");
                            break;
                        default:
                            context.Response.Write("\"" + row[i].ToString().Replace("\"", "\"\"") + "\",");
                            break;
                    }
                }
                context.Response.Write(Environment.NewLine);
            }
            context.Response.End();
        }
        private void ResponseWriteEnd(HttpContext context, string msg)
        {
            context.Response.Write(msg);
            context.Response.End();
        }
        public string DataTableToJsonWithJsonNet(DataTable table)
        {
            string jsonString = string.Empty;
            jsonString = JsonConvert.SerializeObject(table);
            return jsonString;
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}