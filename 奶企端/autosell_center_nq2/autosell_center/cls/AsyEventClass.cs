using Consumer.cls;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Web;

namespace autosell_center.cls
{
    public class AsyEventClass
    {
        private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        //异步线程，委托
        //public delegate string AsyncEventHandler(long num1, long num2);
        //public string ToDealData(long num1, long num2)
        //{
        //    logger.Error("异步处理数据开始：num1:" + num1 + ",num2:"+num2);
        //    Thread.Sleep(5000);
        //    return (num1 + num2).ToString();
        //    //处理数据或其他操作return "异步成功，num1:" + mainDigitalId + ",num2:" + bindDigitalId;
        //}
        public delegate string AsyncEventHandler(string recordList);
        public string upSellRecord(string recordList)
        {
            return Util.uploadSellDetail(recordList);
        }
        public void Callback(IAsyncResult result)
        {
            AsyncEventHandler handler = (AsyncEventHandler)((AsyncResult)result).AsyncDelegate;
            Util.Debuglog("result="+ handler.EndInvoke(result), "AsyEventClass.txt");
             
        }
    }
}