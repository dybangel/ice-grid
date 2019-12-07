using autosell_center.api;

using Consumer.cls;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace autosell_center.main.jiqi
{
    public partial class Test : System.Web.UI.Page
    {


        public static int abc = 0;

        protected void Page_Load(object sender, EventArgs e)
        {

            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            //for (int i = 0; i < 20; i++) {
                abc = abc + 1;
                stopwatch.Start();
                string reqUrl = "http://websocket.cs.suqiangkeji.com:88";
                Util.log("start", "test.txt");
                string result = reqUrl + "/api/api.ashx?action=ceshisocket&id=68&cmd=心跳&MsgId="+ abc;
                string msg = OperUtil.GetHttpResponse(result, 60000);
                //JiQi ji= JiQiManager.Get("68");
                //JiQi ji2 = JiQiManager.Get("69");
                stopwatch.Stop();

                Util.log("end:" + msg+",time:" + stopwatch.Elapsed.TotalSeconds+",abc:"+abc, "test.txt");
     
            //}
        }
    }
}