using autosell_center.cls;
using autosell_center.util;
using autosell_center.WXPay;
using Common;
using Consumer.cls;
using DBUtility;
using Maticsoft.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Xml;
using uniondemo.com.allinpay.syb;

namespace autosell_center.api
{
    /// <summary>
    /// mechineService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://nq.bingoseller.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class mechineService : System.Web.Services.WebService
    {
        public static string NQ_URL = "http://nq.bingoseller.com/";
        public static string WX_URL = "http://wx.bingoseller.com/";
        //测试用
        [WebMethod]
        public string getCardInfo(string cardno) {
            return "876919065";
        }
        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }
        [WebMethod]
        //获取机器指定产品库存
        public string getKC(string mechineID, string productID, string dgOrderDetailID)
        {
            Util.Debuglog("mechineID=" + mechineID + ";productID=" + productID + ";dgOrderDetailID=" + dgOrderDetailID, "查看库存.txt");
            if (!string.IsNullOrEmpty(dgOrderDetailID) && dgOrderDetailID != "0")
            {
                string sql = "SELECT * FROM asm_orderlistDetail where id=" + dgOrderDetailID;
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["zt"].ToString() == "3")
                    {
                        return "200";
                    }
                    else {
                        return "100";//该笔订单状态不正确
                    }
                }
                else {
                    return "300";//没有查询到该笔售卖信息
                }
            }
            else {
                string result = getLDNO(mechineID, productID);
                if (string.IsNullOrEmpty(result))
                {
                    Util.ClearRedisProductInfoByMechineID(mechineID);
                    return "500";
                }
                else
                {
                    return "200";
                }
            }

        }
        //[WebMethod]
        ////获取预生成订单号
        //public string getReqsn(string mechineID, string productID)
        //{
        //    string reqsn=mechineID +DateTime.Now.ToFileTime().ToString();
        //    RedisHelper.SetRedisModel<string>(reqsn, "0", new TimeSpan(0, 30, 0));
        //    string sqlInsert = "insert into asm_product_pick (mechineID,productID,pickProductTime,reqsnNo) values("+ mechineID + ","+ productID + ",'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+ reqsn +"') ";
        //    Util.Debuglog("sqlInsert=" + sqlInsert, "获取预生成订单号.txt");
        //    DbHelperSQL.ExecuteSql(sqlInsert);
        //    return reqsn;
        //}
        [WebMethod]
        //获取预生成订单号
        public string getReqsn(string companyID, string mechineID, string productID, string sftj, string product, string dgOrderDetailID, string type)
        {
            string reqsn = mechineID +"_"+ DateTime.Now.ToFileTime().ToString();
          
            string sqlInsert = "insert into asm_product_pick (companyID,mechineID,productID,sftj,product,dgOrderDetailID,type,pickProductTime,reqsnNo) values(" + companyID + "," + mechineID + "," + productID + ",'" + sftj + "','" + product + "','" + dgOrderDetailID + "','" + type + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + reqsn + "') ";
            Util.Debuglog("sqlInsert=" + sqlInsert, "获取预生成订单号.txt");
            DbHelperSQL.ExecuteSql(sqlInsert);
            return reqsn;
        }
        /// <summary>
        /// 零售时候 获取出货料道
        /// </summary>
        /// <param name="mechineID"></param>
        /// <param name="productID"></param>
        /// <returns></returns>
        public static string getLDNO(string mechineID, string productID)
        {
            Util.Debuglog("mechineID=" + mechineID + ";productID=" + productID, "获取料道.txt");
            string sql = "select SUM(ld_productNum) ld_productNum from  asm_ldinfo where mechineID=" + mechineID + " and productID=" + productID + " and ld_productNum>0 and (zt is null or zt=0) group by productID";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                string sql1 = "select COUNT(*) num  from asm_orderlistDetail where mechineID=" + mechineID + " and productID=" + productID + " and zt=4 and createTime='" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
                DataTable d1 = DbHelperSQL.Query(sql1).Tables[0];
                if (int.Parse(dt.Rows[0]["ld_productNum"].ToString()) <= int.Parse(d1.Rows[0]["num"].ToString()))
                {
                    return null;
                }
                string sql3 = "select * from  asm_ldinfo where mechineID=" + mechineID + " and productID=" + productID + " and ld_productNum>0 and (zt is null or zt=0) order by yxch";
                DataTable d3 = DbHelperSQL.Query(sql3).Tables[0];
                Util.Debuglog("mechineID=" + mechineID + ";productID=" + productID + ";ldno=" + d3.Rows[0]["ldNO"].ToString(), "获取料道.txt");
                return d3.Rows[0]["ldNO"].ToString();
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 获取订购料道编号
        /// </summary>
        /// <param name="mechineID"></param>
        /// <param name="productID"></param>
        /// <returns></returns>
        public static string getDGLDNO(string mechineID, string productID)
        {
            try
            {
                string sql = "SELECT * from asm_ldInfo where  mechineID=" + mechineID + " AND productID=" + productID + " and ld_productNum>0 and (zt is null or zt=0) ORDER BY yxch DESC";
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return dt.Rows[0]["ldNO"].ToString();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
        //根据机器编号获取商品列表
        [WebMethod]
        public string getProductList(string mechineBH)//迎春乐专用
        {
            string sql1 = "select * from asm_mechine where bh='" + mechineBH + "'";
            OperUtil.Debuglog("sql1=" + sql1, "_获取商品信息.txt");
            DataTable dt1 = DbHelperSQL.Query(sql1).Tables[0];
            if (dt1.Rows.Count > 0)
            {
                string sql2 = "select * from (select *,isnull((select sum(ld_productNum) from asm_ldInfo ald where ald.productID=ap.productID and type=2 and ald.statu=1 and ald.mechineID=" + dt1.Rows[0]["id"].ToString() + "),0) num from asm_product ap where ap.companyID=" + dt1.Rows[0]["companyID"].ToString() + ") A where num>=0 and productID in(select productID from asm_ldInfo where mechineID=" + dt1.Rows[0]["id"].ToString() + " and productID!='')";
                DataTable dt = DbHelperSQL.Query(sql2).Tables[0];
                OperUtil.Debuglog("sql=" + sql2, "_获取商品信息.txt");
                if (dt.Rows.Count > 0)
                {
                    OperUtil.Debuglog("sql=" + DataTableToJsonWithJsonNet(dt), "_获取商品信息.txt");
                    return DataTableToJsonWithJsonNet(dt);
                }
            }
            return "1";
        }
        //获取会员价
        public string getMemberprice(string companyID)
        {
            string memberprice = RedisHelper.GetRedisModel<string>(companyID + "_memberprice");
            if (string.IsNullOrEmpty(memberprice))
            {
                string sql3 = "select * from asm_tqlist where companyID=" + companyID;
                DataTable d3 = DbHelperSQL.Query(sql3).Tables[0];
                if (d3.Rows.Count > 0)
                {
                    RedisHelper.SetRedisModel<string>(companyID + "_memberprice", d3.Rows[0]["memberprice"].ToString(), new TimeSpan(4, 0, 0));
                    memberprice = d3.Rows[0]["memberprice"].ToString();
                }
                else {
                    RedisHelper.SetRedisModel<string>(companyID + "_memberprice", "0", new TimeSpan(4, 0, 0));
                    memberprice = "0";
                }
              
            }
            return memberprice;
           
        }
        [WebMethod]
        //获取产品列表
        public string getProductList2(string mechineID)
        {
            Util.log(mechineID, "getProductList2.txt");
            string mechineInfo = RedisHelper.GetRedisModel<string>(mechineID + "_mechineInfo");
            if (string.IsNullOrEmpty(mechineInfo))
            {
                string sql = "select am.*,ac.p1,ac.p2,ac.p3,ac.p4,ac.p5,ac.p6,ac.p7,ac.p8,ac.p9,ac.p10,am.setTem from asm_mechine am left join asm_company ac on am.companyID=ac.id where am.id='" + mechineID + "'";
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                   
                    RedisHelper.SetRedisModel<string>(mechineID + "_mechineInfo", OperUtil.DataTableToJsonWithJsonNet(dt), new TimeSpan(0, 10, 0));
                    mechineInfo = OperUtil.DataTableToJsonWithJsonNet(dt);
                }
            }


            if (!string.IsNullOrEmpty(mechineInfo))
            {
                JArray jArray = (JArray)JsonConvert.DeserializeObject(mechineInfo);
                string companyID = jArray[0]["companyID"].ToString();

                //根据限时活动到期 清空

                string result = RedisHelper.GetRedisModel<string>(mechineID + "_productInfo");
                // DataTable dresult= JsonToDataTable(result);
                if (string.IsNullOrEmpty(result))
                {
                    string sql2 = "";
                    //排序规则 按照权重优先 库存为0 的时候权重设置为-100    会员售卖的排最前权重设置200
                    //if (mechineID == "67") {//有商品，但是被订购，零售为0，显示到机器但标注为售空
                    sql2 = @"select * from (
                        (SELECT p.productID, p.proName, o.sellPrice AS price0, o.sellPrice AS price1, o.sellPrice AS price2, o.sellPrice AS price3, p.path, p.protype, p.mechineID, p.description, p.productSize, p.bzq,
                            p.companyID, p.ljxs, p.httpImageUrl, p.sluid, p.progg, p.brandID, p.shortName, p.bh, p.tag, p.dstype, p.startSend, p.is_del, '200' weight, '100' num, '3' type, o.id, '' AS sftj
                        FROM asm_product p RIGHT JOIN asm_orderlistDetail o ON p.productID = o.productID WHERE o.zt = 3 AND o.createTime = '@createTime' AND o.mechineID =@mechineID
                        )
                        UNION
                        (select p.productID, p.proName, p.price0, p.price1, p.price2, p.price3, p.path, p.protype, p.mechineID, p.description, p.productSize, p.bzq,
                        p.companyID, p.ljxs, p.httpImageUrl, p.sluid, p.progg, p.brandID, p.shortName, p.bh, p.tag, p.dstype, p.startSend, p.is_del,
                                CASE WHEN lsNum <= 0 THEN

                                '-100'  ELSE        p.weight    END weight, lsNum num,  '2' type,'' id, '' sftj from (select t.* from
                        (select asm_kcDetail.*,row_number() over (partition by productID order by id desc) rn 
                        from asm_kcDetail where mechineID=@mechineID) t
                        where rn=1) b left
                                                                                                                join asm_product p on b.productID = p.productID  where

                            p.is_del = 0        AND p.dstype IN (2, 3)		AND p.companyID = @companyID and b.productID IN (SELECT          productID       FROM            asm_ldInfo      WHERE           mechineID =@mechineID

                        AND zt != 1     AND productID != '' ) )) s order by s.weight desc";
                    sql2 = sql2.Replace("@createTime", DateTime.Now.ToString("yyyy-MM-dd"));
                    sql2 = sql2.Replace("@mechineID", mechineID);
                    sql2 = sql2.Replace("@companyID", companyID);
                    //} else {
                    //    sql2 = "SELECT p.productID,p.proName,o.sellPrice as price0,o.sellPrice as price1,o.sellPrice as price2,o.sellPrice as price3,p.path,p.protype,p.mechineID,p.description,p.productSize,p.bzq,p.companyID,p.ljxs,p.httpImageUrl,p.sluid,p.progg,"
                    //        + " p.brandID,p.shortName,p.bh,p.tag,p.dstype,p.startSend,p.is_del,'200' weight,"
                    //        + " '100' num,'3' type,o.id,'' as sftj FROM asm_product p RIGHT JOIN asm_orderlistDetail o ON p.productID=o.productID where o.zt=3 AND o.createTime='" + DateTime.Now.ToString("yyyy-MM-dd") + "' AND o.mechineID=" + mechineID
                    //        + "  UNION"
                    //        + "  SELECT productID,	proName,	price0,	price1,	price2,	price3,	path,	protype,	mechineID,	description,	productSize,	bzq,	companyID,	ljxs,	httpImageUrl,	sluid,	progg,	brandID,	shortName,	bh,	tag,	dstype,	startSend,	is_del,	"
                    //        + " CASE WHEN num <= 0 then '-100' ELSE weight END weight,"
                    //        + "  num, type, id, sftj  FROM"
                    //        + "  (SELECT *, isnull((SELECT SUM(ld_productNum)FROM asm_ldInfo ald WHERE ald.productID = ap.productID AND ald.statu = 1 and ald.zt!=1 AND ald.mechineID = " + mechineID + "), 0) num,"
                    //        + "     '2' type,'' as id,''as sftj FROM asm_product ap WHERE ap.is_del=0 and  ap.dstype in(2,3) and ap.companyID = " + companyID + ") A"
                    //        + "  WHERE"
                    //        + "    productID IN("
                    //        + "      SELECT"
                    //        + "          productID"
                    //        + "      FROM"
                    //        + "          asm_ldInfo"
                    //        + "      WHERE"
                    //        + "          mechineID = " + mechineID
                    //        + "      and zt!=1 AND productID != '')  order by  weight desc";
                    //}
                   
                    
                    
                    Util.log(mechineID + "sql2" + sql2, "getProductList2.txt");
                    DataTable dt = DbHelperSQL.Query(sql2).Tables[0];

                    if (dt.Rows.Count > 0)
                    {
                        //查询type=2 即 零售产品 是否有限时活动 有的话 修改对的产品价格 以及sftj 标识=1
                        result = OperUtil.DataTableToJsonWithJsonNet(dt);
                        result = getNewProductList(result, mechineID);
                        //result = DataTableToJsonWithJsonNet(dt);
                        //保存到redis 到修改产品列表里需要清空redis 涉及到的表asm_product（后台）  asm_orderlistDetail（微信端会员接口 后台global）  asm_ldInfo（微信端操作员接口 后台global）
                        //long times=Util.GetTimeStamp();
                        //string zz = "{\"db\":" + result + " ,\"productlistno\":" + times + "}";
                        //RedisHelper.SetRedisModel<string>(mechineID + "_productInfo", result, new TimeSpan(4, 0, 0));
                        RedisHelper.SetRedisModel<string>(mechineID + "_productInfo", result, new TimeSpan(4, 0, 0));
                        Util.log(mechineID+"数据库result"+ result, "getProductList2.txt");
                        RedisHelper.SetRedisModel<string>(companyID + "_memberprice", getMemberprice(companyID), new TimeSpan(4, 0, 0));
                        return "{\"code\":\"200\",\"db\":" + result + ",\"priceSwitch\":\"" + getMemberprice(companyID) + "\"}";
                    }
                }
                else
                {
                    result = getNewProductList(result, mechineID);
                    Util.log(mechineID + "缓存result" + result, "getProductList2.txt");
                    return "{\"code\":\"200\",\"db\":" + result + ",\"priceSwitch\":\"" + getMemberprice(companyID) + "\"}";
                }
            }
            return "{\"code\":\"500\",\"msg\":\"暂无记录\"}";
        }
       
        /// <summary>
        /// </summary>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <returns></returns>
        public string getNewProductList(string json, string mechineID)
        {
            JArray jsonArray = (JArray)JsonConvert.DeserializeObject(json);
            if (jsonArray.Count > 0)
            {
                for (int i = 0; i < jsonArray.Count; i++)
                {
                    string productID = jsonArray[i]["productID"].ToString();

                    string result = RedisHelper.GetRedisModel<string>(mechineID + "_xstj");
                    if (string.IsNullOrEmpty(result))
                    {
                       // Util.Debuglog("数据库读取=" + result, "getNewProductList2.txt");
                        string sql1 = "select * from asm_xstj where mechineID=" + mechineID + "  order by timeSpan desc"; 
                        DataTable d1 = DbHelperSQL.Query(sql1).Tables[0];
                        if (d1.Rows.Count > 0)
                        {
                            RedisHelper.SetRedisModel<string>(mechineID+ "_xstj", OperUtil.DataTableToJsonWithJsonNet(d1), new TimeSpan(12, 0, 0));
                            result = OperUtil.DataTableToJsonWithJsonNet(d1);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    JArray jo = (JArray)JsonConvert.DeserializeObject(result);
                    for (int n = 0; n < jo.Count; n++)
                    {
                        string _productID = jo[n]["productID"].ToString();//限时活动产品ID
                        string type = jo[n]["type"].ToString();
                        string startTime = jo[n]["startTime"].ToString();
                        string endTime = jo[n]["endTime"].ToString();
                        string timeSpan = jo[n]["timeSpan"].ToString();
                        string price0 = jo[n]["price0"].ToString();
                        string price1 = jo[n]["price1"].ToString();
                        string price2 = jo[n]["price2"].ToString();
                        string price3 = jo[n]["price3"].ToString();
                        string companyID = jo[n]["companyID"].ToString();
                        if (productID == _productID)//存在限时活动 判断是周期活动或者是阶段活动
                        { 
                            if (DateTime.Parse(startTime) < DateTime.Now && DateTime.Parse(endTime) > DateTime.Now)//有效期内
                            {
                                if (type == "1")//周期特价
                                {
                                    string time1 = timeSpan.Split('-')[0];
                                    string time2 = timeSpan.Split('-')[1]; 
                                    if (DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " " + time1 + ":00") < DateTime.Now && DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " " + time2 + ":00") > DateTime.Now)
                                    {
                                        jsonArray[i]["price0"] = price0;
                                        jsonArray[i]["price1"] = price1;
                                        jsonArray[i]["price2"] = price2;
                                        jsonArray[i]["price3"] = price3;
                                        jsonArray[i]["sftj"] = "1";
                                        break;
                                    }
                                    else
                                    {
                                        //修改成原价
                                        
                                        JObject jpinfo= Util.getProductInfo(productID,companyID);
                                        jsonArray[i]["price0"] =jpinfo["price0"];
                                        jsonArray[i]["price1"] = jpinfo["price1"];
                                        jsonArray[i]["price2"] = jpinfo["price2"];
                                        jsonArray[i]["price3"] = jpinfo["price3"];
                                        jsonArray[i]["sftj"] = "";
                                    }
                                }
                                else if (type == "2")//阶段特价
                                {
                                     
                                    jsonArray[i]["price0"] = price0;
                                    jsonArray[i]["price1"] = price1;
                                    jsonArray[i]["price2"] = price2;
                                    jsonArray[i]["price3"] = price3;
                                    jsonArray[i]["sftj"] = "1"; 
                                    break;
                                }
                            }
                            else
                            {
                                JObject jpinfo = Util.getProductInfo(productID, companyID);
                                jsonArray[i]["price0"] = jpinfo["price0"];
                                jsonArray[i]["price1"] = jpinfo["price1"];
                                jsonArray[i]["price2"] = jpinfo["price2"];
                                jsonArray[i]["price3"] = jpinfo["price3"];
                                jsonArray[i]["sftj"] = "";
                            }

                        }
                    }

                }
            }

            return jsonArray.ToString();
        }
        //获取销售统计
        public DataTable getXS(DataTable dt,string mechineID)
        {
            if (dt.Rows.Count>0)
            {
                string companyID = dt.Rows[0]["companyID"].ToString();
                string xstjList = RedisHelper.GetRedisModel<string>(companyID + "_xstj");
                if (string.IsNullOrEmpty(xstjList))
                {
                    string sql2 = "select * from asm_xstj where companyID="+companyID;
                    DataTable d2 = DbHelperSQL.Query(sql2).Tables[0];
                    if (d2.Rows.Count>0)
                    {
                        RedisHelper.SetRedisModel<string>(companyID + "_xstj",OperUtil.DataTableToJsonWithJsonNet(d2),new TimeSpan(12,0,0));
                        xstjList = OperUtil.DataTableToJsonWithJsonNet(d2);
                    }
                }
                if (string.IsNullOrEmpty(xstjList))
                {
                    return dt;
                }
                JArray jArray = (JArray)JsonConvert.DeserializeObject(xstjList);
                for (int i=0;i<dt.Rows.Count;i++)
                {
                    if (dt.Rows[i]["type"].ToString()=="2")
                    {
                        // 查询是否有限时活动
                        for (int m=0;m<jArray.Count;m++)
                        {
                            string productID = dt.Rows[i]["productID"].ToString();
                            string _productID = jArray[m]["productID"].ToString();//限时活动产品ID
                            string type= jArray[m]["type"].ToString();
                            string startTime = jArray[m]["startTime"].ToString();
                            string endTime = jArray[m]["endTime"].ToString();
                            string timeSpan= jArray[m]["timeSpan"].ToString();
                            string price0= jArray[m]["price0"].ToString();
                            string price1 = jArray[m]["price1"].ToString();
                            string price2 = jArray[m]["price2"].ToString();
                            string price3 = jArray[m]["price3"].ToString();
                            if (productID==_productID)//存在限时活动 判断是周期活动或者是阶段活动
                            { 
                                if (DateTime.Parse(startTime)<DateTime.Now&&DateTime.Parse(endTime)>DateTime.Now)//有效期内
                                {
                                    if (type == "1")//周期特价
                                    {
                                        string time1 = timeSpan.Split('-')[0];
                                        string time2 = timeSpan.Split('-')[1];
                                        Util.Debuglog("time1=" + DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " " + time1 + ":00"), "getXS.txt");
                                        Util.Debuglog("time2=" + DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " " + time2 + ":00"), "getXS.txt");
                                        Util.Debuglog("now=" + DateTime.Now, "getXS.txt");
                                        if (DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")+" "+time1+":00")<DateTime.Now&& DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " " + time2 + ":00")>DateTime.Now)
                                        {
                                            dt.Rows[i]["price0"] = price0;
                                            dt.Rows[i]["price1"] = price1;
                                            dt.Rows[i]["price2"] = price2;
                                            dt.Rows[i]["price3"] = price3;
                                            dt.Rows[i]["sftj"] = "1"; 
                                        }
                                    } else if (type=="2")//阶段特价
                                    {
                                        dt.Rows[i]["price0"] = price0;
                                        dt.Rows[i]["price1"] = price1;
                                        dt.Rows[i]["price2"] = price2;
                                        dt.Rows[i]["price3"] = price3;
                                        dt.Rows[i]["sftj"] = "1"; 
                                    }
                                }

                            }
                        }
                         
                    }
                }
            }
            RedisHelper.SetRedisModel<string>(mechineID + "_productInfo", OperUtil.DataTableToJsonWithJsonNet(dt), new TimeSpan(1, 0, 0));
            return dt;
        }
       
        [WebMethod]
        //订购出货
        public string dgCh(string code,string mechineID)
        {
            Util.log("code:"+code+ "mechineID:"+ mechineID, "订购出货.txt");
            try
            {
                int a;
                if (!int.TryParse(code,out a)) {
                    return "{ \"code\" : \"500\", \"msg\" : \"请输入取货码\" }";
                }
                string count = RedisHelper.GetRedisModel<string>("code" + mechineID);
                if (string.IsNullOrEmpty(count))
                {
                    RedisHelper.SetRedisModel<string>("code" + mechineID, "1", new TimeSpan(0, 1, 0));
                }
                else
                {
                    if (int.Parse(count) > 3)
                    {
                        return "{ \"code\" : \"500\", \"msg\" : \"当前操作过于频繁1分钟后重试\" }";
                    }
                    count = (int.Parse(count) + 1).ToString();
                    RedisHelper.SetRedisModel<string>("code" + mechineID, count, new TimeSpan(0, 1, 0));
                }
                Util.log("1code:" + code + "mechineID:" + mechineID, "订购出货.txt");

                if (mechineID == "68" || mechineID == "69")
                {
                    string _mechineInfo = RedisUtil.getMechine(mechineID);
                    JArray _mechineJArray = RedisUtil.DeserializeObject(_mechineInfo);
                    if (_mechineJArray != null)
                    {
                        if (mechineID != "25")
                        {

                            if (_mechineJArray[0]["openStatus"].ToString() == "1")
                            {
                                return "{ \"code\" : \"500\", \"msg\" : \"机器暂停营业中\" }";
                            }
                        }

                        if (_mechineJArray[0]["netStatus"].ToString() == "1" || _mechineJArray[0]["gkjStatus"].ToString() == "1")
                        {
                            return "{ \"code\" : \"500\", \"msg\" : \"机器暂停营业中\" }";
                        }
                        if (_mechineJArray[0]["updateSoftStatus"].ToString() != "0")
                        {
                            return "{ \"code\" : \"500\", \"msg\" : \"机器暂停营业中\" }";
                        }
                    }
                }
                else {
                    //先查询机器是否营业中
                    string mechineInfo = RedisHelper.GetRedisModel<string>(mechineID + "_mechineInfo");
                    if (string.IsNullOrEmpty(mechineInfo))
                    {
                        string sqlM = "select am.*,ac.p1,ac.p2,ac.p3,ac.p4,ac.p5,ac.p6,ac.p7,ac.p8,ac.p9,ac.p10,am.setTem from asm_mechine am left join asm_company ac on am.companyID=ac.id where am.id='" + mechineID + "'";
                        DataTable dtM = DbHelperSQL.Query(sqlM).Tables[0];
                        if (dtM.Rows.Count > 0)
                        {

                            RedisHelper.SetRedisModel<string>(mechineID + "_mechineInfo", OperUtil.DataTableToJsonWithJsonNet(dtM), new TimeSpan(2, 0, 0));
                            mechineInfo = OperUtil.DataTableToJsonWithJsonNet(dtM);
                        }
                    }
                    if (!string.IsNullOrEmpty(mechineInfo))
                    {
                        JArray jArray = (JArray)JsonConvert.DeserializeObject(mechineInfo);
                        string openStatus = jArray[0]["openStatus"].ToString();
                        if (openStatus == "1")
                        {
                            return "{ \"code\" : \"500\", \"msg\" : \"机器暂停营业中\" }";
                        }
                    }
                }
               
                string sql1 = "select * from asm_orderlistDetail where code='" + code + "'  AND createTime='" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
                DataTable d1 = DbHelperSQL.Query(sql1).Tables[0];
                if (d1.Rows.Count <= 0)
                {
                    return "{ \"code\" : \"500\", \"msg\" : \"当前取货码不存在\" }";
                }



                if (d1.Rows[0]["zt"].ToString() != "4")
                {

                    return "{ \"code\" : \"500\", \"msg\" : \"当前取货码已使用\" }";
                }

                if (d1.Rows[0]["statu"].ToString() != "0")
                {

                    return "{ \"code\" : \"500\", \"msg\" : \"当前取货码已使用\" }";
                }
                if (mechineID != d1.Rows[0]["mechineID"].ToString())
                {
                    return "{ \"code\" : \"500\", \"msg\" : \"当前取货码不属于该机器\" }";

                }

                string ldno = getDGLDNO(mechineID, d1.Rows[0]["productID"].ToString());
                if (string.IsNullOrEmpty(ldno))
                {
                    return "{ \"code\" : \"500\", \"msg\" : \"当前机器库存不足，请等待配送员上货\" }";

                }
                Util.log("2code:" + code + "mechineID:" + mechineID, "订购出货.txt");
                string sql3 = "select * from asm_orderlist where orderNO='" + d1.Rows[0]["orderNO"].ToString() + "'";
                DataTable d3 = DbHelperSQL.Query(sql3).Tables[0];

                string sql4 = "update  asm_orderlistDetail set statu=1 ,ldNO="+ ldno + ",sellTime='"+ DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where id='" + d1.Rows[0]["id"].ToString() +"'";
                Util.log("3code:" + code + "mechineID:" + mechineID, "订购出货.txt");
                if (DbHelperSQL.ExecuteSql(sql4)>0) {
                    Util.log("4code:" + code + "mechineID:" + mechineID, "订购出货.txt");
                    if (mechineID=="68" || mechineID == "69") {

                        string searchSql = "select 1 from asm_firstPayRecord where mechineID=" + mechineID + " AND memberID='" + d1.Rows[0]["memberID"].ToString() + "'";
                        DataTable searchSqldt = DbHelperSQL.Query(searchSql).Tables[0];
                        if (searchSqldt.Rows.Count > 0)
                        {

                        }
                        else
                        {
                            string insertsql = "insert into   asm_firstPayRecord(mechineID,memberID,firstinfo,firstbuyTime,type) values (" + mechineID + "," + d1.Rows[0]["memberID"].ToString() + ",'" + d1.Rows[0]["code"].ToString() + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + 1 + "') ";
                            Util.Debuglog("sqlInsert=" + insertsql, "获取预生成订单号.txt");
                            DbHelperSQL.ExecuteSql(insertsql);
                        }
                        dgchNew(ldno, mechineID, d1.Rows[0]["memberID"].ToString(), d1.Rows[0]["productID"].ToString(), code, d3.Rows[0]["price"].ToString());
                    } else {
                        dgch(ldno, mechineID, d1.Rows[0]["memberID"].ToString(), d1.Rows[0]["productID"].ToString(), code, d3.Rows[0]["price"].ToString());
                    }
                   

                    return "{ \"code\" : \"200\", \"msg\" : \"出货成功\" }";

                }
                else
                {
                    return "{ \"code\" : \"500\", \"msg\" : \"失败\" }";
                }

            }
            catch (Exception e)
            {
                Util.Debuglog("e=" + e.ToString() + " ;code=" + code, "订购出货.txt");
                return "{ \"code\" : \"500\", \"msg\" : \"失败\" }";
                
            }
           //根据机器判断1分钟内是否操作频繁
           
              
         
          
           
        }
        public static async System.Threading.Tasks.Task dgch(string ldNO, string mechineID, string memberID, string productID,string code,string money)
        {
            try
            {
                Util.Debuglog("ldNO="+ ldNO+ ";mechineID="+ mechineID+ ";memberID="+ memberID+ ";productID="+ productID+ ";code="+ code, "订购出货.txt");
                string url = "http://114.116.16.200/api/api.ashx";
                using (var client = new WebClient())
                {
                    var values = new NameValueCollection();
                    values["action"] = "dgch";
                    values["ldNO"] = ldNO;
                    values["mechineID"] = mechineID;
                    values["memberID"] = memberID;
                    values["productID"] = productID;
                    values["type"] = "1";
                    values["code"] = code;
                    values["money"] = money;
                    var response = client.UploadValues(url, values);
                    var responseString = Encoding.Default.GetString(response);
                    Util.Debuglog("responseStrin="+ responseString, "订购出货.txt");

                }
            }
            catch (Exception e)
            {

            }
        }
        //针对个别机器设定走此新流程
        public static async System.Threading.Tasks.Task dgchNew(string ldNO, string mechineID, string memberID, string productID, string code, string money)
        {
            try
            {
                Util.Debuglog("ldNO=" + ldNO + ";mechineID=" + mechineID + ";memberID=" + memberID + ";productID=" + productID + ";code=" + code, "订购出货.txt");
                string url = "http://alisocket.bingoseller.com/api/api.ashx";
                using (var client = new WebClient())
                {
                    var values = new NameValueCollection();
                    values["action"] = "dgchNew";
                    values["ldNO"] = ldNO;
                    values["mechineID"] = mechineID;
                    values["memberID"] = memberID;
                    values["productID"] = productID;
                    values["type"] = "1";
                    values["code"] = code;
                    values["money"] = money;
                    var response = client.UploadValues(url, values);
                    var responseString = Encoding.Default.GetString(response);
                    Util.Debuglog("responseStrin=" + responseString, "订购出货.txt");

                }
            }
            catch (Exception e)
            {

            }
        }
        //根据机器id获取料道信息
        [WebMethod]
        public string getLDInfo(string mechineID)
        {
            //string sql = "select * from (select ald.*,al.ldNum,al.statu from asm_ld_day ald left join asm_ldInfo al on ald.ldNO=al.ldNO where al.mechineID="+mechineID+ " and al.ld_productNum>0) A  left join asm_product ap on A.productID=ap.productID  where A.stu=0 and  A.mechineID=" + mechineID;
            string sql = "select al.*,ap.* from asm_ldinfo al left join asm_product ap on al.productID=ap.productID where al.statu=1 and al.mechineID='" + mechineID + "' and type=2 and al.productID!=''";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                OperUtil.Debuglog("sql=" + DataTableToJsonWithJsonNet(dt), "根据机器id获取料道信息.txt");
                return DataTableToJsonWithJsonNet(dt);
            }
            else
            {
                return "1";
            }
        }
        /// <summary>
        /// 获取支付宝红包
        /// </summary>
        /// <param name="mechineID"></param>
        /// <returns></returns>
        [WebMethod]
        public string getHB(string mechineID)
        {
            string sql = "select path,name from asm_zfbhb where id in(select hbID from asm_mechine where id='"+mechineID+"')";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return DataTableToJsonWithJsonNet(dt);
            }
            return "1";
        }
        [WebMethod]
        //time  yyyy-MM-dd
        public string updateStatuLD_day(string mechineID, string time)
        {
            string sql = "update asm_ld_day set stu=1 where mechineID=" + mechineID + " and time like '" + time + "%'";
            DbHelperSQL.ExecuteSql(sql);
            return "1";
        }
        //根据机器ID获取该机器的商品类别
        [WebMethod]
        public string getProductType(string mechineID)
        {
            string result = RedisHelper.GetRedisModel<string>("_productTypeInfo");
            if (string.IsNullOrEmpty(result))
            {
                string sql = "select * from asm_protype";
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    RedisHelper.SetRedisModel<string>("_productTypeInfo", DataTableToJsonWithJsonNet(dt),new TimeSpan(12,0,0));
                    OperUtil.Debuglog(mechineID+"数据库读取=" + DataTableToJsonWithJsonNet(dt), "getProductType.txt");
                    return DataTableToJsonWithJsonNet(dt);
                }
            }
            else {
                OperUtil.Debuglog(mechineID + "缓存读取=" + result, "getProductType.txt");
                return result;
            }
           
            return "1";
        }
        //根据机器编号获取视频列表
        [WebMethod]
        public string getVideoList(string mechineID)
        {
            string result = RedisHelper.GetRedisModel<string>(mechineID+"_videoInfo");
            if (string.IsNullOrEmpty(result))
            {
                //2019-08-09添加and ((tfType=1 and times<tfcs) or (tfType=2 and GETDATE()<valiDate))
                string sql = "select GETDATE() as dtime,avm.*,av.path,av.description,av.name from asm_videoAddMechine avm left join asm_video av on avm.videoID=av.id where mechineID='" + mechineID + "' and zt=0  and shType=1 and tfType!=0 and ((tfType=1 and times<tfcs) or (tfType=2 and GETDATE()<valiDate)) and is_open=0  and (GETDATE()>startTime or startTime is null)";
              
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    OperUtil.Debuglog(mechineID+"数据库=" + DataTableToJsonWithJsonNet(dt), "getVideoList.txt");
                    RedisHelper.SetRedisModel<string>(mechineID+ "_videoInfo", DataTableToJsonWithJsonNet(dt),new TimeSpan(2,0,0));
                    return DataTableToJsonWithJsonNet(dt);
                }
            }
            else {
                OperUtil.Debuglog(mechineID+"缓存读取=" +result, "getVideoList.txt");
                return result;
            }
            return "1";
        }
        [WebMethod]//视频上传记录

        public string uploadVideoRecordList(string recordList)
        {
            Util.Debuglog("recordList=" + recordList, "视频上传记录.txt");
            List<asm_videoRecord> twoList = JsonConvert.DeserializeObject<List<asm_videoRecord>>(recordList);
            List<string> sqlList = new List<string>();
            string mechineID = ""; 
            foreach (asm_videoRecord stu in twoList)
            {
                try
                {
                    mechineID = stu.mechineID.ToString();
                    Util.ClearRedisVideoByMechineID(stu.mechineID.ToString());//清空缓存
                    string sql = "select * from asm_videoAddMechine  where mechineID=" + stu.mechineID + " and videoID=" + stu.videoID;
                    DataTable dd = DbHelperSQL.Query(sql).Tables[0];
                    if (dd.Rows.Count > 0)
                    {
                        if (dd.Rows[0]["tfType"].ToString() == "1")
                        {
                            //按照次数
                            if (int.Parse(dd.Rows[0]["times"].ToString()) + stu.num <= int.Parse(dd.Rows[0]["tfcs"].ToString()))
                            {
                                string updateSQL = "update asm_videoAddMechine set times=times+" + stu.num + " where mechineID=" + stu.mechineID + " and videoID=" + stu.videoID;
                                sqlList.Add(updateSQL);
                                Util.Debuglog("updateSQL=" + updateSQL, "视频上传记录.txt");
                                string updateSQL1 = "update asm_video set totalTimes=totalTimes+" + stu.num + " where id=" + stu.videoID;
                                sqlList.Add(updateSQL1);
                            }
                            else
                            {
                                //更新视频状态
                                string update = "update [dbo].[asm_videoAddMechine] set zt=1,times=times+" + stu.num + " where mechineID=" + stu.mechineID + " and videoID=" + dd.Rows[0]["videoID"].ToString();
                                sqlList.Add(update);
                            }
                        }
                        else if (dd.Rows[0]["tfType"].ToString() == "2")
                        {
                            Util.Debuglog("updateSQL.." + DateTime.Parse(dd.Rows[0]["valiDate"].ToString()) , "视频上传记录.txt");

                            //按照时间
                            if (DateTime.Parse(dd.Rows[0]["valiDate"].ToString()) <= DateTime.Now)
                            {
                               
                                //更新视频状态
                                string update = "update [dbo].[asm_videoAddMechine] set zt=1,times=times+" + stu.num + " where mechineID=" + stu.mechineID + " and videoID=" + dd.Rows[0]["videoID"].ToString();
                                Util.Debuglog("updateSQL.1." + DateTime.Parse(dd.Rows[0]["valiDate"].ToString()) + "=" + update, "视频上传记录.txt");
                                sqlList.Add(update);
                            }
                            else
                            {
                                string updateSQL = "update asm_videoAddMechine set times=times+" + stu.num + " where mechineID=" + stu.mechineID + " and videoID=" + stu.videoID;
                                Util.Debuglog("updateSQL.."+ DateTime.Parse(dd.Rows[0]["valiDate"].ToString()) + "=" + updateSQL, "视频上传记录.txt");
                                sqlList.Add(updateSQL);
                                string updateSQL1 = "update asm_video set totalTimes=totalTimes+" + stu.num + " where id=" + stu.videoID;
                                sqlList.Add(updateSQL1);
                            }
                        }
                    }
                    RedisHelper.Remove(stu.mechineID + "_VideoAddMechine");
                }
                catch { }
            }
            DbHelperSQL.ExecuteSqlTran(sqlList);
            
            return "1";
        }
        
        [WebMethod]
        public string upSellRecord(string recordList)
        {
            OperUtil.Debuglog("开始recordList=" + recordList, "upSellRecord1.txt");
            AsyEventClass ac = new AsyEventClass();
            AsyEventClass.AsyncEventHandler asy = new AsyEventClass.AsyncEventHandler(ac.upSellRecord);
            IAsyncResult result = asy.BeginInvoke(recordList, null, null);
            OperUtil.Debuglog("结束recordList", "upSellRecord1.txt");
            return "1";
        }
        
        
        [WebMethod]//机器登录验证
        public string Login(string BH, string pwd)
        {
            string sql = "select  A.[id],[bh],[mechineName],[province],[city],[country],[addres],A.[version],[operaID],A.[agentID],A.[companyID],A.[statu],[validateTime],[regTime],[zt],[zdX],[zdY],[sjX],[sjY],[sjAddress],[temperture],[brokenTime],[videoPath],[lastReqTime],socketUrl ,ao.name from (select  am.*,aa.id dlsID from asm_mechine am left join asm_agent aa on am.operaID=aa.id) A left join asm_opera ao on A.operaID=ao.id where A.bh='" + BH + "' and A.pwd='" + pwd + "'";
            OperUtil.Debuglog("sql=" + sql, "登录.txt");
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                OperUtil.Debuglog("recordList=" + DataTableToJsonWithJsonNet(dt), "登录.txt");
                return DataTableToJsonWithJsonNet(dt);
            }
            else
            {
                return "1";
            }
        }
        [WebMethod]//出货
        public string getCH(string mechineID)
        {
            string sql = "select * from asm_waitCH where  zt=0  and mechineID=" + mechineID + " and createTime like '" + DateTime.Now.ToString("yyyy-MM-dd") + "%'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                OperUtil.Debuglog("sql=" + DataTableToJsonWithJsonNet(dt), "出货.txt");
                return DataTableToJsonWithJsonNet(dt);
            }
            else
            {
                return "1";
            }
        }
        [WebMethod]//获取订购
        public string getDGList(string mechineID)
        {
            //获取已经付款的  不下载已经完成的订单
            string sql = "select aod.* from asm_orderDetail aod left join asm_order ao on aod.orderNO=ao.orderNO where aod.mechineID='" + mechineID + "' and aod.createTime='" + DateTime.Now.ToString("yyyy-MM-dd") + "'  and aod.ldNO!='' and aod.zt!=1 and ao.fkzt=1";

            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                OperUtil.Debuglog("sql=" + DataTableToJsonWithJsonNet(dt), "获取订购.txt");
                return DataTableToJsonWithJsonNet(dt);
            }
            else
            {
                return "1";
            }
        }
        [WebMethod]//上传温度
        public string readZTMechine(string mechineID, string temperature)
        {
            try
            {
                Util.Debuglog("mechineID=" + mechineID + ";temperature=" + temperature, "上传温度.txt");
                string result = RedisHelper.GetRedisModel<string>(mechineID+"_mechineInfo");
                string tem = "";
                if (string.IsNullOrEmpty(result))
                {
                    string sql = "select am.*,ac.p1,ac.p2,ac.p3,ac.p4,ac.p5,ac.p6,ac.p7,ac.p8,ac.p9,ac.p10,am.setTem from asm_mechine am left join asm_company ac on am.companyID=ac.id where am.id='" + mechineID + "'";
                    DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                    if (dt.Rows.Count>0)
                    {
                        tem = dt.Rows[0]["setTem"].ToString();
                        Util.Debuglog(mechineID+"数据库读取="+tem, "上传温度.txt");
                        RedisHelper.SetRedisModel<string>(mechineID + "_mechineInfo",OperUtil.DataTableToJsonWithJsonNet(dt),new TimeSpan(2,0,0));
                        result = OperUtil.DataTableToJsonWithJsonNet(dt);
                    }
                }
                else {
                    JArray jArray = (JArray)JsonConvert.DeserializeObject(result);
                    tem = jArray[0]["setTem"].ToString();
                    Util.Debuglog(mechineID + "缓存读取=" + tem, "上传温度.txt");
                }
                //向asm_info 表插入一条消息 用于判断机器是否脱机
                //更新机器温度

                double min = 0;
                double max = 0;
                try
                {
                    string[] arr = tem.Split('-');
                    if (arr.Length==2) {
                         min = double.Parse(arr[0]);
                         max = double.Parse(arr[1]);
                    }
                }
                catch {

                }
                if (double.Parse(temperature)<min||double.Parse(temperature)>max)//温度大于10度显示异常
                {
                    string sqlUpdate = "update asm_mechine set temperture='" + double.Parse(temperature) + "',statu=2,temStatus=1,brokenTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',lastReqTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',sendF=0,sendT=1 where id='" + mechineID + "'";
                    Util.Debuglog("1sqlUpdate=" + sqlUpdate, "上传温度.txt");
                    DbHelperSQL.ExecuteSql(sqlUpdate);
                }
                else
                {
                    string sqlUpdate = "update asm_mechine set temperture='" + double.Parse(temperature) + "',lastReqTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',statu=0,sendF=0,sendT=0,temStatus=0 where id='" + mechineID + "'";
                    Util.Debuglog("2sqlUpdate=" + sqlUpdate, "上传温度.txt");
                    DbHelperSQL.ExecuteSql(sqlUpdate);
                }
                //插入温度记录
                if (double.Parse(temperature)>-20)
                {
                    string sqlInsert = "insert into asm_temperature (mechineID,temperature,time) values('" + mechineID + "','" + temperature + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "')";
                    DbHelperSQL.ExecuteSql(sqlInsert);
                }
                
                if (!string.IsNullOrEmpty(result))
                {
                    OperUtil.Debuglog("sql=" + result, "机器状态.txt");
                    return result;
                }
                else
                {
                    return "1";
                }
            }
            catch(Exception e) {
                Util.Debuglog("error=" + e.Message, "上传温度.txt");
            }
            return "1";
           
        }
        [WebMethod]//上传温度2
        public string readZTMechine2(string mechineID, string temperature,string versions)
        {
            try
            {
                Util.Debuglog("mechineID=" + mechineID + ";temperature=" + temperature, "上传温度2.txt");
                string result = RedisHelper.GetRedisModel<string>(mechineID + "_mechineInfo");
                string tem = "";
                if (string.IsNullOrEmpty(result))
                {
                    string sql = "select am.*,ac.p1,ac.p2,ac.p3,ac.p4,ac.p5,ac.p6,ac.p7,ac.p8,ac.p9,ac.p10,am.setTem from asm_mechine am left join asm_company ac on am.companyID=ac.id where am.id='" + mechineID + "'";
                    DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        tem = dt.Rows[0]["setTem"].ToString();
                        Util.Debuglog(mechineID + "数据库读取=" + tem, "上传温度.txt");
                        RedisHelper.SetRedisModel<string>(mechineID + "_mechineInfo", OperUtil.DataTableToJsonWithJsonNet(dt), new TimeSpan(2, 0, 0));
                        result = OperUtil.DataTableToJsonWithJsonNet(dt);
                    }
                }
                else
                {
                    JArray jArray = (JArray)JsonConvert.DeserializeObject(result);
                    tem = jArray[0]["setTem"].ToString();
                    Util.Debuglog(mechineID + "缓存读取=" + tem, "上传温度.txt");
                }
                //向asm_info 表插入一条消息 用于判断机器是否脱机
                //更新机器温度

                double min = 0;
                double max = 0;
                try
                {
                    string[] arr = tem.Split('-');
                    if (arr.Length == 2)
                    {
                        min = double.Parse(arr[0]);
                        max = double.Parse(arr[1]);
                    }
                }
                catch
                {

                }
                if (double.Parse(temperature) < min || double.Parse(temperature) > max)//温度大于10度显示异常
                {
                    string sqlUpdate = "update asm_mechine set softversion='"+versions+"',temperture='" + double.Parse(temperature) + "',statu=2,temStatus=1,brokenTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',lastReqTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',sendF=0,sendT=1 where id='" + mechineID + "'";
                    Util.Debuglog("1sqlUpdate=" + sqlUpdate, "上传温度.txt");
                    DbHelperSQL.ExecuteSql(sqlUpdate);
                }
                else
                {
                    string sqlUpdate = "update asm_mechine set softversion='" + versions + "',temperture='" + double.Parse(temperature) + "',lastReqTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',statu=0,sendF=0,sendT=0,temStatus=0 where id='" + mechineID + "'";
                    Util.Debuglog("2sqlUpdate=" + sqlUpdate, "上传温度.txt");
                    DbHelperSQL.ExecuteSql(sqlUpdate);
                }
                //插入温度记录
                if (double.Parse(temperature) > -20)
                {
                    string sqlInsert = "insert into asm_temperature (mechineID,temperature,time) values('" + mechineID + "','" + temperature + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "')";
                    DbHelperSQL.ExecuteSql(sqlInsert);
                }

                if (!string.IsNullOrEmpty(result))
                {
                    OperUtil.Debuglog("sql=" + result, "机器状态.txt");
                    return result;
                }
                else
                {
                    return "1";
                }
            }
            catch (Exception e)
            {
                Util.Debuglog("error=" + e.Message, "上传温度.txt");
            }
            return "1";

        }
        //更新下载状态
        [WebMethod]
        public string updateDGList(string mechineID)
        {
            string sql = "update asm_orderDetail set statu=1 where createTime='" + DateTime.Now.ToString("yyyy-MM-dd") + "' and ldNO!='' and  mechineID=" + mechineID;
            int a = DbHelperSQL.ExecuteSql(sql);
            if (a > 0)
            {
                return "1";
            }
            else
            {
                return "0";
            }
        }
        [WebMethod]//更新订购状态
        public string updateDGZT(string mechineID, string code)
        {

            string sql = "update asm_orderDetail set zt=1 where code='" + code + "' and  mechineID=" + mechineID;
            OperUtil.Debuglog("sql=" + sql, "更新订购状态_.txt");
            int a = DbHelperSQL.ExecuteSql(sql);
            if (a > 0)
            {
                //更新订购订单的剩余天数
                string sql1 = "select * from asm_orderDetail where mechineID=" + mechineID + " and code='" + code + "'";
                DataTable db = DbHelperSQL.Query(sql1).Tables[0];
                if (db.Rows.Count > 0)
                {
                    string sql2 = "update asm_order set  syNum=syNum-1 where mechineID='" + mechineID + "' and memberID='" + db.Rows[0]["memberID"].ToString() + "' and orderNO='" + db.Rows[0]["orderNO"].ToString() + "' and syNum>0";
                    DbHelperSQL.ExecuteSql(sql2);
                    string sqlUpdate = "select * from asm_order where orderNO='" + db.Rows[0]["orderNO"].ToString() + "'";
                    DataTable dd = DbHelperSQL.Query(sqlUpdate).Tables[0];
                    if (dd.Rows.Count > 0 && dd.Rows[0]["syNum"].ToString() == "0")
                    {
                        string sqlUp = "update asm_order set zt=3 where orderNO='" + db.Rows[0]["orderNO"].ToString() + "'";
                        DbHelperSQL.ExecuteSql(sqlUp);
                    }
                }
                return "1";
            }
            else
            {
                return "0";
            }
        }
        [WebMethod]
        public string updateStatu(string id)
        {
            string sql = "delete from  asm_waitCH";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return "0";
            }
            else
            {
                return "1";
            }
        }
        [WebMethod]//更新料道库存
        public string updateKC(string mechineID, string ldNO)
        {
            string sql = "select * from asm_ldinfo where ldNO='" + ldNO + "' and mechineID=" + mechineID;
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];

            if (dt.Rows.Count > 0 && int.Parse(dt.Rows[0]["ld_productNum"].ToString()) > 0)
            {
                string sql1 = "update asm_ldInfo set ld_productNum=ld_productNum-1 where ldNO='" + ldNO + "' and mechineID=" + mechineID;
                OperUtil.Debuglog("sql1=" + sql1, "更新料道库存_.txt");
                int a = DbHelperSQL.ExecuteSql(sql1);
                if (a > 0)
                {
                    return "1";
                }
                else
                {
                    return "0";
                }
            }
            return "0";
        }
        [WebMethod]//经纬度
        public string upLoadLocation(string str)
        {
            JObject jo = (JObject)JsonConvert.DeserializeObject(str.Replace("[", "").Replace("]", ""));
            try
            {
                string mechineID = jo["mechineID"].ToString();
                string Latitude = jo["Latitude"].ToString();
                string Longitude = jo["Longitude"].ToString();
                string Province = jo["Province"].ToString();
                string City = jo["City"].ToString();
                string District = jo["District"].ToString();
                string Address = jo["Address"].ToString();
                if (!string.IsNullOrEmpty(mechineID))
                {
                    string sql = "update asm_mechine set sjX='" + Longitude + "',sjY='" + Latitude + "',sjAddress='" + Province + "-" + City + "-" + District + "-" + Address + "' where id=" + mechineID;
                    DbHelperSQL.ExecuteSql(sql);
                    if (mechineID == "68" || mechineID == "69")
                    {
                        string mechineInfo = RedisUtil.getMechine(mechineID);
                        JArray jay = RedisUtil.DeserializeObject(mechineInfo);

                        jay[0]["sjX"]= Longitude;
                        jay[0]["sjY"] = Latitude;
                        jay[0]["sjAddress"] = Province + "-" + City + "-" + District + "-" + Address;
                        RedisHelper.SetRedisModel<string>(mechineID + "_mechineInfoSet", JsonConvert.SerializeObject(jay));
                       
                    }
                }
            }
            catch
            {

            }
            return "1";
        }
        /// <summary>
        /// 安徽扫码支付获取二维码
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="mch_id"></param>
        /// <param name="title"></param>
        /// <param name="total_fee"></param>
        /// <param name="client_ip"></param>
        /// <returns></returns>
        [WebMethod]
        public string getQR(string channel, string mch_id, string title, string total_fee, string client_ip)
        {
            WebService1SoapClient wx = new WebService1SoapClient();
            string url = HttpContext.Current.Request.Url.Host + "/ashx/payCallback.aspx/ProcessRequest";
            string str = wx.CreateErWeiMa(channel, mch_id, title, total_fee, client_ip, url);
            //同时向asm_pay表插入记录 然后客户端定时刷新这个单看看是否支付成功 成功了机器出货
            return str;
        }
        /// <summary>
        /// 通联银保支付扫码支付
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        [WebMethod]
        public string getQRTLPayWX(string payMoney, string type, string content, string remark, string mechineID)
        {
            SybWxPayService sybService = new SybWxPayService(mechineID);
            string url = NQ_URL + "pay/Notify.aspx";
            //payMoney=0.01;type=wx;content=燕麦+黄桃素雪酸奶;remarl=;mechineID=34
            Util.Debuglog("payMoney=" + payMoney + ";type=" + type + ";content=" + content + ";remarl=" + remark + ";mechineID=" + mechineID, "_支付链接.txt");
           
            string result = RedisHelper.GetRedisModel<string>(mechineID + "_mechineInfo");
            if (string.IsNullOrEmpty(result))
            {
                string sql = "select am.*,ac.p1,ac.p2,ac.p3,ac.p4,ac.p5,ac.p6,ac.p7,ac.p8,ac.p9,ac.p10,am.setTem from asm_mechine am left join asm_company ac on am.companyID=ac.id where am.id='" + mechineID + "'";
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    RedisHelper.SetRedisModel<string>(mechineID + "_mechineInfo", OperUtil.DataTableToJsonWithJsonNet(dt), new TimeSpan(2, 0, 0));
                    result = OperUtil.DataTableToJsonWithJsonNet(dt);
                }
            }
            JArray jArray = (JArray)JsonConvert.DeserializeObject(result);
            string data = "";
            if (type == "wx")//微信扫码支付
            {
                content = jArray[0]["mechineName"].ToString();
                long f = long.Parse((double.Parse(payMoney) * 100).ToString());
                Dictionary<String, String> rsp = sybService.pay(f, DateTime.Now.ToFileTime().ToString(), "W01", content, remark, "", "", url, "");
                data = OperUtil.SerializeDictionaryToJsonString(rsp);
                Util.Debuglog("微信支付链接data=" + data, "_支付链接.txt");
                //插入预处理订单信息
                string json = (new JavaScriptSerializer()).Serialize(rsp);
                Util.Debuglog("微信支付链接json=" + json, "_支付链接.txt");
                JObject jo = (JObject)JsonConvert.DeserializeObject(json);

                if (jo["retcode"].ToString() == "SUCCESS")
                {
                    string appid = jo["appid"].ToString();
                    string cusid = jo["cusid"].ToString();
                    string trxid = jo["trxid"].ToString();
                    string reqsn = jo["reqsn"].ToString();
                    // //插入预订单信息
                    string insertSQL = @"insert into asm_pay_info(appid,cusid,trxid,statu,reqsn,type,payType,trxamt,createTime)
                                            values('" + appid + "','" + cusid + "','" + trxid + "','0','" + reqsn + "',2,1," + double.Parse(payMoney) * 100 + ",'" + DateTime.Now + "')";
                    Util.Debuglog("微信支付链接=insertSQL=" + insertSQL, "_支付链接.txt");
                    DbHelperSQL.ExecuteSql(insertSQL);
                    
                }
            }
            else if (type == "zfb")
            {
                content = jArray[0]["mechineName"].ToString();
                Dictionary<String, String> rsp = sybService.pay(long.Parse((double.Parse(payMoney) * 100).ToString()), DateTime.Now.ToFileTime().ToString(), "A01", content, remark, "", "", url, "");
                data = OperUtil.SerializeDictionaryToJsonString(rsp);
                Util.Debuglog("支付宝支付链接data=" + data, "_支付链接.txt");
                string json = (new JavaScriptSerializer()).Serialize(rsp);
                Util.Debuglog("支付宝支付链接json=" + json, "_支付链接.txt");
                JObject jo = (JObject)JsonConvert.DeserializeObject(json);
                if (jo["retcode"].ToString() == "SUCCESS")
                {
                    string appid = jo["appid"].ToString();
                    string cusid = jo["cusid"].ToString();
                    string trxid = jo["trxid"].ToString();
                    string reqsn = jo["reqsn"].ToString();
                    // //插入预订单信息
                    string insertSQL = @"insert into asm_pay_info(appid,cusid,trxid,statu,reqsn,type,payType,trxamt,createTime)
                                            values('" + appid + "','" + cusid + "','" + trxid + "','0','" + reqsn + "',2,2," + double.Parse(payMoney) * 100 + ",'" + DateTime.Now + "')";
                    DbHelperSQL.ExecuteSql(insertSQL);

                }
            }
            //Dictionary<String, String> rsp = sybService.pay(1, DateTime.Now.ToFileTime().ToString(), "W01", "商品内容", "备注", "", "", "http://baidu.com", "");
            return data;
        }
        [WebMethod]
        public string refund(string trxid)
        {
            return "";
        }

        [WebMethod]
        public string getQRTLPayZFB(string payMoney, string type, string content, string remark, string mechineID)
        {
            SybWxPayService sybService = new SybWxPayService(mechineID);
            string url = NQ_URL + "pay/Notify.aspx";
            string result = RedisHelper.GetRedisModel<string>(mechineID + "_mechineInfo");
            if (string.IsNullOrEmpty(result))
            {
                string sql = "select am.*,ac.p1,ac.p2,ac.p3,ac.p4,ac.p5,ac.p6,ac.p7,ac.p8,ac.p9,ac.p10,am.setTem from asm_mechine am left join asm_company ac on am.companyID=ac.id where am.id='" + mechineID + "'";
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    RedisHelper.SetRedisModel<string>(mechineID + "_mechineInfo", OperUtil.DataTableToJsonWithJsonNet(dt), new TimeSpan(2, 0, 0));
                    result = OperUtil.DataTableToJsonWithJsonNet(dt);
                }
            }
            JArray jArray = (JArray)JsonConvert.DeserializeObject(result);
            string data = "";
            if (type == "wx")//微信扫码支付
            {
                content = jArray[0]["mechineName"].ToString();
                long f = long.Parse((double.Parse(payMoney) * 100).ToString());
                Dictionary<String, String> rsp = sybService.pay(f, DateTime.Now.ToFileTime().ToString(), "W01", content, remark, "", "", url, "");
                data = OperUtil.SerializeDictionaryToJsonString(rsp);
                //插入预处理订单信息
                string json = (new JavaScriptSerializer()).Serialize(rsp);
                JObject jo = (JObject)JsonConvert.DeserializeObject(json);

                if (jo["retcode"].ToString() == "SUCCESS")
                {
                    string appid = jo["appid"].ToString();
                    string cusid = jo["cusid"].ToString();
                    string trxid = jo["trxid"].ToString();
                    string reqsn = jo["reqsn"].ToString();
                    // //插入预订单信息
                    string insertSQL = @"insert into asm_pay_info(appid,cusid,trxid,statu,reqsn,type,payType,trxamt,createTime)
                                            values('" + appid + "','" + cusid + "','" + trxid + "','0','" + reqsn + "',2,1," + double.Parse(payMoney) * 100 + ",'" + DateTime.Now + "')";
                    DbHelperSQL.ExecuteSql(insertSQL);

                }
            }
            else if (type == "zfb")
            {
                content = jArray[0]["mechineName"].ToString();
                Dictionary<String, String> rsp = sybService.pay(long.Parse((double.Parse(payMoney) * 100).ToString()), DateTime.Now.ToFileTime().ToString(), "A01", content, remark, "", "", url, "");
                data = OperUtil.SerializeDictionaryToJsonString(rsp);
                string json = (new JavaScriptSerializer()).Serialize(rsp);
                JObject jo = (JObject)JsonConvert.DeserializeObject(json);
                Util.Debuglog("支付宝支付链接json=" + json, "_支付链接.txt");
                if (jo["retcode"].ToString() == "SUCCESS")
                {
                    string appid = jo["appid"].ToString();
                    string cusid = jo["cusid"].ToString();
                    string trxid = jo["trxid"].ToString();
                    string reqsn = jo["reqsn"].ToString();
                    // //插入预订单信息
                    string insertSQL = @"insert into asm_pay_info(appid,cusid,trxid,statu,reqsn,type,payType,trxamt,createTime)
                                            values('" + appid + "','" + cusid + "','" + trxid + "','0','" + reqsn + "',2,2," + double.Parse(payMoney) * 100 + ",'" + DateTime.Now + "')";
                    Util.Debuglog("支付宝支付链接insertSQL=" + insertSQL, "_支付链接.txt");
                    DbHelperSQL.ExecuteSql(insertSQL);
                    
                }
            }
            //Dictionary<String, String> rsp = sybService.pay(1, DateTime.Now.ToFileTime().ToString(), "W01", "商品内容", "备注", "", "", "http://baidu.com", "");

            return data;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="payMoney"></param>
        /// <param name="type"></param>
        /// <param name="content"></param>
        /// <param name="remark"></param>
        /// <param name="mechineID"></param>
        /// <returns></returns>
        [WebMethod]
        public string getQRTotal(string payMoney, string mechineID, string asmpayid)
        {
            //先判断该订单是否存在 存在的话判断该二维码是否已经支付statu=1是已经支付
            Util.Debuglog("payMoney=" + payMoney + ";mechineID=" + mechineID + ";asmpayid=" + asmpayid, "_钱包支付.txt");
           
            string insertSQL = "insert into asm_pay_info(trxamt,asmpayid) values(" + double.Parse(payMoney) * 100 + ",'" + asmpayid + "')";
            DbHelperSQL.ExecuteSql(insertSQL);
            string strUrl = "?mechineID=" + mechineID + "&payMoney=" + payMoney + "&asmpayid=" + asmpayid;
            string url = NQ_URL + "page.aspx" + strUrl;//生成支付链接返回
            string str = "{asmpayid:'" + asmpayid + "',url:'" + url + "'}";
            Util.Debuglog("钱包支付链接=" + str, "_钱包支付.txt");
            return str;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mechineID">机器ID</param>
        /// <param name="money">元为单位</param>
        /// <returns></returns>
        [WebMethod]
        public string getUserPayInfoUrl(string mechineID, string money)
        {
            //先判断该订单是否存在 存在的话判断该二维码是否已经支付statu=1是已经支付
            string sql1 = "select * from asm_mechine where id=" + mechineID;
            DataTable dd = DbHelperSQL.Query(sql1).Tables[0];
            string sql2 = "select * from asm_company where id='"+dd.Rows[0]["companyID"].ToString()+"'";
            DataTable d2 = DbHelperSQL.Query(sql2).Tables[0];
            Random rd = new Random();
            int rand = rd.Next(10000, 99999);
            string trxid = Util.ConvertDateTimeToInt(DateTime.Now).ToString() + rand;
            string insertSQL = "insert into asm_pay_info(trxid,statu,type,payType,trxamt,createTime,appid) values('" + trxid + "',0,2,4," + double.Parse(money) * 100 + ",'" + DateTime.Now + "','"+d2.Rows[0]["tl_APPID"].ToString()+"')";
            DbHelperSQL.ExecuteSql(insertSQL);
            string req = "trxid=" + trxid + "&money=" + money + "&companyID=" + dd.Rows[0]["companyID"].ToString();
            req = PwdHelper.EncodeDES(req, "bingoseller");
            string url = WX_URL + "main/paypage.aspx?req=" + req;//生成支付链接返回
            string str = "{trxid:'" + trxid + "',url:'" + url + "'}";
            Util.Debuglog("钱包支付链接=" + str, "_支付链接.txt");
            return str;
        }
      
        public void printRsp(Dictionary<String, String> rspDic)
        {
            string rsp = "请求返回数据:\n";
            foreach (var item in rspDic)
            {
                rsp += item.Key + "-----" + item.Value + ";\n";
            }
        }
        //返回1说明支付成功 0失败
        [WebMethod]
        public string getPayInfo(string trxid)
        {
            string sql = "select * from asm_pay_info  where trxid='" + trxid + "' and statu=1 and  chzt is null";
            DataSet ds = DbHelperSQL.Query(sql);
            if (ds.Tables.Count>0)
            {
                DataTable dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return "1";
                }
                else {
                    return "0";
                }
            }
            else
            {
                return "0";
            }
        }
        [WebMethod]
        public string updatePayInfo(string trxid)
        {
            OperUtil.Debuglog("流水号=" + trxid, "更新扫码支付状态.txt");
            string sql = "update asm_pay_info set chzt=1  where trxid='" + trxid + "'";
            int a=DbHelperSQL.ExecuteSql(sql);
            if (a> 0)
            {
                return "1";
            }
            else
            {
                return "0";
            }
        }
        [WebMethod]
        public string uploadERR(string mechineID, string errMsg)
        {
            string sql = "insert into asm_error (mechineID,errTime,STACK_TRACE) values(" + mechineID + ",'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + errMsg + "')";
            int a = DbHelperSQL.ExecuteSql(sql);
            if (a > 0)
            {
                return "1";
            }
            else
            {
                return "0";
            }
        }
        [WebMethod]//出售的商品被购买_
        public string updateNotice(string code)
        {
            OperUtil.Debuglog("出售的商品被购买code=" + code, "出售的商品被购买_.txt");
            string sql = "update asm_orderDetail set zt=6  where  code='" + code + "'";
            int a = DbHelperSQL.ExecuteSql(sql);
            if (a > 0)
            {
                string sql1 = "select * from asm_orderDetail where code='" + code + "'";
                DataTable dd = DbHelperSQL.Query(sql1).Tables[0];
                string sql2 = "update asm_member set AvailableMoney=AvailableMoney+" + dd.Rows[0]["sellPrice"].ToString() + " where id=" + dd.Rows[0]["memberID"].ToString();
                DbHelperSQL.ExecuteSql(sql2);
                //发通知
                string sqlM = "select * from asm_member where id=" + dd.Rows[0]["memberID"].ToString();
                DataTable dm = DbHelperSQL.Query(sqlM).Tables[0];
                string sqlC = "select companyID from asm_mechine where id in(select mechineID from asm_orderDetail where code='" + code + "') ";
                DataTable dc = DbHelperSQL.Query(sqlC).Tables[0];
                wxHelper wx = new wxHelper(dc.Rows[0]["companyID"].ToString());
                string data = TemplateMessage.money_bd(dm.Rows[0]["openID"].ToString(), OperUtil.getMessageID(dc.Rows[0]["companyID"].ToString(), "OPENTM403148135"), "余额变动提醒", "转售成功", dd.Rows[0]["sellPrice"].ToString(), dm.Rows[0]["AvailableMoney"].ToString(), "", "支付时间:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                TemplateMessage.SendTemplateMsg(wx.IsExistAccess_Token(dc.Rows[0]["companyID"].ToString()), data);

                OperUtil.insertNotice(dd.Rows[0]["memberID"].ToString(), "出售成功", "您出售的商品已卖出,钱已打到您的可用余额");
                Util.moneyChange(dd.Rows[0]["id"].ToString(), dd.Rows[0]["sellPrice"].ToString(), dm.Rows[0]["AvailableMoney"].ToString(), "会员转售", "4", "", "您出售的商品已卖出,钱已打到您的可用余额");

            }
            return "1";
        }
       
        public string DataTableToJsonWithJsonNet(DataTable table)
        {
            string jsonString = string.Empty;
            jsonString = JsonConvert.SerializeObject(table);
            return jsonString;
        }
        public DataTable JsonToDataTable(string json)
        {
            DataTable dataTable = new DataTable();  //实例化
            DataTable result;
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                javaScriptSerializer.MaxJsonLength = Int32.MaxValue; //取得最大数值
                ArrayList arrayList = javaScriptSerializer.Deserialize<ArrayList>(json);
                if (arrayList.Count > 0)
                {
                    foreach (Dictionary<string, object> dictionary in arrayList)
                    {
                        if (dataTable.Columns.Count == 0)
                        {
                            foreach (string current in dictionary.Keys)
                            {
                                dataTable.Columns.Add(current, dictionary[current].GetType());
                            }
                        }
                        DataRow dataRow = dataTable.NewRow();
                        foreach (string current in dictionary.Keys)
                        {
                            dataRow[current] = dictionary[current];
                        }

                        dataTable.Rows.Add(dataRow); //循环添加行到DataTable中
                    }
                }
            }
            catch(Exception e)
            {
                string res = e.Message;
            }
            result = dataTable;
            return result;
        }
 
     public static void Debuglog(string log, string logname = "_Debuglog.txt")
        {
            try
            {
                StreamWriter writer = System.IO.File.AppendText(HttpRuntime.AppDomainAppPath.ToString() + "log/" + (DateTime.Now.ToString("yyyyMMdd") + logname));
                writer.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + ":" + log);
                writer.WriteLine("---------------");
                writer.Close();
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception)
            {
            }

        }
       
    }
}
