const config = require('../../../config')
const app = getApp()
Page({

  /**
   * 页面的初始数据
   */
  data: {
    tradeList:'',
    showInfo:true
  },

  /**
     * 下拉刷新(请求服务获取数据并赋值)
     */
  pullDownRefreshData: function (context) {
    let params = {
      pageIndex: 1,
    };
    app.YoniClient.request(app.Func.GET_FAVORITE_LIST, params, function (res) {
      wx.stopPullDownRefresh();
      if (res.code == 0) {
        context.setData({
          tradeList: res.result.goodsList,
        });
      }
    });
  },
  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    var that=this
    if (wx.getStorageSync("userInfo") != "" && wx.getStorageSync("userInfo") != undefined && wx.getStorageSync("unionID") != ""
      && wx.getStorageSync("minOpenID") != "" && wx.getStorageSync("minOpenID") != undefined && wx.getStorageSync("mechineID") != "" && wx.getStorageSync("mechineID")!=undefined) {
      that.getCode()
    }else{
      wx.redirectTo({
        url: '../login',
      })
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
    wx.login({
      success(res) {
        if (res.code) {
          var that = this
          // 发起网络请求
          wx.request({
            url: config.httpsUrl,
            data: {
              code: res.code,
              companyID: config.companyID,
              action: "userlogin"
            },
            method: "GET",
            header: {
              "Content-Type": "application/json"
            },
            success: function (res) {
              console.log("==="+JSON.stringify(res.data));
              app.globalData.minOpenID = res.data.openid
              app.globalData.unionID = res.data.unionid
               
              wx.setStorageSync("minOpenID", res.data.openid)
              wx.setStorageSync("unionID",  res.data.unionid)
            }
          })
        } else {

        }
      }
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
  getCode:function()
  {
    var that = this;
     wx.request({
       url: config.httpsUrl,
       data: {
         mechineID: wx.getStorageSync("mechineID"),
         minOpenID:app.globalData.minOpenID,
         unionID:app.globalData.unionID,
         action: 'getCode'
       },
       method: "GET",
       header: {
         "Content-Type": "application/json"
       },
       success: function (res) {
         if(res.data.code=="200")
         {
           that.setData({
             showInfo:false,
             tradeList: res.data.db
           })
         }else if(res.data.code=="300"){
           that.setData({
             showInfo: true
           })
         }
        
       }
     })
  }
})