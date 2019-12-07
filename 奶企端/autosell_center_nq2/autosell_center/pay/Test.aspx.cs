using autosell_center.util;
using Consumer.cls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uniondemo.com.allinpay.syb;

namespace uniondemo
{
    public partial class Test : System.Web.UI.Page
    {
        SybWxPayService sybService = new SybWxPayService("25");
        protected void Page_Load(object sender, EventArgs e)
        {

           
            
            



    }

        protected void btn_pay_Click(object sender, EventArgs e)
        {
            try
            {

                Dictionary<String, String> rsp = sybService.pay(1, DateTime.Now.ToFileTime().ToString(), "W01", "商品内容", "备注", "", "", "http://baidu.com", "");
                printRsp(rsp);
            }
            catch (Exception ex)
            {
                this.tblank.Value = ex.Message;
            }
            
        }

        protected void btn_cancel_Click(object sender, EventArgs e)
        {
            try
            {

                Dictionary<String, String> rsp = sybService.cancel(1, DateTime.Now.ToFileTime().ToString(), "12525075", "");
                printRsp(rsp);
            }
            catch (Exception ex)
            {
                this.tblank.Value = ex.Message;
            }
           
        }

        protected void btn_refund_Click(object sender, EventArgs e)
        {
            try
            {

                Dictionary<String, String> rsp = sybService.refund(1, DateTime.Now.ToFileTime().ToString(), "12525075", "");
                printRsp(rsp);
            }
            catch (Exception ex)
            {
                this.tblank.Value = ex.Message;
            }
        }

        protected void btn_query_Click(object sender, EventArgs e)
        {
            try
            {

                Dictionary<String, String> rsp = sybService.query("", "17273218");
                printRsp(rsp);
            }
            catch (Exception ex)
            {
                this.tblank.Value = ex.Message;
            }
            
        }

        private void doRequest(Dictionary<String, String> param, String url)
        {
            //String rsp = HttpUtil.CreatePostHttpResponse(AppConstants.API_URL + url, param, Encoding.UTF8);
            //Dictionary<String, String> rspDic = (Dictionary<String, String>)JsonConvert.DeserializeObject(rsp, typeof(Dictionary<String, String>));
            //rsp = "请求返回数据:" + rsp + "\n";
            //if ("SUCCESS".Equals(rspDic["retcode"]))//验签
            //{
            //    String signRsp = rspDic["sign"];
            //    rspDic.Remove("sign");
            //    String sign = AppUtil.signParam(rspDic, AppConstants.APPKEY);
            //    if (signRsp.Equals(sign))
            //    {
            //        rsp = rsp + "验签成功";
            //    }
            //    else
            //        rsp = rsp + "验签失败";

            //}
            //this.tblank.Value = rsp;
        }

        private void printRsp(Dictionary<String, String> rspDic)
        {
            string rsp = "请求返回数据:\n";
            foreach (var item in rspDic)
            {
                rsp += item.Key + "-----" + item.Value + ";\n";
            }
            this.tblank.Value = rsp;
        }
    }
}