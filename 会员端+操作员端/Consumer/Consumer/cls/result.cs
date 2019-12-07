using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Consumer.cls
{
    public class result
    {
        private string openid;
        private string session_key;
        private string errcode;
        private string errmsg;

        public string Openid
        {
            get
            {
                return openid;
            }

            set
            {
                openid = value;
            }
        }

        public string Session_key
        {
            get
            {
                return session_key;
            }

            set
            {
                session_key = value;
            }
        }

        public string Errcode
        {
            get
            {
                return errcode;
            }

            set
            {
                errcode = value;
            }
        }

        public string Errmsg
        {
            get
            {
                return errmsg;
            }

            set
            {
                errmsg = value;
            }
        }
    }
}