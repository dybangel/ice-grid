// pages/index/member/wallet.js
const config = require('../../../config')
const util = require('../../../utils/util.js')
const { $Toast } = require('../../../dist/base/index');
const app = getApp()
Page({
  /**
   * 页面的初始数据
   */
  data: {
    memberInfo:'',
    payActivityList:'',
    inputMoney:'',//充值自定义金额
    phone:'',
    money:'',//转账金额
    hiddenmodalput: false,
    memberInfoFlag: false,
    tqlist:'',
    _unionID:'',
    _minOpenID:'',
    val:'',
    _phone:'',
    bz:'',//转账备注
    buttonClicked: false,//限制按钮点击间隔
     buttonClicked1: false,//限制按钮点击间隔
    hiddenmodalput4:false,
    _cardNO: '',
    _pwd: ''
  },
  buttonClickedFunc:function(self) {
    self.setData({
      buttonClicked: true
    })
  setTimeout(function() {
      self.setData({
        buttonClicked: false
      })
    }, 5000)
  },
  buttonClickedFunc1: function (self) {
    self.setData({
      buttonClicked1: true
    })
    setTimeout(function () {
      self.setData({
        buttonClicked1: false
      })
    }, 10000)
  },
  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    var that=this;
    wx.setNavigationBarTitle({
      title: "我的钱包",
    });
    if (wx.getStorageSync("userInfo") != "" && wx.getStorageSync("userInfo") != undefined && wx.getStorageSync("unionID") != ""
      && wx.getStorageSync("minOpenID") != "" && wx.getStorageSync("minOpenID") != undefined) {
    }else{
      wx.redirectTo({
        url: '../login',
      })
    }

    console.log("_minOpenID=" + wx.getStorageSync("minOpenID"));
    console.log("_unionID=" + wx.getStorageSync("unionID"));
    if (wx.getStorageSync("unionID")!= "" && wx.getStorageSync("unionID")!=null)
     {
      that.setData({
        _minOpenID: wx.getStorageSync("minOpenID"),
        _unionID: wx.getStorageSync("unionID")
      })
     }else{
       wx.navigateTo({
         url: '../login',
       })
     }
    if (wx.getStorageSync("userInfo") != "" && wx.getStorageSync("userInfo") != undefined && wx.getStorageSync("userInfo")!=null)
     {
       
      if (wx.getStorageSync("userInfo") != "" && wx.getStorageSync("userInfo")!=undefined)
       {
        if (wx.getStorageSync("userInfo").length==1)
         {
          that.setData({
            _phone: wx.getStorageSync("userInfo")[0].phone
          })
         }
        
       }
     }
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
              action: "userlogin2"
            },
            method: "GET",
            header: {
              "Content-Type": "application/json"
            },
            success: function (res) {
              if (res.data.code == "200") {
                if (res.data.db.openid != "" && res.data.db.openid!=undefined)
                {
                  app.globalData.minOpenID = res.data.db.openid
                  wx.setStorageSync("minOpenID", res.data.db.openid)
                }
                if (res.data.db.unionid != "" && res.data.db.unionid!=undefined)
                {
                  app.globalData.unionID = res.data.db.unionid
                  wx.setStorageSync("unionID", res.data.db.unionid)
                }
                
              } else {
                $Toast({
                  content: res.data.msg,
                  type: 'error'
                });
              }
            }
          })
        } else {

        }
      }
    }) 
    that.getMemberInfo();
    that.getPayActivityList()
    that.getTQList()
  },
  // clickFormView: function (event) {
  //   var formId = event.detail.formId
  //   console.log("formId=" + formId);
  //   console.log("touser=" + app.globalData.minOpenID);
  //   console.log("companyID=" + app.globalData.companyID);
  //   wx.request({
  //     url: config.httpsUrl,//自己的服务接口地址
  //     method: "GET",
  //     header: {
  //       "Content-Type": "application/json"
  //     },
  //     data: {
  //       type: "2",
  //       formId: formId,
  //       companyID: app.globalData.companyID,
  //       touser: app.globalData.minOpenID,
  //       action: "sendTemplateMessage"
  //     },
  //     success: function (res) {
  //       console.log("模板消息发送成功");
  //     },
  //     fail: function (res) {
  //       console.log("模板消息发送失败");
  //     }
  //   })

  // },
  /**
  * 生命周期函数--监听页面显示
  */
  onShow: function () {
     
  },
  getTQList:function()
  {
    var that=this
    wx.request({
      url: config.httpsUrl,
      data: {
        companyID: config.companyID,
        action: 'getTQList'
      },
      method: "GET",
      header: {
        "Content-Type": "application/json"
      },
      success: function (res) {
         if(res.data.code=="200")
         {
           that.setData({
             tqlist:res.data.db
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
  onclick:function(e)
  {
    let index = e.currentTarget.dataset['index'];
    if(index==1)
    {
     
    }else if(index==2)
    {
      wx.navigateTo({
        url: 'record',
      })
    }
  },
  getMemberInfo:function()
  {
    var that = this;
    console.log("that.data._minOpenID=" + that.data._minOpenID);
    console.log("that.data._unionID=" + that.data._unionID);
    if (that.data._minOpenID == "" || that.data._unionID=="")
    {
      wx.redirectTo({
        url: '../login',
      })
      return;
    }
    wx.request({
      url: config.httpsUrl,
      data: {
        minOpenID:that.data._minOpenID,
        unionID: that.data._unionID,
        action: 'getMemberInfo'
      },
      method: "GET",
      header: {
        "Content-Type": "application/json"
      },
      success: function (res) {
        if(res.data.code=="200")
        {
          that.setData({
            memberInfo: res.data.db[0]
          })
          app.globalData.memberInfo = res.data.db
          wx.setStorageSync("userInfo", res.data.db)
        }else if(res.data.code=="400")
        {
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
        }else{
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
  },
  getPayActivityList:function()
  {
    var that = this;
    if (app.globalData.companyID=="")
    {
      wx.redirectTo({
        url: '../login',
      })
      return;
    }
    wx.request({
      url: config.httpsUrl,
      data: {
        companyID: app.globalData.companyID,
        action: 'getPayActivityList'
      },
      method: "GET",
      header: {
        "Content-Type": "application/json"
      },
      success: function (res) {
        if (res.data.code == "200") {
          that.setData({
            payActivityList: res.data.db
          })
        } 
         else if(res.data.code=="500"){
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
  },
  bindMoney:function(e)
  {
    var that=this
    that.setData({
      inputMoney: e.detail.value
    })
  },
  bindMoney2: function (e) {
    var that = this
    that.setData({
      money: e.detail.value
    })
  },
  bindbz: function (e) {
    var that = this
    that.setData({
      bz: e.detail.value
    })
  },
  isInteger: function (obj) {
   
  },
  payOK:function(){
    var that=this;
     
    that.buttonClickedFunc1(that);
    var r = /^\+?[1-9][0-9]*$/;　　//正整数
    if (r.test(that.data.inputMoney) && parseFloat(that.data.inputMoney) > 0){
      
      that.payMethod('0', that.data.inputMoney, that.data.inputMoney);
    }else{
      wx.showModal({
        title: '提示',
        content: "充值金额必须是整数",
        showCancel: false,
        success: function (res) {

        }
      })
    }
 
  },
  
  payMethod: function (id, int_money,dz_Money)
  {
     
    var that=this
    //验证是否绑定手机号
    if (that.data._phone == "" || that.data._phone==null)
    {
      that.setData({
        memberInfoFlag: true
      })
      return;
    }
    if (int_money <= 0) {
      wx.showModal({
        title: '提示',
        content: "充值金额必须大于0",
        showCancel: false,
        success: function (res) {

        }
      })
    }
    wx.request({
      url: config.httpsUrl,
      data: {
        id:id,
        openID: app.globalData.minOpenID,
        companyID: app.globalData.companyID,
        unionID:app.globalData.unionID,
        money: int_money,
        dzMoney: dz_Money,
        action: 'payCZ'
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
            paySign: res.data.payInfo.paySign,
            success:function()
            {
              that.setData({
                inputMoney:""
              })
              wx.redirectTo({
                url: 'success',
              })
            }
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
  },
  serviceInfo:function()
  {
    wx.navigateTo({
      url: '../notice/news',
    })
  },
  onclickPay: function (event)
  {
    //限制两次点击时间间隔
    var that = this
    that.buttonClickedFunc(that); 
    var id = event.target.dataset.id;
    var money = event.target.dataset.money;
    var dzMoney = event.target.dataset.dzmoney; 
    that.payMethod(id, money, dzMoney);
  },
  bindPhone:function(e)
  {
    var that = this
    that.setData({
      phone: e.detail.value
    })
  },
  backCenter:function()
  {
    wx.navigateTo({
      url: 'center',
    }) 
  },
  //点击按钮痰喘指定的hiddenmodalput弹出框
  modalinput: function () {
    var that=this
    that.setData({
      hiddenmodalput: !that.data.hiddenmodalput
    })
  },
  modalinputCZ:function()
  {
    var that = this
    that.setData({
      hiddenmodalput4: !that.data.hiddenmodalput4
    })
  },
  bindCardNO: function (e) {
    var that = this
    that.setData({
      _cardNO: e.detail.value
    })
  },
  bindPwd: function (e) {
    var that = this
    that.setData({
      _pwd: e.detail.value
    })
  },
  payCode: function () {
    var that = this
    if (that.data._cardNO == "") {
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
  //取消按钮
  cancel: function () {
    var that = this
    that.setData({
      hiddenmodalput: false,
      hiddenmodalput4: false,
      val:''
    });
  },
  //确认
  confirm: function () {
   var that=this
    if (parseFloat(that.data.money)<=0)
   {
      $Toast({
        content: "请输入正确的转账金额",
        type: 'error'
      });
      return;
   }
    wx.showModal({
      title: '提示',
      content: '账号：'+that.data.phone+"\n"+"金额："+that.data.money,
      success: function (sm) {
        if (sm.confirm) {
          wx.request({
            url: config.httpsUrl,
            data: {
              minOpenID: app.globalData.minOpenID,
              companyID: app.globalData.companyID,
              unionID:app.globalData.unionID,
              money: that.data.money,
              phone: that.data.phone,
              bz:that.data.bz,
              action: 'transferAccounts'
            },
            method: 'GET',
            success(res) { 
               if(res.data.code=="200")
               {
                 $Toast({
                   content: res.data.msg,
                   type: 'success'
                 });
                 that.setData({
                   hiddenmodalput: !that.data.hiddenmodalput
                  })
                  //清空
                  that.setData({
                    val:''
                  });
                 that.getMemberInfo()
               }else{
                 $Toast({
                   content: res.data.msg,
                   type: 'error' 
                 });
               }
            }
          })
        } else if (sm.cancel) {
          //清空
          that.setData({
            val: ''
          });
          console.log('用户点击取消')
        }
      }
    })
    
    // this.setData({
    //   hiddenmodalput: true
    // })
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
              if (res.data.unionid != "" && res.data.unionid!=undefined)
              {
                app.globalData.unionID = res.data.unionid
              }
              if (res.data.openid != "" && res.data.openid!=undefined)
              {
                app.globalData.minOpenID = res.data.openid
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
          that.data._phone = res.data.phoneNumber
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
              that.getMemberInfo()
            }
          })
          that.setData({
            memberInfoFlag: false
          })
        }
      });
    }
  },
})