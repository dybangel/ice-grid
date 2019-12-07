using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Consumer.cls
{
   [Serializable]
     public sealed class WeChatTemplatecsMsg
     {
         public string touser { get; set; }
         public string template_id { get; set; }
         public string topcolor { get; set; }
         public string url { get; set; }
         public Dictionary<string, MessageData> data { get; set; }
     }
     [Serializable]
     public sealed class MessageData
     {
         public string value { get; set; }
         public string color { get; set; } = "#1C86EE";
     }
}