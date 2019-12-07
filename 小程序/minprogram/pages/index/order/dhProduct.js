const config = require('../../../config')
const { $Toast } = require('../../../dist/base/index');
const app = getApp()
Page({

  /**
   * 页面的初始数据
   */
  data: {
    hiddenmodalput: true,
    current_scroll: 'tab1',
    isShow: false,
    oldOrderNO:'',
    productInfo:'',
    mechineName:'',
    typeList:'',
    productList:'',
    newProductInfo:'',
    oldTotalMoney:'0',
    newTotalMoney:'0',
    tnum:'0',//当前剩余数量
    mechineID:'',
    oldproductID:'',
    newProductID:'',
    chaMoney:'0',//差值
    txtMoney:''
  },
  // 点击更换按钮
  replace() {
    this.setData({
      isShow: true
    });
  },

  // 切换产品分类 滚动
  handleChangeScroll({ detail }) {
    this.setData({
      current_scroll: detail.key
    });
  },
  // 关闭弹框
  offpopup() {
    this.setData({
      isShow: false
    });
  },
  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    var that=this
    wx.setNavigationBarTitle({
      title: "生鲜时逐",
    })
    that.setData({
      oldOrderNO: options.orderno
    })
    that.getProduct()
    that.onInitType()
    that.onInitProduct(0)
  },
  getProduct: function () {
   
    var that = this;
    wx.request({
      url: config.httpsUrl,
      data: {
        orderNO: that.data.oldOrderNO,
        companyID: app.globalData.companyID,
        action: 'getProductByOrderNO'
      },
      method: "GET",
      header: {
        "Content-Type": "application/json"
      },
      success: function (res) {
        if (res.data.code == "200") {
          that.setData({
            productInfo:res.data.db[0],
            mechineName: res.data.db[0].mechineName,
            mechineID: res.data.db[0].mechineID,
            tnum: res.data.db[0].tnum,
            oldTotalMoney: res.data.db[0].tnum * res.data.db[0].price,
            oldproductID: res.data.db[0].productID
          })
        }
      }
    })
  },
  onclickPro:function(e)
  {
    var that = this;
    if (this.data.currentTab == e.target.dataset.current) {
      return false;
    } else {
      that.setData({
        currentTab: e.target.dataset.current
      })
      that.onInitProduct(e.target.dataset.current)
    }
  },
  onInitType: function () {
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
          typeList: res.data
        })
        console.log(that.data.typeList)
      }
    })
  },
  onInitProduct: function (type) {
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
          if (res.data[i].tag != "" && res.data[i].tag != null) {
            res.data[i].tag = res.data[i].tag.split('|')
          }
        }
        that.setData({
          productList: res.data
        })
      }
    })
  },
  chg:function(e)
  {
    var productID = e.currentTarget.dataset.id;
    var that = this;
    wx.request({
      url: config.httpsUrl,
      data: {
        productID: productID,
        action: 'getProduct'
      },
      method: "GET",
      header: {
        "Content-Type": "application/json"
      },
      success: function (res) {
        if (res.data!="") {
         
          that.setData({
            newProductInfo: res.data[0],
            newTotalMoney: that.data.tnum * res.data[0].price0,
            newProductID: res.data[0].productID,
            chaMoney: (that.data.tnum * res.data[0].price0-that.data.oldTotalMoney).toFixed(2)
          })
         
          if (that.data.chaMoney>0)
         {
            that.setData({
              txtMoney:'付款：'
            })
         }else{
           that.setData({
             txtMoney: '退款：'
           })
         }
        }
        that.offpopup()
      }
    })
  },
  addOrder:function()
  {
    //判断是付款还是退款 如果是付款 发起支付  
    var that=this
    var totalMoney = that.data.chaMoney;
    
    if(totalMoney>0)//需要发起微信支付首先调后台添加订单方法
    {
      wx.request({
        url: config.httpsUrl,
        data: {
          unionID: app.globalData.unionID,
          companyID: app.globalData.companyID,
          mechineID: wx.getStorageSync("mechineID"),
          minOpenID:app.globalData.minOpenID,
          oldOrderNO: that.data.oldOrderNO,
          oldProductID:that.data.oldproductID,
          newProductID:that.data.newProductID,
          days: that.data.tnum,
          chaMoney: totalMoney,
          startDate: that.getNowTime(that.data.newProductInfo.startSend),
          action: 'dhProduct'
        },
        method: "GET",
        header: {
          "Content-Type": "application/json"
        },
        success: function (res) {
          if(res.data.code=="200")
          {
            //新生成订单的ID
            var orderID = res.data.orderID
            var activityID = res.data.activityID
            wx.request({
              url: config.httpsUrl,
              data: {
                openID: app.globalData.minOpenID,
                unionID: app.globalData.unionID,
                companyID: app.globalData.companyID,
                orderID: orderID,
                money: totalMoney,
                activityID: activityID,
                action: 'pay'
              },
              method: 'GET',
              success(res) {
                if (res.data.code == "200") {
                  const appId = res.data.payInfo.appId
                  wx.requestPayment({
                    timeStamp: res.data.payInfo.timeStamp,
                    nonceStr: res.data.payInfo.nonceStr,
                    package: res.data.payInfo.package,
                    signType: res.data.payInfo.signType,
                    paySign: res.data.payInfo.paySign
                  })
                } else if (res.data.code == "500") {
                  wx.showModal({
                    title: '提示',
                    content: res.data.msg,
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
              }
            })


          }
        }
      })
    }else{
      //需要补退给会员差价到余额
     
      wx.request({
        url: config.httpsUrl,
        data: {
          unionID: app.globalData.unionID,
          companyID: app.globalData.companyID,
          mechineID: wx.getStorageSync("mechineID"),
          minOpenID: app.globalData.minOpenID,
          oldOrderNO: that.data.oldOrderNO,
          oldProductID: that.data.oldproductID,
          newProductID: that.data.newProductID,
          days: that.data.tnum,
          chaMoney: totalMoney,
          startDate: that.getNowTime(that.data.newProductInfo.startSend),
          action: 'dhProduct'
        },
        method: "GET",
        header: {
          "Content-Type": "application/json"
        },
        success: function (res) {
           
          if (res.data.code == "200") {
            //新生成订单的ID
            var orderID = res.data.orderID
            var activityID = res.data.activityID
            $Toast({
              content: "兑换成功",
              type: 'success'
            });
          }else{
            $Toast({
              content: "兑换失败",
              type: 'warning'
            });
          }
        }
      })
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
  //点击按钮痰喘指定的hiddenmodalput弹出框
  modalinput: function () {
    this.setData({
      hiddenmodalput: !this.data.hiddenmodalput
    })
  },
  //取消按钮
  cancel: function () {
    this.setData({
      hiddenmodalput: true
    });
  },
  //确认
  confirm: function () {
    var that = this
     
  }

})