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
    public class MsgDTO
    {
        private long createtime;
        private string statusvalue;

        public long Createtime
        {
            get
            {
                return createtime;
            }

            set
            {
                createtime = value;
            }
        }

        public string Statusvalue
        {
            get
            {
                return statusvalue;
            }

            set
            {
                statusvalue = value;
            }
        }
    }
}