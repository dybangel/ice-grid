using autosell_center.util;
using DBUtility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Script.Serialization;
using Consumer.cls;

namespace autosell_center.util
{
    public static class Util
    {
          public static void Debuglog(string log, string logname = "_Debuglog.txt")
        {
            try
            {
                System.IO.StreamWriter writer = System.IO.File.AppendText(HttpRuntime.AppDomainAppPath.ToString() + "log/" + (DateTime.Now.ToString("yyyyMMdd") + logname));
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberID"></param>
        /// <param name="money"></param>
        /// <param name="bz"></param>
        /// <param name="description"></param>
        /// <param name="payType">1 会员充值 2 会员消费 3会员转账收入 4转账支出 5 售卖</param>
        public static void moneyChange(string memberID, string money, string avaMoney, string bz, string type, string skID, string description)
        {
            //string sql1 = "insert into asm_moneyChange(payTime,money,AvaiilabMOney,memberID,type,bz,skID) values('" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," + money + "," + avaMoney + "," + memberID + ","+type+",'"+bz+"','"+skID+"')";
            //DbHelperSQL.ExecuteSql(sql1);

            string sql = "insert into asm_chgMoney(memberID,payTime,type,money,bz,description) values('" + memberID + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + type + "','" + money + "','" + bz + "','" + description + "')";
            DbHelperSQL.ExecuteSql(sql);

        }
        public static void insertNotice(string memberID, string title, string con)
        {
            string sql = "insert into asm_notice (memberID,title,con,time) values('" + memberID + "','" + title + "','" + con + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
            DbHelperSQL.ExecuteSql(sql);

        }
        public static void ClearRedisProductInfoByMechineID(string mechineID)
        {
            RedisHelper.SetRedisModel<string>(mechineID + "_productInfo", "", new TimeSpan(0, 0, 0));
        }
        public static void ClearRedisVideoByMechineID(string mechineID)
        {
            RedisHelper.SetRedisModel<string>(mechineID + "_videoInfo", "", new TimeSpan(0, 0, 0));
        }
        public static string getCompany(string companyID)
        {
            string result = RedisHelper.GetRedisModel<string>(companyID + "_companyInfo");
            if (string.IsNullOrEmpty(result))
            {
                string sql = "select * from asm_company where id=" + companyID;
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                result = OperUtil.DataTableToJsonWithJsonNet(dt);
                RedisHelper.SetRedisModel<string>(companyID + "_companyInfo", result, new TimeSpan(2, 0, 0));
            }
            return result;
        }
        public static async System.Threading.Tasks.Task addMechineToList(string mechineID)
        {
            try
            {
                string url = "http://alisocket.bingoseller.com/api/api.ashx";
                using (var client = new WebClient())
                {
                    var values = new NameValueCollection();
                    values["action"] = "addMechineToList";

                    values["mechineID"] = mechineID;//支付宝订单号
                    var response = client.UploadValues(url, values);
                    var responseString = Encoding.Default.GetString(response);
                   
                }
            }
            catch (Exception e)
            {
               
            }
        }
    }
}