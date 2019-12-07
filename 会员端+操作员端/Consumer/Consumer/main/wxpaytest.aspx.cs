using Consumer.cls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Consumer.main
{
    public partial class wxpaytest : System.Web.UI.Page
    {
        public string wxJsApiParam;
        protected void Page_Load(object sender, EventArgs e)
        {
            //app_secret  f00ca380481727f4c50293ba6a121c3c
            Random rand = new Random();
            PayData pd = new PayData();
            pd.SetValue("app_id", "wx5fa5622ada06cfe3");
            pd.SetValue("nonce_str", GetTimestamp());
            pd.SetValue("version","1.0");
            pd.SetValue("timestamp", GetTimestamp());
            pd.SetValue("mch_id","9");
            pd.SetValue("title","测试");
            pd.SetValue("total_fee","1");
            pd.SetValue("out_trade_no", GetTimestamp()+"CS"+rand.Next(100, 999));
            pd.SetValue("channel", "pay.weixin.jspay");
            pd.SetValue("openid", "ogqHzt8Yc0agDQzqBjYN2Vnw_7Zs");
            pd.SetValue("app_secret", "f00ca380481727f4c50293ba6a121c3c");

            pd.SetValue("sign", makeSignForQrCodePay(pd));
        }
        public static double GetTimestamp()
        {
            DateTime d = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            TimeSpan ts = d.ToUniversalTime() - new DateTime(1970, 1, 1);
            return ts.TotalMilliseconds/1000;     //精确到毫秒
        }
        //生成签名--支付
        public static string makeSignForQrCodePay(PayData pd)
        {
            Dictionary<string, string> dics = new Dictionary<string, string>();
            //1-赋值公共参数
            dics.Add("app_id", pd.GetValue("app_id").ToString());//所给的appId
            dics.Add("nonce_str", pd.GetValue("nonce_str").ToString());
            dics.Add("version", pd.GetValue("version").ToString());
            dics.Add("timestamp", pd.GetValue("timestamp").ToString());
            dics.Add("mch_id", pd.GetValue("mch_id").ToString());
            //2-赋值支付参数
            dics.Add("title", pd.GetValue("title").ToString());
            dics.Add("total_fee", pd.GetValue("total_fee").ToString());
            dics.Add("out_trade_no", pd.GetValue("out_trade_no").ToString());
            dics.Add("channel", pd.GetValue("channel").ToString());
            dics.Add("openid", pd.GetValue("openid").ToString());
            //3-ASCII排序后的字符串  
            var signStr = getParamSrc(dics);
            string API = signStr + "&key=" + pd.GetValue("app_secret") ;
            string signMd5 = MD5(API);
            return signMd5;
        }
        //ASCII码排序
        public static string getParamSrc(Dictionary<string, string> paramsMap)
        {
            var vDic = (from objDic in paramsMap orderby objDic.Key ascending select objDic);
            StringBuilder str = new StringBuilder();
            foreach (KeyValuePair<string, string> kv in vDic)
            {
                string pkey = kv.Key;
                string pvalue = kv.Value;
                str.Append(pkey + "=" + pvalue + "&");
            }


            String result = str.ToString().Substring(0, str.ToString().Length - 1);
            return result;
        }
        // MD5加密
        public static string MD5(string strSource)
        {
            string Md5Str = "";
            // new
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();

            //获取密文字节数组
            byte[] bytResult = md5.ComputeHash(System.Text.Encoding.GetEncoding("utf-8").GetBytes(strSource));

            //转换成字符串，并取9到25位 
            //string strResult = BitConverter.ToString(bytResult, 4, 8);  
            //转换成字符串，32位 

            string strResult = BitConverter.ToString(bytResult);

            //BitConverter转换出来的字符串会在每个字符中间产生一个分隔符，需要去除掉 
            strResult = strResult.Replace("-", "");
            return strResult;
        }
    }
}