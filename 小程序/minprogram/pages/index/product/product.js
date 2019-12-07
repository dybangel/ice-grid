// pages/index/product/product.js
const config = require('../../../config')
const app = getApp()
Page({
  /**
   * 页面的初始数据
   */
  data: {
    winWidth: 0,
    winHeight: 0,
    // tab切换  
    currentTab: 0,
    typeList:'',
    productList:'',
    dj:''
  },
  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    var that = this
    
    that.setData({
     
      dj: app.globalData.memberDJ
    }),
    wx.setNavigationBarTitle({
      title: wx.getStorageSync("mechineName"),
    })
    /** 
     * 获取系统信息 
     */
    wx.getSystemInfo({
      success: function (res) {
        that.setData({
          winWidth: res.windowWidth,
          winHeight: res.windowHeight
        });
      }
    });
    that.onInitType();
    that.onInitProduct(0);
    that.onloadLBT()
  },
  onloadLBT: function () {
    var that = this
    var that = this
    wx.request({
      url: config.httpsUrl,
      data: {
        companyID: app.globalData.companyID,
        action: 'getAdverseList'
      },
      method: "GET",
      header: {
        "Content-Type": "application/json"
      },
      success: function (res) {
        if (res.data.code == "200") {
          that.setData({
           
            picList: res.data.db,
          })
        }
      }
    })
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
  /** 
     * 滑动切换tab 
     */
  bindChange: function (e) {
    var that = this;
    that.setData({ currentTab: e.detail.current });
  },
  /** 
   * 点击tab切换 
   */
  swichNav: function (e) {
    var that = this;
    if (this.data.currentTab === e.target.dataset.current) {
      return false;
    } else {
      that.setData({
        currentTab: e.target.dataset.current
      })
      that.onInitProduct(e.target.dataset.current)
    }
  },
  onInitType:function()
  {
    var that = this;
    wx.request({
      url: config.httpsUrl,
      data: {
        companyID: config.companyID,
        action: 'getType'
      },
      method: "GET",
      header: {
        "Content-Type": "application/json"
      },
      success: function (res) {
        that.setData({
          typeList:res.data
        })
      }
    })
  },
  onInitProduct:function(type)
  {
    console.log("companyID====" + config.companyID)
    console.log("mechineID====" + app.globalData.mechineID)
    var that = this;
    wx.request({
      url: config.httpsUrl,
      data: {
        companyID: config.companyID,
        mechineID: wx.getStorageSync("mechineID"),
        action: 'getProductList',
        type: type
      },
      method: "GET",
      header: {
        "Content-Type": "application/json"
      },
      success: function (res) {
        for (var i = 0; i < res.data.length; i++) {
          if (res.data[i].tag != "" && res.data[i].tag!=null )
          {
            res.data[i].tag = res.data[i].tag.split('|')
          }
        }
        console.log("===="+res.data)
        that.setData({
          productList:res.data
        })
      }
    })
  },
  pickProduct:function(e)
  {
    wx.navigateTo({
      url: '../order/cycle?productID=' + e.target.dataset.id,
    })
  }
})