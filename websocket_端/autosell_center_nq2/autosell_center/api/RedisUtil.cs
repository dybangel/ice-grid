using autosell_center.api;
using Consumer.cls;
using DBUtility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace autosell_center
{
    public class RedisUtil
    {
        public static string getMechine(string mechineID) {
            string mechineInfo = null;
            try {
                 mechineInfo = RedisHelper.GetRedisModel<string>(mechineID + "_mechineInfoSet");
            }
            catch  {
            }
         
            if (string.IsNullOrEmpty(mechineInfo))
            {
                string sql = "select am.*,ac.p1,ac.p2,ac.p3,ac.p4,ac.p5,ac.p6,ac.p7,ac.p8,ac.p9,ac.p10,am.setTem,'' videoListNo,'' productTypeNo,'' androidProductNo,'' priceSwitch from asm_mechine am left join asm_company ac on am.companyID=ac.id where am.id='" + mechineID + "'";
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {

                    RedisHelper.SetRedisModel<string>(mechineID + "_mechineInfoSet", OperUtil.DataTableToJsonWithJsonNet(dt));
                    mechineInfo = OperUtil.DataTableToJsonWithJsonNet(dt);
                }
            }

            return mechineInfo;
        }

        public static JArray DeserializeObject(string mechineInfo)
        {
            if (!String.IsNullOrEmpty(mechineInfo)) {
                JArray jArray = (JArray)JsonConvert.DeserializeObject(mechineInfo);
                return jArray;
            }
            return null;
        }

        public static DataTable getAllMechine() {
            string sql = "select id from asm_mechine where id in (68,69)";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            return dt;
        }
        public static string getMechineChString(string mechineID)
        {
            string mechineIDCHString = RedisHelper.GetRedisModel<string>("CH" + mechineID);  //出货指令缓存
            if (mechineIDCHString == null || mechineIDCHString == "")
            {
                JObject mechineIDCHObj = new JObject();
                RedisHelper.SetRedisModel<string>("CH" + mechineID, mechineIDCHObj.ToString());
            }
            return RedisHelper.GetRedisModel<string>("CH" + mechineID);  //出货指令缓存
        }
        //安卓屏幕的产品类别
        public static void getProductTypeInfo()
        {
           
            string sql = "select * from asm_protype";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                Util.log("dt=" + OperUtil.DataTableToJsonWithJsonNet(dt), "updateProductType.txt");
                SetNewRedisInfo("_productTypeInfoSet", OperUtil.DataTableToJsonWithJsonNet(dt));
            }
           
            
        }
        //产品信息
        public static JArray getSellOrderInfo(string mechineID)
        {
            string mechineSellOrderInfo = null;
            try
            {
                mechineSellOrderInfo = RedisHelper.GetRedisModel<string>(mechineID + "_SellOrderInfo");
            }
            catch
            {

            }
            if (string.IsNullOrEmpty(mechineSellOrderInfo))
            {

                string sql = "select * from asm_orderlistDetail where zt = 3   AND mechineID ='" + mechineID + "'";
               
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    RedisHelper.SetRedisModel<string>(mechineID + "_SellOrderInfo", OperUtil.DataTableToJsonWithJsonNet(dt));
                    mechineSellOrderInfo = OperUtil.DataTableToJsonWithJsonNet(dt);
                }
            }

            return mechineSellOrderInfo == null ? new JArray() : (JArray)JsonConvert.DeserializeObject(mechineSellOrderInfo);

        }
        //产品信息
        public static JArray getVideoAddMechine(string mechineID)
        {
            string mechineVideoAddMechine = null;
            try
            {
                mechineVideoAddMechine = RedisHelper.GetRedisModel<string>(mechineID + "_VideoAddMechine");
            }
            catch
            {

            }
            if (string.IsNullOrEmpty(mechineVideoAddMechine))
            {
                
                string sql = "select * from asm_videoAddMechine  where mechineID='" + mechineID + "' and zt=0  and tfType!=0 and ((tfType=1 and times<tfcs) or (tfType=2 and GETDATE()<valiDate)) and is_open=0  and (GETDATE()>startTime or startTime is null)";

                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    RedisHelper.SetRedisModel<string>(mechineID + "_VideoAddMechine", OperUtil.DataTableToJsonWithJsonNet(dt));
                    mechineVideoAddMechine = OperUtil.DataTableToJsonWithJsonNet(dt);
                }
            }

            return mechineVideoAddMechine == null ? new JArray() : (JArray)JsonConvert.DeserializeObject(mechineVideoAddMechine);

        }
        //产品信息
        public static JArray getVideoInfoList()
        {
            string mechineVideoInfoList = null;
            try
            {
                mechineVideoInfoList = RedisHelper.GetRedisModel<string>("_VideoInfoList");
            }
            catch
            {

            }
            if (string.IsNullOrEmpty(mechineVideoInfoList))
            {

                string sql = "select * from asm_video where  shType=1";

                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    RedisHelper.SetRedisModel<string>("_VideoInfoList", OperUtil.DataTableToJsonWithJsonNet(dt));
                    mechineVideoInfoList = OperUtil.DataTableToJsonWithJsonNet(dt);
                }
            }

            return mechineVideoInfoList == null ? new JArray() : (JArray)JsonConvert.DeserializeObject(mechineVideoInfoList);

        }
        //安卓屏幕的视频信息
        public static void getVideoList(string mechineID)
        {
            JArray mechineVideoAddMechine = getVideoAddMechine(mechineID);
            JArray companyVideoInfoList = getVideoInfoList();
            JArray newVideoInfoList = new JArray();
           
            for (int i = 0; i < mechineVideoAddMechine.Count; i++)
            {

                for (int j = 0; j < companyVideoInfoList.Count; j++)
                {
                    if (mechineVideoAddMechine[i]["videoID"].ToString()== companyVideoInfoList[j]["id"].ToString()) {
                        newVideoInfoList.Add(mechineVideoAddMechine[i]);
                        newVideoInfoList[newVideoInfoList.Count - 1]["path"] = companyVideoInfoList[j]["path"];
                        newVideoInfoList[newVideoInfoList.Count - 1]["description"] = companyVideoInfoList[j]["description"];
                        newVideoInfoList[newVideoInfoList.Count - 1]["name"] = companyVideoInfoList[j]["name"];
                    }
                    
                }
            }
            
            SetNewRedisInfo(mechineID + "_videoInfoSet", newVideoInfoList.ToString());
            
           
           
        }
        //为了在redis存最新的产品列表
        //联合几个redis碰撞出大屏上的产品信息，并判断是否更新
        public static void getAPPProductView(string mechineID, string companyID)
        {
            
            JArray mechineKcProduct = getMechineKcProduct(mechineID);
            JArray companyProductList = getCompanyProductList(companyID);
            JArray mechineSellOrderInfo = getSellOrderInfo(mechineID);
            Util.log("mechineSellOrderInfo", mechineID + "updateProductView.txt");
            JArray mechineLDList = getMechineLDList(mechineID);
            Util.log("getMechineLDList", mechineID + "updateProductView.txt");
            JArray XSTJList = getXSTJList(mechineID);
            JArray memberSellProduct = new JArray();
            JArray newMechineKcProduct = new JArray();
            JArray newCompanyProductList = new JArray();
            Util.log("newCompanyProductList", mechineID + "updateProductView.txt");
            #region
            //会员订购的转售产品
            for (int i = 0; i < mechineSellOrderInfo.Count; i++)
            {
                if(mechineSellOrderInfo[i]["createTime"].ToString() == DateTime.Now.ToString("yyyy-MM-dd"))
                {
                    for (int j = 0; j < companyProductList.Count; j++)
                    {

                        if (mechineSellOrderInfo[i]["productID"].ToString() == companyProductList[j]["productID"].ToString())
                        {
                            memberSellProduct.Add(new JObject());
                            memberSellProduct[memberSellProduct.Count - 1]["productID"] = companyProductList[j]["productID"];
                            memberSellProduct[memberSellProduct.Count - 1]["proName"] = companyProductList[j]["proName"];
                            memberSellProduct[memberSellProduct.Count - 1]["price0"] = mechineSellOrderInfo[i]["sellPrice"];
                            memberSellProduct[memberSellProduct.Count - 1]["price1"] = mechineSellOrderInfo[i]["sellPrice"];
                            memberSellProduct[memberSellProduct.Count - 1]["price2"] = mechineSellOrderInfo[i]["sellPrice"];
                            memberSellProduct[memberSellProduct.Count - 1]["price3"] = mechineSellOrderInfo[i]["sellPrice"];
                            memberSellProduct[memberSellProduct.Count - 1]["path"] = companyProductList[j]["path"];
                            memberSellProduct[memberSellProduct.Count - 1]["protype"] = companyProductList[j]["protype"];
                            memberSellProduct[memberSellProduct.Count - 1]["mechineID"] = companyProductList[j]["mechineID"];
                            memberSellProduct[memberSellProduct.Count - 1]["description"] = companyProductList[j]["description"];
                            memberSellProduct[memberSellProduct.Count - 1]["productSize"] = companyProductList[j]["productSize"];
                            memberSellProduct[memberSellProduct.Count - 1]["bzq"] = companyProductList[j]["bzq"];
                            memberSellProduct[memberSellProduct.Count - 1]["companyID"] = companyProductList[j]["companyID"];
                            memberSellProduct[memberSellProduct.Count - 1]["ljxs"] = companyProductList[j]["ljxs"];
                            memberSellProduct[memberSellProduct.Count - 1]["httpImageUrl"] = companyProductList[j]["httpImageUrl"];
                            memberSellProduct[memberSellProduct.Count - 1]["sluid"] = companyProductList[j]["sluid"];
                            memberSellProduct[memberSellProduct.Count - 1]["progg"] = companyProductList[j]["progg"];
                            memberSellProduct[memberSellProduct.Count - 1]["brandID"] = companyProductList[j]["brandID"];
                            memberSellProduct[memberSellProduct.Count - 1]["shortName"] = companyProductList[j]["shortName"];
                            memberSellProduct[memberSellProduct.Count - 1]["bh"] = companyProductList[j]["bh"];
                            memberSellProduct[memberSellProduct.Count - 1]["tag"] = companyProductList[j]["tag"];
                            memberSellProduct[memberSellProduct.Count - 1]["dstype"] = companyProductList[j]["dstype"];
                            memberSellProduct[memberSellProduct.Count - 1]["startSend"] = companyProductList[j]["startSend"];
                            memberSellProduct[memberSellProduct.Count - 1]["is_del"] = companyProductList[j]["is_del"];
                            memberSellProduct[memberSellProduct.Count - 1]["weight"] = 200;
                            memberSellProduct[memberSellProduct.Count - 1]["num"] ="100";
                            memberSellProduct[memberSellProduct.Count - 1]["type"] = "3";
                            memberSellProduct[memberSellProduct.Count - 1]["id"] = mechineSellOrderInfo[i]["id"];
                            memberSellProduct[memberSellProduct.Count - 1]["sftj"] = "";

                        }

                    }

                }
                
            }
            #endregion
            //循环去掉料道异常等无效的库存产品信息
            Util.log("mechineKcProduct", mechineID + "updateProductView.txt");
            for (int i = 0; i < mechineKcProduct.Count; i++)
            {

                for (int j = 0; j < mechineLDList.Count; j++)
                {

                    if (mechineKcProduct[i]["productID"].ToString() == mechineLDList[j]["productID"].ToString())
                    {
                        if (int.Parse(mechineLDList[j]["errorld_productNum"].ToString())>0) {
                            mechineKcProduct[i]["lsNum"] = (int.Parse(mechineKcProduct[i]["lsNum"].ToString()) - int.Parse(mechineLDList[j]["errorld_productNum"].ToString())).ToString();
                        }
                        newMechineKcProduct.Add(mechineKcProduct[i]);
                    }
                }

            }
            Util.log("newMechineKcProduct", mechineID + "updateProductView.txt");
            mechineKcProduct = newMechineKcProduct;
            //得到有效的零售产品信息
            for (int i = 0; i < companyProductList.Count; i++)
            {
                if (companyProductList[i]["dstype"].ToString() == "2" || companyProductList[i]["dstype"].ToString() == "3")
                {
                    newCompanyProductList.Add(companyProductList[i]);
                }
            }
            companyProductList = newCompanyProductList;
            //将机器正常产品列表加到转售的里，生成列表
            for (int i = 0; i < mechineKcProduct.Count; i++)
            {

                for (int j = 0; j < companyProductList.Count; j++)
                {

                    if (mechineKcProduct[i]["productID"].ToString() == companyProductList[j]["productID"].ToString())
                    {
                        memberSellProduct.Add(new JObject());
                        memberSellProduct[memberSellProduct.Count - 1]["productID"] = companyProductList[j]["productID"];
                        memberSellProduct[memberSellProduct.Count - 1]["proName"] = companyProductList[j]["proName"];
                        memberSellProduct[memberSellProduct.Count - 1]["price0"] = companyProductList[j]["price0"];
                        memberSellProduct[memberSellProduct.Count - 1]["price1"] = companyProductList[j]["price1"];
                        memberSellProduct[memberSellProduct.Count - 1]["price2"] = companyProductList[j]["price2"];
                        memberSellProduct[memberSellProduct.Count - 1]["price3"] = companyProductList[j]["price3"];
                        memberSellProduct[memberSellProduct.Count - 1]["path"] = companyProductList[j]["path"];
                        memberSellProduct[memberSellProduct.Count - 1]["protype"] = companyProductList[j]["protype"];
                        memberSellProduct[memberSellProduct.Count - 1]["mechineID"] = companyProductList[j]["mechineID"];
                        memberSellProduct[memberSellProduct.Count - 1]["description"] = companyProductList[j]["description"];
                        memberSellProduct[memberSellProduct.Count - 1]["productSize"] = companyProductList[j]["productSize"];
                        memberSellProduct[memberSellProduct.Count - 1]["bzq"] = companyProductList[j]["bzq"];
                        memberSellProduct[memberSellProduct.Count - 1]["companyID"] = companyProductList[j]["companyID"];
                        memberSellProduct[memberSellProduct.Count - 1]["ljxs"] = companyProductList[j]["ljxs"];
                        memberSellProduct[memberSellProduct.Count - 1]["httpImageUrl"] = companyProductList[j]["httpImageUrl"];
                        memberSellProduct[memberSellProduct.Count - 1]["sluid"] = companyProductList[j]["sluid"];
                        memberSellProduct[memberSellProduct.Count - 1]["progg"] = companyProductList[j]["progg"];
                        memberSellProduct[memberSellProduct.Count - 1]["brandID"] = companyProductList[j]["brandID"];
                        memberSellProduct[memberSellProduct.Count - 1]["shortName"] = companyProductList[j]["shortName"];
                        memberSellProduct[memberSellProduct.Count - 1]["bh"] = companyProductList[j]["bh"];
                        memberSellProduct[memberSellProduct.Count - 1]["tag"] = companyProductList[j]["tag"];
                        memberSellProduct[memberSellProduct.Count - 1]["dstype"] = companyProductList[j]["dstype"];
                        memberSellProduct[memberSellProduct.Count - 1]["startSend"] = companyProductList[j]["startSend"];
                        memberSellProduct[memberSellProduct.Count - 1]["is_del"] = companyProductList[j]["is_del"];
                        memberSellProduct[memberSellProduct.Count - 1]["weight"] = int.Parse(mechineKcProduct[i]["lsNum"].ToString()) <= 0 ? -100 : (string.IsNullOrEmpty(companyProductList[j]["weight"].ToString())? 0: int.Parse(companyProductList[j]["weight"].ToString()));
                        memberSellProduct[memberSellProduct.Count - 1]["num"] = mechineKcProduct[i]["lsNum"].ToString();
                        memberSellProduct[memberSellProduct.Count - 1]["type"] ="2";
                        memberSellProduct[memberSellProduct.Count - 1]["id"] = 0;
                        memberSellProduct[memberSellProduct.Count - 1]["sftj"] ="";
                        
                      
                    }
                }

            }
            Util.log("将机器正常产品列表加到转售的里结束", mechineID + "updateProductView.txt");
            memberSellProduct = new JArray(memberSellProduct.OrderByDescending(x => x["weight"]));
            Util.log("OrderByDescending", mechineID + "updateProductView.txt");
            //特价变更价格
            for (int i = 0; i < memberSellProduct.Count; i++)
            {
                string productID = memberSellProduct[i]["productID"].ToString();


                for (int n = 0; n < XSTJList.Count; n++)
                {
                    string _productID = XSTJList[n]["productID"].ToString();//限时活动产品ID
                    string type = XSTJList[n]["type"].ToString();
                    string startTime = XSTJList[n]["startTime"].ToString();
                    string endTime = XSTJList[n]["endTime"].ToString();
                    string timeSpan = XSTJList[n]["timeSpan"].ToString();
                    string price0 = XSTJList[n]["price0"].ToString();
                    string price1 = XSTJList[n]["price1"].ToString();
                    string price2 = XSTJList[n]["price2"].ToString();
                    string price3 = XSTJList[n]["price3"].ToString();
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
                                    memberSellProduct[i]["price0"] = price0;
                                    memberSellProduct[i]["price1"] = price1;
                                    memberSellProduct[i]["price2"] = price2;
                                    memberSellProduct[i]["price3"] = price3;
                                    memberSellProduct[i]["sftj"] = "1";
                                    break;
                                }

                            }
                            else if (type == "2")//阶段特价
                            {

                                memberSellProduct[i]["price0"] = price0;
                                memberSellProduct[i]["price1"] = price1;
                                memberSellProduct[i]["price2"] = price2;
                                memberSellProduct[i]["price3"] = price3;
                                memberSellProduct[i]["sftj"] = "1";
                                break;
                            }
                        }
                    }
                }

            }
            SetNewRedisInfo(mechineID + "_androidProductView", memberSellProduct.ToString());
            
        }
       

        //机器库存中去重后的产品列表
        public static JArray getMechineKcProduct(string mechineID)
        {
            string kcProduct = null;
            try
            {
                kcProduct = RedisHelper.GetRedisModel<string>(mechineID + "_KcProduct");
            }
            catch
            {

            }
            if (string.IsNullOrEmpty(kcProduct))
            {
                string sql = @"select t.* from (select asm_kcDetail.*, row_number() over(partition by productID order by id desc) rn from asm_kcDetail where mechineID =@mechineID) t where rn = 1";
                sql = sql.Replace("@mechineID", mechineID);
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    RedisHelper.SetRedisModel<string>(mechineID + "_KcProduct", OperUtil.DataTableToJsonWithJsonNet(dt));
                    kcProduct = OperUtil.DataTableToJsonWithJsonNet(dt);
                }
            }
            return kcProduct == null ? new JArray() : (JArray)JsonConvert.DeserializeObject(kcProduct);

        }
        //产品信息
        public static JArray getCompanyProductList(string companyID)
        {
            string companyProductList = null;
            try
            {
                companyProductList = RedisHelper.GetRedisModel<string>(companyID + "_ProductListSet");
            }
            catch
            {

            }
            if (string.IsNullOrEmpty(companyProductList))
            {

                string sql = @"select * from asm_product where  is_del = 0 AND companyID = @companyID";
                sql = sql.Replace("@companyID", companyID);
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    RedisHelper.SetRedisModel<string>(companyID + "_ProductListSet", OperUtil.DataTableToJsonWithJsonNet(dt));
                    companyProductList = OperUtil.DataTableToJsonWithJsonNet(dt);
                }
            }

            return companyProductList == null ? new JArray() : (JArray)JsonConvert.DeserializeObject(companyProductList);
        }
        //可用料道信息
        public static JArray getMechineLDList(string mechineID)
        {
            string mechineLDList = null;
            try
            {
                mechineLDList = RedisHelper.GetRedisModel<string>(mechineID + "_LDList");
            }
            catch
            {

            }
            if (string.IsNullOrEmpty(mechineLDList))
            {

                //string sql = @"SELECT distinct productID FROM  asm_ldInfo WHERE  mechineID =@mechineID AND zt != 1 AND productID != '' ";
                string sql = @"SELECT productID, sum(case when zt = 0 then 0 else ld_productNum end) errorld_productNum FROM  asm_ldInfo WHERE  mechineID = @mechineID  AND productID != '' GROUP BY productID";
                
                sql = sql.Replace("@mechineID", mechineID);
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    RedisHelper.SetRedisModel<string>(mechineID + "_LDList", OperUtil.DataTableToJsonWithJsonNet(dt));
                    mechineLDList = OperUtil.DataTableToJsonWithJsonNet(dt);
                }
            }
            return mechineLDList == null ? new JArray() : (JArray)JsonConvert.DeserializeObject(mechineLDList);
        }
        //限时特价信息
        public static JArray getXSTJList(string mechineID)
        {
            string mechineXSTJList = null;
            try
            {
                mechineXSTJList = RedisHelper.GetRedisModel<string>(mechineID + "_XSTJList");
            }
            catch
            {

            }
            if (string.IsNullOrEmpty(mechineXSTJList))
            {

                string sql = @"select * from asm_xstj where mechineID=@mechineID  order by timeSpan desc";
                sql = sql.Replace("@mechineID", mechineID);
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    RedisHelper.SetRedisModel<string>(mechineID + "_XSTJList", OperUtil.DataTableToJsonWithJsonNet(dt));
                    mechineXSTJList = OperUtil.DataTableToJsonWithJsonNet(dt);
                }
            }

            return mechineXSTJList == null ? new JArray() : (JArray)JsonConvert.DeserializeObject(mechineXSTJList);
        }

        //是否开启会员价
        public static string getMemberprice(string companyID)
        {
            string memberprice = RedisHelper.GetRedisModel<string>(companyID + "_memberprice");
            if (string.IsNullOrEmpty(memberprice))
            {
                string sql3 = "select * from asm_tqlist where companyID=" + companyID;
                DataTable d3 = DbHelperSQL.Query(sql3).Tables[0];
                if (d3.Rows.Count > 0)
                {
                    RedisHelper.SetRedisModel<string>(companyID + "_memberprice", d3.Rows[0]["memberprice"].ToString());
                    memberprice = d3.Rows[0]["memberprice"].ToString();
                }
                else
                {
                    RedisHelper.SetRedisModel<string>(companyID + "_memberprice", "0");
                    memberprice = "0";
                }

            }
            return memberprice;

        }

        //更新传值的最新redis
        public static void SetNewRedisInfo(string rediskey, string newDetail) {
            //取出之前存的产品redis
            string oldRedisString = RedisHelper.GetRedisModel<string>(rediskey);
            //为空则需要添加并通知机器更新
            if (string.IsNullOrEmpty(oldRedisString))
            {
                long times = Util.GetTimeStamp();
                JObject _androidRedisView = new JObject();
                _androidRedisView["androidNo"] = times;
                _androidRedisView["androidInfoDetail"] = newDetail;
               
                RedisHelper.SetRedisModel<string>(rediskey, _androidRedisView.ToString());
            }
            else
            {
                //此处需要先判断新产品列表和老的区别，有区别则更新，没区别不动
                //将之前redis的产品详情取出
                JObject _oldAndroidRedisView = (JObject)JsonConvert.DeserializeObject(oldRedisString);
                //不一样
                if (_oldAndroidRedisView["androidInfoDetail"].ToString() != newDetail)
                {

                    long times = Util.GetTimeStamp();
                    //更新产品信息详情以及序号
                    _oldAndroidRedisView["androidInfoDetail"] = newDetail;
                    _oldAndroidRedisView["androidNo"] = times;
                    RedisHelper.SetRedisModel<string>(rediskey, _oldAndroidRedisView.ToString());
                }
            }
        }
    }
}