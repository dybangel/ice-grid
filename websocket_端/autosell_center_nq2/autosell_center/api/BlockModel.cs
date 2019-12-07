using System;
using System.Web;
using System.Xml;
using System.Collections;
using System.Net;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace autosell_center.api
{
    public class BlockModel
    {
        private string sendMsg;

        public string SendMsg

        {

            get { return sendMsg; }

            set { sendMsg = value; }

        }
        private long msgId;

        public long MsgId

        {

            get { return msgId; }

            set { msgId = value; }

        }
        private string id;

        public string ID

        {

            get { return id; }

            set { id = value; }

        }

        //返回数据的状态、0正常、1未接收到、2异常
        private int status;

        public int Status

        {

            get { return status; }

            set { status = value; }

        }
        private string _cmd;
        public string cmd
        {

            get { return _cmd; }

            set { _cmd = value; }

        }
        
        private string _samtype;
        public string samtype
        {

            get { return _samtype; }

            set { _samtype = value; }

        }

      
    }
}