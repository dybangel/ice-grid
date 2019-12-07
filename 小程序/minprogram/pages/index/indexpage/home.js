const config = require('../../../config')
const { $Toast } = require('../../../dist/base/index');
const app = getApp()
 Page({
  data: {
    mechineName:'',
    xjlc: '',  
    address:'',
    showRight1: false,
    mechineList:'',
    payActivityList:'',
    payFlag:true,
    productActivity:'',
    productFlag:true,
    current: 'homepage',
    lbtList:'',
    num:'0',
    memberInfoFlag: "true",
    canIUse: wx.canIUse('button.open-type.getUserInfo'),
    sqFlag:false
  },
   distance: function (la1, lo1, la2, lo2) {

     var La1 = la1 * Math.PI / 180.0;

     var La2 = la2 * Math.PI / 180.0;

     var La3 = La1 - La2;

     var Lb3 = lo1 * Math.PI / 180.0 - lo2 * Math.PI / 180.0;

     var s = 2 * Math.asin(Math.sqrt(Math.pow(Math.sin(La3 / 2), 2) + Math.cos(La1) * Math.cos(La2) * Math.pow(Math.sin(Lb3 / 2), 2)));
     s = s * 6378.137;//地球半径
     s = Math.round(s * 10000) / 10000;
     // return s
   },
   handleChange({ detail }) {
     this.setData({
       current: detail.key
     });
     if (detail.key =="homepage")
     {
       wx.redirectTo({
          url: 'home',
        })
     } else if (detail.key =="order")
     {
       wx.redirectTo({
         url: '../order/orderlist',
       })
     } else if (detail.key =="notice")
     {
       wx.redirectTo({
         url: '../notice/noticelist',
       })
     } else if (detail.key =="mine")
     {
       wx.redirectTo({
         url: '../member/center',
       })
     }
   },
  
  //预览图片
  previewImg: function (e) {
    var currentUrl = e.currentTarget.dataset.currenturl
    var previewUrls = e.currentTarget.dataset.previewurl
    if (previewUrls!="")
    {
      wx.previewImage({
        current: currentUrl, //必须是http图片，本地图片无效
        urls: previewUrls, //必须是http图片，本地图片无效
      })
    }
  },
   previewImgAdver: function (e) {
     var currentUrl = e.currentTarget.dataset.currenturl
     var previewUrls = e.currentTarget.dataset.previewurl
     if(currentUrl!=""&&currentUrl!=null)
     {
       wx.navigateTo({
         url: 'adver?url=' + currentUrl,
       })
     }
   },
  onLoad: function () {
    var that = this
    that.setData({
      current: 'homepage'
    })
    app.globalData.userPhone = wx.getStorageSync("userPhone")
    app.globalData.minOpenID = wx.getStorageSync("minOpenID")
    app.globalData.unionID = wx.getStorageSync("unionID");

   
    var mID = wx.getStorageSync("mechineID")
    var mname = wx.getStorageSync("mechineName")
    var mxjlc = wx.getStorageSync("address")
    var maddress = wx.getStorageSync("xjlc")
    if(mID!=""&&mname!=""&&mxjlc!="")
    {
      that.setData({
        sqFlag:true,
        mechineName:mname,
        xjlc:mxjlc,
        address:maddress
      });
    }
    
     
    if (wx.getStorageSync("userInfo") != "" && wx.getStorageSync("userInfo") != undefined && wx.getStorageSync("unionID")!=""
      && wx.getStorageSync("minOpenID") != "" && wx.getStorageSync("minOpenID") != undefined)
    {
      that.setData({
        memberInfoFlag: !that.data.memberInfoFlag
      })
    }
  },
   handleClick: function () {
     var that=this
      wx.getLocation({
      success(res) {
        console.log("经度="+res.longitude)
        console.log("纬度="+res.latitude)
        wx.setStorageSync("longitude", res.longitude)
        wx.setStorageSync("latitude", res.latitude)
        that.onInitMechineList(res.longitude, res.latitude)
        that.setData({
          sqFlag:true
        });
      }
    })
     this.setData({
       showRight1: !this.data.showRight1
     });
   },
   //现在预订
   nowOrder: function(){
     var that=this;
     var mechine_id = wx.getStorageSync("mechineID")
     console.log("mechine_id=" + mechine_id);
     if (mechine_id != null && mechine_id!=''&&mechine_id!=undefined)
     {
       wx.navigateTo({
         url: '../product/product',
       })
     }else{
       wx.getLocation({
         success(res) {
           wx.setStorageSync("longitude", res.longitude)
           wx.setStorageSync("latitude", res.latitude)
           that.onInitMechineList(res.longitude, res.latitude)
           that.setData({
             sqFlag: true
           });
         }
       })
     }
    
   },
   clickMechine:function(e)
   {
     var name=e.currentTarget.dataset.name;
     var address=e.currentTarget.dataset.address;
     var jl = e.currentTarget.dataset.jl;
     var id=e.currentTarget.dataset.id;
     var that=this;
     that.setData({
       mechineName: name,
       xjlc: jl,
       address: address,
       showRight1: !this.data.showRight1
     })

     wx.setStorageSync("mechineID", id)
     wx.setStorageSync("mechineName", name)
     wx.setStorageSync("address", address)
     wx.setStorageSync("xjlc", jl)
    //app.globalData.mechineName,
   },
    
   
  // 加载机器列表
  onInitMechineList:function (lng,lat)
  {
    var that = this;
    wx.request({
      url: config.httpsUrl,
      data: {
        companyID: config.companyID,
        longitude:lng,
        latitude:lat,
        action: 'getMechineList'
      },
      method: "GET",
      header: {
        "Content-Type": "application/json"
      },
      success: function (res) {
        var len=res.data.length;
        for(var i=0;i<len;i++)
        {
          if(i==0)
          {
            that.setData({
              mechineName: res.data[i].mechineName,
              xjlc: res.data[i].xjlc,
              address: res.data[i].addres,
              mechineList : res.data,
             
            });
            
            wx.setStorageSync("mechineID", res.data[0].id)
            wx.setStorageSync("mechineName", res.data[0].mechineName)
            wx.setStorageSync("address", res.data[0].addres)
            wx.setStorageSync("xjlc", res.data[0].xjlc)
          }
        }
      }
    })
  },
   myWallet:function(e)
   {
     wx.navigateTo({
       url: '../member/wallet',
     })
   },
   getOrderActivity:function()
   {

   },
   getPayActivity2:function()
   {
     var that = this;
     wx.request({
       url: config.httpsUrl,
       data: {
         companyID: app.globalData.companyID,
         action: 'getPayActivityList2'
       },
       method: "GET",
       header: {
         "Content-Type": "application/json"
       },
       success: function (res) {
         if(res.data!="")
         {
           that.setData({
             payFlag: true,
             payActivityList: res.data
           })
         }else{
           that.setData({
             payFlag:false
           })
         }
       }
     })
   },
   getProductActivity2: function () {
     var that = this;
     wx.request({
       url: config.httpsUrl,
       data: {
         companyID:app.globalData.companyID,
         action: 'getProductActivity2'
       },
       method: "GET",
       header: {
         "Content-Type": "application/json"
       },
       success: function (res) {
         if(res.data!="")
         {
           that.setData({
             productFlag:true,
             productActivity: res.data
           })
         }else{
           that.setData({
             productFlag:false
           })
         }
        
       }
     })
   },
   getCode:function()
   {
    wx.navigateTo({
      url: '../order/getCode',
      
    })
   },
   scanQH:function()
   {
     
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
             if(res.data.code=="200")
             {
               wx.showModal({
                 title: '提示',
                 content: res.data.msg,
                 showCancel: false,
                 success: function (res) {
                    
                 }
               })
             
             }else{
               wx.showModal({
                 title: '提示',
                 content: res.data.msg,
                 showCancel: false,
                 success: function (res) {
                   
                 }
               })
              
             }
           }
         })
       }
     })
   },
   onloadAdverse:function()
   {
     var that = this
   
     wx.request({
       url: config.httpsUrl,
       data: {
         companyID: app.globalData.companyID,
         action: 'getAdverseList'
       },
       method: "GET",
       header: {
         "Content-Type": "application/json"
       },
       success: function (res) {
         if (res.data.code == "200") { 
           that.setData({
             lbtList: res.data.db2,
             picList: res.data.db,
           })
         } 
       }
     })
   },
   onloadNotice:function()
   {
     var that = this
     wx.request({
       url: config.httpsUrl,
       data: {
         unionID: app.globalData.unionID,
         minOpenID: app.globalData.minOpenID,
         action: 'getNoticeListCount'
       },
       method: "GET",
       header: {
         "Content-Type": "application/json"
       },
       success: function (res) {
         if (res.data.code == "200") {
           that.setData({
             num: res.data.num
           })
         }  
       }
     })
   },
   onShow: function () {
     console.log("onShow");
     var that = this;
     that.getPayActivity2()
     that.getProductActivity2()
     that.onloadAdverse()
     that.onloadNotice()
    
   },
   onShareAppMessage:function()
   {
     
   },
   clickFormView:function(event)
   {
     var formId = event.detail.formId
     console.log("formId=" + formId);
     console.log("touser=" + app.globalData.minOpenID);
     console.log("companyID=" +app.globalData.companyID);
     wx.request({
       url: config.httpsUrl,//自己的服务接口地址
       method: "GET",
       header: {
         "Content-Type": "application/json"
       },
       data: {
         type:"1",
         formId: formId,
         companyID: app.globalData.companyID,
         touser: app.globalData.minOpenID,
         action: "sendTemplateMessage"
       },
       success: function (res) {
          console.log("模板消息发送成功");
       },
       fail:function(res){
         console.log("模板消息发送失败");
       }
     })

   },
   bindGetUserInfo: function (event) {
     var that=this
     
     console.log(event.detail.userInfo)
     //使用
     wx.getSetting({
       success: res => {
         if (res.authSetting['scope.userInfo']) {
           // 已经授权，可以直接调用 getUserInfo 获取头像昵称，不会弹框
           wx.login({
             success: function (res) {
               var code = res.code;//登录凭证
               if (code) {
                 //2、调用获取用户信息接口
                 wx.getUserInfo({
                   lang:"zh_CN",
                   success: function (res) {
                     console.log({ encryptedData: res.encryptedData, iv: res.iv, code: code })
                     //3.请求自己的服务器，解密用户信息 获取unionId等加密信息
                     wx.request({
                       url: config.httpsUrl,//自己的服务接口地址
                       method: "GET",
                       header: {
                         "Content-Type": "application/json"
                       },
                       data: { 
                         encryptedData: res.encryptedData, 
                         iv: res.iv, 
                         code: code,
                         companyID:app.globalData.companyID,
                         phone: app.globalData.userPhone,
                         action: "getDecryptUserInfo" 
                         },
                       success: function (res) { 
                         if (res.data.code=="200")
                         { 
                           console.log("解密用户信息=" + JSON.stringify(res.data.db));
                           app.globalData.minOpenID = res.data.db.openId
                           app.globalData.unionID = res.data.db.unionId
                           app.globalData.memberInfo = res.data.userInfo
                           wx.setStorageSync("minOpenID", res.data.db.openId)
                           wx.setStorageSync("unionID", res.data.db.unionId)
                           wx.setStorageSync("userInfo", res.data.userInfo)
                           that.setData({
                             memberInfoFlag: !that.data.memberInfoFlag
                           });
                         }else if(res.data.code=="400"){
                           wx.navigateTo({
                             url: '../login',
                           })
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

               } else {
                 $Toast({
                   content: '获取用户登录态失败！' + r.errMsg,
                   type: 'warning'
                 });
                 
               }
             },
             fail: function () {
               $Toast({
                 content: '登陆失败',
                 type: 'warning'
               });
               
             }
           })

         } else {
           //用户拒绝获取会员信息
          //  $Toast({
          //    content: '获取用户信息失败',
          //    type: 'warning'
          //  });
          
         }

       }
     })

   }
})
 