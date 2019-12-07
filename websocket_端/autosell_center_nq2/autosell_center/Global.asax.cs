using autosell_center.api;
using autosell_center.cls;
using autosell_center.main.jiqi;
using Consumer.cls;
using DBUtility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Timers;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Web.SessionState;
using uniondemo.com.allinpay.syb;

namespace autosell_center
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            
            JiQiManager.Init();

            //System.Timers.Timer myTimer = new System.Timers.Timer(1000 * 1);
            //myTimer.Elapsed += new ElapsedEventHandler(myTimer_Elapsed);
            //myTimer.Enabled = true;
            //myTimer.AutoReset = true;

        }
       
        public static readonly          Object lock1 = new Object();
        public void myTimer_Elapsed(object source, ElapsedEventArgs e)
        {
            string time = DateTime.Now.ToString("HH:mm");
            //  if (time == "01:01")
            lock (lock1)
            {
                
                if (time == time)
                {
                    string path = HttpRuntime.AppDomainAppPath.ToString() + "log/video/";
                    DirectoryInfo root = new DirectoryInfo(path);
                    List<VideoTimes> list = new List<VideoTimes>();
                    foreach (FileInfo f in root.GetFiles())
                    {
                        string name = f.Name;
                        string mechineID = name.Split('_')[1];
                        Dictionary<string, int> dic = Util.Read(name);
                        foreach (KeyValuePair<string, int> keyValuePair in dic)
                        {
                            //此处是获取到的每个机器每个视频一天的播放次数
                            //拼接json 格式 [{"videoID":145,"time":"2019-02-26 00:02:57","num":1,"mechineid":"37"},{"videoID":145,"time":"2019-02-26 00:03:54","num":1,"mechineid":"37"}]
                            VideoTimes vtimes = new VideoTimes();
                            vtimes.mechineid = mechineID;
                            vtimes.num = keyValuePair.Value;
                            vtimes.videoID = int.Parse(keyValuePair.Key);
                            vtimes.time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            list.Add(vtimes);
                        }
                    }
                    JsonConvert.SerializeObject(list);
                    string result = JsonConvert.SerializeObject(list);
                    Util.log("result=" + result, "提交播放记录到服务器.txt");
                    Util.uploadVideoRecord(result);
                }

            }
            
           
        }
        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}