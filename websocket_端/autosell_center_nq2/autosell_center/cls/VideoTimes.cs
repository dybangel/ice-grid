using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace autosell_center.cls
{
    public class VideoTimes
    {
        private int _videoID;
        private string _time;
        private int _num;
        private string _mechineid;

        public int videoID
        {
            get
            {
                return _videoID;
            }

            set
            {
                _videoID = value;
            }
        }

        public string time
        {
            get
            {
                return _time;
            }

            set
            {
                _time = value;
            }
        }

        public int num
        {
            get
            {
                return _num;
            }

            set
            {
                _num = value;
            }
        }

        public string mechineid
        {
            get
            {
                return _mechineid;
            }

            set
            {
                _mechineid = value;
            }
        }
    }
}