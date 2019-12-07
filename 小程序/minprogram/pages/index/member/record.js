// pages/index/member/record.js
const config = require('../../../config')
const app = getApp()
Page({

  /**
   * 页面的初始数据
   */
  data: {
    dates: '',
    recordList:'',
    totalIn:'0',
    totalOut:'0',
    record:''
  },
  getNowTime: function () {
    var now = new Date();
    var year = now.getFullYear();
    var month = now.getMonth() + 1;
    var day = now.getDate() + day;
    if (month < 10) {
      month = '0' + month;
    };
    if (day < 10) {
      day = '0' + day;
    };
    //  如果需要时分秒，就放开
    // var h = now.getHours();
    // var m = now.getMinutes();
    // var s = now.getSeconds();
    var formatDate = year + '-' + month;
    return formatDate;
  },
  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    wx.setNavigationBarTitle({
      title: " 奶卡余额变化明细",
    })
    var that=this
    that.setData({
      dates: that.getNowTime()
    })
    that.getRecordList()
   
  },

  /**
   * 生命周期函数--监听页面初次渲染完成
   */
  onReady: function () {

  },

  /**
   * 生命周期函数--监听页面显示
   */
  onShow: function () {

  },

  /**
   * 生命周期函数--监听页面隐藏
   */
  onHide: function () {

  },

  /**
   * 生命周期函数--监听页面卸载
   */
  onUnload: function () {

  },

  /**
   * 页面相关事件处理函数--监听用户下拉动作
   */
  onPullDownRefresh: function () {

  },

  /**
   * 页面上拉触底事件的处理函数
   */
  onReachBottom: function () {

  },

  /**
   * 用户点击右上角分享
   */
  onShareAppMessage: function () {

  },
  //  点击日期组件确定事件  
  bindDateChange: function (e) {
    var that=this
    that.setData({
      dates: e.detail.value
    })
    that.getRecordList()
  },
  getRecordList:function()
  {
    var that=this
    wx.request({
      url: config.httpsUrl,
      data: {
        minOpenID: app.globalData.minOpenID,
        unionID:app.globalData.unionID,
        time: that.data.dates,
        action: 'getMoneyChange'
      },
      method: "GET",
      header: {
        "Content-Type": "application/json"
      },
      success: function (res) {
        console.log(JSON.stringify(res.data));
        if (res.data.code == "200") {
          var len=res.data.db.length;
          if(len>0)
          {
            var _in=0,_out=0;
            for(var i=0;i<len;i++)
            {
              if (res.data.db[i].type == "1" || res.data.db[i].type == "3" || res.data.db[i].type == "5")
              {
                _in += res.data.db[i].money
                res.data.db[i].money = "￥" + (res.data.db[i].money).toFixed(2)
               
              }
              if (res.data.db[i].type == "2" || res.data.db[i].type == "4") {
                _out+= res.data.db[i].money
                res.data.db[i].money = "￥-" + (res.data.db[i].money).toFixed(2)
              }
            }
            that.setData({
              totalIn: _in.toFixed(2),
              totalOut: _out.toFixed(2)
            })
          }
          that.setData({
            recordList: res.data.db,
            
          })
        } else {
           that.setData({
             recordList:'',
             record:'暂无记录'
           })
        }
      }
    })
  }
  
 
})