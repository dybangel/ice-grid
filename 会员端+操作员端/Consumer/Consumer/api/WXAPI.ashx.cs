using autosell_center.util;
using Consumer.cls;
using DBUtility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Xml;
using uniondemo.com.allinpay.syb;
using WZHY.Common.DEncrypt;
using static Consumer.cls.MinTemplageMsg;

namespace Consumer.api
{
    /// <summary>
    /// WXAPI 的摘要说明
    /// </summary>
    public class WXAPI : IHttpHandler
    {
        //public string Appid = "wxcdb958e5f684f086";
        //public string Secret = "b0fb1e727fa154818219fc84f491f46c";
        public string grant_type = "authorization_code";
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string text = context.Request["action"];
            string key;
            switch (key = text)
            {
                case "getPhoneNum":  //获取用户手机号
                    this.getPhoneNum(context);
                    return;
                case "pay":  //发起支付
                    this.pay(context);
                    return;
                case "payMany":    //小程序一次购买多份付款
                    this.payMany(context);
                    return;
                    
                case "getOpenID":  //获取openid
                    this.getOpenID(context);
                    return;
                case "getMechineList"://获取机器列表
                    this.getMechineList(context);
                    return;
                case "getType"://获取商品类型
                    this.getType(context);
                    return;
                case "getProductList"://获取商品列表
                    this.getProductList(context);
                    return;
                case "getProduct":
                    this.getProduct(context);
                    return;
                case "getProductActivity":
                    this.getProductActivity(context);
                    return;
                case "getProductActivity2":
                    this.getProductActivity2(context);
                    return;
                case "userlogin":
                    this.userlogin(context);
                    return;
                case "userlogin2":
                    this.userlogin2(context);
                    return;
                case "registUser"://registUserByPhone
                    this.registUser(context);
                    return;
                case "registUserByPhone"://registUserByPhone
                    this.registUserByPhone(context);
                    return;
                case "addOrder":
                    this.addOrder(context);
                    return;
                //小程序一次购买多份
                case "addOrderMany":
                    this.addOrderMany(context);
                    return;
                    
                case "getOrderList":
                    this.getOrderList(context);
                    return;
                case "getMemberInfo":
                    this.getMemberInfo(context);
                    return;
                case "getPayActivityList":
                    this.getPayActivityList(context);
                    return;
                case "getPayActivityList2":
                    this.getPayActivityList2(context);
                    return;
                case "payCZ":
                    this.payCZ(context);
                    return;
                case "getMoneyChange":
                    this.getMoneyChange(context);
                    return;
                case "updateMemberName":
                    this.updateMemberName(context);
                    return;
                case "updatePwd":
                    this.updatePwd(context);
                    return;
                case "updateMemberBirthday":
                    this.updateMemberBirthday(context);
                    return;
                case "transferAccounts"://转账
                    this.transferAccounts(context);
                    return;
                case "getCode"://获取取货码
                    this.getCode(context);
                    return;
                case "getOrderInfo"://获取订单信息 根据orderID
                    this.getOrderInfo(context);
                    return;
                case "getOrderDetail"://获取订单明细 根据单号
                    this.getOrderDetail(context);
                    return;
                case "chgDateTime":// 调整订单派送时间
                    this.chgDateTime(context);
                    return;
                case "sellProduct":// 半价出售订单
                    this.sellProduct(context);
                    return;
                case "sendMessage":
                    this.sendMessage(context);
                    return;
                case "ch"://出货
                    this.ch(context);
                    return;
                case "getAdverseList"://获取广告列表
                    this.getAdverseList(context);
                    return;
                case "getLBTList"://获取轮播列表
                    this.getLBTList(context);
                    return;
                case "getNoticeList"://获取通知
                    this.getNoticeList(context);
                    return;
                case "getNotice"://更新通知
                    this.getNotice(context);
                    return;
                case "getNoticeListCount"://获取未读通知数量
                    this.getNoticeListCount(context);
                    return;
                case "getDJList"://获取等级列表
                    this.getDJList(context);
                    return;
                case "dhProduct"://兑换产品
                    this.dhProduct(context);
                    return;
                case "getOrderSYNum"://获取订单剩余订单数量
                    this.getOrderSYNum(context);
                    return;
                case "getProductByOrderNO"://根据订单号获取产品及订单信息
                    this.getProductByOrderNO(context);
                    return;
                case "updatePhone"://更新手机号
                    this.updatePhone(context);
                    return;
                case "getTQList"://获取特权列表
                    this.getTQList(context);
                    return;
                case "getProductByCode"://根据订奶码获取产品 
                    this.getProductByCode(context);
                    return;
                case "addDGOrder"://插入兑换订单记录  
                    this.addDGOrder(context);
                    return;
                case "getDecryptUserInfo"://解密会员信息
                    this.getDecryptUserInfo(context);
                    return;
                case "sendTemplateMessage"://发送模板消息
                    this.sendTemplateMessage(context);
                    return;
                case "getHidStr"://发送模板消息
                    this.getHidStr(context);
                    return;
                case "setLocation"://设置机器地理位置
                    this.setLocation(context);
                    return;
                case "getMoney"://获取会员价格 用于测试
                    this.getMoney(context);
                    return;
                case "payCode"://充值码兑换
                    this.payCode(context);
                    return;
                case "updateTest":
                    this.updateTest(context);
                    return;
                case "dhProductceshi":
                    this.dhProductceshi(context);
                    return;
                case "setDelay":
                    this.setDelay(context);
                    return;

            }
        }
        public void dhProductceshi(HttpContext context)
        {
            try
               // companyID = 14; mechineID = 51; unionID = owhCR0ef_sajVc3jwc0g_kzYMg7U; openID = oJ1cB5UTRUv1N8gFkGnAdfvrhUrw; oldOrderNO = 511567141169066; oldProductID = 311;
           // newProductID = 319; startDate = 2019 - 09 - 05
            {
                string companyID = "14";//14
                string mechineID ="51";//56
                string unionID = "owhCR0ef_sajVc3jwc0g_kzYMg7U";//owhCR0esai2hPXH4lYkeLMAcccuE
                string openID = "oJ1cB5UTRUv1N8gFkGnAdfvrhUrw";//oJ1cB5bVOpZSma4Sy_wb0eMsmUqw
                string oldOrderNO = "511567141169066"; //251566194618366
                string newOrderNO = OperUtil.getOrderNO(mechineID); ;
                string oldProductID = "311";//424
                string newProductID = "319";//439
                string startDate = "2019-09-05";//首送日期2019-08-28
                string days = "100";//剩余天数176
                string chaMoney = "-1";//差额 大于0证明需要会员微信支付  小于0证明需要退还给会员差额  668.80
                Util.Debuglog("companyID=" + companyID + ";mechineID=" + mechineID + ";unionID=" + unionID + ";openID=" + openID + ";oldOrderNO=" + oldOrderNO + ";oldProductID=" + oldProductID + ";newProductID=" + newProductID + ";startDate=" + startDate, "dhProduct.txt");
                if (string.IsNullOrEmpty(mechineID) || string.IsNullOrEmpty(unionID) || string.IsNullOrEmpty(openID) || string.IsNullOrEmpty(oldOrderNO) || string.IsNullOrEmpty(newOrderNO) || string.IsNullOrEmpty(oldProductID) || string.IsNullOrEmpty(newProductID) || string.IsNullOrEmpty(chaMoney))
                {
                    context.Response.Write("{\"code\":\"500\",\"msg\":\"参数不全\"}");
                    return;
                }
                if (double.Parse(chaMoney) > 0)
                {
                    //1 首先修改旧订单状态  只允许配送中的订单兑换
                    string sql1 = "select * from  asm_orderlist where orderNO='" + oldOrderNO + "'";
                    Util.Debuglog("sql1=" + sql1, "dhProduct.txt");
                    DataTable d1 = DbHelperSQL.Query(sql1).Tables[0];
                    if (d1.Rows.Count > 0)
                    {
                        if (d1.Rows[0]["orderZT"].ToString() == "1")
                        {
                            //此处计算剩余天数
                            string sql5 = "select count(*) num from  asm_orderlistDetail where orderNO='" + oldOrderNO + "' and zt=5";
                            Util.Debuglog("sql5=" + sql5, "dhProduct.txt");
                            DataTable d5 = DbHelperSQL.Query(sql5).Tables[0];
                            //2019-08-29调整，因为之前的方法会出现没付款但是就直接给更老订单状态的问题
                            //修改旧订单状态为4 兑换 修改订单明细剩余订单zt=7
                            //List<string> list = new List<string>();
                            //string sql2 = "update asm_orderlist set orderZT=4 where orderNO='" + oldOrderNO + "'";
                            //string sql3 = "update asm_orderlistDetail set zt=7 where mechineID=" + mechineID + " and zt=5 and orderNO='" + oldOrderNO + "'";
                            //list.Add(sql2);
                            //list.Add(sql3);
                            //int a = DbHelperSQL.ExecuteSqlTran(list);
                            if (int.Parse(d5.Rows[0]["num"].ToString()) > 0)
                            {
                                //创建新订单 
                                string psCycleID = d1.Rows[0]["activityID"].ToString();//参加活动的ID
                                                                                       //1 1日1送 2 2日1送 3 3日1送 4 周一至周五 5 周末送
                                string psMode = d1.Rows[0]["psMode"].ToString();
                                string sql4 = "select * from  asm_product where productID=" + newProductID;
                                DataTable d4 = DbHelperSQL.Query(sql4).Tables[0];
                                if (d4.Rows.Count > 0)
                                {

                                    double newprice = double.Parse(d4.Rows[0]["price0"].ToString());
                                    double oldprice = double.Parse(d1.Rows[0]["price"].ToString());//旧订单表里存的单价
                                    int num = int.Parse(d5.Rows[0]["num"].ToString());
                                    double totalMoney = newprice * num;
                                    string result = addOrder2(psCycleID, psMode, openID, newProductID, startDate, companyID, mechineID, totalMoney.ToString("f2"), days, newOrderNO);
                                    JObject jo = (JObject)JsonConvert.DeserializeObject(result);
                                    string code = jo["code"].ToString();
                                    if (code == "200")
                                    {
                                        //插入兑换记录
                                        string memberID = d1.Rows[0]["memberID"].ToString();
                                        string sql6 = "insert into [dbo].[asm_duihuan](memberID,oldOrderNO,newOrderNO,oldProductID,newProductID,bce,dhTime,bz)values('" + memberID + "','" + oldOrderNO + "','" + newOrderNO + "','" + oldProductID + "','" + newProductID + "','" + (newprice - oldprice) * num + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','当前剩余天数" + num + "')";
                                        Util.Debuglog("sql6=" + sql6, "dhProduct.txt");
                                        DbHelperSQL.ExecuteSql(sql6);
                                        context.Response.Write(result);
                                        return;
                                    }
                                    else
                                    {
                                        context.Response.Write(result);
                                        return;
                                    }
                                }
                                else
                                {
                                    context.Response.Write("{\"code\":\"500\",\"msg\":\"产品信息获取异常\"}");
                                    return;
                                }

                            }
                            else
                            {
                                context.Response.Write("{\"code\":\"500\",\"msg\":\"修改订单异常\"}");
                                return;
                            }

                        }
                        else
                        {
                            context.Response.Write("{\"code\":\"500\",\"msg\":\"该笔订单状态不允许兑换\"}");
                            return;
                        }


                    }
                }
                else
                {
                    //需要退差额的
                    //1 首先修改旧订单状态  只允许配送中的订单兑换
                    chaMoney = System.Math.Abs(double.Parse(chaMoney)) + "";
                    string sql1 = "select * from  asm_orderlist where orderNO='" + oldOrderNO + "'";
                    Util.Debuglog("sql1=" + sql1, "dhProduct.txt");
                    DataTable d1 = DbHelperSQL.Query(sql1).Tables[0];
                    if (d1.Rows.Count > 0)
                    {
                        if (d1.Rows[0]["orderZT"].ToString() == "1")
                        {
                            //此处计算剩余天数
                            string sql5 = "select count(*) num from  asm_orderlistDetail where orderNO='" + oldOrderNO + "' and zt=5";
                            Util.Debuglog("sql5=" + sql5, "dhProduct.txt");
                            DataTable d5 = DbHelperSQL.Query(sql5).Tables[0];
                            //修改旧订单状态为4 兑换 修改订单明细剩余订单zt=7
                            List<string> list = new List<string>();
                            string sql2 = "update asm_orderlist set orderZT=4 where orderNO='" + oldOrderNO + "'";
                            string sql3 = "update asm_orderlistDetail set zt=7 where mechineID=" + mechineID + " and zt=5 and orderNO='" + oldOrderNO + "'";
                            list.Add(sql2);
                            list.Add(sql3);
                            int a = DbHelperSQL.ExecuteSqlTran(list);
                            if (a > 0)
                            {
                                //创建新订单 
                                string psCycleID = d1.Rows[0]["activityID"].ToString();//参加活动的ID
                                                                                       //1 1日1送 2 2日1送 3 3日1送 4 周一至周五 5 周末送
                                string psMode = d1.Rows[0]["psMode"].ToString();
                                string sql4 = "select * from  asm_product where productID=" + newProductID;
                                Util.Debuglog("sql4=" + sql4, "dhProduct.txt");
                                DataTable d4 = DbHelperSQL.Query(sql4).Tables[0];
                                if (d4.Rows.Count > 0)
                                {

                                    double newprice = double.Parse(d4.Rows[0]["price0"].ToString());
                                    double oldprice = double.Parse(d1.Rows[0]["price"].ToString());//旧订单表里存的单价
                                    int num = int.Parse(d5.Rows[0]["num"].ToString());
                                    double totalMoney = newprice * num;
                                    Util.Debuglog("totalMoney=" + totalMoney, "dhProduct.txt");
                                    string result = addOrder3(psCycleID, psMode, openID, newProductID, startDate, companyID, mechineID, totalMoney.ToString("f2"), days, chaMoney, newOrderNO);
                                    Util.Debuglog("result=" + result, "dhProduct.txt");
                                    JObject jo = (JObject)JsonConvert.DeserializeObject(result);
                                    string code = jo["code"].ToString();
                                    if (code == "200")
                                    {
                                        //插入兑换记录
                                        string memberID = d1.Rows[0]["memberID"].ToString();
                                        string sql6 = "insert into [dbo].[asm_duihuan](memberID,oldOrderNO,newOrderNO,oldProductID,newProductID,bce,dhTime,bz)values('" + memberID + "','" + oldOrderNO + "','" + newOrderNO + "','" + oldProductID + "','" + newProductID + "','" + (newprice - oldprice) * num + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','当前剩余天数" + num + "')";
                                        Util.Debuglog("sql6=" + sql6, "dhProduct.txt");
                                        DbHelperSQL.ExecuteSql(sql6);
                                        context.Response.Write(result);
                                        return;
                                    }
                                    else
                                    {
                                        context.Response.Write(result);
                                        return;
                                    }
                                }
                                else
                                {
                                    context.Response.Write("{\"code\":\"500\",\"msg\":\"产品信息获取异常\"}");
                                    return;
                                }
                            }
                            else
                            {
                                context.Response.Write("{\"code\":\"500\",\"msg\":\"修改订单异常\"}");
                                return;
                            }
                        }
                        else
                        {
                            context.Response.Write("{\"code\":\"500\",\"msg\":\"该笔订单状态不允许兑换\"}");
                            return;
                        }

                    }
                }

            }
            catch (Exception e)
            {
                Util.Debuglog("err=" + e, "dhProduct.txt");
                context.Response.Write("{\"code\":\"500\",\"msg\":\"系统异常\"}");
                return;
            }
        }
        public void updateTest(HttpContext context)
        {
            string day = context.Request["day"].ToString();
            string zq = context.Request["zq"].ToString();
            string str=OperUtil.getSelDate(zq, day, "2019-07-02");
            context.Response.Write(str);
        }
        public void payCode(HttpContext context)
        {
            try
            {
                string unionID = context.Request["unionID"].ToString();
                string minOpenID = context.Request["minOpenID"].ToString();
                string cardNO = context.Request["cardNO"].ToString();
                string pwd = context.Request["pwd"].ToString();
                string companyID = context.Request["companyID"].ToString();
                Util.Debuglog("unionID="+ unionID+ ";minOpenID="+ minOpenID+ ";cardNO="+ cardNO+ ";pwd="+ pwd+ ";companyID="+ companyID, "payCode.txt");
                if (string.IsNullOrEmpty(unionID)||string.IsNullOrEmpty(minOpenID)||unionID=="undefined"|| minOpenID== "undefined")
                {
                    context.Response.Write("{\"code\":\"500\",\"msg\":\"会员信息读取失败\"}");
                    return;
                }
                if (string.IsNullOrEmpty(cardNO))
                {
                    context.Response.Write("{\"code\":\"500\",\"msg\":\"充值卡号不能为空\"}");
                    return;
                }
                if (string.IsNullOrEmpty(pwd))
                {
                    context.Response.Write("{\"code\":\"500\",\"msg\":\"充值卡密码不能为空\"}");
                    return;
                }
                string sql = "select * from asm_member where unionID='"+unionID+"'";
                DataTable DM = DbHelperSQL.Query(sql).Tables[0];
                if (DM.Rows.Count > 0)
                {
                    string sql1 = "select * from  asm_payCodeList where cardNO='"+cardNO+"' and pwd='"+pwd+"'";
                    DataTable d1 = DbHelperSQL.Query(sql1).Tables[0];
                    if (d1.Rows.Count<=0)
                    {
                        context.Response.Write("{\"code\":\"500\",\"msg\":\"充值卡账号或密码错误\"}");
                        return;
                    }
                    if (d1.Rows[0]["statu"].ToString()=="2")
                    {
                        context.Response.Write("{\"code\":\"500\",\"msg\":\"充值卡已经使用过\"}");
                        return;
                    }
                    if (d1.Rows[0]["statu"].ToString() == "3")
                    {
                        context.Response.Write("{\"code\":\"500\",\"msg\":\"充值卡已经作废\"}");
                        return;
                    }
                    string startTime = d1.Rows[0]["startTime"].ToString();
                    string endTime = d1.Rows[0]["endTime"].ToString();
                    if (DateTime.Parse(startTime) < DateTime.Now && DateTime.Parse(endTime) > DateTime.Now)
                    {
                        //走正常流程
                        //1更新卡状态为已使用
                        string Sql = "update asm_payCodeList set statu=2,memberID='"+DM.Rows[0]["id"].ToString()+"' where id="+d1.Rows[0]["id"].ToString();
                        int a = DbHelperSQL.ExecuteSql(Sql);
                        if (a > 0)
                        {
                            //更新会员余额
                            string update1 = "update asm_member set AvailableMoney=AvailableMoney+" + d1.Rows[0]["mzMoney"].ToString() + ",sumRecharge=sumRecharge+" + d1.Rows[0]["mzMoney"].ToString() + " where unionID='" + unionID + "'";
                            int b = DbHelperSQL.ExecuteSql(update1);
                            if (b > 0)
                            {
                                //插入充值记录
                                Util.chgMoney(DM.Rows[0]["id"].ToString(), d1.Rows[0]["mzMoney"].ToString(), "充值卡充值", "充值卡充值：" + d1.Rows[0]["mzMoney"].ToString() + "元", "1");
                                Util.insertNotice(DM.Rows[0]["id"].ToString(), "充值到账通知", "充值成功到账金额："+ double.Parse(d1.Rows[0]["mzMoney"].ToString()).ToString("f2") +"元", "");
                                //发送模板消息
                                if (!string.IsNullOrEmpty(DM.Rows[0]["openID"].ToString()))
                                {
                                     
                                    string openID = DM.Rows[0]["openID"].ToString();
                                    Util.Debuglog("发送模板消息=companyID=" + companyID + ";openID=" + openID, "充值卡充值.txt");
                                    wxHelper wx = new wxHelper(companyID);
                                    string data = TemplateMessage.success_cz(openID, "Tmin60E6DJtBO962B_5BEzVRC3Rbdv1JrKQNuzoY0Gw",
                                        "充值成功通知", d1.Rows[0]["mzMoney"].ToString(), (double.Parse(DM.Rows[0]["AvailableMoney"].ToString())+double.Parse(d1.Rows[0]["mzMoney"].ToString())).ToString("f2"), "充值时间:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                    TemplateMessage.SendTemplateMsg(wx.IsExistAccess_Token(companyID), data);
                                }
                                context.Response.Write("{\"code\":\"200\",\"msg\":\"充值成功\"}");
                                return;
                            }
                            else {
                                context.Response.Write("{\"code\":\"500\",\"msg\":\"充值失败，请联系销售人员\"}");
                                return;
                            }
                        }
                      
                    }
                    else {
                        context.Response.Write("{\"code\":\"500\",\"msg\":\"充值卡过期\"}");
                        return;
                    }

                }
                else {
                    context.Response.Write("{\"code\":\"500\",\"msg\":\"会员信息读取失败\"}");
                    return;
                }
            }
            catch {

            }
        }
        public void getMoney(HttpContext context)
        {
           string money= Util.getNewProductPrice("357", "25","3");
            context.Response.Write("{\"code\":\"200\",\"msg\":\""+ money + "\"}");
        }
        public void setLocation(HttpContext context)
        {
            try
            {
                string latitude = context.Request["latitude"].ToString();
                string longitude = context.Request["longitude"].ToString();
                string bh = context.Request["bh"].ToString();
                Util.Debuglog("latitude="+ latitude+ ";longitude="+ longitude+";bh="+bh, "setLocation.txt");
                if (string.IsNullOrEmpty(latitude)||string.IsNullOrEmpty(longitude)||string.IsNullOrEmpty(bh))
                {
                    context.Response.Write("{\"code\":\"500\",\"msg\":\"参数不全\"}");
                    return;
                }
                string sql = "select * from asm_mechine where bh='"+bh+"'";
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count<=0)
                {
                    context.Response.Write("{\"code\":\"500\",\"msg\":\"当前机器编号不存在\"}");
                    return;
                }
                string update = "update asm_mechine set zdx='"+longitude+"',zdy='"+latitude+"' where bh='"+bh+"'";
                int a= DbHelperSQL.ExecuteSql(update);
                if (a > 0)
                {
                    if (dt.Rows[0]["id"].ToString() == "68"||dt.Rows[0]["id"].ToString() == "69")
                    {
                        string mechineInfo = RedisUtil.getMechine(dt.Rows[0]["id"].ToString());
                        JArray jay = RedisUtil.DeserializeObject(mechineInfo);

                        jay[0]["zdx"] = longitude;
                        jay[0]["zdy"] = latitude;
                        RedisHelper.SetRedisModel<string>(dt.Rows[0]["id"].ToString() + "_mechineInfoSet", JsonConvert.SerializeObject(jay));

                    }
                    context.Response.Write("{\"code\":\"200\",\"msg\":\"设置成功\"}");
                    return;
                }
                else {
                    context.Response.Write("{\"code\":\"500\",\"msg\":\"设置失败\"}");
                    return;
                }
            }
            catch {

            }
        }
        public void getHidStr(HttpContext context)
        {
            try
            {
                string companyID = context.Request["companyID"].ToString();
                string sql = "select *  from  asm_company where id="+companyID;
                Util.Debuglog("sql="+ sql, "getHidStr.txt");
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0 && !string.IsNullOrEmpty(dt.Rows[0]["hidStr"].ToString()))
                {
                    context.Response.Write("{\"code\":\"200\",\"db\":\""+ dt.Rows[0]["hidStr"].ToString() + "\"}");
                    return;
                }
                else {
                    context.Response.Write("{\"code\":\"500\",\"msg\":\"查询异常\"}");
                    return;
                }
            }
            catch {
                context.Response.Write("{\"code\":\"500\",\"msg\":\"系统异常\"}");
                return;
            }
        }
        /// <summary>
        /// 发送小程序模板消息
        /// </summary>
        /// <param name="context"></param>
        public void sendTemplateMessage(HttpContext context)
        {
            try
            {
                string touser = context.Request["touser"].ToString();//接收者
                Util.Debuglog("touser=" + touser, "sendTemplateMessage.txt");
                string formId = context.Request["formId"].ToString();//提交表单的ID
                Util.Debuglog("formId=" + formId, "sendTemplateMessage.txt");
                string type = context.Request["type"].ToString();//类型 1会员注册2充值3订单支付4消费成功5订单出货6取货通知
                Util.Debuglog("type=" + type, "sendTemplateMessage.txt");
                string companyID = context.Request["companyID"].ToString();
                Util.Debuglog("touser=" + touser+ ";formId="+ formId+ ";type="+ type+ ";companyID="+ companyID, "sendTemplateMessage.txt");
                string sql = "select minAppid,minSecret from asm_company where id=" + companyID;
                Util.Debuglog("sql=" + sql, "sendTemplateMessage.txt");
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    string sqlM = "select * from asm_member where minOpenID='" + touser + "'";
                    DataTable dM = DbHelperSQL.Query(sqlM).Tables[0];
                    if (dM.Rows.Count > 0)
                    {
                        if (type == "1")
                        {
                            string result=MinTemplageMsg.sendRegist(touser, formId, dM.Rows[0]["name"].ToString(), "恭喜您！注册成功，成为“生鲜时逐”新鲜售奶机普通会员！点击立即查看→");
                            Util.Debuglog("会员注册="+ result, "sendTemplateMessage.txt");
                            sendTemplateMsg(result,companyID);
                        }
                        else if (type == "2")
                        {
                            string orderNO = context.Request["orderNO"].ToString();
                            string money = context.Request["money"].ToString();
                            string zsMoney = context.Request["zsMoney"].ToString();
                            string bz = "会员钱包余额为售奶机消费积分，限机器处购买使用。点击查看会员钱包";
                            string result=MinTemplageMsg.sendCZ(touser, formId,money,orderNO,zsMoney,bz);
                            Util.Debuglog("会员充值=" + result, "sendTemplateMessage.txt");
                            sendTemplateMsg(result, companyID);
                        }
                        else if (type == "3")
                        {
                            string orderNO = context.Request["orderNO"].ToString();
                            string money = context.Request["money"].ToString();
                            string productName = context.Request["productName"].ToString();
                            string address = context.Request["address"].ToString();
                            string bz = "“生鲜时逐”订奶订单已生成，鲜活即将配送到家。点击查看订单配送计划";
                            string result=MinTemplageMsg.sendPay(touser,formId,orderNO,money,productName,bz,address);
                            sendTemplateMsg(result, companyID);
                        }
                        else if (type == "4")
                        {
                            //string orderNO = context.Request["orderNO"].ToString();
                            //string name = context.Request["name"].ToString();
                            //string money = context.Request["money"].ToString();
                            //string payType = context.Request["payType"].ToString();
                            //string bz = "机器已出货，请尽快推开机器左下方推板取出奶品，超过1分钟未取视为丢弃奶品，推板将关闭。点击查看会员情况";
                            //string result=MinTemplageMsg.sendConsume(touser,formId,orderNO,name,money,payType,bz);
                            //sendTemplateMsg(result, companyID);
                        }
                        else if (type == "5")
                        {
                            string productName = context.Request["productName"].ToString();
                            string money = context.Request["money"].ToString();
                            string info = context.Request["info"].ToString();
                            string result=MinTemplageMsg.sendCH(touser,formId,productName,money,info);
                            sendTemplateMsg(result, companyID);
                        }
                        else if (type == "6")
                        {
                            string orderNO = context.Request["orderNO"].ToString();
                            string productName = context.Request["productName"].ToString();
                            string address = context.Request["address"].ToString();
                            string code = context.Request["code"].ToString();
                            string bz = "今天的奶品已新鲜到货，请尽快到售奶机处取出，营养更佳。当日24:00后仍未取，将回收处理";
                            string noticeInfo = context.Request["noticeInfo"].ToString();
                            string phone = context.Request["phone"].ToString();
                            string result=MinTemplageMsg.sendQH(touser,formId,orderNO,productName,address,code,bz,noticeInfo,phone);
                            sendTemplateMsg(result, companyID);
                        }
                    }
                }
            }
            catch
            {
                
            }
        }
        public void getProductByCode(HttpContext context)
        {
            try
            {
                string minOpenID = context.Request["minOpenID"].ToString();
                string unionID = context.Request["unionID"].ToString();
                string companyID = context.Request["companyID"].ToString();
                string code = context.Request["code"].ToString();
                Util.Debuglog("minOpenID=" + minOpenID + ";unionID=" + unionID + ";companyID=" + companyID + ";code=" + code, "getProductByCode.txt");
                string sql = "select* from asm_dgOrder where orderCode='" + code + "'";
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];

                string count = RedisHelper.GetRedisModel<string>("code" + minOpenID);
                if (string.IsNullOrEmpty(count))
                {
                    RedisHelper.SetRedisModel<string>("code" + minOpenID, "1", new TimeSpan(0, 1, 0));
                }
                else
                {
                    if (int.Parse(count) > 3)
                    {
                        
                        context.Response.Write("{\"code\":\"500\",\"msg\":\"当前操作过于频繁1分钟后重试\"}");
                        return;
                    }
                    count = (int.Parse(count) + 1).ToString();
                    RedisHelper.SetRedisModel<string>("code" + minOpenID, count, new TimeSpan(0, 1, 0));
                }


                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["status"].ToString() == "1")
                    {
                        context.Response.Write("{\"code\":\"500\",\"msg\":\"该订奶码已经使用\"}");
                        return;
                    }
                    //返回对应的产品信息
                    string sqlp = "select * from  asm_product where bh='" + dt.Rows[0]["productCode"].ToString() + "'";
                    DataTable dp = DbHelperSQL.Query(sqlp).Tables[0];
                    if (dp.Rows.Count <= 0)
                    {
                        context.Response.Write("{\"code\":\"500\",\"msg\":\"订购的产品信息不存在\"}");
                        return;
                    }
                    context.Response.Write("{\"code\":\"200\",\"db\":" + OperUtil.DataTableToJsonWithJsonNet(dp) + ",\"dgOrder\":" + OperUtil.DataTableToJsonWithJsonNet(dt) + "}");
                    return;
                }
                else
                {
                    context.Response.Write("{\"code\":\"500\",\"msg\":\"该订奶码不存在\"}");
                    return;
                }
            }
            catch
            {
                context.Response.Write("{\"code\":\"500\",\"msg\":\"系统异常\"}");
                return;
            }
        }
        public void getTQList(HttpContext context)
        {
            try
            {
                string companyID = context.Request["companyID"].ToString();
                string sql1 = "select top 3 * from  asm_tqlist where companyID=" + companyID;
                DataTable d1 = DbHelperSQL.Query(sql1).Tables[0];
                if (d1.Rows.Count > 0)
                {
                    if (d1.Rows[0]["czswitch"].ToString() == "1")
                    {
                        string sql2 = "select * from [dbo].[asm_tqdetail] where companyID=" + companyID;
                        DataTable d2 = DbHelperSQL.Query(sql2).Tables[0];
                        if (d2.Rows.Count > 0)
                        {
                            context.Response.Write("{\"code\":\"200\",\"db\":" + OperUtil.DataTableToJsonWithJsonNet(d2) + "}");
                            return;
                        }
                    }
                }
                context.Response.Write("{\"code\":\"500\",\"msg\":\"系统未开启充值活动\"}");
                return;
            }
            catch
            {
                context.Response.Write("{\"code\":\"500\",\"msg\":\"系统异常\"}");
                return;
            }
        }
        public void updatePhone(HttpContext context)
        {
            try
            {
                string minOpenID = context.Request["minOpenID"].ToString();
                string unionID = context.Request["unionID"].ToString();
                string phone = context.Request["phone"].ToString();
                string companyID = context.Request["companyID"].ToString();
                Util.Debuglog("minOpenID=" + minOpenID + ";unionID=" + unionID + ";phone=" + phone + ";companyID=" + companyID, "修改手机号.txt");
                if (string.IsNullOrEmpty(minOpenID) || string.IsNullOrEmpty(unionID) || unionID == "undefined")
                {
                    context.Response.Write("{\"code\":\"500\",\"msg\":\"参数不全\"}");
                    return;
                }
                string sql = "select * from asm_member where unionID='" + unionID + "'";
                Util.Debuglog("sql=" + sql, "修改手机号.txt");
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count <= 0)
                {
                    context.Response.Write("{\"code\":\"400\",\"msg\":\"会员信息读取失败\"}");
                    return;
                }
                else
                {
                    string sql1 = "select * from asm_member where companyID=" + companyID + " and phone='" + phone + "'";
                    Util.Debuglog("sql1=" + sql1, "修改手机号.txt");
                    DataTable d1 = DbHelperSQL.Query(sql1).Tables[0];
                    if (d1.Rows.Count > 0)
                    {
                        context.Response.Write("{\"code\":\"500\",\"msg\":\"当前手机号已存在\"}");
                        return;
                    }
                    if (phone.Length!=11)
                    {
                        context.Response.Write("{\"code\":\"500\",\"msg\":\"手机号不正确\"}");
                        return;
                    }
                    string update = "";
                    if (phone!="undefined")
                    {
                        if (d1.Rows.Count > 0 && d1.Rows[0]["dj"].ToString() == "0")
                        {
                            update = "update asm_member set phone='" + phone + "',dj=1 where minOpenID='" + minOpenID + "' ";
                        }
                        else
                        {
                            update = "update asm_member set phone='" + phone + "' where minOpenID='" + minOpenID + "' ";
                        }
                    }
                    Util.Debuglog("update=" + update, "修改手机号.txt");
                    int a = DbHelperSQL.ExecuteSql(update);
                    if (a > 0)
                    {
                        context.Response.Write("{\"code\":\"200\",\"msg\":\"会员信息修改成功\"}");
                        return;
                    }
                    else
                    {
                        context.Response.Write("{\"code\":\"500\",\"msg\":\"会员信息修改失败\"}");
                        return;
                    }
                }

            }
            catch
            {
                context.Response.Write("{\"code\":\"500\",\"msg\":\"系统异常\"}");
                return;
            }
        }
        public void getProductByOrderNO(HttpContext context)
        {
            try
            {
                string orderNO = context.Request["orderNO"].ToString();

                if (!string.IsNullOrEmpty(orderNO))
                {
                    string sql = "select o.*,p.httpImageUrl,p.proName,p.progg,(select mechineName from  asm_mechine m where m.id=o.mechineID)mechineName,(select addres from  asm_mechine m where m.id=o.mechineID)addres,(select count(*) from asm_orderlistDetail od where od.orderNO=o.orderNO and zt=5) tnum  from  asm_orderlist o left join asm_product p on o.productID=p.productID where o.orderNO='" + orderNO + "'";
                    Util.Debuglog("sql=" + sql, "getProductByOrderNO.txt");
                    DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        context.Response.Write("{\"code\":\"200\",\"db\":" + OperUtil.DataTableToJsonWithJsonNet(dt) + "}");
                        return;
                    }
                    context.Response.Write("{\"code\":\"500\",\"msg\":\"产品信息查询异常\"}");
                    return;
                }
                else
                {
                    context.Response.Write("{\"code\":\"500\",\"msg\":\"参数不全\"}");
                    return;
                }
            }
            catch
            {
                context.Response.Write("{\"code\":\"500\",\"msg\":\"系统异常\"}");
                return;
            }
        }
        /// <summary>
        /// 获取订单剩余天数 此处不根据asm_orderlist 表中字段syNum计算 因为 syNum 是在订购取货之后减1   根据订单明细表中zt=5 未派送的订单数量计算
        /// </summary>
        /// <param name="context"></param>
        public void getOrderSYNum(HttpContext context)
        {
            try
            {
                string orderNO = context.Request["orderNO"].ToString();
                string sql = "select count(*) num from  asm_orderlistDetail where orderNO='" + orderNO + "' and zt=5";
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                context.Response.Write("{\"code\":\"200\",\"num\":\"" + dt.Rows[0]["num"].ToString() + "\"}");
                return;

            }
            catch
            {
                context.Response.Write("{\"code\":\"500\",\"msg\":\"订单信息获取异常\"}");
                return;
            }
        }
        /// <summary>
        /// 产品兑换
        /// </summary>
        /// <param name="context"></param>
        public void dhProduct(HttpContext context)
        {
            try
            {
                string companyID = context.Request["companyID"].ToString();//14
                string mechineID = context.Request["mechineID"].ToString();//56
                string unionID = context.Request["unionID"];//owhCR0esai2hPXH4lYkeLMAcccuE
                string openID = context.Request["minOpenID"].ToString();//oJ1cB5bVOpZSma4Sy_wb0eMsmUqw
                string oldOrderNO = context.Request["oldOrderNO"].ToString(); //251566194618366
                string newOrderNO = OperUtil.getOrderNO(mechineID); ;
                string oldProductID = context.Request["oldProductID"].ToString();//424
                string newProductID = context.Request["newProductID"].ToString();//439
                string startDate = context.Request["startDate"].ToString();//首送日期2019-08-28
                string days = context.Request["days"].ToString();//剩余天数176
                string chaMoney = context.Request["chaMoney"].ToString();//差额 大于0证明需要会员微信支付  小于0证明需要退还给会员差额  668.80
                Util.Debuglog("companyID=" + companyID + ";mechineID=" + mechineID + ";unionID=" + unionID + ";openID=" + openID + ";oldOrderNO=" + oldOrderNO + ";oldProductID=" + oldProductID + ";newProductID=" + newProductID + ";startDate=" + startDate, "dhProduct.txt");
                if (string.IsNullOrEmpty(mechineID) || string.IsNullOrEmpty(unionID) || string.IsNullOrEmpty(openID) || string.IsNullOrEmpty(oldOrderNO) || string.IsNullOrEmpty(newOrderNO) || string.IsNullOrEmpty(oldProductID) || string.IsNullOrEmpty(newProductID) || string.IsNullOrEmpty(chaMoney))
                {
                    context.Response.Write("{\"code\":\"500\",\"msg\":\"参数不全\"}");
                    return;
                }
                if (double.Parse(chaMoney) > 0)
                {
                    //1 首先修改旧订单状态  只允许配送中的订单兑换
                    string sql1 = "select * from  asm_orderlist where orderNO='" + oldOrderNO + "'";
                    Util.Debuglog("sql1=" + sql1, "dhProduct.txt");
                    DataTable d1 = DbHelperSQL.Query(sql1).Tables[0];
                    if (d1.Rows.Count > 0)
                    {
                        if (d1.Rows[0]["orderZT"].ToString() == "1")
                        {
                            //此处计算剩余天数
                            string sql5 = "select count(*) num from  asm_orderlistDetail where orderNO='" + oldOrderNO + "' and zt=5";
                            Util.Debuglog("sql5=" + sql5, "dhProduct.txt");
                            DataTable d5 = DbHelperSQL.Query(sql5).Tables[0];
                            //2019-08-29调整，因为之前的方法会出现没付款但是就直接给更老订单状态的问题
                            //修改旧订单状态为4 兑换 修改订单明细剩余订单zt=7
                            //List<string> list = new List<string>();
                            //string sql2 = "update asm_orderlist set orderZT=4 where orderNO='" + oldOrderNO + "'";
                            //string sql3 = "update asm_orderlistDetail set zt=7 where mechineID=" + mechineID + " and zt=5 and orderNO='" + oldOrderNO + "'";
                            //list.Add(sql2);
                            //list.Add(sql3);
                            //int a = DbHelperSQL.ExecuteSqlTran(list);
                            if (int.Parse(d5.Rows[0]["num"].ToString()) > 0)
                            {
                                //创建新订单 
                                string psCycleID = d1.Rows[0]["activityID"].ToString();//参加活动的ID
                                                                                       //1 1日1送 2 2日1送 3 3日1送 4 周一至周五 5 周末送
                                string psMode = d1.Rows[0]["psMode"].ToString();
                                string sql4 = "select * from  asm_product where productID=" + newProductID;
                                DataTable d4 = DbHelperSQL.Query(sql4).Tables[0];
                                if (d4.Rows.Count > 0)
                                {
                                   
                                    double newprice = double.Parse(d4.Rows[0]["price0"].ToString());
                                    double oldprice = double.Parse(d1.Rows[0]["price"].ToString());//旧订单表里存的单价
                                    int num = int.Parse(d5.Rows[0]["num"].ToString());
                                    double totalMoney = newprice * num;
                                    string result = addOrder2(psCycleID, psMode, openID, newProductID, startDate, companyID, mechineID, totalMoney.ToString("f2"), days, newOrderNO);
                                    JObject jo = (JObject)JsonConvert.DeserializeObject(result);
                                    string code = jo["code"].ToString();
                                    if (code == "200")
                                    {
                                        //插入兑换记录
                                        string memberID = d1.Rows[0]["memberID"].ToString();
                                        string sql6 = "insert into [dbo].[asm_duihuan](memberID,oldOrderNO,newOrderNO,oldProductID,newProductID,bce,dhTime,bz)values('" + memberID + "','" + oldOrderNO + "','" + newOrderNO + "','" + oldProductID + "','" + newProductID + "','" + (newprice - oldprice) * num + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','当前剩余天数" + num + "')";
                                        Util.Debuglog("sql6=" + sql6, "dhProduct.txt");
                                        DbHelperSQL.ExecuteSql(sql6);
                                        context.Response.Write(result);
                                        return;
                                    }
                                    else
                                    {
                                        context.Response.Write(result);
                                        return;
                                    }
                                }
                                else
                                {
                                    context.Response.Write("{\"code\":\"500\",\"msg\":\"产品信息获取异常\"}");
                                    return;
                                }

                            }
                            else
                            {
                                context.Response.Write("{\"code\":\"500\",\"msg\":\"修改订单异常\"}");
                                return;
                            }

                        }
                        else
                        {
                            context.Response.Write("{\"code\":\"500\",\"msg\":\"该笔订单状态不允许兑换\"}");
                            return;
                        }


                    }
                }
                else
                {
                    //需要退差额的
                    //1 首先修改旧订单状态  只允许配送中的订单兑换
                    chaMoney = System.Math.Abs(double.Parse(chaMoney)) + "";
                    string sql1 = "select * from  asm_orderlist where orderNO='" + oldOrderNO + "'";
                    Util.Debuglog("sql1=" + sql1, "dhProduct.txt");
                    DataTable d1 = DbHelperSQL.Query(sql1).Tables[0];
                    if (d1.Rows.Count > 0)
                    {
                        if (d1.Rows[0]["orderZT"].ToString() == "1")
                        {
                            //此处计算剩余天数
                            string sql5 = "select count(*) num from  asm_orderlistDetail where orderNO='" + oldOrderNO + "' and zt=5";
                            Util.Debuglog("sql5=" + sql5, "dhProduct.txt");
                            DataTable d5 = DbHelperSQL.Query(sql5).Tables[0];
                            //修改旧订单状态为4 兑换 修改订单明细剩余订单zt=7
                            List<string> list = new List<string>();
                            string sql2 = "update asm_orderlist set orderZT=4 where orderNO='" + oldOrderNO + "'";
                            string sql3 = "update asm_orderlistDetail set zt=7 where mechineID=" + mechineID + " and zt=5 and orderNO='" + oldOrderNO + "'";
                            list.Add(sql2);
                            list.Add(sql3);
                            int a = DbHelperSQL.ExecuteSqlTran(list);
                            if (a > 0)
                            {
                                //创建新订单 
                                string psCycleID = d1.Rows[0]["activityID"].ToString();//参加活动的ID
                                                                                       //1 1日1送 2 2日1送 3 3日1送 4 周一至周五 5 周末送
                                string psMode = d1.Rows[0]["psMode"].ToString();
                                string sql4 = "select * from  asm_product where productID=" + newProductID;
                                Util.Debuglog("sql4="+sql4, "dhProduct.txt");
                                DataTable d4 = DbHelperSQL.Query(sql4).Tables[0];
                                if (d4.Rows.Count > 0)
                                {
                                    
                                    double newprice = double.Parse(d4.Rows[0]["price0"].ToString());
                                    double oldprice = double.Parse(d1.Rows[0]["price"].ToString());//旧订单表里存的单价
                                    int num = int.Parse(d5.Rows[0]["num"].ToString());
                                    double totalMoney = newprice * num;
                                    Util.Debuglog("totalMoney=" + totalMoney, "dhProduct.txt");
                                    string result = addOrder3(psCycleID, psMode, openID, newProductID, startDate, companyID, mechineID, totalMoney.ToString("f2"), days, chaMoney, newOrderNO);
                                    Util.Debuglog("result=" + result, "dhProduct.txt");
                                    JObject jo = (JObject)JsonConvert.DeserializeObject(result);
                                    string code = jo["code"].ToString();
                                    if (code == "200")
                                    {
                                        //插入兑换记录
                                        string memberID = d1.Rows[0]["memberID"].ToString();
                                        string sql6 = "insert into [dbo].[asm_duihuan](memberID,oldOrderNO,newOrderNO,oldProductID,newProductID,bce,dhTime,bz)values('" + memberID + "','" + oldOrderNO + "','" + newOrderNO + "','" + oldProductID + "','" + newProductID + "','" + (newprice - oldprice) * num + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','当前剩余天数" + num + "')";
                                        Util.Debuglog("sql6=" + sql6, "dhProduct.txt");
                                        DbHelperSQL.ExecuteSql(sql6);
                                        context.Response.Write(result);
                                        return;
                                    }
                                    else
                                    {
                                        context.Response.Write(result);
                                        return;
                                    }
                                }
                                else
                                {
                                    context.Response.Write("{\"code\":\"500\",\"msg\":\"产品信息获取异常\"}");
                                    return;
                                }
                            }
                            else
                            {
                                context.Response.Write("{\"code\":\"500\",\"msg\":\"修改订单异常\"}");
                                return;
                            }
                        }
                        else
                        {
                            context.Response.Write("{\"code\":\"500\",\"msg\":\"该笔订单状态不允许兑换\"}");
                            return;
                        }
                        
                    }
                }

            }
            catch(Exception e)
            {
                Util.Debuglog("err=" + e, "dhProduct.txt");
                context.Response.Write("{\"code\":\"500\",\"msg\":\"系统异常\"}");
                return;
            }
        }
        public void getDJList(HttpContext context)
        {
            try
            {
                string companyID = context.Request["companyID"].ToString();
                string unionID = context.Request["unionID"];
                string openID = context.Request["minOpenID"].ToString();
                Util.Debuglog("companyID=" + companyID + ";unionID=" + unionID + ";openID=" + openID, "getDJList.txt");
                if (string.IsNullOrEmpty(companyID) || string.IsNullOrEmpty(unionID) || string.IsNullOrEmpty(openID))
                {
                    context.Response.Write("{\"code\":\"500\",\"msg\":\"参数不全\"}");
                    return;
                }
                //查询当前消费天数
                string sql1 = "select COUNT(*) num,left(CONVERT(varchar,convert(datetime,orderTime),20),11) from asm_sellDetail where DATEDIFF(DD, orderTime, GETDATE())<= 30 and memberID in(select id from  asm_member where minOpenID='" + openID + "') and type=2 group by left(CONVERT(varchar,convert(datetime,orderTime),20),11)";
                DataTable d1 = DbHelperSQL.Query(sql1).Tables[0];
                string sql = "select * from asm_dj where companyID=" + companyID;
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count >= 3)
                {
                    context.Response.Write("{\"code\":\"200\",\"db\":" + OperUtil.DataTableToJsonWithJsonNet(dt) + ",\"consumeDay\":\"" + d1.Rows.Count + "\"}");
                    return;
                }
                else
                {
                    context.Response.Write("{\"code\":\"300\",\"msg\":\"暂无记录\"}");
                    return;
                }
            }
            catch
            {
                context.Response.Write("{\"code\":\"500\",\"msg\":\"系统异常\"}");
                return;
            }
        }
        public void getNoticeListCount(HttpContext context)
        {
            try
            {
                string unionID = context.Request["unionID"].ToString();
                string minOpenID = context.Request["minOpenID"].ToString();
                string sqlUpdate = "select * from asm_member where unionID='"+unionID+ "' and unionID is not null and unionID!=''";
                DataTable dm = DbHelperSQL.Query(sqlUpdate).Tables[0];
                if (dm.Rows.Count>0&&string.IsNullOrEmpty(dm.Rows[0]["minOpenID"].ToString()))
                {
                    string update = "update asm_member set minOpenID='"+minOpenID+ "' where id="+dm.Rows[0]["id"].ToString();
                    DbHelperSQL.ExecuteSql(update);
                }
          
                string sql = "select count(*) num from [dbo].[asm_notice] where isRead=0 and memberID in (select id from asm_member where ( minOpenID='" + minOpenID + "')) ";
                Util.Debuglog("sql=" + sql, "getNoticeList.txt");
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0&& !string.IsNullOrEmpty(minOpenID))
                {
                    context.Response.Write("{\"code\":\"200\",\"num\":\"" + dt.Rows[0]["num"].ToString() + "\"}");
                    return;
                }
                else
                {
                    context.Response.Write("{\"code\":\"300\",\"msg\":\"暂无记录\"}");
                    return;
                }
            }
            catch
            {
                context.Response.Write("{\"code\":\"500\",\"msg\":\"系统异常\"}");
                return;
            }
        }
        public void getNotice(HttpContext context)
        {
            try
            {
                string id = context.Request["id"].ToString();
                string sql = "select *,(select addres from asm_mechine where id=mechineID) as address from asm_notice where id=" + id;
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    updateNotice(id);
                    context.Response.Write("{\"code\":\"200\",\"db\":" + OperUtil.DataTableToJsonWithJsonNet(dt) + "}");
                    return;
                }
                context.Response.Write("{\"code\":\"500\",\"msg\":\"系统异常\"}");
                return;
            }
            catch
            {
                context.Response.Write("{\"code\":\"500\",\"msg\":\"系统异常\"}");
                return;
            }
        }
        public void getNoticeList(HttpContext context)
        {
            try
            {
                string unionID = context.Request["unionID"].ToString();
                string minOpenID = context.Request["minOpenID"].ToString();
                string mechineID = context.Request["mechineID"].ToString();
                string sql = "select top 30  *,right(LEFT(time,16),11) timeStr,(select addres from asm_mechine where id=mechineID) as addres from [dbo].[asm_notice] where memberID in (select id from asm_member where (minOpenID='" + minOpenID + "')) order by id desc";
                Util.Debuglog("sql=" + sql, "getNoticeList.txt");
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    context.Response.Write("{\"code\":\"200\",\"db\":" + OperUtil.DataTableToJsonWithJsonNet(dt) + "}");
                    return;
                }
                else
                {

                }
            }
            catch
            {
                context.Response.Write("{\"code\":\"500\",\"msg\":\"系统异常\"}");
                return;
            }
        }
        public void updateNotice(string id)
        {
            try
            {

                string sql = "update asm_notice set isRead=1 where id=" + id;
                int a = DbHelperSQL.ExecuteSql(sql);
                if (a > 0)
                {

                }

            }
            catch
            {

            }
        }
        public void ch(HttpContext context)
        {
            try
            {
                string unionID = context.Request["unionID"].ToString();
                string minOpenID = context.Request["minOpenID"].ToString();
                string mechineID = context.Request["mechineID"].ToString();
                try
                {
                    int.Parse(mechineID);
                }
                catch {
                    context.Response.Write("{\"code\":\"500\",\"msg\":\"请使用微信小程序的摄像头扫码支付\"}");
                    return;
                }
                if (mechineID== "68" || mechineID == "69") {
                    string _mechineInfo = RedisUtil.getMechine(mechineID);
                    JArray _mechineJArray = RedisUtil.DeserializeObject(_mechineInfo);
                    if (_mechineJArray != null)
                    {
                        if (mechineID != "25")
                        {

                            if (_mechineJArray[0]["openStatus"].ToString() == "1")
                            {
                                context.Response.Write("{\"code\":\"500\",\"msg\":\"当前机器失去连接，请稍后重试\"}");
                                return;
                            }
                        }

                        if (_mechineJArray[0]["netStatus"].ToString() == "1" || _mechineJArray[0]["gkjStatus"].ToString() == "1")
                        {
                            context.Response.Write("{\"code\":\"500\",\"msg\":\"当前机器失去连接，请稍后重试\"}");
                            return;
                        }
                        if (_mechineJArray[0]["updateSoftStatus"].ToString() != "0")
                        {
                            context.Response.Write("{\"code\":\"500\",\"msg\":\"当前机器失去连接，请稍后重试\"}");
                            return;
                        }
                    }
                }
                else if (Util.chStatus("0", mechineID) != "200"){
                    context.Response.Write("{\"code\":\"500\",\"msg\":\"当前机器失去连接，请稍后重试\"}");
                    return;
                }
                //查询该会员该机器当天是否有需要出的货
                string sql = "SELECT * from  asm_orderlistDetail "
                            + " where mechineID = " + mechineID + " "
                            + " and memberID in (SELECT id from asm_member where minOpenID = '" + minOpenID + "') "
                            + " AND createTime = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'"
                            + " AND zt = 4 and statu<>1";
                Util.Debuglog("sql=" + sql, "订购出货料道.txt");
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {

                    Util.Debuglog("ldno=" + Util.getDGLDNO(mechineID, dt.Rows[0]["productID"].ToString()), "订购出货料道.txt");
                    string ldno = Util.getDGLDNO(mechineID, dt.Rows[0]["productID"].ToString());
                    if (string.IsNullOrEmpty(ldno))
                    {
                        context.Response.Write("{\"code\":\"500\",\"msg\":\"当前产品没有上货\"}");
                        return;
                    }
                    string sqlOrder = "SELECT * FROM asm_orderlist where orderNO='"+dt.Rows[0]["orderNO"]+"'";
                    DataTable dtOrder = DbHelperSQL.Query(sqlOrder).Tables[0];

                    string sql4 = "update  asm_orderlistDetail set statu=1,ldNO=" + ldno + " ,sellTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where id='" + dt.Rows[0]["id"].ToString() + "'";
                    Util.Debuglog("3code:" + dt.Rows[0]["code"].ToString() + "mechineID:" + mechineID, "订购出货.txt");
                    if (DbHelperSQL.ExecuteSql(sql4) > 0)
                    {

                        if (mechineID == "68" || mechineID == "69")
                        {
                            string searchSql = "select 1 from asm_firstPayRecord where mechineID=" + mechineID + " AND memberID='" + dt.Rows[0]["memberID"].ToString() + "'";
                            DataTable searchSqldt = DbHelperSQL.Query(searchSql).Tables[0];
                            if (searchSqldt.Rows.Count > 0)
                            {

                            }
                            else
                            {
                                string insertsql = "insert into   asm_firstPayRecord(mechineID,memberID,firstinfo,firstbuyTime,type) values (" + mechineID + "," + dt.Rows[0]["memberID"].ToString() + ",'" + dt.Rows[0]["code"].ToString() + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + 1 + "') ";
                                Util.Debuglog("sqlInsert=" + insertsql, "获取预生成订单号.txt");
                                DbHelperSQL.ExecuteSql(insertsql);
                            }
                            Util.dgchNew(ldno, mechineID, dt.Rows[0]["memberID"].ToString(), dt.Rows[0]["productID"].ToString(), dt.Rows[0]["code"].ToString(), dtOrder.Rows[0]["price"].ToString());
                        }
                        else
                        {
                            Util.dgch(ldno, mechineID, dt.Rows[0]["memberID"].ToString(), dt.Rows[0]["productID"].ToString(), dt.Rows[0]["code"].ToString(), dtOrder.Rows[0]["price"].ToString());
                        }
                      
                        context.Response.Write("{\"code\":\"200\",\"msg\":\"等待出货\"}");
                        return;

                    }
                    
                }
                else
                {
                    context.Response.Write("{\"code\":\"500\",\"msg\":\"当前机器今天没有需要取的货，请重新定位机器\"}");
                    return;
                }

            }
            catch
            {
                context.Response.Write("{\"code\":\"500\",\"msg\":\"系统异常\"}");
                return;
            }
        }
        public void sendMessage(HttpContext context)
        {
            try
            {
                string unionID = context.Request["unionID"].ToString();
                string openID = context.Request["openID"].ToString();
                string phone = context.Request["phone"].ToString();
                string sql = "select * from asm_member where minOpenID='" + openID + "'";
                Util.Debuglog("unionID=" + unionID + ";openID=" + openID + ";phone=" + phone, "sendMessage.txt");
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count <= 0)
                {
                    context.Response.Write("{\"code\":\"400\",\"msg\":\"会员信息读取失败\"}");
                    return;
                }
                if (dt.Rows[0]["phone"].ToString() == "")
                {
                    context.Response.Write("{\"code\":\"500\",\"msg\":\"请先暂未绑定手机号\"}");
                    return;
                }
                Random rd = new Random();
                string yzm = rd.Next(100000, 999999).ToString();
                sendSMS(phone, yzm);
                context.Response.Write("{\"code\":\"200\",\"msg\":\"" + yzm + "\"}");

            }
            catch
            {
                context.Response.Write("{\"code\":\"500\",\"msg\":\"参数不全\"}");
                return;
            }
        }
        public void updatePwd(HttpContext context)
        {
            try
            {
                string minOpenID = context.Request["minOpenID"].ToString();
                string unionID = context.Request["unionID"].ToString();
                string pwd = context.Request["pwd"].ToString();
                string yzm = context.Request["yzm"].ToString();
                string sql = "select * from asm_member where unionID='" + unionID + "'";
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count <= 0)
                {
                    context.Response.Write("{\"code\":\"400\",\"msg\":\"会员信息读取失败\"}");
                    return;
                }
                string _yzm = RedisHelper.GetRedisModel<string>(dt.Rows[0]["phone"].ToString());
                if (string.IsNullOrEmpty(_yzm))
                {
                    context.Response.Write("{\"code\":\"500\",\"msg\":\"请获取验证码\"}");
                    return;
                }
                if (_yzm != yzm)
                {
                    context.Response.Write("{\"code\":\"500\",\"msg\":\"验证码不正确\"}");
                    return;
                }
                string update = "update asm_member set pwd='" + pwd + "' where minOpenID='" + minOpenID + "' ";
                int a = DbHelperSQL.ExecuteSql(update);
                if (a > 0)
                {
                    context.Response.Write("{\"code\":\"200\",\"msg\":\"会员信息修改成功\"}");
                }
                else
                {
                    context.Response.Write("{\"code\":\"200\",\"msg\":\"会员信息修改失败\"}");
                }
            }
            catch
            {
                context.Response.Write("{\"code\":\"500\",\"msg\":\"会员信息修改异常\"}");
            }
        }
        public void sellProduct(HttpContext context)
        {
            try
            {
                string orderNO = context.Request["orderNO"].ToString();
                string time = context.Request["time"].ToString();
                string mechineID = context.Request["mechineID"].ToString();
                string sql = "select * from asm_orderlistDetail where orderNO='" + orderNO + "' and mechineID=" + mechineID + " and createTime='" + delTime(time) + "'";
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["zt"].ToString() != "4")
                    {
                        context.Response.Write("{\"code\":\"500\",\"msg\":\"订单状态不允许转售\"}");
                        return;
                    }
                    string sql1 = "select * from asm_orderlist where orderNO='" + orderNO + "' and mechineID=" + mechineID;
                    DataTable dt1 = DbHelperSQL.Query(sql1).Tables[0];

                    string updateSql = "update asm_orderlistDetail set bz='于" + DateTime.Now.ToString("HH:mm") + "转售',sellPrice=" + double.Parse(dt1.Rows[0]["price"].ToString()) / 2 + ",zt=3 where orderNO='" + orderNO + "' and mechineID=" + mechineID + " and createTime='" + delTime(time) + "'";
                    int a = DbHelperSQL.ExecuteSql(updateSql);
                    if (a > 0)
                    {
                        Util.ClearRedisProductInfoByMechineID(mechineID);
                        RedisHelper.Remove(mechineID + "_SellOrderInfo");
                        context.Response.Write("{\"code\":\"200\",\"msg\":\"转售成功，等待买家购买\"}");
                        return;
                    }
                    else
                    {
                        context.Response.Write("{\"code\":\"500\",\"msg\":\"订单查询异常\"}");
                        return;
                    }
                }
                else
                {
                    context.Response.Write("{\"code\":\"500\",\"msg\":\"订单状态异常\"}");
                    return;
                }
            }
            catch
            {
                context.Response.Write("{\"code\":\"500\",\"msg\":\"系统异常\"}");
                return;
            }
        }
        public void setDelay(HttpContext context)
        {
            try
            {
                string orderNO = context.Request["orderNO"].ToString();
                if (string.IsNullOrEmpty(orderNO))
                {
                    context.Response.Write("{\"code\":\"500\",\"msg\":\"参数不全\"}");
                    return;
                }
                string sql = "select * from asm_orderlist where orderNO='" + orderNO + "'";
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["orderZT"].ToString() == "1" || dt.Rows[0]["orderZT"].ToString() == "0")
                    {
                        Util.Debuglog("暂停订单号=" + orderNO, "订单暂停.txt");
                        //设置为暂停
                        //把日期以后的订单zt=5的 设置为8
                        string update = "update asm_orderlist set orderZT=6 where id=" + dt.Rows[0]["id"].ToString();
                        int a = DbHelperSQL.ExecuteSql(update);
                        if (a > 0)
                        {
                            string update2 = "update asm_orderlistDetail set zt=8 where orderNO='" + orderNO + "' and zt=5 and  DATEDIFF(dd,GETDATE(), createTime )>=2 ";
                            int a2 = DbHelperSQL.ExecuteSql(update2);
                            Util.Debuglog("暂停数量=" + a2, "订单暂停.txt");
                            Util.ClearRedisProductInfoByMechineID(dt.Rows[0]["mechineID"].ToString());
                            //  Util.orderDelay();
                            context.Response.Write("{\"code\":\"200\",\"msg\":\"设置成功\"}");
                            return;
                        }
                    }
                    else if (dt.Rows[0]["orderZT"].ToString() == "6")
                    {
                        Util.Debuglog("恢复订单号=" + orderNO, "订单暂停.txt");
                        Util.SleepOrder(dt);

                        context.Response.Write("{\"code\":\"200\",\"msg\":\"设置成功\"}");
                        return;

                    }
                    context.Response.Write("{\"code\":\"500\",\"msg\":\"订单状态不允许暂停\"}");
                    return;
                 
                }
                else
                {
                    context.Response.Write("{\"code\":\"500\",\"msg\":\"订单查询异常\"}");
                    return;
                }
            }
            catch
            {
                context.Response.Write("{\"code\":\"500\",\"msg\":\"系统异常\"}");
                return;
            }
        }
        public void chgDateTime(HttpContext context)
        {
            try
            {
                string orderNO = context.Request["orderNO"].ToString();
                string olddateTime = context.Request["olddateTime"].ToString();//原时间
                string newTime = context.Request["newTime"].ToString();//新时间
                string mechineID = context.Request["mechineID"].ToString();
                string sql = "select * from asm_orderlistDetail where orderNO='" + orderNO + "' and mechineID=" + mechineID + " and createTime='" + delTime(olddateTime) + "'";
                Util.Debuglog(sql, "调整派送时间.txt");
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["zt"].ToString() != "5")
                    {
                        context.Response.Write("{\"code\":\"500\",\"msg\":\"状态不允许调整配送时间\"}");
                        return;
                    }
                    if (Convert.ToDateTime(newTime) < DateTime.Now)
                    {
                        context.Response.Write("{\"code\":\"500\",\"msg\":\"调整配送时间不能小于当前时间\"}");
                        return;
                    }
                    string sql1 = "select * from asm_orderlistDetail where orderNO='" + orderNO + "' and mechineID=" + mechineID + " and createTime='" + delTime(olddateTime) + "'";
                    Util.Debuglog(sql1, "调整派送时间.txt");
                    DataTable dt1 = DbHelperSQL.Query(sql1).Tables[0];
                    if (dt1.Rows.Count > 0)
                    {
                        string updateSql = "update asm_orderlistDetail set bz='派送时间由" + olddateTime + "更改为" + newTime + "',sellTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',zt=5,createTime='" + delTime(newTime) + "' where id=" + dt1.Rows[0]["id"].ToString();
                        Util.Debuglog(updateSql, "调整派送时间.txt");
                        string sql2 = "select * from asm_orderlistDetail where orderNO='"+ orderNO + "' order by createTime desc";
                        DataTable d2 = DbHelperSQL.Query(sql2).Tables[0];
                        string update1 = "update asm_orderlist set endTime='"+d2.Rows[0]["createTime"].ToString()+"' where orderNO='"+ orderNO + "'";
                        DbHelperSQL.ExecuteSql(update1);
                        int a = DbHelperSQL.ExecuteSql(updateSql);
                        if (a > 0)
                        {
                            context.Response.Write("{\"code\":\"200\",\"msg\":\"派送时间修改成功\"}");
                            return;
                        }
                        else
                        {
                            context.Response.Write("{\"code\":\"500\",\"msg\":\"订单查询异常\"}");
                            return;
                        }
                    }
                    else
                    {
                        context.Response.Write("{\"code\":\"500\",\"msg\":\"没有需要修改派送时间的订单\"}");
                        return;
                    }

                }
                else
                {
                    context.Response.Write("{\"code\":\"500\",\"msg\":\"该笔订单查询异常\"}");
                    return;
                }
            }
            catch
            {
                context.Response.Write("{\"code\":\"500\",\"msg\":\"系统异常\"}");
                return;
            }
        }

        public void getOrderDetail(HttpContext context)
        {
            try
            {
                string orderNO = context.Request["orderNO"].ToString();
                string companyID = context.Request["companyID"].ToString();
                string mechineID = context.Request["mechineID"].ToString();
                string minOpenID = context.Request["minOpenID"].ToString();
                string unionID = context.Request["unionID"].ToString();
                string dateTime = context.Request["dateTime"].ToString();
                string sql = "select *,right(createTime,2) dates from asm_orderlistDetail where orderNO='" + orderNO + "' and companyID=" + companyID + " and mechineID='" + mechineID + "' and createTime like '%" + delMonth(dateTime) + "%' order by id ";
                Util.Debuglog(sql, "订单明细.txt");
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    context.Response.Write("{\"code\":\"200\",\"db\":" + Util.DataTableToJsonWithJsonNet(dt) + "}");
                    return;
                }
                else
                {
                    context.Response.Write("{\"code\":\"500\",\"msg\":\"无此订单信息\"}");
                    return;
                }
            }
            catch
            {
                context.Response.Write("{\"code\":\"500\",\"msg\":\"系统异常\"}");
                return;
            }
        }
        //处理时间
        public string delMonth(string time)
        {
            string[] timeArr = time.Split('-');
            if (timeArr.Length == 2)
            {
                timeArr[1] = timeArr[1].PadLeft(2, '0');
                return timeArr[0] + "-" + timeArr[1];
            }
            else
            {
                return time;
            }

        }
        /// <summary>
        /// 获取订单信息 根据orderID
        /// </summary>
        /// <param name="context"></param>
        public void getOrderInfo(HttpContext context)
        {
            try
            {
                string orderID = context.Request["orderID"].ToString();
                string sql = "select o.*,p.httpImageUrl,p.progg,p.proName,p.startSend,(select customerPhone from asm_company where id=o.companyID)customerPhone from asm_orderlist o left join asm_product p  on o.productID=p.productID where id='"+ orderID + "'";
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    context.Response.Write("{\"code\":\"200\",\"db\":" + Util.DataTableToJsonWithJsonNet(dt) + "}");
                    return;
                }
                else
                {
                    context.Response.Write("{\"code\":\"500\",\"msg\":\"无此订单信息\"}");
                    return;
                }
            }
            catch
            {
                context.Response.Write("{\"code\":\"500\",\"msg\":\"系统异常\"}");
                return;
            }
        }
        public void getCode(HttpContext context)
        {
            try
            {
                //此处不限制机器展示会员的所有机器的取货码  等到取货的时候去验证提示
                string mechineID = context.Request["mechineID"].ToString();
                string minOpenID = context.Request["minOpenID"].ToString();
                string unionID = context.Request["unionID"].ToString();
                string sql = "select *,(SELECT proName from  asm_product p where p.productID=o.productID) proName,(select addres from  asm_mechine m where m.id=mechineID) addres,(SELECT  httpImageUrl from  asm_product p where p.productID=o.productID) httpImageUrl from asm_orderlistDetail o  where memberID in(select id from asm_member where minOpenID='" + minOpenID + "' or unionID='" + unionID + "') and zt=4 and createTime='" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
                Util.Debuglog("sql=" + sql, "获取取货码.txt");
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    context.Response.Write("{\"code\":\"200\",\"db\":" + Util.DataTableToJsonWithJsonNet(dt) + "}");
                }
                else
                {
                    context.Response.Write("{\"code\":\"300\",\"msg\":\"今日没有要取的商品\"}");
                    return;
                }
            }
            catch
            {
                context.Response.Write("{\"code\":\"500\",\"msg\":\"系统异常\"}");
                return;
            }
        }
        public void transferAccounts(HttpContext context)
        {
            try
            {
                string minOpenID = context.Request["minOpenID"].ToString();
                string unionID = context.Request["unionID"].ToString();
                string phone = context.Request["phone"].ToString();
                string companyID = context.Request["companyID"].ToString();
                string money = double.Parse(context.Request["money"].ToString()).ToString("f2");
                string bz = context.Request["bz"].ToString();
                if (string.IsNullOrEmpty(minOpenID) || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(companyID) || string.IsNullOrEmpty(money))
                {
                    context.Response.Write("{\"code\":\"500\",\"msg\":\"参数不全 请退出重试\"}");
                    return;
                }
                string sql1 = "select * from asm_member where minOpenID='" + minOpenID + "' ";
                DataTable d1 = DbHelperSQL.Query(sql1).Tables[0];
                string sql2 = "select * from asm_member where  phone='" + phone + "'";
                DataTable d2 = DbHelperSQL.Query(sql2).Tables[0];
                if (d2.Rows.Count <= 0)
                {
                    context.Response.Write("{\"code\":\"500\",\"msg\":\"收款人信息不存在\"}");
                    return;
                }
                string sql3 = "select * from asm_member where companyID=" + companyID + " and phone='" + phone + "'";
                DataTable d3 = DbHelperSQL.Query(sql3).Tables[0];
                if (d3.Rows.Count <= 0)
                {
                    context.Response.Write("{\"code\":\"500\",\"msg\":\"该收款人与您不在同一奶企不允许转账\"}");
                    return;
                }
                if (d1.Rows.Count <= 0)
                {
                    context.Response.Write("{\"code\":\"500\",\"msg\":\"会员信息查询异常\"}");
                    return;
                }
                if (string.IsNullOrEmpty(d1.Rows[0]["phone"].ToString()))
                {
                    context.Response.Write("{\"code\":\"500\",\"msg\":\"当前账号需要先绑定手机号\"}");
                    return;
                }
                if (double.Parse(d1.Rows[0]["AvailableMoney"].ToString()) < double.Parse(money))
                {
                    context.Response.Write("{\"code\":\"500\",\"msg\":\"余额不足\"}");
                    return;
                }
                //修改两个账号的余额
                string sqlUp1 = "update asm_member set AvailableMoney=AvailableMoney-" + money + " where id=" + d1.Rows[0]["id"].ToString();
                string sqlUp2 = "update asm_member set AvailableMoney=AvailableMoney+" + money + " where id=" + d3.Rows[0]["id"].ToString();
                List<String> list = new List<String>();
                list.Add(sqlUp1);
                list.Add(sqlUp2);
                int a = DbHelperSQL.ExecuteSqlTran(list);
                if (a > 0)
                {
                    //插入资金变动记录
                    Util.chgMoney(d1.Rows[0]["id"].ToString(), money, "转账支出", "转账给" + phone + ";金额：" + money, "4");
                    if (string.IsNullOrEmpty(bz))
                    {
                        Util.chgMoney(d3.Rows[0]["id"].ToString(), money, "转账收入", "获得" + d1.Rows[0]["phone"].ToString() + "转账;金额：" + money, "3");
                    }
                    else {
                        Util.chgMoney(d3.Rows[0]["id"].ToString(), money, "转账收入", bz, "3");
                    }
                    Util.Debuglog("openID=" + d3.Rows[0]["openID"].ToString(), "转账模板.txt");
                    if (!string.IsNullOrEmpty(d3.Rows[0]["openID"].ToString()))
                    {
                        try
                        { 
                            wxHelper wx = new wxHelper(companyID);
                            Util.Debuglog("模板ID=" + OperUtil.getMessageID(companyID, "OPENTM206956529"), "转账模板.txt");
                            string data = TemplateMessage.getTransf(d3.Rows[0]["openID"].ToString(),
                            OperUtil.getMessageID(companyID, "OPENTM206956529"), "您有一笔积分到账，详情如下：", money, string.IsNullOrEmpty(bz)?"会员转账": bz, "来自会员" + d1.Rows[0]["phone"].ToString());
                            Util.Debuglog("data=" + data, "转账模板.txt");
                            TemplateMessage.SendTemplateMsg(wx.IsExistAccess_Token(companyID), data);
                        }
                        catch (Exception e) {
                            Util.Debuglog("e="+e.Message,"转账模板.txt");
                        }
                    }
                   
                }
                context.Response.Write("{\"code\":\"200\",\"msg\":\"转账成功\"}");
            }
            catch
            {
                context.Response.Write("{\"code\":\"500\",\"msg\":\"转账异常\"}");
            }
        }
        public void updateMemberName(HttpContext context)
        {
            try
            {
                string minOpenID = context.Request["minOpenID"].ToString();
                string unionID = context.Request["unionID"].ToString();
                string name = context.Request["name"].ToString();
                if (string.IsNullOrEmpty(minOpenID) || string.IsNullOrEmpty(unionID) || unionID == "undefined")
                {
                    context.Response.Write("{\"code\":\"500\",\"msg\":\"参数不全\"}");
                    return;
                }
                string sql = "select * from asm_member where unionID='" + unionID + "' ";
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count <= 0)
                {
                    context.Response.Write("{\"code\":\"400\",\"msg\":\"会员信息读取失败\"}");
                }
                else
                {
                    string update = "update asm_member set name='" + name + "' where minOpenID='" + minOpenID + "' ";
                    int a = DbHelperSQL.ExecuteSql(update);
                    if (a > 0)
                    {
                        context.Response.Write("{\"code\":\"200\",\"msg\":\"会员信息修改成功\"}");
                    }
                    else
                    {
                        context.Response.Write("{\"code\":\"200\",\"msg\":\"会员信息修改失败\"}");
                    }
                }
            }
            catch
            {
                context.Response.Write("{\"code\":\"500\",\"msg\":\"会员信息修改异常\"}");
            }
        }
        public void updateMemberBirthday(HttpContext context)
        {
            try
            {
                string minOpenID = context.Request["minOpenID"].ToString();
                string unionID = context.Request["unionID"].ToString();
                string birthday = context.Request["birthday"].ToString();
                string sql = "select * from asm_member where minOpenID='" + minOpenID + "'";
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count <= 0)
                {
                    context.Response.Write("{\"code\":\"400\",\"msg\":\"会员信息读取失败\"}");
                }
                else
                {
                    string update = "update asm_member set birthday='" + birthday + "' where minOpenID='" + minOpenID + "'";
                    int a = DbHelperSQL.ExecuteSql(update);
                    if (a > 0)
                    {
                        context.Response.Write("{\"code\":\"200\",\"msg\":\"会员信息修改成功\"}");
                    }
                    else
                    {
                        context.Response.Write("{\"code\":\"200\",\"msg\":\"会员信息修改失败\"}");
                    }
                }
            }
            catch
            {
                context.Response.Write("{\"code\":\"500\",\"msg\":\"会员信息修改异常\"}");
            }

        }
        public void getMoneyChange(HttpContext context)
        {
            try
            {
                string openID = context.Request["minOpenID"].ToString();
                string unionID = context.Request["unionID"].ToString();
                string time = context.Request["time"].ToString();
                //
                string sql = "select * from asm_chgMoney where memberID in (select id from asm_member where unionID='" + unionID + "' and unionID!='' and unionID is not null) and LEFT(payTime,7)='" + time + "' order by id desc";
                Util.Debuglog(sql, "余额变动明细.txt");
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    context.Response.Write("{\"code\":\"200\",\"db\":" + Util.DataTableToJsonWithJsonNet(dt) + "}");
                }
                else
                {
                    context.Response.Write("{\"code\":\"300\",\"msg\":\"暂无明细记录\"}");
                }
            }
            catch
            {
                context.Response.Write("{\"code\":\"500\",\"msg\":\"交易记录查询异常\"}");
            }
        }
        /// <summary>
        /// 会员充值
        /// </summary>
        /// <param name="context"></param>
        public void payCZ(HttpContext context)
        {
            string openid = context.Request["openID"].ToString();
            string unionID = context.Request["unionID"].ToString();
            string companyID = context.Request["companyID"].ToString();
            string money = context.Request["money"].ToString();
            string dzMoney = context.Request["dzMoney"].ToString();
            string id = context.Request["id"].ToString();//充值活动的ID
            Util.Debuglog("openid=" + openid + ";companyID=" + companyID + ";money=" + money + ";dzMoney=" + dzMoney + ";unionID=" + unionID, "_充值日志2.txt");


            int fen = 0;
            string json = "";
            try
            {
                if (string.IsNullOrEmpty(openid))
                {
                    context.Response.Write("{\"code\":\"500\",\"msg\":\"参数不全,请退出重试\"}");
                    return;
                }
                DataTable dtM = DbHelperSQL.Query("select * from asm_member where unionID='" + unionID + "'and unionID is not null and unionID!=''").Tables[0];

                if (dtM.Rows.Count <= 0)
                {
                    context.Response.Write("{\"code\":\"400\",\"msg\":\"会员信息查询失败\"}");
                    return;
                }
                if (string.IsNullOrEmpty(dtM.Rows[0]["minOpenID"].ToString())&&!string.IsNullOrEmpty(openid))
                {
                    string update = "update asm_member set minOpenID='"+openid+ "' where unionID='"+unionID+"'";
                    DbHelperSQL.ExecuteSql(update);
                }
                if (unionID == "undefined")
                {
                    unionID = dtM.Rows[0]["unionID"].ToString();
                }

                fen = int.Parse((double.Parse(money) * 100).ToString());
                //fen = 1;
                //获取收货地址js函数入口参数
                string url = "https://wx.bingoseller.com/pay/czNotify.aspx";
                SybWxPayService sybService = new SybWxPayService(companyID);
                Dictionary<String, String> rsp = sybService.payW06(fen, DateTime.Now.ToFileTime().ToString(), "W06", "会员充值", "会员充值", openid, "", url, "", Util.getMinAppid(companyID));
                json = (new JavaScriptSerializer()).Serialize(rsp);
                Util.Debuglog("json=" + json, "_充值日志2.txt");

                JObject jo = (JObject)JsonConvert.DeserializeObject(json);
                if (jo["retcode"].ToString() == "SUCCESS")
                {
                    string appid = jo["appid"].ToString();
                    string cusid = jo["cusid"].ToString();
                    string trxid = jo["trxid"].ToString();
                    string reqsn = jo["reqsn"].ToString();
                    string insertSQL = @"insert into asm_pay_info(appid,cusid,trxid,acct,statu,reqsn,[type],payType,trxamt,dzMoney,activityID,unionID,companyID) values('" + appid + "','" + cusid + "','" + trxid + "','" + openid + "','0','" + reqsn + "',1,3," + fen + "," + dzMoney + ",'" + id + "','" + unionID + "','" + companyID + "')";
                    Util.Debuglog("insertSQL=" + insertSQL, "_充值日志2.txt");
                    DbHelperSQL.ExecuteSql(insertSQL);

                    Util.Debuglog("payinfo=" + jo["payinfo"].ToString(), "_充值日志2.txt");

                    context.Response.Write("{\"code\":\"200\",\"payInfo\":" + jo["payinfo"].ToString() + "}");
                }
            }
            catch (Exception ex)
            {
                context.Response.Write("{\"code\":\"500\",\"msg\":\"微信支付发起失败\"}");
                return;
            }
        }
        /// <summary>
        /// 获取充值活动
        /// </summary>
        /// <param name="context"></param>
        public void getPayActivityList(HttpContext context)
        {
            try
            {
                string companyID = context.Request["companyID"].ToString();
                string sql = "select * from asm_pay_activity where companyID=" + companyID + " and status=1";
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    context.Response.Write("{\"code\":\"200\",\"db\":" + Util.DataTableToJsonWithJsonNet(dt) + "}");
                }
                else
                {
                    context.Response.Write("{\"code\":\"300\",\"msg\":\"系统暂未设置充值活动\"}");
                }
            }
            catch
            {
                context.Response.Write("{\"code\":\"500\",\"msg\":\"获取充值活动异常\"}");
            }
        }
        /// <summary>
        /// 获取充值活动
        /// </summary>
        /// <param name="context"></param>
        public void getPayActivityList2(HttpContext context)
        {
            try
            {
                string companyID = context.Request["companyID"].ToString();
                string sql = "select top 2 * from asm_pay_activity where companyID=" + companyID + " and status=1";
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    string str = "";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        str += "充" + dt.Rows[i]["czMoney"].ToString() + "元" + dt.Rows[i]["tag"].ToString() + ",";
                    }

                    context.Response.Write(str.Substring(0, str.Length - 1));
                }
                context.Response.Write("");
            }
            catch
            {
                context.Response.Write("");
            }
        }
        /// <summary>
        /// 根据minOpenID获取用户信息
        /// </summary>
        /// <param name="context"></param>
        public void getMemberInfo(HttpContext context)
        {
            try
            {
                string minOpenID = context.Request["minOpenID"].ToString();
                string unionID = context.Request["unionID"].ToString();
                Util.Debuglog("minOpenID=" + minOpenID + ";unionID=" + unionID, "getMemberInfo.txt");
                if (string.IsNullOrEmpty(minOpenID) || string.IsNullOrEmpty(unionID) || unionID == "undefined" || minOpenID == "undefined")
                {
                    context.Response.Write("{\"code\":\"500\",\"msg\":\"参数不全,请退出重试\"}");
                    return;
                }
                string sql = "select * from asm_member where unionID='" + unionID + "'";
                Util.Debuglog("sql=" + sql, "getMemberInfo.txt");
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    if (string.IsNullOrEmpty(dt.Rows[0]["minOpenID"].ToString())&&!string.IsNullOrEmpty(minOpenID))
                    {
                        string update = "update asm_member set minOpenID='"+minOpenID+"' where id='"+dt.Rows[0]["id"].ToString()+"'";
                        DbHelperSQL.ExecuteSql(update);
                    }
                    context.Response.Write("{\"code\":\"200\",\"db\":" + Util.DataTableToJsonWithJsonNet(dt) + "}");
                }
                else
                {
                    context.Response.Write("{\"code\":\"400\",\"msg\":\"未获取到用户信息\"}");
                }
            }
            catch
            {
                context.Response.Write("{\"code\":\"500\",\"msg\":\"获取用户信息异常\"}");
            }
        }
        /// <summary>
        /// 获取订单列表
        /// </summary>
        /// <param name="context"></param>
        public void getOrderList(HttpContext context)
        {
            try
            {
                string openID = context.Request["openID"].ToString();
                string unionID = context.Request["unionID"].ToString();
                string companyID = context.Request["companyID"].ToString();
                string type = context.Request["type"].ToString();

                string sql1 = " and 1=1 ";
                if (type != "-1")
                {
                    sql1 = " and orderZT=" + type;
                }
                string sql = "select o.id,o.price,o.createTime,o.totalMoney,o.totalNum,o.syNum,o.fkzt,o.orderZT,o.youhuiMoney,o.orderNO,o.mechineID,m.mechineName,m.addres,o.companyID,o.memberID, p.proName,p.httpImageUrl,p.progg  from asm_orderlist o left join asm_mechine m on  o.mechineID=m.id   left join asm_product p on o.productID=p.productID where  fkzt=1 and p.companyID=" + companyID + " and memberID in (select id from asm_member where minOpenID='" + openID + "') " + sql1+ " order by o.createTime desc ";
                Util.Debuglog(sql, "获取订单列表.txt");
                DataTable db = DbHelperSQL.Query(sql).Tables[0];
                if (db.Rows.Count > 0)
                {
                    context.Response.Write("{\"code\":\"200\",\"db\":" + Util.DataTableToJsonWithJsonNet(db) + "}");
                    //context.Response.Write(Util.DataTableToJsonWithJsonNet(db));
                    return;
                }
                else
                {
                    context.Response.Write("{\"code\":\"500\",\"msg\":\"暂无记录\"}");
                }

            }
            catch
            {
                context.Response.Write("{\"code\":\"500\",\"msg\":\"订单获取异常稍后再试\"}");
            }
        }
        /// <summary>
        /// 创建订购订单
        /// </summary>
        /// <param name="context"></param>
        public void addOrder(HttpContext context)
        {
            try
            {
                string psCycleID = context.Request["psCycleID"].ToString();
                //1 1日1送 2 2日1送 3 3日1送 4 周一至周五 5 周末送
                string psMode = context.Request["psMode"].ToString();
                string unionID = context.Request["unionID"].ToString();
                string openID = context.Request["openID"].ToString();
                string productID = context.Request["productID"].ToString();
                string startDate = context.Request["startDate"].ToString();
                string companyID = context.Request["companyID"].ToString();
                string mechineID = context.Request["mechineID"].ToString();
                string totalMoney = context.Request["totalMoney"].ToString();
                if (string.IsNullOrEmpty(psCycleID) || string.IsNullOrEmpty(psMode) || string.IsNullOrEmpty(unionID) ||
                    string.IsNullOrEmpty(productID) || string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(companyID) ||
                    string.IsNullOrEmpty(mechineID) || string.IsNullOrEmpty(totalMoney))
                {
                    context.Response.Write("{\"code\":\"500\",\"msg\":\"参数不全，请退出重试\"}");
                    return;
                }
                //清空redis
                Util.ClearRedisProductInfoByMechineID(mechineID);
                string orderNO = OperUtil.getOrderNO(mechineID);
                string sqlMechine = "select * from asm_mechine where id=" + mechineID;
                DataTable dM = DbHelperSQL.Query(sqlMechine).Tables[0];
                if (dM.Rows.Count <= 0)
                {
                    context.Response.Write("{\"code\":\"500\",\"msg\":\"机器信息读取失败\"}");
                    return;
                }
                string sqlproduct = "select p.*,b.brandName from asm_product p left join asm_brand b on p.brandID=b.id where p.productID=" + productID;
                DataTable dp = DbHelperSQL.Query(sqlproduct).Tables[0];
                if (dp.Rows.Count <= 0)
                {
                    context.Response.Write("{\"code\":\"500\",\"msg\":\"产品信息读取失败\"}");
                    return;
                }
                string sqlUser = "select * from asm_member where minOpenID='" + openID + "'";
                DataTable duser = DbHelperSQL.Query(sqlUser).Tables[0];
                if (duser.Rows.Count <= 0)
                {
                    context.Response.Write("{\"code\":\"400\",\"msg\":\"会员信息读取失败\"}");
                    return;
                }
                string sqlActivity = "select * from  asm_activity where id=" + psCycleID;
                DataTable dactivity = DbHelperSQL.Query(sqlActivity).Tables[0];
                if (dactivity.Rows.Count <= 0)
                {
                    context.Response.Write("{\"code\":\"500\",\"msg\":\"配送周期信息读取失败\"}");
                    return;
                }
                int days = int.Parse(dactivity.Rows[0]["zq"].ToString());
                if (dactivity.Rows[0]["type"].ToString()=="2")
                {
                    days = days + int.Parse(dactivity.Rows[0]["num"].ToString());
                }
                string[] date = OperUtil.getSelDate(days.ToString(), psMode, startDate).Split(',');
                string insertsql = "insert into asm_orderlist(mechineID,companyID,memberID,productID,totalNum,syNum,createTime,activityID,startTime,endTime,psMode,psModeStr,orderNO,fkzt,orderZT,totalMoney,psCycle,trxid,mechineName,mechineAddress,productBrand,productBrandID,province,city,county,youhuiMoney,price,zqNum)" +
                    "values(@mechineID,@companyID,@memberID,@productID,@totalNum,@syNum,'@createTime',@activityID,'@startTime','@endTime','@psModeID','@psModeStr','@orderNO','@fkzt','@orderZT','@totalMoney','@psCycle','@trxid','@mechineName','@mechineAddress','@productBrand',@BrandID,'@province','@city','@county','@youhuiMoney','@price','@zqNum');select @@IDENTITY";
                insertsql = insertsql.Replace("@mechineID", mechineID);
                insertsql = insertsql.Replace("@companyID", companyID);
                insertsql = insertsql.Replace("@memberID", duser.Rows[0]["id"].ToString());
                insertsql = insertsql.Replace("@productID", productID);
                insertsql = insertsql.Replace("@totalNum", days.ToString());
                insertsql = insertsql.Replace("@syNum", days.ToString());
                insertsql = insertsql.Replace("@createTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                insertsql = insertsql.Replace("@activityID", dactivity.Rows[0]["id"].ToString());
                insertsql = insertsql.Replace("@startTime", startDate);
                insertsql = insertsql.Replace("@endTime", date[date.Length - 1]);
                insertsql = insertsql.Replace("@psModeID", psMode);
                insertsql = insertsql.Replace("@psModeStr", psMode == "1" ? "1日1送" : (psMode == "2" ? "2日1送" : (psMode == "3" ? "3日1送" : (psMode == "4" ? "周一到周五" : (psMode == "5" ? "周末送" : "")))));
                insertsql = insertsql.Replace("@orderNO", OperUtil.getOrderNO(mechineID));
                insertsql = insertsql.Replace("@fkzt", "0");
                insertsql = insertsql.Replace("@orderZT", "0");
                insertsql = insertsql.Replace("@totalMoney", totalMoney);
                insertsql = insertsql.Replace("@trxid", "");
                insertsql = insertsql.Replace("@psCycle", dactivity.Rows[0]["zqName"].ToString() + "+" + dactivity.Rows[0]["activityname"].ToString());
                insertsql = insertsql.Replace("@mechineName", dM.Rows[0]["mechineName"].ToString());
                insertsql = insertsql.Replace("@mechineAddress", dM.Rows[0]["addres"].ToString());
                insertsql = insertsql.Replace("@productBrand", dp.Rows[0]["brandName"].ToString());
                insertsql = insertsql.Replace("@BrandID", dp.Rows[0]["brandID"].ToString());
                insertsql = insertsql.Replace("@province", dM.Rows[0]["province"].ToString());
                insertsql = insertsql.Replace("@city", dM.Rows[0]["city"].ToString());
                insertsql = insertsql.Replace("@county", dM.Rows[0]["country"].ToString());
                insertsql = insertsql.Replace("@youhuiMoney", (double.Parse(dp.Rows[0]["price0"].ToString()) * double.Parse(dactivity.Rows[0]["zq"].ToString()) - double.Parse(totalMoney)).ToString());
                insertsql = insertsql.Replace("@price", dp.Rows[0]["price0"].ToString());
                insertsql = insertsql.Replace("@zqNum", dactivity.Rows[0]["zq"].ToString());
                Util.Debuglog("sql=" + insertsql, "预定订单.txt");

                object obj = DbHelperSQL.GetSingle(insertsql, null);
                if (obj != null)
                {

                    context.Response.Write("{\"code\":\"200\",\"msg\":\"订单提交成功\",\"orderID\":\"" + Convert.ToInt64(obj) + "\"}");
                }
                else
                {
                    context.Response.Write("{\"code\":\"500\",\"msg\":\"订单提交异常\"}");
                }
            }
            catch
            {
                context.Response.Write("{\"code\":\"500\",\"msg\":\"系统异常\"}");
                return;
            }
        }
        /// <summary>
        /// 创建订购订单
        /// </summary>
        /// <param name="context"></param>
        /// 小程序多订单生成
        public void addOrderMany(HttpContext context)
        {
            try
            {
                string psCycleID = context.Request["psCycleID"].ToString();
                //1 1日1送 2 2日1送 3 3日1送 4 周一至周五 5 周末送
                string psMode = context.Request["psMode"].ToString();
                string unionID = context.Request["unionID"].ToString();
                string openID = context.Request["openID"].ToString();
                string productID = context.Request["productID"].ToString();
                string startDate = context.Request["startDate"].ToString();
                string companyID = context.Request["companyID"].ToString();
                string mechineID = context.Request["mechineID"].ToString();
                string totalMoney = context.Request["totalMoney"].ToString();
                string orderNum = context.Request["orderNum"].ToString();
                
                if (string.IsNullOrEmpty(psCycleID) || string.IsNullOrEmpty(psMode) || string.IsNullOrEmpty(unionID) ||
                    string.IsNullOrEmpty(productID) || string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(companyID) ||
                    string.IsNullOrEmpty(mechineID) || string.IsNullOrEmpty(totalMoney) || string.IsNullOrEmpty(orderNum))
                {
                    context.Response.Write("{\"code\":\"500\",\"msg\":\"参数不全，请退出重试\"}");
                    return;
                }
                //清空redis
                Util.ClearRedisProductInfoByMechineID(mechineID);
                string orderNO = OperUtil.getOrderNO(mechineID);
                string sqlMechine = "select * from asm_mechine where id=" + mechineID;
                DataTable dM = DbHelperSQL.Query(sqlMechine).Tables[0];
                if (dM.Rows.Count <= 0)
                {
                    context.Response.Write("{\"code\":\"500\",\"msg\":\"机器信息读取失败\"}");
                    return;
                }
                string sqlproduct = "select p.*,b.brandName from asm_product p left join asm_brand b on p.brandID=b.id where p.productID=" + productID;
                DataTable dp = DbHelperSQL.Query(sqlproduct).Tables[0];
                if (dp.Rows.Count <= 0)
                {
                    context.Response.Write("{\"code\":\"500\",\"msg\":\"产品信息读取失败\"}");
                    return;
                }
                string sqlUser = "select * from asm_member where minOpenID='" + openID + "'";
                DataTable duser = DbHelperSQL.Query(sqlUser).Tables[0];
                if (duser.Rows.Count <= 0)
                {
                    context.Response.Write("{\"code\":\"400\",\"msg\":\"会员信息读取失败\"}");
                    return;
                }
                string sqlActivity = "select * from  asm_activity where id=" + psCycleID;
                DataTable dactivity = DbHelperSQL.Query(sqlActivity).Tables[0];
                if (dactivity.Rows.Count <= 0)
                {
                    context.Response.Write("{\"code\":\"500\",\"msg\":\"配送周期信息读取失败\"}");
                    return;
                }
                int days = int.Parse(dactivity.Rows[0]["zq"].ToString());
                if (dactivity.Rows[0]["type"].ToString() == "2")
                {
                    days = days + int.Parse(dactivity.Rows[0]["num"].ToString());
                }
                string orderIDList = "";
                for (int i=0;i< int.Parse(orderNum);i++) {
                    string[] date = OperUtil.getSelDate(days.ToString(), psMode, startDate).Split(',');
                    string insertsql = "insert into asm_orderlist(mechineID,companyID,memberID,productID,totalNum,syNum,createTime,activityID,startTime,endTime,psMode,psModeStr,orderNO,fkzt,orderZT,totalMoney,psCycle,trxid,mechineName,mechineAddress,productBrand,productBrandID,province,city,county,youhuiMoney,price,zqNum)" +
                        "values(@mechineID,@companyID,@memberID,@productID,@totalNum,@syNum,'@createTime',@activityID,'@startTime','@endTime','@psModeID','@psModeStr','@orderNO','@fkzt','@orderZT','@totalMoney','@psCycle','@trxid','@mechineName','@mechineAddress','@productBrand',@BrandID,'@province','@city','@county','@youhuiMoney','@price','@zqNum');select @@IDENTITY";
                    insertsql = insertsql.Replace("@mechineID", mechineID);
                    insertsql = insertsql.Replace("@companyID", companyID);
                    insertsql = insertsql.Replace("@memberID", duser.Rows[0]["id"].ToString());
                    insertsql = insertsql.Replace("@productID", productID);
                    insertsql = insertsql.Replace("@totalNum", days.ToString());
                    insertsql = insertsql.Replace("@syNum", days.ToString());
                    insertsql = insertsql.Replace("@createTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    insertsql = insertsql.Replace("@activityID", dactivity.Rows[0]["id"].ToString());
                    insertsql = insertsql.Replace("@startTime", startDate);
                    insertsql = insertsql.Replace("@endTime", date[date.Length - 1]);
                    insertsql = insertsql.Replace("@psModeID", psMode);
                    insertsql = insertsql.Replace("@psModeStr", psMode == "1" ? "1日1送" : (psMode == "2" ? "2日1送" : (psMode == "3" ? "3日1送" : (psMode == "4" ? "周一到周五" : (psMode == "5" ? "周末送" : "")))));
                    insertsql = insertsql.Replace("@orderNO", OperUtil.getOrderNO(mechineID));
                    insertsql = insertsql.Replace("@fkzt", "0");
                    insertsql = insertsql.Replace("@orderZT", "0");
                    insertsql = insertsql.Replace("@totalMoney", (double.Parse(totalMoney)/ int.Parse(orderNum)).ToString());
                    insertsql = insertsql.Replace("@trxid", "");
                    insertsql = insertsql.Replace("@psCycle", dactivity.Rows[0]["zqName"].ToString() + "+" + dactivity.Rows[0]["activityname"].ToString());
                    insertsql = insertsql.Replace("@mechineName", dM.Rows[0]["mechineName"].ToString());
                    insertsql = insertsql.Replace("@mechineAddress", dM.Rows[0]["addres"].ToString());
                    insertsql = insertsql.Replace("@productBrand", dp.Rows[0]["brandName"].ToString());
                    insertsql = insertsql.Replace("@BrandID", dp.Rows[0]["brandID"].ToString());
                    insertsql = insertsql.Replace("@province", dM.Rows[0]["province"].ToString());
                    insertsql = insertsql.Replace("@city", dM.Rows[0]["city"].ToString());
                    insertsql = insertsql.Replace("@county", dM.Rows[0]["country"].ToString());
                    insertsql = insertsql.Replace("@youhuiMoney", (double.Parse(dp.Rows[0]["price0"].ToString()) * double.Parse(dactivity.Rows[0]["zq"].ToString()) - double.Parse(totalMoney)).ToString());
                    insertsql = insertsql.Replace("@price", dp.Rows[0]["price0"].ToString());
                    insertsql = insertsql.Replace("@zqNum", dactivity.Rows[0]["zq"].ToString());
                    Util.Debuglog("sql=" + insertsql, "预定订单.txt");

                    object obj = DbHelperSQL.GetSingle(insertsql, null);
                    if (obj == null)
                    {
                        context.Response.Write("{\"code\":\"500\",\"msg\":\"订单提交异常\"}");
                    }
                    else {
                        if (string.IsNullOrEmpty(orderIDList))
                        {
                            orderIDList = obj.ToString();
                        }
                        else {
                            orderIDList = orderIDList + ","+obj.ToString();
                        }
                        
                    }
                }
               
               

                context.Response.Write("{\"code\":\"200\",\"msg\":\"订单提交成功\",\"orderID\":\"" + orderIDList + "\"}");
               
            }
            catch
            {
                context.Response.Write("{\"code\":\"500\",\"msg\":\"系统异常\"}");
                return;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="psCycleID"></param>
        /// <param name="psMode"></param>
        /// <param name="unionID"></param>
        /// <param name="productID"></param>
        /// <param name="startDate"></param>
        /// <param name="companyID"></param>
        /// <param name="mechineID"></param>
        /// <param name="totalMoney"></param>
        /// <param name="days">剩余天数</param>
        /// <returns></returns>
        public  static string addOrder2(string psCycleID, string psMode, string minOpenID, string productID, string startDate, string companyID, string mechineID, string totalMoney, string days , string newOrderNO)
        {
            try
            {
                Util.Debuglog("进入addOrder2", "dhProduct.txt");

                if ( string.IsNullOrEmpty(psMode) || string.IsNullOrEmpty(minOpenID) ||
                    string.IsNullOrEmpty(productID) || string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(companyID) ||
                    string.IsNullOrEmpty(mechineID) || string.IsNullOrEmpty(totalMoney)|| string.IsNullOrEmpty(newOrderNO))
                {
                    return "{\"code\":\"500\",\"msg\":\"参数不全，请退出重试\"}";

                }
                //string orderNO = OperUtil.getOrderNO(mechineID);
                string sqlMechine = "select * from asm_mechine where id=" + mechineID;
                DataTable dM = DbHelperSQL.Query(sqlMechine).Tables[0];
                if (dM.Rows.Count <= 0)
                {
                    return "{\"code\":\"500\",\"msg\":\"机器信息读取失败\"}";

                }
                string sqlproduct = "select p.*,b.brandName from asm_product p left join asm_brand b on p.brandID=b.id where p.productID=" + productID;
                DataTable dp = DbHelperSQL.Query(sqlproduct).Tables[0];
                Util.Debuglog("sqlproduct"+sqlproduct, "dhProduct.txt");
                if (dp.Rows.Count <= 0)
                {
                    return "{\"code\":\"500\",\"msg\":\"产品信息读取失败\"}";

                }
                string sqlUser = "select * from asm_member where minOpenID='" + minOpenID + "'";
                Util.Debuglog("sqlUser" + sqlUser, "dhProduct.txt");
                DataTable duser = DbHelperSQL.Query(sqlUser).Tables[0];
                if (duser.Rows.Count <= 0)
                {
                    return "{\"code\":\"400\",\"msg\":\"会员信息读取失败\"}";

                }
                string sqlActivity = "select * from  asm_activity where id=" + psCycleID;

                Util.Debuglog("sqlActivity" + sqlActivity, "dhProduct.txt");
                DataTable dactivity = DbHelperSQL.Query(sqlActivity).Tables[0];
                string id = "0";
                string name = "";
                if (dactivity.Rows.Count <= 0)
                {

                }
                else
                {
                    id = dactivity.Rows[0]["id"].ToString();
                    name = dactivity.Rows[0]["zqName"].ToString() + "+" + dactivity.Rows[0]["activityname"].ToString();
                }
                string[] date = OperUtil.getSelDate(days, psMode, startDate).Split(',');

                Util.Debuglog("进入date" , "dhProduct.txt");
                //清空redis
                //Util.ClearRedisProductInfoByMechineID(mechineID);
                string insertsql = "insert into asm_orderlist(mechineID,companyID,memberID,productID,totalNum,syNum,createTime,activityID,startTime,endTime,psMode,psModeStr,orderNO,fkzt,orderZT,totalMoney,psCycle,trxid,mechineName,mechineAddress,productBrand,productBrandID,province,city,county,price,source,zqNum)" +
                    "values(@mechineID,@companyID,@memberID,@productID,@totalNum,@syNum,'@createTime',@activityID,'@startTime','@endTime','@psModeID','@psModeStr','@orderNO','@fkzt','@orderZT','@totalMoney','@psCycle','@trxid','@mechineName','@mechineAddress','@productBrand',@BrandID,'@province','@city','@county','@price','@source','@zqNum');select @@IDENTITY";
                insertsql = insertsql.Replace("@mechineID", mechineID);
                insertsql = insertsql.Replace("@companyID", companyID);
                insertsql = insertsql.Replace("@memberID", duser.Rows[0]["id"].ToString());
                insertsql = insertsql.Replace("@productID", productID);
                insertsql = insertsql.Replace("@totalNum", days);
                insertsql = insertsql.Replace("@syNum", days);
                insertsql = insertsql.Replace("@createTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                insertsql = insertsql.Replace("@activityID", id);
                insertsql = insertsql.Replace("@startTime", startDate);
                insertsql = insertsql.Replace("@endTime", date[date.Length - 1]);
                insertsql = insertsql.Replace("@psModeID", psMode);
                insertsql = insertsql.Replace("@psModeStr", psMode == "1" ? "1日1送" : (psMode == "2" ? "2日1送" : (psMode == "3" ? "3日1送" : (psMode == "4" ? "周一到周五" : (psMode == "5" ? "周末送" : "")))));
                insertsql = insertsql.Replace("@orderNO", newOrderNO);
                insertsql = insertsql.Replace("@fkzt", "0");
                insertsql = insertsql.Replace("@orderZT", "0");
                insertsql = insertsql.Replace("@totalMoney", totalMoney);
                insertsql = insertsql.Replace("@trxid", "");
                insertsql = insertsql.Replace("@psCycle", name);
                insertsql = insertsql.Replace("@mechineName", dM.Rows[0]["mechineName"].ToString());
                insertsql = insertsql.Replace("@mechineAddress", dM.Rows[0]["addres"].ToString());
                insertsql = insertsql.Replace("@productBrand", dp.Rows[0]["brandName"].ToString());
                insertsql = insertsql.Replace("@BrandID", dp.Rows[0]["brandID"].ToString());
                insertsql = insertsql.Replace("@province", dM.Rows[0]["province"].ToString());
                insertsql = insertsql.Replace("@city", dM.Rows[0]["city"].ToString());
                insertsql = insertsql.Replace("@county", dM.Rows[0]["country"].ToString());
               // insertsql = insertsql.Replace("@youhuiMoney", (double.Parse(dp.Rows[0]["price0"].ToString()) * double.Parse(dactivity.Rows[0]["zq"].ToString()) - double.Parse(totalMoney)).ToString());
                insertsql = insertsql.Replace("@price", dp.Rows[0]["price0"].ToString());
                insertsql = insertsql.Replace("@source", "1");
                insertsql = insertsql.Replace("@zqNum", days);
                Util.Debuglog("sql=" + insertsql, "预定订单.txt");

                object obj = DbHelperSQL.GetSingle(insertsql, null);
                //因为此处牵扯到差额付款，所以不需要在此生成listdetail，生成方法写到pay方法里
                if (obj != null)
                {

                    return "{\"code\":\"200\",\"msg\":\"订单提交成功\",\"orderID\":\"" + Convert.ToInt64(obj) + "\",\"activityID\":\"" + id + "\"}";
                }
                else
                {
                    return "{\"code\":\"500\",\"msg\":\"订单提交异常\"}";
                }
            }
            catch(Exception e)
            {
                return "{\"code\":\"500\",\"msg\":\"系统异常\"}";

            }
        }
        public static  string addOrder3(string psCycleID, string psMode, string minOpenID, string productID, string startDate, string companyID, string mechineID, string totalMoney, string days, string chaMoney, string newOrderNO)
        {
            try
            {
                Util.Debuglog("进入addOrder2", "dhProduct.txt");
                if (string.IsNullOrEmpty(psMode) || string.IsNullOrEmpty(minOpenID) ||
                    string.IsNullOrEmpty(productID) || string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(companyID) ||
                    string.IsNullOrEmpty(mechineID) || string.IsNullOrEmpty(totalMoney) || string.IsNullOrEmpty(newOrderNO))
                {
                    return "{\"code\":\"500\",\"msg\":\"参数不全，请退出重试\"}";

                }
                string sqlMechine = "select * from asm_mechine where id=" + mechineID;
                Util.Debuglog("sqlMechine"+ sqlMechine, "dhProduct.txt");
                DataTable dM = DbHelperSQL.Query(sqlMechine).Tables[0];
                if (dM.Rows.Count <= 0)
                {
                    return "{\"code\":\"500\",\"msg\":\"机器信息读取失败\"}";

                }
                //清空redis
                Util.ClearRedisProductInfoByMechineID(mechineID);
                string sqlproduct = "select p.*,b.brandName from asm_product p left join asm_brand b on p.brandID=b.id where p.productID=" + productID;
                Util.Debuglog("sqlproduct" + sqlproduct, "dhProduct.txt");
                DataTable dp = DbHelperSQL.Query(sqlproduct).Tables[0];
                if (dp.Rows.Count <= 0)
                {
                    return "{\"code\":\"500\",\"msg\":\"产品信息读取失败\"}";

                }
                string sqlUser = "select * from asm_member where minOpenID='" + minOpenID + "'";
                DataTable duser = DbHelperSQL.Query(sqlUser).Tables[0];
                if (duser.Rows.Count <= 0)
                {
                    return "{\"code\":\"400\",\"msg\":\"会员信息读取失败\"}";

                }
                string sqlActivity = "select * from  asm_activity where id='" + psCycleID+"'";
                Util.Debuglog("sqlActivity" + sqlActivity, "dhProduct.txt");
                DataTable dactivity = DbHelperSQL.Query(sqlActivity).Tables[0];
                string id = "0";
                string name = "";
                if (dactivity.Rows.Count <= 0)
                {

                }
                else {
                    id = dactivity.Rows[0]["id"].ToString();
                    name = dactivity.Rows[0]["zqName"].ToString() + "+" + dactivity.Rows[0]["activityname"].ToString();
                }
                string[] date = OperUtil.getSelDate(days, psMode, startDate).Split(',');
                Util.Debuglog("date" , "dhProduct.txt");
                string insertsql = "insert into asm_orderlist(mechineID,companyID,memberID,productID,totalNum,syNum,createTime,activityID,startTime,endTime,psMode,psModeStr,orderNO,fkzt,orderZT,totalMoney,psCycle,trxid,mechineName,mechineAddress,productBrand,productBrandID,province,city,county,price,source,zqNum)" +
                    "values(@mechineID,@companyID,@memberID,@productID,@totalNum,@syNum,'@createTime',@activityID,'@startTime','@endTime','@psModeID','@psModeStr','@orderNO','@fkzt','@orderZT','@totalMoney','@psCycle','@trxid','@mechineName','@mechineAddress','@productBrand',@BrandID,'@province','@city','@county','@price','@source','@zqNum');select @@IDENTITY";
                insertsql = insertsql.Replace("@mechineID", mechineID);
                insertsql = insertsql.Replace("@companyID", companyID);
                insertsql = insertsql.Replace("@memberID", duser.Rows[0]["id"].ToString());
                insertsql = insertsql.Replace("@productID", productID);
                insertsql = insertsql.Replace("@totalNum", days);
                insertsql = insertsql.Replace("@syNum", days);
                insertsql = insertsql.Replace("@createTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                insertsql = insertsql.Replace("@activityID",id );
                insertsql = insertsql.Replace("@startTime", startDate);
                insertsql = insertsql.Replace("@endTime", date[date.Length - 1]);
                insertsql = insertsql.Replace("@psModeID", psMode);
                insertsql = insertsql.Replace("@psModeStr", psMode == "1" ? "1日1送" : (psMode == "2" ? "2日1送" : (psMode == "3" ? "3日1送" : (psMode == "4" ? "周一到周五" : (psMode == "5" ? "周末送" : "")))));
                insertsql = insertsql.Replace("@orderNO", newOrderNO);
                insertsql = insertsql.Replace("@fkzt", "1");
                insertsql = insertsql.Replace("@orderZT", "0");
                insertsql = insertsql.Replace("@totalMoney", totalMoney);
                insertsql = insertsql.Replace("@trxid", "");
                insertsql = insertsql.Replace("@psCycle",name);
                insertsql = insertsql.Replace("@mechineName", dM.Rows[0]["mechineName"].ToString());
                insertsql = insertsql.Replace("@mechineAddress", dM.Rows[0]["addres"].ToString());
                insertsql = insertsql.Replace("@productBrand", dp.Rows[0]["brandName"].ToString());
                insertsql = insertsql.Replace("@BrandID", dp.Rows[0]["brandID"].ToString());
                insertsql = insertsql.Replace("@province", dM.Rows[0]["province"].ToString());
                insertsql = insertsql.Replace("@city", dM.Rows[0]["city"].ToString());
                insertsql = insertsql.Replace("@county", dM.Rows[0]["country"].ToString());
                //insertsql = insertsql.Replace("@youhuiMoney", (double.Parse(dp.Rows[0]["price0"].ToString()) * double.Parse(dactivity.Rows[0]["zq"].ToString()) - double.Parse(totalMoney)).ToString());
                insertsql = insertsql.Replace("@price", dp.Rows[0]["price0"].ToString());
                insertsql = insertsql.Replace("@source", "1");
                insertsql = insertsql.Replace("@zqNum", days);
                Util.Debuglog("sql=" + insertsql, "预定订单.txt");

                object obj = DbHelperSQL.GetSingle(insertsql, null);
                if (obj != null)
                {
                    string[] selDate = OperUtil.getSelDate(days, psMode, startDate).Split(',');

                    if (selDate.Length > 0)
                    {
                        string sql14 = "select * from asm_orderlistDetail where id=0";
                        DataTable dtNew = DbHelperSQL.Query(sql14).Tables[0];
                        for (int i = 0; i < selDate.Length; i++)
                        {
                            int code = new Random(Guid.NewGuid().GetHashCode()).Next(100000, 999999);

                            //zt   1-已完成；2-已失效；3-已转售；4-待取货；5-待配送
                            DataRow dr = dtNew.NewRow();
                            dr["mechineID"] = mechineID; //通过索引赋值
                            dr["productID"] = productID;
                            dr["createTime"] = delTime(selDate[i]);//
                            dr["code"] = code;//
                            dr["memberID"] = duser.Rows[0]["id"].ToString(); //通过索引赋值
                            if (delTime(selDate[i]) == DateTime.Now.ToString("yyyy-MM-dd"))
                            {
                                dr["zt"] = "4";
                            }
                            else
                            {
                                dr["zt"] = "5";
                            }
                            dr["ldNO"] = "";//
                            dr["orderNO"] = newOrderNO;//
                            dr["statu"] = "0"; //通过索引赋值
                            dr["sellPrice"] = 0.0;
                            dr["sellTime"] = "";
                            dr["bz"] = "";
                            dr["companyID"] = companyID;
                            dtNew.Rows.Add(dr);
                        }
                        DbHelperSQL.BatchInsertBySqlBulkCopy(dtNew, "[dbo].[asm_orderlistDetail]");
                    }
                    string sql1 = "select * from asm_orderlistDetail where orderNO in ('" + newOrderNO + "') ORDER BY createTime DESC ";
                    DataTable d1 = DbHelperSQL.Query(sql1).Tables[0];
                    if (d1.Rows.Count > 0)
                    {
                        string update12 = "UPDATE asm_orderlist set endTime='" + d1.Rows[0]["createTime"].ToString() + "' WHERE orderNO in ('" + newOrderNO + "')";
                        DbHelperSQL.ExecuteSql(update12);
                    }
                    //退款到余额
                    string sql = "update asm_member set availableMoney=availableMoney+" + chaMoney + " where id=" + duser.Rows[0]["id"].ToString();
                    int a = DbHelperSQL.ExecuteSql(sql);
                    if (a > 0)
                    {
                        //
                        Util.chgMoney(duser.Rows[0]["id"].ToString(), chaMoney, "兑换产品", "兑换产品退还差额", "5");
                    }
                    return "{\"code\":\"200\",\"msg\":\"订单提交成功\",\"orderID\":\"" + Convert.ToInt64(obj) + "\",\"activityID\":\"" + id + "\"}";
                }
                else
                {
                    return "{\"code\":\"500\",\"msg\":\"订单提交异常\"}";
                }
            }
            catch
            {
                return "{\"code\":\"500\",\"msg\":\"系统异常\"}";

            }
        }
        /// <summary>
        /// 订购订单兑换
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public void addDGOrder(HttpContext context)
        {
            try
            {
                string code = context.Request["code"].ToString();
                string psMode = context.Request["psMode"].ToString();
                string unionID = context.Request["unionID"].ToString();
                string openID = context.Request["openID"].ToString();
                string startDate = context.Request["startDate"].ToString();
                string companyID = context.Request["companyID"].ToString();
                string mechineID = context.Request["mechineID"].ToString();
                Util.Debuglog("code=" + code + ";psMode=" + psMode + ";unionID=" + unionID + ";startDate=" + startDate + ";companyID=" + companyID + ";mechineID=" + mechineID, "addDGOrder.txt");
                if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(psMode) || string.IsNullOrEmpty(unionID) || string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(companyID) || string.IsNullOrEmpty(mechineID))
                {

                    context.Response.Write("{\"code\":\"500\",\"msg\":\"参数不全，请退出重试\"}");
                    return;
                }
                string sqlCode = "select * from  asm_dgOrder where orderCode='" + code + "'";
                Util.Debuglog("sqlCode=" + sqlCode, "addDGOrder.txt");
                DataTable dcode = DbHelperSQL.Query(sqlCode).Tables[0];
                if (dcode.Rows.Count <= 0)
                {

                    context.Response.Write("{\"code\":\"500\",\"msg\":\"订奶码不存在\"}");
                    return;
                }
                if (dcode.Rows[0]["status"].ToString() == "1")
                {
                    context.Response.Write("{\"code\":\"500\",\"msg\":\"订奶码已经使用\"}");
                    return;
                }
                //清空redis
                Util.ClearRedisProductInfoByMechineID(mechineID);
                string sqlM = "select * from asm_member where minOpenID='" + openID + "'";
                Util.Debuglog("sqlM=" + sqlM, "addDGOrder.txt");
                DataTable dm = DbHelperSQL.Query(sqlM).Tables[0];//取会员数据
                //修改asm_dgOrder表 status为1 已兑换 
                string update1 = "update  asm_dgOrder set status=1,memberID='"+ dm.Rows[0]["id"].ToString()+ "' ,dhTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where orderCode='" + code + "' and status=0";
                Util.Debuglog("update1=" + update1, "addDGOrder.txt");
                int a = DbHelperSQL.ExecuteSql(update1);
                if (a > 0)
                {
                    string orderNO = OperUtil.getOrderNO(mechineID);
                   
                    string sqlp = "select p.*,b.brandName from asm_product p left join asm_brand b on p.brandID=b.id where bh='" + dcode.Rows[0]["productCode"].ToString() + "'";
                    Util.Debuglog("sqlp=" + sqlp, "addDGOrder.txt");
                    DataTable dp = DbHelperSQL.Query(sqlp).Tables[0];
                    string sqlMechine = "select * from  asm_mechine where id=" + mechineID;
                    Util.Debuglog("sqlMechine=" + sqlMechine, "addDGOrder.txt");
                    DataTable dMechine = DbHelperSQL.Query(sqlMechine).Tables[0];
                    if (dm.Rows.Count > 0)
                    {
                        string[] selDate = OperUtil.getSelDate(dcode.Rows[0]["zq"].ToString(), psMode, startDate).Split(',');
                        //向asm_orderlist 表插入记录
                        string insert = "insert into asm_orderlist(mechineID,companyID,memberID,productID,totalNum,price,syNum,createTime,startTime,endTime,psMode,psModeStr,orderNO,fkzt,orderZT,totalMoney,mechineName,mechineAddress,productBrand,province,city,county,productBrandID,exportData,zqNum) "
                            + "values(@mechineID,@companyID,@memberID,@productID,@totalNum,@price,@syNum,'@createTime','@startTime','@endTime',@psMode,'@psStr','@orderNO',@fkzt,@orderZT,@totalMoney,'@mechineName','@mechineAddress','@productBrand','@province','@city','@county',@productBID,@exportData,'@zqNum');select @@IDENTITY";
                        insert = insert.Replace("@mechineID", mechineID);
                        insert = insert.Replace("@companyID", companyID);
                        insert = insert.Replace("@memberID", dm.Rows[0]["id"].ToString());
                        insert = insert.Replace("@productID", dp.Rows[0]["productID"].ToString());
                        insert = insert.Replace("@totalNum", dcode.Rows[0]["zq"].ToString());
                        insert = insert.Replace("@price", dcode.Rows[0]["productPrice"].ToString());
                        insert = insert.Replace("@syNum", dcode.Rows[0]["zq"].ToString());//周期
                        insert = insert.Replace("@createTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        insert = insert.Replace("@startTime", startDate);
                        insert = insert.Replace("@endTime", selDate[selDate.Length - 1]);
                        insert = insert.Replace("@psMode", psMode);
                        insert = insert.Replace("@psStr", psMode == "1" ? "1日1送" : (psMode == "2" ? "2日1送" : (psMode == "3" ? "3日1送" : (psMode == "4" ? "周一到周五" : (psMode == "5" ? "周末送" : "")))));
                        insert = insert.Replace("@orderNO", orderNO);
                        insert = insert.Replace("@fkzt", "1");
                        insert = insert.Replace("@orderZT", "0");
                        insert = insert.Replace("@totalMoney", (double.Parse(dcode.Rows[0]["zq"].ToString()) * double.Parse(dcode.Rows[0]["productPrice"].ToString())).ToString());
                        insert = insert.Replace("@mechineName", dMechine.Rows[0]["mechineName"].ToString());
                        insert = insert.Replace("@mechineAddress", dMechine.Rows[0]["addres"].ToString());
                        insert = insert.Replace("@productBrand", dp.Rows[0]["brandName"].ToString());
                        insert = insert.Replace("@province", dMechine.Rows[0]["province"].ToString());
                        insert = insert.Replace("@city", dMechine.Rows[0]["city"].ToString());
                        insert = insert.Replace("@county", dMechine.Rows[0]["country"].ToString());
                        insert = insert.Replace("@productBID", dp.Rows[0]["brandID"].ToString());
                        insert = insert.Replace("@exportData", "1");
                        insert = insert.Replace("@zqNum", dcode.Rows[0]["zq"].ToString());
                        Util.Debuglog("insert=" + insert, "addDGOrder.txt");
                        object obj = DbHelperSQL.GetSingle(insert, null);
                        Util.Debuglog("obj=" + obj, "addDGOrder.txt");
                        if (obj != null)
                        {
                            Util.Debuglog("date=" + OperUtil.getSelDate(dcode.Rows[0]["zq"].ToString(), psMode, startDate), "addDGOrder.txt");
                            if (selDate.Length > 0)
                            {
                                string sql14 = "select * from asm_orderlistDetail where id=0";
                                DataTable dtNew = DbHelperSQL.Query(sql14).Tables[0];
                                for (int i = 0; i < selDate.Length; i++)
                                {
                                    int QHcode = new Random(Guid.NewGuid().GetHashCode()).Next(100000, 999999);

                                    //zt   1-已完成；2-已失效；3-已转售；4-待取货；5-待配送
                                    DataRow dr = dtNew.NewRow();
                                    dr["mechineID"] = mechineID; //通过索引赋值
                                    dr["productID"] = dp.Rows[0]["productID"].ToString();
                                    dr["createTime"] = delTime(selDate[i]);//
                                    dr["code"] = QHcode;//
                                    dr["memberID"] = dm.Rows[0]["id"].ToString(); //通过索引赋值
                                    if (delTime(selDate[i]) == DateTime.Now.ToString("yyyy-MM-dd"))
                                    {
                                        dr["zt"] = "4";
                                    }
                                    else
                                    {
                                        dr["zt"] = "5";
                                    }
                                    dr["ldNO"] = "";//
                                    dr["orderNO"] = orderNO;//
                                    dr["statu"] = "0"; //通过索引赋值
                                    dr["sellPrice"] = 0.0;
                                    dr["sellTime"] = "";
                                    dr["bz"] = "";
                                    dr["companyID"] = companyID;
                                    dtNew.Rows.Add(dr);
                                }
                                DbHelperSQL.BatchInsertBySqlBulkCopy(dtNew, "[dbo].[asm_orderlistDetail]");

                                string sql1 = "select * from asm_orderlistDetail where orderNO in ('" + orderNO + "') ORDER BY createTime DESC ";
                                DataTable d1 = DbHelperSQL.Query(sql1).Tables[0];
                                if (d1.Rows.Count > 0)
                                {
                                    string update12 = "UPDATE asm_orderlist set endTime='" + d1.Rows[0]["createTime"].ToString() + "' WHERE orderNO in ('" + orderNO + "')";
                                    DbHelperSQL.ExecuteSql(update12);
                                }
                                context.Response.Write("{\"code\":\"200\",\"msg\":\"订单提交成功\"}");
                                return;
                            }
                        }
                        else
                        {
                            context.Response.Write("{\"code\":\"500\",\"msg\":\"订单提交异常\"}");
                            return;
                        }
                    }
                    else
                    {
                        context.Response.Write("{\"code\":\"500\",\"msg\":\"会员信息读取失败\"}");
                        return;
                    }
                }
                else
                {
                    context.Response.Write("{\"code\":\"500\",\"msg\":\"订单更新失败\"}");
                    return;
                }


            }
            catch
            {
                context.Response.Write("{\"code\":\"500\",\"msg\":\"系统异常\"}");
                return;

            }
        }
        /// <summary>
        /// 发起支付
        /// </summary>
        /// <param name="context"></param>
        public void pay(HttpContext context)
        {
            string openid = context.Request["openID"].ToString();
            string unionID = context.Request["unionID"].ToString();
            string companyID = context.Request["companyID"].ToString();
            string orderID = context.Request["orderID"].ToString();
            string money = context.Request["money"].ToString();
            string activityID = context.Request["activityID"].ToString();
            Util.Debuglog("openid="+ openid+ ";unionID="+ unionID+ ";companyID="+ companyID+ ";orderID="+ orderID+ ";money="+ money+ ";activityID="+ activityID, "_订单支付2.txt");
            int fen = 0;
            string json = "";
            try
            {
                if (string.IsNullOrEmpty(openid))
                {
                    context.Response.Write("{\"code\":\"500\",\"msg\":\"参数不全,请退出重试\"}");
                    return;
                }
                DataTable dtM = DbHelperSQL.Query("select * from asm_member where minOpenID='" + openid + "'").Tables[0];
                if (dtM.Rows.Count <= 0)
                {
                    context.Response.Write("{\"code\":\"400\",\"msg\":\"会员信息查询失败\"}");
                    return;
                }
                
                
                fen = int.Parse(((double.Parse(money) * 100).ToString()));
                //获取收货地址js函数入口参数
                string url = "https://wx.bingoseller.com/pay/wxOrder.aspx";
                SybWxPayService sybService = new SybWxPayService(companyID);
                Dictionary<String, String> rsp = sybService.payW06(fen, DateTime.Now.ToFileTime().ToString(), "W06", "会员订购", "会员订购", openid, "", url, "", Util.getMinAppid(companyID));
                json = (new JavaScriptSerializer()).Serialize(rsp);
                JObject jo = (JObject)JsonConvert.DeserializeObject(json);
                if (jo["retcode"].ToString() == "SUCCESS")
                {
                    string appid = jo["appid"].ToString();
                    string cusid = jo["cusid"].ToString();
                    string trxid = jo["trxid"].ToString();
                    string reqsn = jo["reqsn"].ToString();

                    //插入预订单信息
                    string insertSQL = @"insert into asm_pay_info(appid,cusid,trxid,acct,statu,reqsn,type,payType,trxamt,activityID,unionID,companyID,dgOrderDetailID)
                                            values('" + appid + "','" + cusid + "','" + trxid + "','" + openid + "','0','" + reqsn + "',3,3," + fen + ",'" + activityID + "','" + unionID + "','" + companyID + "','"+orderID+"')";
                    Util.Debuglog("insertSQL=" + insertSQL, "_订单支付2.txt");
                    //根据订单编号更新 订单表  在回调方法里添加订单明细记录
                    string update = "update asm_orderlist set trxid='" + trxid + "' where id=" + orderID;
                    Util.Debuglog("update=" + update, "_订单支付2.txt");
                    DbHelperSQL.ExecuteSql(insertSQL);
                    DbHelperSQL.ExecuteSql(update);


                    Util.Debuglog("payinfo=" + jo["payinfo"].ToString(), "_订单支付2.txt");
                    context.Response.Write("{\"code\":\"200\",\"payInfo\":" + jo["payinfo"].ToString() + "}");


                }
            }
            catch (Exception ex)
            {
                Util.Debuglog("err=" + ex.Message, "_订单支付2.txt");
                context.Response.Write("{\"code\":\"500\",\"msg\":\"微信支付发起失败\"}");
                return;
            }
        }
        /// <summary>
        /// 发起支付，，，小程序多份购买付款
        /// </summary>
        /// <param name="context"></param>
        public void payMany(HttpContext context)
        {
            string openid = context.Request["openID"].ToString();
            string unionID = context.Request["unionID"].ToString();
            string companyID = context.Request["companyID"].ToString();
            string orderID = context.Request["orderID"].ToString();
            string money = context.Request["money"].ToString();
            string activityID = context.Request["activityID"].ToString();
            Util.Debuglog("openid=" + openid + ";unionID=" + unionID + ";companyID=" + companyID + ";orderID=" + orderID + ";money=" + money + ";activityID=" + activityID, "_订单支付2.txt");
            int fen = 0;
            string json = "";
            try
            {
                if (string.IsNullOrEmpty(openid))
                {
                    context.Response.Write("{\"code\":\"500\",\"msg\":\"参数不全,请退出重试\"}");
                    return;
                }
                DataTable dtM = DbHelperSQL.Query("select * from asm_member where minOpenID='" + openid + "'").Tables[0];
                if (dtM.Rows.Count <= 0)
                {
                    context.Response.Write("{\"code\":\"400\",\"msg\":\"会员信息查询失败\"}");
                    return;
                }


                fen = int.Parse(((double.Parse(money) * 100).ToString()));
                //获取收货地址js函数入口参数
                string url = "https://wx.bingoseller.com/pay/wxOrderMany.aspx";
                SybWxPayService sybService = new SybWxPayService(companyID);
                Dictionary<String, String> rsp = sybService.payW06(fen, DateTime.Now.ToFileTime().ToString(), "W06", "会员订购", "会员订购", openid, "", url, "", Util.getMinAppid(companyID));
                json = (new JavaScriptSerializer()).Serialize(rsp);
                JObject jo = (JObject)JsonConvert.DeserializeObject(json);
                if (jo["retcode"].ToString() == "SUCCESS")
                {
                    string appid = jo["appid"].ToString();
                    string cusid = jo["cusid"].ToString();
                    string trxid = jo["trxid"].ToString();
                    string reqsn = jo["reqsn"].ToString();

                    //插入预订单信息
                    string insertSQL = @"insert into asm_pay_info(appid,cusid,trxid,acct,statu,reqsn,type,payType,trxamt,activityID,unionID,companyID,dgOrderDetailID)
                                            values('" + appid + "','" + cusid + "','" + trxid + "','" + openid + "','0','" + reqsn + "',3,3," + fen + ",'" + activityID + "','" + unionID + "','" + companyID + "','" + orderID + "')";
                    Util.Debuglog("insertSQL=" + insertSQL, "_订单支付2.txt");
                    //根据订单编号更新 订单表  在回调方法里添加订单明细记录
                    string update = "update asm_orderlist set trxid='" + trxid + "' where id in (" + orderID+")";
                    Util.Debuglog("update=" + update, "_订单支付2.txt");
                    DbHelperSQL.ExecuteSql(insertSQL);
                    DbHelperSQL.ExecuteSql(update);


                    Util.Debuglog("payinfo=" + jo["payinfo"].ToString(), "_订单支付2.txt");
                    context.Response.Write("{\"code\":\"200\",\"payInfo\":" + jo["payinfo"].ToString() + "}");


                }
            }
            catch (Exception ex)
            {
                Util.Debuglog("err=" + ex.Message, "_订单支付2.txt");
                context.Response.Write("{\"code\":\"500\",\"msg\":\"微信支付发起失败\"}");
                return;
            }
        }
        
        public void registUserByPhone(HttpContext context)
        {
            string companyID = context.Request["companyID"].ToString();
            string minOpenID = context.Request["minOpenID"].ToString();//小程序openID
            string unionID = context.Request["unionID"].ToString();//
            string phone = context.Request["phone"].ToString();
            Util.Debuglog("companyID=" + companyID + ";minOpenID=" + minOpenID + ";unionID=" + unionID + ";phone=" + phone, "registUserByPhone.txt");

            //根据unionid判断用户是否存在
            if (phone == "undefined")
            {
                phone = "";
            }
            if (unionID == "undefined")
            {
                unionID = "";
            }
            if (string.IsNullOrEmpty(minOpenID) || string.IsNullOrEmpty(unionID) || unionID == "undefined")
            {
                context.Response.Write("{\"code\":\"500\",\"msg\":\"参数不全\"}");
                return;
            }
            string sql = "select * from asm_member where unionID='" + unionID + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count <= 0)
            {
                string insert = "insert into asm_member(phone,AvailableMoney,sumConsume,sumRecharge,createDate,companyID,unionID,minOpenID)"
                    + " values('" + phone + "',0,0,0,'" + DateTime.Now + "','" + companyID + "','" + unionID + "','" + minOpenID + "')";
                Util.Debuglog(insert, "用户注册.txt");
                DbHelperSQL.ExecuteSql(insert);
            }
            else
            {
                //更新手机号和minOpenID
                if (string.IsNullOrEmpty(dt.Rows[0]["phone"].ToString()))
                {
                    string update = "update asm_member set minOpenID='" + minOpenID + "',phone='" + phone + "',dj=1 where unionID='" + unionID + "'";
                    DbHelperSQL.ExecuteSql(update);
                }

            }
            sql = "select * from asm_member where minOpenID='" + minOpenID + "'";
            dt = DbHelperSQL.Query(sql).Tables[0];
            context.Response.Write(Util.DataTableToJsonWithJsonNet(dt));
        }
        public void registUser(HttpContext context)
        {
            string companyID = context.Request["companyID"].ToString();
            string nickname = context.Request["nickname"].ToString();
            string gender = context.Request["gender"].ToString() == "1" ? "男" : "女";//1男2女
            string city = context.Request["city"].ToString();
            string province = context.Request["province"].ToString();
            string country = context.Request["country"].ToString();
            string minOpenID = context.Request["minOpenID"].ToString();//小程序openID
            string unionID = context.Request["unionID"].ToString();//
            string phone = context.Request["phone"].ToString();
            string headImg = context.Request["headImg"].ToString();

            Util.Debuglog("phone=" + phone + "companyID=" + companyID + ";nickname=" + nickname + ";city=" + city + ";province=" + province + ";minOpenID=" + minOpenID + ";unionID=" + unionID + ";gender=" + gender, "用户注册.txt");
            //根据unionid判断用户是否存在
            if (phone == "undefined")
            {
                phone = "";
            }
            if (string.IsNullOrEmpty(minOpenID) || string.IsNullOrEmpty(unionID) || unionID == "undefined")
            {
                context.Response.Write("{\"code\":\"500\",\"msg\":\"参数不全\"}");
                return;
            }
            string sql = "select * from asm_member where unionID='" + unionID + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count <= 0)
            {
                if (string.IsNullOrEmpty(phone))
                {
                    string insert = "insert into asm_member(name,phone,province,city,country,AvailableMoney,sumConsume,sumRecharge,createDate,companyID,headurl,nickname,sex,unionID,minOpenID)"
                   + " values('" + nickname + "','" + phone + "','" + province + "','" + city + "','" + country + "',0,0,0,'" + DateTime.Now + "','" + companyID + "','" + headImg + "','" + nickname + "','" + gender + "','" + unionID + "','" + minOpenID + "')";
                    Util.Debuglog(insert, "用户注册.txt");
                    DbHelperSQL.ExecuteSql(insert);
                }
                else
                {
                    string insert = "insert into asm_member(dj,name,phone,province,city,country,AvailableMoney,sumConsume,sumRecharge,createDate,companyID,headurl,nickname,sex,unionID,minOpenID)"
                   + " values('1','" + nickname + "','" + phone + "','" + province + "','" + city + "','" + country + "',0,0,0,'" + DateTime.Now + "','" + companyID + "','" + headImg + "','" + nickname + "','" + gender + "','" + unionID + "','" + minOpenID + "')";
                    Util.Debuglog(insert, "用户注册.txt");
                    DbHelperSQL.ExecuteSql(insert);
                }

            }
            else
            {
                //更新手机号和minOpenID
                if (string.IsNullOrEmpty(dt.Rows[0]["phone"].ToString()))
                {
                    string update = "update asm_member set phone='" + phone + "',nickname='" + nickname + "',province='" + province + "',city='" + city + "',country='" + country + "',headurl='" + headImg + "',sex='" + gender + "',name='" + nickname + "',unionID='" + unionID + "' where minOpenID='" + minOpenID + "'";
                    DbHelperSQL.ExecuteSql(update);
                }
                else
                {
                    string update = "update asm_member set nickname='" + nickname + "',province='" + province + "',city='" + city + "',country='" + country + "',headurl='" + headImg + "',sex='" + gender + "',name='" + nickname + "',unionID='" + unionID + "' where minOpenID='" + minOpenID + "'";
                    DbHelperSQL.ExecuteSql(update);
                }

            }
            sql = "select * from asm_member where minOpenID='" + minOpenID + "'";
            dt = DbHelperSQL.Query(sql).Tables[0];
            context.Response.Write(Util.DataTableToJsonWithJsonNet(dt));
        }
        public void userlogin(HttpContext context)
        {
            try
            {
                
                string code = context.Request["code"].ToString();
                string companyID = context.Request["companyID"].ToString();
                Util.Debuglog("code=" + code+ ";companyID="+ companyID, "小程序登录.txt");
                string url = "https://api.weixin.qq.com/sns/jscode2session?appid=" + Util.getMinAppid(companyID) + "&secret=" + Util.getMinSecret(companyID) + "&js_code=" + code + "&grant_type=authorization_code";
                string type = "utf-8";
                GetUsersHelper GetUsersHelper = new GetUsersHelper();
                string j = GetUsersHelper.GetUrltoHtml(url, type);//获取微信服务器返回字符串  
                                                                  //将字符串转换为json格式  
                JObject jo = (JObject)JsonConvert.DeserializeObject(j);
                //{"session_key":"WGU5gsB9zNSGr+lTx9L3kQ==","openid":"oJ1cB5cNfxOI2COElfzZiB-mUfuc","unionid":"owhCR0Yh6wTJwWWmeNihP4_7VEPU"}
                Util.Debuglog("j=" + j, "小程序登录.txt");
                context.Response.Write(j);

            }
            catch(Exception ex)
            {
                Util.Debuglog("ex=" + ex.Message, "小程序登录.txt");
            }

        }
        public void userlogin2(HttpContext context)
        {
            try
            {
                string code = context.Request["code"].ToString();
                string companyID = context.Request["companyID"].ToString();
                Util.Debuglog("code=" + code + ";companyID=" + companyID, "小程序登录2.txt");
                string url = "https://api.weixin.qq.com/sns/jscode2session?appid=" + Util.getMinAppid(companyID) + "&secret=" + Util.getMinSecret(companyID) + "&js_code=" + code + "&grant_type=authorization_code";
                string type = "utf-8";
                GetUsersHelper GetUsersHelper = new GetUsersHelper();
                string j = GetUsersHelper.GetUrltoHtml(url, type);//获取微信服务器返回字符串  
                                                                  //将字符串转换为json格式  
                JObject jo = (JObject)JsonConvert.DeserializeObject(j);
                //{"session_key":"WGU5gsB9zNSGr+lTx9L3kQ==","openid":"oJ1cB5cNfxOI2COElfzZiB-mUfuc","unionid":"owhCR0Yh6wTJwWWmeNihP4_7VEPU"}
                Util.Debuglog("j=" + j, "小程序登录2.txt");
                context.Response.Write("{\"code\":\"200\",\"db\":" + j + "}");
                return;
            }
            catch(Exception ex)
            {
                Util.Debuglog("ex=" + ex.Message, "小程序登录2.txt");
                context.Response.Write("{\"code\":\"500\",\"msg\":\"小程序登录异常，稍后重试\"}");
                return;
            }

        }
        public void getProductActivity2(HttpContext context)
        {
            string companyID = context.Request["companyID"].ToString();
            string sql = "select top 2 * from asm_activity where is_del=0 and statu=1 and companyID=" + companyID;
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                string str = "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    str += "订" + dt.Rows[i]["zq"].ToString() + "天" + dt.Rows[i]["activitytag"].ToString() + ",";
                }

                context.Response.Write(str.Substring(0, str.Length - 1));
            }
            else
            {
                context.Response.Write("");
            }
        }
        public void getProductActivity(HttpContext context)
        {
            try
            {
                string productID = context.Request["productID"].ToString();
                string mechineID = context.Request["mechineID"].ToString();
                string sql = "select ROW_NUMBER() OVER ( ORDER BY zq ) AS rowNum,a.* from asm_activity_detail d left join asm_activity  a on d.activityID=a.id where productID='" + productID + "' and  d.mechineID=" + mechineID + "  and a.statu=1 and a.is_del=0 order by zq";
                Util.Debuglog("sql=" + sql, "getProductActivity.txt");
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {

                    context.Response.Write(Util.DataTableToJsonWithJsonNet(dt));
                }
                else
                {
                    context.Response.Write("");
                }
            }
            catch
            {
                context.Response.Write("");
            }
        }

        public void getProduct(HttpContext context)
        {
            string productID = context.Request["productID"].ToString();
            string sql = "select * from asm_product where productID=" + productID;
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                context.Response.Write(Util.DataTableToJsonWithJsonNet(dt));
            }
            else
            {
                context.Response.Write("");
            }
        }
        /// <summary>
        /// 获取商品列表
        /// </summary>
        /// <param name="context"></param>
        public void getProductList(HttpContext context)
        {
            try
            {
                string companyID = context.Request["companyID"].ToString();//企业ID 
                string mechineID = context.Request["mechineID"].ToString();
                string type = context.Request["type"].ToString();
                if (string.IsNullOrEmpty(companyID)||string.IsNullOrEmpty(mechineID))
                {
                    context.Response.Write("");
                    return;
                }
                Util.Debuglog("companyID=" + companyID + ";type=" + type, "getProductList.txt");
                
                string sql = "";
                if (type == "0")
                {
                    sql = "select * from asm_product where companyID=" + companyID + " and dstype in(1,3) and is_del=0 and productID in(select acd.productID from [dbo].[asm_activity_detail] acd left JOIN asm_activity ac on acd.activityID=ac.id  where acd.mechineID="+mechineID+" AND ac.is_del=0)  order by weight desc";
                }
                else
                {
                    sql = "select * from asm_product where companyID=" + companyID + " and protype=" + type + " and dstype in(1,3) and is_del=0 and productID in(select acd.productID from [dbo].[asm_activity_detail] acd left JOIN asm_activity ac on acd.activityID=ac.id  where acd.mechineID="+mechineID+" AND ac.is_del=0)  order by weight desc";
                }
                Util.Debuglog("sql=" + sql, "getProductList.txt");
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    context.Response.Write(Util.DataTableToJsonWithJsonNet(dt));
                }
                else
                {
                    context.Response.Write("");
                }
            }
            catch {

            }
        }
        /// <summary>
        /// 获取商品列表
        /// </summary>
        /// <param name="context"></param>
        public void getType(HttpContext context)
        {
            string companyID = context.Request["companyID"].ToString();//企业ID 
            string sql = "select '0' productTypeID,'全部' typeName from asm_protype "
                         + "  union"
                         + "  select productTypeID,typeName from asm_protype ";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                context.Response.Write(Util.DataTableToJsonWithJsonNet(dt));
            }
            else
            {
                context.Response.Write("");
            }
        }
        /// <summary>
        /// 加载机器列表
        /// </summary>
        /// <param name="context"></param>
        public void getMechineList(HttpContext context)
        {
            string latitude = context.Request["latitude"].ToString();//纬度34.86426
            string longitude = context.Request["longitude"].ToString();//经度 117.55601
            string companyID = context.Request["companyID"].ToString();//企业ID
            Util.Debuglog("latitude=" + latitude + ";longitude=" + longitude + ";companyID=" + companyID, "距离差.txt");
            string sql = "select * from asm_mechine where companyID=" + companyID + " and zdX!='' and zdY!='' and openStatus=0";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            Dictionary<string, double> map = new Dictionary<string, double>();
            string idlist = "";
            if (dt.Rows.Count > 0)
            {
                string sqlC = "select * from asm_company where id=" + companyID;
                DataTable dc = DbHelperSQL.Query(sqlC).Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    double SZ = 900000000;
                    if (!string.IsNullOrEmpty(dc.Rows[0]["jlc"].ToString()))
                    {
                        SZ = double.Parse(dc.Rows[0]["jlc"].ToString());
                    }
                    double m = Util.GetDistance(double.Parse(latitude), double.Parse(longitude), double.Parse(dt.Rows[i]["zdY"].ToString()), double.Parse(dt.Rows[i]["zdX"].ToString()));
                    Util.Debuglog(i + "m=" + m, "距离差.txt");
                    if (m < SZ)
                    {
                        map.Add(dt.Rows[i]["id"].ToString(), m);
                        idlist += dt.Rows[i]["id"].ToString() + ",";
                    }
                }
                if (idlist.Length > 0)
                {
                    idlist = idlist.Substring(0, idlist.Length - 1);
                }
                string sql1 = "select *,'' xjlc from asm_mechine where id in(" + idlist + ")";
                Util.Debuglog("sql1=" + sql1, "距离差.txt");
                DataTable dt1 = DbHelperSQL.Query(sql1).Tables[0];
                if (dt1.Rows.Count > 0)
                {
                    for (int j = 0; j < dt1.Rows.Count; j++)
                    {
                        double n = 0;
                        map.TryGetValue(dt1.Rows[j]["id"].ToString(), out n);
                        dt1.Rows[j]["xjlc"] = (n / 1000).ToString("f2");
                    }
                }
                dt1.DefaultView.Sort = "xjlc asc";
                dt1 = dt1.DefaultView.ToTable();
                Util.Debuglog("m=" + Util.DataTableToJsonWithJsonNet(dt1), "距离差.txt");
                context.Response.Write(Util.DataTableToJsonWithJsonNet(dt1));
            }
            else
            {
                context.Response.Write("");
            }
        }
        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="context"></param>
        public void sendTemplateMsg(string data,string companyID)
        {
            try
            {
                string token = getMinAccessToken(Util.getMinAppid(companyID), Util.getMinSecret(companyID));
                string url = string.Format("https://api.weixin.qq.com/cgi-bin/message/wxopen/template/send?access_token={0}", token);
                Util.Debuglog("token=" + token, "sendTemplateMessage.txt");
                Util.Debuglog("url=" + url, "sendTemplateMessage.txt");
                HttpWebRequest hwr = WebRequest.Create(url) as HttpWebRequest;
                hwr.Method = "POST";
                hwr.ContentType = "application/x-www-form-urlencoded";
                byte[] payload;
                payload = System.Text.Encoding.UTF8.GetBytes(data); //通过UTF-8编码  
                hwr.ContentLength = payload.Length;
                Stream writer = hwr.GetRequestStream();
                writer.Write(payload, 0, payload.Length);
                writer.Close();
                var result = hwr.GetResponse() as HttpWebResponse; //此句是获得上面URl返回的数据  
                string strMsg = WebResponseGet(result);
                Util.Debuglog("发送结果strMsg="+ strMsg, "sendTemplateMessage.txt");
            }
            catch {

            }

        }

        /// <summary>
        /// 获取openID
        /// </summary>
        /// <param name="context"></param>
        public void getOpenID(HttpContext context)
        {
            string code = "";
            string companyID = "";
            try
            {
                code = context.Request["code"].ToString();
                companyID = context.Request["companyID"].ToString();
            }
            catch (Exception ex)
            {
                context.Response.Write(ex.ToString());
            }
            //向微信服务端 使用登录凭证 code 获取 session_key 和 openid   
            string url = "https://api.weixin.qq.com/sns/jscode2session?appid=" + Util.getMinAppid(companyID) + "&secret=" + Util.getMinSecret(companyID) + "&js_code=" + code + "&grant_type=" + grant_type;
            string type = "utf-8";

            GetUsersHelper GetUsersHelper = new GetUsersHelper();
            string j = GetUsersHelper.GetUrltoHtml(url, type);//获取微信服务器返回字符串  

            context.Response.Write(j);
        }
        /// <summary>
        /// 获取手机号
        /// </summary>
        /// <param name="context"></param>
        public void getPhoneNum(HttpContext context)
        {
            string code = "";
            string iv = "";
            string encryptedData = "";
            string companyID = "";
            try
            {
                code = context.Request["code"].ToString();
                iv = context.Request["iv"].ToString();
                encryptedData = context.Request["encryptedData"].ToString();
                companyID = context.Request["companyID"].ToString();
            }
            catch (Exception ex)
            {
                context.Response.Write(ex.ToString());
            }
            //向微信服务端 使用登录凭证 code 获取 session_key 和 openid   
            string url = "https://api.weixin.qq.com/sns/jscode2session?appid=" + Util.getMinAppid(companyID) + "&secret=" + Util.getMinSecret(companyID) + "&js_code=" + code + "&grant_type=" + grant_type;
            string type = "utf-8";

            GetUsersHelper GetUsersHelper = new GetUsersHelper();
            string j = GetUsersHelper.GetUrltoHtml(url, type);//获取微信服务器返回字符串  
            Util.Debuglog("j=" + j, "解密字符串.txt");
            //将字符串转换为json格式  
            JObject jo = (JObject)JsonConvert.DeserializeObject(j);
            result res = new result();
            try
            {
                //微信服务器验证成功  
                res.Openid = jo["openid"].ToString();
                res.Session_key = jo["session_key"].ToString();
            }
            catch (Exception)
            {
                //微信服务器验证失败  
                res.Errcode = jo["errcode"].ToString();
                res.Errmsg = jo["errmsg"].ToString();
            }
            if (!string.IsNullOrEmpty(res.Openid))
            {
                //用户数据解密  
                GetUsersHelper.AesIV = iv;
                GetUsersHelper.AesKey = res.Session_key;
                string result = GetUsersHelper.AESDecrypt(encryptedData);
                JObject numberObj = (JObject)JsonConvert.DeserializeObject(result);
                string number = numberObj["phoneNumber"].ToString();//手机号
                if (!string.IsNullOrEmpty(number)) {

                    
                    string sql = "select * from asm_member where minOpenID='" + res.Openid + "'";
                    DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["dj"].ToString() == "0")
                        {
                            
                                string update = "update asm_member set dj=1,phone='" + number + "' where  minOpenID='" + res.Openid + "'";
                                Util.Debuglog(update, "用户更新.txt");
                                DbHelperSQL.ExecuteSql(update);
                            
                        }
                        else
                        {
                            
                                string update = "update asm_member set phone='" + number + "' where minOpenID='" + res.Openid + "'";
                                Util.Debuglog(update, "用户更新.txt");
                                DbHelperSQL.ExecuteSql(update);
                            
                        }

                    }
                   
                }
                Util.Debuglog("result=" + result, "解密字符串.txt");
                //返回解密后的用户数据  
                context.Response.Write(result);
            }
            else
            {
                context.Response.Write(j);
            }
        }
        /// <summary>
        /// 解密会员信息 与获取手机号方法相同
        /// </summary>
        ///{"openId":"oJ1cB5cNfxOI2COElfzZiB-mUfuc",
        ///"nickName":"小爷不狂",
        ///"gender":1,
        ///"language":"zh_CN",
        ///"city":"枣庄",
        ///"province":"山东",
        ///"country":"中国",
        ///"avatarUrl":"https://wx.qlogo.cn/mmopen/vi_32/tRZ5lGPG9La2hUFz95eATeq0poA0Fx8yBMNu7WZH5RodqHKaWSicPnn7owVDX7YecCumaI2vgiadyvwicGkBdfpGA/132",
        ///"unionId":"owhCR0Yh6wTJwWWmeNihP4_7VEPU",
        ///"watermark":{"timestamp":1555724672,"appid":"wxcdb958e5f684f086"}}
        /// <param name="context"></param>
        public void getDecryptUserInfo(HttpContext context)
        {
            string code = "";
            string iv = "";
            string encryptedData = "";
            string companyID = "";
            string phone = "";//可为空 空说明用户拒绝获取手机号
            try
            {
                code = context.Request["code"].ToString();
                iv = context.Request["iv"].ToString();
                encryptedData = context.Request["encryptedData"].ToString();
                companyID = context.Request["companyID"].ToString();
                phone = context.Request["phone"].ToString();
            }
            catch (Exception ex)
            {
                context.Response.Write("{\"code\":\"500\",\"msg\":\"参数不全\"}");
                return;

            }
            //向微信服务端 使用登录凭证 code 获取 session_key 和 openid   
            string url = "https://api.weixin.qq.com/sns/jscode2session?appid=" + Util.getMinAppid(companyID) + "&secret=" + Util.getMinSecret(companyID) + "&js_code=" + code + "&grant_type=" + grant_type;
            string type = "utf-8";

            GetUsersHelper GetUsersHelper = new GetUsersHelper();
            string j = GetUsersHelper.GetUrltoHtml(url, type);//获取微信服务器返回字符串  
            Util.Debuglog("j=" + j, "解密字符串.txt");
            //将字符串转换为json格式  
            JObject jo = (JObject)JsonConvert.DeserializeObject(j);
            result res = new result();
            try
            {
                //微信服务器验证成功  
                res.Openid = jo["openid"].ToString();
                res.Session_key = jo["session_key"].ToString();
            }
            catch (Exception)
            {
                //微信服务器验证失败  
                res.Errcode = jo["errcode"].ToString();
                res.Errmsg = jo["errmsg"].ToString();
                context.Response.Write("{\"code\":\"500\",\"msg\":\"服务器验证失败\"}");
                return;
            }
            if (!string.IsNullOrEmpty(res.Openid))
            {
                //用户数据解密  
                GetUsersHelper.AesIV = iv;
                GetUsersHelper.AesKey = res.Session_key;
                string result = GetUsersHelper.AESDecrypt(encryptedData);
                Util.Debuglog("result=" + result, "解密字符串.txt");
                //返回解密后的用户数据  
                //注册会员
                JObject usrinfo = (JObject)JsonConvert.DeserializeObject(result);
                string nickname = usrinfo["nickName"].ToString().Replace("'","");
                string gender = usrinfo["gender"].ToString() == "1" ? "男" : "女";//1男2女
                string city = usrinfo["city"].ToString();
                string province = usrinfo["province"].ToString();
                string country = usrinfo["country"].ToString();
                string minOpenID = usrinfo["openId"].ToString();//小程序openID
                string unionID = usrinfo["unionId"].ToString();//
                string headImg = usrinfo["avatarUrl"].ToString();
                Util.Debuglog("phone="+phone+";companyID=" + companyID + ";nickname=" + nickname + ";city=" + city + ";province=" + province + ";minOpenID=" + minOpenID + ";unionID=" + unionID + ";gender=" + gender, "用户注册.txt");
                //根据unionid判断用户是否存在

                string sql = "select * from asm_member where unionID='" + unionID + "'";
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count <= 0)
                { 
                    if (string.IsNullOrEmpty(phone)|| phone== "undefined")
                    {
                        string insert = "insert into asm_member(name,province,city,country,AvailableMoney,sumConsume,sumRecharge,createDate,companyID,headurl,nickname,sex,unionID,minOpenID,consumeCount)"
                       + " values(N'" + nickname + "','" + province + "','" + city + "','" + country + "',0,0,0,'" + DateTime.Now + "','" + companyID + "','" + headImg + "',N'" + nickname + "','" + gender + "','" + unionID + "','" + minOpenID + "',0)";
                        Util.Debuglog(insert, "用户注册.txt");
                        DbHelperSQL.ExecuteSql(insert);
                    }
                    else
                    {

                        string insert = "insert into asm_member(dj,phone,name,province,city,country,AvailableMoney,sumConsume,sumRecharge,createDate,companyID,headurl,nickname,sex,unionID,minOpenID,consumeCount)"
                       + " values(1,'" + phone + "',N'" + nickname + "','" + province + "','" + city + "','" + country + "',0,0,0,'" + DateTime.Now + "','" + companyID + "','" + headImg + "',N'" + nickname + "','" + gender + "','" + unionID + "','" + minOpenID + "',0)";
                        Util.Debuglog(insert, "用户注册.txt");
                        DbHelperSQL.ExecuteSql(insert);
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(phone))
                    {
                        string update = "update asm_member set  nickname=N'" + nickname + "',province='" + province + "',city='" + city + "',country='" + country + "',headurl='" + headImg + "',sex='" + gender + "',name=N'" + nickname + "',minOpenID='"+minOpenID+"' where unionID='" + unionID + "'";
                        Util.Debuglog(update, "用户更新.txt");
                        DbHelperSQL.ExecuteSql(update);
                    }
                    else
                    {
                        if (dt.Rows[0]["dj"].ToString() == "0")
                        {
                            if (phone!= "undefined")
                            {
                                string update = "update asm_member set dj=1,phone='" + phone + "', nickname='" + nickname + "',province='" + province + "',city='" + city + "',country='" + country + "',headurl='" + headImg + "',sex='" + gender + "',name='" + nickname + "',minOpenID='" + minOpenID + "' where unionID='" + unionID + "'";
                                Util.Debuglog(update, "用户更新.txt");
                                DbHelperSQL.ExecuteSql(update);
                            }
                        }
                        else
                        {
                            if (phone!= "undefined")
                            {
                                string update = "update asm_member set phone='" + phone + "', nickname='" + nickname + "',province='" + province + "',city='" + city + "',country='" + country + "',headurl='" + headImg + "',sex='" + gender + "',name='" + nickname + "',minOpenID='" + minOpenID + "' where unionID='" + unionID + "'";
                                Util.Debuglog(update, "用户更新.txt");
                                DbHelperSQL.ExecuteSql(update);
                            }
                        }

                    }

                }
                sql = "select * from asm_member where unionID='" + unionID + "'";
                dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    context.Response.Write("{\"code\":\"200\",\"db\":" + result + ",\"userInfo\":" + OperUtil.DataTableToJsonWithJsonNet(dt) + "}");
                    return;
                }
                else
                {
                    context.Response.Write("{\"code\":\"400\",\"msg\":\"会员信息读取失败\"}");
                    return;
                }
            }
            else
            {
                context.Response.Write("{\"code\":\"500\",\"msg\":\"字符串解密错误\"}");
                return;
            }
        }
        /// <summary>
        /// 获取广告列表
        /// </summary>
        /// <param name="context"></param>
        public void getAdverseList(HttpContext context)
        {
            try
            {
                string companyID = context.Request["companyID"].ToString();
                string sql = "SELECT * from  asm_zfbhb where companyID=" + companyID + " and startTime is not null and endTime is not null and status=0 and type=1";
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                string sql2 = "SELECT * from  asm_zfbhb where companyID=" + companyID + " and startTime is not null and endTime is not null and status=0 and type=2";
                DataTable dt2 = DbHelperSQL.Query(sql2).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    context.Response.Write("{\"code\":\"200\",\"db\":" + OperUtil.DataTableToJsonWithJsonNet(dt) + ",\"db2\":" + OperUtil.DataTableToJsonWithJsonNet(dt2) + "}");
                    return;
                }
                context.Response.Write("{\"code\":\"300\",\"msg\":\"暂无上传广告\"}");
                return;
            }
            catch
            {
                context.Response.Write("{\"code\":\"500\",\"msg\":\"系统异常\"}");
                return;
            }
        }
        /// <summary>
        /// 获取轮播图列表
        /// </summary>
        /// <param name="context"></param>
        public void getLBTList(HttpContext context)
        {
            try
            {
                string companyID = context.Request["companyID"].ToString();
                string sql = "select * from asm_company where id=" + companyID;
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    context.Response.Write("{\"code\":\"200\",\"db\":" + OperUtil.DataTableToJsonWithJsonNet(dt) + "}");
                    return;
                }
                context.Response.Write("{\"code\":\"300\",\"msg\":\"暂无上传广告\"}");
                return;
            }
            catch
            {
                context.Response.Write("{\"code\":\"500\",\"msg\":\"系统异常\"}");
                return;
            }
        }
        public string AES_decrypt(string encryptedDataStr, string key, string iv)
        {
            RijndaelManaged rijalg = new RijndaelManaged();
            //-----------------    
            //设置 cipher 格式 AES-128-CBC    
            rijalg.KeySize = 128;
            rijalg.Padding = PaddingMode.PKCS7;
            rijalg.Mode = CipherMode.CBC;
            rijalg.Key = Convert.FromBase64String(key);
            rijalg.IV = Convert.FromBase64String(iv);
            byte[] encryptedData = Convert.FromBase64String(encryptedDataStr);
            //解密    
            ICryptoTransform decryptor = rijalg.CreateDecryptor(rijalg.Key, rijalg.IV);
            string result;

            using (MemoryStream msDecrypt = new MemoryStream(encryptedData))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {

                        result = srDecrypt.ReadToEnd();
                    }
                }
            }

            return result;
        }
        public string getMinAccessToken(string appid, string APPSECRET)
        {
            string access_token = RedisHelper.GetRedisModel<string>(appid);
            if (string.IsNullOrEmpty(access_token))
            {
                string Str = GetJson("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + appid + "&secret=" + APPSECRET);
                JObject jo = (JObject)JsonConvert.DeserializeObject(Str);
                access_token = jo["access_token"].ToString();
                RedisHelper.SetRedisModel("appid", access_token, new TimeSpan(1, 50, 0));
            }

            return access_token;
        }
        protected string GetJson(string url)
        {
            WebClient wc = new WebClient();
            wc.Credentials = CredentialCache.DefaultCredentials;
            wc.Encoding = Encoding.UTF8;
            string returnText = wc.DownloadString(url);

            if (returnText.Contains("errcode"))
            {

            }
            return returnText;
        }
        public class JsonHelper
        {

            public static string GetJson<T>(T obj)
            {
                DataContractJsonSerializer json = new DataContractJsonSerializer(obj.GetType());
                using (MemoryStream stream = new MemoryStream())
                {
                    json.WriteObject(stream, obj);
                    string szJson = Encoding.UTF8.GetString(stream.ToArray()); return szJson;
                }
            }

            public static T ParseFromJson<T>(string szJson)
            {
                T obj = Activator.CreateInstance<T>();
                using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(szJson)))
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
                    return (T)serializer.ReadObject(ms);
                }
            }
        }
        //处理时间
        public static string delTime(string time)
        {
            string[] timeArr = time.Split('-');
            if (timeArr.Length == 3)
            {
                timeArr[1] = timeArr[1].PadLeft(2, '0');
                timeArr[2] = timeArr[2].PadLeft(2, '0');
                return timeArr[0] + "-" + timeArr[1] + "-" + timeArr[2];
            }
            else
            {
                return time;
            }

        }
        public static string WebResponseGet(HttpWebResponse webResponse)
        {
            StreamReader responseReader = null;
            string responseData = "";
            try
            {
                responseReader = new StreamReader(webResponse.GetResponseStream());
                responseData = responseReader.ReadToEnd();
            }
            catch
            {
                throw;
            }
            finally
            {
                webResponse.GetResponseStream().Close();
                responseReader.Close();
                responseReader = null;
            }
            return responseData;
        }
        public bool IsReusable
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        public static string sendSMS(string member_phone, string yzm)
        {
            if (string.IsNullOrEmpty(member_phone))
            {
                return "1";//手机号不能为空
            }
            if (!Regex.IsMatch(member_phone, @"^1\d{10}$"))//判断手机号
            {

                return "2";//手机号不正确
            }
            if (Util.CheckServeStatus("sms.ruizhiwei.net"))
            {

                String registration = "您的验证码是：{yzm}，请不要把验证码泄漏给其他人。【冰格售卖】";
                //registration = registration.Replace("@SMSign", SMSign);
                registration = registration.Replace("{yzm}", yzm);  //卡号
                Hashtable ht = new Hashtable();
                ht.Add("U", "bingge");
                ht.Add("p", DESEncrypt.MD5Encrypt("123456"));
                ht.Add("N", member_phone);
                ht.Add("M", registration);
                ht.Add("T", "2");
                XmlDocument xmlDoc = WZHY.Common.WebService.QueryGetWebService("http://sms.ruizhiwei.cn/API/SMS_Send.asmx", "SendMes", ht);
                String status = xmlDoc.InnerText;
                RedisHelper.SetRedisModel(member_phone, yzm, new TimeSpan(0, 5, 0));
                return "4";//发送成功
            }
            else
            {
                return "3";//服务器异常
            }

        }
    }
}