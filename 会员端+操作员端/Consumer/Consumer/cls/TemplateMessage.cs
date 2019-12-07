using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace Consumer.cls
{
    public class TemplateMessage
    {   
        /// <summary>  
        /// 发送模板消息  
        /// </summary>  
        /// <param name="accessToken">AccessToken</param>  
        /// <param name="data">发送的模板数据</param>  
        /// <returns></returns>  
        public static string SendTemplateMsg(string accessToken, string data)
        {
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={0}", accessToken);
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
            Util.Debuglog(data+";strMsg=" + strMsg, "公众号SendTemplateMsg.txt");
            return strMsg;
        }
        
        public static string GetJson(string url)
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
        /// <summary>
        /// 会员注册  OPENTM203347141 
        /// </summary>kRrjmZhPvWeWqblT0kIR4HNgrXJIKBhU61URKiX8PfA
        /// <param name="openID">接收者openid</param>
        /// <param name="templateID">模板id</param>
        /// <param name="title">标题</param>
        /// <param name="nickName">昵称</param>
        /// <param name="remark">备注</param>
        /// <returns></returns>
        public static string Member_ZC(string openID,string templateID,string title,string nickName,string remark)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"touser\":\""+openID+"\",");
            sb.Append("\"template_id\":\""+templateID+"\",");
           // sb.Append("\"url\":\"http://www.baidu.com\",");
            sb.Append("\"data\":{");
            sb.Append("\"first\":{\"value\":\""+title+"\",\"color\":\"#173177\"},");
            sb.Append("\"keyword1\":{\"value\":\""+ nickName + "\",\"color\":\"#173177\"},");
            sb.Append("\"keyword2\":{\"value\":\""+DateTime.Now.ToString("yyyy年MM月dd日 HH:mm")+"\",\"color\":\"#173177\"},");
            sb.Append("\"remark\":{\"value\":\""+remark+"\",\"color\":\"#173177\"}}}");
            return sb.ToString();
        }
        /// <summary>
        /// 售货机商品支付成功通知  OPENTM401313503
        /// </summary>54Ngl16JFx0hK_vuiTZBT_16jxSKmhGfA6i0NtFn8Cs
        /// <param name="openID"></param>
        /// <param name="templateID"></param>
        /// <param name="title"></param>
        /// <param name="productName"></param>
        /// <param name="price"></param>
        /// <param name="orderNO"></param>
        /// <param name="phone"></param>
        /// <param name="remark"></param>可以传订单周期和取货时间
        /// <returns></returns>

        public static string comsume(string openID,string templateID,string title,string productName,string price,string orderNO,string mechineBH,string remark)
        {

            string url = "";
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"touser\":\"" + openID + "\",");
            sb.Append("\"template_id\":\"" + templateID + "\",");
            sb.Append("\"url\":\""+url+"\",");
            sb.Append("\"data\":{");
            sb.Append("\"first\":{\"value\":\"" + title + "\",\"color\":\"#173177\"},");
            sb.Append("\"keyword1\":{\"value\":\"" + productName + "\",\"color\":\"#173177\"},");
            sb.Append("\"keyword2\":{\"value\":\"" + price + "\",\"color\":\"#173177\"},");
            sb.Append("\"keyword3\":{\"value\":\"" + orderNO + "\",\"color\":\"#173177\"},");
            sb.Append("\"keyword4\":{\"value\":\""+mechineBH+"\",\"color\":\"#173177\"},");
            sb.Append("\"keyword5\":{\"value\":\"每日请在会员端首页查看\",\"color\":\"#173177\"},");
            sb.Append("\"remark\":{\"value\":\"" +remark+ "\",\"color\":\"#173177\"}}}");
            return sb.ToString();
        }
        /// <summary>
        /// 售货机取货失败通知	OPENTM414811026
        /// </summary>FQUCA4TcfzER81c8HmohkwOAWyLBb6-NlbPVYX482ZU
        /// <param name="openID"></param>
        /// <param name="templateID"></param>
        /// <param name="productName"></param>
        /// <param name="mechineName"></param>
        /// <param name="ldNO"></param>
        /// <param name="errType"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public static string ch_error(string openID, string templateID,  string productName, string mechineName, string ldNO, string errType, string remark)
        {
            string url = "";
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"touser\":\"" + openID + "\",");
            sb.Append("\"template_id\":\"" + templateID + "\",");
            sb.Append("\"url\":\""+url+"\",");
            sb.Append("\"data\":{");
            sb.Append("\"first\":{\"value\":\"" + productName + "\",\"color\":\"#173177\"},");
            sb.Append("\"keyword1\":{\"value\":\"" + mechineName + "\",\"color\":\"#173177\"},");
            sb.Append("\"keyword2\":{\"value\":\"" + ldNO + "\",\"color\":\"#173177\"},");
            sb.Append("\"keyword3\":{\"value\":\"" + errType + "\",\"color\":\"#173177\"},");
            sb.Append("\"keyword4\":{\"value\":\"1\",\"color\":\"#173177\"},");
            sb.Append("\"remark\":{\"value\":\"" + remark + "\",\"color\":\"#173177\"}}}");
            return sb.ToString();
        }

        /// <summary>
        /// 余额变动提醒  OPENTM403148135  用于会员用钱包支付
        /// </summary>IcZgKQ09LQdAlqG4XzrxDQ5daeF5Zg1E2yaXTlDD2Kk
        /// <param name="openID"></param>
        /// <param name="templateID"></param>
        /// <param name="title"></param>
        /// <param name="bdLX"></param>变动类型 例如消费打款
        /// <param name="price"></param>
        /// <param name="totalMoney"></param>
        /// <param name="md"></param>门店 可以传机器
        /// <param name="remark"></param>
        /// <returns></returns>
        public static string money_bd(string openID, string templateID,string title, string bdLX, string price,string totalMoney, string md, string remark)
        {
            string url ="";
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"touser\":\"" + openID + "\",");
            sb.Append("\"template_id\":\"" + templateID + "\",");
            sb.Append("\"url\":\""+url+"\",");
            sb.Append("\"data\":{");
            sb.Append("\"first\":{\"value\":\"" + title + "\",\"color\":\"#173177\"},");
            sb.Append("\"keyword1\":{\"value\":\"" + bdLX + "\",\"color\":\"#173177\"},");
            sb.Append("\"keyword2\":{\"value\":\"" + price + "\",\"color\":\"#173177\"},");
            sb.Append("\"keyword3\":{\"value\":\"" + totalMoney + "\",\"color\":\"#173177\"},");
            sb.Append("\"keyword4\":{\"value\":\""+md+"\",\"color\":\"#173177\"},");
            sb.Append("\"keyword5\":{\"value\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\",\"color\":\"#173177\"},");
            sb.Append("\"remark\":{\"value\":\"" + remark + "\",\"color\":\"#173177\"}}}");
            return sb.ToString();
        }
        /// <summary>
        /// 充值成功通知 OPENTM410481462
        /// </summary>Tmin60E6DJtBO962B_5BEzVRC3Rbdv1JrKQNuzoY0Gw
        /// <param name="openID"></param>
        /// <param name="templateID"></param>
        /// <param name="title"></param>
        /// <param name="price1"></param>到账金额
        /// <param name="price2"></param>账户余额
        /// <param name="remark"></param>
        /// <returns></returns>
        public static string success_cz(string openID, string templateID, string title,string price1,string price2,string remark)
        {
            string url ="";
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"touser\":\"" + openID + "\",");
            sb.Append("\"template_id\":\"" + templateID + "\",");
            sb.Append("\"url\":\""+url+"\",");
            sb.Append("\"data\":{");
            sb.Append("\"first\":{\"value\":\"" + title + "\",\"color\":\"#173177\"},");
            sb.Append("\"keyword1\":{\"value\":\"" + price1 + "\",\"color\":\"#173177\"},");
            sb.Append("\"keyword2\":{\"value\":\"" + price2 + "\",\"color\":\"#173177\"},");
            sb.Append("\"remark\":{\"value\":\"" + remark + "\",\"color\":\"#173177\"}}}");
            return sb.ToString();
        }
        /// <summary>
        ///  OPENTM406259604  密码重置通知 
        /// </summary>jrGY-AhYtG-17HCl3Oz8-myZ6lpAsWscPIWwd60dTUA
        /// <param name="openID"></param>
        /// <param name="templateID"></param>
        /// <param name="title"></param> 尊敬的会员，您通过PC端修改了【登陆密码】
        /// <param name="bh"></param>会员编号：10000055
        /// <param name="name"></param>会员姓名：章子怡
        /// <param name="newpwd"></param>重置密码：1006201
        /// <returns></returns>
        public static string modify_pwd(string openID, string templateID,string title, string bh, string name, string newpwd)
        {
            string url = "";
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"touser\":\"" + openID + "\",");
            sb.Append("\"template_id\":\"" + templateID + "\",");
            sb.Append("\"url\":\""+url+"\",");
            sb.Append("\"data\":{");
            sb.Append("\"first\":{\"value\":\"" + title + "\",\"color\":\"#173177\"},");
            sb.Append("\"keyword1\":{\"value\":\"" + bh + "\",\"color\":\"#173177\"},");
            sb.Append("\"keyword2\":{\"value\":\"" + name + "\",\"color\":\"#173177\"},");
            sb.Append("\"keyword3\":{\"value\":\"" + newpwd + "\",\"color\":\"#173177\"},");
            sb.Append("\"keyword4\":{\"value\":\"" + DateTime.Now.ToString() + "\",\"color\":\"#173177\"},");          
            sb.Append("\"remark\":{\"value\":\"请认真保管自己的密码\",\"color\":\"#173177\"}}}");
            return sb.ToString();
        }
        /// <summary>
        /// 取货通知
        /// </summary>
        /// <param name="openID"></param>
        /// <param name="templateID"></param>
        /// <param name="title"></param>
        /// <param name="orderNO">订单编号</param>
        /// <param name="bh">取货分店</param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public static string getProduct(string openID, string templateID, string title, string mechineName, string address, string remark)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"touser\":\"" + openID + "\",");
            sb.Append("\"template_id\":\"" + templateID + "\",");
            sb.Append("\"url\":\"\",");
            sb.Append("\"data\":{");
            sb.Append("\"first\":{\"value\":\"" + title + "\",\"color\":\"#173177\"},");
            sb.Append("\"keyword1\":{\"value\":\"" + mechineName + "\",\"color\":\"#173177\"},");
            sb.Append("\"keyword2\":{\"value\":\"" + address + "\",\"color\":\"#173177\"},");
            sb.Append("\"remark\":{\"value\":\"" + remark + "\",\"color\":\"#173177\"}}}");
            return sb.ToString();
        }
        /// <summary>
        /// 会员等级变更
        /// </summary>YN6Z7mVieotApjpOuiymDIXUpqb_v10g_CkmyB9VM_s
        /// <param name="openID"></param>
        /// <param name="templateID"></param>
        /// <param name="title"></param>
        /// <param name="oldDJName"></param>
        /// <param name="newDJName"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public static string getDJChange(string openID, string templateID, string title, string oldDJName, string newDJName, string remark)
        {
            
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"touser\":\"" + openID + "\",");
            sb.Append("\"template_id\":\"" + templateID + "\",");
            sb.Append("\"url\":\"\",");
            sb.Append("\"data\":{");
            sb.Append("\"first\":{\"value\":\"" + title + "\",\"color\":\"#173177\"},");
            sb.Append("\"keyword1\":{\"value\":\"" + getName(oldDJName) + "\",\"color\":\"#173177\"},");
            sb.Append("\"keyword2\":{\"value\":\"" + getName(newDJName) + "\",\"color\":\"#173177\"},");
            sb.Append("\"keyword3\":{\"value\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "\",\"color\":\"#173177\"},");
            sb.Append("\"remark\":{\"value\":\"" + remark + "\",\"color\":\"#173177\"}}}");
            return sb.ToString();
        }
        public static string getName(string dj)
        {
            if (dj == "0")
            {
                return "游客";
            } else if (dj == "1")
            {
                return "普通会员";
            } else if (dj == "2")
            {
                return "白银会员";
            } else if (dj=="3")
            {
                return "黄金会员";
            }
            return "游客";

        }
        /// <summary>
        /// 积分转账
        /// </summary>X9YiKlxgDCXwJZRE1y70zFWV4VeUtkm1btK_lCPTJmg
        /// <param name="openID"></param>
        /// <param name="templateID"></param>
        /// <param name="title"></param>
        /// <param name="point"></param>
        /// <param name="bz"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public static string getTransf(string openID, string templateID, string title, string point, string bz, string remark)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"touser\":\"" + openID + "\",");
            sb.Append("\"template_id\":\"" + templateID + "\",");
            sb.Append("\"url\":\"\",");
            sb.Append("\"data\":{");
            sb.Append("\"first\":{\"value\":\"" + title + "\",\"color\":\"#173177\"},");
            sb.Append("\"keyword1\":{\"value\":\"" + point + "\",\"color\":\"#173177\"},");
            sb.Append("\"keyword2\":{\"value\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "\",\"color\":\"#173177\"},");
            sb.Append("\"keyword3\":{\"value\":\"" + bz + "\",\"color\":\"#173177\"},");
            sb.Append("\"remark\":{\"value\":\"" + remark + "\",\"color\":\"#173177\"}}}");
            return sb.ToString();
        }
        /// <summary>
        /// 中奖通知
        /// </summary>
        /// <param name="openID"></param>
        /// <param name="templateID"></param>
        /// <param name="title"></param>
        /// <param name="activity"></param>
        /// <param name="prize"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public static string getPrize(string openID, string templateID, string title, string activity, string prize, string remark)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"touser\":\"" + openID + "\",");
            sb.Append("\"template_id\":\"" + templateID + "\",");
            sb.Append("\"url\":\"\",");
            sb.Append("\"data\":{");
            sb.Append("\"first\":{\"value\":\"" + title + "\",\"color\":\"#173177\"},");
            sb.Append("\"keyword1\":{\"value\":\"" + activity + "\",\"color\":\"#173177\"},");
            sb.Append("\"keyword2\":{\"value\":\"" + prize + "\",\"color\":\"#173177\"},");
            sb.Append("\"keyword3\":{\"value\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\",\"color\":\"#173177\"},");
            sb.Append("\"remark\":{\"value\":\"" + remark + "\",\"color\":\"#173177\"}}}");
            return sb.ToString();
        }
        public static string tk(string openID, string templateID, string title, string money, string remark)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"touser\":\"" + openID + "\",");
            sb.Append("\"template_id\":\"" + templateID + "\",");
            sb.Append("\"url\":\"\",");
            sb.Append("\"data\":{");
            sb.Append("\"first\":{\"value\":\"" + title + "\",\"color\":\"#173177\"},");
            sb.Append("\"keyword1\":{\"value\":\"" + money + "\",\"color\":\"#173177\"},");
            sb.Append("\"keyword2\":{\"value\":\"" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "\",\"color\":\"#173177\"},");
            sb.Append("\"remark\":{\"value\":\"" + remark + "\",\"color\":\"#173177\"}}}");
            return sb.ToString();
        }
    }
}