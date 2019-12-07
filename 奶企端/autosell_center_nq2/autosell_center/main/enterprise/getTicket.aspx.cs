using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenPlatForm.Common;
using System.Data;
using DBUtility;
using Consumer.cls;
using System.Data.SqlClient;

namespace OpenPlatForm.api
{
    public partial class getTicket : System.Web.UI.Page
    {
        public DataTable DT = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
          
            string sql = "select * from asm_platformInfo";
            DT = DbHelperSQL.Query(sql).Tables[0];
            #region 获取并解析ticket
            //log4net.ILog log = log4net.LogManager.GetLogger("testApp.Logging");//获取一个日志记录器
            //log.Info(DateTime.Now.ToString() + ":" + "*************进来了*************");//写入一条新log
            //1-获取回传加密数据、签名字符串、时间戳、随机字符串
            byte[] data = Request.BinaryRead(Request.TotalBytes);
            string postData = Encoding.Default.GetString(data);
            string sReqMsgSig = Request.QueryString["msg_signature"];//签名字符串encrypt_type
            string sReqTimeStamp = Request.QueryString["timestamp"];//时间戳
            string sReqNonce = Request.QueryString["nonce"];//获取随机字符串
            Util.Debuglog("postData="+ postData+ ";sReqMsgSig="+ sReqMsgSig+ ";sReqTimeStamp="+ sReqTimeStamp+ ";sReqNonce="+ sReqNonce,"_getTicket.txt");
            if (string.IsNullOrEmpty(postData)||string.IsNullOrEmpty(sReqMsgSig) || string.IsNullOrEmpty(sReqTimeStamp) || string.IsNullOrEmpty(sReqNonce))
            {
                //postData = DT.Rows[0]["postData"].ToString();
                //sReqMsgSig = DT.Rows[0]["sReqMsgSig"].ToString();
                //sReqTimeStamp = DT.Rows[0]["sReqTimeStamp"].ToString();
                //sReqNonce = DT.Rows[0]["sReqNonce"].ToString();
                string updateSQL = "update asm_platformInfo set postData='"+postData+ "',sReqMsgSig='"+sReqMsgSig+ "',sReqTimeStamp='"+sReqTimeStamp+ "',sReqNonce='"+ sReqNonce + "'";
                Util.Debuglog("updateSQL=" + updateSQL, "_getTicket.txt");
                DbHelperSQL.ExecuteSql(updateSQL);
            }
            else
            {
                //RedisHelper.SetRedisModel("postData", postData, new TimeSpan(1, 0, 0));
                //RedisHelper.SetRedisModel("sReqMsgSig", sReqMsgSig, new TimeSpan(1, 0, 0));
                //RedisHelper.SetRedisModel("sReqTimeStamp", sReqTimeStamp, new TimeSpan(1, 0, 0));
                //RedisHelper.SetRedisModel("sReqNonce", sReqNonce, new TimeSpan(1, 0, 0));
                string updateSQL = "update asm_platformInfo set postData='" + postData + "',sReqMsgSig='" + sReqMsgSig + "',sReqTimeStamp='" + sReqTimeStamp + "',sReqNonce='" + sReqNonce + "'";
                Util.Debuglog("updateSQL=" + updateSQL, "_getTicket.txt");
                DbHelperSQL.ExecuteSql(updateSQL);
            }
            //2-开放平台上设置的token, appID, EncodingAESKey
            string sToken = OpenPFConfig.Token;
            string sAppID = OpenPFConfig.Appid;
            string sEncodingAESKey = OpenPFConfig.AESKey;
            Tencent.WXBizMsgCrypt wxcpt = new Tencent.WXBizMsgCrypt(sToken, sEncodingAESKey, sAppID);
            string sMsg = "";  //解析之后的明文
            #region 测试用的代码
            #endregion
            int ret = 0;
            ret = wxcpt.DecryptMsg(sReqMsgSig, sReqTimeStamp, sReqNonce, postData.Replace(" ","").Replace("\n",""), ref sMsg);
            //if (ret != 0)
            //{
            //    log.Info("ERR: 解析失败, ret: " + ret);
            //}
            //else
            //{
            //    log.Info(ret + "----" + sMsg);
            //}
            
            XDocument xDoc = XDocument.Parse(sMsg.Replace(" ",""));
            List<XElement> q = (from c in xDoc.Elements() select c).ToList();
            string component_verify_ticket = q.Elements("ComponentVerifyTicket").First().Value;
            #endregion
            //将ticket存入Redis
            string ticket =component_verify_ticket.Split(new string[] {"@@@"}, StringSplitOptions.RemoveEmptyEntries)[1].Replace(" ", "");
            string update = "update asm_platformInfo set ticket='"+ticket+"'";
            DbHelperSQL.ExecuteSql(update);
            Response.Write("SUCCESS");
        }
        
    }
}