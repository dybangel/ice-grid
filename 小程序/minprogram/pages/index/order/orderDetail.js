const config = require('../../../config')
const util = require('../../../utils/util')
const { $Toast } = require('../../../dist/base/index');
const app = getApp()
var Moment = require("moment.js");
var DATE_YEAR = new Date().getFullYear();
var DATE_MONTH = new Date().getMonth() + 1;
var DATE_DAY = new Date().getDate();
Page({
  data: {
    year: '',
    month: '',
    day: '',
    days: {},
    systemInfo: {},
    weekStr: ['日', '一', '二', '三', '四', '五', '六'],
    checkDate: [] ,
    showDialog: false,
    dates: '',
    zsproductflag:false,
    orderInfo:'',
    strArr:{},
    orderNO:'',
    mechineID:'',
    orderID:'',
    olddateTime:'',
    startSend:'',
    orderTime:''

  },
  //加载订单明细
  loadOrderDetail: function (orderno, mechineID,year,month)
  {
   
    var that=this
    wx.request({
      url: config.httpsUrl,
      data: {
        orderNO: orderno,
        companyID: app.globalData.companyID,
        mechineID: mechineID,
        minOpenID: app.globalData.minOpenID,
        unionID:app.globalData.unionID,
        dateTime: year + "-" + month,
        action: 'getOrderDetail'
      },
      method: "GET",
      header: {
        "Content-Type": "application/json"
      },
      success: function (res) {
        console.log("res.data.code=" + res.data.code);
        if (res.data.code == "200") {
          
          let dateArr = []; //需要遍历的日历数组数据
          let len=res.data.db.length
          for(var i=0;i<len;i++)
          {
            var zt = res.data.db[i].zt
            zt= zt=="1"?"已完成":zt=="2"?"已失效":zt=="3"?"已转售":zt=="4"?"待取货":zt=="5"?"待配送":zt=="6"?"已售出":zt=="8"?"暂停":zt=="7"?"已更换":""
            dateArr.push({
              day: res.data.db[i].dates,
              amount: zt
            })
            that.setData({
              strArr:dateArr
            })
            console.log(+"dateArr="+dateArr);
            that.createDateListData(year,month,dateArr);
          }
        } else {
          let dateArr = []; //需要遍历的日历数组数据
          that.createDateListData(year, month, dateArr);
        }
      }
    })
  },
  //加载产品
  loadOrderInfo: function (orderID) {
    var that = this;
    wx.request({
      url: config.httpsUrl,
      data: {
        orderID: orderID,
        action: 'getOrderInfo'
      },
      method: "GET",
      header: {
        "Content-Type": "application/json"
      },
      success: function (res) { 
        if(res.data.code=="200")
        {
          
          that.setData({
            orderInfo: res.data.db[0],
            startSend: res.data.db[0].startSend,
            orderTime: res.data.db[0].createTime.substring(0, 19).replace("T", " ")
          })

        }else{
          $Toast({
            content: res.data.msg,
            type: 'warning'
          });
        }
      }
    })
  },
  
  //  点击日期组件确定事件  
  bindDateChange: function (e) {
    var that=this
    var days = that.data.dates
    if (that.data.startSend != "" && that.data.startSend != null) {
      var time1 = new Date(days);//获取点击的日期-该件产品设置的首送日期天数
      if (time1 > new Date(this.getNowTime(that.data.startSend))) {
        
      } else {
        wx.showModal({
          title: '提示',
          content: "首送日期内无法延期",
          showCancel: true,
          success: function (res) {

          }
        })
        return;
      }
    }
    if (new Date(days) > new Date(e.detail.value))
    {
      wx.showModal({
        title: '提示',
        content: "请选择"+days+"之后日期",
        showCancel: true,
        success: function (res) {

        }
      })
      return;
    }
    this.setData({
      dates: e.detail.value
    })
  },
  zsproduct:function()
  {
    var that=this
    that.setData({
      zsproductflag: !this.data.zsproductflag
    })
  },
  onLoad: function (options) {
    var _this = this;
    let now = new Date();
    let year = now.getFullYear();
    let month = now.getMonth() + 1;
    // 页面初始化 options为页面跳转所带来的参数
    var that = this
    that.setData({
      orderNO: options.orderNO,
      mechineID: options.mechineID,
      orderID: options.orderID
    })
    if (options.orderID!=undefined)
    {
      that.loadOrderInfo(options.orderID)
    }
    
    that.loadOrderDetail(options.orderNO, options.mechineID, year, month)
    this.setData({
      year: year,
      month: month
    })
    wx.getSystemInfo({
      success: function (res) {
        that.setData({
          systemInfo: res,
        });
      }
    })
  },
  onReady: function () {
    // 页面渲染完成
  },
  onShow: function () {

  },
  /**创建日历数据 */
  createDateListData: function (setYear, setMonth, strArr) {
    //全部时间的月份都是按0~11基准，显示月份才+1
    let dateArr = []; //需要遍历的日历数组数据
    let arrLen = 0; //dateArr的数组长度
    let now = setYear ? new Date(setYear, setMonth) : new Date();
    let year = setYear || now.getFullYear();
    let nextYear = 0;
    let month = setMonth || now.getMonth();
    //没有+1方便后面计算当月总天数
    let nextMonth = (month + 1) > 11 ? 1 : (month);
    console.log("当前选中月nextMonth：" + nextMonth);
    //目标月1号对应的星期
    let startWeek = this.getWeek(year, nextMonth, 1); //new Date(year + ',' + (month + 1) + ',' + 1).getDay();  
    console.log("目标月1号对应的星期startWeek:" + startWeek);
    //获取目标月有多少天
    let dayNums = this.getTotalDayByMonth(year, nextMonth); //new Date(year, nextMonth, 0).getDate();         
    console.log("获取目标月有多少天dayNums:" + dayNums);
    let obj = {};
    let num = 0;
    var that=this
    if (month + 1 > 11) {
      nextYear = year + 1;
      dayNums = new Date(nextYear, nextMonth, 0).getDate();
    }
    for (var j = -startWeek + 1; j <= dayNums; j++) {
      var tempWeek = -1;
      if (j > 0) {
        tempWeek = this.getWeek(year, nextMonth, j);
       
      }
      var clazz = '';
      if (tempWeek == 0 || tempWeek == 6)
        clazz = 'week'
      if (j < DATE_DAY && year == DATE_YEAR && nextMonth == DATE_MONTH)
        //当天之前的日期不可用
        clazz = 'unavailable ' + clazz;
      else
        clazz = '' + clazz
      /**如果当前日期已经选中，则变色 */
      var date = year + "-" + nextMonth + "-" + j;
      var index = this.checkItemExist(this.data.checkDate, date);
      if (index != -1) {
        //clazz = clazz + ' active';
      }
      if(strArr!='')
      {
        dateArr.push({
          day: j,
          class: clazz,
          amount: that.getStr(j)
        })
      }else{
        dateArr.push({
          day: j,
          class: '',
          amount: ''
        })
      }
    }
    this.setData({
      days: dateArr
    })
  },
  getStr:function(j)
  {
    var that=this
    var len=that.data.strArr.length;
    for(var i=0;i<len;i++)
    {
      if(that.data.strArr[i].day==j)
      {
        console.log("amount=" + that.data.strArr[i].amount);
        return that.data.strArr[i].amount
      }
    }
   return '';
  },
  /**
   * 上个月
   */
  lastMonthEvent: function () {
    //全部时间的月份都是按0~11基准，显示月份才+1
    let year = this.data.month - 2 < 0 ? this.data.year - 1 : this.data.year;
    let month = this.data.month - 2 < 0 ? 11 : this.data.month - 2;
    this.setData({
      year: year,
      month: (month + 1)
    })
    var that=this
    that.loadOrderDetail(that.data.orderNO, that.data.mechineID,year,month+1)
  },
  /**
   * 下个月
   */
  nextMonthEvent: function () {
    //全部时间的月份都是按0~11基准，显示月份才+1
    let year = this.data.month > 11 ? this.data.year + 1 : this.data.year;
    let month = this.data.month > 11 ? 0 : this.data.month;
    this.setData({
      year: year,
      month: (month + 1)
    })
    var that = this
    that.loadOrderDetail(that.data.orderNO, that.data.mechineID, year, month+1)
  },
 
  /*
   * 获取月的总天数
   */
  getTotalDayByMonth: function (year, month) {
    month = parseInt(month, 10);
    var d = new Date(year, month, 0);
    return d.getDate();
  },
  /*
   * 获取月的第一天是星期几
   */
  getWeek: function (year, month, day) {
    var d = new Date(year, month - 1, day);
    return d.getDay();
  },
  /**
   * 点击日期事件
   */
  onPressDateEvent: function (e) {
    var that=this
    var {
      year,
      month,
      day,
      amount
    } = e.currentTarget.dataset;
    console.log("当前点击的日期：" + year + "-" + month + "-" + day + ";amount=" + amount);
   

    

    //当前选择的日期为同一个月并小于今天，或者点击了空白处（即day<0），不执行
    if ((day < DATE_DAY && month == DATE_MONTH) || day <= 0)
      return;
    this.setData({
      dates: year + "-" + month + "-" + day,
      olddateTime: year + "-" + month + "-" + day
    })
    if(amount!='')
    {
      this.toggleDialog();
    }
    
    this.renderPressStyle(year, month, day, amount);
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
  renderPressStyle: function (year, month, day, amount) {
    var days = this.data.days;
    //渲染点击样式
    for (var j = 0; j < days.length; j++) {
      var tempDay = days[j].day;
      if (tempDay == day) {
        var date = year + "-" + month + "-" + day;
        var obj = {
          day: date,
          amount: amount
        };
        var checkDateJson = this.data.checkDate;
        var index = this.checkItemExist(checkDateJson, date);
        if (index == -1) {
          checkDateJson.push(obj);
          // days[j].class = days[j].class + ' active';
        } else {
          checkDateJson.splice(index, 1);
          days[j].class = days[j].class.replace('active', ' ');
        }
        this.setData({
          checkDate: checkDateJson
        })
        
        break;
      }
    }
    this.setData({
      days: days
    });

  },
  /**检查数组中是否存在该元素 */
  checkItemExist: function (arr, value) {
    for (var i = 0; i < arr.length; i++) {
      if (value === arr[i].day) {
        return i;
      }
    }
    return -1;
  },
  /**
 * 控制 pop 的打开关闭
 * 该方法作用有2:
 * 1：点击弹窗以外的位置可消失弹窗
 * 2：用到弹出或者关闭弹窗的业务逻辑时都可调用
 */
  toggleDialog:function() {
    this.setData({
      showDialog: !this.data.showDialog
    });
  },
  //确定 调整订单明细信息
  changeOrder:function()
  {
    var that=this
    var pickDate=that.data.dates
    if(that.data.zsproductflag)
    {
        //半价转售
        console.log("半价转售");
      wx.showModal({
        title: '提示',
        content:"是否确定半价转售",
        showCancel: true,
        success: function (res) {
          if (res.confirm) {
            console.log('确定半价转售')
            that.sellProduct()
          } else {
            console.log('用户点击取消')
          }
        }
      })

    }else{
      console.log("调整配送时间");
      //调整配送时间 判断选定时间是否是未来时间
      console.log(new Date(pickDate));
      wx.showModal({
        title: '提示',
        content: "是否确定调派送时间",
        showCancel: true,
        success: function (res) {
          if (res.confirm) {
            if (new Date(pickDate) > new Date()) {
              that.chgDateTime(pickDate)
              //更新订单
              
              that.loadOrderDetail(that.data.orderNO, that.data.mechineID, that.data.year, that.data.month)
            } else {
              $Toast({
                content: "请选择未来时间",
                type: 'warning'
              });
            }
          } else {
            console.log('用户点击取消')
          }
        }
      })
    }
   
    that.toggleDialog()
  },
  //调整派送时间
  chgDateTime: function (newTime)
  {
    var that = this;
    wx.request({
      url: config.httpsUrl,
      data: {
        orderNO: that.data.orderNO,
        olddateTime: that.data.olddateTime,
        mechineID: that.data.mechineID,
        newTime: newTime,
        action: 'chgDateTime'
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
        }else{
          $Toast({
            content: res.data.msg,
            type: 'warning'
          });
        }
      }
    })
  },
  sellProduct:function()
  {
    var that = this;
    wx.request({
      url: config.httpsUrl,
      data: {
        orderNO: that.data.orderNO,
        mechineID: that.data.mechineID,
        time:that.data.olddateTime,
        action: 'sellProduct'
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
        }else{
          $Toast({
            content: res.data.msg,
            type: 'warning'
          });
        }
      }
    })
  }

})
 