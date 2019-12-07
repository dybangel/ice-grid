using autosell_center.util;
using Consumer.cls;
using DBUtility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

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
                case "qx_judge"://判断权限登录
                    this.qx_judge(context);
                    return;
                case "login":  //奶企登录
                    this.login(context);
                    return;
                case "yyzzUploads2"://修改图片
                    this.yyzzUploads2(context);
                    return;
                case "updateCompany"://修改图片
                    this.updateCompany(context);
                    return;
                case "addHB"://上传支付宝红包
                    this.addHB(context);
                    return;
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
        public void login(HttpContext context)
        {
            string name = context.Request["name"].ToString();
            string pwd = context.Request["pwd"].ToString();
            string qx = context.Request["qx"].ToString();
            if (qx == "1")//管理员
            {
                string sql = "select * from [dbo].[asm_manager] where bh='" + name + "' and pwd='" + pwd + "'";
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    
                    OperUtil.Add("AdminOperaID", "0");
                    OperUtil.setCooki("operaName", name);
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
            else if (qx == "2")
            {
                string sql = "select * from asm_opera where name='" + name + "' and pwd='" + pwd + "'";
                DataTable dd = DbHelperSQL.Query(sql).Tables[0];
                if (dd.Rows.Count > 0)
                {
                    OperUtil.setCooki("operaName",name);
                    OperUtil.Add("AdminOperaID", dd.Rows[0]["id"].ToString());
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
        public void qx_judge(HttpContext context)
        {
            string menu = context.Request["menu"].ToString();
            string operaID = OperUtil.Get("AdminOperaID");//为0说明是管理员登录 
            if (operaID == "0")
            {
                //允许查看登录
                System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                stringBuilder.Append("{");
                stringBuilder.Append("\"result\":\"ok\""); //完成
                stringBuilder.Append("}");
                context.Response.Write(stringBuilder.ToString());
            }
            else
            {
                string sql = "select * from asm_opera where id=" + operaID;
                DataTable dd = DbHelperSQL.Query(sql).Tables[0];
                if (dd.Rows.Count > 0)
                {
                    string sql2 = "select * from asm_qx where roleID=" + dd.Rows[0]["qx"].ToString() + " and menuID='" + menu + "' ";
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
                        }
                        else if (dt.Rows[0]["flag"].ToString() == "1")
                        {
                            //允许登录
                            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                            stringBuilder.Append("{");
                            stringBuilder.Append("\"result\":\"ok\""); //完成
                            stringBuilder.Append("}");
                            context.Response.Write(stringBuilder.ToString());
                        }
                    }
                    else
                    {
                        //请联系管理员给当前登录角色赋值权限
                        System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                        stringBuilder.Append("{");
                        stringBuilder.Append("\"result\":\"1\""); //完成
                        stringBuilder.Append("}");
                        context.Response.Write(stringBuilder.ToString());
                    }
                }
                else
                {
                    //跳转重新登录
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
                    for (int i = 10; i <= 69; i++)
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
                    if (dt.Rows[i]["ldNO"].ToString()=="10"|| dt.Rows[i]["ldNO"].ToString() == "20"|| dt.Rows[i]["ldNO"].ToString() == "30"|| dt.Rows[i]["ldNO"].ToString() == "40"|| dt.Rows[i]["ldNO"].ToString() == "50"|| dt.Rows[i]["ldNO"].ToString() == "60")
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
                string sql1 = "insert into asm_ldModel (mechineType,ldNO,statu,ldNum,type) values(" + type + ",'" + val3.Split('|')[i].Split('-')[0] + "'," + val3.Split('|')[i].Split('-')[1] + "," + sel3+ ",0)";
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
                string sql1 = "insert into asm_ldModel (mechineType,ldNO,statu,ldNum,type) values(" + type + ",'" + val6.Split('|')[i].Split('-')[0] + "'," + val6.Split('|')[i].Split('-')[1] + "," + sel6 + ",0)";
                DbHelperSQL.ExecuteSql(sql1);
            }
            context.Response.Write("1");//成功
        }
        public void setNormal(HttpContext context)
        {
            string id = context.Request["id"].ToString();
            string type = context.Request["type"].ToString();
            string sql = "update asm_mechine set zt="+type+" where id="+id;
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
            string update = "update asm_mechine set zt="+type+",validateTime='"+yxq+"' where id="+equID;
            int a=DbHelperSQL.ExecuteSql(update);
            if (a > 0)
            {
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
            string sql = "insert into asm_mechine (bh,pwd,validateTime,statu,regTime,version,zt) values('" + bh+"','"+pwd+"','"+start+"',0,'"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+ "',"+ mechineType + ",2);select @@IDENTITY";
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
                if (Convert.ToInt32(obj).ToString() == "68" || Convert.ToInt32(obj).ToString() == "69")
                {
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
            string bh = context.Request["bh"].ToString();
            string pwd = context.Request["pwd"].ToString();
            string comName = context.Request["comName"].ToString();
            string fzr = context.Request["fzr"].ToString();
            string cwr = context.Request["cwr"].ToString();
            string phone = context.Request["phone"].ToString();
            string business = context.Request["business"].ToString();
            string code = context.Request["code"].ToString();
            string logoPath = "logo/"+dateTime+".jpg";//logo上传路径
            string yyzzPath = "yyzz/"+dateTime+".jpg";

            string sql1 = "select * from asm_company where name='"+comName+"'";
            DataTable dt1 = DbHelperSQL.Query(sql1).Tables[0];
            if (dt1.Rows.Count>0)
            {
                context.Response.Write("3");//保存成功
                return;
            }
            string sql23 = "select * from asm_company where code='" + code + "'";
            DataTable dt23 = DbHelperSQL.Query(sql23).Tables[0];
            if (dt23.Rows.Count > 0)
            {
                context.Response.Write("4");//保存成功
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
                       ,[yyzzPath],comBH,pwd,code,pwd2)
                 VALUES
                       ('"+comName+"', '"+fzr+"', '"+phone+"', '', '', '', '', '"+logoPath+"', 1, '"+cwr+"', '"+business+"', '"+yyzzPath+"','"+bh+"','"+pwd+ "','"+code+"','"+pwd+"');select @@IDENTITY";
            object obj = DbHelperSQL.GetSingle(sql);
            if (obj!=null)
            {
                //切需要给该企业
                int companyID=Convert.ToInt32(obj);
                string sql2 = "insert into asm_activity(companyID,zqName,zq,statu) values("+companyID+",'1天',1,0)";
                DbHelperSQL.ExecuteSql(sql2);
                string sql3 = "insert into asm_activity(companyID,zqName,zq,statu) values(" + companyID + ",'30天',30,0)";
                DbHelperSQL.ExecuteSql(sql3);
                string sql4 = "insert into asm_activity(companyID,zqName,zq,statu) values(" + companyID + ",'90天',90,0)";
                DbHelperSQL.ExecuteSql(sql4);
                string sql5 = "insert into asm_activity(companyID,zqName,zq,statu) values(" + companyID + ",'半年(180天)',180,0)";
                DbHelperSQL.ExecuteSql(sql5);
                string sql6 = "insert into asm_activity(companyID,zqName,zq,statu) values(" + companyID + ",'1年',365,0)";
                DbHelperSQL.ExecuteSql(sql6);

                //需要给企业插入模板消息
                string sql_1 = "insert into asm_wxTemplate(templateTitle,templateBH,companyID) values('会员注册','OPENTM203347141',"+companyID+")";
                DbHelperSQL.ExecuteSql(sql_1);
                string sql_2 = "insert into asm_wxTemplate(templateTitle,templateBH,companyID) values('售货机商品支付成功通知','OPENTM401313503'," + companyID + ")";
                DbHelperSQL.ExecuteSql(sql_2);
                string sql_3 = "insert into asm_wxTemplate(templateTitle,templateBH,companyID) values('售货机取货失败通知','OPENTM414811026'," + companyID + ")";
                DbHelperSQL.ExecuteSql(sql_3);
                string sql_4 = "insert into asm_wxTemplate(templateTitle,templateBH,companyID) values('余额变动提醒','OPENTM403148135'," + companyID + ")";
                DbHelperSQL.ExecuteSql(sql_4);
                string sql_5 = "insert into asm_wxTemplate(templateTitle,templateBH,companyID) values('充值成功通知','OPENTM410481462'," + companyID + ")";
                DbHelperSQL.ExecuteSql(sql_5);
                string sql_6 = "insert into asm_wxTemplate(templateTitle,templateBH,companyID) values('密码重置通知','OPENTM406259604'," + companyID + ")";
                DbHelperSQL.ExecuteSql(sql_6);
                string sql_7 = "insert into asm_wxTemplate(templateTitle,templateBH,companyID) values('取货通知','OPENTM407685552'," + companyID + ")";
                DbHelperSQL.ExecuteSql(sql_7);
                context.Response.Write("1");//保存成功
            }
            else {
                context.Response.Write("2");//保存失败
            }

        }
        public void updateCompany(HttpContext context)
        {
            string id = context.Request["id"].ToString();
            string bh = context.Request["bh"].ToString();
            string pwd = context.Request["pwd"].ToString();
            string comName = context.Request["comName"].ToString();
            string fzr = context.Request["fzr"].ToString();
            string cwr = context.Request["cwr"].ToString();
            string phone = context.Request["phone"].ToString();
            string business = context.Request["business"].ToString();
            string code = context.Request["code"].ToString();
            
            string sql1 = "select * from asm_company where name='" + comName + "' and id!="+id;
            DataTable dt1 = DbHelperSQL.Query(sql1).Tables[0];
            if (dt1.Rows.Count > 0)
            {
                context.Response.Write("3");//保存成功
                return;
            }
            string sql23 = "select * from asm_company where code='" + code + "' and id!="+id;
            DataTable dt23 = DbHelperSQL.Query(sql23).Tables[0];
            if (dt23.Rows.Count > 0)
            {
                context.Response.Write("4");//保存成功
                return;
            }
            
            if (pwd!="")
            {
                string update1 = "update asm_company set pwd='"+pwd+"' where id=" + id + "";

                DbHelperSQL.ExecuteSql(update1);
            }
            
            string update = "update asm_company set comBH='"+bh+ "',name='"+comName+ "',linkman='"+fzr+ "',linkPhone='"+phone+ "',business='"+business+"',code='"+code+ "' where id="+id+"";

            int obj = DbHelperSQL.ExecuteSql(update);
            if (obj>0)
            {
                //切需要给该企业
               
                context.Response.Write("1");//保存成功
            }
            else
            {
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
        public void yyzzUploads2(HttpContext context)
        {
            updateCompany(context);
            //HttpFileCollection files = context.Request.Files;
            //string path = "";//照片路径  
            //bool errorflag = true;
            //string tishi = "";
            //if (files.Count > 0)//Form中获取文件对象  
            //{
            //    HttpPostedFile file = files[0];
            //    if (file.ContentLength > 0)//文件大小大于零  
            //    {
            //        string fileName = file.FileName;//文件名  
            //        int fileSize = file.ContentLength;//文件大小  
            //        if (fileName.Substring(fileName.Length - 4, 4).ToLower() == ".jpg")//只支持.jpg文件上传  
            //        {
            //            try
            //            {
            //                Bitmap bitmap = new Bitmap(file.InputStream);//获取图片文件  

            //            }
            //            catch
            //            {
            //                errorflag = false;
            //                tishi = "照片错误，上传文件非图像文件！";
            //            }
            //        }
            //        else
            //        {
            //            errorflag = false;
            //            tishi = "照片格式错误，请上传JPG格式照片文件！";
            //        }
            //    }
            //}
            //else
            //{
            //    errorflag = false;
            //    tishi = "照片错误，上传照片文件为0字节！";
            //}
            //if (errorflag)
            //{
            //    string extension = ".jpg";
            //    dateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            //    path = "../yyzz/" + dateTime + extension;//指定保存路径以及文件名，也就是完整相对路径  
            //    files[0].SaveAs(System.Web.HttpContext.Current.Server.MapPath(path));//保存文件（将相对路径转化为绝对路径）  
            //    context.Response.Write(path);//相应给客户端该照片的相对路径  
            //    logoUploads(context, dateTime);
            //    updateCompany(context);
            //}
            //else
            //{
            //    context.Response.Write("ERR" + tishi);
            //}
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