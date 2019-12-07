// pages/index/member/memberInfo.js
const config = require('../../../config')
const { $Toast } = require('../../../dist/base/index');
const app = getApp()
Page({
  /**
   * 页面的初始数据
   */
  data: {
    memberInfo: '',
    dates:'1900-01-01',
    hiddenmodalput: false,
    hiddenmodalput1: false,
    hiddenmodalput2: false,
    name:'',//用于记录会员输入的姓名
    pwd:'',
    yzm:'',
    serverYzm:'',
    timer: '',//定时器名字
    countDownNum: '60',//倒计时初始值
    content:'发送验证码',
    setDis:false,
    newPhone:'',
    memberName:''
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
      hiddenmodalput: !this.data.hiddenmodalput
    });
  },
  //确认
  confirm: function () {
    this.setData({
      hiddenmodalput: !this.data.hiddenmodalput
    })
    var that = this
    wx.request({
      url: config.httpsUrl,
      data: {
        minOpenID: app.globalData.minOpenID,
        unionID:app.globalData.unionID,
        name:that.data.name,
        companyID:config.companyID,
        action: 'updateMemberName'
      },
      method: "GET",
      header: {
        "Content-Type": "application/json"
      },
      success: function (res) {
         if(res.data.code=="200")
         {
           //需要重新更新全局会员信息
           // that.getUserInfo()
          //  $Toast({
          //    content: '成功',
          //    type: 'success'
          //  });
           that.setData({
             memberName: that.data.name
          })
         }else{
           $Toast({
             content: res.data.msg,
             type: 'error'
           });
         }
      }
    })
  },
  
  modalinput1: function () {
    this.setData({
      hiddenmodalput1: !this.data.hiddenmodalput1
    })
  },
  modalinput2: function () {
    this.setData({
      hiddenmodalput2: !this.data.hiddenmodalput1
    })
  },
  confirm2: function () {
    this.setData({
      hiddenmodalput2: !this.data.hiddenmodalput2
    })
    var that = this
    if(that.data.yzm=="")
    {
      $Toast({
        content: "请填写验证码",
        type: 'error'
      });
      return;
    }
    if (that.data.yzm != that.data.serverYzm)
    {
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
        companyID:config.companyID,
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
  //取消按钮
  cancel2: function () {
    this.setData({
      hiddenmodalput2: !this.data.hiddenmodalput2
    });
  },
  //取消按钮
  cancel1: function () {
    this.setData({
      hiddenmodalput1: !this.data.hiddenmodalput1
    });
  },
  //确认
  confirm1: function () {
    this.setData({
      hiddenmodalput1: !this.data.hiddenmodalput1
    })
    var that = this
    wx.request({
      url: config.httpsUrl,
      data: {
        minOpenID: app.globalData.minOpenID,
        unionID: app.globalData.unionID,
        pwd: this.data.pwd,
        yzm:this.data.yzm,
        action: 'updatePwd'
      },
      method: "GET",
      header: {
        "Content-Type": "application/json"
      },
      success: function (res) {
        if (res.data.code == "200") {
          //需要重新更新全局会员信息
         // that.getUserInfo()
          // $Toast({
          //   content: '成功',
          //   type: 'success'
          // });
         

        } else {
          $Toast({
            content: res.data.msg,
            type: 'error'
          });
        }
      }
    })
  },
  getUserInfo: function () {
    var that = this;
    wx.request({
      url: config.httpsUrl,
      data: {
        minOpenID: app.globalData.minOpenID,
        unionID:app.globalData.unionID,
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
            memberName:res.data.db[0].name,
            memberInfo:res.data.db
          })
          if (res.data.db[0].birthday != null && res.data.db[0].birthday!="")
          {
            console.log("生日=" + res.data.db[0].birthday);
            that.setData({
              dates: res.data.db[0].birthday
            })
          }
          app.globalData.memberInfo = res.data.db
        }  
      }
    })
  },
  bindName:function(e)
  {
    this.setData({
      name: e.detail.value
    })
  },
  bindPhone:function(e){
    this.setData({
      newPhone: e.detail.value
    })
  },
  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    var that = this 
    that.setData({
      memberInfo: wx.getStorageSync("userInfo")[0]
    })
    if (that.data.memberInfo.birthday != null)
    { 
      that.setData({
        dates: wx.getStorageSync("userInfo")[0].birthday,
        memberName: wx.getStorageSync("userInfo")[0].name
      })
    }
    wx.setNavigationBarTitle({
      title: "个人资料",
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
    var that=this
     
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
  //  点击日期组件确定事件  
  bindDateChange: function (e) {
    this.setData({
      dates: e.detail.value
    })
    var that = this
    wx.request({
      url: config.httpsUrl,
      data: {
        birthday: that.data.dates,
        minOpenID: app.globalData.minOpenID,
        unionID:app.globalData.unionID,
        action: 'updateMemberBirthday'
      },
      method: "GET",
      header: {
        "Content-Type": "application/json"
      },
      success: function (res) {
        console.log("res=" + res.data.code );
        if (res.data.code == "200") {
          //需要重新更新全局会员信息
          // that.getUserInfo()
          // $Toast({
          //   content: '成功',
          //   type: 'success'
          // });
        } else {
          $Toast({
            content: res.data.msg,
            type: 'error'
          });
        }
      }
    })
  },
  bindPwd:function(e)
  {
    this.setData({
      pwd: e.detail.value
    })
  },
  bindYzm:function(e)
  {
    this.setData({
      yzm: e.detail.value
    })
  },
  sendMsg:function()
  {
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
        unionID: app.globalData.unionID,
        openID:app.globalData.openID,
        phone: that.data.newPhone,
        action: 'sendMessage'
      },
      method: "GET",
      header: {
        "Content-Type": "application/json"
      },
      success: function (res) {
         if(res.data.code=="200")
         {
           that.setData({
             serverYzm:res.data.msg
           })
           $Toast({
             content: '发送成功',
             type: 'success'
           });
         }else{
           $Toast({
             content: res.data.msg,
             type: 'error'
           });
         }
      }
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
          content: countDownNum+'s后重发',
          setDis:true
        })
        //在倒计时还未到0时，这中间可以做其他的事情，按项目需求来
        if (countDownNum == 0) {
          //这里特别要注意，计时器是始终一直在走的，如果你的时间为0，那么就要关掉定时器！不然相当耗性能
          //因为timer是存在data里面的，所以在关掉时，也要在data里取出后再关闭
          clearInterval(that.data.timer);
          that.setData({
            content: '发送验证码',
            setDis:false
          })
          //关闭定时器之后，可作其他处理codes go here
        }
      }, 1000)
    })
  }

 

})