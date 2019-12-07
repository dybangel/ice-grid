using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using WxPayAPI;

namespace gb
{
    public partial class ResultNotifyPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            ResultNotify resultNotify = new ResultNotify(this);
            //扫码回调调用没有参数的
            resultNotify.ProcessNotify();
        }

        [WebMethod]
        public static string GetWxPayBackData(string out_trade_no, string cardId, string orderNo)
        {
            WxPayData data = new WxPayData();
            data.SetValue("out_trade_no", out_trade_no);
            WxPayData result = WxPayApi.OrderQuery(data);//提交订单查询请求给API，接收返回数据
            //appid=wx3dd4d4698955cef3<br>attach=wx20171129171510297580<br>bank_type=CFT<br>cash_fee=1<br>fee_type=CNY<br>is_subscribe=Y<br>mch_id=1314471801<br>nonce_str=i1Kr0FZp6v5zUgY4<br>openid=opsTsvvf2DWDYhJWiRiGmcJF5uWQ<br>out_trade_no=131447180120171129171510327<br>result_code=SUCCESS<br>return_code=SUCCESS<br>return_msg=OK<br>sign=F57273F59E2470DAB0B01EDB4615DDD6<br>time_end=20171129171528<br>total_fee=1<br>trade_state=SUCCESS<br>trade_type=NATIVE<br>transaction_id=4200000015201711297852647372<br>

            #region 扫码之后所做操作（验证订单是否支付成功+修改订单状态）
            Dictionary<string, string> dic = new Dictionary<string, string>();
            var WxPayDataStr = result.ToPrintStr();
            string[] arr = WxPayDataStr.Split(new string[1] { "<br>" }, StringSplitOptions.RemoveEmptyEntries);
            if (arr.Length > 1)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    if (!string.IsNullOrEmpty(arr[i]) && arr[i].Length > 0)
                    {
                        var arr1 = arr[i].Split('=');
                        dic.Add(arr1[0], arr1[1]);
                    }
                }
                if (dic.Keys.Contains("trade_state_desc") && dic["trade_state_desc"] == "订单未支付")
                {
                    //订单尚未支付
                    return "notpay";
                }
                else
                {
                    string back_out_trade_no = dic["out_trade_no"];
                    string back_total_fee = dic["total_fee"];
                    string back_transaction_id = dic["transaction_id"];
                    //订单支付成功（更新count+改订单状态）
                  
                    
                }
                return "";
            }
            else
            {
                return "";
            }
            #endregion
        }
    }
}