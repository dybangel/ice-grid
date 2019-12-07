package com.sq.util;
import java.util.Date;



import android.os.Environment;
import de.tavendo.autobahn.WebSocketConnection;

public class Config {
	public static final String NameSpace = "http://nq.bingoseller.com/";
	public static final String url = "http://nq.bingoseller.com/api/mechineService.asmx";
	public static final String ACTION_UPDATEUI="action.updateUI";
	public static final String ACTION_UPDATEUI_IMG="action.updateUI.img";
	public static final String ACTION_PRODUCTTYPE="action.updateUI.producttype";//更新产品类别
	public static final String ACTION_UPDATEUI_VIDEO="action.updateUI.video";
	public static final String ACTION_JUMP_URL="action.jump.url";
	// /storage/emulated/0/asm/asm/video/hvideo/
	public static final String OPEN_HVIDEO_PATH=Environment.getExternalStorageDirectory() + "/asm/video/hvideo/";//横屏视频播放地址
	public static final String OPEN_VVIDEO_PATH=Environment.getExternalStorageDirectory() + "/asm/video/vvideo/";//竖屏视频播放地址
	public static final String PRODUCT_IMAGE_SAVE_PATH=Environment.getExternalStorageDirectory() + "/asm/image/";//商品图片保存路径
	public static final String PRODUCT_IMAGE_SAVE_PATH_QR=Environment.getExternalStorageDirectory() + "/asm/image/qr/";//商品图片保存路径
	public static final String LOG_URL=Environment.getExternalStorageDirectory() + "/asm/";//log 日志
	public static final String BILL_URL=Environment.getExternalStorageDirectory() + "/asm/bill/";//出货 日志
	public static  String BILL_NO="0000";//单号
	public static final String apkUrl="";
	public static final String apkSet="http://admin.bingoseller.com/soft/newsoft/ver1.json";
	public static String locationAddress="";
	 
	//public static final String TCP_HOST = "ws://114.116.16.200/api/webSocket.ashx";
	//public static final String TCP_HOST = "ws://akmsocket.bingoseller.com/api/webSocket.ashx";
	public static  WebSocketConnection webSocketConnection;
	public static int socketStatus=1;//0正常 1异常
	public static int gkjStatus=1;//0正常 1异常
	public static final String CMD="cmd";
	public static final String APP_HEARTBEAT="app.heartbeat";
	public static final String INFO="info";
	public static final String RECEIVE="receive";
	public static final String SEND="send"; 
	public static final String BZ="bz"; 
	public static final String LX="lx";
	public static final String LOG="log";
	public static final String CH="ch";
	public static final String STATUS="status";
	public static final String TEM="tem";
	public static final String ALL="all";
	public static final String MECHINE_ID="mechineID";
	public static final String VERSION="version"; 
	public static final String VERCODE="vercode"; 
	public static final String VIDEO="video";
	public static String billno="";
	public static int type=0;//1订购2零售
	public static String memberID="";
	public static int temperature=0;
	//此处三个变量等2019-12-02更新后版本若稳定则会弃用
	public static Long update_produtct_time=1000*60*2l;//更新产品时间
	public static Long update_productType_time=1000*60*20l;//更新产品类型时间
	public static Long update_video_time=1000*60*10l;//更新视频时间
	public static boolean flag=true;
	public static int busy=0;//0 不忙 1正在出货忙
	
	public static Date lastsockettime=null;//最后一次接受到socket的时间
	public static Date lastgkjtime=null;//工控机最后一次接受到数据的时间
	public static String socketUrl="ws://alisocket.bingoseller.com/api/webSocket.ashx";
	public static int netWorkType=0;//网络等级0、无;1、wifi;2、2G;3、3G;4、4G;5、未找到默认;
	public static int netLevel=0;//网络等级-1、异常0-4代表5个等级;0无信号，4最强
}
