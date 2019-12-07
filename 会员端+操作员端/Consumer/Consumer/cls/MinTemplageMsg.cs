using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Consumer.cls
{
    public static class MinTemplageMsg
    {
        public class MsgInfo
        {
            private string Touser;
            private string Template_id;
            private string Page;
            private string Form_id;
            private Dictionary<string, Dictionary<string, string>> Data;
            public string touser
            {
                get { return Touser; }
                set { Touser = value; }
            }
            public string template_id
            {
                get { return Template_id; }
                set { Template_id = value; }
            }
            public string page
            {
                get { return Page; }
                set { Page = value; }
            }
            public string form_id
            {
                get { return Form_id; }
                set { Form_id = value; }
            }
            public Dictionary<string, Dictionary<string, string>> data
            {
                get { return Data; }
                set { Data = value; }
            }


        }
        
        public static Dictionary<string, string> Val(string val)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("value",val);
            return dic;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="touser">接收者</param>
        /// <param name="formId">表单ID</param>
        /// <param name="name">会员昵称</param>
        /// <param name="time">时间</param>
        /// <param name="noticeInfo">温馨提示</param>
        /// <returns></returns>
        public static string sendRegist(string touser, string formId,string name,string noticeInfo)
        {
            MsgInfo msgInfo = new MsgInfo();
            msgInfo.touser = touser;
            msgInfo.form_id = formId;
            msgInfo.template_id = "jIgqkxQstR7tw9Wo6IiQK0z49SJ88USO6bGdkb2vz5c";
            msgInfo.page = "";
            Dictionary<string, Dictionary<string, string>> dic = new Dictionary<string, Dictionary<string, string>>();
            dic.Add("keyword1",Val(name));
            dic.Add("keyword2", Val(DateTime.Now.ToString("yyyy年MM月dd日 HH:mm")));
            dic.Add("keyword3", Val(noticeInfo));
            msgInfo.data = dic;
            return ToJSON(msgInfo);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="touser"></param>
        /// <param name="formId"></param>
        /// <param name="money">充值金额</param>
        /// <param name="orderNO">单号</param>
        /// <param name="zsMoney">赠送金额</param>
        /// <param name="bz">备注</param>
        /// <returns></returns>
        public static string sendCZ(string touser, string formId,string money,string orderNO,string zsMoney,string bz)
        {
            MsgInfo msgInfo = new MsgInfo();
            msgInfo.touser = touser;
            msgInfo.form_id = formId;
            msgInfo.template_id = "dcw2nZIJJPzoSquRdYU-Fp-17ODy8RjPMhI-X9WpL-s";
            msgInfo.page = "";
            Dictionary<string, Dictionary<string, string>> dic = new Dictionary<string, Dictionary<string, string>>();
            dic.Add("keyword1", Val("青岛冰格智能科技有限公司"));
            dic.Add("keyword2", Val(DateTime.Now.ToString("yyyy年MM月dd日 HH:mm")));
            dic.Add("keyword3", Val(money));
            dic.Add("keyword4", Val(orderNO));
            dic.Add("keyword5", Val(zsMoney));
            dic.Add("keyword6", Val(bz));
            msgInfo.data = dic;
            return ToJSON(msgInfo);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="touser"></param>
        /// <param name="formId"></param>
        /// <param name="orderNO"></param>
        /// <param name="name"></param>
        /// <param name="money"></param>
        /// <param name="payType">支付方式</param>
        /// <param name="bz"></param>
        /// <returns></returns>
        public static string sendConsume(string touser, string formId,string orderNO,string name,string money,string payType,string bz)
        {
            MsgInfo msgInfo = new MsgInfo();
            msgInfo.touser = touser;
            msgInfo.form_id = formId;
            msgInfo.template_id = "enfbHpcZnDKP3SCSXraIwttKjAD3L3TxmXQrSp5FDsQ";
            msgInfo.page = "";
            Dictionary<string, Dictionary<string, string>> dic = new Dictionary<string, Dictionary<string, string>>();
            dic.Add("keyword1", Val(orderNO));
            dic.Add("keyword2", Val(name));
            dic.Add("keyword3", Val(money));
            dic.Add("keyword4", Val(DateTime.Now.ToString("yyyy年MM月dd日 HH:mm")));
            dic.Add("keyword5", Val(payType));
            dic.Add("keyword6", Val(bz));
            msgInfo.data = dic;
            return ToJSON(msgInfo);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="touser"></param>
        /// <param name="formId"></param>
        /// <param name="orderNO"></param>
        /// <param name="productName">商品名称</param>
        /// <param name="address">取货地址</param>
        /// <param name="code">取货码</param>
        /// <param name="bz">备注</param>
        /// <param name="nociteInfo">温馨提示</param>
        /// <param name="phone">联系电话</param>
        /// <returns></returns>
        public static string sendQH(string touser, string formId,string orderNO,string productName,string address,string code,string bz,string nociteInfo,string phone)
        {
            MsgInfo msgInfo = new MsgInfo();
            msgInfo.touser = touser;
            msgInfo.form_id = formId;
            msgInfo.template_id = "U1_WbqK_u9zIewJvy-x0nDh8AJynOlkSmPmwtEakk6Y";
            msgInfo.page = "";
            Dictionary<string, Dictionary<string, string>> dic = new Dictionary<string, Dictionary<string, string>>();
            dic.Add("keyword1", Val(orderNO));
            dic.Add("keyword2", Val(productName));
            dic.Add("keyword3", Val(address));
            dic.Add("keyword4", Val(code));
            dic.Add("keyword5", Val(bz));
            dic.Add("keyword6", Val(nociteInfo));
            dic.Add("keyword7", Val(phone));
            msgInfo.data = dic;
            return ToJSON(msgInfo);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="touser"></param>
        /// <param name="formId"></param>
        /// <param name="productName">产品名称</param>
        /// <param name="money">金额</param>
        /// <param name="info">出货信息</param>
        /// <returns></returns>
        public static string sendCH(string touser, string formId,string productName,string money,string info)
        {
            MsgInfo msgInfo = new MsgInfo();
            msgInfo.touser = touser;
            msgInfo.form_id = formId;
            msgInfo.template_id = "co_xOLxyDt6C5dGApkldNka0Lmuiwjxl0H-J0-nHtTY";
            msgInfo.page = "";
            Dictionary<string, Dictionary<string, string>> dic = new Dictionary<string, Dictionary<string, string>>();
            dic.Add("keyword1", Val(productName));
            dic.Add("keyword2", Val(money));
            dic.Add("keyword3", Val(DateTime.Now.ToString("yyyy年MM月dd日 HH:mm")));
            dic.Add("keyword4", Val(info));
            msgInfo.data = dic;
            return ToJSON(msgInfo);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="touser"></param>
        /// <param name="formId"></param>
        /// <param name="orderNO">单号</param>
        /// <param name="money">金额</param>
        /// <param name="productName">产品名称</param>
        /// <param name="bz">备注</param>
        /// <param name="address">取货地址</param>
        /// <returns></returns>
        public static string sendPay(string touser, string formId,string orderNO,string money,string productName,string bz,string address)
        {
            MsgInfo msgInfo = new MsgInfo();
            msgInfo.touser = touser;
            msgInfo.form_id = formId;
            msgInfo.template_id = "mQMkENvsOaGKGnKGiDu2ELqYuJ9GmQ7qr7CAaLNYK7A";
            msgInfo.page = "";
            Dictionary<string, Dictionary<string, string>> dic = new Dictionary<string, Dictionary<string, string>>();
            dic.Add("keyword1", Val(orderNO));
            dic.Add("keyword2", Val(money));
            dic.Add("keyword3", Val(DateTime.Now.ToString("yyyy年MM月dd日 HH:mm")));
            dic.Add("keyword4", Val(productName));
            dic.Add("keyword5", Val(bz));
            dic.Add("keyword6", Val(address));
            msgInfo.data = dic;
            return ToJSON(msgInfo);
        }
        public static string ToJSON(this object o)
        {
            if (o == null)
            {
                return null;
            }
            return JsonConvert.SerializeObject(o);
        }
    }
}