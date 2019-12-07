using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
namespace Maticsoft.Model{
	 	//asm_sellDetail
		public class asm_sellDetail
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
		/// 商品
        /// </summary>		
		private int _productid;
        public int productID
        {
            get{ return _productid; }
            set{ _productid = value; }
        }        
		/// <summary>
		/// 售卖数量
        /// </summary>		
		private int _num;
        public int num
        {
            get{ return _num; }
            set{ _num = value; }
        }        
		/// <summary>
		/// 总金额
        /// </summary>		
		private decimal _totalmoney;
        public decimal totalMoney
        {
            get{ return _totalmoney; }
            set{ _totalmoney = value; }
        }        
		/// <summary>
		/// 售卖时间
        /// </summary>		
		private DateTime _ordertime;
        public DateTime orderTime
        {
            get{ return _ordertime; }
            set{ _ordertime = value; }
        }        
		/// <summary>
		/// 出货料到
        /// </summary>		
		private int _prold;
        public int proLD
        {
            get{ return _prold; }
            set{ _prold = value; }
        }        
		/// <summary>
		/// 0表示零售1表示会员订购
        /// </summary>		
		private int _type;
        public int type
        {
            get{ return _type; }
            set{ _type = value; }
        }        
		/// <summary>
		/// 交易序列号
        /// </summary>		
		private string _orderno;
        public string orderNO
        {
            get{ return _orderno; }
            set{ _orderno = value; }
        }
        private string _bz;
        public string bz
        {
            get { return _bz; }
            set { _bz = value; }
        }

        private string _zt;
        public string zt
        {
            get { return _zt; }
            set { _zt = value; }
        }
        /// <summary>
        /// memberID
        /// </summary>		
        private string _memberid;
        public string memberID
        {
            get{ return _memberid; }
            set{ _memberid = value; }
        }        
		/// <summary>
		/// 取货码（针对会员扫码取货）
        /// </summary>		
		private string _code;
        public string code
        {
            get{ return _code; }
            set{ _code = value; }
        }        
		/// <summary>
		/// 0微信1支付宝 2卡包3其他
        /// </summary>		
		private string _paytype;
        public string payType
        {
            get{ return _paytype; }
            set{ _paytype = value; }
        }        
		/// <summary>
		/// 机器ID
        /// </summary>		
		private int _mechineid;
        public int mechineID
        {
            get{ return _mechineid; }
            set{ _mechineid = value; }
        }
        private string _billno;
        public string billno
        {
            get { return _billno; }
            set { _billno = value; }
        }
    }
}

