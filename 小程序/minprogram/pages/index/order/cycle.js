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
    productInfo:'',
    value1: 1,
    dj:'',
    tags:'',
    zsStr:'',//用于显示总件数地方
    memberInfoFlag: false,
    buttonClicked: false,//限制按钮点击间隔
    cycle: [
      {
        name: '1日1送',
        checked: true,
        color: 'red',
        id:'1'
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
    psCycle:'',
    psMode:'1',
    num:'',
    product_ID: '',
    zq:'',
    totalMoney:0,
    loadFlag:false,
    zqType:'',
    zqDis:'0',
    showRight1: false,
    mechineList: '',
    mechineName: '',
    xjlc: '',
    address: ''
   
  },
  buttonClickedFunc: function (self) {
    self.setData({
      buttonClicked: true
    })
    setTimeout(function () {
      self.setData({
        buttonClicked: false
      })
    }, 20000)
  },
  handleClick: function () {
    this.setData({
      showRight1: !this.data.showRight1
    });
  },
  clickFormView: function (event) {
    var formId = event.detail.formId
    console.log("formId=" + formId);
    console.log("touser=" + app.globalData.minOpenID);
    console.log("companyID=" + app.globalData.companyID);
    wx.request({
      url: config.httpsUrl,//自己的服务接口地址
      method: "GET",
      header: {
        "Content-Type": "application/json"
      },
      data: {
        type: "3",
        formId: formId,
        companyID: app.globalData.companyID,
        touser: app.globalData.minOpenID,
        action: "sendTemplateMessage"
      },
      success: function (res) {
        console.log("模板消息发送成功");
      },
      fail: function (res) {
        console.log("模板消息发送失败");
      }
    })

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
    var that=this
    that.data.product_ID=options.productID
    wx.setNavigationBarTitle({
      title: wx.getStorageSync("mechineName")
    })
    that.loadProduct()
    that.setData({
      dj: app.globalData.memberDJ,
      
    })
    // wx.getLocation({
    //   success(res) {
    //     that.onInitMechineList(res.longitude, res.latitude)
    //   }
    // })
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
    that.getUserInfo()
    that.setData({
      address: wx.getStorageSync("address"),
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
  openNotice:function()
  {
    wx.navigateTo({
      url: '../notice/news',
    })

  },
  oninitTags:function(id,zq,type,num)
  {
    var that=this
    this.setData({
      ['tags[0].checked']: 'false',
      ['tags[0].color']: 'white'
    })
    this.setData({
      ['tags[0].checked']: true,
      ['tags[0].color']: 'red'
    }) 
    
    this.data.psCycle = id;
    this.data.zq = zq;
    this.data.zqType = type;
    this.data.zqDis = num;
    if (this.data.zqType == "2")//代表赠送天数
    {
      that.setData({
        zsStr: this.data.zq + "天+送" + this.data.zqDis + "天"
      })
    }else{
      that.setData({
        zsStr: this.data.zq + "天+打" + this.data.zqDis + "折"
      })
    }
    that.calTotalMoney();
  },
  onChange(event) {
    const detail = event.detail;
    var that=this 
    for(var i=0;i<this.data.tags.length;i++)
    {

      if (i == event.detail.name) {
        this.setData({
          ['tags[' +i + '].checked']: true,
          ['tags[' + i + '].color']: 'red'
        }) 
      } else {
         this.setData({
        ['tags[' + i + '].checked']: 'false' ,
        ['tags[' + i + '].color']: 'white' 
      })
      }
       

    }
   
    this.data.psCycle = event.target.dataset.id;
    this.data.zq = event.target.dataset.cycle;
    this.data.zqType = event.target.dataset.type;
    this.data.zqDis = event.target.dataset.num;
    if (this.data.zqType=="2")//代表赠送天数
    {
      that.setData({
        zsStr: this.data.zq + "天+送" + this.data.zqDis+"天"
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
  bindOKBtn:function(){
    var that=this;
    that.buttonClickedFunc(that); 
    //首先验证是否绑定手机号 
    if (app.globalData.memberInfo[0].phone=="")
    {
      that.setData({
        memberInfoFlag:true
      })
      return;
    }
    if (this.data.psCycle=="")
    { 
      wx.showModal({
        title: '提示',
        content:'请选择配送周期',
        showCancel: false,
        success: function (res) {
          if (res.confirm) {
            console.log('用户点击确定')
          } else {
            console.log('用户点击取消')
          }

        }
      })
      return;
    }
    if(this.data.psMode=="")
    {
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
      return;
    }
    that.setData({
      loadFlag:true
    })
    wx.request({
      url: config.httpsUrl,
      data: {
        psCycleID: this.data.psCycle,
        psMode: this.data.psMode,
        unionID: app.globalData.unionID,
        openID:app.globalData.minOpenID,
        productID: that.data.product_ID,
        startDate: this.data.dates,
        companyID: app.globalData.companyID,
        mechineID: wx.getStorageSync("mechineID"),
        totalMoney:this.data.totalMoney,
        action: 'addOrder'
      },
      method: "GET",
      header: {
        "Content-Type": "application/json"
      },
      success: function (res) {
        if(res.data.code=="200")
        {
          //跳转支付
          that.setData({
            loadFlag: false
          })
        ////////////
           
          wx.request({
            url: config.httpsUrl,
            data: {
              openID: app.globalData.minOpenID,
              unionID:app.globalData.unionID,
              companyID:app.globalData.companyID,
              orderID: res.data.orderID,
              money: that.data.totalMoney,
              activityID: that.data.psCycle,
              action: 'pay'
            },
            method: 'GET',
            success(res) {
              if(res.data.code=="200")
              {
                const appId = res.data.payInfo.appId
                wx.requestPayment({
                  timeStamp: res.data.payInfo.timeStamp,
                  nonceStr: res.data.payInfo.nonceStr,
                  package: res.data.payInfo.package,
                  signType: res.data.payInfo.signType,
                  paySign: res.data.payInfo.paySign,
                  success: function (res) {
                    wx.navigateTo({
                      url: 'paysuccess',
                    })
                  },
                  fail:function()
                  {

                  }
                })
              }else if(res.data.code=="500")
              {
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

 

          ///////////
        }else{
          $Toast({
            content: res.data.msg,
            type: 'warning'
          });
        }
      }
    })
  },
  
  calTotalMoney:function()
  {
    var price = 0;
    var that=this
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
    if(that.data.zqType=="1")
    {
      that.setData({
        totalMoney: (price * this.data.zq*this.data.zqDis/10).toFixed(2)
      })
    }else{
      that.setData({
        totalMoney: (price * this.data.zq).toFixed(2)
      })
    }
 
  },
  loadProduct:function()
  {
    var that=this
    if (that.data.product_ID==undefined)
    {
      return;
    }
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
        console.log("派送天数="+res.data[0].startSend);
        console.log("派送日期=" + that.getNowTime(res.data[0].startSend));
        that.setData({
          productInfo: res.data,
          dates: that.getNowTime(res.data[0].startSend)
        })
        that.loadActivity();
      }
    })
  },
  
  loadActivity:function()
  {
    var that = this;
    wx.request({
      url: config.httpsUrl,
      data: {
        productID: that.data.product_ID,
        mechineID: wx.getStorageSync("mechineID"),
        action: 'getProductActivity'
      },
      method: "GET",
      header: {
        "Content-Type": "application/json"
      },
      success: function (res) {
        console.log("tags=" + JSON.stringify(res.data) + "length=" + res.data.length);
        that.setData({
          tags:res.data
        })
        if (res.data.length>0)
        {
          that.oninitTags(res.data[0].id, res.data[0].zq, res.data[0].type, res.data[0].num)
        }
       
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
              app.globalData.minOpenID = res.data.openid
              app.globalData.unionID = res.data.unionid
               
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
            memberInfoFlag:false
          })


        }
      });
    } 
  },
  getNowTime: function (days)
  {
    var now = new Date(new Date().setDate(new Date().getDate() +days));
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
   
})