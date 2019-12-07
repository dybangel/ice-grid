package com.bjw.ComAssistant;

import java.io.Console;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import java.util.zip.ZipEntry;
import java.util.zip.ZipOutputStream;

import org.apache.http.impl.conn.tsccm.ThreadSafeClientConnManager;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import android.R.integer;
import android.R.string;
import android.app.Service;
import android.content.ComponentName;
import android.content.Context;
import android.content.Intent;
import android.content.ServiceConnection;
import android.content.pm.PackageManager.NameNotFoundException;
import android.os.Environment;
import android.os.Handler;
import android.os.IBinder;
import android.os.Message;
import android.util.Log;

import com.google.gson.JsonObject;
import com.google.gson.JsonParser;
import com.sq.dao.Dao;
import com.sq.util.Config;
import com.sq.util.HttpDownloader;
import com.sq.util.SharedPreferencesHelper;
import com.sq.util.Util;

import de.tavendo.autobahn.WebSocketConnection;
import de.tavendo.autobahn.WebSocketException;
import de.tavendo.autobahn.WebSocketHandler;
import de.tavendo.autobahn.WebSocketOptions;

public class WebSocketService extends Service {
	// public static String WEB_SOCKET_HOST = Config.TCP_HOST;//
	// ������Ƕ˿ں��Լ����壨��˶��壩

	private static int is_tcp_online = 0;
	private static boolean is_loop_checking = false;// �Ƿ�ѭ�����
	private ArrayList<String> msgQueen = new ArrayList<String>();
	private static WebSocketOptions options = new WebSocketOptions();
	SharedPreferencesHelper sharedPreferencesHelper = null;
	public String mechineID = "";
	private UpdateManager mUpdateManager;// �������
	//ͨѶ���
	private static final String billLogPath = Environment.getExternalStorageDirectory() + "/asm/bill/";
	@Override
	public IBinder onBind(Intent intent) {
		showToast("DaemonServiece onCreate11");
		return null;
	}

	@Override
	public void onCreate() {
		super.onCreate();
		mechineID = Util.getSharePer(getApplicationContext(), "mechineID");
		mHanlder.postDelayed(task, 3000);// ��һ�ε���,�ӳ�1��ִ��task
	}

	public void showToast(String str) {
		Intent intent = new Intent();
		intent.putExtra("toast", "" + str);
		intent.setAction("com.tools.ui.toast");
		sendBroadcast(intent);
	}

	@Override
	public void onDestroy() {
		// TODO Auto-generated method stub
		super.onDestroy();

	}

	public void initWebSocket() {
		final String mechineID = Util.getSharePer(getApplicationContext(),
				"mechineID");
		Config.webSocketConnection = new WebSocketConnection();
		String WEB_SOCKET_HOST =Config.socketUrl; 
		if(WEB_SOCKET_HOST==null)
		{
			Util.writeBillTxtToFile("WEB_SOCKET_HOST:" +WEB_SOCKET_HOST+ "<<<-:");
			try {
				Thread.sleep(1000*60);
				initWebSocket();
			} catch (InterruptedException e) {
				 
				e.printStackTrace();
			}
		}
		try {
			Util.writeBillTxtToFile("WEB_SOCKET_HOST:" +WEB_SOCKET_HOST+ "<<<-:");
			Config.webSocketConnection.connect(WEB_SOCKET_HOST + "?mechineID="
					+ mechineID, new WebSocketHandler() {
				// websocket����ʱ��Ļص�
				@Override
				public void onOpen() {
					Log.i("3333", "onOpen");
					Config.socketStatus = 0;
					
					try {
						Thread.sleep(1000 * 1);
						
					} catch (InterruptedException e) {
						// TODO Auto-generated catch block
						e.printStackTrace();
					}
					
				}

				// websocket���յ���Ϣ��Ļص�
				@Override
				public void onTextMessage(String message) {
					try {
						
						Util.debuglog(message,"socket");
						Config.lastsockettime=new Date();
						JSONObject jsonObject = new JSONObject(message);
						String cmd = jsonObject.getString("cmd");
						if(cmd.equals("heartbeat")){
							if(PortService.videoListNo==""||PortService.productTypeNo==""||PortService.productListNo==""||PortService.priceSwitch==""){
								JSONObject startNeedJsonObject = new JSONObject();
								startNeedJsonObject.put("mechineID", mechineID);
								startNeedJsonObject.put("cmd", "startNeed");
								startNeedJsonObject.put("MsgId", "");
								JSONObject samtypeJson= new JSONObject();
								samtypeJson.put(Config.VERSION, getVerName(getApplicationContext()));
								samtypeJson.put(Config.VERCODE, getVerCode(getApplicationContext()));
								startNeedJsonObject.put("samtype", PortService.Getjiqigezhongxinxi(samtypeJson).toString());
								Log.i("1010","startNeed");
								Util.sendMsg(startNeedJsonObject.toString());
							}
							
							jsonObject.put("mechineID", mechineID);
							JSONObject samtypeJson= new JSONObject();
							samtypeJson.put(Config.VERSION, getVerName(getApplicationContext()));
							samtypeJson.put(Config.VERCODE, getVerCode(getApplicationContext()));
							jsonObject.put("samtype", PortService.Getjiqigezhongxinxi(samtypeJson).toString());
							
							Util.sendMsg(jsonObject.toString());
						}
						if(cmd.equals("updateVideoList")){
							Log.i("1010","updateVideoList");
							String sendMsgString=jsonObject.getString("SendMsg");
							Log.i("1010",sendMsgString);
							//�˴���֪��Ϊʲô�������ôд���ܻ�ȡ�Եĸ�ʽ
							JsonParser parser=new JsonParser();  //����JSON������
							JsonObject sendMsgObject=(JsonObject)parser.parse(sendMsgString);
							
							String androidInfoDetail=sendMsgObject.get("androidInfoDetail").getAsJsonArray().toString();
							Log.i("1010",androidInfoDetail);
							Util.setSharePer(getApplicationContext(), "videoInfo", androidInfoDetail);
							PortService.videoListNo=sendMsgObject.get("androidNo").getAsString();
							new DownLoadVideo().start();
							
							
						}
						if(cmd.equals("updateProductType")){
						
							String sendMsgString=jsonObject.getString("SendMsg");
							
							//�˴���֪��Ϊʲô�������ôд���ܻ�ȡ�Եĸ�ʽ
							JsonParser parser=new JsonParser();  //����JSON������
							JsonObject sendMsgObject=(JsonObject)parser.parse(sendMsgString);
							
							String androidInfoDetail=sendMsgObject.get("androidInfoDetail").getAsJsonArray().toString();
							
							Util.setSharePer(getApplicationContext(), "productType", androidInfoDetail);
							PortService.productTypeNo=sendMsgObject.get("androidNo").getAsString();
							if(PortService.chRecord==false){
								final Intent intent = new Intent();
								intent.setAction(Config.ACTION_PRODUCTTYPE);
								intent.putExtra("TYPE_FLAG", true);
								sendBroadcast(intent);
							}
							
							
							
						}
						if(cmd.equals("updateProductList")){
							Log.i("1010","updateProductList");
							
							String sendMsgString=jsonObject.getString("SendMsg");
							//�˴���֪��Ϊʲô�������ôд���ܻ�ȡ�Եĸ�ʽ
							JsonParser parser=new JsonParser();  //����JSON������
							JsonObject sendMsgObject=(JsonObject)parser.parse(sendMsgString);
							String productInfo=sendMsgObject.get("androidInfoDetail").getAsJsonArray().toString();
							Util.setSharePer(getApplicationContext(), "productInfo", productInfo);
							Util.setSharePer(getApplicationContext(), "priceSwitch", sendMsgObject.get("priceSwitch").getAsString());
							PortService.productListNo=String.valueOf(sendMsgObject.get("androidNo").getAsInt());
							PortService.priceSwitch=sendMsgObject.get("priceSwitch").getAsString();
							Thread td2 = new Thread(downProduct, "downProduct");// ������Ʒ��Ϣ
							td2.start();
							
							
						}
						//��ѯ�¶ȵ�����
						if(cmd.equals("searchTem")){
							String samtype=jsonObject.getString("samtype");
							if(samtype==null||samtype==""||samtype=="null"){
								PortService.is_search_tem=true;
								PortService.is_return_tem=false;
								samtype="0";
							}
							if(!PortService.is_return_tem){
								
								if(Integer.parseInt(samtype)==5){
									return;
									
								}else{
									samtype=String.valueOf(Integer.parseInt(samtype)+1);
									
								}
								Thread.sleep(1000 * 2);
								
								jsonObject.put("samtype",samtype);
								onTextMessage(jsonObject.toString());
							}else{
								Log.i("1010","else");
								jsonObject.put("mechineID", mechineID);
								JSONObject samtypeJson= new JSONObject();
								samtypeJson.put(Config.VERSION, getVerName(getApplicationContext()));
								jsonObject.put("samtype", PortService.Getjiqigezhongxinxi(samtypeJson).toString());
								
								Util.sendMsg(jsonObject.toString());
								return;
							}
							
						}
						
						if(cmd.equals("chEnd")){
							
							Log.i("1010","chEnd"+message);
							if(PortService.chRecord==false){
								PortService.chEnd=true;
							}
							
						}
						//���ٳ�������ʱ�������
						if(cmd.equals("updateSoft")){


							if(PortService.chRecord==false){
								Log.i("1010","updateSoft"+message);
								String sendMsgString=jsonObject.getString("SendMsg");
								//�˴���֪��Ϊʲô�������ôд���ܻ�ȡ�Եĸ�ʽ
								JsonParser parser=new JsonParser();  //����JSON������
								JsonObject sendMsgObject=(JsonObject)parser.parse(sendMsgString);
								int newVerCode=sendMsgObject.get("newVerCode").getAsInt();
								String downUrl=sendMsgObject.get("downUrl").getAsString();
								String newsoftversion=sendMsgObject.get("newsoftversion").getAsString();
								// app����
								mUpdateManager = new UpdateManager(getApplicationContext());
								mUpdateManager.mechineID=mechineID;
								mUpdateManager.newVerCode=newVerCode;
								mUpdateManager.newVerName=newsoftversion;
								mUpdateManager.apkUrl=downUrl;
								if (mUpdateManager.getServerVer(getApplicationContext())) {
	
								}
							}
							
						}
						
						
						if ("ch".equals(cmd)) {
							if(jsonObject.getString("samtype")=="null"){
								Log.i("1010","111"+message);
								jsonObject.put(Config.MECHINE_ID, mechineID);
								JSONObject samtypeJson= new JSONObject();
								samtypeJson.put(Config.CMD, "chReceive");
								jsonObject.put("samtype", PortService.Getjiqigezhongxinxi(samtypeJson).toString());
								
								Util.sendMsg(jsonObject.toString());
							}
							Log.i("1010","111"+22222);
							String sendMsgString=jsonObject.getString("SendMsg");
							Log.i("1010","111"+5555);
							JSONObject chJsonObject = new JSONObject(sendMsgString);
							String cmdaa=chJsonObject.getString("cmd");
							String type=chJsonObject.getString("type");
							Log.i("1010","111"+chJsonObject.toString());
							
							Log.i("1010","111"+6666);
							
							
							
							
							
							if ("2".equals(type)) {
								String billno = chJsonObject.getString("billno");
								if (Config.billno.equals(billno)) {
									return;// ��ֹ��������
								}
							}
							
							if (PortService.STATUS == 0)// ��ѯ״̬
							{
								Log.i("1010","222"+message);
								if ("2".equals(type)) {

									String ldNO = chJsonObject.getString("ldNO");
									String billno = chJsonObject
											.getString("billno");
									String payType = chJsonObject
											.getString("payType");
									String productID = chJsonObject
											.getString("productID");
									String money = chJsonObject
											.getString("money");
									Intent intent = new Intent(
											getApplicationContext(),
											PortService.class);
									intent.putExtra("is_ch", true);
									intent.putExtra("ldNO", ldNO);
									intent.putExtra("billno", billno);
									intent.putExtra("payType", payType);
									intent.putExtra("productID", productID);
									intent.putExtra("money", money);
									intent.putExtra("orderType", type);
									intent.putExtra("memberID", "");
									intent.putExtra("code", "");
									Config.billno = billno;
									Config.type = 2;
									int count = 0;
									String strTxt=	"���ۣ�������"+mechineID+"���׺�:"+Config.billno+"���ϵ����"+ldNO;

									
									jsonObject.put(Config.MECHINE_ID, mechineID);
									JSONObject samtypeJson= new JSONObject();
									samtypeJson.put(Config.CMD, "chForeach");
									jsonObject.put("samtype", PortService.Getjiqigezhongxinxi(samtypeJson).toString());
									Util.sendMsg(jsonObject.toString());
									startService(intent);
								} else if ("1".equals(type)) {
									Log.i("1010","1"+1111111);
									//ldNO=50;mechineID=67;memberID=498;productID=424;code=531907
									Config.billno="";
									String ldNO = chJsonObject.getString("ldNO");
									String memberID = chJsonObject
											.getString("memberID");
									String productID = chJsonObject
											.getString("productID");
									String mechineID = chJsonObject
											.getString("mechineID");
									String code = chJsonObject.getString("code");
									String money = chJsonObject.getString("money");
									Intent intent = new Intent(
											getApplicationContext(),
											PortService.class);
									Log.i("1010","1"+2222222);
									intent.putExtra("is_ch", true);
									intent.putExtra("ldNO", ldNO);
									intent.putExtra("productID", productID);
									intent.putExtra("memberID", memberID);
									intent.putExtra("mechineID", mechineID);
									intent.putExtra("orderType", type);
									intent.putExtra("money", money);
									intent.putExtra("code", code);
									intent.putExtra("billno", "");
									intent.putExtra("payType", "");
									Config.type = 1;
									Config.memberID = memberID;
									int count = 0;
									String strTxt=	"������������"+mechineID+"���ϵ����"+ldNO;
									Log.i("1010","1"+333333333);
									jsonObject.put(Config.MECHINE_ID, mechineID);
									JSONObject samtypeJson= new JSONObject();
									samtypeJson.put(Config.CMD, "chForeach");
									jsonObject.put("samtype", PortService.Getjiqigezhongxinxi(samtypeJson).toString());
									Log.i("1010","1"+4444444);
									Util.sendMsg(jsonObject.toString());
									startService(intent);
									
								}
								
								

							} else {
								// Threa

								Thread.sleep(1000 * 2);
								jsonObject.put("samtype","again");
								onTextMessage(jsonObject.toString());

							}

						}
						if ("pack".equals(cmd)) {
							try {
								String sendMsgString=jsonObject.getString("SendMsg");
								//�˴���֪��Ϊʲô�������ôд���ܻ�ȡ�Եĸ�ʽ
								JsonParser parser=new JsonParser();  //����JSON������
								JsonObject sendMsgObject=(JsonObject)parser.parse(sendMsgString);
								String packTime=sendMsgObject.get("packTime").getAsString();
								Util.ZipFolder(billLogPath+packTime,billLogPath+packTime+".zip");
								Util.delete(billLogPath+packTime);
							} catch (Exception e) {
								// TODO Auto-generated catch block
								e.printStackTrace();
							}
						
						}
					} catch (Exception e) {
						Log.i("1010","1"+"cuo");
						Util.writeBillTxtToFile("����<<<-:");
						// TODO: handle exception
					}
				}

				// websocket�ر�ʱ��Ļص�
				@Override
				public void onClose(int code, String reason) {
					Log.i("3333", "onClose");
					is_tcp_online = 0;
					Config.socketStatus = 1;
				}
			}, options);
		} catch (WebSocketException e) {
			e.printStackTrace();
		}
	}
	private Runnable downProduct = new Runnable() {
		@Override
		public void run() {
			
				down_productImg();
				if(PortService.chRecord==false){
					final Intent intent = new Intent();
					intent.setAction(Config.ACTION_UPDATEUI_IMG);
					intent.putExtra("IMG_FLAG", true);
					sendBroadcast(intent);
					Log.i("5555", "������ɷ��͹㲥");
				}
				
				
			
		}
	};
	public void returnServer(String billno) {
		JSONObject reqJson = new JSONObject();
		try {
			reqJson.put(Config.CMD, "chBillno");
			reqJson.put(Config.MECHINE_ID, mechineID);
			reqJson.put("billno", billno);
			reqJson.put("MsgId", "");
			reqJson.put(Config.VERSION, getVerName(getApplicationContext()));
			Util.sendMsg(reqJson.toString());
		} catch (JSONException e) {
			e.printStackTrace();
		}
	}

	// �رղ���
	public void closeWebsocket() {
		if (Config.webSocketConnection != null
				&& Config.webSocketConnection.isConnected()) {
			Util.debuglog("���ر�<<<-:", "�ر�");
			Config.webSocketConnection.disconnect();
			Config.webSocketConnection = null;
			Config.socketStatus = 1;
		}
	}

	public void log_to_server(String info) {
		JSONObject reqJson = new JSONObject();
		try {
			reqJson.put("cmd", "app.log");
			reqJson.put("mechineID", mechineID);
			reqJson.put("info", info);
			reqJson.put("MsgId", "");
			Util.sendMsg(reqJson.toString());
		} catch (JSONException e) {
			e.printStackTrace();
		}
	}

	public int calLastedTime(Date startDate) {
		try {
			long a = new Date().getTime();
			long b = startDate.getTime();
			int c = (int) ((a - b) / 1000);
			return c;
		} catch (Exception e) {
			return 0;
		}
		
	}

	public void checkWebsocket() {
		Log.i("3333", "checkWebsocket");
		closeWebsocket();
		initWebSocket();
	}

	private Handler mHanlder = new Handler() {
		@Override
		public void handleMessage(Message msg) {
			switch (msg.what) {
			case 1:
				Log.i("3333", "socketStatus"+Config.socketStatus);
				if (Config.socketStatus == 1) {
					checkWebsocket();
				}
				break;
			default:
				break;
			}
			super.handleMessage(msg);
		}
	};
	private Runnable task = new Runnable() {
		@Override
		public void run() {
			/**
			 * �˴�ִ������
			 * */
			mHanlder.sendEmptyMessage(1);
			mHanlder.postDelayed(this, 5 * 1000);// �ӳ�5��,�ٴ�ִ��task����,ʵ����ѭ����Ч��
		}
	};

	@Override
	public int onStartCommand(Intent intent, int flags, int startId) {
		sharedPreferencesHelper = new SharedPreferencesHelper(
				WebSocketService.this);
		
		closeWebsocket();
		initWebSocket();
		// ���Service����ֹ
		// ����Դ��������£�����service
		return START_STICKY;
	}

	/**
	 * ��ȡ�汾��
	 * 
	 * @param context
	 * @return
	 */
	public static String getVerName(Context context) {
		String verName = "";
		try {
			verName = context.getPackageManager().getPackageInfo(
					"com.bjw.ComAssistant", 0).versionName;
		} catch (NameNotFoundException e) {
		}
		return verName;
	}
	public static int getVerCode(Context context) {
		int verCode = -1;
		try {
			verCode = context.getPackageManager().getPackageInfo("com.bjw.ComAssistant", 0).versionCode;

		} catch (NameNotFoundException e) {

		}
		return verCode;
	}
	/***
	 * ������ƷͼƬ
	 */
	public void down_productImg() {
		try {
			HttpDownloader httpDownloader = new HttpDownloader();
			String productInfo= Util.getSharePer(getApplicationContext(), "productInfo");
			String companyID= Util.getSharePer(getApplicationContext(), "companyID");
			if(productInfo==null&&"".equals(productInfo))
			{
				return ;
			}
			List<File> bdFileList = new ArrayList<File>();
			File f = new File(Config.PRODUCT_IMAGE_SAVE_PATH);
			bdFileList = Util.getFile(f);
			for (int i = 0; i < bdFileList.size(); i++) {
				File file = bdFileList.get(i);
				if (!productInfo.contains(file.getName())) {
					// ɾ�����ص���Ƶ
					Util.deleteFile(Config.PRODUCT_IMAGE_SAVE_PATH+ file.getName());
				}
			}
			JSONArray jsonArray = new JSONArray(productInfo);
			for(int i=0;i<jsonArray.length();i++)
			{
				Log.i("1010",i+"jsonArray");
				JSONObject jsonObject = jsonArray.getJSONObject(i);
				String httpImageUrl=jsonObject.getString("httpImageUrl");
				String path=jsonObject.getString("path");
				int downloadResult = httpDownloader.downloadFiles(httpImageUrl, Config.PRODUCT_IMAGE_SAVE_PATH, path.replace("img/", ""));
				
				//���ض�ά��ͼƬ
				httpDownloader.downloadFiles("http://nq.bingoseller.com/qrcode/"+companyID+".png", Config.PRODUCT_IMAGE_SAVE_PATH_QR, companyID+".png");
			}
		} catch (Exception e) {
			
		}
	}
	/**
	 * ������Ƶ �������˲����ڵ���Ҫɾ��
	 * @author Administrator
	 *
	 */
	private class DownLoadVideo extends Thread {
		public void run() {
			try {
				// �жϱ����Ƿ��и���Ƶ �еĻ��Ͳ���Ҫ���� ȡ��json��
				String videoInfo= Util.getSharePer(getApplicationContext(), "videoInfo");
				if(videoInfo==null&&"".equals(videoInfo))
				{
					return ;
				}
				JSONArray jsonArray = new JSONArray(videoInfo);
				for(int i=0;i<jsonArray.length();i++)
				{
					//�жϺ������ֿ�����0 ���� 1����
					JSONObject jsonObject = jsonArray.getJSONObject(i);
					String type=jsonObject.getString("type");
					String path=jsonObject.getString("path");
					String name=jsonObject.getString("name");
					HttpDownloader httpDownloader = new HttpDownloader();
					if("0".equals(type))
					{
						//����  // downloadResult 0���سɹ�1 �ļ��Ѿ�����-1 ����ʧ��
						int downloadResult = httpDownloader.downloadFiles(path, Config.OPEN_HVIDEO_PATH,name);
					}else if("1".equals(type)) {
						//����
						int downloadResult = httpDownloader.downloadFiles(path, Config.OPEN_VVIDEO_PATH,name);
					}
				}
				List<File> bdFileList = new ArrayList<File>();//��ű�����Ƶ�б�
				// ѭ���ļ����µ�������Ƶ��������е���Ƶvideolist���ڲ����� ��Ѹ���Ƶɾ�� �������
				File f = new File(Util.getHVideoPath(getApplicationContext()));
				bdFileList = Util.getFile(f);
				for (int i = 0; i < bdFileList.size(); i++) {
					File file = bdFileList.get(i);
					//���Ը������µ�json���Ƿ�������ش�ŵ���Ƶ���� �����������ɾ��
					if (!videoInfo.contains(file.getName())) {
						// ɾ�����ص���Ƶ
						Util.deleteFile(Util.getHVideoPath(getApplicationContext())
								+ file.getName());
					}
				}
				// �������
				File vf = new File(Util.getVVideoPath(getApplicationContext()));
				List<File> vf_fileList = Util.getFile(vf);
				for (int i = 0; i < vf_fileList.size(); i++) {
					File file = vf_fileList.get(i);
					if (!videoInfo.contains(file.getName())) {
						// ɾ�����ص���Ƶ
						Util.deleteFile(Util.getVVideoPath(getApplicationContext())
								+ file.getName());
					}
				}
			} catch (Exception e) {
				
			}
		}
	}
	
}
