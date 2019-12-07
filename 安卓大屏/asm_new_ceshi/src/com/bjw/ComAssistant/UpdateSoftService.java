package com.bjw.ComAssistant;

import android.app.Service;
import android.content.Context;
import android.content.Intent;
import android.location.LocationManager;
import android.os.Handler;
import android.os.IBinder;
import android.os.Message;
import android.os.Vibrator;
import android.util.Log;

import java.text.SimpleDateFormat;
import java.util.Date;

import org.json.JSONArray;
import org.json.JSONObject;

import com.baidu.location.LocationClient;
import com.baidu.location.LocationClientOption;
import com.sq.util.Config;
import com.sq.util.Util;

public class UpdateSoftService extends Service {

	private UpdateManager mUpdateManager;// 软件更新
	private Vibrator mVibrator01 = null;
	private LocationClient mLocClient;
	private LocationManager mLocationManager;
	private String mtv;

	/**
	 * 绑定服务时才会调用 必须要实现的方法
	 * 
	 * @param intent
	 * @return
	 */
	@Override
	public IBinder onBind(Intent intent) {
		return null;
	}

	/**
	 * 首次创建服务时，系统将调用此方法来执行一次性设置程序（在调用 onStartCommand() 或 onBind() 之前）。
	 * 如果服务已在运行，则不会调用此方法。该方法只被调用一次
	 */
	@Override
	public void onCreate() {
		try {
			mLocationManager = (LocationManager) getApplicationContext()
					.getSystemService(Context.LOCATION_SERVICE);
			mLocClient = ((Location) getApplication()).mLocationClient;
			((Location) getApplication()).mTv = mtv;
			mVibrator01 = (Vibrator) getApplication().getSystemService(
					Service.VIBRATOR_SERVICE);
			((Location) getApplication()).mVibrator01 = mVibrator01;
			setLocationOption();
			mLocClient.start();

//			Thread td1 = new Thread(updateRunnable, "updateRunnable");// 更新软件
//			td1.start();

			super.onCreate();
		} catch (Exception e) {
		}
	}

	public void showToast(String str) {
		Intent intent = new Intent();
		intent.putExtra("toast", "" + str);
		intent.setAction("com.tools.ui.toast");
		sendBroadcast(intent);
	}

	/**
	 * 每次通过startService()方法启动Service时都会被回调。
	 * 
	 * @param intent
	 * @param flags
	 * @param startId
	 * @return
	 */
	@Override
	public int onStartCommand(Intent intent, int flags, int startId) {
		mLocClient = ((Location) getApplication()).mLocationClient;
		((Location) getApplication()).mTv = mtv;
		mVibrator01 = (Vibrator) getApplication().getSystemService(
				Service.VIBRATOR_SERVICE);
		((Location) getApplication()).mVibrator01 = mVibrator01;
		setLocationOption();
		mLocClient.requestLocation();

		// mHanlder.sendEmptyMessage(1);

		return super.onStartCommand(intent, flags, startId);
	}

	/**
	 * 服务销毁时的回调
	 */
	@Override
	public void onDestroy() {
		super.onDestroy();
	}

	private Handler mHanlder = new Handler() {
		@Override
		public void handleMessage(Message msg) {
			switch (msg.what) {
			case 1:
				// app更新
				mUpdateManager = new UpdateManager(getApplicationContext());
				if (mUpdateManager.getServerVer(getApplicationContext())) {

				}
				break;

			default:
				break;
			}
			super.handleMessage(msg);
		}
	};

	private Runnable updateRunnable = new Runnable() {
		@Override
		public void run() {
			/**
			 * 此处执行任务 检测软件是否有更新
			 * */
			while (true) {
				try {
					Thread.sleep(1000 * 60);
					String mechineInfo = Util.getSharePer(getApplicationContext(), "mechineInfo");
					if (!"".equals(mechineInfo) && mechineInfo != null) {
						Log.i("3333", "mechineInfo="+mechineInfo);
						try {
							JSONArray jsonArray = new JSONArray(mechineInfo);
							JSONObject object = jsonArray.getJSONObject(0);
							String p2 = object.getString("p2");
							SimpleDateFormat sdfDateFormat = new SimpleDateFormat("HH:mm");
							String time = sdfDateFormat.format(new Date());
							if (time.equals(p2)) {
								mHanlder.sendEmptyMessage(1);
							}
						} catch (Exception e) {

						}
					}
				} catch (Exception e1) {
					e1.printStackTrace();
				}
			}
		}
	};

	// 设置相关参数
	private void setLocationOption() {
		LocationClientOption option = new LocationClientOption();
		option.setOpenGps(true); // 打开gps
		option.setCoorType("bd09ll"); // 设置坐标类型
		option.setServiceName("com.baidu.location.service_v2.9");
		option.setPoiExtraInfo(true);
		option.setAddrType("all");
		option.setScanSpan(3000);
		if (mLocationManager.isProviderEnabled(LocationManager.GPS_PROVIDER)) {
			option.setPriority(LocationClientOption.GpsFirst); // 设置网络优先
		} else {
			option.setPriority(LocationClientOption.NetWorkFirst); // 设置网络优先
		}
		option.setPoiNumber(10);
		option.disableCache(true);
		mLocClient.setLocOption(option);
	}

}
