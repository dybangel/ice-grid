using autosell_center.util;
using Consumer.cls;
using DBUtility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Consumer.api
{
    /// <summary>
    /// czy 的摘要说明
    /// </summary>
    public class czy : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string text = context.Request["action"];
            string key;
            switch (key = text)
            {
                case "login"://登录
                    this.login(context);
                    return;
                case "getMechineList"://机器列表
                    this.getMechineList(context);
                    return;
                case "getProductList"://产品列表列表
                    this.getProductList(context);
                    return;
                case "getProductType"://产品分类
                    this.getProductType(context);
                    return;
                case "setLDProduct"://设置该料道的产品信息
                    this.setLDProduct(context);
                    return;
                case "getLDProductInfo"://获取料道商品及库存
                    this.getLDProductInfo(context);
                    return;
                case "setLDProductInfo"://设置料道商品及库存
                    this.setLDProductInfo(context);
                    return;
                case "getLDInfo"://获取料道信息
                    this.getLDInfo(context);
                    return;
                case "setLDProductNum"://设置料道库存数量
                    this.setLDProductNum(context);
                    return;
                case "getDGNum"://获取该机器订购的数量
                    this.getDGNum(context);
                    return;
                case "getVersion"://获取该机器订购的数量getVersion
                    this.getVersion(context);
                    return;
                case "getMechineInfo"://获取该机器的信息
                    this.getMechineInfo(context);
                    return;
                case "delException"://处理异常状态
                    this.delException(context);
                    return;
                case "getExceptionList"://处理异常状态
                    this.getExceptionList(context);
                    return;
                case "clearLDNO"://清空料道
                    this.clearLDNO(context);
                    return;
                case "getXSE":
                    this.getXSE(context);
                    return;
                case "saveRemoveProductInfo"://下架产品
                    this.saveRemoveProductInfo(context);
                    return;
            }
        }
        public void saveRemoveProductInfo(HttpContext context)
        {
            try
            {
                string operaList = context.Request["operaList"].ToString();
                string operaID = context.Request["operaID"].ToString();//操作员ID
                string companyID = context.Request["companyID"].ToString();//奶企ID
                string token = context.Request["token"].ToString();//操作员token

                string mechineID = context.Request["mechineID"].ToString();
                Util.Debuglog("operaList=" + operaList+ "operaID=" + operaID+ "companyID=" + companyID+ "token=" + token+ "mechineID=" + mechineID, "saveRemoveProductInfo.txt");
                List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
                //去除两端方括号
                operaList = operaList.Replace("[", "");
                operaList = operaList.Replace("]", "");
                //第一次分割
                string[] splitArray = Regex.Split(operaList, "},", RegexOptions.IgnoreCase);
                for (int i = 0; i < splitArray.Length; i++)
                {
                    //去除大括号，好方法去除大括号就用了子串的方式
                    if (i == splitArray.Length - 1)
                    {
                        splitArray[i] = splitArray[i].Substring(1, splitArray[i].Length - 2);
                    }
                    else
                    {
                        splitArray[i] = splitArray[i].Substring(1, splitArray[i].Length-1);
                    }
                    Dictionary<string, object> map = new Dictionary<string, object>();
                    //第二次分割
                    string[] mapArray = splitArray[i].Split(',');
                    for (int j = 0; j < mapArray.Length; j++)
                    {
                        string str = mapArray[j].Replace("\"", "");
                        //第三次分割,为了防止value为空，下面加了一个长度判断
                        string[] keyValue = str.Split(':');
                        if (keyValue.Length == 2)
                        {
                            map.Add(keyValue[0], keyValue[1]);
                        }
                        else {
                            map.Add(keyValue[0], "");
                            
                        }
                    }
                    list.Add(map);
                }
                if (string.IsNullOrEmpty(mechineID))
                {
                    context.Response.Write("{\"code\":\"500\",\"msg\":\"请选择机器\"}");
                    return;
                }
                if (checkToken(operaID, token) == "200")
                {
                    List<string> listsql = new List<string>();

                    for (int i = 0; i < list.Count; i++)
                    {
                        string liststr = "";
                        liststr = liststr + "insert into asm_removeproduct (companyID,mechineID,removeTime,operaID,productID,productName,removeNum,removeRemark) values (" + companyID+","  + mechineID + ",'"+ DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',"+ operaID + ",'" + list[i]["productID"] + "','" + list[i]["proName"] + "'," + list[i]["removeNum"] + ",'" + list[i]["remarks"] + "')  ;";


                       listsql.Add(liststr);





                    }


                    int a = DbHelperSQL.ExecuteSqlTran(listsql);

                    if (a > 0)
                    {
                        context.Response.Write("{\"code\":\"200\",\"msg\":\"设置成功\"}");
                        return;
                    }

                }
                else
                {
                    context.Response.Write("{\"code\":\"400\",\"msg\":\"账号在其他手机登录，请重登\"}");
                    return;
                }
            }
            catch
            {
                context.Response.Write("{\"code\":\"500\",\"msg\":\"参数不全\"}");
                return;
            }
        }
        public void getXSE(HttpContext context)
        {
            try
            {
                string operaID = context.Request["operaID"].ToString();//操作员ID
                string companyID = context.Request["companyID"].ToString();//奶企ID
                string token = context.Request["token"].ToString();//操作员token

                string mechineID = context.Request["mechineID"].ToString();
                string startTime = context.Request["startTime"].ToString();
                string endTime = context.Request["endTime"].ToString();
                if (string.IsNullOrEmpty(mechineID))
                {
                    context.Response.Write("{\"code\":\"500\",\"msg\":\"请选择机器\"}");
                    return;
                }
                if (checkToken(operaID, token) == "200")
                {
                    string sql = "select (select proname from asm_product where productID=s.productID)proname,"
                      +"  (select count(*) dgNum from asm_sellDetail where productID = s.productID and type = 1 and orderTime > '"+startTime+"' and orderTime < '"+endTime+ "' and mechineID="+mechineID+") dgNum,"
                      + "  isnull((select SUM(totalMoney) dgMoney from asm_sellDetail where productID = s.productID and type = 1 and orderTime > '"+startTime+"' and orderTime < '"+endTime+ "' and mechineID=" + mechineID + "),0) dgMoney,"
                      + "  (select count(*) lsNum from asm_sellDetail where productID = s.productID and type = 2 and orderTime> '"+ startTime + "' and orderTime< '"+endTime+ "' and mechineID=" + mechineID + ") lsNum,"
                      + "  isnull((select SUM(totalMoney) lsMoney from asm_sellDetail where productID = s.productID and type = 2 and orderTime > '"+ startTime + "' and orderTime < '"+endTime+ "' and mechineID=" + mechineID + "),0) lsMoney"
                      + "          from  asm_sellDetail s where mechineID = "+mechineID+" and orderTime> '"+ startTime + "' and orderTime< '"+endTime+"'"
                      +"  group by productID";
                    DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                    if (dt.Rows.Count>0)
                    {
                        context.Response.Write("{\"code\":\"200\",\"db\":" + OperUtil.DataTableToJsonWithJsonNet(dt) + "}");
                        return;
                    }
                    context.Response.Write("{\"code\":\"300\",\"msg\":\"暂无记录\"}");
                    return;
                }
                else
                {
                    context.Response.Write("{\"code\":\"400\",\"msg\":\"账号在其他手机登录，请重登\"}");
                    return;
                }
            }
            catch {

            }
        }
        public void clearLDNO(HttpContext context)
        {
            try
            {
                string operaID = context.Request["operaID"].ToString();//操作员ID
                string companyID = context.Request["companyID"].ToString();//奶企ID
                string token = context.Request["token"].ToString();//操作员token

                string mechineID = context.Request["mechineID"].ToString();
                string ldNO = context.Request["ldNO"].ToString();
                if (checkToken(operaID, token) == "200")
                {
                    string sql = "select * from asm_ldinfo where mechineID="+mechineID+" and ldNO='"+ldNO+"'";
                    DataTable dt = DbHelperSQL.Query(sql).Tables[0];

                    
                    Util.asm_ld_change(mechineID, dt.Rows[0]["productID"].ToString(), ldNO, int.Parse(dt.Rows[0]["ld_productNum"].ToString()), "4", -1,operaID);//库存变动记录
                    string update = "update asm_ldinfo set ld_productNum=0,productID='' where mechineID="+mechineID+" and ldNO='"+ldNO+"'";
                    DbHelperSQL.ExecuteSql(update);
                    RedisHelper.Remove(mechineID + "_LDList");
                    if (!string.IsNullOrEmpty(dt.Rows[0]["productID"].ToString()))
                    {
                        updateKC(mechineID,companyID, dt.Rows[0]["productID"].ToString());
                    }
                    ClearRedisProductInfoByMechineID(mechineID);
                    context.Response.Write("{\"code\":\"200\",\"msg\":\"清空成功\"}");
                    return;
                }
                else
                {
                    context.Response.Write("{\"code\":\"400\",\"msg\":\"账号在其他手机登录，请重登\"}");
                    return;
                }
            }
            catch
            {
                context.Response.Write("{\"code\":\"500\",\"msg\":\"系统异常\"}");
                return;
            }
        }
        public void getExceptionList(HttpContext context)
        {
            try
            {
                string operaID = context.Request["operaID"].ToString();//操作员ID
                string companyID = context.Request["companyID"].ToString();//奶企ID
                string token = context.Request["token"].ToString();//操作员token

                string mechineID = context.Request["mechineID"].ToString();
                if (checkToken(operaID, token) == "200")
                {
                   //string sql = "select  * from asm_mechine_statu where mechineID=" + mechineID + " and breakID=2 and statu=0"
                   //              + " union"
                   //              + " select  * from asm_mechine_statu where mechineID = " + mechineID + " and breakID = 3 and statu=0"
                   //              + "union "
                   //              + " select  * from asm_mechine_statu where mechineID = " + mechineID + " and breakID = 4 and statu=0"
                   //             +"union "
                   //             + " select  * from asm_mechine_statu where mechineID = " + mechineID + " and breakID = 5 and statu=0"
                   //             +"union "
                   //             + " select  * from asm_mechine_statu where mechineID = " + mechineID + " and breakID = 6 and statu=0";

                    string sql = "select top 100 asm_breakdown.des, asm_mechine_statu.* from asm_mechine_statu  LEFT JOIN asm_breakdown on  breakID =  asm_breakdown.id where asm_mechine_statu.mechineID=" + mechineID+ "  ORDER BY id desc";
                    DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                    if (dt.Rows.Count>0)
                    {
                        context.Response.Write("{\"code\":\"200\",\"data\":" + Util.DataTableToJsonWithJsonNet(dt) + "}");
                        return;
                    }
                    else
                    {
                        context.Response.Write("{\"code\":\"300\",\"msg\":\"暂无记录\"}");
                        return;
                    }
                }
                else
                {
                    context.Response.Write("{\"code\":\"400\",\"msg\":\"账号在其他手机登录，请重登\"}");
                    return;
                }
            }
            catch {
                context.Response.Write("{\"code\":\"500\",\"msg\":\"系统异常\"}");
                return;
            }
        }
        public void getMechineInfo(HttpContext context)
        {
            try
            {
                string operaID = context.Request["operaID"].ToString();//操作员ID
                string companyID = context.Request["companyID"].ToString();//奶企ID
                string token = context.Request["token"].ToString();//操作员token

                string mechineID = context.Request["mechineID"].ToString();
                if (checkToken(operaID, token) == "200")
                {
                    string sql = string.Format("select id,bh,mechineName,addres,temStatus,netStatus,ldStatus,ISNULL(b.statunum, 0) statunum,"
                                           + " (select isnull(SUM(totalMoney), 0) totalMoney from asm_sellDetail s where type = 2 and s.mechineID = "+mechineID+" and left(convert(varchar, convert(datetime, orderTime), 23), 12) = '"+DateTime.Now.ToString("yyyy-MM-dd")+"') todayMoney,"
                                           + " (select isnull(SUM(totalMoney),0) totalMoney from  asm_sellDetail s where type=2 and s.mechineID="+mechineID+" and DATEDIFF(WEEK,orderTime,GETDATE())=1) lastWeek,"
                                           + " (select isnull(SUM(totalMoney), 0) totalMoney from asm_sellDetail s where type = 2 and s.mechineID = " + mechineID + " and left(convert(varchar, convert(datetime, orderTime), 23), 12) = '"+ DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + "') yesterday,"
                                           + " (select COUNT(*) num from  asm_sellDetail s where type = 2 and s.mechineID = " + mechineID + " and left(convert(varchar, convert(datetime, orderTime), 23), 12)= '"+ DateTime.Now.ToString("yyyy-MM-dd") + "') lsNUm,"
                                           + " (select isnull(SUM(totalMoney), 0) totalMoney from  asm_sellDetail s where type = 1 and s.mechineID = " + mechineID + " and left(convert(varchar, convert(datetime, orderTime), 23), 12)= '"+ DateTime.Now.ToString("yyyy-MM-dd") + "') dgMoney,"
                                           + " (select count(*) num from  asm_orderlistDetail where zt<7 and  mechineID = " + mechineID + " and createTime = '"+ DateTime.Now.ToString("yyyy-MM-dd") + "') dgNum,"
                                           + " (select count(*) from asm_sellDetail where mechineID="+mechineID+" and payType in (1,2,3)) totalCount,"
                                           + " (select count(*) from asm_sellDetail where mechineID=" + mechineID + " and payType in (1,3)) totalUserCount"
                                           + " from  asm_mechine a left join view_mechine_statu_num b on a.id = b.mechineid   where a.id = " + mechineID + "");
                    DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        context.Response.Write("{\"code\":\"200\",\"data\":" + Util.DataTableToJsonWithJsonNet(dt) + "}");
                        return;
                    }
                    else
                    {
                        context.Response.Write("{\"code\":\"300\",\"msg\":\"暂无记录\"}");
                        return;
                    }
                }
                else
                {
                    context.Response.Write("{\"code\":\"400\",\"msg\":\"账号在其他手机登录，请重登\"}");
                    return;
                }
            }
            catch
            {
                context.Response.Write("{\"code\":\"500\",\"msg\":\"参数不全\"}");
                return;
            }
        }
        public void delException(HttpContext context)
        {
            try
            {
                string operaID = context.Request["operaID"].ToString();//操作员ID
                string companyID = context.Request["companyID"].ToString();//奶企ID
                string token = context.Request["token"].ToString();//操作员token

                string mechineID = context.Request["mechineID"].ToString();
                string id = context.Request["id"].ToString();
                if (checkToken(operaID, token) == "200")
                {
                    string sql2 = "select * from asm_mechine_statu where id="+id;
                    DataTable dt2 = DbHelperSQL.Query(sql2).Tables[0];
                    string sql = "update asm_mechine_statu set statu=1 where statu=0 and mechineID="+mechineID+" and id="+id;
                    int a = DbHelperSQL.ExecuteSql(sql);
                    if (a>0)
                    {
                        if (dt2.Rows[0]["breakID"].ToString() == "2")
                        {
                            string sql1 = "update asm_mechine set netStatus=0 where id=" + mechineID;
                            DbHelperSQL.ExecuteSql(sql1);
                            if (mechineID == "68" || mechineID == "69")
                            {
                                string mechineInfo = RedisUtil.getMechine(mechineID);
                                JArray jay = RedisUtil.DeserializeObject(mechineInfo);

                                jay[0]["netStatus"] = 0;

                                RedisHelper.SetRedisModel<string>(mechineID + "_mechineInfoSet", JsonConvert.SerializeObject(jay));

                            }
                        } else if (dt2.Rows[0]["breakID"].ToString() == "3") {
                            string sql1 = "update asm_ldInfo set zt=0 where mechineID=" + mechineID;
                            DbHelperSQL.ExecuteSql(sql1);
                            RedisHelper.Remove(mechineID + "_LDList");
                            string sql3 = "update asm_mechine set ldStatus=0 where id=" + mechineID;
                            DbHelperSQL.ExecuteSql(sql3);
                            if (mechineID == "68" || mechineID == "69")
                            {
                                string mechineInfo = RedisUtil.getMechine(mechineID);
                                JArray jay = RedisUtil.DeserializeObject(mechineInfo);

                                jay[0]["ldStatus"] = 0;

                                RedisHelper.SetRedisModel<string>(mechineID + "_mechineInfoSet", JsonConvert.SerializeObject(jay));

                            }
                        }
                        else if (dt2.Rows[0]["breakID"].ToString() == "4")
                        {
                            string sql1 = "update asm_mechine set temStatus=0 where id=" + mechineID;
                            DbHelperSQL.ExecuteSql(sql1);
                            if (mechineID == "68" || mechineID == "69")
                            {
                                string mechineInfo = RedisUtil.getMechine(mechineID);
                                JArray jay = RedisUtil.DeserializeObject(mechineInfo);

                                jay[0]["temStatus"] = 0;

                                RedisHelper.SetRedisModel<string>(mechineID + "_mechineInfoSet", JsonConvert.SerializeObject(jay));

                            }
                        }

                        ClearRedisProductInfoByMechineID(mechineID);
                        context.Response.Write("{\"code\":\"200\",\"msg\":\"处理成功\"}");
                        return;
                    }
                    context.Response.Write("{\"code\":\"500\",\"msg\":\"处理失败\"}");
                    return;
                }
                else
                {
                    context.Response.Write("{\"code\":\"400\",\"msg\":\"账号在其他手机登录，请重登\"}");
                    return;
                }
            }
            catch
            {
                context.Response.Write("{\"code\":\"500\",\"msg\":\"参数不全\"}");
                return;
            }
        }
        public static void ClearRedisProductInfoByMechineID(string mechineID)
        {
            RedisHelper.SetRedisModel<string>(mechineID + "_productInfo", "", new TimeSpan(0, 0, 0));
        }
        public void getDGNum(HttpContext context)
        {
            try
            {
                string mechineID = context.Request["mechineID"].ToString();
                string companyID = context.Request["companyID"].ToString();//奶企ID
                string sql = "select p.productID,p.proName,p.httpImageUrl,(select count(*) from asm_orderlistDetail o where o.productID=p.productID and mechineID="+mechineID+" and o.zt<7  and createTime = '"+DateTime.Now.ToString("yyyy-MM-dd")+"') num  from  asm_product p where companyID="+ companyID;
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    context.Response.Write("{\"code\":\"200\",\"data\":" + Util.DataTableToJsonWithJsonNet(dt) + "}");
                    return;
                }
                else {
                    context.Response.Write("{\"code\":\"300\",\"msg\":\"暂无记录\"}");
                    return;
                }
            }
            catch {
                context.Response.Write("{\"code\":\"500\",\"msg\":\"参数不全\"}");
                return;
            }
        }
        public void getModel(HttpContext context)
        {
            try
            {
                string operaID = context.Request["operaID"].ToString();//操作员ID
                string companyID = context.Request["companyID"].ToString();//奶企ID
                string token = context.Request["token"].ToString();//操作员token
                if (checkToken(operaID, token) == "200")
                {
                    string sql = string.Format("需要执行的sql");
                    DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        context.Response.Write("{\"code\":\"200\",\"data\":" + Util.DataTableToJsonWithJsonNet(dt) + "}");
                        return;
                    }
                    else
                    {
                        context.Response.Write("{\"code\":\"300\",\"msg\":\"暂无记录\"}");
                        return;
                    }
                }
                else
                {
                    context.Response.Write("{\"code\":\"400\",\"msg\":\"账号在其他手机登录，请重登\"}");
                    return;
                }
            }
            catch
            {
                context.Response.Write("{\"code\":\"500\",\"msg\":\"参数不全\"}");
                return;
            }
        }
        /// <summary>
        /// 设置 库存 
        /// strJson 格式 [{'ldNO':'10','num':'5',productID:'1'},{'ldNO':'11','num':'4',productID:'1'}]
        /// </summary>
        /// <param name="context"></param>
        public void setLDProductNum(HttpContext context)
        {
            try
            {
                string operaID = context.Request["operaID"].ToString();//操作员ID
                string companyID = context.Request["companyID"].ToString();//奶企ID
                string token = context.Request["token"].ToString();//操作员token

                string mechineID = context.Request["mechineID"].ToString();//机器ID
                string strJson = context.Request["numJson"].ToString();

                Util.Debuglog("operaID=" + operaID, "setLDProductNum.txt");
                Util.Debuglog("companyID=" + companyID, "setLDProductNum.txt");
                Util.Debuglog("token=" + token, "setLDProductNum.txt");
                Util.Debuglog("mechineID=" + mechineID, "setLDProductNum.txt");
                Util.Debuglog("strJson=" + strJson, "setLDProductNum.txt");
                if (checkToken(operaID, token) == "200")
                {
                    //清空redis
                    Util.ClearRedisProductInfoByMechineID(mechineID);
                    JArray jArray = (JArray)JsonConvert.DeserializeObject(strJson.Replace(",null","").Replace("null,",""));//jsonArrayText必须是带[]数组格式字符串
                    List<string> list = new List<string>();
                    List<string> list2 = new List<string>();
                    foreach (JObject obj in jArray)
                    {
                        string ldNO = obj["ldNO"].ToString();
                        string num = obj["num"].ToString();
                        string productID = obj["productID"].ToString();
                        Util.Debuglog("ldNO=" + ldNO + ";num=" + num + ";productID=" + productID, "setLDProductNum.txt");
                        //检验每个料道设置的数量
                        string sql = string.Format("select * from asm_ldInfo where ldNO={0} and mechineID={1}", ldNO, mechineID);
                        Util.Debuglog("sql=" + sql, "setLDProductNum.txt");
                        DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                         
                        int status = 1;
                        if (int.Parse(dt.Rows[0]["ld_productNum"].ToString()) > int.Parse(num))
                        {
                            status = -1;
                        }
                        int cha = int.Parse(dt.Rows[0]["ld_productNum"].ToString()) - int.Parse(num);
                        Util.asm_ld_change(mechineID, dt.Rows[0]["productID"].ToString(), ldNO, System.Math.Abs(cha), "1", status, operaID);//库存变动记录
                        list.Add(string.Format("update asm_ldInfo set ld_productNum={0} where ldNO={1} and mechineID={2}", num, ldNO, mechineID));
                        list2.Add(string.Format("insert into [dbo].[asm_ld_day](mechineID,ldNO,productID,num,time) values({0},{1},{2},{3},'{4}')", mechineID, ldNO, productID, num, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                    }

                    
                    int a = DbHelperSQL.ExecuteSqlTran(list);
                    if (a>0)
                    {
                        int b= DbHelperSQL.ExecuteSqlTran(list2);
                        for (int j=0;j<jArray.Count;j++)
                        {
                            string productID = jArray[j]["productID"].ToString();
                            if (!string.IsNullOrEmpty(productID))
                            {
                                updateKC(mechineID, companyID, productID);//更新产品库存
                                //插入提醒 给会员
                                string sqlN = "select o.*,m.mechineName,m.addres from asm_orderlistDetail o left join asm_mechine m on o.mechineID=m.id where o.mechineID = " + mechineID+" and o.zt = 4 and o.productID="+productID;
                                Util.Debuglog("sqlN=" + sqlN, "setLDProductNum.txt");
                                DataTable dn = DbHelperSQL.Query(sqlN).Tables[0];
                                for (int i=0;i<dn.Rows.Count;i++)
                                {
                                    string memberID = dn.Rows[i]["memberID"].ToString();
                                    string bz = "您今日订购的产品已经上货，请前往指定的售卖机取货,取货码:" + dn.Rows[i]["code"].ToString();
                                    string sql1 = "select * from asm_notice where memberID="+memberID+" and con='"+bz+"'";
                                    Util.Debuglog("sql1=" + sql1, "setLDProductNum.txt");
                                    DataTable d1 = DbHelperSQL.Query(sql1).Tables[0];
                                    if (d1.Rows.Count<=0)
                                    {
                                        Util.insertNotice(memberID, "待取货通知", bz, mechineID);
                                        try
                                        {
                                            string sqlM = "select * from asm_member where id=" + dn.Rows[i]["memberID"].ToString();
                                            DataTable dM = DbHelperSQL.Query(sqlM).Tables[0];
                                            if (dM.Rows.Count > 0)
                                            {
                                                if (!string.IsNullOrEmpty(dM.Rows[0]["openID"].ToString()))
                                                {
                                                    wxHelper wx = new wxHelper(companyID);
                                                    string data = TemplateMessage.getProduct(dM.Rows[0]["openID"].ToString(), OperUtil.getMessageID(companyID, "OPENTM416620870"), "亲爱的会员，您今日订购的产品已配送到机器", "" + dn.Rows[i]["mechineName"].ToString() + "",""+ dn.Rows[i]["addres"].ToString() + "", "取货码:"+dn.Rows[i]["code"].ToString()+";请及时取件,否则第二天会自动失效处理");
                                                    TemplateMessage.SendTemplateMsg(wx.IsExistAccess_Token(companyID), data);
                                                }
                                            }
                                        }
                                        catch
                                        {

                                        }
                                    }
                                }
                            }
                        }
                        
                        context.Response.Write("{\"code\":\"200\",\"msg\":\"设置成功\"}");
                        return;
                    }
                }
                else
                {
                    context.Response.Write("{\"code\":\"400\",\"msg\":\"账号在其他手机登录，请重登\"}");
                    return;
                }
            }
            catch(Exception ex)
            {
                Util.Debuglog("ex="+ex.Message, "setLDProductNum.txt");
                context.Response.Write("{\"code\":\"500\",\"msg\":\"参数不全\"}");
                return;
            }
        }
        /// <summary>
        /// 获取料道信息
        /// </summary>
        /// <param name="context"></param>
        public void getLDInfo(HttpContext context)
        {
            try
            {
                string operaID = context.Request["operaID"].ToString();//操作员ID
                string companyID = context.Request["companyID"].ToString();//奶企ID
                string token = context.Request["token"].ToString();//操作员token
                string mechineID = context.Request["mechineID"].ToString();//机器ID
               
                if (checkToken(operaID, token) == "200")
                {
                    string sql = string.Format("select l.ldNO,ldNum,ld_productNum,l.productID,csldNum,yxch,sfqk,zt,proName,shortName,(select brandName from asm_brand where id=p.brandID)  brandName,p.brandID,p.protype,(ldNum-csldNum) dgldNum ,p.httpImageUrl  from asm_ldInfo l left join asm_product p on l.productID=p.productID  where l.mechineID ={0}  and statu = 1 and (p.is_del=0 or p.is_del is null) order by ldno", mechineID);
                    Util.Debuglog(sql, "getLDInfo.txt");
                    DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        context.Response.Write("{\"code\":\"200\",\"data\":" + Util.DataTableToJsonWithJsonNet(dt) + "}");
                        return;
                    }
                    else
                    {
                        context.Response.Write("{\"code\":\"300\",\"msg\":\"暂无记录\"}");
                        return;
                    }
                }
                else
                {
                    context.Response.Write("{\"code\":\"400\",\"msg\":\"账号在其他手机登录，请重登\"}");
                    return;
                }
            }
            catch
            {
                context.Response.Write("{\"code\":\"500\",\"msg\":\"参数不全\"}");
                return;
            }
        }
        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="context"></param>
        public void setLDProductInfo(HttpContext context)
        {
            try
            {
                string operaID = context.Request["operaID"].ToString();//操作员ID
                string companyID = context.Request["companyID"].ToString();//奶企ID
                string token = context.Request["token"].ToString();//操作员token
                string num = context.Request["num"].ToString();//剩余总库存
                string mechineID = context.Request["mechineID"].ToString();//机器ID
                string ldNO = context.Request["ldNO"].ToString();//料道编号
                if (checkToken(operaID, token) == "200")
                {
                    //清空redis
                    Util.ClearRedisProductInfoByMechineID(mechineID);
                    string sql1 = "select * from asm_ldInfo where mechineID=" + mechineID + " and ldNO=" + ldNO;
                    DataTable d1 = DbHelperSQL.Query(sql1).Tables[0];
                    int status = 1;
                    if (int.Parse(d1.Rows[0]["ld_productNum"].ToString())>int.Parse(num))
                    {
                        status = -1;
                    }
                    int cha = int.Parse(d1.Rows[0]["ld_productNum"].ToString()) - int.Parse(num);
                    Util.asm_ld_change(mechineID, d1.Rows[0]["productID"].ToString(),ldNO, System.Math.Abs(cha), "4",status,operaID);//库存变动记录
                    string sql = string.Format("update asm_ldInfo set ld_productNum={0} where mechineID={1} and ldNO={2}",num,mechineID,ldNO);
                    int a= DbHelperSQL.ExecuteSql(sql);
                    if (a > 0)
                    {
                        sql1 = "select * from asm_ldInfo where mechineID="+mechineID+ " and ldNO="+ldNO;
                        d1 = DbHelperSQL.Query(sql1).Tables[0];
                        updateKC(mechineID, companyID, d1.Rows[0]["productID"].ToString());//更新产品库存
                        context.Response.Write("{\"code\":\"200\",\"msg\":\"保存成功\"}");
                        return;
                    }
                    else {
                        context.Response.Write("{\"code\":\"500\",\"msg\":\"保存失败\"}");
                        return;
                    }
                }
                else
                {
                    context.Response.Write("{\"code\":\"400\",\"msg\":\"账号在其他手机登录，请重登\"}");
                    return;
                }
            }
            catch
            {
                context.Response.Write("{\"code\":\"500\",\"msg\":\"参数不全\"}");
                return;
            }
        }
        /// <summary>
        /// 设置料道商品及库存
        /// </summary>
        /// <param name="context"></param>
        public void getLDProductInfo(HttpContext context)
        {
            try
            {
                string operaID = context.Request["operaID"].ToString();//操作员ID
                string companyID = context.Request["companyID"].ToString();//奶企ID
                string token = context.Request["token"].ToString();//操作员token
                string ldNO = context.Request["ldNO"].ToString();//料道编号
                string mechineID = context.Request["mechineID"].ToString();//机器ID
                if (checkToken(operaID, token) == "200")
                {
                    string sql = string.Format("select l.* ,p.proName,(select brandName from  asm_brand where asm_brand.id=p.brandID) brandName,httpImageUrl,(select typeName from asm_protype where asm_protype.productTypeID=p.protype) typeName from asm_ldInfo l left join asm_product p on l.productID=p.productID where l.ldNO='{0}' and l.mechineID={1}", ldNO,mechineID);
                    DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        string productID = dt.Rows[0]["productID"].ToString();
                        if (string.IsNullOrEmpty(productID))
                        {
                            context.Response.Write("{\"code\":\"500\",\"msg\":\"请先为该料道设置产品\"}");
                            return;
                        }
                        //加载该产品的订购未取的信息
                        string sql1 = string.Format("select a.phone , a.nickname,o.zt,t.ztName   from asm_orderlistDetail  o  LEFT JOIN asm_member a on o.memberID=a.id  LEFT JOIN asm_orderDetailType t on o.zt=t.ztID where o.productID={0} and o.mechineID={1} and o.createTime='{2}' order by zt desc  ", productID,mechineID,DateTime.Now.ToString("yyyy-MM-dd"));
                        DataTable dt1 = DbHelperSQL.Query(sql1).Tables[0];
                        //计算剩余库存 4(订购+零售 剩余)/5（预设零售+订购）/15（这件产品所占的总料道数）
                        //预设零售 4(当前剩余零售)/5（预设零售）
                        //寄售库存 4（该产品剩余未卖）/5(当日该产品寄售总量)
                        //订购数量4(该产品当日订购剩余未取)/5(当日该产品订购总量)
                        string sql2 = "select productID,sum(ld_productNum)ld_productNum,sum(csldNum) csldNum,sum(csldNum)-sum(ld_productNum) cha,SUM(ldNum)ldNum  from asm_ldInfo where mechineID=" + mechineID + " and productID='" + productID + "'  group by productID";
                        DataTable dt2 = DbHelperSQL.Query(sql2).Tables[0];
                        string sql3 = "SELECT COUNT(*) dgnum FROM asm_orderlistDetail WHERE createTime='"+DateTime.Now.ToString("yyyy-MM-dd")+"' AND productID="+productID+ " AND zt=4 and mechineID=" + mechineID ;
                        DataTable dt3 = DbHelperSQL.Query(sql3).Tables[0];
                        string sql4 = "SELECT COUNT(*) dgTotal FROM asm_orderlistDetail WHERE createTime='" + DateTime.Now.ToString("yyyy-MM-dd") + "'and zt<7 and  mechineID=" + mechineID+" AND productID=" + productID;
                        DataTable dt4 = DbHelperSQL.Query(sql4).Tables[0];
                        string sql5 = "SELECT COUNT(*) NUM FROM asm_orderlistDetail WHERE  mechineID="+mechineID+" AND productID="+productID+" AND createTime='"+DateTime.Now.ToString("yyyy-MM-dd")+"' AND zt=3";
                        DataTable dt5 = DbHelperSQL.Query(sql5).Tables[0];
                        string sql6 = "SELECT COUNT(*) NUM FROM asm_orderlistDetail WHERE  mechineID=" + mechineID + " AND productID=" + productID + " AND createTime='" + DateTime.Now.ToString("yyyy-MM-dd") + "' AND zt=6";
                        DataTable dt6 = DbHelperSQL.Query(sql6).Tables[0];
                        //当日订购剩余
                        string dgNum = dt3.Rows[0]["dgnum"].ToString();
                        //当日总订购
                        string dgTotal = dt4.Rows[0]["dgTotal"].ToString();
                        //剩余零售=总剩余-剩余订购未取
                        string lssyNum = (int.Parse(dt.Rows[0]["ld_productNum"].ToString()) - int.Parse(dgNum)).ToString();
                        //预设总零售
                        string lsTotal = dt2.Rows[0]["csldNum"].ToString();
                        //该件产品所占的总料道数
                        string totalLDNum = dt2.Rows[0]["ldNum"].ToString();
                        //当日寄售已卖
                        string alreadySell = dt6.Rows[0]["NUM"].ToString(); 
                        //当日寄售总量
                        string Sell= dt5.Rows[0]["NUM"].ToString();
                        string sykc = (int.Parse(dgNum)+int.Parse(lssyNum)).ToString()+"/"+(int.Parse(lsTotal)+int.Parse(dgTotal))+"/"+totalLDNum;
                        string ysls = "0";
                        if (int.Parse(lssyNum) >= 0)
                        {
                             ysls = lssyNum + "/" + lsTotal;
                        }
                        else {
                            ysls =   "0/" + lsTotal;
                        }
                        
                        string jskc = alreadySell+"/"+Sell;
                        string dgsl = dgNum+"/"+dgTotal;
                        context.Response.Write("{\"code\":\"200\",\"data\":" + Util.DataTableToJsonWithJsonNet(dt) + ",\"dgOrder\":"+Util.DataTableToJsonWithJsonNet(dt1)+",\"sykc\":\""+sykc+"\",\"ysls\":\""+ysls+"\",\"jskc\":\""+jskc+"\",\"dgsl\":\""+dgsl+"\"}");
                        return;
                    }
                    else
                    {
                        context.Response.Write("{\"code\":\"300\",\"msg\":\"暂无记录\"}");
                        return;
                    }
                }
                else
                {
                    context.Response.Write("{\"code\":\"400\",\"msg\":\"账号在其他手机登录，请重登\"}");
                    return;
                }
            }
            catch
            {
                context.Response.Write("{\"code\":\"500\",\"msg\":\"参数不全\"}");
                return;
            }
        }
        /// <summary>
        /// 设置该料道的产品信息
        /// </summary>
        /// <param name="context"></param>
        public void setLDProduct(HttpContext context)
        {
            try
            {
                string operaID = context.Request["operaID"].ToString();//操作员ID
                string companyID = context.Request["companyID"].ToString();//奶企ID
                string token = context.Request["token"].ToString();//操作员token
                string ldNO = context.Request["ldNO"].ToString();//料道编号
                string productID = context.Request["productID"].ToString();//产品ID
                string maxNum = context.Request["maxNum"].ToString();//最大容量
                string initNum = context.Request["initNum"].ToString();//初始库存
                string mechineID = context.Request["mechineID"].ToString();//机器ID
                string yxch = context.Request["yxch"].ToString();//订购的优先出货 1 是 0否
                string sfqk = context.Request["sfqk"].ToString();//是否清空料道在补货 1 清空 0否
                Util.Debuglog("operaID=" + operaID + ";companyID=" + companyID + ";token=" + token + ";ldNO=" + ldNO + ";productID="+productID+ ";maxNum="+ maxNum+ ";initNum="+initNum+ ";mechineID="+ mechineID+ ";yxch="+ yxch+ ";sfqk="+ sfqk, "setLDProduct.txt");
                if (checkToken(operaID, token) == "200")
                {
                    //清空redis
                    Util.ClearRedisProductInfoByMechineID(mechineID);
                    string sql = string.Format("select * from asm_ldInfo where ldNO='{0}' and mechineID={1}",ldNO,mechineID);
                    DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["statu"].ToString()!="1")
                        {
                            context.Response.Write("{\"code\":\"500\",\"msg\":\"当前料道没启用\"}");
                            return;
                        }
                        if (int.Parse(maxNum)<int.Parse(initNum))
                        {
                            context.Response.Write("{\"code\":\"500\",\"msg\":\"初始库存不能大于最大库存\"}");
                            return;
                        }
                        //如果产品ID不一致需要在asm_kcDetail 插入一条0000记录
                        if (!string.IsNullOrEmpty(dt.Rows[0]["productID"].ToString())&& dt.Rows[0]["productID"].ToString()!=productID)
                        {
                            string insert = "insert into asm_kcDetail (companyID,mechineID,productID,dateTime,dgNum,lsNum,totalLsNum,imbalance,totalDgNum) "+
                                "values("+companyID+","+mechineID+","+ dt.Rows[0]["productID"].ToString() + ",'"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',0,0,0,0,0)";
                            DbHelperSQL.ExecuteSql(insert);
                            RedisHelper.Remove(mechineID + "_KcProduct");

                        }
                        string update = string.Format("update asm_ldInfo set ldNum={0} ,productID={1},csldNum={2},yxch={3},sfqk={4} where ldNO='{5}' and mechineID='{6}'", maxNum,productID,initNum,yxch,sfqk,ldNO,mechineID) ;
                        int a = DbHelperSQL.ExecuteSql(update);
                        RedisHelper.Remove(mechineID + "_LDList");
                        if (a > 0)
                        {
                            
                            if (!string.IsNullOrEmpty(dt.Rows[0]["productID"].ToString()) && dt.Rows[0]["productID"].ToString() != productID)
                            {
                                updateKC(mechineID, companyID, dt.Rows[0]["productID"].ToString());//更新产品库存
                            }
                            updateKC(mechineID, companyID, productID);//更新产品库存
                           
                            context.Response.Write("{\"code\":\"200\",\"msg\":\"设置成功\"}");
                            return;
                        }
                        else {
                            context.Response.Write("{\"code\":\"500\",\"msg\":\"设置失败\"}");
                            return;
                        }
                    }
                    else {
                        context.Response.Write("{\"code\":\"500\",\"msg\":\"料道信息查询异常\"}");
                        return;
                    }
                }
                else
                {
                    context.Response.Write("{\"code\":\"400\",\"msg\":\"账号在其他手机登录，请重登\"}");
                    return;
                }
            }
            catch
            {
                context.Response.Write("{\"code\":\"500\",\"msg\":\"参数不全\"}");
                return;
            }
        }
        /// <summary>
        /// 获取产品类型列表
        /// </summary>
        /// <param name="context"></param>
        public void getProductType(HttpContext context)
        {
            try
            {
                string operaID = context.Request["operaID"].ToString();//操作员ID
                string companyID = context.Request["companyID"].ToString();//奶企ID
                string token = context.Request["token"].ToString();//操作员token
                Util.Debuglog("operaID="+operaID+ ";companyID="+ companyID+ ";token="+ token, "getProductType.txt");
                if (checkToken(operaID, token) == "200")
                {
                    string sql = string.Format("select * from asm_protype");
                    DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        context.Response.Write("{\"code\":\"200\",\"data\":" + Util.DataTableToJsonWithJsonNet(dt) + "}");
                        return;
                    }
                    else
                    {
                        context.Response.Write("{\"code\":\"300\",\"msg\":\"暂无记录\"}");
                        return;
                    }
                }
                else
                {
                    context.Response.Write("{\"code\":\"400\",\"msg\":\"账号在其他手机登录，请重登\"}");
                    return;
                }
            }
            catch
            {
                context.Response.Write("{\"code\":\"500\",\"msg\":\"参数不全\"}");
                return;
            }
        }
        /// <summary>
        /// 获取产品列表
        /// </summary>
        /// <param name="context"></param>
        public void getProductList(HttpContext context)
        {
            try
            {
                string operaID = context.Request["operaID"].ToString();//操作员ID
                string companyID = context.Request["companyID"].ToString();//奶企ID
                string token = context.Request["token"].ToString();//操作员token
                string type = context.Request["type"].ToString();//产品分类 0代表全部
                if (checkToken(operaID, token) == "200")
                {
                    
                    StringBuilder sb = new StringBuilder();
                    sb.Append("select productID,proName,shortName,(select typeName from asm_protype t where p.protype=t.productTypeID) typeName, (select b.brandName from asm_brand b where p.brandID=b.id) brandName from asm_product p where is_del=0 and companyID=" + companyID);
                    if (type != "0"&&type!="") {
                        sb.Append(" and protype="+type);
                    }
                    DataTable dt = DbHelperSQL.Query(sb.ToString()).Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        context.Response.Write("{\"code\":\"200\",\"data\":" + Util.DataTableToJsonWithJsonNet(dt) + "}");
                        return;
                    }
                    else
                    {
                        context.Response.Write("{\"code\":\"300\",\"msg\":\"暂无记录\"}");
                        return;
                    }
                }
                else
                {
                    context.Response.Write("{\"code\":\"400\",\"msg\":\"账号在其他手机登录，请重登\"}");
                    return;
                }
            }
            catch
            {
                context.Response.Write("{\"code\":\"500\",\"msg\":\"参数不全\"}");
                return;
            }
        }
        /// <summary>
        /// 获取机器列表
        /// </summary>
        /// <param name="context"></param>
        public void getMechineList(HttpContext context)
        {
            try
            {
                string operaID = context.Request["operaID"].ToString();//操作员ID
                string companyID = context.Request["companyID"].ToString();//奶企ID
                string token = context.Request["token"].ToString();//操作员token
                if (checkToken(operaID, token) == "200")
                {
                    string sql = string.Format("select a.*, ISNULL(b.statunum, 0) statunum  from asm_mechine a left join view_mechine_statu_num b on a.id = b.mechineid   where (a.operaID={0} or a.dls={1}) and a.companyID={2}", operaID,operaID,companyID);
                    DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        context.Response.Write("{\"code\":\"200\",\"data\":" + Util.DataTableToJsonWithJsonNet(dt) + "}");
                        return;
                    }
                    else {
                        context.Response.Write("{\"code\":\"300\",\"msg\":\"暂无记录\"}");
                        return;
                    }
                }
                else
                {
                    context.Response.Write("{\"code\":\"400\",\"msg\":\"账号在其他手机登录，请重登\"}");
                    return;
                }
            }
            catch {
                context.Response.Write("{\"code\":\"500\",\"msg\":\"参数不全\"}");
                return;
            }
        }
        /// <summary>
        /// 操作员登录
        /// </summary>
        /// <param name="context"></param>
        public void getVersion(HttpContext context)
        {
            try
            {

                //string sql = string.Format("select top 1 vers version from asm_opera ");
                // DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                string version = ConfigurationManager.ConnectionStrings["version"].ConnectionString;
                string appurl = ConfigurationManager.ConnectionStrings["appurl"].ConnectionString;
                string aa = "{\"version\":\"" + version + "\",\"appurl\":\"" + appurl + "\"}";
                context.Response.Write("{\"code\":\"200\",\"data\":" + aa + "}");
                return;
              
            }
            catch
            {
                context.Response.Write("{\"code\":\"500\",\"msg\":\"系统异常\"}");
                return;
            }
        }
        /// <summary>
        /// 操作员登录
        /// </summary>
        /// <param name="context"></param>
        public void login(HttpContext context)
        {
            try
            {
                string uname = context.Request["uname"].ToString();//用户名
                string pwd = context.Request["pwd"].ToString();//密码
                string sql = string.Format("select * from asm_opera where name='{0}' and pwd='{1}'", uname, pwd);
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["appQX"].ToString()=="0"||string.IsNullOrEmpty(dt.Rows[0]["appQX"].ToString()))
                    {
                        context.Response.Write("{\"code\":\"500\",\"msg\":\"当前没有登录APP权限\"}");
                        return;
                    }
                    updateToken(dt.Rows[0]["id"].ToString());
                    dt = DbHelperSQL.Query(sql).Tables[0];
                    context.Response.Write("{\"code\":\"200\",\"data\":" + Util.DataTableToJsonWithJsonNet(dt) + "}");
                    return;
                }
                else
                {
                    context.Response.Write("{\"code\":\"500\",\"msg\":\"用户名或密码错误\"}");
                    return;
                }
            }
            catch {
                context.Response.Write("{\"code\":\"500\",\"msg\":\"系统异常\"}");
                return;
            }
        }
        public void updateToken(string id)
        {
            string sql = string.Format("update asm_opera set token='{0}' where id={1}", Guid.NewGuid().ToString("N"),id);
            DbHelperSQL.ExecuteSql(sql);
        }
        public string checkToken(string id,string token)
        {
            try
            {
                string sql = string.Format("select * from asm_opera where id={0} and token='{1}'", id, token);
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {

                    return "200";
                }
                else
                {
                    return "500";
                }
            }
            catch {
                return "500";
            }
        }
        /// <summary>
        /// 库存变动记录表 会员购买， 配送员加货，
        /// </summary>
        /// <param name="mechineID"></param>
        /// <param name="companyID"></param>
        /// <param name="productID"></param>
        public void updateKC(string mechineID,string companyID,string productID)
        {
            Util.Debuglog("mechineID="+ mechineID+ ";companyID="+ companyID+ ";productID="+ productID, "库存变动.txt");
            string sql= "select productID,sum(ld_productNum)ld_productNum,sum(csldNum) csldNum,sum(csldNum)-sum(ld_productNum) cha from asm_ldInfo where mechineID=" + mechineID+ " and productID='"+productID+"'  group by productID";
            Util.Debuglog("sql=" + sql, "库存变动.txt");
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count>0)
            {
                string orderDetail = "select COUNT(*) num from asm_orderlistDetail where createTime='"+DateTime.Now.ToString("yyyy-MM-dd")+ "' and mechineID="+mechineID+" and productID=" + productID+" and zt=4";
                Util.Debuglog("orderDetail=" + orderDetail, "库存变动.txt");
                DataTable dtDetail = DbHelperSQL.Query(orderDetail).Tables[0];
                string orderDetailTotal = "select COUNT(*) num from asm_orderlistDetail where createTime='" + DateTime.Now.ToString("yyyy-MM-dd") + "' and zt<7 and mechineID=" + mechineID + " and productID=" + productID ;
               
                DataTable dtDetailT = DbHelperSQL.Query(orderDetailTotal).Tables[0];
                //剩余订购
                string dgNum = dtDetail.Rows[0]["num"].ToString();
                //剩余零售=总剩余-剩余订购
                string lsNum =(int.Parse(dt.Rows[0]["ld_productNum"].ToString())-int.Parse(dgNum)).ToString() ;
                //string sql2 = "select COUNT(*) num from asm_orderlistDetail where mechineID="+mechineID+" and createTime='"+DateTime.Now.ToString("yyyy-MM-dd")+"' group by productID";
                //Util.Debuglog("sql2=" + sql2, "库存变动.txt");
                //DataTable d2 = DbHelperSQL.Query(sql2).Tables[0];
                
                //totalLsNum 计算规则 用该产品所占料道的最大初始料道数量之和 -当日订购的数量=总的零售数量
                string totalLsNum = (int.Parse(dt.Rows[0]["csldNum"].ToString())).ToString();
                string cha = (int.Parse(totalLsNum) - int.Parse(lsNum)).ToString();
                if (int.Parse(cha)<0)
                {
                    cha = "0";
                }
                string insert = "insert into asm_kcDetail(companyID,mechineID,productID,dateTime,dgNum,lsNum,totalLsNum,imbalance,totalDgNum) "
                   + " values('"+companyID+"','"+mechineID+"','"+productID+"','"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+dgNum+"','"+lsNum+"','"+totalLsNum+"','"+cha+"','"+ dtDetailT.Rows[0]["num"].ToString() + "')";
                Util.Debuglog("insert=" + insert, "库存变动.txt");
                if (int.Parse(lsNum)>=0)
                {
                    DbHelperSQL.ExecuteSql(insert);
                    RedisHelper.Remove(mechineID + "_KcProduct");
                }
               
            }
            Util.ClearRedisProductInfo(companyID);//清空缓存
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