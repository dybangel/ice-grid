// pages/index/member/center.js
const config = require('../../../config')
const { $Toast } = require('../../../dist/base/index');
const app = getApp()
Page({

  /**
   * 页面的初始数据
   */
  data: {
    memberInfo: '',
    current: 'mine',
    day1: 10,
    day2: 15,
    day3: 25,
    day4: 0,
    phone:'',
    memberDj:'',
    hjhyDay:'0',
    hiddenmodalput: false,
    hiddenmodalput1: false,
    hiddenmodalput3:false,
    hiddenmodalput4:false,
    code:'',//订奶码
    productName:'',
    zq:'',
    productID:'',
    hiddenmodalput2: false,
    timer: '',//定时器名字
    countDownNum: '60',//倒计时初始值
    content: '发送验证码',
    setDis: false,
    yzm: '',
    serverYzm: '',
    newPhone: '',
    val:'',
    hidStr:'',//用于设置经纬度
    _longitude:'',//经度
    _latitude:'',//纬度
    _bh:'',
    _cardNO:'',
    _pwd:''
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
        url: 'center',
      })
    }
  },
  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    wx.setNavigationBarTitle({
      title: "我的个人中心",
    })
    var that = this
    // app.globalData.userPhone = wx.getStorageSync("userPhone")
    // app.globalData.minOpenID = wx.getStorageSync("minOpenID")
    // app.globalData.unionID = wx.getStorageSync("unionID")
    that.getUserInfo()
    that.getDJ() 
    that.getHidStr()
    if (wx.getStorageSync("userInfo") != "" && wx.getStorageSync("userInfo") != undefined) {
      if (wx.getStorageSync("userInfo").length == 1) {
        that.setData({
          memberInfo: wx.getStorageSync("userInfo")[0]
        })
      }
    } 
    that.setData({
      _longitude: wx.getStorageSync("longitude"),
      _latitude: wx.getStorageSync("latitude")
    })
 
  },
 
  /**
   * 生命周期函数--监听页面初次渲染完成
   */
  onReady: function () {

  },
  confirm2: function () {
    this.setData({
      hiddenmodalput2: true
    })
    var that = this
    if (that.data.newPhone=="")
    {
      $Toast({
        content: "请填写手机号",
        type: 'error'
      });
      return;
    }
    if (that.data.yzm == "") {
      $Toast({
        content: "请填写验证码",
        type: 'error'
      });
      return;
    }
    if (that.data.yzm != that.data.serverYzm) {
      $Toast({
        content: "验证码不正确",
        type: 'error'
      });
      return;
    }
  
    wx.request({
      url: config.httpsUrl,
      data: {
        minOpenID: app.globalData.minOpenID,
        unionID: app.globalData.unionID,
        phone: that.data.newPhone,
        companyID: config.companyID,
        yzm: that.data.yzm,
        action: 'updatePhone'
      },
      method: "GET",
      header: {
        "Content-Type": "application/json"
      },
      success: function (res) {

        if (res.data.code == "200") {
          that.getUserInfo()
          //需要重新更新全局会员信息
          $Toast({
            content: '成功',
            type: 'success'
          });
           that.setData({
             val:'',
             newPhone:'',
             yzm:''
           })
          wx.setStorageSync("userPhone", that.data.newPhone)
        } else {
          $Toast({
            content: res.data.msg,
            type: 'error'
          });
        }
      }
    })
   
  },
  bindYzm: function (e) {
    var that=this
    that.setData({
      yzm: e.detail.value
    })
  },
  bindPhone: function (e) {
    var that = this
    that.setData({
      newPhone: e.detail.value
    })
  },
  bindBH: function (e) {
    var that = this
    that.setData({
      _bh: e.detail.value
    })
  },

  sendMsg: function () {
    var that = this;
    if (that.data.newPhone == "") {
      $Toast({
        content: "请输入手机号",
        type: 'error'
      });
      return;
    }
    this.countDown();
    wx.request({
      url: config.httpsUrl,
      data: {
        unionID: that.data.memberInfo.unionID,
        openID: that.data.memberInfo.minOpenID,
        phone: that.data.newPhone,
        action: 'sendMessage'
      },
      method: "GET",
      header: {
        "Content-Type": "application/json"
      },
      success: function (res) {
        if (res.data.code == "200") {
          that.setData({
            serverYzm: res.data.msg
          })
          $Toast({
            content: '发送成功',
            type: 'success'
          });
        } else {
          $Toast({
            content: res.data.msg,
            type: 'error'
          });
        }
      }
    })
  },
  //取消按钮
  cancel2: function () {
    this.setData({
      hiddenmodalput2: false
    });
  },
  modalinput2: function () {
    this.setData({
      hiddenmodalput2: !this.data.hiddenmodalput1
    })
  },
  /**
   * 生命周期函数--监听页面显示
   */
  onShow: function () {
    var that=this
    that.setData({
      current: 'mine'
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
  onclick:function(e){
    let index = e.currentTarget.dataset['index'];
    if(index==1)
    {
      wx.navigateTo({
        url: "wallet",
      })
    }else if(index==2)
    {
      wx.navigateTo({
        url: "memberInfo",
      })
      
    } else if (index == 3) {
      wx.navigateTo({
        url: '../order/orderlist?tab=4',
      })
    } else if (index == 4) {
      wx.scanCode({
        success(res) {
          wx.request({
            url: config.httpsUrl,
            data: {
              mechineID: res.result,
              minOpenID: app.globalData.minOpenID,
              unionID: app.globalData.unionID,
              action: 'ch'
            },
            method: "GET",
            header: {
              "Content-Type": "application/json"
            },
            success: function (res) {

              if (res.data.code == "200") {
                $Toast({
                  content: res.data.msg,
                  type: 'success'
                });
              } else {
                $Toast({
                  content: res.data.msg,
                  type: 'warning'
                });
              }
            }
          })
        }
      })
    } else if (index == 5) {
      wx.navigateTo({
        url: '../order/getCode',
      })
    }else if(index==6)
    {

    }
  },
  getUserInfo:function()
  {
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
          wx.setStorageSync("userInfo", res.data.db)
          that.setData({
            memberInfo: res.data.db[0],
            memberDj: res.data.db[0].dj,
            hjhyDay: res.data.db[0].hjhyDays,
            phone:res.data.db[0].phone
           
          }) 
          if (res.data.db[0].headurl == "" || res.data.db[0].headurl==null)
          {
              that.setData({
                memberInfoFlag: true
              })
          }else{
            that.setData({
              memberInfoFlag:false
            })
          }
          app.globalData.memberInfo = res.data.db
        } else {
          // wx.showModal({
          //   title: '提示',
          //   content: res.data.msg,
          //   showCancel: false,
          //   success: function (res) {
          //     if (res.confirm) {
          //       console.log('用户点击确定')
          //     } else {
          //       console.log('用户点击取消')
          //     }
          //   }
          // })
          that.setData({
            memberInfoFlag:true
          })
        }
      }
    })
  },
  getDJ:function()
  {
    var that = this;
    wx.request({
      url: config.httpsUrl,
      data: {
        minOpenID: app.globalData.minOpenID,
        unionID: app.globalData.unionID,
        companyID:app.globalData.companyID,
        action: 'getDJList'
      },
      method: "GET",
      header: {
        "Content-Type": "application/json"
      },
      success: function (res) {
        if (res.data.code == "200") {
          that.setData({
            day1: res.data.db[0].consumeDay,
            day2: res.data.db[1].consumeDay,
            day3: res.data.db[2].consumeDay,
            day4: res.data.consumeDay
          })
        } 
      }
    })
  },
  getHidStr:function()
  { 
    var that = this;
    wx.request({
      url: config.httpsUrl,
      data: {
        companyID: app.globalData.companyID,
        action: 'getHidStr'
      },
      method: "GET",
      header: {
        "Content-Type": "application/json"
      },
      success: function (res) {
        if (res.data.code == "200") {
            
          that.data.hidStr = res.data.db
        }
      }
    })
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
      hiddenmodalput: false,
      hiddenmodalput1: false,
      hiddenmodalput4:false,
      hiddenmodalput3: false
    });
  },
  //确认
  confirm: function () {

  },
  bindCode:function(e)
  {
    this.setData({
      code: e.detail.value
    })
  },
  getProductByCode:function()
  {

    var that = this;
    console.log("code=" + that.data.code)
    console.log("hidStr=" + that.data.hidStr)
    if (that.data.code==that.data.hidStr&&that.data.code!="")
    {
      that.setData({
        hiddenmodalput3: !that.data.hiddenmodalput3
      })
    }else{
      wx.request({
        url: config.httpsUrl,
        data: {
          minOpenID: app.globalData.minOpenID,
          unionID: app.globalData.unionID,
          companyID: app.globalData.companyID,
          code: that.data.code,
          action: 'getProductByCode'
        },
        method: "GET",
        header: {
          "Content-Type": "application/json"
        },
        success: function (res) {
          if (res.data.code == "200") {
            console.log(JSON.stringify(res.data.db[0]));
            that.setData({
              productName: res.data.db[0].proName,
              zq: res.data.dgOrder[0].zq,
              productID: res.data.db[0].productID,
              hiddenmodalput: !that.data.hiddenmodalput,
              hiddenmodalput1: !that.data.hiddenmodalput1
            })
          } else {
            $Toast({
              content: res.data.msg,
              type: 'warning'
            });
          }
        }
      })
    }
   
  },
  setLocation:function()
  {
    var that = this
    wx.request({
      url: config.httpsUrl,
      data: {
        latitude:that.data._latitude,
        longitude:that.data._longitude,
        bh:that.data._bh,
        action: 'setLocation'
      },
      method: "GET",
      header: {
        "Content-Type": "application/json"
      },
      success: function (res) {
        if (res.data.code == "200") { 
          that.setData({
            hiddenmodalput: false,
            hiddenmodalput1: false,
            hiddenmodalput2: false,
            hiddenmodalput3: false
          })
        } else {
          $Toast({
            content: res.data.msg,
            type: 'warning'
          });
        }
      }
    })
  },
  bindCardNO:function(e)
  {
    var that=this
    that.setData({
      _cardNO: e.detail.value
    })
  },
  bindPwd:function(e){
    var that = this
    that.setData({
      _pwd: e.detail.value
    })
  },
  payCodeTC:function()
  {
    var that=this
    that.setData({
      hiddenmodalput4:true
    })
  },
  payCode:function()
  {
    var that = this
    if(that.data._cardNO=="")
    {
      $Toast({
        content: "充值卡号不能为空",
        type: 'warning'
      });
      return;
    }
    if (that.data._pwd == "") {
      $Toast({
        content: "密码不能为空",
        type: 'warning'
      });
      return;
    }
    wx.request({
      url: config.httpsUrl,
      data: {
        cardNO: that.data._cardNO,
        pwd: that.data._pwd,
        minOpenID: app.globalData.minOpenID,
        unionID: app.globalData.unionID,
        companyID: app.globalData.companyID,
        action: 'payCode'
      },
      method: "GET",
      header: {
        "Content-Type": "application/json"
      },
      success: function (res) {
        console.log("payCode=" + res.data.code);
        if (res.data.code == "200") {
          $Toast({
            content: "充值成功",
            type: 'success'
          });
          that.setData({
            hiddenmodalput4: false
          })
        } else {
          $Toast({
            content: res.data.msg,
            type: 'warning'
          });
        }
      }
    })
  },
  setInfo:function()
  {
     
    var that=this
    this.setData({
      hiddenmodalput1: !this.data.hiddenmodalput1
    });
    wx.navigateTo({
      url: '../order/dgcycle?productID=' + that.data.productID+"&code="+that.data.code,
    })
  },
  countDown: function () {
    let that = this;
    let countDownNum = that.data.countDownNum;//获取倒计时初始值
    //如果将定时器设置在外面，那么用户就看不到countDownNum的数值动态变化，所以要把定时器存进data里面
    that.setData({
      timer: setInterval(function () {//这里把setInterval赋值给变量名为timer的变量
        //每隔一秒countDownNum就减一，实现同步
        countDownNum--;
        //然后把countDownNum存进data，好让用户知道时间在倒计着
        that.setData({
          countDownNum: countDownNum,
          content: countDownNum + 's后重发',
          setDis: true
        })
        //在倒计时还未到0时，这中间可以做其他的事情，按项目需求来
        if (countDownNum == 0) {
          //这里特别要注意，计时器是始终一直在走的，如果你的时间为0，那么就要关掉定时器！不然相当耗性能
          //因为timer是存在data里面的，所以在关掉时，也要在data里取出后再关闭
          clearInterval(that.data.timer);
          that.setData({
            content: '发送验证码',
            setDis: false
          })
          //关闭定时器之后，可作其他处理codes go here
        }
      }, 1000)
    })
  }

})