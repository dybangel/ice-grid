using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace autosell_center.main.jiqi
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