using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
namespace Maticsoft.Model{
	 	//asm_videoRecord
		public class asm_videoRecord
	{
   		     
      	/// <summary>
		/// id
        /// </summary>		
		private int _id;
        public int id
        {
            get{ return _id; }
            set{ _id = value; }
        }        
		/// <summary>
		/// 机器id
        /// </summary>		
		private int _mechineid;
        public int mechineID
        {
            get{ return _mechineid; }
            set{ _mechineid = value; }
        }        
		/// <summary>
		/// 视频id
        /// </summary>		
		private int _videoid;
        public int videoID
        {
            get{ return _videoid; }
            set{ _videoid = value; }
        }        
		/// <summary>
		/// 累计播放次数
        /// </summary>		
		private int _num;
        public int num
        {
            get{ return _num; }
            set{ _num = value; }
        }
        private string _time;
        public string time
        {
            get { return _time; }
            set { _time = value; }
        }

    }
}

