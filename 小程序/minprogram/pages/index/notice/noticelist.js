// pages/index/notice/noticelist.js
const config = require('../../../config')
const app = getApp()
Page({

  /**
   * 页面的初始数据
   */
  data: {
    noticelist:'',
    current: 'notice'
  },
  handleChange({ detail }) {
    console.log("key=" + detail.key);
    this.setData({
      current: detail.key
    });
    if (detail.key == "homepage") {
      wx.redirectTo({
        url: '../indexpage/home',
      })
    } else if (detail.key == "order") {
      wx.redirectTo({
        url: '../order/orderlist',
      })
    } else if (detail.key == "notice") {
      wx.redirectTo({
        url: '../notice/noticelist',
      })
    } else if (detail.key == "mine") {
      wx.redirectTo({
        url: '../member/center',
      })
    }
  },
   onloadNoticeList:function()
   {
     var that = this;
     wx.request({
       url: config.httpsUrl,
       data: {
         unionID: app.globalData.unionID,
         minOpenID:app.globalData.minOpenID,
         mechineID: wx.getStorageSync("mechineID"),
         action: 'getNoticeList'
       },
       method: "GET",
       header: {
         "Content-Type": "application/json"
       },
       success: function (res) {
        if(res.data.code=="200")
        {
          that.setData({
            noticelist: res.data.db
          })
        }
       }
     })
   },
  getNotice:function(e)
  {
    var val = e.currentTarget.dataset.id;
    wx.navigateTo({
      url: 'notice?id='+val
    })
  },
  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    wx.setNavigationBarTitle({
      title: "通知",
    })
    app.globalData.userPhone = wx.getStorageSync("userPhone")
    
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
    var that = this;
    that.onloadNoticeList()
    that.setData({
      current: 'notice'
    })
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

  }
})