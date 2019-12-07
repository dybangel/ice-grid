package com.bjw.ComAssistant;

import java.io.IOException;
import java.security.InvalidParameterException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.Date;
import java.util.LinkedList;
import java.util.List;
import java.util.Locale;
import java.util.Queue;

import net.sf.json.util.NewBeanInstanceStrategy;

import org.json.JSONException;
import org.json.JSONObject;

import android.R.bool;
import android.R.integer;
import android.R.string;
import android.app.Service;
import android.content.Context;
import android.content.Intent;
import android.content.pm.PackageManager.NameNotFoundException;
import android.os.Binder;
import android.os.Handler;
import android.os.IBinder;
import android.os.Message;
import android.os.Parcel;
import android.os.RemoteException;
import android.util.Log;
import android.widget.TextView;

import com.bjw.bean.ComBean;
import com.sq.dao.Dao;
import com.sq.util.Config;
import com.sq.util.Util;

public class PortService extends Service {
	public static String com_A = "/dev/ttyS1";
	public static String comA_rate = "9600";
	public static SerialControl ComA;//
	public static DispQueueThread DispQueue;// 刷新显示线程
	public static String recLogString;
	public static String chLD = "";// 出货料道
	public static boolean is_ch = false;// 是否出货
	public static boolean is_search_tem = false;
	
	public static int temperature = 0;// 温度信息
	public String mechineID = "";
	public static int STATUS = 1;// 0不忙状态 1忙状态不可以出货
	public static String payType = "";// 支付类型 1微信2支付宝 3微信公众号4余额
	public static String billno = "";// 第三方支付的单号
	public static String orderType;// 1订购2 零售 0半价
	public static boolean chRecord = false;// 出货记录开关 提交一次置成false
	public static String productID = "";
	public static String money = "0";
	public static String memberID = "0";
	public static String code = "";// 取货码
	public static Date sclxsj = new Date();// 上次轮询进入的时间 刚开启时候 给打开软件的时间
	public static boolean chjs = true;// 是否出货
	public static Date jqmtime = new Date();// 机器进入忙的时间
	public static int comlianjie = 0;// com 进入连接 收到信息赋值1 发送服务器后赋值0
	public static Date jrlxsj = new Date();// 进入轮询时间
	public static int ld = 9;// 进入轮询时间
	public static boolean opendoor =false;
	public static Date opendoortime = new Date();// 上次轮询进入的时间 刚开启时候 给打开软件的时间

	public static List<String> stlist = new ArrayList();
	public static boolean is_return_tem = true;//查询温度是否有回值了true已回复
	
	
	
	
	//新加的心跳包信息
	
	public static String productListNo="";// 产品列表编号
	public static String priceSwitch="";// 是否开启会员价
	
	public static String productTypeNo="";// 产品类型编号
	public static String videoListNo="";// 视频列表编号
	public static String mechineOtherStatus="";// 机器的其他信息
	
	public static boolean isSetTime =false;//给机器设定时间
	public static boolean chEnd = true;// 是否出货完成，在轮询时发广播返回产品页面，并将状态给更成false。然后机器出货成功或失败后fasocket给服务器，服务器处理过发回chEnd指令安卓接收到后将此更成true
	
	
	public static void csch()

	{
		int fenzhong1 = (int) ((new Date().getTime() - jrlxsj.getTime()) / (1000));
		Util.writeBillTxtToFile("轮询:" + fenzhong1 + "s"
				+ (STATUS == 0 ? "空闲：可以接受指令" : "机器未就绪"));

		if (STATUS == 0 && fenzhong1 > 5) {

			if (ld % 10 == 7 && ((int) (ld + 10) / 10) == 5) {
				ld = 10;
			} else if (ld % 10 == 7) {
				ld = ((int) (ld + 10) / 10) * 10;

			} else {

				ld = ld + 1;
			}
			//如果有温度查询任务，执行查询
			SimpleDateFormat sfDateFormat1 = new SimpleDateFormat("MMddHHmmss");

			chLD = ld + "";
			is_ch = true;
			payType = "1";
			billno = sfDateFormat1.format(new Date());
			orderType = "";
			productID = "";
			money = "" + 1;
			memberID = "" + 1;// 订购的时候会有这个字段
			code = "222";
			Util.debuglog("单号："+Config.billno+"料道："+ld+"开始","chlcAll");
			Util.writeBillTxtToFile("准备订单:" + billno + "料道：" + chLD + "-->>>:");
			Config.billno = billno;

			Config.billno = billno;

			Config.type = 2;

		}

	}

	public static void sendchlc(String chlcString) {
		SimpleDateFormat sfDateFormat1 = new SimpleDateFormat("hh:mm:ss SSS");

		stlist.add(sfDateFormat1.format(new Date()) + "-----|  "
				+ Config.billno + "订单," + ld + "料道" + chlcString);
		if (stlist.size() > 30) {
			stlist.remove(0);

		}
		StringBuilder S = new StringBuilder();
		for (String item : stlist) {

			S.append(item + "\r\n");

		}

		Message msg = new Message();

		msg.obj = S.toString();
		IndexActivity.chlchandler.sendMessage(msg);

	}
	public static String Getjiqigezhongxinxi() {
		JSONObject reqJson= new JSONObject();
		try {
			int openDoorTime=0; 
			//取货口打开
			if(opendoor){
				openDoorTime = (int) ((new Date().getTime() - opendoortime.getTime()) / (1000));
					//发送当前机器的状态给服务器，，时间
			}
			//机器从未就绪到就绪会查询一次温度
			reqJson.put("TEXT", "设备的一些状态");
			int fenzhong = (int) ((new Date().getTime() - sclxsj.getTime()) / (1000 * 60));
			reqJson.put("t1", fenzhong);// 上一次轮询 至今的时间差 分钟
			reqJson.put("t2", temperature);// 温度
			reqJson.put("t3", comlianjie);// com口连接
			reqJson.put("t4", STATUS);// 机器售卖状态
			reqJson.put("t5", openDoorTime);// 出货门打开的时间
			return reqJson.toString();

		} catch (Exception e) {
			return reqJson.toString();
		}

	}
	public static String Getjiqigezhongxinxi(JSONObject reqJson) {
		if(reqJson==null){
			reqJson= new JSONObject();
		}
		try {
			int openDoorTime=0; 
			//取货口打开
			if(opendoor){
				openDoorTime = (int) ((new Date().getTime() - opendoortime.getTime()) / (1000));
				
					
					//发送当前机器的状态给服务器，，时间
				
				
			}
			
			
			//机器从未就绪到就绪会查询一次温度
			int fenzhong = (int) ((new Date().getTime() - sclxsj.getTime()) / (1000 * 60));
			reqJson.put("lastLXTime", fenzhong);// 上一次轮询 至今的时间差 分钟
			reqJson.put("temperature", temperature);// 温度
			//reqJson.put("comlianjie", comlianjie);// 
			reqJson.put("mechineStatus", STATUS);// 轮询没有任务时是0，其他是1
			reqJson.put("busyStatus", Config.busy);//是否在出货
			reqJson.put("openDoorTime", openDoorTime);// 出货门打开的时间
			reqJson.put("mechineOtherStatus", mechineOtherStatus);// 类型列表编号
			
			reqJson.put("gkjStatus", Config.gkjStatus);// 工控机状态
			reqJson.put("productListNo", productListNo);// 产品列表编号
			reqJson.put("priceSwitch", priceSwitch);// 是否开启会员模式
			reqJson.put("videoListNo", videoListNo);// 视频编号
			
			reqJson.put("productTypeNo", productTypeNo);// 类型列表编号
			reqJson.put("netWorkType", Config.netWorkType);// 网络类型
			reqJson.put("netLevel", Config.netLevel);// 网络强度
			return reqJson.toString();

		} catch (Exception e) {
			return reqJson.toString();
		}

	}
	public static String GetBillObject() {
		JSONObject reqJson= new JSONObject();
		try {
			reqJson.put("ldNO",chLD);
			reqJson.put("payType",payType);
			reqJson.put("billno",billno);
			reqJson.put("productID",productID);
			reqJson.put("money",money);
			reqJson.put("type",orderType);
			reqJson.put("memberID",memberID);
			reqJson.put("code",code);
			return reqJson.toString();

		} catch (Exception e) {
			return reqJson.toString();
		}

	}
	@Override
	public IBinder onBind(Intent intent) {
		return null;
	}

	@Override
	public int onStartCommand(Intent intent, int flags, int startId) {
		try {
			chLD = intent.getStringExtra("ldNO");
			is_ch = intent.getBooleanExtra("is_ch", false);
			payType = intent.getStringExtra("payType");
			billno = intent.getStringExtra("billno");
			orderType = intent.getStringExtra("orderType");
			productID = intent.getStringExtra("productID");
			money = intent.getStringExtra("money");
			memberID = intent.getStringExtra("memberID");// 订购的时候会有这个字段
			code = intent.getStringExtra("code");
			
		} catch (Exception e) {
			Log.i("4444", "onStartCommand=" + e.getMessage() + "传值错误");
		}
		return super.onStartCommand(intent, flags, startId);
	}

	@Override
	public void onCreate() {
		mechineID = Util.getSharePer(getApplicationContext(), "mechineID");
		open();
		
		super.onCreate();
	}

	public void open() {
		ComA = new SerialControl();
		DispQueue = new DispQueueThread();
		DispQueue.start();
		ComA.setPort(com_A);
		ComA.setBaudRate(comA_rate);
		OpenComPort(ComA);
	}

	@Override
	public void onDestroy() {
		super.onDestroy();
	}

	private static class SerialControl extends SerialHelper {
		public SerialControl(String sPort, String sBaudRate) {
			super(sPort, sBaudRate);
		}

		public SerialControl() {

		}

		@Override
		protected void onDataReceived(final ComBean ComRecData) {
			DispQueue.AddQueue(ComRecData);// 线程定时刷新显示(推荐)
		} 
	}

	// ----------------------------------------------------刷新显示线程
	private class DispQueueThread extends Thread {
		private Queue<ComBean> QueueList = new LinkedList<ComBean>();

		@Override
		public void run() {
			super.run();
			while (!isInterrupted()) {
				final ComBean ComData;
				while ((ComData = QueueList.poll()) != null) {
					try {
						DispRecData(ComData);
						// Thread.sleep(100);// 显示性能高的话，可以把此数值调小。不能太高会造成不出货
					} catch (Exception e) {
						e.printStackTrace();
					}
					break;
				}
			}
		}

		public synchronized void AddQueue(ComBean ComData) {
			QueueList.add(ComData);
		}
	}

	// ----------------------------------------------------显示接收数据
	public static String REC_DATA = "";

	private void DispRecData(ComBean ComRecData) {
		StringBuilder sMsg = new StringBuilder();
		sMsg.append(ComRecData.sRecTime);
		sMsg.append("[");
		sMsg.append(ComRecData.sComPort);
		sMsg.append("]");
		sMsg.append("[Hex] ");
		sMsg.append(MyFunc.ByteArrToHex(ComRecData.bRec));
		sMsg.append("\r\n");
		String recData = MyFunc.ByteArrToHex(ComRecData.bRec).replaceAll(" ",
				"");

		if (recData.length() >= 2 && "FE".equals(recData.substring(0, 2))) {
			REC_DATA = recData;
		} else {
			REC_DATA += recData;
		}
		if (REC_DATA.length() > 10) {
			String jyh = makeChecksum(REC_DATA.substring(0,
					REC_DATA.length() - 2));
			if (jyh.equals(REC_DATA.substring(REC_DATA.length() - 2,
					REC_DATA.length()))) {
				init(REC_DATA);
				final Intent intent = new Intent();
				intent.setAction(IndexActivity.PORT_ACTION);
				intent.putExtra("status", STATUS);
				// intent.putExtra("hex", REC_DATA);
				sendBroadcast(intent);
			}
		}
	}

	// ----------------------------------------------------打开串口
	public void OpenComPort(SerialHelper ComPort) {
		try {
			ComPort.open();
		} catch (SecurityException e) {
			ShowMessage("打开串口失败:没有串口读/写权限!");
		} catch (IOException e) {
			ShowMessage("打开串口失败:未知错误!");
		} catch (InvalidParameterException e) {
			ShowMessage("打开串口失败:参数错误!");
		}
	}

	public String bzInfo = "";
	//料道状态，出现异常1，正常轮询时改为0
	
	public static String reshex(String hex) {
		
		
		if (hex.contains("FE55EF1278")) {
			return "机器复位初始化";
		} else if (hex.contains("FE55EF0D73")) {
			return "判断机器类型";
		} else if (hex.contains("FE55EF1573")) {
			return "获取料道配置信息";
		} else if (hex.contains("FE55EF2C7D")) {
			return "系统配置  状态信息";
		} else if (hex.contains("FE55EF2F7C")) {
			return "销售信息  出货信息";
		} else if (hex.contains("FE55EF137A")) {
			return "料道故障状态";
		} else if (hex.contains("FE55EF0979")) {
			return "系统故障状态";
		} else if (hex.contains("FE55EF0D7B")) {
			return "料道有无货信息";
		}
		// 76
		else if (hex.contains("FE55EF0376F6")) {
			return "读卡器没准备好";
		} 
		else if (hex.contains("FE55EF0376F6")) {
			return "读卡器没准备好";
		} else if (hex.contains("FE55EF0376F8")) {
			return "无法启动读卡器";
		} else if (hex.contains("FE55EF0376F5")) {
			return "交易序列号相同";
		} else if (hex.contains("FE55EF037600")) {
			return "交易完成";
		} else if (hex.contains("FE55EF11760700000000000000000000000000008E")) {
			return "机器就绪，可以接受新的指令";
		} else if (hex.contains("FE55EF11760700000000000000000000000000018F")) {
			return "取货门未关 或者没有复位";
		} else if (hex.contains("FE55EF117607000000000000000000000000000593")) {
			return "有可能设置门没有关";
		} else if (hex.contains("FE55EF117607000000000000000000000000000492")) {
			return "有可能是设置门没有关";
		}

		else if (hex.contains("FE55EF117601")) {
			return "出货中，机器正忙";
		}

		else if (hex.contains("FE55EF037601".trim())) {
			return "交易忙";
		} else if (hex.contains("FE55EF0376FF".trim())) {
			return "交易失败";
		} else if (hex.contains("FE55EF0376FA".trim())) {
			return "超时出错";
		} else if (hex.contains("FE55EF0376FE".trim())) {
			return "校验错误";
		} else if (hex.contains("FE55EF0376F4".trim())) {
			return "料道错误";
		} else if (hex.equals("FE55EF0376F3".trim())) {
			return "料道错误";
		}

		return "未知类型";

	}

	public static String sendhex(String hex) {

		if (hex.equals("EF55FE0378007B")) {
			return "复位指令";
		} else if (hex.equals("EF55FE0676000000007C")) {
			return "轮询指令";
		} else if (hex.equals("EF55FE067673000000EF".trim())) {
			return "查 询 饮 料 机 配 置";
		} else if (hex.equals("EF55FE03730076".trim())) {
			return "料道配置应答";
		} else if (hex.equals("EF55FE067673010000F0".trim())) {
			return "查询弹簧机配置";
		}

		else if (hex.contains("EF55FE06767D000000F9".trim())) {
			return "状态查询";
		} else if (hex.contains("EF55FE037D0080".trim())) {
			return "状态应答";
		} else if (hex.contains("EF55FE0379007C".trim())) {
			return "系统故障状态";
		} else if (hex.contains("EF55FE06767A000000F6".trim())) {
			return "料道故障査询";
		} else if (hex.contains("EF55FE037B007E".trim())) {
			return "料道信息初始化";
		} else if (hex.contains("EF55FE037A007D".trim())) {
			return "故障回复";
		} else if (hex.contains("EF55FE067677000000F3".trim())) {
			return "售卖查询";
		}

		else if (hex.contains("EF55FE0377007A".trim())) {
			return "售卖查询";
		} else if (hex.contains("EF55FE067677000000F3".trim())) {
			return "售卖查询回复";
		} else if (hex.contains("EF55FE177605".trim())) {
			return "出货指令";
		} else if (hex.contains("EF55FE037C007F".trim())) {
			return "出货报告";
		} else if (hex.contains("EF55FE097604".trim())){
			return "设定时间";
			
		}

		return "没有对应类型";
	}
	//机器基础信息
	public boolean baseInit(String str) {
		
		if (str.trim().length() <= 10)
			return true;
		//机器复位初始化
		if (str.trim().substring(0, 10).equals("FE55EF1278")) {
			sendPortData("EF55FE0378007B");
			return true;
		}
		//判断机器类型
		if (str.trim().substring(0, 10).equals("FE55EF0D73")) {// 料道配置
			sendPortData("EF55FE03730076");
			return true;
		}
		//获取料道配置信息
		if (str.trim().substring(0, 10).equals("FE55EF1573")) {
			sendPortData("EF55FE03730076");
			return true;
		}
		//系统配置  状态信息
		if (str.trim().substring(0, 10).equals("FE55EF2C7D")) {
			// 系统配置 状态信息
			
			if (str.length() >= 96) {
				// 获取温度信息
				temperature = Integer.parseInt(Util.getTemper(str.substring(84,
						86)));
				is_return_tem = true;
			}
			sendPortData("EF55FE037D0080");
			return true;
		}
		//系统故障状态
		if (str.trim().substring(0, 10).equals("FE55EF0979")) {// 系统故障状态
			
			sendPortData("EF55FE0379007C");
			return true;
		}
		//料道故障状态
		if (str.trim().substring(0, 10).equals("FE55EF137A")) {// 料道故障状态
			sendPortData("EF55FE037A007D");
			return true;
		}
		//料道有无货信息
		if (str.trim().substring(0, 10).equals("FE55EF0D7B")) {// 料道有无货信息
			sendPortData("EF55FE037B007E");
			return true;
		}
		return false;
		
	}
	public void init(String str) {
		Config.lastgkjtime=new Date();
		Config.gkjStatus = 0;
		Util.writeBillTxtToFile("接收:" + reshex(str) + "<<<-:" + str);
		//csch();
		
		if(baseInit(str)){
			return;
			
		};
		if (str.equals("FE55EF11760700000000000000000000000000018F")) 
		{
			//取货口打开
			if(!opendoor){
				
				opendoor=true;//开门状态
				opendoortime=new Date();							
				Util.debuglog("单号："+Config.billno+"料道："+ld+"开取货门","chlcAll");
			}
			int opendoortimes = (int) ((new Date().getTime() - opendoortime.getTime()) / (1000));
			if(opendoortimes>60&&opendoortimes%60==0){
				
				//发送当前机器的状态给服务器，，时间
			}
			//取货门开门时间
			
		}else {
			if(opendoor){
				int opendoortimes = (int) ((new Date().getTime() - opendoortime.getTime()) / (1000));
				opendoor=false;//开门关闭
				Util.debuglog("单号："+Config.billno+"料道："+ld+"取货门关闭 开门总计用时："+opendoortimes+"s","chlcAll");
			}
		}
		
		// 如果当前是轮询，8E；并且有出货任务，发送任务并改变任务状态为已发送给下位机
		// 出货指令-->>>:EF55FE177605006201110100000000000001000100000000000009
		// 交易忙<<<-:FE55EF0376017A 代表机器已经收到指令 开始干活
		// FE55EF117601 出货中
		// FE55EF03760079 交易完成
		// FE55EF2F7C0111 结果
		//is_ch = false;
		
		if (str.trim().substring(0, 12).equals("FE55EF117607")) {// 轮询
			// 判断后两位是8E 则是正常轮询 否则就是机器要么暂停营业要么门没关好
			sclxsj=new Date();
			if (!str.trim().substring(str.trim().length() - 2, str.length())
					.equals("8E")) {
				STATUS = 1;
				return;
			}
			if (is_ch) {
				
				STATUS = 1;
				Integer ld = Integer.parseInt(chLD);
				String hexld = ld.toHexString(ld);
				String xlh = Util.getXLH();
				String str1 = "EF55FE17760500"
						+ xlh
						+ "01"
						+ ld
						+ "01000000000000010001000000000000"
								.replaceAll(" ", "");
				sendPortData(str1 + makeChecksum(str1));
				is_ch = false;
				chRecord = true;
				final Intent intent = new Intent();
				intent.setAction(Config.ACTION_JUMP_URL);
				intent.putExtra("TYPE", "1"); // type=1 跳转到倒计时页面
				sendBroadcast(intent);
				
			} else {
				if (is_search_tem) {
					STATUS = 1;
					
					sendPortData("EF55FE06767D000000F9");// 查询机器状态
					Log.i("1111","EF55FE06767D000000F9");
					is_search_tem = false;
					final Intent intent = new Intent();
					intent.setAction(MainActivity.ACTION_UPDATEUI);
					intent.putExtra("temperature", temperature);
					sendBroadcast(intent);
					return;
				}else if(!isSetTime){
					//开机时更新售卖机的时间
					//说明：更新售卖机时间只在开启软件收到第一次无任务轮询时设置，之后就不在走
					//命令的发送与接受学习：
					//前面EF55FE不计算，后面的2位算一位，10进制转16进制写法，最后一位是校验和，将前面的计算的用10进制加起来然后在写成16进制
					//16进制1-9+ABCDEFG共十六位
					//例子：如下是将当前时间写成16位的命令年去后两位
					STATUS = 1;
					Calendar now = Calendar.getInstance();
					int year= now.get(Calendar.YEAR);
					int month=now.get(Calendar.MONTH) + 1;
					int day=now.get(Calendar.DAY_OF_MONTH) ;
					int hour=now.get(Calendar.HOUR_OF_DAY);
					int min=now.get(Calendar.MINUTE) ;
					int second=now.get(Calendar.SECOND);
					int total=131+(year%100)+month+day+hour+min+second;
					int year1=(year%100)/16;
					int year2=(year%100)%16;
					int month1=month/16;
					int month2=month%16;
					int day1=day/16;
					int day2=day%16;
					int hour1=hour/16;
					int hour2=hour%16;
					int min1=min/16;
					int min2=min%16;
					int second1=second/16;
					int second2=second%16;
					int total1=total/16;
					int total2=total%16;
					String setTimeString=tenToSixTeen(year1)+tenToSixTeen(year2)
							+tenToSixTeen(month1)+tenToSixTeen(month2)
							+tenToSixTeen(day1)+tenToSixTeen(day2)
							+tenToSixTeen(hour1)+tenToSixTeen(hour2)
							+tenToSixTeen(min1)+tenToSixTeen(min2)
							+tenToSixTeen(second1)+tenToSixTeen(second2)
							+tenToSixTeen(total1)+tenToSixTeen(total2);
					sendPortData("EF55FE097604"+setTimeString);// 查询机器状态
					Util.debuglog("时间："+year+"-"+month+"-"+day+" "+hour+":"+min+":"+second+"命令:"+"EF55FE097604"+setTimeString,"setTime");
					
					isSetTime=true;
					
				}
				else {
					// 正常轮询
					STATUS = 0;
					chRecord = false;
					if(chEnd){
						Log.i("1111","chEnd");
						final Intent intent = new Intent();
						intent.setAction(Config.ACTION_JUMP_URL);
						intent.putExtra("TYPE", "2"); // type=1 跳转到倒计时页面
														// type=2 跳转到产品列表页面
						sendBroadcast(intent);
						chEnd=false;
					}
					
					Config.busy=0;
					sendPortData("EF55FE037C007F");
				}
			}
			
		}
		
		//正在出货
		if (str.trim().substring(0, 12).equals("FE55EF117601")) {
			Config.busy = 1;
			try {
				JSONObject jsonObject= new JSONObject();
				jsonObject.put(Config.MECHINE_ID, mechineID);
				jsonObject.put("cmd", "ch");
				jsonObject.put("SendMsg", GetBillObject());
				jsonObject.put("MsgId", "");
				JSONObject samtypeJson= new JSONObject();
				samtypeJson.put(Config.CMD, "chAlways");
				jsonObject.put("samtype", Getjiqigezhongxinxi(samtypeJson));
				Util.sendMsg(jsonObject.toString());
			} catch (JSONException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
			sendPortData("EF55FE0676000000007C");
			return;
		}
		//料道错误的，但是没有找到匹配的回值
		//暂且按料道错误写
		if (str.equals("FE55EF0376F36C"))//料道错误
		{
			if (chRecord) {
				chRecord = false;
				// 提交出货记录
				String json = "";
				String pay_Type = payType;
				String product_ID = productID;
				String totalMoney = money;
				String proLD = chLD;
				String type = orderType;
				String bill_no = Config.billno;
				String mechineID = Util.getSharePer(getApplicationContext(), "mechineID");
				String bz = "料道错误";
				json = Dao.returnJson(pay_Type, product_ID, totalMoney,proLD, type, bill_no, mechineID, bz, memberID,code);
				// 提交出货记录
				sendchFail(json);
				

			}

			bzInfo = "料道错误";
			sendPortData("EF55FE0676000000007C");

			Util.debuglog("单号："+Config.billno+"料道："+chLD+"料道错误","chlcAll");
			return;
		}
		
		
		//成功或失败
		if (str.trim().substring(0, 10).equals("FE55EF2F7C")) {// 销售信息 出货信息
			recLogString = "销售信息  出货信息";
			// 此处查询购买是否成功 每售卖一个货物vmc会主动发送一次 如果vmc没有收到相应会在发送一次最多发送3次
			// 需要核对交易序列号
			if (str.trim().length() == 102) {
				String xlh = str.substring(86, 88);
				String ldNO = str.substring(12, 14);
				String status = str.substring(60, 62);// 00代表交易成功01代表交易失败
				if ("00".equals(status)) {
					// 交易成功
					if (chRecord) {
						chRecord = false;
						
						String json = "";
						String pay_Type = payType;
						String product_ID = productID;
						String totalMoney = money;
						String proLD = chLD;
						String type = orderType;
						String bill_no = Config.billno;
						String mechineID = Util.getSharePer(getApplicationContext(), "mechineID");
						String bz = "交易成功";
						json = Dao.returnJson(pay_Type, product_ID, totalMoney,proLD, type, bill_no, mechineID, bz, memberID,code);
						
						sendchSuccess(json);
						
						
						
					}
				} else if ("01".equals(status)) {
					if (chRecord) {
						chRecord = false;
						
						String json = "";
						String pay_Type = payType;
						String product_ID = productID;
						String totalMoney = money;
						String proLD = chLD;
						String type = orderType;
						String bill_no = Config.billno;
						String mechineID = Util.getSharePer(getApplicationContext(), "mechineID");
						String bz = "出货失败";
						json = Dao.returnJson(pay_Type, product_ID, totalMoney,proLD, type, bill_no, mechineID, bz, memberID,code);

						sendchFail(json);
						
					
						
					}
				}
			}
			//出货报告
			sendPortData("EF55FE037C007F");
			return;
		}
		// 出货部分
		if (str.trim().substring(0, 12).equals("FE55EF117602")) {
			Config.busy = 0;
			// 料道没出货
			JSONObject reqJson = new JSONObject();
			if (chRecord) {
				chRecord = false;
				// 提交出货记录
				String json = "";
				String pay_Type = payType;
				String product_ID = productID;
				String totalMoney = money;
				String proLD = chLD;
				String type = orderType;
				String bill_no = Config.billno;
				String mechineID = Util.getSharePer(getApplicationContext(), "mechineID");
				String bz = "出货失败";
				json = Dao.returnJson(pay_Type, product_ID, totalMoney,proLD, type, bill_no, mechineID, bz, memberID,code);
				// 提交出货记录
				
				sendchFail(json);
			}
			
				

		

			

			sendPortData("EF55FE0676000000007C");
			// sendPortData("EF55FE037C007F");
			return;
		}
		if (str.trim().substring(0, 12).equals("FE55EF117604")) {
			Config.busy = 0;
			STATUS = 1;
			String json = "";
			// 交易成功
			if (chRecord) {
				chRecord = false;
				// 提交出货记录
				String pay_Type = payType;
				String product_ID = productID;
				String totalMoney = money;
				String proLD = chLD;
				String type = orderType;
				String bill_no = Config.billno;
				String mechineID = Util.getSharePer(getApplicationContext(), "mechineID");
				String bz = "交易成功";
				json = Dao.returnJson(pay_Type, product_ID, totalMoney,proLD, type, bill_no, mechineID, bz, memberID,code);
				sendchSuccess(json);
				

			}
			// sendPortData("EF55FE0676000000007C");
			sendPortData("EF55FE037C007F");
			return;
		}
		if (str.trim().substring(0, 10).equals("FE55EF0376")) {
			Config.busy = 0;
			STATUS = 1;

			// 20191029 03:26:24 030:接收:交易成功<<<-:FE55EF03760079
			if (str.trim().substring(0, 12).equals("FE55EF037600")) {// 交易成功
				bzInfo = "交易成功";
				Log.i("1111","FE55EF037600");
				sendPortData("EF55FE0676000000007C");
				return;
			}
			//交易忙
			if (str.trim().substring(0, 12).equals("FE55EF037601")) {//
				Config.busy = 1;
				
				try {
					JSONObject jsonObject= new JSONObject();
					jsonObject.put(Config.MECHINE_ID, mechineID);
					jsonObject.put("cmd", "ch");
					jsonObject.put("SendMsg", GetBillObject());
					jsonObject.put("MsgId", "");
					JSONObject samtypeJson= new JSONObject();
					samtypeJson.put(Config.CMD, "chBusy");
					jsonObject.put("samtype", Getjiqigezhongxinxi(samtypeJson));
					Util.sendMsg(jsonObject.toString());
				} catch (JSONException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
				
				sendPortData("EF55FE0676000000007C");
				return;
			}
			if (str.trim().substring(0, 12).equals("FE55EF0376FF")) {// 交易失败

				if (chRecord) {
					chRecord = false;
					// 提交出货记录
					String json = "";
					String pay_Type = payType;
					String product_ID = productID;
					String totalMoney = money;
					String proLD = chLD;
					String type = orderType;
					String bill_no = Config.billno;
					String mechineID = Util.getSharePer(getApplicationContext(), "mechineID");
					String bz = "交易失败";
					json = Dao.returnJson(pay_Type, product_ID, totalMoney,proLD, type, bill_no, mechineID, bz, memberID,code);
					// 提交出货记录
					
					sendchFail(json);
				}

				bzInfo = "交易失败";
				sendPortData("EF55FE0676000000007C");
				return;
			}
			if (str.trim().substring(0, 12).equals("FE55EF0376FA")) {// 超时 出错
				recLogString = "超时出错";
				sendPortData("EF55FE0676000000007C");
				
				return;
			}
			if (str.trim().substring(0, 12).equals("FE55EF0376FE")) {// 校验错误

				bzInfo = "校验错误";
				sendPortData("EF55FE0676000000007C");
				return;
			}
			if (str.trim().substring(0, 12).equals("FE55EF0376F4")) {// 料道错误 //

				if (chRecord) {
					chRecord = false;
					// 提交出货记录
					String json = "";
					String pay_Type = payType;
					String product_ID = productID;
					String totalMoney = money;
					String proLD = chLD;
					String type = orderType;
					String bill_no = Config.billno;
					String mechineID = Util.getSharePer(getApplicationContext(), "mechineID");
					String bz = "料道错误";
					json = Dao.returnJson(pay_Type, product_ID, totalMoney,proLD, type, bill_no, mechineID, bz, memberID,code);
					// 提交出货记录
					sendchFail(json);
					
				}

				bzInfo = "料道错误";
				sendPortData("EF55FE0676000000007C");
				return;
			}
			if (str.trim().substring(0, 12).equals("FE55EF0376F3")) {// 料道错误

				if (chRecord) {
					chRecord = false;
					// 提交出货记录
					String json = "";
					String pay_Type = payType;
					String product_ID = productID;
					String totalMoney = money;
					String proLD = chLD;
					String type = orderType;
					String bill_no = Config.billno;
					String mechineID = Util.getSharePer(getApplicationContext(), "mechineID");
					String bz = "料道故障";
					json = Dao.returnJson(pay_Type, product_ID, totalMoney,proLD, type, bill_no, mechineID, bz, memberID,code);
					// 提交出货记录
					sendchFail(json);
				
				}

				bzInfo = "料道故障";
				sendPortData("EF55FE0676000000007C");
				return;
			}
			if (str.trim().substring(0, 12).equals("FE55EF0376F6")) {// 读卡器没准备好
				recLogString = "读卡器没准备好";

				sendPortData("EF55FE0676000000007C");
				return;
			}
			if (str.trim().substring(0, 12).equals("FE55EF0376F8")) {// 无法启动读卡器
				recLogString = "无法启动读卡器";

				sendPortData("EF55FE0676000000007C");
				return;
			}
			if (str.trim().substring(0, 12).equals("FE55EF0376F5")) {// 交易序列号相同
				recLogString = "交易序列号相同";

				if (chRecord) {
					chRecord = false;
					
					// 提交出货记录
					String json = "";
					String pay_Type = payType;
					String product_ID = productID;
					String totalMoney = money;
					String proLD = chLD;
					String type = orderType;
					String bill_no = Config.billno;
					String mechineID = Util.getSharePer(getApplicationContext(), "mechineID");
					String bz = "交易序列号相同";
					json = Dao.returnJson(pay_Type, product_ID, totalMoney,proLD, type, bill_no, mechineID, bz, memberID,code);
					// 提交出货记录
					sendchFail(json);
					
				}

				bzInfo = "交易序列号相同";
				sendPortData("EF55FE0676000000007C");
				return;
			}
			return;
		}
		
		
		Log.i("kkk", str);
		// Util.sendLog(getApplicationContext(), str);
	}

	public static String makeChecksum(String data) {
		if (data.length() > 6) {
			data = data.substring(6, data.length());
			if (data == null || data.equals("")) {
				return "00";
			}
			int total = 0;
			int len = data.length();
			int num = 0;
			while (num < len) {
				String s = data.substring(num, num + 2);
				total += Integer.parseInt(s, 16);
				num = num + 2;
			}
			String str = ("" + total);
			str = Integer.toHexString(Integer.parseInt(str));
			if (str.length() == 1) {
				str = "0" + str;
			}
			return lowerToUpper(str.substring(str.length() - 2, str.length()));
		} else {
			return "00";
		}
	}

	public static String lowerToUpper(String str) {
		char[] ch = str.toCharArray();
		for (int i = 0; i < ch.length; i++) {
			if (((int) ch[i] > 96) && ((int) ch[i] < 123)) {
				ch[i] = (char) ((int) ch[i] - 32);
			}
		}
		String childStr = String.valueOf(ch);
		return childStr;
	}

	public static void sendPortData(String sOut) {
		if (ComA != null && ComA.isOpen()) {
			Util.writeBillTxtToFile("发送:" + sendhex(sOut) + "-->>>:" + sOut);
			ComA.sendHex(sOut);
		}
	}

	// ------------------------------------------显示消息
	private void ShowMessage(String sMsg) {
		Intent intent = new Intent();
		intent.putExtra("toast", "" + sMsg);
		intent.setAction("com.tools.ui.toast");
		sendBroadcast(intent);
	}

	public static String getVerName(Context context) {
		String verName = "";
		try {
			verName = context.getPackageManager().getPackageInfo(
					"com.bjw.ComAssistant", 0).versionName;
		} catch (NameNotFoundException e) {
		}
		return verName;
	}

	
	private void sendchFail(String json) {
		try {
			JSONObject jsonObject= new JSONObject();
			jsonObject.put(Config.MECHINE_ID, mechineID);
			jsonObject.put("cmd", "ch");
			jsonObject.put("SendMsg", GetBillObject());
			jsonObject.put("MsgId", "");
			JSONObject samtypeJson= new JSONObject();
			samtypeJson.put(Config.CMD, "chFail");
			samtypeJson.put("upSellRecord",json);
			jsonObject.put("samtype", samtypeJson.toString());
			Util.sendMsg(jsonObject.toString());
		} catch (JSONException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}
	private void sendchSuccess(String json) {
		// 提交出货记录
		
		try {
			JSONObject jsonObject= new JSONObject();
			jsonObject.put(Config.MECHINE_ID, mechineID);
			jsonObject.put("cmd", "ch");
			jsonObject.put("SendMsg", GetBillObject());
			jsonObject.put("MsgId", "");
			JSONObject samtypeJson= new JSONObject();
			samtypeJson.put(Config.CMD, "chSuccess");
			samtypeJson.put("upSellRecord",json);
			jsonObject.put("samtype", samtypeJson.toString());
			Util.sendMsg(jsonObject.toString());
		} catch (JSONException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		
	}
	private String tenToSixTeen(int num){
		if(num==10){
			return "A";
		}else if (num==11) {
			return "B";
		}
		else if (num==12) {
			return "C";
		}
		else if (num==13) {
			return "D";
		}
		else if (num==14) {
			return "E";
		}
		else if (num==15) {
			return "F";
		}
		else if (num==16) {
			return "G";
		}else{
			
			return String.valueOf(num);
		}
		
	}
	
}
