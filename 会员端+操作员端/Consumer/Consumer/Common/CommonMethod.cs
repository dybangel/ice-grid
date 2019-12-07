using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml;
using Tencent;

namespace OpenPlatForm.Common
{
    public static class CommonMethod
    {
        /**
        * 生成随机串，随机串包含字母或数字
        * @return 随机串
        */
        public static string GenerateNonceStr()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
        /// <summary>  
        /// 获取时间戳  
        /// </summary>  
        /// <returns></returns>  
        public static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        #region 解析Json
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
        #endregion

        #region Json解析后的类
        //componentToken对象
        public class Token
        {
            private string _component_access_token;
            private string _expires_in;

            public string component_access_token
            {
                get
                {
                    return _component_access_token;
                }

                set
                {
                    _component_access_token = value;
                }
            }

            public string expires_in
            {
                get
                {
                    return _expires_in;
                }

                set
                {
                    _expires_in = value;
                }
            }
        }
        //预授权码对象
        public class Pre_Auth_Code
        {
            private string _pre_auth_code;
            private string _expires_in;

            public string pre_auth_code
            {
                get
                {
                    return _pre_auth_code;
                }

                set
                {
                    _pre_auth_code = value;
                }
            }

            public string expires_in
            {
                get
                {
                    return _expires_in;
                }

                set
                {
                    _expires_in = value;
                }
            }
        }
        #region 授权公众号简要信息（带authorizer_access_token）
        public class funcscope_category
        {
            public string id { get; set; }
        }
        public class func_info
        {
            public funcscope_category funcscope_category { get; set; }
        }
        public class authorization_info
        {
            public string authorizer_appid { get; set; }
            public string authorizer_access_token { get; set; }
            public string expires_in { get; set; }
            public string authorizer_refresh_token { get; set; }
            public List<func_info> func_info { get; set; }
        }
        public class RootObject
        {
            public authorization_info authorization_info { get; set; }
        }
        #endregion

        #region 授权公众号详细信息
        public class service_type_info
        {
            public int id { get; set; }
        }

        public class verify_type_info
        {
            public int id { get; set; }
        }

        public class business_info
        {
            public int open_store { get; set; }
            public int open_scan { get; set; }
            public int open_pay { get; set; }
            public int open_card { get; set; }
            public int open_shake { get; set; }
        }

        public class authorizer_info
        {
            public string nick_name { get; set; }
            public string head_img { get; set; }
            public service_type_info service_type_info { get; set; }
            public verify_type_info verify_type_info { get; set; }
            public string user_name { get; set; }
            public string principal_name { get; set; }
            public business_info business_info { get; set; }
            public string alias { get; set; }
            public string qrcode_url { get; set; }
        }
        

        public class func_infoItem
        {
            public funcscope_category funcscope_category { get; set; }
        }

        //public class authorization_info
        //{
        //    public string authorization_appid { get; set; }
        //    public List<func_infoItem> func_info { get; set; }
        //}

        public class RootObjectDetail
        {
            public authorizer_info authorizer_info { get; set; }
            public authorization_info authorization_info { get; set; }
        }
        #endregion

        public class RefreshToken
        {
            public string authorizer_access_token { get; set; }
            public string expires_in { get; set; }
            public string authorizer_refresh_token { get; set; }
        }
        #endregion
    }
}