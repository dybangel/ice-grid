using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace autosell_center.main.jiqi
{
    public class Statusvalue
    {
        private string text; 

        private string _t1; 

        private string _t2;

        private string _t3;

        private string _t4;
        
        private string _t5;

        public string TEXT
        {
            get
            {
                return text;
            }

            set
            {
                text = value;
            }
        }

        public string t1
        {
            get
            {
                return _t1;
            }

            set
            {
                _t1 = value;
            }
        }

        public string t2
        {
            get
            {
                return _t2;
            }

            set
            {
                _t2 = value;
            }
        }

        public string t3
        {
            get
            {
                return _t3;
            }

            set
            {
                _t3 = value;
            }
        }

        public string t4
        {
            get
            {
                return _t4;
            }

            set
            {
                _t4 = value;
            }
        }

        public string t5
        {
            get
            {
                return _t5;
            }

            set
            {
                _t5 = value;
            }
        }
    }
}