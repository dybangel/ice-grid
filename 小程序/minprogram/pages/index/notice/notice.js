// pages/index/notice/noticelist.js
const config = require('../../../config')
const app = getApp()
Page({

  /**
   * 页面的初始数据
   */
  data: {
    id:'',
    title:'',
    con:'',
    address:''
  },
  getNotice: function (val) {
    var that = this;
    wx.request({
      url: config.httpsUrl,
      data: {
        id: val,
        action: 'getNotice'
      },
      method: "GET",
      header: {
        "Content-Type": "application/json"
      },
      success: function (res) {
        if(res.data.code=="200")
        {
          that.setData({
            title:res.data.db[0].title,
            con:res.data.db[0].con,
            address:res.data.db[0].address
          })
        }
        
      }
    })
  },
  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    var that=this;
    that.setData({
      id: options.id
    })
    if (options.id!=undefined)
     {
      that.getNotice(options.id);
     }
    
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

  }
})