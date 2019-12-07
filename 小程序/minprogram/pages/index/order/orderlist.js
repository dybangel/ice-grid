//pages/index/order/orderlist.js
const config = require('../../../config')
const { $Toast } = require('../../../dist/base/index');
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
    allList: '',
    shengchan:'',
    quhuozhong:'',
    wancheng:'',
    zhuanshou:'',
    shixiao:'',
    current: 'order',
    dh:''
  },
  handleChange({ detail }) {
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
  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    wx.setNavigationBarTitle({
      title: "订单列表",
    })
    var that = this;
    if (options.tab !=undefined)
    {
      that.setData({
        currentTab: options.tab
      })
    }
    /** 
     * 获取系统信息 
     */
    // wx.getSystemInfo({
    //   success: function (res) {
    //     that.setData({
    //       winWidth: res.windowWidth,
    //       winHeight: res.windowHeight
    //     });
    //   }
    // });
    that.loadOrderAll();
    that.loadOrdersc();
    that.loadOrderqh();
    that.loadOrderwc();
    that.loadOrderDH();
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
    var that=this
    that.setData({
      current: 'order'
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

  },
  dhProduct:function(e)
  {
    var orderNO = e.currentTarget.dataset.orderno;
    wx.navigateTo({
      url: 'dhProduct?orderno=' + orderNO,
    })
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
    }
  },
  //加载订单列表 0生产中1配送中2已转售3配送完成4已兑换5取消
  loadOrderAll: function () {
    var that = this;
    wx.request({
      url: config.httpsUrl,
      data: {
        type: "-1",
        openID: app.globalData.minOpenID,
        unionID:app.globalData.unionID,
        companyID: app.globalData.companyID,
        action: 'getOrderList'
      },
      method: "GET",
      header: {
        "Content-Type": "application/json"
      },
      success: function (res) { 
        if (res.data.code=="200")
        {
         
          that.setData({
            winHeight: 160 * (res.data.db.length),
            allList: res.data.db
          })
        } 
      
      }
    })
  },
  loadOrdersc: function () {
    var that = this;
    wx.request({
      url: config.httpsUrl,
      data: {
        type: "0",
        openID: app.globalData.minOpenID,
        unionID: app.globalData.unionID,
        companyID: app.globalData.companyID,
        action: 'getOrderList'
      },
      method: "GET",
      header: {
        "Content-Type": "application/json"
      },
      success: function (res) {
        if (res.data.code=="200")
        {
          that.setData({
            shengchan: res.data.db
          })
        }
      }
    })
  },
  loadOrderqh: function () {
    var that = this;
    wx.request({
      url: config.httpsUrl,
      data: {
        type: "1",
        openID: app.globalData.minOpenID,
        unionID: app.globalData.unionID,
        companyID: app.globalData.companyID,
        action: 'getOrderList'
      },
      method: "GET",
      header: {
        "Content-Type": "application/json"
      },
      success: function (res) {
        if(res.data.code=="200")
        {
          that.setData({
            quhuozhong: res.data.db
          })
        }
      }
    })
  },
  loadOrderwc: function () {
    var that = this;
    wx.request({
      url: config.httpsUrl,
      data: {
        type: "3",
        openID: app.globalData.minOpenID,
        unionID: app.globalData.unionID,
        companyID: app.globalData.companyID,
        action: 'getOrderList'
      },
      method: "GET",
      header: {
        "Content-Type": "application/json"
      },
      success: function (res) {
       if(res.data.code=="200")
       {
         that.setData({
           wancheng: res.data.db
         })
       }
      }
    })
  },
  loadOrderDH: function () {
    var that = this;
    wx.request({
      url: config.httpsUrl,
      data: {
        type: "4",
        openID: app.globalData.minOpenID,
        unionID: app.globalData.unionID,
        companyID: app.globalData.companyID,
        action: 'getOrderList'
      },
      method: "GET",
      header: {
        "Content-Type": "application/json"
      },
      success: function (res) {
        if (res.data.code == "200") {
          that.setData({
            dh: res.data.db
          })
        }
      }
    })
  },
  loadOrderzs: function () {
    var that = this;
    wx.request({
      url: config.httpsUrl,
      data: {
        type: 4,
        openID: app.globalData.minOpenID,
        unionID: app.globalData.unionID,
        companyID: app.globalData.companyID,
        action: 'getOrderListDetail'
      },
      method: "GET",
      header: {
        "Content-Type": "application/json"
      },
      success: function (res) {


      }
    })
  },
  loadOrdersx: function () {
    var that = this;
    wx.request({
      url: config.httpsUrl,
      data: {
        type: 0,
        openID: app.globalData.minOpenID,
        unionID: app.globalData.unionID,
        companyID: app.globalData.companyID,
        action: 'getOrderListDetail'
      },
      method: "GET",
      header: {
        "Content-Type": "application/json"
      },
      success: function (res) {


      }
    })
  },
  selOrder:function(e)
  {
    var orderID = e.currentTarget.dataset.id;
    var orderno = e.currentTarget.dataset.orderno;
    var mechineid = e.currentTarget.dataset.mechineid;
    var that=this
    wx.navigateTo({
      url: 'orderDetail?orderID=' + orderID + '&orderNO=' + orderno + '&mechineID=' + mechineid,
    })
  }

})