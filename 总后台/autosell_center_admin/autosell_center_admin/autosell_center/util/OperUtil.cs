using DBUtility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace autosell_center.util
{
    public class OperUtil
    {

        /// <summary>
        /// 将字典类型序列化为json字符串
        /// </summary>
        /// <typeparam name="TKey">字典key</typeparam>
        /// <typeparam name="TValue">字典value</typeparam>
        /// <param name="dict">要序列化的字典数据</param>
        /// <returns>json字符串</returns>
        public static string SerializeDictionaryToJsonString<TKey, TValue>(Dictionary<TKey, TValue> dict)
        {
            if (dict.Count == 0)
                return "";

            string jsonStr = JsonConvert.SerializeObject(dict);
            return jsonStr;
        }

        public static string DataTableToJsonWithJsonNet(DataTable table)
        {
            string jsonString = string.Empty;
            jsonString = JsonConvert.SerializeObject(table, Newtonsoft.Json.Formatting.None);
            return jsonString;
        }
        public static void Add(string strSessionName, string strValue)
        {
            HttpContext.Current.Session[strSessionName] = strValue;
            HttpContext.Current.Session.Timeout = 30;
        }
        /// <summary>
        /// 读取某个Session对象值
        /// </summary>
        /// <param name="strSessionName">Session对象名称</param>
        /// <returns>Session对象值</returns>
        public static string Get(string strSessionName)
        {
            if (HttpContext.Current.Session[strSessionName] == null)
            {
                return null;
            }
            return HttpContext.Current.Session[strSessionName].ToString();
        }

        public static void setCooki(string key, string val)
        {
            System.Web.HttpCookie httpCookie = new System.Web.HttpCookie(key);
            httpCookie.Value = val;
            httpCookie.Expires = System.DateTime.Now.AddYears(1);
            System.Web.HttpContext.Current.Response.Cookies.Add(httpCookie);

        }
        public static string getCooki(string key)
        {
            HttpCookie httpCookie = HttpContext.Current.Request.Cookies.Get(key);
            string result;
            if (httpCookie == null || string.IsNullOrEmpty(httpCookie.Value))
            {
                result = "0";
            }
            else
            {
                result = httpCookie.Value;
            }
            return result;
        }
        //获取微信消息模板的ID
        public static string getMessageID(string companyID, string templateBH)
        {
            string sql = "select * from [dbo].[asm_wxTemplate] where companyID=" + companyID + " and templateBH='" + templateBH + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["templateID"].ToString();
            }
            return "0";
        }
    }
}