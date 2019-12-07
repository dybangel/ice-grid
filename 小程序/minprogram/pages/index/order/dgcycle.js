// pages/index/order/cycle.js
const config = require('../../../config')
const util = require('../../../utils/util')
const { $Toast } = require('../../../dist/base/index');
const app = getApp()
Page({
  /**
   * 页面的初始数据
   */
  data: {
    dates: '',
    productInfo: '',
    dgOrderInfo:'',
    value1: 1,
    dj: '',
    tags: '',
    zsStr: '',//用于显示总件数地方
    memberInfoFlag: false,
    cycle: [
      {
        name: '1日1送',
        checked: true,
        color: 'red',
        id: '1'
      },
      {
        name: '2日1送',
        checked: false,
        color: 'white',
        id: '2'
      },
      {
        name: '3日1送',
        checked: false,
        color: 'white',
        id: '3'
      },
      {
        name: '周一至周五',
        checked: false,
        color: 'white',
        id: '4'
      },
      {
        name: '周末送',
        checked: false,
        color: 'white',
        id: '5'
      }
    ],
    psCycle: '',
    psMode: '1',
    num: '',
    product_ID: '',
    zq: '',
    totalMoney: 0,
    loadFlag: false,
    zqType: '',
    zqDis: '0',
    showRight1: false,
    mechineList: '',
    mechineName: '',
    xjlc: '',
    address: '',
    DGcode:''
  },
  handleClick: function () {
    this.setData({
      showRight1: !this.data.showRight1
    });
  },
  // 加载机器列表
  onInitMechineList: function (lng, lat) {
    var that = this;
    wx.request({
      url: config.httpsUrl,
      data: {
        companyID: config.companyID,
        longitude: lng,
        latitude: lat,
        action: 'getMechineList'
      },
      method: "GET",
      header: {
        "Content-Type": "application/json"
      },
      success: function (res) {
        var len = res.data.length;
        for (var i = 0; i < len; i++) {
          if (i == 0) {
            that.setData({
              mechineList: res.data
            });
            
            wx.setStorageSync("mechineID", res.data[0].id)
            wx.setStorageSync("mechineName", res.data[0].mechineName)
          }
        }
      }
    })
  },
  clickMechine: function (e) {
    var name = e.currentTarget.dataset.name;
    var address = e.currentTarget.dataset.address;
    var jl = e.currentTarget.dataset.jl;
    var id = e.currentTarget.dataset.id;
    var that = this;
    that.setData({
      mechineName: name,
      xjlc: jl,
      address: address,
      showRight1: !this.data.showRight1
    })
    
    wx.setStorageSync("mechineID", id)
    wx.setStorageSync("mechineName", name)
  },
  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    var that = this
    that.data.product_ID = options.productID
    wx.setNavigationBarTitle({
      title: wx.getStorageSync("mechineName")
    })
    that.loadProduct()
    that.setData({
      dj: app.globalData.memberDJ,
      DGcode: options.code
    })
    that.getProductByCode()
    wx.getLocation({
      success(res) {
        that.onInitMechineList(res.longitude, res.latitude)
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
    var that = this
    that.getUserInfo()
    that.setData({
      address: app.globalData.address,
      mechineName: wx.getStorageSync("mechineName")
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
  openNotice: function () {
    wx.navigateTo({
      url: '../notice/news',
    })

  },

  onChange(event) {
    const detail = event.detail;
    var that = this
    for (var i = 0; i < this.data.tags.length; i++) {
      this.setData({
        ['tags[' + i + '].checked']: 'false',
        ['tags[' + i + '].color']: 'white'
      })
    }
    this.setData({
      ['tags[' + event.detail.name + '].checked']: true,
      ['tags[' + event.detail.name + '].color']: 'red'
    })
    this.data.psCycle = event.target.dataset.id;
    this.data.zq = event.target.dataset.cycle;
    this.data.zqType = event.target.dataset.type;
    this.data.zqDis = event.target.dataset.num;
    if (this.data.zqType == "2")//代表赠送天数
    {
      that.setData({
        zsStr: this.data.zq + "天+送" + this.data.zqDis + "天"
      })
    } else {
      that.setData({
        zsStr: this.data.zq + "天+打" + this.data.zqDis + "折"
      })
    }
    that.calTotalMoney();
  },
  onChangeCycle(event) {
    const detail = event.detail;
    for (var i = 0; i < this.data.cycle.length; i++) {
      this.setData({
        ['cycle[' + i + '].checked']: 'false',
        ['cycle[' + i + '].color']: 'white'
      })
    }
    this.setData({
      ['cycle[' + event.detail.name + '].checked']: true,
      ['cycle[' + event.detail.name + '].color']: 'red'
    })
    this.data.psMode = event.target.dataset.id

  },
  handleChange1({ detail }) { // 改变选购数量
    this.setData({
      value1: detail.value
    })
  },
  //  点击日期组件确定事件  
  bindDateChange: function (e) {

    this.setData({
      dates: e.detail.value
    })
  },
  getUserInfo: function () {
    var that = this;
    wx.request({
      url: config.httpsUrl,
      data: {
        minOpenID: app.globalData.minOpenID,
        unionID: app.globalData.unionID,
        action: 'getMemberInfo'
      },
      method: "GET",
      header: {
        "Content-Type": "application/json"
      },
      success: function (res) {
        if (res.data.code == "200") {
          app.globalData.memberInfo = res.data.db
          wx.setStorageSync("userInfo", res.data.db)
        } else if (res.data.code == "400") {
          wx.showModal({
            title: '提示',
            content: res.data.msg,
            showCancel: false,
            success: function (res) {
              wx.navigateTo({
                url: '../login',
              })
            }
          })
        }
      }
    })
  },
  bindOKBtn: function () {

    var that = this;
    //首先验证是否绑定手机号 
    if (app.globalData.memberInfo[0].phone == "") {
      that.setData({
        memberInfoFlag: true
      })
      return;
    }
     
    if (this.data.psMode == "") {
      wx.showModal({
        title: '提示',
        content: '请选择配送模式',
        showCancel: false,
        success: function (res) {
          if (res.confirm) {
            console.log('用户点击确定')
          } else {
            console.log('用户点击取消')
          }

        }
      })
    }
    that.setData({
      loadFlag: true
    })
    wx.request({
      url: config.httpsUrl,
      data: {
        code: that.data.DGcode,
        psMode: this.data.psMode,
        unionID: app.globalData.unionID,
        openID:app.globalData.minOpenID,
        startDate: this.data.dates,
        companyID: app.globalData.companyID,
        mechineID: wx.getStorageSync("mechineID"),
        action: 'addDGOrder'
      },
      method: "GET",
      header: {
        "Content-Type": "application/json"
      },
      success: function (res) {
        console.log("兑换=" + JSON.stringify(res.data.msg) + "==" + res.data.code);
        if (res.data.code == "200") {
          that.setData({
            loadFlag: true
          })
          $Toast({
            content: "兑换成功",
            type: 'success'
          });

          ///////////
        } else {
          $Toast({
            content: res.data.msg,
            type: 'warning'
          });
        }
      }
    })
  },

  calTotalMoney: function () {
    var price = 0;
    var that = this
    price = this.data.productInfo[0].price0
    if (price <= 0) {

      wx.showModal({
        title: '提示',
        content: '产品价格获取失败',
        showCancel: false,
        success: function (res) {
          if (res.confirm) {
            console.log('用户点击确定')
          } else {
            console.log('用户点击取消')
          }
        }
      })
    }
    if (that.data.zqType == "1") {
      that.setData({
        totalMoney: (price * this.data.zq * this.data.zqDis / 10).toFixed(2)
      })
    } else {
      that.setData({
        totalMoney: (price * this.data.zq).toFixed(2)
      })
    }

  },
  loadProduct: function () {
    var that = this;
    wx.request({
      url: config.httpsUrl,
      data: {
        productID: that.data.product_ID,
        action: 'getProduct'
      },
      method: "GET",
      header: {
        "Content-Type": "application/json"
      },
      success: function (res) {
        if (res.data[0].tag != "" && res.data[0].tag != null) {
          res.data[0].tag = res.data[0].tag.split('|')
        }
        that.setData({
          productInfo: res.data,
          dates: that.getNowTime(res.data[0].startSend)
        })
        that.loadActivity();
      }
    })
  },

  loadActivity: function () {
    var that = this;
    wx.request({
      url: config.httpsUrl,
      data: {
        productID: that.data.product_ID,
        action: 'getProductActivity'
      },
      method: "GET",
      header: {
        "Content-Type": "application/json"
      },
      success: function (res) {

        that.setData({
          tags: res.data
        })
      }
    })
  },
  getPhoneNumber: function (e) {
    //获取用户信息
    // 登录
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
              if (res.data.openid != "" && res.data.openid!=undefined)
              {
                app.globalData.minOpenID = res.data.openid
              }
              if (res.data.unionid != "" && res.data.unionid!=undefined)
              {
                app.globalData.unionID = res.data.unionid
              }
              wx.setStorageSync("minOpenID", res.data.openid)
              wx.setStorageSync("unionID", res.data.unionid)
               
            }
          })
        }
      }
    })
    if (e.detail.errMsg == "getPhoneNumber:ok") {
      var that = this
      wx.request({
        url: config.httpsUrl,
        data: {
          action: 'getPhoneNum',
          code: app.globalData.code,
          iv: e.detail.iv,
          companyID: config.companyID,
          encryptedData: e.detail.encryptedData
        },
        method: "GET",
        header: {
          "Content-Type": "application/json"
        },
        success: function (res) {
          app.globalData.userPhone = res.data.phoneNumber
           
          wx.setStorageSync("userPhone", res.data.phoneNumber)
          wx.request({
            url: config.httpsUrl,
            data: {
              minOpenID: app.globalData.minOpenID,
              unionID: app.globalData.unionID,
              phone: res.data.phoneNumber,
              companyID: config.companyID,
              action: 'updatePhone'
            },
            method: "GET",
            header: {
              "Content-Type": "application/json"
            },
            success: function (res) {
              that.getUserInfo();
            }
          })
          that.setData({
            memberInfoFlag: false
          })


        }
      });
    }
  },
  getNowTime: function (days) {
    var now = new Date(new Date().setDate(new Date().getDate() + days));
    var year = now.getFullYear();
    var month = now.getMonth() + 1;
    now.setDate(now.getDate());
    var day = (now.getDate());

    if (month < 10) {
      month = '0' + month;
    };
    if (day < 10) {
      day = '0' + day;
    };

    var formatDate = year + '-' + month + '-' + day;
    return formatDate;
  },
  getProductByCode: function () {
    var that = this; 
    wx.request({
      url: config.httpsUrl,
      data: {
        minOpenID: app.globalData.minOpenID,
        unionID: app.globalData.unionID,
        companyID: app.globalData.companyID,
        code: that.data.DGcode,
        action: 'getProductByCode'
      },
      method: "GET",
      header: {
        "Content-Type": "application/json"
      },
      success: function (res) { 
        if (res.data.code == "200") { 
          that.setData({
            dgOrderInfo: res.data.dgOrder[0]
          })
        }  
      }
    })
  }

})