//index.js
//获取应用实例
const config = require('../../config')
const app = getApp()
Page({
  data: {
    motto: 'Hello World',
    userInfo: {},
    hasUserInfo: false,
    canIUse: wx.canIUse('button.open-type.getUserInfo'),
    valSource:''
    
  },
  //事件处理函数
  bindViewTap: function() {
    wx.navigateTo({
      url: '../logs/logs'
    })
  },
  onLoad: function (options) {
    var that=this
    if (options.source !=undefined){
      that.setData({
        valSource: options.source
      })
    }
    if (that.data.valSource=="1")
    {
      //证明此处是从公众号过来的请求 且需要跳转到 订奶 跳到首页
       
      var userPhone = wx.getStorageSync("userPhone")
      if(userPhone!=""&&userPhone!=null)
      {
        app.globalData.userPhone = userPhone
        wx.redirectTo({
          url: 'indexpage/home',
        })
      }
    } else if (that.data.valSource=="2")
    {
      //跳转到取奶 取货码
      
      var userPhone = wx.getStorageSync("userPhone")
      if (userPhone != "" && userPhone != null) {
        app.globalData.userPhone = userPhone
        wx.redirectTo({
          url: 'order/getCode',
        })
      }
    } else if (that.data.valSource=="3")
    {
      //跳转到充值 跳转到钱包
       
      var userPhone = wx.getStorageSync("userPhone")
      if (userPhone != "" && userPhone != null) {
        app.globalData.userPhone = userPhone
        wx.redirectTo({
          url: 'member/wallet',
        })
      }
    }
    
  },
  onShow:function()
  {
    var that = this
    app.globalData.minOpenID = wx.getStorageSync("minOpenID")
    app.globalData.unionID = wx.getStorageSync("unionID")
    var userPhone = wx.getStorageSync("userPhone")
    if(userPhone!=""&&userPhone!=null)
    {
      app.globalData.userPhone = userPhone
      wx.redirectTo({
        url: 'indexpage/home',
      })
    }
     
  },
  getUserInfo: function(e) {
    app.globalData.userInfo = e.detail.userInfo
    this.setData({
      userInfo: e.detail.userInfo,
      hasUserInfo: true
    })
  },
  getPhoneNumber:function(e){
    //获取用户信息
    // 登录
    wx.login({
      success(res) {
        if (res.code) {
          var that=this
          // 发起网络请求
          wx.request({
            url: config.httpsUrl,
            data: {
              code: res.code,
              companyID:config.companyID,
              action: "userlogin"
            },
            method: "GET",
            header: {
              "Content-Type": "application/json"
            },
            success: function (res) {
              app.globalData.minOpenID = res.data.openid
              app.globalData.unionID = res.data.unionid
              wx.setStorageSync("minOpenID", res.data.openid)
              wx.setStorageSync("unionID", res.data.unionid)
            }
          })
        } else {
           
        }
      }
    })
    if (e.detail.errMsg =="getPhoneNumber:ok")
    {
      var that=this
      wx.request({
        url: config.httpsUrl,
       data: { 
         action:'getPhoneNum',
         code: app.globalData.code,
         iv: e.detail.iv,
         companyID:config.companyID,
         encryptedData:e.detail.encryptedData
       },
       method: "GET",
       header: {
         "Content-Type": "application/json"
       },
       success:function(res){
         console.log("登录获取手机号="+JSON.stringify(res.data));
         app.globalData.userPhone = res.data.phoneNumber
         wx.setStorageSync("userPhone", res.data.phoneNumber)

         //插入会员
          



         if (that.data.valSource == "1") {
           //证明此处是从公众号过来的请求 且需要跳转到 订奶 跳到首页
            
           var userPhone = wx.getStorageSync("userPhone")
           if (userPhone != "" && userPhone != null) {
             app.globalData.userPhone = userPhone
             wx.redirectTo({
               url: 'indexpage/home',
             })
           }
         } else if (that.data.valSource == "2") {
           //跳转到取奶 取货码
           
           var userPhone = wx.getStorageSync("userPhone")
           if (userPhone != "" && userPhone != null) {
             app.globalData.userPhone = userPhone
             wx.redirectTo({
               url: 'order/getCode',
             })
           }
         } else if (that.data.valSource == "3") {
           //跳转到充值 跳转到钱包
           
           var userPhone = wx.getStorageSync("userPhone")
           if (userPhone != "" && userPhone != null) {
             app.globalData.userPhone = userPhone
             wx.redirectTo({
               url: 'member/wallet',
             })
           }
         }else{
           //此处为正常手机号授权登录之后的跳转
           wx.redirectTo({
             url: 'indexpage/home',
           })
         }

       }
     });
    } else{
      wx.redirectTo({
        url: 'indexpage/home',
      })
    }
  }
})
