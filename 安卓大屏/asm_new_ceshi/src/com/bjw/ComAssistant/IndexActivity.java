package com.bjw.ComAssistant;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import java.util.Map;
import java.util.Set;
import java.util.TimeZone;
import java.util.Timer;
import java.util.TimerTask;
import java.util.zip.ZipEntry;
import java.util.zip.ZipOutputStream;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import android.R.integer;
import android.annotation.SuppressLint;
import android.app.Activity;
import android.app.ActivityManager;
import android.app.AlertDialog;
import android.app.Dialog;
import android.app.ProgressDialog;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.DialogInterface;
import android.content.DialogInterface.OnClickListener;
import android.content.Intent;
import android.content.pm.PackageManager.NameNotFoundException;
import android.content.res.Configuration;
import android.graphics.Bitmap;
import android.media.MediaPlayer;
import android.os.Bundle;
import android.os.Environment;
import android.os.Handler;
import android.os.Message;
import android.telephony.PhoneStateListener;
import android.telephony.SignalStrength;
import android.telephony.TelephonyManager;
import android.text.Html;
import android.util.Log;
import android.view.Display;
import android.view.Gravity;
import android.view.KeyEvent;
import android.view.Window;
import android.view.WindowManager;
import android.webkit.ConsoleMessage;
import android.webkit.JavascriptInterface;
import android.webkit.JsResult;
import android.webkit.WebChromeClient;
import android.webkit.WebSettings;
import android.webkit.WebStorage.QuotaUpdater;
import android.webkit.WebView;
import android.webkit.WebViewClient;
import android.widget.ImageView;
import android.widget.TextView;

import com.lidroid.xutils.util.LogUtils;
import com.sq.dao.Dao;
import com.sq.util.Config;
import com.sq.util.NetUtils;
import com.sq.util.Util;
import com.sq.util.cc;


@SuppressLint("SetJavaScriptEnabled")
public class IndexActivity extends Activity {
	
	//网络状态的
	public TextView mTextView;
    public TelephonyManager mTelephonyManager;
    public PhoneStatListener mListener;

	private static WebView indexWebView;
	private static String url = "file:///android_asset/index.html";
	public static final String ACTION_UPDATEUI = "action.updateUI";
	public static final String PORT_ACTION = "action.port";
	public static final String DIALOG_ACTION = "action.dialog";
	public static final String LOADING_ACTION = "action.loading";
	private ProgressDialog progressDialog;
	public static int status = 1;// 0不忙 1忙
	public static Date lastClickDate;// 最后一次点击屏幕时间
	public static int indexPage = 0;// 0显示的是首页 1显示的竖屏页
	private Timer timerTT = new Timer(true);
	private Timer timerUploadLocation = new Timer(true);
	public String videoStr = "";// 记录最后一次上传视频的字符串防止重复提交
	private UpdateManager mUpdateManager;// 软件更新
	private TextView timeTxt;
	private static TextView leftTxt;
	private static TextView chlcTxt;
	private static int indexCount = 0;// 唤醒屏幕
	private static int smCount = 0;// 扫码页面
	private static int productCount = 0;// 选择商品页面
	private static int endCount = 0;// 支付完成页面
	private static String productStr = "";// 查看的产品ID
	public static String locationStr = "";// 地理位置信息
	public static final int REFRESH = 3;
	public static boolean b = true;// 线程mythread开关
	public MyThread myThread;
	public static MediaPlayer mMediaPlayer;

	private TextView mOffTextView;
	private Handler mOffHandler;
	private Timer mOffTime;
	public static int socketNum=0 ;
	public static boolean alertStatus=false;//alert状态1弹0未弹
	

	@SuppressWarnings("deprecation")
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		
        
		// TODO Auto-generated method stub
		super.onCreate(savedInstanceState);
		setContentView(R.layout.index);
		
		
		mTextView = (TextView) findViewById(R.id.levelTxt);
        //获取telephonyManager
        mTelephonyManager = (TelephonyManager) getSystemService(Context.TELEPHONY_SERVICE);
        //开始监听
        mListener = new PhoneStatListener();
        //监听信号强度
        mTelephonyManager.listen(mListener, PhoneStatListener.LISTEN_SIGNAL_STRENGTHS);
        
		
		
		timeTxt = (TextView) findViewById(R.id.timeTxt);
		leftTxt = (TextView) findViewById(R.id.leftTxt);
		chlcTxt=(TextView) findViewById(R.id.chlcTxt);
		indexWebView = (WebView) findViewById(R.id.indexweb);
		WebSettings webSettings = indexWebView.getSettings();
		// webSettings.setUseWideViewPort(true);// 设置此属性，可任意比例缩放
		webSettings.setLoadWithOverviewMode(true);
		webSettings.setJavaScriptEnabled(true);// 表示webview 可以执行服务器端接收代码
		// 允许webview对文件的操作
		webSettings.setAllowUniversalAccessFromFileURLs(true);
		webSettings.setAllowFileAccess(true);
		webSettings.setAllowFileAccessFromFileURLs(true);
		webSettings.setDefaultTextEncodingName("utf-8");
		// webSettings.setDomStorageEnabled(true);
		webSettings.setPluginState(WebSettings.PluginState.ON);
		webSettings.setAppCacheMaxSize(1024 * 1024 * 8);
		String appCachePath = getApplicationContext().getCacheDir()
				.getAbsolutePath();
		webSettings.setAppCachePath(appCachePath);
		webSettings.setAppCacheEnabled(true);
		webSettings.setBlockNetworkImage(false);
		webSettings.setMediaPlaybackRequiresUserGesture(false);
		checkService();

		indexWebView.setWebChromeClient(new WebChromeClient() {
			@Override
			public void onReachedMaxAppCacheSize(long requiredStorage,
					long quota, QuotaUpdater quotaUpdater) {
				// TODO Auto-generated method stub
				quotaUpdater.updateQuota(requiredStorage * 2);
			}

			@Override
			public boolean onConsoleMessage(ConsoleMessage cm) {
				Log.i("console", cm.message());
				return true;
			}

			@Override
			public void onConsoleMessage(String message, int lineNumber,
					String sourceID) {
				super.onConsoleMessage(message, lineNumber, sourceID);
			}

			@Override
			public boolean onJsAlert(WebView view, String url, String message,
					final JsResult result) {
				mOffTextView = new TextView(IndexActivity.this);
				AlertDialog.Builder builder = new AlertDialog.Builder(
						IndexActivity.this);
				builder.setCancelable(false);
				builder.setTitle("提示");
				builder.setMessage(message);
				builder.setView(mOffTextView);
				builder.setPositiveButton("OK", new OnClickListener() {
					@Override
					public void onClick(DialogInterface dialog, int which) {
						mOffTime.cancel();
						result.confirm();// 表示确认用户的选择
						alertStatus=false;
					}
				});
				final AlertDialog dialog = builder.create();
				dialog.show();
				alertStatus=true;
				// 放在show()之后，不然有些属性是没有效果的，比如height和width
				Window dialogWindow = dialog.getWindow();
				WindowManager m = getWindowManager();
				Display d = m.getDefaultDisplay(); // 获取屏幕宽、高
				WindowManager.LayoutParams p = dialogWindow.getAttributes(); // 获取对话框当前的参数值
				// 设置宽度
				p.width = (int) (d.getWidth() * 0.5); // 宽度设置为屏幕的0.95
				p.gravity = Gravity.CENTER;// 设置位置
				// p.alpha = 0.8f;//设置透明度
				dialogWindow.setAttributes(p);
				result.cancel();
				mOffHandler = new Handler() {
					public void handleMessage(Message msg) {
						if (msg.what > 0) {
							// //动态显示倒计时
							 String str2 = "<font color='#F8F8FF'><big>"+msg.what+"s 后自动关闭"+"</big></font>";
							 mOffTextView.setPadding(380, 0, 0, 0);
							 mOffTextView.setText(Html.fromHtml(str2));
						} else {
							// //倒计时结束自己主动关闭
							if (dialog != null) {
								dialog.dismiss();
							}
							mOffTime.cancel();
							alertStatus=false;
						}
						super.handleMessage(msg);
					}
				};
				mOffTime = new Timer(true);
				TimerTask tt = new TimerTask() {
					int countTime = 3;
					public void run() {
						if (countTime > 0) {
							countTime--;
						}
						Message msg = new Message();
						msg.what = countTime;
						mOffHandler.sendMessage(msg);
					}
				};
				mOffTime.schedule(tt, 0, 1000);
				return true;
			}
		});
		indexWebView.addJavascriptInterface(new InJavaScriptLocalObj(),
				"local_obj");
		indexWebView.setWebViewClient(new WebViewClient() {
			@Override
			public void onPageStarted(WebView view, String url, Bitmap favicon) {
				// TODO Auto-generated method stub
				super.onPageStarted(view, url, favicon);
			}

			@Override
			public boolean shouldOverrideUrlLoading(WebView view, String url) {
				// TODO Auto-generated method stub
				return super.shouldOverrideUrlLoading(view, url);
			}

			@Override
			public void onPageFinished(WebView view, String url) {

				super.onPageFinished(view, url);
			}

			@Override
			public void onReceivedError(WebView view, int errorCode,
					String description, String failingUrl) {
				super.onReceivedError(view, errorCode, description, failingUrl);
			}
		});
		indexWebView.loadUrl(url);
		indexPage = 0;
		if (Config.flag) {
			lastClickDate = new Date();
			timerUploadLocation.schedule(taskUpLoadLocation, 5000,
					1000 * 60 * 30);// 30分钟上传一次地理位置信息
			timerTT.schedule(taskTT, 1000, 1000);
			Config.flag = false;
			mMediaPlayer = MediaPlayer.create(this, R.raw.selectproduct);
			mMediaPlayer.start();
		}
		timeTxt.setText("当前版本:" + getVerName(IndexActivity.this));
		myThread = new MyThread();
		myThread.start();
		
		
			
		
	}
	
	public class MyThread extends Thread {
		public void run() {
			while (1==1) {
				Message msg = new Message();
				msg.what = REFRESH;
				msg.obj = this;
				handler.sendMessage(msg);
				try {
					Thread.sleep(1000 * 5);
				} catch (InterruptedException e) {
					Thread.currentThread().interrupt();
				}
			}
		}
	}

	public void checkService() {
		Log.i("5555", "checkService");
		// 启动服务
		Intent intent = new Intent(this, UpdateSoftService.class);
		if (intent == null
				|| cc.isServiceRunning(getApplicationContext(),
						"com.bjw.ComAssistant.UpdateSoftService") == false) {
			Log.i("5555", "1111");
			IndexActivity.this.startService(intent);
		}
		Intent intent1 = new Intent(this, WebSocketService.class);
		if (intent1 == null
				|| cc.isServiceRunning(getApplicationContext(),
						"com.bjw.ComAssistant.WebSocketService") == false) {
			Log.i("5555", "22222");
			IndexActivity.this.startService(intent1);
		}
		Intent intent2 = new Intent(this, PortService.class);
		if (intent2 == null
				|| cc.isServiceRunning(getApplicationContext(),
						"com.bjw.ComAssistant.PortService") == false) {
			Log.i("5555", "33333");
			IndexActivity.this.startService(intent2);
		}
//		Intent intent3 = new Intent(this, UpdateProductService.class);
//		if (intent3 == null
//				|| cc.isServiceRunning(getApplicationContext(),
//						"com.bjw.ComAssistant.UpdateProductService") == false) {
//			Log.i("5555", "44444");
//			IndexActivity.this.startService(intent3);
//		}
//		Intent intent4 = new Intent(this, UpdateVideoService.class);
//		if (intent4 == null
//				|| cc.isServiceRunning(getApplicationContext(),
//						"com.bjw.ComAssistant.UpdateVideoService") == false) {
//			Log.i("5555", "6666");
//			IndexActivity.this.startService(intent4);
//		}
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

	public static class uiReceiver extends BroadcastReceiver {
		@Override
		public void onReceive(final Context context, Intent intent) {
			String action = intent.getAction();
			if (action.equals("com.tools.ui.toast")) {

			} else if (action.equals(DIALOG_ACTION)) {

			} else if (action.equals(LOADING_ACTION)) {

			} else if (action.equals(ACTION_UPDATEUI)) {
				locationStr = intent.getStringExtra("msg");

			} else if (action.equals(PORT_ACTION)) {
				try {
					status = intent.getIntExtra("status", 0);// 机器当前状态
					//leftTxt.setText(intent.getStringExtra("hex"));;
				} catch (Exception e) {

				}
			} else if (action.equals(Config.ACTION_UPDATEUI_IMG)) {
				// 更新产品列表 刷新页面
				try {
					if (indexPage == 0 && indexWebView != null) {
						
						indexWebView.loadUrl(url);
					}

				} catch (Exception e) {

				}
			} else if (action.equals(Config.ACTION_PRODUCTTYPE)) {
				// 更新产品类别
				try {
					if (indexPage == 0 && indexWebView != null) {
						indexWebView.loadUrl(url);
					}

				} catch (Exception e) {

				}
			} else if (action.equals(Config.ACTION_JUMP_URL)) {
				// type=1 跳转到倒计时页面 type=2 跳转到产品列表页面
				try {
					String type = intent.getStringExtra("TYPE").toString();
					if (type.equals("1")) {
						mMediaPlayer = MediaPlayer.create(context,
								R.raw.playing);
						mMediaPlayer.start();
						endCount++;
						upLLRecord(context);// 跳转到出货页面
					} else if (type.equals("2")) {
						indexCount++;
					}
					indexWebView.loadUrl("javascript:jumpUrl('" + type + "')");
				} catch (Exception e) {

				}
			}
		}
	};

	final class InJavaScriptLocalObj {
		@JavascriptInterface
		public void showHtml(String html) {
			Log.d("HTML", html);
		}

		@JavascriptInterface
		public String playMp3(String type) {
			if ("1".equals(type)) {
				mMediaPlayer = MediaPlayer.create(getApplicationContext(),
						R.raw.selectproduct);
				mMediaPlayer.start();
			} else if ("2".equals(type)) {
				mMediaPlayer = MediaPlayer.create(getApplicationContext(),
						R.raw.scanget);
				mMediaPlayer.start();
			}
			return "";
		}

		@JavascriptInterface
		public String getMechineID() {
			return Util.getSharePer(IndexActivity.this, "mechineID");
		}

		@JavascriptInterface
		public void showSource(String html) {

		}

		@JavascriptInterface
		public String dgCh(String code) {
			if (status == 1) {
				return "300";
			}
			Dao dao = new Dao();
			String result = dao.dgCh(code,
					Util.getSharePer(IndexActivity.this, "mechineID"));

			return result;
		}

		@JavascriptInterface
		public String testKC(String productID, String dgOrderDetailID) {
			lastClickDate = new Date();
			Dao dao = new Dao();
			productStr += productID + ",";
			String resultString = dao.getKC(
					Util.getSharePer(IndexActivity.this, "mechineID"),
					productID, dgOrderDetailID, IndexActivity.this);
			// 如果返回code=200播放语音提示
			if ("200".equals(resultString)) {
				mMediaPlayer = MediaPlayer.create(getApplicationContext(),
						R.raw.pay);
				mMediaPlayer.start();
			}
			return resultString;
		}

		@JavascriptInterface
		public String getStatus() {
			if (status == 0) {
				// 播放语音加返回
				return status + "";// 返回机器状态
			}
			return status + "";// 返回机器状态
		}
		@JavascriptInterface
		public boolean getAlertStatus() {
			
				return alertStatus;// 返回alert状态
		}
		@JavascriptInterface
		public String getReqsn(String companyID,String mechineID,String productID,String sftj,String product,String dgOrderDetailID,String type) {
			Dao dao = new Dao();
			
			String resultString = dao.getReqsn(companyID,
					Util.getSharePer(IndexActivity.this, "mechineID"),productID,sftj,product,dgOrderDetailID,type, IndexActivity.this);
			
			return resultString;
		}
		
		@JavascriptInterface
		public void addClickTime(String val) {
			// val=1 首页 =2扫码付款页面 =3 扫码支付页面 =4 支付完成页面
			lastClickDate = new Date();
			Log.i("1111", "点击了屏幕=" + lastClickDate + ";index=" + indexPage);
			indexPage = 0;
			SimpleDateFormat sfDateFormat = new SimpleDateFormat(
					"yyyy-MM-dd HH:mm:ss");
			if ("1".equals(val)) {
				// 更新下产品页面
				try {
					Message msg = Message.obtain();
					msg.what = 4;
					handler.sendMessage(msg);
				} catch (Exception e) {
					Log.i("5555", e.getMessage());
				}

			}
		}

		@JavascriptInterface
		public void updateCount(String val) {
			if (val.equals("1")) {
				indexCount++;
			} else if (val.equals("2")) {
				smCount++;
			} else if (val.equals("3")) {
				productCount++;
			} else if (val.equals("4")) {
				// endCount++;
			}
		}

		@JavascriptInterface
		public void updateIndexCount() {
			indexCount++;
		}

		@JavascriptInterface
		public String updatePro(String productType, String pageNO) {
			String productInfo = Util.getSharePer(IndexActivity.this,
					"productInfo");
			Log.i("9999", "productInfo="+productInfo);
			String priceSwich = Util.getSharePer(IndexActivity.this,
					"priceSwitch");
			String htmlString = "";
			int totalCount = 0;
			int pageSize = 9;// 每页显示9条
			if (productInfo != null && !"".equals(productInfo)) {
				try {
					String mechineID = Util.getSharePer(IndexActivity.this,
							"mechineID");
					String companyID = Util.getSharePer(IndexActivity.this,
							"companyID");
					JSONArray jsonArray = new JSONArray(productInfo);
					List<JSONObject> list = new ArrayList<JSONObject>();
					for (int i = 0; i < jsonArray.length(); i++) {
						JSONObject object = jsonArray.getJSONObject(i);
						String product_Type = object.getString("protype");
						if ("0".contentEquals(productType)) {
							list.add(object);
						} else {
							if (productType.equals(product_Type)) {
								list.add(object);
							}
						}
					}
					if (list.size() > 0) {
						totalCount = list.size();
						int initNum = (Integer.parseInt(pageNO) - 1) * pageSize;
						int record = Integer.parseInt(pageNO) * pageSize;
						if (record > list.size()) {
							record = list.size();
						}
						for (int i = initNum; i < record; i++) {
							JSONObject object = list.get(i);
							String path = Environment
									.getExternalStorageDirectory()
									+ "/asm/image/"
									+ object.getString("path").replace("/img/",
											"");
							String productName = object.getString("proName");
							String price = object.getString("price0");
							String price1 = object.getString("price1");
							String price2 = object.getString("price2");
							String price3 = object.getString("price3");
							String dgOrderDetailID = object.getString("id");
							String type = object.getString("type");// 1订购2零售 3半价
							String xstj = object.getString("sftj");// 1限时特价
							String className = "pli1";
							String xstjLogo = "";
							String kcNum=object.getString("num");//库存数量 根据库存数量 按钮变成缺货 图片灰掉
							String btnTxt="购买";
							String imgStyle="";
							if(Integer.parseInt(kcNum)<=0)
							{
								btnTxt="售空";
								imgStyle="-webkit-filter: grayscale(100%);-moz-filter: grayscale(100%);-ms-filter: grayscale(100%); -o-filter: grayscale(100%);filter: grayscale(100%);filter: gray;";
							}
							if ("1".equals(xstj)) {
								className = "pli2";
								xstjLogo = "<img class=\"proBg\" src=\"./img/xstj.png\" alt=\"\">";
							} else {
								xstj = "0";
							}
							// 当type=3的时候dgOrderDetailID 会有值且不等于0
							String P = "无";
							if (!"".equals(price3) && price3 != null) {
								P = price3;
							}
							//半价转售都按照一样的价格显示
//							if ("3".equals(type)) {
//								// 此处是半价出售
//								price = (Double.parseDouble(object
//										.getString("price0")) / 2) + "";
//								price1 = price;
//							}
							price = String.format("%.2f",
									Double.parseDouble(price));
							if ("1".equals(priceSwich)) {
								String clickString="pickProduct(\""
										+ object.getString("productID")
										+ "\",\""
										+ mechineID
										+ "\",\""
										+ companyID
										+ "\",\""
										+ productName
										+ "\",\""
										+ price
										+ "\",\""
										+ path
										+ "\",\""
										+ dgOrderDetailID
										+ "\",\""
										+ type
										+ "\",\""
										+ String.format("%.2f",
												Double.parseDouble(price1))
										+ "\",\""
										+ String.format("%.2f",
												Double.parseDouble(price2))
										+ "\",\""
										+ String.format("%.2f",
												Double.parseDouble(price3))
										+ "\",\""
										+ priceSwich
										+ "\",\""
										+ xstj
										+ "\")";
								if(Integer.parseInt(kcNum)==0)
								{
									clickString="";
								}
								htmlString += " <li class=\""
										+ className
										+ "\" style=\"position: relative\" onclick='"+clickString+"'> "
										+ xstjLogo
										+ " <div class='listimg'>"
										+ "    	 <img style='"+imgStyle+"' src='"
										+ path
										+ "'>"
										+ " </div>"
										+ " <div class='listtext'>"
										+ "     <h4 style='font-size:0.8rem'>"
										+ object.getString("proName")
										+ "</h4>"
										+ "  <span>￥ "
										+ price
										+ "</span>"
										+ "<a style='font-size:0.9rem;font-weight:bold'>会员价￥"
										+ String.format("%.2f",
												Double.parseDouble(P)) + "</a>"
										+ "<div>"+btnTxt+"</div>" + " </div>" + "</li>";
							} else {
								String click="pickProduct(\""
										+ object.getString("productID")
										+ "\",\""
										+ mechineID
										+ "\",\""
										+ companyID
										+ "\",\""
										+ productName
										+ "\",\""
										+ price
										+ "\",\""
										+ path
										+ "\",\""
										+ dgOrderDetailID
										+ "\",\""
										+ type
										+ "\",\""
										+ String.format("%.2f",
												Double.parseDouble(price1))
										+ "\",\""
										+ String.format("%.2f",
												Double.parseDouble(price2))
										+ "\",\""
										+ String.format("%.2f",
												Double.parseDouble(price3))
										+ "\",\""
										+ priceSwich
										+ "\",\""
										+ xstj
										+ "\")";
								if(Integer.parseInt(kcNum)==0)
								{
									click="";
								}
								htmlString += " <li class=\""
										+ className
										+ "\" style=\"position: relative\" onclick='"+click+"'>"
										+ xstjLogo
										+ " <div class='listimg' >"
										+ "  <img style='"+imgStyle+"' src='"
										+ path
										+ "'><span class=\"proBg\">限时特价</span>"
										+ " </div>"
										+ " <div class='listtext'>"
										+ "<h4 style='font-size:0.8rem'>"
										+ object.getString("proName")
										+ "</h4>"
										+ "     <span>￥ "
										+ String.format("%.2f",
												Double.parseDouble(price))
										+ "</span>" + "<div>"+btnTxt+"</div>"
										+ " </div>" + "</li>";
							}
						}
					}
				} catch (JSONException e) {
					Log.i("9999", "e"+e.getMessage());
					e.printStackTrace();
				}
			}
			String hidString = "<input type=\"hidden\" id=\"hidTotalCount\" value=\""
					+ totalCount + "\"/>";
			return htmlString + hidString;
		}

		@JavascriptInterface
		public String videoPlay() {
			String videoInfo = Util
					.getSharePer(IndexActivity.this, "videoInfo");
			if (!"".equals(videoInfo) && videoInfo != null) {
				String json = "";
				try {
					JSONArray jsonArray = new JSONArray(videoInfo);
					for (int i = 0; i < jsonArray.length(); i++) {
						JSONObject object = jsonArray.getJSONObject(i);
						String name = object.getString("name");
						if (object.getString("type").equals("0")
								&& (name.contains(".jpg") || name
										.contains(".png")))// 横屏
						{
							// /mnt/internal_sd/asm/video/hvideo/20190510165514.jpg
							String path = Config.OPEN_HVIDEO_PATH
									+ object.getString("name");
							json += " <div class=\"item\"> <img class=\"img\" src=\""
									+ path + "\"></div>";

						}
					}
					return json.toString();
				} catch (Exception e) {

				}
			} else {
				return "";
			}
			return "";
		}

		@JavascriptInterface
		public String updateProductType() {
			// [{"productTypeID":22,"typeName":"酸奶","companyID":0},{"productTypeID":33,"typeName":"鲜牛奶","companyID":0}]
			String result = Util.getSharePer(IndexActivity.this, "productType");
			if ("".equals(result) || result == null) {
				return "";
			}
			String htmlString = "<a class=\"acolor\" href=\"#\" data-id='0'>全部</a>";
			try {
				JSONArray jsonArray = new JSONArray(result);
				for (int i = 0; i < jsonArray.length(); i++) {
					JSONObject object = jsonArray.getJSONObject(i);
					htmlString += "<a href=\"#\" data-id='"
							+ object.getString("productTypeID") + "'>"
							+ object.getString("typeName") + "</a>";
				}
			} catch (Exception e) {

			}
			return htmlString;
		}

		@JavascriptInterface
		public void uploadVideoRecord(String videoID) {
			JSONObject reqJson = new JSONObject();
			try {
				// [{"videoID":145,"time":"2019-02-26
				// 00:02:57","num":1,"mechineid":"37"},{"videoID":145,"time":"2019-02-26
				// 00:03:54","num":1,"mechineid":"37"}]
				reqJson.put(Config.CMD, Config.VIDEO);
				reqJson.put(Config.MECHINE_ID,
						Util.getSharePer(IndexActivity.this, "mechineID"));
				reqJson.put("videoID", videoID);
				reqJson.put("times", 1);
				reqJson.put("time", Util.getDate());
				reqJson.put("MsgId", "");
				reqJson.put("samtype", PortService.Getjiqigezhongxinxi());
				if (!videoStr.equals(reqJson.toString())
						&& !"0".equals(videoID)) {
					Util.sendMsg(reqJson.toString());
					videoStr = reqJson.toString();
				}
			} catch (JSONException e) {
				e.printStackTrace();
			}
		}

		@JavascriptInterface
		public String updatevPro() {
			String productInfo = Util.getSharePer(IndexActivity.this,
					"productInfo");
			String htmlString = "";
			if (productInfo != null && !"".equals(productInfo)) {
				try {
					String mechineID = Util.getSharePer(IndexActivity.this,
							"mechineID");
					String companyID = Util.getSharePer(IndexActivity.this,
							"companyID");
					JSONArray jsonArray = new JSONArray(productInfo);
					for (int i = 0; i < jsonArray.length(); i++) {
						JSONObject object = jsonArray.getJSONObject(i);
						String path = Environment.getExternalStorageDirectory()
								+ "/asm/image/"
								+ object.getString("path").replace("/img/", "");
						String productName = object.getString("proName");
						String price = object.getString("price3");
						String product_Type = object.getString("protype");
						String P = "无";
						if (!"".equals(price) && price != null) {
							P = price;
						}
						htmlString += "<li>" + "<div class='listimg'>"
								+ "	<img src='" + path + "' />" + "</div>"
								+ "<div class='listtext'>" + "	<h4>"
								+ productName + "</h4>" + "	<span>¥"
								+ object.getString("price0") + "</span>"
								+ "  <a>会员价￥" + P + "</a>" + "	<div>" + "	购买"
								+ "	</div>" + "</div>" + "</li>";
					}
				} catch (JSONException e) {
					e.printStackTrace();
				}
			}
			return htmlString;
		}

		// 播放竖屏视频
		@JavascriptInterface
		public String spvideoPlay() {
			String videoInfo = Util
					.getSharePer(IndexActivity.this, "videoInfo");
			if (!"".equals(videoInfo) && videoInfo != null) {
				String json = "[";
				try {
					JSONArray jsonArray = new JSONArray(videoInfo);
					for (int i = 0; i < jsonArray.length(); i++) {
						JSONObject object = jsonArray.getJSONObject(i);
						if (object.getString("type").equals("1"))// 横屏
						{
							String path = Config.OPEN_VVIDEO_PATH
									+ object.getString("name");
							json += "{\"id\":\"" + object.getString("videoID")
									+ "\",\"src\":\"" + path + "\"}" + ",";
						}
					}
					json = json.substring(0, json.length() - 1);
					json = json + "]";
					if (json.length() == 1) {
						return "";
					}
				} catch (Exception e) {

				}
				return json.toString();
			} else {
				return "";
			}
		}

		@JavascriptInterface
		public void updateIndex() {
			// soundPool.play(soundMap.get(2), 1, 1, 0, 0, 1);
			indexPage = 0;
		}
	}
	

	   
	@Override
	protected void onPause() {
		if (indexWebView != null) {
			indexWebView.reload();
		}
		super.onPause();
		 //用户不在当前页面时，停止监听
        mTelephonyManager.listen(mListener, PhoneStatListener.LISTEN_NONE);
	}

	/*
	 * 提示加载
	 */
	public void showProgressDialog(String title, String message) {
		if (progressDialog == null) {
			progressDialog = ProgressDialog.show(IndexActivity.this, title,
					message, true, false);
		} else if (progressDialog.isShowing()) {
			progressDialog.setTitle(title);
			progressDialog.setIcon(R.drawable.dialog_tip);
			progressDialog.setMessage(message);
		}
		progressDialog.show();
	};
	
	public static Handler chlchandler = new Handler() {
		// 在Handler中获取消息，重写handleMessage()方法
		@Override
		public void handleMessage(Message msg) {
			chlcTxt.setText(msg.obj.toString());
			Log.i("8888",msg.obj.toString());
		}
		
	};
	
	
	private Handler handler = new Handler() {
		// 在Handler中获取消息，重写handleMessage()方法
		@Override
		public void handleMessage(Message msg) {
			// 判断消息码是否为1
			if (msg.what == 1) {
				if (indexPage == 0 && indexWebView != null)// 代表首页
				{
					indexWebView.loadUrl("javascript:showScreen()");
					indexPage = 1;
					upLLRecord(getApplicationContext());// 从首页跳转到竖屏页面
				} else {
					lastClickDate = new Date();
				}
			} else if (msg.what == 2) {
				// soundPool.play(soundMap.get(1), 1, 1, 0, 0, 1);
			} else if (msg.what == REFRESH) {
				if(Config.lastsockettime != null){
					
					if((new Date().getTime() - Config.lastsockettime.getTime()) / 1000>10){
						Config.socketStatus =1;
					}else{
						Config.socketStatus =0;
					}
				}else{
					Config.socketStatus =1;
				}
				if(Config.lastgkjtime != null){
					
					if((new Date().getTime() - Config.lastgkjtime.getTime()) / 1000>10){
						Config.gkjStatus =1;
					}else{
						Config.gkjStatus =0;
					}
				}else{
					Config.gkjStatus =1;
				}
				final String mechineID = Util.getSharePer(getApplicationContext(),
						"mechineID");
				
				
				String str = "";
				if (Config.gkjStatus == 0) {
					str = "下位机：正常；";
					if (Config.socketStatus == 0) {
						str += " 服务端：正常";
					} else {
						str += " 服务端：异常";
					}
				} else {
					str = "下位机：异常;";
					if (Config.socketStatus == 0) {
						str += " 服务端：正常";
					} else {
						str += " 服务端：异常";
					}
				}
				leftTxt.setText(str);
				ActivityManager activityManager = (ActivityManager) getSystemService(ACTIVITY_SERVICE);
				// 最大分配内存
				int memory = activityManager.getMemoryClass();
				// 最大分配内存获取方法2
				float maxMemory = (float) (Runtime.getRuntime().maxMemory() * 1.0 / (1024 * 1024));
				// 当前分配的总内存
				float totalMemory = (float) (Runtime.getRuntime().totalMemory() * 1.0 / (1024 * 1024));
				// 剩余内存
				float freeMemory = (float) (Runtime.getRuntime().freeMemory() * 1.0 / (1024 * 1024));
				JSONObject reqJson = new JSONObject();
				try {
					reqJson.put(Config.CMD, "memory");
					reqJson.put(Config.MECHINE_ID, Util.getSharePer(
							getApplicationContext(), "mechineID"));
					reqJson.put("maxMemory", maxMemory);
					reqJson.put("totalMemory", totalMemory);
					reqJson.put("freeMemory", freeMemory);
					reqJson.put("MsgId", "");
					reqJson.put(Config.VERSION,
							getVerName(getApplicationContext()));
					reqJson.put("samtype", PortService.Getjiqigezhongxinxi());
					Util.sendMsg(reqJson.toString());
				} catch (JSONException e) {
					e.printStackTrace();
				}
			} else if (msg.what == 4) {
				if (indexWebView != null) {
					indexWebView.loadUrl(url);
				}

			}
		}
	};

	
	// 上传流量统计记录
	public static void upLLRecord(Context context) {
		JSONObject reqJson = new JSONObject();
		try {
			reqJson.put(Config.CMD, "record");
			reqJson.put("companyID", Util.getSharePer(context, "companyID"));
			reqJson.put(Config.MECHINE_ID,
					Util.getSharePer(context, "mechineID"));
			reqJson.put("indexCount", indexCount);
			reqJson.put("smCount", smCount);
			reqJson.put("productCount", productCount);
			reqJson.put("endCount", endCount);
			reqJson.put("billno", Config.billno);
			reqJson.put("type", Config.type);
			reqJson.put("memberID", Config.memberID);
			reqJson.put("MsgId", "");
			if ("".equals(productStr)) {
				reqJson.put("productStr", productStr);
			} else {
				reqJson.put("productStr",
						productStr.substring(0, productStr.length() - 1));
			}
			reqJson.put(Config.VERSION, getVerName(context));
			if (!"".equals(productStr)) {
				reqJson.put("samtype", PortService.Getjiqigezhongxinxi());
				Util.sendMsg(reqJson.toString());
			}
		} catch (JSONException e) {
			e.printStackTrace();
		}
		indexCount = 0;
		smCount = 0;
		productCount = 0;
		endCount = 0;
		productStr = "";
		// Config.billno = "";
	}

	private TimerTask taskTT = new TimerTask() {
		public void run() {
			try {
				SimpleDateFormat sfDateFormat = new SimpleDateFormat(
						"yyyy-MM-dd HH:mm:ss");
				Date nowDate = new Date();
				Log.i("2222", "最后点击事件=" + sfDateFormat.format(lastClickDate)
						+ "时间差="
						+ (nowDate.getTime() - lastClickDate.getTime()) / 1000
						+ ";index=" + indexPage);
				Log.i("4444", "indexCount=" + indexCount + ";smCount="
						+ smCount + ";productCount=" + productCount
						+ ";endCount=" + endCount + ";productStr=" + productStr);
				if ((nowDate.getTime() - lastClickDate.getTime()) / 1000 >= 120) {
					Message msg = Message.obtain();
					msg.what = 1;
					// 发送这个消息到消息队列中
					handler.sendMessage(msg);
				}
			} catch (Exception e) {
				e.printStackTrace();
			}
		}
	};
	private TimerTask taskUpLoadLocation = new TimerTask() {
		public void run() {
			try {
				Dao dao = new Dao();
				if (!"".equals(locationStr)) {
					dao.upLoadLocation(locationStr);
				}

			} catch (Exception e) {
				e.printStackTrace();
			}
		}
	};

	/*
	 * 隐藏提示加载
	 */
	public void hideProgressDialog() {
		if (progressDialog != null && progressDialog.isShowing()) {
			progressDialog.dismiss();
		}
	}

	public void destroyWebView() {
		if (indexWebView != null) {
			Log.i("7777", "221212");
			indexWebView.clearHistory();
			indexWebView.clearCache(true);
			indexWebView.loadUrl("about:blank");
			indexWebView.freeMemory();
			indexWebView.pauseTimers();
			indexWebView = null; // Note that mWebView.destroy() and mWebView =
									// null do the exact same thing
		}
	}

	@Override
	public void onConfigurationChanged(Configuration newConfig) {
		// TODO Auto-generated method stub
		super.onConfigurationChanged(newConfig);
	}

	@Override
	protected void onStart() {
		super.onStart();
	}

	@Override
	protected void onDestroy() {
		// if (null != _handler && null != _runnable) {
		// _handler.removeCallbacks(_runnable);
		// }
		// destroyWebView();
		if (myThread != null) {
			b = false;
		}
		super.onDestroy();// 不可去掉*****
	}

	@Override
	protected void onResume() {
		
		mTelephonyManager.listen(mListener, PhoneStatListener.LISTEN_SIGNAL_STRENGTHS);
		super.onResume();
	}

	@Override
	public boolean onKeyDown(int keyCode, KeyEvent event) {
		Log.i("4444", "点击快了返回=" + keyCode + "=" + KeyEvent.KEYCODE_BACK);
		if (keyCode == KeyEvent.KEYCODE_BACK) {
			android.os.Process.killProcess(android.os.Process.myPid());
		}
		return super.onKeyDown(keyCode, event);
	}

	@Override
	protected void onRestart() {
		// TODO Auto-generated method stub
		// _handler.postDelayed(_runnable, 5000);
		super.onRestart();

	}
	 
    @SuppressWarnings("deprecation")
    private class PhoneStatListener extends PhoneStateListener {
        //获取信号强度


        @Override
        public void onSignalStrengthChanged(int asu) {
            super.onSignalStrengthChanged(asu);
        }

        @Override
        public void onSignalStrengthsChanged(SignalStrength signalStrength) {
            super.onSignalStrengthsChanged(signalStrength);
            //获取网络信号强度
            //获取0-4的5种信号级别，越大信号越好,但是api23开始才能用
            //if (android.os.Build.VERSION.SDK_INT >= android.os.Build.VERSION_CODES.M) {
               // int level = signalStrength.getLevel();
                //System.out.println("level====" + level);
            //}
            int ltedbm=NetUtils.getMobileDbm(IndexActivity.this);
          
            int cdmaDbm = signalStrength.getCdmaDbm();
            int evdoDbm = signalStrength.getEvdoDbm();
            int gsmSignalStrength = signalStrength.getGsmSignalStrength();
            int dbm = -113 + 2 * gsmSignalStrength;

            //获取网络类型
            int netWorkType = NetUtils.getNetworkState(IndexActivity.this);
            Config.netWorkType=netWorkType;
            int netLevel=-1;
            switch (netWorkType) {
            
                case NetUtils.NETWORK_WIFI:
                	netLevel=NetUtils.getNetworkWifiLevel(IndexActivity.this);
                	mTextView.setText("当前网络为wifi,信号强度为：" + netLevel);
                	Config.netLevel=netLevel;
                    break;
                case NetUtils.NETWORK_2G:
                	netLevel=NetUtils.getLteLevel(gsmSignalStrength==99?ltedbm:gsmSignalStrength);
                    mTextView.setText("当前网络为2G移动网络,信号强度为：" + netLevel);
                    Config.netLevel=netLevel;
                    break;
                case NetUtils.NETWORK_3G:
                	netLevel=NetUtils.getLteLevel(gsmSignalStrength==99?ltedbm:gsmSignalStrength);
                    mTextView.setText("当前网络为3G移动网络,信号强度为：" + netLevel);
                    Config.netLevel=netLevel;
                    break;
                case NetUtils.NETWORK_4G:
                	
                	netLevel=NetUtils.getLteLevel(gsmSignalStrength==99?ltedbm:gsmSignalStrength);
                	
                    mTextView.setText("当前网络为4G移动网络,信号强度为：" + netLevel);
                    Config.netLevel=netLevel;
                    break;
                case NetUtils.NETWORK_NONE:
                	
                    mTextView.setText("当前没有网络!" );
                    Config.netLevel=-1;
                    break;
                case -1:
                    mTextView.setText("当前网络错误!" );
                    Config.netLevel=-1;
                    break;
            }
            Message.obtain(mHandler, LISTEN_SIGNAL_STRENGTHS, signalStrength).sendToTarget();
        }
        Handler mHandler = new Handler() {
        	public void handleMessage(Message msg) {
	        	switch (msg.what) {
	        	
	        		case LISTEN_SIGNAL_STRENGTHS:
	        			PhoneStatListener.this.onSignalStrengthsChanged((SignalStrength)msg.obj);
	        			break;
	        	}
        	}
    	};
    }
}
